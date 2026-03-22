// RegiLattice.Core — Tweaks/WindowsDiagnostics.cs
// Windows Diagnostics, Reliability & Feedback policy tweaks (Sprint 107).
// Slug: "wdiag-*" — distinct from Telemetry.cs (AllowTelemetry DWORD),
//   TelemetryAdvanced.cs (DiagTrack service), Privacy.cs (general data/location).
// Focus: Windows Error Reporting (WER), Problem Steps Recorder (PSR),
//   Application Compatibility telemetry, Reliability Monitor, and
//   the Feedback Hub / Windows Insider notification machinery.
// Registry bases:
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting
//   HKLM\SOFTWARE\Microsoft\Windows\Windows Error Reporting
//   HKLM\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat
//   HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsDiagnostics
{
    private const string WerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
    private const string WerService = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";
    private const string WerPCHealth = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";
    private const string AppCompatPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";
    private const string AppCompatFlags = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags";
    private const string FeedbackPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wdiag-disable-wer-reporting",
            Label = "Diagnostics: Disable Windows Error Reporting (WER)",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 4,
            RegistryKeys = [WerPolicy],
            Tags = ["diagnostics", "wer", "error-reporting", "privacy", "telemetry", "crash", "microsoft"],
            Description =
                "Sets Disabled=1 in the Windows Error Reporting policy. "
                + "Stops the WerFault.exe process from uploading crash dumps, minidumps, and "
                + "application error reports to Microsoft. Reduces background I/O on crash events "
                + "and prevents unintentional sensitive data transmission in crash reports.",
            ApplyOps = [RegOp.SetDword(WerPolicy, "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPolicy, "Disabled")],
            DetectOps = [RegOp.CheckDword(WerPolicy, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "wdiag-disable-wer-queue",
            Label = "Diagnostics: Disable Windows Error Report Queue (Prevent Deferred Uploads)",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 4,
            RegistryKeys = [WerService],
            Tags = ["diagnostics", "wer", "error-reporting", "privacy", "disk"],
            Description =
                "Sets DontSendAdditionalData=1 in the WER service key. "
                + "Prevents WER from sending additional data beyond the basic error notification "
                + "(e.g. queued minidump files, extended diagnostic data) to Microsoft. "
                + "The queue can accumulate gigabytes of crash archives on unstable systems.",
            ApplyOps = [RegOp.SetDword(WerService, "DontSendAdditionalData", 1)],
            RemoveOps = [RegOp.DeleteValue(WerService, "DontSendAdditionalData")],
            DetectOps = [RegOp.CheckDword(WerService, "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "wdiag-opt-out-wer-enterprise",
            Label = "Diagnostics: Opt Out of Windows Error Reporting Enterprise Tier",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [WerPCHealth],
            Tags = ["diagnostics", "wer", "error-reporting", "privacy", "enterprise"],
            Description =
                "Sets DoReport=0 in PCHealth\\ErrorReporting. "
                + "Disables Windows Error Reporting at the PCHealth layer (legacy 'Dr Watson' path). "
                + "This covers older error-reporting pathways that bypass the modern WER policy. "
                + "CorpSafe=true because corporate environments often control WER separately.",
            ApplyOps = [RegOp.SetDword(WerPCHealth, "DoReport", 0)],
            RemoveOps = [RegOp.DeleteValue(WerPCHealth, "DoReport")],
            DetectOps = [RegOp.CheckDword(WerPCHealth, "DoReport", 0)],
        },
        new TweakDef
        {
            Id = "wdiag-disable-app-compat-telemetry",
            Label = "Diagnostics: Disable Application Compatibility Telemetry (CEIP)",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 4,
            RegistryKeys = [AppCompatPolicy],
            Tags = ["diagnostics", "app-compat", "telemetry", "privacy", "ceip"],
            Description =
                "Sets DisableInventory=1 and DisableUAR=1 in AppCompat policy. "
                + "Stops the Application Compatibility Telemetry component from collecting and "
                + "uploading application compatibility data and 'User Activity Reporting' logs. "
                + "This data feeds into Microsoft's CEIP program.",
            ApplyOps = [RegOp.SetDword(AppCompatPolicy, "DisableInventory", 1), RegOp.SetDword(AppCompatPolicy, "DisableUAR", 1)],
            RemoveOps = [RegOp.DeleteValue(AppCompatPolicy, "DisableInventory"), RegOp.DeleteValue(AppCompatPolicy, "DisableUAR")],
            DetectOps = [RegOp.CheckDword(AppCompatPolicy, "DisableInventory", 1), RegOp.CheckDword(AppCompatPolicy, "DisableUAR", 1)],
        },
        new TweakDef
        {
            Id = "wdiag-disable-app-compat-engine",
            Label = "Diagnostics: Disable Application Compatibility Engine",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 3,
            RegistryKeys = [AppCompatPolicy],
            Tags = ["diagnostics", "app-compat", "performance", "privacy"],
            Description =
                "Sets DisableProgramLog=1 in AppCompat policy. "
                + "Disables the Windows Application Compatibility engine (Shim Engine). "
                + "Slightly reduces application launch latency. "
                + "WARNING: may prevent older 16-bit or poorly-written apps from running "
                + "because compatibility shims are no longer applied at launch.",
            ApplyOps = [RegOp.SetDword(AppCompatPolicy, "DisableProgramLog", 1)],
            RemoveOps = [RegOp.DeleteValue(AppCompatPolicy, "DisableProgramLog")],
            DetectOps = [RegOp.CheckDword(AppCompatPolicy, "DisableProgramLog", 1)],
        },
        new TweakDef
        {
            Id = "wdiag-disable-psr",
            Label = "Diagnostics: Disable Problem Steps Recorder (PSR / psr.exe)",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [WerPolicy],
            Tags = ["diagnostics", "psr", "screen-capture", "privacy", "security"],
            Description =
                "Sets DisableArchive=1 in the WER policy (blocks PSR archive creation). "
                + "Disables the Problem Steps Recorder (psr.exe) which can silently screenshot "
                + "an entire troubleshooting session. Prevents potential privacy exposure if "
                + "support staff misuse PSR on sensitive workloads.",
            ApplyOps = [RegOp.SetDword(WerPolicy, "DisableArchive", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPolicy, "DisableArchive")],
            DetectOps = [RegOp.CheckDword(WerPolicy, "DisableArchive", 1)],
        },
        new TweakDef
        {
            Id = "wdiag-disable-feedback-notifications",
            Label = "Diagnostics: Disable Windows Feedback and Satisfaction Surveys",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [FeedbackPolicy],
            Tags = ["diagnostics", "feedback", "feedback-hub", "notifications", "privacy", "debloat"],
            Description =
                "Sets DoNotShowFeedbackNotifications=1 in DataCollection policy. "
                + "Prevents Windows from displaying pop-up feedback request dialogs "
                + "(\"How do you like Windows?\", \"Rate your experience\"). "
                + "These notifications are distracting and require user interaction to dismiss.",
            ApplyOps = [RegOp.SetDword(FeedbackPolicy, "DoNotShowFeedbackNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedbackPolicy, "DoNotShowFeedbackNotifications")],
            DetectOps = [RegOp.CheckDword(FeedbackPolicy, "DoNotShowFeedbackNotifications", 1)],
        },
        new TweakDef
        {
            Id = "wdiag-disable-reliability-monitor",
            Label = "Diagnostics: Disable Reliability Monitor Data Collection",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 4,
            RegistryKeys = [AppCompatFlags],
            Tags = ["diagnostics", "reliability-monitor", "privacy", "disk", "performance"],
            Description =
                "Sets DisableReliabilityAnalysisComponent=1 in AppCompatFlags. "
                + "Stops the Reliability Analysis Component (RAC) from tracking application "
                + "failures and system events in the Reliability Monitor database (RacAgent task). "
                + "Reduces background disk activity. Reliability Monitor data is also deleted.",
            ApplyOps = [RegOp.SetDword(AppCompatFlags, "DisableReliabilityAnalysisComponent", 1)],
            RemoveOps = [RegOp.DeleteValue(AppCompatFlags, "DisableReliabilityAnalysisComponent")],
            DetectOps = [RegOp.CheckDword(AppCompatFlags, "DisableReliabilityAnalysisComponent", 1)],
        },
        new TweakDef
        {
            Id = "wdiag-set-wer-local-dumps",
            Label = "Diagnostics: Redirect Crash Dumps to Local Folder (No Upload)",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 3,
            SafetyRating = 5,
            RegistryKeys = [WerService],
            Tags = ["diagnostics", "wer", "crash-dump", "privacy", "forensics", "debugging"],
            Description =
                "Sets LocalDumps path to %LOCALAPPDATA%\\CrashDumps in WER service key. "
                + "Configures WER to save application crash minidumps locally instead of uploading "
                + "to Microsoft. Allows developers and IT to analyse crashes without cloud upload. "
                + "Dumps are useful for post-mortem debugging while keeping data on-premises.",
            ApplyOps =
            [
                RegOp.SetExpandString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps",
                    "DumpFolder",
                    @"%LOCALAPPDATA%\CrashDumps"
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpCount", 10),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 2),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps",
                    "DumpFolder",
                    @"%LOCALAPPDATA%\CrashDumps"
                ),
            ],
        },
        new TweakDef
        {
            Id = "wdiag-disable-insider-preview-builds",
            Label = "Diagnostics: Block Windows Insider Preview Build Notifications",
            Category = "Windows Diagnostics",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 17763,
            ImpactScore = 2,
            SafetyRating = 5,
            RegistryKeys = [FeedbackPolicy],
            Tags = ["diagnostics", "insider", "windows-update", "notifications", "privacy", "debloat"],
            Description =
                "Sets DisableWindowsConsumerFeatures=1 in DataCollection policy. "
                + "Prevents Windows from offering Insider Preview builds, feature previews, and "
                + "consumer-targeted nag screens in enterprise/standalone environments. "
                + "Keeps production machines on stable release channels.",
            ApplyOps = [RegOp.SetDword(FeedbackPolicy, "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(FeedbackPolicy, "DisableWindowsConsumerFeatures")],
            DetectOps = [RegOp.CheckDword(FeedbackPolicy, "DisableWindowsConsumerFeatures", 1)],
        },
    ];
}
