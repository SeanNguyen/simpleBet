'use-strict';
var app = angular.module('app');

//TODO: move this to "value" in app, and the same one in betCreatorController
var PARTICIPATION_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2,
    VOTED: 3
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

        //input model
        $scope.input = { option: null };

        //function
        $scope.nextTab = nextTab;
        $scope.lastTab = lastTab;
        $scope.isCreator = isCreator;
        $scope.isLoggedIn = isLoggedIn;
        $scope.getState = getState;
        $scope.isParticipant = isParticipant;
        $scope.selectOption = selectOption;
        $scope.confirmSelectOption = confirmSelectOption;
        $scope.getParticipantByOption = getParticipantByOption;

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
        
        function isParticipant() {
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if ($rootScope.user.Id === participation.UserId) {
                    return true;
                }
            }
            return false;
        }

        function accept() {
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if ($rootScope.user.Id === participation.UserId) {
                    participation.State = PARTICIPATION_STATE.CONFIRMED;
                }
            }
            $scope.bet.$update();
        }

        function decline() {
            //TODO: update to database
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if ($rootScope.user.Id === participation.UserId) {
                    $scope.bet.Participations.splice(i, 1);
                }
            }
        }

        function selectOption(option) {
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if ($rootScope.user.Id === participation.UserId && participation.State !== PARTICIPATION_STATE.CONFIRMED) {
                    return;
                }
            }
            if ($scope.input.option === option) {
                $scope.input.option = null;
            } else {
                $scope.input.option = option;
            }
        }

        function confirmSelectOption() {
            //TODO: update to database
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if ($rootScope.user.Id === participation.UserId) {
                    participation.Option = $scope.input.option.Content;
                    participation.State = PARTICIPATION_STATE.VOTED;
                    $scope.input.option = null;
                    //save to server
                    var participationResource = new BetUser({
                        BetId: participation.BetId,
                        UserId: participation.UserId,
                        Option: participation.Option,
                        State: participation.State
                    });
                    participationResource.$update();
                }
            }
        }

        function getParticipantByOption(option) {
            var result = [];
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if (option.Content === participation.Option) {
                    result.push(participation)
                }
            }
            return result;
        }

        function getState() {
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if ($rootScope.user.Id === participation.UserId) {
                    return participation.State;
                }
            }
        }
    }]);