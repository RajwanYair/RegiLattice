# Run-MutationTests.ps1
# Runs Stryker.NET mutation testing against RegiLattice.Core (T6.6).
# Usage:  . .\Run-MutationTests.ps1
# Output: .tmp/stryker-output/mutation-report.html

param(
    [switch]$Install
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Repo root is the parent of the scripts/ directory.
$repoRoot = Split-Path $PSScriptRoot -Parent
$coreDir  = Join-Path $repoRoot 'src' 'RegiLattice.Core'

Push-Location $repoRoot
try {
    if ($Install) {
        Write-Host "[stryker] Installing dotnet-stryker tool..." -ForegroundColor Cyan
        dotnet tool install --global dotnet-stryker
    }

    Write-Host "[stryker] Restoring local tools (includes dotnet-stryker)..." -ForegroundColor Cyan
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw "dotnet tool restore failed" }

    Write-Host "[stryker] Building Core library..." -ForegroundColor Cyan
    dotnet build src/RegiLattice.Core/RegiLattice.Core.csproj -c Debug -m:1 --verbosity minimal
    if ($LASTEXITCODE -ne 0) { throw "Build failed" }
} finally {
    Pop-Location
}

# Run Stryker from src/RegiLattice.Core/ so it does NOT auto-detect RegiLattice.sln
# and enter solution-scan mode (which fails on GUI.Tests).
Push-Location $coreDir
try {
    Write-Host "[stryker] Running mutation tests from: $coreDir" -ForegroundColor Cyan
    Write-Host "[stryker] Target >= 60% kill score..." -ForegroundColor Cyan
    dotnet stryker --config-file ../../stryker-config.json
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[stryker] FAILED — mutation score below break threshold (55%)." -ForegroundColor Red
        exit 1
    }

    Write-Host "[stryker] Done. Report: StrykerOutput/" -ForegroundColor Green
} finally {
    Pop-Location
}
