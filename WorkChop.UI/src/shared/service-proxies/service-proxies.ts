import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, OpaqueToken } from '@angular/core';
import { Http, Headers, Response, RequestOptionsArgs } from '@angular/http';
import { Config } from '../../shared/AppConst';

export const API_BASE_URL = Config.getEnvironmentVariable('endPoint');

@Injectable()
export class CourseServiceProxy {
    protected jsonParseReviver: (key: string, value: any) => any = undefined;
    
    constructor(private _http: Http) { 
    }

    getAllCourses(assignRoleId: number): Observable<any> {
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        let url_ = API_BASE_URL + "/api/course/getcourses?userId=" + currentUser.userId + "&assigneeRoleId=" + assignRoleId;
        return this._http.request(url_, {
            method: "get",
            headers: new Headers({
                "Authorization": "bearer " + currentUser.token
            })
        }).map((response) => {
            return this.processGetCurrentLoginInformations(response);
            }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                   return Observable.of(this.processGetCurrentLoginInformations(response));
                } catch (e) {
                    return <Observable<any>><any>Observable.throw(e);
                }
            } else
                return <Observable<any>><any>Observable.throw(response);
        });
    }

    protected processGetCurrentLoginInformations(response: Response): any {
        const responseText = response.text();
        const status = response.status;

        if (status === 201) {
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
        else if (status === 200 && status < 300) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    protected throwException(message: string, status: number, response: string, result?: any): any {
        if (result !== null && result !== undefined)
            throw result;
    }

}

