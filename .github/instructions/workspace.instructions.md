---
applyTo: "**"
---

# GitHub Copilot Workspace Instructions

## Project Overview

Windows-only .NET development workspace for the **RegiLattice** registry tweak toolkit.
Version 3.4.0 — C# 13 / .NET 10.0-windows, migrated from Python v1.x.

## Technical Stack

- **Language**: C# 13 / .NET 10.0-windows (x64)
- **Build**: `dotnet build` via MSBuild / `RegiLattice.sln`
- **GUI Framework**: WinForms with 4 themes (Catppuccin Mocha/Latte, Nord, Dracula)
- **CLI**: Args-based command parsing (25+ commands)
- **Testing**: xUnit 2.9.2 + coverlet 6.0.2
- **NuGet**: System.Management 9.0.3, Microsoft.NET.Test.Sdk 17.11.1
- **Registry**: `Microsoft.Win32.Registry` via `RegistrySession` wrapper
- **P/Invoke**: Only 2 calls (GetComputerNameExW, GlobalMemoryStatusEx)

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
│   │   │                        #   TweakHistory, WinGetManager
│   │   ├── Plugins/             # Tweak Pack system (JSON marketplace)
│   │   └── Tweaks/              # 90 category modules, ~2,410 tweaks
│   ├── RegiLattice.GUI/         # WinForms application
│   │   ├── Program.cs           # Entry point
│   │   ├── AppIcons.cs          # Programmatic icon/bitmap generation
│   │   ├── Theme.cs             # 4-theme engine
│   │   ├── Forms/               # MainForm, AboutDialog, ChocolateyManagerDialog,
│   │   │                        #   MarketplaceDialog, PipManagerDialog, PSModuleManagerDialog,
│   │   │                        #   ScoopManagerDialog, ToolVersionsDialog, WindowsHealthDialog,
│   │   │                        #   WinGetManagerDialog
│   │   └── PackageManagers/     # GUI-side package manager wrappers
│   │                            #   PackageNameValidator, ShellRunner, ScoopManager, PipManager,
│   │                            #   PSModuleManager, ChocolateyManager, WinGetManager,
│   │                            #   ToolVersionChecker, WindowsHealthManager
│   └── RegiLattice.CLI/         # Console application
│       ├── Program.cs           # 25+ commands via args parsing
│       ├── CliArgs.cs           # CLI argument model
│       └── ConsoleColorizer.cs  # ANSI terminal colour helpers
├── tests/
│   ├── RegiLattice.Core.Tests/  # 779 xUnit tests
│   ├── RegiLattice.CLI.Tests/   # 111 xUnit tests
│   └── RegiLattice.GUI.Tests/   # 131 xUnit tests
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

| Kind | Typical Pattern | Example Category | TweakDef Fields Used |
|------|-----------------|------------------|----------------------|
| `Registry` | RegOps on HKCU/HKLM | Privacy, Performance | `ApplyOps`, `RemoveOps`, `DetectOps` |
| `PowerShell` | PSH cmdlet/script block | PowerShell Tweaks | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `SystemCommand` | bcdedit, dism, netsh | Boot, Network Opt. | `ApplyAction`, `RemoveAction` |
| `ServiceControl` | sc.exe, Set-Service | Services | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `ScheduledTask` | schtasks cmd | Scheduled Tasks | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `FileConfig` | JSON, INI, .wslconfig | WSL, Win Terminal | `ApplyAction`, `RemoveAction`, `DetectAction` |
| `GroupPolicy` | HKLM\...\Policies\... | Security, Hardening | `ApplyOps` with policy paths |
| `PackageManager` | scoop, pip, winget | Package Management | `ApplyAction`, `UpdateAction`, `DetectAction` |

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

```powershell
# Build
dotnet build RegiLattice.sln

# Test
dotnet test

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
- **P/Invoke minimized** — only 2 calls in entire codebase, both documented
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
