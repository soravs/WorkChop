import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, OpaqueToken } from '@angular/core';
import { Http, Headers, Response, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Config } from '../../shared/AppConst';
import swal from 'sweetalert2';
import { BlockUI, NgBlockUI } from 'ng-block-ui';


export const API_BASE_URL = Config.getEnvironmentVariable('endPoint');

@Injectable()
export class AuthenticationServiceProxy {
    private token: string;
    private currentUser = JSON.parse(localStorage.getItem('currentUser'));

    constructor(private _http: Http) {
        // set token if saved in local storage
        this.token = this.currentUser && this.currentUser.token;
    }

    login(loginVM: any): Observable<boolean> {
        let url = API_BASE_URL + "/api/user/signin";
        return this._http.request(url, {
            method: 'post',
            body: JSON.stringify(loginVM),
            headers: new Headers({ 'Content-Type': 'application/json' })
        }).map((response: Response) => {
            // login successful if there's a jwt token in the response
            let result = response.json();
            if (result.TokenModel != null) {
                // set token property
                this.token = result.TokenModel.access_token;
                let email = result.Email;
                let userId = result.UserID;
                let userRoles = result.TokenModel.Roles;
                // store username and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify({ email: email, userId: userId, token: this.token }));
                localStorage.setItem('userRoles', JSON.stringify({ roles: userRoles }));
                localStorage.setItem('UserID', JSON.stringify({ userId: userId }));
                return true;
            } else {
                // return false to indicate failed login
                return false;
            }
        }, error => {
        });
    }

    logout(): void {
        // clear token remove user from local storage to log user out
        this.token = null;
        localStorage.removeItem('currentUser');
        localStorage.removeItem('UserID');
        localStorage.removeItem('userRoles');

    }
}

@Injectable()
export class CourseServiceProxy {
    private currentUser = JSON.parse(localStorage.getItem('currentUser'));

    private ApiCalls: APICallsServiceProxy;
    constructor(private _http: Http,private _router:Router) {
        this.ApiCalls = new APICallsServiceProxy(_http, _router);
    }

    getAllCourses(assignRoleId: number): Observable<any> {
        let url_ = API_BASE_URL + "/api/course/getcourses?userId=" + this.currentUser.userId + "&assigneeRoleId=" + assignRoleId;
        return this.ApiCalls.GetAjaxCall(url_);
    }

    isCourseOwner(userId: string, courseId: string): Observable<boolean> {
        let url_ = API_BASE_URL + "/api/course/isCourseOwner?userId=" + userId + "&courseId=" + courseId;
        return this.ApiCalls.GetAjaxCall(url_);
    }
    getCourseById(courseId: string): Observable<any> {
        let url_ = API_BASE_URL + "/api/course/getCourseById?courseId=" + courseId;
        return this.ApiCalls.GetAjaxCall(url_);
    }
    isCourseExist(courseVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/course/isCourseExist";
        courseVM.createdBy = this.currentUser.userId;
        return this.ApiCalls.PostAjaxCall(courseVM, url);
    }

    addNewCourse(courseVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/course/addnewcourse";
        courseVM.createdBy = this.currentUser.userId;
        return this.ApiCalls.PostAjaxCall(courseVM, url);
    }

    deleteCourse(courseId: string): Observable<any> {
        let url = API_BASE_URL + "/api/course/deletecourse?courseId=" + courseId;
        return this.ApiCalls.GetAjaxCall(url);
    }

    leaveCourse(userCourseMappingId: string): Observable<any> {
        let url = API_BASE_URL + "/api/course/leaveCourse?UserCourseMappingId=" + userCourseMappingId;
        return this.ApiCalls.GetAjaxCall(url);
    }

    UpdateCourse(courseVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/course/updateCourse";
        courseVM.createdBy = this.currentUser.userId;
        return this.ApiCalls.PostAjaxCall(courseVM, url);
    }

    getAllTeachers(userId: string): Observable<any> {
        let url_ = API_BASE_URL + "/api/user/getAllTeachers?userId=" + userId;
        return this.ApiCalls.GetAjaxCall(url_);
    }
}


@Injectable()
export class CategoryServiceProxy {
    private currentUser = JSON.parse(localStorage.getItem('currentUser'));

    private ApiCalls: APICallsServiceProxy;
    constructor(private _http: Http, private _router: Router) {
        this.ApiCalls = new APICallsServiceProxy(_http, _router);
    }

    ///Categories functions
    addUpdateCategory(categoryVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/addupdatecategory";
        return this.ApiCalls.PostAjaxCall(categoryVM, url);
    }

    isCategoryExist(categoryVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/iscategoryexist";
        return this.ApiCalls.PostAjaxCall(categoryVM, url);
    }
    getAllCategories(courseId: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/getcategories?courseId=" + courseId + "&userId=" + this.currentUser.userId;
        return this.ApiCalls.GetAjaxCall(url);
    }

    deleteCategory(categoryVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/deletecategory";
        return this.ApiCalls.PostAjaxCall(categoryVM, url);
    }
    /////////

    //////Content functions
    add_updateContent(contentVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/addupdatecontent";
        return this.ApiCalls.PostAjaxCall(contentVM, url);
    }

