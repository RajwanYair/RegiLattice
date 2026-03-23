#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// USB Storage Policy — removable storage access, write-protect, and device class policies.
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings
internal static class UsbStoragePolicy
{
    private const string StoragePolicy = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies";
    private const string RemovableDevices = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices";
    private const string UsbFloppyClass =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";
    private const string CdRomClass =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56308-b6bf-11d0-94f2-00a0c91efb8b}";
    private const string TapeClass =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630b-b6bf-11d0-94f2-00a0c91efb8b}";
    private const string WpdClass =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{6AC27878-A6FA-4155-BA85-F98F491D4F33}";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "usbstor-write-protect",
            Label = "USB Storage: Enable Hardware Write-Protection on All Removable Drives",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [StoragePolicy],
            Tags = ["usb", "storage", "write-protect", "dlp", "security"],
            Description =
                "Sets WriteProtect=1 in StorageDevicePolicies. Blocks all write operations to removable "
                + "storage devices (USB drives, SD cards) at the OS level. "
                + "Prevents data exfiltration via portable drives. Default: read/write. Recommended for kiosk/corporate.",
            ApplyOps = [RegOp.SetDword(StoragePolicy, "WriteProtect", 1)],
            RemoveOps = [RegOp.SetDword(StoragePolicy, "WriteProtect", 0)],
            DetectOps = [RegOp.CheckDword(StoragePolicy, "WriteProtect", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-all-removable-write",
            Label = "USB Storage: Deny Write Access to All Removable Storage Classes (GPO)",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RemovableDevices],
            Tags = ["usb", "storage", "gpo", "write", "dlp", "security"],
            Description =
                "Sets Deny_Write=1 in the RemovableStorageDevices class policy. Blocks write access to all "
                + "removable storage device classes via Group Policy. Complements the hardware WriteProtect flag. "
                + "Default: write allowed. Recommended for data loss prevention.",
            ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Write")],
            DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-all-removable-execute",
            Label = "USB Storage: Deny Execution from All Removable Storage Classes (GPO)",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RemovableDevices],
            Tags = ["usb", "storage", "gpo", "execute", "autorun", "security"],
            Description =
                "Sets Deny_Execute=1 in the RemovableStorageDevices class policy. Prevents launching "
                + "executables directly from any removable storage device. E.g., blocks BadUSB payloads. "
                + "Default: execution allowed. Recommended to block portable malware.",
            ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Execute", 1)],
            RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Execute")],
            DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Execute", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-all-removable-read",
            Label = "USB Storage: Deny Read Access to All Removable Storage (Strict Lockdown)",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [RemovableDevices],
            Tags = ["usb", "storage", "gpo", "lockdown", "high-security"],
            Description =
                "Sets Deny_Read=1 in the RemovableStorageDevices class policy. Blocks read access to all "
                + "removable storage devices. Extreme lockdown for air-gapped or high-security environments. "
                + "Default: read allowed. WARNING: prevents legitimate USB storage use.",
            ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Read", 1)],
            RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Read")],
            DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Read", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-usb-disk-write",
            Label = "USB Storage: Deny Write Access to USB Disk Drives (Class Policy)",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [UsbFloppyClass],
            Tags = ["usb", "storage", "disk", "write", "dlp", "class-driver"],
            Description =
                "Sets Deny_Write=1 in the USB disk drive device class GUID policy. "
                + "Targets the removable disk class specifically (includes USB flash, external HDD). "
                + "Default: write allowed. More targeted than the all-classes RemovableDevices key.",
            ApplyOps = [RegOp.SetDword(UsbFloppyClass, "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(UsbFloppyClass, "Deny_Write")],
            DetectOps = [RegOp.CheckDword(UsbFloppyClass, "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-cdrom-write",
            Label = "USB Storage: Deny Write Access to Optical/CD-ROM Drives",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [CdRomClass],
            Tags = ["usb", "cdrom", "optical", "write", "dlp", "security"],
            Description =
                "Sets Deny_Write=1 in the CD-ROM / optical drive device class GUID policy. "
                + "Prevents disc burning (ISO, data CD/DVD). Blocks data exfiltration via optical media. "
                + "Default: write allowed. Effective for blocking burnable media.",
            ApplyOps = [RegOp.SetDword(CdRomClass, "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(CdRomClass, "Deny_Write")],
            DetectOps = [RegOp.CheckDword(CdRomClass, "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-cdrom-read",
            Label = "USB Storage: Deny Read Access to optical drives",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [CdRomClass],
            Tags = ["usb", "cdrom", "optical", "read", "lockdown"],
            Description =
                "Sets Deny_Read=1 in the CD-ROM device class GUID policy. "
                + "Blocks mounting and reading optical discs. "
                + "Intended for high-security environments where disc insertion is prohibited.",
            ApplyOps = [RegOp.SetDword(CdRomClass, "Deny_Read", 1)],
            RemoveOps = [RegOp.DeleteValue(CdRomClass, "Deny_Read")],
            DetectOps = [RegOp.CheckDword(CdRomClass, "Deny_Read", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-tape-write",
            Label = "USB Storage: Deny Write Access to Tape Drives",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [TapeClass],
            Tags = ["usb", "tape", "backup", "write", "dlp", "security"],
            Description =
                "Sets Deny_Write=1 in the tape drive device class GUID policy. "
                + "Prevents writing to tape backup units from non-authorized users. "
                + "Default: tape writes allowed. Recommended for environments with tape backup controls.",
            ApplyOps = [RegOp.SetDword(TapeClass, "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(TapeClass, "Deny_Write")],
            DetectOps = [RegOp.CheckDword(TapeClass, "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-wpd-write",
            Label = "USB Storage: Deny Write Access to WPD (MTP/PTP) Portable Devices",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WpdClass],
            Tags = ["usb", "wpd", "mtp", "ptp", "phone", "write", "dlp"],
            Description =
                "Sets Deny_Write=1 in the WPD (Windows Portable Devices) class GUID policy. "
                + "Prevents file transfers to phones, cameras, and MTP/PTP devices connected via USB. "
                + "Default: write allowed. Blocks data transfer to smartphones and cameras.",
            ApplyOps = [RegOp.SetDword(WpdClass, "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(WpdClass, "Deny_Write")],
            DetectOps = [RegOp.CheckDword(WpdClass, "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "usbstor-deny-wpd-read",
            Label = "USB Storage: Deny Read Access to WPD (MTP/PTP) Portable Devices",
            Category = "USB Storage Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [WpdClass],
            Tags = ["usb", "wpd", "mtp", "ptp", "phone", "read", "lockdown"],
            Description =
                "Sets Deny_Read=1 in the WPD device class GUID policy. "
                + "Prevents reading files from phones, cameras, and MTP/PTP devices. "
                + "Extreme DLP measure to prevent any data exchange with portable consumer devices.",
            ApplyOps = [RegOp.SetDword(WpdClass, "Deny_Read", 1)],
            RemoveOps = [RegOp.DeleteValue(WpdClass, "Deny_Read")],
            DetectOps = [RegOp.CheckDword(WpdClass, "Deny_Read", 1)],
        },
    ];
}
