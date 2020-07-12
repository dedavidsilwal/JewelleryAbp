import { Component, Injector, ChangeDetectionStrategy, OnInit, NgModule } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { OrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { OrderDto, SaleServiceProxy, SalesReportDashboard, OrderDashboardDto } from '../../shared/service-proxies/service-proxies';

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends AppComponentBase implements OnInit {


  orders: OrderDashboardDto[];
  sales: SalesReportDashboard[];

  totalActiveOrder = 0;
  totalSale = 0;

  constructor(injector: Injector,
    private _orderService: OrderServiceProxy,
    private _saleService: SaleServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {

    this._saleService.recentSale()
    .subscribe((result: SalesReportDashboard[]) => {
      this.sales = result;
    });

    this._orderService.nearOrderDeliver()
      .subscribe((result: OrderDashboardDto[]) => {
        this.orders = result;
      });

    this._orderService.totalOrderCount()
      .subscribe((result: number) => {
        this.totalActiveOrder = result;
      });

    this._saleService.totalSaleCount()
      .subscribe((result: number) => {
        this.totalSale = result;
      });


  }


}
