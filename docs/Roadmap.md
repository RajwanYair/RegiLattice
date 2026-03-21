# RegiLattice — Roadmap

> Living document — updated after every sprint.
> Last updated: 2026-07-22 · v4.1.0 · 3 719 tweaks · 99 categories · 1 538 tests

---

## Current State (as of v4.1.0)

| Metric | Value |
|--------|-------|
| Language | C# 13 / .NET 10.0-windows (x64) |
| Tweaks | 3 719 verified across 99 categories |
| Tests | 1 538 (1 121 Core + 175 CLI + 242 GUI), all passing |
| GUI | WinForms with 11 themes, system theme auto-detection, tray icon, progress bar, live CPU/RAM status bar, 59 dialog forms |
| Profiles | 5 (business, gaming, privacy, minimal, server) |
| Services | 27 Core services (AutoUpdater, HealthScoreService, ConflictDetector, ComplianceService, ScheduledTweak, UpdateCheck, GroupPolicyExporter, etc.) |
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
- 5 application profiles, Corporate Guard, Plugin marketplace concept

### v2.0.0 — C# Migration

Full C# rewrite: Core library, 68 tweak modules, WinForms GUI (4 themes),
CLI (25+ commands), 203 xUnit tests, all services ported.

### v3.0.0 — Quality Audit

Removed 468 non-functional stubs, added TweakKind + CategoryIcons, TweakEngine
no-op guard, 0 errors/0 warnings Release build.

### Sprints 1 – 47b (v3.1.0 → v3.7.0)

| Sprint(s) | Key Deliverables |
|-----------|-----------------|
| 1 | GitHub Actions CI, coverlet, 648 Core tests, snapshot tests |
| 2 | Async GUI, progress bar, tray icon, system theme, DPI-aware |
| 3 | FrozenDictionary, cache Categories/ScopeCounts, parallel StatusMap |
| 4 | Self-contained publish, winget manifest, WiX MSI, Scoop bucket |
| 5 | JSON Plugin/Pack system with marketplace, German locale |
| 6 | Documentation overhaul, CHANGELOG, Scoop manifest |
| 7 | RegisterBuiltins perf profiling, 15 new tweaks (Recall, Debloat, Proxy) |
| 8 | Untrack archive/, ValidateTweaks, ResolveDependencies, Dependents, CLI --depends-on/--no-color |
| 9 | `update <id>` CLI, Analytics integration, Filter/Update/Dependency tests |
| 10 | Snapshot tests, ExportJson, RegistrySession read ops, HardwareInfo & CorporateGuard tests |
| 11 | GUI log panel default-visible, all instruction files updated, 94.9% line coverage |
| 12 | Extract SnapshotManager, TweakValidator, DependencyResolver, CliArgs, ConsoleColorizer, PackageNameValidator |
| 13 | SnapshotManagerTests, TweakValidatorTests, DependencyResolverTests (+76 tests) |
| 14 | TweakEngineBuiltinsTests (shared fixture), +173 tests (972 total) |
| 15 | GUI test optimisation, 50 new tweaks (PowerManagement, CommandLine, Developer, Hardening, NetworkOpt) |
| 16 | Security audit (removed Telnet/TFTP/EFS tweaks), TweakValidator duplicate-registry detection |
| 17 | ConfigExporter, Favorites, TweakHistory services; 7 CLI commands; 50 tweaks; 40 tests |
| 18 | 7 new themes (11 total), AppIcons gradient overhaul, 50 tweaks (DNS, Enc, FW, Hardening, Recovery) |
| 19 | SystemMonitor (CPU/RAM/uptime P/Invoke), live status bar, 50 tweaks, 7 monitor tests |
| 20–22 | MSBuild centralisation, Locale hot cache, debounce timer, CI hardening, SHA256, 2 610 tweaks |
| 23 | TweakEngineCoverageTests (+62 tests), CorporateGuard Lazy refactor |
| 24 | Search highlighting, WhatsNew, WindowAppearance.cs (51 tweaks), PreferencesDialog |
| 25 | SystemOptimization.cs (39 tweaks), DesktopCustomization.cs (36 tweaks), 92 categories |
| 26 | WhatsNewDialog auto-show, detail panel fix, hang fix, 1 645 tests |
| 27 | NetworkManager service, NetworkToolsDialog (DNS quick-switch, TCP/IP repair) |
| 28 | StartupManager service + dialog |
| 29 | ServiceManager service + dialog (ServiceController NuGet) |
| 29–30 | BaseDialog consolidation, all 3 tool dialogs migrated |
| 30 | 8 new tool dialogs (ContextMenu, HostsFile, TempCleaner, InstalledApps, etc.), AppConfig enriched |
| 31 | PowerScheduler, SleepTimer, BatterySaver, UsbPower dialogs |
| 32 | AdRemovalWizard, TelemetryDashboard, AppPermissions, DnsOverHttps dialogs |
| 33 | NetworkRepair, DnsSwitcher, NetworkAdapter, WiFiProfile dialogs |
| 34 | FirewallRules, ProxyConfig, ShellExtension, BootTimeAnalyzer dialogs |
| 35 | WindowsUpdateControl dialog |
| 36 | NotificationManager, BrowserCacheCleaner, DriverUpdateChecker dialogs |
| 37 | WakeOnLan + BrightnessScheduler dialogs |
| 38 | French + Spanish locales (4 built-in) |
| 39 | Plugin URL install, conflict detector, marketplace tag filter |
| 40 | Before/After registry preview, Undo Last button |
| 41 | DiskSpace, PortScanner, BatteryHealth dialogs; 31 new tweaks |
| 42 | HardwareTemperature, NetworkBandwidth, MacAddress dialogs; 29 new tweaks |
| 43 | Version fix, ObjectDisposedException fix, Package Manager menu split, Hebrew locale, 50 tweaks |
| 44 | CRLF normalisation, Japanese locale (6 total), 50 new tweaks |
| 45 | 50 new tweaks (Audio, Gaming, Security, WindowsUpdate, RemoteDesktop), UpdateChecker + ComplianceService |
| 46 | v3.6.0 release — MSI, About icons, README badges, SECURITY.md |
| 47 | 50 new tweaks (Communication, Accessibility, Multimedia, Clipboard, VsCode), 5 dialog enhancements |
| 47b | v3.7.0 release — MSI pipeline fix, service API enhancements (+19 tests) |
| 48–56 | Bluetooth/Printing/TouchPen/Speech/Storage/Audio/Package/Maint/Scoop tweaks, 5 modules per sprint |
| 57 | ImpactScore + SafetyRating metadata on TweakDef; Impact/Safety badges in GUI and CLI |
| 58 | NLP synonym search — 60+ entry synonym map, multi-token AND logic with expansion |
| 59 | Portable mode — `--portable` flag + `AppConfig.SetPortable()`; auto-detect via sentinel file |
| 60 | Silent/unattended CLI mode — `--silent`, `--log-file`, exit-code-based scripting support |
| 61 | AutoUpdater service — GitHub Releases v3 API poller, `IsNewer()`, `UpdateInfo` record |
| 62 | HealthScoreService — Privacy/Performance/Security/Stability scores (0–100) from `StatusMap()` |
| 63 | 50 new tweaks: XboxGameBar.cs + WindowsHello.cs + SmartAppControl.cs + EnergySaver.cs + CopilotPlus.cs |
| 64 | FirstRunWizardDialog — 3-step onboarding wizard (profile + dry-run + feature tour) |
| 65 | ProfileWizardDialog — 5-question personalized profile generator |
| 66 | ConflictDetector service — static conflict pair table, Detect() + ConflictsFor() API |
| 67 | Sprint 57–66 tests (1538 total): HealthScoreServiceTests, AppConfigPortableTests, ConflictDetectorTests, NewTweakModulesTests, AutoUpdaterTests, TweakDefMetadataTests, TweakEngineSearchNlpTests |

