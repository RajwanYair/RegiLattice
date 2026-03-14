namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Advanced power management tweaks — power plans, CPU parking, sleep/wake, USB suspend,
/// display timeout, lid close actions, and thermal throttling settings.
/// </summary>
internal static class PowerManagement
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string PowerKey = $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pwrmgmt-disable-fast-startup",
            Label = "Disable Fast Startup (Hybrid Boot)",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Fast Startup which hibernates the kernel. Prevents issues with dual-boot, BIOS updates, and bitlocker.",
            Tags = ["power", "fast-startup", "boot", "hybrid"],
            SideEffects = "Cold boot times will be slightly longer.",
            RegistryKeys = [$@"{PowerKey}\PowerSettings"],
            ApplyOps = [RegOp.SetDword(PowerKey, "HibernateEnabled", 0)],
            RemoveOps = [RegOp.SetDword(PowerKey, "HibernateEnabled", 1)],
            DetectOps = [RegOp.CheckDword(PowerKey, "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-connected-standby",
            Label = "Disable Connected/Modern Standby",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Modern Standby (S0ix) which can cause battery drain and overheating while 'sleeping'.",
            Tags = ["power", "standby", "sleep", "battery"],
            RegistryKeys = [PowerKey],
            ApplyOps = [RegOp.SetDword(PowerKey, "PlatformAoAcOverride", 0)],
            RemoveOps = [RegOp.DeleteValue(PowerKey, "PlatformAoAcOverride")],
            DetectOps = [RegOp.CheckDword(PowerKey, "PlatformAoAcOverride", 0)],
        },
        new TweakDef
        {
            Id = "pwrmgmt-set-high-performance-plan",
            Label = "Set High Performance Power Plan",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Activates the built-in High Performance power plan for maximum CPU performance.",
            Tags = ["power", "performance", "cpu", "plan"],
            ApplyAction = _ => ShellRunner.Run("powercfg.exe", ["/setactive", "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"]),
            RemoveAction = _ => ShellRunner.Run("powercfg.exe", ["/setactive", "381b4222-f694-41f0-9685-ff5bb260df2e"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("powercfg.exe", ["/getactivescheme"]);
                return stdout.Contains("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-cpu-parking",
            Label = "Disable CPU Core Parking",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables core parking so all CPU cores remain active. Improves responsiveness for latency-sensitive apps.",
            Tags = ["power", "cpu", "parking", "performance", "latency"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "CPMINCORES", "100"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "CPMINCORES", "5"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(powercfg /query scheme_current sub_processor CPMINCORES) -match '0x00000064'");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pwrmgmt-set-max-cpu-state-100",
            Label = "Set Maximum CPU State to 100%",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Ensures the CPU can boost to its maximum frequency on both AC and battery.",
            Tags = ["power", "cpu", "boost", "performance"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "PROCTHROTTLEMAX", "100"]);
                ShellRunner.Run("powercfg.exe", ["-setdcvalueindex", "scheme_current", "sub_processor", "PROCTHROTTLEMAX", "100"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "PROCTHROTTLEMAX", "100"]);
                ShellRunner.Run("powercfg.exe", ["-setdcvalueindex", "scheme_current", "sub_processor", "PROCTHROTTLEMAX", "80"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(powercfg /query scheme_current sub_processor PROCTHROTTLEMAX) -match '0x00000064'");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-sleep-timeout-ac",
            Label = "Disable Sleep on AC Power",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets sleep timeout to Never when on AC power.",
            Tags = ["power", "sleep", "desktop", "timeout"],
            ApplyAction = _ => ShellRunner.Run("powercfg.exe", ["-change", "standby-timeout-ac", "0"]),
            RemoveAction = _ => ShellRunner.Run("powercfg.exe", ["-change", "standby-timeout-ac", "30"]),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-screen-timeout-ac",
            Label = "Disable Screen Off on AC Power",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets display timeout to Never when on AC power.",
            Tags = ["power", "display", "timeout", "desktop"],
            ApplyAction = _ => ShellRunner.Run("powercfg.exe", ["-change", "monitor-timeout-ac", "0"]),
            RemoveAction = _ => ShellRunner.Run("powercfg.exe", ["-change", "monitor-timeout-ac", "15"]),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-usb-selective-suspend",
            Label = "Disable USB Selective Suspend",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents USB devices from being suspended to save power. Fixes disconnecting peripherals.",
            Tags = ["power", "usb", "suspend", "peripherals"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\USB"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
        },
        new TweakDef
        {
            Id = "pwrmgmt-set-lid-close-nothing",
            Label = "Set Lid Close Action to Do Nothing",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Closing the laptop lid does nothing on AC power. Useful for clamshell/docking station setups.",
            Tags = ["power", "lid", "laptop", "docking"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_buttons", "LIDACTION", "0"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_buttons", "LIDACTION", "1"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-wake-timers",
            Label = "Disable Wake Timers",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents scheduled tasks and Windows Update from waking the computer from sleep.",
            Tags = ["power", "wake", "sleep", "update"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Power\PowerSettings\BD3B718A-0680-4D9D-8AB2-E1D2B4AC806D"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Power\PowerSettings\BD3B718A-0680-4D9D-8AB2-E1D2B4AC806D", "ACSettingIndex", 0),
                RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Power\PowerSettings\BD3B718A-0680-4D9D-8AB2-E1D2B4AC806D", "DCSettingIndex", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Power\PowerSettings\BD3B718A-0680-4D9D-8AB2-E1D2B4AC806D", "ACSettingIndex"),
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Power\PowerSettings\BD3B718A-0680-4D9D-8AB2-E1D2B4AC806D", "DCSettingIndex"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Policies\Microsoft\Power\PowerSettings\BD3B718A-0680-4D9D-8AB2-E1D2B4AC806D",
                    "ACSettingIndex",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-enable-hibernate-after-sleep",
            Label = "Enable Hibernate After 3 Hours of Sleep",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Hibernates after 3 hours of sleep to prevent data loss from power failure.",
            Tags = ["power", "hibernate", "sleep", "data-protection"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-change", "hibernate-timeout-ac", "180"]);
                ShellRunner.Run("powercfg.exe", ["-change", "hibernate-timeout-dc", "60"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-change", "hibernate-timeout-ac", "0"]);
                ShellRunner.Run("powercfg.exe", ["-change", "hibernate-timeout-dc", "0"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-set-min-cpu-state-5",
            Label = "Set Minimum CPU State to 5% (Power Save)",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Allows the CPU to drop to 5% frequency when idle, maximizing power savings.",
            Tags = ["power", "cpu", "idle", "efficiency"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "PROCTHROTTLEMIN", "5"]);
                ShellRunner.Run("powercfg.exe", ["-setdcvalueindex", "scheme_current", "sub_processor", "PROCTHROTTLEMIN", "5"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "PROCTHROTTLEMIN", "100"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () => false,
        },
    ];
}
