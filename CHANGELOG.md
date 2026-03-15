# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [3.2.1] — 2026-03-15

### Sprint 11 — GUI Polish, Package Manager Fixes & Documentation Refresh

#### Changed

- **ShellRunner default timeout** increased from 10 s → 30 s — fixes package managers
  that failed to list installed packages due to timeout (winget list, pip list, scoop list)
- **Explicit longer timeouts** for slow list commands: winget list/upgrade (60 s),
  scoop list/status (30 s), pip list (30 s), PowerShell Get-Module (30 s)
- **Scoop detection** now falls back to PATH check when `~/scoop/shims/scoop.ps1`
  doesn't exist (non-default Scoop installations)
- **GUI log panel** now visible by default (was hidden)

#### Added

- **Menu item icons** — all 8 Tools menu entries now show 16×16 tool-specific icons
  (Scoop, PowerShell, pip, WinGet, Chocolatey, Tool Versions, Windows Health, Marketplace)
- **MarketplaceIcon** — new purple shopping-bag icon for the Tweak Pack Marketplace dialog
- **AppIcons.MenuBitmap()** — generates 16×16 bitmaps for ToolStripMenuItem images

#### Documentation

- Updated `copilot-instructions.md`: full TweakDef model (all fields), TweakKind table
  (8 variants with fields used), TweakResult table (7 outcomes), GUI details
- Updated `workspace.instructions.md`: TweakKind table with fields used per kind
- Updated `lessons-learned.instructions.md`: HasOperations gate, coverage patterns,
  Assert.Contains ambiguity lessons
- Updated `testing.instructions.md`: TweakKind coverage by kind, actual coverage data
  (94.9% line), intentionally untested components
- Updated `Roadmap.md`: Sprint 11 entry, marked completed backlog items (self-contained
  publish, parallel StatusMap, winget manifest v3.2.0, GitHub Releases automation)
- Total: **2,316 tweaks**, **700 tests** (571 Core + 58 CLI + 71 GUI)

## [3.2.0] — 2025-07-22

### Added

- **Windows Health & Maintenance manager** — 19 system health commands
  (DISM, SFC, disk cleanup, network reset, chkdsk, power reports) with
  full dialog UI, admin badge, progress bar, and per-command log
- **320 new tweaks** across expanded modules, bringing total to **2 301 tweaks**
  across **89 categories**:
  - 8 modules expanded to ~20 tweaks each: EventLogging, SsdOptimization,
    AppCompatibility, BrowserCommon, Security, UserAccount, SystemRestore,
    ScheduledTaskTweaks
  - 6 first-wave modules (57 tweaks): CommandLine, PowerShell, Hardening,
    Developer, MemoryOptimization, ScheduledTaskTweaks
  - 4 second-wave modules + expanded Developer/Hardening (+71 tweaks)
  - Wave 3: SsdOptimization, AppCompatibility, UserAccount, BrowserCommon
  - Wave 4: WindowsRecall, ProxyVpn, EventLogging, SystemRestore
  - MemoryOptimization 6→15, PowerShellTweaks 9→15
- **RegistryHives.cs** constant strings for common registry paths
- **77 new expansion tests** — total now **556 tests** (435 Core + 52 CLI + 69 GUI)
- **AppIcons.WindowsHealthIcon** (green shield with white cross)
- **Windows Health** menu item in Tools menu

### Fixed

- **Duplicate tweak ID crash** — `evtlog-enable-powershell-module-logging`
  appeared at lines 80 and 302 in EventLogging.cs; renamed second to
  `evtlog-enable-powershell-transcription`
- **3 duplicate sec- IDs** between Security.cs and Defender.cs: renamed to
  `sec-enforce-lsa-ppl`, `sec-block-wdigest-caching`, `sec-enforce-sehop`
- **Test hangs** — added `tests/.runsettings` with `MaxCpuCount=1`,
  capped `maxParallelThreads` to 4 in all xunit.runner.json

### Changed

- Performance optimizations: tag index, search, HardwareInfo parallelization,
  Analytics caching, MainForm filter dedup, UpdateCounters single-pass
- Updated all documentation with current statistics (2 301 tweaks, 89 categories,
  556 tests)

