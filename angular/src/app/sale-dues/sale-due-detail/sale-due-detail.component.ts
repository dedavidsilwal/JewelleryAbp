import { Component, OnInit, Output, EventEmitter, Injector } from '@angular/core';
import { OrderDueDetailDto, OrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SaleServiceProxy, DuesServiceProxy, SaleDueDetailDto } from '../../../shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-sale-due-detail',
  templateUrl: './sale-due-detail.component.html',
  styleUrls: ['./sale-due-detail.component.css']
})
export class SaleDueDetailComponent extends AppComponentBase  implements OnInit {

  saving = false;

  id: string;
  @Output() onSave = new EventEmitter<any>();

  loading = new EventEmitter<boolean>();

  orderDue: SaleDueDetailDto;


  dueForm: FormGroup;
  constructor(
    injector: Injector,
    private _saleService: SaleServiceProxy,
    private _dueservice: DuesServiceProxy,
    public bsModalRef: BsModalRef,
    private fb: FormBuilder
  ) {
    super(injector);

  }

  ngOnInit(): void {
    this.buildForm();

    this._saleService.getSaleDueDetail(this.id)
      .subscribe(result => {
        this.orderDue = result;
      });

  }

  buildForm(): void {
    this.dueForm = this.fb.group({
      paidAmount: ''
    });
  }

  save(): void {
    this.saving = true;

    this._saleService
      .duePaid(this.id, this.dueForm.get('paidAmount').value)
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
