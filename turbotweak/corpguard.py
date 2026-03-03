"""Corporate network detection and safety guard.

Prevents accidental registry modifications on domain-joined, Azure-AD-joined,
VPN-connected, or otherwise managed machines.
"""

from __future__ import annotations

import os
import re
import subprocess
from typing import Optional

from .registry import SESSION, is_windows


# ── Detection helpers ────────────────────────────────────────────────────────

_VPN_KEYWORDS: tuple[str, ...] = (
    "cisco", "anyconnect", "pulse", "globalprotect", "juniper",
    "forticlient", "zscaler", "vpn", "wireguard", "openvpn",
    "f5", "palo alto", "check point", "sonicwall",
)


class CorporateNetworkError(RuntimeError):
    """Raised when a corporate network is detected and tweaks are blocked."""


def _is_domain_joined() -> bool:
    """Check Win32_ComputerSystem.PartOfDomain via WMI."""
    if not is_windows():
        return False
    try:
        import ctypes
        import ctypes.wintypes

        advapi32 = ctypes.windll.advapi32  # type: ignore[attr-defined]
        buf = ctypes.create_unicode_buffer(256)
        size = ctypes.wintypes.DWORD(256)
        # GetComputerNameExW with NameDnsDomain (2)
        if advapi32.GetComputerNameExW(2, buf, ctypes.byref(size)):
            domain = buf.value.strip()
            if domain:
                SESSION.log(f"Corp-guard: domain detected: {domain}")
                return True
    except Exception as exc:
        SESSION.log(f"Corp-guard: domain check error: {exc}")

    # Fallback: WMI via PowerShell
    try:
        result = subprocess.run(
            ["powershell", "-NoProfile", "-Command",
             "(Get-CimInstance Win32_ComputerSystem).PartOfDomain"],
            capture_output=True, text=True, timeout=10,
        )
        if result.stdout.strip().lower() == "true":
            SESSION.log("Corp-guard: WMI confirms domain-joined")
            return True
    except Exception as exc:
        SESSION.log(f"Corp-guard: WMI fallback error: {exc}")

    return False


def _is_azure_ad_joined() -> bool:
    """Check dsregcmd /status for Azure AD or hybrid join."""
    if not is_windows():
        return False
    try:
        result = subprocess.run(
            ["dsregcmd", "/status"],
            capture_output=True, text=True, timeout=10,
        )
        output = result.stdout
        if re.search(r"AzureAdJoined\s*:\s*YES", output, re.IGNORECASE):
            SESSION.log("Corp-guard: Azure AD joined")
            return True
        if re.search(r"DomainJoined\s*:\s*YES", output, re.IGNORECASE):
            SESSION.log("Corp-guard: dsregcmd domain joined")
            return True
    except Exception as exc:
        SESSION.log(f"Corp-guard: dsregcmd error: {exc}")
    return False


def _has_vpn_adapter() -> bool:
    """Detect active VPN network adapters via PowerShell."""
    if not is_windows():
        return False
    try:
        result = subprocess.run(
            ["powershell", "-NoProfile", "-Command",
             "Get-NetAdapter | Where-Object Status -eq Up | "
             "Select-Object -ExpandProperty InterfaceDescription"],
            capture_output=True, text=True, timeout=10,
        )
        for line in result.stdout.splitlines():
            desc = line.strip().lower()
            for keyword in _VPN_KEYWORDS:
                if keyword in desc:
                    SESSION.log(f"Corp-guard: VPN adapter: {line.strip()}")
                    return True
    except Exception as exc:
        SESSION.log(f"Corp-guard: VPN check error: {exc}")
    return False


def _has_management_agent() -> bool:
    """Detect SCCM/Intune management agent."""
    if not is_windows():
        return False
    # SCCM client
    try:
        result = subprocess.run(
            ["powershell", "-NoProfile", "-Command",
             "Get-CimInstance -Namespace root\\ccm -ClassName SMS_Client "
             "-ErrorAction Stop | Select-Object -ExpandProperty ClientVersion"],
            capture_output=True, text=True, timeout=10,
        )
        if result.returncode == 0 and result.stdout.strip():
            SESSION.log("Corp-guard: SCCM client detected")
            return True
    except Exception:
        pass

    # Intune enrollment
    try:
        import winreg

        with winreg.OpenKey(
            winreg.HKEY_LOCAL_MACHINE,
            r"SOFTWARE\Microsoft\Enrollments",
            0,
            winreg.KEY_READ,
        ) as hkey:
            idx = 0
            while True:
                try:
                    subkey_name = winreg.EnumKey(hkey, idx)
                    with winreg.OpenKey(hkey, subkey_name) as sub:
                        provider, _ = winreg.QueryValueEx(sub, "ProviderID")
                        if provider and "MS DM Server" in str(provider):
                            SESSION.log("Corp-guard: Intune enrollment detected")
                            return True
                    idx += 1
                except OSError:
                    break
    except Exception:
        pass

    return False


# ── Public API ───────────────────────────────────────────────────────────────

def is_corporate_network() -> bool:
    """Return True when the machine appears to be on a corporate network.

    Checks:
    1. Active Directory domain membership
    2. Azure AD / Hybrid join
    3. Active VPN adapters
    4. SCCM / Intune management agent
    """
    checks = [
        ("domain-join", _is_domain_joined),
        ("azure-ad", _is_azure_ad_joined),
        ("vpn-adapter", _has_vpn_adapter),
        ("mgmt-agent", _has_management_agent),
    ]
    for name, fn in checks:
        try:
            if fn():
                return True
        except Exception as exc:
            SESSION.log(f"Corp-guard: {name} check failed: {exc}")

    SESSION.log("Corp-guard: no corporate indicators found")
    return False


def assert_not_corporate(*, force: bool = False) -> None:
    """Raise ``CorporateNetworkError`` when on a corp network.

    Parameters
    ----------
    force:
        If True, skip the guard entirely (for IT pros who know the risk).
    """
    if force:
        SESSION.log("Corp-guard: bypassed with force=True")
        return

    if is_corporate_network():
        raise CorporateNetworkError(
            "Registry modifications are blocked: corporate network detected.\n"
            "Disconnect from VPN / corporate network first, "
            "or use --force / -Force to bypass (at your own risk)."
        )


def corp_guard_status() -> Optional[str]:
    """Return a human-readable status string, or None if not corporate."""
    if not is_windows():
        return None

    reasons: list[str] = []
    if _is_domain_joined():
        reasons.append("AD domain-joined")
    if _is_azure_ad_joined():
        reasons.append("Azure AD joined")
    if _has_vpn_adapter():
        reasons.append("VPN connected")
    if _has_management_agent():
        reasons.append("Managed (SCCM/Intune)")

    return ", ".join(reasons) if reasons else None
