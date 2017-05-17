import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';

@Component({
    selector: 'app-course-dashboard-route',
    templateUrl: './course-dashboard.component.html',
    styleUrls: ['./course-dashboard.component.css']

})

export class CourseDashboardComponent implements OnInit {

    constructor(
    ) {
    }

    ngOnInit() {
        // reset login status
    }

}