
app.controller('AdminController', function ($scope, $http, AdminService) {
    $scope.newUser = function () {
        var data = {
            email: $scope.email,
            password: $scope.password,
            info: ""
        }
        AdminService.create(data,$http);
    }
    $scope.deleteUser = function (id) {
        if (confirm("Delete User?")) {
            AdminService.delete(id, $http);
        }       
    }


    AdminService.list($http, function (response) {       
        $scope.users = response.data;
    }, function (response) {

    });


});

app.factory('AdminService', function () {
    return {
        get: function (id, $http, onSuccess, onError) {
            $http.get("/api/admin/" + id)
                .then(function successCallback(response) {
                    if (onSuccess) {
                        onSuccess(response);
                    }
                }, function errorCallback(response) {
                    if (onError) {
                        onError(response);
                    }
                })
        },
        list: function ($http, onSuccess, onError) {
            $http.get("/api/admin")
                .then(function successCallback(response) {
                    if (onSuccess) {
                        onSuccess(response);
                    }
                }, function errorCallback(response) {
                    if (onError) {
                        onError(response);
                    }
                })
        },
        create: function (data, $http, onSuccess, onError) {
            $http.post("/api/admin", data, {})
                .then(function successCallback(response) {
                    if (onSuccess) {
                        onSuccess(response);
                    }
                }, function errorCallback(response) {
                    if (onError) {
                        onError(response);
                    }
                })
        },
        update: function (data, $http, onSuccess, onError) {
            $http.put("/api/admin/" + id, data, {})
                .then(function successCallback(response) {
                    if (onSuccess) {
                        onSuccess(response);
                    }
                }, function errorCallback(response) {
                    if (onError) {
                        onError(response);
                    }
                })
        },
        delete: function (id, $http, onSuccess, onError) {
            $http.delete("/api/admin/" + id)
                .then(function successCallback(response) {
                    if (onSuccess) {
                        onSuccess(response);
                    }
                }, function errorCallback(response) {
                    if (onError) {
                        onError(response);
                    }
                })
        }
    };
});