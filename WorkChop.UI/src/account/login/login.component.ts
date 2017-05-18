import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
    selector: 'app-login-route',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']

})

export class LoginComponent implements OnInit {

    private loginVM: LoginViewModel;
    errorMessage = '';

    constructor(
        private _http: Http,
        private _router: Router,
        private _authenticationService: AuthenticationService) {
        this.loginVM = new LoginViewModel('','');
    }

    ngOnInit() {
        // reset login status
        this._authenticationService.logout();
    }

    login() {
        this._authenticationService.login(this.loginVM)
            .subscribe(result => {
                if (result) {
                     this._router.navigate(['/app/course']);

                } else {
                    this.errorMessage = 'Username or password is incorrect';
                }
            }, error => {
                this.errorMessage = error.json().Message;
            });
    }
}

export class LoginViewModel {
    constructor(public userName: string, public password: string) { }
}
