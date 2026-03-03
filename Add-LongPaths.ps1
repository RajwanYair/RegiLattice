#Requires -Version 5.1
# Add-LongPaths.ps1
# Enables Win32 long paths to remove the 260-character path limit.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Enable long paths (remove 260-char limit)?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-LongPaths'

$key = 'HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem'

try {
    Backup-Registry -Keys @($key) -Label 'LongPaths'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    Set-ItemProperty -Path $key -Name 'LongPathsEnabled' -Value 1 -Type DWord

    Write-Host '📂 Win32 long paths enabled.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-LongPaths'
