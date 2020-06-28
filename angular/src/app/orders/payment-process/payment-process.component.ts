import { Component, OnInit, EventEmitter, Output, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { OrderServiceProxy, PaymentOrderDto, UpdatePaymentDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap/modal';

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

  updatePayment: UpdatePaymentDto;


  constructor(
    private _orderService: OrderServiceProxy,
    injector: Injector,
    public bsModalRef: BsModalRef

  ) {

    super(injector);

  }

  ngOnInit(): void {
    this._orderService
      .getPaymentOrderDto(this.id)
      .subscribe((result: PaymentOrderDto) => {
        this.paymentProcess = result;
        this.updatePayment.orderId = this.id;
      });
  }

  save(): void {
    this.saving = true;
    this._orderService.updatePayment(this.updatePayment)
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
