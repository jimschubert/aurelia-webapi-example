'use strict';
import gulp from 'gulp';
import paths from '../paths';
import bs from 'browser-sync';

// outputs changes to files to the console
function reportChange(event){
  console.log(`File ${event.path} was ${event.type}, running tasks...`);
}

// this task wil watch for changes and call the
// reportChange method.
gulp.task('watch', ['serve'], function() {
  gulp.watch(paths.source, ['build-source', bs.reload]).on('change', reportChange);
  gulp.watch(paths.html, ['build-html', bs.reload]).on('change', reportChange);
  gulp.watch(paths.css, ['build-css', bs.reload]).on('change', reportChange);
  gulp.watch(paths.styles, ['build-sass', bs.reload]).on('change', reportChange);
  gulp.watch(paths.images, ['build-images', bs.reload]).on('change', reportChange);
  gulp.watch(paths.jspmLocation, ['build-deps', bs.reload]).on('change', reportChange);
});