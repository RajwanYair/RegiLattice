<#
.SYNOPSIS
    Launch-RegiLattice.ps1 - Starts RegiLattice via Python.
.DESCRIPTION
    Prefers Python on PATH; falls back to common install locations.
    Pass any CLI arguments through: -gui, --list, apply, remove, etc.
.EXAMPLE
    .\Launch-RegiLattice.ps1 --gui
    .\Launch-RegiLattice.ps1 --list
    .\Launch-RegiLattice.ps1 apply perf-startup-delay
#>
param(
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$Arguments
)

$ErrorActionPreference = 'Stop'

# ── Locate Python ────────────────────────────────────────────────────────────
$PythonPaths = @(
    "$env:LOCALAPPDATA\Python\bin\python.exe",
    "$env:ProgramFiles\Python314\python.exe",
    "$env:ProgramFiles\Python312\python.exe",
    "$env:ProgramFiles\Python313\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python314\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python312\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python313\python.exe"
)

$Python = $null

# Try PATH first
$pythonCmd = Get-Command python -ErrorAction SilentlyContinue
if ($pythonCmd -and $pythonCmd.Source -notmatch 'WindowsApps') {
    $Python = $pythonCmd.Source
}

# Fall back to known install locations
if (-not $Python) {
    foreach ($p in $PythonPaths) {
        if (Test-Path $p) {
            $Python = $p
            break
        }
    }
}

if (-not $Python) {
    Write-Host 'ERROR: Python not found. Install Python 3.10+ from https://python.org' -ForegroundColor Red
    Read-Host 'Press Enter to exit'
    exit 1
}

Write-Host "Starting RegiLattice with $Python ..." -ForegroundColor Cyan

# ── Launch ───────────────────────────────────────────────────────────────────
$allArgs = @('-m', 'regilattice') + ($Arguments ?? @())
& $Python @allArgs

if ($LASTEXITCODE -ne 0) {
    Write-Host ''
    Write-Host 'RegiLattice exited with errors. Check the log for details.' -ForegroundColor Yellow
    Read-Host 'Press Enter to exit'
}
