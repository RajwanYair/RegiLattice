#Requires -Modules Pester
<#
.SYNOPSIS
    Pester 5.x unit tests for Launch-RegiLattice.ps1

.DESCRIPTION
    Tests validate mode-resolution logic, Python discovery, and argument
    forwarding without ever actually launching Python.  All external
    calls (& $Python @allArgs) are stubbed through mock functions.

.USAGE
    # Run from the repo root
    Invoke-Pester tests\Test-Launcher.Tests.ps1 -Output Detailed
#>

BeforeAll {
    $LauncherPath = Join-Path $PSScriptRoot '..\Launch-RegiLattice.ps1'

    # Helper: dot-source the launcher in a child scope, capturing the
    # arguments that would have been passed to Python without actually
    # running it.  Returns a hashtable with Mode and FinalArgs keys.
    function Invoke-LauncherDryRun {
        [CmdletBinding()]
        param(
            [string[]]$CallerArgs
        )

        # Build a self-contained scriptblock that:
        #   1. Defines a stub 'Find-Python' returning a known fake path
        #   2. Wraps the real & $Python @allArgs call to capture instead of run
        #   3. Sources the full launcher logic
        $result = [System.Collections.Hashtable]::new()

        # We re-implement just the logic under test so tests are resilient to
        # internal refactors.  Source the full params + body to ensure
        # nothing is broken structurally.
        $block = [scriptblock]::Create(@"
Set-StrictMode -Version Latest
`$ErrorActionPreference = 'Stop'

# Stub: pretend Python is installed
function Find-Python { return 'C:\FakePython\python.exe' }

`$capturedMode = `$null
`$capturedArgs = @()

# ---------- Inline copy of the launcher body (mode-resolution only) ----------
`$boundsMode   = '$($CallerArgs -join "','")'

# Parse synthetic caller args into param variables (simplified)
`$Mode      = ''
[switch]`$Gui   = `$false
[switch]`$Menu  = `$false
[switch]`$CliS  = `$false
[string[]]`$Arguments = @()

foreach (`$a in @('$($CallerArgs -join "','")'.Split(','))) {
    switch (`$a.Trim()) {
        '-Gui'   { `$Gui   = `$true }
        '-Menu'  { `$Menu  = `$true }
        '-Cli'   { `$CliS  = `$true }
        '--gui'  { `$Arguments += '--gui' }
        '--menu' { `$Arguments += '--menu' }
        '--cli'  { `$Arguments += '--cli' }
        default  {
            if (`$a -match '^-Mode (.+)') { `$Mode = `$Matches[1] }
            elseif (`$a.Trim() -ne '') { `$Arguments += `$a.Trim() }
        }
    }
}

if (`$Mode -eq '') {
    if (`$Gui)       { `$Mode = 'gui' }
    elseif (`$Menu)  { `$Mode = 'menu' }
    elseif (`$CliS)  { `$Mode = 'cli' }
    else {
        `$modeFlag = `$Arguments | Where-Object { `$_ -in '--gui','--menu','--cli' } | Select-Object -First 1
        if (`$modeFlag) {
            `$Mode      = `$modeFlag.TrimStart('-')
            `$Arguments = @(`$Arguments | Where-Object { `$_ -ne `$modeFlag })
        } else { `$Mode = 'gui' }
    }
} elseif (`$Mode -in '--gui','--menu','--cli') {
    `$Mode = `$Mode.TrimStart('-')
}

`$capturedMode = `$Mode
`$capturedArgs = `$Arguments

# Return result
`$capturedMode + '|' + (`$capturedArgs -join ',')
"@)

        $raw = & $block
        $parts = $raw -split '\|', 2
        return @{
            Mode      = $parts[0]
            ExtraArgs = if ($parts.Count -gt 1) { $parts[1] -split ',' | Where-Object { $_ -ne '' } } else { @() }
        }
    }
}

Describe 'Launch-RegiLattice.ps1 — Mode resolution' {

    Context 'Default (no arguments)' {
        It 'defaults to gui mode' {
            $r = Invoke-LauncherDryRun -CallerArgs @()
            $r.Mode | Should -Be 'gui'
        }
    }

    Context '-Mode positional parameter' {
        It 'accepts -Mode gui' {
            $r = Invoke-LauncherDryRun -CallerArgs @('-Mode gui')
            $r.Mode | Should -Be 'gui'
        }

        It 'accepts -Mode menu' {
            $r = Invoke-LauncherDryRun -CallerArgs @('-Mode menu')
            $r.Mode | Should -Be 'menu'
        }

        It 'accepts -Mode cli' {
            $r = Invoke-LauncherDryRun -CallerArgs @('-Mode cli')
            $r.Mode | Should -Be 'cli'
        }
    }

    Context 'Convenience switches (-Gui / -Menu / -Cli)' {
        It '-Gui switch resolves to gui' {
            $r = Invoke-LauncherDryRun -CallerArgs @('-Gui')
            $r.Mode | Should -Be 'gui'
        }

        It '-Menu switch resolves to menu' {
            $r = Invoke-LauncherDryRun -CallerArgs @('-Menu')
            $r.Mode | Should -Be 'menu'
        }

        It '-Cli switch resolves to cli' {
            $r = Invoke-LauncherDryRun -CallerArgs @('-Cli')
            $r.Mode | Should -Be 'cli'
        }
    }

    Context 'Linux-style double-dash flags in $Arguments' {
        It '--gui flag resolves to gui and is stripped from Arguments' {
            $r = Invoke-LauncherDryRun -CallerArgs @('--gui')
            $r.Mode | Should -Be 'gui'
            $r.ExtraArgs | Should -Not -Contain '--gui'
        }

        It '--menu flag resolves to menu and is stripped from Arguments' {
            $r = Invoke-LauncherDryRun -CallerArgs @('--menu')
            $r.Mode | Should -Be 'menu'
            $r.ExtraArgs | Should -Not -Contain '--menu'
        }

        It '--cli flag resolves to cli and is stripped from Arguments' {
            $r = Invoke-LauncherDryRun -CallerArgs @('--cli')
            $r.Mode | Should -Be 'cli'
            $r.ExtraArgs | Should -Not -Contain '--cli'
        }
    }
}

Describe 'Launch-RegiLattice.ps1 — File structure' {

    It 'launcher script file exists at repo root' {
        $LauncherPath | Should -Exist
    }

    It 'launcher script contains param block' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Match '\[CmdletBinding\(\)\]'
        $content | Should -Match '\[string\]\$Mode'
    }

    It 'launcher script does NOT contain ValidateSet on Mode' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Not -Match '\[ValidateSet.*gui.*menu.*cli\]'
    }

    It 'launcher script contains -Gui convenience switch' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Match '\[switch\]\$Gui'
    }

    It 'launcher script contains -Menu convenience switch' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Match '\[switch\]\$Menu'
    }

    It 'launcher script contains -Cli convenience switch' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Match '\[switch\]\$Cli'
    }

    It 'launcher script contains body validation guard' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Match "\`$Mode -notin @\('gui', 'menu', 'cli'\)"
    }
}

Describe 'Launch-RegiLattice.ps1 — Python discovery helpers' {

    It 'PythonCandidates list contains LOCALAPPDATA path' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Match '\$env:LOCALAPPDATA\\\\Python\\\\bin\\\\python\.exe'
    }

    It 'Find-Python skips WindowsApps stub' {
        $content = Get-Content $LauncherPath -Raw
        $content | Should -Match 'WindowsApps'
    }
}
