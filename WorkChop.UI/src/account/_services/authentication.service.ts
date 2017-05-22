import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs';
import { Config } from '../../shared/AppConst';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthenticationService {
    public token: string;
    private url: string = '';

    constructor(private http: Http) {
        // set token if saved in local storage
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
        this.url = Config.getEnvironmentVariable('endPoint');
    }

    login(loginVM: any): Observable<boolean> {
        return this.http.request(this.url + "api/user/signin", {
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
                localStorage.setItem('UserID', JSON.stringify({userId: userId}));
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