'use strict';
import { argv as args } from 'yargs';

let bumps = 'major|minor|patch|prerelease'.split('|');
let bump = (args.bump||'patch').toLowerCase();

if(bumps.indexOf(bump) === -1) {
  throw new Error(`Unrecognized bump "${bump}".`);
}

export default args;
