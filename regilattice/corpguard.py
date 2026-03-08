"""Corporate network detection and safety guard.

Prevents accidental registry modifications on domain-joined, Azure-AD-joined,
VPN-connected, Group-Policy-managed, or otherwise managed machines.
"""

from __future__ import annotations

import concurrent.futures
import functools
import re
import subprocess
import threading
from collections.abc import Callable

from .registry import SESSION, is_windows

__all__ = [
    "CorporateNetworkError",
    "assert_not_corporate",
    "corp_guard_reasons",
    "corp_guard_status",
    "is_corporate_network",
    "is_gpo_managed",
    "reset_corp_cache",
]

# ── Detection helpers ────────────────────────────────────────────────────────

_VPN_KEYWORDS: tuple[str, ...] = (
    "cisco",
    "anyconnect",
    "pulse",
    "globalprotect",
    "juniper",
    "forticlient",
    "zscaler",
    "vpn",
    "wireguard",
    "openvpn",
    "f5",
    "palo alto",
    "check point",
    "sonicwall",
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

        advapi32 = ctypes.windll.advapi32
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
            [
                "powershell",
                "-NoProfile",
                "-Command",
                "(Get-CimInstance Win32_ComputerSystem).PartOfDomain",
            ],
            capture_output=True,
            text=True,
            timeout=10,
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
            capture_output=True,
            text=True,
            timeout=10,
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
            [
                "powershell",
                "-NoProfile",
                "-Command",
                "Get-NetAdapter | Where-Object Status -eq Up | Select-Object -ExpandProperty InterfaceDescription",
            ],
            capture_output=True,
            text=True,
            timeout=10,
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
            [
                "powershell",
                "-NoProfile",
                "-Command",
                "Get-CimInstance -Namespace root\\ccm -ClassName SMS_Client -ErrorAction Stop | Select-Object -ExpandProperty ClientVersion",
            ],
            capture_output=True,
            text=True,
            timeout=10,
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


def _has_group_policy() -> bool:
    """Detect active Group Policy enforcement via registry indicators.

    Checks:
    1. ``HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\State``
    2. Non-empty ``HKLM\\SOFTWARE\\Policies`` tree
    3. ``HKLM\\SOFTWARE\\Microsoft\\PolicyManager`` (MDM-delivered policies)
    """
    if not is_windows():
        return False
    try:
        import winreg

        # Check GP State key — present only after gpupdate has applied policies
        try:
            with winreg.OpenKey(
                winreg.HKEY_LOCAL_MACHINE,
                r"SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\State",
                0,
                winreg.KEY_READ,
            ) as hkey:
                # If we can enumerate at least one sub-key, GP is active
                try:
                    winreg.EnumKey(hkey, 0)
                    SESSION.log("Corp-guard: Group Policy State key has entries")
                    return True
                except OSError:
                    pass
        except OSError:
            pass

        # Check HKLM\SOFTWARE\Policies for substantial policy values
        try:
            with winreg.OpenKey(
                winreg.HKEY_LOCAL_MACHINE,
                r"SOFTWARE\Policies",
                0,
                winreg.KEY_READ,
            ) as hkey:
                count = 0
                idx = 0
                while True:
                    try:
                        winreg.EnumKey(hkey, idx)
                        count += 1
                        idx += 1
                        if count >= 3:  # 3+ policy sub-keys = likely GP-managed
                            SESSION.log(f"Corp-guard: {count}+ policy sub-keys under HKLM\\SOFTWARE\\Policies")
                            return True
                    except OSError:
                        break
        except OSError:
            pass

        # Check PolicyManager (MDM / Intune policies)
        try:
            with winreg.OpenKey(
                winreg.HKEY_LOCAL_MACHINE,
                r"SOFTWARE\Microsoft\PolicyManager\current\device",
                0,
                winreg.KEY_READ,
            ):
                SESSION.log("Corp-guard: MDM PolicyManager detected")
                return True
        except OSError:
            pass

    except Exception as exc:
        SESSION.log(f"Corp-guard: GP check error: {exc}")

    return False


def is_gpo_managed(registry_keys: list[str]) -> bool:
    """Return True if any of the given registry keys have a Group Policy overlay.

    For each key, checks whether a corresponding path exists under
    ``HKLM\\SOFTWARE\\Policies\\...`` or ``HKCU\\Software\\Policies\\...``.
    This indicates the value may be overridden or locked by Group Policy.

    Results are cached per unique key-set for the process lifetime.
    """
    return _is_gpo_managed_cached(tuple(registry_keys))


@functools.lru_cache(maxsize=2048)
def _is_gpo_managed_cached(registry_keys: tuple[str, ...]) -> bool:
    """Cached implementation of :func:`is_gpo_managed`."""
    if not is_windows() or not registry_keys:
        return False
    try:
        import winreg

        for key in registry_keys:
            upper = key.upper()
            # Already a policy path — check if it actually exists with values
            if "\\POLICIES\\" in upper or "\\SOFTWARE\\POLICIES\\" in upper:
                hive, subpath = _split_hive(key)
                if hive is not None:
                    try:
                        with winreg.OpenKey(hive, subpath, 0, winreg.KEY_READ) as hk:
                            try:
                                winreg.EnumValue(hk, 0)
                                return True
                            except OSError:
                                # Key exists but no values
                                pass
                    except OSError:
                        pass
                continue

            # Non-policy key — derive the policy overlay path
            policy_path = _derive_policy_path(key)
            if policy_path is None:
                continue
            hive, subpath = _split_hive(policy_path)
            if hive is not None:
                try:
                    with winreg.OpenKey(hive, subpath, 0, winreg.KEY_READ) as hk:
                        try:
                            winreg.EnumValue(hk, 0)
                            return True
                        except OSError:
                            pass
                except OSError:
                    pass
    except Exception:
        pass
    return False


def _split_hive(key: str) -> tuple[int | None, str]:
    """Split a full registry path into (hive_constant, subpath)."""
    import winreg

    upper = key.upper()
    hive_map: dict[str, int] = {
        "HKEY_LOCAL_MACHINE\\": winreg.HKEY_LOCAL_MACHINE,
        "HKLM\\": winreg.HKEY_LOCAL_MACHINE,
        "HKEY_CURRENT_USER\\": winreg.HKEY_CURRENT_USER,
        "HKCU\\": winreg.HKEY_CURRENT_USER,
    }
    for prefix, hive in hive_map.items():
        if upper.startswith(prefix):
            return hive, key[len(prefix) :]
    return None, key


def _derive_policy_path(key: str) -> str | None:
    """Derive the Group Policy overlay path for a non-policy registry key.

    Examples:
      ``HKLM\\SOFTWARE\\Microsoft\\Edge\\...``
        → ``HKLM\\SOFTWARE\\Policies\\Microsoft\\Edge\\...``
      ``HKCU\\Software\\Microsoft\\Office\\...``
        → ``HKCU\\Software\\Policies\\Microsoft\\Office\\...``
    """
    upper = key.upper()
    # HKLM\\SOFTWARE\\... → HKLM\\SOFTWARE\\Policies\\...
    for hive_prefix in ("HKEY_LOCAL_MACHINE\\", "HKLM\\"):
        if upper.startswith(hive_prefix):
            rest = key[len(hive_prefix) :]
            rest_upper = rest.upper()
            if rest_upper.startswith("SOFTWARE\\"):
                inner = rest[len("SOFTWARE\\") :]
                return f"{key[: len(hive_prefix)]}SOFTWARE\\Policies\\{inner}"
    # HKCU\\Software\\... → HKCU\\Software\\Policies\\...
    for hive_prefix in ("HKEY_CURRENT_USER\\", "HKCU\\"):
        if upper.startswith(hive_prefix):
            rest = key[len(hive_prefix) :]
            rest_upper = rest.upper()
            if rest_upper.startswith("SOFTWARE\\"):
                inner = rest[len("SOFTWARE\\") :]
                return f"{key[: len(hive_prefix)]}Software\\Policies\\{inner}"
    return None


# ── Public API ───────────────────────────────────────────────────────────────


_corp_cache: bool | None = None
_corp_reasons: list[str] = []
_corp_lock = threading.Lock()

_CHECK_NAMES: list[tuple[str, str, str]] = [
    ("domain-join", "AD domain-joined", "_is_domain_joined"),
    ("azure-ad", "Azure AD joined", "_is_azure_ad_joined"),
    ("vpn-adapter", "VPN connected", "_has_vpn_adapter"),
    ("mgmt-agent", "Managed (SCCM/Intune)", "_has_management_agent"),
    ("group-policy", "Group Policy active", "_has_group_policy"),
]


def _run_corp_checks() -> tuple[bool, list[str]]:
    """Run all corporate detection checks in parallel, returning (is_corp, reasons).

    Each check is independent so they run concurrently in a thread pool,
    reducing worst-case time from ~50s sequential to ~10s parallel.
    """
    global _corp_cache, _corp_reasons
    # Fast path without lock (already populated)
    if _corp_cache is not None:
        return _corp_cache, list(_corp_reasons)
    with _corp_lock:
        # Double-checked locking inside the lock
        if _corp_cache is not None:
            return _corp_cache, list(_corp_reasons)

    import sys

    _this = sys.modules[__name__]
    reasons: list[str] = []

    def _run_single(name: str, label: str, fn_name: str) -> tuple[str, str, bool]:
        try:
            fn: Callable[[], bool] = getattr(_this, fn_name)
            return name, label, fn()
        except Exception as exc:
            SESSION.log(f"Corp-guard: {name} check failed: {exc}")
            return name, label, False

    with concurrent.futures.ThreadPoolExecutor(max_workers=5) as pool:
        futures = [pool.submit(_run_single, name, label, fn_name) for name, label, fn_name in _CHECK_NAMES]
        for future in concurrent.futures.as_completed(futures, timeout=30):
            _name, label, detected = future.result(timeout=15)
            if detected:
                reasons.append(label)

    is_corp = bool(reasons)
    if not is_corp:
        SESSION.log("Corp-guard: no corporate indicators found")

    with _corp_lock:
        _corp_cache = is_corp
        _corp_reasons = reasons
    return is_corp, reasons


def is_corporate_network() -> bool:
    """Return True when the machine appears to be on a corporate network.

    Result is cached for the process lifetime after the first call.

    Checks:
    1. Active Directory domain membership
    2. Azure AD / Hybrid join
    3. Active VPN adapters
    4. SCCM / Intune management agent
    5. Group Policy enforcement
    """
    return _run_corp_checks()[0]


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


def corp_guard_status() -> str | None:
    """Return a human-readable status string, or None if not corporate.

    Uses the same cached results as :func:`is_corporate_network` to avoid
    redundant subprocess calls.
    """
    if not is_windows():
        return None

    is_corp, reasons = _run_corp_checks()
    return ", ".join(reasons) if is_corp else None


def corp_guard_reasons() -> list[str]:
    """Return the list of reasons why the machine was flagged as corporate.

    Returns an empty list when no corporate indicators were found.
    Triggers the detection checks on first call (same cached result as
    :func:`is_corporate_network`).
    """
    if not is_windows():
        return []
    _, reasons = _run_corp_checks()
    return list(reasons)


def reset_corp_cache() -> None:
    """Clear the corporate-network detection result cache.

    Forces :func:`is_corporate_network` to re-run all checks on the next
    call.  Primarily useful for testing and for GUI refresh after the user
    disconnects from a VPN.
    """
    global _corp_cache, _corp_reasons
    with _corp_lock:
        _corp_cache = None
        _corp_reasons = []
