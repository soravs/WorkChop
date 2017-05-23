import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.CourseServiceProxy,
        ApiServiceProxies.AuthenticationServiceProxy,
        ApiServiceProxies.CustomExceptionHandlingServiceProxy
    ]
})
export class ServiceProxyModule { }