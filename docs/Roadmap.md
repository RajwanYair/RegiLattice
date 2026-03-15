# RegiLattice — Roadmap

> Living document — updated after every sprint.
> Last updated: 2025-07-22 · v3.2.0 · 2 316 tweaks · 89 categories · 658 tests

---

## Current State (as of v3.2.0)

| Metric | Value |
|--------|-------|
| Language | C# 13 / .NET 10.0-windows (x64) |
| Tweaks | 2 316 verified across 89 categories |
| Tests | 658 (529 Core + 58 CLI + 71 GUI), all passing, 4-thread parallel |
| GUI | WinForms with 4 themes, system theme auto-detection, tray icon, percentage progress |
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

## Prioritized Backlog

### P0 — Critical (next sprint)

- [x] GitHub Actions CI workflow (.NET build + test)
- [ ] Self-contained single-file publish
- [x] Async GUI operations (no UI thread blocking)

### P1 — High Value

- [x] Coverage reporting with coverlet
- [x] CLI test coverage
- [x] DPI-aware GUI scaling
- [x] Profile `RegisterBuiltins()` performance
- [ ] Parallel `StatusMap()` optimization
- [ ] winget manifest v3.0.0
- [ ] GitHub Releases automation

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
