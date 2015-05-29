var app = angular.module('app');

app.controller('nonmonetaryController', function($scope) {
	$scope.tabs = [{title: 'Our Dares', imgage: ''}, {title: 'Custom Dares', image: ''}];	
	$scope.currentTab = 0;
	$scope.ourItems = [{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'A Noble King', description: 'Donate all of your valubles in your wallet to a nearby beggar.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Static Shock ', description: 'Get eletricfied in a thunderstorm. Zap Zap pikachu I choose you.', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' },
					{title: 'Catch‘em All', description: 'Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)', avata: 'assets/icon_giftBox.png' }]

	$scope.customItems = [{title: 'Get Rekt Fool', description: 'Loser has to get wrecked like a wrecking ball. Wearing wrecking ball clothes.', avata: 'assets/x_button.png' },
						{title: 'Self Input Title', description: 'Ut doluptatem demolessi dolescid qui alicit debitiore voloratibus ipsandandit la net quis receprat hiliquisquid que pererum quidipsam, con', avata: 'assets/x_button.png' },
						{title: 'Self Input Title', description: 'Amenit ipsapitatem susdandebit ut il inus resto offictotas eum volume voluptatqui aut que core percius etur reperci psanienda qui venimet lam', avata: 'assets/x_button.png' }];

	$scope.isTabselected = function (index) {
		if(index === $scope.currentTab) {
			return true;
		}
		return false;
	}

	$scope.setTab = function (index) {
		$scope.currentTab = index;
	}
});