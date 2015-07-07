'use-strict';

var app = angular.module('app',
    ['ui.router',
    'ngResource',
    'angular-loading-bar',
    'ngAnimate',
    'ngMaterial',
    'ngDialog']);

app.config(function ($stateProvider, $urlRouterProvider, $sceDelegateProvider, cfpLoadingBarProvider, ngDialogProvider) {
    $urlRouterProvider.otherwise("/home");

    //TODO: restrict this white list down to only the site we need
    $sceDelegateProvider.resourceUrlWhitelist(['**']);

    //config loading bar
    cfpLoadingBarProvider.includeSpinner = false;

    //set default config fot dialogbox
    ngDialogProvider.setDefaults({
        className: 'ngdialog-theme-default',
        showClose: false
    });

    //load fast click
    $(function () {
        FastClick.attach(document.body);
    });
});

app.run(function ($rootScope, $templateCache) {
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        if (typeof (fromState) !== 'undefined') {
            $templateCache.remove(fromState.templateUrl);
        }
    });
});