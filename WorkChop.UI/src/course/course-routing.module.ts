import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CourseComponent } from './course.component';
import { SettingComponent } from './setting/setting.component';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: CourseComponent,
                children: [
                    { path: 'setting', component: SettingComponent, canActivate: [AppRouteGuard] },
                ]
            }
        ])
    ],
    exports: [RouterModule],
    providers: []
})
export class CourseRoutingModule { }