import { Component, OnInit, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import '../../scripts/course.js';
@Component({
    templateUrl: './course.component.html',
    styleUrls: ['./course.component.css'],

})
export class CourseComponent implements OnInit {

    course_id: any;

    isSubmenuOne: boolean = false;
    isSubmenuTwo: boolean = false;
    isSubmenuThree: boolean = false;
    isSubmenuFour: boolean = false;
    isSubmenuFive: boolean = false;

    isMainmenuOne: boolean = false;
    isMainmenuTwo: boolean = false;
    isMainmenuThree: boolean = false;



    public constructor(private element: ElementRef, private activatedRoute: ActivatedRoute, private _router: Router
    ) {
        this.removeActiveFromMenus();
        if (_router.url.match("/course/setting")) { this.isMainmenuOne = true; }
        else if (_router.url.match("/course/content")) { this.isSubmenuTwo = true; }
    }

    ngOnInit(): void {
        //To Get the course id
        this.activatedRoute.queryParams.subscribe(
            params => this.course_id = params['course_id']);

    }

    navigateToCourseContent(): void {
        this.removeActiveFromMenus();
        this.isSubmenuTwo = true;
        this._router.navigate(['/course/content/:'], { queryParams: { course_id: this.course_id } });
    }
    navigateToCourseSettings(): void {
        this.removeActiveFromMenus();
        this.isMainmenuOne = true;
        this._router.navigate(['/course/setting/:'], { queryParams: { course_id: this.course_id } });
    }

    removeActiveFromMenus(): void {
        this.isSubmenuOne = false;
        this.isSubmenuTwo = false;
        this.isSubmenuThree = false;
        this.isSubmenuFour = false;
        this.isSubmenuFive = false;
        this.isMainmenuOne = false;
        this.isMainmenuTwo = false;
        this.isMainmenuThree = false;
    }

}