
app.controller('SurveyResultsController', function ($scope, $http, $location, $routeParams, SurveyService, ClientService) {
    $scope.survey = SurveyService.getSurvey($routeParams.surveyid);
    if ($scope.survey == null) {
        SurveyService.get($routeParams.surveyid, $http, function (response) {
            $scope.survey = response.data;
            $scope.survey.info = JSON.parse($scope.survey.info);
        })
    }
    $scope.surveyid = $routeParams.surveyid;
    $scope.clients = [];
    $scope.results = {};
    $scope.backToSurvey = function () {
        $location.path("/survey/" + $scope.surveyid);
    }
    ClientService.list($scope.surveyid, $http, function (response) {        
        for (var i = 0; i < response.data.length; i++) {

            var info = JSON.parse(response.data[i].info);
            if (!info.answers) {
                info.answers = [];
            }
            $scope.clients.push({
                name: info.name,
                answers: info.answers
            });

            angular.forEach(info.answers, function (value, key) {
                var quest_res = $scope.results[value.id] || {};
                if (quest_res[value.answer]) {
                    quest_res[value.answer].count += 1;
                    quest_res[value.answer].users.push(info.name)
                } else {
                    quest_res[value.answer] = {};
                    quest_res[value.answer].count = 1;
                    quest_res[value.answer].users = [];
                    quest_res[value.answer].users.push(info.name);
                }
                $scope.results[value.id] = quest_res;
            });
            
        }

    }, function (response) {

        });
    
});