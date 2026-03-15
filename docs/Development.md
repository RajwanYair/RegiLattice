# RegiLattice — Development Guide

> Local setup, workflow, testing, and contribution instructions for the C# codebase.
> Last updated: 2025-07-22 · v3.2.0

---

## Prerequisites

| Requirement | Version | Notes |
|---|---|---|
| .NET SDK | 10.0+ | Required. Download from <https://dot.net> |
| Git | 2.40+ | For version control |
| Windows | 10/11 (build 19041+) | Required for registry operations and WinForms GUI |
| VS Code | 1.85+ | Recommended editor with C# Dev Kit extension |

---

## Quick Start (Windows)

```powershell
# Clone the repository
git clone https://github.com/RajwanYair/RegiLattice.git
cd RegiLattice

# Restore NuGet packages and build
dotnet build RegiLattice.sln

# Run all tests
dotnet test RegiLattice.sln --logger "console;verbosity=normal"

# Run the CLI
dotnet run --project src/RegiLattice.CLI -- --list | Select-Object -First 10

# Run the GUI
dotnet run --project src/RegiLattice.GUI
```

---

## Solution Structure

```
RegiLattice.sln
├── src/RegiLattice.Core/      # Class library — engine, models, registry, services, plugins
├── src/RegiLattice.GUI/       # WinForms application (4 themes)
├── src/RegiLattice.CLI/       # Console application (25+ commands)
├── tests/RegiLattice.Core.Tests/   # 514 xUnit tests
├── tests/RegiLattice.CLI.Tests/    # 56 xUnit tests
└── tests/RegiLattice.GUI.Tests/    # 71 xUnit tests
```

---

## NuGet Packages

| Package | Version | Project |
|---|---|---|
| System.Management | 9.0.3 | Core (WMI queries) |
| xunit | 2.9.2 | Tests |
| xunit.runner.visualstudio | 2.8.2 | Tests |
| Microsoft.NET.Test.Sdk | 17.11.1 | Tests |
| coverlet.collector | 6.0.2 | Tests (coverage) |

---

## Common Development Tasks

| Task | Command |
|---|---|
| Build (Debug) | `dotnet build RegiLattice.sln` |
| Build (Release) | `dotnet build RegiLattice.sln -c Release` |
| Run tests | `dotnet test RegiLattice.sln` |
| Run tests (verbose) | `dotnet test --logger "console;verbosity=normal"` |
| Run CLI | `dotnet run --project src/RegiLattice.CLI -- <args>` |
| Run GUI | `dotnet run --project src/RegiLattice.GUI` |
| Publish (self-contained) | `dotnet publish src/RegiLattice.CLI -c Release -r win-x64 --self-contained` |
| Clean | `dotnet clean RegiLattice.sln` |

VS Code tasks (`Ctrl+Shift+B`) provide IDE integration for all commands above.
See `.vscode/tasks.json`.

---

## Adding a New Tweak

1. Create or edit `src/RegiLattice.Core/Tweaks/<Category>.cs`
2. Define registry key constants: `private const string Key = @"HKEY_...";`
3. Add a `TweakDef` to the `Tweaks` list using `RegOp` factories:

```csharp
new TweakDef
{
    Id = "cat-slug-tweak-name",
    Label = "Human-Readable Name",
    Category = "Category Name",
    Description = "What this tweak does",
    Tags = ["performance", "privacy"],
    NeedsAdmin = true,
    CorpSafe = false,
    RegistryKeys = [Key],
    ApplyOps = [RegOp.SetDword(Key, "ValueName", 1)],
    RemoveOps = [RegOp.DeleteValue(Key, "ValueName")],
    DetectOps = [RegOp.CheckDword(Key, "ValueName", 1)],
}
```

1. Ensure the `Id` is globally unique (kebab-case)
1. Register the module in `TweakEngine.RegisterBuiltins()` (one line)
1. Run: `dotnet test`

---

## Testing

Run the full test suite:

```powershell
dotnet test RegiLattice.sln --logger "console;verbosity=normal"
```

Run a specific test project:

```powershell
dotnet test tests/RegiLattice.Core.Tests/
dotnet test tests/RegiLattice.GUI.Tests/
```

Run a specific test class:

```powershell
dotnet test --filter "FullyQualifiedName~TweakEngineTests"
```

Collect coverage:

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

---

## Code Style

- **C# 13** features preferred (collection expressions, primary constructors, etc.)
- **File-scoped namespaces** (`namespace X;`)
- **`sealed`** on classes that aren't inherited
- **`required`** on mandatory init properties
- **`IReadOnlyList<T>`** for public collection properties
- **No `var`** for non-obvious types in public APIs
- Follow existing patterns in the codebase

---

## Git Workflow

- Commit per logical phase during a session
- Push only at end of chat session
- Conventional Commits: `type(scope): description`
- See `.github/instructions/git-workflow.instructions.md` for full details
