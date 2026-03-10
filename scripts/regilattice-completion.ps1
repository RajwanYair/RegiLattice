# RegiLattice PowerShell argument-completion script
#
# Usage — add ONE of the following to your $PROFILE:
#
#   # Option A: dot-source this file
#   . "C:\path\to\scripts\regilattice-completion.ps1"
#
#   # Option B: if the project is on $env:PATH, just run it
#   regilattice-completion
#
# After sourcing, tab-completion works for:
#   regilattice <mode>          (apply | remove | status)
#   regilattice --log-level     (DEBUG | INFO | WARNING | ERROR | CRITICAL)
#   regilattice --profile       (business | gaming | privacy | minimal | server)
#   regilattice --diff          (same profile names)
#   regilattice --output        (table | json)
#   regilattice --scope         (user | machine | both)
#   regilattice apply  <id>     (dynamic: calls `regilattice --list` to gather IDs)
#   regilattice remove <id>     (same)
#   All long flags are completed from the static list below.

$_RegiLatticeFlags = @(
    '--assume-yes', '-y',
    '--list',
    '--force',
    '--gui',
    '--snapshot',
    '--restore',
    '--snapshot-diff',
    '--html',
    '--profile',
    '--dry-run',
    '--config',
    '--version',
    '--check-deps',
    '--hwinfo',
    '--search',
    '--category',
    '--export-json',
    '--import-json',
    '--list-profiles',
    '--categories',
    '--tags',
    '--export-reg',
    '--check',
    '--diff',
    '--output',
    '--validate',
    '--stats',
    '--list-categories',
    '--scope',
    '--min-build',
    '--corp-safe',
    '--needs-admin',
    '--doctor',
    '--log-level'
)

$_RegiLatticeModes = @('apply', 'remove', 'status')

# Cache tweak IDs to avoid re-running regilattice on every keypress
$script:_RegiLatticeTweakIdCache = $null

function _Get-RegiLatticeTweakIds {
    if ($null -ne $script:_RegiLatticeTweakIdCache) {
        return $script:_RegiLatticeTweakIdCache
    }
    try {
        $raw = & python -m regilattice --list 2>$null
        $ids = $raw |
        Select-String -Pattern '^\s+([a-z][a-z0-9-]+)' |
        ForEach-Object { $_.Matches[0].Groups[1].Value }
        $script:_RegiLatticeTweakIdCache = $ids
        return $ids
    } catch {
        return @()
    }
}

Register-ArgumentCompleter -Native -CommandName @('regilattice', 'python') -ScriptBlock {
    param($wordToComplete, $commandAst, $cursorPosition)

    $tokens = $commandAst.CommandElements | ForEach-Object { $_.ToString() }

    # Only activate for `python -m regilattice` or bare `regilattice`
    $isPythonModule = ($tokens.Count -ge 3 -and
        $tokens[0] -match 'python' -and
        $tokens[1] -eq '-m' -and
        $tokens[2] -eq 'regilattice')
    $isBare = ($tokens[0] -eq 'regilattice')

    if (-not $isPythonModule -and -not $isBare) { return }

    # Determine the "effective" argument list (strip `python -m regilattice` prefix)
    $argOffset = if ($isPythonModule) { 3 } else { 1 }
    $args = $tokens[$argOffset..($tokens.Count - 1)]

    # Find the previous token to determine what a value should complete for
    $prevToken = if ($tokens.Count -gt 1) { $tokens[$tokens.Count - 2] } else { '' }

    switch ($prevToken) {
        '--log-level' {
            @('DEBUG', 'INFO', 'WARNING', 'ERROR', 'CRITICAL') |
            Where-Object { $_ -like "$wordToComplete*" } |
            ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
            return
        }
        { $_ -in @('--profile', '--diff') } {
            @('business', 'gaming', 'privacy', 'minimal', 'server') |
            Where-Object { $_ -like "$wordToComplete*" } |
            ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
            return
        }
        '--output' {
            @('table', 'json') |
            Where-Object { $_ -like "$wordToComplete*" } |
            ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
            return
        }
        '--scope' {
            @('user', 'machine', 'both') |
            Where-Object { $_ -like "$wordToComplete*" } |
            ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
            return
        }
        { $_ -in @('apply', 'remove') } {
            # Complete tweak IDs
            $ids = _Get-RegiLatticeTweakIds
            $ids += 'all'
            $ids |
            Where-Object { $_ -like "$wordToComplete*" } |
            ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
            return
        }
    }

    # Position 0 after the command: complete mode if nothing typed yet
    $nonFlagArgs = $args | Where-Object { -not $_.StartsWith('-') }
    if ($nonFlagArgs.Count -eq 0) {
        # offer both modes and flags
        $results = @()
        $results += $_RegiLatticeModes | Where-Object { $_ -like "$wordToComplete*" } |
        ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
        $results += $_RegiLatticeFlags | Where-Object { $_ -like "$wordToComplete*" } |
        ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
        return $results
    }

    # Complete flags starting with -
    if ($wordToComplete.StartsWith('-')) {
        $_RegiLatticeFlags |
        Where-Object { $_ -like "$wordToComplete*" } |
        ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
        return
    }

    # Tweak ID position after apply/remove
    $modeToken = $nonFlagArgs | Where-Object { $_ -in $_RegiLatticeModes } | Select-Object -First 1
    if ($modeToken -in @('apply', 'remove')) {
        $ids = _Get-RegiLatticeTweakIds
        $ids += 'all'
        $ids |
        Where-Object { $_ -like "$wordToComplete*" } |
        ForEach-Object { [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_) }
    }
}

Write-Host "RegiLattice tab-completion registered. Use 'regilattice <Tab>' to complete commands." -ForegroundColor Cyan
