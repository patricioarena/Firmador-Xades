import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";

export enum TiposDeFirma {
  Xades_BES_Con_ds_Object = 1,
  Xades_BES_Sin_ds_Object = 2,
}

export class XmlModel {
  // tslint:disable-next-line: ban-types
  public Archivo: String;
  // tslint:disable-next-line: ban-types
  public Extension: String;

  constructor(objeto?: any) {
    this.Archivo = objeto && objeto.Archivo || "";
    this.Extension = objeto && objeto.Extension || ".xml";
  }
}

// tslint:disable-next-line: max-classes-per-file
@Injectable({
  providedIn: "root",
})

export class DigitalSignatureService {

  private _apiurl = "https://localhost:8400/";

  public get apiurl() {
    return this._apiurl;
  }

  // tslint:disable-next-line: no-shadowed-variable
  constructor(private HttpClient: HttpClient) { }

  public verificar(objeto: XmlModel, tipoFirma: TiposDeFirma) {
    const url = `${this.apiurl}api/Signature/Verify/${tipoFirma}`;
    return this.HttpClient.post(url, objeto, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
    }).pipe(
      map((res) => res as any),
    );
  }

  public firmaDigital(objeto: XmlModel, tipoFirma: TiposDeFirma) {
    const url = `${this.apiurl}api/Signature/Digital/Signature/${tipoFirma}`;
    return this.HttpClient.post(url, objeto, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
      responseType: "text",
    }).pipe(
      map((res) => res as any),
    );
  }

  public firmaElectronica(objeto: XmlModel, tipoFirma: TiposDeFirma) {
    const url = `${this.apiurl}api/Signature/Electronic/Signature/${tipoFirma}`;
    return this.HttpClient.post(url, objeto, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
      responseType: "text",
    }).pipe(
      map((res) => res as any),
    );
  }

  public isAlive() {
    const url = `${this.apiurl}api/Signature/isAlive`;
    return this.HttpClient.get(url, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
      responseType: "blob"
      // tslint:disable-next-line: object-literal-sort-keys
      , observe: "response",
    }).pipe(map((res) => res as any));
  }

}
