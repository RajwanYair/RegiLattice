// Diagnostic Data Viewer & Telemetry Compliance Policy — Sprint 146
// Slug "diagdvr" — controls Diagnostic Data Viewer enablement and enterprise
// telemetry compliance pipeline values in the Windows DataCollection policy key.
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection
// (distinct from AllowTelemetry, AllowDeviceNameInTelemetry — already in other modules)
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class DiagnosticDataViewerPolicy
{
    private const string DataCol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "diagdvr-disable-viewer",
            Label = "Telemetry: Disable the Diagnostic Data Viewer app",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets DisableDiagnosticDataViewer=1. Prevents end users from opening the Diagnostic "
                + "Data Viewer app to inspect telemetry sent to Microsoft, reducing data-disclosure risk.",
            Tags = ["telemetry", "diagnostic", "viewer", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "DisableDiagnosticDataViewer", 1)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "DisableDiagnosticDataViewer")],
            DetectOps = [RegOp.CheckDword(DataCol, "DisableDiagnosticDataViewer", 1)],
        },
        new TweakDef
        {
            Id = "diagdvr-disable-device-health-attestation",
            Label = "Telemetry: Disable Device Health Attestation service reporting",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets AllowDeviceHealthAttestationService=0. Prevents Windows from uploading "
                + "boot-state measurements to the Microsoft Device Health Attestation cloud service.",
            Tags = ["telemetry", "health-attestation", "tpm", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "AllowDeviceHealthAttestationService", 0)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "AllowDeviceHealthAttestationService")],
            DetectOps = [RegOp.CheckDword(DataCol, "AllowDeviceHealthAttestationService", 0)],
        },
        new TweakDef
        {
            Id = "diagdvr-limit-diagnostic-log-collection",
            Label = "Telemetry: Limit diagnostic log collection for Windows Update",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets LimitDiagnosticLogCollection=1. Restricts the volume of diagnostic logs "
                + "collected from the device and uploaded during Windows Update servicing operations.",
            Tags = ["telemetry", "diagnostic", "logs", "windows-update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "LimitDiagnosticLogCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "LimitDiagnosticLogCollection")],
            DetectOps = [RegOp.CheckDword(DataCol, "LimitDiagnosticLogCollection", 1)],
        },
        new TweakDef
        {
            Id = "diagdvr-disable-enterprise-auth-proxy",
            Label = "Telemetry: Disable enterprise auth-proxy for telemetry uploads",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets DisableEnterpriseAuthProxy=1. Prevents the Connected User Experiences service "
                + "from using Authenticated Proxy to send telemetry, forcing direct connection only.",
            Tags = ["telemetry", "proxy", "enterprise", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "DisableEnterpriseAuthProxy", 1)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "DisableEnterpriseAuthProxy")],
            DetectOps = [RegOp.CheckDword(DataCol, "DisableEnterpriseAuthProxy", 1)],
        },
        new TweakDef
        {
            Id = "diagdvr-disable-onesettings-auditing",
            Label = "Telemetry: Disable OneSettings diagnostic auditing",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets EnableOneSettingsAuditing=0. Prevents Windows from recording a local audit log "
                + "of each OneSettings configuration payload fetched from Microsoft cloud endpoints.",
            Tags = ["telemetry", "onesettings", "audit", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "EnableOneSettingsAuditing", 0)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "EnableOneSettingsAuditing")],
            DetectOps = [RegOp.CheckDword(DataCol, "EnableOneSettingsAuditing", 0)],
        },
        new TweakDef
        {
            Id = "diagdvr-disable-update-compliance-processing",
            Label = "Telemetry: Disable Update Compliance telemetry processing",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets AllowUpdateComplianceProcessing=0. Prevents the device from sending telemetry "
                + "to the Windows Update Compliance cloud analytics workspace.",
            Tags = ["telemetry", "update-compliance", "analytics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "AllowUpdateComplianceProcessing", 0)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "AllowUpdateComplianceProcessing")],
            DetectOps = [RegOp.CheckDword(DataCol, "AllowUpdateComplianceProcessing", 0)],
        },
        new TweakDef
        {
            Id = "diagdvr-disable-wufb-cloud-processing",
            Label = "Telemetry: Disable Windows Update for Business cloud processing",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets AllowWUfBCloudProcessing=0. Prevents the device from sending telemetry to the "
                + "Windows Update for Business cloud processing pipeline.",
            Tags = ["telemetry", "wufb", "cloud", "windows-update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "AllowWUfBCloudProcessing", 0)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "AllowWUfBCloudProcessing")],
            DetectOps = [RegOp.CheckDword(DataCol, "AllowWUfBCloudProcessing", 0)],
        },
        new TweakDef
        {
            Id = "diagdvr-disable-desktop-analytics",
            Label = "Telemetry: Disable Desktop Analytics/Endpoint Analytics telemetry",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets AllowDesktopAnalyticsProcessing=0. Stops the device from contributing "
                + "telemetry to Microsoft Desktop Analytics and Endpoint Analytics workloads.",
            Tags = ["telemetry", "desktop-analytics", "intune", "analytics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "AllowDesktopAnalyticsProcessing", 0)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "AllowDesktopAnalyticsProcessing")],
            DetectOps = [RegOp.CheckDword(DataCol, "AllowDesktopAnalyticsProcessing", 0)],
        },
        new TweakDef
        {
            Id = "diagdvr-disable-commercial-data-pipeline",
            Label = "Telemetry: Disable commercial data pipeline telemetry upload",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets AllowCommercialDataPipeline=0. Prevents Windows from routing diagnostic data "
                + "through the commercial telemetry pipeline used by enterprise monitoring solutions.",
            Tags = ["telemetry", "commercial", "pipeline", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "AllowCommercialDataPipeline", 0)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "AllowCommercialDataPipeline")],
            DetectOps = [RegOp.CheckDword(DataCol, "AllowCommercialDataPipeline", 0)],
        },
        new TweakDef
        {
            Id = "diagdvr-limit-enhanced-diagnostic-data",
            Label = "Telemetry: Limit enhanced diagnostic data for Windows Analytics",
            Category = "Diagnostic Data Viewer Policy",
            Description =
                "Sets LimitEnhancedDiagnosticDataWindowsAnalytics=0. When telemetry is set to Enhanced, "
                + "this policy further limits the enhanced-tier subset sent to Windows Analytics.",
            Tags = ["telemetry", "enhanced", "analytics", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(DataCol, "LimitEnhancedDiagnosticDataWindowsAnalytics", 0)],
            RemoveOps = [RegOp.DeleteValue(DataCol, "LimitEnhancedDiagnosticDataWindowsAnalytics")],
            DetectOps = [RegOp.CheckDword(DataCol, "LimitEnhancedDiagnosticDataWindowsAnalytics", 0)],
        },
    ];
}
