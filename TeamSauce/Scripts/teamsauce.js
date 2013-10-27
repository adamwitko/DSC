var app = angular.module('teamsauce', ['ratings', 'ui.bootstrap']);

app.controller('testCtrl', function ($scope) {
    $scope.user_rating = 0;
    $scope.id = 1;
});

app.controller('questionnaireCtrl', ['$scope', '$rootScope', function ($scope, $rootScope) {
    var answers = [];
    
    var proxy = $.connection.questionnaireHub;

    $.connection.hub.start();

    $rootScope.$on('ratingUpdate', function (broadcastData, answer) {
        answers.push(answer);
    });

    proxy.client.sentOutQuestionnaire = function (questionnaire) {
        $scope.questionnaire = questionnaire;
        $scope.questions = $scope.questionnaire.categoryQuestions;
        $('#mymodal').modal('show');
        $scope.$apply();

        $('.star').raty({
            'click': function (score) {

                answers.push({
                    category: $(this).attr('data-category'),
                    value: score
                });
            }
        });
    };
    
    $scope.go = function() {
        var questionnaireResponse = {
            ratings: answers
        };

        proxy.server.complete($scope.questionnaire.id, JSON.stringify(questionnaireResponse));
        
        answers = [];
        $('#mymodal').modal('hide');
    };
    

}]);