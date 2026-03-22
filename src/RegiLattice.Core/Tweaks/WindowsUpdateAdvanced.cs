// RegiLattice.Core — Tweaks/WindowsUpdateAdvanced.cs
// Advanced Windows Update policy: deferral periods, WSUS, driver updates, delivery optimization.
// Slug: "wuadv" — HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate GPO keys.
// Complements WindowsUpdate.cs which covers the basic WU settings.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsUpdateAdvanced
{
    private const string WuPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    private const string WuAu = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

    private const string DeliveryOpt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wuadv-exclude-driver-updates",
            Label = "Exclude Driver Updates from Windows Update",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["windows update", "drivers", "policy", "quality update"],
            Description =
                "Prevents Windows Update from automatically installing driver updates "
                + "alongside quality/security updates. ExcludeWUDriversInQualityUpdate=1. "
                + "Useful when you manage drivers manually via Device Manager or vendor tools.",
            ApplyOps = [RegOp.SetDword(WuPolicy, "ExcludeWUDriversInQualityUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(WuPolicy, "ExcludeWUDriversInQualityUpdate")],
            DetectOps = [RegOp.CheckDword(WuPolicy, "ExcludeWUDriversInQualityUpdate", 1)],
        },
        new TweakDef
        {
            Id = "wuadv-defer-feature-updates-30-days",
            Label = "Defer Feature (Major) Updates by 30 Days",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["windows update", "feature update", "deferral", "stability"],
            Description =
                "Delays the installation of major Windows feature updates (annual releases) "
                + "by 30 days. DeferFeatureUpdatesPeriodInDays=30. Gives time for early bugs "
                + "in new Windows versions to be patched before your machine upgrades.",
            ApplyOps = [RegOp.SetDword(WuPolicy, "DeferFeatureUpdates", 1), RegOp.SetDword(WuPolicy, "DeferFeatureUpdatesPeriodInDays", 30)],
            RemoveOps = [RegOp.DeleteValue(WuPolicy, "DeferFeatureUpdates"), RegOp.DeleteValue(WuPolicy, "DeferFeatureUpdatesPeriodInDays")],
            DetectOps = [RegOp.CheckDword(WuPolicy, "DeferFeatureUpdatesPeriodInDays", 30)],
        },
        new TweakDef
        {
            Id = "wuadv-defer-quality-updates-7-days",
            Label = "Defer Quality (Security) Updates by 7 Days",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["windows update", "quality update", "security", "deferral"],
            Description =
                "Delays monthly quality (security) updates by 7 days. "
                + "DeferQualityUpdatesPeriodInDays=7. Allows time for faulty patches to be "
                + "identified and pulled before your machine installs them, "
                + "while keeping you close to the security patch baseline.",
            ApplyOps = [RegOp.SetDword(WuPolicy, "DeferQualityUpdates", 1), RegOp.SetDword(WuPolicy, "DeferQualityUpdatesPeriodInDays", 7)],
            RemoveOps = [RegOp.DeleteValue(WuPolicy, "DeferQualityUpdates"), RegOp.DeleteValue(WuPolicy, "DeferQualityUpdatesPeriodInDays")],
            DetectOps = [RegOp.CheckDword(WuPolicy, "DeferQualityUpdatesPeriodInDays", 7)],
        },
        new TweakDef
        {
            Id = "wuadv-block-update-settings-access",
            Label = "Block Standard Users from Accessing Windows Update Settings",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["windows update", "settings", "access control", "admin"],
            Description =
                "Prevents non-administrator users from accessing Windows Update settings. "
                + "SetDisableUXWUAccess=1. Standard users cannot scan for, pause, or configure "
                + "updates. Only administrators can manage the update schedule.",
            ApplyOps = [RegOp.SetDword(WuPolicy, "SetDisableUXWUAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(WuPolicy, "SetDisableUXWUAccess")],
            DetectOps = [RegOp.CheckDword(WuPolicy, "SetDisableUXWUAccess", 1)],
        },
        new TweakDef
        {
            Id = "wuadv-disable-update-reboot-notification",
            Label = "Suppress Forced Reboot Notifications After Updates",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["windows update", "reboot", "notification", "restart"],
            Description =
                "Prevents Windows Update from showing aggressive restart countdown notifications "
                + "after installing updates. SetAutoRestartNotificationConfig=1 (suppress) / "
                + "NoAutoRebootWithLoggedOnUsers=1. Users restart at their own pace.",
            ApplyOps = [RegOp.SetDword(WuAu, "NoAutoRebootWithLoggedOnUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(WuAu, "NoAutoRebootWithLoggedOnUsers")],
            DetectOps = [RegOp.CheckDword(WuAu, "NoAutoRebootWithLoggedOnUsers", 1)],
        },
        new TweakDef
        {
            Id = "wuadv-disable-delivery-optimization",
            Label = "Disable Delivery Optimization (P2P Update Sharing)",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["windows update", "delivery optimization", "p2p", "bandwidth"],
            Description =
                "Disables Windows Delivery Optimization — the P2P update sharing feature that "
                + "uploads update packages to other devices on the LAN or internet. "
                + "DODownloadMode=0 (disabled). Eliminates upload bandwidth usage and privacy "
                + "concerns about sharing data with unknown peers.",
            ApplyOps = [RegOp.SetDword(DeliveryOpt, "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue(DeliveryOpt, "DODownloadMode")],
            DetectOps = [RegOp.CheckDword(DeliveryOpt, "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "wuadv-lan-only-delivery-optimization",
            Label = "Restrict Delivery Optimization to LAN Only",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["windows update", "delivery optimization", "lan", "p2p", "bandwidth"],
            Description =
                "Restricts Delivery Optimization to only share update data with devices on "
                + "the local LAN — not with external internet peers. DODownloadMode=1 (LAN "
                + "only). Allows faster local updates while preventing internet upload.",
            ApplyOps = [RegOp.SetDword(DeliveryOpt, "DODownloadMode", 1)],
            RemoveOps = [RegOp.DeleteValue(DeliveryOpt, "DODownloadMode")],
            DetectOps = [RegOp.CheckDword(DeliveryOpt, "DODownloadMode", 1)],
        },
        new TweakDef
        {
            Id = "wuadv-require-update-signature",
            Label = "Require Code-Signed Updates from WSUS",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["windows update", "wsus", "signing", "security"],
            Description =
                "Requires that all updates from a WSUS server are signed by a trusted publisher "
                + "in the local machine certificate store. UsePolicyBasedQosMarkings=1 is the "
                + "underlying policy; AcceptTrustedPublisherCerts=1 enables the WSUS signing check.",
            ApplyOps = [RegOp.SetDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
            RemoveOps = [RegOp.DeleteValue(WuPolicy, "AcceptTrustedPublisherCerts")],
            DetectOps = [RegOp.CheckDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
        },
        new TweakDef
        {
            Id = "wuadv-allow-mu-updates-with-wu",
            Label = "Enable Microsoft Update (Office + Products) via Windows Update",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["windows update", "microsoft update", "office", "products"],
            Description =
                "Enables Microsoft Update service via the Windows Update policy — allows Office, "
                + "Visual Studio, and other Microsoft products to receive updates through "
                + "Windows Update instead of requiring separate update channels. "
                + "EnableFeaturedSoftware=1.",
            ApplyOps = [RegOp.SetDword(WuAu, "EnableFeaturedSoftware", 1)],
            RemoveOps = [RegOp.DeleteValue(WuAu, "EnableFeaturedSoftware")],
            DetectOps = [RegOp.CheckDword(WuAu, "EnableFeaturedSoftware", 1)],
        },
        new TweakDef
        {
            Id = "wuadv-set-active-hours-start",
            Label = "Set Windows Update Active Hours (8am–8pm)",
            Category = "Windows Update Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["windows update", "active hours", "restart", "schedule"],
            Description =
                "Sets Windows Update active hours to 8am–8pm (hours 8–20). Windows will not "
                + "automatically restart to apply updates during these hours. "
                + "ActiveHoursStart=8, ActiveHoursEnd=20. Prevents disruptive mid-day reboots.",
            ApplyOps =
            [
                RegOp.SetDword(WuPolicy, "SetActiveHours", 1),
                RegOp.SetDword(WuPolicy, "ActiveHoursStart", 8),
                RegOp.SetDword(WuPolicy, "ActiveHoursEnd", 20),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(WuPolicy, "SetActiveHours"),
                RegOp.DeleteValue(WuPolicy, "ActiveHoursStart"),
                RegOp.DeleteValue(WuPolicy, "ActiveHoursEnd"),
            ],
            DetectOps = [RegOp.CheckDword(WuPolicy, "ActiveHoursStart", 8), RegOp.CheckDword(WuPolicy, "ActiveHoursEnd", 20)],
        },
    ];
}
