import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebScrapingService {

  private apiUrl = 'https://localhost:44326/api';

  constructor(private httpClient: HttpClient) { }

  getDepartamentos() :Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Departamentos/GetDepartamentos`);
  }

  getInvestigadores(data: any) :Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Investigadores/GetInvestigadoresData`, {
      params: data
    });
  }

  getInvestigadoresList() :Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Investigadores/GetInvestigadoresList`);
  }

  bindInvestigators(data: any) {
    return this.httpClient.post(`${this.apiUrl}/Investigadores/BindInvestigators`, data);
  }
}
