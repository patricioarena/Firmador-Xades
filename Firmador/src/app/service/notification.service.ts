import { Injectable } from '@angular/core';
import { ToastrService, IndividualConfig } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  options: IndividualConfig;

  constructor(private toastr: ToastrService) {
    this.toastr.toastrConfig.preventDuplicates = true;
    this.options = this.toastr.toastrConfig;
    this.options.positionClass = 'toast-bottom-right';
    this.options.timeOut = 1500;
  }

  show(title, message, type) {
    this.toastr.show(message, title, this.options, 'toast-' + type);
  }
}
