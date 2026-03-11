---
mode: agent
description: "Add a new registry tweak to a category module following workspace conventions"
---

# Add New Tweak

Add a new registry tweak to the RegiLattice engine.

## Input

Tweak Category: `${input:category}`
Tweak Description: `${input:description}`
Registry Path: `${input:registryPath}`

## Steps

1. **Read** the existing category module in `src/RegiLattice.Core/Tweaks/`
2. **Generate** a unique ID following the `{category_slug}-{descriptive-name}` pattern
3. **Add** a `TweakDef` with `ApplyOps`, `RemoveOps`, `DetectOps` using `RegOp` factories
4. **Set** `NeedsAdmin` (true for HKLM, false for HKCU-only)
5. **Set** `CorpSafe` (true for HKCU-only non-policy keys)
6. **Verify** ID uniqueness across all modules
7. **Build** and **test**: `dotnet build` + `dotnet test`

## Template

```csharp
new TweakDef
{
    Id = "{slug}-{name}",
    Label = "{Human Readable Name}",
    Category = "{Category}",
    Description = "{Description}. Default: X. Recommended: Y.",
    Tags = ["{tag1}", "{tag2}"],
    NeedsAdmin = true,
    CorpSafe = false,
    RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\..."],
    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "Value", 1)],
    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "Value")],
    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "Value", 1)],
},
```

## Validation Checklist

- [ ] ID is globally unique kebab-case
- [ ] Category matches an existing category
- [ ] `ApplyOps`, `RemoveOps`, `DetectOps` all populated
- [ ] `NeedsAdmin` and `CorpSafe` correctly set
- [ ] `RegistryKeys` lists all touched paths
- [ ] `dotnet build` succeeds
- [ ] `dotnet test` passes
