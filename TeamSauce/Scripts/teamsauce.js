var app = angular.module('teamsauce', ['ratings']);

//app.controller('testCtrl', function ($scope) {
//    $scope.user_rating = 0;
//    $scope.id = 1;
//});
app.controller('QuestionnaireCtrl', ['$scope', 'questionnaireService', function($scope, questionnaireService) {
    questionnaireService.connect();

    $scope.questions = [
        { text: 'how hungry are you feeling?' },
        { text: 'build an angular app' }];

    $scope.go = function () {
        questionnaireService.complete(undefined);
    };
}]);

app.factory('questionnaireService', ['$', '$rootScope', function ($, $rootScope) {
    var proxy;
    var connection;
    return {
        connect: function () {
            connection = $.hubConnection();
            proxy = connection.createHubProxy('questionnaire');
            connection.start();
            proxy.on('complete', function (questionnaire) {
                $rootScope.$broadcast('complete', questionnaire);
            });
        },
        isConnecting: function () {
            return connection.state === 0;
        },
        isConnected: function () {
            return connection.state === 1;
        },
        connectionState: function () {
            return connection.state;
        },
        complete: function (questionnaire) {
            proxy.invoke('complete', questionnaire);
        },
    };
}]);

//function QuestionnaireCtrl($scope, questionnaireService) {
//    questionnaireService.connect();

//    $scope.questions = [
//        { text: 'how hungry are you feeling?' },
//        { text: 'build an angular app' }];

//    $scope.go = function () {
//        questionnaireService.complete(undefined);
//    };
//}

//QuestionnaireCtrl.$inject = ['$scope', 'questionnaireService'];