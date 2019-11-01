import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { HttpToolsService } from './http-tools.service';
import { Objeto } from '../modelos/Objeto';
import { promise } from 'protractor';

@Injectable({
  providedIn: 'root'
})
export class DigitalSignatureService {

  apiurl = environment.API_URL;

  // tslint:disable-next-line: no-shadowed-variable
  constructor(private HttpClient: HttpClient, private HttpToolsService: HttpToolsService) { }

  private extractData(res) {
    const body = res.data;
    return body || {};
  }

   verificar(objeto, tipoFirma) {
    const options = this.HttpToolsService.setHeaders();
    const url = `${this.apiurl}api/Signature/Verify/${tipoFirma}`;
    return this.HttpClient.post( url, objeto, options).pipe(map(res => res as any));
  }

  firma(objeto, tipoFirma) {
    const url = `${this.apiurl}api/Signature/Signature/${tipoFirma}`;
    return this.HttpClient.post( url, objeto, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .append('Access-Control-Allow-Origin', '*'),
      responseType: 'text'
    }).pipe(map(res => res as any));
  }


}
