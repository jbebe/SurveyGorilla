
var app = angular.module('myApp', ['ngRoute']);
app.config(function ($routeProvider) {
    $routeProvider
        .when('/login', {
            templateUrl: 'view/login.html',
            controller: 'LoginController'
        }).when('/logout', { 
            templateUrl: 'view/logout.html',
            controller: 'LogoutController'
        }).when('/home', {
            templateUrl: 'view/home.html',
            controller: 'HomeController'
        }).when('/register', {
            templateUrl: 'view/register.html',
            controller: 'RegistrationController'
        }).when('/admin', {
            templateUrl: 'view/admin.html',
            controller: 'AdminController'
        }).when('/survey', {
            templateUrl: 'view/survey.html',
            controller: 'SurveyController'
        }).when('/survey/:id', {
            templateUrl: 'view/surveyedit.html',
            controller: 'SurveyEditController'
        }).when('/survey/:surveyid/client', {
            templateUrl: 'view/client.html',
            controller: 'ClientController'
        }).otherwise({ redirectTo: '/home' });
});

app.run(function ($rootScope, $location, LoginService) {
    $rootScope.LoginService = LoginService;
    $rootScope.$on("$locationChangeStart", function (event, next, current) {
        var notroute = ["register", "login", "logout"];
        var routepath = next.split("#!/")[1];
        if (notroute.indexOf(routepath) == -1) {
            if (!LoginService.isAuthenticated()) {
               // $location.path("/login");
            } 
        }                
    });
    if (!LoginService.isAuthenticated()) {
        //$location.path("/login");
    }
});


app.controller('HomeController', function ($rootScope, LoginService) {
   //TODO

});
