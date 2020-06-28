import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import {  CustomerServiceProxy, CustomerDtoPagedResultDto, InvoiceDto, InvoiceServiceProxy, InvoiceDtoPagedResultDto } from '@shared/service-proxies/service-proxies';

import { debug } from 'console';

class PagedInvoiceRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: './invoices.component.html',
  animations: [appModuleAnimation()]
})

export class InvoicesComponent extends PagedListingComponentBase<InvoiceDto> {

 
  invoices: InvoiceDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _invoiceService: InvoiceServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  list(
    request: PagedInvoiceRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._invoiceService
      .getAll(request.keyword, false,request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: InvoiceDtoPagedResultDto) => {
        this.invoices = result.items;
        this.showPaging(result, pageNumber);
      });

    }

    protected delete(entity: InvoiceDto): void {
      throw new Error('Method not implemented.');
    }

}
