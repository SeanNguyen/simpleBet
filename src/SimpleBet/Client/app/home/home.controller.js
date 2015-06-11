'use-strict';

var app = angular.module('app');

app.controller('homeController', function ($rootScope, $scope, facebook) {
    $scope.logIn = function () {
        facebook.logIn().then(function () {
            $rootScope.$apply();
        });
    }
});