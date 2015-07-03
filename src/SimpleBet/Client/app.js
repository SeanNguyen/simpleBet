'use-strict';

var app = angular.module('app', ['ui.router', 'ngResource', 'angular-loading-bar', 'ngAnimate', 'ngMaterial']);

app.config(function ($stateProvider, $urlRouterProvider, $sceDelegateProvider, cfpLoadingBarProvider) {
    $urlRouterProvider.otherwise("/");
    //TODO: restrict this white list down to only the site we need
    $sceDelegateProvider.resourceUrlWhitelist(['**']);

    //config loading bar
    cfpLoadingBarProvider.includeSpinner = false;
});

app.controller('appController', function ($rootScope, $scope, $location) {
    $scope.isNavbarVisible = function () {
        var currentPath = $location.url();
        return currentPath !== "/";
    }
});

app.run(['$rootScope', '$window', 'facebook', 'User', '$q',
    function ($rootScope, $window, facebook, User, $q) {
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
                $q.all([
                    facebook.updateRootUserByFacebookId(response.authResponse.userID),
                    facebook.queryFacebookFriends()
                ])
                .then(function () {
                    $rootScope.loaded = true;
                });

            } else if (response.status === 'not_authorized' || response.status === 'unknown') {
                //haven't connect, let show the login button
                $rootScope.user = {};
                $rootScope.loaded = true;
            }
        })
        .catch(function (e) {
            console.log(e); // "oh, no!"
            $rootScope.loaded = true;
        });

        $(function () {
            FastClick.attach(document.body);
        });
    }]);