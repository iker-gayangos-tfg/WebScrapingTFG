import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InvestigadoresComponent } from './investigadores.component';


const ROUTES: Routes = [
  {
    path: '',
    component: InvestigadoresComponent
  }
];

// const ROUTES: Routes = [
//   {
//     path: '',
//     redirectTo: '/investigadores',
//     pathMatch: 'full'
//   },
//   {
//     path: '',
//     children: [
//       {
//         path: '',
//         component: InvestigadoresComponent
//       },
//     ]
//   }
// ];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class InvestigadoresRoutingModule { }
