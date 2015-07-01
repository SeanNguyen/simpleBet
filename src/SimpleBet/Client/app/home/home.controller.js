'use-strict';

var app = angular.module('app');

app.controller('homeController', function ($rootScope, $scope, facebook, $q) {
    $scope.logIn = function () {
        $rootScope.loaded = false;
        facebook.logIn()
        .then(function (response) {
            var promise = $q.all([  facebook.updateRootUserByFacebookId(response.authResponse.userID),
                                    facebook.queryFacebookFriends()]);
            return promise;
        }).then(function () {
            $rootScope.loaded = true;
        }).catch(function () {
            console.log("Cant log in into Facebook");
            $rootScope.loaded = true;
        });
    }
});