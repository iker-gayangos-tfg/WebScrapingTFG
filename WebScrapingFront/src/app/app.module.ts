import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { ContentLayoutComponent } from './modules/content-layout/content-layout.component';
import { MaterialModule } from './modules/material/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
const ROUTES: Routes = [
  // {
  //   path: '',
  //   redirectTo: '/investigadores',
  //   pathMatch: 'full'
  // },
  {
    path: '',
    component: ContentLayoutComponent,
    children: [
      {
        path: 'investigadores',
        loadChildren: () => import('./modules/investigadores/investigadores.module').then(m => m.InvestigadoresModule)
      },
      {
        path: 'departamentos',
        loadChildren: () => import('./modules/departamentos/departamentos.module').then(m => m.DepartamentosModule)
      },
      {
        path: 'facultades',
        loadChildren: () => import('./modules/facultades/facultades.module').then(m => m.FacultadesModule)
      },

    ]
  },
  // { path: '**', redirectTo: '/investigadores', pathMatch: 'full' }
];

@NgModule({
  declarations: [
    AppComponent,
    ContentLayoutComponent,
  ],  
  imports: [
    BrowserModule,
    HttpClientModule,
    MaterialModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(ROUTES),
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }