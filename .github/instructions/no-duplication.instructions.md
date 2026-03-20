---
applyTo: "**/*.cs,**/Tweaks/**,**/tests/**"
---

# No-Duplication Rules — RegiLattice

> Prevention is cheaper than remediation.
> Apply these rules when adding tweaks, reviewing code, or before every commit.

## The 4 Layers of Duplication to Prevent

### Layer 1 — ID Duplication (HARD BLOCK)

**Rule**: Every tweak `Id` must be globally unique across all 94+ modules.

`TweakEngine.Register()` throws `ArgumentException` on duplicate IDs — this is a
compile-time-equivalent guard. A duplicate ID is always a bug.

**Pre-commit check** (run before writing any new tweak):
```powershell
# Search all tweak modules for an ID before using it
Select-String -Pattern '"new-tweak-id"' -Path src/RegiLattice.Core/Tweaks/*.cs
```

**Automated gate**: `TweakEngineBuiltinsTests.RegisterBuiltins_AllIdsUnique` — must be green.

---

### Layer 2 — Functional / Ops Duplication (WARN → JUSTIFY OR REMOVE)

**Rule**: No two `TweakDef` entries should write to the same registry `PATH\ValueName`
(via `ApplyOps`). Writing the same value from two different tweaks means one of them:
- Is in the wrong module (misfiled)
- Is a redundant tweak that should be removed
- Is a semantic duplicate with a different category claim

`TweakValidator.DetectDuplicateRegistryOps()` finds all such cases.

**Pre-commit check** (run after writing new tweaks):
```powershell
# Quick scan for literal duplicates in source files (catches most cases)
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'SetDword|SetString|SetExpandString|SetQword' |
    ForEach-Object {
        if ($_ -match '"(HKEY[^"]+|HKCU[^"]+|HKLM[^"]+)"') {
            $path = $Matches[1]
            if ($_ -match '",\s*"([^"]+)"') { "$path\$($Matches[1])" }
        }
    } | Group-Object | Where-Object Count -gt 1 | Select-Object -First 20 Count, Name
```

**Automated gate**: `TweakEngineBuiltinsTests.RegisterBuiltins_DuplicateRegistryOps_BelowRegressionThreshold`
— the count must not exceed the current threshold. If your change increases the count, justify or remove the dupe.

**Acceptable exceptions** (document with `// INTENTIONAL DUPLICATE:` comment):
- Same registry key/value but semantically different context (e.g., telemetry key set by both
  a "minimal" tweak and a "privacy hardening" tweak as different user journeys)
- Cross-category coordination where both tweaks need to ensure the value is set (rare)

---

### Layer 3 — Label Duplication (CONTEXTUAL)

**Rule**: Identical `Label` strings across different modules are a smell but not always a bug.
Same label + same registry path = almost certainly a true duplicate (must be removed).
Same label + different registry paths = may be acceptable (same feature, different mechanism).

**Pre-commit check**:
```powershell
# Find tweaks with duplicate labels
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'Label = "' |
    ForEach-Object { ($_ -split '"')[1] } |
    Group-Object | Where-Object Count -gt 1 |
    Sort-Object Count -Descending | Select-Object -First 20 Count, Name
```

**Automated gate**: `TweakEngineBuiltinsTests.RegisterBuiltins_NoCrossModuleLabelAndPathCollision`
— same (Label + RegistryKeys[0]) across multiple categories = fail.

---

### Layer 4 — Cross-Module Conceptual Duplication

**Rule**: Two tweaks that achieve the same user-visible outcome through different registry
paths should be merged into one tweak with multiple `ApplyOps`. Don't split the same
function across two module files just because different registry paths are involved.

**Detection approach**:
- Same `Label`, same `Category`, different registry keys → likely should be one tweak
- Same category slug prefix, very similar description → review for consolidation
- Run `scripts/Audit-Duplications.ps1` for a comprehensive report

---

## Pre-Commit Duplication Checklist

Before committing any change to `src/RegiLattice.Core/Tweaks/`:

```
☐ Searched for the new tweak ID in all Tweaks/*.cs files — no match found
☐ Searched for the registry PATH\ValueName in all Tweaks/*.cs files — no match found
☐ Checked that the Label is unique OR that any collision is intentional (same label, different mechanism)
☐ Ran: dotnet build (0 errors — RegisterBuiltins would throw on duplicate IDs)
☐ Ran: dotnet test tests/RegiLattice.Core.Tests — all duplication guard tests green
```

---

## Running the Full Audit

For a comprehensive duplication report across all layers:

```powershell
. .\scripts\Audit-Duplications.ps1
```

This script checks:
1. Duplicate tweak IDs (hard error)
2. Duplicate registry operations (Path\ValueName written by 2+ tweaks)
3. Duplicate labels (same human-readable name in multiple modules)
4. Duplicate instruction/skill files in `.github/`

---

## Fixing Duplicates — Decision Tree

```
Found a duplicate registry op?
├── Same category, same label → REMOVE the newer one (keep the original)
├── Same category, different label → MERGE ApplyOps into one tweak
├── Different categories, same label → Check if both are needed by different profiles
│   ├── Both needed → add "// INTENTIONAL DUPLICATE: needed for {reason}" comment
│   └── Only one needed → REMOVE the one with the wrong category
└── Different categories, different labels → audit semantics; usually remove one
```

---

## What NOT to Do

- Don't add `// duplicate of X` comments and leave the duplicate — remove it
- Don't rename an ID to avoid a collision if the tweak is functionally the same — remove it
- Don't split a multi-registry-path tweak into two separate tweaks — use multiple `ApplyOps`
- Don't commit without running the pre-commit checklist above
