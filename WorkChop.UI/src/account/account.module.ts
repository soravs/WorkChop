import { NgModule} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule} from '@angular/http';
import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { LoginComponent } from './login/login.component';
import { AuthenticationServiceProxy } from '../shared/service-proxies/service-proxies';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        AccountRoutingModule,
    ],
    declarations: [
        AccountComponent,
        LoginComponent,
    ],
    providers: [
        AuthenticationServiceProxy
    ]
})
export class AccountModule {

}