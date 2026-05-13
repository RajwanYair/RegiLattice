---
applyTo: "**/*.cs"
---

# C# Coding Instructions

## Style & Formatting

- **Target**: C# 13 / .NET 10.0-windows (x64)
- **Indentation**: 4 spaces (no tabs)
- **Braces**: Allman style (new line for opening brace)
- **Naming**: PascalCase for types, methods, properties; camelCase for locals and parameters; `_camelCase` for private fields
- **Line length**: ~150 characters (soft limit)
- **Nullable**: `#nullable enable` — treat all nullable warnings as errors
- **Implicit usings**: enabled via `<ImplicitUsings>enable</ImplicitUsings>`

## Zero-Warning Policy — Non-Negotiable

> **Every warning is an error. The build must produce 0 fatals, 0 errors, 0 warnings.**

`Directory.Build.props` sets `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` globally (except Stryker mutation builds). This is a **hard policy** — not aspirational. CI fails on any new warning.

```xml
<!-- Already in Directory.Build.props — do not remove or weaken -->
<TreatWarningsAsErrors Condition="'$(STRYKER_BUILD)' != '1'">true</TreatWarningsAsErrors>
```

### Fix at Source — Never Suppress

When the compiler or analyzer emits a diagnostic, **fix the root cause**. The following suppression mechanisms are **absolutely forbidden**:

```csharp
// ❌ FORBIDDEN — suppresses without fixing
#pragma warning disable CS8602
#pragma warning disable
[SuppressMessage("Category", "Rule")]
// NOSONAR
// NCA
// ReSharper disable ...
// NOLINT
// HACK: (used to sidestep a quality check)
```

**Inline quality gate waivers are also forbidden** — they are indistinguishable from suppressions:

```csharp
// ❌ FORBIDDEN — lint ignore / bypass comments
// csharpier-ignore
// dotnet-stryker-exclude
// coverage: ignore
```

**Resolution patterns** (fix instead of suppress):

| Warning | Fix instead of suppress |
| ------- | ----------------------- |
| CS8600 / CS8602 nullable | Add `!` null-forgiving only if provably non-null; otherwise add null check or propagate nullability |
| CS8618 uninitialized property | Use `required` + `init`, or assign `= ""` / `= []` default; never `null!` |
| CA1416 platform compat | Annotate with `[SupportedOSPlatform("windows")]`; move to the GUI project which targets `net10.0-windows10.0.19041.0` |
| CS0168 unused variable | Remove the variable; if a catch clause needs it for logging, use `ex` and log it |
| CS8604 dereferencing nullable | Add `?? throw new InvalidOperationException(...)` or guard with `if (x is null)` |

### No TODO / FIXME in Committed Code

Every `TODO` or `FIXME` comment represents **incomplete work** that will silently age. They are forbidden in committed code.

```csharp
// ❌ FORBIDDEN — deferred work with no action plan
// TODO: handle the case where registry key doesn't exist
// FIXME: this crashes when called from a background thread

// ✅ CORRECT — fix the issue now or open a tracked GitHub Issue
// The TweakEngine is thread-safe via _lock; callers on background threads are safe.
```

**If work is genuinely deferred**: open a GitHub Issue, reference it in the commit, and describe the scope in the PR body. No inline reminders in code.

## Type System — Non-Negotiable

Every public member must have explicit types. Use nullable annotations:

```csharp
// ✅ Good — explicit types, nullable annotation
public string ProcessItem(string item, bool verbose = false) { ... }
public TweakDef? GetTweak(string id) { ... }

// ❌ Bad — no nullable annotation on reference return
public TweakDef GetTweak(string id) { ... }  // could return null but not annotated
```

## Prefer Records and Init-Only Properties

```csharp
// ✅ Good — immutable data model
public sealed class TweakDef
{
    public required string Id { get; init; }
    public required string Label { get; init; }
    public IReadOnlyList<string> Tags { get; init; } = [];
}

// ✅ Good — simple value type
public record ThemeDef(string Name, Color Background, Color Foreground, ...);

// ❌ Bad — mutable POCO with public setters for config data
public class TweakDef { public string Id { get; set; } }
```

## Sealed Classes by Default

Mark classes `sealed` unless inheritance is explicitly needed:

```csharp
// ✅ Good
public sealed class TweakEngine { ... }

// ❌ Bad — unsealed without reason
public class TweakEngine { ... }
```

## Collection Expressions and IReadOnlyList

```csharp
// ✅ Good — collection expression, immutable interface
public IReadOnlyList<string> Tags { get; init; } = [];

// ✅ Good — LINQ for transformations
var ids = tweaks.Select(t => t.Id).ToList();

// ❌ Bad — mutable List exposed directly
public List<string> Tags { get; set; } = new();
```

## Pattern Matching

```csharp
// ✅ Good — switch expression
public string StatusLabel(TweakResult result) => result switch
{
    TweakResult.Applied => "[ON]",
    TweakResult.NotApplied => "[OFF]",
    TweakResult.Unknown => "[???]",
    _ => "[---]",
};

// ✅ Good — is pattern
if (value is int dword and dword == expected)
    return true;
```

## Exception Handling

- Use specific exception types (never bare `catch (Exception)` unless re-throwing)
- Provide meaningful messages with context
- Use `when` clauses for filtered catches

```csharp
// ✅ Good — specific, with context
try
{
    session.SetDword(path, name, value);
}
catch (UnauthorizedAccessException ex)
{
    Log.Error($"Access denied writing {path}\\{name}: {ex.Message}");
    throw;
}
catch (System.Security.SecurityException ex)
{
    Log.Error($"Security error on {path}: {ex.Message}");
    return TweakResult.Error;
}

// ❌ Bad — swallows errors
try { Apply(); } catch { }
```

