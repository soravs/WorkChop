import { FormControl } from "@angular/forms";
import { ElementRef, NgModule, Component, OnInit, ViewChild, ViewChildren, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CourseServiceProxy, CategoryServiceProxy, AlertifyModals, Roles, AssigneeRole, API_BASE_URL } from '../../shared/service-proxies/service-proxies';
import { ModalDirective } from 'ng2-bootstrap/ng2-bootstrap';
import { ContentFilterPipe } from '../../shared/filter/CommonFilterPipe';
import { Debounce } from 'angular2-debounce';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/debounceTime';
import { Config } from '../../shared/AppConst';

@Component({
    selector: 'content-dashboard-route',
    templateUrl: './content.component.html',
    styleUrls: ['./content.component.css', '../course.component.css']
})


export class ContentComponent implements OnInit {

    baseUrl: string = API_BASE_URL;
    categories: any = [];
    course_id: any;
    category_id: any;
    isOwner: boolean;
    toUpdateContent: boolean = false; //To handle if content is updated or newly added
    toUpdateCategory: boolean = false; // To handle content if content is updated or newly added
    previousCategoryName: string;
    fileErrMsg: boolean = false;
    isFileUploaded: boolean = false;// TO check if file is uploaded on Add file then show name of file 

    private categoryVM: CategoryViewModel;
    private contentVM: ContentViewModel;

    private _alertfyVMModel: AlertfyVMModel;
    //For Modal
    @ViewChild('staticModalCategory') public staticModalCategory: ModalDirective;
    @ViewChild('staticModalContent') public staticModalContent: ModalDirective;
    @ViewChildren('category_name') vc;

    loggedInUserId: string;

    constructor(
        private _renderer: Renderer,
        private _elementRef: ElementRef,
        private _alertify: AlertifyModals,
        private activatedRoute: ActivatedRoute,
        private _courseService: CourseServiceProxy,
        private _CategoryService: CategoryServiceProxy,
        private _router: Router) {
        this.loggedInUserId = JSON.parse(localStorage.getItem('UserID'));

        this._alertfyVMModel = new AlertfyVMModel('', '', "warning", true, '');
        this.checkIfUserLoggedIn();
    }

    ngOnInit() {

        //To Get the course id
        this.activatedRoute.queryParams.subscribe(
            params => this.course_id = params['course_id']);

        this.isCourseOwner();

        this.categoryVM = new CategoryViewModel('', this.course_id, "");
        this.contentVM = new ContentViewModel(this.course_id, '', '', '', '', '', '', false, false, null)


        /////// 
        this.getCourseDetail();
        this.getCategories();

    }

    ngAfterViewInit() {
     
    }
    //To display course name on the top
    getCourseDetail(): void {
        this._courseService.getCourseById(this.course_id).subscribe(result => {
            if (result) {
                document.getElementById("SelectedCourseName").innerText = result.CourseName + " Course";
            }

        });
    }


    //To Check if Course if for owner or enrolled
    isCourseOwner(): void {
        this._courseService.isCourseOwner(this.loggedInUserId["userId"], this.course_id).subscribe(result => {

            this.isOwner = result;

            var toDeleteSection = document.getElementById('owner_Menu_Section');
            if (!result && toDeleteSection != null)
                toDeleteSection.remove();

        });
    }

    //To open the Create category modal
    OpenCreateModal(): void {
        //let inputField: HTMLElement = <HTMLElement>document.querySelectorAll('.category_modal input')[0];
        //debugger;
        // inputField.focus();

        this.toUpdateCategory = false;
        this.categoryVM.CategoryId = "";
        this.categoryVM.CategoryName = '';
        this.staticModalCategory.show();
        //document.getElementById('category_name').focus();
    }

    //To add new category in database corrosponding to course
    addUpdateCategory(): void {
        let _self = this;
        this.isCategoryExists(function (data) {
            if (!data) {
                _self._CategoryService.addUpdateCategory(_self.categoryVM)
                    .subscribe(result => {
                        _self.staticModalCategory.hide();
                        if (!_self.toUpdateCategory) {
                            _self.categories.unshift(result);
                        }
                        _self.categoryVM.CategoryName = "";
                    }, error => {
                    });
            } else {
                _self._alertify.SimpleAlert("Category Name already exists in this course.");
            }
        });

    }

