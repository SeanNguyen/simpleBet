'use-strict';
var app = angular.module('app');

app.controller('viewBetController', ['$rootScope', '$scope', '$stateParams', 'Bet', 'User',
    function ($rootScope, $scope, $stateParams, Bet, User) {
        $scope.tabs = [
            { name: 'Bet Conditions' },
            { name: 'Bet Wager' },
            { name: 'Challengers Invited' }]

        $rootScope.title = "Bet";
        $scope.currentTab = 0;
        $scope.creator = {};
        $scope.bet = {};

        //function
        $scope.nextTab = nextTab;
        $scope.lastTab = lastTab;

        //start the controller
        active()
        
        //private helper methods
        function active() {
            $scope.bet = Bet.get({ id: $stateParams.id }, function () {
                $scope.creator = User.get({ id: $scope.bet.CreatorId });
            });
        }

        function nextTab() {
            if ($scope.currentTab < $scope.tabs.length - 1) {
                $scope.currentTab++;
            }
        }

        function lastTab() {
            if ($scope.currentTab > 0) {
                $scope.currentTab--;
            }
        }
    }]);