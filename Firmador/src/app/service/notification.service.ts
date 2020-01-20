import { Injectable } from '@angular/core';
import { ToastrService, IndividualConfig } from 'ngx-toastr';
import { take } from 'rxjs/internal/operators/take';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../modal/modal.component';
import { settings } from 'cluster';

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

  showVerify(title, message, arr) {
    this.toastr.success(title, message, this.options)
      .onTap
      .pipe(take(1))
      .subscribe(() => this.openModal('Firmas en el documento', arr));
  }

  showNoVerify(title, message, arr) {
    this.toastr.error(title, message, this.options)
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
    // console.log('Toastr clicked');
  }

  openModal(title: String, arr) {
    const auxArrCN = this.makeArray(arr);
    this.modalRef = this.modalService.show(ModalComponent,  {
      class: 'modal-lg',
      initialState: {
        title: title,
        data: arr,
        arrCN: auxArrCN
      }
    });
  }



  subjectTOArray(arr: any, index: number) {
    let aux = arr[index].Subject;
    aux = aux.replace(/=/g, '":"');
    aux = JSON.stringify(aux);
    aux = aux.replace(/\\/g, '');
    aux = aux.replace(/\, /g, '","');
    aux = JSON.parse('{' + aux + '}');
    return aux;
  }

  makeArray(arr: any) {
    const aux = [];
    arr.forEach(element => {
      const temp = this.subjectTOArray(arr, arr.indexOf(element));
      aux.push(temp.CN);
    });
    return aux;
  }

}

