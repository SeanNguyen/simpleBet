'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('create', {
      url: "/create",
      templateUrl: "app/betCreator/betCreator.html",
      controller: 'betCreatorController'
    })
});