namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyWindowsFeedback
{
    private const string DataCollection = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
    private const string FeedbackHub = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Feedback";
    private const string ReportingKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
    private const string PcHealth = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "fbk-suppress-feedback-prompt",
                Label = "Feedback: Suppress Feedback Survey Prompts",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Sets DoNotShowFeedbackNotifications=1 under the DataCollection policy key. "
                    + "Suppresses Windows Feedback survey prompts that periodically appear asking users to "
                    + "rate their experience or submit diagnostic reports. "
                    + "Prevents productivity interruptions from unsolicited feedback pop-ups.",
                Tags = ["feedback", "notifications", "privacy", "telemetry", "survey"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Feedback survey prompts suppressed; no product functionality affected.",
                ApplyOps = [RegOp.SetDword(DataCollection, "DoNotShowFeedbackNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "DoNotShowFeedbackNotifications")],
                DetectOps = [RegOp.CheckDword(DataCollection, "DoNotShowFeedbackNotifications", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-one-settings-downloads",
                Label = "Feedback: Disable One Settings Telemetry Downloads",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Prevents Windows from downloading updated telemetry configuration files from Microsoft servers. "
                    + "These 'One Settings' downloads adjust what diagnostic data is collected without a full update. "
                    + "Disabling them ensures the telemetry profile is static and controlled by local policy only.",
                Tags = ["feedback", "telemetry", "one-settings", "privacy", "cloud"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Telemetry collection profile fixed to local policy; Microsoft cannot adjust it remotely.",
                ApplyOps = [RegOp.SetDword(DataCollection, "DisableOneSettingsDownloads", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "DisableOneSettingsDownloads")],
                DetectOps = [RegOp.CheckDword(DataCollection, "DisableOneSettingsDownloads", 1)],
            },
            new TweakDef
            {
                Id = "fbk-limit-dump-collection",
                Label = "Feedback: Limit Diagnostic Dump File Collection",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Restricts collection of memory and crash dump files sent to Microsoft as part of "
                    + "Windows Error Reporting and diagnostic feedback. "
                    + "Reduces the chance of partial memory contents — which may include sensitive in-memory data — "
                    + "from being uploaded to Microsoft diagnostics servers.",
                Tags = ["feedback", "dump-collection", "crash-report", "privacy", "telemetry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Fewer dump files transmitted; in-memory data exposure to Microsoft reduced.",
                ApplyOps = [RegOp.SetDword(DataCollection, "LimitDumpCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "LimitDumpCollection")],
                DetectOps = [RegOp.CheckDword(DataCollection, "LimitDumpCollection", 1)],
            },
            new TweakDef
            {
                Id = "fbk-limit-diagnostic-log-collection",
                Label = "Feedback: Limit Diagnostic Log Collection",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Restricts the collection of system diagnostic logs sent to Microsoft as part of "
                    + "the Windows diagnostic data pipeline. "
                    + "Complementary to LimitDumpCollection; prevents large log bundles from being "
                    + "automatically uploaded without explicit user action.",
                Tags = ["feedback", "diagnostic-log", "privacy", "telemetry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Diagnostic log upload volume reduced; Windows Update and error reporting still function.",
                ApplyOps = [RegOp.SetDword(DataCollection, "LimitDiagnosticLogCollection", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "LimitDiagnosticLogCollection")],
                DetectOps = [RegOp.CheckDword(DataCollection, "LimitDiagnosticLogCollection", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-wer-reporting",
                Label = "Feedback: Disable Windows Error Reporting",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Disables Windows Error Reporting (WER) entirely, preventing crash and error reports "
                    + "from being sent to Microsoft. "
                    + "Applications will still crash and be captured in Event Viewer, but no data is transmitted externally. "
                    + "Recommended in air-gapped, classified, or strict-privacy environments.",
                Tags = ["feedback", "wer", "error-reporting", "privacy", "crash"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "No crash data sent to Microsoft; WER data still written locally to Event Log and %LOCALAPPDATA%.",
                ApplyOps = [RegOp.SetDword(ReportingKey, "Disabled", 1)],
                RemoveOps = [RegOp.DeleteValue(ReportingKey, "Disabled")],
                DetectOps = [RegOp.CheckDword(ReportingKey, "Disabled", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-wer-server-consent",
                Label = "Feedback: Disable WER Automatic Consent to Send Data",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Prevents Windows Error Reporting from automatically consenting to send additional data "
                    + "to Microsoft without prompting the user. "
                    + "Sets DefaultConsent to 1 (Always Ask), requiring explicit user approval for every data submission.",
                Tags = ["feedback", "wer", "consent", "privacy", "telemetry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WER will prompt before sending additional report data; no silent data submission.",
                ApplyOps = [RegOp.SetDword(ReportingKey, "DefaultConsent", 1)],
                RemoveOps = [RegOp.DeleteValue(ReportingKey, "DefaultConsent")],
                DetectOps = [RegOp.CheckDword(ReportingKey, "DefaultConsent", 1)],
            },
            new TweakDef
            {
                Id = "fbk-disable-wer-network-lookup",
                Label = "Feedback: Disable WER Network Solution Lookup",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Prevents Windows Error Reporting from querying Microsoft servers for known solutions "
                    + "to application crashes. "
                    + "Stops outbound WER HTTP lookups that reveal application crash signatures and module hashes "
                    + "to Microsoft's online databases.",
                Tags = ["feedback", "wer", "network-lookup", "privacy", "crash-signature"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "No outbound WER solution lookups; crash signatures not disclosed to Microsoft online service.",
                ApplyOps = [RegOp.SetDword(ReportingKey, "DontSendAdditionalData", 1)],
                RemoveOps = [RegOp.DeleteValue(ReportingKey, "DontSendAdditionalData")],
                DetectOps = [RegOp.CheckDword(ReportingKey, "DontSendAdditionalData", 1)],
            },
            new TweakDef
            {
                Id = "fbk-log-errors-to-eventlog",
                Label = "Feedback: Log WER Events to System Event Log",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Forces Windows Error Reporting to write application crash events to the Windows System "
                    + "Event Log even when WER network submission is disabled. "
                    + "Enables local audit trails and SIEM correlation without requiring cloud crash reporting.",
                Tags = ["feedback", "wer", "event-log", "audit", "siem"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Application crashes written to Event Log for local auditing; no change to external reporting.",
                ApplyOps = [RegOp.SetDword(ReportingKey, "LoggingDisabled", 0)],
                RemoveOps = [RegOp.DeleteValue(ReportingKey, "LoggingDisabled")],
                DetectOps = [RegOp.CheckDword(ReportingKey, "LoggingDisabled", 0)],
            },
            new TweakDef
            {
                Id = "fbk-disable-pchealth-error-reporting",
                Label = "Feedback: Disable PC Health Error Reporting Service",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Disables the PC Health error reporting component which collects hardware and "
                    + "application reliability data for the Windows Reliability Monitor. "
                    + "Prevents telemetry from the PC Health subsystem from being sent to Microsoft.",
                Tags = ["feedback", "pc-health", "reliability-monitor", "telemetry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "PC Health diagnostic feed to Microsoft disabled; Reliability Monitor history stays local.",
                ApplyOps = [RegOp.SetDword(PcHealth, "AllOrNone", 0)],
                RemoveOps = [RegOp.DeleteValue(PcHealth, "AllOrNone")],
                DetectOps = [RegOp.CheckDword(PcHealth, "AllOrNone", 0)],
            },
            new TweakDef
            {
                Id = "fbk-disable-enhanced-diag-upload",
                Label = "Feedback: Block Windows Analytics Enhanced Diagnostic Upload",
                Category = "Privacy — Windows Feedback",
                Description =
                    "Limits the Enhanced Diagnostic Data level used by Windows Analytics from uploading "
                    + "browser usage, app usage, and file type activity in addition to basic crash data. "
                    + "Sets LimitEnhancedDiagnosticDataWindowsAnalytics=1 to restrict the Enhanced level "
                    + "to only the required minimum for Windows Update.",
                Tags = ["feedback", "windows-analytics", "diagnostic-data", "privacy", "telemetry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enhanced diagnostic upload restricted to basic Windows Update data only; usage analytics blocked.",
                ApplyOps = [RegOp.SetDword(DataCollection, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
                RemoveOps = [RegOp.DeleteValue(DataCollection, "LimitEnhancedDiagnosticDataWindowsAnalytics")],
                DetectOps = [RegOp.CheckDword(DataCollection, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
            },
        ];
}
