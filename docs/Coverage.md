# RegiLattice — Coverage Report

> Test coverage baseline for the C# codebase.
> Last verified: 2025-07-20 · v2.0.0
> Command: `dotnet test --collect:"XPlat Code Coverage"`

---

## Summary

| Scope | Tests | Status |
|---|---|---|
| **RegiLattice.Core.Tests** | 93 tests | All passing |
| **RegiLattice.GUI.Tests** | 36 tests | All passing |
| **Total** | 129 tests | All passing |

---

## Test File Inventory

### Core Tests (93 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `TweakDefTests.cs` | TweakDef model, RegOp factories, TweakScope computation | ~25 |
| `TweakEngineTests.cs` | Engine registration, lookup, search, profiles, batch operations | ~30 |
| `RegistrySessionTests.cs` | Session helpers, dry-run mode, path parsing | ~15 |
| `ServicesTests.cs` | Analytics, AppConfig, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings | ~23 |

### GUI Tests (36 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `ThemeTests.cs` | Theme switching, colour attributes, all 4 themes validated | ~20 |
| `PackageManagerValidationTests.cs` | Package name validation for Scoop, pip, PowerShell modules | ~16 |

---

## Coverage Collection

`powershell
# Collect coverage with coverlet
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report (requires ReportGenerator tool)
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html

# View report
Start-Process coveragereport\index.html
`

---

## Coverage Design Notes

- **Tweak modules** (~1 490 tweaks across 68 .cs files): Registry operations are
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
