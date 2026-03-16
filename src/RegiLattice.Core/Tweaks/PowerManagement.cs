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
        new TweakDef
        {
            Id = "pwrmgmt-disable-adaptive-brightness",
            Label = "Disable Adaptive Brightness",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic brightness adjustment based on ambient light sensor.",
            Tags = ["power", "display", "brightness"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SensorState\Sensors", "AdaptiveBrightnessEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SensorState\Sensors", "AdaptiveBrightnessEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\SensorState\Sensors", "AdaptiveBrightnessEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-power-throttling",
            Label = "Disable Power Throttling",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows power throttling to prevent CPU frequency capping on background processes.",
            Tags = ["power", "cpu", "performance", "throttling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-hard-disk-timeout",
            Label = "Disable Hard Disk Auto Power-Off",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Prevents hard disks from spinning down after inactivity. Useful for always-on workstations.",
            Tags = ["power", "disk", "hdd", "spin-down"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_disk", "DISKIDLE", "0"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_disk", "DISKIDLE", "20"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-core-parking",
            Label = "Disable CPU Core Parking (All Cores Active)",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Keeps all CPU cores active by setting minimum core parking percentage to 100%.",
            Tags = ["power", "cpu", "core-parking", "performance"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "CPMINCORES", "100"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_processor", "CPMINCORES", "50"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-set-pci-express-max-performance",
            Label = "PCI Express Link State — Maximum Performance",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables PCI Express Active State Power Management (ASPM) for maximum GPU/NVMe throughput.",
            Tags = ["power", "pcie", "gpu", "nvme", "aspm"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_pciexpress", "ASPM", "0"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_pciexpress", "ASPM", "2"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-display-scaling",
            Label = "Disable Display Power Savings",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables Intel/AMD display power saving features (dimming, adaptive backlight) for consistent brightness.",
            Tags = ["power", "display", "brightness", "intel"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_video", "ADAPTBRIGHT", "0"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("powercfg.exe", ["-setacvalueindex", "scheme_current", "sub_video", "ADAPTBRIGHT", "1"]);
                ShellRunner.Run("powercfg.exe", ["-setactive", "scheme_current"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-processor-idle-demote",
            Label = "Disable CPU Idle Demotion",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the OS from demoting a processor to a deeper C-state, reducing latency spikes.",
            Tags = ["power", "cpu", "latency", "c-state"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-energy-estimation",
            Label = "Disable Energy Estimation Engine",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows energy estimation engine to reduce overhead on battery-powered systems used as desktops.",
            Tags = ["power", "energy", "battery", "overhead"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergyEstimationEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergyEstimationEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergyEstimationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "pwrmgmt-enable-high-precision-timer",
            Label = "Enable High Precision Event Timer",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the HPET timer in BCD for more consistent system timing (lower DPC latency).",
            Tags = ["power", "timer", "hpet", "latency"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "useplatformtick", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "useplatformtick"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("useplatformtick", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-turbo-boost",
            Label = "Disable CPU Turbo Boost (Thermal Control)",
            Category = "Power Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps CPU frequency at base clock by disabling turbo boost. Reduces heat and power consumption.",
            Tags = ["power", "cpu", "turbo", "thermal"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\be337238-0d82-4146-a960-4f3749d470c7",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\be337238-0d82-4146-a960-4f3749d470c7",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\be337238-0d82-4146-a960-4f3749d470c7",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\be337238-0d82-4146-a960-4f3749d470c7",
                    "Attributes",
                    2
                ),
            ],
        },
    ];
}
