# RegiLattice — Architecture

> Deep-dive into data flow, dependency graph, and design decisions.
> Last verified: 2025-07-22 (v3.2.0, 2 316 tweaks, 89 categories, 641 tests).
> C# 13 / .NET 10.0-windows (x64).

---

## 1  High-Level Data Flow

```
                      User
                       |
            +----------+-----------+
            |          |           |
          CLI        Menu        GUI
     (Program.cs) (interactive) (MainForm.cs)
            |          |           |
            +----------+-----------+
                       |
                 TweakEngine.cs        ← central tweak manager
                       |
            +----------+-----------+
            |                      |
       TweakDef list         ProfileDefinitions
       (2 316 tweaks)        (5 profiles)
            |
       Tweaks/*.cs                 ← 90 category modules (89 categories)
            |
       +----+----+
       |         |
RegistrySession  CorporateGuard
       |              |
Microsoft.Win32   P/Invoke + WMI
  .Registry      (GetComputerNameExW,
                  System.Management)
```

## 2  Project Dependency Graph

```
RegiLattice.sln
│
├── src/RegiLattice.Core/               ← Class library (no UI dependencies)
│     ├── TweakEngine.cs                ← register, search, filter, apply, profiles
│     ├── Models/
│     │     ├── TweakDef.cs             ← immutable tweak definition + RegOp + TweakScope
│     │     ├── ProfileDef.cs           ← profile record type
│     │     └── ProfileDefinitions.cs   ← 5 hardcoded profiles
│     ├── Registry/
│     │     └── RegistrySession.cs      ← registry read/write/backup/execute wrapper
│     ├── Services/
│     │     ├── Analytics.cs            ← local usage analytics
│     │     ├── AppConfig.cs            ← configuration management
│     │     ├── CorporateGuard.cs       ← corp network detection (P/Invoke + WMI)
│     │     ├── Elevation.cs            ← UAC elevation helpers
│     │     ├── HardwareInfo.cs         ← hardware detection + profile suggestion
│     │     ├── Locale.cs               ← i18n string table (en + de)
│     │     └── Ratings.cs              ← tweak rating system (1-5 stars)
│     ├── Plugins/                        ← Tweak Pack system (JSON marketplace)
│     └── Tweaks/                       ← 90 category modules, 2 316 tweaks
│           ├── Accessibility.cs
│           ├── Performance.cs
│           ├── Privacy.cs
│           └── ... (87 more)
│
├── src/RegiLattice.GUI/ ──────────────► depends on RegiLattice.Core
│     ├── Program.cs                    ← WinForms entry point
│     ├── Theme.cs                      ← 4-theme engine (ThemeDef record)
│     └── Forms/
│           ├── MainForm.cs             ← main window (categories, search, filters)
│           ├── AboutDialog.cs          ← about + hardware info
│           ├── ScoopManagerDialog.cs   ← Scoop package manager
│           └── PSModuleManagerDialog.cs
│
├── src/RegiLattice.CLI/ ──────────────► depends on RegiLattice.Core
│     └── Program.cs                    ← 25+ commands via args parsing
│
├── tests/RegiLattice.Core.Tests/ ─────► depends on RegiLattice.Core
│     ├── TweakDefTests.cs              ← model, RegOp factories, scope computation
│     ├── TweakEngineTests.cs           ← engine registration, search, profiles, validation, deps
│     ├── RegistrySessionTests.cs       ← session helpers, dry-run, path parsing
│     ├── ServicesTests.cs              ← Analytics, Config, CorporateGuard, etc.
│     └── PluginTests.cs               ← PackLoader, PackManager, PackIndex, Locale
│
├── tests/RegiLattice.CLI.Tests/ ─────► depends on RegiLattice.CLI + Core
│     └── ParseArgsTests.cs            ← CLI argument parsing, --depends-on, --no-color
│
└── tests/RegiLattice.GUI.Tests/ ──────► depends on RegiLattice.GUI + Core
      ├── ThemeTests.cs                 ← theme switching, colour attributes
      └── PackageManagerValidationTests.cs
```

**Key rule:** No circular references. GUI and CLI depend on Core. Core has zero
project references — only one NuGet dependency (`System.Management 9.0.3`).

## 3  Tweak Registration Sequence

