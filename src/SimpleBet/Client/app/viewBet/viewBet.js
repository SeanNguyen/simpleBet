'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('bet', {
      url: "/bet/:id",
      templateUrl: "app/viewBet/viewBet.html",
      controller: 'viewBetController'
    })
});