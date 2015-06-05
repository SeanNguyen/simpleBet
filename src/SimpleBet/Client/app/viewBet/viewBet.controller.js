'use-strict';
var app = angular.module('app');

app.controller('viewBetController', ['$rootScope', '$scope', 'betApi', 'userApi', '$stateParams',
	function ($rootScope, $scope, betApi, userApi, $stateParams) {
	    var constructor = function () {
	        if (!$stateParams.id) {
	            //handle invalid link here
	        }
	        betApi.get($stateParams.id, updateBet);
	        $scope.loginStatus = false;
	        $rootScope.title = "Bet";
	    }
	    
	    //private methods
		var updateBet = function (data) {
		    $scope.bet = data;
		    userApi.get($scope.bet.creatorId, updateCreator);
		}
		var updateCreator = function (data) {
		    $scope.creator = data;
		}

		constructor();
}]); 