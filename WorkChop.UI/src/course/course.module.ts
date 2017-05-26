import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { CourseRoutingModule } from './course-routing.module';
import { CourseComponent } from './course.component';
import { CommonModule } from '@angular/common';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';
import { SettingComponent } from './setting/setting.component';

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
    ],
    providers: [
        AppRouteGuard

    ],
    bootstrap: [CourseComponent]
})
export class CourseModule {

}