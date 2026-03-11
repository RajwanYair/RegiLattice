---
mode: agent
description: "Generate comprehensive xUnit tests for the selected code following workspace testing standards"
---

# Write Tests

Generate comprehensive xUnit tests for the selected/specified code.

## Context

File to test: `${file}`
Selected code: `${selection}`

## Requirements

Follow `.github/instructions/testing.instructions.md`. Generate tests that:

1. **Cover all public methods** — happy path, edge cases, error paths
2. **Use Arrange-Act-Assert** pattern
3. **Use `[Fact]` and `[Theory]`** with `[InlineData]` for parameterized tests
4. **Mock external dependencies** — use `RegistrySession { DryRun = true }` for registry tests
5. **Target 90%+ coverage** for the code under test
6. **Follow naming convention**: `MethodName_Scenario_ExpectedResult`

## Test Structure

```csharp
using Xunit;
using RegiLattice.Core;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tests;

public sealed class Test_ClassName
{
    [Fact]
    public void Method_HappyPath_ReturnsExpected()
    {
        // Arrange
        var engine = new TweakEngine(new RegistrySession { DryRun = true });

        // Act
        var result = engine.Method();

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData("valid-id", true)]
    [InlineData("", false)]
    public void Method_WithVariousInputs_ReturnsCorrectly(string input, bool expected)
    {
        // ...
    }

    [Fact]
    public void Method_InvalidInput_ThrowsArgumentException()
    {
        var engine = new TweakEngine();
        Assert.Throws<ArgumentException>(() => engine.Method(null!));
    }
}
```

## WinForms Test Rules

When writing tests for GUI code:

1. **Never create actual Form instances** — they hang in headless CI
2. **Test ThemeDef records and data models** — pure data, no UI dependency
3. **Test package name validation** — regex patterns and edge cases
4. **Test CategoryIcons mappings** — static data lookups

## Naming Conventions

- Test files: `<ClassName>Tests.cs`
- Test classes: `sealed class <ClassName>Tests`
- Test methods: `MethodName_Scenario_ExpectedResult`

## Generate

Create the complete test file with all imports, fixtures, and test cases.
