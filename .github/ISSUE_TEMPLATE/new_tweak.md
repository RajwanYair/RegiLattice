---
name: New Tweak Proposal
about: Propose a new registry tweak to add
title: "[Tweak] "
labels: tweak, enhancement
assignees: ''
---

## Tweak Details

- **ID (kebab-case):** `category-disable-feature`  (prefix must match a slug in `copilot-instructions.md`)
- **Label:** Disable Feature
- **Category:** (must be one of the 72 existing categories, or propose a new one)
- **Needs admin:** Yes / No  (HKLM = Yes, HKCU-only = No)
- **Corp safe:** Yes / No  (HKCU-only and non-policy = Yes)
- **Min build:** 0 (or specific Windows build, e.g. 22000 for Windows 11)
- **Source URL:** <https://learn.microsoft.com/>...  (KB article or doc reference)

## Registry Keys

```
HKEY_LOCAL_MACHINE\SOFTWARE\Policies\...
  ValueName (REG_DWORD) = 1    # applied state
  (absent or 0)                # default / removed state
```

## What It Does

Description of what the tweak changes and why a user would want it.
Include `Default: 0. Recommended: 1.` in the description field.

## Apply Logic

```csharp
ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\...", "ValueName", 1)]
```

## Remove Logic (revert to default)

```csharp
RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\...", "ValueName")]
```

## Detect Logic

```csharp
DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\...", "ValueName", 1)]
```

## References

- Microsoft docs link
- Related Group Policy Object (GPO) setting name
- Windows version and build requirements

## Checklist

- [ ] Registry path verified on a clean Windows install
- [ ] Tested apply + remove manually (`reg add` / `reg delete` or `regedit`)
- [ ] Tweak is reversible — remove restores exact default state
- [ ] ID is globally unique: `dotnet run --project src/RegiLattice.CLI -- --list` shows no conflicts
- [ ] Tests pass: `dotnet test`
- [ ] Build clean: `dotnet build RegiLattice.sln -c Release` with no warnings
- [ ] Validate passes: `dotnet run --project src/RegiLattice.CLI -- --validate`
