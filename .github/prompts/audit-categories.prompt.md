---
mode: agent
description: "Audit all 158 tweak categories: identify duplicates, stale IDs, missing registry ops, unbalanced tweak counts, and category naming inconsistencies."
---

# Audit Categories

Perform a comprehensive audit of all tweak categories in `src/RegiLattice.Core/Tweaks/`.

## Scope

Category to audit (leave blank for all):
`${input:Category name or slug (e.g. Privacy, perf, wsl) — or press Enter for all}`

## Audit Steps

### 1. Count tweaks per category

```powershell
dotnet run --project src/RegiLattice.CLI -- --show-categories --no-color
```

Flag categories with:
- **< 5 tweaks** — may be too granular; consider merging
- **> 100 tweaks** — may benefit from splitting into sub-categories

### 2. Check for duplicate registry ops within a category

```powershell
# Find duplicate PATH\ValueName combos within each category's modules
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'SetDword|SetString' |
    ForEach-Object { ($_ -split '"')[1..3] -join '"' } |
    Group-Object |
    Where-Object Count -gt 1 |
    Select-Object Name, Count
```

### 3. Verify ID naming convention

All IDs must match `{category_slug}-{descriptive-name}`.

```powershell
# Find IDs that don't match the slug pattern
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'Id\s*=\s*"([^"]+)"' |
    ForEach-Object { $_.Matches[0].Groups[1].Value } |
    Where-Object { $_ -notmatch '^[a-z][a-z0-9]+-[a-z][a-z0-9-]+$' }
```

### 4. Find tweaks missing required ops

Every tweak must have `ApplyOps` or `ApplyAction`. Tweaks without these are silently
skipped by `TweakEngine.Register()`:

```powershell
# Tweaks with no ApplyOps and no ApplyAction in the same block
$tweakBlocks = Get-Content src/RegiLattice.Core/Tweaks/*.cs -Raw | Select-String -Pattern 'new TweakDef\s*\{[^}]+\}' -AllMatches
```

### 5. Check ImpactScore / SafetyRating are set explicitly

```powershell
# Find TweakDef blocks that don't set ImpactScore or SafetyRating
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'new TweakDef' |
    ForEach-Object {
        $file = $_.Path; $line = $_.LineNumber
        $block = Get-Content $file |
            Select-Object -Skip ($line - 1) -First 30 |
            Out-String
        if ($block -notmatch 'ImpactScore' -or $block -notmatch 'SafetyRating') {
            "$file`:$line — missing ImpactScore/SafetyRating"
        }
    }
```

### 6. Verify categories are registered in TweakEngine

Every module must be registered in `TweakEngine.RegisterBuiltins()`:

```powershell
# Module file names vs RegisterBuiltins() calls
$Files = Get-ChildItem src/RegiLattice.Core/Tweaks -Filter '*.cs' | Select-Object -ExpandProperty BaseName
$Registered = Select-String -Path src/RegiLattice.Core/TweakEngine.cs -Pattern 'Register\((new\s+)?(\w+)' |
    ForEach-Object { $_.Matches[0].Groups[2].Value }
$Files | Where-Object { $_ -notin $Registered }
```

### 7. Validate via CLI

```powershell
dotnet run --project src/RegiLattice.CLI -- --validate --no-color
```

### 8. Run duplication detection test

```powershell
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj `
    --filter "RegisterBuiltins_AllIdsUnique" `
    --settings tests/.runsettings `
    --logger "console;verbosity=minimal"
```

## Output Format

Report findings as:

```
CATEGORY AUDIT REPORT — YYYY-MM-DD
===================================

Totals
  Categories : NNN
  Tweaks     : N,NNN
  Modules    : NNN

Issues Found
  [DUPLICATE-ID] priv-disable-telemetry — appears in Privacy.cs and Telemetry.cs
  [MISSING-OPS]  boot-fast-startup — no ApplyOps and no ApplyAction
  [STALE-ID]     w11-old-name — ID slug 'w11' but category is 'Windows 11'
  [NO-IMPACT]    svc-disable-search — missing ImpactScore/SafetyRating

Categories Needing Attention
  Font (2 tweaks) — consider merging into Display
  Developer Sub2 (3 tweaks) — consider merging into Dev Drive
```
