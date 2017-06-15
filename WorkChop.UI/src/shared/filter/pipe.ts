import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name:'CourseFilter'
})

export class CourseFilter implements PipeTransform {
    transform(value: any, input: string) {
        if (input) {
            input = input.toLowerCase();
            return value.filter(function (el: any) {
                return el.CourseName.toLowerCase().indexOf(input) > -1;
            });
        }
        return value;
    }
}