```
Application starts
  │
  ▼
TweakEngine.RegisterBuiltins()
  │
  ▼
For each category module (90 modules):
  │
  ├──► Module exposes: public static IReadOnlyList<TweakDef> Tweaks { get; }
  │
  ├──► TweakEngine.Register(tweaks) iterates each TweakDef:
  │       │
  │       ├──► Checks HasOperations (ApplyOps/ApplyAction defined)
  │       │       Skip silently if no-op stub
  │       │
  │       ├──► Checks Id uniqueness (throws ArgumentException on duplicate)
  │       │
  │       └──► Indexes into _allTweaks, _tweakIndex, _tweaksByCategory
  │
  ▼
Ready — AllTweaks() returns the immutable sorted list
```

## 4  Tweak Execution Flow

### Apply

```
User picks "apply <id>"
  │
  ▼
TweakEngine.GetTweak(id) → TweakDef?
  │
  ▼
CorporateGuard.IsCorporateNetwork() check
  │  CorpSafe=false + corporate detected → blocked (unless --force)
  │
  ▼
RegistrySession.Backup(keys, label)     ← JSON backup to %LOCALAPPDATA%
  │
  ▼
Declarative path (95%):                 Custom path (5%):
  RegistrySession.Execute(ApplyOps)       td.ApplyAction(dryRun)
  │  SetDword / SetString / DeleteValue     │  custom logic
  │  respects DryRun mode                   │  may use RegistrySession
  │                                         │
  ▼                                         ▼
TweakResult.Applied / Error             TweakResult.Applied / Error
```

### Detect

```
GUI refresh / TweakEngine.StatusMap(parallel: true)
  │
  ▼
For each TweakDef (parallel via Task.Run):
  │
  ▼
Declarative path:                       Custom path:
  RegistrySession.Evaluate(DetectOps)     td.DetectAction()
  │  CheckDword / CheckString / etc.      │  returns bool
  │  All ops must pass → true             │
  │                                       │
  ▼                                       ▼
TweakResult.Applied / NotApplied / Unknown
```

## 5  GUI Architecture

```
MainForm (Forms/MainForm.cs)
  │
  ├── Toolbar
  │     ├── Search TextBox (incremental filter)
  │     ├── Status filter ComboBox (All/Applied/Default/Unknown)
  │     ├── Scope filter ComboBox (All/User/Machine/Both)
  │     ├── Profile selector ComboBox (Business/Gaming/Privacy/Minimal/Server)
  │     ├── Theme selector ComboBox (Catppuccin Mocha/Latte, Nord, Dracula)
  │     └── Force checkbox (bypass corporate guard)
  │
  ├── Scrollable panel (double-buffered, SuspendLayout for bulk updates)
  │     ├── CategorySection (collapsible)
  │     │     ├── Header: arrow + title + count + scope/profile badges
  │     │     └── TweakRow instances:
  │     │           ├── Status indicator (Applied/Default/Unknown)
  │     │           ├── Checkbox (batch selection)
  │     │           ├── Toggle button (individual enable/disable)
  │     │           └── Badges: SCOPE (User/Machine/Both), ADMIN, CORP
  │     └── ... (89 category sections, 90 modules)
  │
  ├── Action bar: Apply/Remove Selected, Snapshot Save/Restore, Export
  ├── Summary stats bar: Applied / Default / Unknown counts
  └── Progress bar + status label
```

**Threading model:**
- Heavy operations (`StatusMap`, `ApplyBatch`) run via `Task.Run`
- UI updates dispatched via `Invoke` / `BeginInvoke` (WinForms thread safety)
- Status refresh uses `StatusMap(parallel: true)` for bulk detection

## 6  Theme System

```csharp
public record ThemeDef(
    string Name,
    Color Background, Color Surface, Color Overlay,
    Color Foreground, Color SubText,
    Color Accent, Color Success, Color Warning, Color Error,
    Color HeaderBg, Color HeaderFg,
    Color RowBg, Color RowAltBg, Color RowHoverBg,
    Color ButtonBg, Color ButtonFg, Color ButtonHoverBg,
    Color InputBg, Color InputFg, Color InputBorder,
    Color ScrollBg, Color ScrollThumb
);
```

4 themes: Catppuccin Mocha (default), Catppuccin Latte, Nord, Dracula.
`Theme.SetTheme(name)` updates all form controls at runtime.

## 7  Profile System

```csharp
public record ProfileDef(
    string Name,
    string Description,
    IReadOnlyList<string> ApplyCategories,
    IReadOnlyList<string> SkipCategories
);
```

| Profile    | Categories | Description                                     |
| ---------- | ---------- | ----------------------------------------------- |
| `business` | 39         | Productivity, security, cloud & workflow        |
| `gaming`   | 31         | GPU, performance, low-latency, distraction-free |
| `privacy`  | 31         | Telemetry, tracking, cloud & browser data       |
| `minimal`  | 22         | Fast, clean system essentials                   |
| `server`   | 28         | Hardened, headless, uptime & remote mgmt        |

