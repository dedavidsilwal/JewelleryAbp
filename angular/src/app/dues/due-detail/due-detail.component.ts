import { Component, OnInit, EventEmitter, Injector, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { OrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { OrderDueDetailDto } from '../../../shared/service-proxies/service-proxies';
import { FormGroup, FormBuilder, Validator } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { SaleDueDetailComponent } from '@app/sale-dues/sale-due-detail/sale-due-detail.component';

@Component({
  selector: 'app-due-detail',
  templateUrl: './due-detail.component.html',
  styleUrls: ['./due-detail.component.css']
})
export class DueDetailComponent extends AppComponentBase implements OnInit {

  saving = false;

  id: string;
  @Output() onSave = new EventEmitter<any>();

  loading = new EventEmitter<boolean>();

  orderDue: OrderDueDetailDto;


  dueForm: FormGroup;

  constructor(
    injector: Injector,
    private _orderService: OrderServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService,
    private fb: FormBuilder
  ) {
    super(injector);

  }

  ngOnInit(): void {
    this.buildForm();

    this._orderService.getOrderDueDetail(this.id)
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

    this._orderService
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
