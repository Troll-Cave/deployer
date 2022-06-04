import { Injectable } from '@nestjs/common';
import { Pool } from 'pg';

@Injectable()
export class DataService {
  private pool: Pool = new Pool();

  getPool(): Pool {
    return this.pool;
  }
}
