---
applyTo: "**/*.cs,**/tests/**,**/*Tests/**"
---

# Lessons Learned — RegiLattice Development

> Accumulated hard-won insights from the Python → C# migration, test coverage sprints,
> and the 453-tweak restoration campaign.
> These rules are **as important as the coding standards** — they prevent recurring mistakes.
> Last updated: 2025-07-22 (v3.2.0, C# 13 / .NET 10.0-windows, 2 301 tweaks, 89 categories)

---

## WinForms Test Pitfalls

### ❌ Never create actual Form instances in CI tests

```csharp
// ❌ BAD — creates a real window, hangs in headless CI
[Fact]
public void MainForm_Opens() { var form = new MainForm(); }

// ✅ GOOD — test the data/logic layer, not the Form
[Fact]
public void ThemeDef_HasCorrectDefaults()
{
    var theme = Theme.GetTheme("catppuccin-mocha");
    Assert.NotNull(theme);
    Assert.Equal("Catppuccin Mocha", theme.Name);
}
```

### ✅ Test theme records and data models instead of UI controls

WinForms controls require a message pump and are fragile in xUnit. Focus tests on:
- `ThemeDef` records (pure data, no UI dependency)
- Package name validation logic
- TweakDef model properties and computed values
- TweakEngine search/filter/profile logic

---

## Unique TweakDef IDs — Global Uniqueness Required

`TweakEngine.Register()` throws `ArgumentException` on duplicate IDs.
Every tweak ID across ALL 71 modules must be globally unique.

```csharp
// ❌ BAD — duplicate ID across modules will throw at registration
new TweakDef { Id = "perf-disable-animations", ... }  // in Performance.cs
new TweakDef { Id = "perf-disable-animations", ... }  // in Display.cs — CRASH

// ✅ GOOD — unique prefix per category
new TweakDef { Id = "perf-disable-animations", ... }     // Performance.cs
new TweakDef { Id = "display-disable-animations", ... }  // Display.cs
```

**Rule**: ID format is `{category_slug}-{descriptive-name}`. Category slugs are defined
in the copilot-instructions.md tweak ID naming convention table.

---

## RegistrySession DryRun for Safe Testing

Never write to the real registry in tests. Always use DryRun mode:

```csharp
// ✅ GOOD — DryRun captures ops without registry writes
var session = new RegistrySession { DryRun = true };
session.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Value", 42);
Assert.Single(session.DryOps);

// ❌ BAD — modifies actual registry
var session = new RegistrySession();
session.SetDword(@"HKEY_CURRENT_USER\Software\Test", "Value", 42);
```

---

## RegOp Path Format

Registry paths must use full hive names. Abbreviations are accepted but full names preferred:

```csharp
// ✅ Preferred — full hive name
RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\...", "Value", 0)

// ✅ Also accepted
RegOp.SetDword(@"HKLM\SOFTWARE\Policies\...", "Value", 0)

// ❌ BAD — missing hive prefix
RegOp.SetDword(@"SOFTWARE\Policies\...", "Value", 0)
```

---

## Sealed Classes Everywhere

All classes must be `sealed` unless they are explicitly designed as base classes:

```csharp
// ✅ GOOD
public sealed class TweakEngine { ... }
public sealed class RegistrySession { ... }
public sealed class CorporateGuard { ... }

// ❌ BAD — unsealed without justification
public class TweakEngine { ... }
```

**Why**: Sealed classes enable JIT devirtualisation and prevent unintended inheritance.

---

## IReadOnlyList Over List for Public API

```csharp
// ✅ GOOD — immutable public contract
public IReadOnlyList<RegOp> ApplyOps { get; init; } = [];
public IReadOnlyList<string> Tags { get; init; } = [];

// ❌ BAD — caller can mutate the collection
public List<RegOp> ApplyOps { get; set; } = new();
```

---

## Collection Expressions (C# 13)

Use collection expressions `[]` instead of `new List<T>()` or `Array.Empty<T>()`:

```csharp
// ✅ GOOD — C# 13 collection expression
Tags = ["telemetry", "privacy"],
ApplyOps = [RegOp.SetDword(...)],

// ❌ BAD — verbose pre-C# 12 syntax
Tags = new List<string> { "telemetry", "privacy" },
ApplyOps = new List<RegOp> { RegOp.SetDword(...) },
```

---

## P/Invoke — Only 2 Calls Allowed

The entire codebase uses exactly 2 P/Invoke calls:
1. `GetComputerNameExW` in `CorporateGuard.cs` — AD domain detection
2. `GlobalMemoryStatusEx` in `HardwareInfo.cs` — RAM detection

Any new P/Invoke must be explicitly justified. Prefer `Microsoft.Win32.Registry`
and `System.Management` (WMI) for system queries.

---

## WinForms Performance — SuspendLayout Pattern

Always suspend layout during bulk control updates:

```csharp
// ✅ GOOD — O(1) layout recalculation
panel.SuspendLayout();
try
{
    foreach (var tweak in tweaks)
        panel.Controls.Add(CreateTweakRow(tweak));
}
finally
{
    panel.ResumeLayout(true);
}

// ❌ BAD — O(n²) layout recalculations
foreach (var tweak in tweaks)
    panel.Controls.Add(CreateTweakRow(tweak));  // triggers layout each time
```

---

## Offload Heavy Work to Background

```csharp
// ✅ GOOD — keep UI responsive
var statusMap = await Task.Run(() => engine.StatusMap(parallel: true));
UpdateUI(statusMap);

// ❌ BAD — blocks UI thread
var statusMap = engine.StatusMap(parallel: true);  // freezes for seconds
UpdateUI(statusMap);
```

---

## Multi-File Edits — Use multi_replace_string_in_file

When making independent edits across multiple files (or multiple non-overlapping
edits in one file), prefer `multi_replace_string_in_file` over sequential calls.
It is faster, atomic per-replacement, and produces a single summary.

---

## GitHub Username is RajwanYair

All references to the GitHub account must use `RajwanYair`:

- `CODEOWNERS`: `@RajwanYair`
- All GitHub URLs: `https://github.com/RajwanYair/RegiLattice`
- No `aeger` references anywhere

---

## Version & Metadata

- Version lives in `.csproj` files — `<Version>3.2.0</Version>`
- Do not duplicate version strings — single source of truth per project
- GitHub URLs: `https://github.com/RajwanYair/RegiLattice`

---

## String Escape Sequences — Always Use Verbatim `@""`

Registry paths and file paths must use verbatim strings to avoid escape sequence errors:

```csharp
// ✅ GOOD — verbatim string, no escape issues
RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", 1)

// ❌ BAD — \S, \P, \W are invalid escape sequences → CS1009
RegOp.SetDword("HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", 1)
```

**Common trap**: When batch-editing tweak files, forgetting the `@` prefix on registry path strings.
The C# compiler treats `\S`, `\P`, `\W`, `\D`, `\M` etc. as invalid escape sequences.

---

## Duplicate ID Detection — Check Before Adding

When adding tweaks in bulk across multiple modules, duplicates can sneak in:

```csharp
// chrome.cs already has chrome-disable-translate
// firefox.cs accidentally also defines chrome-disable-translate → CRASH at RegisterBuiltins()
```

**Prevention**: Before adding a tweak ID, search the Tweaks/ directory:

```powershell
Select-String -Pattern '"new-tweak-id"' -Path src/RegiLattice.Core/Tweaks/*.cs
```

During the 453-tweak restoration, 5 duplicate IDs were found across Chrome.cs and Firefox.cs.

---

## Tuple Deconstruction — Match Return Types Exactly

When calling helper methods, verify the return tuple arity:

```csharp
// ShellRunner.RunPowerShell returns (int exitCode, string stdout, string stderr)

// ✅ GOOD — deconstruct all 3 values
var (exitCode, stdout, stderr) = ShellRunner.RunPowerShell(script, dryRun);

// ❌ BAD — CS8132: only deconstructing 2 of 3 values
var (exitCode, output) = ShellRunner.RunPowerShell(script, dryRun);
```

---

## Batch Editing Workflow — Verify File Anchors

When making bulk edits across many files (e.g., restoring 453 tweaks):

1. Read the target file first to confirm exact content
2. Use `multi_replace_string_in_file` for independent edits
3. After each batch, verify the build: `dotnet build`
4. Commit per logical phase (e.g., per 10 modules)

**Common trap**: File contents change between sessions (e.g., formatter runs, manual edits).
Always re-read before editing — never assume content matches what was seen earlier.

---

## SetExpandString and SetQword — Less Common RegOp Factories

`RegOp.SetExpandString` and `RegOp.SetQword` exist but are rarely used.
When a registry value contains `%SystemRoot%` or other environment variables,
use `SetExpandString` (REG_EXPAND_SZ), not `SetString` (REG_SZ).

```csharp
// ✅ GOOD — REG_EXPAND_SZ preserves environment variable expansion
RegOp.SetExpandString(@"HKLM\...", "ImagePath", @"%SystemRoot%\System32\svchost.exe -k netsvcs")

// ❌ BAD — REG_SZ won't expand %SystemRoot% at runtime
RegOp.SetString(@"HKLM\...", "ImagePath", @"%SystemRoot%\System32\svchost.exe -k netsvcs")
```

---

## IsApplicable — Hardware Gating for Tweaks

Tweaks that target specific software (Chrome, Firefox, Java, Docker) or hardware
(NVIDIA GPU, WSL, Hyper-V) should set `IsApplicable` to grey them out in the GUI:

```csharp
new TweakDef
{
    Id = "chrome-disable-translate",
    IsApplicable = () => HardwareInfo.IsChromeInstalled(),
    ApplicabilityNote = "Google Chrome is not installed",
    ...
}
```

`TweakEngine.IsApplicableOnHardware()` checks custom predicates first,
then auto-detects from category (WSL, Virtualization) and tags (nvidia).
MainForm caches results in `_inapplicableIds` at startup.
