Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/docs/Roadmap.md#1-1), lines 1 to 100

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/docs/Roadmap.md#100-100), lines 100 to 269

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/docs/Development.md#1-1), lines 1 to 60

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/src/RegiLattice.Core/Services)

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/src/RegiLattice.GUI/Forms)

I now have a complete picture of the project state. Let me write the roadmap.

---

# RegiLattice — Next-Generation Development Roadmap

> **Baseline:** v5.0.0 · 4 825 tweaks · 198 categories · 193 modules · 2 661 tests · 63 GUI dialogs · 34 Core services
> **Scope:** Full refactor and expansion — no restrictions
> **Planning horizon:** 30 sprints across 3 milestones → v5.1, v5.2, v6.0
> **Sprint numbering:** Continues from Sprint 161 (last v5.0.0 sprint)

---

## Major Themes

| # | Theme | Priority | Target Milestone |
|---|-------|----------|-----------------|
| **A** | Testing Hardening & Quality Gates | P0 | v5.1.0 |
| **B** | CLI Overhaul & Developer Experience | P0 | v5.1.0 |
| **C** | Architecture Refactor & Code Health | P0 | v5.1.0 → v6.0.0 |
| **D** | GUI Excellence & UX Modernization | P1 | v5.2.0 |
| **E** | Enterprise & Compliance Deepening | P1 | v5.2.0 |
| **F** | Distribution, Release & DevOps | P1 | v5.1.0 → v5.2.0 |
| **G** | Plugin & Community Ecosystem | P2 | v5.2.0 → v6.0.0 |
| **H** | Tweak Intelligence & Content Expansion | P2 | v6.0.0 |

---

## Theme A — Testing Hardening & Quality Gates

### Context

