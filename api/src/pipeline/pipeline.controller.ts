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

  /**
   * This method will be totally changed, it's just here now for testing
   * @deprecated putting this in now.
   */
  @Post('run')
  async runVersion(): Promise<void> {
    const version = 'e5627c0f-a65f-4b23-9ae6-fe3cdf770be8';

    await this.pipelineService.runVersion(version);

    return;
  }
}
