// RegiLattice.Core — Tweaks/DesktopAnalyticsPolicy.cs
// Sprint 347: Desktop Analytics Policy tweaks (10 tweaks)
// Category: "Desktop Analytics Policy" | Slug: dskanlyt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DesktopAnalyticsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dskanlyt-set-commercial-id",
            Label = "Configure Commercial ID for Desktop Analytics Data Collection",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The Commercial ID associates diagnostic data sent to Microsoft with a specific organization's Desktop Analytics workspace enabling analytics dashboards for update compliance. Configuring the Commercial ID is required to use Desktop Analytics for Windows update readiness assessments and compatibility analysis. Without a Commercial ID diagnostic data is anonymized and cannot be correlated with organizational devices for Desktop Analytics reporting. Organizations using Desktop Analytics for update risk assessment must deploy the Commercial ID policy to all managed devices. Commercial IDs are generated in the Azure Portal for Desktop Analytics workspaces and should be protected as organizational identifiers. Organizations that disable diagnostic data collection should clear the Commercial ID setting to prevent any residual correlation of device data.",
            Tags = ["desktop-analytics", "commercial-id", "diagnostic-data", "compliance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetString(Key, "CommercialId", "")],
            RemoveOps = [RegOp.DeleteValue(Key, "CommercialId")],
            DetectOps = [RegOp.CheckMissing(Key, "CommercialId")],
        },
        new TweakDef
        {
            Id = "dskanlyt-set-diagnostic-data-level",
            Label = "Set Diagnostic Data Collection Level to Security Only",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Diagnostic data levels control the amount of telemetry transmitted to Microsoft with Security being the minimal level and Full being the maximum. Setting diagnostic data to Security level (0) transmits only the minimum data required to keep Windows secure including Malicious Software Removal Tool data and Windows Defender security intelligence. Restricting diagnostic data to Security level minimizes the organizational data transmitted to Microsoft while still receiving security updates. Enterprise editions of Windows 10 and later support the Security (0) level which is not available on consumer editions. Organizations concerned about data privacy should configure Security level or at most Basic Enhanced level for managed systems. Desktop Analytics requires at least Enhanced diagnostic data level to provide full compatibility and update readiness intelligence.",
            Tags = ["desktop-analytics", "diagnostic-data", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "dskanlyt-disable-msdt-telemetry",
            Label = "Disable Microsoft Diagnostics and Troubleshooting Tool Telemetry",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Microsoft Diagnostics and Troubleshooting (MSDT) transmits debugging and crash information to Microsoft that can include sensitive data from system crash dumps or application error reports. Disabling MSDT telemetry prevents diagnostic data from system failures from being transmitted to Microsoft which may include memory contents from the time of the failure. Crash dumps can contain sensitive in-memory data including encryption keys login tokens and sensitive application data that should not be transmitted externally. Organizations should configure crash dump settings to capture the minimum data needed for internal debugging rather than sending full crash reports to Microsoft. MSDT telemetry disabling should be combined with WER (Windows Error Reporting) restrictions to provide comprehensive control over error data transmission. Systems running high-security workloads should minimize diagnostic data transmission through both Microsoft and third-party error reporting frameworks.",
            Tags = ["desktop-analytics", "msdt", "telemetry", "crash-data", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDiagnosticData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDiagnosticData")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDiagnosticData", 1)],
        },
        new TweakDef
        {
            Id = "dskanlyt-disable-data-in-device-health-reports",
            Label = "Disable Device Health Attestation Data Reporting to External Services",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Device Health Attestation reports security configuration data including boot state and security feature status to cloud services for compliance verification. Disabling external Device Health Attestation data reporting prevents health data from being transmitted to Microsoft cloud services for organizations that use internal attestation systems. Organizations running on-premises Device Health Attestation servers manage their own attestation data without requiring Microsoft cloud services for health reporting. Cloud-based Device Health Attestation provides valuable security enforcement for conditional access but requires transmitting device state to Microsoft. The choice between cloud and on-premises Device Health Attestation should align with the organization's data sovereignty and privacy requirements. Organizations deploying Microsoft Intune typically rely on cloud-based Device Health Attestation as part of conditional access policy enforcement.",
            Tags = ["desktop-analytics", "device-health", "attestation", "reporting", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LimitDiagnosticLogCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LimitDiagnosticLogCollection")],
            DetectOps = [RegOp.CheckDword(Key, "LimitDiagnosticLogCollection", 1)],
        },
        new TweakDef
        {
            Id = "dskanlyt-disable-inventory-collection",
            Label = "Disable Automatic Software Inventory Collection for Analytics",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Automatic software inventory collection transmits an inventory of installed applications and drivers to Microsoft's Desktop Analytics service for compatibility analysis. Disabling software inventory collection prevents application lists from being transmitted to Microsoft which reduces the data footprint in Microsoft's analytics cloud. Software inventory data may reveal internal application names versions and configurations that organizations consider sensitive or confidential. Organizations using Desktop Analytics for actual update readiness assessment need inventory data enabled to benefit from compatibility analysis. For organizations not using Desktop Analytics the inventory collection provides no business value and represents unnecessary data transmission. Inventory collection disabling should be applied to all systems outside the Analytics scope to minimize unnecessary data collection.",
            Tags = ["desktop-analytics", "inventory", "software-collection", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DoNotShowFeedbackNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DoNotShowFeedbackNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DoNotShowFeedbackNotifications", 1)],
        },
        new TweakDef
        {
            Id = "dskanlyt-disable-update-compliance-collection",
            Label = "Disable Update Compliance Data Collection for Non-Analytics Systems",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Update Compliance data collection transmits Windows Update and Windows Defender update status to Microsoft's Log Analytics service for organizational compliance reporting. Disabling Update Compliance collection for systems outside the analytics scope eliminates unnecessary transmission of update status data to Microsoft cloud services. Organizations using Update Compliance must maintain the Collection settings for enrolled devices while disabling it for systems out of scope for analytics. Update Compliance provides valuable data for identifying unpatched systems but requires cloud data transmission for analysis. On-premises alternatives to Update Compliance include WSUS reports and Configuration Manager update compliance reports that keep data internal. Organizations with strict data sovereignty requirements should use on-premises update compliance reporting instead of Microsoft's collected analytics.",
            Tags = ["desktop-analytics", "update-compliance", "cloud-reporting", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowDeviceNameInTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowDeviceNameInTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowDeviceNameInTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "dskanlyt-restrict-feedback-hub-telemetry",
            Label = "Restrict Feedback Hub from Submitting User Diagnostic Data",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Windows Feedback Hub allows users to submit feedback to Microsoft that can include diagnostic logs screenshots and system state information which requires careful control in enterprise environments. Restricting Feedback Hub telemetry prevents users from intentionally or inadvertently submitting sensitive system information through the feedback channel. Feedback submissions can include diagnostic logs from applications or system components that contain sensitive business data requiring restriction in regulated environments. Organizations should restrict Feedback Hub access for all managed systems while maintaining channels for product feedback through approved enterprise feedback mechanisms. Feedback Hub feedback settings should be part of the overall data classification and transmission policy for managed endpoints. Disabling Feedback Hub does not prevent Windows from collecting system telemetry through other channels which must be controlled separately.",
            Tags = ["desktop-analytics", "feedback-hub", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "dskanlyt-disable-compat-appraiser-task",
            Label = "Disable Compatibility Appraiser Scheduled Task for Analytics",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The Compatibility Appraiser scheduled task runs assessments that collect application and device compatibility data for Desktop Analytics and Windows upgrade readiness. Disabling the Compatibility Appraiser task prevents compatibility data from being collected and transmitted to Microsoft's analytics services. The Appraiser scan runs daily on systems enrolled in Desktop Analytics consuming CPU and I/O resources to assess installed applications and hardware compatibility. Organizations not using Desktop Analytics for upgrade planning have no need for the Appraiser task and should disable it to reduce unnecessary resource consumption and data transmission. Disabling the Appraiser task does not affect Windows Update delivery or security update installation. Organizations planning Windows version upgrades should enable the Compatibility Appraiser for a period before the upgrade to identify compatibility blockers.",
            Tags = ["desktop-analytics", "compatibility-appraiser", "scheduled-task", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCompatibilityAppraiser", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCompatibilityAppraiser")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCompatibilityAppraiser", 1)],
        },
        new TweakDef
        {
            Id = "dskanlyt-restrict-enhanced-diagnostic-data",
            Label = "Restrict Enhanced Diagnostic Data to Required Minimum Events",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enhanced diagnostic data level transmits additional optional events beyond the Basic level that provide richer analytics data but also represent greater data transmission and privacy exposure. Restricting Enhanced diagnostic data to the required events subset limits transmission to only events required for Desktop Analytics without sending all Enhanced level events. Windows 10 1803 introduced an Enhanced Required Events option that allows Desktop Analytics usage while minimizing miscellaneous Enhanced events. Organizations using Desktop Analytics who want to minimize data transmission should configure Enhanced Required Events rather than the full Enhanced level. The EnableOneSettingsAuditing and RequiredEventsPolicies settings control which specific Enhanced events are transmitted when Enhanced Required Events is configured. Regular review of configured diagnostic data policies through compliance monitoring ensures that settings remain aligned with the organization's data handling requirements.",
            Tags = ["desktop-analytics", "enhanced-diagnostics", "data-minimization", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LimitEnhancedDiagnosticDataWindowsAnalytics")],
            DetectOps = [RegOp.CheckDword(Key, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
        },
        new TweakDef
        {
            Id = "dskanlyt-audit-diagnostic-data-changes",
            Label = "Enable Audit Logging for Diagnostic Data Policy Configuration Changes",
            Category = "Desktop Analytics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Diagnostic data configuration audit logging captures changes to telemetry settings that could indicate attempts to circumvent data collection restrictions or silently increase diagnostic data levels. Enabling audit logging for diagnostic policy changes provides visibility into telemetry configuration modifications that may violate organizational privacy policies. Changes to AllowTelemetry and related policies should be rare in production environments and any unauthorized changes should trigger security investigation. Malware that attempts to re-enable telemetry collection after it has been disabled will generate audit events that can be detected through SIEM alerting. Diagnostic data policy auditing should be included in the baseline configuration monitoring for all managed systems. Correlation of diagnostic data policy changes with user logon events helps identify which accounts made changes for accountability.",
            Tags = ["desktop-analytics", "audit", "policy-monitoring", "telemetry", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableOneSettingsAuditing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableOneSettingsAuditing")],
            DetectOps = [RegOp.CheckDword(Key, "EnableOneSettingsAuditing", 1)],
        },
    ];
}
