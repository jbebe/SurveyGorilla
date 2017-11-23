app.controller('LoginController', function ($scope, $rootScope, $location, $http, LoginService) {

    $scope.formSubmit = function () {

        function onSuccess() {
            $scope.error = '';
            $scope.username = '';
            $scope.password = '';

            $location.path("/survey");
        }

        function onError() {
            $scope.error = "Invalid Login Parameters!";
        }

        LoginService.login({
            "email": $scope.email,
            "password": $scope.password,
            "info": "{}"
        }, $http, onSuccess, onError);

    };

});


app.controller('LogoutController', function ($http, $location, LoginService) {
    LoginService.logout($http, function () {
        $location.path("/home");
    });

});


app.controller('RegistrationController', function ($scope, $rootScope, $http, $location, LoginService) {

    $scope.formSubmit = function () {

        function onSuccess() {
                LoginService.login({
                    "email": $scope.email,
                    "password": $scope.password,
                    "info": "{}"
                }, $http, function () {
                    $scope.error = '';
                    $scope.username = '';
                    $scope.password = '';
                    $location.path("/survey");
                }, function () { }
            ); 
        }

        function onError() {
            $scope.error = "Cannot Register!";
        }

        LoginService.register({
            "email": $scope.email,
            "password": $scope.password,
            "info": JSON.stringify({
                   "name":$scope.name
            })
        }, $http, onSuccess, onError);

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
            logout($http, function (response) {
                isAuthenticated = false;
                if (onSuccess) {
                    onSuccess(response);
                }

            }, function (response) {
                if (onError) {
                    onError(response);
                }
            })
        },
        auth: function () {
            isAuthenticated = true;
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
