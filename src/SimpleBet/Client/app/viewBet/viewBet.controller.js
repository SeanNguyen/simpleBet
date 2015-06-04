'use-strict';
var app = angular.module('app');

app.controller('viewBetController', ['$rootScope', '$scope', 'betApi', 'userApi', 
	function($rootScope, $scope, betApi, userApi) {
		if(!$scope.id) {
			//handle invalid link here
		}
		$scope.bet = betApi.get($scope.id);
		$scope.creator = userApi.get($scope.bet.creatorId);
		$scope.loginStatus = false;
}]); 