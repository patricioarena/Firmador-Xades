import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { HttpToolsService } from './service/http-tools.service';
import { Firma } from './modelos/propietario';

var httpHeaders = new HttpHeaders();

const httpOptions = {
  headers: httpHeaders
};

@Injectable({
  providedIn: 'root'
})
export class DigitalSignatureService {
  
  apiurl = environment.API_URL;

  constructor(private HttpClient: HttpClient, private HttpToolsService: HttpToolsService) { }

  private extractData(res) {
    let body = res.data;
    return body || {};
  }

  Verificar(value, scope) {
    return this.HttpClient.post(this.apiurl + 'api/Signature/Verify', {
      "value": value,
      "scope": scope
    }, httpOptions).toPromise()
      .then(this.extractData)
      .catch();
  }


  firmar(firma): Observable<Firma> {
    const options = this.HttpToolsService.setHeaders();
    const url = `${this.apiurl}api/Signature/Signature`;
    return this.HttpClient
      .post<Firma>(url, firma, options)
      .pipe(
        map(res => res as any)
      );
  }

  // Firmar(value) {
  //   return this.HttpClient.post(this.apiurl + 'api/Signature/Signature', {
  //     "userName": value,
  //   }, httpOptions).toPromise()
  //     .then(this.extractData)
  //     .catch();
  // }



}
