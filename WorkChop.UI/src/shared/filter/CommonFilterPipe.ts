import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'ContentFilterPipe'
})

export class ContentFilterPipe implements PipeTransform {
    transform(value: any, input: string) {
        if (input) {
            input = input.toLowerCase();
            return value.filter(function (el: any) {
                return el.ContentName.toLowerCase().indexOf(input) > -1;
            });
        }
        return value;
    }
}