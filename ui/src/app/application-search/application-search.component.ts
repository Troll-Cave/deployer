import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base-component';
import { ApplicationService } from '../application.service';
import { Application } from '../models';
import { Router } from '@angular/router';
import { RoutingService } from '../routing.service';

@Component({
  selector: 'app-application-search',
  templateUrl: './application-search.component.html',
  styleUrls: ['./application-search.component.scss']
})
export class ApplicationSearchComponent implements OnInit, BaseComponent {

  applications: Application[] = [];

  constructor(private applicationService: ApplicationService, private router: Router,
              private routingService: RoutingService) { }

  ngOnInit(): void {
    this.applicationService.search()
      .subscribe(x => this.applications = x);
  }

  manageApp(id: string) {
    this.routingService.setRouteData({
      action: 'Manage',
      resource: 'Application',
      query: id
    })
  }

}
