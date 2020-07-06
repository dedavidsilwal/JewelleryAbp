import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import { OrderServiceProxy, EditOrderDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})

export class OrderDetailComponent extends AppComponentBase implements OnInit {

  id: string;
  loading = new EventEmitter<boolean>();

  order: EditOrderDto;


  constructor(
    private _orderService: OrderServiceProxy,
    injector: Injector,
    public bsModalRef: BsModalRef,
  ) {

    super(injector);
  }

  ngOnInit(): void {
    this._orderService.fetchOrderWithDetails(this.id)
      .subscribe(result => {
        this.order = result;
      });
  }

}
