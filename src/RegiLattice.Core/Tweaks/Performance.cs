namespace RegiLattice.Core.Tweaks;

// Sprint B.2: attribute-based module discovery sample

using RegiLattice.Core.Models;

[TweakModule]
internal static class Power
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "power-disable-hibernation",
            Label = "Disable Hibernation",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables hibernation and removes the hiberfil.sys file.",
            Tags = ["power", "disk", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberFileSizePercent", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberFileSizePercent", 75),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-fast-startup",
            Label = "Disable Fast Startup",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Fast Startup (hybrid boot) which can cause driver and dual-boot issues.",
            Tags = ["power", "boot", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberbootEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberbootEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberbootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-connected-standby",
            Label = "Disable Connected Standby",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Modern/Connected Standby which can cause high battery drain and wake-from-sleep issues on some laptops.",
            Tags = ["power", "standby", "laptop", "battery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-sleep-ac",
            Label = "Disable Auto-Sleep on AC Power",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the PC from automatically sleeping while on AC power.",
            Tags = ["power", "sleep", "desktop"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da",
                    "ValueMax",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da",
                    "ValueMax",
                    1800
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da",
                    "ValueMax",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "power-disable-disk-idle",
            Label = "Disable Disk Idle Timeout",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents hard drives from spinning down when idle. Reduces wake latency.",
            Tags = ["power", "disk", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 20)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-adaptive-brightness",
            Label = "Disable Adaptive Display Brightness",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables adaptive display brightness that adjusts screen brightness based on ambient light sensor readings. Default: enabled (1). Recommended: disabled on desktops or when manual brightness control is preferred.",
            Tags = ["power", "brightness", "display", "sensor"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI", "AllowAdaptiveBrightness", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI", "AllowAdaptiveBrightness", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI", "AllowAdaptiveBrightness", 0),
            ],
        },
        new TweakDef
        {
            Id = "power-pwr-pcie-link-pm-off",
            Label = "Disable PCI Express Link State Power Management",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Opts out of Active State Power Management (ASPM) for PCI Express devices. Prevents link-state power saving that can cause latency. Default: ASPM enabled. Recommended: Disabled for low-latency.",
            Tags = ["power", "pcie", "aspm", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI", "ASPMOptOut", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI", "ASPMOptOut")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI", "ASPMOptOut", 1)],
        },
        new TweakDef
        {
            Id = "power-pwr-disable-idle-states",
            Label = "Disable Processor Idle States",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables energy estimation and enables exit latency checking to prevent deep processor idle states (C-states). Maximises CPU responsiveness. Default: Enabled. Recommended: Disabled for gaming.",
            Tags = ["power", "idle", "c-states", "processor", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergyEstimationEnabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "ExitLatencyCheckEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergyEstimationEnabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "ExitLatencyCheckEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergyEstimationEnabled", 0)],
        },
        new TweakDef
        {
            Id = "power-set-high-performance-plan",
            Label = "Set Active Power Plan to High Performance",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the active power scheme to High Performance (8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c). Default: Balanced.",
            Tags = ["power", "plan", "high-performance", "scheme"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes",
                    "ActivePowerScheme",
                    "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes",
                    "ActivePowerScheme",
                    "381b4222-f694-41f0-9685-ff5bb260df2e"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes",
                    "ActivePowerScheme",
                    "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"
                ),
            ],
        },
        new TweakDef
        {
            Id = "power-disable-hard-disk-idle-timeout",
            Label = "Disable Hard Disk Idle Turn Off",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from spinning down hard disks after idle. Avoids seek delays on HDDs. Default: 20 minutes.",
            Tags = ["power", "hard-disk", "idle", "spin-down"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c",
                    "ACSetting",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c",
                    "ACSetting",
                    1200
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c",
                    "ACSetting",
                    0
                ),
            ],
        },
        // ── Command-based power tweaks (powercfg) ──────────────────────────
        new TweakDef
        {
            Id = "power-ultimate-performance-plan",
            Label = "Enable Ultimate Performance Power Plan (powercfg)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Unhides and activates the Ultimate Performance power plan via powercfg. Provides maximum performance by disabling power-saving features. Available on Win10 1803+.",
            Tags = ["power", "plan", "ultimate", "performance", "powercfg"],
            KindHint = TweakKind.SystemCommand,
            MinBuild = 17134, // Win10 1803
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                // Duplicate the Ultimate Performance plan to make it visible
                var (code, stdout, _) = Elevation.RunElevated("powercfg", ["/duplicatescheme", "e9a42b02-d5df-448d-aa00-03f14749eb61"]);
                if (code == 0 && stdout.Length > 0)
                {
                    // Extract GUID from output and set as active
                    var guidStart = stdout.IndexOf('{');
                    var guidEnd = stdout.IndexOf('}');
                    if (guidStart >= 0 && guidEnd > guidStart)
                    {
                        var guid = stdout[(guidStart + 1)..guidEnd];
                        Elevation.RunElevated("powercfg", ["/setactive", guid]);
                    }
                }
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                // Revert to Balanced plan
                Elevation.RunElevated("powercfg", ["/setactive", "381b4222-f694-41f0-9685-ff5bb260df2e"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("powercfg", ["/getactivescheme"]);
                return stdout.Contains("Ultimate", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "power-disable-hibernate",
            Label = "Disable Hibernation (powercfg)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables hibernation and removes the hiberfil.sys file, freeing disk space equal to RAM size.",
            Tags = ["power", "hibernate", "disk", "space", "powercfg"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Removes hiberfil.sys. Fast Startup will also be disabled.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("powercfg", ["/hibernate", "off"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("powercfg", ["/hibernate", "on"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("powercfg", ["/availablesleepstates"]);
                return !stdout.Contains("Hibernate", StringComparison.OrdinalIgnoreCase)
                    || stdout.Contains("not available", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "power-disable-pci-express-pm",
            Label = "Disable PCI Express ASPM (Driver)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Active State Power Management (ASPM) at the PCI driver level. Prevents PCI Express link power transitions that add latency. Default: enabled.",
            Tags = ["power", "pcie", "aspm", "latency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Pci\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Pci\Parameters", "ASPMOptOut", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Pci\Parameters", "ASPMOptOut")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Pci\Parameters", "ASPMOptOut", 1)],
        },
        new TweakDef
        {
            Id = "power-high-performance-plan",
            Label = "Activate High Performance Plan",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Activates the built-in High Performance power plan via powercfg. Uses GUID 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c. Default: Balanced.",
            Tags = ["power", "plan", "high-performance", "performance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("powercfg", ["/setactive", "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("powercfg", ["/setactive", "381b4222-f694-41f0-9685-ff5bb260df2e"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = Elevation.RunElevated("powercfg", ["/getactivescheme"]);
                return stdout.Contains("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "power-max-processor-turbo",
            Label = "Maximise Processor State (100%)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets maximum processor state to 100% via powercfg. Ensures the CPU can reach full turbo frequency. Default: 100% (confirms setting).",
            Tags = ["power", "cpu", "turbo", "performance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("powercfg", ["/setacvalueindex", "SCHEME_CURRENT", "SUB_PROCESSOR", "PROCTHROTTLEMAX", "100"]);
                Elevation.RunElevated("powercfg", ["/setactive", "SCHEME_CURRENT"]);
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("powercfg", ["/setacvalueindex", "SCHEME_CURRENT", "SUB_PROCESSOR", "PROCTHROTTLEMAX", "100"]);
                Elevation.RunElevated("powercfg", ["/setactive", "SCHEME_CURRENT"]);
            },
            DetectAction = () => true,
        },
        new TweakDef
        {
            Id = "power-no-password-on-resume",
            Label = "No Password on Wake/Resume",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables password requirement when waking from sleep or hibernate. Faster resume at the cost of physical security. Default: password required.",
            Tags = ["power", "password", "resume", "wake", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\0e796bdb-100d-47d6-a2d5-f7d2daa51f51"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\0e796bdb-100d-47d6-a2d5-f7d2daa51f51",
                    "ACSettingIndex",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\0e796bdb-100d-47d6-a2d5-f7d2daa51f51",
                    "DCSettingIndex",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\0e796bdb-100d-47d6-a2d5-f7d2daa51f51",
                    "ACSettingIndex"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\0e796bdb-100d-47d6-a2d5-f7d2daa51f51",
                    "DCSettingIndex"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings\0e796bdb-100d-47d6-a2d5-f7d2daa51f51",
                    "ACSettingIndex",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "power-pwr-disable-usb-selective-suspend",
            Label = "Disable USB Selective Suspend (Hub)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables USB selective suspend at the USB hub driver level. Prevents individual USB ports from being suspended. Default: enabled.",
            Tags = ["power", "usb", "selective-suspend", "hub"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3\HubFlags"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3\HubFlags", "DisableSelectiveSuspend", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3\HubFlags", "DisableSelectiveSuspend")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3\HubFlags", "DisableSelectiveSuspend", 1)],
        },
        new TweakDef
        {
            Id = "power-standby-reserve-grace",
            Label = "Set Standby Reserve Grace Period to 0",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the standby memory reserve grace period to 0 seconds. Causes Windows to reclaim standby memory immediately rather than waiting. Default: system managed.",
            Tags = ["power", "memory", "standby", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "StandbyReserveGracePeriod",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "StandbyReserveGracePeriod"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "StandbyReserveGracePeriod",
                    0
                ),
            ],
        },
    ];
}

// ── Merged from EnergySaver.cs ──────────────────────────────────────────────────

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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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

// ── Merged from PowerManagement.cs ──────────────────────────────────────────────────

internal static class PowerManagement
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string PowerKey = $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pwrmgmt-set-high-performance-plan",
            Label = "Set High Performance Power Plan",
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Id = "pwrmgmt-disable-hard-disk-timeout",
            Label = "Disable Hard Disk Auto Power-Off",
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Category = "Performance",
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
            Id = "pwrmgmt-enable-high-precision-timer",
            Label = "Enable High Precision Event Timer",
            Category = "Performance",
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
            Id = "pwrmgmt-disable-processor-boost",
            Label = "Disable Processor Turbo Boost via Registry",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables CPU turbo boost via performance boost mode registry key. Reduces heat and power on laptops at the cost of peak performance.",
            Tags = ["power", "processor", "turbo", "boost"],
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
        new TweakDef
        {
            Id = "pwrmgmt-disable-processor-idle-promote",
            Label = "Disable Processor Idle Promote Threshold",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets processor idle promote threshold to 100% so the CPU always stays in the shallowest idle state. Maximum responsiveness at the cost of power.",
            Tags = ["power", "processor", "idle", "performance"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\7b224883-b3cc-4d79-819f-8374152cbe7c",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\7b224883-b3cc-4d79-819f-8374152cbe7c",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\7b224883-b3cc-4d79-819f-8374152cbe7c",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\7b224883-b3cc-4d79-819f-8374152cbe7c",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-throttle-states",
            Label = "Disable CPU Throttle States (T-States)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables processor throttle states hidden power setting. Prevents the OS from reducing CPU clock speed for thermal management.",
            Tags = ["power", "throttle", "states", "cpu"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\68f262a7-f621-4069-b9a5-4874169be23c",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\68f262a7-f621-4069-b9a5-4874169be23c",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\68f262a7-f621-4069-b9a5-4874169be23c",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\68f262a7-f621-4069-b9a5-4874169be23c",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-energy-saver",
            Label = "Disable Energy Saver (Battery Saver Override)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Energy Saver feature introduced in Windows 11 24H2. Prevents automatic brightness reduction and performance throttling.",
            Tags = ["power", "energy", "saver", "battery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergySaverEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergySaverEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "EnergySaverEnabled", 0)],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-away-mode",
            Label = "Disable Away Mode",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Away Mode which keeps the system in low-power state while appearing off. Ensures full off or full sleep behaviour.",
            Tags = ["power", "away", "mode", "sleep"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\25dfa149-5dd1-4736-b5ab-e8a37b5b8187",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\25dfa149-5dd1-4736-b5ab-e8a37b5b8187",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\25dfa149-5dd1-4736-b5ab-e8a37b5b8187",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\25dfa149-5dd1-4736-b5ab-e8a37b5b8187",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-set-min-processor-state-100",
            Label = "Set Minimum Processor State to 100%",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the CPU from downclocking below maximum frequency. Ensures consistent single-thread performance at the expense of power efficiency.",
            Tags = ["power", "processor", "frequency", "performance"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\893dee8e-2bef-41e0-89c6-b55d0929964c",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\893dee8e-2bef-41e0-89c6-b55d0929964c",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\893dee8e-2bef-41e0-89c6-b55d0929964c",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\893dee8e-2bef-41e0-89c6-b55d0929964c",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-system-unattended-timeout",
            Label = "Disable System Unattended Sleep Timeout",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the hidden unattended sleep timeout (2 minutes by default). Prevents surprise sleeps after waking from user-initiated wake events.",
            Tags = ["power", "unattended", "sleep", "timeout"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\7bc4a2f9-d8fc-4469-b07b-33eb785aaca0",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\7bc4a2f9-d8fc-4469-b07b-33eb785aaca0",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\7bc4a2f9-d8fc-4469-b07b-33eb785aaca0",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\7bc4a2f9-d8fc-4469-b07b-33eb785aaca0",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-dimmed-display-timeout",
            Label = "Disable Dimmed Display Timeout",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the hidden dimmed display timeout setting. Prevents screen from dimming before the display timeout kicks in.",
            Tags = ["power", "display", "dim", "timeout"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\17aaa29b-8b43-4b94-aafe-35f64daaf1ee",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\17aaa29b-8b43-4b94-aafe-35f64daaf1ee",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\17aaa29b-8b43-4b94-aafe-35f64daaf1ee",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\7516b95f-f776-4464-8c53-06167f40cc99\17aaa29b-8b43-4b94-aafe-35f64daaf1ee",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-lid-close-action",
            Label = "Do Nothing on Lid Close (AC Power)",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the lid close action to 'Do nothing' while on AC power. Keeps the laptop running when the lid is closed (e.g., with external display).",
            Tags = ["power", "lid", "close", "laptop"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\4f971e89-eebd-4455-a8de-9e59040e7347\5ca83367-6e45-459f-a27b-476b1d01c936",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\4f971e89-eebd-4455-a8de-9e59040e7347\5ca83367-6e45-459f-a27b-476b1d01c936",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\4f971e89-eebd-4455-a8de-9e59040e7347\5ca83367-6e45-459f-a27b-476b1d01c936",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\4f971e89-eebd-4455-a8de-9e59040e7347\5ca83367-6e45-459f-a27b-476b1d01c936",
                    "Attributes",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "pwrmgmt-disable-hybrid-sleep",
            Label = "Disable Hybrid Sleep",
            Category = "Performance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables hybrid sleep which combines sleep and hibernate. Uses pure sleep for faster wake. Saves disk space from hiberfil.sys partial dump.",
            Tags = ["power", "hybrid", "sleep", "hibernate"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\94ac6d29-73ce-41a6-809f-6363ba21b47e",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\94ac6d29-73ce-41a6-809f-6363ba21b47e",
                    "Attributes",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\94ac6d29-73ce-41a6-809f-6363ba21b47e",
                    "Attributes",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\94ac6d29-73ce-41a6-809f-6363ba21b47e",
                    "Attributes",
                    2
                ),
            ],
        },
    ];
}
