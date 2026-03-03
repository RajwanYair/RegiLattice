#Requires -Version 5.1
# Remove-SvcHostSplit.ps1
# Restores SvcHost split threshold to Windows default (380000 KB ≈ 380 MB).

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Restore SvcHost split threshold to default?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-SvcHostSplit'

$key = 'HKLM:\SYSTEM\CurrentControlSet\Control'

try {
    Backup-Registry -Keys @($key) -Label 'SvcHostSplit_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Windows default: ~380000 KB
    Set-ItemProperty -Path $key -Name 'SvcHostSplitThresholdInKB' -Value 380000 -Type DWord

    Write-Host '⚙️ SvcHost split threshold restored to default (380 MB).' -ForegroundColor Green
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-SvcHostSplit'
