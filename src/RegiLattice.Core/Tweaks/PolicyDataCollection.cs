namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// RegiLattice.Core — Tweaks/PolicyDataCollection.cs
// Advanced telemetry suppression and application compatibility CEIP Group Policy tweaks.
// Category: "Privacy"
// Sprints 669 (v6.11.0)

internal static class PolicyDataCollection
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _DataCollectionPolicy.Data,
            .. _AppCompatPolicy.Data,
        ];

    // ── Sprint 669a — Data Collection (Telemetry) Advanced Policy ─────────────
    private static class _DataCollectionPolicy
    {
        private const string DcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "telem-policy-limit-diag-data",
                    Label = "Limit Diagnostic Data to Required Level (Policy)",
                    Category = "Privacy",
                    Description =
                        "Sets the diagnostic data collection level to 1 (Required) via Group Policy. Required level sends only basic device health data. Values: 0=Security, 1=Required, 3=Optional. Default: 3 (Optional on Home). Recommended: 1 (Required) or 0 (Security edition only).",
                    Tags = ["telemetry", "diagnostic", "data-collection", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(DcKey, "AllowTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "AllowTelemetry")],
                    DetectOps = [RegOp.CheckDword(DcKey, "AllowTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-preview-builds",
                    Label = "Disable Insider Preview Build Enrollment (Policy)",
                    Category = "Privacy",
                    Description =
                        "Blocks enrollment in Windows Insider Preview builds via Group Policy. Prevents the device from downloading and installing pre-release Windows builds that include additional telemetry and diagnostics. Default: allowed. Recommended: disabled on production machines.",
                    Tags = ["telemetry", "insider", "preview", "update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(DcKey, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "LimitEnhancedDiagnosticDataWindowsAnalytics")],
                    DetectOps = [RegOp.CheckDword(DcKey, "LimitEnhancedDiagnosticDataWindowsAnalytics", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-device-census",
                    Label = "Disable Device Census Telemetry Task (Policy)",
                    Category = "Privacy",
                    Description =
                        "Disables the Device Census scheduled task via Group Policy, which collects detailed hardware inventory, installed software, and system configuration data for Microsoft analytics. Default: enabled. Recommended: disabled.",
                    Tags = ["telemetry", "census", "inventory", "policy", "diagnostic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(DcKey, "DisableDeviceCensus", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "DisableDeviceCensus")],
                    DetectOps = [RegOp.CheckDword(DcKey, "DisableDeviceCensus", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-onedrive-sync-telemetry",
                    Label = "Disable OneDrive Diagnostic Telemetry (Policy)",
                    Category = "Privacy",
                    Description =
                        "Disables OneDrive-specific diagnostic telemetry via the DataCollection Group Policy key. Prevents OneDrive from contributing sync diagnostic and usage pattern data to Microsoft telemetry pipelines. Default: enabled. Recommended: disabled.",
                    Tags = ["telemetry", "onedrive", "diagnostic", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(DcKey, "DisableOneDriveSyncDiagnostics", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "DisableOneDriveSyncDiagnostics")],
                    DetectOps = [RegOp.CheckDword(DcKey, "DisableOneDriveSyncDiagnostics", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-compat-appraiser",
                    Label = "Disable Windows Compatibility Appraiser Telemetry (Policy)",
                    Category = "Privacy",
                    Description =
                        "Disables the Windows Compatibility Appraiser data collection that feeds the Microsoft CEIP upgrade readiness pipeline. The Appraiser scans installed software and drivers to assess Windows upgrade compatibility. Default: enabled. Recommended: disabled on stable systems.",
                    Tags = ["telemetry", "appraiser", "compatibility", "ceip", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(DcKey, "DisableCompatibilityAppraiser", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "DisableCompatibilityAppraiser")],
                    DetectOps = [RegOp.CheckDword(DcKey, "DisableCompatibilityAppraiser", 1)],
                },
            ];
    }

    // ── Sprint 669b — Application Compatibility CEIP Policy ───────────────────
    private static class _AppCompatPolicy
    {
        private const string CompatKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "telem-policy-disable-uac-detection",
                    Label = "Disable Application Compatibility UAC Mitigation Detection",
                    Category = "Privacy",
                    Description =
                        "Disables the Application Compatibility layer from running UAC compatibility mitigations on applications. Reduces compatibility engine overhead and telemetry data about legacy application UAC patterns. Default: enabled. Recommended: disabled on modern application environments.",
                    Tags = ["appcompat", "uac", "compatibility", "policy", "telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisableUACompleteAutomation", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisableUACompleteAutomation")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisableUACompleteAutomation", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-pca",
                    Label = "Disable Program Compatibility Assistant Monitoring",
                    Category = "Privacy",
                    Description =
                        "Disables the Program Compatibility Assistant (PCA) that monitors application crashes and automatically suggests compatibility settings. PCA sends crash and sentinel event telemetry to Microsoft. Default: PCA enabled. Recommended: disabled on stable environments.",
                    Tags = ["appcompat", "pca", "crash", "monitoring", "policy", "telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisablePCA", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisablePCA")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisablePCA", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-engine",
                    Label = "Disable Application Compatibility Engine",
                    Category = "Privacy",
                    Description =
                        "Disables the Windows Application Compatibility Engine at policy level. The engine intercepts API calls on each program launch to check compatibility shim databases, adds overhead to every process start, and contributes usage telemetry. Default: enabled. Recommended: disabled on systems running only modern Win32/.NET applications.",
                    Tags = ["appcompat", "engine", "performance", "policy", "telemetry"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisableEngine", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisableEngine")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisableEngine", 1)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-switch-back",
                    Label = "Disable Application Compatibility Switchback",
                    Category = "Privacy",
                    Description =
                        "Disables the Switchback compatibility subsystem. Switchback is an API shim layer that intercepts Win32 API calls for compatibility with applications that incorrectly use deprecated API behaviour. Disabling reduces process start overhead and associated compatibility telemetry. Default: enabled. Recommended: disabled.",
                    Tags = ["appcompat", "switchback", "api", "shim", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ApplyOps = [RegOp.SetDword(CompatKey, "SbEnable", 0)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "SbEnable")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "SbEnable", 0)],
                },
                new TweakDef
                {
                    Id = "telem-policy-disable-ceip-reporting",
                    Label = "Disable Application Compatibility CEIP Reporting",
                    Category = "Privacy",
                    Description =
                        "Disables the Customer Experience Improvement Program (CEIP) data collection within the Application Compatibility subsystem. Stops the engine from uploading crash and compatibility sentinel events to Microsoft telemetry servers. Default: CEIP data uploaded. Recommended: disabled.",
                    Tags = ["appcompat", "ceip", "telemetry", "reporting", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisablePropPageShim", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisablePropPageShim")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisablePropPageShim", 1)],
                },
            ];
    }
}
