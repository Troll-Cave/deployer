import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ApplicationSearchComponent } from './application-search/application-search.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'Search/Application', component: ApplicationSearchComponent },
  { path: '**', redirectTo: '/home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
