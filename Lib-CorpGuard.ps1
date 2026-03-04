#Requires -Version 5.1
# Lib-CorpGuard.ps1
# Corporate-network safety guard.
# Detects domain-joined machines, VPN connections, and corporate indicators
# to prevent accidental registry modifications on managed environments.

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot\Lib-RegiLattice.ps1"

function Test-CorporateNetwork {
    <#
    .SYNOPSIS  Returns $true when the machine appears to be on a corporate network.
    .DESCRIPTION
        Checks (in order):
        1. Active Directory domain membership
        2. Azure AD join status
        3. Active VPN network adapters (Cisco, Pulse, GlobalProtect, etc.)
        4. Group-Policy controlled registry indicators
        5. SCCM / Intune management agent
    #>
    [CmdletBinding()]
    param ()

    # ── 1. AD domain join ─────────────────────────────────────────────────
    try {
        $cs = Get-CimInstance -ClassName Win32_ComputerSystem -ErrorAction Stop
        if ($cs.PartOfDomain) {
            Write-TurboLog "Corp-guard: machine is domain-joined ($($cs.Domain))"
            return $true
        }
    } catch {
        Write-TurboLog "Corp-guard: Win32_ComputerSystem query failed: $_"
    }

    # ── 2. Azure AD / Workplace join ──────────────────────────────────────
    try {
        $dsregOutput = dsregcmd /status 2>$null | Out-String
        if ($dsregOutput -match 'AzureAdJoined\s*:\s*YES') {
            Write-TurboLog 'Corp-guard: machine is Azure AD joined'
            return $true
        }
        if ($dsregOutput -match 'DomainJoined\s*:\s*YES') {
            Write-TurboLog 'Corp-guard: dsregcmd reports domain joined'
            return $true
        }
    } catch {
        Write-TurboLog "Corp-guard: dsregcmd check failed: $_"
    }

    # ── 3. VPN adapters ──────────────────────────────────────────────────
    $vpnPatterns = @(
        '*Cisco*', '*AnyConnect*', '*Pulse*', '*GlobalProtect*',
        '*Juniper*', '*FortiClient*', '*Zscaler*', '*VPN*',
        '*WireGuard*', '*OpenVPN*', '*F5*', '*Palo Alto*',
        '*Check Point*', '*SonicWall*'
    )
    try {
        $adapters = Get-NetAdapter -ErrorAction SilentlyContinue |
        Where-Object { $_.Status -eq 'Up' }
        foreach ($adapter in $adapters) {
            foreach ($pattern in $vpnPatterns) {
                if ($adapter.InterfaceDescription -like $pattern -or
                    $adapter.Name -like $pattern) {
                    Write-TurboLog "Corp-guard: active VPN adapter detected: $($adapter.Name)"
                    return $true
                }
            }
        }
    } catch {
        Write-TurboLog "Corp-guard: network adapter check failed: $_"
    }

    # ── 4. Group Policy indicator ─────────────────────────────────────────
    $gpKey = 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies'
    try {
        if (Test-Path "$gpKey\System\EnableLUA") {
            # Check for non-default GP overrides (common in enterprise)
            $gpItems = Get-ChildItem -Path $gpKey -Recurse -ErrorAction SilentlyContinue
            if ($gpItems.Count -gt 20) {
                Write-TurboLog 'Corp-guard: extensive Group Policy overrides detected'
                return $true
            }
        }
    } catch {
        Write-TurboLog "Corp-guard: GP registry check failed: $_"
    }

    # ── 5. SCCM / Intune management ──────────────────────────────────────
    try {
        $sccm = Get-CimInstance -Namespace 'root\ccm' -ClassName SMS_Client -ErrorAction SilentlyContinue
        if ($sccm) {
            Write-TurboLog 'Corp-guard: SCCM client detected'
            return $true
        }
    } catch {
        # WMI namespace does not exist = no SCCM
    }

    try {
        $intuneCert = Get-ChildItem -Path 'Cert:\LocalMachine\My' -ErrorAction SilentlyContinue |
        Where-Object { $_.Issuer -match 'Microsoft Intune' }
        if ($intuneCert) {
            Write-TurboLog 'Corp-guard: Intune certificate detected'
            return $true
        }
    } catch {
        Write-TurboLog "Corp-guard: Intune cert check failed: $_"
    }

    Write-TurboLog 'Corp-guard: no corporate network indicators found'
    return $false
}

