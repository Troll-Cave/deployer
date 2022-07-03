import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-application-search',
  templateUrl: './application-search.component.html',
  styleUrls: ['./application-search.component.scss']
})
export class ApplicationSearchComponent implements OnInit, BaseComponent {

  constructor() { }

  ngOnInit(): void {
  }

}
