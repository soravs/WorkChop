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
    temp: any;
    private courseVM: CourseViewModel;
    public hideEnrolledDiv = true;
    courses: any = [];
    loggedInUserId: string;
   
    constructor(private _courseService: CourseServiceProxy, private _router: Router) {
        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));
        this.courseVM = new CourseViewModel('');
    }

    ngOnInit() {
        this.getCourse(1);
    }
    

    getCourse(assignRoleId: number): void {
        debugger;
        this.hideEnrolledDiv = true;
        this.temp = assignRoleId;
        this._courseService.getAllCourses(assignRoleId)
            .subscribe((result) => {
                this.courses = result;
            });
        if (assignRoleId == 2) {
            this.hideEnrolledDiv = false;
        }
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
       
        var result = confirm("Are you sure want to delete the course?");
        if (result) {
            this._courseService.deleteCourse(courseId)
                .subscribe(result => {
                    if (result.HasError) {
                        alert(result.ErrorMessage);
                        return;
                    }
                    this.getCourse(this.temp);
                }, error => {
                });
        }

       
    }

    leaveCourse(userCourseMappingId: string): void {
        var result = confirm("Are you sure want to leave the course?");
        if (result) {
            this._courseService.leaveCourse(userCourseMappingId)
                .subscribe(result => {
                    if (result.HasError) {
                        alert(result.ErrorMessage);
                        return;
                    }
                    this.getCourse(this.temp);
                }, error => {
                });
        }
      
    }

    getCourseSetting(courseId: string): void {
        debugger;
        this._router.navigate(['/course/setting']);
    }

}

export class CourseViewModel {
    constructor(public courseName: string) { }
}