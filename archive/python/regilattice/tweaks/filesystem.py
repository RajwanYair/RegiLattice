"""File System tweaks -- NTFS optimization, indexing, IRP tuning, case sensitivity."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# -- Registry key constants ---------------------------------------------------

_KEY_EFS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EFS"

_KEY_LANMAN = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"

_KEY_MSRDC = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSRDC\Parameters"

_KEY_DEDUP = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dedup\Parameters"

_KEY_WSEARCH = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"

_KEY_FILESYSTEM = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"

_KEY_SESSION_MGR = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"

_KEY_SEARCH_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"


# -- 1. Disable EFS Encryption Warning ----------------------------------------


def _apply_disable_encryption_warning(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable EFS encryption warning")
    SESSION.backup([_KEY_EFS], "EfsEncryptionWarning")
    SESSION.set_dword(_KEY_EFS, "EfsConfiguration", 1)
    SESSION.log("Completed fs-disable-encryption-warning")


def _remove_disable_encryption_warning(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_EFS], "EfsEncryptionWarning_Remove")
    SESSION.set_dword(_KEY_EFS, "EfsConfiguration", 0)
    SESSION.log("Restored EFS encryption warning to default")


def _detect_disable_encryption_warning() -> bool:
    return SESSION.read_dword(_KEY_EFS, "EfsConfiguration") == 1


# -- 2. Increase IRP Stack Size -----------------------------------------------


def _apply_increase_irp_stack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: increase IRP stack size for network file sharing")
    SESSION.backup([_KEY_LANMAN], "IRPStackSize")
    SESSION.set_dword(_KEY_LANMAN, "IRPStackSize", 50)
    SESSION.log("Completed fs-increase-irp-stack")


def _remove_increase_irp_stack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_LANMAN], "IRPStackSize_Remove")
    SESSION.set_dword(_KEY_LANMAN, "IRPStackSize", 15)
    SESSION.log("Restored IRP stack size to default (15)")


def _detect_increase_irp_stack() -> bool:
    return SESSION.read_dword(_KEY_LANMAN, "IRPStackSize") == 50


# -- 3. Disable Remote Differential Compression -------------------------------


def _apply_disable_remote_diff_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable Remote Differential Compression")
    SESSION.backup([_KEY_MSRDC], "RemoteDiffCompression")
    SESSION.set_dword(_KEY_MSRDC, "DisableMSDC", 1)
    SESSION.log("Completed fs-disable-remote-diff-compression")


def _remove_disable_remote_diff_compression(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_MSRDC], "RemoteDiffCompression_Remove")
    SESSION.set_dword(_KEY_MSRDC, "DisableMSDC", 0)
    SESSION.log("Restored Remote Differential Compression to enabled")


def _detect_disable_remote_diff_compression() -> bool:
    return SESSION.read_dword(_KEY_MSRDC, "DisableMSDC") == 1


# -- 4. Enable Higher Dedup Memory Usage --------------------------------------


def _apply_enable_dedup_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: set higher dedup memory usage")
    SESSION.backup([_KEY_DEDUP], "DedupMemory")
    SESSION.set_dword(_KEY_DEDUP, "MaxMemory", 2048)
    SESSION.log("Completed fs-enable-dedup-memory")


def _remove_enable_dedup_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_DEDUP], "DedupMemory_Remove")
    SESSION.delete_value(_KEY_DEDUP, "MaxMemory")
    SESSION.log("Removed MaxMemory dedup override (restored default)")


def _detect_enable_dedup_memory() -> bool:
    return SESSION.read_dword(_KEY_DEDUP, "MaxMemory") == 2048


# -- 5. Disable Content Indexing -----------------------------------------------


def _apply_disable_content_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable Windows Search content indexing service")
    SESSION.backup([_KEY_WSEARCH], "ContentIndexing")
    SESSION.set_dword(_KEY_WSEARCH, "Start", 4)
    SESSION.log("Completed fs-disable-content-indexing")


def _remove_disable_content_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_WSEARCH], "ContentIndexing_Remove")
    SESSION.set_dword(_KEY_WSEARCH, "Start", 2)
    SESSION.log("Restored Windows Search service to automatic start")


def _detect_disable_content_indexing() -> bool:
    return SESSION.read_dword(_KEY_WSEARCH, "Start") == 4


# -- 6. Enable Per-Directory Case Sensitivity ---------------------------------


def _apply_enable_case_sensitive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: enable per-directory case sensitivity (global flag)")
    SESSION.backup([_KEY_FILESYSTEM], "CaseSensitivity")
    SESSION.set_dword(_KEY_FILESYSTEM, "ObCaseInsensitive", 0)
    SESSION.log("Completed fs-enable-case-sensitive")


def _remove_enable_case_sensitive(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "CaseSensitivity_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "ObCaseInsensitive", 1)
    SESSION.log("Restored case-insensitive behavior (default)")


def _detect_enable_case_sensitive() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "ObCaseInsensitive") == 0


# -- 7. Disable 8.3 Extended Character Name Generation ------------------------


def _apply_disable_name_generation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable extended character 8.3 name generation")
    SESSION.backup([_KEY_FILESYSTEM], "NameGeneration")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsAllowExtendedCharacterIn8dot3Name", 0)
    SESSION.log("Completed fs-disable-name-generation")


def _remove_disable_name_generation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "NameGeneration_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsAllowExtendedCharacterIn8dot3Name", 1)
    SESSION.log("Restored extended character 8.3 name generation to enabled")


def _detect_disable_name_generation() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsAllowExtendedCharacterIn8dot3Name") == 0


# -- 8. Increase NTFS MFT Zone Reservation ------------------------------------


def _apply_increase_mft_zone(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: increase NTFS MFT zone reservation")
    SESSION.backup([_KEY_FILESYSTEM], "MftZone")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsMftZoneReservation", 2)
    SESSION.log("Completed fs-increase-mft-zone")


def _remove_increase_mft_zone(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "MftZone_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsMftZoneReservation", 1)
    SESSION.log("Restored NTFS MFT zone reservation to default (1)")


def _detect_increase_mft_zone() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsMftZoneReservation") == 2


# -- 9. Disable Paging File Encryption ----------------------------------------


def _apply_disable_encrypt_paging(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable NTFS paging file encryption")
    SESSION.backup([_KEY_FILESYSTEM], "EncryptPaging")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsEncryptPagingFile", 0)
    SESSION.log("Completed fs-disable-encrypt-paging")


def _remove_disable_encrypt_paging(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "EncryptPaging_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsEncryptPagingFile", 1)
    SESSION.log("Restored NTFS paging file encryption to enabled")


def _detect_disable_encrypt_paging() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsEncryptPagingFile") == 0


# -- 10. Disable DOS Device Mapping -------------------------------------------


def _apply_disable_dos_devices(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable DOS device mapping protection")
    SESSION.backup([_KEY_SESSION_MGR], "DosDeviceMapping")
    SESSION.set_dword(_KEY_SESSION_MGR, "ProtectionMode", 0)
    SESSION.log("Completed fs-disable-dos-devices")


def _remove_disable_dos_devices(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_SESSION_MGR], "DosDeviceMapping_Remove")
    SESSION.set_dword(_KEY_SESSION_MGR, "ProtectionMode", 1)
    SESSION.log("Restored DOS device mapping protection to enabled")


def _detect_disable_dos_devices() -> bool:
    return SESSION.read_dword(_KEY_SESSION_MGR, "ProtectionMode") == 0


# -- 11. Set Additional Critical Disk Allocation Margin -----------------------


def _apply_set_additional_del_margin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: set additional critical disk allocation margin")
    SESSION.backup([_KEY_FILESYSTEM], "AdditionalDelMargin")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsAdditionallyReservedBytes", 1048576)
    SESSION.log("Completed fs-set-additional-del-margin")


def _remove_set_additional_del_margin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "AdditionalDelMargin_Remove")
    SESSION.delete_value(_KEY_FILESYSTEM, "NtfsAdditionallyReservedBytes")
    SESSION.log("Removed NtfsAdditionallyReservedBytes (restored default)")


def _detect_set_additional_del_margin() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsAdditionallyReservedBytes") == 1048576


# -- 12. Disable Azure AD Cloud Content Indexing -------------------------------


def _apply_disable_azure_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable Azure AD cloud content indexing")
    SESSION.backup([_KEY_SEARCH_POLICY], "AzureIndexing")
    SESSION.set_dword(_KEY_SEARCH_POLICY, "AllowCloudSearch", 0)
    SESSION.log("Completed fs-disable-azure-indexing")


def _remove_disable_azure_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_SEARCH_POLICY], "AzureIndexing_Remove")
    SESSION.delete_value(_KEY_SEARCH_POLICY, "AllowCloudSearch")
    SESSION.log("Removed AllowCloudSearch policy (restored default)")


def _detect_disable_azure_indexing() -> bool:
    return SESSION.read_dword(_KEY_SEARCH_POLICY, "AllowCloudSearch") == 0


# -- TWEAKS export ------------------------------------------------------------

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="fs-disable-encryption-warning",
        label="Disable EFS Encryption Warning",
        category="File System",
        apply_fn=_apply_disable_encryption_warning,
        remove_fn=_remove_disable_encryption_warning,
        detect_fn=_detect_disable_encryption_warning,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_EFS],
        description=(
            "Disables the Encrypting File System (EFS) configuration warning dialog. "
            "Prevents prompts when EFS is not configured or not in use. "
            "Default: 0 (warning enabled). Recommended: disabled on machines not using EFS."
        ),
        tags=["filesystem", "efs", "encryption", "warning", "ntfs"],
    ),
    TweakDef(
        id="fs-increase-irp-stack",
        label="Increase IRP Stack Size",
        category="File System",
        apply_fn=_apply_increase_irp_stack,
        remove_fn=_remove_increase_irp_stack,
        detect_fn=_detect_increase_irp_stack,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_LANMAN],
        description=(
            "Increases the I/O Request Packet (IRP) stack size for the LanmanServer service. "
            "Improves reliability and throughput for network file sharing with many concurrent connections. "
            "Default: 15. Recommended: 50 for file servers or heavy SMB workloads."
        ),
        tags=["filesystem", "irp", "network", "smb", "file-sharing", "lanman"],
    ),
    TweakDef(
        id="fs-disable-remote-diff-compression",
        label="Disable Remote Differential Compression",
        category="File System",
        apply_fn=_apply_disable_remote_diff_compression,
        remove_fn=_remove_disable_remote_diff_compression,
        detect_fn=_detect_disable_remote_diff_compression,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_MSRDC],
        description=(
            "Disables the Remote Differential Compression (MSRDC) feature used during remote file sync. "
            "Reduces CPU overhead when RDC is unnecessary in local or fast-network environments. "
            "Default: 0 (enabled). Recommended: disabled on fast LANs or when RDC is not needed."
        ),
        tags=["filesystem", "rdc", "remote", "compression", "sync", "network"],
    ),
    TweakDef(
        id="fs-enable-dedup-memory",
        label="Set Higher Dedup Memory Usage",
        category="File System",
        apply_fn=_apply_enable_dedup_memory,
        remove_fn=_remove_enable_dedup_memory,
        detect_fn=_detect_enable_dedup_memory,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_DEDUP],
        description=(
            "Sets the data deduplication service MaxMemory to 2048 MB for faster processing. "
            "Allows the dedup engine to use more RAM during optimization passes. "
            "Default: not set (engine default). Recommended: 2048 on servers with 16 GB+ RAM."
        ),
        tags=["filesystem", "dedup", "deduplication", "memory", "server"],
    ),
    TweakDef(
        id="fs-disable-content-indexing",
        label="Disable Content Indexing Service",
        category="File System",
        apply_fn=_apply_disable_content_indexing,
        remove_fn=_remove_disable_content_indexing,
        detect_fn=_detect_disable_content_indexing,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_WSEARCH],
        description=(
            "Disables the Windows Search (WSearch) content indexing service via registry. "
            "Eliminates background I/O caused by index building on large volumes. "
            "Default: 2 (automatic start). Recommended: disabled (4) on servers or SSD-only machines."
        ),
        tags=["filesystem", "indexing", "wsearch", "search", "io", "performance"],
    ),
    TweakDef(
        id="fs-enable-case-sensitive",
        label="Enable Per-Directory Case Sensitivity",
        category="File System",
        apply_fn=_apply_enable_case_sensitive,
        remove_fn=_remove_enable_case_sensitive,
        detect_fn=_detect_enable_case_sensitive,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Enables the global flag allowing per-directory NTFS case sensitivity. "
            "Required for WSL interop and POSIX-compliant directory behavior on Windows. "
            "Default: 1 (case-insensitive). Recommended: 0 (case-sensitive) for WSL/developer use."
        ),
        tags=["filesystem", "case-sensitive", "ntfs", "wsl", "posix", "developer"],
    ),
    TweakDef(
        id="fs-disable-name-generation",
        label="Disable Extended Character 8.3 Name Generation",
        category="File System",
        apply_fn=_apply_disable_name_generation,
        remove_fn=_remove_disable_name_generation,
        detect_fn=_detect_disable_name_generation,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Disables automatic 8.3 short name generation for extended (Unicode) characters. "
            "Reduces NTFS overhead when creating files with non-ASCII names. "
            "Default: 1 (enabled). Recommended: disabled unless legacy 16-bit app compatibility is needed."
        ),
        tags=["filesystem", "8dot3", "short-name", "unicode", "ntfs", "performance"],
    ),
    TweakDef(
        id="fs-increase-mft-zone",
        label="Increase NTFS MFT Zone Reservation",
        category="File System",
        apply_fn=_apply_increase_mft_zone,
        remove_fn=_remove_increase_mft_zone,
        detect_fn=_detect_increase_mft_zone,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Increases the NTFS Master File Table zone reservation from level 1 to level 2. "
            "Reserves more contiguous disk space for MFT growth, reducing fragmentation on busy volumes. "
            "Default: 1. Recommended: 2 for volumes with many small files."
        ),
        tags=["filesystem", "mft", "ntfs", "fragmentation", "reservation"],
    ),
    TweakDef(
        id="fs-disable-encrypt-paging",
        label="Disable Paging File Encryption",
        category="File System",
        apply_fn=_apply_disable_encrypt_paging,
        remove_fn=_remove_disable_encrypt_paging,
        detect_fn=_detect_disable_encrypt_paging,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Disables NTFS paging file encryption to reduce CPU overhead during memory paging. "
            "Improves paging performance at the cost of not encrypting swapped data at rest. "
            "Default: 0 (disabled). Recommended: keep disabled unless full-disk encryption is required."
        ),
        tags=["filesystem", "paging", "encryption", "swap", "performance", "ntfs"],
    ),
    TweakDef(
        id="fs-disable-dos-devices",
        label="Disable DOS Device Mapping Protection",
        category="File System",
        apply_fn=_apply_disable_dos_devices,
        remove_fn=_remove_disable_dos_devices,
        detect_fn=_detect_disable_dos_devices,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_SESSION_MGR],
        description=(
            "Disables the Session Manager DOS device mapping protection mode. "
            "Allows legacy applications to create global DOS device names without restrictions. "
            "Default: 1 (protection enabled). Recommended: disabled only for legacy app compatibility."
        ),
        tags=["filesystem", "dos", "device", "session-manager", "legacy", "compatibility"],
    ),
    TweakDef(
        id="fs-set-additional-del-margin",
        label="Set Critical Disk Allocation Margin",
        category="File System",
        apply_fn=_apply_set_additional_del_margin,
        remove_fn=_remove_set_additional_del_margin,
        detect_fn=_detect_set_additional_del_margin,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Sets an additional 1 MB reserved byte margin for critical NTFS disk allocations. "
            "Prevents low-disk-space write failures for system-critical operations. "
            "Default: not set (0). Recommended: 1048576 (1 MB) on volumes that approach capacity."
        ),
        tags=["filesystem", "ntfs", "disk-space", "reserved", "allocation", "margin"],
    ),
    TweakDef(
        id="fs-disable-azure-indexing",
        label="Disable Azure AD Cloud Content Indexing",
        category="File System",
        apply_fn=_apply_disable_azure_indexing,
        remove_fn=_remove_disable_azure_indexing,
        detect_fn=_detect_disable_azure_indexing,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_SEARCH_POLICY],
        description=(
            "Disables Azure AD / Entra ID cloud content indexing via Windows Search policy. "
            "Prevents cloud-sourced content from being indexed locally, reducing network and I/O usage. "
            "Default: not set (cloud search allowed). Recommended: disabled for privacy-focused setups."
        ),
        tags=["filesystem", "azure", "cloud", "indexing", "search", "privacy"],
    ),
]


# -- Disable 8.3 short filename creation ------------------------------------


def _apply_disable_8dot3_names(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable 8.3 short file name creation")
    SESSION.backup([_KEY_FILESYSTEM], "Disable8dot3")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsDisable8dot3NameCreation", 1)


def _remove_disable_8dot3_names(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "Disable8dot3_Remove")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsDisable8dot3NameCreation", 0)


def _detect_disable_8dot3_names() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsDisable8dot3NameCreation") == 1


# -- Disable last access time updates ----------------------------------------


def _apply_disable_last_access_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable NTFS last access time updates")
    SESSION.backup([_KEY_FILESYSTEM], "DisableLastAccess")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsDisableLastAccessUpdate", 0x80000003)


def _remove_disable_last_access_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "DisableLastAccess_Remove")
    SESSION.delete_value(_KEY_FILESYSTEM, "NtfsDisableLastAccessUpdate")


def _detect_disable_last_access_update() -> bool:
    val = SESSION.read_dword(_KEY_FILESYSTEM, "NtfsDisableLastAccessUpdate")
    return val in (1, 0x80000003)


TWEAKS += [
    TweakDef(
        id="fs-disable-8dot3-names",
        label="Disable 8.3 Short Filename Creation",
        category="File System",
        apply_fn=_apply_disable_8dot3_names,
        remove_fn=_remove_disable_8dot3_names,
        detect_fn=_detect_disable_8dot3_names,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Disables automatic 8.3 (DOS-compatible) short filename generation on NTFS. "
            "Reduces directory enumeration overhead and speeds up file creation. "
            "Default: 0 (enabled). Recommended: 1 (disabled)."
        ),
        tags=["filesystem", "ntfs", "8dot3", "performance", "filenames"],
    ),
    TweakDef(
        id="fs-disable-last-access-update",
        label="Disable NTFS Last Access Time Updates",
        category="File System",
        apply_fn=_apply_disable_last_access_update,
        remove_fn=_remove_disable_last_access_update,
        detect_fn=_detect_disable_last_access_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Disables updating the last-access timestamp on every file read. "
            "Significant NTFS performance improvement for I/O-heavy workloads. "
            "Default: 0 (system managed). Recommended: 0x80000003 (user-disabled, system-managed)."
        ),
        tags=["filesystem", "ntfs", "last-access", "performance", "io"],
    ),
]


# -- Increase NTFS memory usage -----------------------------------------------

_KEY_MEMORY_MGMT = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Session Manager\Memory Management"
)


def _apply_increase_ntfs_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: increase NTFS paged-pool memory usage")
    SESSION.backup([_KEY_FILESYSTEM], "NtfsMemoryUsage")
    SESSION.set_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage", 2)


def _remove_increase_ntfs_memory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "NtfsMemoryUsage_Remove")
    SESSION.delete_value(_KEY_FILESYSTEM, "NtfsMemoryUsage")


def _detect_increase_ntfs_memory() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "NtfsMemoryUsage") == 2


# -- Disable NTFS filename tunneling ------------------------------------------


def _apply_disable_tunneling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: disable NTFS filename tunneling")
    SESSION.backup([_KEY_FILESYSTEM], "DisableTunneling")
    SESSION.set_dword(_KEY_FILESYSTEM, "MaximumTunnelEntries", 0)


def _remove_disable_tunneling(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_FILESYSTEM], "DisableTunneling_Remove")
    SESSION.delete_value(_KEY_FILESYSTEM, "MaximumTunnelEntries")


def _detect_disable_tunneling() -> bool:
    return SESSION.read_dword(_KEY_FILESYSTEM, "MaximumTunnelEntries") == 0


# -- Enable large system cache ------------------------------------------------


def _apply_enable_large_system_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("FileSystem: enable large system cache for file server workloads")
    SESSION.backup([_KEY_MEMORY_MGMT], "LargeSystemCache")
    SESSION.set_dword(_KEY_MEMORY_MGMT, "LargeSystemCache", 1)


def _remove_enable_large_system_cache(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_KEY_MEMORY_MGMT], "LargeSystemCache_Remove")
    SESSION.set_dword(_KEY_MEMORY_MGMT, "LargeSystemCache", 0)


def _detect_enable_large_system_cache() -> bool:
    return SESSION.read_dword(_KEY_MEMORY_MGMT, "LargeSystemCache") == 1


TWEAKS += [
    TweakDef(
        id="fs-increase-ntfs-memory",
        label="Increase NTFS Memory Usage",
        category="File System",
        apply_fn=_apply_increase_ntfs_memory,
        remove_fn=_remove_increase_ntfs_memory,
        detect_fn=_detect_increase_ntfs_memory,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Sets NtfsMemoryUsage to 2 (maximum) to allocate more paged pool "
            "for NTFS operations. Improves performance on file-heavy workloads. "
            "Default: 1. Recommended: 2 on systems with >=16 GB RAM."
        ),
        tags=["filesystem", "ntfs", "memory", "performance", "paged-pool"],
    ),
    TweakDef(
        id="fs-disable-tunneling",
        label="Disable NTFS Filename Tunneling",
        category="File System",
        apply_fn=_apply_disable_tunneling,
        remove_fn=_remove_disable_tunneling,
        detect_fn=_detect_disable_tunneling,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_FILESYSTEM],
        description=(
            "Disables NTFS filename tunneling by setting MaximumTunnelEntries "
            "to 0. Prevents DOS-era filename compatibility caching. "
            "Default: 256. Recommended: 0 on modern systems."
        ),
        tags=["filesystem", "ntfs", "tunneling", "performance", "legacy"],
    ),
    TweakDef(
        id="fs-enable-large-system-cache",
        label="Enable Large System Cache",
        category="File System",
        apply_fn=_apply_enable_large_system_cache,
        remove_fn=_remove_enable_large_system_cache,
        detect_fn=_detect_enable_large_system_cache,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_KEY_MEMORY_MGMT],
        description=(
            "Enables the large system file cache (LargeSystemCache=1). "
            "Optimizes memory for file server workloads at the cost of app memory. "
            "Default: 0 (desktop). Recommended: 1 for NAS/file server roles."
        ),
        tags=["filesystem", "cache", "memory", "server", "performance"],
    ),
]
