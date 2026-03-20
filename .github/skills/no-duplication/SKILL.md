---
name: no-duplication
description: "Detect and prevent all forms of duplication in RegiLattice: tweak IDs, registry ops, labels, docs, and config. Use when adding tweaks, reviewing code, auditing categories, or before committing. Triggers on: 'duplicate', 'duplication', 'same tweak', 'already exists', 'audit', 'check duplicates', 'dedup', 'overlap'."
argument-hint: "What to audit: 'all', 'registry-ops', 'labels', 'ids', or a specific module name (e.g. 'Privacy', 'Office')"
---

# No-Duplication Audit — RegiLattice Skill

## When to Use

- Before committing a batch of new tweaks
- After a sprint that added 10+ tweaks across multiple modules
- When a build fails with "Duplicate tweak Id" from `TweakEngine.Register()`
- When a test fails in `TweakEngineBuiltinsTests` (duplication guard tests)
- When reviewing code for duplicate registry operations or labels
- When running `scripts/Audit-Duplications.ps1` to get a full picture

## The 4 Duplication Types

| Type | Severity | Auto-Detected By |
|------|----------|-----------------|
| Duplicate ID | **HARD BLOCK** (build error) | `TweakEngine.Register()` + `RegisterBuiltins_AllIdsUnique` test |
| Duplicate registry op (same PATH\ValueName by 2+ tweaks) | **Warning** | `TweakValidator.DetectDuplicateRegistryOps()` + builtins threshold test |
| Duplicate label (same human name in 2+ modules) | **Smell** | Label scan + `RegisterBuiltins_NoCrossModuleLabelAndPathCollision` test |
| Conceptual duplicate (same outcome, different code path) | **Debt** | Manual review + `Audit-Duplications.ps1` |

---

## Step-by-Step Audit Workflow

### Step 1 — Check ID Uniqueness

```powershell
# Before adding a new ID, search all modules
Select-String -Pattern '"proposed-id"' -Path src/RegiLattice.Core/Tweaks/*.cs
# Zero results = safe to use
```

For a batch scan of ALL duplicate IDs in the codebase:
```powershell
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'Id = "([^"]+)"' |
    ForEach-Object { [regex]::Match($_.Line, 'Id = "([^"]+)"').Groups[1].Value } |
    Group-Object | Where-Object Count -gt 1 | Sort-Object Count -Descending |
    Select-Object Count, Name
```

**Expected result**: Empty (0 duplicate IDs). Any result here is a hard bug.

---

### Step 2 — Check for Duplicate Registry Operations

Run the model-level check (exact, matches what the unit test does):
```powershell
# This requires dotnet scripting or a unit test run
dotnet test tests/RegiLattice.Core.Tests --filter "DuplicateRegistryOps" --no-build --logger "console;verbosity=normal"
```

For a quick source-level approximation:
```powershell
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern '(SetDword|SetString|SetExpandString|SetQword)\s*\(' |
    ForEach-Object {
        $m = [regex]::Match($_.Line, '@?"([^"]+)",\s*"([^"]+)"')
        if ($m.Success) { "$($m.Groups[1].Value)\$($m.Groups[2].Value)" }
    } | Group-Object | Where-Object Count -gt 1 |
    Sort-Object Count -Descending | Select-Object -First 20 Count, Name
```

**Expected result**: Ideally 0. Current known debt: ~300–800 duplicate targets exist.
New additions must not increase this count — the threshold test will catch them.

---

### Step 3 — Check for Duplicate Labels

```powershell
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'Label = "([^"]+)"' |
    ForEach-Object { [regex]::Match($_.Line, 'Label = "([^"]+)"').Groups[1].Value } |
    Group-Object | Where-Object Count -gt 1 |
    Sort-Object Count -Descending | Select-Object -First 20 Count, Name
```

**Expected result**: Label collisions are acceptable ONLY if the tweaks operate through
different registry paths (same feature, different mechanism). Same Label + same
`RegistryKeys[0]` = guaranteed duplicate → must remove.

---

