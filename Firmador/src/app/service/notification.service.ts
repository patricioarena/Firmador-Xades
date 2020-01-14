import { Injectable } from '@angular/core';
import { ToastrService, IndividualConfig } from 'ngx-toastr';
import { take } from 'rxjs/internal/operators/take';

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

  showSuccess(title, message) {
    this.toastr.success(title, message, this.options)
      .onTap
      .pipe(take(1))
      .subscribe(() => this.toasterClickedHandler());
  }

  showError(title, message) {
    this.toastr.error(title, message, this.options)
      .onTap
      .pipe(take(1))
      .subscribe(() => this.toasterClickedHandler());
  }

  showInfo(title, message) {
    this.toastr.info(title, message, this.options)
      .onTap
      .pipe(take(1))
      .subscribe(() => this.toasterClickedHandler());
  }

  toasterClickedHandler() {
    console.log('Toastr clicked');
  }

}

