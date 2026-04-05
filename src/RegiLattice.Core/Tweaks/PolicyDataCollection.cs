namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyDataCollection
{
    public static IReadOnlyList<TweakDef> Tweaks => [.. _DataCollectionPolicy.Data, .. _AppCompatPolicy.Data];

    // ── Sprint 669a — Data Collection (Telemetry) Advanced Policy ─────────────
    private static class _DataCollectionPolicy
    {
        private const string DcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "telem-policy-disable-device-census",
                    Label = "Disable Device Census Telemetry Task (Policy)",
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
                    Category = "Privacy — Location Sensors",
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
                    Id = "telem-policy-disable-ceip-reporting",
                    Label = "Disable Application Compatibility CEIP Reporting",
                    Category = "Privacy — Location Sensors",
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
