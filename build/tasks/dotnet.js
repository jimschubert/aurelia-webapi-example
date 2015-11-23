'use strict';

import gulp from 'gulp';
import gutil from 'gulp-util';
import { spawn } from 'child_process';

function runner(cmd, args){
	return function(callback) {
		let command = spawn(cmd, args);
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

gulp.task('dotnet-build', runner('dnu', ['build']));
gulp.task('dotnet-restore', runner('dnu', ['restore']));
gulp.task('dotnet-run', runner('dnx', ['web']));
gulp.task('dotnet-test', runner('dnx', ['test']));
