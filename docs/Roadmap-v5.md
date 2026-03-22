# RegiLattice — Development Roadmap v5

> Detailed, sprint-ready execution plan for the next major evolution of RegiLattice.
> Baseline: v4.3.0 · 4 058 tweaks · 116 categories · 62 dialogs · 32 services · 1 858 tests
> Created: 2026-03-22 · Author: Technical Lead
> Status legend: ⬜ Not started · 🔄 In progress · ✅ Done · ⚠️ Blocked/Risk

---

## Executive Summary

RegiLattice is the deepest Windows registry tweak toolkit available (4 058 tweaks vs.
~400 for the nearest competitor). The core engine, services, and tweak coverage are
mature. The next phase focuses on **seven strategic themes** that transform RegiLattice
from a power-user tool into a production-grade, enterprise-ready, accessible platform.

**Key outcomes targeted:**

| Metric | v4.3.0 (Today) | v5.0.0 Target |
|--------|----------------|---------------|
| Tweaks | 4 058 | 5 000+ |
| Tests | 1 858 | 2 500+ |
| GUI Performance | ~4 000 live Controls | ~30 (virtual scrolling) |
| Code Signed | ❌ | ✅ Authenticode EV |
| Auto-updater | Check-only | ✅ Download + guided install |
| GUI Accessibility | ❌ Not implemented | ✅ WCAG 2.1 AA (keyboard + screen reader) |
| Localization Coverage | 17 keys × 6 languages (GUI unhook) | 100% GUI strings × 10 languages |
| Config Validation | Silent fallback | ✅ Schema-validated + migration |
| Plugin Security | Hash-verified, no sandbox | ✅ Hash + optional GPG + process sandbox |
| Branch Coverage | 56.8% | 75%+ |

---

## Theme Overview

| # | Theme | Sprints | Priority | Risk |
|---|-------|---------|----------|------|
| **T1** | GUI Performance & UX Modernization | 6–8 | P0 — Critical | High (virtual scrolling is a rewrite) |
| **T2** | Accessibility & Internationalization | 4–5 | P0 — Critical | Medium (wide surface area) |
| **T3** | CLI Overhaul & Scripting Power | 3–4 | P1 — High | Low |
| **T4** | Enterprise & Compliance Automation | 4–5 | P1 — High | Medium (GPO/Intune mapping) |
| **T5** | Distribution, Trust & Release Pipeline | 3–4 | P1 — High | Medium (code signing cert procurement) |
| **T6** | Testing Excellence & Quality Gates | 3–4 | P1 — High | Low |
| **T7** | Plugin Ecosystem & Community | 3–4 | P2 — Medium | Medium (sandboxing complexity) |
| **T8** | Tweak Expansion & Intelligence | Ongoing | P2 — Medium | Low |

**Total estimated effort: 30–38 sprints (each sprint = 1 Copilot session or ~50 tasks)**

---

## T1 — GUI Performance & UX Modernization

### Problem Statement

`MainForm` instantiates a WinForms `Control` per tweak (~4 058 live objects in
`FlowLayoutPanel`). This is the #1 performance bottleneck. Beyond performance, the
GUI lacks keyboard shortcuts, multi-select, and a modern visual language.

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T1.1 | **Virtual scrolling: Replace `FlowLayoutPanel` with `ListView` `VirtualMode`** — owner-draw tweak rows with toggle icon, label, category badge, scope badge, impact/safety indicators. Only ~30 visible rows rendered at any time. This is the single highest-impact change in the entire roadmap. | P0 | High (2 sprints) | — |
| T1.2 | **Tweak detail side panel** — when a tweak row is selected, show a slide-out or split-panel with: full description, registry paths, tags, impact score, safety rating, dependency chain, last-applied timestamp, expected result. Replaces the current tooltip-only approach. | P0 | Medium (1 sprint) | T1.1 |
| T1.3 | **Keyboard shortcut system** — `Ctrl+F` focus search, `Space` toggle selected tweak, `Ctrl+Z` undo last apply, `F5` refresh status, `Ctrl+A` select all visible, `Escape` clear search. Display shortcuts in menu items and tooltips. | P1 | Low (1 sprint) | T1.1 |
| T1.4 | **Multi-select operations** — `Shift+Click` range select, `Ctrl+Click` individual select on `ListView`. Right-click context menu: Apply Selected, Remove Selected, Add to Favorites, Export Selection. Status bar shows "N selected". | P1 | Medium (1 sprint) | T1.1 |
| T1.5 | **Animated toggle switch control** — custom `ToggleSwitchControl` (GDI+ drawn, smooth slide animation, theme-aware colors). Replaces checkboxes in the tweak list for a modern feel. | P1 | Medium (1 sprint) | T1.1 |
| T1.6 | **WinForms visual polish pass** — rounded panel corners (`GraphicsPath` + `Region`), subtle shadows on cards, Segoe Fluent Icons for menu items, Mica-like tinted background approximation on `Form.BackColor`, smooth category expand/collapse animation. | P2 | Medium (1 sprint) | T1.5 |
| T1.7 | **Tag chip filter sidebar** — replace current dropdown tag filter with a visual grid of clickable tag chips. Active tags highlighted. Chips show count badge. Click to toggle filter, `Ctrl+Click` for AND logic. | P2 | Low (0.5 sprint) | T1.1 |
| T1.8 | **Rich hover tooltips** — `ToolTip` with multi-line HTML-like rendering: description line, expected result, registry path (truncated), safety badge, "Click for details". Delay: 400ms show, 5s auto-hide. | P2 | Low (0.5 sprint) | T1.2 |

