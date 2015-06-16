'use-strict';
var app = angular.module('app');

app.controller('shareController', ['$rootScope', '$scope', '$http', shareController]);

function shareController($rootScope, $scope, $http) {
    $scope.gifs;

    $http.get('http://api.giphy.com/v1/gifs/search?q=funny+cat&api_key=dc6zaTOxFJmzC').
      success(function (data, status, headers, config) {
          $scope.gifs = data.data;
          console.log($scope.gifs);
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
      });
}