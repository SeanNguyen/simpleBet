'use-strict';
var app = angular.module('app');

app.controller('challengerController', ['$rootScope', '$scope', challengerController]); 

var visibleRange_min = 20;

function challengerController ($rootScope, $scope) {

    $scope.currentTab = 0;
    $scope.friends = [];
    $scope.visibleFriends = [];
    $scope.visibleRange = 0;
     
    //functions
	$scope.setTab = setTab;
	$scope.onFriendSelect = onFriendSelect;
	$scope.onSearchChange = onSearchChange;
	$scope.canSubmit = canSubmit;
	$scope.increaseVisibleLimit = increaseVisibleLimit;
	$scope.onSubmitSearch = onSearchChange;

	active();

    //private helper methods
	function onSubmitSearch(chip) {
	    return chip;
	}

	function onSearchChange(chip) {
	    return chip;
	}

	function setTab(index) {
	    $scope.currentTab = index;
	}

	function onFriendSelect(friend) {
	    if (!isFriendSelected(friend)) {
	        $scope.input.participants.push(friend);
	    } else {
	        for (var i = $scope.input.participants.length - 1; i >= 0; i--) {
	            if ($scope.input.participants[i] === friend)
	                $scope.input.participants.splice(i, 1);
	        }
	    }
	}

	function isFriendSelected(friend) {
	    for (var i = $scope.input.participants.length - 1; i >= 0; i--) {
	        if ($scope.input.participants[i] === friend)
	            return true;
	    }
	}

	function onSearchChange(query) {
	    //remove all choosen friend's names
	    for (var i = $scope.input.participants.length - 1; i >= 0; i--) {
	        var name = $scope.input.participants[i].name;
	        query = query.replace(name + ',', '')
	    }

        //then search the real query
	    $scope.visibleFriends = [];
	    query = query.toLowerCase();
	    if (query === '') {
	        $scope.visibleFriends = $scope.friends;
	    } else {
	        for (var i = $scope.friends.length - 1; i >= 0; i--) {
	            var friendName = $scope.friends[i].name.toLowerCase();
	            if (friendName.indexOf(query) > -1) {
	                $scope.visibleFriends.push($scope.friends[i]);
	            }
	        }
	    }
        
	    //reset the visible limit
	    $scope.visibleRange = visibleRange_min;
	}
    
	function active() {
	    $scope.friends = $rootScope.taggableFriends
	    $scope.visibleFriends = $scope.friends;
	    $scope.visibleRange =visibleRange_min;
	}

	function canSubmit() {
	    if ($scope.input.participants && $scope.input.participants.length > 0) {
	        return true;
	    }
	    return false;
	}

	function increaseVisibleLimit() {
	    $scope.visibleRange += visibleRange_min;
	}
};