---

## Competitive Analysis Summary

> Research from 2026-03-16 covering 13 top Win11 tweak tools.

| Tool | Type | Focus | Stars/Reach |
|------|------|-------|-------------|
| Winaero Tweaker | All-in-one | Hundreds of registry/UI tweaks | Millions of users |
| ExplorerPatcher | Shell patcher | Win10 taskbar/Start on Win11 | 31.9k★ |
| TranslucentTB | Taskbar transparency | Taskbar visual effects | 19.1k★ |
| Mem Reduct | Memory cleaner | Cache clearing | 8.9k★ |
| Open-Shell | Start menu | Classic Start menu | 8.7k★ |
| OFGB | Ad remover | Win11 ads only | 7.5k★ |
| StartAllBack | Start + taskbar | Win7/10 UI on Win11 | Paid |
| NetAdapter Repair | Network fixer | TCP/IP/Winsock reset | Free |
| MS PC Manager | System optimizer | Cleanup + boost | Free (MS) |

### RegiLattice Unique Strengths

- Most tweaks: 2 995 across 92 categories + declarative TweakDef model
- 5 profile system, plugin marketplace, Corporate Guard
- Dependency resolution, snapshot/restore, DryRun preview
- CLI + GUI, 11 themes, live system monitoring
- Validation engine + full test suite (1 879 tests, 95% line coverage)

---

## Future Roadmap — 100 Enhancement Items (10 Phases)

### Phase 1 — UX & Config Management ✅

| # | Item | Status |
|---|------|--------|
| 1 | Import/export tweak selections as JSON config file | ✅ Sprint 17 |
| 2 | Tweak favorites/bookmarks | ✅ Sprint 17 |
| 3 | Tweak history panel with undo | ✅ Sprint 17 |
| 4 | Search result highlighting | ✅ Sprint 24 |
| 5 | Recently applied tweaks section | ✅ Sprint 24 |
| 6 | Tweak comparison view | 🔄 Future |
| 7 | Bulk select by tag | 🔄 Future |
| 8 | Keyboard shortcuts | 🔄 Future |
| 9 | "What's New" dialog on version upgrade | ✅ Sprint 26 |
| 10 | Tweak tooltip with full description | 🔄 Future |

