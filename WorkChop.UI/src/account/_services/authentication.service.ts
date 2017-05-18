import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'

@Injectable()
export class AuthenticationService {
    public token: string;
    private url: string = '';

    constructor(private http: Http) {
        // set token if saved in local storage
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
        this.url = "http://localhost:53027";
    }

    login(loginVM: any): Observable<boolean> {
        return this.http.request(this.url + "/api/user/SignIn", {
            method: 'post',
            body: JSON.stringify(loginVM),
            headers: new Headers({ 'Content-Type': 'application/json' })
        }).map((response: Response) => {
            // login successful if there's a jwt token in the response
            let result = response.json();
            // let token = response.json() && response.json().TokenModel.access_token;
            if (result.TokenModel != null) {
                // set token property
                this.token = result.TokenModel.access_token;
                let username = result.TokenModel.email;
                // store username and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify({ username: username, token: this.token }));
                return true;
            } else {
                // return false to indicate failed login
                return false;
            }
        }, error => {
            debugger;
        });
    }

    logout(): void {
        // clear token remove user from local storage to log user out
        this.token = null;
        localStorage.removeItem('currentUser');
    }
}