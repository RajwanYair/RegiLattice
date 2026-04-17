# RegiLattice — Project Roadmap

> Strategic improvement plan — rethinking every layer of the project for best-in-class quality.
> Consolidates completed phases 1–7, then proposes a new forward-looking architecture.
> Baseline: **v6.33.0** — 7,718 tweaks · 158 categories · 195 files · 3,296 tests · 11 themes

---

## Table of Contents

- [Completed Work (v6.0–v6.33)](#completed-work-v60v633)
- [Strategic Assessment](#strategic-assessment)
- [Phase 8 — Architecture Modernisation](#phase-8--architecture-modernisation)
- [Phase 9 — Data Layer & Persistence](#phase-9--data-layer--persistence)
- [Phase 10 — Frontend Revolution](#phase-10--frontend-revolution)
- [Phase 11 — Scope Discipline & Feature Focus](#phase-11--scope-discipline--feature-focus)
- [Phase 12 — Build, CI/CD & Distribution](#phase-12--build-cicd--distribution)
- [Phase 13 — Quality, Testing & Observability](#phase-13--quality-testing--observability)
- [Phase 14 — Documentation & Developer Experience](#phase-14--documentation--developer-experience)
- [Phase 15 — Data-Driven Tweaks](#phase-15--data-driven-tweaks)
- [Phase 16 — Security & Trust](#phase-16--security--trust)
- [Phase 17 — Ecosystem & Community](#phase-17--ecosystem--community)
- [Success Metrics](#success-metrics)
- [Risk Register](#risk-register)
- [Migration Sequence](#migration-sequence)

---

## Completed Work (v6.0–v6.33)

All seven original phases have been delivered. This section is a consolidated summary.

| Phase | Focus | Versions | Key Deliverables |
|-------|-------|----------|------------------|
| **1** | Engine Hardening | v6.14–v6.15 | Transactional apply with rollback, CancellationToken on all APIs, TweakRisk bitmask flags, before/after registry diff, search relevance ranking, custom profile API, recommendation engine |
| **2** | UI/UX | v6.18–v6.20 | 19 keyboard shortcuts with F1 cheatsheet, risk confirmation dialog (ConfirmApplyDialog), batch ETA with EMA, 11-item context menu, user JSON themes with FileSystemWatcher hot-reload |
| **3** | CLI & Integration | v6.16, v6.20 | `--json` global output flag, conditional apply flags (`--if-not-applied`, `--if-admin`, `--if-build`), interactive wizard, Ansible `win_regedit` YAML + DSC `.ps1` export |
| **4** | Test & Quality | v6.21 | 13 E2E + concurrent tests, GDI leak fixes (`using` on all Paint-path objects), ShellRunner kill-on-timeout, test hang elimination via DryRun + StubCorporate |
| **5** | Tweak Expansion | v6.22–v6.26 | +300 new tweaks: security WDAG/LSA/CredGuard, gaming DirectStorage/VRR/latency, accessibility magnifier/captions/eye-control/voice, energy battery/charging/standby/CPU/display, developer WSL/Git/containers, Office GP Word/Excel/Outlook/PowerPoint/Access |
| **6** | Services & Intelligence | v6.27–v6.28 | TweakHistory audit logging + ExportCsv, per-category HealthScoreService, ConflictDetector with severity classification, ScheduledTweakService, TweakMigrationService + SnapshotManager auto-migrate |
| **7** | Internationalisation & Ecosystem | v6.29–v6.30 | 10 locale stubs, 5 official `.rlpack.json` packs, 22 PowerShell cmdlets + 16 aliases, `pack-validation.yml` reusable CI workflow, `docs/PackAuthoring.md` |

**Post-Phase 7 (v6.31–v6.33)**: 15 new policy modules (+150 tweaks), `RegistrySession.Backup()` DryRun fix, `.runsettings` HangTimeout fix. Current totals: 7,718 tweaks, 158 categories, 195 module files, 3,296 tests.

---

## Strategic Assessment

> An honest, first-principles review of every major decision in the project.
> Each area is scored on a 1–5 scale (1 = needs rethinking, 5 = best-in-class).

### Decision Matrix

| Area | Current State | Score | Verdict |
|------|--------------|-------|---------|
| **Language (C# 13 / .NET 10)** | Modern, performant, Windows-native, excellent tooling | **5/5** | ✅ Keep — best choice for Windows registry work |
| **Frontend (WinForms)** | Legacy framework, 67+ dialogs, no data binding, poor DPI/a11y | **2/5** | 🔴 Rethink — migrate to WPF or WinUI 3 |
| **Backend architecture** | Monolithic `TweakEngine` God class, tight coupling, no DI | **2/5** | 🔴 Rethink — interface segregation + DI container |
| **Data persistence** | Scattered JSON files, no transactions, cross-assembly races | **1/5** | 🔴 Rethink — embedded database (SQLite/LiteDB) |
| **Tweak authoring** | 195 C# files, boilerplate-heavy, manual registration | **2/5** | 🟡 Enhance — data-driven YAML/JSON + auto-registration |
| **Testing (xUnit v2)** | 3,296 tests, held at v2, no UI automation, flaky perf tests | **3/5** | 🟡 Enhance — migrate to xUnit v3, proper integration testing |
| **CI/CD** | 14 workflows, manual 28-file version bump, over-engineered | **2/5** | 🔴 Rethink — consolidate to 3–4 workflows, automate bumps |
| **Documentation** | 12+ docs, 8 instruction files, manual SVG updates | **2/5** | 🔴 Rethink — auto-generate from code, consolidate |
| **Package registries** | npm/maven/gem stubs for a Windows-only tool | **1/5** | 🔴 Remove — keep only winget/scoop/chocolatey |
| **Feature scope** | 67+ dialogs (battery, port scanner, DNS, memory cleaner...) | **2/5** | 🔴 Rethink — refocus on core mission, extract utilities |
| **Security** | No code signing, no sandbox, admin elevation at startup | **2/5** | 🟡 Enhance — sign binaries, lazy elevation, pack sandboxing |
| **Localization** | Custom `Locale.T()`, 10 stubs, 2 actual translations | **2/5** | 🟡 Enhance — .resx standard pattern or accept English-only |
| **Build system** | OneDrive-specific workarounds baked into global config | **2/5** | 🟡 Enhance — isolate workarounds, clean global config |

### Top 5 Strategic Priorities

1. **Frontend modernisation** — WinForms → WPF/WinUI 3 (biggest user experience gain)
2. **Architecture cleanup** — DI, interface segregation, embedded database (biggest code quality gain)
3. **Scope discipline** — Extract 40+ utility dialogs, refocus on registry tweaks (biggest maintainability gain)
4. **Data-driven tweaks** — YAML/JSON definitions instead of 195 C# files (biggest authoring gain)
5. **CI/CD simplification** — 14 → 4 workflows, automated version bumps (biggest velocity gain)

---

## Phase 8 — Architecture Modernisation

> **Goal**: Transform the monolith into a clean, testable, extensible architecture.

### 8.1 Dependency Injection Container

**Priority**: P0 — Critical
**Effort**: Large

Replace manual wiring with `Microsoft.Extensions.DependencyInjection`. Every service
becomes injectable and mockable.

```csharp
// Before: tight coupling, God class
var engine = new TweakEngine();
engine.RegisterBuiltins();
var status = engine.StatusMap(parallel: true);

// After: DI-wired, interface-segregated
var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<ITweakRegistry, TweakRegistry>();
        services.AddSingleton<ITweakApplier, TweakApplier>();
        services.AddSingleton<ITweakDetector, TweakDetector>();
        services.AddSingleton<ITweakSearcher, TweakSearcher>();
        services.AddSingleton<IProfileService, ProfileService>();
        services.AddSingleton<ISnapshotService, SnapshotService>();
        services.AddSingleton<IRegistrySession, RegistrySession>();
        services.AddScoped<ITweakHistoryService, TweakHistoryService>();
    })
    .Build();
```

**Key interfaces to extract from `TweakEngine`**:

| Interface | Responsibility | Current Methods |
|-----------|---------------|-----------------|
| `ITweakRegistry` | Registration, lookup, categories | `Register`, `AllTweaks`, `GetTweak`, `Categories`, `TweaksByCategory` |
| `ITweakApplier` | Apply, remove, batch operations | `Apply`, `Remove`, `ApplyBatch`, `RemoveBatch` |
| `ITweakDetector` | Status detection | `DetectStatus`, `StatusMap` |
| `ITweakSearcher` | Search, filter | `Search`, `Filter` |
| `IProfileService` | Profile management | `Profiles`, `GetProfile`, `TweaksForProfile`, `ApplyProfile` |
| `ISnapshotService` | Snapshot save/load/restore | `SaveSnapshot`, `LoadSnapshot`, `RestoreSnapshot` |

**Backward compatibility**: `TweakEngine` becomes a facade that delegates to the
interfaces, preserving the existing API during migration.

### 8.2 Interface Segregation for RegistrySession

**Priority**: P0 — Critical

`RegistrySession` mixes read, write, check, backup, and execute concerns. Split into:

| Interface | Methods |
|-----------|---------|
| `IRegistryReader` | `ReadDword`, `ReadString`, `ReadValue`, `KeyExists`, `ValueExists` |
| `IRegistryWriter` | `SetDword`, `SetString`, `DeleteValue`, `DeleteTree` |
| `IRegistryExecutor` | `Execute`, `Evaluate`, `ExecuteWithDiff` |
| `IRegistryBackup` | `Backup`, `Restore` |

**Benefit**: Tests mock only the interface they need. GUI can inject a read-only session
for status display. CLI can inject a dry-run writer for preview mode.

### 8.3 Event-Driven Architecture (Mediator Pattern)

**Priority**: P1 — High

Replace direct method calls between services with an event bus. When a tweak is applied,
multiple services react independently:

```csharp
// Event
public sealed record TweakAppliedEvent(string TweakId, TweakResult Result, IReadOnlyList<RegDiff> Diffs);

// Handlers (registered via DI)
public sealed class HistoryHandler : IEventHandler<TweakAppliedEvent> { ... }
public sealed class AnalyticsHandler : IEventHandler<TweakAppliedEvent> { ... }
public sealed class HealthScoreHandler : IEventHandler<TweakAppliedEvent> { ... }
public sealed class ConflictHandler : IEventHandler<TweakAppliedEvent> { ... }
```

**Benefits**: Services are decoupled. Adding a new handler requires zero changes to
existing code. Testing a handler means firing one event.

**Implementation**: Use `MediatR` (OSS, MIT) or a minimal in-process event dispatcher.

### 8.4 Lazy Module Loading

**Priority**: P2 — Medium

Currently `RegisterBuiltins()` loads all 195 modules (~7,718 tweaks) into memory at
startup. With DI, modules can be loaded on-demand per category:

```csharp
public interface ITweakModule
{
    string Category { get; }
    IReadOnlyList<TweakDef> Tweaks { get; }  // materialized on first access
}

// Registration via assembly scanning
services.Scan(scan => scan
    .FromAssemblyOf<TweakEngine>()
    .AddClasses(c => c.AssignableTo<ITweakModule>())
    .AsImplementedInterfaces()
    .WithSingletonLifetime());
```

**Benefit**: Startup time drops from ~200ms to ~50ms. Memory usage: only loaded
categories occupy RAM.

---

## Phase 9 — Data Layer & Persistence

> **Goal**: Replace scattered JSON files with a proper embedded database.

### 9.1 SQLite Embedded Database

**Priority**: P0 — Critical
**Effort**: Large

Replace 8+ individual JSON files with a single SQLite database. Use
`Microsoft.Data.Sqlite` (OSS, MIT, first-party).

**Current file-based state** (each with its own serialization/IO logic):

| File | Service | Records |
|------|---------|---------|
| `config.json` | AppConfig | 1 (settings) |
| `favorites.json` | Favorites | ~50 IDs |
| `ratings.json` | Ratings | ~100 entries |
| `history.json` | TweakHistory | ~1000 events |
| `analytics.json` | Analytics | ~500 entries |
| `compliance-history.json` | ComplianceHistory | ~200 entries |
| `smartscan-feedback.json` | SmartScanService | ~100 entries |
| `profiles/*.json` | UserProfileService | ~5 files |

**Target schema** (single `regilattice.db` file in `%LOCALAPPDATA%\RegiLattice\`):

```sql
CREATE TABLE config (key TEXT PRIMARY KEY, value TEXT);
CREATE TABLE favorites (tweak_id TEXT PRIMARY KEY, added_at TEXT);
CREATE TABLE ratings (tweak_id TEXT PRIMARY KEY, score INTEGER, rated_at TEXT);
CREATE TABLE history (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tweak_id TEXT NOT NULL,
    action TEXT NOT NULL,          -- Apply, Remove, Update
    result TEXT NOT NULL,          -- Applied, Error, etc.
    timestamp TEXT NOT NULL,
    username TEXT,
    machine_name TEXT,
    session_id TEXT,
    diffs_json TEXT                -- JSON blob for registry diffs
);
CREATE TABLE analytics (tweak_id TEXT, event TEXT, timestamp TEXT);
CREATE TABLE profiles (name TEXT PRIMARY KEY, definition_json TEXT);
CREATE TABLE compliance (id INTEGER PRIMARY KEY AUTOINCREMENT, snapshot_json TEXT, timestamp TEXT);

CREATE INDEX idx_history_tweak ON history(tweak_id);
CREATE INDEX idx_history_timestamp ON history(timestamp);
CREATE INDEX idx_analytics_tweak ON analytics(tweak_id);
```

**Benefits**:
- **ACID transactions**: No more data corruption on power failure
- **Concurrent access safety**: Eliminates cross-assembly test race conditions
- **Structured queries**: `SELECT * FROM history WHERE tweak_id = ? ORDER BY timestamp DESC LIMIT 10`
- **Single file backup**: One file to backup/restore instead of 8+
- **Full-text search**: SQLite FTS5 extension for tweak searching
- **Migration support**: Schema versioning with `PRAGMA user_version`

**Migration path**: `DatabaseMigrator` reads existing JSON files on first launch,
imports into SQLite, renames old files to `.json.migrated`.

### 9.2 Repository Pattern

**Priority**: P1 — High

Abstract database access behind repository interfaces:

```csharp
public interface ITweakHistoryRepository
{
    void RecordApply(string tweakId, TweakResult result, IReadOnlyList<RegDiff>? diffs = null);
    void RecordRemove(string tweakId, TweakResult result);
    IReadOnlyList<HistoryEntry> Recent(int count = 50);
    IReadOnlyList<HistoryEntry> ForTweak(string tweakId);
    void ExportCsv(string path);
}
```

**Benefit**: Swappable between SQLite (production), in-memory (tests), and mock
(unit tests). Tests no longer need file cleanup or `IDisposable` temp-file patterns.

### 9.3 Caching Layer with Change Notifications

**Priority**: P2 — Medium

Hot data (favorites, status map, config) cached in-memory with cache invalidation when
the database changes:

```csharp
public interface ICacheService
{
    T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? ttl = null);
    void Invalidate(string key);
    void InvalidateAll();
}
```

**Status map caching**: After `StatusMap()` runs (~300ms for 7K tweaks), results are
cached. Cache is invalidated only when `Apply`/`Remove` changes state.

---

## Phase 10 — Frontend Revolution

> **Goal**: Modern, accessible, high-DPI, data-bound UI.

### 10.1 WPF Migration (Recommended Path)

**Priority**: P0 — Critical
**Effort**: Very Large (multi-sprint)

**Why WPF over WinUI 3**:
- Mature ecosystem, extensive community support
- `CommunityToolkit.Mvvm` provides source-generated MVVM
- `MaterialDesignInXamlToolkit` or `HandyControl` for modern visuals
- Full accessibility support (UI Automation patterns)
- HiDPI is native (resolution-independent vector rendering)
- WinForms interop period possible via `WindowsFormsHost`

**Why not WinUI 3**:
- Packaging requirements (MSIX or unpackaged workarounds)
- Smaller community, fewer third-party controls
- Still maturing (missing some WPF features like multi-window)

**Migration strategy** (incremental, not big-bang):

| Sprint | Scope | Approach |
|--------|-------|----------|
| 10.1a | Shell + navigation | New WPF `MainWindow` with sidebar nav, embed WinForms panels via `WindowsFormsHost` |
| 10.1b | Tweak browser | Native WPF `ListView` with virtualization, replace `TweakBrowserPanel` |
| 10.1c | Settings & dialogs | Migrate dialog-by-dialog (focused core dialogs first) |
| 10.1d | Remove WinForms dependency | All controls are native WPF, remove `WindowsFormsHost` |

**Architecture** (MVVM):

```
View (XAML)          ← data binding →    ViewModel (C#)    ← DI →    Service (Core)
MainWindow.xaml      ←→  MainViewModel         ← ITweakRegistry
TweakBrowser.xaml    ←→  TweakBrowserViewModel  ← ITweakSearcher, ITweakDetector
ProfilePanel.xaml    ←→  ProfileViewModel       ← IProfileService
SettingsPage.xaml    ←→  SettingsViewModel       ← IAppConfig
```

### 10.2 Alternative: Terminal UI for Power Users (Spectre.Console)

**Priority**: P2 — Medium

For users who prefer the terminal, build a rich TUI using `Spectre.Console` (OSS, MIT):

```
┌─ RegiLattice ────────────────────────────────────────────┐
│ Categories (158)        │ Privacy (31 tweaks)             │
│ ▸ Privacy          [31] │ ☑ Disable Telemetry    Applied │
│ ▸ Performance      [28] │ ☑ Disable Activity     Applied │
│ ▸ Security         [45] │ ☐ Disable Location   NotApplied│
│ ▸ Gaming           [22] │ ☐ Disable Cortana    NotApplied│
│   ...                   │   ...                          │
├─────────────────────────┴────────────────────────────────┤
│ [A]pply  [R]emove  [S]earch  [P]rofile  [Q]uit          │
└──────────────────────────────────────────────────────────┘
```

This replaces the current `--menu` interactive mode with a full TUI.

### 10.3 WCAG 2.1 AA Accessibility (Carried Forward)

**Priority**: P1 — High

Full accessibility audit and remediation. Applies to whichever UI framework is active:

1. **Tab order** on every interactive control
2. **Screen reader labels** (`AccessibleName` / `AutomationProperties.Name`)
3. **Colour contrast** ≥ 4.5:1 (audit all 11 themes)
4. **Focus indicators** visible in all themes
5. **High contrast mode** detection and compatible palette
6. **Keyboard-only navigation** for every feature

### 10.4 High-DPI Vector Icon Rendering (Carried Forward)

**Priority**: P2 — Medium

Generate icons at DPI-aware sizes. WPF migration makes this nearly free (vector
rendering is native). For WinForms interim: 2× and 3× bitmap variants with
`DeviceDpi`-based selection.

---

## Phase 11 — Scope Discipline & Feature Focus

> **Goal**: Refocus on the core mission. Extract utilities into a separate project.

### 11.1 Feature Audit & Classification

**Priority**: P0 — Critical

The 67+ dialog classes need honest classification:

**Core (keep in main app)** — directly support tweak management:

| Dialog | Reason |
|--------|--------|
| `MainForm` | Primary interface |
| `AboutDialog` | Standard |
| `PreferencesDialog` | App configuration |
| `ConfirmApplyDialog` | Safety gate |
| `KeyboardShortcutsDialog` | Usability |
| `ProfileWizardDialog` | Core workflow |
| `SmartScanDialog` | Core workflow |
| `MarketplaceDialog` | Pack management |
| `DependencyGraphDialog` | Tweak relationships |
| `WhatsNewDialog` | Onboarding |
| `FirstRunWizardDialog` | Onboarding |
| `ComplianceTrendDialog` | Enterprise |
| `ScheduledTweakDialog` | Automation |
| `ProfileCompareDialog` | Core workflow |
| `ProfileSchedulerDialog` | Automation |
| 5× Package manager dialogs | Supported feature |

**Extract to "RegiLattice Tools" plugin** — standalone utilities unrelated to tweaks:

| Dialog | Current Purpose | Better Home |
|--------|----------------|-------------|
| `BatteryHealthDialog` | Battery diagnostics | Separate tool |
| `BatterySaverDialog` | Battery settings | Separate tool |
| `BootTimeAnalyzerDialog` | Boot time analysis | Separate tool |
| `BrightnessSchedulerDialog` | Display scheduling | Separate tool |
| `BrowserCacheCleanerDialog` | Cache cleanup | Separate tool |
| `ContextMenuManagerDialog` | Shell extension mgmt | Separate tool |
| `DiskSpaceDialog` | Disk analysis | Separate tool |
| `DnsOverHttpsDialog` | DNS config | Separate tool |
| `DnsSwitcherDialog` | DNS config | Separate tool |
| `DriverUpdateCheckerDialog` | Driver updates | Separate tool |
| `FirewallRulesDialog` | Firewall management | Separate tool |
| `HardwareTemperatureDialog` | Hardware monitoring | Separate tool |
| `HostsFileManagerDialog` | Hosts file editor | Separate tool |
| `InstalledAppsDialog` | App management | Separate tool |
| `MacAddressDialog` | Network info | Separate tool |
| `MemoryCleanerDialog` | Memory optimization | Separate tool |
| `NetworkAdapterDialog` | NIC management | Separate tool |
| `NetworkBandwidthDialog` | Bandwidth monitoring | Separate tool |
| `NetworkRepairDialog` | Network fix | Separate tool |
| `NetworkToolsDialog` | Network diagnostics | Separate tool |
| `NotificationManagerDialog` | Notification settings | Separate tool |
| `PortScannerDialog` | Port scanning | Separate tool |
| `PowerPlanDialog` | Power plan config | Separate tool |
| `PowerSchedulerDialog` | Power scheduling | Separate tool |
| `PrivacyDashboardDialog` | Privacy overview | Could stay |
| `ProxyConfigDialog` | Proxy settings | Separate tool |
| `ScheduledTaskManagerDialog` | Task scheduler | Separate tool |
| `ServiceManagerDialog` | Service management | Separate tool |
| `ShellExtensionDialog` | Shell extensions | Separate tool |
| `SleepTimerDialog` | Sleep timer | Separate tool |
| `StartupManagerDialog` | Startup programs | Separate tool |
| `TelemetryDashboardDialog` | Telemetry overview | Could stay |
| `TempFileCleanerDialog` | Temp cleanup | Separate tool |
| `UpdateCheckerDialog` | Update checking | Could stay |
| `UsbPowerDialog` | USB power config | Separate tool |
| `WakeOnLanDialog` | WOL utility | Separate tool |
| `WiFiProfileDialog` | WiFi management | Separate tool |
| `WindowsUpdateControlDialog` | WU control | Could stay |

**Impact**: ~35 dialogs extracted. Main app drops from 67 to ~30 dialogs. Dramatically
simpler to maintain, test, and evolve.

### 11.2 Create `RegiLattice.Tools` Project

**Priority**: P1 — High

New class library project for extracted utilities:

```
src/
├── RegiLattice.Core/       # Engine, tweaks, registry (unchanged)
├── RegiLattice.GUI/        # Main app (30 focused dialogs)
├── RegiLattice.CLI/        # CLI (unchanged)
└── RegiLattice.Tools/      # NEW: extracted utility dialogs
    ├── Network/             # DNS, proxy, firewall, port scanner, bandwidth
    ├── System/              # Services, startup, disk, memory, temp cleaner
    ├── Hardware/            # Battery, temperature, USB power
    └── Scheduling/          # Power scheduler, sleep timer, brightness
```

**Loading**: GUI loads `RegiLattice.Tools` as an optional plugin. If the DLL is present,
"Tools" menu appears. If absent, the app runs fine without it.

### 11.3 Core Services Audit

**Priority**: P1 — High

Apply the same discipline to Core services. Some belong in a "Tools" layer:

| Service | Keep in Core? | Reason |
|---------|--------------|--------|
| `TweakEngine` (split) | ✅ | Core mission |
| `RegistrySession` | ✅ | Core mission |
| `AppConfig` | ✅ | Configuration |
| `TweakHistory` | ✅ | Audit trail |
| `Favorites` | ✅ | User preference |
| `SnapshotManager` | ✅ | State management |
| `ProfileService` | ✅ | Core feature |
| `CorporateGuard` | ✅ | Safety gate |
| `ConflictDetector` | ✅ | Validation |
| `TweakValidator` | ✅ | Validation |
| `DependencyResolver` | ✅ | Core feature |
| `NetworkManager` | ❌ Move to Tools | Utility |
| `PowerPlanManager` | ❌ Move to Tools | Utility |
| `ServiceManager` | ❌ Move to Tools | Utility |
| `ScheduledTaskManager` | ❌ Move to Tools | Utility |
| `StartupManager` | ❌ Move to Tools | Utility |
| `SystemMonitor` | 🟡 Keep (used by HardwareInfo) | Supporting |

---

## Phase 12 — Build, CI/CD & Distribution

> **Goal**: Simplify the build pipeline, automate version bumps, remove dead weight.

### 12.1 Consolidate Workflows: 14 → 4

**Priority**: P0 — Critical

| Keep | Purpose | Replaces |
|------|---------|----------|
| `ci.yml` | Build + test on push/PR | ci.yml (keep), smoke.yml (merge) |
| `release.yml` | Tag-triggered builds + GitHub Release | release.yml (keep), release-prep.yml (automate) |
| `weekly.yml` | Stale issues, dependency review, mutations, CodeQL | stale.yml, dependency-review.yml, codeql.yml, packages.yml |
| `pages.yml` | GitHub Pages deployment | pages.yml (keep) |

**Remove**: `debug.yml` (ad-hoc, use `workflow_dispatch` on ci.yml), `label.yml`
(replace with branch-name labelling in ci.yml), `notify-failure.yml` (use GitHub's
native notification), `powershell.yml` (merge into ci.yml), `pack-validation.yml`
(merge into ci.yml as a job).

### 12.2 Automated Version Bump Script

**Priority**: P0 — Critical

The current 28+ file manual update process is unsustainable. Create a single PowerShell
script:

```powershell
# Usage: .\scripts\Bump-Version.ps1 -Version "6.34.0" -TweakCount 7768 -CategoryCount 163 -TestCount 3320
param(
    [Parameter(Mandatory)] [string] $Version,
    [Parameter(Mandatory)] [int] $TweakCount,
    [Parameter(Mandatory)] [int] $CategoryCount,
    [Parameter(Mandatory)] [int] $TestCount
)
```

The script updates ALL 28 files from a single invocation. Includes a `--dry-run` mode
that shows what would change without modifying files.

**Ground-truth extraction**: The script can also auto-detect counts from the compiled
assembly (eliminating the need to pass them manually):

```powershell
.\scripts\Bump-Version.ps1 -Version "6.34.0" -AutoDetectCounts
```

### 12.3 Remove Dead Package Registries

**Priority**: P1 — High

**Remove entirely** (Windows-only tool has zero npm/maven/gem users):

| Directory | Why remove |
|-----------|-----------|
| `npm/` | No one installs a Windows registry tool via npm |
| `maven/` | No JVM users for a Windows .NET tool |
| `gem/` | No Ruby users for a Windows .NET tool |

**Keep and maintain**:

| Registry | Why keep |
|----------|---------|
| `winget/` | Native Windows package manager |
| `scoop/` | Power-user Windows package manager |
| `chocolatey/` | Enterprise Windows deployment |
| `powershell/` | Administrative automation |

**Impact**: 28-file version bump checklist drops to ~20 files. 3 fewer directories to
maintain. Simpler mental model.

### 12.4 Code Signing

**Priority**: P1 — High

Windows SmartScreen blocks unsigned executables. Users see "Windows protected your PC"
on every download.

**Options** (prefer OSS/free):
- **SignPath.io**: Free for OSS projects, integrates with GitHub Actions
- **Azure Trusted Signing**: Microsoft's cloud signing service (~$10/month)
- **.pfx certificate**: Self-managed, stored in GitHub Secrets

**Implementation**: Add a signing step to `release.yml` after build, before upload.

### 12.5 Clean Build System Configuration

**Priority**: P2 — Medium

Isolate OneDrive-specific workarounds from global build configuration:

```xml
<!-- Directory.Build.props — conditional on OneDrive detection -->
<PropertyGroup Condition="$(MSBuildProjectDirectory.Contains('OneDrive'))">
    <RegiLatticeLocalBuildRoot>$(TEMP)\RegiLattice-build\$(MSBuildProjectName)</RegiLatticeLocalBuildRoot>
    <BaseIntermediateOutputPath>$(RegiLatticeLocalBuildRoot)\obj\</BaseIntermediateOutputPath>
    <BaseOutputPath>$(RegiLatticeLocalBuildRoot)\bin\</BaseOutputPath>
</PropertyGroup>
```

Contributors cloning to normal paths get standard MSBuild behaviour without the temp
redirect. OneDrive users still get the workaround automatically.

---

## Phase 13 — Quality, Testing & Observability

> **Goal**: Modern test infrastructure, real coverage, and production observability.

### 13.1 xUnit v3 Migration

**Priority**: P1 — High

xUnit v2 is end-of-life. v3 brings:
- Better parallel execution
- Improved assertions
- Source-generated test discovery (faster)
- `IAsyncLifetime` improvements

**Migration checklist**:

| Package | v2 | v3 |
|---------|----|----|
| `xunit` | 2.9.3 | 3.x |
| `xunit.runner.visualstudio` | 2.8.2 | 3.x |
| `FsCheck.Xunit` | 2.16.6 | 3.x |
| `Microsoft.NET.Test.Sdk` | 17.14.1 | 18.x |

**Breaking changes to handle**: New test class model, changed attribute APIs for
`FsCheck`, new test-host protocol for Test SDK 18.x.

### 13.2 Integration Testing with Real Registry (Sandboxed)

**Priority**: P1 — High

Current tests use `DryRun = true` exclusively. No test verifies actual registry
read/write behavior.

**Solution**: Dedicated integration test project that:
- Creates a temporary `HKCU\Software\RegiLattice-Test-{guid}` key
- Runs real Apply → Detect → Remove cycles
- Cleans up the key in `IAsyncLifetime.DisposeAsync()`
- Runs only when `REGILATTICE_INTEGRATION_TESTS=1` env var is set

### 13.3 Structured Logging with Serilog

**Priority**: P2 — Medium

Replace `Console.WriteLine` and custom log panels with structured logging:

```csharp
services.AddSerilog(config => config
    .WriteTo.File("logs/regilattice-.log", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .Enrich.FromLogContext());
```

**Benefits**: Log levels, structured properties, log correlation with session IDs,
export to any sink (file, EventLog, SIEM). Replaces the ad-hoc `RegistrySession.Log`
and `TweakHistory` logging.

### 13.4 OpenTelemetry Metrics (Opt-In)

**Priority**: P3 — Nice to Have

For enterprise deployments, expose metrics via OpenTelemetry:

```csharp
// Counters
meter.CreateCounter<long>("regilattice.tweaks.applied");
meter.CreateCounter<long>("regilattice.tweaks.removed");
meter.CreateHistogram<double>("regilattice.statusmap.duration_ms");
```

Users opt-in via config. Metrics exported to Prometheus, Azure Monitor, or any OTLP
endpoint. Zero overhead when disabled.

### 13.5 Performance Benchmarks in CI (Carried Forward)

**Priority**: P2 — Medium

`RegiLattice.Benchmarks` already exists. Add:
- Monthly CI job that runs benchmarks
- Results stored as JSON artifacts
- Comparison against previous run, flag regressions > 50%
- Dashboard in GitHub Pages

---

## Phase 14 — Documentation & Developer Experience

> **Goal**: Documentation that maintains itself. Less to write, less to update.

### 14.1 Auto-Generated API Documentation

**Priority**: P1 — High

Replace manually maintained `docs/Api.md` with auto-generated docs:

**Option A** — DocFX (OSS, Microsoft):
```yaml
# docfx.json
{
  "metadata": [{ "src": [{ "src": "src", "files": ["**/*.csproj"] }] }],
  "build": { "dest": "_site" }
}
```

**Option B** — xmldoc2md (lightweight, OSS):
```powershell
dotnet tool install -g xmldoc2md
xmldoc2md src/RegiLattice.Core/bin/Release/net10.0-windows/RegiLattice.Core.dll docs/api/
```

### 14.2 Consolidate Instruction Files

**Priority**: P1 — High

8 instruction files with heavy overlap:

| Current | Merge Into |
|---------|-----------|
| `copilot-instructions.md` (500+ lines) | **Keep**, but trim to essentials |
| `workspace.instructions.md` | Merge into `copilot-instructions.md` |
| `csharp.instructions.md` | Keep (scoped to `*.cs`) |
| `testing.instructions.md` | Keep (scoped to `tests/**`) |
| `git-workflow.instructions.md` | Keep, but trim 50% |
| `lessons-learned.instructions.md` | Convert to tests/analyzers where possible |
| `cicd.instructions.md` | Merge into `git-workflow.instructions.md` |
| `no-duplication.instructions.md` | Merge into `csharp.instructions.md` |

**Result**: 8 files → 4 files. Reduce total instruction content by ~40%.

**Lessons-learned conversion**: Many entries in lessons-learned are rules that should be
**enforced by analyzers or tests**, not documented:

| Lesson | Better Enforcement |
|--------|--------------------|
| "No `#pragma warning disable`" | Roslyn analyzer rule |
| "All classes must be `sealed`" | Custom Roslyn analyzer |
| "All tweak IDs must be unique" | Already tested (keep test, remove doc) |
| "DryRun mode in tests" | Test base class enforces this |
| "No `--no-build` for GUI.Tests" | CI workflow already handles this |

### 14.3 Template SVGs with CI Substitution

**Priority**: P2 — Medium

Replace hardcoded count SVGs with templates:

```xml
<!-- docs/assets/stats.svg.template -->
<text>{{TWEAK_COUNT}}</text>
<text>{{CATEGORY_COUNT}}</text>
<text>{{TEST_COUNT}}</text>
```

CI or the `Bump-Version.ps1` script substitutes values and generates final SVGs.
No more manual SVG editing on every version bump.

### 14.4 Single-Source README

**Priority**: P2 — Medium

`README.md` duplicates content from `Architecture.md`, `Development.md`, and
`CLI-Reference.md`. Keep README focused:

1. One-paragraph description
2. Installation (3 methods)
3. Screenshot / demo GIF
4. Quick start (5 commands)
5. Link to full docs

Move detailed content to `docs/` and link from README. Current README is ~300 lines;
target: ~100 lines.

---

## Phase 15 — Data-Driven Tweaks

> **Goal**: Tweak definitions as data (YAML/JSON), not C# code.

### 15.1 YAML Tweak Definitions

**Priority**: P1 — High
**Effort**: Very Large

The biggest architectural shift. Replace 195 C# tweak module files with YAML data files:

```yaml
# tweaks/privacy/disable-telemetry.yaml
id: priv-disable-telemetry
label: Disable Telemetry
category: Privacy
description: Disables Windows diagnostic and usage data collection.
tags: [telemetry, privacy, data-collection]
needsAdmin: true
corpSafe: true
impactScore: 5
safetyRating: 5
impactNote: Prevents Windows from sending diagnostic data to Microsoft.
apply:
  - setDword: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: AllowTelemetry, value: 0 }
remove:
  - deleteValue: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: AllowTelemetry }
detect:
  - checkDword: { path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", name: AllowTelemetry, expected: 0 }
```

**Benefits**:
- **Non-developers can author tweaks** — just edit a YAML file
- **JSON Schema validation** — catches errors before compile
- **Tooling**: YAML linters, diff-friendly, IDE autocompletion
- **Smaller codebase**: 195 C# files (~50K LOC) → 195 YAML files (~15K lines)
- **Hot-reload**: Load new tweaks without recompiling
- **Pack convergence**: `.rlpack.json` and built-in tweaks use the same format

**Implementation**:

```csharp
public sealed class YamlTweakLoader : ITweakModule
{
    public IReadOnlyList<TweakDef> Load(string yamlPath)
    {
        var yaml = new YamlDotNet.Serialization.Deserializer();
        var raw = yaml.Deserialize<TweakYamlModel>(File.ReadAllText(yamlPath));
        return raw.ToTweakDef();  // validated, immutable
    }
}
```

**NuGet**: `YamlDotNet` (OSS, MIT, 300M+ downloads).

**Migration**: Script converts existing C# `TweakDef` initializers to YAML. Run once,
delete the 195 `.cs` files.

### 15.2 JSON Schema for Tweak Validation

**Priority**: P1 — High

Publish a JSON Schema (works for YAML too) that IDEs use for autocompletion and
validation:

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "type": "object",
  "required": ["id", "label", "category"],
  "properties": {
    "id": { "type": "string", "pattern": "^[a-z0-9]+-[a-z0-9-]+$" },
    "label": { "type": "string", "minLength": 3 },
    "category": { "type": "string" },
    "impactScore": { "type": "integer", "minimum": 1, "maximum": 5 },
    "safetyRating": { "type": "integer", "minimum": 1, "maximum": 5 },
    "apply": { "type": "array", "items": { "$ref": "#/$defs/RegOp" } }
  }
}
```

### 15.3 Auto-Registration via Assembly Scanning

**Priority**: P2 — Medium

If YAML is too large a shift, at minimum replace the manual `RegisterBuiltins()` method
(which must list every module by hand) with assembly scanning:

```csharp
// Before: manual registration of 195 modules
engine.Register(Privacy.Tweaks);
engine.Register(Performance.Tweaks);
// ... 193 more lines ...

// After: auto-discover all ITweakModule implementations
var modules = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && typeof(ITweakModule).IsAssignableFrom(t))
    .Select(Activator.CreateInstance)
    .Cast<ITweakModule>();
foreach (var m in modules) engine.Register(m.Tweaks);
```

**Or**: Source generators at compile time (zero runtime reflection cost).

---

## Phase 16 — Security & Trust

> **Goal**: Enterprise-grade security posture.

### 16.1 Lazy Admin Elevation

**Priority**: P1 — High

Currently `Elevation.RequestElevation()` re-launches the entire app as admin at startup.
Many tweaks only touch `HKCU` (user scope) and don't need admin.

**Better**: Elevate only when the user attempts to apply/remove an `HKLM` tweak:

```csharp
public async Task ApplyAsync(string tweakId)
{
    var tweak = registry.GetTweak(tweakId);
    if (tweak.NeedsAdmin && !Elevation.IsAdmin())
    {
        // Launch a helper process with admin rights for just this operation
        await ElevatedHelper.RunAsync("apply", tweakId);
        return;
    }
    applier.Apply(tweak);
}
```

**Benefits**: App starts faster, user sees fewer UAC prompts, principle of least
privilege.

### 16.2 Pack Sandboxing

**Priority**: P2 — Medium

Packs currently run in the same AppDomain with full access. A malicious pack could
execute arbitrary code via `ApplyAction` delegates.

**Mitigation**: Packs are **data-only** (YAML/JSON tweak definitions). No executable
code allowed. `PackLoader` rejects any pack that defines `ApplyAction`, `RemoveAction`,
or `DetectAction` delegates.

### 16.3 SBOM Generation

**Priority**: P2 — Medium

Generate Software Bill of Materials on every release:

```yaml
# In release.yml
- name: Generate SBOM
  run: dotnet CycloneDX RegiLattice.sln -o sbom.json -j
```

Published alongside binaries. Required for SOC 2 and many enterprise procurement
processes.

### 16.4 Reproducible Builds

**Priority**: P3 — Nice to Have

`<Deterministic>true</Deterministic>` is already set. Add:
- `<EmbedUntrackedSources>true</EmbedUntrackedSources>`
- Source Link for GitHub
- Publish `.snupkg` symbol packages

Any user can rebuild from source and get bit-identical output.

---

## Phase 17 — Ecosystem & Community

> **Goal**: Make it easy for others to contribute and extend.

### 17.1 Contribution Experience

**Priority**: P1 — High

- **`CONTRIBUTING.md`**: Streamlined guide (currently exists but needs updating)
- **Good First Issues**: Label and curate 10+ starter issues
- **Developer setup**: One-command `.\scripts\Setup-Dev.ps1` that installs SDK, restores, builds, tests
- **PR template**: Checklist with quality gates

### 17.2 Plugin SDK NuGet Package

**Priority**: P2 — Medium

Publish `RegiLattice.SDK` NuGet package containing:
- `TweakDef`, `RegOp`, `TweakKind`, `TweakResult` models
- `ITweakModule` interface
- JSON Schema for pack files
- Validation helpers

Third-party developers reference the SDK to build packs with compile-time validation.

### 17.3 Community Translation Platform

**Priority**: P3 — Nice to Have

Replace manual `Locale.T()` string maintenance with Crowdin or Weblate:
- Translators work in a web UI
- Pull requests auto-generated when translations are complete
- Coverage dashboard per locale
- OR: Accept English-only and remove localization overhead

### 17.4 Watch Mode for Tweak Drift (Carried Forward)

**Priority**: P2 — Medium

`--watch` command monitors applied tweaks for external reversion:

```powershell
RegiLatticeCLI.exe --watch --interval 300 --auto-fix
# → [22:20:30] ⚠ DRIFT: priv-disable-telemetry reverted (AllowTelemetry: 0 → 1)
# → [22:20:31] ✅ Auto-fixed: priv-disable-telemetry re-applied
```

**Use case**: IT scheduled task enforcing compliance continuously.

### 17.5 Batch Script Executor (Carried Forward)

**Priority**: P2 — Medium

`--batch-file <path>` reads a YAML recipe for multi-step deployment:

```yaml
name: "Privacy Hardening"
rollbackOnFailure: true
steps:
  - apply: ["priv-disable-telemetry", "priv-disable-activity-history"]
  - apply-profile: "privacy"
  - verify:
      tweaks: ["priv-disable-telemetry"]
      expected: "Applied"
```

---

## Success Metrics

| Metric | Current (v6.33) | Phase 10 Target | Phase 15 Target |
|--------|----------------|-----------------|-----------------|
| Tweaks | 7,718 | 7,718 | 8,000+ |
| GUI framework | WinForms | WPF shell + WinForms interop | Full WPF |
| Backend architecture | Monolithic | DI + interfaces | Full CQRS-lite |
| Data persistence | 8 JSON files | SQLite | SQLite + cache |
| Tweak format | 195 C# files | 195 C# files | YAML data files |
| Tests | 3,296 (xUnit v2) | 3,500+ (xUnit v3) | 4,000+ |
| CI workflows | 14 | 4 | 4 |
| Version bump files | 28 manual | 1 script | 1 script |
| Package registries | 7 (npm/maven/gem/etc) | 4 (winget/scoop/choco/PS) | 4 |
| Dialogs in main app | 67 | ~30 (rest extracted) | ~25 |
| Code signing | None | SignPath.io | SignPath.io |
| Locales | 2 real + 8 stubs | 2 (focused) | Crowdin (community) |
| DPI support | Bitmap scaling | WPF vector rendering | Native |
| a11y compliance | None | WCAG 2.1 AA partial | WCAG 2.1 AA full |
| Startup time | ~200ms | ~100ms (lazy load) | ~50ms |
| Build output size | ~40MB self-contained | ~35MB (scope reduction) | ~30MB (trimming-safe) |

---

## Risk Register

| ID | Risk | Impact | Probability | Mitigation |
|----|------|--------|------------|------------|
| R1 | WPF migration takes longer than expected | Months of dual-framework maintenance | High | Incremental migration via `WindowsFormsHost`; WinForms remains functional |
| R2 | SQLite migration corrupts existing user data | Users lose favorites, history, config | Medium | JSON backup before migration; rollback path; extensive migration tests |
| R3 | YAML tweak format has edge cases C# handled implicitly | Broken tweaks after migration | Medium | Comprehensive round-trip test: C# → YAML → load → compare |
| R4 | xUnit v3 migration breaks 3,296 tests | CI blocked for days | Medium | Migrate in a branch; fix one test project at a time |
| R5 | Removing npm/maven/gem breaks unknown downstream users | Broken installs (unlikely — no evidence of usage) | Low | Announce deprecation in CHANGELOG one version before removal |
| R6 | Scope reduction (dialog extraction) upsets existing users | Feature regression perception | Medium | Extract to a separate downloadable plugin, not deleted |
| R7 | DI container adds startup overhead | Slower cold start | Low | Benchmark before/after; DI registration is ~10ms typically |
| R8 | Code signing certificate expires or is compromised | Broken release pipeline, trust loss | Low | Auto-renew via SignPath; revocation procedure documented |

---

## Migration Sequence

> Recommended execution order. Each phase can be a MINOR version bump.

```
Phase 11 (Scope Discipline)           ─── v7.0.0 (MAJOR — breaking: dialogs extracted)
  ↓
Phase 12.1–12.3 (CI + Registry Cleanup) ── v7.1.0
  ↓
Phase 8.1–8.2 (DI + Interfaces)       ─── v7.2.0
  ↓
Phase 9.1–9.2 (SQLite + Repository)    ─── v7.3.0
  ↓
Phase 14 (Docs Consolidation)          ─── v7.4.0
  ↓
Phase 13.1 (xUnit v3)                 ─── v7.5.0
  ↓
Phase 10.1a–b (WPF Shell + Browser)    ─── v8.0.0 (MAJOR — new UI framework)
  ↓
Phase 15 (YAML Tweaks)                ─── v9.0.0 (MAJOR — new tweak format)
  ↓
Phase 16 (Security + Signing)          ─── v9.1.0
  ↓
Phase 17 (Ecosystem)                   ─── v9.2.0+
```

**Key principle**: Each phase is independently valuable. No phase depends on all prior
phases being complete. Start with the highest-ROI items (scope discipline, CI cleanup,
DI) before the larger migrations (WPF, YAML).

---

## Appendix — Completed Phase Details (v6.0–v6.33)

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

</details>
