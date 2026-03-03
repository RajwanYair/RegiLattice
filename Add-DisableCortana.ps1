#Requires -Version 5.1
# Add-DisableCortana.ps1
# Disables Cortana to reduce background resource usage.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Disable Cortana? (Reboot recommended)' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-DisableCortana'

$key = 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\Windows Search'

try {
    Backup-Registry -Keys @($key) -Label 'Cortana'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    New-Item -Path $key -Force | Out-Null
    Set-ItemProperty -Path $key -Name 'AllowCortana'        -Value 0 -Type DWord
    Set-ItemProperty -Path $key -Name 'AllowSearchToUseLocation' -Value 0 -Type DWord
    Set-ItemProperty -Path $key -Name 'DisableWebSearch'    -Value 1 -Type DWord
    Set-ItemProperty -Path $key -Name 'ConnectedSearchUseWeb' -Value 0 -Type DWord

    Write-Host '🤖 Cortana disabled. Reboot for full effect.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-DisableCortana'
