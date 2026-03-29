Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/docs/Roadmap.md#1-1), lines 1 to 100

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/docs/Roadmap.md#100-100), lines 100 to 269

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/docs/Development.md#1-1), lines 1 to 60

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/src/RegiLattice.Core/Services)

Read [](file:///c%3A/Users/ryair/OneDrive%20-%20Intel%20Corporation/Documents/MyScripts/RegiLattice/src/RegiLattice.GUI/Forms)

I now have a complete picture of the project state. Let me write the roadmap.

---

# RegiLattice — Next-Generation Development Roadmap

> **Baseline:** v5.0.0 · 4825 tweaks · 198 categories · 193 modules · 2661 tests · 63 GUI dialogs · 34 Core services
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
- Property tests for all 4825 TweakDef instances: no null labels, valid hive prefixes, consistent DetectOps

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
| C4 | **Memory footprint audit** | Use `dotnet-counters` and a memory snapshot to find top allocators during a `Search()` + `StatusMap()` cycle. Target: < 150 MB working set at idle with all 4825 tweaks loaded. | M |
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

4825 tweaks across 198 categories is a strong foundation. The next expansion focuses on quality over quantity: AI-enhanced descriptions, predictive impact scoring, conflict detection enrichment, and coverage of emerging Windows features (Windows 11 24H2/25H2+).

### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| H1 | **AI-enhanced tweak descriptions** | One-time pass: run all 4825 tweak `Description` fields through an LLM prompt (ChatGPT/Claude API or local Ollama) targeting: plain English, ~30 words, mentions real-world impact. Human review on output before commit. Stored as a data migration commit only. | M |
| H2 | **Windows 11 24H2/25H2 tweak audit** | Cross-reference all 4825 tweaks against KB articles for Windows 11 24H2 and 25H2 registry changes. Identify deprecated keys, changed value semantics, and new opportunities. Create `TweakCompatibility` field: `All`, `Min22H2`, `Min24H2`, `Deprecated`. | L |
| H3 | **Conflict enrichment** | Extend `ConflictDetector` to surface known Windows-enforced conflicts (e.g., "Cannot disable Windows Defender if Tamper Protection is on"). Map 50 common conflict pairs. Add conflict reason to `TweakDef.SideEffects`. | M |
| H4 | **Predictive impact score preview** | Before applying a tweak, show the predicted `ImpactScore` delta across Privacy / Performance / Security dimensions as a tooltip overlay. Sources from existing `ImpactScoreService` data. | M |
| H5 | **Sprint tweak expansion — 200 new tweaks** | Sprints 162–191 each add 10 tweaks to targeted categories. Priority: Windows 11 Recall, Windows Copilot AI, Hyper-V fine-grained, WDAG Application Guard, Microsoft Edge enterprise GPO. Total: 4825 → 5025+ | Ongoing |
| H6 | **TweakDef `MinBuild` population audit** | Audit all 4825 tweaks for missing or incorrect `MinBuild`. Cross-reference Windows build release notes. Especially important for 24H2+ registry additions. | M |
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
| Tweaks | 4825 | 4925 | 5025 | 5250+ |
| Tests | 2661 | 2750 | 2900 | 3100+ |
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

---

## Tweak Content Expansion Plan — v5.7.0 through v5.25.0 (Sprints 192–286)

> **Standing cadence**: Every MINOR version bump = 5 sprints = 5 new tweak modules × 10 tweaks each = **+50 tweaks**.
> **Release rule**: Every version bump **must be tagged and pushed immediately** — the tag push triggers
> GitHub Actions `release.yml` which publishes self-contained GUI EXE, CLI EXE, and MSI to GitHub Releases.
> No version bump may be committed without a corresponding `git tag vX.Y.Z && git push --tags`.

### Baseline at v5.7.0 (current)

| Metric | Value |
|--------|-------|
| Tweaks | 5,175 |
| Categories | 233 |
| Module files | 228 |
| Tests | 2,693 |

### Baseline at v5.6.0

| Metric | Value |
|--------|-------|
| Tweaks | 5,125 |
| Categories | 228 |
| Module files | 223 |
| Tests | 2,693 |

### Sprint Cadence Rules

1. **1 module per sprint** — each sprint produces exactly one new tweak module (`.cs` file)
2. **10 tweaks per module** — all declarative `ApplyOps`/`RemoveOps`/`DetectOps`, `NeedsAdmin = true`, `CorpSafe = true`
3. **IDs globally unique** — run pre-commit ID scan before each new module
4. **Gap analysis before each sprint** — confirm `PATH\ValueName` pairs are not in existing modules
5. **`ImpactScore` + `SafetyRating` + `ImpactNote` set explicitly** on all new tweaks (no default reliance)
6. **Commit after each sprint** — one commit per module; do not batch sprints into a single commit
7. **Push + tag on every version bump** — required immediately; triggers release workflow

### 100-Sprint Version Table

| Version | Sprints | Module Focus Areas (5 modules, 10 tweaks each) | +Tweaks | Cumulative |
|---------|---------|-----------------------------------------------|---------|-----------|
| **v5.6.0** | 187–191 | Biometrics Config Policy · App Consent Store Policy · Network Access Protection Policy · Defender Exclusions Policy · System Recovery Options Policy | +50 | **5,125** |
| **v5.7.0** ✅ | 192–196 | WSL Enterprise Policy · Azure AD Tenant Policy · Nearby Sharing Policy · Windows AI / Recall / Copilot+ Policy · WinRM Remote Shell Quota Policy | +50 | **5,175** |
| **v5.8.0** | 197–201 | Certificate Auto-Enrollment Policy · Smart Card Logon Policy · Kerberos Advanced Policy · NTLM Authentication Policy · PKI Infrastructure Policy | +50 | **5,225** |
| **v5.9.0** | 202–206 | Windows Spotlight Advanced Policy · Lock Screen Timeout Policy · Login Banner Policy · Session Lock Enforcement Policy · Screen Saver Policy | +50 | **5,275** |
| **v5.10.0** | 207–211 | OneDrive for Business GPO · SharePoint Online Policy · Microsoft Teams Policy · Exchange Online Policy · Meeting Room Policy | +50 | **5,325** |
| **v5.11.0** | 212–216 | Windows Hello PIN Complexity · Smart Card PIN Policy · FIDO2 Security Key Policy · Windows Credential Guard Advanced · Key Storage Provider Policy | +50 | **5,375** |
| **v5.12.0** | 217–221 | RPC Endpoint Mapper Policy · DCOM Machine Launch Rights · DCOM App-Specific Launch · RPC Packet-Level Security · COM+ Component Services Policy | +50 | **5,425** |
| **v5.13.0** | 222–226 | Defender ATP Sensor Policy · Defender Cloud-Delivered Protection · Defender Network Protection Advanced · Defender PUA Block Policy · Defender Real-Time Scan Policy | +50 | **5,475** |
| **v5.14.0** | 227–231 | App-V Client Policy · MSIX App Attach Policy · App Installer GPO · Side-Loading Restrictions Policy · Microsoft Store for Business Policy | +50 | **5,525** |
| **v5.15.0** | 232–236 | Windows Update Ring Policy · Update Deadline & Grace Period Policy · Forced Restart Options Policy · Driver Update Control Policy · Update Rollback Policy | +50 | **5,575** |
| **v5.16.0** | 237–241 | File History Advanced Policy · Backup Operator Rights Policy · Cloud Backup Enforcement Policy · Volume Shadow Copy Policy · Backup Scheduling Policy | +50 | **5,625** |
| **v5.17.0** | 242–246 | WDAG Application Guard Advanced · AppContainer Isolation Policy · Protected Process Light Policy · Code Integrity Enforcement Policy · CI Boot-Time Verification | +50 | **5,675** |
| **v5.18.0** | 247–251 | Remote Assistance Control Policy · Remote Shell (WinRM) Policy · WinRM Authentication Policy · SSH Server Hardening Policy · Telnet Disable Policy | +50 | **5,725** |
| **v5.19.0** | 252–256 | Speech Recognition Privacy Policy · Inking & Typing Personalization Policy · Activity History Advanced Policy · User Account Sync Control Policy · Timeline Data Policy | +50 | **5,775** |
| **v5.20.0** | 257–261 | Mixed Reality Device Policy · Holographic First Run Policy · Spatial Audio Policy · XR Platform Control Policy · AR Application Policy | +50 | **5,825** |
| **v5.21.0** | 262–266 | Provisioning Package Lock Policy · Enrollment Status Page Policy · Autopilot OOBE Policy · MDM Unenrollment Policy · OOBE Experience Policy | +50 | **5,875** |
| **v5.22.0** | 267–271 | Data Access Control Policy · Resource Attribute Policy · Claims-Based Access Policy · Dynamic Access Control Policy · File Classification Policy | +50 | **5,925** |
| **v5.23.0** | 272–276 | Windows Time Service (NTP) Policy · DirectAccess Client Policy · Always On VPN Profile Policy · Routing & Remote Access Policy · DNS Update Security Policy | +50 | **5,975** |
| **v5.24.0** | 277–281 | Magnifier Advanced Policy · Narrator Verbosity Policy · Speech Recognition Accuracy Policy · Accessibility Sign-In Policy · Input Method Policy | +50 | **6,025** |
| **v5.25.0** | 282–286 | Hyper-V Generation 2 Policy · WSL Distribution Control Policy · Container Network Isolation Policy · Windows Sandbox Advanced Policy · VM Integration Services Policy | +50 | **6,075** |

### End-State at v5.25.0

| Metric | Projected Value |
|--------|----------------|
| Tweaks | **6,075** |
| Categories | **~328** (+100 new) |
| Module files | **~323** (+100 new) |
| Tests | **2,693+** (unchanged unless new test categories created) |

### Pre-Sprint Gap Analysis Workflow

Before executing any sprint from this table, always run all three phases:

```powershell
# Phase 1 — Registry key path not claimed
Select-String -Pattern 'TargetKeyPath' -Path "src/RegiLattice.Core/Tweaks/*.cs"

# Phase 2 — Slug prefix not claimed
Select-String -Pattern '"slug-' -Path "src/RegiLattice.Core/Tweaks/*.cs"

# Phase 3 — PATH\ValueName semantic conflict (most critical)
Select-String -Pattern '"ValueNameToCheck"' -Path "src/RegiLattice.Core/Tweaks/*.cs"
```

If a path or value is found in an existing module, shift to an alternative area from the same theme.
The table above describes **themes and areas**, not fixed registry paths — every sprint requires gap analysis.

### Release Checklist (Per Version Bump)

When bumping from vX.Y.Z to vX.(Y+1).0, update ALL of the following before tagging:

| File | What to Update |
|------|---------------|
| `Directory.Build.props` | All 4 version properties: `<Version>`, `<AssemblyVersion>`, `<FileVersion>`, `<InformationalVersion>` |
| `installer/Package.wxs` | `Version="X.Y.Z"` in `<Package>` element |
| `README.md` | Version badge, tweak count, category count, download link |
| `docs/assets/stats.svg` | Tweak count card, category count card (space-separated thousands: `6 075`) |
| `docs/CHANGELOG.md` | Prepend `## [X.Y.Z] — YYYY-MM-DD (Sprints NNN–NNN)` section with blank lines after headings |
| `.github/copilot-instructions.md` | Quick Facts table: Version row, Tweaks row |

Then:

```powershell
dotnet build RegiLattice.sln -c Release -m:1    # must succeed
dotnet test RegiLattice.sln --no-build -c Release   # must be 0 failures
git add -A
git commit -m "chore: bump version to vX.Y.Z — Sprint NNN-NNN (+50 tweaks)"
git tag vX.Y.Z
git push
git push --tags   # ← TRIGGERS release.yml → publishes EXEs + MSI to GitHub Releases
```

---

---

---

# v6.0.0 Major Release — Full Plan

> **Drafted:** 2026-03-29
> **Baseline:** v5.54.0 · 7,505 tweaks · 464 categories · 461 modules · 2,742 tests · 11 themes · 25+ CLI commands
> **Target:** v6.0.0 by end of 2026
> **Sprint range:** 437–530+
> **Nature:** The first true major version since the Python→C# migration — combines architectural modernization, new surface areas (REST API, Intelligence Engine), and breaking CLI changes.

## Updated State at v5.54.0

The tweak expansion cadence has outpaced the original projection by ~1400 tweaks (reached 7,505 vs. the v5.25.0 target of 6,075). The sprint numbering is now at 431. Architectural work from the original Themes A–G remains largely deferred. This plan addresses that.

| Metric | v5.0.0 Baseline | v5.54.0 Actual | v6.0.0 Target |
|--------|----------------|----------------|---------------|
| Tweaks | 4,825 | **7,505** | **8,000+** |
| Categories | 198 | **464** | 500+ |
| Tests | 2,661 | **2,742** | **3,200+** |
| Branch coverage | ~75% | ~75% | **≥ 85%** |
| Mutation kill score | Unknown | Unknown | **≥ 70%** |
| Startup time | Unknown | Unknown | **< 800 ms** |
| CLI subcommands | ❌ | ❌ | ✅ |
| Source generator | ❌ | ❌ | ✅ |
| REST API | ❌ | ❌ | ✅ |
| Intelligence Engine v2 | ❌ | ❌ | ✅ |
| Dashboard home | ❌ | ❌ | ✅ |
| Undo/redo | ❌ | ❌ | ✅ |
| Authenticode signed | ❌ | ❌ | ✅ |
| Custom user tweaks | ❌ | ❌ | ✅ |
| SCAP/XCCDF export | ❌ | ❌ | ✅ |
| Cross-platform CLI | ❌ | ❌ | ✅ (beta) |

---

## New Themes for v6.0.0

### Theme I — Local REST API & Background Service

#### Context

RegiLattice is used only interactively (GUI or CLI). Power users, home-lab operators, VS Code extensions, and automation scripts have no stable machine-local API to query tweak status, trigger applies, or receive events. A lightweight `localhost`-only HTTP service fills this gap without any cloud dependency.

#### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| I1 | **`regilattice serve` command** | Start a `localhost`-only HTTP server (Kestrel minimal API, `Microsoft.AspNetCore.App` trimmed). Default port: `21397` (configurable). Endpoints: `GET /tweaks`, `GET /tweaks/{id}`, `POST /tweaks/{id}/apply`, `POST /tweaks/{id}/remove`, `GET /tweaks/{id}/status`, `GET /categories`, `GET /profiles`, `POST /profiles/{name}/apply`. API key required (auto-generated UUID in `AppConfig`, shown on first start). | L |
| I2 | **OpenAPI schema** | Auto-generate `swagger.json` at `GET /openapi.json`. Serve Swagger UI at `GET /ui`. Schema version-locked to RegiLattice version. Publish `regilattice-api.yaml` as a GitHub release artifact per version. | M |
| I3 | **Server-Sent Events (SSE) change stream** | `GET /events` returns a text/event-stream of `{ type: "apply"\|"remove"\|"detect", tweakId, result, timestamp }`. Enables home-lab dashboards and VS Code status bar extension without polling. | M |
| I4 | **VS Code extension (companion)** | New repo `regilattice-vscode`. Connects to the REST API. Status bar shows `[3 tweaks unapplied]`. Command palette: `RegiLattice: Apply Tweak`, `RegiLattice: Open Dashboard`. Ships independently, published to VS Code Marketplace. | XL |
| I5 | **API rate limiting & auth hardening** | Enforce: max 60 req/min per API key. Block all non-loopback network interfaces (bind `127.0.0.1` only, never `0.0.0.0`). Log all `4xx`/`5xx` requests to `%LOCALAPPDATA%\RegiLattice\serve.log`. SSL/TLS with a self-signed cert generated at first run. | M |
| I6 | **PowerShell REST client module** | `RegiLattice.REST.psd1` — thin wrapper around the REST endpoints using `Invoke-RestMethod`. Exports `Get-RLTweak`, `Invoke-RLApply`, `Invoke-RLRemove`, `Watch-RLEvents`. Ships in the `powershell/` directory. | S |

#### Security Note

The server MUST bind exclusively to `127.0.0.1`. The auto-generated API key (stored in `AppConfig`, never logged) prevents other localhost processes from issuing commands. All write operations (`apply`, `remove`) log to `TweakHistory` just like GUI/CLI operations.

---

### Theme J — Intelligence Engine Phase 2: Smart Scan & Recommendations

#### Context

`ImpactScore`, `SafetyRating`, and `ImpactNote` are now populated on all tweaks. `HealthScoreService` computes a privacy/perf/security score. The next step is proactive: surface *actionable* recommendations, model *what-if* scenarios, and create a machine-driven Smart Scan that synthesizes hardware, profile, and applied history.

#### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| J1 | **Smart Scan engine** | `SmartScanService.Scan()` returns a `ScanReport` with: (a) unapplied tweaks ranked by `(ImpactScore * SafetyRating)` within the user's detected profile, (b) conflicting currently-applied tweaks, (c) tweaks obsoleted by newer entries, (d) estimated time to apply. Replaces the ad-hoc profile suggestions. | L |
| J2 | **What-If Analysis** | `WhatIfService.Simulate(tweakIds[])` returns a `WhatIfResult`: projected score delta per dimension (Privacy/Perf/Security), list of conflicts that would be introduced, list of dependencies not yet satisfied, risk rating. Exposed in GUI as "Simulate" button before batch apply. | M |
| J3 | **Conflict map enrichment** | Extend `TweakDef` with `Conflicts[]` — explicit list of tweak IDs that this tweak is incompatible with. Populate 100 known pairs. `TweakEngine.ValidateTweaks()` warns on detected conflicts. GUI shows conflict warning in detail pane. | M |
| J4 | **Recommendation "nudge" system** | After each apply/remove operation, `NudgeService` checks if any complementary tweaks are unapplied and queues a one-time notification (info bar in GUI, stdout in CLI). "You applied priv-disable-telemetry — 3 related privacy tweaks are still off." Max 2 nudges per session. | M |
| J5 | **Trend analytics dashboard** | `AnalyticsService` already records apply/remove. New: `TrendReport` aggregating most-applied tweaks this week/month, category application rate, and "improvement velocity" (how fast the health score is changing). Shown as sparklines on the dashboard home. | M |
| J6 | **Scheduled background scan** | `ScheduledScanService` runs a compliance scan every N hours (configurable). If drift detected vs. the last saved baseline, shows a system tray notification. `--schedule <hours>` CLI flag to configure. Runs as a user-mode timer, no service installation required. | L |
| J7 | **Model-Confidence flag** | Add `DataConfidence` enum to `TweakDef`: `Verified` (tested in registry), `Community` (reported by users), `Inferred` (derived from ADMX/docs). Surface in GUI tooltip. Filter in CLI: `--filter confidence=verified`. | S |

---

### Theme K — Performance & Scale

#### Context

Startup time and memory footprint have never been measured. With 7,505 tweaks, `RegisterBuiltins()` calls 461 `Tweaks` property getters at startup. `Search()` builds a LINQ pipeline over 7,505 objects. `StatusMap()` can issue 7,505 registry reads. These are the three performance-critical paths.

#### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| K1 | **Startup profiling & baseline** | Instrument with `System.Diagnostics.Stopwatch`. Measure: cold start time, `RegisterBuiltins()` time, first `Search()` time, first `StatusMap()` time. Publish results as `perf-baseline.md` in `.tmp/`. | S |
| K2 | **Lazy module loading** | Instead of calling all 461 `Tweaks` getters at startup, load modules on first category access. Profile at startup shows < 5 modules needed on cold start. Implement `LazyTweakModule` wrapper. Target: reduce cold start by 40%. | M |
| K3 | **Search index pre-computation** | At registration time, build a pre-computed search index: tokenize all `Id`, `Label`, `Description`, `Tags` into a `Dictionary<string, List<TweakDef>>`. `Search()` performs inverted index lookup instead of LINQ scan. Target: `Search("telemetry")` < 5 ms on 7,500 tweaks. | M |
| K4 | **Incremental `StatusMap()`** | `StatusMap()` with `ids: null` reads up to 7,505 registry keys. Add `StatusMap(since: DateTime)` that re-checks only tweaks whose last-detected status is older than N minutes. Cache status with TTL per tweak. | M |
| K5 | **Source generator for module auto-registration** | Roslyn incremental source generator emits a `GeneratedTweakModules.cs` file that calls `Register(module.Tweaks)` for all 461 modules — replacing the 461-line `RegisterBuiltins()`. Zero runtime reflection. Build-time only. | XL |
| K6 | **Memory footprint audit** | Use `dotnet-counters` to measure GC allocation during `Search()` + `StatusMap()` cycle. Target: < 150 MB working set at idle. Fix top 3 allocation hotspots. | S |
| K7 | **BenchmarkDotNet suite + CI gate** | Add `RegiLattice.Benchmarks` project. Benchmarks: `RegisterBuiltins`, `Search("telemetry")`, `StatusMap(parallel:true, 100 ids)`, `Filter(category="Privacy")`. CI: fail if any benchmark regresses > 20% vs published baseline. | M |

---

### Theme L — Developer SDK & Extensibility

#### Context

`PackLoader` handles JSON packs but there is no documented .NET API for embedding RegiLattice in other tools, no IntelliSense-friendly NuGet package, and no starter template. Making `RegiLattice.Core` a first-class library opens the ecosystem.

#### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| L1 | **NuGet package — `RegiLattice.Core`** | Publish `RegiLattice.Core` to NuGet.org. Add `<PackageId>`, `<Description>`, `<PackageTags>`, `<PackageProjectUrl>`, `<PackageIcon>`, `<GenerateDocumentationFile>true`. CI publishes on each tag push to a NuGet GitHub release asset and optionally to nuget.org via `NUGET_API_KEY` secret. | M |
| L2 | **XML doc comments — full pass** | Add `<summary>`, `<param>`, `<returns>`, `<example>` XML doc comments to all `public` and `protected` members in `RegiLattice.Core`. Fail CI if public member is undocumented (via `CS1591` with `TreatWarningsAsErrors`). | L |
| L3 | **Plugin SDK starter template** | `dotnet new regilattice-plugin` template that creates a `MyCustomTweaks.cs` with a sample `TweakModule`, wires up `PackLoader.LoadFromAssembly()`, and includes a `PackLoader.ValidatePackJson` call. Published to `dotnet new --list`. | M |
| L4 | **`ITweakModule` interface** | Formalize the module contract: `string Name { get; }`, `string Category { get; }`, `IReadOnlyList<TweakDef> Tweaks { get; }`. Source generator (K5) targets this interface. Plugin SDK (L3) implements it. Enables third-party modules loaded at runtime. | S |
| L5 | **Assembly-based plugin loading** | `TweakEngine.LoadPlugin(assemblyPath)` loads a .NET assembly, scans for `ITweakModule` implementors, registers their tweaks with `PackSource = assemblyPath`. Isolate via `AssemblyLoadContext`. CLI: `regilattice plugin load <dll>`. | L |
| L6 | **Schema export for pack authoring** | `regilattice schema export --output regilattice-pack-schema.json` emits a JSON Schema document describing the pack format. Enables IDE autocompletion and validation in VS Code when editing `.rlpack` files. | S |

---

### Theme M — Cross-Platform CLI

#### Context

RegiLattice is `net10.0-windows` only today. The registry access and WinForms code are Windows-specific, but the CLI's core logic — search, filter, validate, export — is pure logic. A multi-TFM build allowing `dotnet run` on Linux/macOS (without registry operations) opens RegiLattice to server admins and CI pipelines running on non-Windows.

#### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| M1 | **TFM conditional compilation audit** | Annotate all P/Invoke, `Microsoft.Win32.Registry`, and `WinForms` usages with `[SupportedOSPlatform("windows")]`. Identify which `Core` services can compile on net10.0 (non-Windows TFM). | M |
| M2 | **`net10.0` target for RegiLattice.Core** | Add `<TargetFrameworks>net10.0-windows;net10.0</TargetFrameworks>` to `RegiLattice.Core.csproj`. Stub out `RegistrySession` on non-Windows (all operations return `TweakResult.Unknown`). SnapshotManager, TweakValidator, TweakEngine search/filter/export work fully on all platforms. | L |
| M3 | **`net10.0` target for RegiLattice.CLI** | Build CLI for `net10.0` (cross-platform). Remove Windows-specific commands (`apply`, `remove`, `status` return `[registry unavailable on this platform]`). `--list`, `--search`, `--validate`, `--export-json`, `--profile list` work on all platforms. | M |
| M4 | **Linux/macOS self-contained publish in CI** | Add `publish-linux` and `publish-macos` jobs to `release.yml`. Upload `RegiLatticeCLI-linux-x64` and `RegiLatticeCLI-osx-x64` as GitHub Release assets. | M |
| M5 | **Homebrew tap** | Create `regilattice/homebrew-tap` repository. Formula installs `RegiLatticeCLI-osx-x64`. CI auto-updates the formula SHA on each tag push. | M |

---

### Theme N — Home Lab & Automation Integration

#### Context

Power users deploy RegiLattice as part of machine provisioning (Ansible, Packer, WinPE). Currently they must shell out to the CLI. Native integrations with common automation tools reduce friction and errors.

#### Tasks

| ID | Task | Description | Effort |
|----|------|-------------|--------|
| N1 | **Ansible collection `regilattice.windows`** | Write `modules/regilattice_tweak.py` (Python Ansible module). Uses CLI `--output json` as backend. Supports `state: present\|absent\|query`. Published to Ansible Galaxy. | L |
| N2 | **Packer provisioner plugin** | Write `packer-plugin-regilattice` in Go (or PowerShell wrapper). `type = "regilattice"` in Packer HCL applies a profile or list of tweak IDs during image build. | L |
| N3 | **GitHub Actions action** | `regilattice/apply-action@v1` — composite action that installs RegiLattice CLI and applies a list of tweak IDs or a profile. Input: `tweaks`, `profile`, `dry-run`. Output: `applied_count`, `failed_count`. | M |
| N4 | **WinPE integration guide** | Document how to include `RegiLatticeCLI.exe` in a WinPE/WinRE image, call `--profile server` during bare-metal provisioning, and verify with `--validate`. | S |
| N5 | **DSC resource module v2** | Rewrite `RegiLatticeDSC` for DSC v3 (PowerShell 7.5+). Uses the REST API (I1) as backend for resource discovery. Compatible with `Test-DscConfiguration` and Azure Machine Configuration. | L |

---

## v6.0.0 Sprint Plan

**Baseline assumption:** Sprint 437 is the next available sprint after the current v5.55.0 tweak expansion begins.

### Phase 1 — Code Health Foundation (Sprints 437–442)

> Prerequisite to everything else. Zero-overhead quality gates that unlock safe refactoring.

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **437** | C, A | Nullable audit (`#nullable enable` + `CS8xxx` → 0 warnings) · Dead code sweep (IDE0051/IDE0052) | `dotnet build /warnaserror:nullable` passes on all 3 projects |
| **438** | A | Test isolation audit — move all file-writing tests to `Path.GetTempPath()` · Add `[Collection]` guards | No test writes to `%LOCALAPPDATA%\RegiLattice\` during `dotnet test` |
| **439** | K | Startup profiling baseline (K1) · BenchmarkDotNet suite scaffolding (K7) | `perf-baseline.md` committed; `RegisterBuiltins` time measured |
| **440** | K | Search index pre-computation (K3) · `IReadOnlyList<T>` sweep in all services (C5) | `Search("telemetry")` < 5 ms; zero `List<T>` in public API returns |
| **441** | C | `ITweakModule` interface (L4) · Source generator spike — PoC registers 5 modules (K5 milestone 1) | Generator PoC builds; registers 5 real modules without `RegisterBuiltins()` |
| **442** | A | Stryker.NET baseline run · Coverage gate in CI (A2, A8) | Stryker runs in CI; kill score measured; branch coverage badge ≥ 80% |

### Phase 2 — CLI Overhaul (Sprints 443–448)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **443** | B | `ICommand` interface · Subcommand router · `tweak apply\|remove\|status\|update` (B1 phase 1) | Old `--` flags still work; `regilattice tweak apply <id>` works |
| **444** | B | `profile apply\|list`, `snapshot save\|restore\|diff` subcommands (B1 phase 2) | All snapshot CLI workflows use new subcommands |
| **445** | B | `--output json\|csv\|table` on all listing/status commands (B2) · Exit code contract (0-4) | JSON output matches documented schema; exit codes correct in all branches |
| **446** | B | Grouped contextual `--help` (B3) · Shell completions for all subcommands + tweak IDs (B6) | `regilattice --help` shows grouped sections; PowerShell tab-complete works |
| **447** | B | CLI contract tests project `RegiLattice.CLI.Contract.Tests` (A5) | Every subcommand has ≥ 1 contract test; exit codes tested |
| **448** | B | PowerShell module `RegiLattice.psd1` parity — new cmdlets matching B1 subcommands (B4) | `Get-RLTweak`, `Invoke-RLApply`, `Get-RLProfile` all functional |

### Phase 3 — Intelligence Engine Phase 2 (Sprints 449–454)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **449** | J | `SmartScanService.Scan()` + `ScanReport` model (J1) | `regilattice scan` returns ranked unapplied tweaks by `ImpactScore × SafetyRating` |
| **450** | J | Conflict map — 100 pairs populated in `TweakDef.Conflicts[]` (J3) | `--validate` reports conflicts; GUI detail pane shows conflict badge |
| **451** | J | `WhatIfService.Simulate()` (J2) — CLI: `regilattice tweak what-if <ids...>` | Returns score delta + conflict list before apply; dry-run implied |
| **452** | J | `NudgeService` (J4) + trend analytics `TrendReport` (J5) | After apply, nudge shows complementary tweaks; dashboard shows trend sparklines |
| **453** | J | `DataConfidence` enum on `TweakDef` (J7) · `--filter confidence=verified` (CLI) | All 7,505 tweaks have a DataConfidence value; CLI filter works |
| **454** | J | `ScheduledScanService` — tray notification on drift (J6) · `--schedule N` config | Tray notification fires on compliance drift; configurable schedule |

### Phase 4 — Local REST API (Sprints 455–459)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **455** | I | Kestrel minimal API scaffolding · `regilattice serve` command · `GET /tweaks`, `/categories` (I1 phase 1) | Server starts; `Invoke-RestMethod http://localhost:21397/tweaks` returns JSON |
| **456** | I | Write endpoints: `POST /tweaks/{id}/apply\|remove` · Auth: API key header (I1 phase 2, I5) | Apply/remove via REST works; unauthenticated calls return `401` |
| **457** | I | `GET /events` SSE change stream (I3) · OpenAPI schema + Swagger UI (I2) | SSE stream emits events on apply/remove; `GET /openapi.json` is valid |
| **458** | I | `RegiLattice.REST.psd1` PowerShell client module (I6) | `Get-RLTweak`, `Invoke-RLApply` use REST backend; Pester tests pass |
| **459** | I | REST API integration tests (new test project `RegiLattice.API.Tests`) | All endpoints have contract tests; 401 on missing key; 404 on unknown ID |

### Phase 5 — GUI Excellence (Sprints 460–467)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **460** | D | `TweakOperationHistory` (undo/redo stack) — `Ctrl+Z`/`Ctrl+Y` (D1) | Last 50 operations undoable; status bar shows undo hint |
| **461** | D | Dashboard home screen `DashboardPanel` — 3 score rings + recent tweaks (D3) | Dashboard shows live privacy/perf/security rings sourced from `HealthScoreService` |
| **462** | D | Smart Scan results panel on dashboard (J1 → GUI surface) | Dashboard shows "Top 5 Recommended" from `SmartScanService` |
| **463** | D | Tweak detail pane enrichment — registry path, impact matrix, conflict badges (D6) | Detail pane shows full registry path (copyable) + impact delta visualization |
| **464** | D | What-If preview modal (J2 → GUI surface) | "Simulate" button opens modal with score preview before batch apply |
| **465** | D | Onboarding rework wizard — hardware detection, dry-run preview, one-click apply (D2) | Wizard guides new users through 5 steps; creates initial snapshot |
| **466** | D | In-app What's New dialog (D7) · Saved filter presets (D4) | What's New auto-populated from CHANGELOG; 5 presets saveable in AppConfig |
| **467** | D | Accessibility completion pass — `AccessibleName` + `TabIndex` on all dialogs (D5) | Narrator reads all interactive controls; no missing `AccessibleName` warnings |

### Phase 6 — Architecture Completion (Sprints 468–474)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **468** | K | Source generator full rollout — all 461 modules auto-registered (K5) | `RegisterBuiltins()` replaced by generated code; all 2,742 tests still pass |
| **469** | K | Lazy module loading (K2) · Memory footprint fix — top 3 allocations (K6) | Cold start < 800 ms; working set < 150 MB at idle |
| **470** | C | `ITweakService` lifecycle — `Init()`, `Flush()`, `Dispose()` on all 15 services (C2) | All services implement `ITweakService`; DI container wires them |
| **471** | L | NuGet package publishing `RegiLattice.Core` (L1) · XML doc comments full pass (L2) | `dotnet add package RegiLattice.Core` works; all public members have `<summary>` |
| **472** | L | Assembly-based plugin loading (L5) · `dotnet new regilattice-plugin` template (L3) | `regilattice plugin load MyPlugin.dll` registers custom tweaks; template installable |
| **473** | H | Custom user-defined tweaks from `custom-tweaks.json` (H7) | Users define tweaks in JSON; loaded at startup; validated by `TweakValidator` |
| **474** | A | FlaUI E2E test suite — 5 GUI smoke scenarios (A4) | App launches, applies tweak in dry-run, theme switches, dialogs open — all in CI |

### Phase 7 — Distribution & Enterprise (Sprints 475–483)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **475** | F | Authenticode code-signing integration in `release.yml` (F1) | GUI EXE + CLI EXE + MSI all signed; `signtool verify /pa` passes in CI |
| **476** | F | Automated release notes from CHANGELOG.md (F3) · Reproducible builds verification (F8) | GitHub Release body auto-populated; two builds produce identical SHA-256 |
| **477** | F | Chocolatey auto-submit CI step (F4) · Smoke test matrix on Win11 22H2 + 24H2 (F5) | `choco install regilattice` works on clean VM within 2 minutes of tag push |
| **478** | F | Staged rollout workflow `staged-release.yml` (F6) · Auto-updater rollback (F2) | Pre-release auto-promotes to latest after 48 h with zero bug reports |
| **479** | E | User-defined baseline profiles (E4) · CIS/DISA STIG baseline templates (E6) | `--baseline-compare cis-l1-desktop` works; 4 built-in baselines ship in repo |
| **480** | E | Audit log CEF export (E3) · Webhook drift alerts (E7) | `--export-audit-log --format cef` emits valid CEF; Teams webhook fires on drift |
| **481** | E | SCAP/XCCDF export for GroupPolicy-kind tweaks (E1) | SCAP XML imports into STIG Viewer; ≥ 200 tweaks mapped to XCCDF rule IDs |
| **482** | E | Deployment manifest (E5) · GitHub reusable workflow `regilattice/deploy-action` | `regilattice deploy --manifest deployment.json` applies profiles + overrides |
| **483** | N | `regilattice/apply-action@v1` GitHub Actions composite action (N3) | Action installs CLI + applies tweaks in < 2 min on `windows-latest` runner |

### Phase 8 — Cross-Platform & Plugin Ecosystem (Sprints 484–492)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **484** | M | TFM audit: annotate all `[SupportedOSPlatform("windows")]` usages (M1) | Zero `CA1416` warnings with `TreatWarningsAsErrors` on net10.0 build |
| **485** | M | `net10.0` multi-TFM for `RegiLattice.Core` (M2) — stub `RegistrySession` on non-Windows | Core compiles on Linux; search/filter/validate/export work cross-platform |
| **486** | M | `net10.0` multi-TFM for CLI (M3) · Linux/macOS publish in CI (M4) | CLI `--list`, `--search`, `--export-json` run on Ubuntu 22.04 + macOS 14 |
| **487** | M | Homebrew tap `regilattice/homebrew-tap` (M5) | `brew install regilattice/tap/regilattice` works on macOS |
| **488** | G | Pack GPG/RSA signing + verification (G2) · "Verified ✓" badge in Marketplace | Signed packs display badge; unsigned show warning dialog |
| **489** | G | Pack versioning + dependency resolution (G3) · Pack update notifications (G5) | `PackDef.Version` enables `--update-packs`; circular pack deps blocked |
| **490** | G | Community submission CI pipeline (G4) · Pack schema export CLI (L6) | GitHub Issue template validates pack JSON in CI; schema export produces valid JSON Schema |
| **491** | G | Plugin sandbox — phase 1: named-pipe protocol (G1 milestone 1) | Single `ApplyAction` delegate runs in isolated child process via named pipe |
| **492** | G | Plugin sandbox — phase 2: timeout + crash containment (G1 milestone 2) | 30 s timeout enforced; child crash returns `TweakResult.Error`; no host crash |

### Phase 9 — v6.0.0 Final Gate (Sprints 493–495)

| Sprint | Theme | Deliverable | Exit Criteria |
|--------|-------|-----------|---------------|
| **493** | All | Integration testing marathon — run full suite against real registry (dry-run mode) | 0 test failures; Stryker ≥ 70% kill; E2E smoke tests pass |
| **494** | All | Performance gate: startup < 800 ms · memory < 150 MB · SearchIndex < 5 ms (K7 CI gate) | All perf benchmarks pass; no regressions vs Sprint 439 baseline |
| **495** | All | v6.0.0 release: update Directory.Build.props, CHANGELOG, README, stats.svg, installer · tag v6.0.0 | `git tag v6.0.0; git push --tags` triggers full release workflow; all artifacts signed |

**v6.0.0 milestone gate:** Source generator live · CLI subcommands complete · REST API serving · Intelligence Engine Phase 2 · Dashboard home · Undo/redo · Authenticode signed · Custom tweaks · SCAP export · NuGet package · Cross-platform CLI · Plugin sandboxed · FlaUI E2E · Stryker ≥ 70%.

---

## Sprint Expansion Table — v5.55.0 through v5.80.0

> **Continuing the standing 5×10 cadence** (5 modules × 10 tweaks = +50 tweaks per MINOR bump).
> Next sprint is **432** (v5.55.0 starts there).

| Version | Sprints | Module Focus Areas (5 modules, 10 tweaks each) | +Tweaks | Cumulative |
|---------|---------|-----------------------------------------------|---------|-----------|
| **v5.55.0** | 432–436 | IIS Web Server Hardening Policy · SQL Server Access Audit Policy · Active Directory CS (ADCS) Policy · ADFS Federation Policy · Kerberoast Mitigation Policy | +50 | **7,555** |
| **v5.56.0** | 437–441 | Windows Defender Firewall Advanced Policy · IPSec Rule Enforcement Policy · Network Location Awareness Policy · DNS-over-HTTPS (DoH) Enforcement Policy · Proxy Bypass Policy | +50 | **7,605** |
| **v5.57.0** | 442–446 | Clipboard History Advanced Policy · Clipboard Redirection Policy (RDS) · Shared Clipboard Control Policy · Universal Clipboard Sync Policy · Clipboard Data Sensitivity Policy | +50 | **7,655** |
| **v5.58.0** | 447–451 | Windows Terminal Advanced Config Policy · PowerShell 7 Execution Mode Policy · ISE Deprecation Enforcement Policy · Script Block Logging Advanced Policy · Remote PowerShell (JEA) Policy | +50 | **7,705** |
| **v5.59.0** | 452–456 | Microsoft Edge WebView2 Policy · Edge Application Guard Policy · Edge Sleeping Tabs Policy · Edge Site Isolation Policy · Edge Early Hints Policy | +50 | **7,755** |
| **v5.60.0** | 457–461 | Azure AD Conditional Access Compliance Policy · Entra ID Device Registration Policy · Azure AD PRT SSO Policy · Azure AD SSPR Writeback Policy · Hybrid Join DNS Suffix Policy | +50 | **7,805** |
| **v5.61.0** | 462–466 | Virtualization-Based Security (VBS) Enforcement Policy · HVCI (Memory Integrity) Policy · Secure Launch (D-RTM) Policy · System Guard Runtime Monitor Policy · Kernel DMA Protection Policy | +50 | **7,855** |
| **v5.62.0** | 467–471 | Windows Subsystem for Android Policy · Android App Debugging Policy · WSA Network Isolation Policy · Android Sensor Access Policy · WSA Storage Allocation Policy | +50 | **7,905** |
| **v5.63.0** | 472–476 | Print Spooler Advanced Hardening Policy · Printer Driver Isolation Policy · Internet Printing Protocol Policy · WSD Print Discovery Policy · IPP Everywhere Policy | +50 | **7,955** |
| **v5.64.0** | 477–481 | Windows Hello for Business Advanced Policy · Passwordless Authentication Policy · Biometric Sign-In Policy · Portable Device Authentication Policy · Certificate-Based Authentication (CBA) Policy | +50 | **8,005** |
| **v5.65.0** | 482–486 | Recall AI Indexing Advanced Policy · Copilot+ Feature Gate Policy · Windows AI Platform Policy · NPU Workload Control Policy · AI Content Moderation Policy | +50 | **8,055** |
| **v5.66.0** | 487–491 | Storage Spaces Advanced Policy · ReFS Integrity Streams Policy · Disk Quota Advanced Policy · Virtual Disk Service Policy · Storage Bus Cache Policy | +50 | **8,105** |
| **v5.67.0** | 492–496 | Windows Event Forwarding Advanced Policy · Subscriptions Management Policy · Channel Access Permissions Policy · Event Log Transport Security Policy · Log Archival Policy | +50 | **8,155** |
| **v5.68.0** | 497–501 | Network Bridge Control Policy · LLTD Topology Discovery Policy · HomeGroup Remnant Cleanup Policy · Network Adapter Advanced Policy · NIC Teaming Policy | +50 | **8,205** |
| **v5.69.0** | 502–506 | Microsoft Defender SmartScreen V2 Policy · Reputation-Based Protection Policy · Exploit Guard (EMET Successor) Policy · Controlled Folder Access Advanced Policy · Network Protection Advanced Policy | +50 | **8,255** |
| **v5.70.0** | 507–511 | Font Embedding Restriction Policy · OpenType Layout Feature Policy · Variable Font Control Policy · Font Smoothing Advanced Policy · System Font Substitution Policy | +50 | **8,305** |
| **v5.71.0** | 512–516 | Direct3D Feature Level Policy · GPU Scheduler Advanced Policy · WDDM Driver Store Policy · Hardware-Accelerated GPU Scheduling Policy · Display Power Management Policy | +50 | **8,355** |
| **v5.72.0** | 517–521 | Xbox Game Bar Advanced Policy · Game Mode Scheduler Policy · DirectX Diagnostic Policy · Hardware Performance Counter Policy · GPU-P Partitioning Policy | +50 | **8,405** |
| **v5.73.0** | 522–526 | Microsoft Store Advanced APT Policy · Private Store URL Policy · App License Acquisition Policy · In-App Purchase Control Policy · Store App Auto-Update Policy | +50 | **8,455** |
| **v5.74.0** | 527–531 | Windows Sandbox Security Baseline Policy · Container Isolation Advanced Policy · Docker Desktop Integration Policy · WSL2 Network Backend Policy · Hyper-V Quick Create Policy | +50 | **8,505** |
| **v5.75.0** | 532–536 | Roaming User Profile Advanced Policy · Folder Redirection Quota Policy · User State Migration Tool (USMT) Policy · AppData Virtual Store Policy · Profile Space Enforcement Policy | +50 | **8,555** |
| **v5.76.0** | 537–541 | Error Reporting Advanced Bucketing Policy · WER Silent Submission Policy · Windows Quality Update Reporting Policy · Feedback Hub Diagnostic Policy · Reliability Event Filter Policy | +50 | **8,605** |
| **v5.77.0** | 542–546 | Active Setup Policy · App Paths Control Policy · Shell Infrastructure Advanced Policy · StartMenuExperience Restriction Policy · Legacy IE Component Policy | +50 | **8,655** |
| **v5.78.0** | 547–551 | Kernel Event Tracing Advanced Policy · ETW Session Quota Policy · WPP Software Tracing Policy · Circular Kernel Context Logger Policy · NT Kernel Logger Policy | +50 | **8,705** |
| **v5.79.0** | 552–556 | Delivery Optimization Advanced Edge Cache Policy · LEDBAT+ Bandwidth Policy · DO Upload Throttle Policy · DO Network Type Restriction Policy · DO VPN Detection Policy | +50 | **8,755** |
| **v5.80.0** | 557–561 | Time Zone Change Restriction Policy · DST Override Policy · NTP Source Hardening Policy · Secure Time Seeding Policy · Clock Drift Alert Policy | +50 | **8,805** |

### End-State at v5.80.0 (Before v6.0.0 Architectural Work)

| Metric | Projected Value |
|--------|----------------|
| Tweaks | **8,805** |
| Categories | **~540** |
| Module files | **~535** |
| Minimum tests | **2,742** (test expansion happens in v6.0.0 architectural phase) |

---

## v6.0.0 Success Metrics

| Metric | v5.54.0 (Now) | v5.80.0 (Pre-v6) | v6.0.0 Target |
|--------|--------------|-----------------|---------------|
| Tweaks | 7,505 | 8,805 | **9,000+** |
| Tests | 2,742 | ~2,742 | **3,400+** |
| Branch coverage | ~75% | ~75% | **≥ 85%** |
| Mutation kill score | Unknown | Unknown | **≥ 70%** |
| Cold start time | Unknown | Measured | **< 800 ms** |
| Working set (idle) | Unknown | Measured | **< 150 MB** |
| Search latency | Unknown | Measured | **< 5 ms** |
| CLI subcommand structure | ❌ | ❌ | ✅ |
| JSON output on all CLI | ❌ | ❌ | ✅ |
| REST API (localhost) | ❌ | ❌ | ✅ |
| Source generator | ❌ | ❌ | ✅ |
| Intelligence Engine v2 | ❌ | ❌ | ✅ |
| Dashboard home screen | ❌ | ❌ | ✅ |
| Undo/redo | ❌ | ❌ | ✅ |
| Authenticode signed | ❌ | ❌ | ✅ |
| Custom user tweaks | ❌ | ❌ | ✅ |
| SCAP/XCCDF export | ❌ | ❌ | ✅ |
| NuGet package (Core) | ❌ | ❌ | ✅ |
| Cross-platform CLI | ❌ | ❌ | ✅ (beta) |
| Plugin sandboxed | ❌ | ❌ | ✅ |
| FlaUI E2E tests | ❌ | ❌ | ✅ (5 scenarios) |
| Locales | 2 (en, de) | 2 | **6+** |

---

## v6.0.0 Risk Register

| # | Risk | Likelihood | Impact | Mitigation |
|---|------|-----------|--------|------------|
| R11 | Source generator (K5) debuggability | Medium | High | 3-sprint spike (441, 468); fallback = reflection scan at startup |
| R12 | Kestrel adds 20 MB to single-file publish size | High | Low | Benchmark before commit; if > 15 MB increase, use `HttpListener` (built-in) instead |
| R13 | FlaUI E2E instability in headless CI | Medium | Medium | Scope to 5 scenarios; not a blocking gate; `--no-e2e` CI flag available |
| R14 | Cross-platform CLI registry stub misuse | Low | High | Annotate all stubs with `[SupportedOSPlatform]`; every stub method throws `PlatformNotSupportedException` with a clear message |
| R15 | SCAP mapping coverage < 25% of tweaks | High | Medium | Define `[UNMAPPED]` flag; v1 only maps GroupPolicy-kind tweaks (~800–1000 tweaks); full mapping across all kinds is v6.1 |
| R16 | CI runner time increases significantly (E2E + Stryker + contract tests) | Medium | Low | Parallelize test projects; E2E as optional nightly job; Stryker: incremental scan targeting changed files only |
| R17 | NuGet API key compromise | Low | High | Use GitHub Environments with `required reviewers` gate for NuGet publish step; rotate key if exposed |
| R18 | Named-pipe latency for plugin sandbox (G1) > 200 ms | Medium | High | Benchmark in Sprint 491 before full rollout; if too slow, batch ops per call to amortize roundtrip |

---

## Code & Config Improvements — Immediate (Pre-v6.0.0)

The following improvements can be implemented incrementally during the tweak expansion sprints (v5.55–v5.80) without a version bump or breaking change:

### Documentation

- Update `workspace.instructions.md` Solution Structure tree to reflect current 461-module reality (stub the `Tweaks/` listing)
- Update `testing.instructions.md` test counts from actual projects (currently slightly stale)
- Add `docs/Api.md` scaffolding that auto-populates from XML doc comments in CI

### CI/CD

- Add `MSBUILDDISABLENODEREUSE=1` to root `.env.ps1` export guard (already set but check for edge cases)
- Add `dotnet format --verify-no-changes` as a CI step to catch CSharpier formatting before it reaches the editor
- Upgrade GitHub Actions runners: pin `actions/setup-dotnet@v4` SHA + upgrade to .NET 10 RC if available

### Testing

- Add "total tweak count" assertion to `TweakEngineBuiltinsTests`: `Assert.Equal(7505, engine.AllTweaks().Count)` — currently only checks a minimum (`> N`)
- Add `RegisterBuiltins_AllModulesHaveAtLeast1Tweak` test to catch accidentally empty modules
- Add performance timing assertion: `RegisterBuiltins_CompletesUnder2s`

### Config

- Add `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` to `Directory.Build.props` scoped to `CS8xxx` nullable warnings only — use `<WarningsAsErrors>$(WarningsAsErrors);CS8600;CS8602;CS8603</WarningsAsErrors>`
- Add `<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>` to enforce IDE analyzers at build time (IDE0051 dead code, IDE0052 unused members)
- Add `<AnalysisLevel>latest-all</AnalysisLevel>` to activate all Roslyn analyzer rules

### Performance Pre-work

- Move `private static readonly` LINQ expressions in `TweakEngine` to pre-compiled delegates (eliminates repeated delegate allocation per `Filter()` call)
- Cache `Categories()` result (currently recomputes from `_TWEAKS_BY_CAT.Keys` on each call — should be frozen after `Freeze()`)
- Add `Freeze()` call verification: `Debug.Assert(_frozen)` at the top of all read-only public API methods

---

---

# v6.0.0 — "Next Level" Major Release Plan

> **Drafted:** 2026-03-29
> **Current baseline:** v5.54.0 · 7,505 tweaks · 464 categories · 461 modules · 2,742 tests · 11 themes
> **Target release:** v6.0.0 — Q4 2026
> **Breaking changes:** Yes — CLI subcommand structure, async engine API, AppConfig schema v2, source-generated module registration
> **Sprint range:** 432+ (v5.55.0 tweak expansion continues in parallel; architectural sprints interleave)

This plan is structured in **4 pillars** (Engine, Experience, Ecosystem, Operations) containing **16 workstreams** and **~120 deliverables** across 9 phases. Each phase produces a shippable increment.

---

## Pillar 1 — Engine Modernization

### Workstream E1: Async Engine Core

**Problem:** `TweakEngine.Apply()`, `Remove()`, `ApplyBatch()`, `StatusMap()` are synchronous. GUI calls them via `Task.Run()` but cannot cancel, report progress, or handle timeouts. With 7,505 tweaks, `StatusMap()` can block for 3+ seconds.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| E1.1 | **`ApplyAsync(id, CancellationToken)`** | Async overload of Apply/Remove that yields between registry writes. Returns `ValueTask<TweakResult>`. | M |
| E1.2 | **`ApplyBatchAsync(ids, IProgress<BatchProgress>, CancellationToken)`** | Reports progress (applied/total/current ID). GUI binds progress bar directly. CLI `--progress` flag outputs JSON progress lines. | M |
| E1.3 | **`StatusMapAsync(ids?, IProgress<StatusProgress>, CancellationToken)`** | Async parallel registry reads using `Task.WhenAll` with configurable degree-of-parallelism instead of `Parallel.For`. | M |
| E1.4 | **`FilterAsync(predicate, CancellationToken)`** | Cancelable filter for GUI search-as-you-type. Previous keystroke cancels in-flight filter. Eliminates UI thread contention in MainForm search box. | S |
| E1.5 | **`CancellationToken` propagation through services** | All service methods that call `RegistrySession.Execute()` accept optional `CancellationToken`. `RegistrySession.Execute()` checks token between ops. | M |

**Exit criteria:** All `*Async` overloads have tests. GUI ApplyBatch shows live progress bar with cancel button.

### Workstream E2: Thread Safety & Immutability Hardening

**Problem:** `_statusCache` is a mutable `Dictionary<string, TweakResult>` without synchronization. `_cachedCategories` rebuilt during `Freeze()` is not atomic. Pack registration after Freeze is undefined.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| E2.1 | **`ConcurrentDictionary` for `_statusCache`** | Replace `Dictionary` with `ConcurrentDictionary<string, TweakResult>`. Status reads and writes become lock-free. | S |
| E2.2 | **Freeze-gate assertions** | `Debug.Assert(_frozen)` at top of all read-only public API methods (`AllTweaks`, `Categories`, `Search`, `Filter`, `GetTweak`). Catches post-freeze `Register()` calls in debug builds. | S |
| E2.3 | **Immutable frozen state** | After `Freeze()`, replace `_tweaksById` Dictionary with `FrozenDictionary<string, TweakDef>` (.NET 8+). Zero-allocation lookups. `_searchPairs` → `ImmutableArray`. | M |
| E2.4 | **`IReadOnlyList<T>` sweep** | Audit all 35 services. Replace all `List<T>` public API returns with `IReadOnlyList<T>`. Zero mutable collections exposed. | S |
| E2.5 | **Pack registration after Freeze** | Define behavior: `Register()` after `Freeze()` throws `InvalidOperationException`. `RegisterPack()` is the only post-freeze registration path (unfreezes, registers, re-freezes atomically). | S |

**Exit criteria:** Zero `List<T>` in public API. `FrozenDictionary` used. `Debug.Assert(_frozen)` on all read paths.

### Workstream E3: Source-Generated Module Registration

**Problem:** `RegisterBuiltins()` is a 461-line method that manually calls `Register(ModuleName.Tweaks)` for every module. Adding a module requires editing this file. Reflection-based alternative adds startup cost.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| E3.1 | **`[TweakModule]` attribute** | Marker attribute: `[TweakModule("Privacy")]` annotates a class with a `public static IReadOnlyList<TweakDef> Tweaks` property. | S |
| E3.2 | **Roslyn incremental source generator — PoC** | Scans for `[TweakModule]` classes, emits `GeneratedModuleRegistration.g.cs` with `RegisterAll(TweakEngine engine)` that calls `engine.Register(T.Tweaks)` for each. | L |
| E3.3 | **Source generator — full rollout** | Remove `RegisterBuiltins()` body. Replace with single call: `GeneratedModuleRegistration.RegisterAll(this)`. All 461+ modules auto-registered. Zero runtime reflection. | M |
| E3.4 | **Build-time duplicate ID detection** | Source generator also emits a compile-time check: if two modules have a tweak with the same `Id`, emit a diagnostic error (`RLTK001: Duplicate tweak ID`). Eliminates runtime `ArgumentException`. | M |
| E3.5 | **Build-time duplicate registry op detection** | Extend generator to collect all `RegOp.SetDword`/`SetString` path+name pairs. Emit warning `RLTK002` for duplicate `PATH\ValueName` across modules. | M |

**Exit criteria:** `RegisterBuiltins()` deleted. All modules discovered at compile time. Duplicate ID/op detection at build. Zero runtime reflection for module discovery.

### Workstream E4: Performance Engineering

**Problem:** Startup time, search latency, and memory footprint have never been measured. 7,505 TweakDef objects + search pairs + synonym map consume unknown memory. `Search()` is O(n × synonyms × string length).

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| E4.1 | **Startup profiling baseline** | Instrument `RegisterBuiltins()`, `Freeze()`, first `Search()`, first `StatusMap(100)`. Publish baseline to `.tmp/perf-baseline.md`. Measure on low-end (Celeron N4500) and high-end (i9-13900K). | S |
| E4.2 | **Inverted search index** | At `Freeze()`, tokenize all `Id`, `Label`, `Description`, `Tags` into a `Dictionary<string, List<int>>` (token → tweak indices). `Search()` performs inverted index lookup → O(k) where k = matching token count, not O(n). | M |
| E4.3 | **Lazy module loading** | `LazyTweakModule<T>` wrapper delays `T.Tweaks` property access until first category access. Profile shows < 5 modules needed at cold start. Target: 40% startup reduction. | M |
| E4.4 | **Incremental StatusMap with TTL cache** | `StatusMap(since: TimeSpan)` re-checks only tweaks whose last detection is older than TTL. Default TTL: 5 min. Fresh results returned from cache. Reduces registry reads by 80%+ on repeated polls. | M |
| E4.5 | **Memory audit + top-3 fix** | `dotnet-counters` snapshot during `Search() + StatusMap()` cycle. Fix top 3 allocation hotspots. Target: < 120 MB working set at idle with all 7,505 tweaks. | M |
| E4.6 | **BenchmarkDotNet suite + CI gate** | Benchmarks: `RegisterBuiltins`, `Search("telemetry")`, `StatusMap(100)`, `Filter(Privacy)`. CI: fail on > 20% regression vs stored baseline JSON. | M |
| E4.7 | **Cold start target: < 600 ms** | Combine E4.2 (index) + E4.3 (lazy) + E2.3 (FrozenDictionary) + E3.3 (source gen). Measure delta. Target: < 600 ms total engine init on Celeron N4500. | L |

**Exit criteria:** Perf baseline published. Search < 5 ms. Startup < 600 ms on low-end. BenchmarkDotNet in CI.

---

## Pillar 2 — User Experience Excellence

### Workstream UX1: CLI Architecture Overhaul

**Problem:** 53-property `CliArgs` with flat `--flag` parsing. No subcommands, no structured JSON output, no stable exit code contract. Power users cannot reliably script against the CLI.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| UX1.1 | **`ICommand` interface + subcommand router** | `tweak apply <id>`, `tweak remove <id>`, `tweak status <id>`, `tweak update <id>`, `tweak what-if <ids...>`, `profile apply <name>`, `profile list`, `snapshot save <file>`, `snapshot restore <file>`, `snapshot diff <a> <b>`, `scan`, `serve`. Old `--flag` aliases preserved for backward compat. | L |
| UX1.2 | **`--output json\|csv\|table`** | Default: `table`. `json` mode emits parseable payloads with stable schema. Exit codes: `0` = success, `1` = partial, `2` = invalid args, `3` = access denied, `4` = corp guard blocked. | M |
| UX1.3 | **Grouped `--help` with sections** | Sections: Tweak Operations, Profiles, Snapshots, Export/Import, Diagnostics, System, Advanced. Each subcommand has `--help` with examples. | M |
| UX1.4 | **Shell completions** | PowerShell `Register-ArgumentCompleter` for all subcommands + tweak IDs + category names. Publish `completions/RegiLattice.ps1` (enhanced). | M |
| UX1.5 | **`regilattice batch apply <file>`** | Processes a text file of tweak IDs or a snapshot JSON. `--dry-run` works. Per-tweak result output. Returns exit code 1 if any failed. | S |
| UX1.6 | **`--progress` flag for batch operations** | Machine-readable JSON progress lines to stderr: `{"applied": 42, "total": 100, "current": "priv-disable-telemetry", "eta_s": 12}`. | S |
| UX1.7 | **`--dry-run --show-operations`** | Lists all registry changes that would be made in unified diff format: `+ HKLM\...\Value = 0 (DWORD)`, `- HKLM\...\Value [DELETE]`. | S |
| UX1.8 | **CLI contract test project** | `RegiLattice.CLI.Contract.Tests` — every subcommand tested for: exit code, stdout JSON schema, no unhandled exceptions. | M |

**Exit criteria:** All 25+ commands accessible via subcommands. JSON output on all listing/status. Exit codes documented and tested.

### Workstream UX2: GUI Dashboard & Intelligence

**Problem:** No home screen — app opens to an empty state until a category is selected. SmartScan and HealthScore services exist but have no GUI surface. Users lack actionable guidance.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| UX2.1 | **Dashboard home panel** | Replaces blank state. Shows: 3 score rings (Privacy, Performance, Security) from `HealthScoreService`, last 5 applied tweaks, top 5 Smart Scan recommendations, quick-access buttons (Apply Profile, Run Smart Scan, Open Snapshots). | L |
| UX2.2 | **Smart Scan results panel** | "Scan Now" button triggers `SmartScanService.Scan()`. Results in a sortable ListView: tweak ID, label, reason, impact, safety. "Apply Selected" button. | M |
| UX2.3 | **What-If preview modal** | "Simulate" button before batch apply opens a modal: projected score delta per dimension, conflicts introduced, dependencies unsatisfied, risk rating. Uses `WhatIfService.Simulate()`. | M |
| UX2.4 | **Tweak detail pane enrichment** | Extend detail pane with: copyable registry path visualization, impact matrix (Privacy/Perf/Security deltas), "depended on by N tweaks" badge, conflict warnings, `DataConfidence` badge, "See similar" link. | M |
| UX2.5 | **Undo/Redo system** | Command-pattern stack of `(TweakDef, Operation, Timestamp)`. `Ctrl+Z` undoes last apply/remove. `Ctrl+Y` redoes. Max 50 ops. Status bar: "Undo: Disabled Telemetry (2 min ago)". | L |
| UX2.6 | **Saved filter presets** | Name + save a filter combination (scope + category + tags + query + status). Stored in `AppConfig`. Recall from dropdown. "Save as Preset" right-click on active filter bar. | M |
| UX2.7 | **In-app What's New dialog** | Shows on first launch after upgrade. Auto-populated from CHANGELOG.md (parsed at build time into structured resource). Tweak count delta, new features, breaking changes. | S |
| UX2.8 | **Onboarding wizard rework** | 5 steps: (1) Admin check, (2) Hardware-based profile suggestion with scored panel, (3) Dry-run preview of recommended tweaks, (4) One-click apply, (5) Create initial snapshot. Re-runnable from Help menu. | M |

**Exit criteria:** Dashboard live with 3 score rings. Smart Scan functional. Undo/redo working. What's New shows on upgrade.

### Workstream UX3: Accessibility & Internationalization

**Problem:** No screen reader testing, incomplete `AccessibleName` on 63 dialogs, only 2 locales (en, de), no RTL support, no high-contrast theme, no system theme auto-follow.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| UX3.1 | **`.resx` locale migration** | Replace hand-rolled `Dictionary<string,Dictionary>` in `Locale.cs` with `.resx` files per locale. Runtime locale switching via `ResourceManager`. Satellite assemblies for each locale. | L |
| UX3.2 | **4 new locales: zh-CN, ko, pt-BR, es** | Translations for all UI strings. Include right-to-left preparation. Community-reviewable translation files in `src/RegiLattice.GUI/Resources/`. | M |
| UX3.3 | **High-contrast theme + system theme listener** | `SystemColors`-based high-contrast theme. Subscribe to `SystemEvents.UserPreferenceChanged` to auto-switch when Windows dark/light/high-contrast mode changes while app is running. | M |
| UX3.4 | **AccessibleName audit — all 63 dialogs** | Add `AccessibleName` + `AccessibleDescription` to every interactive control. Full `TabIndex` chains. Test with Narrator. Target: WCAG 2.1 AA. | L |

**Exit criteria:** 6+ locales. High-contrast theme. Narrator reads all controls. Auto-theme switch on OS change.

---

## Pillar 3 — Ecosystem & Extensibility

### Workstream ECO1: Local REST API

**Problem:** No machine-local API. Power users, VS Code extensions, and automation scripts have no programmatic access without shelling out to the CLI.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| ECO1.1 | **`regilattice serve` — Kestrel minimal API** | Localhost-only HTTP server (port 21397). `GET /tweaks`, `GET /tweaks/{id}`, `POST /tweaks/{id}/apply\|remove`, `GET /categories`, `GET /profiles`, `POST /profiles/{name}/apply`. API key auth (auto-generated UUID in AppConfig). | L |
| ECO1.2 | **OpenAPI schema + Swagger UI** | `GET /openapi.json` — auto-generated. `GET /ui` — Swagger UI. Schema version-locked to RegiLattice version. Published as release artifact. | M |
| ECO1.3 | **Server-Sent Events (SSE) change stream** | `GET /events` — text/event-stream of `{ type, tweakId, result, timestamp }` on every apply/remove. Enables real-time dashboards without polling. | M |
| ECO1.4 | **Rate limiting + security hardening** | Max 60 req/min per API key. Bind `127.0.0.1` only (never `0.0.0.0`). TLS with self-signed cert. Log all `4xx`/`5xx` to `serve.log`. | M |
| ECO1.5 | **REST API integration tests** | New `RegiLattice.API.Tests` project. Contract tests for all endpoints. 401 on missing key. 404 on unknown ID. Rate limit triggers 429. | M |

**Security note:** Server binds exclusively to loopback. API key stored in AppConfig, never logged. All write ops logged to TweakHistory.

### Workstream ECO2: Plugin Sandbox & SDK

**Problem:** Third-party packs can include `ApplyAction` delegates that run in the host process with full trust. Malicious or buggy delegates can crash the host. No formal SDK or NuGet package for Core.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| ECO2.1 | **`ITweakModule` interface** | Formal contract: `string Name`, `string Category`, `IReadOnlyList<TweakDef> Tweaks`. Source generator targets this. Plugin SDK implements it. | S |
| ECO2.2 | **Assembly-based plugin loading** | `TweakEngine.LoadPlugin(path)` loads .NET assembly via `AssemblyLoadContext`, scans for `ITweakModule`, registers tweaks with `PackSource = path`. CLI: `regilattice plugin load <dll>`. | L |
| ECO2.3 | **Plugin delegate sandbox** | Third-party `ApplyAction`/`DetectAction` run in isolated child process via named pipes. 30s timeout. Child crash → `TweakResult.Error`, no host crash. | XL |
| ECO2.4 | **Pack GPG/RSA signing + verification** | Author signs `.rlpack` with RSA key. `PackLoader` verifies against trust store. "Verified ✓" badge in MarketplaceDialog. Unsigned packs show warning. | M |
| ECO2.5 | **NuGet package `RegiLattice.Core`** | Publish to NuGet.org. Full XML doc comments. `<GenerateDocumentationFile>true`. CI publishes on tag push. | M |
| ECO2.6 | **`dotnet new regilattice-plugin` template** | Starter template: sample `ITweakModule` class, sample `.rlpack`, validation script. Published to NuGet template feed. | M |
| ECO2.7 | **Pack schema export CLI** | `regilattice schema export --output schema.json` — JSON Schema for pack format. Enables VS Code autocompletion when editing `.rlpack` files. | S |

**Exit criteria:** Plugin assemblies loadable. Sandbox isolates delegate execution. NuGet package published. Template installable.

### Workstream ECO3: Cross-Platform CLI (Beta)

**Problem:** RegiLattice is Windows-only. The CLI's search, filter, validate, export logic is platform-neutral but locked behind `net10.0-windows` TFM.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| ECO3.1 | **`[SupportedOSPlatform("windows")]` audit** | Annotate all P/Invoke, `Microsoft.Win32.Registry`, WinForms usages. Identify platform-neutral Core surface. | M |
| ECO3.2 | **Multi-TFM for Core: `net10.0-windows;net10.0`** | Stub `RegistrySession` on non-Windows (all ops return `TweakResult.Unknown`). Search, filter, validate, export work cross-platform. | L |
| ECO3.3 | **Multi-TFM for CLI: `net10.0`** | Windows-only commands (`apply`, `remove`, `status`) return `"registry unavailable"`. `--list`, `--search`, `--validate`, `--export-json` work universally. | M |
| ECO3.4 | **Linux/macOS publish in CI** | `release.yml` publishes `RegiLatticeCLI-linux-x64` and `RegiLatticeCLI-osx-arm64` as GitHub Release assets. | M |
| ECO3.5 | **Homebrew tap** | `regilattice/homebrew-tap` repository. Formula installs macOS CLI binary. CI auto-updates SHA on tag push. | S |

**Exit criteria:** CLI runs on Ubuntu 22.04 + macOS 14 for read-only commands. Published as GitHub Release assets.

---

## Pillar 4 — Operations & Quality

### Workstream OPS1: Testing Hardening

**Problem:** Line coverage is 94.9% but branch coverage is 56.8%. Mutation score is unknown. No E2E GUI tests. CLI integration untested. No performance regression gate.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| OPS1.1 | **Test isolation audit** | Move all file-writing tests to `Path.GetTempPath()` + `IDisposable` cleanup. No test writes to `%LOCALAPPDATA%\RegiLattice\`. | M |
| OPS1.2 | **Stryker.NET — 70% kill target** | Run mutation testing on Core. Fix surviving mutants in TweakEngine, RegistrySession, TweakValidator, DependencyResolver. | L |
| OPS1.3 | **FsCheck property tests** | Generate 10,000 random `TweakDef` instances: no null labels, valid hive prefixes, consistent DetectOps. Test `Register()` with arbitrary valid defs. | M |
| OPS1.4 | **FlaUI E2E GUI smoke tests** | New `RegiLattice.E2E.Tests` project. 5 scenarios: launch, apply dry-run, switch theme, open 3 dialogs, close. Runs on CI as optional gate. | L |
| OPS1.5 | **CLI integration tests** | Test all 25+ commands end-to-end against built `RegiLattice.dll` in dry-run mode. Validate exit codes, stdout structure, no unhandled exceptions. | M |
| OPS1.6 | **Branch coverage gate ≥ 80%** | Codecov with `fail_ci_if_error: true; threshold: -0.5%`. Target branch coverage ≥ 80% (up from 56.8%). | M |
| OPS1.7 | **Performance regression CI gate** | BenchmarkDotNet diff against stored baseline. Fail CI if `RegisterBuiltins`, `Search`, `StatusMap`, or `Filter` regresses > 20%. | M |

**Exit criteria:** Stryker ≥ 70% kill. Branch coverage ≥ 80%. FlaUI smoke tests in CI. Perf gate running.

### Workstream OPS2: CI/CD & Distribution

**Problem:** No code signing, no Dependabot, no formatter gate, no reproducible build verification, no staged rollout.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| OPS2.1 | **Authenticode code signing** | EV certificate. Sign GUI EXE, CLI EXE, MSI in `release.yml`. `signtool verify /pa` passes. | L |
| OPS2.2 | **Automated release notes** | CI extracts CHANGELOG.md section for current tag → GitHub Release body. Tweak count delta and test count delta vs previous tag. | M |
| OPS2.3 | **Dependabot + vulnerability scanning** | NuGet + GitHub Actions dependency PRs. `dotnet list package --vulnerable` in CI. Fail on high/critical CVEs. | S |
| OPS2.4 | **`dotnet format --verify-no-changes` CI gate** | Catch formatting drift before merge. No more CSharpier diff noise in `get_errors`. | S |
| OPS2.5 | **Reproducible builds** | Build twice, compare SHA-256. Document reproducibility in README. Fix `DateTime.UtcNow` embedded timestamps (use git commit timestamp instead). | M |
| OPS2.6 | **Staged rollout workflow** | Publish as `pre-release`. Monitor 48 hours. Auto-promote to `latest` if zero bug-labeled issues. | L |
| OPS2.7 | **Smoke test matrix** | Post-publish: install MSI silently, run `--validate` + `--stats`, check exit 0. Test on `windows-2022` and `windows-2025`. | M |
| OPS2.8 | **Chocolatey auto-submit** | CI on tag: update SHA, `choco push` via `CHOCO_API_KEY`. Verify install on clean VM. | M |

**Exit criteria:** Signed releases. Auto release notes. Dependabot active. Staged rollout for all MINOR+ bumps.

### Workstream OPS3: Enterprise & Compliance

**Problem:** CIS/DISA STIG baselines, SCAP export, webhook alerts, and deployment manifests are planned but unbuilt.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| OPS3.1 | **User-defined baseline profiles** | Save current applied state as named baseline. `--baseline-create <name>`, `--baseline-compare <name>`. | M |
| OPS3.2 | **CIS/DISA STIG baseline templates** | 4 built-in baselines as JSON: `cis-l1-desktop`, `cis-l1-server`, `disa-stig-win11`, `regilattice-recommended`. | M |
| OPS3.3 | **Audit log CEF export** | `--export-audit-log --format cef\|jsonl <output>`. SIEM-ready for Splunk, Sentinel, Elastic. | M |
| OPS3.4 | **Webhook drift alerts** | POST JSON to configured URL on compliance drift. Slack/Teams/generic. No credentials stored — URL only. | M |
| OPS3.5 | **SCAP/XCCDF export** | Generate SCAP-compatible XML from GroupPolicy-kind tweaks. Import into OpenSCAP, STIG Viewer. | XL |
| OPS3.6 | **Deployment manifest** | `deployment.json` schema: profiles + overrides + blacklist. `regilattice deploy --manifest file`. Reusable GitHub Action `regilattice/deploy-action`. | L |
| OPS3.7 | **DSC resource module (v3)** | PowerShell DSC v3 compatible. `Test-DscConfiguration` detects drift. `Set-DscConfiguration` applies. | L |

**Exit criteria:** 4 baselines ship. CEF export valid. Webhook fires on drift. SCAP importable in STIG Viewer.

### Workstream OPS4: Config & Build Modernization

**Problem:** AppConfig has 34 flat JSON properties with no schema, no sections, no migration path. `Directory.Build.props` has OneDrive workarounds. No build-time analyzer enforcement.

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| OPS4.1 | **AppConfig schema v2** | Restructure into nested sections: `{ "engine": {...}, "gui": {...}, "scheduler": {...}, "api": {...} }`. JSON Schema validation. Migration script from v1. | M |
| OPS4.2 | **`<WarningsAsErrors>` for nullable** | `CS8600;CS8602;CS8603;CS8604` promoted to errors in `Directory.Build.props`. Zero nullable warnings. | S |
| OPS4.3 | **`<EnforceCodeStyleInBuild>true`** | IDE0051 (dead code), IDE0052 (unused members) enforced at build. | S |
| OPS4.4 | **`<AnalysisLevel>latest-all`** | Activate all Roslyn analyzer categories. Suppress only justified warnings via `GlobalSuppressions.cs`. | S |
| OPS4.5 | **Version automation for package manifests** | Single `Directory.Build.props` as source-of-truth. MSBuild target updates `scoop/regilattice.json`, `winget/*.yaml`, `chocolatey/*.nuspec` version at build time. | M |
| OPS4.6 | **XML doc comments — full Core pass** | `<summary>`, `<param>`, `<returns>` on all public/protected members. `CS1591` as error. Auto-generate `docs/Api.md` in CI. | L |

**Exit criteria:** AppConfig v2 with migration. Zero nullable warnings. All public API documented. Version auto-syncs to package manifests.

---

## Intelligence Engine Phase 2

### Workstream INT1: Smart Scan & Recommendations

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| INT1.1 | **`SmartScanService.Scan()` full implementation** | Returns `ScanReport`: unapplied tweaks ranked by `ImpactScore × SafetyRating` within detected profile, conflicting applied tweaks, obsoleted tweaks, estimated apply time. | L |
| INT1.2 | **`WhatIfService.Simulate(tweakIds[])`** | Projected score delta per dimension. Conflict list. Unmet dependencies. Risk rating. Exposed in GUI as "Simulate" button and CLI `tweak what-if`. | M |
| INT1.3 | **Conflict map — 100 explicit pairs** | `TweakDef.Conflicts[]` populated. `TweakValidator` warns on conflicts. GUI shows conflict badge on detail pane. | M |
| INT1.4 | **Nudge system** | After apply/remove, check for complementary tweaks. Queue one-time info bar in GUI, stdout line in CLI. Max 2 nudges per session. | M |
| INT1.5 | **`DataConfidence` enum** | `Verified`, `Community`, `Inferred`. All 7,505 tweaks tagged. GUI tooltip. CLI `--filter confidence=verified`. | S |
| INT1.6 | **Trend analytics** | `TrendReport`: most-applied this week/month, category rates, health score velocity. Sparklines on dashboard. | M |
| INT1.7 | **Scheduled compliance scan** | Background timer (configurable hours). Drift vs baseline → tray notification. `--schedule N` config. | L |
| INT1.8 | **Custom user-defined tweaks** | Load from `custom-tweaks.json` in `%LOCALAPPDATA%\RegiLattice\`. Validated by `TweakValidator`. Registered after builtins. | L |

---

## Documentation & Developer Experience

| ID | Deliverable | Description | Effort |
|----|-------------|-------------|--------|
| DOC.1 | **Architecture Decision Records (ADR)** | `docs/adr/` directory. ADR-001: Why WinForms, ADR-002: Why declarative TweakDef, ADR-003: Why source generator, ADR-004: Why localhost API. Template follows MADR format. | S |
| DOC.2 | **Deployment & Administration Guide** | `docs/Deployment.md`: MSI silent install, portable mode, centralized config, GPO deployment prep, config file reference. | M |
| DOC.3 | **Plugin Authoring Guide** | `docs/PluginAuthoring.md`: create pack, use RegOp factories, `ITweakModule` interface, test with `ValidatePackJson`, submit to marketplace. Ship `template.rlpack` starter. | M |
| DOC.4 | **Troubleshooting expansion** | Expand `docs/Troubleshooting.md` to 20+ items: common errors, revert procedures, performance tuning, known issues per OS version. | M |
| DOC.5 | **SECURITY.md expansion** | Vulnerability disclosure process, security hardening checklist for admins, signed release verification instructions. | S |
| DOC.6 | **REST API quickstart** | `docs/RestApi.md`: start server, authenticate, query tweaks, apply via REST, subscribe to SSE events. Code samples in PowerShell, Python, and curl. | M |
| DOC.7 | **Updated instruction files** | Sync `workspace.instructions.md`, `testing.instructions.md`, `copilot-instructions.md` with actual v6.0.0 counts, new services, new CLI commands. | S |

---

## v6.0.0 Phase Schedule

### Phase 1 — Code Health Foundation (Sprints 437–442)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 437 | OPS4 | OPS4.2 (nullable errors) · OPS4.3 (code style) · OPS4.4 (analyzers) |
| 438 | OPS1 | OPS1.1 (test isolation) · E2.4 (IReadOnlyList sweep) |
| 439 | E4 | E4.1 (perf baseline) · E4.6 (BenchmarkDotNet scaffolding) |
| 440 | E4, E2 | E4.2 (search index) · E2.1 (ConcurrentDictionary) · E2.2 (freeze-gate) |
| 441 | E3 | E3.1 ([TweakModule] attribute) · E3.2 (source generator PoC) |
| 442 | OPS1 | OPS1.2 (Stryker baseline) · OPS1.6 (coverage gate) |

**Phase 1 gate:** Nullable warnings at zero. BenchmarkDotNet running. Source gen PoC working. Stryker baseline measured. Coverage ≥ 80% branch.

### Phase 2 — CLI Overhaul (Sprints 443–448)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 443 | UX1 | UX1.1 (subcommand router — tweak commands) |
| 444 | UX1 | UX1.1 (profile + snapshot subcommands) |
| 445 | UX1 | UX1.2 (JSON output + exit codes) |
| 446 | UX1 | UX1.3 (grouped help) · UX1.4 (shell completions) |
| 447 | UX1 | UX1.8 (CLI contract tests) · UX1.5 (batch mode) |
| 448 | UX1 | UX1.6 (progress flag) · UX1.7 (dry-run show-ops) |

**Phase 2 gate:** All CLI commands work via subcommands. JSON output on all listing/status. Contract tests green. Old `--flags` backward compatible.

### Phase 3 — Intelligence Engine (Sprints 449–454)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 449 | INT1 | INT1.1 (SmartScan) · INT1.5 (DataConfidence) |
| 450 | INT1 | INT1.3 (Conflict map — 100 pairs) |
| 451 | INT1 | INT1.2 (WhatIf simulation) |
| 452 | INT1 | INT1.4 (Nudge system) · INT1.6 (Trend analytics) |
| 453 | INT1 | INT1.7 (Scheduled scan) |
| 454 | INT1 | INT1.8 (Custom user tweaks) |

**Phase 3 gate:** Smart Scan functional. 100 conflict pairs. WhatIf simulation working. Custom tweaks loadable.

### Phase 4 — REST API (Sprints 455–459)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 455 | ECO1 | ECO1.1 (serve command — read endpoints) |
| 456 | ECO1 | ECO1.1 (write endpoints + auth) · ECO1.4 (rate limiting) |
| 457 | ECO1 | ECO1.3 (SSE events) · ECO1.2 (OpenAPI) |
| 458 | ECO1 | ECO1.5 (API integration tests) |
| 459 | E1 | E1.1 (ApplyAsync) · E1.2 (ApplyBatchAsync) · E1.5 (CancellationToken) |

**Phase 4 gate:** REST API serving. SSE streaming. Swagger UI. API tests green. Async engine core working.

### Phase 5 — GUI Excellence (Sprints 460–467)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 460 | UX2 | UX2.5 (Undo/Redo) |
| 461 | UX2 | UX2.1 (Dashboard home — 3 score rings) |
| 462 | UX2 | UX2.2 (Smart Scan panel) · UX2.3 (What-If modal) |
| 463 | UX2 | UX2.4 (Detail pane enrichment) |
| 464 | UX2 | UX2.8 (Onboarding wizard) · UX2.7 (What's New) |
| 465 | UX2 | UX2.6 (Saved filter presets) |
| 466 | UX3 | UX3.1 (.resx migration) · UX3.2 (4 new locales) |
| 467 | UX3 | UX3.3 (High-contrast + system follow) · UX3.4 (Accessibility) |

**Phase 5 gate:** Dashboard live. Undo/Redo. 6+ locales. Accessibility WCAG 2.1 AA on all interactive controls.

### Phase 6 — Architecture Completion (Sprints 468–474)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 468 | E3 | E3.3 (Source gen full rollout — all 461+ modules) · E3.4 (build-time dupe ID) |
| 469 | E4 | E4.3 (Lazy loading) · E4.5 (Memory fix) · E4.7 (Cold start target) |
| 470 | E2, E1 | E2.3 (FrozenDictionary) · E2.5 (Pack registration) · E1.3 (StatusMapAsync) |
| 471 | ECO2 | ECO2.1 (ITweakModule) · ECO2.5 (NuGet package) · OPS4.6 (XML docs) |
| 472 | ECO2 | ECO2.2 (Assembly plugin loading) · ECO2.6 (dotnet new template) |
| 473 | ECO2 | ECO2.3 (Plugin sandbox — phase 1) · ECO2.4 (Pack signing) |
| 474 | OPS1 | OPS1.4 (FlaUI E2E) · OPS1.5 (CLI integration tests) |

**Phase 6 gate:** Source gen live. `RegisterBuiltins()` deleted. Startup < 600 ms. NuGet published. Plugin loading + sandbox. E2E tests.

### Phase 7 — Distribution & Enterprise (Sprints 475–483)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 475 | OPS2 | OPS2.1 (Authenticode signing) |
| 476 | OPS2 | OPS2.2 (Auto release notes) · OPS2.5 (Reproducible builds) |
| 477 | OPS2 | OPS2.8 (Chocolatey auto-submit) · OPS2.7 (Smoke test matrix) |
| 478 | OPS2 | OPS2.6 (Staged rollout) · OPS2.3 (Dependabot) · OPS2.4 (format gate) |
| 479 | OPS3 | OPS3.1 (User baselines) · OPS3.2 (CIS/STIG templates) |
| 480 | OPS3 | OPS3.3 (CEF export) · OPS3.4 (Webhook alerts) |
| 481 | OPS3 | OPS3.5 (SCAP/XCCDF) |
| 482 | OPS3 | OPS3.6 (Deployment manifest) · OPS3.7 (DSC v3) |
| 483 | ECO3 | ECO3.1 (Platform audit) · ECO3.2 (Multi-TFM Core) |

**Phase 7 gate:** Signed releases. CIS/STIG baselines. SCAP export. Staged rollout. Chocolatey auto-submit.

### Phase 8 — Cross-Platform & Polish (Sprints 484–492)

| Sprint | Workstream | Key Deliverables |
|--------|------------|-----------------|
| 484 | ECO3 | ECO3.3 (Multi-TFM CLI) · ECO3.4 (Linux/macOS publish) |
| 485 | ECO3 | ECO3.5 (Homebrew tap) |
| 486 | ECO2 | ECO2.3 (Sandbox phase 2 — timeout + crash) · ECO2.7 (Schema export) |
| 487 | INT1 | INT1.3 (Conflict map expansion to 200 pairs) |
| 488 | E3 | E3.5 (Build-time dupe registry op detection) |
| 489 | E4 | E4.4 (Incremental StatusMap with TTL) |
| 490 | DOC | DOC.1–DOC.7 (All documentation deliverables) |
| 491 | OPS1 | OPS1.3 (FsCheck property tests) · OPS1.7 (Perf regression gate) |
| 492 | OPS1 | OPS1.2 (Stryker final pass ≥ 70%) |

**Phase 8 gate:** Cross-platform CLI on GitHub Releases. Homebrew tap. Plugin sandbox complete. All docs updated. Stryker ≥ 70%.

### Phase 9 — v6.0.0 Release Gate (Sprints 493–495)

| Sprint | Deliverable |
|--------|-------------|
| 493 | Integration test marathon: full suite in dry-run against real registry. Zero failures. |
| 494 | Performance gate: startup < 600 ms, search < 5 ms, memory < 120 MB. BenchmarkDotNet CI gate green. |
| 495 | **v6.0.0 release**: Update all version files, CHANGELOG, README, stats.svg, installer. Tag `v6.0.0`. Push. Verify release workflow. |

---

## v6.0.0 Success Metrics

| Metric | v5.54.0 (Now) | v6.0.0 Target | Delta |
|--------|--------------|---------------|-------|
| Tweaks | 7,505 | 9,000+ | +1,500 |
| Categories | 464 | 540+ | +76 |
| Tests | 2,742 | 3,400+ | +658 |
| Branch coverage | ~56.8% | **≥ 80%** | +23% |
| Mutation kill score | Unknown | **≥ 70%** | — |
| Cold start time | Unknown | **< 600 ms** | — |
| Search latency | Unknown | **< 5 ms** | — |
| Working set (idle) | Unknown | **< 120 MB** | — |
| CLI subcommands | ❌ | ✅ | new |
| JSON output all CLI | ❌ | ✅ | new |
| REST API (localhost) | ❌ | ✅ | new |
| Source generator | ❌ | ✅ | new |
| Async engine | ❌ | ✅ | new |
| FrozenDictionary | ❌ | ✅ | new |
| Intelligence Engine v2 | ❌ | ✅ | new |
| Smart Scan | stub | ✅ (full) | new |
| Dashboard home | ❌ | ✅ | new |
| Undo/redo | ❌ | ✅ | new |
| Authenticode signed | ❌ | ✅ | new |
| Custom user tweaks | ❌ | ✅ | new |
| SCAP/XCCDF export | ❌ | ✅ | new |
| NuGet package (Core) | ❌ | ✅ | new |
| Cross-platform CLI | ❌ | ✅ (beta) | new |
| Plugin sandboxed | ❌ | ✅ | new |
| FlaUI E2E tests | ❌ | ✅ (5 scenarios) | new |
| Locales | 2 (en, de) | **6+** | +4 |
| Accessibility | partial | **WCAG 2.1 AA** | new |
| Homebrew tap | ❌ | ✅ | new |

---

## v6.0.0 Risk Register

| # | Risk | Likelihood | Impact | Mitigation |
|---|------|-----------|--------|------------|
| R19 | Source generator complexity exceeds estimate | Medium | High | 3-sprint spike (441→468); fallback: `[ModuleInitializer]` + reflection scan |
| R20 | Kestrel adds significant size to single-file publish | High | Low | If > 20 MB increase, use `HttpListener` (built-in) |
| R21 | FlaUI instability in headless CI | Medium | Medium | Scope to 5 scenarios; optional gate |
| R22 | Async engine migration breaks existing GUI code | Medium | High | Introduce async overloads alongside sync; deprecate sync in v6.1 |
| R23 | AppConfig v2 migration breaks user configs | Medium | High | Auto-migration in `AppConfig.Load()`: detect v1 schema, convert to v2, write back |
| R24 | Named-pipe plugin sandbox latency > 200 ms/op | Medium | High | Batch ops per pipe call; benchmark in spike sprint |
| R25 | Cross-platform RegistrySession stubs confuse users | Low | Medium | Clear `PlatformNotSupportedException` messages; GUI-only on Windows |
| R26 | EV certificate procurement delay | High | Medium | Order 3 sprints ahead; use self-signed in interim |
| R27 | FrozenDictionary requires .NET 8+ APIs | Low | Low | Already on .NET 10 — no issue |
| R28 | SCAP mapping coverage < 25% | High | Medium | v1 targets GroupPolicy-kind only (~1000 tweaks); `[UNMAPPED]` flag for rest |

---

## Breaking Changes Summary (v5.x → v6.0.0)

| Change | Category | Migration Path |
|--------|----------|----------------|
| CLI subcommand structure | CLI | All `--flag` aliases preserved. New `tweak <verb>` syntax is additive. Deprecation warnings in v5.80. |
| `AppConfig` JSON schema v2 (nested) | Config | Auto-migration in `AppConfig.Load()`. Old flat schema detected and converted on first load. |
| `RegisterBuiltins()` removed | Engine | Replaced by source-generated `RegisterAll()`. External code calling `RegisterBuiltins()` should call `RegisterAll()` instead. |
| Async engine overloads | Engine | Sync methods still exist but marked `[Obsolete]`. Async preferred. |
| `FrozenDictionary` for internal state | Engine | No public API change. Internal only. Requires .NET 8+ (already on .NET 10). |
| Exit code contract for CLI | CLI | `0` = success, `1` = partial, `2` = invalid args, `3` = access denied, `4` = corp guard. Previously all non-zero were undefined. |
| `ITweakModule` interface required for plugins | Plugins | Existing `.rlpack` JSON packs unaffected. Only assembly-based plugins need `ITweakModule`. |

---

## Dependency Graph

```
OPS4.2 (nullable) ─────────────► E3.2 (source gen PoC) ──► E3.3 (full rollout)
OPS1.1 (test isolation) ─────► OPS1.2 (Stryker)
E4.1 (perf baseline) ─────────► E4.7 (cold start target)
E4.2 (search index) ──────────► E4.7
E3.3 (source gen) ────────────► E4.7
E2.3 (FrozenDictionary) ─────► E4.7
UX1.1 (subcommands) ──────────► UX1.2 (JSON output) ──► UX1.8 (contract tests)
UX1.1 ─────────────────────────► UX1.4 (completions)
UX1.1 ─────────────────────────► ECO1.1 (serve subcommand)
INT1.1 (SmartScan) ───────────► UX2.2 (GUI surface)
INT1.2 (WhatIf) ──────────────► UX2.3 (GUI modal)
ECO2.1 (ITweakModule) ────────► E3.2 (source gen targets it)
ECO2.1 ────────────────────────► ECO2.2 (assembly loading)
ECO2.2 ────────────────────────► ECO2.3 (sandbox)
ECO3.1 (platform audit) ──────► ECO3.2 (multi-TFM Core) ──► ECO3.3 (multi-TFM CLI)
OPS2.1 (signing) ─────────────► OPS2.8 (Chocolatey publish)
OPS2.7 (smoke tests) ─────────► OPS2.6 (staged rollout)
OPS3.1 (user baselines) ──────► OPS3.2 (CIS/STIG templates)
OPS4.6 (XML docs) ────────────► ECO2.5 (NuGet package)
```

---

*Roadmap updated 2026-03-29 · v6.0.0 "Next Level" plan appended · 4 pillars, 16 workstreams, ~120 deliverables · 9 phases (sprints 437–495) · Next sprint: 432*
