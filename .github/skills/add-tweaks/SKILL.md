---
name: add-tweaks
description: "Add registry tweaks to RegiLattice. Use when creating new TweakDef entries, adding to a category module, generating unique IDs, choosing RegOp factories, or registering a new category. Triggers on: 'add tweak', 'new tweak', 'registry tweak', 'create tweak', 'TweakDef'."
argument-hint: "Category and description of tweaks to add (e.g. 'WSL memory management')"
---

# Add Registry Tweaks — RegiLattice Skill

## When to Use

- Adding one or more `TweakDef` entries to a category module
- Creating a brand-new category module and registering it
- Choosing between `ApplyOps` (declarative) and `ApplyAction` (custom delegate) patterns

## Step-by-Step Process

1. **Identify** the target module: `src/RegiLattice.Core/Tweaks/<Category>.cs`
2. **Read** the file to understand the existing pattern and registered key constants
3. **Generate** a globally unique ID: `{category_slug}-{descriptive-name}` (kebab-case, ASCII only)
4. **Verify uniqueness** — search all `Tweaks/*.cs` for duplicates before writing
5. **Write** the `TweakDef` with `ApplyOps`, `RemoveOps`, `DetectOps` (declarative preferred)
6. **Set flags**: `NeedsAdmin = true` for HKLM; `CorpSafe = true` for HKCU non-policy tweaks
7. **Build**: `dotnet build src/RegiLattice.Core/RegiLattice.Core.csproj -c Debug`
8. **Test**: run the built-in test suite — all must pass

## TweakDef Template

```csharp
new TweakDef
{
    Id = "{slug}-{name}",
    Label = "{Human Readable Name}",
    Category = "{Category}",
    Description = "{What it does and why}. Default: {X}. Recommended: {Y}.",
    Tags = ["{tag1}", "{tag2}"],
    NeedsAdmin = true,
    CorpSafe = false,
    RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\..."],
    ApplyOps   = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "ValueName", 1)],
    RemoveOps  = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "ValueName")],
    DetectOps  = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "ValueName", 1)],
},
```

## RegOp Factory Reference

| Method                                     | Registry Type | Notes                     |
| ------------------------------------------ | ------------- | ------------------------- |
| `RegOp.SetDword(path, name, value)`        | REG_DWORD     | Most common               |
| `RegOp.SetString(path, name, value)`       | REG_SZ        | Plain string              |
| `RegOp.SetExpandString(path, name, value)` | REG_EXPAND_SZ | Paths with `%SystemRoot%` |
| `RegOp.SetQword(path, name, value)`        | REG_QWORD     | 64-bit value              |
| `RegOp.SetBinary(path, name, bytes)`       | REG_BINARY    | Raw bytes                 |
| `RegOp.SetMultiSz(path, name, strings)`    | REG_MULTI_SZ  | String array              |
| `RegOp.DeleteValue(path, name)`            | —             | Remove op                 |
| `RegOp.DeleteTree(path)`                   | —             | Remove entire key         |
| `RegOp.CheckDword(path, name, expected)`   | —             | Detect op                 |
| `RegOp.CheckString(path, name, expected)`  | —             | Detect op                 |
| `RegOp.CheckMissing(path, name)`           | —             | Detect value absent       |
| `RegOp.CheckKeyMissing(path)`              | —             | Detect key absent         |

## Critical Rules

- **Verbatim strings**: Always use `@"HKEY_..."` — never bare strings (escape sequence bug)
- **HasOperations gate**: `TweakEngine.Register()` silently skips tweaks with no `ApplyOps` and no `ApplyAction`
- **Unique IDs across ALL 170 modules** — duplicates throw `ArgumentException` at startup
- **Registry path format**: Full hive names preferred (`HKEY_LOCAL_MACHINE\...`); abbreviations (`HKLM\...`) also accepted
- **Duplicate scan before writing**:
    ```powershell
    Select-String -Pattern '"my-new-id"' -Path src/RegiLattice.Core/Tweaks/*.cs
    ```

## Category Slug Reference

| Slug      | Category        |     | Slug       | Category    |
| --------- | --------------- | --- | ---------- | ----------- |
| `priv`    | Privacy         |     | `perf`     | Performance |
| `game`    | Gaming          |     | `wsl`      | WSL         |
| `sec`     | Security        |     | `svc`      | Services    |
| `schtask` | Scheduled Tasks |     | `net`      | Network     |
| `power`   | Power           |     | `explorer` | Explorer    |

Full slug table in `.github/copilot-instructions.md`.

## Post-Add Quality Gate

After adding tweaks, verify all quality gates pass before committing:

```powershell
# 1. Build — must produce 0 fatals, 0 errors, 0 warnings
dotnet build src/RegiLattice.Core/RegiLattice.Core.csproj -c Debug

# 2. Run Core tests — must be 0 failures, 0 skipped
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s

# 3. Check for duplicate IDs (catches new duplicates the build won't)
Select-String -Pattern 'Id = "' -Path src/RegiLattice.Core/Tweaks/*.cs |
    ForEach-Object { ($_ -replace '.*Id = "([^"]+)".*','$1') } |
    Group-Object | Where-Object Count -gt 1
```

The build **will throw** `ArgumentException` at registration if any ID is duplicated — but the
duplicate ID check above catches conflicts before the build does.
