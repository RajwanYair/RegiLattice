#Requires -Version 5.1
Set-StrictMode -Version Latest

<#
.SYNOPSIS
    RegiLattice PowerShell module — pipeline-native interface to the RegiLattice tweak engine.

.DESCRIPTION
    Wraps the RegiLattice CLI (RegiLattice.exe) to expose 5 cmdlets that integrate cleanly
    with the PowerShell pipeline, tab completion, and -WhatIf / -Confirm semantics.

    Cmdlets:
        Get-RLTweak          — list / filter all registered tweaks
        Get-RLTweakStatus    — query the live apply-state of one or more tweaks
        Invoke-RLApply       — apply a tweak (supports -DryRun, -Force, -WhatIf)
        Invoke-RLRemove      — remove/revert a tweak (supports -DryRun, -Force, -WhatIf)
        Get-RLHealthScore    — summarise the system's overall tweak-health percentage

    Quick start:
        Import-Module .\RegiLattice.psd1
        Get-RLTweak -Category Privacy | Select-Object Id, Label, NeedsAdmin
        Get-RLTweak -Search telemetry | Invoke-RLApply -DryRun
        Get-RLHealthScore
#>

# ---------------------------------------------------------------------------
#  Internal helpers
# ---------------------------------------------------------------------------

$script:RLExePath = $null
$script:ExportCache = $null

function Find-RLExe {
    <# Locate the RegiLattice.exe binary, caching the result. #>
    if ($script:RLExePath -and (Test-Path $script:RLExePath)) {
        return $script:RLExePath
    }
    $candidates = @(
        (Join-Path $PSScriptRoot 'RegiLattice.exe'),
        (Join-Path $PSScriptRoot '..' 'cli' 'RegiLattice.exe'),
        (Join-Path $PSScriptRoot '..' 'RegiLattice.exe'),
        (Get-Command 'RegiLattice' -ErrorAction SilentlyContinue |
        Select-Object -ExpandProperty Source -ErrorAction SilentlyContinue)
    )
    foreach ($c in $candidates) {
        if ($c -and (Test-Path $c)) {
            $script:RLExePath = $c
            return $c
        }
    }
    throw [System.IO.FileNotFoundException]::new(
        'RegiLattice.exe not found. Publish the CLI with ''dotnet publish'' and ensure ' +
        'the exe is in the same folder as this module, or add it to PATH.')
}

function Invoke-RLRaw {
    <# Pass raw arguments to RegiLattice.exe and return combined stdout+stderr. #>
    param([string[]]$Arguments)
    $exe = Find-RLExe
    & $exe @Arguments 2>&1
}

function Export-TweakJson {
    <# Export all tweaks to a temp JSON file and return the parsed array. #>
    $exe = Find-RLExe
    $tmp = [IO.Path]::ChangeExtension([IO.Path]::GetTempFileName(), '.json')
    try {
        & $exe --export-json $tmp --no-color 2>&1 | Out-Null
        if (-not (Test-Path $tmp)) {
            throw 'RegiLattice --export-json did not produce a file.'
        }
        Get-Content -Raw $tmp | ConvertFrom-Json
    } finally {
        Remove-Item $tmp -ErrorAction SilentlyContinue
    }
}

# ---------------------------------------------------------------------------
#  Get-RLTweak
# ---------------------------------------------------------------------------

