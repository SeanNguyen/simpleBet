'use-strict';

var app = angular.module('app', ['ngRoute']);

app.config(['$routeProvider',
	function($routeProvider) {
		$routeProvider.
			when('/', {
				templateUrl: 'app/home/home.html'
			}).
			when('/create', {
				templateUrl: 'app/betCreator/betCreator.html',
				controller: 'betCreatorController'
			}).
			otherwise({
				redirectTo: '/'
			});
	}]);

app.controller('appController', function($rootScope, $scope, $location) {
	$scope.isNavbarVisible = function() {
		var currentPath = $location.url();
		return currentPath !== "/";
	}


	// FACEBOOK
	//FACEBOOK SDK
	var facebookInit = function() {
		window.fbAsyncInit = function() {
			FB.init({
				appId      : '1461214374172661',
				cookie     : true,
				xfbml      : true,
				version    : 'v2.3'
			});

			//do all the stuff need FB object here
			//check login
			FB.getLoginStatus(function(response) {
				$rootScope.facebook.loginStatus = response.status;
				$rootScope.$apply();
				if (response.status === 'connected') {
					console.log(response.authResponse.accessToken);
					$rootScope.facebook.accessToken = response.authResponse.accessToken;
				}


				FB.api('/me', function(response) {
			    	console.log(JSON.stringify(response));

					FB.api('/me/friendlists', function(response) {
						console.log('All friend lists');
						console.log(response);
	
						FB.api('/' + response.data[2].id, function(response) {
							console.log('A friend list');
					    	console.log(JSON.stringify(response));
						});

						FB.api('/' + response.data[2].id + '/members', function(response) {
							console.log('Members');
					    	console.log(response);
						});
					})

					FB.api('/me/invitable_friends?limit=1000', function(response) {
						console.log('invitable_friends');
						console.log(response);

						FB.api(response.paging.next, function(response) {
						console.log('invitable_friends');
						console.log(response);
						});
					});
				});
			});
		};

		(function(d, s, id){
			var js, fjs = d.getElementsByTagName(s)[0];
			if (d.getElementById(id)) {return;}
			js = d.createElement(s); js.id = id;
			js.src = "//connect.facebook.net/en_US/sdk.js";
			fjs.parentNode.insertBefore(js, fjs);
		}(document, 'script', 'facebook-jssdk'));
	}

	$rootScope.facebook = {loginStatus: ''};
	$rootScope.fbLogin = function() {
		FB.login(function(response){
		}, {scope: 'user_friends,read_custom_friendlists'});
	}
	//init facebook component
	facebookInit();
});