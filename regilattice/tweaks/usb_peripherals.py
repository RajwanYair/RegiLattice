"""USB & Peripherals tweaks.

Covers USB selective suspend, power management, removable storage policies,
AutoPlay, mass-storage restrictions, and USB hub power settings.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_USB_POWER = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"
_USB_STOR = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR"
_AUTOPLAY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"
_AUTOPLAY_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"
_REMOVABLE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices"
_USB_HUB = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbhub\HubG"
_SELECTIVE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"
_WPD_DENY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}"
_ENUM_USB = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\USB"
_STORAGE_POL = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"

# ── 1. Disable USB selective suspend ─────────────────────────────────────────


def _apply_disable_usb_suspend(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SELECTIVE, "DisableSelectiveSuspend", 1)


def _remove_disable_usb_suspend(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SELECTIVE, "DisableSelectiveSuspend", 0)


def _detect_disable_usb_suspend() -> bool:
    return SESSION.read_dword(_SELECTIVE, "DisableSelectiveSuspend") == 1


# ── 2. Disable AutoPlay for all devices ──────────────────────────────────────


def _apply_disable_autoplay(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_AUTOPLAY, "DisableAutoplay", 1)


def _remove_disable_autoplay(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_AUTOPLAY, "DisableAutoplay", 0)


def _detect_disable_autoplay() -> bool:
    return SESSION.read_dword(_AUTOPLAY, "DisableAutoplay") == 1


# ── 3. Disable AutoPlay (machine policy) ─────────────────────────────────────


def _apply_disable_autoplay_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_AUTOPLAY_POLICY, "NoDriveTypeAutoRun", 255)


def _remove_disable_autoplay_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_AUTOPLAY_POLICY, "NoDriveTypeAutoRun", 145)


def _detect_disable_autoplay_policy() -> bool:
    return SESSION.read_dword(_AUTOPLAY_POLICY, "NoDriveTypeAutoRun") == 255


# ── 4. Disable USB mass storage driver ───────────────────────────────────────


def _apply_disable_usb_storage(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_USB_STOR, "Start", 4)


def _remove_disable_usb_storage(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_USB_STOR, "Start", 3)


def _detect_disable_usb_storage() -> bool:
    return SESSION.read_dword(_USB_STOR, "Start") == 4


# ── 5. Deny write to removable drives ────────────────────────────────────────


def _apply_deny_removable_write(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_STORAGE_POL, "Deny_Write", 1)


def _remove_deny_removable_write(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STORAGE_POL, "Deny_Write")


def _detect_deny_removable_write() -> bool:
    return SESSION.read_dword(_STORAGE_POL, "Deny_Write") == 1


# ── 6. Deny read from removable drives ───────────────────────────────────────


def _apply_deny_removable_read(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_STORAGE_POL, "Deny_Read", 1)


def _remove_deny_removable_read(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STORAGE_POL, "Deny_Read")


def _detect_deny_removable_read() -> bool:
    return SESSION.read_dword(_STORAGE_POL, "Deny_Read") == 1


# ── 7. Deny execute from removable drives ────────────────────────────────────


def _apply_deny_removable_exec(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_STORAGE_POL, "Deny_Execute", 1)


def _remove_deny_removable_exec(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STORAGE_POL, "Deny_Execute")


def _detect_deny_removable_exec() -> bool:
    return SESSION.read_dword(_STORAGE_POL, "Deny_Execute") == 1


# ── 8. Deny WPD (Windows Portable Device) access ──────────────────────────────


def _apply_deny_wpd(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WPD_DENY, "Deny_Read", 1)
    SESSION.set_dword(_WPD_DENY, "Deny_Write", 1)


def _remove_deny_wpd(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WPD_DENY, "Deny_Read")
    SESSION.delete_value(_WPD_DENY, "Deny_Write")


def _detect_deny_wpd() -> bool:
    return SESSION.read_dword(_WPD_DENY, "Deny_Read") == 1


# ── 9. Disable all removable storage (policy) ────────────────────────────────


def _apply_disable_all_removable(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_REMOVABLE, "Deny_All", 1)


def _remove_disable_all_removable(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_REMOVABLE, "Deny_All")


def _detect_disable_all_removable() -> bool:
    return SESSION.read_dword(_REMOVABLE, "Deny_All") == 1


# ── 10. Disable AutoRun for all drives ───────────────────────────────────────

_AUTORUN = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"


def _apply_disable_autorun(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_AUTORUN, "NoAutorun", 1)


def _remove_disable_autorun(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_AUTORUN, "NoAutorun")


def _detect_disable_autorun() -> bool:
    return SESSION.read_dword(_AUTORUN, "NoAutorun") == 1


# ── 11. Force safe removal notification ──────────────────────────────────────

_HOTPLUG = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoplayHandlers"


def _apply_force_safe_remove(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_HOTPLUG, "ShowSafeRemovePrompt", 1)


def _remove_force_safe_remove(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_HOTPLUG, "ShowSafeRemovePrompt")


def _detect_force_safe_remove() -> bool:
    return SESSION.read_dword(_HOTPLUG, "ShowSafeRemovePrompt") == 1


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="usb-disable-selective-suspend",
        label="Disable USB Selective Suspend",
        category="USB & Peripherals",
        apply_fn=_apply_disable_usb_suspend,
        remove_fn=_remove_disable_usb_suspend,
        detect_fn=_detect_disable_usb_suspend,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SELECTIVE],
        description="Prevent USB devices from entering low-power suspend. Fixes disconnect issues. Default: enabled. Recommended: disabled.",
        tags=["usb", "suspend", "power", "disconnect"],
    ),
    TweakDef(
        id="usb-disable-autoplay",
        label="Disable AutoPlay (User)",
        category="USB & Peripherals",
        apply_fn=_apply_disable_autoplay,
        remove_fn=_remove_disable_autoplay,
        detect_fn=_detect_disable_autoplay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_AUTOPLAY],
        description="Disable automatic playback dialog when inserting media. Default: enabled. Recommended: disabled.",
        tags=["autoplay", "media", "usb"],
    ),
    TweakDef(
        id="usb-disable-autoplay-policy",
        label="Disable AutoPlay (Machine Policy)",
        category="USB & Peripherals",
        apply_fn=_apply_disable_autoplay_policy,
        remove_fn=_remove_disable_autoplay_policy,
        detect_fn=_detect_disable_autoplay_policy,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_AUTOPLAY_POLICY],
        description="Machine-wide policy: disable AutoPlay for all drive types. Default: partial (145). Recommended: full disable (255).",
        tags=["autoplay", "policy", "security"],
    ),
    TweakDef(
        id="usb-disable-mass-storage",
        label="Disable USB Mass Storage Driver",
        category="USB & Peripherals",
        apply_fn=_apply_disable_usb_storage,
        remove_fn=_remove_disable_usb_storage,
        detect_fn=_detect_disable_usb_storage,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_USB_STOR],
        description="Prevent USB flash drives from being mounted. Security hardening. Default: enabled (Start=3).",
        tags=["usb", "storage", "security", "block"],
    ),
    TweakDef(
        id="usb-deny-removable-write",
        label="Deny Write to Removable Drives",
        category="USB & Peripherals",
        apply_fn=_apply_deny_removable_write,
        remove_fn=_remove_deny_removable_write,
        detect_fn=_detect_deny_removable_write,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORAGE_POL],
        description="Block writing to removable storage devices via policy. Default: allowed.",
        tags=["removable", "write", "policy", "dlp"],
    ),
    TweakDef(
        id="usb-deny-removable-read",
        label="Deny Read from Removable Drives",
        category="USB & Peripherals",
        apply_fn=_apply_deny_removable_read,
        remove_fn=_remove_deny_removable_read,
        detect_fn=_detect_deny_removable_read,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORAGE_POL],
        description="Block reading from removable storage devices via policy. Default: allowed.",
        tags=["removable", "read", "policy", "dlp"],
    ),
    TweakDef(
        id="usb-deny-removable-execute",
        label="Deny Execute from Removable Drives",
        category="USB & Peripherals",
        apply_fn=_apply_deny_removable_exec,
        remove_fn=_remove_deny_removable_exec,
        detect_fn=_detect_deny_removable_exec,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORAGE_POL],
        description="Block execution of programs from removable storage. Default: allowed. Recommended: enabled for security.",
        tags=["removable", "execute", "policy", "security"],
    ),
    TweakDef(
        id="usb-deny-wpd-access",
        label="Deny WPD (Portable Device) Access",
        category="USB & Peripherals",
        apply_fn=_apply_deny_wpd,
        remove_fn=_remove_deny_wpd,
        detect_fn=_detect_deny_wpd,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WPD_DENY],
        description="Block read/write to Windows Portable Devices (phones, cameras). Default: allowed.",
        tags=["wpd", "portable", "phone", "camera", "mtp"],
    ),
    TweakDef(
        id="usb-disable-all-removable",
        label="Deny All Removable Storage",
        category="USB & Peripherals",
        apply_fn=_apply_disable_all_removable,
        remove_fn=_remove_disable_all_removable,
        detect_fn=_detect_disable_all_removable,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_REMOVABLE],
        description="Block all removable storage device classes. Maximum lockdown. Default: allowed.",
        tags=["removable", "all", "block", "lockdown"],
    ),
    TweakDef(
        id="usb-disable-autorun",
        label="Disable AutoRun for All Drives",
        category="USB & Peripherals",
        apply_fn=_apply_disable_autorun,
        remove_fn=_remove_disable_autorun,
        detect_fn=_detect_disable_autorun,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_AUTORUN],
        description="Disable AutoRun.inf processing for all drive types. Prevents malware auto-execution. Default: enabled. Recommended: disabled.",
        tags=["autorun", "security", "malware"],
    ),
    TweakDef(
        id="usb-force-safe-removal",
        label="Force Safe Removal Notification",
        category="USB & Peripherals",
        apply_fn=_apply_force_safe_remove,
        remove_fn=_remove_force_safe_remove,
        detect_fn=_detect_force_safe_remove,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HOTPLUG],
        description="Always show 'Safe to Remove Hardware' notification. Default: conditional.",
        tags=["safe-remove", "eject", "notification"],
    ),
]


# -- 12. Disable USB 3.0 Link Power Management ───────────────────────────────

_USB_CTRL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"


def _apply_disable_usb3_lpm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_USB_CTRL], "USB3LPM")
    SESSION.set_dword(_USB_CTRL, "EnhancedPowerManagementEnabled", 0)


def _remove_disable_usb3_lpm(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_USB_CTRL, "EnhancedPowerManagementEnabled", 1)


def _detect_disable_usb3_lpm() -> bool:
    return SESSION.read_dword(_USB_CTRL, "EnhancedPowerManagementEnabled") == 0


# -- 13. Disable Write to Removable Storage ─────────────────────────────────

_STORAGE_DEV_POL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies"


def _apply_removable_write_protect(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_STORAGE_DEV_POL], "RemovableWriteProtect")
    SESSION.set_dword(_STORAGE_DEV_POL, "WriteProtect", 1)


def _remove_removable_write_protect(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_STORAGE_DEV_POL, "WriteProtect", 0)


def _detect_removable_write_protect() -> bool:
    return SESSION.read_dword(_STORAGE_DEV_POL, "WriteProtect") == 1


TWEAKS += [
    TweakDef(
        id="usb-disable-usb3-power-save",
        label="Disable USB 3.0 Link Power Management",
        category="USB & Peripherals",
        apply_fn=_apply_disable_usb3_lpm,
        remove_fn=_remove_disable_usb3_lpm,
        detect_fn=_detect_disable_usb3_lpm,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_USB_CTRL],
        description=(
            "Disables USB 3.0 enhanced power management / link power management. "
            "Improves USB stability at cost of power. Default: Enabled. Recommended: Disabled for desktops."
        ),
        tags=["usb", "usb3", "power", "lpm", "stability"],
    ),
    TweakDef(
        id="usb-disable-removable-write",
        label="Disable Write to Removable Storage",
        category="USB & Peripherals",
        apply_fn=_apply_removable_write_protect,
        remove_fn=_remove_removable_write_protect,
        detect_fn=_detect_removable_write_protect,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_STORAGE_DEV_POL],
        description=(
            "Enables write protection on all removable storage devices. "
            "Prevents data exfiltration via USB drives. Default: Disabled. Recommended: Enabled for secure envs."
        ),
        tags=["usb", "removable", "write-protect", "security", "dlp"],
    ),
]
