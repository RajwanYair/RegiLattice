namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsUpdate
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wu-defer-quality-updates",
            Label = "Defer Quality Updates (30 days)",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Defers quality (security/bug-fix) updates by 30 days.",
            Tags = ["update", "deferral"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 30),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1)],
        },
        new TweakDef
        {
            Id = "wu-defer-feature-updates",
            Label = "Defer Feature Updates (90 days)",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Defers feature (major version) updates by 90 days.",
            Tags = ["update", "deferral"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1)],
        },
        new TweakDef
        {
            Id = "wu-no-auto-restart",
            Label = "Disable Forced Auto-Restart",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from automatically restarting while a user is logged in after update installation.",
            Tags = ["update", "restart"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
        },
        new TweakDef
        {
            Id = "wu-update-notify-only",
            Label = "Notify-Only Updates (No Auto-Install)",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Windows Update to notify before downloading, giving you full control over update timing.",
            Tags = ["update", "control"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 3),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 2)],
        },
        new TweakDef
        {
            Id = "wu-set-active-hours-au",
            Label = "Set Active Hours (8 AM - 11 PM)",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Windows Update active hours to 8 AM - 11 PM to prevent restart during work.",
            Tags = ["update", "active-hours", "restart"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursStart", 8),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursEnd", 23),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursStart"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursEnd"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours", 1)],
        },
        new TweakDef
        {
            Id = "wu-disable-msrt",
            Label = "Disable MSRT Delivery",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the Malicious Software Removal Tool from being offered via Windows Update.",
            Tags = ["update", "msrt", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
        },
        new TweakDef
        {
            Id = "wu-target-release-version",
            Label = "Pin to Windows 11 24H2",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Pins the device to Windows 11 24H2 to prevent unwanted feature updates.",
            Tags = ["update", "feature", "pin", "24H2"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersion", 1),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersionInfo", "24H2"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ProductVersion", "Windows 11"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersion"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersionInfo"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ProductVersion"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersion", 1)],
        },
        new TweakDef
        {
            Id = "wu-disable-do-upload",
            Label = "Disable Delivery Optimization Upload",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Delivery Optimization peer-to-peer upload. Prevents your PC from serving update files to other PCs. Sets upload bandwidth to zero. Default: Unlimited. Recommended: Disabled.",
            Tags = ["update", "delivery-optimization", "bandwidth", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "wu-disable-driver-updates",
            Label = "Disable Driver Updates via Windows Update",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Excludes driver updates from Windows Update quality updates. Default: Included. Recommended: Excluded for driver stability.",
            Tags = ["update", "driver", "exclude", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
            ],
        },
        new TweakDef
        {
            Id = "wu-defer-quality-updates-14d",
            Label = "Defer Quality Updates by 14 Days",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Defers quality/security updates by 14 days to allow time for issue reports. Default: 0. Recommended: 14 for stability.",
            Tags = ["update", "defer", "quality", "delay"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 30),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1)],
        },
        new TweakDef
        {
            Id = "wu-block-driver-search",
            Label = "Block Driver Search via Windows Update",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from searching for driver updates through Windows Update. Different from WU driver exclusion policy. Default: enabled. Recommended: disabled for stability.",
            Tags = ["update", "driver", "search", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 0)],
        },
        new TweakDef
        {
            Id = "wu-defer-feature-365d",
            Label = "Defer Feature Updates by 365 Days",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Defers Windows feature updates by 365 days. Provides maximum time for stability reports before upgrading. Default: 0. Recommended: 365 for production stability.",
            Tags = ["update", "defer", "feature", "delay", "365"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1)],
        },
        new TweakDef
        {
            Id = "wu-disable-os-upgrade",
            Label = "Disable Windows OS Upgrade via Update",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Update from offering or installing OS version upgrades. Blocks W10 to W11 upgrades being pushed silently. Default: Enabled. Recommended: Disabled for production stability.",
            Tags = ["update", "upgrade", "os", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade", 1)],
        },
        new TweakDef
        {
            Id = "wu-disable-safeguard-hold",
            Label = "Disable Windows Update Safeguard Holds",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Microsoft's safeguard holds that block updates on incompatible hardware. Use only if you understand the update risks for your system. Default: Enabled. Recommended: Enabled (disable only if blocked).",
            Tags = ["update", "safeguard", "hold", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards", 1)],
        },
        new TweakDef
        {
            Id = "wu-disable-optional-updates",
            Label = "Disable Auto-Install of Optional Updates",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Update from automatically installing optional/minor updates. Gives you manual control over optional update installations. Default: Enabled. Recommended: Disabled.",
            Tags = ["update", "optional", "minor", "auto-install"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates", 0)],
        },
        new TweakDef
        {
            Id = "wu-set-active-hours-8-20",
            Label = "Set Windows Update Active Hours (8 AM – 8 PM)",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Windows Update active hours to 8 AM – 8 PM. No restart prompts during this window. Default: auto.",
            Tags = ["update", "active-hours", "restart", "schedule"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart", 8),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursEnd", 20),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursEnd"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart", 8)],
        },
        new TweakDef
        {
            Id = "wu-defer-quality-updates-7days",
            Label = "Defer Quality Updates by 7 Days",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Defers quality (security/bug fix) updates by 7 days. Gives time for known issues to surface. Default: 0 days.",
            Tags = ["update", "defer", "quality", "days"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 7),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 7),
            ],
        },
        new TweakDef
        {
            Id = "wu-defer-feature-updates-90days",
            Label = "Defer Feature Updates by 90 Days",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Defers feature updates (major releases) by 90 days. Ensures stability before adopting new builds. Default: 0 days.",
            Tags = ["update", "defer", "feature", "days"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
            ],
        },
        new TweakDef
        {
            Id = "wu-disable-seeker-updates",
            Label = "Disable Optional Update Seeker",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from seeking optional quality updates. Only mandatory updates are installed. Default: seeks all.",
            Tags = ["update", "optional", "seeker", "quality"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
        },
        new TweakDef
        {
            Id = "wu-disable-auto-restart",
            Label = "Disable Auto-Restart After Updates",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically restarting after installing updates. User must manually initiate the restart. Default: auto-restart.",
            Tags = ["update", "restart", "automatic", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
        },
        new TweakDef
        {
            Id = "wu-disable-delivery-optimization",
            Label = "Disable Delivery Optimisation P2P",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables peer-to-peer delivery optimisation. Updates only download from Microsoft servers, not other PCs. Default: LAN + Internet.",
            Tags = ["update", "delivery-optimization", "p2p", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "wu-disable-update-notifications",
            Label = "Disable Update Notifications",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses Windows Update restart notifications and nagging prompts. Updates still install but silently. Default: notifications shown.",
            Tags = ["update", "notifications", "nag", "quiet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel", 2)],
        },
        new TweakDef
        {
            Id = "wu-disable-update-orchestrator",
            Label = "Disable Update Orchestrator Service",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Update Orchestrator Service (UsoSvc). Prevents Windows from automatically checking for and installing updates. Default: automatic.",
            Tags = ["update", "orchestrator", "service", "disable"],
            SideEffects = "Windows will not automatically check for security updates.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "wu-disable-ux-access",
            Label = "Disable Windows Update UX Access",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hides the Windows Update page in Settings. Prevents non-admin users from triggering manual update checks. Default: accessible.",
            Tags = ["update", "ux", "settings", "hide"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
        },
        new TweakDef
        {
            Id = "wu-disable-wus-medic",
            Label = "Disable Windows Update Medic Service",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Update Medic Service (WaaSMedicSvc) that repairs Windows Update components. Prevents forced re-enablement. Default: automatic.",
            Tags = ["update", "medic", "service", "disable"],
            SideEffects = "Windows Update cannot self-repair if components become corrupted.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "wu-exclude-drivers-quality",
            Label = "Exclude Drivers from Quality Updates",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Excludes driver updates from quality update installations. Prevents Windows Update from overwriting manually installed drivers. Default: included.",
            Tags = ["update", "drivers", "exclude", "quality"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
            ],
        },
        new TweakDef
        {
            Id = "wu-disable-automatic-updates",
            Label = "Disable Automatic Update Downloads",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows Update from automatically downloading updates. Updates will still be detected but must be manually approved and installed. Default: auto-download enabled.",
            Tags = ["windows-update", "automatic", "download", "control"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "wu-set-schedule-day-saturday",
            Label = "Schedule Updates for Saturday Installation",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures Windows Update to install scheduled updates on Saturday at 3:00 AM, minimising disruption during working hours.",
            Tags = ["windows-update", "schedule", "maintenance", "saturday"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay", 7),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallTime", 3),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 4),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallTime"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay", 7)],
        },
        new TweakDef
        {
            Id = "wu-disable-store-app-auto-updates",
            Label = "Disable Microsoft Store App Auto-Updates",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Microsoft Store from automatically updating installed applications in the background. You retain control over when app updates are applied.",
            Tags = ["windows-update", "store", "apps", "auto-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
        },
        new TweakDef
        {
            Id = "wu-set-update-service-manual",
            Label = "Set Windows Update Service to Manual Start",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Changes the Windows Update service (wuauserv) to manual start so it only runs when you initiate a check, preventing background update scans from consuming resources.",
            Tags = ["windows-update", "service", "manual", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 3)],
        },
        new TweakDef
        {
            Id = "wu-require-admin-for-updates",
            Label = "Require Admin Approval for Update Installation",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Update from installing updates without explicit administrator approval. Useful on shared systems to maintain control over when patches are applied.",
            Tags = ["windows-update", "admin", "approval", "control"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins", 0)],
        },
        new TweakDef
        {
            Id = "wu-disable-metered-update-download",
            Label = "Block Updates on Metered Connections",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Update from downloading updates when the network connection is marked as metered (mobile hotspot, limited data plans), saving mobile data costs.",
            Tags = ["windows-update", "metered", "mobile", "data", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings", "DownloadMode", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings", "DownloadMode"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings", "DownloadMode", 0),
            ],
        },
        new TweakDef
        {
            Id = "wu-disable-reboot-required-notification",
            Label = "Disable Post-Update Reboot Notifications",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses the nagging 'Restart Required' toast notifications that appear after Windows Update installs patches. Reboots can still be performed manually.",
            Tags = ["windows-update", "reboot", "notification", "toast"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig", 1),
            ],
        },
        new TweakDef
        {
            Id = "wu-set-feature-update-channel-general",
            Label = "Set Windows Update Channel to General Availability",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Pins the Windows Update servicing channel to General Availability / Semi-Annual Channel, avoiding early feature updates that may be less stable.",
            Tags = ["windows-update", "channel", "feature", "stable", "semi-annual"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "BranchReadinessLevel", 16)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "BranchReadinessLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "BranchReadinessLevel", 16)],
        },
        new TweakDef
        {
            Id = "wu-set-orchestrator-service-manual",
            Label = "Set Update Orchestrator Service to Manual",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Changes the Update Orchestrator Service (UsoSvc) to manual start, preventing it from waking the system for updates outside your active hours.",
            Tags = ["windows-update", "orchestrator", "schedule", "wake", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 3)],
        },
        new TweakDef
        {
            Id = "wu-disable-third-party-preview",
            Label = "Disable Third-Party Windows Update Preview Consent",
            Category = "Windows Update",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the consent dialog that prompts users to participate in Windows Update previews from third-party software publishers.",
            Tags = ["windows-update", "preview", "third-party", "consent"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess", 0)],
        },
    ];
}
