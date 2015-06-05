'use-strict';

var app = angular.module('app');

app.controller('betCreatorController', function($rootScope, $scope, $state, $window, betApi, $location) {

	//some static constants
	var PATH_TAB_DONE = 'assets/icon_tab_done.png';
	var PATH_TAB_UNDONE = 'assets/icon_tab_undone.png';
	var PATH_TAB_CURRENT = 'assets/icon_tab_current.png';
	var TAB_SIZE = 5;
	var TAB_NAMES = ['The Bet', 'The Bet', 'The Wager', 'The Rule', 'The Challengers'];
	var TAB_STATES = ['question', 'option', 'wager', 'rule', 'challenger'];

	$scope.getTabStatusIcon = function(tabIndex) {
		if (tabIndex < $scope.currentTab) {
			return PATH_TAB_DONE;
		}
		if (tabIndex === $scope.currentTab) {
			return PATH_TAB_CURRENT;
		}
		return PATH_TAB_UNDONE;
	}

	$scope.setTab = function(tabIndex) {
		if (tabIndex < TAB_SIZE && tabIndex > -1) {
			$scope.currentTab = tabIndex;
			$rootScope.title = TAB_NAMES[$scope.currentTab];
			$state.go('create.' + TAB_STATES[tabIndex]);
			$window.scrollTo(0,0); 
		}
	}

	$scope.addOption = function() {
		if(!$scope.input.option || $scope.input.option.lenght <= 0) {
			return;
		}
		$scope.betModel.addOption($scope.input.option);
		$scope.input.option = '';
	}

	$scope.removeOption = function(index) {
		$scope.betModel.removeOption(index);
	}

	$scope.isTypeSelected = function(optionIndex) {
		var maxTeamSizeForFirstTeam = $scope.betModel.getMaxTeamSize(0);
		if (optionIndex === 0 && maxTeamSizeForFirstTeam === 1) {
			return true;
		} else if (optionIndex === 1 && maxTeamSizeForFirstTeam > 1) {
			return true;
		}
		return false;
	}

	$scope.setBetType = function(type) {
		if (type === 0) {
			$scope.betModel.setMaxTeamSize(0, 1);
		} else {
			$scope.betModel.setMaxTeamSize(0, 10);
		}
	}


	$scope.submitBet = function () {
	    var ids = [];
	    for (var i = $scope.betModel.participants.length - 1; i >= 0; i--) {
	        ids.push($scope.betModel.participants[i].id);
	    };
	    var tagIds = ids.join(",");

	    betApi.add($scope.betModel, function (betId) {
	        FB.api(
		        "/me/feed",
		        "POST",
		        {
		            message: "This is a test message which going to be change: " + $scope.betModel.question,
		            place: "1424132167909654", //this is our page id TODO: move this to config
		            tags: tagIds,
		            privacy: {
		                value: "SELF"
		            },
		            link: "http://192.168.0.113:9000/#/bet/" + betId
		        },
		        function (response) {
		            console.log(response);
		            if (response && !response.error) {
		                /* handle the result */
		            }
		        });
	        $location.path('bet/' + betId);
	    });
	    
	}

	//Constructor
	$scope.currentTab = 0;
	$scope.input = {option:''};
	$rootScope.title = TAB_NAMES[$scope.currentTab];
	$scope.betModel = new BetModel();
	$scope.setBetType(0);
	$scope.setTab(0);

});