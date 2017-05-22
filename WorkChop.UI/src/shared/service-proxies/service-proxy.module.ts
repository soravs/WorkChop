import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.CourseServiceProxy
    ]
})
export class ServiceProxyModule { }