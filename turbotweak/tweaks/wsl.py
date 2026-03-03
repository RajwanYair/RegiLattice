"""WSL tweaks — Windows Subsystem for Linux registry optimisations.

These tweaks cover registry-based settings that improve the WSL experience.
File-based settings (.wslconfig) are outside the scope of this registry
toolkit; see Microsoft docs for those.
"""

from __future__ import annotations

import subprocess
from typing import List

from turbotweak.registry import SESSION, assert_admin
from turbotweak.tweaks import TweakDef

# ── WSL Default Version 2 ───────────────────────────────────────────────────

_LXSS_CU = r"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"
_LXSS_KEYS = [_LXSS_CU]


def apply_wsl_default_v2(*, require_admin: bool = True) -> None:
    """Set the default WSL distribution version to 2."""
    SESSION.log("Starting Add-WSLDefaultV2")
    SESSION.backup(_LXSS_KEYS, "WSL_DefaultV2")
    SESSION.set_dword(_LXSS_CU, "DefaultVersion", 2)
    SESSION.log("Completed Add-WSLDefaultV2")


def remove_wsl_default_v2(*, require_admin: bool = True) -> None:
    """Revert default WSL version to 1."""
    SESSION.log("Starting Remove-WSLDefaultV2")
    SESSION.backup(_LXSS_KEYS, "WSL_DefaultV2_Remove")
    SESSION.set_dword(_LXSS_CU, "DefaultVersion", 1)
    SESSION.log("Completed Remove-WSLDefaultV2")


def detect_wsl_default_v2() -> bool:
    return SESSION.read_dword(_LXSS_CU, "DefaultVersion") == 2


# ── WSL Auto-Start Service ──────────────────────────────────────────────────

_LXSS_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LxssManager"
_LXSS_SVC_KEYS = [_LXSS_SVC]


def apply_wsl_autostart(*, require_admin: bool = True) -> None:
    """Set LxssManager service to start automatically (Start=2)."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-WSLAutoStart")
    SESSION.backup(_LXSS_SVC_KEYS, "WSL_AutoStart")
    # 2 = SERVICE_AUTO_START, 3 = SERVICE_DEMAND_START (default)
    SESSION.set_dword(_LXSS_SVC, "Start", 2)
    SESSION.log("Completed Add-WSLAutoStart")


def remove_wsl_autostart(*, require_admin: bool = True) -> None:
    """Revert LxssManager to demand start (default)."""
    assert_admin(require_admin)
    SESSION.log("Starting Remove-WSLAutoStart")
    SESSION.backup(_LXSS_SVC_KEYS, "WSL_AutoStart_Remove")
    SESSION.set_dword(_LXSS_SVC, "Start", 3)
    SESSION.log("Completed Remove-WSLAutoStart")


def detect_wsl_autostart() -> bool:
    return SESSION.read_dword(_LXSS_SVC, "Start") == 2


# ── WSL Nested Virtualisation ───────────────────────────────────────────────

_HYPERV_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Virtualization"
)
_HYPERV_KEYS = [_HYPERV_KEY]


def apply_wsl_nested_virt(*, require_admin: bool = True) -> None:
    """Enable nested virtualisation so WSL2 can run Docker/VM workloads."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-WSLNestedVirt")
    SESSION.backup(_HYPERV_KEYS, "WSL_NestedVirt")
    SESSION.set_dword(_HYPERV_KEY, "NestedVirtualization", 1)
    SESSION.log("Completed Add-WSLNestedVirt")


def remove_wsl_nested_virt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-WSLNestedVirt")
    SESSION.backup(_HYPERV_KEYS, "WSL_NestedVirt_Remove")
    SESSION.delete_value(_HYPERV_KEY, "NestedVirtualization")
    SESSION.log("Completed Remove-WSLNestedVirt")


def detect_wsl_nested_virt() -> bool:
    return SESSION.read_dword(_HYPERV_KEY, "NestedVirtualization") == 1


# ── WSL Optional Feature (enable via DISM) ──────────────────────────────────

_WSL_FEATURE_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Component Based Servicing\Notifications"
    r"\OptionalFeatures\Microsoft-Windows-Subsystem-Linux"
)


