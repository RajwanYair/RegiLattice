# RegiLattice — Copilot Instructions

> Auto-loaded by GitHub Copilot on every chat/agent session in this workspace.
> Keep this file accurate — it is the fastest path to project understanding.
> Last verified: 2025-07-22 (v3.2.0, 2 316 tweaks, 89 categories, 700 tests).

## Companion Instruction Files

All files below are auto-loaded into every Copilot chat session via `.vscode/settings.json`:

| File | Scope | Purpose |
|------|-------|---------|
| `.github/copilot-instructions.md` | `**` | This file — project overview and shell rules |
| `.github/instructions/workspace.instructions.md` | `**` | Solution structure, architecture, build commands |
| `.github/instructions/csharp.instructions.md` | `**/*.cs` | C# 13 coding standards and patterns |
| `.github/instructions/testing.instructions.md` | `**/tests/**` | xUnit test patterns, coverage targets |
| `.github/instructions/git-workflow.instructions.md` | `**` | Commit/push strategy, conventional commits |
| `.github/instructions/lessons-learned.instructions.md` | `**/*.cs` | Hard-won migration insights, common pitfalls |
| `.github/instructions/cicd.instructions.md` | `**/*.yml` | GitHub Actions, release workflow |

### Environment Bootstrap

Dot-source `.env.ps1` at the project root to ensure **all** CLI tools are on PATH:

```powershell
. .\.env.ps1
```

This is auto-loaded by the default VS Code terminal profile (`RegiLattice Dev`).
See `Win11_tools.engineer‑friendly.tool.md` and `SCOOP.engineer‑friendly.tool.md`
in the repo root for the full tool reference.

## ⛔ CRITICAL: PowerShell ONLY — NO Unix/Bash Commands

**This is a Windows-only workspace. EVERY terminal command MUST use PowerShell syntax.**
**Violation of this rule is a hard error. STOP and rewrite using PowerShell before executing.**

Before running ANY terminal command, verify it does NOT contain: `tail`, `grep`,
`cat`, `ls`, `rm`, `cp`, `mv`, `mkdir`, `touch`, `wc`, `which`, `find`, `diff`,
`echo ... >>`, `export`, `&&`, `bash`, `sh`, `zsh`, `python3`, or any other
Unix/POSIX utility.

| BANNED (Unix/bash)               | REQUIRED (PowerShell)                                 |
| -------------------------------- | ----------------------------------------------------- |
| `tail -n 20 file`                | `Get-Content file \| Select-Object -Last 20`          |
| `tail -f file`                   | `Get-Content file -Wait`                              |
| `grep pattern file`              | `Select-String -Pattern 'pattern' file`               |
| `grep -r pattern .`              | `Get-ChildItem -Recurse \| Select-String 'pattern'`   |
| `ls -la` / `ls`                  | `Get-ChildItem` (or `gci`)                            |
| `cat file`                       | `Get-Content file`                                    |
| `rm -rf dir`                     | `Remove-Item -Recurse -Force dir`                     |
| `cp src dst`                     | `Copy-Item src dst`                                   |
| `mv src dst`                     | `Move-Item src dst`                                   |
| `mkdir dir`                      | `New-Item dir -ItemType Directory`                    |
| `touch file`                     | `New-Item file -ItemType File`                        |
| `wc -l file`                     | `(Get-Content file).Count`                            |
| `which cmd`                      | `Get-Command cmd`                                     |
| `echo text >> file`              | `Add-Content file 'text'`                             |
| `find . -name "*.py"`            | `Get-ChildItem -Recurse -Filter '*.py'`               |
| `diff a b`                       | `Compare-Object (Get-Content a) (Get-Content b)`      |
| `export VAR=val`                 | `$env:VAR = 'val'`                                    |
| `&&` chaining                    | `;` or `if ($LASTEXITCODE -eq 0) { ... }`             |

