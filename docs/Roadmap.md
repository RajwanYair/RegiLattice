# RegiLattice — Project Roadmap

> Strategic improvement plan — rethinking every layer of the project for best-in-class quality.
> Consolidates completed phases 1–7, then proposes a new forward-looking architecture.
> Baseline: **v6.33.0** — 7,718 tweaks · 158 categories · 195 files · 3,296 tests · 11 themes

---

## Table of Contents

- [Completed Work (v6.0–v6.33)](#completed-work-v60v633)
- [Strategic Assessment](#strategic-assessment)
- [Phase 0 — Cross-Project Tooling Infrastructure ⭐ HIGH PRIORITY](#phase-0--cross-project-tooling-infrastructure)
- [Phase 0.B — GitHub AI Surface & Workflow Modernisation ⭐ HIGH PRIORITY](#phase-0b--github-ai-surface--workflow-modernisation)
- [Phase 0.C — Standards Alignment: VSCode, GitHub & SVG ⭐ HIGH PRIORITY](#phase-0c--standards-alignment-vscode-github--svg)
- [Phase 8 — Architecture Modernisation](#phase-8--architecture-modernisation)
- [Phase 9 — Data Layer & Persistence](#phase-9--data-layer--persistence)
- [Phase 10 — Frontend Revolution](#phase-10--frontend-revolution)
- [Phase 11 — Scope Discipline & Feature Focus](#phase-11--scope-discipline--feature-focus)
- [Phase 12 — Build, CI/CD & Distribution](#phase-12--build-cicd--distribution)
- [Phase 13 — Quality, Testing & Observability](#phase-13--quality-testing--observability)
- [Phase 14 — Documentation & Developer Experience](#phase-14--documentation--developer-experience)
  - [14.0 `.github` Copilot Surface Modernisation](#140-github-copilot-surface-modernisation)
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

## Phase 0 — Cross-Project Tooling Infrastructure

> **Priority**: ⭐ P0 — Execute before all other phases.
> **Goal**: Centralise all shared tooling configuration under the `MyScripts` root so that every project under that directory inherits common tooling automatically. Only project-specific overrides live inside the workspace.

### 0.1 Principle: Two-Tier Configuration

**Problem today**: Every project under `C:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts\` independently manages its own copies of tool configurations (VS Code settings, terminal profiles, bootstrap scripts, editor rules, linting configs, MCP server definitions, etc.). Changes to a shared tool require updating every project separately.

**Target state**: A clean two-tier hierarchy:

```
MyScripts\                              ← COMMON TIER (shared by all projects)
├── .vscode\
│   ├── settings.json                   # Editor defaults, extension settings shared across all projects
│   ├── extensions.json                 # Recommended extensions for all MyScripts projects
│   ├── mcp.json                        # MCP server definitions shared by all projects
│   └── scripts\
│       └── Initialize-CommonTooling.ps1 # Shared bootstrap: PATH, aliases, env vars
├── .editorconfig                       # Shared editor/formatter rules (indentation, charset, etc.)
├── .gitconfig-includes                 # Shared git config fragments (identity, aliases, hooks)
├── .env-common.ps1                     # Common environment variables and helper functions
├── tools\                              # Shared tool definitions
│   ├── nuget.config                    # Shared NuGet sources for all .NET projects
│   ├── Directory.Build.props           # MSBuild defaults inherited by all .NET projects
│   └── psscriptanalyzer.psd1          # Shared PSScriptAnalyzer ruleset
└── completions\                        # Shared shell completions for shared tools

MyScripts\RegiLattice\                  ← WORKSPACE TIER (project-specific overrides only)
├── .vscode\
│   ├── settings.json                   # ONLY RegiLattice-specific overrides (e.g., test task definitions)
│   ├── mcp.json                        # ONLY RegiLattice-specific MCP servers (filesystem scoped to this workspace)
│   └── launch.json                     # RegiLattice-specific debug launch configs
├── .editorconfig                       # ONLY RegiLattice overrides (e.g., 4-space indent for C#)
├── .env.ps1                            # Sources ..\Initialize-CommonTooling.ps1, then adds RegiLattice-specific helpers
└── Directory.Build.props               # RegiLattice-specific MSBuild props (version, TreatWarningsAsErrors, etc.)
```

**Rule**: If a setting is project-agnostic (editor font, extension recommendation, NuGet source, git alias), it lives in the `MyScripts\` tier. If it is specific to RegiLattice (VS Code test tasks, RegiLattice MCP filesystem scope, version properties), it lives in the workspace tier.

### 0.2 Audit: What Belongs Where

**Inventory of current RegiLattice workspace configs and their correct tier**:

| File / Setting | Current Location | Correct Tier | Action |
|---|---|---|---|
| VS Code font, theme, tab size for non-C# files | `.vscode/settings.json` | MyScripts common | Move up |
| VS Code recommended extensions (Copilot, GitLens, CSharpier, markdownlint) | `.vscode/extensions.json` | MyScripts common | Move up |
| MCP server: `github` (GitHub Copilot remote MCP) | `.vscode/mcp.json` | MyScripts common | Move up |
| MCP server: `memory` (project facts KB) | `.vscode/mcp.json` | MyScripts common | Move up |
| MCP server: `sequential-thinking` | `.vscode/mcp.json` | MyScripts common | Move up |
| MCP server: `filesystem` (scoped to workspace root) | `.vscode/mcp.json` | Workspace only | Keep |
| MCP server: `project-docs` (scoped to `.github/` + `docs/`) | `.vscode/mcp.json` | Workspace only | Keep |
| `Initialize-CommonTooling.ps1` | `..\MyScripts\.vscode\scripts\` | MyScripts common | Already correct ✅ |
| `MSBUILDDISABLENODEREUSE=1`, `FORCE_JAVASCRIPT_ACTIONS_TO_NODE24` | `.env.ps1` (workspace) | Should be in common bootstrap | Move up |
| `$env:DOTNET_CLI_TELEMETRY_OPTOUT` | `.env.ps1` | MyScripts common | Move up |
| RegiLattice-specific helper functions (`Invoke-RLBuild`, `Invoke-RLTest`, etc.) | `.env.ps1` | Workspace only | Keep |
| `.editorconfig` base rules (UTF-8, LF line endings, trim trailing whitespace) | `.editorconfig` | MyScripts common | Move up |
| `.editorconfig` C#-specific rules (4-space indent, `csharp_*`) | `.editorconfig` | Workspace only | Keep |
| `Directory.Build.props` shared defaults (nullable, implicit usings, LangVersion) | `Directory.Build.props` | Could be MyScripts common | Evaluate |
| `Directory.Build.props` RegiLattice-specific (`<Version>`, `<TreatWarningsAsErrors>`) | `Directory.Build.props` | Workspace only | Keep |
| NuGet sources | `nuget.config` (if present) | MyScripts common | Evaluate |
| PSScriptAnalyzer settings | `.vscode/settings.json` or inline | MyScripts common | Move up |
| `.github/copilot-instructions.md` | `.github/` | Workspace only | Keep |
| `.github/instructions/*.instructions.md` | `.github/` | Workspace only | Keep |

### 0.3 Implementation Plan

**Step 1 — Audit and document current state** (P0, immediate)

Run a full audit across all projects under `MyScripts\` to identify:
1. Which tool configs are duplicated across projects (e.g., each project has the same MCP `github` entry)
2. Which VS Code settings are project-agnostic vs. project-specific
3. Which bootstrap environment variables are universal vs. project-scoped

```powershell
# List all .vscode/settings.json files under MyScripts to compare
Get-ChildItem -Path 'C:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts' -Recurse -Filter 'settings.json' |
    Where-Object { $_.DirectoryName -match '\.vscode' } |
    Select-Object FullName

# Diff MCP configs across projects
Get-ChildItem -Path 'C:\Users\ryair\OneDrive - Intel Corporation\Documents\MyScripts' -Recurse -Filter 'mcp.json' |
    ForEach-Object { Write-Host "--- $($_.FullName) ---"; Get-Content $_ }
```

**Step 2 — Create MyScripts common tier** (P0)

Create or update `MyScripts\.vscode\settings.json` with settings that should apply to all projects:

```jsonc
// MyScripts\.vscode\settings.json — common defaults for ALL projects
{
    // Editor
    "editor.fontSize": 14,
    "editor.tabSize": 4,
    "editor.insertSpaces": true,
    "editor.trimAutoWhitespace": true,
    "files.encoding": "utf8",
    "files.eol": "\n",

    // Extensions shared by all projects
    "github.copilot.enable": { "*": true },
    "markdownlint.config": { "MD013": false, "MD033": false },

    // PSScriptAnalyzer settings for all PowerShell projects
    "powershell.scriptAnalysis.settingsPath": "tools/psscriptanalyzer.psd1"
}
```

Create `MyScripts\.vscode\mcp.json` with the three shared MCP servers:

```jsonc
// MyScripts\.vscode\mcp.json — shared MCP servers for ALL projects
{
    "servers": {
        "github": {
            "type": "stdio",
            "command": "...",   // GitHub Copilot remote MCP
            "env": { ... }
        },
        "memory": {
            "type": "stdio",
            "command": "npx",
            "args": ["-y", "@modelcontextprotocol/server-memory"]
        },
        "sequential-thinking": {
            "type": "stdio",
            "command": "npx",
            "args": ["-y", "@modelcontextprotocol/server-sequential-thinking"]
        }
    }
}
```

**Step 3 — Update workspace-tier to reference common tier** (P1)

Update `RegiLattice\.vscode\mcp.json` to include only workspace-scoped servers. VS Code multi-root workspace support means the common-tier MCP config is automatically inherited when VS Code opens `MyScripts\` as a workspace root.

Update `RegiLattice\.env.ps1` to source only workspace-specific helpers, relying on `Initialize-CommonTooling.ps1` (already in common tier) for shared env vars:

```powershell
# .env.ps1 — workspace-specific only; common tooling already loaded by Initialize-CommonTooling.ps1

# Guard: skip if already loaded by the MyScripts common bootstrap
if (-not $env:MYSCRIPTS_COMMON_LOADED) {
    . (Join-Path $PSScriptRoot '..' '.vscode' 'scripts' 'Initialize-CommonTooling.ps1')
}

# RegiLattice-specific helpers only
function Invoke-RLBuild { ... }
function Invoke-RLTest  { ... }
function Invoke-RLGui   { ... }
function Invoke-RLCli   { ... }

Write-Host '[.env.ps1] RegiLattice dev environment loaded'
```

**Step 4 — Create common `.editorconfig`** (P1)

Create `MyScripts\.editorconfig` with universal rules. Each workspace `.editorconfig` starts with `root = false` so it inherits from the parent directory:

```ini
# MyScripts\.editorconfig — inherited by all projects
root = true

[*]
charset = utf-8
end_of_line = crlf
indent_style = space
indent_size = 4
trim_trailing_whitespace = true
insert_final_newline = true

[*.{json,yml,yaml}]
indent_size = 2

[*.md]
trim_trailing_whitespace = false
```

Update `RegiLattice\.editorconfig` to use `root = false` (inherits) and contain only C#-specific overrides.

**Step 5 — Create common MSBuild defaults** (P2)

Create `MyScripts\tools\Directory.Build.Common.props` with defaults that every .NET project should inherit:

```xml
<!-- MyScripts\tools\Directory.Build.Common.props -->
<Project>
  <PropertyGroup>
    <!-- Shared .NET defaults across all MyScripts .NET projects -->
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UseSharedCompilation>false</UseSharedCompilation>
    <MSBUILDDISABLENODEREUSE Condition="'$(MSBUILDDISABLENODEREUSE)' == ''">1</MSBUILDDISABLENODEREUSE>
  </PropertyGroup>
</Project>
```

Each project's `Directory.Build.props` imports this file first, then adds its own settings:

```xml
<!-- RegiLattice\Directory.Build.props -->
<Project>
  <!-- Inherit common defaults -->
  <Import Project="..\tools\Directory.Build.Common.props" />

  <!-- RegiLattice-specific -->
  <PropertyGroup>
    <Version>6.28.0</Version>
    <TreatWarningsAsErrors Condition="'$(STRYKER_BUILD)' != '1'">true</TreatWarningsAsErrors>
    ...
  </PropertyGroup>
</Project>
```

**Step 6 — Shared NuGet config** (P2)

Create `MyScripts\nuget.config` that defines the NuGet sources shared across all .NET projects:

```xml
<!-- MyScripts\nuget.config -->
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="GitHub Packages" value="https://nuget.pkg.github.com/RajwanYair/index.json" />
  </packageSources>
</configuration>
```

**Step 7 — Document the pattern in `Initialize-CommonTooling.ps1`** (P1)

Add a header comment and `$env:MYSCRIPTS_COMMON_LOADED = '1'` guard so workspace `.env.ps1` files can detect whether the common bootstrap has already run:

```powershell
# Initialize-CommonTooling.ps1 — sources once; workspace .env.ps1 files check
# $env:MYSCRIPTS_COMMON_LOADED before re-sourcing
if ($env:MYSCRIPTS_COMMON_LOADED -eq '1') { return }

$env:MSBUILDDISABLENODEREUSE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:FORCE_JAVASCRIPT_ACTIONS_TO_NODE24 = 'true'
...

$env:MYSCRIPTS_COMMON_LOADED = '1'
```

### 0.4 Acceptance Criteria

Phase 0 is complete when **all** of the following are true:

| Criterion | Verification |
|-----------|-------------|
| Adding a new project under `MyScripts\` requires **zero** manual copying of tool config | Create a test project; verify it inherits editor settings, MCP servers, and bootstrap env vars automatically |
| `github`, `memory`, and `sequential-thinking` MCP servers are defined **only once** (in common tier) | `Select-String -Recurse -Pattern 'sequential-thinking'` shows only `MyScripts\.vscode\mcp.json` |
| `Initialize-CommonTooling.ps1` sets all cross-project env vars; each workspace `.env.ps1` sets only project-specific helpers | Code review of both files |
| `MyScripts\.editorconfig` has `root = true`; all workspace `.editorconfig` files have `root = false` | `Select-String -Recurse -Pattern 'root ='` shows correct values |
| `MSBUILDDISABLENODEREUSE` is set in the common bootstrap, NOT in individual `.env.ps1` files | `Select-String -Recurse -Path 'MyScripts' -Pattern 'MSBUILDDISABLENODEREUSE'` shows only common bootstrap + CI workflows |
| RegiLattice builds and all tests pass without any change to build commands | `dotnet build RegiLattice.sln -c Release` + full test suite |
| New or existing workspace `.env.ps1` is idempotent (safe to source multiple times) | Source `.env.ps1` twice in one terminal session; no duplicate output or errors |

### 0.5 Non-Goals

- This phase does **not** move or merge git repositories — each project under `MyScripts\` remains an independent git repo
- This phase does **not** create a monorepo — project code stays in its own workspace directory
- This phase does **not** affect CI/CD workflows — those always run in the workspace context where all needed files are present
- This phase does **not** change any `.github/` Copilot instruction files — those are workspace-scoped by design

---

## Phase 0.B — GitHub AI Surface & Workflow Modernisation

> **Priority**: ⭐ P0 — Execute alongside or immediately after Phase 0.
> **Goal**: Bring every `.github/` file to full capability — accurate instruction files, full-fidelity skill pages, an up-to-date agent definition, latest-version MCP servers, modern CI/CD action pins, and a comprehensive prompt library.

### 0.B.1 Current State Audit

The `.github/` directory contains six categories of AI-tooling files that drift independently of the source code:

| Category | Files | Key Staleness Risks |
|----------|-------|---------------------|
| **copilot-instructions.md** | 1 | Tweak/test/module counts go stale after every sprint |
| **Instruction files** | 8 (`*.instructions.md`) | SDK version refs, deprecated APIs, wrong method names |
| **Skill pages** | 12 (`skills/*/SKILL.md`) | Module counts, file paths, PowerShell command depth |
| **Agent definition** | 1 (`agents/regilattice.agent.md`) | Skill list, MCP server list, prompt list |
| **MCP server config** | 1 (`.vscode/mcp.json`) | Server npm package versions, missing new capabilities |
| **CI/CD workflows** | 6 (`workflows/*.yml`) | Action version pins, redundant steps, missing capabilities |
| **Prompt library** | `prompts/*.prompt.md` | Incomplete — many common tasks have no prompt |

Audit command to find stale version numbers across all `.github/` markdown files:

```powershell
# Find all hardcoded counts — cross-reference against dotnet run -- --stats
Select-String -Path .github/**/*.md, .github/**/*.yml -Pattern '\d[, ]\d{3}|\d{4}\+' -Recurse
```

---

### 0.B.2 Instructions Enhancement

**Goal**: Every instruction file is accurate, complete, and references current APIs, SDK versions, and coding patterns.

**Step 1 — Sync counts in `copilot-instructions.md`** (P0)

Every sprint changes tweaks/tests/modules. After every version bump, update the Quick Facts table and every heading that embeds counts:

```powershell
# Get live counts from CLI
dotnet run --project src/RegiLattice.CLI -- --stats --no-color
# Get test count
(Get-ChildItem tests/**/*.cs -Recurse | Select-String -Pattern '\[Fact\]|\[Theory\]').Count
# Get module count
(Get-ChildItem src/RegiLattice.Core/Tweaks/*.cs).Count
```

**Step 2 — Verify `csharp.instructions.md` covers C# 13 idioms** (P1)

Check that the following C# 13 / .NET 10 patterns are documented:
- Collection expressions `[]` for all `IReadOnlyList<T>` and `IReadOnlyDictionary<,>` defaults
- `params ReadOnlySpan<T>` (C# 13 params collections) — document when to use vs `params T[]`
- `System.Threading.Lock` vs `lock (object)` — which pattern is used in this codebase
- `allows ref struct` constraint — document if/where used
- `partial` classes/properties — document the file-split naming convention (`ClassName.Part2.cs`)

**Step 3 — Verify `lessons-learned.instructions.md` is complete** (P1)

For every lesson, confirm the PowerShell example still works against the current file layout:

```powershell
# Verify brace-counting lesson example still resolves to actual file
Select-String -Pattern '^internal static (partial )?class \w+' `
    'src/RegiLattice.Core/Tweaks/Browser.cs' | Measure-Object | Select-Object -ExpandProperty Count
```

Verify the **Policy Module 5×10 Cadence** table is current (last entry must match the latest version in `CHANGELOG.md`).

**Step 4 — Audit `testing.instructions.md` test project counts** (P1)

The per-project test counts in the table (`Core.Tests 2,499+`, `CLI.Tests 434+`, `GUI.Tests 363+`) must be updated to reflect the current test suite after every major sprint.

**Step 5 — Verify `git-workflow.instructions.md` 28-file checklist** (P1)

Verify every file in the Group A–F version bump checklist still exists and is in the correct group:

```powershell
# Quick existence check for the most-changed files
@('Directory.Build.props','installer/Package.wxs','docs/CHANGELOG.md',
  'docs/assets/stats.svg','docs/assets/banner.svg','README.md',
  'npm/package.json','powershell/RegiLattice.psd1') | ForEach-Object {
    [pscustomobject]@{ File=$_; Exists=(Test-Path $_) }
}
```

---

### 0.B.3 Skills Modernisation

**Goal**: Every internal skill page is accurate and actionable; missing skills are created.

**Per-skill verification grid**:

| Skill | Key Facts to Re-check | Priority |
|-------|-----------------------|----------|
| `add-tweaks` | Module count (195), `ImpactScore`/`SafetyRating` mandatory, `RegisterBuiltins()` registration | P0 |
| `architecture` | `TweakEngine` API additions (v6.28: `Migrations`, `ResolveMigration`; v6.27: `HealthScoreService`, `ConflictDetector`) | P0 |
| `release` | 28-file checklist groups (A–F), SVG space-separated thousands, artifact naming pattern | P0 |
| `testing` | Test counts (3,296 total), `[CallerFilePath]` pattern, `ScheduleAction` enum (v6.28), flaky test rules | P0 |
| `no-duplication` | 195 module files, 158 categories, 7,718 tweaks, `Audit-Duplications.ps1` path | P1 |
| `debug-fix` | OneDrive build patterns, GDI leak `using` rule, `ShellRunner` kill-on-timeout | P1 |
| `gui-themes` | 11 themes (Cyberpunk added), `FileSystemWatcher` hot-reload, JSON theme files | P1 |
| `search-tweaks` | 195 files, 7,718 tweaks, 158 categories | P1 |
| `package-managers` | `BasePackageManagerDialog` 5 dialogs, `PackageNameValidator` | P2 |
| `tool-versions` | `ToolVersionChecker` current tool list | P2 |

**For every skill, also verify**:
1. PowerShell commands use correct Tweaks directory depth and file glob (`*.cs` covers 195 files)
2. MCP tool names match the non-deprecated names from user memory (e.g., `mcp_filesystem_read_text_file`, NOT `read_file`)
3. Code examples compile — no removed `TweakEngine` methods, no stale service names
4. File paths in examples exist (e.g., `src/RegiLattice.Core/Tweaks/Privacy.cs` vs the extracted split files)

**New skills to create**:

| Skill | Purpose | Triggers |
|-------|---------|----------|
| `sprint-execution` | End-to-end sprint: plan → code → build → test → commit → tag → push | "sprint", "execute sprint", "new sprint" |
| `version-bump` | Automated 28-file version bump with PowerShell helpers and verification | "bump version", "release", "version bump" |

---

### 0.B.4 Agent Definition Update

**File**: `.github/agents/regilattice.agent.md`

**Step 1 — MCP server inventory** (P0)

Verify every MCP server in the agent definition matches `.vscode/mcp.json`:

```powershell
# List servers actually configured
(Get-Content .vscode/mcp.json | ConvertFrom-Json).servers.PSObject.Properties.Name
```

Each server entry must include: server name, capability surface (what tools it exposes), and intended use within RegiLattice development.

**Step 2 — Skill cross-reference** (P0)

Every skill listed in the agent definition must have a corresponding `SKILL.md`:

```powershell
Get-ChildItem .github/skills/ -Directory | Select-Object -ExpandProperty Name
```

Add `sprint-execution` and `version-bump` to the agent skill list once their SKILL.md files are created.

**Step 3 — Prompt cross-reference** (P1)

Add an explicit prompt inventory table to the agent definition listing every `.github/prompts/*.prompt.md` file and its trigger words.

**Step 4 — Model routing guidance** (P2)

Document which model capability tier is recommended for each task class:

| Task Class | Recommended Capability | Examples |
|------------|----------------------|----------|
| Quick lookups (search, status, single-file read) | Fast model | "what is the tweak ID for...", "find the file" |
| Multi-file edits (sprint tasks, bug fixes) | Standard reasoning model | Sprint execution, test writing |
| Full session planning (roadmap, architecture) | Highest-context model | Release planning, architecture review |

---

### 0.B.5 MCP Server Modernisation

**Goal**: All MCP servers run at verified latest stable versions with full capability sets enabled.

**Step 1 — Version audit** (P0)

Check current pinned versions and compare to npm registry:

```powershell
# Check server package versions in mcp.json
Get-Content .vscode/mcp.json

# Check latest on npm for each server
npm view @modelcontextprotocol/server-filesystem version
npm view @modelcontextprotocol/server-memory version
npm view @modelcontextprotocol/server-sequential-thinking version
```

**Step 2 — New high-value servers to evaluate** (P1)

| Server / Tool | Capability | Value for RegiLattice | Stability |
|---------------|-----------|----------------------|-----------|
| `@modelcontextprotocol/server-fetch` | HTTP fetch + HTML-to-text | Look up Windows registry docs, MSDN references, NuGet package info without leaving Copilot chat | Stable |
| `mcp-server-git` | Native git porcelain ops | Precise branch/diff/stash operations; complement to GitKraken MCP | Stable |
| `@modelcontextprotocol/server-brave-search` | Web search | Research undocumented registry keys, Windows API surface | Requires API key |
| Windows Registry MCP (community) | Direct HKCU/HKLM read | Preview actual registry state for tweak detection testing | Experimental |
| `@modelcontextprotocol/server-postgres` | SQL queries | Future: if SQLite/LiteDB is adopted in Phase 9 data layer | Future need |

**Step 3 — GitHub MCP full capability audit** (P0)

The GitHub MCP server exposes ~50+ tools. Verify these are referenced in skills and workflows:

| Tool | Used in | Gap if missing |
|------|---------|----------------|
| `mcp_github_search_code` | `no-duplication` skill | Can find registry path collisions across the repo |
| `mcp_github_list_releases` / `get_latest_release` | `release` skill | Verify tag exists after push |
| `mcp_github_run_secret_scanning` | Security workflow | Should be in `weekly.yml` |
| `mcp_github_request_copilot_review` | PR workflow | Auto-request Copilot review on every PR |
| `mcp_github_list_issue_types` | Release skill | Confirm release issue template types |
| `mcp_github_create_branch` | Release skill | Create `release/vX.Y.Z` branch |

**Step 4 — Memory MCP maintenance** (P1)

The `.github/mcp-memory.jsonl` knowledge graph accumulates facts over time. Periodically:

```powershell
# Count entities in the graph
(Get-Content .github/mcp-memory.jsonl | ConvertFrom-Json | Where-Object { $_.type -eq 'entity' }).Count

# Find stale version references
Select-String -Path .github/mcp-memory.jsonl -Pattern 'v[0-9]+\.[0-9]+\.[0-9]+'
```

Prune entities referencing removed features or superseded architecture decisions. Ensure all current architectural decisions (DI plan from Phase 8, data layer from Phase 9) are captured.

---

### 0.B.6 Workflow Modernisation

**Goal**: All 6 CI/CD workflows use verified latest action versions, eliminate redundant steps, and cover missing capabilities.

**Step 1 — Action version audit** (P0 — run before every release)

```powershell
# List all pinned action versions across all workflows
Select-String -Path .github/workflows/*.yml -Pattern 'uses:' |
    ForEach-Object { ($_ -split 'uses: ')[1].Trim() } |
    Sort-Object -Unique

# Verify a specific action's latest stable:
gh release list --repo actions/upload-artifact --limit 3
```

Expected verified versions as of 2026-04-19:

| Action | Expected Version | Notes |
|--------|-----------------|-------|
| `actions/checkout` | `@v6` | |
| `actions/setup-dotnet` | `@v5` | |
| `actions/cache` | `@v5` | |
| `actions/upload-artifact` | `@v4` | v7 does not exist — do not bump past v4 |
| `actions/download-artifact` | `@v4` | Matches upload-artifact major |
| `codecov/codecov-action` | `@v5` | Verify token secret name matches repo secret |
| `github/codeql-action/*` | `@v3` | v4 does not exist |
| `actions/dependency-review-action` | `@v4` | |
| `actions/labeler` | `@v5` | |
| `actions/stale` | `@v9` | |
| `actions/github-script` | `@v7` | |

> ⚠️ Always verify a version exists with `gh release list --repo <owner>/<repo>` before pinning. A non-existent version silently fails the CI step.

**Step 2 — Missing workflow capabilities** (P1)

| Capability | Current State | Workflow to Update | Action Needed |
|-----------|--------------|-------------------|---------------|
| SBOM generation | Missing | `release.yml` | Add `anchore/sbom-action` or `syft` after build step |
| Binary signing | Missing | `release.yml` | Add `signtool.exe` step (requires code-signing cert secret) |
| Auto-label PRs by path | Missing | `ci.yml` | Add `actions/labeler@v5` with `.github/labeler.yml` config |
| Dependabot config | Incomplete | `.github/dependabot.yml` | Add NuGet + GitHub Actions update groups |
| Release notes auto-gen | Manual CHANGELOG | `release.yml` | Add `--generate-notes` or `actions/release-drafter` |
| Workflow failure alert | Missing | All workflows | Add `if: failure()` step that creates a GH issue |

**Step 3 — Workflow consolidation with reusable workflows** (P2)

The NuGet restore + build + test pattern is duplicated across `ci.yml` and `release.yml`. Extract to a reusable workflow:

```yaml
# .github/workflows/reusable-build-test.yml
on:
  workflow_call:
    inputs:
      configuration:
        type: string
        default: Release
      upload-coverage:
        type: boolean
        default: false
jobs:
  build-test:
    runs-on: windows-latest
    env:
      MSBUILDDISABLENODEREUSE: 1
      FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true
    steps:
      # ... shared build + 3-project test steps ...
```

---

### 0.B.7 Prompts Expansion

**Goal**: A complete `.github/prompts/` library covering every common RegiLattice development task.

**Audit existing prompts**:

```powershell
Get-ChildItem .github/prompts/ -Filter *.prompt.md | Select-Object Name, LastWriteTime
```

**Prompts to create or verify** (P1 unless marked):

| Prompt File | Purpose | Mode | Key Tools |
|-------------|---------|------|----------|
| `add-tweaks.prompt.md` | Guided tweak authoring: duplicate check → module edit → test → build | `agent` | filesystem, github |
| `sprint-execution.prompt.md` | Full sprint: plan → code → build → test → commit | `agent` | filesystem, sequential-thinking |
| `version-bump.prompt.md` | 28-file version bump with PowerShell automation | `agent` | filesystem, github |
| `ci-failure-diagnosis.prompt.md` | Step-by-step CI failure triage | `agent` | github (run logs), filesystem |
| `test-coverage-gap.prompt.md` | Find and fill coverage gaps | `agent` | filesystem |
| `duplicate-audit.prompt.md` | Full 4-layer duplication scan + report | `agent` | filesystem, github |
| `roadmap-review.prompt.md` | Review roadmap against current codebase state | `ask` | project-docs |
| `release-verification.prompt.md` | Post-push release artifact verification | `agent` | github |

**Standard prompt template**:

```markdown
---
mode: agent
tools: [filesystem, github, project-docs, memory, sequential-thinking]
description: "One-line description"
---

# Task: <Name>

## Pre-conditions
<!-- What state the workspace must be in before running this prompt -->

## Steps
<!-- Numbered, deterministic, tool-driven steps -->

## Success Criteria
<!-- How the agent knows the task is complete -->

## Output
<!-- What the agent must produce (files, commits, a summary) -->
```

---

### 0.B.8 `copilot-instructions.md` Count Auto-Sync

**Problem**: `copilot-instructions.md` and the 8 instruction files contain hardcoded tweak/test/module counts that go stale after every sprint (currently updated manually as part of the 28-file version bump checklist).

**Solution**: A PowerShell script `scripts/Sync-CopilotInstructions.ps1`:

```powershell
# scripts/Sync-CopilotInstructions.ps1
param([Parameter(Mandatory)][string]$Version)

# 1. Get live counts
$tweakCount  = (dotnet run --project src/RegiLattice.CLI -- --stats --no-color 2>&1 |     Select-String 'Total tweaks:').ToString() -replace '.*:(\s+)', '' -replace '[^0-9]'
$moduleCount = (Get-ChildItem src/RegiLattice.Core/Tweaks/*.cs).Count
$testCount   = (Select-String -Path tests/**/*.cs -Pattern '\[Fact\]|\[Theory\]' -Recurse).Count

# 2. Update copilot-instructions.md
$files = @('.github/copilot-instructions.md') + (Get-ChildItem .github/instructions/*.md)
foreach ($file in $files) {
    (Get-Content $file) -replace '\d[, ]+\d{3} tweaks', "$tweakCount tweaks" |
        Set-Content $file
}

Write-Host "Synced counts: $tweakCount tweaks, $moduleCount modules, $testCount tests → v$Version"
```

This eliminates the manual count-update step from the version bump checklist. Run it as part of `scripts/` during every version bump.

---

### 0.B.9 Acceptance Criteria

Phase 0.B is complete when **all** of the following are true:

| Criterion | Verification Command |
|-----------|---------------------|
| `copilot-instructions.md` counts match live CLI output | `dotnet run --project src/RegiLattice.CLI -- --stats` vs file contents |
| All skill pages reference ≤1-sprint-old module/test counts | Spot-check 3 skills; compare counts to `--stats` output |
| All workflow action versions are pinned to verified latest stables | `Select-String .github/workflows/*.yml -Pattern 'uses:'` — manually verify top 5 |
| Agent definition lists every current skill and every `.github/prompts/*.prompt.md` | `Get-ChildItem .github/skills/ -Directory` vs agent definition skill list |
| MCP server list in agent definition matches `.vscode/mcp.json` | Diff server names in both files |
| Every planned prompt file exists and follows the standard template | `Get-ChildItem .github/prompts/ -Filter *.prompt.md` |
| `scripts/Sync-CopilotInstructions.ps1` exists and exits 0 with correct counts | Run script on current workspace; verify no unexpected changes |

### 0.B.10 Non-Goals

- This phase does **not** change any C# source code, tests, or the build system
- This phase does **not** bump the project version — it is a developer-tooling improvement only
- This phase does **not** add MCP servers without evaluating their stability on this machine first
- This phase does **not** restructure `.github/` directory layout — only file contents change
- This phase does **not** remove any existing instruction file — only additions and corrections

---

## Phase 0.C — Standards Alignment: VSCode, GitHub & SVG

> **Priority**: ⭐ P0 — Execute alongside Phase 0 and Phase 0.B.
> **Goal**: Bring every configuration file, document, and graphical asset into full conformance with the **latest VS Code, GitHub, and SVG standards** — eliminating stale formats, deprecated configuration keys, and hand-maintained diagrams in favour of standards-compliant, auto-renderable SVG representations.

---

### 0.C.1 Problem Statement

The project has accumulated configuration drift across three orthogonal axes:

| Axis | Problem | Impact |
|------|---------|--------|
| **VS Code config** | `.vscode/settings.json` uses deprecated keys (e.g., `editor.suggestOnTriggerCharacters` removed in 1.90+), missing new workspace-trust and Copilot settings, outdated task formats | Copilot features silently disabled; tasks fail on new VS Code versions |
| **GitHub config** | `.github/` files use deprecated ISSUE_TEMPLATE (v1 `*.md`), missing CODEOWNERS patterns, workflow action versions lag behind stable releases, Dependabot config incomplete | CI regressions; missed automated security PRs; stale issue triage |
| **SVG / Diagrams** | Diagrams are either hand-maintained SVG (drifts from code), Mermaid in README (renders via GitHub but no local preview), or absent | Stats go stale after every sprint; architecture diagrams mislead; no offline viewing |

---

### 0.C.2 VS Code Configuration Audit and Remediation

**0.C.2.1 — Settings audit**

Verify every key in `.vscode/settings.json` against the VS Code 1.99 (April 2026) release notes and settings schema. Flag and remove deprecated/renamed keys:

```powershell
# Compare workspace settings keys to a known-good baseline
(Get-Content .vscode/settings.json | ConvertFrom-Json).PSObject.Properties.Name | Sort-Object
```

Key areas to review:

| Setting Category | Current State | Required Action |
|-----------------|---------------|-----------------|
| `github.copilot.*` | Mixed — some keys removed in Copilot v1.240+ | Audit against Copilot extension changelog; remove `github.copilot.advanced` subtree if deprecated |
| `editor.inlayHints.*` | May reference old sub-key names | Verify against VS Code 1.90+ settings schema |
| `[csharp]` language overrides | Present but may conflict with CSharpier v0.30+ settings | Reconcile with `csharpier.configPath` and C# Dev Kit settings |
| `python.*` | Likely present from .venv setup; most are now under `"python.analysis.*"` (Pylance) | Remove legacy `python.linting.*` keys deprecated in Pylance 2024.x |
| `terminal.integrated.profiles.windows` | Custom `RegiLattice Dev` profile | Verify `source` key format matches VS Code 1.95+ terminal profile schema |
| `github.copilot.chat.agent.terminal.allowList` | Present | Verify format matches latest Copilot agent spec (`Record<string, boolean>` vs new format) |
| `files.watcherExclude` | Likely missing `**/obj/**` and `**/TestResults/**` | Add to prevent OneDrive file-watcher conflicts with build outputs |

**0.C.2.2 — Extensions manifest**

`.vscode/extensions.json` — verify every recommended extension ID is current:

```powershell
# Quick check all extension IDs are valid (requires VS Code CLI)
code --list-extensions | ForEach-Object {
    [pscustomobject]@{ Installed=$_; InManifest=(Select-String -Path .vscode/extensions.json -Pattern $_ -Quiet) }
}
```

| Extension | Check |
|-----------|-------|
| `ms-dotnettools.csharp` | Replaced by `ms-dotnettools.csdevkit` + `ms-dotnettools.csharp` pair — verify both present |
| `esbenp.prettier-vscode` | Not applicable to C#/PS; remove if present |
| `streetsidesoftware.code-spell-checker` | Add with `cSpell.words` array for RegiLattice domain terms (`TweakDef`, `RegOp`, `HKCU`, `NeedsAdmin`) |
| `ms-vscode.powershell` | Verify ID is `ms-vscode.powershell` (not legacy `ms-vscode.PowerShell` capitalisation) |
| `github.copilot` | Must be present; `github.copilot-chat` is now bundled |
| `usernamehw.errorlens` | Add — surfaces CS errors inline without opening Problems panel |

**0.C.2.3 — Tasks manifest**

`.vscode/tasks.json` — verify task format against VS Code 1.99 schema:

- All tasks must have `"presentation": { "reveal": "always" }` or `"silent"` — bare tasks without presentation group cause UX regression in VS Code 1.88+
- Replace deprecated `"problemMatcher": "$msCompile"` with `"problemMatcher": "$tsc"` where relevant
- Add a `"RunTests: All"` compound task that sequences Core → CLI → GUI tests with `dependsOrder: sequence`
- Add a `"Sync-Counts"` task that runs `scripts/Sync-CopilotInstructions.ps1` with the current version

**0.C.2.4 — Launch configuration**

`.vscode/launch.json` — verify format against VS Code 1.99 schema:

- `"type": "coreclr"` must be present for all .NET launch configs (not just `"clr"`)
- Add `"stopAtEntry": false` explicitly (default changed in C# Dev Kit 1.12)
- Add a CLI launch config with `"args": ["--list"]` for quick interactive testing

---

### 0.C.3 GitHub Configuration Standards

**0.C.3.1 — ISSUE_TEMPLATE modernisation**

GitHub's v1 issue templates (`.github/ISSUE_TEMPLATE/*.md` with YAML front-matter) are largely superseded by **Issue Forms** (`.github/ISSUE_TEMPLATE/*.yml` with `body:` sections). Verify the current templates:

```powershell
Get-ChildItem .github/ISSUE_TEMPLATE/ | Select-Object Name, Extension
```

Migration targets:

| Template | Current Format | Target Format | Action |
|----------|---------------|---------------|--------|
| `bug_report` | Likely `.md` (v1) | `.yml` (Issue Forms) | Convert: add `type: textarea` / `type: dropdown` / `type: checkboxes` fields |
| `feature_request` | Likely `.md` (v1) | `.yml` (Issue Forms) | Convert: structured fields for impact, scope, acceptance criteria |
| `release.yml` | Already `.yml` (checklist) | `.yml` ✅ | Verify checklist items match the current 28-file version bump list |
| `new_tweak_proposal` | May be absent | `.yml` (Issue Forms) | Create: fields for category slug, registry path, expected result, CorpSafe |

Issue Form format reference:

```yaml
# .github/ISSUE_TEMPLATE/bug_report.yml
name: Bug Report
description: Something is broken in RegiLattice
title: "[Bug]: "
labels: ["bug", "triage"]
assignees: ["RajwanYair"]
body:
  - type: markdown
    attributes:
      value: "Thanks for reporting a bug. Please fill in all fields."
  - type: textarea
    id: description
    attributes:
      label: What happened?
      placeholder: "Describe the bug clearly"
    validations:
      required: true
  - type: dropdown
    id: component
    attributes:
      label: Component
      options: ["GUI", "CLI", "Core Engine", "Registry", "Package Managers", "CI/CD"]
    validations:
      required: true
  - type: input
    id: version
    attributes:
      label: RegiLattice version
      placeholder: "e.g. v6.33.0"
    validations:
      required: true
  - type: checkboxes
    id: checklist
    attributes:
      label: Pre-submission checklist
      options:
        - label: I searched existing issues and this is not a duplicate
          required: true
        - label: I am running the latest release
          required: true
```

**0.C.3.2 — CODEOWNERS**

`.github/CODEOWNERS` — verify patterns cover the full directory tree and use correct GitHub username:

```
# .github/CODEOWNERS
*                                   @RajwanYair
src/RegiLattice.Core/Tweaks/**      @RajwanYair
.github/workflows/**                @RajwanYair
docs/**                             @RajwanYair
installer/**                        @RajwanYair
```

Ensure patterns use forward slashes and that the file is NOT nested inside a subdirectory (must be `.github/CODEOWNERS`, not `.github/config/CODEOWNERS`).

**0.C.3.3 — Dependabot configuration**

`.github/dependabot.yml` — verify it covers all package ecosystems used in the project:

```yaml
# .github/dependabot.yml
version: 2
updates:
  # .NET NuGet packages
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
      day: "monday"
    groups:
      test-tooling:
        patterns: ["xunit*", "Microsoft.NET.Test.Sdk", "coverlet*"]
      dotnet-runtime:
        patterns: ["System.*", "Microsoft.Extensions.*"]
    ignore:
      # Held at v2 pending xUnit v3 migration (see lessons-learned)
      - dependency-name: "xunit"
        versions: ["3.*"]
      - dependency-name: "xunit.runner.visualstudio"
        versions: ["3.*"]

  # GitHub Actions
  - package-ecosystem: "github-actions"
    directory: "/"
    schedule:
      interval: "weekly"
      day: "monday"
    groups:
      github-actions:
        patterns: ["*"]
```

**0.C.3.4 — Pull request template**

`.github/pull_request_template.md` — verify it follows GitHub's current template spec and prompts for:
- Conventional commit type + scope
- Tweak/test count deltas (e.g., "+50 tweaks, +32 tests")
- Checklist: build passes, tests pass, 0 warnings, CHANGELOG updated, SVG stats updated

```markdown
## Summary
<!-- type(scope): what and why -->

## Changes
- <!-- List key changes -->

## Counts (if tweaks/tests changed)
| Metric | Before | After |
|--------|--------|-------|
| Tweaks | | |
| Tests | | |
| Categories | | |

## Checklist
- [ ] `dotnet build RegiLattice.sln -c Release` — 0 errors, 0 warnings
- [ ] All 3 test projects pass (`dotnet test` per-project)
- [ ] CHANGELOG.md updated with new `## [X.Y.Z]` section
- [ ] `docs/assets/stats.svg` counts updated (space-separated thousands)
- [ ] No `TODO`/`FIXME` comments in committed code
- [ ] No `#pragma warning disable` or `[SuppressMessage]` added
```

**0.C.3.5 — Security policy and support files**

Verify `SECURITY.md` references the current GitHub Private Vulnerability Reporting URL format (changed in 2024):

```markdown
# Reporting a Vulnerability
Please use GitHub's [private vulnerability reporting](https://github.com/RajwanYair/RegiLattice/security/advisories/new)
rather than opening a public issue.
```

Verify `SUPPORT.md` links to current Discussions URL and references the latest `--doctor` CLI command.

---

### 0.C.4 SVG Standards and Graphical Representations

**0.C.4.1 — SVG compliance audit**

All SVG files in `docs/assets/` must comply with **SVG 1.1 / SVG 2.0** and render correctly in:
- GitHub Markdown preview (inline `<img src="...svg">` and direct SVG view)
- VS Code Markdown preview
- Modern browsers (Chrome 124+, Edge 124+, Firefox 125+)

```powershell
# Inventory all SVG files
Get-ChildItem docs/assets/*.svg | Select-Object Name, @{N='Lines';E={(Get-Content $_).Count}}, @{N='HasScript';E={(Select-String $_ -Pattern '<script').Count -gt 0}}
```

Current SVG inventory and known issues:

| File | Purpose | Known Issue | Fix |
|------|---------|-------------|-----|
| `stats.svg` | Tweaks / categories / tests counts badge | Hardcoded counts; space-separated thousands must be maintained manually | Add auto-update step to `Bump-Version.ps1` |
| `banner.svg` | Header graphic with feature counts | Contains hardcoded counts; may have non-standard `<foreignObject>` | Audit for `foreignObject` use; replace with `<text>` if present |
| `architecture.svg` | System architecture diagram | Mismatch with Phase 8+ planned architecture | Update architecture diagram to reflect DI + interface plan |
| `features.svg` | Per-category tweak counts | Counts stale after every sprint | Add to `Sync-CopilotInstructions.ps1` auto-sync |
| `how-it-works.svg` | User workflow diagram | Static; no auto-update mechanism | Accept manual update; document update trigger in version bump checklist |
| `project-structure.svg` | File / module / category counts | Counts stale | Add to auto-sync script |
| `solution-overview.svg` | Core engine metrics | Counts stale | Add to auto-sync script |

**SVG compliance checklist** for each file:

```
☐ DOCTYPE or XML declaration correct: <?xml version="1.0" encoding="UTF-8"?>
☐ SVG namespace: xmlns="http://www.w3.org/2000/svg"
☐ viewBox attribute present (not just width/height) — required for responsive scaling
☐ No <script> elements (GitHub strips them; security risk)
☐ No external http:// image references (privacy + mixed-content issues)
☐ Fonts referenced via system-safe fallback stack: font-family="system-ui, -apple-system, sans-serif"
☐ Text elements use dominant-baseline or dy for vertical alignment (not deprecated alignment-baseline)
☐ No use of deprecated xlink:href — use href directly (SVG 2.0+)
☐ aria-label or <title> element present on root <svg> (accessibility)
☐ Dark-mode compatible via prefers-color-scheme media query OR separate dark variants
```

**0.C.4.2 — New SVG diagrams to create**

The following architecture and data-flow diagrams are absent but valuable for developer onboarding and AI-assisted development:

| Diagram | File | Purpose | Format |
|---------|------|---------|--------|
| **TweakDef lifecycle** | `docs/assets/tweak-lifecycle.svg` | From definition → registration → detection → apply/remove → history | SVG (flowchart) |
| **TweakEngine API surface** | `docs/assets/engine-api.svg` | All public methods grouped by concern (search, apply, status, profiles, export) | SVG (grouped bubbles) |
| **Registry operation types** | `docs/assets/regop-types.svg` | 12 RegOp factories, their inputs, outputs, and DetectOps counterparts | SVG (table-style) |
| **GUI component tree** | `docs/assets/gui-components.svg` | WinForms control hierarchy: MainForm → panels → dialogs | SVG (tree) |
| **CI/CD pipeline** | `docs/assets/cicd-pipeline.svg` | 6 workflows with trigger arrows and artifact flow | SVG (pipeline) |
| **Data persistence map** | `docs/assets/data-persistence.svg` | JSON files in `%LOCALAPPDATA%\RegiLattice\`, their owners and formats | SVG (map) |

**SVG authoring standards for new diagrams**:

```xml
<!-- Template: every new SVG must start with this header -->
<?xml version="1.0" encoding="UTF-8"?>
<svg xmlns="http://www.w3.org/2000/svg"
     viewBox="0 0 1200 600"
     role="img"
     aria-label="[Diagram title]">
  <title>[Diagram title] — RegiLattice v6.33+</title>
  <desc>[One-sentence description of what the diagram shows]</desc>
  <!-- Use system-safe font stack -->
  <style>
    text { font-family: system-ui, -apple-system, 'Segoe UI', sans-serif; }
    .label { font-size: 13px; fill: #24292f; }
    .heading { font-size: 15px; font-weight: 600; fill: #1f2328; }
    /* Dark-mode support */
    @media (prefers-color-scheme: dark) {
      .label { fill: #e6edf3; }
      .heading { fill: #f0f6fc; }
    }
  </style>
  <!-- diagram content -->
</svg>
```

**0.C.4.3 — Auto-sync stats SVG via `Bump-Version.ps1`**

The `scripts/Bump-Version.ps1` script (added in v6.28.0) handles the 28-file version bump checklist. Extend it to auto-update all count-bearing SVG files:

```powershell
# Add to scripts/Bump-Version.ps1 — SVG count auto-update section

function Update-SvgCounts {
    param([string]$TweakCount, [string]$CategoryCount, [string]$TestCount, [string]$ModuleCount)

    # stats.svg uses space-separated thousands: "7 718" not "7718"
    $tweakFormatted    = Format-SpaceSeparated $TweakCount
    $categoryFormatted = Format-SpaceSeparated $CategoryCount
    $testFormatted     = Format-SpaceSeparated $TestCount

    $svgFiles = @{
        'docs/assets/stats.svg'            = @{ tweaks=$tweakFormatted; categories=$categoryFormatted; tests=$testFormatted }
        'docs/assets/banner.svg'           = @{ tweaks=$tweakFormatted; categories=$categoryFormatted }
        'docs/assets/features.svg'         = @{ tweaks=$tweakFormatted }
        'docs/assets/architecture.svg'     = @{ tweaks=$tweakFormatted; categories=$categoryFormatted; modules=$ModuleCount }
        'docs/assets/how-it-works.svg'     = @{ tweaks=$tweakFormatted }
        'docs/assets/project-structure.svg'= @{ tweaks=$tweakFormatted; categories=$categoryFormatted }
        'docs/assets/solution-overview.svg'= @{ tweaks=$tweakFormatted }
    }

    foreach ($file in $svgFiles.Keys) {
        if (-not (Test-Path $file)) { Write-Warning "$file not found — skipping"; continue }
        # Update is done by sed-equivalent PowerShell replacements
        # Each SVG has known text node patterns — update them here
        Write-Host "  Updated $file"
    }
}

function Format-SpaceSeparated([string]$n) {
    # "7718" -> "7 718", "158" -> "158", "3296" -> "3 296"
    if ($n.Length -gt 3) { return $n.Substring(0, $n.Length - 3) + ' ' + $n.Substring($n.Length - 3) }
    return $n
}
```

**0.C.4.4 — README Mermaid diagrams**

The Mermaid diagrams in `README.md` render via GitHub's built-in Mermaid support (as of 2022). Verify:

1. All Mermaid blocks use ` ```mermaid ` fencing (not ` ```diagram `)
2. Module/category counts embedded in Mermaid node labels match `docs/assets/stats.svg`
3. Mermaid syntax is valid with `mermaid-diagram-validator` (available as deferred tool)
4. At least one diagram uses `flowchart TD` or `graph TD` style (most compatible with GitHub rendering)
5. No `%%` comment lines adjacent to node definitions (triggers Mermaid 10.x parse errors)

```powershell
# Extract all Mermaid blocks from README for validation
$content = Get-Content README.md -Raw
$blocks = [regex]::Matches($content, '```mermaid\r?\n(.*?)\r?\n```', 'Singleline')
$blocks | ForEach-Object { Write-Host "Block $($_.Index): $($_.Groups[1].Value.Substring(0, [Math]::Min(80,$_.Groups[1].Value.Length)))..." }
```

---

### 0.C.5 Documentation Standards

**0.C.5.1 — Markdown linting**

All `.md` files in `docs/`, `.github/`, and root must pass `markdownlint` with the project's configured ruleset. Current known violations:

| Rule | Violation Pattern | Fix |
|------|------------------|-----|
| MD022 | `####` headings without blank line below (CHANGELOG) | Add blank line after every `####` heading |
| MD013 | Lines > 120 chars in instruction files | Disable MD013 globally (already in `.vscode/settings.json`) |
| MD033 | Raw HTML in markdown (e.g., `<details>`, `<summary>`) | Allow selectively; add to markdownlint config |
| MD041 | First line of file not an H1 | Verify all docs files start with `# Title` |
| MD024 | Duplicate headings in CHANGELOG | Disable MD024 for CHANGELOG only (use `<!-- markdownlint-disable MD024 -->`) |

```powershell
# Run markdownlint across all docs
npx markdownlint-cli2 "docs/**/*.md" ".github/**/*.md" "README.md" "CHANGELOG.md"
```

**0.C.5.2 — Broken link audit**

Verify all cross-document links in `docs/` are non-broken:

```powershell
# Find all markdown links and check they resolve
$links = Select-String -Path docs/*.md, .github/**/*.md -Pattern '\[.*?\]\((.*?)\)' -AllMatches |
    ForEach-Object { $_.Matches.Groups[1].Value } |
    Where-Object { $_ -notmatch '^https?://' -and $_ -notmatch '^#' } |
    Sort-Object -Unique

$links | ForEach-Object {
    $resolved = Join-Path docs $_
    [pscustomobject]@{ Link=$_; Exists=(Test-Path $resolved) }
} | Where-Object { -not $_.Exists }
```

**0.C.5.3 — `docs/tweak-schema.json` validation**

The newly added `docs/tweak-schema.json` (JSON Schema 2020-12) should be validated against the spec and used in:

- `PackLoader.ValidatePackJson()` — reference the schema URI for validation
- `pack-validation.yml` CI workflow — run `ajv validate` against official packs
- VS Code settings — add `json.schemas` entry so `.rlpack.json` files get IntelliSense:

```jsonc
// .vscode/settings.json addition
"json.schemas": [
    {
        "fileMatch": ["**/*.rlpack.json"],
        "url": "https://raw.githubusercontent.com/RajwanYair/RegiLattice/main/docs/tweak-schema.json"
    }
]
```

---

### 0.C.6 Implementation Sequence

Execute steps in this order to minimise regressions:

| Step | Action | Risk | Verification |
|------|--------|------|-------------|
| **C.1** | Audit `.vscode/settings.json` — remove deprecated keys | Low | VS Code opens without warnings in Output panel |
| **C.2** | Update `.vscode/extensions.json` — add `csdevkit`, `errorlens`, `cSpell` | Low | `code --install-extension` for new IDs |
| **C.3** | Update `.vscode/tasks.json` — add presentation, fix deprecated matchers, add compound tasks | Low | Run each task manually; verify no error |
| **C.4** | Update `.vscode/launch.json` — add `stopAtEntry`, fix type | Low | Debug launch works |
| **C.5** | Modernise `.github/ISSUE_TEMPLATE/` — convert bug/feature to Issue Forms | Low | Create a test issue on GitHub; verify form renders |
| **C.6** | Update `.github/CODEOWNERS` — verify patterns | Low | `git log --diff-filter=A` shows correct reviewer |
| **C.7** | Update `.github/dependabot.yml` — add NuGet groups + xUnit ignore | Low | Dependabot PRs appear on Monday as configured |
| **C.8** | Update `.github/pull_request_template.md` — add counts table + checklist | Low | Open a test PR; verify template renders |
| **C.9** | SVG compliance audit — run compliance checklist on all 7 SVGs | Medium | All SVGs render in VS Code Markdown Preview |
| **C.10** | Extend `Bump-Version.ps1` with `Update-SvgCounts` function | Medium | Run script on current workspace; SVGs updated correctly |
| **C.11** | Add `json.schemas` for `.rlpack.json` to `.vscode/settings.json` | Low | Open a `.rlpack.json` file; IntelliSense shows field completions |
| **C.12** | Validate `docs/tweak-schema.json` with `ajv` against official packs | Low | `npx ajv validate -s docs/tweak-schema.json -d packs/*.rlpack.json` exits 0 |
| **C.13** | Create missing SVG diagrams (lifecycle, API surface, CI pipeline) | High | SVGs render in browser and GitHub; reviewed by user |
| **C.14** | Run markdownlint across all docs; fix MD022 violations | Low | `npx markdownlint-cli2 "docs/**/*.md"` exits 0 |
| **C.15** | Validate README Mermaid blocks | Low | `mermaid-diagram-validator` reports no errors |

---

### 0.C.7 Acceptance Criteria

Phase 0.C is complete when **all** of the following are true:

| Criterion | Verification Command / Check |
|-----------|------------------------------|
| No deprecated VS Code settings keys | VS Code Output panel shows no `[settings]` deprecation warnings on workspace open |
| All recommended extensions present and using current IDs | `code --list-extensions` matches `.vscode/extensions.json` |
| All VS Code tasks run without errors | Run each task from the Command Palette; all exit 0 |
| All `.github/ISSUE_TEMPLATE/` files are Issue Forms (`.yml`) | `Get-ChildItem .github/ISSUE_TEMPLATE/ -Filter *.md` returns empty |
| `CODEOWNERS` covers full directory tree | Create a test PR touching `src/` and `docs/`; both auto-assign `@RajwanYair` |
| `dependabot.yml` covers `nuget` + `github-actions` ecosystems with correct ignore list | File present; `nuget` and `github-actions` both appear as package ecosystems |
| All 7 SVG files pass compliance checklist (viewBox, no scripts, system font, aria-label) | Manual review with browser devtools |
| `Bump-Version.ps1` auto-updates all SVG count fields | Run script on test version; diff confirms all SVG count fields updated |
| `.vscode/settings.json` includes `json.schemas` for `.rlpack.json` | Open `packs/gaming-fps.rlpack.json`; IntelliSense shows field completions |
| `docs/tweak-schema.json` validates all 5 official packs | `npx ajv validate -s docs/tweak-schema.json -d packs/*.rlpack.json` exits 0 |
| All SVG diagrams render correctly in GitHub Markdown and VS Code preview | Push to a test branch; view SVGs in GitHub UI |
| `npx markdownlint-cli2 "docs/**/*.md" "README.md"` exits 0 | Run command; 0 violations |
| README Mermaid blocks validate without errors | `mermaid-diagram-validator` reports clean |

---

### 0.C.8 Non-Goals

- This phase does **not** change any C# source code, tests, or the version number
- This phase does **not** redesign the visual style of existing SVGs — only compliance and correctness
- This phase does **not** migrate from GitHub Actions to another CI platform
- This phase does **not** add new tweak categories or modules
- This phase does **not** create a design system — SVG diagrams are informational, not UI components
- This phase does **not** set up a GitHub Pages docs site (that is in Phase 14)

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

### 13.0 Test Suite Consolidation & Performance

**Priority**: P0 — Critical (execute before all other Phase 13 items)

The test suite has grown organically to 3,296 tests across 51 files. Execution time is
dominated by redundant `RegisterBuiltins()` calls (~200 ms each × 7,718 tweaks) scattered
across individual test methods. The largest file — `ExtendedCoverageTests.cs` — contains
**58 independent test classes** in a single ~4,800-line monolith.

#### Current State (v6.33.0)

| Metric | Value |
|--------|-------|
| Total tests | 3,296 (Core 2,499 · CLI 434 · GUI 363) |
| Core test files | 40 |
| Test classes in `ExtendedCoverageTests.cs` | 58 |
| Standalone `RegisterBuiltins()` calls (Core) | 36 |
| Already migrated to `BuiltinsFixture` | 13 classes (via `[Collection("Builtins")]`) |
| Remaining unmigrated files | 7 files (BatchImpactEstimator, TweakDef, Intune, Plugin, Phase2, TweakEngineBuiltins, ExtendedCoverage) |

#### Problem Analysis

1. **Redundant engine initialization**: Each `RegisterBuiltins()` call constructs a
   fresh `TweakEngine`, registers all 195 module files (7,718 tweaks), and builds
   internal indices. At ~200 ms per call × 36 standalone calls ≈ 7.2 s wasted.
2. **Monolith test files**: `ExtendedCoverageTests.cs` (58 classes, ~4,800 lines) is
   unmaintainable. Test classes cover unrelated services (Analytics, PackLoader,
   RegistrySession, StartupManager, CorporateGuard) in one file.
3. **Overlapping coverage**: Several "BranchTests" and "BranchTests2" classes in
   `ExtendedCoverageTests.cs` duplicate assertions that already exist in dedicated
   test files (e.g., `AppConfigTests.cs`, `FavoritesTests.cs`, `PluginTests.cs`).
4. **Sequential per-project execution**: Cross-assembly file races require sequential
   project execution (`MaxCpuCount=1`), but intra-project parallelism is underused.

#### Execution Plan

**Step 1 — Complete BuiltinsFixture migration** (P0, immediate)

Migrate remaining 7 files to use the shared `BuiltinsFixture` via `[Collection("Builtins")]`:

| File | Standalone calls | Action |
|------|-----------------|--------|
| `TweakDefTests.cs` | 12 | Inject `BuiltinsFixture`, replace per-test `new TweakEngine()` |
| `ExtendedCoverageTests.cs` | 12 | Inject fixture into applicable classes |
| `BatchImpactEstimatorTests.cs` | 2 | Inject fixture |
| `TweakEngineBuiltinsTests.cs` | 2 | Remove 2 remaining standalone calls |
| `Phase2Tests.cs` | 1 | Inject fixture |
| `IntuneExporterTests.cs` | 1 | Inject fixture |
| `PluginTests.cs` | 1 | Inject fixture |

**Expected savings**: Eliminate ~30 redundant `RegisterBuiltins()` calls → ~6 s faster.

**Step 2 — Split ExtendedCoverageTests.cs** (P0, immediate)

Decompose the 58-class monolith into domain-specific test files:

| Target file | Classes to move | Rationale |
|-------------|----------------|-----------|
| `AppConfigTests.cs` | AppConfigLoadNoArg, AppConfigBrightness, AppConfigValidate, AppConfigPortable, AppConfigBranch | Join existing AppConfig tests |
| `FavoritesTests.cs` | FavoritesWhitespace, FavoritesBranch | Join existing Favorites tests |
| `RegistrySessionTests.cs` | RegistrySessionCheckValue, RegistrySessionBranch2, RegistrySessionQwordBinary | Join existing RegistrySession tests |
| `PluginTests.cs` | PackLoader*, PackManager*, PackConflict, PackLoaderNullJson | Join existing Plugin tests |
| `StartupManagerTests.cs` | StartupManagerBranch, StartupManagerBranch2, StartupManagerRemaining | Join existing StartupManager tests |
| `ComplianceServiceTests.cs` | ComplianceHistoryNullJson, ComplianceDriftAdditional, ComplianceReportExporter | Join existing Compliance tests |
| `HealthScoreServiceTests.cs` | HealthScoreEmpty, HealthScoreServiceBranch | Join existing HealthScore tests |
| `TweakEngineTests.cs` | TweakEngineBranch, TweakEngineIsApplicable, TweakEnginePartial, TweakEngineRegisterInstalledPacks | Join existing engine tests |
| `HardwareInfoTests.cs` (new) | HardwareInfoSoftwareDetection, HardwareInfoProfile | Hardware-specific tests |
| `SecurityTests.cs` (new) | ElevationBranch2, ElevationAllowedCommand, CorporateGuardRemaining, SshHardening2 | Security-specific tests |
| `ServiceManagerTests.cs` (new) | ServiceManagerBranch, NetworkManagerBranch, UpdateCheckService, UpdateCheckService2, ScheduledTweakService | Service manager tests |
| `MiscBranchTests.cs` (keep) | Remaining edge-case classes | Catch-all for genuinely miscellaneous tests |

**Expected result**: `ExtendedCoverageTests.cs` shrinks from 58 classes → ~5 miscellaneous.
Each domain file gains relevant branch-coverage tests alongside its primary tests.

**Step 3 — Deduplicate overlapping tests** (P1, next sprint)

Audit for test methods that assert the same behaviour:
- Compare all `*BranchTests` and `*BranchTests2` classes against their primary test files
- Merge tests that exercise the same code path with the same assertions
- Replace multi-assertion `[Fact]` methods with `[Theory]` where appropriate
- Target: remove ~100–200 redundant tests without reducing code coverage

**Step 4 — Optimize intra-project parallelism** (P1)

- Group tests into xUnit collections by shared resource (file-system, engine state)
- Tests without shared resources run in parallel (default xUnit behaviour)
- Add `[Collection("FileSystem")]` to all tests writing to `%LOCALAPPDATA%\RegiLattice\`
- Increase `MaxCpuCount` from 1 to 4 in `.runsettings` for within-assembly parallelism
- Verify no new file races via CI

**Step 5 — Reduce per-test overhead** (P2)

- Cache `TweakEngine.AllTweaks()` results in test fixtures (avoid repeated LINQ)
- Use `IAsyncLifetime` for tests that need async setup/teardown
- Profile with `dotnet-trace` to identify remaining hotspots
- Add performance-budget tests for the test suite itself (total Core test time < 60 s)

#### Success Metrics

| Metric | Current | Target |
|--------|---------|--------|
| Core.Tests execution time | ~90 s | < 60 s |
| `RegisterBuiltins()` standalone calls | 36 | 1 (in fixture only) |
| `ExtendedCoverageTests.cs` classes | 58 | ≤ 5 |
| Total test files (Core) | 40 | ~45 (split from monolith) |
| Redundant test methods removed | 0 | 100–200 |

#### Risk Mitigation

- **Coverage regression**: Run `dotnet test --collect:"XPlat Code Coverage"` before and
  after each step. No line below 90% gate.
- **File-race reintroduction**: Keep sequential per-project execution. Only increase
  intra-project parallelism after verifying collection isolation.
- **Merge conflicts during split**: Perform the ExtendedCoverageTests split in a
  dedicated branch with a single PR to minimize conflict surface.

---

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

### 14.0 `.github` Copilot Surface Modernisation

**Priority**: P0 — Critical (execute before all other Phase 14 items)

**Problem**: The `.github` Copilot surface (instructions, agent, skills, prompts, MCP servers, workflows) was built incrementally over 33 minor versions. Each piece works, but the surface has grown organically without a unified design pass. Result: duplicated guidance across files, skills that don't leverage the latest tool capabilities, an agent definition that lists tools without describing decision logic, and MCP servers that could provide richer context.

**Current inventory** (31 files):

| Layer | Files | Observation |
|-------|-------|-------------|
| Instructions | 7 (`copilot-instructions.md` + 6 scoped `.instructions.md`) | Heavy overlap between copilot-instructions and workspace instructions; lessons-learned is 1,100+ lines of prose that should be enforced by tests/analyzers |
| Agent | 1 (`regilattice.agent.md`) | Lists 45 tools but no decision-tree or routing logic; no mode definitions (sprint vs. review vs. debug) |
| Skills | 10 (`add-tweaks`, `architecture`, `debug-fix`, `gui-themes`, `no-duplication`, `package-managers`, `release`, `search-tweaks`, `testing`, `tool-versions`) | Missing skills for: CI/CD troubleshooting, performance profiling, migration/upgrade, compliance audit |
| Prompts | 8 (add-tweak, check-tool-updates, code-review, create-project, fix-quality, package-health, search-tweaks, write-tests) | `create-project` references Python UEF v12 (unrelated to this C# workspace); no prompt for release prep, changelog generation, or SVG count updates |
| MCP Servers | 5 (github, filesystem, project-docs, memory, sequential-thinking) | All stdio servers duplicate the same 8-line env block; no Pylance or .NET-aware MCP; sequential-thinking rarely used |
| Workflows | 6 (`ci.yml`, `release.yml`, `weekly.yml`, `smoke.yml`, `pages.yml`, `packages.yml`) | Solid coverage; minor: mutation testing is in `ci.yml` weekly schedule but could be its own workflow for clarity |
| Issue Templates | 8 (bug, feature, tweak, build, performance, pack-submission, release, config) | Complete; no gaps identified |
| Other | labeler.yml, PULL_REQUEST_TEMPLATE.md, dependabot.yml, FUNDING.yml, CODEOWNERS | Current and correct |

**Execution plan** — 5 steps:

---

#### Step 1 — Instruction File Consolidation (eliminate overlap)

**Target**: 7 instruction files → 5, total line count reduced 40%.

| Action | Detail |
|--------|--------|
| Merge `workspace.instructions.md` into `copilot-instructions.md` | The workspace file duplicates the solution structure, build commands, and architecture overview already in copilot-instructions; deduplicate into one authoritative file |
| Merge `cicd.instructions.md` into `git-workflow.instructions.md` | Both cover CI/release workflow; consolidate into a single "workflow & CI" reference |
| Merge `no-duplication.instructions.md` into `csharp.instructions.md` | Duplication rules are C# coding standards; keep one scoped file for `**/*.cs` |
| Trim `lessons-learned.instructions.md` | Convert enforceable rules into Roslyn analyzers or test assertions; keep only non-automatable architectural lessons (target: 600 lines → 300 lines) |
| Keep `testing.instructions.md` standalone | Already well-scoped to `**/tests/**`; no overlap |

**Deliverable**: 5 instruction files with no content duplication, each under 400 lines.

---

#### Step 2 — Agent Definition Overhaul

**Target**: Transform the agent from a tool list into a decision-routing engine.

| Enhancement | Detail |
|-------------|--------|
| **Mode definitions** | Define 5 explicit modes: `sprint` (add tweaks), `debug` (fix errors), `review` (code quality), `release` (version bump), `explore` (architecture Q&A). Each mode specifies which skills to load, which tools to prefer, and what standing rules apply |
| **Decision tree** | Add a routing flowchart: user intent → mode selection → skill loading → tool selection. Replace the flat 45-tool list with mode-scoped tool groups |
| **Guardrails per mode** | Sprint mode: enforce commit-per-phase, duplicate check, build verification. Release mode: enforce issue/PR flow, 31-item checklist. Debug mode: enforce get_errors first, then targeted reads |
| **MCP integration** | Document when to use MCP tools vs. built-in tools (e.g., `mcp_github_search_code` for cross-repo searches, built-in `grep_search` for local) |
| **Context loading** | On session start, auto-load: current branch, dirty file count, last test run status, tweak/test counts from `Directory.Build.props` |

**Template structure**:
```yaml
modes:
  sprint:
    skills: [add-tweaks, no-duplication, testing]
    tools_prefer: [grep_search, read_file, replace_string_in_file, run_in_terminal]
    standing_rules: [commit_per_phase, duplicate_check, build_verify]
  debug:
    skills: [debug-fix, architecture]
    tools_prefer: [get_errors, grep_search, read_file, semantic_search]
    standing_rules: [errors_first, targeted_reads, no_guessing]
  release:
    skills: [release, search-tweaks]
    tools_prefer: [mcp_github_*, run_in_terminal, multi_replace_string_in_file]
    standing_rules: [issue_pr_flow, full_checklist, tag_push_mandatory]
```

---

#### Step 3 — Skill Gap Analysis & New Skills

**Target**: 10 skills → 14 (add 4 missing domain skills).

| New Skill | Trigger Phrases | Content |
|-----------|----------------|---------|
| `ci-troubleshoot` | "CI failed", "workflow error", "action broken", "build red" | Diagnose GitHub Actions failures: check action version existence, parse error logs, identify YAML syntax issues, runner-specific failures (Windows SDK, NuGet restore). Reference canonical action versions table. |
| `perf-profile` | "slow", "performance", "benchmark", "profiling" | Run BenchmarkDotNet benchmarks, interpret results, identify hot paths. Guide: `dotnet-trace`, `dotnet-counters`, `PerfView` for WinForms. Budget thresholds for search (<150ms), StatusMap (<5s), GUI startup (<3s). |
| `migration-upgrade` | "upgrade", "migrate", ".NET version", "NuGet update" | .NET SDK upgrade checklist, TFM change, NuGet package holds (xUnit v2→v3 gating), breaking change scan. Safe update order: SDK → NuGet → TFM → test. |
| `compliance-audit` | "audit", "compliance", "corporate", "policy" | Run `--validate` + duplication audit + profile coverage check + CorporateGuard status. Generate compliance report JSON/CSV. |

**Update existing skills**:

| Skill | Enhancement |
|-------|-------------|
| `release` | Add SVG count update commands, package registry manifest update steps, GitHub About sidebar update. Currently references lessons-learned externally — inline the 31-item checklist directly. |
| `testing` | Add BuiltinsFixture migration guide, ExtendedCoverageTests decomposition steps, test performance budget guidance. |
| `add-tweaks` | Add `ImpactScore`/`SafetyRating` calibration guidance with examples per score level. Add IsApplicable predicate patterns for software detection. |
| `debug-fix` | Add OneDrive cache-lock recovery (MSBUILDDISABLENODEREUSE), Hebrew terminal injection handling, CSharpier vs. CS-error discrimination. |

---

#### Step 4 — Prompt Refresh & New Prompts

**Target**: 8 prompts → 11 (add 3, update 2, remove 1).

| Action | Prompt | Detail |
|--------|--------|--------|
| **Add** | `release-prep.prompt.md` | Pre-release checklist: version file scan, SVG count verification, CHANGELOG section template, package manifest diff, draft PR creation command |
| **Add** | `changelog-entry.prompt.md` | Generate a CHANGELOG section from git log since last tag: categorise commits by type (feat/fix/perf/refactor), count tweaks/tests delta, format with Stats line |
| **Add** | `svg-count-update.prompt.md` | Scan all 9 SVG files + README + instruction files for stale counts; generate bulk `replace_string_in_file` operations for all outdated values |
| **Update** | `code-review.prompt.md` | Add: duplication layer check (Layer 1–4), ImpactScore/SafetyRating validation, sealed-class enforcement, IsApplicable predicate check |
| **Update** | `write-tests.prompt.md` | Add: BuiltinsFixture usage pattern, `[Collection("Builtins")]` requirement for RegisterBuiltins tests, CorporateGuard stub mandate |
| **Remove** | `create-project.prompt.md` | References Python UEF v12 — unrelated to this C# workspace. Move to the shared MyScripts prompt library if still needed. |

---

#### Step 5 — MCP Server Optimisation

**Target**: Cleaner config, deduplicated env blocks, evaluate new MCP capabilities.

| Action | Detail |
|--------|--------|
| **Deduplicate env blocks** | Extract the 8-line proxy/PATH env block into a shared `$env` variable or use a base env object spread. Currently duplicated 4× across filesystem, project-docs, memory, sequential-thinking |
| **Evaluate Pylance MCP** | VS Code Pylance MCP tools (`mcp_pylance_*`) are available but unused — irrelevant for a C# project. Document explicitly as "not applicable" to prevent future confusion |
| **Add .NET-aware context** | Evaluate `mcp_sequential-th_sequentialthinking` for complex debugging workflows — currently configured but rarely invoked. Add explicit guidance in the agent definition for when to use it (circular dependency analysis, multi-file refactors) |
| **Memory server hygiene** | `.github/mcp-memory.jsonl` should be reviewed and pruned quarterly; add a scheduled prompt or checklist item. Stale entities from old sprint states accumulate and pollute context |
| **GitHub MCP capabilities** | Document the full tool surface of the GitHub remote MCP: `mcp_github_search_code`, `mcp_github_list_issues`, `mcp_github_create_pull_request`, etc. Currently listed in agent tools but not described in any skill |

---

**Success metrics**:

| Metric | Before | After |
|--------|--------|-------|
| Instruction file count | 7 files, ~3,500 total lines | 5 files, ~2,100 total lines |
| Agent modes defined | 0 (flat tool list) | 5 (sprint, debug, review, release, explore) |
| Skills coverage | 10 skills, no CI/perf/migration/compliance | 14 skills, full development lifecycle |
| Prompt coverage | 8 prompts (1 irrelevant Python) | 11 prompts, all C#/RegiLattice-specific |
| MCP env duplication | 4× identical 8-line blocks | 1× shared block |
| Instruction overlap | ~40% content duplication | <5% duplication |

**Risk mitigations**:

| Risk | Mitigation |
|------|------------|
| Instruction consolidation breaks Copilot `applyTo` scoping | Keep `csharp.instructions.md` scoped to `**/*.cs` and `testing.instructions.md` to `**/tests/**`; only merge files with identical `applyTo: "**"` scope |
| Agent mode routing confuses Copilot model | Modes are advisory, not enforced by tooling; the agent still responds to any request regardless of mode |
| Removing `create-project.prompt.md` loses Python capability | Move to `MyScripts/.github/prompts/` (shared) before deleting from workspace |
| Lessons-learned trimming loses institutional knowledge | Convert to tests/analyzers FIRST, verify they catch the documented failure, THEN remove the prose entry |

---

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
Phase 13.0 (Test Consolidation)        ─── v6.34.0 (P0 — immediate, no version gate)
  ↓
Phase 14.0 (Copilot Surface)          ─── v6.35.0 (P0 — no code changes, docs only)
  ↓
Phase 11 (Scope Discipline)           ─── v7.0.0 (MAJOR — breaking: dialogs extracted)
  ↓
Phase 12.1–12.3 (CI + Registry Cleanup) ── v7.1.0
  ↓
Phase 8.1–8.2 (DI + Interfaces)       ─── v7.2.0
  ↓
Phase 9.1–9.2 (SQLite + Repository)    ─── v7.3.0
  ↓
Phase 14.1–14.4 (Docs Consolidation)  ─── v7.4.0
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
