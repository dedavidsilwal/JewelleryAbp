import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import { CustomerServiceProxy, CustomerDtoPagedResultDto, InvoiceDto, InvoiceServiceProxy, InvoiceDtoPagedResultDto } from '@shared/service-proxies/service-proxies';

import { debug } from 'console';
import { OrderDetailComponent } from '@app/orders/order-detail/order-detail.component';
import { SaleDetailComponent } from '../sales/sale-detail/sale-detail.component';
import { DueDto, DuesServiceProxy, DueDtoPagedResultDto } from '../../shared/service-proxies/service-proxies';

class PagedDueRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: './sale-dues.component.html',
  animations: [appModuleAnimation()]
})

export class SaleDuesComponent extends PagedListingComponentBase<DueDto> {



  dues: DueDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _dueService: DuesServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  list(
    request: PagedDueRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._dueService
      .getSaleDuesAmount(request.keyword, false, request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: DueDtoPagedResultDto) => {
        this.dues = result.items;
        console.log(this.dues);
        this.showPaging(result, pageNumber);
      });

  }

  protected delete(entity: DueDto): void {
    throw new Error('Method not implemented.');
  }
}
