import { Injectable } from '@angular/core';
import { ToastrService, IndividualConfig } from 'ngx-toastr';
import { take } from 'rxjs/internal/operators/take';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../modal/modal.component';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  modalRef: BsModalRef;
  options: IndividualConfig;

  constructor(private toastr: ToastrService, private modalService: BsModalService) {
    this.toastr.toastrConfig.preventDuplicates = true;
    this.options = this.toastr.toastrConfig;
    this.options.positionClass = 'toast-bottom-right';
    this.options.timeOut = 1500;
  }

  showSuccess(title, message, arr) {
    this.toastr.success(title, message, this.options)
      .onTap
      .pipe(take(1))
      .subscribe(() => this.openModal('Firmas en el documento', arr));
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

  openModal(title:String, arr) {
    this.modalRef = this.modalService.show(ModalComponent,  {
      initialState: {
        title: title,
        data: arr
      }
    });
  }

}

