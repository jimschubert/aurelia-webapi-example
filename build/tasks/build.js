'use strict';

import gulp from 'gulp';
import Seq from 'run-sequence';
import changed from 'gulp-changed';
import plumber from 'gulp-plumber';
import to5 from 'gulp-babel';
import sourcemaps from 'gulp-sourcemaps';
import paths from '../paths';
import compilerOptions from '../babel-options';
import assign from 'object.assign';
import notify from 'gulp-notify';
import sass from 'gulp-sass';
import {default as $if} from 'gulp-if';
import cache from 'gulp-cache';
import imagemin from 'imagemin';

let exceptJspm = '!' + paths.globs.recursive(paths.jspmLocation);
let systemConfig = `${paths.clientSource}/config.js`;

// transpiles changed es6 files to SystemJS format
// the plumber() call prevents 'pipe breaking' caused
// by errors from other gulp plugins
// https://www.npmjs.com/package/gulp-plumber
gulp.task('build-source', () => {
  return gulp.src([ 
      paths.source,
      `!${systemConfig}`,
      exceptJspm
    ], {base: paths.clientSource})
    .pipe(plumber({errorHandler: notify.onError('Error: <%= error.message %>')}))
    .pipe(changed(paths.output, {extension: '.js'}))
    .pipe(sourcemaps.init({loadMaps: true}))
    .pipe(to5(assign({}, compilerOptions, {modules: 'system'})))
    .pipe(sourcemaps.write({includeContent: true}))
    .pipe(gulp.dest(paths.output));
});

// build deps copies jspm and other 'straight-copy' dependencies to output
// without running it through any preprocessors
gulp.task('build-deps', () => {
  return gulp.src([
      systemConfig,
      paths.globs.recursive(paths.jspmLocation),
      `${paths.clientSource}/web.config`,
      paths.globs.recursive(paths.clientSource, '*.{eot,svg,ttf,woff,woff2}')
    ], {base: paths.clientSource})
    .pipe(plumber({errorHandler: notify.onError('Error: <%= error.message %>')}))
    .pipe(gulp.dest(paths.output));
});

// copies changed html files to the output directory
gulp.task('build-html', () => {
  return gulp.src([ paths.html, exceptJspm ], {base: paths.clientSource})
    .pipe(changed(paths.output, {extension: '.html'}))
    .pipe(gulp.dest(paths.output));
});

// copies changed css files to the output directory
gulp.task('build-css', function() {
  return gulp.src(paths.css, {base: paths.clientSource})
    .pipe(changed(paths.output, {extension: '.css'}))
    .pipe(gulp.dest(paths.output));
});

// compiles sass files to the output directory
gulp.task('build-sass', function () {
  gulp.src(paths.styles, {base: paths.clientSource})
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest(paths.output));
});

// builds images, uses cached imagemin build results
gulp.task('build-images', () => {
  return gulp.src(paths.images, {base: paths.clientSource})
    .pipe($if($if.isFile, cache(imagemin({
      progressive: true,
      interlaced: true,
      // don't remove IDs from SVGs, they are often used
      // as hooks for embedding and styling
      svgoPlugins: [{cleanupIDs: false}]
    }))
    .on('error', function (err) {
      console.log(err);
      this.end();
    })))
    .pipe(gulp.dest(paths.output));
});

// this task runs the build-* tasks in parallel
// https://www.npmjs.com/package/gulp-run-sequence
gulp.task('build', (callback) => {
  return Seq(
    [
      'build-source', 
      'build-html',
      'build-css',
      'build-deps',
      'build-sass',
      'build-images'
    ],
    callback
  );
});
