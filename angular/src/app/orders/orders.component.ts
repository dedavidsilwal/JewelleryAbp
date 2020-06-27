
import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import {
  OrderServiceProxy, OrderDto, OrderDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';

import { CreateOrderComponent } from './create-order/create-order.component';
import { EditOrderComponent } from './edit-order/edit-order.component';

class PagedMetalTypeRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: './orders.component.html',
  animations: [appModuleAnimation()]
})
export class OrdersComponent extends PagedListingComponentBase<OrderDto> {
  orders: OrderDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _orderService: OrderServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  list(
    request: PagedMetalTypeRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._orderService
      .getAll(request.keyword, false,request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: OrderDtoPagedResultDto) => {
        this.orders = result.items;
        this.showPaging(result, pageNumber);
      });
    }
      
  delete(product: OrderDto): void {
    abp.message.confirm(
      this.l('RoleDeleteWarningMessage', product.id),
      undefined,
      (result: boolean) => {
        if (result) {
          this._orderService
            .delete(product.id)
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

  createOrder(): void {
    this.showCreateOrEditOrderDialog();
  }

  editOrder(product: OrderDto): void {
    this.showCreateOrEditOrderDialog(product.id);
  }

  showCreateOrEditOrderDialog(id?: string): void {
    let createOrEditMetalTypeDialog: BsModalRef;
    if (!id) {
      createOrEditMetalTypeDialog = this._modalService.show(
        CreateOrderComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditMetalTypeDialog = this._modalService.show(
        EditOrderComponent,
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
