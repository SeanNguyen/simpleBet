(function () {
    'use-strict';
    var app = angular.module('app');

    var TABS = {
        BASIC: 0,
        CREDENTIAL: 1,
        ADDRESS: 2
    }

    app.controller('profileController', function ($rootScope, $scope) {
        $scope.currentTab = TABS.BASIC;

        //functions
        $scope.nextTab = nextTab;


        //private
        function nextTab() {
            $scope.currentTab++;
        }

    });
})();