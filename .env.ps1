param(
    [switch]$SkipCommonBootstrap
)

$ErrorActionPreference = 'Stop'

$commonBootstrap = Join-Path (Split-Path $PSScriptRoot -Parent) '.vscode\scripts\Initialize-CommonTooling.ps1'

if (-not $SkipCommonBootstrap -and (Test-Path $commonBootstrap -PathType Leaf)) {
    & $commonBootstrap -WorkspaceRoot $PSScriptRoot -WorkspaceBootstrap $PSCommandPath
    return
}

if ($env:REGILATTICE_ENV_LOADED -eq '1') {
    return
}

$env:REGILATTICE_ENV_LOADED = '1'

function global:Invoke-RLBuild {
    dotnet build RegiLattice.sln @args
}

function global:Invoke-RLTest {
    dotnet test RegiLattice.sln --logger "console;verbosity=normal" @args
}

function global:Invoke-RLGui {
    dotnet run --project src/RegiLattice.GUI @args
}

function global:Invoke-RLCli {
    dotnet run --project src/RegiLattice.CLI -- @args
}

if (-not (Get-Alias -Name 'rlbuild' -ErrorAction SilentlyContinue)) {
    Set-Alias -Name 'rlbuild' -Value 'Invoke-RLBuild' -Option ReadOnly -ErrorAction SilentlyContinue
}

if (-not (Get-Alias -Name 'rltest' -ErrorAction SilentlyContinue)) {
    Set-Alias -Name 'rltest' -Value 'Invoke-RLTest' -Option ReadOnly -ErrorAction SilentlyContinue
}

$script:RegiLatticeTestProjects = @(
    'tests/RegiLattice.Core.Tests'
    'tests/RegiLattice.CLI.Tests'
    'tests/RegiLattice.GUI.Tests'
)

Register-ArgumentCompleter -CommandName 'dotnet' -ScriptBlock {
    param($wordToComplete, $commandAst, $cursorPosition)

    $cmdText = $commandAst.ToString()
    if ($cmdText -match 'dotnet\s+test\s+') {
        $script:RegiLatticeTestProjects |
        Where-Object { $_ -like "$wordToComplete*" } |
        ForEach-Object {
            [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
        }
    }
}

Write-Host '[regilattice-env] Loaded workspace bootstrap on top of MyScripts common tooling' -ForegroundColor DarkCyan
Write-Host '  Helpers: Invoke-RLBuild, Invoke-RLTest, Invoke-RLGui, Invoke-RLCli' -ForegroundColor DarkGray
