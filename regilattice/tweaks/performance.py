"""Performance tweaks — startup delay, SvcHost split, NTFS last access."""

from __future__ import annotations

import subprocess

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Performance (startup / responsiveness / network) ─────────────────────────

_PERF_SERIALIZE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Serialize"
)
_PERF_MULTIMEDIA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Multimedia\SystemProfile"
)
_PERF_KEYS = [_PERF_SERIALIZE, _PERF_MULTIMEDIA]


def apply_performance(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-Performance")
    SESSION.backup(_PERF_KEYS, "Performance")
    SESSION.set_dword(_PERF_SERIALIZE, "StartupDelayInMSec", 0)
    SESSION.set_dword(_PERF_SERIALIZE, "WaitforIdleState", 0)
    SESSION.set_dword(_PERF_MULTIMEDIA, "SystemResponsiveness", 10)
    SESSION.set_dword(_PERF_MULTIMEDIA, "NetworkThrottlingIndex", 0xFFFFFFFF)
    SESSION.log("Completed Add-Performance")


def remove_performance(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-Performance")
    SESSION.backup(_PERF_KEYS, "Performance_Remove")
    SESSION.delete_value(_PERF_SERIALIZE, "StartupDelayInMSec")
    SESSION.delete_value(_PERF_SERIALIZE, "WaitforIdleState")
    SESSION.set_dword(_PERF_MULTIMEDIA, "SystemResponsiveness", 20)
    SESSION.set_dword(_PERF_MULTIMEDIA, "NetworkThrottlingIndex", 10)
    SESSION.log("Completed Remove-Performance")


def detect_performance() -> bool:
    return SESSION.read_dword(_PERF_SERIALIZE, "StartupDelayInMSec") == 0


# ── SvcHost Split Threshold ─────────────────────────────────────────────────

_SVCHOST_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"


def apply_svchost_split(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-SvcHostSplit")
    SESSION.backup([_SVCHOST_KEY], "SvcHostSplit")
    import ctypes

    class MEMORYSTATUSEX(ctypes.Structure):
        _fields_ = [
            ("dwLength", ctypes.c_ulong),
            ("dwMemoryLoad", ctypes.c_ulong),
            ("ullTotalPhys", ctypes.c_ulonglong),
            ("ullAvailPhys", ctypes.c_ulonglong),
            ("ullTotalPageFile", ctypes.c_ulonglong),
            ("ullAvailPageFile", ctypes.c_ulonglong),
            ("ullTotalVirtual", ctypes.c_ulonglong),
            ("ullAvailVirtual", ctypes.c_ulonglong),
            ("ullAvailExtendedVirtual", ctypes.c_ulonglong),
        ]

    mem = MEMORYSTATUSEX()
    mem.dwLength = ctypes.sizeof(MEMORYSTATUSEX)
    ctypes.windll.kernel32.GlobalMemoryStatusEx(ctypes.byref(mem))
    ram_kb = int(mem.ullTotalPhys / 1024)
    SESSION.set_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB", ram_kb)
    SESSION.log(f"Completed Add-SvcHostSplit: {ram_kb} KB")


def remove_svchost_split(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-SvcHostSplit")
    SESSION.backup([_SVCHOST_KEY], "SvcHostSplit_Remove")
    SESSION.set_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB", 380000)
    SESSION.log("Completed Remove-SvcHostSplit")


def detect_svchost_split() -> bool:
    val = SESSION.read_dword(_SVCHOST_KEY, "SvcHostSplitThresholdInKB")
    return val is not None and val > 380000


# ── NTFS Last Access Timestamps ─────────────────────────────────────────────


def apply_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableLastAccess")
    subprocess.run(
        ["fsutil", "behavior", "set", "disablelastaccess", "1"],
        check=True,
        capture_output=True,
        text=True,
    )
    SESSION.log("Completed Add-DisableLastAccess")


def remove_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableLastAccess")
    subprocess.run(
        ["fsutil", "behavior", "set", "disablelastaccess", "2"],
        check=True,
        capture_output=True,
        text=True,
    )
    SESSION.log("Completed Remove-DisableLastAccess")


def detect_disable_last_access() -> bool:
    try:
        r = subprocess.run(
            ["fsutil", "behavior", "query", "disablelastaccess"],
            capture_output=True,
            text=True,
            timeout=5,
            check=False,
        )
        # Output like "DisableLastAccess = 1"
        return "= 1" in r.stdout
    except Exception:
        return False


# ── Disable Transparency Effects ─────────────────────────────────────────

_PERSONALIZE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Themes\Personalize"
)


def _apply_disable_transparency(*, require_admin: bool = False) -> None:
    SESSION.log("Performance: disable transparency effects")
    SESSION.backup([_PERSONALIZE], "Transparency")
    SESSION.set_dword(_PERSONALIZE, "EnableTransparency", 0)


def _remove_disable_transparency(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_PERSONALIZE, "EnableTransparency", 1)


def _detect_disable_transparency() -> bool:
    return SESSION.read_dword(_PERSONALIZE, "EnableTransparency") == 0


# ── Disable Background Apps ──────────────────────────────────────────────

_BG_APPS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\BackgroundAccessApplications"
)


def _apply_disable_bg_apps(*, require_admin: bool = False) -> None:
    SESSION.log("Performance: disable background UWP apps")
    SESSION.backup([_BG_APPS], "BgApps")
    SESSION.set_dword(_BG_APPS, "GlobalUserDisabled", 1)


def _remove_disable_bg_apps(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_BG_APPS, "GlobalUserDisabled", 0)


def _detect_disable_bg_apps() -> bool:
    return SESSION.read_dword(_BG_APPS, "GlobalUserDisabled") == 1


# ── Disable Window Animations ───────────────────────────────────────────────

_VISUAL_FX = r"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"


def _apply_disable_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable window animations")
    SESSION.backup([_VISUAL_FX], "Animations")
    SESSION.set_string(_VISUAL_FX, "MinAnimate", "0")


def _remove_disable_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_VISUAL_FX, "MinAnimate", "1")


def _detect_disable_animations() -> bool:
    return SESSION.read_string(_VISUAL_FX, "MinAnimate") == "0"


# ── Reduce Menu Show Delay ──────────────────────────────────────────────────

_DESKTOP_KEY = r"HKEY_CURRENT_USER\Control Panel\Desktop"


def _apply_menu_delay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: reduce menu show delay to 50ms")
    SESSION.backup([_DESKTOP_KEY], "MenuDelay")
    SESSION.set_string(_DESKTOP_KEY, "MenuShowDelay", "50")


def _remove_menu_delay(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_DESKTOP_KEY, "MenuShowDelay", "400")


def _detect_menu_delay() -> bool:
    val = SESSION.read_string(_DESKTOP_KEY, "MenuShowDelay")
    return val is not None and int(val) <= 100


# ── Disable SearchProtocolHost Priority Boost ────────────────────────────────

_SEARCH_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search"


def _apply_disable_search_protocol_host(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable SearchProtocolHost priority boost")
    SESSION.backup([_SEARCH_KEY], "SearchProtocolHost")
    SESSION.set_dword(_SEARCH_KEY, "SetupCompletedSuccessfully", 0)


def _remove_disable_search_protocol_host(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_KEY, "SetupCompletedSuccessfully", 1)


def _detect_disable_search_protocol_host() -> bool:
    return SESSION.read_dword(_SEARCH_KEY, "SetupCompletedSuccessfully") == 0


# ── Large System Cache ───────────────────────────────────────────────────────

_MEMORY_MGMT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management"
)


def _apply_large_system_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: enable large system cache")
    SESSION.backup([_MEMORY_MGMT], "LargeSystemCache")
    SESSION.set_dword(_MEMORY_MGMT, "LargeSystemCache", 1)


def _remove_large_system_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MEMORY_MGMT, "LargeSystemCache", 0)


