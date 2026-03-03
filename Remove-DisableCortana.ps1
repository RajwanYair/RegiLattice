#Requires -Version 5.1
# Remove-DisableCortana.ps1
# Re-enables Cortana and web search.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Re-enable Cortana?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-DisableCortana'

$key = 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\Windows Search'

try {
    Backup-Registry -Keys @($key) -Label 'Cortana_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    Remove-ItemProperty -Path $key -Name 'AllowCortana'        -ErrorAction SilentlyContinue
    Remove-ItemProperty -Path $key -Name 'AllowSearchToUseLocation' -ErrorAction SilentlyContinue
    Remove-ItemProperty -Path $key -Name 'DisableWebSearch'    -ErrorAction SilentlyContinue
    Remove-ItemProperty -Path $key -Name 'ConnectedSearchUseWeb' -ErrorAction SilentlyContinue

    Write-Host '🤖 Cortana re-enabled.' -ForegroundColor Green
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-DisableCortana'
