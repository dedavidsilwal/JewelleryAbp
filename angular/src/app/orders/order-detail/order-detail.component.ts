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
import { OrderServiceProxy, CustomerOrderDisplayDto } from '@shared/service-proxies/service-proxies';
import * as jspdf from 'jspdf';

import html2canvas from 'html2canvas';
@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})

export class OrderDetailComponent extends AppComponentBase implements OnInit {

  id: string;
  loading = new EventEmitter<boolean>();

  order: CustomerOrderDisplayDto;
  todayDateString: string = new Date().toDateString();

  AmountDue: number;

  constructor(
    private _orderService: OrderServiceProxy,
    injector: Injector,
    public bsModalRef: BsModalRef,
  ) {

    super(injector);
  }

  ngOnInit(): void {
    this._orderService.fetchOrderDetail(this.id)
      .subscribe(result => {
        this.order = result;
      });
  }


  downloadAsPDF(): void {

    const data = document.getElementById('invoice');
    html2canvas(data).then(canvas => {
      // Few necessary setting options  
      const imgWidth = 208;
      const pageHeight = 295;
      const imgHeight = canvas.height * imgWidth / canvas.width;
      const heightLeft = imgHeight;

      const contentDataURL = canvas.toDataURL('image/png')
      const pdf = new jspdf('p', 'mm', 'a4'); // A4 size page of PDF  
      const position = 0;
      pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight)
      pdf.save(this.order.customerName + ' ' + this.todayDateString); // Generated PDF   
    });
  }

}
