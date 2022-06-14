import { Body, Controller, Post, Put } from '@nestjs/common';
import { AppInput } from '../../input-models.interface';
import { AppService } from './app.service';

@Controller('app')
export class AppController {
  constructor(private appService: AppService) {}

  @Post()
  async create(@Body() body: AppInput) {
    await this.appService.create(body);
  }

  @Put()
  async update(@Body() body: AppInput) {
    await this.appService.update(body);
  }
}
