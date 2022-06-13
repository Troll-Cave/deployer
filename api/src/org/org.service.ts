import { Injectable } from '@nestjs/common';
import { DataService } from '../data/data.service';
import { v4 as uuidv4 } from 'uuid';
import { OrgModel } from '../../input-models.interface';

@Injectable()
export class OrgService {
  constructor(private dataService: DataService) {}

  async create(body: OrgModel): Promise<void> {
    const pool = this.dataService.getPool();

    await pool.query('INSERT INTO public.organization (id, name) VALUES ($1, $2)', [uuidv4(), body.name]);
  }
}
