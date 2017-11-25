
describe('ClientController', function () {
    var scope, createController, sservice;

    beforeEach(function () {
        
        angular.mock.module('ngRoute', 'ngCookies','myApp');
        inject(function ($rootScope, $controller, $injector, $http, $routeParams) {

             scope = $rootScope.$new();
            
             routeParams = $routeParams;
             http = $http;
             cservice = $injector.get('ClientService');
             sservice = $injector.get('SurveyService');
             mservice = $injector.get('MailService');
            
            createController = function () {
                return $controller('ClientController', {
                    '$scope': scope,
                    '$http': http,
                    '$routeParams': routeParams,
                    'ClientService': cservice,
                    SurveyService: sservice,
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

    it('Scope  functions behavior [new,delete,mailall]', inject(function ($rootScope, $controller) {
        var clientcreatecalled = spyOn(cservice, "create");
        var clientdeletecalled = spyOn(cservice, "delete");
        var controller = createController();
        scope.newClient();
        expect(clientcreatecalled).toHaveBeenCalled();
        var c = window.confirm;
        window.confirm = function (txt) { console.log(txt); return true;}        
        scope.deleteClient();
        expect(clientdeletecalled).toHaveBeenCalled();
        window.confirm = c;
    }));

   

    it('SurveyService called if survey null', inject(function ($rootScope, $controller) {
        var onservicegetcalledSpy = spyOn(sservice, "get");        
        var controller = createController();             
        expect(onservicegetcalledSpy).toHaveBeenCalled();
    }));

    it('Maillall mailallsent', inject(function ($rootScope, $controller) {
        spyOn(sservice, "getSurvey").and.returnValue({           
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

});



describe('SurveyController', function () {
    var scope, createController, loc, sservice;
    beforeEach(function () {
        angular.mock.module('ngRoute', 'ngCookies', 'myApp');
        inject(function ($rootScope, $controller, $injector,$http, $location) {
            scope = $rootScope.$new();
            loc = $location;
            http = $http;
            sservice = $injector.get('SurveyService');
            createController = function () {
                return $controller('SurveyController', {
                    '$scope': scope,
                    '$http': http,
                    '$location': loc,
                    SurveyService: sservice
                });
            };
        })
    });

    it('Controller Existence', inject(function ($rootScope, $controller) {
        var controller = createController();
        expect(controller).toBeDefined();
    }));


  
});
