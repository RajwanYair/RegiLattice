<#
.SYNOPSIS
    Automated version bump script for RegiLattice.
    Updates all 28+ files from a single invocation.

.DESCRIPTION
    Updates Directory.Build.props, installer/Package.wxs, README.md, docs/CHANGELOG.md,
    .github/copilot-instructions.md, all SVG assets, and all package registry manifests.

.PARAMETER Version
    New semantic version string (e.g. "6.34.0").

.PARAMETER TweakCount
    Total tweak count after this bump. Pass 0 to auto-detect from the compiled assembly.

.PARAMETER CategoryCount
    Total category count after this bump.

.PARAMETER TestCount
    Total test count after this bump.

.PARAMETER ModuleCount
    Total module file count (*.cs in Tweaks/).

.PARAMETER DryRun
    Show what would change without modifying any files.

.PARAMETER AutoDetectCounts
    Auto-detect tweak/category/test/module counts from the build output.

.EXAMPLE
    .\scripts\Bump-Version.ps1 -Version "6.34.0" -TweakCount 7768 -CategoryCount 163 -TestCount 3346
    .\scripts\Bump-Version.ps1 -Version "6.34.0" -AutoDetectCounts
    .\scripts\Bump-Version.ps1 -Version "6.34.0" -AutoDetectCounts -DryRun
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^\d+\.\d+\.\d+$')]
    [string] $Version,

    [int] $TweakCount = 0,
    [int] $CategoryCount = 0,
    [int] $TestCount = 0,
    [int] $ModuleCount = 0,

    [switch] $DryRun,
    [switch] $AutoDetectCounts
)

$ErrorActionPreference = 'Stop'
$Root = Split-Path $PSScriptRoot -Parent
$Today = Get-Date -Format 'yyyy-MM-dd'
$Major, $Minor, $Patch = $Version -split '\.'

Write-Host "RegiLattice Bump-Version.ps1" -ForegroundColor Cyan
Write-Host "Version : $Version" -ForegroundColor Cyan
Write-Host "Date    : $Today" -ForegroundColor Cyan
if ($DryRun) { Write-Host "[DRY RUN] No files will be modified." -ForegroundColor Yellow }

