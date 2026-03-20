---
name: tool-versions
description: "Check, update, or add version detection for external tools used by RegiLattice (PowerShell, Python, winget, scoop, chocolatey, git, node). Use when verifying tool availability, adding a new tool to ToolVersionChecker, or debugging version detection failures. Triggers on: 'tool version', 'ToolVersionChecker', 'winget version', 'check tools', 'update tools'."
argument-hint: "Tool name or what to check/add (e.g. 'add docker version check')"
---

# Tool Versions — RegiLattice

## Tools Tracked

| Tool | Executable | Version Arg | Upgrade via |
|------|-----------|------------|-------------|
| PowerShell 7 | `pwsh` | `-NoProfile -c "$PSVersionTable.PSVersion"` | `winget upgrade Microsoft.PowerShell` |
| Python | `python` | `-c "import sys; print(sys.version)"` | `winget upgrade Python.Python.3` |
| WinGet | `winget` | `--version` | Windows Store (built-in) |
| Scoop | `scoop` | `--version` | `scoop update` |
| Chocolatey | `choco` | `--version` | `choco upgrade chocolatey` |
| Git | `git` | `--version` | `winget upgrade Git.Git` |
| Node.js | `node` | `--version` | `winget upgrade OpenJS.NodeJS` |

## Key Files

| File | Purpose |
|------|---------|
| `src/RegiLattice.GUI/PackageManagers/ToolVersionChecker.cs` | Version detection + comparison logic |
| `src/RegiLattice.GUI/Forms/ToolVersionsDialog.cs` | UI showing all tool versions |
| `src/RegiLattice.Core/Services/AppConfig.cs` | `CheckToolUpdates` setting |
| `tests/RegiLattice.GUI.Tests/PackageManagerValidationTests.cs` | Tool version format tests |

## Adding a New Tool

1. **Add entry to `ToolVersionChecker.CheckAllAsync()`**:
```csharp
new ToolVersionEntry
{
    Name = "MyTool",
    Executable = "mytool",
    VersionArgs = ["--version"],
    ParseVersion = static output => output.Trim().TrimStart('v'),
    LatestVersionUrl = "https://api.github.com/repos/org/mytool/releases/latest"
}
```

2. **Add the tool row to `ToolVersionsDialog`** display list

3. **Write a test** in `PackageManagerValidationTests.cs` verifying version string format parsing

## Process Execution Rules

- All tool invocations use `ShellRunner.RunAsync(executable, argsArray, ct)` — never `Process.Start` with a string command
- Never concatenate user input into command arguments
- Check `exitCode == 0` before trusting `stdout`

## Version Comparison

```csharp
// Parse and compare semantic versions
if (Version.TryParse(detected, out var current) &&
    Version.TryParse(latest, out var latest))
{
    bool needsUpdate = latest > current;
}
```
