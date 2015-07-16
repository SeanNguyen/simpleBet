(function () {
    'user-strict'

    var app = angular.module('app');
    app.factory('Config', ['$location', Config]);

    function Config($location) {

        var facebook = {};
        if ($location.host() === 'localhost' && $location.port() === 14248) {
            facebook.appId = '1480828435544588';
        } else if ($location.host() === '192.168.0.113' && $location.port() === 9000) {
            facebook.appId = '1486054991688599';
        } else if ($location.host() === 'fluttr.azurewebsites.net') {
            facebook.appId = '1466436923650406';
        } else if ($location.host() === 'simplebet.arcadier.com') {
            facebook.appId = '1462927764001322';
        } else {
            console.log('ERR: host or port is not valid');
        }

        return {
            facebook: facebook
        }
    }
})();