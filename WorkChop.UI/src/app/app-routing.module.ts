import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CourseDashboardComponent } from './course-dashboard/course-dashboard.component';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'course', component: CourseDashboardComponent, canActivate: [AppRouteGuard] }
                ]
            }
        ])
    ],
    exports: [RouterModule],
    providers: []
})
export class AppRoutingModule { }
