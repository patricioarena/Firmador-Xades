import { Component } from '@angular/core';
import { TitleService } from './service/title.service';
// import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public constructor(private titleService: TitleService ) {
    // this.setTitle("CertiFisc");
    this.titleService.init("CertiFisc");
  }

  // public setTitle( newTitle: string ) {
  //   this.titleService.setTitle( newTitle );
  // }
}

