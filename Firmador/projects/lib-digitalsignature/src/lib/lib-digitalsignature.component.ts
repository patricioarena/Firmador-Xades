import { Component, OnInit } from "@angular/core";
import { saveAs } from "file-saver";
import { DigitalSignatureService, TiposDeFirma, XmlModel } from "./lib-digitalsignature.service";

@Component({
  selector: "lib-digitalsignature",
  template: `<section class="ss-d ss-style-triangles-digitalsignature">
  <div>
    <button type="button" class="btn-digitalsignature btn-success-digitalsignature" (click)="downloadFile()" [disabled]="!isEnabled">Descargar Documento
      Firmado&nbsp;
      <i class="fa fa-download fa-lg" aria-hidden="true"></i></button></div>
</section>

<div class="container-digitalsignature">
  <div class="form-group-digitalsignature">
    <textarea class="form-control-digitalsignature" id="Textarea1" rows="13" style="resize: none;" [(ngModel)]="text"></textarea>
  </div>
  <select class="select-digitalsignature" (change)="changeType($event)">
    <option [ngValue]="2">Xades-BES - Sin ds:Object</option>
    <option [ngValue]="1">Xades-BES - Con ds:Object</option>
  </select>
</div>

<br>

<div class="container-digitalsignature">
  <div class="text-center-digitalsignature">
    <button type="button" class="btn-digitalsignature btn-primary-digitalsignature waves-effect-digitalsignature waves-light" (click)='FirmarDigital()'><i
        class="fa fa-pencil fa-fw" aria-hidden="true"></i>&nbsp;Firmar Digital</button>
    <button type="button" class="btn-digitalsignature btn-default-digitalsignature waves-effect-digitalsignature waves-light" (click)='FirmarElectronica()'><i
        class="fa fa-pencil fa-fw" aria-hidden="true"></i>&nbsp;Firmar Electronica</button>
    <button type="button" class="btn-digitalsignature btn-danger-digitalsignature waves-effect-digitalsignature waves-light" (click)='preview()' [disabled]="!isEnabled"><i
        class="fa fa-eye fa-fw" aria-hidden="true"></i>&nbsp;Vista Previa</button>
    <button type="button" class="btn-digitalsignature btn-warning-digitalsignature waves-effect-digitalsignature waves-light" (click)="Verificar()"><i
        class="fa fa-search fa-fw" aria-hidden="true"></i>&nbsp;Verificar Firmas</button>
  </div>
</div>

<br>

<div class="container-digitalsignature">
  <div class="form-group-digitalsignature">

    <button type="button" class="btn-digitalsignature btn-grey-digitalsignature copy waves-effect-digitalsignature waves-light" [ngxClipboard]="textArea2"
      (click)="copyMessage()" [disabled]="!showPreview">
      <i class="fa fa-clipboard" aria-hidden="true"></i>&nbsp;Copiar
    </button>

    <button type="button" class="btn-digitalsignature btn-grey-digitalsignature download waves-effect-digitalsignature waves-light" (click)="downloadFile()"
      [disabled]="!showPreview"><i class="fa fa-download" aria-hidden="true">
      </i>&nbsp;Guardar
    </button>

    <textarea class="form-control-digitalsignature textPreview-digitalsignature" id="textArea2" #textArea2 rows="13" style="resize: none;"
      [(ngModel)]="textPreview" readonly></textarea>
  </div>


</div>
`,
  styles: [`:root {
    -btn-primary-digitalsignature: #4285f4;
    -btn-default-digitalsignature: #2bbbad;
    -btn-secondary-digitalsignature: #a6c;
    -btn-success-digitalsignature: #00c851;
    -btn-info-digitalsignature: #33b5e5;
    -btn-warning-digitalsignature: #fb3;
    -btn-danger-digitalsignature: #ff3547;
    -btn-grey-digitalsignature: #8dacc5;
  }

  body {
    font-family: 'Roboto', sans-serif;
  }

  .null {
    margin: 0;
  }

  .select-digitalsignature{
    margin: 0;
    font-family: inherit;
    font-size: inherit;
    line-height: inherit;
    text-transform: none;
    word-wrap: normal;
  }

  .text-center-digitalsignature {
    text-align: center !important;
  }

  .btn-digitalsignature {
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

  .btn-digitalsignature:focus, .btn-digitalsignature:hover {
    text-decoration: none;
  }

  .btn-digitalsignature:active, .btn-digitalsignature:focus, .btn-digitalsignature:hover {
    -webkit-box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
    box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
    outline: 0;
  }

  .btn-digitalsignature:not(:disabled):not(.disabled) {
    cursor: pointer;
  }

  .waves-effect-digitalsignature {
    position: relative;
    cursor: pointer;
    overflow: hidden;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    -webkit-tap-highlight-color: transparent;
  }


  .btn-primary-digitalsignature:hover {
    color: #fff;
    background-color: #0069d9;
    border-color: #0062cc;
  }

  .btn-primary-digitalsignature:not([disabled]):not(.disabled).active, .btn-primary:not([disabled]):not(.disabled):active, .show>.btn-primary.dropdown-toggle {
    -webkit-box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
    box-shadow: 0 5px 11px 0 rgba(0,0,0,.18), 0 4px 15px 0 rgba(0,0,0,.15);
    background-color: #0b51c5!important;
  }

  .btn-digitalsignature:disabled, .btn-digitalsignature.disabled,
  fieldset:disabled .btn {
    pointer-events: none;
    opacity: 0.4;
  }

  .btn-default-digitalsignature {
    background-color: #2bbbad!important;
    color: #fff;
  }

  .btn-primary-digitalsignature {
    background-color: #4285f4!important;
    color: rgb(255, 255, 255);
  }

  .btn-secondary-digitalsignature {
    background-color: #a6c!important;
    color: #fff;
  }

  .btn-success-digitalsignature {
    background-color: #00c851!important;
    color: #fff;
  }

  .btn-info-digitalsignature {
    background-color: #33b5e5!important;
    color: #fff;
  }

  .btn-warning-digitalsignature {
    background-color: #fb3!important;
    color: #fff;
  }

  .btn-danger-digitalsignature {
    background-color: #ff3547!important;
    color: #fff;
  }

  .btn-grey-digitalsignature {
    background-color: #8dacc5!important;
    color: #fff;
    width:auto;
    height:auto;
    padding:5px 5px;
    text-align:center;
  }

  .copy-digitalsignature {
    position: relative;;
    float: right;
    top: 3.2em;
  }

  .download-digitalsignature {
    position: relative;;
    float: right;
    top: 3.2em;
  }

  .textPreview-digitalsignature{
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
  .ss-style-triangles-digitalsignature::before,
  .ss-style-triangles-digitalsignature::after {
    left: 50%;
    width: 30px;
    height: 30px;
    -webkit-transform: translateX(-50%) rotate(45deg);
    transform: translateX(-50%) rotate(45deg);
  }

  .ss-style-triangles-digitalsignature::before {
    top: -15px;
    background: #ecf0f5;
  }

 .container-digitalsignature {
  width: 100%;
  padding-right: 1rem;
  padding-left: 1rem;
  margin-right: auto;
  margin-left: auto;
}

@media (min-width: 576px) {
  .container-digitalsignature {
    max-width: 540px;
  }
}

@media (min-width: 768px) {
  .container-digitalsignature {
    max-width: 720px;
  }
}

@media (min-width: 992px) {
  .container-digitalsignature {
    max-width: 960px;
  }
}

@media (min-width: 1200px) {
  .container-digitalsignature {
    max-width: 1140px;
  }
}

@media (min-width: 1400px) {
  .container-digitalsignature {
    max-width: 1320px;
  }
}


.form-control-digitalsignature {
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
  .form-control-digitalsignature {
    transition: none;
  }
}

.form-control-digitalsignature:focus {
  color: #495057;
  background-color: #fff;
  border-color: #8bbafe;
  outline: 0;
  box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
}

.form-control-digitalsignature::-webkit-input-placeholder {
  color: #6c757d;
  opacity: 1;
}

.form-control-digitalsignature::-moz-placeholder {
  color: #6c757d;
  opacity: 1;
}

.form-control-digitalsignature::-ms-input-placeholder {
  color: #6c757d;
  opacity: 1;
}

.form-control-digitalsignature::placeholder {
  color: #6c757d;
  opacity: 1;
}

.form-control-digitalsignature:disabled, .form-control-digitalsignature[readonly] {
  background-color: #e9ecef;
  opacity: 1;
}


  `],
})

