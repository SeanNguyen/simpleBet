'use-strict';
var app = angular.module('app');

app.config(function ($stateProvider) {
    // Now set up the states
    $stateProvider
      .state('root.create.wager.monetary', {
          url: "/monetary",
          templateUrl: "app/betCreator/wager/monetary/monetary.html",
          controller: 'monetaryController'
      })
});