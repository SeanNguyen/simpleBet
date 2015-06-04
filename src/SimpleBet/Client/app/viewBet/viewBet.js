'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('bet', {
      url: "/bet",
      templateUrl: "app/viewBet/viewBet.html",
      controller: 'viewBetController'
    })
});