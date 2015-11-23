'use strict';
import gulp from 'gulp';
import Seq from 'run-sequence';
import paths from '../paths';
import changelog from 'conventional-changelog';
import fs from 'fs';
import bump from 'gulp-bump';
import args from '../args';

let packageJson = `${paths.root}/package.json`;

// utilizes the bump plugin to bump the
// semver for the repo
gulp.task('bump-version', () => {
  return gulp.src([packageJson])
    .pipe(bump({type:args.bump })) //major|minor|patch|prerelease
    .pipe(gulp.dest(paths.root));
});

// generates the CHANGELOG.md file based on commit
// from git commit messages
gulp.task('changelog', (callback) => {
  let pkg = JSON.parse(fs.readFileSync(packageJson, 'utf-8'));
  let CHANGELOG_MD = `${paths.root}/CHANGELOG.md`;
  
  return changelog({
    repository: pkg.repository.url,
    version: pkg.version,
    file: CHANGELOG_MD
  }, (err, log) => {
    fs.writeFileSync(CHANGELOG_MD, log);
    callback(err);
  });
});

// calls the listed sequence of tasks in order
gulp.task('prepare-release', (callback) => {
  return Seq(
    'build',
    'bump-version',
    'changelog',
    callback
  );
});