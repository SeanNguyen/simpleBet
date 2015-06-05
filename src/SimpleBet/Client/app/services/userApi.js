'use-strict';
var app = angular.module('app');

app.factory('userApi', ['$http', function($http) {
	var get = function(id, callback) {
		// $http.get('/api/bet/' + id).
		// 	success(function(data, status, headers, config) {
		// 		// this callback will be called asynchronously
		// 		// when the response is available
		// 		return data;
		// 	}).
		// 	error(function(data, status, headers, config) {
		// 		// called asynchronously if an error occurs
		// 		// or server returns response with an error status.
		// 		return null;
		// 	});
	    $http.get('/api/user/' + id).
	    success(function (data, status, headers, config) {
	        // this callback will be called asynchronously
	        // when the response is available
	        callback(data);
	    }).
	    error(function (data, status, headers, config) {
	        // called asynchronously if an error occurs
	        // or server returns response with an error status.
	        //console.log(data);
	    });
	}

	var getAll = function() {

	}

	var add = function (model) {

	}

	var update = function (model) {

	}

	var remove = function (model) {

	}

	return {
		get: get,
		getAll: getAll,
		add: add,
		update: update,
		remove: remove
	};
}]);