import { NgModule,Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
@Component({
    selector: 'setting-dashboard-route',
    templateUrl: './setting.component.html',
    styleUrls: ['./setting.component.css', '../course.component.css']

})
export class SettingComponent implements OnInit {

    private courseVM: CourseViewModel;
    courses: any = [];
    loggedInUserId: string;
    constructor() {
        debugger;
        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));
        this.courseVM = new CourseViewModel('');
    }

    ngOnInit() {
        debugger;
    }

  
}

export class CourseViewModel {
    constructor(public courseName: string) { }
}