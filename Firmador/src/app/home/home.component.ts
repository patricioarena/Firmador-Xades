import { Component, OnInit } from '@angular/core';
import { DigitalSignatureService, TiposDeFirma, XmlModel } from '../service/digital-signature.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { NotificationService } from '../service/notification.service';
import { TitleService } from '../service/title.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {
  title: String;
  TiposDeFirma: any = [
    { 'key': 'Xades-BES - Sin ds:Object', 'value': TiposDeFirma.Xades_BES_Sin_ds_Object },
    { 'key': 'Xades-BES - Con ds:Object', 'value': TiposDeFirma.Xades_BES_Con_ds_Object }
  ];
  TipoDeFirma = TiposDeFirma.Xades_BES_Sin_ds_Object;
  text: String;
  textPreview: String = '';
  objeto: XmlModel;
  isEnabled = false;
  showPreview = false;
  fileUrl;
  responseFirma;

  constructor(
    public signatureService: DigitalSignatureService,
    public spinner: NgxSpinnerService,
    private notificationService: NotificationService,
    private titleServive: TitleService
  ) { }

  ngOnInit() {
    this.title = this.titleServive.APP_TITLE;
    this.text = '<?xml version="1.0" encoding="UTF-8" standalone="no" ?>\n' +
      '<Tour>\n' +
      '   <NombreTour> The Offspring y Bad Religion </NombreTour> \n' +
      '   <Fecha> 24/10/2019 19:00:00 </Fecha>\n' +
      '   <Videos>\n' +
      // tslint:disable-next-line: max-line-length
      '       <video nombre="Bad Religion - 21st century digital boy - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=iDVeAAvFb3U</video> \n' +
      '       <video nombre="The Offspring - Americana - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=Zd7bAu7hVZQ</video> \n' +
      '   </Videos>\n' +
      '</Tour>';
  }

  FirmarDigital() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = '';
    this.objeto = new XmlModel;
    this.objeto.Archivo = this.text;
    this.signatureService.firmaDigital(this.objeto, this.TipoDeFirma).subscribe(
      resp => {
        if (resp === '-1') {
        } else if (resp === '-2') {
          this.notificationService.showInfo('Certificado', 'Certificado no valido');
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, err => {
        const message = JSON.parse(err.error).ExceptionMessage;
        this.notificationService.showError('Certificado', message);
      });
  }

  FirmarElectronica() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = '';
    this.objeto = new XmlModel;
    this.objeto.Archivo = this.text;
    this.signatureService.firmaElectronica(this.objeto, this.TipoDeFirma).subscribe(
      resp => {
        if (resp === '-1') {
        } else if (resp === '-2') {
          this.notificationService.showInfo('Certificado', 'Certificado no valido');
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, err => {
        const message = JSON.parse(err.error).ExceptionMessage;
        this.notificationService.showInfo('Certificado', message);
      });
  }

  Verificar() {
    this.objeto = new XmlModel;
    this.objeto.Archivo = this.text;
    this.signatureService.verificar(this.objeto, this.TipoDeFirma).subscribe(
      resp => {

        const data = JSON.parse(JSON.stringify(resp.data));
        const cantTotalDeFirmas = data.length;
        let firmasValidas = 0;
        let firmasInvalidas = 0;

        data.forEach(element => {
          if (data[data.indexOf(element)].IsValid === true) {
            firmasValidas = firmasValidas + 1;
          }
        });

        firmasInvalidas = cantTotalDeFirmas - firmasValidas;

        if (cantTotalDeFirmas === firmasValidas) {
          // tslint:disable-next-line: max-line-length
          this.notificationService.showVerify('Verificación de firmas satisfactoria', `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`, data);
        } else {
          // tslint:disable-next-line: max-line-length
          this.notificationService.showNoVerify('Verificación de firmas no satisfactoria', `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`, data);
        }
      }, err => {
        const message = err.error.ExceptionMessage;
        this.notificationService.showError('Firmas', message);
      });
  }

  downloadFile() {
    const blob = new Blob([this.responseFirma], { type: 'text/xml; charset=utf-8' });
    saveAs(blob, 'Document.xml');
  }

  preview() {
    this.showPreview = true;
    this.textPreview = this.responseFirma;
  }

  changeType(event) {
    const key = event.target.value;
    const value = this.TiposDeFirma.find(function (item) { return item.key === key; }).value;
    this.TipoDeFirma = value;
  }

  copyMessage() {
    this.notificationService.showInfo('', 'Documento copiado');
  }

}

