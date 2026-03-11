# Troubleshooting Guide

## Common Errors

### `System.UnauthorizedAccessException` / Access Denied

**Cause:** The tweak modifies `HKEY_LOCAL_MACHINE` keys which require administrator access.

**Fix:**
- Right-click your terminal → **Run as administrator**, then re-run the command.
- Use the GUI which auto-elevates via UAC.
- If running from VS Code, restart VS Code as administrator.

### Corporate Environment Detected

**Cause:** RegiLattice detected a domain-joined, Azure AD/Entra ID, or managed machine
via `CorporateGuard`.

**Fix:**
- Use `--force` to bypass the safety check (at your own risk).
- Or select only `CorpSafe = true` tweaks (shown as green USER badges in GUI).
- In GUI, check the "Force" checkbox.

### `WinError 5: Access is denied`

**Cause:** The registry key is protected by ACLs or Group Policy.

**Fix:**
- Ensure you're running as administrator.
- Some keys are locked by Group Policy — contact IT or use `gpedit.msc`.
- Corporate-managed machines may block certain keys entirely.

### `WinError 6: The handle is invalid`

**Cause:** The registry operation failed due to a stale or invalid handle. This can
happen in automated test environments or when the registry is being modified by
another process.

**Fix:**
- Retry the operation.
- If in tests, ensure dry-run mode is active on `RegistrySession`.

### Duplicate TweakDef ID at Registration

**Cause:** Two tweak modules define the same `Id` value.

**Fix:**
- Search for the duplicate:
  `Select-String -Pattern 'the-id' -Path src\RegiLattice.Core\Tweaks\*.cs`
- Rename one of the duplicates to be globally unique.

### Build Errors After .NET SDK Update

**Cause:** Target framework mismatch after SDK update.

**Fix:**
- Verify your SDK version: `dotnet --version`
- Required: .NET 10.0 SDK (`net10.0-windows`)
- Clean and rebuild: `dotnet clean; dotnet build`

## GUI Issues

### GUI Window is Blank / Categories Don't Load

**Cause:** The `RegisterBuiltins()` call may be slow on first run.

**Fix:**
- Wait a moment — categories load during initialization.
- Check the Output window in VS Code for any C# exceptions.

### Theme Looks Wrong / Colours Are Off

**Cause:** WinForms theme application issue.

**Fix:**
- Try switching themes via the theme selector dropdown.
- 4 themes available: Catppuccin Mocha (default), Catppuccin Latte, Nord, Dracula.

## CLI Issues

### `--snapshot-diff` Shows No Colours

**Cause:** ANSI colour codes may not be supported in all terminals.

**Fix:**
- Use Windows Terminal, PowerShell 7+, or VS Code integrated terminal.

### `--profile` Doesn't Apply All Expected Tweaks

**Cause:** Some tweaks may be skipped due to corporate guard, build version, or admin requirements.

**Fix:**
- Check the output for `SkippedCorp`, `SkippedBuild`, or `SkippedAdmin` entries.
- Use `--force` to bypass corporate safety.

## Build Issues

### NuGet Restore Fails

**Fix:**
```powershell
dotnet nuget locals all --clear
dotnet restore RegiLattice.sln
```

### Test Discovery Fails

**Fix:**
```powershell
dotnet clean RegiLattice.sln
dotnet build RegiLattice.sln
dotnet test RegiLattice.sln
```
