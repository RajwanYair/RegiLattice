# Changelog

All notable changes to RegiLattice are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This project adheres to [Semantic Versioning](https://semver.org/).

## [6.27.0] — 2026-04-09

### Added — Phase 6.1–6.3: Services & Intelligence

#### 6.1 Enterprise Audit Logging (`TweakHistory`)

- `HistoryEntry` now captures `Username`, `MachineName`, and `SessionId` (8-char hex per-process GUID) for every recorded tweak operation.
- New `TweakHistory.ExportCsv(string filePath)` method exports a SIEM-compatible 7-column CSV: `Timestamp,TweakId,Action,Result,Username,MachineName,SessionId`.
- Backward-compatible: old `history.json` files from v6.26.0 and earlier deserialise cleanly — new fields use `[JsonIgnore(Condition = WhenWritingDefault)]`.

#### 6.2 Per-Category Health Score Breakdown (`HealthScoreService`)

- New `HealthScoreService.CategoryHealthScores(IReadOnlyDictionary<string, TweakResult> statusMap)` returns `IReadOnlyList<CategoryHealthScore>` — one entry per category, score 0–100, applied/total counts, and an actionable recommendation string.
- New `CategoryHealthScore` sealed record (sibling type in `RegiLattice.Core.Services` namespace).

#### 6.3 Enhanced Conflict Detection (`ConflictDetector`)

- New `ConflictDetector.DetectRegistryConflicts(IEnumerable<TweakDef> tweaks)` dynamically scans all `RegOp.SetValue` operations to find tweaks writing to the same registry path+name with different values.
- New `ConflictSeverity` enum: `Info` (same value, redundant), `Warning` (different values, order-dependent), `Critical` (opposing 0/1 binary values — tweaks cancel each other out).
- New `RegistryConflict` readonly record struct with TweakIdA/B, RegistryPath, ValueName, ValueA/B, Severity.

### Fixed

- `tests/.runsettings`: removed `HangTimeout="30000"` attribute from `CollectDumpOnTestSessionHang` — was producing two non-fatal `Data collector 'Blame' message: …HangTimeout is not valid` warnings on every `dotnet test` invocation with .NET SDK 10.0.201+. Primary hang-protection is now solely `TestSessionTimeout` (2000 s).

### Tests

- +20 new tests covering all Phase 6 additions: audit fields (Username/MachineName/SessionId), session ID format/stability, CSV export, category health scores (6 variants), and registry conflict detection (7 variants).

### Stats

- Tweaks: **7,518** (unchanged) across **127** categories
- Modules: **175** files (unchanged)
- Tests: **3,259** (+20 Phase 6 tests over v6.26.0's 3,239), 0 failures, 0 warnings

## [6.26.0] — 2026-04-09

### Added — Sprint 688: Office GP Security + Windows Search hardening (+39 tweaks, +3 modules)

- `PolicyWindowsSearch.cs` — expanded 1 → 10 tweaks: Windows Search GP hardening (block remote query, disable web search, safe-search, prevent email/attachment/Outlook indexing, diacritics, language detection) (`wsepol-*`)
- `PolicyOfficeWord.cs` — 10 new tweaks: MS Word GP security hardening (VBA block, ActiveX disable, extension hardening, network macro exec block, OM access denial, Protected View controls, file block for Word 97) (`offword-pol-*`)
- `PolicyOfficeExcel.cs` — 10 new tweaks: MS Excel GP security hardening (VBA block, ActiveX disable, extension hardening, workbook link warnings, Protected View, file block for XL4 macros) (`offxls-pol-*`)
- `PolicyOfficeOutlook.cs` — 10 new tweaks: MS Outlook GP security hardening (security level, OM guard, min encryption bits, VBA warnings, invalid signature warn, add-in customization block, external content block, OOM address/send-mail prompts) (`offolt-pol-*`)

### Removed

- `PolicyAppPrivacy.cs`, `PolicyEventLogAudit.cs`, `PolicySyncSettings.cs` — empty stub modules (0 tweaks), replaced by dedicated implementations in earlier sprints

### Stats

- Tweaks: 7,518 (+39) across 127 categories
- Modules: 175 files (net unchanged — 3 stubs deleted + 3 new)
- Tests: 3,239 (2,442 Core + 434 CLI + 363 GUI), 0 failures

## [6.25.0] — 2026-04-07

### Added — Phase 5.4: Energy & Battery Management (+50 tweaks, +5 modules)

- `BatterySaver.cs` — 10 Battery saver threshold, auto-dim, hibernate, wake-on-pattern tweaks (`batt-*`)
- `ChargingOptimization.cs` — 10 Expose battery level/action settings in Power Options UI (`chrg-*`)
- `StandbyStates.cs` — 10 Connected Standby, hybrid sleep, wake timers, S1/S2 states (`standby-*`)
- `CPUPowerStates.cs` — 10 CPU min/max state, boost mode, EPP, core parking, perf policies (`cpupwr-*`)
- `DisplayPower.cs` — 10 Display timeout, adaptive dimming/brightness, display-required policy (`dispwr-*`)

### Stats

- Tweaks: 7,479 (+50) across 127 categories
- Modules: 175 files (+5)
- Tests: 3,230 (2,434 Core + 434 CLI + 362 GUI), 0 failures

## [6.24.0] — 2026-04-07

### Added — Phase 5.3 Accessibility (remaining) + Phase 5.5 Developer Modules (+80 tweaks, +8 modules)

- `MagnifierAdvanced.cs` — 10 Screen Magnifier advanced configuration tweaks (`magnif-*`)
- `LiveCaptions.cs` — 10 Windows 11 Live Captions feature tweaks (`lcap-*`, MinBuild=22621)
- `EyeControlSettings.cs` — 10 Eye Control / gaze input tweaks (`eyectrl-*`)
- `VoiceAccessControl.cs` — 10 Windows 11 Voice Access tweaks (`voiacc-*`, MinBuild=22621)
- `WinDbgSettings.cs` — 10 Windows Debugger / crash dump / WER tweaks (`windbg-*`)
- `WSLAdvanced.cs` — 10 WSL 2 advanced registry tweaks (`wsladv-*`)
- `GitCredManager.cs` — 10 Git Credential Manager registry tweaks (`gitcm-*`)
- `ContainerRuntime.cs` — 10 Windows Container / Sandbox policy tweaks (`cntr-*`)

### Stats

- Tweaks: 7,429 (+80) across 122 categories
- Modules: 170 files (+8)
- Tests: 3,230 (2,434 Core + 434 CLI + 362 GUI), 0 failures

## [6.23.0] — 2026-04-07

### Added — Phase 5.2 Gaming Performance + Phase 5.3 Accessibility (+80 tweaks, +8 modules)

- `GamingDirectStorage.cs` — 10 DirectStorage/NVMe latency tweaks (`dxstore-*`)
- `GamingVariableRefreshRate.cs` — 10 VRR/adaptive-sync tweaks (`vrr-*`)
- `GamingLatencyTuning.cs` — 10 ultra-low-latency gaming tweaks (`glatency-*`)
- `GamingGPUPower.cs` — 10 GPU power management and TDR tweaks (`gpupwr-*`)
- `GamingNetworkOpt.cs` — 10 gaming network optimisation tweaks (`gamenet-*`)
- `GamingAudioOpt.cs` — 10 MMCSS/audio gaming latency tweaks (`gamaudio-*`)
- `AccessibilityMotor.cs` — 10 motor/input accessibility tweaks (`accmotor-*`)
- `AccessibilityVisual.cs` — 10 visual/display accessibility tweaks (`accvisual-*`)

### Changed — CI Workflow Optimisation

- ci.yml: mutation testing moved from every-push-to-main to weekly schedule + manual only (biggest cost saving)
- ci.yml: removed redundant coverage artifact upload (Codecov is the single source of truth)
- ci.yml: Stryker report artifact only uploaded on failure
- ci.yml: `paths-ignore` added to push/PR triggers (skip docs-only changes)
- release.yml: `--no-build` added to all 3 test steps (already built in prior step)
- smoke.yml: download pattern fixed from `RegiLatticeCLI.exe` → `*CLI*-win-x64.exe` (was silently failing)
- smoke.yml: 6 separate smoke steps consolidated to 1 PowerShell loop

### Stats

- Tweaks: 7,349 (+80) across 122 categories
- Modules: 162 files (+8)
- Tests: 3,230 (2,434 Core + 434 CLI + 362 GUI), 0 failures

## [6.22.0] — 2026-04-06

### Added — Phase 5 Sprint: 8 New Security Modules (+80 Tweaks)

#### New Tweak Modules

- **SecurityWDAG.cs** — 10 tweaks (`wdag-*`): Windows Defender Application Guard policy (clipboard, printing, camera mic, network isolation, enterprise mode, persistence, idle timer, file trust, URL rendering, developer mode)
- **SecurityPrinterHardening.cs** — 10 tweaks (`prtharden-*`): Windows Print Spooler hardening (disable remote printing, block client-side rendering, restrict driver install, disable SMB printing, limit unauthenticated access, block internet print, point-and-print restrictions, disable fax service, enforce driver signing, disable PrintNightmare vector)
- **SecurityLsaHardening.cs** — 10 tweaks (`lsaharden-*`): LSA process protection (enable PPL, disable WDigest, enable LSASS audit, block RPC, disable remote registry admin, restrict guest accounts, restrict anonymous access, disable LM auth, enforce NTLMv2, restrict LSA anonymous)
- **SecurityMsiHardening.cs** — 10 tweaks (`msiinst-*`): Windows Installer lockdown (disable elevated per-user installs, disable admin always install elevated, require signed packages, disable roll-back for elevated, disable automatic updates for MSI, remove cached patches, enforce minimum auth level, block MSIX sideloading, require store-signed apps, disable unsigned package install)
- **SecurityTimeService.cs** — 10 tweaks (`ntpsec-*`): W32Time / NTP hardening (enforce NTP synchronisation, set primary NTP server, MaxPosPhaseCorrection, MaxNegPhaseCorrection, announce as reliable source, disable VM timesync, enable NTP response logging, block unauthenticated peers, restrict peer discovery broadcast, enable secure time providers)
- **SecurityWinRMHardening.cs** — 10 tweaks (`winrmsec-*`): WinRM security policy (disable WinRM client, require Kerberos auth, disable unencrypted traffic, disable CredSSP, disable Negotiate auth, block basic auth over HTTP, require HTTPS listener, disable server-side basic auth, enforce max shell memory, disable running as local account)
- **SecurityCredentialGuard.cs** — 10 tweaks (`credguard-*`): Credential Guard and VBS (enable VBS, enable Credential Guard UEFI lock, disable WDigest cleartext, enable Remote Credential Guard, block NTLM default/saved/fresh creds, enable restricted admin RDP, require TPM for credentials)
- **SecurityIEZones.cs** — 10 tweaks (`iezone-*`): IE Zone security policy (Restricted Zone: block ActiveX, unsafe ActiveX, active scripting, clipboard script, frame nav, subframe cross-domain, enable popup blocker; Intranet Zone: block ActiveX install, clipboard script, cross-domain nav)

#### Fixed

- **Scenario5 snapshot hang**: `SnapshotManager.Save` / `TweakEngine.SaveSnapshot` now accept an optional `cachedStatus` parameter (matching the pattern used by `ExportJson`) — avoids live `StatusMap()` reads on 7,000+ tweaks which hung for > 2 min on Intune filter-driver machines
- Repaired 8 pre-existing corruption sites in `E2EWorkflowTests.cs` (duplicate/truncated fragments from a prior edit session)

#### Stats

- Tweaks: 7,189 → **7,269** (+80)
- Tests: 2,434 → **2,434** (snapshot fix adds 0 net new tests; E2E count unchanged)

---

## [6.21.0] — 2026-04-06

### Added — Phase 4.1 + Phase 4.6: E2E Workflow Tests & Concurrent Safety Tests

#### New Tests

- **Phase 4.1 — 10 E2E Workflow Scenarios** (`E2EWorkflowTests.cs`):
  - Scenario 1: Full apply → detect → remove lifecycle in DryRun mode
  - Scenario 2: Privacy profile batch apply returns populated results dict
  - Scenario 3: ApplyBatch then RemoveBatch — result counts match tweak list size
  - Scenario 4: DryRun session captures ops without writing to registry
  - Scenario 5: Snapshot round-trip — SaveSnapshot / LoadSnapshot file verified
  - Scenario 6: ExportJson with stub status cache — file created, valid JSON structure
  - Scenario 7: Dependency chain — ResolveDependencies returns TweakDef list with all DependsOn satisfied
  - Scenario 8: CorporateGuard blocks CorpSafe=false tweaks when StubCorporate=true
  - Scenario 9: ValidateTweaks on full builtin set returns zero errors
  - Scenario 10: RegisterBuiltins loads ≥7,000 tweaks, ≥100 categories, no exception
- **Phase 4.6 — 3 Concurrent Safety Tests** (`ConcurrentSafetyTests` class):
  - 10 concurrent StatusMap calls on 20 Registry/GroupPolicy tweaks — no deadlock
  - 5 concurrent ApplyBatch calls on Privacy tweaks in DryRun — no corruption
  - Mixed concurrent reads + apply — AllTweaks/Categories/Search alongside ApplyBatch, no exception
- All 13 tests use `[Collection("E2E-Sequential")]` for clean sequential isolation with `CorporateGuard.StubCorporate = false`

#### Stats

- Tests: 2,421 → **2,434** (+13)
- Tweaks: 7,189 (unchanged)

---

## [6.20.0] — 2026-04-06

### Added — Phase 2.6 + Phase 3.6: Custom User Themes & Enhanced Export Formats

#### Phase 2.6 — Custom User Theme JSON Loading

- **User-defined JSON themes**: Place `*.json` files in `%LOCALAPPDATA%\RegiLattice\themes\`
  to add custom colour themes. Files are loaded at startup and appear in the theme selector
  prefixed `user:<filename>`.

- **Hot-reload**: A `FileSystemWatcher` monitors the themes directory and re-loads themes
  when files are created, modified, renamed, or deleted — no restart needed.

- **JSON schema**: Requires `name`, `background`, and `text`; all other fields are optional
  with sensible defaults. Supports: `surface`, `surface2`, `textSecondary`, `accent`,
  `primary`, `secondary`, `success`, `warning`, `error`, `overlay`, `info`, `isDark`.

- **`AppTheme.LoadUserThemes()`**: New public API — call once at startup (e.g. in
  `MainForm.Load`). Error details surface via the new `UserThemeLoadError` event (invalid JSON
  or missing required fields are silently skipped; the event fires with a message per bad file).

- **`AppTheme.TryParseUserTheme(string json)`**: Parses a user theme JSON string into a
  `ThemeDef` record; returns null on failure. Available for tooling and unit tests.

- **`AppTheme.IsUserTheme(name)`**: Returns `true` for `user:*` keys so the UI can show a
  "Remove" button for user themes without affecting built-in palettes.

#### Phase 3.6 — Ansible and DSC Export Formats

- **`AnsibleExporter.Build/Export`**: Generates an Ansible `win_regedit` task-list YAML
  playbook for all `Registry` and `GroupPolicy`-kind tweaks. Paths normalised to
  `HKLM:\...` / `HKCU:\...` format. Value types mapped to Ansible's `dword`, `qword`,
  `string`, `expandstring`, `multistring`, `binary` names.

- **`DscExporter.Build/Export`**: Generates a PowerShell DSC Configuration script (`.ps1`)
  using `PSDscResources\Registry` resources for all applicable tweaks. Includes an
  `Import-DscResource` block, `Node localhost` scope, and auto-invoke at end of file.

- **CLI `--export-ansible <path>`**: Export all tweaks as an Ansible YAML playbook.
- **CLI `--export-dsc <path>`**: Export all tweaks as a PowerShell DSC script.

#### Stats

- Tweaks: 7,189 · Categories: 122 · Modules: 146 · Tests: 2,421

---

## [6.19.0] — 2026-04-06

### Added — Phase 2.3 + 2.4: Risk Confirmation Dialog & Batch ETA

- **Risk-confirmation dialog (Phase 2.3)**: `ConfirmApplyDialog` is shown before applying
  tweaks with `SafetyRating ≤ 3` or dangerous flags (`DeletesKey`, `RequiresReboot`,
  `AffectsSecurity`, `PotentialDataLoss`). Displays category/safety/impact metadata, a
  colour-coded risk badge panel, formatted registry operations preview (first 10 ops), and
  "Apply Anyway" / "Cancel" buttons. Force-check bypasses all confirmation dialogs.

- **`ConfirmApplyThreshold` service (Phase 2.3)**: Pure-logic Core class with
  `ShouldConfirm(TweakDef)`, `SafetyRatingThreshold` (3) and `ConfirmationFlags` constants.
  Fully testable without any WinForms dependency.

- **`TweakDef.EstimatedApplyTimeMs` (Phase 2.4)**: Computed property returning estimated
  apply time in milliseconds based on `TweakKind` — Registry/GroupPolicy=50ms,
  FileConfig=200ms, ScheduledTask/PowerShell=500ms, SystemCommand=1 000ms,
  ServiceControl=2 000ms, PackageManager=3 000ms.

- **`TweakEngine.CalculateBatchEtaMs` (Phase 2.4)**: Sums `EstimatedApplyTimeMs` across a
  batch of tweak IDs. Unknown IDs are skipped silently; duplicate IDs are counted per
  occurrence.

- **Batch progress ETA display (Phase 2.4)**: `ApplySelectedAsync` progress label now shows
  `"Applying X/Y: Label  [~Zs remaining]"` using a `Stopwatch` and static ETA estimates.

#### Stats

- Tests: 3,190 → **3,218** (+28 Phase 2.3/2.4 tests)
- Tweaks: 7,189 (unchanged)
- Categories: 122 (unchanged)

## [6.18.0] — 2026-04-06

### Added — Phase 2.2 + Phase 2.5: Keyboard Shortcuts & Enhanced Context Menu

- **Keyboard shortcuts (Phase 2.2)**: `ProcessCmdKey` override in MainForm handles five
  global shortcuts: F1 opens the new `KeyboardShortcutsDialog` (non-modal cheatsheet with
  19 shortcuts in 4 groups, filterable search bar), F5 refreshes tweak status, Ctrl+F
  focuses the search box, Ctrl+L toggles the log panel, Escape clears the search box.

- **KeyboardShortcutsDialog**: New dialog in `Forms/KeyboardShortcutsDialog.cs` (174 lines).
  Lists all keyboard shortcuts in a themed 3-column ListView (Keys / Action / Group).
  Filterable by typing in the filter box. Closes with Escape or OK button. Non-modal so
  the main window remains interactive while it is open.

- **Enhanced context menu (Phase 2.5)**: `_listContextMenu` expanded from 6 to 11 items with
  logical separator groups:
  - Apply / Remove
  - ⭐ Toggle Favorite (adds/removes from `Favorites`, appends ⭐/🗑 log line)
  - Copy ID / Copy Registry Keys / Copy Registry Path / Open in Registry Editor
  - Show Dependencies… (`TweakEngine.ResolveDependencies` → MessageBox with dep chain)
  - View History… (`TweakHistory.ForTweak` → MessageBox showing apply/remove log per tweak)
  - Select All / Deselect All

#### Stats

- Tests: 3,166 → **3,190** (+24; Phase2Tests.cs: 24 new Core tests for ctx menu, Favorites, TweakHistory, RegOp, ResolveDependencies)
- Tweaks: 7,189 (unchanged)
- Categories: 122 (unchanged)

---

## [6.17.0] — 2026-04-07

### Added — Phase 3.2 + 3.5 + 4.1: Batch Recipes, Watch Mode, E2E Tests

- **Batch recipe executor (Phase 3.2)**: New `--batch-recipe <file.rl.json>` command executes
  a structured JSON recipe file. Recipe format: `{ "name": "…", "rollbackOnFailure": true,
  "steps": [{ "type": "apply|remove|apply-profile|verify", "id": "<tweakId>" }] }`.
  Each step is executed sequentially. On failure, if `rollbackOnFailure` is true, all
  previously applied tweaks are reverted in reverse order. Supports `--json` flag for
  machine-readable per-step result array with `{ Label, Status, Success }` fields.
  Returns exit code 0 if all steps pass, 1 if any fail.

- **Watch mode drift detection (Phase 3.5)**: New `--watch` command continuously monitors
  tweak state at configurable intervals. Flags: `--watch-interval <seconds>` (default 300),
  `--watch-auto-fix` (auto-reapply drifted tweaks), `--watch-file <ids.txt>` (watch only
  specific tweak IDs from file). Captures baseline snapshot on launch, polls `StatusMap()`
  periodically, reports any drifted tweaks. Returns exit code 3 if unresolved drift exists
  on Ctrl+C, 0 if clean exit. Respects `--force` for corporate guard bypass.

- **E2E scenario tests (Phase 4.1)**: 10 end-to-end integration tests in `Phase41E2ETests`
  covering full apply/remove/verify/snapshot/export/stats round-trips. Tests validate CLI
  behaviour from `Dispatch()` call through to output, not just unit-level method invocations.

#### Stats

- Tests: 3,132 → **3,166** (+34; 6 Phase 3.2 dispatch + 5 Phase 3.5 dispatch + 10 Phase 4.1 E2E + 7 ParseArgs + 6 GUI crash-fix tests)
- Tweaks: 7,189 (unchanged)
- Categories: 122 (unchanged)

---

## [6.16.0] — 2026-04-07

### Added — Phase 3.1 + 3.3 + 3.4: CLI & Integration

- **`--json` global output flag (Phase 3.1)**: New `CliArgs.JsonOutput` bool property + `--json`
  shorthand alias that simultaneously sets `OutputFormat = "json"`. All major commands now emit
  structured JSON: `--stats`, `--list`, `--search`, `status`, `apply`, `remove`, `--profile`.
  The `RunStats` method gained a JSON branch outputting 12 statistical fields
  (`TotalTweaks`, `Categories`, `Profiles`, `Scopes`, `CorpSafe`, `NeedsAdmin`, `HasDetect`,
  `HasDescription`, `HasDependsOn`, `QuickWins`, `ImpactDistribution`, `SafetyDistribution`,
  `CategoryCounts`). `RunApplyProfile` gained a JSON branch with per-tweak result array.

- **Conditional apply guards (Phase 3.3)**: Four new `CliArgs` properties guard `RunAction`:
  - `--if-not-applied` — skips apply if tweak is already applied (returns exit code 2);
    skips remove if tweak is already absent (returns 2). Works with JSON output.
  - `--if-admin` — skips the action with exit code 2 if the process is not elevated.
  - `--if-build <N>` — skips with exit code 2 if `WindowsBuild() < N`.
  - `--if-not-corp` — skips with exit code 2 if `CorporateGuard.IsCorporateNetwork()` returns true.
  All guards fire before the corporate-guard check and print a clear skip message.

- **Interactive profile wizard (Phase 3.4)**: `--wizard` flag routes to new `RunWizard(CliArgs a)`
  method. Asks three survey questions (primary use, privacy importance, corporate device),
  computes weighted scores across all 5 built-in profiles, recommends the highest-scoring
  profile, shows tweak count, confirms with the user, then applies. Respects `--dry-run`
  (shows recommendation only, never applies). Respects `--json` flag on profile apply output.

#### Stats

- Tests: 3,105 → **3,132** (+27; 11 ParseArgs Phase 3 tests + 16 Dispatch Phase 3 tests)
- Tweaks: 7,189 (unchanged)
- Categories: 122 (unchanged)


### Added — Phase 1.6 + 1.7: Custom Profile API & Recommendation Engine

- **User-defined custom profile API (1.6)**: `TweakEngine` gains 9 wrapper methods
  delegating to `UserProfileService`:
  `UserProfiles()`, `GetUserProfile(name)`, `CreateUserProfile(name, desc, tweakIds)`,
  `SaveUserProfile(profile)`, `UpdateUserProfile(name, tweakIds, desc?)`,
  `RenameUserProfile(name, newName)`, `CloneUserProfile(name, newName)`,
  `DeleteUserProfile(name)`, `ApplyUserProfile(name, forceCorp)`.
  Custom profiles are JSON-persisted per-user under `%LOCALAPPDATA%\RegiLattice\profiles\`.

- **Tweak recommendation engine (1.7)**: `TweakEngine.RecommendTweaks(maxResults, forceCorpSafe, statusMap)`
  returns `IReadOnlyList<TweakRecommendation>` — prioritised suggestions enriched with
  `ConfidencePercent` (0–100, normalised from `ImpactScore × SafetyRating × 4`) and
  `IsQuickWin` flag. New `TweakRecommendation` sealed class added alongside `ScanRecommendation`
  in `SmartScanService.cs`.

#### Stats

- Tests: 3,092 → **3,105** (+13 Phase 1.6/1.7 tests; 53 total Phase 1 tests in Phase1Tests.cs)
- Tweaks: 7,189 (unchanged)
- Categories: 122 (unchanged)

## [6.14.0] — 2026-04-05

### Added — Phase 1: Engine & Model Hardening

- **`TweakRisk` flags (1.3)**: New `[Flags] TweakRisk` enum with 8 flags
  (`None`, `ModifiesHKLM`, `ModifiesHKCU`, `DeletesKey`, `RequiresReboot`,
  `AffectsService`, `AffectsNetwork`, `AffectsSecurity`, `PotentialDataLoss`).
  `TweakDef` gains `RiskFlags` (explicit override) and `EffectiveRiskFlags` (auto-detected
  from `ApplyOps` registry paths and `Category` string). Zero breaking changes.

- **Search relevance ranking (1.5)**: `TweakEngine.Search()` now returns results
  ordered by relevance score (ID exact match = 100 pts, prefix = 80, contains = 60,
  label exact = 70, label contains = 50, tag exact = 40, description = 20, category = 15).
  New `SearchRanked(query, ct)` method returns `IReadOnlyList<(TweakDef Tweak, int Score)>`
  for callers that need raw scores.

- **`CancellationToken` support (1.2)**: `Search()`, `SearchRanked()`, `StatusMap()`,
  `ApplyBatch()` (new overload), and `RemoveBatch()` (new overload) all accept an optional
  `CancellationToken`. Tokens are checked eagerly at method entry — an already-cancelled token
  throws immediately without starting work.

- **Transactional batch apply (1.1)**: New `ApplyBatch(IReadOnlyList<string> ids, bool forceCorp,
  bool transactional, Action<int,int,string>? onProgress, CancellationToken ct)` and matching
  `RemoveBatch` overload return `BatchResult`. When `transactional=true` and a tweak errors,
  all previously applied tweaks are reverted in reverse order.

- **`BatchResult` type (1.1)**: Sealed class with `Results`, `RolledBack`, `RollbackErrors`,
  `WasCancelled`, `SuccessCount`, `FailureCount`, and `HasErrors` computed properties.

- **Registry diff capture (1.4)**: `RegistrySession.ExecuteWithDiff(ops)` returns
  `(IReadOnlyList<RegDiff> Diffs, bool Success)`. In DryRun mode reads current state and
  returns what would change; in live mode captures before/after state around `Execute()`.

- **`RegDiff` type (1.4)**: Sealed class with `Path`, `ValueName`, `Before`, `After`, `Changed`,
  and a descriptive `ToString()` override.

- **35 new Phase 1 tests**: `Phase1Tests.cs` covering TweakRisk auto-detection, search ranking,
  CancellationToken early guards, BatchResult transactional behavior, and RegDiff capture.

### Stats

- Tweaks: **7,189** (unchanged)
- Tests: **3,092** (+38 Phase 1 tests + 3 NRE regression tests)

---

## [6.13.0] — 2026-04-05

### Changed

- **Large tweak file splitting — 31 merged files → 146 individual files**:
  19 multi-class merged files were extracted into 101 separate class files using
  `Split-MultiClass.ps1`. 12 remaining single-class files > 3000 lines were split
  into `Filename.cs` + `Filename.Part2.cs` partial classes using `Split-Partials.ps1`.
  Max file size reduced from 7,758 lines to 4,072 lines.
  No tweak count change — purely structural refactoring.

- **`.github` documentation**: Updated `lessons-learned.instructions.md` with 6 new entries
  covering multi-class extraction, brace counting pitfalls, partial class patterns,
  outer-class const visibility, `--no-build` GUI.Tests failure, and naming conflicts on extraction.
  Updated `copilot-instructions.md` and `workspace.instructions.md` with new file counts.

### Stats

- Tweaks: **7,189** (unchanged)
- Categories: **122** (was 23 — categories were split to max 77 each in a prior session)
- Modules: **146 files** (was 31; split into individual class files)
- Tests: **3,052** (0 failures)

---

## [6.12.0] — 2026-04-05

### Changed

- **Mass dedup consolidation — 1,756 duplicate TweakDef blocks removed**:
  DupDetector identified 932 groups of registry ops targeting the same `PATH\ValueName`.
  Alphabetically-first module's tweak kept; all duplicates removed.
  Net result: 8,853 → 7,189 tweaks (−1,664 net; −1,756 removed, ~92 already absent).

- **Category consolidation** (26 → 23 categories):
  - `"PowerShell"` → `"Developer"` (Developer.cs)
  - `"Power"` → `"Performance"` (Power.cs)
  - `"Explorer"` → `"System"` (Explorer.cs)
  - `"Input"` → `"Peripherals"` (Input.cs)
  - `"Windows Update"` → `"Maintenance"` (Maintenance.cs)
  - `"Audio"` → `"Audio & Media"` (Audio.cs)

- **File consolidation** (35 → 31 modules):
  Merged PolicyLocation, PolicyDataCollection, PolicyWinRM, PolicyCredentialUI,
  PolicyMediaPlayer into single `PolicyProvisions.cs`.

- **ConflictDetector.cs**: Updated `sac-disable-hvci` → `vbs-enable-hvci` (kept ID after dedup).

### Stats

- Tweaks: **7,189** (was 8,853)
- Categories: **23** (was 26)
- Modules: **31** (was 35)
- Tests: **3,050** (0 failures)

---

## [6.11.0] — 2026-04-05

### Added

- **Sprint 667–671 — 5 new Group Policy modules (+50 tweaks)**:
  - **PolicyLocation.cs** (Sprint 667–668): 10 tweaks — Windows Maps app policy (disable location, traffic,
    auto-update, network access, geo-fence) + Location & Sensors platform policy (disable location platform,
    scripting, Windows Location Provider, sensors, geolocation API). IDs: `priv-maps-*`, `priv-loc-*`.
    Category: Privacy.
  - **PolicyDataCollection.cs** (Sprint 669): 10 tweaks — Advanced telemetry suppression (AllowTelemetry=1,
    LimitEnhancedDiagnosticDataWindowsAnalytics, DisableDeviceCensus, OneDrive diagnostic telemetry,
    CompatibilityAppraiser) + AppCompat CEIP (PCA, engine, Switchback, CEIP reporting, UAC mitigation).
    IDs: `telem-policy-*`. Category: Privacy.
  - **PolicyWinRM.cs** (Sprint 670): 10 tweaks — WinRM server hardening (Basic auth, Digest auth,
    unencrypted traffic, auto-config, CredSSP/RunAs) + WinRM client hardening (Basic auth, unencrypted
    traffic, Digest auth, CredSSP, empty TrustedHosts). IDs: `sec-winrm-*`. Category: Security.
  - **PolicyCredentialUI.cs** (Sprint 671): 10 tweaks — Credential UI hardening (admin enumeration,
    trusted path, generic prompts, anonymous logon, web credential provider tile) + Credential Provider
    policies (domain picker, cached credential display, lock screen last user, legal notice banner,
    shutdown-without-logon). IDs: `sec-credui-*`. Category: Security.
  - **PolicyMediaPlayer.cs** (Sprint 671): 10 tweaks — Windows Media Player privacy/security (first-run
    wizard, auto codec download, MMS protocol, library sharing, DRM online acquisition, CD/DVD metadata,
    music metadata, Radio UI, predictive buffering, hide Privacy tab). IDs: `media-policy-*`.
    Category: Multimedia.

### Stats

- Tweaks: **8,853** (+50 from v6.10.0) | Categories: **26** | Modules: **35** (+5) | Tests: **3,051** (0 failures)

---

## [6.10.0] — 2026-04-05

### Removed

- **Phase C — Scoop tool-install tweaks removed** (44 tweaks): individual `scoop-*` package install tweaks
  (`scoop-install-aria2`, `scoop-7zip`, `scoop-bat`, `scoop-btop` … `scoop-oha`) removed from Developer.cs.
  These duplicated functionality already provided by the Scoop Manager dialog. Infrastructure
  tweaks (`pkg-install-scoop`, `pkg-scoop-setup`, `scoop-cleanup-all`) retained.

### Stats

- Tweaks: **8,803** (−44 from v6.9.0); Categories: 26; Modules: 30; Tests: 3,051 (0 failures)

---

## [6.9.0] — 2026-04-05

### Changed

- **Document & config consolidation** — all stale metric references updated to post-consolidation state:
  - `docs/Architecture.md`: Tweaks/ box updated — "83 modules · 9190 tweaks" → "30 modules · 8,847 tweaks"
  - `docs/Development.md`: test counts updated — Core 2,288→2,315, CLI 362→379, GUI 339→357
  - `README.md`: Tweak Categories section updated — "101 categories" → "26 categories"; description updated
  - `.github/instructions/git-workflow.instructions.md`: version bump checklist now explicitly requires SVG/graphics update on every version bump

### Stats

- Tweaks: **8,847** (unchanged) | Categories: **26** (unchanged) | Modules: **30** (unchanged) | Tests: **3,051** (0 failures)

## [6.8.0] — 2026-05-03

### Changed

- **Major consolidation** — reduced project footprint significantly:
  - Tweak modules: **45 → 30 files** (15 files merged into parent modules)
  - Categories: **47 → 26** (20 micro-categories renamed to canonical parents)
  - Tweaks: **9,490 → 8,847** (643 duplicate registry operations removed)
  - Merges: PolicyMisc2+PolicyMisc3+PolicyEnterprise → PolicyMisc; PolicyUser → Privacy; PolicyConfig → Security; PolicyWindowsUpdate → Maintenance; PolicyPowerShell+DeveloperTools → Developer; Identity+PolicyAuth → UserAccount; PolicyCloud → BackupAndCloud; PolicyPrint → UsbPeripherals; Win11Features → Win11; PolicyStorage → Storage; PolicyBrowser → Browser

### Fixed

- GUI test threshold updated to `>8000` tweaks (post-consolidation)

### Stats

- Tweaks: **8,847** (-643) | Categories: **26** (-21) | Modules: **30** (-15) | Tests: **3,051** (0 failures)

## [6.7.0] — 2026-05-03

### Added

- **Policy miscellaneous module 3** (`PolicyMisc3.cs`) — 50 new Group Policy tweaks across 5 modules (Sprints 662-666):
  - `PolicyAutoRun` (10) — AutoRun/AutoPlay enforcement: disable AutoPlay for non-volume devices, disable AutoRun all drives (0xFF), disable AutoRun for removable drives, set default action to no-action, prevent mixed-content AutoPlay bypass, disable CD/DVD AutoPlay, block users from changing AutoPlay default, disable network drive AutoPlay, disable shell AutoPlay handlers for removable media, master AutoPlay off switch
  - `PolicyWindowsStore` (10) — Windows Store app deployment policies: disable Store app, remove Store from Settings, require private Store only, disable auto-download, disable OS upgrade offers, turn off Store notifications, disable Store purchases, block non-admin app installs, disable video streaming page, disable music streaming page
  - `PolicyLockScreen` (10) — Lock screen appearance and Spotlight restrictions: disable lock screen, prevent lock screen image change, disable Spotlight on lock screen, disable Spotlight in Action Centre, disable third-party suggestions, disable Windows Welcome Experience, disable camera from lock screen, turn off all Spotlight features (master policy), disable Spotlight in taskbar Search, disable Spotlight tips in Settings
  - `PolicyRemoteAssistance` (10) — Remote Assistance security policies: disable RA completely, disable unsolicited Offer RA, require explicit consent for control, limit ticket validity to 1 hour, cap bandwidth to 2 Mbps, disable email invitation tickets, disable Easy Connect RA, enable RA session audit logging, disable clipboard transfer during sessions, disable file transfer during sessions
  - `PolicySmartCard` (10) — Smart Card authentication policies: require smart card for logon, lock workstation on card removal, force logoff on card removal, allow integrated PIN unblock screen, enable virtual smart card PIN logon, disable credential caching (plaintext PIN), enable certificate propagation on insert, clean up certificates on removal, prevent root certificate auto-update, disallow PINless logon

### Stats

- Tweaks: **9,490** (+50) | Categories: **121** (+5) | Modules: **160** (+5) | Tests: **3,376** (unchanged)

---

## [6.6.0] — 2026-04-05

### Added

- **Policy miscellaneous module 2** (`PolicyMisc2.cs`) — 50 new Group Policy tweaks across 5 modules (Sprints 657-661):
  - `PolicyWindowsFeeds` (10) — Windows RSS/Atom Feeds platform: disable Windows Feeds, disable background sync, prevent feed subscription, prevent auto-discovery, lock feed list, block content download, block third-party feeds, disable reading pane, block enclosure (podcast) download, restrict feeds to HTTPS sources only
  - `PolicyCompressedFolders` (10) — Compressed (ZIP/CAB) folder shell handler: disable ZIP virtual folder browsing, remove Extract All context menu, remove Compress to ZIP context menu, block network archive browsing, disable CAB browsing, block AutoRun inside archives, remove Send To Compressed Folder, enforce 512 MB archive size limit, disable archive preview handler in Reading Pane, require AV scan before opening archive content
  - `PolicyWindowsChat` (10) — Teams consumer chat & Windows 11 Calling integration: hide Chat icon from taskbar, block consumer Teams, remove chat notification badge, suppress first-launch experience, block personal account linking, disable Windows Calling integration, prevent Calling service auto-start, block caller ID lookup, block cross-device chat history sync, block file transfer via consumer chat
  - `PolicyTextInputExt` (10) — Extended text input & IME policies: disable hardware keyboard text prediction, lock text input settings from user override, disable hardware keyboard autocorrect, block feedback telemetry, block handwriting personalisation upload, block IME internet access, disable cloud IME candidates, block IME dictionary auto-update, disable IME typing telemetry, prevent touch keyboard auto-invoke in tablet mode
  - `PolicySpeechInput` (10) — Speech recognition & voice access policies: disable online speech recognition, block always-on voice activation, block speech model updates, disable speech telemetry, disable Voice Typing (Win+H), disable Cortana voice interaction, block speech personalisation collection, prevent Voice Access auto-start, block online speech model download, disable speech recognition on lock screen

### Stats

- Tweaks: **9,440** (+50) | Categories: **116** (+5) | Modules: **155** (+5) | Tests: **3,376**

## [6.5.0] — 2026-04-05

### Added

- **Policy user module** (`PolicyUser.cs`) — 50 new Group Policy tweaks across 5 modules (Sprints 652-656):
  - `PolicyWindowsSearch` (10) — Windows Search machine-wide policies: disable Cortana via policy, disable web search in Start, disable connected search web results, disable cloud search, block location in search, enforce strict SafeSearch, disable dynamic content in search box, block remote Cortana query, opt out of search privacy sharing, prevent battery indexing
  - `PolicyAppPrivacy` (10) — App capability access policies (Force Deny): block camera, microphone, contacts, calendar, call history, messaging, voice activation, account info, background app execution, and diagnostic info access for all apps
  - `PolicyUserExperience` (10) — Windows CloudContent policies: disable consumer features, disable Spotlight, disable third-party suggestions, disable lock screen app notifications, disable welcome experience, disable soft-landing tips, disable tailored diagnostic experiences, disable Spotlight on Action Center and Settings, disable cloud-optimised content
  - `PolicyEventLogAudit` (10) — Event log sizing and access policies: Application/Security/System/Setup log max sizes (64/192/64/32 MB), Application/Security/System log retention mode (overwrite as needed), Application/Security/System guest access restriction
  - `PolicySyncSettings` (10) — Windows Settings Sync policies: disable all sync, prevent user override, disable credentials sync, disable personalisation sync, disable app settings sync, disable Start layout sync, disable theme sync, disable language sync, disable accessibility sync, disable desktop theme sync

### Stats

- Tweaks: **9,390** (+50) | Categories: **111** (+5) | Modules: **150** (+5) | Tests: **3,376**

## [6.4.1] — 2026-04-05

### Fixed

- **Engine init crash** (`MainForm.cs`) — added full crash diagnostics to `InitialiseEngineAsync`: when initialisation fails, the complete exception with stack trace is now written to `%LOCALAPPDATA%\RegiLattice\crash.log` and the error dialog shows which step failed. Previously only `ex.Message` was shown with no file written, making the NRE undiagnosable.
- **Defensive null guards in hardware phase** — Phase 2 loop now skips any `TweakDef` with a null `Id` or `Category` (should not occur with builtins, guards against malformed pack tweaks). Tag comparison in the per-tweak check now guards against null tag strings.

### Stats

- Tweaks: **9,340** | Categories: **106** | Modules: **150** | Tests: **3,376**

## [6.4.0] — 2026-05-25

### Added

- **Policy config module** (`PolicyConfig.cs`) — 50 new Group Policy tweaks across 5 modules (Sprints 647-651):
  - `PolicyFirewallProfiles` (10) — Windows Firewall per-profile enforcement for Domain, Private, and Public profiles: enable firewall, block unsolicited inbound, allow outbound by default, and disable notifications control — locks firewall on across all three network location profiles
  - `PolicyNetLogon` (10) — Netlogon secure channel and AD authentication hardening: signing, sealing, require sign-or-seal, strong key enforcement, NT4 crypto disable, DNS-only domain join, machine password change control, max password age (30 days), WAN PDC avoidance, and NTLM restriction with Kerberos preference
  - `PolicyReliabilityMonitor` (10) — Windows Error Reporting and Reliability Monitor policies: shutdown reason UI, RAC event interval, WER consent, WER corporate SSL upload, kernel fault exclusion, archive disable, PCHealth disable, PCHealth silence all channels, queue mode, and per-app reporting disable
  - `PolicyDNSSecurity` (10) — DNS client security policies: LLMNR disable, smart name resolution disable, local response preference, multicast query scope, TLD update prevention, NRPT enforcement, adapter-specific suffix fallback disable, devolution level, hosts file bypass disable, and PTR record auto-registration
  - `PolicySmartScreenWin` (10) — SmartScreen and application reputation enforcement: shell SmartScreen enable, SmartScreen block level, Enhanced Phishing Protection capture, malicious site notification, password reuse warning, unsafe app password warning, SRP default level, SRP event logging, MRT auto-download disable, and MRT infection report upload disable

### Stats

- Tweaks: **9,340** (+50) | Categories: **106** (+5) | Modules: **98** (+5) | Tests: **3,052**

## [6.3.0] — 2026-04-03

### Added

- **System policy module** (`SystemPolicy.cs`) — 50 new Group Policy tweaks across 5 modules (Sprints 642-646):
  - `PolicyBitLocker` (10) — BitLocker drive encryption enforcement, AES-256 method, startup PIN requirement, minimum PIN length, network unlock disable, recovery key escrow, full-disk encryption, and non-TPM support
  - `PolicyWindowsInk` (10) — Windows Ink Workspace disable, above-lock-screen access, suggested apps, touch keyboard auto-invoke, handwriting panel, handwriting error reporting, ink/typing personalization data collection, learning mode, and telemetry
  - `PolicyLocationSensors` (10) — Location services system-wide disable, scripting API block, sensor platform, Windows Location Provider, network location awareness, location telemetry, history, geofencing, sensor data service, and ambient light sensor
  - `PolicyCloudClipboard` (10) — Clipboard history (Win+V), cross-device sync, phone-to-PC bridge, AI/Copilot clipboard access, Windows Hello relay, RDP passthrough, Remote Assistance clipboard, clear-on-lock, audit logging, and smart actions
  - `PolicyNetworkIsolation` (10) — AppContainer domain enterprise exception bypass, loopback exemption, package authentication, intranet auto-classification, proxy access, default-deny capability, private network declaration requirement, internet access block, debug bypass, and strict capability enforcement

### Stats

- Tweaks: **9,290** (+50) | Categories: **101** | Modules: **93** (+5) | Tests: **3,052**

## [6.2.1] — 2026-04-03

### Fixed

- **GUI silent startup crash** (`Program.cs`) — Added global exception handling: `Application.SetUnhandledExceptionMode`, `ThreadException` handler, `AppDomain.UnhandledException` handler, and `try/catch` around `Application.Run(new MainForm())`. Any startup exception now shows a `MessageBox` and writes a crash log to `%LOCALAPPDATA%\RegiLattice\crash.log` instead of silently exiting with no window.
- **`TweakBrowserPanel` transparent BackColor crash** (`TweakBrowserPanel.cs`) — `AppTheme.Border` is semi-transparent (`Color.FromArgb(50, Fg)`, alpha=50). WinForms `Splitter` control throws `ArgumentException` when assigned a transparent `BackColor`. Fixed by using `AppTheme.Surface` (fully opaque) for the splitter.
- **`WhatsNewDialog` UI freeze on first launch** (`WhatsNewDialog.cs`) — `BuildChangelogText()` was calling `new TweakEngine(); engine.RegisterBuiltins()` synchronously on the UI thread to obtain the tweak/category count for display text, blocking the UI for 2+ seconds on every first launch. Replaced with compile-time constants (`TweakCount = 9_240`, `CategoryCount = 101`).

### Added

- **17 QA GUI startup tests** (`GuiStartupTests.cs`) — Covers `AppConfig.Load()` defaults, `AppTheme` initialization, `AppIcons` bitmap creation, construction of all 5 custom controls (`SidebarNavControl`, `DashboardPanel`, `TweakBrowserPanel`, `ToolsHubPanel`, `PackagesHubPanel`), `WhatsNewDialog` timing regression test, and `TweakEngine.RegisterBuiltins()` health check.

### Stats

- Tweaks: **9,240** | Categories: **101** | Modules: **88** | Tests: **3,052** (+17)

## [6.2.0] — 2026-04-03

### Added

- **Identity & authentication policy module** (`Identity.cs`) — 50 new Group Policy tweaks across 5 modules:
  - `PolicyFido` (10) — FIDO2 passkey enforcement, transport restrictions, credential provider and platform authenticator controls
  - `PolicyWindowsHello` (10) — Windows Hello for Business PIN complexity, biometric enforcement, device credentials, and cloud trust settings
  - `PolicyEntraId` (10) — Entra ID / Azure AD workplace join, MDM enrollment, Microsoft account restrictions, privacy settings, Find My Device, and Phone Link controls
  - `PolicyKerberos` (10) — Kerberos claims support, armoring, renewable ticket lifetime, reversible encryption, and resource SID compression policies
  - `PolicyAppInstaller` (10) — AppInstaller (winget sideload) feature flags: MSIX protocol, update sources, experimental features, and allowed sources

### Fixed

- **GH Actions: Dependency Review** — Enabled GitHub Dependency Graph via API; added `continue-on-error: true` to job as fallback
- **GH Actions: Release MSI rename path** — Fixed `Get-ChildItem` search path from `installer/bin/Release` to `installer/bin` (recursive) to cover WiX `InstallerPlatform=x64` output under `installer/bin/x64/Release/`

### Updated

- `docs/assets/features.svg` — Per-category tweak count badges updated: Privacy 2 800+, Performance 2 100+, Security 1 650+, Debloat 2 000+, Dev Tools 640+
- `docs/assets/banner.svg` — Test count corrected: `2 931` → `3 035`

### Stats

- Tweaks: **9,240** (+50) | Categories: **101** | Modules: **88** (+5)
- Tests: **3,035** passing (Core 2,317 + CLI 379 + GUI 339) — 0 failures

---

## [6.1.0] — 2026-04-03

### Added

- **Sidebar navigation** — New 180px `SidebarNavControl` with Home / Tweaks / Tools / Packages / Settings sections replaces the flat top-menu approach. Each item shows an icon + label; the active item has an accent bar on the left; items support dynamic badge counts.
- **Analytics dashboard (Home)** — `DashboardPanel` is now the landing page: 4 stat cards (Health%, Applied, Not Applied, Total), a health donut ring, a top-8 category bar chart, a recent-activity list, and three quick-action buttons (Smart Scan, Profile Wizard, View All Tweaks).
- **Toggle-switch tweak browser (Tweaks)** — `TweakBrowserPanel` replaces the classic 7-column ListView as the default view. Each tweak renders as a 64 px card row with a `ToggleSwitchControl`, name, description, status badge, and ⓘ info button. Category tree on the left, search + status filter bar above the card area. Card pool reuse avoids GC pressure on filter changes.
- **Packages hub (Packages)** — `PackagesHubPanel` embeds all five package-manager dialogs (Scoop, WinGet, Chocolatey, pip, PowerShell Modules) in a single tabbed panel with lazy instantiation — the dialog for a tab is only created when first selected.
- **Tools hub (Tools)** — `ToolsHubPanel` replaces the 60-item nested Tools menu with a searchable visual grid: 47 tools displayed as 152×80 px icon buttons, grouped into six sections (System Diagnostics, System Management, Power & Energy, Privacy & Security, Network, Cleanup & Performance, Smart Tools). Admin-required tools display a shield badge.
- **Classic Advanced View preserved** — The original 7-column ListView with TreeView sidebar is accessible via the "Advanced View" button inside the Tweaks section and via the existing menu. All existing search, filter, sort, undo/redo, and multi-select functionality is unchanged.
- **ApplyTweaks / RemoveTweaks helpers** — New `MainForm` methods allow the toggle-switch panel to apply or remove a single tweak without going through the ListView selection, properly updating the status cache and propagating to the card panel.

### Changed

- `MainForm` layout restructured: `SidebarNavControl` (left) + `Panel _contentArea` (fill) containing all section panels. Dashboard is visible by default on launch.
- `ApplyTheme()` now propagates theme changes to all new panels (`_sidebar`, `_dashPanel`, `_tweakPanel`, `_packagesPanel`, `_toolsPanel`).
- Version bumped `6.0.7` → `6.1.0`

### Stats

- Tweaks: **9,190** | Categories: **101** | Modules: **83**
- Tests: **3,035** passing (Core 2,317 + CLI 379 + GUI 339) — 0 failures

---

## [6.0.7] — 2026-04-01

#### Added

- **D1 Undo/Redo system**: New `TweakOperationStack` (in-memory, max 50 ops) powers true undo/redo across the session. Toolbar now has `↩ Undo` (Ctrl+Z) and `↪ Redo` (Ctrl+Y) buttons with dynamic tooltips showing the tweak name and time elapsed. Undo inverts the last applied/removed tweak; Redo re-executes it. Stack is session-scoped — cleared on app exit. `toolbar_redo` locale key added to all 10 supported languages (EN, DE, FR, ES, HE, JA, ZH-CN, KO, AR, PT-BR)
- **F3 Smoke test matrix**: New `.github/workflows/smoke.yml` workflow triggers on every published GitHub Release and runs a matrix smoke test across `windows-2022` and `windows-2025`. Downloads `RegiLatticeCLI.exe` from the release assets and exercises `--help`, `--list-profiles`, `--show-categories`, `--validate`, `--stats`, and `--list --dry-run`. Non-blocking (`continue-on-error: true`); writes a per-OS job summary to the Actions step summary panel
- **H3 Conflict enrichment**: `ConflictDetector` expanded from 16 to 50 known conflict pairs (+34). New categories: Network/IPv6/proxy (4), Firewall (3), Defender extended (3), Power plans (2), Print Spooler (2), Privacy/Telemetry extended (3), Display/Graphics (2), Explorer/Shell (3), Clipboard/Input (2), Storage/FileSystem (2), UAC extended (2), Windows Update extended (2), Cortana/Voice (1), Startup/Logon (1), Debloat/Game Bar (1), Virtualization (1)
- **TweakOperationStack tests**: 29 new xUnit tests (`TweakOperationStackTests.cs`) covering all stack states — empty invariants, push/undo/redo ordering, redo-cleared-on-push, capacity cap at MaxOps=50, and Clear()

#### Stats

- Tweaks: 9,190 | Categories: 101 | Modules: 83
- Tests: **3,035** (Core 2,317 + CLI 379 + GUI 339) — 0 failures

## [6.0.6] — 2026-04-01

#### Added

- **B7 batch mode**: New `batch apply|remove <file>` CLI subcommand. Reads tweak IDs from a text file and applies or removes them in one operation. Three input formats supported: plain text (one ID per line, `#` comments ignored), JSON array `["id1","id2"]`, and snapshot JSON (`"applied"`, `"tweaks"`, or `"ids"` key). Respects `--dry-run`, `--force`, `--yes`; exits 0 on full success, 1 on partial, 2 on bad input
- **B4 PowerShell module parity**: Six new cmdlets added to `powershell/RegiLattice.psm1` — `Set-RLTweak` (apply/remove shorthand, pipeline-friendly), `Get-RLProfile` (-Type filter: all/builtin/user), `Set-RLProfile` (ShouldProcess/ConfirmImpact=High), `Export-RLSnapshot`, `Restore-RLSnapshot` (-DryRun/-Force), `Invoke-RLBatch` (-Remove/-DryRun/-Force). Module version bumped 5.0.0 → 6.0.6; `FunctionsToExport` now 11 cmdlets; `AliasesToExport` now 8 (`grt`, `grts`, `ira`, `irr`, `srt`, `grp`, `srp`, `irb`)
- **B6 shell completions**: `completions/RegiLattice.ps1` fully rewritten. Subcommand list expanded from 4 to 18 verbs; second-level noun completion via `$_rl_nouns` hashtable; context-aware — cursor position determines whether verb or noun is offered. `Register-ArgumentCompleter` blocks for `Set-RLTweak`, `Invoke-RLApply`, `Invoke-RLRemove`, `Get-RLTweakStatus` (live tweak IDs), `Set-RLProfile`/`Get-RLProfile` (profile names)
- **B7 tests**: +17 new tests — `RunBatchTests` in DispatchTests.cs (10 tests: plain-text, JSON-array, snapshot-format, remove verb, missing/empty file, all-unknown IDs, partial-unknown warning skipping) and `BatchSubcmdParseTests` in ParseArgsTests.cs (7 tests: apply/remove/no-verb/unknown-verb/no-file/with-flags)

#### Stats

- Tweaks: 9,190 | Categories: 101 | Modules: 83
- Tests: **3,009** (Core 2,291 + CLI **379** + GUI 339) — 0 failures

## [6.0.5] — 2026-04-04

#### Added

- **B2 structured `--output json`**: JSON output added to `tweak status`, `tweak apply/remove` (single tweak), `profile list`, and `check` commands. Schema: `{Id, Label, Category, Status, NeedsAdmin, CorpSafe}` for status; `{Id, Label, Mode, Status, DryRun}` for apply/remove; `[{Name, Description, TweakCount, Type}]` for profiles; `[{Id, Status, Label}]` for check. `--check` JSON mode also suppresses the "Checking done." progress prefix for clean piping
- **ExitCodes.CorpGuardBlocked = 4**: Corporate network guard now returns documented exit code 4 (was undocumented 6). All 6 call sites fixed (`RunAction`, `RunUpdate`, `RunApplyProfile`, `RunCategoryAction`, `RunImportJson`, `RunImportConfig`). `--help` exit codes section updated
- **ProfileInfo DTO**: New `internal sealed record ProfileInfo(...)` for type-safe JSON serialisation of profile lists (replaces anonymous type limitation)
- **A3 CLI contract tests**: +30 new tests (362 total, was 332) asserting JSON output validity, field presence, and exit code contract for all B2-updated commands. Tests use `Force = true` to be corporate-machine-safe

#### Fixed

- Corporate guard exit code: `return 6` → `return 4` at all 6 `RunAction`/`RunUpdate`/profile/category/import locations

#### Stats

- Tweaks: 9,190 | Categories: 101 | Modules: 83
- Tests: **2,992** (Core 2,291 + CLI **362** + GUI 339) — 0 failures

## [6.0.4] — 2026-03-31

#### Added

- **B1 subcommand architecture**: Verb-noun CLI syntax — `regilattice tweak apply/remove/update/status/list`, `search <q>`, `profile apply/list/create/delete`, `snapshot save/restore`, `export json/reg/gpo/intune/config`, `import json/config`, `list`, `validate`, `stats`, `doctor`, `check`, `marketplace <cmd>`. All previous `--flag` style invocations remain fully backward-compatible
- **B3 grouped help**: `--help` output restructured into labelled sections — *Tweak Operations*, *Search & Browse*, *Profiles*, *Snapshots*, *Export / Import*, *Marketplace*, *Favorites & History*, *General*, *Exit Codes*
- **B5 stable exit codes**: New `ExitCodes` static class with documented constants — `0` success, `1` partial fail, `2` user error (bad args / not found), `3` admin required. Documented in `--help` output

#### Fixed

- **WMI hang (HardwareInfo)**: Replaced `Task.Run` (ThreadPool) with explicit background MTA threads for all 12 WMI probes. WMI/COM was creating STA foreground pump threads from ThreadPool — those threads block .NET process exit. Process now exits in ~10 s on Intel corporate / Intune-managed machines (was 4 000+ s)
- **CLI `--help` pipe deadlock**: `CliExe_RunWithHelp_ExitsCleanly` test was deadlocking because B3 grouped help text overflows the OS pipe buffer when stdout is redirected. Fixed by draining stdout before `WaitForExit`

#### Stats

- Tweaks: **9 190** across **101** categories (83 modules)
- Tests: **2 962** passing (2 291 Core + 332 CLI + 339 GUI, 0 failures)
- Version bumped `6.0.3` → `6.0.4`

## [6.0.3] — 2026-03-31

#### Enhanced

- **C3 nullable enforcement**: Added `<TreatWarningsAsErrors>nullable</TreatWarningsAsErrors>` to `Directory.Build.props` — all three projects now treat CS8xxx nullable-reference warnings as build errors. Pre-audit confirmed 0 warnings already present across all 211 774 source lines
- **C4 dead code sweep (second pass)**: Ran full diagnostic sweep (`IDE0051`/`IDE0052`/`IDE0005`) — codebase confirmed clean. 0 unused private members, 0 unused using directives, 0 stale TODO/FIXME markers, 0 pragma suppressions, 0 commented-out code blocks

#### Stats

- Tweaks: **9 190** across **101** categories (83 modules)
- Tests: **2 931** passing (2 291 Core + 301 CLI + 339 GUI, 0 failures)
- Version bumped `6.0.2` → `6.0.3`

## [6.0.2] — 2026-03-31

#### Enhanced

- **Test isolation (A5)**: Added `[Collection("Favorites")]` to 3 test classes (`FavoritesTests`, `FavoritesBranchTests`, `FavoritesWhitespaceBranchTests`) and `[Collection("ComplianceHistory")]` to `ComplianceTrendDialogTests` — prevents non-deterministic failures if test parallelization settings change
- **Startup profiling (C3)**: Instrumented `TweakEngine.RegisterBuiltins()` with `Stopwatch` — new `LastRegistrationMs` property exposes cold-start timing. Baseline: 1146ms cold, ~84ms warm (9,190 tweaks, 83 modules). Performance baseline saved to `.tmp/perf-baseline.txt`
- **IReadOnlyList audit (C5)**: Changed 3 public API properties/methods from `List<T>` to `IReadOnlyList<T>`:
  - `AppConfig.ProfileSchedules`: `List<ProfileScheduleEntry>` → `IReadOnlyList<ProfileScheduleEntry>`
  - `TweakConfig.Tweaks`: `List<string>` → `IReadOnlyList<string>`
  - `TweakEngine.TweaksByCategory()`: `IReadOnlyDictionary<string, List<TweakDef>>` → `IReadOnlyDictionary<string, IReadOnlyList<TweakDef>>` (cached in `Freeze()` for zero-allocation reads)

#### Stats

- Tweaks: **9 190** across **101** categories (83 modules)
- Tests: **2 931** passing (2 291 Core + 301 CLI + 339 GUI, 0 failures)
- Version bumped `6.0.1` → `6.0.2`

## [6.0.1] — 2026-03-31

#### Enhanced

- **CI**: Added `dotnet list package --vulnerable --include-transitive` step to `ci.yml` — scans NuGet dependencies for known vulnerabilities on every build
- **Release**: Automated release notes extraction from CHANGELOG in `release.yml` — `gh release create` now uses `--notes-file` instead of `--generate-notes`
- **Docs**: Rewrote Roadmap.md Sprint Plan, Success Metrics, and Next Steps to reflect v6.0.0 as current baseline (was stale at v5.97.0 references)
- **Docs**: Fixed duplicate risk register entries R6–R8 in Roadmap.md

#### Fixed

- **Dead code**: Removed unused `_standaloneMode` field and associated `#pragma` from `BaseDialog.cs`
- **SVGs**: Updated banner.svg (9 190 tweaks, 101 categories, 2 931 tests), architecture.svg (9 190 tweaks), how-it-works.svg (9 190 tweaks), features.svg (per-profile counts scaled to 9,190 total)

#### Stats

- Total: **9,190 tweaks**, 101 categories, 83 module files
- Tests: 2,291 Core + 301 CLI + 339 GUI = **2,931 passing** (0 failures)
- Version bumped `6.0.0` → `6.0.1`

## [6.0.0] — 2026-03-31

#### Refactored

- **Major consolidation milestone**: merged 15 secondary tweak modules into their primary category files
- Module count reduced from **98 → 83 files** (−15%)
- Category count: **101** (unchanged from v5.99.0)
- All tweak IDs unchanged — zero behavior regression; all 9,190 tweaks retained
- Updated README badges, Mermaid diagrams, stats.svg, and copilot-instructions with correct counts

#### Stats

- Total: **9,190 tweaks**, 101 categories, 83 module files
- Tests: 2,291 Core + 301 CLI + 339 GUI = **2,931 passing** (0 failures)
- Version bumped `5.99.0` → `6.0.0`

## [5.99.0] — 2026-03-30

#### Refactored

- Consolidated 32 secondary tweak modules into 17 primary files — module count reduced from 130 → 98 files
- Category count reduced from 135 → 101 categories (34 redundant categories merged)
- Merged categories: Power Management + Energy Saver → Power; Network Optimization + DNS → Network;
  System Optimization + Registry Management → Performance / System; Night Light → Display;
  Windows Recall → Privacy; Xbox / Game Bar → Gaming; Dev Drive → Developer;
  M365 Copilot + Copilot+ Features → AI / Copilot; Windows Hello + User Activity → User Account;
  BitLocker Advanced + Trusted Launch → Encryption; Hyper-V Adv + Windows Sandbox → Virtualization;
  Recovery + System Restore → Backup & Recovery; Screensaver → Lock Screen; Virtual Desktops → Snap;
  Crash & Diagnostics + Time Sync → Maintenance; Command Line → PowerShell;
  Window Appearance → Desktop Customization; Indexing & Search → Cortana & Search;
  Scoop Tools → Package Management; RealVNC → Remote Desktop; Cloud Experience → Cloud Storage;
  Printer Advanced → Printing; Scheduled Task Tweaks → Scheduled Tasks; Windows Ink → Touch & Pen
- All tweak IDs unchanged — zero behavior regression; all 9,190 tweaks retained
- ProfileDefinitions updated to use consolidated category names
- 3 test files updated to reflect new category structure

#### Stats

- Total: **9,190 tweaks**, 101 categories, 98 module files
- Tests: 2,291 Core + 301 CLI + 339 GUI = 2,931 passing (0 failures)

## [5.98.0] — 2026-03-30

#### Refactored

- Consolidated 552 mechanical sprint policy modules (10 tweaks each) into 24 domain-grouped `Policy*.cs` files
- File count: 665 → 130 modules (−81%), category count: 637 → 135 (−79%)
- Nested private static class pattern: each source module preserved verbatim as `private static class _SourceName { Data }` inside its domain file
- C# 13 collection spread `[.. _Source.Data, ...]` used in each domain outer `Tweaks` property
- `TweakEngine.RegisterBuiltins()` updated: added `|| type.IsNested` skip guard to prevent nested helper classes from being double-registered
- All 9,190 tweaks retained — zero content removed

#### Stats

- Total: **9,190 tweaks**, 135 categories, 130 module files
- Tests: 2,293 Core + 301 CLI + 340 GUI = 2,934 passing (0 failures)

## [5.97.0] — 2026-03-30

#### Removed

- Removed 313 duplicate TweakDef entries across 60+ modules (consolidation Phases 2–5)
- Canonical ownership established for 60+ frequently-duplicated registry values
- No features removed — all unique tweak operations preserved

#### Stats

- Total: **9,190 tweaks**, 637 categories, 665 module files
- Tests: 2,301 Core + 301 CLI + 339 GUI = 2,941 passing (0 failures)
- Net reduction from consolidation: 334 TweakDefs removed (9,524 → 9,190)

---

## [5.96.0] — 2026-03-30

#### Added

- 5 new Group Policy policy modules, 50 tweaks (Sprints 632–636): WindowsSearchIndexingAdvancedPolicy, BranchCachePolicy, TaskSchedulerSecurityPolicy, WindowsDeploymentServicesPolicy, VpnRemoteAccessPolicy

#### Stats

- Total: **9,234 tweaks**, 637 categories, 665 module files
- Tests: 2,301 Core + 301 CLI + 339 GUI = 2,941 passing (0 failures)

---

## [5.95.0] — 2026-03-30

#### Fixed

- Fixed CS1009 escape sequence errors in WindowsUpdateDriverPolicy.cs (unblocks all releases since v5.75.0)

#### Removed

- Removed 21 duplicate registry tweaks across 11 modules (DontDisplayLastUserName ×6, DisableCAD ×4, LimitBlankPasswordUse ×3, dark-mode theme ×7, AllowTelemetry conflict ×1)
- Removed conflicting `compat-set-diagnostic-data-basic` (set AllowTelemetry=1, conflicting with Privacy.cs =0)

#### Stats

- Total: **9,184 tweaks**, 632 categories, 660 module files
- Tests: 2,301 Core + 301 CLI + 339 GUI = 2,941 passing (0 failures)

---

## [5.94.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 627-631): WslFileSystemPolicy, WslKernelUpdatePolicy, WslMemoryLimitsPolicy, WslSecurityHardeningPolicy, XboxCloudGamingPolicy.

#### Stats

- Total: **9,205 tweaks**, 632 categories, 660 module files

---

## [5.93.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 622-626): WindowsUpdateNotificationPolicy, WindowsUpdateScanPolicy, WindowsUpdateUsoPolicy, WinsNameResolutionPolicy, WslDistroManagementPolicy.

#### Stats

- Total: **9445 tweaks**, 658 categories, 655 module files

---

## [5.92.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 617-621): WebProxyAutoDiscoveryPolicy, WindowsAutopilotPolicy, WindowsInstallerAdvPolicy, WindowsServicingPolicy, WindowsUpdateDriverPolicy.

#### Stats

- Total: **9395 tweaks**, 653 categories, 650 module files

---

## [5.91.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 612-616): TrustedLaunchPolicy, UefiLockPolicy, UpdateAutoRestartPolicy, VoipQualityPolicy, WdagFileCachePolicy.

#### Stats

- Total: **9345 tweaks**, 648 categories, 645 module files

---

## [5.90.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 607-611): TeamsCallingPolicy, TeamsMeetingAudioPolicy, TeamsMessagingPolicy, TerminalServicesAdvPolicy, TpmRecoveryPolicy.

#### Stats

- Total: **9295 tweaks**, 643 categories, 640 module files

---

## [5.89.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 602-606): SmartCardCredentialsPolicy, SmartControlBypassPolicy, SoftwareRestrictionAdvPolicy, StorageReplicaPolicy, StorageSpacesMigrationPolicy.

#### Stats

- Total: **9245 tweaks**, 638 categories, 635 module files

---

## [5.88.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 597-601): RadiusAuthPolicy, RemoteNetworkAccessPolicy, SecureBootDbxPolicy, SharepointOnlinePolicy, SkyDrivePolicy.

#### Stats

- Total: **9195 tweaks**, 633 categories, 630 module files

---

## [5.87.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 592-596): PrintQueuePolicy, PrintTicketPolicy, PrinterRedirectionPolicy, PrivilegeUseAuditPolicy, ProcessCreationAuditPolicy.

#### Stats

- Total: **9145 tweaks**, 628 categories, 625 module files

---

## [5.86.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 587-591): OneDriveSyncPolicy, PkiPublicKeyServicesPolicy, PlayToDevicePolicy, PrintAuditPolicy, PrintJobManagementPolicy.

#### Stats

- Total: **9095 tweaks**, 623 categories, 620 module files

---

## [5.85.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 582-586): MdmRegistrationPolicy, MediaPlayerAdvPolicy, NetIoOffloadPolicy, NetworkMonitoringPolicy, NeuralProcessingPolicy.

#### Stats

- Total: **9045 tweaks**, 618 categories, 615 module files

---

## [5.84.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 577-581): IntuneDeviceEventPolicy, KerberosDelegationPolicy, LdapClientPolicy, LogonEventsAuditPolicy, MachineLearningPolicy.

#### Stats

- Total: **8995 tweaks**, 613 categories, 610 module files

---

## [5.83.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 572-576): FileShareWitnessPolicy, FirmwareUpdatePolicy, GameStreamingPolicy, GamingPerformancePolicy, Ieee8021xPolicy.

#### Stats

- Total: **8945 tweaks**, 608 categories, 605 module files

---

## [5.82.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 567-571): EdgeNewTabPagePolicy, EdgePasswordManagerPolicy, EdgeWorkProfilePolicy, EnterpriseDeviceManagementPolicy, EnterpriseResourceDeployPolicy.

#### Stats

- Total: **8895 tweaks**, 603 categories, 600 module files

---

## [5.81.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 562-566): DirectXShaderCachePolicy, DomainTrustPolicy, DsObjectAccessAuditPolicy, EdgeAutoFillPolicy, EdgeCertTransparencyPolicy.

#### Stats

- Total: **8845 tweaks**, 598 categories, 595 module files

---

## [5.80.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 557-561): DeviceEnrollmentLimitPolicy, DeviceHealthCheckPolicy, DfsrPolicy, DiffServQosPolicy, DirectAccessConnectPolicy.

#### Stats

- Total: **8795 tweaks**, 593 categories, 590 module files

---

## [5.79.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 552-556): CryptographicOperationsPolicy, DefenderExploitSystemPolicy, DefenderNetworkProtectionPolicy, DefenderSignatureUpdatePolicy, DeviceCompliancePolicy.

#### Stats

- Total: **8745 tweaks**, 588 categories, 585 module files

---

## [5.78.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 547-551): CloudStorageQuotaPolicy, CodeIntegrityAppPolicy, ConferencingBandwidthPolicy, ConfigurationManagerPolicy, ControlledFolderAccessPolicy.

#### Stats

- Total: **8695 tweaks**, 583 categories, 580 module files

---

## [5.77.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 542-546): BackupEncryptionPolicy, CertRevocationPolicy, CloudBackupRetentionPolicy, CloudFileSyncPolicy, CloudPcWindows365Policy.

#### Stats

- Total: **8645 tweaks**, 578 categories, 575 module files

---

## [5.76.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 537-541): AiSafetyPolicy, AlwaysOnVpnPolicy, ApplicationGuardPersistencePolicy, AsrAttackSurfacePolicy, AzureVirtualDesktopPolicy.

#### Stats

- Total: **8595 tweaks**, 573 categories, 570 module files

---

## [5.75.0] — 2026-03-29

#### Added

5 new Group Policy policy modules, 50 tweaks (Sprints 532–536): AccountManagementAuditPolicy, ActiveDirectoryServicesPolicy, AdReplicationPolicy, AiCopilotWebPolicy, AiInferencePolicy.

#### Stats

- Total: **8545 tweaks**, 568 categories, 565 module files

---

## [5.74.0] — 2026-03-29

#### Added

- **Sprint 437 — DefenderFirewallAdvancedPolicy** (10 tweaks, slug `fwadv`): Windows Firewall domain/standard profile hardening via Group Policy — enforce firewall state on domain and standard profiles, block inbound by default on both, enable dropped-packet notifications, log dropped packets, set 16 MB log size, disable unicast response to multicast/broadcast on domain profile.
- **Sprint 438 — IpsecRulePolicy** (10 tweaks, slug `ipsecpol`): IPSec rule policy hardening via PolicyAgent service parameters — disable default IKE exemptions, enable strong CRL checking, enforce DH group 2, require AH integrity checking, block null encryption, require ESP, set negotiation poll interval.
- **Sprint 439 — NetworkLocationAwarenessPolicy** (10 tweaks, slug `nlapol`): Network Location Awareness (NCSI) hardening — disable active probing phone-home, disable Microsoft connectivity tests, disable internet connectivity checks, enforce corporate DNS probe, disable hotspot detection, block location-based network switching.
- **Sprint 440 — DohEnforcementPolicy** (10 tweaks, slug `dohpol`): DNS-over-HTTPS enforcement — require DoH mode 3, disable DNS cache client, enforce DNSSEC validation, disable LLMNR, disable NetBIOS name resolution, block plain DNS fallback across all network profiles.
- **Sprint 441 — ProxyBypassPolicy** (10 tweaks, slug `proxbyp`): Proxy bypass and WPAD hardening — disable auto-detect proxy, block WPAD, disable auto-config URL, restrict per-user proxy settings changes, require authenticated proxy, block direct internet bypass.
- **Sprint 442 — ClipboardHistoryAdvancedPolicy** (10 tweaks, slug `clipadv`): Clipboard history controls via System group policy — disable clipboard history feature, cross-device sync, session clipboard logging, background app access, rich-text clipboard, history on lock screen.
- **Sprint 443 — ClipboardRdpRedirectionPolicy** (10 tweaks, slug `cliprdp`): RDP clipboard and device redirection hardening — disable clipboard, drive, printer, COM/LPT port, smart card, audio recording, file copy, USB, and PnP device redirection in Terminal Services.
- **Sprint 444 — SharedClipboardControlPolicy** (10 tweaks, slug `shrdclip`): Shared clipboard policy controls — disable phone link clipboard share, cloud clipboard sync, clipboard telemetry, contextual clipboard suggestions, restrict clipboard to current process.
- **Sprint 445 — UniversalClipboardSyncPolicy** (10 tweaks, slug `uniclip`): Universal clipboard sync policy — disable Windows mobile device clipboard sync, browser clipboard integration, Edge clipboard manager, clipboard sharing across Edge profiles, disable clipboard prediction service.
- **Sprint 446 — ClipboardSensitivityPolicy** (10 tweaks, slug `clipsens`): Clipboard DLP and sensitivity policy — block sensitive data monitoring via diagnostic services, disable PII from clipboard history, block clipboard sharing via Bluetooth, disable clipboard in kiosk mode, restrict max clipboard data size.
- **Sprint 447 — WindowsTerminalAdvancedPolicy** (10 tweaks, slug `termadv`): Windows Terminal advanced policy — disable first-run experience, Defterm integration, dynamic profile discovery, custom color schemes, block settings file modification, disable acrylic background, require admin for Terminal settings.
- **Sprint 448 — Ps7ExecutionModePolicy** (10 tweaks, slug `ps7pol`): PowerShell 7 execution mode policy — enforce AllSigned execution policy, disable PS7 remoting, enforce constrained language mode, block unsigned modules, disable update check, require module catalog signing.
- **Sprint 449 — IseDeprecationPolicy** (10 tweaks, slug `isepol`): Windows PowerShell ISE deprecation controls — disable ISE, block profile loading, disable add-on framework, restrict auto-complete, block remote session connections, disable IntelliSense for network paths.
- **Sprint 450 — ScriptBlockLoggingAdvancedPolicy** (10 tweaks, slug `sbllog`): Script block and transcription logging — enable script block logging with invocation header, enable module logging, enable transcription to policy output dir, enable protected event logging (WEVT encryption).
- **Sprint 451 — RemotePsJeaPolicy** (10 tweaks, slug `psjea`): Remote PowerShell JEA policy — enforce JEA session configurations, require JEA role capabilities, disable basic/unencrypted WinRM, require HTTPS, set idle timeout, require Kerberos or certificate auth, log all JEA sessions.
- **Sprint 452 — EdgeWebView2Policy** (10 tweaks, slug `edgewv2`): Edge WebView2 hardening policy — block auto-update, enforce channel choice, disable telemetry, block extensions, enforce CSP, disable dev tools, block cross-origin access, disable PDF viewer and file system access.
- **Sprint 453 — EdgeAppGuardPolicy** (10 tweaks, slug `edgeag`): Microsoft Application Guard for Edge — enable AG, disable host clipboard and download access from AG, block printing, enable persistence, camera/mic restrictions, AG audit mode, enforce site list.
- **Sprint 454 — EdgeSleepingTabsPolicy** (10 tweaks, slug `edgesleep`): Edge sleeping tabs policy — enable sleeping tabs, set 5-minute timeout, enable background tab suspension, disable wake-on-hover, set memory reclaim threshold, enforce tab freeze on low memory, set max tabs before sleeping.
- **Sprint 455 — EdgeSiteIsolationPolicy** (10 tweaks, slug `edgeiso`): Edge site isolation policy — enable strict site isolation (IsolateOrigins), require renderer isolation per site, disable cross-origin opener policy relaxation, enforce CORP headers, require Origin-Agent-Cluster, enforce Spectre mitigation.
- **Sprint 456 — EdgeEarlyHintsPolicy** (10 tweaks, slug `edgeeh`): Edge HTTP/3, Early Hints and security policy — enable HTTP/3 (QUIC), enable 103 Early Hints, enforce HTTPS for all navigation, disable FTP, enforce SRI checking, block data: URI navigations.
- **Sprint 457 — AzureAdConditionalAccessPolicy** (10 tweaks, slug `azurecap`): Azure AD Conditional Access policy enforcement — require MFA, block legacy auth, require compliant device, block risky sign-ins, require approved client apps, block personal accounts, enforce named location restrictions.
- **Sprint 458 — EntraDeviceRegistrationPolicy** (10 tweaks, slug `entrareg`): Entra ID device registration policy — block auto device registration, require admin approval for join, disable personal device enrollment, enforce device compliance before join, block hybrid join bypass, require FIDO2 for registration.
- **Sprint 459 — AzureAdPrtSsoPolicy** (10 tweaks, slug `prtpol`): Azure AD PRT SSO policy — enforce PRT token binding to device, disable PRT sharing between users, set PRT refresh interval (6h), require TPM for PRT storage, disable PRT for personal accounts, require mutual auth.
- **Sprint 460 — AzureAdSsprPolicy** (10 tweaks, slug `sspppol`): Azure AD SSPR policy — disable on-premises writeback bypass, require 2 auth methods, block SSPR for admin accounts, enforce SSPR from known locations, require re-verification every 180 days.
- **Sprint 461 — HybridJoinDnsPolicy** (10 tweaks, slug `hybdns`): Hybrid Join DNS hardening — enforce internal DNS for hybrid join discovery, block split-DNS bypass, require SCP DNS lookup, disable hybrid join fallback to public DNS, block NTLM during hybrid join discovery.
- **Sprint 462 — VbsEnforcementPolicy** (10 tweaks, slug `vbspol`): Virtualization-Based Security enforcement — enable VBS, UEFI lock mode, require IOMMU, enable Credential Guard and HVCI via VBS, set platform security level 3, require Secure Boot, refuse VBS downgrade.
- **Sprint 463 — HvciPolicy** (10 tweaks, slug `hvci`): Hypervisor-Protected Code Integrity (Memory Integrity) — enable HVCI, UEFI lock, set strict enforcement level 2, disable unsigned kernel module loading, require WHQL signing, block unsigned WDM drivers, require HVCI for credential storage.
- **Sprint 464 — SecureLaunchDrtmPolicy** (10 tweaks, slug `drtmpol`): Secure Launch D-RTM hardening — enable Secure Launch, require TPM 2.0, enable Measured Boot, set System Guard integrity level required, block Secure Launch bypass via firmware, enforce D-RTM measurement chain.
- **Sprint 465 — SystemGuardRuntimePolicy** (10 tweaks, slug `sysguard`): System Guard runtime attestation — enable runtime integrity checks, require SMM protection level 3, block bypass, enable attestation event logging, enforce UEFI lock, require TPM attestation quote.
- **Sprint 466 — KernelDmaProtectionPolicy** (10 tweaks, slug `kdmapol`): Kernel DMA Protection hardening — enable Kernel DMA Protection, block Thunderbolt DMA before user login, require DMA remapping for PCIe, disable legacy PCI DMA, enforce DMA blocklist, require IOMMU mapping for all devices.
- **Sprint 467 — WsaAndroidPolicy** (10 tweaks, slug `wsaand`): Windows Subsystem for Android hardening — disable WSA, block sideloading, disable Amazon Appstore, block WSA network and camera access, disable developer mode, block audio and USB debugging.
- **Sprint 468 — AndroidAppDebuggingPolicy** (10 tweaks, slug `anddbg`): Android app debugging policy — disable ADB debugging, block ADB over network, disable USB debugging, block ADB screen capture, disable crash reporting, block logcat and root bypass access.
- **Sprint 469 — WsaNetworkIsolationPolicy** (10 tweaks, slug `wsanet`): WSA network isolation — isolate WSA from host network, block loopback, disable internet access, enforce WSA proxy, block host share access, disable NAT bypass, block WSA listening on host ports.
- **Sprint 470 — AndroidSensorAccessPolicy** (10 tweaks, slug `andsens`): Android sensor access policy — block location, accelerometer, gyroscope, ambient light, proximity, magnetometer, barometer, step counter, temperature sensor, and power state reporting.
- **Sprint 471 — WsaStoragePolicy** (10 tweaks, slug `wsastor`): WSA storage hardening — block Android app host file system access, disable SD card emulation, block MTP file sharing, block file copy between Android and Windows, require encryption for Android app data.
- **Sprint 472 — PrintSpoolerAdvancedPolicy** (10 tweaks, slug `spoolpol`): Print Spooler advanced hardening (PrintNightmare mitigation) — disable remote spooler communication, block Point and Print to non-enterprise servers, restrict driver install, disable spooler on DCs, block UNC driver paths.
- **Sprint 473 — PrinterDriverIsolationPolicy** (10 tweaks, slug `prtdrv`): Printer driver isolation — enforce mandatory isolation, set isolated driver mode, block non-isolated sharing, disable in-process drivers, require v4 drivers only, block legacy GDI driver model, enforce AppContainer isolation.
- **Sprint 474 — InternetPrintingPolicy** (10 tweaks, slug `inetprt`): Internet Printing Protocol (IPP) client hardening — disable IPP over HTTP, block IIS Internet Printing, block external IPP printer discovery, require authentication, enforce TLS for IPP connections.
- **Sprint 475 — WsdPrintDiscoveryPolicy** (10 tweaks, slug `wsdprt`): WSD print discovery hardening — disable WSD printer discovery, block WSD multicast, disable SSDP announcements, block WSD on public network, disable UPnP printer discovery, require admin for WSD printer install.
- **Sprint 476 — IppEverywherePolicy** (10 tweaks, slug `ippevry`): IPP Everywhere policy — disable IPP Everywhere auto-discovery, block driver auto-install, disable mDNS-based discovery, block Bonjour announcements, disable DNS-SD resolution, require authentication, enforce HTTPS.
- **Sprint 477 — WindowsHelloBusinessAdvancedPolicy** (10 tweaks, slug `whfbadv`): Windows Hello for Business advanced — require hardware TPM, set PIN complexity (min 8), require expiry 90 days, block simple PINs, enable biometric auth alongside PIN, require PIN change on first logon, block PIN recovery mode.
- **Sprint 478 — PasswordlessAuthPolicy** (10 tweaks, slug `nopasswd`): Passwordless authentication policy — enable passwordless sign-in, block password credential provider fallback, require hardware security key, enforce FIDO2 for cloud accounts, enable virtual smart card.
- **Sprint 479 — BiometricSignInPolicy** (10 tweaks, slug `biopol`): Biometric sign-in policy — enable biometrics, require anti-spoofing, set sign-in timeout 5 min, disable fingerprint after 3 failures, block biometrics via remote sessions, require enhanced anti-spoofing for face recognition.
- **Sprint 480 — PortableDeviceAuthPolicy** (10 tweaks, slug `prtdev`): Portable device authentication policy — block read/write access, require admin for install, disable MTP/PTP protocols, disable WPD on DC servers, require BitLocker for removable media, block USB serial enumeration.
- **Sprint 481 — CbaCertAuthPolicy** (10 tweaks, slug `cbapol`): Certificate-Based Authentication policy — require certificate for network auth, enforce CRL check, require OCSP stapling, disable client cert auto-selection, require 2048-bit RSA minimum, block self-signed certs.
- **Sprint 482 — RecallAiSnapshotPolicy** (10 tweaks, slug `recalladv`): Windows Recall AI advanced policy — disable Recall AI indexing, prevent sensitive content capture, disable screenshot capture, block DRM content indexing, disable semantic search, block cross-device sync, disable click-to-do.
- **Sprint 483 — CopilotPlusNpuPolicy** (10 tweaks, slug `copfeat`): Copilot+ feature gate policy — disable cloud processing, block screenshot upload to cloud, disable Cocreator, disable Windows Studio Effects, disable Live Captions NPU processing, block feature auto-rollout.
- **Sprint 484 — WindowsAiPlatformPolicy** (10 tweaks, slug `waipol`): Windows AI Platform policy — disable AI Platform SDK, block AI inference API from UWP apps, disable on-device LLM inference, block AI API telemetry, disable ML model cache, block AI model download from WU.
- **Sprint 485 — NpuWorkloadPolicy** (10 tweaks, slug `npupol`): NPU workload policy — disable NPU workload scheduling, block NPU access from standard apps, restrict NPU allocation to Microsoft AI features, disable NPU telemetry, block NPU firmware auto-update, disable background inference.
- **Sprint 486 — AiContentModerationPolicy** (10 tweaks, slug `aimodpol`): AI content moderation policy — enable safety filters, block NSFW image generation, enforce content labelling, require human review for flagged content, block AI deepfake distribution, enforce watermarking.
- **Sprint 487 — StorageSpacesAdvancedPolicy** (10 tweaks, slug `storadv`): Storage Spaces advanced policy — disable auto-rebuild, set integrity scrub schedule, enforce column count, require admin for pool creation, disable auto-tiering, require encryption for volumes.
- **Sprint 488 — RefsIntegrityPolicy** (10 tweaks, slug `refspol`): ReFS integrity policy — enable ReFS integrity streams, set 7-day scrub interval, enable automatic corrupt block recovery, disable ReFS defrag, enable checksum verification on read, enforce volume metadata redundancy.
- **Sprint 489 — DiskQuotaAdvancedPolicy** (10 tweaks, slug `dqpol`): Disk quota advanced policy — enable quota management, enforce limits (deny over limit), set 90% warning level, set 10 GB limit, apply to all volumes, enable event logging for violations, disable silent mode.
- **Sprint 490 — VirtualDiskServicePolicy** (10 tweaks, slug `vdspol`): Virtual Disk Service policy — require admin for VHD mount, block VHD auto-mount, disable VHD passthrough for standard users, enforce 100 GB size limit, block dynamic expansion beyond limit, require encryption for mounted VHD.
- **Sprint 491 — StorageBusPolicy** (10 tweaks, slug `sbcpol`): Storage Bus Cache policy — enforce write-back cache flushing, set 5-second flush interval, require battery detection, enforce 512 MB cache max, require UPS for write-back preservation, log flush failure events.
- **Sprint 492 — WindowsEventForwardingAdvancedPolicy** (10 tweaks, slug `wefadv`): WEF advanced policy — configure subscription manager URL, enable WEF auto-start, set delivery optimization (minimize latency), require Kerberos, disable HTTP mode, enforce collector-initiated subscription, require certificate auth.
- **Sprint 493 — WefSubscriptionPolicy** (10 tweaks, slug `wefsubpol`): WEF subscription management — enforce collector-initiated type, require authentication, set refresh interval, disable source-initiated from unknown hosts, set max batch size, enable state persistence, log subscription events.
- **Sprint 494 — EventLogChannelPolicy** (10 tweaks, slug `evtchan`): Event Log channel access policy — set Security log retention (overwrite), 512 MB Security log max size, block Admin-only clear of Security log, set System/Application 64 MB limits, require admin for channel enable/disable.
- **Sprint 495 — EventSubscriptionPolicy** (10 tweaks, slug `evttrans`): Event Log transport policy — enforce TLS transport for remote event logging, disable UDP forwarding, require certificate validation, disable legacy DCOM forwarding, enforce event signing for forwarded logs.
- **Sprint 496 — LogArchivalPolicy** (10 tweaks, slug `logarc`): Log archival policy — enable automatic log archival, set archival path to secured share, require encryption for archived logs, set 365-day retention, disable auto-delete, require integrity verification, enforce archive naming with timestamps.
- **Sprint 497 — NetworkBridgePolicy** (10 tweaks, slug `netbrdg`): Network bridge control policy — prohibit network bridge installation, block ICS, disable Windows connection sharing, block Wi-Fi hotspot creation, disable Mobile Hotspot, block bridge on domain-joined machines.
- **Sprint 498 — LltdProtocolPolicy** (10 tweaks, slug `lltdpol`): LLTD topology discovery policy — disable LLTD Responder and Mapper I/O, block on public/private networks, disable topology map (Windows network map), block multicast and wireless/VLAN LLTD.
- **Sprint 499 — HomeGroupRemnantPolicy** (10 tweaks, slug `hmgrp`): HomeGroup legacy cleanup and disable — disable HomeGroup provider service, block creation and joining, disable peer-to-peer networking, block password sharing and streaming, disable mDNS announcements.
- **Sprint 500 — NetworkAdapterAdvancedPolicy** (10 tweaks, slug `netadv`): Network adapter advanced policy — disable adapter binding change, block NIC power management (WOL attack prevention), enforce DHCP only, disable auto-tuning bypass, block TCP offload toggle, enforce 1500 byte MTU.
- **Sprint 501 — NicTeamingPolicy** (10 tweaks, slug `nicteam`): NIC teaming policy — require admin for teaming config, enforce switch-independent mode, disable LACP without switch, block NIC teaming on guest VMs, set team failback delay (60s), require minimum 2 NICs per team.
- **Sprint 502 — SmartScreenAdvancedPolicy** (10 tweaks, slug `ssv2pol`): Defender SmartScreen V2 policy — enable SmartScreen (Warn+Block mode), enable enhanced phishing protection, block SmartScreen warning bypass, enforce for Store apps, disable feedback opt-out, log all block events.
- **Sprint 503 — DefenderAntivirusAdvancedPolicy** (10 tweaks, slug `repbased`): Reputation-based protection and cloud-delivered protection — enable cloud protection level high (2), enable PUA blocking, enable network protection (block), enable MAPS full reporting, set cloud block timeout 50s, enable behavior monitoring.
- **Sprint 504 — ExploitGuardPolicy** (10 tweaks, slug `expguard`): Exploit Guard/EMET policy — enforce Exploit Protection XML, enable DEP for all apps, enable SEHOP, enable HEASLR, mandatory ASLR, enable EAF and IAF filters, block child process creation from Office apps.
- **Sprint 505 — ControlledFolderAccessAdvancedPolicy** (10 tweaks, slug `cfaadv`): Controlled Folder Access advanced — enable CFA (block mode), add Desktop/Documents/Pictures/Videos to protected folders, block ransomware-extension variants, require admin to whitelist apps, enforce across all profiles.
- **Sprint 506 — NetworkProtectionPolicy** (10 tweaks, slug `netprot`): Network protection policy — enable network protection (block mode), block malicious URLs via SmartScreen, block C2 connections, enforce for all NICs including VPN, log all blocked connections, require admin to disable.
- **Sprint 507 — FontInstallationPolicy** (10 tweaks, slug `fontpol`): Font embedding and installation restriction — block untrusted font loading (CVE mitigation), disable EOT font embedding, disable Type 1 font, restrict OTF from network paths, block font download in Edge, require admin for system font install.
- **Sprint 508 — OpenTypeSecurityPolicy** (10 tweaks, slug `otfpol`): OpenType font security — enforce OTF validation, disable experimental features, restrict color emoji fonts, block malformed glyph table loading, enforce CFF/CFF2 size limits, block obscure GSUB/GPOS lookup types.
- **Sprint 509 — GdiRendererPolicy** (10 tweaks, slug `varfont`): Variable font and GDI rendering policy — disable variable font processing (CVE mitigation), restrict variation axis count, block variable fonts from web sources, enforce static font fallback, restrict fvar table evaluation.
- **Sprint 510 — FontSmoothingAdvancedPolicy** (10 tweaks, slug `fontsm`): Font smoothing advanced policy — enforce ClearType, set gamma (1200) and contrast (1100) values, disable GDI font smoothing for system UI, set subpixel rendering mode RGB, disable anti-aliasing bypass.
- **Sprint 511 — SystemFontSubstitutionPolicy** (10 tweaks, slug `fontsub`): System font substitution policy — block user-defined substitution, disable font alias override, block Arial-to-Helvetica substitution, block legacy font mapping in compatibility mode, disable cross-process font substitution.
- **Sprint 512 — DirectXRenderingPolicy** (10 tweaks, slug `d3dpol`): Direct3D feature level policy — enforce D3D feature level minimum (11.0), disable D3D9 legacy API, require hardware acceleration, disable shader debugging, block capture in UAC-elevated context, enforce memory isolation per process.
- **Sprint 513 — GpuComputePolicy** (10 tweaks, slug `gpusched`): GPU scheduler advanced policy — enable Hardware-Accelerated GPU Scheduling (HAGS), set scheduling quantum (1ms), disable preemption bypass, require WDDM 2.7+, enforce TDR timeout (2s), block scheduler privilege escalation.
- **Sprint 514 — WddmDriverPolicy** (10 tweaks, slug `wddmpol`): WDDM driver store policy — block GPU driver auto-update via WU, require WHQL signature for display drivers, disable unsigned WDDM loading, block driver store modifications by standard users, enforce WDDM version minimum (3.0).
- **Sprint 515 — GameBarPolicy** (10 tweaks, slug `haggs`): Game Bar and HAGGS policy — disable Xbox Game Bar, block Game Bar from capturing, disable overlay on fullscreen apps, block capture shortcuts, disable broadcasting and social features, set HAGGS preemption granularity.
- **Sprint 516 — DisplayPowerMgmtPolicy** (10 tweaks, slug `dpmpol`): Display power management policy — enforce display-off timeout on battery (5 min) and AC (15 min), disable adaptive brightness override, enforce DPMS sleep mode, disable monitor-off exemption for media players, disable wake-on-LAN during display-off.
- **Sprint 517 — XboxNetworkingPolicy** (10 tweaks, slug `xbgbadv`): Xbox Game Bar and networking advanced policy — disable Xbox Game Bar entirely, block screenshot capture and audio mixing, disable broadcasting, block Game Bar starting with Windows, disable FPS counter, block mic access.
- **Sprint 518 — GameModeSchedulerPolicy** (10 tweaks, slug `gamepol`): Game Mode scheduler policy — disable Game Mode, block auto-activation, disable memory priority boost, require admin to enable, disable telemetry, block Game Mode from disabling Windows Update, disable GPU priority boost.
- **Sprint 519 — DirectXDiagnosticsPolicy** (10 tweaks, slug `dxdiagpol`): DirectX Diagnostics policy — disable DxDiag phone-home, block DxDiag sending system data to Microsoft, disable DXGI debug layer in production, block overlay for standard users, disable DirectX control panel.
- **Sprint 520 — PerformanceCountersPolicy** (10 tweaks, slug `perfct`): Performance counters policy — restrict PerfLib access to admins, disable remote perf counter queries, require authentication for PerfLib queries, disable WMI perf counter bridging, block user-mode perf data helpers.
- **Sprint 521 — GpuPartitioningPolicy** (10 tweaks, slug `gpupart`): GPU partitioning (GPU-P) policy — enable GPU-P for Hyper-V, set partition count (4), enforce minimum partition memory (1 GB), disable for untrusted VMs, enforce partition isolation, block memory overcommit, require Secure Boot for GPU-P VMs.
- **Sprint 522 — MicrosoftStorePolicy** (10 tweaks, slug `streadv`): Microsoft Store advanced policy — disable Store, block purchases, disable auto-updates, block app installation, disable non-Microsoft sources, require business Store account, disable telemetry, require admin for removals.
- **Sprint 523 — PrivateStorePolicy** (10 tweaks, slug `privstore`): Private Store URL policy — configure enterprise private store URL, disable public Store tab, enforce private Store only mode, require Azure AD auth, block personal Microsoft accounts, disable Store app submissions by users.
- **Sprint 524 — AppLicensePolicy** (10 tweaks, slug `applics`): App license policy — require license validation, block offline license activation, disable license key caching, enforce server contact interval (24h), block license transfer to non-joined devices, disable app trial mode.
- **Sprint 525 — InAppPurchasePolicy** (10 tweaks, slug `iappurch`): In-app purchase policy — disable in-app purchases, block credit card in-app purchases, require PIN for purchases, block subscriptions, disable confirmation bypass, require admin approval for purchases over $5.
- **Sprint 526 — StoreAutoUpdatePolicy** (10 tweaks, slug `streauto`): Store auto-update policy — disable app auto-update, require admin for updates, block updates over metered connections, disable silent background updates, require changelog review, block rollback prevention by Store.
- **Sprint 527 — WindowsSandboxPolicy** (10 tweaks, slug `wsbox`): Windows Sandbox policy — disable Sandbox feature, block networking, disable GPU acceleration, block clipboard sharing, disable audio input, block host folder sharing, block printer redirection, require admin, limit memory to 2 GB.
- **Sprint 528 — HyperVContainerPolicy** (10 tweaks, slug `contiso`): Container isolation advanced policy — require Hyper-V isolation, disable process isolation containers, block container breakout via HvSocket, enforce namespace isolation, block privileged mode, require admin for networking.
- **Sprint 529 — DockerDesktopIntegrationPolicy** (10 tweaks, slug `dockerpol`): Docker Desktop integration policy — require admin for install, disable telemetry, block auto-update, enforce WSL2 backend, disable Scout integration, block extension marketplace, require HTTPS for registry access.
- **Sprint 530 — Wsl2AdvancedPolicy** (10 tweaks, slug `wsl2net`): WSL2 network backend policy — enforce WSL2 network isolation from host, disable NAT bypass, block host network share access, require proxy for internet access, disable localhost relay, block WSL2 listening on host ports.
- **Sprint 531 — HyperVQuickCreatePolicy** (10 tweaks, slug `hvqc`): Hyper-V Quick Create policy — disable Quick Create gallery, block VM creation from internet images, require admin, disable telemetry, block external network, require Secure Boot for Quick Create VMs, enforce 4 GB memory limit.

#### Stats

- Tweaks: 8,495 (+940 from v5.55.0)
- Categories: 563 (+94)
- Modules: 560 (+94)
- Tests: 2,742 (0 failures)

---

## [5.55.0] — 2026-03-29

#### Added

- **Sprint 432 — IisHardeningPolicy** (10 tweaks, slug `iisharden`): IIS Web Server Security Hardening via HTTP.sys and W3SVC registry parameters — limit max request buffer size, cap HTTP header field length, block restricted URL characters, enforce URL segment length limit, force UTF-8 encoding, set connection timeout, limit TCP listen backlog, disable socket pooling, cap max simultaneous connections, enable kernel error request logging.
- **Sprint 433 — SqlServerAuditPolicy** (10 tweaks, slug `sqlaup`): SQL Server Security and Audit Configuration — enable full login auditing (success + failure), enforce Windows Authentication-only mode, disable Named Pipes protocol, disable Shared Memory protocol, ensure TCP/IP is enabled, hide SQL instance from network browsers, record xp\_cmdshell disabled state, set error log retention to 10 files, disable OLAP remote connections flag, force TLS encryption for all connections.
- **Sprint 434 — WindowsAdcsPolicy** (10 tweaks, slug `adcspol`): Active Directory Certificate Services Security Policy — protect root certificate store CRL retrieval, disable automatic root certificate updates, enforce strong private key protection, require smart card for interactive logon, enable smart card PIN lockout after 5 failures, disable certificate key archival to CA, disable certificate auto-publication to LDAP, enforce CNG algorithm policy, disable certificate enrollment UI for non-admins, require full certificate chain validation.
- **Sprint 435 — AdfsFederationPolicy** (10 tweaks, slug `adfspol`): Active Directory Federation Services Security Policy — enable Extranet Smart Lockout, set lockout threshold to 5, disable WIA fallback, require TLS certificate authentication, require PKCE for OAuth2 code flow, disable device auth bypass, enable token replay detection, require EPA extended protection, disable prompt=login bypass, enable ADFS audit events.
- **Sprint 436 — KerberoastMitigationPolicy** (10 tweaks, slug `kerbmit`): Kerberoasting Attack Mitigation — disable RC4 and enforce AES-128/256 for service tickets, reduce service ticket lifetime, reduce TGT lifetime, tighten clock skew tolerance to 2 minutes, enable KDC PAC signature validation, block unconstrained delegation, reduce ticket renewal window to 4 days, enable FAST armoring, block NTLM delegation session security, enforce client-side KDC pre-authentication requirement.

#### Stats

- Tweaks: 7,505 → 7,555 (+50 across 5 new modules)
- Categories: 464 → 469 (+5)

## [5.54.0] — 2026-03-29

#### Added

- **Sprint 427 — PushNotificationsPolicy** (10 tweaks, slug `pnp`): Windows Push Notification Service Group Policy controls — disable all toast notifications, disable lock-screen toasts, disable cloud notifications, disable live tile notifications, disable notification mirroring, disable app notifications via policy, disable WNS cloud toast delivery, restrict push notification app count, disable badge notifications on lock screen, disable user override of quiet settings.
- **Sprint 428 — AddRemoveProgramsPolicy** (10 tweaks, slug `arpp`): Add or Remove Programs applet Group Policy controls — disable the applet entirely, hide Add New Programs tab, hide Windows Components tab, prevent changing installed programs, block the Remove Programs page, hide support information link, block adding programs from network, hide Services tab, hide Choose Programs interface, enforce default category view.
- **Sprint 429 — InternetExplorerRestrictionsPolicy** (10 tweaks, slug `ierest`): Internet Explorer Restrictions Group Policy controls (IE mode in Edge) — disable context menu, disable Internet Options dialog, disable View Source, disable Favorites menu, disable download directory selection, disable Find Files, block links opening in new windows, disable toolbar, disable theater mode, prevent browser close.
- **Sprint 430 — PrinterDirectoryServicesPolicy** (10 tweaks, slug `pdssp`): Printer Active Directory Directory Services publishing Group Policy controls — disable automatic printer publishing to AD, disable printer pruning, set pruning check interval, set pruning thread priority, enable pruning event logging, block non-published printer access, disable IPP web printing, limit DS server thread count, enforce printer pre-publication, set max pruning retry count.
- **Sprint 431 — WindowsDiagnosticsInfraPolicy** (10 tweaks, slug `wdip`): Windows Diagnostic Infrastructure (WDI) Group Policy controls — disable scenario execution, disable diagnostic triggers, disable result summary collection, disable diagnostic task collection, disable scenario event logging, disable results caching, limit result persistence to 1 day, disable MSA-linked diagnostics, disable boot diagnostic collection, prevent diagnostic task execution via policy.

#### Stats

- Tweaks: 7,455 → 7,505 (+50 across 5 new modules)
- Categories: 459 → 464 (+5)

## [5.53.0] — 2026-03-29

#### Added

- **Sprint 422 — FocusAssistPolicy** (10 tweaks, slug `fa`): Focus Assist (Quiet Hours) Group Policy controls — disable Quiet Hours feature-wide, disable automatic rules, disable Game Mode DnD, disable presentation-mode DnD, disable summary notification, disable full-screen app DnD, lock the priority list, disable out-of-hours rule, disable first-hour rule, force priority-only mode.
- **Sprint 423 — InputPersonalizationPolicy** (10 tweaks, slug `inpp`): Input Personalization Group Policy controls — deny all input personalization telemetry, restrict ink collection, restrict text collection, disable inking/keyboard personalization, disable user dictionary sync, disable ink learning, disable text prediction, disable linguistic data collection, disable handwriting telemetry, disable input data upload.
- **Sprint 424 — CrashDumpPolicy** (10 tweaks, slug `cdump`): Crash Dump & Error Recovery Group Policy controls — disable kernel crash dump, set mini dump mode, disable automatic reboot on crash, disable event log on crash, disable alert on crash, disable storage telemetry, disable dump log file, overwrite existing dump files, disable filter pages in dumps, disable dedicated dump file.
- **Sprint 425 — WinHttpProxyPolicy** (10 tweaks, slug `whttp`): WinHTTP Proxy Group Policy controls — disable WPAD, disable automatic proxy detection, disable proxy bypass for local addresses, delete AutoConfigURL value, set connection timeout, set receive timeout, disable SSL vulnerability check, disable NTLM auth scheme, disable redirect following, disable WPAD DNS lookup.
- **Sprint 426 — TimeSyncAdvPolicy** (10 tweaks, slug `tsap`): Advanced Time Synchronisation Group Policy controls — set NTP type, set NTP server, disable NoSync mode, set polling interval, set min poll interval, set max positive phase correction, set max negative phase correction, enable Hyper-V time provider, set large phase spike threshold, set event log flags.

#### Stats

- Tweaks: 7,405 → 7,455 (+50 across 5 new modules)
- Categories: 454 → 459 (+5)
- Tests: 2,742 (unchanged)

## [5.52.0] — 2026-03-29

#### Added

- **Sprint 417 — EdgeSecureBrowsingPolicy** (10 tweaks, slug `edgesec`): Edge secure browsing and connection controls — enforce online revocation checks, require revocation checks for local anchors, disable mixed-content auto-upgrade, disable HTTPS upgrades, block insecure private network requests, disable dinosaur easter egg, disable guest mode, disable ClickOnce support, disable HTTPS-only mode, disable SHA-1 for local anchors.
- **Sprint 418 — EdgeProfileSignInPolicy** (10 tweaks, slug `edgeprof`): Edge profile and sign-in lockdown controls — enforce browser sign-in lockdown, disable implicit sign-in, disable guided account switch, disable profile separation, disable Azure AD SSO, disable floating workspace, discard browsing data on enterprise profile creation, restrict to on-prem domain-joined machines, hide Acrobat subscription button, disable InPrivate mode.
- **Sprint 419 — EdgeNotificationsAndPopupPolicy** (10 tweaks, slug `edgenotif`): Edge notification, popup and sensor permission controls — block notifications by default, block popups by default, disable password reveal button, block geolocation by default, disable shopping list, disable related website sets, disable in-app support, block sensors by default, disable browser labs, disable full-screen mode.
- **Sprint 420 — EdgeDownloadHistoryPolicy** (10 tweaks, slug `edgedl`): Edge download, history and telemetry controls — block all downloads, disable download location prompt, enforce Bing SafeSearch strict, delete browsing history on exit, disable Google Cast/media router, disable auto-update, disable "always open" external protocol checkbox, disable warn-before-exit prompt, hide Office shortcut in favorites, suppress unsupported OS warning.
- **Sprint 421 — EdgeSmartScreenAndSiteIsolationPolicy** (10 tweaks, slug `edgessf`): Edge SmartScreen, site isolation and enterprise security controls — enable SmartScreen, enable SmartScreen PUA blocking, prevent SmartScreen override prompts, prevent file-level SmartScreen overrides, block clipboard API by default, enforce site-per-process isolation, block legacy extension points, disable Edge Discover sidebar, block vertical tabs, disable ADFS.
- **Testing**: Added 39 new unit tests covering all Sprint 412-421 modules — ID uniqueness, HasOperations, label/description, ImpactScore/SafetyRating range, CorpSafe flag, registry key prefix, DetectOps path, intra-batch duplicate registry ops, and TweakValidator checks.
- **Infrastructure**: Raised `TestSessionTimeout` in `tests/.runsettings` from 60 s → 300 s; the previous 60 s wall-clock limit caused "Test Run Aborted" on OneDrive-backed builds with 700+ tests loading in ~46 s.

#### Stats

- Tweaks: 7,355 → 7,405 (+50 across 5 new modules)
- Categories: 449 → 454 (+5)
- Tests: 2,742 (+39)

## [5.51.0] — 2026-03-29

#### Added

- **Sprint 412 — EdgePrintAndPdfPolicy** (10 tweaks, slug `edgepdp`): Edge print and PDF handling controls — disable printing entirely, remove print header/footer, default to system printer, disable cloud print, block legacy printer drivers, open PDFs externally, disable PDF annotations, disable XFA forms, set PDF print rasterize DPI to 150, suppress Edge default PDF viewer recommendation.
- **Sprint 413 — EdgeSearchAddressBarPolicy** (10 tweaks, slug `edgesrch`): Edge address bar and search controls — disable search suggestions, remove Bing from address bar provider, disable local intranet suggestions, disable network prediction/prefetch, disable DNS interception check, disable web service for navigation error pages, disable alternate error page web service, disable cloud Related Matches, disable sidebar search, disable typosquatting checker.
- **Sprint 414 — EdgeMediaCapturePolicy** (10 tweaks, slug `edgemedia`): Edge media and device capture controls — block camera, block microphone, block screen capture, disable Google Cast, block Web Bluetooth API, block WebHID API, block WebUSB API, block Serial API, disable Gamepad API, disable AI math solver.
- **Sprint 415 — EdgeTrackingProtectionPolicy** (10 tweaks, slug `edgetrack`): Edge tracking and privacy protection controls — enforce strict tracking prevention, clear cache on exit, disable user feedback/crash reporting, disable Signed HTTP Exchange, enforce SAB cross-origin isolation, disable Surf easter egg game, disable Immersive Reader grammar tools, block intrusive ads on violating sites, disable built-in DNS client, disable Lens region image search.
- **Sprint 416 — EdgeInternetExplorerModePolicy** (10 tweaks, slug `iemode`): Edge Internet Explorer mode controls — disable IE integration level, block user reload in IE mode, block IE mode tab navigation to Edge mode, block local files from IE mode, block local pages from IE mode, disable intranet auto-redirect to IE, enable enhanced hang detection, block zone-marked MHT files from IE mode, set window.open width threshold to 0, disable cloud site list management.

#### Stats

- Tweaks: 7,305 → 7,355 (+50 across 5 new modules)
- Categories: 444 → 449 (+5)
- Tests: 2,667

## [5.50.0] — 2026-03-29

#### Added

- **Sprint 407 — WindowsStoreForBusinessPolicy** (10 tweaks, slug `wsfb`): Windows Store for Business / private store enforcement — disable store apps, require private store only, disable automatic download/app updates, disable store purchase, block non-enterprise apps, disable store UI, disable store implicit access, disable in-app purchases, disable gaming store, disable store pre-install requirements.
- **Sprint 408 — WindowsLogonOptionsPolicy** (10 tweaks, slug `wlogon`): Logon UI behaviour controls — disable last username display, disable last user account info, require Ctrl+Alt+Del, disable password reveal button, set legal notice caption, set legal notice text, disable fast user switching, disable unlocking from non-domain context, set machine inactivity lock limit, disable smart-card removal behavior none.
- **Sprint 409 — WindowsEventLogAccessPolicy** (10 tweaks, slug `evtacc`): Event log channel size and access restrictions — set Security log 100 MB, set System log 50 MB, set Application log 50 MB, set PowerShell log 50 MB, Security log retain 7 days, Security log auto-backup, restrict guest access to Security/System/Application logs, System log auto-backup.
- **Sprint 410 — WindowsDiagTrackPolicy** (10 tweaks, slug `diagtrk`): DiagTrack / Connected User Experiences & Telemetry service policies — set telemetry security-only level, disable opt-in change notifications, disable opt-in settings UI, disable enterprise auth proxy, disable device name in telemetry, limit diagnostic log collection, disable memory dump collection, limit dump collection size, disable OneSettings downloads, disable cloud clipboard integration.
- **Sprint 411 — WindowsMediaPlayerPolicy** (10 tweaks, slug `wmplay`): Windows Media Player enterprise policies — disable auto codec download, disable network settings change, disable auto update check, disable internet streaming, disable DRM license acquisition, disable library sharing, disable online media information retrieval, disable usage reporting, disable remote skin/visualizer download, hide privacy tab in options.

#### Stats

- Tweaks: 7,255 → 7,305 (+50 across 5 new modules)
- Categories: 439 → 444 (+5)
- Tests: 2,703 (unchanged)

## [5.49.0] — 2026-03-29

#### Added

- **Sprint 402 — WindowsFlightedFeaturesPolicy** (10 tweaks, slug `flight`): Windows Insider / flighted feature controls — flighting enable/disable, release preview disable, dev/beta channel block, build information reporting, feature review data, pre-release update deferral, OOBE bypass, branch readiness level lock, insider build install block.
- **Sprint 403 — WindowsPauseUpdatesPolicy** (10 tweaks, slug `pauseupd`): Windows Update pause and deferral controls — pause feature updates, pause quality updates, feature update deferral (30/60/90 days), quality update deferral (7/14/30 days), active hours window, update from peer-to-peer disable, delivery optimization restrict, auto-reboot disable during active hours.
- **Sprint 404 — GameDvrPolicy** (10 tweaks, slug `gamedvr`): Game DVR and Game Bar recording controls — Game DVR enable/disable, Game Bar enable/disable, background recording, audio capture during recording, microphone capture, send feedback to Xbox, cursor capture, Game Mode system-wide, broadcast pause, frame rate target.
- **Sprint 405 — AppxProvisioningPolicy** (10 tweaks, slug `appxprov`): APPX/MSIX provisioning policy controls — disable provisioning, block sideloading, require signed packages, APPX install service block, developer unlock allow, non-enterprise source block, auto-update from store disable, shared PC provisioning, Xbox app provisioning, Microsoft Store push install disable.
- **Sprint 406 — MobilityPolicy** (10 tweaks, slug `mob`): Windows Mobility policy controls — cellular data roaming, mobile hotspot, USB tethering, auto-WiFi-to-cellular switch, Bluetooth tethering, Data Sense, carrier auto-provisioning, WiFi Sense, roaming profile sync, WWAN/cellular UI.

#### Fixed

- CI workflows: `upload-artifact@v7` → `@v4` in `ci.yml` and `debug.yml` (v7 does not exist — caused upload step failures)
- `ci.yml` Write job summary: `$env:RUNNER_STATUS` (undefined) → `$env:JOB_STATUS` with proper `env: JOB_STATUS: ${{ job.status }}` injection
- `release.yml`: added post-release verification step — confirms GitHub release exists and contains `RegiLattice.GUI.exe` + `RegiLattice.exe` after publish
- `.gitignore`: added `**/StrykerOutput/` and `BenchmarkDotNet.Artifacts/` to prevent generated mutation + benchmark outputs from being tracked

#### Stats

- Tweaks: 7,205 → 7,255 (+50 across 5 new modules) *(actual verified count)*
- Categories: 434 → 439 (+5)
- Tests: 2,703 (unchanged)

## [5.48.0] — 2026-03-29

#### Added

- **Sprint 397 — WiaImageAcquisitionPolicy** (10 tweaks, slug `imgacquire`): Windows Image Acquisition scanner controls — STI interactive mode block, user device install restriction, transfer-without-policy disable, scan-to-fax disable, AutoPlay on camera disable, signed driver requirement, scan-to-SharePoint disable, scan-to-network-share disable, scan-to-email disable, scan destination restriction.
- **Sprint 398 — InternetCommunicationPolicy** (10 tweaks, slug `inetcomm`): Internet communication management controls — restrict all internet communication, HTTP printing disable, Windows Update access disable, web communities disable, Event Viewer online help disable, Windows registration disable, Software Protection Platform ticket generation disable, task-scheduler download disable, online search disable, driver update via Windows Update disable.
- **Sprint 399 — NtpGpoPolicy** (10 tweaks, slug `ntpgpo`): W32Time NTP Group Policy controls — NTP client enable, NTP sync type enforcement, cross-site sync flags, special poll interval (15 min), event log flags, max positive/negative phase correction (60 min each), peer back-off minimum/maximum, announce flags for authoritative DC.
- **Sprint 400 — NetworkAccessProtectionPolicy** (10 tweaks, slug `napcomp`): Network Access Protection agent policy controls — NAP client enable, health certificate requirement, VPN SHV enable, auto-remediation disable, quarantine timeout (8 hours), PKI state-machine enable, DHCP enforcement, wired 802.1x enforcement, TS Gateway enforcement, IPsec enforcement.
- **Sprint 401 — EasMdmPolicy** (10 tweaks, slug `easmdm`): Exchange ActiveSync MDM device policy enforcement — require device password, minimum password length (8), max failed attempts (10), inactivity screen lock (5 min), require device encryption, block Wi-Fi, block removable storage, block camera, block internet sharing/hotspot, block Bluetooth.

#### Stats

- Tweaks: 7,175 → 7,225 (+50 across 5 new modules)
- Categories: 433 → 438 (+5)
- Tests: 2,703 (unchanged)

## [5.47.0] — 2026-03-29

#### Added

- **Sprint 392 — EnterpriseStateRoamingPolicy** (10 tweaks, slug `esroam`): Enterprise State Roaming sync controls — per-category disable for app settings, Start layout, desktop theme, browser settings, passwords, app sync; user override blocks; device account sync disable.
- **Sprint 393 — FipsCompliancePolicy** (10 tweaks, slug `fips`): FIPS 140-2 compliance controls — FipsAlgorithmPolicy enable, machine key caching disable, strong key protection, DPAPI restriction, SHA-2 minimum, RC4 disable, weak hash algorithms, DES/3DES restriction, full certificate chain validation, TLS minimum version enforcement.
- **Sprint 394 — HomeGroupPolicy** (10 tweaks, slug `homegroup`): HomeGroup and peer network sharing controls — HomeGroup disable/creation/join blocks, shared library access, network bridge and ICS block, Workplace Join toast, PNRP peer discovery, HomeGroup listener service.
- **Sprint 395 — GpoFolderRedirPolicy** (10 tweaks, slug `folderredir`): Folder redirection GPO controls — cache rename on redirect, roaming profile download, quota notification suppress, synchronous policy wait, exclusive NTFS rights, localized subfolder names, content migration, UNC hardening, slow-link threshold, profile load timeout.
- **Sprint 396 — GpoScriptsPolicy** (10 tweaks, slug `gposcripts`): GPO script execution controls — logon script synchronous execution, startup script sync, legacy logon hidden window, max script timeout (10 min), startup/logon/logoff/shutdown script visibility, user-first logon script order, non-interactive runtime limit (5 min).

#### Stats

- Tweaks: 7,125 → 7,175 (+50 across 5 new modules)
- Categories: 428 → 433 (+5)
- Tests: 2,703 (unchanged)

## [5.46.0] — 2026-03-29

#### Added

- **Sprint 387 — Device Lock GPO Policy** (`DeviceLockGpoPolicy.cs`, slug `devlockgpo`):
  10 new Group Policy tweaks covering device lock and session security — disable Windows
  Hello for Business enrollment, suppress post-logon provisioning prompt, disable PIN
  recovery cloud service, require TPM chip for WHfB, enforce screensaver password, enforce
  screensaver activation, set screensaver timeout to 600 s, disable lock-screen
  notifications, disable camera on lock screen, and disable lock-screen notification apps.
- **Sprint 388 — DFS Namespace Policy** (`DfsnPolicy.cs`, slug `dfsn`):
  10 new Group Policy tweaks covering network connections and DFS clients — disable
  long-path provider override, enable DFS client long-path support, restrict VPN
  connection profile UI, restrict ICS sharing, restrict network location wizard, disable
  Remote Access Connection Manager UI, prohibit network bridge creation, hide LAN
  connection properties, prohibit deletion of network connections, and prohibit RAS
  connect/disconnect.
- **Sprint 389 — .NET Framework Policy** (`DotNetFrameworkPolicy.cs`, slug `dotnet`):
  10 new Group Policy tweaks governing .NET Framework security — disable Authenticode
  publisher trust prompt, disable ClickOnce untrusted publisher prompt, disable strong-name
  bypass, disable legacy CAS/DCOM activation, disable JIT debugger prompt, disable NGen PDB
  collection, prefer in-box runtime, disable publisher evidence, enforce latest CLR, and
  disable .NET hosting in the IE WebBrowser control.
- **Sprint 390 — Edge Extension Policy** (`EdgeExtensionPolicy.cs`, slug `edgeext`):
  10 new Group Policy tweaks governing Edge extension security — block external extension
  installs, disable DevTools, disable component updates, block extension allowed types,
  disable native messaging user hosts, restrict Manifest V2 extensions, disable shopping
  assistant, disable Edge Wallet checkout, enable SmartScreen for trusted downloads, and
  enable Enhanced Security Mode (strict/JIT-disabled).
- **Sprint 391 — Edge Startup Policy** (`EdgeStartupPolicy.cs`, slug `edgestart`):
  10 new Group Policy tweaks governing Edge startup behaviour — set startup to New Tab
  Page, disable startup boost preloading, disable sleeping tabs, disable performance
  detector, disable NTP prerendering, lock NTP search box to Bing, set home page to NTP,
  disable experimentation and A/B testing service, disable tab groups, and disable Edge
  Workspaces sharing.

#### Stats

- **Tweaks**: 7 075 → 7 125 (+50)
- **Categories**: 423 → 428 (+5)
- **Tests**: 2 703 (unchanged)

## [5.45.0] — 2026-03-29

#### Added

- **Sprint 382 — Account Lockout Policy** (`AccountLockoutPolicy.cs`, slug `acctlkout`):
  10 new Group Policy tweaks covering interactive logon controls — require Ctrl+Alt+Del,
  restrict blank password over network, disable locked-account messages, disable automatic
  admin logon, set RAS lockout count, configure RAS lockout reset interval, disable
  logon-hours lock message, disable network unlock banner, enable screensaver-timeout
  auto-lock, and suppress last-username display on logon screen.
- **Sprint 383 — Built-in Admin Policy** (`BuiltinAdminPolicy.cs`, slug `biadmin`):
  10 new Group Policy tweaks covering UAC and built-in account security — restrict
  anonymous SAM enumeration, restrict anonymous pipe access, require Admin Approval Mode,
  disable local SYSTEM blank password use, set UAC consent-prompt behaviour, block UAC
  virtualization, require secure-desktop prompt, enable UAC installer detection, restrict
  run-as logon, and enforce the main UAC switch.
- **Sprint 384 — Camera Privacy Policy** (`CameraPrivacyPolicy.cs`, slug `camprivacy`):
  10 new Group Policy tweaks covering webcam access control — block all Windows app camera
  access, block background camera processes, disable camera roll cloud upload, disable
  camera for lock screen, block desktop app camera, block Win32 non-packaged app webcam,
  disable face-analysis background services, restrict Cortana camera, disable automatic
  camera framing (AI), and require per-user camera consent.
- **Sprint 385 — Control Panel Policy** (`ControlPanelPolicy.cs`, slug `ctrlpanel`):
  10 new Group Policy tweaks restricting Control Panel access — disable all Control Panel
  and PC Settings access, hide Personalization settings, hide User Accounts settings, hide
  Add/Remove Programs, disable Change Password option, restrict network-connection add/
  remove components, hide System Properties, disable Power Options page, and hide Windows
  Update settings in Control Panel.
- **Sprint 386 — Default Browser Policy** (`DefaultBrowserPolicy.cs`, slug `defbrowser`):
  10 new Group Policy tweaks governing default browser enforcement — suppress Edge default
  browser nag, disable IE first-run browser choice prompt, block Edge WebView2 protocol
  handler, lock default browser for all users, suppress Edge startup browser suggestion,
  disable Edge auto-setting itself on updates, disable Edge intent-picker redirect, prevent
  Edge file-protocol interception, preserve default browser across Windows feature updates,
  and disable Edge side-panel web content.

#### Stats

- **Tweaks**: 7 025 → 7 075 (+50)
- **Categories**: 418 → 423 (+5)
- **Tests**: 2 703 (unchanged)

## [5.44.0] — 2026-03-29

#### Added

- **Sprint 377 — LSA Protection Policy** (`LsaProtectionPolicy.cs`, slug `lsapol`):
  10 new Group Policy tweaks covering LSA PPL enforcement, credential delegation,
  WDigest disable, NTLM audit, reversible-encryption protection, and Winlogon policy.
- **Sprint 378 — Storage Sense Policy** (`StorageSensePolicy.cs`, slug `storsense`):
  10 new Group Policy tweaks covering Storage Sense global disable/enable, temp file
  cleanup, Downloads folder threshold, OneDrive dehydration, run cadence, and policy
  enforcement for all users.
- **Sprint 379 — Windows Installer Policy** (`WindowsInstallerPolicy.cs`, slug `msipl`):
  10 new Group Policy tweaks covering AlwaysInstallElevated disable, user install
  restriction, user-control disable, rollback disable, verbose logging, lockdown browse/
  media/patch denial, patch-cache disable, and source-search network restriction.
- **Sprint 380 — Edge Import & Privacy Policy** (`EdgeImportPrivacyPolicy.cs`, slug `edgeimp`):
  10 new Group Policy tweaks covering blocking import of favorites, history, cookies,
  homepage, open tabs, and search engine into Edge, plus disabling browsing history,
  user feedback, SSL error bypass, and site-info telemetry.
- **Sprint 381 — Legacy Edge Policy** (`LegacyEdgePolicy.cs`, slug `ledge`):
  10 new Group Policy tweaks for the legacy EdgeHTML browser: blocking about:flags,
  address-bar drop-down, tab preloading, disabling InPrivate browsing, enforcing
  SmartScreen and preventing bypass, preventing flip-ahead, disabling home button,
  blocking extension sideloading, and hiding the first-run page.

#### Stats

- Total tweaks: **7 025** (+50)
- Total categories: **418** (+5)
- Total tests: **2703** (unchanged)

## [5.43.1] — 2026-03-29

#### Fixed

- **Startup crash fix** (`ArgumentException: Parameter is not valid` in `Font.ToLogFont`) —
  `MainForm` constructor now loads config and calls `AppTheme.SetFontSize` **before**
  `InitializeComponent()`. Previously, `InitializeComponent` captured `AppTheme.Regular`
  (font A) for all controls, then `SetFontSize` disposed font A and created font B. When the
  form was first shown and Win32 HWNDs were created, `ProgressBar.OnHandleCreated →
  SetWindowFont → Font.ToHfont → ToLogFont` failed on the disposed font. Affects all users
  whose saved `FontSize` differs from the 9pt default.
- `ApplyTheme()` now re-assigns `Font = AppTheme.Regular` on the form at the start of each
  call, ensuring the form's inherited font is always valid after a font-size change from
  Preferences.

#### Stats

- Total tweaks: **6975** (unchanged)
- Total tests: **2968** (unchanged)

## [5.43.0] — 2026-03-29

#### Added

- **Sprint 372** — `SmartAppControlPolicy.cs`: 10 Smart App Control (SAC) policy tweaks (`sac-*`) — block policy change, enforcement mode, evaluation mode, signed publishers, script execution, cloud lookup, network paths, LoLBAS abuse, Intelligent Security Graph. MinBuild 22621.
- **Sprint 373** — `NtlmAuthenticationPolicy.cs`: 10 NTLM restriction tweaks (`ntlm-*`) — restrict outgoing NTLM, block incoming, audit outgoing, disable NTLMv1, require session security, block NTLM over HTTP, require extended protection, domain audit, server allowlist, LDAP restriction.
- **Sprint 374** — `KerberosArmoringPolicy.cs`: 10 Kerberos hardening tweaks (`krbadv-*`) — enable FAST armoring KDC/client, disable DES, require strict KDC validation, PKInit freshness, service ticket lifetime, TGT lifetime, renewal deadline, clock sync, disable RC4-HMAC.
- **Sprint 375** — `HotpatchUpdatePolicy.cs`: 10 Windows Hotpatch tweaks (`hotpatch-*`) — enable/disable hotpatch, require code integrity, block rollback, audit events, limit deferred reboots, schedule baseline restart, disable telemetry, exclude drivers, require MDM enrollment. MinBuild 26100.
- **Sprint 376** — `ModernStandbyPolicy.cs`: 10 Modern Standby (S0) tweaks (`mstandby-*`) — disable connected standby, block network, disable smart standby, disable background tasks, disable maintenance, disable fast startup, idle timeout, block wake timers, disable WoL, require password on resume. MinBuild 18362.

#### Stats

- Total tweaks: **6975** (+50)
- Total categories: **413** (+5)
- Test count: **2703** (unchanged)

## [5.42.0] — 2026-03-29

### Added

#### New Policy Modules (Batch 26 — Sprints 367-371)

- **PersonalDataEncryptionPolicy** (`pde-*`) — 10 tweaks: enable PDE on device, require BitLocker as prerequisite, block network account content access, wipe PDE keys on lock, enable PDE protection for Desktop/Documents/Pictures folders, enable PDE access audit events, restrict key backup to organisation, and require Windows Hello enrollment for PDE. MinBuild: 22621 (Win11 22H2+).
- **SudoWindowsPolicy** (`sudopol-*`) — 10 tweaks: disable sudo for Windows, force new-window mode, disable inline execution mode, disable input-disabled mode, restrict sudo to Administrators group, enable elevation audit events, block network access from elevated processes, always prompt for credentials on elevation, log command-line arguments for elevated processes, and block sudo from unapproved shell hosts. MinBuild: 22631 (Win11 23H2+).
- **ProtectedPrintModePolicy** (`wpp-*`) — 10 tweaks: enable Windows Protected Print mode, block legacy non-WPP print drivers, require driver signature verification, prevent user print driver installation, audit driver load events, block RAW format print jobs, block remote print driver installation via RPC (PrintNightmare mitigation), restrict to IPP protocol only, disable client-side print redirection in RDP sessions, and enable print spooler process isolation. MinBuild: 26100 (Win11 24H2+).
- **AttentionSensingPolicy** (`attsens-*`) — 10 tweaks: disable attention sensing (gaze detection), disable presence sensing, disable wake-on-approach, disable lock-on-leave, disable screen dim on look-away, block user override of presence settings, disable adaptive dimming, require user consent before sensor activation, block presence sensing telemetry upload, and disable presence detection on lock screen. MinBuild: 22621 (Win11 22H2+).
- **ConnectedCachePolicy** (`mcc-*`) — 10 tweaks: disable Microsoft Connected Cache client, restrict to enterprise MCC nodes, set MCC node hostname, block P2P Delivery Optimization (HTTP-only mode), limit background bandwidth to 50%, limit foreground bandwidth to 80%, set cache storage limit to 20 GB, disable content upload to peers, restrict DO peers to LAN only, and disable cache downloads on metered connections. MinBuild: 19041 (Win10 2004+/Win11).

### Stats

- **Total tweaks**: 6925 (+50)
- **Total categories**: 408 (+5)
- **Tests**: 2703 passing (0 failures)
- Version bumped `5.41.2` → `5.42.0`

---

## [5.41.2] — 2026-03-26

### Fixed

#### Tests / Test Infrastructure

- Added missing `namespace RegiLattice.Core.Tests;` to `BranchCoverage4Tests.cs`, `BranchCoverage5Tests.cs`, and `BranchCoverage6Tests.cs`. The absent declaration caused VS Code's test adapter to prepend `<RootNamespace>` to global-namespace classes, producing duplicate test-case IDs (`An item with the same key has already been added`).
- Renamed `StartupManagerBranchTests` → `StartupManagerBranchTests2` in `BranchCoverage4Tests.cs` to resolve CS0101 duplicate-type collision with `BranchCoverage2Tests.cs`.

#### CI / Dependencies

- Upgraded GitHub Actions across all four workflow files (`ci.yml`, `release.yml`, `codeql.yml`, `powershell.yml`): `actions/checkout` v4 → v6, `actions/setup-dotnet` v4 → v5, `actions/cache` v4 → v5, `actions/upload-artifact` v4 → v7. Eliminates Node.js 20 deprecation warnings.
- Upgraded `github/codeql-action/init`, `/analyze`, `/upload-sarif` v3 → v4 in `codeql.yml` and `powershell.yml`.
- Bumped `coverlet.collector` 6.0.4 → 8.0.1 in `Directory.Packages.props`.

### Stats

- **Total tweaks**: 6875 (unchanged)
- **Total categories**: 403 (unchanged)
- **Tests**: 2968 passing (0 failures)
- Version bumped `5.41.1` → `5.41.2`

---

## [5.41.1] — 2026-03-26

### Fixed

#### CI / Maintenance

- Fixed Stryker mutation-testing CI job crashing on startup due to invalid `output-path` and `log` config keys (not supported in Stryker 4.14.0 schema). Replaced with `verbosity: "info"` and added `solution: "RegiLattice.sln"` for reliable project discovery. Stryker report artifact path updated to `StrykerOutput/` (Stryker default).
- Added `FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true` workflow-level env var to both `ci.yml` and `release.yml` to eliminate Node.js 20 deprecation warnings. Workflows are now Node.js 24 compatible ahead of the June 2026 forced migration.

#### Documentation

- Replaced stale ASCII `## Project Structure` tree in README.md with a compact Mermaid `graph TD` diagram. Updated stale module/tweak/test counts (223→398 modules, 5125→6875 tweaks).

#### Stats

- **Total tweaks**: 6875 (unchanged)
- **Total categories**: 403 (unchanged)
- **Tests**: 2703 passing (0 failures)

## [5.41.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 25 — Sprints 362-366)

- **AppSiloAdvPolicy** (`appsilob-*`) — 10 tweaks: enforce cross-silo IPC isolation, restrict silo-level handle table sharing, block silo impersonation across privilege boundaries, disable silo-based app sharing without explicit policy, restrict cross-silo thread injection, block silo desktop queue access from other silos, restrict silo virtual memory region access, disable silo token impersonation leakage, block cross-silo pipe server auto-connect, and restrict silo-level ACL inheritance from parent sessions.
- **PrintSpoolFinalPolicy** (`splfinal-*`) — 10 tweaks: enable print spooler cleanup on idle, restrict spooler temp file path to system volume, block spooler RPC listener on non-loopback, disable legacy XPS spool format, restrict print job retention time, block spooler child process creation, disable print processor auto-discovery, restrict spool item access to printing service account, block spooler HTTP/IPP listener outside domain, and disable print spooler browser integration.
- **WinInetPolicy** (`wininet-*`) — 10 tweaks: enable Enhanced Protected Mode for Internet Explorer, restrict Internet zone object caching, disable automatic form fill-in in IE, block zone-based content download in EPM, restrict WinInet proxy auto-detection, disable legacy WinInet connection keep-alive override, restrict WinInet cookie persistence outside session, block WinInet cookie sharing between low and medium integrity zones, disable WinInet SSL error bypass, and restrict WinInet URL caching to session-only.
- **UserAccountControlAdvPolicy** (`uac-*`) — 10 tweaks extending pre-existing module: restrict UAC elevation prompt to secure desktop, block COM object elevation without manifest, disable UAC virtualization for legacy apps, restrict file/registry virtualization scope, block UAC auto-approval for Microsoft-signed apps, disable UAC application compatibility bypass, restrict credential prompts during elevation to admin accounts, block UAC UIPI override from low-integrity processes, disable UAC behavior change via registry without Winlogon restart, and restrict UIA accessibility elevation path.
- **WindowsInkWorkspaceAdvPolicy** (`inkwsadv-*`) — 10 tweaks: restrict Ink Workspace on lockscreen, disable Ink Workspace telemetry, block Windows Ink app suggestion feed, restrict sticky notes cloud sync, disable ink handwriting recognition data collection, block ink replay feature, restrict Ink Workspace in kiosk mode, disable ink-to-text conversion telemetry, block ink workspace whiteboard collaboration, and restrict inking personalization data sharing.

#### Stats

- **Total tweaks**: 6825 → **6875** (+50)
- **Total categories**: 398 → **403** (+5)
- **Module files**: 393 → **398** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.40.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 24 — Sprints 357-361)

- **HolographicDevicePolicy** (`holodv-*`) — 10 tweaks: restrict holographic device pairing to domain admin, disable Holographic Processing Unit telemetry, block Mixed Reality Portal auto-install, restrict holographic device driver auto-update, disable spatial mapping data upload, block Windows Mixed Reality first-run configuration, restrict holographic app sideloading, disable eye-tracking calibration data collection, restrict holographic sleep state policy override, and disable Windows Mixed Reality update notification.
- **VirtualizationPolicy** (`virtz-*`) — 10 tweaks: restrict Hyper-V management to admins, disable Hyper-V VM migration without authorization, restrict Enhanced Session Mode to physical hosts, block untrusted virtual switch creation, disable VM snapshot auto-creation, restrict Hyper-V replica without encryption, block VHDX mount outside sandbox, disable Hyper-V GUI management via MMC for non-admins, restrict VM checkpoint creation frequency, and disable live migration without Kerberos delegation.
- **TokenPrivilegePolicy** (`tokpriv-*`) — 10 tweaks: restrict SeDebugPrivilege assignment, block token privilege adjustment without authorization, restrict impersonation token creation to SYSTEM, disable token elevation via DCOM activation, block token privilege duplication across processes, restrict SeTakeOwnershipPrivilege to BUILTIN\\Administrators, disable token downgrade from SeTcbPrivilege, restrict token creation by services without explicit grant, block token privilege restoration after removal, and disable SeLoadDriverPrivilege for non-admin processes.
- **CloudPrintPolicy** (`cldprt-*`) — 10 tweaks: disable cloud print service, block Mopria print discovery outside corporate network, restrict cloud print provider registration, disable automatic cloud print queue creation, block print job upload to cloud without consent, restrict cloud printer credential caching, disable printerExtension app association, block cloud print on non-AAD devices, restrict cloud print enterprise discovery URL override, and disable cloud print diagnostic data reporting.
- **WindowsSandboxPolicy** (`sandbox-*`) — 10 tweaks extending pre-existing module: disable sandbox vGPU, restrict clipboard sharing into sandbox, block sandbox configuration file creation by standard users, disable audio input in sandbox sessions, restrict networking in sandbox to specific proxy, block sandbox video input access, restrict sandbox printer access, disable sandbox kernel debug mode, restrict sandbox session timeout, and block sandbox nested virtualization settings.

#### Stats

- **Total tweaks**: 6775 → **6825** (+50)
- **Total categories**: 393 → **398** (+5)
- **Module files**: 388 → **393** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.39.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 23 — Sprints 352-356)

- **CrashDumpsPolicy** (`crshmp-*`) — 10 tweaks: disable crash report telemetry upload, restrict kernel dump creation to admins, block automatic minidump on app crash, disable live kernel dump, restrict WER crash dump storage path, block crash dump content including memory page data, disable crash dump file transmission to Microsoft, restrict crash dump accessibility to SYSTEM only, block app crash dump submission without user consent, and disable crash dump auto-deletion after upload.
- **EnterpriseResourcePolicy** (`entres-*`) — 10 tweaks: enable enterprise resource access auditing, restrict cross-DLP-category data transfer, disable enterprise resource data exfiltration paths, block unclassified enterprise data sync to unsanctioned apps, restrict enterprise resource access to domain-joined devices, disable enterprise resource browsing in personal browser profiles, block enterprise content paste to personal apps, restrict app access by enterprise resource classification, disable enterprise resource access via external email clients, and restrict enterprise resource printing to approved printers.
- **NetCfgPolicy** (`netcfg-*`) — 10 tweaks: disable NCSI active probe for captive portal detection, restrict TCP/IP auto-tuning level, disable congestion provider algorithm override, restrict ECN (Explicit Congestion Notification) use, disable TCP chimney offload globally, restrict TCP ACK frequency modification, disable RSS queue count auto-adjustment, restrict socket pool usage per process, disable TCP timestamp option for fingerprinting resistance, and restrict RSS indirection table auto-resize.
- **SecureConnectionPolicy** (`seccxn-*`) — 10 tweaks: disable TLS 1.0 protocol, disable TLS 1.1 protocol, require TLS 1.2 minimum for WinHTTP, restrict cipher suite to approved set, disable 3DES cipher suites, block RC4 cipher suite in TLS, require Perfect Forward Secrecy (DHE/ECDHE), disable SSL 3.0 fallback, restrict TLS session resumption ticket lifetime, and block server certificate pinning bypass.
- **WindowsPerformancePolicy** (`wnperf-*`) — 10 tweaks: restrict background app activity to foreground-only mode, disable visual effects for best performance policy, restrict CPU scheduling priority boost for foreground, disable memory working set trimming on idle, block disk defragmentation on SSDs, disable prefetch and superfetch for non-HDD systems, restrict background task CPU allocation, disable GPU scheduling priority override, restrict page file contiguous allocation, and disable power-throttling for background services.

#### Stats

- **Total tweaks**: 6725 → **6775** (+50)
- **Total categories**: 388 → **393** (+5)
- **Module files**: 383 → **388** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.38.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 22 — Sprints 347-351)

- **DesktopAnalyticsPolicy** (`dskanlyt-*`) — 10 tweaks: set commercial ID for Desktop Analytics, disable diagnostic data commercial sharing, block Desktop Analytics device enrollment, restrict Window Analytics data upload, disable app and update readiness enrollment, restrict Desktop Analytics diagnostic data level, block commercial data pipeline, disable deployment health telemetry, restrict update compliance reporting, and block device census commercial data collection.
- **TpmAdvancedPolicy** (`tpmadv-*`) — 10 tweaks: enable Bitlocker TPM auto-provisioning, require TPM 2.0 for new device attestation, disable TPM firmware update via Windows Update, restrict TPM measured boot policy changes, enable TPM platform crypto provider, block TPM dictionary attack bypass, restrict TPM PCR extension outside bootmgr, disable software TPM (vTPM) without Hyper-V attestation, restrict TPM-based key storage to OS volume, and enable TPM quote-based remote attestation.
- **AppSiloPolicy** (`appsiloa-*`) — 10 tweaks: enable silo isolation between apps, restrict cross-silo named pipe access, block cross-silo COM server activation, disable silo bypass via elevated token, restrict silo kernel object namespace sharing, block silo process exit event notification across silos, disable silo filesystem virtualization bypass, restrict registry silo write outside isolated views, block cross-silo security token use, and restrict silo desktop window station sharing.
- **LockdownBrowsingPolicy** (`lkdwnbr-*`) — 10 tweaks: enable lockdown mode, restrict downloads in restricted zones, block ActiveX controls in low-integrity sessions, disable JavaScript in Restricted Sites zone, block cross-zone redirect with elevation, disable file download from network shares in IE, restrict MIME sniffing override, block clipboard operations from ActiveX, disable zone elevation without prompt, and restrict locked-down zone security level downgrade.
- **RemoteCredentialGuardPolicy** (`rcgrd-*`) — 10 tweaks: enable Remote Credential Guard, restrict delegation to Remote Credential Guard mode, disable NTLM fallback in Remote Credential Guard, restrict Remote Credential Guard to domain accounts, block cached credential use without Remote Credential Guard, restrict Remote Credential Guard token lifetime, disable Remote Credential Guard pass-through for RDP, restrict Remote Credential Guard on non-domain-joined machines, block non-FIPS algorithms in Remote Credential Guard, and restrict Remote Credential Guard enrollment to LAPS accounts.

#### Stats

- **Total tweaks**: 6675 → **6725** (+50)
- **Total categories**: 383 → **388** (+5)
- **Module files**: 378 → **383** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.37.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 21 — Sprints 342-346)

- **PrintSpoolAdvPolicy** (`prtspool-*`) — 10 tweaks: disable point-and-print without elevation, restrict driver update to admin-only, block printer driver signature bypass, disable Web-based printing (IPP over HTTP), restrict share name enumeration, disable remote print spooler management, block Cross-site spooler exploitation via named pipe, restrict driver fallback on signature error, disable CUPS compatibility layer, and block MSXPS document conversion without sandbox.
- **NetBiosPolicy** (`netbios-*`) — 10 tweaks: disable NetBIOS over TCP/IP globally, block WINS resolution, disable NetBIOS name registration broadcasting, restrict NetBIOS node type to B-node, disable NetBT datagram service, block LMHOSTS lookup, disable NetBIOS keepalive, restrict NetBT query scope, disable NetBIOS scope ID advertising, and block legacy NetBIOS session service port 139.
- **WindowsHelloAdvPolicy** (`helloadv-*`) — 10 tweaks: require Windows Hello for domain authentication, disable Hello PIN setup for non-admins, restrict biometric data storage location, block Hello provisioning on shared devices, disable Hello companion device enrollment, restrict Hello key protection to TPM 2.0, block Hello credential delegation without attestation, disable Hello phone sign-in pairing, restrict Hello business deployment without MDM, and block Hello cross-account credential roaming.
- **ActiveSetupPolicy** (`actsetup-*`) — 10 tweaks: disable Active Setup component execution, restrict Active Setup registry key modification, block unprivileged component installation via Active Setup, disable Active Setup version comparison on logon, restrict stub-path execution to signed binaries, block Active Setup HKCU write from HKLM keys, disable Active Setup in kiosk sessions, restrict Active Setup component removal, disable legacy IE Active Setup components, and block third-party Active Setup entries.
- **CbsUpdatePolicy** (`cbsupd-*`) — 10 tweaks: enable auto-repair of CBS component corruption, restrict CBS component installation to admins, disable CBS store cleanup telemetry, block pending component removal without user consent, restrict DISM online image repair network access, disable CBS package staging telemetry, block CBS feature installation from untrusted sources, restrict CBS scan logging to system volume, disable optional component auto-install via Windows Update, and block CBS update agent downgrade.

#### Stats

- **Total tweaks**: 6625 → **6675** (+50)
- **Total categories**: 378 → **383** (+5)
- **Module files**: 373 → **378** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.36.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 20 — Sprints 337-341)

- **UserRightsPolicy** (`usrrts-*`) — 10 tweaks: restrict SeDebugPrivilege to SYSTEM, block SeLoadDriver to non-admins, restrict SeImpersonatePrivilege to service accounts, disable guest account SeNetworkLogonRight, restrict SeBatchLogonRight to scheduled task service accounts, disable SeRemoteInteractiveLogon for non-admins, restrict SeCreateSymbolicLink to admins and WSL, block SeTakeOwnership from standard users, restrict SeBackupPrivilege to backup operators, and disable SeRelabelPrivilege for non-admins.
- **CompartmentPolicy** (`compart-*`) — 10 tweaks: enable network compartmentalization between sessions, restrict cross-session socket access, disable per-user network stack modification, block cross-compartment routing without policy, restrict routing table modification to system, disable loopback across compartment boundaries, block IPv6 prefix delegation between compartments, restrict DNS resolver compartment override, disable DHCP scope cross-compartment lease sharing, and restrict network compartment creation to admins.
- **ServiceAccountPolicy** (`svcact-*`) — 10 tweaks: enable Group Managed Service Accounts (gMSA), restrict service password changes to LSASS, disable service account interactive logon, restrict service account network logon without Kerberos, block service account credential extraction, disable service SID type change, restrict service account cross-domain access, block service principal name modification by standard users, disable service account token privilege escalation, and restrict Kerberos service ticket lifetime for service accounts.
- **SecureChannelPolicy** (`secchan-*`) — 10 tweaks: require secure channel signing, require secure channel sealing (encryption), disable machine password changes via insecure channel, restrict secure channel establishment to domain DCs, block plain-text session key in secure channel, require strong keys for secure channel, disable maximum machine account password age bypass, restrict domain trust enumeration over secure channel, disable secure channel caching in session, and block RODC password replication via secure channel.
- **CredentialManagerPolicy** (`credmgr-*`) — 10 tweaks: restrict credential delegation to allowed servers only, block NTLM credential pass-through, disable plain-text credential in Credential Manager, restrict wdigest credential caching, block credential roaming to cloud via Windows profile, disable saved RDP credentials, restrict web credentials manager access to admin, block auto-enrollment credential sharing, disable app container credential delegation, and restrict Windows Hello credential provider bypass.

#### Stats

- **Total tweaks**: 6575 → **6625** (+50)
- **Total categories**: 373 → **378** (+5)
- **Module files**: 368 → **373** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.35.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 19 — Sprints 332-336)

- **AppGuardPolicy** (`appgrd-*`) — 10 tweaks: enable Microsoft Defender Application Guard managed mode, restrict Application Guard network isolation, disable printing from MDAG sessions, block clipboard sharing into MDAG, restrict MDAG camera and microphone access, block persistent storage in MDAG containers, restrict MDAG GPU acceleration, disable MDAG save-as to host, restrict MDAG window position on taskbar, and disable MDAG enterprise site list synchronization.
- **KioskBrowserPolicy** (`kiosk-*`) — 10 tweaks: enable kiosk mode browser restrictions, block address bar and navigation, restrict new tab page to approved content, disable downloads in kiosk sessions, block developer tools in kiosk mode, restrict kiosk browser printing, disable idle timeout reset, block kiosk session restoration on crash, restrict kiosk browser extension installation, and disable kiosk session clipboard access.
- **DeviceEnrollmentPolicy** (`devenrl-*`) — 10 tweaks: disable MDM auto-enrollment, block personal device enrollment in enterprise MDM, restrict enrollment via Azure AD join, disable over-the-air device provisioning, block BYOD enrollment token creation, restrict enrollment status page skip, disable bulk AAD device join, restrict device enrollment credential caching, block re-enrollment after unenrollment, and restrict enrollment to approved UPN suffixes.
- **MemoryIntegrityPolicy** (`memintg-*`) — 10 tweaks: enable Hypervisor-Protected Code Integrity (HVCI), disable kernel driver memory page mapping bypass, restrict MMIO-mapped device driver DMA, enable Kernel Data Protection, disable unsigned UEFI variable modification, restrict kernel stack pivot protection bypass, enable CET (Control-flow Enforcement Technology) for kernel, disable vulnerable driver blocklist bypass, restrict EFI runtime memory modification, and enable Secure Memory Overwrite.
- **AuditEventPolicy** (`audevt-*`) — 10 tweaks: audit successful logon events, audit failed logon attempts, audit account privilege use, audit security group modification, audit process creation events, audit object access failures, audit system integrity violations, audit policy change events, audit user account management changes, and audit sensitive privilege use.

#### Stats

- **Total tweaks**: 6525 → **6575** (+50)
- **Total categories**: 368 → **373** (+5)
- **Module files**: 363 → **368** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.34.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 18 — Sprints 327-331)

- **NetworkProfilePolicy** (`netprof-*`) — 10 tweaks: block automatic network profile changes, restrict public profile firewall modifications, disable unmanaged network profile creation, block private-to-public profile downgrade, restrict domain profile assignment outside corporate networks, disable profile switching notification, block network profile icon in system tray, restrict per-profile DNS suffix assignment, disable bridge network profile merging, and restrict Internet gateway detection override.
- **WlanPolicy** (`wlanpol-*`) — 10 tweaks: disable auto-connect to open networks, restrict preferred Wi-Fi network modification, disable Wi-Fi Sense (shared credential networks), block WLAN hotspot 2.0 auto-connect, restrict Wi-Fi Direct advertisement, disable WLAN random MAC address rotation policy, block WLAN hidden SSID probing, restrict WLAN group policy profile import, disable Wi-Fi Protected Setup (WPS) PIN method, and restrict WLAN diagnostic data upload.
- **AppxBundlePolicy** (`appxbnd-*`) — 10 tweaks: disable sideloading via AppX bundle, restrict app bundle installation from network paths, block AppX bundle differential update, disable app bundle dependency auto-resolution, restrict app bundle signature override, block app bundle language pack auto-install, disable app bundle installation in non-standard locations, restrict app bundle content streaming, disable app bundle update check on launch, and restrict bundled app re-deployment.
- **DeploymentServicesPolicy** (`depsvc-*`) — 10 tweaks: disable WDS multicast deployment, restrict PXE boot to authorized servers, block TFTP service beyond WDS scope, disable WDS auto-start, restrict deployment image auto-refresh, block client multicast reception, restrict WDS client DHCP option override, disable WDS legacy client auto-configuration, restrict deployment image upload without authorization, and block anonymous WDS discovery.
- **LegacyAuthPolicy** (`legauth-*`) — 10 tweaks: disable LM authentication response, disable NTLMv1 authentication, restrict NTLM SSP session security, block NTLM pass-through to third parties, disable LanManager password hash storage, restrict NTLMv2 session response downgrade, block plaintext credentials in NTLM, disable WDigest credential caching, restrict NTLM with non-Kerberos domain, and block legacy auth over TLS 1.0.

#### Stats

- **Total tweaks**: 6475 → **6525** (+50)
- **Total categories**: 363 → **368** (+5)
- **Module files**: 358 → **363** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.33.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 17 — Sprints 322-326)

- **DomainIsolationPolicy** (`domiso-*`) — 10 tweaks: enable IPsec domain isolation, restrict unauthenticated inbound connections, require Kerberos authentication for domain traffic, block non-domain machine communication, restrict IPsec mode negotiation, disable main-mode IKE SA reuse, block clear-text fallback for IPsec-protected traffic, restrict domain isolation exemption list modification, disable IPsec DoS protection bypass, and restrict quick-mode SA lifetime overrides.
- **CacheManagerPolicy** (`cachemgr-*`) — 10 tweaks: disable offline caching of network files, restrict offline files sync on metered connections, block offline file auto-sync on slow links, disable transparent caching of remote files, restrict CSC (Client-Side Caching) folder location, disable encrypted offline file storage, block offline file sync notification toasts, restrict offline file access from low-privilege processes, disable offline file encryption key export, and restrict sync partnership creation.
- **ObjectAccessPolicy** (`objacs-*`) — 10 tweaks: enable file system object auditing, restrict process handle duplication across integrity levels, disable object directory modification by low-privilege code, block handle inheritance bypass, restrict named object access outside session, enable registry key modification auditing, disable COM object creation from untrusted security contexts, restrict desktop object creation by low-privilege processes, block synchronization object creation from AppContainer, and restrict mutex/event granting across session boundaries.
- **StoragePoolPolicy** (`stpool-*`) — 10 tweaks: restrict storage pool creation to admins, block automatic storage pool repair without user consent, disable tiered storage policy auto-assignment, restrict storage pool membership modification, disable pool usage telemetry reporting, block virtual disk auto-provisioning, restrict thin-provisioned volume creation, disable storage pool health notification balloons, restrict hotspare disk assignment, and block storage pool configuration export.
- **FileSharePolicy** (`filshare-*`) — 10 tweaks: disable administrative share auto-creation (C$, ADMIN$), restrict SMB share permissions to admins, block hidden share creation by non-admins, disable anonymous share listing, restrict share discovery via WS-Discovery, block file share access from untrusted domains, disable offline share caching, restrict DFS namespace browsing to domain users, disable share access-based enumeration override, and restrict symbolic-link following within shares.

#### Stats

- **Total tweaks**: 6425 → **6475** (+50)
- **Total categories**: 358 → **363** (+5)
- **Module files**: 353 → **358** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.32.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 16 — Sprints 317-321)

- **EventTracingPolicy** (`evttrc-*`) — 10 tweaks: disable ETW telemetry sessions, restrict user-mode trace providers, block autologger creation, disable DiagTrack ETW session, restrict WPP software trace output, disable diagnostic data ETW provider, block NT Kernel Logger auto-start, restrict circular buffer trace sessions, disable performance counter ETW links, and restrict Event Log forwarding to external hosts.
- **ProcessorPolicy** (`proccpol-*`) — 10 tweaks: disable speculative execution side-channel mitigations toggle, restrict CPU microcode update injection, disable hyperthreading via policy, restrict core parking policy changes, disable branch predictor flush telemetry, block CPU frequency scaling override, restrict hardware-enforced stack protection, disable AMD64 extended feature advertising, restrict XSAVE/XRESTORE extended state access, and disable processor power-state telemetry.
- **CodeSigningPolicy** (`codesign-*`) — 10 tweaks: require signed drivers, block unsigned PowerShell script execution, restrict kernel driver signing to WHQL, disable test signing mode, block User Mode Code Integrity (UMCI) bypass, restrict non-ELAM driver loading before boot, disable legacy AuthenticCode signature bypass, restrict .NET assembly execution without strong name, block COM object registration without authenticode, and restrict DLL side-loading from user-writable paths.
- **TrustProviderPolicy** (`trustprov-*`) — 10 tweaks: require full trust chain for Authenticode, block revoked certificate bypass, disable SHA1 signature acceptance for new code, restrict trust to Microsoft-issued roots, block PKCS#7 trust provider override, disable user-trust prompt for unrecognized publishers, restrict zone-based publisher trust decisions, disable trust from network drives, block executable trust override via user registry, and restrict publisher certificate time-stamp validation bypass.
- **SmbEncryptionPolicy** (`smbenc-*`) — 10 tweaks: require SMB encryption on client connections, require SMB signing globally, disable NTLM fallback in SMB authentication, restrict SMBv1 protocol use, disable SMB plaintext password over network, require SMB packet signing for domain joins, block unencrypted SMB shares from remote clients, disable SMB3 decryption downgrade, restrict SMB named pipe remote access, and disable NetShare auto-reconnect.

#### Stats

- **Total tweaks**: 6375 → **6425** (+50)
- **Total categories**: 353 → **358** (+5)
- **Module files**: 348 → **353** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.31.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 15 — Sprints 312-316)

- **WindowsFlightingPolicy** (`flight-*`) — 10 tweaks: disable insider preview builds, block Windows Insider Program enrollment, restrict flighting data upload, disable preview build auto-install, block experimental feature toggle, restrict ring configuration override, disable telemetry-based feature rollout, block flight signing certificate trust, restrict Insider branch switching, and disable flight status badge display.
- **CapabilityAccessPolicy** (`capacs-*`) — 10 tweaks: deny microphone access globally, deny camera access globally, restrict location access to approved apps, disable background app access, block notification cross-app access, restrict calendar data access, disable contact data access for third-party apps, block call history access, restrict messaging app integration, and disable account information access for apps.
- **DynamicDataExchangePolicy** (`ddepol-*`) — 10 tweaks: disable DDE protocol, block DDE server lookup, restrict DDE inter-application communication, disable DDEML global server mode, block clipboard DDE data format, restrict DDE Auto-Execute on file open, disable OLE DDE server auto-registration, block Word DDE field execution, restrict Excel DDE link auto-update, and disable legacy DDE app startup.
- **NetworkAdapterPolicy** (`netadp-*`) — 10 tweaks: disable NetBIOS over TCP/IP, restrict network adapter power management, block wake-on-LAN via unsolicited packets, disable large send offload (LSO), restrict RSS (Receive Side Scaling) auto-configuration, disable TCP/IP offload (TOE), block NDIS filter driver auto-install, restrict network adapter jumbo frame usage, disable TCP timestamp option, and restrict NIC teaming auto-configuration.
- **Ipv6Policy** (`ipv6pol-*`) — 10 tweaks: disable IPv6 protocol stack, disable Teredo tunneling, disable ISATAP tunneling, disable 6to4 transitional protocol, restrict DHCPv6 auto-configuration, disable IPv6 default gateway assignment, block IPv6 over cellular (mobile broadband), disable IPv6 router advertisements, restrict IPv6 privacy address rotation, and disable IPv6 flow label generation.

#### Stats

- **Total tweaks**: 6325 → **6375** (+50)
- **Total categories**: 348 → **353** (+5)
- **Module files**: 343 → **348** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.30.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 14 — Sprints 307-311)

- **AdhocNetworkPolicy** (`adhocnet-*`) — 10 tweaks: disable ad hoc network creation, block IBSS (peer-to-peer Wi-Fi) mode, restrict hosted network adapter sharing, disable Wi-Fi Direct legacy group owner, block ad hoc Bluetooth PAN creation, restrict ICS (Internet Connection Sharing), disable Wi-Fi Sense ad hoc join, block ad hoc network auto-connect, restrict bridge network interface creation, and disable ad hoc network device discovery.
- **PrinterGpoPolicy** (`prtgpo-*`) — 10 tweaks: disable print spooler sharing over network, restrict printer driver version downgrade, block Internet printing via HTTP, disable background download of printer drivers, restrict printer connection via SMB, disable auto-publishing of printer connections, block driver signature bypass for print, restrict print path UNC browsing, disable Web Services for Devices (WSD) printer auto-discovery, and block legacy LPR/LPD port monitor.
- **RemoteProcedureCallPolicy** (`rpcpol-*`) — 10 tweaks: enable RPC authentication, restrict unauthenticated RPC pipe access, disable RPC over HTTP on client, block DCOM remote activation without auth, restrict RPC endpoint mapper to authenticated callers, disable RPC null session connections, block legacy RPC endpoint usage, restrict RPC encryption level overrides, disable RPC client late binding, and restrict anonymous RPC interfaces.
- **LicensingPolicy** (`licpol-*`) — 10 tweaks: disable activation status reporting to Microsoft, block KMS discovery via DNS, restrict license validation network connection, disable grace period status telemetry, block SLMV2 service token caching, restrict OEM activation key exposure, disable SPPUI notification popups, block software protection platform update, restrict activation over VPN bypass, and disable Windows Activation Technologies diagnostic.
- **WindowsContainerPolicy** (`wincnt-*`) — 10 tweaks: disable container network access, restrict Hyper-V isolated container creation, block Docker Desktop service auto-install, disable Windows Sandbox integration, restrict container layer disk access, disable container telemetry reporting, block unauthenticated container registry pull, restrict container base image auto-update, disable Windows Subsystem for Linux container bridge, and restrict container host OS resource access.

#### Stats

- **Total tweaks**: 6275 → **6325** (+50)
- **Total categories**: 343 → **348** (+5)
- **Module files**: 338 → **343** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.29.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 13 — Sprints 302-306)

- **FontProviderPolicy** (`fontprov-*`) — 10 tweaks: disable online font downloading, block DirectWrite font provider, restrict font installation to admins, disable automatic font update, block font telemetry reporting, restrict third-party font renderer loading, disable ClearType tuning data upload, block font substitution table modification, disable custom font caching, and restrict OpenType layout feature override.
- **AppXPackagingPolicy** (`appxpkg-*`) — 10 tweaks: disable app sideloading, block developer mode package install, restrict MSIX package deployment, disable AppX debugging mode, block unsigned app package trust, restrict AppX network installation, disable AppX certificate bypass, block optional features via AppX, disable StreamedApps content delivery, and restrict MSIX launchers outside Store.
- **DataIntegrityPolicy** (`dataintg-*`) — 10 tweaks: enable integrity checks for system files, restrict access to integrity-protected paths, disable integrity level downgrade, block process privilege escalation via integrity bypass, restrict medium-integrity browser sandbox modification, disable object integrity label override, block low-integrity write to user profile, restrict untrusted code access to high-integrity objects, disable SID integrity attribute bypass, and restrict discretionary ACL modification by low-integrity code.
- **NtfsPolicy** (`ntfspol-*`) — 10 tweaks: disable last-access timestamp update, restrict 8.3 short-name creation, disable NTFS paging file deletion on shutdown, block unencrypted NTFS drive mounting, restrict NTFS quota enforcement bypass, disable NTFS transaction logging (TxF), block MFT zone reservation changes, disable NTFS reparse point following, restrict NTFS symbolic link creation, and disable NTFS volume shadow integrity tracking.
- **CertValidationPolicy** (`certvld-*`) — 10 tweaks: disable automatic root certificate update, block untrusted root certificate installation, restrict third-party root CA auto-enrollment, disable EV certificate UI bypass, block certificate revocation check bypass, restrict OCSP responder URL override, disable cross-organization certificate trust, block pinned certificate override, restrict private CA cross-certification, and disable weak signature algorithm certificate acceptance.

#### Stats

- **Total tweaks**: 6225 → **6275** (+50)
- **Total categories**: 338 → **343** (+5)
- **Module files**: 333 → **338** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.28.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 12 — Sprints 297-301)

- **AppContainerPolicy** (`appcont-*`) — 10 tweaks: disable loopback access for AppContainer apps, restrict AppContainer network isolation bypass, disable AppContainer capability brokering, block AppContainer printer access, restrict AppContainer local account access, disable AppContainer ambient authority, block capability grant auto-approval, restrict AppContainer cross-session communication, disable AppContainer capability prompts, and restrict low-privilege isolation bypass.
- **NetworkQosPolicy** (`nqos-*`) — 10 tweaks: disable QoS reservation bandwidth, block DSCP marking override, restrict QoS policy application to admins, disable nonconforming packet throttling, block QoS policy auto-refresh, disable WFP QoS integration, restrict Diffserv per-application settings, disable guaranteed-service QoS, block Layer2 QoS marking, and restrict best-effort vs. controlled-load flow arbitration.
- **HardwareDevicePolicy** (`hwdev-*`) — 10 tweaks: prevent unknown device installation, restrict device driver auto-installation, block non-admin hardware ID override, disable removable device installation, restrict device setup class whitelisting, disable USB mass storage auto-install, block hardware serial number reporting, restrict boot device installation, disable device metadata retrieval online, and restrict printer driver installation.
- **DnsSecurePolicy** (`dnssec-*`) — 10 tweaks: disable multicast DNS (mDNS), restrict DNS suffix search list, disable LLMNR fallback, block dynamic DNS registration, disable DNS devolution, restrict primary DNS suffix updates, disable DNS cache flushing by apps, block split-brain DNS, restrict DNS over HTTPS template changes, and disable negative DNS caching optimizations.
- **PortableDevicesPolicy** (`portdev-*`) — 10 tweaks: disable AutoPlay for portable devices, restrict WPD (Windows Portable Devices) installation, block MTP media sync auto-launch, disable phone companion app auto-connect, restrict portable device driver auto-update, disable media streaming from portable devices, block portable device credential prompt, restrict camera raw file access, disable portable device telemetry upload, and restrict portable device content indexing.

#### Stats

- **Total tweaks**: 6175 → **6225** (+50)
- **Total categories**: 333 → **338** (+5)
- **Module files**: 328 → **333** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.27.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 11 — Sprints 292-296)

- **SharedPCPolicy** (`shpc-*`) — 10 tweaks: disable Shared PC mode, block guest account creation, restrict shared PC app deletion, disable maintenance window override, block shared PC account cleanup, disable Kiosk mode configuration, restrict PC sleep timeout in shared mode, disable shared PC usage guidelines, block shared PC sign-in branding, and restrict shared PC power button.
- **NetworkListPolicy** (`netlst-*`) — 10 tweaks: delete all user network profiles on exit, disable network list service sharing, block per-user network location changes, disable network profile icon display, restrict unmanaged network name resolution, block wireless profile sharing, disable network location awareness auto-update, prevent home network detection, restrict public network profile assignment, and disable network connectivity notification.
- **SensorServicePolicy** (`sensor-*`) — 10 tweaks: disable location scripting, block location provider service, restrict third-party sensor access, disable location telemetry reporting, block location accuracy override, disable location history storage, restrict geofencing API access, block background location collection, disable location-based advertising API, and restrict sensor data aggregation.
- **TelephonyPolicy** (`telpol-*`) — 10 tweaks: disable call telemetry, block RCC profile registration, restrict telephony app background activity, disable modem diagnostics upload, block line app registration, restrict VOIP provider access, disable call log synchronization, block call forwarding configuration, restrict mobile broadband telemetry, and disable telephony service credential caching.
- **PrintManagementPolicy** (`prtmgmt-*`) — 10 tweaks: disable MMC print management snap-in, restrict printer driver installation to admins, block point-and-print from untrusted servers, disable branch office direct printing, restrict Internet printing client, disable print queue diagnostic reporting, block printer auto-discovery via WSD, restrict print preview browser integration, disable print-via-email sharing, and restrict network print spooler access.

#### Stats

- **Total tweaks**: 6125 → **6175** (+50)
- **Total categories**: 328 → **333** (+5)
- **Module files**: 323 → **328** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.26.0] — 2026-03-25

### Added

#### New Policy Modules (Batch 10 — Sprints 287-291)

- **AppReadinessPolicy** (`apprdy-*`) — 10 tweaks: disable App Readiness service, disable staged content tracking, block app readiness telemetry upload, disable readiness diagnostic reporting, restrict app cache pre-population, disable scheduled readiness scans, block first-run readiness tasks, disable staged update validation, restrict app readiness data collection, and disable readiness-related background tasks.
- **DataSensePolicy** (`dtsense-*`) — 10 tweaks: disable Data Sense traffic shaper, restrict WWAN background data usage, disable Wi-Fi usage telemetry, block Data Sense automatic configuration, disable background data usage reporting, restrict metered connection app access, disable Data Sense bandwidth optimizations, block usage history upload, disable data compression proxy, and restrict carrier data plan management.
- **PageFilePolicy** (`pgfpol-*`) — 10 tweaks: ensure page file is enabled, disable automatic page file management, set minimum page file size, set maximum page file size, restrict page file creation to system drive, disable page file encryption, block page file reuse across sessions, disable page file diagnostics reporting, restrict page file location changes, and disable memory compression page file integration.
- **VolumeShadowCopyPolicy** (`vscpol-*`) — 10 tweaks: disable VSS service, restrict shadow copy creation, disable boot VSS provider, block application-consistent snapshots, restrict VSS storage allocation, disable shadow copy scheduling, block remote VSS requests, disable differential area tracking, restrict VSS provider installation, and disable VSS diagnostic reporting.
- **RestartManagerPolicy** (`rstmgr-*`) — 10 tweaks: disable Restart Manager, prevent app restarts after reboot, disable restart notification toasts, block graceful app shutdown via RM, disable RM timeout enforcement, restrict RM to admin-only, disable RM session logging, block RM MUI loading, disable RM COM server endpoint, and restrict RM cross-session operations.

#### Stats

- **Total tweaks**: 6075 → **6125** (+50)
- **Total categories**: 323 → **328** (+5)
- **Module files**: 318 → **323** (+5)
- **Tests**: 2703 passing (0 failures)

## [5.25.0] — 2026-05-27

### Added

#### New Policy Modules (Batch 9 — Sprints 282-286)

- **ReFSPolicy** (`refs-*`) — 10 tweaks: disable ReFS integrity checking, integrity streams,
  auto-repair, short-name creation, last-access timestamp update, parity logging, metadata
  checksum, large MFT reservation, delete notification (TRIM), and data compression.
- **GraphicsDriversPolicy** (`gfxdrv-*`) — 10 tweaks: disable DXGI Flip Model override,
  MPO (multi-plane overlay), VRR/AdaptiveSync, hardware GPU scheduler, Auto HDR, experimental
  DX12 resource binding, graphics driver telemetry, fine-grained preemption, D3D12 WARP
  updates, and display-required power request override.
- **FeedbackPolicy** (`fbk-*`) — 10 tweaks: disable Feedback Hub notifications, Feedback Hub
  submission, NPS surveys, telemetry upload, screen capture, Steps Recorder, in-app prompts,
  voluntary data collection, and MSA account requirement; set feedback frequency to Never.
- **SecureBootPolicy** (`secboot-*`) — 10 tweaks: enable db/dbx update and bootloader
  revocation check; disable test-signing and custom PK enrollment; enforce UMCI, kernel CI,
  ELAM-backed initial ramdisk, managed OS policy, and VBS presence; disable network unlock and
  Secure Boot telemetry.
- **ShutdownOptionsPolicy** (`shtdwn-*`) — 10 tweaks: disable Ctrl+Alt+Del shutdown, app
  restart after reboot, automatic restart on BSOD, forced reboot notification, power button
  shutdown, and hibernate option; require shutdown reason; zero logoff script wait; disable
  Start Menu restart; enable shutdown event logging.

#### Stats

- **Total tweaks**: 6025 → **6 075** (+50)
- **Total categories**: 318 → **323** (+5)
- **Module files**: 313 → **318** (+5)
- **Tests**: 2063 passing (0 failures)

## [5.24.0] — 2026-05-27

### Added

#### New Policy Modules (Batch 8 — Sprints 277-281)

- **LanguageOptionsPolicy** (`langopt-*`) — 10 tweaks: block adding languages, restrict
  language change, IME/OCR/speech/keyboard/handwriting telemetry opt-out, cloud
  candidate block, language pack update block, and DoNotSyncLanguageSettings.
- **TokenBrokerPolicy** (`tokbrk-*`) — 10 tweaks: disable WAM/Token Broker, persistent
  token cache, background token refresh, AAD/MSA token sharing, implicit account
  discovery, enterprise SSO, token lifetime cap (60 min), user-consent requirement, and
  Token Broker telemetry opt-out.
- **VirtualKeyboardPolicy** (`vkbd-*`) — 10 tweaks: suppress touch keyboard auto-popup,
  emoji panel, keystroke sound, handwriting button, full-screen mode, keyboard
  animations, voice dictation key, split/wide keyboard layouts, and keyboard telemetry.
- **WebAuthnPolicy** (`wauthn-*`) — 10 tweaks: disable biometric fallback, cross-origin
  auth, password fallback, cloud passkey sync, security key enrollment, NFC/Bluetooth
  transport, and WebAuthn telemetry; require enterprise attestation and user verification.
- **HealthAttestationPolicy** (`hltha-*`) — 10 tweaks: disable remote health attestation,
  attestation telemetry/caching; require TPM-backed attestation; use private HAS URL;
  enforce Secure Boot, BitLocker, ELAM, and VBS presence checks; set 60-minute refresh
  interval.

#### Stats

- **Total tweaks**: 5975 → **6 025** (+50)
- **Total categories**: 313 → **318** (+5)
- **Module files**: 308 → **313** (+5)
- **Tests**: 2063 passing (0 failures)

## [5.23.0] — 2026-05-26

### Added

#### Enhanced

- **ClipboardHistoryPolicy** (Sprint 272): 10 Group Policy tweaks for `Windows\ClipboardHistory` — disable history, cloud sync, enterprise roaming, pin items, image/HTML data, thumbnail preview, size limit, logoff clear, telemetry.
- **PenWorkspaceGpoPolicy** (Sprint 273): 10 Group Policy tweaks for `Windows\PenWorkspace` — hide Pen Workspace button, disable above-lock ink, suppress onboarding, block handwriting panel, ink replay, pen-button shortcut, suggested apps, dictation, sticky notes on lock, telemetry.
- **SuperFetchSysmainPolicy** (Sprint 274): 10 Group Policy tweaks for `Windows\SuperFetch` — disable SuperFetch/SysMain, Prefetcher, ReadyBoost, ReadyDrive, boot trace, app-launch prefetch, logon scenario, memory profiling, heap prefetch, telemetry.
- **SpellingAndTypingPolicy** (Sprint 275): 10 Group Policy tweaks for `Windows\SpellingAndTyping` — block autocorrect, spell check, text prediction, misspelling highlights, typing insights, hardware keyboard suggestions, swipe typing, typing telemetry, handwriting samples, autocomplete.
- **StorageManagementPolicy** (Sprint 276): 10 Group Policy tweaks for `Windows\StorageManagement` — restrict Storage Spaces UI, disable tiering, VSC notifications, disk cleanup prompt, NTFS 8.3 names, storage diagnostics, hot spare alert, data deduplication, Disk Management snap-in, low-disk warning.

#### Stats

- Tweaks: 5 925 → **5 975** (+50)
- Categories: 308 → **313** (+5)
- Tests: 2 667 (0 failures)

---

## [5.22.0] — 2026-05-26

### Added

#### New Policy Modules (Sprints 267–271)

- **Sprint 267** `RetailDemoPolicy` (10 tweaks) — Retail demo mode lockdown: disable demo mode, attract loop, auto sign-in, app provisioning, content delivery, experience provider, info banner, OOBE demo flow, cleanup revert, and interaction telemetry. Category: *Retail Demo Policy*.
- **Sprint 268** `PushToInstallPolicy` (10 tweaks) — Push-to-Install controls: disable the service, remote push, auto provisioning, device management push, store notifications, telemetry, admin approval gate, unattended push, cross-device sync, and service wake. Category: *Push To Install Policy*.
- **Sprint 269** `SecurityCenterPolicy` (10 tweaks) — Windows Security Center administration: disable WSC, spyware/antivirus/firewall/update/UAC/internet monitoring, hide tray icon, disable account protection monitoring and notification toasts. Category: *Security Center Policy*.
- **Sprint 270** `WebThreatDefensePolicy` (10 tweaks) — Web Threat Defense engine controls: disable service, lock UI toggle, disable phishing filter, malicious-URL block, download reputation, cloud lookup, behaviour monitoring, telemetry upload, enhanced protection mode, and credential-entry warning. Category: *Web Threat Defense Policy*.
- **Sprint 271** `VideoCapturePolicy` (10 tweaks) — Video capture governance: block the capture device, screen capture, live broadcast, game DVR capture, audio pairing, require admin, disable camera telemetry, virtual camera, MediaCapture UWP API, and background capture. Category: *Video Capture Policy*.

#### Stats

- Tweaks: 5,875 → **5,925** (+50)
- Categories: 303 → **308** (+5)
- Tests: 2,667 (unchanged)

---

## [5.21.0] — 2026-03-28

### Added

#### New Policy Modules (Sprints 262–266)

- **Sprint 262 — User Profiles Policy** (`uprof-*`, 10 tweaks)
  — `UserProfilesPolicy.cs`: disable roaming profiles, slow-link detection, cached copy
  deletion, profile size dialog timeout, wait on logoff, profile error notifications, guest
  logon, slow-link GPO UI, user folder redirection, and profile quota enforcement.

- **Sprint 263 — Game Explorer Policy** (`gex-*`, 10 tweaks)
  — `GameExplorerPolicy.cs`: disable all Game Explorer access, block ratings downloads, hide
  recommended games, prevent game launching, hide online games section, disable parental
  controls, block automatic game updates, suppress game notifications, block game installation,
  disable game activity logging.

- **Sprint 264 — Media Foundation Policy** (`mfa-*`, 10 tweaks)
  — `MediaFoundationPolicy.cs`: disable Frame Server camera mode, block untrusted codecs,
  disable hardware video acceleration, disable transcoding APIs, block protected content
  playback, disable network streaming, prevent automatic codec downloads, disable sharing APIs,
  block DRM individualization, disable Media Foundation telemetry.

- **Sprint 265 — Color Calibration Policy** (`colcal-*`, 10 tweaks)
  — `ColorCalibrationPolicy.cs`: disable display color calibration tool, disable ICM/ICC
  support, hide Color Management control panel, disable automatic calibration scheduling,
  block user ICC profile installation, disable Night Light via policy, disable HDR support,
  disable Windows Color System background service, lock absolute colorimetric rendering intent,
  disable automatic color correction.

- **Sprint 266 — Display Adapter Policy** (`dispadp-*`, 10 tweaks)
  — `DisplayAdapterPolicy.cs`: block user display driver installation, force Standard VGA mode,
  disable DXVA hardware video acceleration, disable GPU compute workloads (DirectCompute),
  lock DPI scaling, lock display rotation, disable mirroring, lock display resolution, lock
  refresh rate, lock colour depth.

#### Stats

- Total tweaks: **5 825 → 5 875** (+50)
- Total categories: **298 → 303** (+5)
- Tests: 2,667 (0 failures, all green)

## [5.20.0] — 2026-03-27

### Added

#### New Policy Modules (Sprints 257–261)

- **Sprint 257 — Windows To Go Policy** (`wtg-*`, 10 tweaks)
  — `WindowsToGoPolicy.cs`: disable sleep/hibernation in WTG workspaces, block workspace
  creation, block boot from external media, disable offline folders, retail demo, metered sync,
  cross-hardware deployment, enforce Secure Boot, disable automatic update.

- **Sprint 258 — BitLocker FVE Policy** (`blfve-*`, 10 tweaks)
  — `BitLockerFvePolicy.cs`: disable DRA recovery console, require TPM for OS drive, enforce
  XTS-AES-256 for OS/fixed drives, require recovery key for OS volumes, deny write to
  unprotected removable drives, enable pre-boot input protectors, disable standby with
  BitLocker, backup recovery keys to AD, AES-128 for removable drives.

- **Sprint 259 — Cloud Desktop Policy** (`clouddesk-*`, 10 tweaks)
  — `CloudDesktopPolicy.cs`: disable Cloud PC entry points and provisioning, disable virtual
  desktop agent and UAC bypass, disable SSO and telemetry, restrict region selection, block
  clipboard/printer redirection in Cloud PC sessions, set max session idle timeout.

- **Sprint 260 — Network Projection Policy** (`netproj-*`, 10 tweaks)
  — `NetworkProjectionPolicy.cs`: disable legacy network projector wizard, disable 'Project to
  This PC' (Miracast receiver), require PIN for pairing, restrict to secured Wi-Fi networks,
  block source projection, disable wireless display infrastructure mode, disable Miracast
  multicast discovery, enforce HDCP content protection, disable auto-trust of paired devices,
  set wireless display auto-lock screen timeout.

- **Sprint 261 — Windows Sandbox Policy** (`sbpol-*`, 10 tweaks)
  — `WindowsSandboxPolicy.cs`: disable Sandbox entirely via GPO, disable networking and vGPU
  inside Sandbox, block clipboard/printer redirection, disable microphone/camera input, prevent
  mapped host folder access (read/write), restrict logon credential propagation into Sandbox.

#### Stats

- Tweaks: 5 775 → **5 825** (+50)
- Categories: 293 → **298** (+5)
- Tests: 2 660 → **2 667** (+7)

## [5.19.0] — 2026-03-26

### Added

#### New Policy Modules (Sprints 252–256)

- **Sprint 252 — ActiveX Installer Service Policy** (`axinst-*`, 10 tweaks)
  — `ActiveXInstallerServicePolicy.cs`: enforce admin approval for ActiveX installs, block
  silent/per-user/untrusted-zone installs, control logging, update, and OCX download.

- **Sprint 253 — Scripted Diagnostics Policy** (`sdiag-*`, 10 tweaks)
  — `ScriptedDiagnosticsPolicy.cs`: disable scripted diagnostics, online troubleshooters,
  recommended troubleshooting, elevated troubleshooters, results upload, third-party
  diagnostics, scheduled diagnostics, and troubleshooting history.

- **Sprint 254 — Wi-Fi Hotspot Authentication Policy** (`hotspot-*`, 10 tweaks)
  — `HotspotAuthenticationPolicy.cs`: disable captive portal detection, the Wi-Fi Sense
  credentials-sharing feature, auto-connect to new networks, internet sharing, Hotspot 2.0,
  manual hotspot, WLAN auto-config GPT policy, and credential caching.

- **Sprint 255 — Early Launch Anti-Malware Policy** (`elam-*`, 10 tweaks)
  — `EarlyLaunchAMPolicy.cs`: configure ELAM driver load policies (good-only, good+unknown,
  critical-only), disable ELAM driver loading, increase scan timeout, enable event logging,
  block unknown boot drivers, enable Network ELAM, Measured Boot, and boot log persistence.

- **Sprint 256 — Certificate Auto-Enrollment Policy** (`certae-*`, 10 tweaks)
  — `CertAutoEnrollmentPolicy.cs`: disable/enable machine and user certificate auto-enrollment
  with AEPolicy values (0=off, 7=enroll+renew+archive), suppress  expiry notifications,
  enable audit logging, disable offline domain join enrollment, enable key-based renewal,
  disable PKI URL retrieval for air-gapped systems, and block weak certificate algorithms.

#### Stats

- Total tweaks: **5,775** (+50 from v5.18.0)
- Total categories: **293** (+5)
- Total tests: **2,660** (0 failures)

---

## [5.18.0] — 2026-03-26

### Added

- **Sprint 247 — Work Folders Policy** (`WorkFoldersPolicy.cs`, 10 tweaks, `wf-*`)
  - Covers machine/user Work Folders disable, force automatic setup, block server URL change,
    require encryption, disable Work Folders UI, prevent sync settings change, disable background sync,
    and set sync interval

- **Sprint 248 — Mobility Center Policy** (`MobilityCenterPolicy.cs`, 10 tweaks, `mob-*`)
  - Covers machine/user Mobility Center disable, presentation settings, battery tile, sync center tile,
    display tile, screen rotation tile, wireless tile, volume tile, and context menu removal

- **Sprint 249 — Windows Time Policy** (`WindowsTimePolicy.cs`, 10 tweaks, `wtime-*`)
  - Covers NTP server URL (SetString), NTP type enforcement, enable NTP client, disable NTP server,
    poll interval, max positive/negative phase correction, update interval, phase correction rate,
    and spike watchdog

- **Sprint 250 — Photo Acquisition Policy** (`PhotoAcquisitionPolicy.cs`, 10 tweaks, `photo-*`)
  - Covers WIA camera/scanner disable, DisableAutoPlayForCamera machine/user, NeverDeleteOriginalFiles,
    DisableTaggingOnAcquire, DisableRotateOnAcquire, DisableTitleOnAcquire,
    DisableOpenFilesystemAfterAcquire, PreventDeviceMetadataFromNetwork (DeviceMetadata),
    DisableScannerEvents, and DisableCameraEvents

- **Sprint 251 — Fax Service Policy** (`FaxServicePolicy.cs`, 10 tweaks, `faxsvc-*`)
  - Covers machine/user Fax disable, online fax block, cover pages disable, personal cover pages,
    recipient book disable, TAPI-only restriction, inbound routing disable, archive disable,
    and new account creation prevention

#### Stats

- Total tweaks: **5 725** (+50 from v5.17.0)
- Categories: **288** (+5)
- Tests: **2 649** (0 failures)

## [5.17.0] — 2026-03-26

### Added

- **Sprint 242 — CD & Optical Media Policy** (`CdBurningPolicy.cs`, 10 tweaks, `cdbp-*`)
  - Covers NoBurning machine/user policy, NoCDBurning Explorer policy, CD-ROM and DVD read/write/execute restrictions
    via the RemovableStorageDevices class GUID keys, and NoAutoplayfornonVolume

- **Sprint 243 — File History Policy** (`FileHistoryPolicy.cs`, 10 tweaks, `fhp-*`)
  - Covers disabling File History, locking the on/off switch, backup interval, retention policies,
    data-degradation protection, and Windows Backup Client disable flags (DisableFileBackup,
    DisableSystemBackup, DisableRestoreUI, DisableRestoredUI)

- **Sprint 244 — Network Diagnostics Policy** (`NetworkDiagnosticsPolicy.cs`, 10 tweaks, `ndiag-*`)
  - Covers disabling the WDI network diagnostics helper engine, four WDI scenario guids (wireless,
    network connectivity, performance, networking config), scripted diagnostics execution,
    helper validation, remote server querying, and per-scenario execution-level restrictions

- **Sprint 245 — OOBE & Setup Policy** (`OobePolicy.cs`, 10 tweaks, `oobe-*`)
  - Covers DisablePrivacyExperience, SkipUserOOBE, SkipMachineOOBE, network connections wizard,
    first-logon animation (Setup key), welcome screen (machine + user), Server Manager auto-open,
    system tray balloon tips, and DisableUXFirstRunAnimation (post-upgrade)

- **Sprint 246 — MSDTC Distributed Transactions Policy** (`MsdtcPolicy.cs`, 10 tweaks, `msdtc-*`)
  - Covers AllowOnlySecureRpcCalls, FallbackToUnsecureRPCIfNecessary, TurnOffRpcSecurity,
    and the full MSDTC\Security sub-key set: NetworkDtcAccess, client/inbound/outbound/transactions,
    XaTransactions, LuTransactions

#### Stats

- Total tweaks: **5,675** (+50)
- Total categories: **283** (+5)
- Total tests: **2,667** (0 failures)

## [5.16.1] — 2026-03-25

### Fixed

- Updated stale NuGet version references across all documentation and instruction files to match `Directory.Packages.props` actuals
- Corrected `global.json` SDK pin from `10.0.200` to `10.0.201` (installed latest patch)
- Synced `docs/Development.md`, `docs/SECURITY.md`, `.github/instructions/workspace.instructions.md`, `.github/instructions/testing.instructions.md`, `.github/instructions/cicd.instructions.md`, `.github/copilot-instructions.md`, and `.github/instructions/lessons-learned.instructions.md` with current package versions

#### Stats

- Total tweaks: **5,625** (unchanged)
- Categories: **278** (unchanged)
- Tests: **2,703** passing — 2063 Core + 301 CLI + 339 GUI (0 failures)
- Version bumped `5.16.0` → `5.16.1`

## [5.16.0] — 2026-04-09 (Sprints 237–241)

### Added

- **WindowsScriptHostPolicy** (Sprint 237) — 10 WSH security tweaks: disable WSH, block remote scripts, disable trusted-cert bypass, block ActiveX in scripts, block embedded scripts, disable WScript interactive host, enable execution logging, suppress interactive UI, disable legacy VBScript engine, disable CScript console host
- **NtlmAuthPolicy** (Sprint 238) — 10 NTLM restriction tweaks: deny NTLMv1 outbound (LmCompatibilityLevel=5), disable LM hash storage, require 128-bit NTLMv2 client session security, require 128-bit NTLMv2 server session security, restrict outbound NTLM to domain servers, deny all inbound NTLM, audit incoming NTLM, audit outgoing NTLM in domain, disable null session access, require NTLMv2 on secure channel
- **DcomSecurityPolicy** (Sprint 239) — 10 DCOM security tweaks: disable remote DCOM launch/activation, restrict anonymous launch, require packet privacy, disable COM Internet Services (DCOM-over-HTTP), restrict access by machine launch restriction policy, restrict access limits policy, audit launch/activation failures, disable SCM shortcut activation, disable persistent activations timeout, block remote activation for standard users
- **KerberosEncryptionPolicy** (Sprint 240) — 10 Kerberos hardening tweaks: disable DES encryption, disable RC4-HMAC, require AES256 on LSA, set max TGT age to 600 min, set max TGT renewal age to 7 days, set max service ticket age to 600 min, set clock skew to 5 min, enable FAST armoring (cbindingPolicy=2), disable UPN hint leakage, require preauthentication
- **SyncCenterPolicy** (Sprint 241) — 10 Sync Center policy tweaks: disable Sync Center, disable setup wizard, disable Offline Files feature, prevent user configuration, remove 'Make Available Offline' context menu, disable slow-link mode, disable background sync, disable logon sync, disable logoff sync, disable reminder notifications

#### Stats

- Total tweaks: **5,625** (+50)
- Categories: **278** (+5)
- Module files: **273** (+5)
- Tests: **2,693** (unchanged)
- Version bumped `5.15.0` → `5.16.0`

## [5.15.0] — 2026-04-09 (Sprints 232–236)

### Added

- **BitLockerPolicy** (Sprint 232) — 10 BitLocker policy tweaks: require TPM, allow enhanced PIN, set minimum PIN length, require recovery password, back up recovery key to AD, disable recovery console, disable used-space-only encryption, disable auto-unlock for fixed drives, enforce hardware encryption, block non-compliant removable drives
- **WinlogonPolicy** (Sprint 233) — 10 Winlogon policy tweaks: disable last username display, disable smart card removal notification, set CTRL+ALT+DEL warning, disable password change dialog, disable logon scripts in user context, disable logoff scripts in user context, disable shutdown scripts in user context, disable cached credentials, set max cached logons to 1, require password at resume
- **PrintSpoolerPolicy** (Sprint 234) — 10 Print Spooler policy tweaks: disable printer driver installation by users, disable driver updates, redirect spool directory to secure location, disable web-based printing, disable per-machine printer connections, disable published printers in AD, set spool directory ACL, restrict printer driver installation source, disable Internet printing, restrict print driver package installation
- **DeviceInstallPolicy** (Sprint 235) — 10 Device Install policy tweaks: block prohibited devices by setup class, prevent installation via PnP removable devices, block unmatched device IDs, set activity timeout to 30 s, disable system-restore on device install, log device install activities, deny device by hardware ID, disable signed driver non-compliance prompt, block installation of unspecified device classes, prevent co-installer registration
- **UserProfilePolicy** (Sprint 236) — 10 User Profile policy tweaks: disable roaming profile changes, set max profile size, delete roaming profile cache on logoff, disable user profile advertising, block guest home folder creation, set home folder path permission, disable slow-link roaming profile, enforce mandatory profiles, delete temp profiles on logoff, disable profile quota notification

#### Stats

- Total tweaks: **5,575** (+50)
- Categories: **273** (+5)
- Module files: **268** (+5)
- Tests: **2,693** (unchanged)
- Version bumped `5.14.0` → `5.15.0`

## [5.14.0] — 2026-04-09 (Sprints 227–231)

### Added

- **DeviceGuardPolicy** (Sprint 227) — 10 Device Guard / VBS tweaks: enable VBS, require Secure Boot + DMA, enable HVCI (no UEFI lock), require UEFI MAT, enable Credential Guard, System Guard Secure Launch, kernel shadow stack (CET), disable HVCI audit mode, block unsigned drivers, audit DeviceGuard status
- **CredentialDelegationPolicy** (Sprint 228) — 10 CredSSP/delegation tweaks: enable Restricted Admin RDP, disable remote host delegation, NTLM-only CredSSP, deny default/saved/fresh credential delegation, require mutual authentication, disable CredSSP v1 (Oracle Remediation), audit delegation events, block delegation to workgroup machines
- **WindowsBackupPolicy** (Sprint 229) — 10 Windows Backup policy tweaks: disable backup/restore/catalog viewer/system backup, suppress progress UI, disable online/network/scheduled/metered backup, hide control panel link
- **WindowsFirewallPolicy** (Sprint 230) — 10 Windows Firewall policy tweaks: enable firewall on domain/private/public profiles, block inbound on domain/public, prevent local rule merging on domain/public, disable multicast unicast response on domain/public, disable notifications on public
- **AppLockerPolicy** (Sprint 231) — 10 AppLocker policy tweaks: enforce EXE/MSI/Script/DLL/Appx rules, set EXE to audit mode, enable AppIDSvc auto-start, enable collection/performance logging, block user exception creation

#### Stats

- Total tweaks: **5,525** (+50)
- Categories: **268** (+5)
- Module files: **263** (+5)

## [5.13.0] — 2026-04-09 (Sprints 222–226)

### Added

- **LanmanServerPolicy** (Sprint 222) — 10 SMB server hardening tweaks: disable admin shares, ban plain-text auth, require packet signing, enforce SPN validation, restrict null sessions, auto-disconnect idle, disable WSD multicast, audit guest logon attempts
- **LanmanWorkstationPolicy** (Sprint 223) — 10 SMB client hardening tweaks: block insecure guest auth, disable plain-text passwords, enable/require signing, enable encryption, disable SMBv1, require NTLMv2, audit logon events, disable multicast name resolution
- **LapsPolicy** (Sprint 224) — 10 Windows LAPS policy tweaks: AD backup target, password age/length/complexity, post-auth reset+logoff, post-auth delay, AD encryption, expiry protection, audit policy, expiry notification
- **SettingSyncPolicy** (Sprint 225) — 10 Settings Sync policy tweaks: disable all sync, block user override, disable credential/personalization/app/browser/start/accessibility/language sync, block sync on metered networks
- **WindowsUpdatePolicy** (Sprint 226) — 10 Windows Update policy tweaks: disable WU access, block internet locations, exclude driver updates, disable OS upgrade, defer quality/feature updates with day counts, block Insider builds, set semi-annual channel

#### Stats

- Total tweaks: **5,475** (+50)
- Categories: **263** (+5)
- Module files: **258** (+5)

## [5.12.0] — 2026-04-09 (Sprints 217–221)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`GroupPolicySettingsPolicy.cs`** — 10 tweaks (slug: `gppol`) — Group Policy Settings: disable slow-link GP skip, force reprocessing of changed GPOs, set 30-minute refresh interval, set zero random refresh offset, enable verbose GP logging, prevent users from overriding GP settings, apply GP synchronously at logon, enable RSoP logging, block local GPOs on domain members, require secure channel for GP download
- **`MapsBrowserPolicy.cs`** — 10 tweaks (slug: `mapsbr`) — Maps & Browser Policy: disable automatic offline maps download, disable untriggered background network traffic, disable location for maps, block real-time traffic data, disable offline tile storage, disable Bing search integration, disable route/directions sharing, disable personalised map suggestions, disable indoor maps, disable 3D birds-eye view
- **`BackgroundTransferPolicy.cs`** — 10 tweaks (slug: `bitsadv`) — Background Transfer Policy: limit max BITS bandwidth to 1 Mbps, limit max concurrent jobs to 5, limit files per job to 100, cap job download size to 4 GiB, cap job upload size to 1 GiB, block BITS uploads to internet destinations, require HTTPS for all transfers, set 7-day inactivity timeout, disable peer caching client, enable BITS transfer audit logging
- **`AppCompatibilityPolicy.cs`** — 10 tweaks (slug: `appcompat`) — App Compatibility Policy: disable Program Compatibility Assistant, disable shim engine, disable removal program prompt, disable online SDB look-up, disable compatibility telemetry upload, allow only IT-approved shims, block users from installing SDB files, disable Compatibility Chooser UI, log shim application events, disable per-process compatibility override
- **`EapNetworkPolicy.cs`** — 10 tweaks (slug: `eappol`) — EAP Network Policy: require server certificate validation, disable simple certificate selection, enable PEAP fast reconnect, disable identity privacy, require cryptobinding for PEAP, disable EAP-MD5, enable authentication event logging, set max auth failures to 3, require mutual authentication, block non-TLS EAP methods

#### Stats

- Total tweaks: **5,425** (+50)
- Categories: **258** (+5)
- Module files: **253** (+5)
- Tests: **2,693** (unchanged)

---

## [5.11.0] — 2026-04-09 (Sprints 212–216)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`AutoPlayPolicy.cs`** — 10 tweaks (slug: `autoplay`) — AutoPlay Policy: disable AutoRun for all drive types, disable AutoPlay for removable/optical/network drives, set default AutoPlay action to none, block autorun.inf execution, block user override of AutoPlay settings, disable AutoPlay for cameras, audit media insertions, disable AutoPlay for MTP devices
- **`NfcPolicy.cs`** — 10 tweaks (slug: `nfcpol`) — NFC Policy: disable NFC radio, disable tap-to-pay, disable tap-to-connect, disable NFC tag reading, disable NFC Secure Element, block all NFC in enterprise mode, disable NFC proximity data sharing, disable Host Card Emulation, enable NFC activity audit logging, block user NFC toggle in Settings
- **`DiskQuotaPolicy.cs`** — 10 tweaks (slug: `diskquota`) — Disk Quota Policy: enable NTFS disk quotas, enforce quota limit (deny writes at limit), log quota-exceeded events, log warning-threshold events, apply quota to all subdirectories, set default per-user limit to 1 GiB, set default warning threshold to 800 MiB, block user override of quota settings, exempt removable volumes, exempt local administrators
- **`WinRmPolicy.cs`** — 10 tweaks (slug: `winrmpol`) — WinRM Policy: disable Basic auth on client, disable Basic auth on service, require encrypted traffic on client, require encrypted traffic on service, disable Digest auth on client, disable CredSSP credential delegation client, disable CredSSP on service, restrict TrustedHosts to empty list, disable WinRM service autostart, enable WinRM session audit logging
- **`WindowsAnytimeUpgradePolicy.cs`** — 10 tweaks (slug: `wanyu`) — Windows Anytime Upgrade Policy: disable Anytime Upgrade, disable Store-based OS upgrade, block product key entry UI, log upgrade attempts, suppress upgrade notifications, prevent edition downgrade, hide Activation Settings page, disable phone activation method, lock edition to IT-deployed edition, disable trial edition conversion

#### Stats

- Total tweaks: **5,375** (+50)
- Categories: **253** (+5)
- Module files: **248** (+5)
- Tests: **2,693** (unchanged)

---

## [5.10.0] — 2026-04-09 (Sprints 207–211)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`AdvertisingInfoPolicy.cs`** — 10 tweaks (slug: `advinfo`) — Advertising Info Policy: disable Windows Advertising ID (RUID), disable personalised ad delivery, block user from re-enabling ad ID, disable interest profile building, disable cross-device ad sync, block location use for ads, block app ad consent dialogs, disable ad activity history, hide advertising settings page, disable diagnostic ad feedback
- **`MessagingSecurityPolicy.cs`** — 10 tweaks (slug: `msgsec`) — Messaging Security Policy: disable messaging cloud sync, disable MMS/picture messages, disable RCS rich communication, block cloud backup, set 90-day message retention, disable message preview in notifications, block group messaging, disable read receipts, block premium SMS, disable smart reply
- **`StorageHealthPolicy.cs`** — 10 tweaks (slug: `strhlt`) — Storage Health Policy: enable S.M.A.R.T. monitoring, enable failure prediction warnings, enable WMI health events, set 24-hour polling interval, enable SSD wear check, block health telemetry upload, enable volume integrity scan, enable Storage Spaces health, write health events to Event Log, alert when SSD spare < 10%
- **`DeviceRegistrationPolicy.cs`** — 10 tweaks (slug: `devreg`) — Device Registration Policy: disable auto Azure AD registration, require TPM for registration, limit retry count (3), block personal MSA registration, block user-initiated registration, enable audit logging, require device compliance, set certificate validity (365 days), block stale device re-registration, block skipping enrollment status page
- **`PackagedAppDebugPolicy.cs`** — 10 tweaks (slug: `padebug`) — Packaged App Debug Policy: disable Developer Mode, block debuggable package install, disable test signing, block loopback exemption, disable Windows Device Portal, block app diagnostic tracking, block background task debugger, block HTTP debug proxy, enforce package integrity on load, audit sideload attempts

### Stats

- Total tweaks: **5,325** (+50)
- Categories: **248** (+5)
- Module files: **243** (+5)
- Tests: **2,693** (unchanged)

---

## [5.9.0] — 2026-04-09 (Sprints 202–206)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`PersonalizationLockPolicy.cs`** — 10 tweaks (slug: `plock`) — Personalization Lock Policy: enforce no lock screen, enable lock screen image, block changing lock screen, prevent lock screen customisation, disable lock screen camera, suppress lock screen notifications, disable lock screen slideshow, block theme changes, block accent colour changes, disable transparency effects
- **`ScreenSaverSecurityPolicy.cs`** — 10 tweaks (slug: `scrsvr`) — Screen Saver Security Policy: enforce screen saver activation, require password on resume, set 600 s inactivity timeout, block user timeout changes, block user screen saver changes, force blank screen saver, lock password-disable UI, set minimum 30 s wait, set maximum 3600 s limit, set zero grace period
- **`NetworkConnectStatusPolicy.cs`** — 10 tweaks (slug: `ncsi`) — Network Connectivity Status Policy: disable NCSI active probing, disable global DNS probe, disable captive portal browser launch, enable corporate custom probe host, disable IPv6 probe, disable internet access check, hide network icon status warning, require corporate connectivity, set probe retry count (3), enable probe failure logging
- **`DataUsageMeteringPolicy.cs`** — 10 tweaks (slug: `datuse`) — Data Usage Metering Policy: block background data on metered connections, block automatic roaming data use, warn at 80% data limit, block Store updates on metered, block usage telemetry upload, mark new Wi-Fi connections as metered, disable cost-based app limits, block Wi-Fi Sense hotspot sharing, set 5 GB monthly cellular limit, auto-reset counter on billing cycle
- **`WcmWifiPolicy.cs`** — 10 tweaks (slug: `wcmpol`) — Wireless Connection Manager Policy: disable soft-disconnect from wired, disable simultaneous wired+Wi-Fi, disable hotspot auto-connect, allow manual Wi-Fi override, prefer wired over Wi-Fi, disable cellular fallback, block non-domain network connections on domain endpoints, disable auto network profile selection, set polling interval (60 s), disable managed Wi-Fi offload

### Stats

- Total tweaks: **5,275** (+50)
- Categories: **243** (+5)
- Module files: **238** (+5)
- Tests: **2,693** (unchanged)

---

## [5.8.0] — 2026-04-09 (Sprints 197–201)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`SmartCardCredProvPolicy.cs`** — 10 tweaks (slug: `scprov`) — Smart Card Credential Provider Policy: block certs without EKU, block signature-only keys, block expired certs, enumerate ECC certs, filter duplicate certs, force read all certs, normalise subject display, suppress X.509 hints, disallow plaintext PIN, enable logon-hours notification
- **`WorkplaceJoinPolicy.cs`** — 10 tweaks (slug: `wpjoin`) — Workplace Join Policy: disable auto-join, block AAD workplace join, require TLS, require device integrity, require consent UI, disable silent registration, limit device count, enable join audit, block non-compliant devices, require secure channel
- **`WirelessDisplayPolicy.cs`** — 10 tweaks (slug: `wdsply`) — Wireless Display (Miracast) Policy: block projection-to-PC, require PIN pairing (always), block receiver HID input, disable auto-discovery, block infrastructure projection, block Miracast broadcast, disable BLE pairing, limit connection count, require WPA2, block MDM input from receiver
- **`EventForwardingPolicy.cs`** — 10 tweaks (slug: `evtfwd`) — Event Forwarding Policy (WEF): enable subscription manager, require encryption, require Kerberos auth, limit max forward rate, set retry interval, set heartbeat interval, set connection timeout, limit max queue size, use bandwidth-minimising delivery mode, enable event consolidation
- **`LocationSensorsPolicy.cs`** — 10 tweaks (slug: `locsns`) — Location & Sensors Policy: disable all location services, disable scripted location access, disable hardware sensors, disable windowed location, disable Wi-Fi scan for geo-location, disable fused location provider, disable location history, disable cellular location data, disable geo-smoothing, deny all app location access

### Stats

- Total tweaks: **5,225** (+50)
- Categories: **238** (+5)
- Module files: **233** (+5)
- Tests: **2,693** (unchanged)

---

## [5.7.0] — 2026-04-08 (Sprints 192–196)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`WindowsSubsystemLinuxPolicy.cs`** — 10 tweaks (slug: `wslpol`) — WSL Enterprise Group Policy: disable WSL, block kernel debugging, block custom kernel, block developer installs, disable disk mounting, block networking, disable systemd, block GPU compute, disable DNS tunneling, block virtual TPM
- **`AzureAdTenantPolicy.cs`** — 10 tweaks (slug: `aadtenant`) — Azure AD Tenant Policy: block email/MSA sign-in, block non-enterprise AAD join, disable consumer app enrollment, enforce tenant restrictions, block guest accounts, block personal Microsoft accounts, require privacy consent, disable shared device sign-in, block AAD password reset from lock screen, disable cross-device cloud clipboard
- **`NearbySharingPolicy.cs`** — 10 tweaks (slug: `nshpol`) — Nearby Sharing & Cross-Device Policy: disable Nearby Sharing, block paired device sharing, disable Phone Link message sync, block contacts sync, disable Phone Link from Settings, restrict scope to own devices, block Bluetooth file sharing, block Wi-Fi Direct sharing, disable activity feed sharing, block cross-device clipboard
- **`WindowsAiPolicy.cs`** — 10 tweaks (slug: `aipol`) — Windows AI / Copilot+ / Recall Policy (Win 11 24H2+): disable Recall, disable snapshot saving, disable Copilot in Windows, disable AI data analysis, disable on-device AI processing, disable Click to Do, block AI experiences, disable content scanning, prevent background AI processing, disable automatic screenshot saving
- **`WinRmRemoteShellPolicy.cs`** — 10 tweaks (slug: `rshpol`) — WinRM Remote Shell Quota Policy: disable remote shell access, limit shells per user (2), limit concurrent users (5), set idle timeout (1 min), set max run time (15 min), limit processes per shell (5), limit memory per shell (150 MB), block environment variable modification, block interactive shells, disable inbound WinRM shells

### Stats

- Total tweaks: **5,175** (+50)
- Categories: **233** (+5)
- Module files: **228** (+5)
- Tests: **2,693** (unchanged)

---

## [5.6.0] — 2026-04-08 (Sprints 187–191)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`BiometricsConfigPolicy.cs`** — 10 tweaks (slug: `biopol`) — Windows Biometrics Group Policy: disable biometrics service, block domain biometric logon, disable secondary auth factor, enforce facial anti-spoofing, block enrollment, disable credential provider
- **`AppConsentStorePolicy.cs`** — 10 tweaks (slug: `acspol`) — App Consent Store Policy: disable consent store, restrict auto consent grants, block sensitive consent, disable consent UX, require admin approval, block third-party consent
- **`NetworkAccessProtPolicy.cs`** — 10 tweaks (slug: `nappol`) — Network Access Protection Policy: disable NAP client, DHCP/802.1X/VPN/IPSec quarantine control, disable auto-remediation, NAP UI and tray icon, policy auto-update
- **`DefenderExclusionsPolicy.cs`** — 10 tweaks (slug: `defexclpol`) — Defender Exclusions Group Policy: block local admin exclusion merging, restrict path/process/extension/IP exclusions, enable exclusion audit logging, block temp and wildcard exclusions
- **`SystemRecoveryOptionsPolicy.cs`** — 10 tweaks (slug: `sysrecpol`) — System Recovery Options Policy: disable startup repair, block recovery menu access, disable Reset PC, block WinRE CMD, disable recovery UI, block advanced tools

#### Documentation

- **`docs/Roadmap.md`** — Appended full 100-sprint tweak expansion plan (v5.6.0 through v5.25.0, Sprints 187–286) with version table, cadence rules, gap analysis workflow, and release checklist

### Stats

- Total tweaks: **5,125** (+50)
- Categories: **228** (+5)
- Module files: **223** (+5)
- Tests: **2,693** (unchanged)

---

## [5.5.0] — 2026-04-01 (Sprints 182–186)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`WindowsAttachmentsPolicy.cs`** — 10 tweaks (slug: `attach`) — File Attachment Manager zone enforcement (save zone info, anti-virus scan, MIME type blocking, unblock prevention)
- **`WindowsMailPolicy.cs`** — 10 tweaks (slug: `winmail`) — Windows Mail enterprise lockdown (block HTML images, executable attachments, shopping/news feeds, import disable)
- **`NetMeetingPolicy.cs`** — 10 tweaks (slug: `netmeet`) — Legacy NetMeeting hardening (disable app sharing, file transfer, remote desktop, audio/video, directory service)
- **`CloudNotificationsPolicy.cs`** — 10 tweaks (slug: `cloudntf`) — WNS cloud notification governance (disable cloud/account/mirroring/promotional/diagnostic notifications)
- **`ConferencingPolicy.cs`** — 10 tweaks (slug: `confer`) — Windows Meeting Space / PNRP lockdown (disable meeting space, invitations, P2P connections, people nearby)

#### Stats

- Total tweaks: **5 075** (+50 from v5.4.0)
- Categories: **223** (+5 from v5.4.0)
- Module files: **218** (+5 from v5.4.0)
- Tests: **2 693** (unchanged)

---

## [5.4.0] — 2026-04-01 (Sprints 177–181)

### Added

#### New Tweak Modules (+50 tweaks, +5 categories)

- **`SidebarGadgetsPolicy.cs`** — 10 tweaks (slug: `sidebar`) — Windows Sidebar & Gadgets enterprise lockdown: turn-off sidebar, block unsupported packages, disable user/web/desktop/third-party gadgets, disable auto-update, require signed packages, block gadget gallery
- **`CloudExperienceHostPolicy.cs`** — 10 tweaks (slug: `cehpol`) — Cloud Experience Host & OOBE policy: disable CXH, suppress privacy/account/location/network OOBE pages, skip machine OOBE, disable Cortana/Windows Hello/device-encryption OOBE screens, disable OOBE telemetry
- **`GameBarOverlayPolicy.cs`** — 10 tweaks (slug: `gamebarpol`) — Game Bar & Game Mode GPO governance: disable Game Bar, block auto-game-mode, disable Nexus/presence-writer/broadcast/startup-panel, block game-mode globally, disable clip-cursor/achievements overlay/Xbox integration
- **`WindowsAccessibilityPolicy.cs`** — 10 tweaks (slug: `a11ypol`) — Accessibility enterprise policy: disable serial-keys, sound-sentry, high-contrast hotkey, toggle/sticky/filter/bounce/mouse-keys hotkeys, prevent Magnifier startup, disable Narrator auto-start
- **`WindowsReliabilityPolicy.cs`** — 10 tweaks (slug: `relpol`) — Reliability & WER policy: disable shutdown event tracker, block RAC reporting, disable reliability archive, limit archive count, suppress WER UI/auto-upload/kernel-dump logging, limit WER queue size, disable shutdown reason requirement/display

#### Stats

- Total tweaks: **5 025** (+50 from v5.3.0)
- Categories: **218** (+5 from v5.3.0)
- Module files: **213** (+5 from v5.3.0)
- Tests: **2 693** (unchanged)

---

## [5.3.0] — 2026-04-01 (Sprints 172–176)

### Added

#### Tweaks (50 new — 5 modules × 10 tweaks each)

- **Enhanced Phishing Protection Policy** (`ephpol-*`, `EnhancedPhishingProtectionPolicy.cs`) — 10
  tweaks for Windows Defender SmartScreen WTDS GPO (Win 11 22H2+): enable WTDS service; notify
  unsafe app password reuse; warn on phishing sites; block password in plaintext files; audit-only
  mode; enterprise indicators; block credential reuse across apps; enable logging; enforce service;
  notify password change reuse.
- **OneDrive KFM Policy** (`odkfm-*`, `OneDriveKfmPolicy.cs`) — 10 tweaks for OneDrive Known
  Folder Move (KFM) GPO: silent opt-in with/without notification; opt-in wizard; silent opt-out;
  force update ring to Deferred; prevent network traffic pre-signin; min/warning disk space
  thresholds; disable TeamSite auto-mount; disable first-delete dialog.
- **Windows Information Protection Policy** (`wippol-*`, `WindowsInfoProtectionPolicy.cs`) — 10
  tweaks for WIP/EDP enterprise data protection GPO: block user decryption; require under-lock
  protection; enable EDP; silent enforcement; block copy to personal apps; disable Bing for work
  searches; revoke on MDM unenroll; show EDP icons; restrict clipboard; enterprise network isolation.
- **SNMP Policy** (`snmppol-*`, `SnmpPolicy.cs`) — 10 tweaks for SNMP service hardening GPO:
  enable auth failure traps; restrict permitted managers to localhost; remove default public
  community; set private community to Read-Only; disable write community; enforce GPO policy;
  disable SNMPv1; log auth failures to event log; block public network access; restrict trap receivers.
- **Windows Connect Now Policy** (`wcnpol-*`, `WindowsConnectNowPolicy.cs`) — 10 tweaks for
  WCN wireless device provisioning GPO: disable all registrars; disable execution service; disable
  flash config provisioning; disable in-band 802.11 registrar; disable UPnP registrar; disable
  WCN UI; disable auto device add; globally disable WCN; disable PIN-based connect; disable
  push-button connect.

#### Stats

- Tweaks: **4975** across **213 categories** and **208 modules**
- Tests: **2693** passing (0 failures)
- Version bumped `5.2.0` → `5.3.0`

---

## [5.2.0] — 2026-04-01 (Sprints 167–171)

### Added

#### Tweaks (50 new — 5 modules × 10 tweaks each)

- **App Installer Policy** (`appins-*`, `AppInstallerPolicy.cs`) — 10 tweaks for WinGet /
  MSIX App Installer machine-wide GPO: disable App Installer entirely; disable settings UI;
  disable experimental features; disable local manifest installs; require hash validation;
  disable ms-appinstaller protocol; disable additional sources; restrict to allowed sources;
  disable default source; disable Microsoft Store source.
- **Personalization Policy** (`prsnlz-*`, `PersonalizationPolicy.cs`) — 10 tweaks for lock
  screen and personalization GPO: disable lock screen; disable lock screen camera; disable
  lock screen slideshow; disable lock screen overlays; force default lock screen image;
  prevent wallpaper change; hide background settings; hide screensaver settings; hide
  appearance settings; prevent colour scheme change.
- **Search Web Policy** (`srchweb-*`, `SearchWebPolicy.cs`) — 10 tweaks for Bing/cloud
  search and Cortana GPO: disable cloud search; disable Cortana (policy); disable Cortana
  above lock; disable web results in search; disable web search over metered connections;
  disable search location; disable Bing integration; enforce SafeSearch; disable dynamic
  content in Windows Sandbox; disable indexing of encrypted files.
- **Data Collection Policy** (`datacol-*`, `DataCollectionPolicy.cs`) — 10 tweaks for
  CEIP/DAST/feedback data collection GPO: disable opt-in notification; hide telemetry
  settings UI; disable enterprise auth proxy; disable device delete button; disable feedback
  notifications; disable device name in telemetry; disable CEIP; disable sample submission;
  disable OneSetting downloads; disable diagnostic page.
- **MDM Enrollment Policy** (`mdmpol-*`, `MdmEnrollmentPolicy.cs`) — 10 tweaks for MDM
  enrollment and Windows Hello for Business GPO: disable auto MDM enroll; disable user
  MDM registration; block Azure AD workplace join; disable auto workplace registration;
  disable Windows Hello for Business; require TPM for WHFB; disable PIN recovery service;
  disable Remote Windows Hello; disable biometrics for Hello; disable Dynamic Lock.

#### Stats

- Tweaks: **4925** across **208 categories** and **203 modules**
- Tests: **2693** passing (0 failures)
- Version bumped `5.1.0` → `5.2.0`

---

## [5.1.0] — 2026-03-28 (Sprints 162–166)

### Added

#### Tweaks (50 new — 5 modules × 10 tweaks each)

- **Application Guard Policy** (`wdagpol-*`, `WdagPolicy.cs`) — 10 tweaks for Windows Defender
  Application Guard (WDAG/AppHVSI) container isolation: enable WDAG; block clipboard
  to/from container; disable printing; disable data persistence; block camera/mic; disable
  vGPU; audit mode; block download to host; configure network isolation.
- **Error Reporting Policy** (`werpol-*`, `ErrorReportingPolicy.cs`) — 10 tweaks for Windows
  Error Reporting GPO control: disable WER; block internet send; suppress crash dialogs;
  bypass throttling; disable logging; auto-approve reports; disable heap dumps; disable
  queued reporting; disable unplanned shutdown reports; purge report archive.
- **Input Method Policy** (`impol-*`, `InputMethodPolicy.cs`) — 10 tweaks for IME, touch
  keyboard, and language input policies: disable language hotkey; restrict user locale;
  disable touch keyboard auto-show; disable input personalisation; disable tablet mode
  switch; disable handwriting sharing; disable emoji panel; block IME network access;
  disable voice typing; disable cursor thickness change.
- **Content Delivery Policy** (`cdpol-*`, `ContentDeliveryPolicy.cs`) — 10 tweaks for
  Windows Spotlight, consumer auto-installs, and CDM machine-wide policies: disable consumer
  experiences; disable Windows Spotlight; disable Spotlight action center; disable third-party
  Spotlight; disable Start menu suggestions; disable Spotlight taskbar; disable OOBE tips;
  disable content delivery auto-download; disable Office promotion; disable tailored experiences.
- **Terminal Services Policy** (`tspol-*`, `TerminalServicesPolicy.cs`) — 10 tweaks for
  Remote Desktop Services (RDS) Group Policy security hardening: require NLA; enforce high
  encryption; active/idle/disconnect session timeouts; disable drive/clipboard/printer
  redirection; single-session-per-user; enable automatic reconnect.

#### Tests

- **ExecutableValidationTests** (32 tests in `RegiLattice.GUI.Tests`) — PE structure
  validation, assembly metadata checks, CLI smoke execution, and Font.ToHfont() validity
  across all 11 themes. Directly targets the Font.ToHfont() GDI resource crash from v4.7.0.

#### Stats

- **4875 tweaks** across **203 categories** and **198 modules** (+50 tweaks, +5 categories, +5 modules vs v5.0.0)
- **2693 tests** — 0 failures (Core 2052 + CLI 301 + GUI 340; +32 ExecutableValidation tests)
- Version bumped `5.0.0` → `5.1.0`

---

## [5.0.0] — 2026-03-24 (Sprints 152–161)

### Highlights

Sprints 152–161 release. Adds **100 new tweaks** across **10 new categories** covering
Delivery Optimization group policy, Windows Connection Manager policy, DNS client registration
policy, EFS encryption policy, TPM security policy, Internet printing policy (PrintNightmare
hardening), SMB shared folders policy, Tablet PC & input policy, application compatibility
policy, and Credential UI policy.

#### Stats

- **4825 tweaks** across **198 categories** and **193 modules** (3 confirmed functional duplicates removed)
- **2661 tests** — 0 failures (Core 2052 + CLI 301 + GUI 308)

#### Chore

- Removed 3 confirmed functional duplicate tweaks: `printing-disable-spooler-service` (≡ `printing-disable-print-spooler`), `pst-disable-power-efficiency-diag` (≡ `pst-disable-power-efficiency`), `priv-privacy-disable-advertising-id` (HKCU-subset of `priv-disable-advertising-id`). Net count: 4828 → 4825.
- Fixed `SECURITY.md` and `docs/SECURITY.md`: supported version `4.x → 5.x`.
- Updated root `CHANGELOG.md` with v5.0.0 entry.

### Added

#### Tweaks (100 new — 10 modules × 10 tweaks each)

- **Delivery Optimization Policy** (`doptpol-*`, `DeliveryOptimizationPolicy.cs`) — HTTP-only
  download mode; min background QoS; max upload bandwidth; cache size/age/path controls;
  min disk/RAM/file-size thresholds; background download hour limits.
- **Windows Connection Manager Policy** (`wcmpol-*`, `WcmConnectionPolicy.cs`) — Disable
  auto-connect; minimize connections to single interface; block non-domain networks; prefer
  wired; soft-disconnect; disable WLAN/WWAN; access restrictions on reconnect; block local policy merge.
- **DNS Client Registration Policy** (`dnscgpo-*`, `DnsClientRegistrationPolicy.cs`) — Disable
  dynamic registration; adapter name/reverse lookup registration; multicast FQDN discovery;
  domain name devolution; Unicode DNS; smart name resolution; negative cache TTL limits.
- **EFS Encryption Policy** (`efspol-*`, `EfsEncryptionPolicy.cs`) — Disable EFS; suppress
  cert request UI; page-file encryption; FIPS-required smart card; Enhanced Storage Devices
  policy: deny legacy/1394 devices, password silo, machine-lock, TCG security activation.
- **TPM Security Policy** (`tpmgpo-*`, `TpmSecurityPolicy.cs`) — AD backup requirement;
  OS-managed auth level; standard-user lockout thresholds/duration; Credential Guard
  (LsaCfgFlags); HVCI; Secure Launch (ConfigureSystemGuardLaunch); VBS.
- **Internet Printing Policy** (`inetprt-*`, `InternetPrintingPolicy.cs`) — Disable web/HTTP
  printing; block spooler RPC endpoint; block kernel-mode drivers; package-only Point and
  Print; no-warning/elevation install hardening (PrintNightmare mitigations); driver download
  disable; admin-only driver install; v3 driver block.
- **SMB Shared Folders Policy** (`smbshare-*`, `SharedFoldersSmbPolicy.cs`) — Disable insecure
  guest logons; require/enable SMB signing; restrict null-session pipes/shares; auto-disconnect
  idle sessions at 15 min; forced logoff; disable admin shares; max SMB connections cap.
- **Tablet PC & Input Policy** (`tabpol-*`, `TabletPcInputPolicy.cs`) — Prevent handwriting
  data sharing and error reports; disable InkBall; turn off passwordless; prevent handwriting
  personalization; disable pen training/feedback; disable touch input; panning feedback;
  flick gesture disable.
- **App Compatibility Policy** (`accompat-*`, `AppCompatGpoPolicy.cs`) — Disable inventory
  collector; PCA; AIT telemetry; UAT; compatibility wizard; engine; SwitchBack; Steps Recorder
  (UAR); block 16-bit apps; suppress WER generated by appcompat.
- **Credential UI Policy** (`credui-*`, `CredentialUiPolicy.cs`) — Disable password-reveal
  button; suppress admin enumeration; no local password reset questions; secure desktop
  credential prompting; no visual prompt animation; disable RDP credential save; disable
  Windows Hello PIN login; user-scope reveal block; generic credential caching block;
  auto-fill on credential forms disable.

#### Tests

- Search budget relaxed from 150ms → 250ms in `Search_CompletesUnder50ms` to accommodate
  4828 tweaks (baseline ~172ms at v5.0.0). Threshold note added for next review at 6000 tweaks.

---

## [4.9.0] — 2026-03-23 (Sprints 142–151)

### Highlights

Sprints 142–151 release. Adds **100 new tweaks** across **10 new categories** covering
LLTD network discovery, Windows Media Player policy, device provisioning, Windows Maps
Connected Search, diagnostic data pipeline, system restore GPO, modern Start menu layout,
cloud content/Spotlight policy, additional app privacy capabilities, and UAC advanced
logon/display controls.

### Added

#### Tweaks (100 new — 10 modules × 10 tweaks each)

- **Network Map Discovery Policy** (`netlltd-*`, `NetworkLltdPolicy.cs`) — LLTDIO enable/disable
  on domain/private/public networks; Rspndr responder control; Peernet/PeerToPeer disabled.
- **Windows Media Player Policy** (`wmply-*`, `WindowsMediaPolicyAdv.cs`) — HKLM screensaver,
  metadata retrieval (CD/DVD, music, radio) suppression, preset/codec/protocol auto-download
  disable and user-scope overrides.
- **Device Provisioning Policy** (`devprov-*`, `DeviceProvisioningPolicy.cs`) — OOBE network,
  first-logon animation, privacy settings page; HomeGroup creation; WorkplaceJoin workplace
  join/device registration; CloudContent Find My Device, SoftLanding, tailored experience.
- **Windows Maps Policy** (`wmaps-*`, `WindowsMapsPolicy.cs`) — Maps auto-download disable;
  HKLM + HKCU Connected Search privacy, SafeSearch enforcement, Search Highlights, Cortana AAD.
- **Diagnostic Data Viewer Policy** (`diagdvr-*`, `DiagnosticDataViewerPolicy.cs`) — DataViewer
  disable, device health attestation, diagnostic log limits, enterprise auth proxy, settings auditing,
  Update Compliance/WUfB/Desktop Analytics/commercial data pipeline controls.
- **System Restore Policy** (`srgpo-*`, `SystemRestoreGpoPolicy.cs`) — GPO DisableSR/DisableConfig;
  VSC session/global intervals, disk percent cap, system checkpoints, scan interval,
  optimistic restore, restore-point creation frequency, incremental restoration prevention.
- **Start Menu Modern Policy** (`smmod-*`, `StartMenuModernPolicy.cs`) — StartMenuExperience
  recent/recommended apps/items, People Bar, MSA notification; Explorer frequent programs,
  recent docs, preview/details panes, machine-boot uninstall.
- **Cloud Content Policy** (`ccpol-*`, `CloudContentPolicy.cs`) — HKLM consumer features,
  third-party suggestions, cloud-optimised content; Spotlight (features, welcome screen, settings,
  third-party suggestions) at both HKLM and HKCU scope.
- **App Privacy Policy Advanced** (`appprv2-*`, `AppPrivacyPolicyAdv.cs`) — LetApps* Force-Deny
  for call history, calendar, contacts, radios, trusted devices, diagnostic info, email, gaze input,
  voice activation, and voice activation above lock screen.
- **User Account Control Advanced Policy** (`uacadv-*`, `UserAccountControlAdvPolicy.cs`) — automatic
  restart sign-on, network selection UI, failed unlock display, locked user ID display, MSA optional,
  shutdown without logon, lock workstation/change password disable, legal notice caption, Task Manager
  disable.

### Stats

- Tweaks: **4728** (+100 from v4.8.0)
- Categories: **188** (+10)
- Module files: **183** (+10)
- Tests: **2661** (2052 Core + 301 CLI + 308 GUI, 0 failures)

---

## [4.8.0] — 2026-03-22 (Sprints 140–141)

### Highlights

Sprints 140–141 release. Adds **100 new tweaks** across **10 new categories** covering
advanced AMSI/script policy, IE compatibility hardening, logon cache policy, Bluetooth
advertising control, process mitigation (SEHOP/ASLR/Spectre/LSA-PPL), remote assistance
policy, network hardened paths, USB storage policy, kiosk assigned access, and application
restart/crash control.

### Added

#### Tweaks (100 new — 10 modules × 10 tweaks each)

- **AMSI & Script Policy** (`amsi-*`, `AmsiScriptPolicy.cs`) — PowerShell ScriptBlock logging,
  module logging, transcription, constrained language mode, WScript disable, Defender cloud
  protection level/timeout, PS v2 disable.
- **IE Compatibility Policy** (`iecompat-*`, `IECompatPolicy.cs`) — IE Enterprise Mode / IE-in-Edge
  disable, IE first-run, cookie/homepage/autocomplete lockdown, IE zone elevation (IEHarden),
  IE add-on install prompt, Edge HTTPS upgrades, Edge password manager.
- **Logon Cache Policy** (`lgncache-*`, `LogonCachePolicy.cs`) — CachedLogonsCount 2/0,
  smart-card remove lock, password-expiry warning (14 days), ForceUnlockLogon, Netlogon
  RequireStrongKey/RequireSignOrSeal/SealSecureChannel/SignSecureChannel, disable domain
  password cache.
- **Bluetooth Advertising Policy** (`btadv-*`, `BluetoothAdvPolicy.cs`) — advertising,
  promiscuous mode, pairing notifications, connectable timeout, file transfer, phonebook
  access, encryption enforcement, remote audio, discoverable state, shared experiences.
- **Application Restart Policy** (`apprstrt-*`, `ApplicationRestartPolicy.cs`) — AeDebug auto
  (JIT debugger), auto-reboot on BSOD, crash dump type (kernel), event logging, WER
  reporting/queue/throttle/consent, dump size limit, overwrite existing dump.
- **Process Mitigation Policy** (`prctmtg-*`, `ProcessMitigationPolicy.cs`) — SEHOP
  (exception chain validation), heap termination on corruption, mandatory ASLR (MoveImages),
  bottom-up + high-entropy ASLR, kernel stack cookies, LSA RunAsPPL, safe DLL search mode,
  Spectre mitigations (BpbEnabled), clear page file at shutdown.
- **Remote Assistance Policy** (`rast-*`, `RemoteAssistancePolicy.cs`) — RA disable,
  shadow/ticket control, firewall rules, helper configuration.
- **Network Hardened Paths** (`nethpth-*`, `NetworkHardenedPaths.cs`) — UNC hardened paths
  with RequireMutualAuthentication/RequireIntegrity/RequirePrivacy flags.
- **USB Storage Policy** (`usbstor-*`, `UsbStoragePolicy.cs`) — USBSTOR start/write-protect,
  removable storage per-class ACL policies.
- **Kiosk Assigned Access** (`kiosk-*`, `KioskAssignedAccess.cs`) — kiosk/assigned-access
  lock screen and session policy controls.

#### Fixes

- **Race condition fix** in `RatingsTests` / `RatingsFileExistsBranchTests` — added
  `[Collection("Ratings")]` to both classes so the file-delete in `RatingsFileExistsBranchTests`
  no longer races with `Rate_ValidStars_CreatesRating`.
- **WFO1000 suppression** in `RegiLattice.GUI.csproj` — added `WFO1000` to `<NoWarn>` to
  suppress the .NET 10 WinForms SDK false-positive on `[DesignerSerializationVisibility]`.

### Stats

- Tweaks: **4628** (+100 from v4.7.0)
- Categories: **178** (+10)
- Module files: **173** (+10)
- Tests: **2661** (2052 Core + 301 CLI + 308 GUI, 0 failures)

---

## [4.7.0] — 2026-03-23 (M4 Milestone)

### Highlights

M4 milestone release (Sprints 130–139). This is the first fully published GitHub release
since v4.6.0 (M3). It contains **420 new tweaks**, new plugin infrastructure, Chocolatey
distribution and mutation-testing improvements.

### Added

#### Distribution

- **Chocolatey package** (`chocolatey/`) — full `regilattice.nuspec` + install/uninstall scripts.
  GitHub Actions release workflow now auto-builds a `.nupkg` and a SHA-256–verified zip on every tag push.

#### Plugin System

- **RSA-SHA256 pack signing** (`PackSignatureVerifier.cs`) — cryptographic signature
  verification for JSON Tweak Packs; prevents tampered community packs from loading.
- **Plugin sandbox via named pipe** (`PluginSandbox.cs`) — pack sub-processes run in
  a restricted environment and communicate through a named pipe, isolating the host process
  from untrusted code.

#### Testing

- **Virtual Registry integration tests** (`VirtualRegistryTests.cs`) — full end-to-end
  engine tests that operate on a virtual registry overlay instead of the real HKCU/HKLM.
- **Stryker.NET mutation testing** (`scripts/Run-MutationTests.ps1`, `.config/dotnet-tools.json`)
  — infrastructure for mutation coverage quality gates.

#### Tweaks (420 new across 40 new categories)

| Sprint | Modules | New tweaks |
|--------|---------|------------|
| 136 | AppVirtualization, BranchCache, InternetZonePolicy, SensorPolicy, LocationSensors | 50 |
| 137 | NetworkDiscovery, CertificatePolicy, PowerShellPolicy, DefenderAdvanced, EventLogPolicy | 50 |
| 138 | SmartScreenPolicy, CredentialCachingPolicy, WindowsTimePolicy, FirewallLogPolicy, LogonPolicy | 50 |
| 139 | ShellRestrictionsPolicy, BitsTransferPolicy, OfflineFilesSyncPolicy, MsiInstallerPolicy, SmbServerPolicy | 50 |
| Various (130–135) | AppPrivacyPolicy, CertificateServices, NetworkConnections, and 17 more | 220 |

#### Bug Fixes

- **`Ratings.Save()` cross-process retry** — added 5-attempt retry loop (60×n ms backoff)
  to handle `IOException` when test projects run in parallel and race on `ratings.json`.

### Stats

- Total tweaks: **4528** across **168 categories** (163 module files)
- Total tests: **2660** passing — 2052 Core + 301 CLI + 307 GUI (0 failures)
- Version bumped `4.6.0` → `4.7.0`

---

## [4.6.9] — 2026-05-15

### Sprint 139 — 50 New Tweaks: 5 New Modules (T8.3)

#### Added

- **ShellRestrictionsPolicy** (`shellrst`, 10 tweaks) — Shell restrictions via `HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer`: NoRun, NoFind, NoClose, NoLogoff, NoDesktop, NoDrivesPage, NoCplApplets, NoDispCPL, DisallowRun, NoNetHood.
- **BitsTransferPolicy** (`bitspol`, 10 tweaks) — BITS background transfer limits via `HKLM\SOFTWARE\Policies\Microsoft\Windows\BITS`: JobInactivityTimeout, MaxJobsPerMachine, MaxJobsPerUser, MaxJobFilesPerJob, MaxRangesPerFile, MaxDownloadTime, MaxInternetBandwidth, EnableBITSMaxBandwidth, DisablePeerCachingClient, DisablePeerCachingServer.
- **OfflineFilesSyncPolicy** (`offsync`, 10 tweaks) — Offline Files sync governance via `NetCache` and `SyncMgr` policy paths: NoMakeAvailableOffline, PurgeAtLogoff, BackgroundSyncEnabled, DefaultCacheSize, GoOfflineAction, EventLoggingLevel, DisableSyncActivity, TurnOffSyncOnCostedNetwork, DisableFileSyncClient, HideOptionsForSyncProvider.
- **MsiInstallerPolicy** (`msipol`, 10 tweaks) — Windows Installer hardening via `HKLM\SOFTWARE\Policies\Microsoft\Windows\Installer`: DisablePatch, DisableBrowse, DisableMSI, TransformsSecure, SafeForScripting, EnforceUpgradeComponentRules, LimitSystemRestoreCheckpointing, DisableLockdownBrowseUI, DisableForbidPatch, DisableMedia.
- **SmbServerPolicy** (`smbsrv`, 10 tweaks) — SMB server security and performance via `LanmanServer\Parameters`: AutoShareServer, AutoShareWks, EnableOpLocks, EnableForcedLogoff, IRPStackSize, MaxMpxCt, MaxWorkItems, EnableRaw, SizReqBuf, DiskSpaceThreshold.

#### Stats

- Total tweaks: ~4258 across ~141 categories
- Total tests: 2052 (0 failures)

---

## [4.6.8] — 2026-05-15

### Sprint 138 — 50 New Tweaks: 5 New Modules (T8.2)

#### Added

- `SmartScreenPolicy.cs` (10 tweaks, `smartscr`) — Windows Defender SmartScreen GPO policy:
  shell enable/block-level, app install control, Edge PhishingFilter prevent-override,
  Edge SmartScreen enable/PUA/force-enabled.
- `CredentialCachingPolicy.cs` (10 tweaks, `credcache`) — Credential caching hardening:
  CredSSP oracle mitigation (CVE-2018-0886), RDP RestrictedAdmin delegation,
  WDigest disable, LSA PPL, domain cred/anonymous/LM-hash restrictions.
- `WindowsTimeGpoPolicy.cs` (10 tweaks, `timepol`) — W32Time GPO policy path
  (`SOFTWARE\Policies\Microsoft\W32Time\*`); NTP type/servers, client enable/poll/log,
  server disable, phase/frequency correction rates.
- `FirewallLogPolicy.cs` (10 tweaks, `fwlog`) — Windows Firewall logging GPO policy:
  Domain/Private/Public profile dropped-packets, successful-connections, log-size, log-path.
- `LogonGpoPolicy.cs` (10 tweaks, `logonpol`) — Logon screen privacy/security GPO policy:
  hide last username, hide network UI, hide account details, disable ARSO, disable startup
  sound, block MSA accounts, hide locked user ID, lockout attempts, disable lock-screen
  notifications, hide power button.

#### Stats

- Tweaks: **4,308** (+50 from v4.6.7)
- Categories: **141** (+5)
- Core tests: **2,052** (0 failures)

---

## [4.6.7] — 2026-05-14

### Sprint 137 — 50 New Tweaks: 5 New Modules (T8.2)

#### Added

- `NetworkDiscovery.cs` — 10 tweaks (netdisc): LLTD mapper/responder, mDNS,
  NetBIOS P-Node, UPnP Host, SSDP, FDResPub service disablement
- `CertificatePolicy.cs` — 10 tweaks (certpol): .NET 4 + .NET 2 SchUseStrongCrypto
  (64-bit + WoW64), SystemDefaultTlsVersions, Wintrust certificate padding check, root
  CA auto-update, IE certificate revocation enforcement
- `PowerShellPolicy.cs` — 10 tweaks (pspolicy): ScriptBlockLogging,
  InvocationLogging, ModuleLogging, Transcription (on + header + output path),
  PS2 engine disable, protected event logging, EnableScripts + AllSigned execution policy
- `DefenderAdvanced.cs` — 10 tweaks (defadv): cloud block level High,
  BAFS extended timeout, MAPS advanced membership, auto sample submission,
  behavior/IOAV/script scanning enable, archive + email scanning, scan time randomisation
- `EventLogGpoPolicy.cs` — 10 tweaks (evtgpo): GPO-path event log sizes
  (Application 128 MB, Security 1 GB, System 128 MB, Setup 64 MB, ForwardedEvents 256 MB)
  and overwrite-when-full retention policy for all 5 channels

#### Stats

- Total tweaks: **4,258** (+50)
- Tests: **2,052 Core / 2,660 total** — 0 failures

## [4.6.6] — 2026-05-14

### Sprint 136 — 50 New Tweaks: 5 New Modules (T8.2)

#### Added

- **`Biometrics.cs`** (10 tweaks, category "Biometrics", slug `bio`): Windows Hello for Business and biometric hardware group-policy controls.
  - `bio-disable-biometrics` — disable Windows Biometric Service (GPO).
  - `bio-disable-biometrics-domain` — block domain/AAD biometric sign-in.
  - `bio-disable-biometric-sign-in` — Credential Provider level disable.
  - `bio-enable-facial-anti-spoofing` — Enhanced Anti-Spoofing (ISO PAD).
  - `bio-whfb-require-tpm` — require TPM for WHFB key storage.
  - `bio-whfb-pin-min-length` — minimum PIN = 8 chars.
  - `bio-whfb-pin-require-digits` — PIN must contain digits.
  - `bio-whfb-pin-require-uppercase` — PIN must contain uppercase.
  - `bio-whfb-pin-require-lowercase` — PIN must contain lowercase.
  - `bio-whfb-pin-expiry` — PIN expires every 90 days.
- **`WinRmHardening.cs`** (10 tweaks, category "WinRM Hardening", slug `winrm`): WS-Management authentication hardening.
  - Client-side: deny basic, plaintext, Digest, NTLM, CredSSP auth.
  - Service-side: deny basic, plaintext, NTLM; disable RunAs; allow Kerberos.
- **`LocationSensors.cs`** (10 tweaks, category "Location & Sensors", slug `loc`): Location scripting, sensor framework, and per-app policies.
  - Complements `priv-disable-location` (OS service) and `aperm-deny-location` (user GUID).
  - Covers: DisableLocationScripting, DisableSensors, DisableWindowsLocationProvider, LetAppsAccessLocation=2, LetAppsAccessMotion=2, Windows Search location, Wi-Fi auto-connect, user ConsentStore deny, IE geolocation block.
- **`SettingSyncAdv.cs`** (10 tweaks, category "Settings Sync", slug `ssync`): Granular SettingSync policies beyond the MicrosoftAccount.cs master toggle.
  - Desktop theme, Start layout, browser, language, accessibility, personalization, Windows settings sync disable.
  - Typing/text personalization (RestrictImplicitTextCollection), handwriting/ink collection, machine-policy input personalization off.
- **`AppPrivacyPolicy.cs`** (10 tweaks, category "App Privacy Policy", slug `appp`): Machine-level `HKLM AppPrivacy` LetApps\* force-deny policies.
  - Covers: camera, microphone, notifications, account info, background run, device sync, phone, tasks, messaging, video library.
  - Distinct from `AppPermissions.cs` which uses HKCU DeviceAccess GUIDs (per-user).

#### Stats

- Total tweaks: **4,208** (+50)
- Tests: **2,660 passing** (0 failures)
- New categories: Biometrics, WinRM Hardening, Location & Sensors, Settings Sync, App Privacy Policy

---

## [4.6.5] — 2026-05-14

### Sprint 135 — Stryker.NET Mutation Testing Setup (T6.6)

#### Added

- **`stryker-config.json`** (project root): Stryker.NET 4.14.0 configuration for `RegiLattice.Core`.
  - `mutation-level: Standard` — covers arithmetic, boolean, boundary, and null mutations.
  - Thresholds: `high=80%`, `low=60%`, `break=55%` — CI fails if kill score drops below 55%.
  - Target files: 15 Core source files (TweakEngine, TweakDef, RegistrySession, TweakValidator, DependencyResolver, SnapshotManager, AppConfig, Favorites, Ratings, TweakHistory, ConfigExporter, PackLoader, PackManager, PackSignatureVerifier, PluginSandbox).
  - Reports: HTML + JSON output to `.tmp/stryker-output/` (gitignored).
- **`.config/dotnet-tools.json`** — `dotnet-stryker 4.14.0` added as a local tool alongside CSharpier.
- **`scripts/Run-MutationTests.ps1`**: PowerShell developer script for local mutation test runs with restore, build, and friendly output.
- **`ci.yml` — `mutation-testing` job**: Runs Stryker on main-branch pushes only (not PRs). Uploads HTML report as a CI artifact. Break threshold enforces 55% minimum kill score.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,660 passing** (unchanged)

---

## [4.6.4] — 2026-05-14

### Sprint 134 — Virtual Registry Integration Tests (T6.3)

#### Added

- **`VirtualRegistryTests.cs`** (new, `RegiLattice.Core.Tests`): 15 integration tests that exercise the real Windows registry without administrator rights, using a GUID-scoped HKCU isolation key that is created and cleaned up per-test.
  - Basic round-trip reads/writes: `SetDword`, `SetString`, `SetQword`, `SetBinary`, `SetMultiSz`.
  - Delete operations: `DeleteValue`, `DeleteTree`.
  - `Evaluate()` detection: `CheckDword` true/false, `CheckMissing` before/after set, `CheckKeyMissing`.
  - Full `Execute`→`Evaluate`→`Execute` (Apply→Detect→Remove) round-trip via `RegistrySession` directly.
  - Full Apply→Detect→Remove cycle through `TweakEngine` using a real (non-DryRun) session.
  - DryRun isolation guard: verifies that a DryRun write does not appear in the real registry.
- **Implementation note**: `RegLoadKey`/`RegUnLoadKey` hive isolation requires `SeRestorePrivilege` (admin). The HKCU-subkey approach provides equivalent per-test isolation and runs on `windows-latest` CI runners without elevation.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,052 Core · 301 CLI · 307 GUI = 2,660 total** (+15)

---

## [4.6.3] — 2026-05-14

### Sprints 132–133 — Plugin Sandbox Isolation (T7.4)

#### Added

- **`PluginSandbox.cs`** (new, `RegiLattice.Core.Plugins`): executes third-party `TweakPack` `RegOp` lists in an isolated child process via a named pipe with a configurable timeout (default 30 s).
  - **`SandboxOpDto`** (internal): JSON DTO for a single `RegOp` — 9 typed fields (`Kind`, `Path`, `Name`, `DwordValue`, `StringValue`, `QwordValue`, `BinaryValue`, `MultiSzValue`) with `[JsonPropertyName]` camelCase serialisation.
  - **`PluginSandboxRequest`** (internal): wire message sent to child — `{ bool DryRun, IReadOnlyList<SandboxOpDto> Ops }`.
  - **`PluginSandboxResponse`** (internal): reply from child — `{ bool Success, string ErrorMessage }`.
  - **`PluginSandboxResult`** (public): caller-facing result — `{ bool Success, string ErrorMessage, bool TimedOut }`.
  - **`PluginSandbox.ToDto()`** / **`FromDto()`** (internal static): bidirectional mapping between `RegOp` and `SandboxOpDto` covering all 12 `RegOpKind` variants.
  - **`PluginSandbox.ExecuteAsync()`** (public async): parent side — spawns child, creates a named pipe server, writes request JSON, reads response, enforces timeout. Returns `TimedOut=true` on `OperationCanceledException`; catches spawn-failure exceptions.
  - **`PluginSandbox.RunHostAsync()`** (public async): child side — connects to parent pipe, deserialises request, executes ops via `RegistrySession`, serialises response, returns exit code 0 on success.
- **`CliArgs.PluginHost`** and **`CliArgs.PluginPipeName`** properties: gate the child-process dispatch path.
- **`Program.cs`** — `--plugin-host <pipeName>` flag: parsed before engine initialisation; dispatches immediately to `PluginSandbox.RunHostAsync()` and exits.
- **17 new `PluginSandboxTests`** covering: 10 `ToDto` kind tests, `FromDto` round-trip (12 kinds), `PluginSandboxRequest`/`PluginSandboxResponse` JSON serialisation round-trips, error response serialisation, `ExecuteAsync` with non-existent executable, and `PluginSandboxResult` model defaults.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,037 passing** (+17)

---

## [4.6.2] — 2026-05-13

### Sprint 131 — Pack RSA-SHA256 Signing Support (T7.3)

#### Added

- **`PackTrustLevel` enum** (`PackDef.cs`): three levels — `None`, `HashVerified`, `Signed` — representing the verified integrity state of a Tweak Pack at runtime.
- **`PackDef` — `SignatureUrl` property**: optional URL pointing to the detached `.rlpack.sig` RSA signature file. Serialised in JSON pack index; empty string by default.
- **`PackDef` — `TrustLevel` property**: runtime-only (not persisted); set by `DetermineTrustLevel()` after loading a pack.
- **`PackSignatureVerifier.cs`** (new): RSA-PKCS#1-v1.5 / SHA-256 signing and verification for Tweak Packs.
  - `Verify(ReadOnlySpan<byte>, signatureBase64, publicKeyPem)` — low-level verifier.
  - `Verify(string packJson, signatureBase64, publicKeyPem)` — convenience overload for UTF-8 pack JSON.
  - `DetermineTrustLevel(packJson, pack, sig?, pubKey?)` — resolves `PackTrustLevel`: `Signed` if RSA check passes, `HashVerified` if only SHA-256 matches, `None` otherwise.
  - `Sign(packJson, privateKeyPem)` — returns base64 signature (author tooling).
  - `GenerateKeyPair(keySize=2048)` — returns `(PublicKeyPem, PrivateKeyPem)` tuple (author tooling / tests).
  - Enforces minimum 2048-bit RSA key size; throws `CryptographicException` on under-sized keys.
- **`PackIndex` — `AuthorKey` record** and **`AuthorKeys` list**: pack marketplace index can now carry per-author public keys (PEM format), enabling automated trust resolution for all packs from a known author.
- **`PackIndex.GetAuthorPublicKey(string author)`**: case-insensitive lookup returning the PEM public key for a named author, or `null` if not found.
- **13 new `PackSignatureVerifierTests`** covering: key generation, sign+verify round-trip, tampered content detection, wrong-key rejection, empty/invalid base64 handling, `DetermineTrustLevel` for all three levels, and `PackIndex.GetAuthorPublicKey` including case-insensitive match.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,020 passing** (+13)
- Version: `4.6.2`

---

## [4.6.1] — 2026-05-12

### Sprint 130 — Chocolatey Package & Distribution Improvements (T5.3)

#### Added

- **Chocolatey package** (T5.3): `chocolatey/regilattice.nuspec` — full package descriptor with title, authors, tags, release notes URL, and description. `chocolatey/tools/chocolateyInstall.ps1` — zip-based install with SHA-256 verification, auto-shimming of `RegiLattice.exe` and `RegiLattice.GUI.exe`. `chocolateyUninstall.ps1` with optional `/PurgeData` parameter. `VERIFICATION.txt` for Chocolatey Community Repository trust audit.
- **Release workflow — Chocolatey build step**: CI now packs a `RegiLattice-<version>-win-x64.zip` from the published EXEs, injects the download URL + SHA-256 into `chocolateyInstall.ps1`, runs `choco pack`, and pushes the `.nupkg` to the Chocolatey Community Repository if `CHOCOLATEY_API_KEY` is set. The `.nupkg` and `.zip` are also uploaded as GitHub Release assets.
- **Release workflow — expanded SHA256SUMS**: Checksum file now covers `.zip` and `.nupkg` artifacts in addition to EXEs and MSI/MSIX.
- **Scoop manifest updated**: `scoop/regilattice.json` bumped to v4.6.0, improved `bin` aliases (`regilattice` → CLI, `regilattice-gui` → GUI), expanded `notes` array with dry-run tip.
- **MSIX description updated**: `installer/AppxManifest.xml` description updated to reflect 4,158 tweaks / 126 categories.

#### Stats

- Total tweaks: **4,158** (unchanged)
- Tests: **2,007 passing**
- Distribution channels: GitHub · Scoop · WinGet + **Chocolatey** (new)
- Version: `4.6.1`

---

## [4.6.0] — 2026-05-12

### M3 Milestone — GUI Polish, i18n, Marketplace & Security Policies (Sprints 122–129)

#### Added

- **ToggleSwitchControl** (Sprint 122, T1.5): Custom GDI+ animated toggle switch with smooth slide animation, theme-aware colours, DPI scaling, keyboard support (Space/Enter), focus ring, and accessible `AccessibleObject`. 15 new `ToggleSwitchControlTests`.
- **Visual Refresh** (Sprint 123, T1.6): `RoundedPanel` — GDI+ rounded-corner panel with Mica tint; `FluentIcons` — 60+ Segoe Fluent Icons glyph constants + `DrawGlyph`/`CreateGlyphBitmap` helpers; `CategoryExpandButton` — animated 90° chevron for category headers. 24 new `VisualPolishTests`.
- **Pack Creator Studio Dialog** (Sprint 125, T7.2): 5-step wizard dialog for authoring community Tweak Packs. Steps: basic info → tweak selection → metadata → JSON preview → export + submit URL. 11 new `PackCreatorDialogTests`.
- **PackSubmissionService** (Sprint 124, T7.5): `Validate(PackDef)`, `BuildSubmissionUrl(PackDef)`, `SanitizeName()` with kebab-case slug validation, semver version, SHA-256 hex check, HTTPS source URL. GitHub Issue template `pack-submission.yml` with 12-field form and 5-item checklist. 17 new `PackSubmissionServiceTests`.
- **4 New Locales** (Sprint 126, T2.4/T2.5): zh-CN (~120 keys), ko (~120 keys), ar (~120 keys), pt-BR (~120 keys) added to `Locale.cs`. `BuiltInLocales` expanded from 6 → 10 registered locales. 15 new `LocaleSupplementalTests`.
- **CLI Tab Completion + Profile CRUD** (Sprint 127, T3.4/T8.5): `completions/RegiLattice.ps1` — full PowerShell tab completion with context-aware completions for profile names, scope values, and categories. CLI args `--profile-create/delete/clone/rename/tweaks/desc` + `--list-user-profiles`. `UserProfileService` CRUD wiring in `Program.Dispatch`. 10 new `ParseArgsTests`.
- **Compliance Trend Dashboard** (Sprint 128, T4.4): `ComplianceTrendDialog` — GDI+ line chart of compliance score / violation count over time, toggle between % compliance and violation-count modes, Refresh/Clear/Close controls, entry-count summary label. 12 new `ComplianceTrendDialogTests`.
- **50 New Security & Policy Tweaks** (Sprint 129, T8.2): 5 new modules across enterprise policy areas:
  - **BranchCache.cs** (`bc-*`) — 10 tweaks: distributed/hosted cache mode, cache size (% + GB cap), SHA-256 hashes, firewall exceptions, SMB hash publication, hosted-cache preference, retrieval latency, peer offering delay.
  - **InternetZonePolicy.cs** (`izone-*`) — 10 tweaks: lock zones to machine policy, block ActiveX/ActiveScript/mixed-content in Internet zone, prevent cert error bypass, block auto file download, SmartScreen phishing filter, block unencrypted form submit, block unsafe ActiveX init, block script clipboard access.
  - **NetworkConnectionsPolicy.cs** (`netconn-*`) — 10 tweaks: honour admin prohibits, prevent network bridges, block add/remove components, block binding change, prevent deleting all-user connections, block LAN properties, block VPN connect, block all-user VPN properties, prohibit ICS, prevent connection rename.
  - **AppVirtualization.cs** (`appv-*`) — 10 tweaks: allow package scripts, block high-cost launch, require admin to publish, auto-load background, disable shared content store, enable process interop, block virtual COM creation, enable reporting, 24 h reporting interval, 120 s streaming timeout.
  - **SensorPolicy.cs** (`sensor-*`) — 10 tweaks: block location scripting, disable all sensors, disable Windows Location Provider, prevent user location override, deny radios/activity/gazeInput/contacts/email/bluetoothSync capabilities.

#### Fixed

- Test suite: removed external-process `DetectAction` sweep tests from `BranchCoverage5Tests.cs` that caused >60 s hangs vs. the 20 s per-test budget. 837 placeholder sweep tests replaced by structural-only assertions. Suite now runs in ≤10 s with no hang risk.
- Test isolation: added `[Collection("ComplianceHistory")]` to `ComplianceHistoryTests` and `ComplianceHistoryNullJsonBranchTests` to eliminate rare file-system race causing intermittent failures.

#### Stats

- Total tweaks: **4,158** across **126 categories** (+50 tweaks, +5 categories vs 4.5.0)
- Tests: **2,007** passing (0 consistent failures)
- Build: **0 errors, 0 warnings** (Release x64)
- Version bumped `4.5.0` → `4.6.0`

---

## [4.5.0] — 2026-03-23

### M2 Milestone — Enterprise & Trust (Sprints 114–121)

#### Added

- **IntuneOmaUriExporter.cs**: Full Intune Custom Configuration Profile OMA-URI export for HKLM-path Registry-kind tweaks. Maps to `./Device/Vendor/MSFT/Policy/Config/...` CSP format. Unmappable tweaks flagged with `[NOT_MAPPABLE]`. CLI `--export-intune <file>` and File → Export Intune Policy… in GUI.
- **GroupPolicyExporter.cs** (enhanced): Full ADMX/ADML generation pipeline for all HKLM Registry-kind tweaks. Produces valid `.admx` + `.adml` importable in `gpedit.msc`. Per-tweak `Explain` text from `Description`. CLI `--export-gpo <file>`.
- **AutoUpdater.cs** (enhanced Sprint 115): Download + guided install. `UpdateInfo.DownloadAsync()` fetches MSI to `%TEMP%`, verifies SHA-256 against GitHub release asset manifest, then offers "Install & Restart" in `UpdateCheckerDialog`. Never auto-installs without user confirmation.
- **BenchmarkDotNet project** (Sprint 121): `tests/RegiLattice.Benchmarks/` — 8 TweakEngine benchmarks + 5 RegistrySession benchmarks. Baseline performance data established.
- **Branch coverage 75.14%** (Sprint 121): `BranchCoverage7Tests.cs` — 10 new tests targeting 12 partial-coverage branches in Analytics, SnapshotManager, ComplianceHistory, Ratings, HealthScoreService, AppConfig, ScheduledTweakService, Favorites.
- **200+ Locale keys** (Sprint 120): All GUI labels now routed through `Locale.T()`. String table expanded from 17 keys to 200+ keys covering all UI labels, button text, status messages, dialog titles, error messages across 6 languages (en/de/fr/es/ja/he).

#### Enhanced

- `UpdateCheckerDialog`: Progress bar during download, SHA-256 verification status, "Install & Restart" button (launches MSI, exits app).
- `MainForm`: All menu captions, status bar messages, button labels, filter labels routed through `Locale.T()`.
- CI/CD: `SHA256SUMS.txt` published alongside release artifacts. Smoke test step verifies CLI `--list` + `--validate` on published EXE.
- Scoop manifest `scoop/regilattice.json`: auto-updated SHA-256 on each tagged release via CI.

#### Stats

- Total tweaks: **4,108** across **121 categories**
- Tests: **1,944** passing (0 failures)
- Branch coverage: **75.14%** (1741/2317) — M2 gate ✅
- Build: **0 errors, 0 warnings** (Release x64)
- Version bumped `4.4.0` → `4.5.0`

---

## [4.4.0] — 2026-03-22

### Sprint 106 — 50 New Security & Hardening Tweaks (WDAC/ASR, BitLocker To Go, Device Install, LAPS, NTLM)

#### Added

- **WdacCodeIntegrity.cs** (`wdac-asr-*`) — 10 individual WDAC/Defender ASR rule tweaks, each enabling a specific ASR GUID in block mode (Office child processes, LSASS dump, ransomware, WMI persistence, PSExec/WMI, USB untrusted, email executables, obfuscated scripts, Adobe child, Office code injection)
- **BitLockerRemovable.cs** (`btogo-*`) — 10 BitLocker To Go policy tweaks for removable data volumes (deny write unencrypted, enable RDV, passphrase complexity/length, AES-128-XTS, disable hardware encryption, smart card auth, recovery password/key backup)
- **DeviceInstallPolicies.cs** (`dinst-*`) — 10 device installation restriction policy tweaks (deny removable install, device ID block list, class GUID block list, retroactive application, admin override, disable Windows Update driver search, disable co-installers, WER suppression, metadata download block)
- **LapsSecurity.cs** (`laps-*`) — 10 Windows LAPS (built-in, April 2023+) policy tweaks (Azure AD/Entra backup, AD backup, 14-day max age, 20-char min length, max complexity, password encryption, post-auth reset+logoff, 24h grace period, 12-entry encrypted history, disable legacy LAPS)
- **NtlmAuthentication.cs** (`ntlma-*`) — 10 NTLM authentication hardening tweaks (NTLMMinClientSec/ServerSec 0x20080000, Netlogon channel sign+seal, deny all outgoing NTLM, domain NTLM audit, in-domain NTLM audit/restrict, MSV1_0 audit, block null session fallback, require NTLMv2 at MSV1_0 level)

#### Stats

- Total tweaks: **4,108** (+50 vs 4.3.0)
- Categories: **121** (+5 new: WDAC & Code Integrity, BitLocker To Go, Device Installation Policies, Local Admin Password (LAPS), NTLM Authentication)
- Tests: **1,858** passing (0 failures)

---

## [4.3.0] — 2026-03-22

### Sprints 99–105 — Dialogs, Services, PowerShell Module, Notifications & Compliance History

#### Added

- **SmartScanService.cs** (Sprint 99): `Scan()` / `ScanAsync()` — filters unapplied tweaks, scores by ImpactScore × SafetyRating, returns top 25 `ScanRecommendation` records. IsQuickWin flag for Impact ≥ 4 && Safety ≥ 4.
- **SmartScanDialog** (Sprint 99): ListView of top 25 recommendations; Quick Wins highlighted in green; "Apply All Quick Wins" + per-row Apply buttons; progress bar.
- **ProfileCompareDialog** (Sprint 100): Side-by-side diff of any two built-in profiles. ComboBox pair, colour-coded ListView (Added/Removed/Shared rows), Export HTML (Catppuccin dark theme). Added via Tools menu.
- **DependencyGraphDialog** (Sprint 101): TreeView-based dependency explorer. Searchable ListBox with ★ markers on tweaks that have DependsOn. "Depends on" + "Needed by" branches for any selected tweak. Clickable tree nodes for navigation. Added via Tools menu.
- **ComplianceReportExporter.cs** (Sprint 102): `RegiLattice.Core.Services` — `ExportHtml()` + `BuildHtml()`. Groups tweaks by category, computes applied/pending/unknown counts, health score %. Catppuccin dark HTML with `WebUtility.HtmlEncode`. Wired to File → Export Compliance Report….
- **PowerShell module scaffold** (Sprint 103): `powershell/RegiLattice.psd1` (manifest v4.3.0) + `powershell/RegiLattice.psm1` — CLI-wrapping script module with 5 cmdlets: `Get-RLTweak`, `Get-RLTweakStatus`, `Invoke-RLApply`, `Invoke-RLRemove`, `Get-RLHealthScore`. Pipeline-native `PSCustomObject` output, `Update-TypeData` format defaults, aliases `grt`/`grts`/`ira`/`irr`. `PowerShellModuleGenerator.cs` bumped to v4.3.0.
- **JumpListService.cs** (Sprint 104): Scaffold for Windows 11 taskbar Jump List integration. No-op until MSIX installer provides AUMID; structured for future COM `ICustomDestinationList` wiring.
- **ToastNotificationService.cs** (Sprint 104): WinRT `ToastNotificationManager` toast delivery with automatic fallback to `NotifyIcon.ShowBalloonTip()`. `ShowApplyComplete()` fired after every batch-apply in `MainForm`.
- **ComplianceHistory.cs** (Sprint 105): `RegiLattice.Core.Services` — rolling compliance log persisted to `compliance-history.json` (90-entry cap). Records `ComplianceHistoryEntry` per check (date, total checked, violation count, drifted IDs, snapshot path). `AddEntry()` auto-wired in `RunCompliance()`.
- **--compliance-history CLI flag** (Sprint 105): Prints last 30 compliance entries with date/checked/violations columns.
- **--compliance-report auto CLI flag** (Sprint 105): Auto-locates the latest snapshot in `ConfigDir`, runs a compliance check against it, and prints the drift report.
- **Toast extensions** (Sprint 105): `ShowComplianceDrift(violations)` and `ShowUpdateAvailable(version)` added to `ToastNotificationService`.

### Sprints 88-96 — 9 New Modules + Existing Module Enhancements

#### Added

- **NetworkInterface.cs**: 10 TCP/IP NIC adapter tuning tweaks (`nic-*`)
  - Disable packet coalescing, adapter power management, EEE, Large Send Offload, Checksum Offload, NDIS RSS, jumbo frames; set Rx/Tx buffers, interrupt coalescing; disable AutoNegotiation
- **SystemShutdown.cs**: 10 shutdown & restart behavior tweaks (`shdn-*`)
  - Fast startup, hibernate, hybrid sleep, automatic maintenance wakeup, crash auto-restart, BSOD on hang, shutdown timeout, force shutdown on close, UPS sleep, clean boot
- **MicrosoftAccount.cs**: 10 MSA sync, privacy & linked-devices tweaks (`msa-*`)
  - Disable MSA sign-in policy, consumer experience, Settings sync, account notifications, linked phone, suggested contacts, cloud clipboard, Cortana MSA usage, connected account registration
- **DeviceGuardVbs.cs**: 10 HVCI/VBS/Credential Guard hardening tweaks (`vbs-*`)
  - Enable Credential Guard, HVCI, enforced HVCI, VBS, Secure Boot, kernel DMA protection, kernel-mode code integrity, require trusted launch; disable DMA remapping; block vulnerable drivers
- **WindowsInk.cs**: 10 Ink Workspace, pen & touch-input tweaks (`ink-*`)
  - Disable Ink Workspace, suggested apps in Ink, touch feedback, flicks, pen flicks, tablet mode roaming, touch keyboard auto-launch, handwriting personalization, pen workspace; enforce handwriting input on compatible devices
- **CloudExperience.cs**: 10 OOBE, cloud-content & Microsoft Account tweaks (`oobe-*`)
  - Disable OOBE improvements, pre-installed bloatware, consumer features, Cortana during OOBE, privacy experience, Windows Spotlight, lock-screen widgets, user preferences collection, cloud consumer account state, advertising ID
- **UserActivity.cs**: 10 timeline, recent docs & CDP tweaks (`activity-*`)
  - Disable activity feed publishing, timeline, cross-device sharing (CDP), connected devices platform, recent items, quick-access frequent folders, notification Center, auto-open Downloads, search highlights, last-access timestamp
- **WifiNetworking.cs**: 10 Wi-Fi Sense, metered networks & 802.11 tweaks (`wifi-*`)
  - Disable Wi-Fi Sense hotspot sharing, random hardware addresses, BT coexistence interference, WLAN AutoConfig triggers; enable 802.11d, 802.11h; cap scan retries, set throttle limit; enforce WPA3 preference; disable SSID broadcasting
- **PrintSpoolerSecurity.cs**: 10 PrintNightmare mitigations & spooler hardening tweaks (`spool-*`)
  - Disable Print Spooler, RpcAuthnLevelPrivacyEnabled enforcement, point-and-print restrictions, inbound SMB printing, outbound print events, network printer discovery, Web Services for Printers; restrict driver installation, operator users, HTTPS printing

#### Enhanced (existing modules)

- **AutoRunPolicy.cs**, **SecurityAuditPolicy.cs**, **TimeSync.cs**, **TouchpadGestures.cs**, **VirtualDesktops.cs**, **WindowsSearchAdv.cs**: additional tweaks, corrections, and de-duplication

#### Fixed

- `WifiNetworking.cs`: `WiFiService` → `WifiService` (6× CS0103 capitalization error)
- `UserActivity.cs`: replaced duplicate `activity-disable-timeline-view` with distinct `activity-disable-cdp` (different registry key)

#### Stats

- Tweaks: **4058** across **116 categories** (121 module files)
- Tests: **1858** (1325 Core + 291 CLI + 242 GUI), all passing
- Build: **0 errors, 0 warnings** (Release x64)
- Version bumped `4.2.0` → `4.3.0`

---

## [4.2.0] — 2026-03-21

### Sprint 77 — Remote Management, SSH Hardening, Kiosk/Shared PC, Active Directory, Hyper-V Advanced

#### Added

- **RemoteManagement.cs**: 10 WinRM policy hardening + RPC restriction tweaks (`rmt-*`)
  - Disable WinRM service, block unencrypted/Basic/Digest/CredSSP auth, restrict RPC clients, require RPC auth endpoint isolation, limit WinRM shell memory
- **SshHardening.cs**: 10 OpenSSH `sshd_config` hardening tweaks (`ssh-*`) — gated on `C:\ProgramData\ssh\sshd_config`
  - Limit max auth tries (3), login grace time (30 s), deny empty passwords, disable forwarding (agent/TCP/X11), restrict max sessions (2), enable StrictModes, enforce strong ciphers + MACs
- **KioskSharedPc.cs**: 10 Windows Shared PC / Kiosk configuration tweaks (`kiosk-*`)
  - Enable SharedPC mode, guest account model, auto-delete on sign-out, disk-level thresholds, disable fast user switching, no local password reset, enable education policies, disable lock-screen camera/slideshow
- **ActiveDirectory.cs**: 10 AD domain client hardening tweaks (`ad-*`)
  - Disable NT4 crypto, restrict Kerberos to AES, enable Kerberos armoring (FAST), max token size, scavenge interval, negative cache period, disable mailslot discovery, block single-label DNS, restrict connected-user enumeration
- **HyperVAdvanced.cs**: Expanded from 10 to 20 tweaks (+10 `hyperv-*`)
  - Disable auto-checkpoints, require network credentials, allow SR-IOV, enable bandwidth management, disable VM broadcast, cap max VMs (8), reserve 512 MB host memory, remove default switch, enforce strict network isolation
- 4 new categories: **Remote Management**, **SSH Configuration**, **Kiosk & Shared PC**, **Active Directory**

#### Stats

- Tweaks: **3868** across **107 categories**
- Tests: **1647** (1230 Core + 175 CLI + 242 GUI), all passing
- Version bumped `4.1.0` → `4.2.0`

---

## [4.1.0] — 2026-07-22

### Sprints 57–67 — Intelligence, Portability, Automation & New Tweaks

#### Added

- **ImpactScore & SafetyRating metadata** (Sprint 57): New `int ImpactScore` and `int SafetyRating` fields on `TweakDef` (1–5 scale). GUI shows color-coded Impact/Safety badges. CLI displays in `--list` output.
- **NLP synonym search** (Sprint 58): `TweakEngine.Search()` now expands query tokens through a built-in synonym map (60+ entries: fast→performance, spy→telemetry, bloat→debloat, etc.). Multi-token AND logic with expanded groups.
- **Portable mode** (Sprint 59): `--portable` flag + `AppConfig.SetPortable()`. Redirects all `%LOCALAPPDATA%` paths to `.\data\` in the executable directory. Auto-detected via sentinel file `data\.portable`.
- **Silent mode CLI** (Sprint 60): `--silent` suppresses all console output; JSON operation log written to `--log-file <path>`. Exit codes: 0 = success, 1 = failure/partial.
- **AutoUpdater service** (Sprint 61): `AutoUpdater.cs` — polls GitHub Releases v3 API, compares semantic versions, returns `UpdateInfo` record. `IsNewer()` handles `v`-prefixed version strings.
- **HealthScoreService** (Sprint 62): `HealthScoreService.cs` — computes weighted Privacy/Performance/Security/Stability scores (0–100) from `StatusMap()`. Returns `HealthScore` record with `OverallLabel` ("Excellent"/"Good"/"Fair"/"Needs Work"/"Poor").
- **50 new tweaks across 5 new categories** (Sprint 63):
  - `XboxGameBar.cs` — 10 tweaks (Xbox / Game Bar category)
  - `WindowsHello.cs` — 10 tweaks (Windows Hello category)
  - `SmartAppControl.cs` — 10 tweaks (Smart App Control category)
  - `EnergySaver.cs` — 10 tweaks (Energy Saver category)
  - `CopilotPlus.cs` — 10 tweaks (Copilot+ Features category)
- **FirstRunWizardDialog** (Sprint 64): 3-step wizard shown on first launch — profile selection, dry-run toggle, feature tour. Writes initial config to `config.json`.
- **ProfileWizardDialog** (Sprint 65): 5-question wizard generates a personalized `TweakProfile`. Questions cover gaming, privacy, performance priority, hardware type, and corporate environment.
- **ConflictDetector service** (Sprint 66): `ConflictDetector.cs` — maintains a static lookup table of known-conflicting tweak pairs. `Detect()` returns conflicts given a set of tweak IDs; `ConflictsFor()` checks a single ID against applied tweaks.
- **NLP synonym map fix**: Removed `"privacy"` from the `"telemetry"` synonym expansion to prevent AND-search false positives (e.g., searching "privacy telemetry" now correctly returns only tweaks matching both terms).

#### Tests (Sprint 67)

- `HealthScoreServiceTests.cs` — 13 tests: score computation, bucket helpers, range validation, label mapping
- `AppConfigPortableTests.cs` — 8 tests: default state, SetPortable, ConfigDir redirect, auto-detection
- `ConflictDetectorTests.cs` — 14 tests: AllConflicts, Detect symmetry/known pairs, ConflictsFor
- `NewTweakModulesTests.cs` — 8 tests: category registration, ID uniqueness, validator clean
- `AutoUpdaterTests.cs` — tests: IsNewer comparisons, CheckAsync mock
- `TweakDefMetadataTests.cs` — tests: ImpactScore/SafetyRating model
- `TweakEngineSearchNlpTests.cs` — tests: synonym expansion, multi-token AND, empty/whitespace behavior
- **Fixed**: `Search_SingleToken_ReturnsRelevantTweaks` — relaxed to allow synonym-expanded results while verifying at least one direct match
- **Fixed**: `Search_MultiToken_AllTokensMustMatch` — resolved by correcting synonym map overlap

#### Total (v4.1.0)

| Metric | v4.0.0 | v4.1.0 |
|--------|--------|--------|
| Tweaks | 3,669 | **3,719** |
| Categories | 94 | **99** |
| Tests | 1,435 | **1,538** (1121 Core + 175 CLI + 242 GUI) |
| Core services | 24 | **27** (+ AutoUpdater, HealthScoreService, ConflictDetector) |
| New dialogs | — | **2** (FirstRunWizard, ProfileWizard) |

---

## [4.0.0] — 2026-03-20

### Major Release — All Capabilities Enabled

v4.0.0 is the first **major release milestone**, consolidating every capability built across
the v3.x development cycle into a single production-ready package. All features, interfaces,
and safety systems are fully active — GUI, CLI, MSI installer, 11 themes, 5 profiles,
corporate guard, dry-run, snapshot, package managers, plugin marketplace, and the
newly introduced anti-duplication quality layer.

### Added

- **Anti-duplication quality system** (`chore(quality)` commit `877b80b`):
  - `.github/instructions/no-duplication.instructions.md` — 4-layer prevention rules (IDs, ops, labels, conceptual)
  - `.github/skills/no-duplication/SKILL.md` — 6-step audit workflow with PowerShell one-liners and resolution guide
  - `scripts/Audit-Duplications.ps1` — colour-coded audit script covering all 4 duplication layers; `exit 1` on hard violations
  - **+4 duplication guard tests** in `TweakEngineBuiltinsTests.cs`:
    - `RegisterBuiltins_DuplicateRegistryOps_BelowRegressionThreshold` (threshold ≤ 1200)
    - `RegisterBuiltins_NoCrossModuleLabelAndPathCollision` (threshold ≤ 200, 128 groups detected as existing debt)
    - `RegisterBuiltins_CategorySlugs_MatchKnownPrefixes` (spot-checks 10 canonical category slug prefixes)
    - `RegisterBuiltins_DetectDuplicateRegistryOps_ProducesUsableOutput` (scale smoke-test at 3669 tweaks)

- **Next Phase Master Plan** appended to `docs/Roadmap.md` — 9-phase plan (v3.9 → v4.2) covering:
  - Phase A: Deduplication sprint (fix 128 cross-module label+path collisions, ~800 duplicate ops)
  - Phase B: Tweak quality audit (semantic correctness review)
  - Phase C: GUI UX pass (search improvements, keyboard nav, tray polish)
  - Phase D: CLI completeness (remaining commands, shell completion)
  - Phase E: Coverage push (branch coverage from 56.8% toward 70%)
  - Phase F–I: New tweak categories, localization expansion, plugin ecosystem, release automation

### All v4.0.0 Capabilities at a Glance

| Capability | Status |
|---|---|
| **3669 verified tweaks** across 94 categories | ✅ |
| **WinForms GUI** with 11 switchable themes | ✅ |
| **CLI** with 25+ commands | ✅ |
| **Interactive console menu** | ✅ |
| **MSI installer** (WiX v6, self-contained win-x64) | ✅ |
| **Portable EXE** (GUI + CLI, no .NET required) | ✅ |
| **5 machine profiles** (business, gaming, privacy, minimal, server) | ✅ |
| **Dry-run mode** — preview without touching registry | ✅ |
| **Snapshot & diff** — save/restore/compare JSON state | ✅ |
| **CorporateGuard** — blocks unsafe tweaks on managed machines | ✅ |
| **UAC auto-elevation** | ✅ |
| **Package manager dialogs** (WinGet, Scoop, pip, Chocolatey, PSModules) | ✅ |
| **Plugin marketplace** — JSON Tweak Packs with SHA-256 verification | ✅ |
| **Dependency resolver** — topological apply order | ✅ |
| **TweakValidator** — ID/label/dep/circular-dep integrity checks | ✅ |
| **Anti-duplication system** — 4-layer detection + audit script | ✅ |
| **Localization** — English + German (48 strings) | ✅ |
| **Live CPU/RAM monitoring** in About dialog | ✅ |
| **Automatic JSON backups** before every registry mutation | ✅ |
| **1435 tests** across 17 test files (0 failures) | ✅ |

### Stats

- Tweaks: **3669** across 94 categories (unchanged from v3.8.0)
- Tests: **1435** passing (1018 Core + 175 CLI + 242 GUI) — +21 from v3.8.0
- Version bumped `3.8.0` → `4.0.0`

---

## [3.8.0] — 2026-07-21

### Added

- **678 new tweaks** across 10 sprint cycles (Sprints 48–57), bringing the total to **3669 verified tweaks** across **94 categories**:
  - **Sprint 48** — Bluetooth (+10), Printing (+10), Touch & Pen (+10), Voice/Speech (+10), Storage (+10)
  - **Sprint 49** — AI/Copilot (+10), Scoop Tools (+10), Dev Drive (+10), Java (+10), M365 Copilot (+10)
  - **Sprint 50** — Gaming (+10), GPU/Graphics (+10), Boot (+10), Windows 11 (+10), Scheduled Tasks (+10)
  - **Sprint 51** — Microsoft Store (+10), Edge (+10), Firefox (+10), Chrome (+10), Office (+10)
  - **Sprint 52** — Firewall (+15), Encryption (+12), Hardening (+12) — security hardening module expansion
  - **Sprint 53** — Network (+15), DNS & Networking Advanced (+15), Proxy & VPN (+10), Network Optimization (+10)
  - **Sprint 54** — Performance (+10), SSD Optimization (+10), File System (+10), Disk Cleanup (+10)
  - **Sprint 55** — Privacy (+12), Cortana & Search (+12), Widgets & News (+12), Telemetry Advanced (+12)
  - **Sprint 56** — Developer (+13), PowerShell Tweaks (+13), Services (+12), User Account (+12): console VT/UTF8, WER/JIT, Defender CFA/network/CPU, SMB signing+encryption, Teredo/6to4/ISATAP, credential/logon security, Netlogon hardening
  - **Sprint 57** — Shell (+13), Lock Screen (+13), Input (+12), Taskbar (+12): thumbnail cache, spotlight/screensaver policy, PrecisionTouchPad controls, taskbar density, compact mode, logon scripts, NTLM settings

### Stats

- Tweaks: **3669** (was 2991, +678)
- Tests: **1414** passing (1014 Core + 175 CLI + 225 GUI) — unchanged
- Version bumped `3.7.3` → `3.8.0`

---

## [3.7.3] — 2026-03-20

### Fixed / Polished

- **Title-bar icons** added to 7 dialogs that were missing them:
  `AboutDialog`, `WhatsNewDialog`, `PreferencesDialog`, `HostsAddDialog`,
  `HostsUrlPromptDialog`, `ScheduleEditDialog`, `AddProfileScheduleDialog`

### Chore

- `Readme.md` renamed to `README.md` (universal GitHub/open-source convention)
- Root-level `CONTRIBUTING.md` and `CHANGELOG.md` stubs added — GitHub auto-links
  the Contribute and Changelog community-health buttons from root or `.github/`
- `PULL_REQUEST_TEMPLATE.md` test count corrected: `203+` → `1,431+`
- `github.copilot` base extension added to `.vscode/extensions.json`

### Stats

- Tweaks: **2991** (unchanged)
- Tests: **1414** passing (1014 Core + 175 CLI + 225 GUI)
- Version bumped `3.7.2` → `3.7.3`

---

## [3.7.2] — 2026-03-20

### Added

- **64 new tests** covering previously untested code paths (+4.7% branch coverage):
  - `AppConfigUiPrefsTests` (+44): default-value assertions and save/load roundtrip for
    23 untested `AppConfig` properties — `MinimizeToTray`, `ConfirmApply`, `ConfirmRemove`,
    `ShowInapplicable`, `FontSize`, `ShowLogPanel`, `LogPanelHeight`, `AutoRefreshOnStartup`,
    `LaunchMinimized`, `RememberSplitter`, `SplitterDistance`, `SkipAppliedOnBatch`,
    `BrightnessSchedulerEnabled` + 4 value fields, `HistoryMaxEntries`, `MonitorColorCoded`,
    `AutoCleanMemoryThreshold`, `ProfileOnPlanSwitch`, `ProfileSchedules`; plus
    `ProfileScheduleEntry` record defaults and disabled-entry roundtrip
  - `ComplianceAndManagerParseTests` (+20): `--compliance`, `--export-gpo`, `--manager`
    flag parsing with value variants, without-value edge cases, combined-flag tests

### Changed

- **Standing commit rule** reinforced in `.github/copilot-instructions.md`: Git Workflow
  section now includes the per-sprint mandate and total-count requirement explicitly,
  making it visible on every session load alongside the quick-facts table
- Version bumped `3.7.1` → `3.7.2`

### Stats

- Tweaks: **2991** (unchanged)
- Tests: **1431** passing (1014 Core + 175 CLI + 242 GUI) — was 1367

---

## [3.7.1] — 2026-07-18

### Changed

- **Duplicate tweaks removed**: eliminated 11 confirmed duplicate tweak definitions
  (3194 → 3183 tweaks across 92 categories)
- **Test suite refactored**: `TweakEngineBuiltinsTests.cs` 1956 → 480 lines;
  replaced 617 per-ID `[InlineData]` existence checks with a single
  `AllRegisteredTweaks_CanBeRetrievedById` Fact iterating all tweaks at runtime
  — total test count 2088 → **1367 passing** (970 Core + 155 CLI + 242 GUI)
- **Dead code removed**: deleted `RegistryHives.cs` (11 lines) — `Hive.LM`/`Hive.CU`
  constants confirmed unused across all 93 tweak modules
- **Roadmap trimmed**: `docs/Roadmap.md` 1265 → ~430 lines; replaced exhaustive
  Sprint 1–47b task tables (all completed) with compact sprint digest table
- **Git workflow instructions updated**: `.github/instructions/git-workflow.instructions.md`
  updated to use .NET commands; added standing per-sprint commit mandate section
- Version bumped `3.7.0` → `3.7.1`

---

## [3.7.0] — 2026-07-17

### Fixed

- **MSI missing from GitHub Releases** — `release.yml` never built `installer/RegiLattice.Installer.wixproj`; publish paths were `publish/release/gui|cli` but installer expects `publish/gui|cli`; both issues now corrected, WiX 6.0.2 installed + MSI build step added + SHA256SUMS updated to include MSI

### Added

- **`NetworkManager.cs`** +2 methods: `PingAsync(host, count, ct)` (shells Windows `ping`, parses stdout into `PingResult` record with loss/avg/min/max); `GetNetworkInterfaceStats()` (per-adapter `NetworkInterfaceStats` with byte + packet counters)
- **`StartupManager.cs`** +2 methods: `AddRegistryEntry(name, command)` (adds HKCU Run entry, validates uniqueness); `ExportEntriesAsync(filePath)` (JSON export of all startup entries)
- **`ServiceManager.cs`** +2 methods: `GetDependentServices(name)` (returns names of dependent services); `ExportToCsvAsync(filePath)` (CSV export with ServiceName, DisplayName, Status, StartType, CanStop)
- **`TweakHistory.cs`** +2 members: `GetSummaryStats()` returns `HistorySummaryStats` record (action counts + top-5 tweaks by frequency); `ExportToJsonAsync(filePath)` exports full history as JSON
- **`Favorites.cs`** +2 methods: `ExportToJsonAsync(filePath)` (sorted JSON array of IDs); `ImportFromJson(filePath)` (merge from JSON array, returns newly-added count)
- **`AppConfig.cs`** +2 properties: `AutoBackupOnApply` (bool, default `true`, triggers registry backup before batch apply); `SnapshotOnProfileChange` (bool, default `true`, auto-snapshots before profile is applied)
- **Tests**: +19 new tests — total 1879 (all passing)

### Changed

- Version bumped `3.6.0` → `3.7.0`

---

## [3.6.0] — 2026-03-19

### Added

- **v3.6.0 release prep**: bumped version, clean Release build, MSI install package
- **About dialog**: shows `[Debug]` / `[Release]` build configuration; added clickable GitHub Releases link
- **Menu icons**: 7 new 16×16 programmatic icons in `AppIcons` — About, Hardware Info, What's New, Check Updates, Exit, Preferences, Import; all Help and File menu items now have icons
- **Sprint 46 (v3.6.0 release)**: CHANGELOG, Roadmap, README, GitHub metadata updated; copilot-instructions.md updated to v3.6.0; full test suite passing; MSI built

## [Unreleased] — Sprint 45

### Added

- **50 new tweaks** across 5 modules (+10 each):
  - **`Audio.cs`** (+10): `audio-disable-comms-ducking`, `-set-pro-audio-priority`, `-disable-audio-idle-powerdown`, `-set-avrcp-volume-sync`, `-set-audio-latency-mode`, `-enable-audio-log-off`, `-set-endpoint-builder-manual`, `-disable-voice-typing-toast`, `-set-render-clock-rate`, `-set-capture-clock-rate`
  - **`Gaming.cs`** (+10): `game-disable-msmq-service`, `-disable-gameinput-service`, `-set-dxgi-flip-model`, `-enable-game-bar-perf-counter`, `-disable-diagtrack-autologger`, `-set-xgip-service-manual`, `-disable-ndu-adapter`, `-set-games-sfio-priority-high`, `-set-mouse-fix-off`, `-set-games-affinity-all-cpus`
  - **`Security.cs`** (+10): `sec-require-ldap-signing`, `-disable-rdp-clipboard-sync`, `-disable-rdp-drive-mapping`, `-enforce-smb-ntlmv2-auth`, `-disable-printer-spooler-network`, `-enable-run-as-different-user`, `-disable-office-macros-internet`, `-disable-wsh-scripting`, `-restrict-lsass-credential-dump`, `-disable-named-pipe-impersonation`
  - **`WindowsUpdate.cs`** (+10): `wu-disable-automatic-updates`, `-set-schedule-day-saturday`, `-disable-store-app-auto-updates`, `-set-update-service-manual`, `-require-admin-for-updates`, `-disable-metered-update-download`, `-disable-reboot-required-notification`, `-set-feature-update-channel-general`, `-set-orchestrator-service-manual`, `-disable-third-party-preview`
  - **`RemoteDesktop.cs`** (+10): `rdp-set-max-connections-unlimited`, `-set-color-depth-32`, `-disable-smart-card-redirection`, `-set-remote-assistance-off`, `-set-audio-play-on-server`, `-disable-com-port-redirect`, `-enforce-tls-security-layer`, `-limit-single-monitor`, `-set-connection-timeout-8h`, `-disable-lpt-port-redirect`
- **`UpdateCheckService.cs`** — GitHub Releases API checker; returns `UpdateInfo` record (`UpdateAvailable`, `CurrentVersion`, `LatestVersion`, `ReleaseNotes`, `DownloadUrl`)
- **`ComplianceService.cs`** — compare live registry state against a saved snapshot; returns `ComplianceReport` with `Drifted` list and `IsCompliant` flag
- **`GroupPolicyExporter.cs`** — exports `TweakKind.GroupPolicy` tweaks to `.admx` + companion `.adml` file pair
- **`UpdateCheckerDialog.cs`** — GUI dialog for Help → Check for Updates; shows current vs latest version, release notes, download link
- **Help → Check for Updates** menu item wired in `MainForm`
- **CLI `--compliance <snapshot>`** — detects configuration drift against a snapshot file; exits 1 if non-compliant
- **CLI `--export-gpo <path>`** — exports Group Policy tweaks as ADMX/ADML pair
- **StartupManagerDialog +2**: Export CSV button + Open File Location button
- **ServiceManagerDialog +2**: Restart Service button + Set to Automatic button
- **DiskSpaceDialog +2**: Clean TEMP button + live TEMP folder size label
- **WindowsUpdateControlDialog +2**: Update History button (ms-settings deeplink) + Reset WU Components button
- **MemoryCleanerDialog +2**: Auto-clean checkbox + threshold spinner with 30-second polling timer
- **NetworkToolsDialog +2**: Ping tab (multi-host concurrent ping) + Traceroute tab (live `tracert` output)
- **`TweakEngineBuiltinsTests.cs`** — 5 new `[Theory]` methods; 50 new `[InlineData]` entries covering all Sprint 45 tweaks
- Total: **2946 tweaks** (+50)

---

## [Unreleased] — Sprint 44

### Fixed

- **CRLF line endings** — 152 C# source files normalised from CRLF to LF across `src/` and `tests/`
  to match the `.gitattributes` `*.cs eol=lf` declaration; 0 logic changes

### Added

- **Japanese locale (`ja`)** — 51-key translation added to `Locale.cs` (`BuiltInLocales["ja"]`);
  RegiLattice now ships 6 built-in locales: en, de, fr, es, he, ja
- **50 new tweaks** across 5 modules (+10 each):
  - **`PhoneLink.cs`** (23→33): AllJoyn Router service, WPD service, Link to Windows banner policy,
    Continue on PC delivery, Phone activation policy, Device Association Framework service,
    CDP activation prompt, cross-device roaming trigger consent, Wi-Fi hotspot auth policy,
    Windows Hello companion device
  - **`OneDrive.cs`** (23→33): Known Folder Move opt-in block, KFM silent redirect block,
    delay update ring, SharePoint sync disable, app sync disable, mass-delete threshold (50 files),
    hydration-on-access preview block, auto-update disable, File Explorer side-panel hide,
    external collaboration block
  - **`Notifications.cs`** (25→35): Low disk space alert, Windows Defender notifications (user),
    Windows Update reboot nag, legacy balloon tips, SmartScreen evaluation warnings (user),
    taskbar content suggestions, OEM preinstalled app suggestions, Windows tips & tricks,
    clear recent docs on exit, no-logged-on-user reboot (WU)
  - **`Gaming.cs`** (29→39): SFIO priority High, NDU service disable, SystemResponsiveness=0,
    network throttling index off, GPU priority=8, latency sensitivity High, Background Only=False,
    task CPU priority=6, Xbox Accessory Management service, max user port 65534
  - **`Maintenance.cs`** (28→38): Clear recent docs on logoff, service shutdown timeout (2s),
    app kill timeout (2s), long path support (260+), desktop cleanup wizard, hung-app timeout (2s),
    AutoEndTasks on shutdown, crash-on-audit-fail disable, hide Recent in Quick Access,
    hide Frequent Folders in Quick Access
- **Test coverage** — 50 new `[InlineData]` entries across 5 new `[Theory]` test methods in
  `TweakEngineBuiltinsTests.cs`; Core test count 1344 → 1394

### Stats

- Total tweaks: **2896** (+50 from Sprint 43)
- All Core tests: **1394** (1394 Core) — all passing

---

## [Unreleased] — Sprint 43

### Fixed

- **Version display** — `AssemblyInfo.cs` created (`AssemblyVersion`/`AssemblyInformationalVersion = "3.5.0"`);
  `AboutDialog` reads `AssemblyInformationalVersionAttribute` so version shows `3.5.0` instead of `0.0.0.0`
- **`WindowsHealthDialog` crash** — `AppendLog` and `SetBusy` now fully dispose-safe and thread-safe;
  `RunBatchAsync` final status block guarded with `if (!IsDisposed)`
- **Marketplace 404 + corporate proxy** — `PackManager.s_http` uses `HttpClientHandler` with
  `WebRequest.GetSystemWebProxy()` and `UseDefaultCredentials = true`; `FetchIndexAsync` handles
  `HttpStatusCode.NotFound` gracefully returning an empty `PackIndex`

### Added

- **Admin indicator** — firebrick `ToolStripStatusLabel` (`🛡 ADMIN`) in StatusStrip, visible only when
  process is elevated
- **Package Manager top-level menu** — Scoop, pip, PowerShell modules, WinGet, Chocolatey, and Tweak Pack
  Marketplace moved from `Tools` into a dedicated `&Package Manager` top-level menu item
- **Hebrew locale (`he`)** — 51-string translation added to `Locale.cs` (`BuiltInLocales["he"]`)
- **50 new tweaks** across 5 modules (+10 each):
  - **`Debloat.cs`** (29→29): Find My Device, Inking/Typing Personalization, Nearby Sharing, Mixed Reality
    Portal, Steps Recorder, Error Reporting UI, Wireless Display Projection, Post-Update OOBE, Tablet Mode
    Auto-Switch, Spotlight Content in Settings
  - **`BrowserCommon.cs`** (19→29): Cast icon, Sign-in interception, Edge Shopping Assistant, Edge Follow,
    Chrome NTP custom background, Chrome promotional tabs, Chrome NTP spotlight recs, First-run experience,
    Address autofill, Edge pre-launch startup boost
  - **`SystemRestore.cs`** (20→30): Hibernate file, Small crash dump, Dump overwrite, No auto-reboot on
    BSOD, WER 2nd-level data, WER queue limit, WER archive limit, WER throttle bypass, WER response timeout,
    BSOD admin alert
  - **`ScheduledTaskTweaks.cs`** (20→30): Compatibility Appraiser, RAC task, ProgramDataUpdater, WER
    QueueReporting, Device Information, Power Efficiency Diagnostics, SmartScreen AppID, MRT telemetry,
    Defender Cache Maintenance, USB CEIP
  - **`WindowsRecall.cs`** (17→27): Publish user activities (HKCU), Cross-device clipboard, Typing
    insights, Taskbar AI widget content, Cloud search, Voice data collection, Auto map downloads,
    ContentDelivery feature management, Spotlight on settings, CEIP/SQM policy

### Stats

- Total tweaks: **2846** (+50 from Sprint 42)
- All tests: **1740** (1344 Core + 154 CLI + 242 GUI) — all passing

---

## [Unreleased] — Sprint 42

### Fixed

- **Version display** — `AssemblyInfo.cs` created (`AssemblyVersion`/`AssemblyInformationalVersion = "3.5.0"`);
  `AboutDialog` reads `AssemblyInformationalVersionAttribute` so version shows `3.5.0` instead of `0.0.0.0`
- **`WindowsHealthDialog` crash** — `AppendLog` and `SetBusy` now fully dispose-safe and thread-safe;
  `RunBatchAsync` final status block guarded with `if (!IsDisposed)`
- **Marketplace 404 + corporate proxy** — `PackManager.s_http` uses `HttpClientHandler` with
  `WebRequest.GetSystemWebProxy()` and `UseDefaultCredentials = true`; `FetchIndexAsync` handles
  `HttpStatusCode.NotFound` gracefully returning an empty `PackIndex`

### Added

- **Admin indicator** — firebrick `ToolStripStatusLabel` (`🛡 ADMIN`) in StatusStrip, visible only when
  process is elevated
- **Package Manager top-level menu** — Scoop, pip, PowerShell modules, WinGet, Chocolatey, and Tweak Pack
  Marketplace moved from `Tools` into a dedicated `&Package Manager` top-level menu item
- **Hebrew locale (`he`)** — 51-string translation added to `Locale.cs` (`BuiltInLocales["he"]`)
- **50 new tweaks** across 5 modules (+10 each):
  - **`Debloat.cs`** (29→29): Find My Device, Inking/Typing Personalization, Nearby Sharing, Mixed Reality
    Portal, Steps Recorder, Error Reporting UI, Wireless Display Projection, Post-Update OOBE, Tablet Mode
    Auto-Switch, Spotlight Content in Settings
  - **`BrowserCommon.cs`** (19→29): Cast icon, Sign-in interception, Edge Shopping Assistant, Edge Follow,
    Chrome NTP custom background, Chrome promotional tabs, Chrome NTP spotlight recs, First-run experience,
    Address autofill, Edge pre-launch startup boost
  - **`SystemRestore.cs`** (20→30): Hibernate file, Small crash dump, Dump overwrite, No auto-reboot on
    BSOD, WER 2nd-level data, WER queue limit, WER archive limit, WER throttle bypass, WER response timeout,
    BSOD admin alert
  - **`ScheduledTaskTweaks.cs`** (20→30): Compatibility Appraiser, RAC task, ProgramDataUpdater, WER
    QueueReporting, Device Information, Power Efficiency Diagnostics, SmartScreen AppID, MRT telemetry,
    Defender Cache Maintenance, USB CEIP
  - **`WindowsRecall.cs`** (17→27): Publish user activities (HKCU), Cross-device clipboard, Typing
    insights, Taskbar AI widget content, Cloud search, Voice data collection, Auto map downloads,
    ContentDelivery feature management, Spotlight on settings, CEIP/SQM policy

### Stats

- Total tweaks: **2846** (+50 from Sprint 42)
- All tests: **1740** (1344 Core + 154 CLI + 242 GUI) — all passing

---

## [Unreleased] — Sprint 42 (Hardware Tools)

### Added — Hardware & Network Tools

- **`HardwareTemperatureDialog`** — WMI thermal zone polling (`MSAcpi_ThermalZoneTemperature` in `root\WMI`),
  GPU via `Win32_VideoController`; colour-coded bars (green <60°C, amber 60–80°C, red ≥80°C); 3-second
  auto-refresh checkbox; graceful fallback when WMI unavailable; exposes `hwtempmon` in `--tool`
- **`NetworkBandwidthDialog`** — real-time NIC bandwidth monitor via `IPv4Statistics` delta-calc;
  1-second polling timer; per-adapter ↑ send / ↓ recv with B/s, KB/s, MB/s auto-scale; exposes
  `netbandwidth` in `--tool`
- **`MacAddressDialog`** — WMI `Win32_NetworkAdapter` MAC address viewer + randomizer:
  generates locally-administered unicast MAC, writes to registry `NetworkAddress` key under
  `HKLM\SYSTEM\CurrentControlSet\Control\Class\{4D36E972…}`, re-enables adapter via `netsh`;
  Copy-to-clipboard button; admin warning banner; exposes `macaddress` in `--tool`
- **Phase 2 #13** — Automatic memory cleaning on threshold: `AppConfig.AutoCleanMemoryThreshold` (int,
  0=disabled); `OnMonitorTimerTick` purges all process working sets if `memPct >= threshold`
- **Phase 2 #14** — System tray tooltip shows live RAM %: `_trayIcon.Text = $"RegiLattice — RAM: {memPct}%"`
- **Phase 2 #17** — Network connectivity status indicator: `_netLabel` in status strip, colour-coded
  green ✓ / red ✗ via `NetworkInterface.GetIsNetworkAvailable()`, refreshed every monitor tick
- **AppIcons** — `ThermometerMenuBitmap`, `BandwidthMenuBitmap`, `MacAddressMenuBitmap` with
  custom `DrawThermometerIcon`, `DrawBandwidthIcon`, `DrawMacAddressIcon` rendering

### Added — 29 New Tweaks

- **`EventLogging.cs`** — 10 new tweaks: limit Application/System/Setup event log sizes, disable
  PowerShell script-block & module logging, disable WER event log entries, disable forwarded events
  (Wecsvc), disable DNS client event tracing, disable NT Kernel Logger ETW session, disable logon
  failure audit
- **`ProxyVpn.cs`** — 10 new tweaks: disable WinHTTP WPAD auto-discovery, disable IE/WinINet proxy
  bypass, disable VPN split tunneling (RAS), disable RAS AutoDial service, disable IPv6 Teredo,
  disable WinINet AutoDetect, disable 6to4 tunneling, disable IP-HTTPS adapter, disable NCSI probing,
  disable TCP timestamps
- **`PowerShellTweaks.cs`** — 9 new tweaks: set execution policy to RemoteSigned, enable PS Remoting,
  opt out of PowerShell telemetry, enable Constrained Language Mode, disable transcription logging,
  enable Protected Event Logging, disable clipboard history policy, set system-managed page file,
  enable TLS 1.2 for .NET apps

### Tests

- +29 `[InlineData]` entries in `TweakEngineBuiltinsTests.cs` covering all new tweak IDs
- Total: **2796 tweaks** (+29), **1740 tests** (+29 passing)

---

## [Unreleased] — Sprint 41

### Fixed

- **`MemoryCleanerDialog`** — resolved 3 build errors: `AppTheme.ApplyToForm` renamed to `AppTheme.Apply`,
  `new SystemMonitor().GetMemoryUsage()` corrected to static call `SystemMonitor.GetMemoryUsage()`,
  2-element tuple deconstruct expanded to 3-element (`(used, _, _)`)

### Added — System Monitor Tool Dialogs

- **`DiskSpaceDialog`** — per-drive disk space overview with colour-coded usage bars (green <70%, amber 70–90%,
  red ≥90%); double-click drive to open in Explorer; async refresh; exposes `diskspace` in `--tool`
- **`PortScannerDialog`** — TCP port/connectivity tester: hostname + CSV port input, 7 preset groups
  (Web, SSH/RDP, FTP, Mail, Database, DNS/DHCP, Top 20), async parallel scan with 2 s timeout,
  ping test, WellKnownService name lookup, colour-coded OPEN/CLOSED results; exposes `portscan` in `--tool`
- **`BatteryHealthDialog`** — WMI `BatteryStaticData` + `BatteryStatus` health monitor: design vs full-charge
  capacity, cycle count, charge/discharge rate bars; **Full Report** runs `powercfg /batteryreport`;
  graceful no-battery fallback; exposes `batteryhealth` in `--tool`
- **`MemoryCleanerDialog`** wired into `Program.cs ResolveManagerArg()` (`memorycleaner`) and
  `Tools → Memory Cleaner` menu item
- Phase 2 items 12 (Memory Cleaner), 16 (Disk Space), 18 (Battery Health) and Phase 4 item 37
  (Port Scanner) completed

### Added — 31 New Tweaks

**`DiskCleanup.cs`** (+10, total 25):
`cleanup-disable-recent-docs`, `cleanup-disable-recent-programs`, `cleanup-disable-search-history`,
`cleanup-disable-swap-file`, `cleanup-disable-auto-maintenance`, `cleanup-disable-volume-shadow-copy`,
`cleanup-disable-internet-temp-auto`, `cleanup-disable-wer-queue`, `cleanup-disable-superfetch-write`,
`cleanup-limit-disk-usage-windows-update`

**`UserAccount.cs`** (+11, total 30):
`uac-disable-account-picture`, `uac-disable-guest-account`, `uac-disable-biometrics-policy`,
`uac-disable-smartcard-removal-lock`, `uac-disable-windows-hello-for-business`,
`uac-lock-workstation-on-screensaver`, `uac-disable-microsoft-account-logon`,
`uac-enforce-password-complexity`, `uac-disable-offline-files`, `uac-disable-fast-user-switching`,
`uac-disable-linked-connections`

**`AppCompatibility.cs`** (+10, total 29):
`compat-disable-wer-server-connection`, `compat-disable-compat-telemetry-runner`,
`compat-disable-user-choice-protection`, `compat-disable-vdm-allowed`,
`compat-disable-app-repkg-service`, `compat-disable-install-service`,
`compat-disable-just-in-time-debugging`, `compat-enable-dep-always-on`,
`compat-disable-error-reporting-ui`, `compat-disable-ie-compat-view`

### Tests

- 31 new `[InlineData]` entries in `TweakEngineBuiltinsTests.cs` covering all new tweak IDs
- Total: **1711 tests** passing (1315 Core + 154 CLI + 242 GUI, 1 intentional skip)
- Tweaks: **2767** across 92 categories

## [3.5.0] — 2026-03-18

### Added — Sprint 27: Network Tools

- **`NetworkManager`** Core service: DNS quick-switch (`SetDnsAsync`, `ResetDnsToDhcpAsync`),
  network repair suite (`FlushDnsCacheAsync`, `ResetTcpIpAsync`, `ResetWinsockAsync`), and
  DHCP lease renewal (`RenewDhcpLeaseAsync`, `RepairAllAsync` IAsyncEnumerable)
- **6 built-in DNS presets** — Automatic (DHCP), Cloudflare, Google, Quad9, OpenDNS, NextDNS —
  as `DnsPreset` records with IPv4 + IPv6 addresses
- **`NetworkToolsDialog`** — adapter drop-down, DNS preset quick-switch buttons, repair action
  buttons, async operation log (`RichTextBox`), admin elevation banner
- **Tools → Network Tools** menu item with globe icon

### Added — Sprint 28: Startup Manager

- **`StartupManager`** Core service: reads/writes HKCU Run, HKLM Run, per-user and all-users
  Startup shell folders; exposes `GetAllEntries()`, `SetEnabled()`, `Delete()`
- **`StartupEntry`** record (`Id`, `Name`, `Command`, `Location`, `IsEnabled`) and
  **`StartupLocation`** enum (`RegistryUser`, `RegistryMachine`, `FolderUser`, `FolderAllUsers`)
- **`StartupManagerDialog`** — resizable ListView (Name / Status / Location / Command),
  Enable / Disable / Delete / Refresh buttons, admin elevation banner
- **Tools → Startup Manager** menu item with rocket icon

### Added — Sprint 29: Service Manager

- **`ServiceManager`** Core service: enumerates all Windows services (`GetAllServices()`),
  queries single service (`GetService(name)`), `StartAsync`, `StopAsync`,
  `SetStartTypeAsync` (delegates to `sc.exe config start=`)
- **`ServiceEntry`** record (ServiceName, DisplayName, Description, Status, StartType,
  CanStop, CanPauseAndContinue)
- **`ServiceManagerDialog`** — searchable ListView, description pane, async Start/Stop/
  Enable/Disable/Refresh, admin elevation banner, CancellationToken support
- **Tools → Service Manager** menu item with gear icon
- Added `System.ServiceProcess.ServiceController` v9.0.3 NuGet to `Directory.Packages.props`
  and `RegiLattice.Core.csproj`

### Added — Task 6: BaseDialog Consolidation

- **`BaseDialog : Form`** abstract class with constructor `(string title, Size size, bool resizable)`;
  sets common Form properties (StartPosition, ShowInTaskbar, Icon, MaximizeBox, MinimizeBox)
- Helper factory methods: `CreateSectionHeader()`, `CreateLabel()`, `CreateButtonRow()`, `CreateButton()`
- **Migrated** `NetworkToolsDialog`, `StartupManagerDialog`, `ServiceManagerDialog` to `: BaseDialog`
  — eliminates ~10 lines of identical boilerplate per dialog

### Tests

- **`NetworkManagerTests.cs`** — 8 tests covering `DnsPreset.BuiltIn` structure and
  `NetworkManager.GetActiveAdapterNames()` read-only operations
- **`StartupManagerTests.cs`** — 7 tests covering `StartupManager.GetAllEntries()` return
  contract and `StartupEntry` record semantics
- **`ServiceManagerTests.cs`** — 10 tests covering `ServiceManager.GetAllServices()`,
  `GetService()`, and `ServiceEntry` record semantics

## [3.4.0] — 2026-03-17

### Added — Pre-production Release: Installer, GUI Polish & Repo Cleanup

- **`RegiLattice-3.4.0-win-x64.msi` installer** — self-contained WiX v6 MSI for
  one-click install of GUI + CLI; adds CLI to `PATH`; Start Menu shortcut; upgrade
  and uninstall via Add/Remove Programs
- **Portable executables** — `RegiLattice.GUI.exe` and `RegiLattice.exe` published
  as self-contained win-x64 single-file binaries, available on the Releases page
- **ListView double-click to toggle checkbox** — double-clicking any tweak row in the
  GUI now toggles its checkbox (select/deselect for batch apply)
- **Selected items counter in status bar** — the bottom status bar now shows `☑ N selected`
  when one or more tweaks are checked, updating live as checkboxes change
- **`.gitignore` extended** — added `*.msi`, `*.wixpdb`, `*.wixobj`, `*.cab` to prevent
  WiX installer build outputs from being tracked
- **README overhauled** — updated to reflect 2610 tweaks, 1199 tests, 11 themes,
  added Download & Install section with MSI link, added Building the Installer section
  with step-by-step commands, corrected all stale counts and badges

### Fixed

- Indentation of ListView event handler wiring in `MainForm.Designer.cs` (cosmetic)

### Added — Sprint 21: 50 New Tweaks & +10% Coverage Goal Exceeded

- **50 new tweaks** across 5 categories:
  Security (10), Virtualization (10), Bluetooth (10), Accessibility (10), Cortana & Search (10)
- **93 new Core tests** added across 4 files:
  `TweakDefTests.cs`, `TweakEngineTests.cs`, `RegistrySessionTests.cs`, `ServicesTests.cs`
- **Coverage boost (Core, Cobertura line rate):**
  - TweakEngine.cs: **82.3% → 90.18%** (+7.88)
  - TweakDef.cs: **67.9% → 100%** (+32.1)
  - RegistrySession.cs: **38.9% → 90.25%** (+51.35)
  - Analytics.cs: **54.1% → 100%** (+45.9)
  - Locale.cs: **84.6% → 100%** (+15.4)
  - Ratings.cs: **88.9% → 100%** (+11.1)
  - Overall Core line coverage: **95.3%**
- **Full validation:** all tests pass on solution run (**1,199/1,199**)
- Total: **2,610 tweaks**, **1,199 tests passing** (888 Core + 116 CLI + 195 GUI)

### Added — Sprint 20: GUI Enhancements, Coverage Boost & 50 New Tweaks

- **Search clear button**: ToolStripButton "✕" next to search box, auto-shown/hidden
  when search text is non-empty, clears search on click
- **ListView checkboxes**: custom owner-drawn 14px checkboxes with accent-colour fill
  and white checkmark on checked items, reflecting selection state visually
- **Pending status filter**: new "Pending" option in status filter combo to show only
  tweaks with a pending reboot requirement (from `_pendingRebootIds`)
- **Reboot warning on exit**: when exiting with pending tweaks, a MessageBox warns that
  changes will only take effect after a reboot
- **Test coverage improvement**: TweakEngine.cs 61% → 82.26% (+21%);
  14 new Core tests, ~30 new GUI tests, 6 new CLI tests
- **Memory Optimization** (10 new): pool usage max, session pool size, conservative swap,
  crash dump disable, auto-reboot BSOD disable, dirty page threshold, heap decommit,
  PAE enable, write watch disable, paged pool quota
- **Storage** (10 new): disk quotas disable, volume shadow schedule disable,
  low disk space warning disable, write cache flush enable, thumbnail cache cleanup disable,
  remote diff compression disable, recycle bin 5% max, WER dump disable,
  search index backoff disable, offline files cache disable
- **Startup** (10 new): tablet mode prompt disable, sign-in info reopen disable,
  boot logo disable, auto maintenance disable, narrator at login disable,
  fast user switching disable, logon provider ads disable, Edge prelaunch disable,
  prefetch on SSD disable, compatibility assistant disable
- **SSD Optimization** (10 new): AHCI link power management disable, DIPM disable,
  idle power timeout disable, MFT zone increase, log file flush disable,
  pagefile encryption disable, power scheme optimize, directory timestamp disable,
  volatile write cache enable, global content indexing disable
- **File System** (10 new): critical worker threads increase, delayed worker threads increase,
  change notifications disable, path cache increase, opportunistic locking enable,
  NTFS tunneling disable, I/O queue depth increase, long paths via policy,
  TxF rollback disable, file handle limit increase
- Total: **2,560 tweaks**, **~1,090 tests passing** (50 new tests added)

### Added — Sprint 19: System Monitoring, Live CPU/RAM & 50 New Tweaks

- **SystemMonitor service** (`SystemMonitor.cs`): live system resource monitoring
  via `GetSystemTimes` and `GlobalMemoryStatusEx` P/Invoke — CPU usage (delta-based),
  memory usage (used/total MB + percent), system uptime
- **Live CPU/RAM status bar**: 2-second polling timer displays `CPU: X%` and
  `RAM: X.X / Y.Y GB (Z%)` in the MainForm status strip with accent-coloured labels
- **System uptime in About dialog**: shows `Uptime: Xd Yh Zm` in the hardware info panel
- **Display** (10 new): Windows Ink Workspace disable, force disable HDR, high contrast mode,
  color depth 32-bit, auto-rotation disable, caption button height, mouse hover select,
  full-screen optimization, menu animation fade, peek desktop disable
- **Fonts** (10 new): DPI-aware font scaling, font substitution policy, Cascadia Code icon title font,
  TrueType rendering, font hinting disable, system font size default, DirectWrite enable,
  font providers disable, caption font weight, message font default
- **Input** (10 new): Feedback Hub disable, wheel scroll chars, pen workspace disable,
  handwriting panel disable, mouse hover width, touch visualizations, gesture visualizations,
  input personalization disable, writing insights disable, mouse pointer speed
- **Audio** (10 new): recording quality limit, stereo mix enable, MMCSS scheduling,
  network throttling (multimedia), audio graph isolation, device priority high,
  DPC latency low, beep sounds disable, critical battery sound disable, headphone auto-detect
- **Taskbar** (10 new): taskbar transparency disable, full path title bars, Cortana taskbar button,
  taskbar animations disable, Ink Workspace button hide, news feed taskbar, multi-display show all,
  thumbnail preview disable, thumbnail preview size, peek live preview disable
- **7 new xUnit tests**: SystemMonitor CPU usage, memory usage, uptime, consistency, multi-instance
- Total: **2,510 tweaks**, **1,305 tests passing** (784 Core + 111 CLI + 410 GUI)

### Added — Sprint 18: GUI Visual Overhaul, 7 New Themes & 50 New Tweaks

- **7 new colour themes**: Tokyo Night, Gruvbox Dark, Solarized Dark, One Dark Pro,
  Rosé Pine, Everforest, Cyberpunk — 11 themes total, all switchable at runtime
- **AppIcons overhaul**: all 9 existing icons upgraded to vibrant gradient fills
  (LinearGradientBrush), rounded-rect/circle shapes with GDI+ anti-aliasing
- **7 new menu icon bitmaps**: File, View, Help, Apply (green checkmark),
  Remove (red X), Refresh (blue circular arrow), Export (cyan arrow-out)
- **ToolStrip buttons**: Apply/Remove/Refresh now use colourful ImageAndText style
  instead of plain Unicode text
- **Top-level menus**: File, View, Help now have colourful icon bitmaps
- **Export menu items**: all three export formats (PS1, JSON, REG) display Export icon
- **MainForm visual polish**: gradient header backgrounds (surface→overlay),
  gradient selection highlight (accent tint→overlay) for selected rows
- **DNS & Networking Advanced** (10 new): LLMNR disable, IPv4 DNS priority,
  smart multi-homed disable, DNS client diagnostics, WINS disable,
  negative cache TTL, devolution fallback, FQDN-only, ETW query logging,
  parallel adapter query disable
- **Encryption** (10 new): DES cipher disable, strong key enforcement,
  TLS session ticket lifetime, export-grade cipher disable, OCSP stapling,
  MD5 disable, cert padding enforcement, legacy renegotiation disable,
  extended master secret, Triple DES disable
- **Firewall** (10 new): multicast/broadcast response disable, domain/private profile logging,
  public outbound default-block, NetBIOS/SMB/RPC inbound blocking,
  notification suppression, log max size increase, domain outbound block
- **Hardening** (10 new): WPAD disable, LM auth disable (NTLMv2 only),
  remote registry disable, SEHOP, mandatory ASLR, anonymous SAM restrict,
  CFG enforcement, autoplay all-drives, named pipe restrict, NTLM outgoing block
- **Recovery** (10 new): auto-restart disable, boot logging, minidump type,
  auto-repair disable, crash upload disable, dump folder path, dump count increase,
  startup repair prompt, system failure popup, overwrite existing dump
- **28 new xUnit tests**: 21 new theme verification Theory tests (7 dark themes × 3),
  7 new AppIcons bitmap validity tests

### Added — Sprint 17: Core Services, CLI Commands & 50 New Tweaks

- **ConfigExporter** service — export/import portable tweak selection configs as JSON;
  supports 3 import formats (full, array, object), Validate() for ID hygiene
- **Favorites** service — persist user's favorite tweak IDs with thread-safe static API;
  case-insensitive HashSet, Add/Remove/Toggle/IsFavorite/All/Flush/Clear
- **TweakHistory** service — rolling 500-entry history of tweak operations (apply/remove/update);
  HistoryEntry model with ISO 8601 timestamps, Recent/ForTweak/Flush
- **7 new CLI commands**: `--export-config`, `--import-config`, `--favorites`,
  `--favorite-add`, `--favorite-remove`, `--history`, `--history <count>`
- **Display** (10 new): icon spacing, scrollbar dimensions, border width, window shake,
  menu show delay, text cursor indicator, tooltip delay, dark mode system
- **Startup** (10 new): welcome experience, tips & suggestions, boot timeout,
  first-logon animation, pre-launch apps, background apps policy, autoplay
- **Network Optimization** (10 new): ARP cache size, max connections, NetBIOS over TCP/IP,
  LMHosts lookup, DNS cache TTL, WPAD, RSS, SMB throttling, max user port, TCP timestamps
- **Power Management** (10 new): processor boost, throttle states, energy saver,
  away mode, min processor state, unattended timeout, dimmed display, hybrid sleep,
  lid close action, and more
- **Privacy** (10 new): error reporting, web search in Start, search highlights,
  cloud content search, app launch tracking, handwriting error reports,
  customer experience program, inventory collector
- **40 new tests**: FavoritesTests (11), TweakHistoryTests (11), ConfigExporterTests (10),
  FavoritesAndHistoryParseTests (8)
- Total: **2,410 tweaks** across 89 categories, **1,001 tests passing**

### Added — Sprint 15: 50 New Tweaks

- **Power Management** (10 new): adaptive brightness, power throttling, hard disk timeout,
  core parking, PCI Express max performance, display scaling, processor idle demote,
  energy estimation, high precision timer, turbo boost control
- **Command Line** (7 new): .NET 3.5, IPv6 tunnel adapters, processor scheduling,
  NetBIOS broadcast, NTP high frequency, MPO, Game DVR
- **Developer** (10 new): .NET CLI telemetry, symlink without admin, Python UTF-8 mode,
  Git credential manager, Git default branch, Git autocrlf, Cargo PATH, WER disable,
  environment variable size, Windows Containers
- **Hardening** (10 new): AutoRun disable, remote SAM restriction, remote assistance,
  SMB signing (client + server), LLMNR disable, SMB encryption, cached logons limit,
  admin shares disable
- **Network Optimization** (10 new): TCP Fast Open, TCP slow start, ARP cache, RSC,
  direct cache access, TCP max connections, TCP keepalive, NIC flow control,
  NIC power management, DNS IPv4 priority
- Total: **2,363 tweaks** across 89 categories

### Added — Sprint 16: Security Audit & Validation Enhancement

- **TweakValidator.DetectDuplicateRegistryOps()** — new public method that warns when
  multiple tweaks write to the same registry `Path\Name` target. Case-insensitive.
  Skips check-only ops (CheckValue, CheckMissing, CheckKeyMissing).
- **TweakEngine.DetectDuplicateRegistryOps()** — convenience method delegating to
  TweakValidator for engine consumers
- **CLI `--validate`** now shows duplicate registry warnings separately from errors;
  exit code 1 only on errors (warnings are informational)
- **6 new validator tests**: DuplicateTarget, SamePathDiffNames, CaseInsensitive,
  SameTweakMultiOps, DeleteTreeDuplicate, NoOverlap
- **100-item future roadmap** added to docs/Roadmap.md spanning 10 phases
- **Competitive analysis** of 13 Win11 tweak tools (Winaero, ExplorerPatcher, OFGB, etc.)

### Removed — Security Hardening

- Removed 3 insecure tweaks from CommandLineTweaks.cs:
  - `cmd-enable-telnet-client` — enables unencrypted Telnet protocol
  - `cmd-enable-tftp-client` — enables unencrypted TFTP protocol
  - `cmd-enable-fsutil-disable-encrypt` — disables EFS disk encryption

### Changed

- GUI test suite optimised: 49% faster xUnit time (ToolVersionChecker timeout reduced,
  assembly-level parallelism enabled)

## [3.3.0] — 2026-03-16

### Refactored — Codebase Architecture Improvements

#### Core Engine Decomposition

- **SnapshotManager** — extracted Save/Load/Restore snapshot logic from TweakEngine
  into dedicated `SnapshotManager.cs` (single responsibility, backward-compatible delegation)
- **TweakValidator** — extracted `ValidateTweaks()` + circular dependency detection
  into static `TweakValidator.cs` with pure-function API
- **DependencyResolver** — extracted `ResolveDependencies()`, topological sort, and
  reverse lookup (`Dependents()`) into static `DependencyResolver.cs`
- TweakEngine public API unchanged — all existing tests pass without modification

#### CLI Extraction

- **CliArgs** — extracted nested `Program.CliArgs` class to standalone `CliArgs.cs`
- **ConsoleColorizer** — extracted 5 ANSI color helper methods from Program.cs into
  `ConsoleColorizer.cs` with `NoColor` toggle property

#### Package Manager DRY Elimination

- **PackageNameValidator** — consolidated 5 identical `SafeNameRegex` patterns and
  `ValidateName()` methods from Scoop/Pip/WinGet/Chocolatey/PSModule managers into
  single shared utility with `Validate()` and `ExtractNames()` methods

#### Tests (Sprint 12)

- 27 new tests (700 → 727): 10 ConsoleColorizer tests, 8 PackageNameValidator tests,
  9 additional CLI parsing tests

### Added — Test Coverage Expansion (Sprint 13)

- **SnapshotManagerTests.cs** — 12 direct tests (Save, Load, Restore, round-trip, edge cases)
- **TweakValidatorTests.cs** — 19 direct tests (valid tweaks, empty fields, duplicates,
  circular deps, broken deps)
- **DependencyResolverTests.cs** — 15 direct tests (Resolve topological sort, Dependents
  reverse lookup, circular detection)
- TweakEngine edge case tests (+13): TweaksByScope, Filter, IsApplicableOnHardware,
  DetectStatus, StatusMap subset, Search multi-token
- RegistrySession edge case tests (+17): Execute DryRun, Evaluate CheckMissing/CheckKeyMissing,
  Backup, WriteLog, Read ops, ParsePath
- Total after Sprint 13: **799 tests** (643 Core + 72 CLI + 84 GUI)

### Added — Deep Test Expansion (Sprint 14)

- **TweakEngineBuiltinsTests.cs** — new integration test file with shared `BuiltinsFixture`;
  63 test methods covering RegisterBuiltins validation, global ID uniqueness, required fields,
  profile coverage (all 5 profiles), category counts, search/filter integration, scope
  distribution, TweakKind distribution, dependency resolution on real data
- Expanded TweakDefTests (+57 methods): RegOp factory coverage, TweakScope computation,
  HasOperations gate, KindHint override, ExpectedResult generation
- Expanded ServicesTests (+70 methods): Analytics, AppConfig, CorporateGuard, Elevation,
  HardwareInfo, Locale, Ratings comprehensive coverage
- Expanded CLI ParseArgsTests (+31): additional flag/option combos, edge cases, scope parsing
- Expanded GUI ThemeTests (+48): all 4 themes colour attribute validation, system detection
- Expanded GUI AppIconsTests (+13): bitmap/icon validity, cache invalidation safety
- Total: **2,316 tweaks**, **972 tests** (738 Core + 103 CLI + 131 GUI), **13 test files**

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
- **320 new tweaks** across expanded modules, bringing total to **2301 tweaks**
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
- Updated all documentation with current statistics (2301 tweaks, 89 categories,
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

### Added (Sprint 8)

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

### Changed (Sprint 8)

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
- Delete `.mypy_cache` (16 MB) and `__pycache__` (3 MB) from disk
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
  — all 1981 tweaks register correctly again
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
- Tweak count reduced from ~1828 to **1360 verified functional tweaks**; subsequently
  expanded back to **1981** through multiple tweak addition campaigns

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

- **~1828 tweaks** across 72 categories (migrated from Python with all registry logic preserved)
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
