<#
.SYNOPSIS
    Launch-RegiLattice.ps1 — Start RegiLattice in GUI, Menu, or CLI mode.
.DESCRIPTION
    Automatically discovers Python. Supports three named launch modes:
      -Mode gui    — Tkinter graphical interface (default when no arguments given)
      -Mode menu   — Interactive terminal category menu
      -Mode cli    — Pass arbitrary CLI arguments (use with -Arguments)
    Any extra flags are forwarded to the Python module.
.EXAMPLE
    .\Launch-RegiLattice.ps1                         # opens GUI
    .\Launch-RegiLattice.ps1 -Mode gui               # opens GUI explicitly
    .\Launch-RegiLattice.ps1 -Mode menu              # opens interactive menu
    .\Launch-RegiLattice.ps1 -Mode cli --list        # list all tweaks
    .\Launch-RegiLattice.ps1 -Mode cli apply perf-win32-priority-sep
    .\Launch-RegiLattice.ps1 -Mode cli --doctor      # system health check
    .\Launch-RegiLattice.ps1 -Mode cli --version     # print version
#>
[CmdletBinding()]
param(
    # Launch mode: gui (default), menu, or cli
    [ValidateSet('gui', 'menu', 'cli')]
    [string]$Mode = 'gui',

    # Extra arguments forwarded to the Python module (used with -Mode cli)
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$Arguments
)

$ErrorActionPreference = 'Stop'

# ── Locate Python ────────────────────────────────────────────────────────────
# Priority: PATH (non-WindowsApps) → $LOCALAPPDATA/Python/bin → common installs → py.exe
$PythonCandidates = @(
    "$env:LOCALAPPDATA\Python\bin\python.exe",
    "$env:LOCALAPPDATA\Python\pythoncore-3.14-64\python.exe",
    "$env:ProgramFiles\Python314\python.exe",
    "$env:ProgramFiles\Python313\python.exe",
    "$env:ProgramFiles\Python312\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python314\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python313\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python312\python.exe"
)

function Find-Python {
    # 1. Try python on PATH, skip WindowsApps stub
    $cmd = Get-Command python -ErrorAction SilentlyContinue
    if ($cmd -and $cmd.Source -notmatch 'WindowsApps') { return $cmd.Source }

    # 2. Try known install locations
    foreach ($p in $PythonCandidates) {
        if (Test-Path $p) { return $p }
    }

    # 3. Fall back to py.exe launcher (returns path, not the launcher itself)
    $py = Get-Command py -ErrorAction SilentlyContinue
    if ($py) { return $py.Source }

    return $null
}

$Python = Find-Python
if (-not $Python) {
    Write-Host 'ERROR: Python 3.10+ not found.' -ForegroundColor Red
    Write-Host '  Install from https://python.org or add Python to PATH.' -ForegroundColor DarkGray
    Read-Host 'Press Enter to exit'
    exit 1
}

# Ensure Python's bin/Scripts dirs are on PATH for this session (idempotent)
$PyBin = Split-Path $Python
$PyScripts = Join-Path (Split-Path $PyBin) 'Scripts'
foreach ($dir in @($PyBin, $PyScripts)) {
    if ($env:PATH -notmatch [regex]::Escape($dir)) {
        $env:PATH = "$dir;$env:PATH"
    }
}

# ── Build module arguments ────────────────────────────────────────────────────
$moduleArgs = switch ($Mode) {
    'gui'  { @('--gui') }
    'menu' { @('--menu') }
    'cli'  { @() }  # pass Arguments directly
}
$allArgs = @('-m', 'regilattice') + $moduleArgs + ($Arguments ?? @())

Write-Host "RegiLattice  [$Mode]  $Python" -ForegroundColor Cyan

# ── Launch ────────────────────────────────────────────────────────────────────
& $Python @allArgs
$rc = $LASTEXITCODE

if ($rc -ne 0) {
    Write-Host ''
    Write-Host "RegiLattice exited with code $rc. Check the log for details." -ForegroundColor Yellow
    if (-not $env:CI) { Read-Host 'Press Enter to exit' }
}
exit $rc

