import { Body, Controller, Get, Post, Query } from '@nestjs/common';
import { PipelineInput, PipelineVersionModel } from '../input-models.interface';
import { PipelineService } from './pipeline.service';

@Controller('pipeline')
export class PipelineController {
  constructor(private pipelineService: PipelineService) {}

  @Post()
  async create(@Body() body: PipelineInput): Promise<string> {
    await this.pipelineService.create(body);
    return body.name;
  }

  @Get()
  async get(@Query('org') org: string): Promise<any> {
    return await this.pipelineService.get(org);
  }

  @Post('version')
  async createVersion(@Body() body: PipelineVersionModel): Promise<void> {
    await this.pipelineService.createVersion(body);
  }
}
