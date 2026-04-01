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
#  Set-RLTweak  (B4)
# ---------------------------------------------------------------------------

function Set-RLTweak {
    <#
    .SYNOPSIS
        Apply or remove a tweak by ID.

    .DESCRIPTION
        Convenience cmdlet that wraps Invoke-RLApply and Invoke-RLRemove.
        Use -Apply to activate a tweak or -Remove to revert it.
        Accepts pipeline input from Get-RLTweak.

    .PARAMETER Id
        Tweak ID. Accepts pipeline input from Get-RLTweak.

    .PARAMETER Apply
        Activate the tweak (calls RegiLattice apply <id>).

    .PARAMETER Remove
        Revert the tweak (calls RegiLattice remove <id>).

    .PARAMETER DryRun
        Preview the operation without writing to the registry.

    .PARAMETER Force
        Override the corporate-guard check.

    .EXAMPLE
        Set-RLTweak -Id 'priv-disable-telemetry' -Apply

    .EXAMPLE
        Get-RLTweak -Category Privacy | Set-RLTweak -Remove -DryRun
    #>
    [CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'Medium', DefaultParameterSetName = 'Apply')]
    param(
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]$Id,

        [Parameter(ParameterSetName = 'Apply')][switch]$Apply,
        [Parameter(ParameterSetName = 'Remove')][switch]$Remove,
        [Parameter()][switch]$DryRun,
        [Parameter()][switch]$Force
    )

    process {
        if ($Remove) {
            if (-not $PSCmdlet.ShouldProcess($Id, 'Remove tweak')) { return }
            Invoke-RLRemove -Id $Id -DryRun:$DryRun -Force:$Force
        } else {
            if (-not $PSCmdlet.ShouldProcess($Id, 'Apply tweak')) { return }
            Invoke-RLApply -Id $Id -DryRun:$DryRun -Force:$Force
        }
    }
}

# ---------------------------------------------------------------------------
#  Get-RLProfile  (B4)
# ---------------------------------------------------------------------------

function Get-RLProfile {
    <#
    .SYNOPSIS
        List available RegiLattice profiles (built-in and user-defined).

    .DESCRIPTION
        Calls `regilattice profile list --output json` and returns structured
        profile objects. Supports filtering by type (builtin / user).

    .PARAMETER Type
        Filter by profile type: 'builtin', 'user', or 'all' (default).

    .EXAMPLE
        Get-RLProfile

    .EXAMPLE
        Get-RLProfile -Type user
    #>
    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    param(
        [Parameter()]
        [ValidateSet('all', 'builtin', 'user')]
        [string]$Type = 'all'
    )

    $raw = Invoke-RLRaw @('profile', 'list', '--output', 'json', '--no-color') 2>&1
    $json = ($raw | Out-String).Trim()

    $profiles = try { $json | ConvertFrom-Json } catch { $null }
    if (-not $profiles) {
        Write-Warning "Could not parse profile list: $json"
        return
    }

    foreach ($p in $profiles) {
        if ($Type -ne 'all' -and $p.Type -ne $Type) { continue }
        [PSCustomObject]@{
            PSTypeName  = 'RegiLattice.Profile'
            Name        = $p.Name
            Description = $p.Description
            TweakCount  = $p.TweakCount
            Type        = $p.Type
        }
    }
}

# ---------------------------------------------------------------------------
#  Set-RLProfile  (B4)
# ---------------------------------------------------------------------------

function Set-RLProfile {
    <#
    .SYNOPSIS
        Apply a RegiLattice profile by name.

    .DESCRIPTION
        Calls `regilattice profile apply <name>`. All tweaks in the profile
        are applied in sequence. Use -DryRun to preview.

    .PARAMETER Name
        Profile name (e.g. 'gaming', 'privacy', 'minimal').

    .PARAMETER DryRun
        Preview the operation without writing to the registry.

    .PARAMETER Force
        Override the corporate-guard check.

    .EXAMPLE
        Set-RLProfile -Name privacy

    .EXAMPLE
        Set-RLProfile -Name gaming -DryRun
    #>
    [CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'High')]
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter()][switch]$DryRun,
        [Parameter()][switch]$Force
    )

    if (-not $PSCmdlet.ShouldProcess($Name, 'Apply profile')) { return }

    $pArgs = [System.Collections.Generic.List[string]]::new()
    $pArgs.Add('profile')
    $pArgs.Add('apply')
    $pArgs.Add($Name)
    $pArgs.Add('--no-color')
    if ($DryRun) { $pArgs.Add('--dry-run') }
    if ($Force)  { $pArgs.Add('--force') }

    $raw = Invoke-RLRaw $pArgs
    Write-Output ($raw | Out-String).Trim()
}

# ---------------------------------------------------------------------------
#  Export-RLSnapshot  (B4)
# ---------------------------------------------------------------------------

function Export-RLSnapshot {
    <#
    .SYNOPSIS
        Save the current registry tweak state to a snapshot JSON file.

    .DESCRIPTION
        Calls `regilattice snapshot save <path>`. The resulting JSON can be
        used with Restore-RLSnapshot or supplied to batch apply.

    .PARAMETER Path
        Destination path for the snapshot file.

    .EXAMPLE
        Export-RLSnapshot -Path "$env:USERPROFILE\Desktop\my-state.json"
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    if (-not $PSCmdlet.ShouldProcess($Path, 'Save snapshot')) { return }

    $raw = Invoke-RLRaw @('snapshot', 'save', $Path, '--no-color')
    Write-Output ($raw | Out-String).Trim()
}