### Step 4 — Check for Duplicate Instruction/Skill Files

```powershell
# Duplicate instruction filenames (different paths, same content intent)
Get-ChildItem .github/instructions/*.md | Select-Object Name
Get-ChildItem .github/skills/**/*.md -Recurse | Select-Object Name, FullName

# Check for instruction files that cover the same topic
# (manual review — look for overlapping topics across files)
```

---

### Step 5 — Check for Duplicate Config/Build Entries

```powershell
# NuGet package references (should only appear once per PackageReference)
Select-String -Path Directory.Packages.props -Pattern 'PackageVersion' | 
    ForEach-Object { ($_ -split '"')[1] } |
    Group-Object | Where-Object Count -gt 1

# .vscode/settings.json instruction list (no file should appear twice)
Select-String -Path .vscode/settings.json -Pattern '"file":'
```

---

### Step 6 — Run the Full Audit Script

```powershell
. .\scripts\Audit-Duplications.ps1
```

This covers all above layers and produces a colour-coded summary report.
Exit code 1 if any hard violations (duplicate IDs) are found.

---

## Resolution Strategy

### Removing a Functional Duplicate

1. Identify which tweak is the "original" (older creation date, or the one in the semantically
   correct module based on its category)
2. Keep the original; delete the copy
3. If the copy has a better description or additional ops, merge those into the original
4. Re-run `dotnet build` to confirm no ID collision
5. Re-run the duplication guard tests to confirm warning count decreased

### Consolidating Same-Category Ops

If two tweaks in the same category write to the same registry key/value with different values:
```csharp
// BEFORE (two separate tweaks — functional duplicate)
new TweakDef { Id = "priv-disable-tracking-a", ApplyOps = [RegOp.SetDword(key, "Value", 0)] }
new TweakDef { Id = "priv-disable-tracking-b", ApplyOps = [RegOp.SetDword(key, "Value", 2)] }

// AFTER (one tweak with the correct semantic value)
new TweakDef { Id = "priv-disable-tracking", ApplyOps = [RegOp.SetDword(key, "Value", 2)] }
```

### When a Duplicate is Intentional

Add a `// INTENTIONAL DUPLICATE:` comment in BOTH tweaks explaining why:
```csharp
new TweakDef
{
    Id = "sec-disable-wer-policy",
    Label = "Disable Windows Error Reporting",
    Category = "Security",
    // INTENTIONAL DUPLICATE: crash module also sets this; security profile needs direct control
    ApplyOps = [RegOp.SetDword(@"HKLM\SOFTWARE\Policies\...", "Disabled", 1)],
}
```

---

## Automated Tests That Guard Against Duplication

| Test | File | What It Checks |
|------|------|---------------|
| `RegisterBuiltins_AllIdsUnique` | `TweakEngineBuiltinsTests.cs` | Zero duplicate tweak IDs |
| `RegisterBuiltins_DuplicateRegistryOps_BelowRegressionThreshold` | `TweakEngineBuiltinsTests.cs` | Total duplicate op targets ≤ threshold |
| `RegisterBuiltins_NoCrossModuleLabelAndPathCollision` | `TweakEngineBuiltinsTests.cs` | No cross-category duplicate (Label + RegistryKeys[0]) |
| `RegisterBuiltins_CategorySlugs_MatchKnownPrefixes` | `TweakEngineBuiltinsTests.cs` | ID prefixes match canonical category slugs |
| `DetectDuplicateRegistryOps_DuplicateTarget_ReturnsWarning` | `TweakValidatorTests.cs` | Unit test for the detection method |
| `DetectDuplicateRegistryOps_SameTweakMultipleOps_NoWarning` | `TweakValidatorTests.cs` | Intra-tweak ops are not false-positives |
| `DetectDuplicateRegistryOps_NoOverlap_NoWarning` | `TweakValidatorTests.cs` | Clean baseline returns empty |
| `ValidateTweaks_AllBuiltins_ReturnsNoErrors` | `TweakEngineBuiltinsTests.cs` | Full validation (IDs, labels, deps) passes |
