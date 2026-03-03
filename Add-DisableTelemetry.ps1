#Requires -Version 5.1
# Add-DisableTelemetry.ps1
# Reduces Windows diagnostic data collection to the minimum (Security level).

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Reduce Windows telemetry to minimum? (Reboot recommended)' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-DisableTelemetry'

$keys = @(
    'HKLM:\SOFTWARE\Policies\Microsoft\Windows\DataCollection'
    'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection'
)

try {
    Backup-Registry -Keys $keys -Label 'Telemetry'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Set telemetry to Security level (0 = Security/Off, 1 = Basic, 2 = Enhanced, 3 = Full)
    New-Item -Path $keys[0] -Force | Out-Null
    Set-ItemProperty -Path $keys[0] -Name 'AllowTelemetry' -Value 0 -Type DWord

    New-Item -Path $keys[1] -Force | Out-Null
    Set-ItemProperty -Path $keys[1] -Name 'AllowTelemetry' -Value 0 -Type DWord

    # Disable Connected User Experiences and Telemetry service
    Set-ItemProperty -Path $keys[0] -Name 'DoNotShowFeedbackNotifications' -Value 1 -Type DWord

    Write-Host '🔐 Telemetry reduced to minimum. Reboot for full effect.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-DisableTelemetry'
