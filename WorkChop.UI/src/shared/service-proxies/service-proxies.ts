import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, OpaqueToken } from '@angular/core';
import { Http, Headers, Response, RequestOptionsArgs } from '@angular/http';
import { Config } from '../../shared/AppConst';

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
                // store username and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify({ email: email, userId: userId, token: this.token }));
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
    }
}

@Injectable()
export class CourseServiceProxy {
    private currentUser = JSON.parse(localStorage.getItem('currentUser'));
    private headers = new Headers({
        "Authorization": "bearer " + (this.currentUser === null ? '' : this.currentUser.token)
    });
    
    private customExceptionHandling: CustomExceptionHandlingServiceProxy;
    constructor(private _http: Http) {
        this.customExceptionHandling = new CustomExceptionHandlingServiceProxy();
    }

    getAllCourses(assignRoleId: number): Observable<any> {
        let url_ = API_BASE_URL + "/api/course/getcourses?userId=" + this.currentUser.userId + "&assigneeRoleId=" + assignRoleId;
        return this._http.request(url_, {
            method: 'get',
            headers: this.headers
        }).map((response) => {
            return this.customExceptionHandling.processGetCurrentLoginInformations(response);
        }).catch((response: any, caught: any) => {
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

    addNewCourse(courseVM: any): Observable<any> {
        let url = API_BASE_URL + "/api/course/addnewcourse";
        courseVM.createdBy = this.currentUser.userId;
        return this._http.request(url, {
            method: 'post',
            body: courseVM,
            headers: this.headers
        }).map((response) => {
            return this.customExceptionHandling.processGetCurrentLoginInformations(response);
            }).catch((response: any, caught: any) => {
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

    deleteCourse(courseId: string): Observable<any> {
        let url = API_BASE_URL + "/api/course/deletecourse?courseId=" + courseId;
        
        return this._http.request(url, {
            method: 'get',
            headers: this.headers
        }).map((response) => {
            return this.customExceptionHandling.processGetCurrentLoginInformations(response);
        }).catch((response: any, caught: any) => {
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

    leaveCourse(userCourseMappingId: string): Observable<any> {
        let url = API_BASE_URL + "/api/course/leaveCourse?UserCourseMappingId=" + userCourseMappingId;

        return this._http.request(url, {
            method: 'get',
            headers: this.headers
        }).map((response) => {
            return this.customExceptionHandling.processGetCurrentLoginInformations(response);
        }).catch((response: any, caught: any) => {
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

export class CustomExceptionHandlingServiceProxy {
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
                return errData401.Message;
            }
        }
        else {
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