### Phase 2 — System Monitoring & Diagnostics ✅

| # | Item | Status |
|---|------|--------|
| 11 | Real-time memory stats in status bar | ✅ Sprint 19 |
| 12 | Memory cache cleaner | ✅ Sprint 41 |
| 13 | Auto memory cleaning on threshold | ✅ Sprint 42 |
| 14 | System tray memory usage indicator | ✅ Sprint 42 |
| 15 | CPU usage monitor in status bar | ✅ Sprint 19 |
| 16 | Disk usage overview panel | ✅ Sprint 41 |
| 17 | Network connectivity status indicator | ✅ Sprint 42 |
| 18 | Battery health monitor | ✅ Sprint 41 |
| 19 | System uptime in About dialog | ✅ Sprint 19 |
| 20 | Hardware temperature monitoring | ✅ Sprint 42 |

### Phase 3 — Visual Appearance Tweaks ✅

| # | Item | Status |
|---|------|--------|
| 21–30 | Title bar, scrollbar, fonts, icons, borders, animations, accent colours | ✅ Sprint 24 |

### Phase 4 — Network & Connectivity Tools ✅

| # | Item | Status |
|---|------|--------|
| 31 | One-click network repair wizard | ✅ Sprint 33 |
| 32 | DNS server quick-switch | ✅ Sprint 33 |
| 33 | Network adapter diagnostics | ✅ Sprint 33 |
| 34 | Wi-Fi profile management | ✅ Sprint 33 |
| 35 | Proxy configuration wizard | ✅ Sprint 34 |
| 36 | Firewall rule manager | ✅ Sprint 34 |
| 37 | Port scanner / connectivity tester | ✅ Sprint 41 |
| 38 | Network bandwidth monitor | ✅ Sprint 42 |
| 39 | VPN quick-connect from tray | 🔄 Future |
| 40 | MAC address randomization | ✅ Sprint 42 |

### Phase 5 — Startup & Service Management ✅

| # | Item | Status |
|---|------|--------|
| 41 | Startup manager | ✅ Sprint 28 |
| 42 | Service manager | ✅ Sprint 29 |
| 43 | Scheduled task manager | ✅ Sprint 28 |
| 44 | Boot time analyzer | ✅ Sprint 34 |
| 45 | Context menu manager | ✅ Sprint 30 |
| 46 | Shell extension manager | ✅ Sprint 34 |
| 47 | Installed programs quick-uninstaller | ✅ Sprint 30 |
| 48 | Temporary file cleaner | ✅ Sprint 30 |
| 49 | Windows Update pause/resume | ✅ Sprint 35 |
| 50 | Driver update checker | ✅ Sprint 36 |

### Phase 6 — Power & Energy Management ✅

| # | Item | Status |
|---|------|--------|
| 51 | Power plan quick-switch from tray | ✅ Sprint 29–30 |
| 52 | Timer-based power plan switching | ✅ Sprint 31 |
| 53 | Custom power plan creator | ✅ Sprint 31 |
| 54 | Battery saver automation | ✅ Sprint 31 |
| 55 | Sleep/hibernate timer | ✅ Sprint 31 |
| 56 | Monitor power-off timer | ✅ Sprint 31 |
| 57 | USB selective suspend | ✅ Sprint 31 |
| 58 | Wake-on-LAN configuration | ✅ Sprint 37 |
| 59 | Power consumption estimator | 🔄 Future |
| 60 | Screen brightness scheduler | ✅ Sprint 37 |

### Phase 7 — Privacy & Ad Removal ✅

| # | Item | Status |
|---|------|--------|
| 61 | Desktop ad removal wizard | ✅ Sprint 32 |
| 62 | Notification manager | ✅ Sprint 36 |
| 63 | Browser cache & cookie cleaner | ✅ Sprint 36 |
| 64 | Telemetry dashboard | ✅ Sprint 32 |
| 65 | Privacy score | ✅ Sprint 29 |
| 66 | Hosts file manager | ✅ Sprint 30 |
| 67 | Browser privacy overview | ✅ Sprint 36 |
| 68 | DNS-over-HTTPS quick setup | ✅ Sprint 32 |
| 69 | Location services control | ✅ Sprint 32 |
| 70 | App permission manager | ✅ Sprint 32 |

### Phase 8 — Plugin & Extensibility Improvements