### Sprint 10 — Test Deepening & Engine Coverage

- 36 new tests across TweakEngine, RegistrySession, and Services
- TweakEngine: snapshot round-trip (SaveSnapshot, LoadSnapshot, RestoreSnapshot),
  ExportJson validation, TweaksByTag, TweaksByScope, GetScope, Freeze/CategoryCounts/ScopeCounts,
  TweaksForProfile, WindowsBuild
- RegistrySession: ReadValue, ReadString, KeyExists, ValueExists, ListSubKeys,
  ListValueNames, ParsePath abbreviated/edge cases
- Services: HardwareInfo (DetectHardware, Summary, SuggestProfile, IsEdgeInstalled),
  CorporateGuard (IsCorporateNetwork, Status, IsGpoManaged, ClearCache)
- Total: **2,316 tweaks**, **700 tests** (571 Core + 58 CLI + 71 GUI)

### Sprint 9 — Test Coverage & Analytics Integration

- CLI: `update <id>` command — runs UpdateAction or falls back to Apply
- CLI: Analytics integration — `RecordSession()` on startup, `Flush()` on exit,
  `RecordApply/Remove/Error` in all action methods (RunAction, RunApplyProfile,
  RunCategoryAction, RunImportJson, RunUpdate)
- 17 new tests: Filter multi-criteria (4), Update method (3), complex dependency
  graphs (3), Analytics persistence (5), CLI update parsing (2)
- Total: **2,316 tweaks**, **658 tests** (529 Core + 58 CLI + 71 GUI)

### Added

- **System theme auto-detection** — GUI follows Windows dark/light mode on startup,
  `Theme.DetectSystemTheme()` reads `AppsUseLightTheme` registry value
- **Percentage progress bar** — batch apply/remove shows percentage instead of
  indeterminate marquee; `SetBusy(bool, string?, int)` + `SetProgress(int)`
- **Tray icon** — minimize to system tray with context menu (Show/Exit);
  `NotifyIcon` with app icon, restore on double-click
- **FrozenDictionary performance** — `TweakEngine.Freeze()` builds `FrozenDictionary`
  for O(1) ID lookups, caches sorted categories, category counts, scope counts;
  called automatically at end of `RegisterBuiltins()`
- **62 plugin system tests** — `PluginTests.cs` covering PackLoader (load, validate,
  SHA-256, all 12 RegOp kinds, validation failures), PackManager (install/uninstall
  lifecycle, version comparison), PackIndex (round-trip), TweakEngine `RegisterPack`
  integration, and Locale (German translations, format args, file loading)
- **Built-in German locale** — 48 translated UI strings in `Locale.cs`,
  `AvailableLocales` property, `SetLocale()` uses built-in locale as base
- **2 system theme tests** in ThemeTests.cs: `DetectSystemTheme_ReturnsValidThemeKey`,
  `DetectSystemTheme_ThemeKeyExistsInAvailable`

### Changed

- **Test parallelism** — `.runsettings` `MaxCpuCount` 1→4 (4 assemblies parallel),
  `TestSessionTimeout` 300s→60s; all `xunit.runner.json` now include
  `longRunningTestSeconds: 5`
- **ShellRunner.DefaultTimeout** reduced from 30s to 10s; `ToolVersionChecker`
  per-tool timeout 5s, per-probe timeout 2s
- **`ScopeCounts()`** now uses `_tweaksByScope` dictionary (O(3)) instead of
  O(n=2301) GroupBy scan
- **`Categories()`** returns cached sorted array instead of re-sorting on every call
- **PackageManagerValidationTests** — removed `OperationCanceledException` swallow
  that silently passed when tool checks timed out
- `.gitignore` pattern changed from `RegiLattice.log` to `*.log`
- Updated Roadmap with Sprint 1–5 completion status
- Total tests: **641** (514 Core + 56 CLI + 71 GUI), all passing

### Sprint 7 — Engine Optimization & Tweak Expansion

