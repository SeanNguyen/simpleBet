'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('root.create', {
      url: "/create",
      abstract: true,
      templateUrl: "app/betCreator/betCreator.html",
      controller: 'betCreatorController'
    })
});