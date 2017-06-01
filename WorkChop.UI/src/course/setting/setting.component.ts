import { FormControl } from "@angular/forms";
import { ElementRef, NgZone, NgModule, Component, OnInit, ViewChild } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { CourseServiceProxy } from '../../shared/service-proxies/service-proxies';
import { MapsAPILoader } from 'angular2-google-maps/core';
import { CKEditorModule } from 'ng2-ckeditor';
import { } from '@types/googlemaps';
import { ConstantValues } from '../../shared/AppConst';
import { ModalDirective } from 'ng2-bootstrap/ng2-bootstrap';


export const CourseDefaultImage = ConstantValues.CourseImage;

@Component({
    selector: 'setting-dashboard-route',
    templateUrl: './setting.component.html',
    styleUrls: ['./setting.component.css', '../course.component.css']
})
export class SettingComponent implements OnInit {
    course_id: any;
    user_type: string;

    //For Modal
    @ViewChild('staticModal') public staticModal: ModalDirective;

    //For Ng-AutoCOmplete
    public latitude: number;
    public longitude: number;
    public searchControl: FormControl;
    public zoom: number;

    @ViewChild("search")
    public searchElementRef: ElementRef;
    ///////

    //To Bind Teachers
    dropdownList = [];
    selectedItems = [];
    dropdownSettings = {};
    /////

    successMessage = '';
    errorMessage = '';
    imageErrorMessage = '';

    private courseSettingVM: CourseSettingViewModel;
    loggedInUserId: string;

