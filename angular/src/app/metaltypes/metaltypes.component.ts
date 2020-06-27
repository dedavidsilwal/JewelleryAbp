import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import {
  MetalTypeDto,
  MetalTypeServiceProxy,
  MetalTypeDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { CreateMetaltypeComponent } from './create-metaltype/create-metaltype.component';
import { EditMetaltypeComponent } from './edit-metaltype/edit-metaltype.component';

class PagedMetalTypeRequestDto extends PagedRequestDto {
  keyword: string;  
}

@Component({
  templateUrl: './metaltypes.component.html',
  animations: [appModuleAnimation()]
})
export class MetaltypesComponent extends PagedListingComponentBase<MetalTypeDto> {
  metaltypes: MetalTypeDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _metalTypeService: MetalTypeServiceProxy,
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

    this._metalTypeService
      .getAll(request.keyword, false,request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: MetalTypeDtoPagedResultDto) => {
        this.metaltypes = result.items;
        this.showPaging(result, pageNumber);
      });
    }
      
  delete(metalType: MetalTypeDto): void {
    abp.message.confirm(
      this.l('RoleDeleteWarningMessage', metalType.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._metalTypeService
            .delete(metalType.id)
            .pipe(
              finalize(() => {
                abp.notify.success(this.l('SuccessfullyDeleted'));
                this.refresh();
              })
            )
            .subscribe(() => {});
        }
      }
    );
  }

  createMetalType(): void {
    this.showCreateOrEditMetalTypeDialog();
  }

  editMetalType(metaltype: MetalTypeDto): void {
    this.showCreateOrEditMetalTypeDialog(metaltype.id);
  }

  showCreateOrEditMetalTypeDialog(id?: string): void {
    let createOrEditMetalTypeDialog: BsModalRef;
    if (!id) {
      createOrEditMetalTypeDialog = this._modalService.show(
        CreateMetaltypeComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditMetalTypeDialog = this._modalService.show(
        EditMetaltypeComponent,
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
