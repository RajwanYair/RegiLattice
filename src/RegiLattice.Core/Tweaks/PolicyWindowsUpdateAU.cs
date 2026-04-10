namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyWindowsUpdateAU
{
    private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wuau-disable-auto-update",
            Label = "Disable Automatic Windows Updates",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoAutoUpdate=1 in Windows Update AU policy. Completely disables the "
                + "Automatic Updates client. Updates must be downloaded and installed manually "
                + "via Settings > Windows Update or WSUS. Useful for tightly controlled environments.",
            Tags = ["windows-update", "auto-update", "disable", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 4,
            SafetyRating = 2,
            ImpactNote = "No automatic updates; machine may miss critical security patches.",
            ApplyOps = [RegOp.SetDword(AuKey, "NoAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "NoAutoUpdate")],
            DetectOps = [RegOp.CheckDword(AuKey, "NoAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "wuau-notify-download-and-install",
            Label = "Set WU to Notify Before Download and Install",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AUOptions=2 in Windows Update AU policy. Configures Automatic Updates to "
                + "notify before downloading and installing. The user must approve both the download "
                + "and the installation, giving full manual control over update timing.",
            Tags = ["windows-update", "notify", "download", "install", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "User notified before any download; updates never apply silently.",
            ApplyOps = [RegOp.SetDword(AuKey, "AUOptions", 2)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "AUOptions")],
            DetectOps = [RegOp.CheckDword(AuKey, "AUOptions", 2)],
        },
        new TweakDef
        {
            Id = "wuau-auto-download-notify-install",
            Label = "Set WU to Auto-Download, Notify Before Install",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AUOptions=3 in Windows Update AU policy. Configures Automatic Updates to "
                + "download updates automatically but notify the user before installing. Balances "
                + "timely downloads with user control over reboot timing.",
            Tags = ["windows-update", "auto-download", "notify", "install", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Updates download silently; user decides when to install and reboot.",
            ApplyOps = [RegOp.SetDword(AuKey, "AUOptions", 3)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "AUOptions")],
            DetectOps = [RegOp.CheckDword(AuKey, "AUOptions", 3)],
        },
        new TweakDef
        {
            Id = "wuau-no-auto-reboot-logged-on",
            Label = "Prevent Auto-Reboot When Users Are Logged On",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoAutoRebootWithLoggedOnUsers=1 in Windows Update AU policy. Prevents "
                + "Windows Update from automatically restarting the machine when a user is logged "
                + "on. Avoids data loss from unexpected reboots during active sessions.",
            Tags = ["windows-update", "reboot", "logged-on", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "No surprise reboots while users are working; manual reboot required.",
            ApplyOps = [RegOp.SetDword(AuKey, "NoAutoRebootWithLoggedOnUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "NoAutoRebootWithLoggedOnUsers")],
            DetectOps = [RegOp.CheckDword(AuKey, "NoAutoRebootWithLoggedOnUsers", 1)],
        },
        new TweakDef
        {
            Id = "wuau-schedule-install-day-sunday",
            Label = "Schedule Update Install Day to Sunday",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ScheduledInstallDay=1 in Windows Update AU policy. Schedules automatic "
                + "update installation for every Sunday. Combined with AUOptions=4, this ensures "
                + "updates install on a predictable day during off-hours.",
            Tags = ["windows-update", "schedule", "install-day", "sunday", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Updates install every Sunday; predictable maintenance window.",
            ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallDay", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallDay")],
            DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallDay", 1)],
        },
        new TweakDef
        {
            Id = "wuau-schedule-install-time-3am",
            Label = "Schedule Update Install Time to 03:00",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ScheduledInstallTime=3 in Windows Update AU policy. Schedules automatic "
                + "update installation for 3:00 AM local time. Minimises user disruption by "
                + "running installations during typical off-hours.",
            Tags = ["windows-update", "schedule", "install-time", "3am", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Updates install at 3 AM; machine must be powered on or wake-on-LAN enabled.",
            ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallTime", 3)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallTime")],
            DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallTime", 3)],
        },
        new TweakDef
        {
            Id = "wuau-set-detection-frequency-22h",
            Label = "Set Update Detection Frequency to 22 Hours",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DetectionFrequencyEnabled=1 and DetectionFrequency=22 in Windows Update "
                + "AU policy. Changes the update scan interval from the default (randomised) to "
                + "exactly 22 hours, giving admins a predictable scan cadence.",
            Tags = ["windows-update", "detection", "frequency", "scan", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Update scans every 22 hours; slightly less frequent than default.",
            ApplyOps =
            [
                RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1),
                RegOp.SetDword(AuKey, "DetectionFrequency", 22),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled"),
                RegOp.DeleteValue(AuKey, "DetectionFrequency"),
            ],
            DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequencyEnabled", 1)],
        },
        new TweakDef
        {
            Id = "wuau-disable-featured-software-notifications",
            Label = "Disable Featured Software Update Notifications",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFeaturedSoftware=0 in Windows Update AU policy. Disables notifications "
                + "about optional featured software (Edge promotions, Office trials, etc.) in the "
                + "Windows Update interface. Keeps the update UI focused on security patches only.",
            Tags = ["windows-update", "featured", "software", "notification", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "No featured software promotions in Windows Update; only security updates shown.",
            ApplyOps = [RegOp.SetDword(AuKey, "EnableFeaturedSoftware", 0)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "EnableFeaturedSoftware")],
            DetectOps = [RegOp.CheckDword(AuKey, "EnableFeaturedSoftware", 0)],
        },
        new TweakDef
        {
            Id = "wuau-include-recommended-updates",
            Label = "Include Recommended Updates in Auto-Updates",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IncludeRecommendedUpdates=1 in Windows Update AU policy. Ensures that "
                + "recommended updates (driver updates, optional quality fixes) are installed "
                + "alongside critical and security updates, keeping the system fully patched.",
            Tags = ["windows-update", "recommended", "updates", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Recommended updates auto-installed alongside security updates.",
            ApplyOps = [RegOp.SetDword(AuKey, "IncludeRecommendedUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "IncludeRecommendedUpdates")],
            DetectOps = [RegOp.CheckDword(AuKey, "IncludeRecommendedUpdates", 1)],
        },
        new TweakDef
        {
            Id = "wuau-defer-upgrade-period-30d",
            Label = "Defer Feature Upgrades by 30 Days",
            Category = "Windows Update AU Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DeferUpgradePeriod=30 in Windows Update AU policy. Defers major feature "
                + "upgrades (e.g., Windows 11 24H2 → 25H1) by 30 days, giving IT teams time to "
                + "test compatibility before rollout. Security updates are not deferred.",
            Tags = ["windows-update", "defer", "upgrade", "feature", "policy"],
            RegistryKeys = [AuKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Feature upgrades delayed 30 days; security updates delivered on time.",
            ApplyOps = [RegOp.SetDword(AuKey, "DeferUpgradePeriod", 30)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "DeferUpgradePeriod")],
            DetectOps = [RegOp.CheckDword(AuKey, "DeferUpgradePeriod", 30)],
        },
    ];
}