def _detect_large_system_cache() -> bool:
    return SESSION.read_dword(_MEMORY_MGMT, "LargeSystemCache") == 1


# ── Disable Paging Executive ─────────────────────────────────────────────────


def _apply_disable_paging_executive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable paging of kernel to disk")
    SESSION.backup([_MEMORY_MGMT], "DisablePagingExecutive")
    SESSION.set_dword(_MEMORY_MGMT, "DisablePagingExecutive", 1)


def _remove_disable_paging_executive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MEMORY_MGMT, "DisablePagingExecutive", 0)


def _detect_disable_paging_executive() -> bool:
    return SESSION.read_dword(_MEMORY_MGMT, "DisablePagingExecutive") == 1


# ── Optimize Processor Scheduling ────────────────────────────────────────────

_PRIORITY_CTRL = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"


def _apply_optimize_processor_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: optimize processor scheduling for programs")
    SESSION.backup([_PRIORITY_CTRL], "ProcessorScheduling")
    SESSION.set_dword(_PRIORITY_CTRL, "Win32PrioritySeparation", 38)


def _remove_optimize_processor_scheduling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PRIORITY_CTRL, "Win32PrioritySeparation", 2)


def _detect_optimize_processor_scheduling() -> bool:
    return SESSION.read_dword(_PRIORITY_CTRL, "Win32PrioritySeparation") == 38


# ── Disable NTFS Encryption (EFS) ────────────────────────────────────────────

_FILESYSTEM = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"


def _apply_disable_ntfs_encryption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable NTFS encryption (EFS)")
    SESSION.backup([_FILESYSTEM], "NtfsEncryption")
    SESSION.set_dword(_FILESYSTEM, "NtfsDisableEncryption", 1)