`TweakEngine.ApplyProfile(name)` iterates `TweaksForProfile(name)` and calls
`Apply()` for each, respecting corporate guard flags.

## 8  Corporate Guard Decision Tree

```
CorporateGuard.IsCorporateNetwork()
  │
  ├──► _isDomainJoined()              AD domain (GetComputerNameExW P/Invoke)
  │       True? → return True
  │
  ├──► _isAzureAdJoined()             Azure AD / Entra ID registry check
  │       True? → return True
  │
  ├──► _hasGpoIndicators()            HKLM\...\Policies registry check
  │       True? → return True
  │
  ├──► _isManagedDevice()             SCCM/Intune WMI query (System.Management)
  │       True? → return True
  │
  └──► return False                   Not corporate
```

When corporate is detected and a tweak has `CorpSafe = false`:
- CLI: prints error message
- GUI: grays out the tweak row
- Override: `--force` CLI flag or Force checkbox in GUI

## 9  Registry Session Internals

```
RegistrySession
  │
  ├── DryRun : bool                   ← When true, captures ops without writing
  ├── DryOps : List<RegOp>            ← Captured ops during dry-run
  ├── Log : List<string>              ← Structured log entries
  │
  ├── Execute(IReadOnlyList<RegOp>)   ← Write: processes SetDword, DeleteValue, etc.
  ├── Evaluate(IReadOnlyList<RegOp>)  ← Read: processes CheckDword, CheckString, etc.
  │
  ├── Backup(keys, label)             ← JSON backup to %LOCALAPPDATA%\RegiLattice\backups\
  │
  ├── Write methods:
  │     SetDword, SetString, SetExpandString, SetQword,
  │     SetBinary, SetMultiSz, SetValue, DeleteValue, DeleteTree
  │
  ├── Read methods:
  │     ReadDword, ReadString, ReadQword, ReadBinary,
  │     ReadMultiSz, ReadValue
  │
  └── Check methods:
        KeyExists, ValueExists, ListSubKeys, ListValueNames
```

All registry access flows through `RegistrySession`. Direct `Microsoft.Win32.Registry`
calls are never used elsewhere. This enables:
- **DryRun mode**: preview all changes without writes
- **JSON backups**: automatic before destructive operations
- **Structured logging**: every operation recorded
- **Testability**: `DryRun = true` in unit tests

## 10  Configuration Hierarchy

Priority order (highest → lowest):

1. **Command-line arguments** — `--dry-run`, `--force`, `--config path`
2. **Environment variables** — `REGILATTICE_*`
3. **User config file** — `%LOCALAPPDATA%\RegiLattice\config.json`
4. **Default values** — hardcoded in `AppConfig`

`AppConfig.Load()` merges all layers. Properties include:
- `Theme` — UI theme name
- `Locale` — i18n locale
- `ForceCorpGuard` — bypass corporate guard
- `DryRun` — preview mode

## 11  Design Decisions

| Decision | Rationale |
|---|---|
| **Sealed classes by default** | JIT devirtualisation; prevents unintended inheritance |
| **Immutable TweakDef** | Single source of truth; enables safe parallel detection, GUI rendering, CLI listing |
| **Declarative RegOp pattern** | 95% of tweaks need zero custom code; just `ApplyOps`/`RemoveOps`/`DetectOps` |
| **RegistrySession wrapper** | DryRun mode, JSON backups, structured logging, testability |
| **DryRun mode** | Tests validate tweak logic without touching the real registry |
| **Corporate guard** | Prevents accidental damage on managed machines; legal/compliance safety |
| **Catppuccin Mocha + 3 themes** | Modern dark theme with switchable alternatives (Latte, Nord, Dracula) |
| **Parallel status detection** | `StatusMap(parallel: true)` via Task.Run for fast GUI refresh across 2 316 tweaks |
| **IReadOnlyList everywhere** | Immutable public contracts; prevents caller mutation |
| **Collection expressions** | C# 13 `[]` syntax for concise, readable tweak definitions |
| **SuspendLayout pattern** | O(1) layout recalculation during bulk control updates |
| **Task.Run for heavy work** | Keeps WinForms UI responsive during batch operations |
| **JSON backup before write** | Every mutation is preceded by JSON export; enables programmatic rollback |
| **Only 2 P/Invoke calls** | Minimise unsafe code surface; prefer managed APIs |
| **frozenset profiles** | Immutable category sets; hashable for caching |
