
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
  MetalTypeServiceProxy,
  MetalTypeDto,
  CustomerServiceProxy,
  CustomerDto,
  ProductServiceProxy,
  ProductDto
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { SaleServiceProxy, CreateEditSaleDto } from '../../../shared/service-proxies/service-proxies';


@Component({
  templateUrl: './create-sale.component.html',
  styleUrls: ['./create-sale.component.css']
})

export class CreateSaleComponent extends AppComponentBase implements OnInit {
  saving = false;

  @Output() onSave = new EventEmitter<any>();

  loading = new EventEmitter<boolean>();

  form: FormGroup;

  changesprop: number;

  metaltypes = new MetalTypeDto();

  totalPrice = 0;

  public Customers: CustomerDto[] = [];
  public Products: ProductDto[] = [];

  showPartialPayment = false;

  dueAmount = 0;

  constructor(
    private _saleService: SaleServiceProxy,
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
    return this.form.get('saleDetails') as FormArray;
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

    this._customerService.fetchAllCustomers()
      .subscribe((result: CustomerDto[]) => {
        this.Customers = result;
      });

    this._productService.fetchAll()
      .subscribe((result: ProductDto[]) => {
        this.Products = result;
      });


    this.buildForm();

    this.orderDetailsFormArray.push(this.orderDetailsFormGroup);

    this.orderDetailsFormArray.valueChanges.
      subscribe(() => this.calculateTotalAmount());

    this.form.get('paidAmount').valueChanges.subscribe((val) => {
      this.calculateTotalAmount();
    });

  }

  buildForm(): void {
    this.form = this.fb.group({
      customerId: '',
      customerName: '',
      paidAmount: '',
      saleDetails: this.fb.array([])
    });
  }


  calculateTotalAmount(): void {

    const orderDetailFormArray = (this.form.get('saleDetails') as FormArray);

    let Amount = 0;
    for (let index = 0; index < orderDetailFormArray.length; index++) {
      const element = orderDetailFormArray.controls[index];

      const price = parseFloat(element.get('totalPrice').value) || 0;
      Amount = Amount + price;
    }
    this.totalPrice = Amount;

    if (this.totalPrice > 0) {
      const paid = parseFloat(this.form.get('paidAmount').value) || 0;

      this.dueAmount = this.totalPrice - paid;
    }

  }

  selectedCustomer(e: TypeaheadMatch) {
    this.form.get('customerId').setValue(e.item.id);
  }

  selectedProduct(e: TypeaheadMatch, index: number) {
    const product = e.item as ProductDto;


    const orderEntry = (this.form.get('saleDetails') as FormArray).controls[index];

    orderEntry.get('productId').setValue(product.id);

    orderEntry.get('quantity').setValue(1);

    orderEntry.get('metalType').setValue(product.metalType);

    this._metalTypeService
      .fetchTodayMetalPrice(product.metalType)
      .subscribe((price: number) => orderEntry.get('todayMetalCost').setValue(price));

    orderEntry.get('totalWeight').setValue(product.estimatedWeight);
    orderEntry.get('totalPrice').setValue(product.estimatedCost);
  }

  metalWeightChanged(index: number): void {

    const orderEntry = (this.form.get('saleDetails') as FormArray).controls[index];

    const weight = parseFloat(orderEntry.get('weight').value) || 0;
    const wastage = parseFloat(orderEntry.get('wastage').value) || 0;

    const totalWeight = weight + wastage;

    orderEntry.get('totalWeight').setValue(totalWeight);

    const todayPrice = parseFloat(orderEntry.get('todayMetalCost').value);
    const makingCharge = parseFloat(orderEntry.get('makingCharge').value) || 0;
    const totalPrice = totalWeight * todayPrice + makingCharge;

    orderEntry.get('totalPrice').setValue(totalPrice);

  }

  makingChargeChanged(index: number): void {

    const orderEntry = (this.form.get('saleDetails') as FormArray).controls[index];

    const makingCharge = parseFloat(orderEntry.get('makingCharge').value) || 0;
    const totalWeight = parseFloat(orderEntry.get('totalWeight').value) || 0;
    const todayPrice = parseFloat(orderEntry.get('todayMetalCost').value);

    const totalPrice = (totalWeight * todayPrice) + makingCharge;

    orderEntry.get('totalPrice').setValue(totalPrice);
  }

  quantityChanged(index: number): void {

    const orderEntry = (this.form.get('saleDetails') as FormArray).controls[index];

    const todayPrice = parseFloat(orderEntry.get('todayMetalCost').value);
    const quantity = parseFloat(orderEntry.get('quantity').value) || 1;

    const totalPrice = todayPrice * quantity;

    orderEntry.get('totalPrice').setValue(totalPrice);
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

    const sale: CreateEditSaleDto = this.form.value as CreateEditSaleDto;

    if (!this.showPartialPayment) {
      sale.paidAmount = this.totalPrice;
    }

    this._saleService
      .create(sale)
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
