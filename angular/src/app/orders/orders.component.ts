
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
import { PaymentProcessComponent } from './payment-process/payment-process.component';
import { OrderDetailComponent } from './order-detail/order-detail.component';

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
      .getAll(request.keyword, false, request.skipCount, request.maxResultCount)
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
            .subscribe(() => { });
        }
      }
    );
  }

  cancelOrder(id: string): void {
    abp.message.confirm(
      this.l('OrderCancelWarningMessage', id),
      undefined,
      (result: boolean) => {
        if (result) {
          this._orderService
            .cancel(id)
            .pipe(
              finalize(() => {
                abp.notify.success(this.l('SuccessfullyDeleted'));
                this.refresh();
              })
            )
            .subscribe(() => { });
        }
      }
    );
  }
  createOrder(): void {
    this.showCreateOrEditOrderDialog();
  }
  editOrder(order: OrderDto): void {
    this.showCreateOrEditOrderDialog(order.id);
  }

  paymentProcess(id: string): void {
    let paymentProcessDialog: BsModalRef;

    paymentProcessDialog = this._modalService.show(
      PaymentProcessComponent,
      {
        class: 'modal-lg'
        ,
        initialState: {
          id: id,
        }
      });

    paymentProcessDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  DisplayOrderDetail(id: string): void {
    let OrderDisplayDialog: BsModalRef;

    OrderDisplayDialog = this._modalService.show(
      OrderDetailComponent,
      {
        class: 'modal-lg',
        initialState: {
          id: id,
        }
      });
  }

  showCreateOrEditOrderDialog(id?: string): void {
    let createOrEditDialog: BsModalRef;
    if (!id) {
      createOrEditDialog = this._modalService.show(
        CreateOrderComponent,
        {
          class: 'modal-xl',
        }
      );
    } else {
      createOrEditDialog = this._modalService.show(
        EditOrderComponent,
        {
          class: 'modal-xl',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}
