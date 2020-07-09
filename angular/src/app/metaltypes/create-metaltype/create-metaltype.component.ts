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
  MetalTypeServiceProxy,
  MetalTypeDto,
  JewelleryServiceProxy,
  SelectListItem,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './create-metaltype.component.html',
})
export class CreateMetaltypeComponent extends AppComponentBase
  implements OnInit {
  saving = false;

  metaltype = new MetalTypeDto();

  WeightTypes: SelectListItem[];


  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _metalTypeService: MetalTypeServiceProxy,
    private _jewelleryService: JewelleryServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);

    this._jewelleryService.getAllWeighTypes().subscribe((result: SelectListItem[]) => {
      this.WeightTypes = result;
    });

    this.metaltype.weightType = 0;

    // tslint:disable-next-line: radix
    // this.weightTypeKeys = Object.keys(this.metaltype).filter(k => !isNaN(Number(k))).map(k => parseInt(k));
  }

  ngOnInit(): void {

  }

  save(): void {
    this.saving = true;

    const metalType = new MetalTypeDto();
    metalType.init(this.metaltype);

    this._metalTypeService
      .create(metalType)
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
