# RegiLattice — Coverage Report

> Test coverage baseline for the C# codebase.
> Last verified: 2025-07-21 · v3.1.5
> Command: `dotnet test --collect:"XPlat Code Coverage"`

---

## Summary

| Scope | Tests | Status |
|---|---|---|
| **RegiLattice.Core.Tests** | 112 tests | All passing |
| **RegiLattice.CLI.Tests** | 52 tests | All passing |
| **RegiLattice.GUI.Tests** | 39 tests | All passing |
| **Total** | 203 tests | All passing |

---

## Test File Inventory

### Core Tests (112 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `TweakDefTests.cs` | TweakDef model, RegOp factories, TweakScope computation | ~25 |
| `TweakEngineTests.cs` | Engine registration, lookup, search, profiles, batch operations | ~30 |
| `RegistrySessionTests.cs` | Session helpers, dry-run mode, path parsing | ~15 |
| `ServicesTests.cs` | Analytics, AppConfig, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings | ~23 |

### CLI Tests (52 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `ParseArgsTests.cs` | CLI argument parsing, flags, options, scope, positional args | ~52 |

### GUI Tests (39 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `ThemeTests.cs` | Theme switching, colour attributes, all 4 themes validated | ~20 |
| `PackageManagerValidationTests.cs` | Package name validation for Scoop, pip, PowerShell modules | ~16 |

---

## Coverage Collection

```powershell
# Collect coverage with coverlet
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report (requires ReportGenerator tool)
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html

# View report
Start-Process coveragereport\index.html
```

---

## Coverage Design Notes

- **Tweak modules** (~1 981 tweaks across 71 .cs files): Registry operations are
  covered via RegOp declarative pattern validation. Actual `HKLM`/`HKCU` writes
  are intentionally **not** exercised in CI — they require admin and a real registry.
- **Detection operations**: `DetectOps` are validated via `Evaluate()` in dry-run mode.
- **CorporateGuard**: P/Invoke (`GetComputerNameExW`) and WMI paths are tested with
  mocked data where possible; live detection is environment-specific.
- **GUI forms**: WinForms creation and theme application are tested; interactive
  behavior (button clicks, user input) requires manual testing.

---

## Coverage Targets

| Target | Priority |
|---|---|
| Increase Core.Tests to cover all TweakEngine edge cases | P1 |
| Add integration tests for RegistrySession with real registry (admin CI) | P2 |
| Add snapshot round-trip tests | P2 |
| Add CLI argument parsing tests | P1 |
| Increase GUI.Tests for form layout and control binding | P2 |