- Clean up stale tracking files (current-ids.txt regenerated, missing/removed deleted)
- Profile RegisterBuiltins() performance: 37ms for 2,301 tweaks (budget 500ms)
- Add 4 perf benchmark tests (startup, search, freeze, caching)
- Add 15 new tweaks: 5 Windows Recall, 5 Debloat, 5 Proxy & VPN
- Total: 2,316 tweaks, 89 categories

### Sprint 8 — Consolidation, Validation & CLI Enhancements

- Untrack archive/ (151 files, 84,575 line deletions) + current-ids.txt from git
- Delete .mypy_cache (16 MB) and __pycache__ (3 MB) from disk
- Core: `ValidateTweaks()` — checks empty IDs/Labels/Categories, broken DependsOn, circular deps
- Core: `ResolveDependencies(id)` — topological-sort dependency resolution
- Core: `Dependents(id)` — reverse dependency lookup
- Core: `ApplyBatch`/`RemoveBatch` progress overloads with `Action<int,int,string,TweakResult>` callback
- CLI: ANSI colour output for status display (Green/Red/Yellow/Dim)
- CLI: `--depends-on <id>` command showing deps, reverse deps, and resolved chain
- CLI: `--no-color` flag + auto-detect `Console.IsOutputRedirected`
- CLI: version bump 3.0.0 → 3.2.0, `RunValidate` delegates to engine
- 15 new tests (11 Core + 4 CLI): validation, dep resolution, batch progress, CLI flags
- Total: **2,316 tweaks**, **641 tests** (514 Core + 56 CLI + 71 GUI)

## [3.1.5] — 2025-07-20

### Added

- **49 DetectOps additions** across 18 tweak modules — every registry-based tweak now has
  detection logic (CheckDword, CheckString, or CheckMissing) so `StatusMap()` and the GUI
  status column report accurate applied/not-applied state
  - Backup (1), Boot (3), ContextMenu (4), Defender (2), DevDrive (1), Explorer (1),
    GPU (1), IndexingSearch (2), LockScreen (4), MsStore (1), Network (2), NightLight (2),
    Office (2), OneDrive (1), Performance (1), RealVnc (2), Screensaver (1),
    CloudStorage (14), Startup (3)

### Fixed

- **16 broken TweakDef headers** restored after multi-edit consumed `new TweakDef` openers
  — all 1 981 tweaks register correctly again
- **Build clean** — 0 warnings, 0 errors with `-warnaserror`

### Changed

- Version bump to 3.1.5 across csproj, winget manifests, and WiX installer

## [3.0.0] — 2025-07-20

### ⚠️ BREAKING: Quality audit — removed 468 non-functional tweak stubs

v3.0.0 is the first verified-clean release of the C# codebase. Every remaining tweak
has functional apply/remove/detect operations. Non-functional metadata-only stubs
that silently returned "Applied" without performing any action have been removed.

### Removed

- **468 non-functional tweak stubs** across 66 modules — these had metadata (Id, Label,
  Category, Tags) but no ApplyOps, RemoveOps, DetectOps, or Action delegates. The engine
  silently returned `TweakResult.Applied` for these without performing any registry changes.
- Tweak count reduced from ~1 828 to **1 360 verified functional tweaks**; subsequently
  expanded back to **1 981** through multiple tweak addition campaigns

### Added

- **TweakKind enum** — `Registry`, `Command`, `FileConfig` — classifies how each tweak operates
- **CategoryIcon enum** — 24 icon categories (Shield, Globe, Monitor, Gear, Lock, etc.)
- **CategoryIcons helper class** — maps all 72 category names to icons with Unicode symbols
  for CLI display
- **TweakDef.HasOperations** — computed property that returns true only if a tweak has
  ApplyOps or ApplyAction defined
- **TweakDef.Kind** — computed property returning the TweakKind classification
- **TweakEngine no-op guard** — `Register()` now silently skips tweaks where `HasOperations`
  is false, preventing non-functional tweaks from entering the engine

### Fixed

- **CS0067 warning** — `RegistrySession.LogWritten` event was declared but never invoked;
  now fires in `WriteLog()` method
- **Build warnings** — Release build now produces 0 errors, 0 warnings

## [2.0.0] — 2025-07-20

### ⚠️ BREAKING: Complete rewrite from Python to C#/.NET

