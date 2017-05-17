import { Component, OnInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../_services/authentication.service'


import { UserService } from '../Service/user.service';

@Component({
    templateUrl: './home.component.html',
  
})


export class HomeComponent implements OnInit {
    debugger;
    users: any[] = [];
    constructor(private userService: UserService) { }

    ngOnInit() {
        debugger;
         //get users from secure api end point
        this.userService.getUsers()
            .subscribe(users => {
                this.users = users;
            });
    }
    
}