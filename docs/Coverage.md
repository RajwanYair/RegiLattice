# RegiLattice — Coverage Report

> Test coverage baseline for the C# codebase.
> Last verified: 2025-07-22 · v3.2.0
> Command: `dotnet test --collect:"XPlat Code Coverage"`

---

## Summary

| Scope | Tests | Status |
|---|---|---|
| **RegiLattice.Core.Tests** | 529 tests | All passing |
| **RegiLattice.CLI.Tests** | 58 tests | All passing |
| **RegiLattice.GUI.Tests** | 71 tests | All passing |
| **Total** | 658 tests | All passing |

---

## Test File Inventory

### Core Tests (529 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `TweakDefTests.cs` | TweakDef model, RegOp factories, TweakScope computation | ~25 |
| `TweakEngineTests.cs` | Engine registration, lookup, search, profiles, batch operations, validation, dependency resolution | ~200 |
| `RegistrySessionTests.cs` | Session helpers, dry-run mode, path parsing | ~15 |
| `ServicesTests.cs` | Analytics, AppConfig, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings | ~23 |
| `PluginTests.cs` | PackLoader, PackManager, PackIndex, TweakEngine pack integration, Locale | ~62 |

### CLI Tests (56 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `ParseArgsTests.cs` | CLI argument parsing, flags, options, scope, positional args, --depends-on, --no-color | ~56 |

### GUI Tests (71 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `ThemeTests.cs` | Theme switching, colour attributes, all 4 themes, system theme detection | ~40 |
| `PackageManagerValidationTests.cs` | Package name validation for Scoop, pip, PowerShell modules | ~31 |

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

- **Tweak modules** (~2 316 tweaks across 90 .cs files): Registry operations are
  covered via RegOp declarative pattern validation. Actual `HKLM`/`HKCU` writes
  are intentionally **not** exercised in CI — they require admin and a real registry.
- **Plugin system**: PackLoader, PackManager, PackIndex round-trip and validation
  tested via 62 dedicated tests in PluginTests.cs.
- **Detection operations**: `DetectOps` are validated via `Evaluate()` in dry-run mode.
- **CorporateGuard**: P/Invoke (`GetComputerNameExW`) and WMI paths are tested with
  mocked data where possible; live detection is environment-specific.
- **GUI forms**: WinForms creation and theme application are tested; interactive
  behavior (button clicks, user input) requires manual testing.

---

## Coverage Targets

| Target | Priority | Status |
|---|---|---|
| Core.Tests covers all TweakEngine edge cases | P1 | ✅ Done |
| Integration tests for RegistrySession (DryRun mode) | P2 | ✅ Done |
| Snapshot round-trip tests | P2 | ✅ Done |
| CLI argument parsing tests (56 tests) | P1 | ✅ Done |
| Plugin system tests (62 tests) | P1 | ✅ Done |
| Validation & dependency resolution tests | P1 | ✅ Done |
| GUI.Tests for theme records and package validation | P2 | ✅ Done |