The entire project has been rewritten from Python (tkinter, argparse, winreg) to
C#/.NET 10 (WinForms, Microsoft.Win32.Registry). This is a clean-break major version.

### Architecture

- **RegiLattice.Core** — class library with TweakEngine, TweakDef model, RegOp declarative
  pattern, RegistrySession, CorporateGuard, ProfileDefinitions, and 7 service classes
- **RegiLattice.GUI** — WinForms application with 4 switchable themes, package manager
  dialogs, collapsible categories, and scope badges
- **RegiLattice.CLI** — console application with 25+ commands (apply, remove, status,
  list, search, profile, snapshot, diff, doctor, hwinfo, export, import, etc.)
- **71 tweak category modules** — each exports `static List<TweakDef> Tweaks`
- **Declarative RegOp pattern** — ~95% of tweaks defined as data (ApplyOps/RemoveOps/DetectOps)
  instead of imperative code; remaining ~5% use Action/Func delegates

### Added

- **~1 828 tweaks** across 72 categories (migrated from Python with all registry logic preserved)
- **WinForms GUI** replacing tkinter — native Windows look, double-buffered rendering,
  4 themes (Catppuccin Mocha/Latte, Nord, Dracula) with runtime switching and persistence
- **TweakDef.RegOp** — 12 factory methods (SetDword, SetString, SetExpandString, SetQword,
  SetBinary, SetMultiSz, DeleteValue, DeleteTree, CheckDword, CheckString, CheckMissing, CheckKeyMissing)
- **TweakScope enum** — User, Machine, Both (auto-computed from registry key paths)
- **TweakResult enum** — Applied, NotApplied, Unknown, Error, SkippedCorp, SkippedBuild
- **TweakEngine** — comprehensive API: Register, AllTweaks, GetTweak, Categories, Search,
  Filter, StatusMap (parallel), Apply/Remove/ApplyBatch/RemoveBatch, Profiles,
  SaveSnapshot/LoadSnapshot/RestoreSnapshot, CategoryCounts, ScopeCounts, ExportJson
- **RegistrySession** — full registry wrapper with SetDword/SetString/SetExpandString/
  SetQword/SetBinary/SetMultiSz, DeleteValue/DeleteTree, ReadDword/ReadString/etc.,
  KeyExists/ValueExists, ListSubKeys/ListValueNames, Execute(RegOps), Evaluate(DetectOps),
  Backup/Restore, DryRun mode, structured logging
- **5 profiles** — business (39 cats), gaming (31 cats), privacy (31 cats), minimal (22 cats),
  server (28 cats)
- **CorporateGuard** — domain membership, Azure AD detection, GPO checks, SCCM/Intune detection
  with caching and detailed status reporting
- **HardwareInfo** — CPU/GPU/RAM/disk detection, profile suggestion, hardware summary
- **Package manager dialogs** — Scoop, pip, PowerShell module managers in GUI
- **About dialog** — system info, hardware detection, shortcut reference
- **129 xUnit tests** (93 Core + 36 GUI) at initial 2.0.0 release;
  expanded to **203 tests** (112 Core + 52 CLI + 39 GUI) by v3.1.5
- **winget manifests** — installer package for Windows Package Manager

### Changed

- **Language**: Python 3.10+ → C# 13 / .NET 10
- **GUI framework**: tkinter → Windows Forms
- **Registry access**: winreg (Python) → Microsoft.Win32.Registry (C#)
- **Test framework**: pytest → xUnit 2.9.2
- **Build system**: hatchling/pip → dotnet build/MSBuild
- **Tweak pattern**: Function triplet (_apply/_remove/_detect) → Declarative RegOp lists
- **Plugin loading**: Dynamic file discovery → Static RegisterBuiltins() method

### Removed

- Python codebase (archived in `archive/python/`)
- pyproject.toml, requirements.txt, ruff/mypy/pytest configuration
- tkinter GUI, argparse CLI, interactive console menu (Python versions)
- All Python-specific dependencies (hypothesis, pytest-mock, pytest-xdist, etc.)

---

## [1.0.2] — 2025-07-19 (Python — archived)

Final Python release before C# migration. See `archive/python/` for full history.
