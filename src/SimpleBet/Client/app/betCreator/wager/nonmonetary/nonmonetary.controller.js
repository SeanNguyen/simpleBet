'use-strict';

var app = angular.module('app');

app.controller('nonmonetaryController', ['$rootScope', '$scope', '$window', '$state', 'WinningItem', 'WINNING_ITEM_TYPE', nonmonetaryControllerfunction]);

function nonmonetaryControllerfunction($rootScope, $scope, $window, $state, WinningItem, WINNING_ITEM_TYPE) {
    var STATE = { select: 'select', create: 'create' };

    $scope.input = { newDare: { title: '', description: '', avata: 'assets/x_button.png' } };
    $scope.tabs = [{ title: 'Our Dares', imgage: '' }, { title: 'Custom Dares', image: '' }];
    $scope.currentTab = 0;
    $scope.currentState = STATE.select;
    $scope.selectedDare;
    $scope.thirdNavbar = { title: 'Your New Dare', image: 'assets/revertSubmit_button.png' }

    $scope.ourItems = WinningItem.query({ creatorId: 1 });
    $scope.customItems = WinningItem.query({ creatorId: $rootScope.user.id });

    $scope.isTabselected = function (index) {
        if (index === $scope.currentTab) {
            return true;
        }
        return false;
    }

    $scope.setTab = function (index) {
        $scope.currentTab = index;
        $window.scrollTo(0, 0);
    }

	$scope.setState = function (state) {
		if(state === STATE.select || state === STATE.create) {
			$scope.currentState = state;
			$window.scrollTo(0,0); 
		}
	}

	$scope.addDare = function (dare) {
		if(dare && dare.title.length > 0 && dare.title.length < 30 && dare.description.length > 0) {
		    //save the dare to server
		    $rootScope.loaded = false;
		    $scope.input.newDare.type = WINNING_ITEM_TYPE.NONMONETARY;
		    $scope.input.newDare.creatorId = $rootScope.user.id;
		    $scope.input.newDare = new WinningItem($scope.input.newDare);
		    $scope.input.newDare.$save()
            .then(function () {
                $rootScope.loaded = true;
                $scope.customItems.push($scope.input.newDare);
                $scope.input.newDare = { title: '', description: '' };
                $scope.setState(STATE.select);
            })
            .catch(function () {
                $rootScope.loaded = true;
                console.log('cannot save bet');
                $scope.input.newDare = { title: '', description: '' };
                $scope.setState(STATE.select);
            });
		}
	}

	$scope.removeCustomDare = function (index) {
	    if (index > -1 && index < $scope.customItems.length) {
	        var dare = $scope.customItems[index];
	        dare.$delete();
			$scope.customItems.splice(index, 1);
		}
	}

	$scope.selectDare = function (dare) {
	    $scope.selectedDare = dare;
	    $scope.betModel.winningItemId = dare.id;
	    $scope.$parent.$parent.setTab(3);
	}
};