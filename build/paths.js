/* global __dirname */
'use strict';
import path from 'path';

let source = path.resolve(path.normalize(__dirname + '/../'));
let client =  `${source}/client`;
let clientSource = `${client}/app`;
let outputRoot =  `${source}/wwwroot`;
let specs = `${client}/specs`;

const subdir = '{,/**}';
const allFiles = subdir + '/*';

let paths = {
  root: source,
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
