// RegiLattice.Core — Tweaks/PortableDevicePolicy.cs
// Portable device (WPD) access controls, MTP, camera, and media device policy — Sprint 480.
// Category: "Portable Device Policy" | Slug: wpd
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PortableDevicePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wpd-deny-removable-devices",
                Label = "Deny All Removable Device Installation",
                Category = "Portable Device Policy",
                Description =
                    "Blocks installation of all removable storage devices (USB drives, SD cards, portable media players) by denying their device class setup, preventing data exfiltration via removable media.",
                Tags = ["wpd", "removable-device", "usb", "storage", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "All removable storage devices blocked at installation; USB drives and SD cards cannot be used.",
                ApplyOps = [RegOp.SetDword(Key, "DenyRemovableDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyRemovableDevices")],
                DetectOps = [RegOp.CheckDword(Key, "DenyRemovableDevices", 1)],
            },
            new TweakDef
            {
                Id = "wpd-deny-portable-devices",
                Label = "Deny Portable Device (MTP/WPD) Access",
                Category = "Portable Device Policy",
                Description =
                    "Blocks access to Windows Portable Devices (WPD) via the Media Transfer Protocol (MTP), preventing smartphones, cameras, and media players from accessing or transferring files when connected via USB.",
                Tags = ["wpd", "mtp", "portable-device", "usb", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WPD MTP access blocked; phones/cameras connected via USB cannot transfer files.",
                ApplyOps = [RegOp.SetDword(Key, "DenyPortableDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyPortableDevices")],
                DetectOps = [RegOp.CheckDword(Key, "DenyPortableDevices", 1)],
            },
            new TweakDef
            {
                Id = "wpd-block-autoplay-portable",
                Label = "Block AutoPlay for Portable Media Devices",
                Category = "Portable Device Policy",
                Description =
                    "Disables the AutoPlay action that launches when a portable device (camera, media player, phone) is connected, preventing automatic media import dialogs and auto-execution of content from portable devices.",
                Tags = ["wpd", "autoplay", "portable-device", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AutoPlay disabled for portable devices; no media import dialog when connecting cameras or phones.",
                ApplyOps = [RegOp.SetDword(Key, "NoAutoplayForPortableDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAutoplayForPortableDevices")],
                DetectOps = [RegOp.CheckDword(Key, "NoAutoplayForPortableDevices", 1)],
            },
            new TweakDef
            {
                Id = "wpd-block-camera-device-install",
                Label = "Block Camera Device Installation",
                Category = "Portable Device Policy",
                Description =
                    "Prevents installation of USB-connected camera devices (webcams, digital cameras) on the system, useful in secure environments where all photography and video capture must be blocked.",
                Tags = ["wpd", "camera", "usb", "device-install", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "USB camera devices blocked from installing; no external webcam or digital camera usable.",
                ApplyOps = [RegOp.SetDword(Key, "DenyCameraDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyCameraDevices")],
                DetectOps = [RegOp.CheckDword(Key, "DenyCameraDevices", 1)],
            },
            new TweakDef
            {
                Id = "wpd-disable-picture-transfer",
                Label = "Disable Windows Picture Transfer Protocol",
                Category = "Portable Device Policy",
                Description =
                    "Disables the Picture Transfer Protocol (PTP) used by digital cameras and smartphones to transfer photos, preventing photo device discovery and auto-import from cameras.",
                Tags = ["wpd", "ptp", "picture-transfer", "camera", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "PTP picture transfer disabled; digital cameras and phones in PTP mode cannot transfer photos.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePTPTransfer", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePTPTransfer")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePTPTransfer", 1)],
            },
            new TweakDef
            {
                Id = "wpd-apply-audit-on-write",
                Label = "Enable Write Audit Logging for Removable Media",
                Category = "Portable Device Policy",
                Description =
                    "Enables security audit events when files are written to removable storage devices, creating a log trail for data being exfiltrated to USB drives or portable devices.",
                Tags = ["wpd", "removable-media", "audit-log", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Write-to-removable-media events logged; potential data exfiltration to USB drives is auditable.",
                ApplyOps = [RegOp.SetDword(Key, "AuditRemovableMediaWrites", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableMediaWrites")],
                DetectOps = [RegOp.CheckDword(Key, "AuditRemovableMediaWrites", 1)],
            },
            new TweakDef
            {
                Id = "wpd-disable-usb-mass-storage",
                Label = "Disable USB Mass Storage Class Driver",
                Category = "Portable Device Policy",
                Description =
                    "Disables the USB Mass Storage class driver (usbstor), preventing USB flash drives, external hard drives, and USB memory sticks from mounting as drive letters in Windows Explorer.",
                Tags = ["wpd", "usb-mass-storage", "usbstor", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "USB mass storage driver disabled; USB drives, external HDDs, and flash drives cannot be used.",
                ApplyOps = [RegOp.SetDword(Key, "DisableUSBMassStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUSBMassStorage")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUSBMassStorage", 1)],
            },
            new TweakDef
            {
                Id = "wpd-block-portable-music-player",
                Label = "Block Portable Music Player Synchronisation",
                Category = "Portable Device Policy",
                Description =
                    "Blocks synchronisation between Windows media players (Windows Media Player sync) and portable MP3 or music players via USB, preventing media content from being exported to portable devices.",
                Tags = ["wpd", "music-player", "sync", "media", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Portable music player sync blocked; cannot sync music files from WMP to MP3 players.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPortableMusicPlayerSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPortableMusicPlayerSync")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPortableMusicPlayerSync", 1)],
            },
            new TweakDef
            {
                Id = "wpd-readonly-removable-media",
                Label = "Enforce Read-Only Mode for All Removable Media",
                Category = "Portable Device Policy",
                Description =
                    "Mounts all removable storage devices in read-only mode, allowing data to be read from USB drives but blocking any write operations to prevent data exfiltration.",
                Tags = ["wpd", "removable-media", "read-only", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Removable media read-only; cannot write files to USB drives or SD cards.",
                ApplyOps = [RegOp.SetDword(Key, "ReadOnlyRemovableMedia", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ReadOnlyRemovableMedia")],
                DetectOps = [RegOp.CheckDword(Key, "ReadOnlyRemovableMedia", 1)],
            },
            new TweakDef
            {
                Id = "wpd-block-external-thunderbolt-storage",
                Label = "Block External Thunderbolt Storage Devices",
                Category = "Portable Device Policy",
                Description =
                    "Prevents external storage devices connected via Thunderbolt from mounting, closing the high-speed Thunderbolt exfiltration path while allowing other Thunderbolt peripherals like displays and docks.",
                Tags = ["wpd", "thunderbolt", "storage", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Thunderbolt external storage blocked; TB drives and NVMe enclosures cannot mount.",
                ApplyOps = [RegOp.SetDword(Key, "BlockThunderboltStorage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThunderboltStorage")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThunderboltStorage", 1)],
            },
        ];
}
