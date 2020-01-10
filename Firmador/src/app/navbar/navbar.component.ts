import { Component, OnInit, Input } from '@angular/core';
import { TitleService } from '../service/title.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  title: String;

  constructor(private titleService: TitleService ) { }

  ngOnInit() {
    this.title = this.titleService.APP_TITLE;
  }

}
