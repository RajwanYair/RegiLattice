#Requires -Version 5.1
# Add-Performance.ps1
# Applies Windows performance & responsiveness registry tweaks.

param ([switch]$Force)

. "$PSScriptRoot\Lib-RegiLattice.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Apply performance tweaks? Reboot recommended after' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-Performance'

$keys = @(
    'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Serialize'
    'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile'
)

try {
    Backup-Registry -Keys $keys -Label 'Performance'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Eliminate startup delay
    New-Item -Path $keys[0] -Force | Out-Null
    Set-ItemProperty -Path $keys[0] -Name 'StartupDelayInMSec' -Value 0 -Type DWord
    Set-ItemProperty -Path $keys[0] -Name 'WaitforIdleState'   -Value 0 -Type DWord

    # Multimedia: lower system responsiveness (more CPU to foreground)
    New-Item -Path $keys[1] -Force | Out-Null
    Set-ItemProperty -Path $keys[1] -Name 'SystemResponsiveness'   -Value 10          -Type DWord
    Set-ItemProperty -Path $keys[1] -Name 'NetworkThrottlingIndex' -Value 0xffffffff  -Type DWord

    Write-Host '⚡ Tweaks applied. Reboot for full effect.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-Performance'