| # | Item | Status |
|---|------|--------|
| 71 | Plugin sandboxing | 🔄 Future |
| 72 | Plugin auto-update | ✅ Sprint 36 |
| 73 | Plugin rating and review | 🔄 Future |
| 74 | Plugin dependency resolution | 🔄 Future |
| 75 | Plugin template generator | 🔄 Future |
| 76 | Community plugin submission workflow | 🔄 Future |
| 77 | Plugin categories/tags in marketplace | ✅ Sprint 39 |
| 78 | Plugin install from URL | ✅ Sprint 39 |
| 79 | Plugin changelog viewer | 🔄 Future |
| 80 | Plugin conflict detector | ✅ Sprint 39 |

### Phase 9 — Advanced Features & Automation

| # | Item | Status |
|---|------|--------|
| 81 | Scheduled tweak application | 🔄 Future |
| 82 | Before/after preview | ✅ Sprint 40 |
| 83 | Tweak rollback queue | ✅ Sprint 40 |
| 84 | Profile scheduler | 🔄 Future |
| 85 | REST API | 🔄 Future |
| 86 | Web dashboard | 🔄 Future |
| 87 | PowerShell module wrapper | 🔄 Future |
| 88 | Group Policy export (.admx/.adml) | 🔄 Future |
| 89 | Intune/SCCM integration | 🔄 Future |
| 90 | Compliance reporting (drift detection) | 🔄 Future |

### Phase 10 — Localization, Packaging & Community

| # | Item | Status |
|---|------|--------|
| 91 | French locale | ✅ Sprint 38 |
| 92 | Spanish locale | ✅ Sprint 38 |
| 93 | Japanese locale | ✅ Sprint 44 |
| 94 | Chocolatey package | 🔄 Future |
| 95 | Microsoft Store listing | 🔄 Future |
| 96 | Code signing | 🔄 Future |
| 97 | Auto-update mechanism | 🔄 Future |
| 98 | Portable mode | 🔄 Future |
| 99 | Community tweak submission form | 🔄 Future |
| 100 | Documentation site (mkdocs/docfx) | 🔄 Future |

---

## Prioritized Backlog

### P0 — Critical ✅

- [x] GitHub Actions CI workflow (.NET build + test)
- [x] Self-contained single-file publish
- [x] Async GUI operations (no UI thread blocking)

### P1 — High Value ✅

- [x] Coverage reporting with coverlet
- [x] CLI test coverage, DPI-aware GUI scaling
- [x] Parallel `StatusMap()` optimization + FrozenDictionary
- [x] winget manifest, GitHub Releases automation

### P2 — Medium Value

- [x] Snapshot round-trip tests
- [x] System theme auto-detection, export to .REG
- [x] Plugin system (JSON Tweak Packs with marketplace)
- [x] User-defined tweaks via JSON, Scoop bucket
- [ ] Lazy module loading, Code signing

### P3 — Nice to Have

- [x] Tray icon, German/French/Spanish/Japanese locales
- [ ] Scheduled tweak application, REST API, Web dashboard, Chocolatey

---

## Planned Sprints (48 – 96)

> Each sprint targets ≥50 new tweaks and enhances existing dialogs by ≥2 items.

| Sprint | Theme | Target Tweaks |
|--------|-------|---------------|
| 48 | Bluetooth, Printing, TouchPen, Speech, Storage tweaks + 5 dialog enhancements | 3 046 |
| 49 | Plugin template generator + changelog viewer, Copilot/ScoopTools/DevDrive/Java tweaks | 3 096 |
| 50 | Scheduled tweaks service + dialog, Gaming/GPU/Boot/Win11 tweaks | 3 146 |
| 51 | Profile scheduler, MsStore/Edge/Firefox/Chrome/Office tweaks | 3 196 |
| 52 | GPO export dialog, compliance reporting, Firewall/Enc/Hardening tweaks | 3 246 |
| 53 | VPN quick-connect, power estimator, Network/DNS/Proxy tweaks | 3 296 |
| 54 | Plugin ratings + dependencies, Performance/Memory/SSD/Desktop tweaks | 3 346 |
| 55 | Portable mode, auto-update, Privacy/Cortana/Widgets/Copilot tweaks | 3 396 |
| 56 | PowerShell module, Korean locale, PowerShell/CommandLine/Developer tweaks | 3 446 |
| 57 | Chinese (zh-CN) locale, community pipeline, Taskbar/LockScreen/NightLight tweaks | 3 496 |
| 58 | Compliance history dialog, UserAccount/Backup/FileSystem/DiskCleanup tweaks | 3 546 |
| 59 | System resource dashboard, event log viewer, Explorer/Shell/CloudStorage tweaks | 3 596 |
| 60 | Profile manager dialog, Power/PowerManagement/Virtualization/WSL tweaks | 3 646 |
| 61 | CLI enhancement pack + 50 tweaks | 3 696 |
| 62 | Hardware analytics dashboard + 50 tweaks | 3 746 |
| 63 | **v3.7.0 Release** — full changelog + 50 tweaks | 3 796 |
| 64–74 | Interactive console, security packs, bulk tag, tooltips, network security, registry explorer, disk health, printer manager, gaming tools, advanced search, **v3.8.0** | 4 096–4 396 |
| 75 | **v3.8.0 Release** ✅ | 3 669 |
| 76–95 | Task scheduler, credential manager, font manager, plugin sandboxing, Polish/Italian/Korean/Arabic/Dutch locales, remote management, developer tools, touch/accessibility, USB manager, pack creator studio, Store manager, benchmarks | 4 000–4 800 |
| 96 | **v4.0.0 Major Release** | 5 000+ |

