// RegiLattice.Core — Tweaks/WslFileSystemPolicy.cs
// WSL virtual file system, DrvFs mount, and Linux-Windows filesystem boundary Group Policy controls (Sprint 608).
// Category: "WSL Filesystem Policy" | Slug: wslfs
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Lxss\FileSystem

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WslFileSystemPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\FileSystem";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wslfs-disable-windows-drive-automount",
            Label = "WSL Filesystem: Disable Auto-Mount of Windows Drives in WSL",
            Category = "WSL Filesystem Policy",
            Description = "Sets DisableWindowsDriveAutomount=1 in Lxss FileSystem policy. Prevents WSL from automatically mounting Windows drive letters (C:, D:, etc.) under /mnt/ when a terminal session starts. " +
                "Auto-mounting of Windows drives gives every process within the WSL environment — including any Linux malware — unrestricted read/write access to the full Windows user profile, including OneDrive, Documents, and AppData. With auto-mount disabled, a compromised Linux process cannot traverse from /mnt/c to Windows system paths without an explicit user mount command.",
            Tags = ["wsl", "filesystem", "automount", "drvfs", "isolation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Windows drives not auto-mounted in WSL; Linux processes cannot access Windows file system without explicit mount.",
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsDriveAutomount", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsDriveAutomount")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsDriveAutomount", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-disable-network-drive-mount",
            Label = "WSL Filesystem: Disable Mounting of Network UNC Paths in WSL",
            Category = "WSL Filesystem Policy",
            Description = "Sets DisableNetworkDriveMount=1 in Lxss FileSystem policy. Prevents WSL from mounting UNC paths (\\\\server\\share) or mapped network drives in the WSL file system. " +
                "Allowing Linux processes to mount network shares expands the blast radius of WSL-based compromise to network-attached storage. Ransomware running in WSL with network drive access can encrypt network share contents with Linux-native encryption tools (openssl, gpg) that may not be detected by Windows-based endpoint protection monitoring network path writes.",
            Tags = ["wsl", "filesystem", "network-drive", "unc", "ransomware"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "UNC/network paths not mountable in WSL; Linux processes cannot reach network shares.",
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkDriveMount", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkDriveMount")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkDriveMount", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-disable-wsl-host-mount",
            Label = "WSL Filesystem: Disable WSL Host Physical Disk Mount (wsl --mount)",
            Category = "WSL Filesystem Policy",
            Description = "Sets DisableHostDiskMount=1 in Lxss FileSystem policy. Blocks the 'wsl --mount' command that allows a user to attach a physical disk or disk image directly into the WSL 2 VM, bypassing Windows file system filters. " +
                "The 'wsl --mount' feature was designed for accessing Linux-native file systems (ext4, btrfs) on physical disks. However, it also allows attaching NTFS volumes directly into the WSL VM's kernel, bypassing Windows NTFS ACLs and file system minifilter drivers (including DLP, AV, and EDR file access monitors). Blocking this command eliminates a Windows security control bypass vector.",
            Tags = ["wsl", "filesystem", "disk-mount", "host-mount", "filter-bypass"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Physical disk mount into WSL blocked; prevents Windows file system filter driver bypass via wsl --mount.",
            ApplyOps = [RegOp.SetDword(Key, "DisableHostDiskMount", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHostDiskMount")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHostDiskMount", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-disable-bind-mount",
            Label = "WSL Filesystem: Disable Linux Bind Mounts Across Distro Boundaries",
            Category = "WSL Filesystem Policy",
            Description = "Sets DisableBindMount=1 in Lxss FileSystem policy. Prevents the use of Linux bind mounts within WSL that would map one distro's file system paths into another distro's namespace. " +
                "In environments where multiple WSL distros coexist, allowing bind mounts between distros removes the isolation boundary between them. A compromised distro could bind-mount another distro's home directory or secret store, reading credentials that belong to a separate Linux identity/environment. Disabling cross-distro bind mounts preserves per-distro filesystem isolation.",
            Tags = ["wsl", "filesystem", "bind-mount", "distro-isolation", "multi-distro"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Cross-distro bind mounts blocked; each WSL distro's filesystem remains isolated from sibling distros.",
            ApplyOps = [RegOp.SetDword(Key, "DisableBindMount", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBindMount")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBindMount", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-enforce-drvfs-read-only",
            Label = "WSL Filesystem: Enforce DrvFs Windows Drive Mounts as Read-Only",
            Category = "WSL Filesystem Policy",
            Description = "Sets EnforceDrvFsReadOnly=1 in Lxss FileSystem policy. Forces all DrvFs mounts of Windows drives (e.g., /mnt/c) to be mounted read-only, preventing Linux processes from writing to the Windows file system through the DrvFs mount point. " +
                "This is the strongest DrvFs hardening mode — Linux tools can read Windows files (e.g., build input files) but cannot write to Windows folders. Write-only WSL access to Windows paths is the most common vector for WSL-based file destruction: ransomware in WSL can encrypt /mnt/c/Users/... using Linux commands that bypass Windows AV real-time protection.",
            Tags = ["wsl", "drvfs", "read-only", "ransomware", "write-protection"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "DrvFs mounts are read-only; Linux processes cannot write to Windows drives. Cross-environment write workflows will break.",
            ApplyOps = [RegOp.SetDword(Key, "EnforceDrvFsReadOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceDrvFsReadOnly")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceDrvFsReadOnly", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-disable-drvfs-metadata-mode",
            Label = "WSL Filesystem: Disable DrvFs Metadata Mode (Linux Permission Emulation)",
            Category = "WSL Filesystem Policy",
            Description = "Sets DisableDrvFsMetadata=1 in Lxss FileSystem policy. Disables the DrvFs metadata extension that stores Linux file permissions, ownership (UID/GID), and extended attributes in NTFS extended attributes on Windows files. " +
                "DrvFs metadata mode allows Linux processes to mark Windows files as setuid-root or setgid, creating files in Windows directories that, if subsequently executed by a Windows process, might behave unexpectedly due to permission metadata misinterpretation. While Windows ignores setuid bits, disabling metadata prevents Linux permission artefacts from being embedded in Windows file system objects.",
            Tags = ["wsl", "drvfs", "metadata", "permissions", "setuid"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "No Linux permission metadata stored on Windows NTFS files; DrvFs presents all files with default umask ownership.",
            ApplyOps = [RegOp.SetDword(Key, "DisableDrvFsMetadata", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDrvFsMetadata")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDrvFsMetadata", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-disable-plan9-mount-server",
            Label = "WSL Filesystem: Disable Plan 9 File System Mount Server",
            Category = "WSL Filesystem Policy",
            Description = "Sets DisablePlan9MountServer=1 in Lxss FileSystem policy. Disables the 9P (Plan 9 File System Protocol) server running inside the WSL 2 VM that provides the Windows←→Linux file sharing capability over a virtual Hyper-V vsock connection. " +
                "The 9P file server in the WSL VM is the component that handles all cross-OS file access. If a vulnerability exists in the 9P server implementation, it could be exploited by a compromised Windows process to escalate into the WSL VM, or by a compromised Linux process to reach the Windows namespace. Disabling 9P eliminates this boundary component entirely.",
            Tags = ["wsl", "plan9", "9p", "vsock", "attack-surface"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "9P file server disabled; cross-OS file sharing via /mnt/ will stop working entirely.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePlan9MountServer", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePlan9MountServer")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePlan9MountServer", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-set-vhd-disk-quota-20gb",
            Label = "WSL Filesystem: Set VHD Disk Quota Maximum to 20 GB",
            Category = "WSL Filesystem Policy",
            Description = "Sets VhdDiskQuotaGB=20 in Lxss FileSystem policy. Limits the maximum size that a WSL virtual hard disk (ext4.vhdx) can grow to 20 GB per distribution, preventing runaway Linux processes from filling the host disk. " +
                "WSL 2 VHD files start small and dynamically expand on demand up to a default cap of 1 TB. Linux processes performing large operations (building Docker images, running large ML training jobs, downloading large datasets) can inadvertently — or intentionally — fill the host disk by consuming the VHD expansion headroom. A 20 GB cap ensures WSL disk usage remains bounded.",
            Tags = ["wsl", "vhd", "disk-quota", "disk-space", "ext4"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL VHD limited to 20 GB; Linux environments filling the host disk mitigation.",
            ApplyOps = [RegOp.SetDword(Key, "VhdDiskQuotaGB", 20)],
            RemoveOps = [RegOp.DeleteValue(Key, "VhdDiskQuotaGB")],
            DetectOps = [RegOp.CheckDword(Key, "VhdDiskQuotaGB", 20)],
        },
        new TweakDef
        {
            Id = "wslfs-enable-filesystem-access-audit",
            Label = "WSL Filesystem: Enable Cross-OS Filesystem Access Audit Logging",
            Category = "WSL Filesystem Policy",
            Description = "Sets EnableFileSystemAccessAudit=1 in Lxss FileSystem policy. Enables logging of all file access events that cross the Windows-Linux filesystem boundary via DrvFs, writing entries to the Security event log under the Windows Subsystem for Linux provider. " +
                "Without DrvFs access auditing, there is no Windows Security event log record of which Linux processes accessed which Windows files through /mnt/. This makes it impossible to determine the scope of a WSL-based file access incident post-breach. Audit logging of DrvFs access enables forensic reconstruction and real-time SIEM alerting on unexpected Linux access to sensitive Windows paths.",
            Tags = ["wsl", "filesystem", "audit", "siem", "drvfs"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "DrvFs cross-OS access events logged to Security event log; enables forensic reconstruction of Linux file access incidents.",
            ApplyOps = [RegOp.SetDword(Key, "EnableFileSystemAccessAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableFileSystemAccessAudit")],
            DetectOps = [RegOp.CheckDword(Key, "EnableFileSystemAccessAudit", 1)],
        },
        new TweakDef
        {
            Id = "wslfs-disable-tmpfs-overflow-to-host",
            Label = "WSL Filesystem: Disable tmpfs Overflow Spilling to Windows Host Disk",
            Category = "WSL Filesystem Policy",
            Description = "Sets DisableTmpfsHostOverflow=1 in Lxss FileSystem policy. Prevents the WSL VM's in-memory tmpfs (/tmp, /run) from spilling overflow pages onto the Windows host disk when the VM's allocated RAM is exhausted. " +
                "When WSL processes fill /tmp with large temporary files and the VM's RAM is exhausted, the kernel may begin swapping tmpfs pages to a backing swap store. Allowing this swap store to be on the host Windows disk effectively extends the VM's writable footprint onto the Windows NTFS volume in a way that bypasses the explicit DrvFs mount controls, since swap activity occurs at a lower abstraction layer.",
            Tags = ["wsl", "tmpfs", "swap", "host-disk", "overflow"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "tmpfs overflow to host disk blocked; WSL VM memory pressure will OOM-kill processes rather than spill to host.",
            ApplyOps = [RegOp.SetDword(Key, "DisableTmpfsHostOverflow", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTmpfsHostOverflow")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTmpfsHostOverflow", 1)],
        },
    ];
}
