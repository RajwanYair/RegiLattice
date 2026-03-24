// RegiLattice.Core — Tweaks/ErrorReportingPolicy.cs
// Windows Error Reporting (WER) Group Policy settings — corporate WER control.
// Slug: "werpol" — distinct from CrashDiagnostics.cs (user-level crash settings).
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting
// These are Group Policy managed WER controls used in enterprise deployments.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ErrorReportingPolicy
{
    private const string WerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
    private const string WerConsent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting\Consent";
    private const string WerQueue = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting\ExcludedApplications";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "werpol-disable-wer",
            Label = "Disable Windows Error Reporting via Group Policy",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Tags = ["wer", "error reporting", "crash", "privacy", "group policy"],
            Description =
                "Disables Windows Error Reporting (WER) system-wide via Group Policy. "
                + "Disabled = 1. No crash reports are collected, stored, or sent to Microsoft. "
                + "Default: enabled. Recommended for air-gapped or high-privacy deployments. "
                + "Note: disabling WER also prevents WER-based crash analysis in Event Viewer.",
            ApplyOps = [RegOp.SetDword(WerPol, "Disabled", 1)],
            RemoveOps = [RegOp.SetDword(WerPol, "Disabled", 0)],
            DetectOps = [RegOp.CheckDword(WerPol, "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "werpol-disable-internet-send",
            Label = "WER: Block Sending Error Reports to Microsoft",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["wer", "error reporting", "telemetry", "privacy", "group policy"],
            Description =
                "Prevents Windows Error Reporting from sending crash data over the internet to Microsoft. "
                + "DontSendAdditionalData = 1. Crash data is still collected locally but not transmitted. "
                + "Preferred privacy setting that retains local crash diagnostics while stopping uploads.",
            ApplyOps = [RegOp.SetDword(WerPol, "DontSendAdditionalData", 1)],
            RemoveOps = [RegOp.SetDword(WerPol, "DontSendAdditionalData", 0)],
            DetectOps = [RegOp.CheckDword(WerPol, "DontSendAdditionalData", 1)],
        },
        new TweakDef
        {
            Id = "werpol-disable-crash-dialog",
            Label = "WER: Suppress Crash Report Dialog",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wer", "error reporting", "crash", "ui", "kiosk", "group policy"],
            Description =
                "Suppresses the 'Windows has stopped working' crash dialog box shown to users. "
                + "DontShowUI = 1. Errors are still logged but users see no dialog. "
                + "Recommended for kiosk deployments and unattended servers to avoid hanging on UI prompts.",
            ApplyOps = [RegOp.SetDword(WerPol, "DontShowUI", 1)],
            RemoveOps = [RegOp.SetDword(WerPol, "DontShowUI", 0)],
            DetectOps = [RegOp.CheckDword(WerPol, "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "werpol-bypass-throttling",
            Label = "WER: Bypass Error Report Throttling",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wer", "error reporting", "crash", "diagnostics", "group policy"],
            Description =
                "Disables WER's built-in throttling that limits how many reports can be sent. "
                + "BypassDataThrottling = 1. Ensures all crash reports are captured in corporate "
                + "WER server deployments where local rate-limiting would mask incident scope. "
                + "Default: throttling enabled. Use in conjunction with a corporate WER server.",
            ApplyOps = [RegOp.SetDword(WerPol, "BypassDataThrottling", 1)],
            RemoveOps = [RegOp.SetDword(WerPol, "BypassDataThrottling", 0)],
            DetectOps = [RegOp.CheckDword(WerPol, "BypassDataThrottling", 1)],
        },
        new TweakDef
        {
            Id = "werpol-disable-logging",
            Label = "WER: Disable Error Report Logging to Event Log",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Tags = ["wer", "error reporting", "event log", "privacy", "group policy"],
            Description =
                "Prevents WER from writing crash report summaries to the Windows Application event log. "
                + "LoggingDisabled = 1. Reduces noise in event logs on systems with frequent non-critical "
                + "application crashes. Default: logging enabled.",
            ApplyOps = [RegOp.SetDword(WerPol, "LoggingDisabled", 1)],
            RemoveOps = [RegOp.SetDword(WerPol, "LoggingDisabled", 0)],
            DetectOps = [RegOp.CheckDword(WerPol, "LoggingDisabled", 1)],
        },
        new TweakDef
        {
            Id = "werpol-auto-approve-reports",
            Label = "WER: Auto-Approve All Error Report Submissions",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["wer", "error reporting", "consent", "group policy", "enterprise"],
            Description =
                "Configures WER consent to automatically send all error reports without prompting users. "
                + "DefaultConsent = 4 (send all data). Used in enterprise environments where crash data "
                + "is routed to an internal WER server. Default: prompt user (1). "
                + "Levels: 1=prompt, 2=basic params, 3=params+heap, 4=all data.",
            ApplyOps = [RegOp.SetDword(WerConsent, "DefaultConsent", 4)],
            RemoveOps = [RegOp.SetDword(WerConsent, "DefaultConsent", 1)],
            DetectOps = [RegOp.CheckDword(WerConsent, "DefaultConsent", 4)],
        },
        new TweakDef
        {
            Id = "werpol-disable-heap-dumps",
            Label = "WER: Disable Heap Memory Dump Collection",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["wer", "error reporting", "memory dump", "privacy", "security", "group policy"],
            Description =
                "Prevents WER from collecting heap memory dumps alongside crash reports. "
                + "LocalDumps\\DumpType = 0 (no dump). Heap dumps can contain sensitive data "
                + "including passwords, tokens, or PII present in application memory at crash time. "
                + "Default: dumps enabled. Recommended for privacy-sensitive deployments.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", "DumpType", 0)],
        },
        new TweakDef
        {
            Id = "werpol-disable-queue-reporting",
            Label = "WER: Disable Queued Error Report Sending",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wer", "error reporting", "queue", "privacy", "group policy"],
            Description =
                "Disables WER's queuing mechanism that stores crash reports and sends them later "
                + "when connectivity is available. MaxQueueSizePercentage = 0. "
                + "Prevents accumulation of potentially sensitive crash data in %LOCALAPPDATA%\\Microsoft\\Windows\\WER\\. "
                + "Default: up to 10% of available disk quota used for queue.",
            ApplyOps = [RegOp.SetDword(WerPol, "MaxQueueSizePercentage", 0)],
            RemoveOps = [RegOp.DeleteValue(WerPol, "MaxQueueSizePercentage")],
            DetectOps = [RegOp.CheckDword(WerPol, "MaxQueueSizePercentage", 0)],
        },
        new TweakDef
        {
            Id = "werpol-disable-unplanned-shutdown-reports",
            Label = "WER: Suppress Unplanned OS Shutdown Reports",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wer", "error reporting", "shutdown", "privacy", "group policy"],
            Description =
                "Prevents WER from generating and sending a report after unplanned OS shutdowns "
                + "(power loss, hard resets). DisableArchive = 1 blocks archiving of these events. "
                + "Reduces telemetry from power-sensitive environments such as laptops in unreliable "
                + "power conditions. Default: reports sent on next boot.",
            ApplyOps = [RegOp.SetDword(WerPol, "DisableArchive", 1)],
            RemoveOps = [RegOp.SetDword(WerPol, "DisableArchive", 0)],
            DetectOps = [RegOp.CheckDword(WerPol, "DisableArchive", 1)],
        },
        new TweakDef
        {
            Id = "werpol-purge-report-archive",
            Label = "WER: Set Maximum Archive Store to Zero Days",
            Category = "Error Reporting Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["wer", "error reporting", "archive", "privacy", "cleanup", "group policy"],
            Description =
                "Sets the WER archive retention period to 0 days, causing crash reports in "
                + "%ProgramData%\\Microsoft\\Windows\\WER\\ReportArchive to be purged immediately. "
                + "MaxArchiveCount = 1. Prevents long-term storage of crash dumps that may "
                + "contain sensitive application memory. Default: reports kept for 1 year.",
            ApplyOps = [RegOp.SetDword(WerPol, "MaxArchiveCount", 1)],
            RemoveOps = [RegOp.DeleteValue(WerPol, "MaxArchiveCount")],
            DetectOps = [RegOp.CheckDword(WerPol, "MaxArchiveCount", 1)],
        },
    ];
}
