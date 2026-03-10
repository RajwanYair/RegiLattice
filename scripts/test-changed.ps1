#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Run pytest only on tests that correspond to files changed since HEAD (or a base ref).
.DESCRIPTION
    Computes the diff between the working tree and HEAD (default), maps each changed
    source file to its test file by naming convention, collects all changed test files,
    then invokes pytest on just that subset — significantly faster than the full suite.

    Mapping convention:
      regilattice/<module>.py          -> tests/test_<module>.py
      regilattice/tweaks/<module>.py   -> tests/test_tweaks_smoke.py  (auto-discovered)
      tests/test_*.py                  -> included as-is

    If no test files are found (e.g., only docs changed), exits cleanly without running.
    Falls back to the full suite when called with -Full.

.PARAMETER Base
    Git ref to diff against. Default: HEAD (unstaged + staged changes).
.PARAMETER Full
    Switch: ignore diff and run the full suite (same as 'pytest tests/').
.PARAMETER Verbosity
    Pytest -v level. 0 = -q (quiet), 1 = -v (default), 2 = -vv.

.EXAMPLE
    pwsh scripts/test-changed.ps1                 # test changes vs HEAD
    pwsh scripts/test-changed.ps1 -Base main      # test changes vs main branch
    pwsh scripts/test-changed.ps1 -Full           # run everything
#>
param(
    [string]$Base = "",
    [switch]$Full = $false,
    [int]$Verbosity = 1
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Root = $PSScriptRoot | Split-Path -Parent
$Tests = Join-Path $Root "tests"

# ── Map a source file path to its test file (if one exists) ──────────────────
function Get-TestFile {
    param([string]$Src)
    $rel = $Src -replace [regex]::Escape($Root + [IO.Path]::DirectorySeparatorChar), ""
    $candidate = $null

    if ($rel -match '^tests[/\\]test_.*\.py$') {
        # Already a test file
        $candidate = Join-Path $Root $rel
    } elseif ($rel -match '^regilattice[/\\]tweaks[/\\](.+)\.py$') {
        # Any tweak module change → smoke tests
        $candidate = Join-Path $Tests "test_tweaks_smoke.py"
    } elseif ($rel -match '^regilattice[/\\](.+)\.py$') {
        $module = $Matches[1]
        $candidate = Join-Path $Tests "test_${module}.py"
    }

    if ($candidate -and (Test-Path $candidate)) {
        return $candidate
    }
    return $null
}

# ── Full suite shortcut ───────────────────────────────────────────────────────
if ($Full) {
    Write-Host "[test-changed] -Full flag: running complete test suite." -ForegroundColor Cyan
    python -m pytest $Tests --tb=short --strict-markers -q
    exit $LASTEXITCODE
}

# ── Collect changed files ─────────────────────────────────────────────────────
Push-Location $Root
try {
    if ($Base) {
        $changed = git diff --name-only "$Base" HEAD 2>&1
    } else {
        # Unstaged + staged changes vs HEAD
        $staged = git diff --cached --name-only HEAD 2>&1
        $unstaged = git diff        --name-only      2>&1
        $changed = ($staged + $unstaged) | Sort-Object -Unique
    }
} finally {
    Pop-Location
}

if (-not $changed) {
    Write-Host "[test-changed] No changed files detected. Nothing to test." -ForegroundColor Yellow
    exit 0
}

# ── Map to test files ─────────────────────────────────────────────────────────
$testFiles = @()
foreach ($f in $changed) {
    $abs = Join-Path $Root $f
    $mapped = Get-TestFile -Src $abs
    if ($mapped) {
        $testFiles += $mapped
    }
}

# Always include test_tweaks_init.py when __init__.py of tweaks changes
if ($changed | Where-Object { $_ -match 'tweaks[/\\]__init__\.py' }) {
    $ti = Join-Path $Tests "test_tweaks_init.py"
    if (Test-Path $ti) { $testFiles += $ti }
}

$testFiles = $testFiles | Sort-Object -Unique

if ($testFiles.Count -eq 0) {
    Write-Host "[test-changed] No test files map to the changed sources. Skipping." -ForegroundColor Yellow
    exit 0
}

# ── Run pytest ────────────────────────────────────────────────────────────────
$verbFlag = switch ($Verbosity) {
    0 { "-q" }
    2 { "-vv" }
    default { "-v" }
}

Write-Host "[test-changed] Running $($testFiles.Count) test file(s):" -ForegroundColor Cyan
$testFiles | ForEach-Object { Write-Host "  - $(Split-Path $_ -Leaf)" -ForegroundColor DarkCyan }

python -m pytest @testFiles --tb=short --strict-markers $verbFlag
exit $LASTEXITCODE
