namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── Merged from CrashDiagnostics.cs ──────────────────────────────────────────────────

[TweakModule]
internal static class CrashDiagnostics
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "crash-disable-auto-restart",
            Label = "Disable Auto-Restart on BSOD",
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Id = "crash-disable-error-dialog",
            Label = "Disable Automatic Error Dialog Boxes",
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
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
            Category = "Maintenance 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables creation of user-mode crash dump files when applications crash. Saves disk space. Default: dump files created.",
            Tags = ["crash", "user-mode", "dump", "disk-space"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 0)],
        },
    ];
}
