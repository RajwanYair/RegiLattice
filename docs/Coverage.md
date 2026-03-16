# RegiLattice — Coverage Report

> Test coverage baseline for the C# codebase.
> Last verified: 2026-03-17 · v3.2.1
> Command: `dotnet test --collect:"XPlat Code Coverage"`

---

## Summary

| Scope | Tests | Status |
|---|---|---|
| **RegiLattice.Core.Tests** | 643 tests | All passing |
| **RegiLattice.CLI.Tests** | 72 tests | All passing |
| **RegiLattice.GUI.Tests** | 84 tests | All passing |
| **Total** | 799 tests | All passing |

---

## Test File Inventory

### Core Tests (643 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `TweakDefTests.cs` | TweakDef model, RegOp factories, TweakScope computation | ~25 |
| `TweakEngineTests.cs` | Engine registration, lookup, search, profiles, batch operations, validation, dependency resolution, snapshots | ~213 |
| `RegistrySessionTests.cs` | Session helpers, dry-run mode, path parsing, read/write ops, evaluate | ~53 |
| `ServicesTests.cs` | Analytics, AppConfig, CorporateGuard, Elevation, HardwareInfo, Locale, Ratings | ~23 |
| `PluginTests.cs` | PackLoader, PackManager, PackIndex, TweakEngine pack integration, Locale | ~64 |
| `SnapshotManagerTests.cs` | Save/Load/Restore, round-trip, edge cases | 12 |
| `TweakValidatorTests.cs` | Valid tweaks, empty fields, duplicates, circular deps, broken deps | 19 |
| `DependencyResolverTests.cs` | Resolve (topological sort), Dependents (reverse lookup), circular detection | 15 |

### CLI Tests (72 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `ParseArgsTests.cs` | CLI argument parsing, flags, options, scope, positional args, --depends-on, --no-color, ConsoleColorizer | ~72 |

### GUI Tests (84 tests)

| Test File | Focus Area | Tests |
|---|---|---|
| `ThemeTests.cs` | Theme switching, colour attributes, all 4 themes, system theme detection | ~40 |
| `PackageManagerValidationTests.cs` | Package name validation (Scoop, pip, PSModules), PackageNameValidator shared utility | ~44 |

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
| CLI argument parsing tests (72 tests) | P1 | ✅ Done |
| Plugin system tests (62 tests) | P1 | ✅ Done |
| Validation & dependency resolution tests | P1 | ✅ Done |
| GUI.Tests for theme records and package validation | P2 | ✅ Done |
| SnapshotManager direct tests (12 tests) | P1 | ✅ Done |
| TweakValidator direct tests (19 tests) | P1 | ✅ Done |
| DependencyResolver direct tests (15 tests) | P1 | ✅ Done |
| TweakEngine edge case expansion (+13 tests) | P2 | ✅ Done |
| RegistrySession edge case expansion (+17 tests) | P2 | ✅ Done |
