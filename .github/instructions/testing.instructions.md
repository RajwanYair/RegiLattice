---
applyTo: "**/tests/**,**/*Tests/**,**/*Tests.csproj,**/test_*.py,**/conftest.py"
---

# Testing Instructions

## Framework & Tools

- **Primary**: xUnit 2.9.2 (C# test projects)
- **Runner**: Microsoft.NET.Test.Sdk 17.11.1
- **Coverage**: coverlet.collector 6.0.2 + ReportGenerator
- **Legacy Python**: pytest 8.0+ (for archived Python tests only)

## Test Projects

| Project | Tests | Covers |
|---------|-------|--------|
| `RegiLattice.Core.Tests` | 93 | TweakDef, TweakEngine, RegistrySession, Services |
| `RegiLattice.GUI.Tests` | 36 | Theme, PackageManagerValidation |
| **Total** | **129** | |

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
│   ├── RegistrySessionTests.cs  # Session helpers, dry-run, path parsing
│   └── ServicesTests.cs         # Analytics, Config, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings
└── RegiLattice.GUI.Tests/
    ├── ThemeTests.cs             # Theme switching, colour attributes, all 4 themes
    └── PackageManagerValidationTests.cs  # Package name validation
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
public void Categories_ReturnsAll69Categories()
{
    var engine = new TweakEngine();
    engine.RegisterBuiltins();

    var categories = engine.Categories();

    Assert.Equal(69, categories.Count);
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

| Component | Target | Notes |
|-----------|--------|-------|
| TweakDef model | 95%+ | Pure logic, fully testable |
| TweakEngine | 90%+ | Core business logic |
| RegistrySession | 80%+ | DryRun mode for safe testing |
| Services | 85%+ | Mock P/Invoke and WMI |
| GUI (Theme) | 90%+ | Theme records are pure data |
| GUI (Forms) | 60%+ | WinForms hard to unit test |

## What NOT to Do in Tests

- Don't test implementation details — test behaviour
- Don't use `Thread.Sleep()` — use `Task.Delay` with async tests or mocks
- Don't leave temporary files — use `Path.GetTempPath()` with cleanup
- Don't catch exceptions to silence test failures
- Don't test private methods directly — test through public API
- Don't create real registry keys in tests — use `DryRun = true` on RegistrySession
- Don't create actual WinForms windows in CI — test data models and logic only
- Don't use `Assert.True(condition)` when a specific assertion exists (e.g., `Assert.Equal`, `Assert.Contains`)
