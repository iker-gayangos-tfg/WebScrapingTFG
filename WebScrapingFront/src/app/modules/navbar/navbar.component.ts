import { Component, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'a-ui-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  @Output() sideNavToggle = new EventEmitter<void>();
  @Output() closeDialog = new EventEmitter<void>();



  constructor(

  ) {

  }

  ngOnInit(): void {

  }


  onToggleSidenav() {
    this.sideNavToggle.emit();
  }

}