    updateCategoryName(categoryId: string, categoryName: any): void {
        if (this.previousCategoryName == categoryName.trim()) {
            return;
        }
        else if (categoryName == "") {
            this._alertify.SimpleAlert("Please enter name of category to save!");
            return;
        }
        this.previousCategoryName = categoryName.trim();
        this.toUpdateCategory = true;
        this.categoryVM.CategoryId = categoryId;
        this.categoryVM.CategoryName = categoryName;
        this.addUpdateCategory();
    }

    //To set the selected category name to update the category
    setCategoryName(event: any): void {
        this.previousCategoryName = event.target.value.trim();
    }

    //Check if Category exists in database
    isCategoryExists(callback): void {
        this._CategoryService.isCategoryExist(this.categoryVM)
            .subscribe(data => {
                callback(data);
            }, error => {
                callback(false);
            });
    }

    deleteCategory(categoryId: string): void {
        this.categoryVM.CategoryId = categoryId;
        let _self = this;
        this._alertfyVMModel.title = "Are you sure want to delete this categoty and its contents ?";
        this._alertfyVMModel.text = "You won't be able to revert this!";
        this._alertfyVMModel.confirmButtonText = "Delete";
        this._alertify.Confirm(this._alertfyVMModel, function (isConfirmed) {
            if (isConfirmed) {
                _self._CategoryService.deleteCategory(_self.categoryVM)
                    .subscribe(result => {
                        if (result.HasError) {
                            _self._alertify.SimpleAlert(result.ErrorMessage);
                            return;
                        }

                        for (var i = 0; i < _self.categories.length; i++) {
                            if (_self.categories[i].CategoryId == result.CategoryId) {
                                _self.categories.splice(i, 1);
                                break;
                            }
                        }
                    }, error => {
                    });
            }
        });



    }

    //To show all the categories and contents on the page
    getCategories(): void {
        this._CategoryService.getAllCategories(this.course_id)
            .subscribe((result) => {
                this.categories = result;
                console.log(result)
            });
    }

    // to open the create content modal
    openContentModal(categoryId: string): void {
        this.fileErrMsg = false;
        let _self = this;

        this.toUpdateContent = false;
        this.isFileUploaded = false;
        this.category_id = categoryId;
        this.contentVM = new ContentViewModel(this.course_id, this.category_id, '', '', '', '', '', false, false, null)

        this.staticModalContent.show();

    }

    add_updateContent(): void {

        let _self = this;

        _self.contentVM.CategoryId = this.category_id;
        _self.contentVM.CourseId = this.course_id;
        this.isContentExists(function (data) {
            if (!data) {
                _self.postFileAndForm(function (result) {

                    //if (result.status != 200) {
                    //    _self._alertify.SimpleAlert("Please try again !");
                    //    return;
                    //}


                    _self.staticModalContent.hide();
                    _self.contentVM.ContentName = "";
                    //loop to check if deleted category is matched in the list
                    for (var i = 0; i < _self.categories.length; i++) {
                        if (_self.category_id === _self.categories[i].CategoryId) {
                            //to check if data is updated
                            if (_self.toUpdateContent) {
                                //if categorry matched then loop on contents of selected category
                                for (var j = 0; j < _self.categories[i].Contents.length; j++) {
                                    if (_self.categories[i].Contents[j].ContentId == result.ContentId) {
                                        _self.categories[i].Contents.splice(j, 1);
                                        _self.categories[i].Contents.splice(j, 0, result);
                                        break;
                                    }
                                }
                            }
                            else {
                                _self.categories[i].Contents.unshift(result);
                            }
                            break;
                        }
                    }

                });


            } else {
                _self._alertify.SimpleAlert("Content Name already exists in this category.");
            }
        });

    }

    postFileAndForm(callback): void {
        let _self = this;
        let formData = new FormData();
        formData.append('ContentViewModel', JSON.stringify(_self.contentVM));
        formData.append("file", this.contentVM.ContentFile);
        this._CategoryService.postFile(formData)
            .subscribe((result) => {
                callback(result);
            });
    }

    upload(event) {
        if (event.length > 0) {
            var file = event[0];
            if (file.size / 1024 / 1024 > 50) {
                this.fileErrMsg = true;
            }
            else {
                this.fileErrMsg = false;
                this.contentVM.ContentFile = file;
                this.isFileUploaded = true;
                this.contentVM.FileName = file.name;
            }

        }
    }

    deleteUploadedFile(): void {
        this.contentVM.FileName = "";
        this.isFileUploaded = false;
    }

