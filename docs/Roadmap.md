# RegiLattice — Roadmap

> Living document — updated after every sprint.
> Last updated: 2025-07-22 · v3.2.0 · 2 301 tweaks · 89 categories · 556 tests

---

## Current State (as of v3.2.0)

| Metric | Value |
|--------|-------|
| Language | C# 13 / .NET 10.0-windows (x64) |
| Tweaks | 1 981 verified across 72 categories |
| Tests | 203 (112 Core + 52 CLI + 39 GUI), all passing |
| GUI | WinForms with 4 themes (Catppuccin Mocha/Latte, Nord, Dracula) |
| Profiles | 5 (business, gaming, privacy, minimal, server) |
| NuGet | System.Management 9.0.3, xUnit 2.9.2, coverlet 6.0.2 |
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

### Sprint 1 — Test Coverage & CI Pipeline

| # | Task | Priority |
|---|------|----------|
| 1 | Set up GitHub Actions CI workflow for .NET (build + test on push/PR) | P0 |
| 2 | Add coverlet integration and coverage reporting | P1 |
| 3 | Increase Core.Tests to cover all TweakEngine edge cases | P1 |
| 4 | Add CLI argument parsing tests | P1 |
| 5 | Add snapshot round-trip tests | P2 |
| 6 | Add integration tests for RegistrySession (admin CI runner) | P2 |

### Sprint 2 — GUI Modernization

| # | Task | Priority |
|---|------|----------|
| 1 | Implement async tweak application (no UI freeze) via `Task.Run()` | P0 |
| 2 | Add progress bar for batch operations (apply profile, apply all) | P1 |
| 3 | Add system theme auto-detection (follow Windows dark/light mode) | P1 |
| 4 | Add tray icon with quick-access menu | P2 |
| 5 | Add tweak detail panel (description, registry keys, dependencies) | P2 |
| 6 | Add export to .REG file from GUI | P2 |
| 7 | DPI-aware scaling for high-resolution displays | P1 |

### Sprint 3 — Performance Optimization

| # | Task | Priority |
|---|------|----------|
| 1 | Profile `RegisterBuiltins()` startup time with BenchmarkDotNet | P1 |
| 2 | Implement lazy module loading for tweak categories | P2 |
| 3 | Optimize `StatusMap()` with parallel registry reads | P1 |
| 4 | Add caching layer for expensive operations (category counts, scope computation) | P2 |
| 5 | Reduce memory allocation in hot paths | P2 |

### Sprint 4 — Packaging & Distribution

| # | Task | Priority |
|---|------|----------|
| 1 | Self-contained single-file publish for CLI + GUI | P0 |
| 2 | Update winget manifest for v3.0.0 | P1 |
| 3 | Create Scoop bucket entry | P2 |
| 4 | GitHub Releases with auto-generated release notes | P1 |
| 5 | Automated build pipeline (build → test → publish on tag) | P1 |
| 6 | Code signing for published binaries | P2 |

### Sprint 5 — Advanced Features

| # | Task | Priority |
|---|------|----------|
| 1 | Plugin system: load custom `.dll` tweak modules | P2 |
| 2 | User-defined tweaks via JSON/TOML (no C# required) | P2 |
| 3 | Scheduled tweak application (apply on boot/login) | P3 |
| 4 | REST API layer for remote management | P3 |
| 5 | Web dashboard for tweak status visualization | P3 |
| 6 | Localization: add German locale as proof-of-concept | P3 |

---

## Prioritized Backlog

### P0 — Critical (next sprint)

- [ ] GitHub Actions CI workflow (.NET build + test)
- [ ] Self-contained single-file publish
- [ ] Async GUI operations (no UI thread blocking)

### P1 — High Value

- [ ] Coverage reporting with coverlet
- [ ] CLI test coverage
- [ ] DPI-aware GUI scaling
- [ ] Profile `RegisterBuiltins()` performance
- [ ] Parallel `StatusMap()` optimization
- [ ] winget manifest v3.0.0
- [ ] GitHub Releases automation

### P2 — Medium Value

- [ ] Snapshot round-trip tests
- [ ] Integration tests with real registry
- [ ] System theme auto-detection
- [ ] Export to .REG from GUI
- [ ] Lazy module loading
- [ ] Caching layer for computed properties
- [ ] Scoop bucket entry
- [ ] Code signing
- [ ] Plugin system (custom .dll modules)
- [ ] User-defined tweaks via JSON/TOML

### P3 — Nice to Have

- [ ] Tray icon with quick-access menu
- [ ] Scheduled tweak application
- [ ] REST API for remote management
- [ ] Web dashboard
- [ ] Localization (German proof-of-concept)
- [ ] Chocolatey package submission
