import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';

@Component({
  templateUrl: './create-order.component.html',
})
export class CreateOrderComponent extends AppComponentBase implements OnInit {
  saving = false;

  @Output() onSave = new EventEmitter<any>();

  orderForm: FormGroup;
  orderDetails: FormArray;

  orderFormValueChanges$;

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private formBuilder: FormBuilder
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.orderForm = this.formBuilder.group({
      customerId: '',
      requiredDate: '',
      orderDetails: this.formBuilder.array([this.createOrderDetailForm()]),
    });
  }

  createOrderDetailForm(): FormGroup {
    return this.formBuilder.group({
      productId: '',
      quantity: '',
      weight: '',
      makingCharge: '',
      wastage: '',
      metalType: '',
      metalCostThisDay: '',
      unitPrice: '',
      totalPrice: ''
    });
  }

  createOderDetail(): void {
    this.orderDetails = this.orderForm.get('orderDetails') as FormArray;

    this.orderDetails.push(this.createOrderDetailForm());
  }
}