export class DigitalSignatureComponent implements OnInit {

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
    public signatureService: DigitalSignatureService,
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
    this.signatureService.firmaDigital(this.objeto, this.TipoDeFirma, true).subscribe(
      (resp) => {
        // tslint:disable-next-line: no-empty
        if (resp === "-1") {
        } else if (resp === "-2") {
          // this.notificationService.showInfo("Certificado", "Certificado no valido");
          alert("Certificado: Certificado no valido");
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, (err) => {
        const message = JSON.parse(err.error).ExceptionMessage;
        // this.notificationService.showError("Certificado", message);
        alert("Certificado: " + message);
      });
  }

  public FirmarElectronica() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = "";
    this.objeto = new XmlModel();
    this.objeto.Archivo = this.text;
    this.signatureService.firmaElectronica(this.objeto, this.TipoDeFirma, true).subscribe(
      (resp) => {
        // tslint:disable-next-line: no-empty
        if (resp === "-1") {
        } else if (resp === "-2") {
          // this.notificationService.showInfo("Certificado", "Certificado no valido");
          alert("Certificado: Certificado no valido");
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, (err) => {
        const message = JSON.parse(err.error).ExceptionMessage;
        // this.notificationService.showInfo("Certificado", message);
        alert("Certificado: " + message);
      });
  }

  public ping() {
    this.signatureService.ping().subscribe(
      (resp) => {
        const blob = resp.body;
        const reader = new FileReader();
        reader.onload = () => {
          console.log(reader.result); // Aquí se mostrará el contenido en la consola
        };
        reader.readAsText(blob);
      }, 
      (err) => {
        const message = err.error.ExceptionMessage;
        console.log(message);
      }
    );
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
          // tslint:disable-next-line: max-line-length
          // this.notificationService.showVerify("Verificación de firmas satisfactoria", `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`, data);
          alert("Cantidad de firmas validas: " + firmasValidas);
        } else {
          // tslint:disable-next-line: max-line-length
          // this.notificationService.showNoVerify("Verificación de firmas no satisfactoria", `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`, data);
          alert("Cantidad de firmas validas: " + firmasValidas + " Cantidad de firmas invalidas: " + firmasInvalidas);
        }
      }, (err) => {
        const message = err.error.ExceptionMessage;
        // this.notificationService.showError("Firmas", message);
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
    // console.log(`event.target.value:: ${typeof (key)} :: ${key} :: ${value}`);
  }

  public copyMessage() {
    // this.notificationService.showInfo("", "Documento copiado");
    alert("Documento copiado");
  }

}
