---
mode: agent
description: "Analyze package manager health — check installed packages, outdated packages, and tool availability across winget, scoop, pip, chocolatey, and PowerShell modules"
---

# Package Health Check

Analyze the health and update status of all 5 package managers in RegiLattice.

## Steps

1. Check which package managers are installed:
   - `winget --version`
   - `pwsh -NoProfile -Command "scoop --version | Select-Object -First 1"`
   - `python -m pip --version`
   - `choco --version`
   - `pwsh -NoProfile -Command "Get-Module PowerShellGet -ListAvailable | Select-Object -First 1"`

2. For each installed manager, check for outdated packages
3. Report a health summary

## Expected Output

| Manager | Status | Installed Packages | Updates Available |
|---------|--------|-------------------|-------------------|
| winget | Installed (v1.x) | 42 | 3 |
| scoop | Not found | — | — |
| pip | Installed (24.x) | 18 | 2 |
| choco | Installed (2.x) | 15 | 1 |
| PS Modules | Available | 8 | 0 |

## Related Files

- `src/RegiLattice.GUI/PackageManagers/*.cs` — Backend managers
- `src/RegiLattice.GUI/Forms/*ManagerDialog.cs` — UI dialogs
- `src/RegiLattice.GUI/PackageManagers/ToolVersionChecker.cs` — Version detection
