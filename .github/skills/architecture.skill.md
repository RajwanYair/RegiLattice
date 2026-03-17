---
mode: agent
description: "Explore the RegiLattice architecture, dependencies, and data flow"
tools: ["read_file", "grep_search", "semantic_search", "list_dir"]
---

# Architecture Explorer — RegiLattice Skill

You are an expert at navigating and explaining the RegiLattice codebase architecture.

## Solution Map

```
RegiLattice.sln
├── src/RegiLattice.Core/       # Class library — zero UI dependency
│   ├── TweakEngine.cs          # Central engine (register, search, filter, apply, profiles)
│   ├── Models/                 # TweakDef, ProfileDef, RegOp, TweakScope, CategoryIcons
│   ├── Registry/               # RegistrySession — all Win32 registry I/O
│   ├── Services/               # AppConfig, Analytics, CorporateGuard, Elevation,
│   │                           #   HardwareInfo, Locale, Ratings
│   └── Tweaks/                 # 90 modules × ~29 tweaks each = 2,610 tweaks
├── src/RegiLattice.GUI/        # WinForms app (depends on Core)
│   ├── Theme.cs                # 4-theme engine
│   ├── Forms/                  # MainForm, AboutDialog, 5 package manager dialogs,
│   │                           #   ToolVersionsDialog
│   └── PackageManagers/        # ShellRunner, ScoopMgr, WinGetMgr, PipMgr,
│                               #   ChocolateyMgr, PSModuleMgr, ToolVersionChecker
├── src/RegiLattice.CLI/        # Console app (depends on Core)
│   └── Program.cs              # 25+ commands via args parsing
└── tests/                      # xUnit: 888 Core + 116 CLI + 195 GUI = 1,199+ tests
```

## Data Flow

1. `TweakEngine.RegisterBuiltins()` loads all 90 category modules
2. Each module provides `static IReadOnlyList<TweakDef> Tweaks` property
3. Engine indexes by ID, Category, Scope; builds search index
4. GUI/CLI calls `Search()`, `Filter()`, `DetectStatus()`, `Apply()`, `Remove()`
5. All registry I/O goes through `RegistrySession` (DryRun support + backup)

## Key Design Decisions

- **Declarative tweaks** (95%): pure data, no custom code — `ApplyOps/RemoveOps/DetectOps`
- **Custom tweaks** (5%): use `ApplyAction/RemoveAction/DetectAction` delegates
- **Immutable models**: `TweakDef` sealed with `required init`, `RegOp` immutable
- **Sealed by default**: ALL classes sealed unless inheritance is explicit
- **Zero raw registry access**: Everything via `RegistrySession` wrapper
- **Only 2 P/Invoke calls**: `GetComputerNameExW` + `GlobalMemoryStatusEx`

## Investigation Commands

```powershell
# Count tweaks per category
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'new TweakDef' | Group-Object Filename | Sort-Object Count -Descending

# Find all registry paths used
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'HKEY_' | Select-Object -First 20

# Check for duplicate IDs
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'Id = "' | ForEach-Object { $_ -replace '.*Id = "([^"]+)".*','$1' } | Group-Object | Where-Object Count -gt 1
```
