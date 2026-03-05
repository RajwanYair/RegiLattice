"""Power management registry tweaks.

Covers USB selective suspend, high-performance power plans,
hibernation, prefetch/superfetch, and processor scheduling.
"""

from __future__ import annotations

import subprocess

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_POWER = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
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
_POWER_THROTTLE = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
    r"\PowerThrottling"
)
_DISK_IDLE = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\disk"
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


# ── Disable Power Throttling ───────────────────────────────────────────────


def _apply_disable_power_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable power throttling")
    SESSION.backup([_POWER_THROTTLE], "PowerThrottle")
    SESSION.set_dword(_POWER_THROTTLE, "PowerThrottlingOff", 1)


def _remove_disable_power_throttle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_POWER_THROTTLE, "PowerThrottlingOff")


def _detect_disable_power_throttle() -> bool:
    return SESSION.read_dword(_POWER_THROTTLE, "PowerThrottlingOff") == 1


# ── NTFS Disable Last Access Timestamp ────────────────────────────────────

_NTFS = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\FileSystem"
)


def _apply_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable NTFS last-access timestamp updates")
    SESSION.backup([_NTFS], "NtfsLastAccess")
    SESSION.set_dword(_NTFS, "NtfsDisableLastAccessUpdate", 1)


def _remove_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NTFS, "NtfsDisableLastAccessUpdate", 0)


def _detect_disable_last_access() -> bool:
    return SESSION.read_dword(_NTFS, "NtfsDisableLastAccessUpdate") == 1


# ── Disable Connected Standby ────────────────────────────────────────────────


def _apply_disable_connected_standby(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable Connected Standby (Modern Standby)")
    SESSION.backup([_POWER], "ConnectedStandby")
    SESSION.set_dword(_POWER, "CsEnabled", 0)


def _remove_disable_connected_standby(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_POWER, "CsEnabled", 1)


def _detect_disable_connected_standby() -> bool:
    return SESSION.read_dword(_POWER, "CsEnabled") == 0


# ── Disable Core Parking ─────────────────────────────────────────────────────

_CORE_PARKING = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
    r"\PowerSettings\54533251-82be-4824-96c1-47b60b740d00"
    r"\0cc5b647-c1df-4637-891a-dec35c318583"
)


def _apply_disable_core_parking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable CPU core parking")
    SESSION.backup([_CORE_PARKING], "CoreParking")
    SESSION.set_dword(_CORE_PARKING, "ValueMax", 0)


def _remove_disable_core_parking(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CORE_PARKING, "ValueMax")


def _detect_disable_core_parking() -> bool:
    return SESSION.read_dword(_CORE_PARKING, "ValueMax") == 0


# ── Disable Turbo Boost Limit ────────────────────────────────────────────────

_TURBO = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
    r"\PowerSettings\54533251-82be-4824-96c1-47b60b740d00"
    r"\be337238-0d82-4146-a960-4f3749d470c7"
)


def _apply_max_turbo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: set max processor state to 100%")
    SESSION.backup([_TURBO], "TurboBoost")
    SESSION.set_dword(_TURBO, "ValueMax", 100)


def _remove_max_turbo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TURBO, "ValueMax")


def _detect_max_turbo() -> bool:
    return SESSION.read_dword(_TURBO, "ValueMax") == 100


# ── Disable Sleep Timeout (Never Sleep on AC) ────────────────────────────────

_SLEEP_TIMEOUT_AC = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
    r"\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20"
    r"\29f6c1db-86da-48c5-9fdb-f2b67b1f44da"
)


