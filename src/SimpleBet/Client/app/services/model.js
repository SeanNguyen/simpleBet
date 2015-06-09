'user-strict'

var app = angular.module('app');

app.factory('User', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/user/:id', { id: '@Id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);

app.factory('Bet', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/bet/:id');
}]);