import { Injectable } from '@nestjs/common';
import {
  Pipeline,
  PipelineFlow,
  PipelineInput,
  PipelineModel,
  PipelineVariable,
  PipelineVersionModel,
} from '../input-models.interface';
import { DataService } from '../data/data.service';
import { v4 as uuidv4 } from 'uuid';
import { parse } from 'yaml';
import { exec } from 'child_process';

@Injectable()
export class PipelineService {
  constructor(private dataService: DataService) {}
  async create(body: PipelineInput): Promise<void> {
    const pool = this.dataService.getPool();

    const res = await pool.query('insert into public.pipeline (id, name, organization) values ($2, $1, null);', [
      body.name,
      uuidv4(),
    ]);

    console.log(res.rows);
  }

  /**
   * get a list of orgs
   * TODO: actually implement org param
   * @param org organization to search against
   */
  async get(org: string): Promise<PipelineModel[]> {
    const pool = this.dataService.getPool();

    const res = await pool.query('SELECT * FROM public.pipeline');

    return res.rows;
  }

  /**
   * Basically does a version upsert for now based on version info
   * @param body the body lol
   */
  async createVersion(body: PipelineVersionModel): Promise<void> {
    // It's more than slightly annoying that we have to convert.
    const code = parse(body.code) as Pipeline;

    console.log(JSON.stringify(code, null, '  '));

    const variables = code.variables || [];

    const pool = this.dataService.getPool();

    const checkRes = await pool.query('SELECT * FROM public.pipeline_version WHERE pipeline = $1 AND name = $2', [
      body.pipeline,
      body.name,
    ]);

    if (checkRes.rows.length === 0) {
      await pool.query(
        'INSERT INTO public.pipeline_version (id, name, pipeline, variables, code) VALUES ($1, $2, $3, $4, $5)',
        [uuidv4(), body.name, body.pipeline, { variables }, body.code],
      );
    } else {
      const id = checkRes.rows[0].id;

      await pool.query(
        'UPDATE public.pipeline_version SET name = $2, pipeline = $3, variables = $4, code = $5 WHERE id = $1',
        [id, body.name, body.pipeline, { variables }, body.code],
      );
    }

    return;
  }

  /**
   * @deprecated for early testing only
   * @param id the version id
   */
  async runVersion(id: string, variables: Record<string, any> = {}): Promise<void> {
    const pool = this.dataService.getPool();

    const checkRes = await pool.query('SELECT * FROM public.pipeline_version WHERE id = $1', [id]);

    const version = checkRes.rows[0];

    const code = parse(version.code) as Pipeline;

    await this.runFlow(code, [...code.flow]);
  }

  private async runFlow(code: Pipeline, pipelineFlows: PipelineFlow[], parent: string = null) {
    const replacements: Record<string, string> = {};

    for (const variable of code.variables) {
      replacements[`variables.${variable.name}`] = 'john';
    }

    // Basically if parent isn't null look for it, if it is find empties.
    const matches = pipelineFlows.filter(
      (flow) => (parent !== null && flow.depends_on.indexOf(parent) !== -1) || flow.depends_on.length === 0,
    );

    const rest = pipelineFlows.filter((flow) => matches.indexOf(flow) === -1);

    for (const match of matches) {
      const localReplacements = { ...replacements };
      const localKeys = Object.keys(match.locals);

      for (const local of localKeys) {
        localReplacements[`locals.${local}`] = this.processTemplate(replacements, match.locals[local]);
      }

      const step = code.steps.filter((s) => s.name === match.step)[0];

      console.log(localReplacements);

      for (const action of step.actions) {
        const processedCommand = this.processTemplate(localReplacements, action.command);
        exec(processedCommand, (err, stdout, stderr) => {
          if (err !== null) {
            console.log(err);
          }

          console.log(stdout);
        });
      }
    }
  }

  private processTemplate(replacements: Record<string, string>, template: string): string {
    const replacementKeys = Object.keys(replacements);

    for (const key of replacementKeys) {
      template = template.replace(`\$\{${key}\}`, replacements[key]);
    }

    return template;
  }
}
