import { Component, OnInit, EventEmitter, Output, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { OrderServiceProxy, CreateOrderDto, OrderDetailDto } from '@shared/service-proxies/service-proxies';
import { finalize, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  templateUrl: './new-order.component.html',
  styleUrls: ['./new-order.component.css']
})
export class NewOrderComponent extends AppComponentBase implements OnInit {

  saving = false;

  @Output() onSave = new EventEmitter<any>();

  loading = new EventEmitter<boolean>();

  orderForm: FormGroup;

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private fb: FormBuilder,
    private _orderService: OrderServiceProxy,

  ) {
    super(injector);

  }

  get orderItem(): FormArray {
    return this.orderForm.get('orderDetails') as FormArray;
  }

  get buildOrderItem(): FormGroup {
    return this.fb.group({
      productName: ['', Validators.required],
      quantity: [0, Validators.required],
      metalType: ['', Validators.required],
      todayMetalCost: [0, Validators.required],
      makingCharge: [0],
      weight: [0, Validators.required],
      wastage: [0],
      totalWeight: [0, Validators.required],
      subTotal: [0],
    });

  }



  ngOnInit(): void {

    this.orderForm = this.fb.group({
      customerName: ['', Validators.required],
      requiredDate: ['', Validators.required],
      advancePaid: [0],
      orderDetails: this.fb.array([this.buildOrderItem])
    });



    // this.orderItem.valueChanges
    //   // .pipe(debounceTime(500))
    //   // .pipe(distinctUntilChanged())
    //   .subscribe((value) => {

    //   });


    // this.orderItem.value.reduce((prev, next) => {
    //   console.log(prev + next);
    // });

    // this.orderForm.value.array.forEach(element => {

    // });

    // Object.keys(this.orderItem.controls).forEach(element => {
    //   console.log(element);
    // });


  }

  calculateOrderItem(index: number): void {
    const currentItem = this.orderItem.controls[index];
    console.log(currentItem);


    // const totalWeight = +item.weight + +item.wastage;

    // currentItem.patchValue({ 'totalWeight': totalWeight });

    // const subTotal = (((item.totalWeight * item.todayMetalCost) + item.makingCharge) * item.quantity) || 0;
    // currentItem.patchValue({ 'subTotal': totalWeight });
  }


  AddOrderItem(): void {
    this.orderItem.push(this.buildOrderItem);
  }
  RemoveOrderItem(index: number): void {
    this.orderItem.removeAt(index);
  }

  save(): void {
    this.saving = true;

    const order: CreateOrderDto = this.orderForm.value as CreateOrderDto;

    this._orderService
      .create(order)
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
