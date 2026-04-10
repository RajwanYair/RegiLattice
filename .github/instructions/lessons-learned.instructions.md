---
applyTo: "**/*.cs,**/tests/**,**/*Tests/**"
---

# Lessons Learned — RegiLattice Development

> Accumulated hard-won insights from the Python → C# migration, test coverage sprints,
> the 453-tweak restoration campaign, and the large-file splitting campaign.
> These rules are **as important as the coding standards** — they prevent recurring mistakes.
> Last updated: 2026-04-09 (v6.28.0, C# 13 / .NET 10.0-windows, ~7,568 tweaks, 127 categories, 3,291 tests)

---

## XML Comments Must Not Contain `--` — `.runsettings` Fatal Error on .NET SDK 10.0.201+

XML 1.0 prohibits `--` (double hyphen) anywhere inside XML comment content (between `<!--`
and `-->`). Prior to .NET SDK 10.0.201, the vstest XML parser silently ignored this.
Starting with SDK 10.0.201, it raises a **fatal parse error** that causes `dotnet test` to
exit immediately with code 1 before running a single test.

**Error**: `"Settings file provided does not conform to required format. An XML comment cannot
contain '--', and '-' cannot be the last character. Line NN, position PP."`

**How it was found (v6.1.0)**: `tests/.runsettings` contained the comment
`<!-- Equivalent to passing --blame-hang-timeout 30s on the CLI, but applies even when` — the
`--blame-hang-timeout` text (double hyphen) was the culprit. CI failed in 1-3s on Test(Core)
and Test(CLI) despite the code being perfectly valid.

```xml
<!-- ❌ BAD — "--" is forbidden in XML 1.0 comment content -->
<!-- Equivalent to passing --blame-hang-timeout 30s on the CLI -->

<!-- ✅ GOOD — remove double-hyphen from comment text -->
<!-- Equivalent to passing the blame-hang-timeout 30s flag on the CLI -->
```

**Why CI failed but local passed**: Local test runs were done WITHOUT `--settings tests/.runsettings`
(the XML file was never parsed). CI always passes `--settings tests/.runsettings`, so CI always
triggered the XML error. The discrepancy masked the issue during development.

**Fix**: Changed `--blame-hang-timeout` → `blame-hang-timeout` (removed `--`) in the XML comment.
All other instances of `--` in `.runsettings` are `<!--` or `-->` (the XML delimiters themselves),
which are valid.

**Rule**: Never use `--flag-syntax` or any `--` sequence inside XML comment text, especially in
`.runsettings`, `.csproj`, `.targets`, or any XML file parsed by the .NET toolchain.

---

## `--no-build` in `dotnet test` Requires All Test DLLs To Already Exist

`dotnet test --no-build` skips the project build step entirely. If the test DLL doesn't exist
at the expected output path, the command exits immediately with:
`"The test source file ... was not found."` (exit code 1, ~2s).

**When this matters in CI**: Adding `--no-build` to test steps with the intent of "using
pre-built binaries from the Build step" is only safe if:
1. The Build step builds EVERY test project (confirmed in the `.sln` configuration)
2. The Build step output paths exactly match what `dotnet test` expects
3. No test DLL was skipped, failed to copy, or went to a different path

**In this project (v6.1.0 and v6.22.0/v6.23.0)**: `dotnet build RegiLattice.sln -c Release` was confirmed to
build Core.Tests but NOT CLI.Tests (reason unclear — possibly a race condition with a
running `testhost` holding the Core DLL locked, preventing CLI.Tests from being built).
Test(Core) passed with `--no-build`, Test(CLI) failed with "DLL not found."

> **Recurrence (v6.22.0/v6.23.0)**: This same failure resurfaced in the `release.yml`
> workflow. The `ci.yml` had already been fixed in v6.1.0, but `release.yml` still carried
> `--no-build` on its three test steps. Removing `--no-build` from **all** test steps in
> both `ci.yml` and `release.yml` is the permanent fix.

**Fix**: Remove `--no-build` from all test steps. Without it, `dotnet test ... --no-restore`
performs an incremental build (near-instant if deps are already built) before running tests.
This is the safe, robust approach: each test step is self-contained.

```yaml
# ✅ CORRECT — test step builds its own project (near-instant incremental)
dotnet test tests/RegiLattice.CLI.Tests/... -c Release --no-restore --settings ...

# ❌ RISKY — fails if DLL wasn't built by a prior Build step
dotnet test tests/RegiLattice.CLI.Tests/... -c Release --no-build --no-restore --settings ...
```

**Performance note**: Without `--no-build`, the test step does a few extra MSBuild
evaluation steps. On CI this adds ~5-15s per test project — negligible compared to a
broken build.

---

## `WebRequest.GetSystemWebProxy()` in Static Initializers Hangs the Test Runner

Calling `System.Net.WebRequest.GetSystemWebProxy()` eagerly (in a `static readonly` field
initialiser) on Intel/Intune/SCCM corporate machines triggers **WPAD proxy auto-discovery**
via WinHTTP. This performs a DNS lookup for `wpad.<domain>` and an HTTP fetch of `wpad.dat`,
which can block for **20–30+ seconds** or hang permanently when the corporate proxy is
unreachable or the Intune filter-driver intercepts WinHTTP calls.

**Problem in `PackManager.cs`**: The `static readonly HttpClient s_http` field was
initialised with `Proxy = System.Net.WebRequest.GetSystemWebProxy()`. Because it is a
`static readonly` field, it is initialised the first time **any member** of `PackManager` is
accessed — including `new PackManager()` in the test
`Dispatch_Marketplace_Installed_CallsMethod_ReturnsZero`. This froze the test runner
before the test body even ran.

**Fix (v6.0.8)**:
```csharp
// ✅ CORRECT — no eager proxy resolution; system proxy resolved lazily per-request
private static readonly HttpClient s_http = new(
    new HttpClientHandler { UseDefaultCredentials = true, UseProxy = true }
) { Timeout = TimeSpan.FromSeconds(30) };

// ❌ BROKEN — GetSystemWebProxy() blocks the static initialiser on corporate machines
private static readonly HttpClient s_http = new(
    new HttpClientHandler { Proxy = System.Net.WebRequest.GetSystemWebProxy() }
);
```

**Rule**: **Never** call `WebRequest.GetSystemWebProxy()`, `WebProxy.GetDefaultProxy()`, or
any synchronous proxy/DNS discovery API in a `static` field initialiser, static constructor,
or anywhere that runs at class-load time. Proxy resolution must happen at request time (lazy).

---

## `CorporateGuard.Status()` Blocks Test Threads on Intune Machines — Use StubCorporate

`CorporateGuard.Status()` calls all four detection methods sequentially every time it is
invoked: `IsDomainJoined()`, `IsAzureAdJoined()`, `HasGroupPolicy()`,
`HasManagementAgent()`. On Intel machines enrolled in Intune, these registry reads go
through the Intune/SCCM registry filter driver, which can block for several seconds per
call. Across 3+ `RunDoctorTests`, this adds up and can stall the test runner.

**Fix (v6.0.8)**: Added `internal static bool? StubCorporate { get; set; }` to
`CorporateGuard`. When set, both `IsCorporateNetwork()` and `Status()` return the stub
value immediately, bypassing all detection logic. Test fixtures must set this:

```csharp
// In DispatchTestFixture constructor:
CorporateGuard.StubCorporate = false;   // Bypasses all registry/P-Invoke detection

// In DispatchTestFixture.Dispose():
CorporateGuard.StubCorporate = null;    // Reset so non-test code is unaffected
```

**Rule**: Every test fixture that invokes any code path reaching `CorporateGuard` MUST set
`StubCorporate = false` (or `true` to simulate a corporate machine). Never let real corporate
detection run during unit/integration tests on a developer machine.

---

## `Task.Run` + WMI = COM STA Threads That Block Process Exit

`ManagementObjectSearcher.Get()` inside `Task.Run` (ThreadPool) can cause the
`dotnet-testhost` process to hang for hours after all tests pass.

**Root cause**: The COM infrastructure invoked by `System.Management` may create
internal STA (Single-Threaded Apartment) message-pump threads.  These threads are
**foreground threads** — the .NET runtime waits for all foreground threads to finish
before the process exits.  On Intel/corporate machines with busy Intune or SCCM WMI
providers, those COM threads can stay alive indefinitely.

**Fix applied (v6.0.3)** in `HardwareInfo.BuildHwProfile()`:
```csharp
// ✅ CORRECT — explicit background MTA threads prevent COM STA pump creation
var t = new Thread(() => { try { action(); } catch { } }) { IsBackground = true };
t.SetApartmentState(ApartmentState.MTA);
t.Start();

// ❌ BROKEN — ThreadPool Task.Run allows COM to create STA message-pump threads
Task.Run(() => DetectCpu(), cts.Token);   // STA threads block process exit
```

**Rule**: Any code that calls into `System.Management` (WMI) **must** run on a thread
created with `IsBackground = true` AND `ApartmentState.MTA`.  Never rely on `Task.Run`
for WMI calls — use explicit `Thread` with both properties set.

**Symptom**: Test output shows `Passed! Failed: 0, Passed: NNN` but then the terminal
hangs for minutes or hours before returning to the shell prompt.

---

## Redirected Stdout Pipe Buffer Deadlock in Process Tests

When a test launches a child process with `RedirectStandardOutput = true` and calls
`WaitForExit()` **without first draining stdout**, the process will deadlock if it
outputs more than ~4–64 KB of text (OS pipe buffer size).

**Root cause**: The child writes to the stdout pipe. When the pipe buffer fills, the child
blocks on `Write()`. The test is blocked on `WaitForExit()`. Neither side proceeds —
**deadlock**.

**Symptom (v6.0.4)**: `CliExe_RunWithHelp_ExitsCleanly` timed out at 10 s because the
B3 grouped help text overflows the Windows pipe buffer.

**Fix**: Drain stdout **before** or **concurrently with** `WaitForExit`:
```csharp
// ✅ CORRECT — ReadToEnd() drains stdout; process can then exit; WaitForExit returns instantly
process.Start();
string _ = process.StandardOutput.ReadToEnd();
bool exited = process.WaitForExit(10_000);

// ❌ BROKEN — pipe buffer fills if help/list output > ~64 KB
process.Start();
bool exited = process.WaitForExit(10_000);   // deadlock for large output
```

**Async alternative** (when you also need to time out independently):
```csharp
process.Start();
process.BeginOutputReadLine();  // async drain, discards data
process.BeginErrorReadLine();   // drain stderr too
bool exited = process.WaitForExit(10_000);
```

**Rule**: Any test that passes `RedirectStandardOutput = true` to `ProcessStartInfo` **must**
drain stdout (and stderr if redirected) before or during `WaitForExit`. This applies to all
smoke/integration tests that launch the CLI or GUI exe.

---

## Cross-Assembly Test Race Condition — NEVER Use `dotnet test RegiLattice.sln`

`dotnet test RegiLattice.sln` spawns all three test assemblies (Core.Tests, CLI.Tests,
GUI.Tests) as **separate processes that run concurrently** on CI. Tests in different
assemblies that write to the same on-disk file cause non-deterministic failures:

| Failing test | Root cause |
|---|---|
| `GetHistory_NullJsonFile_ReturnsEmpty` | `ComplianceHistoryNullJsonBranchTests` wrote "null" to file; concurrently running `ComplianceTrendDialogTests` (GUI.Tests) overwrote it with real entries |
| `TotalViolationsInLast_PartialCount_SumsOnlyLastN` | `ComplianceHistoryTests` added 5 entries; GUI.Tests tear-down truncated the file mid-test |

**Fix (applied v5.52.0)**: Run each test project individually so they execute sequentially:

```yaml
# ✅ CORRECT — avoids cross-assembly file races
- name: Test (Core)
  run: dotnet test tests/RegiLattice.Core.Tests/RegiLattice.Core.Tests.csproj ...
- name: Test (CLI)
  run: dotnet test tests/RegiLattice.CLI.Tests/RegiLattice.CLI.Tests.csproj ...
- name: Test (GUI)
  run: dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj ...

# ❌ BROKEN — runs Core.Tests and GUI.Tests concurrently
- name: Test
  run: dotnet test RegiLattice.sln ...
```

`tests/.runsettings` has `<MaxCpuCount>1</MaxCpuCount>` and a comment explaining this.
The `xunit.runner.json` in tests/ has `"parallelizeAssembly": false` — but this only
controls parallelism WITHIN one assembly, not across test runner processes.

**Rule**: Every new test file that writes to disk (ratings, favorites, analytics,
compliance history, etc.) MUST use `IDisposable` + temp-file or `[Collection("X")]`
isolation. Files in `%LOCALAPPDATA%\RegiLattice\` are shared across all test assemblies.

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

**What to update on each version bump (28 files, listed in order)**:

> **Count fields across ALL files**: tweaks · categories · modules · tests · themes (11) · profiles (5) · pkg-managers (5)
> Themes, profiles, and pkg-manager counts ONLY change when those features are added/removed — not on every bump.

**Group A — Version properties (EVERY version bump)**:

| # | File                                                | What changes                                                                                          |
| - | --------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| 1 | `Directory.Build.props`                             | All 4 version properties: `<Version>`, `<AssemblyVersion>`, `<FileVersion>`, `<InformationalVersion>` |
| 2 | `installer/Package.wxs`                             | `Version="X.Y.Z"` (no `.0` suffix here)                                                               |

**Group B — SVG graphics (when tweak/category/test/module counts change)**:

| # | File                                                | What changes                                                                                          |
| - | --------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| 3 | `docs/assets/stats.svg`                             | Tweaks count + categories count + tests count (space-separated thousands e.g. `7 189`)               |
| 4 | `docs/assets/banner.svg`                            | Tweaks count · categories count · tests count · themes count · profiles count                        |
| 5 | `docs/assets/features.svg`                          | Per-category tweak count badges (Privacy, Performance, Security, Debloat, Dev Tools)                 |
| 6 | `docs/assets/architecture.svg`                      | Stats badge + category pills: tweak count · category count · module file count                        |
| 7 | `docs/assets/how-it-works.svg`                      | Tweaks count in Browse step                                                                           |
| 8 | `docs/assets/project-structure.svg`                 | Summary line: file count · tweak count · category count                                               |
| 9 | `docs/assets/solution-overview.svg`                 | Core engine metrics: file count · tweak count                                                         |

**Group C — Documentation & instruction files**:

| # | File                                                | What changes                                                                                          |
| - | --------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
|10 | `README.md`                                         | Version badge, test badge, download link, description line, features bullet, diagram counts, module count |
|11 | `CHANGELOG.md` (root stub)                          | Latest version entry summary                                                                          |
|12 | `docs/CHANGELOG.md`                                 | Prepend new `## [X.Y.Z]` section with Stats line                                                      |
|13 | `docs/Development.md`                               | Header "Last updated" date + version                                                                  |
|14 | `docs/Roadmap.md`                                   | Baseline counts (tweaks · categories · files · tests · themes) if they changed                         |
|15 | `.github/copilot-instructions.md`                   | Header line, version table row, tweaks/categories/modules/tests row                                   |
|16 | `.github/instructions/workspace.instructions.md`    | Tweaks/module class count in `Tweaks/` directory comment                                              |
|17 | `.github/instructions/lessons-learned.instructions.md` | Header date + version + tweak/category/test counts                                                 |
|18 | `.github/instructions/testing.instructions.md`      | Test project counts table (Core/CLI/GUI/Total)                                                        |
|19 | `.github/agents/regilattice.agent.md`               | "Current state" line: tweak/category/module/test counts                                               |

**Group D — Package registry manifests (version + description counts)**:

| # | File                                                | What changes                                                                                          |
| - | --------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
|20 | `chocolatey/regilattice.nuspec`                     | `<version>`, `<summary>`, description counts                                                          |
|21 | `scoop/regilattice.json`                            | `version`, `url` (both under `architecture.64bit` and `autoupdate`), description                      |
|22 | `winget/RegiLattice.RegiLattice.yaml`               | `PackageVersion`                                                                                      |
|23 | `winget/RegiLattice.RegiLattice.installer.yaml`     | `PackageVersion`, `InstallerUrl`                                                                      |
|24 | `winget/RegiLattice.RegiLattice.locale.en-US.yaml`  | `PackageVersion`, `ShortDescription`, `Description` counts                                            |
|25 | `npm/package.json`                                  | `version`, `description` counts                                                                       |
|26 | `maven/pom.xml`                                     | `<version>`, `<description>` counts                                                                   |
|27 | `powershell/RegiLattice.psd1`                       | `ModuleVersion`                                                                                       |

**Group E — Derived files (version in filename/URL, update AFTER release build)**:

| # | File                                                | What changes                                                                                          |
| - | --------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
|28 | `gem/lib/regilattice/version.rb`                    | `VERSION` string                                                                                      |
|29 | `Dockerfile`                                        | `LABEL` description counts                                                                            |

**Group F — External (post-push, after CI publishes release)**:

| # | Action                                              | What changes                                                                                          |
| - | --------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
|30 | GitHub About sidebar                                | `gh repo edit RajwanYair/RegiLattice --description "... N,NNN tweaks ..."` — update tweak count       |

**Release artifact naming convention (from v6.2.0 onward)** — all EXE, MSI, and MSIX files
include the version tag in their filename. The `release.yml` "Rename artifacts for release" step
handles this automatically. The naming pattern to verify in each release:

| Artifact               | Versioned filename pattern          |
| ---------------------- | ----------------------------------- |
| GUI EXE (portable)     | `RegiLattice-vX.Y.Z-win-x64.exe`   |
| CLI EXE (portable)     | `RegiLatticeCLI-vX.Y.Z-win-x64.exe` |
| MSI installer          | `RegiLattice-vX.Y.Z-win-x64.msi`   |
| MSIX package           | `RegiLattice-vX.Y.Z-win-x64.msix`  |
| ZIP (Chocolatey input) | `RegiLattice-vX.Y.Z-win-x64.zip`   |
| Checksums              | `SHA256SUMS.txt`                    |

The MSIX and ZIP filenames are set directly in the build scripts that produce them.
The MSI rename searches `installer/bin` recursively (not `installer/bin/Release`) because
WiX SDK with `InstallerPlatform=x64` outputs to `installer/bin/x64/Release/`.

**GitHub About sidebar** — update via CLI after every version bump that changes counts.
Replace `N,NNN tweaks` with the actual new count:

```powershell
gh repo edit RajwanYair/RegiLattice --description "Windows 10/11 registry tweaks toolkit — 9,240 tweaks, debloater, privacy hardening, performance optimizer, security hardening, group policy alternative, compliance audit. WinForms GUI + CLI. .NET 10, C# 13. Open source."
```

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

## `PublishTrimmed` Breaks Reflection-Based Services in RegiLattice.Core

Setting `<PublishTrimmed>true</PublishTrimmed>` in any project that references `RegiLattice.Core`
produces **48+ IL2026 trim-analysis errors** during self-contained publish. The root cause is
that `RegiLattice.Core` uses `System.Text.Json` reflection-based serialization in at least 9 services:
`Analytics`, `AppConfig`, `Favorites`, `TweakHistory`, `PackManager`, `PipManager`, `PluginSandbox`,
`ComplianceHistory`, `StartupManager`.

**Error form**: `IL2026 Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access ...`

**Why the wrong fix was tried**: `CliJsonContext.cs` already provides a source-generation `JsonSerializerContext`
for the CLI's own dispatch results. It was assumed this would make the whole project trim-safe.
But a source-gen context in the CLI project does NOT cover types serialized in Core services —
those still hit the reflection path.

```xml
<!-- ❌ BAD — causes 48 IL2026 errors on CLI self-contained publish -->
<PublishTrimmed>true</PublishTrimmed>
<TrimMode>partial</TrimMode>
<JsonSerializerIsReflectionEnabledByDefault>true</JsonSerializerIsReflectionEnabledByDefault>

<!-- ✅ CORRECT — remove all three; EXE is slightly larger but all services work -->
<!-- (no entry needed — just omit these elements entirely from the csproj) -->
```

**Rule**: Do NOT add `<PublishTrimmed>true</PublishTrimmed>` to any project that references
`RegiLattice.Core`. To enable trimming, every service in Core that uses `JsonSerializer` would
need to be migrated to source-generation contexts first — a major refactor.

**Note**: `<InvariantGlobalization>true</InvariantGlobalization>` is safe to keep — it saves
ICU bundle size without affecting reflection or serialization.

**Found in**: v6.23.0 release — CLI EXE build failed with 48 trim errors. Fixed by removing
the three trim-related elements from `RegiLattice.CLI.csproj`.

---

## SnapshotManager.Save() Calls Live StatusMap() — Hangs on 7k+ Tweaks

`TweakEngine.SaveSnapshot()` (and `SnapshotManager.Save()`) originally called `StatusMap()` synchronously
inside the save path to embed live tweak detections. With 7,000+ tweaks this takes 30+ seconds
and caused the `Scenario5_SnapshotRoundTrip` E2E test to hang.

**Where it manifested**: The Phase 4.1 E2E test called `engine.SaveSnapshot(path)` mid-test.
At 7,189 tweaks × ~4ms average detection time = ~28s blocking call.

**Fix pattern** (same as `ExportJson` which already supports this):
```csharp
// ❌ BAD — triggers live StatusMap() on all 7k+ tweaks inside Save()
engine.SaveSnapshot("snapshot.json");

// ✅ GOOD — pass the UI layer's already-computed status map
var cached = engine.StatusMap(parallel: true);   // computed once, reused everywhere
engine.SaveSnapshot("snapshot.json", cachedStatus: cached);
engine.ExportJson("export.json", cachedStatus: cached);   // same pattern
```

The `cachedStatus` parameter is optional (`IReadOnlyDictionary<string, TweakResult>?`).
When `null`, the method falls back to live detection (backward compatible).

**Rule**: Any Core method that computes or persists "current status of all tweaks" should:
1. Accept an optional `cachedStatus` parameter
2. Use `cachedStatus ?? engine.StatusMap(parallel: true)` internally
3. The UI/CLI layer pre-computes status once and passes it to all downstream calls

**Found in**: v6.22.0 — `Scenario5_SnapshotRoundTrip` E2E test was hanging.

---

## PowerShell `ApplyAction` Scripts Must Be Validated Before Committing

Three complex `ApplyAction` delegates were found to be silently broken in v6.23.0, having
compiled and even passed registration without errors but failing at runtime:

### 1. Embedded `#` comment breaks a PS inline script string

```csharp
// ❌ BAD — '#' inside a double-quoted PS string terminates the expression
ApplyAction = (_) => ShellRunner.RunPowerShell(
    "$shell = New-Object -ComObject Shell.Application  # this comment breaks execution")

// ✅ GOOD — put the comment on its own line BEFORE the code
ApplyAction = (_) => ShellRunner.RunPowerShell(
    "# Get Recycle Bin via Shell.Application\n$shell = New-Object -ComObject Shell.Application")
```

### 2. secedit full-template reset instead of targeted INF

`uac-enforce-password-complexity` called `secedit /configure /db secedit.sdb /cfg %windir%\inf\defltbase.inf`
which applies the **entire Windows default security template** (resetting all password and account policies).
Only `PasswordComplexity=1` was needed.

**Fix**: Write a minimal INF file containing only the targeted setting, then call `secedit /configure` on it.

```powershell
# ✅ CORRECT — minimal targeted secedit INF
$inf = "[Unicode]`nUnicode=yes`n[System Access]`nPasswordComplexity = 1"
$tmp = "$env:TEMP\pwdcmplx.inf"; $inf | Set-Content $tmp -Encoding Unicode
secedit /configure /db "$env:TEMP\secedit_temp.sdb" /cfg $tmp /quiet
```

### 3. Wrong Storage API (subsystem policy vs per-disk write caching)

`ssd-enable-write-caching` called `Set-StorageSetting -NewDiskPolicy OnlineAll` which is a
**disk initialization policy** (whether new disks come online automatically), not disk write cache control.

**Fix**: Use `Get-PhysicalDisk | Set-PhysicalDisk -WriteCacheEnabled $true` (per-disk granularity).

### Prevention rules

- **Validate inline PS scripts separately** before embedding them in `ApplyAction` — paste into a PS prompt and verify execution.
- **Validate `DetectAction` reads the right output** — if a tool writes to a file (not stdout), `DetectAction` must read from that file, not `process.StandardOutput`.
- **Search for the exact cmdlet** that controls the specific setting before coding the tweak — Windows has many cmdlets with similar names that do different things (`Set-StorageSetting` ≠ `Set-PhysicalDisk`).
- **Prefer RegOp-only tweaks** where possible — declarative ops are statically verifiable and far less likely to have hidden bugs.

**Found in**: v6.23.0 — systematic review of non-registry TweakKind implementations.

---

## CI Mutation Testing — Move to Weekly Schedule to Avoid Per-Push Overhead

Running `dotnet-stryker` mutation tests on every push to `main` adds ~15 minutes per CI
run (7,000+ tweaks × mutation round-trips). For a single-dev repository with frequent
commits this makes the feedback loop impractical.

**Fix**: Move mutation testing from `on: push` to a weekly cron schedule in `ci.yml`:

```yaml
# ❌ SLOW — mutations run on every push to main (~15 extra minutes)
on:
  push:
    branches: [main]

# ✅ FAST — mutations run weekly on Sunday night
on:
  schedule:
    - cron: '0 2 * * 0'   # 02:00 UTC Sunday
  workflow_dispatch:        # keep manual trigger
```

**Also**: Add `paths-ignore` to the main CI workflow for docs-only changes to avoid rebuilding
and testing when only `.md`, `.svg`, or `.txt` files changed:

```yaml
on:
  push:
    branches: [main]
    paths-ignore:
      - 'docs/**'
      - '**.md'
      - '**.svg'
      - '**.txt'
      - '.github/instructions/**'
```

**Found in**: v6.23.0 — CI run took 30+ minutes per push due to mutation testing. Weekly
schedule reduces per-push CI time from ~30 min to ~5–8 min.

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
| v5.27.0 | 5 | 50 | 292–296 |
| v5.28.0 | 5 | 50 | 297–301 |
| v5.29.0 | 5 | 50 | 302–306 |
| v5.30.0 | 5 | 50 | 307–311 |
| v5.31.0 | 5 | 50 | 312–316 |
| v5.32.0 | 5 | 50 | 317–321 |
| v5.33.0 | 5 | 50 | 322–326 |
| v5.34.0 | 5 | 50 | 327–331 |
| v5.35.0 | 5 | 50 | 332–336 |
| v5.36.0 | 5 | 50 | 337–341 |
| v5.37.0 | 5 | 50 | 342–346 |
| v5.38.0 | 5 | 50 | 347–351 |
| v5.39.0 | 5 | 50 | 352–356 |
| v5.40.0 | 5 | 50 | 357–361 |
| v5.41.0 | 5 | 50 | 362–366 |
| v5.42.0 | 5 | 50 | 367–371 |
| v5.43.0 | 5 | 50 | 372–376 |
| v5.44.0 | 5 | 50 | 377–381 |
| v5.45.0 | 5 | 50 | 382–386 |
| v5.46.0 | 5 | 50 | 387–391 |
| v5.47.0 | 5 | 50 | 392–396 |
| v5.48.0 | 5 | 50 | 397–401 |
| v5.49.0 | 5 | 50 | 402–406 |
| v5.50.0 | 5 | 50 | 407–411 |
| v5.51.0 | 5 | 50 | 412–416 |
| v5.52.0 | 5 | 50 | 417–421 |
| v5.53.0 | 5 | 50 | 422–426 |
| v5.54.0 | 5 | 50 | 427–431 |
| v5.55.0 | 5 | 50 | 432–436 |
| v5.74.0 | 94 | 940 | 437–531 |
| v5.75.0 | 5 | 50 | 532–536 |
| v5.76.0 | 5 | 50 | 537–541 |
| v5.77.0 | 5 | 50 | 542–546 |
| v5.78.0 | 5 | 50 | 547–551 |
| v5.79.0 | 5 | 50 | 552–556 |
| v5.80.0 | 5 | 50 | 557–561 |
| v5.81.0 | 5 | 50 | 562–566 |
| v5.82.0 | 5 | 50 | 567–571 |
| v5.83.0 | 5 | 50 | 572–576 |
| v5.84.0 | 5 | 50 | 577–581 |
| v5.85.0 | 5 | 50 | 582–586 |
| v5.86.0 | 5 | 50 | 587–591 |
| v5.87.0 | 5 | 50 | 592–596 |
| v5.88.0 | 5 | 50 | 597–601 |
| v5.89.0 | 5 | 50 | 602–606 |
| v5.90.0 | 5 | 50 | 607–611 |
| v5.91.0 | 5 | 50 | 612–616 |
| v5.92.0 | 5 | 50 | 617–621 |
| v5.93.0 | 5 | 50 | 622–626 |
| v5.94.0 | 5 | 50 | 627–631 |
| v5.95.0 | 0 | -21 | — (consolidation: removed 21 duplicates) |
| v5.96.0 | 5 | 50 | 632–636 |
| v5.97.0 | 0 | -313 | — (consolidation: removed 313 cross-module duplicate tweaks) |
| v5.98.0 | 0 | 0 | — (consolidation: merged 552 policy modules into 24 domain files; 665→130 modules, 637→135 categories) |
| v5.99.0 | 0 | -3 | — (consolidation: merged 32 secondary modules into 17 primary files; 130→98 modules, 135→101 categories) |
| v6.0.0 | 0 | 0 | — (consolidation: merged 15 more secondary modules; 98→83 modules, 101 categories unchanged) |
| v6.2.0 | 5 | 50 | 637–641 (Identity.cs: PolicyFido/WindowsHello/EntraId/Kerberos/AppInstaller) |
| v6.3.0 | 5 | 50 | 642–646 (SystemPolicy.cs: PolicyBitLocker/WindowsInk/LocationSensors/CloudClipboard/NetworkIsolation) |
| v6.4.0 | 5 | 50 | 647–651 (PolicyConfig.cs: PolicyFirewallProfiles/NetLogon/ReliabilityMonitor/DNSSecurity/SmartScreenWin) |
| v6.5.0 | 5 | 50 | 652–656 (PolicyUser.cs: PolicyWindowsSearch/AppPrivacy/UserExperience/EventLogAudit/SyncSettings) |
| v6.6.0 | 5 | 50 | 657–661 (PolicyMisc2.cs: WindowsFeeds/CompressedFolders/WindowsChat/TextInputExt/SpeechInput) |
| v6.7.0 | 5 | 50 | 662–666 (PolicyMisc3.cs: AutoPlay/Store/LockScreen/RemoteAssistance/SmartCard) |
| v6.8.0 | 0 | -643 | — (consolidation: 15 file merges, 20 category renames, 643 duplicate tweaks removed; 45→30 files, 47→26 categories, 9490→8847 tweaks) |
| v6.9.0 | 0 | 0 | — (doc consolidation: Architecture.md/Development.md/README.md stale counts fixed; git-workflow updated with SVG update mandate) |
| v6.10.0 | 0 | -44 | — (Phase C: removed 44 scoop tool-install tweaks from Developer.cs; 8,847→8,803 tweaks) |
| v6.11.0 | 5 | +50 | 667–671 (PolicyLocation/PolicyDataCollection/PolicyWinRM/PolicyCredentialUI/PolicyMediaPlayer) |
| v6.12.0 | 0 | -1,664 | — (mass dedup: removed 1,756 duplicate TweakDef blocks, kept alphabetically-first module; 8,853→7,189 tweaks; 26→23 categories; 35→31 modules) |
| v6.13.0 | 0 | 0 | — (file split: 31 merged tweak files → 146 individual files via multi-class extraction + partial class splits; no tweak count change) |
| v6.14.0 | 0 | 0 | — (Phase 1 Engine Hardening: TweakRisk flags, search relevance ranking, CancellationToken batch support, TransactionalBatchResult, RegistrySession.ExecuteWithDiff; 7,189 tweaks, 3,092 tests) |
| v6.15.0 | 0 | 0 | — (Phase 1.6+1.7: custom profile API (9 TweakEngine methods + UserProfileService), recommendation engine (TweakRecommendation.ConfidencePercent + IsQuickWin); 3,105 tests) |
| v6.16.0 | 0 | 0 | — (Phase 3.1/3.3/3.4: CLI --json output, conditional apply flags, interactive wizard; fix: --json mode outputs [] for no results; 3,105 tests) |
| v6.18.0 | 0 | 0 | — (Phase 2.2+2.5: KeyboardShortcutsDialog (19 shortcuts/4 groups), context menu 6→11 items (Favorite/CopyRegPath/OpenRegedit/Dependencies/History); +24 Core tests; 3,190 total) |
| v6.19.0 | 0 | 0 | — (Phase 2.3+2.4: risk confirmation dialog (ConfirmApplyDialog + ConfirmApplyThreshold), batch ETA (TweakDef.EstimatedApplyTimeMs per-kind); +28 tests; 3,218 total) |
| v6.20.0 | 0 | 0 | — (Phase 2.6+3.6: user JSON themes with FileSystemWatcher hot-reload, Ansible win_regedit YAML + DSC .ps1 export; 2,421 tests) |
| v6.21.0 | 0 | 0 | — (Phase 4.1+4.6: 13 E2E+concurrent tests — full lifecycle, profiles, DryRun, snapshots, JSON export, dep chain, CorporateGuard, concurrent StatusMap/ApplyBatch; 2,434 tests total) |
| v6.22.0 | 8 | +80 | 672–679 (SecurityWDAG/Printer/LSA/MSI/NTP/WinRM/CredGuard/IEZones; fix SnapshotManager.Save() live StatusMap() hang via optional cachedStatus param) |
| v6.23.0 | 8 | +80 | 680–687 (GamingDirectStorage/VRR/LatencyTuning/GPUPower/NetworkOpt/AudioOpt/AccessibilityMotor/AccessibilityVisual; CI paths-ignore + weekly mutation tests; fix 3 broken ApplyAction impls; 7,349 tweaks) |
| v6.24.0 | 8 | +80 | — (Phase 5.3 Accessibility remaining + Phase 5.5 Developer: MagnifierAdvanced/LiveCaptions/EyeControlSettings/VoiceAccessControl/WinDbgSettings/WSLAdvanced/GitCredManager/ContainerRuntime) |
| v6.25.0 | 5 | +50 | — (Phase 5.4 Energy & Battery Management: BatterySaver/ChargingOptimization/StandbyStates/CPUPowerStates/DisplayPower; 7,479 tweaks, 127 categories, 175 modules) |
| v6.26.0 | 3 | +39 | Sprint 688 — 3 new Office GP security modules (PolicyOfficeWord/PolicyOfficeExcel/PolicyOfficeOutlook) + PolicyWindowsSearch expanded 1→10 tweaks; deleted 3 empty stubs; 7,518 tweaks, 127 categories, 175 modules |
| v6.27.0 | 0 | 0 | — (Phase 6.1–6.3: TweakHistory audit logging + ExportCsv, HealthScoreService.CategoryHealthScores, ConflictDetector.DetectRegistryConflicts + ConflictSeverity/RegistryConflict; fix .runsettings HangTimeout warning; +20 tests → 3,259 total) |
| v6.28.0 | 5 | +50 | — (Phase 6.4 ScheduledTweakService + Phase 6.5 TweakMigrationService + TweakEngine.Migrations/ResolveMigration + SnapshotManager auto-migrate; 5 new Office Policy modules: PowerPoint/Access/Publisher/Visio/Project; +32 tests → 3,291 total; 7,568 tweaks, 180 modules) |
**Current version**: v6.28.0 — 7,568 tweaks, 127 categories, 180 files (31 original + 149 extracted/split). Run full gap analysis on all three phases before creating any new module.

---

## GitHub Actions — Non-Existent Version Tags Break CI Silently

Pinning a GitHub Action to a version that does not exist (e.g., `actions/upload-artifact@v7`)
causes the CI step to fail with a confusing message — the step runner can't find the
action, but the error is not always obvious from the log.

**Caught in v5.49.0**: `upload-artifact@v7` was pinned in `ci.yml` and `debug.yml`. v7 does
not exist; the latest stable at that time was `@v4`. Fixed to `@v4`.

**Caught in v5.74.0**: All 6 active workflows had a cluster of non-existent versions —
`actions/checkout@v6`, `actions/setup-dotnet@v5`, `actions/cache@v5` (none of these major
versions exist; latest stable is `@v4` for all three), plus `github/codeql-action/init@v4`,
`github/codeql-action/analyze@v4`, and `github/codeql-action/upload-sarif@v4` (v4 does not
exist for codeql-action; latest stable is `@v3`). Fixed in commit `bc51a02`.

**Canonical stable versions (verified 2026-03-29)**:

| Action | Latest Stable |
|--------|--------------|
| `actions/checkout` | `@v6` |
| `actions/setup-dotnet` | `@v5` |
| `actions/cache` | `@v5` |
| `actions/upload-artifact` | `@v7` |
| `github/codeql-action/init` | `@v4` |
| `github/codeql-action/analyze` | `@v4` |
| `github/codeql-action/upload-sarif` | `@v4` |
| `actions/dependency-review-action` | `@v4` |
| `actions/labeler` | `@v6` |
| `actions/github-script` | `@v8` |
| `actions/stale` | `@v10` |
| `codecov/codecov-action` | `@v6` |

**Rule**: Before committing any new or bumped action version, verify it exists:

1. Check the action's GitHub releases page (e.g., `https://github.com/actions/upload-artifact/releases`)
2. Or run `gh release list --repo actions/upload-artifact`

```yaml
# ✅ Verified — actions/upload-artifact@v4 is the latest stable
- uses: actions/upload-artifact@v4

# ❌ Silently broken — v7 does not exist
- uses: actions/upload-artifact@v7
```

---

## CI Job Status Variable — `$env:JOB_STATUS` Not `$env:RUNNER_STATUS`

When writing a job summary or post-step in GitHub Actions via PowerShell, the variable
`$env:RUNNER_STATUS` does **not exist**. The correct approach is to inject `job.status`
as an explicit env var and reference `$env:JOB_STATUS`.

**Caught in v5.49.0**: The CI write-job-summary step produced an empty status line
because `$env:RUNNER_STATUS` expanded to the empty string.

```yaml
# ✅ CORRECT — inject job.status explicitly
- name: Write Job Summary
  if: always()
  env:
    JOB_STATUS: ${{ job.status }}
  shell: pwsh
  run: Write-Host "Job completed with status: $env:JOB_STATUS"

# ❌ BROKEN — RUNNER_STATUS does not exist
- name: Write Job Summary
  shell: pwsh
  run: Write-Host "Status: $env:RUNNER_STATUS"   # always empty
```

---

## `TestSessionTimeout` Must Be ≥ 300 000ms for Large Test Suites on OneDrive

The default `TestSessionTimeout` in earlier `.runsettings` was `60000` (60 s). With
700+ tests in `RegiLattice.Core.Tests`, the OneDrive-hosted build takes **~46 seconds**
just loading the test assembly — leaving only ~14 s for all 2100+ tests. This caused
sporadic "Test Run Aborted" failures that appeared as non-deterministic CI failures.

**Fixed in v5.52.0**: `TestSessionTimeout` raised to `300000` (300 s) in
`tests/.runsettings`. This leaves ample time for the assembly to load and all tests to
execute even on a slow OneDrive-backed drive.

```xml
<!-- ✅ CORRECT — 5-minute session budget for OneDrive-hosted builds -->
<TestSessionTimeout>300000</TestSessionTimeout>

<!-- ❌ Too tight — causes "Test Run Aborted" when test assembly loads in ~46s -->
<TestSessionTimeout>60000</TestSessionTimeout>
```

**Rule**: If a CI run fails with "Test Run Aborted" (not a test failure, but the runner
itself timing out), increase `TestSessionTimeout` by 5× before diagnosing test code.

---

## Package Registry Metadata Files Age Silently — Add to Version Bump Checklist

`scoop/regilattice.json`, `winget/RegiLattice.*.yaml`, and
`chocolatey/regilattice.nuspec` are maintained **manually and independently** from
`Directory.Build.props`. They contain version numbers, tweak counts, category counts,
and download URL templates that must match the release.

**Found in v5.55.0 stale audit**: These files were stuck at v5.0.0 for 54 minor
versions — across 54 MINOR version bumps and thousands of new tweaks, none of the
package registry files were ever updated. The URL templates still pointed to the
`v5.0.0` release asset.

**Prevention**: These files are now added to the version bump checklist (see
`git-workflow.instructions.md` — "What to update on each version bump"):

| File | Fields to update |
|---|---|
| `scoop/regilattice.json` | `version`, `url`, `hash` (after release), description counts |
| `winget/RegiLattice.RegiLattice.yaml` | `PackageVersion` |
| `winget/RegiLattice.RegiLattice.installer.yaml` | `PackageVersion`, `InstallerUrl` |
| `winget/RegiLattice.RegiLattice.locale.en-US.yaml` | `PackageVersion`, description counts |
| `chocolatey/regilattice.nuspec` | `<version>`, description counts, locale list |

**Rule**: After every version bump, update all five files above BEFORE pushing the tag.

---

## README Mermaid Diagram Counts Are Hardcoded and Drift

The Mermaid diagram in `README.md` uses hardcoded module and category counts:

```mermaid
   RL["RegiLattice Core
 461 Modules, 464 Categories"]
```

There is no automation to update these counts. Every sprint that adds modules WILL
cause the diagram to show a stale count unless manually updated.

**Found in v5.55.0 stale audit**: Diagram showed `446 Modules` when the actual
count was `461` (a drift of 15 modules over ~15 sprints).

**Rule**: When adding new tweak modules in a sprint:
1. Update the Mermaid diagram module count (e.g., `461 Modules` → `466 Modules` when adding 5 new modules)
2. Update the Mermaid category count if new categories are added
3. These are separate from the `docs/assets/stats.svg` counts — both must be updated

**Quick check command** (run after any sprint to verify diagram vs reality):
```powershell
$actual = (Get-ChildItem src/RegiLattice.Core/Tweaks/*.cs).Count
$inDiagram = (Select-String -Path README.md -Pattern '(\d+) Modules').Matches[0].Groups[1].Value
Write-Host "Actual modules: $actual | Diagram shows: $inDiagram"
```

---

## xUnit v2 Test Stack — Intentional Version Holds Against Major Updates

The test packages have newer major versions available but are intentionally held at
v2-compatible versions:

| Package | Pinned | Latest | Reason for hold |
|---|---|---|---|
| `Microsoft.NET.Test.Sdk` | 17.14.1 | 18.x | New test-host protocol; v18 breaks `.runsettings` format |
| `xunit` | 2.9.3 | 3.x | xUnit v3 has new test class model — breaking migration |
| `xunit.runner.visualstudio` | 2.8.2 | 3.x | v3 runner only works with xUnit v3 framework |
| `FsCheck` | 2.16.6 | 3.x | FsCheck v3 changes attribute API and runner integration |
| `FsCheck.Xunit` | 2.16.6 | 3.x | Same as FsCheck |

**Rule**: These packages ONLY need updating as part of a dedicated xUnit v3 migration
sprint — not during routine "update dependencies" maintenance. Attempting to bump
any one of them in isolation will break the test build.

**Comments in `Directory.Packages.props`** track these holds with the verification date.
When undertaking the migration, update ALL of the above packages in one commit.

---

## `.gitignore` Must Include Stryker and BenchmarkDotNet Output Directories

Local runs of mutation testing or BenchmarkDotNet create output directories that
should never be tracked:

```gitignore
# ✅ Add to .gitignore
**/StrykerOutput/
BenchmarkDotNet.Artifacts/
```

**Caught in v5.49.0**: Running `dotnet-stryker` locally produced `StrykerOutput/`,
and running `RegiLattice.Benchmarks` produced `BenchmarkDotNet.Artifacts/`. Both were
appearing as untracked files in `git status` and polluting the working tree.

**Rule**: Every tool that generates output to project-relative directories needs a
`.gitignore` entry. This is most relevant for: Stryker, BenchmarkDotNet,
`coveragereport/`, `htmlcov/`, and any tool that writes to the workspace root.

---

## `FORCE_JAVASCRIPT_ACTIONS_TO_NODE24` — Required for Modern GitHub Actions

GitHub Actions that use JavaScript internally (e.g., `actions/checkout`,
`actions/cache`, `codecov/codecov-action`) upgraded from Node 16/20 to Node 24. Setting
the environment variable `FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true` at the job level
ensures these actions run on Node 24 without showing deprecation warnings.

```yaml
# ✅ Set at job or workflow level for all CI, release, and debug jobs
env:
  FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true
```

**When this matters**: Without the variable, GitHub Actions logs may show:
`"Node.js 16 actions are deprecated. Please update..."` which can obscure real errors.
Adding the variable to `env:` at the job level (not just inside steps) ensures it
applies to all steps including setup steps.

**Current state**: `FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true` is set in `ci.yml`,
`release.yml`, and `debug.yml` at the job level. It is NOT needed in `stale.yml` or
`dependency-review.yml` (those use GitHub-managed actions that handle this internally).

---

## CHANGELOG Date Discipline — Use Actual Commit Date, Not Planned Date

When multiple sprint versions are written in a planning session or committed on the same
day, there is a temptation to use the day of the sprint (planned or future date) instead
of the actual commit date. This leads to CHANGELOG entries with future dates.

**Found in v5.55.0 stale audit**: `v5.53.0` and `v5.54.0` had dates `2026-04-18` and
`2026-04-19` respectively, but the audit was run on `2026-03-29` — making those dates
3 weeks in the future. The `copilot-instructions.md` header also read `"Last verified:
2026-04-19"` which was a future date.

**Rule**: Always use `Get-Date -Format "yyyy-MM-dd"` to get today's actual date when
writing CHANGELOG entries. If multiple releases are committed on the same day, they
share the same date.

```powershell
# Get today's date for CHANGELOG header
Get-Date -Format "yyyy-MM-dd"
```

**Why it matters**: Future dates in CHANGELOG cause confusion when auditing the project
history ("when was this actually shipped?") and may break date-parsing tools or
release notes generators.

---

## Hardcoded Major Version in Tests — Use Dynamic Assertions

`ExecutableValidationTests` had `Assert.StartsWith("5.", ...)` and `Assert.Equal(5, version.Major)`
hardcoded for three EXE/DLL version checks and two loaded-assembly checks. When the version
bumped from v5.x to v6.0.0, all five assertions failed and broke the CI release workflow.

```csharp
// ❌ BAD — hardcoded major version breaks on next major bump
Assert.StartsWith("5.", versionInfo.FileVersion);
Assert.Equal(5, version.Major);

// ✅ GOOD — reads expected major from loaded Core assembly
var expectedMajor = typeof(RegiLattice.Core.TweakEngine).Assembly.GetName().Version!.Major;
Assert.StartsWith($"{expectedMajor}.", versionInfo.FileVersion);
Assert.True(version.Major >= 5, $"Major version {version.Major} unexpectedly low");
```

**Rule**: Never hardcode version numbers in tests. Read the expected version dynamically
from the loaded assembly or `Directory.Build.props`. This applies to any test that
validates PE metadata, assembly version, or file version info.

---

## Tweak Files Were Merged From Multiple Original Files — Not One Large Class

The large files in `src/RegiLattice.Core/Tweaks/` (e.g., `Browser.cs 6410L`,
`Maintenance.cs 7758L`, `Storage.cs 6007L`) appeared to be one large class with many
inner classes. They were actually **multiple independent top-level `internal static class`
declarations merged into one file** during a prior consolidation sprint.

**Symptom that revealed this**: `Storage.cs` had a `Tweaks` property at lines 7, 380,
801, 5122, 5451, 5638 — each in a different top-level class (Storage, FileSystem,
SsdOptimization, etc.).

**Why a line-level partial class split fails on merged files**: If you split at line N
and mark both halves as `partial class Storage`, but the original file's line N+1 starts
a completely different top-level class declaration (`internal static class FileSystem`),
the compiler sees a nested partial that doesn't exist — cascading CS0103 errors on every
inner-class name that the outer `Storage` class doesn't know about.

**Correct approach**: Find all top-level `internal static class` declarations, extract
each into its own file. Leave the first class in the original file; write subsequent classes
to new files with the same namespace and using directives.

**Detection command** — run before deciding how to split a large file:
```powershell
# How many top-level classes does this file have?
Select-String -Path SomeFile.cs -Pattern '^internal static (partial )?class \w+'
```

If count > 1 → multi-class extraction (see `Split-MultiClass.ps1` in `.tmp/`).
If count = 1 → partial class split (see `Split-Partials.ps1` in `.tmp/`).

---

## Brace Counting for C# Class Boundary Detection Is Unreliable

Counting `{` and `}` characters to find class end-boundaries in C# source fails because:
1. Registry path string literals contain `{GUID}` patterns counted as opening braces
2. Verbatim string literals `@"..."` may span multiple lines with braces
3. String interpolation `$"...{expr}..."` adds unbalanced braces inside strings

**Symptom**: A brace-counting script extracted an "inner class" that ended 200 lines
before the real closing brace, leaving constants and helper methods stranded.

**Correct approaches for boundary detection**:
- **Top-level class**: Match `^internal static (partial )?class \w+` (starts at column 0)
- **Inner class**: Match `^\s{4}private static class _\w+` (exactly 4-space indent)
- **End of top-level class**: The LAST `^}` (no indent) in the file = outer class close
- **Split near midpoint**: Find the inner class start closest to `lineCount / 2`

**Never use brace counting** unless you have a full C# parser available.

---

## Partial Class Splitting Requires the `partial` Keyword in Both Files

When splitting a single-class file into `Filename.cs` + `Filename.Part2.cs`, both
files must declare the class as `partial`. Missing the `partial` keyword on either file
causes `CS0260: Missing partial modifier` or silently creates two independent classes.

```csharp
// ✅ File A: Filename.cs
internal static partial class PolicyDesktop { ... }

// ✅ File B: Filename.Part2.cs
internal static partial class PolicyDesktop { ... }

// ❌ WRONG — without partial, File B is a duplicate class declaration → CS0101
internal static class PolicyDesktop { ... }
```

**Script rule**: When writing the main (trimmed) file, replace `internal static class`
with `internal static partial class`. When writing the Part2 file header, also use
`internal static partial class`. Use a regex replace that won't double-add `partial`:
```powershell
$line = $line -replace 'internal static class', 'internal static partial class'
$line = $line -replace 'internal static partial partial', 'internal static partial'
```

---

## Outer-Class Constants Are Visible in Both Partial Files — No Need to Duplicate

In C# partial classes, `const`, `static readonly`, and other top-level-class members
declared in **either** part are visible in **all** parts. This means:

- `Part2.cs` can freely refer to constants declared in `Filename.cs`
- No header-sharing or `#include`-like mechanism is needed
- The split line can be at ANY inner-class boundary without copying constants

**Why this matters**: Earlier failed split attempts tried to copy outer-class `const`
definitions into the Part2 file "to make them available." This caused `CS0102: The
type already contains a definition for '...'` because partial classes merge all
members at compile time.

**Rule**: When splitting: copy ONLY the `namespace`, `using` statements, and the
`internal static partial class ClassName {` header into Part2. Never copy any
constant, field, or method — they are automatically shared.

---

## `--no-build` for GUI.Tests Fails After Cross-Project File Changes

`dotnet test tests/RegiLattice.GUI.Tests/... --no-build` fails with:
```
An assembly specified in the application dependencies manifest was not found:
  package: 'runtimepack.Microsoft.Windows.SDK.NET.Ref', version: '10.0.19041.57'
  path: 'Microsoft.Windows.SDK.NET.dll'
```

This happens because `RegiLattice.GUI.Tests` targets `net10.0-windows10.0.19041.0`
and relies on Windows SDK runtime packs being copied into the build output during the
build phase. When `--no-build` skips the build, those runtime pack DLLs are absent.

**Root cause**: The Windows desktop SDK `runtimepack` is not a static NuGet package;
it is resolved and copied only during `dotnet build`. `--no-build` bypasses this copy.

**Rule**: Never use `--no-build` for `RegiLattice.GUI.Tests`. It must be built before
testing. In CI, either:
- Use a single `dotnet test` without `--no-build` (builds + tests in one step), or
- Ensure the Build step is `dotnet build RegiLattice.sln -c Release` (not just Core)
  before adding `--no-build` to the GUI test step.

```powershell
# ✅ CORRECT — allows build to copy Windows SDK runtime packs
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj --settings tests/.runsettings

# ❌ BROKEN — skips runtime pack copy; test host crashes before a single test runs
dotnet test tests/RegiLattice.GUI.Tests/RegiLattice.GUI.Tests.csproj --no-build --settings tests/.runsettings
```

---

## Multi-Class Tweak Files — Naming Conflicts on Extraction

When extracting top-level classes from merged tweak files, a newly extracted class name
may conflict with an existing `.cs` file in the `Tweaks/` directory.

**Examples found during v6.13.0 extraction**:
- Class `Input` extracted from `Peripherals.cs` → `Input.cs` already exists → write as `Peripherals_Input.cs`
- Class `Performance` extracted from `System.cs` → `Performance.cs` exists → write as `System_Performance.cs`

**Rule**: Before writing an extracted class to `<ClassName>.cs`, check if that file
already exists. If it does, prefix with the source file's base name: `<SourceBase>_<ClassName>.cs`.
The C# namespace is unchanged — only the filename is adjusted.

```powershell
# Conflict-check pattern used in Split-MultiClass.ps1:
$destName = if (Test-Path (Join-Path $TweaksDir "$className.cs")) {
    "$($file.BaseName)_$className.cs"
} else {
    "$className.cs"
}
```

**Important**: The class name in the file must remain exactly `internal static class Input`
(not renamed). Only the filename gets the prefix. TweakEngine discovers classes by
reflection on the namespace, not by filename, so filenames are arbitrary.

---

## Blame Data Collector `HangTimeout` Is Invalid in .NET SDK 10.0.201+

In `.NET SDK 10.0.201`, the `HangTimeout` attribute on `<CollectDumpOnTestSessionHang>` in
`.runsettings` became an unrecognised configuration key. Leaving it produces two non-fatal
**test-runner warnings** (not CS compiler warnings) on every `dotnet test` invocation:

```
Microsoft.TestPlatform.targets(48,5): warning Data collector 'Blame' message:
  The blame parameter key specified HangTimeout is not valid. Ignoring this key..
Microsoft.TestPlatform.targets(48,5): warning Data collector 'Blame' message:
  All tests finished running, Sequence file will not be generated.
```

These are classified as `warning` in the `build succeeded with N warning(s)` summary, but
they are **NOT** subject to `TreatWarningsAsErrors=true` — that policy applies to the C#
compiler (`CS*`) and Roslyn analyzers, not to the `Microsoft.TestPlatform.targets` blame
collector messages. Tests still run and pass correctly.

**Fix**: Remove the `HangTimeout="30000"` attribute from `<CollectDumpOnTestSessionHang>`.
Use `TestSessionTimeout` in `<RunConfiguration>` as the primary hang-protection mechanism.

```xml
<!-- ❌ BAD — produces warning in .NET SDK 10.0.201+ -->
<CollectDumpOnTestSessionHang DumpType="None" HangTimeout="30000" />

<!-- ✅ GOOD — remove HangTimeout; rely on TestSessionTimeout *)  -->
<CollectDumpOnTestSessionHang DumpType="None" />
```

**Fixed in**: v6.27.0 — `tests/.runsettings` updated.

---

## Copilot Agent Terminal Stdout Capture — Check Output Repeatedly, Not Once

When `dotnet build` or `dotnet test` is run in an async terminal in a Copilot agent session,
the stdout output arrives in chunks and may initially appear blank. The build IS running; output
is just buffered.

**Pattern that works**:

1. Run the command with `mode=async` (non-blocking)
2. Call `get_terminal_output` once after 5–10 seconds — may see blank or partial output
3. Wait a further 5–10 seconds and call `get_terminal_output` AGAIN
4. Continue until you see final lines like `Build succeeded in Ns` or `Test Run Successful`

**Do NOT**:
- Retry the same build command assuming it didn't start
- Open a new terminal and run the command again (causes a second build to start concurrently)
- Wait for `stdout` in a `mode=sync` call with a 60-second timeout — on OneDrive paths,
  core library compilation alone (`CoreCompile`) takes ~54 seconds before any output appears

**Rule**: A `Build succeeded` line WILL appear in `get_terminal_output` output eventually.
Poll 3–5 times at 5–10 second intervals rather than treating the first blank response as failure.

**Symptom context**: This is a terminal ringbuffer/scroll artefact in the VS Code agent
surface. The underlying process is running; the Copilot tool only captures what fits in the
current view. The `RegiLattice.Core CoreCompile` step takes ~54 s, then `Core.Tests CoreCompile`
takes ~30 s — total ~84–110 s before the `Build succeeded` summary line appears.

---

## New Service Methods: `ExportCsv` Is Sync, `ExportToJsonAsync` Is Async

When adding new export methods to Core services (e.g., `TweakHistory`, `Analytics`,
`Favorites`), follow the established naming and sync/async convention:

| Method                  | Pattern  | Why                                               |
| ----------------------- | -------- | ------------------------------------------------- |
| `ExportToJsonAsync()`   | `async`  | JSON serialization can be large; async I/O avoids UI freeze |
| `ExportCsv()`           | `sync`   | CSV is row-by-row; `StringBuilder` + `File.WriteAllText` is fast enough |
| `Load()` / `Flush()`    | `sync`   | Small local file; OneDrive sync is async at OS level |
| Any network or pack I/O | `async`  | Always async for anything going over the wire |

**Specifically for `TweakHistory.ExportCsv`**: The method writes a 7-column CSV with header
`Timestamp,TweakId,Action,Result,Username,MachineName,SessionId`. It uses `StringBuilder`
and `File.WriteAllText(path, csv, Encoding.UTF8)`. Making this `async` would require
`await File.WriteAllTextAsync` and would force callers to `await` unnecessarily for a
local file that is typically under 1 KB.

**Rule**: Do NOT make `ExportCsv` async unless profiling shows it is a bottleneck.

---

## `[JsonIgnore(Condition = WhenWritingDefault)]` for Backward-Compatible DTO Expansion

When adding optional fields to a serialised data class (e.g., `HistoryEntry`, `AnalyticsEntry`),
use `[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]` on nullable fields.
This preserves backward compatibility with existing JSON files that don't have the new fields:

```csharp
// ✅ GOOD — new field is omitted from JSON when null; old files deserialise correctly
[JsonPropertyName("username")]
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
public string? Username { get; set; }

// ❌ BAD — writes "username": null to every entry; bloats the file and breaks old parsers
[JsonPropertyName("username")]
public string? Username { get; set; }
```

**Applied in**: `TweakHistory.HistoryEntry` — three new audit fields (`Username`, `MachineName`,
`SessionId`) added in v6.27.0. Existing `history.json` files from v6.26.0 and earlier load
without error; new entries written by v6.27.0+ include the audit fields automatically.

---

## Per-Process Session ID Pattern — `Guid.NewGuid().ToString("N")[..8]`

When you need a stable, short, readable ID that identifies "all operations in one process run"
(as opposed to a per-machine or per-user ID), use a static GUID truncated to hex:

```csharp
// ✅ GOOD — 8-char hex derived from a unique Guid; stable for the lifetime of the process
private static readonly string s_sessionId = Guid.NewGuid().ToString("N")[..8];
```

Properties:
- **Length**: exactly 8 characters
- **Charset**: lowercase hex `[0-9a-f]` (the `"N"` format omits hyphens)
- **Uniqueness**: collision probability ~1.5 × 10⁻⁹ between two concurrent processes
- **Stability**: same value for all operations in one process invocation
- **Range notation**: `[..8]` = C# 13 range expression for `Substring(0, 8)` — requires .NET 8+

**Applied in**: `TweakHistory.TweakHistory` — `s_sessionId` initialised once at class load,
stamped on every `HistoryEntry` recorded by that process run.

**Test pattern**: To verify the contract, check both constraints in separate tests:

```csharp
[Fact]
public void Record_SameProcess_SharesSessionId()
{
    var entry1 = ...; var entry2 = ...;
    Assert.Equal(entry1.SessionId, entry2.SessionId);   // stable within process
}

[Fact]
public void Record_SessionId_IsEightCharHex()
{
    var entry = ...;
    Assert.NotNull(entry.SessionId);
    Assert.Equal(8, entry.SessionId!.Length);
    Assert.Matches(@"^[0-9a-f]{8}$", entry.SessionId);
}
```

---

## `CategoryHealthScore` Sealed Record — Placement After Class Closing Brace

When adding a companion record or struct to a service class in the same file, place it
**after** the class closing brace, not inside the class:

```csharp
// ✅ GOOD — CategoryHealthScore is a sibling type in the same namespace, not nested
internal sealed class HealthScoreService { ... }

public sealed record CategoryHealthScore(
    string Category, int Score, int AppliedCount, int TotalCount, string Recommendation);

// ❌ BAD — nested inside HealthScoreService; callers need the full qualified name
internal sealed class HealthScoreService {
    public sealed record CategoryHealthScore(...);
}
```

**Why**: Callers (GUI, CLI, tests) need `CategoryHealthScore` without knowing which service
class hosts it. Placing it at namespace level ("sibling type") lets callers import it with
just `using RegiLattice.Core.Services` rather than `using ... = HealthScoreService.CategoryHealthScore`.

---

## `ConflictSeverity` Enum and `RegistryConflict` Record — Phase 6.3 Pattern

When extending `ConflictDetector.cs` or similar static analysis services:

1. Define supporting enums and record structs **before** the service class declaration.
2. Use `readonly record struct` for result types that are small, value-typed, and returned
   in `IReadOnlyList<T>` — avoids heap allocations in hot paths.
3. The `DetectRegistryConflicts()` method indexes all `RegOp` ApplyOps by `"Path\Name"` key.
   Only `SetValue`-family ops (SetDword, SetString, etc.) are indexed — not Check, Delete, or DeleteTree ops.
4. Severity classification:
   - `Info` — same path+name with the SAME value (redundant but not conflicting)
   - `Critical` — same path+name with opposing binary values `0` / `1` (tweaks cancel each other)
   - `Warning` — same path+name with different non-binary values (order-dependent behaviour)

```csharp
// Severity helper pattern:
private static ConflictSeverity ClassifySeverity(string valA, string valB)
{
    if (string.Equals(valA, valB, StringComparison.Ordinal)) return ConflictSeverity.Info;
    if (int.TryParse(valA, out var ia) && int.TryParse(valB, out var ib)
        && ((ia == 0 && ib == 1) || (ia == 1 && ib == 0)))
        return ConflictSeverity.Critical;
    return ConflictSeverity.Warning;
}
```

**Test pattern**: Use a private factory helper `MakeSetDword(id, path, name, value)` in the
test class to create minimal `TweakDef` instances without cluttering each test body:
```csharp
private static TweakDef MakeSetDword(string id, string path, string name, int value) =>
    new() { Id = id, Label = id, Category = "Test",
            ApplyOps = [RegOp.SetDword(path, name, value)] };
```
