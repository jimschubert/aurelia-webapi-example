import paths from './paths';
import babelOptions from './babel-options';

let options = {

  // base path that will be used to resolve all patterns (eg. files, exclude)
  basePath: paths.clientRoot + '/',

  // frameworks to use
  // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
  frameworks: ['jspm', 'jasmine'],

  jspm: {
    config: 'app/config.js',
    packages: 'app/jspm_packages/',
    loadFiles: [
      'specs/**/*.js'
    ],
    serveFiles: [
      'app/**/*.js'
    ],
    paths: {
      "specs/*": "specs/*",
      "github:*": "app/jspm_packages/github/*",
      "npm:*": "app/jspm_packages/npm/*",
      "*": "app/src/*"
    }
  },

  proxies: {
    '/base/src/specs': '/base/specs'
  },

  // list of files / patterns to load in the browser
  files: [
  ],

  // list of files to exclude
  exclude: [
  ],

  // preprocess matching files before serving them to the browser
  // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
  preprocessors: {
    'app/src/**/*.js': ['babel']
    , 'specs/unit/**/*.js': ['babel']
  },
  babelPreprocessor: {
    options: babelOptions
  },

  // test results reporter to use
  // possible values: 'dots', 'progress'
  // available reporters: https://npmjs.org/browse/keyword/karma-reporter
  reporters: ['progress'],

  // web server port
  port: 9876,

  // enable / disable colors in the output (reporters and logs)
  colors: true,

  // level of logging
  // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
  logLevel: 'INFO',

  // enable / disable watching file and executing tests whenever any file changes
  autoWatch: true,

  // start these browsers
  // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
  browsers: ['Chrome'],

  // Continuous Integration mode
  // if true, Karma captures browsers, runs the tests and exits
  singleRun: false
};

module.exports = exports = function (config) {
  config.set(options);
};

export const config = options;
