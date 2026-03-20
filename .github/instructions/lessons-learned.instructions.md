---
applyTo: "**/*.cs,**/tests/**,**/*Tests/**"
---

# Lessons Learned — RegiLattice Development

> Accumulated hard-won insights from the Python → C# migration, test coverage sprints,
> and the 453-tweak restoration campaign.
> These rules are **as important as the coding standards** — they prevent recurring mistakes.
> Last updated: 2026-03-20 (v3.7.1, C# 13 / .NET 10.0-windows, 2 991 tweaks, 92 categories)

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
Every tweak ID across ALL 90 modules must be globally unique.

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

## P/Invoke — Only 4 Calls Allowed

The entire codebase uses exactly 4 P/Invoke calls:

1. `GetComputerNameExW` in `CorporateGuard.cs` — AD domain detection
2. `GlobalMemoryStatusEx` in `HardwareInfo.cs` — RAM detection
3. `GetSystemTimes` in `SystemMonitor.cs` — CPU usage (idle/kernel/user delta)
4. `GlobalMemoryStatusEx` in `SystemMonitor.cs` — live memory monitoring

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

---

## Version & Metadata

- Version lives in `Directory.Build.props` — update **all four** properties together:
  ```xml
  <Version>X.Y.Z</Version>
  <AssemblyVersion>X.Y.Z.0</AssemblyVersion>
  <FileVersion>X.Y.Z.0</FileVersion>
  <InformationalVersion>X.Y.Z</InformationalVersion>
  ```
- **No manual `Properties/AssemblyInfo.cs`** — was deleted in v3.7.1. Version attributes are
  embedded exclusively via MSBuild auto-generated AssemblyInfo (`GenerateAssemblyInfo=true`).
- Do not duplicate version strings — single source of truth is `Directory.Build.props`
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

---

## HasOperations Gate — Tweaks Without ApplyOps Are Silently Skipped

`TweakEngine.Register()` skips any `TweakDef` where `HasOperations` is `false`:

```csharp
// HasOperations == ApplyOps.Count > 0 || ApplyAction is not null

// ❌ BAD — silently skipped, never registered
new TweakDef { Id = "test-1", Label = "X", Category = "A", Tags = ["t"] }

// ✅ GOOD — has ApplyOps, so HasOperations == true
new TweakDef { Id = "test-1", Label = "X", Category = "A",
    ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)] }
```

**Common trap in tests**: Creating TweakDef instances for tag/scope/search tests without
ApplyOps. The engine silently skips them, making assertions fail with empty results.

---

## Coverage Patterns — What Is and Isn't Testable

**94.9% line coverage** on Core (571 tests). Branch coverage at 56.8%.

Components that are trivially 100%: TweakDef model, RegOp factories, all ~90 tweak
module static Tweaks properties (just declarative data), ProfileDef, PackDef.

Components that are NOT easily testable:

- Package managers (ChocolateyManager, PipManager, WinGetManager) — require external tools
- ShellRunner — runs real processes
- PackManager async methods — require network/filesystem
- CorporateGuard P/Invoke paths — environment-dependent

Safe to test against real registry (read-only):

- `RegistrySession.ReadDword/ReadString` on well-known HKCU keys
- `KeyExists/ValueExists` on known paths like `HKCU\Software`
- `ListSubKeys/ListValueNames` on populated keys

---

## EXE Version Shows 0.0.0.0 — GenerateAssemblyInfo Trap

Setting `<GenerateAssemblyInfo>false</GenerateAssemblyInfo>` disables embedding of ALL
assembly version resources. Even if `<Version>`, `<AssemblyVersion>`, `<FileVersion>` are
set as MSBuild properties, they are only written into the auto-generated file — they have
no effect when that generation is off. The PE header stays at `0.0.0.0`.

```xml
<!-- ❌ BAD — kills version embedding in the EXE/DLL PE header -->
<GenerateAssemblyInfo>false</GenerateAssemblyInfo>

<!-- ✅ GOOD — UseSharedCompilation=false is the correct OneDrive file-lock guard.
     Keep GenerateAssemblyInfo at its default (true). -->
<UseSharedCompilation>false</UseSharedCompilation>
```

**When was this hit?** The original workaround for OneDrive-hosted builds was
`GenerateAssemblyInfo=false` + a manual `Properties/AssemblyInfo.cs` in Core. But that
file was only created for Core (not GUI/CLI) and was left stale at an old version.
**Fix**: removed `GenerateAssemblyInfo=false`, deleted the manual `AssemblyInfo.cs`,
added all 4 version properties explicitly to `Directory.Build.props` so they flow
through auto-gen to all three projects.

**OneDrive cache-lock**: `<UseSharedCompilation>false</UseSharedCompilation>` prevents
the Roslyn compiler server from holding file handles on `AssemblyInfoInputs.cache`.
This is the only guard needed — there is no need to also disable assembly info generation.

---

## Wrong Registry Key Copied Across Modules

When porting or bulk-adding tweaks, it is easy to accidentally copy the wrong registry
key from a nearby tweak. Two categories of copy-paste bugs found:

### Semantic value mismatch
`EnableAutoDoh` in `HKLM\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters`:
- Value `2` = **Automatic** (try DoH, fall back to plain DNS if unavailable)
- Value `3` = **Require/Enforce** (fail DNS resolution if DoH is unavailable)

```csharp
// ❌ BAD — value 2 only tries DoH; falls back to plaintext
ApplyOps = [RegOp.SetDword(@"HKLM\SYSTEM\..\Dnscache\Parameters", "EnableAutoDoh", 2)],

// ✅ GOOD — value 3 enforces DoH (no plaintext fallback)
ApplyOps = [RegOp.SetDword(@"HKLM\SYSTEM\..\Dnscache\Parameters", "EnableAutoDoh", 3)],
```

### Completely wrong key
`evtlog-disable-dns-client-log` was incorrectly writing to `Dnscache\Parameters\EnableAutoDoh`
(which controls DNS-over-HTTPS, not event tracing). The correct key to disable the DNS
client operational event channel is:
```
HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational
  Enabled = 0
  (restore: Enabled = 1)
```

**Prevention**: When creating tweaks for event log channels, ALWAYS use the
`\WINEVT\Channels\<provider-name>` path, not the service's `Parameters` key.

---

## Duplicate Tweaks Across Modules — Auditing Pattern

The same registry operation can be silently duplicated across unrelated modules
(e.g., a Maps auto-update tweak appearing in both ScheduledTasks and WindowsRecall).
Duplicate tweaks confuse users and dilute the ID namespace.

**Detection scan**:
```powershell
# Find duplicate registry key+valuename combinations across all tweak modules
Select-String -Path src/RegiLattice.Core/Tweaks/*.cs -Pattern 'SetDword|SetString' |
    ForEach-Object { ($_ -split '"')[1..3] -join '"' } | Group-Object | Where-Object Count -gt 1
```

**Cases found and removed (v3.7.1)**:
- `msstore-disable-auto-app-updates` + `msstore-auto-update-off` → both duplicated
  `msstore-disable-auto-update` (`WindowsStore\AutoDownload=2`)
- `schtask-disable-maps-update` → subset of `schtask-task-disable-maps-update`
  (the latter sets both `AutoDownloadAndUpdateMapData` AND `AllowUntriggeredNetworkTrafficOnSettingsPage`)
- `recall-disable-auto-map-downloads` → misplaced in WindowsRecall module;
  Maps auto-download has nothing to do with Windows Recall

**Rules**:
- Keep the tweak in the **semantically correct** module, not wherever it was first written
- When two tweaks target the same registry key+value, keep the one with better description, correct CorpSafe, and matching category slug
- The simpler (fewer ops) duplicate is almost always the one to remove

---

## Code Comments — Inline Trailing vs. Standalone

Trailing inline comments (`code; // reason`) on the same line as code are harder to see
and harder to maintain than standalone comment lines:

```csharp
// ❌ BAD — trailing inline; easy to miss, can't be multi-line
_detailBox.BackColor = AppTheme.Surface; // must be set AFTER ReadOnly to prevent Windows override
e.NewValue = e.CurrentValue; // cancel second toggle in double-click sequence

// ✅ GOOD — standalone comment above the code; always visible, can expand to multiple lines
// NOTE: BackColor must be set AFTER ReadOnly; on some Windows versions ReadOnly forces
// the color back to the system default.
_detailBox.BackColor = AppTheme.Surface;

// Cancel the second checkbox toggle that fires during a double-click.
e.NewValue = e.CurrentValue;
```

**Exception**: Short unit-clarifying comments on enum values or single `// NOTE:` on
property declarations (e.g., `[assembly: ...]`) are acceptable inline.
XML `WriteEndElement()` comments (`// tagName`) are acceptable for readability in deep XML writers.

---

## Assert.Contains Ambiguity with Collection Expressions

xUnit 2.9.2’s `Assert.Contains<T>(T, collection)` is ambiguous when the collection
is a C# 13 collection expression `["a", "b"]` (matches both `HashSet<T>` and
`SortedSet<T>` overloads):

