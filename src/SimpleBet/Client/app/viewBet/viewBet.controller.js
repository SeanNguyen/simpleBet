'use-strict';
var app = angular.module('app');

//TODO: move this to "value" in app, and the same one in betCreatorController
var PARTICIPATION_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2,
    VOTED: 3
};
var BET_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2,
    CANCELLING: 3
};
var VOTE_CANCEL_BET_STATE = {
    NONE: 0,
    CREATOR: 1,
    DISAGREE: 2,
    AGREE: 3,
}

app.controller('viewBetController', ['$rootScope', '$scope', '$stateParams', 'Bet', 'User', 'BetUser', '$timeout',
    function ($rootScope, $scope, $stateParams, Bet, User, BetUser, $timeout) {
        $scope.tabs = [
            { name: 'Bet Conditions' },
            { name: 'Bet Wager' },
            { name: 'Challengers Invited' }]

        $rootScope.title = "Bet";
        $scope.currentTab = 0;
        $scope.creator = {};
        $scope.betCancelCreator;
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
        $scope.cancelBet = cancelBet;

        $scope.accept = accept;
        $scope.decline = decline;

        //start the controller
        active();
        
        //public method implementations
        function active() {
            $scope.bet = Bet.get({ id: $stateParams.id }, function () {
                $scope.creator = User.get({ id: $scope.bet.creatorId });
                if ($scope.bet.state === BET_STATE.CANCELLING) {
                    updateCancellingAlert();
                }
            });
            intervalUpdateBet();
        }

        function intervalUpdateBet() {
            $timeout(function () {
                $scope.bet.$get({ id: $scope.bet.id }, function () {
                    if ($scope.bet.state === BET_STATE.CANCELLING) {
                        updateCancellingAlert();
                    }
                    intervalUpdateBet();
                })
            }, 5000);
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
            if ($rootScope.user && $rootScope.user.id === $scope.bet.creatorId) {
                return true;
            }
            return false;
        }

        function isLoggedIn() {
            if ($rootScope.user && $rootScope.user.id) {
                return true;
            }
            return false;
        }
        
        function isParticipant() {
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if ($rootScope.user.id === participation.userId) {
                    return true;
                }
            }
            return false;
        }

        function accept() {
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if ($rootScope.user.id === participation.userId) {
                    participation.state = PARTICIPATION_STATE.CONFIRMED;
                }
            }
            $scope.bet.$update();
        }

        function decline() {
            //TODO: update to database
            for (var i = $scope.bet.Participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.Participations[i];
                if ($rootScope.user.id === participation.userId) {
                    $scope.bet.Participations.splice(i, 1);
                }
            }
        }

        function selectOption(option) {
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if ($rootScope.user.id === participation.userId && participation.state !== PARTICIPATION_STATE.CONFIRMED) {
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
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if ($rootScope.user.id === participation.userId) {
                    participation.option = $scope.input.option.content;
                    participation.state = PARTICIPATION_STATE.VOTED;
                    $scope.input.option = null;
                    //save to server
                    var participationResource = new BetUser(participation);
                    participationResource.$update();
                }
            }
        }

        function getParticipantByOption(option) {
            var result = [];
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if (option.content === participation.option) {
                    result.push(participation)
                }
            }
            return result;
        }

        function getState() {
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if ($rootScope.user.id === participation.userId) {
                    return participation.state;
                }
            }
        }

        function cancelBet() {
            $scope.bet.state = BET_STATE.CANCELLING;
            $scope.bet.$update();

            var participation = getParticipationByUserId($rootScope.user.id);
            if (participation) {
                participation.voteCancelBetState = VOTE_CANCEL_BET_STATE.CREATOR;
                var participationModel = new BetUser(participation);
                participationModel.$update();
            }

            //ok, now we can set the bet cancelling creator
            updateCancellingAlert();
        }

        function onFinallizeSelect() {

        }

        function getCancellingPerson() {

        }

        //private helper methods
        function getParticipationByUserId(userId) {
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if (userId === participation.userId) {
                    return participation;
                }
            }
        }

        function updateCancellingAlert() {
            for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
                var participation = $scope.bet.participations[i];
                if (participation.voteCancelBetState === VOTE_CANCEL_BET_STATE.CREATOR) {
                    $scope.betCancelCreator = _.clone(participation.user);
                }
            }
        }
    }]);