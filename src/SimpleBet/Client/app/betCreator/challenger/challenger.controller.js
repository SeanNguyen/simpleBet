'use-strict';
var app = angular.module('app');

app.controller('challengerController', ['$rootScope', '$scope', challengerController]); 

var visibleRange_min = 10;

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

	active();

    //private helper methods
	function setTab(index) {
	    $scope.currentTab = index;
	}

	function onFriendSelect(friend) {
	    if (!friend.selected) {
	        resetSearchField();
	        $scope.input.friendList += friend.name + ",";
	        $scope.input.participants.push(friend);
	    } else {
	        $scope.input.friendList = $scope.input.friendList.replace(friend.name + ",", '');
	        var friendIndex = getChoosenFriendIndexById(friend.id);
	        $scope.input.participants.splice(friendIndex, 1);
	    }
	    friend.selected = !friend.selected;
	}

	function getChoosenFriendIndexById(tagId) {
	    for (var i = $scope.input.participants.length - 1; i >= 0; i--) {
	        if ($scope.input.participants[i].id === tagId) {
	            return i;
	        }
	    };
	}

	function resetSearchField() {
	    $scope.input.friendList = '';
	    for (var i = $scope.input.participants.length - 1; i >= 0; i--) {
	        $scope.input.friendList += $scope.input.participants[i].name + ","
	    };
	}

	function onSearchChange(query) {
	    $scope.visibleFriends = [];
	    query = query.toLowerCase();
	    for (var i = $scope.friends.length - 1; i >= 0; i--) {
	        var friendName = $scope.friends[i].name.toLowerCase();
	        if (friendName.indexOf(query) > -1) {
	            $scope.visibleFriends.push($scope.friends[i]);
	        }
	    }
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
};