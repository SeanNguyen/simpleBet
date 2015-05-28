'use-strict';

var app = angular.module('app');

app.factory('facebookService', function() {
	var loginStatus;
	//FACEBOOK SDK
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
			loginStatus = response;
		});
	};

	(function(d, s, id){
		var js, fjs = d.getElementsByTagName(s)[0];
		if (d.getElementById(id)) {return;}
		js = d.createElement(s); js.id = id;
		js.src = "//connect.facebook.net/en_US/sdk.js";
		fjs.parentNode.insertBefore(js, fjs);
	}(document, 'script', 'facebook-jssdk'));

	var service = {loginStatus: loginStatus};

	return service;
});