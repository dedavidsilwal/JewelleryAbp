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
import {
  ProductDto,
  ProductServiceProxy,
  MetalTypeServiceProxy,
  MetalTypeDto,
  CreateEditProductDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './create-product.component.html',
})
export class CreateProductComponent extends AppComponentBase
  implements OnInit {
  saving = false;

  product = new CreateEditProductDto();

  metalTypes = new Array<MetalTypeDto>();

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _productService: ProductServiceProxy,
    private _metalTypeService: MetalTypeServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {

    this._metalTypeService
      .fetchAllMetalTypes()
          .subscribe((result: MetalTypeDto[]) => {
            this.metalTypes = result;
          });
  }

  save(): void {
    this.saving = true;

    const product = new CreateEditProductDto();
    product.init(this.product);

    this._productService
      .create(product)
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