---

## ★ Next Phase Master Plan — "World-Class Windows 11 Configurator"

> Strategic plan for v3.9.0 → v4.0.0 → v4.x.
> Objective: Outcompete every Windows tweak tool across trust, UX, intelligence, automation, and community.
> Created: 2026-07-21 · Baseline: v3.8.0 · 3 669 tweaks · 94 categories
> Status legend: ⬜ Not started · 🔄 In progress · ✅ Done

---

### Competitive Gap Analysis

| Capability | RegiLattice | Winaero Tweaker | Chris Titus WinUtil | O&O ShutUp10++ |
|---|---|---|---|---|
| Tweak depth | **3 669 ✅** | ~400 | ~200 | ~200 |
| DryRun preview | ✅ | ❌ | ❌ | ❌ |
| Registry backup | ✅ | ✅ | ❌ | ✅ |
| CLI interface | ✅ | ❌ | Partial | ❌ |
| Plugin ecosystem | ✅ | ❌ | ❌ | ❌ |
| Corporate Guard | ✅ | ❌ | ❌ | ❌ |
| Code signing | ❌ → Phase A | ✅ | N/A | ✅ |
| Auto-updater | Detect-only | ✅ | ✅ | ✅ |
| Portable mode | ❌ → Phase A | ✅ | ✅ | ✅ |
| Health/score dashboard | Basic | ❌ | ❌ | Simple |
| Intelligent recommendations | ❌ → Phase C | ❌ | ❌ | ❌ |
| PowerShell module | ❌ → Phase D | ❌ | ❌ | ❌ |
| GPO/Intune export | Partial | ❌ | ❌ | ❌ |
| WinUI 3 / Fluent UI | ❌ → Phase E | ❌ | Partial | ❌ |
| Compliance reporting | Partial | ❌ | ❌ | ❌ |
| Community marketplace | ✅ (local) | ❌ | GitHub | ❌ |
| AI/NLP search | ❌ → Phase G | ❌ | ❌ | ❌ |

**Key insight**: RegiLattice leads on depth, safety, and extensibility.
The gaps to close: trust (signing), distribution (auto-update/portable), intelligence (health score/recommendations), modern UI, enterprise automation.

---

### Phase A — Distribution & Trust (v3.9.0) — Partial ✅
> Goal: Remove every friction point between a new user and their first successful tweak.

