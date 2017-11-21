
app.controller('SurveyResultsController', function ($scope, $location, $routeParams, SurveyService) {
    $scope.survey = SurveyService.getSurvey($routeParams.id);
    $scope.surveyid = $routeParams.id;

    $scope.backToSurvey = function () {
        $location.path("/survey/" + $scope.surveyid);
    }


});