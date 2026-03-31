# RegiLattice — Development Roadmap

> **Baseline:** v6.0.0 · 9,190 tweaks · 101 categories · 83 modules · 2,931 tests · 64 GUI forms · 34 Core services
> **Last updated:** 2026-03-31
> **Planning horizon:** v6.0.0 → v7.0.0

---

## Current Project State

| Area | v5.0.0 Baseline | v6.0.0 Actual | Change |
|------|----------------|----------------|--------|
| Tweaks | 4,825 | 9,190 | +4,365 (+90%) |
| Categories | 198 | 101 | −97 (consolidated) |
| Modules | 193 | 83 | −110 (consolidated) |
| Tests | 2,661 | 2,931 | +270 |
| GUI Forms | 63 | 64 | +1 |
| Core Services | 34 | 34 | (expanded) |

---

## ✅ Completed Since v5.0.0

The following roadmap themes are **fully or substantially delivered**:

### Theme H — Tweak Intelligence Engine ✅
- `ImpactScore`, `SafetyRating`, `ImpactNote` fields on every `TweakDef` (range-validated by `TweakValidator`)
- `BatchImpactEstimator` service — estimates aggregate impact of a tweak set
- `SmartScanService` + `SmartScanDialog` — context-aware recommendation engine
- `HealthScoreService` — privacy/performance/security score computation
- `ConflictDetector` service — detects tweaks with conflicting registry operations

### Theme D — GUI Excellence ✅ (major items)
- `WhatsNewDialog` — in-app version changelog on first launch after upgrade
- `PrivacyDashboardDialog` — privacy score ring with per-category breakdown
- `TelemetryDashboardDialog` — live view of active telemetry policy tweaks
- `FirstRunWizardDialog` — hardware-aware onboarding with profile suggestion
- `ProfileWizardDialog` — guided profile selection flow
- `ProfileCompareDialog` — side-by-side profile diff view
- `DependencyGraphDialog` — visual dependency graph for tweak chains
- `PreferencesDialog` — per-user app settings
- `ComplianceTrendDialog` — compliance score over time

### Theme E — Enterprise & Compliance ✅ (major items)
- `ComplianceService` + `ComplianceHistory` — baseline-vs-current drift detection
- `ComplianceReportExporter` — HTML/JSON compliance audit reports
- `HtmlReportGenerator` — full-fidelity styled HTML report output
- `GroupPolicyExporter` — ADMX/ADML export of Group Policy tweaks
- `IntuneOmaUriExporter` — Microsoft Intune OMA-URI manifest export
- `PowerShellModuleGenerator` — generate `.psm1` wrappers per tweak
- `ScheduledTweakService` + `ScheduledTweakDialog` — periodic drift checking
- `UserProfileService` — per-user baseline profiles

### Theme F — Distribution & DevOps ✅ (major items)
- `UpdateCheckService` + `UpdateCheckerDialog` — in-app update check UI
- Mutation testing (Stryker.NET) in CI with 55% break threshold
- MSIX packaging (`RegiLattice-vX.Y.Z-win-x64.msix`)
- MSI installer via WiX (`RegiLattice.Installer.msi`)
- Scoop, WinGet, Chocolatey package registry metadata
- Codecov integration with per-run coverage reporting
- CodeQL security analysis on every push to `main`
- PSScriptAnalyzer lint checks in CI

### Miscellaneous ✅
- `NetworkManager`, `PowerPlanManager`, `ServiceManager`, `StartupManager`, `ScheduledTaskManager` services — 5 OS-management backends
- `BatteryHealthDialog`, `BatterySaverDialog`, `BootTimeAnalyzerDialog`, `DiskSpaceDialog`, `DriverUpdateCheckerDialog`, `FirewallRulesDialog`, `HardwareTemperatureDialog`, `HostsFileManagerDialog`, `InstalledAppsDialog`, `MemoryCleanerDialog`, `NetworkAdapterDialog/BandwidthDialog/RepairDialog/ToolsDialog`, `NotificationManagerDialog`, `PackCreatorDialog`, `PortScannerDialog`, `PowerPlanDialog/SchedulerDialog`, `ProxyConfigDialog`, `ServiceManagerDialog`, `ShellExtensionDialog`, `TempFileCleanerDialog`, `UsbPowerDialog`, `WakeOnLanDialog`, `WiFiProfileDialog`, `WindowsUpdateControlDialog`, `ContextMenuManagerDialog`, `AdRemovalWizardDialog`, `AppPermissionsDialog`, `BrowserCacheCleanerDialog`, `DnsOverHttpsDialog`, `DnsSwitcherDialog`, `MacAddressDialog`, `SleepTimerDialog`

---

## Open Themes — Pending Work

The themes below remain open and are ordered by priority for upcoming sprints.

| # | Theme | Priority | Target |
|---|-------|----------|--------|
| **A** | Testing — Remaining Gaps | P0 | v5.98.0–v5.100.0 |
| **B** | CLI Overhaul & Developer Experience | P0 | v6.0.0 |
| **C** | Architecture & Code Health | P1 | v5.98.0–v6.0.0 |
| **D** | GUI — Remaining Items | P1 | v6.0.0 |
| **E** | Enterprise — Remaining Items | P2 | v6.0.0 |
| **F** | Distribution — Remaining Items | P1 | v5.98.0–v6.0.0 |

---

## Theme A — Testing — Remaining Gaps

### Context

Line coverage is strong (~95% line on Core), branch coverage is ~57%, and Stryker mutation testing is live in CI (passes at the 55% kill-score break threshold). Remaining open gaps: virtual registry integration tests (real Apply/Remove/Detect round-trips), FlaUI GUI automation, performance regression baselines, and finalising test isolation for services that write to `%LOCALAPPDATA%`.

### ✅ Completed