## String Handling

```csharp
// ✅ Good — interpolated strings
var msg = $"Applied {count} tweaks in {elapsed.TotalMilliseconds:F0}ms";

// ✅ Good — StringComparison for comparisons
if (id.Equals(other, StringComparison.OrdinalIgnoreCase)) { ... }
if (query.Contains(term, StringComparison.OrdinalIgnoreCase)) { ... }

// ❌ Bad — culture-sensitive by default
if (id.ToLower() == other.ToLower()) { ... }
```

## Registry Access — Use Microsoft.Win32.Registry

```csharp
// ✅ Good — structured registry access via RegistrySession
session.SetDword(@"HKEY_CURRENT_USER\Software\...", "ValueName", 1);

// ✅ Good — RegOp declarative pattern (95% of tweaks)
ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\...", "Value", 1)],
RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\...", "Value")],
DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\...", "Value", 1)],

// ❌ Bad — raw Registry API without wrapper
Registry.SetValue(@"HKEY_CURRENT_USER\...", "Value", 1);
```

## P/Invoke — Minimize and Document

Only 4 P/Invoke calls exist in the codebase (GetComputerNameExW, GlobalMemoryStatusEx ×2, GetSystemTimes). Any new P/Invoke must be justified:

```csharp
// ✅ Good — documented, DllImport with exact signature
[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
private static extern bool GetComputerNameExW(int nameType, StringBuilder buffer, ref int size);
```

## WinForms Performance

```csharp
// ✅ Good — suspend layout during bulk updates
panel.SuspendLayout();
try
{
    foreach (var tweak in tweaks)
        panel.Controls.Add(CreateRow(tweak));
}
finally
{
    panel.ResumeLayout(true);
}

// ✅ Good — double buffering for smooth scrolling
SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

// ✅ Good — offload heavy work
var status = await Task.Run(() => engine.StatusMap(parallel: true));
```

## LINQ — Prefer Over Manual Loops

```csharp
// ✅ Good
var privacyTweaks = engine.AllTweaks()
    .Where(t => t.Category == "Privacy")
    .OrderBy(t => t.Label)
    .ToList();

// ❌ Bad — manual loop for simple filter/transform
var list = new List<TweakDef>();
foreach (var t in engine.AllTweaks())
    if (t.Category == "Privacy")
        list.Add(t);
```

## Async/Await — For I/O and Long Operations

```csharp
// ✅ Good — async for I/O
public async Task<string> ExportJsonAsync(string path)
{
    var json = JsonSerializer.Serialize(AllTweaks(), _jsonOptions);
    await File.WriteAllTextAsync(path, json);
    return path;
}
```

## What NOT to Do

- Don't use `var` when the type isn't obvious from the right-hand side
- Don't use mutable public fields — use properties
- Don't use `dynamic` — use generics or pattern matching
- Don't use `Thread` directly — use `Task.Run` or async/await
- Don't hardcode absolute paths — use `Environment.GetFolderPath`
- Don't use `Process.Start` with unsanitized user input
- Don't leave `Console.WriteLine` in library code — use structured logging
- Don't use `string.Format` — use interpolated strings
- Don't catch `Exception` without re-throwing or explicit justification
- Don't use `#pragma warning disable` — fix the root cause instead
- Don't use `[SuppressMessage(...)]` — fix the root cause instead
- Don't use `// NOSONAR`, `// NCA`, `// ReSharper disable`, `// NOLINT`, or any inline suppression/waiver pattern
- Don't leave `TODO` or `FIXME` comments — complete the work or open a GitHub Issue
- Don't use `null!` to suppress CS8618 — use `required` + `init` or assign a real default
- Don't use `!` (null-forgiving) unless the value is provably non-null at that point
- Don't skip tests with `[Fact(Skip=...)]` or `[Theory(Skip=...)]` — fix the test or the code it tests

---

## Tweak Duplication Prevention

> Prevention is cheaper than remediation. Apply when adding tweaks, reviewing code, or before every commit.

### The 4 Duplication Layers

| Layer | Severity | Auto-detected by |
|---|---|---|
| 1 — Duplicate ID | **HARD BLOCK** | `TweakEngine.Register()` + `RegisterBuiltins_AllIdsUnique` test |
| 2 — Duplicate registry op (`PATH\ValueName`) | **Warning** | `TweakValidator.DetectDuplicateRegistryOps()` + builtins threshold test |
| 3 — Duplicate label (same human name in 2+ modules) | **Smell** | Label scan + `RegisterBuiltins_NoCrossModuleLabelAndPathCollision` test |
| 4 — Conceptual duplicate (same outcome, different code path) | **Debt** | Manual review + `Audit-Duplications.ps1` |

### Pre-Commit Checklist (for Tweaks/ changes)

```
☐ Searched for the new tweak ID in all Tweaks/*.cs files — no match found
☐ Searched for the registry PATH\ValueName in all Tweaks/*.cs files — no match found
☐ Ran: dotnet build (0 errors — RegisterBuiltins throws on duplicate IDs)
☐ Ran: dotnet test tests/RegiLattice.Core.Tests — all duplication guard tests green
```

### Fixing Duplicates

```
Found a duplicate registry op?
├── Same category, same label → REMOVE the newer one (keep the original)
├── Same category, different label → MERGE ApplyOps into one tweak
├── Different categories, same label → Check if both are needed by different profiles
│   ├── Both needed → add "// INTENTIONAL DUPLICATE: needed for {reason}" comment
│   └── Only one needed → REMOVE the one with the wrong category
└── Different categories, different labels → audit semantics; usually remove one
```

**Full audit**: `. .\scripts\Audit-Duplications.ps1` checks all 4 layers across the entire codebase.
