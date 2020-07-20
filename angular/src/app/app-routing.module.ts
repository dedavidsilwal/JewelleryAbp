import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { MetaltypesComponent } from './metaltypes/metaltypes.component';
import { OrdersComponent } from './orders/orders.component';
import { ProductsComponent } from './products/products.component';
import { CustomersComponent } from './customers/customers.component';
import { InvoicesComponent } from './invoices/invoices.component';
import { NewOrderComponent } from './orders/new-order/new-order.component';
import { SalesComponent } from './sales/sales.component';
import { DuesComponent } from './dues/dues.component';
import { SaleDuesComponent } from './sale-dues/sale-dues.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },
                    { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
                    // tslint:disable-next-line: max-line-length
                    { path: 'metaltypes', component: MetaltypesComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'products', component: ProductsComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'sales', component: SalesComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    {
                        path: 'orders', children: [
                            { path: '', component: OrdersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                        ]
                    },

                    { path: 'customers', component: CustomersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'invoices', component: InvoicesComponent, data: { permission: 'Pages.Orders' }, canActivate: [AppRouteGuard] },
                    { path: 'dues', component: DuesComponent, data: { permission: 'Pages.Orders' }, canActivate: [AppRouteGuard] },
                    { path: 'saledues', component: SaleDuesComponent, data: { permission: 'Pages.Orders' }, canActivate: [AppRouteGuard] },
                    { path: 'update-password', component: ChangePasswordComponent }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
