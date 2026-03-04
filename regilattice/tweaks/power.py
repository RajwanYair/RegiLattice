"""Power management registry tweaks.

Covers USB selective suspend, high-performance power plans,
hibernation, prefetch/superfetch, and processor scheduling.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_POWER = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
_USB_POWER = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\USB\DisableSelectiveSuspend"
)
_USB_HUB = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbhub\HubG"
)
_HIBERNATE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
_PREFETCH = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management\PrefetchParameters"
)
_SUPERFETCH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"
_PRIO_CTRL = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"
)
_MM = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management"
)


# ── Disable USB Selective Suspend ────────────────────────────────────────────

_USB_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\USB"
)


def _apply_disable_usb_suspend(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable USB selective suspend")
    SESSION.backup([_USB_KEY], "USBSuspend")
    SESSION.set_dword(_USB_KEY, "DisableSelectiveSuspend", 1)


def _remove_disable_usb_suspend(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_USB_KEY, "DisableSelectiveSuspend", 0)


def _detect_disable_usb_suspend() -> bool:
    return SESSION.read_dword(_USB_KEY, "DisableSelectiveSuspend") == 1


# ── Disable Hibernation ─────────────────────────────────────────────────────


def _apply_disable_hibernation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable hibernation")
    SESSION.backup([_HIBERNATE], "Hibernation")
    SESSION.set_dword(_HIBERNATE, "HibernateEnabled", 0)
    SESSION.set_dword(_HIBERNATE, "HiberFileSizePercent", 0)


def _remove_disable_hibernation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_HIBERNATE, "HibernateEnabled", 1)
    SESSION.set_dword(_HIBERNATE, "HiberFileSizePercent", 75)


def _detect_disable_hibernation() -> bool:
    return SESSION.read_dword(_HIBERNATE, "HibernateEnabled") == 0


# ── Disable Prefetch / Superfetch (SysMain) ──────────────────────────────────


def _apply_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable Prefetch + Superfetch")
    SESSION.backup([_PREFETCH, _SUPERFETCH], "Prefetch")
    SESSION.set_dword(_PREFETCH, "EnablePrefetcher", 0)
    SESSION.set_dword(_PREFETCH, "EnableSuperfetch", 0)
    SESSION.set_dword(_SUPERFETCH, "Start", 4)  # 4 = Disabled


def _remove_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PREFETCH, "EnablePrefetcher", 3)
    SESSION.set_dword(_PREFETCH, "EnableSuperfetch", 3)
    SESSION.set_dword(_SUPERFETCH, "Start", 2)  # 2 = Automatic


def _detect_disable_prefetch() -> bool:
    return SESSION.read_dword(_PREFETCH, "EnablePrefetcher") == 0


# ── Optimize Processor Scheduling for Programs ──────────────────────────────


def _apply_proc_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: optimize processor scheduling for programs")
    SESSION.backup([_PRIO_CTRL], "ProcScheduling")
    SESSION.set_dword(_PRIO_CTRL, "Win32PrioritySeparation", 38)


def _remove_proc_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PRIO_CTRL, "Win32PrioritySeparation", 2)  # default


def _detect_proc_scheduling() -> bool:
    return SESSION.read_dword(_PRIO_CTRL, "Win32PrioritySeparation") == 38


# ── Disable Fast Startup ────────────────────────────────────────────────────


def _apply_disable_fast_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable Fast Startup (hybrid boot)")
    SESSION.backup([_POWER], "FastStartup")
    SESSION.set_dword(_POWER, "HiberbootEnabled", 0)


def _remove_disable_fast_startup(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_POWER, "HiberbootEnabled", 1)


def _detect_disable_fast_startup() -> bool:
    return SESSION.read_dword(_POWER, "HiberbootEnabled") == 0


# ── Large System Cache ───────────────────────────────────────────────────────


def _apply_large_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: enable large system cache")
    SESSION.backup([_MM], "LargeCache")
    SESSION.set_dword(_MM, "LargeSystemCache", 1)


def _remove_large_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MM, "LargeSystemCache", 0)


def _detect_large_cache() -> bool:
    return SESSION.read_dword(_MM, "LargeSystemCache") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-usb-suspend",
        label="Disable USB Selective Suspend",
        category="Power",
        apply_fn=_apply_disable_usb_suspend,
        remove_fn=_remove_disable_usb_suspend,
        detect_fn=_detect_disable_usb_suspend,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_USB_KEY],
        description=(
            "Prevents Windows from powering down USB devices to save "
            "energy, fixing USB disconnection issues."
        ),
        tags=["power", "usb", "stability"],
    ),
    TweakDef(
        id="disable-hibernation",
        label="Disable Hibernation",
        category="Power",
        apply_fn=_apply_disable_hibernation,
        remove_fn=_remove_disable_hibernation,
        detect_fn=_detect_disable_hibernation,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HIBERNATE],
        description="Disables hibernation and removes the hiberfil.sys file.",
        tags=["power", "disk", "performance"],
    ),
    TweakDef(
        id="disable-prefetch",
        label="Disable Prefetch / Superfetch",
        category="Power",
        apply_fn=_apply_disable_prefetch,
        remove_fn=_remove_disable_prefetch,
        detect_fn=_detect_disable_prefetch,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PREFETCH, _SUPERFETCH],
        description=(
            "Disables Prefetch and Superfetch (SysMain) services — "
            "beneficial on SSD-only systems."
        ),
        tags=["power", "performance", "ssd"],
    ),
    TweakDef(
        id="optimize-proc-scheduling",
        label="Optimize Processor Scheduling",
        category="Power",
        apply_fn=_apply_proc_scheduling,
        remove_fn=_remove_proc_scheduling,
        detect_fn=_detect_proc_scheduling,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PRIO_CTRL],
        description=(
            "Adjusts Win32PrioritySeparation for foreground-app "
            "responsiveness (value 38)."
        ),
        tags=["power", "performance", "cpu"],
    ),
    TweakDef(
        id="disable-fast-startup",
        label="Disable Fast Startup",
        category="Power",
        apply_fn=_apply_disable_fast_startup,
        remove_fn=_remove_disable_fast_startup,
        detect_fn=_detect_disable_fast_startup,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_POWER],
        description=(
            "Disables Windows Fast Startup (hybrid boot) which can "
            "cause driver and dual-boot issues."
        ),
        tags=["power", "boot", "stability"],
    ),
    TweakDef(
        id="large-system-cache",
        label="Enable Large System Cache",
        category="Power",
        apply_fn=_apply_large_cache,
        remove_fn=_remove_large_cache,
        detect_fn=_detect_large_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MM],
        description=(
            "Enables large system cache, dedicating more RAM for file "
            "caching (beneficial with 16 GB+ RAM)."
        ),
        tags=["power", "performance", "memory"],
    ),
]
