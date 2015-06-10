'use-strict';
var app = angular.module('app');

app.controller('challengerController', function($rootScope, $scope) {
	$scope.input = {friendList: ''}
	$scope.currentTab = 0;
	$scope.$parent.betModel.Participants = [];

	$scope.setTab = function(index) {
		$scope.currentTab = index;
	}

	$scope.onFriendSelect = function(friend) {
		if(!friend.selected) {
			resetSearchField();
			$scope.input.friendList += friend.name + ",";
			$scope.$parent.betModel.Participants.push(friend);
		} else {
			$scope.input.friendList = $scope.input.friendList.replace(friend.name + ",", '');
			var friendIndex = getChoosenFriendIndexById(friend.id);
			$scope.$parent.betModel.Participants.splice(friendIndex, 1);
		}
		friend.selected = !friend.selected;
	}

	$scope.isInSearch = function(name) {
		var keyword = $scope.input.friendList;
		for (var i = $scope.$parent.betModel.Participants.length - 1; i >= 0; i--) {
		    keyword = keyword.replace($scope.$parent.betModel.Participants[i].name + ",", '');
		};

		//until here we have the real keyword
		keyword = keyword.toLowerCase();
		name = name.toLowerCase();
		var isContain = name.search(keyword) !== -1;
		return isContain;
	}

	//private helper methods
	var getChoosenFriendIndexById = function(tagId) {
	    for (var i = $scope.$parent.betModel.Participants.length - 1; i >= 0; i--) {
	        if ($scope.$parent.betModel.Participants[i].id === tagId) {
				return i;
			}
		};
	}

	var resetSearchField = function() {
		$scope.input.friendList = '';
		for (var i = $scope.$parent.betModel.Participants.length - 1; i >= 0; i--) {
		    $scope.input.friendList += $scope.$parent.betModel.Participants[i].name + ","
		};
	}
}); 