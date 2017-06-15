import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CourseDashboardComponent } from './course-dashboard/course-dashboard.component';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module'
import { ModalModule } from 'ng2-bootstrap';
import { CourseFilter } from '../shared/filter/pipe';
import { TinymceModule } from 'angular2-tinymce';
import { BlockUIModule } from 'ng-block-ui';

@NgModule({
    declarations: [
        AppComponent,
        CourseDashboardComponent,
        CourseFilter,
       
    ],   
    imports: [
        FormsModule,
        CommonModule,
        HttpModule,
        AppRoutingModule,
        ServiceProxyModule,
        ModalModule.forRoot(),
        TinymceModule.withConfig({
        }),
        BlockUIModule
    ],
    providers: [
        AppRouteGuard,
        
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
