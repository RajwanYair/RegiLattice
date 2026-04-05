namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Maintenance
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
    ];
}

// ── Merged from CrashDiagnostics.cs ──────────────────────────────────────────────────

internal static class CrashDiagnostics
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "crash-disable-auto-restart",
            Label = "Disable Auto-Restart on BSOD",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Stay on BSOD screen instead of auto-rebooting. Helps read stop codes. Default: auto-restart. Recommended: disabled.",
            Tags = ["bsod", "crash", "restart", "blue-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "crash-set-minidump",
            Label = "Set Crash Dump to Minidump",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Save only small minidumps on crash (saves disk). Default: automatic memory dump.",
            Tags = ["crash", "dump", "minidump", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "crash-disable-wer",
            Label = "Disable Windows Error Reporting",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable WER service from sending error reports to Microsoft. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["wer", "error", "reporting", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "crash-disable-dump-overwrite",
            Label = "Disable Crash Dump Overwrite",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Keep all crash dumps instead of overwriting with latest. Default: overwrite.",
            Tags = ["crash", "dump", "overwrite", "history"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-scripted-diagnostics",
            Label = "Disable Scripted Diagnostics",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable automatic troubleshooters and scripted diagnostics. Default: enabled.",
            Tags = ["diagnostics", "troubleshooter", "script"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "EnableQueryRemoteServer",
                    0
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics", "EnableDiagnostics", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "EnableQueryRemoteServer"
                ),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics", "EnableDiagnostics"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics", "EnableDiagnostics", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-perf-tracking",
            Label = "Disable Performance Tracking (WDI)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable Windows Diagnostic Infrastructure performance tracking. Default: enabled.",
            Tags = ["wdi", "performance", "tracking", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}",
                    "ScenarioExecutionEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "crash-disable-pca",
            Label = "Disable Program Compatibility Assistant",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable PCA popup suggesting compatibility settings. Default: enabled. Recommended: disabled if not needed.",
            Tags = ["pca", "compatibility", "assistant", "popup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "crash-disable-error-dialog",
            Label = "Disable Automatic Error Dialog Boxes",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic error dialog boxes (ErrorMode=2). Suppresses critical-error-handler message boxes for background services. Default: 0 (show all). Recommended: 2 for servers.",
            Tags = ["crash", "error", "dialog", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows", "ErrorMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows", "ErrorMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows", "ErrorMode", 2)],
        },
        new TweakDef
        {
            Id = "crash-disable-jit-debugger",
            Label = "Disable Auto-Attach of JIT Debugger",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically launching a JIT debugger when an application crashes. Suppresses the 'attach debugger?' dialog. Default: 1 (auto-attach). Recommended: 0 on production machines.",
            Tags = ["crash", "debugger", "jit", "aedebug", "dev"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Auto", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Auto", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Auto", "0")],
        },
        new TweakDef
        {
            Id = "crash-wer-no-additional-data",
            Label = "WER: Don't Send Additional Crash Data",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Instructs Windows Error Reporting to not include supplemental data (heap dumps, user-mode state) when submitting crash reports. Default: sends all data. Recommended: disabled for privacy.",
            Tags = ["crash", "wer", "privacy", "data", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "crash-wer-disable-archive",
            Label = "WER: Disable Local Report Archive",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                @"Stops Windows Error Reporting from maintaining a local archive of submitted crash reports in ProgramData\Microsoft\Windows\WER\ReportArchive. Default: archive enabled. Recommended: disabled for disk space.",
            Tags = ["crash", "wer", "archive", "disk", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1)],
        },
        new TweakDef
        {
            Id = "crash-wer-min-consent",
            Label = "WER: Send Parameters Only (Minimal Consent)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WER DefaultConsent=2 (parameters only), limiting transmitted crash data to exception codes and fault module, not executable images. Default: varies (1=always ask, 4=send all). Recommended: 2 for privacy.",
            Tags = ["crash", "wer", "consent", "privacy", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 2)],
        },
        new TweakDef
        {
            Id = "crash-disable-online-crash-analysis",
            Label = "Disable Online Crash Analysis",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents crash reports from being sent to Microsoft for online analysis. Default: enabled.",
            Tags = ["crash", "online", "analysis", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting", "DoReport", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting", "DoReport")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting", "DoReport", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-user-mode-crashdump",
            Label = "Disable User-Mode Crash Dumps",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables creation of user-mode crash dump files when applications crash. Saves disk space. Default: dump files created.",
            Tags = ["crash", "user-mode", "dump", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-steps-recorder",
            Label = "Disable Steps Recorder",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Steps Recorder (psr.exe). Prevents screen recording for troubleshooting. Default: enabled.",
            Tags = ["crash", "steps-recorder", "psr", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
        },
    ];
}

// ── Merged from TimeSync.cs ──────────────────────────────────────────────────

internal static class TimeSync
{
    private const string W32TimeParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Parameters";

    private const string W32TimeConfig = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config";

    private const string W32TimeNtpClient = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpClient";

    private const string W32TimeNtpServer = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpServer";

    private const string TimeZoneInfo = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation";

    private const string VmicTimeProv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\VMICTimeProvider";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "time-set-ntp-pool-servers",
            Label = "Set NTP Servers to pool.ntp.org",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "sync", "servers"],
            Description =
                "Sets the NTP time server to the global pool.ntp.org pool, which "
                + "provides geographically distributed, highly available time sources. "
                + "More reliable than the default time.windows.com.",
            ApplyOps = [RegOp.SetString(W32TimeParams, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9")],
            RemoveOps = [RegOp.SetString(W32TimeParams, "NtpServer", "time.windows.com,0x9")],
            DetectOps =
            [
                RegOp.CheckString(W32TimeParams, "NtpServer", "0.pool.ntp.org,0x9 1.pool.ntp.org,0x9 2.pool.ntp.org,0x9 3.pool.ntp.org,0x9"),
            ],
        },
        new TweakDef
        {
            Id = "time-set-cloudflare-ntp",
            Label = "Set NTP Server to Cloudflare time.cloudflare.com",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "cloudflare", "sync"],
            Description =
                "Sets the NTP server to Cloudflare's time.cloudflare.com, which uses "
                + "anycast routing and roughtime for high accuracy. Privacy-respecting "
                + "and does not log queries.",
            ApplyOps = [RegOp.SetString(W32TimeParams, "NtpServer", "time.cloudflare.com,0x9")],
            RemoveOps = [RegOp.SetString(W32TimeParams, "NtpServer", "time.windows.com,0x9")],
            DetectOps = [RegOp.CheckString(W32TimeParams, "NtpServer", "time.cloudflare.com,0x9")],
        },
        new TweakDef
        {
            Id = "time-increase-sync-interval",
            Label = "Increase NTP Sync Interval to 12 Hours",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "sync", "interval"],
            Description =
                "Increases the NTP polling interval to 43,200 seconds (12 hours). "
                + "Reduces network traffic to the NTP server while still keeping the "
                + "clock accurate for most workstation use cases.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "MaxPosPhaseCorrection", 43200)],
            RemoveOps = [RegOp.SetDword(W32TimeConfig, "MaxPosPhaseCorrection", 3600)],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "MaxPosPhaseCorrection", 43200)],
        },
        new TweakDef
        {
            Id = "time-decrease-sync-interval",
            Label = "Decrease NTP Sync Interval to 30 Minutes (High Accuracy)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "sync", "accuracy", "interval"],
            Description =
                "Lowers the minimum NTP polling interval to 1,800 seconds (30 minutes) "
                + "for tighter clock accuracy. Useful for logging servers, stock traders, "
                + "or any system where precise timestamps matter.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "SpecialPollInterval", 1800)],
            RemoveOps = [RegOp.SetDword(W32TimeNtpClient, "SpecialPollInterval", 3600)],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "SpecialPollInterval", 1800)],
        },
        new TweakDef
        {
            Id = "time-enable-ntp-client",
            Label = "Enable NTP Client (W32Time Provider)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "client", "enable"],
            Description =
                "Ensures the NTP client time provider is enabled in W32Time. "
                + "Can become disabled if Windows Time Service is partially configured "
                + "or if a third-party time sync tool interfered.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue(W32TimeNtpClient, "Enabled")],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "time-disable-ntp-server",
            Label = "Disable NTP Server Role (Workstation Only)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "server", "disable", "security"],
            Description =
                "Disables the Windows Time NTP server role. Non-server Windows machines "
                + "should not act as NTP servers; disabling reduces UDP 123 exposure "
                + "and eliminates NTP amplification DDoS risk.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(W32TimeNtpServer, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "time-enable-utc-hardware-clock",
            Label = "Store Hardware Clock in UTC (Linux Dual-Boot)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["time", "utc", "rtc", "dual boot", "linux"],
            Description =
                "Configures Windows to treat the hardware (RTC) clock as UTC rather than "
                + "local time. Required for dual-boot systems with Linux to prevent time "
                + "drift between OS sessions.",
            ApplyOps = [RegOp.SetDword(TimeZoneInfo, "RealTimeIsUniversal", 1)],
            RemoveOps = [RegOp.DeleteValue(TimeZoneInfo, "RealTimeIsUniversal")],
            DetectOps = [RegOp.CheckDword(TimeZoneInfo, "RealTimeIsUniversal", 1)],
        },
        new TweakDef
        {
            Id = "time-set-type-ntp",
            Label = "Set W32Time to Use NTP (Internet Sync)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["time", "ntp", "type", "sync", "w32time"],
            Description =
                "Sets the Windows Time Service type to 'NTP', enabling internet-based "
                + "synchronisation. The default may be 'NT5DS' (domain hierarchy sync) "
                + "on domain-joined machines. Switch to NTP on standalone workstations.",
            ApplyOps = [RegOp.SetString(W32TimeParams, "Type", "NTP")],
            RemoveOps = [RegOp.SetString(W32TimeParams, "Type", "NT5DS")],
            DetectOps = [RegOp.CheckString(W32TimeParams, "Type", "NTP")],
        },
        new TweakDef
        {
            Id = "time-increase-max-neg-correction",
            Label = "Increase Max Negative Time Correction to 24 Hours",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["time", "correction", "drift", "sync"],
            Description =
                "Allows W32Time to step the clock backwards by up to 86,400 seconds "
                + "(24 h). By default, very large negative corrections are rejected. "
                + "Useful after hibernation or VM resume when the clock has drifted far.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "MaxNegPhaseCorrection", 86400)],
            RemoveOps = [RegOp.SetDword(W32TimeConfig, "MaxNegPhaseCorrection", 3600)],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "MaxNegPhaseCorrection", 86400)],
        },
        new TweakDef
        {
            Id = "time-set-crosssite-sync-flags",
            Label = "Set NTP Cross-Site Sync Flags (AllFlags)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "cross-site", "flags", "sync"],
            Description =
                "Sets the NTP client CrossSiteSyncFlags to 2 (UseAnyDomainController), "
                + "allowing the client to sync from domain controllers in other sites "
                + "when the local site DC is unavailable. Improves time sync reliability.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "CrossSiteSyncFlags", 2)],
            RemoveOps = [RegOp.DeleteValue(W32TimeNtpClient, "CrossSiteSyncFlags")],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "CrossSiteSyncFlags", 2)],
        },
        new TweakDef
        {
            Id = "time-set-max-pos-phase-correction",
            Label = "Set Maximum Forward Time Correction to 1 Hour",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "phase", "correction", "sync"],
            Description =
                "Sets the maximum positive phase correction (MaxPosPhaseCorrection) to "
                + "3600 seconds (1 hour). Prevents W32Time from jumping the clock forward by "
                + "more than one hour in a single correction, protecting against rogue NTP servers.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "MaxPosPhaseCorrection", 3600)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "MaxPosPhaseCorrection")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "MaxPosPhaseCorrection", 3600)],
        },
        new TweakDef
        {
            Id = "time-set-update-interval",
            Label = "Set W32Time Clock Update Interval (100 000 units = 10 s)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "update", "interval", "precision"],
            Description =
                "Sets the W32Time UpdateInterval to 100 000 (100 ms ticks × 100 000 = 10 s). "
                + "Controls how often W32Time updates the local clock when it is in slew mode, "
                + "reducing the time between small, smooth clock adjustments.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "UpdateInterval", 100000)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "UpdateInterval")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "UpdateInterval", 100000)],
        },
        new TweakDef
        {
            Id = "time-disable-event-logging",
            Label = "Disable W32Time Verbose Event Log",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["time", "ntp", "event log", "logging", "privacy"],
            Description =
                "Sets W32Time EventLogFlags to 0, suppressing informational and debug "
                + "entries in the System event log. Errors are still logged. Reduces "
                + "noise in the event log on high-uptime servers.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "EventLogFlags", 0)],
            RemoveOps = [RegOp.SetDword(W32TimeConfig, "EventLogFlags", 2)],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "EventLogFlags", 0)],
        },
        new TweakDef
        {
            Id = "time-set-announce-flags",
            Label = "Set W32Time Announce Flags (Reliable Time Source)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "announce", "domain", "server"],
            Description =
                "Sets AnnounceFlags to 5 (0x05 = always reliable, always announce). "
                + "Marks this computer as a reliable time source for domain clients. "
                + "Recommended for PDC emulators and dedicated NTP servers.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "AnnounceFlags", 5)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "AnnounceFlags")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "AnnounceFlags", 5)],
        },
        new TweakDef
        {
            Id = "time-set-hold-period",
            Label = "Set W32Time Hold Period (5 Iterations)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "hold period", "slew", "stability"],
            Description =
                "Sets HoldPeriod to 5. After a large phase correction, W32Time waits 5 "
                + "poll intervals before adjusting frequencies again. Reduces instability "
                + "from rapid successive corrections.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "HoldPeriod", 5)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "HoldPeriod")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "HoldPeriod", 5)],
        },
        new TweakDef
        {
            Id = "time-set-local-clock-dispersion",
            Label = "Set Local Clock Dispersion to 10 Seconds",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "dispersion", "precision", "peers"],
            Description =
                "Sets LocalClockDispersion to 10 seconds, which is the advertised "
                + "uncertainty of the local clock to NTP peers. Lower values improve "
                + "how this machine is ranked as a time source.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "LocalClockDispersion", 10)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "LocalClockDispersion")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "LocalClockDispersion", 10)],
        },
        new TweakDef
        {
            Id = "time-set-large-phase-offset-threshold",
            Label = "Set Large Phase Offset Threshold to 50 ms",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "large offset", "threshold", "spike"],
            Description =
                "Sets LargePhaseOffset to 50 000 000 (100 ns units = 5 s). When clock "
                + "drift exceeds this threshold, W32Time steps instead of slewing. "
                + "Setting to 50 ms (5 000 000) enables faster response to large drifts.",
            ApplyOps = [RegOp.SetDword(W32TimeConfig, "LargePhaseOffset", 5000000)],
            RemoveOps = [RegOp.DeleteValue(W32TimeConfig, "LargePhaseOffset")],
            DetectOps = [RegOp.CheckDword(W32TimeConfig, "LargePhaseOffset", 5000000)],
        },
        new TweakDef
        {
            Id = "time-enable-ntp-server-provider",
            Label = "Enable NTP Server (Share System Time via UDP/123)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["time", "ntp", "server", "share", "udp"],
            Description =
                "Enables the NtpServer time provider, allowing this machine to serve "
                + "time via NTP on UDP port 123. Intended for machines that act as an "
                + "internal NTP server for a local network.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 1)],
            RemoveOps = [RegOp.SetDword(W32TimeNtpServer, "Enabled", 0)],
            DetectOps = [RegOp.CheckDword(W32TimeNtpServer, "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "time-disable-vmictimeprovider",
            Label = "Disable Hyper-V / VM Integration Time Sync",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["time", "hyper-v", "vm", "vmic", "virtualization"],
            Description =
                "Disables the VMICTimeProvider that synchronises the clock of Hyper-V "
                + "guest VMs from the host. Use when the guest should use an independent "
                + "NTP source instead of relying on the hypervisor clock.",
            ApplyOps = [RegOp.SetDword(VmicTimeProv, "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(VmicTimeProv, "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(VmicTimeProv, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "time-set-ntp-poll-interval-1h",
            Label = "Set NTP Client Poll Interval to 1 Hour",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["time", "ntp", "poll", "interval", "bandwidth"],
            Description =
                "Sets the NTP client SpecialPollInterval to 3600 seconds (1 hour). "
                + "Reduces NTP traffic on low-bandwidth or metered connections while "
                + "maintaining reasonable time accuracy.",
            ApplyOps = [RegOp.SetDword(W32TimeNtpClient, "SpecialPollInterval", 3600)],
            RemoveOps = [RegOp.DeleteValue(W32TimeNtpClient, "SpecialPollInterval")],
            DetectOps = [RegOp.CheckDword(W32TimeNtpClient, "SpecialPollInterval", 3600)],
        },
    ];
}

// === Merged from: DiskCleanup.cs ===

/// <summary>
/// Disk cleanup and storage hygiene tweaks — automated cleanup, temp files, caches,
/// thumbnail databases, delivery optimisation, and Windows Update cleanup.
/// </summary>
internal static class DiskCleanup
{
    private const string CuKey = @"HKEY_CURRENT_USER";
    private const string LmKey = @"HKEY_LOCAL_MACHINE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cleanup-disable-thumbnail-cache",
            Label = "Disable Thumbnail Cache (Thumbs.db)",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from creating Thumbs.db thumbnail cache files in folders.",
            Tags = ["cleanup", "disk", "thumbnails", "explorer"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbnailCache", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-thumbs-network",
            Label = "Disable Thumbnail Cache on Network Folders",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents creation of hidden Thumbs.db files on network shares.",
            Tags = ["cleanup", "disk", "network", "thumbnails"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbsDBOnNetworkFolders", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbsDBOnNetworkFolders"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableThumbsDBOnNetworkFolders", 1),
            ],
        },
        new TweakDef
        {
            Id = "cleanup-disable-delivery-optimisation",
            Label = "Disable Delivery Optimisation (P2P Updates)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables peer-to-peer update sharing which can consume bandwidth and cache disk space.",
            Tags = ["cleanup", "disk", "updates", "bandwidth"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-run-disk-cleanup-silent",
            Label = "Run Disk Cleanup (Silent, All Profiles)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Runs cleanmgr with all cleanup handlers enabled silently. Removes temp files, logs, caches, and old Windows installations.",
            Tags = ["cleanup", "disk", "temp", "maintenance"],
            ApplyAction = _ =>
            {
                // Set all cleanup handlers to active in Sageset profile 9999
                ShellRunner.RunPowerShell(
                    "$path = 'HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VolumeCaches'; "
                        + "Get-ChildItem $path | ForEach-Object { Set-ItemProperty $_.PSPath -Name StateFlags9999 -Value 2 -Type DWord -ErrorAction SilentlyContinue }"
                );
                ShellRunner.Run("cleanmgr.exe", ["/sagerun:9999"]);
            },
            RemoveAction = _ => { }, // One-shot cleanup, nothing to remove
            DetectAction = () => false, // One-shot action
        },
        new TweakDef
        {
            Id = "cleanup-clear-windows-temp",
            Label = "Clear Windows Temp Folder",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Removes all files from C:\\Windows\\Temp and user %TEMP% folders.",
            Tags = ["cleanup", "disk", "temp"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-Item \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-clear-windows-update-cache",
            Label = "Clear Windows Update Download Cache",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Stops Windows Update service, clears the SoftwareDistribution\\Download folder, then restarts the service.",
            Tags = ["cleanup", "disk", "windows-update", "cache"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service wuauserv -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:windir\\SoftwareDistribution\\Download\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Start-Service wuauserv -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-clear-font-cache",
            Label = "Clear Font Cache",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Stops the Font Cache service, clears cached font data, then restarts the service. Fixes font rendering issues.",
            Tags = ["cleanup", "disk", "fonts", "cache"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service FontCache -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:windir\\ServiceProfiles\\LocalService\\AppData\\Local\\FontCache\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Start-Service FontCache -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-clear-icon-cache",
            Label = "Clear Icon Cache Database",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Deletes the icon cache database and restarts Explorer. Fixes missing or corrupted icons.",
            Tags = ["cleanup", "disk", "icons", "explorer"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Process -Name explorer -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:LOCALAPPDATA\\IconCache.db\" -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item \"$env:LOCALAPPDATA\\Microsoft\\Windows\\Explorer\\iconcache*\" -Force -ErrorAction SilentlyContinue; "
                        + "Start-Process explorer.exe"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-enable-storage-sense",
            Label = "Enable Storage Sense (Automatic Cleanup)",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Storage Sense to automatically free disk space by cleaning temp files and Recycle Bin.",
            Tags = ["cleanup", "disk", "storage-sense", "automatic"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "04", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "08", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "32", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "2048", 30),
            ],
            RemoveOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 0)],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy", "01", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-hibernation",
            Label = "Disable Hibernation (Free hiberfil.sys Space)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables hibernation and deletes hiberfil.sys, recovering RAM-size disk space.",
            Tags = ["cleanup", "disk", "hibernation", "space"],
            SideEffects = "Hibernation and Fast Startup will be unavailable.",
            ApplyAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "off"]),
            RemoveAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "on"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("Test-Path \"$env:SystemDrive\\hiberfil.sys\"");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cleanup-compact-os",
            Label = "Enable Compact OS (Compress System Files)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Compresses Windows system files using compact.exe. Saves 2-3 GB on SSDs with minimal performance impact.",
            Tags = ["cleanup", "disk", "compact", "compression"],
            ApplyAction = _ => ShellRunner.Run("compact.exe", ["/compactos:always"]),
            RemoveAction = _ => ShellRunner.Run("compact.exe", ["/compactos:never"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("compact.exe", ["/compactos:query"]);
                return stdout.Contains("compacted", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cleanup-clear-event-logs",
            Label = "Clear All Windows Event Logs",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Clears all Windows Event logs. Use with caution — forensic evidence will be lost.",
            Tags = ["cleanup", "disk", "event-log", "maintenance"],
            SideEffects = "All event logs will be permanently deleted.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-WinEvent -ListLog * -ErrorAction SilentlyContinue | ForEach-Object { "
                        + "try { [System.Diagnostics.Eventing.Reader.EventLogSession]::GlobalSession.ClearLog($_.LogName) } catch {} }"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-disable-reserved-storage",
            Label = "Disable Reserved Storage (7 GB)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables Windows Reserved Storage which holds ~7 GB for updates. Frees disk space on small drives.",
            Tags = ["cleanup", "disk", "reserved-storage", "space"],
            ApplyAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Set-ReservedStorageState", "/State:Disabled"]),
            RemoveAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Set-ReservedStorageState", "/State:Enabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-ReservedStorageState"]);
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cleanup-clear-prefetch",
            Label = "Clear Prefetch Cache",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Clears the Windows Prefetch folder. System will rebuild it automatically.",
            Tags = ["cleanup", "disk", "prefetch", "cache"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Remove-Item \"$env:windir\\Prefetch\\*\" -Force -ErrorAction SilentlyContinue"),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cleanup-reduce-recycle-bin-size",
            Label = "Limit Recycle Bin to 5% of Drive",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the Recycle Bin maximum size to 5% of each drive instead of the default 10%.",
            Tags = ["cleanup", "disk", "recycle-bin"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "$shell = New-Object -ComObject Shell.Application; "
                        + "$rb = $shell.Namespace(10); "
                        + "# Note: Recycle Bin size is stored per-drive in the registry "
                        + "Set-ItemProperty 'HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\BitBucket\\Volume' -Name 'NukeOnDelete' -Value 0 -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        // ── Sprint 41 additions ────────────────────────────────────────────

        new TweakDef
        {
            Id = "cleanup-disable-recent-docs",
            Label = "Disable Recent Documents Tracking",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from recording recently opened documents in Jump Lists and Quick Access.",
            Tags = ["cleanup", "privacy", "recents"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-recent-programs",
            Label = "Disable Recent Programs in Start Menu",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides recently-used programs section from the Start menu.",
            Tags = ["cleanup", "privacy", "start"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-search-history",
            Label = "Disable Search History",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Search from persisting search queries to disk.",
            Tags = ["cleanup", "privacy", "search"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableSearchHistory", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableSearchHistory")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableSearchHistory", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-swap-file",
            Label = "Disable Swap File (SysMain paging supplement)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the secondary swapfile.sys used by Windows apps. Frees disk space on SSDs. Not recommended for systems with <8 GB RAM.",
            Tags = ["cleanup", "disk", "performance", "ssd"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SwapFileControl", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SwapFileControl")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SwapFileControl", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-auto-maintenance",
            Label = "Disable Automatic Maintenance Wake-ups",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from waking up the computer at night to run maintenance tasks.",
            Tags = ["cleanup", "maintenance", "power"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-volume-shadow-copy",
            Label = "Disable Volume Shadow Copy Service Writer",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the VSS system writer to reduce disk I/O. Affects System Restore and backup. Only for systems where backups are managed externally.",
            Tags = ["cleanup", "disk", "backup", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\VSS", "Start", 4)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-internet-temp-auto",
            Label = "Disable Automatic Deletion of Internet Temp Files",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Internet Explorer/Legacy Edge from automatically emptying the temp cache on browser exit.",
            Tags = ["cleanup", "browser", "temp"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache", "Persistent", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache", "Persistent")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Cache", "Persistent", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-wer-queue",
            Label = "Disable Windows Error Reporting Queue",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Error Reporting from queuing crash dumps and reports to disk.",
            Tags = ["cleanup", "crash", "wer", "disk"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-superfetch-write",
            Label = "Disable SuperFetch Disk Write Activity",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Reduces SysMain (SuperFetch) write activity to disk by disabling pre-population of the cache.",
            Tags = ["cleanup", "performance", "ssd", "superfetch"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "cleanup-limit-disk-usage-windows-update",
            Label = "Limit Windows Update Download Disk Cache",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the disk space Windows Update can use for its download cache to 1024 MB.",
            Tags = ["cleanup", "windows-update", "disk"],
            KindHint = TweakKind.PowerShell,
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\DeliveryOptimization' -Name 'AbsoluteMaxCacheSize' -Value 1024 -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-ItemProperty 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\DeliveryOptimization' -Name 'AbsoluteMaxCacheSize' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                try
                {
                    var val = Microsoft.Win32.Registry.GetValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization",
                        "AbsoluteMaxCacheSize",
                        null
                    );
                    return val is int v && v <= 1024;
                }
                catch
                {
                    return false;
                }
            },
        },
        new TweakDef
        {
            Id = "cleanup-limit-wer-max-queue",
            Label = "Limit Windows Error Reporting Queue to 1 Report",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxQueueCount=1 in Windows Error Reporting. At most one queued problem report is retained, preventing WER from accumulating large archives of crash data on disk.",
            Tags = ["cleanup", "wer", "crash", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "MaxQueueCount", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-set-minidump-count-1",
            Label = "Retain Only 1 Kernel Minidump File",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MinidumpsCount=1 in CrashControl. Only the most recent minidump is kept; older dumps are deleted automatically, preventing gradual accumulation in %SystemRoot%\\Minidump.",
            Tags = ["cleanup", "minidump", "crash", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "MinidumpsCount", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-crash-filter-pages",
            Label = "Filter Zero Pages from Kernel Crash Dump",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets FilterPages=1 in CrashControl. Zero-filled memory pages are excluded from crash dump files, significantly reducing dump size on systems with large amounts of idle RAM.",
            Tags = ["cleanup", "crash", "dump", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "FilterPages", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "FilterPages")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "FilterPages", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-installer-rollback",
            Label = "Disable MSI Installer Rollback (Save Disk Space)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets DisableRollback=1 via Windows Installer policy. Prevents MSI from creating rollback scripts and temporary backup files during installation. Saves significant disk space during large installs but removes the ability to undo a failed installation.",
            Tags = ["cleanup", "installer", "msi", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Installer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Installer", "DisableRollback", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Installer", "DisableRollback")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Installer", "DisableRollback", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-limit-restore-points-5pct",
            Label = "Limit System Restore Disk Usage to 5%",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DiskPercent=5 in SystemRestore policy. Caps the disk space allocated to System Restore shadow copies at 5%, freeing space currently consumed by older restore-point snapshots.",
            Tags = ["cleanup", "system-restore", "disk-space", "shadow-copy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-wer-show-ui",
            Label = "Suppress Windows Error Reporting UI Dialogs",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontShowUI=1 in Windows Error Reporting. WER operates silently in the background; no \"Check for Solution\" dialog is shown to the user. Eliminates interactive stalls after crashes.",
            Tags = ["cleanup", "wer", "ui", "dialog"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-activity-history",
            Label = "Disable Windows Activity History / Timeline",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableActivityFeed=0 via System policy. Stops Windows from recording app launches, file opens, and web browsing into the Timeline activity database, eliminating ongoing writes to the ActivitiesCache.db on disk.",
            Tags = ["cleanup", "activity-history", "timeline", "privacy", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-clipboard-sync",
            Label = "Disable Clipboard History (No Disk Persistence)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowClipboardHistory=0 via System policy. Prevents Windows from recording clipboard entries to disk. Stops clipboard data from being written to the activity store and synced across devices.",
            Tags = ["cleanup", "clipboard", "privacy", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowWindowsInkWorkspace=0 via policy. Removes the Windows Ink Workspace button and stops its background processes, eliminating related trace files and reducing RAM usage on non-tablet systems.",
            Tags = ["cleanup", "ink", "tablet", "debloat"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-search-removable-index",
            Label = "Disable Search Indexing on Removable Drives",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingRemovableDrive=1 in Windows Search policy. Stops the indexer from crawling USB and removable drives, preventing spurious index updates every time a drive is inserted.",
            Tags = ["cleanup", "search", "indexer", "usb", "removable"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingRemovableDrive", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingRemovableDrive")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingRemovableDrive", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-implicit-text-collection",
            Label = "Disable Implicit Text Data Collection",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RestrictImplicitTextCollection=1 in InputPersonalization. Stops Windows from silently harvesting typed text for handwriting/typing personalisation, eliminating writes to the per-user text data store.",
            Tags = ["cleanup", "privacy", "text", "personalization", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitTextCollection")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-implicit-ink-collection",
            Label = "Disable Implicit Ink/Handwriting Data Collection",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RestrictImplicitInkCollection=1 in InputPersonalization. Stops Windows from recording stylus and touch strokes for handwriting personalisation and sending them to Microsoft.",
            Tags = ["cleanup", "privacy", "ink", "handwriting", "personalization"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitInkCollection")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "cleanup-disable-enhanced-diagnostic-data",
            Label = "Disable Enhanced Windows Analytics Diagnostic Data",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowEnhancedDiagnosticDataWindowsAnalytics=0 in DataCollection policy. Prevents the enhanced diagnostic upload tier used by Windows Analytics/Update Compliance, reducing background telemetry writes and uploads.",
            Tags = ["cleanup", "telemetry", "privacy", "diagnostic", "disk-space"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection",
                    "AllowEnhancedDiagnosticDataWindowsAnalytics",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection",
                    "AllowEnhancedDiagnosticDataWindowsAnalytics"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection",
                    "AllowEnhancedDiagnosticDataWindowsAnalytics",
                    0
                ),
            ],
        },
    ];
}

// === Merged from: Printing.cs ===

internal static class Printing
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "printing-disable-spooler-autostart",
            Label = "Set Print Spooler to Manual Start",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Print Spooler service to Manual start. Reduces attack surface (PrintNightmare) and improves boot time if no printer is used.",
            Tags = ["printing", "spooler", "security", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 3)],
        },
        new TweakDef
        {
            Id = "printing-driver-isolation",
            Label = "Enable Print Driver Isolation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Runs third-party print drivers in an isolated process. Prevents a buggy driver from crashing the spooler service.",
            Tags = ["printing", "driver", "stability", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationGroups", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverrideCompat", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationGroups"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverrideCompat"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationGroups", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-pointandprint",
            Label = "Restrict Point-and-Print Drivers",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires admin approval for Point-and-Print driver installs. Mitigates PrintNightmare and similar spooler vulnerabilities.",
            Tags = ["printing", "security", "pointandprint", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-disable-xps-writer",
            Label = "Disable XPS Document Writer",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the Microsoft XPS Document Writer virtual printer from the printers list. Reduces clutter if you never use XPS.",
            Tags = ["printing", "xps", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableXPSDocumentWriter", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-internet-printing",
            Label = "Disable Internet Printing (IPP)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Internet Printing Protocol and Web PnP driver downloads. Reduces attack surface from internet-facing print services.",
            Tags = ["printing", "internet", "security", "ipp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnPDownload", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnPDownload"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-queue-logging",
            Label = "Disable Print Queue Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables event logging for print queue events. Reduces disk I/O from print job tracking. Default: 1 (Enabled). Recommended: 0 (Disabled).",
            Tags = ["printing", "logging", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Providers", "EventLog", 0)],
        },
        new TweakDef
        {
            Id = "printing-limit-spooler-memory",
            Label = "Limit Print Spooler Memory",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Print Spooler to normal priority. Prevents spooler from consuming excessive CPU during large print jobs. Default: Above Normal. Recommended: Normal.",
            Tags = ["printing", "memory", "performance", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerPriority", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-remote",
            Label = "Disable Remote Printing",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks incoming remote print connections. Reduces attack surface by preventing remote users from sending print jobs to this machine. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["printing", "remote", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableRemotePrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-restrict-driver-install",
            Label = "Restrict Printer Driver Installation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Restricts printer driver installation to administrators only. Mitigates PrintNightmare-class vulnerabilities. Default: not restricted. Recommended: restricted.",
            Tags = ["printing", "driver", "security", "restriction"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-print-disable-legacy-mode",
            Label = "Disable Print Spooler Legacy Mode",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables legacy default printer mode in the print spooler. Uses modern Windows-managed default printer instead. Default: Legacy. Recommended: Disabled (modern mode).",
            Tags = ["printing", "spooler", "legacy", "default-printer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "LegacyDefaultPrinterMode", 0)],
        },
        new TweakDef
        {
            Id = "printing-print-point-and-print-restrict",
            Label = "Enable Point and Print Restrictions",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables strict Point and Print restrictions: trusted servers only, no silent installs, UAC prompts on updates. Mitigates PrintNightmare. Default: unrestricted. Recommended: restricted.",
            Tags = ["printing", "point-and-print", "security", "printnightmare"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "TrustedServers", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "InForest", 0),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall",
                    0
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "UpdatePromptSettings", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "TrustedServers"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "InForest"),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "NoWarningNoElevationOnInstall"
                ),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "UpdatePromptSettings"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-remote-inbound",
            Label = "Disable Remote Inbound Printing",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables accepting inbound print jobs from remote machines. Default: enabled.",
            Tags = ["printing", "remote", "inbound", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "RegisterSpoolerRemoteRpcEndPoint", 2),
            ],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-isolation",
            Label = "Enforce Printer Driver Isolation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces printer drivers to load in isolated processes. Prevents a buggy driver from crashing the spooler. Default: per-driver setting.",
            Tags = ["printing", "driver", "isolation", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverride", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverride")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "PrintDriverIsolationOverride", 2)],
        },
        new TweakDef
        {
            Id = "printing-disable-printer-sharing",
            Label = "Disable Printer Sharing Across Network",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sharing printers with other network computers. Default: not shared.",
            Tags = ["printing", "sharing", "network", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableServerThread", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableServerThread")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableServerThread", 1)],
        },
        new TweakDef
        {
            Id = "printing-copy-files-policy",
            Label = "Disable Point and Print Copy Files",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables copying of driver files during Point and Print connections. Mitigates PrintNightmare-class driver injection attacks. Default: allowed.",
            Tags = ["printing", "point-and-print", "copy-files", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-disable-client-side-map",
            Label = "Disable Client-Side Printer Mapping",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic mapping of client printers in remote sessions. Reduces RDP session overhead. Default: enabled.",
            Tags = ["printing", "client-side", "mapping", "rdp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-default-mgmt",
            Label = "Disable Windows Manage Default Printer",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically changing the default printer based on the last printer used at each network. Default: managed.",
            Tags = ["printing", "default", "management", "automatic"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-spooler-log",
            Label = "Disable Print Spooler Event Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables verbose event logging by the Print Spooler service. Reduces disk I/O from spooler log writes. Default: enabled.",
            Tags = ["printing", "spooler", "logging", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerEventLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerEventLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "SpoolerEventLogging", 0)],
        },
        new TweakDef
        {
            Id = "printing-emf-despooling",
            Label = "Enable EMF Despooling for Faster Printing",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables EMF despooling for faster print rendering. Applications regain control sooner while printing continues in background. Default: off.",
            Tags = ["printing", "emf", "spool", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "ForceEMFDespooling", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "ForceEMFDespooling")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "ForceEMFDespooling", 1)],
        },
        new TweakDef
        {
            Id = "printing-package-point-server-list",
            Label = "Restrict Package Point and Print Servers",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the approved Package Point and Print server list. Only servers on the approved list can install print drivers via Point and Print. Default: unrestricted.",
            Tags = ["printing", "package", "point-and-print", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-print-default-paper-a4",
            Label = "Set Default Paper Size to A4",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default paper size to A4 (international standard). Changes from US Letter default. Default: Letter.",
            Tags = ["printing", "paper", "a4", "default"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "DefaultPaperSize", "A4")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "DefaultPaperSize")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "DefaultPaperSize", "A4")],
        },
        new TweakDef
        {
            Id = "printing-disable-ipp-web-client",
            Label = "Disable IPP Web Printing Client",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Internet Printing Protocol (IPP) client feature. Prevents connecting to web-hosted printers. Default: enabled.",
            Tags = ["printing", "internet", "ipp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-notifications",
            Label = "Disable Print Job Notifications",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses toast notifications for print job completion. Default: notifications enabled.",
            Tags = ["printing", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.PrintDialog",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-disable-lpr-monitor",
            Label = "Disable LPR Port Monitor",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the LPR (Line Printer Remote) port monitor. Not needed on modern networks without legacy Unix/Linux printers. Default: enabled.",
            Tags = ["printing", "lpr", "legacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lpdsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-workflow-svc",
            Label = "Disable Print Workflow Service",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Print Workflow service used for custom print dialogs in Store apps. Default: manual start.",
            Tags = ["printing", "workflow", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PrintWorkflowUserSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "printing-spooler-crash-recovery-off",
            Label = "Disable Spooler Auto-Restart on Crash",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic restart of the print spooler on failure. Useful in hardened server environments. Default: auto-restart enabled.",
            Tags = ["printing", "spooler", "services", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "FailureActions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "FailureActions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "FailureActions", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-shared-printer-browse",
            Label = "Disable Printer Browsing on Network",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic browsing and publishing of shared printers on the network. Reduces network noise. Default: enabled.",
            Tags = ["printing", "network", "sharing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableBrowsing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableBrowsing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableBrowsing", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-web-pnp",
            Label = "Disable Web-Based Printer Plug and Play",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Web Plug and Play printer discovery (DisableWebPnP). Prevents printers from being located via internet/intranet discovery protocols. Default: web PnP enabled.",
            Tags = ["printing", "pnp", "network", "discovery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnP", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnP")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableWebPnP", 1)],
        },
        new TweakDef
        {
            Id = "printing-restrict-driver-to-admins",
            Label = "Restrict Printer Driver Installation to Admins",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Requires administrator rights to install printer drivers. Prevents standard users from installing untrusted print drivers. Default: users can install drivers.",
            Tags = ["printing", "drivers", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers",
                    "RestrictDriverInstallationToAdministrators",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "printing-block-kernel-mode-drivers",
            Label = "Block Kernel-Mode Printer Drivers",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks kernel-mode printer drivers from running. Forces all print drivers to user mode (V4 drivers), reducing kernel attack surface. Default: kernel-mode drivers permitted.",
            Tags = ["printing", "drivers", "security", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "KMPrintersAreBlocked", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "KMPrintersAreBlocked")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "KMPrintersAreBlocked", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-ipp-over-usb",
            Label = "Disable IPP over USB Printing",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Internet Printing Protocol over USB (IPP-USB). Prevents modern USB printers from using the IPP-USB communication path. Default: IPP-USB enabled.",
            Tags = ["printing", "ipp", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB", "DisableIPOverUSB", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB", "DisableIPOverUSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPOverUSB", "DisableIPOverUSB", 1)],
        },
        new TweakDef
        {
            Id = "printing-enable-client-side-render",
            Label = "Enable Client-Side Print Rendering",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Enhanced Metafile (EMF) de-spooling so the client renders print jobs locally. Reduces print server CPU and network load. Default: server-side rendering.",
            Tags = ["printing", "performance", "rendering"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "EMFDespoolingEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "EMFDespoolingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "EMFDespoolingEnabled", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-ad-publish",
            Label = "Disable Auto-Publish Printers to Active Directory",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents printers from being automatically published to Active Directory. Reduces AD directory pollution on workgroup machines. Default: auto-publish enabled.",
            Tags = ["printing", "active-directory", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "PublishPrinters", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "PublishPrinters")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "PublishPrinters", 0)],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-auto-update",
            Label = "Disable Automatic Printer Driver Updates",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic Windows Update-driven printer driver updates. Prevents unexpected driver changes that can break printing configuration. Default: auto-update enabled.",
            Tags = ["printing", "drivers", "windows-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "AutoUpdateDriverEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "AutoUpdateDriverEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "AutoUpdateDriverEnabled", 0)],
        },
    ];
}

// ── Merged from PrinterAdvanced.cs ──────────────────────────────────────────────────

internal static class PrinterAdvanced
{
    private const string WsdPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD";
    private const string PrintPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string SpoolerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Workflow";
    private const string IppPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string AuditPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prnta-disable-wsd-printer-discovery",
            Label = "Disable WSD (Web Services on Devices) Printer Discovery",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "network", "discovery", "security"],
            Description =
                "Disables the WSD (Web Services on Devices) port monitor, preventing Windows from auto-discovering "
                + "network printers via SOAP/WSD. Reduces broadcast network noise and eliminates a legacy protocol attack surface.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableWSDPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableWSDPrinting")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableWSDPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-require-https-ipp-printing",
            Label = "Require HTTPS for Internet Printing Protocol (IPP)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "ipp", "https", "security", "tls", "network"],
            Description =
                "Forces Windows to only accept IPP (Internet Printing Protocol) connections over HTTPS (port 443/631 TLS). "
                + "Prevents cleartext print job data from being transmitted over the network.",
            ApplyOps = [RegOp.SetDword(PrintPol, "ForceIPPSsl", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "ForceIPPSsl")],
            DetectOps = [RegOp.CheckDword(PrintPol, "ForceIPPSsl", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-print-workflow-service",
            Label = "Disable Print Workflow App Integration",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "workflow", "uwp", "policy", "attack surface"],
            Description =
                "Prevents third-party UWP applications from registering as print workflow apps. "
                + "Eliminates the risk of a malicious workflow app intercepting or modifying print jobs.",
            ApplyOps = [RegOp.SetDword(SpoolerPol, "DisablePrintWorkflow", 1)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPol, "DisablePrintWorkflow")],
            DetectOps = [RegOp.CheckDword(SpoolerPol, "DisablePrintWorkflow", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-ports",
            Label = "Disable WSD Port Monitor Installation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "port monitor", "security", "lateral movement"],
            Description =
                "Blocks installation of the WSD port monitor via Group Policy. "
                + "WSD ports have been used in lateral-movement scenarios; disabling the monitor prevents auto-creation.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor", 1)],
        },
        new TweakDef
        {
            Id = "prnta-enable-spooler-audit",
            Label = "Enable Print Spooler Service Audit Events",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "audit", "spooler", "logging", "security"],
            Description =
                "Enables detailed audit logging for the Print Spooler service in the Windows event log. "
                + "Required to detect PrintNightmare-style exploitation attempts and unauthorized driver installs.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-package-point-and-print",
            Label = "Disable Package Point-and-Print Non-Admin Install",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "security", "package point and print", "driver", "policy"],
            Description =
                "Restricts Package Point-and-Print to prevent non-admin users from installing packaged print drivers. "
                + "Closes a known elevation vector where a malicious print server could install arbitrary kernel drivers.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "prnta-restrict-printer-connection-unsigned",
            Label = "Disallow Connecting to Servers with Unsigned Printer Drivers",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "security", "unsigned", "driver", "policy"],
            Description =
                "Prevents Windows from connecting to a print server if the server's driver package is unsigned. "
                + "Ensures all automatically installed print drivers pass Windows Signature Enforcement (WHQL).",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-banner-page",
            Label = "Disable Printer Banner/Separator Page",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "paper", "waste", "eco", "banner page"],
            Description =
                "Removes the separator cover page that some print servers insert before each print job. "
                + "Eliminates wasted paper and toner on shared printers in small-office environments.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-lpr-port",
            Label = "Disable LPR Port Monitor (Legacy Unix Printing)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "lpr", "legacy", "security", "port monitor"],
            Description =
                "Disables the legacy LPR/LPD (Line Printer Remote) port monitor via Group Policy. "
                + "LPR is an unencrypted 1980s printing protocol; disabling it eliminates a legacy attack surface.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort", 1)],
        },
        new TweakDef
        {
            Id = "prnta-require-https-spooler",
            Label = "Require HTTPS for Print Spooler Remote Connections",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "spooler", "https", "tls", "security"],
            Description =
                "Enforces TLS-encrypted HTTPS for all inbound remote print spooler "
                + "connections, blocking unencrypted (HTTP) print job submissions "
                + "across the network. Requires the spooler to present a valid certificate.",
            ApplyOps = [RegOp.SetDword(PrintPol, "EnableTLSForHTTPSPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "EnableTLSForHTTPSPrinting")],
            DetectOps = [RegOp.CheckDword(PrintPol, "EnableTLSForHTTPSPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-restrict-driver-install-admin",
            Label = "Restrict Print Driver Installation to Administrators",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "driver", "admin", "security", "privilege escalation"],
            Description =
                "Prevents standard users from installing print drivers. "
                + "Unvetted print drivers run in kernel space and are a documented "
                + "privilege-escalation vector (PrintNightmare/CVE-2021-34527 family).",
            ApplyOps = [RegOp.SetDword(PrintPol, "RestrictDriverInstallationToAdministrators", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "RestrictDriverInstallationToAdministrators")],
            DetectOps = [RegOp.CheckDword(PrintPol, "RestrictDriverInstallationToAdministrators", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-cloud-print",
            Label = "Disable Microsoft Cloud Print (Print to Cloud)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "cloud print", "microsoft", "privacy"],
            Description =
                "Blocks the Microsoft Cloud Print service (formerly Mopria) from "
                + "enumerating and uploading spool data to Microsoft cloud endpoints. "
                + "Keeps all print jobs local.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisablePrinterCloudPrint", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisablePrinterCloudPrint")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisablePrinterCloudPrint", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-multicast-discovery",
            Label = "Disable WSD Multicast Printer Discovery",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "discovery", "multicast", "network"],
            Description =
                "Prevents the Web Services on Devices (WSD) listener from responding "
                + "to multicast discovery probes on the local subnet. "
                + "Reduces unsolicited network chatter and removes WSD as an attack surface.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableDiscovery")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-win32-spool-com",
            Label = "Disable Windows 32-Bit Spooler COM Object",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "spooler", "com", "security", "legacy"],
            Description =
                "Disables the legacy 32-bit in-process COM spooler extensions. "
                + "These extensions can be abused to load arbitrary DLLs into the print spooler process "
                + "under SYSTEM context — a known persistence vector.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableWebPnpDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableWebPnpDownload")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableWebPnpDownload", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-rpc-over-http-spooler",
            Label = "Disable RPC-over-HTTP for Spooler (Restrict to Named Pipes)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "rpc", "http", "spooler", "network", "security"],
            Description =
                "Restricts inbound RPC connections to the print spooler to named "
                + "pipes only, blocking RPC-over-HTTP transport. Reduces remote "
                + "exploit surface for CVE-2021-1675 / PrintNightmare variants.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-internet-print-client",
            Label = "Disable Internet Printing Client",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "internet printing", "ipp", "feature", "security"],
            Description =
                "Disables the Windows Internet Printing Client component "
                + "(connects to HTTP/HTTPS printers by URL). Closes an infrequently "
                + "used remote printing feature that can be abused for SSRF and "
                + "credential-relay attacks.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableHTTPPrintingClient", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableHTTPPrintingClient")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableHTTPPrintingClient", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-printer-announce",
            Label = "Disable WSD Printer Announce (Host Advertising)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "announcement", "network", "privacy"],
            Description =
                "Stops Windows from broadcasting WSD printer-announcement packets "
                + "on the network (Hello/Bye messages). Prevents other hosts on "
                + "the subnet from seeing shared printers.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableAnnouncement", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableAnnouncement")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableAnnouncement", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-driver-update-from-wu",
            Label = "Block Automatic Print Driver Updates via Windows Update",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "driver", "windows update", "wu", "policy"],
            Description =
                "Prevents Windows Update from automatically pushing print driver "
                + "updates. Automatic driver installs have been weaponised by "
                + "PrintNightmare-class vulnerabilities; use WSUS or manual approval instead.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableAutoInstallDriverViaPnP", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableAutoInstallDriverViaPnP")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableAutoInstallDriverViaPnP", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-inbound-print-spooler",
            Label = "Disable Inbound Remote Print Connections (Spooler Server)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["printing", "spooler", "remote", "network", "security"],
            Description =
                "Blocks inbound remote connections to the Windows print spooler "
                + "service. Workstations should not accept remote print jobs; "
                + "this policy closes the highest-impact PrintNightmare attack path "
                + "without disabling the spooler entirely (local printing still works).",
            ApplyOps = [RegOp.SetDword(PrintPol, "NoAddPrinter", 0), RegOp.SetDword(PrintPol, "DisableSpoolerRemote", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableSpoolerRemote"), RegOp.DeleteValue(PrintPol, "NoAddPrinter")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableSpoolerRemote", 1)],
        },
    ];
}

// ── merged from EventLogging.cs ────────────────────────────────────────
internal static class EventLogging
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string EventLogKey = $@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "evtlog-increase-system-log-size",
            Label = "Increase System Event Log to 64 MB",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the System event log maximum size from 20 MB to 64 MB for better diagnostic retention.",
            Tags = ["event-log", "diagnostics", "system", "capacity"],
            RegistryKeys = [$@"{EventLogKey}\System"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 67108864)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\System", "MaxSize", 67108864)],
        },
        new TweakDef
        {
            Id = "evtlog-increase-security-log-size",
            Label = "Increase Security Event Log to 128 MB",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Security event log maximum size to 128 MB for better audit trail retention.",
            Tags = ["event-log", "security", "audit", "capacity"],
            RegistryKeys = [$@"{EventLogKey}\Security"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 134217728)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Security", "MaxSize", 134217728)],
        },
        new TweakDef
        {
            Id = "evtlog-increase-application-log-size",
            Label = "Increase Application Event Log to 64 MB",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Application event log maximum size from 20 MB to 64 MB.",
            Tags = ["event-log", "application", "capacity"],
            RegistryKeys = [$@"{EventLogKey}\Application"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 67108864)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Application", "MaxSize", 67108864)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-powershell-script-block-logging",
            Label = "Enable PowerShell Script Block Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables detailed PowerShell script block logging. Essential for security monitoring and incident response.",
            Tags = ["event-log", "powershell", "security", "audit", "logging"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 1),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-enable-powershell-module-logging",
            Label = "Enable PowerShell Module Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables logging of PowerShell module invocations for security auditing.",
            Tags = ["event-log", "powershell", "security", "audit", "module"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-process-creation-audit",
            Label = "Enable Process Creation Auditing",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Includes command-line arguments in process creation audit events (Event ID 4688).",
            Tags = ["event-log", "security", "audit", "process", "forensics"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-set-crash-dump-mini",
            Label = "Set Crash Dumps to Mini Dump",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Windows to create small memory dumps (mini dumps) instead of full dumps. Saves disk space.",
            Tags = ["event-log", "crash", "dump", "disk-space", "diagnostics"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-auto-reboot-on-crash",
            Label = "Disable Auto-Reboot on Blue Screen",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents automatic reboot after a BSOD so you can read the error message and stop code.",
            Tags = ["event-log", "crash", "bsod", "diagnostics", "debug"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-verbose-boot-status",
            Label = "Enable Verbose Boot Status Messages",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Shows detailed status messages during boot/shutdown instead of generic 'Please wait'. Helps diagnose slow boot issues.",
            Tags = ["event-log", "boot", "diagnostics", "verbose", "troubleshooting"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "VerboseStatus", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-shutdown-reason",
            Label = "Enable Shutdown Event Tracker",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prompts for a reason when shutting down or restarting the system. Useful for server/audit scenarios.",
            Tags = ["event-log", "shutdown", "audit", "server", "tracking"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonOn", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonOn")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\Reliability", "ShutdownReasonOn", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-log-retention-overwrite",
            Label = "Set Event Logs to Overwrite as Needed",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures all event logs to overwrite old entries when full. Prevents log overflow causing service failures.",
            Tags = ["event-log", "retention", "overwrite", "maintenance"],
            RegistryKeys = [$@"{EventLogKey}\Application", $@"{EventLogKey}\System", $@"{EventLogKey}\Security"],
            ApplyOps =
            [
                RegOp.SetDword($@"{EventLogKey}\Application", "Retention", 0),
                RegOp.SetDword($@"{EventLogKey}\System", "Retention", 0),
                RegOp.SetDword($@"{EventLogKey}\Security", "Retention", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{EventLogKey}\Application", "Retention"),
                RegOp.DeleteValue($@"{EventLogKey}\System", "Retention"),
                RegOp.DeleteValue($@"{EventLogKey}\Security", "Retention"),
            ],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Application", "Retention", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-event-forwarding",
            Label = "Disable Windows Event Forwarding",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Event Forwarding subscription service. Prevents events from being sent to remote collectors.",
            Tags = ["event-log", "forwarding", "privacy", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager", "Enabled")],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\EventLog\EventForwarding\SubscriptionManager", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-set-app-log-32mb",
            Label = "Set Application Event Log to 32 MB",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Application event log maximum size to 32 MB for better diagnostic history.",
            Tags = ["event-log", "application", "size", "diagnostics"],
            RegistryKeys = [$@"{EventLogKey}\Application"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 33554432)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Application", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Application", "MaxSize", 33554432)],
        },
        new TweakDef
        {
            Id = "evtlog-set-system-log-32mb",
            Label = "Set System Event Log to 32 MB",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the System event log maximum size to 32 MB for better diagnostic history.",
            Tags = ["event-log", "system", "size", "diagnostics"],
            RegistryKeys = [$@"{EventLogKey}\System"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 33554432)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\System", "MaxSize", 33554432)],
        },
        new TweakDef
        {
            Id = "evtlog-set-security-log-64mb",
            Label = "Set Security Event Log to 64 MB",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the Security event log maximum size to 64 MB. Critical for security auditing in hardened environments.",
            Tags = ["event-log", "security", "size", "auditing", "hardening"],
            RegistryKeys = [$@"{EventLogKey}\Security"],
            ApplyOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 67108864)],
            RemoveOps = [RegOp.SetDword($@"{EventLogKey}\Security", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{EventLogKey}\Security", "MaxSize", 67108864)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-event-tracing-autologger",
            Label = "Disable Autologger Event Tracing Sessions",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables several Autologger ETW sessions that perform background diagnostic tracing.",
            Tags = ["event-log", "etw", "autologger", "performance", "privacy"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-powershell-logging",
            Label = "Disable PowerShell Script Block Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables PowerShell script block logging that records all executed scripts to the event log.",
            Tags = ["event-log", "powershell", "logging", "privacy", "performance"],
            SideEffects = "Reduces forensic capability for detecting malicious PowerShell scripts.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 0),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-enable-powershell-transcription",
            Label = "Enable PowerShell Transcription Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables PowerShell transcription to log all input/output to a file for forensic auditing.",
            Tags = ["event-log", "powershell", "transcription", "auditing", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription", "EnableTranscripting", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription", "EnableTranscripting")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription", "EnableTranscripting", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-enable-command-line-auditing",
            Label = "Enable Process Command-Line Auditing",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Logs the full command line for process creation events (Event ID 4688). Critical for forensic analysis.",
            Tags = ["event-log", "auditing", "command-line", "forensics", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit",
                    "ProcessCreationIncludeCmdLine_Enabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-disable-srum",
            Label = "Disable System Resource Usage Monitor (SRUM)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables SRUM which tracks application resource usage, network activity, and energy data.",
            Tags = ["event-log", "srum", "tracking", "privacy", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-application-log",
            Label = "Limit Application Event Log Size",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the Application event log maximum size to 1 MB and enables auto-overwrite to free disk space.",
            Tags = ["event-log", "disk", "maintenance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "MaxSize", 1048576),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "Retention", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "MaxSize", 20971520),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "Retention", 0),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Application", "MaxSize", 1048576)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-system-log",
            Label = "Limit System Event Log Size",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the System event log maximum size to 1 MB and enables auto-overwrite.",
            Tags = ["event-log", "disk", "maintenance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\System"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 1048576),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\System", "Retention", 0),
            ],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\System", "MaxSize", 1048576)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-security-audit-logon",
            Label = "Disable Logon Failure Audit",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables auditing of failed logon attempts in the Security event log. Reduces event log spam on unattended machines.",
            Tags = ["event-log", "security", "audit", "logon"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "AuditBaseObjects", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "AuditBaseObjects")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Lsa", "AuditBaseObjects", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-powershell-scriptblock-logging",
            Label = "Disable PowerShell Script Block Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables PowerShell script block logging in the event log. Reduces privacy exposure from logged command content.",
            Tags = ["event-log", "powershell", "privacy", "logging"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging", "EnableScriptBlockLogging", 0),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-disable-module-logging",
            Label = "Disable PowerShell Module Logging",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables PowerShell module logging, preventing every module command from being recorded in the event log.",
            Tags = ["event-log", "powershell", "module", "logging"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ModuleLogging", "EnableModuleLogging", 0)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-windows-error-reporting-log",
            Label = "Disable WER Event Log Entries",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Error Reporting from writing crash and hang events to the Application event log.",
            Tags = ["event-log", "wer", "crash", "diagnostics"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "LoggingDisabled", 1)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-setup-log",
            Label = "Limit Setup Event Log Size",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the Setup event log to 1 MB with auto-overwrite, preventing unbounded growth on frequently updated systems.",
            Tags = ["event-log", "disk", "setup", "maintenance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "MaxSize", 1048576),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "Retention", 0),
            ],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "MaxSize", 20971520)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\EventLog\Setup", "MaxSize", 1048576)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-forwarded-log",
            Label = "Disable Forwarded Events Log",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Event Log Forwarding service (Wecsvc) used to forward events to a remote collector. Not needed on standalone PCs.",
            Tags = ["event-log", "forwarding", "network", "svc"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Wecsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "evtlog-disable-dns-client-log",
            Label = "Disable DNS Resolver Event Tracing",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables DNS client operational event logging in the Microsoft-Windows-DNS-Client/Operational channel to reduce disk I/O.",
            Tags = ["event-log", "dns", "network", "tracing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational",
                    "Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\WINEVT\Channels\Microsoft-Windows-DNS-Client/Operational",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "evtlog-disable-kernel-event-tracing",
            Label = "Disable NT Kernel Logger Session",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the NT Kernel Logger ETW session to not auto-start, reducing baseline CPU and disk overhead.",
            Tags = ["event-log", "etw", "kernel", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger", "Start", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger", "Start", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\NtKernelLogger", "Start", 0)],
        },
    ];
}

// ── merged from ScheduledTasks.cs ──
internal static class ScheduledTasks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "schtask-task-disable-maps-update",
            Label = "Disable Offline Maps Auto-Update",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic download and update of offline maps data. Saves bandwidth and storage. Default: enabled. Recommended: 0 (disabled).",
            Tags = ["tasks", "maps", "bandwidth", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AllowUntriggeredNetworkTrafficOnSettingsPage", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AllowUntriggeredNetworkTrafficOnSettingsPage"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData", 0)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostics Data Collection",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the disk diagnostic data collector scheduled task. Default: enabled. Recommended: 0 (disabled).",
            Tags = ["tasks", "disk", "diagnostics", "telemetry"],
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
            Id = "schtask-disable-ceip",
            Label = "Disable CEIP Data Collection (Policy)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Customer Experience Improvement Program data collection via policy. Stops CEIP telemetry scheduled tasks. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "ceip", "telemetry", "policy"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient",
            ],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-disk-diag",
            Label = "Disable Disk Diagnostics Data Collector",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Disk Diagnostics data collector scheduled task. Stops disk telemetry reporting to Microsoft. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "disk", "diagnostics", "telemetry"],
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
            Id = "schtask-disable-mrt-update",
            Label = "Disable MRT Automatic Update via WU",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Malicious Software Removal Tool from being offered through Windows Update Automatic Updates. Default: Offered. Recommended: Blocked for controlled environments.",
            Tags = ["tasks", "mrt", "malware", "update", "wu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-smartscreen",
            Label = "Disable SmartScreen Background Updates",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows SmartScreen via policy, stopping background filter data updates. Reduces network calls for reputation checking. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "smartscreen", "filter", "update", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostic Scheduled Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the DiskDiagnosticDataCollector that sends disk health data to Microsoft. Default: enabled.",
            Tags = ["scheduled-tasks", "disk", "diagnostics", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}",
                    "ScenarioExecutionEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-disable-ngen-log",
            Label = "Disable .NET Framework NGen Log Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the .NET Framework NGen log compilation task that precompiles .NET assemblies in the background. Saves CPU cycles. Default: enabled.",
            Tags = ["scheduled-tasks", "ngen", "dotnet", "performance"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-disable-power-diagnostics",
            Label = "Disable Power Efficiency Diagnostics",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Power Efficiency Diagnostics task that analyses power consumption. Reduces background CPU and I/O. Default: enabled.",
            Tags = ["scheduled-tasks", "power", "diagnostics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics", "EnabledExecution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics", "EnabledExecution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics", "EnabledExecution", 0)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-diagtrack-autologger",
            Label = "Disable DiagTrack AutoLogger Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DiagTrack AutoLogger ETW session that collects telemetry data in the background. Reduces CPU and I/O overhead. Default: enabled.",
            Tags = ["scheduled-tasks", "diagtrack", "autologger", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0),
            ],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-diagtrack-service",
            Label = "Disable Connected User Experiences Service",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DiagTrack (Connected User Experiences and Telemetry) service. Stops telemetry data collection at the service level. Default: automatic.",
            Tags = ["scheduled-tasks", "diagtrack", "telemetry", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 4)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-maintenance-wakeup",
            Label = "Disable Maintenance Wake Timer",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents automatic maintenance from waking the computer from sleep. Stops surprise middle-of-night wake events. Default: enabled.",
            Tags = ["scheduled-tasks", "maintenance", "wake", "sleep"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-scheduled-diagnostics",
            Label = "Disable Scheduled Diagnostics Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows scheduled diagnostics task that analyses system performance and reports issues. Reduces background activity. Default: enabled.",
            Tags = ["scheduled-tasks", "diagnostics", "performance", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "DisableQueryRemoteServer",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "DisableQueryRemoteServer"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "DisableQueryRemoteServer",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-disable-defender-scheduled-scan",
            Label = "Disable Windows Defender Scheduled Full Scan (GPO)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Defender automatic full-disk scheduled scan via Group Policy. Reduces background CPU/IO spikes from periodic full scans. Real-time protection remains active. Default: scheduled scan enabled.",
            Tags = ["scheduled-tasks", "defender", "scan", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableScheduledScanning", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableScheduledScanning")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableScheduledScanning", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-mrt-run",
            Label = "Block Malicious Removal Tool Execution",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Microsoft Windows Malicious Software Removal Tool (MRT) from running via Group Policy (DontRunMRT=1). Stops the monthly MRT execution that scans for specific malware. Default: MRT runs monthly.",
            Tags = ["scheduled-tasks", "mrt", "malware", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontRunMRT", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontRunMRT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontRunMRT", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-ngen-32",
            Label = "Disable .NET NGEN Pre-JIT Service (32-bit)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the 32-bit CLR Optimization Service (clr_optimization_v4.0.30319_32) which background-compiles .NET assemblies at idle. Can cause CPU spikes during gaming or presentations. Default: automatic.",
            Tags = ["scheduled-tasks", "ngen", "dotnet", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32", "Start", 4)],
        },
        new TweakDef
        {
            Id = "schtask-disable-ngen-64",
            Label = "Disable .NET NGEN Pre-JIT Service (64-bit)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the 64-bit CLR Optimization Service (clr_optimization_v4.0.30319_64) which idle-compiles native images for .NET assemblies. Prevents background CPU spikes from the JIT optimization jobs. Default: automatic.",
            Tags = ["scheduled-tasks", "ngen", "dotnet", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64", "Start", 4)],
        },
        new TweakDef
        {
            Id = "schtask-disable-app-usage-record",
            Label = "Disable Application Usage Recording (AppCompat)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Application Compatibility user action recorder (DisableUserActionRecord=1) that logs which apps are run and when. Reduces AppCompat background telemetry data collection. Default: enabled.",
            Tags = ["scheduled-tasks", "compat", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUserActionRecord", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUserActionRecord")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUserActionRecord", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-inventory",
            Label = "Disable Application Compatibility Inventory Collection",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the scheduled AppCompat inventory collection task that catalogues installed applications. Reduces background CPU and disk usage from inventory scans. Default: enabled.",
            Tags = ["scheduled-tasks", "compat", "inventory", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-winsat-rating",
            Label = "Block Windows Experience Index (WinSAT) Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the Windows System Assessment Tool (WinSAT) from running by setting BlockWinSAT=1. WinSAT benchmarks hardware for the Windows Experience Index score; disabling eliminates periodic benchmark runs. Default: enabled.",
            Tags = ["scheduled-tasks", "winsat", "benchmark", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat", "BlockWinSAT", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat", "BlockWinSAT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat", "BlockWinSAT", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-autoplay",
            Label = "Disable AutoPlay for All Media and Devices (GPO)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables AutoPlay for all media types and devices via Group Policy. Prevents automatic execution of code when USB drives, CDs, or other removable media are inserted. Default: enabled per-device.",
            Tags = ["scheduled-tasks", "autoplay", "usb", "security", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay", "DisableAutoplay", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay", "DisableAutoplay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay", "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-fault-tolerant-heap",
            Label = "Disable Fault Tolerant Heap (FTH)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Fault Tolerant Heap (FTH) service which monitors crashing applications and silently patches their heap allocations to prevent re-crashes. Removes memory overhead and startup interference from FTH monitoring. Default: enabled.",
            Tags = ["scheduled-tasks", "fth", "heap", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-spotlight-features",
            Label = "Disable All Windows Spotlight Features (GPO)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables all Windows Spotlight features globally via Group Policy. Prevents Spotlight from running background tasks to fetch and rotate AI-curated images, tips, and ads. Default: enabled for eligible Windows editions.",
            Tags = ["scheduled-tasks", "spotlight", "ai", "gpo", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightFeatures"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightFeatures", 1),
            ],
        },
    ];
}

// ── Merged from ScheduledTaskTweaks.cs ──────────────────────────────────────────────────

internal static class ScheduledTaskTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pst-disable-customer-experience",
            Label = "Disable Customer Experience Improvement Program Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables CEIP data collection scheduled tasks (Consolidator, UsbCeip, KernelCeipTask).",
            Tags = ["scheduledtask", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\KernelCeipTask')) { "
                        + "Disable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\KernelCeipTask')) { "
                        + "Enable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 2;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-app-telemetry",
            Label = "Disable Application Telemetry Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Application Experience and Compatibility Appraiser data collection tasks.",
            Tags = ["scheduledtask", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser',"
                        + "'\\Microsoft\\Windows\\Application Experience\\ProgramDataUpdater',"
                        + "'\\Microsoft\\Windows\\Application Experience\\StartupAppTask')) { "
                        + "Disable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser',"
                        + "'\\Microsoft\\Windows\\Application Experience\\ProgramDataUpdater',"
                        + "'\\Microsoft\\Windows\\Application Experience\\StartupAppTask')) { "
                        + "Enable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 2;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-windows-maps-update",
            Label = "Disable Windows Maps Update Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the offline maps background update scheduled task.",
            Tags = ["scheduledtask", "network", "disk"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsUpdateTask' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsToastTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsUpdateTask' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsToastTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-feedback-hub",
            Label = "Disable Feedback Hub Scheduled Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Feedback Hub and SIUF data collection tasks.",
            Tags = ["scheduledtask", "telemetry", "privacy", "feedback"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClient' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClientOnScenarioDownload' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClient' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClientOnScenarioDownload' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostic Data Collection",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the disk diagnostic data collector scheduled task.",
            Tags = ["scheduledtask", "telemetry", "disk", "diagnostics"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\DiskDiagnostic\\' -TaskName 'Microsoft-Windows-DiskDiagnosticDataCollector' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\DiskDiagnostic\\' -TaskName 'Microsoft-Windows-DiskDiagnosticDataCollector' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\DiskDiagnostic\\' -TaskName 'Microsoft-Windows-DiskDiagnosticDataCollector' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-office-telemetry",
            Label = "Disable Office Telemetry Scheduled Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Microsoft Office telemetry agent and dashboard tasks.",
            Tags = ["scheduledtask", "telemetry", "office", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Office\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'Telemetry' } | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Office\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'Telemetry' } | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Office\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'Telemetry' -and $_.State -eq 'Disabled' }).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-speech-model-update",
            Label = "Disable Speech Model Auto-Update Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the speech model download and update scheduled task.",
            Tags = ["scheduledtask", "speech", "network", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Speech\\' -TaskName 'SpeechModelDownloadTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Speech\\' -TaskName 'SpeechModelDownloadTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Speech\\' -TaskName 'SpeechModelDownloadTask' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-device-census",
            Label = "Disable Device Census Telemetry Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Device Census hardware inventory collection task.",
            Tags = ["scheduledtask", "telemetry", "privacy", "hardware"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -TaskName 'Device' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -TaskName 'Device' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -TaskName 'Device' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-handwriting-data",
            Label = "Disable Handwriting Data Sharing Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables handwriting recognition data collection and sharing scheduled tasks.",
            Tags = ["scheduledtask", "privacy", "handwriting", "telemetry"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TabletPC\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TabletPC\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TabletPC\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-cloud-experience",
            Label = "Disable Cloud Experience Host Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Cloud Experience Host tasks that handle Windows setup, OOBE, and Microsoft account prompts.",
            Tags = ["scheduledtask", "privacy", "cloud", "microsoft-account"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\CloudExperienceHost\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\CloudExperienceHost\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\CloudExperienceHost\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-diagnostic-data-controller",
            Label = "Disable Diagnostic Data Controller Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Windows diagnostic data upload controller tasks.",
            Tags = ["scheduledtask", "telemetry", "diagnostics", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\FeatureConfig\\' -TaskName 'ReconcileFeatures' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\OneSettings\\' -TaskName 'RefreshCache' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\FeatureConfig\\' -TaskName 'ReconcileFeatures' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\OneSettings\\' -TaskName 'RefreshCache' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\*' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-power-efficiency",
            Label = "Disable Power Efficiency Diagnostics Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the power efficiency diagnostic report task that runs periodically.",
            Tags = ["scheduledtask", "performance", "power", "diagnostics"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Power Efficiency Diagnostics\\' -TaskName 'AnalyzeSystem' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Power Efficiency Diagnostics\\' -TaskName 'AnalyzeSystem' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Power Efficiency Diagnostics\\' -TaskName 'AnalyzeSystem' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-idle-maintenance",
            Label = "Disable Idle Maintenance Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables automatic idle maintenance that runs during system idle periods.",
            Tags = ["scheduledtask", "performance", "maintenance", "idle"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TaskScheduler\\' -ErrorAction SilentlyContinue "
                        + "| Where-Object { $_.TaskName -match 'Idle|Maintenance' } "
                        + "| Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TaskScheduler\\' -ErrorAction SilentlyContinue "
                        + "| Where-Object { $_.TaskName -match 'Idle|Maintenance' } "
                        + "| Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TaskScheduler\\' -ErrorAction SilentlyContinue "
                        + "| Where-Object { $_.TaskName -match 'Idle|Maintenance' -and $_.State -eq 'Disabled' }).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-defrag-scheduled",
            Label = "Disable Scheduled Defragmentation",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the scheduled disk defragmentation task. Recommended for SSD-only systems where defrag is unnecessary.",
            Tags = ["scheduledtask", "performance", "defrag", "ssd", "disk"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-location-notification",
            Label = "Disable Location Notification Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the location notification background task.",
            Tags = ["scheduledtask", "privacy", "location", "notifications"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Location\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Location\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Location\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-windows-error-reporting",
            Label = "Disable Windows Error Reporting Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables WER queue and reporting scheduled tasks.",
            Tags = ["scheduledtask", "privacy", "error-reporting", "wer"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-family-safety",
            Label = "Disable Family Safety Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Microsoft Family Safety monitoring tasks.",
            Tags = ["scheduledtask", "privacy", "family-safety", "parental"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyMonitor*' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null;"
                        + "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyRefresh*' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyMonitor*' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null;"
                        + "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyRefresh*' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'FamilySafety' -and $_.State -eq 'Disabled' }).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-autochk-rebooter",
            Label = "Disable AutoChk Reboot Notification Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Autochk proxy scheduled task that notifies about pending chkdsk operations.",
            Tags = ["scheduledtask", "performance", "chkdsk", "notifications"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Autochk\\' -TaskName 'Proxy' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Autochk\\' -TaskName 'Proxy' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Autochk\\' -TaskName 'Proxy' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-license-validation",
            Label = "Disable License Validation Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Software Protection Platform license validation and rearm tasks.",
            Tags = ["scheduledtask", "privacy", "licensing", "activation"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\SoftwareProtectionPlatform\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\SoftwareProtectionPlatform\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\SoftwareProtectionPlatform\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-net-framework-ngen",
            Label = "Disable .NET Framework NGEN Tasks",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables background .NET native image generation tasks that consume CPU during idle periods.",
            Tags = ["scheduledtask", "performance", "dotnet", "ngen", "cpu"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\.NET Framework\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\.NET Framework\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\.NET Framework\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-compat-appraiser",
            Label = "Disable Compatibility Appraiser Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Microsoft Compatibility Appraiser task that gathers telemetry for upgrade compatibility.",
            Tags = ["scheduledtask", "privacy", "telemetry", "compatibility", "upgrade"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'Microsoft Compatibility Appraiser' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'Microsoft Compatibility Appraiser' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'Microsoft Compatibility Appraiser' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-rac-task",
            Label = "Disable Reliability Monitor (RAC) Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Reliability Monitor data-collection task that reports system stability errors.",
            Tags = ["scheduledtask", "privacy", "telemetry", "reliability", "rac"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RAC\\' -TaskName 'RacTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RAC\\' -TaskName 'RacTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RAC\\' -TaskName 'RacTask' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-program-compat-updater",
            Label = "Disable Program Compatibility Data Updater",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Application Experience ProgramDataUpdater task that uploads compatibility telemetry.",
            Tags = ["scheduledtask", "privacy", "telemetry", "compatibility", "appcompat"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'ProgramDataUpdater' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'ProgramDataUpdater' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'ProgramDataUpdater' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-wer-queue-reporting",
            Label = "Disable WER Queue Error Reporting Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Windows Error Reporting QueueReporting scheduled task that submits crash reports.",
            Tags = ["scheduledtask", "privacy", "telemetry", "wer", "crash"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -TaskName 'QueueReporting' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -TaskName 'QueueReporting' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -TaskName 'QueueReporting' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-device-info-collector",
            Label = "Disable Device Information Collection Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Device Information tasks that gather hardware inventory for Microsoft telemetry.",
            Tags = ["scheduledtask", "privacy", "telemetry", "hardware", "inventory"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-smart-screen-app-id",
            Label = "Disable SmartScreen App-ID Background Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the SmartScreen credential lookup background task under AppID.",
            Tags = ["scheduledtask", "smartscreen", "privacy", "security", "appid"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\AppID\\' -TaskName 'SmartScreenSpecific' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\AppID\\' -TaskName 'SmartScreenSpecific' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\AppID\\' -TaskName 'SmartScreenSpecific' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-mrt-telemetry",
            Label = "Disable MRT Telemetry Heartbeat Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the MRT_HB heartbeat task that sends usage telemetry for the Malicious Software Removal Tool.",
            Tags = ["scheduledtask", "mrt", "telemetry", "privacy", "malware"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RemovalTools\\' -TaskName 'MRT_HB' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RemovalTools\\' -TaskName 'MRT_HB' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RemovalTools\\' -TaskName 'MRT_HB' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-defender-cache-maintenance",
            Label = "Disable Defender Cache Maintenance Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Windows Defender background cache maintenance scheduled task to reduce idle CPU usage.",
            Tags = ["scheduledtask", "defender", "cache", "performance", "cpu"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Defender\\' -TaskName 'Windows Defender Cache Maintenance' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Defender\\' -TaskName 'Windows Defender Cache Maintenance' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Defender\\' -TaskName 'Windows Defender Cache Maintenance' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-usbceip",
            Label = "Disable USB CEIP Telemetry Task",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the USB Customer Experience Improvement Program background data collection task.",
            Tags = ["scheduledtask", "usb", "ceip", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -TaskName 'UsbCeip' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -TaskName 'UsbCeip' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -TaskName 'UsbCeip' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}


internal static class PolicyUpdate
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CbsUpdatePolicy.Data,
            .. _UpdateAutoRestartPolicy.Data,
            .. _WindowsPauseUpdatesPolicy.Data,
            .. _WindowsUpdateAdvanced.Data,
            .. _WindowsUpdateDriverPolicy.Data,
            .. _WindowsUpdateNotificationPolicy.Data,
            .. _WindowsUpdatePolicy.Data,
            .. _WindowsUpdateScanPolicy.Data,
            .. _WindowsUpdateUsoPolicy.Data,
            // ── merged from: WindowsUpdate.cs ───────────────────────────────────────
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
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 0),
                ],
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
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates", 0),
                ],
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
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel", 2),
                ],
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
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings",
                        "DownloadMode",
                        0
                    ),
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
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess", 0),
                ],
            },
        ];

    // ── CbsUpdatePolicy ──
    private static class _CbsUpdatePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CBS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cbsupd-enable-auto-repair",
                Label = "Enable Automatic Component-Based Servicing Repair",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Component-Based Servicing (CBS) is the Windows component store infrastructure that manages OS component installation, updates, and repairs through the DISM subsystem. Enabling automatic CBS repair ensures that corrupted or missing system components are automatically detected and repaired from the Windows component store without manual intervention. CBS corruption can prevent Windows Update from installing updates and security patches creating security vulnerabilities from missed patching cycles. Automatic repair through CBS uses the component manifest store to verify component integrity and restore damaged components to their correct state. Organizations should enable automatic CBS repair to ensure that system component corruption does not cause persistent patching failures or security gaps. CBS repair events are logged in the CBS.log file which should be reviewed during system health checks to identify recurring repair needs.",
                Tags = ["cbs", "component-repair", "system-integrity", "update", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutomaticRepair", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutomaticRepair")],
                DetectOps = [RegOp.CheckDword(Key, "AutomaticRepair", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enforce-component-hash-verification",
                Label = "Enforce Cryptographic Hash Verification for CBS Components",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS component hash verification validates the cryptographic hash of each system component against the component manifest preventing installation of tampered or corrupted components. Enforcing hash verification for CBS operations ensures that only genuine Microsoft-signed components are installed as part of servicing operations. Component hash bypass attacks attempt to install modified system files by manipulating the CBS manifest or hash database to accept attacker-controlled components. CBS hash verification provides a layer of protection against supply chain attacks that attempt to replace legitimate system files with backdoored versions. Organizations should ensure that CBS integrity checking is enabled and that the component store hash database has not been modified through monitoring. CBS hash verification failures generate events in the CBS.log that should be treated as high-severity alerts indicating potential system tampering.",
                Tags = ["cbs", "hash-verification", "integrity", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceHashVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceHashVerification")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceHashVerification", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-restrict-cbs-offline-servicing",
                Label = "Restrict CBS Offline Servicing to Authorized Administrators",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "CBS offline servicing allows modification of Windows component store contents from offline boot environments which is a powerful capability that can be used to bypass OS-level security controls. Restricting CBS offline servicing to authorized administrators prevents unauthorized use of offline tools to modify system components outside the normal OS boot environment. BitLocker full-disk encryption is the primary defense against offline servicing attacks as it prevents booting from external media to access the encrypted drive. Organizations running Secure Boot with TPM-based integrity measurement provide additional protection against offline servicing attacks by detecting changes to the boot environment. CBS offline servicing is a legitimate maintenance capability used for repair scenarios but should be restricted through physical security and encryption rather than software policy alone. Organizations should include CBS offline servicing in their threat model and ensure physical security controls prevent unauthorized access to servers.",
                Tags = ["cbs", "offline-servicing", "admin-restriction", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictOfflineServicing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictOfflineServicing")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictOfflineServicing", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-cbs-cleanup-scheduled",
                Label = "Enable Scheduled Cleanup of Superseded CBS Components",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Windows component store retains superseded component versions after updates to support rollback capability but accumulates significant disk space over time. Enabling scheduled CBS cleanup removes superseded components after a defined retention period freeing disk space while retaining recent versions for rollback. System drives running close to capacity due to component store accumulation can cause update failures when insufficient space exists for patch installation. The DISM cleanup task removes components that can no longer be uninstalled based on the uninstall window policy reducing disk usage by 10-20% on long-running systems. Organizations should balance component store cleanup with rollback requirements as aggressive cleanup prevents rolling back recent updates if problems are discovered. WSFC clusters during CBS cleanup and fail-safe mechanisms prevent critical system failures due to premature component removal.",
                Tags = ["cbs", "cleanup", "disk-space", "maintenance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ScheduledCleanup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScheduledCleanup")],
                DetectOps = [RegOp.CheckDword(Key, "ScheduledCleanup", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enforce-manifest-signing",
                Label = "Enforce Digital Signature on CBS Component Manifests",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS component manifests describe the contents and attributes of system components and are signed by Microsoft to ensure their integrity and prevent modification. Enforcing manifest signature verification ensures that modified manifests that attempt to introduce malicious components or disable security features are rejected. Component manifest signing is part of the Windows Trusted Installer infrastructure that protects system file integrity against unauthorized modification. Manifests that have been tampered with to override hash values or add unauthorized components will be rejected when signature enforcement is active. Manifest signature enforcement is a defense-in-depth measure complementing Windows Resource Protection (WRP) and other component store integrity mechanisms. Organizations should treat CBS manifest signature verification failures as critical security events indicating potential kernel-level or bootkit-level compromise.",
                Tags = ["cbs", "manifest-signing", "code-signing", "integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceManifestSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceManifestSigning")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceManifestSigning", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-cbs-verbose-logging",
                Label = "Enable Verbose CBS Logging for Update Failure Diagnostics",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "CBS verbose logging captures detailed information about servicing operations including component installations, updates, and failures in the CBS.log file for troubleshooting. Enabling verbose CBS logging provides the detailed diagnostic data needed to identify root causes of Windows Update failures that may indicate security patching gaps. CBS.log is typically several hundred megabytes to gigabytes in size with verbose logging and should be captured and analyzed as part of update compliance monitoring. Update failures identified through CBS verbose logging should be cross-referenced with security vulnerability databases to prioritize remediation of security-relevant failures. Organizations with update compliance requirements should monitor CBS logs for persistent failures that indicate systems are not receiving security patches. Verbose CBS logging helps distinguish between installation failures caused by disk space, compatibility, corruption, or other factors to inform targeted remediation.",
                Tags = ["cbs", "verbose-logging", "diagnostics", "update-compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "VerboseLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "VerboseLogging")],
                DetectOps = [RegOp.CheckDword(Key, "VerboseLogging", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-set-cbs-store-health-check-interval",
                Label = "Set Scheduled Interval for CBS Component Store Health Verification",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "CBS component store health checks verify the integrity of the Windows component store by comparing installed component hashes against the reference values in the component manifest. Setting regular health check intervals ensures that component store corruption is detected promptly before it leads to update failures or security vulnerabilities. Component store corruption can occur due to disk errors, unexpected shutdowns, or malware modification of system files. Regular health verification similar to running DISM /CheckHealth provides ongoing assurance that the system components match their expected values. Health check interval policies complement automatic repair by detecting corruption early before it causes operational problems. Organizations should define health check intervals based on their risk posture with more frequent checks for high-security systems and critical infrastructure.",
                Tags = ["cbs", "health-check", "component-integrity", "maintenance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HealthCheckInterval", 7)],
                RemoveOps = [RegOp.DeleteValue(Key, "HealthCheckInterval")],
                DetectOps = [RegOp.CheckDword(Key, "HealthCheckInterval", 7)],
            },
            new TweakDef
            {
                Id = "cbsupd-block-unsigned-packages",
                Label = "Block Installation of Unsigned or Untrusted CBS Packages",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS package signature verification ensures that only packages signed by trusted certificate authorities including Microsoft and hardware vendors can be installed through the CBS servicing infrastructure. Blocking unsigned CBS packages prevents installation of tampered or third-party packages that could introduce vulnerabilities or backdoors into the system component store. Unsigned packages submitted to CBS represent a significant threat vector for supply chain attacks where unauthorized components are installed as system components. CBS package signature enforcement should apply to both online and offline servicing operations to prevent bypass through offline tools. Organizations running Windows Server should audit the custom packages installed through CBS to identify any unsigned or questionable packages in the component store. CBS signature enforcement is complementary to Windows code signing policies and should be aligned with the organization's overall application trust model.",
                Tags = ["cbs", "unsigned-packages", "code-signing", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUnsignedPackages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnsignedPackages")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnsignedPackages", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-restrict-cbs-to-trusted-sources",
                Label = "Restrict CBS Package Sources to Microsoft Update and WSUS Only",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "CBS package source restriction limits where the CBS servicing infrastructure can obtain component packages to Microsoft Update or organizational WSUS servers. Restricting CBS to trusted sources prevents the use of arbitrary package sources that could deliver malicious components masked as system updates. Third-party package sources for CBS are rarely needed in enterprise environments where updates are managed through WSUS or Configuration Manager. Source restriction for CBS complements Windows Update source restrictions to create a consistent update trust chain from Microsoft to the endpoint. Organizations should configure both Windows Update and CBS source policies together to ensure coherent update supply chain protection. Audit CB package installation events to detect any packages sourced from unexpected origins that may indicate a source restriction bypass.",
                Tags = ["cbs", "trusted-sources", "update-chain", "wsus", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToTrustedSources", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToTrustedSources")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToTrustedSources", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-servicing-stack-updates-priority",
                Label = "Enable Priority Installation of Servicing Stack Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Servicing Stack Updates (SSUs) update the foundational CBS infrastructure itself and must be installed before cumulative updates that depend on the updated servicing stack. Enabling priority installation of SSUs ensures that the servicing stack is always current before applying other updates preventing installation failures from an outdated stack. Outdated servicing stacks are a common cause of Windows Update failure where cumulative updates cannot be installed because they require SSU capabilities not yet present. SSU prioritization is implemented in Windows 10 1903 and later through the Unified Update Platform that automatically handles SSU installation order. Organizations running older Windows versions should prioritize SSU installation in their WSUS or Configuration Manager patch deployment groups. Servicing stack currency is a prerequisite for comprehensive security patching and should be verified during update compliance audits.",
                Tags = ["cbs", "servicing-stack", "update-priority", "patching", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PrioritizeServicingStackUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PrioritizeServicingStackUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "PrioritizeServicingStackUpdates", 1)],
            },
        ];
    }

    // ── UpdateAutoRestartPolicy ──
    private static class _UpdateAutoRestartPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-deadline-7days",
                    Label = "WU Auto-Restart: Set Engaged Restart Deadline to 7 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets EngagedRestartDeadline=7 in WU policy. After a quality update is downloaded, Windows enters 'engaged restart' mode where users are repeatedly notified. "
                        + "This value sets the absolute deadline after which Windows will force a restart regardless of user activity. "
                        + "7 days is a balance that gives users a full work week to schedule the restart while ensuring machines don't stay un-patched indefinitely.",
                    Tags = ["windows-update", "restart", "deadline", "policy", "engaged"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Forces restart after 7 days; ensures machines are patched while giving users a workweek to choose their own restart time.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartDeadline", 7)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartDeadline")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartDeadline", 7)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-snooze-3days",
                    Label = "WU Auto-Restart: Set Engaged Restart Snooze Interval to 3 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets EngagedRestartSnoozeSchedule=3 in WU policy. Controls how frequently Windows re-displays the engaged restart notification after a user dismisses it. "
                        + "Value of 3 means the reminder returns every 3 days, ensuring users don't forget a pending restart while avoiding daily interruptions that lead to notification fatigue and dismissal without action.",
                    Tags = ["windows-update", "restart", "snooze", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "3-day snooze interval for restart reminders; balances user awareness with notification fatigue.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartSnoozeSchedule", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartSnoozeSchedule")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartSnoozeSchedule", 3)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-transition-2days",
                    Label = "WU Auto-Restart: Set Engaged Restart Transition Schedule to 2 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets EngagedRestartTransitionSchedule=2 in WU policy. Controls how many days after an update becomes ready-to-install that Windows transitions from passive notifications to the more prominent 'engaged restart' mode. "
                        + "Setting this to 2 days means the first two days show soft notifications, after which the full engaged restart UI (with deadline counter) takes over.",
                    Tags = ["windows-update", "restart", "transition", "policy", "engaged"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Transitions to engaged restart mode after 2 days; earlier transition increases restart compliance rate.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartTransitionSchedule", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartTransitionSchedule")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartTransitionSchedule", 2)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-quality-update-deadline-3days",
                    Label = "WU Auto-Restart: Set Quality Update Install Deadline to 3 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineForQualityUpdates=3 in WU policy. Establishes a hard deadline of 3 days from when a quality (security + non-security) update is offered before Windows must restart to install it. "
                        + "For security teams managing patch compliance under CIS or NIST 800-53 patch SLAs, a 3-day restart deadline for quality updates ensures critical CVE patches are active within the compliance window.",
                    Tags = ["windows-update", "deadline", "quality", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "3-day hard restart deadline for quality updates; supports NIST 800-53 and CIS patch compliance SLAs.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForQualityUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-feature-update-deadline-14days",
                    Label = "WU Auto-Restart: Set Feature Update Install Deadline to 14 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineForFeatureUpdates=14 in WU policy. Establishes a 14-day hard deadline from when a feature update is offered before Windows must restart to complete installation. "
                        + "Feature updates are far more disruptive than quality updates (longer restart time, possible app compatibility breaks), so a longer 14-day window gives users and IT departments time to validate and prepare.",
                    Tags = ["windows-update", "deadline", "feature", "upgrade", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "14-day deadline for feature updates; longer window accommodates compatibility validation before forced restart.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForFeatureUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-deadline-grace-period-2days",
                    Label = "WU Auto-Restart: Set Post-Deadline Grace Period to 2 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineGracePeriod=2 in WU policy. After the restart deadline passes, this grace period gives users an additional 2 days before the machine will restart outside of active hours. "
                        + "The grace period prevents the deadline enforcement from causing a disruptive forced restart mid-workday as soon as the deadline hits. The machine will restart during the next scheduled non-active hours window within the grace period.",
                    Tags = ["windows-update", "deadline", "grace", "restart", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "2-day grace period post-deadline; restart deferred to next active-hours window reducing in-day disruption.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineGracePeriod", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineGracePeriod")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineGracePeriod", 2)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-disable-no-auto-reboot-after-deadline",
                    Label = "WU Auto-Restart: Allow Auto-Reboot After Deadline Expires",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineNoAutoReboot=0 in WU policy. Ensures that once the deadline and grace period pass, Windows WILL automatically restart to apply the update. "
                        + "Value=0 means no moratorium on auto-reboot after the deadline. This overrides any 'NoAutoRebootWithLoggedOnUsers' policy for machines that have exceeded their deadline, ensuring patching is never blocked indefinitely by a persistent logged-on session.",
                    Tags = ["windows-update", "restart", "deadline", "enforcement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Post-deadline auto-reboot enabled; overrides logged-on user protection once deadline expires for compliance.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineNoAutoReboot")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-restart-warning-4hours",
                    Label = "WU Auto-Restart: Set Pre-Restart Warning to 4 Hours",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduleRestartWarning=4 in WU policy. When Windows schedules an automatic restart, this setting controls how many hours in advance users receive a prominent restart warning notification. "
                        + "A 4-hour advance warning gives users time to save work, close applications, and plan the restart, significantly reducing data loss from unexpected restarts.",
                    Tags = ["windows-update", "restart", "warning", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "4-hour advance restart warning; gives users time to save work and plan restart timing.",
                    ApplyOps = [RegOp.SetDword(Key, "ScheduleRestartWarning", 4)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScheduleRestartWarning")],
                    DetectOps = [RegOp.CheckDword(Key, "ScheduleRestartWarning", 4)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-enable-auto-restart-required-notification",
                    Label = "WU Auto-Restart: Enable Mandatory Restart Required Notification",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartRequiredNotificationDismissal=1 in WU policy. Configures Windows to show a non-dismissable restart required notification when a patch deadline is imminent. "
                        + "Without this, users can indefinitely dismiss restart prompts. With value=1, close-to-deadline notifications must be acknowledged with a concrete restart time selection rather than a simple dismiss.",
                    Tags = ["windows-update", "restart", "notification", "mandatory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Non-dismissable restart notification near deadline; forces users to choose restart time, increasing compliance.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartRequiredNotificationDismissal")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-enable-auto-restart-notification-config",
                    Label = "WU Auto-Restart: Enable Automatic Restart Notification Banner",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartNotificationConfig=1 in WU policy. Enables the automatic restart notification configuration, which shows a system tray and action centre banner when a pending restart is required. "
                        + "Without this setting the notification may be suppressed in locked-down enterprise notification policies. Enabling it ensures users are always informed of pending update restarts even in notification-restricted environments.",
                    Tags = ["windows-update", "restart", "notification", "banner", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables restart notification banner in action centre; ensures user visibility of pending restarts in locked environments.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartNotificationConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartNotificationConfig")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartNotificationConfig", 1)],
                },
            ];
    }

    // ── WindowsPauseUpdatesPolicy ──
    private static class _WindowsPauseUpdatesPolicy
    {
        private const string PauseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
        private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pauseupd-defer-feature-30days",
                Label = "Windows Update Pause: Defer Feature Updates 30 Days",
                Category = "Windows Update",
                Description =
                    "Defers Windows feature updates by 30 days beyond their general availability date. "
                    + "Deferral gives IT administrators time to test compatibility before feature updates reach production endpoints. "
                    + "30 days is the minimum recommended deferral for enterprise deployments and allows Microsoft to identify critical regressions first. "
                    + "Removing this policy re-enables immediate feature update availability.",
                Tags = ["windows-update", "defer", "feature-update", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "DeferFeatureUpdatesPeriodInDays", 30)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "DeferFeatureUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(PauseKey, "DeferFeatureUpdatesPeriodInDays", 30)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Delays feature updates by 30 days; reduces exposure to day-zero feature regressions.",
            },
            new TweakDef
            {
                Id = "pauseupd-defer-quality-7days",
                Label = "Windows Update Pause: Defer Quality Updates 7 Days",
                Category = "Windows Update",
                Description =
                    "Defers Windows quality (security patch) updates by 7 days, allowing time for emergency patch retraction. "
                    + "Quality updates occasionally introduce regressions; a 7-day deferral window reduces blast radius from faulty patches. "
                    + "7 days is short enough to maintain adequate security posture while providing a testing buffer. "
                    + "Removing this policy makes quality updates available immediately upon release.",
                Tags = ["windows-update", "defer", "quality-update", "patch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "DeferQualityUpdatesPeriodInDays", 7)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "DeferQualityUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(PauseKey, "DeferQualityUpdatesPeriodInDays", 7)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Delays security patches by 7 days; provides testing buffer without excessive security lag.",
            },
            new TweakDef
            {
                Id = "pauseupd-disable-auto-install-on-shutdown",
                Label = "Windows Update Pause: Disable Auto-Install Updates on Shutdown",
                Category = "Windows Update",
                Description =
                    "Prevents Windows Update from automatically installing updates when the user initiates a shutdown. "
                    + "Auto-install-on-shutdown can extend shutdown times and cause unexpected restarts, especially on laptops before meetings. "
                    + "Updates are controlled through scheduled windows instead, giving IT full control over the timing. "
                    + "Removing this policy re-enables automatic installation during shutdown sequences.",
                Tags = ["windows-update", "shutdown", "auto-install", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "NoAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "NoAutoUpdate")],
                DetectOps = [RegOp.CheckDword(AuKey, "NoAutoUpdate", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents updates installing on shutdown; avoids unexpected extended shutdown times.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-active-hours-start",
                Label = "Windows Update Pause: Set Active Hours Start (8 AM)",
                Category = "Windows Update",
                Description =
                    "Configures the Windows Update active hours start time to 8 AM, preventing reboots for updates during business hours. "
                    + "Active hours protect users from unexpected reboots during the configured working hours window. "
                    + "Setting an explicit start ensures policy is enforced rather than relying on user configuration. "
                    + "Removing this policy reverts to Windows default or user-configured active hours.",
                Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursStart", 8)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursStart")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursStart", 8)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks active hours start to 8 AM; prevents update reboots interrupting morning workflows.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-active-hours-end",
                Label = "Windows Update Pause: Set Active Hours End (6 PM)",
                Category = "Windows Update",
                Description =
                    "Configures the Windows Update active hours end time to 6 PM (18:00), ensuring reboots cannot occur during standard business hours. "
                    + "With start fixed at 8 AM and end at 6 PM, the full working day is protected from forced reboots. "
                    + "Updates can install after 6 PM via the scheduled maintenance window. "
                    + "Removing this policy reverts to Windows default or user-configured active hours end.",
                Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursEnd", 18)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursEnd")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursEnd", 18)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks active hours end to 6 PM; complete 8 AM–6 PM protection from forced reboots.",
            },
            new TweakDef
            {
                Id = "pauseupd-block-driver-updates",
                Label = "Windows Update Pause: Block Driver Updates via Windows Update",
                Category = "Windows Update",
                Description =
                    "Prevents Windows Update from automatically downloading and installing driver updates. "
                    + "Automatic driver updates can replace validated enterprise drivers with incompatible versions, causing hardware failures or BSODs. "
                    + "Driver management should be handled by IT through validated packages rather than Windows Update. "
                    + "Removing this policy re-enables automatic driver updates through Windows Update.",
                Tags = ["windows-update", "driver", "exclusion", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ExcludeWUDriversInQualityUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ExcludeWUDriversInQualityUpdate")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ExcludeWUDriversInQualityUpdate", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks automatic driver updates via WU; prevents validated drivers being silently replaced.",
            },
            new TweakDef
            {
                Id = "pauseupd-disable-upgrade-notifications",
                Label = "Windows Update Pause: Disable Upgrade Notification Toasts",
                Category = "Windows Update",
                Description =
                    "Suppresses the Windows Update toast notifications that prompt users to restart for pending updates. "
                    + "In a managed environment, restart timing is controlled by IT policy — user-visible prompts are redundant and disruptive. "
                    + "Suppressing notifications prevents users from inadvertently triggering reboots outside the maintenance window. "
                    + "Removing this policy re-enables Windows Update restart notification toasts.",
                Tags = ["windows-update", "notifications", "restart", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(AuKey, "SetDisableUXWUAccess", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides WU restart prompts from users; IT maintains full control of update timing.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-update-detection-frequency",
                Label = "Windows Update Pause: Set Update Detection Frequency (22 Hours)",
                Category = "Windows Update",
                Description =
                    "Sets the Windows Update service to check for updates every 22 hours instead of the default automatic random interval. "
                    + "A predictable 22-hour check interval prevents multiple machines on the same network from surging the update server simultaneously. "
                    + "Combined with an WSUS/SCCM deployment, this ensures consistent, manageable update bandwidth. "
                    + "Removing this policy reverts to Windows' random detection frequency.",
                Tags = ["windows-update", "detection", "frequency", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled")],
                DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequencyEnabled", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Predictable 22-hour WU check interval; prevents bandwidth surge on shared networks.",
            },
            new TweakDef
            {
                Id = "pauseupd-allow-mu-updates",
                Label = "Windows Update Pause: Allow Microsoft Update for Other Products",
                Category = "Windows Update",
                Description =
                    "Configures Windows Update to also deliver updates for other Microsoft products (Office, .NET, Visual C++) alongside OS patches. "
                    + "Receiving all Microsoft product updates through a single channel simplifies patch management and reduces the attack surface. "
                    + "This is equivalent to enabling 'Give me updates for other Microsoft products' in Windows Update settings. "
                    + "Removing this policy reverts to OS-only updates via Windows Update.",
                Tags = ["windows-update", "microsoft-update", "office", "patch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "AllowMUUpdateService", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "AllowMUUpdateService")],
                DetectOps = [RegOp.CheckDword(AuKey, "AllowMUUpdateService", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables Microsoft Update for all products; consolidates patching into a single channel.",
            },
            new TweakDef
            {
                Id = "pauseupd-enforce-restart-deadline",
                Label = "Windows Update Pause: Enforce 72-Hour Restart Deadline",
                Category = "Windows Update",
                Description =
                    "Sets a 72-hour mandatory restart deadline after Windows Update installs updates requiring a reboot. "
                    + "Without a deadline, users can indefinitely postpone required restarts, leaving the system vulnerable to active exploits. "
                    + "72 hours provides reasonable flexibility for users to save work while ensuring security patches are applied promptly. "
                    + "Removing this policy removes the forced restart deadline.",
                Tags = ["windows-update", "restart", "deadline", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "SetAutoRestartDeadline", 72)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "SetAutoRestartDeadline")],
                DetectOps = [RegOp.CheckDword(PauseKey, "SetAutoRestartDeadline", 72)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Forces restart within 72 hours of patch install; prevents indefinite deferral of security updates.",
            },
        ];
    }

    // ── WindowsUpdateAdvanced ──
    private static class _WindowsUpdateAdvanced
    {
        private const string WuPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        private const string WuAu = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        private const string DeliveryOpt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wuadv-exclude-driver-updates",
                Label = "Exclude Driver Updates from Windows Update",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["windows update", "drivers", "policy", "quality update"],
                Description =
                    "Prevents Windows Update from automatically installing driver updates "
                    + "alongside quality/security updates. ExcludeWUDriversInQualityUpdate=1. "
                    + "Useful when you manage drivers manually via Device Manager or vendor tools.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "ExcludeWUDriversInQualityUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "ExcludeWUDriversInQualityUpdate")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "ExcludeWUDriversInQualityUpdate", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-defer-feature-updates-30-days",
                Label = "Defer Feature (Major) Updates by 30 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "feature update", "deferral", "stability"],
                Description =
                    "Delays the installation of major Windows feature updates (annual releases) "
                    + "by 30 days. DeferFeatureUpdatesPeriodInDays=30. Gives time for early bugs "
                    + "in new Windows versions to be patched before your machine upgrades.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "DeferFeatureUpdates", 1), RegOp.SetDword(WuPolicy, "DeferFeatureUpdatesPeriodInDays", 30)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "DeferFeatureUpdates"), RegOp.DeleteValue(WuPolicy, "DeferFeatureUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "DeferFeatureUpdatesPeriodInDays", 30)],
            },
            new TweakDef
            {
                Id = "wuadv-defer-quality-updates-7-days",
                Label = "Defer Quality (Security) Updates by 7 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["windows update", "quality update", "security", "deferral"],
                Description =
                    "Delays monthly quality (security) updates by 7 days. "
                    + "DeferQualityUpdatesPeriodInDays=7. Allows time for faulty patches to be "
                    + "identified and pulled before your machine installs them, "
                    + "while keeping you close to the security patch baseline.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "DeferQualityUpdates", 1), RegOp.SetDword(WuPolicy, "DeferQualityUpdatesPeriodInDays", 7)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "DeferQualityUpdates"), RegOp.DeleteValue(WuPolicy, "DeferQualityUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "DeferQualityUpdatesPeriodInDays", 7)],
            },
            new TweakDef
            {
                Id = "wuadv-block-update-settings-access",
                Label = "Block Standard Users from Accessing Windows Update Settings",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["windows update", "settings", "access control", "admin"],
                Description =
                    "Prevents non-administrator users from accessing Windows Update settings. "
                    + "SetDisableUXWUAccess=1. Standard users cannot scan for, pause, or configure "
                    + "updates. Only administrators can manage the update schedule.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "SetDisableUXWUAccess", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-disable-update-reboot-notification",
                Label = "Suppress Forced Reboot Notifications After Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "reboot", "notification", "restart"],
                Description =
                    "Prevents Windows Update from showing aggressive restart countdown notifications "
                    + "after installing updates. SetAutoRestartNotificationConfig=1 (suppress) / "
                    + "NoAutoRebootWithLoggedOnUsers=1. Users restart at their own pace.",
                ApplyOps = [RegOp.SetDword(WuAu, "NoAutoRebootWithLoggedOnUsers", 1)],
                RemoveOps = [RegOp.DeleteValue(WuAu, "NoAutoRebootWithLoggedOnUsers")],
                DetectOps = [RegOp.CheckDword(WuAu, "NoAutoRebootWithLoggedOnUsers", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-disable-delivery-optimization",
                Label = "Disable Delivery Optimization (P2P Update Sharing)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "delivery optimization", "p2p", "bandwidth"],
                Description =
                    "Disables Windows Delivery Optimization — the P2P update sharing feature that "
                    + "uploads update packages to other devices on the LAN or internet. "
                    + "DODownloadMode=0 (disabled). Eliminates upload bandwidth usage and privacy "
                    + "concerns about sharing data with unknown peers.",
                ApplyOps = [RegOp.SetDword(DeliveryOpt, "DODownloadMode", 0)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOpt, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DeliveryOpt, "DODownloadMode", 0)],
            },
            new TweakDef
            {
                Id = "wuadv-lan-only-delivery-optimization",
                Label = "Restrict Delivery Optimization to LAN Only",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["windows update", "delivery optimization", "lan", "p2p", "bandwidth"],
                Description =
                    "Restricts Delivery Optimization to only share update data with devices on "
                    + "the local LAN — not with external internet peers. DODownloadMode=1 (LAN "
                    + "only). Allows faster local updates while preventing internet upload.",
                ApplyOps = [RegOp.SetDword(DeliveryOpt, "DODownloadMode", 1)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOpt, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DeliveryOpt, "DODownloadMode", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-require-update-signature",
                Label = "Require Code-Signed Updates from WSUS",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "wsus", "signing", "security"],
                Description =
                    "Requires that all updates from a WSUS server are signed by a trusted publisher "
                    + "in the local machine certificate store. UsePolicyBasedQosMarkings=1 is the "
                    + "underlying policy; AcceptTrustedPublisherCerts=1 enables the WSUS signing check.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "AcceptTrustedPublisherCerts")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-allow-mu-updates-with-wu",
                Label = "Enable Microsoft Update (Office + Products) via Windows Update",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "microsoft update", "office", "products"],
                Description =
                    "Enables Microsoft Update service via the Windows Update policy — allows Office, "
                    + "Visual Studio, and other Microsoft products to receive updates through "
                    + "Windows Update instead of requiring separate update channels. "
                    + "EnableFeaturedSoftware=1.",
                ApplyOps = [RegOp.SetDword(WuAu, "EnableFeaturedSoftware", 1)],
                RemoveOps = [RegOp.DeleteValue(WuAu, "EnableFeaturedSoftware")],
                DetectOps = [RegOp.CheckDword(WuAu, "EnableFeaturedSoftware", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-set-active-hours-start",
                Label = "Set Windows Update Active Hours (8am–8pm)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "active hours", "restart", "schedule"],
                Description =
                    "Sets Windows Update active hours to 8am–8pm (hours 8–20). Windows will not "
                    + "automatically restart to apply updates during these hours. "
                    + "ActiveHoursStart=8, ActiveHoursEnd=20. Prevents disruptive mid-day reboots.",
                ApplyOps =
                [
                    RegOp.SetDword(WuPolicy, "SetActiveHours", 1),
                    RegOp.SetDword(WuPolicy, "ActiveHoursStart", 8),
                    RegOp.SetDword(WuPolicy, "ActiveHoursEnd", 20),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(WuPolicy, "SetActiveHours"),
                    RegOp.DeleteValue(WuPolicy, "ActiveHoursStart"),
                    RegOp.DeleteValue(WuPolicy, "ActiveHoursEnd"),
                ],
                DetectOps = [RegOp.CheckDword(WuPolicy, "ActiveHoursStart", 8), RegOp.CheckDword(WuPolicy, "ActiveHoursEnd", 20)],
            },
        ];
    }

    // ── WindowsUpdateDriverPolicy ──
    private static class _WindowsUpdateDriverPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";
        private const string SignKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Driver Signing";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wudrv-deny-unidentified-device-installation",
                    Label = "WU Driver: Block Installation of Unidentified Device Drivers",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyUnidentifiedDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from installing drivers for hardware devices that are not in the Windows Driver Store and do not have a matching entry in Windows Update. "
                        + "Unidentified devices are a common attack vector — malicious USB devices can present as unknown hardware that auto-installs a malicious driver. This policy requires all devices to have a recognized driver before they can function.",
                    Tags = ["driver", "device", "security", "usb", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks unidentified device driver installs; prevents USB hardware-based driver injection attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyUnidentifiedDeviceInstallation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyUnidentifiedDeviceInstallation")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyUnidentifiedDeviceInstallation", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-deny-removable-device-driver-install",
                    Label = "WU Driver: Block Automatic Driver Installation for Removable Devices",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyRemovableDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from automatically installing drivers for any removable device. "
                        + "Removable devices (USB storage, USB hubs, card readers, portable audio devices) are frequently connected in enterprise environments. Without this policy, each new removable device triggers an automatic driver installation from WU, bypassing IT-managed driver sets and potentially installing unsigned or vulnerable drivers.",
                    Tags = ["driver", "removable", "usb", "device", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks auto-install of removable device drivers via WU; requires IT-managed driver pre-staging for new devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyRemovableDeviceInstallation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyRemovableDeviceInstallation")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyRemovableDeviceInstallation", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enforce-driver-signing-block-unsigned",
                    Label = "WU Driver: Block Installation of Unsigned Device Drivers",
                    Category = "Windows Update",
                    Description =
                        "Sets BehaviorOnFailedVerify=2 in Driver Signing policy. Configures Windows to silently block the installation of any device driver that fails digital signature verification. "
                        + "Value 2 = Block (value 1 = Warn, value 0 = Ignore). Blocking unsigned drivers prevents rootkits and malicious kernel-mode code from loading under the guise of a hardware driver. This is a critical defence-in-depth control alongside Secure Boot and HVCI.",
                    Tags = ["driver", "signing", "security", "kernel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Silently blocks unsigned drivers; prevents rootkits and kernel-level malware from installing via driver packages.",
                    ApplyOps = [RegOp.SetDword(SignKey, "BehaviorOnFailedVerify", 2)],
                    RemoveOps = [RegOp.DeleteValue(SignKey, "BehaviorOnFailedVerify")],
                    DetectOps = [RegOp.CheckDword(SignKey, "BehaviorOnFailedVerify", 2)],
                },
                new TweakDef
                {
                    Id = "wudrv-prevent-device-class-installations",
                    Label = "WU Driver: Enable Device Class Installation Restriction Policy",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyDeviceClasses=1 in DeviceInstall\\Restrictions policy. Activates the device class restriction feature that, when combined with a list of blocked device class GUIDs, prevents installation of entire categories of devices. "
                        + "This policy enables the enforcement of device class blocklists (e.g., blocking all Bluetooth adapters, all wireless adapters, or all imaging devices) across the enterprise without per-device ID management.",
                    Tags = ["driver", "device-class", "restriction", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Activates device class restriction framework; prerequisite for GUID-based device category blocklists.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyDeviceClasses", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyDeviceClasses")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyDeviceClasses", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enable-device-id-restriction-policy",
                    Label = "WU Driver: Enable Device ID-Based Installation Restriction",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyDeviceIDs=1 in DeviceInstall\\Restrictions policy. Activates the device ID restriction feature. When enabled, Windows checks all device hardware IDs against a configured deny list. "
                        + "Device ID restrictions are more granular than class restrictions and allow blocking specific problematic hardware models (e.g., a specific USB key brand with a known firmware vulnerability) while permitting similar hardware from other vendors.",
                    Tags = ["driver", "device-id", "restriction", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Activates device ID restriction; enables HWID-based device blocklists for targeted hardware exclusions.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyDeviceIDs", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyDeviceIDs")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyDeviceIDs", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-log-driver-install-restriction-events",
                    Label = "WU Driver: Enable Event Logging for Blocked Driver Installations",
                    Category = "Windows Update",
                    Description =
                        "Sets WritePolicy=1 in DeviceInstall\\Restrictions policy. Enables Windows to write an event log entry whenever a device installation is blocked by Device Installation Policy. "
                        + "Without this, blocked installations fail silently, making it impossible to audit what hardware was attempted and blocked. With logging enabled, security teams can monitor for repeated installation attempts which may indicate hardware-based persistence attempts.",
                    Tags = ["driver", "logging", "audit", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Logs blocked driver installations to event log; enables audit trail for hardware-based attack detection.",
                    ApplyOps = [RegOp.SetDword(Key, "WritePolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WritePolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "WritePolicy", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-disable-windows-error-reporting-driver",
                    Label = "WU Driver: Disable Driver Crash Data Upload to Microsoft",
                    Category = "Windows Update",
                    Description =
                        "Sets DisableDriverLookup=1 in DeviceInstall\\Restrictions policy. Prevents Windows from looking up driver information and uploading crash data to the Microsoft Windows Error Reporting service when a device driver causes an error. "
                        + "In regulated environments, data sovereignty requirements may prohibit telemetry of driver crash details (device type, hardware ID, crash context) from being transmitted to Microsoft's cloud infrastructure.",
                    Tags = ["driver", "telemetry", "privacy", "wer", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks driver crash data upload to Microsoft; supports data sovereignty requirements for regulated industries.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDriverLookup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverLookup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDriverLookup", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-prevent-non-admin-driver-install",
                    Label = "WU Driver: Restrict Driver Installation to Administrators Only",
                    Category = "Windows Update",
                    Description =
                        "Sets PreventInstallationOfDevicesNotDescribedByOtherPolicySettings=1 in DeviceInstall\\Restrictions policy. Sets a default-deny posture for device installation: only devices explicitly permitted by an allowlist policy are installed. All others are blocked. "
                        + "This inverts the default Windows behaviour (allow-by-default) into a deny-by-default stance that requires active IT involvement to introduce any new device type into the environment.",
                    Tags = ["driver", "device", "allowlist", "default-deny", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Default-deny for new device types; requires IT-managed allowlist for any new hardware class to function.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enable-device-metadata-retrieval-block",
                    Label = "WU Driver: Block Device Metadata Retrieval from Windows Update",
                    Category = "Windows Update",
                    Description =
                        "Sets PreventDeviceMetadataFromNetwork=1 in DeviceInstall policy. Prevents Windows from searching the Windows Update network service for device metadata (device icons, model pages, UWP companion apps). "
                        + "Device metadata retrieval can prompt automatic download of companion apps without explicit user action. In locked-down environments, all device metadata should be pre-staged via WSUS rather than retrieved on-demand from Microsoft servers.",
                    Tags = ["driver", "metadata", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks network-sourced device metadata; prevents unsolicited companion app downloads on device connection.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork",
                            1
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork",
                            1
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "wudrv-allow-admin-override-device-restriction",
                    Label = "WU Driver: Allow Administrators to Override Device Installation Policy",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowAdminInstall=1 in DeviceInstall\\Restrictions policy. When device installation restrictions are in effect (including deny-by-default), this allows users in the local Administrators group to install any device regardless of policy restrictions. "
                        + "This maintains an escape hatch for IT staff to provision new hardware on managed endpoints without requiring a Group Policy update cycle, while standard users remain restricted.",
                    Tags = ["driver", "admin", "override", "device", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Allows admins to bypass device installation restrictions; provides IT escape hatch without weakening user-level controls.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAdminInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAdminInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAdminInstall", 1)],
                },
            ];
    }

    // ── WindowsUpdateNotificationPolicy ──
    private static class _WindowsUpdateNotificationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wunotif-set-update-notification-level-standard",
                    Label = "WU Notification: Set Update Notification Level to Standard",
                    Category = "Windows Update",
                    Description =
                        "Sets UpdateNotificationLevel=1 in WU policy. Configures the Windows Update notification level presented to users. "
                        + "Level 1 = Standard Notifications (users see action centre notifications and system tray alerts for pending updates). Level 2 = Disable all restart notifications. "
                        + "Setting level 1 ensures users are informed without overly aggressive interruptions, and is the baseline for notification management before other more specific controls are applied.",
                    Tags = ["windows-update", "notification", "level", "action-centre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sets base notification level; ensures users are informed of pending updates without restart interruptions.",
                    ApplyOps = [RegOp.SetDword(Key, "UpdateNotificationLevel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UpdateNotificationLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "UpdateNotificationLevel", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-suppress-restart-notification-when-busy",
                    Label = "WU Notification: Suppress Auto-Restart Notifications During Active Use",
                    Category = "Windows Update",
                    Description =
                        "Sets SuppressRestartNotification=1 in WU policy. Instructs Windows to suppress automatic restart notifications while the user is actively using the computer (mouse/keyboard activity detected). "
                        + "This prevents the restart prompt from appearing mid-presentation or mid-call, reducing user frustration while still allowing notifications when the device is idle.",
                    Tags = ["windows-update", "notification", "restart", "suppress", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses restart notifications during device activity; notifications appear only when user is idle.",
                    ApplyOps = [RegOp.SetDword(Key, "SuppressRestartNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SuppressRestartNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "SuppressRestartNotification", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-update-availability-popup",
                    Label = "WU Notification: Disable Update Availability Pop-Up Toast",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartNotificationExclusion=1 in WU policy. Disables the 'restart to update' toast notification pop-up that appears in the bottom-right corner of the screen. "
                        + "In enterprise SCCM/Intune-managed environments, the deployment tool provides its own notification and deadline management. The built-in WU toast in these environments creates duplicate, confusing messages that contradict the managed deployment window.",
                    Tags = ["windows-update", "notification", "toast", "popup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses WU toast pop-ups; eliminates duplicate notifications in SCCM/Intune managed environments.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartNotificationExclusion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartNotificationExclusion")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartNotificationExclusion", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-suppress-update-reboot-during-fullscreen",
                    Label = "WU Notification: Block Update Restart During Full-Screen Applications",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartDeadline=1 in WU policy combined with full-screen detection. Prevents Windows from showing the restart notification or initiating an automatic restart while a full-screen application is active. "
                        + "This is critical for kiosk, digital signage, and presentation machines where a mid-presentation WU restart notification would disrupt a live business event or customer-facing display.",
                    Tags = ["windows-update", "notification", "fullscreen", "kiosk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses WU restarts during full-screen apps; prevents disruption of presentations and digital signage.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartDeadline", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartDeadline")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartDeadline", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-upgrade-feature-notifications",
                    Label = "WU Notification: Disable Feature Upgrade Recommendation Notifications",
                    Category = "Windows Update",
                    Description =
                        "Sets DisableWindowsUpdateUI=0 in WU policy combined with DisableWUfBSafeguards=0. Suppresses the persistent Windows 11/Windows 10 upgrade promotion banners and notifications that appear when a newer major version is available. "
                        + "In enterprise environments managed to a specific OS release, these upgrade solicitations confuse users and generate IT support calls from users requesting to upgrade outside the approved schedule.",
                    Tags = ["windows-update", "notification", "upgrade", "feature", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses OS version upgrade promotions; prevents users from self-initiating unapproved major upgrades.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWUfBSafeguards", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWUfBSafeguards")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWUfBSafeguards", 0)],
                },
                new TweakDef
                {
                    Id = "wunotif-set-reboot-warning-timeout-15min",
                    Label = "WU Notification: Set Reboot Warning Timeout to 15 Minutes",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduleImminentRestartWarning=15 in WU policy. Sets the duration of the imminent-restart countdown dialog to 15 minutes. "
                        + "When Windows determines a restart is imminent (e.g., deadline approaching), this countdown gives users exactly 15 minutes to save their work before the restart proceeds. This is shorter than the ScheduleRestartWarning (advance warning hours) and is the 'last chance' save reminder.",
                    Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "15-minute last-chance countdown before restart; reduces data loss from unwarned forced restarts.",
                    ApplyOps = [RegOp.SetDword(Key, "ScheduleImminentRestartWarning", 15)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScheduleImminentRestartWarning")],
                    DetectOps = [RegOp.CheckDword(Key, "ScheduleImminentRestartWarning", 15)],
                },
                new TweakDef
                {
                    Id = "wunotif-enable-windows-update-log-events",
                    Label = "WU Notification: Enable Verbose Windows Update Event Logging",
                    Category = "Windows Update",
                    Description =
                        "Sets EnableDetailedLogging=1 in WU policy. Enables detailed verbose logging of Windows Update events to the Windows Event Log under the WindowsUpdateClient/Operational channel. "
                        + "By default, Windows Update logs minimal information. Detailed logs capture download start/stop, error codes, and deployment decisions, enabling IT to troubleshoot why updates fail, succeed late, or trigger unexpected restarts on specific machines.",
                    Tags = ["windows-update", "logging", "audit", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables verbose WU logging to event log; critical for diagnosing update failures and compliance audit trails.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDetailedLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDetailedLogging", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-block-user-changing-update-settings",
                    Label = "WU Notification: Block Users from Modifying Update Settings",
                    Category = "Windows Update",
                    Description =
                        "Sets SetUpdateNotificationLevel=2 in WU policy. Removes the Windows Update section from the Windows Settings app for standard users, so they cannot view or modify the pending update state, notification preferences, or restart schedules. "
                        + "For high-security and kiosk deployments, the WU settings page should be invisible to users to prevent them from deferring updates or changing restart windows outside of IT-approved schedules.",
                    Tags = ["windows-update", "settings", "user", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hides WU settings from non-admin users; prevents unauthorised deferrals or notification preference changes.",
                    ApplyOps = [RegOp.SetDword(Key, "SetUpdateNotificationLevel", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetUpdateNotificationLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "SetUpdateNotificationLevel", 2)],
                },
                new TweakDef
                {
                    Id = "wunotif-enable-update-health-tools-reporting",
                    Label = "WU Notification: Enable Update Health Tools Status Reporting",
                    Category = "Windows Update",
                    Description =
                        "Sets EnableUpdateHealthTools=1 in WU policy. Activates the Update Compliance Health Tools which report patch status, restart compliance, and update health metrics to Azure Monitor, Microsoft Endpoint Manager, or custom OMS workspaces. "
                        + "Without health tools enabled, IT dashboards show no patch status for affected machines, making it impossible to identify non-compliant devices in the estate.",
                    Tags = ["windows-update", "health", "reporting", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables patch status reporting to endpoint management platforms; provides patch compliance visibility.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableUpdateHealthTools", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableUpdateHealthTools")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableUpdateHealthTools", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-outdated-browser-notifications",
                    Label = "WU Notification: Disable Outdated Browser/App Update Notifications from WU",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowNonMicrosoftSignedUpdate=0 in WU policy. Prevents Windows Update from delivering and notifying about updates from non-Microsoft third-party publishers via the Microsoft Update service. "
                        + "Third-party update notifications through Windows Update are not needed when dedicated application management tools (SCCM, Intune, Chocolatey) are already used for non-OS software, reducing noise and preventing IT-unmanaged software updates.",
                    Tags = ["windows-update", "notification", "third-party", "apps", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Blocks third-party software update notifications via WU; channel reserved for OS updates only in managed environments.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowNonMicrosoftSignedUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
                },
            ];
    }

    // ── WindowsUpdatePolicy ──
    private static class _WindowsUpdatePolicy
    {
        private const string WuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wupol-disable-wu-access",
                    Label = "Disable Direct Windows Update Access",
                    Category = "Windows Update",
                    Description = "Blocks direct access to Windows Update servers; devices must use an internal WSUS or managed update source.",
                    Tags = ["windows-update", "wsus", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Prevents unmanaged updates; requires a WSUS or Microsoft Endpoint Manager infrastructure to deliver updates.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DisableWindowsUpdateAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DisableWindowsUpdateAccess")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DisableWindowsUpdateAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-block-internet-wu-locations",
                    Label = "Block Direct Connection to Windows Update Internet Locations",
                    Category = "Windows Update",
                    Description =
                        "Forces all update traffic through an internal catalog; prevents the client from contacting Microsoft update servers directly.",
                    Tags = ["windows-update", "internet", "policy", "wsus"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Devices only receive updates through the configured internal source; requires WUServer to be set.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DoNotConnectToWindowsUpdateInternetLocations")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-exclude-driver-updates",
                    Label = "Exclude Hardware Drivers from Windows Update",
                    Category = "Windows Update",
                    Description = "Prevents Windows Update from automatically delivering hardware driver updates through quality update channels.",
                    Tags = ["windows-update", "drivers", "policy", "stability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Driver updates must be installed manually or via WSUS driver category; prevents unstable driver push.",
                    ApplyOps = [RegOp.SetDword(WuKey, "ExcludeWUDriversInQualityUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "ExcludeWUDriversInQualityUpdate")],
                    DetectOps = [RegOp.CheckDword(WuKey, "ExcludeWUDriversInQualityUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-disable-os-upgrade",
                    Label = "Disable OS Upgrade Offers via Windows Update",
                    Category = "Windows Update",
                    Description = "Prevents Windows Update from offering or installing major operating system version upgrades.",
                    Tags = ["windows-update", "upgrade", "feature-update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Stops feature upgrades (e.g., Windows 10 → 11); quality and security patches are unaffected.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DisableOSUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DisableOSUpgrade")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DisableOSUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-quality-updates",
                    Label = "Defer Quality Updates",
                    Category = "Windows Update",
                    Description = "Enables deferral of quality (non-security) updates, delaying their installation after Microsoft release.",
                    Tags = ["windows-update", "quality-update", "deferral", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quality non-security updates are delayed; security patches are included in quality updates and also deferred.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferQualityUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferQualityUpdates")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferQualityUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-quality-updates-14d",
                    Label = "Set Quality Update Deferral to 14 Days",
                    Category = "Windows Update",
                    Description = "Defers quality updates by 14 days after Microsoft releases them, providing a burn-in window.",
                    Tags = ["windows-update", "quality-update", "deferral", "days", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "14-day window to observe crash reports on early adopters before applying to managed devices.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferQualityUpdatesPeriodInDays", 14)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferQualityUpdatesPeriodInDays")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferQualityUpdatesPeriodInDays", 14)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-feature-updates",
                    Label = "Defer Feature Updates",
                    Category = "Windows Update",
                    Description = "Enables deferral of Windows feature updates, preventing the installation of new OS versions immediately.",
                    Tags = ["windows-update", "feature-update", "deferral", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Feature updates are held back until the deferral period expires; quality updates can be deferred separately.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferFeatureUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferFeatureUpdates")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferFeatureUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-feature-updates-180d",
                    Label = "Set Feature Update Deferral to 180 Days",
                    Category = "Windows Update",
                    Description = "Defers Windows feature updates by 180 days, keeping the device on the current version for 6 months.",
                    Tags = ["windows-update", "feature-update", "deferral", "days", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Six-month stability window before upgrading OS; balance between security and compatibility testing.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferFeatureUpdatesPeriodInDays", 180)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferFeatureUpdatesPeriodInDays")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferFeatureUpdatesPeriodInDays", 180)],
                },
                new TweakDef
                {
                    Id = "wupol-block-preview-builds",
                    Label = "Block Windows Insider / Preview Builds",
                    Category = "Windows Update",
                    Description = "Prevents users from opting in to Windows Insider or preview builds on managed devices.",
                    Tags = ["windows-update", "insider", "preview", "policy", "stability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot enrol in Windows Insider program; production build only on this device.",
                    ApplyOps = [RegOp.SetDword(WuKey, "ManagePreviewBuilds", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "ManagePreviewBuilds")],
                    DetectOps = [RegOp.CheckDword(WuKey, "ManagePreviewBuilds", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-set-semi-annual-channel",
                    Label = "Set Update Branch to Semi-Annual Channel",
                    Category = "Windows Update",
                    Description = "Configures Windows Update to use the Semi-Annual Channel for feature update readiness (General Availability).",
                    Tags = ["windows-update", "branch", "semi-annual", "channel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Value 16 = Semi-Annual Channel; device receives GA feature updates rather than Insider or preview rings.",
                    ApplyOps = [RegOp.SetDword(WuKey, "BranchReadinessLevel", 16)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "BranchReadinessLevel")],
                    DetectOps = [RegOp.CheckDword(WuKey, "BranchReadinessLevel", 16)],
                },
            ];
    }

    // ── WindowsUpdateScanPolicy ──
    private static class _WindowsUpdateScanPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
        private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuscan-enable-wsus-server-mode",
                    Label = "WU Scan: Route Update Scanning Through WSUS Server",
                    Category = "Windows Update",
                    Description =
                        "Sets UseWUServer=1 in WU AU policy. Configures the Windows Update client to scan against the WSUS server configured in WUServer, rather than the public Windows Update service. "
                        + "This is the primary switch that activates WSUS-based update management. Without this flag set to 1, WUServer and WUStatusServer URL values are present in the registry but ignored by the WU client, which continues to scan against Microsoft's cloud endpoint.",
                    Tags = ["windows-update", "wsus", "server", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Activates WSUS-sourced scanning; all updates sourced from and approved via internal WSUS server.",
                    ApplyOps = [RegOp.SetDword(AuKey, "UseWUServer", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "UseWUServer")],
                    DetectOps = [RegOp.CheckDword(AuKey, "UseWUServer", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-wsus-scan-frequency-22hours",
                    Label = "WU Scan: Set WSUS Detection Frequency to 22 Hours",
                    Category = "Windows Update",
                    Description =
                        "Sets DetectionFrequency=22 and DetectionFrequencyEnabled=1 in WU AU policy. Configures the WU client to scan for updates every 22 hours instead of the default random interval (17-22 hours). "
                        + "A fixed 22-hour interval ensures predictable scan timing for environments where WSUS server load must be managed. Scan frequency should be set to complement WSUS synchronisation schedule so clients scan after the server has synced from Microsoft.",
                    Tags = ["windows-update", "wsus", "scan", "frequency", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "22-hour fixed scan interval; predictable WSUS load distribution vs. default random timing.",
                    ApplyOps = [RegOp.SetDword(AuKey, "DetectionFrequency", 22), RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "DetectionFrequency"), RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled")],
                    DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequency", 22)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-automatic-update-download-and-schedule",
                    Label = "WU Scan: Set Auto-Update Mode to Download and Schedule Install",
                    Category = "Windows Update",
                    Description =
                        "Sets AUOptions=4 in WU AU policy. Configures the auto-update behaviour to automatically download approved updates and schedule their installation for a configured maintenance window. "
                        + "AUOptions values: 2=Notify only, 3=Auto download + notify for install, 4=Auto download + schedule install, 5=Allow local admin to configure. Value 4 is standard for enterprise WSUS where deployments are scheduled to minimize business disruption.",
                    Tags = ["windows-update", "auto-update", "download", "schedule", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Auto-download with scheduled install; standard WSUS mode for planned maintenance window deployments.",
                    ApplyOps = [RegOp.SetDword(AuKey, "AUOptions", 4)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "AUOptions")],
                    DetectOps = [RegOp.CheckDword(AuKey, "AUOptions", 4)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-scheduled-install-day-0-every-day",
                    Label = "WU Scan: Set Scheduled Install Day to Every Day",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduledInstallDay=0 in WU AU policy. Configures Windows Update to install scheduled updates every day (rather than a specific day of the week). "
                        + "Day=0 means daily; Day=1-7 means a specific day (1=Sunday through 7=Saturday). Combined with ScheduledInstallTime, daily installation ensures patches are applied within 24 hours of their scheduled maintenance window rather than waiting up to a week.",
                    Tags = ["windows-update", "schedule", "install", "daily", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Daily scheduled install cadence; updates applied within 24h of availability rather than weekly batch.",
                    ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallDay", 0)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallDay")],
                    DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallDay", 0)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-scheduled-install-time-2am",
                    Label = "WU Scan: Set Scheduled Install Time to 2:00 AM",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduledInstallTime=2 in WU AU policy. Schedules automatic update installations to occur at 2:00 AM local time. "
                        + "2 AM is the classic maintenance window: after business hours, before early-morning workers arrive, outside of backup windows (typically 1–2 AM), and during a period when most machines are idle but still powered on. "
                        + "This time balances update deployment speed with business disruption minimisation.",
                    Tags = ["windows-update", "schedule", "install", "maintenance-window", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "2 AM scheduled installs; classic after-hours maintenance window that avoids business hours disruption.",
                    ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallTime", 2)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallTime")],
                    DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallTime", 2)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-intranet-update-service-stats",
                    Label = "WU Scan: Enable Intranet Update Statistics Reporting",
                    Category = "Windows Update",
                    Description =
                        "Sets UseWUServer=1 and IntranetServerInternetOptions=3 in WU AU policy. Configures the WU client to send update scan statistics (detection results, download progress, installation outcomes) to the WSUS status server rather than Microsoft. "
                        + "This populates the WSUS server's reporting database, enabling IT administrators to view an accurate picture of update compliance across the enterprise from the WSUS console.",
                    Tags = ["windows-update", "wsus", "reporting", "statistics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Routes update scan stats to WSUS; populates compliance reports in WSUS console.",
                    ApplyOps = [RegOp.SetDword(AuKey, "IntranetServerInternetOptions", 3)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "IntranetServerInternetOptions")],
                    DetectOps = [RegOp.CheckDword(AuKey, "IntranetServerInternetOptions", 3)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-automatic-minor-update-install",
                    Label = "WU Scan: Enable Automatic Installation of Minor Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets AutoInstallMinorUpdates=1 in WU AU policy. Allows Windows Update to automatically install minor (maintenance release) updates without user notification or interaction. "
                        + "Minor updates are typically service definition updates, component metadata refreshes, and low-risk patches that carry essentially no regression risk. Auto-installing these keeps the system at the latest minor version baseline without requiring a scheduled maintenance window for trivial updates.",
                    Tags = ["windows-update", "minor-updates", "auto-install", "baseline", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto-installs minor updates silently; keeps system at full baseline without scheduled window for low-risk patches.",
                    ApplyOps = [RegOp.SetDword(AuKey, "AutoInstallMinorUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "AutoInstallMinorUpdates")],
                    DetectOps = [RegOp.CheckDword(AuKey, "AutoInstallMinorUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-allow-mu-service-alongside-wu",
                    Label = "WU Scan: Scan Microsoft Update Service Alongside Windows Update",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowMUUpdateService=1 in WU AU policy. Opts the machine into the Microsoft Update (MU) service in addition to the base Windows Update service. "
                        + "Microsoft Update delivers updates for Office, Visual Studio, .NET, SQL Server, and other Microsoft products alongside OS updates. Without this setting, only Windows OS updates are delivered by WU, while Office and other products update through their own channels, which may not honour the configured maintenance window.",
                    Tags = ["windows-update", "microsoft-update", "office", "products", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enrolls in Microsoft Update alongside WU; Office and other MS products update in the same maintenance window.",
                    ApplyOps = [RegOp.SetDword(AuKey, "AllowMUUpdateService", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "AllowMUUpdateService")],
                    DetectOps = [RegOp.CheckDword(AuKey, "AllowMUUpdateService", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-reboot-launch-timeout-5min",
                    Label = "WU Scan: Set Post-Install Reboot Launch Timeout to 5 Minutes",
                    Category = "Windows Update",
                    Description =
                        "Sets RebootLaunchTimeout=5 and RebootLaunchTimeoutEnabled=1 in WU policy. After updates are installed during a scheduled maintenance window and a restart is required, Windows waits this many minutes before initiating the restart automatically. "
                        + "5 minutes gives any background processes time to complete gracefully while keeping the restart within the maintenance window. Without a timeout, the restart may be postponed indefinitely if a user was actively logged in during the overnight window.",
                    Tags = ["windows-update", "restart", "timeout", "maintenance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "5-minute post-install restart timeout; keeps restart within maintenance window while allowing graceful process shutdown.",
                    ApplyOps = [RegOp.SetDword(Key, "RebootLaunchTimeout", 5), RegOp.SetDword(Key, "RebootLaunchTimeoutEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RebootLaunchTimeout"), RegOp.DeleteValue(Key, "RebootLaunchTimeoutEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RebootLaunchTimeoutEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-reboot-warning-timeout-30min",
                    Label = "WU Scan: Set Pre-Restart Warning Timeout to 30 Minutes",
                    Category = "Windows Update",
                    Description =
                        "Sets RebootWarningTimeout=30 and RebootWarningTimeoutEnabled=1 in WU policy. Configures Windows to display a countdown restart warning 30 minutes before the scheduled restart. "
                        + "30 minutes provides a comfortable window for users to save work and close applications before the restart. This setting complements ScheduleRestartWarning (hours-in-advance general notice) — the 30-minute warning is the final specific countdown before imminent restart.",
                    Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "30-minute final restart countdown; gives users time to save before scheduled maintenance restart.",
                    ApplyOps = [RegOp.SetDword(Key, "RebootWarningTimeout", 30), RegOp.SetDword(Key, "RebootWarningTimeoutEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RebootWarningTimeout"), RegOp.DeleteValue(Key, "RebootWarningTimeoutEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RebootWarningTimeoutEnabled", 1)],
                },
            ];
    }

    // ── WindowsUpdateUsoPolicy ──
    private static class _WindowsUpdateUsoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuuso-block-wu-downloads-metered-network",
                    Label = "WU USO: Block Windows Update Downloads on Metered Networks",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowAutoWindowsUpdateDownloadOverMeteredNetwork=0 in WU policy. Prevents Windows Update from automatically downloading update packages when the active network connection is marked as metered. "
                        + "On mobile devices and machines on cellular or satellite connections, unrestricted WU downloads can exhaust data allowances or incur substantial overage charges. This policy applies to both background and foreground download scenarios.",
                    Tags = ["windows-update", "metered", "network", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks WU auto-downloads on metered connections; prevents data-plan exhaustion on mobile/satellite links.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-temporary-enterprise-feature-drops",
                    Label = "WU USO: Block In-Period Temporary Enterprise Feature Drops",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowTemporaryEnterpriseFeatureControl=0 in WU policy. Disables the delivery of optional 'temporary enterprise feature' updates — incremental functionality enhancements that Microsoft ships between major version releases. "
                        + "These in-period feature drops are not security updates and can change application behaviour mid-support-lifecycle. Blocking them keeps the OS in a stable, enterprise-validated state between planned upgrade windows.",
                    Tags = ["windows-update", "features", "enterprise", "stability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks temporary enterprise feature drops; keeps OS behaviour predictable between scheduled upgrade events.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowTemporaryEnterpriseFeatureControl")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-prevent-user-pausing-updates",
                    Label = "WU USO: Prevent Users from Pausing Windows Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets SetDisablePauseUXAccess=1 in WU policy (AU subkey). Removes the 'Pause Updates' option from the Windows Update settings UI. "
                        + "Without this policy, standard users can pause updates for up to 5 weeks, leaving machines unpatched and out of compliance. This is a key control in corporate environments operating under patch management SLAs where user-initiated update deferrals are not permitted.",
                    Tags = ["windows-update", "pause", "user", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removes pause updates control from user UI; ensures patch compliance SLAs are not bypassed by users.",
                    ApplyOps = [RegOp.SetDword(Key, "SetDisablePauseUXAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetDisablePauseUXAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "SetDisablePauseUXAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-disable-dual-scan-on-wsus",
                    Label = "WU USO: Disable Dual-Scan When WSUS Is Configured",
                    Category = "Windows Update",
                    Description =
                        "Sets DisableDualScan=1 in WU policy. When a WSUS server (WUServer) is configured, Windows 10/11 will by default simultaneously scan both the WSUS server and the public Windows Update/Microsoft Update cloud. "
                        + "This 'dual scan' allows unapproved updates to arrive from the cloud even when WSUS approval workflows are in place. Disabling dual scan ensures all updates flow exclusively through WSUS, preserving IT update approval control.",
                    Tags = ["windows-update", "wsus", "dual-scan", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks cloud WU source when WSUS is configured; enforces WSUS approval pipeline with no cloud bypass.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDualScan", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDualScan")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDualScan", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-internet-wu-when-wsus-active",
                    Label = "WU USO: Block Internet Windows Update Access When WSUS Active",
                    Category = "Windows Update",
                    Description =
                        "Sets DoNotConnectToWindowsUpdateInternetLocations=1 in WU policy. When active, prevents the WU client from connecting to the public internet endpoints for update detection, metadata, or downloads. "
                        + "This is required in air-gapped or WSUS-only environments where all internet traffic is blocked by firewall policy. Without this setting, WU may attempt internet connections that trigger firewall alerts or fail silently and produce misleading update status.",
                    Tags = ["windows-update", "wsus", "internet", "air-gapped", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks all public WU internet connections; required for WSUS-only or air-gapped deployment scenarios.",
                    ApplyOps = [RegOp.SetDword(Key, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DoNotConnectToWindowsUpdateInternetLocations")],
                    DetectOps = [RegOp.CheckDword(Key, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-recommended-updates-auto-install",
                    Label = "WU USO: Block Automatic Installation of Recommended Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets IncludeRecommendedUpdates=0 in WU policy. Prevents Windows Update from automatically installing 'recommended' updates which include non-security improvements, application updates, and optional Windows features. "
                        + "In enterprise environments, recommended updates should be reviewed and approved through a patch management process rather than automatically deployed, as they can change application behaviour without a security justification.",
                    Tags = ["windows-update", "recommended", "auto-install", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks auto-install of recommended updates; only critical and security updates deploy automatically.",
                    ApplyOps = [RegOp.SetDword(Key, "IncludeRecommendedUpdates", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IncludeRecommendedUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "IncludeRecommendedUpdates", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-allow-only-trusted-publisher-certs",
                    Label = "WU USO: Accept Only Updates from Trusted Publisher Certificates",
                    Category = "Windows Update",
                    Description =
                        "Sets AcceptTrustedPublisherCerts=1 in WU policy. Configures the WU client to only accept and install updates that are signed by certificates in the machine's Trusted Publishers certificate store. "
                        + "This prevents installation of updates signed by untrusted authority chains, which is relevant in WSUS deployments where custom update packages may be published by third parties or internal teams.",
                    Tags = ["windows-update", "trusted-publisher", "certificate", "signing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only installs updates signed by trusted publisher certificates; guards against malicious WSUS packages.",
                    ApplyOps = [RegOp.SetDword(Key, "AcceptTrustedPublisherCerts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AcceptTrustedPublisherCerts")],
                    DetectOps = [RegOp.CheckDword(Key, "AcceptTrustedPublisherCerts", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-optional-content-updates",
                    Label = "WU USO: Block Optional Windows Content Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowOptionalContent=0 in WU policy. Prevents Windows Update from offering and installing optional content packages — these include font packs, additional language components, accessibility features, and recreational apps. "
                        + "Optional content updates consume storage and bandwidth and are not security-relevant. Blocking them reduces WU noise and storage footprint on tightly managed enterprise machines.",
                    Tags = ["windows-update", "optional", "content", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks optional Windows content updates; reduces WU bandwidth and storage usage on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowOptionalContent", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowOptionalContent")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowOptionalContent", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-featured-software-via-wu",
                    Label = "WU USO: Block Automatic Installation of Featured Software",
                    Category = "Windows Update",
                    Description =
                        "Sets EnableFeaturedSoftware=0 in WU policy. Stops Windows Update from offering and automatically installing 'featured software' — typically free Microsoft utilities, game trials, and promotional apps. "
                        + "Without this setting, WU silently installs marketing-tied software packages that were never requested by the user or IT administrator, increasing the installed application footprint and creating an unexpected change management event.",
                    Tags = ["windows-update", "featured", "software", "bloat", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks OEM/Microsoft featured software installs via WU; prevents unsolicited app additions on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableFeaturedSoftware", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableFeaturedSoftware")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableFeaturedSoftware", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-policy-driven-other-update-source",
                    Label = "WU USO: Force Policy-Driven Update Source for Other Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets SetPolicyDrivenUpdateSourceForOtherUpdates=1 in WU policy. Ensures that non-feature, non-quality updates (such as drivers from the 'Other' category in WU) are sourced exclusively through the configured policy-driven update source (WSUS/SCCM). "
                        + "Without this setting, updates in the 'Other' category may still be retrieved directly from Microsoft Update regardless of the WSUS or DeliveryOptimization configuration.",
                    Tags = ["windows-update", "wsus", "policy-driven", "other-updates", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Routes 'Other' category updates through policy-driven source; closes WSUS bypass for non-standard update types.",
                    ApplyOps = [RegOp.SetDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
                },
            ];
    }
}
