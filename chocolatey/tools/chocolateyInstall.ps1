# Chocolatey install script for RegiLattice
# See https://docs.chocolatey.org/en-us/create/functions for helper reference

$ErrorActionPreference = 'Stop'

# These values are replaced by the release workflow during `choco pack`:
#   - $url64    → download URL from the GitHub Release
#   - $checksum → SHA-256 of the .zip file
$packageName   = 'regilattice'
$toolsDir      = Split-Path -Parent $MyInvocation.MyCommand.Definition

$url64     = 'RELEASE_URL_PLACEHOLDER'   # replaced by release.yml
$checksum  = 'RELEASE_SHA256_PLACEHOLDER' # replaced by release.yml

$packageArgs = @{
    packageName   = $packageName
    unzipLocation = $toolsDir
    url64bit      = $url64
    checksum64    = $checksum
    checksumType64 = 'sha256'
    # Not using url32bit — RegiLattice is x64-only (.NET 10.0-windows x64).
}

Install-ChocolateyZipPackage @packageArgs

# Create an application shim so `regilattice` (CLI) resolves from any prompt.
# The GUI shim is created automatically by Chocolatey for .exe files in tools/.
$installDir = $toolsDir

$guiExe = Join-Path $installDir 'RegiLattice.GUI.exe'
$cliExe = Join-Path $installDir 'RegiLattice.exe'

if (-not (Test-Path $cliExe)) {
    $cliExe = Join-Path $installDir 'RegiLattice.CLI.exe'
}

foreach ($exe in @($guiExe, $cliExe)) {
    if (Test-Path $exe) {
        # Ensure the exe is not blocked by the OS SmartScreen / Zone.Identifier
        Unblock-File $exe -ErrorAction SilentlyContinue
    }
}

Write-Host ''
Write-Host 'RegiLattice has been installed successfully.'
Write-Host "  GUI : $guiExe"
Write-Host "  CLI : $cliExe"
Write-Host ''
Write-Host 'Quick-start:'
Write-Host '  regilattice --list           # list all tweaks'
Write-Host '  regilattice --dry-run apply perf-disable-superfetch'
Write-Host '  regilattice --gui            # launch graphical interface'
