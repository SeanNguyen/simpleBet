'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('root.create.option', {
      url: "/option",
      templateUrl: "app/betCreator/option/option.html"
    })
});