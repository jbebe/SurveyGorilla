app.controller('ClientController', function ($scope, $http, $routeParams, ClientService) {
    $scope.surveyid = $routeParams.surveyid;
    $scope.newClient = function () {
        var data = {
            email: $scope.email,
            info: JSON.stringify({
                name: $scope.name
            })
        }
        ClientService.create($scope.surveyid,data, $http);
    }
    $scope.deleteClient = function (id) {
        if (confirm("Delete Survey?")) {
            ClientService.delete($scope.surveyid,id, $http);
        }
    }

    ClientService.list($scope.surveyid, $http, function (response) {
        $scope.clients = response.data;
        for (var i = 0; i < $scope.clients.length; i++) {
            $scope.clients[i].info = JSON.parse($scope.clients[i].info);
        }
    }, function (response) {

    });
});


app.factory('ClientService', function () {
    return {
        get: function (surveyid,id, $http, onSuccess, onError) {
            $http.get("/api/survey/" + surveyid+"/client/" + id)
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
        list: function (surveyid,$http, onSuccess, onError) {
            $http.get("/api/survey/" + surveyid+"/client")
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
        create: function (surveyid,data, $http, onSuccess, onError) {
            $http.post("/api/survey/" + surveyid+"/client", data, {})
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
        update: function (surveyid,id, data, $http, onSuccess, onError) {
            $http.put("/api/survey/" + surveyid+"/client/" + id, data, {})
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
        delete: function (surveyid,id, $http, onSuccess, onError) {
            $http.delete("/api/survey/" + surveyid+"/client/" + id)
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