# ---------------------------------------------------------------------------
#  Restore-RLSnapshot  (B4)
# ---------------------------------------------------------------------------

function Restore-RLSnapshot {
    <#
    .SYNOPSIS
        Restore tweak state from a snapshot JSON file.

    .DESCRIPTION
        Calls `regilattice snapshot restore <path>`. Applies all tweaks whose
        IDs are recorded in the snapshot as applied.

    .PARAMETER Path
        Path to the snapshot JSON file previously created by Export-RLSnapshot.

    .PARAMETER DryRun
        Preview the restore without writing to the registry.

    .PARAMETER Force
        Override the corporate-guard check.

    .EXAMPLE
        Restore-RLSnapshot -Path "$env:USERPROFILE\Desktop\my-state.json"
    #>
    [CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'High')]
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter()][switch]$DryRun,
        [Parameter()][switch]$Force
    )

    if (-not $PSCmdlet.ShouldProcess($Path, 'Restore snapshot')) { return }

    $pArgs = [System.Collections.Generic.List[string]]::new()
    $pArgs.Add('snapshot')
    $pArgs.Add('restore')
    $pArgs.Add($Path)
    $pArgs.Add('--no-color')
    if ($DryRun) { $pArgs.Add('--dry-run') }
    if ($Force)  { $pArgs.Add('--force') }

    $raw = Invoke-RLRaw $pArgs
    Write-Output ($raw | Out-String).Trim()
}

# ---------------------------------------------------------------------------
#  Invoke-RLBatch  (B4/B7)
# ---------------------------------------------------------------------------

function Invoke-RLBatch {
    <#
    .SYNOPSIS
        Apply or remove a batch of tweaks from a file.

    .DESCRIPTION
        Calls `regilattice batch apply|remove <file>`. The file may be a plain
        text file with one ID per line, a JSON array, or a snapshot JSON.

    .PARAMETER Path
        Path to the batch file (text, JSON array, or snapshot JSON).

    .PARAMETER Remove
        When specified, removes the listed tweaks instead of applying them.

    .PARAMETER DryRun
        Preview operations without writing to the registry.

    .PARAMETER Force
        Override the corporate-guard check.

    .EXAMPLE
        Invoke-RLBatch -Path ids.txt

    .EXAMPLE
        Invoke-RLBatch -Path privacy-snapshot.json -Remove -DryRun
    #>
    [CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'Medium')]
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter()][switch]$Remove,
        [Parameter()][switch]$DryRun,
        [Parameter()][switch]$Force
    )

    $verb = if ($Remove) { 'remove' } else { 'apply' }
    if (-not $PSCmdlet.ShouldProcess($Path, "Batch $verb")) { return }

    $pArgs = [System.Collections.Generic.List[string]]::new()
    $pArgs.Add('batch')
    $pArgs.Add($verb)
    $pArgs.Add($Path)
    $pArgs.Add('--no-color')
    if ($DryRun) { $pArgs.Add('--dry-run') }
    if ($Force)  { $pArgs.Add('--force') }
    $pArgs.Add('--assume-yes')

    $raw = Invoke-RLRaw $pArgs
    Write-Output ($raw | Out-String).Trim()
}

# ---------------------------------------------------------------------------
#  Type formatting hints (Format-Table defaults)
# ---------------------------------------------------------------------------

Update-TypeData -TypeName 'RegiLattice.Tweak' -DefaultDisplayPropertySet Id, Label, Category, Scope, NeedsAdmin -Force
Update-TypeData -TypeName 'RegiLattice.TweakStatus' -DefaultDisplayPropertySet Id, Status -Force
Update-TypeData -TypeName 'RegiLattice.HealthScore' -DefaultDisplayPropertySet HealthScore, Grade, Applied, Pending, TotalTweaks -Force
Update-TypeData -TypeName 'RegiLattice.Profile' -DefaultDisplayPropertySet Name, Type, TweakCount, Description -Force

# ---------------------------------------------------------------------------
#  Aliases
# ---------------------------------------------------------------------------

Set-Alias -Name grt  -Value Get-RLTweak        -Scope Global -Force
Set-Alias -Name grts -Value Get-RLTweakStatus  -Scope Global -Force
Set-Alias -Name ira  -Value Invoke-RLApply     -Scope Global -Force
Set-Alias -Name irr  -Value Invoke-RLRemove    -Scope Global -Force
Set-Alias -Name srt  -Value Set-RLTweak        -Scope Global -Force
Set-Alias -Name grp  -Value Get-RLProfile      -Scope Global -Force
Set-Alias -Name srp  -Value Set-RLProfile      -Scope Global -Force
Set-Alias -Name irb  -Value Invoke-RLBatch     -Scope Global -Force

Export-ModuleMember -Function `
        Get-RLTweak, Get-RLTweakStatus, Invoke-RLApply, Invoke-RLRemove, Get-RLHealthScore, `
        Set-RLTweak, Get-RLProfile, Set-RLProfile, Export-RLSnapshot, Restore-RLSnapshot, Invoke-RLBatch `
    -Alias grt, grts, ira, irr, srt, grp, srp, irb
