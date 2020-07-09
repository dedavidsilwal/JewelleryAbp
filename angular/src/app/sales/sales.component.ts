
import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import { SaleDto, SaleServiceProxy, SaleDtoPagedResultDto } from '../../shared/service-proxies/service-proxies';
import { CreateSaleComponent } from './create-sale/create-sale.component';
import { EditSaleComponent } from './edit-sale/edit-sale.component';

class PagedSalerequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: './sales.component.html',
  animations: [appModuleAnimation()]
})
export class SalesComponent extends PagedListingComponentBase<SaleDto> {

  sales: SaleDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _saleService: SaleServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  list(
    request: PagedSalerequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._saleService
      .getAll(request.keyword, false, request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: SaleDtoPagedResultDto) => {
        this.sales = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  delete(product: SaleDto): void {
    abp.message.confirm(
      this.l('RoleDeleteWarningMessage', product.id),
      undefined,
      (result: boolean) => {
        if (result) {
          this._saleService
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

  create(): void {
    this.showCreateOrEditDialog();
  }

  edit(order: SaleDto): void {
    this.showCreateOrEditDialog(order.id);
  }



  showCreateOrEditDialog(id?: string): void {
    let createOrEditDialog: BsModalRef;
    if (!id) {
      createOrEditDialog = this._modalService.show(
        CreateSaleComponent,
        {
          class: 'modal-xl',
        }
      );
    } else {
      createOrEditDialog = this._modalService.show(
        EditSaleComponent,
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
