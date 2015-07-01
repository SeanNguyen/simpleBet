'use-strict';
var app = angular.module('app');

app.controller('viewBetController', ['$rootScope', '$scope', '$stateParams', 'Bet', 'User',
    'BetUser', '$timeout', '$window', '$state', 'facebook', '$q', 'PARTICIPATION_STATE', 'BET_STATE',
    'VOTE_CANCEL_BET_STATE', viewBetController]);

function viewBetController($rootScope, $scope, $stateParams, Bet, User, BetUser, $timeout, $window,
    $state, facebook, $q, PARTICIPATION_STATE, BET_STATE, VOTE_CANCEL_BET_STATE) {
    $scope.tabs = [
        { name: 'Bet Conditions' },
        { name: 'Bet Wager' },
        { name: 'Challengers Invited' }]

    $rootScope.title = "Bet";
    $scope.currentTab = 0;
    $scope.creator = {};
    $scope.betCancelCreator;
    $scope.bet = {};
    $scope.BET_STATE = BET_STATE;
    $scope.VOTE_CANCEL_BET_STATE = VOTE_CANCEL_BET_STATE;

    //input model
    $scope.input = { option: null };

    //function
    $scope.nextTab = nextTab;
    $scope.lastTab = lastTab;
    $scope.isCreator = isCreator;
    $scope.isLoggedIn = isLoggedIn;
    $scope.isParticipant = isParticipant;
    $scope.selectOption = selectOption;
    $scope.confirmSelectOption = confirmSelectOption;
    $scope.getParticipantByOption = getParticipantByOption;
    $scope.cancelBet = cancelBet;
    $scope.getControlButtonState = getControlButtonState;

    $scope.onAgreeCancelBet = onAgreeCancelBet;
    $scope.onDisagreeCancelBet = onDisagreeCancelBet;

    $scope.accept = accept;
    $scope.decline = decline;

    $scope.finalize = finalize;
    $scope.share = share;
    $scope.logIn = logIn;

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
        var participant = getParticipationByUserId($rootScope.user.id);
        participant.state = PARTICIPATION_STATE.DECLINED;
        BetUser.update(participant);
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

    function cancelBet() {
        var participation = getParticipationByUserId($rootScope.user.id);
        if (participation) {
            participation.voteCancelBetState = VOTE_CANCEL_BET_STATE.CREATOR;
            var participationModel = new BetUser(participation);
            participationModel.$update();

            //ok, now we can set the bet cancelling creator
            updateCancellingAlert();

            if ($scope.bet.participations.length > 2) {
                $scope.bet.state = BET_STATE.CANCELLING;
            } else {
                $scope.bet.state = BET_STATE.CANCELLED;
            }
            $scope.bet.$update();
        }
    }

    function onFinallizeSelect() {

    }

    function getCancellingPerson() {

    }

    function getControlButtonState() {
        if (!isLoggedIn()) {
            return 0; //0: havent logged in yet
        }
        var participation = getParticipationByUserId($rootScope.user.id);
        if (!participation) {
            return 1; //1: logged in but not in the bet, show no button
        }
        if (participation.state === PARTICIPATION_STATE.PENDING) {
            return 2; //2: is in the bet but havent accepted
        }
        if($scope.input.option) {
            return 3; //choosing some option
        }
        if ($scope.bet.state === BET_STATE.CONFIRMED || $scope.bet.state === BET_STATE.PENDING
            || $scope.bet.state === BET_STATE.FINALIZABLE) {
            return 4; //when not choosing option
        }
        if ($scope.bet.state === BET_STATE.CANCELLING && participation.voteCancelBetState === VOTE_CANCEL_BET_STATE.NONE) {
            return 5; //cancelling a bet and havent voted
        }
        if ($scope.bet.state === BET_STATE.FINALIZED) {
            return 6;
        }
    }

    function onAgreeCancelBet() {
        var participation = getParticipationByUserId($rootScope.user.id);
        if (participation) {
            participation.voteCancelBetState = VOTE_CANCEL_BET_STATE.AGREE;
            var participationModel = new BetUser(participation);
            participationModel.$update();
            updateBetCancellingStatus();
            //ok, now we can set the bet cancelling vote
            updateCancellingAlert();
        }
    }

    function onDisagreeCancelBet() {
        var participation = getParticipationByUserId($rootScope.user.id);
        if (participation) {
            participation.voteCancelBetState = VOTE_CANCEL_BET_STATE.DISAGREE;
            var participationModel = new BetUser(participation);
            participationModel.$update();
            updateBetCancellingStatus();
            //ok, now we can set the bet cancelling vote
            updateCancellingAlert();
        }
    }

    function finalize() {
        $scope.bet.state = BET_STATE.FINALIZED;
        $scope.bet.$update();
        $window.scrollTo(0, 0);
    }

    function share() {
        $state.go('bet.share');
    }

    function logIn() {
        $rootScope.loaded = false;
        facebook.logIn()
        .then(function (response) {
            var promise = $q.all([facebook.updateRootUserByFacebookId(response.authResponse.userID),
                                    facebook.queryFacebookFriends()]);
            return promise;
        }).then(function () {
            $rootScope.loaded = true;
        }).catch(function () {
            console.log("Cant log in into Facebook");
            $rootScope.loaded = true;
        });
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

    function updateBetCancellingStatus() {
        var agreeCount = 0;
        var disagreeCount = 0;
        for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
            var participation = $scope.bet.participations[i];
            if (participation.voteCancelBetState === VOTE_CANCEL_BET_STATE.CREATOR 
                || participation.voteCancelBetState === VOTE_CANCEL_BET_STATE.AGREE) {
                agreeCount++;
            } else if (participation.voteCancelBetState === VOTE_CANCEL_BET_STATE.DISAGREE) {
                disagreeCount++;
            }
        }
        var total = $scope.bet.participations.length;
        if (agreeCount / total >= 0.5) {
            $scope.bet.state = BET_STATE.CANCELLED;
            $scope.betCancelCreator = null;
        } else if (disagreeCount / total >= 0.5) {
            //This was moved to server side
            //reset cancelling state of all participants
            //for (var i = $scope.bet.participations.length - 1; i >= 0; i--) {
            //    $scope.bet.participations[i].voteCancelBetState = VOTE_CANCEL_BET_STATE.NONE;
            //    var model = new BetUser($scope.bet.participations[i]);
            //    model.$update();
            //}

            //short circuit for responsive effect
            $scope.bet.state = BET_STATE.CONFIRMED;
            $scope.betCancelCreator = null;
        }
    }
}