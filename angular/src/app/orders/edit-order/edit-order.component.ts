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
import { OrderServiceProxy, CreateOrderDto, MetalTypeServiceProxy, MetalTypeDto, CustomerServiceProxy, CustomerDto, ProductServiceProxy, ProductDto, EditOrderDto, OrderDetailDto } from '@shared/service-proxies/service-proxies';
import { Observable, of, Subject, Subscription, fromEvent } from 'rxjs';
import { map, filter, debounceTime, distinctUntilChanged, switchAll, tap, finalize, publishReplay } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
const CACHE_SIZE = 1;


@Component({
  templateUrl: './edit-order.component.html',
})

export class EditOrderComponent extends AppComponentBase implements OnInit {
  saving = false;

  @Output() onSave = new EventEmitter<any>();

  loading = new EventEmitter<boolean>();

  form: FormGroup;

  changesprop: number;

  metaltypes = new MetalTypeDto();

  totalPrice: number;

  id: string;

  public Customers: CustomerDto[] = [];
  public Products: ProductDto[] = [];

  showAdvancePayment = false;


  constructor(
    private _orderService: OrderServiceProxy,
    private _customerService: CustomerServiceProxy,
    private _metalTypeService: MetalTypeServiceProxy,
    private _productService: ProductServiceProxy,
    injector: Injector,
    public bsModalRef: BsModalRef,
    private fb: FormBuilder
  ) {

    super(injector);

  }

  get orderDetailsFormArray(): FormArray {
    return this.form.get('orderDetails') as FormArray;
  }

  get orderDetailsFormGroup(): FormGroup {
    return this.fb.group({
      productName: '',
      productId: '',
      quantity: '',
      makingCharge: '',
      weight: '',
      wastage: '',
      metalType: '',
      metalCostThisDay: '',
      totalWeight: '',
      totalPrice: ''
    });
  }

  get orderFormControl() {
    return this.form.controls;
  }

  ngOnInit(): void {

    this.buildForm();

    this.orderDetailsFormArray.push(this.orderDetailsFormGroup);

    this._orderService.fetchOrderWithDetails(this.id)
      .subscribe(result => {

        this.form.patchValue(result);
        this.form.patchValue({ 'requiredDate': new Date(result.requiredDate?.toString()) });

        this.orderDetailsFormArray.patchValue(result.orderDetails);

      });

    this._customerService.fetchAllCustomers()
      //  .pipe(publishReplay(CACHE_SIZE))
      .subscribe((result: CustomerDto[]) => {
        this.Customers = result;
      });

    this._productService.fetchAll()
      // .pipe(publishReplay(CACHE_SIZE))
      .subscribe((result: ProductDto[]) => {
        this.Products = result;
      });

    this.form.controls['customerId']
      .valueChanges
      .subscribe(val => {
        const cs = this.Customers.find(s => s.id.match(val))?.customerName;
        this.form.get('customerName').setValue(cs);
      });


    this.orderDetailsFormArray.controls.forEach(element => {

      this.form.controls['orderDetails']
        .valueChanges
        .subscribe((orderEntry: OrderDetailDto[]) => {
          this.totalPrice = orderEntry.map(c => c.totalPrice).reduce((sum, current) => sum + current);

        });

      element.get('productId').valueChanges.subscribe(val => {
        element.get('productName').setValue(this.Products.find(s => s.id.match(val))?.productName);
      });

      element.get('weight').valueChanges.subscribe(val => {

        const weight = parseFloat(element.get('weight').value) || 0;
        const wastage = parseFloat(element.get('wastage').value) || 0;

        const totalWeight = weight + wastage;

        element.get('totalWeight').setValue(totalWeight);

      });


      element.get('totalWeight').valueChanges.subscribe(val => {

        const todayPrice = parseFloat(element.get('metalCostThisDay').value);
        const makingCharge = parseFloat(element.get('makingCharge').value) || 0;
        const totalPrice = val * todayPrice + makingCharge;

        element.get('totalPrice').setValue(totalPrice);

      });
    });
  }


  buildForm(): void {
    this.form = this.fb.group({
      customerId: '',
      customerName: '',
      requiredDate: '',
      advancePaymentAmount: '',
      orderDetails: this.fb.array([])
    });
  }

  metalWeightChanged(index: number): void {

    const orderEntry = (this.form.get('orderDetails') as FormArray).controls[index];

    const weight = parseFloat(orderEntry.get('weight').value) || 0;
    const wastage = parseFloat(orderEntry.get('wastage').value) || 0;

    const totalWeight = weight + wastage;

    orderEntry.get('totalWeight').setValue(totalWeight);

    const todayPrice = parseFloat(orderEntry.get('metalCostThisDay').value);
    const makingCharge = parseFloat(orderEntry.get('makingCharge').value) || 0;
    const totalPrice = totalWeight * todayPrice + makingCharge;

    orderEntry.get('totalPrice').setValue(totalPrice);

  }

  makingChargeChanged(index: number): void {

    const orderEntry = (this.form.get('orderDetails') as FormArray).controls[index];
    let totalPrice = parseFloat(orderEntry.get('totalPrice').value) || 0;
    const makingCharge = parseFloat(orderEntry.get('makingCharge').value) || 0;

    totalPrice += makingCharge;
    orderEntry.get('totalPrice').setValue(totalPrice);
  }

  selectedCustomer(e: TypeaheadMatch) {
    console.log(e.item.id);
    this.form.get('customerId').setValue(e.item.id);
  }

  selectedProduct(e: TypeaheadMatch, index: number) {
    const product = e.item as ProductDto;
    console.log(product);


    const orderEntry = (this.form.get('orderDetails') as FormArray).controls[index];
    console.log(orderEntry);

    orderEntry.get('productId').setValue(product.id);


    orderEntry.get('quantity').setValue(1);

    orderEntry.get('metalType').setValue(product.metalType);

    this._metalTypeService
      .fetchTodayMetalPrice(product.metalType)
      .subscribe((price: number) => orderEntry.get('metalCostThisDay').setValue(price));

    orderEntry.get('totalWeight').setValue(product.estimatedWeight);
    orderEntry.get('totalPrice').setValue(product.estimatedCost);
  }

  DeleteOrderDetail(index: number): void {
    this.orderDetailsFormArray.removeAt(index);
  }

  createOderDetail(e: Event): void {

    e.stopPropagation();
    e.preventDefault();

    this.orderDetailsFormArray.push(this.orderDetailsFormGroup);
  }

  save(): void {
    this.saving = true;

    const order: EditOrderDto = this.form.value as EditOrderDto;
    order.id = this.id;

    this._orderService
      .update(order)
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
