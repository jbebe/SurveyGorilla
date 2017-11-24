
app.controller('SurveyResultsController', function ($scope, $http, $location, $routeParams, $timeout, SurveyService, ClientService) {
    $scope.survey = SurveyService.getSurvey($routeParams.surveyid);
    if ($scope.survey == null) {
        SurveyService.get($routeParams.surveyid, $http, function (response) {
            $scope.survey = response.data;
            $scope.survey.info = JSON.parse($scope.survey.info);
           
        }, function (response) {
            showError("Error", "Can't load Survey");
        })
        
    } else {
       
    }
    $scope.renderChart = function (id) {    
        setTimeout(function () {
            var quest = $scope.survey.info.questions[id];
            createCanvas(id, quest, $scope.results[quest.id])
        }, 0)
                    
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
                if (!quest_res.sum) {
                    quest_res.sum = 0;
                }
                quest_res.sum++;
                $scope.results[value.id] = quest_res;
            });

           
            
        }

    }, function (response) {
        showError("Error", "Can't load Answers");
    });
    
});


function createCanvas(id,question,result) {
    var ctx = document.getElementById("chart_" + id).getContext('2d');
    var data = [];
    var labels = [];
    var backgound = [];
    var border = [];
    var colors = [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(75, 192, 192, 0.2)',
        'rgba(153, 102, 255, 0.2)',
        'rgba(255, 159, 64, 0.2)'
    ];
    var bordercolors = [
        'rgba(255,99,132,1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)'
    ];
    var i = 0;
    angular.forEach(result, function (details, answer) {
        if (answer != "sum") {
           
      
            data.push(details.count);
            labels.push(answer);
            backgound.push(colors[i])
            border.push(bordercolors[i]);
            i++;
            if (i == colors.length) {
                i = 0;
            }
        }
    });

        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: question.question,
                    data: data,
                    backgroundColor:backgound,
                    borderColor: border,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                responsive: false
            }
        });
 
}