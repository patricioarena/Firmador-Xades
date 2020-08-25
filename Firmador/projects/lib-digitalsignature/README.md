 <h1>  Do not download, this library works with an external program, I do not publish</h1>

----

<p align="center">
  <img height="200px" width="200px" style="text-align: center;" src="https://angular.io/assets/images/logos/angular/angular.svg">
  <h1 align="center">Lib-DigitalSignature</h1>
</p>

[![Support](https://img.shields.io/badge/Support-Angular%209%2B-blue.svg?style=flat-square)]()
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)]()
[![devDependency Status](https://img.shields.io/david/expressjs/express.svg?style=flat-square)]()

This library was generated with [Angular CLI](https://github.com/angular/angular-cli) version 9.1.12.

Use appropriate version based on your Angular version.

|  Angular 9  |
| ----------- |
| >=`v9.1.12` |


## Browser Support

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>IE / Edge | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari-ios/safari-ios_48x48.png" alt="iOS Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Latest ✔                                                                                                                                                                                                      | Latest ✔                                                                                                                                                                                                          | IE11, Edge ✔                                                                                                                                                                                                    | Latest ✔                                                                                                                                                                                                                  | Latest ✔                                                                                                                                                                                                  |

## Requirements for quick use

| dependencia | version | link |
|----------|-------|--------|
| file-saver | 2.0.2 | [npm :link:](https://www.npmjs.com/package/file-saver/) |
| ngx-clipboard | 12.3.0| [npm :link:](https://www.npmjs.com/package/ngx-clipboard/) |

## Installation

`lib-digitalsignature` is available via [npm](https://www.npmjs.com/package/lib-digitalsignature) and [yarn](https://yarnpkg.com/package/lib-digitalsignature)

Using npm:

```bash
$ npm install lib-digitalsignature --save
```

Using yarn:

```bash
$ yarn add lib-digitalsignature
```

Using angular-cli:

```bash
$ ng add lib-digitalsignature
```

## Basic Usage :arrow_right: Add to your project

Import `lib-digitalsignature`  and `HttpClientModule ` in the root module(`AppModule`):

```typescript
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
// Import library module
import { DigitalSignatureModule } from "lib-digitalsignature";
import { HttpClientModule } from "@angular/common/http";

@NgModule({
  imports: [
    // ...
    HttpClientModule,
    DigitalSignatureModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
```

Add `DigitalSignatureModule` service wherever you want to use the `lib-digitalsignature`.

```typescript
import { DigitalSignatureModule } from "lib-digitalsignature";
import { saveAs } from "file-saver";
```

Now use in your template

```html
<lib-digitalsignature></lib-digitalsignature>
```

## Advanced Usage - Service :arrow_right: Add to your project

Import `lib-digitalsignature` , `ClipboardModule`, `FormsModule` and  `HttpClientModule ` in the root module(`AppModule`):

```typescript
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
// Import library module
import { DigitalSignatureModule } from "lib-digitalsignature";
import { HttpClientModule } from "@angular/common/http";
import { ClipboardModule } from "ngx-clipboard";
import { FormsModule } from "@angular/forms";

@NgModule({
  imports: [
    // ...
    DigitalSignatureModule,
    HttpClientModule,
    ClipboardModule,
    FormsModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
```

Add `DigitalSignatureModule` service wherever you want to use the `lib-digitalsignature`.

```typescript
import { DigitalSignatureModule, DigitalSignatureService, TiposDeFirma, XmlModel } from "lib-digitalsignature";
import { saveAs } from "file-saver";
```

## Available service methods

```typescript

  verificar(objeto: XmlModel, tipoFirma: TiposDeFirma)

  firmaDigital(objeto: XmlModel, tipoFirma: TiposDeFirma)

  firmaElectronica(objeto: XmlModel, tipoFirma: TiposDeFirma)

  isAlive():

```

## Service usage example

<details>
  <summary>In your component.ts</summary>

```typescript
import { Component, OnInit } from '@angular/core';
import { DigitalSignatureModule, DigitalSignatureService, TiposDeFirma, XmlModel } from "lib-digitalsignature";
import { saveAs } from "file-saver";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

// Advanced Usage - Service
export class AppComponent implements OnInit {

  public TiposDeFirma: any = [
    { key: "Xades-BES - Sin ds:Object", value: TiposDeFirma.Xades_BES_Sin_ds_Object },
    { key: "Xades-BES - Con ds:Object", value: TiposDeFirma.Xades_BES_Con_ds_Object },
  ];
  public TipoDeFirma = TiposDeFirma.Xades_BES_Sin_ds_Object;
  public text: String;
  public textPreview: String = "";
  public objeto: XmlModel;
  public isEnabled = false;
  public showPreview = false;
  public fileUrl;
  public responseFirma;

  constructor(
    public signatureService: DigitalSignatureService
  ) { }

  public ngOnInit() {
    this.text = '<?xml version="1.0" encoding="UTF-8" standalone="no" ?>\n' +
      "<Tour>\n" +
      "   <NombreTour> The Offspring y Bad Religion </NombreTour> \n" +
      "   <Fecha> 24/10/2019 19:00:00 </Fecha>\n" +
      "   <Videos>\n" +
      // tslint:disable-next-line: max-line-length
      '       <video nombre="Bad Religion - 21st century digital boy - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=iDVeAAvFb3U</video> \n' +
      '       <video nombre="The Offspring - Americana - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=Zd7bAu7hVZQ</video> \n' +
      "   </Videos>\n" +
      "</Tour>";
  }

  public FirmarDigital() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = "";
    this.objeto = new XmlModel();
    this.objeto.Archivo = this.text;
    this.signatureService.firmaDigital(this.objeto, this.TipoDeFirma).subscribe(
      (resp) => {
        // tslint:disable-next-line: no-empty
        if (resp === "-1") {
        } else if (resp === "-2") {
          alert("Certificado: Certificado no valido");
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, (err) => {
        const message = JSON.parse(err.error).ExceptionMessage;
        alert("Certificado: " + message);
      });
  }

  public FirmarElectronica() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = "";
    this.objeto = new XmlModel();
    this.objeto.Archivo = this.text;
    this.signatureService.firmaElectronica(this.objeto, this.TipoDeFirma).subscribe(
      (resp) => {
        // tslint:disable-next-line: no-empty
        if (resp === "-1") {
        } else if (resp === "-2") {
          alert("Certificado: Certificado no valido");
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, (err) => {
        const message = JSON.parse(err.error).ExceptionMessage;
        alert("Certificado: " + message);
      });
  }

  public Verificar() {
    this.objeto = new XmlModel();
    this.objeto.Archivo = this.text;
    this.signatureService.verificar(this.objeto, this.TipoDeFirma).subscribe(
      (resp) => {

        const data = JSON.parse(JSON.stringify(resp.data));
        const cantTotalDeFirmas = data.length;
        let firmasValidas = 0;
        let firmasInvalidas = 0;

        data.forEach((element) => {
          if (data[data.indexOf(element)].IsValid === true) {
            firmasValidas = firmasValidas + 1;
          }
        });

        firmasInvalidas = cantTotalDeFirmas - firmasValidas;

        if (cantTotalDeFirmas === firmasValidas) {
          alert("Cantidad de firmas validas: " + firmasValidas);
        } else {
          alert("Cantidad de firmas validas: " + firmasValidas + " Cantidad de firmas invalidas: " + firmasInvalidas);
        }
      }, (err) => {
        const message = err.error.ExceptionMessage;
        alert("Firmas: " + message);
      });
  }

  public downloadFile() {
    const blob = new Blob([this.responseFirma], { type: "text/xml; charset=utf-8" });
    saveAs(blob, "Document.xml");
  }

  public preview() {
    this.showPreview = true;
    this.textPreview = this.responseFirma;
  }

  public changeType(event) {
    const key = event.target.value;
    const value = this.TiposDeFirma.find((item) => item.key === key).value;
    this.TipoDeFirma = value;
  }

  public copyMessage() {
    alert("Documento copiado");
  }

}

```
</details>

<details>
  <summary>In your component.html</summary>

```html
<section class="ss-d ss-style-triangles">
  <div>
    <button type="button" class="btn btn-success" (click)="downloadFile()" [disabled]="!isEnabled">Descargar Documento
      Firmado&nbsp;
      <i class="fa fa-download fa-lg" aria-hidden="true"></i></button></div>
</section>

<div class="container">
  <div class="form-group">
    <textarea class="form-control" id="Textarea1" rows="13" style="resize: none;" [(ngModel)]="text"></textarea>
  </div>
  <select class="custom-select" (change)="changeType($event)">
    <option *ngFor="let aType of TiposDeFirma" [ngValue]="aType">{{aType.key}}</option>
  </select>
</div>


<br>

<div class="container">
  <div class="text-center">
    <button type="button" class="btn btn-primary waves-effect waves-light" (click)='FirmarDigital()'><i
        class="fa fa-pencil fa-fw" aria-hidden="true"></i>&nbsp;Firmar Digital</button>
    <button type="button" class="btn btn-default waves-effect waves-light" (click)='FirmarElectronica()'><i
        class="fa fa-pencil fa-fw" aria-hidden="true"></i>&nbsp;Firmar Electronica</button>
    <button type="button" class="btn btn-danger waves-effect waves-light" (click)='preview()' [disabled]="!isEnabled"><i
        class="fa fa-eye fa-fw" aria-hidden="true"></i>&nbsp;Vista Previa</button>
    <button type="button" class="btn btn-warning waves-effect waves-light" (click)="Verificar()"><i
        class="fa fa-search fa-fw" aria-hidden="true"></i>&nbsp;Verificar Firmas</button>
  </div>
</div>

<br>

<div *ngIf="showPreview">
  <div class="container">
    <div class="form-group">
      <button type="button" class="btn btn-grey copy waves-effect waves-light" [ngxClipboard]="textArea2"
        (click)="copyMessage()"><i class="fa fa-clipboard" aria-hidden="true"></i>&nbsp;Copiar</button>

      <button type="button" class="btn btn-grey download waves-effect waves-light" (click)="downloadFile()"><i
          class="fa fa-download" aria-hidden="true"></i>&nbsp;Guardar</button>

      <textarea class="form-control textPreview" id="textArea2" #textArea2 rows="13" style="resize: none;"
        [(ngModel)]="textPreview" readonly></textarea>
    </div>

  </div>

</div>

```
</details>

<details>
  <summary>In your component.scss</summary>

```scss
  :root {
  -btn-primary: #4285f4;
  -btn-default: #2bbbad;
  -btn-secondary: #a6c;
  -btn-success: #00c851;
  -btn-info: #33b5e5;
  -btn-warning: #fb3;
  -btn-danger: #ff3547;
  -btn-grey: #8dacc5;
}

body {
  font-family: 'Roboto', sans-serif;
}

.null {
  margin: 0;
}

.select{
  margin: 0;
  font-family: inherit;
  font-size: inherit;
  line-height: inherit;
  text-transform: none;
  word-wrap: normal;
}

.text-center {
  text-align: center !important;
}

.btn {
  -webkit-box-shadow: 0 2px 5px 0 rgba(0,0,0,.16), 0 2px 10px 0 rgba(0,0,0,.12);
  box-shadow: 0 2px 5px 0 rgba(0,0,0,.16), 0 2px 10px 0 rgba(0,0,0,.12);
  padding: .84rem 2.14rem;
  font-size: .81rem;
  -webkit-transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,-webkit-box-shadow .15s ease-in-out;
  transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,-webkit-box-shadow .15s ease-in-out;
  -o-transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
  transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
  transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out,-webkit-box-shadow .15s ease-in-out;
  margin: .375rem;
  border: 0;
  -webkit-border-radius: .125rem;
  border-radius: .125rem;
  cursor: pointer;
  text-transform: uppercase;
  white-space: normal;
  word-wrap: break-word;
  color: #fff;
}

.btn:focus, .btn:hover {
  text-decoration: none;
}

.btn:active, .btn:focus, .btn:hover {
  -webkit-box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
  box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
  outline: 0;
}

.btn:not(:disabled):not(.disabled) {
  cursor: pointer;
}

.waves-effect {
  position: relative;
  cursor: pointer;
  overflow: hidden;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
  -webkit-tap-highlight-color: transparent;
}


.btn-primary:hover {
  color: #fff;
  background-color: #0069d9;
  border-color: #0062cc;
}

.btn-primary:not([disabled]):not(.disabled).active, .btn-primary:not([disabled]):not(.disabled):active, .show>.btn-primary.dropdown-toggle {
  -webkit-box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
  box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
  background-color: #0b51c5!important;
}

.btn:disabled, .btn.disabled,
fieldset:disabled .btn {
  pointer-events: none;
  opacity: 0.4;
}

.btn-default {
  background-color: #2bbbad!important;
  color: #fff;
}

.btn-primary {
  background-color: #4285f4!important;
  color: rgb(255, 255, 255);
}

.btn-secondary {
  background-color: #a6c!important;
  color: #fff;
}

.btn-success {
  background-color: #00c851!important;
  color: #fff;
}

.btn-info {
  background-color: #33b5e5!important;
  color: #fff;
}

.btn-warning {
  background-color: #fb3!important;
  color: #fff;
}

.btn-danger {
  background-color: #ff3547!important;
  color: #fff;
}

.btn-grey {
  background-color: #8dacc5!important;
  color: #fff;
  width:auto;
  height:auto;
  padding:5px 5px;
  text-align:center;
}

.copy {
  position: relative;;
  float: right;
  top: 3.2em;
}

.download {
  position: relative;;
  float: right;
  top: 3.2em;
}

.textPreview{
  font-size: 12px;
  font-family: monospace;
  white-space: pre;
}


.ss-d {
  position: relative;
  background-color: rgba(155, 168, 174, 0.3);
  padding-top:20px;
  padding-bottom:20px;
  margin-top:15px;
  margin-bottom:80px;
  text-align: center;
  height: 96px;
}

/* Common style for pseudo-elements */
.ss-d::before,
.ss-d::after {
  position: absolute;
  content: '';
  pointer-events: none;
}

/* Triangles */
.ss-style-triangles::before,
.ss-style-triangles::after {
  left: 50%;
  width: 30px;
  height: 30px;
  -webkit-transform: translateX(-50%) rotate(45deg);
  transform: translateX(-50%) rotate(45deg);
}

.ss-style-triangles::before {
  top: -15px;
  background: #ecf0f5;
}

.container {
width: 100%;
padding-right: 1rem;
padding-left: 1rem;
margin-right: auto;
margin-left: auto;
}

@media (min-width: 576px) {
.container {
  max-width: 540px;
}
}

@media (min-width: 768px) {
.container {
  max-width: 720px;
}
}

@media (min-width: 992px) {
.container {
  max-width: 960px;
}
}

@media (min-width: 1200px) {
.container {
  max-width: 1140px;
}
}

@media (min-width: 1400px) {
.container {
  max-width: 1320px;
}
}


.form-control {
display: block;
width: 100%;
min-height: calc(1.5em + 0.75rem + 2px);
padding: 0.375rem 0.75rem;
font-size: 1rem;
font-weight: 400;
line-height: 1.5;
color: #495057;
background-color: #fff;
background-clip: padding-box;
border: 1px solid #ced4da;
-webkit-appearance: none;
-moz-appearance: none;
appearance: none;
border-radius: 0.25rem;
transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}

@media (prefers-reduced-motion: reduce) {
.form-control {
  transition: none;
}
}

.form-control:focus {
color: #495057;
background-color: #fff;
border-color: #8bbafe;
outline: 0;
box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
}

.form-control::-webkit-input-placeholder {
color: #6c757d;
opacity: 1;
}

.form-control::-moz-placeholder {
color: #6c757d;
opacity: 1;
}

.form-control::-ms-input-placeholder {
color: #6c757d;
opacity: 1;
}

.form-control::placeholder {
color: #6c757d;
opacity: 1;
}

.form-control:disabled, .form-control[readonly] {
background-color: #e9ecef;
opacity: 1;
}

  ```
</details>

<details>
  <summary>In your index.html</summary>

```html
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
```
</details>

## Project example

If you want to see an example, it is recommended to [download](https://github.com/patricioarena/Lib-DigitalSignature) the project, compile the library and run with `ng serve`

## Build the library
Download the source code from github.
Run `ng build lib-digitalsignature --prod` to build the project. The build artifacts will be stored in the `dist/` directory.


## Creator

#### [Patricio E. Arena](mailto:patricio.e.arena@gmail.com)

- [@GitHub](https://github.com/patricioarena)

## Future Plan

- Smaller bundle

## License

lib-digitalsignature is [MIT licensed](./LICENSE).