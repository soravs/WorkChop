import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//import { LoginComponent } from './login/Component/login.component';
//import { HomeComponent } from './Home/Component/home.component';
import { AuthGuard } from '../account/_guards/auth.guard';
import { AppComponent } from './app.component'
import {CourseDashboardComponent } from './course-dashboard/course-dashboard.component'

//export const routes: Routes = [
//    {
//        path: '',
//        pathMatch: 'full',
//        component: LoginComponent
//    },
//    {
//        path: 'home',
//        component:HomeComponent
//    }
//];

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'course', component: CourseDashboardComponent, canActivate: [AuthGuard] }
                ]
            }
        ])
    ],
    exports: [RouterModule],
    providers: []
})
export class AppRoutingModule { }
