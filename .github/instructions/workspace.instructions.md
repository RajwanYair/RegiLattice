---
applyTo: "**"
---

# GitHub Copilot Workspace Instructions

## Project Overview

Windows-only .NET development workspace for the **RegiLattice** registry tweak toolkit.
Version 3.0.0 — C# 13 / .NET 10.0-windows, migrated from Python v1.x.

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
│   │   ├── Models/              # TweakDef, ProfileDef, ProfileDefinitions
│   │   ├── Registry/            # RegistrySession wrapper
│   │   ├── Services/            # Analytics, AppConfig, CorporateGuard, Elevation,
│   │   │                        #   HardwareInfo, Locale, Ratings
│   │   └── Tweaks/              # 71 category modules, ~1,981 tweaks
│   ├── RegiLattice.GUI/         # WinForms application
│   │   ├── Program.cs           # Entry point
│   │   ├── Theme.cs             # 4-theme engine
│   │   └── Forms/               # MainForm, AboutDialog, package manager dialogs
│   └── RegiLattice.CLI/         # Console application
│       └── Program.cs           # 25+ commands via args parsing
├── tests/
│   ├── RegiLattice.Core.Tests/  # 112 xUnit tests
│   ├── RegiLattice.CLI.Tests/   # 52 xUnit tests
│   └── RegiLattice.GUI.Tests/   # 39 xUnit tests
└── archive/python/              # Archived Python v1.x codebase
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

- `TweakDef` — sealed class with `required` + `init` properties
- `RegOp` — immutable with 12 factory methods
- `ProfileDef` — record type
- `ThemeDef` — record type

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