    isContentExist(contentVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/iscontentexist";
        return this.ApiCalls.PostAjaxCall(contentVM, url);
    }

    deleteContent(contentVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/deletecontent";
        return this.ApiCalls.PostAjaxCall(contentVM, url);
    }

    postFile(formData: any): Observable<any> {
        let url = API_BASE_URL + "/api/content/addupdatecontent";
        return this.ApiCalls.PostAjaxCallToUploadFile(formData, url);

    }
}

@Injectable()
export class CustomExceptionHandlingServiceProxy {
    @BlockUI() blockUI: NgBlockUI;

    constructor(private _http: Http, private _router: Router) {
        
    }
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    public processGetCurrentLoginInformations(response: Response): any {
        const responseText = response.text();
        const status = response.status;
        if (status >= 200 && status <= 299) {
            let result200: any = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            return resultData200;
        }
        else if (status === 401) {
            let errData401 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            if (errData401.Message === "Authorization has been denied for this request.") {
                localStorage.removeItem('currentUser');
                localStorage.removeItem('UserID');
                localStorage.removeItem('userRoles');
                this._router.navigate(['/account/login']);
                return errData401.Message;
            }
        }
        else {
            this.blockUI.stop();
            let errData = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            //this.throwException("An unexpected server error occurred.", status, responseText);
            console.log(errData.Message);
        }
        return null;
    }

    protected throwException(message: string, status: number, response: string, result?: any): any {
        if (result !== null && result !== undefined)
            throw result;
    }

}



export class APICallsServiceProxy {
    @BlockUI() blockUI: NgBlockUI;

    private currentUser = JSON.parse(localStorage.getItem('currentUser'));
    private headers = new Headers({
        "Authorization": "bearer " + (this.currentUser === null ? '' : this.currentUser.token),

    });

    private customExceptionHandling: CustomExceptionHandlingServiceProxy;
    constructor(private _http: Http, private _router: Router) {
        this.customExceptionHandling = new CustomExceptionHandlingServiceProxy(_http,_router);
    }

    GetAjaxCall(url_: any): Observable<any> {
        this.blockUI.start();
        return this._http.request(url_, {
            method: 'get',
            headers: this.headers
        }).map((response) => {

            this.blockUI.stop();
            return this.customExceptionHandling.processGetCurrentLoginInformations(response);
        }).catch((response: any, caught: any) => {

            this.blockUI.stop();
            if (response instanceof Response) {
                try {
                    return Observable.of(this.customExceptionHandling.processGetCurrentLoginInformations(response));
                } catch (e) {
                    return <Observable<any>><any>Observable.throw(e);
                }
            } else
                return <Observable<any>><any>Observable.throw(response);
        });
    }

    PostAjaxCall(dataModel: any, url_: string): Observable<any> {
        this.blockUI.start();
        return this._http.request(url_, {
            method: 'post',
            body: dataModel,
            headers: this.headers
        }).map((response) => {

            this.blockUI.stop();
            return this.customExceptionHandling.processGetCurrentLoginInformations(response);
        }).catch((response: any, caught: any) => {

            this.blockUI.stop();
            if (response instanceof Response) {
                try {
                    return Observable.of(this.customExceptionHandling.processGetCurrentLoginInformations(response));
                } catch (e) {
                    return <Observable<any>><any>Observable.throw(response);
                }
            } else
                return <Observable<any>><any>Observable.throw(response);
        });
    }

    PostAjaxCallToUploadFile(dataModel: any, url_: string): Observable<any> {
        this.blockUI.start();

        let options = new RequestOptions({ headers: this.headers });

        return this._http.post(url_, dataModel, options)
            .map((response) => {

                this.blockUI.stop();
                return this.customExceptionHandling.processGetCurrentLoginInformations(response);
            }).catch((response: any, caught: any) => {

                this.blockUI.stop();
                if (response instanceof Response) {
                    try {
                        return Observable.of(this.customExceptionHandling.processGetCurrentLoginInformations(response));
                    } catch (e) {
                        return <Observable<any>><any>Observable.throw(response);
                    }
                } else
                    return <Observable<any>><any>Observable.throw(response);
            });
    }

}




@Injectable()
export class AlertifyModals {

    Confirm(AlertfyVMModel: any, callback) {
        let status = false;
        swal({
            title: AlertfyVMModel.title,
            text: AlertfyVMModel.text,
            type: AlertfyVMModel.type,
            showCancelButton: true,
            confirmButtonText: AlertfyVMModel.confirmButtonText,
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn btn-danger',
        }).then(function () {
            callback(true);
        }, function (dismiss) {
            callback(false);
        });

    }
    SimpleAlert(message: string) {
        swal(message);
    }
}


export enum Roles {
    Teacher = 1,
    Student = 2,
    Admin = 3
}

export enum AssigneeRole {
    All = 1,
    Enrolled = 2,
    Self = 3
}


export enum FileTypes {
    All = 1,
    Enrolled = 2,
    Self = 3
}




//@Injectable()
//export class AlertfyVMModel {
//    constructor(
//        public title: string,
//        public text: string,
//        public type: string,
//        public showCancelButton: boolean,
//        public confirmButtonText: string,

//    ) { }
//}
