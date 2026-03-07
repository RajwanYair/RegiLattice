# RegiLattice PowerShell Module
# Thin wrapper around the Python CLI for PowerShell-native users.
#
# Usage:
#   Import-Module .\RegiLattice.psm1
#   Get-RegiLattice                    # list all tweaks
#   Invoke-RegiLattice apply "tweak-id"
#   Invoke-RegiLattice --gui

$script:PythonExe = $null

function Find-Python {
    if ($script:PythonExe) { return $script:PythonExe }
    $candidates = @(
        "$env:LOCALAPPDATA\Python\bin\python.exe",
        (Get-Command python -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source -ErrorAction SilentlyContinue),
        (Get-Command python3 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source -ErrorAction SilentlyContinue)
    )
    foreach ($c in $candidates) {
        if ($c -and (Test-Path $c)) {
            $script:PythonExe = $c
            return $c
        }
    }
    throw "Python not found. Install Python 3.10+ and ensure it's on PATH."
}

function Invoke-RegiLattice {
    <#
    .SYNOPSIS
    Run a RegiLattice CLI command.

    .DESCRIPTION
    Passes arguments directly to `python -m regilattice`.

    .EXAMPLE
    Invoke-RegiLattice --list
    Invoke-RegiLattice apply "explorer-show-file-extensions"
    Invoke-RegiLattice --gui
    Invoke-RegiLattice --snapshot "before.json"
    Invoke-RegiLattice --profile gaming -y
    #>
    [CmdletBinding()]
    param(
        [Parameter(ValueFromRemainingArguments)]
        [string[]]$Arguments
    )
    $py = Find-Python
    & $py -m regilattice @Arguments
}

function Get-RegiLattice {
    <#
    .SYNOPSIS
    List all available RegiLattice tweaks.

    .EXAMPLE
    Get-RegiLattice
    Get-RegiLattice | Where-Object { $_ -match "privacy" }
    #>
    [CmdletBinding()]
    param()
    Invoke-RegiLattice --list
}

function Get-RegiLatticeStatus {
    <#
    .SYNOPSIS
    Check the status of a specific tweak.

    .EXAMPLE
    Get-RegiLatticeStatus "explorer-show-file-extensions"
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [string]$TweakId
    )
    Invoke-RegiLattice status $TweakId
}

function Set-RegiLattice {
    <#
    .SYNOPSIS
    Apply a tweak.

    .EXAMPLE
    Set-RegiLattice "explorer-show-file-extensions"
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [string]$TweakId,
        [switch]$Force,
        [switch]$Yes
    )
    $args_list = @("apply", $TweakId)
    if ($Force) { $args_list += "--force" }
    if ($Yes) { $args_list += "-y" }
    Invoke-RegiLattice @args_list
}

function Remove-RegiLattice {
    <#
    .SYNOPSIS
    Remove (revert) a tweak.

    .EXAMPLE
    Remove-RegiLattice "explorer-show-file-extensions"
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [string]$TweakId,
        [switch]$Force,
        [switch]$Yes
    )
    $args_list = @("remove", $TweakId)
    if ($Force) { $args_list += "--force" }
    if ($Yes) { $args_list += "-y" }
    Invoke-RegiLattice @args_list
}

Export-ModuleMember -Function Invoke-RegiLattice, Get-RegiLattice, Get-RegiLatticeStatus, Set-RegiLattice, Remove-RegiLattice
