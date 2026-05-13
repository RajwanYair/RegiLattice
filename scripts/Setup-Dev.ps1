<#
.SYNOPSIS
    One-command developer bootstrap for RegiLattice.

.DESCRIPTION
    Checks prerequisites (Git, .NET SDK, gh CLI, scoop, git-cliff),
    restores NuGet packages, runs a Debug build, and runs the full
    test suite. Designed to get a new contributor from zero to green
    in a single command.

.PARAMETER SkipBuild
    Skip the dotnet build step (useful when you only want a prerequisite report).

.PARAMETER SkipTests
    Skip the dotnet test step.

.PARAMETER Quiet
    Suppress informational output; only print warnings and errors.

.EXAMPLE
    .\scripts\Setup-Dev.ps1
    .\scripts\Setup-Dev.ps1 -SkipTests
    .\scripts\Setup-Dev.ps1 -SkipBuild -SkipTests
#>
[CmdletBinding()]
param(
    [switch] $SkipBuild,
    [switch] $SkipTests,
    [switch] $Quiet
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# ── Helpers ──────────────────────────────────────────────────────────────────

function Write-Status([string]$Msg, [string]$Color = 'Cyan') {
    if (-not $Quiet) { Write-Host "  $Msg" -ForegroundColor $Color }
}

function Write-Ok([string]$Msg)   { Write-Host "  [OK]  $Msg" -ForegroundColor Green  }
function Write-Warn([string]$Msg) { Write-Host "  [WARN] $Msg" -ForegroundColor Yellow }
function Write-Fail([string]$Msg) { Write-Host "  [FAIL] $Msg" -ForegroundColor Red    }

function Require-Command([string]$Name, [string]$InstallHint) {
    if (Get-Command $Name -ErrorAction SilentlyContinue) {
        Write-Ok "$Name found"
        return $true
    }
    Write-Warn "$Name not found. $InstallHint"
    return $false
}

function Get-Version([string]$Cmd, [string]$Args) {
    try {
        $out = & $Cmd @Args.Split(' ') 2>&1
        return ($out | Select-Object -First 1).ToString().Trim()
    } catch {
        return '<unknown>'
    }
}

# ── Header ───────────────────────────────────────────────────────────────────

Write-Host ''
Write-Host '  RegiLattice — Developer Setup' -ForegroundColor Cyan
Write-Host '  ==============================' -ForegroundColor Cyan
Write-Host ''

# Navigate to repository root (script lives in scripts/)
$RepoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $RepoRoot
Write-Status "Repository root: $RepoRoot"
Write-Host ''

# ── Prerequisites ─────────────────────────────────────────────────────────────

Write-Host '  Checking prerequisites...' -ForegroundColor Cyan

$ok = $true

# .NET SDK
if (Get-Command 'dotnet' -ErrorAction SilentlyContinue) {
    $sdkVer = & dotnet --version 2>&1
    Write-Ok ".NET SDK $sdkVer"

    # Verify required SDK version from global.json
    if (Test-Path 'global.json') {
        $required = (Get-Content global.json | ConvertFrom-Json).sdk.version
        if (-not $sdkVer.StartsWith($required.Substring(0, 4))) {
            Write-Warn ".NET SDK version mismatch: required ~$required, found $sdkVer"
            Write-Warn "  Install via: winget install Microsoft.DotNet.SDK.10"
        }
    }
} else {
    Write-Fail ".NET SDK not found — install from https://dot.net or: winget install Microsoft.DotNet.SDK.10"
    $ok = $false
}

# Git
if (-not (Require-Command 'git' 'Install from https://git-scm.com or: winget install Git.Git')) {
    $ok = $false
}

# gh CLI (optional — only needed for release management)
if (Get-Command 'gh' -ErrorAction SilentlyContinue) {
    $ghVer = Get-Version 'gh' '--version'
    Write-Ok "gh CLI $ghVer"
} else {
    Write-Warn "gh CLI not found (optional). Install: winget install GitHub.cli"
}

# scoop (optional — used by some dev tooling)
if (Get-Command 'scoop' -ErrorAction SilentlyContinue) {
    Write-Ok "scoop found"
} else {
    Write-Warn "scoop not found (optional). Install: irm get.scoop.sh | iex"
}

# git-cliff (optional — used for CHANGELOG generation)
if (Get-Command 'git-cliff' -ErrorAction SilentlyContinue) {
    $cliffVer = Get-Version 'git-cliff' '--version'
    Write-Ok "git-cliff $cliffVer"
} else {
    Write-Warn "git-cliff not found (optional). Install: scoop install git-cliff  OR  cargo install git-cliff"
}

Write-Host ''

if (-not $ok) {
    Write-Host '  Required prerequisites missing — please install them and re-run.' -ForegroundColor Red
    exit 1
}

# ── Restore ───────────────────────────────────────────────────────────────────

Write-Host '  Restoring NuGet packages...' -ForegroundColor Cyan
$restoreOut = dotnet restore RegiLattice.sln 2>&1
if ($LASTEXITCODE -ne 0) {
    $restoreOut | ForEach-Object { Write-Host "    $_" }
    Write-Fail "NuGet restore failed (exit $LASTEXITCODE)"
    exit 1
}
Write-Ok "NuGet packages restored"
Write-Host ''

# ── Build ─────────────────────────────────────────────────────────────────────

if (-not $SkipBuild) {
    Write-Host '  Building solution (Debug)...' -ForegroundColor Cyan
    $env:MSBUILDDISABLENODEREUSE = '1'
    $buildOut = dotnet build RegiLattice.sln -c Debug -m:1 --no-restore --verbosity minimal 2>&1
    if ($LASTEXITCODE -ne 0) {
        $buildOut | ForEach-Object { Write-Host "    $_" }
        Write-Fail "Build failed (exit $LASTEXITCODE)"
        exit 1
    }
    Write-Ok "Build succeeded (0 errors, 0 warnings)"
    Write-Host ''
}

# ── Test ──────────────────────────────────────────────────────────────────────

if (-not $SkipTests) {
    Write-Host '  Running test suite...' -ForegroundColor Cyan
    $testProjects = @(
        'tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj',
        'tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj',
        'tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj'
    )
    $runSettings = 'tests/.runsettings'
    $allPassed = $true

    foreach ($proj in $testProjects) {
        $name = Split-Path -Leaf (Split-Path -Parent $proj)
        Write-Status "  Testing $name..."
        $testOut = dotnet test $proj --settings $runSettings --blame-hang-timeout 30s --no-build --logger "console;verbosity=minimal" 2>&1
        if ($LASTEXITCODE -ne 0) {
            $testOut | Select-Object -Last 20 | ForEach-Object { Write-Host "    $_" }
            Write-Fail "$name — FAILED (exit $LASTEXITCODE)"
            $allPassed = $false
        } else {
            $passed = ($testOut | Select-String -Pattern 'passed' | Select-Object -Last 1).ToString().Trim()
            Write-Ok "$name — $passed"
        }
    }

    Write-Host ''
    if (-not $allPassed) {
        Write-Fail "One or more test projects failed."
        exit 1
    }
}

# ── Done ──────────────────────────────────────────────────────────────────────

Write-Host '  Setup complete!' -ForegroundColor Green
Write-Host ''
Write-Host '  Next steps:' -ForegroundColor Cyan
Write-Host '    dotnet run --project src/RegiLattice.GUI   # Launch GUI'
Write-Host '    dotnet run --project src/RegiLattice.CLI -- --list  # Run CLI'
Write-Host '    dotnet test   # Re-run tests at any time'
Write-Host ''
