import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoutingService {

  private _routeState: RouteState = {
    action: '',
    resource: '',
    query: ''
  }

  routeState: BehaviorSubject<RouteState> = new BehaviorSubject<RouteState>(this.getInitialState())

  constructor(private router: Router) {

  }

  getInitialState(): RouteState {
    const routeState: RouteState = {
      action: '',
      resource: '',
      query: ''
    };

    const routeParts = window.location.pathname
      .split('/').filter(x => x !== '');

    if (routeParts.length > 0 && routeParts[0] !== 'home')
    {
      routeState.action = routeParts[0].replace('_', ' ');

      if (routeParts.length > 1) {
        routeState.resource = routeParts[1].replace('_', ' ');
      }
    }

    this._routeState = routeState;
    return routeState;
  }

  setAction(action: string = '') {
    this._routeState.action = action;
    this.sendState();
    this.resetNavigation();
  }

  setResource(resource: string = '') {
    this._routeState.resource = resource;
    this.sendState();
    this.resetNavigation();
  }

  setQuery(query: string = '') {
    this._routeState.query = query;
    this.sendState();
  }

  setRouteData(data: RouteState) {
    this._routeState = data;
    this.sendState();
    this.resetNavigation();
  }

  sendState() {
    this.routeState.next(this._routeState);
  }

  resetNavigation() {
    if (this._routeState.action !== '' && this._routeState.resource !== '') {
      this.router.navigateByUrl(
        `/${this._routeState.action.replace(' ', '_')}/${this._routeState.resource.replace(' ', '_')}`
      ).then();
    }
    else {
      this.router.navigateByUrl('/home').then();
    }
  }
}

export interface RouteState {
  action: string;
  resource: string;
  query: string;
}
