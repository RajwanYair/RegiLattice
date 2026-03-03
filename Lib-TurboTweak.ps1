#Requires -Version 5.1
# Lib-TurboTweak.ps1
# Shared utility library — elevation, logging, confirmation, explorer restart.

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# ── Paths ──────────────────────────────────────────────────────────────────────
$script:TT_ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:TT_LogPath = Join-Path $TT_ScriptDir 'TurboTweak.log'

# ── Logging ────────────────────────────────────────────────────────────────────
function Write-TurboLog {
    <#
    .SYNOPSIS Appends a timestamped line to TurboTweak.log.
    #>
    param ([Parameter(Mandatory)][string]$Message)
    $ts = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
    Add-Content -Path $script:TT_LogPath -Value "$ts : $Message" -Encoding UTF8
}

# ── Elevation ──────────────────────────────────────────────────────────────────
function Test-Elevated {
    <#
    .SYNOPSIS Returns $true when the current session runs elevated.
    #>
    ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()
    ).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Assert-Elevated {
    <#
    .SYNOPSIS Relaunches the current script elevated when required.
    .DESCRIPTION If the session is not elevated and -Required is set the
                 script is relaunched via RunAs and the caller returns.
    #>
    param ([switch]$Required = $true)
    if (-not $Required) { return }
    if (-not (Test-Elevated)) {
        $pwsh = if (Get-Command pwsh.exe -ErrorAction SilentlyContinue) { 'pwsh.exe' } else { 'powershell.exe' }
        Start-Process $pwsh "-ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs
        return $true   # caller should `return` immediately
    }
    return $false
}

# ── User prompts ───────────────────────────────────────────────────────────────
function Confirm-Action {
    <#
    .SYNOPSIS Prompts the user for y/n confirmation.
    .PARAMETER Force  Skip the prompt and assume yes (for automation).
    #>
    param (
        [Parameter(Mandatory)][string]$Prompt,
        [switch]$Force
    )
    if ($Force) { return $true }
    $choice = Read-Host "$Prompt (y/n)"
    return ($choice -eq 'y')
}

function Confirm-ExplorerRestart {
    <#
    .SYNOPSIS Optionally restarts Windows Explorer to apply shell changes.
    #>
    param ([switch]$Force)
    if (-not (Confirm-Action -Prompt 'Restart Windows Explorer to apply changes immediately?' -Force:$Force)) { return }
    try {
        Stop-Process -Name explorer -Force -ErrorAction Stop
        Write-Host '🛡️ Explorer restarted.' -ForegroundColor Green
        Write-TurboLog 'Explorer restarted'
    } catch {
        Write-Host "⚠️ Explorer restart failed: $_" -ForegroundColor Yellow
        Write-TurboLog "Explorer restart warning: $_"
    }
}

# ── Tweak runner (used by menu) ─────────────────────────────────────────
function Invoke-TweakModule {
    <#
    .SYNOPSIS Dot-sources a tweak script with error handling.
    #>
    param (
        [Parameter(Mandatory)][string]$FileName,
        [Parameter(Mandatory)][string]$Label,
        [switch]$Force
    )
    $fullPath = Join-Path $script:TT_ScriptDir $FileName
    if (-not (Test-Path $fullPath)) {
        Write-Host "❌ Module '$FileName' not found." -ForegroundColor Red
        Read-Host 'Press Enter to return to menu...'
        return
    }

    # ── Corporate network safety guard ─────────────────────────────────────
    $guardPath = Join-Path $script:TT_ScriptDir 'Lib-CorpGuard.ps1'
    if (Test-Path $guardPath) {
        . $guardPath
        if (Assert-NotCorporate -Force:$Force) {
            Read-Host 'Press Enter to return to menu...'
            return
        }
    }

    Write-Host "`n🧩 Running $Label..." -ForegroundColor Cyan
    try {
        . $fullPath
    } catch {
        Write-Host "❌ Error running module: $_" -ForegroundColor Red
        Write-TurboLog "Error running $Label : $_"
    }
    Read-Host 'Press Enter to return to menu...'
}
