'use-strict';

module.exports = function (grunt) {
	// Load grunt tasks automatically
	require('load-grunt-tasks')(grunt);

	// Time how long tasks take. Can help when optimizing build times
	require('time-grunt')(grunt);

	// configure plugins
	grunt.initConfig({
		pathConfig: {
			app: 'Client',
			webRoot: 'wwwroot',
			bower: 'bower_components'
		},

		copy: {
			code: {
				expand: true, cwd: '<%= pathConfig.app %>/', src: ['**/*.js', '**/*.html', '**/*.css'], dest: '<%= pathConfig.webRoot%>/'
			},
			all: {
				files: [
					//all js, html, css, and assets
					{expand: true, cwd: '<%= pathConfig.app %>/', src: ['**'], dest: '<%= pathConfig.webRoot%>/'},
					//all dependencies
					{expand: true, src: ['<%= pathConfig.bower %>/**'], dest: '<%= pathConfig.webRoot%>/'}
				]
			}
		}
	});

	// define tasks
	// grunt.registerTask('default', ['copy:all', 'copy:bower']);
	grunt.registerTask('all', ['copy:all']);
	grunt.registerTask('code', ['copy:code']);
};