---
mode: agent
description: "Check external tool versions and verify if updates are available for PowerShell, Python, winget, scoop, git, etc."
---

# Check Tool Updates

Check all external tool versions and notify about available updates.

## Steps

1. Run version checks for all tools:
   - PowerShell: `pwsh -NoProfile -Command "$PSVersionTable.PSVersion.ToString()"`
   - Python: `python -c "import sys; print(f'{sys.version_info.major}.{sys.version_info.minor}.{sys.version_info.micro}')"`
   - winget: `winget --version`
   - Git: `git --version`
   - Node.js: `node --version`
   - Scoop: `pwsh -NoProfile -Command "scoop --version | Select-Object -First 1"`
   - Chocolatey: `choco --version`

2. Check if updates are available:
   - For winget-managed tools: `winget upgrade`
   - For scoop: `scoop status`
   - For choco: `choco outdated --limit-output`
   - For pip: `python -m pip list --outdated --format=json`
   - For PS modules: `Get-InstalledModule | ForEach-Object { $o = Find-Module $_.Name -EA 0; if ($o.Version -gt $_.Version) { "$($_.Name): $($_.Version) -> $($o.Version)" } }`

3. Report results

## Expected Output

| Tool | Installed | Latest | Status |
|------|-----------|--------|--------|
| PowerShell | 7.4.1 | 7.5.0 | Update available |
| Python | 3.12.2 | 3.12.8 | Update available |
| Git | 2.43.0 | 2.43.0 | Up to date |