Rules:
1. Every shell command MUST be valid PowerShell syntax — no exceptions.
2. No `bash`, `sh`, `zsh`, `fish`, or Unix coreutil invocations.
3. Use `dotnet` CLI for build/test/run — never `python` or `python3`.
4. Use `pwsh` when invoking PowerShell scripts from the terminal.
5. Path separators: use `\` or `Join-Path`.
6. When piping to limit output, use `Select-Object -First N` or `-Last N`.

---

## Quick Facts

| Key         | Value                                                            |
| ----------- | ---------------------------------------------------------------- |
| Language    | C# 13 / .NET 10.0-windows (x64)                                 |
| Build       | `dotnet build` / MSBuild via `RegiLattice.sln`                   |
| Test        | xUnit 2.9.2 — 700 tests across 8 test files                     |
| GUI         | WinForms with 4 themes (Catppuccin Mocha/Latte, Nord, Dracula)   |
| Version     | 3.2.0                                                            |
| Install     | `dotnet build RegiLattice.sln -c Release`                        |
| Tweaks      | 2 316 across 89 categories (90 module files)                      |
| Tests       | 700 passing (571 Core + 58 CLI + 71 GUI)                         |
| NuGet       | System.Management 9.0.3, xUnit 2.9.2, coverlet 6.0.2            |

## Git Workflow (IMPORTANT)

- **Commit per logical phase/task** during a session (granular local history)
- **Push to GitHub only at end of a chat session** — never mid-session
- Commit message format: `type(scope): description` (Conventional Commits)
- Full details: `.github/instructions/git-workflow.instructions.md`

---

## Architecture at a Glance

```
RegiLattice.sln
├── src/
│   ├── RegiLattice.Core/            # Class library — engine, models, registry, services
│   │   ├── TweakEngine.cs           # Central tweak manager (register, apply, search, profiles)
│   │   ├── Models/
│   │   │   ├── TweakDef.cs          # Immutable tweak definition + RegOp + TweakScope + TweakResult
│   │   │   ├── ProfileDef.cs        # Profile definition model
│   │   │   └── ProfileDefinitions.cs # 5 hardcoded profiles
│   │   ├── Registry/
│   │   │   └── RegistrySession.cs   # Registry read/write/backup/execute wrapper
│   │   ├── Services/
│   │   │   ├── Analytics.cs         # Local usage analytics
│   │   │   ├── AppConfig.cs         # Configuration management
│   │   │   ├── CorporateGuard.cs    # Corporate network detection (P/Invoke + WMI)
│   │   │   ├── Elevation.cs         # UAC elevation helpers
│   │   │   ├── HardwareInfo.cs      # Hardware detection + profile suggestion
│   │   │   ├── Locale.cs            # i18n string table (English + German)
│   │   │   └── Ratings.cs           # Tweak rating system (1-5 stars)
│   │   ├── Plugins/                 # Tweak Pack system (JSON marketplace)
│   │   │   ├── PackDef.cs           # Pack metadata record
│   │   │   ├── PackLoader.cs        # JSON→TweakDef converter with validation
│   │   │   ├── PackManager.cs       # Install, uninstall, update, marketplace
│   │   │   └── PackIndex.cs         # Remote marketplace index model
│   │   └── Tweaks/                  # 90 category modules, 2 316 tweaks total
│   │       ├── Accessibility.cs
│   │       ├── Performance.cs
│   │       ├── Privacy.cs
│   │       ├── ...                  # 86 more
│   │       └── Wsl.cs
│   ├── RegiLattice.GUI/            # WinForms application
│   │   ├── Program.cs              # Entry point
│   │   ├── Theme.cs                # 4-theme engine (ThemeDef record, runtime switching)
│   │   └── Forms/
│   │       ├── MainForm.cs         # Main window (categories, search, filters, profiles)
│   │       ├── AboutDialog.cs      # About + hardware info
│   │       ├── ScoopManagerDialog.cs
│   │       └── PSModuleManagerDialog.cs
│   └── RegiLattice.CLI/           # Console application
│       └── Program.cs             # 25+ commands via args parsing
├── tests/
│   ├── RegiLattice.Core.Tests/    # 571 xUnit tests
│   │   ├── TweakDefTests.cs
│   │   ├── TweakEngineTests.cs
│   │   ├── RegistrySessionTests.cs
│   │   ├── ServicesTests.cs
│   │   └── PluginTests.cs          # Pack system + locale tests
│   ├── RegiLattice.CLI.Tests/     # 58 xUnit tests
│   │   └── ParseArgsTests.cs
│   └── RegiLattice.GUI.Tests/    # 71 xUnit tests
│       ├── ThemeTests.cs
│       └── PackageManagerValidationTests.cs
└── archive/                      # Archived Python v1.x + old NativeGUI (untracked)
```

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

    // Declarative RegOp pattern (preferred — ~95% of tweaks)
    public IReadOnlyList<RegOp> ApplyOps { get; init; } = [];
    public IReadOnlyList<RegOp> RemoveOps { get; init; } = [];
    public IReadOnlyList<RegOp> DetectOps { get; init; } = [];

    // Custom logic delegates (complex tweaks only — ~5%)
    public Action<bool>? ApplyAction { get; init; }
    public Action<bool>? RemoveAction { get; init; }
    public Func<bool>? DetectAction { get; init; }

    // Computed
    public TweakScope Scope => ComputeScope();  // User, Machine, or Both
}
```

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

