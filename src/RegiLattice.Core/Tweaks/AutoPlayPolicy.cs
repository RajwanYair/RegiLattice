// RegiLattice.Core — Tweaks/AutoPlayPolicy.cs
// AutoPlay and AutoRun machine-scope GPO controls — Sprint 212.
// Disables AutoRun/AutoPlay for USB drives, optical discs, and removable media.
// Category: "AutoPlay Policy" | Slug: autoplay
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AutoPlayPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "autoplay-disable-autorun-all",
                Label = "Disable AutoRun on All Drive Types",
                Category = "AutoPlay Policy",
                Description =
                    "Disables AutoRun on all drive types (removable, fixed, optical, network, RAM disk). Prevents automatic execution of malware from inserted USB drives or optical media — one of the most common infection vectors. Default: AutoRun enabled for some types. Recommended: 255 (all types).",
                Tags = ["autoplay", "autorun", "usb", "removable", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "AutoRun is disabled on all media; no code executes automatically when media is inserted.",
                ApplyOps = [RegOp.SetDword(Key, "NoAutorun", 255)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAutorun")],
                DetectOps = [RegOp.CheckDword(Key, "NoAutorun", 255)],
            },
            new TweakDef
            {
                Id = "autoplay-disable-for-removable",
                Label = "Disable AutoPlay on Removable Drives",
                Category = "AutoPlay Policy",
                Description =
                    "Turns off the AutoPlay dialog for removable storage (USB flash drives, memory cards). Users must manually open and browse inserted removable media. Default: AutoPlay dialog shown. Recommended: 1.",
                Tags = ["autoplay", "usb", "removable", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "No AutoPlay dialog appears for USB stick or SD card insertions.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoplayForNonVolume", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoplayForNonVolume")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoplayForNonVolume", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-disable-for-optical",
                Label = "Disable AutoPlay on Optical Drives (CD/DVD/Blu-ray)",
                Category = "AutoPlay Policy",
                Description =
                    "Disables the AutoPlay dialog when a CD, DVD, or Blu-ray disc is inserted. Prevents automatic installation, media play, or execution of disc content. Default: AutoPlay shown for optical discs. Recommended: 1.",
                Tags = ["autoplay", "cd", "dvd", "optical", "media", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "No AutoPlay dialog for optical disc insertions; disc contents must be browsed manually.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoplayForOptical", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoplayForOptical")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoplayForOptical", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-disable-for-network",
                Label = "Disable AutoPlay on Network Drives",
                Category = "AutoPlay Policy",
                Description =
                    "Prevents the AutoPlay dialog from opening when a network drive is mapped or connected. Eliminates risk from autorun.inf files on network shares. Default: AutoPlay disabled on network by default in Windows 10+. Recommended: 1 for explicit policy enforcement.",
                Tags = ["autoplay", "network", "share", "smb", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "AutoPlay is explicitly blocked on network drives via policy.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoplayForNetworkDrive", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoplayForNetworkDrive")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoplayForNetworkDrive", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-set-default-action-none",
                Label = "Set AutoPlay Default Action to 'Take No Action'",
                Category = "AutoPlay Policy",
                Description =
                    "Configures the AutoPlay default handler for all media types to 'Take no action'. Even if AutoPlay is not fully disabled, no action is taken automatically on media insertion. Default: Windows auto-selects handler. Recommended: 1.",
                Tags = ["autoplay", "default-action", "media", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "AutoPlay takes no action on any media type; users see no dialog and no auto-launch occurs.",
                ApplyOps = [RegOp.SetDword(Key, "NoAutoplayDefaultAction", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoAutoplayDefaultAction")],
                DetectOps = [RegOp.CheckDword(Key, "NoAutoplayDefaultAction", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-block-autorun-inf",
                Label = "Block autorun.inf Execution from Any Drive",
                Category = "AutoPlay Policy",
                Description =
                    "Explicitly blocks execution of autorun.inf files from all drive types. The autorun.inf mechanism is the primary vehicle for USB weaponisation. Default: blocked on fixed/network drives in modern Windows, but enforced here for all types. Recommended: 1.",
                Tags = ["autoplay", "autorun.inf", "usb", "malware", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "autorun.inf files are ignored on all drive types; USB malware relying on this vector is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAutorunInf", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAutorunInf")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAutorunInf", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-block-user-override",
                Label = "Block Users from Changing AutoPlay Settings",
                Category = "AutoPlay Policy",
                Description =
                    "Prevents users from changing AutoPlay settings in Settings → Bluetooth & Devices → AutoPlay. Ensures the IT-configured AutoPlay policy cannot be overridden by end users. Default: users can change. Recommended: 1.",
                Tags = ["autoplay", "user-restriction", "settings", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AutoPlay settings page is locked; users cannot re-enable AutoPlay or change media defaults.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAutoplaySettingsChange", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoplaySettingsChange")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAutoplaySettingsChange", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-disable-for-camera-import",
                Label = "Disable AutoPlay for Camera / Photo Import",
                Category = "AutoPlay Policy",
                Description =
                    "Prevents the AutoPlay dialog from offering to import photos/videos when a digital camera or phone is connected. Users must manually launch the import workflow. Default: AutoPlay dialog offered. Recommended: 1 to prevent unintended data access.",
                Tags = ["autoplay", "camera", "photo", "import", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Camera/phone connections do not trigger the AutoPlay photo import dialog.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoPlayForCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoPlayForCamera")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoPlayForCamera", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-log-media-insertions",
                Label = "Audit Log Media Insertion Events",
                Category = "AutoPlay Policy",
                Description =
                    "Enables logging of removable media insertion events to the Security audit log. Provides a device usage trail for DLP and forensic investigations. Default: not audited. Recommended: 1 on monitored endpoints.",
                Tags = ["autoplay", "audit", "media", "usb", "dlp", "forensics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "USB/optical media insertions are recorded in the Security event log for forensic purposes.",
                ApplyOps = [RegOp.SetDword(Key, "AuditMediaInsertions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditMediaInsertions")],
                DetectOps = [RegOp.CheckDword(Key, "AuditMediaInsertions", 1)],
            },
            new TweakDef
            {
                Id = "autoplay-disable-for-mtp-devices",
                Label = "Disable AutoPlay for MTP / Portable Devices",
                Category = "AutoPlay Policy",
                Description =
                    "Turns off AutoPlay for MTP (Media Transfer Protocol) devices such as smartphones, tablets, and MP3 players. Stops automatic launch of Windows Photo Import or Windows Media Player when a mobile device is connected. Default: AutoPlay dialog offered. Recommended: 1.",
                Tags = ["autoplay", "mtp", "mobile", "portable-device", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Connecting a phone/tablet does not trigger AutoPlay; users must manually open File Explorer.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoPlayForMTP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoPlayForMTP")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoPlayForMTP", 1)],
            },
        ];
}