def apply_wsl_feature(*, require_admin: bool = True) -> None:
    """Enable the 'Windows Subsystem for Linux' optional feature."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-WSLFeature")
    cmd = (
        "dism.exe /online /enable-feature "
        "/featurename:Microsoft-Windows-Subsystem-Linux /all /norestart"
    )
    subprocess.run(cmd, shell=True, check=False, capture_output=True)
    SESSION.log("Completed Add-WSLFeature")


def remove_wsl_feature(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-WSLFeature")
    cmd = (
        "dism.exe /online /disable-feature "
        "/featurename:Microsoft-Windows-Subsystem-Linux /norestart"
    )
    subprocess.run(cmd, shell=True, check=False, capture_output=True)
    SESSION.log("Completed Remove-WSLFeature")


def detect_wsl_feature() -> bool:
    return SESSION.key_exists(_WSL_FEATURE_KEY)


# ── Virtual Machine Platform Feature ────────────────────────────────────────

_VMP_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Component Based Servicing\Notifications"
    r"\OptionalFeatures\VirtualMachinePlatform"
)


def apply_vmp_feature(*, require_admin: bool = True) -> None:
    """Enable Virtual Machine Platform (required for WSL 2)."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-VMPlatform")
    cmd = (
        "dism.exe /online /enable-feature "
        "/featurename:VirtualMachinePlatform /all /norestart"
    )
    subprocess.run(cmd, shell=True, check=False, capture_output=True)
    SESSION.log("Completed Add-VMPlatform")


def remove_vmp_feature(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-VMPlatform")
    cmd = (
        "dism.exe /online /disable-feature "
        "/featurename:VirtualMachinePlatform /norestart"
    )
    subprocess.run(cmd, shell=True, check=False, capture_output=True)
    SESSION.log("Completed Remove-VMPlatform")


def detect_vmp_feature() -> bool:
    return SESSION.key_exists(_VMP_KEY)


# ── WSL DNS Auto-Config ─────────────────────────────────────────────────────

_LXSS_NET = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows" r"\CurrentVersion\Lxss"
_LXSS_NET_KEYS = [_LXSS_NET]


def apply_wsl_network_mode(*, require_admin: bool = True) -> None:
    """Set WSL 2 networking to mirrored mode for localhost forwarding."""
    assert_admin(require_admin)
    SESSION.log("Starting Add-WSLNetworkMode")
    SESSION.backup(_LXSS_NET_KEYS, "WSL_Network")
    # NetworkingMode: 0=NAT (default), 1=Mirrored
    SESSION.set_dword(_LXSS_NET, "EnableMirroredNetworking", 1)
    SESSION.log("Completed Add-WSLNetworkMode")


def remove_wsl_network_mode(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-WSLNetworkMode")
    SESSION.backup(_LXSS_NET_KEYS, "WSL_Network_Remove")
    SESSION.delete_value(_LXSS_NET, "EnableMirroredNetworking")
    SESSION.log("Completed Remove-WSLNetworkMode")


def detect_wsl_network_mode() -> bool:
    return SESSION.read_dword(_LXSS_NET, "EnableMirroredNetworking") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="wsl-default-v2",
        label="WSL Default Version 2",
        category="WSL",
        apply_fn=apply_wsl_default_v2,
        remove_fn=remove_wsl_default_v2,
        detect_fn=detect_wsl_default_v2,
        needs_admin=False,
        corp_safe=True,
        registry_keys=_LXSS_KEYS,
        description="Sets new WSL distributions to install as version 2 by default.",
    ),
    TweakDef(
        id="wsl-autostart",
        label="WSL Auto-Start Service",
        category="WSL",
        apply_fn=apply_wsl_autostart,
        remove_fn=remove_wsl_autostart,
        detect_fn=detect_wsl_autostart,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_LXSS_SVC_KEYS,
        description=("Sets LxssManager to automatic start so WSL is ready instantly."),
    ),
    TweakDef(
        id="wsl-nested-virt",
        label="WSL Nested Virtualisation",
        category="WSL",
        apply_fn=apply_wsl_nested_virt,
        remove_fn=remove_wsl_nested_virt,
        detect_fn=detect_wsl_nested_virt,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_HYPERV_KEYS,
        description=(
            "Enables nested virtualisation for WSL 2 guests, allowing "
            "Docker Desktop, KVM, and other VM workloads inside WSL."
        ),
    ),
    TweakDef(
        id="wsl-feature",
        label="Enable WSL Feature",
        category="WSL",
        apply_fn=apply_wsl_feature,
        remove_fn=remove_wsl_feature,
        detect_fn=detect_wsl_feature,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WSL_FEATURE_KEY],
        description="Enables the Windows Subsystem for Linux optional feature via DISM.",
    ),
    TweakDef(
        id="wsl-vm-platform",
        label="Enable VM Platform",
        category="WSL",
        apply_fn=apply_vmp_feature,
        remove_fn=remove_vmp_feature,
        detect_fn=detect_vmp_feature,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VMP_KEY],
        description=(
            "Enables the Virtual Machine Platform feature (required for WSL 2)."
        ),
    ),
    TweakDef(
        id="wsl-mirrored-network",
        label="WSL Mirrored Networking",
        category="WSL",
        apply_fn=apply_wsl_network_mode,
        remove_fn=remove_wsl_network_mode,
        detect_fn=detect_wsl_network_mode,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_LXSS_NET_KEYS,
        description=(
            "Switches WSL 2 networking to mirrored mode so localhost "
            "forwarding and host-network access work transparently."
        ),
    ),
]
