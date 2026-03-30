---
applyTo: "**"
---

# GitHub Copilot Workspace Instructions

## Project Overview

Windows-only .NET development workspace for the **RegiLattice** registry tweak toolkit.
Version 5.95.0 — C# 13 / .NET 10.0-windows, migrated from Python v1.x.

## Technical Stack

- **Language**: C# 13 / .NET 10.0-windows (x64)
- **Build**: `dotnet build` via MSBuild / `RegiLattice.sln`
- **GUI Framework**: WinForms with 11 themes (Catppuccin Mocha/Latte, Nord, Dracula + 7 more)
- **CLI**: Args-based command parsing (25+ commands)
- **Testing**: xUnit 2.9.3 + coverlet 6.0.4
- **NuGet**: System.Management 10.0.5, Microsoft.NET.Test.Sdk 17.14.1
- **Registry**: `Microsoft.Win32.Registry` via `RegistrySession` wrapper
- **P/Invoke**: Only 4 calls (GetComputerNameExW, GlobalMemoryStatusEx, GetSystemTimes, GlobalMemoryStatusEx×2)

## Solution Structure

```
RegiLattice.sln
├── src/
│   ├── RegiLattice.Core/        # Class library — engine, models, registry, services
│   │   ├── TweakEngine.cs       # Central tweak manager
│   │   ├── SnapshotManager.cs   # Save/load/restore tweak state snapshots
│   │   ├── TweakValidator.cs    # Tweak integrity validation & circular dep detection
│   │   ├── DependencyResolver.cs # Topological dependency resolution
│   │   ├── Models/              # TweakDef, ProfileDef, ProfileDefinitions
│   │   ├── Registry/            # RegistrySession wrapper
│   │   ├── Services/            # Analytics, AppConfig, ChocolateyManager, ConfigExporter,
│   │   │                        #   CorporateGuard, Elevation, Favorites, HardwareInfo,
│   │   │                        #   Locale, PipManager, Ratings, ShellRunner,
│   │   │                        #   SystemMonitor, TweakHistory, WinGetManager
│   │   ├── Plugins/             # Tweak Pack system (JSON marketplace)
│   │   └── Tweaks/              # 665 module files, 9,190 tweaks across 637 categories
│   ├── RegiLattice.GUI/         # WinForms application
│   │   ├── Program.cs           # Entry point
│   │   ├── AppIcons.cs          # Programmatic icon/bitmap generation
│   │   ├── Theme.cs             # 11-theme engine
│   │   ├── Forms/               # MainForm, AboutDialog, BasePackageManagerDialog,
│   │   │                        #   ChocolateyManagerDialog, MarketplaceDialog,
│   │   │                        #   PipManagerDialog, PSModuleManagerDialog,
│   │   │                        #   ScoopManagerDialog, ToolVersionsDialog,
│   │   │                        #   WindowsHealthDialog, WinGetManagerDialog
│   │   └── PackageManagers/     # GUI-side package manager wrappers
│   │                            #   PackageNameValidator, ShellRunner, ScoopManager, PipManager,
│   │                            #   PSModuleManager, ChocolateyManager, WinGetManager,
│   │                            #   ToolVersionChecker, WindowsHealthManager
│   └── RegiLattice.CLI/         # Console application
│       ├── Program.cs           # 25+ commands via args parsing
│       ├── CliArgs.cs           # CLI argument model
│       └── ConsoleColorizer.cs  # ANSI terminal colour helpers
├── tests/
│   ├── RegiLattice.Core.Tests/  # 2,301+ xUnit tests
│   ├── RegiLattice.CLI.Tests/   # 301+ xUnit tests
│   └── RegiLattice.GUI.Tests/   # 339+ xUnit tests
├── .tmp/                        # Intermediate dev files (gitignored)
└── archive/                     # Archived (untracked)
```

## Core Architecture Patterns

### Declarative Tweak Definition (95% of tweaks)

```csharp
new TweakDef
{
    Id = "priv-disable-telemetry",
    Label = "Disable Telemetry",
    Category = "Privacy",
    Tags = ["telemetry", "privacy"],
    NeedsAdmin = true,
    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "AllowTelemetry", 0)],
    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "AllowTelemetry")],
    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\...", "AllowTelemetry", 0)],
}
```

### Immutable Models

- `TweakDef` — sealed class with `required` + `init` properties (see copilot-instructions.md for full field list)
- `RegOp` — immutable with 12 factory methods
- `ProfileDef` — record type
- `ThemeDef` — record type

### TweakKind — How Tweaks Operate (8 variants)

Auto-detected from category/registry paths, or explicitly set via `KindHint`:

| Kind             | Typical Pattern         | Example Category     | TweakDef Fields Used                          |
| ---------------- | ----------------------- | -------------------- | --------------------------------------------- |
| `Registry`       | RegOps on HKCU/HKLM     | Privacy, Performance | `ApplyOps`, `RemoveOps`, `DetectOps`          |
| `PowerShell`     | PSH cmdlet/script block | PowerShell Tweaks    | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `SystemCommand`  | bcdedit, dism, netsh    | Boot, Network Opt.   | `ApplyAction`, `RemoveAction`                 |
| `ServiceControl` | sc.exe, Set-Service     | Services             | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `ScheduledTask`  | schtasks cmd            | Scheduled Tasks      | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `FileConfig`     | JSON, INI, .wslconfig   | WSL, Win Terminal    | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `GroupPolicy`    | HKLM\...\Policies\...   | Security, Hardening  | `ApplyOps` with policy paths                  |
| `PackageManager` | scoop, pip, winget      | Package Management   | `ApplyAction`, `UpdateAction`, `DetectAction` |

### Registry Access via RegistrySession

All registry operations go through `RegistrySession` which provides:

- DryRun mode (no actual writes)
- JSON backup before destructive operations
- Structured logging
- Execute/Evaluate for RegOp lists

### Configuration Hierarchy (highest → lowest priority)

1. Command-line arguments
2. Environment variables
3. User config file (`%LOCALAPPDATA%\RegiLattice\config.json`)
4. Default values

## Coding Standards

### Sealed by Default

All classes should be `sealed` unless inheritance is explicitly needed.

### Nullable Reference Types

`#nullable enable` everywhere. All nullable returns must be annotated with `?`.

### IReadOnlyList for Collections

Expose `IReadOnlyList<T>` (not `List<T>`) for all public collection properties.

### Error Handling

- Specific exception types (never bare `catch`)
- Meaningful messages with context
- `UnauthorizedAccessException` for registry permission errors
- `ArgumentException` for invalid tweak IDs

## Build & Test Commands

Build and package configuration is centralized in:

- `Directory.Build.props`
- `Directory.Packages.props`

```powershell
# Build
dotnet build RegiLattice.sln

# Test
dotnet test RegiLattice.sln --settings tests/.runsettings --blame-hang-timeout 60s

# Run GUI
dotnet run --project src/RegiLattice.GUI

# Run CLI
dotnet run --project src/RegiLattice.CLI -- --list

# Publish
dotnet publish src/RegiLattice.GUI/RegiLattice.GUI.csproj -c Release -r win-x64 --self-contained true
```

## Security Guidelines

- **No hardcoded credentials** — use environment variables or `%LOCALAPPDATA%`
- **No `Process.Start` with user input** — parameterize all commands
- **Registry access via RegistrySession** — never raw `Registry.SetValue`
- **P/Invoke minimized** — only 4 calls in entire codebase (GetComputerNameExW, GlobalMemoryStatusEx ×2, GetSystemTimes), all documented
- **CorporateGuard** — detects managed environments and blocks unsafe tweaks
- **DryRun mode** — preview changes before applying

## What NOT to Do

- Don't hardcode absolute paths — use `Environment.GetFolderPath`
- Don't use `Process.Start` with unsanitized arguments
- Don't leave `Console.WriteLine` in library code
- Don't use mutable public fields — use properties with `init` or `private set`
- Don't use `dynamic` type — use generics or pattern matching
- Don't use `Thread` directly — use `Task.Run` or async/await
- Don't skip tests for "simple" code
- Don't commit debug code or temp files

## Intermediate / Temporary Files

Place all development intermediate files (generated reports, profiling output, scratch
scripts, coverage HTML, etc.) in the `.tmp/` directory at the project root. This
directory is gitignored and will never be tracked. Do NOT place these in root or `docs/`.

```powershell
# Example: export coverage report to .tmp/
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:".tmp/htmlcov" -reporttypes:Html
```

## Package Manager Dialog Architecture

All 5 package manager dialogs extend `BasePackageManagerDialog` (template method pattern):

```
BasePackageManagerDialog (abstract)
├── ChocolateyManagerDialog
├── ScoopManagerDialog
├── PipManagerDialog         (+BuildScopePanel override)
├── WinGetManagerDialog      (+AddExtraButtons override for Search)
└── PSModuleManagerDialog    (+BuildScopePanel override, UpgradeText="Update")
```

The base class provides:

- `SplitContainer` with resizable pane (top: ListView + buttons, bottom: RichTextBox log)
- Prereq banner with async install flow
- `AppendLog()`, `SetBusy()`, `SetStatus()`, `SetOutdated()`, `RebuildQuickInstallButtons()`
- Shared async wrappers: `RefreshAsync()`, `InstallAsync()`, `RemoveAsync()`, `UpgradeAsync()`

To add a new manager dialog, extend `BasePackageManagerDialog` and override the abstract
properties and methods. See `.github/skills/package-managers.skill.md` for full details.
