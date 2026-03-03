#Requires -Version 5.1
# Remove-DisableLastAccess.ps1
# Re-enables NTFS last-access timestamp updates (Windows default).

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt 'Re-enable NTFS last-access timestamps?' -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Remove-DisableLastAccess'

try {
    # 2 = System managed (default on Win10+), 0 = User managed (always update)
    $result = & fsutil behavior set disablelastaccess 2 2>&1
    Write-TurboLog "fsutil output: $result"

    Write-Host '💾 NTFS last-access timestamps restored to system-managed default.' -ForegroundColor Green
    Write-TurboLog 'Removed successfully'
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Remove-DisableLastAccess'
