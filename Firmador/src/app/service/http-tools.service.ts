import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpToolsService {

  private static _router: Router;
  private static _token: any;

  constructor(private router: Router,
              ) {
    HttpToolsService._router = this.router;
  }

  // puesto que los envíos requieren siempre la misma configuración
  setHeaders() {
    const headers = new HttpHeaders().set('Content-Type', 'application/json')
    .set('Access-Control-Allow-Origin', '*')
    .set('Access-Control-Allow-Credentials', 'true')
    return {headers: headers};
  }

  // para extraer los datos json de la respuesta http
  getData(response) {
    // TODO: validar el satusCode y controlar vacíos
    return response.json().data;
  }
}
