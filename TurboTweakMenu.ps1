#Requires -Version 5.1
# TurboTweakMenu.ps1
# Master menu to launch tweak modules.

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot\Lib-TurboTweak.ps1"

# ── Menu definition ────────────────────────────────────────────────────────────
$menuItems = [ordered]@{
    '1'  = @{ File = 'Add-TakeOwnership.ps1';    Label = 'Add Take Ownership' }
    '2'  = @{ File = 'Remove-TakeOwnership.ps1'; Label = 'Remove Take Ownership' }
    '3'  = @{ File = 'Add-RecentFolders.ps1';    Label = 'Add Recent Folders' }
    '4'  = @{ File = 'Remove-RecentFolders.ps1'; Label = 'Remove Recent Folders' }
    '5'  = @{ File = 'Add-VerboseBoot.ps1';      Label = 'Enable Verbose Boot Messages' }
    '6'  = @{ File = 'Remove-VerboseBoot.ps1';   Label = 'Disable Verbose Boot Messages' }
    '7'  = @{ File = 'Add-Performance.ps1';      Label = 'Apply Performance Tweaks' }
    '8'  = @{ File = 'Remove-Performance.ps1';   Label = 'Remove Performance Tweaks' }
    '9'  = @{ File = 'Add-RegistryBackup.ps1';   Label = 'Enable Registry Backup' }
    '10' = @{ File = 'Remove-RegistryBackup.ps1'; Label = 'Disable Registry Backup' }
}

$applyAllScripts = @(
    'Add-TakeOwnership.ps1'
    'Add-RecentFolders.ps1'
    'Add-VerboseBoot.ps1'
    'Add-Performance.ps1'
    'Add-RegistryBackup.ps1'
)

$removeAllScripts = @(
    'Remove-TakeOwnership.ps1'
    'Remove-RecentFolders.ps1'
    'Remove-VerboseBoot.ps1'
    'Remove-Performance.ps1'
    'Remove-RegistryBackup.ps1'
)

# ── Display ────────────────────────────────────────────────────────────────────
function Show-Banner {
    Clear-Host
    $elevated = if (Test-Elevated) { '(Admin)' } else { '(Standard)' }
    Write-Host ''
    Write-Host '  ╔══════════════════════════════════════════════╗' -ForegroundColor DarkCyan
    Write-Host '  ║         ⚡ TurboTweak Launcher ⚡            ║' -ForegroundColor Cyan
    Write-Host '  ╚══════════════════════════════════════════════╝' -ForegroundColor DarkCyan
    Write-Host "   Session: $elevated | Log: TurboTweak.log" -ForegroundColor DarkGray
    Write-Host ''

    foreach ($key in $menuItems.Keys) {
        $label = $menuItems[$key].Label
        $pad   = if ($key.Length -eq 1) { ' ' } else { '' }
        Write-Host "   [$pad$key] $label"
    }
    Write-Host ''
    Write-Host '   [11] Apply All Tweaks'  -ForegroundColor Green
    Write-Host '   [12] Remove All Tweaks' -ForegroundColor Yellow
    Write-Host '   [13] Create Restore Point' -ForegroundColor Magenta
    Write-Host '    [0] Exit' -ForegroundColor DarkGray
    Write-Host ''
    Write-Host '  ──────────────────────────────────────────────' -ForegroundColor DarkCyan
}

# ── Main loop (non-recursive) ─────────────────────────────────────────────────
while ($true) {
    Show-Banner
    $choice = Read-Host '  Enter your choice'

    switch ($choice) {
        { $menuItems.ContainsKey($_) } {
            $item = $menuItems[$_]
            Invoke-TweakModule -FileName $item.File -Label $item.Label
        }
        '11' {
            foreach ($script in $applyAllScripts) {
                Invoke-TweakModule -FileName $script -Label "Apply: $script"
            }
        }
        '12' {
            foreach ($script in $removeAllScripts) {
                Invoke-TweakModule -FileName $script -Label "Remove: $script"
            }
        }
        '13' {
            Invoke-TweakModule -FileName 'System_Restore_Point.ps1' -Label 'Create Restore Point'
        }
        '0' {
            Write-Host "`n  👋 TurboTweak session ended." -ForegroundColor Gray
            break
        }
        default {
            Write-Host '  ❌ Invalid selection. Please try again.' -ForegroundColor Red
            Read-Host '  Press Enter to retry...'
        }
    }

    # Break in the switch only breaks the switch; we need a labelled break for the while.
    if ($choice -eq '0') { break }
}