import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { CourseServiceProxy,AlertifyModals } from '../../shared/service-proxies/service-proxies';
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
    private _alertfyVMModel: AlertfyVMModel;

    constructor(
        private _alertify: AlertifyModals,
        private _courseService: CourseServiceProxy,
        private _router: Router) {
        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));
        this.courseVM = new CourseViewModel('');
        this._alertfyVMModel = new AlertfyVMModel('','',"warning",true,'');
        
    }

    ngOnInit() {
        this.getCourse(1);
    }
    

    getCourse(assignRoleId: number): void {
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
                    this._router.navigate(['/course/setting/:'], { queryParams: { course_id: result.CourseId} });
                    this.courseVM = new CourseViewModel('');
                } else {
                }
            }, error => {
            });
    }

    deleteCourse(courseId: string): void {
        var _self = this;
        this._alertfyVMModel.title = "Are you sure want to delete the course?";
        this._alertfyVMModel.text = "You won't be able to revert this!";
        this._alertfyVMModel.confirmButtonText = "Delete";
         this._alertify.Confirm(this._alertfyVMModel, function (isConfirmed) {
            if (isConfirmed) {
                _self._courseService.deleteCourse(courseId)
                    .subscribe(result => {
                        if (result.HasError) {
                            return;
                        }
                        _self.getCourse(_self.temp);
                    }, error => {
                    });
            }
        });
      
        
    }

    leaveCourse(userCourseMappingId: string): void {
        var _self = this;
        this._alertfyVMModel.title = "Are you sure want to leave the course?";
        this._alertfyVMModel.text = "";
        this._alertfyVMModel.confirmButtonText = "Delete";
        this._alertify.Confirm(this._alertfyVMModel, function (isConfirmed) {
            if (isConfirmed) {
                _self._courseService.leaveCourse(userCourseMappingId)
                    .subscribe(result => {
                        if (result.HasError) {
                            return;
                        }
                        _self.getCourse(_self.temp);
                    }, error => {
                    });
            }
        });
      
    }

    getCourseSetting(courseId: string, userType: string): void {
        debugger;
        this._router.navigate(['/course/setting/:'], { queryParams: { course_id: courseId } });
        //this._router.navigate(['SettingComponent', { course_id: courseId, user_type: userType }]);
    }

}

export class CourseViewModel {
    constructor(public courseName: string) { }
}

export class AlertfyVMModel {
    constructor(
        public title: string,
        public text: string,
        public type: string,
        public showCancelButton: boolean,
        public confirmButtonText: string,

    ) { }
}