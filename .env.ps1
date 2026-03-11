# ─────────────────────────────────────────────────────────────────────────────
# .env.ps1 — RegiLattice project environment bootstrap
# ─────────────────────────────────────────────────────────────────────────────
# Dot-source this file in any PowerShell session to ensure every CLI tool
# available on this machine is reachable without manual $env:PATH editing.
#
# Usage:
#   . .\.env.ps1                 # from the project root
#   . "$PSScriptRoot\.env.ps1"   # from scripts or tasks
#
# Idempotent — safe to source multiple times in the same session.
# ─────────────────────────────────────────────────────────────────────────────
#$env:REGILATTICE_ENV_LOADED = '1'

# Guard: only run once per session
if ($env:REGILATTICE_ENV_LOADED -eq '1') { return }

# ── Helper: append a directory to PATH only if it exists and isn't there yet ─
function Add-PathEntry {
    param([string]$Dir)
    $resolved = [System.Environment]::ExpandEnvironmentVariables($Dir)
    if (-not (Test-Path $resolved -PathType Container)) { return }
    $current = $env:PATH -split ';'
    if ($current -notcontains $resolved) {
        $env:PATH = "$resolved;$env:PATH"
    }
}

# ── Scoop shims (covers 7zip, bat, fd, rg, jq, cmake, ninja, delta, etc.) ───
Add-PathEntry "$env:USERPROFILE\scoop\shims"

# ── Scoop apps with their own bin directories ────────────────────────────────
Add-PathEntry "$env:USERPROFILE\scoop\apps\git\current\cmd"
Add-PathEntry "$env:USERPROFILE\scoop\apps\git\current\usr\bin"
Add-PathEntry "$env:USERPROFILE\scoop\apps\mingw\current\bin"
Add-PathEntry "$env:USERPROFILE\scoop\apps\llvm\current\bin"
Add-PathEntry "$env:USERPROFILE\scoop\apps\perl\current\perl\bin"
Add-PathEntry "$env:USERPROFILE\scoop\apps\cmake\current\bin"
Add-PathEntry "$env:USERPROFILE\scoop\apps\nuget\current"

# ── PowerShell 7 ─────────────────────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\PowerShell\7"

# ── .NET SDK ─────────────────────────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\dotnet"
Add-PathEntry "$env:USERPROFILE\.dotnet\tools"

# ── Python (python-build-standalone / Python Launcher) ───────────────────────
Add-PathEntry "$env:LOCALAPPDATA\Python\bin"
Add-PathEntry "$env:LOCALAPPDATA\Python\pythoncore-3.14-64\Scripts"
Add-PathEntry "$env:LOCALAPPDATA\Programs\Python\Python314\Scripts"
Add-PathEntry "$env:LOCALAPPDATA\Programs\Python\Python313\Scripts"

# ── Node.js / npm / pnpm / yarn ─────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\nodejs"
Add-PathEntry "$env:APPDATA\npm"
Add-PathEntry "$env:LOCALAPPDATA\pnpm"
Add-PathEntry "$env:USERPROFILE\scoop\apps\nvm\current\nodejs\nodejs"

# ── WinGet (via App Installer) ──────────────────────────────────────────────
Add-PathEntry "$env:LOCALAPPDATA\Microsoft\WindowsApps"

# ── Chocolatey ───────────────────────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMDATA\chocolatey\bin"

# ── GitHub CLI ───────────────────────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\GitHub CLI"

# ── Java (VS Code JDK extension fallback) ───────────────────────────────────
Add-PathEntry "$env:APPDATA\Code\User\globalStorage\pleiades.java-extension-pack-jdk\java\latest\bin"
Add-PathEntry "$env:JAVA_HOME\bin"

# ── Docker Desktop ──────────────────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\Docker\Docker\resources\bin"

# ── Rust / Cargo ─────────────────────────────────────────────────────────────
Add-PathEntry "$env:USERPROFILE\.cargo\bin"

# ── Go ───────────────────────────────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\Go\bin"
Add-PathEntry "$env:USERPROFILE\go\bin"

# ── VS Code CLI ─────────────────────────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\Microsoft VS Code"

# ── Azure / AWS / Terraform / kubectl ────────────────────────────────────────
Add-PathEntry "$env:PROGRAMFILES\Microsoft SDKs\Azure\CLI2\wbin"
Add-PathEntry "$env:PROGRAMFILES\Amazon\AWSCLIV2"
Add-PathEntry "$env:LOCALAPPDATA\Programs\Terraform"

# ── Inno Setup (from Scoop extras) ──────────────────────────────────────────
Add-PathEntry "$env:USERPROFILE\scoop\apps\inno-setup\current"

# ── WiX Toolset ─────────────────────────────────────────────────────────────
Add-PathEntry "$env:USERPROFILE\.dotnet\tools"

# ── Mark as loaded ──────────────────────────────────────────────────────────
$env:REGILATTICE_ENV_LOADED = '1'

Write-Host "[.env.ps1] RegiLattice dev environment loaded — $(($env:PATH -split ';').Count) PATH entries" -ForegroundColor DarkCyan
