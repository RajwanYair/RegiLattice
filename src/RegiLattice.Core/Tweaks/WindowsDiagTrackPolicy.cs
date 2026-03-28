// WindowsDiagTrackPolicy.cs — Connected User Experiences & Telemetry / DiagTrack policies
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection
// Slug: diagtrk
// Category: Windows DiagTrack Policy

namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class WindowsDiagTrackPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "diagtrk-set-telemetry-security-only",
            Label = "Windows DiagTrack: Set Telemetry to Security (Level 0)",
            Category = "Windows DiagTrack Policy",
            Description =
                "Sets the Windows diagnostic data collection level to Security (level 0) — the lowest telemetry tier. "
                + "Security-level telemetry sends only data required for Windows Defender ATP and Windows security functions. "
                + "No app usage, browsing, or performance data is collected at this level. "
                + "Note: Security level (0) applies to Enterprise and Education editions only; other editions receive Basic as their minimum.",
            Tags = ["telemetry", "diagtrack", "privacy", "security-level", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowTelemetry", 0)],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Minimizes telemetry to security-only; maximum privacy for enterprise endpoints.",
        },
        new TweakDef
        {
            Id = "diagtrk-disable-opt-in-change-notifications",
            Label = "Windows DiagTrack: Disable Telemetry Opt-In Change Notifications",
            Category = "Windows DiagTrack Policy",
            Description =
                "Prevents Windows from prompting users to change their diagnostic data settings. "
                + "When telemetry is centrally managed via GPO, user-facing prompts to adjust data collection settings are noise and could cause confusion. "
                + "This policy suppresses the Settings dialog and toast notifications that ask users to consent to telemetry level changes. "
                + "Removing this policy re-enables opt-in change notifications and Settings prompts.",
            Tags = ["telemetry", "diagtrack", "opt-in", "notifications", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInChangeNotification")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInChangeNotification", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Suppresses telemetry opt-in dialogs; keeps managed telemetry settings stable.",
        },
        new TweakDef
        {
            Id = "diagtrk-disable-opt-in-settings-ui",
            Label = "Windows DiagTrack: Disable Telemetry Opt-In Settings UI",
            Category = "Windows DiagTrack Policy",
            Description =
                "Hides the diagnostic data opt-in settings page in the Privacy section of Windows Settings. "
                + "When telemetry is managed via GPO or Intune, the Settings page is redundant and could confuse users into thinking they can adjust the policy. "
                + "Hiding the page ensures users do not inadvertently change settings that are centrally managed. "
                + "Removing this policy restores the telemetry settings UI in Windows Settings > Privacy.",
            Tags = ["telemetry", "diagtrack", "settings-ui", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetryOptInSettingsUx")],
            DetectOps = [RegOp.CheckDword(Key, "DisableTelemetryOptInSettingsUx", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes telemetry UI from Settings; prevents user changes to managed telemetry policy.",
        },
        new TweakDef
        {
            Id = "diagtrk-disable-enterprise-auth-proxy",
            Label = "Windows DiagTrack: Disable Enterprise Authentication Proxy for Telemetry",
            Category = "Windows DiagTrack Policy",
            Description =
                "Prevents the DiagTrack service from using authenticated proxy servers for telemetry data uploads. "
                + "When an enterprise proxy requires NTLM/Kerberos authentication, the DiagTrack service may authenticate with machine credentials. "
                + "Disabling this proxy allows the service to fail uploads rather than authenticate, reducing credential exposure over potentially monitored proxies. "
                + "Removing this policy allows DiagTrack to use the enterprise auth proxy for uploads.",
            Tags = ["telemetry", "diagtrack", "proxy", "enterprise", "credentials", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseAuthProxy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseAuthProxy")],
            DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseAuthProxy", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks DiagTrack proxy auth; prevents machine credential use for telemetry uploads.",
        },
        new TweakDef
        {
            Id = "diagtrk-disable-device-name-in-telemetry",
            Label = "Windows DiagTrack: Prevent Device Name from Being Sent in Telemetry",
            Category = "Windows DiagTrack Policy",
            Description =
                "Prevents Windows from including the device hostname in diagnostic telemetry data sent to Microsoft. "
                + "Device names can reveal organizational naming conventions, asset tag formats, and employee details. "
                + "With this policy set, telemetry reports use an anonymized device identifier instead of the human-readable hostname. "
                + "Removing this policy allows the device name to be included in telemetry.",
            Tags = ["telemetry", "diagtrack", "device-name", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowDeviceNameInTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDeviceNameInTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDeviceNameInTelemetry", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Excludes hostname from telemetry; prevents device naming convention disclosure.",
        },
        new TweakDef
        {
            Id = "diagtrk-limit-diagnostic-log-collection",
            Label = "Windows DiagTrack: Limit Diagnostic Log Collection",
            Category = "Windows DiagTrack Policy",
            Description =
                "Limits the amount of diagnostic log data that the Connected User Experiences and Telemetry service can collect. "
                + "Unrestricted log collection can gather large volumes of user activity and application usage data. "
                + "Limiting collection reduces the diagnostic data footprint while still allowing critical security telemetry. "
                + "Removing this policy restores unrestricted diagnostic log collection by DiagTrack.",
            Tags = ["telemetry", "diagtrack", "log-collection", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LimitDiagnosticLogCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LimitDiagnosticLogCollection")],
            DetectOps = [RegOp.CheckDword(Key, "LimitDiagnosticLogCollection", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Limits diagnostic log volume; reduces telemetry data footprint on managed endpoints.",
        },
        new TweakDef
        {
            Id = "diagtrk-disable-dump-collection",
            Label = "Windows DiagTrack: Disable Crash Dump Collection for Telemetry",
            Category = "Windows DiagTrack Policy",
            Description =
                "Prevents the DiagTrack service from collecting and uploading process crash dumps as part of diagnostic telemetry. "
                + "Crash dumps can contain sensitive memory contents, user data, and application secrets captured at the moment of a crash. "
                + "Disabling dump collection ensures no memory contents are transmitted to Microsoft telemetry endpoints. "
                + "Removing this policy allows DiagTrack to include crash dumps in telemetry uploads.",
            Tags = ["telemetry", "diagtrack", "crash-dump", "privacy", "memory", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DoNotShowFeedbackNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotShowFeedbackNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotShowFeedbackNotifications", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suppresses feedback collection notifications; reduces DiagTrack memory upload.",
        },
        new TweakDef
        {
            Id = "diagtrk-limit-dump-collection",
            Label = "Windows DiagTrack: Limit Dump Collection Level",
            Category = "Windows DiagTrack Policy",
            Description =
                "Limits the diagnostic dump collection level to minimize the amount of memory captured during crash events used for telemetry. "
                + "Windows can collect kernel dumps, mini dumps, or full memory dumps for telemetry reporting. "
                + "Setting a lower collection level reduces sensitive data exposure while still enabling crash analysis. "
                + "Removing this policy reverts to the default dump collection level for telemetry.",
            Tags = ["telemetry", "diagtrack", "dump", "memory", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LimitDumpCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LimitDumpCollection")],
            DetectOps = [RegOp.CheckDword(Key, "LimitDumpCollection", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Limits memory dump level for telemetry; reduces sensitive data in crash reports.",
        },
        new TweakDef
        {
            Id = "diagtrk-disable-one-settings-downloads",
            Label = "Windows DiagTrack: Disable OneSettings Policy Downloads",
            Category = "Windows DiagTrack Policy",
            Description =
                "Prevents Windows from downloading configuration and feature flag updates via the OneSettings service used by DiagTrack. "
                + "OneSettings allows Microsoft to remotely change Windows behavior, feature availability, and telemetry configurations. "
                + "Blocking OneSettings downloads ensures that remote policy changes cannot override locally set configurations on managed endpoints. "
                + "Removing this policy allows OneSettings to download and apply remote configuration changes.",
            Tags = ["telemetry", "diagtrack", "onesettings", "remote-config", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOneSettingsDownloads", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOneSettingsDownloads")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOneSettingsDownloads", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks remote Windows configuration changes via OneSettings; preserves local policy integrity.",
        },
        new TweakDef
        {
            Id = "diagtrk-disable-cloud-clipboard-integration",
            Label = "Windows DiagTrack: Disable Cloud Clipboard Telemetry",
            Category = "Windows DiagTrack Policy",
            Description =
                "Prevents the DiagTrack service from collecting clipboard content samples as part of cloud clipboard telemetry. "
                + "When Cloud Clipboard is enabled, diagnostic telemetry may include usage metadata that could indirectly expose clipboard patterns. "
                + "This policy disables the cloud clipboard data collection path within DiagTrack. "
                + "Removing this policy restores Cloud Clipboard telemetry collection within DiagTrack.",
            Tags = ["telemetry", "diagtrack", "clipboard", "cloud", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(Key, "AllowClipboardHistory", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks clipboard telemetry via DiagTrack; prevents clipboard usage patterns from being collected.",
        },
    ];
}