# ---------------------------------------------------------------------------
# Auto-detect counts from the compiled assembly or Tweaks/ directory
# ---------------------------------------------------------------------------
if ($AutoDetectCounts -or $TweakCount -eq 0) {
    Write-Host "`nAuto-detecting counts..." -ForegroundColor Gray

    $tweaksDir = Join-Path $Root 'src\RegiLattice.Core\Tweaks'
    $moduleFiles = Get-ChildItem -Path $tweaksDir -Filter '*.cs' | Where-Object { $_.Name -notmatch '\.Part\d+\.cs$' }
    if ($ModuleCount -eq 0) { $ModuleCount = (Get-ChildItem -Path $tweaksDir -Filter '*.cs').Count }

    # Count tweaks by searching for 'new TweakDef' in all module files
    $tweakMatches = Select-String -Path (Join-Path $tweaksDir '*.cs') -Pattern 'new TweakDef\b' -ErrorAction SilentlyContinue
    if ($TweakCount -eq 0 -and $tweakMatches) { $TweakCount = $tweakMatches.Count }

    # Count categories by collecting unique Category values
    $categoryMatches = Select-String -Path (Join-Path $tweaksDir '*.cs') -Pattern 'Category\s*=\s*"([^"]+)"' -ErrorAction SilentlyContinue
    if ($CategoryCount -eq 0 -and $categoryMatches) {
        $CategoryCount = ($categoryMatches | ForEach-Object { $_.Matches[0].Groups[1].Value } | Sort-Object -Unique).Count
    }

    # Count tests by scanning test files for [Fact] and [Theory]
    $testDir = Join-Path $Root 'tests'
    $testMatches = Select-String -Path (Get-ChildItem -Path $testDir -Filter '*.cs' -Recurse) `
        -Pattern '\[Fact\]|\[Theory\]' -ErrorAction SilentlyContinue
    if ($TestCount -eq 0 -and $testMatches) { $TestCount = $testMatches.Count }

    Write-Host "  Tweaks   : $TweakCount" -ForegroundColor Gray
    Write-Host "  Categories: $CategoryCount" -ForegroundColor Gray
    Write-Host "  Tests    : $TestCount" -ForegroundColor Gray
    Write-Host "  Modules  : $ModuleCount" -ForegroundColor Gray
}

# Validate we have counts
if ($TweakCount -eq 0 -or $CategoryCount -eq 0 -or $TestCount -eq 0) {
    Write-Error "Could not auto-detect counts. Pass -TweakCount, -CategoryCount, -TestCount explicitly."
}

# ---------------------------------------------------------------------------
# Helper functions
# ---------------------------------------------------------------------------
function Update-File {
    param([string]$Path, [string]$OldText, [string]$NewText, [string]$Description)
    $fullPath = Join-Path $Root $Path
    if (-not (Test-Path $fullPath)) {
        Write-Warning "  SKIP (not found): $Path"
        return
    }
    $content = Get-Content $fullPath -Raw -Encoding UTF8
    if ($content -notlike "*$OldText*") {
        Write-Warning "  SKIP (pattern not found): $Path — '$OldText'"
        return
    }
    if ($DryRun) {
        Write-Host "  [DRY] $Description in $Path" -ForegroundColor Yellow
        Write-Host "        - $OldText" -ForegroundColor DarkRed
        Write-Host "        + $NewText" -ForegroundColor DarkGreen
    } else {
        ($content -replace [regex]::Escape($OldText), $NewText) | Set-Content $fullPath -Encoding UTF8 -NoNewline
        Write-Host "  OK  $Description" -ForegroundColor Green
    }
}

function Update-FileRegex {
    param([string]$Path, [string]$Pattern, [string]$Replacement, [string]$Description)
    $fullPath = Join-Path $Root $Path
    if (-not (Test-Path $fullPath)) {
        Write-Warning "  SKIP (not found): $Path"
        return
    }
    $content = Get-Content $fullPath -Raw -Encoding UTF8
    $newContent = $content -replace $Pattern, $Replacement
    if ($content -eq $newContent) {
        Write-Warning "  SKIP (no change): $Path — $Description"
        return
    }
    if ($DryRun) {
        Write-Host "  [DRY] $Description in $Path" -ForegroundColor Yellow
    } else {
        $newContent | Set-Content $fullPath -Encoding UTF8 -NoNewline
        Write-Host "  OK  $Description" -ForegroundColor Green
    }
}

function Prepend-File {
    param([string]$Path, [string]$NewContent, [string]$Description)
    $fullPath = Join-Path $Root $Path
    if (-not (Test-Path $fullPath)) {
        Write-Warning "  SKIP (not found): $Path"
        return
    }
    if ($DryRun) {
        Write-Host "  [DRY] $Description in $Path" -ForegroundColor Yellow
        return
    }
    $existing = Get-Content $fullPath -Raw -Encoding UTF8
    "$NewContent`n$existing" | Set-Content $fullPath -Encoding UTF8 -NoNewline
    Write-Host "  OK  $Description" -ForegroundColor Green
}

# Need old version to construct correct replacements
$dbpPath = Join-Path $Root 'Directory.Build.props'
$dbpContent = Get-Content $dbpPath -Raw
if ($dbpContent -match '<Version>(\d+\.\d+\.\d+)</Version>') {
    $OldVersion = $Matches[1]
} else {
    Write-Error "Could not find current version in Directory.Build.props"
}
Write-Host "`nOld version: $OldVersion" -ForegroundColor Gray
Write-Host "New version: $Version`n" -ForegroundColor Gray

# ---------------------------------------------------------------------------
# Group A: Version properties
# ---------------------------------------------------------------------------
Write-Host "--- Group A: Version files ---" -ForegroundColor Cyan

Update-File 'Directory.Build.props' "<Version>$OldVersion</Version>" "<Version>$Version</Version>" 'Version'
Update-File 'Directory.Build.props' "<AssemblyVersion>$OldVersion.0</AssemblyVersion>" "<AssemblyVersion>$Version.0</AssemblyVersion>" 'AssemblyVersion'
Update-File 'Directory.Build.props' "<FileVersion>$OldVersion.0</FileVersion>" "<FileVersion>$Version.0</FileVersion>" 'FileVersion'
Update-File 'Directory.Build.props' "<InformationalVersion>$OldVersion</InformationalVersion>" "<InformationalVersion>$Version</InformationalVersion>" 'InformationalVersion'
Update-File 'installer\Package.wxs' "Version=""$OldVersion""" "Version=""$Version""" 'Package.wxs version'

