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
            Id = "crash-disable-crash-dump",
            Label = "Disable Crash Dump Entirely",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Do not write any crash dump files. Saves disk but loses BSOD data. Default: automatic dump.",
            Tags = ["crash", "dump", "disable", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
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
            Id = "crash-disable-wer-policy",
            Label = "Disable WER (Policy)",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Machine-wide policy to disable Windows Error Reporting. Default: not set.",
            Tags = ["wer", "error", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "crash-disable-wer-user",
            Label = "Disable WER (User Level)",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable error reporting for current user only. Default: enabled.",
            Tags = ["wer", "error", "user", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Windows Error Reporting"],
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
            Id = "crash-disable-app-compat-engine",
            Label = "Disable App Compatibility Engine",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable the application compatibility engine. Slight performance gain. Default: enabled.",
            Tags = ["compatibility", "app-compat", "engine", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
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
            Id = "crash-disable-feedback-notifications",
            Label = "Disable Feedback Request Notifications",
            Category = "Maintenance",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows feedback request notification prompts. Sets NumberOfSIUFInPeriod to 0 to suppress all feedback popups. Default: periodic. Recommended: disabled.",
            Tags = ["crash", "feedback", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
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
            Id = "crash-enable-full-memory-dump",
            Label = "Enable Full Memory Dumps",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets CrashDumpEnabled to 1 (Complete memory dump) for full debugging information on BSOD. Requires sufficient page file. Default: 7 (Automatic). Recommended: 1 for debugging.",
            Tags = ["crash", "dump", "memory", "debugging", "bsod"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 1)],
        },
        new TweakDef
        {
            Id = "crash-disable-wer-queue",
            Label = "Disable WER Queue Reporting",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows Error Reporting queue-based report collection. Stops WER from queuing reports for later submission. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["crash", "wer", "queue", "reporting", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "crash-set-kernel-dump-only",
            Label = "Set Crash Dump to Kernel Only",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets CrashDumpEnabled to 2 (Kernel memory dump). Captures only kernel-mode memory, saving disk space while retaining key info. Default: 7 (Automatic). Recommended: 2 for production.",
            Tags = ["crash", "dump", "kernel", "production", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 2)],
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
            Id = "crash-disable-app-telemetry",
            Label = "Disable Application Telemetry",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Application Telemetry diagnostic pipeline. Prevents app compatibility data collection. Default: enabled.",
            Tags = ["crash", "telemetry", "app-compat", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
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
        new TweakDef
        {
            Id = "crash-disable-program-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Program Compatibility Assistant that prompts after app crashes. Default: enabled.",
            Tags = ["crash", "compatibility", "assistant", "pca"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "crash-suppress-wer-ui",
            Label = "Suppress WER Error Dialog",
            Category = "Maintenance",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses the Windows Error Reporting UI dialog when applications crash. Errors are logged silently without user interruption. Default: shown.",
            Tags = ["crash", "wer", "dialog", "suppress"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-print-spooler",
            Label = "Disable Print Spooler Service",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Print Spooler service entirely (Start=4). Reduces attack surface on machines that do not use printers. Default: Automatic (2). Recommended: Disabled (4) if no printing needed.",
            Tags = ["printing", "spooler", "service", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "Start", 4)],
        },
        new TweakDef
        {
            Id = "printing-restrict-driver-install",
            Label = "Restrict Printer Driver Installation",
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-web-printing",
            Label = "Disable Internet Printing (IPP)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Internet Printing Protocol (IPP) support. Prevents printing via HTTP/HTTPS URLs. Default: enabled.",
            Tags = ["printing", "ipp", "internet", "web", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-spooler-remote-access",
            Label = "Disable Print Spooler Remote Access",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Print Spooler from accepting remote connections. Mitigates PrintNightmare-class vulnerabilities. Default: allowed.",
            Tags = ["printing", "spooler", "remote", "security"],
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
            Id = "printing-copy-files-policy",
            Label = "Disable Point and Print Copy Files",
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-publishing",
            Label = "Disable Printer Publishing to AD",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents printers from being published to Active Directory. Reduces domain traffic from printer advertisements. Default: enabled.",
            Tags = ["printing", "publishing", "active-directory", "domain"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-spooler-log",
            Label = "Disable Print Spooler Event Logging",
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-auto-default-printer",
            Label = "Disable Automatic Default Printer",
            Category = "Printing",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Stops Windows from automatically changing the default printer to the most recently used one. Default: Windows manages default printer.",
            Tags = ["printing", "default-printer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", "LegacyDefaultPrinterMode", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-driver-updates-via-wu",
            Label = "Block Printer Driver Downloads via Windows Update",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Update from automatically downloading printer drivers. Avoids unwanted driver changes. Default: auto-download enabled.",
            Tags = ["printing", "driver", "windows-update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-notifications",
            Label = "Disable Print Job Notifications",
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-point-and-print",
            Label = "Restrict Point and Print Driver Installation",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Restricts Point and Print to approved servers only. Mitigates PrintNightmare-class vulnerabilities. Default: unrestricted.",
            Tags = ["printing", "security", "point-and-print", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint", "Restricted", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-print-workflow-svc",
            Label = "Disable Print Workflow Service",
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Id = "printing-disable-http-printing",
            Label = "Disable HTTP Printer Sharing",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables sharing printers over HTTP. Prevents the Print Spooler from accepting HTTP-based print jobs. Default: HTTP sharing permitted.",
            Tags = ["printing", "http", "network", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "printing-disable-web-pnp",
            Label = "Disable Web-Based Printer Plug and Play",
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
            Category = "Printing",
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
