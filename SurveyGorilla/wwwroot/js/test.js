
describe('ClientController', function () {
    var scope, createController, surveyservice;

    beforeEach(function () {
        
        angular.mock.module('ngRoute', 'ngCookies','myApp');
        inject(function ($rootScope, $controller, $injector, $http, $routeParams) {

             scope = $rootScope.$new();
            
             routeParams = $routeParams;
             http = $http;
             cservice = $injector.get('ClientService');
             surveyservice = $injector.get('SurveyService');
             mservice = $injector.get('MailService');
            
            createController = function () {
                return $controller('ClientController', {
                    '$scope': scope,
                    '$http': http,
                    '$routeParams': routeParams,
                    'ClientService': cservice,
                    SurveyService: surveyservice,
                    MailService: mservice
                });
            };
        })
    });

 
    it('Controller Existence', inject(function ($rootScope, $controller) {  
        var controller = createController();      
        expect(controller).toBeDefined();
    }));
    
   
    it('Scope  functions existences [new,delete,mailall]', inject(function ($rootScope, $controller) { 
        var controller = createController();             
        expect(typeof scope.newClient).toEqual('function');
        expect(typeof scope.deleteClient).toEqual('function');
        expect(typeof scope.sendMailAll).toEqual('function');        
    }));

  

    it('Scope create functions behavior ', inject(function ($rootScope, $controller) {
        var clientcreatecalled = spyOn(cservice, "create");       
        var controller = createController();
        scope.newClient();
        expect(clientcreatecalled).toHaveBeenCalled();        
    }));

    it('Scope  delete function behavior', inject(function ($rootScope, $controller) {       
        var clientdeletecalled = spyOn(cservice, "delete");
        var controller = createController();
        var c = window.confirm;
        window.confirm = function (txt) { console.log(txt); return true; }
        scope.deleteClient();
        expect(clientdeletecalled).toHaveBeenCalled();
        window.confirm = c;
    }));

   

    it('SurveyService called if survey null', inject(function ($rootScope, $controller) {
        var onservicegetcalledSpy = spyOn(surveyservice, "get");        
        var controller = createController();             
        expect(onservicegetcalledSpy).toHaveBeenCalled();
    }));

    it('Maillall mailallsent', inject(function ($rootScope, $controller) {
        spyOn(surveyservice, "getSurvey").and.returnValue({           
            info: {
                mailall: true
            }
        });      
        var controller = createController();
        expect(scope.mailallsent).toEqual(true);
    }));
    

});



describe('FillSurveyController', function () {
    var scope, createController, loc, fillservice;
    beforeEach(function () {
        angular.mock.module('ngRoute', 'ngCookies', 'myApp');
        inject(function ($rootScope, $controller, $injector, $http, $location, $routeParams) {
            scope = $rootScope.$new();
            loc = $location;
            http = $http;
            routeparams = $routeParams;
            fillservice = $injector.get('FillSurveyService');
            createController = function () {
                return $controller('FillSurveyController', {
                    '$scope': scope,
                    '$http': http,
                    '$location': loc,
                    '$routeParams': routeparams,
                    FillSurveyService: fillservice
                });
            };
        })
    });

    it('Controller Existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(controller).toBeDefined();
    }));

    it('answer function existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(typeof scope.answer).toEqual('function');
       
    }));


    it('answer function behaviour', inject(function ($rootScope, $controller) {
        var sendcalled = spyOn(fillservice, "send").and.callFake(function () {
            expect(
                JSON.parse(arguments[1].info)
            ).toEqual({
                "answers": [{ "id": 'a', "answer": { "id": "id", "answer": "answ" } }]
            });
        });
        var controller = createController();
        scope.answers = { "a": { id: "id", answer: "answ" } };
        scope.answer();
        expect(sendcalled).toHaveBeenCalled();

    }));
});


