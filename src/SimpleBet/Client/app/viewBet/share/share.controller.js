'use-strict';
var app = angular.module('app');

app.controller('shareController', ['$rootScope', '$scope', '$http', '$state', 'facebook', shareController]);

function shareController($rootScope, $scope, $http, $state, facebook) {
    //attributes
    $scope.gifs;
    $scope.selection = -1;
    $scope.input = { query: "", message: ""};
    $scope.state = 1;
    $scope.image = { src: null, link: ''};

    //functions
    $scope.select = select;
    $scope.isSelected = isSelected;
    $scope.search = search;
    $scope.share = share;
    $scope.changeToSelectGif = changeToSelectGif;
    $scope.changeToComposingView = changeToComposingView;

    //implementations
    var timeStamp;
    function search(query) {
        $scope.selection = -1;
        timeStamp = new Date().getTime();
        setTimeout(function() {
        var currentTime = new Date().getTime();
        if(currentTime - timeStamp > 500 && query && query !== '') {
            query.replace(' ', '+');
            //TODO move this shit out of the ctroller
            $http.get('http://api.giphy.com/v1/gifs/search?q=' + query + '&limit=20&api_key=dc6zaTOxFJmzC').
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
        $scope.image.src = $scope.gifs[index].images.fixed_width.url;
        $scope.image.link = $scope.gifs[index].images.original.url;
    }

    function changeToComposingView() {
        $scope.state = 1;
    }

    function changeToSelectGif() {
        $scope.state = 2;
    }

    function isSelected(index) {
      return index === $scope.selection;
    }

    function share() {
        $rootScope.loaded = false;

        //get tag ids
        var tagId = [];
        for (var i = $rootScope.taggableFriends.length - 1; i >= 0 ; i--) {
            if (isParticipant($rootScope.taggableFriends[i].name)) {
                tagId.push($rootScope.taggableFriends[i].tagId);
            }
        }

        facebook.post($scope.input.message, $scope.image.link, tagId).then(function () {
            $rootScope.loaded = true;
            $state.go("root.bet");
        });
    }

    //private helper methods
    function isParticipant(name) {
        for(var i = $scope.bet.participations.length - 1; i >= 0; i--) {
            if (name === $scope.bet.participations[i].user.name) {
                return true;
            }
        }
        return false;
    }
}