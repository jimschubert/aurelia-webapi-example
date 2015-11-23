'use strict';

import gulp from 'gulp';
import Seq from 'run-sequence';

gulp.task('default', (callback) => {
	return Seq(
		'clean',
		['build', 'dotnet-restore'],
		'dotnet-build',
		// 'dotnet-test',
		['dotnet-run', 'watch'],
		callback
	);
});
