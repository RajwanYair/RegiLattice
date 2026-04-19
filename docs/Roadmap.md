# RegiLattice — Strategic Roadmap

> **Baseline**: v6.33.0 — 7,718 tweaks · 158 categories · 195 modules · 3,296 tests · 11 themes
> **Last updated**: 2026-04-11
> **Stack**: C# 13 / .NET 10.0-windows (x64) · WinForms · xUnit 2.9.3
> **Repository**: [github.com/RajwanYair/RegiLattice](https://github.com/RajwanYair/RegiLattice)

This document is a **ground-up strategic reassessment** of every major project decision —
language, framework, architecture, data layer, testing, distribution, documentation, and
ecosystem. It replaces the previous incremental roadmap with a consolidated plan that
questions existing choices, compares against best-in-class alternatives, harvests proven
patterns from competitors, and charts a clear path from v6.33 through v10+.

---

## Table of Contents

- [Part I — Completed Work](#part-i--completed-work-v60v633)
- [Part II — Competitive Landscape](#part-ii--competitive-landscape)
- [Part III — Strategic Decision Audit](#part-iii--strategic-decision-audit)
  - [Decision 1: Programming Language](#decision-1-programming-language--c-13-vs-alternatives)
  - [Decision 2: GUI Framework](#decision-2-gui-framework--winforms-vs-modern-alternatives)
  - [Decision 3: Architecture](#decision-3-architecture--monolith-vs-di--cqrs)
  - [Decision 4: Data Persistence](#decision-4-data-persistence--json-flat-files-vs-sqlite)
  - [Decision 5: Tweak Definition Format](#decision-5-tweak-definition-format--c-classes-vs-yamljson-data)
  - [Decision 6: Test Framework](#decision-6-test-framework--xunit-v2-vs-v3)
  - [Decision 7: CI/CD & Distribution](#decision-7-cicd--distribution)
  - [Decision 8: Documentation](#decision-8-documentation-strategy)
  - [Decision 9: Platform Scope](#decision-9-platform-scope--windows-only-vs-cross-platform)
  - [Decision 10: Installer Strategy](#decision-10-installer-strategy)
- [Part IV — Improvement Roadmap](#part-iv--improvement-roadmap)
  - [Phase A: Immediate Wins (v6.34–v6.39)](#phase-a-immediate-wins-v634v639)
  - [Phase B: Architecture Modernisation (v7.0)](#phase-b-architecture-modernisation-v70)
  - [Phase C: Data Layer Revolution (v7.1–v7.3)](#phase-c-data-layer-revolution-v71v73)
  - [Phase D: Frontend Rewrite (v8.0)](#phase-d-frontend-rewrite-v80)
  - [Phase E: Data-Driven Tweaks (v9.0)](#phase-e-data-driven-tweaks-v90)
  - [Phase F: Security, Trust & Ecosystem (v9.1+)](#phase-f-security-trust--ecosystem-v91)
- [Part V — Success Metrics](#part-v--success-metrics)
- [Part VI — Risk Register](#part-vi--risk-register)
- [Part VII — Migration Sequence](#part-vii--migration-sequence)
- [Part VIII — Appendix: Completed Phase Details](#part-viii--appendix-completed-phase-details-v60v633)

---

## Part I — Completed Work (v6.0–v6.33)

Seven development phases have been delivered since the Python→C# migration.
These represent thousands of engineering hours and should not be revisited:

| Phase | Focus | Version Range | Key Deliverables |
|-------|-------|---------------|-----------------|
| 1 | Engine Hardening | v6.14–v6.15 | Transactional apply, CancellationToken, TweakRisk flags, search ranking, custom profiles, recommendation engine |
| 2 | UI/UX | v6.18–v6.20 | 19 keyboard shortcuts, risk confirmation, batch ETA, 11-item context menu, JSON user themes with hot-reload |
| 3 | CLI & Integration | v6.16, v6.20 | `--json` output, conditional flags, interactive wizard, Ansible/DSC export |
| 4 | Test & Quality | v6.21 | 13 E2E tests, concurrent safety, GDI leak fixes, ShellRunner kill-on-timeout |
| 5 | Tweak Expansion | v6.22–v6.26 | +300 tweaks: security, gaming, accessibility, energy, developer, Office GP |
| 6 | Services | v6.27–v6.28 | Audit logging, HealthScore, ConflictDetector, ScheduledTweakService, TweakMigrationService |
| 7 | Ecosystem | v6.29–v6.30 | 10 locale stubs, 5 official packs, 22 PS cmdlets, pack-validation CI |

**Post-Phase 7** (v6.31–v6.33): +150 policy tweaks across 15 modules, RegistrySession.Backup()
DryRun fix, .runsettings HangTimeout fix.

---

## Part II — Competitive Landscape

### Competitor Comparison Matrix

| Feature | **RegiLattice** | **WinUtil** | **Sophia Script** | **Optimizer** | **BloatyNosy** | **Privatezilla** | **Win10Debloater** | **WPD** |
|---|---|---|---|---|---|---|---|---|
| **Language** | C# 13 / .NET 10 | PowerShell | PowerShell | C# / .NET FW 4.8 | C# + HTML/JS/PS | C# + PS | PowerShell | C++ (native) |
| **GUI** | WinForms | WPF/XAML | CLI + SophiApp (WinUI 3) | WinForms | WebView2 + native | WinForms | PS GUI | Win32 API |
| **Tweak count** | **7,718** | ~200 | ~150 functions | ~50 toggles | ~30 plugins | 60 settings | ~40 removals | ~30 toggles |
| **Stars** | ~0 (new) | **52.3k** | **9.2k** | **18.1k** | 5.6k | 3.7k | 18.8k | N/A |
| **CLI support** | 25+ commands | Partial | Full (PS7) | Silent mode | None | None | Switch params | CLI args |
| **Profiles** | 5 built-in + custom | 3 presets | None | Templates | None | None | 3 modes | None |
| **Undo/revert** | Per-tweak + snapshot | Limited | Per-function | Limited | None | Per-setting | Revert mode | None |
| **Package mgmt** | 5 managers (UI) | WinGet integration | None | None | None | None | None | None |
| **Themes** | 11 colour themes | System theme | N/A | Dark mode | 1 theme | 1 theme | N/A | Dark mode |
| **Plugin system** | JSON packs | None | None | None | Plugin-based | Community pkg | None | None |
| **ARM64** | No | No | **Yes** | No | No | No | No | No |
| **i18n** | 2 + 8 stubs | English only | Multi-lang | **24 languages** | English only | English only | English only | Multi-lang |
| **Code signing** | No | No | No | No | No | No | No | No |
| **Status** | Active | Active | Active | **Archived** | Active | Stale (3yr) | **Archived** | Stale (2022) |
| **License** | MIT | MIT | MIT | GPL-3.0 | MIT | MIT | MIT | Proprietary |
| **Contributors** | 1 | **239** | ~15 | ~20 | ~5 | 7 | 31 | 2 |
| **Install method** | EXE/MSI/MSIX | `irm | iex` | Scoop/Choco/WinGet | EXE download | EXE download | EXE download | PS download | ZIP portable |
| **Portable size** | ~40MB | ~50MB | ~2MB (script) | ~5MB | ~3MB | ~500KB | ~100KB | **335KB** |

### Key Insights to Harvest

**From WinUtil (52.3k stars)**:
- **One-liner install** (`irm christitus.com/win | iex`) — dramatically lowers adoption friction
- **WPF/XAML GUI** — modern, GPU-accelerated, MVVM-friendly; this is the industry direction
- **JSON-driven tweak definitions** — similar to RegiLattice's declarative pattern; validates our approach
- **Massive community** (239 contributors) — proves the market demand; we need contributor-friendliness

**From Sophia Script (9.2k stars)**:
- **ARM64 support** — forward-looking; Qualcomm Snapdragon X laptops are shipping now
- **Group Policy integration** — applied policies visible in `gpedit.msc`; our policy tweaks should verify this
- **Multi-distribution** (Scoop, Chocolatey, WinGet) — mirrors our package registry strategy
- **SophiApp 2.0 → C# + WinUI 3** — the PS→C# migration path validates our architectural choice

**From Optimizer (18.1k stars, archived)**:
- **24 languages** — proves demand for internationalisation in this space
- **Cautionary tale** — archived despite 18k stars; maintainer burnout kills projects. Keep scope tight
- **DNS/HOSTS management** — practical feature gap in RegiLattice

**From BloatyNosy (5.6k stars)**:
- **Plugin architecture** — validates our pack system direction
- **WebView2 hybrid GUI** — interesting alternative to WPF; HTML/CSS for layout, C# for logic
- **"No AI/no web crap" philosophy** — resonates with privacy-conscious users

**From WPD (335KB portable)**:
- **Ultra-lightweight** — challenges our 40MB self-contained binary
- **Windows API native** — no framework overhead; proves minimal footprint is possible
- **CLI argument support** — even the smallest tools need automation hooks

### Strategic Position

RegiLattice has **unmatched breadth** (7,718 tweaks vs next-best ~200) and **unmatched depth**
(profiles, snapshots, 25+ CLI commands, 5 package managers, 11 themes, plugin packs).
Our weaknesses are: **GUI antiquity** (WinForms), **binary size** (40MB), **community** (single dev),
**platform reach** (no ARM64), **discoverability** (0 stars vs competitors' thousands).

The roadmap must preserve our breadth advantage while modernising the stack to match the
best-in-class GUI (WinUtil's WPF), distribution (Sophia's multi-package), and community
experience (WinUtil's 239 contributors).

---

## Part III — Strategic Decision Audit

### Decision 1: Programming Language — C# 13 vs Alternatives

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **C# 13 / .NET 10** (current) | Strong Windows API interop, rich NuGet ecosystem, AOT compilation improving, excellent IDE tooling, type safety, `Microsoft.Win32.Registry` first-party | Windows-only for GUI, 40MB self-contained, no ARM64 yet | **KEEP** |
| Rust | Zero-cost abstractions, tiny binaries (~2MB), memory safety, cross-platform potential | No `winreg` parity with .NET, steep learning curve, rewrite = 12+ months | Reject |
| Go | Small binaries (~8MB), fast compilation, good Windows support | No GUI framework, no NuGet, weaker type system, manual registry interop | Reject |
| PowerShell | Zero dependency on user machines, inline `gpedit` visibility (Sophia), community scripts | No compiled speed, no type safety, no real GUI, debugging hell at scale | Reject (keep PS module as wrapper only) |
| C++ / Win32 | Ultra-small binary (WPD=335KB), direct API access, maximum performance | Maintenance nightmare, no modern tooling, memory unsafety, 10x dev time | Reject |

**Decision: RETAIN C# 13 / .NET 10.** The registry interop, NuGet ecosystem, and type safety
are irreplaceable at our scale (7,718 tweaks, 195 modules). The language is not the bottleneck —
the framework and architecture are.

**Action items**:
- [ ] Enable .NET Native AOT when WinForms support ships (tracked in dotnet/winforms#4649)
- [ ] Add ARM64 RID to publish matrix when .NET 10 ships ARM64 WinForms support
- [ ] Maintain PowerShell module as a thin CLI wrapper, not a rewrite target

---

### Decision 2: GUI Framework — WinForms vs Modern Alternatives

This is the **single highest-impact decision** in the project. WinForms was chosen for
migration speed (Python Tkinter → WinForms is a 1:1 mapping) but now limits us.

| Option | Pros | Cons | Migration Cost | Verdict |
|--------|------|------|----------------|---------|
| **WinForms** (current) | Working, stable, all features implemented, 11 themes | No MVVM, no GPU rendering, no vector icons, no XAML, bitmap scaling only, dated look | N/A | **MIGRATE AWAY** |
| **WPF** | MVVM, GPU-accelerated, XAML data binding, vector graphics, mature ecosystem, `WindowsFormsHost` for incremental migration, WinUtil proves it works | Windows-only, steeper learning curve, ~6 month migration | Medium (incremental via interop) | **ADOPT** |
| **WinUI 3** | Modern Fluent Design, recommended by Microsoft, SophiApp 2.0 uses it | Immature packaging (MSIX-only initially), fewer community packages, no `WindowsFormsHost` interop | High (full rewrite) | Defer |
| **Avalonia UI** | Cross-platform (Linux/macOS), XAML-like, growing ecosystem | Registry tweaks are Windows-only → cross-platform GUI adds complexity with no user benefit | High | Reject |
| **.NET MAUI** | Microsoft-backed cross-platform | Poor Windows desktop support, Blazor hybrid = web overhead, immature for desktop | High | Reject |
| **WebView2 (Electron-lite)** | Modern HTML/CSS layout (BloatyNosy uses this), flexible theming | Web overhead, Chromium dependency (~150MB), security surface, not native | Medium | Reject |
| **Spectre.Console TUI** | Terminal power users, zero GUI dependency, tiny binary | No mouse interaction, limited discoverability, niche audience | Low (additive) | **ADD as optional** |

**Decision: MIGRATE TO WPF** (Phase D, v8.0). Use `WindowsFormsHost` for incremental
migration — move one panel at a time. WinForms remains functional throughout. Additionally,
add a **Spectre.Console TUI** as a `--tui` CLI mode for terminal power users.

**Key WPF architecture decisions**:
- MVVM with `CommunityToolkit.Mvvm` (source generators for `ObservableProperty`, `RelayCommand`)
- `MaterialDesignThemes` or `HandyControl` for Fluent-style controls
- Data virtualization for the 7,718-tweak list (VirtualizingStackPanel)
- Theme system: XAML resource dictionaries (replace programmatic `Color` objects)

---

### Decision 3: Architecture — Monolith vs DI + CQRS

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **Monolithic TweakEngine** (current) | Simple, everything in one place, low ceremony | God class (3,000+ LOC), untestable in isolation, no interface contracts | **REFACTOR** |
| **DI + Interface Segregation** | Testable, mockable, SOLID, standard .NET pattern | Startup overhead (~10ms), more files, learning curve for contributors | **ADOPT** |
| **Full CQRS + MediatR** | Clean separation of reads/writes, event sourcing potential | Over-engineered for a desktop app, adds 5+ NuGet packages | **Reject** (too heavy) |
| **Vertical Slice Architecture** | Feature-focused folders, minimal cross-cutting | Doesn't fit our domain (tweaks are the domain, not features) | Reject |

**Decision: ADOPT DI with interface segregation.** Extract 6 interfaces from `TweakEngine`,
4 from `RegistrySession`. Use `Microsoft.Extensions.DependencyInjection` (already a transitive
dependency). The existing `TweakEngine` becomes a backward-compatible facade that delegates
to injected services.

**Extracted interfaces**:

```
ITweakRegistry       — Register(), AllTweaks(), GetTweak(), Categories()
ITweakSearch         — Search(), Filter(), TweaksByTag(), TweaksByScope()
ITweakExecutor       — Apply(), Remove(), ApplyBatch(), RemoveBatch()
ITweakStatus         — DetectStatus(), StatusMap()
ITweakValidator      — ValidateTweaks()
IProfileManager      — Profiles, GetProfile(), TweaksForProfile(), ApplyProfile()

IRegistryReader      — ReadDword(), ReadString(), KeyExists(), ValueExists()
IRegistryWriter      — SetDword(), SetString(), DeleteValue(), DeleteTree()
IRegistryExecutor    — Execute(), Evaluate()
IRegistryBackup      — Backup()
```

---

### Decision 4: Data Persistence — JSON Flat Files vs SQLite

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **8 JSON files** (current) | Human-readable, zero dependencies, easy debugging | No ACID, no queries, no indexing, file locks on OneDrive, scattered across 8 locations | **MIGRATE AWAY** |
| **SQLite** (`Microsoft.Data.Sqlite`) | ACID, structured queries, single file, migrations, indexing | Binary format (less debuggable), adds ~2MB, migration code needed | **ADOPT** |
| **LiteDB** | Document DB (JSON-like), LINQ queries, embedded, .NET native | Less mature than SQLite, limited community, no SQL standard | Consider as fallback |
| **SQL Server Express LocalDB** | Full SQL Server features | Requires installation, heavyweight, absurd for a desktop tool | Reject |

**Decision: MIGRATE TO SQLite.** Single `regilattice.db` file replaces:
- `config.json` → `config` table
- `favorites.json` → `favorites` table
- `ratings.json` → `ratings` table
- `history.json` → `history` table with indexed timestamps
- `analytics.json` → `analytics` table
- `compliance-history.json` → `compliance` table
- `snapshots/*.json` → `snapshots` table (BLOB storage)
- `themes/*.json` → `user_themes` table

**Migration strategy**: On first launch post-upgrade, detect JSON files → import into SQLite →
rename JSON files to `.json.migrated` (don't delete — user can manually revert).

---

### Decision 5: Tweak Definition Format — C# Classes vs YAML/JSON Data

This is the most **controversial** decision. Currently, 7,718 tweaks are defined across
195 C# files (~50K LOC). Each tweak is a `new TweakDef { ... }` object literal.

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **C# classes** (current) | Compile-time validation, IDE autocompletion, type safety, `ApplyAction` delegates for complex tweaks, refactoring support | 50K LOC of repetitive data, recompilation required for tweak changes, contributor barrier (must know C#) | **KEEP with improvements** |
| **YAML data files** | Human-editable, hot-reloadable, contributor-friendly (no C# knowledge), smaller codebase | Loses compile-time safety, `ApplyAction` delegates can't be expressed in YAML, JSON Schema validation is weaker than compiler | Partially adopt |
| **JSON data files** (WinUtil pattern) | Machine-parseable, widely understood, WinUtil validates this approach | Verbose for 7,718 entries, same limitations as YAML for delegates | Partially adopt |
| **Hybrid: YAML for registry-only, C# for delegates** | Best of both worlds — 95% of tweaks are pure `RegOp` and could be YAML | Two definition systems to maintain, loading complexity, ambiguous ownership | **ADOPT (long-term)** |

**Decision: KEEP C# as primary, ADOPT YAML as secondary format.** The ~5% of tweaks
with `ApplyAction`/`DetectAction` delegates (PowerShell, ServiceControl, SystemCommand, FileConfig,
PackageManager kinds) cannot be expressed in data files. For the ~95% that are pure
`ApplyOps`/`RemoveOps`/`DetectOps`, introduce **optional** YAML loading with JSON Schema
validation. This is additive, not a replacement.

**Immediate improvement for C# tweaks**: Eliminate the manual `RegisterBuiltins()` listing
by using **assembly scanning** with a `[TweakModule]` attribute or source generators.

---

### Decision 6: Test Framework — xUnit v2 vs v3

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **xUnit 2.9.3** (current) | Stable, all 3,296 tests pass, well-understood | EOL soon, missing features (generic test classes, `Assert.Equivalent`) | **MIGRATE** (when v3 stabilizes) |
| **xUnit 3.x** | Generic test classes, better parallelism, `IAsyncLifetime` improvements, `Assert.Equivalent` | Breaking changes in test class model, `FsCheck.Xunit` 2.x incompatible | **ADOPT in Phase A** |
| **NUnit 4.x** | Constraint model, rich assertion library, `TestContext` | Migration cost from xUnit, different assertion style, less ecosystem momentum | Reject |
| **MSTest 3.x** | Microsoft-backed, source generators, fast discovery | Least flexible, worst assertion library, no FsCheck integration | Reject |

**Decision: MIGRATE TO xUnit v3** in a dedicated sprint. Update all 5 held packages
simultaneously: `xunit` → 3.x, `xunit.runner.visualstudio` → 3.x, `FsCheck` → 3.x,
`FsCheck.Xunit` → 3.x, `Microsoft.NET.Test.Sdk` → 18.x.

---

### Decision 7: CI/CD & Distribution

**Current state**: 6 workflows (`ci.yml`, `release.yml`, `weekly.yml`, `smoke.yml`,
`pages.yml`, `packages.yml`) + 7 package registries (npm, maven, gem, winget, scoop,
chocolatey, PowerShell Gallery).

| Decision | Current | Proposed | Rationale |
|----------|---------|----------|-----------|
| Workflow count | 6 | **4** | Merge `smoke.yml` into `release.yml`, merge `pages.yml` into `ci.yml` |
| Package registries | 7 | **4** | Remove npm/maven/gem (no evidence of users); keep winget/scoop/chocolatey/PSGallery |
| Version bump process | 28 manual files | **1 script** | `Bump-Version.ps1` automates all 28 file updates |
| Code signing | None | **SignPath.io** | Free for OSS; required for Windows SmartScreen trust |
| One-liner install | None | **`irm regilattice.dev/install \| iex`** | Harvest from WinUtil; dramatically reduces adoption friction |
| Mutation testing | Weekly CI | **Weekly CI** | Keep; move fully off per-push trigger |

---

### Decision 8: Documentation Strategy

| Area | Current | Proposed | Rationale |
|------|---------|----------|-----------|
| Instruction files | 8 files (~15K words) | **5 files** | Merge `no-duplication` into `csharp`; merge `cicd` into `git-workflow`; convert enforceable rules to Roslyn analyzers |
| Skills | 10 skills | **12 skills** | Add `release-notes`, `perf-profiling` |
| README.md | ~500 lines with Mermaid diagrams | **~150 lines** + link to docs site | Focused quick-start; detailed docs on GitHub Pages |
| API docs | Manual `Api.md` | **DocFX auto-generated** | Source-of-truth from `///` XML comments |
| SVG assets | 7 manually-updated SVGs | **CI-templated SVGs** | `Bump-Version.ps1` does string substitution; CI validates counts match |
| CHANGELOG | Manual entries | **auto-generated from commits** | `git-cliff` or `standard-version` from Conventional Commits |

---

### Decision 9: Platform Scope — Windows-Only vs Cross-Platform

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **Windows x64 only** (current) | Focused, simple, all registry APIs available | Missing ARM64 laptops, no server-side use | **EXPAND to ARM64** |
| **Windows x64 + ARM64** | Covers Snapdragon X laptops, future-proof | Dual RID publish, untested on ARM64 | **ADOPT** |
| **Cross-platform (Linux/macOS)** | Wider audience | Registry tweaks are Windows-only — the entire value proposition is Windows | **Reject** |

**Decision: ADD ARM64** as a second RID in publish matrix. The Core library and CLI are
already architecture-neutral; only the GUI needs WinForms ARM64 testing.

---

### Decision 10: Installer Strategy

| Installer | Current | Proposed | Rationale |
|-----------|---------|----------|-----------|
| Portable EXE (GUI) | ✅ | ✅ Keep | Primary distribution; zero-install |
| Portable EXE (CLI) | ✅ | ✅ Keep | Automation/scripting use case |
| MSI (WiX) | ✅ | ✅ Keep | Enterprise deployment (GPO/SCCM) |
| MSIX | ✅ | ❌ Remove | Low adoption, complex packaging, Store not needed |
| WinGet manifest | ✅ | ✅ Keep | Primary package manager for Windows 11 |
| Scoop manifest | ✅ | ✅ Keep | Developer/power-user channel |
| Chocolatey | ✅ | ✅ Keep | Enterprise channel |
| npm/maven/gem | ✅ | ❌ Remove | Zero evidence of usage; maintenance burden |
| One-liner web install | ❌ | ✅ Add | Harvest from WinUtil; `irm ... \| iex` |

---

## Part IV — Improvement Roadmap

### Phase A: Immediate Wins (v6.34–v6.39)

> **Timeline**: Next 2–4 sprints · **Risk**: Low · **Impact**: High
> **Theme**: Clean house before building additions.

#### A.1 — Test Suite Consolidation (v6.34)

The test suite has accumulated structural debt:

- [ ] Split `ExtendedCoverageTests.cs` monolith (58 test classes, ~4K lines) into per-topic files
- [ ] Migrate remaining standalone test classes to use `BuiltinsFixture` (shared engine instance)
- [ ] Identify and remove ~100–200 tests that duplicate `TweakEngineBuiltinsTests` assertions
- [ ] Add `[Collection]` isolation to all file-writing test classes to prevent cross-assembly races
- [ ] Standardize all performance budget tests with tweak-count comments

**Measurable outcome**: Test count may decrease (dedup), but coverage stays ≥90%. Test run
time decreases by ~20% from reduced engine initialization.

#### A.2 — `.github/` Copilot Surface Overhaul (v6.35)

- [ ] Update all hardcoded counts in `copilot-instructions.md`, 8 instruction files, agent definition
- [ ] Add `Sync-CopilotInstructions.ps1` to auto-update counts from `TweakEngine.AllTweaks().Count`
- [ ] Add 2 new skills: `release-notes` (auto-draft CHANGELOG from commits), `perf-profiling`
- [ ] Add 3 new prompts: `review-pr`, `audit-categories`, `generate-pack`
- [ ] Consolidate instruction files: merge `no-duplication` → `csharp`, merge `cicd` → `git-workflow`
- [ ] Update agent mode routing (sprint/debug/review/release/explore)

#### A.3 — CI/CD Cleanup (v6.36)

- [ ] Merge `smoke.yml` post-release job into `release.yml`
- [ ] Merge `pages.yml` into `ci.yml` as a conditional job
- [ ] Delete npm/maven/gem package registry files and `packages.yml` GHCR job
- [ ] Create `Bump-Version.ps1` script that updates all 28 version-bearing files
- [ ] Add `paths-ignore` for docs-only changes to all workflows
- [ ] Pin all action versions to verified latest (see `cicd.instructions.md` canonical table)

#### A.4 — Scope Discipline: Extract Utility Dialogs (v6.37–v6.38)

Audit the 67+ dialog/form classes and extract non-core ones to `RegiLattice.Tools`:

**Core (keep in GUI — ~30 classes)**:
- MainForm, AboutDialog, ConfirmApplyDialog, KeyboardShortcutsDialog
- All 5 package manager dialogs + BasePackageManagerDialog
- MarketplaceDialog, ToolVersionsDialog, WindowsHealthDialog
- Theme-related, TweakBrowserPanel, TweakCardRow, all Controls/*

**Tools (extract to plugin DLL — ~35 classes)**:
- Battery/power monitoring dialogs
- Network tools (port scanner, DNS lookup, ping, traceroute)
- System tools (memory cleaner, disk analyzer, startup manager GUI)
- Hardware info detail dialogs beyond the core AboutDialog

- [ ] Create `src/RegiLattice.Tools/RegiLattice.Tools.csproj` (class library)
- [ ] Move extracted dialog classes to Tools project
- [ ] Add `RegiLattice.Tools.dll` as optional plugin loaded by MainForm
- [ ] Update tests to reference the new project structure

#### A.5 — xUnit v3 Migration (v6.39)

- [ ] Update all 5 held packages simultaneously in `Directory.Packages.props`
- [ ] Fix breaking API changes (test class model, `Assert` namespace)
- [ ] Update `FsCheck.Xunit` to v3-compatible version
- [ ] Update `.runsettings` for v3 test host
- [ ] Verify all 3,296+ tests pass on the new framework

---

### Phase B: Architecture Modernisation (v7.0)

> **Timeline**: 1 MAJOR version · **Risk**: Medium · **Impact**: High
> **Theme**: Replace the God class with testable, injectable services.

#### B.1 — Dependency Injection Container

```csharp
// Before: God class, everything static-ish
var engine = new TweakEngine();
engine.RegisterBuiltins();
var results = engine.Search("privacy");

// After: DI with interface contracts
var services = new ServiceCollection()
    .AddRegiLatticeCore()        // extension method registers all services
    .AddSingleton<IRegistrySession>(new RegistrySession { DryRun = true })
    .BuildServiceProvider();

var search = services.GetRequiredService<ITweakSearch>();
var results = search.Search("privacy");
```

- [ ] Extract 6 interfaces from `TweakEngine` (see Decision 3 table)
- [ ] Extract 4 interfaces from `RegistrySession`
- [ ] Create `ServiceCollectionExtensions.AddRegiLatticeCore()` registration
- [ ] Keep `TweakEngine` as a backward-compatible facade
- [ ] Update GUI and CLI to use DI container
- [ ] Update all tests to inject mock interfaces where beneficial

#### B.2 — Auto-Registration via `[TweakModule]` Attribute

Eliminate the manual `RegisterBuiltins()` method that lists all 195 module classes:

```csharp
// Before: manual listing in TweakEngine.RegisterBuiltins()
Register(Privacy.Tweaks);
Register(Performance.Tweaks);
Register(Security.Tweaks);
// ... 192 more lines

// After: assembly scanning
[TweakModule]
internal static class Privacy
{
    public static IReadOnlyList<TweakDef> Tweaks => [ ... ];
}

// TweakEngine discovers all [TweakModule] classes via reflection at startup
```

- [ ] Define `[TweakModule]` attribute
- [ ] Implement assembly scanning in `TweakEngine.RegisterBuiltins()` (or source generator)
- [ ] Remove manual registration lines
- [ ] Add startup benchmark to verify scanning cost is <50ms

#### B.3 — Event-Driven Architecture (Optional, Lightweight)

Replace direct method calls for cross-cutting concerns with a simple event bus:

```csharp
// Events raised by engine operations
public sealed record TweakAppliedEvent(string Id, TweakResult Result, TimeSpan Duration);
public sealed record TweakRemovedEvent(string Id, TweakResult Result);
public sealed record StatusMapCompletedEvent(int TweakCount, TimeSpan Duration);

// Subscribers: Analytics, TweakHistory, HealthScore, UI status bar
engine.Events.Subscribe<TweakAppliedEvent>(e => analytics.RecordApply(e.Id));
```

- [ ] Define event types as sealed records
- [ ] Implement minimal `IEventBus` (publish/subscribe, no external dependency)
- [ ] Wire Analytics, TweakHistory, HealthScore as subscribers
- [ ] Update GUI to subscribe to progress events instead of polling

---

### Phase C: Data Layer Revolution (v7.1–v7.3)

> **Timeline**: 3 MINOR versions · **Risk**: Medium-High · **Impact**: High
> **Theme**: Replace 8 scattered JSON files with a single SQLite database.

#### C.1 — SQLite Foundation (v7.1)

- [ ] Add `Microsoft.Data.Sqlite` (already a well-known package, ~200KB)
- [ ] Design schema: `config`, `favorites`, `ratings`, `history`, `analytics`,
  `compliance`, `snapshots`, `user_themes`, `user_profiles`, `packs`
- [ ] Implement `DbMigrationRunner` with versioned migration scripts
- [ ] Create `IRegiLatticeDb` interface with connection management
- [ ] Implement automatic JSON→SQLite migration on first launch

#### C.2 — Repository Pattern (v7.2)

```csharp
public interface IFavoritesRepository
{
    bool IsFavorite(string tweakId);
    void Add(string tweakId);
    void Remove(string tweakId);
    IReadOnlyList<string> All();
}

// SQLite implementation
internal sealed class SqliteFavoritesRepository : IFavoritesRepository
{
    private readonly IRegiLatticeDb _db;
    // ACID transactions, indexed lookups, no file-lock issues
}
```

- [ ] Create repository interfaces for all 8 data services
- [ ] Implement SQLite-backed repositories
- [ ] Update all service classes to use repository interfaces (DI)
- [ ] Remove direct `File.ReadAllText`/`File.WriteAllText` calls from services

#### C.3 — In-Memory Cache with Change Notifications (v7.3)

- [ ] Implement `ICache<TKey, TValue>` with `MemoryCache`-backed storage
- [ ] Add `IChangeNotifier` interface for cache invalidation
- [ ] Cache frequently-accessed data: `AllTweaks()`, `Categories()`, `StatusMap()`
- [ ] Wire `FileSystemWatcher` for external SQLite changes (multi-instance scenario)

---

### Phase D: Frontend Rewrite (v8.0)

> **Timeline**: 1 MAJOR version, 3–6 months · **Risk**: High · **Impact**: Very High
> **Theme**: Replace WinForms with WPF for a modern, accessible, GPU-accelerated UI.

#### D.1 — WPF Shell + WinForms Interop

```
┌─────────────────────────────────────────────────┐
│  WPF MainWindow (v8.0)                          │
│  ┌───────────────┐  ┌────────────────────────┐  │
│  │  WPF Sidebar   │  │  WindowsFormsHost      │  │
│  │  (categories,  │  │  (existing TweakPanel)  │  │
│  │   profiles,    │  │  Migrated to WPF in    │  │
│  │   search)      │  │  v8.1, v8.2, v8.3...   │  │
│  └───────────────┘  └────────────────────────┘  │
│  ┌────────────────────────────────────────────┐  │
│  │  WPF Log Panel                              │  │
│  └────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────┘
```

- [ ] Create `RegiLattice.WPF` project (new project, not rename)
- [ ] Implement WPF `MainWindow` with `WindowsFormsHost` hosting existing panels
- [ ] Migrate sidebar (category tree) to WPF TreeView with data binding
- [ ] Implement MVVM `TweakListViewModel` with `CommunityToolkit.Mvvm`
- [ ] Data-virtualized `ListView` for 7,718 tweaks (VirtualizingStackPanel)
- [ ] XAML resource dictionaries for all 11 themes

#### D.2 — Incremental Panel Migration (v8.1–v8.5)

Each minor version migrates one panel from WinForms to native WPF:

| Version | Panel | Key Challenge |
|---------|-------|--------------|
| v8.1 | TweakBrowserPanel → WPF DataGrid | Data virtualization for 7,718 rows |
| v8.2 | TweakCardRow → WPF UserControl | Custom rendering, scope badges |
| v8.3 | Package manager dialogs → WPF | Template method pattern in MVVM |
| v8.4 | Settings/About dialogs → WPF | Final WinForms dependency removal |
| v8.5 | Remove WinForms project reference | Clean break; `RegiLattice.GUI` → archive |

#### D.3 — WCAG 2.1 AA Accessibility

- [ ] Keyboard navigation for all controls (Tab order, arrow keys in lists)
- [ ] Screen reader support via UI Automation (WPF provides this natively)
- [ ] High-contrast theme support (detect Windows high-contrast mode)
- [ ] Minimum 4.5:1 contrast ratio on all text (audit all 11 themes)
- [ ] Focus indicators on all interactive elements

#### D.4 — Spectre.Console TUI (Optional, additive)

```
$ regilattice --tui

┌─ RegiLattice v8.0 ────────────────────────────────────────┐
│ Search: telemetry_                                         │
│                                                            │
│ ▸ Privacy (142 tweaks)                                     │
│   ☐ priv-disable-telemetry         Disable Telemetry       │
│   ☑ priv-disable-ad-id             Disable Advertising ID  │
│   ☐ priv-disable-activity-feed     Disable Activity Feed   │
│                                                            │
│ ▸ Telemetry Advanced (48 tweaks)                           │
│   ☐ telem-disable-ceip             Disable CEIP            │
│                                                            │
│ [Space] Toggle · [Enter] Apply · [/] Search · [q] Quit    │
└────────────────────────────────────────────────────────────┘
```

- [ ] Add `Spectre.Console` NuGet package
- [ ] Implement `--tui` CLI command that launches an interactive terminal UI
- [ ] Category tree, search, tweak toggle, status display
- [ ] Profile selector and batch operations

---

### Phase E: Data-Driven Tweaks (v9.0)

> **Timeline**: 1 MAJOR version · **Risk**: Medium · **Impact**: Medium
> **Theme**: Allow non-developers to contribute tweaks without writing C#.

#### E.1 — YAML Tweak Definitions (Registry-Only)

```yaml
# tweaks/privacy/disable-telemetry.yaml
id: priv-disable-telemetry
label: Disable Telemetry
category: Privacy
tags: [telemetry, privacy, data-collection]
needs-admin: true
corp-safe: false
impact-score: 5
safety-rating: 4
impact-note: Stops all diagnostic and usage data from being sent to Microsoft.
source-url: https://learn.microsoft.com/en-us/windows/privacy/

apply:
  - set-dword: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: AllowTelemetry, value: 0 }
  - set-dword: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: MaxTelemetryAllowed, value: 0 }

remove:
  - delete-value: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: AllowTelemetry }
  - delete-value: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: MaxTelemetryAllowed }

detect:
  - check-dword: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: AllowTelemetry, expected: 0 }
```

- [ ] Define YAML schema with JSON Schema validation (`tweaks.schema.json`)
- [ ] Implement `YamlTweakLoader` that converts YAML files to `TweakDef` objects
- [ ] Load YAML tweaks alongside C# tweaks in `RegisterBuiltins()`
- [ ] Support hot-reload: `FileSystemWatcher` on `tweaks/` directory
- [ ] CLI command: `--validate-yaml <path>` for pack/community tweak validation

#### E.2 — JSON Schema for Tweak Validation

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "type": "object",
  "required": ["id", "label", "category", "apply"],
  "properties": {
    "id": { "type": "string", "pattern": "^[a-z][a-z0-9-]+$" },
    "label": { "type": "string", "minLength": 3 },
    "category": { "type": "string" },
    "impact-score": { "type": "integer", "minimum": 1, "maximum": 5 },
    "safety-rating": { "type": "integer", "minimum": 1, "maximum": 5 }
  }
}
```

- [ ] Publish schema to `schemas/tweak.schema.json` in the repository
- [ ] Add VS Code `yaml.schemas` setting for auto-validation in editor
- [ ] Integrate schema validation into `--validate` CLI command
- [ ] Add schema to pack validation CI workflow

#### E.3 — Gradual C# → YAML Migration

- [ ] Identify "pure RegOp" tweaks (no `ApplyAction`/`DetectAction`) — expected ~95% of 7,718
- [ ] Auto-generate YAML from C# `TweakDef` objects: `--export-yaml <directory>`
- [ ] Validate round-trip: C# → YAML → load → compare (`TweakDef.Equals()`)
- [ ] Migrate 1 category per sprint as proof of concept
- [ ] Keep C# modules for the ~5% with delegate-based logic

---

### Phase F: Security, Trust & Ecosystem (v9.1+)

> **Timeline**: Ongoing · **Risk**: Low · **Impact**: Medium
> **Theme**: Earn user trust and grow community.

#### F.1 — Code Signing (v9.1)

- [ ] Register with [SignPath.io](https://signpath.io/) (free for OSS)
- [ ] Add signing step to `release.yml` after build, before upload
- [ ] Sign all EXE, DLL, and MSI artifacts
- [ ] Document certificate pinning for `winget` and Chocolatey manifests

#### F.2 — Lazy Admin Elevation (v9.1)

Currently, the CLI checks `IsAdmin()` once at startup. Many HKCU tweaks don't need admin.

- [ ] Categorize tweaks by `Scope`: User (no admin), Machine (admin), Both (admin)
- [ ] In GUI: apply User tweaks immediately; prompt UAC only when Machine tweaks are selected
- [ ] In CLI: `--no-elevate` flag to skip Machine tweaks silently
- [ ] Use `runas` verb for per-operation elevation instead of requiring full-admin launch

#### F.3 — SBOM & Reproducible Builds (v9.2)

- [ ] Generate CycloneDX SBOM in `release.yml` (`dotnet-cyclonedx` tool)
- [ ] Attach `sbom.cdx.json` to every GitHub Release
- [ ] Enable Source Link (`<EmbedAllSources>true</EmbedAllSources>`)
- [ ] Verify deterministic builds with `<Deterministic>true</Deterministic>` (already set)

#### F.4 — Community & Contribution Experience (v9.3+)

- [ ] Create `Setup-Dev.ps1` — one-command dev environment bootstrap
- [ ] Curate 10+ "Good First Issues" with detailed descriptions and file pointers
- [ ] Publish `RegiLattice.SDK` NuGet package for third-party pack authors
- [ ] Add `ARCHITECTURE.md` with data flow diagrams and onboarding guide
- [ ] Set up GitHub Discussions for community Q&A
- [ ] One-liner install script: `irm regilattice.dev/install | iex`

#### F.5 — Internationalisation (v9.4+)

Currently: 2 real locales (en, de) + 8 stubs.

- [ ] Evaluate Crowdin (free for OSS) vs manual `.resx` management
- [ ] Prioritize top 5 languages by Windows market share: en, zh-CN, es, de, ja
- [ ] Extract all UI strings to `.resx` resources (currently some are inline)
- [ ] Harvest from Optimizer's 24-language approach (community PRs for translations)

#### F.6 — Watch Mode & Batch Scripting (v9.5+)

- [ ] CLI `--watch` mode: monitor registry for tweak drift, alert on reversion
- [ ] CLI `--batch-file <yaml>`: YAML deployment recipes for enterprise use

```yaml
# deploy-privacy.yaml
profile: privacy
additional:
  - priv-disable-telemetry
  - telem-disable-ceip
skip:
  - priv-disable-cortana  # needed for accessibility
mode: apply
dry-run: false
```

---

## Part V — Success Metrics

| Metric | Current (v6.33) | Phase A Target | Phase D Target | Phase E Target |
|--------|----------------|----------------|----------------|----------------|
| **Tweaks** | 7,718 | 7,718 | 7,718 | 8,000+ |
| **GUI framework** | WinForms | WinForms | WPF | WPF |
| **Architecture** | Monolithic | Monolithic | DI + interfaces | DI + interfaces |
| **Data persistence** | 8 JSON files | 8 JSON files | SQLite | SQLite + cache |
| **Tweak format** | 195 C# files | 195 C# files | 195 C# files | Hybrid C#/YAML |
| **Tests** | 3,296 (xUnit v2) | 3,200+ (xUnit v3) | 3,500+ | 4,000+ |
| **CI workflows** | 6 | 4 | 4 | 4 |
| **Version bump files** | 28 manual | 1 script | 1 script | 1 script |
| **Package registries** | 7 | 4 | 4 | 4 |
| **Dialogs in main app** | 67+ | ~30 | ~30 | ~25 |
| **Code signing** | None | None | SignPath.io | SignPath.io |
| **ARM64 support** | No | CI only | Published | Published |
| **Locales** | 2 (en, de) | 2 | 5 | 5+ (Crowdin) |
| **DPI support** | Bitmap scaling | Bitmap scaling | WPF vector | WPF vector |
| **a11y compliance** | None | None | WCAG 2.1 AA | WCAG 2.1 AA |
| **Startup time** | ~200ms | ~180ms | ~100ms (lazy) | ~50ms |
| **Binary size (portable)** | ~40MB | ~38MB | ~35MB | ~30MB |
| **Community contributors** | 1 | 1 | 5+ | 10+ |
| **One-liner install** | No | No | Yes | Yes |

---

## Part VI — Risk Register

| ID | Risk | Impact | Probability | Mitigation |
|----|------|--------|------------|------------|
| R1 | **WPF migration takes longer than expected** | Months of dual-framework maintenance | High | Incremental migration via `WindowsFormsHost`; WinForms stays functional throughout; each panel is a separate PR |
| R2 | **SQLite migration corrupts user data** | Users lose favorites, history, config | Medium | JSON backup before migration; `.json.migrated` files kept; automated rollback on failure; extensive migration tests |
| R3 | **YAML tweak format has edge cases** | Broken tweaks, wrong registry values | Medium | Keep C# as authoritative; YAML is additive. Comprehensive round-trip tests: C# → YAML → load → diff |
| R4 | **xUnit v3 breaks 3,296 tests** | CI blocked for days | Medium | Migrate in a branch; one test project at a time; hold `FsCheck.Xunit` v3 until verified compatible |
| R5 | **Scope reduction upsets users** | Feature regression perception | Medium | Extract to downloadable plugin DLL, not deleted. Announce in CHANGELOG one version before |
| R6 | **DI container adds startup overhead** | Slower cold start | Low | Benchmark before/after; typical DI registration is ~10ms; lazy module loading offsets it |
| R7 | **Code signing certificate compromise** | Broken release pipeline, trust loss | Low | Auto-renew via SignPath; revocation procedure documented; HSM-backed key storage |
| R8 | **Maintainer burnout** (Optimizer cautionary tale) | Project abandoned | Medium | Scope discipline (Phase A.4); attract contributors (Phase F.4); automate everything possible |
| R9 | **ARM64 WinForms breaks at runtime** | Broken ARM64 builds | Low | Add ARM64 CI runner; test on Snapdragon X devkit before publishing |
| R10 | **npm/maven/gem removal breaks downstream** | Unknown users lose install method | Low | Announce deprecation in CHANGELOG; add redirect note to package README |

---

## Part VII — Migration Sequence

```
v6.34  ─ Phase A.1  Test Suite Consolidation
v6.35  ─ Phase A.2  Copilot Surface Overhaul
v6.36  ─ Phase A.3  CI/CD Cleanup
v6.37  ─ Phase A.4a Scope Discipline (audit + plan)
v6.38  ─ Phase A.4b Scope Discipline (extract RegiLattice.Tools)
v6.39  ─ Phase A.5  xUnit v3 Migration
         ↓
v7.0   ─ Phase B.1  DI Container + Interface Segregation     ← MAJOR (breaking: new service API)
v7.1   ─ Phase B.2  Auto-Registration + Assembly Scanning
v7.2   ─ Phase C.1  SQLite Foundation + Migration
v7.3   ─ Phase C.2  Repository Pattern
v7.4   ─ Phase C.3  In-Memory Cache
         ↓
v8.0   ─ Phase D.1  WPF Shell + WinForms Interop             ← MAJOR (new UI framework)
v8.1   ─ Phase D.2a TweakBrowserPanel → WPF
v8.2   ─ Phase D.2b TweakCardRow → WPF
v8.3   ─ Phase D.2c Package Manager Dialogs → WPF
v8.4   ─ Phase D.2d Settings/About → WPF
v8.5   ─ Phase D.2e Remove WinForms dependency
         ↓
v9.0   ─ Phase E.1  YAML Tweak Definitions                   ← MAJOR (new tweak format)
v9.1   ─ Phase F.1  Code Signing + F.2 Lazy Elevation
v9.2   ─ Phase F.3  SBOM + Reproducible Builds
v9.3   ─ Phase F.4  Community + Contribution Experience
v9.4   ─ Phase F.5  Internationalisation (Crowdin)
v9.5+  ─ Phase F.6  Watch Mode + Batch Scripting
```

**Key principles**:

1. **Each phase is independently valuable.** No phase requires all prior phases.
2. **MAJOR bumps are reserved for breaking changes**: DI API (v7.0), WPF (v8.0), YAML (v9.0).
3. **Phase A is entirely backward-compatible** — pure cleanup, no breaking changes.
4. **WinForms remains functional** throughout Phase D migration (interop layer).
5. **C# tweak modules remain authoritative** even after YAML support ships (Phase E).
6. **Highest-ROI phases first**: Test cleanup → CI cleanup → scope reduction → DI → SQLite.

---

## Part VIII — Appendix: Completed Phase Details (v6.0–v6.33)

<details>
<summary>Click to expand completed phase specifications</summary>

### Phase 1 — Engine & Model Hardening (v6.14–v6.15)

- **1.1 Transactional Apply**: `ApplyBatch(transactional: true)` with auto-rollback on failure
- **1.2 CancellationToken**: Added to `StatusMap`, `ApplyBatch`, `RemoveBatch`, `Search`, `ValidateTweaks`, `Filter`
- **1.3 TweakRisk Flags**: `[Flags] enum TweakRisk` with 8 flags, auto-detected from `ApplyOps`
- **1.4 Registry Diff**: `ExecuteWithDiff()` returns before/after values for every registry write
- **1.5 Search Ranking**: 8-tier relevance scoring (ID exact match → synonym match)
- **1.6 Custom Profiles**: `CreateProfile()`, `SaveProfile()`, `DeleteProfile()`, `UserProfiles()`
- **1.7 Recommendation Engine**: `RecommendTweaks()` with confidence percentages and `IsQuickWin`

### Phase 2 — UI/UX & Accessibility (v6.18–v6.20)

- **2.2 Keyboard Shortcuts**: 19 shortcuts, 4 groups, `KeyboardShortcutsDialog`
- **2.3 Confirm Apply Dialog**: `ConfirmApplyDialog` + `ConfirmApplyThreshold` for risk-rated tweaks
- **2.4 Batch ETA**: `TweakDef.EstimatedApplyTimeMs` per-kind, exponential moving average display
- **2.5 Context Menu**: 11 items (Apply/Remove/Favorite/CopyRegPath/OpenRegedit/Dependencies/History)
- **2.6 User Themes**: JSON themes in `%LOCALAPPDATA%\RegiLattice\themes\`, `FileSystemWatcher` hot-reload

### Phase 3 — CLI & Integration (v6.16, v6.20)

- **3.1 JSON Output**: `--json` flag, `CliJsonContext` source-generated serializer
- **3.3 Conditional Flags**: `--if-not-applied`, `--if-admin`, `--if-build`, `--if-hardware`, `--if-not-corp`
- **3.4 Interactive Wizard**: `--wizard` command with 3-question profile recommender
- **3.6 Export Formats**: Ansible `win_regedit` YAML, DSC `.ps1` export

### Phase 4 — Test & Quality (v6.21)

- **4.1 E2E Tests**: 13 scenarios covering full lifecycle, profiles, DryRun, snapshots, JSON export, dep chain, CorporateGuard, concurrent operations
- **4.6 Concurrent Safety**: 10 concurrent `StatusMap()` + 5 concurrent `ApplyBatch()` in DryRun mode

### Phase 5 — Tweak Expansion (v6.22–v6.26)

- **5.1 Security**: WDAG, Printer, LSA, MSI, NTP, WinRM, CredGuard, IE Zones (+80)
- **5.2 Gaming**: DirectStorage, VRR, Latency, GPU Power, Network Opt, Audio Opt (+60)
- **5.3 Accessibility**: Motor, Visual, Magnifier, LiveCaptions, EyeControl, VoiceAccess (+40)
- **5.4 Energy**: BatterySaver, Charging, Standby, CPUPower, DisplayPower (+50)
- **5.5 Developer**: WinDbg, WSLAdvanced, GitCredManager, ContainerRuntime (+70)
- **Office GP**: Word, Excel, Outlook, PowerPoint, Access, Publisher, Visio, Project (+80)

### Phase 6 — Services & Intelligence (v6.27–v6.28)

- **6.1 Audit Logging**: `Username`, `MachineName`, `SessionId` on `HistoryEntry` + `ExportCsv()`
- **6.2 Health Scores**: `CategoryHealthScore` record with per-category breakdown
- **6.3 Conflict Detection**: `ConflictDetector.DetectRegistryConflicts()` with `ConflictSeverity`
- **6.4 Scheduling**: `ScheduledTweakService` with per-tweak `ScheduleTrigger`
- **6.5 Migration**: `TweakMigrationService` + `TweakEngine.Migrations` + `SnapshotManager` auto-migrate

### Phase 7 — Internationalisation & Ecosystem (v6.29–v6.30)

- **7.1 Locales**: 10 locale stubs documented (en, de, fr, es, ja, zh-CN, ko, pt, it, ru)
- **7.2 Official Packs**: 5 packs in `packs/` (gaming-fps, privacy-extreme, enterprise-soc2, developer-full, accessibility-inclusive)
- **7.3 Pack Authoring**: `docs/PackAuthoring.md` with schema, examples, publishing workflow
- **7.4 PowerShell Module**: 22 cmdlets + 16 aliases in `RegiLattice.psm1`/`.psd1`
- **7.5 Pack Validation CI**: `pack-validation.yml` reusable workflow

### Post-Phase 7 (v6.31–v6.33)

- **v6.31**: 5 new policy modules (PolicyWindowsFeedback/PolicySettingSync/PolicyWindowsRAHardening/PolicyWindowsSecCenter/PolicyDeliveryOpt); fixed 3 duplicate tweak IDs
- **v6.32**: 5 new policy modules (PolicyBITS/PolicyPersonalization/PolicyTabletPC/PolicyWindowsBackup/PolicyGameDVR); fix RegistrySession.Backup() DryRun short-circuit
- **v6.33**: 5 new policy modules (PolicyWindowsDefenderATP/PolicyWindowsInstaller/PolicyCryptography/PolicyFVE/PolicyWindowsUpdateAU)

**Totals at v6.33.0**: 7,718 tweaks · 158 categories · 195 modules · 3,296 tests

</details>
