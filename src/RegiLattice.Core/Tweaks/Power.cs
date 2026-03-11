namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Power
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "power-disable-hibernation",
            Label = "Disable Hibernation",
            Category = "Power",
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
            Id = "power-optimize-proc-scheduling",
            Label = "Optimize Processor Scheduling",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Adjusts Win32PrioritySeparation for foreground-app responsiveness (value 38).",
            Tags = ["power", "performance", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
        },
        new TweakDef
        {
            Id = "power-disable-fast-startup",
            Label = "Disable Fast Startup",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Fast Startup (hybrid boot) which can cause driver and dual-boot issues.",
            Tags = ["power", "boot", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberbootEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberbootEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HiberbootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "power-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables large system cache, dedicating more RAM for file caching (beneficial with 16 GB+ RAM).",
            Tags = ["power", "performance", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
        },
        new TweakDef
        {
            Id = "power-disable-power-throttling",
            Label = "Disable Power Throttling",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables CPU power throttling for maximum sustained performance.",
            Tags = ["power", "performance", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
        },
        new TweakDef
        {
            Id = "power-disable-ntfs-last-access",
            Label = "Disable NTFS Last Access Timestamp",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops NTFS from updating last-access timestamps, reducing disk I/O.",
            Tags = ["power", "performance", "disk", "ntfs"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"],
        },
        new TweakDef
        {
            Id = "power-disable-connected-standby",
            Label = "Disable Connected Standby",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Modern/Connected Standby which can cause high battery drain and wake-from-sleep issues on some laptops.",
            Tags = ["power", "standby", "laptop", "battery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-core-parking",
            Label = "Disable CPU Core Parking",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables CPU core parking so all cores stay active. Can improve latency-sensitive workloads and gaming.",
            Tags = ["power", "cpu", "performance", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583", "ValueMax", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583", "ValueMax"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583", "ValueMax", 0)],
        },
        new TweakDef
        {
            Id = "power-max-processor-turbo",
            Label = "Set Max Processor State to 100%",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures the CPU is allowed to reach its maximum turbo frequency.",
            Tags = ["power", "cpu", "turbo", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\be337238-0d82-4146-a960-4f3749d470c7"],
        },
        new TweakDef
        {
            Id = "power-disable-sleep-ac",
            Label = "Disable Auto-Sleep on AC Power",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the PC from automatically sleeping while on AC power.",
            Tags = ["power", "sleep", "desktop"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da", "ValueMax", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da", "ValueMax", 1800),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da", "ValueMax", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-disk-idle",
            Label = "Disable Disk Idle Timeout",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents hard drives from spinning down when idle. Reduces wake latency.",
            Tags = ["power", "disk", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 20),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk", "TimeOutValue", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-usb-suspend",
            Label = "Disable USB Selective Suspend",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables USB selective suspend power management. Prevents USB devices from disconnecting during use. Default: Enabled (0). Recommended: Disabled (1) for desktops.",
            Tags = ["power", "usb", "suspend", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
        },
        new TweakDef
        {
            Id = "power-disable-hdd-powerdown",
            Label = "Disable Hard Disk Power Down",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables hibernation (hiberfil.sys). Frees disk space equal to RAM size and speeds up shutdown. Default: Enabled. Recommended: Disabled for desktops with SSDs.",
            Tags = ["power", "hibernate", "disk", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", 0)],
        },
        new TweakDef
        {
            Id = "power-disable-adaptive-brightness",
            Label = "Disable Adaptive Display Brightness",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables adaptive display brightness that adjusts screen brightness based on ambient light sensor readings. Default: enabled (1). Recommended: disabled on desktops or when manual brightness control is preferred.",
            Tags = ["power", "brightness", "display", "sensor"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI", "AllowAdaptiveBrightness", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI", "AllowAdaptiveBrightness", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SensorAPI", "AllowAdaptiveBrightness", 0)],
        },
        new TweakDef
        {
            Id = "power-high-performance-plan",
            Label = "Set High Performance Power Plan",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the active power plan to High Performance (8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c). Maximizes CPU frequency and disables power-saving features. Remove reverts to Balanced plan.",
            Tags = ["power", "plan", "high-performance", "cpu"],
        },
        new TweakDef
        {
            Id = "power-disable-usb-power-save",
            Label = "Disable USB Power Saving",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables USB selective suspend globally to prevent USB device disconnects. Keeps all USB ports powered at all times. Default: Enabled. Recommended: Disabled for desktop PCs.",
            Tags = ["power", "usb", "suspend", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"],
        },
        new TweakDef
        {
            Id = "power-disable-pci-express-pm",
            Label = "Disable Connected Standby",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Connected Standby (Modern Standby) which manages PCI Express link state power. Prevents low-power idle states that can cause wake issues. Default: Enabled. Recommended: Disabled for desktops.",
            Tags = ["power", "connected-standby", "pci-express", "idle"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
        },
        new TweakDef
        {
            Id = "power-pwr-disable-usb-selective-suspend",
            Label = "Disable USB 3.0 Selective Suspend",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables selective suspend on USB 3.0 hubs (USBHUB3 driver). Prevents USB 3.0 device disconnects during idle. Default: Enabled. Recommended: Disabled for desktop PCs.",
            Tags = ["power", "usb", "selective-suspend", "usb3"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3\Parameters"],
        },
        new TweakDef
        {
            Id = "power-pwr-pcie-link-pm-off",
            Label = "Disable PCI Express Link State Power Management",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Opts out of Active State Power Management (ASPM) for PCI Express devices. Prevents link-state power saving that can cause latency. Default: ASPM enabled. Recommended: Disabled for low-latency.",
            Tags = ["power", "pcie", "aspm", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI", "ASPMOptOut", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI", "ASPMOptOut"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PCI", "ASPMOptOut", 1)],
        },
        new TweakDef
        {
            Id = "power-pwr-disable-idle-states",
            Label = "Disable Processor Idle States",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables energy estimation and enables exit latency checking to prevent deep processor idle states (C-states). Maximises CPU responsiveness. Default: Enabled. Recommended: Disabled for gaming.",
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
            Id = "power-no-password-on-resume",
            Label = "Disable Password Prompt on Sleep Resume",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the policy requiring a password when the system resumes from sleep. Improves convenience on personal machines. Default: Enabled. Recommended: Disabled on personal workstations.",
            Tags = ["power", "sleep", "password", "resume", "lock"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\Power"],
        },
        new TweakDef
        {
            Id = "power-disable-throttling-policy",
            Label = "Disable Power Throttling (Modern Standby)",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets PowerThrottlingOff=1 to globally disable power throttling for all processes. Prevents Windows from reducing background process CPU clocks. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["power", "throttling", "modern-standby", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1)],
        },
        new TweakDef
        {
            Id = "power-disable-energy-estimation",
            Label = "Disable Energy Estimation Engine",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Energy Estimation engine that continuously monitors power usage. Reduces overhead on desktop systems. Default: Enabled. Recommended: Disabled.",
            Tags = ["power", "energy", "estimation", "overhead"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\EnergyEstimation"],
        },
        new TweakDef
        {
            Id = "power-disable-sleep-away",
            Label = "Disable Hibernate Boot and Sleep Away",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables HiberBoot and SleepAway features that use disk-based sleep states. Ensures clean cold boots each time. Default: Enabled. Recommended: Disabled.",
            Tags = ["power", "hibernate", "hiberboot", "sleep-away", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da", "ValueMax", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da", "ValueMax", 1800),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\238c9fa8-0aad-41ed-83f4-97be242c8f20\29f6c1db-86da-48c5-9fdb-f2b67b1f44da", "ValueMax", 0)],
        },
        new TweakDef
        {
            Id = "power-standby-reserve-grace",
            Label = "Set Standby Reserve Grace Period",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the StandbyReserveGracePeriod to 120 seconds, giving more time before the system enters deep standby. Prevents premature sleep interruptions. Default: System defined. Recommended: 120s.",
            Tags = ["power", "standby", "grace", "sleep", "timeout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
        },
        new TweakDef
        {
            Id = "power-disable-usb-selective-suspend",
            Label = "Disable USB Selective Suspend",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables USB selective suspend. Prevents USB devices from being powered down to save energy, improving reliability. Default: enabled.",
            Tags = ["power", "usb", "selective-suspend", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
        },
        new TweakDef
        {
            Id = "power-set-high-performance-plan",
            Label = "Set Active Power Plan to High Performance",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the active power scheme to High Performance (8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c). Default: Balanced.",
            Tags = ["power", "plan", "high-performance", "scheme"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes", "ActivePowerScheme", "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes", "ActivePowerScheme", "381b4222-f694-41f0-9685-ff5bb260df2e")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes", "ActivePowerScheme", "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c")],
        },
        new TweakDef
        {
            Id = "power-disable-hard-disk-idle-timeout",
            Label = "Disable Hard Disk Idle Turn Off",
            Category = "Power",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from spinning down hard disks after idle. Avoids seek delays on HDDs. Default: 20 minutes.",
            Tags = ["power", "hard-disk", "idle", "spin-down"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", "ACSetting", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", "ACSetting", 1200)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e\DefaultPowerSchemeValues\8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", "ACSetting", 0)],
        },
    ];
}
