
app.controller('SurveyResultsController', function ($scope, $location, $routeParams) {
    $scope.surveyid = $routeParams.id;
    $scope.survey = window.survey;
    if (!$scope.survey || $scope.survey.id != $scope.surveyid) {
        // TODO GET Survey FROM SERVER
    }

    $scope.backToSurvey = function () {
        $location.path("/survey/" + $scope.surveyid);
    }

});