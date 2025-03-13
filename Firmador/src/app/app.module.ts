import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { ModalModule } from "ngx-bootstrap/modal";
import { TooltipModule } from "ngx-bootstrap/tooltip";

import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { FooterComponent } from "./footer/footer.component";
import { HomeComponent } from "./home/home.component";
import { NavbarComponent } from "./navbar/navbar.component";

import { FormsModule } from "@angular/forms";
import { FileUploadModule } from "ng2-file-upload";

import { HttpClientModule } from "@angular/common/http";

import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ClipboardModule } from "ngx-clipboard";
import { ToastrModule } from "ngx-toastr";
import { DigitalSignatureComponent } from "./digitalsignature/digitalsignature.component";
import { HeaderComponent } from "./header/header.component";
import { ModalComponent } from "./modal/modal.component";
import { RoutingModule } from "./modules/routing.module";
import { NotificationService } from "./service/notification.service";

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    FooterComponent,
    HeaderComponent,
    ModalComponent,
    DigitalSignatureComponent,
  ],
  imports: [
    ClipboardModule,
    BrowserModule,
    HttpClientModule,
    FormsModule,
    FileUploadModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 6000,
      positionClass: "toast-bottom-center",
      progressBar: true,
      progressAnimation: "increasing",
    }),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    RoutingModule,
    RouterModule,
  ],
  providers: [NotificationService],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