def _remove_disable_ntfs_encryption(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_FILESYSTEM, "NtfsDisableEncryption", 0)


def _detect_disable_ntfs_encryption() -> bool:
    return SESSION.read_dword(_FILESYSTEM, "NtfsDisableEncryption") == 1


# ── Disable Last Access Timestamp ────────────────────────────────────────────


def _apply_disable_last_access_ts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable NTFS last access timestamp updates")
    SESSION.backup([_FILESYSTEM], "LastAccessTimestamp")
    SESSION.set_dword(_FILESYSTEM, "NtfsDisableLastAccessUpdate", 1)


def _remove_disable_last_access_ts(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_FILESYSTEM, "NtfsDisableLastAccessUpdate", 0)


def _detect_disable_last_access_ts() -> bool:
    return SESSION.read_dword(_FILESYSTEM, "NtfsDisableLastAccessUpdate") == 1


# ── Disable Spectre/Meltdown Mitigations ────────────────────────────────────


def _apply_disable_spectre(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable Spectre/Meltdown mitigations")
    SESSION.backup([_MEMORY_MGMT], "SpectreMitigations")
    SESSION.set_dword(_MEMORY_MGMT, "FeatureSettingsOverride", 3)
    SESSION.set_dword(_MEMORY_MGMT, "FeatureSettingsOverrideMask", 3)


def _remove_disable_spectre(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MEMORY_MGMT, "FeatureSettingsOverride")
    SESSION.delete_value(_MEMORY_MGMT, "FeatureSettingsOverrideMask")


def _detect_disable_spectre() -> bool:
    return SESSION.read_dword(_MEMORY_MGMT, "FeatureSettingsOverride") == 3


# ── Unpark CPU Cores ─────────────────────────────────────────────────────────

_CORE_PARKING_PERF = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
    r"\PowerSettings\54533251-82be-4824-96c1-47b60b740d00"
    r"\0cc5b647-c1df-4637-891a-dec35c318583"
)


