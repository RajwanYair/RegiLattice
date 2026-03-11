namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Maintenance
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "maint-registry-autobackup",
            Label = "Enable Registry Auto-Backup",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = @"Enables Windows nightly registry hive backup to C:\Windows\System32\config\RegBack.",
            Tags = ["maintenance", "backup", "registry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
        },
        new TweakDef
        {
            Id = "maint-disable-defrag-boot-optimize",
            Label = "Disable Scheduled Defragmentation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows automatic disk defragmentation schedule. Recommended for SSD-only systems.",
            Tags = ["maintenance", "disk", "defrag", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "Y"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N")],
        },
        new TweakDef
        {
            Id = "maint-disable-crash-dumps",
            Label = "Disable Crash Memory Dumps",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables crash memory dump files to save disk space and avoid large MEMORY.DMP writes.",
            Tags = ["maintenance", "disk", "cleanup", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
        },
        new TweakDef
        {
            Id = "maint-disable-tips-suggestions",
            Label = "Disable Windows Tips & Suggestions",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, suggestions, and soft-landing notifications that clutter the notification area. Default: Enabled. Recommended: Disabled.",
            Tags = ["maintenance", "notifications", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-reliability-monitor",
            Label = "Disable Reliability Monitor (Perf)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables reliability monitor event tracking. Saves background I/O and CPU overhead. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["maintenance", "performance", "monitoring"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-maintenance-wakeup",
            Label = "Disable Automatic Maintenance Wake-Up",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from waking the PC to run automatic maintenance tasks. Default: Enabled. Recommended: Disabled.",
            Tags = ["maintenance", "power", "wakeup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostics",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Disk Diagnostic scenario via WDI policy. Reduces background disk analysis overhead.",
            Tags = ["maintenance", "disk", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}", "ScenarioExecutionEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}", "ScenarioExecutionEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}", "ScenarioExecutionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-error-reporting",
            Label = "Disable Windows Error Reporting",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Error Reporting (WER). Stops automatic submission of crash data to Microsoft.",
            Tags = ["maintenance", "privacy", "error-reporting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "maint-disable-superfetch-prefetch",
            Label = "Disable SuperFetch/SysMain Prefetch",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SuperFetch (SysMain) and Prefetcher services via registry. Recommended for SSD-only systems to reduce writes.",
            Tags = ["maintenance", "performance", "ssd", "prefetch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnableSuperfetch", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 3),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnableSuperfetch", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-storage-sense",
            Label = "Disable Storage Sense Auto-Cleanup",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Storage Sense automatic disk cleanup. Prevents Windows from automatically deleting temporary files.",
            Tags = ["maintenance", "disk", "storage-sense", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-cleanup-nag",
            Label = "Disable Disk Cleanup Notifications",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Storage Sense and low disk space notifications. Prevents automatic cleanup of temp files and downloads. Default: Enabled. Recommended: Disabled for manual control.",
            Tags = ["maintenance", "storage", "cleanup", "notifications"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense", "AllowStorageSenseGlobal", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense", "AllowStorageSenseGlobal"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense", "AllowStorageSenseGlobal", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-defrag",
            Label = "Disable Background Defragmentation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables background disk defragmentation and boot optimization. Reduces disk I/O on SSDs where defragmentation is unnecessary. Default: Enabled. Recommended: Disabled for SSDs.",
            Tags = ["maintenance", "defrag", "performance", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "Y"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N")],
        },
        new TweakDef
        {
            Id = "maint-disable-prefetch",
            Label = "Disable Prefetch/Superfetch",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables both Prefetch and Superfetch/SysMain services. Reduces disk I/O overhead on SSD-based systems. Default: 3 (enabled). Recommended: 0 (disabled) for SSDs.",
            Tags = ["maintenance", "prefetch", "superfetch", "ssd", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
        },
        new TweakDef
        {
            Id = "maint-disable-compatibility-assistant",
            Label = "Disable Compatibility Assistant",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Program Compatibility Assistant via Group Policy. Prevents compatibility shims from being applied automatically. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["maintenance", "compatibility", "pca", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "maint-disable-auto-wakeup",
            Label = "Disable Automatic Maintenance Wakeup",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic maintenance from waking the PC. Prevents unexpected wakeups for maintenance tasks. Default: Enabled. Recommended: Disabled for always-on PCs.",
            Tags = ["maintenance", "wakeup", "automatic", "sleep"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
        },
        new TweakDef
        {
            Id = "maint-compat-telemetry-minimal",
            Label = "Set Compatibility Telemetry to Minimal",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Windows compatibility telemetry to minimal (0) via policy. Reduces data sent to Microsoft for compatibility analysis. Default: 3 (Full). Recommended: 0 (Security/minimal).",
            Tags = ["maintenance", "telemetry", "compatibility", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-defrag-schedule",
            Label = "Disable Disk Defrag Schedule",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the automatic disk defragmentation schedule via OptimalLayout. Prevents background defrag I/O on SSD-based systems. Default: Enabled. Recommended: Disabled for SSDs.",
            Tags = ["maintenance", "defrag", "schedule", "ssd", "optimization"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "Y"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N")],
        },
    ];
}
