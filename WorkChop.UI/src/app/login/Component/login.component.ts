import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';

import { AuthenticationService } from '../../_services/authentication.service'

@Component({
    selector: 'app-login-route',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']

})

export class LoginComponent implements OnInit {

    private loginVM: LoginViewModel;
    error = '';

    constructor(
        private _http: Http,
        private _router: Router,
        private _authenticationService: AuthenticationService) {
        this.loginVM = new LoginViewModel('','' );
    }

    ngOnInit() {
        // reset login status
        this._authenticationService.logout();
    }

    login() {
        debugger;
        this._authenticationService.login(this.loginVM.username, this.loginVM.password)
            .subscribe(result => {
                debugger;
                if (result === true) {
                     this._router.navigate(['home']);

                } else {
                    this.error = 'Username or password is incorrect';
                }
            }, error => {
                debugger;
                this.error = error.json().Message;
            });
    }
}

export class LoginViewModel {
    constructor(public username: string, public password: string) { }
}
