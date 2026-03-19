# RegiLattice — Roadmap

> Living document — updated after every sprint.
> Last updated: 2026-05-29 · v3.5.0 · 2 736 tweaks · 92 categories · 1 921 tests

---

## Current State (as of v3.5.0)

| Metric | Value |
|--------|-------|
| Language | C# 13 / .NET 10.0-windows (x64) |
| Tweaks | 2 736 verified across 92 categories |
| Tests | 1 671 total, all passing (1 skipped integration), 4-thread parallel |
| GUI | WinForms with 11 themes, system theme auto-detection, tray icon, percentage progress, live color-coded CPU/RAM status bar |
| Profiles | 5 (business, gaming, privacy, minimal, server) |
| NuGet | System.Management 9.0.3, xUnit 2.9.2, coverlet 6.0.2 |
| CI/CD | GitHub Actions: build + test + coverage + release + CodeQL |
| Platform | Windows 10/11 (x64) |
| Repo | [github.com/RajwanYair/RegiLattice](https://github.com/RajwanYair/RegiLattice) |

---

## Long-Term Vision

Make RegiLattice the **reference Windows registry tweak toolkit**:

- Production-grade packaging distributed via winget, scoop, and GitHub Releases
- Modern WinForms GUI that rivals dedicated system administration tools
- Full corporate-environment safety and detection
- Extensible plugin system with sandboxing
- World-class CI/CD pipeline with automated testing and coverage
- Comprehensive documentation and developer experience

---

## Completed Sprints

### v1.x — Python Foundation (archived)

- Built initial Python prototype with tkinter GUI
- ~1 490 tweaks across 69 categories
- 5 application profiles
- Corporate guard detection
- Plugin marketplace concept

### v2.0.0 — C# Migration (completed)

| Deliverable | Status |
|-------------|--------|
| Full C# rewrite of Core library (TweakDef, RegOp, TweakEngine, RegistrySession) | ✅ |
| 68 tweak module files auto-generated from Python definitions | ✅ |
| WinForms GUI with 4 themes | ✅ |
| CLI with 25+ commands | ✅ |
| 203 xUnit tests (112 Core + 52 CLI + 39 GUI) | ✅ |
| Services: Analytics, AppConfig, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings | ✅ |
| Documentation overhaul (all docs updated for C#) | ✅ |

### v3.0.0 — Quality Audit (completed)

| Deliverable | Status |
|-------------|--------|
| Remove 468 non-functional tweak stubs (no ApplyOps/ApplyAction) | ✅ |
| Add TweakKind + CategoryIcon enums | ✅ |
| Add CategoryIcons mapping class for CLI/GUI display | ✅ |
| TweakEngine no-op guard in Register() | ✅ |
| Fix CS0067 warning (RegistrySession.LogWritten) | ✅ |
| 0 errors, 0 warnings Release build | ✅ |

---

## Planned Sprints

### Sprint 1 — Test Coverage & CI Pipeline ✅

| # | Task | Status |
|---|------|--------|
| 1 | Set up GitHub Actions CI workflow for .NET (build + test on push/PR) | ✅ ci.yml |
| 2 | Add coverlet integration and coverage reporting | ✅ Codecov |
| 3 | Increase Core.Tests to cover all TweakEngine edge cases | ✅ 648 tests |
| 4 | Add CLI argument parsing tests | ✅ 52 tests |
| 5 | Add snapshot round-trip tests | ✅ |
| 6 | Add integration tests for RegistrySession (admin CI runner) | ✅ DryRun |

### Sprint 2 — GUI Modernization ✅

| # | Task | Status |
|---|------|--------|
| 1 | Implement async tweak application (no UI freeze) via `Task.Run()` | ✅ |
| 2 | Add progress bar for batch operations (percentage-based) | ✅ |
| 3 | Add system theme auto-detection (follow Windows dark/light mode) | ✅ |
| 4 | Add tray icon with quick-access menu (minimize to tray) | ✅ |
| 5 | Add tweak detail panel (description, registry keys, dependencies) | ✅ |
| 6 | Add export to .REG file from GUI | ✅ |
| 7 | DPI-aware scaling for high-resolution displays | ✅ |
| 8 | Test parallelism: 4 threads per assembly, 4 assemblies parallel | ✅ |
| 9 | ShellRunner timeout optimization (30s → 10s default, 5s per tool) | ✅ |

### Sprint 3 — Performance Optimization ✅

| # | Task | Status |
|---|------|--------|
| 1 | FrozenDictionary for ID lookups via `Freeze()` | ✅ |
| 2 | Cache `Categories()` sorted array (eliminate O(k log k) per call) | ✅ |
| 3 | Optimize `StatusMap()` with parallel registry reads | ✅ (existed) |
| 4 | Cache category counts + scope counts post-registration | ✅ |
| 5 | Fix `ScopeCounts()`: O(3) from `_tweaksByScope` vs O(n=2301) GroupBy | ✅ |

### Sprint 4 — Packaging & Distribution ✅

| # | Task | Status |
|---|------|--------|
| 1 | Self-contained single-file publish for CLI + GUI | ✅ release.yml |
| 2 | Update winget manifest for v3.2.0 | ✅ |
| 3 | GitHub Releases with auto-generated release notes | ✅ softprops/action-gh-release |
| 4 | Automated build pipeline (build → test → publish on tag) | ✅ release.yml |
| 5 | WiX 6.0.2 MSI installer | ✅ installer/Package.wxs |
| 6 | Create Scoop bucket entry | ✅ scoop/regilattice.json |
| 7 | Code signing for published binaries | ⭕ planned |

### Sprint 5 — Advanced Features ✅

| # | Task | Status |
|---|------|--------|
| 1 | Plugin system: JSON Tweak Packs with marketplace (PackDef, PackLoader, PackManager, PackIndex) | ✅ |
| 2 | User-defined tweaks via JSON (no C# required) — PackLoader validates & converts JSON to TweakDef | ✅ |
| 3 | Plugin system tests: 62 tests covering PackLoader, PackManager, PackIndex, locale | ✅ |
| 4 | Localization: built-in German locale with all 48 UI strings | ✅ |
| 5 | Scheduled tweak application (apply on boot/login) | ⭕ planned (P3) |
| 6 | REST API layer for remote management | ⭕ planned (P3) |
| 7 | Web dashboard for tweak status visualization | ⭕ planned (P3) |

### Sprint 6 — Documentation, Distribution & Polish ✅

| # | Task | Status |
|---|------|--------|
| 1 | Update CHANGELOG.md [Unreleased] with Sprint 2-5 post-v3.2.0 work | ✅ |
| 2 | Update copilot-instructions.md: 622 tests, 8 files, PluginTests, Plugins/, locale, tray icon | ✅ |
| 3 | Update testing.instructions.md + workspace.instructions.md stats | ✅ |
| 4 | Create Scoop bucket manifest (`scoop/regilattice.json`) | ✅ |
| 5 | Fix ListViewColumnSorter.cs CRLF → LF line endings | ✅ |
| 6 | Update repo memory from Python-era to C# conventions | ✅ |

### Sprint 7 — Engine Optimization & Tweak Expansion ✅

| # | Task | Status |
|---|------|--------|
| 1 | Clean up stale tracking files (current-ids.txt regenerated, missing/removed deleted) | ✅ |
| 2 | Profile RegisterBuiltins() performance: 37ms for 2,301 tweaks (budget 500ms) | ✅ |
| 3 | Add 4 perf benchmark tests (startup, search, freeze, caching) | ✅ |
| 4 | Add 15 new tweaks: 5 Windows Recall, 5 Debloat, 5 Proxy & VPN | ✅ |
| 5 | Total: 2,316 tweaks, 626 tests (503 Core + 52 CLI + 71 GUI) | ✅ |

### Sprint 8 — Consolidation, Validation & CLI Enhancements ✅

| # | Task | Status |
|---|------|--------|
| 1 | Untrack archive/ (151 files, 84 575 line deletions) + current-ids.txt from git | ✅ |
| 2 | Delete .mypy_cache (16 MB) and __pycache__ (3 MB) from disk | ✅ |
| 3 | Update .gitignore: archive/, current-ids.txt, rename Python section | ✅ |
| 4 | Core: ValidateTweaks() — checks empty IDs/Labels/Categories, broken DependsOn, circular deps | ✅ |
| 5 | Core: ResolveDependencies(id) — topological-sort dependency resolution | ✅ |
| 6 | Core: Dependents(id) — reverse dependency lookup | ✅ |
| 7 | Core: ApplyBatch/RemoveBatch progress overloads with Action<int,int,string,TweakResult> callback | ✅ |
| 8 | CLI: ANSI colour output for status display (Green/Red/Yellow/Dim) | ✅ |
| 9 | CLI: --depends-on <id> command showing deps, reverse deps, and resolved chain | ✅ |
| 10 | CLI: --no-color flag + auto-detect redirected output | ✅ |
| 11 | CLI: version bump 3.0.0 → 3.2.0, RunValidate() delegates to engine | ✅ |
| 12 | 15 new tests (11 Core + 4 CLI): validation, dep resolution, batch progress, CLI flags | ✅ |
| 13 | Update instruction files: copilot, workspace, testing, Roadmap | ✅ |
| 14 | Total: 2,316 tweaks, 641 tests (514 Core + 56 CLI + 71 GUI) | ✅ |

### Sprint 9 — Test Coverage & Analytics Integration ✅

| # | Task | Status |
|---|------|--------|
| 1 | CLI: `update <id>` command — runs UpdateAction or falls back to Apply | ✅ |
| 2 | CLI: Analytics.RecordSession() on startup + Analytics.Flush() on exit | ✅ |
| 3 | CLI: RecordApply/Remove/Error in RunAction, RunApplyProfile, RunCategoryAction, RunImportJson, RunUpdate | ✅ |
| 4 | CLI: Add `update <id>` to help text and `--depends-on`, `--no-color` to General section | ✅ |
| 5 | 4 Filter tests: AllCriteria, NoMatches, NoCriteria, QueryAndScope | ✅ |
| 6 | 3 Update tests: NoUpdateAction fallback, WithUpdateAction, UpdateActionThrows | ✅ |
| 7 | 3 dependency tests: diamond graph, deep chain (5 levels), multiple children | ✅ |
| 8 | 5 Analytics tests: RecordRemove, RecordError, RecordSession, Flush persist, Flush no-op | ✅ |
| 9 | 2 CLI tests: update mode positional, update mode with flags | ✅ |
| 10 | Update all documentation with current stats (658 tests, 529 Core + 58 CLI + 71 GUI) | ✅ |
| 11 | Total: 2,316 tweaks, 658 tests (529 Core + 58 CLI + 71 GUI) | ✅ |

### Sprint 10 — Test Deepening & Engine Coverage ✅

| # | Task | Status |
|---|------|--------|
| 1 | TweakEngine: snapshot tests (SaveSnapshot, LoadSnapshot, RestoreSnapshot round-trip) | ✅ |
| 2 | TweakEngine: ExportJson validation (valid JSON array, correct count) | ✅ |
| 3 | TweakEngine: TweaksByTag, TweaksByScope, GetScope tests | ✅ |
| 4 | TweakEngine: Freeze, CategoryCounts, ScopeCounts tests | ✅ |
| 5 | TweakEngine: TweaksForProfile valid/invalid name tests | ✅ |
| 6 | TweakEngine: WindowsBuild returns positive | ✅ |
| 7 | RegistrySession: read ops (ReadValue, ReadString), existence checks (KeyExists, ValueExists) | ✅ |
| 8 | RegistrySession: ListSubKeys, ListValueNames, ParsePath edge cases | ✅ |
| 9 | Services: HardwareInfo (DetectHardware, Summary, SuggestProfile, IsEdgeInstalled) | ✅ |
| 10 | Services: CorporateGuard (IsCorporateNetwork, Status, IsGpoManaged, ClearCache) | ✅ |
| 11 | Total: 2,316 tweaks, 700 tests (571 Core + 58 CLI + 71 GUI) | ✅ |

---

## Sprint 11 — Documentation Refresh & Roadmap Cleanup

| # | Task | Status |
|---|------|--------|
| 1 | GUI: log panel visible by default | ✅ |
| 2 | Update copilot-instructions.md: full TweakDef model, TweakKind, TweakResult tables | ✅ |
| 3 | Update workspace.instructions.md: TweakKind table with fields used | ✅ |
| 4 | Update lessons-learned.instructions.md: HasOperations gate, coverage patterns, Assert.Contains ambiguity | ✅ |
| 5 | Update testing.instructions.md: TweakKind coverage patterns, actual coverage data, intentionally untested components | ✅ |
| 6 | Mark completed roadmap items: self-contained publish, parallel StatusMap, winget manifest, GitHub Releases | ✅ |
| 7 | Coverage verification: 94.9% line coverage (exceeds 80% target) | ✅ |

### Sprint 12 — Codebase Refactoring & Architecture Improvements ✅

| # | Task | Status |
|---|------|--------|
| 1 | Extract `SnapshotManager.cs` from TweakEngine (Save/Load/Restore) | ✅ |
| 2 | Extract `TweakValidator.cs` from TweakEngine (ValidateTweaks + circular dep detection) | ✅ |
| 3 | Extract `DependencyResolver.cs` from TweakEngine (topological sort + Dependents) | ✅ |
| 4 | Extract `CliArgs.cs` from CLI Program.cs (nested class to standalone) | ✅ |
| 5 | Extract `ConsoleColorizer.cs` from CLI Program.cs (ANSI colour helpers) | ✅ |
| 6 | Create `PackageNameValidator.cs` — consolidate 5 identical regex/validation across package managers | ✅ |
| 7 | 27 new tests (700 → 727): 10 ConsoleColorizer, 8 PackageNameValidator, 9 CLI parsing | ✅ |
| 8 | TweakEngine reduced from ~750 → ~580 lines, all public API unchanged | ✅ |
| 9 | Update all documentation: CHANGELOG, copilot-instructions, workspace, testing, Roadmap, Api, Coverage, Development, Readme, Profiling, lessons-learned | ✅ |
| 10 | Total: 2,316 tweaks, 727 tests (571 Core + 72 CLI + 84 GUI) | ✅ |

### Sprint 13 — Test Coverage Expansion ✅

| # | Task | Status |
|---|------|--------|
| 1 | Add `SnapshotManagerTests.cs` — 12 direct tests (Save, Load, Restore, round-trip, edge cases) | ✅ |
| 2 | Add `TweakValidatorTests.cs` — 19 direct tests (valid tweaks, empty fields, duplicates, circular deps) | ✅ |
| 3 | Add `DependencyResolverTests.cs` — 15 direct tests (Resolve topological sort, Dependents reverse lookup, circular detection) | ✅ |
| 4 | Add TweakEngine edge case tests (+13): TweaksByScope, Filter, IsApplicableOnHardware, DetectStatus, StatusMap subset, Search multi-token | ✅ |
| 5 | Add RegistrySession edge case tests (+17): Execute DryRun, Evaluate CheckMissing/CheckKeyMissing, Backup, WriteLog, Read ops, ParsePath | ✅ |
| 6 | Fix corporate guard interference in tests (`forceCorp: true`) | ✅ |
| 7 | GUI verified — all status data is dynamic, no stale values | ✅ |
| 8 | Update all documentation with new test counts | ✅ |
| 9 | Total: 2,316 tweaks, 799 tests (643 Core + 72 CLI + 84 GUI) | ✅ |

### Sprint 14 — Deep Test Expansion & Documentation Reconciliation ✅

| # | Task | Status |
|---|------|--------|
| 1 | Add `TweakEngineBuiltinsTests.cs` — 63 integration tests with shared `BuiltinsFixture` (RegisterBuiltins perf, unique IDs, required fields, profiles, search, filter, categories) | ✅ |
| 2 | Expand Core tests: +95 (643 → 738) across TweakDef, TweakEngine, RegistrySession, Services, Plugins | ✅ |
| 3 | Expand CLI tests: +31 (72 → 103) in ParseArgsTests | ✅ |
| 4 | Expand GUI tests: +47 (84 → 131) across ThemeTests, PackageManagerValidation, AppIcons | ✅ |
| 5 | Reconcile all documentation: CHANGELOG, Roadmap, Readme, copilot-instructions, testing, workspace, Coverage, Development | ✅ |
| 6 | Total: 2,316 tweaks, 972 tests (738 Core + 103 CLI + 131 GUI), 13 test files | ✅ |

### Sprint 22 — Performance + Consolidation + Production Readiness (50 Tasks)

| # | Task | Status |
|---|------|--------|
| 1 | Add `Directory.Build.props` for shared .NET build properties | ✅ |
| 2 | Add `Directory.Packages.props` for central package version management | ✅ |
| 3 | Remove duplicate framework/nullable/implicit/platform/version config from Core csproj | ✅ |
| 4 | Remove duplicate framework/nullable/implicit/platform/version config from GUI csproj | ✅ |
| 5 | Remove duplicate framework/nullable/implicit/platform/version config from CLI csproj | ✅ |
| 6 | Remove duplicate test package versions from Core.Tests csproj | ✅ |
| 7 | Remove duplicate test package versions from GUI.Tests csproj | ✅ |
| 8 | Remove duplicate test package versions from CLI.Tests csproj | ✅ |
| 9 | Switch TweakEngine scope index to enum-keyed dictionary | ✅ |
| 10 | Eliminate repeated `ToString().ToLowerInvariant()` scope lookups in hot paths | ✅ |
| 11 | Simplify scope count computation without enum parsing | ✅ |
| 12 | Convert search text cache to ID-based lookup for lower overhead | ✅ |
| 13 | Add Locale hot-key cache to reduce repeated dictionary lookups | ✅ |
| 14 | Invalidate Locale hot cache when locale changes | ✅ |
| 15 | Replace per-keystroke search timer allocation in MainForm | ✅ |
| 16 | Reuse a single debounce timer instance for search | ✅ |
| 17 | Skip redundant search refresh when text has not changed | ✅ |
| 18 | Expand VS Code file excludes to hide generated/large folders | ✅ |
| 19 | Add VS Code watcher excludes for bin/obj/publish/.tmp to reduce CPU churn | ✅ |
| 20 | Add VS Code recommendation for GitHub Actions extension | ✅ |
| 21 | Align VS Code Core test task with runsettings and hang timeout | ✅ |
| 22 | Align VS Code GUI test task with runsettings and hang timeout | ✅ |
| 23 | Add VS Code CLI test task | ✅ |
| 24 | Add VS Code release gate task (build+test+publish) | ✅ |
| 25 | Harden CI workflow with manual dispatch support | ✅ |
| 26 | Add CI concurrency guard to cancel superseded runs | ✅ |
| 27 | Add CI timeout budget to prevent stuck runners | ✅ |
| 28 | Expand CI NuGet cache key to include central package/SDK config | ✅ |
| 29 | Use runsettings + hang timeout in CI test execution | ✅ |
| 30 | Harden coverage artifact upload with missing-file warning behavior | ✅ |
| 31 | Harden Release workflow with manual dispatch support | ✅ |
| 32 | Add Release workflow concurrency guard | ✅ |
| 33 | Add Release workflow timeout budget | ✅ |
| 34 | Expand Release NuGet cache key to include central package/SDK config | ✅ |
| 35 | Use runsettings + hang timeout in Release test execution | ✅ |
| 36 | Publish GUI into deterministic release output directory | ✅ |
| 37 | Publish CLI into deterministic release output directory | ✅ |
| 38 | Generate SHA256 checksums for release artifacts | ✅ |
| 39 | Include checksums file in GitHub release assets | ✅ |
| 40 | Align PowerShell analyzer workflow to Windows runner | ✅ |
| 41 | Update Development guide with centralized build/package management | ✅ |
| 42 | Update Development guide with production release gate commands | ✅ |
| 43 | Update README test badge with latest total | ✅ |
| 44 | Update Roadmap headline test metrics | ✅ |
| 45 | Add this 50-task sprint plan to roadmap with tracked completion | ✅ |
| 46 | Validate solution builds after refactor | ✅ |
| 47 | Validate full test suite after refactor | ✅ |
| 48 | Validate release publish outputs after refactor | ✅ |
| 49 | Commit refactor and production-hardening changes | ✅ |
| 50 | Push release-ready state to GitHub main branch | ✅ |

### Sprint 15 — 50 New Tweaks & Test Performance ✅

| # | Task | Status |
|---|------|--------|
| 1 | Optimise GUI test suite: ToolVersionChecker timeout 5s→3s, test timeout 25s→12s, CancellationToken 20s→10s | ✅ |
| 2 | Enable assembly-level parallelism across all 3 test projects (xunit.runner.json) | ✅ |
| 3 | Add 10 tweaks to PowerManagement.cs (12→22): adaptive brightness, power throttling, core parking, etc. | ✅ |
| 4 | Add 10 tweaks to CommandLineTweaks.cs (14→24): .NET 3.5, Telnet, TFTP, IPv6 tunnels, NTP, MPO, etc. | ✅ |
| 5 | Add 10 tweaks to Developer.cs (15→25): .NET CLI telemetry, symlinks, Python UTF-8, Git config, containers, etc. | ✅ |
| 6 | Add 10 tweaks to Hardening.cs (15→25): AutoRun, SAM, remote assistance, SMB signing, LLMNR, admin shares, etc. | ✅ |
| 7 | Add 10 tweaks to NetworkOptimization.cs (15→25): TCP Fast Open, RSC, ARP cache, DCA, keepalive, etc. | ✅ |
| 8 | Update all documentation with new tweak count (2,316→2,366) | ✅ |
| 9 | Total: 2,366 tweaks, 972 tests (738 Core + 103 CLI + 131 GUI), 13 test files | ✅ |

### Sprint 16 — Security Audit & Validation Enhancement ✅

| # | Task | Status |
|---|------|--------|
| 1 | Security audit: identify & remove insecure tweaks (Telnet, TFTP, EFS disable) | ✅ |
| 2 | Remove `cmd-enable-telnet-client`, `cmd-enable-tftp-client`, `cmd-enable-fsutil-disable-encrypt` from CommandLineTweaks.cs | ✅ |
| 3 | Add `TweakValidator.DetectDuplicateRegistryOps()` — warns when >1 tweak writes to same `Path\Name` | ✅ |
| 4 | Add `TweakEngine.DetectDuplicateRegistryOps()` convenience method | ✅ |
| 5 | CLI `--validate` now shows errors + duplicate registry warnings separately (exit 1 only on errors) | ✅ |
| 6 | 6 new validator tests: DuplicateTarget, SamePathDiffNames, CaseInsensitive, SameTweakMultiOps, DeleteTreeDuplicate, NoOverlap | ✅ |
| 7 | Competitive analysis: 13 Win11 tweak tools researched (Winaero, ExplorerPatcher, OFGB, etc.) | ✅ |
| 8 | Gap analysis & enhancement roadmap generated (100 items across 10 phases) | ✅ |
| 9 | Total: 2,363 tweaks, 960 tests (738 Core + 103 CLI + 131 GUI) | ✅ |

### Sprint 17 — Core Services, CLI Commands & 50 New Tweaks ✅

| # | Task | Status |
|---|------|--------|
| 1 | ConfigExporter service — export/import portable tweak configs as JSON | ✅ |
| 2 | Favorites service — persist user favorite tweak IDs, thread-safe, case-insensitive | ✅ |
| 3 | TweakHistory service — rolling 500-entry operation history with ISO 8601 timestamps | ✅ |
| 4 | 7 CLI commands: export-config, import-config, favorites, favorite-add/remove, history | ✅ |
| 5 | 50 new tweaks: Display +10, Startup +10, Network Optimization +10, Power Management +10, Privacy +10 | ✅ |
| 6 | 40 new tests: FavoritesTests (11), TweakHistoryTests (11), ConfigExporterTests (10), FavoritesAndHistoryParseTests (8) | ✅ |
| 7 | Total: 2,410 tweaks, 1,001 tests (779 Core + 111 CLI + 131 GUI) | ✅ |

### Sprint 18 — GUI Visual Overhaul, 7 New Themes & 50 New Tweaks ✅

| # | Task | Status |
|---|------|--------|
| 1 | 7 new colour themes: Tokyo Night, Gruvbox Dark, Solarized Dark, One Dark Pro, Rosé Pine, Everforest, Cyberpunk (11 total) | ✅ |
| 2 | AppIcons gradient overhaul: 9 existing + 7 new icons with LinearGradientBrush | ✅ |
| 3 | ToolStrip buttons: Apply/Remove/Refresh with colourful ImageAndText style | ✅ |
| 4 | MainForm visual polish: gradient column headers + gradient selection highlight | ✅ |
| 5 | 50 new tweaks: DNS +10, Encryption +10, Firewall +10, Hardening +10, Recovery +10 | ✅ |
| 6 | 28 new tests: 21 theme Theory tests (7 themes × 3), 7 AppIcons bitmap tests | ✅ |
| 7 | Total: 2,460 tweaks, 1,029 tests (779 Core + 111 CLI + 159 GUI) | ✅ |

### Sprint 19 — System Monitoring & 50 New Tweaks ✅

| # | Task | Status |
|---|------|--------|
| 1 | SystemMonitor service: CPU usage (GetSystemTimes P/Invoke, delta-based), memory (GlobalMemoryStatusEx), uptime (TickCount64) | ✅ |
| 2 | Live CPU/RAM in MainForm status bar: 2s timer, accent-coloured labels | ✅ |
| 3 | System uptime in About dialog (`Xd Yh Zm` format) | ✅ |
| 4 | 50 new tweaks: Display +10, Fonts +10, Input +10, Audio +10, Taskbar +10 | ✅ |
| 5 | 7 new SystemMonitor tests: CPU range, memory values, uptime consistency, multi-instance | ✅ |
| 6 | Total: 2,510 tweaks, 1,305 tests (784 Core + 111 CLI + 410 GUI) | ✅ |

---

## Competitive Analysis Summary

> Research from 2026-03-16 covering 13 top Win11 tweak tools.

| Tool | Type | Focus | GitHub Stars |
|------|------|-------|-------------|
| **Winaero Tweaker** | All-in-one tweaker | Hundreds of registry/UI tweaks | Millions of users |
| **ExplorerPatcher** | Shell patcher | Win10 taskbar/Start on Win11 | 31.9k★ |
| **TranslucentTB** | Taskbar transparency | Taskbar visual effects | 19.1k★ |
| **Mem Reduct** | Memory cleaner | Cache clearing | 8.9k★ |
| **Open-Shell** | Start menu | Classic Start menu | 8.7k★ |
| **OFGB** | Ad remover | Win11 ads only | 7.5k★ |
| **StartAllBack** | Start + taskbar | Win7/10 UI on Win11 | Paid |
| **Start11** | Start menu | Custom Start + taskbar | Paid |
| **WindowBlinds 11** | Visual skins | Window frames/themes | Paid |
| **Fences 6** | Desktop organizer | Icon groups | Paid |
| **NetAdapter Repair** | Network fixer | TCP/IP/Winsock reset | Free |
| **AutoPowerOptionsOK** | Power switcher | Power plan toggle | Free |
| **MS PC Manager** | System optimizer | Cleanup + boost | Free (MS) |

### RegiLattice Unique Strengths (already ahead)

- Most tweaks by far: 2,510 across 89 categories
- Declarative tweak model (TweakDef + RegOp)
- 5 profile system (business, gaming, privacy, minimal, server)
- Plugin marketplace (JSON Tweak Packs)
- Corporate Guard (enterprise safety)
- Dependency resolution (topological sort)
- Snapshot/restore (save and rollback)
- CLI + GUI (25+ commands + full WinForms)
- DryRun mode (preview before applying)
- 11 themes (Catppuccin Mocha/Latte, Nord, Dracula + 7 more)
- Live system monitoring (CPU, RAM, uptime in status bar)
- Validation engine (TweakValidator + duplicate registry detection)
- Full test suite (1,305 xUnit tests, 95% line coverage)

---

## Future Roadmap — 100 Enhancement Items (10 Phases)

### Phase 1 — UX & Config Management (Sprint 17–18)

| # | Item | Priority | Source |
|---|------|----------|--------|
| 1 | Import/export tweak selections as JSON config file | HIGH | Winaero Tweaker |
| 2 | Tweak favorites/bookmarks — star frequently used tweaks | HIGH | Winaero Tweaker |
| 3 | Tweak history panel — timeline of applied/removed with undo | HIGH | Original |
| 4 | Search result highlighting — bold matched terms in tweak list | MEDIUM | UX improvement |
| 5 | Recently applied tweaks section in GUI sidebar | MEDIUM | UX improvement |
| 6 | Tweak comparison view — diff two tweak configs side by side | MEDIUM | Original |
| 7 | Bulk select by tag (e.g., select all "privacy" tagged tweaks) | MEDIUM | UX improvement |
| 8 | Keyboard shortcuts for common operations (Ctrl+A apply, Ctrl+Z undo) | MEDIUM | UX improvement |
| 9 | "What's New" dialog on version upgrade showing changelog | LOW | Original |
| 10 | Tweak tooltip with full description on hover | LOW | UX improvement |

### Phase 2 — System Monitoring & Diagnostics (Sprint 19–20)

| # | Item | Priority | Source |
|---|------|----------|--------|
| 11 | Real-time memory stats in GUI status bar (RAM usage, cache) | ✅ Sprint 19 | Mem Reduct |
| 12 | Memory cache cleaner — working set purge via Native API | HIGH | Mem Reduct |
| 13 | Automatic memory cleaning on threshold (e.g., >80% RAM) | MEDIUM | Mem Reduct |
| 14 | System tray memory usage indicator (icon or percentage) | MEDIUM | Mem Reduct |
| 15 | CPU usage monitor in status bar | ✅ Sprint 19 | Original |
| 16 | Disk usage overview panel (per-drive space breakdown) | MEDIUM | MS PC Manager |
| 17 | Network connectivity status indicator | LOW | NetAdapter Repair |
| 18 | Battery health monitor for laptops | LOW | Original |
| 19 | System uptime display in About dialog | ✅ Sprint 19 | Original |
| 20 | Hardware temperature monitoring (CPU/GPU) via WMI | LOW | Original |

### Phase 3 — Visual Appearance Tweaks (Sprint 21–22)

| # | Item | Priority | Source |
|---|------|----------|--------|
| 21 | Title bar color customization (active/inactive) | HIGH | Winaero Tweaker |
| 22 | Scrollbar width/height adjustment | HIGH | Winaero Tweaker |
| 23 | System font replacement (menus, dialogs, title bars) | HIGH | Winaero Tweaker |
| 24 | Icon spacing adjustment (horizontal/vertical) | MEDIUM | Winaero Tweaker |
| 25 | Window border width customization | MEDIUM | Winaero Tweaker |
| 26 | Menu animation speed control | MEDIUM | Winaero Tweaker |
| 27 | Tooltip delay adjustment | MEDIUM | Winaero Tweaker |
| 28 | Alt+Tab appearance switch (Win10 vs Win11 style) | MEDIUM | ExplorerPatcher |
| 29 | Accent color customization for Start/Taskbar/Title bars | LOW | WindowBlinds |
| 30 | Dark mode per-app overrides | LOW | Original |

### Sprint 20 — Sprint 22: Refactoring, Performance & CI Hardening ✅

> See Sprint 22 commit `f8e4129`, tag `v3.4.0-refactor.1`.
> MSBuild centralization, runtime performance (enum scope index, locale hot cache, debounce timer),
> CI hardening (concurrency, timeouts, SHA256), VS Code tooling updates, docs refresh.
> Total: 2,610 tweaks, 1,308 tests (950 Core + 116 CLI + 241 GUI + 1 skipped)

### Sprint 23 — Coverage Expansion (+62 tests) ✅

| # | Task | Status |
|---|------|--------|
| 1 | `TweakEngineCoverageTests.cs` — 50 new tests covering all unvisited TweakEngine branches | ✅ |
| 2 | Apply/Remove/Update result branches: SkippedBuild, SkippedHw, delegate paths, forceCorp | ✅ |
| 3 | `IsApplicableOnHardware` — all 13 software categories (Theory) + 4 tag types | ✅ |
| 4 | Filter(query:), multi-token AND search, StatusMap(ids:), ExportJson, GetScope, Freeze caching | ✅ |
| 5 | ServicesTests extensions: Locale `_hotCache` hit/miss, LoadLocaleFile, CorporateGuard branches | ✅ |
| 6 | HardwareInfo extended: IsAdobeInstalled, IsLibreOffice, IsRealVnc, IsScoop, IsDocker, Gpus, Disk | ✅ |
| 7 | Refactor `CorporateGuard._cached` from `bool?` to `Lazy<bool>` (thread-safe) | ✅ |
| 8 | Total: 2,610 tweaks, **1,370 tests** (1,012 Core + 116 CLI + 241 GUI + 1 skipped) | ✅ |

### Sprint 24 — UX Enhancements + 51 Visual Appearance Tweaks ✅

| # | Task | Status |
|---|------|--------|
| 1 | Search text highlighting in ListView owner-draw (bold accent on matched text) | ✅ |
| 2 | "Recently Applied" virtual category in tree view (last 50 from TweakHistory) | ✅ |
| 3 | `WindowAppearance.cs` — 51 new tweaks (title bars, scrollbars, fonts, icons, borders, animations, accent colours) | ✅ |
| 4 | CategoryIcons mapping for "Window Appearance" category | ✅ |
| 5 | PreferencesDialog — tabbed settings (Appearance, Behaviour, Performance, Data) | ✅ |
| 6 | AppConfig extended with 6 new properties (ConfirmApply/Remove, ShowInapplicable, etc.) | ✅ |
| 7 | Sprint 24 builtins tests (15 specific IDs + category assertions) | ✅ |
| 8 | Total: 2,661 tweaks, 90 categories | ✅ |

### Sprint 25 — 75 New Tweaks (System Optimization + Desktop Customization) ✅

| # | Task | Status |
|---|------|--------|
| 1 | `SystemOptimization.cs` — 39 new tweaks (memory mgmt, I/O scheduling, kernel params, crash control) | ✅ |
| 2 | `DesktopCustomization.cs` — 36 new tweaks (Explorer behaviour, Quick Access, ribbon, feeds) | ✅ |
| 3 | CategoryIcons mapping for 2 new categories | ✅ |
| 4 | Sprint 25 builtins tests (27 specific IDs + category assertions) | ✅ |
| 5 | Total: 2,736 tweaks, 92 categories | ✅ |

### Sprint 26 — WhatsNew Dialog + Test Expansion ✅

| # | Task | Status |
|---|------|--------|
| 1 | WhatsNewDialog — version-gated changelog viewer with ShouldShow/MarkSeen logic | ✅ |
| 2 | AppConfig `LastSeenVersion` property for upgrade detection | ✅ |
| 3 | WhatsNewDialog auto-show on startup for new versions | ✅ |
| 4 | Help menu integration ("What's New..." menu item) | ✅ |
| 5 | GUI detail panel BackColor fix (gray block → themed Surface colour) | ✅ |
| 6 | Test hang fix — Sprint 24 ExportJson/ApplyProfile tests refactored to avoid process spawning | ✅ |
| 7 | Sprint 26 GUI tests (WhatsNewDialog ShouldShow) | ✅ |
| 8 | Total: 2,736 tweaks, 92 categories, **1,645 tests** (1,248 Core + 154 CLI + 242 GUI + 1 skipped) | ✅ |

### Sprint 27 — Network Tools ✅

| # | Task | Status |
|---|------|--------|
| 1 | `NetworkManager` Core service (DNS quick-switch, TCP/IP + Winsock + DNS reset, DHCP renew) | ✅ |
| 2 | 6 built-in DNS presets: DHCP, Cloudflare, Google, Quad9, OpenDNS, NextDNS | ✅ |
| 3 | `NetworkToolsDialog` GUI — adapter drop-down, DNS preset buttons, repair action buttons, async log | ✅ |
| 4 | Tools menu → Network Tools (globe icon) | ✅ |
| 5 | `NetworkManagerTests.cs` — 8 unit tests covering presets and read-only operations | ✅ |

### Sprint 28 — Startup Manager ✅

| # | Task | Status |
|---|------|--------|
| 1 | `StartupManager` Core service — reads HKCU/HKLM Run + Run-Disabled registry keys + user/all-users Startup folders | ✅ |
| 2 | `StartupEntry` record + `StartupLocation` enum (RegistryUser, RegistryMachine, FolderUser, FolderAllUsers) | ✅ |
| 3 | Enable/Disable (moves between Run ↔ Run-Disabled), Delete operations | ✅ |
| 4 | `StartupManagerDialog` GUI — ListView with Name/Status/Location/Command, admin banner, Enable/Disable/Delete/Refresh | ✅ |
| 5 | Tools menu → Startup Manager (rocket icon) | ✅ |
| 6 | `StartupManagerTests.cs` — 7 unit tests covering read-only operations and record model | ✅ |

### Sprint 29 — Service Manager ✅

| # | Task | Status |
|---|------|--------|
| 1 | `ServiceManager` Core service — enumerates and controls Windows services via `System.ServiceProcess.ServiceController` | ✅ |
| 2 | `ServiceEntry` record (ServiceName, DisplayName, Description, Status, StartType, CanStop, CanPauseAndContinue) | ✅ |
| 3 | `StartAsync`, `StopAsync`, `SetStartTypeAsync` (uses `sc.exe config`) async operations with CancellationToken | ✅ |
| 4 | `ServiceManagerDialog` GUI — searchable ListView, description panel, async Start/Stop/Enable/Disable/Refresh, admin banner | ✅ |
| 5 | Tools menu → Service Manager (gear icon) | ✅ |
| 6 | `System.ServiceProcess.ServiceController` v9.0.3 NuGet added | ✅ |
| 7 | `ServiceManagerTests.cs` — 10 unit tests covering enumeration and record model | ✅ |

### Task 6 — BaseDialog Consolidation ✅

| # | Task | Status |
|---|------|--------|
| 1 | `BaseDialog : Form` abstract class with common constructor (title, size, resizable) | ✅ |
| 2 | Helper factory methods: `CreateSectionHeader()`, `CreateLabel()`, `CreateButtonRow()`, `CreateButton()` | ✅ |
| 3 | Migrated `NetworkToolsDialog`, `StartupManagerDialog`, `ServiceManagerDialog` to `: BaseDialog` | ✅ |

### Sprint 30 — System Utilities & UX Enhancements ✅

| # | Task | Status |
|---|------|--------|
| 1 | `BaseDialog.EnableStandaloneMode()` — all dialogs launchable via `--tool <name>` with correct title bar + minimize button | ✅ |
| 2 | `BaseDialog.CreateAdminBanner()` / `CreateWarningBanner()` — consistent banner factories for all dialogs | ✅ |
| 3 | `BaseDialog.StartPosition = CenterScreen`, `MaximizeBox = resizable` — UX consistency | ✅ |
| 4 | Removed theme combo from toolbar — theme selection moved exclusively to Preferences dialog | ✅ |
| 5 | `AppConfig` enriched: `RememberSplitter`, `SplitterDistance`, `SkipAppliedOnBatch`, `HistoryMaxEntries`, `MonitorColorCoded` | ✅ |
| 6 | `PreferencesDialog` — Behaviour tab: `SkipAppliedOnBatch`, `RememberSplitter` checkboxes | ✅ |
| 7 | `PreferencesDialog` — Performance tab: `HistoryMaxEntries` spinner, `MonitorColorCoded` checkbox | ✅ |
| 8 | `MainForm` — splitter position persisted to config on move, restored on startup | ✅ |
| 9 | `MainForm.ApplySelectedAsync` — respects `SkipAppliedOnBatch` config flag | ✅ |
| 10 | `MainForm.OnMonitorTimerTick` — color-coded CPU/RAM labels (green/amber/red) when `MonitorColorCoded` enabled | ✅ |
| 11 | `ContextMenuManagerDialog` — view/enable/disable Windows shell context-menu handlers from registry | ✅ |
| 12 | `HostsFileManagerDialog` — read/add/toggle/delete hosts file entries with inline add dialog | ✅ |
| 13 | `TempFileCleanerDialog` — scan %TEMP%, Windows\Temp, Prefetch, SoftwareDistribution, Recycle Bin with size preview | ✅ |
| 14 | `InstalledAppsDialog` — installed programs viewer with column-sort + launch native uninstaller | ✅ |
| 15 | `AppIcons.ExplorerMenuBitmap` / `CleanupMenuBitmap` — programmatic menu icons for new dialogs | ✅ |
| 16 | Tools menu: Context Menu Manager, Hosts File Manager, Temp File Cleaner, Installed Applications entries | ✅ |
| 17 | `Program.cs ResolveManagerArg` — contextmenu, hostsfile, tempcleaner, installedapps standalone launch support | ✅ |
| 18 | Fixed `PrivacyDashboardDialog` pre-existing `TweaksByCategory(string)` API mismatch | ✅ |
| 19 | All 1 671 tests passing (1 275 Core + 154 CLI + 242 GUI) | ✅ |

### Sprint 31 — Power & Energy Expansion ✅

| # | Task | Status |
|---|------|--------|
| 1 | `PowerSchedulerDialog` — time-window based automatic power plan switching with background `System.Threading.Timer` | ✅ |
| 2 | `SleepTimerDialog` — countdown/at-time Sleep, Hibernate, Shutdown, Monitor-Off with WinAPI `SendMessage` | ✅ |
| 3 | `BatterySaverDialog` — battery saver threshold `TrackBar` + registry R/W (`BatterySaverPercent`, `EnergySaverStatus`) | ✅ |
| 4 | `UsbPowerDialog` — USB selective suspend registry controls (`USB`, `usbhub`, `Control\USB`) with description panel | ✅ |
| 5 | All 4 dialogs registered in `Program.cs ResolveManagerArg()`: `powerscheduler`/`sleeptimer`/`batterysaver`/`usbpower` | ✅ |
| 6 | Tools menu: Power Plan Scheduler, Sleep/Hibernate Timer, Battery Saver, USB Power entries added | ✅ |
| 7 | 1 920 tests passing | ✅ |

### Sprint 32 — Privacy & Ad Removal ✅

| # | Task | Status |
|---|------|--------|
| 1 | `AdRemovalWizardDialog` — 14-item guided ad/tip removal (ContentDeliveryManager, news feed, Bing, search highlight) | ✅ |
| 2 | `TelemetryDashboardDialog` — 12-item telemetry dashboard (diagnostic data level, CEIP, activity history, error reporting, Cortana) | ✅ |
| 3 | `AppPermissionsDialog` — 16-item app capability manager via `HKLM AppPrivacy` policy keys (camera/mic/location/etc.) | ✅ |
| 4 | `DnsOverHttpsDialog` — DoH quick setup with 5 providers (Cloudflare, Google, Quad9, NextDNS, AdGuard) | ✅ |
| 5 | All 4 dialogs registered in `Program.cs ResolveManagerArg()`: `adremoval`/`telemetry`/`apppermissions`/`dnsoverhttps` | ✅ |
| 6 | Tools menu: Ad Removal Wizard, Telemetry Dashboard, App Permissions, DNS-over-HTTPS entries added | ✅ |
| 7 | 1 921 tests passing | ✅ |

### Sprint 33 — Network Tools Part 1 ✅

| # | Task | Status |
|---|------|--------|
| 1 | `NetworkRepairDialog` — 8-item repair wizard (DNS flush, IP release/renew, TCP auto-tuning, Winsock, TCP/IP stack, IPv6, Firewall reset) via `cmd.exe /c`; colour-coded log | ✅ |
| 2 | `DnsSwitcherDialog` — DNS quick-switch with 8 presets (Cloudflare, Cloudflare malware/family, Google, Quad9, OpenDNS, AdGuard, Comodo, DHCP auto-revert) + adapter picker | ✅ |
| 3 | `NetworkAdapterDialog` — adapter list with WMI `Win32_NetworkAdapter` + `NetworkInterface` fallback; enable/disable; ping gateway/Cloudflare/DNS diagnostics | ✅ |
| 4 | `WiFiProfileDialog` — Wi-Fi saved profile list via `netsh wlan show profiles`; export (key=clear), import XML, delete with confirm | ✅ |
| 5 | All 4 dialogs registered in `Program.cs ResolveManagerArg()`: `netrepair`/`dnsswitcher`/`netadapter`/`wifiprofiles` | ✅ |
| 6 | Tools menu: Network Repair Wizard, DNS Server Quick-Switch, Network Adapter Manager, Wi-Fi Profile Manager entries added | ✅ |
| 7 | Build: 0 errors, 0 warnings | ✅ |

### Sprint 34 — Network Tools Part 2 & System Tools ✅

| # | Task | Status |
|---|------|--------|
| 1 | `FirewallRulesDialog` — Windows Firewall rule viewer (inbound/outbound tabs) via `netsh advfirewall`; enable/disable rules; search filter; colour-coded Allow/Block | ✅ |
| 2 | `ProxyConfigDialog` — WinINet proxy R/W (`HKCU Internet Settings`): enable toggle, server, bypass list, local bypass; WinHTTP import-from-IE / reset-direct | ✅ |
| 3 | `ShellExtensionDialog` — Shell extension manager: enumerates `HKLM…Shell Extensions\Approved`; toggles enabled/disabled via `(disabled)` prefix; DLL path resolution | ✅ |
| 4 | `BootTimeAnalyzerDialog` — reads `Microsoft-Windows-Diagnostics-Performance/Operational` event log; Event ID 100 (boot history) + 101-103 (startup degradation); top slowdowns | ✅ |
| 5 | All 4 dialogs registered in `Program.cs ResolveManagerArg()`: `firewallrules`/`proxyconfig`/`shellextensions`/`bootanalyzer` | ✅ |
| 6 | Tools menu: Firewall Rules, Proxy Configuration, Shell Extension Manager, Boot Time Analyzer entries added | ✅ |
| 7 | Phase 5 items 44 (Boot Time Analyzer) and 46 (Shell Extension Manager) also completed | ✅ |
| 8 | Build: 0 errors, 0 warnings | ✅ |

### Phase 4 — Network & Connectivity Tools (Sprint 33–34) ✅

| # | Item | Priority | Source | Status |
|---|------|----------|--------|--------|
| 31 | One-click network repair wizard (TCP/IP, Winsock, DNS reset) | HIGH | NetAdapter Repair | ✅ Sprint 33 |
| 32 | DNS server quick-switch (Cloudflare, Google, Quad9, custom) | HIGH | Original | ✅ Sprint 33 |
| 33 | Network adapter diagnostics panel | MEDIUM | NetAdapter Repair | ✅ Sprint 33 |
| 34 | Wi-Fi profile management (export/import/delete) | MEDIUM | Original | ✅ Sprint 33 |
| 35 | Proxy configuration wizard | MEDIUM | Original | ✅ Sprint 34 |
| 36 | Firewall rule manager (simplified view of Windows Firewall) | MEDIUM | Original | ✅ Sprint 34 |
| 37 | Port scanner / connectivity tester | LOW | Original | 🔄 Future |
| 38 | Network bandwidth monitor | LOW | Original | 🔄 Future |
| 39 | VPN quick-connect from system tray | MEDIUM | Original | 🔄 Future |
| 40 | MAC address randomization toggle | LOW | Original | 🔄 Future |

### Phase 5 — Startup & Service Management (Sprint 29–30) ✅

| # | Item | Priority | Source | Status |
|---|------|----------|--------|--------|
| 41 | Startup manager — review and disable startup items | HIGH | MS PC Manager | ✅ Sprint 28 |
| 42 | Service manager — disable/enable Windows services with descriptions | HIGH | Original | ✅ Sprint 29 |
| 43 | Scheduled task manager — view and toggle system tasks | MEDIUM | Original | ✅ Sprint 28 |
| 44 | Boot time analyzer — identify slow-starting services | MEDIUM | Original | ✅ Sprint 34 |
| 45 | Context menu manager — add/remove/sort right-click items | MEDIUM | Original | ✅ Sprint 30 |
| 46 | Shell extension manager — enable/disable Explorer extensions | MEDIUM | Original | ✅ Sprint 34 |
| 47 | Installed programs quick-uninstaller | LOW | MS PC Manager | ✅ Sprint 30 |
| 48 | Temporary file cleaner with size preview | LOW | MS PC Manager | ✅ Sprint 30 |
| 49 | Windows Update pause/resume controls | LOW | Original | ✅ Sprint 35 |
| 50 | Driver update checker (optional components) | LOW | Original | 🔄 Pending |

### Phase 6 — Power & Energy Management (Sprint 31–32) ✅

| # | Item | Priority | Source | Status |
|---|------|----------|--------|--------|
| 51 | Power plan quick-switch from system tray | HIGH | AutoPowerOptionsOK | ✅ Sprint 29–30 |
| 52 | Timer-based power plan switching (e.g., gaming hours) | MEDIUM | AutoPowerOptionsOK | ✅ Sprint 31 |
| 53 | Custom power plan creator with presets | MEDIUM | Original | ✅ Sprint 31 |
| 54 | Battery saver automation (auto-enable at threshold) | MEDIUM | Original | ✅ Sprint 31 |
| 55 | Sleep/hibernate timer with countdown | LOW | Original | ✅ Sprint 31 |
| 56 | Monitor power-off timer | LOW | Original | ✅ Sprint 31 |
| 57 | USB selective suspend per-device control | LOW | Original | ✅ Sprint 31 |
| 58 | Wake-on-LAN configuration | LOW | Original | 🔄 Pending |
| 59 | Power consumption estimator (from current configuration) | LOW | Original | 🔄 Pending |
| 60 | Screen brightness scheduler (time-based) | LOW | Original | 🔄 Pending |

### Phase 7 — Privacy & Ad Removal (Sprint 32–34) ✅

| # | Item | Priority | Source | Status |
|---|------|----------|--------|--------|
| 61 | Desktop ad removal wizard — guided OFGB-like step-by-step flow | HIGH | OFGB | ✅ Sprint 32 |
| 62 | Pop-up/toolbar blocker for system notifications | HIGH | MS PC Manager | 🔄 Sprint 34 |
| 63 | Browser tracking protection overview (all installed browsers) | MEDIUM | Original | 🔄 Sprint 35 |
| 64 | Telemetry dashboard — visualize what data Windows sends | MEDIUM | Original | ✅ Sprint 32 |
| 65 | Privacy score — rate current system privacy level (0-100) | MEDIUM | Original | ✅ Sprint 29 |
| 66 | Hosts file manager — block domains via hosts file GUI | MEDIUM | Original | ✅ Sprint 30 |
| 67 | Cookie/cache cleaner for all installed browsers | LOW | Original | 🔄 Sprint 35 |
| 68 | DNS-over-HTTPS quick setup | LOW | Original | ✅ Sprint 32 |
| 69 | Location services granular control | LOW | Original | ✅ Sprint 32 |
| 70 | App permission manager (camera, microphone, location per-app) | LOW | Original | ✅ Sprint 32 |

### Phase 8 — Plugin & Extensibility Improvements (Sprint 35–36)

| # | Item | Priority | Source |
|---|------|----------|--------|
| 71 | Plugin sandboxing — isolated execution of third-party packs | HIGH | Original |
| 72 | Plugin auto-update — check marketplace for newer versions | HIGH | Original |
| 73 | Plugin rating and review system | MEDIUM | Original |
| 74 | Plugin dependency resolution (pack A requires pack B) | MEDIUM | Original |
| 75 | Plugin template generator (CLI command to scaffold a new pack) | MEDIUM | Original |
| 76 | Community plugin submission workflow (GitHub PR-based) | MEDIUM | Original |
| 77 | Plugin categories and tags in marketplace browser | LOW | Original |
| 78 | Plugin install from URL (direct .json download) | LOW | Original |
| 79 | Plugin changelog viewer in marketplace | LOW | Original |
| 80 | Plugin conflict detector (two packs modifying same registry keys) | LOW | Original |

### Phase 9 — Advanced Features & Automation (Sprint 37–38)

| # | Item | Priority | Source |
|---|------|----------|--------|
| 81 | Scheduled tweak application — apply tweaks on boot/login/timer | HIGH | Planned (P3) |
| 82 | Before/after preview — show what a tweak changes before applying | HIGH | ExplorerPatcher |
| 83 | Tweak rollback queue — undo last N operations with one click | MEDIUM | Original |
| 84 | Profile scheduler — auto-switch profiles by time or event | MEDIUM | Original |
| 85 | REST API for remote tweak management | MEDIUM | Planned (P3) |
| 86 | Web dashboard for tweak status visualization | MEDIUM | Planned (P3) |
| 87 | PowerShell module wrapper (`Install-Module RegiLattice`) | MEDIUM | Original |
| 88 | Group Policy export — generate .admx/.adml from tweak selections | LOW | Original |
| 89 | Intune/SCCM integration — deploy tweaks via MDM | LOW | Original |
| 90 | Tweak compliance reporting (drift detection from baseline) | LOW | Original |

### Phase 10 — Localization, Packaging & Community (Sprint 39–40)

| # | Item | Priority | Source |
|---|------|----------|--------|
| 91 | French locale (3rd language) | HIGH | Original |
| 92 | Spanish locale (4th language) | HIGH | Original |
| 93 | Japanese locale (5th language) | MEDIUM | Original |
| 94 | Chocolatey package submission | MEDIUM | Planned (P3) |
| 95 | Microsoft Store listing | MEDIUM | Original |
| 96 | Code signing for published binaries | MEDIUM | Planned (P2) |
| 97 | Auto-update mechanism (check GitHub Releases) | MEDIUM | Original |
| 98 | Portable mode (run from USB, no install) | LOW | Original |
| 99 | Community tweak submission form (web-based) | LOW | Original |
| 100 | Comprehensive user documentation site (mkdocs/docfx) | LOW | Original |

---

## Prioritized Backlog

### P0 — Critical (next sprint)

- [x] GitHub Actions CI workflow (.NET build + test)
- [x] Self-contained single-file publish
- [x] Async GUI operations (no UI thread blocking)

### P1 — High Value

- [x] Coverage reporting with coverlet
- [x] CLI test coverage
- [x] DPI-aware GUI scaling
- [x] Profile `RegisterBuiltins()` performance
- [x] Parallel `StatusMap()` optimization
- [x] winget manifest v3.2.0
- [x] GitHub Releases automation

### P2 — Medium Value

- [x] Snapshot round-trip tests
- [x] Integration tests with real registry
- [x] System theme auto-detection
- [x] Export to .REG from GUI
- [ ] Lazy module loading
- [x] Caching layer for computed properties
- [x] Scoop bucket entry
- [ ] Code signing
- [x] Plugin system (JSON Tweak Packs with marketplace)
- [x] User-defined tweaks via JSON

### P3 — Nice to Have

- [x] Tray icon with quick-access menu
- [ ] Scheduled tweak application
- [ ] REST API for remote management
- [ ] Web dashboard
- [x] Localization (German built-in locale)
- [ ] Chocolatey package submission
