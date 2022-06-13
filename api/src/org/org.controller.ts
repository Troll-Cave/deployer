import { Body, Controller, Get, Post, Put } from '@nestjs/common';
import { OrgService } from './org.service';
import { OrgModel } from '../../input-models.interface';

@Controller('org')
export class OrgController {
  constructor(private orgService: OrgService) {}

  @Get()
  public async get(): Promise<string> {
    return 'test';
  }

  @Post()
  async create(@Body() body: OrgModel): Promise<void> {
    await this.orgService.create(body);
    return;
  }

  @Put()
  async update(): Promise<void> {
    //
  }
}