function Get-RLTweak {
    <#
    .SYNOPSIS
        List all RegiLattice tweaks, with optional filtering.

    .DESCRIPTION
        Queries the RegiLattice tweak registry via --export-json and returns
        one PSCustomObject per tweak. Accepts pipeline input and supports
        filtering by category, free-text search, admin requirement, and
        corp-safe flag.

    .PARAMETER Category
        Filter by exact category name (e.g. 'Privacy', 'Performance').

    .PARAMETER Search
        Filter tweaks whose Label or Description match the given string/pattern.

    .PARAMETER NeedsAdmin
        When set, return only tweaks that require elevated privileges.

    .PARAMETER CorpSafe
        When set, return only tweaks marked safe for corporate environments.

    .EXAMPLE
        Get-RLTweak
        # Returns all ~4 000 registered tweaks.

    .EXAMPLE
        Get-RLTweak -Category Privacy | Select-Object Id, Label | Format-Table

    .EXAMPLE
        Get-RLTweak -Search telemetry | Where-Object NeedsAdmin -eq $true
    #>
    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    param(
        [Parameter()][string]$Category,
        [Parameter()][string]$Search,
        [Parameter()][switch]$NeedsAdmin,
        [Parameter()][switch]$CorpSafe
    )

    $tweaks = Export-TweakJson

    if ($Category) { $tweaks = $tweaks | Where-Object { $_.Category -eq $Category } }
    if ($Search) { $tweaks = $tweaks | Where-Object { $_.Label -match $Search -or $_.Description -match $Search } }
    if ($NeedsAdmin) { $tweaks = $tweaks | Where-Object { $_.NeedsAdmin -eq $true } }
    if ($CorpSafe) { $tweaks = $tweaks | Where-Object { $_.CorpSafe -eq $true } }

    foreach ($t in $tweaks) {
        [PSCustomObject]@{
            PSTypeName  = 'RegiLattice.Tweak'
            Id          = $t.Id
            Label       = $t.Label
            Category    = $t.Category
            NeedsAdmin  = [bool]$t.NeedsAdmin
            CorpSafe    = [bool]$t.CorpSafe
            Description = $t.Description
            Tags        = $t.Tags -join ', '
            Scope       = $t.Scope
        }
    }
}

# ---------------------------------------------------------------------------
#  Get-RLTweakStatus
# ---------------------------------------------------------------------------

function Get-RLTweakStatus {
    <#
    .SYNOPSIS
        Query the live apply-state of one or more tweaks by ID.

    .DESCRIPTION
        Calls `RegiLattice.exe status <id>` for each ID and returns a
        structured result object with Id, Status (Applied / NotApplied /
        Unknown), and the raw CLI output.

    .PARAMETER Id
        One or more tweak IDs. Accepts pipeline input from Get-RLTweak.

    .EXAMPLE
        Get-RLTweakStatus -Id 'priv-disable-telemetry'

    .EXAMPLE
        Get-RLTweak -Category Privacy | Get-RLTweakStatus | Where-Object Status -eq 'Applied'
    #>
    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    param(
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Id
    )

    process {
        $raw = Invoke-RLRaw @('status', $Id, '--no-color') 2>&1
        $out = ($raw | Out-String).Trim()

        $status = switch -Regex ($out) {
            'Applied' { 'Applied'; break }
            'NotApplied|Not Applied' { 'NotApplied'; break }
            default { 'Unknown' }
        }

        [PSCustomObject]@{
            PSTypeName = 'RegiLattice.TweakStatus'
            Id         = $Id
            Status     = $status
            Output     = $out
        }
    }
}

# ---------------------------------------------------------------------------
#  Invoke-RLApply
# ---------------------------------------------------------------------------

function Invoke-RLApply {
    <#
    .SYNOPSIS
        Apply a tweak by ID.

    .DESCRIPTION
        Calls `RegiLattice.exe apply <id>`. Supports -DryRun to preview
        registry writes without committing them, and -WhatIf/-Confirm via
        PowerShell's standard ShouldProcess mechanism.

    .PARAMETER Id
        The tweak ID to apply. Accepts pipeline input from Get-RLTweak.

    .PARAMETER DryRun
        Preview the operation without writing to the registry.

    .PARAMETER Force
        Override the corporate-guard check.

    .EXAMPLE
        Invoke-RLApply -Id 'priv-disable-telemetry'

    .EXAMPLE
        Get-RLTweak -Category Privacy | Invoke-RLApply -DryRun

    .EXAMPLE
        Invoke-RLApply 'game-disable-fth' -Force
    #>
    [CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'Medium')]
    param(
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Id,

        [Parameter()][switch]$DryRun,
        [Parameter()][switch]$Force
    )

    process {
        if (-not $PSCmdlet.ShouldProcess($Id, 'Apply tweak')) { return }

        $args = [System.Collections.Generic.List[string]]::new()
        $args.Add('apply')
        $args.Add($Id)
        $args.Add('--no-color')
        if ($DryRun) { $args.Add('--dry-run') }
        if ($Force) { $args.Add('--force') }

        $raw = Invoke-RLRaw $args
        Write-Output ($raw | Out-String).Trim()
    }
}

# ---------------------------------------------------------------------------
#  Invoke-RLRemove
# ---------------------------------------------------------------------------

