import { Component, OnInit } from "@angular/core";
import { saveAs } from "file-saver";
import { DigitalSignatureService, TiposDeFirma, XmlModel } from "./digital-signature.service";

@Component({
  selector: "lib-digital-signature",
  templateUrl: "./digital-signature.component.html",
  styleUrls: ["./digital-signature.component.scss"],
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
    this.signatureService.firmaDigital(this.objeto, this.TipoDeFirma).subscribe(
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
    this.signatureService.firmaElectronica(this.objeto, this.TipoDeFirma).subscribe(
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
