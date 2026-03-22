// RegiLattice.Core — Tweaks/AutoRunPolicy.cs
// Windows AutoRun and AutoPlay policy settings (Sprint 82).
// Slug "autorun" — NoDriveTypeAutoRun bitmask, AutoPlay defaults, USB/CD policies.
// Distinct from UsbPeripherals.cs (device detection/power) and security hardening.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AutoRunPolicy
{
    private const string AutoRunUser =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    private const string AutoRunSys =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

    private const string AutoPlayUser =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers";

    private const string AutoRunPolicy2 =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "autorun-disable-all-drives",
            Label = "Disable AutoRun on All Drive Types",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["autorun", "autoplay", "security", "usb", "removable"],
            Description =
                "Disables AutoRun on all drive types (0xFF = 255 = all drives). "
                + "Prevents malware from auto-executing via USB drives, CDs, or network "
                + "shares. One of the most effective defenses against physical-access malware.",
            ApplyOps = [RegOp.SetDword(AutoRunUser, "NoDriveTypeAutoRun", 0xFF)],
            RemoveOps = [RegOp.DeleteValue(AutoRunUser, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(AutoRunUser, "NoDriveTypeAutoRun", 0xFF)],
        },
        new TweakDef
        {
            Id = "autorun-disable-all-drives-system",
            Label = "Disable AutoRun System-Wide (All Drive Types)",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["autorun", "autoplay", "security", "policy", "system"],
            Description =
                "Disables AutoRun on all drive types for all users via system policy "
                + "(HKLM). Affects all accounts including newly created ones. Stronger "
                + "than per-user setting for shared/managed machines.",
            ApplyOps = [RegOp.SetDword(AutoRunSys, "NoDriveTypeAutoRun", 0xFF)],
            RemoveOps = [RegOp.DeleteValue(AutoRunSys, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(AutoRunSys, "NoDriveTypeAutoRun", 0xFF)],
        },
        new TweakDef
        {
            Id = "autorun-disable-removable-drives",
            Label = "Disable AutoRun on Removable Drives Only",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["autorun", "usb", "removable", "security"],
            Description =
                "Disables AutoRun specifically for removable drives (USB flash drives, "
                + "portable HDDs). Value 0x4 disables bit 2 (removable). CD/DVD and "
                + "fixed disk AutoRun remain unaffected.",
            ApplyOps = [RegOp.SetDword(AutoRunUser, "NoDriveTypeAutoRun", 0x4)],
            RemoveOps = [RegOp.DeleteValue(AutoRunUser, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(AutoRunUser, "NoDriveTypeAutoRun", 0x4)],
        },
        new TweakDef
        {
            Id = "autorun-disable-autoplay-default",
            Label = "Disable AutoPlay Default Handler",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["autorun", "autoplay", "default", "usb"],
            Description =
                "Sets AutoPlay to 'Take no action' for all media types by default. "
                + "The AutoPlay dialog is still shown but no app launches automatically. "
                + "Value 1 = disabled automatic action selection.",
            ApplyOps = [RegOp.SetDword(AutoPlayUser, "DisableAutoplay", 1)],
            RemoveOps = [RegOp.DeleteValue(AutoPlayUser, "DisableAutoplay")],
            DetectOps = [RegOp.CheckDword(AutoPlayUser, "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "autorun-disable-autoplay-policy",
            Label = "Disable AutoPlay via System Policy",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["autorun", "autoplay", "policy", "system", "security"],
            Description =
                "Disables the AutoPlay feature entirely via Group Policy for all drives. "
                + "The AutoPlay dialog will not appear when media/devices are connected. "
                + "Recommended for corporate/shared-use environments.",
            ApplyOps = [RegOp.SetDword(AutoRunPolicy2, "NoAutoPlay", 1)],
            RemoveOps = [RegOp.DeleteValue(AutoRunPolicy2, "NoAutoPlay")],
            DetectOps = [RegOp.CheckDword(AutoRunPolicy2, "NoAutoPlay", 1)],
        },
        new TweakDef
        {
            Id = "autorun-disable-autorun-exe",
            Label = "Block AutoRun.inf Executable Launch",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["autorun", "autorun.inf", "security", "malware", "usb"],
            Description =
                "Blocks the execution of entries in AutoRun.inf files via policy. "
                + "Prevents the [open], [shellexecute], and [shell] AutoRun.inf sections "
                + "from launching executables, even when AutoRun is otherwise enabled.",
            ApplyOps = [RegOp.SetDword(AutoRunSys, "NoAutorun", 1)],
            RemoveOps = [RegOp.DeleteValue(AutoRunSys, "NoAutorun")],
            DetectOps = [RegOp.CheckDword(AutoRunSys, "NoAutorun", 1)],
        },
        new TweakDef
        {
            Id = "autorun-disable-cd-autoplay",
            Label = "Disable AutoPlay for CD/DVD Drives",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["autorun", "cd", "dvd", "optical", "autoplay"],
            Description =
                "Disables AutoPlay for CD and DVD optical drives (bits 0x20 + 0x4 = 32+4). "
                + "CD media will no longer prompt or auto-launch when inserted. "
                + "USB and fixed drives are unaffected.",
            ApplyOps = [RegOp.SetDword(AutoRunUser, "NoDriveTypeAutoRun", 0x24)],
            RemoveOps = [RegOp.DeleteValue(AutoRunUser, "NoDriveTypeAutoRun")],
            DetectOps = [RegOp.CheckDword(AutoRunUser, "NoDriveTypeAutoRun", 0x24)],
        },
        new TweakDef
        {
            Id = "autorun-disable-network-drive-autoplay",
            Label = "Disable AutoPlay on Network Drives",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["autorun", "network", "share", "autoplay", "security"],
            Description =
                "Disables AutoPlay when accessing mapped network drives or UNC paths. "
                + "Prevents attackers from placing malicious AutoRun content on "
                + "network shares that automatically executes when browsed.",
            ApplyOps = [RegOp.SetDword(AutoRunUser, "NoDriveAutoRun", 0xFF)],
            RemoveOps = [RegOp.DeleteValue(AutoRunUser, "NoDriveAutoRun")],
            DetectOps = [RegOp.CheckDword(AutoRunUser, "NoDriveAutoRun", 0xFF)],
        },
        new TweakDef
        {
            Id = "autorun-disable-mixed-content-autoplay",
            Label = "Disable AutoPlay for Mixed-Content Drives",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["autorun", "mixed content", "autoplay", "media"],
            Description =
                "Disables AutoPlay for drives containing mixed content (both data files "
                + "and media). Prevents Windows from auto-opening Explorer or a media "
                + "player when such drives are inserted.",
            ApplyOps = [RegOp.SetDword(AutoPlayUser, "MixedContentAutoplayType", 0)],
            RemoveOps = [RegOp.DeleteValue(AutoPlayUser, "MixedContentAutoplayType")],
            DetectOps = [RegOp.CheckDword(AutoPlayUser, "MixedContentAutoplayType", 0)],
        },
        new TweakDef
        {
            Id = "autorun-disable-enhanced-autoplay",
            Label = "Disable Enhanced AutoPlay Search",
            Category = "AutoRun & AutoPlay",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["autorun", "autoplay", "search", "media"],
            Description =
                "Disables Windows enhanced AutoPlay which scans removable media for "
                + "additional content types (photos, music, video) beyond what AutoRun.inf "
                + "specifies. Reduces disk scanning delay on USB insertion.",
            ApplyOps = [RegOp.SetDword(AutoPlayUser, "UseAutoPlay", 0)],
            RemoveOps = [RegOp.DeleteValue(AutoPlayUser, "UseAutoPlay")],
            DetectOps = [RegOp.CheckDword(AutoPlayUser, "UseAutoPlay", 0)],
        },
    ];
}
