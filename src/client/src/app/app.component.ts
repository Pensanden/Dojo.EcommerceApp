import { Component, OnInit } from '@angular/core';
import { IProduct } from './shared/models/product';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Sports Equipment Store App';
  
  constructor() {  }

  ngOnInit(): void {
  }
}
