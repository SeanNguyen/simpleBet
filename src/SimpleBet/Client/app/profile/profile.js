'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('profile', {
      url: "/profile",
      templateUrl: "app/profile/profile.html"
    })
});