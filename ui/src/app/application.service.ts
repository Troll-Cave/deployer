import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Application } from './models';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {

  constructor(private httpClient: HttpClient) { }

  search(): Observable<Application[]> {
    return this.httpClient.get<Application[]>('http://localhost:5251/Application');
  }
}
