# RegiLattice — Roadmap

> Single source of truth — `Roadmap-v5.md` is archived and redirects here.
> Last updated: 2026-03-23 · v4.5.0 · 4 108 tweaks · 121 categories · ~2 540 tests
> ✅ **M2 complete** (Q2 / v4.5.0). Q3 starts with Sprint 122.

---

## Current State

| Metric | Value |
|--------|-------|
| Version | **4.5.0** |
| Language | C# 13 / .NET 10.0-windows (x64) |
| Tweaks | **4 108** across **121 categories** (126 module files) |
| Tests | **~2 540** passing (2 007 Core + 291 CLI + 242 GUI), 0 failures |
| Branch Coverage | **75.14%** — M2 gate ✅ |
| GUI | WinForms · 11 themes · 62+ dialogs · live CPU/RAM · tray icon |
| Profiles | 5 (business, gaming, privacy, minimal, server) |
| Services | 32 Core services |
| Locales | 6 (en, de, fr, es, ja, he) — 200+ GUI string keys |
| CLI | 30+ commands · PowerShell module (`RegiLattice.psd1`) |
| Enterprise | ADMX/ADML export · Intune OMA-URI export |
| Distribution | GitHub Releases · Scoop (auto-SHA) · WinGet · MSI (WiX) |
| CI/CD | GitHub Actions: build + test + coverage + CodeQL + release + SHA256SUMS |
| Benchmarks | BenchmarkDotNet suite established (Sprint 121) |
| Repo | [github.com/RajwanYair/RegiLattice](https://github.com/RajwanYair/RegiLattice) |

---

## Vision

Make RegiLattice the **reference Windows registry tweak toolkit** — production-grade, enterprise-ready, and accessible:

- **Deepest coverage**: 4 108+ tweaks vs. ~400 for nearest competitor
- **Zero-lag GUI**: Virtual scrolling handles 5 000+ tweaks with ~30 live controls
- **Trusted distribution**: Code-signed, auto-updating via GitHub / Scoop / WinGet / Chocolatey / MSIX
- **Enterprise-ready**: Compliance automation, ADMX/GPO/Intune export, fleet deployment templates
- **Accessible & global**: WCAG 2.1 AA, 10+ locales, screen-reader navigable

---

## Development History

| Era | Version | Highlights |
|-----|---------|------------|
| Python prototype | v1.x | ~1 490 tweaks · tkinter GUI · 5 profiles · plugin concept |
| C# migration | v2.0.0 | 68 modules · WinForms GUI (4 themes) · CLI (25+ cmds) · 203 tests |
| Quality & features | v3.0–v3.7 | TweakKind · CI/CD · 11 themes · 85 dialogs · 3 868 tweaks · 1 647 tests |
| Services & intelligence | v3.8–v4.2 | ImpactScore · NLP search · HealthScore · ConflictDetector · AutoUpdater · ProfileWizard |
| Compliance & power tools | v4.3.0 | ComplianceHistory · PowerShell module · ToastNotifications · SmartScan · 1 858 tests |
| Security tweaks | v4.4.0 ✅ M1 | WDAC/ASR · BitLocker To Go · LAPS · NTLM · DeviceInstall — **4 108 tweaks** · 121 categories |
| Enterprise & trust | v4.5.0 ✅ M2 | ADMX/ADML + Intune export · auto-updater install · 200+ locale keys · 75.14% branch coverage · BenchmarkDotNet |

---

## Strategic Themes (T1 – T8)

> Status legend: ✅ Done · 🔄 In progress · ⬜ Planned

---

### T1 — GUI Performance & UX Modernization *(P0 — Q3–Q4)*

`MainForm` instantiates a WinForms `Control` per tweak (~4 108 live objects). Virtual scrolling reduces this to ~30 visible rows at any time — the single highest-impact change remaining.

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T1.1 | **Virtual ListView** — replace `FlowLayoutPanel` with `VirtualMode` `ListView`; owner-draw rows with toggle, label, scope/kind badges | ⬜ | 122–123 |
| T1.2 | **Tweak detail side panel** — full description, registry paths, tags, impact score, dependency chain, last-applied timestamp | ⬜ | 124 |
| T1.3 | **Keyboard shortcuts** — `Ctrl+F`, `Space` toggle, `Ctrl+Z` undo, `F5` refresh, `Ctrl+A`, `Escape` | ⬜ | 124 |
| T1.4 | **Multi-select operations** — `Shift+Click`, `Ctrl+Click`, context menu: Apply / Remove / Export Selected | ⬜ | 124 |
| T1.5 | **Animated toggle switch control** — custom GDI+ `ToggleSwitchControl`, theme-aware, DPI-safe | ⬜ | 131 |
| T1.6 | **WinForms visual polish** — rounded panels, Segoe Fluent Icons, Mica-like tinted `Form.BackColor` | ⬜ | 131 |
| T1.7 | **Tag chip filter sidebar** — clickable tag chips replacing dropdown; count badges; AND logic | ⬜ | 132 |
| T1.8 | **Rich hover tooltips** — description, expected result, safety badge; 400 ms show / 5 s hide | ⬜ | 132 |

---

### T2 — Accessibility & Internationalization *(P0 — Q3)*

GUI controls have no `AccessibleName`/`AccessibleDescription`. `Locale.T()` now covers 200+ keys but `.resx` migration and 4 additional locales remain.

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T2.1 | **Accessibility audit** — `AccessibleName` + `AccessibleDescription` on every interactive control across all 62+ dialogs; explicit `TabIndex` | ⬜ | 124 |
| T2.2 | **Keyboard-only navigation** — full tab order, `&` accelerators on all buttons/menus, Narrator verification | ⬜ | 124 |
| T2.3 | **Locale string extraction** — 200+ keys covering all GUI labels, dialogs, error messages across 6 languages | ✅ | Sprint 120 |
| T2.4 | **Migrate to `.resx` ResourceManager** — replace hand-rolled `Dictionary` in `Locale.cs`; enables satellite assemblies | ⬜ | 126 |
| T2.5 | **4 new locales** — zh-CN, ko, ar (RTL layout), pt-BR → 10 total | ⬜ | 126 |
| T2.6 | **High-contrast theme** — uses system high-contrast colours, large fonts, thick borders | ⬜ | 126 |

---

### T3 — CLI Overhaul & Scripting Power *(P1 — Q3)*

53 properties in `CliArgs.cs`, hand-rolled parser. Subcommands and JSON output were planned for Q1 but deferred to Q3 in favour of enterprise features.

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T3.1 | **Subcommand structure** — `regilattice tweak apply <id>`, `profile apply gaming`; old `--flags` remain as aliases | ⬜ | 127 |
| T3.2 | **Structured JSON output** — `--output json` on all commands; documented exit codes 0/1/2/3 | ⬜ | 127 |
| T3.3 | **Grouped `--help`** — sections: Tweak Ops / Profiles / Snapshots / Diagnostics / Advanced; per-command help | ⬜ | 127 |
| T3.4 | **Tab completion** — `Register-ArgumentCompleter` for PowerShell; bash completion script | ⬜ | 127 |
| T3.5 | **Interactive TUI overhaul** — arrow-key nav, search-as-you-type, ANSI (no external lib) | ⬜ | 128 |
| T3.6 | **Batch file support** — `--batch <file>` reads `.txt`/`.json` tweak ID list; per-tweak results | ⬜ | 128 |

---

### T4 — Enterprise & Compliance Automation *(P1 — partially done)*

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T4.1 | **ADMX/ADML Group Policy export** — valid XML for all HKLM Registry-kind tweaks, importable in `gpedit.msc` | ✅ | 116–117 |
| T4.2 | **Intune OMA-URI export** — CSP-mapped JSON for Intune Custom Profiles; unmappable flagged with `[NOT_MAPPABLE]` | ✅ | 118–119 |
| T4.3 | **Scheduled compliance scans** — daily/weekly drift check, toast on violation, optional `--compliance-auto-fix` | ⬜ | 128 |
| T4.4 | **Compliance trend dashboard** — `ComplianceTrendDialog` line chart from `ComplianceHistory`; export PNG | ⬜ | 128 |
| T4.5 | **Baseline policy templates** — 4 built-in: CIS L1 Desktop, CIS L1 Server, DISA STIG Win11, RegiLattice Recommended | ⬜ | 129 |
| T4.6 | **Multi-machine deployment template** — GitHub Actions workflow to provision dev machines via Scoop + profile apply | ⬜ | 129 |

---

### T5 — Distribution, Trust & Release Pipeline *(P1 — partially done)*

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T5.1 | **Authenticode code signing** — EV cert; sign GUI + CLI + MSI in CI; PFX in GitHub Secrets | 🔄 | 114 |
| T5.2 | **Auto-updater: download + guided install** — download MSI → verify SHA-256 → "Install & Restart" on user click | ✅ | Sprint 115 |
| T5.3 | **Chocolatey community package** — `.nuspec` + `chocolateyinstall.ps1`; CI auto-submit on tagged release | ⬜ | 130 |
| T5.4 | **MSIX packaging** — alongside MSI; Windows 11 Store submission test flight | ⬜ | 130 |
| T5.5 | **Release pipeline hardening** — SHA256SUMS.txt, smoke test CLI `--list`/`--validate`, auto-generate release notes | ✅ | Sprint 117 |
| T5.6 | **Scoop auto-update on release** — CI computes SHA-256, auto-commits to scoop bucket | ✅ | Sprint 117 |

---

### T6 — Testing Excellence & Quality Gates *(P1 — mostly done)*

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T6.1 | **Branch coverage → 75%** — targeted tests for TweakEngine, RegistrySession, CorporateGuard branches | ✅ | Sprint 121 (75.14%) |
| T6.2 | **Dedicated tests for untested services** — `SmartScanServiceTests`, `LocaleTests`, `ComplianceHistoryTests` | ✅ | Sprint 113 |
| T6.3 | **Virtual registry integration tests** — `RegLoadKey`/`RegUnLoadKey` isolated hive; real Apply↔Remove cycle in CI | ⬜ | 134 |
| T6.4 | **BenchmarkDotNet performance suite** — `RegisterBuiltins`, `Search`, `StatusMap`, `Filter` baselines; CI regression gate | ✅ | Sprint 121 |
| T6.5 | **Property-based tests (FsCheck)** — invariants on all 4 108 tweaks: non-null ID, valid hive paths, no dup ops | ✅ | Sprint 121 |
| T6.6 | **Mutation testing (Stryker.NET)** — 60%+ mutation kill score on Core library | ⬜ | 135 |
| T6.7 | **`AppConfig.Validate()`** — `MaxWorkers` ∈ [1,128], `Theme`/`Locale` ∈ known sets; tests for all edge cases | ✅ | Sprint 113 |

---

### T7 — Plugin Ecosystem & Community *(P2 — Q3–Q4)*

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T7.1 | **Curated online marketplace** — `RajwanYair/RegiLattice-Packs` CDN; 5 starter packs (Privacy, Gaming, Corporate, Minimal, Developer) | ⬜ | 125 |
| T7.2 | **Pack Creator Studio dialog** — wizard: name → select tweaks → metadata → preview/validate → export `.rlpack` | ⬜ | 125 |
| T7.3 | **Pack GPG signing** — optional `.rlpack.sig`; "Verified" badge in `MarketplaceDialog` for signed packs | ⬜ | 131 |
| T7.4 | **Plugin sandboxing** — third-party `ApplyAction` delegates in isolated process via named pipes; 30 s timeout | ⬜ | 132–133 |
| T7.5 | **Community submission workflow** — GitHub Issue template; CI schema validation; auto-merge on approval | ⬜ | 125 |
| T7.6 | **Pack dependency resolution** — `PackManager` resolves `DependsOn` pack chain before install | ⬜ | 125 |

---

### T8 — Tweak Expansion & Intelligence *(P2 — ongoing)*

| ID | Task | Status | Sprint |
|----|------|--------|--------|
| T8.1 | **5 new tweak modules** — WdacCodeIntegrity, BitLockerRemovable, DeviceInstallPolicies, LapsSecurity, NtlmAuthentication (50 tweaks) | ✅ | Sprint 106 (v4.4.0) |
| T8.2 | **Sprint tweak additions** — +50 tweaks per sprint; 4 108 → 5 000+ by Sprint ~137 | 🔄 | Ongoing |
| T8.3 | **Score-change preview on hover** — predicted score delta (Privacy/Perf/Sec) shown in tooltip before applying | ⬜ | 137 |
| T8.4 | **AI-enhanced tweak descriptions** — one-time LLM pass for all 4 108 tweaks; commit improved strings | ⬜ | 136 |
| T8.5 | **Custom user-defined profiles** — "Save Current as Profile"; `regilattice profile create <name>` CLI | ⬜ | 127 |

---

## Sprint Plan

### Q1 — Foundation (Sprints 106–113) ✅ → v4.4.0

| Sprint | Key Deliverables |
|--------|-----------------|
| 106 | 50 security tweaks: WDAC/ASR, BitLocker To Go, Device Install, LAPS, NTLM (T8.1) |
| 107–108 | Accessibility audit pass, `AppConfig.Validate()`, keyboard navigation verification |
| 109–110 | CLI restructure, detail side panel |
| 111–112 | Branch coverage push → 65%+, keyboard shortcuts, multi-select |
| 113 | `SmartScanServiceTests` + `LocaleTests` + new tweak modules → **4 108 tweaks · 121 categories** |

**M1 Deliverable: v4.4.0** — 4 108 tweaks · 121 categories · 1 858 tests · accessibility audit.

---

### Q2 — Enterprise & Trust (Sprints 114–121) ✅ → v4.5.0

| Sprint | Key Deliverables |
|--------|-----------------|
| 114 | Code signing setup + CI integration (T5.1) |
| 115 | Auto-updater: download + install + SHA-256 verification (T5.2) |
| 116–117 | ADMX/ADML export full pipeline · release hardening · Scoop auto-update (T4.1, T5.5, T5.6) |
| 118–119 | Intune OMA-URI export + CLI `--export-intune` (T4.2) |
| 120 | Locale string extraction — 200+ keys across all 6 languages (T2.3) |
| 121 | Branch coverage 75.14% · BenchmarkDotNet suite · FsCheck property tests (T6.1, T6.4, T6.5) |

**M2 Deliverable: v4.5.0** — ADMX/Intune export · auto-updater install · 200+ locale keys · 75.14% branch coverage · benchmarks · ~2 540 tests.

---

### Q3 — Polish & Community (Sprints 122–129) 🔄 Starting Now → v4.6.0

| Sprint | Theme | Exit Criteria |
|--------|-------|---------------|
| **122** | T1.1 — Virtual ListView (part 1: rendering, category headers, scroll, DPI) | < 30 live Controls for 4 108 tweaks; smooth 60 fps scroll |
| **123** | T1.1 — Virtual ListView (part 2: toggle, apply/remove, status, search integration) | Full functional parity with old `FlowLayoutPanel`; all GUI tests pass |
| **124** | T2.1+T2.2 — Accessibility audit (MainForm + top 20 dialogs) + keyboard nav | Narrator reads tweak labels; every feature reachable keyboard-only |
| **125** | T7.1+T7.2+T7.5+T7.6 — Online marketplace (5 packs) + Pack Creator Studio + submission | `MarketplaceDialog` lists packs from GitHub CDN; install/uninstall works |
| **126** | T2.4+T2.5+T2.6 — `.resx` migration + 4 new locales (zh-CN, ko, ar, pt-BR) + high-contrast | 10 locales total; Arabic RTL renders correctly |
| **127** | T3.1+T3.3+T3.4+T8.5 — CLI subcommands + grouped help + tab completion + custom profiles | `regilattice tweak apply <id>` works; PowerShell tab completion for IDs |
| **128** | T4.3+T4.4+T3.5+T3.6 — Scheduled compliance + trend dashboard + TUI + batch file | Daily compliance toast on drift; trend chart in GUI |
| **129** | T4.5+T4.6+T8.2 — Compliance baselines + deployment template + ~50 tweaks | ~4 208 tweaks; CIS/STIG baselines ship |

**M3 Deliverable: v4.6.0** — Virtual scrolling · online marketplace · 10 locales · compliance automation · ~4 208 tweaks.

---

### Q4 — Scale & Harden (Sprints 130–137) ⬜ Planned → v5.0.0

| Sprint | Theme | Exit Criteria |
|--------|-------|---------------|
| **130** | T5.3+T5.4 — Chocolatey package + MSIX packaging | `choco install regilattice` works; MSIX installs cleanly on Win11 |
| **131** | T1.5+T1.6+T7.3 — Animated toggle switch + visual polish + Pack GPG signing | Toggle renders in all 11 themes; signed packs show "Verified" badge |
| **132–133** | T7.4 — Plugin sandboxing (2 sprints) | Third-party `ApplyAction` crashes contained; 30 s timeout enforced |
| **134** | T6.3 — Virtual registry integration tests | Apply→Detect→Remove round-trip on isolated hive passes in CI |
| **135** | T6.6 — Stryker.NET mutation testing | 60%+ mutation kill score on Core library |
| **136** | T8.2+T8.4 — 100 tweaks + AI-enhanced descriptions | ~4 608 tweaks |
| **137** | T8.2+T8.3 — 100 tweaks + score-change preview on hover | ~4 808 tweaks; predicted score delta shown in tooltip |

**M4 Deliverable: v5.0.0** — 5 000+ tweaks · MSIX + Chocolatey + signed · plugin sandboxing · mutation tested · 2 500+ tests.

---

## Success Metrics

| Metric | v4.5.0 Now | M3 (v4.6.0) | M4 (v5.0.0) |
|--------|-----------|-------------|------------|
| Tweaks | **4 108** | ~4 208 | **5 000+** |
| Tests | **~2 540** | 2 350+ | **2 500+** |
| Branch Coverage | **75.14%** | 75%+ | **80%+** |
| Live GUI Controls | ~4 000 (FLP) | **~30** (virtual) | ~30 |
| Code Signed | 🔄 | ✅ | ✅ |
| Accessibility | ⬜ | ✅ Partial | ✅ Full |
| Locales | **6** | **10** | 10 |
| Plugin Marketplace | Local | **Online** | Online + Signed |
| Distribution | GitHub + Scoop + WinGet + MSI | + MSIX + Choco | ✅ All channels |

---

## Technical Investigations

Items requiring proof-of-concept before sprint commitment:

| # | Item | Action |
|---|------|--------|
| 1 | **WinUI 3 migration** — native Mica/ToggleSwitch but is a full GUI rewrite. WinForms polish (T1.5–T1.6) may be sufficient. | Build single-page prototype; decide v5.1 vs v6.0. |
| 2 | **EV code signing vendor** — DigiCert / Sectigo / GlobalSign; may need Azure Key Vault HSM rather than file-based PFX. | Confirm GitHub Secrets vs cloud HSM before Sprint 130 procurement. |
| 3 | **Intune CSP coverage** — not all HKLM paths have OMA-URI equivalents; actual coverage may be below 60%. | Cross-reference `RegOp` paths against Microsoft CSP reference; measure before T4.2 follow-ups. |
| 4 | **Virtual registry hive in CI** — `RegLoadKey` requires elevation; `windows-latest` runner may not support it. | Test elevated CI step; fall back to in-memory registry mock if blocked. |
| 5 | **RTL layout (Arabic)** — WinForms needs `RightToLeftLayout = true`; some custom-drawn controls may break. | Test all 62+ dialogs with RTL before committing to T2.5 Arabic locale. |
| 6 | **Named-pipe sandboxing latency** — per-`SetDword` overhead via pipes may be unacceptable for batch ops. | Benchmark 100 `SetDword` calls via pipe vs direct call; consider `AppDomain` if > 200 ms. |


---
