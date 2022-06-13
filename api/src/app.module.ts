import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { PipelineController } from './pipeline/pipeline.controller';
import { PipelineService } from './pipeline/pipeline.service';
import { DataService } from './data/data.service';
import { GithubService } from './github/github.service';
import { OrgService } from './org/org.service';
import { AppService } from './app/app.service';
import { OrgController } from './org/org.controller';
import { AppController } from './app/app.controller';

@Module({
  imports: [],
  controllers: [AppController, PipelineController, OrgController],
  providers: [AppService, PipelineService, DataService, GithubService, OrgService],
})
export class AppModule {}
