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
  CreateEditProductDto,
  MetalTypeServiceProxy,
  MetalTypeDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './edit-product.component.html',
})
export class EditProductComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  product = new CreateEditProductDto();
 
  id: string;

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
   
    this._productService.get(this.id)
    .subscribe(result => this.product = result);

    this._metalTypeService
    .fetchAllMetalTypes()
        .subscribe((result: MetalTypeDto[]) => {
          this.metalTypes = result;
        });

  }

  save(): void {
    this.saving = true;

    const metalType = new CreateEditProductDto();
    metalType.init(this.product);

    this._productService
      .update(metalType)
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