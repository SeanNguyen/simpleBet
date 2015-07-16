'use-strict';
var app = angular.module('app');

app.controller('challengerController', ['$rootScope', '$scope', 'ngDialog', '$anchorScroll', '$location', 'focus', challengerController]);

var visibleRange_min = 20;

function challengerController($rootScope, $scope, ngDialog, $anchorScroll, $location, focus) {

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

	active();

    //private helper methods
	function setTab(index) {
	    $scope.currentTab = index;
	}

	function onFriendSelect(friend) {
	    if (!friend.selected) {
	        if ($scope.input.participants.length >= 9) {
	            ngDialog.open({
	                template: 'app/components/dialogs/messageDialog.html',
	                data: { message: 'You cannot choose more than 9 friends' }
	            });
	            return;
	        }
	        resetSearchField();
	        $scope.input.participants.push(friend);

	        var focusId = 'friendQueryInput';
	        focus(focusId);
	        //scroll to bottom
	        $location.hash('choosenFriendEnd');
	        $anchorScroll();
	    } else {
	        var friendIndex = getChoosenFriendIndexById(friend.tagId);
	        $scope.input.participants.splice(friendIndex, 1);
	        resetSearchField();
	    }
	    friend.selected = !friend.selected;
	    onSearchChange($scope.input.friendList);
	}

	function getChoosenFriendIndexById(tagId) {
	    for (var i = $scope.input.participants.length - 1; i >= 0; i--) {
	        if ($scope.input.participants[i].tagId === tagId) {
	            return i;
	        }
	    };
	}

	function resetSearchField() {
	    $scope.input.friendList = '';
	}

	function onSearchChange(query) {
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