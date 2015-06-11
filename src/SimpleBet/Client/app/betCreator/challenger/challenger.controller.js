'use-strict';
var app = angular.module('app');

app.controller('challengerController', function($rootScope, $scope) {
	$scope.currentTab = 0;

    //functions
	$scope.setTab = setTab;
	$scope.onFriendSelect = onFriendSelect;
	$scope.isInSearch = isInSearch;

    //private helper methods
	function setTab(index) {
	    $scope.currentTab = index;
	}

	function onFriendSelect(friend) {
	    if (!friend.selected) {
	        resetSearchField();
	        $scope.input.friendList += friend.name + ",";
	        $scope.input.Participants.push(friend);
	    } else {
	        $scope.input.friendList = $scope.input.friendList.replace(friend.name + ",", '');
	        var friendIndex = getChoosenFriendIndexById(friend.id);
	        $scope.input.Participants.splice(friendIndex, 1);
	    }
	    friend.selected = !friend.selected;
	}

	function isInSearch(name) {
	    var keyword = $scope.input.friendList;
	    for (var i = $scope.input.Participants.length - 1; i >= 0; i--) {
	        keyword = keyword.replace($scope.input.Participants[i].name + ",", '');
	    };

	    //until here we have the real keyword
	    keyword = keyword.toLowerCase();
	    name = name.toLowerCase();
	    var isContain = name.search(keyword) !== -1;
	    return isContain;
	}

	function getChoosenFriendIndexById(tagId) {
	    for (var i = $scope.input.Participants.length - 1; i >= 0; i--) {
	        if ($scope.input.Participants[i].id === tagId) {
	            return i;
	        }
	    };
	}

	function resetSearchField() {
	    $scope.input.friendList = '';
	    for (var i = $scope.input.Participants.length - 1; i >= 0; i--) {
	        $scope.input.friendList += $scope.input.Participants[i].name + ","
	    };
	}
});