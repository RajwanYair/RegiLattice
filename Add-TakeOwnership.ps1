#Requires -Version 5.1
# Add-TakeOwnership.ps1
# Adds "Take Ownership" right-click context menu entries for files, directories, and drives.
# Uses custom \shell\TakeOwnership to avoid conflicting with built-in RunAs verb.

param ([switch]$Force)

. "$PSScriptRoot\Lib-TurboTweak.ps1"
. "$PSScriptRoot\Lib-BackupRegistry.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt "Add 'Take Ownership' context menu? This modifies registry" -Force:$Force)) {
    Write-Host '❌ Operation cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog 'Starting Add-TakeOwnership'

$keys = @(
    'HKEY_CLASSES_ROOT\*\shell\TakeOwnership'
    'HKEY_CLASSES_ROOT\*\shell\TakeOwnership\command'
    'HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership'
    'HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership\command'
    'HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership'
    'HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership\command'
)

try {
    Backup-Registry -Keys $keys -Label 'TakeOwnership'
    Write-TurboLog 'Backup completed'
} catch {
    Write-Host "⚠️ Backup failed: $_ (continuing)." -ForegroundColor Yellow
    Write-TurboLog "Backup warning: $_"
}

# Commands executed on context-menu click
$cmd_file = 'cmd.exe /k takeown /f "%1" && icacls "%1" /grant *S-1-3-4:F /t /c /l && pause'
$cmd_dir = 'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1" /grant *S-1-3-4:F /t /c /q && pause'
$cmd_drive = 'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1" /grant *S-1-3-4:F /t /c && pause'

function Add-ContextEntry {
    param ([string]$Base, [string]$Command, [string]$TargetType)
    try {
        reg add $Base /f                                       | Out-Null
        reg add $Base /ve /d 'Take Ownership' /f               | Out-Null
        reg add $Base /v 'NoWorkingDirectory' /d '' /f          | Out-Null
        reg add $Base /v 'Extended' /d '' /f                    | Out-Null    # Shift+Right-click only
        reg add "$Base\command" /f                              | Out-Null
        reg add "$Base\command" /ve /d $Command /f              | Out-Null
        Write-Host "✅ Added for $TargetType." -ForegroundColor Green
        Write-TurboLog "Added for $TargetType"
    } catch {
        Write-Host "⚠️ Warning adding for ${TargetType}: $_" -ForegroundColor Yellow
        Write-TurboLog "Warning for ${TargetType}: $_"
    }
}

Add-ContextEntry -Base 'HKEY_CLASSES_ROOT\*\shell\TakeOwnership'         -Command $cmd_file  -TargetType 'files'
Add-ContextEntry -Base 'HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership' -Command $cmd_dir   -TargetType 'directories'
Add-ContextEntry -Base 'HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership'     -Command $cmd_drive -TargetType 'drives'

Write-Host "✅ 'Take Ownership' context menu added. Log at $($script:TT_LogPath)" -ForegroundColor Green
Write-TurboLog 'Completed Add-TakeOwnership'

Confirm-ExplorerRestart -Force:$Force