    constructor( private activatedRoute: ActivatedRoute,
        private mapsAPILoader: MapsAPILoader,
        private ngZone: NgZone, private _courseService: CourseServiceProxy, private _router: Router)
    {
        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));
        this.courseSettingVM = new CourseSettingViewModel('', '', '', '', CourseDefaultImage, '', 0, 0, true);
        this.GetAllTeachers();
       
    }

    ngOnInit() {

        this.dropdownList = [
            { "id": 1, "itemName": "India" },
            { "id": 2, "itemName": "Singapore" },
            { "id": 3, "itemName": "Australia" },
            { "id": 4, "itemName": "Canada" },
            { "id": 5, "itemName": "South Korea" },
            { "id": 6, "itemName": "Germany" },
            { "id": 7, "itemName": "France" },
            { "id": 8, "itemName": "Russia" },
            { "id": 9, "itemName": "Italy" },
            { "id": 10, "itemName": "Sweden" }
        ];
        this.selectedItems = [
            { "id": 2, "itemName": "Singapore" },
            { "id": 3, "itemName": "Australia" },
            { "id": 4, "itemName": "Canada" },
            { "id": 5, "itemName": "South Korea" }
        ];
        this.dropdownSettings = {
            singleSelection: false,
            text: "Select Teachers",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            enableSearchFilter: true
        };  



        //set google maps defaults
        this.zoom = 4;
        this.latitude = 39.8282;
        this.longitude = -98.5795;

        //To Get the course id
        this.activatedRoute.queryParams.subscribe(
            params => this.course_id = params['course_id']);

        this.getCourseById();
    }

    onItemSelect(item) {
        console.log('Selected Item:');
        console.log(item);
    }
    OnItemDeSelect(item) {
        console.log('De-Selected Item:');
        console.log(item);
    }

    //Initialize Auto Complete and Google map
    private InitializeMap() {

        //create search FormControl
        this.searchControl = new FormControl();

        //set current position
        this.setCurrentPosition();

        //load Places Autocomplete

        this.mapsAPILoader.load().then(() => {
            let autocomplete = new google.maps.places.Autocomplete(
                <HTMLInputElement>document.getElementById("address"), {
                    types: ['address']
                });

            autocomplete.addListener('place_changed', () => {
                this.ngZone.run(() => {
                    // get the place result
                    let place: google.maps.places.PlaceResult = autocomplete.getPlace();

                    //verify result
                    if (place.geometry === undefined || place.geometry === null) {
                        return;
                    }

                    //set latitude, longitude and zoom
                    this.latitude = place.geometry.location.lat();
                    this.longitude = place.geometry.location.lng();
                    this.zoom = 11;


                    if (place.formatted_address != undefined && place.formatted_address != null) {
                        this.courseSettingVM.Location = place.formatted_address;
                        this.courseSettingVM.Latitude = this.latitude;
                        this.courseSettingVM.Longitude = this.longitude
                    }
                    else {
                        this.courseSettingVM.Location = "";
                    }
                });
            });
        });
    }

    //To Bind The Map for the first time
    private setCurrentPosition() {
        this.latitude = this.courseSettingVM.Latitude;
        this.longitude = this.courseSettingVM.Longitude;
        this.zoom = 12;
        //if (this.latitude != 0 || this.longitude != 0 )
        //{
        //    if ("geolocation" in navigator) {
        //        navigator.geolocation.getCurrentPosition((position) => {
        //            this.latitude = position.coords.latitude;
        //            this.longitude = position.coords.longitude;
        //            this.zoom = 12;
        //        });
        //    }
        //}

    }

    //Bind the course details on page
    getCourseById(): void {
        this._courseService.getCourseById(this.course_id).subscribe(result => {
            if (result) {
                this.courseSettingVM.CourseId = this.course_id;
                this.courseSettingVM.CourseName = result.CourseName;
                this.courseSettingVM.Description = result.Description;
                this.courseSettingVM.Co_Teacher = result.Co_Teacher;
                this.courseSettingVM.ImageSrc = result.ImageSrc == null ? CourseDefaultImage : result.ImageSrc;
                this.courseSettingVM.Location = result.Location;
                this.courseSettingVM.Latitude = result.Latitude;
                this.courseSettingVM.Longitude = result.Longitude;
            }
            else {
                this.errorMessage = 'No Record Found';
            }
            window.scrollTo(0, 0);
            this.InitializeMap();
        }, error => {
            this.errorMessage = 'No Record Found';
            window.scrollTo(0, 0);
            this.InitializeMap();
        });
    }

    //To Update the course detail in database
    updateCourse(): void {
        this.successMessage = '';
        if (!this.courseSettingVM.Status) {
            this.errorMessage = 'Please fill the course details carefully !';
            window.scroll(0, 0);
            return;
        }
        this._courseService.UpdateCourse(this.courseSettingVM)
            .subscribe(result => {
                if (result) {
                    this.successMessage = "Course updated successfully !";
                    window.scroll(0, 0);
                    this.staticModal.show();
                    return;
                } else {
                    this.errorMessage = 'Please try again !';
                    window.scroll(0, 0);
                }
            }, error => {
                this.errorMessage = error.json().Message;
                window.scroll(0, 0);
            });
    }

    //On Uploading the Course Image Validate Image and display uploaded image
    upload(event): void {
        this.courseSettingVM.Status = true;
        let _self = this;

        _self.imageErrorMessage = "";
        var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.png|.jpeg)$");
        if (event.length > 0 && regex.test(event[0].type.toLowerCase())) {

            var reader = new FileReader();
            reader.readAsDataURL(event[0]);
            reader.onload = function (e) {

                var image = new Image();

                //Set the Base64 string return from FileReader as source.
                image.src = this.result;

                //Validate the File Height and Width.
                image.onload = function () {

                    var height = image.height;
                    var width = image.width;
                    if (height > 600 || width > 400) {
                        _self.imageErrorMessage = "Choose the image in correct dimention. Otherwise Previous Image will be used to save.";
                       // _self.courseSettingVM.Status = false;
                    }
                    else {
                        _self.courseSettingVM.ImageSrc = image.src;
                        _self.imageErrorMessage = "";
                        _self.courseSettingVM.Status = true;
                    }

                };

            }
        }
        else if (event.length > 0) {
            _self.imageErrorMessage = "Please Upload the file in Correct format. Otherwise Previous Image will be used to save.";
            //_self.courseSettingVM.Status = false;
        }
    }

    //To Bind all the teachers 
    GetAllTeachers(): void {
        this._courseService.getAllTeachers(this.loggedInUserId["userId"]).subscribe(result => {
            if (result) {
                debugger;
            }
            else {

            }

        }, error => {

        });
    }

}



//Common Classes
export class CourseSettingViewModel {
    constructor(
        public CourseId: string,
        public CourseName: string,
        public Description: string,
        public Co_Teacher: string,
        public ImageSrc: string,
        public Location: string,
        public Latitude: number,
        public Longitude: number,
        public Status: boolean
    ) { }
}