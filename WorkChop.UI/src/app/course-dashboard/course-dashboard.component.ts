import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { CourseServiceProxy } from '../../shared/service-proxies/service-proxies';

@Component({
    selector: 'app-course-dashboard-route',
    templateUrl: './course-dashboard.component.html',
    styleUrls: ['./course-dashboard.component.css', '../app.component.css']

})

export class CourseDashboardComponent implements OnInit {
    //today = new Date();
    //oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    courses: any = [];
    //createdDate: number;
    loggedInUserId: string;
    constructor(private _courseService: CourseServiceProxy) {
        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));
    }

    ngOnInit() {
        this.getCourse(1);
    }

    getCourse(assignRoleId: number): void {
        this._courseService.getAllCourses(assignRoleId)
            .subscribe((result) => {
                debugger;
                this.courses = result;
            });
    }

}