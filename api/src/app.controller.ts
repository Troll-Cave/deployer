import { Controller, Get } from '@nestjs/common';
import { AppService } from './app.service';
import { GithubService } from './github/github.service';

@Controller()
export class AppController {
  constructor(private readonly appService: AppService, private readonly githubService: GithubService) {}

  @Get()
  async getHello(): Promise<string> {
    await this.githubService.downloadZip('Troll-Cave', 'story');
    return this.appService.getHello();
  }
}
