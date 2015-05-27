'use-strict';

var app = angular.module('app');

app.controller('BetCreatorController', function($scope) {
    $scope.currentView = 2;
    $scope.question = 'What the colour of the next car passing by would be';
    $scope.options = [  {content: 'Maximum Number of characters is fifty incluing sp' },
                        {content: "It's a red car"}]
});