- **Stryker.NET mutation testing** — integrated in `ci.yml` as `mutation-testing` job, runs on every `main` push, 55% break threshold enforced, passing as of v5.97.0.

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| A1 | **Virtual registry integration tests** | Load an isolated `.hiv` hive via `RegLoadKey`/`RegUnLoadKey` in a `[Collection]`-isolated xUnit fixture. Run a real `Apply → Detect → Remove` round-trip for one tweak per `TweakKind`. Requires elevation — add elevated `windows-latest` CI step. | L |
| A2 | **FlaUI GUI E2E tests** | Add `RegiLattice.E2E.Tests` project. Cover: launch app, apply/remove a tweak in dry-run, switch theme, open 5 most-used dialogs, close app. Run on CI as optional (non-blocking) gate. Use FlaUI (not deprecated WinAppDriver). | XL |
| A3 | **CLI contract tests** | Add `RegiLattice.CLI.Contract.Tests` project. For each command, assert: exit code, stdout structure (when `--output json` is added in B2), no unhandled exception. Depend on B2 completion. | M |
| A4 | **Performance regression gate** | Add `BenchmarkDotNet` baselines for `RegisterBuiltins`, `Search`, `StatusMap`, `Filter`. Fail CI if any regresses >15% vs stored baseline. Store baseline JSON as a GitHub artifact per release. A `RegiLattice.Benchmarks` project already exists — wire it into CI. | M |
| A5 | **Test isolation audit — eliminate shared file-system state** | Identify all tests writing to `%LOCALAPPDATA%\RegiLattice\` (ratings, favorites, analytics, compliance history). Refactor each to use `Path.GetTempPath()` + `IDisposable` temp-file cleanup or `[Collection("X")]` exclusive isolation. | M |
| A6 | **Raise Stryker kill-score threshold to 70%** | Once A5 is done (stable mutation runs), raise `"break": 55` to `"break": 70` in `stryker-config.json`. Fix surviving mutants in `TweakEngine`, `RegistrySession`, `TweakValidator`, `DependencyResolver`. | M |
| A7 | **FsCheck property tests for TweakDef corpus** | Property tests asserting: no null labels, valid hive prefixes on all `RegOp` paths, `DetectOps` non-empty when `ApplyOps` non-empty, `ImpactScore`/`SafetyRating` in 1–5. Runs against live `RegisterBuiltins()` output. Currently only spot-checked via `TweakEngineBuiltinsTests`. | M |
| A8 | **Coverage delta gate** | Set `codecov/codecov-action` `fail_ci_if_error: true` and `threshold: 0.5%` drop. Enforce branch coverage ≥ 65% (current ~57%) via codecov.yml `patch` threshold. | S |

### Dependencies

- A1 requires elevated CI runner — investigate GitHub Actions permissions before sprint
- A2 requires FlaUI NuGet package; FlaUI 4.x supports .NET 10 — verify before commit
- A3 depends on B2 (structured JSON output must exist before contract-testing it)
- A6 depends on A5 (isolated mutation runs avoid flaky results from shared file state)

### Risks

- A1: `RegLoadKey` needs `SE_RESTORE_NAME` privilege. GitHub Actions `windows-latest` runners are not elevated by default. Research `runs-on` self-hosted or `runas` options before sprint start.
- A2: FlaUI requires a real display or virtual display driver. GitHub Actions runners have display access — verify xvfb-equivalent is not needed on Windows.

### Stretch Goals

- Snapshot round-trip fuzz tests: generate random tweak ID sets, snapshot, restore, diff against original
- `RegistrySession` fuzz tests: malformed paths, oversized value names, non-existent hives — verify no unhandled exceptions escape `Execute()`/`Evaluate()`

---

## Theme B — CLI Overhaul & Developer Experience

### Context

`CliArgs.cs` has a hand-rolled flat `ParseArgs()` returning a flat object. There is no subcommand structure, no structured JSON output, and no stable exit-code contract. Power users and CI pipelines cannot reliably script against the CLI. This is purely additive — all existing `--flags` must be preserved as aliases.

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| B1 | **Subcommand architecture** | Introduce `ICommand` interface. Implement `tweak apply <id>`, `tweak remove <id>`, `tweak status <id>`, `tweak update <id>`, `profile apply <name>`, `profile list`, `snapshot save <file>`, `snapshot restore <file>`, `snapshot diff <a> <b>`. Preserve all `--flag` aliases for backward compatibility. | L |
| B2 | **Structured `--output` mode** | Add `--output json\|csv\|table` (default: `table`) to all listing and status commands. JSON mode emits parseable payloads with stable schema. Define and document exit codes: `0` = success, `1` = partial failure, `2` = invalid args, `3` = registry access denied, `4` = corporate guard blocked. Breaking change for v6.0.0 — document in CHANGELOG. | M |
| B3 | **Grouped contextual `--help`** | Rewrite help output with named sections: `Tweak Operations`, `Profiles`, `Snapshots`, `Export/Import`, `Diagnostics`, `Advanced`. Each subcommand gets its own `--help` page with usage examples. | M |
| B4 | **PowerShell module parity** | Ensure `RegiLattice.psd1` exports cmdlets for every B1 subcommand: `Get-RLTweak`, `Set-RLTweak`, `Get-RLProfile`, `Set-RLProfile`, `Export-RLSnapshot`, `Restore-RLSnapshot`. Add Pester tests. | M |
| B5 | **Interactive REPL mode** | `regilattice shell` launches an interactive session: persistent engine state, command history, tab-completion of tweak IDs, `help` command, `exit`. Pure ANSI/Console — no external readline libraries. | L |
| B6 | **Shell completions** | Generate `Register-ArgumentCompleter` blocks for PowerShell (all subcommands + tweak IDs + category names). Generate `_regilattice` bash completion script for WSL users. CI asserts completions generate without error. | M |
| B7 | **Batch apply/remove mode** | `regilattice batch apply <ids.txt\|snapshot.json>` processes a list of tweak IDs. Emits per-tweak results. `--dry-run` works in batch mode. Enables provisioning scripts without a manifest file. | S |

### Dependencies

- B1 must come first — all other B tasks depend on the subcommand foundation
- B2 must come before A3 (CLI contract tests cannot be written against undefined output schema)
- B4 depends on B1; B5 and B6 depend on B2

### Risks

- Keeping all legacy `--flags` as aliases doubles the surface area to test. Write A3 contract tests against both old and new invocation forms.
- Exit code changes are a breaking change. Ship in a `v6.0.0` commit tagged as `BREAKING CHANGE` in the footer.

### Stretch Goals

- `regilattice doctor --fix` — detects and auto-applies missing recommended tweaks for detected hardware profile
- `--output json` schema published as `cli-schema.json` in the repo root (resolvable by tools like `jq` TypeScript definitions)

---

## Theme C — Architecture & Code Health

### Context

`TweakEngine.RegisterBuiltins()` is now a 665-module registry — manual maintenance is an increasing burden. Services in `Core` (34 total) share no lifecycle contract. Nullable analysis has never been run with `TreatWarningsAsErrors`. A dead-code removal pass has not been done since v4.x.

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| C1 | **Startup performance profiling** | Instrument `RegisterBuiltins()` and engine initialization with `Stopwatch`. Target: cold start < 800 ms on Celeron N4500-equivalent hardware. Identify top 3 bottlenecks and fix. Current baseline unknown — establish first. | M |
| C2 | **Memory footprint audit** | Use `dotnet-counters` during a `Search()` + `StatusMap()` cycle with 9,190 tweaks loaded. Target: < 200 MB working set at idle. Fix top allocators. | M |
| C3 | **Nullable reference types — zero-warning build** | Run `dotnet build /warnaserror:nullable` on all three projects. Fix all suppressed `CS8xxx` warnings. Enforce in CI via `<TreatWarningsAsErrors>nullable</TreatWarningsAsErrors>` in `Directory.Build.props`. | M |
| C4 | **Dead code removal sweep** | Run `dotnet analyzer` with `IDE0051`/`IDE0052` rules. Remove unreachable code, unused private members, stale TODO/FIXME comments. Target: ≥ 5% reduction in `.cs` source line count. | S |
| C5 | **`IReadOnlyList<T>` audit — all 34 services** | Audit every public and internal method on all 34 services for `List<T>` returns. Replace with `IReadOnlyList<T>`. No logic changes — clean-up only. | S |
| C6 | **Source generator for tweak module auto-registration** | Write a `[TweakModule]` Roslyn incremental source generator that auto-scans classes with `IReadOnlyList<TweakDef>` property and emits the `RegisterBuiltins()` body. Eliminates the 665-module manual registry. Spike required before sprint commitment — fallback: T4 template. | XL |
| C7 | **`ITweakService` lifecycle interface** | Define `Init()`, `Flush()`, `Dispose()` contract. Migrate all 34 services to implement it. Register via `Microsoft.Extensions.DependencyInjection` (verify size impact on single-file publish before committing). | L |
| C8 | **API surface XML documentation** | Add `<summary>` doc comments to all `public` and `protected` members in `RegiLattice.Core`. Generate docs via `xmldoc2md` in CI. Fail CI if public API has undocumented members. Update `docs/Api.md`. | M |

### Dependencies

- C3 before C6 (fewer noise warnings during generator development)
- C6 requires C7 (DI lifecycle needed before engine initialization order matters)
- C1 and C2 are independent spikes — run in parallel

### Risks

- C6 (source generator): Roslyn incremental generators are complex to debug. Spike first. If complexity exceeds 2 sprints, accept `/T4` or a `[assembly: TweakModule(typeof(Foo))]` registration approach.
- C7: `Microsoft.Extensions.DependencyInjection` adds ~500 KB to single-file publish. Measure before committing.

### Stretch Goals

- AOT publication support: tag all `ApplyAction`/`DetectAction` delegates as `[DynamicallyAccessedMembers]`-safe; enable `PublishAot=true` for CLI
- Extract `RegiLattice.Core` as a standalone NuGet package on NuGet.org

---

## Theme D — GUI — Remaining Items

### Context

The GUI now has 64 forms spanning system tools, compliance, networking, power management, privacy, and more. `WhatsNewDialog`, `FirstRunWizardDialog`, `ProfileWizardDialog`, `ProfileCompareDialog`, `PrivacyDashboardDialog`, and `TelemetryDashboardDialog` are all shipped. Remaining open items focus on power-user UX, accessibility parity, locale expansion, and long-term platform direction.

### ✅ Completed

- **`WhatsNewDialog`** — in-app changelog shown on first launch after upgrade
- **`FirstRunWizardDialog`** — onboarding with hardware-aware profile suggestion
- **`ProfileWizardDialog`** — guided profile selection
- **`ProfileCompareDialog`** — side-by-side profile snapshot diff
- **`PrivacyDashboardDialog`** and **`TelemetryDashboardDialog`** — score/policy dashboards
- **`DependencyGraphDialog`** — visual tweak dependency graph

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| D1 | **Undo/redo system** | Implement a command-pattern `TweakOperationStack` wrapping `TweakHistory`. `Ctrl+Z` undoes the last apply/remove (calls the inverse operation). `Ctrl+Y` redoes. Max 50 ops. Show "Undo: Disabled Telemetry (2 min ago)" in status bar. Write `TweakOperationStackTests`. | L |
| D2 | **Saved filter presets** | Users save a named filter combination (scope + category + tag + query + status) and recall from a dropdown. Store in `AppConfig`. Enable via right-click "Save as Preset" on the active filter bar. | M |
| D3 | **Tweak detail pane enrichment** | Extend detail pane with: copyable registry path, impact matrix (`ImpactScore` / `SafetyRating` badges), "depended on by N tweaks" count, last-applied timestamp, "See similar tweaks" link filtered by category+tags. | M |
| D4 | **Accessibility completion** | Audit all 64 dialogs for missing `AccessibleName`/`AccessibleDescription`. Complete `TabIndex` chains. Test with Narrator and NVDA. Target: WCAG 2.1 AA on all interactive controls. | L |
| D5 | **`.resx` / `ResourceManager` locale migration** | Replace the hand-rolled `Dictionary<string,Dictionary<string,string>>` in `Locale.cs` with `.resx` files. Add `zh-CN`, `ko`, `pt-BR` locales. Enables satellite assemblies and runtime locale switching without recompile. | L |
| D6 | **High-contrast theme** | Add a `SystemHighContrast` theme using `SystemColors`. Subscribe to `SystemEvents.UserPreferenceChanged` to auto-switch theme when Windows changes dark/light/high-contrast mode at runtime. | M |
| D7 | **Integrated dashboard home panel** | Add a `DashboardPanel` shown when no category is selected: privacy/performance/security score rings from `HealthScoreService`, last 5 operations, top 5 SmartScan recommendations, quick-action buttons. | L |
| D8 | **WinUI 3 migration investigation spike** | 2-sprint investigation: build one `MainForm`-equivalent WinUI 3 page with Mica material, TreeView categories, virtual `ListView`, animated `ToggleSwitch`. Measure: LOC delta, startup time, DPI quality, single-file publish compatibility. Decision gate for v6.1 roadmap direction. | XL |

### Dependencies

- D1 (undo) must integrate with existing `TweakHistory` service — extend, do not duplicate
- D5 (`.resx`) must precede D5b (Arabic RTL): `RightToLeftLayout = true` needs testing on all 64 dialogs
- D7 (dashboard) depends on `HealthScoreService` and `SmartScanService` being production-quality — verify coverage before sprint
- D8 (WinUI spike) is a decision gate — results determine whether v6.1 is WinUI 3 or WinForms-enhanced

### Risks

- D5 RTL Arabic: WinForms `RightToLeftLayout = true` can break custom-drawn controls. All 64 dialogs need spot-testing.
- D8: WinUI 3 requires Windows App SDK packaging — changes single-file publish, installer strategy, and minimum Windows version. Full analysis needed.

### Stretch Goals

- Drag-to-reorder Favorites with persistent order in `Favorites.cs`
- "Tweak of the Day" suggestion panel in the dashboard
- Animated toggle switch for apply/remove on the MainForm tweak list

---

## Theme E — Enterprise & Compliance — Remaining Items

### Context

`ComplianceService`, `ComplianceHistory`, `ComplianceReportExporter`, `HtmlReportGenerator`, `GroupPolicyExporter`, `IntuneOmaUriExporter`, `PowerShellModuleGenerator`, and `ScheduledTweakService` are all shipped. Open items are SCAP/XCCDF export, DSC v3, pre-built CIS/DISA baselines, user-defined baseline profiles, and fleet deployment.

### ✅ Completed

- `ComplianceService` + `ComplianceHistory` — baseline-vs-current drift detection
- `ComplianceReportExporter` + `HtmlReportGenerator` — HTML/JSON compliance audit output
- `GroupPolicyExporter` — ADMX/ADML export for Group Policy tweaks
- `IntuneOmaUriExporter` — Intune OMA-URI manifest export
- `PowerShellModuleGenerator` — `.psm1` wrapper generation per tweak
- `ScheduledTweakService` + `ScheduledTweakDialog` — periodic drift checking
- `ComplianceTrendDialog` — compliance score over time visualization

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| E1 | **CIS / DISA STIG built-in baseline templates** | Ship 4 JSON baselines in the repo: `cis-l1-desktop`, `cis-l1-server`, `disa-stig-win11`, `regilattice-hardened`. Each maps to a list of tweak IDs + expected values. Runnable via `--baseline-compare <name>`. Prerequisite: E2. | M |
| E2 | **User-defined custom baseline profiles** | Allow saving current applied state as a named baseline. `ComplianceService.CompareAgainstBaseline(name)` compares current live state against any saved baseline JSON. CLI: `--baseline-create <name>`, `--baseline-compare <name>`. | M |
| E3 | **SCAP/XCCDF export** | Generate a SCAP-compatible XML document from all `GroupPolicy`-kind tweaks with `NeedsAdmin = true`. Map tweak IDs to XCCDF rule IDs where DISA/CIS equivalents exist. Unmapped tweaks use `[UNMAPPED]` placeholder. Enables import into OpenSCAP, STIG Viewer, Rapid7. Requires research spike for mapping table. | XL |
| E4 | **Audit log export — SIEM-ready** | Extend `TweakHistory` to export as CEF (Common Event Format) or JSON-L. CLI: `--export-audit-log --format cef <file>`. Target: ingestion into Splunk, Microsoft Sentinel, Elastic without custom transform. | M |
| E5 | **Compliance drift webhook alerts** | Extend `ScheduledTweakService` to POST a JSON payload to a configured webhook URL on drift detection. Configurable in `AppConfig`: URL, min severity, cooldown period. No credentials stored — URL only. Deliver to Slack/Teams/generic endpoint. | M |
| E6 | **Multi-machine deployment manifest** | Define `deployment.json` schema: profiles + per-machine overrides + deny-listed tweak IDs. CLI: `regilattice deploy --manifest deployment.json`. Publish a reusable `regilattice/deploy-action` GitHub Actions workflow. Requires B1 first. | L |
| E7 | **PowerShell DSC v3 resource module** | Generate a DSC v3-compatible module (`RegiLatticeDSC.psm1`) wrapping each tweak as a DSC resource. `Test-DscConfiguration` checks drift; `Set-DscConfiguration` applies. Verify DSC v3 compatibility (v2 is in maintenance mode as of PowerShell 7.4). | L |

### Dependencies

- E2 (user baselines) before E1 (CIS/STIG templates use the same compare engine)
- E6 (deployment manifest) requires B1 (subcommand architecture)
- E3 (SCAP) requires a research spike — a subset of tweaks will map to DISA/CIS controls; plan for partial coverage from day one
- E5 (webhooks) requires security review: webhook body must not expose sensitive system state (no hostname, no user data)

### Risks

- E3 SCAP: XCCDF/OVAL mapping to Windows registry keys is not fully standardised. Use DISA STIG Viewer export as the reference mapping source.
- E7 DSC: PowerShell 7 DSC v2 → v3 migration broke many community resources. Validate DSC v3 module loading end-to-end before writing tests.

### Stretch Goals

- Ansible role: wraps CLI apply/remove operations as Ansible tasks
- Terraform `regilattice_tweak` resource (proof of concept)

---

## Theme F — Distribution & DevOps — Remaining Items

### Context

Release pipeline delivers GitHub Releases with `RegiLattice.GUI.exe`, `RegiLattice.exe`, MSI, MSIX, Chocolatey `.nupkg`, and `SHA256SUMS.txt` on every version tag. `UpdateCheckService` handles in-app update detection. `UpdateCheckerDialog` provides UI. Open items: Authenticode code signing, automated release notes, release smoke tests, Dependabot NuGet scanning, and staged rollout support.

### ✅ Completed

- `UpdateCheckService` + `UpdateCheckerDialog` — in-app update check
- Stryker mutation testing in CI (`mutation-testing` job in `ci.yml`)
- MSI (WiX `RegiLattice.Installer.msi`), MSIX, Scoop, WinGet, Chocolatey metadata
- Codecov integration + CodeQL security analysis + PSScriptAnalyzer lint
- SHA256SUMS.txt per release

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| F1 | **Authenticode code signing** | Procure EV certificate (DigiCert or Sectigo). Store PFX as GitHub Actions secret `SIGNING_CERT_PFX` + `SIGNING_CERT_PASSWORD`. Sign `RegiLattice.GUI.exe`, `RegiLattice.exe`, MSI, MSIX in `release.yml` using `signtool`. Verify with `signtool verify /pa /v`. Must precede F4 (Chocolatey community will reject unsigned EXEs). | L |
| F2 | **Automated release notes** | CI step after all tests pass: extract the current tag CHANGELOG.md section, calculate tweak count delta and test count delta vs previous tag, format as the GitHub Release body. No manual editing required. Implement as a PowerShell step in `release.yml`. | M |
| F3 | **Release smoke test matrix** | Post-publish CI step: download the produced MSI from the release draft, install silently on `windows-2022` and `windows-2025` runners, run `regilattice.exe --validate` and `--stats`, assert exit code 0. Fail the release job if smoke tests fail. | M |
| F4 | **Chocolatey community package auto-submit** | Wire `CHOCO_API_KEY` GitHub Actions secret. On tag push: compute SHA256 of the published EXE, update `chocolateyinstall.ps1`, run `choco push`. Verify installation in a clean VM. Requires F1 (signing) first. | M |
| F5 | **Dependabot NuGet + Actions vulnerability scanning** | Enable Dependabot for NuGet packages and GitHub Actions pinned versions. Add `dotnet list package --vulnerable` CI step. Fail fast on high/critical CVEs in production dependencies. | S |
| F6 | **Staged rollout** | Publish releases as `pre-release` for 48 hours. CI monitors new issue creation tagged `bug` via GitHub API. Auto-promote to `latest` if zero new bug issues were filed. Workflow: `staged-release.yml`. Add documentation to CONTRIBUTING.md. Requires F2 (release notes) and F3 (smoke tests) as safety gates. | L |
| F7 | **Auto-updater rollback support** | If the new `RegiLattice.GUI.exe` exits with non-zero code within 10 s of first launch after update, restore the previous EXE from `%LOCALAPPDATA%\RegiLattice\backup\`. CLI flag: `--no-auto-update`. | M |
| F8 | **WinGet manifest auto-PR** | On release: use `gh` CLI + GitHub token to open a PR against `microsoft/winget-pkgs` with updated YAMLs. Currently manual submission is required; this automates the winget submission step. | M |

### Dependencies

- F1 (signing) must precede F4 (Chocolatey)
- F2 (automated release notes) and F3 (smoke tests) must precede F6 (staged rollout)
- F6 requires robust `bug` label discipline — document label taxonomy in CONTRIBUTING.md before automating

### Risks

- F1: EV certificate procurement takes 1–2 weeks. Plan purchase 2 sprints ahead of the signing sprint.
- F6: 48-hour pre-release window requires the GitHub API polling step to have robust error handling — if the polling job fails, the release stays as `pre-release` indefinitely. Add a manual override.
- F8: `winget-pkgs` PRs have a validation pipeline; the automated PR may fail validation for reasons outside this codebase's control.

### Stretch Goals

- SBOM (Software Bill of Materials) generation per release via `dotnet sbom-tool`
- macOS/Linux Homebrew tap for the CLI-only build (requires .NET multi-TFM support)
- Build reproducibility gate: build twice and compare SHA-256 of output binaries

---

## Theme G — Plugin & Community Ecosystem — Remaining Items

### Context

The Tweak Pack plugin system is live and functional: `PackLoader`, `PackManager`, `PackDef`, `PackIndex`, `MarketplaceDialog` (browse / install / update packs from index), and `PackCreatorDialog` (author + export `.tweakpack` files). Outstanding work focuses on security hardening, pack versioning with dependency semantics, and the community submission pipeline.

### ✅ Completed

- `PackDef` record, `PackLoader.LoadFromJson`, `PackLoader.ValidatePackJson`, `PackLoader.ComputeSha256`
- `PackManager.InstallPackAsync`, `UninstallPack`, `InstalledPacks`, `CheckUpdatesAsync`
- `PackIndex.FromJson` / `ToJson`
- `MarketplaceDialog` — browse, install, update packs
- `PackCreatorDialog` — create, validate, export `.tweakpack` files

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| G1 | **Plugin sandbox (process isolation)** | Third-party pack `ApplyAction` delegates execute in-process today with full user privileges. Isolate via a secondary process (`RegiLattice.PluginHost.exe`) communicating over named pipe. Main process sends `RegOp` lists; child returns `TweakResult`. Timeout: 30 s per delegate. Child crash → `TweakResult.Error`; host never crashes. Requires C2 (stable `ITweakService`) for correct initialization. | XL |
| G2 | **Pack signing + trust store** | Extend `PackDef` with `SignatureBase64`. `PackLoader.VerifySignature` checks against a built-in trust store of known public keys (use .NET RSA/Ed25519 APIs, not GPG, for self-contained Windows operation). `MarketplaceDialog` shows "Verified" badge; unsigned packs show a warning before install. | L |
| G3 | **Pack versioning + in-pack dependency resolution** | `PackDef` gains `Version` (SemVer string), `MinEngineVersion`, and `DependsOnPacks[]`. `PackManager` resolves and installs dependency packs first via `DependencyResolver`. Circular pack dependencies blocked with a clear error message. | M |
| G4 | **Community submission CI pipeline** | GitHub Actions workflow `pack-validation.yml`: on PR touching `marketplace/` directory, auto-runs `PackLoader.ValidatePackJson`, checks for ID collisions against the full built-in tweak index (~9,190 IDs), posts a validation summary as a PR comment. Requires G2 (packs must be signed to enter the trusted store). | M |
| G5 | **Pack update notifications** | `UpdateCheckService` already polls GitHub for app updates. Extend to also poll the marketplace index URL for installed pack version updates. Surface stale packs in `MarketplaceDialog` with a badge. Add `--update-packs` CLI option for batch updates. Requires G3 (version semantics). | S |
| G6 | **Plugin authoring SDK documentation** | Write `docs/PluginAuthoring.md`: pack JSON schema reference, all 12 `RegOp` factories, `ApplyAction` delegate guidance, step-by-step "create your first pack", security constraints, and submission instructions. Ship a `template.tweakpack` starter file. | M |

### Dependencies

- G1 (sandbox) must be designed before G2 (signing); they share the trust boundary
- G2 (signing) must precede G4 (community pipeline enforces signing)
- G3 (versioning) must precede G5 (update check requires version comparison)
- G6 can proceed in parallel with G1–G5 but should be frozen before opening community contributions

### Risks

- G1: Named pipe latency adds overhead per delegate invocation. Benchmark 100 `SetDword` calls via pipe; if > 200 ms total, reconsider protocol (e.g., batch serialize operations and send once instead of per-call).
- G4: Community pack quality control cannot be fully automated. Schema + ID validation is automatic; semantic review requires manual approval. Document the review process in CONTRIBUTING.md before the pipeline goes live.
- G6: Docs drift if `PackDef` fields change without corresponding `PluginAuthoring.md` updates. Add a CI step to scan `PluginAuthoring.md` for field name mentions and flag removals.

### Stretch Goals

- Official pack registry website (GitHub Pages) with searchable pack browser
- `regilattice pack search <query>` CLI command
- In-app pack ratings: extend `Ratings.cs` to handle pack-scoped ratings
- Hot-reload of packs without app restart

---

## Theme H — Tweak Intelligence & Content Expansion — Remaining Items

### Context

The intelligence layer is substantially delivered. `ImpactScore` / `SafetyRating` / `ImpactNote` fields are on every `TweakDef`. `BatchImpactEstimator` scores selections. `SmartScanService` produces ranked recommendations. `HealthScoreService` computes composite Privacy + Performance + Security scores. `ConflictDetector` surfaces known conflicts. Tweak content has grown from 4,825 at v5.0.0 to 9,190 at v5.97.0 (+90%) across 637 categories in 665 modules — the ongoing 5×10 policy cadence continues.

### ✅ Completed

- `ImpactScore` (1–5), `SafetyRating` (1–5), `ImpactNote` fields on all `TweakDef` instances
- `BatchImpactEstimator` — score a selection before applying
- `SmartScanService` + `SmartScanDialog` — ranked tweak recommendations
- `HealthScoreService` — composite Privacy / Performance / Security rings
- `ConflictDetector` — flags mutually exclusive tweaks
- Tweak expansion: 4,825 → 9,190 tweaks (+4,365, +90%) across 637 categories, 665 modules
- `ImpactScore` / `SafetyRating` calibration guide in `lessons-learned.instructions.md`

### Pending Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| H1 | **AI-enhanced tweak descriptions** | One-time quality pass: run all 9,190 tweak `Description` fields through a structured LLM prompt (local Ollama or API) targeting ≤40 words, plain English, mentioning real-world impact. Human review pass before merge. Output as a single data-migration commit with no logic changes. | M |
| H2 | **Windows 11 24H2/25H2 tweak audit** | Cross-reference all 9,190 tweaks against KB articles for Windows 11 24H2 and Windows 11 25H2. Identify deprecated registry keys, changed value semantics, and new policy opportunities. Add a `TweakCompatibility` field to `TweakDef`: `All`, `Min22H2`, `Min24H2`, `Deprecated`. Surface deprecated tweaks with a badge in the GUI. | L |
| H3 | **Conflict pair enrichment (50 known pairs)** | Extend `ConflictDetector` with a static catalogue of 50 known Windows-enforced conflicts (e.g., "Disable Defender requires Tamper Protection to be off first"; "Force DoH conflicts with proxy PAC file"). Add conflict reason text to `TweakDef.SideEffects`. Surface in detail pane. | M |
| H4 | **Predictive impact score preview tooltip** | Before applying a tweak, show the delta across Privacy / Performance / Security dimensions as a tooltip overlay on the Apply button (sourcing from `BatchImpactEstimator`). No new service needed — wires existing data to UI. | S |
| H5 | **`MinBuild` population audit** | Audit all 9,190 tweaks for missing or incorrect `MinBuild` values. Cross-reference Windows build release notes for keys that were introduced in specific builds. Priority modules: Recall, Copilot/AI, 24H2-only features. Target: 100% of tweaks with a valid `MinBuild` or explicit `MinBuild = 0` (meaning "all builds"). | M |
| H6 | **Custom user-defined tweaks** | Allow users to place a `custom-tweaks.json` in `%LOCALAPPDATA%\RegiLattice\`. `TweakEngine` loads these after built-ins. `TweakValidator` validates before registering. GUI shows custom tweaks with a distinct badge. CLI `--validate` checks the custom file. Document format in `docs/PluginAuthoring.md` (G6). | L |
| H7 | **Tweak expansion — target 10,000 tweaks** | Continue the 5×10 policy cadence. Priority categories for remaining ~810 tweaks: Windows 11 Recall policy, Microsoft Copilot AI/cloud, Hyper-V fine-grained tuning, Windows App SDK GPO, WDAG Application Guard hardening, and enterprise Identity/certificates policy. Each sprint = 5 modules × 10 tweaks. | Ongoing |

### Dependencies

- H2 (version audit) should inform H5 (`MinBuild` population) and H7 (expansion content)
- H6 (custom tweaks) depends on C2 (stable `ITweakService` lifecycle) for correct load order
- H6 documentation belongs in G6 (`PluginAuthoring.md`) — coordinate with Theme G
- H4 (tooltip) wires `BatchImpactEstimator` to UI — only depends on D-series (detail pane enrichment D6 is a natural pairing)

### Risks

- H1: LLM-generated descriptions risk hallucinating incorrect facts about registry value semantics. Mandatory human review; use a side-by-side diff view to assess all ~9,190 descriptions before committing.
- H2: Windows 11 25H2 registry changes are not fully documented at time of writing. Monitor `windhawk` and `tails.it` community registries for 25H2 discoveries.
- H6: Custom tweaks create an open-ended support surface. Gate with clear warning: "Custom tweaks are provided as-is; RegiLattice is not responsible for instability from user-defined registry ops."

### Stretch Goals

- ML-based tweak recommender: based on `TweakHistory` of what users actually apply, surface "Users like you also applied" suggestions
- Tweak health monitoring: flag tweaks where the detected state drifted back to default (Windows Update reset them) within 24 hours of being applied
- Export `ImpactScore` data as a public JSON API for ecosystem tooling

---

## Dependency Map

```
A7 (test isolation)  ──► A2 (Stryker) [A2 = DONE]
B1 (subcommands)     ──► B2 (structured output) ──► A5 (CLI contracts) ──► B4, B5, B6, B7
C1 (source generator) ◄── C7 (nullable audit) [C1 = design, C7 = prerequisite cleanup]
C7 (nullable audit)  ──► C3 (startup perf) [clean nullability first]
C2 (DI lifecycle)    ──► G1 (plugin sandbox) ──► G2 (pack signing) ──► G4 (community pipeline)
C2 (DI lifecycle)    ──► H6 (custom tweaks)
E2 (user baselines)  ──► E1 (CIS/DISA baselines) [user baselines first, then curated ones]
B1 (subcommands)     ──► E6 (deployment manifest)
F1 (code signing)    ──► F4 (Chocolatey auto-submit)
F2 (auto release notes) + F3 (smoke tests) ──► F6 (staged rollout)
D8 (.resx locales)   ──► D9 (high-contrast theme) [locale infra first]
H2 (Windows build audit) ──► H5 (MinBuild population) + H7 (expansion content)
G3 (pack versioning) ──► G5 (pack update notifications)
G6 (PluginAuthoring docs) <──► H6 (custom tweaks — same doc)
```

---

## Sprint Plan

### Milestone 1 — v6.0.x: Code Health & DevOps (current)

**Status: In progress (v6.0.0 released 2026-03-31)**

| Version | Theme | Key Deliverables | Status |
|---------|-------|-----------------|--------|
| **v6.0.0** | — | Module consolidation (98→83 modules), doc/metadata cleanup | ✅ Released |
| **v6.0.1** | C, F | C4 dead code sweep · F2 automated release notes · F5 `--vulnerable` CI check | ✅ Released |
| **v6.0.2** | A, C | A5 test isolation audit · C3 startup perf baseline · C5 `IReadOnlyList` audit | ✅ Released |
| **v6.0.3** | C | C3 nullable enforcement (`TreatWarningsAsErrors`) · C4 dead code audit confirmed clean | ✅ Released |

**M1 gate:** Dead code eliminated · CI scans for vulnerable packages · release notes auto-populated from CHANGELOG · test isolation clean · startup baseline measured. **✅ M1 complete as of v6.0.3.**

---

### Milestone 2 — v6.1.0: CLI Overhaul + Distribution

**Target: ~8 sprints post-M1**

| Sprint | Theme | Key Deliverables | Exit Criteria |
|--------|-------|-----------------|---------------|
| **v6.0.4** | B | B1 subcommand architecture · B3 grouped help · B5 stable exit codes | `regilattice tweak apply <id>` works; old `--flags` preserved; exit codes 0/1/2/3 documented | ✅ Released |
| **+2** | B, A | B2 structured output (JSON/CSV) · A3 CLI contract tests | All commands support `--output json`; 100% branch coverage on new routes |
| **+3** | B | B4 PowerShell module parity · B6 shell completions · B7 batch mode | `Import-Module RegiLattice` works; tab completion in pwsh |
| **+4** | F | F1 Authenticode code signing (EV cert + CI) | `signtool verify /pa` passes on GUI EXE, CLI EXE, MSI in CI |
| **+5** | F | F3 release smoke test matrix · F4 Chocolatey auto-submit | `choco install regilattice` works from community |
| **+6** | E | E2 user baselines · E1 CIS/DISA L1 Desktop baseline template | `--baseline-compare cis-l1-desktop` works |
| **+7** | D | D1 undo/redo · D2 saved filter presets | `Ctrl+Z` undoes the last apply or remove |
| **+8** | A, H | A4 BenchmarkDotNet perf gate · H3 conflict enrichment (50 pairs) | Perf gate in CI; 50 conflict pairs documented |

**M2 gate (v6.1.0):** CLI subcommands complete · `--output json` on all commands · Authenticode signed · CIS baseline ships · undo/redo · release notes automated. Tag `v6.1.0` (MINOR — new CLI subcommands additive).

---

### Milestone 3 — v7.0.0: Architecture Renaissance + Platform

**Target: 12–18 months post-v6.1.0**

| Focus | Theme | Key Deliverables |
|-------|-------|-----------------|
| Source generator | C | C1 — auto-register all 665 modules via source generator; `RegisterBuiltins()` removed |
| Service lifecycle | C | C2 — formal `ITweakService` interface; DI-friendly constructor injection |
| GUI UX | D | D2 (onboarding rework) · D3 (integrated health dashboard panel) · D5 (accessibility completion for all 64 dialogs) |
| Plugin security | G | G1 (plugin sandbox, process isolation) · G2 (pack signing + trust store) |
| SCAP export | E | E1 SCAP/XCCDF export importable in STIG Viewer |
| Custom tweaks | H | H6 (`custom-tweaks.json`) · H1 (AI descriptions quality pass) · H2 (24H2/25H2 audit) |
| WinUI 3 | D | D10 spike — 2-sprint fixed budget decision gate (migrate or defer to v7.5) |
| E2E tests | A | A4 FlaUI smoke scenarios (5 key workflows wired in CI) |

**M3 gate (v7.0.0):** Source generator live · plugin sandbox ships · all 64 dialogs WCAG 2.1 AA compliant · SCAP export · custom tweaks · WinUI 3 decision committed.

---

## Risk Register

| # | Risk | Likelihood | Impact | Mitigation |
|---|------|-----------|--------|------------|
| R1 | Source generator (C1) complexity exceeds estimate | Medium | High | Spike in dedicated sprint before committing; fallback = retain `RegisterBuiltins()` reflection scan |
| R2 | EV cert procurement delay (F1) | **High** | Medium | Order cert **now** — 1–2 week lead time. CI uses self-signed cert until real cert arrives. Do not block F3/F4 on F1. |
| R3 | WinAppDriver unmaintained / FlaUI instability on GitHub CI | Medium | Medium | Use FlaUI only; scope A4 to 5 smoke scenarios; mark as non-blocking gate |
| R4 | `RegLoadKey` needs elevation on GitHub runner (A1 virtual registry tests) | High | Medium | Spike in first sprint; if blocked, use RegistrySession DryRun + in-memory comparisons instead |
| R5 | WinUI 3 migration scope underestimated (D10) | Medium | High | 2-sprint time-boxed spike before decision; if full migration > 6 months estimate, defer to v7.5 |
| R6 | LLM hallucinations in AI description pass (H1) | High | Medium | 100% human review before merge; run diff against existing descriptions side-by-side |
| R7 | Chocolatey community pipeline (F4) rejects unsigned EXE | High | High | F1 (signing) must complete before F4 is attempted — hard dependency |
| R8 | Named pipe latency in G1 sandbox exceeds usability threshold | Medium | High | Benchmark 100 `SetDword` calls via pipe before design commit; if > 200 ms total, adopt batch-serialize protocol |

---

## Success Metrics

| Metric | v6.0.0 (now) | v6.1.0 | v7.0.0 |
|--------|-------------|--------|--------|
| Tweaks | 9,190 | 9,500+ | 10,000+ |
| Categories | 101 | 110+ | 120+ |
| Modules | 83 | 85+ | 90+ |
| Tests | 2,931 | 3,100+ | 3,500+ |
| Branch coverage | ~57% | ≥ 65% | **≥ 75%** |
| Mutation kill score | ~55% threshold | 60%+ | **≥ 70%** |
| CLI subcommand structure | ❌ | ✅ | ✅ |
| JSON output on all CLI commands | ❌ | ✅ | ✅ |
| Stable exit codes | ❌ | ✅ | ✅ |
| Authenticode signed | ❌ | ✅ | ✅ |
| Locales (`.resx`) | 6 (Locale.cs) | 6 | **10** |
| Source generator | ❌ | ❌ | ✅ |
| Startup time | Unmeasured | Baseline captured | **< 800 ms** |
| Working set (idle) | Unmeasured | Baseline captured | **< 150 MB** |
| Undo/redo | ❌ | ✅ | ✅ |
| CIS/DISA baselines | ❌ | ✅ | ✅ |
| SCAP export | ❌ | ❌ | ✅ |
| Plugin sandbox | ❌ | ❌ | ✅ |
| E2E GUI tests | ❌ | ❌ | ✅ (5 scenarios) |

---

## Next Steps — Immediate Priority (v6.0.x)

> **These are the next 3–5 actions.** Ordered by dependency chain and effort size. Each item below produces a committable, releasable delta.

### Step 1 — Start EV Certificate Procurement (F1) — Do This Today

**Status**: Not started. **Lead time**: 1–2 weeks.

Authenticode code signing (F1) is the single highest-impact unblocked item that cannot be compressed by writing code. Every day of delay pushes the Chocolatey community package (F4) and signed-installer distribution further out. The action is simple:

1. Visit DigiCert or Sectigo, select "Code Signing Certificate (EV)" — ~$400/year
2. Begin the identity verification process (requires government-issued ID + business address)
3. While waiting, prepare the signing CI step in `release.yml` (`signtool sign /fd SHA256 /tr ...`)
4. Store PFX + password as GitHub Actions secrets `SIGNING_CERT_PFX_B64` + `SIGNING_CERT_PASSWORD`

No other work is blocked by this procurement; the EV cert work can proceed in parallel with all code sprints below.

---

### Step 2 — v6.0.2: Test & Performance Baseline Sprint

**Target**: 1–2 sprints. **Can start immediately** (no prerequisites).

| Priority | Task | Why Now |
|----------|------|---------|
| **A7** | Test isolation audit — eliminate shared `%LOCALAPPDATA%` file access in tests | Prevents non-deterministic CI failures as test count grows; cheap to fix now, expensive to debug later |
| **C3** | Startup profiling baseline — instrument `RegisterBuiltins()` + full GUI cold-start | Establishes the baseline before architectural changes in C1/C2 complicate measurement |
| **C5** | `IReadOnlyList` audit — find remaining `List<T>` in public APIs | Clean up API surface before CLI overhaul |

**Exit criteria**: All tests pass; no shared file paths in test constructors without temp-file isolation; startup time baseline committed to `.tmp/perf-baseline.txt`.

---

### Step 3 — v6.0.3: Nullable Audit + Remaining Dead Code

**Target**: 1 sprint. **Can start immediately** (no prerequisites).

| Priority | Task | Why Now |
|----------|------|---------|
| **C3** | Nullable audit — treat nullable warnings as errors in Core | Prevents null-reference bugs as codebase grows |
| **C4** | Remaining dead code sweep — second pass with Stryker guidance | Reduce noise |

**Exit criteria**: Zero nullable warnings in Core build; Stryker report shows no surviving easy mutants.

---

### Step 4 — v6.1.0: CLI Subcommand Architecture (B1)

**Target**: 2–3 sprints. **Starts after v6.0.x code health is clean.**

**What changes**: `Program.cs` currently parses `string[] args` with a flat `if/else` chain. Refactor to an `ICommand` interface + dispatcher pattern.

```
Before:  regilattice --apply priv-disable-telemetry
After:   regilattice tweak apply priv-disable-telemetry
         regilattice search telemetry
         regilattice profile apply privacy
         (old --flags still work via compat shim)
