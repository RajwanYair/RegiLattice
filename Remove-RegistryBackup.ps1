#Requires -Version 5.1
# Remove-RegistryBackup.ps1
# Disables the built-in Windows registry backup engine.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Disable registry backup service?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-RegistryBackup'

$key = 'HKLM:\System\CurrentControlSet\Control\Session Manager\Configuration Manager'

try {
    Backup-Registry -Keys @($key) -Label 'RegistryBackup_Remove'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    Write-TurboLog "Backup error: $_"
    return
}

try {
    Set-ItemProperty -Path $key -Name 'EnablePeriodicBackup' -Value 0 -Type DWord
    Write-Host '🚫 Registry backup disabled.' -ForegroundColor Yellow
    Write-TurboLog 'Disabled successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-RegistryBackup'