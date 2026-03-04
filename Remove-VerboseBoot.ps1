#Requires -Version 5.1
# Remove-VerboseBoot.ps1
# Disables verbose boot messages.

param ([switch]$Force)

. "$PSScriptRoot\Lib-RegiLattice.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Disable verbose boot messages?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-VerboseBoot'

$key = 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System'

try {
    Backup-Registry -Keys @($key) -Label 'VerboseBoot_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    Set-ItemProperty -Path $key -Name 'verbosestatus' -Value 0 -Type DWord
    Write-Host '🚫 Verbose boot disabled.' -ForegroundColor Yellow
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-VerboseBoot'