import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { PipelineController } from './pipeline/pipeline.controller';
import { PipelineService } from './pipeline/pipeline.service';
import { DataService } from './data/data.service';
import { GithubService } from './github/github.service';

@Module({
  imports: [],
  controllers: [AppController, PipelineController],
  providers: [AppService, PipelineService, DataService, GithubService],
})
export class AppModule {}