def _apply_unpark_cpu_cores(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: unpark all CPU cores")
    SESSION.backup([_CORE_PARKING_PERF], "UnparkCPUCores")
    SESSION.set_dword(_CORE_PARKING_PERF, "ValueMax", 0)
    SESSION.set_dword(_CORE_PARKING_PERF, "ValueMin", 0)


def _remove_unpark_cpu_cores(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CORE_PARKING_PERF, "ValueMax")
    SESSION.delete_value(_CORE_PARKING_PERF, "ValueMin")


def _detect_unpark_cpu_cores() -> bool:
    return SESSION.read_dword(_CORE_PARKING_PERF, "ValueMax") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="perf-performance",
        label="Performance Tweaks (Visual Effects)",
        category="Performance",
        apply_fn=apply_performance,
        remove_fn=remove_performance,
        detect_fn=detect_performance,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_PERF_KEYS,
        description=("Removes startup delay, lowers system responsiveness timer, and disables network throttling for snappier performance."),
        tags=["performance", "startup", "network"],
    ),
    TweakDef(
        id="perf-svchost-split",
        label="Optimize SvcHost Split (RAM-based)",
        category="Performance",
        apply_fn=apply_svchost_split,
        remove_fn=remove_svchost_split,
        detect_fn=detect_svchost_split,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SVCHOST_KEY],
        description=("Raises the SvcHost split threshold to match installed RAM, reducing the number of svchost.exe processes."),
        tags=["performance", "memory", "svchost"],
    ),
    TweakDef(
        id="perf-disable-ntfs-last-access",
        label="Disable NTFS Last Access Timestamp",
        category="Performance",
        apply_fn=apply_disable_last_access,
        remove_fn=remove_disable_last_access,
        detect_fn=detect_disable_last_access,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[],
        description="Disables NTFS last-access timestamp updates to reduce disk I/O overhead.",
        tags=["performance", "ntfs", "disk"],
    ),
    TweakDef(
        id="perf-disable-transparency",
        label="Disable Transparency Effects",
        category="Performance",
        apply_fn=_apply_disable_transparency,
        remove_fn=_remove_disable_transparency,
        detect_fn=_detect_disable_transparency,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PERSONALIZE],
        description=(
            "Disables window transparency/blur effects for snappier UI. "
            "Reduces GPU compositing overhead. Default: Enabled. "
            "Recommended: Disabled for performance."
        ),
        tags=["performance", "visual", "transparency"],
    ),
    TweakDef(
        id="perf-disable-background-apps",
        label="Disable Background UWP Apps",
        category="Performance",
        apply_fn=_apply_disable_bg_apps,
        remove_fn=_remove_disable_bg_apps,
        detect_fn=_detect_disable_bg_apps,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_BG_APPS],
        description=(
            "Prevents Store/UWP apps from running in the background. Frees CPU, "
            "memory, and network resources used by idle Store apps. "
            "Default: Enabled. Recommended: Disabled for performance."
        ),
        tags=["performance", "uwp", "background"],
    ),
    TweakDef(
        id="perf-disable-window-animations",
        label="Disable Window Animations",
        category="Performance",
        apply_fn=_apply_disable_animations,
        remove_fn=_remove_disable_animations,
        detect_fn=_detect_disable_animations,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VISUAL_FX],
        description=("Disables window minimize/maximize animations and taskbar animations for snappier window management."),
        tags=["performance", "visual", "animations"],
    ),
    TweakDef(
        id="perf-menu-show-delay",
        label="Reduce Menu Show Delay",
        category="Performance",
        apply_fn=_apply_menu_delay,
        remove_fn=_remove_menu_delay,
        detect_fn=_detect_menu_delay,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DESKTOP_KEY],
        description=("Reduces the delay before menus appear from 400ms to 50ms. Makes context menus and Start menu feel instant."),
        tags=["performance", "menu", "ux", "responsiveness"],
    ),
    TweakDef(
        id="perf-disable-search-protocol-host",
        label="Disable SearchProtocolHost Priority Boost",
        category="Performance",
        apply_fn=_apply_disable_search_protocol_host,
        remove_fn=_remove_disable_search_protocol_host,
        detect_fn=_detect_disable_search_protocol_host,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SEARCH_KEY],
        description=("Disables SearchProtocolHost priority boost to reduce background CPU usage from Windows Search indexing."),
        tags=["performance", "search", "indexing"],
    ),
    TweakDef(
        id="perf-large-system-cache",
        label="Enable Large System Cache",
        category="Performance",
        apply_fn=_apply_large_system_cache,
        remove_fn=_remove_large_system_cache,
        detect_fn=_detect_large_system_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MEMORY_MGMT],
        description=("Enables large system cache, allowing Windows to use more RAM for file caching and improving disk performance."),
        tags=["performance", "memory", "cache"],
    ),
    TweakDef(
        id="perf-disable-paging-executive",
        label="Disable Paging of Kernel to Disk",
        category="Performance",
        apply_fn=_apply_disable_paging_executive,
        remove_fn=_remove_disable_paging_executive,
        detect_fn=_detect_disable_paging_executive,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MEMORY_MGMT],
        description=("Keeps kernel and drivers in physical RAM instead of paging them to disk, improving system responsiveness."),
        tags=["performance", "memory", "kernel", "paging"],
    ),
    TweakDef(
        id="perf-optimize-processor-scheduling",
        label="Optimize for Programs (Not Services)",
        category="Performance",
        apply_fn=_apply_optimize_processor_scheduling,
        remove_fn=_remove_optimize_processor_scheduling,
        detect_fn=_detect_optimize_processor_scheduling,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PRIORITY_CTRL],
        description=(
            "Sets Win32PrioritySeparation to 38 (0x26): short variable quantum "
            "with maximum foreground boost. Prioritizes interactive desktop apps "
            "over background services and increases scheduler responsiveness. "
            "Default: 2. Recommended: 38 for desktops, 2 for servers."
        ),
        tags=["performance", "cpu", "scheduling", "responsiveness", "priority", "foreground", "quantum"],
    ),
    TweakDef(
        id="perf-disable-ntfs-encryption",
        label="Disable NTFS Encryption (EFS) Service",
        category="Performance",
        apply_fn=_apply_disable_ntfs_encryption,
        remove_fn=_remove_disable_ntfs_encryption,
        detect_fn=_detect_disable_ntfs_encryption,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FILESYSTEM],
        description=("Disables NTFS Encrypting File System to reduce filesystem overhead. Not recommended if EFS encryption is in use."),
        tags=["performance", "ntfs", "encryption", "filesystem"],
    ),
    TweakDef(
        id="perf-disable-last-access",
        label="Disable Last Access Timestamp",
        category="Performance",
        apply_fn=_apply_disable_last_access_ts,
        remove_fn=_remove_disable_last_access_ts,
        detect_fn=_detect_disable_last_access_ts,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FILESYSTEM],
        description=(
            "Disables NTFS last access timestamp updates. Reduces disk "
            "I/O for every file read operation. Default: 0 (Enabled). "
            "Recommended: 1 (Disabled)."
        ),
        tags=["performance", "ntfs", "disk", "io"],
    ),
    TweakDef(
        id="perf-disable-spectre-mitigations",
        label="Disable Spectre/Meltdown Mitigations",
        category="Performance",
        apply_fn=_apply_disable_spectre,
        remove_fn=_remove_disable_spectre,
        detect_fn=_detect_disable_spectre,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_MEMORY_MGMT],
        description=(
            "Disables Spectre and Meltdown CPU mitigations for maximum "
            "performance. WARNING: reduces security. Only use on trusted, "
            "isolated machines. Default: mitigations enabled. "
            "Recommended: keep enabled unless benchmarking."
        ),
        tags=["performance", "spectre", "meltdown", "cpu", "security"],
    ),
    TweakDef(
        id="perf-unpark-cpu-cores",
        label="Unpark All CPU Cores",
        category="Performance",
        apply_fn=_apply_unpark_cpu_cores,
        remove_fn=_remove_unpark_cpu_cores,
        detect_fn=_detect_unpark_cpu_cores,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CORE_PARKING_PERF],
        description=(
            "Disables CPU core parking so all cores remain active at all "
            "times. Reduces latency spikes in real-time and gaming "
            "workloads. Default: Windows-managed. "
            "Recommended: disabled for desktops and gaming rigs."
        ),
        tags=["performance", "cpu", "core-parking", "latency", "gaming"],
    ),
]