| Profile    | Categories | Description                                    |
| ---------- | ---------- | ---------------------------------------------- |
| `business` | 39         | Productivity, security, cloud & workflow       |
| `gaming`   | 31         | GPU, performance, low-latency, distraction-free|
| `privacy`  | 31         | Telemetry, tracking, cloud & browser data      |
| `minimal`  | 22         | Fast, clean system essentials                  |
| `server`   | 28         | Hardened, headless, uptime & remote mgmt       |

### Corporate Guard

`CorporateGuard.cs` detects corporate environments via:

1. AD domain membership (`GetComputerNameExW` P/Invoke)
2. Azure AD / Entra ID join status
3. Group Policy registry indicators
4. SCCM / Intune enrollment (WMI)

If corporate detected → tweaks with `CorpSafe = false` are blocked.
Override: `--force` CLI flag or GUI "Force" checkbox.

### GUI Details

- 4 colour themes: Catppuccin Mocha (default), Catppuccin Latte, Nord, Dracula — switchable at runtime
- System theme auto-detection (follows Windows dark/light mode on startup)
- Collapsible category sections with tweak counts
- Scope badges: USER (green) / MACHINE (blue) / BOTH (yellow)
- Search bar + status filters + scope filters
- Profile selector dropdown
- Theme selector
- Package manager dialogs (Scoop, pip, PowerShell modules)
- About dialog with hardware info
- Double-buffered rendering for smooth scrolling
- Minimize to system tray with context menu
- Percentage progress bar for batch operations

## CLI Commands (25+)

| Command | Description |
|---------|-------------|
| `apply <id>` | Apply a single tweak |
| `remove <id>` | Remove/revert a single tweak |
| `update <id>` | Update a tweak (runs UpdateAction or falls back to Apply) |
| `status <id>` | Check current status |
| `--list` | List all tweaks |
| `--search <query>` | Full-text search |
| `--show-categories` | List all categories |
| `--show-tags` | List all tags |
| `--profile <name>` | Apply a profile |
| `--list-profiles` | List available profiles |
| `--category <name> apply\|remove` | Batch by category |
| `--snapshot <file>` | Save state to JSON |
| `--restore <file>` | Restore from snapshot |
| `--snapshot-diff <a> <b>` | Compare snapshots |
| `--export-json <file>` | Export as JSON |
| `--export-reg <file>` | Export as .REG |
| `--import-json <file>` | Import selection |
| `--validate` | Integrity check |
| `--stats` | Statistics summary |
| `--doctor` | System health check |
| `--hwinfo` | Hardware detection |
| `--report` | Full report |
| `--check` | Admin status check |
| `--gui` | Launch WinForms GUI |
| `--menu` | Interactive console menu |
| `--dry-run` | Preview mode |
| `--force` | Override corporate guard |
| `--config <path>` | Custom config file |
| `--depends-on <id>` | Show dependency chain for a tweak |
| `--no-color` | Disable ANSI colour output |

