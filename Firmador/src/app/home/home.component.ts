import { Component, OnInit } from '@angular/core';
import { DigitalSignatureService } from '../service/digital-signature.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Objeto } from 'src/app/modelos/Objeto';
import { saveAs } from 'file-saver';
import { resolve } from 'path';
import { error } from 'util';
import { NotificationService } from '../service/notification.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {
  TiposDeFirma: any = [
                        {'key': 'Xades CIFE - Sin ds:Object' , 'value': '2' },
                        {'key': 'Xades Cartagena (Union Europea) - Con ds:Object' , 'value': '1' }
                      ];

  TipoDeFirma: String = '2';
  text: String;
  objeto: Objeto;
  show = false;
  fileUrl;
  responseFirma;

  constructor(
    public searchDocumentService: DigitalSignatureService,
    public spinner: NgxSpinnerService,
    private notificationService: NotificationService) { }

  ngOnInit() {
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

  Firmar() {
    this.show = false;
    // console.log("TextArea::text: " + this.text);
    this.objeto = new Objeto;
    this.objeto.Archivo = this.text;
    // this.objeto.Extension = '.xml';
    this.searchDocumentService.firma(this.objeto, this.TipoDeFirma).subscribe(
      resp => {
        if (resp === '-1') {
          // this.notificationService.show('Certificado', 'Certificado no valido', 'info');
        }
        else if (resp === '-2') {
          this.notificationService.show('Certificado', 'Certificado no valido', 'info');
        }
        else {
          this.responseFirma = resp;
          this.show = true;
        }
      }, err => {
        // console.log(JSON.parse(err.error).ExceptionMessage)
        let message = JSON.parse(err.error).ExceptionMessage;
        this.notificationService.show('Certificado', message, 'error');
      });
  }


  Verificar() {
    this.objeto = new Objeto;
    this.objeto.Archivo = this.text;
    // this.objeto.Extension = '.xml';
    this.searchDocumentService.verificar(this.objeto, this.TipoDeFirma).subscribe(
      resp => {

        let arr = JSON.parse(JSON.stringify(resp.data));
        console.log(JSON.stringify(arr));
        console.table(arr);
        // console.log(JSON.stringify(arr.data))
        if (arr[0].IsValid === true) {
          this.notificationService.show('Firmas', JSON.stringify(arr[0].Message), 'success');
        }
        else if (arr[0].IsValid === false) {
          this.notificationService.show('Firmas', JSON.stringify(arr[0].Message), 'error');
        }else{

        }

      }, err => {
        // console.log(err.error.ExceptionMessage)
        let message = err.error.ExceptionMessage;
        this.notificationService.show('Firmas', message, 'error');
      });
  }

  downloadFile() {
    const blob = new Blob([this.responseFirma], { type: 'text/xml; charset=utf-8' });
    saveAs(blob, 'Document.xml');
  }

  changeType(event) {
    let key = event.target.value;
    let value = this.TiposDeFirma.find( function(item) { return item.key == key } ).value;
    this.TipoDeFirma = value;
    console.log(`event.target.value:: ${typeof(key)} :: ${key} :: ${value}`);
  }

}
