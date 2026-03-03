#Requires -Version 5.1
# Add-DisableMouseAccel.ps1
# Disables mouse acceleration (enhanced pointer precision) for pixel-perfect input.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

# This is a per-user setting — no elevation needed
if (-not (Confirm-Action -Prompt 'Disable mouse acceleration for 1:1 input?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-DisableMouseAccel'

$key = 'HKCU:\Control Panel\Mouse'

try {
    Backup-Registry -Keys @($key) -Label 'MouseAccel'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # MouseSpeed 0 = no acceleration, MouseThreshold1/2 = 0 for flat curve
    Set-ItemProperty -Path $key -Name 'MouseSpeed'      -Value '0'
    Set-ItemProperty -Path $key -Name 'MouseThreshold1'  -Value '0'
    Set-ItemProperty -Path $key -Name 'MouseThreshold2'  -Value '0'

    # Also set the smooth mouse acceleration curve to flat (6/11 speed = linear)
    $flat = @(0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
              0,40,0,0, 0,80,0,0, 0,120,0,0, 0,160,0,0,
              0,200,0,0, 0,240,0,0, 0,24,1,0, 0,64,1,0)
    Set-ItemProperty -Path $key -Name 'SmoothMouseXCurve' -Value ([byte[]]$flat) -Type Binary
    Set-ItemProperty -Path $key -Name 'SmoothMouseYCurve' -Value ([byte[]]$flat) -Type Binary

    Write-Host '🎯 Mouse acceleration disabled. Log out/in for full effect.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-DisableMouseAccel'
