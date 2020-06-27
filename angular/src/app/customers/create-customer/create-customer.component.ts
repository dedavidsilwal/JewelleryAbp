import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import { CustomerDto, CustomerServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './create-customer.component.html',
})
export class CreateCustomerComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  customer = new CustomerDto();


  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _customerService: CustomerServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {

  }

  save(): void {
    this.saving = true;

    const customer = new CustomerDto();
    customer.init(this.customer);

    this._customerService
      .create(customer)
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