# ══ Additional Performance Tweaks (Sophia Script / WinUtil) ══════════════

_POWER_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"
_MULTIMEDIA_SYS = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Multimedia\SystemProfile"
)
_MEMORY_MGMT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management"
)


# -- Disable Modern Standby (S0 Low Power Idle) ─────────────────────────


def _apply_disable_modern_standby(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable Modern Standby (S0 Low Power Idle)")
    SESSION.backup([_POWER_KEY], "ModernStandby")
    SESSION.set_dword(_POWER_KEY, "PlatformAoAcOverride", 0)


def _remove_disable_modern_standby(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_POWER_KEY, "PlatformAoAcOverride")


def _detect_disable_modern_standby() -> bool:
    return SESSION.read_dword(_POWER_KEY, "PlatformAoAcOverride") == 0


# -- Multimedia System Profile: Gaming Priority ─────────────────────────


def _apply_multimedia_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: set multimedia system profile to gaming priority")
    SESSION.backup([_MULTIMEDIA_SYS], "MultimediaPriority")
    SESSION.set_dword(_MULTIMEDIA_SYS, "SystemResponsiveness", 0)
    SESSION.set_string(_MULTIMEDIA_SYS, "NetworkThrottlingIndex", "4294967295")


def _remove_multimedia_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MULTIMEDIA_SYS, "SystemResponsiveness", 20)
    SESSION.delete_value(_MULTIMEDIA_SYS, "NetworkThrottlingIndex")


def _detect_multimedia_priority() -> bool:
    return SESSION.read_dword(_MULTIMEDIA_SYS, "SystemResponsiveness") == 0


# -- Disable Prefetch ───────────────────────────────────────────────────

_PREFETCH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"


def _apply_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable Prefetch (SSD systems)")
    SESSION.backup([_PREFETCH], "Prefetch")
    SESSION.set_dword(_PREFETCH, "EnablePrefetcher", 0)


def _remove_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PREFETCH, "EnablePrefetcher", 3)


def _detect_disable_prefetch() -> bool:
    return SESSION.read_dword(_PREFETCH, "EnablePrefetcher") == 0


# -- Large Page Support ─────────────────────────────────────────────────


def _apply_large_pages(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: enable large memory pages")
    SESSION.backup([_MEMORY_MGMT], "LargePages")
    SESSION.set_dword(_MEMORY_MGMT, "LargePageMinimum", 1)


def _remove_large_pages(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MEMORY_MGMT, "LargePageMinimum")


def _detect_large_pages() -> bool:
    return SESSION.read_dword(_MEMORY_MGMT, "LargePageMinimum") == 1


TWEAKS += [
    TweakDef(
        id="perf-disable-modern-standby",
        label="Disable Modern Standby (S0)",
        category="Performance",
        apply_fn=_apply_disable_modern_standby,
        remove_fn=_remove_disable_modern_standby,
        detect_fn=_detect_disable_modern_standby,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_POWER_KEY],
        description=(
            "Disables Modern Standby (S0 Low Power Idle) and restores classic "
            "S3 sleep. Prevents wake-from-sleep issues and battery drain. "
            "Default: Modern Standby. Recommended: disabled on desktops."
        ),
        tags=["performance", "standby", "sleep", "s3", "power"],
    ),
    TweakDef(
        id="perf-multimedia-priority",
        label="Multimedia Gaming Priority (SystemProfile)",
        category="Performance",
        apply_fn=_apply_multimedia_priority,
        remove_fn=_remove_multimedia_priority,
        detect_fn=_detect_multimedia_priority,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MULTIMEDIA_SYS],
        description=(
            "Configures the multimedia system profile for maximum gaming "
            "priority (SystemResponsiveness=0, no network throttling). "
            "Default: balanced (20). Recommended: 0 for gaming."
        ),
        tags=["performance", "multimedia", "gaming", "priority", "network"],
    ),
    TweakDef(
        id="perf-disable-prefetch",
        label="Disable Prefetch (SSD Systems)",
        category="Performance",
        apply_fn=_apply_disable_prefetch,
        remove_fn=_remove_disable_prefetch,
        detect_fn=_detect_disable_prefetch,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PREFETCH],
        description=(
            "Disables the Windows Prefetcher which is unnecessary on SSD systems "
            "and wastes I/O cycles. Default: enabled (3). Recommended: disabled on SSDs."
        ),
        tags=["performance", "prefetch", "ssd", "io"],
    ),
    TweakDef(
        id="perf-large-pages",
        label="Enable Large Memory Pages",
        category="Performance",
        apply_fn=_apply_large_pages,
        remove_fn=_remove_large_pages,
        detect_fn=_detect_large_pages,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MEMORY_MGMT],
        description=(
            "Enables large page support for improved memory performance in memory-intensive applications. Default: disabled. Recommended: enabled."
        ),
        tags=["performance", "memory", "large-pages", "ram"],
    ),
]


