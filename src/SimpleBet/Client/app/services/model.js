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
    //init state
    NONE: 0,
    //wait for everyone to accept or decline the bet and pick their option
    PENDING: 1,
    //Answer phase
    ANSWERABLE: 2,
    //Verify duration
    VERIFYING: 3,
    //after everyone key in the correct option, the bet now officially closed
    FINALLIZED: 4
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
    VOTED: 4,
    AGREE: 5
});
app.factory('BetUser', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/betUser/:betId/:userId', { betId: '@betId', userId: '@userId' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);

//WINNING ITEM
app.value('WINNING_ITEM_TYPE', WINNING_ITEM_TYPE = {
    VOUCHER: 0,
    BEAUTY: 1,
    FNB: 2,
    WINE: 3,
    BEER: 4
});
app.value('WINNING_ITEM_CATEGORY', WINNING_ITEM_CATEGORY = {
    MONETARY: 0,
    NONMONETARY: 1
});
app.factory('WinningItem', ['$resource', '$q', function ($resource, $q) {
    return $resource('/api/winningItem/:id', { id: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
}]);