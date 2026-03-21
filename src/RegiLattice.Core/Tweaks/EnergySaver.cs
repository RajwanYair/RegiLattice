// RegiLattice.Core — Tweaks/EnergySaver.cs
// Windows 11 Energy Saver mode and per-session efficiency settings (Win11 23H2+).
// Uses slug "energy" — focuses on the NEW unified Energy Saver feature (not the
// classic power plan tweaks already in PowerManagement.cs / "pwrmgmt-").
// All paths here are distinct from PowerManagement.cs.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EnergySaver
{
    // Win11 24H2 Energy Saver unified mode
    private const string EnSaver = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\EnergySaver";
    private const string EnSaverSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\EnergySaver";

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
    ];
}
