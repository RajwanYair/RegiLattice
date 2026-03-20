---
name: search-tweaks
description: "Find tweaks in RegiLattice by keyword, tag, registry path, category, or scope. Use when asked to locate a specific tweak, audit a category, check for duplicates, or explore what tweaks exist for a feature area. Triggers on: 'find tweak', 'search tweaks', 'which tweak', 'list tweaks', 'registry path', 'tweak for'."
argument-hint: "Search term, tag, registry path, or category name"
---

# Search Tweaks — RegiLattice

## Quick Search Commands

```powershell
# Search by keyword (label, description, tags)
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern "keyword" -Context 2,2

# Search by registry path or value name
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern "AllowTelemetry" -Context 4,4

# Search by tweak ID prefix (category slug)
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'Id = "priv-'

# List all category module files
Get-ChildItem "src\RegiLattice.Core\Tweaks\*.cs" | Select-Object -ExpandProperty BaseName | Sort-Object

# Count tweaks per module
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'new TweakDef' |
  Group-Object Filename | Sort-Object Count -Descending | Select-Object -First 20
```

## TweakEngine Runtime API

```csharp
// Full-text search: ID + Label + Category + Description + Tags
engine.Search("telemetry");

// Multi-criteria filter
engine.Filter(
    corpSafe: true,      // corporate-safe tweaks only
    needsAdmin: false,   // user-scope tweaks only
    scope: TweakScope.User,
    category: "Privacy",
    minBuild: 22000,     // Windows 11
    query: "disable"
);

// By category
engine.TweaksByCategory("WSL");

// By tag
engine.TweaksByTag("gaming");

// By scope
engine.TweaksByScope(TweakScope.Machine);
```

## Scope Reference

| Scope | Registry Hive | Description |
|-------|--------------|-------------|
| `User` | `HKCU\...` | Current user only |
| `Machine` | `HKLM\...` | All users / system |
| `Both` | Mixed | Some ops on each hive |

## Finding a Tweak for a Registry Key

1. Search for the value name in Tweaks/*.cs
2. Also search for the key path fragment
3. If not found, it may not yet be implemented → use the `add-tweaks` skill

## Category Slug → Module File Mapping

Each slug maps to a file in `src/RegiLattice.Core/Tweaks/`:

| Slug | File |
|------|------|
| `priv` | `Privacy.cs` |
| `perf` | `Performance.cs` |
| `game` | `Gaming.cs` |
| `wsl` | `Wsl.cs` |
| `svc` | `Services.cs` |
| `schtask` | `ScheduledTasks.cs` |
| `net` | `Network.cs` |
| `sec` | `Security.cs` |
| `explorer` | `Explorer.cs` |
| `power` | `Power.cs` |

Full table in `.github/copilot-instructions.md`.

## Duplicate Detection

```powershell
# Find duplicate IDs
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'Id = "' |
  ForEach-Object { ($_ -replace '.*Id = "([^"]+)".*','$1') } |
  Group-Object | Where-Object Count -gt 1 | Select-Object Name, Count

# Find duplicate registry key+value combinations
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'SetDword|SetString' |
  ForEach-Object { ($_ -split '"')[1..3] -join '"' } |
  Group-Object | Where-Object Count -gt 1
```
