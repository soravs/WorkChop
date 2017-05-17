import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'

@Injectable()
export class AuthenticationService {
    public token: string;

    constructor(private http: Http) {
        // set token if saved in local storage
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
    }

    login(username: string, password: string): Observable<boolean> {
        let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        let options = new RequestOptions({ headers: headers });
        let body = "UserName=" + username + "&Password=" + password;
        let url = 'http://localhost:53027/api/User/SignIn';
        return this.http.post(url, body, options)
            .map((response: Response) => {
                debugger;
                // login successful if there's a jwt token in the response
                let result = response.json();
               // let token = response.json() && response.json().TokenModel.access_token;
                if (result.TokenModel != null) {
                    // set token property
                    this.token = result.TokenModel.access_token;
                    let username = result.TokenModel.email;
                    // store username and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify({ username: username, token:this.token }));
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