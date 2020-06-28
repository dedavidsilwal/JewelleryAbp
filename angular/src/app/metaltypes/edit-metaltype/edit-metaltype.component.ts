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
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './edit-metaltype.component.html',
})
export class EditMetaltypeComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  metaltype = new MetalTypeDto();
 
  id:string;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _metalTypeService: MetalTypeServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._metalTypeService.get(this.id)
    .subscribe(result => this.metaltype = result);

  }

  save(): void {
    this.saving = true;

    const metalType = new MetalTypeDto();
    metalType.init(this.metaltype);

    this._metalTypeService
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
