import gulp from 'gulp';
import {Server} from 'karma';
import paths from '../paths';
import path from 'path';
import babelOptions from '../babel-options';
import assign from 'object.assign';
import {config} from '../karma.conf.js'

// These tasks taken entirely from https://github.com/aurelia/skeleton-navigation/blob/master/build/tasks/test.js

let karmaPath = path.resolve(path.normalize(__dirname + '/../karma.conf.js'));

/**
 * Run test once and exit
 */
gulp.task('client-test', function (done) {
  new Server({
    configFile: karmaPath,
    singleRun: true
  }, done).start();
});

/**
 * Run test once and exit
 */
gulp.task('client-test:watch', function (done) {
  new Server({
    configFile: karmaPath,
    singleRun: false
  }, done).start();
});
