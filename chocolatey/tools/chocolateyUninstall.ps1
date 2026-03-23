# Chocolatey uninstall script for RegiLattice
# RegiLattice is a zip-based package (no system installer) so Chocolatey's
# default shim removal is sufficient.  This script removes any leftover files
# and cleans up user-data if the user opts in via --params "/PurgeData".

$ErrorActionPreference = 'Stop'
$packageName = 'regilattice'
$toolsDir    = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Honour optional /PurgeData parameter
$pp = Get-PackageParameters
if ($pp['PurgeData']) {
    $dataDir = Join-Path $env:LOCALAPPDATA 'RegiLattice'
    if (Test-Path $dataDir) {
        Remove-Item $dataDir -Recurse -Force
        Write-Host "Removed user data directory: $dataDir"
    }
}

Write-Host "RegiLattice uninstalled. Shimmed executables removed by Chocolatey."
