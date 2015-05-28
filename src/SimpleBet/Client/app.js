'use strict';

var app = angular.module('app', ['ngRoute']);

app.config(['$routeProvider',
	function($routeProvider) {
		$routeProvider.
			when('/', {
				templateUrl: 'app/home/home.html'
			}).
			when('/create', {
				templateUrl: 'app/betCreator/betCreator.html',
				controller: 'betCreatorController'
			}).
			otherwise({
				redirectTo: '/'
			});
	}]);

app.controller('appController', function($rootScope, $scope, $location) {
	$scope.isNavbarVisible = function() {
		var currentPath = $location.url();
		return currentPath !== "/";
	}
});