#Requires -Version 5.1
# Add-DisableGameDVR.ps1
# Disables Game DVR / Game Bar to reduce gaming overhead and background recording.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Disable Game DVR / Game Bar? (Improves game performance)' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-DisableGameDVR'

$keys = @(
    'HKCU:\System\GameConfigStore'
    'HKLM:\SOFTWARE\Policies\Microsoft\Windows\GameDVR'
    'HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR'
)

try {
    Backup-Registry -Keys $keys -Label 'GameDVR'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Disable Game Bar
    Set-ItemProperty -Path $keys[0] -Name 'GameDVR_Enabled' -Value 0 -Type DWord

    # Disable Game DVR via policy
    New-Item -Path $keys[1] -Force | Out-Null
    Set-ItemProperty -Path $keys[1] -Name 'AllowGameDVR' -Value 0 -Type DWord

    # Disable background recording
    New-Item -Path $keys[2] -Force | Out-Null
    Set-ItemProperty -Path $keys[2] -Name 'AppCaptureEnabled' -Value 0 -Type DWord

    Write-Host '🎮 Game DVR / Game Bar disabled.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-DisableGameDVR'
