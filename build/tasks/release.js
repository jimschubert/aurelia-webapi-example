'use strict';

import gulp from 'gulp';
import Seq from 'run-sequence';

gulp.task('release', (callback) => {
  return Seq(
    'prepare-release',
    callback
  );
});
