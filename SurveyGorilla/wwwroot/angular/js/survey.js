
app.controller('SurveyController', function ($scope, $http, SurveyService) {
    $scope.newSurvey = function () {
        var data = {
            name: $scope.name,
            info: "{}",            
        }
        SurveyService.create(data, $http);
    }
    $scope.deleteSurvey = function (id) {
        if (confirm("Delete Survey?")) {
            SurveyService.delete(id, $http);
        }
    }

    SurveyService.list($http, function (response) {
        $scope.surveys = response.data;
        for (var i = 0; i < $scope.surveys.length; i++) {
            $scope.surveys[i].info = JSON.parse($scope.surveys[i].info); 
            $scope.surveys[i].info.created = new Date($scope.surveys[i].info.created).toLocaleString()
        }
    }, function (response) {

    });


});


app.controller('SurveyEditController', function ($scope, $http, $routeParams, SurveyService) {
    $scope.id = $routeParams.id;
    $scope.addQuestion = function () {
        
        $scope.questions.push({
            question: $scope.question,
            answer: $scope.answer
        });
       
        $scope.info.questions = $scope.questions;
        $scope.survey.info = JSON.stringify($scope.info);
        SurveyService.update($scope.id,$scope.survey, $http);
    }


    SurveyService.get($routeParams.id, $http, function (response) {
        $scope.survey = response.data;
        $scope.name = $scope.survey.name;
        $scope.info = JSON.parse($scope.survey.info);
        $scope.created = new Date($scope.info.created).toLocaleString();
        $scope.questions = $scope.info.questions || [];
    }, function (response) {

    });
});



app.factory('SurveyService', function () {
    return {
        get: function (id, $http, onSuccess, onError) {
            $http.get("/api/survey/" + id)
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
            $http.get("/api/survey")
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
            $http.post("/api/survey", data, {})
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
        update: function (id,data, $http, onSuccess, onError) {
            $http.put("/api/survey/" + id, data, {})
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
            $http.delete("/api/survey/" + id)
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