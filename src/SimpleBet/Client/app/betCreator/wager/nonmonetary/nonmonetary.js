'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('root.create.wager.nonmonetary', {
      url: "/nonmonetary",
      templateUrl: "app/betCreator/wager/nonmonetary/nonmonetary.html",
      controller: 'nonmonetaryController'
    })
});