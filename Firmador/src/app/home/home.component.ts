import { Component, OnInit } from '@angular/core';
import { DigitalSignatureService } from '../digital-signature.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';
import { Propietario, Firma } from 'src/app/modelos/propietario';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {

  firma: Firma;

  constructor(public searchDocumentService: DigitalSignatureService, public spinner: NgxSpinnerService) { }

  ngOnInit() {
  }

  firmar() {
    this.firma = new Firma;
    this.firma.nombre = "Puto de mierda";
    this.firma.firma = "algo";
    this.searchDocumentService.firmar(this.firma).subscribe(res => this.firma = res)
    console.log(this.firma)
  }


}