```

**Key deliverables**:
- `ICommand` interface with `Name`, `Usage`, `Execute(CliArgs, TweakEngine)`
- `CommandDispatcher` in `Program.cs`
- All 25+ existing commands ported to subcommand classes
- Old `--flag` style still dispatches correctly via alias mapping
- `B3`: Grouped `--help` output (by category: Tweak, Search, Profile, Snapshot, Export, Admin)
- `B2`: `--output json|csv|table` on all commands
- `B5`: Stable exit codes (0 = success, 1 = partial, 2 = error, 3 = admin required)

**Exit criteria**: `regilattice tweak apply <id>` works; `regilattice --apply <id>` still works; all CLI tests green; `--output json` on all commands.

**Tag**: `v6.1.0` (MINOR — additive CLI subcommands)

---

### Step 5 — v6.0.0: CLI Complete + Key Distribution Wins

After Steps 2–4 establish the subcommand foundation, the v6.0.0 milestone delivers on the most user-visible items:

| Item | Scope | Description |
|------|-------|-------------|
| **B4** | CLI | PowerShell module parity — `Import-Module RegiLattice` exposes all subcommands as cmdlets |
| **B6** | CLI | Tab completions (PowerShell + pwsh auto-complete for tweak IDs and profile names) |
| **B7** | CLI | Batch apply from file: `regilattice apply --from-file tweaks.txt` |
| **F1** | Distribution | Authenticode code signing live (EV cert from Step 1 deployed) |
| **F2** | CI | Automated release notes from CHANGELOG.md on every tag push |
| **F3** | CI | Release smoke test matrix: MSI + `--validate` on Windows 2022 + 2025 runners |
| **E2** | Enterprise | User-defined compliance baselines: `--baseline-compare my-baseline.json` |
| **E1** | Enterprise | CIS Windows 11 Level 1 Desktop baseline template ships with release |
| **D1** | GUI | Undo/redo: `Ctrl+Z` / `Ctrl+Y` for apply and remove operations |
| **C7** | Core | Nullable audit — zero nullable warnings in `RegiLattice.Core` |
| **A8** | Tests | Coverage delta gate — PR fails if branch coverage drops > 2% |

**MAJOR version justification for v6.0.0**: Breaking CLI interface (old `--apply <id>` and `--remove <id>` positional commands are removed after 2 minor versions of deprecation warnings in v5.99.x and v5.100.x). All other v5.x behavior is preserved. Document breaking changes in CHANGELOG.md under `### Breaking Changes`.

---

### Tweak Content Cadence (Ongoing — Every Version Bump)

The 5×10 policy cadence continues independently of the feature sprints above. Every MINOR version bump = 5 new modules × 10 tweaks = +50 tweaks. Current target: **10,000 tweaks** by v5.107.0.

**Running the pre-sprint gap analysis before each batch (mandatory)**:

```powershell
# Phase 1 — Registry key path not already claimed
Select-String -Pattern 'TargetKeyPath' -Path "src/RegiLattice.Core/Tweaks/*.cs"

# Phase 2 — Slug prefix not already claimed
Select-String -Pattern '"slug-' -Path "src/RegiLattice.Core/Tweaks/*.cs"

# Phase 3 — PATH\ValueName semantic conflict (most critical — prevents duplicate ops)
Select-String -Pattern '"ValueNameToCheck"' -Path "src/RegiLattice.Core/Tweaks/*.cs"
```

---

*Last updated: 2026-05-29 · v5.97.0 · 9,190 tweaks · 637 categories · 665 modules · 2,941 tests*
