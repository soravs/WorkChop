import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { CourseServiceProxy } from '../../shared/service-proxies/service-proxies';
import { FilterPipe } from '../../shared/filter/pipe';

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
                    // Need to redirect to the course setting page
                    this.getCourse(1);
                    this.courseVM = new CourseViewModel('');
                } else {
                }
            }, error => {
            });
    }

    deleteCourse(courseId: string): void {
        this._courseService.deleteCourse(courseId)
            .subscribe(result => {
                if (result.HasError) {
                    alert(result.ErrorMessage);
                    return;
                }
                this.getCourse(1);
            }, error => {
            });
    }

}

export class CourseViewModel {
    constructor(public courseName: string) { }
}