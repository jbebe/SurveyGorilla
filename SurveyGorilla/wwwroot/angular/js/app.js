
var app = angular.module('myApp', ['ngRoute', 'ngCookies']);
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
        }).when('/survey/:surveyid/client/:id', {
            templateUrl: 'view/clientedit.html',
            controller: 'ClientEditController'
        }).when('/questions/:token', {
            templateUrl: 'view/fillsurvey.html',
            controller: 'FillSurveyController'
        }).when('/survey/:surveyid/results', {
            templateUrl: 'view/surveyresults.html',
            controller: 'SurveyResultsController'
        }).otherwise({ redirectTo: '/home' });
});

app.run(function ($rootScope, $location, $cookies, LoginService) {
    $rootScope.LoginService = LoginService;
    $rootScope.$on("$locationChangeStart", function (event, next, current) {
        var notroute = ["register", "login", "logout", "home"];
        var routepath = next.split("#!/")[1];
        if (notroute.indexOf(routepath) == -1) {
            if (!LoginService.isAuthenticated()) {
               $location.path("/login");
            } 
        }                
    });
    
    if ($cookies.get('session_id')) {
        LoginService.auth();
        $location.path("/survey");
    }else if (!LoginService.isAuthenticated()) {
        $location.path("/home");
    }
});


app.controller('HomeController', function ($rootScope, LoginService) {
   //TODO

});