| # | Item | Priority | Effort |
|---|------|----------|--------|
| A1 | **Authenticode code signing** — EV certificate for GUI + CLI EXE | P0 | High |
| A2 | **Built-in auto-updater** — query GitHub Releases API, show badge, download + relaunch | P0 | Medium |
| A3 | **Portable mode** — `--portable` flag writes all data to `.\data\` instead of `%LOCALAPPDATA%` | P0 | Low |
| A4 | **Silent/unattended mode** — `--silent` + `--log-file` for scripting | P1 | Low |
| A5 | **Chocolatey package** — publish to community.chocolatey.org | P1 | Low |
| A6 | **MSIX packaging** — clean install/uninstall via Windows Package Manager, supports Store | P1 | Medium |
| A7 | **First-run wizard** — 3 screens: choose profile, dry-run toggle, brief feature tour | P1 | Medium |
| A8 | **Onboarding health check** — on first launch, run `StatusMap()` and show "X tweaks recommended for your hardware" | P1 | Low |

**Completed**: A2 ✅ (Sprint 61), A3 ✅ (Sprint 59), A4 ✅ (Sprint 60), A7 ✅ (Sprint 64), A8 ✅ (Sprint 64/62)
**Remaining**: A1 (code signing), A5 (Chocolatey), A6 (MSIX)

---

### Phase B — UX Modernization (v3.9.x)
> Goal: A UI that feels native to Windows 11 and fast with 3 669 tweaks.

| # | Item | Priority | Effort |
|---|------|----------|--------|
| B1 | **Virtual scrolling** — replace `FlowLayoutPanel` with owner-drawn `VirtualMode` `ListView`. Renders 3 669 tweaks instantly (from ~30 visible Controls instead of 3 669 live Controls) | P0 | High |
| B2 | **Tweak detail side panel** — slide-out panel: full description, registry paths, tags, impact rating, dependency chain, last applied timestamp | P0 | Medium |
| B3 | **Animated toggle switch control** — custom `ToggleSwitchControl` (smooth ON/OFF animation) replaces checkboxes | P1 | Medium |
| B4 | **Card / List view toggle** — card mode: tweak name + one-line desc + impact badge + toggle; list mode: current behavior | P1 | Medium |
| B5 | **Complete keyboard shortcut scheme** — `Ctrl+F` focus search, `Space` toggle selected, `Ctrl+Z` undo last, `F5` refresh, `Ctrl+A` select all | P1 | Low |
| B6 | **Multi-select operations** — `Shift+Click`, `Ctrl+Click`, right-click menu: Apply Selected / Remove Selected / Add to Profile | P1 | Low |
| B7 | **Rich tweak tooltips** — hover popup: description, expected result, registry path, safety rating | P2 | Low |
| B8 | **Tag chip filter sidebar** — clickable tag chips replacing the current dropdown filter | P2 | Medium |
| B9 | **Drag-to-profile** — drag tweaks from the main list onto a profile name in the sidebar | P2 | High |
| B10 | **Bulk select by tag** — right-click a tag chip → "Select all X tweaks with this tag" | P2 | Low |

**Architecture note — B1 is the most critical performance fix:**
Current `FlowLayoutPanel` instantiates a `Control` per tweak (3 669 live WinForms objects). Switching to `ListView` with `VirtualMode = true` + `DrawItem` overrides reduces live controls to ~30 (viewport), cutting UI thread pressure by ~99%.

---

### Phase C — Intelligence Engine (v4.0.0) — Partial ✅
> Goal: The only tweak tool that tells you *what* to apply and *why* — not just an endless list.

| # | Item | Priority | Effort |
|---|------|----------|--------|
| C1 | **System Health Score Dashboard** — four weighted sub-scores: Privacy (0-100), Performance (0-100), Security (0-100), Stability (0-100) derived live from `StatusMap()`. New Dashboard tab with `CircleProgress` rings | P0 | High |
| C2 | **Smart Scan & Quick Wins** — async on startup: detect installed apps, hardware, locale → surface the 20 highest-impact unapplied tweaks for *this specific machine*. "These 8 tweaks are safe and will boost your Privacy score by +34" | P0 | Medium |
| C3 | **Impact & Safety metadata on TweakDef** — new fields: `int ImpactScore` (1-5, benefit magnitude) and `int SafetyRating` (1-5, risk level). GUI shows color-coded badges. CLI shows in `--list`. Add to all 3 669 tweaks | P0 | Medium |
| C4 | **Conflict detection engine** — `ConflictDetector.cs` maintains known-conflicting tweak pairs. Warn before apply: "This conflicts with `svc-disable-winsearch` which you already applied" | P1 | Medium |
| C5 | **Before/After score prediction** — before applying a batch, show "Predicted change: Privacy +28, Performance +12, Security +5" | P1 | Low |
| C6 | **Dependency chain visualizer** — `DependencyGraphDialog.cs` with GDI+ node graph showing `DependsOn` relationships. Click a node to jump to that tweak | P2 | Medium |
| C7 | **Profile comparison view** — side-by-side diff of two profiles or profile vs. current applied state | P2 | Medium |

**Completed**: C1 ✅ (Sprint 62 — HealthScoreService), C3 ✅ (Sprint 57 — ImpactScore/SafetyRating), C4 ✅ (Sprint 66 — ConflictDetector)
**Remaining**: C2 (Smart Scan), C5 (score prediction), C6 (dep graph), C7 (profile diff)

---

### Phase D — Enterprise & Automation (v4.0.x)
> Goal: Make RegiLattice the go-to tool for IT admins deploying Windows 11 at scale.

| # | Item | Priority | Effort |
|---|------|----------|--------|
| D1 | **PowerShell module** (`RegiLattice.psd1`) — cmdlets: `Get-Tweak`, `Get-TweakStatus`, `Apply-Tweak`, `Remove-Tweak`, `New-Profile`, `Get-HealthScore`. Pipeline-aware output objects, `Format-Table` defaults, tab completion via `ArgumentCompleter` | P0 | High |
| D2 | **Full compliance drift detection** — extend `ComplianceService`: scheduled daily scan, Windows toast notification on drift, `ComplianceHistory` log, `--compliance-report auto` CLI flag | P0 | Medium |
| D3 | **HTML/PDF compliance report** — printable report listing applied/unapplied tweaks per category, health score trend chart, drift since last snapshot. Export from GUI + CLI | P1 | Medium |
| D4 | **ADMX/ADML GPO export** — for all Registry-kind tweaks: generate `.admx` + `.adml` deployable via Group Policy. Extends existing `GroupPolicyExporter.cs` | P1 | High |
| D5 | **Intune OMA-URI export** — map applied Registry tweaks to Intune Custom Configuration Profile JSON (`./Vendor/MSFT/Policy/Config/...` OMA-URI paths where applicable) | P1 | Medium |
| D6 | **Silent/unattended mode** — `RegiLattice.exe --silent --profile gaming --log result.json` — zero UI, JSON output log, exit code 0/1 for CI pipeline integration | P1 | Low |
| D7 | **Local REST API** — `RegiLattice.exe --serve 8765` starts `HttpListener`: `GET /tweaks`, `POST /tweaks/{id}/apply`, `GET /health-score`, `GET /profiles` — for RPA/automation | P2 | High |
| D8 | **GitHub Actions workflow template** — ship `.github/workflow-templates/regilattice-configure.yml` for provisioning dev machines in CI | P2 | Low |

**New services/classes needed:**
- `PowerShellModuleGenerator.cs` (`dotnet publish` post-step) — emits `.psd1` and `.psm1` wrapping the Core library
- `IntuneExporter.cs` — translates `RegOp` HKLM paths to Intune OMA-URI; handles CSP path mapping
- `RegiLatticeApiServer.cs` — `HttpListener`-based; routes parsed with simple string matching; serializes with `System.Text.Json`

---

### Phase E — Platform Modernization (v4.1.0)
> Goal: A UI that Windows 11 users recognize as native — Fluent Design, Mica, rounded corners.

| # | Item | Priority | Effort |
|---|------|----------|--------|
| E1 | **WinForms deep polish** (immediate, lower risk) — custom `RoundedPanel`, `ShadowBorder`, `GlassCard` controls; smooth animations via `System.Threading.Timer`; Segoe Fluent Icons; Mica-like blur approximation | P0 | Medium |
| E2 | **WinUI 3 migration** (strategic) — rewrite GUI project targeting WinAppSDK 1.7+. Full Mica, `ToggleSwitch`, `NavigationView`, `InfoBar`, animated. Win11-native look | P1 | Very High |
| E3 | **Modern NavigationView** — replace current tab/panel with left-rail nav: Dashboard, Tweaks, Profiles, Tools, Marketplace, Settings | P1 | High |
| E4 | **MSIX packaging** — `.msix` alongside MSI for Microsoft Store submission | P1 | Medium |
| E5 | **Windows 11 Jumplist** — taskbar right-click: "Apply Gaming Profile", "Run Smart Scan", "Open Dashboard" | P2 | Low |
| E6 | **Toast notifications** — Action Center: compliance drift, scheduled tweak done, update available | P2 | Low |
| E7 | **Full keyboard/screen reader accessibility** — `AutomationProperties` on all custom controls; keyboard-only full navigation | P2 | Medium |

**Recommended path**: WinForms polish (E1) first in v3.9.x for quick visual wins, WinUI 3 (E2–E3) as dedicated v4.1.0 effort spanning 6–8 sprints.

---

### Phase F — Community & Ecosystem (v4.1.x)
> Goal: Make RegiLattice the center of gravity for Windows tweak knowledge.

| # | Item | Priority | Effort |
|---|------|----------|--------|
| F1 | **Curated online pack marketplace** — `RajwanYair/RegiLattice-Packs` GitHub repo as CDN. Launch categories: "Privacy First", "Gaming", "Work from Home", "Corporate Hardened", "Minimal" | P0 | Medium |
| F2 | **Pack Creator Studio** — GUI wizard: name/desc → add tweaks (drag from main list) → set tags → preview + validate → export `.rlpack` JSON | P1 | High |
| F3 | **Deep-link URLs** — `regilattice://apply?id=priv-disable-telemetry` protocol handler + "Copy Share Link" button | P1 | Medium |
| F4 | **Community safety ratings** — crowd-sourced "X users apply this safely" count. Opt-in analytics aggregate stored in marketplace CDN | P2 | High |
| F5 | **Plugin sandboxing** — run `ApplyAction` delegates from packs in separate process via named pipes, contain crashes from third-party packs | P2 | High |
| F6 | **Pack signing** — SHA256 + optional GPG signature on marketplace packs; verification dialog before install | P2 | Medium |
| F7 | **More locales** — Chinese (zh-CN/zh-TW), Korean, Arabic (RTL layout), Polish, Italian, Dutch, Portuguese. Migrate to `.resx` `ResourceManager` | P2 | Medium per locale |
| F8 | **Documentation site** — `docfx` auto-generated from XML doc comments + hand-written guides, hosted on GitHub Pages (`RajwanYair.github.io/RegiLattice`) | P2 | Medium |

