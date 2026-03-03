#Requires -Version 5.1
# Remove-DisableGameDVR.ps1
# Re-enables Game DVR / Game Bar (Windows default).

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Re-enable Game DVR / Game Bar?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-DisableGameDVR'

$keys = @(
    'HKCU:\System\GameConfigStore'
    'HKLM:\SOFTWARE\Policies\Microsoft\Windows\GameDVR'
    'HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR'
)

try {
    Backup-Registry -Keys $keys -Label 'GameDVR_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    Set-ItemProperty -Path $keys[0] -Name 'GameDVR_Enabled' -Value 1 -Type DWord -ErrorAction SilentlyContinue
    Remove-ItemProperty -Path $keys[1] -Name 'AllowGameDVR' -ErrorAction SilentlyContinue
    Set-ItemProperty -Path $keys[2] -Name 'AppCaptureEnabled' -Value 1 -Type DWord -ErrorAction SilentlyContinue

    Write-Host '🎮 Game DVR / Game Bar re-enabled.' -ForegroundColor Green
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-DisableGameDVR'
