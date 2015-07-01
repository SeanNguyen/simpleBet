'user-strict'

var app = angular.module('app');

app.factory('User', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/user/:id', { id: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);

//BET
app.value("BET_TYPE", BET_TYPE = { ONE_MANY: 0, MANY_MANY: 1 });
app.value("WAGER_TYPE", WAGER_TYPE = { MONETARY: 0, NONMONETARY: 1 });
app.value("BET_STATE", BET_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2,
    CANCELLING: 3,
    CANCELLED: 4,
    FINALIZABLE: 5,
    FINALIZED: 6
});

app.factory('Bet', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/bet/:id', { id: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);

//BETUSER
app.value('PARTICIPATION_STATE', PARTICIPATION_STATE = {
    NONE: 0,
    PENDING: 1,
    CONFIRMED: 2,
    DECLINED: 3,
    VOTED: 4
});
app.value('VOTE_CANCEL_BET_STATE', VOTE_CANCEL_BET_STATE = {
    NONE: 0,
    CREATOR: 1,
    DISAGREE: 2,
    AGREE: 3,
})
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