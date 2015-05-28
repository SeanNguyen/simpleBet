var app = angular.module('app');

app.controller('wagerController', function($scope) {
	var STATE = {none: 'none', monetary: 'monetary', nonMonetary: 'nonMonetary'};

	$scope.state = STATE.none;

    $scope.setWagerType = function(type) {
    	if(type === 0) {
    		$scope.state = STATE.monetary;
    	} else if (type === 1) {
    		$scope.state = STATE.nonMonetary;
    	}
    }
});