# ---------------------------------------------------------------------------
# Group B: SVG graphics
# ---------------------------------------------------------------------------
Write-Host "`n--- Group B: SVG graphics ---" -ForegroundColor Cyan

# Detect old counts from stats.svg
$svgPath = Join-Path $Root 'docs\assets\stats.svg'
$svgContent = Get-Content $svgPath -Raw -ErrorAction SilentlyContinue
$OldTweakCount = if ($svgContent -match '>(\d{4,5})<') { $Matches[1] } else { '0' }

Update-FileRegex 'docs\assets\stats.svg' '>(\d{4,5})<' ">$TweakCount<" "stats.svg tweak count"
Update-FileRegex 'docs\assets\banner.svg' '(\d{4,5}) Tweaks' "$TweakCount Tweaks" "banner.svg tweak pill"
Update-FileRegex 'docs\assets\banner.svg' '(\d{3}) Categories' "$CategoryCount Categories" "banner.svg category pill"
Update-FileRegex 'docs\assets\banner.svg' '(\d{4}) Tests' "$TestCount Tests" "banner.svg tests pill"
Update-FileRegex 'docs\assets\architecture.svg' 'TweakDef Modules \(\d+\)' "TweakDef Modules ($ModuleCount)" "architecture.svg module count"
Update-FileRegex 'docs\assets\architecture.svg' '(\d{4,5}) tweaks' "$TweakCount tweaks" "architecture.svg tweak count"
Update-FileRegex 'docs\assets\how-it-works.svg' '(\d{4,5}) tweaks' "$TweakCount tweaks" "how-it-works.svg tweak count"
Update-FileRegex 'docs\assets\project-structure.svg' '(\d{4,5}) tweak' "$TweakCount tweak" "project-structure.svg tweak count"
Update-FileRegex 'docs\assets\solution-overview.svg' '(\d{4,5}) tweak' "$TweakCount tweak" "solution-overview.svg tweak count"

# ---------------------------------------------------------------------------
# Group C: Documentation
# ---------------------------------------------------------------------------
Write-Host "`n--- Group C: Documentation ---" -ForegroundColor Cyan

# README.md — multiple locations (regex-safe)
Update-FileRegex 'README.md' '\*\*\d{4,5} verified tweaks\*\*' "**$TweakCount verified tweaks**" "README.md verified tweaks bold"
Update-FileRegex 'README.md' '\*\*\d{3,4} categories\*\*' "**$CategoryCount categories**" "README.md categories bold"
Update-FileRegex 'README.md' '(\d{4,5}) tweaks.*?(\d{3,4}) categor' "$TweakCount tweaks across $CategoryCount categor" "README.md inline count"
Update-FileRegex 'README.md' "v$([regex]::Escape($OldVersion))" "v$Version" "README.md version badge"

# CHANGELOG: prepend new section
$changelogEntry = @"
## [$Version] — $Today

### Stats

- Tweaks: $TweakCount
- Categories: $CategoryCount
- Modules: $ModuleCount
- Tests: $TestCount

"@
Prepend-File 'docs\CHANGELOG.md' $changelogEntry "Prepend CHANGELOG entry"

# copilot-instructions.md
Update-FileRegex '.github\copilot-instructions.md' 'Last verified: \d{4}-\d{2}-\d{2} \(v[\d.]+' "Last verified: $Today (v$Version" 'copilot-instructions header'
Update-FileRegex '.github\copilot-instructions.md' '\| Version\s+\| [\d.]+' "| Version  | $Version" 'copilot-instructions version row'
Update-FileRegex '.github\copilot-instructions.md' '\| Tweaks\s+\| \d{4,5} across' "| Tweaks   | $TweakCount across" 'copilot-instructions tweaks row'
Update-FileRegex '.github\copilot-instructions.md' '~\d{4,5} tweaks, \d{3,4} categories, \d{4} tests' "~$TweakCount tweaks, $CategoryCount categories, $TestCount tests" 'copilot-instructions header stats'

# ---------------------------------------------------------------------------
# Group D: Package registry manifests
# ---------------------------------------------------------------------------
Write-Host "`n--- Group D: Package manifests ---" -ForegroundColor Cyan

