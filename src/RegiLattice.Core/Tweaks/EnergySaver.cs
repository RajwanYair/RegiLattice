// RegiLattice.Core — Tweaks/EnergySaver.cs
// Windows 11 Energy Saver mode and per-session efficiency settings (Win11 23H2+).
// Uses slug "energy" — focuses on the NEW unified Energy Saver feature (not the
// classic power plan tweaks already in PowerManagement.cs / "pwrmgmt-").
// All paths here are distinct from PowerManagement.cs.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EnergySaver
{
    private const string EnSaver = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\EnergySaver";
    private const string EnSaverSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\EnergySaver";

    // Battery care / charge-limit feature (Win11 24H2+ on supported hardware)
    private const string BatteryCare = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power";

    // Efficiency Mode (background process throttling, Win11 22H2+)
    private const string EcoQos = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
    private const string EcoPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    // Aggressive low-latency GPU scheduler (Win10 2004+)
    private const string Gpu = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";

    // CPU idle states (deeper than regular power management, specifically for Efficiency Cores)
    private const string CpuIdle =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583";
    private const string TimedRes = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power";
    private const string BrightPol = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings";
    private const string BattPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\245d8541-3943-4422-b025-13a784f679b7";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "energy-disable-energy-saver-brightness-reduction",
            Label = "Disable Energy Saver Screen Dimming",
            Category = "Energy Saver",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["energy saver", "brightness", "battery", "display"],
            Description =
                "Prevents the Win11 Energy Saver mode from automatically reducing "
                + "display brightness. Screen brightness stays constant regardless of "
                + "the battery/power state.",
            ApplyOps = [RegOp.SetDword(EnSaver, "BrightnessDimEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaver, "BrightnessDimEnabled")],
            DetectOps = [RegOp.CheckDword(EnSaver, "BrightnessDimEnabled", 0)],
        },
        new TweakDef
        {
            Id = "energy-set-energy-saver-threshold",
            Label = "Set Energy Saver Auto-Activate Threshold to 30%",
            Category = "Energy Saver",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "power"],
            Description =
                "Configures the battery percentage (30%) at which Energy Saver "
                + "activates automatically.  Lower than the default 20% to preserve "
                + "battery longer. Set to 0 to disable auto-activation.",
            ApplyOps = [RegOp.SetDword(EnSaver, "BatteryLifePercent", 30)],
            RemoveOps = [RegOp.DeleteValue(EnSaver, "BatteryLifePercent")],
            DetectOps = [RegOp.CheckDword(EnSaver, "BatteryLifePercent", 30)],
        },
        new TweakDef
        {
            Id = "energy-disable-energy-saver-on-ac",
            Label = "Disable Energy Saver on AC Power",
            Category = "Energy Saver",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["energy saver", "power", "ac", "performance"],
            Description =
                "Prevents Energy Saver from activating while the laptop is plugged "
                + "in, ensuring full performance is available on AC even if battery is low.",
            ApplyOps = [RegOp.SetDword(EnSaver, "OnPluggedIn", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaver, "OnPluggedIn")],
            DetectOps = [RegOp.CheckDword(EnSaver, "OnPluggedIn", 0)],
        },
        new TweakDef
        {
            Id = "energy-disable-background-activity-manager",
            Label = "Disable Background Activity Manager Throttling",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "background", "performance"],
            Description =
                "Stops the Background Activity Manager (BAM) from throttling "
                + "background processes when Energy Saver activates. Prevents game "
                + "launchers and steam from stalling when running in the background.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "BackgroundActivityThrottle", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "BackgroundActivityThrottle")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "BackgroundActivityThrottle", 0)],
        },
        new TweakDef
        {
            Id = "energy-enable-hardware-accelerated-gpu-scheduling",
            Label = "Enable Hardware-Accelerated GPU Scheduling (HAGS)",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["gaming", "gpu", "performance", "latency", "hags"],
            Description =
                "Enables HAGS — moves GPU memory management from the CPU driver to "
                + "the GPU hardware, reducing scheduling latency. Requires a DX12 GPU "
                + "with WDDM 2.7+ driver (most modern NVIDIA/AMD/Intel cards). Reboot required.",
            ApplyOps = [RegOp.SetDword(Gpu, "HwSchMode", 2)],
            RemoveOps = [RegOp.DeleteValue(Gpu, "HwSchMode")],
            DetectOps = [RegOp.CheckDword(Gpu, "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "energy-set-timer-resolution-highest",
            Label = "Increase System Timer Resolution (1 ms)",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["performance", "gaming", "latency", "timer", "scheduler"],
            Description =
                "Sets the global Windows timer resolution to 1 ms by configuring "
                + "the minimum timer period. Improves game frame timing and reduces "
                + "sleep() over-shoot in real-time applications. "
                + "Slightly increases CPU wake-up frequency — disable on battery.",
            ApplyOps = [RegOp.SetDword(TimedRes, "TimerResolution", 10000)],
            RemoveOps = [RegOp.DeleteValue(TimedRes, "TimerResolution")],
            DetectOps = [RegOp.CheckDword(TimedRes, "TimerResolution", 10000)],
        },
        new TweakDef
        {
            Id = "energy-disable-auto-brightness-sensor",
            Label = "Disable Auto-Brightness (Ambient Light Sensor)",
            Category = "Energy Saver",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["energy saver", "brightness", "display", "ambient light"],
            Description =
                "Disables the ambient-light sensor from automatically adjusting "
                + "display brightness. Keeps your chosen brightness constant across "
                + "lighting conditions, avoiding distracting flicker.",
            ApplyOps = [RegOp.SetDword(BrightPol, "EnableAmbientLightSensor", 0)],
            RemoveOps = [RegOp.DeleteValue(BrightPol, "EnableAmbientLightSensor")],
            DetectOps = [RegOp.CheckDword(BrightPol, "EnableAmbientLightSensor", 0)],
        },
        new TweakDef
        {
            Id = "energy-disable-battery-limit-notification",
            Label = "Disable Low-Battery Notification Below 30%",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "notifications"],
            Description =
                "Suppresses the low-battery warning toast that appears when battery "
                + "drops below 30%, useful when you prefer the battery icon for monitoring "
                + "and want to avoid repeated interruptions.",
            ApplyOps = [RegOp.SetDword(BattPolicy, "DCSettingIndex", 0)],
            RemoveOps = [RegOp.DeleteValue(BattPolicy, "DCSettingIndex")],
            DetectOps = [RegOp.CheckDword(BattPolicy, "DCSettingIndex", 0)],
        },
        new TweakDef
        {
            Id = "energy-enable-efficiency-mode-background",
            Label = "Enable Windows Efficiency Mode for Background Apps",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "efficiency mode", "background", "battery", "performance"],
            Description =
                "Enables the Windows 11 Efficiency Mode flag globally, which routes "
                + "non-foreground processes to the CPU's efficiency cores (E-cores on "
                + "Intel 12th+), extending battery life without impacting foreground tasks.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "EnableEfficiencyMode", 1)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "EnableEfficiencyMode")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "EnableEfficiencyMode", 1)],
        },
        new TweakDef
        {
            Id = "energy-disable-promotional-notifications",
            Label = "Disable Windows Energy Saver Promotional Notifications",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "notifications", "debloat"],
            Description =
                "Prevents the Energy Saver feature from displaying promotional "
                + "notifications suggesting Energy Saver upgrades, Windows 365, or "
                + "Azure-linked power plans.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "PromoNotificationsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "PromoNotificationsEnabled")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "PromoNotificationsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "energy-set-saver-threshold-15pct",
            Label = "Set Energy Saver Auto-Activate Threshold to 15%",
            Category = "Energy Saver",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "threshold"],
            Description =
                "Lowers the battery percentage at which Windows automatically "
                + "activates Energy Saver mode from the default 20% to 15%, "
                + "giving you more unplugged run time at full performance.",
            ApplyOps = [RegOp.SetDword(EnSaver, "EnergySaverThreshold", 15)],
            RemoveOps = [RegOp.DeleteValue(EnSaver, "EnergySaverThreshold")],
            DetectOps = [RegOp.CheckDword(EnSaver, "EnergySaverThreshold", 15)],
        },
        new TweakDef
        {
            Id = "energy-disable-auto-saver",
            Label = "Disable Automatic Energy Saver Activation",
            Category = "Energy Saver",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "auto"],
            Description =
                "Prevents Windows from turning on Energy Saver mode automatically "
                + "when the battery drops below the configured threshold. "
                + "You retain full control over when Energy Saver is active.",
            ApplyOps = [RegOp.SetDword(EnSaver, "AutoEnergySaverEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaver, "AutoEnergySaverEnabled")],
            DetectOps = [RegOp.CheckDword(EnSaver, "AutoEnergySaverEnabled", 0)],
        },
        new TweakDef
        {
            Id = "energy-disable-saver-dim-amount",
            Label = "Set Energy Saver Screen Dim Amount to Zero",
            Category = "Energy Saver",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["energy saver", "brightness", "battery", "display"],
            Description =
                "Configures the Energy Saver dimming amount to 0% so the screen "
                + "stays at its current brightness even while Energy Saver is active, "
                + "superseding the binary on/off tweak for finer control.",
            ApplyOps = [RegOp.SetDword(EnSaver, "BrightnessDimAmount", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaver, "BrightnessDimAmount")],
            DetectOps = [RegOp.CheckDword(EnSaver, "BrightnessDimAmount", 0)],
        },
        new TweakDef
        {
            Id = "energy-enable-battery-care",
            Label = "Enable Battery Care (Charge Limit) Feature",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery care", "battery health", "charging"],
            Description =
                "Activates the Windows 11 24H2+ Battery Care feature which limits "
                + "maximum charge to preserve long-term battery health. "
                + "Only effective on laptops whose manufacturer exposes this capability.",
            ApplyOps = [RegOp.SetDword(BatteryCare, "BatteryCareEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(BatteryCare, "BatteryCareEnabled")],
            DetectOps = [RegOp.CheckDword(BatteryCare, "BatteryCareEnabled", 1)],
        },
        new TweakDef
        {
            Id = "energy-set-charge-limit-80",
            Label = "Set Battery Charge Limit to 80%",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery care", "battery health", "charging", "80 percent"],
            Description =
                "Caps the maximum battery charge at 80% via the Windows Battery Care "
                + "registry key. Lithium-ion cells degrade faster above 80%, so this "
                + "significantly extends the long-term capacity of your battery.",
            ApplyOps = [RegOp.SetDword(BatteryCare, "ChargingThreshold", 80)],
            RemoveOps = [RegOp.DeleteValue(BatteryCare, "ChargingThreshold")],
            DetectOps = [RegOp.CheckDword(BatteryCare, "ChargingThreshold", 80)],
        },
        new TweakDef
        {
            Id = "energy-disable-background-tasks-battery",
            Label = "Disable Background App Tasks While on Battery",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "background apps", "efficiency"],
            Description =
                "Instructs the system-level Energy Saver service to suspend "
                + "non-critical background application tasks when on battery power, "
                + "reducing CPU wake-ups and extending run time.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "BackgroundTasksOnBattery", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "BackgroundTasksOnBattery")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "BackgroundTasksOnBattery", 0)],
        },
        new TweakDef
        {
            Id = "energy-disable-network-on-saver",
            Label = "Restrict Network Activity During Energy Saver",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "network", "background"],
            Description =
                "Blocks non-critical network traffic (telemetry, background sync) "
                + "while Energy Saver is active. Does not affect foreground network "
                + "connections — only background Windows services.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "NetworkActivityOnBattery", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "NetworkActivityOnBattery")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "NetworkActivityOnBattery", 0)],
        },
        new TweakDef
        {
            Id = "energy-disable-push-sync-on-saver",
            Label = "Disable Cloud Sync During Energy Saver",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "sync", "onedrive", "background"],
            Description =
                "Prevents Windows from triggering push-sync operations (OneDrive, "
                + "Mail, Calendar) while Energy Saver is active, reducing disk and "
                + "network wake activity on battery.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "DisableSyncOnEnergySaver", 1)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "DisableSyncOnEnergySaver")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "DisableSyncOnEnergySaver", 1)],
        },
        new TweakDef
        {
            Id = "energy-disable-location-on-saver",
            Label = "Disable Location Services During Energy Saver",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "location", "gps", "privacy"],
            Description =
                "Suspends background location polling by non-foreground apps "
                + "while Energy Saver is active, preserving GPS/cellular radio "
                + "power on mobile devices.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "DisableLocationOnEnergySaver", 1)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "DisableLocationOnEnergySaver")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "DisableLocationOnEnergySaver", 1)],
        },
        new TweakDef
        {
            Id = "energy-disable-battery-reminder",
            Label = "Disable Energy Saver Battery Reminder Notifications",
            Category = "Energy Saver",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["energy saver", "battery", "notifications"],
            Description =
                "Stops the system-level Energy Saver engine from generating "
                + "reminder notifications asking you to plug in or switch to "
                + "Energy Saver mode.",
            ApplyOps = [RegOp.SetDword(EnSaverSys, "BatteryReminderEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EnSaverSys, "BatteryReminderEnabled")],
            DetectOps = [RegOp.CheckDword(EnSaverSys, "BatteryReminderEnabled", 0)],
        },
    ];
}
