# RegiLattice — Copilot Instructions

> Auto-loaded by GitHub Copilot on every chat/agent session in this workspace.
> Keep this file accurate — it is the fastest path to project understanding.
> Last verified: 2026-05-03 (v6.7.0, ~9,490 tweaks, 121 categories, 3,376 tests).

## Companion Instruction Files

All files below are auto-loaded into every Copilot chat session via `.vscode/settings.json`:

| File                                                   | Scope         | Purpose                                          |
| ------------------------------------------------------ | ------------- | ------------------------------------------------ |
| `.github/copilot-instructions.md`                      | `**`          | This file — project overview and shell rules     |
| `.github/instructions/workspace.instructions.md`       | `**`          | Solution structure, architecture, build commands |
| `.github/instructions/csharp.instructions.md`          | `**/*.cs`     | C# 13 coding standards and patterns              |
| `.github/instructions/testing.instructions.md`         | `**/tests/**` | xUnit test patterns, coverage targets            |
| `.github/instructions/git-workflow.instructions.md`    | `**`          | Commit/push strategy, conventional commits       |
| `.github/instructions/lessons-learned.instructions.md` | `**/*.cs`     | Hard-won migration insights, common pitfalls     |
| `.github/instructions/cicd.instructions.md`            | `**/*.yml`    | GitHub Actions, release workflow                 |
| `.github/instructions/no-duplication.instructions.md`  | `**/*.cs`     | Duplication detection rules and prevention       |

### Environment Bootstrap

Dot-source `.env.ps1` at the project root to ensure **all** CLI tools are on PATH:

```powershell
. .\.env.ps1
```

This is auto-loaded by the default VS Code terminal profile (`RegiLattice Dev`).
See `docs/archive/Win11_tools.engineer‑friendly.tool.md` and `docs/archive/SCOOP.engineer‑friendly.tool.md`
for the full tool reference (local only — not tracked in git).

## ⛔ CRITICAL: PowerShell ONLY — NO Unix/Bash Commands

**This is a Windows-only workspace. EVERY terminal command MUST use PowerShell syntax.**
**Violation of this rule is a hard error. STOP and rewrite using PowerShell before executing.**

Before running ANY terminal command, verify it does NOT contain: `tail`, `grep`,
`cat`, `ls`, `rm`, `cp`, `mv`, `mkdir`, `touch`, `wc`, `which`, `find`, `diff`,
`echo ... >>`, `export`, `&&`, `bash`, `sh`, `zsh`, `python3`, or any other
Unix/POSIX utility.

| BANNED (Unix/bash)    | REQUIRED (PowerShell)                               |
| --------------------- | --------------------------------------------------- |
| `tail -n 20 file`     | `Get-Content file \| Select-Object -Last 20`        |
| `tail -f file`        | `Get-Content file -Wait`                            |
| `grep pattern file`   | `Select-String -Pattern 'pattern' file`             |
| `grep -r pattern .`   | `Get-ChildItem -Recurse \| Select-String 'pattern'` |
| `ls -la` / `ls`       | `Get-ChildItem` (or `gci`)                          |
| `cat file`            | `Get-Content file`                                  |
| `rm -rf dir`          | `Remove-Item -Recurse -Force dir`                   |
| `cp src dst`          | `Copy-Item src dst`                                 |
| `mv src dst`          | `Move-Item src dst`                                 |
| `mkdir dir`           | `New-Item dir -ItemType Directory`                  |
| `touch file`          | `New-Item file -ItemType File`                      |
| `wc -l file`          | `(Get-Content file).Count`                          |
| `which cmd`           | `Get-Command cmd`                                   |
| `echo text >> file`   | `Add-Content file 'text'`                           |
| `find . -name "*.py"` | `Get-ChildItem -Recurse -Filter '*.py'`             |
| `diff a b`            | `Compare-Object (Get-Content a) (Get-Content b)`    |
| `export VAR=val`      | `$env:VAR = 'val'`                                  |
| `&&` chaining         | `;` or `if ($LASTEXITCODE -eq 0) { ... }`           |

Rules:

