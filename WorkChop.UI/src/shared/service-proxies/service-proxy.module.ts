import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.CourseServiceProxy,
        ApiServiceProxies.AuthenticationServiceProxy,
        ApiServiceProxies.CustomExceptionHandlingServiceProxy,
        //ApiServiceProxies.AlertfyVMModel,
        ApiServiceProxies.AlertifyModals,
    ]
})
export class ServiceProxyModule { }