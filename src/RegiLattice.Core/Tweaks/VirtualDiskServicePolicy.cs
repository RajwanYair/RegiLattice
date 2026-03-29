// RegiLattice.Core — Tweaks/VirtualDiskServicePolicy.cs
// Virtual Disk Service (VDS), disk management, attachment, and detach policy controls — Sprint 490.
// Category: "Virtual Disk Service Policy" | Slug: vdspol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DiskManagement

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VirtualDiskServicePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DiskManagement";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "vdspol-block-vhd-mount",
                Label = "Block Standard Users from Mounting VHD/VHDX Files",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Prevents standard (non-admin) users from attaching or mounting Virtual Hard Disk (VHD/VHDX) files, closing the data-exfiltration path of creating an encrypted virtual disk and filling it with sensitive data.",
                Tags = ["vhd", "virtual-disk", "mount", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "VHD/VHDX mounting restricted to administrators; standard users cannot attach virtual disk files.",
                ApplyOps = [RegOp.SetDword(Key, "AllowVHDMount", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowVHDMount")],
                DetectOps = [RegOp.CheckDword(Key, "AllowVHDMount", 0)],
            },
            new TweakDef
            {
                Id = "vdspol-block-iso-mount",
                Label = "Block Standard Users from Mounting ISO/IMG Files",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Prevents standard users from mounting ISO, IMG, and other optical disc image files via the Explorer 'Mount' context menu, restricting virtual drive creation to administrators.",
                Tags = ["iso", "virtual-drive", "mount", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "ISO/IMG mounting restricted to admins; standard users cannot browse or execute content from disc images.",
                ApplyOps = [RegOp.SetDword(Key, "AllowISOMount", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowISOMount")],
                DetectOps = [RegOp.CheckDword(Key, "AllowISOMount", 0)],
            },
            new TweakDef
            {
                Id = "vdspol-disable-disk-management-snap-in",
                Label = "Disable Disk Management Snap-In for Standard Users",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Blocks the Disk Management MMC snap-in (diskmgmt.msc) for non-administrator accounts, preventing standard users from viewing, partitioning, formatting, or managing physical and virtual disks.",
                Tags = ["disk-management", "mmc", "standard-user", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disk Management blocked for standard users; partitioning and disk operations require admin elevation.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDiskManagementSnapIn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDiskManagementSnapIn")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDiskManagementSnapIn", 1)],
            },
            new TweakDef
            {
                Id = "vdspol-require-admin-for-partition",
                Label = "Require Admin for Disk Partitioning Operations",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Enforces that all disk partitioning operations (create, delete, resize partition) require administrator privileges, preventing accidental or malicious disk modification by standard users.",
                Tags = ["disk-management", "partition", "admin", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Partitioning operations require admin rights; standard users cannot modify partition layout.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminForPartitioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForPartitioning")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminForPartitioning", 1)],
            },
            new TweakDef
            {
                Id = "vdspol-disable-removable-format",
                Label = "Disable Formatting of Removable Drives by Standard Users",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Prevents standard users from formatting removable drives (USB drives, SD cards, external HDDs) through Explorer or Disk Management, avoiding irreversible data loss by users without sufficient knowledge.",
                Tags = ["disk-management", "format", "removable", "standard-user", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removable media formatting restricted to admins; standard users cannot format USB drives.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRemovableMediaFormat", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRemovableMediaFormat")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRemovableMediaFormat", 1)],
            },
            new TweakDef
            {
                Id = "vdspol-log-vhd-attach-events",
                Label = "Enable Audit Logging for VHD/VHDX Attach and Detach Events",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Enables event log entries for every VHD/VHDX mount and unmount operation, recording the file path and user account responsible for each attachment.",
                Tags = ["vhd", "audit-log", "virtual-disk", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "VHD/VHDX attach/detach events logged; virtual disk mount activity is auditable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditVHDMountEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditVHDMountEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditVHDMountEvents", 1)],
            },
            new TweakDef
            {
                Id = "vdspol-disable-dynamic-disk",
                Label = "Disable Dynamic Disk Conversions",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Prevents conversion of basic disks to dynamic disk format, blocking the creation of spanned, striped, or mirrored volumes via Windows dynamic disk — recommending Storage Spaces instead for resilient configurations.",
                Tags = ["disk-management", "dynamic-disk", "conversion", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Dynamic disk conversion disabled; basic disks cannot be upgraded to dynamic. Use Storage Spaces for resilience.",
                ApplyOps = [RegOp.SetDword(Key, "DisableDynamicDiskConversion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDynamicDiskConversion")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDynamicDiskConversion", 1)],
            },
            new TweakDef
            {
                Id = "vdspol-block-auto-initialize-disk",
                Label = "Block Automatic Disk Initialisation on New Disk Detection",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Prevents Windows from automatically opening the Initialize Disk wizard when a new uninitialized disk is detected, requiring an administrator to manually initiate disk initialisation.",
                Tags = ["disk-management", "auto-initialize", "new-disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Automatic disk initialisation wizard suppressed; admins must manually initialise new disks.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAutoInitializeDisk", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoInitializeDisk")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAutoInitializeDisk", 1)],
            },
            new TweakDef
            {
                Id = "vdspol-require-bitlocker-for-external",
                Label = "Require BitLocker Encryption Before External Drive Writability",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Requires that external or removable drives be encrypted with BitLocker To Go before allowing write access, preventing unencrypted exfiltration of sensitive data to external media.",
                Tags = ["disk-management", "bitlocker", "external-drive", "encryption", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "External drives require BitLocker encryption to become writable; unencrypted drives are read-only.",
                ApplyOps = [RegOp.SetDword(Key, "RequireBitLockerForExternalWritable", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireBitLockerForExternalWritable")],
                DetectOps = [RegOp.CheckDword(Key, "RequireBitLockerForExternalWritable", 1)],
            },
            new TweakDef
            {
                Id = "vdspol-disable-wps-disk-provision",
                Label = "Disable Windows Provisioning Service Disk Auto-Provision",
                Category = "Virtual Disk Service Policy",
                Description =
                    "Disables the Windows Provisioning Service automatic disk provisioning feature that configures disk topology on first boot, ensuring that enterprise imaging tools retain full control over disk layout.",
                Tags = ["disk-management", "provisioning", "auto-provision", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows Provisioning disk auto-provision disabled; disk layout managed by enterprise imaging tools.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoProvisionDisk", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProvisionDisk")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoProvisionDisk", 1)],
            },
        ];
}