describe('LoginController', function () {
    var scope, createController, loc, loginservice;
    beforeEach(function () {
        angular.mock.module('ngRoute', 'ngCookies', 'myApp');
        inject(function ($rootScope, $controller, $injector, $location, $http) {            
            scope = $rootScope.$new();
            loc = $location;
            http = $http;
            loginservice = $injector.get('LoginService');
            createController = function () {
                return $controller('LoginController', {
                    '$scope': scope,
                    '$http': http,
                    '$rootScope': $rootScope,
                    '$location': loc,
                    LoginService: loginservice
                });
            };
        })
    });

    it('Controller Existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(controller).toBeDefined();
    }));

    it('Autenticated redirection to home', inject(function ($rootScope, $controller) {
        spyOn(loginservice, "isAuthenticated").and.returnValue(true);              
        var onpathcalled = spyOn(loc, 'path');        
        var controller = createController();
        expect(onpathcalled).toHaveBeenCalledWith('/home');
    }));

    it('formSubmit behaviour', inject(function ($rootScope, $controller) {
        var logincalled = spyOn(loginservice, "login");
        var controller = createController();
        scope.formSubmit();
        expect(logincalled).toHaveBeenCalled();        
    }));

});



describe('SurveyController', function () {
    var scope, createController, loc, surveyservice;
    beforeEach(function () {
        angular.mock.module('ngRoute', 'ngCookies', 'myApp');
        inject(function ($rootScope, $controller, $injector,$http, $location) {
            scope = $rootScope.$new();
            loc = $location;
            http = $http;
            surveyservice = $injector.get('SurveyService');
            createController = function () {
                return $controller('SurveyController', {
                    '$scope': scope,
                    '$http': http,
                    '$location': loc,
                    SurveyService: surveyservice
                });
            };
        })
    });

    it('Controller Existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(controller).toBeDefined();
    }));

    it('Scope functions existences ', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(typeof scope.newSurvey).toEqual('function');
        expect(typeof scope.deleteSurvey).toEqual('function');
        expect(typeof scope.clientEditOpen).toEqual('function');
        expect(typeof scope.surveyResults).toEqual('function');
        expect(typeof scope.splitView).toEqual('function');
    }));

    it('newSurvey functions behavior ', inject(function ($rootScope, $controller) {
        var controller = createController();
        var createcalled = spyOn(surveyservice, "create").and.callFake(function () {
            expect(
                arguments[0].name
            ).toEqual("Test");
        });
        var controller = createController();
        scope.NewSurveyName = "Test";
        scope.newSurvey();
        expect(createcalled).toHaveBeenCalled();
    }));

    it('deleteSurvey functions behavior ', inject(function ($rootScope, $controller) {
        var c = window.confirm;
        window.confirm = function (txt) { console.log(txt); return true; }
        var controller = createController();
        var called = spyOn(surveyservice, "delete").and.callFake(function () {
            expect(
                arguments[0]
            ).toEqual(1);
        });
        var controller = createController();
        scope.deleteSurvey(1);
        expect(called).toHaveBeenCalled();       
        window.confirm = c;
    }));

    it('clientEditOpen functions behavior ', inject(function ($rootScope, $controller) {        
        var setcalled = spyOn(surveyservice, "setSurvey");
        var onpathcalled = spyOn(loc, 'path');       
        var controller = createController();
        var survey = {};
        survey.id = 1;       
        scope.clientEditOpen(survey);
        expect(setcalled).toHaveBeenCalled();
        expect(onpathcalled).toHaveBeenCalledWith('/survey/1/client');
    }));

    it('surveyResults functions behavior ', inject(function ($rootScope, $controller) {
        var setcalled = spyOn(surveyservice, "setSurvey");
        var onpathcalled = spyOn(loc, 'path');
        var controller = createController();
        var survey = {};
        survey.id = 1;
        scope.surveyResults(survey);
        expect(setcalled).toHaveBeenCalled();
        expect(onpathcalled).toHaveBeenCalledWith('/survey/1/results');
    }));

    it('splitView functions behavior ', inject(function ($rootScope, $controller) {
        var setcalled = spyOn(surveyservice, "setSurvey");
        var onpathcalled = spyOn(loc, 'path');
        var controller = createController();
        var survey = {};
        survey.id = 1;
        scope.splitView(survey);
        expect(setcalled).toHaveBeenCalled();
        expect(onpathcalled).toHaveBeenCalledWith('/splitview/1');
    }));

});



