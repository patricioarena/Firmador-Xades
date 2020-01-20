import { Component, OnInit, Input } from '@angular/core';
import { DigitalSignatureService } from '../service/digital-signature.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Objeto } from 'src/app/modelos/Objeto';
import { saveAs } from 'file-saver';
import { resolve } from 'path';
import { error } from 'util';
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
    { 'key': 'Xades CIFE - Sin ds:Object', 'value': '2' },
    { 'key': 'Xades Cartagena (Union Europea) - Con ds:Object', 'value': '1' }
  ];
  TipoDeFirma: String = '2';
  text: String;
  textPreview: String = '';
  objeto: Objeto;
  isEnabled = false;
  showPreview = false;
  fileUrl;
  responseFirma;
  // signatureInDocument;

  constructor(
    public searchDocumentService: DigitalSignatureService,
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
      '       <video nombre="Bad Religion - 21st century digital boy - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=iDVeAAvFb3U</video> \n' +
      '       <video nombre="The Offspring - Americana - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=Zd7bAu7hVZQ</video> \n' +
      '   </Videos>\n' +
      '</Tour>';
  }

  FirmarDigital() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = '';
    // console.log("TextArea::text: " + this.text);
    this.objeto = new Objeto;
    this.objeto.Archivo = this.text;
    // this.objeto.Extension = '.xml';
    this.searchDocumentService.firmaDigital(this.objeto, this.TipoDeFirma).subscribe(
      resp => {
        if (resp === '-1') {
          // this.notificationService.show('Certificado', 'Certificado no valido', 'info');
        } else if (resp === '-2') {
          this.notificationService.showInfo('Certificado', 'Certificado no valido');
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, err => {
        // console.log(JSON.parse(err.error).ExceptionMessage)
        const message = JSON.parse(err.error).ExceptionMessage;
        this.notificationService.showError('Certificado', message);
      });
  }

  FirmarElectronica() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = '';
    // console.log("TextArea::text: " + this.text);
    this.objeto = new Objeto;
    this.objeto.Archivo = this.text;
    // this.objeto.Extension = '.xml';
    this.searchDocumentService.firmaElectronica(this.objeto, this.TipoDeFirma).subscribe(
      resp => {
        if (resp === '-1') {
          // this.notificationService.show('Certificado', 'Certificado no valido', 'info');
        } else if (resp === '-2') {
          this.notificationService.showInfo('Certificado', 'Certificado no valido');
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      }, err => {
        // console.log(JSON.parse(err.error).ExceptionMessage)
        const message = JSON.parse(err.error).ExceptionMessage;
        this.notificationService.showInfo('Certificado', message);
      });
  }

  Verificar() {
    this.objeto = new Objeto;
    this.objeto.Archivo = this.text;
    // this.objeto.Extension = '.xml';
    this.searchDocumentService.verificar(this.objeto, this.TipoDeFirma).subscribe(
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
          // this.signatureInDocument = data;
          this.notificationService.showVerify('Verificación de firmas satisfactoria', `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`, data);
        } else {
          this.notificationService.showNoVerify('Verificación de firmas no satisfactoria', `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`, data);
        }
      }, err => {
        // console.log(err.error.ExceptionMessage)
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
    // console.log(`event.target.value:: ${typeof (key)} :: ${key} :: ${value}`);
  }

  copyMessage() {
    this.notificationService.showInfo('', 'Documento copiado');
  }

}