# -- Disable Memory Page Combining (Compression) ───────────────────────────


def _apply_disable_memory_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable memory page combining")
    SESSION.backup([_MEMORY_MGMT], "MemoryCompression")
    SESSION.set_dword(_MEMORY_MGMT, "DisablePageCombining", 1)


def _remove_disable_memory_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MEMORY_MGMT, "DisablePageCombining")


def _detect_disable_memory_compression() -> bool:
    return SESSION.read_dword(_MEMORY_MGMT, "DisablePageCombining") == 1


TWEAKS += [
    TweakDef(
        id="perf-disable-memory-compression",
        label="Disable Memory Page Combining",
        category="Performance",
        apply_fn=_apply_disable_memory_compression,
        remove_fn=_remove_disable_memory_compression,
        detect_fn=_detect_disable_memory_compression,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MEMORY_MGMT],
        description=(
            "Disables memory page combining (compression) to reduce CPU overhead "
            "on systems with ample RAM. Default: Enabled. "
            "Recommended: Disabled on 16 GB+ systems."
        ),
        tags=["performance", "memory", "compression", "page-combining"],
    ),
]

# ── Extra performance controls ───────────────────────────────────────────────

_FOREGROUND = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"
_GPU_SCHED = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"
_IO_PRIORITY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"
_SPLIT_LARGE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"


def _apply_perf_win32_priority_sep(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_FOREGROUND], "Win32PriSep")
    SESSION.set_dword(_FOREGROUND, "Win32PrioritySeparation", 38)


def _remove_perf_win32_priority_sep(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_FOREGROUND, "Win32PrioritySeparation", 2)  # Windows default


def _detect_perf_win32_priority_sep() -> bool:
    return SESSION.read_dword(_FOREGROUND, "Win32PrioritySeparation") == 38


def _apply_perf_gpu_hw_sched(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_GPU_SCHED], "GPUHWSched")
    SESSION.set_dword(_GPU_SCHED, "HwSchMode", 2)


def _remove_perf_gpu_hw_sched(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GPU_SCHED, "HwSchMode")


def _detect_perf_gpu_hw_sched() -> bool:
    return SESSION.read_dword(_GPU_SCHED, "HwSchMode") == 2


def _apply_perf_disable_lock_pages(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SPLIT_LARGE], "LockPagesInMemory")
    SESSION.set_dword(_SPLIT_LARGE, "LargePageMinimum", 0)


def _remove_perf_disable_lock_pages(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPLIT_LARGE, "LargePageMinimum")


def _detect_perf_disable_lock_pages() -> bool:
    return SESSION.read_dword(_SPLIT_LARGE, "LargePageMinimum") == 0


def _apply_perf_games_io_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_IO_PRIORITY], "GamesIOPriority")
    SESSION.set_dword(_IO_PRIORITY, "Affinity", 0)
    SESSION.set_dword(_IO_PRIORITY, "Background Only", 0)
    SESSION.set_dword(_IO_PRIORITY, "Priority", 6)


def _remove_perf_games_io_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_IO_PRIORITY, "Affinity")
    SESSION.delete_value(_IO_PRIORITY, "Background Only")
    SESSION.delete_value(_IO_PRIORITY, "Priority")


def _detect_perf_games_io_priority() -> bool:
    return SESSION.read_dword(_IO_PRIORITY, "Priority") == 6


def _apply_perf_disable_hung_app_detection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_FOREGROUND], "HungAppDetect")
    SESSION.set_dword(_FOREGROUND, "HungAppTimeout", 1000)


def _remove_perf_disable_hung_app_detection(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_FOREGROUND, "HungAppTimeout", 5000)  # default 5s


def _detect_perf_disable_hung_app_detection() -> bool:
    return SESSION.read_dword(_FOREGROUND, "HungAppTimeout") == 1000


