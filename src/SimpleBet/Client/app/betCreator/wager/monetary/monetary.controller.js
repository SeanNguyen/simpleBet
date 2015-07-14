(function () {
    'use-strict';

    var app = angular.module('app');

    app.controller('monetaryController', ['$rootScope', 
                                            '$scope', 
                                            '$window', 
                                            '$state', 
                                            'WinningItem', 
                                            'WINNING_ITEM_TYPE', 
                                            'WINNING_ITEM_CATEGORY',
                                            monetaryController]);

    function monetaryController($rootScope, $scope, $window, $state, WinningItem, WINNING_ITEM_TYPE, WINNING_ITEM_CATEGORY) {
        $scope.currentTab = 'popular';
        $scope.popularItems = WinningItem.query({ type: WINNING_ITEM_TYPE.MONETARY });
        $scope.currentCategoryItems = [];

        $scope.isSelectingCategory = true;
        $scope.categories = [{ name: 'VOUCHER', typeId: WINNING_ITEM_CATEGORY.VOUCHER },
                            { name: 'BEAUTY', typeId: WINNING_ITEM_CATEGORY.BEAUTY },
                            { name: 'FNB', typeId: WINNING_ITEM_CATEGORY.FNB },
                            { name: 'WINE', typeId: WINNING_ITEM_CATEGORY.WINE },
                            { name: 'BEER', typeId: WINNING_ITEM_CATEGORY.BEER }];

        //fucntions
        $scope.setTab = setTab;
        $scope.selectDare = selectDare;

        //events
        $scope.onCategoryClicked = onCategoryClicked;

        //
        function setTab(tab) {
            $scope.currentTab = tab;
            $scope.isSelectingCategory = true;
        }

        function onCategoryClicked(category) {
            $scope.currentCategoryItems = WinningItem.query({ type: WINNING_ITEM_TYPE.MONETARY, category: category.typeId });
            $scope.isSelectingCategory = false;
        }

        function selectDare(dare) {
            $scope.selectedDare = dare;
            $scope.betModel.winningItemId = dare.id;
            $scope.$parent.$parent.setTab(3);
        }
    };
})();