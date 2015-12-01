# Aurelia Web API Example

This is an example of an Aurelia web application backed by an ASP.NET 5 Web API web service.

The client and server build tasks are handled by gulp.

This example assumes you are familiar with node.js, npm, and gulp.

## Recommendation:  

If you're using npm 2.x, I recommend upgrading npm. If you don't want to upgrade your system npm, install a version manager.

    npm install nave -g
	nave use 4.2.2
	npm install npm -g
	npm --version # should be 3.x

If you don't install npm 3, projects with many npm dependencies will result in lots of nested folders and excessive disk IO.

# Install

Download and install ASP.NET 5 from [https://get.asp.net/](https://get.asp.net/). 

Install `1.0.0-rc1-final`:

    dnvm install 1.0.0-rc1-final
	dnvm use 1.0.0-rc1-final
	
Install dependencies:

    npm install
	npm install gulp jspm -g
	jspm install

# Structure

	.
	├── build
	│   └── tasks
	├── node_modules
	├── src
	│   ├── Controllers
	│   ├── Migrations
	│   ├── Models
	│   ├── Properties
	│   ├── client
	│   │   ├── app
	│   │   └── specs
	│   └── wwwroot
	└── test
		└── AureliaWebApiTests

* build
> Gulp build settings
* build/tasks
> Gulp build tasks, relies on settings in parent directory
* src
> Location of the client and server source code
* src/{Controllers,Migrations,Models,Properties,wwwroot}
> Folders related to Web API
* src/client
> Base folder for client-related source and tests
* src/client/app
> The client source: Aurelia written in ES6
* src/client/specs
> Unit and end-to-end tests for Aurelia
* test
> Aggregate folder for all server-side tests

# Tasks

	$ gulp help
	[22:52:08] Using gulpfile /Volumes/Extra/projects/AureliaWebApi/gulpfile.js
	[22:52:08] Starting 'help'...
	
	Main Tasks
	------------------------------
		build
		changelog
		clean
		default
		help
		lint
		release
		serve
		watch
	
	Sub Tasks
	------------------------------
		build-css
		build-deps
		build-html
		build-images
		build-sass
		build-source
		bump-version
		dotnet-build
		dotnet-restore
		dotnet-run
		dotnet-test
		prepare-release
	
	[22:52:08] Finished 'help' after 1.25 ms

Main build tasks should be pretty self-explanatory. 

Run `gulp` and everything gets built, tested, and staged for testing in browser sync.

By default, `gulp dotnet-run` will run as a Production environment. You can run in Development by passing the environment variable:

    ASPNET_ENV=Development gulp dotnet-run

The development environment will use an in-memory database while the production environment persists to a local SQLite database file.

# TODO

* Rethink `jspm_packages` to avoid copying from client/app to wwwroot during build.
* Iterate all tests in `test` directory and execute dnx test.
* Setup karma and end-to-end testing
* Work up a client application in Aurelia
* Upgrade everything to include dotnet50
