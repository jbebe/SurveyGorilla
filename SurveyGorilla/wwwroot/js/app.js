
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
        }).when('/survey/:surveyid', {
            templateUrl: 'view/surveyedit.html',
            controller: 'SurveyEditController'
        }).when('/survey/:surveyid/client', {
            templateUrl: 'view/client.html',
            controller: 'ClientController'
        }).when('/survey/:surveyid/client/:id', {
            templateUrl: 'view/clientedit.html',
            controller: 'ClientEditController'
        }).when('/token/:token', {
            templateUrl: 'view/fillsurvey.html',
            controller: 'FillSurveyController'
        }).when('/splitview/:surveyid', {
            templateUrl: 'view/splitview.html',
            controller: 'SplitViewController'
        }).when('/survey/:surveyid/results', {
            templateUrl: 'view/surveyresults.html',
            controller: 'SurveyResultsController'
        }).otherwise({ redirectTo: '/login' });
});

app.run(function ($rootScope, $location, $cookies, LoginService) {
    $rootScope.LoginService = LoginService;
    $rootScope.$on("$locationChangeStart", function (event, next, current) {

        if (LoginService.needLogin(next)) {
            if (!LoginService.isAuthenticated() || !$cookies.get('session_id')) {
               $location.path("/login");
            } 
        }                
    });
    
    if ($cookies.get('session_id')) {
        LoginService.auth();        
    } else if (!LoginService.isAuthenticated() && LoginService.needLogin($location.path())) {
        $location.path("/login");
    }
});


app.controller('HomeController', function ($rootScope, LoginService) {
   //TODO

});

app.controller('SplitViewController', function ($rootScope, $routeParams) {
    window.surveyid = $routeParams.surveyid;
    
});


