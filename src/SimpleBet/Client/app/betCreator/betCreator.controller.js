'use-strict';

var app = angular.module('app');

//some static constants
var PATH_TAB_DONE = 'assets/icon_tab_done.png';
var PATH_TAB_UNDONE = 'assets/icon_tab_undone.png';
var PATH_TAB_CURRENT = 'assets/icon_tab_current.png';
var TAB_SIZE = 5;
var TAB_NAMES = ['The Bet', 'The Bet', 'The Wager', 'The Rule', 'The Challengers'];
var TAB_STATES = ['question', 'option', 'wager', 'rule', 'challenger'];

app.controller('betCreatorController', betCreatorController);

function betCreatorController($rootScope, $scope, $state, $window, $location, Bet, User, BetUser,
    facebook, BET_TYPE, WAGER_TYPE, BET_STATE, PARTICIPATION_STATE, focus, ngDialog, $anchorScroll) {
    //navigations
    $scope.currentTab = 0;
    $rootScope.title = TAB_NAMES[$scope.currentTab];
    $scope.Math = Math;
    $scope.thirdNavbar = { title: 'Set Up', image: '' };

    //bet model
    $scope.betModel = new Bet({ 
        options: [{content: ''}],
        pendingDuration: 120,
        participations: [], //this field will actually be on the server database, dont be confuse with the field above
        state: BET_STATE.NONE,
        //TODO: #367 add  function to load data before controller or app so that this user will be alr loaded here
        //CreatorId: $rootScope.user.Id 
    });

    //input
    $scope.input = {
        friendList: '',
        option: null,
        participants: []
    }

    //functions
    $scope.getTabStatusIcon = getTabStatusIcon;
    $scope.setTab = setTab;

    $scope.addOption = addOption;
    $scope.removeOption = removeOption;
    $scope.submitQuestionAndOptions = submitQuestionAndOptions;
    $scope.isOptionsValid = isOptionsValid;
    $scope.isQuestionValid = isQuestionValid;
    $scope.selectOption = selectOption;

    $scope.isTypeSelected = isTypeSelected;
    $scope.setBetType = setBetType;

    $scope.setWagerType = setWagerType;

    $scope.submitBet = submitBet;

    //TODO: minify this to just 1 single method
    $scope.increaseHour = increaseHour;
    $scope.decreaseHour = decreaseHour;
    $scope.increaseDay = increaseDay;
    $scope.decreaseDay = decreaseDay;
    $scope.increaseMinute = increaseMinute;
    $scope.decreaseMinute = decreaseMinute;

    active();

    //DETAIL

    function submitQuestionAndOptions() {
        if (!isOptionsValid()) {
            ngDialog.open({
                template: 'app/betCreator/option/optionNotValidDialog.html',
                appendTo: "#optionView",
                data: {message: 'Your options is not valid'}
            });
            return;
        }

        if (!isQuestionValid()) {
            ngDialog.open({
                template: 'app/betCreator/option/optionNotValidDialog.html',
                appendTo: "#optionView",
                data: { message: 'Your question is not valid' }
            });
            return;
        }

        if (!$scope.input.option || $scope.input.option.content.length == 0) {
            ngDialog.open({
                template: 'app/betCreator/option/optionNotValidDialog.html',
                appendTo: "#optionView",
                data: { message: 'You must choose an option' }
            });
            return;
        }

        if (!$scope.betModel.question || $scope.betModel.question.length === 0) {
            return;
        }
        if (!$scope.betModel.options || $scope.betModel.options.length === 0) {
            return;
        }
        setTab(2);
    }

    function increaseHour() {
        $scope.betModel.pendingDuration += 60;
    };

    function decreaseHour() {
        if ($scope.betModel.pendingDuration > 60) {
            $scope.betModel.pendingDuration -= 60;
        } else {
            $scope.betModel.pendingDuration = 0;
        }
    };

    function increaseDay() {
        $scope.betModel.pendingDuration += 24 * 60;
    }

    function decreaseDay() {
        if ($scope.betModel.pendingDuration > 24 * 60) {
            $scope.betModel.pendingDuration -= 24 * 60;
        } else {
            $scope.betModel.pendingDuration = 0;
        }
    }

    function increaseMinute() {
        $scope.betModel.pendingDuration += 5;
    }

    function decreaseMinute() {
        if ($scope.betModel.pendingDuration > 5) {
            $scope.betModel.pendingDuration -= 5;
        } else {
            $scope.betModel.pendingDuration = 0;
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
			$state.go('root.create.' + TAB_STATES[tabIndex]);
			$window.scrollTo(0,0); 
		}
	}

	function addOption() {
	    var lastOption = $scope.betModel.options[$scope.betModel.options.length - 1];
	    if (lastOption && (!lastOption.content || lastOption.content.length <= 0
            || $scope.betModel.options.length >= 10 || isOptionDumplicate(lastOption))) {
	        console.log("Warning: option input invalid");

	        if (!isOptionsValid()) {
	            ngDialog.open({
	                template: 'app/betCreator/option/optionNotValidDialog.html',
	                appendTo: "#optionView",
	                data: { message: 'Your options is not valid' }
	            });
	            return;
	        }

			return;
	    }
	    var newOption = {content: ''};
	    $scope.betModel.options.push(newOption);

	    var focusId = 'option' + ($scope.betModel.options.length - 1);
	    focus(focusId);

	    //scroll to bottom
	    $location.hash('pageEnd');
	    $anchorScroll();
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
	    if (type === BET_TYPE.ONE_MANY || type === BET_TYPE.MANY_MANY) {
	        $scope.betModel.betType = type;

	        if (type === BET_TYPE.ONE_MANY) {
	            $scope.betModel.options = [{ content: 'Me' }, { content: 'The World' }];
	            $scope.input.option = $scope.betModel.options[0];
	        }

	        setTab(1);
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

	function isOptionsValid() {
	    if (!$scope.betModel.options ||$scope.betModel.options.length < 2 || $scope.betModel.options.length > 10) {
	        return false;
	    }
	    for (var i = $scope.betModel.options.length - 1; i >= 0; i--) {
	        var option = $scope.betModel.options[i];
	        if (!option.content || option.content.length <= 0 || isOptionDumplicate(option)) {
	            return false;
	        }
	    }
	    return true;
	}

	function isQuestionValid() {
	    if ($scope.betModel.question && $scope.betModel.question.length > 0 && $scope.betModel.question.length <= 100) {
	        return true;
	    }
	    return false;
	}

	function selectOption(option) {
	    $scope.input.option = option;
	}

    //TODO: this function make me look fat, break it down
	function submitBet() {

	    $rootScope.loaded = false;

	    //TODO: remove this line when the #367 solved

	    $scope.betModel.creatorId = $rootScope.user.id;
	    $scope.betModel.$save()
        .then(function () {
	        var link = "http://" + $location.$$host + ":" + $location.$$port + "/#/bet/" + $scope.betModel.id; //TODO: move this link into the config file

	        var message = "JOIN THE BET NOW !!! \n" + $scope.betModel.question + "\n" + link;
	        
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
	                            avatarUrl: findUserFromTaggedUserList($scope.input.participants, taggedFriends[i].name).avatarUrl,
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
                                        state: PARTICIPATION_STATE.VOTED,
                                        option: $scope.input.option.content
                                    });
                                    $scope.betModel.state = BET_STATE.PENDING;

                                    //yay now we can save the betModel
                                    $scope.betModel.$update(function () {
                                        //after settle down everything, relocate to the bet view
                                        $rootScope.loaded = true;
                                        $location.path('bet/' + $scope.betModel.id);
                                    })
	                            }

	                        }, function () {
	                            console.log("ERROR: cannot save new user");
	                        });
	                    }
	                } else {
	                    console.log("ERROR: get post from facebook");
	                    console.log(response.error);
	                    $rootScope.loaded = true;
	                }
	            })
	        } else {
	            console.log("ERROR: post to facebook");
	            console.log(response.error);
	            $rootScope.loaded = true;
	        }
	    });
	}

	function active() {
	    setTab(0);
	}

	function isOptionDumplicate(option) {
	    for (var i = $scope.betModel.options.length - 1; i >= 0; i--) {
	        if (option.content === $scope.betModel.options[i].content && option !== $scope.betModel.options[i]) {
	            return true;
	        }
	    }
	    return false;
	}

    //private helper methods
	function findUserFromTaggedUserList(users, name) {
	    for (var i = users.length - 1; i >= 0; i--) {
	        if(users[i].name === name)
                return users[i]
	    }
	}
}