Line coverage is strong (~95% on Core line coverage), but branch coverage is ~75% and mutation score is unknown. Three known gaps remain open from the previous roadmap: virtual registry integration tests, Stryker mutation testing, and fuzz/chaos coverage of the registry session layer. GUI automation has no E2E coverage.

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| A1 | **Virtual registry integration tests** | Load an isolated `.hiv` hive via `RegLoadKey`/`RegUnLoadKey` in a `[Collection]`-isolated xUnit fixture. Run a real `Apply → Detect → Remove` round-trip for one tweak per TweakKind. Requires elevation on CI runner — add `runs-on: windows-latest` step with `runas: administrator`. | L |
| A2 | **Stryker.NET mutation testing** — 70%+ kill score | Run stryker-config.json across Core library. Fix surviving mutants in `TweakEngine`, `RegistrySession`, `TweakValidator`, `DependencyResolver`. Target threshold: 70% kill. Currently untested. | L |
| A3 | **Fuzz testing for `RegistrySession`** | Use `SharpFuzz` or property-based `FsCheck` to generate malformed registry paths, oversized value names, and non-existent hives. Validate no unhandled exceptions escape `Execute()`/`Evaluate()`. | M |
| A4 | **WinAppDriver / FlaUI GUI E2E tests** | Add a new `RegiLattice.E2E.Tests` project. Cover: launch app, apply/remove tweak in dry-run mode, switch theme, open 5 most-used dialogs, close app. Run on CI as optional gate (not blocking). | XL |
| A5 | **CLI contract tests** | Create `RegiLattice.CLI.Contract.Tests` project. For each CLI command, assert: exit code, stdout structure (when `--output json`), no unhandled exception. Run against real built `RegiLattice.dll`. | M |
| A6 | **Performance regression gate** | Integrate `BenchmarkDotNet` baselines into CI via `dotnet benchmark` diff. Fail CI if `RegisterBuiltins`, `Search`, `StatusMap`, or `Filter` regresses by >15%. Store baseline JSON as GitHub artifact. | M |
| A7 | **Test isolation audit — eliminate shared state** | Identify all tests writing to `%LOCALAPPDATA%\RegiLattice\` real files (e.g., the flaky `ratings.json` file-lock seen in this session). Refactor to use `Path.GetTempPath()` + `IDisposable` cleanup. | M |
| A8 | **Coverage delta gate on PRs** | Add `codecov/codecov-action` with `fail_ci_if_error: true` and `threshold: 0.5%` drop. Generate badge from Codecov. Enforce branch coverage ≥ 80% (up from 75%). | S |

### Dependencies
- A1 requires elevated CI runner configuration (investigate before commit)
- A2 requires A7 first (shared state makes mutation runs flaky)
- A4 requires WinAppDriver installation on CI agent (investigate agent support)
- A5 requires B2 (CLI structured output must be built before it can be contract-tested)

### Risks
- `RegLoadKey` elevation in `windows-latest` GitHub runner — may need `windows-latest` self-hosted runner. Verify with a spike before sprint commit.
- WinAppDriver v1.x is unmaintained; FlaUI is the safer choice. Decision needed before A4 sprint.

### Stretch Goals
- Snapshot round-trip fuzz tests: generate random tweak ID sets, snapshot, restore, diff
- Property tests for all 4 825 TweakDef instances: no null labels, valid hive prefixes, consistent DetectOps

---

## Theme B — CLI Overhaul & Developer Experience

### Context

`CliArgs.cs` has 53 properties with a hand-rolled `ParseArgs()` returning a flat object. There is no subcommand structure, no structured JSON output, and no stable exit code contract. Power users and CI pipelines cannot reliably script against the CLI.

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| B1 | **Subcommand architecture** | Introduce `ICommand` interface. Implement `tweak apply <id>`, `tweak remove <id>`, `tweak status <id>`, `tweak update <id>`, `profile apply <name>`, `profile list`, `snapshot save <file>`, `snapshot restore <file>`, `snapshot diff <a> <b>`. Keep all `--flag` aliases for backward compat. | L |
| B2 | **Structured `--output` mode** | Add `--output json\|csv\|table` (default: `table`) to all listing and status commands. JSON mode emits parseable payloads with stable schema. Define and document exit codes: `0` = success, `1` = partial failure, `2` = invalid args, `3` = registry access denied, `4` = corporate guard blocked. | M |
| B3 | **Grouped contextual `--help`** | Rewrite help output using sections: `Tweak Operations`, `Profiles`, `Snapshots`, `Export/Import`, `Diagnostics`, `Advanced`. Each subcommand has its own `--help` with examples. No more wall of `--flags` with no grouping. | M |
| B4 | **PowerShell module parity** | Ensure `RegiLattice.psd1` exports cmdlets for every B1 subcommand. Add `Get-RLTweak`, `Set-RLTweak`, `Get-RLProfile`, `Set-RLProfile`, `Export-RLSnapshot`, `Restore-RLSnapshot`. Add Pester tests. | M |
| B5 | **Interactive REPL mode** | `regilattice shell` launches an interactive session: persistent engine state, command history, tab-completion of tweak IDs, clear `help` command, `exit`. Avoid external libraries — pure ANSI/Console. | L |
| B6 | **Shell completions** | Generate `Register-ArgumentCompleter` blocks for PowerShell (all subcommands + tweak IDs + category names). Generate `_regilattice` bash completion script for WSL users. CI test that completions generate without error. | M |
| B7 | **Batch mode** | `regilattice batch apply <file.txt\|file.json>` processes a list of tweak IDs or a saved snapshot JSON. Emits per-tweak results. `--dry-run` works in batch mode. Useful for provisioning scripts. | S |
| B8 | **`--watch` mode for status** | `regilattice tweak status <id> --watch` polls every N seconds (configurable) and outputs only on change. Enables live monitoring in CI or provisioning pipelines. | S |

### Dependencies
- B2 before B5 (REPL can reuse JSON output internals)
- B1 before B4, B5, B6, B7 (subcommand structure is the foundation)
- A5 (CLI contract tests) should be written *alongside* B1 and B2, not after

### Risks
- Breaking existing scripts using `--flags` — mitigated by keeping all flags as aliases
- `regilattice shell` REPL: readline-like input on Windows has edge cases with CJK input; scope to ASCII-safe input initially
- Exit code redefinition is a semver-breaking change — must be documented in CHANGELOG as breaking change for v6.0.0, not v5.x

### Stretch Goals
- `regilattice doctor --fix` that detects and auto-applies missing recommended tweaks based on profile
- OpenAPI-style schema for `--output json` payloads, published as `cli-schema.json` in the repo
- `regilattice changelog <id>` that shows what changed for a tweak across versions (from git log)

---

## Theme C — Architecture Refactor & Code Health

### Context

The `TweakEngine.RegisterBuiltins()` method is a manual 193-line registry of modules. Services in `Core` have grown to 34 but share no common lifecycle contract. `CliArgs.cs` has 53 properties (addressed in B1). WinForms forms don't share a consistent base layout contract beyond `BaseDialog`. The project has never been profiled for startup time or memory footprint.

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| C1 | **Source generator for tweak module auto-registration** | Write a `[TweakModule]` attribute and a Roslyn incremental source generator that scans for classes returning `IReadOnlyList<TweakDef>` and auto-registers them in `TweakEngine`. Eliminates the 193-line `RegisterBuiltins()` maintenance burden. Zero runtime reflection. | XL |
| C2 | **`ITweakService` lifecycle interface** | Define `Init()`, `Flush()`, `Dispose()` contract. Migrate all 34 services to implement it. Register via a lightweight DI container (Microsoft.Extensions.DependencyInjection — already available via transitive deps). Enables testable, swappable service implementations. | L |
| C3 | **Startup performance profiling** | Instrument `RegisterBuiltins()` and engine initialization with `System.Diagnostics.Stopwatch`. Target: cold start < 800 ms on low-end hardware (Celeron N4500 equivalent). Identify top 3 bottlenecks and fix. | M |
| C4 | **Memory footprint audit** | Use `dotnet-counters` and a memory snapshot to find top allocators during a `Search()` + `StatusMap()` cycle. Target: < 150 MB working set at idle with all 4 825 tweaks loaded. | M |
| C5 | **Eliminate all `List<T>` → `IReadOnlyList<T>` in service returns** | Audit all 34 services for `List<T>` returns exposed as public API. Replace with `IReadOnlyList<T>`. This is a clean-up pass—no logic changes. | S |
| C6 | **`TweakDef` source compatibility layer** | Add a `[Obsolete]` on any `TweakDef` properties that will change in v6.0 (e.g., if `RegistryKeys` is superseded). Emit deprecation warnings at `compile` time 1 minor version before removal. | S |
| C7 | **Nullable reference types audit** | Run `dotnet build /warnaserror:nullable` on all three projects and fix all suppressed `CS8xxx` nullability warnings. Target: zero nullable warnings with `TreatWarningsAsErrors` on all projects. | M |
| C8 | **Dead code removal sweep** | Use Roslyn Analyzers (`IDE0051`, `IDE0052`) and `dotnet-roslyn-suppressors` to identify unreachable code, unused private members, and leftover TODO comments. Reduce `.cs` file footprint by 5%+. | S |
| C9 | **API surface documentation** | Add XML `<summary>` doc comments to all `public` and `protected` members in `RegiLattice.Core`. Generate Api.md via `xmldoc2md` in CI. Fail CI if public API has undocumented members. | M |

### Dependencies
- C1 requires C2 (services need lifecycle before engine initialization order matters)
- C3 and C4 are independent profiling spikes — run as parallel investigations
- C7 should be done before C1 (fewer noise warnings during generator development)

### Risks
- C1 (source generator): Incremental generators have complex debugging paths. Spike required before sprint commit. If generator proves too complex, acceptable fallback is T4 (convention-based scanning via reflection at startup).
- C2 (DI container): Introducing Microsoft.Extensions.DependencyInjection adds a new dependency. Verify size impact on single-file publish before committing.

### Stretch Goals
- AOT (Ahead-of-Time) publication support: mark all delegates as `[DynamicallyAccessedMembers]`-safe, enable `PublishAot=true` for CLI, measure startup time improvement
- Extract `RegiLattice.Core` to its own NuGet package on NuGet.org — enables third-party tooling on top of the engine

---

## Theme D — GUI Excellence & UX Modernization

### Context

The GUI has 63 dialogs, virtual scrolling, and 11 themes. The next step is quality depth: onboarding, discoverability, undo/redo, accessibility completion, and a polished first-run experience. WinUI 3 migration remains an open investigation item.

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| D1 | **Undo/redo system** | Implement `TweakOperationHistory` — a command-pattern stack of `(TweakDef, Operation, Timestamp)`. `Ctrl+Z` undoes the last apply/remove. `Ctrl+Y` redoes. Max 50 ops. Visual indicator in status bar: "Undo: Disabled Telemetry (2 min ago)". | L |
| D2 | **Onboarding flow rework** | Redesign `FirstRunWizardDialog`: (1) Admin check, (2) Hardware-based profile suggestion with scored panel, (3) Dry-run preview of recommended tweaks, (4) One-click apply selected, (5) Create initial snapshot. Add "Run Setup Wizard" to Help menu for re-run. | M |
| D3 | **Dashboard home screen** | New `DashboardPanel` replacing the blank state when no category is selected. Shows: privacy score ring, performance score ring, security score ring, last-applied tweaks, top 5 recommended, quick-access buttons. Data sourced from `HealthScoreService`. | L |
| D4 | **Saved searches / filter presets** | Users can save a named filter combination (scope + category + tag + query + status) and recall it from a dropdown. Stored in `AppConfig`. Enable via right-click "Save as Preset" on active filter bar. | M |
| D5 | **Accessibility completion** | Complete `AccessibleName` + `AccessibleDescription` on all 63 dialogs (audit shows gaps). Full `TabIndex` chains. Test with Narrator + NVDA. Target: WCAG 2.1 AA on all interactive elements. | L |
| D6 | **Tweak detail pane enrichment** | Extend the existing detail pane with: registry path visualization (copyable), impact matrix (Privacy/Perf/Security deltas), "this tweak is depended on by N others", last-applied badge, and a "See similar tweaks" link. | M |
| D7 | **In-app changelog / What's New** | `WhatsNewDialog` that shows on first launch after upgrade. Auto-populated from CHANGELOG.md parsed at build time into a structured resource. Shows new tweak count, new features. | S |
| D8 | **`.resx` / `ResourceManager` locale migration** | Replace the hand-rolled `Dictionary<string,Dictionary<string,string>>` in `Locale.cs` with `.resx` files per locale. Add `zh-CN`, `ko`, `pt-BR`, `ar` (RTL). Enables satellite assemblies and runtime locale switching. Prerequisite for full i18n. | L |
| D9 | **High-contrast theme + system theme auto-follow** | Add a system high-contrast theme using `SystemColors`. Subscribe to `SystemEvents.UserPreferenceChanged` to auto-switch theme when Windows dark/light/high-contrast mode changes while the app is running. | M |
| D10 | **WinUI 3 migration investigation** | 2-sprint spike: build a single `MainForm`-equivalent WinUI 3 page with Mica material, TreeView categories, virtual `ListView`, and animated `ToggleSwitch`. Measure: LOC delta, startup time, DPI quality vs WinForms. Decide: full migration in v6.1 or visual polish WinForms-only. | XL |

### Dependencies
- D3 (Dashboard) depends on `HealthScoreService` being production-quality (verify before sprint)
- D8 (`.resx` migration) must precede the 4 new locales; RTL Arabic needs WinForms `RightToLeftLayout` audit
- D10 (WinUI spike) is a decision gate — results determine v6.1 roadmap direction
- D1 (undo/redo) must coordinate with the existing `TweakHistory` service — extend, don't duplicate

### Risks
- D8 RTL Arabic: WinForms `RightToLeftLayout = true` can break custom-drawn controls. All 63 dialogs need verification.
- D10: WinUI 3 requires Windows App SDK packaging and changes how single-file publish works — significant impact on installer strategy.

### Stretch Goals
- Drag-to-reorder favourites with persistent order in `Favorites.cs`
- Side-by-side profile diff view: compare two profile snapshots visually
- "Tweak of the day" suggestion panel on dashboard with reason and link

---

## Theme E — Enterprise & Compliance Deepening

### Context

ADMX/ADML and Intune OMA-URI export are complete. Compliance scanning is basic. Enterprise customers need audit-ready reports, SCAP/DISA STIG alignment, and fleet deployment tooling.

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| E1 | **SCAP/XCCDF export** | Generate a SCAP-compatible `XML` document from all `GroupPolicy`-kind tweaks with `NeedsAdmin = true`. Maps tweak IDs to XCCDF rule IDs. Enables import into OpenSCAP, STIG Viewer, and Rapid7. | XL |
| E2 | **PowerShell DSC resource module** | Generate a Desired State Configuration resource (`RegiLatticeDSC.psd1`) that wraps each tweak as a DSC resource. PowerShell `Test-DscConfiguration` can check drift. `Set-DscConfiguration` applies/removes. | L |
| E3 | **Audit log export (SIEM-ready)** | Extend `TweakHistory` to export as CEF (Common Event Format) or JSON-L suitable for ingestion into Splunk, Microsoft Sentinel, or Elastic. `--export-audit-log --format cef <output>`. | M |
| E4 | **User-defined baseline profiles** | Allow saving the current applied state as a named baseline. `ComplianceService` compares current state against any saved baseline (not just built-ins). `--baseline-create <name>`, `--baseline-compare <name>`. | M |
| E5 | **Multi-machine deployment manifest** | Define a `deployment.json` schema: list of profiles + overrides + blacklisted tweak IDs. CLI `regilattice deploy --manifest deployment.json` applies the manifest. Publish a GitHub Actions reusable workflow `regilattice/deploy-action`. | L |
| E6 | **CIS / DISA STIG baseline templates** | Ship 4 built-in compliance baselines as JSON in the repo: `cis-l1-desktop`, `cis-l1-server`, `disa-stig-win11`, `regilattice-recommended`. Each baseline maps to a list of tweak IDs + expected values. Runnable via `--baseline-compare <name>`. | M |
| E7 | **Compliance drift email/webhook alerts** | `ScheduledTweakService` can POST a JSON payload to a configured webhook URL (Slack, Teams, generic) on compliance drift. Configurable in `AppConfig`. No credentials stored — webhook URL only. | M |

### Dependencies
- E1 (SCAP) requires identification of which tweaks map to DISA STIG/CIS controls — requires research spike
- E4 (user baselines) before E6 (built-in baselines use the same storage/comparison engine)
- E5 (manifest) requires B1 (CLI subcommand architecture)
- E7 (webhooks) requires security review — webhook URL in config file is not a secret but content must not leak sensitive system state

### Risks
- SCAP/XCCDF mapping: only a fraction of tweaks will have exact DISA STIG rule IDs. Define scope clearly before sprint — partial coverage with `[UNMAPPED]` flagging is acceptable.
- DSC resources (E2): DSC is in maintenance mode in PowerShell 7+. Investigate DSC v2/v3 compatibility before full commitment.

### Stretch Goals
- Ansible role that wraps CLI apply/remove commands
- Terraform provider (proof of concept) with `regilattice_tweak` resource
- Fleet dashboard SaaS concept (out of scope for this roadmap — flag as v7.0 investigation)

---

## Theme F — Distribution, Release & DevOps

### Context

Distribution covers GitHub Releases, Scoop, WinGet, MSI (WiX), MSIX. Release pipeline includes SHA256SUMS, CodeQL, and auto coverage upload. Outstanding: Authenticode code signing, Chocolatey community package auto-submission, release notes automation, and rollback support in the auto-updater.

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| F1 | **Authenticode code signing — finalize** | Procure EV certificate (DigiCert or Sectigo). Store PFX in GitHub Actions secret `SIGNING_CERT_PFX` + `SIGNING_CERT_PASSWORD`. Sign `RegiLattice.GUI.exe`, `RegiLattice.dll`, MSI, MSIX in `release.yml`. Verify with `signtool verify /pa /v`. | L |
| F2 | **Auto-updater rollback support** | If the new version fails to launch (exit code ≠ 0 within 5 s of first run after update), auto-updater restores the previous MSI/EXE from a local backup at `%LOCALAPPDATA%\RegiLattice\backup\`. Configurable `--no-auto-update` flag. | M |
| F3 | **Automated release notes generation** | CI step that runs after all tests pass: extract CHANGELOG.md section for the current version tag, format as GitHub Release body. Emit tweak count delta and test count delta vs previous tag. No manual editing required. | M |
| F4 | **Chocolatey community package + CI auto-submit** | Write `.nuspec` + `chocolateyinstall.ps1`. CI on tag push: update SHA, auto-submit via `choco push` using `CHOCO_API_KEY` secret. Verify via `choco install regilattice --source https://community.chocolatey.org/api/v2/`. | M |
| F5 | **Release smoke test matrix** | Post-publish CI step: download the produced MSI, install silently, run `regilattice.exe --validate` and `--stats`, check exit code 0. Run on `windows-2022` and `windows-2025` runners. | M |
| F6 | **Staged rollout support** | GitHub Release: publish as `pre-release` first, monitor issue creation for 48 hours via GitHub API, auto-promote to `latest` if zero new issues tagged `bug`. Workflow: `staged-release.yml`. | L |
| F7 | **Dependency vulnerability scanning** | Add Dependabot for NuGet + GitHub Actions dependencies. Add `dotnet list package --vulnerable` check in CI. Fail fast if high/critical CVEs in production dependencies. | S |
| F8 | **Build reproducibility** | Enable deterministic builds (`<Deterministic>true</Deterministic>` already in Directory.Build.props). Add CI step to build twice and compare SHA-256 of output binaries. Document reproducibility status in README. | M |

