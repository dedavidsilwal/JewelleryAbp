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
import { FileParameter, ImageServiceProxy } from '../../../shared/service-proxies/service-proxies';
import {
  ProductDto,
  ProductServiceProxy,
  MetalTypeServiceProxy,
  MetalTypeDto,
  CreateEditProductDto,
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';

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
    private _imageService: ImageServiceProxy,
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

  uploadFile = (event): void => {
    // file is the selected file

    const file = event.target.files[0];

    this._imageService
      .upload({ data: file, fileName: file.name } as FileParameter)
      .subscribe((fileName) => {
        // Handle Response
        console.log('image uploadaded ' + fileName);
        this.product.photo = fileName;
      });
  }

  getImageUrl = (relativeImagePath: string) =>
    `${AppConsts.remoteServiceBaseUrl}/Images/${relativeImagePath}.jpg`

  save(): void {
    this.saving = true;

    const product = new CreateEditProductDto();
    product.init(this.product);

    console.log(product);
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
