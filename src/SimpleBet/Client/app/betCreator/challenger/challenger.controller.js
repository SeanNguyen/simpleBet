'use-strict';
var app = angular.module('app');

app.controller('challengerController', function($rootScope, $scope) {
	$scope.input = {friendList: ''}
	$scope.currentTab = 0;
	var choosenFriends = [];

	$scope.setTab = function(index) {
		$scope.currentTab = index;
	}

	$scope.onFriendSelect = function(friend) {
		if(!friend.selected) {
			resetSearchField();
			$scope.input.friendList += friend.name + ",";
			choosenFriends.push(friend);
		} else {
			$scope.input.friendList = $scope.input.friendList.replace(friend.name + ",", '');
			var friendIndex = getChoosenFriendIndexById(friend.id);
			choosenFriends.splice(friendIndex, 1);
		}
		friend.selected = !friend.selected;
	}

	$scope.isInSearch = function(name) {
		var keyword = $scope.input.friendList;
		for (var i = choosenFriends.length - 1; i >= 0; i--) {
			keyword = keyword.replace(choosenFriends[i].name + ",", '');
		};

		//until here we have the real keyword
		keyword = keyword.toLowerCase();
		name = name.toLowerCase();
		var isContain = name.search(keyword) !== -1;
		return isContain;
	}

	//private helper methods
	var getChoosenFriendIndexById = function(tagId) {
		for (var i = choosenFriends.length - 1; i >= 0; i--) {
			if(choosenFriends[i].id === tagId) {
				return i;
			}
		};
	}

	var resetSearchField = function() {
		$scope.input.friendList = '';
		for (var i = choosenFriends.length - 1; i >= 0; i--) {
			$scope.input.friendList += choosenFriends[i].name + ","
		};
	}

	$scope.submitBet = function() {
		//betApi.add($scope.bet);
		var ids = [];
		for (var i = choosenFriends.length - 1; i >= 0; i--) {
			ids.push(choosenFriends[i].id);
		};
		var tagIds = ids.join(",");
		FB.api(
		 "/me/feed",
		 "POST",
		 {
		     message: "This is a test message which going to be change",
		     place: "1424132167909654", //this is our page id TODO: move this to config
		     tags: tagIds,
		     privacy: {
		         value: "SELF"
		     },
		     link: "http://192.168.0.113:9001/bet/1"
		 },
		 function (response) {
		     console.log(response);
		     if (response && !response.error) {
		         /* handle the result */
		     }
		 }
		);
	}
}); 