function Assert-NotCorporate {
    <#
    .SYNOPSIS  Blocks execution when a corporate network is detected.
    .PARAMETER Force  Override guard (for IT administrators who know what they're doing).
    #>
    param ([switch]$Force)

    if ($Force) {
        Write-TurboLog 'Corp-guard: bypassed with -Force'
        return $false  # not blocked
    }

    if (Test-CorporateNetwork) {
        Write-Host ''
        Write-Host '  ╔══════════════════════════════════════════════════════════╗' -ForegroundColor Red
        Write-Host '  ║  🛑 CORPORATE NETWORK DETECTED                         ║' -ForegroundColor Red
        Write-Host '  ║                                                          ║' -ForegroundColor Red
        Write-Host '  ║  Registry modifications are blocked to prevent           ║' -ForegroundColor Red
        Write-Host '  ║  conflicts with corporate policies and potential         ║' -ForegroundColor Red
        Write-Host '  ║  network access bans.                                    ║' -ForegroundColor Red
        Write-Host '  ║                                                          ║' -ForegroundColor Red
        Write-Host '  ║  Disconnect from VPN / corporate network first,          ║' -ForegroundColor Red
        Write-Host '  ║  or use -Force to bypass (at your own risk).             ║' -ForegroundColor Red
        Write-Host '  ╚══════════════════════════════════════════════════════════╝' -ForegroundColor Red
        Write-Host ''
        Write-TurboLog 'Corp-guard: BLOCKED — corporate network detected'
        return $true   # blocked
    }

    return $false  # not blocked
}
#Requires -Version 5.1
# Lib-CorpGuard.ps1
# Corporate-network safety guard.
# Detects domain-joined machines, VPN connections, and corporate indicators
# to prevent accidental registry modifications on managed environments.

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot\Lib-RegiLattice.ps1"

function Test-CorporateNetwork {
    <#
    .SYNOPSIS  Returns $true when the machine appears to be on a corporate network.
    .DESCRIPTION
        Checks (in order):
        1. Active Directory domain membership
        2. Azure AD join status
        3. Active VPN network adapters (Cisco, Pulse, GlobalProtect, etc.)
        4. Group-Policy controlled registry indicators
        5. SCCM / Intune management agent
    #>
    [CmdletBinding()]
    param ()

    # ── 1. AD domain join ─────────────────────────────────────────────────
    try {
        $cs = Get-CimInstance -ClassName Win32_ComputerSystem -ErrorAction Stop
        if ($cs.PartOfDomain) {
            Write-TurboLog "Corp-guard: machine is domain-joined ($($cs.Domain))"
            return $true
        }
    } catch {
        Write-TurboLog "Corp-guard: Win32_ComputerSystem query failed: $_"
    }

    # ── 2. Azure AD / Workplace join ──────────────────────────────────────
    try {
        $dsregOutput = dsregcmd /status 2>$null | Out-String
        if ($dsregOutput -match 'AzureAdJoined\s*:\s*YES') {
            Write-TurboLog 'Corp-guard: machine is Azure AD joined'
            return $true
        }
        if ($dsregOutput -match 'DomainJoined\s*:\s*YES') {
            Write-TurboLog 'Corp-guard: dsregcmd reports domain joined'
            return $true
        }
    } catch {
        Write-TurboLog "Corp-guard: dsregcmd check failed: $_"
    }

    # ── 3. VPN adapters ──────────────────────────────────────────────────
    $vpnPatterns = @(
        '*Cisco*', '*AnyConnect*', '*Pulse*', '*GlobalProtect*',
        '*Juniper*', '*FortiClient*', '*Zscaler*', '*VPN*',
        '*WireGuard*', '*OpenVPN*', '*F5*', '*Palo Alto*',
        '*Check Point*', '*SonicWall*'
    )
    try {
        $adapters = Get-NetAdapter -ErrorAction SilentlyContinue |
        Where-Object { $_.Status -eq 'Up' }
        foreach ($adapter in $adapters) {
            foreach ($pattern in $vpnPatterns) {
                if ($adapter.InterfaceDescription -like $pattern -or
                    $adapter.Name -like $pattern) {
                    Write-TurboLog "Corp-guard: active VPN adapter detected: $($adapter.Name)"
                    return $true
                }
            }
        }
    } catch {
        Write-TurboLog "Corp-guard: network adapter check failed: $_"
    }

    # ── 4. Group Policy indicator ─────────────────────────────────────────
    $gpKey = 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies'
    try {
        if (Test-Path "$gpKey\System\EnableLUA") {
            # Check for non-default GP overrides (common in enterprise)
            $gpItems = Get-ChildItem -Path $gpKey -Recurse -ErrorAction SilentlyContinue
            if ($gpItems.Count -gt 20) {
                Write-TurboLog 'Corp-guard: extensive Group Policy overrides detected'
                return $true
            }
        }
    } catch {
        Write-TurboLog "Corp-guard: GP registry check failed: $_"
    }

    # ── 5. SCCM / Intune management ──────────────────────────────────────
    try {
        $sccm = Get-CimInstance -Namespace 'root\ccm' -ClassName SMS_Client -ErrorAction SilentlyContinue
        if ($sccm) {
            Write-TurboLog 'Corp-guard: SCCM client detected'
            return $true
        }
    } catch {
        # WMI namespace does not exist = no SCCM
    }

    try {
        $intuneCert = Get-ChildItem -Path 'Cert:\LocalMachine\My' -ErrorAction SilentlyContinue |
        Where-Object { $_.Issuer -match 'Microsoft Intune' }
        if ($intuneCert) {
            Write-TurboLog 'Corp-guard: Intune certificate detected'
            return $true
        }
    } catch {
        Write-TurboLog "Corp-guard: Intune cert check failed: $_"
    }

    Write-TurboLog 'Corp-guard: no corporate network indicators found'
    return $false
}

function Assert-NotCorporate {
    <#
    .SYNOPSIS  Blocks execution when a corporate network is detected.
    .PARAMETER Force  Override guard (for IT administrators who know what they're doing).
    #>
    param ([switch]$Force)

    if ($Force) {
        Write-TurboLog 'Corp-guard: bypassed with -Force'
        return $false  # not blocked
    }

    if (Test-CorporateNetwork) {
        Write-Host ''
        Write-Host '  ╔══════════════════════════════════════════════════════════╗' -ForegroundColor Red
        Write-Host '  ║  🛑 CORPORATE NETWORK DETECTED                         ║' -ForegroundColor Red
        Write-Host '  ║                                                          ║' -ForegroundColor Red
        Write-Host '  ║  Registry modifications are blocked to prevent           ║' -ForegroundColor Red
        Write-Host '  ║  conflicts with corporate policies and potential         ║' -ForegroundColor Red
        Write-Host '  ║  network access bans.                                    ║' -ForegroundColor Red
        Write-Host '  ║                                                          ║' -ForegroundColor Red
        Write-Host '  ║  Disconnect from VPN / corporate network first,          ║' -ForegroundColor Red
        Write-Host '  ║  or use -Force to bypass (at your own risk).             ║' -ForegroundColor Red
        Write-Host '  ╚══════════════════════════════════════════════════════════╝' -ForegroundColor Red
        Write-Host ''
        Write-TurboLog 'Corp-guard: BLOCKED — corporate network detected'
        return $true   # blocked
    }

    return $false  # not blocked
}
