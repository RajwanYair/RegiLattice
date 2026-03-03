#Requires -Version 5.1
# Add-SvcHostSplit.ps1
# Optimizes SvcHost split threshold based on installed RAM to reduce process overhead.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Optimize SvcHost split threshold based on RAM?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-SvcHostSplit'

$key = 'HKLM:\SYSTEM\CurrentControlSet\Control'

try {
    Backup-Registry -Keys @($key) -Label 'SvcHostSplit'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    # Get total physical memory in KB
    $ramKB = [math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory / 1KB)
    Set-ItemProperty -Path $key -Name 'SvcHostSplitThresholdInKB' -Value $ramKB -Type DWord

    $ramGB = [math]::Round($ramKB / 1MB, 1)
    Write-Host "⚙️ SvcHost split threshold set to ${ramGB}GB (${ramKB}KB). Reboot required." -ForegroundColor Green
    Write-TurboLog "Applied: SvcHostSplitThresholdInKB = $ramKB (${ramGB}GB RAM)"
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-SvcHostSplit'
