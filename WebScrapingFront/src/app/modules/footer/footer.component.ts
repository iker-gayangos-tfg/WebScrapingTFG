import { Component, OnInit } from '@angular/core';
import moment from 'moment';

@Component({
  selector: 'a-ui-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  year: string = '';

  constructor(
  ) { }

  ngOnInit(): void {
    this.year = moment().format('YYYY');
  }

}