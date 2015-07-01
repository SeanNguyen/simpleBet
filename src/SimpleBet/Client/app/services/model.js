'user-strict'

var app = angular.module('app');

app.factory('User', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/user/:id', { id: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);

app.factory('Bet', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/bet/:id', { id: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);

app.factory('BetUser', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/betUser/:betId/:userId', { betId: '@betId', userId: '@userId' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);

app.factory('WinningItem', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/winningItem/:id', { id: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);