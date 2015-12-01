/* global __dirname */
'use strict';
import path from 'path';

let root = path.resolve(path.normalize(__dirname + '/../'));
let source = path.resolve(path.normalize(__dirname + '/../src/AureliaWebApi'));
let client =  `${source}/client`;
let clientSource = `${client}/app`;
let outputRoot =  `${source}/wwwroot`;
let specs = `${client}/specs`;

const subdir = '{,/**}';
const allFiles = subdir + '/*';

let paths = {
  root: root,
  srcDir: source,
  testDir: `${root}/test`,
  html:  `${clientSource}/**/*.html`,
  css: `${clientSource}/styles/**/*.css`,
  styles: `${clientSource}/styles/**/*.scss`,
  images: `${clientSource}/images/**/*`,
  output: outputRoot,
  clientRoot: client,
  clientSource: clientSource,
  jspmLocation:  `${clientSource}/jspm_packages`,
  allJavaScript: [clientSource],
  source:  `${clientSource}/**/*.js`,
  specsLocation: specs,
  unitTestLocation: `${specs}/unit`,
  e2eTestLocation: `${specs}/e2e`,
  globs: {
    subdir: subdir,
    recursive: (dir, ext = '*') => {
      return `${dir}/${subdir}/${ext}`;
    },
    allFiles: allFiles
  }
};

export default paths;
