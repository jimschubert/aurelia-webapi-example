'use strict';
import gulp from 'gulp';
import paths from '../paths';

// outputs changes to files to the console
function reportChange(event){
  console.log(`File ${event.path} was ${event.type}, running tasks...`);
}

// this task wil watch for changes and call the
// reportChange method.
gulp.task('watch', function() {
  gulp.watch(paths.source, ['build-source']).on('change', reportChange);
  gulp.watch(paths.html, ['build-html']).on('change', reportChange);
  gulp.watch(paths.css, ['build-css']).on('change', reportChange);
  gulp.watch(paths.styles, ['build-sass']).on('change', reportChange);
  gulp.watch(paths.images, ['build-images']).on('change', reportChange);
  gulp.watch(paths.jspmLocation, ['build-deps']).on('change', reportChange);
});