'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('create.rule', {
      url: "/rule",
      templateUrl: "app/betCreator/rule/rule.html",
      controller: 'ruleController'
    })
});