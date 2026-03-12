---
mode: agent
description: "Add registry tweaks to an existing or new category module"
tools: ["read_file", "replace_string_in_file", "create_file", "grep_search", "semantic_search", "runTests"]
---

# Add Registry Tweaks — RegiLattice Skill

You are an expert at adding new Windows registry tweaks to RegiLattice.

## Context

- RegiLattice is a C# 13 / .NET 10.0-windows registry tweak toolkit
- 1,981+ tweaks across 72 categories in `src/RegiLattice.Core/Tweaks/`
- Each tweak is a `TweakDef` with declarative `RegOp` operations

## Process

1. **Identify** the target category module under `src/RegiLattice.Core/Tweaks/`
2. **Read** the existing file to understand the pattern
3. **Generate** a unique ID: `{category_slug}-{descriptive-name}` (kebab-case)
4. **Verify uniqueness** by searching across all Tweaks/*.cs files
5. **Add** the TweakDef with all 3 op lists: ApplyOps, RemoveOps, DetectOps
6. **Set flags**: `NeedsAdmin` (true for HKLM), `CorpSafe` (true for HKCU non-policy)
7. **Build**: `dotnet build RegiLattice.sln`
8. **Test**: `dotnet test`

## TweakDef Template

```csharp
new TweakDef
{
    Id = "{slug}-{name}",
    Label = "{Human Readable Name}",
    Category = "{Category}",
    Description = "{What it does}. Default: X. Recommended: Y.",
    Tags = ["{tag1}", "{tag2}"],
    NeedsAdmin = true,
    CorpSafe = false,
    RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\..."],
    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "ValueName", 1)],
    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "ValueName")],
    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "ValueName", 1)],
},
```

## RegOp Factory Methods Available

| Method | Registry Type |
|--------|--------------|
| `RegOp.SetDword(path, name, value)` | REG_DWORD |
| `RegOp.SetString(path, name, value)` | REG_SZ |
| `RegOp.SetExpandString(path, name, value)` | REG_EXPAND_SZ |
| `RegOp.SetQword(path, name, value)` | REG_QWORD |
| `RegOp.SetBinary(path, name, bytes)` | REG_BINARY |
| `RegOp.SetMultiSz(path, name, strings)` | REG_MULTI_SZ |
| `RegOp.DeleteValue(path, name)` | Delete value |
| `RegOp.DeleteTree(path)` | Delete key tree |
| `RegOp.CheckDword(path, name, expected)` | Detect DWORD |
| `RegOp.CheckString(path, name, expected)` | Detect string |
| `RegOp.CheckMissing(path, name)` | Detect value absent |
| `RegOp.CheckKeyMissing(path)` | Detect key absent |

## Category Slug Reference

See `.github/copilot-instructions.md` for the full slug-to-category mapping table.

## Rules

- Every ID must be **globally unique** across all 72 modules
- Use full hive names: `HKEY_LOCAL_MACHINE\...` or `HKEY_CURRENT_USER\...`
- All classes are `sealed`, all collections are `IReadOnlyList<T>`
- Use C# 13 collection expressions `[]` not `new List<T>()`
