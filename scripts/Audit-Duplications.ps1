<#
.SYNOPSIS
    Comprehensive duplication audit for the RegiLattice codebase.

.DESCRIPTION
    Scans the RegiLattice source tree for 4 types of duplication:
      1. Duplicate tweak IDs (hard error — build will fail)
      2. Duplicate registry operations (same PATH\ValueName written by 2+ tweaks)
      3. Duplicate tweak labels (same human-readable name across modules)
      4. Duplicate documentation/instruction files and config entries

    Reports results with colour coding and exits with code 1 if any
    hard violations (duplicate IDs) are found.

.EXAMPLE
    . .\scripts\Audit-Duplications.ps1
    # Run from the repo root — outputs a detailed colour-coded report

.NOTES
    Run this script before committing any changes to src/RegiLattice.Core/Tweaks/.
    For background on the rules, see .github/instructions/no-duplication.instructions.md
#>

[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# ── Helpers ──────────────────────────────────────────────────────────────────

function Write-Header([string]$text) {
    Write-Host "`n══════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host "  $text" -ForegroundColor Cyan
    Write-Host "══════════════════════════════════════════════════════" -ForegroundColor Cyan
}

function Write-Pass([string]$text) { Write-Host "  ✔  $text" -ForegroundColor Green }
function Write-Warn([string]$text) { Write-Host "  ⚠  $text" -ForegroundColor Yellow }
function Write-Fail([string]$text) { Write-Host "  ✖  $text" -ForegroundColor Red }
function Write-Info([string]$text) { Write-Host "     $text" -ForegroundColor DarkGray }

# ── Resolve root dir ─────────────────────────────────────────────────────────

$Root = $PSScriptRoot ? (Split-Path $PSScriptRoot -Parent) : (Get-Location).Path
$TweaksDir = Join-Path $Root 'src\RegiLattice.Core\Tweaks'
$InstructionsDir = Join-Path $Root '.github\instructions'
$SkillsDir = Join-Path $Root '.github\skills'

if (-not (Test-Path $TweaksDir)) {
    Write-Error "Tweaks directory not found: $TweaksDir`nRun this script from the repo root."
    exit 2
}

$hardViolations = 0
$warnings = 0

# ──────────────────────────────────────────────────────────────────────────────
# LAYER 1 — Duplicate Tweak IDs
# ──────────────────────────────────────────────────────────────────────────────

Write-Header "Layer 1: Duplicate Tweak IDs"

$tweakFiles = Get-ChildItem -Path $TweaksDir -Filter '*.cs'
$allIds = @()

foreach ($file in $tweakFiles) {
    $content = Get-Content $file.FullName -Raw
    $matches = [regex]::Matches($content, 'Id\s*=\s*"([^"]+)"')
    foreach ($m in $matches) {
        $allIds += [PSCustomObject]@{ Id = $m.Groups[1].Value; File = $file.Name }
    }
}

$dupIds = $allIds | Group-Object Id | Where-Object { $_.Count -gt 1 }

if ($dupIds.Count -eq 0) {
    Write-Pass "No duplicate IDs found across $($tweakFiles.Count) modules ($($allIds.Count) total IDs scanned)"
} else {
    foreach ($dup in $dupIds | Sort-Object Count -Descending) {
        $files = ($dup.Group | Select-Object -ExpandProperty File) -join ', '
        Write-Fail "DUPLICATE ID '$($dup.Name)' appears $($dup.Count) times: $files"
        $hardViolations++
    }
}

# ──────────────────────────────────────────────────────────────────────────────
# LAYER 2 — Duplicate Registry Operations (same PATH\ValueName in ApplyOps)
# ──────────────────────────────────────────────────────────────────────────────

Write-Header "Layer 2: Duplicate Registry Operations"

$regOps = @()

foreach ($file in $tweakFiles) {
    $lines = Get-Content $file.FullName
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        # Match Set* operations: RegOp.SetDword(@"PATH", "Name", value)
        if ($line -match 'RegOp\.(SetDword|SetString|SetExpandString|SetQword|SetBinary|SetMultiSz|DeleteValue|DeleteTree)\(') {
            # Extract path and optional value name
            $pathMatch = [regex]::Match($line, '@?"((?:HKEY_[^"]+|HKLM[^"]+|HKCU[^"]+))"')
            if ($pathMatch.Success) {
                $path = $pathMatch.Groups[1].Value
                $nameMatch = [regex]::Match($line, '@?"(?:HKEY_[^"]+|HKLM[^"]+|HKCU[^"]+)",\s*"([^"]+)"')
                $key = if ($nameMatch.Success) { "$path\$($nameMatch.Groups[1].Value)" } else { $path }
                $regOps += [PSCustomObject]@{ Key = $key; File = $file.Name; Line = $i + 1 }
            }
        }
    }
}

$dupRegOps = $regOps | Group-Object Key | Where-Object { $_.Count -gt 1 }

if ($dupRegOps.Count -eq 0) {
    Write-Pass "No duplicate registry operations found ($($regOps.Count) total ops scanned)"
} else {
    Write-Warn "$($dupRegOps.Count) registry targets written by multiple tweaks (source-level scan)"
    Write-Info "Note: run 'dotnet test --filter DuplicateRegistryOps' for the exact model-level count"
    Write-Info ""
    Write-Info "TOP 20 DUPLICATES:"

    $top20 = $dupRegOps | Sort-Object Count -Descending | Select-Object -First 20
    foreach ($dup in $top20) {
        $locations = ($dup.Group | ForEach-Object { "$($_.File):$($_.Line)" }) -join ', '
        Write-Warn "  [$($dup.Count)×] $($dup.Name)"
        Write-Info "       at: $locations"
    }

    if ($dupRegOps.Count -gt 20) {
        Write-Info "  ... and $($dupRegOps.Count - 20) more. Run full report for complete list."
    }
    $warnings += $dupRegOps.Count
}

