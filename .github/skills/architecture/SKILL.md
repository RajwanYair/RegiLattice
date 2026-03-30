---
name: architecture
description: "Explore RegiLattice codebase architecture, data flow, dependencies, and design decisions. Use when asked to explain how the system works, trace a feature end-to-end, understand module structure, or find where specific logic lives. Triggers on: 'how does', 'where is', 'explain', 'architecture', 'data flow', 'TweakEngine', 'RegistrySession'."
argument-hint: "What to explore (e.g. 'how tweaks are applied', 'where profiles are defined')"
---

# Architecture Explorer — RegiLattice

## Solution Map

```
RegiLattice.sln
├── src/RegiLattice.Core/               # Class library — zero UI dependency
│   ├── TweakEngine.cs                  # Central engine (register, search, filter, apply, profiles)
│   ├── SnapshotManager.cs              # Save/load/restore state snapshots
│   ├── TweakValidator.cs               # Integrity check + circular dependency detection
│   ├── DependencyResolver.cs           # Topological sort for DependsOn
│   ├── Models/TweakDef.cs              # Immutable tweak definition + RegOp + TweakScope
│   ├── Models/ProfileDefinitions.cs    # 5 built-in profiles (gaming, privacy, business, …)
│   ├── Registry/RegistrySession.cs     # All Win32 registry I/O (DryRun + backup)
│   ├── Services/                       # Analytics, AppConfig, CorporateGuard, Elevation,
│   │                                   #   Favorites, HardwareInfo, Locale, Ratings,
│   │                                   #   ShellRunner, SystemMonitor, TweakHistory
│   ├── Plugins/                        # Tweak Pack marketplace (JSON packs)
│   └── Tweaks/                         # 660 module files, 9,184 tweaks across 632 categories
├── src/RegiLattice.GUI/                # WinForms app (depends on Core)
│   ├── Theme.cs                        # 11-theme engine (ThemeDef records)
│   ├── Forms/MainForm.cs               # Main window — categories, search, tray, log panel
│   ├── Forms/BasePackageManagerDialog  # Abstract template for 5 package manager dialogs
│   └── PackageManagers/                # WinGet, Scoop, pip, Chocolatey, PSModule wrappers
├── src/RegiLattice.CLI/                # Console app (depends on Core)
│   ├── Program.cs                      # 25+ commands via args parsing
│   └── CliArgs.cs                      # Extracted CLI argument model
└── tests/                              # 2,102+ Core + 301+ CLI + 339+ GUI = 2,742 tests
```

## Data Flow — Tweak Lifecycle

```
RegisterBuiltins()          TweakEngine indexes by ID/Category/Scope
      │
      ▼
Search() / Filter()         Returns IReadOnlyList<TweakDef>
      │
      ▼
DetectStatus(td)            RegistrySession.Evaluate(DetectOps) → Applied / NotApplied
      │
      ▼
Apply(id) / Remove(id)      RegistrySession.Execute(ApplyOps) → DryRun or real write
      │
      ▼
TweakHistory.Record()       Append entry to %LOCALAPPDATA%\RegiLattice\history.json
```

## Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| Declarative tweaks (95%) | Pure data — no code needed for registry-only tweaks |
| Immutable `TweakDef` (`required init`) | Thread-safe, can't be mutated after construction |
| All registry I/O via `RegistrySession` | DryRun, backup, structured logging, testability |
| Sealed classes everywhere | JIT devirtualisation + prevent unintended subclassing |
| Only 4 P/Invoke calls | `GetComputerNameExW`, `GlobalMemoryStatusEx` ×2, `GetSystemTimes` |
| `IReadOnlyList<T>` for all public collections | Consumers can't mutate engine state |

## Investigation Commands

```powershell
# Count tweaks per module
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'new TweakDef' |
  Group-Object Filename | Sort-Object Count -Descending

# Find all registry paths
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'HKEY_' | Select-Object -First 30

# Detect duplicate IDs
Select-String -Path "src\RegiLattice.Core\Tweaks\*.cs" -Pattern 'Id = "' |
  ForEach-Object { ($_ -replace '.*Id = "([^"]+)".*','$1') } |
  Group-Object | Where-Object Count -gt 1

# Check category module list
Get-ChildItem "src\RegiLattice.Core\Tweaks\*.cs" | Select-Object -ExpandProperty BaseName
```

## Entry Points for Common Questions

| Question | Starting File |
|----------|--------------|
| How is a tweak applied? | `TweakEngine.cs` → `Apply()` → `RegistrySession.Execute()` |
| Where are profiles? | `Models/ProfileDefinitions.cs`, `Models/ProfileDef.cs` |
| How is the GUI built? | `Forms/MainForm.cs` → `BuildCategoryPanels()` |
| How does search work? | `TweakEngine.cs` → `Search()` using pre-built `_searchPairs` |
| What is CorpSafe? | `Services/CorporateGuard.cs` → P/Invoke + WMI domain checks |
| How are themes applied? | `Theme.cs` → `SetTheme()` → `ApplyTheme(control)` |