1. Every shell command MUST be valid PowerShell syntax — no exceptions.
2. No `bash`, `sh`, `zsh`, `fish`, or Unix coreutil invocations.
3. Use `dotnet` CLI for build/test/run — never `python` or `python3`.
4. Use `pwsh` when invoking PowerShell scripts from the terminal.
5. Path separators: use `\` or `Join-Path`.
6. When piping to limit output, use `Select-Object -First N` or `-Last N`.

---

## Quick Facts

| Key      | Value                                                                    |
| -------- | ------------------------------------------------------------------------ |
| Language | C# 13 / .NET 10.0-windows (x64)                                          |
| Build    | `dotnet build` / MSBuild via `RegiLattice.sln`                           |
| Test     | xUnit 2.9.3 — 3,376 tests (0 failures)                                   |
| GUI      | WinForms with 11 themes (Catppuccin Mocha/Latte, Nord, Dracula + 7 more) |
| Version  | 6.7.0                                                                   |
| Install  | `dotnet build RegiLattice.sln -c Release`                                |
| Tweaks   | 9,490 across 121 categories (160 module classes, 45 files)              |
| Tests    | 3376 passing (0 consistent failures)                                     |
| NuGet    | System.Management 10.0.5, Microsoft.NET.Test.Sdk 17.14.1                 |

## Git Workflow (IMPORTANT — STANDING RULE)

> **This mandate applies to EVERY session without exception.**
> Every sprint, phase, or named task MUST be committed before moving to the next one.
> Do NOT batch multiple sprints/phases into a single commit — one commit per logical unit.

- **Commit per logical phase/task** during a session (granular local history)
- **Each commit must state** what phase it covers + total tweak/test counts if changed
- **Push + tag on EVERY version bump** — NEVER defer a pushed tag; tag push triggers GitHub Actions `release.yml` which publishes EXEs + MSI to GitHub Releases
- **Non-version-bump work** stays in local commits until wrapped into the next version bump
- Commit message format: `type(scope): description` (Conventional Commits)
- Full details: `.github/instructions/git-workflow.instructions.md`

### Version Bump → Push Flow (MANDATORY every time)

```powershell
git add -A
git commit -m "feat(tweaks): Sprint NNN — N tweaks, vX.Y.Z"
git tag vX.Y.Z
git push; git push --tags   # ← REQUIRED on every version bump
```

---

## Architecture at a Glance

> Full annotated solution tree: see `.github/instructions/workspace.instructions.md` — Solution Structure section.

Key namespaces: `RegiLattice.Core` (engine + models + registry + tweak modules, 44 files), `RegiLattice.GUI` (WinForms, 11 themes), `RegiLattice.CLI` (25+ commands). Tests live in `tests/` — 3 projects, 3,376 total.

### TweakDef Model

```csharp
public sealed class TweakDef
{
    public required string Id { get; init; }          // unique kebab-case
    public required string Label { get; init; }       // human name
    public required string Category { get; init; }    // UI grouping
    public string Description { get; init; } = "";
    public IReadOnlyList<string> Tags { get; init; } = [];
    public bool NeedsAdmin { get; init; } = true;
    public bool CorpSafe { get; init; }
    public int MinBuild { get; init; }
    public IReadOnlyList<string> RegistryKeys { get; init; } = [];
    public IReadOnlyList<string> DependsOn { get; init; } = [];
    public string SideEffects { get; init; } = "";
    public string SourceUrl { get; init; } = "";
    public string ExpectedResult { get; init; } = ""; // auto-generated if empty

    // Declarative RegOp pattern (preferred — ~95% of tweaks)
    public IReadOnlyList<RegOp> ApplyOps { get; init; } = [];
    public IReadOnlyList<RegOp> RemoveOps { get; init; } = [];
    public IReadOnlyList<RegOp> DetectOps { get; init; } = [];

    // Custom logic delegates (complex tweaks only — ~5%)
    public Action<bool>? ApplyAction { get; init; }   // bool = dryRun
    public Action<bool>? RemoveAction { get; init; }
    public Func<bool>? DetectAction { get; init; }
    public Action<bool>? UpdateAction { get; init; }  // for package managers

    // Kind & applicability
    public TweakKind? KindHint { get; init; }         // override auto-detection
    public Func<bool>? IsApplicable { get; init; }    // hardware gating predicate
    public string ApplicabilityNote { get; init; } = ""; // reason when not applicable
    public string? PackSource { get; init; }          // plugin name if from pack

    // Impact & safety metadata (Phase C — Intelligence Engine)
    public int ImpactScore { get; init; } = 3;        // 1 = minimal benefit, 5 = major benefit
    public int SafetyRating { get; init; } = 4;       // 1 = risky, 5 = very safe
    public string ImpactNote { get; init; } = "";     // short human description of expected impact

