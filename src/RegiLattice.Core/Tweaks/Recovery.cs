namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Recovery
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "recovery-disable-system-restore",
            Label = "Disable System Restore",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables System Restore on all drives via Group Policy. Saves disk space but removes rollback capability. Default: enabled.",
            Tags = ["recovery", "system-restore", "disable", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 1)],
        },
        new TweakDef
        {
            Id = "recovery-limit-restore-disk-usage",
            Label = "Limit System Restore Disk Usage to 5%",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the disk space used by System Restore shadow copies to 5% of the drive. Saves space while preserving restore capability. Default: 10-15%.",
            Tags = ["recovery", "system-restore", "disk-space", "limit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "DiskPercent", 5)],
        },
        new TweakDef
        {
            Id = "recovery-increase-restore-frequency",
            Label = "System Restore: Daily Checkpoints",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets System Restore to create automatic restore points every 24 hours (86400 seconds). Default: 24 hours but may be skipped under load.",
            Tags = ["recovery", "system-restore", "checkpoint", "daily"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval", 86400),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 86400),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPGlobalInterval", 86400)],
        },
        new TweakDef
        {
            Id = "recovery-disable-auto-repair-prompt",
            Label = "Disable Automatic Repair at Boot",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the automatic startup repair prompt after consecutive boot failures. Prevents boot loops on servers. Default: enabled.",
            Tags = ["recovery", "auto-repair", "boot", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "BootExecuteAutoRecovery", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-winre-partition",
            Label = "Disable Windows Recovery Environment",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Recovery Environment (WinRE). Saves ~500 MB disk space but removes recovery boot option. Default: enabled.",
            Tags = ["recovery", "winre", "disable", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "WinREEnabled", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-auto-reboot-bsod",
            Label = "Disable Auto-Reboot on BSOD",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents automatic reboot after a blue screen of death. Allows reading the error code. Default: auto-reboot enabled.",
            Tags = ["recovery", "bsod", "reboot", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoReboot", 0)],
        },
        new TweakDef
        {
            Id = "recovery-full-memory-dump",
            Label = "Enable Full Memory Dump on Crash",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets crash dump type to Full Memory Dump for maximum diagnostic info. Requires disk space equal to RAM. Default: Automatic (kernel).",
            Tags = ["recovery", "crash", "memory-dump", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-error-reporting",
            Label = "Disable Windows Error Reporting",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Error Reporting (WER). Prevents crash data from being sent to Microsoft. Default: enabled.",
            Tags = ["recovery", "error-reporting", "privacy", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-problem-reports",
            Label = "Disable Problem Reports & Solutions",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic problem report generation and solution checks. Reduces background activity and telemetry. Default: enabled.",
            Tags = ["recovery", "problem-reports", "disable", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontShowUI"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "recovery-increase-max-shadow-copies",
            Label = "Increase Max Shadow Copy Storage",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum number of shadow copy storage associations. Allows more restore points to be retained. Default: system-managed.",
            Tags = ["recovery", "shadow-copy", "restore-point", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VSS\Settings", "MaxShadowCopies", 64)],
        },
        new TweakDef
        {
            Id = "recovery-enable-auto-recovery",
            Label = "Enable Automatic Recovery",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the Windows automatic recovery feature that detects startup failures and offers repair options. Default: enabled.",
            Tags = ["recovery", "auto-recovery", "startup", "repair"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "AutoChkTimeout", 10)],
        },
        new TweakDef
        {
            Id = "recovery-disable-auto-restart-sign-on",
            Label = "Disable Auto-Restart Sign-On (ARSO)",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic sign-on after restart/update. Prevents Windows from caching credentials and auto-logging in after reboot. Default: enabled.",
            Tags = ["recovery", "security", "arso", "sign-on", "restart"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableAutomaticRestartSignOn", 1)],
        },
        new TweakDef
        {
            Id = "recovery-enable-last-known-good",
            Label = "Enable Last Known Good Configuration",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the system to report the last known good configuration to the boot manager. Helps recovery from bad driver installs. Default: system-managed.",
            Tags = ["recovery", "boot", "last-known-good", "driver"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Configuration Manager", "LastKnownGood", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-recovery-ui",
            Label = "Disable Windows Recovery UI",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the recovery UI displayed after consecutive boot failures. Use only on kiosk or server systems where manual intervention is not desired. Default: enabled.",
            Tags = ["recovery", "boot", "ui", "kiosk", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayDisabled", 1)],
        },
        new TweakDef
        {
            Id = "recovery-disable-crash-auto-reboot-timeout",
            Label = "Set Crash Reboot Timeout to 30s",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the timeout before automatic reboot after a crash to 30 seconds, giving time to read BSOD information. Default: immediate reboot.",
            Tags = ["recovery", "crash", "bsod", "timeout", "reboot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "AutoRebootTimeout", 30)],
        },
        new TweakDef
        {
            Id = "recovery-enable-event-log-crash",
            Label = "Write Crash Events to System Log",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures crash/BSOD events are written to the Windows Event Log. Essential for post-crash diagnostics. Default: enabled.",
            Tags = ["recovery", "crash", "event-log", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "LogEvent", 1)],
        },
        new TweakDef
        {
            Id = "recovery-enable-overwrite-dump",
            Label = "Enable Overwrite of Existing Dump",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Allows the system to overwrite existing crash dump files when a new crash occurs. Saves disk space by not accumulating old dumps. Default: enabled.",
            Tags = ["recovery", "crash", "dump", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1)],
        },
        new TweakDef
        {
            Id = "recovery-small-memory-dump",
            Label = "Use Small Memory Dump (64 KB)",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures the system to save a small (miniDump) crash dump instead of a full kernel or complete dump. Faster, uses minimal disk space. Default: automatic.",
            Tags = ["recovery", "crash", "dump", "minidump", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "recovery-disable-send-alert",
            Label = "Disable Admin Alert on Crash",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sending an administrative alert when a crash occurs. Reduces noise on standalone systems. Default: off.",
            Tags = ["recovery", "crash", "alert", "notification"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "SendAlert", 0)],
        },
        new TweakDef
        {
            Id = "recovery-disable-automatic-managed-page-file",
            Label = "Disable Auto-Managed Page File",
            Category = "Recovery",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic page file management. Allows manual control over page file size for advanced crash dump configuration. Default: auto-managed.",
            Tags = ["recovery", "page-file", "memory", "crash-dump", "advanced"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", 0)],
        },
    ];
}
