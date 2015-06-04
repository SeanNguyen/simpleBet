'use-strict';
var app = angular.module('app');

app.controller('viewBetController', function($rootScope, $scope) {
	$scope.creator = {name: "Some Dude", avataUrl: "http://png-1.findicons.com/files/icons/1072/face_avatars/300/i04.png"}
	$scope.bet = {question: "This is a question !!!", duration: "2 days"}
}); 