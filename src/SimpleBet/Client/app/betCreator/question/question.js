'use-strict';
var app = angular.module('app');

app.config(function($stateProvider) {
  // Now set up the states
  $stateProvider
    .state('root.create.question', {
      url: "/question",
      templateUrl: "app/betCreator/question/question.html"
    })
});