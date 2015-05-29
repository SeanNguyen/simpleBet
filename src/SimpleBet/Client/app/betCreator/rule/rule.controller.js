'use-strict';

var app = angular.module('app');

app.controller('ruleController', function($scope) {
	$scope.Math = Math;
	$scope.thirdNavbar = {title: 'Set Up', image: ''};
	$scope.time = 2;//time in hours

	$scope.increaseHour = function() {
		$scope.time++;
	};

	$scope.decreaseHour = function() {
		if ($scope.time > 0) {
			$scope.time--;
		}
	};

	$scope.increaseDay = function() {
		$scope.time += 24
	};

	$scope.decreaseDay = function() {
		if($scope.time > 24) {
			$scope.time--;
		} else {
			$scope.time = 0;
		}
	};
});