'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('home', {
      url: "/",
      templateUrl: "app/home/home.html",
      controller: 'homeController'
    })
});