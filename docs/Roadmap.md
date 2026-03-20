# RegiLattice — Roadmap

> Living document — updated after every sprint.
> Last updated: 2026-07-17 · v3.7.0 · 2 995 tweaks · 92 categories · 1 879 tests

---

## Current State (as of v3.7.0)

| Metric | Value |
|--------|-------|
| Language | C# 13 / .NET 10.0-windows (x64) |
| Tweaks | 2 995 verified across 92 categories |
| Tests | 1 879 total, all passing |
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
| 75 | **v3.8.0 Release** | 4 396 |
| 76–95 | Task scheduler, credential manager, font manager, plugin sandboxing, Polish/Italian/Korean/Arabic/Dutch locales, remote management, developer tools, touch/accessibility, USB manager, pack creator studio, Store manager, benchmarks | 4 446–5 346 |
| 96 | **v4.0.0 Major Release** | 5 346 |
