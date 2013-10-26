var app = angular.module('teamsauce', ['ratings']);

app.controller('testCtrl', function ($scope) {
    $scope.user_rating = 0;
    $scope.id = 1;
});

app.controller('questionnaireCtrl', ['$scope', '$rootScope', function ($scope, $rootScope) {
    var answers = [];
    
    var proxy = $.connection.questionnaireHub;

    $.connection.hub.start();

    $scope.questions = [
        { categoryType: "Hunger", text: 'How hungry are you feeling?' },
        { categoryType: "Cats", text: 'How does this cat make you feel?' }];

    $rootScope.$on('ratingUpdate', function (broadcastData, answer) {
        answers.push(answer);
    });

    $scope.go = function() {

        var questionnaireId = "4f231fec-afbf-421b-a6c8-8caceeb745ae";

        var questionnaireResponse = {
            username: "Adam",
            ratings: answers
        };

        proxy.server.complete(questionnaireId, JSON.stringify(questionnaireResponse));
    };
}]);