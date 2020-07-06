import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { OrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { OrderDto } from '../../shared/service-proxies/service-proxies';

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends AppComponentBase implements OnInit {


  orders: OrderDto[];

  constructor(injector: Injector,
    private _orderService: OrderServiceProxy
  ) {
    super(injector);
  }


  ngOnInit() {

    this._orderService.nearOrderDeliver()
      .subscribe((result: OrderDto[]) => {
        this.orders = result;
      });


  }


}
