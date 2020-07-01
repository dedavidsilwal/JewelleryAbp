import { Component, OnInit, EventEmitter, Output, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { OrderServiceProxy, PaymentOrderDto, UpdatePaymentDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';

@Component({
  selector: 'app-payment-process',
  templateUrl: './payment-process.component.html',
  styleUrls: ['./payment-process.component.css']
})
export class PaymentProcessComponent extends AppComponentBase implements OnInit {
  saving = false;

  @Output() onSave = new EventEmitter<any>();

  loading = new EventEmitter<boolean>();

  id: string;


  paymentProcess: PaymentOrderDto;

  updateForm = this.formbuilder.group({
    orderId: ['', Validators.required],
    paidAmount: ['', Validators.required],
  });

  constructor(
    private _orderService: OrderServiceProxy,
    injector: Injector,
    public bsModalRef: BsModalRef,
    private formbuilder: FormBuilder
  ) {

    super(injector);

  }



  ngOnInit(): void {

    this._orderService
      .getPaymentOrderDto(this.id)
      .subscribe((result: PaymentOrderDto) => {

        this.paymentProcess = result;
        this.updateForm.patchValue({ 'orderId': result.orderId });
      });
  }

  save(): void {
    this.saving = true;

    const updatePayment = this.updateForm.value as UpdatePaymentDto;

    console.log(updatePayment);

    this._orderService.updateOrderPayment(updatePayment)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      });
  }

}