Update-FileRegex 'chocolatey\regilattice.nuspec' '<version>[\d.]+</version>' "<version>$Version</version>" 'chocolatey version'
Update-FileRegex 'chocolatey\regilattice.nuspec' '\d{4,5} tweaks' "$TweakCount tweaks" 'chocolatey tweak count'
Update-File 'scoop\regilattice.json' "`"version`": `"$OldVersion`"" "`"version`": `"$Version`"" 'scoop version'
Update-FileRegex 'scoop\regilattice.json' '\d{4,5} tweaks' "$TweakCount tweaks" 'scoop tweak count'
Update-File 'winget\RegiLattice.RegiLattice.yaml' "PackageVersion: $OldVersion" "PackageVersion: $Version" 'winget yaml version'
Update-File 'winget\RegiLattice.RegiLattice.installer.yaml' "PackageVersion: $OldVersion" "PackageVersion: $Version" 'winget installer version'
Update-File 'winget\RegiLattice.RegiLattice.locale.en-US.yaml' "PackageVersion: $OldVersion" "PackageVersion: $Version" 'winget locale version'
Update-FileRegex 'winget\RegiLattice.RegiLattice.locale.en-US.yaml' '\d{4,5} tweaks' "$TweakCount tweaks" 'winget locale tweak count'

# ---------------------------------------------------------------------------
# Group E: Repo memory
# ---------------------------------------------------------------------------
Write-Host "`n--- Group E: Instruction files ---" -ForegroundColor Cyan

Update-FileRegex '.github\instructions\lessons-learned.instructions.md' '\*\*Current version\*\*: v[\d.]+ —' "**Current version**: v$Version —" 'lessons-learned current version'
Update-FileRegex '.github\instructions\lessons-learned.instructions.md' '\d{4,5} tweaks, \d{3,4} categories, \d{3,4} files, \d{4,5} tests' "$TweakCount tweaks, $CategoryCount categories, $ModuleCount files, $TestCount tests" 'lessons-learned counts'

Update-FileRegex '.github\instructions\workspace.instructions.md' '\d{3,4} module files.*?\d{4,5} tweaks across \d{3,4} categories' "$ModuleCount module files (31 original + $($ModuleCount - 31) extracted/split), $TweakCount tweaks across $CategoryCount categories" 'workspace.instructions tweaks line'
Update-FileRegex '.github\instructions\testing.instructions.md' '\*\*Total\*\*\s+\| \*\*[\d,]+\*\*' "**Total**                | **$("{0:N0}" -f $TestCount)**" 'testing.instructions total'

Update-FileRegex '.github\agents\regilattice.agent.md' '\d{4,5} tweaks across \d{3,4} categories, \d{3,4} modules, \d{4,5} tests' "$TweakCount tweaks across $CategoryCount categories, $ModuleCount modules, $TestCount tests" 'agent current state'

Update-FileRegex 'docs\Development.md' 'Last updated: \d{4}-\d{2}-\d{2} . v[\d.]+' "Last updated: $Today · v$Version" 'Development.md header'
Update-FileRegex 'docs\Roadmap.md' '\*\*Baseline\*\*: v[\d.]+ — [\d,]+ tweaks' "**Baseline**: v$Version — $TweakCount tweaks" 'Roadmap.md baseline'

Update-File 'powershell\RegiLattice.psd1' "ModuleVersion     = '$OldVersion'" "ModuleVersion     = '$Version'" 'PowerShell module version'
Update-FileRegex 'Dockerfile' '\d{4,5} tweaks across \d{3,4} categories' "$TweakCount tweaks across $CategoryCount categories" 'Dockerfile description'

# ---------------------------------------------------------------------------
# Summary
# ---------------------------------------------------------------------------
Write-Host "`n--- Done ---" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "DRY RUN complete. No files were modified." -ForegroundColor Yellow
} else {
    Write-Host "Version bump to v$Version complete." -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor White
    Write-Host "  git add -A" -ForegroundColor Gray
    Write-Host "  git commit -m `"chore: bump version to v$Version`"" -ForegroundColor Gray
    Write-Host "  git tag v$Version" -ForegroundColor Gray
    Write-Host "  git push; git push --tags" -ForegroundColor Gray
}
