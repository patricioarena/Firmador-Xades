import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";

export enum TiposDeFirma {
  Xades_BES_Con_ds_Object = 1,
  Xades_BES_Sin_ds_Object = 2,
}

export class XmlModel {
  // tslint:disable-next-line: ban-types
  public XmlFile: String;
  // tslint:disable-next-line: ban-types
  public Extension: String;

  constructor(objeto?: any) {
    this.XmlFile = (objeto && objeto.Archivo) || "";
    this.Extension = (objeto && objeto.Extension) || ".xml";
  }
}

// tslint:disable-next-line: max-classes-per-file
@Injectable({
  providedIn: "root",
})
export class DigitalSignatureService {
  private _apiurl = "https://localhost:8400/";

  public versionAuthentica: string = "3.0.7";

  public get apiurl() {
    return this._apiurl;
  }

  // tslint:disable-next-line: no-shadowed-variable
  constructor(private HttpClient: HttpClient) {}

  public verificar(objeto: XmlModel) {
    const url = `${this.apiurl}api/Signature/Verify/Exist/One/Or/More/Signatures`;
    return this.HttpClient.post(url, objeto, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
    }).pipe(map((res) => res as any));
  }

  public firmaDigital(objeto: XmlModel, ocsp: boolean) {
    const url = `${this.apiurl}api/Signature/Single/oscp/${ocsp}`;
    return this.HttpClient.post(url, objeto, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
      responseType: "text",
    }).pipe(map((res) => res as any));
  }

  public firmaElectronica(objeto: XmlModel, ocsp: boolean) {
    const url = `${this.apiurl}api/Signature/Single/oscp/${ocsp}`;
    return this.HttpClient.post(url, objeto, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
      responseType: "text",
    }).pipe(map((res) => res as any));
  }

  public ping() {
    const url = `${this.apiurl}api/Signature/ping`;
    return this.HttpClient.get(url, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
      responseType: "blob",
      observe: "response",
    }).pipe(map((res) => res));
  }

  public versionSignature() {
    const url = `${this.apiurl}api/Signature/version`;
    return this.HttpClient.get(url, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
    }).pipe(
      map((res) => {
        return res as any;
      })
    );
  }

  public pdfSignature() {
    const url = `${this.apiurl}api/Signature/PDF/Signature`;
    return this.HttpClient.get(url, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
      responseType: "blob",
      // tslint:disable-next-line: object-literal-sort-keys
      observe: "response",
    }).pipe(map((res) => res as any));
  }

  public pdfSign64(model: any) {
    const url = `${this.apiurl}api/Signature/PDF/Signature/Sign/Base64`;
    // Hacer la petición POST
    return this.HttpClient.post(url, model, {
      headers: new HttpHeaders()
        .set("Content-Type", "application/json")
        .append("Access-Control-Allow-Origin", "*"),
    }).pipe(
      map((res) => {
        console.log(res);
        return res;
      })
    );
  }
}
