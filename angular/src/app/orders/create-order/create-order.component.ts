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
import { OrderDto, OrderServiceProxy, OrderDetailDto, CustomerServiceProxy, MetalTypeServiceProxy, MetalTypeDto, MetalTypeDtoPagedResultDto, CreateOrderDto } from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './create-order.component.html',
})
export class CreateOrderComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  order = new CreateOrderDto();

  metaltypes = new Array<MetalTypeDto>();

  orderDetails: OrderDetailDto[] = [];

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _orderService: OrderServiceProxy,
    private _customerService : CustomerServiceProxy,
    private _metalTypeService : MetalTypeServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {

    // this._metalTypeService.getAll()
    // .subscribe((result: MetalTypeDtoPagedResultDto) => {
    //   this.metaltypes = result.items;
    // });

  }

  addOrderDetail(e): void{

    e.preventDefault();
    this.orderDetails.push(new OrderDetailDto());
  }


  removeOrderDetail(entry:OrderDetailDto):void{
    // delete this.orderDetails[index];

    const index = this.orderDetails.indexOf(entry, 0);
if (index > -1) {
  this.orderDetails.splice(index, 1);
}
  }

  save(): void {
    this.saving = true;

    const order = new CreateOrderDto();
    order.init(this.order);

    this._orderService
      .create(order)
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
