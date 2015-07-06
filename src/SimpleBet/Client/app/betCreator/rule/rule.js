'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('root.create.rule', {
      url: "/rule",
      templateUrl: "app/betCreator/rule/rule.html",
      controller: 'ruleController'
    })
});