---

### Phase G — AI & Natural Language (v4.2.0) — Partial ✅
> Goal: Let non-technical users describe what they want in plain English.

| # | Item | Priority | Effort |
|---|------|----------|--------|
| G1 | **NLP search with synonym expansion** — pre-built synonym map in `SearchService` (no external AI): "fast/performance/speed" → `perf` tags, "private/tracking" → `priv` tags, "clean/bloat" → `debloat`. Replaces simple substring match | P0 | Low |
| G2 | **Profile wizard (5 questions)** — "Are you a gamer? Does your PC run hot? How important is privacy 1-5?" → weighted scoring → generates a custom one-time `TweakProfile` | P1 | Medium |
| G3 | **Score-change previews on hover** — hover a category header: "Applying all 14 Privacy tweaks here would raise your Privacy score by +23 pts" | P1 | Low |
| G4 | **Optional LLM integration** — connect to local Ollama or Azure OpenAI (strictly opt-in, zero data leaves without consent): natural language → tweak list; plain-English tweak explanation | P2 | High |
| G5 | **AI-enhanced tweak descriptions** — one-time pass: use an LLM to generate clearer `Description` + `ExpectedResult` for all 3 669 tweaks; commit improved strings as source code | P2 | Medium |

**Completed**: G1 ✅ (Sprint 58 — synonym map in `TweakEngine`), G2 ✅ (Sprint 65 — `ProfileWizardDialog`)
**Remaining**: G3 (score previews), G4 (LLM integration), G5 (AI descriptions)