TWEAKS += [
    TweakDef(
        id="perf-win32-priority-sep",
        label="Optimize Win32 Priority Separation for Performance",
        category="Performance",
        apply_fn=_apply_perf_win32_priority_sep,
        remove_fn=_remove_perf_win32_priority_sep,
        detect_fn=_detect_perf_win32_priority_sep,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FOREGROUND],
        description=(
            "Sets Win32PrioritySeparation=38 (hex 26) to give foreground programs 6x more "
            "CPU time than background processes. Maximises responsiveness. "
            "Default: 2. Recommended: 38 for gaming/workstations."
        ),
        tags=["performance", "priority", "foreground", "cpu", "win32"],
    ),
    TweakDef(
        id="perf-gpu-hw-scheduling",
        label="Enable GPU Hardware Accelerated Scheduling",
        category="Performance",
        apply_fn=_apply_perf_gpu_hw_sched,
        remove_fn=_remove_perf_gpu_hw_sched,
        detect_fn=_detect_perf_gpu_hw_sched,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GPU_SCHED],
        description=(
            "Enables Hardware Accelerated GPU Scheduling (HAGS) mode 2. "
            "Reduces GPU latency by allowing GPU to manage its own memory directly. "
            "Default: Disabled. Recommended: Enabled on Win10 2004+ with supported GPU."
        ),
        tags=["performance", "gpu", "scheduling", "hags", "latency"],
    ),
    TweakDef(
        id="perf-large-page-minimum",
        label="Configure Large Page Minimum in Memory Manager",
        category="Performance",
        apply_fn=_apply_perf_disable_lock_pages,
        remove_fn=_remove_perf_disable_lock_pages,
        detect_fn=_detect_perf_disable_lock_pages,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SPLIT_LARGE],
        description=(
            "Sets LargePageMinimum=0 to allow applications to use large page memory when beneficial. "
            "Can improve performance for large-memory workloads. Default: Not set. Recommended: 0."
        ),
        tags=["performance", "memory", "large-page", "allocation"],
    ),
    TweakDef(
        id="perf-games-io-priority",
        label="Set Highest IO Priority for Games Multimedia Profile",
        category="Performance",
        apply_fn=_apply_perf_games_io_priority,
        remove_fn=_remove_perf_games_io_priority,
        detect_fn=_detect_perf_games_io_priority,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_IO_PRIORITY],
        description=(
            "Sets the Games multimedia system profile to highest scheduling priority (6) "
            "with no background-only restriction. Reduces stutter in games. "
            "Default: 2. Recommended: 6 for gaming."
        ),
        tags=["performance", "gaming", "io", "priority", "multimedia"],
    ),
    TweakDef(
        id="perf-reduce-hung-app-timeout",
        label="Reduce Hung Application Detection Timeout",
        category="Performance",
        apply_fn=_apply_perf_disable_hung_app_detection,
        remove_fn=_remove_perf_disable_hung_app_detection,
        detect_fn=_detect_perf_disable_hung_app_detection,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FOREGROUND],
        description=(
            "Reduces HungAppTimeout from 5000ms to 1000ms so Windows terminates "
            "frozen applications faster. Improves system responsiveness on crashes. "
            "Default: 5000ms. Recommended: 1000ms."
        ),
        tags=["performance", "hung-app", "timeout", "responsiveness"],
    ),
]


# ── Additional Performance Tweaks ─────────────────────────────────────────────

_EXPLORER_ADV = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"
_POLICIES_EXP_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"


