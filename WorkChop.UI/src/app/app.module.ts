import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CourseDashboardComponent } from './course-dashboard/course-dashboard.component';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';
import { ModalModule } from 'ngx-bootstrap';

@NgModule({
    declarations: [
        AppComponent,
        CourseDashboardComponent
    ],
    imports: [
        FormsModule,
        HttpModule,
        AppRoutingModule,
        ModalModule.forRoot()
    ],
    providers: [
        AppRouteGuard
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
