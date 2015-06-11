'use-strict';
var app = angular.module('app');

//TODO: move this to "value" in app, and the same one in betCreatorController
var PARTICIPATION_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2,
    CREATOR: 3
};

app.controller('viewBetController', ['$rootScope', '$scope', '$stateParams', 'Bet', 'User', 'BetUser',
    function ($rootScope, $scope, $stateParams, Bet, User, BetUser) {
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
        $scope.isCreator = isCreator;
        $scope.isLoggedIn = isLoggedIn;
        $scope.accept = accept;
        $scope.decline = decline;

        //start the controller
        active();
        
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

        function isCreator() {
            if ($rootScope.user && $rootScope.user.Id === $scope.bet.CreatorId) {
                return true;
            }
            return false;
        }

        function isLoggedIn() {
            if ($rootScope.user && $rootScope.user.Id) {
                return true;
            }
            return false;
        }

        function accept() {
            var participation = getParticipationFromBet($rootScope.user.Id);
            if (participation) {
                var participationModel = BetUser({
                    BetId: participation.BetId,
                    UserId: participation.UserId,
                    State: PARTICIPATION_STATE.CONFIRMED
                });
                participationModel.update();
            }
        }

        function decline() {

        }

        function getParticipationFromBet(id) {
            $scope.bet.Participations.forEach(function (element, index, array) {
                if (id === element.UserId) {
                    return element;
                }
            });
            return null;
        }
    }]);