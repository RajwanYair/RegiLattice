namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DataCollectionPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
    private const string SqmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows";
    private const string DastKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DAST";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "datacol-disable-opt-in-notification",
            Label = "Suppress Telemetry Opt-In Change Notification",
            Category = "Data Collection Policy",
            Description =
                "Prevents Windows from displaying a notification banner when the telemetry level changes. Eliminates user-visible popups during telemetry configuration.",
            Tags = ["telemetry", "notification", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInChangeNotification")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
        },
        new TweakDef
        {
            Id = "datacol-hide-telemetry-settings-ui",
            Label = "Hide Telemetry Controls from Settings",
            Category = "Data Collection Policy",
            Description =
                "Removes the Diagnostic & Feedback section from Windows Settings, preventing users from changing telemetry level or viewing diagnostic data.",
            Tags = ["telemetry", "settings", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInSettingsUx")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
        },
        new TweakDef
        {
            Id = "datacol-disable-enterprise-auth-proxy",
            Label = "Disable Enterprise Auth Proxy for Telemetry",
            Category = "Data Collection Policy",
            Description =
                "Prevents Windows telemetry service from using authenticated proxy servers to send diagnostic data. Forces direct transmission or blocks it entirely.",
            Tags = ["telemetry", "proxy", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseAuthProxy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseAuthProxy")],
            DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseAuthProxy", 1)],
        },
        new TweakDef
        {
            Id = "datacol-disable-device-delete-button",
            Label = "Disable Delete Device Diagnostic Data Button",
            Category = "Data Collection Policy",
            Description =
                "Prevents users from deleting device diagnostic data via the 'Delete diagnostic data' button in Settings > Privacy > Diagnostics & Feedback.",
            Tags = ["telemetry", "diagnostic-data", "settings", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeviceDelete", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceDelete")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeviceDelete", 1)],
        },
        new TweakDef
        {
            Id = "datacol-disable-feedback-notifications",
            Label = "Suppress Windows Feedback Reminder Pop-Ups",
            Category = "Data Collection Policy",
            Description =
                "Prevents Windows from displaying feedback reminder notifications that prompt users to rate experiences or submit feedback to Microsoft.",
            Tags = ["feedback", "notifications", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DoNotShowFeedbackNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotShowFeedbackNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotShowFeedbackNotifications", 1)],
        },
        new TweakDef
        {
            Id = "datacol-disable-device-name-in-telemetry",
            Label = "Block Device Name in Telemetry Submissions",
            Category = "Data Collection Policy",
            Description =
                "Prevents Windows from including the device's computer name in telemetry payloads sent to Microsoft. Reduces machine-identifying data in diagnostics.",
            Tags = ["telemetry", "device-name", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowDeviceNameInTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDeviceNameInTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDeviceNameInTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "datacol-disable-ceip",
            Label = "Disable Customer Experience Improvement Program",
            Category = "Data Collection Policy",
            Description =
                "Disables Microsoft's Customer Experience Improvement Program (CEIP) which collects anonymous usage statistics across Windows components. Policy-level disable.",
            Tags = ["ceip", "telemetry", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SqmKey],
            ApplyOps = [RegOp.SetDword(SqmKey, "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(SqmKey, "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(SqmKey, "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "datacol-disable-sample-submission",
            Label = "Disable File Sample Submission to Microsoft",
            Category = "Data Collection Policy",
            Description =
                "Prevents Windows Diagnostic Analysis Service (DAST) and Windows Defender from submitting file samples to Microsoft's analysis cloud for threat intelligence.",
            Tags = ["sample-submission", "telemetry", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [DastKey],
            ApplyOps = [RegOp.SetDword(DastKey, "AllowSampleSubmission", 0)],
            RemoveOps = [RegOp.DeleteValue(DastKey, "AllowSampleSubmission")],
            DetectOps = [RegOp.CheckDword(DastKey, "AllowSampleSubmission", 0)],
        },
        new TweakDef
        {
            Id = "datacol-disable-onesettings-downloads",
            Label = "Block WindowsOneSettings Telemetry Overrides",
            Category = "Data Collection Policy",
            Description =
                "Prevents Windows from downloading one-time configuration overrides (OneSettings) that can dynamically change data collection settings without a Windows Update.",
            Tags = ["telemetry", "onesettings", "policy", "hardening"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOneSettingsDownloads", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOneSettingsDownloads")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOneSettingsDownloads", 1)],
        },
        new TweakDef
        {
            Id = "datacol-disable-diagnostic-page",
            Label = "Hide Diagnostic Data Viewer Page in Settings",
            Category = "Data Collection Policy",
            Description =
                "Hides the Diagnostic Data Viewer page from Windows Settings > Privacy & Security > Diagnostics & Feedback, preventing users from reviewing diagnostic data submissions.",
            Tags = ["telemetry", "diagnostic-data", "settings", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HideDiagnosticPage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HideDiagnosticPage")],
            DetectOps = [RegOp.CheckDword(Key, "HideDiagnosticPage", 1)],
        },
    ];
}
