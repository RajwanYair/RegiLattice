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

# ── Convenience aliases for common project tasks ────────────────────
if (-not (Get-Alias -Name 'rlbuild' -ErrorAction SilentlyContinue)) {
    Set-Alias -Name 'rlbuild'  -Value { dotnet build RegiLattice.sln }.GetSteppablePipeline -Option ReadOnly -ErrorAction SilentlyContinue
}
function Invoke-RLBuild   { dotnet build RegiLattice.sln @args }
function Invoke-RLTest    { dotnet test  RegiLattice.sln --logger "console;verbosity=normal" @args }
function Invoke-RLGui     { dotnet run --project src/RegiLattice.GUI @args }
function Invoke-RLCli     { dotnet run --project src/RegiLattice.CLI -- @args }

# ── Tab completion for common dotnet arguments ──────────────────────
$_rlTestProjects = @(
    'tests/RegiLattice.Core.Tests'
    'tests/RegiLattice.CLI.Tests'
    'tests/RegiLattice.GUI.Tests'
)

Register-ArgumentCompleter -CommandName 'dotnet' -ScriptBlock {
    param($wordToComplete, $commandAst, $cursorPosition)
    $cmdText = $commandAst.ToString()
    if ($cmdText -match 'dotnet\s+test\s+') {
        $_rlTestProjects | Where-Object { $_ -like "$wordToComplete*" } |
            ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
    }
}

# ── PSReadLine enhancements (prediction, history search) ────────────
if (Get-Module -ListAvailable PSReadLine -ErrorAction SilentlyContinue) {
    Set-PSReadLineOption -PredictionSource HistoryAndPlugin -ErrorAction SilentlyContinue
    Set-PSReadLineOption -PredictionViewStyle ListView -ErrorAction SilentlyContinue
    Set-PSReadLineKeyHandler -Key UpArrow   -Function HistorySearchBackward
    Set-PSReadLineKeyHandler -Key DownArrow -Function HistorySearchForward
    Set-PSReadLineKeyHandler -Key Tab       -Function MenuComplete
}

# ── Custom prompt with git branch + command timing ──────────────────
function global:prompt {
    $exitCode = $LASTEXITCODE
    $duration = ''
    if ((Get-History -Count 1 -ErrorAction SilentlyContinue) -is [Microsoft.PowerShell.Commands.HistoryInfo]) {
        $last = Get-History -Count 1
        $ms   = ($last.EndExecutionTime - $last.StartExecutionTime).TotalMilliseconds
        if ($ms -ge 1000) { $duration = " ($([math]::Round($ms / 1000, 1))s)" }
        elseif ($ms -ge 100) { $duration = " ($([int]$ms)ms)" }
    }
    $branch = ''
    if (Get-Command git -ErrorAction SilentlyContinue) {
        $b = git rev-parse --abbrev-ref HEAD 2>$null
        if ($b) { $branch = " ($b)" }
    }
    $cwd = (Get-Location).Path -replace [regex]::Escape($HOME), '~'
    $arrow = if ($exitCode -eq 0 -or $null -eq $exitCode) { "`e[32m❯`e[0m" } else { "`e[31m❯`e[0m" }
    "`e[36m$cwd`e[33m$branch`e[90m$duration`e[0m`n$arrow "
}

Write-Host "[.env.ps1] RegiLattice dev environment loaded — $(($env:PATH -split ';').Count) PATH entries" -ForegroundColor DarkCyan
Write-Host "  Helpers: Invoke-RLBuild, Invoke-RLTest, Invoke-RLGui, Invoke-RLCli" -ForegroundColor DarkGray
