import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.CourseServiceProxy,
        ApiServiceProxies.CategoryServiceProxy,
        ApiServiceProxies.AuthenticationServiceProxy,
        ApiServiceProxies.CustomExceptionHandlingServiceProxy,
        ApiServiceProxies.AlertifyModals,
    ]
})
export class ServiceProxyModule { }