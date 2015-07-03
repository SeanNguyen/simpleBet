/// <binding />
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
		},
				
		//inject bower components automatically
		wiredep: {
				app: {
						src: ['<%= pathConfig.app %>/index.html'],
						ignorePath: /\.\.\//
				}
		},

		// Watches files for changes and runs tasks based on the changed files
		watch: {
				bower: {
						files: ['bower.json'],
						tasks: ['all']
				},
				code: {
						files: ['<%= pathConfig.app %>/*', '<%= pathConfig.app %>/app/**'],
						tasks: ['code']
				},
				assets: {
						files: ['<%= pathConfig.app %>/assets/**'],
						tasks: ['all']
				}
		},

		injector: {
			options: {
				
			},
			scripts: {
				options: {
					transform: function(filePath) {
						filePath = filePath.replace('/Client/', '');
						return '<script src="' + filePath + '"></script>';
					},
				},
				files: {
					'<%= pathConfig.app %>/index.html': ['<%= pathConfig.app %>/**/*.js']
				}
			},
			// Inject component css into index.html
			css: {
				options: {
					transform: function(filePath) {
						filePath = filePath.replace('/Client/', '');
						return '<link rel="stylesheet" href="' + filePath + '">';
					}
				},
				files: {
					'<%= pathConfig.app %>/index.html': ['<%= pathConfig.app %>/**/*.css']
				}
			}
		},

		connect: {
			server: {
				options: {
					port: 9000,
					base: '<%= pathConfig.webRoot %>'
				}
			}
		},

		clean: {
			all: ["<%= pathConfig.webRoot %>/*/**", 
				"<%= pathConfig.webRoot %>/*",
				"!<%= pathConfig.webRoot %>/web.config",
			    "!<%= pathConfig.webRoot %>/bin/**", ],
			code: ["<%= pathConfig.webRoot %>/**/*.js", 
				"<%= pathConfig.webRoot %>/**/*.css",
			    "!<%= pathConfig.webRoot %>/bower_components/**"]
		},

		postcss: {
		    options: {
		        processors: [
                  require('autoprefixer-core')({ browsers: 'last 2 versions' }), // add vendor prefixes
                  require('cssnano')() // minify the result
		        ]
		    },
		    dist: {
		        src: ['<%= pathConfig.webRoot%>/app/**/*.css', '<%= pathConfig.webRoot%>/app.css']
		    }
		}
	});

		// define tasks
	grunt.registerTask('all', ['wiredep', 'injector', 'clean:all', 'copy:all', 'postcss']);
	grunt.registerTask('code', ['wiredep', 'injector', 'clean:code', 'copy:code', 'postcss']);
	grunt.registerTask('vs', ['all', 'watch']);
	grunt.registerTask('default', ['all', 'connect', 'watch']);
};