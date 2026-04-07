---
name: debug-fix
description: "Diagnose and fix build errors, test failures, compile warnings, or runtime exceptions in RegiLattice. Use when there are CS errors, test failures, duplicate tweak IDs, nullable warnings, or unexpected runtime behaviour. Triggers on: 'error', 'fails', 'broken', 'exception', 'CS8', 'MSB', 'duplicate', 'not found', 'fix'."
argument-hint: "Describe the error or symptom (e.g. 'CS0121 duplicate tweak ID', 'test hangs')"
---

# Debug & Fix — RegiLattice

## Diagnostic Process

1. **Identify** the layer: build error → compiler; test failure → xUnit; runtime → exception stack
2. **Read** the affected file(s) with full context — never guess at content
3. **Search** for related patterns that might be the root cause
4. **Fix** the root cause, not the symptom
5. **Verify**: build succeeds (0 errors, 0 warnings), all tests pass

## Error Catalogue

### Duplicate Tweak ID
```
ArgumentException: Duplicate tweak ID: xxx (from TweakEngine.Register)
```
**Fix**: Search all modules for the ID → rename the duplicate
```powershell
Select-String -Pattern '"the-duplicate-id"' -Path src/RegiLattice.Core/Tweaks/*.cs
```

### HasOperations Gate (silent skip)
**Symptom**: Tweak registered but never returned by `AllTweaks()` — no error thrown
**Fix**: Ensure `ApplyOps` or `ApplyAction` is set (engine skips tweaks where `HasOperations == false`)

### Nullable Warning CS8602
```
CS8602: Dereference of a possibly null reference
```
**Fix**: Add null guard: `?.` / `??` / early return / `ArgumentNullException.ThrowIfNull()`

### Escape Sequence CS1009
```
CS1009: Unrecognized escape sequence \S (or \P, \W, \M, \D)
```
**Fix**: Add `@` prefix: `@"HKEY_LOCAL_MACHINE\SOFTWARE\..."` (verbatim string)

### Unsealed Class Warning
```
CA1852: Seal 'ClassName'
```
**Fix**: Add `sealed` keyword — all classes are sealed unless inheritance is intentional

### MSB3492 Cache Lock (OneDrive build only)
```
MSB3492: Could not read existing file '...AssemblyInfoInputs.cache'
```
**Fix**: Delete the `.cache` file and rebuild sequentially per project with `-m:1`
```powershell
Remove-Item -Force "src\RegiLattice.Core\obj\Release\net10.0-windows\RegiLattice.Core.AssemblyInfoInputs.cache"
dotnet build src/RegiLattice.Core/RegiLattice.Core.csproj -c Release
```

### WinForms Test Hang
**Symptom**: Test times out with no output
**Fix**: Never create a `Form` instance in tests — test `ThemeDef`, `TweakDef`, validation logic only

### Tuple Deconstruction CS8132
```
CS8132: Cannot deconstruct a tuple of N elements into M variables
```
**Fix**: `ShellRunner.RunPowerShell` returns `(int, string, string)` — destructure all 3

### Wrong Registry Key (semantic)
**Symptom**: Tweak applies but has no effect
**Fix**: Verify the registry path and value name against Windows docs; check `EnableAutoDoh` values (`2`=auto, `3`=enforce)

## Verification Commands

```powershell
# Build Core only (fastest feedback loop)
dotnet build src/RegiLattice.Core/RegiLattice.Core.csproj -c Debug

# Run failing test by name filter
dotnet test RegiLattice.sln --no-build --filter "FullyQualifiedName~MethodName"

# Show all errors and warnings
dotnet build RegiLattice.sln -c Debug 2>&1 | Select-String "error|warning"
```

## Rules

- **PowerShell only** — no bash/Unix commands ever
- **Always DryRun in tests**: `new RegistrySession { DryRun = true }`
- **Fix root cause** — `#pragma warning disable` / `[SuppressMessage]` / `// NOSONAR` are **BUGS not fixes** — forbidden
- **One concern per fix commit** — don't clean up unrelated code in a bug-fix commit
- **0 warnings policy** — `TreatWarningsAsErrors=true` is global; every build must produce 0 warnings
- **No TODO/FIXME** — if the fix requires follow-up, open a GitHub Issue instead

## Forbidden Patterns

These patterns are **never acceptable** — fix the root cause instead:

```csharp
// ❌ FORBIDDEN — suppression without fixing
#pragma warning disable CS8602
[SuppressMessage("Category", "Rule")]
// NOSONAR
// ReSharper disable ...

// ❌ FORBIDDEN — deferred work
// TODO: handle null case
// FIXME: crashes here

// ❌ FORBIDDEN — skipped tests
[Fact(Skip = "not implemented")]
[Theory(Skip = "flaky")]
```

When the compiler emits a diagnostic, **fix the root cause**:

| Warning | Fix |
|---------|-----|
| CS8602 nullable dereference | Add `?.` / `?? throw` / null check |
| CS8618 uninitialized | Use `required init` or assign default `= ""` / `= []` |
| CA1852 unsealed class | Add `sealed` keyword |
| CS0168 unused variable | Remove variable or use it in logging |
| CA1416 platform compat | Add `[SupportedOSPlatform("windows")]` |
