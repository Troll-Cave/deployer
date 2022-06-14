import { Injectable } from '@nestjs/common';
import { AppInput } from '../../input-models.interface';
import { DataService } from '../data/data.service';
import { v4 as uuidv4 } from 'uuid';

@Injectable()
export class AppService {
  constructor(private dataService: DataService) {}

  async create(body: AppInput): Promise<void> {
    const pool = this.dataService.getPool();

    await pool.query('INSERT INTO public.application (id, name, org) VALUES ($1, $2, $3)', [
      uuidv4(),
      body.name,
      body.org,
    ]);
  }

  async update(body: AppInput): Promise<void> {
    const pool = this.dataService.getPool();

    await pool.query('UPDATE public.application SET name = $2, org = $3, variables = $4, source = $5 WHERE id = $1', [
      body.id,
      body.name,
      body.org,
      body.variables,
      body.source
    ]);
  }
}