### Dependencies Graph

```
T1.1 (Virtual ListView) ──► T1.2 (Detail Panel)
         │                         │
         ├──► T1.3 (Keyboard)      │
         ├──► T1.4 (Multi-select)  │
         ├──► T1.5 (Toggle Switch) │
         │         │               │
         │         ▼               │
         │    T1.6 (Visual Polish) │
         │                         │
         ├──► T1.7 (Tag Chips)     │
         │                         ▼
         └──► T1.8 (Rich Tooltips)
```

### Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Virtual ListView breaks existing category-collapse UX | High | Build `VirtualCategoryListView` that groups items with collapsible headers; test with >4 000 items before replacing |
| Custom toggle control inconsistent across DPI scales | Medium | Test at 100%, 125%, 150%, 200% DPI; use `Graphics.DpiX` for scaling |
| Keyboard shortcuts conflict with system shortcuts | Low | Use `ProcessCmdKey` override; avoid system-reserved combos |

### Stretch Goals

- Card view / List view toggle (two rendering modes for the same `ListView`)
- Drag tweaks onto profile names in sidebar to add them
- "Bulk select by tag" context menu
- Animated category section expand/collapse with easing

---

## T2 — Accessibility & Internationalization

### Problem Statement

The GUI has **zero** accessibility properties set (`AccessibleName`, `AccessibleDescription`,
`AccessibleRole`). Screen readers get auto-generated names like "Button1". Tab order is
undefined. Additionally, `Locale.cs` has 17 string keys across 6 languages, but the GUI
hardcodes English strings — `Locale.T()` is used almost exclusively in CLI output.

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T2.1 | **Accessibility audit & annotation pass** — systematically add `AccessibleName` and `AccessibleDescription` to every interactive control in `MainForm.cs` and all 62 dialog forms. Define explicit `TabIndex` ordering for keyboard navigation. Document the audit in a checklist. | P0 | High (2 sprints) | — |
| T2.2 | **Keyboard-only navigation verification** — ensure every feature is reachable without a mouse: category tree, search, tweak list, apply/remove, all dialogs, all menus. Add `&` accelerator keys to all button/menu labels. Test with Narrator enabled. | P0 | Medium (1 sprint) | T2.1 |
| T2.3 | **Locale string extraction** — audit all GUI source files for hardcoded English strings. Replace with `Locale.T("key")` calls. Expand the string table from 17 keys to ~200+ keys covering all UI labels, button text, status messages, dialog titles, error messages. | P1 | High (2 sprints) | — |
| T2.4 | **Migrate to `.resx` ResourceManager** — replace the hand-rolled `Dictionary<string, Dictionary<string, string>>` in `Locale.cs` with standard `.resx` resource files per locale. Enables tooling support (ResXResourceManager), fallback chains, and satellite assemblies. | P1 | Medium (1 sprint) | T2.3 |
| T2.5 | **Add 4 new locales** — Chinese Simplified (zh-CN), Korean (ko), Arabic (ar) with RTL layout support, Portuguese (pt-BR). Each locale requires translating ~200 string keys. | P2 | Medium (1 sprint) | T2.4 |
| T2.6 | **High-contrast theme** — add a "High Contrast" entry to the 11-theme engine that uses system high-contrast colors, large fonts, thick borders, and no background images. Verify with Windows High Contrast mode. | P2 | Low (0.5 sprint) | — |

### Dependencies Graph

```
T2.1 (Accessibility Audit) ──► T2.2 (Keyboard Navigation)
T2.3 (String Extraction) ──► T2.4 (.resx Migration) ──► T2.5 (New Locales)
T2.6 (High Contrast Theme) — independent
```

### Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| RTL layout (Arabic) breaks WinForms panel layouts | Medium | Use `RightToLeftLayout = true` on Forms; test all 62 dialogs |
| 200+ locale keys create translation maintenance burden | Medium | Use community PR template for translations; machine-translate then review |
| `.resx` migration breaks existing `Locale.T()` callers | Medium | Keep `Locale.T()` as facade; internal implementation swaps to `ResourceManager` |

### Stretch Goals

