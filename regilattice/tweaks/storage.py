"""Storage tweaks -- hibernation, reserved storage, NTFS tuning, long paths."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# -- Registry key constants ---------------------------------------------------

_KEY_POWER = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"

_KEY_RESERVE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\ReserveManager"
)

_KEY_STORAGE_SENSE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\StorageSense\Parameters\StoragePolicy"
)

_KEY_EXPLORER_POLICY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Policies\Explorer"
)

_KEY_EXPLORER_ADV = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)

_KEY_FILESYSTEM = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"

_KEY_PREFETCH = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management\PrefetchParameters"
)

_KEY_MEMORY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management"
)


# -- 1. Disable Hibernation ---------------------------------------------------


def _apply_disable_hibernation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable hibernation file")
    SESSION.backup([_KEY_POWER], "Hibernation")
    SESSION.set_dword(_KEY_POWER, "HibernateEnabled", 0)
    SESSION.log("Completed disable-hibernation")


def _remove_disable_hibernation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_POWER], "Hibernation_Remove")
    SESSION.set_dword(_KEY_POWER, "HibernateEnabled", 1)
    SESSION.log("Restored hibernation to enabled")


def _detect_disable_hibernation() -> bool:
    return SESSION.read_dword(_KEY_POWER, "HibernateEnabled") == 0


# -- 2. Disable Reserved Storage ----------------------------------------------


def _apply_disable_reserved(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable reserved storage")
    SESSION.backup([_KEY_RESERVE], "ReservedStorage")
    SESSION.set_dword(_KEY_RESERVE, "ShippedWithReserves", 0)
    SESSION.log("Completed disable-reserved-storage")


def _remove_disable_reserved(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_RESERVE], "ReservedStorage_Remove")
    SESSION.set_dword(_KEY_RESERVE, "ShippedWithReserves", 1)
    SESSION.log("Restored reserved storage to enabled")


def _detect_disable_reserved() -> bool:
    return SESSION.read_dword(_KEY_RESERVE, "ShippedWithReserves") == 0


# -- 3. Disable Storage Sense -------------------------------------------------


def _apply_disable_storage_sense(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable Storage Sense auto-cleanup")
    SESSION.backup([_KEY_STORAGE_SENSE], "StorageSense")
    SESSION.set_dword(_KEY_STORAGE_SENSE, "01", 0)
    SESSION.log("Completed disable-storage-sense")


def _remove_disable_storage_sense(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_STORAGE_SENSE], "StorageSense_Remove")
    SESSION.set_dword(_KEY_STORAGE_SENSE, "01", 1)
    SESSION.log("Restored Storage Sense to enabled")


def _detect_disable_storage_sense() -> bool:
    return SESSION.read_dword(_KEY_STORAGE_SENSE, "01") == 0


# -- 4. Disable Recycle Bin Confirmation Dialog --------------------------------


def _apply_disable_recycle_confirm(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable Recycle Bin delete confirmation")
    SESSION.backup([_KEY_EXPLORER_POLICY], "RecycleConfirm")
    SESSION.set_dword(_KEY_EXPLORER_POLICY, "ConfirmFileDelete", 0)
    SESSION.log("Completed disable-recycle-confirm")


def _remove_disable_recycle_confirm(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_EXPLORER_POLICY], "RecycleConfirm_Remove")
    SESSION.set_dword(_KEY_EXPLORER_POLICY, "ConfirmFileDelete", 1)
    SESSION.log("Restored Recycle Bin confirmation dialog")


def _detect_disable_recycle_confirm() -> bool:
    return SESSION.read_dword(_KEY_EXPLORER_POLICY, "ConfirmFileDelete") == 0


# -- 5. Disable Thumbs.db in Network Folders ----------------------------------


def _apply_disable_thumbs_db(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable Thumbs.db on network folders")
    SESSION.backup([_KEY_EXPLORER_ADV], "ThumbsDb")
    SESSION.set_dword(_KEY_EXPLORER_ADV, "DisableThumbnailCache", 1)
    SESSION.log("Completed disable-thumbs-db")


def _remove_disable_thumbs_db(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_EXPLORER_ADV], "ThumbsDb_Remove")
    SESSION.delete_value(_KEY_EXPLORER_ADV, "DisableThumbnailCache")
    SESSION.log("Restored thumbnail caching to default")


def _detect_disable_thumbs_db() -> bool:
    return SESSION.read_dword(_KEY_EXPLORER_ADV, "DisableThumbnailCache") == 1


# -- 6. Enable Compact OS Compression Flag ------------------------------------


def _apply_compact_os(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: enable Compact OS compression flag")
    SESSION.backup([_KEY_FILESYSTEM], "CompactOs")
    SESSION.set_dword(_KEY_FILESYSTEM, "CompactOsEnabled", 1)
    SESSION.log("Completed compact-os (registry flag only; run 'compact /compactos:always' for full effect)")


def _remove_compact_os(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "CompactOs_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "CompactOsEnabled", 0)
    SESSION.log("Restored Compact OS flag to disabled")


def _detect_compact_os() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "CompactOsEnabled") == 1


# -- 7. Disable Prefetch / Superfetch -----------------------------------------


def _apply_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable Prefetch and Superfetch")
    SESSION.backup([_KEY_PREFETCH], "Prefetch")
    SESSION.set_dword(_KEY_PREFETCH, "EnablePrefetcher", 0)
    SESSION.set_dword(_KEY_PREFETCH, "EnableSuperfetch", 0)
    SESSION.log("Completed disable-prefetch")


def _remove_disable_prefetch(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_PREFETCH], "Prefetch_Remove")
    SESSION.set_dword(_KEY_PREFETCH, "EnablePrefetcher", 3)
    SESSION.set_dword(_KEY_PREFETCH, "EnableSuperfetch", 3)
    SESSION.log("Restored Prefetch and Superfetch to default (3)")


def _detect_disable_prefetch() -> bool:
    return (
        SESSION.read_dword(_KEY_PREFETCH, "EnablePrefetcher") == 0
        and SESSION.read_dword(_KEY_PREFETCH, "EnableSuperfetch") == 0
    )


# -- 8. Optimize NTFS Memory Usage --------------------------------------------


def _apply_optimize_ntfs_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: set NTFS memory usage to high")
    SESSION.backup([_KEY_FILESYSTEM], "NtfsMemory")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage", 2)
    SESSION.log("Completed optimize-ntfs-memory")


def _remove_optimize_ntfs_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "NtfsMemory_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage", 1)
    SESSION.log("Restored NTFS memory usage to default (1)")


def _detect_optimize_ntfs_memory() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage") == 2


# -- 9. Disable NTFS Last Access Timestamp ------------------------------------


def _apply_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable NTFS last access timestamp updates")
    SESSION.backup([_KEY_FILESYSTEM], "LastAccess")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsDisableLastAccessUpdate", 0x80000003)
    SESSION.log("Completed disable-last-access")


def _remove_disable_last_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "LastAccess_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsDisableLastAccessUpdate", 0x80000002)
    SESSION.log("Restored NTFS last access timestamp to system-managed default")


def _detect_disable_last_access() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsDisableLastAccessUpdate") == 0x80000003


# -- 10. Disable 8.3 Short Filename Creation ----------------------------------


def _apply_disable_8dot3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable 8.3 short filename creation")
    SESSION.backup([_KEY_FILESYSTEM], "8dot3")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsDisable8dot3NameCreation", 1)
    SESSION.log("Completed disable-8dot3")


def _remove_disable_8dot3(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "8dot3_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsDisable8dot3NameCreation", 0)
    SESSION.log("Restored 8.3 short filename creation to default (enabled)")


def _detect_disable_8dot3() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsDisable8dot3NameCreation") == 1


# -- 11. Enable Large System Cache --------------------------------------------


def _apply_large_system_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: enable large system cache")
    SESSION.backup([_KEY_MEMORY], "LargeSystemCache")
    SESSION.set_dword(_KEY_MEMORY, "LargeSystemCache", 1)
    SESSION.log("Completed large-system-cache")


def _remove_large_system_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_MEMORY], "LargeSystemCache_Remove")
    SESSION.set_dword(_KEY_MEMORY, "LargeSystemCache", 0)
    SESSION.log("Restored large system cache to default (0)")


def _detect_large_system_cache() -> bool:
    return SESSION.read_dword(_KEY_MEMORY, "LargeSystemCache") == 1


# -- 12. Enable Win32 Long Path Support ----------------------------------------


def _apply_enable_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: enable Win32 long path support")
    SESSION.backup([_KEY_FILESYSTEM], "LongPaths")
    SESSION.set_dword(_KEY_FILESYSTEM, "LongPathsEnabled", 1)
    SESSION.log("Completed enable-long-paths")


def _remove_enable_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "LongPaths_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "LongPathsEnabled", 0)
    SESSION.log("Restored long path support to disabled")


def _detect_enable_long_paths() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "LongPathsEnabled") == 1


# -- 13. Disable Boot Defragmentation -----------------------------------------

_KEY_DFRG = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"


def _apply_disable_defrag_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: disable boot defragmentation")
    SESSION.backup([_KEY_DFRG], "DefragBoot")
    SESSION.set_string(_KEY_DFRG, "Enable", "N")
    SESSION.log("Completed disable-defrag-boot")


def _remove_disable_defrag_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_DFRG], "DefragBoot_Remove")
    SESSION.set_string(_KEY_DFRG, "Enable", "Y")
    SESSION.log("Restored boot defragmentation to enabled")


def _detect_disable_defrag_boot() -> bool:
    return SESSION.read_string(_KEY_DFRG, "Enable") == "N"


# -- 14. Increase NTFS Paged Pool Memory ----------------------------------------


def _apply_increase_ntfs_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Storage: increase NTFS paged pool memory")
    SESSION.backup([_KEY_FILESYSTEM], "NtfsPagedPool")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage", 2)
    SESSION.log("Completed increase-ntfs-memory")


def _remove_increase_ntfs_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "NtfsPagedPool_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage", 1)
    SESSION.log("Restored NTFS memory usage to default (1)")


def _detect_increase_ntfs_memory() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage") == 2


# -- TWEAKS export -------------------------------------------------------------

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="storage-disable-hibernation",
        label="Disable Hibernation File",
        category="Storage",
        apply_fn=_apply_disable_hibernation,
        remove_fn=_remove_disable_hibernation,
        detect_fn=_detect_disable_hibernation,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_POWER],
        description=(
            "Disables the Windows hibernation file (hiberfil.sys) to reclaim disk space. "
            "The file can consume several GB. Sleep mode remains available. "
            "Default: enabled. Recommended: disabled on desktops and SSD-only machines."
        ),
        tags=["storage", "hibernation", "disk", "space"],
    ),
    TweakDef(
        id="storage-disable-reserved",
        label="Disable Reserved Storage",
        category="Storage",
        apply_fn=_apply_disable_reserved,
        remove_fn=_remove_disable_reserved,
        detect_fn=_detect_disable_reserved,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_RESERVE],
        description=(
            "Disables the 7 GB reserved storage partition that Windows keeps for updates and temp files. "
            "Frees disk space on small drives. Takes effect after the next feature update. "
            "Default: enabled. Recommended: disabled on space-constrained devices."
        ),
        tags=["storage", "reserved", "disk", "update"],
    ),
    TweakDef(
        id="storage-disable-storage-sense",
        label="Disable Storage Sense Auto-Cleanup",
        category="Storage",
        apply_fn=_apply_disable_storage_sense,
        remove_fn=_remove_disable_storage_sense,
        detect_fn=_detect_disable_storage_sense,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_STORAGE_SENSE],
        description=(
            "Disables Storage Sense, the automatic disk cleanup feature that deletes temp files "
            "and Recycle Bin content on a schedule. Prevents unintended file removal. "
            "Default: enabled. Recommended: disabled if you manage cleanup manually."
        ),
        tags=["storage", "cleanup", "storage-sense", "automatic"],
    ),
    TweakDef(
        id="storage-disable-recycle-confirm",
        label="Disable Recycle Bin Confirmation Dialog",
        category="Storage",
        apply_fn=_apply_disable_recycle_confirm,
        remove_fn=_remove_disable_recycle_confirm,
        detect_fn=_detect_disable_recycle_confirm,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_EXPLORER_POLICY],
        description=(
            "Removes the confirmation prompt when deleting files to the Recycle Bin. "
            "Files still go to the Recycle Bin and can be restored. "
            "Default: enabled. Recommended: disabled for faster workflow."
        ),
        tags=["storage", "recycle-bin", "confirmation", "explorer"],
    ),
    TweakDef(
        id="storage-disable-thumbs-db",
        label="Disable Thumbs.db on Network Folders",
        category="Storage",
        apply_fn=_apply_disable_thumbs_db,
        remove_fn=_remove_disable_thumbs_db,
        detect_fn=_detect_disable_thumbs_db,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_EXPLORER_ADV],
        description=(
            "Prevents Windows from creating hidden Thumbs.db thumbnail cache files in network folders. "
            "Avoids lock conflicts and clutter on shared drives. "
            "Default: enabled (Thumbs.db created). Recommended: disabled."
        ),
        tags=["storage", "thumbs", "network", "cache", "explorer"],
    ),
    TweakDef(
        id="storage-compact-os",
        label="Enable Compact OS Compression Flag",
        category="Storage",
        apply_fn=_apply_compact_os,
        remove_fn=_remove_compact_os,
        detect_fn=_detect_compact_os,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Sets the Compact OS registry flag to prefer OS file compression. "
            "Can save 1-2 GB on the system drive. For full effect run 'compact /compactos:always' "
            "from an elevated prompt. Default: disabled. Recommended: enabled on small SSDs."
        ),
        tags=["storage", "compact", "compression", "disk", "ssd"],
    ),
    TweakDef(
        id="storage-disable-prefetch",
        label="Disable Prefetch and Superfetch",
        category="Storage",
        apply_fn=_apply_disable_prefetch,
        remove_fn=_remove_disable_prefetch,
        detect_fn=_detect_disable_prefetch,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_PREFETCH],
        description=(
            "Disables the Windows Prefetch and Superfetch (SysMain) caching mechanisms. "
            "On SSD systems these provide negligible benefit and consume disk I/O. "
            "Default: enabled (3). Recommended: disabled on SSD-only machines."
        ),
        tags=["storage", "prefetch", "superfetch", "sysmain", "ssd"],
    ),
    TweakDef(
        id="storage-optimize-ntfs-memory",
        label="NTFS Memory Usage High",
        category="Storage",
        apply_fn=_apply_optimize_ntfs_memory,
        remove_fn=_remove_optimize_ntfs_memory,
        detect_fn=_detect_optimize_ntfs_memory,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Sets NtfsMemoryUsage to 2 (high), allowing NTFS to use more paged pool memory for caching. "
            "Improves file system performance on machines with ample RAM. "
            "Default: 1 (normal). Recommended: 2 on workstations with 16 GB+ RAM."
        ),
        tags=["storage", "ntfs", "memory", "performance", "filesystem"],
    ),
    TweakDef(
        id="storage-disable-last-access",
        label="Disable NTFS Last Access Timestamp",
        category="Storage",
        apply_fn=_apply_disable_last_access,
        remove_fn=_remove_disable_last_access,
        detect_fn=_detect_disable_last_access,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Disables updating the last access timestamp on NTFS files and directories. "
            "Reduces disk writes and improves I/O performance on busy volumes. "
            "Default: system-managed (0x80000002). Recommended: disabled (0x80000003) on SSDs."
        ),
        tags=["storage", "ntfs", "last-access", "timestamp", "performance"],
    ),
    TweakDef(
        id="storage-disable-8dot3",
        label="Disable 8.3 Short Filename Creation",
        category="Storage",
        apply_fn=_apply_disable_8dot3,
        remove_fn=_remove_disable_8dot3,
        detect_fn=_detect_disable_8dot3,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Disables automatic creation of legacy 8.3 short filenames on NTFS volumes. "
            "Improves directory enumeration speed on volumes with many files. "
            "Default: enabled (0). Recommended: disabled unless legacy 16-bit apps are needed."
        ),
        tags=["storage", "ntfs", "8dot3", "short-name", "performance"],
    ),
    TweakDef(
        id="storage-large-system-cache",
        label="Enable Large System Cache",
        category="Storage",
        apply_fn=_apply_large_system_cache,
        remove_fn=_remove_large_system_cache,
        detect_fn=_detect_large_system_cache,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_MEMORY],
        description=(
            "Tells Windows to favor the system file cache over application working sets. "
            "Beneficial for file-server workloads and large sequential reads. "
            "Default: disabled (0). Recommended: enabled on file servers or 16 GB+ workstations."
        ),
        tags=["storage", "cache", "memory", "file-server", "performance"],
    ),
    TweakDef(
        id="storage-enable-long-paths",
        label="Enable Win32 Long Path Support",
        category="Storage",
        apply_fn=_apply_enable_long_paths,
        remove_fn=_remove_enable_long_paths,
        detect_fn=_detect_enable_long_paths,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Enables Win32 long path support, removing the 260-character path length limit "
            "for applications that declare long-path awareness in their manifest. "
            "Default: disabled. Recommended: enabled for developers and deep directory trees."
        ),
        tags=["storage", "long-path", "260", "developer", "filesystem"],
    ),
    TweakDef(
        id="storage-disable-defrag-boot",
        label="Disable Boot Defragmentation",
        category="Storage",
        apply_fn=_apply_disable_defrag_boot,
        remove_fn=_remove_disable_defrag_boot,
        detect_fn=_detect_disable_defrag_boot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_DFRG],
        description=(
            "Disables automatic boot-time defragmentation of frequently used files. "
            "On SSD systems boot defrag provides no benefit and adds write wear. "
            "Default: enabled (Y). Recommended: disabled (N) on SSDs."
        ),
        tags=["storage", "defrag", "boot", "ssd", "performance"],
    ),
    TweakDef(
        id="storage-increase-ntfs-memory",
        label="Increase NTFS Paged Pool Memory",
        category="Storage",
        apply_fn=_apply_increase_ntfs_memory,
        remove_fn=_remove_increase_ntfs_memory,
        detect_fn=_detect_increase_ntfs_memory,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Increases NTFS paged pool memory allocation for improved file system "
            "caching and metadata performance. Best on systems with 16 GB+ RAM. "
            "Default: 1 (normal). Recommended: 2 (high)."
        ),
        tags=["storage", "ntfs", "memory", "paged-pool", "performance"],
    ),
]
