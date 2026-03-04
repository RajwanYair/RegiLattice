#Requires -Version 5.1
# Remove-DisableMouseAccel.ps1
# Re-enables mouse acceleration (enhanced pointer precision) — Windows default.

param ([switch]$Force)

. "$PSScriptRoot\Lib-RegiLattice.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (-not (Confirm-Action -Prompt 'Re-enable mouse acceleration (Windows default)?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-DisableMouseAccel'

$key = 'HKCU:\Control Panel\Mouse'

try {
    Backup-Registry -Keys @($key) -Label 'MouseAccel_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Windows defaults: MouseSpeed=1, Threshold1=6, Threshold2=10
    Set-ItemProperty -Path $key -Name 'MouseSpeed'      -Value '1'
    Set-ItemProperty -Path $key -Name 'MouseThreshold1'  -Value '6'
    Set-ItemProperty -Path $key -Name 'MouseThreshold2'  -Value '10'

    Write-Host '🖱️ Mouse acceleration restored to default.' -ForegroundColor Green
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-DisableMouseAccel'
