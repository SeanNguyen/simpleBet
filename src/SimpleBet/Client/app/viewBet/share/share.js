'use-strict';
var app = angular.module('app');

app.config(function ($stateProvider) {
    // Now set up the states
    $stateProvider
      .state('root.bet.share', {
          url: "/share",
          templateUrl: "app/viewBet/share/share.html",
          controller: 'shareController'
      })
});