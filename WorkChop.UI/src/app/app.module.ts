import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthGuard } from '../account/_guards/auth.guard';
//import { LoginComponent } from './login/Component/login.component';
//import { AuthenticationService } from './_services/authentication.service';
//import { HomeComponent } from './Home/Component/home.component';
//import { UserService } from './Home/Service/user.service';
import { CourseDashboardComponent } from './course-dashboard/course-dashboard.component'

@NgModule({
    declarations: [
        AppComponent,
        CourseDashboardComponent
        //LoginComponent,
        //HomeComponent
    ],
    imports: [
        FormsModule,
        HttpModule,
        AppRoutingModule
    ],
    providers: [
        AuthGuard,
        //AuthenticationService,
        //UserService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
