import { Component } from '@angular/core';

@Component({
  selector: 'a-content-layout',
  templateUrl: './content-layout.component.html',
  styleUrls: ['./content-layout.component.scss']
})
export class ContentLayoutComponent {

  sections: any[] = [];
  
  constructor(
  ) {
    this.sections = [
      { name: "Investigadores", path: "/investigadores"},
      { name: "Departamentos", path: "/departamentos"},
      { name: "Facultades", path: "/facultades"}
    ]
  }

}
