import { Injectable } from '@nestjs/common';
import { PipelineInput, PipelineModel } from '../input-models.interface';
import { Client } from 'pg';
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
}
