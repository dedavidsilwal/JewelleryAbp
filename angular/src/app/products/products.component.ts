
import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import {
  ProductDto,
  ProductDtoPagedResultDto,
  ProductServiceProxy
} from '@shared/service-proxies/service-proxies';
import { CreateProductComponent } from './create-product/create-product.component';
import { EditProductComponent } from './edit-product/edit-product.component';
import { AppConsts } from '@shared/AppConsts';

class PagedMetalTypeRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: './products.component.html',
  animations: [appModuleAnimation()]
})
export class ProductsComponent extends PagedListingComponentBase<ProductDto> {
  products: ProductDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _productService: ProductServiceProxy,
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

    this._productService
      .getAll(request.keyword, false, request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: ProductDtoPagedResultDto) => {
        this.products = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  getImageUrl = (relativeImagePath: string) =>
    `${AppConsts.remoteServiceBaseUrl}/Images/${relativeImagePath}.jpg`


  delete(product: ProductDto): void {
    abp.message.confirm(
      this.l('RoleDeleteWarningMessage', product.productName),
      undefined,
      (result: boolean) => {
        if (result) {
          this._productService
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

  createProduct(): void {
    this.showCreateOrEditMetalTypeDialog();
  }

  editProduct(product: ProductDto): void {
    this.showCreateOrEditMetalTypeDialog(product.id);
  }

  showCreateOrEditMetalTypeDialog(id?: string): void {
    let createOrEditMetalTypeDialog: BsModalRef;
    if (!id) {
      createOrEditMetalTypeDialog = this._modalService.show(
        CreateProductComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditMetalTypeDialog = this._modalService.show(
        EditProductComponent,
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
