/* global __dirname */
'use strict';

import gulp from 'gulp';
import gutil from 'gulp-util';
import { spawn } from 'child_process';
import args from '../args';
import paths from '../paths';

function runner(cmd, args, wd = __dirname){
	return function(callback) {
		let command = spawn(cmd, args, { cwd: wd, env: process.env, detached: false });
		command.stdout.pipe(process.stdout);
		command.stderr.pipe(process.stderr);
		command.on('close', function (code) {
			if (code !== 0) {
				throw new gutil.PluginError('dotnet', `Exited with code ${code}: ${cmd} ${args}.`);
			}
			callback();
		});
	}
}

// kinda ugly hack to account for Kestrel not supporting pipes
function runnerWeb(cmd, args, wd = __dirname){
	return function(callback) {
		let command = spawn(cmd, args, { cwd: wd, env: process.env, detached: false });
		var called = false;
		command.stdout.on('data', function(data){
			process.stdout.write(data);
			
			if(!called && data.toString().indexOf('Now listening') !== -1) {
				called = true;
				callback();
			}
		});
		command.stderr.pipe(process.stderr);
		command.on('close', function (code) {
			if (code !== 0) {
				throw new gutil.PluginError('dotnet', `Exited with code ${code}: ${cmd} ${args}.`);
			}
		});
	}
}

gulp.task('dotnet-build', runner('dnu', ['build'], paths.srcDir));
gulp.task('dotnet-restore', runner('dnu', ['restore'], paths.srcDir));

// Run gulp with, e.g. --port=5000 to change server port. BrowserSync task picks up this change.
gulp.task('dotnet-run', runnerWeb('dnx', ['web', '--server.urls=http://localhost:' + (args.port || '5007')], paths.srcDir));

// TODO: Walk test directory and execute all tests.
gulp.task('dotnet-test', runner('dnx', ['test'], `${paths.testDir}/AureliaWebApiTests`));
