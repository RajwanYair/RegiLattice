#Requires -Version 5.1
# Remove-DisableTelemetry.ps1
# Restores Windows telemetry to default (Full) level.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Restore Windows telemetry to default?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-DisableTelemetry'

$keys = @(
    'HKLM:\SOFTWARE\Policies\Microsoft\Windows\DataCollection'
    'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection'
)

try {
    Backup-Registry -Keys $keys -Label 'Telemetry_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Restore to default (3 = Full)
    Set-ItemProperty -Path $keys[0] -Name 'AllowTelemetry' -Value 3 -Type DWord -ErrorAction SilentlyContinue
    Set-ItemProperty -Path $keys[1] -Name 'AllowTelemetry' -Value 3 -Type DWord -ErrorAction SilentlyContinue
    Remove-ItemProperty -Path $keys[0] -Name 'DoNotShowFeedbackNotifications' -ErrorAction SilentlyContinue

    Write-Host '📊 Telemetry restored to default.' -ForegroundColor Green
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-DisableTelemetry'
