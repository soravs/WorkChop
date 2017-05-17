import { NgModule} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule} from '@angular/http';
import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { AuthenticationService } from './_services/authentication.service';

@NgModule({
    imports: [
        FormsModule,
        HttpModule,
        AccountRoutingModule,
    ],
    declarations: [
        AccountComponent,
        LoginComponent,
    ],
    providers: [
        AuthGuard,
        AuthenticationService,
    ]
})
export class AccountModule {

}