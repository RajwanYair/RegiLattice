# RegiLattice PowerShell Tab Completion
# Source this file (or add to your $PROFILE) to enable tab completion
# for the 'regilattice' / 'rl' command aliases.
#
# Usage: . "$env:LOCALAPPDATA\RegiLattice\completions\RegiLattice.ps1"
# Or add the dot-source line to your PowerShell $PROFILE.

# ── B6: Extended subcommand list ─────────────────────────────────────────────
# Each top-level verb that can appear as the first positional argument.
$_rl_subcommands = @(
    'tweak', 'search', 'list', 'validate', 'stats', 'doctor', 'check',
    'profile', 'snapshot', 'export', 'import', 'marketplace',
    'batch', 'apply', 'remove', 'status', 'update'
)

# Second-level nouns for each verb (used for context-aware completion)
$_rl_nouns = @{
    tweak       = @('apply', 'remove', 'update', 'status', 'list')
    profile     = @('apply', 'list', 'create', 'delete')
    snapshot    = @('save', 'restore')
    export      = @('json', 'reg', 'gpo', 'intune', 'config')
    import      = @('json', 'config')
    marketplace = @('list', 'search', 'install', 'uninstall', 'installed', 'update')
    batch       = @('apply', 'remove')
}

# All recognised flag options
$_rl_flags = @(
    '--list', '--list-profiles', '--list-user-profiles', '--list-categories', '--categories',
    '--tags', '--stats', '--validate', '--report', '--check', '--doctor', '--hwinfo',
    '--dry-run', '--force', '--gui', '--menu', '--no-color', '--assume-yes', '-y',
    '--search', '--profile', '--category', '--scope', '--min-build',
    '--snapshot', '--restore', '--snapshot-diff', '--export-json', '--export-reg',
    '--import-json', '--diff', '--depends-on', '--output', '--html',
    '--export-config', '--import-config',
    '--favorites', '--favorite-add', '--favorite-remove',
    '--history',
    '--compliance', '--compliance-history', '--compliance-report',
    '--marketplace',
    '--export-gpo', '--export-intune',
    '--generate-ps-module',
    '--html-report',
    '--new-pack',
    '--manager',
    '--portable', '--silent', '--log-file',
    '--config',
    '--profile-create', '--profile-delete', '--profile-clone', '--profile-rename',
    '--profile-tweaks', '--profile-desc',
    '--help', '-h', '--version', '-V'
)

# Built-in profile names
$_rl_builtin_profiles = @('business', 'gaming', 'privacy', 'minimal', 'server')

# Helper: get user-defined profile names from disk (lazy, non-blocking)
function _rl_user_profiles {
    $dir = Join-Path $env:LOCALAPPDATA 'RegiLattice\profiles'
    if (Test-Path $dir) {
        Get-ChildItem $dir -Filter '*.json' | ForEach-Object { $_.BaseName }
    }
}

# Helper: all profile names (built-in + user)
function _rl_all_profiles {
    @($_rl_builtin_profiles) + @(_rl_user_profiles)
}

# Helper: build a completion result list from an array of candidates filtered by the word being completed
function _rl_complete_from([string]$word, [string[]]$candidates) {
    $candidates | Where-Object { $_ -like "$word*" } | ForEach-Object {
        [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
    }
}

Register-ArgumentCompleter -Native -CommandName @('regilattice', 'rl', 'RegiLattice') -ScriptBlock {
    param($wordToComplete, $commandAst, $cursorPosition)

    $tokens = $commandAst.CommandElements
    $tokenStrings = $tokens | ForEach-Object { $_.ToString() }
    $prevToken = if ($tokens.Count -ge 2) { $tokens[$tokens.Count - 2].ToString() } else { '' }

    # Context-aware: if we know the verb (first positional), offer its nouns
    $verb = if ($tokens.Count -ge 2 -and $tokenStrings[1] -in $_rl_subcommands) { $tokenStrings[1] } else { '' }
    $isOnNounPosition = $tokens.Count -eq 3 -and -not $wordToComplete.StartsWith('-') -and $verb -ne ''

    if ($isOnNounPosition -and $_rl_nouns[$verb]) {
        return _rl_complete_from $wordToComplete $_rl_nouns[$verb]
    }

    switch ($prevToken) {
        '--profile' { return _rl_complete_from $wordToComplete (_rl_all_profiles) }
        '--profile-create' { return @() }  # free-form name
        '--profile-delete' { return _rl_complete_from $wordToComplete (_rl_user_profiles) }
        '--profile-clone' {
            return _rl_complete_from $wordToComplete (_rl_user_profiles)
        }
        '--profile-rename' {
            return _rl_complete_from $wordToComplete (_rl_user_profiles)
        }
        '--diff' { return _rl_complete_from $wordToComplete (_rl_all_profiles) }
        '--category' {
            $cats = & regilattice --categories --no-color 2>$null |
            Where-Object { $_ -match '^\s+\S' } |
            ForEach-Object { $_.Trim().Split(' ')[0] }
            return _rl_complete_from $wordToComplete $cats
        }
        '--scope' { return _rl_complete_from $wordToComplete @('user', 'machine', 'both') }
        '--output' { return _rl_complete_from $wordToComplete @('table', 'json', 'csv') }
        '--manager' {
            return _rl_complete_from $wordToComplete @('scoop', 'choco', 'winget', 'pip', 'psmodule')
        }
        '--marketplace' {
            return _rl_complete_from $wordToComplete @('list', 'install', 'update', 'info')
        }
        default {
            $hasSubCmd = $tokenStrings | Where-Object { $_ -in $_rl_subcommands }
            if (-not $hasSubCmd) {
                $all = @($_rl_subcommands) + @($_rl_flags)
            } else {
                $all = $_rl_flags
            }
            return _rl_complete_from $wordToComplete $all
        }
    }
}

# ── B6: Parameter completers for PS module cmdlets ───────────────────────────

Register-ArgumentCompleter -CommandName 'Set-RLTweak' -ParameterName 'Id' -ScriptBlock {
    param($commandName, $paramName, $wordToComplete)
    if (-not (Get-Command regilattice -ErrorAction SilentlyContinue)) { return }
    & regilattice list --output json --no-color 2>$null | ConvertFrom-Json |
        Where-Object { $_.Id -like "$wordToComplete*" } |
        ForEach-Object {
            [System.Management.Automation.CompletionResult]::new($_.Id, $_.Id, 'ParameterValue', $_.Label)
        }
}

Register-ArgumentCompleter -CommandName @('Invoke-RLApply', 'Invoke-RLRemove', 'Get-RLTweakStatus') -ParameterName 'Id' -ScriptBlock {
    param($commandName, $paramName, $wordToComplete)
    if (-not (Get-Command regilattice -ErrorAction SilentlyContinue)) { return }
    & regilattice list --output json --no-color 2>$null | ConvertFrom-Json |
        Where-Object { $_.Id -like "$wordToComplete*" } |
        ForEach-Object {
            [System.Management.Automation.CompletionResult]::new($_.Id, $_.Id, 'ParameterValue', $_.Label)
        }
}

Register-ArgumentCompleter -CommandName @('Set-RLProfile', 'Get-RLProfile') -ParameterName 'Name' -ScriptBlock {
    param($commandName, $paramName, $wordToComplete)
    _rl_all_profiles | Where-Object { $_ -like "$wordToComplete*" } | ForEach-Object {
        [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
    }
}

Write-Host "RegiLattice tab completion loaded. Type 'regilattice <Tab>' or 'Set-RLTweak -Id <Tab>' to start." -ForegroundColor DarkGray