function Invoke-RLRemove {
    <#
    .SYNOPSIS
        Remove (revert) a tweak by ID.

    .DESCRIPTION
        Calls `RegiLattice.exe remove <id>`. Supports -DryRun to preview
        the revert without writing to the registry.

    .PARAMETER Id
        The tweak ID to remove. Accepts pipeline input from Get-RLTweak.

    .PARAMETER DryRun
        Preview the operation without writing to the registry.

    .PARAMETER Force
        Override the corporate-guard check.

    .EXAMPLE
        Invoke-RLRemove -Id 'priv-disable-telemetry'

    .EXAMPLE
        Get-RLTweak -Category Privacy | Get-RLTweakStatus |
            Where-Object Status -eq 'Applied' | Invoke-RLRemove -DryRun
    #>
    [CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'High')]
    param(
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Id,

        [Parameter()][switch]$DryRun,
        [Parameter()][switch]$Force
    )

    process {
        if (-not $PSCmdlet.ShouldProcess($Id, 'Remove tweak')) { return }

        $args = [System.Collections.Generic.List[string]]::new()
        $args.Add('remove')
        $args.Add($Id)
        $args.Add('--no-color')
        if ($DryRun) { $args.Add('--dry-run') }
        if ($Force) { $args.Add('--force') }

        $raw = Invoke-RLRaw $args
        Write-Output ($raw | Out-String).Trim()
    }
}

# ---------------------------------------------------------------------------
#  Get-RLHealthScore
# ---------------------------------------------------------------------------

function Get-RLHealthScore {
    <#
    .SYNOPSIS
        Return an overall tweak-health score for the current system.

    .DESCRIPTION
        Calls `RegiLattice.exe --stats` and parses the key counters (total,
        applied, categories) to compute a health percentage.
        HealthScore = applied / (applied + pending) × 100.

    .EXAMPLE
        Get-RLHealthScore

    .EXAMPLE
        $score = Get-RLHealthScore
        if ($score.HealthScore -lt 50) { Write-Warning "System health below 50%" }
    #>
    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    param()

    $raw = Invoke-RLRaw @('--stats', '--no-color')
    $out = ($raw | Out-String)

    $total = if ($out -match 'Total[^\d]*(\d+)') { [int]$Matches[1] } else { 0 }
    $applied = if ($out -match '[Aa]pplied[^\d]*(\d+)') { [int]$Matches[1] } else { 0 }
    $cats = if ($out -match '[Cc]ategor\w+[^\d]*(\d+)') { [int]$Matches[1] } else { 0 }
    $health = if (($total) -gt 0) { [Math]::Round($applied / $total * 100, 1) } else { 0 }

    [PSCustomObject]@{
        PSTypeName  = 'RegiLattice.HealthScore'
        TotalTweaks = $total
        Applied     = $applied
        Pending     = $total - $applied
        Categories  = $cats
        HealthScore = $health
        Grade       = switch ($health) {
            { $_ -ge 80 } { 'A'; break }
            { $_ -ge 60 } { 'B'; break }
            { $_ -ge 40 } { 'C'; break }
            { $_ -ge 20 } { 'D'; break }
            default { 'F' }
        }
        RawOutput   = $out.Trim()
    }
}

# ---------------------------------------------------------------------------
#  Type formatting hints (Format-Table defaults)
# ---------------------------------------------------------------------------

Update-TypeData -TypeName 'RegiLattice.Tweak' -DefaultDisplayPropertySet Id, Label, Category, Scope, NeedsAdmin -Force
Update-TypeData -TypeName 'RegiLattice.TweakStatus' -DefaultDisplayPropertySet Id, Status -Force
Update-TypeData -TypeName 'RegiLattice.HealthScore' -DefaultDisplayPropertySet HealthScore, Grade, Applied, Pending, TotalTweaks -Force

# ---------------------------------------------------------------------------
#  Aliases
# ---------------------------------------------------------------------------

Set-Alias -Name grt  -Value Get-RLTweak        -Scope Global -Force
Set-Alias -Name grts -Value Get-RLTweakStatus  -Scope Global -Force
Set-Alias -Name ira  -Value Invoke-RLApply     -Scope Global -Force
Set-Alias -Name irr  -Value Invoke-RLRemove    -Scope Global -Force

Export-ModuleMember -Function Get-RLTweak, Get-RLTweakStatus, Invoke-RLApply, Invoke-RLRemove, Get-RLHealthScore `
    -Alias    grt, grts, ira, irr
