'use-strict';
var app = angular.module('app');

app.config(function ($stateProvider) {
    // Now set up the states
    $stateProvider
        .state('root', {
            url: "",
            abstract: true,
            templateUrl: "app/root/root.html",
            controller: 'appController',
            resolve: {
                init: function ($rootScope, $window, facebook, User, $q) {

                    //init root
                    $rootScope.user = {};
                    $rootScope.facebookLoginStatus = {};
                    $rootScope.loginStatus = {};
                    $rootScope.loaded = false;

                    var promise = facebook.init()
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

                    return promise;
                }
            }
        })
});