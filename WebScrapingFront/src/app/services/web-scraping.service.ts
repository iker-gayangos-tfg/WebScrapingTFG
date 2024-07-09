import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebScrapingService {

  private apiUrl = 'http://localhost:5000/api';

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
    return this.httpClient.put(`${this.apiUrl}/Investigadores/BindInvestigators`, data);
  }

  getPublicacionesInvestigador(data: any) :Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Investigadores/GetPublicacionesInvestigador`, {
      params: data
    });
  }

  getIndicadoresPublicacion(data: any) :Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Investigadores/GetIndicadoresPublicacion`, {
      params: data
    });
  }

  getPublicacionesInvestigadorCompleto(data: any) {
    return this.httpClient.get<any>(`${this.apiUrl}/Investigadores/GetPublicacionesInvestigadorCompleto`, {
      params: data
    });
  }

  scraping() {
    return this.httpClient.get<any>(`${this.apiUrl}/WebScraping/Scraping`);
  }
  
}
