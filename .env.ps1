# RegiLattice workspace environment bootstrap
# Source this from your $PROFILE or run `. .\.env.ps1` to activate.
# VS Code terminal.integrated.env.windows already injects PATH automatically.
# This file is provided for standalone pwsh sessions and CI.

$_pyBin = "$env:LOCALAPPDATA\Python\bin"
$_pyScripts = "$env:LOCALAPPDATA\Python\pythoncore-3.14-64\Scripts"

foreach ($_dir in @($_pyBin, $_pyScripts)) {
    if (Test-Path $_dir) {
        if ($env:PATH -notmatch [regex]::Escape($_dir)) {
            $env:PATH = "$_dir;$env:PATH"
            Write-Verbose "PATH: added $_dir"
        }
    }
}

# Workspace root
$env:PYTHONPATH = $PSScriptRoot
$env:PYTHONIOENCODING = 'utf-8'
$env:PYTHONDONTWRITEBYTECODE = '1'

# Convenience aliases for this session
Set-Alias -Name rl     -Value "$_pyBin\python.exe" -Option AllScope -ErrorAction SilentlyContinue
function Invoke-RegiLattice  { & python -m regilattice @args }
function Invoke-RegiLatticeGUI  { & python -m regilattice --gui @args }
function Invoke-RegiLatticeMenu { & python -m regilattice --menu @args }
Set-Alias -Name rgl     -Value Invoke-RegiLattice
Set-Alias -Name rgl-gui  -Value Invoke-RegiLatticeGUI
Set-Alias -Name rgl-menu -Value Invoke-RegiLatticeMenu
