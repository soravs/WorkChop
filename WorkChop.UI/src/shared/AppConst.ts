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
                    endPoint: 'http://workchop.com'
                };
        }
        return data[value];
    }
}