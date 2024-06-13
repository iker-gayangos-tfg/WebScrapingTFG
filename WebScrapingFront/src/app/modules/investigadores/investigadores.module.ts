import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { InvestigadoresComponent } from './investigadores.component';
import { InvestigadoresRoutingModule } from './investigadores-routing.module';
import { MaterialModule } from '../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { BindInvestigatorComponent } from './bind-investigator/bind-investigator.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { InvestigatorViewComponent } from './investigator-view/investigator-view.component';
import { PublicationViewComponent } from './publication-view/publication-view.component';

@NgModule({
  declarations: [
    InvestigadoresComponent,
    BindInvestigatorComponent,
    InvestigatorViewComponent,
    PublicationViewComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatPaginatorModule,
    InvestigadoresRoutingModule
  ],
  providers: [
  ]
})
export class InvestigadoresModule { }
