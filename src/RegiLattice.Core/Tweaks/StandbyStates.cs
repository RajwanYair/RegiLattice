#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class StandbyStates
{
    private const string PowerKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power";
    private const string SleepSubgroupKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20";
    private const string HybridSleepKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\94ac6d29-73ce-41a6-809f-6363ba21b47e";
    private const string SleepIdleTimeoutKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\7bc4a2f9-d8dc-4773-825a-960089e11a71";
    private const string WakeTimersKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\bd3b718a-0680-4d9d-8ab2-e1d2b4ac806d";
    private const string AllowStandbyStatesKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\abfc2519-3608-4c2a-94ea-171b0ed546ab";
    private const string SysUnattendedKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\7bc4a2f9-d8dc-4773-825a-960089e11a71";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "standby-disable-connected-standby",
            Label = "Disable Modern Standby (S0 Low Power Idle) — Force S3 Sleep",
            Category = "Sleep & Standby",
            Description =
                "Disables Windows Modern Standby (S0ix/Connected Standby) by setting CsEnabled=0 and PlatformAoAcOverride=0. Forces the system to use the traditional ACPI S3 sleep state, which fully suspends the CPU and eliminates network/push drain during sleep.",
            Tags = ["power", "sleep", "modern-standby", "s3", "laptop"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote =
                "Eliminates battery drain during sleep from network/background activity; trade-off is slower wake and no push notifications during sleep.",
            ApplyOps = [RegOp.SetDword(PowerKey, "CsEnabled", 0), RegOp.SetDword(PowerKey, "PlatformAoAcOverride", 0)],
            RemoveOps = [RegOp.DeleteValue(PowerKey, "CsEnabled"), RegOp.DeleteValue(PowerKey, "PlatformAoAcOverride")],
            DetectOps = [RegOp.CheckDword(PowerKey, "CsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "standby-disable-hybrid-sleep",
            Label = "Disable Hybrid Sleep for Clean S3 Sleep State",
            Category = "Sleep & Standby",
            Description =
                "Disables Hybrid Sleep (which writes a hibernate file before sleeping) for DC power. This reduces the sleep transition time and disk write overhead while still allowing regular hibernate separately.",
            Tags = ["power", "sleep", "hybrid-sleep", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Faster sleep transitions; reduced disk writes per sleep. If power is lost during sleep, session is not recoverable.",
            ApplyOps = [RegOp.SetDword(HybridSleepKey, "ACSettingIndex", 0), RegOp.SetDword(HybridSleepKey, "DCSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(HybridSleepKey, "ACSettingIndex"), RegOp.DeleteValue(HybridSleepKey, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(HybridSleepKey, "DCSettingIndex", 0)],
        },
        new TweakDef
        {
            Id = "standby-expose-hybrid-sleep-setting",
            Label = "Expose Hybrid Sleep Setting in Power Options UI",
            Category = "Sleep & Standby",
            Description =
                "Unhides the Hybrid Sleep option (Attributes=2) in the Power Options advanced settings dialog, allowing per-plan control over whether the system writes a hibernate image before sleeping.",
            Tags = ["power", "sleep", "hybrid-sleep", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Makes Hybrid Sleep configurable per-plan in the Power Options UI.",
            ApplyOps = [RegOp.SetDword(HybridSleepKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(HybridSleepKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(HybridSleepKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "standby-expose-wake-timers",
            Label = "Expose Wake Timers Setting in Power Options UI",
            Category = "Sleep & Standby",
            Description =
                "Unhides the Wake Timers setting in Power Options advanced settings (Attributes=2), allowing per-plan configuration of whether scheduled tasks and maintenance wake the system from sleep.",
            Tags = ["power", "sleep", "wake", "timers", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Exposes wake timer control in Power Options UI.",
            ApplyOps = [RegOp.SetDword(WakeTimersKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(WakeTimersKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(WakeTimersKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "standby-expose-allow-standby-states",
            Label = "Expose Allow Standby States Setting in Power Options UI",
            Category = "Sleep & Standby",
            Description =
                "Unhides the Allow Standby States (S1-S3) power setting in the advanced Power Options dialog, giving users direct control over which ACPI sleep states are permitted.",
            Tags = ["power", "sleep", "acpi", "standby", "ui"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Exposes ACPI standby state allowance setting in UI.",
            ApplyOps = [RegOp.SetDword(AllowStandbyStatesKey, "Attributes", 2)],
            RemoveOps = [RegOp.SetDword(AllowStandbyStatesKey, "Attributes", 18)],
            DetectOps = [RegOp.CheckDword(AllowStandbyStatesKey, "Attributes", 2)],
        },
        new TweakDef
        {
            Id = "standby-sleep-subgroup-visible",
            Label = "Ensure Sleep Subgroup is Visible in Power Options",
            Category = "Sleep & Standby",
            Description =
                "Sets Attributes=0 on the Sleep subgroup in Power Settings registry to ensure the Sleep section is always visible in the Power Options advanced settings dialog.",
            Tags = ["power", "sleep", "ui", "standby"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Restores Sleep group visibility in Power Options advanced settings.",
            ApplyOps = [RegOp.SetDword(SleepSubgroupKey, "Attributes", 0)],
            RemoveOps = [RegOp.SetDword(SleepSubgroupKey, "Attributes", 0)],
            DetectOps = [RegOp.CheckDword(SleepSubgroupKey, "Attributes", 0)],
        },
        new TweakDef
        {
            Id = "standby-hiberfile-100pct",
            Label = "Set Hibernate File Size to 100% of RAM",
            Category = "Sleep & Standby",
            Description =
                "Configures Windows to reserve disk space equal to 100% of installed RAM for the hibernate file (HiberFileSizePercent=100). Ensures sufficient space for full hibernation, preventing failed sleep-to-hibernate transitions on systems with high RAM usage.",
            Tags = ["power", "sleep", "hibernate", "disk"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Guarantees successful hibernation regardless of RAM usage; requires disk space equal to RAM.",
            ApplyOps = [RegOp.SetDword(PowerKey, "HiberFileSizePercent", 100)],
            RemoveOps = [RegOp.SetDword(PowerKey, "HiberFileSizePercent", 75)],
            DetectOps = [RegOp.CheckDword(PowerKey, "HiberFileSizePercent", 100)],
        },
        new TweakDef
        {
            Id = "standby-allow-s1-s2-states",
            Label = "Allow ACPI S1/S2 Sleep States (Power-on Suspend)",
            Category = "Sleep & Standby",
            Description =
                "Enables ACPI S1 (Power-On Suspend) and S2 states for systems whose firmware supports them. These shallow sleep states provide faster wake at the cost of slightly higher power consumption compared to S3.",
            Tags = ["power", "sleep", "acpi", "s1", "s2"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Enables shallower ACPI sleep states where supported; faster wake time.",
            ApplyOps = [RegOp.SetDword(AllowStandbyStatesKey, "ACSettingIndex", 1)],
            RemoveOps = [RegOp.DeleteValue(AllowStandbyStatesKey, "ACSettingIndex")],
            DetectOps = [RegOp.CheckDword(AllowStandbyStatesKey, "ACSettingIndex", 1)],
        },
        new TweakDef
        {
            Id = "standby-disable-dc-hibernation-timeout",
            Label = "Disable Automatic Hibernate Timeout on Battery",
            Category = "Sleep & Standby",
            Description =
                "Sets the hibernate idle timeout to 0 (never) on DC (battery) power. Prevents Windows from automatically transitioning from sleep to hibernate after a set idle period, which can disrupt workloads on battery-powered devices.",
            Tags = ["power", "sleep", "hibernate", "battery", "timeout"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Prevents auto-hibernate after sleep on battery; user controls hibernate explicitly.",
            ApplyOps = [RegOp.SetDword(SleepIdleTimeoutKey, "DCSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(SleepIdleTimeoutKey, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(SleepIdleTimeoutKey, "DCSettingIndex", 0)],
        },
        new TweakDef
        {
            Id = "standby-enable-wake-timers-ac",
            Label = "Allow Wake Timers on AC Power for Scheduled Maintenance",
            Category = "Sleep & Standby",
            Description =
                "Enables wake timers (ACSettingIndex=1) on AC power so Windows Update, scheduled tasks, and maintenance jobs can wake the system at their scheduled times when plugged in. Disabled on battery to prevent drain.",
            Tags = ["power", "sleep", "wake", "timers", "maintenance", "ac"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Allows scheduled tasks to wake the system when on AC; maintenance runs as scheduled while sleeping.",
            ApplyOps = [RegOp.SetDword(WakeTimersKey, "ACSettingIndex", 1), RegOp.SetDword(WakeTimersKey, "DCSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(WakeTimersKey, "ACSettingIndex"), RegOp.DeleteValue(WakeTimersKey, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(WakeTimersKey, "ACSettingIndex", 1)],
        },
    ];
}
