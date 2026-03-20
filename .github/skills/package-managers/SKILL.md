---
name: package-managers
description: "Add, modify, or debug package manager integration in RegiLattice: WinGet, Scoop, pip, Chocolatey, PowerShell modules. Use when extending BasePackageManagerDialog, adding a new manager backend, changing package name validation, or working on ToolVersionChecker. Triggers on: 'package manager', 'winget', 'scoop', 'chocolatey', 'pip', 'PSModule', 'BasePackageManagerDialog', 'dialog'."
argument-hint: "Which package manager or dialog to work with"
---

# Package Manager Integration — RegiLattice

## Architecture

All 5 dialogs extend `BasePackageManagerDialog` (template method pattern):

```
BasePackageManagerDialog (abstract) — Forms/BasePackageManagerDialog.cs
├── WinGetManagerDialog        (+ AddExtraButtons for Search)
├── ScoopManagerDialog
├── ChocolateyManagerDialog
├── PipManagerDialog           (+ BuildScopePanel for venv/global)
└── PSModuleManagerDialog      (+ BuildScopePanel, UpgradeText = "Update")
```

Backend managers are in `src/RegiLattice.GUI/PackageManagers/`:
- `WinGetManager.cs`, `ScoopManager.cs`, `ChocolateyManager.cs`, `PipManager.cs`, `PSModuleManager.cs`
- `ToolVersionChecker.cs` — version detection for all tools
- `WindowsHealthManager.cs` — system health checks
- `PackageNameValidator.cs` — shared `[GeneratedRegex]` name validation

## Base Class — What You Get For Free

- `SplitContainer` (top: ListView + buttons; bottom: RichTextBox log)
- Prereq banner with async install flow
- `AppendLog()`, `SetBusy()`, `SetStatus()`, `SetOutdated()`
- `RebuildQuickInstallButtons()` — builds quick-install buttons from `PopularPackages`
- Shared async wrappers: `RefreshAsync()`, `InstallAsync()`, `RemoveAsync()`, `UpgradeAsync()`

## Adding a New Package Manager

### 1. Backend: `src/RegiLattice.GUI/PackageManagers/NewManager.cs`
```csharp
public static class NewManager
{
    public static async Task<IReadOnlyList<PackageInfo>> ListInstalledAsync(CancellationToken ct) { ... }
    public static async Task<bool> InstallAsync(string name, CancellationToken ct) { ... }
    public static async Task<bool> UninstallAsync(string name, CancellationToken ct) { ... }
    public static async Task<bool> UpgradeAsync(string name, CancellationToken ct) { ... }
    public static async Task<bool> IsInstalledAsync() { ... }
}
```

### 2. Dialog: `src/RegiLattice.GUI/Forms/NewManagerDialog.cs`
```csharp
public sealed class NewManagerDialog : BasePackageManagerDialog
{
    // Required abstract properties
    protected override string DialogTitle => "New Manager";
    protected override Icon DialogIcon => AppIcons.ToolIcon;
    protected override string PrereqReadyText => "Ready";
    protected override string PrereqMissingText => "Not installed — click Install";
    protected override string PrereqInstallingText => "Installing...";
    protected override string PrereqInstallButtonText => "Install New Manager";
    protected override string UpgradeText => "Upgrade";
    protected override IReadOnlyList<string> PopularPackages => [...];
    
    // Required abstract methods
    protected override Task<bool> CheckPrereq() => NewManager.IsInstalledAsync();
    protected override async Task InstallPrereqAsync(CancellationToken ct) { ... }
    protected override async Task RefreshCoreAsync(CancellationToken ct) { ... }
    protected override async Task InstallCoreAsync(string name, CancellationToken ct) { ... }
    protected override async Task RemoveCoreAsync(string name, CancellationToken ct) { ... }
    protected override async Task UpgradeCoreAsync(string name, CancellationToken ct) { ... }
    protected override void BuildListColumns() { ... }
}
```

### 3. Wire up in `MainForm.cs`
Add a menu item under **Tools** menu → `new NewManagerDialog().ShowDialog()`.

### 4. Add version check in `ToolVersionChecker.CheckAllAsync()`

### 5. Add validation tests in `PackageManagerValidationTests.cs`

## Security Rules

- **All process execution via `ShellRunner.RunAsync`** with `ArgumentList` — never string interpolation
- **Package names validated** with `PackageNameValidator.IsValid(name)` before any command
- **Regex**: `^[A-Za-z0-9._\-]+$` — no spaces, no shell metacharacters
- **No `shell: true`** equivalent — always explicit executable + args array
