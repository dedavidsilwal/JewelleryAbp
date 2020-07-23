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
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import {
  OrderServiceProxy,
  CreateOrderDto,
  MetalTypeServiceProxy,
  MetalTypeDto,
  CustomerServiceProxy,
  CustomerDto,
  ProductServiceProxy,
  ProductDto,
  CustomerSearchResultDto
} from '@shared/service-proxies/service-proxies';
import { finalize, switchMap, map, tap } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { debug } from 'console';
import { query } from '@angular/animations';
import { noop, Observable, Observer, of } from 'rxjs';
import { ProductSearchResultDto } from '../../../shared/service-proxies/service-proxies';

@Component({
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css']
})

export class CreateOrderComponent extends AppComponentBase implements OnInit {
  saving = false;

  @Output() onSave = new EventEmitter<any>();

  loading = new EventEmitter<boolean>();

  form: FormGroup;

  changesprop: number;

  metaltypes = new MetalTypeDto();

  totalPrice = 0;
  dueAmount = 0;

  public Products: ProductDto[] = [];

  showAdvancePayment = false;

  suggestionCustomers$: Observable<CustomerSearchResultDto[]>;
  suggestionProducts$: Observable<ProductSearchResultDto[]>;

  searchProductKeyword: string;

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
      todayMetalCost: '',
      totalWeight: '',
      totalPrice: '',
    });
  }

  get orderFormControl() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.buildForm();
    this.orderDetailsFormArray
      .valueChanges
      .subscribe(() => {
      });

    this.orderDetailsFormArray.push(this.orderDetailsFormGroup);

    this.orderDetailsFormArray.valueChanges.subscribe(() => this.calculateTotalAmount());

    this.form.get('advancePaid').valueChanges.subscribe((val) => {
      this.calculateTotalAmount();
    });


    this.suggestionCustomers$ = new Observable((observer: Observer<string>) => {
      observer.next(this.form.get('customerName').value);
    }).pipe(
      // tslint:disable-next-line: no-shadowed-variable
      switchMap((query: string) => {
        if (query) {
          return this._customerService.searchCustomerQuery(query).pipe(
            map((data: CustomerSearchResultDto[]) => {
              return data && data || [];
            }),
            tap(() => noop, err => {
              // in case of http error
              // this.errorMessage = err && err.message || 'Something goes wrong';
            })
          );
        }

        return of([]);
      })
    );

    this.suggestionProducts$ = new Observable((observer: Observer<string>) => {
      observer.next(this.searchProductKeyword);
    }).pipe(
      // tslint:disable-next-line: no-shadowed-variable
      switchMap((query: string) => {
        if (query) {
          return this._productService.searchProductQuery(query).pipe(
            map((data: ProductSearchResultDto[]) => {
              return data && data || [];
            }),
            tap(() => noop, err => {
              // in case of http error
              // this.errorMessage = err && err.message || 'Something goes wrong';
            })
          );
        }

        return of([]);
      })
    );

  }

  calculateTotalAmount(): void {

    const orderDetailFormArray = (this.form.get('orderDetails') as FormArray);

    let Amount = 0;
    for (let index = 0; index < orderDetailFormArray.length; index++) {
      const element = orderDetailFormArray.controls[index];

      const price = parseFloat(element.get('totalPrice').value) || 0;
      Amount = Amount + price;
      console.log(price);

    }
    this.totalPrice = Amount;

    if (this.totalPrice > 0) {
      const advancePaid = parseFloat(this.form.get('advancePaid').value) || 0;

      this.dueAmount = this.totalPrice - advancePaid;
    }
  }


  selectedCustomer(e: TypeaheadMatch) {
    console.log(e.item.id);
    this.form.patchValue({ 'customerId': e.item.id });
  }

  selectedProduct(e: TypeaheadMatch, index: number) {
    const product = e.item as ProductDto;
    console.log(product);


    const orderEntry = (this.form.get('orderDetails') as FormArray).controls[index];
    console.log(orderEntry);

    orderEntry.get('productId').setValue(product.id);

    orderEntry.get('quantity').setValue(1);

    orderEntry.get('metalType').setValue(product.metalType);
    orderEntry.get('weight').setValue(product.estimatedWeight);

    this._metalTypeService
      .fetchTodayMetalPrice(product.metalType)
      .subscribe((price: number) => orderEntry.get('todayMetalCost').setValue(price));

    orderEntry.get('totalWeight').setValue(product.estimatedWeight);
    orderEntry.get('totalPrice').setValue(product.estimatedCost);
  }

  metalWeightChanged(index: number): void {

    const orderEntry = (this.form.get('orderDetails') as FormArray).controls[index];

    const weight = parseFloat(orderEntry.get('weight').value) || 0;
    const wastage = parseFloat(orderEntry.get('wastage').value) || 0;

    const totalWeight = weight + wastage;

    orderEntry.get('totalWeight').setValue(totalWeight);

    const todayPrice = parseFloat(orderEntry.get('todayMetalCost').value);
    const makingCharge = parseFloat(orderEntry.get('makingCharge').value) || 0;
    const totalPrice = (totalWeight * todayPrice) + makingCharge;

    orderEntry.get('totalPrice').setValue(totalPrice);

  }


  makingChargeChanged(index: number): void {

    const orderEntry = (this.form.get('orderDetails') as FormArray).controls[index];

    const makingCharge = parseFloat(orderEntry.get('makingCharge').value) || 0;
    const totalWeight = parseFloat(orderEntry.get('totalWeight').value) || 0;
    const todayPrice = parseFloat(orderEntry.get('todayMetalCost').value);

    const totalPrice = (totalWeight * todayPrice) + makingCharge;

    orderEntry.get('totalPrice').setValue(totalPrice);
  }

  quantityChanged(index: number): void {

    const orderEntry = (this.form.get('orderDetails') as FormArray).controls[index];

    const todayPrice = parseFloat(orderEntry.get('todayMetalCost').value);
    const quantity = parseFloat(orderEntry.get('quantity').value) || 1;

    const totalPrice = todayPrice * quantity;

    orderEntry.get('totalPrice').setValue(totalPrice);
  }

  buildForm(): void {
    this.form = this.fb.group({
      customerId: '',
      customerName: '',
      requiredDate: '',
      advancePaid: '',
      orderDetails: this.fb.array([])
    });
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

    const order: CreateOrderDto = this.form.value as CreateOrderDto;

    if (!this.showAdvancePayment) {
      order.advancePaid = null;
    }

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
