import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import { CustomerDto, CustomerServiceProxy, CustomerDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { CreateCustomerComponent } from './create-customer/create-customer.component';
import { EditCustomerComponent } from './edit-customer/edit-customer.component';
import { debug } from 'console';

class PagedCustomerRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: './customers.component.html',
  animations: [appModuleAnimation()]
})
export class CustomersComponent extends PagedListingComponentBase<CustomerDto> {
  customers: CustomerDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _customerService: CustomerServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  list(
    request: PagedCustomerRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._customerService
      .getAll(request.keyword, false,request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: CustomerDtoPagedResultDto) => {
        this.customers = result.items;
        this.showPaging(result, pageNumber);
      });
    }
      
  delete(customer: CustomerDto): void {
    abp.message.confirm(
      this.l('RoleDeleteWarningMessage', customer.customerName),
      undefined,
      (result: boolean) => {
        if (result) {
          this._customerService
            .delete(customer.id)
            .pipe(
              finalize(() => {
                abp.notify.success(this.l('SuccessfullyDeleted'));
                this.refresh();
              })
            )
            .subscribe(() => {});
        }
      }
    );
  }

  createCustomer(): void {
    this.showCreateOrEditCstomerDialog();
  }

  editCustomer(customer: CustomerDto): void {

    // tslint:disable-next-line: no-debugger
    debugger;
    this.showCreateOrEditCstomerDialog(customer.id);
  }

  showCreateOrEditCstomerDialog(id?: string): void {
    let createOrEditMetalTypeDialog: BsModalRef;
    if (!id) {
      createOrEditMetalTypeDialog = this._modalService.show(
        CreateCustomerComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditMetalTypeDialog = this._modalService.show(
        EditCustomerComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditMetalTypeDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}
