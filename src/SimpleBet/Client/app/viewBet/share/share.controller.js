'use-strict';
var app = angular.module('app');

app.controller('shareController', ['$rootScope', '$scope', '$http', '$state', 'facebook', shareController]);

function shareController($rootScope, $scope, $http, $state, facebook) {
    //attributes
    $scope.gifs;
    $scope.selection = -1;
    $scope.input = {query: ""};

    //functions
    $scope.select = select;
    $scope.isSelected = isSelected;
    $scope.search = search;
    $scope.share = share;

    //implementations
    var timeStamp;
    function search(query) {
      timeStamp = new Date().getTime();
      setTimeout(function() {
        var currentTime = new Date().getTime();
        if(currentTime - timeStamp > 500 && query && query !== '') {
          query.replace(' ', '+');
          //TODO move this shit out of the ctroller
          $http.get('http://api.giphy.com/v1/gifs/search?q=' + query + '&api_key=dc6zaTOxFJmzC').
            success(function (data, status, headers, config) {
                $scope.gifs = data.data;
                console.log($scope.gifs);
            }).
            error(function (data, status, headers, config) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });  
          }
      }, 500);
    }

    function select(index) {
      $scope.selection = index;
    }

    function isSelected(index) {
      return index === $scope.selection;
    }

    function share() {
      if($scope.selection > -1) {
        var gif = $scope.gifs[$scope.selection];
        //post here
        facebook.post();
        $state.go("bet");
      }
    }
}