    //Check if content is the category exists in database
    isContentExists(callback): void {
        this._CategoryService.isContentExist(this.contentVM)
            .subscribe(data => {
                callback(data);
            }, error => {

                callback(false);
            });
    }

    //To delete the content from dabatase
    deleteContent(categoryId: string, contentId: string): void {
        this.contentVM.CategoryId = categoryId;
        this.contentVM.ContentId = contentId;

        let _self = this;
        this._alertfyVMModel.title = "Are you sure want to delete this content?";
        this._alertfyVMModel.text = "You won't be able to revert this!";
        this._alertfyVMModel.confirmButtonText = "Delete";
        this._alertify.Confirm(this._alertfyVMModel, function (isConfirmed) {
            if (isConfirmed) {
                _self._CategoryService.deleteContent(_self.contentVM)
                    .subscribe(result => {
                        if (result.HasError) {
                            _self._alertify.SimpleAlert(result.ErrorMessage);
                            return;
                        }
                        //if (result.status != 200) {
                        //    _self._alertify.SimpleAlert("Please try again !");
                        //    return;
                        //}
                        //loop to check if deleted category is matched in the list
                        for (var i = 0; i < _self.categories.length; i++) {
                            if (_self.categories[i].CategoryId == _self.contentVM.CategoryId) {
                                //if categorry matched then loop on contents of selected category
                                for (var j = 0; j < _self.categories[i].Contents.length; j++) {
                                    if (_self.categories[i].Contents[j].ContentId == result.ContentId) {
                                        _self.categories[i].Contents.splice(j, 1);
                                        break;
                                    }

                                }
                            }
                        }

                    }, error => {
                    });
            }
        });


    }

    //Open the Edit Content Modal
    editContentModal(selectedContent: any, category_id: string): void {
        this.fileErrMsg = false;
        let _self = this;
        this.toUpdateContent = true;
        this.isFileUploaded = true;

        this.category_id = category_id;


        let copy = Object.assign({}, selectedContent)
        this.contentVM = copy;

        this.staticModalContent.show();

    }

    //For the checkbox of send email in content popup
    sendEmailToAttendeesChange(event: any): void {
        this.contentVM.SendEmailToAttendees = event;
    }

    //For the checkbox of visibile to attendee in content popup
    isVisibleToAttendeesChange(event: any): void {
        if (!event) {
            this.contentVM.SendEmailToAttendees = false;
        }
        this.contentVM.IsVisibleToAttendees = event;

    }

    downloadFileLink(content: any): string {
        return this.baseUrl + "/api/content/downloadfilefromserver?fileName=" + content.FileName + "&filePath=" + content.FileUrl;
    }

    fileTypeIcons(content: any): string {
        if (content.FileType.includes('video/'))
            return "fa fa-video-camera";

        if (content.FileType.includes('image/'))
            return "fa fa-file-image-o";

        if (content.FileType.includes('/pdf'))
            return "fa fa-file-pdf-o";

        if (content.FileType.includes('application/octet-stream'))
            return "fa fa-file-o";

        if (content.FileType.includes('audio/'))
            return "fa fa-file-audio-o";

        return "fa fa-file-text";
    }

    fileTypeColor(content: any): string {
        if (content.FileType.includes('video/'))
            return "orange";

        if (content.FileType.includes('image/'))
            return "see_green";

        if (content.FileType.includes('/pdf'))
            return "pdf_color";

        if (content.FileType.includes('application/octet-stream'))
            return "octect_file";

        if (content.FileType.includes('audio/'))
            return "audio_file";

        return "dark-blue";
    }


    checkIfUserLoggedIn(): void {
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser == null || currentUser == undefined) {
            this._router.navigate(['/account/login']);
        }
    }

}


export class CategoryViewModel {
    constructor(
        public CategoryName: string,
        public CourseId: string,
        public CategoryId: string
    ) { }
}

export class ContentViewModel {
    constructor(
        public CourseId: string,
        public CategoryId: string,
        public ContentId: string,
        public ContentName: string,
        public FileName: string,
        public FileUrl: string,
        public FileType: string,
        public IsVisibleToAttendees: boolean,
        public SendEmailToAttendees: boolean,
        public ContentFile: any
    ) { }
}

export class AlertfyVMModel {
    constructor(
        public title: string,
        public text: string,
        public type: string,
        public showCancelButton: boolean,
        public confirmButtonText: string,

    ) { }
}

