# RegiLattice — Strategic Roadmap v2

> **Baseline**: v6.34.0 — 7718 tweaks · 158 categories · 195 modules · 3,304 tests · 11 themes
> **Last updated**: 2026-05-12
> **Stack**: C# 13 / .NET 10.0-windows (x64) · WinForms · xUnit 2.9.3
> **Repository**: [github.com/RajwanYair/RegiLattice](https://github.com/RajwanYair/RegiLattice)

This document is a **ground-up strategic reassessment** of every major project decision —
language, framework, architecture, data layer, testing, security, distribution, documentation,
community, and ecosystem. It replaces all prior roadmap content with a consolidated plan that
questions existing choices (including ones that seemed settled), compares against best-in-class
alternatives, harvests proven patterns from competitors, and charts a clear path from v6.33
through v10+.

**Guiding principle**: *Ship less, ship better.* The project's current breadth (7,718 tweaks)
is an overwhelming competitive advantage — but breadth without depth of quality, modern UX,
and community trust is a liability. Every decision below optimizes for **user trust**,
**maintainability by a single developer**, and **long-term project health**.

---

## Table of Contents

- [Part I — Project State Assessment](#part-i--project-state-assessment)
- [Part II — Competitive Landscape](#part-ii--competitive-landscape)
- [Part III — Strategic Decision Audit (15 Decisions)](#part-iii--strategic-decision-audit)
- [Part IV — Technical Debt Inventory](#part-iv--technical-debt-inventory)
- [Part V — Improvement Roadmap (Phases A–G)](#part-v--improvement-roadmap)
- [Part VI — Success Metrics & KPIs](#part-vi--success-metrics--kpis)
- [Part VII — Risk Register](#part-vii--risk-register)
- [Part VIII — Migration Sequence](#part-viii--migration-sequence)
- [Part IX — Appendix: Completed Work (v6.0–v6.33)](#part-ix--appendix-completed-work-v60v633)

---

## Part I — Project State Assessment

### What We Have (Strengths)

| Asset | Scale | Notes |
|-------|-------|-------|
| Tweak library | **7,718** declarative tweaks | 10x–100x larger than any competitor |
| Category coverage | **158** categories | Privacy, security, gaming, debloat, enterprise, accessibility |
| CLI surface | **25+** commands | `--json` output, conditional flags, interactive wizard, Ansible/DSC export |
| GUI themes | **11** colour themes | Catppuccin, Nord, Dracula, Tokyo Night, Gruvbox, etc. |
| Profile system | **5** built-in + custom profiles | business, gaming, privacy, minimal, server |
| Package managers | **5** GUI dialogs | WinGet, Scoop, Chocolatey, pip, PowerShell modules |
| Plugin system | JSON `.rlpack` format | 5 official packs, marketplace foundation |
| Test coverage | **3,304** tests, ≥90% line | Zero skipped, zero suppressed warnings |
| Services layer | 15+ services | Analytics, favorites, ratings, history, snapshots, health scores |
| Engine features | Transactional apply, conflict detection, dependency resolution, search ranking | Production-grade engine |

### What Hurts (Weaknesses)

| Weakness | Impact | Root Cause |
|----------|--------|------------|
| **WinForms GUI** | Dated look, no GPU rendering, no MVVM, bitmap scaling, no vector icons | Migration speed choice (Tkinter → WinForms 1:1) |
| **~40MB binary** | Distribution friction; WPD achieves same in 335KB | Self-contained .NET + framework overhead |
| **Single developer** | Bus factor = 1; burnout risk (Optimizer cautionary tale) | No contributor pipeline |
| **Zero stars / zero users** | No social proof; discoverability problem | New project, no marketing |
| **No auto-updater** | Users must manually check for updates | Not yet implemented |
| **No code signing** | Windows SmartScreen blocks unsigned EXEs | No certificate |
| **No ARM64** | Missing Snapdragon X laptop market | .NET 10 WinForms ARM64 not yet shipped |
| **8 JSON files for data** | File locks on OneDrive, no ACID, no queries | Quick-start persistence choice |
| **28-file manual version bump** | Error-prone, time-consuming, drift | Organic growth of metadata files |
| **Documentation sprawl** | 8 instruction files (~15K words), 10 skills, overwhelming for AI context | Accumulated organically |
| **God class TweakEngine** | 3,000+ LOC, untestable in isolation, no interface contracts | Monolithic architecture |
| **50K LOC of repetitive tweak data** | Contributor barrier (must know C#), recompilation for tweaks | All tweaks in C# static classes |
| **No enterprise compliance features** | Missing CIS/STIG alignment, compliance audit reports | Consumer-focused design |
| **No structured logging / observability** | Debugging requires manual log reading | Ad-hoc logging approach |

### Where We Stand vs Competitors

RegiLattice occupies a **unique niche**: the most comprehensive, typed, testable registry tweak
toolkit with a real GUI, CLI, profiles, snapshots, and plugin system. No competitor matches
this combination. But competitors lead in: **community** (WinUtil: 239 contributors),
**distribution** (Sophia: `irm script.sophia.team | iex`), **binary size** (WPD: 335KB),
**i18n** (Optimizer: 24 languages), and **GUI modernity** (WinUtil: WPF, SophiApp: WinUI 3).

---

## Part II — Competitive Landscape

### Competitor Matrix (May 2026, verified)

| Dimension | **RegiLattice** | **WinUtil** | **Sophia Script** | **Optimizer** | **BloatyNosy/Winpilot** | **WPD** |
|-----------|----------------|-------------|-------------------|---------------|------------------------|---------|
| **Language** | C# 13 / .NET 10 | PowerShell | PowerShell 5.1/7 | C# / .NET FW 4.8.1 | C# + WebView2 | C++ (Win32) |
| **GUI framework** | WinForms | **WPF/XAML** | CLI + SophiApp (**WinUI 3**) | WinForms | WebView2 + native | Raw Win32 API |
| **GitHub stars** | ~0 (new) | **54.3k** | **9.3k** | 18.2k (archived) | 5.6k | N/A (closed) |
| **Contributors** | 1 | **239** | ~15 | ~20 | ~5 | 2 |
| **Tweak count** | **7,718** | ~200 | ~150 functions | ~50 toggles | ~30 plugins | ~30 |
| **CLI depth** | **25+ cmds**, JSON, wizard | Launch only | Full PS7 module | Silent mode | None | CLI args |
| **Profile system** | **5 built-in + custom** | 3 presets | None | Templates | None | None |
| **Undo / revert** | **Per-tweak + snapshots** | Limited | Per-function | Limited | None | None |
| **Package managers** | **5 integrated (GUI)** | WinGet | None | None | None | None |
| **Plugin / pack system** | **JSON packs (5 official)** | None | None | None | Plugin arch | None |
| **Themes** | **11** | System theme | N/A | Dark mode | 1 | Dark mode |
| **ARM64** | No | No | **Yes** | No | No | No |
| **i18n** | 2 + 8 stubs | English only | Multi-lang | **24 languages** | English | Multi-lang |
| **Code signing** | No | No | No | No | No | No |
| **Auto-updater** | No | N/A (script) | N/A (script) | Built-in | No | No |
| **One-liner install** | No | **`irm christitus.com/win \| iex`** | **`irm script.sophia.team \| iex`** | EXE download | EXE download | ZIP |
| **Binary size** | ~40MB | ~50MB (script) | ~2MB (script) | ~5MB | ~3MB | **335KB** |
| **Test suite** | **3,304 xUnit tests** | Pester tests | None | None | None | None |
| **License** | MIT | MIT | MIT | GPL-3.0 | MIT | Proprietary |
| **Status** | Active (solo) | Active (community) | Active (small team) | **Archived** → NXT | Active (sporadic) | Stale (2022) |
| **Docs** | Extensive (15K+ words) | **Docs site** (Docusaurus) | Wiki + README | FAQ + CHANGELOG | None | None |

### Best Practices to Harvest (Prioritized)

| Priority | Source | Practice | How to Adopt |
|----------|--------|----------|--------------|
| **P0** | WinUtil | One-liner install: `irm ... \| iex` | Create `install.ps1` on GitHub Pages domain |
| **P0** | WinUtil | WPF GUI with MVVM | Phase D migration (v8.0) |
| **P0** | Optimizer | i18n (24 languages via community PRs) | Crowdin + `.resx` resources (Phase F.3) |
| **P0** | Sophia | ARM64 support | Add `win-arm64` RID to publish matrix |
| **P1** | WinUtil | Docs site (Docusaurus/MkDocs) | Deploy via GitHub Pages (Phase A) |
| **P1** | Sophia | `gpedit.msc` visibility for policies | Verify all `HKLM\...\Policies\` tweaks appear |
| **P1** | Optimizer | Automation via JSON templates | Already have profiles + packs; enhance with YAML recipes |
| **P2** | WPD | Ultra-small portable binary | Investigate NativeAOT + trimming when WinForms supports it |
| **P2** | BloatyNosy | Plugin architecture | Already have `.rlpack`; extend with DLL plugins |
| **P2** | WinUtil | Community contribution pipeline | Good First Issues, CONTRIBUTING.md, Discord/Discussions |
| **P3** | Optimizer | DNS/HOSTS management GUI | Add as RegiLattice.Tools dialog |
| **P3** | WinUtil | EXE wrapper with code signing | SignPath.io for OSS (free) |

### Competitive Positioning Statement

> **RegiLattice is the most comprehensive, typed, and testable Windows registry tweak engine
> in the open-source ecosystem.** It offers 10x–100x more tweaks than any competitor, with
> declarative tweak definitions, compile-time validation, 5 built-in profiles, per-tweak
> undo, snapshot/restore, 5 integrated package managers, and a 3,304-test quality gate.
> What it lacks in community and GUI modernity, it compensates with engineering rigour.

---

## Part III — Strategic Decision Audit

### Decision 1: Programming Language — C# 13 vs Alternatives

**Previous verdict**: KEEP C# 13. **Reassessment**: **CONFIRMED — KEEP.**

| Option | Binary Size | Dev Velocity | Registry Interop | Type Safety | Verdict |
|--------|-------------|-------------|-----------------|-------------|---------|
| **C# 13 / .NET 10** | ~40MB (self-contained) | High | Excellent (`Microsoft.Win32.Registry`) | Excellent | **KEEP** |
| Rust | ~2MB | Low (rewrite) | Manual FFI | Excellent | Reject (rewrite cost prohibitive) |
| Go | ~8MB | Medium | Manual `syscall` | Good | Reject (no GUI, no NuGet) |
| PowerShell | ~2MB (script) | Medium | Native | Weak | Reject (not scalable at 7.7K tweaks) |
| C++ / Win32 | ~335KB | Very Low | Native | Manual | Reject (maintenance nightmare) |

**Rationale**: C# 13 with .NET 10 provides the best balance of registry API quality,
development velocity, NuGet ecosystem access, and type safety. The 40MB binary is the
main downside, addressable via NativeAOT when WinForms support ships.

**Action items**:
- [ ] Track dotnet/winforms#4649 (NativeAOT for WinForms) — when available, binary drops to ~10–15MB
- [ ] Target .NET LTS releases for production (10 LTS → 12 LTS)
- [ ] Maintain PowerShell module as a thin CLI wrapper, not a rewrite target

---

### Decision 2: GUI Framework — WinForms → WPF

**Previous verdict**: MIGRATE TO WPF (Phase D, v8.0). **Reassessment**: **CONFIRMED — but
consider WinUI 3 as a stretch goal for v10+.**

| Option | Pros | Cons | Migration Cost | Verdict |
|--------|------|------|----------------|---------|
| **WinForms** (current) | Working, stable, 11 themes done | No MVVM, no GPU, no vector icons, dated look | N/A | **MIGRATE AWAY** |
| **WPF** | MVVM, GPU-accelerated, XAML, `WindowsFormsHost` interop, WinUtil validates | Windows-only, ~6mo migration | Medium (incremental) | **ADOPT (v8.0)** |
| **WinUI 3** | Modern Fluent Design, SophiApp 2.0 validates, Microsoft-recommended | No interop with WinForms, MSIX packaging complexity, less mature | High (full rewrite) | **STRETCH GOAL (v10+)** |
| **Avalonia UI** | Cross-platform, XAML-like | Registry = Windows-only → zero cross-platform benefit | High | Reject |
| **.NET MAUI Blazor** | Cross-platform, web tech | Poor desktop experience, Chromium overhead | High | Reject |
| **WebView2** | HTML/CSS layout flexibility (BloatyNosy model) | ~150MB Chromium dependency, security surface | Medium | Reject |

**Key WPF architecture decisions**:
- MVVM with `CommunityToolkit.Mvvm` (source generators)
- `VirtualizingStackPanel` for 7,718 tweaks (mandatory)
- XAML resource dictionaries for all 11 themes (replacing programmatic `Color` objects)
- `WindowsFormsHost` for incremental migration — one panel per sprint
- Consider `MaterialDesignThemes` or `WPF-UI` (Fluent 2 components) for modern controls

**Why not WinUI 3 now**: SophiApp 2.0's WinUI 3 adoption validates the framework, but
WinUI 3 has no `WindowsFormsHost` equivalent — migration is all-or-nothing. WPF allows
incremental panel migration while keeping the app functional throughout. WinUI 3 becomes
the v10+ candidate once the WPF migration is complete and stable.

---

### Decision 3: Architecture — DI + Interface Segregation

**Previous verdict**: ADOPT DI. **Reassessment**: **CONFIRMED — but scope down from CQRS.
Add Clean Architecture shell boundaries.**

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **Monolithic TweakEngine** (current) | Simple, everything in one place | God class (3K+ LOC), untestable, no contracts | **REFACTOR** |
| **DI + Interface Segregation** | Testable, mockable, SOLID | More files, 10ms startup cost | **ADOPT** |
| **Full Clean Architecture** | Strict layer boundaries, domain purity | Over-engineered for desktop | **Partial adopt** (boundaries yes, full ports/adapters no) |
| **CQRS + MediatR** | Clean separation | 5+ NuGet deps, over-engineered | **Reject** |

**Extracted interfaces (6 from TweakEngine + 4 from RegistrySession)**:

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

**New consideration — Lightweight Event Bus**: Replace direct cross-cutting calls (Analytics,
TweakHistory, HealthScore) with a simple in-process event bus. No external dependencies.

```csharp
public sealed record TweakAppliedEvent(string Id, TweakResult Result, TimeSpan Duration);
engine.Events.Subscribe<TweakAppliedEvent>(e => analytics.RecordApply(e.Id));
```

---

### Decision 4: Data Persistence — JSON Files → Repository Abstraction

**Previous verdict**: MIGRATE TO SQLite. **Reassessment**: **REVISED — Repository
abstraction first, SQLite as the _second_ backend. Keep JSON as the default until v9.0.**

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **8 JSON files** (current) | Human-readable, zero dependencies, easy debugging | No ACID, no queries, file locks on OneDrive | **Abstract away** |
| **SQLite** | ACID, structured queries, single file, indexing | +2MB binary, migration code, less debuggable | **ADOPT as optional backend** |
| **LiteDB** | Document DB, LINQ queries, embedded, .NET native | Less mature, limited community | Consider as fallback |

**Revised approach**: The immediate win is **not** migrating to SQLite — it's introducing a
**repository abstraction** (`IFavoritesRepository`, `IRatingsRepository`, etc.) that decouples
services from file I/O. The JSON implementation stays as default (backward-compatible).
SQLite becomes a second implementation, selectable via config. This avoids a forced data
migration and reduces risk.

```csharp
// Phase B: Repository interface
public interface IFavoritesRepository
{
    bool IsFavorite(string tweakId);
    void Add(string tweakId);
    void Remove(string tweakId);
    IReadOnlyList<string> All();
}

// Phase B: JSON implementation (default, backward-compatible)
internal sealed class JsonFavoritesRepository : IFavoritesRepository { ... }

// Phase C: SQLite implementation (opt-in via config)
internal sealed class SqliteFavoritesRepository : IFavoritesRepository { ... }
```

**Why this is better**: Users who haven't experienced JSON file issues keep their existing
data format. Enterprise users or power users can switch to SQLite for ACID guarantees.
No forced migration, no risk of data loss, no `.json.migrated` files cluttering the system.

---

### Decision 5: Tweak Definition Format — Hybrid C# + YAML

**Previous verdict**: KEEP C# primary, ADOPT YAML secondary. **Reassessment**: **CONFIRMED —
but defer YAML to v9.0. Focus on auto-registration now.**

The ~5% of tweaks with `ApplyAction`/`DetectAction` delegates cannot be expressed in data
files. For the ~95% pure `ApplyOps`/`RemoveOps`/`DetectOps` tweaks, YAML is viable but
lower priority than architecture and GUI work.

**Immediate improvement (v7.0)**: Eliminate manual `RegisterBuiltins()` listing with
`[TweakModule]` attribute + assembly scanning.

```csharp
// Before: 195 manual registration lines
Register(Privacy.Tweaks);
Register(Performance.Tweaks);
// ... 193 more

// After: automatic discovery
[TweakModule]
internal static class Privacy
{
    public static IReadOnlyList<TweakDef> Tweaks => [ ... ];
}
```

---

### Decision 6: Test Framework — xUnit v2 → v3

**Previous verdict**: MIGRATE TO xUnit v3. **Reassessment**: **CONFIRMED — but only when
FsCheck.Xunit v3 ships stable. Not before Phase A.5.**

| Package | Pinned | Latest | Hold Reason |
|---------|--------|--------|-------------|
| `xunit` | 2.9.3 | 3.x | v3 test class model is breaking |
| `xunit.runner.visualstudio` | 2.8.2 | 3.x | v3 runner only works with v3 framework |
| `Microsoft.NET.Test.Sdk` | 17.14.1 | 18.x | New test-host protocol |
| `FsCheck` | 2.16.6 | 3.x | Breaking attribute API |
| `FsCheck.Xunit` | 2.16.6 | 3.x | Same as FsCheck |

**All 5 packages must be updated simultaneously in one commit.** Attempting partial updates
breaks the test build. Migration happens in a branch with one test project at a time.

**New consideration — Snapshot Testing**: Add `Verify` (snapshot testing library) for
regression testing of CLI output, JSON export format, and generated reports. This catches
output regressions that unit tests miss.

---

### Decision 7: CI/CD & Distribution — Simplify

**Previous verdict**: Reduce workflows 6→3, registries 7→4. **Reassessment**: **CONFIRMED —
and add auto-updater + one-liner install.**

| Dimension | Current | Proposed | Rationale |
|-----------|---------|----------|-----------|
| Workflow count | 3 | **3** | `smoke.yml` merged → `release.yml`, `pages.yml` merged → `ci.yml` (done) |
| Package registries | 4 (winget/scoop/choco/PSGallery) | **4** (winget/scoop/choco/PSGallery) | npm/maven/gem removed (zero evidence of usage) |
| Version bump process | 28 manual files | **1 script** (`Bump-Version.ps1`) | Automate all 28 file updates |
| Code signing | None | **SignPath.io** | Free for OSS; eliminates SmartScreen blocks |
| Auto-updater | None | **Squirrel.Windows** or `dotnet-deltas` | Check GitHub Releases API on startup |
| One-liner install | None | **`irm regilattice.dev/install \| iex`** | Harvest from WinUtil; #1 distribution improvement |
| Mutation testing | Weekly CI | Weekly CI (keep) | Adequate for single-dev cadence |

**Auto-updater design**:
- On startup, check GitHub Releases API for newer version (respect `--no-update-check` flag)
- Show non-modal notification bar in GUI: "v6.35.0 available — [Update] [Skip]"
- CLI: print one-line notice to stderr, never block execution
- Enterprise: respect `HKLM\SOFTWARE\Policies\RegiLattice\DisableAutoUpdate` GPO
- Privacy: no telemetry, no analytics — just HTTPS GET to `api.github.com/repos/.../releases/latest`

---

### Decision 8: Documentation Strategy — Reduce and Automate

**Previous verdict**: Consolidate instruction files, add DocFX. **Reassessment**: **REVISED —
MkDocs Material instead of DocFX, and aggressively reduce instruction files.**

| Area | Current | Proposed | Rationale |
|------|---------|----------|-----------|
| Instruction files | 8 files (~15K words) | **4 files** | Merge: `no-duplication` → `csharp`; `cicd` → `git-workflow`; `lessons-learned` → split actionable rules into other files, archive the rest |
| Skills | 12 skills | **8 skills** | Merge related skills; remove skills for operations that are now scripted |
| API docs | Manual `Api.md` | **MkDocs Material auto-generated** | Source from `///` XML comments + `docfx metadata` |
| Docs site | GitHub Pages (static HTML) | **MkDocs Material** | Better theming, search, versioning than DocFX; used by major .NET projects |
| README.md | ~500 lines + Mermaid diagrams | **~200 lines** + link to docs site | Focused quick-start; detailed docs live on the site |
| SVG assets | 7 manually-updated SVGs | **CI-generated from template** | `Bump-Version.ps1` does string substitution; CI validates |
| CHANGELOG | Manual entries | **Auto-generated from Conventional Commits** | `git-cliff` or `standard-version` |

**`lessons-learned.instructions.md` is the biggest problem**: At 1,000+ lines, it's the largest
instruction file and consumes precious AI context window. Most entries are historical —
"this happened, here's the fix" — not forward-looking rules. **Proposed split**:
- Extract 10–15 still-relevant rules into `csharp.instructions.md` and `testing.instructions.md`
- Archive the full file to `docs/archive/lessons-learned-v6.md`
- New `lessons-learned` stays under 200 lines with only the most critical gotchas

---

### Decision 9: Platform Scope — Windows x64 + ARM64

**Previous verdict**: ADD ARM64. **Reassessment**: **CONFIRMED.**

ARM64 Snapdragon X laptops are shipping in volume. Sophia Script already supports ARM64.
The Core library and CLI are architecture-neutral. Only the GUI needs ARM64 WinForms testing.

**Action items**:
- [ ] Add `win-arm64` to the publish matrix in `release.yml`
- [ ] Add ARM64 CI runner (GitHub Actions `windows-11-arm` when available, or self-hosted)
- [ ] Test WinForms rendering on ARM64 (DPI scaling, P/Invoke compatibility)

**Cross-platform (Linux/macOS)**: **Reject.** Registry tweaks are Windows-only — the entire
value proposition is Windows. Cross-platform GUI would add complexity with zero user benefit.

---

### Decision 10: Installer Strategy — Clean Up

**Previous verdict**: Remove MSIX, remove npm/maven/gem. **Reassessment**: **CONFIRMED.**

| Installer | Current | Proposed | Rationale |
|-----------|---------|----------|-----------|
| Portable EXE (GUI + CLI) | Yes | Yes (Keep) | Primary distribution; zero-install |
| MSI (WiX) | Yes | Yes (Keep) | Enterprise (GPO/SCCM/Intune) |
| MSIX | Yes | **Remove** | Low adoption, complex packaging |
| WinGet manifest | Yes | Yes (Keep) | Primary Windows 11 package manager |
| Scoop manifest | Yes | Yes (Keep) | Developer/power-user channel |
| Chocolatey | Yes | Yes (Keep) | Enterprise channel |
| PSGallery module | Yes | Yes (Keep) | PowerShell automation channel |
| npm/maven/gem | Yes | **Remove** | Zero evidence of usage; pure maintenance burden |
| One-liner web install | No | **Add** | Harvest from WinUtil/Sophia: `irm regilattice.dev/install \| iex` |

---

### Decision 11: Auto-Update Mechanism *(NEW)*

No competitor in this space has a robust auto-updater (WinUtil and Sophia are scripts that
re-download on each run). This is an **opportunity to leapfrog**.

| Option | Pros | Cons | Verdict |
|--------|------|------|---------|
| **Squirrel.Windows** | Mature, delta updates, GitHub Releases integration | Requires installation location (not portable) | **ADOPT for MSI installs** |
| **Manual GitHub API check** | Works for portable EXE, no dependencies | User must download manually | **ADOPT for portable** |
| **Velopack** (Squirrel successor) | Modern, cross-platform, delta updates | Newer, less proven | **Monitor** |
| **Microsoft Store** | Auto-updates built-in | Store submission process, MSIX-only | Reject |

**Implementation**: Two-tier approach:
1. **Portable EXE**: On startup, HTTPS GET to `api.github.com/repos/RajwanYair/RegiLattice/releases/latest`. If newer version exists, show notification bar. User clicks to open browser at release page.
2. **MSI/WinGet/Scoop/Choco**: Managed by the package manager's own update mechanism.

---

### Decision 12: Security & Trust *(NEW)*

No tool in this space is code-signed. **Being first to sign would be a major trust signal.**

| Area | Current | Proposed | Priority |
|------|---------|----------|----------|
| Code signing | None | **SignPath.io** (free for OSS) | P0 |
| SBOM | None | **CycloneDX** attached to every release | P1 |
| Reproducible builds | `<Deterministic>true` (already set) | Verify + document | P1 |
| Source Link | Not verified | `<EmbedAllSources>true</EmbedAllSources>` + SourceLink | P2 |
| Binary hash verification | `SHA256SUMS.txt` (already produced) | **GPG-sign** the checksum file | P2 |
| WDAC compatibility | Unknown | Test and document compatibility | P3 |
| Vulnerability scanning | `dotnet list package --vulnerable` in CI | Add CodeQL weekly scan (already in `weekly.yml`) | Done |

---

### Decision 13: Enterprise Features *(NEW)*

The enterprise market is underserved by all competitors. RegiLattice's profile system,
snapshot/restore, and policy tweaks are natural enterprise foundations.

| Feature | Status | Proposed | Target |
|---------|--------|----------|--------|
| CIS Benchmark alignment | Tweaks exist but not mapped | **Tag tweaks with CIS control IDs** | v7.x |
| STIG compliance mapping | Not started | **Tag tweaks with DISA STIG IDs** | v8.x |
| Compliance report export | Partial (CSV audit log) | **HTML/PDF compliance report** with pass/fail per CIS control | v7.x |
| GPO deployment package | MSI exists | **ADMX template** for enterprise GPO-based deployment | v8.x |
| Intune/SCCM integration | CorporateGuard detects only | **PowerShell DSC config** for managed deployment | v7.x |
| Fleet management API | None | **REST API** for multi-machine status queries (stretch) | v10+ |

**CIS Benchmark tagging**: Each tweak already has `Tags`. Adding `cis-l1-2.3.1` tags would
allow `--profile cis-l1` to apply all CIS Level 1 recommendations automatically. This is
a unique selling point no competitor offers.

---

### Decision 14: Community & Sustainability *(NEW)*

Optimizer's archival (18.2k stars → abandoned) is the **strongest cautionary tale** in this
space. A single-developer project with 7,718 tweaks and growing scope will hit the same wall
without deliberate sustainability measures.

| Strategy | Action | Priority |
|----------|--------|----------|
| **Reduce scope** | Extract non-core dialogs to `RegiLattice.Tools` plugin DLL | P0 |
| **Automate everything** | `Bump-Version.ps1` (28 files), `Sync-CopilotInstructions.ps1` (counts), auto-CHANGELOG | P0 |
| **Lower contributor barrier** | YAML tweak format for community contributions (no C# required) | P1 |
| **Curate Good First Issues** | 10+ issues with file pointers, expected outcomes, and test requirements | P1 |
| **Create `Setup-Dev.ps1`** | One-command dev environment bootstrap | P1 |
| **Publish `RegiLattice.SDK`** | NuGet package for third-party pack authors | P2 |
| **Enable GitHub Discussions** | Community Q&A, feature voting, tweak requests | P1 |
| **Establish a GOVERNANCE.md** | Clear ownership, contribution acceptance criteria, CoC enforcement | P2 |
| **Cap tweak growth** | Quality over quantity — audit and consolidate instead of endlessly adding | P0 |

**Cap tweak growth**: At 7,718 tweaks, the library is already 10x–100x larger than any
competitor. Further growth should focus on **quality** (correct registry values, proper
`ImpactScore`/`SafetyRating` calibration, source URL citations) rather than quantity.
Set a soft cap at 8,000 tweaks for v7.x; only add tweaks that fill documented gaps.

---

### Decision 15: Observability & Structured Logging *(NEW)*

The current logging approach (`session.Log`, `AppendLog()`, `Console.WriteLine`) is ad-hoc.
For a tool that modifies system registry, structured logging is a safety requirement.

| Area | Current | Proposed | Rationale |
|------|---------|----------|-----------|
| Log framework | Ad-hoc string lists | **Microsoft.Extensions.Logging** | Standard .NET, DI-friendly, zero config |
| Log output | `session.Log` (in-memory list) | **File + structured JSON** | Audit trail for registry modifications |
| Log viewer | GUI RichTextBox | **Keep** (but also write to `%LOCALAPPDATA%\RegiLattice\logs\`) | Persistent, machine-readable |
| Telemetry | None | **None** (explicitly opt-out) | Privacy-first tool must not phone home |
| Diagnostic mode | None | **`--diagnostic` CLI flag** | Verbose logging for support/debugging |

**Implementation**: Use `Microsoft.Extensions.Logging.Abstractions` (zero dependencies)
in Core. GUI/CLI register their own log providers (RichTextBox sink, console sink, file sink).
No telemetry, no analytics servers — all logs stay local.

---

## Part IV — Technical Debt Inventory

| ID | Debt | Severity | Location | Resolution Phase |
|----|------|----------|----------|-----------------|
| TD-1 | `TweakEngine` god class (3K+ LOC) | High | `TweakEngine.cs` | Phase B (DI) |
| TD-2 | Manual `RegisterBuiltins()` (195 lines) | Medium | `TweakEngine.cs` | Phase B (`[TweakModule]`) |
| TD-3 | 8 JSON files for persistence | Medium | Services/ | Phase B (repository abstraction) |
| TD-4 | 28-file manual version bump | High | Multiple | Phase A (`Bump-Version.ps1`) |
| TD-5 | `lessons-learned.instructions.md` (1K+ lines) | Medium | `.github/instructions/` | Phase A (split + archive) |
| TD-6 | `ExtendedCoverageTests.cs` monolith (58 classes) | Medium | Core.Tests/ | Phase A (split) |
| TD-7 | WinForms bitmap scaling (no vector icons) | High | GUI/ | Phase D (WPF) |
| TD-8 | No interface contracts on TweakEngine | High | Core/ | Phase B (DI) |
| TD-9 | 50K LOC of repetitive `TweakDef` data in C# | Low | Tweaks/ | Phase E (YAML optional) |
| TD-10 | `AppIcons.cs` programmatic bitmap generation | Low | GUI/ | Phase D (XAML vector icons) |
| TD-11 | No structured logging | Medium | All projects | Phase B (MEL) |
| TD-12 | npm/maven/gem registry files (dead weight) | Low | Root dirs | Phase A (remove) |
| TD-13 | 7 SVGs with manually-updated counts | Medium | `docs/assets/` | Phase A (CI-templated) |
| TD-14 | Hardcoded version in Mermaid diagrams | Low | `README.md` | Phase A (`Bump-Version.ps1`) |

---

## Part V — Improvement Roadmap

### Phase A: House Cleaning (v6.34–v6.39)

> **Risk**: Low · **Impact**: High · **Theme**: Reduce maintenance burden before building additions.

#### A.1 — Test Suite Consolidation (v6.34)

- [ ] Split `ExtendedCoverageTests.cs` monolith (58 test classes, ~4K LOC) into per-topic files
- [ ] Migrate standalone test classes to `BuiltinsFixture` (shared engine instance)
- [ ] Remove ~100–200 tests that duplicate `TweakEngineBuiltinsTests` assertions
- [ ] Add `[Collection]` isolation to all file-writing test classes
- [ ] Standardize performance budget tests with tweak-count comments

**Measurable outcome**: Test count may decrease; coverage stays ≥90%; test run time decreases ~20%.

#### A.2 — Documentation Reduction (v6.35)

- [ ] Consolidate instruction files: 8 → 4 (merge `no-duplication` → `csharp`, `cicd` → `git-workflow`)
- [ ] Archive `lessons-learned.instructions.md` to `docs/archive/`; new version < 200 lines
- [ ] Reduce skills: 12 → 8 (merge related skills)
- [ ] Deploy MkDocs Material docs site to GitHub Pages
- [ ] Shrink `README.md` to ~200 lines + link to docs site
- [ ] Add `Sync-CopilotInstructions.ps1` to auto-update counts

#### A.3 — CI/CD Cleanup (v6.36)

- [x] Merge `smoke.yml` → `release.yml`, `pages.yml` → `ci.yml`
- [x] Delete npm/maven/gem package registry files
- [ ] Create `Bump-Version.ps1` automating all 28 files
- [ ] Add `paths-ignore` for docs-only changes
- [ ] Add auto-CHANGELOG generation via `git-cliff`

#### A.4 — Scope Discipline (v6.37–v6.38)

- [ ] Audit 67+ dialog/form classes; split into Core (~30) and Tools (~35)
- [ ] Create `RegiLattice.Tools` project for non-core dialogs (battery, network, disk)
- [ ] Load `RegiLattice.Tools.dll` as optional plugin from MainForm
- [ ] Cap tweak growth: soft cap at 8,000; focus on quality over quantity

#### A.5 — xUnit v3 Migration (v6.39)

- [ ] Update all 5 held packages simultaneously
- [ ] Fix breaking API changes per project (Core → CLI → GUI)
- [ ] Add `Verify` snapshot testing for CLI output regression
- [ ] Verify all 3,304+ tests pass on xUnit v3

---

### Phase B: Architecture Modernisation (v7.0)

> **Risk**: Medium · **Impact**: High · **Theme**: Replace the God class with testable services.

#### B.1 — Dependency Injection Container

```csharp
var services = new ServiceCollection()
    .AddRegiLatticeCore()                                    // registers all interfaces
    .AddSingleton<IRegistrySession>(new RegistrySession())
    .BuildServiceProvider();

var search = services.GetRequiredService<ITweakSearch>();
var results = search.Search("privacy");
```

- [ ] Extract 6 interfaces from `TweakEngine` + 4 from `RegistrySession`
- [ ] Create `ServiceCollectionExtensions.AddRegiLatticeCore()`
- [ ] Keep `TweakEngine` as backward-compatible facade
- [ ] Update GUI and CLI to use DI container

#### B.2 — Auto-Registration via `[TweakModule]` Attribute

- [ ] Define `[TweakModule]` attribute
- [ ] Implement assembly scanning (or source generator) in `RegisterBuiltins()`
- [ ] Remove 195 manual registration lines
- [ ] Benchmark: scanning must complete in <50ms

#### B.3 — Repository Abstraction

- [ ] Define `IFavoritesRepository`, `IRatingsRepository`, `IHistoryRepository`, etc.
- [ ] Implement JSON-backed repositories (default, backward-compatible)
- [ ] Remove direct `File.ReadAllText`/`File.WriteAllText` from services
- [ ] All data access via DI-injected repositories

#### B.4 — Structured Logging

- [ ] Add `Microsoft.Extensions.Logging.Abstractions` to Core
- [ ] Replace ad-hoc `session.Log` with `ILogger<T>` injection
- [ ] Add file sink: `%LOCALAPPDATA%\RegiLattice\logs\regilattice-YYYY-MM-DD.log`
- [ ] Add `--diagnostic` CLI flag for verbose output

#### B.5 — Lightweight Event Bus

- [ ] Define event types as sealed records (`TweakAppliedEvent`, `TweakRemovedEvent`, etc.)
- [ ] Implement minimal `IEventBus` (publish/subscribe, no external dependency)
- [ ] Wire Analytics, TweakHistory, HealthScore as event subscribers
- [ ] Update GUI to subscribe to progress events instead of polling

---

### Phase C: Data Layer & Compliance (v7.1–v7.3)

> **Risk**: Medium · **Impact**: High · **Theme**: Enterprise readiness.

#### C.1 — SQLite Backend (Optional) (v7.1)

- [ ] Add `Microsoft.Data.Sqlite` package
- [ ] Implement SQLite-backed repositories (same interfaces as JSON)
- [ ] Add `config.json` setting: `"dataBackend": "json" | "sqlite"`
- [ ] Implement automatic JSON→SQLite migration when backend is switched
- [ ] Keep JSON files intact (no forced migration, no `.migrated` renames)

#### C.2 — CIS Benchmark Tagging (v7.2)

- [ ] Map relevant tweaks to CIS Windows 11 Benchmark v3.0 control IDs
- [ ] Add `cis-l1-X.Y.Z` and `cis-l2-X.Y.Z` tags to matching `TweakDef` entries
- [ ] Implement `--profile cis-l1` and `--profile cis-l2`
- [ ] Generate **CIS compliance report**: HTML with pass/fail per control

#### C.3 — Enterprise Compliance Reporting (v7.3)

- [ ] HTML/PDF compliance report with executive summary
- [ ] CSV/JSON export of compliance posture
- [ ] Integration with PowerShell DSC for managed deployment
- [ ] ADMX template generation for GPO-based enterprise deployment

---

### Phase D: Frontend Rewrite — WPF (v8.0)

> **Risk**: High · **Impact**: Very High · **Theme**: Modern, accessible, GPU-accelerated UI.

#### D.1 — WPF Shell + WinForms Interop (v8.0)

```
+--------------------------------------------------+
|  WPF MainWindow                                   |
|  +--------------+  +---------------------------+  |
|  |  WPF Sidebar  |  |  WindowsFormsHost         |  |
|  |  (categories, |  |  (existing TweakPanel)    |  |
|  |   profiles,   |  |  → migrated to WPF in    |  |
|  |   search)     |  |    v8.1–v8.5             |  |
|  +--------------+  +---------------------------+  |
|  +---------------------------------------------+  |
|  |  WPF Log Panel                               |  |
|  +---------------------------------------------+  |
+--------------------------------------------------+
```

- [ ] Create `RegiLattice.WPF` project
- [ ] WPF `MainWindow` with `WindowsFormsHost` for existing panels
- [ ] MVVM `TweakListViewModel` with `CommunityToolkit.Mvvm`
- [ ] XAML resource dictionaries for all 11 themes
- [ ] Data-virtualized `ListView` for 7,718 tweaks

#### D.2 — Incremental Panel Migration (v8.1–v8.5)

| Version | Panel | Key Challenge |
|---------|-------|--------------|
| v8.1 | TweakBrowserPanel → WPF DataGrid | Data virtualization for 7,718 rows |
| v8.2 | TweakCardRow → WPF UserControl | Scope badges, kind symbols |
| v8.3 | Package manager dialogs → WPF | Template method pattern in MVVM |
| v8.4 | Settings/About dialogs → WPF | Final WinForms removal |
| v8.5 | Remove `RegiLattice.GUI` project | Clean break; archive WinForms code |

#### D.3 — Accessibility (WCAG 2.1 AA)

- [ ] Full keyboard navigation (Tab order, arrow keys)
- [ ] Screen reader support via UI Automation (WPF native)
- [ ] High-contrast theme support (detect Windows high-contrast mode)
- [ ] 4.5:1 minimum contrast ratio (audit all 11 themes)
- [ ] Focus indicators on all interactive elements

#### D.4 — Auto-Updater

- [ ] Implement GitHub Releases API check on startup
- [ ] Non-modal notification bar: "v8.1.0 available — [Update] [Skip]"
- [ ] CLI: print one-line notice to stderr
- [ ] Respect `DisableAutoUpdate` registry policy for enterprise

#### D.5 — Code Signing

- [ ] Register with SignPath.io (free for OSS)
- [ ] Add signing step to `release.yml`
- [ ] Sign all EXE, DLL, and MSI artifacts
- [ ] Document certificate for WinGet and Chocolatey manifests

---

### Phase E: Data-Driven Tweaks — YAML (v9.0)

> **Risk**: Medium · **Impact**: Medium · **Theme**: Lower the contribution barrier.

#### E.1 — YAML Tweak Definitions (Registry-Only)

```yaml
# tweaks/privacy/disable-telemetry.yaml
id: priv-disable-telemetry
label: Disable Telemetry
category: Privacy
tags: [telemetry, privacy, cis-l1-18.9.16.1]
needs-admin: true
impact-score: 5
safety-rating: 4

apply:
  - set-dword:
      path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection"
      name: AllowTelemetry
      value: 0

remove:
  - delete-value:
      path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection"
      name: AllowTelemetry

detect:
  - check-dword:
      path: "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection"
      name: AllowTelemetry
      expected: 0
```

- [ ] Define YAML schema with JSON Schema validation
- [ ] Implement `YamlTweakLoader` → `TweakDef` conversion
- [ ] Load YAML tweaks alongside C# tweaks in `RegisterBuiltins()`
- [ ] CLI: `--validate-yaml <path>` for community tweak validation
- [ ] VS Code YAML schema setting for auto-validation

#### E.2 — Gradual C# → YAML Migration

- [ ] Auto-generate YAML from C# `TweakDef` objects: `--export-yaml <dir>`
- [ ] Round-trip validation: C# → YAML → load → diff
- [ ] Migrate 1 category per sprint as proof of concept
- [ ] Keep C# for ~5% of tweaks with delegate-based logic

---

### Phase F: Security, Trust & Ecosystem (v9.1+)

> **Risk**: Low · **Impact**: Medium · **Theme**: Earn user trust and grow community.

#### F.1 — SBOM & Reproducible Builds (v9.1)

- [ ] Generate CycloneDX SBOM in `release.yml`
- [ ] Attach `sbom.cdx.json` to every GitHub Release
- [ ] GPG-sign `SHA256SUMS.txt`
- [ ] Document build reproducibility verification steps

#### F.2 — Lazy Admin Elevation (v9.2)

- [ ] Apply HKCU tweaks immediately (no admin needed)
- [ ] Prompt UAC only when HKLM tweaks are selected
- [ ] CLI: `--no-elevate` to skip Machine tweaks silently
- [ ] Use per-operation `runas` verb instead of full-admin launch

#### F.3 — Internationalisation (v9.3)

- [ ] Evaluate Crowdin (free for OSS) for community translations
- [ ] Prioritize: en, zh-CN, es, de, ja (top 5 by Windows market share)
- [ ] Extract all UI strings to `.resx` resources
- [ ] Harvest from Optimizer's 24-language community PR model

#### F.4 — Community Experience (v9.4)

- [ ] Create `Setup-Dev.ps1` — one-command dev environment bootstrap
- [ ] Curate 10+ Good First Issues with detailed descriptions
- [ ] Publish `RegiLattice.SDK` NuGet package for pack authors
- [ ] Enable GitHub Discussions
- [ ] One-liner install: `irm regilattice.dev/install | iex`

#### F.5 — Watch Mode & Batch Scripting (v9.5+)

- [ ] CLI `--watch`: monitor registry for tweak drift, alert on reversion
- [ ] CLI `--batch-file <yaml>`: deployment recipes for enterprise

```yaml
# deploy-privacy.yaml
profile: privacy
additional: [priv-disable-telemetry, telem-disable-ceip]
skip: [priv-disable-cortana]  # needed for accessibility
mode: apply
dry-run: false
```

---

### Phase G: Next Generation (v10+) *(NEW)*

> **Risk**: High · **Impact**: Transformative · **Theme**: Platform evolution.

#### G.1 — WinUI 3 Migration (v10.0)

Once WPF migration is stable and proven (v8.5+), evaluate WinUI 3 for the v10 generation:
- Fluent Design 2 native controls
- Mica/Acrylic materials
- Windows App SDK integration
- Modern notification system (Windows Toast)

**Gate**: Only proceed if WinUI 3 ecosystem has matured (stable packaging without MSIX
requirement, community controls, proven at scale by SophiApp 2.0 and other projects).

#### G.2 — Spectre.Console TUI (v10.1)

```
$ regilattice --tui

+-- RegiLattice v10.0 ----------------------------------------+
| Search: telemetry_                                           |
|                                                              |
| > Privacy (142 tweaks)                                       |
|   [ ] priv-disable-telemetry    Disable Telemetry            |
|   [x] priv-disable-ad-id       Disable Advertising ID       |
|                                                              |
| [Space] Toggle  [Enter] Apply  [/] Search  [q] Quit         |
+--------------------------------------------------------------+
```

- [ ] Add `Spectre.Console` NuGet package
- [ ] `--tui` command for interactive terminal UI
- [ ] Category tree, search, tweak toggle, profile selector

#### G.3 — REST API for Fleet Management (v10.2+)

For enterprise customers managing hundreds of machines:
- Minimal ASP.NET Core API for multi-machine status queries
- Agent mode: headless daemon that listens for commands
- Dashboard: web UI for fleet-wide tweak compliance

**Gate**: Only proceed if enterprise demand is validated through user feedback.

---

## Part VI — Success Metrics & KPIs

| Metric | Current (v6.33) | Phase A | Phase D | Phase E | Phase G |
|--------|----------------|---------|---------|---------|---------|
| **Tweaks** | 7,718 | 7,718 (cap) | 7,718 | 8,000 | 8,000 |
| **GUI framework** | WinForms | WinForms | **WPF** | WPF | WinUI 3? |
| **Architecture** | Monolithic | Monolithic | **DI + interfaces** | DI | DI |
| **Data persistence** | 8 JSON files | 8 JSON files | **Repo abstraction + optional SQLite** | SQLite default | SQLite |
| **Tweak format** | 195 C# files | 195 C# files | 195 C# files | **Hybrid C#/YAML** | YAML primary |
| **Tests** | 3,304 (xUnit v2) | 3,200+ (xUnit v3) | 3,500+ | 4,000+ | 4,000+ |
| **CI workflows** | 6 | **4** | 4 | 4 | 4 |
| **Version bump files** | 28 manual | **1 script** | 1 script | 1 script | 1 script |
| **Package registries** | 7 | **4** | 4 | 4 | 4 |
| **Code signing** | None | None | **SignPath.io** | SignPath.io | SignPath.io |
| **ARM64** | No | CI testing | **Published** | Published | Published |
| **Locales** | 2 (en, de) | 2 | 5 | **5+ (Crowdin)** | 10+ |
| **a11y compliance** | None | None | **WCAG 2.1 AA** | WCAG 2.1 AA | WCAG 2.1 AA |
| **Auto-updater** | No | No | **Yes** | Yes | Yes |
| **One-liner install** | No | No | **Yes** | Yes | Yes |
| **Binary size** | ~40MB | ~38MB | ~35MB | ~30MB | ~15MB (AOT) |
| **Startup time** | ~200ms | ~180ms | ~100ms | ~50ms | ~30ms |
| **Community contributors** | 1 | 1 | **5+** | **10+** | 20+ |
| **CIS benchmark coverage** | 0% | 0% | **30%+** | 60%+ | 80%+ |
| **Docs site** | Static HTML | **MkDocs Material** | MkDocs | MkDocs | MkDocs |
| **Structured logging** | No | No | **Yes** | Yes | Yes |

---

## Part VII — Risk Register

| ID | Risk | Impact | Probability | Mitigation |
|----|------|--------|------------|------------|
| R1 | **WPF migration takes longer than expected** | Months of dual-framework maintenance | High | Incremental via `WindowsFormsHost`; WinForms stays functional; each panel is a separate PR |
| R2 | **Data migration corrupts user data** | Users lose favorites, history, config | Medium | Repository abstraction (no forced migration); JSON stays default; explicit opt-in to SQLite |
| R3 | **YAML tweak format has edge cases** | Broken tweaks, wrong registry values | Medium | C# stays authoritative; YAML is additive; comprehensive round-trip tests |
| R4 | **xUnit v3 breaks 3,304 tests** | CI blocked for days | Medium | Branch migration; one project at a time; hold FsCheck.Xunit v3 until verified |
| R5 | **Scope reduction upsets users** | Feature regression perception | Medium | Extract to downloadable plugin DLL; announce one version before removal |
| R6 | **Maintainer burnout** (Optimizer lesson) | Project abandoned | **High** | Scope cap, contributor pipeline, automate everything, reduce instruction files |
| R7 | **Code signing certificate compromise** | Trust loss, broken pipeline | Low | SignPath HSM-backed keys, auto-renewal, documented revocation procedure |
| R8 | **ARM64 WinForms breaks at runtime** | Broken ARM64 builds | Low | ARM64 CI runner; test before publishing |
| R9 | **npm/maven/gem removal breaks unknown downstream** | Users lose install method | Low | Announce deprecation in CHANGELOG; add redirect note |
| R10 | **DI container adds startup overhead** | Slower cold start | Low | Benchmark before/after; typical DI is ~10ms; lazy loading offsets |
| R11 | **Auto-updater is blocked by corporate firewalls** | Update check fails silently | Medium | `--no-update-check` flag; respect `DisableAutoUpdate` GPO; graceful timeout (3s) |
| R12 | **MkDocs Material docs site maintenance** | Stale docs, broken links | Low | CI build + deploy on every push; link checker in weekly workflow |
| R13 | **CIS benchmark mapping is incorrect** | False compliance claims | Medium | Manual verification against official CIS PDF; disclaimer in reports |

---

## Part VIII — Migration Sequence

```
v6.34  - Phase A.1  Test Suite Consolidation
v6.35  - Phase A.2  Documentation Reduction + Docs Site
v6.36  - Phase A.3  CI/CD Cleanup + Bump-Version.ps1
v6.37  - Phase A.4a Scope Discipline (audit + plan)
v6.38  - Phase A.4b Scope Discipline (extract RegiLattice.Tools)
v6.39  - Phase A.5  xUnit v3 Migration
         |
v7.0   - Phase B.1  DI Container + Interface Segregation     ← MAJOR
v7.1   - Phase B.2  Auto-Registration + B.3 Repository Abstraction
v7.2   - Phase B.4  Structured Logging + B.5 Event Bus
v7.3   - Phase C.1  SQLite Backend (optional)
v7.4   - Phase C.2  CIS Benchmark Tagging
v7.5   - Phase C.3  Enterprise Compliance Reporting
         |
v8.0   - Phase D.1  WPF Shell + WinForms Interop             ← MAJOR
v8.1   - Phase D.2a TweakBrowserPanel → WPF
v8.2   - Phase D.2b TweakCardRow → WPF
v8.3   - Phase D.2c Package Manager Dialogs → WPF
v8.4   - Phase D.2d Settings/About → WPF + D.3 Accessibility
v8.5   - Phase D.2e Remove WinForms + D.4 Auto-Updater + D.5 Code Signing
         |
v9.0   - Phase E.1  YAML Tweak Definitions                   ← MAJOR
v9.1   - Phase F.1  SBOM + Reproducible Builds
v9.2   - Phase F.2  Lazy Admin Elevation
v9.3   - Phase F.3  Internationalisation (Crowdin)
v9.4   - Phase F.4  Community Experience + One-Liner Install
v9.5   - Phase F.5  Watch Mode + Batch Scripting
         |
v10.0  - Phase G.1  WinUI 3 Evaluation/Migration             ← MAJOR (gated)
v10.1  - Phase G.2  Spectre.Console TUI
v10.2+ - Phase G.3  REST API for Fleet Management (gated)
```

**Key principles**:

1. **Each phase is independently valuable.** No phase requires all prior phases.
2. **MAJOR bumps are reserved for breaking changes**: DI API (v7.0), WPF (v8.0), YAML (v9.0), WinUI 3 (v10.0).
3. **Phase A is entirely backward-compatible** — pure cleanup, no breaking changes.
4. **WinForms remains functional throughout Phase D** (interop layer).
5. **C# tweak modules remain authoritative** even after YAML ships (Phase E).
6. **Highest-ROI phases first**: Test cleanup → CI cleanup → scope cap → DI → WPF.
7. **No data migration is forced.** JSON stays default; SQLite is opt-in.
8. **Tweak growth is capped.** Quality and calibration over quantity.

---

## Part IX — Appendix: Completed Work (v6.0–v6.33)

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

- **v6.31**: 5 new policy modules (Feedback, SettingSync, RAHardening, SecCenter, DeliveryOpt); fixed 3 duplicate tweak IDs
- **v6.32**: 5 new policy modules (BITS, Personalization, TabletPC, WindowsBackup, GameDVR); fix Backup() DryRun short-circuit
- **v6.33**: 5 new policy modules (DefenderATP, WindowsInstaller, Cryptography, FVE, WindowsUpdateAU)

**Totals at v6.34.0**: 7,718 tweaks · 158 categories · 195 modules · 3,304 tests

</details>

---

*End of Roadmap v2. This document should be reviewed quarterly and updated after each
MAJOR version release. Use `git log --oneline docs/Roadmap.md` to track revision history.*
