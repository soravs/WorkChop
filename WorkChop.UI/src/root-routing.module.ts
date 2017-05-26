﻿import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: '/app/course', pathMatch: 'full' },
    {
        path: 'account',
        loadChildren: 'account/account.module#AccountModule', //Lazy load account module
        data: { preload: true }
    },
    {
        path: 'app',
        loadChildren: 'app/app.module#AppModule', //Lazy load app module
        data: { preload: true }
    },
    {
        path: 'course',
        loadChildren: 'course/course.module#CourseModule', //Lazy load course module
        data: { preload: true }
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class RootRoutingModule { }