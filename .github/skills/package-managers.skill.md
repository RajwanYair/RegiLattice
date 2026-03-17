---
mode: agent
description: "Manage package managers (winget, scoop, pip, chocolatey, PowerShell modules) in RegiLattice"
tools: ["read_file", "replace_string_in_file", "grep_search", "semantic_search", "runTests"]
---

# Package Manager — RegiLattice Skill

You are an expert at working with the RegiLattice package manager integration layer.

## Architecture

RegiLattice wraps 5 package managers with a consistent interface:

| Manager | Backend Class | Dialog | Location |
|---------|--------------|--------|----------|
| **WinGet** | `WinGetManager.cs` | `WinGetManagerDialog.cs` | GUI/PackageManagers + GUI/Forms |
| **Scoop** | `ScoopManager.cs` | `ScoopManagerDialog.cs` | GUI/PackageManagers + GUI/Forms |
| **pip** | `PipManager.cs` | `PipManagerDialog.cs` | GUI/PackageManagers + GUI/Forms |
| **Chocolatey** | `ChocolateyManager.cs` | `ChocolateyManagerDialog.cs` | GUI/PackageManagers + GUI/Forms |
| **PS Modules** | `PSModuleManager.cs` | `PSModuleManagerDialog.cs` | GUI/PackageManagers + GUI/Forms |

## Common Interface Pattern

Each manager provides:
- `IsXxxInstalled()` — availability check
- `ListInstalledAsync(ct)` — list packages
- `InstallAsync(name, ct)` — install with validated name
- `UninstallAsync(name, ct)` — remove package
- `UpgradeAsync(name, ct)` — upgrade package
- `ListOutdatedAsync(ct)` — check for updates
- `ValidateName(name)` — input validation (regex)
- `PopularPackages` / `PopularTools` / `PopularModules` — quick-install list

## Security

- All process execution via `ShellRunner.RunAsync` with `ArgumentList` (no injection)
- Package names validated with `[GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]`
- No `shell=true` or string interpolation into commands

## Adding a New Manager

All 5 dialog classes extend `BasePackageManagerDialog` (template method pattern).
To add a new package manager dialog:

1. Create `src/RegiLattice.GUI/PackageManagers/NewManager.cs` — backend (static async methods)
2. Create `src/RegiLattice.GUI/Forms/NewManagerDialog.cs` — sealed class extending `BasePackageManagerDialog`
   - Override abstract properties: `DialogTitle`, `DialogIcon`, `PrereqReadyText`, `PrereqMissingText`,
     `PrereqInstallingText`, `PrereqInstallButtonText`, `UpgradeText`, `PopularPackages`, `BuildListColumns()`
   - Override abstract methods: `CheckPrereq()`, `InstallPrereqAsync(ct)`, `RefreshCoreAsync(ct)`,
     `InstallCoreAsync(name, ct)`, `RemoveCoreAsync(name, ct)`, `UpgradeCoreAsync(name, ct)`
   - Optional: override `BuildScopePanel()` for scope selection, `AddExtraButtons()` for custom buttons
3. Add menu item in `MainForm.cs` under Tools menu
4. Add version checking in `ToolVersionChecker.cs`
5. Write validation tests in `PackageManagerValidationTests.cs`

### BasePackageManagerDialog Provides (inherited for free)

- `SplitContainer` with resizable pane: top = ListView + buttons, bottom = RichTextBox log panel
- Shared controls: `_lstInstalled` ListView, `_txtName` TextBox, `_lblStatus`, `_lblOutdated`, `_flowQuick` FlowLayoutPanel
- Prereq banner with async install flow (yellow→green/red states)
- `AppendLog(msg, color?)` — timestamped `[HH:mm:ss]` entries in the log panel
- `SetBusy(bool, msg?)`, `SetStatus(msg, color?)`, `SetOutdated(msg, color?)`
- `RebuildQuickInstallButtons()` — auto-generates quick-install buttons from `PopularPackages`
- Shared async wrappers: `RefreshAsync()`, `InstallAsync()`, `RemoveAsync()`, `UpgradeAsync()`

## Rules

- All names validated before passing to `ShellRunner`
- Async with `CancellationToken` throughout
- `ConfigureAwait(false)` on all awaits in non-UI code
- Use `--disable-interactivity` / `-y` / `--no-progress` flags
