var app = angular.module('teamsauce', ['ratings', 'ui.bootstrap']);

app.controller('testCtrl', function ($scope) {
    $scope.user_rating = 0;
    $scope.id = 1;
});