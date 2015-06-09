var app = angular.module('app');

app.factory('facebook', ['$q', '$rootScope', function ($q, $rootScope) {

    var init = function () {
        var deferred = $q.defer();
        //Facebook Config
        window.fbAsyncInit = function () {
            FB.init({
                appId: '1462927764001322',
                cookie: true,
                xfbml: true,
                version: 'v2.3'
            });
            deferred.resolve();
        };

        (function (d) {
            // load the Facebook javascript SDK
            var js,
            id = 'facebook-jssdk',
            ref = d.getElementsByTagName('script')[0];

            if (d.getElementById(id)) {
                return;
            }

            js = d.createElement('script');
            js.id = id;
            js.async = true;
            js.src = "//connect.facebook.net/en_US/sdk.js";

            ref.parentNode.insertBefore(js, ref);

        }(document));

        return deferred.promise;
    }

    var getLoginStatus = function () {
        var deferred = $q.defer();
        FB.getLoginStatus(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    var getUserInfo = function (userId) {
        var deferred = $q.defer();
        FB.api('/' + userId, function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    return {
        init: init,
        getLoginStatus: getLoginStatus,
        getUserInfo: getUserInfo
    };
}]);