def _apply_disable_sleep(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable automatic sleep on AC power")
    SESSION.backup([_SLEEP_TIMEOUT_AC], "SleepTimeout")
    SESSION.set_dword(_SLEEP_TIMEOUT_AC, "ValueMax", 0)  # 0 = never


def _remove_disable_sleep(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SLEEP_TIMEOUT_AC, "ValueMax", 1800)  # 30 min default


def _detect_disable_sleep() -> bool:
    return SESSION.read_dword(_SLEEP_TIMEOUT_AC, "ValueMax") == 0


# ── Disable Disk Idle Timeout ────────────────────────────────────────────────


def _apply_disable_disk_idle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable disk idle timeout")
    SESSION.backup([_DISK_IDLE], "DiskIdle")
    SESSION.set_dword(_DISK_IDLE, "TimeOutValue", 0)


def _remove_disable_disk_idle(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DISK_IDLE, "TimeOutValue", 20)  # default 20 min


def _detect_disable_disk_idle() -> bool:
    return SESSION.read_dword(_DISK_IDLE, "TimeOutValue") == 0


# ── Disable USB Selective Suspend (Extra) ──────────────────────────────────


def _apply_power_disable_usb_suspend(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable USB selective suspend (desktop)")
    SESSION.backup([_USB_KEY], "PowerUSBSuspend")
    SESSION.set_dword(_USB_KEY, "DisableSelectiveSuspend", 1)


def _remove_power_disable_usb_suspend(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_USB_KEY, "DisableSelectiveSuspend", 0)


def _detect_power_disable_usb_suspend() -> bool:
    return SESSION.read_dword(_USB_KEY, "DisableSelectiveSuspend") == 1


# ── Disable Hibernation (Free Disk Space) ─────────────────────────────────


def _apply_power_disable_hdd_powerdown(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable hibernation to free disk space")
    SESSION.backup([_HIBERNATE], "PowerHDDPowerdown")
    SESSION.set_dword(_HIBERNATE, "HibernateEnabled", 0)


def _remove_power_disable_hdd_powerdown(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_HIBERNATE, "HibernateEnabled", 1)


def _detect_power_disable_hdd_powerdown() -> bool:
    return SESSION.read_dword(_HIBERNATE, "HibernateEnabled") == 0


# ── Disable Adaptive Brightness ──────────────────────────────────────────

_SENSOR_API = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft"
    r"\Windows NT\CurrentVersion\SensorAPI"
)


def _apply_disable_adaptive_brightness(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable adaptive display brightness")
    SESSION.backup([_SENSOR_API], "AdaptiveBrightness")
    SESSION.set_dword(_SENSOR_API, "AllowAdaptiveBrightness", 0)


def _remove_disable_adaptive_brightness(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SENSOR_API, "AllowAdaptiveBrightness", 1)


def _detect_disable_adaptive_brightness() -> bool:
    return SESSION.read_dword(_SENSOR_API, "AllowAdaptiveBrightness") == 0


# ── High Performance Power Plan ───────────────────────────────────────────


def _apply_high_perf_plan(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: set active power plan to High Performance")
    subprocess.run(
        ["powercfg", "/setactive", "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"],
        check=True, capture_output=True, text=True,
    )


def _remove_high_perf_plan(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: restore Balanced power plan")
    subprocess.run(
        ["powercfg", "/setactive", "381b4222-f694-41f0-9685-ff5bb260df2e"],
        check=True, capture_output=True, text=True,
    )


def _detect_high_perf_plan() -> bool:
    try:
        r = subprocess.run(
            ["powercfg", "/getactivescheme"],
            capture_output=True, text=True, timeout=5, check=False,
        )
        return "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c" in r.stdout
    except (OSError, subprocess.SubprocessError):
        return False


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
    TweakDef(
        id="disable-power-throttling",
        label="Disable Power Throttling",
        category="Power",
        apply_fn=_apply_disable_power_throttle,
        remove_fn=_remove_disable_power_throttle,
        detect_fn=_detect_disable_power_throttle,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_POWER_THROTTLE],
        description="Disables CPU power throttling for maximum sustained performance.",
        tags=["power", "performance", "cpu"],
    ),
    TweakDef(
        id="disable-ntfs-last-access",
        label="Disable NTFS Last Access Timestamp",
        category="Power",
        apply_fn=_apply_disable_last_access,
        remove_fn=_remove_disable_last_access,
        detect_fn=_detect_disable_last_access,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NTFS],
        description="Stops NTFS from updating last-access timestamps, reducing disk I/O.",
        tags=["power", "performance", "disk", "ntfs"],
    ),
    TweakDef(
        id="disable-connected-standby",
        label="Disable Connected Standby",
        category="Power",
        apply_fn=_apply_disable_connected_standby,
        remove_fn=_remove_disable_connected_standby,
        detect_fn=_detect_disable_connected_standby,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_POWER],
        description=(
            "Disables Modern/Connected Standby which can cause high battery "
            "drain and wake-from-sleep issues on some laptops."
        ),
        tags=["power", "standby", "laptop", "battery"],
    ),
    TweakDef(
        id="disable-core-parking",
        label="Disable CPU Core Parking",
        category="Power",
        apply_fn=_apply_disable_core_parking,
        remove_fn=_remove_disable_core_parking,
        detect_fn=_detect_disable_core_parking,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CORE_PARKING],
        description=(
            "Disables CPU core parking so all cores stay active. "
            "Can improve latency-sensitive workloads and gaming."
        ),
        tags=["power", "cpu", "performance", "gaming"],
    ),
    TweakDef(
        id="max-processor-turbo",
        label="Set Max Processor State to 100%",
        category="Power",
        apply_fn=_apply_max_turbo,
        remove_fn=_remove_max_turbo,
        detect_fn=_detect_max_turbo,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TURBO],
        description="Ensures the CPU is allowed to reach its maximum turbo frequency.",
        tags=["power", "cpu", "turbo", "performance"],
    ),
    TweakDef(
        id="disable-sleep-ac",
        label="Disable Auto-Sleep on AC Power",
        category="Power",
        apply_fn=_apply_disable_sleep,
        remove_fn=_remove_disable_sleep,
        detect_fn=_detect_disable_sleep,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SLEEP_TIMEOUT_AC],
        description="Prevents the PC from automatically sleeping while on AC power.",
        tags=["power", "sleep", "desktop"],
    ),
    TweakDef(
        id="disable-disk-idle",
        label="Disable Disk Idle Timeout",
        category="Power",
        apply_fn=_apply_disable_disk_idle,
        remove_fn=_remove_disable_disk_idle,
        detect_fn=_detect_disable_disk_idle,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DISK_IDLE],
        description="Prevents hard drives from spinning down when idle. Reduces wake latency.",
        tags=["power", "disk", "performance"],
    ),
    TweakDef(
        id="power-disable-usb-suspend",
        label="Disable USB Selective Suspend",
        category="Power",
        apply_fn=_apply_power_disable_usb_suspend,
        remove_fn=_remove_power_disable_usb_suspend,
        detect_fn=_detect_power_disable_usb_suspend,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_USB_KEY],
        description=(
            "Disables USB selective suspend power management. Prevents USB "
            "devices from disconnecting during use. Default: Enabled (0). "
            "Recommended: Disabled (1) for desktops."
        ),
        tags=["power", "usb", "suspend", "stability"],
    ),
    TweakDef(
        id="power-disable-hdd-powerdown",
        label="Disable Hard Disk Power Down",
        category="Power",
        apply_fn=_apply_power_disable_hdd_powerdown,
        remove_fn=_remove_power_disable_hdd_powerdown,
        detect_fn=_detect_power_disable_hdd_powerdown,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HIBERNATE],
        description=(
            "Disables hibernation (hiberfil.sys). Frees disk space equal to "
            "RAM size and speeds up shutdown. Default: Enabled. "
            "Recommended: Disabled for desktops with SSDs."
        ),
        tags=["power", "hibernate", "disk", "performance"],
    ),
    TweakDef(
        id="power-disable-adaptive-brightness",
        label="Disable Adaptive Display Brightness",
        category="Power",
        apply_fn=_apply_disable_adaptive_brightness,
        remove_fn=_remove_disable_adaptive_brightness,
        detect_fn=_detect_disable_adaptive_brightness,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SENSOR_API],
        description=(
            "Disables adaptive display brightness that adjusts screen "
            "brightness based on ambient light sensor readings. "
            "Default: enabled (1). Recommended: disabled on desktops "
            "or when manual brightness control is preferred."
        ),
        tags=["power", "brightness", "display", "sensor"],
    ),
    TweakDef(
        id="power-high-performance-plan",
        label="Set High Performance Power Plan",
        category="Power",
        apply_fn=_apply_high_perf_plan,
        remove_fn=_remove_high_perf_plan,
        detect_fn=_detect_high_perf_plan,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
        description=(
            "Sets the active power plan to High Performance "
            "(8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c). Maximizes CPU "
            "frequency and disables power-saving features. "
            "Remove reverts to Balanced plan."
        ),
        tags=["power", "plan", "high-performance", "cpu"],
    ),
]