- Runtime locale switching (hot-swap language without restart)
- Crowdin integration for community translations
- Screen reader test automation (UIAutomation + Narrator toggle)

---

## T3 — CLI Overhaul & Scripting Power

### Problem Statement

The CLI has 53 properties in `CliArgs.cs` parsed by a hand-rolled parser. The flag
surface has grown organically and lacks discoverability (`--help` doesn't group commands).
There is no structured JSON output mode for scripting, and error exit codes are not
documented. The PowerShell module (Sprint 103) wraps CLI calls — it would benefit from
a cleaner underlying interface.

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T3.1 | **Introduce subcommand structure** — reorganize flat `--flags` into `verb noun` subcommands while preserving backward compatibility. Example: `regilattice tweak apply <id>`, `regilattice tweak status <id>`, `regilattice profile apply gaming`, `regilattice scan quick-wins`, `regilattice report html --output report.html`. Old `--flags` remain as aliases. | P0 | Medium (1 sprint) | — |
| T3.2 | **Structured JSON output (`--output json`)** — every command that produces output supports `--output json` for machine-readable structured results. Includes exit codes: 0 = success, 1 = partial failure, 2 = invalid args, 3 = permission denied. Document all exit codes. | P0 | Medium (1 sprint) | T3.1 |
| T3.3 | **Auto-generated `--help` with grouped commands** — categorize help output: "Tweak Operations", "Profiles", "Snapshots & Export", "Diagnostics", "Package Managers", "Advanced". Include examples per group. Support `regilattice help <command>` for detailed per-command help. | P1 | Low (0.5 sprint) | T3.1 |
| T3.4 | **Tab completion for PowerShell and bash** — generate `Register-ArgumentCompleter` for PowerShell and bash completion script. Completions for tweak IDs, profile names, category names. Ship as `completions/RegiLattice.ps1` and `completions/regilattice.bash`. | P1 | Medium (1 sprint) | T3.1 |
| T3.5 | **Interactive TUI mode overhaul** — upgrade `--menu` from basic `Console.ReadLine` to a richer terminal UI: arrow-key navigation, colored category headers, search-as-you-type, apply/remove toggles. Use ANSI escape sequences (no external TUI library). | P2 | High (1 sprint) | T3.2 |
| T3.6 | **Batch file support (`--batch <file>`)** — read a `.txt` or `.json` file of tweak IDs to apply/remove in sequence. Support `# comments` in text mode. Report per-tweak results. | P2 | Low (0.5 sprint) | T3.2 |

### Dependencies Graph

```
T3.1 (Subcommands) ──► T3.2 (JSON Output) ──► T3.5 (TUI)
         │                      │
         ├──► T3.3 (Help)       └──► T3.6 (Batch)
         └──► T3.4 (Tab Completion)
```

### Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Subcommand rename breaks existing scripts | High | Keep ALL existing `--flag` syntax as deprecated aliases; warn on stderr; remove in v6.0 |
| JSON output schema changes break consumers | Medium | Version the schema: `{ "schema": "v1", "result": ... }`; document in `docs/Api.md` |

### Stretch Goals

- `regilattice doctor --fix` auto-repairs common issues
- Markdown table output mode (`--output markdown`)
- `regilattice diff <snapshot1> <snapshot2> --output html`

---

## T4 — Enterprise & Compliance Automation

### Problem Statement

IT admins managing fleets of Windows machines need exportable policies and compliance
workflows. RegiLattice has partial GPO export (`GroupPolicyExporter.cs`) and compliance
checking (`ComplianceService.cs`), but lacks Intune OMA-URI export, ADMX/ADML generation,
scheduled compliance scans, and actionable drift remediation.

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T4.1 | **Full ADMX/ADML Group Policy export** — for all `Registry`-kind tweaks with HKLM paths: generate valid `.admx` (policy definitions) and `.adml` (localized strings) XML files deployable via Group Policy Editor. Include per-tweak Explain text from `Description`. Test import in `gpedit.msc`. | P0 | High (2 sprints) | — |
| T4.2 | **Intune OMA-URI export** — map HKLM registry paths to Intune Custom Configuration Profile OMA-URI format (`./Device/Vendor/MSFT/Policy/Config/...`). Generate JSON importable by Intune admin portal. Cover CSP-mapped policies where standard CSP path exists; flag unmappable tweaks. | P1 | High (2 sprints) | — |
| T4.3 | **Scheduled compliance scans** — extend `ScheduledTweakService` to support scheduled compliance checks (daily, weekly, custom). On drift detection: log to `ComplianceHistory`, fire `ToastNotificationService.ShowComplianceDrift()`, optionally auto-remediate (`--compliance-auto-fix`). | P1 | Medium (1 sprint) | — |
| T4.4 | **Compliance trend dashboard** — new `ComplianceTrendDialog` in GUI: line chart of compliance percentage over time (from `ComplianceHistory` entries). Show drift events as red markers. Export chart as PNG. | P2 | Medium (1 sprint) | T4.3 |
| T4.5 | **Baseline policy templates** — ship 4 built-in compliance baselines: "CIS Level 1 Desktop", "CIS Level 1 Server", "DISA STIG Windows 11", "RegiLattice Recommended". Each is a JSON snapshot that `ComplianceService` can check against. | P2 | Medium (1 sprint) | T4.3 |
| T4.6 | **Multi-machine deployment template** — GitHub Actions workflow template (`regilattice-configure.yml`) that provisions a dev machine: install RegiLattice via scoop, apply a profile, export compliance report. Document in `docs/Deployment.md`. | P2 | Low (0.5 sprint) | T3.2 |

### Dependencies Graph

```
T4.1 (ADMX/ADML) — independent
T4.2 (Intune OMA-URI) — independent
T4.3 (Scheduled Compliance) ──► T4.4 (Trend Dashboard)
                             └──► T4.5 (Baseline Templates)
T4.6 (Deployment Template) ◄── T3.2 (JSON Output)
```

### Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| OMA-URI CSP mapping is incomplete — not all HKLM paths have CSP equivalents | High | Flag unmappable tweaks with `[NOT MAPPABLE]`; document coverage percentage |
| ADMX schema validation failure in Group Policy Editor | Medium | Validate generated XML against Microsoft's ADMX schema XSD before shipping |
| Compliance auto-fix applies changes without user confirmation | High | Require explicit `--compliance-auto-fix --force` flags; default is report-only |

### Stretch Goals

- SCCM configuration baseline export
- REST API for remote compliance checking (`regilattice --serve`)
- Compliance report email (SMTP integration)

---

## T5 — Distribution, Trust & Release Pipeline

### Problem Statement

The EXE is not code-signed (SmartScreen blocks it). Auto-updater only detects new
versions but doesn't download or install. No Chocolatey package exists. The MSI/MSIX
pipeline is present but not validated end-to-end in CI.

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T5.1 | **Authenticode code signing** — procure an EV code signing certificate (or standard + SmartScreen reputation). Sign `RegiLattice.GUI.exe`, `RegiLattice.CLI.exe`, and MSI installer in the GitHub Actions release workflow using `signtool.exe`. Store certificate in GitHub Secrets (base64 PFX + password). | P0 | High (1 sprint + procurement lead time) | — |
| T5.2 | **Auto-updater: download + guided install** — extend `AutoUpdater.cs` to download the new MSI/EXE to `%TEMP%`, verify SHA-256 against the GitHub release asset hash, then launch the installer and exit. Show progress bar in `UpdateCheckerDialog`. Never auto-install without user confirmation. | P0 | Medium (1 sprint) | T5.1 |
| T5.3 | **Chocolatey community package** — create `tools/chocolateyinstall.ps1` and `.nuspec` manifest. Publish to `community.chocolatey.org`. Add CI step to auto-submit on tagged release. | P1 | Low (0.5 sprint) | T5.1 |
| T5.4 | **MSIX packaging** — create `.msix` package alongside MSI. Configure identity, capabilities, and visual assets. Test Microsoft Store submission flow (private flight). | P1 | Medium (1 sprint) | T5.1 |
| T5.5 | **Release pipeline hardening** — add to release workflow: (1) binary diff size check (alert if EXE grows >10%), (2) automated smoke test (run CLI `--list` + `--validate` on published EXE), (3) auto-generate release notes from CHANGELOG.md `## [X.Y.Z]` section, (4) upload SHA256SUMS.txt alongside artifacts. | P1 | Medium (1 sprint) | — |
| T5.6 | **Scoop auto-update on release** — CI step to compute SHA-256 of published ZIP, update `scoop/regilattice.json` hash, and auto-commit to scoop bucket repo. | P2 | Low (0.5 sprint) | — |

### Dependencies Graph

```
T5.1 (Code Signing) ──► T5.2 (Auto-updater Download)
         │              ├──► T5.3 (Chocolatey)
         │              └──► T5.4 (MSIX)
T5.5 (Release Hardening) — independent
T5.6 (Scoop Auto-update) — independent
```

### Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| EV certificate procurement takes 2–4 weeks | High | Start procurement immediately in Sprint 1; use standard cert as interim |
| Auto-updater download could be MITM'd | Critical | Verify SHA-256 of downloaded binary against GitHub API response hash; HTTPS only |
| Chocolatey moderation takes 1–3 weeks | Low | Submit early; maintain manual download as primary channel |

### Stretch Goals

- Microsoft Store listing (requires MSIX + app certification)
- WinGet auto-submit via `winget-pkgs` PR bot
- Delta updates (binary patch instead of full download)

---

## T6 — Testing Excellence & Quality Gates

### Problem Statement

Line coverage is 94.9% but branch coverage is 56.8%. Several services lack dedicated
test files (`SmartScanService`, `Locale` string validation). No integration tests
exercise real registry operations (even on isolated hives). No performance regression
tracking. No visual regression testing for the 62 dialogs.

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T6.1 | **Branch coverage push to 75%** — identify the biggest branch-coverage gaps (likely in `TweakEngine` conditional paths, `RegistrySession` error branches, `CorporateGuard` platform checks). Write targeted tests for each uncovered branch. Target: +18% branch coverage. | P0 | High (2 sprints) | — |
| T6.2 | **Dedicated test files for untested services** — create `SmartScanServiceTests.cs`, `LocaleTests.cs` (validate all 6 locales have matching key sets, test `T()` fallback, test format placeholders), `ComplianceHistoryTests.cs`. | P1 | Medium (1 sprint) | — |
| T6.3 | **Virtual registry integration tests** — use `RegLoadKey` / `RegUnLoadKey` to load a temporary hive. Run actual `Apply()` → `DetectStatus()` → `Remove()` cycles on isolated keys. Verify round-trip without touching real system state. Requires admin elevation in CI. | P1 | High (2 sprints) | — |
| T6.4 | **BenchmarkDotNet performance suite** — track `RegisterBuiltins()`, `Search()`, `StatusMap()`, `Filter()` baselines. Run in CI on release builds. Fail if any benchmark regresses >20% from the committed baseline. Publish results as CI artifact. | P1 | Medium (1 sprint) | — |
| T6.5 | **Property-based tests (FsCheck)** — invariant checks on all 4 058 built-in tweaks: non-null/non-empty ID, valid hive prefix on all RegOp paths, no duplicate registry key+value combos, valid `TweakKind`, `ImpactScore` in 1–5 range, `SafetyRating` in 1–5 range. | P2 | Low (0.5 sprint) | — |
| T6.6 | **Mutation testing (Stryker.NET)** — run on Core library. Target 60%+ mutation kill score. Identify surviving mutants and add targeted tests. | P2 | Medium (1 sprint) | T6.1 |
| T6.7 | **AppConfig semantic validation + tests** — add `AppConfig.Validate()` that checks: `MaxWorkers` ∈ [1, 128], `Theme` ∈ known themes, `Locale` ∈ known locales, `LogLevel` ∈ [0, 5]. Return `IReadOnlyList<string>` of validation errors. Add tests for every edge case. | P1 | Low (0.5 sprint) | — |

### Dependencies Graph

```
All tasks are independent and can be parallelized across sprints.
T6.1 (Branch Coverage) should be done before T6.6 (Mutation Testing).
```

### Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Virtual registry tests require admin in CI | Medium | Use `[Trait("RequiresAdmin", "true")]`; skip in non-elevated CI; run in a separate elevated job |
| BenchmarkDotNet baseline varies by CI runner hardware | Medium | Use relative thresholds (% regression), not absolute times; pin to `windows-latest` runner |

### Stretch Goals

- Visual regression testing (WinAppDriver screenshot comparison)
- Code coverage badge in README from CI artifacts
- Test report published as GitHub Pages (per-PR coverage diff)

---

## T7 — Plugin Ecosystem & Community

### Problem Statement

The plugin system validates hash and namespace but lacks code signing, sandboxing,
and a curated online marketplace. The pack creator workflow is manual (edit JSON by hand).

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T7.1 | **Curated online marketplace** — create `RajwanYair/RegiLattice-Packs` GitHub repo as CDN. Publish 5 starter packs: "Privacy Shield" (50 tweaks), "Gaming Boost" (40 tweaks), "Corporate Lock" (30 tweaks), "Minimal Clean" (20 tweaks), "Developer Setup" (35 tweaks). `PackIndex.json` served via GitHub raw URL. | P1 | Medium (1 sprint) | — |
| T7.2 | **Pack Creator Studio dialog** — GUI wizard: step 1 = name/description/author; step 2 = drag tweaks from main list into pack list; step 3 = set tags and metadata; step 4 = preview JSON + validation report; step 5 = export `.rlpack` file. | P1 | High (1.5 sprints) | — |
| T7.3 | **Pack GPG signing** — optional GPG signature file (`.rlpack.sig`) alongside pack JSON. `PackLoader` verifies signature if present. Show "Verified" badge in marketplace dialog for signed packs. Ship public key in app resources. | P2 | Medium (1 sprint) | T7.1 |
| T7.4 | **Plugin sandboxing** — run `ApplyAction`/`DetectAction` delegates from third-party packs in a separate `Process` communicating via named pipes. Contain crashes and resource abuse. Timeout after 30 seconds. | P2 | High (2 sprints) | — |
| T7.5 | **Community submission workflow** — GitHub Issue template for pack submissions. CI validates submitted JSON (schema, ID uniqueness, hash check). Auto-merge to packs repo on approval. | P2 | Low (0.5 sprint) | T7.1 |
| T7.6 | **Pack dependency resolution** — packs can declare `DependsOn` on other packs. `PackManager` resolves the dependency chain and installs prerequisites first. | P2 | Medium (1 sprint) | T7.1 |

### Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Malicious pack submitted to marketplace | High | All packs are registry-only (no code execution); require GPG signature for "Verified" status; manual review for marketplace inclusion |
| Sandboxing via named pipes adds latency | Medium | Only sandbox third-party packs with `ApplyAction` delegates; built-in RegOp-only packs bypass sandbox |

### Stretch Goals

- Pack rating and review system (anonymous stars, opt-in)
- Deep-link URLs (`regilattice://install-pack?name=privacy-shield`)
- Pack auto-update checking with notification

---

## T8 — Tweak Expansion & Intelligence

### Problem Statement

The tweak count is 4 058; the target is 5 000. Several Windows subsystems lack coverage
(BitLocker advanced, AppLocker, Windows Sandbox). The intelligence engine has health
scores and smart scan but lacks predictive score previews.

### Tasks

| ID | Task | Priority | Effort | Depends On |
|----|------|----------|--------|------------|
| T8.1 | **5 new tweak modules (50 tweaks)** — `BitLockerAdvanced.cs` (12), `AppLockerWdac.cs` (10), `WindowsSandboxAdv.cs` (8), `PrinterAdvanced.cs` (10), `WdacCodeIntegrity.cs` (10). All with full `ApplyOps`/`RemoveOps`/`DetectOps`, valid IDs, no duplicates. | P1 | Medium (1 sprint) | — |
| T8.2 | **Sprint tweak additions (5 × 50 = 250 tweaks)** — continue the established pattern of adding 50 tweaks per sprint across existing and new modules. Targets: 4 308 → 4 558 → 4 808 → 5 058 → 5 108. | P2 | Ongoing (5 sprints) | — |
| T8.3 | **Score-change preview on hover** — when hovering a tweak or category header in the GUI, show a tooltip: "Applying this would change: Privacy +5, Performance +2". Computed from `HealthScoreService` by diffing current vs. hypothetical status. | P2 | Medium (1 sprint) | — |
| T8.4 | **AI-enhanced tweak descriptions** — one-time LLM pass to generate clearer `Description` and `ExpectedResult` for all 4 058 tweaks. Commit improved strings as source code. Manual review before merge. | P2 | Medium (1 sprint) | — |
| T8.5 | **Custom user-defined profiles** — extend `ProfileDefinitions` to support user-created profiles persisted in config. GUI: "Save Current as Profile" button. CLI: `regilattice profile create <name> --tweaks id1,id2,...` | P1 | Medium (1 sprint) | — |

---

## Sprint Plan

### Quarter 1: Foundation (Sprints 106–113)

> **Milestone M1**: Virtual scrolling live, accessibility pass complete, CLI restructured.

| Sprint | Theme | Tasks | Exit Criteria |
|--------|-------|-------|---------------|
| **106** | T1 | T1.1 — Virtual ListView (part 1: rendering engine, category headers, scroll, DPI) | `MainForm` renders 4 058 tweaks with <30 live Controls; smooth 60fps scroll |
| **107** | T1 | T1.1 — Virtual ListView (part 2: toggle, apply/remove, status colors, search filter integration) | Full functional parity with old `FlowLayoutPanel`; all existing GUI tests pass |
| **108** | T2, T6 | T2.1 — Accessibility audit (MainForm + top 20 dialogs), T6.7 — AppConfig validation | Every interactive control in MainForm has `AccessibleName`; Narrator reads tweak labels |
| **109** | T3 | T3.1 — Subcommand structure, T3.2 — JSON output mode | `regilattice tweak apply <id>` works; `--output json` produces valid JSON; old `--flags` still work |
| **110** | T1, T2 | T1.2 — Detail side panel, T2.2 — Keyboard navigation verification | Detail panel shows all tweak metadata; all features reachable via keyboard only |
| **111** | T6 | T6.1 — Branch coverage push (part 1: TweakEngine + RegistrySession branches) | Branch coverage: 56.8% → 65%+ |
| **112** | T1 | T1.3 — Keyboard shortcuts, T1.4 — Multi-select operations | `Ctrl+F/Space/Z/A/Escape` work; multi-select apply/remove functional |
| **113** | T6, T8 | T6.2 — SmartScanServiceTests + LocaleTests, T8.1 — 5 new tweak modules (50 tweaks) | New tests green; 4 108 tweaks total |

**M1 Deliverable: v4.4.0** — Virtual scrolling, accessible UI, subcommand CLI, 4 108 tweaks, 2 000+ tests.

---

### Quarter 2: Enterprise & Trust (Sprints 114–121)

> **Milestone M2**: Code-signed binaries, Intune/GPO export, enhanced compliance.

| Sprint | Theme | Tasks | Exit Criteria |
|--------|-------|-------|---------------|
| **114** | T5 | T5.1 — Code signing (setup + CI integration) | `signtool verify /pa RegiLattice.GUI.exe` succeeds; SmartScreen doesn't block |
| **115** | T5 | T5.2 — Auto-updater download + guided install | UpdateCheckerDialog downloads MSI, verifies hash, launches installer on user click |
| **116** | T4 | T4.1 — ADMX/ADML export (part 1: schema generation, XML templates) | Valid `.admx` generated for 50 test tweaks; imports in `gpedit.msc` |
| **117** | T4 | T4.1 — ADMX/ADML export (part 2: full coverage + GUI wiring), T5.5 — Release hardening | All HKLM Registry-kind tweaks exportable; smoke test in CI; SHA256SUMS published |
| **118** | T4 | T4.2 — Intune OMA-URI export (part 1: CSP mapping database) | OMA-URI JSON generated for mapped policies; unmapped flagged |
| **119** | T4, T8 | T4.2 — Intune OMA-URI (part 2: full coverage + CLI `--export-intune`), T8.2 — 50 tweaks | Intune JSON importable; 4 158 tweaks total |
| **120** | T2 | T2.3 — Locale string extraction (200+ keys) | All GUI labels go through `Locale.T()`; 200+ keys in string tables |
| **121** | T6 | T6.1 — Branch coverage push (part 2), T6.4 — BenchmarkDotNet suite | Branch coverage: 65% → 75%+; performance baselines established |

**M2 Deliverable: v4.5.0** — Code-signed, auto-update, ADMX + Intune export, fully localized GUI, 75% branch coverage.

---

### Quarter 3: Polish & Community (Sprints 122–129)

> **Milestone M3**: Plugin marketplace live, visual polish, WinUI 3 preparation.

| Sprint | Theme | Tasks | Exit Criteria |
|--------|-------|-------|---------------|
| **122** | T1 | T1.5 — Animated toggle switch control | Custom `ToggleSwitchControl` renders in all 11 themes; DPI-safe |
| **123** | T1 | T1.6 — WinForms visual polish pass | Rounded panels, Segoe Fluent Icons, Mica-like tint on all forms |
| **124** | T7 | T7.1 — Online marketplace (5 starter packs), T7.5 — Submission workflow | `MarketplaceDialog` lists 5 packs from GitHub CDN; install/uninstall works |
| **125** | T7 | T7.2 — Pack Creator Studio dialog | Full wizard flow: create → select tweaks → metadata → export `.rlpack` |
| **126** | T2 | T2.4 — `.resx` migration, T2.5 — 4 new locales (zh-CN, ko, ar, pt-BR) | 10 locales total; Arabic RTL renders correctly |
| **127** | T3, T8 | T3.4 — Tab completion, T3.3 — Grouped help, T8.5 — Custom profiles | PowerShell tab completion for tweak IDs; user-defined profiles persist |
| **128** | T4 | T4.3 — Scheduled compliance scans, T4.4 — Compliance trend dashboard | Daily compliance check fires toast on drift; trend chart in GUI |
| **129** | T6, T8 | T6.5 — FsCheck property tests, T8.2 — 50 tweaks | 4 258 tweaks; invariant checks pass on all tweaks |

**M3 Deliverable: v4.6.0** — Online marketplace, visual polish, 10 locales, compliance automation, 4 258 tweaks.

---

### Quarter 4: Scale & Harden (Sprints 130–137)

> **Milestone M4**: 5 000+ tweaks, plugin sandboxing, MSIX, mutation tested.

| Sprint | Theme | Tasks | Exit Criteria |
|--------|-------|-------|---------------|
| **130** | T5 | T5.3 — Chocolatey package, T5.4 — MSIX packaging | `choco install regilattice` works; MSIX installs on Win11 |
| **131** | T7 | T7.3 — Pack GPG signing | Signed packs show "Verified" badge; unsigned packs show warning |
| **132–133** | T7 | T7.4 — Plugin sandboxing (2 sprints) | Third-party `ApplyAction` delegates run in isolated process; crash contained |
| **134** | T6 | T6.3 — Virtual registry integration tests | `Apply` → `Detect` → `Remove` round-trip on isolated hive succeeds in CI |
| **135** | T6 | T6.6 — Mutation testing (Stryker.NET) | 60%+ mutation kill score on Core library |
| **136** | T8 | T8.2 — 100 tweaks (double sprint) | 4 558 tweaks total |
| **137** | T8 | T8.2 — 100 tweaks (double sprint), T8.3 — Score preview | 4 858 tweaks total; hover shows predicted score change |

**M4 Deliverable: v5.0.0** — 5 000+ tweaks (with continued Sprint additions), MSIX + Chocolatey + signed, plugin sandboxing, mutation tested, 2 500+ tests.

---

## Technical Considerations & Investigation Items

These items require research or proof-of-concept before committing to a sprint:

| # | Item | Why It Matters | Investigation Action |
|---|------|---------------|---------------------|
| 1 | **WinUI 3 migration feasibility** | WinUI 3 gives native Win11 look (Mica, NavigationView, ToggleSwitch) but is a full GUI rewrite. WinForms polish (T1.6) may be sufficient. | Build a single-page WinUI 3 prototype showing the tweak list + detail panel. Measure effort vs. visual gain. Decide if v5.1 or v6.0. |
| 2 | **EV code signing certificate vendor** | EV cert removes SmartScreen warning immediately (no reputation build). Costs $200-400/year. | Evaluate DigiCert, Sectigo, GlobalSign. Determine if GitHub Secrets can hold the HSM token or if a cloud HSM (Azure Key Vault) is needed. |
| 3 | **CSP path coverage for Intune export** | Not all HKLM registry paths have OMA-URI CSP equivalents. Coverage may be <50%. | Build a mapping table by cross-referencing `RegOp` paths with Microsoft's [CSP reference](https://learn.microsoft.com/en-us/windows/client-management/mdm/policy-configuration-service-provider). Measure coverage before committing T4.2. |
| 4 | **Virtual registry hive for testing** | `RegLoadKey`/`RegUnLoadKey` requires admin and a hive file. CI runners may not support this. | Test on `windows-latest` GitHub Actions runner with elevated step. If blocked, fall back to in-memory registry mock. |
| 5 | **RTL layout in WinForms** | Arabic locale (T2.5) requires `RightToLeftLayout = true`. Some custom-drawn controls may not respect this. | Test all 62 dialogs with `RightToLeft = Yes` and identify broken layouts before committing to Arabic. |
| 6 | **Named pipe sandboxing performance** | T7.4 uses named pipes for plugin isolation. Serialization overhead per registry op may be unacceptable. | Benchmark: 100 `SetDword` calls via named pipe vs. direct. If >200ms overhead, consider `AppDomain` isolation instead. |

---

## Success Metrics by Milestone

| Metric | M1 (v4.4) | M2 (v4.5) | M3 (v4.6) | M4 (v5.0) |
|--------|-----------|-----------|-----------|-----------|
| Tweaks | 4 108 | 4 158 | 4 258 | 5 000+ |
| Tests | 2 000+ | 2 200+ | 2 350+ | 2 500+ |
| Branch Coverage | 60%+ | 75%+ | 75%+ | 80%+ |
| Live GUI Controls | ~30 | ~30 | ~30 | ~30 |
| Code Signed | ❌ | ✅ | ✅ | ✅ |
| Accessibility | Partial | Partial | Full | Full |
| Locales | 6 | 6 (GUI wired) | 10 | 10 |
| Plugin Marketplace | Local | Local | Online | Online + Signed |
| Distribution | GitHub + Scoop + WinGet | + Signed | + 10 locales | + Chocolatey + MSIX |

---

## Priority Justification

**Why T1 (GUI Performance) is P0:**
The FlowLayoutPanel with 4 058 Controls is a scalability wall. Every search keystroke
forces layout recalculation across all controls. This gets worse as tweaks grow toward
5 000. Virtual scrolling must happen before any other GUI work.

**Why T2 (Accessibility) is P0:**
Accessibility is not a feature — it is a legal and ethical requirement. Every new GUI
feature built without accessibility in mind creates more retrofit work later. Doing T2
early means all subsequent GUI work (T1.5, T1.6, T7.2) is automatically accessible.

**Why T5.1 (Code Signing) blocks distribution:**
Unsigned EXEs trigger SmartScreen, Windows Defender, and corporate security tools.
No matter how good the product is, users can't run it if their OS blocks it. This is
the single biggest friction point for new user adoption.

---

## Appendix: Superseded Roadmap Items

The following items from `docs/Roadmap.md` (Phases 1–10) are **already completed** and
should not appear in future sprint planning:

- Phases 1–7: 70/70 items completed (UX, monitoring, appearance, network, startup, power, privacy)
- Phase A: 5/8 items completed (auto-updater check, portable, silent, first-run wizard, onboarding)
- Phase C: 6/7 items completed (health score, smart scan, impact/safety, conflict detection, dependency graph, profile compare)
- Phase D: 3/8 items completed (PowerShell module, compliance report, silent mode)
- Phase G: 2/5 items completed (NLP search, profile wizard)
- Phase H: 5/10 modules completed

Items explicitly **deferred to v6.0+** (not in this roadmap):
- E2: Full WinUI 3 migration (requires investigation item #1 above)
- D7: Local REST API
- F4: Community safety ratings (crowd-sourced)
- G4: LLM integration (opt-in AI)
