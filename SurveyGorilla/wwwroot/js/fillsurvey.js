﻿
app.controller('FillSurveyController', function ($scope, $http, $routeParams, FillSurveyService) {
    $scope.token = $routeParams.token;
    $scope.answers = {};
    $scope.answer = function () {
        var info = {
            answers: $scope.answers
        };

        var data = {
            info: JSON.stringify(info)
        };
        FillSurveyService.send($scope.token, data, $http);
    }

    FillSurveyService.list($scope.token,$http, function (response) {
        $scope.adminName = response.data.adminName;
        $scope.surveyName = response.data.surveyName;
        $scope.surveyStart = response.data.surveyStart;
        $scope.surveyEnd = response.data.surveyEnd;
        $scope.questions = response.data.questions;            
    }, function (response) {
        $scope.surveyName = "SurveyName";
        $scope.questions = [{ "question": "1+1?" }, { "question": "2+2?" }];
    });
});


app.factory('FillSurveyService', function () {
    return {
        send: function (token, data, $http, onSuccess, onError) {
            $http.put("/survey/send/" + token, data, {})
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
        list: function (token, $http, onSuccess, onError) {
            $http.get("/survey/info/" + token)
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