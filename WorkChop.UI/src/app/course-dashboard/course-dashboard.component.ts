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
    private courseVM: CourseViewModel;
    courses: any = [];
    loggedInUserId: string;
    constructor(private _courseService: CourseServiceProxy) {
        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));
        this.courseVM = new CourseViewModel('');
    }

    ngOnInit() {
        this.getCourse(1);
    }

    getCourse(assignRoleId: number): void {
        this._courseService.getAllCourses(assignRoleId)
            .subscribe((result) => {
                this.courses = result;
            });
    }

    addNewCourse(): void {
        this._courseService.addNewCourse(this.courseVM)
            .subscribe(result => {
                if (result) {
                    debugger;
                } else {
                    debugger;
                }
            }, error => {
                debugger;
            });
    }
}

export class CourseViewModel {
    constructor(public courseName: string) { }
}