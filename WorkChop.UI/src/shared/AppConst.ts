export class Config {

    public static getEnvironmentVariable(value) {
        var environment: string;
        var data = {};
        environment = window.location.hostname;
        switch (environment) {
            case 'localhost':
                data = {
                    endPoint: 'http://localhost:53027'
                    
                };
                break;
            default:
                data = {
                    //endPoint: 'http://workchop.com'
                    endPoint: 'alpha-workchop.us-west-2.elasticbeanstalk.com'
                };
        }
        return data[value];
    }
}

export class ConstantValues
{
    public static CourseImage: string = "../../assets/images/Asset 4.png";

    public static GoogleMapKey:string= 'AIzaSyA4KEetv4Mjtar6f-ioisyXcJ65HDjuqCY';
}