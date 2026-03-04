#Requires -Version 5.1
# Add-RegistryBackup.ps1
# Enables the built-in Windows registry backup engine.

param ([switch]$Force)

. "$PSScriptRoot\Lib-RegiLattice.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Enable Windows periodic registry backup?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-RegistryBackup'

$key = 'HKLM:\System\CurrentControlSet\Control\Session Manager\Configuration Manager'

try {
    Backup-Registry -Keys @($key) -Label 'RegistryBackup'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    New-Item -Path $key -Force | Out-Null
    Set-ItemProperty -Path $key -Name 'EnablePeriodicBackup' -Value 1 -Type DWord
    Write-Host '🛡️ Registry backup service enabled. Windows will periodically back up the registry.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-RegistryBackup'
