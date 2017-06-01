import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { CourseRoutingModule } from './course-routing.module';
import { CourseComponent } from './course.component';
import { CommonModule } from '@angular/common';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';
import { SettingComponent } from './setting/setting.component';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';
import { AgmCoreModule } from "angular2-google-maps/core";
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown/angular2-multiselect-dropdown';
import { CKEditorModule, CKButtonDirective } from 'ng2-ckeditor';
import { ModalModule } from 'ng2-bootstrap';

@NgModule({
    declarations: [
         CourseComponent,
        SettingComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        CourseRoutingModule,
        ServiceProxyModule,
        AgmCoreModule.forRoot({
            apiKey: "AIzaSyA4KEetv4Mjtar6f-ioisyXcJ65HDjuqCY",
            libraries: ["places"]
        }),
        AngularMultiSelectModule,
        CKEditorModule,
        ModalModule.forRoot(),
    ],
    providers: [
        AppRouteGuard

    ],
    
    bootstrap: [CourseComponent]
})
export class CourseModule {

}