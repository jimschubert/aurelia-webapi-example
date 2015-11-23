'use strict';
import gulp from 'gulp';
import paths from '../paths';
import del from 'del';
import vinylPaths from 'vinyl-paths';

// deletes all files in the output path
gulp.task('clean', () => {
  let output = `${paths.output}/`
  return gulp.src([
    `${output}*`, 
    `!${output}.gitkeep`
    ])
    .pipe(vinylPaths(del));
});
