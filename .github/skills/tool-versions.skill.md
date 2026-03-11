---
mode: agent
description: "Check and manage external tool versions (PowerShell, Python, winget, scoop, chocolatey, git, node) and verify latest versions are available"
tools: ["read_file", "grep_search", "run_in_terminal", "replace_string_in_file"]
---

# Tool Version Management — RegiLattice Skill

You are an expert at checking and managing external tool versions used by RegiLattice.

## Context

RegiLattice depends on several external tools:

| Tool | Executable | Version Command | Update Command |
|------|-----------|----------------|----------------|
| PowerShell | `pwsh` | `pwsh -NoProfile -c "$PSVersionTable.PSVersion"` | `winget upgrade Microsoft.PowerShell` |
| Python | `python` | `python -c "import sys; print(sys.version)"` | `winget upgrade Python.Python.3.12` |
| winget | `winget` | `winget --version` | Built into Windows — update via Microsoft Store |
| Scoop | scoop.ps1 | `scoop --version` | `scoop update` |
| Chocolatey | `choco` | `choco --version` | `choco upgrade chocolatey` |
| Git | `git` | `git --version` | `winget upgrade Git.Git` |
| Node.js | `node` | `node --version` | `winget upgrade OpenJS.NodeJS` |

## Key Files

- `src/RegiLattice.GUI/PackageManagers/ToolVersionChecker.cs` — Version detection
- `src/RegiLattice.GUI/Forms/ToolVersionsDialog.cs` — Version display UI
- `src/RegiLattice.Core/Services/AppConfig.cs` — `CheckToolUpdates` setting

## Checking for Updates

When `AppConfig.CheckToolUpdates` is enabled, the application checks if newer versions
are available for each installed tool using version comparison APIs.

## Adding Version Check for a New Tool

1. Add a `CheckToolAsync()` entry in `ToolVersionChecker.CheckAllAsync()`
2. Add the latest version API lookup if available
3. Add the tool to `ToolVersionsDialog` display

## Rules

- All terminal commands MUST be PowerShell syntax
- No Unix/bash commands
- All process execution via `ShellRunner.RunAsync` with `ArgumentList`
