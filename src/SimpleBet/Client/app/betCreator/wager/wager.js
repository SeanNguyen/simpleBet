'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('create.wager', {
      url: "/wager",
      templateUrl: "app/betCreator/wager/wager.html",
      controller: 'wagerController'
    })
});