```csharp
// ❌ BAD — CS0121 ambiguous call
Assert.Contains(profile, ["business", "gaming", "privacy"]);

// ✅ GOOD — explicit array
Assert.Contains(profile, new[] { "business", "gaming", "privacy" });
```

---

## Stuck pwsh Terminal — Open a New Shell

If a `pwsh` terminal appears stuck (no output, cursor frozen, command not returning),
do **not** retry the same command or wait indefinitely. Open a new terminal shell and
run the next command there. The stuck shell can be killed separately.

Common causes: a `dotnet test` process hanging on a finalizer, a background build lock,
or the OneDrive file-system watcher delaying process exit.

**Rule**: One stuck shell = open a fresh shell. Never block progress waiting for it.

---

## Prefer MCP / Copilot Tools Over the Real Shell

When a dedicated tool exists for an operation, **always use it** instead of running
a terminal command. Fall back to the shell only when no tool covers the operation.

| Operation                         | Preferred tool                              | Avoid              |
| --------------------------------- | ------------------------------------------- | ------------------ |
| Read a file                       | `read_file` / `mcp_filesystem_read_file`    | `Get-Content`      |
| Write / create a file             | `create_file` / `replace_string_in_file`    | `Set-Content`      |
| Search text in files              | `grep_search` / `semantic_search`           | `Select-String`    |
| List directory contents           | `list_dir` / `mcp_filesystem_list_directory`| `Get-ChildItem`    |
| Git status / log / diff           | `mcp_gitkraken_git_*` tools                 | `git` in terminal  |
| Run tests                         | `runTests` tool                             | `dotnet test`      |
| Get compile/lint errors           | `get_errors`                                | `dotnet build`     |

**Why**: MCP and Copilot tools are faster, produce structured output, avoid shell
encoding issues, don't consume a terminal slot, and never get stuck.
