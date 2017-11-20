app.controller('ClientController', function ($scope, $http, $routeParams, ClientService, SurveyService) {
    $scope.survey = SurveyService.getSurvey($routeParams.id);
    $scope.surveyid = $routeParams.surveyid;
    $scope.newClient = function () {
        var data = {
            email: $scope.email,
            info: angular.toJson({
                name: $scope.name
            })
        }
        ClientService.create($scope.surveyid, data, $http, function () {

            data.info = JSON.parse(data.info);
            $scope.clients.push(data);
        });
    }
    $scope.deleteClient = function (id) {
        if (confirm("Delete Client?")) {
            ClientService.delete($scope.surveyid, id, $http, function () {
                for (var i = 0; i < $scope.clients.length; i++) {
                    if ($scope.clients[i].id == id) {
                        $scope.clients.splice(i, 1);
                        break;
                    }
                }
            });
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


app.controller('ClientEditController', function ($scope, $http, $routeParams, $location, ClientService) {
    $scope.surveyid = $routeParams.surveyid;
    $scope.id = $routeParams.id;
    $scope.editClient = function () {
        var data = {
            email: $scope.email,
            info: angular.toJson({
                name: $scope.name
            })
        }
        ClientService.update($scope.surveyid, $scope.id, data, $http, function () {
            $location.path("/survey/" + $scope.surveyid + "/client");
        });
    }

    ClientService.get($scope.surveyid, $scope.id, $http, function (response) {
        $scope.client = response.data;
        $scope.email = $scope.client.email;
        $scope.info = JSON.parse($scope.client.info);
        $scope.name = $scope.info.name;
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