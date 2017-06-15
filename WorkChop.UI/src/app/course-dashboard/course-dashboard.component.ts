import { Component, OnInit, ViewChild } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { CourseServiceProxy, AlertifyModals, Roles, AssigneeRole } from '../../shared/service-proxies/service-proxies';
import { CourseFilter } from '../../shared/filter/pipe';
import { ModalDirective } from 'ng2-bootstrap/ng2-bootstrap';

@Component({
    selector: 'app-course-dashboard-route',
    templateUrl: './course-dashboard.component.html',
    styleUrls: ['./course-dashboard.component.css', '../app.component.css'],
})
export class CourseDashboardComponent implements OnInit {


    temp: any;
    private courseVM: CourseViewModel;
    public hideEnrolledDiv = true;
    public hideCourses = true;
    courses: any = [];
    loggedInUserId: string;
    private _alertfyVMModel: AlertifyVMModel;
    Enrolled = AssigneeRole.Enrolled;
    Self = AssigneeRole.Self;
    All = AssigneeRole.All;

   

    //For Modal
    @ViewChild('staticModal') public staticModal: ModalDirective;


    constructor(
        private _alertify: AlertifyModals,
        private _courseService: CourseServiceProxy,
        private _router: Router) {

        this.checkIfUserLoggedIn();

        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));
        this.courseVM = new CourseViewModel('');
        this._alertfyVMModel = new AlertifyVMModel('', '', "warning", true, '');

    }

    ngOnInit() {
        this.getCourse(AssigneeRole.All);
    }


    getCourse(assignRoleId: number): void {
        this.hideEnrolledDiv = true;
        this.hideCourses = true;
        this.temp = assignRoleId;
        this._courseService.getAllCourses(assignRoleId)
            .subscribe((result) => {
                this.courses = result;
            });
        if (assignRoleId == AssigneeRole.Enrolled) {
            this.hideEnrolledDiv = false;
        }

        var loggedInUserRole = JSON.parse(localStorage.getItem("userRoles")).roles;
        if (loggedInUserRole != "Teacher") {
            this.hideEnrolledDiv = false;
            this.hideCourses = false;
        }

    }
    openCreateModal(): void {
        this.staticModal.show();
    }
    addNewCourse(): void {
        let _self = this;
        _self.isCourseExists(function (data) {
            if (!data) {
                _self._courseService.addNewCourse(_self.courseVM)
                    .subscribe(result => {
                        if (result) {
                            _self._router.navigate(['/course/setting/:'], { queryParams: { course_id: result.CourseId } });
                            _self.courseVM = new CourseViewModel('');
                        }
                        _self.staticModal.hide();

                    }, error => {
                    });
            } else {
                _self._alertify.SimpleAlert("Course Name already exists");
                _self.courseVM.courseName = "";
            }

        });

    }

    deleteCourse(courseId: string): void {
        let _self = this;
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
                     
                        for (var i = 0; i < _self.courses.length; i++) {
                            if (_self.courses[i].Fk_CourseId == result.CourseId) {
                                _self.courses.splice(i, 1);
                                break;
                            }
                        }


                    }, error => {
                    });
            }
        });


    }

    leaveCourse(userCourseMappingId: string): void {
        let _self = this;
        this._alertfyVMModel.title = "Are you sure want to leave the course?";
        this._alertfyVMModel.text = "";
        this._alertfyVMModel.confirmButtonText = "Leave";
        this._alertify.Confirm(this._alertfyVMModel, function (isConfirmed) {
            if (isConfirmed) {
                _self._courseService.leaveCourse(userCourseMappingId)
                    .subscribe(result => {
                        if (result.HasError) {
                            return;
                        }
                        _self.courses.shift(result);
                    }, error => {
                    });
            }
        });

    }

    getCourseSetting(courseId: string, userType: string): void {

        this._router.navigate(['/course/setting/:'], { queryParams: { course_id: courseId } });

    }

    isCourseExists(callback): void {
        this._courseService.isCourseExist(this.courseVM)
            .subscribe(data => {
                callback(data);
            }, error => {
                callback(false);
            });
    }

  

    checkIfUserLoggedIn(): void {
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser == null || currentUser == undefined) {
            this._router.navigate(['/account/login']);
        }
    }
}

export class CourseViewModel {
    constructor(public courseName: string) { }
}

export class AlertifyVMModel {
    constructor(
        public title: string,
        public text: string,
        public type: string,
        public showCancelButton: boolean,
        public confirmButtonText: string,

    ) { }
}