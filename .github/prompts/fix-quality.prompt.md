---
mode: agent
description: "Fix build warnings, nullable issues, and code quality problems in the current file"
---

# Fix Quality Issues

Fix all quality issues in: `${file}`

## What to Fix

### 1. Nullable Reference Warnings

Add proper nullable annotations or null checks:
```csharp
// Before
public string GetValue() { return dict[key]; }

// After
public string? GetValue() { return dict.GetValueOrDefault(key); }
```

### 2. Sealed Classes

Add `sealed` to all classes without inheritance:
```csharp
// Before
public class MyService { ... }

// After
public sealed class MyService { ... }
```

### 3. Collection Expressions (C# 13)

Replace verbose collection syntax:
```csharp
// Before
Tags = new List<string> { "a", "b" };

// After
Tags = ["a", "b"];
```

### 4. Exception Handling

Replace bare catches with specific types:
```csharp
// Before
try { ... } catch { return null; }

// After
try { ... }
catch (InvalidOperationException ex)
{
    _session.WriteLog($"Error: {ex.Message}");
    return null;
}
```

### 5. Security Issues

- Replace `Process.Start` with raw strings → use `ArgumentList`
- Remove hardcoded paths → use `Environment.GetFolderPath`
- Ensure all registry access goes through `RegistrySession`

## Constraints

- Do NOT change logic or behavior
- Do NOT add features
- Do NOT refactor beyond fixing the issues above
- Keep all existing tests passing
