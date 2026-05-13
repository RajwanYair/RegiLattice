---
name: testing
description: "Write, run, or improve xUnit tests for RegiLattice. Use when adding tests for a new feature, increasing coverage, debugging failing tests, or following testing conventions. Triggers on: 'write tests', 'add tests', 'test coverage', 'xUnit', 'failing test', 'test pattern', 'TweakDef test', 'TweakEngine test'."
argument-hint: "What to test (e.g. 'TweakEngine.Search', 'new service class', 'ConfigExporter')"
---

# Testing — RegiLattice

## Test Projects

| Project                         | Tests | Covers                                                                                                                                      |
| ------------------------------- | ----- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| `tests/RegiLattice.Core.Tests/` | 2,507+ | TweakDef, TweakEngine, RegistrySession, Services, Plugins, Snapshot, Validator, DependencyResolver, Favorites, TweakHistory, ConfigExporter |
| `tests/RegiLattice.CLI.Tests/`  | 434+   | CLI argument parsing, ConsoleColorizer                                                                                                      |
| `tests/RegiLattice.GUI.Tests/`  | 363+   | Theme, PackageManagerValidation, AppIcons                                                                                                   |

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

### Builtins-heavy tests — share `BuiltinsFixture`

```csharp
[Collection("Builtins")]
public sealed class TweakEngineBuiltinsTests
{
    private readonly TweakEngine _engine;

    public TweakEngineBuiltinsTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }
}
```

Use the shared builtins fixture for tests that only read from `RegisterBuiltins()` data. Rebuilding the full tweak catalog per test class is unnecessary unless the test explicitly validates registration.

### Nullable return guards — `ParseArgs()` first needs `Assert.NotNull()`

```csharp
var args = Program.ParseArgs(["--list"]);
Assert.NotNull(args);
Assert.True(args.ShowList);
```

## Running Tests

```powershell
# All tests (run per project to avoid cross-assembly races)
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s
dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s

# Single project
dotnet test tests/RegiLattice.Core.Tests --no-build --verbosity normal

# By name filter
dotnet test --no-build --filter "FullyQualifiedName~TweakEngine"

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Coverage Targets

| Component         | Minimum  | Preferred | Notes                      |
| ----------------- | -------- | --------- | -------------------------- |
| TweakDef model    | **100%** | **100%**  | Pure data                  |
| TweakEngine       | **90%+** | **95%+**  | Core business logic        |
| RegistrySession   | **85%+** | **92%+**  | Use DryRun for write paths |
| Services          | **90%+** | **95%+**  | All deterministic logic    |
| GUI theme records | **90%+** | **95%+**  | Pure data                  |
| GUI Forms         | **60%+** | **75%+**  | UI hard to unit test       |
| **Overall Core**  | **≥90%** | **≥95%**  | **Codecov gate enforced — PR below threshold is blocked** |

## Quality Gate — Non-Negotiable

Every test run must satisfy ALL conditions before committing:

| Gate | Requirement |
|------|-------------|
| Build fatals | **0** — hard CI fail |
| Build warnings | **0** — `TreatWarningsAsErrors=true`; any warning is a build error |
| Build errors | **0** — hard fail |
| Test failures | **0** — all 3,304+ tests must pass |
| Skipped tests | **0** — `[Fact(Skip=...)]` / `[Theory(Skip=...)]` are **FORBIDDEN** |
| Inline suppressions | **0** — `#pragma warning disable` / `[SuppressMessage]` / `// NOSONAR` / `// NCA` / `// NOLINT` in test code **FORBIDDEN** |
| Waivers / lint ignores | **0** — `// csharpier-ignore`, `// coverage: ignore`, `// HACK:` etc. equally forbidden |
| TODO / FIXME in tests | **0** — open a GitHub Issue instead |
| Line coverage (Core) | **≥90%** minimum — **≥95% preferred** — Codecov enforced; drop below gate = block |

```powershell
# Verify quality gate locally before every commit:
dotnet build RegiLattice.sln -c Release -m:1   # must print: 0 Error(s), 0 Warning(s)
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 30s
dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj   --settings tests/.runsettings --blame-hang-timeout 30s
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj   --settings tests/.runsettings --blame-hang-timeout 30s
```

## Flaky Test Prevention

### Timestamp stripping
```csharp
// ❌ BAD — DateTime.Now called twice can straddle a second
Assert.Equal(gen.Build(map), File.ReadAllText(path));

// ✅ GOOD
static string StripTs(string s) =>
    Regex.Replace(s, @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}", "TIMESTAMP");
Assert.Equal(StripTs(gen.Build(map)), StripTs(File.ReadAllText(path)));
```

### Performance budget comments
```csharp
// ✅ Required — document tweak count + expected baseline so future changes are deliberate
// Budget: 150ms — ~7,000 tweaks with synonym expansion; ~60ms on dev machine.
Assert.True(sw.ElapsedMilliseconds < 150, $"Search took {sw.ElapsedMilliseconds}ms");
```

### CorporateGuard isolation
```csharp
public sealed class MyFixture : IDisposable
{
    public MyFixture() => CorporateGuard.StubCorporate = false; // bypass Intune WMI
    public void Dispose() => CorporateGuard.StubCorporate = null;
}
```

## Intentionally Untested (external tools required)

- `ChocolateyManager`, `PipManager`, `WinGetManager` — require installed tool
- `ShellRunner` — executes real processes
- `PackManager` async methods — require network
- `CorporateGuard` WMI paths — environment-dependent
