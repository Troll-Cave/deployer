import { Injectable } from '@nestjs/common';
import { PipelineInput, PipelineModel, PipelineVersionModel } from '../input-models.interface';
import { DataService } from '../data/data.service';
import { v4 as uuidv4 } from 'uuid';

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
    const pool = this.dataService.getPool();

    const checkRes = await pool.query('SELECT * FROM public.pipeline_version WHERE pipeline = $1 AND name = $2', [
      body.pipeline,
      body.name,
    ]);

    if (checkRes.rows.length === 0) {
      await pool.query(
        'INSERT INTO public.pipeline_version (id, name, pipeline, variables, code) VALUES ($1, $2, $3, $4, $5)',
        [uuidv4(), body.name, body.pipeline, null, body.code],
      );
    } else {
      const id = checkRes.rows[0].id;

      await pool.query(
        'UPDATE public.pipeline_version SET name = $2, pipeline = $3, variables = $4, code = $5 WHERE id = $1',
        [id, body.name, body.pipeline, null, body.code],
      );
    }

    return;
  }
}
