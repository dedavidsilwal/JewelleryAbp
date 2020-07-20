import { Component, Injector, ChangeDetectionStrategy, OnInit, NgModule } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { OrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { OrderDto, SaleServiceProxy, SalesReportDashboard, OrderDashboardDto, InvoiceServiceProxy } from '../../shared/service-proxies/service-proxies';

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
})
export class HomeComponent extends AppComponentBase implements OnInit {


  orders: OrderDashboardDto[];
  sales: SalesReportDashboard[];

  pendingOrder = 0;
  TodayOrderneedDeliver = 0;
  TodayOrderTook = 0;
  todaySale = 0;
  todayInvoice = 0;
  todayAdvanceTaken = 0;
  todaySaleAmount = 0;

  constructor(injector: Injector,
    private _orderService: OrderServiceProxy,
    private _saleService: SaleServiceProxy,
    private _invoiceService: InvoiceServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {

    this._orderService.nearOrderDeliver()
      .subscribe((result: OrderDashboardDto[]) => {
        this.orders = result;
        console.log(this.orders);
      });

    this._saleService.recentSale()
      .subscribe((result: SalesReportDashboard[]) => {
        this.sales = result;
      });

    this._orderService.activeOrderCount()
      .subscribe((result: number) => {
        this.pendingOrder = result;
      });

    this._orderService.todayOrderPendingDeliveryCount()
      .subscribe((result: number) => {
        this.TodayOrderneedDeliver = result;
      });

    this._orderService.todayTookOrderCount()
      .subscribe((result: number) => {
        this.TodayOrderTook = result;
      });

    this._orderService.todayAdvanceTaken()
      .subscribe((result: number) => {
        this.todayAdvanceTaken = result;
      });

    this._saleService.todaySaleCount()
      .subscribe((result: number) => {
        this.todaySale = result;
      });
    this._saleService.todaySaleAmount()
      .subscribe((result: number) => {
        this.todaySaleAmount = result;
      });

    this._invoiceService.todayInvoicedAmount()
      .subscribe((result: number) => {
        this.todayInvoice = result;
      });
  }


}