## Tweak ID Naming Convention

All tweak IDs follow the pattern: `{category_slug}-{descriptive-name}`

Canonical category slugs:

| Slug       | Category                    | Slug       | Category              |
| ---------- | --------------------------- | ---------- | --------------------- |
| `acc`      | Accessibility               | `lo`       | LibreOffice           |
| `adobe`    | Adobe                       | `lock`     | Lock Screen & Login   |
| `ai`       | AI / Copilot                | `m365`     | M365 Copilot          |
| `audio`    | Audio                       | `maint`    | Maintenance           |
| `backup`   | Backup & Recovery           | `media`    | Multimedia            |
| `boot`     | Boot                        | `msstore`  | Microsoft Store       |
| `bt`       | Bluetooth                   | `net`      | Network               |
| `chrome`   | Chrome                      | `night`    | Night Light & Display |
| `clip`     | Clipboard & Drag-Drop       | `notif`    | Notifications         |
| `cloud`    | Cloud Storage               | `od`       | OneDrive              |
| `comm`     | Communication               | `office`   | Office                |
| `cortana`  | Cortana & Search            | `perf`     | Performance           |
| `crash`    | Crash & Diagnostics         | `phone`    | Phone Link            |
| `ctx`      | Context Menu                | `pkg`      | Package Management    |
| `dev`      | Dev Drive / Developer Tools | `power`    | Power                 |
| `display`  | Display                     | `printing` | Printing              |
| `dns`      | DNS & Networking Advanced   | `priv`     | Privacy               |
| `edge`     | Edge                        | `rdp`      | Remote Desktop        |
| `enc`      | Encryption                  | `recovery` | Recovery              |
| `explorer` | Explorer                    | `schtask`  | Scheduled Tasks       |
| `firefox`  | Firefox                     | `scoop`    | Scoop Tools           |
| `font`     | Fonts                       | `sec`      | Security              |
| `fs`       | File System                 | `shell`    | Shell                 |
| `fw`       | Firewall                    |            |                       |
| `game`     | Gaming                      | `snap`     | Snap & Multitasking   |
| `gpu`      | GPU / Graphics              | `speech`   | Voice Access & Speech |
| `idx`      | Indexing & Search           | `ss`       | Screensaver & Lock    |
| `input`    | Input                       | `startup`  | Startup               |
| `java`     | Java                        | `stor`     | Storage               |
| `svc`      | Services                    | `sys`      | System                |
| `tb`       | Taskbar                     | `telem`    | Telemetry Advanced    |
| `term`     | Windows Terminal             | `touch`    | Touch & Pen           |
| `usb`      | USB & Peripherals           | `virt`     | Virtualization        |
| `vnc`      | RealVNC                     | `vscode`   | VS Code               |
| `w11`      | Windows 11                  | `widgets`  | Widgets & News        |
| `wsl`      | WSL                         | `wu`       | Windows Update        |

## Test Infrastructure

- `tests/RegiLattice.Core.Tests/TweakDefTests.cs` — TweakDef model, RegOp factories, scope computation
- `tests/RegiLattice.Core.Tests/TweakEngineTests.cs` — engine registration, lookup, search, profiles, batch ops
- `tests/RegiLattice.Core.Tests/RegistrySessionTests.cs` — session helpers, dry-run, path parsing
- `tests/RegiLattice.Core.Tests/ServicesTests.cs` — Analytics, Config, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings
- `tests/RegiLattice.Core.Tests/PluginTests.cs` — PackLoader, PackManager, PackIndex, TweakEngine pack integration, Locale
- `tests/RegiLattice.CLI.Tests/ParseArgsTests.cs` — CLI argument parsing, flags, options, scope, positional args
- `tests/RegiLattice.GUI.Tests/ThemeTests.cs` — theme switching, colour attributes, all 4 themes, system theme detection
- `tests/RegiLattice.GUI.Tests/PackageManagerValidationTests.cs` — package name validation, tool version checking

