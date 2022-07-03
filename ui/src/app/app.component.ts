import { Component } from '@angular/core';
import { BaseComponent } from './base-component';
import { ApplicationSearchComponent } from './application-search/application-search.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
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

  currentComponent: BaseComponent = new ApplicationSearchComponent();

  constructor(private router: Router) {
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
    this.selectedAction = action;
    this.updateOptions();
    this.query = '';
    return;
  }

  unSelectAction() {
    this.selectedAction = null;
    this.updateOptions();
    this.query = '';
    return;
  }

  selectResource(resource: string) {
    this.selectedResource = resource;
    this.updateOptions();
    this.query = '';
    return;
  }

  unSelectResource() {
    this.selectedResource = null;
    this.updateOptions();
    this.query = '';
    return;
  }

  updateOptions() {
    // console.log(this.router.getCurrentNavigation()?.finalUrl?.fragment);

    if (this.selectedAction !== null && this.selectedResource !== null) {
      // clear them out
      this.currentActions = [];
      this.currentResources = [];

      this.router.navigateByUrl(this.getUrl())
        .then();

      return;
    }

    if (this.selectedAction !== null) {
      this.currentActions = [];
      this.currentResources = this.actions[this.selectedAction];

      this.router.navigateByUrl('/home')
        .then();

      return;
    }

    if (this.selectedResource !== null) {
      this.currentResources = [];
      this.currentActions = this.resources[this.selectedResource];

      this.router.navigateByUrl('/home')
        .then();

      return;
    }

    // If nothing is selected, reset everything;
    this.currentActions = Object.keys(this.actions);
    this.currentResources = Object.keys(this.resources);

    this.router.navigateByUrl('/home')
      .then();
  }

  getUrl(): string {
    return `/${this.getUrlPart(this.selectedAction as string)}/${this.getUrlPart(this.selectedResource as string)}`;
  }

  getUrlPart(text: string): string {
    return text.toLowerCase().replace(' ', '_');
  }
}