# ──────────────────────────────────────────────────────────────────────────────
# LAYER 3 — Duplicate Tweak Labels
# ──────────────────────────────────────────────────────────────────────────────

Write-Header "Layer 3: Duplicate Tweak Labels"

$allLabels = @()

foreach ($file in $tweakFiles) {
    $content = Get-Content $file.FullName -Raw
    $matches = [regex]::Matches($content, 'Label\s*=\s*"([^"]+)"')
    foreach ($m in $matches) {
        $allLabels += [PSCustomObject]@{ Label = $m.Groups[1].Value; File = $file.Name }
    }
}

$dupLabels = $allLabels | Group-Object Label | Where-Object { $_.Count -gt 1 }

if ($dupLabels.Count -eq 0) {
    Write-Pass "No duplicate labels found ($($allLabels.Count) total labels scanned)"
} else {
    Write-Warn "$($dupLabels.Count) label(s) used in multiple modules"
    Write-Info "Note: same label across different modules is OK IF registry paths differ."
    Write-Info "Same label + same RegistryKeys[0] = guaranteed duplicate (must remove)."
    Write-Info ""
    Write-Info "TOP 15 LABEL DUPLICATES:"

    $top15 = $dupLabels | Sort-Object Count -Descending | Select-Object -First 15
    foreach ($dup in $top15) {
        $files = ($dup.Group | Select-Object -ExpandProperty File | Select-Object -Unique) -join ', '
        Write-Warn "  [$($dup.Count)×] `"$($dup.Name)`" — in: $files"
    }

    if ($dupLabels.Count -gt 15) {
        Write-Info "  ... and $($dupLabels.Count - 15) more."
    }
    $warnings += $dupLabels.Count
}

# ──────────────────────────────────────────────────────────────────────────────
# LAYER 4 — Documentation / Config Duplications
# ──────────────────────────────────────────────────────────────────────────────

Write-Header "Layer 4: Documentation and Config Duplications"

# Check for duplicate instruction file topics (by filename stem)
if (Test-Path $InstructionsDir) {
    $instructionFiles = Get-ChildItem -Path $InstructionsDir -Filter '*.md'
    $dupInstructions = $instructionFiles | Group-Object Name | Where-Object { $_.Count -gt 1 }

    if ($dupInstructions.Count -eq 0) {
        Write-Pass "$($instructionFiles.Count) instruction files — all unique names"
    } else {
        foreach ($dup in $dupInstructions) {
            Write-Fail "Duplicate instruction filename: $($dup.Name)"
            $hardViolations++
        }
    }
}

# Check .vscode/settings.json for duplicate instruction entries
$settingsFile = Join-Path $Root '.vscode\settings.json'
if (Test-Path $settingsFile) {
    $settingsContent = Get-Content $settingsFile -Raw
    $fileRefs = [regex]::Matches($settingsContent, '"file":\s*"([^"]+)"')
    $fileRefValues = $fileRefs | ForEach-Object { $_.Groups[1].Value }
    $dupRefs = $fileRefValues | Group-Object | Where-Object { $_.Count -gt 1 }

    if ($dupRefs.Count -eq 0) {
        Write-Pass ".vscode/settings.json — no duplicate instruction references"
    } else {
        foreach ($dup in $dupRefs) {
            Write-Fail "Duplicate file reference in settings.json: $($dup.Name)"
            $hardViolations++
        }
    }
}

# Check Directory.Packages.props for duplicate PackageVersion entries
$packagesProps = Join-Path $Root 'Directory.Packages.props'
if (Test-Path $packagesProps) {
    $pkgContent = Get-Content $packagesProps -Raw
    $pkgRefs = [regex]::Matches($pkgContent, 'PackageVersion\s+Include="([^"]+)"')
    $pkgNames = $pkgRefs | ForEach-Object { $_.Groups[1].Value }
    $dupPkgs = $pkgNames | Group-Object | Where-Object { $_.Count -gt 1 }

    if ($dupPkgs.Count -eq 0) {
        Write-Pass "Directory.Packages.props — no duplicate PackageVersion entries ($($pkgNames.Count) packages)"
    } else {
        foreach ($dup in $dupPkgs) {
            Write-Fail "Duplicate PackageVersion in Directory.Packages.props: $($dup.Name)"
            $hardViolations++
        }
    }
}

# ──────────────────────────────────────────────────────────────────────────────
# SUMMARY
# ──────────────────────────────────────────────────────────────────────────────

Write-Header "Audit Summary"

$totalModules = $tweakFiles.Count
$totalIds = $allIds.Count
$totalOps = $regOps.Count
$totalLabels = $allLabels.Count

Write-Info "Scanned: $totalModules modules | $totalIds IDs | $totalOps registry ops | $totalLabels labels"
Write-Info ""

if ($hardViolations -gt 0) {
    Write-Fail "$hardViolations hard violation(s) found — these MUST be fixed before committing."
    Write-Info "See .github/instructions/no-duplication.instructions.md for resolution guidance."
} elseif ($warnings -gt 0) {
    Write-Warn "$warnings warning(s) found — review the items above and eliminate unnecessary duplicates."
    Write-Info "See .github/skills/no-duplication/SKILL.md for the resolution decision tree."
} else {
    Write-Pass "All clean — no duplications detected!"
}

Write-Host ""

if ($hardViolations -gt 0) { exit 1 }
exit 0
