import { Component, OnDestroy, OnInit } from '@angular/core';
import { BaseComponent } from './base-component';
import { ApplicationSearchComponent } from './application-search/application-search.component';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingService } from './routing.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'ui';

  actions: Record<string, string[]> = {
    'Create': ['Application', 'Pipeline', 'Pipeline Version'],
    'Manage': ['Application', 'Pipeline', 'Pipeline Version'],
    'Search': ['Application', 'Pipeline'],
  }

  // current actio
  currentActions = Object.keys(this.actions);

  resources: Record<string, string[]> = {}

  currentResources: string[] = [];

  query: string = '';

  selectedAction: string | null = null;
  selectedResource: string | null = null;

  routerSub?: Subscription = undefined;

  constructor(private router: Router, private routerService: RoutingService) {

    // reverse actions to get resources
    for (const action of Object.keys(this.actions))
    {
      for (const resource of this.actions[action])
      {
        if (this.resources[resource] === undefined) {
          this.resources[resource] = [];
        }

        this.resources[resource].push(action);
      }
    }

    this.currentResources = Object.keys(this.resources);
  }

  ngOnDestroy(): void {
    if (this.routerSub !== undefined) {
      this.routerSub.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.routerSub = this.routerService.routeState.subscribe(x => {
      this.selectedAction = x.action === '' ? null : x.action;
      this.selectedResource = x.resource === '' ? null : x.resource;
      this.query = '';

      this.updateOptions();
    })
  }

  isPillDisabled(text: string): boolean {
    if (this.query.trim() === '') {
      return false;
    }

    return text.toLowerCase().indexOf(this.query.toLowerCase()) === -1;
  }

  querySubmit() {
    if (this.selectedAction === null) {
      let matchingActions = this.currentActions.filter(x => x.toLowerCase().indexOf(this.query.toLowerCase()) !== -1);

      if (matchingActions.length > 0) {
        this.selectAction(matchingActions[0]);
        return;
      }
    }

    if (this.selectedResource === null) {
      let matchingResources = this.currentResources.filter(x => x.toLowerCase().indexOf(this.query.toLowerCase()) !== -1);

      if (matchingResources.length > 0) {
        this.selectResource(matchingResources[0]);
        return;
      }
    }

    if (this.selectedAction !== null && this.selectedResource !== null) {
      // TODO: Throw the query value to the component

    }
  }

  selectAction(action: string) {
    this.routerService.setAction(action);
  }

  unSelectAction() {
    this.routerService.setAction();
  }

  selectResource(resource: string) {
    this.routerService.setResource(resource);
  }

  unSelectResource() {
    this.routerService.setResource();
  }

  updateOptions() {
    if (this.selectedAction !== null && this.selectedResource !== null) {
      // clear them out
      this.currentActions = [];
      this.currentResources = [];

      return;
    }

    if (this.selectedAction !== null) {
      this.currentActions = [];
      this.currentResources = this.actions[this.selectedAction];

      return;
    }

    if (this.selectedResource !== null) {
      this.currentResources = [];
      this.currentActions = this.resources[this.selectedResource];

      return;
    }

    // If nothing is selected, reset everything;
    this.currentActions = Object.keys(this.actions);
    this.currentResources = Object.keys(this.resources);
  }

  getUrlPart(text: string): string {
    return text.replace(' ', '_');
  }
}
