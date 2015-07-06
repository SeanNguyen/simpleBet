'use-strict';
var app = angular.module('app');

app.controller('appController', function ($rootScope, $scope, $location) {
    $scope.isNavbarVisible = function () {
        var currentPath = $location.url();
        return currentPath !== "/";
    }
});