# ── Disable USB Power Saving (global) ────────────────────────────────────────


def _apply_usb_power_save_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable USB selective suspend globally")
    SESSION.backup([_USB_KEY], "USBPowerSave")
    SESSION.set_dword(_USB_KEY, "DisableSelectiveSuspend", 1)


def _remove_usb_power_save_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_USB_KEY, "DisableSelectiveSuspend", 0)


def _detect_usb_power_save_off() -> bool:
    return SESSION.read_dword(_USB_KEY, "DisableSelectiveSuspend") == 1


# ── Disable Connected Standby (PCI Express PM) ──────────────────────────────


def _apply_pci_express_pm_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable connected standby (CsEnabled=0)")
    SESSION.backup([_POWER], "PciExpressPM")
    SESSION.set_dword(_POWER, "CsEnabled", 0)


def _remove_pci_express_pm_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_POWER, "CsEnabled", 1)


def _detect_pci_express_pm_off() -> bool:
    return SESSION.read_dword(_POWER, "CsEnabled") == 0


TWEAKS += [
    TweakDef(
        id="power-disable-usb-power-save",
        label="Disable USB Power Saving",
        category="Power",
        apply_fn=_apply_usb_power_save_off,
        remove_fn=_remove_usb_power_save_off,
        detect_fn=_detect_usb_power_save_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_USB_KEY],
        description=(
            "Disables USB selective suspend globally to prevent USB device "
            "disconnects. Keeps all USB ports powered at all times. "
            "Default: Enabled. Recommended: Disabled for desktop PCs."
        ),
        tags=["power", "usb", "suspend", "stability"],
    ),
    TweakDef(
        id="power-disable-pci-express-pm",
        label="Disable Connected Standby",
        category="Power",
        apply_fn=_apply_pci_express_pm_off,
        remove_fn=_remove_pci_express_pm_off,
        detect_fn=_detect_pci_express_pm_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_POWER],
        description=(
            "Disables Connected Standby (Modern Standby) which manages PCI Express "
            "link state power. Prevents low-power idle states that can cause "
            "wake issues. Default: Enabled. Recommended: Disabled for desktops."
        ),
        tags=["power", "connected-standby", "pci-express", "idle"],
    ),
]


