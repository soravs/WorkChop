import { BrowserModule } from '@angular/platform-browser';
import { NgModule} from '@angular/core';
import { RootRoutingModule } from './root-routing.module';
import { RootComponent } from './root.component';

@NgModule({
    imports: [
        BrowserModule,
        RootRoutingModule
    ],
    declarations: [
        RootComponent
    ],
    providers: [
    ],
    bootstrap: [RootComponent]
})
export class RootModule {

}