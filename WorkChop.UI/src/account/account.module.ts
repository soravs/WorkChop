import { NgModule} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule} from '@angular/http';
import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { LoginComponent } from './login/login.component';
import { AuthenticationService } from './_services/authentication.service';
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
        AuthenticationService
    ]
})
export class AccountModule {

}