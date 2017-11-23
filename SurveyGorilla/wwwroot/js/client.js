app.controller('ClientController', function ($scope, $http, $routeParams, ClientService, SurveyService, MailService) {
    $scope.survey = SurveyService.getSurvey($routeParams.surveyid);
    $scope.surveyid = $routeParams.surveyid;
    $scope.mailallsent = false;
    if ($scope.survey == null) {
        SurveyService.get($routeParams.surveyid, $http, function (response) {
            $scope.survey = response.data;
            $scope.survey.info = JSON.parse($scope.survey.info);
            if ($scope.survey.info.mailall) {
                $scope.mailallsent = true;
            }
        }, function (response) {
            showError("Error", "Can't load Survey");
        })
    } else {
        if ($scope.survey.info.mailall) {
            $scope.mailallsent = true;
        }
    }

   
    

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
        }, function (txt) {
            showError("Error", "Can't crete new client")

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
            }, function () {
                showError("Error", "Can't delete client")
            });
        }
    }

    $scope.sendMailAll = function (id) {
        MailService.sendAll(id, $http, function () {  
            $scope.survey.info.mailall = true;
            $scope.survey.info = angular.toJson($scope.survey.info);            
            SurveyService.update($scope.surveyid, $scope.survey, $http, function () {                
                $scope.survey.info = angular.toJson($scope.survey.info);
                showInfo("E-mail sucessfully", "Email has been sent to all Clients");
            }, function () {
                showInfo("Something went wrong", "Email has been sent succesfully, but some error occured after it");
            });
        }, function () {
               showError("Email error","Cannot send Survey Email to Clients")
        });
    }

    ClientService.list($scope.surveyid, $http, function (response) {
        $scope.clients = response.data;
        for (var i = 0; i < $scope.clients.length; i++) {
            $scope.clients[i].info = JSON.parse($scope.clients[i].info);
        }
    }, function (response) {
        showError("Error","Unforunatelly we can't load clients")
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
        }, function () {          
                showError("Error", "Can't edit client data")          
         });
    }

    ClientService.get($scope.surveyid, $scope.id, $http, function (response) {
        $scope.client = response.data;
        $scope.email = $scope.client.email;
        $scope.info = JSON.parse($scope.client.info);
        $scope.name = $scope.info.name;
    }, function () {
        showError("Error", "Can't load client data")
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


app.factory('MailService', function () {
    return {
        sendAll: function (surveyid, $http, onSuccess, onError) {
            $http.get("/api/mail/survey/" + surveyid)
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
        send: function (surveyid, clientid, $http, onSuccess, onError) {
            $http.get("/api/mail/survey" + surveyid + "/client/" + clientid)
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
    }
});