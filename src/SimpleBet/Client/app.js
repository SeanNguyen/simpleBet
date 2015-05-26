'use strict';

var app = angular.module('app', ['ngRoute']);

app.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/', {
        templateUrl: 'app/home/home.html',
        controller: 'HomeController'
      }).
      when('/create', {
        templateUrl: 'app/betCreator/betCreator.html',
        controller: 'BetCreatorController'
      }).
      otherwise({
        redirectTo: '/'
      });
  }]);