    // Computed
    public TweakKind Kind { get; }                    // auto-detected from category/ops or KindHint
    public bool HasOperations { get; }                // ApplyOps.Count > 0 || ApplyAction != null
    public TweakScope Scope => ComputeScope();        // User, Machine, or Both
    public string GetExpectedResult();                // returns ExpectedResult or auto-generated
}
```

### TweakKind — 8 Operation Variants

> Full table with `TweakDef Fields Used` per kind: see `.github/instructions/workspace.instructions.md` — TweakKind section.

### TweakResult — 7 Outcomes

| Result         | Meaning                              |
| -------------- | ------------------------------------ |
| `Applied`      | Tweak is active                      |
| `NotApplied`   | Tweak is not active                  |
| `Unknown`      | Detection failed or inconclusive     |
| `Error`        | Exception during apply/remove/detect |
| `SkippedCorp`  | Blocked by CorporateGuard            |
| `SkippedBuild` | MinBuild not met                     |
| `SkippedHw`    | IsApplicable returned false          |

### RegOp Factory Methods (12)

```csharp
RegOp.SetDword(path, name, value)        // REG_DWORD
RegOp.SetString(path, name, value)       // REG_SZ
RegOp.SetExpandString(path, name, value) // REG_EXPAND_SZ
RegOp.SetQword(path, name, value)        // REG_QWORD
RegOp.SetBinary(path, name, bytes)       // REG_BINARY
RegOp.SetMultiSz(path, name, strings)   // REG_MULTI_SZ
RegOp.DeleteValue(path, name)
RegOp.DeleteTree(path)
RegOp.CheckDword(path, name, expected)   // Detection
RegOp.CheckString(path, name, expected)
RegOp.CheckMissing(path, name)
RegOp.CheckKeyMissing(path)
```

### TweakEngine Public API

**Core**: `Register()`, `RegisterBuiltins()`, `AllTweaks()`, `GetTweak()`, `Categories()`, `TweaksByCategory()`, `TweaksByIds()`, `TweaksByTag()`, `TweaksByScope()`, `GetScope()`

**Search & Filter**: `Search(query)`, `Filter(corpSafe?, needsAdmin?, scope?, category?, minBuild?, query?)`

**Status**: `DetectStatus(td)`, `StatusMap(parallel, ids?)`

**Apply/Remove**: `Apply()`, `Remove()`, `ApplyBatch()`, `RemoveBatch()` (+ progress callback overloads)

**Validation**: `ValidateTweaks()` — checks IDs, labels, categories, broken DependsOn, circular deps

**Dependencies**: `ResolveDependencies(id)` — topological sort, `Dependents(id)` — reverse lookup

**Profiles**: `Profiles` (5 static), `GetProfile()`, `TweaksForProfile()`, `ApplyProfile()`

**Snapshots**: `SaveSnapshot()`, `LoadSnapshot()`, `RestoreSnapshot()`

**Stats**: `CategoryCounts()`, `ScopeCounts()`, `ExportJson()`, `WindowsBuild()`

### RegistrySession API

**Write**: `SetDword`, `SetString`, `SetExpandString`, `SetQword`, `SetBinary`, `SetMultiSz`, `SetValue`, `DeleteValue`, `DeleteTree`

**Read**: `ReadDword`, `ReadString`, `ReadQword`, `ReadBinary`, `ReadMultiSz`, `ReadValue`

**Check**: `KeyExists`, `ValueExists`, `ListSubKeys`, `ListValueNames`

**Execute**: `Execute(IReadOnlyList<RegOp>)`, `Evaluate(IReadOnlyList<RegOp>)`

**Backup**: `Backup(keys, label)` — JSON backup in `%LOCALAPPDATA%\RegiLattice\backups\`

**Other**: `DryRun`, `DryOps`, `Log`, `WriteLog()`

### 5 Profiles

| Profile    | Categories | Description                                     |
| ---------- | ---------- | ----------------------------------------------- |
| `business` | 39         | Productivity, security, cloud & workflow        |
| `gaming`   | 31         | GPU, performance, low-latency, distraction-free |
| `privacy`  | 31         | Telemetry, tracking, cloud & browser data       |
| `minimal`  | 22         | Fast, clean system essentials                   |
| `server`   | 28         | Hardened, headless, uptime & remote mgmt        |

### Corporate Guard

`CorporateGuard.cs` detects corporate environments via:

1. AD domain membership (`GetComputerNameExW` P/Invoke)
2. Azure AD / Entra ID join status
3. Group Policy registry indicators
4. SCCM / Intune enrollment (WMI)

If corporate detected → tweaks with `CorpSafe = false` are blocked.
Override: `--force` CLI flag or GUI "Force" checkbox.

### GUI Details

- 11 colour themes: Catppuccin Mocha (default), Catppuccin Latte, Nord, Dracula, Tokyo Night, Gruvbox Dark, Solarized Dark, One Dark Pro, Rosé Pine, Everforest, Cyberpunk — switchable at runtime
- System theme auto-detection (follows Windows dark/light mode on startup)
- Collapsible category sections with tweak counts
- Scope badges: USER (green) / MACHINE (blue) / BOTH (yellow)
- Kind column with symbol from `CategoryIcons.GetKindSymbol()`
- Search bar + status filters + scope filters
- Profile selector dropdown
- Theme selector
- **Log panel (bottom, visible by default)** — timestamped operation log, toggle via File menu
- Package manager dialogs (Scoop, pip, PowerShell modules)
- About dialog with hardware info
- Double-buffered rendering for smooth scrolling
- Minimize to system tray with context menu
- Percentage progress bar for batch operations

## CLI Commands (25+)

| Command                           | Description                                               |
| --------------------------------- | --------------------------------------------------------- |
| `apply <id>`                      | Apply a single tweak                                      |
| `remove <id>`                     | Remove/revert a single tweak                              |
| `update <id>`                     | Update a tweak (runs UpdateAction or falls back to Apply) |
| `status <id>`                     | Check current status                                      |
| `--list`                          | List all tweaks                                           |
| `--search <query>`                | Full-text search                                          |
| `--show-categories`               | List all categories                                       |
| `--show-tags`                     | List all tags                                             |
| `--profile <name>`                | Apply a profile                                           |
| `--list-profiles`                 | List available profiles                                   |
| `--category <name> apply\|remove` | Batch by category                                         |
| `--snapshot <file>`               | Save state to JSON                                        |
| `--restore <file>`                | Restore from snapshot                                     |
| `--snapshot-diff <a> <b>`         | Compare snapshots                                         |
| `--export-json <file>`            | Export as JSON                                            |
| `--export-reg <file>`             | Export as .REG                                            |
| `--import-json <file>`            | Import selection                                          |
| `--validate`                      | Integrity check                                           |
| `--stats`                         | Statistics summary                                        |
| `--doctor`                        | System health check                                       |
| `--hwinfo`                        | Hardware detection                                        |
| `--report`                        | Full report                                               |
| `--check`                         | Admin status check                                        |
| `--gui`                           | Launch WinForms GUI                                       |
| `--menu`                          | Interactive console menu                                  |
| `--dry-run`                       | Preview mode                                              |
| `--force`                         | Override corporate guard                                  |
| `--config <path>`                 | Custom config file                                        |
| `--depends-on <id>`               | Show dependency chain for a tweak                         |
| `--no-color`                      | Disable ANSI colour output                                |

## Tweak ID Naming Convention

All tweak IDs follow the pattern: `{category_slug}-{descriptive-name}`

Canonical category slugs:

| Slug       | Category                  | Slug       | Category              |
| ---------- | ------------------------- | ---------- | --------------------- |
| `acc`      | Accessibility             | `lo`       | LibreOffice           |
| `adobe`    | Adobe                     | `lock`     | Lock Screen & Login   |
| `ai`       | AI / Copilot              | `m365`     | M365 Copilot          |
| `audio`    | Audio                     | `maint`    | Maintenance           |
| `backup`   | Backup & Recovery         | `media`    | Multimedia            |
| `boot`     | Boot                      | `mem`      | Memory                |
| `browser`  | Browser Common            | `msstore`  | Microsoft Store       |
| `bt`       | Bluetooth                 | `net`      | Network               |
| `chrome`   | Chrome                    | `netopt`   | Network Optimization  |
| `cleanup`  | Disk Cleanup              | `night`    | Night Light & Display |
| `clip`     | Clipboard & Drag-Drop     | `notif`    | Notifications         |
| `cloud`    | Cloud Storage             | `od`       | OneDrive              |
| `cmd`      | Command Line              | `office`   | Office                |
| `comm`     | Communication             | `perf`     | Performance           |
| `compat`   | App Compatibility         | `phone`    | Phone Link            |
| `cortana`  | Cortana & Search          | `pkg`      | Package Management    |
| `crash`    | Crash & Diagnostics       | `power`    | Power                 |
| `ctx`      | Context Menu              | `printing` | Printing              |
| `debloat`  | Debloat                   | `priv`     | Privacy               |
| `dev`      | Dev Drive / Developer     | `proxy`    | Proxy & VPN           |
| `display`  | Display                   | `ps`       | PowerShell            |
| `dns`      | DNS & Networking Advanced | `pwrmgmt`  | Power Management      |
| `edge`     | Edge                      | `rdp`      | Remote Desktop        |
| `enc`      | Encryption                | `recall`   | Windows Recall        |
| `evtlog`   | Event Logging             | `recovery` | Recovery              |
| `explorer` | Explorer                  | `restore`  | System Restore        |
| `firefox`  | Firefox                   | `schtask`  | Scheduled Tasks       |
| `font`     | Fonts                     | `scoop`    | Scoop Tools           |
| `fs`       | File System               | `sec`      | Security              |
| `fw`       | Firewall                  | `shell`    | Shell                 |
| `game`     | Gaming                    | `snap`     | Snap & Multitasking   |
| `gpu`      | GPU / Graphics            | `speech`   | Voice Access & Speech |
| `harden`   | Hardening                 | `ss`       | Screensaver & Lock    |
| `idx`      | Indexing & Search         | `ssd`      | SSD Optimization      |
| `input`    | Input                     | `startup`  | Startup               |
| `java`     | Java                      | `stor`     | Storage               |
| `svc`      | Services                  | `sys`      | System                |
| `tb`       | Taskbar                   | `telem`    | Telemetry Advanced    |
| `term`     | Windows Terminal          | `touch`    | Touch & Pen           |
| `uac`      | User Account              | `virt`     | Virtualization        |
| `usb`      | USB & Peripherals         | `vnc`      | RealVNC               |
| `vscode`   | VS Code                   | `w11`      | Windows 11            |
| `widgets`  | Widgets & News            | `wsl`      | WSL                   |
| `wu`       | Windows Update            |            |                       |

## Test Infrastructure

> Full test file inventory and coverage targets: see `.github/instructions/testing.instructions.md` — Test File Structure section.

Projects: `RegiLattice.Core.Tests` (2,315 tests), `RegiLattice.CLI.Tests` (379 tests), `RegiLattice.GUI.Tests` (358 tests). Total: 3,376.

## Adding a New Tweak — Checklist

1. Create/edit `src/RegiLattice.Core/Tweaks/<Category>.cs`
2. Define `private const string Key = @"HKEY_...";` at module top
3. Add `TweakDef` to the `Tweaks` list with `ApplyOps`/`RemoveOps`/`DetectOps`
4. Ensure `Id` is **globally unique** kebab-case
5. Set `ImpactScore` (1–5) and `SafetyRating` (1–5) explicitly — do not rely on defaults (3/4)
6. Register the module in `TweakEngine.RegisterBuiltins()` (one line)
7. Run: `dotnet test`

## Common Pitfalls

> Full list of 47 hard-won lessons: see `.github/instructions/lessons-learned.instructions.md`.

Top 5 most critical:

- **Duplicate IDs**: `TweakEngine.Register()` throws on duplicate IDs — every `Id` must be globally unique across ALL modules. Search before adding.
- **Module already exists**: Before `create_file`, always `list_dir` the `Tweaks/` directory. `create_file` fails silently if the file exists — read first and edit instead.
- **`AppConfig.ConfigDir` not `DataRoot`**: The correct data-directory property is `ConfigDir`. `DataRoot` does **not exist** → `CS0117`.
- **`get_errors` shows CSharpier diffs**: "Replace ⏎ with ..." diagnostics are formatter issues, not CS compiler errors. Only act on CS-prefixed errors.
- **OneDrive CoreCompile cache lock**: If build fails with `"Building target 'CoreCompile' completely"`, delete `$env:TEMP\RegiLattice-build\RegiLattice.Core` and retry. Set `MSBUILDDISABLENODEREUSE=1`.

## File-by-File Quick Ref

| File / Namespace                | Purpose                | Key Exports                                                                                           |
| ------------------------------- | ---------------------- | ----------------------------------------------------------------------------------------------------- |
| `TweakEngine.cs`                | Central engine         | `Register`, `AllTweaks`, `Categories`, `Search`, `Filter`, `Apply`, `StatusMap`, `Profiles`, `Freeze` |
| `SnapshotManager.cs`            | Snapshot management    | `Save()`, `Load()`, `Restore()`                                                                       |
| `TweakValidator.cs`             | Tweak validation       | `Validate()` (static — checks IDs, labels, deps, circular refs)                                       |
| `DependencyResolver.cs`         | Dependency resolution  | `Resolve()`, `Dependents()` (static — topological sort)                                               |
| `TweakDef.cs`                   | Tweak model            | `TweakDef`, `RegOp`, `TweakScope`, `TweakResult`, `RegOpKind`                                         |
| `ProfileDef.cs`                 | Profile model          | `ProfileDef` (Name, Description, ApplyCategories, SkipCategories)                                     |
| `ProfileDefinitions.cs`         | 5 profiles             | `All` static list                                                                                     |
| `RegistrySession.cs`            | Registry wrapper       | `SetDword`, `ReadDword`, `Execute`, `Evaluate`, `Backup`, `DryRun`                                    |
| `PackDef.cs`                    | Pack model             | `PackDef` record (Name, Version, Author, TweakCount, Tags, Sha256)                                    |
| `PackLoader.cs`                 | Pack loader            | `LoadFromJson`, `ValidatePackJson`, `ComputeSha256`                                                   |
| `PackManager.cs`                | Pack manager           | `InstallPackAsync`, `UninstallPack`, `InstalledPacks`, `CheckUpdatesAsync`                            |
| `PackIndex.cs`                  | Marketplace index      | `FromJson`, `ToJson`, `Packs`                                                                         |
| `CorporateGuard.cs`             | Corp detection         | `IsCorporateNetwork()`, `Status()`, `IsGpoManaged()`                                                  |
| `Elevation.cs`                  | UAC helpers            | `IsAdmin()`, `RequestElevation()`                                                                     |
| `HardwareInfo.cs`               | Hardware detection     | `Detect()`, `Summary()`, `SuggestProfile()`                                                           |
| `Analytics.cs`                  | Usage analytics        | `RecordApply()`, `RecordRemove()`, `GetStats()`                                                       |
| `AppConfig.cs`                  | Configuration          | `Load()`, `ForceCorpGuard`, `Theme`, `Locale`                                                         |
| `Locale.cs`                     | i18n (en, de)          | `T()`, `SetLocale()`, `CurrentLocale`, `AvailableLocales`                                             |
| `Ratings.cs`                    | Rating system          | `Rate()`, `GetRating()`, `AllRatings()`, `TopRated()`                                                 |
| `ConfigExporter.cs`             | Config export/import   | `Export()`, `ExportApplied()`, `Import()`, `Validate()`                                               |
| `Favorites.cs`                  | Favorite tweaks        | `Add()`, `Remove()`, `Toggle()`, `IsFavorite()`, `All()`, `Flush()`                                   |
| `TweakHistory.cs`               | Operation history      | `RecordApply()`, `RecordRemove()`, `RecordUpdate()`, `Recent()`, `ForTweak()`                         |
| `ShellRunner.cs` (Core)         | Process execution      | `RunPowerShell()`, `RunCommand()` (safe subprocess wrapper)                                           |
| `ChocolateyManager.cs` (Core)   | Chocolatey integration | Package install, list, update via choco CLI                                                           |
| `PipManager.cs` (Core)          | pip integration        | Package install, list, update via pip CLI                                                             |
| `WinGetManager.cs` (Core)       | WinGet integration     | Package install, list, update via winget CLI                                                          |
| `SystemMonitor.cs` (Core)       | System monitoring      | `GetCpuUsagePercent()`, `GetMemoryUsage()`, `GetUptime()` (P/Invoke)                                  |
| `Theme.cs` (GUI)                | Theme engine           | `SetTheme()`, `DetectSystemTheme()`, `AvailableThemes()`, `ThemeDef` record                           |
| `AppIcons.cs` (GUI)             | Icon generation        | Programmatic bitmap/icon creation for menus and tray                                                  |
| `MainForm.cs` (GUI)             | Main window            | Category list, search, filters, profiles, tweak operations, tray icon                                 |
| `PackageNameValidator.cs` (GUI) | Name validation        | Shared regex validation for all package manager dialogs                                               |
| `Program.cs` (CLI)              | CLI entry              | 25+ commands via args parsing                                                                         |
| `CliArgs.cs` (CLI)              | CLI argument model     | Mode, Tweak, ShowList, Force, DryRun, etc.                                                            |
| `ConsoleColorizer.cs` (CLI)     | ANSI colour helpers    | `Green()`, `Red()`, `Yellow()`, `Dim()`, `NoColor`                                                    |
