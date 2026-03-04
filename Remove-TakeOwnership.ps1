#Requires -Version 5.1
# Remove-TakeOwnership.ps1
# Cleans up "Take Ownership" context menu entries.

param ([switch]$Force)

. "$PSScriptRoot\Lib-RegiLattice.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt "Remove 'Take Ownership' context menu? This modifies registry" -Force:$Force)) {
    Write-Host '❌ Operation cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-TakeOwnership'

$keys = @(
    'HKEY_CLASSES_ROOT\*\shell\TakeOwnership'
    'HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership'
    'HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership'
)

try {
    Backup-Registry -Keys $keys -Label 'TakeOwnership_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_ (continuing)." -ForegroundColor Yellow
    Write-TurboLog "Backup warning: $_"
}

try {
    foreach ($key in $keys) {
        reg delete $key /f 2>$null | Out-Null
    }
    Write-Host "🚫 'Take Ownership' context menu removed." -ForegroundColor Yellow
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "⚠️ Warning removing keys: $_" -ForegroundColor Yellow
    Write-TurboLog "Removal warning: $_"
}

Write-TurboLog 'Completed Remove-TakeOwnership'

Confirm-ExplorerRestart -Force:$Force