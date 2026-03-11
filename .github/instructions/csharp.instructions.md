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

Only 2 P/Invoke calls exist in the codebase. Any new P/Invoke must be justified:

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
