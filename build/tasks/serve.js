import gulp from 'gulp';
import browserSync from 'browser-sync';
import Seq from 'run-sequence';
import args from '../args';

gulp.task('serve', ['dotnet-run'], (cb) => {
	browserSync.init({
		online: false,
		open: true,
		// Using a localhost address with a port
		proxy: 'localhost:' + (args.port || '5007')
	}, cb)
});
