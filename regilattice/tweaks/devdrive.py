"""Dev Drive tweaks — Windows 11 Dev Drive and ReFS optimisations.

Covers: Dev Drive performance mode, Defender exclusions for dev volumes,
ReFS settings, package cache redirection, and build performance.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_REFS = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"
_DEFENDER_PERF = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection"
_DEFENDER_EXCL = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Defender\Exclusions\Processes"
_FS_FILTER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection"
_DEV_HOME = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\DevHome"
_NTFS_OPT = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"
_MEM_MGMT = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"
_VSM = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"


# ── Disable Last Access Time Update ─────────────────────────────────────────


def _apply_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable NTFS last access time update")
    SESSION.backup([_NTFS_OPT], "LastAccess")
    SESSION.set_dword(_NTFS_OPT, "NtfsDisableLastAccessUpdate", 0x80000003)


def _remove_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NTFS_OPT, "NtfsDisableLastAccessUpdate", 0x80000002)


def _detect_disable_last_access() -> bool:
    val = SESSION.read_dword(_NTFS_OPT, "NtfsDisableLastAccessUpdate")
    return val is not None and (val & 0x3) == 0x3


# ── Disable 8.3 Short Name Creation ─────────────────────────────────────────


def _apply_disable_8dot3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable 8.3 short name creation")
    SESSION.backup([_NTFS_OPT], "8dot3")
    SESSION.set_dword(_NTFS_OPT, "NtfsDisable8dot3NameCreation", 1)


def _remove_disable_8dot3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NTFS_OPT, "NtfsDisable8dot3NameCreation", 0)


def _detect_disable_8dot3() -> bool:
    return SESSION.read_dword(_NTFS_OPT, "NtfsDisable8dot3NameCreation") == 1


# ── Enable Long Paths ───────────────────────────────────────────────────────


def _apply_enable_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: enable Win32 long path support (>260 chars)")
    SESSION.backup([_NTFS_OPT], "LongPaths")
    SESSION.set_dword(_NTFS_OPT, "LongPathsEnabled", 1)


def _remove_enable_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NTFS_OPT, "LongPathsEnabled", 0)


def _detect_enable_long_paths() -> bool:
    return SESSION.read_dword(_NTFS_OPT, "LongPathsEnabled") == 1


# ── Disable Defender Real-Time for Build Processes ───────────────────────────


def _apply_disable_rt_build(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: exclude common build tools from real-time scanning")
    SESSION.backup([_DEFENDER_EXCL], "DevBuildExclusions")
    for proc in ("devenv.exe", "msbuild.exe", "cl.exe", "link.exe", "node.exe", "python.exe", "cargo.exe", "rustc.exe"):
        SESSION.set_dword(_DEFENDER_EXCL, proc, 0)


def _remove_disable_rt_build(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    for proc in ("devenv.exe", "msbuild.exe", "cl.exe", "link.exe", "node.exe", "python.exe", "cargo.exe", "rustc.exe"):
        SESSION.delete_value(_DEFENDER_EXCL, proc)


def _detect_disable_rt_build() -> bool:
    return SESSION.read_dword(_DEFENDER_EXCL, "devenv.exe") == 0


# ── Reduce Defender CPU During Scans ─────────────────────────────────────────


def _apply_scan_cpu_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: limit Defender scan CPU to 15%")
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"
    SESSION.backup([_key], "DevScanCPU")
    SESSION.set_dword(_key, "AvgCPULoadFactor", 15)


def _remove_scan_cpu_limit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"
    SESSION.set_dword(_key, "AvgCPULoadFactor", 50)


def _detect_scan_cpu_limit() -> bool:
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"
    val = SESSION.read_dword(_key, "AvgCPULoadFactor")
    return val is not None and val <= 15


# ── Disable Minifilter Attach on Dev Volumes ────────────────────────────────


def _apply_disable_filter_attach(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable anti-malware minifilter for performance mode")
    SESSION.backup([_FS_FILTER], "FilterAttach")
    SESSION.set_dword(_FS_FILTER, "DisableRealtimeMonitoring", 1)


def _remove_disable_filter_attach(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FS_FILTER, "DisableRealtimeMonitoring")


def _detect_disable_filter_attach() -> bool:
    return SESSION.read_dword(_FS_FILTER, "DisableRealtimeMonitoring") == 1


# ── Increase File System Memory Cache ───────────────────────────────────────


def _apply_large_fs_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: enable large file system cache")
    SESSION.backup([_MEM_MGMT], "LargeFSCache")
    SESSION.set_dword(_MEM_MGMT, "LargeSystemCache", 1)


def _remove_large_fs_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_MEM_MGMT, "LargeSystemCache", 0)


def _detect_large_fs_cache() -> bool:
    return SESSION.read_dword(_MEM_MGMT, "LargeSystemCache") == 1


# ── Disable File System Encryption Warning ──────────────────────────────────


def _apply_disable_efs_warning(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: suppress EFS encryption warning")
    SESSION.backup([_NTFS_OPT], "EFSWarning")
    SESSION.set_dword(_NTFS_OPT, "NtfsEncryptionService", 0)


def _remove_disable_efs_warning(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NTFS_OPT, "NtfsEncryptionService")


def _detect_disable_efs_warning() -> bool:
    return SESSION.read_dword(_NTFS_OPT, "NtfsEncryptionService") == 0


# ── Enable Paged Pool Optimisation ──────────────────────────────────────────


def _apply_paged_pool_opt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: optimise paged pool for large builds")
    SESSION.backup([_MEM_MGMT], "PagedPool")
    SESSION.set_dword(_MEM_MGMT, "PagedPoolSize", 0)


def _remove_paged_pool_opt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_MEM_MGMT, "PagedPoolSize")


def _detect_paged_pool_opt() -> bool:
    return SESSION.read_dword(_MEM_MGMT, "PagedPoolSize") == 0


# ── Disable Dev Home Telemetry ──────────────────────────────────────────────


def _apply_disable_devhome_telemetry(*, require_admin: bool = False) -> None:
    SESSION.log("Dev Drive: disable Dev Home telemetry")
    SESSION.backup([_DEV_HOME], "DevHomeTelemetry")
    SESSION.set_dword(_DEV_HOME, "DiagnosticsEnabled", 0)


def _remove_disable_devhome_telemetry(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_DEV_HOME, "DiagnosticsEnabled", 1)


def _detect_disable_devhome_telemetry() -> bool:
    return SESSION.read_dword(_DEV_HOME, "DiagnosticsEnabled") == 0


# ── Disable File System Compression ─────────────────────────────────────────


def _apply_disable_fs_compress(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable automatic NTFS compression")
    SESSION.backup([_NTFS_OPT], "FSCompress")
    SESSION.set_dword(_NTFS_OPT, "NtfsAllowExtendedCharacterIn8dot3Name", 0)


def _remove_disable_fs_compress(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_NTFS_OPT, "NtfsAllowExtendedCharacterIn8dot3Name")


def _detect_disable_fs_compress() -> bool:
    return SESSION.read_dword(_NTFS_OPT, "NtfsAllowExtendedCharacterIn8dot3Name") == 0


# ── Disable VBS for Dev Performance ─────────────────────────────────────────


def _apply_disable_vbs_dev(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable VBS for faster compilation")
    SESSION.backup([_VSM], "VBSDev")
    SESSION.set_dword(_VSM, "EnableVirtualizationBasedSecurity", 0)


def _remove_disable_vbs_dev(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_VSM, "EnableVirtualizationBasedSecurity", 1)


def _detect_disable_vbs_dev() -> bool:
    return SESSION.read_dword(_VSM, "EnableVirtualizationBasedSecurity") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="dev-disable-last-access",
        label="Disable Last Access Time Update",
        category="Dev Drive",
        apply_fn=_apply_disable_last_access,
        remove_fn=_remove_disable_last_access,
        detect_fn=_detect_disable_last_access,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NTFS_OPT],
        description="Disables NTFS last access timestamp updates. Reduces I/O for build-heavy workflows. Default: Enabled (volume managed).",
        tags=["dev-drive", "ntfs", "performance", "build"],
    ),
    TweakDef(
        id="dev-disable-8dot3",
        label="Disable 8.3 Short Name Creation",
        category="Dev Drive",
        apply_fn=_apply_disable_8dot3,
        remove_fn=_remove_disable_8dot3,
        detect_fn=_detect_disable_8dot3,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NTFS_OPT],
        description="Disables legacy 8.3 short filename creation. Speeds up directory operations for large repos. Default: Enabled.",
        tags=["dev-drive", "ntfs", "8dot3", "performance"],
    ),
    TweakDef(
        id="dev-win32-long-paths",
        label="Enable Win32 Long Paths (>260 chars)",
        category="Dev Drive",
        apply_fn=_apply_enable_long_paths,
        remove_fn=_remove_enable_long_paths,
        detect_fn=_detect_enable_long_paths,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NTFS_OPT],
        description="Enables paths longer than 260 characters in Win32 applications. Essential for deep node_modules and cargo trees. Recommended.",
        tags=["dev-drive", "long-paths", "node", "development"],
    ),
    TweakDef(
        id="dev-exclude-build-tools",
        label="Exclude Build Tools from Defender",
        category="Dev Drive",
        apply_fn=_apply_disable_rt_build,
        remove_fn=_remove_disable_rt_build,
        detect_fn=_detect_disable_rt_build,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DEFENDER_EXCL],
        description=(
            "Excludes common build processes (devenv, msbuild, node, python, cargo, rustc) from "
            "Defender real-time scanning. Can improve build times 10-30%. Recommended: Enabled for dev machines."
        ),
        tags=["dev-drive", "defender", "build", "performance", "exclusion"],
    ),
    TweakDef(
        id="dev-scan-cpu-limit",
        label="Limit Defender Scan CPU to 15%",
        category="Dev Drive",
        apply_fn=_apply_scan_cpu_limit,
        remove_fn=_remove_scan_cpu_limit,
        detect_fn=_detect_scan_cpu_limit,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
        description="Reduces Defender background scan CPU usage to 15% (default: 50%). Prevents compilation stalls during scheduled scans.",
        tags=["dev-drive", "defender", "cpu", "scan"],
    ),
    TweakDef(
        id="dev-disable-filter-attach",
        label="Disable Anti-Malware Minifilter",
        category="Dev Drive",
        apply_fn=_apply_disable_filter_attach,
        remove_fn=_remove_disable_filter_attach,
        detect_fn=_detect_disable_filter_attach,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FS_FILTER],
        description=(
            "Disables real-time anti-malware minifilter driver (Dev Drive performance mode). "
            "Fastest I/O but reduces security. Only use on trusted dev volumes."
        ),
        tags=["dev-drive", "minifilter", "performance", "security"],
    ),
    TweakDef(
        id="dev-large-fs-cache",
        label="Enable Large File System Cache",
        category="Dev Drive",
        apply_fn=_apply_large_fs_cache,
        remove_fn=_remove_large_fs_cache,
        detect_fn=_detect_large_fs_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MEM_MGMT],
        description="Enables large system cache for file system operations. Improves build I/O at the cost of more RAM usage.",
        tags=["dev-drive", "cache", "memory", "performance"],
    ),
    TweakDef(
        id="dev-disable-efs-warning",
        label="Suppress EFS Encryption Warning",
        category="Dev Drive",
        apply_fn=_apply_disable_efs_warning,
        remove_fn=_remove_disable_efs_warning,
        detect_fn=_detect_disable_efs_warning,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NTFS_OPT],
        description="Suppresses the EFS encryption service prompt when Dev Drive volumes are created without encryption.",
        tags=["dev-drive", "efs", "encryption"],
    ),
    TweakDef(
        id="dev-paged-pool-opt",
        label="Optimise Paged Pool for Builds",
        category="Dev Drive",
        apply_fn=_apply_paged_pool_opt,
        remove_fn=_remove_paged_pool_opt,
        detect_fn=_detect_paged_pool_opt,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MEM_MGMT],
        description="Lets Windows auto-size paged pool (value 0 = system managed). Optimal for machines with 16+ GB RAM running large builds.",
        tags=["dev-drive", "paged-pool", "memory", "performance"],
    ),
    TweakDef(
        id="dev-disable-devhome-telemetry",
        label="Disable Dev Home Telemetry",
        category="Dev Drive",
        apply_fn=_apply_disable_devhome_telemetry,
        remove_fn=_remove_disable_devhome_telemetry,
        detect_fn=_detect_disable_devhome_telemetry,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DEV_HOME],
        description="Disables diagnostic data collection by the Windows Dev Home app. Default: Enabled.",
        tags=["dev-drive", "dev-home", "telemetry", "privacy"],
    ),
    TweakDef(
        id="dev-disable-fs-compress",
        label="Disable NTFS Extended Char in 8.3",
        category="Dev Drive",
        apply_fn=_apply_disable_fs_compress,
        remove_fn=_remove_disable_fs_compress,
        detect_fn=_detect_disable_fs_compress,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NTFS_OPT],
        description="Disables extended character support in 8.3 filenames. Reduces file system overhead for dev volumes.",
        tags=["dev-drive", "ntfs", "8dot3", "performance"],
    ),
    TweakDef(
        id="dev-disable-vbs",
        label="Disable VBS for Dev Performance",
        category="Dev Drive",
        apply_fn=_apply_disable_vbs_dev,
        remove_fn=_remove_disable_vbs_dev,
        detect_fn=_detect_disable_vbs_dev,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VSM],
        description=(
            "Disables Virtualization Based Security. Can improve compilation and "
            "linking speed by 5-15% but reduces security. Only for dedicated dev machines."
        ),
        tags=["dev-drive", "vbs", "performance", "security"],
    ),
]


# ── Disable Memory Compression ───────────────────────────────────────────────

_SYS_MEM = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"


def _apply_disable_memory_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable memory compression")
    SESSION.backup([_SYS_MEM], "MemoryCompression")
    SESSION.set_dword(_SYS_MEM, "DisablePagingExecutive", 1)


def _remove_disable_memory_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SYS_MEM, "DisablePagingExecutive", 0)


def _detect_disable_memory_compression() -> bool:
    return SESSION.read_dword(_SYS_MEM, "DisablePagingExecutive") == 1


# ── Enable Dev Drive Host Caching ─────────────────────────────────────────────

_STOR_PORT = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device"


def _apply_enable_host_caching(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: enable disk host cache write buffering")
    SESSION.backup([_STOR_PORT], "HostCaching")
    SESSION.set_dword(_STOR_PORT, "FirstBurstSize", 0x80000)  # 512 KB burst


def _remove_enable_host_caching(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_STOR_PORT, "FirstBurstSize")


def _detect_enable_host_caching() -> bool:
    return SESSION.read_dword(_STOR_PORT, "FirstBurstSize") == 0x80000


# ── Disable Windows File Indexing on Dev Volumes ─────────────────────────────

_SEARCH_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"


def _apply_disable_search_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable Windows Search / indexer service")
    SESSION.backup([_SEARCH_SVC], "SearchSvc")
    SESSION.set_dword(_SEARCH_SVC, "Start", 4)  # 4 = disabled


def _remove_disable_search_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_SVC, "Start", 2)  # 2 = automatic


def _detect_disable_search_svc() -> bool:
    return SESSION.read_dword(_SEARCH_SVC, "Start") == 4


# ── Disable Prefetch for SSD Dev Machines ────────────────────────────────────

_PREFETCH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"


def _apply_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable Prefetch (optimal for SSDs with build workloads)")
    SESSION.backup([_PREFETCH], "Prefetch")
    SESSION.set_dword(_PREFETCH, "EnablePrefetcher", 0)
    SESSION.set_dword(_PREFETCH, "EnableSuperfetch", 0)


def _remove_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_PREFETCH, "EnablePrefetcher", 3)
    SESSION.set_dword(_PREFETCH, "EnableSuperfetch", 3)


def _detect_disable_prefetch() -> bool:
    return SESSION.read_dword(_PREFETCH, "EnablePrefetcher") == 0


# ── Enable NTFS Write Caching ─────────────────────────────────────────────────

_DISK_CACHE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"


def _apply_enable_ntfs_write_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: enable NTFS write caching")
    SESSION.backup([_DISK_CACHE], "NtfsWriteCache")
    SESSION.set_dword(_DISK_CACHE, "IoPageLockLimit", 0x2000000)  # 32 MB I/O page lock


def _remove_enable_ntfs_write_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DISK_CACHE, "IoPageLockLimit")


def _detect_enable_ntfs_write_cache() -> bool:
    return SESSION.read_dword(_DISK_CACHE, "IoPageLockLimit") == 0x2000000


# ── Set High Performance Power Plan for Dev ───────────────────────────────────

_POWER_THROTTLE = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"


def _apply_disable_power_throttle_dev(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Dev Drive: disable power throttling for all processes")
    SESSION.backup([_POWER_THROTTLE], "PowerThrottleDev")
    SESSION.set_dword(_POWER_THROTTLE, "PowerThrottlingOff", 1)


def _remove_disable_power_throttle_dev(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_POWER_THROTTLE, "PowerThrottlingOff", 0)


def _detect_disable_power_throttle_dev() -> bool:
    return SESSION.read_dword(_POWER_THROTTLE, "PowerThrottlingOff") == 1


TWEAKS += [
    TweakDef(
        id="dev-disable-memory-compression",
        label="Disable Paging Executive (Keep Code in RAM)",
        category="Dev Drive",
        apply_fn=_apply_disable_memory_compression,
        remove_fn=_remove_disable_memory_compression,
        detect_fn=_detect_disable_memory_compression,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SYS_MEM],
        description=(
            "Prevents Windows from paging out kernel and driver code to disk. "
            "Improves compilation speed on systems with ≥16 GB RAM. Default: Disabled (0). Recommended: Enabled for dev."
        ),
        tags=["dev-drive", "memory", "paging", "performance"],
    ),
    TweakDef(
        id="dev-enable-host-cache",
        label="Enable SSD Write Buffer (StorAHCI)",
        category="Dev Drive",
        apply_fn=_apply_enable_host_caching,
        remove_fn=_remove_enable_host_caching,
        detect_fn=_detect_enable_host_caching,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_STOR_PORT],
        description=(
            "Configures StorAHCI burst size for improved sequential write throughput on AHCI SSDs. "
            "Can improve incremental build I/O performance."
        ),
        tags=["dev-drive", "ssd", "ahci", "cache", "performance"],
    ),
    TweakDef(
        id="dev-disable-search-svc",
        label="Disable Windows Search Indexer",
        category="Dev Drive",
        apply_fn=_apply_disable_search_svc,
        remove_fn=_remove_disable_search_svc,
        detect_fn=_detect_disable_search_svc,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SEARCH_SVC],
        description=(
            "Stops the Windows Search service from indexing files. "
            "Reduces I/O contention during builds in large repositories. "
            "Default: Automatic. Recommended: Disabled on build servers."
        ),
        tags=["dev-drive", "search", "indexing", "performance", "service"],
    ),
    TweakDef(
        id="dev-disable-prefetch",
        label="Disable Prefetch / Superfetch (SSD)",
        category="Dev Drive",
        apply_fn=_apply_disable_prefetch,
        remove_fn=_remove_disable_prefetch,
        detect_fn=_detect_disable_prefetch,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PREFETCH],
        description=(
            "Disables Prefetch and Superfetch. Not needed on SSDs and can generate extra I/O during builds. "
            "Default: Enabled (3). Recommended: Disabled for SSD-based dev VMs."
        ),
        tags=["dev-drive", "prefetch", "superfetch", "ssd", "performance"],
    ),
    TweakDef(
        id="dev-ntfs-write-cache",
        label="Expand NTFS I/O Page Lock (32 MB)",
        category="Dev Drive",
        apply_fn=_apply_enable_ntfs_write_cache,
        remove_fn=_remove_enable_ntfs_write_cache,
        detect_fn=_detect_enable_ntfs_write_cache,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DISK_CACHE],
        description=(
            "Sets IoPageLockLimit to 32 MB for improved file I/O throughput during parallel builds. "
            "Default: System managed. Recommended: 32 MB on systems with ≥8 GB RAM."
        ),
        tags=["dev-drive", "ntfs", "io", "cache", "performance"],
    ),
    TweakDef(
        id="dev-disable-power-throttle",
        label="Disable Power Throttling (All Processes)",
        category="Dev Drive",
        apply_fn=_apply_disable_power_throttle_dev,
        remove_fn=_remove_disable_power_throttle_dev,
        detect_fn=_detect_disable_power_throttle_dev,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_POWER_THROTTLE],
        description=(
            "Disables Windows 10/11 power throttling for all processes globally. "
            "Ensures build tools always run at full CPU frequency. "
            "Default: Enabled. Recommended: Disabled for desktop dev machines."
        ),
        tags=["dev-drive", "power", "throttle", "performance", "cpu"],
    ),
]
