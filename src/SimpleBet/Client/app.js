'use-strict';

var app = angular.module('app', ['ui.router', 'ngResource', 'angular-loading-bar', 'ngAnimate']);

app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/");
});

app.controller('appController', function ($rootScope, $scope, $location) {
    $scope.isNavbarVisible = function () {
        var currentPath = $location.url();
        return currentPath !== "/";
    }
});

app.run(['$rootScope', '$window', 'facebook', 'User',
    function ($rootScope, $window, facebook, User) {
        var queryUserByFacebookId = function (facebookId) {
            //alr connected to facebook, let's check if there is any user in the databse or not
            User.get({ id: facebookId })
                .$promise.then(
                function (data) {
                    $rootScope.user = data;
                },
                function (error) {
                    //cant find this fbID, must be new to the town, let's him join
                    if (error.status === 404) {
                        facebook.getUserInfo(facebookId)
                        .then(function (facebookUser) {
                            $rootScope.user = new User();
                            $rootScope.user.FacebookId = facebookId;
                            $rootScope.user.Name = facebookUser.name;
                            $rootScope.user.$save();
                            //TODO: querry correct avatar
                            $rootScope.user.AvatarUrl = "http://png-1.findicons.com/files/icons/1072/face_avatars/300/i04.png"
                        });
                    }
                });
        }

        //init root
        $rootScope.user = {};
        $rootScope.facebookLoginStatus = {};
        $rootScope.loginStatus = {};

        //init facebook
        facebook.init()
        .then(function () {
            return facebook.getLoginStatus();
        })
        .then(function (response) {
            $rootScope.facebookLoginStatus = response;
            if (response.status === 'connected') {
                queryUserByFacebookId(response.authResponse.userID);
            } else if (response.status === 'not_authorized' || response.status === 'unknown') {
                //haven't connect, let show the login button
                $rootScope.user = {};
            }

            //get facebook friends
            FB.api('/me/taggable_friends?limit=5000', function (response) {
                $rootScope.taggableFriends = [];
                for (var i = 0; i < response.data.length; i++) {
                    var friendData = response.data[i];
                    var friend = { tagId: friendData.id, name: friendData.name, AvatarUrl: friendData.picture.data.url, selected: false };
                    $rootScope.taggableFriends.push(friend);
                }
                $rootScope.$apply();
            });
        })
    }]);