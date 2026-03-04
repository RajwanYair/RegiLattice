#Requires -Version 5.1
# Add-RecentFolders.ps1
# Restores "Recent Places" folder in Explorer/File Dialogs.

param ([switch]$Force)

. "$PSScriptRoot\Lib-RegiLattice.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

# HKCU keys — elevation optional but log a note
if (-not (Test-Elevated)) {
    Write-Host 'ℹ️ Running without elevation (HKCU keys). If issues, run as admin.' -ForegroundColor Yellow
}

if (-not (Confirm-Action -Prompt "Restore 'Recent Places' in Explorer?" -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-RecentFolders'

$clsid = '{22877a6d-37a1-461a-91b0-dbda5aaebc99}'
$keys = @(
    "HKCU:\SOFTWARE\Classes\CLSID\$clsid"
    "HKCU:\SOFTWARE\Classes\CLSID\$clsid\ShellFolder"
    "HKCU:\SOFTWARE\Classes\Wow6432Node\CLSID\$clsid"
    "HKCU:\SOFTWARE\Classes\Wow6432Node\CLSID\$clsid\ShellFolder"
)

try {
    Backup-Registry -Keys $keys -Label 'RecentFolders'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_ (continuing — keys may not exist yet)." -ForegroundColor Yellow
    Write-TurboLog "Backup warning: $_"
}

try {
    foreach ($i in @(0, 2)) {
        if (-not (Test-Path $keys[$i]))     { New-Item -Path $keys[$i]     -Force | Out-Null }
        if (-not (Test-Path $keys[$i + 1])) { New-Item -Path $keys[$i + 1] -Force | Out-Null }
        Set-ItemProperty -Path $keys[$i]     -Name '(default)'  -Value 'Recent Places'
        Set-ItemProperty -Path $keys[$i + 1] -Name 'Attributes' -Value 0x30040000 -Type DWord
    }

    Write-Host "✅ 'Recent Places' restored." -ForegroundColor Green
    Write-TurboLog 'Applied successfully'

    if (Confirm-Action -Prompt 'Launch Recent Places to pin?' -Force:$Force) {
        Start-Process explorer.exe "shell:::$clsid"
        Write-Host '📌 Launched. Right-click and pin to Quick Access.' -ForegroundColor Magenta
    }
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    Write-TurboLog "Error: $_"
}

Write-TurboLog 'Completed Add-RecentFolders'