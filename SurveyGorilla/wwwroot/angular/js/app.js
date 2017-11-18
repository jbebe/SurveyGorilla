
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
        }).otherwise({ redirectTo: '/home' });
});

app.run(function ($rootScope,$location, LoginService) {
    $rootScope.$on("$locationChangeStart", function (event, next, current) {
        var notroute = ["register", "login", "logout"];
        var routepath = next.split("#!/")[1];
        if (notroute.indexOf(routepath) == -1) {
            if (!LoginService.isAuthenticated()) {
                $location.path("/login");
            } 
        }                
    });
    if (!LoginService.isAuthenticated()) {
        $location.path("/login");
    }
});


app.controller('LoginController', function ($scope, $rootScope, $location,$http, LoginService) {
       
    $scope.formSubmit = function () {

        function onSuccess() {
            $scope.error = '';
            $scope.username = '';
            $scope.password = '';

            $location.path("/home");
        }

        function onError() {
            $scope.error = "Invalid Login Parameters!";
        }

        LoginService.login({
            "email": $scope.email,
            "password": $scope.password,
            "info": $scope.username
        }, $http, onSuccess, onError );    
            
    };

});

app.controller('HomeController', function ($rootScope, LoginService) {
    $rootScope.title = "AngularJS Login Sample";

});

app.controller('LogoutController', function ($http, $location, LoginService) {
    LoginService.logout($http, function () {
        $location.path("/login");
    });

});


app.controller('RegistrationController', function ($scope, $rootScope, $http, $location, LoginService) {

    $scope.formSubmit = function () {

        function onSuccess() {
            $scope.error = '';
            $scope.username = '';
            $scope.password = '';
                
            $location.path("/home");
        }

        function onError() {
            $scope.error = "Cannot Register!";
        }

        LoginService.register({
            "email": $scope.email,
            "password": $scope.password,
            "info": $scope.username
        }, $http, onSuccess, onError );

    };

});

app.factory('LoginService', function () {
    var admin = 'admin';
    var pass = 'pass';
    var isAuthenticated = false;

    return {
        login: function (data, $http, onSuccess, onError) {
            login(data, $http, function (response) {
                isAuthenticated = true;
                if (onSuccess) {
                    onSuccess(response);
                }
                   
            }, function (response) {
                if (onError) {
                    onError(response);
                }                   
            })
        },
        register: function (data, $http, onSuccess, onError) {
            register(data, $http, function (response) {
                isAuthenticated = true;
                if (onSuccess) {
                    onSuccess(response);
                }

            }, function (response) {
                if (onError) {
                    onError(response);
                }
            })                
        },
        isAuthenticated: function () {
            return isAuthenticated;
        },
        logout: function ($http, onSuccess, onError) {
            logout( $http, function (response) {
                isAuthenticated = false;
                if (onSuccess) {
                    onSuccess(response);
                }

            }, function (response) {
                if (onError) {
                    onError(response);
                }
            })     
        }
    };
});


function register(data, $http, onSuccess, onError) {

    $http.post("/register", data, {})
        .then(function successCallback(response) {
            onSuccess(response);
        }, function errorCallback(response) {
            onError(response);
        });
}


function login(data, $http, onSuccess, onError) {

    $http.post("/login", data, {})
        .then(function successCallback(response) {
            onSuccess(response);
        }, function errorCallback(response) {
            onError(response);
        });
}

function logout($http, onSuccess, onError) {
    $http.get("/logout")
        .then(function successCallback(response) {
            onSuccess(response);
        }, function errorCallback(response) {
            onError(response);
        });
}
