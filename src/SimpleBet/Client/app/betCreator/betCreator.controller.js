'use-strict';

var app = angular.module('app');

//some static constants
var PATH_TAB_DONE = 'assets/icon_tab_done.png';
var PATH_TAB_UNDONE = 'assets/icon_tab_undone.png';
var PATH_TAB_CURRENT = 'assets/icon_tab_current.png';
var TAB_SIZE = 5;
var TAB_NAMES = ['The Bet', 'The Bet', 'The Wager', 'The Rule', 'The Challengers'];
var TAB_STATES = ['question', 'option', 'wager', 'rule', 'challenger'];
var BET_TYPE = { MVW: 'MVW', PAS: 'PAS' };
var WAGER_TYPE = { MONETARY: 'MONETARY', NONMONETARY: 'NONMONETARY' };
var PARTICIPATION_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2
};
var BET_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2,
    CANCELLING: 3,
    FINALIZABLE: 4,
    FINALIZED: 5
}
app.controller('betCreatorController', function ($rootScope, $scope, $state, $window, $location, Bet, User, BetUser, facebook) {
    //navigations
    $scope.currentTab = 0;
    $rootScope.title = TAB_NAMES[$scope.currentTab];
    $scope.Math = Math;
    $scope.thirdNavbar = { title: 'Set Up', image: '' };

    //bet model
    $scope.betModel = new Bet({ 
        options: [], 
        duration: 2, /*in hour, TODO: change to minutes*/
        participations: [], //this field will actually be on the server database, dont be confuse with the field above
        state:BET_STATE.NONE
        //TODO: #367 add  function to load data before controller or app so that this user will be alr loaded here
        //CreatorId: $rootScope.user.Id 
    });

    //input
    $scope.input = {
        friendList: '',
        option: '',
        participants: []
    }

    //functions
    $scope.getTabStatusIcon = getTabStatusIcon;
    $scope.setTab = setTab;

    $scope.addOption = addOption;
    $scope.removeOption = removeOption;

    $scope.isTypeSelected = isTypeSelected;
    $scope.setBetType = setBetType;

    $scope.setWagerType = setWagerType;

    $scope.submitBet = submitBet;

    //TODO: minify this to just 1 single method
    $scope.increaseHour = increaseHour;
    $scope.decreaseHour = decreaseHour;
    $scope.increaseDay = increaseDay;
    $scope.decreaseDay = decreaseDay;

    active();

    //DETAIL

    function increaseHour() {
        $scope.betModel.duration ++;
    };

    function decreaseHour() {
        if ($scope.betModel.duration > 0) {
            $scope.betModel.duration--;
        }
    };

    function increaseDay() {
        $scope.betModel.duration += 24
    }

    function decreaseDay() {
        if ($scope.betModel.duration > 24) {
            $scope.betModel.duration -= 24;
        } else {
            $scope.betModel.duration = 0;
        }
    }

	function getTabStatusIcon(tabIndex) {
		if (tabIndex < $scope.currentTab) {
			return PATH_TAB_DONE;
		}
		if (tabIndex === $scope.currentTab) {
			return PATH_TAB_CURRENT;
		}
		return PATH_TAB_UNDONE;
	}

	function setTab(tabIndex) {
		if (tabIndex < TAB_SIZE && tabIndex > -1) {
			$scope.currentTab = tabIndex;
			$rootScope.title = TAB_NAMES[$scope.currentTab];
			$state.go('create.' + TAB_STATES[tabIndex]);
			$window.scrollTo(0,0); 
		}
	}

	function addOption() {
	    if (!$scope.input.option || $scope.input.option.lenght <= 0) {
	        console.log("Warning: option input invalid");
			return;
	    }
	    var option = { content: $scope.input.option };
		$scope.betModel.options.push(option);
		$scope.input.option = '';
	}

	function removeOption(index) {
		$scope.betModel.options.splice(index, 1);
	}

	function isTypeSelected(type) {
	    if (type === $scope.betModel.type) {
	        return true;
	    }
	    return false;
	}
    
	function setBetType(type) {
	    if (type === BET_TYPE.MVW || type === BET_TYPE.PAS) {
	        $scope.betModel.type = type;
	    } else {
	        console.log("ERROR: type input not valid");
	    }
	}

	function setWagerType(type) {
	    if (type === WAGER_TYPE.MONETARY || type === WAGER_TYPE.NONMONETARY) {
	        $scope.betModel.wagerType = type;
	    } else {
	        console.log("ERROR: type input not valid");
	    }
	}

    //TODO: this function make me look fat, break it down
	function submitBet() {

        //TODO: remove this line when the #367 solved
	    $scope.betModel.creatorId = $rootScope.user.id;

        //save bet without any edges to user just to get the ID first
	    $scope.betModel.$save()
        .then(function () {
	        var link = "192.168.0.113:9000/#/bet/" + $scope.betModel.id //TODO: move this link into the config file

	        var message = "JOIN THE BET NOW !!! \n" + $scope.betModel.question;
	        
	        var tagIds = [];
	        for (var i = $scope.input.participants.length - 1; i >= 0; i--) {
	            tagIds.push($scope.input.participants[i].tagId);
	        };
	        tagIds = tagIds.join(",");

	        var promise = facebook.post(message, link, tagIds);
	        return promise;
	    })
	    .then(function (response) {
            //after post successfully onto facebook, get tagged friends ID and add to our awesome system
	        if (response && !response.error) {
                //TODO: move this to facebook service
	            FB.api(response.id, function (response) {
	                if (response && !response.error) {
	                    //register all friend tagged with the database
	                    var taggedFriends = response.with_tags.data;
	                    var friendSaveCount = 0;
	                    for (var i = taggedFriends.length - 1; i >= 0; i--) {
	                        //create new user
	                        var friend = new User({
	                            facebookId: taggedFriends[i].id,
	                            avatarUrl: $scope.input.participants[i].avatarUrl,
	                            name: taggedFriends[i].name,
	                        });
	                        friend.$save(function (data) {
	                            //added edges to the betModel
	                            $scope.betModel.participations.push({
	                                userId: data.id,
	                                betId: $scope.betModel.id,
	                                state: PARTICIPATION_STATE.PENDING
	                            });

	                            //TODO: improve this hack around (at first this is to wait until all the new users saved)
                                //TODO: to use promise instead of this piramid of callback >"<
	                            friendSaveCount++;
                                if (friendSaveCount === taggedFriends.length) {
                                    //have to add creator to the connection edge as well
                                    $scope.betModel.participations.push({
                                        userId: $scope.betModel.creatorId,
                                        betId: $scope.betModel.id,
                                        state: PARTICIPATION_STATE.CONFIRMED
                                    });
                                    $scope.betModel.state = BET_STATE.PENDING;

                                    //TODO: add loading screen for this and the friend saving above instead of jump directly to the bet pagex
                                    //yay now we can save the betModel
                                    $scope.betModel.$update(function () {
                                        //after settle down everything, relocate to the bet view
                                        $location.path('bet/' + $scope.betModel.id);
                                    });
	                            }

	                        }, function () {
	                            console.log("ERROR: cannot save new user");
	                        });
	                    }
	                } else {
	                    console.log("ERROR: get post from facebook");
	                    console.log(response.error);
	                }
	            })
	        } else {
	            console.log("ERROR: post to facebook");
	            console.log(response.error);
	        }
	    });
	}

	function active() {
	    setTab(0);
	}
});