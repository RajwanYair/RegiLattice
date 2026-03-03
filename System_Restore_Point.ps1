#Requires -Version 5.1
# System_Restore_Point.ps1
# Creates a system restore point before applying tweaks.

param (
    [string]$Description = 'TurboTweak Pre-Tweaks',
    [switch]$Force
)

. "$PSScriptRoot\Lib-TurboTweak.ps1"

if (Assert-Elevated -Required) { return }

if (-not (Confirm-Action -Prompt "Create system restore point '$Description'?" -Force:$Force)) {
    Write-Host '❌ Cancelled.' -ForegroundColor Yellow; return
}

Write-TurboLog "Creating restore point: $Description"

try {
    # Enable System Restore on C: if not already enabled
    Enable-ComputerRestore -Drive 'C:\' -ErrorAction SilentlyContinue

    Checkpoint-Computer -Description $Description -RestorePointType 'MODIFY_SETTINGS' -ErrorAction Stop
    Write-Host '✅ System restore point created successfully.' -ForegroundColor Green
    Write-TurboLog "Restore point created: $Description"
} catch {
    Write-Host "❌ Failed to create restore point: $_" -ForegroundColor Red
    Write-Host '   Note: Windows limits restore points to one per 24 hours.' -ForegroundColor Yellow
    Write-TurboLog "Restore point error: $_"
}