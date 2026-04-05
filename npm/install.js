#!/usr/bin/env node
// postinstall: downloads the RegiLattice CLI binary from GitHub Releases.
'use strict';

const https = require('https');
const fs = require('fs');
const path = require('path');

if (process.platform !== 'win32') {
  console.log('[regilattice] Skipping binary download — RegiLattice is Windows-only.');
  process.exit(0);
}

const pkg = require('./package.json');
const version = pkg.version;
const binDir = path.join(__dirname, 'bin');
const exePath = path.join(binDir, 'regilattice.exe');
const url = `https://github.com/RajwanYair/RegiLattice/releases/download/v${version}/RegiLatticeCLI-v${version}-win-x64.exe`;

// Already installed — skip re-download.
if (fs.existsSync(exePath)) {
  process.exit(0);
}

fs.mkdirSync(binDir, { recursive: true });

function download(url, dest, depth) {
  if ((depth || 0) > 5) { throw new Error('Too many redirects'); }
  return new Promise((resolve, reject) => {
    const file = fs.createWriteStream(dest);
    https.get(url, (res) => {
      if (res.statusCode >= 300 && res.statusCode < 400 && res.headers.location) {
        file.close();
        fs.unlink(dest, () => {});
        download(res.headers.location, dest, (depth || 0) + 1).then(resolve).catch(reject);
        return;
      }
      if (res.statusCode !== 200) {
        file.close();
        fs.unlink(dest, () => {});
        reject(new Error(`HTTP ${res.statusCode} downloading ${url}`));
        return;
      }
      res.pipe(file);
      file.on('finish', () => file.close(resolve));
    }).on('error', (err) => {
      file.close();
      fs.unlink(dest, () => {});
      reject(err);
    });
  });
}

console.log(`[regilattice] Downloading CLI v${version} for Windows x64...`);
download(url, exePath)
  .then(() => console.log('[regilattice] Installation complete.'))
  .catch((err) => {
    console.error(`[regilattice] Download failed: ${err.message}`);
    console.error(`[regilattice] Download manually from: ${url}`);
    process.exit(1);
  });