# ── Disable USB Selective Suspend (USBHUB3) ─────────────────────────────────

_USBHUB3_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
    r"\USBHUB3\Parameters"
)


def _apply_usb_selective_suspend_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable USB 3.0 hub selective suspend")
    SESSION.backup([_USBHUB3_KEY], "USBHUB3SelectiveSuspend")
    SESSION.set_dword(_USBHUB3_KEY, "DisableSelectiveSuspend", 1)


def _remove_usb_selective_suspend_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_USBHUB3_KEY, "DisableSelectiveSuspend")


def _detect_usb_selective_suspend_off() -> bool:
    return SESSION.read_dword(_USBHUB3_KEY, "DisableSelectiveSuspend") == 1


# ── Disable PCI Express Link State Power Management (ASPM) ──────────────────

_PCI_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI"


def _apply_pcie_link_pm_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable PCI Express ASPM (link state PM)")
    SESSION.backup([_PCI_KEY], "PCIeLinkPM")
    SESSION.set_dword(_PCI_KEY, "ASPMOptOut", 1)


def _remove_pcie_link_pm_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PCI_KEY, "ASPMOptOut")


def _detect_pcie_link_pm_off() -> bool:
    return SESSION.read_dword(_PCI_KEY, "ASPMOptOut") == 1


# ── Disable Processor Idle States ───────────────────────────────────────────

_POWER_IDLE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"


def _apply_disable_idle_states(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Power: disable processor idle states")
    SESSION.backup([_POWER_IDLE], "ProcessorIdleStates")
    SESSION.set_dword(_POWER_IDLE, "EnergyEstimationEnabled", 0)
    SESSION.set_dword(_POWER_IDLE, "ExitLatencyCheckEnabled", 1)


def _remove_disable_idle_states(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_POWER_IDLE, "EnergyEstimationEnabled")
    SESSION.delete_value(_POWER_IDLE, "ExitLatencyCheckEnabled")


def _detect_disable_idle_states() -> bool:
    return (
        SESSION.read_dword(_POWER_IDLE, "EnergyEstimationEnabled") == 0
        and SESSION.read_dword(_POWER_IDLE, "ExitLatencyCheckEnabled") == 1
    )


TWEAKS += [
    TweakDef(
        id="pwr-disable-usb-selective-suspend",
        label="Disable USB 3.0 Selective Suspend",
        category="Power",
        apply_fn=_apply_usb_selective_suspend_off,
        remove_fn=_remove_usb_selective_suspend_off,
        detect_fn=_detect_usb_selective_suspend_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_USBHUB3_KEY],
        description=(
            "Disables selective suspend on USB 3.0 hubs (USBHUB3 driver). "
            "Prevents USB 3.0 device disconnects during idle. "
            "Default: Enabled. Recommended: Disabled for desktop PCs."
        ),
        tags=["power", "usb", "selective-suspend", "usb3"],
    ),
    TweakDef(
        id="pwr-pcie-link-pm-off",
        label="Disable PCI Express Link State Power Management",
        category="Power",
        apply_fn=_apply_pcie_link_pm_off,
        remove_fn=_remove_pcie_link_pm_off,
        detect_fn=_detect_pcie_link_pm_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_PCI_KEY],
        description=(
            "Opts out of Active State Power Management (ASPM) for PCI Express "
            "devices. Prevents link-state power saving that can cause latency. "
            "Default: ASPM enabled. Recommended: Disabled for low-latency."
        ),
        tags=["power", "pcie", "aspm", "latency"],
    ),
    TweakDef(
        id="pwr-disable-idle-states",
        label="Disable Processor Idle States",
        category="Power",
        apply_fn=_apply_disable_idle_states,
        remove_fn=_remove_disable_idle_states,
        detect_fn=_detect_disable_idle_states,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_POWER_IDLE],
        description=(
            "Disables energy estimation and enables exit latency checking to "
            "prevent deep processor idle states (C-states). Maximises CPU "
            "responsiveness. Default: Enabled. Recommended: Disabled for gaming."
        ),
        tags=["power", "idle", "c-states", "processor", "latency"],
    ),
]
