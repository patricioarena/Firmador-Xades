import { Component, NgModule } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";

import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ModalModule } from "ngx-bootstrap/modal";
import { TooltipModule } from "ngx-bootstrap/tooltip";

import { RouterModule, Routes } from "@angular/router";
import { AppComponent } from "./app.component";
import { HomeComponent } from "./home/home.component";
import { NavbarComponent } from "./navbar/navbar.component";

import { FormsModule } from "@angular/forms";
import { FileUploadModule } from "ng2-file-upload";

import { HttpClientModule } from "@angular/common/http";
// import { DigitalSignatureService } from "./service/digital-signature.service";

import { NgxSpinnerModule } from "ngx-spinner";
import { NgxSpinnerService } from "ngx-spinner";

import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ClipboardModule } from "ngx-clipboard";
import { ToastrModule } from "ngx-toastr";
import { FooterComponent } from "./footer/footer.component";
import { HeaderComponent } from "./header/header.component";
import { ModalComponent } from "./modal/modal.component";
import { RoutingModule } from "./modules/routing.module";
import { NotificationService } from "./service/notification.service";
import { TitleService } from "./service/title.service";

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    FooterComponent,
    HeaderComponent,
    ModalComponent,
  ],
  imports: [
    ClipboardModule,
    BrowserModule,
    HttpClientModule,
    FormsModule,
    FileUploadModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot(), // ToastrModule added
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    NgxSpinnerModule,
    TooltipModule.forRoot(),
    RoutingModule,
  ],
  // DigitalSignatureService
  providers: [NgxSpinnerService, NotificationService, TitleService],
  bootstrap: [AppComponent],
  entryComponents: [ModalComponent],
})
export class AppModule { }
