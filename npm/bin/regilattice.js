#!/usr/bin/env node
// Entry point — delegates to the native Windows CLI binary.
'use strict';

if (process.platform !== 'win32') {
  process.stderr.write('RegiLattice is Windows-only.\n');
  process.exit(1);
}

const { spawnSync } = require('child_process');
const path = require('path');
const exe = path.join(__dirname, 'regilattice.exe');

if (!require('fs').existsSync(exe)) {
  process.stderr.write(
    '[regilattice] Binary not found. Re-run: npm install @rajwanyair/regilattice\n'
  );
  process.exit(1);
}

const result = spawnSync(exe, process.argv.slice(2), { stdio: 'inherit', windowsHide: false });
process.exit(result.status ?? 1);
