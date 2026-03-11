namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CrashDiagnostics
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "crash-disable-auto-restart",
            Label = "Disable Auto-Restart on BSOD",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Stay on BSOD screen instead of auto-rebooting. Helps read stop codes. Default: auto-restart. Recommended: disabled.",
            Tags = ["bsod", "crash", "restart", "blue-screen"],
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
            Id = "crash-set-minidump",
            Label = "Set Crash Dump to Minidump",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Save only small minidumps on crash (saves disk). Default: automatic memory dump.",
            Tags = ["crash", "dump", "minidump", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 3)],
        },
        new TweakDef
        {
            Id = "crash-disable-crash-dump",
            Label = "Disable Crash Dump Entirely",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Do not write any crash dump files. Saves disk but loses BSOD data. Default: automatic dump.",
            Tags = ["crash", "dump", "disable", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-wer",
            Label = "Disable Windows Error Reporting",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable WER service from sending error reports to Microsoft. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["wer", "error", "reporting", "privacy"],
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
            Id = "crash-disable-wer-policy",
            Label = "Disable WER (Policy)",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Machine-wide policy to disable Windows Error Reporting. Default: not set.",
            Tags = ["wer", "error", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
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
            Id = "crash-disable-wer-user",
            Label = "Disable WER (User Level)",
            Category = "Crash & Diagnostics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable error reporting for current user only. Default: enabled.",
            Tags = ["wer", "error", "user", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Windows Error Reporting"],
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
            Id = "crash-disable-dump-overwrite",
            Label = "Disable Crash Dump Overwrite",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Keep all crash dumps instead of overwriting with latest. Default: overwrite.",
            Tags = ["crash", "dump", "overwrite", "history"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Overwrite", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-scripted-diagnostics",
            Label = "Disable Scripted Diagnostics",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable automatic troubleshooters and scripted diagnostics. Default: enabled.",
            Tags = ["diagnostics", "troubleshooter", "script"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy", "EnableQueryRemoteServer", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics", "EnableDiagnostics", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy", "EnableQueryRemoteServer"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics", "EnableDiagnostics"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics", "EnableDiagnostics", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-perf-tracking",
            Label = "Disable Performance Tracking (WDI)",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable Windows Diagnostic Infrastructure performance tracking. Default: enabled.",
            Tags = ["wdi", "performance", "tracking", "diagnostics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}", "ScenarioExecutionEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}", "ScenarioExecutionEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}", "ScenarioExecutionEnabled", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-app-compat-engine",
            Label = "Disable App Compatibility Engine",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable the application compatibility engine. Slight performance gain. Default: enabled.",
            Tags = ["compatibility", "app-compat", "engine", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
        },
        new TweakDef
        {
            Id = "crash-disable-pca",
            Label = "Disable Program Compatibility Assistant",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable PCA popup suggesting compatibility settings. Default: enabled. Recommended: disabled if not needed.",
            Tags = ["pca", "compatibility", "assistant", "popup"],
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
            Id = "crash-disable-feedback-notifications",
            Label = "Disable Feedback Request Notifications",
            Category = "Crash & Diagnostics",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows feedback request notification prompts. Sets NumberOfSIUFInPeriod to 0 to suppress all feedback popups. Default: periodic. Recommended: disabled.",
            Tags = ["crash", "feedback", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "crash-disable-error-dialog",
            Label = "Disable Automatic Error Dialog Boxes",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic error dialog boxes (ErrorMode=2). Suppresses critical-error-handler message boxes for background services. Default: 0 (show all). Recommended: 2 for servers.",
            Tags = ["crash", "error", "dialog", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows", "ErrorMode", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows", "ErrorMode", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows", "ErrorMode", 2)],
        },
        new TweakDef
        {
            Id = "crash-enable-full-memory-dump",
            Label = "Enable Full Memory Dumps",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets CrashDumpEnabled to 1 (Complete memory dump) for full debugging information on BSOD. Requires sufficient page file. Default: 7 (Automatic). Recommended: 1 for debugging.",
            Tags = ["crash", "dump", "memory", "debugging", "bsod"],
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
            Id = "crash-disable-wer-queue",
            Label = "Disable WER Queue Reporting",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Error Reporting queue-based report collection. Stops WER from queuing reports for later submission. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["crash", "wer", "queue", "reporting", "privacy"],
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
            Id = "crash-set-kernel-dump-only",
            Label = "Set Crash Dump to Kernel Only",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets CrashDumpEnabled to 2 (Kernel memory dump). Captures only kernel-mode memory, saving disk space while retaining key info. Default: 7 (Automatic). Recommended: 2 for production.",
            Tags = ["crash", "dump", "kernel", "production", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 7),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "CrashDumpEnabled", 2)],
        },
        new TweakDef
        {
            Id = "crash-disable-jit-debugger",
            Label = "Disable Auto-Attach of JIT Debugger",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from automatically launching a JIT debugger when an application crashes. Suppresses the 'attach debugger?' dialog. Default: 1 (auto-attach). Recommended: 0 on production machines.",
            Tags = ["crash", "debugger", "jit", "aedebug", "dev"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Auto", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Auto", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "Auto", "0")],
        },
        new TweakDef
        {
            Id = "crash-wer-no-additional-data",
            Label = "WER: Don't Send Additional Crash Data",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Instructs Windows Error Reporting to not include supplemental data (heap dumps, user-mode state) when submitting crash reports. Default: sends all data. Recommended: disabled for privacy.",
            Tags = ["crash", "wer", "privacy", "data", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "crash-wer-disable-archive",
            Label = "WER: Disable Local Report Archive",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = @"Stops Windows Error Reporting from maintaining a local archive of submitted crash reports in ProgramData\Microsoft\Windows\WER\ReportArchive. Default: archive enabled. Recommended: disabled for disk space.",
            Tags = ["crash", "wer", "archive", "disk", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DisableArchive", 1)],
        },
        new TweakDef
        {
            Id = "crash-wer-min-consent",
            Label = "WER: Send Parameters Only (Minimal Consent)",
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets WER DefaultConsent=2 (parameters only), limiting transmitted crash data to exception codes and fault module, not executable images. Default: varies (1=always ask, 4=send all). Recommended: 2 for privacy.",
            Tags = ["crash", "wer", "consent", "privacy", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent", "DefaultConsent", 2)],
        },
        new TweakDef
        {
            Id = "crash-disable-online-crash-analysis",
            Label = "Disable Online Crash Analysis",
            Category = "Crash & Diagnostics",
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
            Category = "Crash & Diagnostics",
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
            Category = "Crash & Diagnostics",
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
            Category = "Crash & Diagnostics",
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
            Category = "Crash & Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Program Compatibility Assistant that prompts after app crashes. Default: enabled.",
            Tags = ["crash", "compatibility", "assistant", "pca"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
    ];
}
