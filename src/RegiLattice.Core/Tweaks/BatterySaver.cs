#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class BatterySaver
{
    private const string BattKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\BatterySaver";
    private const string PowerKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power";
    private const string PwrSessKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power";
    private const string BattPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\e73a048d-bf27-4f12-9731-8b2076e8891f";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "batt-threshold-20",
            Label = "Set Battery Saver Auto-Trigger Threshold to 20%",
            Category = "Battery Saver",
            Description =
                "Configures Windows Battery Saver to activate automatically when the battery reaches 20%, reducing power consumption and extending remaining runtime.",
            Tags = ["battery", "power", "battery-saver", "threshold"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Activates battery saver earlier, extending laptop runtime on low charge.",
            ApplyOps = [RegOp.SetDword(BattKey, "EnergySaverBatteryThreshold", 20)],
            RemoveOps = [RegOp.SetDword(BattKey, "EnergySaverBatteryThreshold", 20)],
            DetectOps = [RegOp.CheckDword(BattKey, "EnergySaverBatteryThreshold", 20)],
        },
        new TweakDef
        {
            Id = "batt-disable-auto-dim",
            Label = "Disable Auto Screen Dimming in Battery Saver",
            Category = "Battery Saver",
            Description =
                "Prevents Windows Battery Saver from automatically reducing screen brightness when activated, useful for users who prefer manual brightness control or use external monitors.",
            Tags = ["battery", "power", "battery-saver", "display", "brightness"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Keeps display brightness unchanged when battery saver activates; slightly less power saving.",
            ApplyOps = [RegOp.SetDword(BattKey, "BatterySaverAutoBrightnessToggle", 0)],
            RemoveOps = [RegOp.SetDword(BattKey, "BatterySaverAutoBrightnessToggle", 1)],
            DetectOps = [RegOp.CheckDword(BattKey, "BatterySaverAutoBrightnessToggle", 0)],
        },
        new TweakDef
        {
            Id = "batt-enable-auto-trigger",
            Label = "Enable Battery Saver Auto-Activation at Threshold",
            Category = "Battery Saver",
            Description =
                "Ensures Battery Saver is configured to automatically activate when the battery reaches the configured threshold, providing automatic power conservation without manual intervention.",
            Tags = ["battery", "power", "battery-saver", "automation"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Automates battery saver activation, extending runtime without user intervention.",
            ApplyOps = [RegOp.SetDword(BattKey, "BatterySaverToggleEnabled", 1)],
            RemoveOps = [RegOp.SetDword(BattKey, "BatterySaverToggleEnabled", 0)],
            DetectOps = [RegOp.CheckDword(BattKey, "BatterySaverToggleEnabled", 1)],
        },
        new TweakDef
        {
            Id = "batt-bg-apps-limited",
            Label = "Limit Background Apps in Battery Saver Mode",
            Category = "Battery Saver",
            Description =
                "Configures Battery Saver to limit background application activity when active, reducing CPU usage from apps that normally run silently in the background.",
            Tags = ["battery", "power", "battery-saver", "background-apps", "performance"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces background CPU/network drain during battery saver mode; some notifications may be delayed.",
            ApplyOps = [RegOp.SetDword(BattKey, "BackgroundAppPolicy", 1)],
            RemoveOps = [RegOp.SetDword(BattKey, "BackgroundAppPolicy", 0)],
            DetectOps = [RegOp.CheckDword(BattKey, "BackgroundAppPolicy", 1)],
        },
        new TweakDef
        {
            Id = "batt-policy-dc-level-20",
            Label = "Policy: Battery Saver DC Threshold 20%",
            Category = "Battery Saver",
            Description =
                "Sets the group policy battery saver threshold for DC (battery) power to 20%. This policy overrides the per-user setting and applies system-wide.",
            Tags = ["battery", "power", "battery-saver", "policy", "gpo"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enforces battery saver at 20% battery across all user accounts.",
            ApplyOps = [RegOp.SetDword(BattPolicyKey, "DCSettingIndex", 20)],
            RemoveOps = [RegOp.DeleteValue(BattPolicyKey, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(BattPolicyKey, "DCSettingIndex", 20)],
        },
        new TweakDef
        {
            Id = "batt-policy-ac-disable",
            Label = "Policy: Disable Battery Saver on AC Power",
            Category = "Battery Saver",
            Description =
                "Sets the group policy to prevent Battery Saver from activating when the device is plugged in (AC power). This ensures full performance when charging.",
            Tags = ["battery", "power", "battery-saver", "policy", "ac"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents battery saver from activating when plugged in; ensures full performance while charging.",
            ApplyOps = [RegOp.SetDword(BattPolicyKey, "ACSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(BattPolicyKey, "ACSettingIndex")],
            DetectOps = [RegOp.CheckDword(BattPolicyKey, "ACSettingIndex", 0)],
        },
        new TweakDef
        {
            Id = "batt-hibernate-enabled",
            Label = "Enable System Hibernate Feature",
            Category = "Battery Saver",
            Description =
                "Enables the Windows hibernate feature, allowing the system to save full RAM state to disk and power off completely. Critical for laptops to prevent battery drain during extended inactivity.",
            Tags = ["battery", "power", "hibernate", "sleep"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Restores hibernate capability; prevents battery drain during extended sleep on laptops.",
            ApplyOps = [RegOp.SetDword(PowerKey, "HibernateEnabled", 1)],
            RemoveOps = [RegOp.SetDword(PowerKey, "HibernateEnabled", 0)],
            DetectOps = [RegOp.CheckDword(PowerKey, "HibernateEnabled", 1)],
        },
        new TweakDef
        {
            Id = "batt-hiberfile-full",
            Label = "Set Hibernate File to Full RAM Image Mode",
            Category = "Battery Saver",
            Description =
                "Configures Windows to create a full hibernate file that stores the complete RAM state (HiberFileType=2). Ensures complete system state restoration after hibernation, preventing data loss on laptops.",
            Tags = ["battery", "power", "hibernate", "disk"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Uses more disk space for hibernate file but ensures 100% faithful state restoration.",
            ApplyOps = [RegOp.SetDword(PowerKey, "HiberFileType", 2)],
            RemoveOps = [RegOp.SetDword(PowerKey, "HiberFileType", 1)],
            DetectOps = [RegOp.CheckDword(PowerKey, "HiberFileType", 2)],
        },
        new TweakDef
        {
            Id = "batt-disable-fast-startup",
            Label = "Disable Fast Startup (Hybrid Boot) for Clean Shutdown",
            Category = "Battery Saver",
            Description =
                "Disables Windows Fast Startup (HibernateOnShutdown/HiberbootEnabled), which stores a kernel session to disk at shutdown. Clean shutdowns ensure proper driver unloading and prevent stale battery state from interfering with charge indicators.",
            Tags = ["battery", "power", "fast-startup", "boot", "shutdown"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Slower startups in exchange for clean shutdowns and accurate battery charge tracking.",
            ApplyOps = [RegOp.SetDword(PwrSessKey, "HiberbootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(PwrSessKey, "HiberbootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(PwrSessKey, "HiberbootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "batt-disable-wake-on-pattern",
            Label = "Disable Wake-on-Pattern (Network/Timer) for Battery",
            Category = "Battery Saver",
            Description =
                "Prevents the system from waking from sleep in response to network packets or scheduled tasks when on battery power. Stops background processes from draining the battery during sleep.",
            Tags = ["battery", "power", "wake", "sleep", "network"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Prevents unexpected wake events that drain the battery during sleep; push notifications still work at next wake.",
            ApplyOps = [RegOp.SetDword(PowerKey, "WakeOnPattern", 0)],
            RemoveOps = [RegOp.SetDword(PowerKey, "WakeOnPattern", 1)],
            DetectOps = [RegOp.CheckDword(PowerKey, "WakeOnPattern", 0)],
        },
    ];
}
