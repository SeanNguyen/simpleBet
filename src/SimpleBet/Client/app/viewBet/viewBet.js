'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('create.challenger', {
      url: "/challenger",
      templateUrl: "app/betCreator/challenger/challenger.html",
      controller: 'challengerController'
    })
});