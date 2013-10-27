﻿var app;

app = angular.module('ratings', []);

app.directive("angularRatings", function () {
    return {
        restrict: 'E',
        scope: {
            model: '=ngModel',
            notifyId: '=notifyId'
        },
        replace: true,
        transclude: true,
        template: '<div><ol class="angular-ratings">' + '<li ng-class="{active:model>0,over:over>0}">1</li>' + '<li ng-class="{active:model>1,over:over>1}">2</li>' + '<li ng-class="{active:model>2,over:over>2}">3</li>' + '<li ng-class="{active:model>3,over:over>3}">4</li>' + '<li ng-class="{active:model>4,over:over>4}">5</li>' + '</ol></div>',
        controller: function ($scope) {
            $scope.over = 1;
            $scope.setRating = function (category, rating) {
                $scope.model = rating;
                this.$emit('ratingUpdate', { "categoryType": category, "value": rating });
                $scope.$apply();
            };
            return $scope.setOver = function (n) {
                $scope.over = n;
                return $scope.$apply();
            };
        },
        link: function (scope, iElem, iAttrs) {
            if (iAttrs.notifyUrl !== void 0) {
                return angular.forEach(iElem.children(), function (ol) {
                    return angular.forEach(ol.children, function (li) {
                        li.addEventListener('mouseover', function () {
                            return scope.setOver(parseInt(li.innerHTML));
                        });
                        li.addEventListener('mouseout', function () {
                            return scope.setOver(0);
                        });
                        return li.addEventListener('click', function () {
                            return scope.setRating(iAttrs.category, parseInt(li.innerHTML));
                        });
                    });
                });
            }
        }
    };
});