describe('SurveyResultsController', function () {
    var scope, createController, surveyservice;

    beforeEach(function () {

        angular.mock.module('ngRoute', 'ngCookies', 'myApp');
        inject(function ($rootScope, $controller, $injector, $http, $location, $routeParams, $timeout, SurveyService, ClientService) {

            scope = $rootScope.$new();
            timeout = $timeout;
            routeParams = $routeParams;
            loc = $location;
            http = $http;
            cservice = $injector.get('ClientService');
            surveyservice = $injector.get('SurveyService');           
            createController = function () {
                return $controller('SurveyResultsController', {
                    '$scope': scope,
                    '$http': http,
                    '$routeParams': routeParams,
                    '$location': loc,
                     timeout: $timeout,
                    'ClientService': cservice,                   
                    SurveyService: surveyservice
                });
            };
        })
    });


    it('Controller Existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(controller).toBeDefined();
    }));

    it('List Clients called', inject(function ($rootScope, $controller) {
        var listcalled = spyOn(cservice, "list");     
        var controller = createController();
        expect(listcalled).toHaveBeenCalled();
    }));

    it('backToSurvey behaviour', inject(function ($rootScope, $controller) {
        var listcalled = spyOn(loc, "path");
        var controller = createController();
        scope.surveyid = 1;
        scope.backToSurvey();       
        expect(listcalled).toHaveBeenCalledWith('/survey/1');        
    }));

});



describe('SurveyEditController', function () {
    var scope, createController, loc, surveyservice;
    beforeEach(function () {
        angular.mock.module('ngRoute', 'ngCookies', 'myApp');
        inject(function ($rootScope, $controller, $injector, $http, $location) {
            scope = $rootScope.$new();
            loc = $location;
            http = $http;
            surveyservice = $injector.get('SurveyService');
            createController = function () {
                return $controller('SurveyEditController', {
                    '$scope': scope,
                    '$http': http,
                    '$location': loc,
                    SurveyService: surveyservice
                });
            };
        })
    });

    it('Controller Existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(controller).toBeDefined();
    }));

    it('addQuestion behaviour', inject(function ($rootScope, $controller) {
        var updatecalled = spyOn(surveyservice, "update");
        var controller = createController();
        scope.questions = [];
        scope.info = {};
        scope.survey = {};
        scope.newQuestion = "new";       
        scope.addQuestion();
        expect(updatecalled).toHaveBeenCalled();

        expect(scope.questions).toEqual(
            [{
                question: "new",
                id: MD5("new"),
                type: "text",
                options:[]
            }]
        );    
    }));

    it('removeQuestion behaviour', inject(function ($rootScope, $controller) {
        var c = window.confirm;
        window.confirm = function (txt) { console.log(txt); return true; }
        var updatecalled = spyOn(surveyservice, "update");
        var controller = createController();
        scope.questions = [{}, {}];
        scope.info = {};
        scope.survey = {};        
        scope.removeQuestion();
        expect(updatecalled).toHaveBeenCalled();
        expect(scope.questions).toEqual([{}]);
        window.confirm = c;
    }));

});



describe('ClientEditController', function () {
    var scope, createController, loc;
    beforeEach(function () {
        angular.mock.module('ngRoute', 'ngCookies', 'myApp');
        inject(function ($rootScope, $controller, $injector, $http, $routeParams, $location) {
            scope = $rootScope.$new();
            loc = $location;
            routeParams = $routeParams;
            http = $http;
            clientservice = $injector.get('ClientService');
            createController = function () {
                return $controller('ClientEditController', {
                    '$scope': scope,
                    '$http': http,
                    '$location': loc,
                    "$routeParams": routeParams,
                    ClientService: clientservice
                });
            };
        })
    });

    it('Controller Existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(controller).toBeDefined();
    }));

    it('Get Clients called', inject(function ($rootScope, $controller) {
        var listcalled = spyOn(clientservice, "get");
        var controller = createController();
        expect(listcalled).toHaveBeenCalled();
    }));

   

    it('editClient behaviour', inject(function ($rootScope, $controller) {
        var updatecalled = spyOn(clientservice, "update");
        var controller = createController();
        scope.editClient();
        expect(updatecalled).toHaveBeenCalled();
    }));

});