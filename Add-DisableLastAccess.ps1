#Requires -Version 5.1
# Add-DisableLastAccess.ps1
# Disables NTFS last-access timestamp updates for improved file-system performance.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Disable NTFS last-access timestamps? (Improves I/O performance)' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-DisableLastAccess'

try {
    # Use fsutil to disable last access time (more reliable than registry)
    $result = & fsutil behavior set disablelastaccess 1 2>&1
    Write-TurboLog "fsutil output: $result"

    Write-Host '💾 NTFS last-access timestamps disabled. Reboot for full effect.' -ForegroundColor Green
    Write-TurboLog 'Applied successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-DisableLastAccess'