### Dependencies
- F1 (code signing) must come before F4 (Chocolatey won't accept unsigned EXEs from unknown publishers)
- F3 (automated release notes) before F6 (staged rollout needs good release notes to evaluate)
- F5 (smoke test matrix) must be green before F6 (staged rollout safety gate)

### Risks
- F1: EV certificate procurement takes 1-2 weeks; plan purchase 2 sprints ahead of signing integration sprint
- F6: Auto-promoting from pre-release with a 48-hour window needs robust issue label discipline — define the process clearly in CONTRIBUTING.md before automating it
- F8: Reproducibility is hard with `DateTime.UtcNow` embedded in generated resources (e.g., `WhatsNewDialog`) — audit all build-time embedded timestamps

### Stretch Goals
- Windows Package Manager (WinGet) auto-PR submission on release (currently manual)
- macOS/Linux Homebrew tap for the CLI-only build (cross-platform stretch, requires .NET multi-TFM)
- SBOM (Software Bill of Materials) generation per release via `dotnet sbom-tool`

---

## Theme G — Plugin & Community Ecosystem

### Context

`PackManager`, `PackLoader`, `MarketplaceDialog`, and `PackCreatorDialog` exist. The marketplace serves packs from a CDN. Open items: plugin sandboxing for third-party `ApplyAction` delegates, GPG signing, and community submission pipeline maturation.

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| G1 | **Plugin sandbox (process isolation)** | Third-party `ApplyAction` delegates run in an isolated child process via named pipes. Main process sends `RegOp` lists, receives `TweakResult`. Timeout: 30 s per delegate. Crash in child process returns `TweakResult.Error`, never crashes host. | XL |
| G2 | **Pack GPG signing + verification** | Pack author signs `.rlpack` with GPG private key; `PackLoader` verifies signature against a trust store of known public keys. "Verified ✓" badge in `MarketplaceDialog`. Unsigned packs show a warning dialog. | M |
| G3 | **Pack versioning + dependency resolution** | `PackDef` gains `Version` (SemVer), `MinEngineVersion`, `DependsOnPacks[]`. `PackManager` resolves dependency graph before install using `DependencyResolver`. Circular pack dependencies are blocked with a clear error. | M |
| G4 | **Community submission CI pipeline** | Define GitHub Issue template `New Pack Submission`. CI job validates submitted JSON pack: schema check, `ValidatePackJson`, duplicate ID scan against all built-in tweaks. Auto-labels `ready-for-review` if passing. Reviewer approves → merged to `RegiLattice-Packs` CDN repo. | M |
| G5 | **Pack auto-update notifications** | `UpdateCheckService` also polls pack CDN for version updates. `MarketplaceDialog` shows "Update available" badge per outdated pack. `--update-packs` CLI command batch-updates all installed packs. | S |
| G6 | **Developer SDK documentation** | Write `docs/PluginAuthoring.md`: how to create a pack, use all 12 RegOp factories, use `ApplyAction` delegates, test with `PackLoader.ValidatePackJson`, submit to marketplace. Ship a `template.rlpack` starter. | M |

### Dependencies
- G1 (sandbox) requires stable `ITweakService` lifecycle (C2) so the child process can be correctly initialized
- G2 (GPG signing) before G4 (community submissions should be signed before the pipeline is promoted)
- G3 (pack versioning) before G5 (update check needs version comparison)
- G6 (developer docs) can be written in parallel with G1–G5

### Risks
- G1 (process isolation): Named pipe latency per `RegOp` is measurable. Benchmark a batch of 100 `SetDword` calls via pipe before committing to this architecture. If latency > 200 ms per call, evaluate `AppDomain` (removed in .NET Core) → must use out-of-process.
- G2 (GPG): Windows GPG toolchain is less standard than Linux. Evaluate using .NET Cryptography APIs (RSA/Ed25519) instead of GPG for self-contained operation.

### Stretch Goals
- Pack rating system: users rate installed packs 1–5 stars; aggregated rating displayed in marketplace
- Pack categories and tags in marketplace UI (currently flat list)
- Hot-reload of packs without app restart (watch `.rlpack` file for changes)

---

## Theme H — Tweak Intelligence & Content Expansion

### Context

4 825 tweaks across 198 categories is a strong foundation. The next expansion focuses on quality over quantity: AI-enhanced descriptions, predictive impact scoring, conflict detection enrichment, and coverage of emerging Windows features (Windows 11 24H2/25H2+).

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| H1 | **AI-enhanced tweak descriptions** | One-time pass: run all 4 825 tweak `Description` fields through an LLM prompt (ChatGPT/Claude API or local Ollama) targeting: plain English, ~30 words, mentions real-world impact. Human review on output before commit. Stored as a data migration commit only. | M |
| H2 | **Windows 11 24H2/25H2 tweak audit** | Cross-reference all 4 825 tweaks against KB articles for Windows 11 24H2 and 25H2 registry changes. Identify deprecated keys, changed value semantics, and new opportunities. Create `TweakCompatibility` field: `All`, `Min22H2`, `Min24H2`, `Deprecated`. | L |
| H3 | **Conflict enrichment** | Extend `ConflictDetector` to surface known Windows-enforced conflicts (e.g., "Cannot disable Windows Defender if Tamper Protection is on"). Map 50 common conflict pairs. Add conflict reason to `TweakDef.SideEffects`. | M |
| H4 | **Predictive impact score preview** | Before applying a tweak, show the predicted `ImpactScore` delta across Privacy / Performance / Security dimensions as a tooltip overlay. Sources from existing `ImpactScoreService` data. | M |
| H5 | **Sprint tweak expansion — 200 new tweaks** | Sprints 162–191 each add 10 tweaks to targeted categories. Priority: Windows 11 Recall, Windows Copilot AI, Hyper-V fine-grained, WDAG Application Guard, Microsoft Edge enterprise GPO. Total: 4 825 → 5 025+ | Ongoing |
| H6 | **TweakDef `MinBuild` population audit** | Audit all 4 825 tweaks for missing or incorrect `MinBuild`. Cross-reference Windows build release notes. Especially important for 24H2+ registry additions. | M |
| H7 | **Custom user-defined tweaks** | Allow users to define custom `TweakDef`-like entries via a JSON file in `%LOCALAPPDATA%\RegiLattice\custom-tweaks.json`. `TweakEngine` loads these at startup after built-ins. Validate with `TweakValidator` before registering. | L |

### Dependencies
- H1 (AI descriptions) is a data-only change — no code changes required; can run in parallel with any sprint
- H2 (Windows version audit) should inform H5 (tweak expansion content) and H6 (`MinBuild` population)
- H7 (custom tweaks) requires C2 (service lifecycle) for correct loading order

### Risks
- H1: LLM output quality varies. Risk of hallucinated "facts" about registry value semantics. Mandatory human review pass before merge.
- H2: Windows 11 25H2 registry changes are not fully documented at time of writing — needs continuous monitoring.
- H7: User-defined tweaks create a support surface. Validate thoroughly; document clearly that custom tweaks are unsupported.

### Stretch Goals
- Machine-learning tweak recommender: based on `TweakHistory` of what users actually apply, surface a "Users like you also applied" panel
- Tweak health scoring: flag tweaks that have not been verified against the latest Windows build as `[NEEDS VERIFICATION]`

---

## Dependency Map

```
A7 (test isolation) ──► A2 (Stryker)
B1 (subcommands)   ──► B2 (structured output) ──► A5 (CLI contracts) ──► B4, B5, B6, B7
C7 (nullable audit) ──► C1 (source generator)
C2 (DI lifecycle)  ──► G1 (plugin sandbox) ──► G2 (GPG signing) ──► G4 (community pipeline)
E4 (user baselines) ──► E6 (CIS/STIG baselines)
B1 (subcommands)   ──► E5 (deployment manifest)
F1 (code signing)  ──► F4 (Chocolatey)
F5 (smoke tests)   ──► F6 (staged rollout)
D8 (.resx locales) ──► D9 (high-contrast theme)
H2 (Windows build audit) ──► H5 (tweak expansion) + H6 (MinBuild population)
```

---

## Sprint Plan

### Milestone 1 — v5.1.0: Hardening & Developer Experience
**Sprints 162–167 · ~6 sprints · Target: May 2026**

| Sprint | Theme | Key Deliverables | Exit Criteria |
|--------|-------|-----------------|---------------|
| **162** | C, A | C7 (nullable audit) · A7 (test isolation) · C8 (dead code sweep) | Zero nullable warnings; no real-file writes in tests |
| **163** | B | B1 (subcommand architecture) · B3 (grouped help) | `regilattice tweak apply <id>` works; old `--flags` still work |
| **164** | B, A | B2 (JSON output) · A5 (CLI contract tests) | All commands support `--output json`; exit codes documented + tested |
| **165** | B | B6 (shell completions) · B7 (batch mode) · B4 (PS module parity) | Tab completion for tweak IDs in PowerShell; batch apply from file |
| **166** | A | A2 (Stryker, first pass — reach 60% kill) · A8 (coverage gate) | Stryker running in CI; coverage badge ≥ 80% branch |
| **167** | F | F7 (Dependabot) · F3 (auto release notes) · F8 (reproducible builds) | Dependabot PRs for NuGet; release notes fully automated |

**M1 milestone gate:** `dotnet test` 0 failures · CLI subcommand structure complete · JSON output on all commands · Stryker running · coverage ≥ 80% branch · auto release notes. Tag: `v5.1.0`.

---

### Milestone 2 — v5.2.0: UX & Power Features
**Sprints 168–177 · ~10 sprints · Target: August 2026**

| Sprint | Theme | Key Deliverables | Exit Criteria |
|--------|-------|-----------------|---------------|
| **168** | C | C2 (DI lifecycle contract) · C5 (IReadOnlyList sweep) | All 34 services implement `ITweakService`; no `List<T>` in public API |
| **169** | D | D8 (.resx locale migration) | `Locale.cs` backed by `.resx`; existing 6 locales pass all tests |
| **170** | D | D8 (4 new locales: zh-CN, ko, pt-BR, ar) · D9 (high-contrast + system follow) | 10 locales total; Arabic RTL renders in MainForm + 5 dialogs |
| **171** | D | D1 (undo/redo) · D4 (saved filter presets) | `Ctrl+Z` undoes last apply; 3 presets saveable/recallable |
| **172** | D | D2 (onboarding rework) · D7 (What's New dialog) | Wizard shows hardware suggestion + dry-run preview; What's New auto-populated |
| **173** | D | D3 (dashboard home) · D6 (detail pane enrichment) | Dashboard shows 3 score rings + recent tweaks; detail pane shows impact matrix |
| **174** | E | E4 (user baselines) · E6 (CIS/STIG baseline templates) | `--baseline-compare cis-l1-desktop` works; 4 built-in baselines ship |
| **175** | E | E3 (audit log CEF export) · E7 (webhook drift alerts) | `--export-audit-log --format cef` emits valid CEF; webhook fires on drift |
| **176** | G | G3 (pack versioning + deps) · G5 (pack update notifications) | Pack `DependsOn` resolved at install; `--update-packs` batch-updates |
| **177** | A | A3 (RegistrySession fuzz) · A6 (perf regression gate) · H3 (conflict enrichment) | No unhandled exceptions from malformed ops; perf gate runs in CI; 50 conflict pairs documented |

**M2 milestone gate:** Dashboard live · undo/redo · 10 locales · user baselines + CIS/STIG · CLI JSON output complete · Stryker 65%+ kill · auto release notes. Tag: `v5.2.0`.

---

### Milestone 3 — v6.0.0: Architecture Renaissance & Platform
**Sprints 178–191 · ~14 sprints · Target: December 2026**

| Sprint | Theme | Key Deliverables | Exit Criteria |
|--------|-------|-----------------|---------------|
| **178** | D, H | D10 spike (WinUI 3 prototype) · H2 (Windows 24H2/25H2 audit) | WinUI decision documented; 24H2 tweak audit complete |
| **179** | C | C1 spike (source generator PoC) | Source generator registers all modules in PoC project |
| **180** | C | C1 (full source generator rollout) | `RegisterBuiltins()` removed; 193 modules auto-registered |
| **181** | C | C3 (startup perf) · C4 (memory footprint) | Cold start < 800 ms; working set < 150 MB at idle |
| **182** | F | F1 (Authenticode signing) | GUI + CLI + MSI signed; `signtool verify` passes in CI |
| **183** | F | F4 (Chocolatey auto-submit) · F5 (smoke test matrix) | `choco install regilattice` works on clean VM; smoke tests pass Win11 22H2 + 24H2 |
| **184** | F | F6 (staged rollout) · F2 (auto-updater rollback) | Rollback restores previous MSI on crash; staged rollout auto-promotes after 48 h |
| **185** | G | G1 (plugin sandbox) — phase 1: named pipe protocol | Single tweak delegate runs in isolated process via pipe |
| **186** | G | G1 (plugin sandbox) — phase 2: timeout + crash containment | 30 s timeout enforced; child crash returns `Error`, no host crash |
| **187** | G | G2 (pack GPG/asymmetric signing) · G4 (community pipeline) | Signed packs show "Verified"; CI validates pack submissions |
| **188** | E, B | E1 (SCAP/XCCDF export) · E5 (deployment manifest) | SCAP XML importable in STIG Viewer; `regilattice deploy --manifest` works |
| **189** | E | E2 (PowerShell DSC module) | `Test-DscConfiguration` drifts on unapplied tweaks; `Set-DscConfiguration` fixes |
| **190** | H | H7 (custom user-defined tweaks) · H1 (AI descriptions) | Custom tweaks load from JSON; all descriptions human-reviewed |
| **191** | D, A | D5 (accessibility completion — remaining 43 dialogs) · A4 (WinAppDriver/FlaUI E2E) | All 63 dialogs WAI-ARIA compliant; 5 E2E GUI scenarios pass in CI |

**M3 milestone gate:** Source generator live · Authenticode signed · plugin sandboxing · SCAP export · WinUI 3 decision committed · all locales · custom tweaks · accessibility full pass · Stryker 70%+ kill. Tag: `v6.0.0` (semantic major because of breaking CLI changes from B1, C1, and F1).

---

## Risk Register

| # | Risk | Likelihood | Impact | Mitigation |
|---|------|-----------|--------|------------|
| R1 | Source generator (C1) complexity exceeds estimate | Medium | High | Spike in Sprint 179 before commit; fallback = reflection scan at startup |
| R2 | EV cert procurement delay (F1) | High | Medium | Order cert 3 sprints ahead of F1 target; CI signing uses self-signed cert until real cert arrives |
| R3 | WinAppDriver unmaintained / FlaUI instability on CI | Medium | Medium | Use FlaUI; scope A4 to 5 smoke scenarios only; mark as optional gate (not blocking) |
| R4 | `RegLoadKey` needs elevation on GitHub runner (A1) | High | Medium | Test in Sprint 162 spike; if blocked, use in-memory registry mock (already exists in RegistrySession DryRun) |
| R5 | WinUI 3 migration estimated too expensive (D10) | Medium | High | Sprint 178 spike with fixed 2-sprint budget; if > 6 months to migrate, defer to v6.5 |
| R6 | LLM hallucination in AI descriptions (H1) | High | Medium | 100% human review pass; use structured prompt with key constraints; block merge until reviewed |
| R7 | Arabic RTL regressions across 63 dialogs (D8, D9) | Medium | Medium | Limit initial RTL scope to MainForm + top 10 dialogs; expand incrementally |
| R8 | Plugin sandbox latency unacceptable (G1) | Medium | High | Benchmark named-pipe `SetDword` batch in Sprint 185 spike before full implementation |
| R9 | SCAP/XCCDF mapping coverage below 30% (E1) | High | Medium | Define "partial SCAP export" as acceptable v1; flag unmapped tweaks as `[UNMAPPED]` |
| R10 | Breaking CLI changes for existing user scripts (B1) | Low | High | All `--flag` aliases preserved in v5.x; breaking flags removed only in v6.0.0 with 2 minor versions of deprecation warnings |

---

## Success Metrics

| Metric | v5.0.0 Now | v5.1.0 | v5.2.0 | v6.0.0 |
|--------|-----------|--------|--------|--------|
| Tweaks | 4 825 | 4 925 | 5 025 | 5 250+ |
| Tests | 2 661 | 2 750 | 2 900 | 3 100+ |
| Branch coverage | ~75% | **≥ 80%** | 82% | **≥ 85%** |
| Mutation kill score | Unknown | 60% | 65% | **≥ 70%** |
| CLI subcommand structure | ❌ | ✅ | ✅ | ✅ |
| JSON output on all CLI commands | ❌ | ✅ | ✅ | ✅ |
| Authenticode signed | 🔄 | 🔄 | 🔄 | ✅ |
| Locales | 6 | 6 | **10** | 10 |
| Plugin sandboxed | ❌ | ❌ | ❌ | ✅ |
| Source generator | ❌ | ❌ | ❌ | ✅ |
| Startup time | Unknown | Measured | < 1.2 s | **< 800 ms** |
| Working set (idle) | Unknown | Measured | Measured | **< 150 MB** |
| SCAP export | ❌ | ❌ | ❌ | ✅ |
| E2E GUI tests | ❌ | ❌ | ❌ | ✅ (5 scenarios) |
| Undo/redo | ❌ | ❌ | ✅ | ✅ |
| Dashboard home screen | ❌ | ❌ | ✅ | ✅ |

---

## Appendix: Technical Investigations (Spikes)

Four topics require timebox-bounded spikes before sprint commitment. Plan one spike per topic in Sprints 162–165 (parallel to main work, 1–2 days each):

| Spike | Output | Decision Deadline |
|-------|--------|------------------|
| **S1** `RegLoadKey` elevation on `windows-latest` CI runner | Can proceed with A1 / fall back to mock | Sprint 162 |
| **S2** Roslyn incremental source generator PoC for tweak auto-registration | Go/no-go for C1 in Sprint 179 | Sprint 179 |
| **S3** FlaUI vs WinAppDriver for GUI E2E on headless CI | Tool selection for A4 | Sprint 166 |
| **S4** Named-pipe `SetDword` batch latency measurement | Go/no-go for G1 architecture | Sprint 185 |

---

This roadmap targets `v6.0.0` as the capstone release. It is structured for a single-developer project operating in sprint pairs, but every theme and task is decomposed to be independently assignable if the team scales. Each sprint is sized for approximately one week of focused work, producing at minimum one green build and one committed deliverable.
