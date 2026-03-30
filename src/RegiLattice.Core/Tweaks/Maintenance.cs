namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Maintenance
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "maint-disable-reliability-monitor",
            Label = "Disable Reliability Monitor (Perf)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables reliability monitor event tracking. Saves background I/O and CPU overhead. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["maintenance", "performance", "monitoring"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "TimeStampInterval", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1)],
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
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "maint-disable-superfetch-prefetch",
            Label = "Disable SuperFetch/SysMain Prefetch",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the SuperFetch (SysMain) and Prefetcher services via registry. Recommended for SSD-only systems to reduce writes.",
            Tags = ["maintenance", "performance", "ssd", "prefetch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "maint-disable-cleanup-nag",
            Label = "Disable Disk Cleanup Notifications",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Storage Sense and low disk space notifications. Prevents automatic cleanup of temp files and downloads. Default: Enabled. Recommended: Disabled for manual control.",
            Tags = ["maintenance", "storage", "cleanup", "notifications"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense", "AllowStorageSenseGlobal", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense", "AllowStorageSenseGlobal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StorageSense", "AllowStorageSenseGlobal", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-compatibility-assistant",
            Label = "Disable Compatibility Assistant",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Program Compatibility Assistant via Group Policy. Prevents compatibility shims from being applied automatically. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["maintenance", "compatibility", "pca", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "maint-disable-auto-wakeup",
            Label = "Disable Automatic Maintenance Wakeup",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic maintenance from waking the PC. Prevents unexpected wakeups for maintenance tasks. Default: Enabled. Recommended: Disabled for always-on PCs.",
            Tags = ["maintenance", "wakeup", "automatic", "sleep"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
        },
        new TweakDef
        {
            Id = "maint-compat-telemetry-minimal",
            Label = "Set Compatibility Telemetry to Minimal",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets Windows compatibility telemetry to minimal (0) via policy. Reduces data sent to Microsoft for compatibility analysis. Default: 3 (Full). Recommended: 0 (Security/minimal).",
            Tags = ["maintenance", "telemetry", "compatibility", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-scheduled-defrag",
            Label = "Disable Scheduled Defragmentation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the scheduled drive optimization task. Recommended for SSDs where TRIM is sufficient. Default: enabled.",
            Tags = ["maintenance", "defrag", "scheduled", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "OptimizeComplete", "Yes")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "OptimizeComplete")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "OptimizeComplete", "Yes")],
        },
        new TweakDef
        {
            Id = "maint-disable-auto-maintenance",
            Label = "Disable Automatic Maintenance",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows automatic maintenance that runs daily. Prevents unexpected disk and CPU activity. Default: enabled.",
            Tags = ["maintenance", "automatic", "scheduled", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "maint-disable-superfetch",
            Label = "Disable Superfetch / SysMain",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (Superfetch) service via registry. Reduces disk I/O on systems with SSD. Default: enabled.",
            Tags = ["maintenance", "superfetch", "sysmain", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "maint-increase-disk-cleanup-sageset",
            Label = "Configure Disk Cleanup for All Categories",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Pre-selects all cleanup categories for Disk Cleanup sageset 0. Enables one-click full cleanup. Default: none selected.",
            Tags = ["maintenance", "cleanup", "disk", "space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Temporary Files"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Temporary Files",
                    "StateFlags0000",
                    2
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Temporary Files",
                    "StateFlags0000"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Temporary Files",
                    "StateFlags0000",
                    2
                ),
            ],
        },
        new TweakDef
        {
            Id = "maint-disable-automatic-maintenance",
            Label = "Disable Automatic Maintenance",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the scheduled Automatic Maintenance task that runs daily. Prevents unexpected disk/CPU activity. Default: daily at 2 AM.",
            Tags = ["maintenance", "automatic", "schedule", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
        },
        // ── Command-based maintenance tweaks ───────────────────────────────
        new TweakDef
        {
            Id = "maint-sfc-scannow",
            Label = "Run System File Checker (SFC /scannow)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Runs SFC /scannow to scan and repair corrupted Windows system files. One-time repair action.",
            Tags = ["maintenance", "sfc", "repair", "system", "integrity"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "May take several minutes. Repairs protected system files from the component store.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                var (code, _, stderr) = Elevation.RunElevated("cmd", ["/c", "sfc", "/scannow"]);
                if (code != 0)
                    throw new InvalidOperationException($"SFC /scannow failed: {stderr}");
            },
            DetectAction = () => false, // One-time action
        },
        new TweakDef
        {
            Id = "maint-dism-restorehealth",
            Label = "Run DISM RestoreHealth",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Runs DISM /Online /Cleanup-Image /RestoreHealth to repair the Windows component store. Should be run before SFC for best results.",
            Tags = ["maintenance", "dism", "repair", "component", "restore"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "May take 15+ minutes. Downloads missing components from Windows Update.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                var (code, _, stderr) = Elevation.RunElevated("dism", ["/online", "/cleanup-image", "/restorehealth"]);
                if (code != 0)
                    throw new InvalidOperationException($"DISM RestoreHealth failed: {stderr}");
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "maint-dism-component-cleanup",
            Label = "DISM Component Store Cleanup",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Runs DISM /Cleanup-Image /StartComponentCleanup to remove superseded components and reduce WinSxS folder size.",
            Tags = ["maintenance", "dism", "cleanup", "winsxs", "disk"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Removes old component versions — may prevent uninstalling certain updates.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("dism", ["/online", "/cleanup-image", "/startcomponentcleanup", "/resetbase"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "maint-flush-dns",
            Label = "Flush DNS Resolver Cache",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Clears the DNS resolver cache. Useful after changing DNS settings or when resolving stale DNS entries.",
            Tags = ["maintenance", "dns", "flush", "network", "cache"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("cmd", ["/c", "ipconfig", "/flushdns"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "maint-reset-winsock",
            Label = "Reset Winsock Catalog (netsh)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Resets the Winsock catalog to a clean state. Fixes network connectivity issues caused by corrupt LSP entries. Requires reboot.",
            Tags = ["maintenance", "winsock", "reset", "network", "repair"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot. May reset VPN/proxy software configurations.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("netsh", ["winsock", "reset"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "maint-reset-ip-stack",
            Label = "Reset TCP/IP Stack (netsh)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Resets the TCP/IP stack to clean defaults. Fixes IP connectivity issues. Requires reboot.",
            Tags = ["maintenance", "tcpip", "reset", "network", "repair"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot. Resets static IP configuration.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("netsh", ["int", "ip", "reset"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "maint-disable-prefetch",
            Label = "Disable Prefetch/Superfetch",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Prefetch and Superfetch (SysMain). Reduces disk I/O on SSD systems where prefetch provides no benefit. Default: enabled.",
            Tags = ["maintenance", "prefetch", "superfetch", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "maint-registry-autobackup",
            Label = "Enable Automatic Registry Backup",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Re-enables automatic registry hive backups (RegIdleBackup). Microsoft disabled this in Win10 1803+. Creates periodic RegBack copies. Default: disabled.",
            Tags = ["maintenance", "registry", "backup", "regback"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager",
                    "EnablePeriodicBackup",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager",
                    "EnablePeriodicBackup"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager",
                    "EnablePeriodicBackup",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "maint-clear-recent-docs-on-exit",
            Label = "Clear Recent Documents List on Logoff",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically clears the Recent Documents (MRU) history when the user logs off, preventing access trail accumulation.",
            Tags = ["maintenance", "recent-docs", "privacy", "mru", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1),
            ],
        },
        new TweakDef
        {
            Id = "maint-reduce-app-kill-timeout",
            Label = "Reduce Application Kill Timeout on Shutdown (2s)",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces time Windows waits for apps to respond during shutdown before force-killing them to 2 seconds (default: 20s).",
            Tags = ["maintenance", "shutdown", "performance", "timeout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "2000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "20000")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WaitToKillAppTimeout", "2000")],
        },
        new TweakDef
        {
            Id = "maint-disable-desktop-cleanup-wizard",
            Label = "Disable Desktop Cleanup Wizard",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 'Clean up Desktop Wizard' that prompts to remove rarely used desktop icons. Default: enabled.",
            Tags = ["maintenance", "desktop", "cleanup", "wizard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Desktop\CleanupWiz"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Desktop\CleanupWiz", "NoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Desktop\CleanupWiz", "NoRun")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Desktop\CleanupWiz", "NoRun", 1)],
        },
        new TweakDef
        {
            Id = "maint-disable-hang-boot-timeout",
            Label = "Reduce Hung-App Boot Timeout (Auto Kill)",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the HungAppTimeout to 2000 ms so Windows auto-closes unresponsive apps faster on shutdown.",
            Tags = ["maintenance", "shutdown", "performance", "timeout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "2000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "5000")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "2000")],
        },
        new TweakDef
        {
            Id = "maint-auto-end-tasks-on-shutdown",
            Label = "Auto-End Tasks on Shutdown (No Prompt)",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables AutoEndTasks so Windows automatically terminates applications that are blocking shutdown without asking the user.",
            Tags = ["maintenance", "shutdown", "performance", "auto-kill"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
        },
        new TweakDef
        {
            Id = "maint-disable-crash-on-audit-fail",
            Label = "Disable Crash-on-Audit-Full (Security)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from shutting down immediately when the security audit log is full. Avoids unexpected reboots when audit logs fill up.",
            Tags = ["maintenance", "audit", "security", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "CrashOnAuditFail", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "CrashOnAuditFail")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "CrashOnAuditFail", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-show-recent-in-explorer",
            Label = "Hide Recent Files in Quick Access",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Hides the 'Recent Files' section from the File Explorer Quick Access panel. Files are still accessible via direct navigation.",
            Tags = ["maintenance", "explorer", "recent-files", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0)],
        },
        new TweakDef
        {
            Id = "maint-disable-frequent-in-explorer",
            Label = "Hide Frequent Folders in Quick Access",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the 'Frequent Folders' section from File Explorer Quick Access, giving a cleaner navigation pane.",
            Tags = ["maintenance", "explorer", "frequent-folders", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0)],
        },
    ];
}