def _apply_perf_disable_font_smoothing(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable font smoothing (ClearType)")
    SESSION.backup([_DESKTOP_KEY], "FontSmoothing")
    SESSION.set_string(_DESKTOP_KEY, "FontSmoothing", "0")


def _remove_perf_disable_font_smoothing(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_DESKTOP_KEY, "FontSmoothing", "2")


def _detect_perf_disable_font_smoothing() -> bool:
    return SESSION.read_string(_DESKTOP_KEY, "FontSmoothing") == "0"


def _apply_perf_always_unload_dll(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: always unload DLLs from memory on process exit")
    SESSION.backup([_EXPLORER_ADV], "AlwaysUnloadDLL")
    SESSION.set_dword(_EXPLORER_ADV, "AlwaysUnloadDLL", 1)


def _remove_perf_always_unload_dll(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXPLORER_ADV, "AlwaysUnloadDLL")


def _detect_perf_always_unload_dll() -> bool:
    return SESSION.read_dword(_EXPLORER_ADV, "AlwaysUnloadDLL") == 1


def _apply_perf_increase_icon_cache(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: increase Explorer icon cache size to 4096")
    SESSION.backup([_EXPLORER_ADV], "MaxIconCache")
    SESSION.set_string(_EXPLORER_ADV, "Max Cached Icons", "4096")


def _remove_perf_increase_icon_cache(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXPLORER_ADV, "Max Cached Icons")


def _detect_perf_increase_icon_cache() -> bool:
    val = SESSION.read_string(_EXPLORER_ADV, "Max Cached Icons")
    return val is not None and int(val) >= 2048


def _apply_perf_clear_recent_docs_exit(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: clear recent documents on logoff")
    SESSION.backup([_POLICIES_EXP_CU], "ClearRecentDocs")
    SESSION.set_dword(_POLICIES_EXP_CU, "ClearRecentDocsOnExit", 1)


def _remove_perf_clear_recent_docs_exit(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_POLICIES_EXP_CU, "ClearRecentDocsOnExit")


def _detect_perf_clear_recent_docs_exit() -> bool:
    return SESSION.read_dword(_POLICIES_EXP_CU, "ClearRecentDocsOnExit") == 1


_EXPLORER_ADVANCED = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"


def _apply_perf_disable_thumbnail_net(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Performance: disable thumbnails on network folders")
    SESSION.backup([_EXPLORER_ADVANCED], "NoThumbnailNet")
    SESSION.set_dword(_EXPLORER_ADVANCED, "DisableThumbnailsOnNetworkFolders", 1)


def _remove_perf_disable_thumbnail_net(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXPLORER_ADVANCED, "DisableThumbnailsOnNetworkFolders")


def _detect_perf_disable_thumbnail_net() -> bool:
    return SESSION.read_dword(_EXPLORER_ADVANCED, "DisableThumbnailsOnNetworkFolders") == 1

TWEAKS += [
    TweakDef(
        id="perf-disable-font-smoothing",
        label="Disable Font Smoothing (ClearType)",
        category="Performance",
        apply_fn=_apply_perf_disable_font_smoothing,
        remove_fn=_remove_perf_disable_font_smoothing,
        detect_fn=_detect_perf_disable_font_smoothing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DESKTOP_KEY],
        description=(
            "Disables ClearType font smoothing to reduce GPU rendering overhead. "
            "Improves performance on low-end hardware. "
            "Default: Enabled. Recommended: Disabled for maximum performance."
        ),
        tags=["performance", "font", "cleartype", "rendering", "ui"],
        depends_on=[],
        side_effects="Text will appear less smooth/anti-aliased.",
    ),
    TweakDef(
        id="perf-always-unload-dll",
        label="Always Unload DLLs on Process Exit",
        category="Performance",
        apply_fn=_apply_perf_always_unload_dll,
        remove_fn=_remove_perf_always_unload_dll,
        detect_fn=_detect_perf_always_unload_dll,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_ADV],
        description=(
            "Forces Windows to immediately unload unused DLLs from memory when processes exit. "
            "Frees RAM faster and reduces memory fragmentation. "
            "Default: Not set. Recommended: Enabled."
        ),
        tags=["performance", "dll", "memory", "unload", "ram"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="perf-increase-icon-cache",
        label="Increase Explorer Icon Cache Size",
        category="Performance",
        apply_fn=_apply_perf_increase_icon_cache,
        remove_fn=_remove_perf_increase_icon_cache,
        detect_fn=_detect_perf_increase_icon_cache,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_ADV],
        description=(
            "Increases Explorer's icon cache to 4096 entries. "
            "Reduces icon reloading delays when switching between folders with many files. "
            "Default: 500. Recommended: 4096 for large libraries."
        ),
        tags=["performance", "explorer", "icon", "cache", "ui"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="perf-clear-recent-docs-exit",
        label="Clear Recent Documents on Logoff",
        category="Performance",
        apply_fn=_apply_perf_clear_recent_docs_exit,
        remove_fn=_remove_perf_clear_recent_docs_exit,
        detect_fn=_detect_perf_clear_recent_docs_exit,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_POLICIES_EXP_CU],
        description=(
            "Clears the recent documents list when the user logs off. "
            "Improves privacy and slightly speeds up logoff. "
            "Default: Not cleared. Recommended: Enabled for shared machines."
        ),
        tags=["performance", "privacy", "recent-docs", "logoff", "explorer"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="perf-disable-thumbnails-network",
        label="Disable Thumbnails on Network Folders",
        category="Performance",
        apply_fn=_apply_perf_disable_thumbnail_net,
        remove_fn=_remove_perf_disable_thumbnail_net,
        detect_fn=_detect_perf_disable_thumbnail_net,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_ADVANCED],
        description=(
            "Disables thumbnail generation for files on network shares. "
            "Eliminates Explorer hangs and delays when browsing slow network drives. "
            "Default: Enabled. Recommended: Disabled on slow networks."
        ),
        tags=["performance", "explorer", "thumbnail", "network", "speed"],
        depends_on=[],
        side_effects="Network folder files will display generic icons instead of thumbnails.",
    ),
]
