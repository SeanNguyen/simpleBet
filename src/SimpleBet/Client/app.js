'use-strict';

var app = angular.module('app', ['ui.router', 'ngResource', 'angular-loading-bar', 'ngAnimate']);

app.config(function ($stateProvider, $urlRouterProvider, $sceDelegateProvider) {
    $urlRouterProvider.otherwise("/");
    //TODO: restrict this white list down to only the site we need
    $sceDelegateProvider.resourceUrlWhitelist(['**']);
});

app.controller('appController', function ($rootScope, $scope, $location) {
    $scope.isNavbarVisible = function () {
        var currentPath = $location.url();
        return currentPath !== "/";
    }
});

app.run(['$rootScope', '$window', 'facebook', 'User',
    function ($rootScope, $window, facebook, User) {

        function queryUserByFacebookId(facebookId) {
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
                            $rootScope.user.facebookId = facebookId;
                            $rootScope.user.name = facebookUser.name;
                            $rootScope.user.$save();
                            //TODO: querry correct avatar
                            $rootScope.user.avatarUrl = "http://png-1.findicons.com/files/icons/1072/face_avatars/300/i04.png"
                        });
                    }
                });
        }

        //init root
        $rootScope.user = {};
        $rootScope.facebookLoginStatus = {};
        $rootScope.loginStatus = {};
        $rootScope.loaded = false;

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
                $rootScope.loaded = true;
            }

            //TODO pull facebook friends for user click the login button as well
            //get facebook friends
            FB.api('/me/taggable_friends?limit=10', function (response) {
                $rootScope.taggableFriends = [];
                for (var i = 0; i < response.data.length; i++) {
                    var friendData = response.data[i];
                    var friend = { tagId: friendData.id, name: friendData.name, avatarUrl: friendData.picture.data.url, selected: false };
                    $rootScope.taggableFriends.push(friend);
                }
                $rootScope.$apply();
                $rootScope.loaded = true;
            });
        })
        .catch(function (e) {
            console.log(e); // "oh, no!"
            $rootScope.loaded = true;
        });
    }]);