#Requires -Version 5.1
# Remove-Performance.ps1
# Removes performance tweaks and restores defaults.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Remove performance tweaks and restore defaults?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-Performance'

$keys = @(
    'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize'
    'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile'
)

try {
    Backup-Registry -Keys $keys -Label 'Performance_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Remove Serialize properties
    Remove-ItemProperty -Path $keys[0] -Name 'StartupDelayInMSec' -ErrorAction SilentlyContinue
    Remove-ItemProperty -Path $keys[0] -Name 'WaitforIdleState'   -ErrorAction SilentlyContinue

    # If Serialize key is now empty, remove it entirely
    if (Test-Path $keys[0]) {
        $props = Get-ItemProperty -Path $keys[0] -ErrorAction SilentlyContinue |
                 Select-Object -Property * -ExcludeProperty PSPath,PSParentPath,PSChildName,PSDrive,PSProvider
        if (($props.PSObject.Properties | Measure-Object).Count -eq 0) {
            Remove-Item -Path $keys[0] -Recurse -ErrorAction SilentlyContinue
        }
    }

    # Restore defaults on multimedia profile
    Set-ItemProperty -Path $keys[1] -Name 'SystemResponsiveness'   -Value 20 -Type DWord
    Set-ItemProperty -Path $keys[1] -Name 'NetworkThrottlingIndex' -Value 10 -Type DWord

    Write-Host '🚫 Tweaks removed, defaults restored.' -ForegroundColor Yellow
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-Performance'
