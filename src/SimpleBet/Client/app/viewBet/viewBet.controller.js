'use-strict';
var app = angular.module('app');

app.controller('viewBetController', ['$rootScope', '$scope', '$stateParams', 'Bet', 'User',
    function ($rootScope, $scope, $stateParams, Bet, User) {
        if (!$stateParams.id) {
            //handle invalid link here
        }
        $scope.creator = {};
        $scope.bet = Bet.get({ id: $stateParams.id }, function () {
            $scope.creator = User.get({ id: $scope.bet.CreatorId });
        });
        
        $scope.loginStatus = false;
        $rootScope.title = "Bet";
    }]);