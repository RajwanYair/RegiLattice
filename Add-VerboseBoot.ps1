#Requires -Version 5.1
# Add-VerboseBoot.ps1
# Enables detailed boot/shutdown status messages.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Enable verbose boot messages?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-VerboseBoot'

$key = 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System'

try {
    Backup-Registry -Keys @($key) -Label 'VerboseBoot'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    New-Item -Path $key -Force | Out-Null
    Set-ItemProperty -Path $key -Name 'verbosestatus' -Value 1 -Type DWord
    Write-Host '🧠 Enabled. Detailed info on startup/shutdown.' -ForegroundColor Cyan
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-VerboseBoot'