## Adding a New Tweak — Checklist

1. Create/edit `src/RegiLattice.Core/Tweaks/<Category>.cs`
2. Define `private const string Key = @"HKEY_...";` at module top
3. Add `TweakDef` to the `Tweaks` list with `ApplyOps`/`RemoveOps`/`DetectOps`
4. Ensure `Id` is **globally unique** kebab-case
5. Register the module in `TweakEngine.RegisterBuiltins()` (one line)
6. Run: `dotnet test`

## Common Pitfalls

- **Duplicate IDs**: `TweakEngine.Register()` will throw on duplicate IDs. Each `Id` must be unique across ALL modules.
- **RegOp paths**: Use full hive names `HKEY_LOCAL_MACHINE\...` or `HKEY_CURRENT_USER\...` (abbreviations `HKLM\...` / `HKCU\...` also accepted).
- **P/Invoke**: Only 2 P/Invoke calls in the entire codebase — `GetComputerNameExW` (CorporateGuard) and `GlobalMemoryStatusEx` (HardwareInfo). Prefer `Microsoft.Win32.Registry` for all registry access.

## File-by-File Quick Ref

| File / Namespace | Purpose | Key Exports |
| --- | --- | --- |
| `TweakEngine.cs` | Central engine | `Register`, `AllTweaks`, `Categories`, `Search`, `Filter`, `Apply`, `StatusMap`, `Profiles`, `Freeze` |
| `TweakDef.cs` | Tweak model | `TweakDef`, `RegOp`, `TweakScope`, `TweakResult`, `RegOpKind` |
| `ProfileDef.cs` | Profile model | `ProfileDef` (Name, Description, ApplyCategories, SkipCategories) |
| `ProfileDefinitions.cs` | 5 profiles | `All` static list |
| `RegistrySession.cs` | Registry wrapper | `SetDword`, `ReadDword`, `Execute`, `Evaluate`, `Backup`, `DryRun` |
| `PackDef.cs` | Pack model | `PackDef` record (Name, Version, Author, TweakCount, Tags, Sha256) |
| `PackLoader.cs` | Pack loader | `LoadFromJson`, `ValidatePackJson`, `ComputeSha256` |
| `PackManager.cs` | Pack manager | `InstallPackAsync`, `UninstallPack`, `InstalledPacks`, `CheckUpdatesAsync` |
| `PackIndex.cs` | Marketplace index | `FromJson`, `ToJson`, `Packs` |
| `CorporateGuard.cs` | Corp detection | `IsCorporateNetwork()`, `Status()`, `IsGpoManaged()` |
| `Elevation.cs` | UAC helpers | `IsAdmin()`, `RequestElevation()` |
| `HardwareInfo.cs` | Hardware detection | `Detect()`, `Summary()`, `SuggestProfile()` |
| `Analytics.cs` | Usage analytics | `RecordApply()`, `RecordRemove()`, `GetStats()` |
| `AppConfig.cs` | Configuration | `Load()`, `ForceCorpGuard`, `Theme`, `Locale` |
| `Locale.cs` | i18n (en, de) | `T()`, `SetLocale()`, `CurrentLocale`, `AvailableLocales` |
| `Ratings.cs` | Rating system | `Rate()`, `GetRating()`, `AllRatings()`, `TopRated()` |
| `Theme.cs` (GUI) | Theme engine | `SetTheme()`, `DetectSystemTheme()`, `AvailableThemes()`, `ThemeDef` record |
| `MainForm.cs` (GUI) | Main window | Category list, search, filters, profiles, tweak operations, tray icon |
| `Program.cs` (CLI) | CLI entry | 25+ commands via args parsing |