---

### Phase H — New Tweak Categories (targeting 5 000 tweaks) — Partial ✅

| Module | Focus | Est. Tweaks | Status |
|--------|-------|-------------|--------|
| `WindowsHello.cs` | PIN, biometrics, FIDO2 pass-through registry | 10 | ✅ Sprint 63 |
| `SmartAppControl.cs` | SAC policy, WDAC lightweight settings | 10 | ✅ Sprint 63 |
| `XboxGameBar.cs` | Game Bar, Game DVR, overlay, screenshots | 10 | ✅ Sprint 63 |
| `EnergySaver.cs` | Win11 24H2 Energy Saver, CPU efficiency mode | 10 | ✅ Sprint 63 |
| `CopilotPlus.cs` | NPU policy, Recall advanced, AI-PC controls | 10 | ✅ Sprint 63 |
| `BitLockerAdvanced.cs` | Pre-boot auth, TPM PCR policy, recovery | 12 | ⬜ Future |
| `AppLockerWdac.cs` | AppLocker policy registry keys | 10 | ⬜ Future |
| `HyperVAdvanced.cs` | vCPU scheduler, MMIO, vNUMA, SLAT | 10 | ⬜ Future |
| `WindowsSandboxAdv.cs` | Sandbox networking, vGPU, clipboard isolation | 8 | ⬜ Future |
| `PrinterAdvanced.cs` | Spooler hardening, Point-and-Print restrictions | 10 | ⬜ Future |
| **Total new** | | **~100 tweaks → ~3 819 total** | |

Plus continued sprint cycles (+50/sprint) reach **5 000 tweaks** by Sprint ~95.

---

### Phase I — Testing Excellence (ongoing)

| # | Item | Priority | Effort |
|---|------|----------|--------|
| I1 | **Virtual registry integration tests** — load a temporary hive with `RegLoadKey` API; run actual `Apply`/`Remove`/`Detect` on the isolated hive; no real system state touched | P1 | High |
| I2 | **Screenshot/visual regression tests** — `WinAppDriver` or `UIAutomation` capture dialogs; diff against golden images in CI | P1 | High |
| I3 | **BenchmarkDotNet suite** — `RegisterBuiltins()` time, `StatusMap()` throughput, `Search()` latency baselines. Regression tracked in CI artifact | P1 | Medium |
| I4 | **Property-based tests (FsCheck)** — invariant checks on all 3 669 built-in tweaks: non-null ID, valid hive paths, no duplicate registry key+value combos | P2 | Low |
| I5 | **Mutation testing (Stryker.NET)** — target 70%+ mutation score on Core library | P2 | Medium |

---

### Recommended Execution Order

```
v3.9.0   Sprint 58-60  — Phase A (portable, auto-updater prep) + Phase B-B1 (virtual ListView)
v3.9.1   Sprint 61-63  — Phase C (health score + smart scan + impact metadata)
v3.9.2   Sprint 64-66  — Phase D-D1/D2/D3 (PowerShell module + compliance report)
v4.0.0   Sprint 67-72  — Phase D complete + Phase E-E1 (WinForms polish + MSIX)
v4.1.0   Sprint 73-82  — Phase E-E2 (WinUI 3 migration) + Phase F (community/marketplace)
v4.2.0   Sprint 83-96  — Phase G (AI/NLP) + Phase H (new tweaks) + Phase I (testing)
```

---

### Success Metrics (v4.0.0 Definition of Done)

| Metric | v3.8.0 | v4.0.0 Target |
|--------|--------|---------------|
| Tweaks | 3 669 | 4 500+ |
| Tests | 1 414 | 2 000+ |
| Code signed | ❌ | ✅ Authenticode |
| Auto-updater | Detect-only | ✅ Download + install |
| Portable mode | ❌ | ✅ |
| Health score dashboard | ❌ | ✅ 4 sub-scores |
| PowerShell module | ❌ | ✅ Full cmdlet set |
| WinForms polish | Basic | ✅ Custom toggle controls, animations |
| WinUI 3 | ❌ | 🔄 In progress (v4.1.0) |
| Locales | 6 | 10+ |
| Pack marketplace | Local-only | ✅ Online curated |
| NLP search | Substring | ✅ Synonym-expanded |
| Intune/GPO export | Partial | ✅ Full OMA-URI |
