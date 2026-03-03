#Requires -Version 5.1
# Remove-LongPaths.ps1
# Disables Win32 long paths (restores 260-char path limit default).

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Disable long paths (restore 260-char limit)?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-LongPaths'

$key = 'HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem'

try {
    Backup-Registry -Keys @($key) -Label 'LongPaths_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    Set-ItemProperty -Path $key -Name 'LongPathsEnabled' -Value 0 -Type DWord

    Write-Host '📂 Win32 long paths disabled (260-char limit restored).' -ForegroundColor Green
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-LongPaths'
