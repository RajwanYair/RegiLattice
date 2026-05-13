<#
.SYNOPSIS
    Synchronises hardcoded counts across all Copilot instruction, agent, and skill files.

.DESCRIPTION
    Reads actual counts from the codebase (tweaks, categories, modules, tests) and
    bulk-replaces stale numbers in:
      - .github/copilot-instructions.md
      - .github/agents/regilattice.agent.md
      - .github/instructions/workspace.instructions.md
      - .github/instructions/lessons-learned.instructions.md
      - .github/instructions/testing.instructions.md
      - docs/CHANGELOG.md  (Stats line in current release section only)

.PARAMETER DryRun
    Print proposed replacements without writing any file.

.EXAMPLE
    .\scripts\Sync-CopilotInstructions.ps1
    .\scripts\Sync-CopilotInstructions.ps1 -DryRun
#>
[CmdletBinding()]
param(
    [switch]$DryRun
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = 'Stop'

$Root = Split-Path -Parent $PSScriptRoot

# ─── 1. Measure actual counts ───────────────────────────────────────────────

Write-Host "Measuring codebase counts..." -ForegroundColor Cyan

# Tweaks: count 'Id = "' lines in Tweaks/*.cs
$TweaksDir = Join-Path $Root 'src\RegiLattice.Core\Tweaks'
$TweakCount = (Get-ChildItem $TweaksDir -Filter '*.cs' |
    Select-String -Pattern '\bId\s*=\s*"[a-z][a-z0-9-]+-[a-z][a-z0-9-]+"' |
    Measure-Object).Count

# Modules: .cs files in Tweaks/
$ModuleCount = (Get-ChildItem $TweaksDir -Filter '*.cs').Count

# Categories: unique Category = "..." values
$CategoryCount = (Get-ChildItem $TweaksDir -Filter '*.cs' |
    Select-String -Pattern 'Category\s*=\s*"([^"]+)"' |
    ForEach-Object { $_.Matches[0].Groups[1].Value } |
    Sort-Object -Unique |
    Measure-Object).Count

# Tests: sum passing test counts from test result files OR count [Fact]/[Theory] attributes
$TestCount = (Get-ChildItem (Join-Path $Root 'tests') -Recurse -Filter '*.cs' |
    Select-String -Pattern '^\s*\[(Fact|Theory)\]' |
    Measure-Object).Count

Write-Host "  Tweaks  : $TweakCount" -ForegroundColor Green
Write-Host "  Modules : $ModuleCount" -ForegroundColor Green
Write-Host "  Categories: $CategoryCount" -ForegroundColor Green
Write-Host "  Tests   : $TestCount" -ForegroundColor Green

# ─── 2. Read current values from copilot-instructions.md ───────────────────

$InstructionsFile = Join-Path $Root '.github\copilot-instructions.md'
$InstructionsContent = Get-Content $InstructionsFile -Raw

# Extract current values via regex
function Get-Match([string]$Pattern, [string]$Text) {
    if ($Text -match $Pattern) { return $matches[1] }
    return $null
}

$OldTweakCount = Get-Match '(\d[\d ,]+) tweaks' $InstructionsContent
$OldModuleCount = Get-Match '(\d+) (?:files|modules)' $InstructionsContent
$OldTestCount = Get-Match '(\d[\d ,]+) tests' $InstructionsContent

if (-not $OldTweakCount) {
    Write-Warning "Could not detect old tweak count from copilot-instructions.md — skipping."
    $OldTweakCount = $TweakCount.ToString()
}

# ─── 3. Build replacement table ─────────────────────────────────────────────

# Formats used across different files
$TweakFmt = $TweakCount.ToString()
$TweakFmtK = if ($TweakCount -ge 1000) { "$([math]::Floor($TweakCount/1000)),$($TweakCount % 1000)" } else { $TweakFmt }

$Replacements = @(
    # copilot-instructions.md and agent — use comma-separated thousands for human text
    @{ File = '.github\copilot-instructions.md'; Old = '~7,718 tweaks, 158 categories, 3,304 tests'; New = "~$TweakFmtK tweaks, $CategoryCount categories, $TestCount tests" }
    @{ File = '.github\copilot-instructions.md'; Old = '7,718 tweaks across 158 categories (195 files)'; New = "$TweakFmtK tweaks across $CategoryCount categories ($ModuleCount files)" }
    @{ File = '.github\copilot-instructions.md'; Old = '7,718 across 158 categories (195 files)'; New = "$TweakFmtK across $CategoryCount categories ($ModuleCount files)" }
    @{ File = '.github\copilot-instructions.md'; Old = '3,304 tests (0 failures)'; New = "$TestCount tests (0 failures)" }
    @{ File = '.github\copilot-instructions.md'; Old = '3,304 passing (0 consistent failures)'; New = "$TestCount passing (0 consistent failures)" }
    # Agent
    @{ File = '.github\agents\regilattice.agent.md'; Old = '7,718 tweaks across 158 categories, 195 modules, 3,304 tests'; New = "$TweakFmtK tweaks across $CategoryCount categories, $ModuleCount modules, $TestCount tests" }
    # Workspace instructions
    @{ File = '.github\instructions\workspace.instructions.md'; Old = '195 module files'; New = "$ModuleCount module files" }
    @{ File = '.github\instructions\workspace.instructions.md'; Old = '7,718 tweaks across 158 categories'; New = "$TweakFmtK tweaks across $CategoryCount categories" }
    # Testing instructions
    @{ File = '.github\instructions\testing.instructions.md'; Old = '| **Total**                | **3,304+**|'; New = "| **Total**                | **$TestCount+**|" }
    @{ File = '.github\instructions\testing.instructions.md'; Old = "all 3,304+ tests must pass"; New = "all $TestCount+ tests must pass" }
)

# ─── 4. Apply replacements ───────────────────────────────────────────────────

$TotalChanged = 0
foreach ($r in $Replacements) {
    $FullPath = Join-Path $Root $r.File
    if (-not (Test-Path $FullPath)) {
        Write-Warning "File not found: $FullPath — skipping."
        continue
    }

    $Content = Get-Content $FullPath -Raw
    if ($Content -notlike "*$($r.Old)*") {
        Write-Verbose "No match for '$($r.Old)' in $($r.File)"
        continue
    }

    if ($DryRun) {
        Write-Host "[DRY-RUN] $($r.File)" -ForegroundColor Yellow
        Write-Host "  OLD: $($r.Old)"
        Write-Host "  NEW: $($r.New)"
    } else {
        $NewContent = $Content.Replace($r.Old, $r.New)
        if ($NewContent -ne $Content) {
            Set-Content -Path $FullPath -Value $NewContent -NoNewline
            Write-Host "Updated: $($r.File)" -ForegroundColor Green
            $TotalChanged++
        }
    }
}

if ($DryRun) {
    Write-Host "`n[DRY-RUN] No files written." -ForegroundColor Yellow
} else {
    Write-Host "`nDone. $TotalChanged file(s) updated." -ForegroundColor Cyan
}
