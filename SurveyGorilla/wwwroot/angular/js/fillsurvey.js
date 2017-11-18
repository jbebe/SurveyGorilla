
app.controller('FillSurveyController', function ($scope, $http, $routeParams, FillSurveyService) {
    $scope.token = $routeParams.token;
    $scope.answers = {};
    $scope.answer = function () {
        var data = {

        }
        //TODO GET ANSWERS

        FillSurveyService.send($scope.token, data, $http);
    }

    FillSurveyService.list($scope.token,$http, function (response) {
        $scope.questions = response.data;
        for (var i = 0; i < $scope.questions.length; i++) {
            $scope.questions[i].info = JSON.parse($scope.questions[i].info);
        }
    }, function (response) {

    });
});


app.factory('FillSurveyService', function () {
    return {
        send: function (token, data, $http, onSuccess, onError) {
            $http.post("/questions/" + token, data, {})
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
            $http.get("/questions/" + token)
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
