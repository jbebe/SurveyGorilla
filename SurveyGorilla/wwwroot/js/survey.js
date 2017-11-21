
app.controller('SurveyController', function ($scope, $http, $location, SurveyService) {
    $scope.newSurvey = function () {
        var data = {
            name: $scope.NewSurveyName,
            info: JSON.stringify({
                availability: { 
                    start: new Date().toJSON(),
                    end: yearlater().toJSON()
                }
            }),            
        }
        SurveyService.create(data, $http, function (response) {
            $scope.surveys.push(data);
            response.data.info = JSON.parse(response.data.info);
            //SurveyService.addListed(response.data);
            
        });
    }
    $scope.deleteSurvey = function (id) {
        if (confirm("Delete Survey?")) {
            SurveyService.delete(id, $http, function () {
                for (var i = 0; i < $scope.surveys.length; i++) {
                    if ($scope.surveys[i].id == id) {
                        $scope.surveys.splice(i, 1);
                        break;
                    }
                }
            });
        }
    }

    $scope.clientEditOpen = function (survey) {
        SurveyService.setSurvey(survey);
        $location.path("/survey/"+survey.id+"/client");
    }

    $scope.surveyResults = function (survey) {
        SurveyService.setSurvey(survey);
        $location.path("/survey/" + survey.id + "/results");
    }

    $scope.splitView = function (survey) {
        SurveyService.setSurvey(survey);
        $location.path("/splitview/" + survey.id);
    }

    SurveyService.list($http, function (response) {
        $scope.surveys = response.data;
        for (var i = 0; i < $scope.surveys.length; i++) {
            $scope.surveys[i].info = JSON.parse($scope.surveys[i].info); 
            $scope.surveys[i].info.created = new Date($scope.surveys[i].info.created).toLocaleString()
        }
        SurveyService.setListed($scope.surveys);
    }, function (response) {

    });


});


app.controller('SurveyEditController', function ($scope, $http, $routeParams, SurveyService) {
    $scope.survey = SurveyService.getSurvey($routeParams.surveyid);
    $scope.id = $routeParams.surveyid;
    $scope.addQuestion = function () {
        $scope.questions.push({
            question: $scope.newQuestion
        });
        $scope.info.questions = $scope.questions;
        $scope.survey.info = angular.toJson($scope.info);
        SurveyService.update($scope.id,$scope.survey, $http);
    }

    $scope.removeQuestion = function (id) {
        if (confirm("Delete Question?")) {
            var deleted = $scope.questions.splice(id, 1);
            $scope.info.questions = $scope.questions;
            $scope.survey.info = angular.toJson($scope.info);
            SurveyService.update($scope.id, $scope.survey, $http, function () {

            }, function () {
                $scope.questions.push(deleted);
            });
        }
    }

    SurveyService.get($routeParams.surveyid, $http, function (response) {
        $scope.survey = response.data;
        $scope.name = $scope.survey.name;
        $scope.info = JSON.parse($scope.survey.info);
        $scope.created = new Date($scope.info.created).toLocaleString();
        $scope.questions = $scope.info.questions || [];
    }, function (response) {

    });
});



app.factory('SurveyService', function () {
    var selected = null;
    var listed = null;
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
        },
        getSurvey: function (id) {
            if (selected && selected.id == id) {
                return selected;
            } else if (listed) {
                for (var i = 0; i < listed.length; i++) {
                    if (listed[i].id == id) {
                        return listed[i];
                    }
                }
            }
            return null;
        },
        setSurvey: function (survey) {
            selected = survey;
        },
        setListed: function (surveyList) {
            listed = surveyList;
        },
        addListed: function (survey) {
            listed.push(survey);
        }
    };
});

function yearlater() {
    var date = new Date();
    date.setDate(date.getDate() + parseInt(365));
    return date;
}