#Requires -Version 5.1
# Remove-RecentFolders.ps1
# Cleans up "Recent Places" tweak from registry.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (-not (Test-Elevated)) {
    Write-Host 'ℹ️ Running without elevation (HKCU keys).' -ForegroundColor Yellow
}

if (-not (Confirm-Action -Prompt "Remove 'Recent Places' from Explorer?" -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-RecentFolders'

$clsid = '{22877a6d-37a1-461a-91b0-dbda5aaebc99}'
$keys = @(
    "HKCU:\SOFTWARE\Classes\CLSID\$clsid"
    "HKCU:\SOFTWARE\Classes\Wow6432Node\CLSID\$clsid"
)

try {
    Backup-Registry -Keys $keys -Label 'RecentFolders_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    foreach ($key in $keys) {
        if (Test-Path $key) {
            Remove-Item -Path $key -Recurse -ErrorAction Stop
        }
    }
    Write-Host "🚫 'Recent Places' removed." -ForegroundColor Yellow
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-RecentFolders'