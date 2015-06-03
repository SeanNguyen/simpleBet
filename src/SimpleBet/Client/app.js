'use-strict';

var app = angular.module('app', ['ui.router']);

app.config(function($stateProvider, $urlRouterProvider) {
	$urlRouterProvider.otherwise("/");
	});

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
				appId      : '1462927764001322',
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

				FB.api('/me/taggable_friends?limit=2000', function(response) {
					$rootScope.facebook.friends = [];
					var tagId;
					for(var i = 0; i < response.data.length; i++) {
						var friendData = response.data[i];
						var friend = {id: friendData.id, name: friendData.name, avata: friendData.picture.data.url};
						$rootScope.facebook.friends.push(friend);

						if(friendData.name === "Charlie Amicgbgdefgi Lauson") {
							console.log(friendData.name);
							tagId = friendData.id;
						}
					}
					$rootScope.$apply();

					FB.api(
					"/me/feed",
					"POST",
					{
						message: "This is a test message",
						place: "1424132167909654",
						tags: tagId,
						privacy: {
							value: "SELF"
						},
						// actions: [{
						// 	name: "Accept Bet",
						// 	link: "http://arcadier.com/"
						// }],
						link: "http://arcadier.com/"
					},
					function (response) {
						console.log(response);
						if (response && !response.error) {
						/* handle the result */
						}
					}
				);
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
		}, {scope: 'user_friends,read_custom_friendlists,publish_actions'});
	}
	//init facebook component
	facebookInit();
});