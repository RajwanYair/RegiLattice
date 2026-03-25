---
applyTo: "**/*.cs,**/tests/**,**/*Tests/**"
---

# Lessons Learned — RegiLattice Development

> Accumulated hard-won insights from the Python → C# migration, test coverage sprints,
> and the 453-tweak restoration campaign.
> These rules are **as important as the coding standards** — they prevent recurring mistakes.
> Last updated: 2026-03-24 (v5.5.0, C# 13 / .NET 10.0-windows, ~5075 tweaks, 223 categories, 2693 tests)

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

xUnit 2.9.3’s `Assert.Contains<T>(T, collection)` is ambiguous when the collection
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

| Operation               | Preferred tool                               | Avoid             |
| ----------------------- | -------------------------------------------- | ----------------- |
| Read a file             | `read_file` / `mcp_filesystem_read_file`     | `Get-Content`     |
| Write / create a file   | `create_file` / `replace_string_in_file`     | `Set-Content`     |
| Search text in files    | `grep_search` / `semantic_search`            | `Select-String`   |
| List directory contents | `list_dir` / `mcp_filesystem_list_directory` | `Get-ChildItem`   |
| Git status / log / diff | `mcp_gitkraken_git_*` tools                  | `git` in terminal |
| Run tests               | `runTests` tool                              | `dotnet test`     |
| Get compile/lint errors | `get_errors`                                 | `dotnet build`    |

**Why**: MCP and Copilot tools are faster, produce structured output, avoid shell
encoding issues, don't consume a terminal slot, and never get stuck.

---

## OneDrive Build Cache — Root Causes and Permanent Fix

The OneDrive-hosted workspace historically triggered a persistent MSBuild cache-lock
sequence on solution builds. These failures have been **permanently fixed** (v4.3.1).
This section documents the root causes, the fix, and the last-resort recovery procedure.

### Root Cause 1 — MSBuild Node Reuse Holds File Locks (MSB3492)

MSBuild uses worker nodes that **persist between builds by default**. On this workspace
the obj directory lives inside `%TEMP%\RegiLattice-build\` (OneDrive redirect). A
persistent MSBuild node running from a previous build holds an exclusive file handle on
`*.AssemblyInfoInputs.cache` in that temp dir. The next build tries to read the locked
file → `error MSB3492: Could not read existing file "...AssemblyInfoInputs.cache"`.

**Permanent fix**: `MSBUILDDISABLENODEREUSE=1` — set in three places so it is never missing:

1. `.env.ps1` — sourced by the default VS Code terminal profile at session start
2. `.vscode/settings.json` → `terminal.integrated.env.windows` — belt-and-suspenders
3. `ci.yml` + `release.yml` → `env: MSBUILDDISABLENODEREUSE: 1` at job level

### Root Cause 2 — `-q` (Quiet Verbosity) Aborts on Question-Build Signals

When `--verbosity quiet` (`-q`) is passed, `dotnet build`'s **question-build phase**
treats the "needs rebuild" signal as a **fatal abort** instead of proceeding to full
compilation. This caused `"Building target 'X' completely"` errors to look like build
failures even though the compiler hadn't run yet. Retrying "worked" because the
question-build's answer changed on the second attempt (partial incremental state).

**Permanent fix**: **Never use `-q`** in any build command. Use no verbosity flag
(normal) or `--verbosity minimal` for concise but non-aborting output.

### Root Cause 3 — Missing `ref\` and `refint\` Directories

On a cold cache, the SDK's `MakeDir` step picks up the non-existent `ref\` and
`refint\` subdirs as a warning/error during `_CheckForCompileOutputPathConflict`.
`Directory.Build.targets` now pre-creates these in both `InitialTargets` (runs once
before any target) and `BeforeTargets="GenerateTargetFrameworkMonikerAttribute;..."`.

### Changes Made (v4.3.1)

- `Directory.Build.targets` — dual-hook strategy (`InitialTargets` + `BeforeTargets`)
  now pre-creates `ref\` and `refint\` subdirs; new `RegiLatticeClearStaleCaches`
  target (`BeforeTargets="CoreGenerateAssemblyInfo"`) deletes `AssemblyInfoInputs.cache`
  pre-emptively on every build, eliminating MSB3492 as a defence-in-depth measure.
- `.env.ps1` — `$env:MSBUILDDISABLENODEREUSE = '1'`
- `.vscode/settings.json` — `"terminal.integrated.env.windows": { "MSBUILDDISABLENODEREUSE": "1" }`
- `.github/workflows/ci.yml` + `release.yml` — `env: MSBUILDDISABLENODEREUSE: 1`

### Normal Build Command (no workarounds needed)

```powershell
# Debug build — first attempt always succeeds
dotnet build RegiLattice.sln -c Debug -m:1

# Release build
dotnet build RegiLattice.sln -c Release -m:1

# NEVER use -q / --verbosity quiet — it causes question-build aborts
```

> **Path note**: The solution platform is `x64`. MSBuild places obj files under
> `obj\x64\<Config>\net10.0-windows\` when built from VS, and under
> `obj\<Config>\net10.0-windows\` when built from the terminal. Cache-clearing
> commands use `Get-ChildItem -Recurse -Filter` to find files regardless of depth.

### Last-Resort Recovery (if MSBUILDDISABLENODEREUSE was not set)

If builds still fail (e.g., on a machine where `.env.ps1` was not sourced):

```powershell
# Step 1 — shut down persistent MSBuild nodes FIRST (releases file locks)
dotnet build-server shutdown

# Step 2 — delete the entire Core cache dir
Remove-Item "$env:TEMP\RegiLattice-build\RegiLattice.Core" -Recurse -Force -ErrorAction SilentlyContinue

# Step 3 — ensure MSBUILDDISABLENODEREUSE is set for this session
$env:MSBUILDDISABLENODEREUSE = '1'
Start-Sleep 2

# Step 4 — single solution build; succeeds on first attempt
dotnet build RegiLattice.sln -c Debug -m:1
```

**Do NOT use `-q`** in any of these recovery commands — it will cause the same abort.

---

## Terminal Hebrew Character Injection — Ignore, Don't Retry

Some terminals in this workspace prepend Hebrew characters (e.g., `בcd`, `בdotnet`) to
the first invocation in a new shell. This is a VS Code terminal input buffer artefact,
not a command error.

**Symptoms**:

- `בcd: The term 'בcd' is not recognized...` → the `cd` command was fine; the previous
  command in the buffer was injected first
- `ExitCode: 0` immediately follows the error → the _real_ command ran correctly

**Rules**:

- Do NOT retry just because of these prefix errors — check `$LASTEXITCODE` or the
  output after the error line to know if the actual command succeeded
- If a fresh terminal is needed, open a new one rather than reusing a shell showing
  this artefact

---

## `get_errors` Tool Returns CSharpier Formatting Diagnostics, Not Build Errors

When `get_errors` reports "Replace ⏎ with ..." or "Delete ·" errors in `.cs` files,
these are **CSharpier code-style whitespace differences**, not C# compiler errors
(CS-prefixed). The file compiles correctly despite them.

**Rule**: Ignore formatting-only `get_errors` output when diagnosing build failures.
Only act on errors that:

- Have a CS-prefix (`CS0121`, `CS1009`, etc.)
- Reference a specific line with a missing symbol, type mismatch, or syntax error

Real compiler errors from `dotnet build` are the authoritative source. Use the
terminal build output; `get_errors` is useful for real compile-time type/symbol errors
but fires on formatting differences too.

---

## Check Whether a Tweak Module Already Exists Before `create_file`

`create_file` returns `"File already exists"` if the file is already in the Tweaks/
directory. This happens because:

- A prior session created the file
- CSharpier or a formatter already generated it
- The sprint list was inaccurate about what was already done

**Rule**: Before calling `create_file` for a new sprint module, always check:

```powershell
Test-Path "src/RegiLattice.Core/Tweaks/<ModuleName>.cs"
```

Or use `list_dir` on the Tweaks directory to confirm the file doesn't exist.
If it does exist, read it first and edit rather than recreate.

---

## Intra-Module Duplicate Registry Ops — Last TweakDef Pattern

When filling the last 1–2 entries in a new 10-tweak module, copy-paste from nearby
tweaks can produce functionally identical registry ops (same path + value name) as an
earlier tweak in the **same module**, not just across modules.

**Example**: `UserActivity.cs` — `activity-disable-publishing` and `activity-disable-timeline-view`
both set `HKLM\...\System\EnableActivityFeed=0`. The fix:

- Audit all 10 entries of a new module after writing them
- For the duplicate, find a **different policy key** that achieves a related but distinct goal
  (e.g., `EnableCdp=0` instead of a second `EnableActivityFeed=0`)
- The `activity-disable-cdp` fix in `UserActivity.cs` (v4.2.x) is the canonical example

**Quick scan for within-module duplication**:

```powershell
# Show all ApplyOps path+value pairs in one file — spot duplicates visually
Select-String -Path src/RegiLattice.Core/Tweaks/UserActivity.cs -Pattern '"HKEY[^"]*"' |
    Select-Object -ExpandProperty Line
```

---

## `Remove-Item` in Agent Terminal — Pre-Approved via settings.json

`Remove-Item` is configured in `.vscode/settings.json` as an approved agent terminal
command and will **not** prompt for approval:

```json
"github.copilot.chat.agent.terminal.allowList": {
    "Remove-Item": true
}
```

This means cache-clearing commands like the following run without interruption:

```powershell
Remove-Item "$env:TEMP\RegiLattice-build\RegiLattice.Core" -Recurse -Force -ErrorAction SilentlyContinue
```

---

## `AppConfig.ConfigDir` — NOT `AppConfig.DataRoot`

`AppConfig` exposes `ConfigDir` as the canonical data-directory property.
`DataRoot` does **not exist** on `AppConfig`; using it produces `CS0117`.

```csharp
// ✅ GOOD — correct property for all services that persist data
string path = Path.Combine(AppConfig.ConfigDir, "compliance-history.json");

// ❌ BAD — CS0117: 'AppConfig' does not contain a definition for 'DataRoot'
string path = Path.Combine(AppConfig.DataRoot, "compliance-history.json");
```

**What `ConfigDir` returns**:

- Normal mode: `%LOCALAPPDATA%\RegiLattice`
- Portable mode: `<exe-dir>\data\`

Every Core service that stores persistent data (SnapshotManager, ComplianceHistory,
Favorites, TweakHistory, Ratings, Analytics …) uses `AppConfig.ConfigDir`.
When creating a new service, grep existing usages to confirm the correct property:

```powershell
Select-String -Pattern 'AppConfig\.' -Path src/RegiLattice.Core/Services/*.cs | Select-Object -First 5
```

**How was this caught?** Sprint 105's `ComplianceHistory.cs` and `Program.cs:RunComplianceAuto`
both used `DataRoot` → `CS0117` on Release build. Fixed to `ConfigDir` in commit `08868e7`.

---

## `ParseArgs()` Returns `CliArgs?` — Always Guard with `Assert.NotNull()`

`Program.ParseArgs()` (CLI entry point) returns `CliArgs?` (nullable). Every xUnit test
that calls it must guard with `Assert.NotNull()` **before** accessing any property,
or the compiler emits `CS8602: Dereference of a possibly null reference`.

```csharp
// ❌ BAD — CS8602: accessing .NewPack on a possibly-null CliArgs?
var a = ParseArgs(new[] { "--new-pack", "my-custom-pack" });
Assert.Equal("my-custom-pack", a.NewPack);

// ✅ GOOD — null-guard satisfies nullable flow analysis
var a = ParseArgs(new[] { "--new-pack", "my-custom-pack" });
Assert.NotNull(a);                               // <-- required
Assert.Equal("my-custom-pack", a.NewPack);
```

**Why `Assert.NotNull` works**: xUnit's `Assert.NotNull<T>(T?)` is annotated with
`[MemberNotNull]` / `[DoesNotReturnIf(false)]` semantics that C#'s nullable flow
analysis understands — after the call the variable is treated as non-null.

**Standing rule**: EVERY test in `ParseArgsTests.cs` that captures the return value of
`ParseArgs(...)` must have `Assert.NotNull(result)` as its very first assertion.
All existing tests follow this pattern — do not break it when adding new tests.

---

## MD022 — CHANGELOG.md Headings Need a Blank Line Below Them

Markdownlint rule **MD022** requires every ATX heading (`#` through `######`) to be
surrounded by blank lines. The rule is enforced by the VS Code markdownlint extension
and will show as Problems panel warnings if violated.

```markdown
<!-- ❌ BAD — MD022: no blank line between heading and content -->

#### Enhanced

- item one

<!-- ✅ GOOD — blank line separates heading from list -->

#### Enhanced

- item one
```

**When this triggers**: Sprint CHANGELOG entries tend to use `####` sub-headings
(`#### Enhanced`, `#### Fixed`, `#### Stats`) followed immediately by bullets or text.
Always insert a blank line after any `####` heading you add to `docs/CHANGELOG.md`.

**Violation found (Sprint 105 build-fix)**: Three `####` headings in the `[4.3.0]`
entry had no blank line below them, causing MD022 warnings in the Problems panel.
Fixed in commit `08868e7`.

---

## Flaky Tests — `DateTime.Now` in Generated Output

Any test that calls a method which internally embeds `DateTime.Now` **twice** (e.g., once
to generate a file, once to build in-memory) can produce a 1-second mismatch between the
two timestamps. This makes the test fail non-deterministically around second boundaries.

**Pattern found in**: `HtmlReportGeneratorTests.Generate_FileContentsMatchBuild`
`HtmlReportGenerator.Build()` embeds `DateTime.Now` as a formatted string. When the test
calls `gen.Generate(path, map)` (writes file) and then `gen.Build(map)` (returns string)
in quick succession, the two calls can straddle a clock second.

**Fix — strip the timestamp before comparing**:

```csharp
static string StripTimestamp(string html) =>
    System.Text.RegularExpressions.Regex.Replace(
        html,
        @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}",
        "TIMESTAMP"
    );

Assert.Equal(StripTimestamp(built), StripTimestamp(written));
```

**General rule**: Whenever a test compares two independently generated strings where
either could contain `DateTime.Now` / `DateTime.UtcNow`, strip or replace all
timestamp-like patterns before the `Assert.Equal`. The stripping should be as narrow
as possible (match only the known format) to catch real content differences.

---

## Flaky Tests — Performance Budget Tests Must Scale with Tweak Count

`Search_CompletesUnder50ms` and similar wall-clock assertions were written when the
codebase had ~3000 tweaks. As the tweak count grows, baseline search time grows
proportionally. A budget that was safe at 3000 tweaks fails non-deterministically at
4000+ tweaks.

**Pattern found in**: `TweakEngineBuiltinsTests.Search_CompletesUnder50ms`

Original: `Assert.True(sw.ElapsedMilliseconds < 50, ...)`
Fixed to: `Assert.True(sw.ElapsedMilliseconds < 150, ...)` with a comment:

```csharp
// Budget relaxed from 50ms → 150ms: 4058 tweaks with synonym expansion;
// baseline ~60ms on dev machine. Increase threshold if tweak count grows past 6000.
Assert.True(sw.ElapsedMilliseconds < 150, $"Search took {sw.ElapsedMilliseconds}ms (budget: 150ms)");
```

**Rule**: Every performance-budget test **must** include a comment with:

1. What the budget was before relaxation and why it was changed
2. The approximate tweak/item count at time of writing
3. When the threshold should be re-evaluated

This makes future budget relaxations deliberate instead of surprising.

---

## `ImpactScore`, `SafetyRating`, `ImpactNote` — Set Explicitly on All New Tweaks

These three fields were added to `TweakDef` as part of the Phase C Intelligence Engine
(commit `1d01c3f`). They power the Health Score dashboard and Smart Scan recommendation
engine. Modules created before the addition have explicit values; modules created after
**should** set them explicitly rather than relying on defaults.

```csharp
// ✅ GOOD — explicit values per tweak, calibrated to actual impact and risk
new TweakDef
{
    Id = "wdag-disable-clipboard-sharing",
    ImpactScore = 4,    // 1 = minimal benefit, 5 = major benefit
    SafetyRating = 5,   // 1 = risky, 5 = very safe
    ImpactNote = "Reduces attack surface; may break clipboard paste into WDAG sessions.",
    ...
}

// ⚠️ WARNING — relying on defaults (ImpactScore=3, SafetyRating=4) loses calibration
new TweakDef
{
    Id = "attach-block-mime-change",
    // No ImpactScore/SafetyRating → engine uses defaults 3/4
    ...
}
```

**TweakValidator** enforces `ImpactScore` and `SafetyRating` are in the range 1–5.
If either is set outside that range the `--validate` command will report it as an error.

**Scale guidance**:

| ImpactScore | Meaning                                  | Example                            |
| ----------- | ---------------------------------------- | ---------------------------------- |
| 5           | Major benefit; changes visible behaviour | Disable consumer experiences       |
| 4           | Significant benefit; measurable effect   | Disable WER data upload            |
| 3           | Moderate benefit; moderate effect        | Disable spotlight on action center |
| 2           | Minor benefit; subtle effect             | Disable CDM spotlight on taskbar   |
| 1           | Marginal benefit; mostly cosmetic        | Suppress a rarely seen UI element  |

| SafetyRating | Meaning                                | Example                              |
| ------------ | -------------------------------------- | ------------------------------------ |
| 5            | Very safe; reversible; no side effects | Disable Spotlight                    |
| 4            | Safe for most users; minor caveats     | Disable WDAG clipboard               |
| 3            | Moderate risk; admin-only subsystems   | Disable WER upload (may affect DR)   |
| 2            | Elevated risk; test on non-prod first  | Disable Terminal Services features   |
| 1            | High risk; can break functionality     | Block all remote desktop connections |

**When was this hit?** Batch 4 modules (`WindowsAttachmentsPolicy.cs`,
`WindowsMailPolicy.cs`, `NetMeetingPolicy.cs`, `CloudNotificationsPolicy.cs`,
`ConferencingPolicy.cs`) were created in v5.5.0 without explicit `ImpactScore`/
`SafetyRating`. The validator passes because defaults are valid, but the intelligence
engine will surface these tweaks with generic scores instead of calibrated ones.
Fix by adding explicit values per TweakDef during the next touch of those files.

---

## Policy Module Gap Analysis — Full Workflow

Before creating any new policy module, run all three phases of the gap analysis:

### Phase 1 — Registry path not already claimed

```powershell
# No hits = CLEAR to use
Select-String -Pattern 'Policies\\[TargetKeyword]' -Path "src/RegiLattice.Core/Tweaks/*.cs"
```

### Phase 2 — Slug not already claimed

```powershell
# No hits = CLEAR to use
Select-String -Pattern '"slug-' -Path "src/RegiLattice.Core/Tweaks/*.cs"
```

### Phase 3 — Semantic conflict check (same PATH\ValueName pair as existing tweak)

This is the most important check and the easiest to miss. A tweak targeting a different
_key_ in the same Windows subsystem (e.g., `Windows Error Reporting`, `Reliability`) can
accidentally reuse an existing `PATH\ValueName` from a distant module.

```powershell
# Check specific value names across ALL modules before using them
Select-String -Pattern '"ValueName"' -Path "src/RegiLattice.Core/Tweaks/*.cs"
```

**Real example**: `WindowsReliabilityPolicy.cs` (v5.4.0) initially defined:

- `WerKey\Disabled = 1` — already in `ApplicationRestartPolicy.cs`
- `WerKey\DontSendAdditionalData = 1` — already in `ApplicationRestartPolicy.cs`
- `WerKey\Disabled = 1` also in `AppCompatibility.cs`

**Fix**: Replace duplicated ops with semantically distinct alternatives using a different
key (`RelKey\ReasonRequired=0`, `RelKey\ShutdownReasonOn=0`).

**Prevention rule**: When you know a module touches a well-known subsystem (WER, Defender,
Windows Update, BITS, SNMP), always grep for that subsystem name in all modules:

```powershell
Select-String -Pattern 'Error Reporting|Windows Error' -Path "src/RegiLattice.Core/Tweaks/*.cs"
```

---

## stats.svg Uses Space-Separated Thousands

The `docs/assets/stats.svg` file uses **non-breaking space** (or regular space) as
a thousands separator: `5 025`, `5 075` — **not** `5025`, `5075`.

```xml
<!-- ✅ GOOD — space-separated thousands as per style commit 7e464a8 -->
<text ...>5 075</text>

<!-- ❌ BAD — no separator; regex replacement will fail to find the old value -->
<text ...>5075</text>
```

**Why**: Commit `7e464a8` ("style(docs): merge space-separated thousands") standardised
all markdown and SVG files to use this format. `replace_string_in_file` against the SVG
must match the exact format including the space.

**What to update on each version bump (6 files, listed in order)**:

| File                              | What changes                                                                                          |
| --------------------------------- | ----------------------------------------------------------------------------------------------------- |
| `docs/assets/stats.svg`           | Tweak count + category count (space-separated thousands)                                              |
| `Directory.Build.props`           | All 4 version properties: `<Version>`, `<AssemblyVersion>`, `<FileVersion>`, `<InformationalVersion>` |
| `installer/Package.wxs`           | `Version="X.Y.Z"` (no `.0` suffix here)                                                               |
| `README.md`                       | Badge, download link, description line, features bullet, diagram count, folder tree count, footer     |
| `.github/copilot-instructions.md` | Header line, version table row, tweaks/categories/modules row                                         |
| `docs/CHANGELOG.md`               | Prepend new `## [X.Y.Z]` section                                                                      |

---

## `git -C "path" command` for One-Shot Git Ops in Unstable Terminals

When terminal state is unreliable (history-picker stuck, Hebrew char injection, cwd drift),
use `git -C "absolute-path" <subcommand>` instead of `cd path; git <subcommand>`:

```powershell
# ✅ RELIABLE — cwd is irrelevant; git operates on the specified path
git -C "c:\Users\ryair\...\RegiLattice" status --short
git -C "c:\Users\ryair\...\RegiLattice" commit -m "message"
git -C "c:\Users\ryair\...\RegiLattice" tag v5.5.0
git -C "c:\Users\ryair\...\RegiLattice" push
git -C "c:\Users\ryair\...\RegiLattice" ls-remote origin refs/tags/v5.5.0

# ⚠️ FRAGILE — depends on the terminal's cwd being correct
cd "c:\Users\ryair\...\RegiLattice"; git status --short
```

**Why it matters**: `run_in_terminal` opens background terminals that `cd` to the
workspace directory but may start in history-picker state (no output for 4–5 seconds).
`git -C` is immune to this — it works regardless of cwd.

---

## Policy Module 5×10 Cadence — Standing Pattern

The current expansion pattern for each MINOR version bump is:

- **5 new policy modules** per version bump
- **10 tweaks per module** (all declarative `ApplyOps`/`RemoveOps`/`DetectOps`)
- All keys under `HKLM\SOFTWARE\Policies\Microsoft\...` (machine-wide Group Policy)
- `NeedsAdmin = true`, `CorpSafe = true` on all policy tweaks
- `ImpactScore` and `SafetyRating` set explicitly per tweak

Version history:
| Version | Modules | Tweaks | Sprint range |
|---------|---------|--------|---|
| v5.1.0 | 5 | 50 | 162–166 |
| v5.2.0 | 5 | 50 | 167–171 |
| v5.3.0 | 5 | 50 | 172–176 |
| v5.4.0 | 5 | 50 | 177–181 |
| v5.5.0 | 5 | 50 | 182–186 |
| v5.6.0 | 5 | 50 | 187–191 |
| v5.7.0 | 5 | 50 | 192–196 |
| v5.8.0 | 5 | 50 | 197–201 |
| v5.9.0 | 5 | 50 | 202–206 |
| v5.10.0 | 5 | 50 | 207–211 |
| v5.11.0 | 5 | 50 | 212–216 |
| v5.12.0 | 5 | 50 | 217–221 |
| v5.13.0 | 5 | 50 | 222–226 |
| v5.14.0 | 5 | 50 | 227–231 |
| v5.15.0 | 5 | 50 | 232–236 |
| v5.16.0 | 5 | 50 | 237–241 |
| v5.17.0 | 5 | 50 | 242–246 |
| v5.18.0 | 5 | 50 | 247–251 |
| v5.19.0 | 5 | 50 | 252–256 |
| v5.20.0 | 5 | 50 | 257–261 |
| v5.21.0 | 5 | 50 | 262–266 |
| v5.22.0 | 5 | 50 | 267–271 |
| v5.23.0 | 5 | 50 | 272–276 |
| v5.24.0 | 5 | 50 | 277–281 |
| v5.25.0 | 5 | 50 | 282–286 |
| v5.26.0 | 5 | 50 | 287–291 |

**Next sprint**: 292–296 (v5.27.0). Run full gap analysis on all three phases before creating any module.
