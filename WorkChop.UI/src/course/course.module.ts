import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { CourseRoutingModule } from './course-routing.module';
import { CourseComponent } from './course.component';
import { CommonModule } from '@angular/common';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';
import { SettingComponent } from './setting/setting.component';
import { ContentComponent } from './content/content.component';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';
import { AgmCoreModule } from "angular2-google-maps/core";
import { ModalModule } from 'ng2-bootstrap';
import { ConstantValues } from "../../src/shared/AppConst";
import { TinymceModule } from 'angular2-tinymce';
import { BlockUIModule } from 'ng-block-ui';
import { MomentModule } from 'angular2-moment';
import { Debounce } from 'angular2-debounce';
import { ContentFilterPipe } from '../shared/filter/CommonFilterPipe';


@NgModule({
    declarations: [
         CourseComponent,
        SettingComponent,
        ContentComponent,
        Debounce,
        ContentFilterPipe
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        CourseRoutingModule,
        ServiceProxyModule,
        AgmCoreModule.forRoot({
            apiKey: ConstantValues.GoogleMapKey,
            libraries: ["places"]
        }),
      
        ModalModule.forRoot(),
        
        TinymceModule.withConfig({
        }),
        BlockUIModule,
        MomentModule
    ],
    providers: [
        AppRouteGuard

    ],
    
    bootstrap: [CourseComponent]
})
export class CourseModule {

}
