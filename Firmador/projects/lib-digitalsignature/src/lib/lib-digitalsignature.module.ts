import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ClipboardModule } from 'ngx-clipboard';
import { DigitalSignatureComponent } from './lib-digitalsignature.component';

@NgModule({
  declarations: [DigitalSignatureComponent],
  imports: [
    FormsModule,
    ClipboardModule,
  ],
  exports: [DigitalSignatureComponent],
})
export class DigitalSignatureModule { }
