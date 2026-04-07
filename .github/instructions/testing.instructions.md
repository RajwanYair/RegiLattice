---
applyTo: "**/tests/**,**/*Tests/**,**/*Tests.csproj,**/test_*.py,**/conftest.py"
---

# Testing Instructions

## Framework & Tools

- **Primary**: xUnit 2.9.3 (C# test projects)
- **Runner**: Microsoft.NET.Test.Sdk 17.14.1
- **Coverage**: coverlet.collector 6.0.4 + ReportGenerator
- **Legacy Python**: pytest 8.0+ (for archived Python tests only)

## Test Projects

| Project                  | Tests     | Covers                                                                                                                                                                         |
| ------------------------ | --------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `RegiLattice.Core.Tests` | 2,434+    | TweakDef, TweakEngine, RegistrySession, Services, Plugins, Locale, SnapshotManager, TweakValidator, DependencyResolver, Favorites, TweakHistory, ConfigExporter, SystemMonitor, BatchImpactEstimator |
| `RegiLattice.CLI.Tests`  | 434+      | CLI argument parsing (ParseArgs, CliArgs, ConsoleColorizer)                                                                                                                    |
| `RegiLattice.GUI.Tests`  | 363+      | Theme, PackageManagerValidation, PackageNameValidator, AppIcons                                                                                                                |
| **Total**                | **3,230+**|                                                                                                                                                                                |

## Running Tests

```powershell
# All tests
dotnet test

# Specific project
dotnet test tests/RegiLattice.Core.Tests

# With console output
dotnet test --logger "console;verbosity=normal"

# Filter by test name
dotnet test --filter "FullyQualifiedName~TweakDef"

# Filter by trait/category
dotnet test --filter "Category=Unit"

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

**In VS Code / GitHub Copilot**: Use the `runTests` tool or VS Code Testing panel.
Do not run `dotnet test` in a Copilot agent terminal — output may be truncated.

## Test File Structure

```
tests/
├── RegiLattice.Core.Tests/
│   ├── TweakDefTests.cs         # TweakDef model, RegOp factories, scope computation
│   ├── TweakEngineTests.cs      # Engine registration, lookup, search, profiles, batch
│   ├── TweakEngineBuiltinsTests.cs # RegisterBuiltins integration, ID uniqueness, profiles, categories, search/filter
│   ├── RegistrySessionTests.cs  # Session helpers, dry-run, path parsing, read/write ops
│   ├── ServicesTests.cs         # Analytics, Config, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings
│   ├── PluginTests.cs           # PackLoader, PackManager, PackIndex, TweakEngine pack integration, Locale
│   ├── SnapshotManagerTests.cs  # Save/Load/Restore, round-trip, edge cases
│   ├── TweakValidatorTests.cs   # Valid tweaks, empty fields, duplicates, circular deps, broken deps
│   ├── DependencyResolverTests.cs # Resolve topological sort, Dependents reverse lookup
│   ├── FavoritesTests.cs        # Add, Remove, Toggle, IsFavorite, case-insensitive, Flush/Reload
│   ├── TweakHistoryTests.cs     # Record apply/remove/update, Recent, ForTweak, MaxEntries, Flush/Reload
│   └── ConfigExporterTests.cs   # Export JSON, Import 3 formats, Validate, RoundTrip
├── RegiLattice.CLI.Tests/
│   └── ParseArgsTests.cs        # CLI argument parsing, flags, options, scope, positional args
└── RegiLattice.GUI.Tests/
    ├── ThemeTests.cs             # Theme switching, colour attributes, all 4 themes, system theme detection
    ├── PackageManagerValidationTests.cs  # Package name validation, tool version checking
    └── AppIconsTests.cs          # AppIcons bitmap/icon validity, cache invalidation safety
```

## Naming Convention

```csharp
// Pattern: MethodName_Scenario_ExpectedResult
[Fact]
public void GetTweak_WithValidId_ReturnsTweakDef() { ... }

[Fact]
public void GetTweak_WithInvalidId_ReturnsNull() { ... }

[Fact]
public void Register_DuplicateId_ThrowsArgumentException() { ... }

// Theory for parameterized tests
[Theory]
[InlineData("perf-disable-animations", "Performance")]
[InlineData("priv-disable-telemetry", "Privacy")]
public void TweakDef_Category_MatchesExpected(string id, string expectedCat) { ... }
```

## Test Patterns

### Arrange-Act-Assert

```csharp
[Fact]
public void Search_WithQueryTerm_ReturnsMatchingTweaks()
{
    // Arrange
    var engine = new TweakEngine();
    engine.RegisterBuiltins();

    // Act
    var results = engine.Search("telemetry");

    // Assert
    Assert.NotEmpty(results);
    Assert.All(results, t =>
        Assert.True(
            t.Label.Contains("telemetry", StringComparison.OrdinalIgnoreCase) ||
            t.Description.Contains("telemetry", StringComparison.OrdinalIgnoreCase) ||
            t.Tags.Any(tag => tag.Contains("telemetry", StringComparison.OrdinalIgnoreCase))
        ));
}
```

### Testing Exceptions

```csharp
[Fact]
public void Register_NullTweak_ThrowsArgumentNullException()
{
    var engine = new TweakEngine();
    Assert.Throws<ArgumentNullException>(() => engine.Register(null!));
}
```

### Testing Collections

```csharp
[Fact]
public void Categories_ReturnsAllCategories()
{
    var engine = new TweakEngine();
    engine.RegisterBuiltins();

    var categories = engine.Categories();

    // Actual count varies as modules are added; verify it's substantial
    Assert.True(categories.Count > 600);
    Assert.Contains("Privacy", categories);
    Assert.Contains("Performance", categories);
}
```

### Testing RegOp Factories

```csharp
[Fact]
public void SetDword_CreatesCorrectRegOp()
{
    var op = RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Value", 1);

    Assert.Equal(RegOpKind.SetDword, op.Kind);
    Assert.Equal(@"HKEY_CURRENT_USER\Software\Test", op.Path);
    Assert.Equal("Value", op.Name);
    Assert.Equal(1, op.DwordValue);
}
```

### RegistrySession with DryRun

```csharp
[Fact]
public void DryRun_DoesNotWriteToRegistry()
{
    var session = new RegistrySession { DryRun = true };
    session.SetDword(@"HKEY_CURRENT_USER\Software\RegiLattice\Test", "Dry", 42);

    Assert.NotEmpty(session.DryOps);
    Assert.Contains(session.DryOps, op => op.Name == "Dry");
}
```

## Coverage

### Collecting Coverage

```powershell
# Collect coverage (generates Cobertura XML)
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"htmlcov" -reporttypes:Html
```

### Coverage Targets

> **Target: ≥90% line coverage on all testable components. Prefer higher. Branch coverage at 60%+.**
> Any PR that drops line coverage below 90% on `RegiLattice.Core.Tests` must be accompanied by a
> documented justification. Coverage regressions from the baseline are treated as build failures.

| Component          | Target    | Notes                                                        |
| ------------------ | --------- | ------------------------------------------------------------ |
| TweakDef model     | **100%**  | Pure logic, fully testable with declarative assertions       |
| TweakEngine        | **90%+**  | Core business logic — all public API paths must be tested   |
| RegistrySession    | **85%+**  | DryRun mode enables safe testing of all write/read paths     |
| Services           | **90%+**  | All services with deterministic logic must have full tests   |
| GUI (Theme)        | **90%+**  | Theme records are pure data with no external dependencies    |
| GUI (Forms)        | **60%+**  | WinForms UI event handling is hard to unit test              |
| **Overall (Core)** | **≥90%** | **Line coverage gate enforced in CI via Codecov threshold**  |

### Coverage by TweakKind

Tweak modules are grouped by `TweakKind`. Coverage patterns differ by kind:

| TweakKind        | Testable?        | Coverage Strategy                                                                              |
| ---------------- | ---------------- | ---------------------------------------------------------------------------------------------- |
| `Registry`       | ✅ Yes (95%+)    | Pure RegOp declarations — tested via `RegisterBuiltins()` + tweak count/ID/category assertions |
| `PowerShell`     | ⚠️ Partial (69%) | `ApplyAction`/`DetectAction` delegates call PSH — test registration + structure only           |
| `SystemCommand`  | ⚠️ Partial (62%) | `ApplyAction` runs bcdedit/dism/netsh — test registration only, no execution in CI             |
| `ServiceControl` | ⚠️ Partial (52%) | `ApplyAction` runs sc.exe — test tweak metadata only                                           |
| `ScheduledTask`  | ⚠️ Partial (52%) | `ApplyAction` runs schtasks — test tweak metadata only                                         |
| `FileConfig`     | ⚠️ Partial       | Writes to .wslconfig / JSON — test registration + structure                                    |
| `GroupPolicy`    | ✅ Yes (95%+)    | Uses RegOps on `Policies\` paths — same as Registry kind                                       |
| `PackageManager` | ❌ No (0%)       | Requires external tools (scoop, pip, winget) — skip in CI                                      |

### Intentionally Untested Components

These components require external tools, network, or system state that cannot be safely mocked in CI:

- `ChocolateyManager`, `PipManager`, `WinGetManager` — require installed package managers
- `ShellRunner` — executes real processes
- `PackManager` async methods — require network + filesystem
- `CorporateGuard` P/Invoke paths — environment-dependent (26% from WMI/registry paths)

## Build Quality Gate — Non-Negotiable

Every test run and every CI build must meet these conditions before merging:

| Gate                   | Requirement                                                          |
| ---------------------- | -------------------------------------------------------------------- |
| Build warnings         | **0** — `TreatWarningsAsErrors=true`; any warning blocks CI            |
| Build errors           | **0** — hard fail                                                     |
| Test failures          | **0** — all 3,230+ tests must pass                                    |
| Skipped tests          | **0** — `[Fact(Skip=...)]` and `[Theory(Skip=...)]` are forbidden       |
| Inline suppressions    | **0** — `#pragma warning disable` / `[SuppressMessage]` are forbidden  |
| TODO / FIXME in tests  | **0** — open a GitHub Issue instead                                   |
| Line coverage (Core)   | **≥90%** — Codecov enforced; PR dropped below gate = block           |

```powershell
# Verify the full quality gate locally before every commit:
dotnet build RegiLattice.sln -c Release   # must print 0 Error(s), 0 Warning(s)
dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj --settings tests/.runsettings --blame-hang-timeout 60s
dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj  --settings tests/.runsettings --blame-hang-timeout 60s
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj  --settings tests/.runsettings --blame-hang-timeout 60s
```

## What NOT to Do in Tests

- Don't test implementation details — test behaviour
- Don't use `Thread.Sleep()` — use `Task.Delay` with async tests or mocks
- Don't leave temporary files — use `Path.GetTempPath()` with cleanup
- Don't catch exceptions to silence test failures
- Don't test private methods directly — test through public API
- Don't create real registry keys in tests — use `DryRun = true` on RegistrySession
- Don't create actual WinForms windows in CI — test data models and logic only
- Don't use `Assert.True(condition)` when a specific assertion exists (e.g., `Assert.Equal`, `Assert.Contains`)
- Don't use `[Fact(Skip=...)]` or `[Theory(Skip=...)]` — fix the test or the code it tests; skips are forbidden
- Don't use `#pragma warning disable` in test code — all warnings must be fixed
- Don't use inline assertion workarounds (`Assert.Equal(expected, actual)` inverted) to hide test logic errors
