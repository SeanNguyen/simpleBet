'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('root.bet', {
      url: "/bet/:id",
      templateUrl: "app/viewBet/viewBet.html",
      controller: 'viewBetController'
    })
});