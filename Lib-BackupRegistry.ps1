#Requires -Version 5.1
# Lib-BackupRegistry.ps1
# Shared function for registry backups with OneDrive fallback to Documents.

Set-StrictMode -Version Latest

function Backup-Registry {
    <#
    .SYNOPSIS  Exports a set of registry keys to timestamped .reg files.
    .DESCRIPTION
        Saves backups under OneDrive\RegistryBackups when available,
        otherwise falls back to ~\Documents\RegistryBackups.
    .PARAMETER Keys   One or more registry key paths (reg.exe format or PS format).
    .PARAMETER Label  A short label used in the backup folder name.
    .OUTPUTS   [string] The path to the backup folder.
    #>
    param (
        [Parameter(Mandatory)]
        [string[]]$Keys,

        [Parameter(Mandatory)]
        [string]$Label
    )

    $timestamp = Get-Date -Format 'yyyy-MM-dd_HH-mm-ss'

    # Pick backup root — prefer OneDrive if it exists
    if ($env:OneDrive -and (Test-Path $env:OneDrive)) {
        $backupRoot = Join-Path $env:OneDrive 'RegistryBackups'
    } else {
        $backupRoot = Join-Path $env:USERPROFILE 'Documents\RegistryBackups'
    }

    $backupPath = Join-Path $backupRoot "${Label}_${timestamp}"

    try {
        New-Item -ItemType Directory -Path $backupPath -Force | Out-Null

        foreach ($key in $Keys) {
            # Normalize PS-style paths (HKLM:\...) to reg.exe paths (HKLM\...)
            $regKey = $key -replace ':\\', '\'
            $safeName = ($regKey -replace '[\\:*{}/<>|"]', '_').TrimStart('_')
            $regFile = Join-Path $backupPath "$safeName.reg"

            $proc = Start-Process reg -ArgumentList "export `"$regKey`" `"$regFile`" /y" `
                -NoNewWindow -Wait -PassThru -RedirectStandardError "$env:TEMP\tt_reg_err.txt" 2>$null
            if ($proc.ExitCode -ne 0) {
                $errMsg = Get-Content "$env:TEMP\tt_reg_err.txt" -ErrorAction SilentlyContinue
                Write-Verbose "Backup skipped for $regKey ($errMsg)"
            }
        }

        Write-Host "✔ Backup saved at: $backupPath" -ForegroundColor Cyan
    } catch {
        Write-Host "⚠️ Backup failed: $_" -ForegroundColor Red
    }

    return $backupPath
}