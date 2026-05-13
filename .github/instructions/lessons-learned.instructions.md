---
applyTo: "**/*.cs,**/tests/**,**/*Tests/**"
---

# Lessons Learned -- RegiLattice (Slim Reference)

> This file contains the top critical pitfalls only.
> **Full archived version** (2,500+ lines): `docs/archive/lessons-learned-v6.md`
> Last updated: 2026-05-13 (v6.34.0)

---

## Duplicate TweakDef IDs -- Global Uniqueness Required

`TweakEngine.Register()` throws `ArgumentException` on duplicate IDs.
Search before adding:

```powershell
Select-String -Pattern '"new-tweak-id"' -Path src/RegiLattice.Core/Tweaks/*.cs
```

---

## `AppConfig.ConfigDir` -- NOT `AppConfig.DataRoot`

`AppConfig.DataRoot` does **not exist** -- CS0117. Always use `AppConfig.ConfigDir`.

```csharp
// OK
string path = Path.Combine(AppConfig.ConfigDir, "history.json");
```

---

## `CorporateGuard.StubCorporate` -- Required in Every Test Fixture

Blocks on Intune/SCCM registry on corporate machines. Stub it:

```csharp
public sealed class MyFixture : IDisposable
{
    public MyFixture() => CorporateGuard.StubCorporate = false;
    public void Dispose() => CorporateGuard.StubCorporate = null;
}
```

---

## `Task.Run` + WMI = COM STA Threads Blocking Process Exit

Use explicit `Thread` with `IsBackground = true` + `ApartmentState.MTA` for all WMI calls.

```csharp
var t = new Thread(() => { try { action(); } catch { } }) { IsBackground = true };
t.SetApartmentState(ApartmentState.MTA);
t.Start();
```

---

## RegistrySession DryRun -- Required for All Tests

```csharp
var session = new RegistrySession { DryRun = true };
```

---

## `ParseArgs()` -- Always `Assert.NotNull()` First

```csharp
var a = ParseArgs(new[] { "--list" });
Assert.NotNull(a);
Assert.True(a.ShowList);
```

---

## OneDrive Build Cache -- Recovery

```powershell
dotnet build-server shutdown
Remove-Item "$env:TEMP\RegiLattice-build\RegiLattice.Core" -Recurse -Force -ErrorAction SilentlyContinue
$env:MSBUILDDISABLENODEREUSE = '1'
dotnet build RegiLattice.sln -c Debug -m:1
```

Never use `-q` / `--verbosity quiet`.

---

## SnapshotManager.Save() -- Pass `cachedStatus` to Avoid 30s Hang

```csharp
var cached = engine.StatusMap(parallel: true);
engine.SaveSnapshot("snapshot.json", cachedStatus: cached);
```

---

## `[CallerFilePath]` for Pack Tests -- Never `AppDomain.BaseDirectory`

```csharp
private static string PackPath(string fileName,
    [System.Runtime.CompilerServices.CallerFilePath] string? src = null)
    => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(src)!, "..", "..", "packs", fileName));
```

---

## `--no-build` for GUI.Tests Always Fails

Windows SDK runtime packs require the build copy step. Never use `--no-build` for GUI.Tests.

---

## Module Already Exists -- Check Before `create_file`

```powershell
Test-Path "src/RegiLattice.Core/Tweaks/<ModuleName>.cs"
```

---

## `get_errors` -- Only Act on CS-Prefixed Errors

"Replace LF with ..." diagnostics are CSharpier diffs, not compiler errors.

---

## Multi-Class Merged Tweaks/ Files

```powershell
Select-String -Pattern '^internal static (partial )?class \w+' SomeFile.cs
```
Count > 1 = multi-class extraction. Count = 1 = partial class split.

---

## Redirected Stdout Deadlock

```csharp
process.Start();
string _ = process.StandardOutput.ReadToEnd();
bool exited = process.WaitForExit(10_000);
```

---

## Cross-Assembly Test Races

Never use `dotnet test RegiLattice.sln`. Run Core, CLI, GUI individually.

---

## XML Comments Must Not Contain `--` in `.runsettings`

.NET SDK 10.0.201+ rejects double-hyphen inside XML comment text.

---

> Full historical record: `docs/archive/lessons-learned-v6.md`
