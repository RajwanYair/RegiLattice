---
name: testing
description: "Write, run, or improve xUnit tests for RegiLattice. Use when adding tests for a new feature, increasing coverage, debugging failing tests, or following testing conventions. Triggers on: 'write tests', 'add tests', 'test coverage', 'xUnit', 'failing test', 'test pattern', 'TweakDef test', 'TweakEngine test'."
argument-hint: "What to test (e.g. 'TweakEngine.Search', 'new service class', 'ConfigExporter')"
---

# Testing — RegiLattice

## Test Projects

| Project                         | Tests | Covers                                                                                                                                      |
| ------------------------------- | ----- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| `tests/RegiLattice.Core.Tests/` | 1014 | TweakDef, TweakEngine, RegistrySession, Services, Plugins, Snapshot, Validator, DependencyResolver, Favorites, TweakHistory, ConfigExporter |
| `tests/RegiLattice.CLI.Tests/`  | 175   | CLI argument parsing, ConsoleColorizer                                                                                                      |
| `tests/RegiLattice.GUI.Tests/`  | 225   | Theme, PackageManagerValidation, AppIcons                                                                                                   |

## Naming Convention

```
MethodOrClass_Scenario_ExpectedResult
```

Examples:

- `Search_WithTelemetryQuery_ReturnsPrivacyTweaks`
- `Register_DuplicateId_ThrowsArgumentException`
- `DryRun_SetDword_CapturesOpWithoutWrite`

## Test Skeleton

```csharp
using Xunit;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tests;

public sealed class MyFeatureTests
{
    [Fact]
    public void Method_HappyPath_ReturnsExpected()
    {
        // Arrange
        var engine = new TweakEngine();
        engine.Register(new TweakDef
        {
            Id = "test-tweak",
            Label = "Test Tweak",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "Val", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\Test", "Val")],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\Test", "Val", 1)],
        });

        // Act
        var result = engine.GetTweak("test-tweak");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Tweak", result.Label);
    }
}
```

## Critical Testing Rules

### Registry safety — always DryRun

```csharp
var session = new RegistrySession { DryRun = true };
session.SetDword(@"HKCU\Software\RegiLattice\Test", "X", 42);
Assert.Single(session.DryOps);  // captured, never written
```

### HasOperations gate — tweaks without ApplyOps are skipped

```csharp
// ❌ This tweak will NOT be registered — HasOperations == false
engine.Register(new TweakDef { Id = "t1", Label = "X", Category = "A" });
Assert.Null(engine.GetTweak("t1"));   // silently skipped!

// ✅ Correct — provide at least ApplyOps
engine.Register(new TweakDef { Id = "t1", ...,
    ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)] });
```

### Never create Form instances in tests

```csharp
// ❌ BAD — hangs in headless CI
var form = new MainForm();

// ✅ GOOD — test the data model
var theme = Theme.GetTheme("catppuccin-mocha");
Assert.Equal("Catppuccin Mocha", theme!.Name);
```

### Assert.Contains ambiguity — use explicit array

```csharp
// ❌ BAD — CS0121 ambiguous (HashSet vs SortedSet overload)
Assert.Contains(profile, ["business", "gaming"]);

// ✅ GOOD
Assert.Contains(profile, new[] { "business", "gaming" });
```

## Running Tests

```powershell
# All tests
dotnet test RegiLattice.sln --settings tests/.runsettings --blame-hang-timeout 60s

# Single project
dotnet test tests/RegiLattice.Core.Tests --no-build --verbosity normal

# By name filter
dotnet test --no-build --filter "FullyQualifiedName~TweakEngine"

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Coverage Targets

| Component         | Target | Notes                      |
| ----------------- | ------ | -------------------------- |
| TweakDef model    | 100%   | Pure data                  |
| TweakEngine       | 90%+   | Core business logic        |
| RegistrySession   | 80%+   | Use DryRun for write paths |
| Services          | 85%+   | Mock P/Invoke/WMI          |
| GUI theme records | 90%+   | Pure data                  |
| GUI Forms         | 60%+   | UI hard to unit test       |

## Intentionally Untested (external tools required)

- `ChocolateyManager`, `PipManager`, `WinGetManager` — require installed tool
- `ShellRunner` — executes real processes
- `PackManager` async methods — require network
- `CorporateGuard` WMI paths — environment-dependent
