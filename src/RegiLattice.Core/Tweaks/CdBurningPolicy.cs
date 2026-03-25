#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 242 — CD & Optical Media Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\CDBurning
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer
//       HKCU\Software\Policies\Microsoft\Windows\Explorer
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56308-...} (CD-ROM)
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-...} (DVD)
internal static class CdBurningPolicy
{
    private const string BurnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CDBurning";
    private const string ExplLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\Explorer";
    private const string ExplCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer";
    private const string CdRomKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56308-b6bf-11d0-94f2-00a0c91efb8b}";
    private const string DvdKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cdbp-no-burning-machine",
            Label = "Disable CD/DVD Burning (Machine-Wide)",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets NoBurning=1 in the Windows CDBurning policy key for all users on this machine. "
                + "Removes the 'Burn to Disc' option from Explorer and prevents the built-in burning wizard from launching. "
                + "Default: absent (burning allowed). Recommended: 1 on managed or public desktops.",
            Tags = ["cd", "burning", "optical", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes Explorer-native disc burning; third-party burning tools are unaffected.",
            ApplyOps = [RegOp.SetDword(BurnKey, "NoBurning", 1)],
            RemoveOps = [RegOp.DeleteValue(BurnKey, "NoBurning")],
            DetectOps = [RegOp.CheckDword(BurnKey, "NoBurning", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-no-burning-user",
            Label = "Disable CD/DVD Burning (Current User)",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets NoCDBurning=1 in the per-user Explorer policy key. "
                + "Removes the disc-burning shell extension for the current user without machine-wide enforcement. "
                + "Default: absent. Recommended: 1 on shared workstations for non-admin users.",
            Tags = ["cd", "burning", "optical", "policy", "user"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "User-scoped removal of Burn to Disc wizard; reversible without admin rights.",
            ApplyOps = [RegOp.SetDword(ExplCu, "NoCDBurning", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplCu, "NoCDBurning")],
            DetectOps = [RegOp.CheckDword(ExplCu, "NoCDBurning", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-no-burning-explorer-lm",
            Label = "Hide CD Burning in Explorer (Machine Policy)",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets NoCDBurning=1 in the machine-scoped Explorer policy key. "
                + "Suppresses the burn-to-disc task pane and context menu item in Explorer for all users. "
                + "Default: absent. Recommended: 1 in kiosk, classroom, or terminal server deployments.",
            Tags = ["cd", "burning", "explorer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides the Explorer CD-burn UI for all users on this machine.",
            ApplyOps = [RegOp.SetDword(ExplLm, "NoCDBurning", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplLm, "NoCDBurning")],
            DetectOps = [RegOp.CheckDword(ExplLm, "NoCDBurning", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-block-cdrom-read",
            Label = "Block CD-ROM Read Access",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets Deny_Read=1 in the CD-ROM removable storage device class policy (GUID {53f56308}). "
                + "Prevents all read operations from CD-ROM drives via the OS removable storage access policy layer. "
                + "Default: absent (read allowed). Recommended: 1 only in air-gapped environments where optical media is prohibited.",
            Tags = ["cd", "read", "removable", "optical", "restriction"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Completely blocks CD-ROM read access; breaks all optical disc software — use with caution.",
            ApplyOps = [RegOp.SetDword(CdRomKey, "Deny_Read", 1)],
            RemoveOps = [RegOp.DeleteValue(CdRomKey, "Deny_Read")],
            DetectOps = [RegOp.CheckDword(CdRomKey, "Deny_Read", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-block-cdrom-write",
            Label = "Block CD-ROM Write Access",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets Deny_Write=1 in the CD-ROM device class policy. "
                + "Prevents write operations to CD-R/RW drives via the removable storage access layer. "
                + "Default: absent (write allowed). Recommended: 1 for data-loss-prevention on managed desktops.",
            Tags = ["cd", "write", "removable", "optical", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks CD-ROM writes at the device access policy layer.",
            ApplyOps = [RegOp.SetDword(CdRomKey, "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(CdRomKey, "Deny_Write")],
            DetectOps = [RegOp.CheckDword(CdRomKey, "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-block-cdrom-execute",
            Label = "Block CD-ROM Execute (AutoRun)",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets Deny_Execute=1 in the CD-ROM device class policy. "
                + "Prevents direct execution of content from CD-ROM drives via the removable storage access layer. "
                + "Default: absent. Recommended: 1 on security-hardened systems processing untrusted optical media.",
            Tags = ["cd", "execute", "autorun", "removable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks auto-execute from CD-ROM; disc content is still readable via explicit app launch.",
            ApplyOps = [RegOp.SetDword(CdRomKey, "Deny_Execute", 1)],
            RemoveOps = [RegOp.DeleteValue(CdRomKey, "Deny_Execute")],
            DetectOps = [RegOp.CheckDword(CdRomKey, "Deny_Execute", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-block-dvd-read",
            Label = "Block DVD Read Access",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets Deny_Read=1 in the DVD/BD removable storage device class policy (GUID {53f56307}). "
                + "Prevents all read access to DVD and Blu-ray drives. "
                + "Default: absent. Recommended: 1 only in air-gapped environments where optical media is prohibited.",
            Tags = ["dvd", "read", "removable", "optical", "restriction"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Completely blocks DVD/BD read access; breaks all optical disc software including media players.",
            ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Read", 1)],
            RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Read")],
            DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Read", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-block-dvd-write",
            Label = "Block DVD Write Access",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets Deny_Write=1 in the DVD device class policy. "
                + "Prevents all write operations to DVD±R/RW drives via the OS removable storage policy layer. "
                + "Default: absent (write allowed). Recommended: 1 for data-exfiltration prevention.",
            Tags = ["dvd", "write", "removable", "optical", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks DVD write at the device access policy layer; requires re-enable for disc authoring.",
            ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Write")],
            DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-block-dvd-execute",
            Label = "Block DVD Execute (AutoRun)",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets Deny_Execute=1 in the DVD device class policy. "
                + "Prevents the system from auto-executing content directly from DVD drives via the removable storage access layer. "
                + "Default: absent. Recommended: 1 for security hardening against malicious autoplay content on optical media.",
            Tags = ["dvd", "execute", "autorun", "removable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks DVD auto-execution; manually launched DVD media apps still work.",
            ApplyOps = [RegOp.SetDword(DvdKey, "Deny_Execute", 1)],
            RemoveOps = [RegOp.DeleteValue(DvdKey, "Deny_Execute")],
            DetectOps = [RegOp.CheckDword(DvdKey, "Deny_Execute", 1)],
        },
        new TweakDef
        {
            Id = "cdbp-no-autoplay-nonvolume",
            Label = "Suppress AutoPlay for Non-Volume Optical Media",
            Category = "CD & Optical Media Policy",
            Description =
                "Sets NoAutoplayfornonVolume=1 in the machine Explorer policy. "
                + "Prevents Windows from automatically opening or showing the AutoPlay dialog when non-volume media "
                + "(audio CDs, video DVDs, mixed-mode discs) is inserted. "
                + "Default: absent (auto-prompt active). Recommended: 1 to reduce unwanted UI interruptions.",
            Tags = ["cd", "dvd", "autoplay", "prompt", "explorer", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Silences the 'What do you want to do?' prompt for non-volume optical discs.",
            ApplyOps = [RegOp.SetDword(ExplLm, "NoAutoplayfornonVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(ExplLm, "NoAutoplayfornonVolume")],
            DetectOps = [RegOp.CheckDword(ExplLm, "NoAutoplayfornonVolume", 1)],
        },
    ];
}
