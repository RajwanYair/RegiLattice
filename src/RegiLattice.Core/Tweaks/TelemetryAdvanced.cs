namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// === Merged from: TelemetryAdvanced.cs ===

internal static class TelemetryAdvanced
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "telem-disable-handwriting",
            Label = "Disable Handwriting Data Sharing",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents handwriting recognition data and error reports from being sent to Microsoft. Default: allowed. Recommended: 1 (blocked).",
            Tags = ["telemetry", "handwriting", "privacy", "tablet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingErrorReports", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingErrorReports"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from using diagnostic data to provide personalized tips, ads, and recommendations. Default: allowed. Recommended: 1 (disabled).",
            Tags = ["telemetry", "tailored", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-sqm-upload",
            Label = "Disable SQM Telemetry Upload",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Software Quality Metrics (SQM) CEIPEnable key, preventing telemetry data from being uploaded to Microsoft. Default: Enabled. Recommended: Disabled.",
            Tags = ["telemetry", "sqm", "ceip", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-speech-model-update",
            Label = "Disable Speech Model Automatic Update",
            Category = "Privacy",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically downloading updated speech recognition models. Reduces background network activity. Default: Enabled. Recommended: Disabled.",
            Tags = ["telemetry", "speech", "model", "update", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech", "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-license-telemetry",
            Label = "Disable License Telemetry (NoGenTicket)",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets NoGenTicket to prevent the Software Protection Platform from sending licensing telemetry to Microsoft. Default: Disabled. Recommended: Enabled for privacy.",
            Tags = ["telemetry", "license", "spp", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform", "NoGenTicket", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform", "NoGenTicket")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SoftwareProtectionPlatform", "NoGenTicket", 1)],
        },
        new TweakDef
        {
            Id = "telem-telemetry-set-max-size",
            Label = "Set Telemetry Cache Max Size to 0 MB",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the maximum diagnostic data cache size to 0 MB. Prevents telemetry data from accumulating on disk. Default: 1024 MB.",
            Tags = ["telemetry", "cache", "size", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "MaxTelemetryCacheSize", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "MaxTelemetryCacheSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "MaxTelemetryCacheSize", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-device-metadata-lookup",
            Label = "Disable Device Metadata Service URL Lookups",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableDeviceMetadataServiceUrlAccess=1 in DataCollection. Prevents Windows from contacting Microsoft's Device Metadata Service to retrieve driver cosmetic information, eliminating outbound telemetry-adjacent HTTP requests.",
            Tags = ["telemetry", "device", "metadata", "network", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDeviceMetadataServiceUrlAccess", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDeviceMetadataServiceUrlAccess"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "DisableDeviceMetadataServiceUrlAccess",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-inking-telemetry",
            Label = "Disable Inking and Typing Privacy Telemetry",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableInkingAndTypingPrivacy=1 in DataCollection policy. Prevents Windows from uploading inking and typing samples used to improve handwriting recognition and autocorrect models.",
            Tags = ["telemetry", "inking", "typing", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableInkingAndTypingPrivacy", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableInkingAndTypingPrivacy"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableInkingAndTypingPrivacy", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-edge-data-opt-in",
            Label = "Opt Out of Microsoft Edge Telemetry Data",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MicrosoftEdgeDataOptIn=0 in DataCollection policy. Prevents the legacy Edge engine from contributing usage and diagnostic data to Microsoft's browser analytics programme.",
            Tags = ["telemetry", "edge", "browser", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "MicrosoftEdgeDataOptIn", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "MicrosoftEdgeDataOptIn"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection",
                    "MicrosoftEdgeDataOptIn",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-map-auto-update",
            Label = "Disable Windows Maps Automatic Data Updates",
            Category = "Privacy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AutoUpdateEnabled=0 in HKLM\\SYSTEM\\Maps. Prevents Windows from silently downloading offline map data updates in the background.",
            Tags = ["telemetry", "maps", "auto-update", "network", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\Maps"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\Maps", "AutoUpdateEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\Maps", "AutoUpdateEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\Maps", "AutoUpdateEnabled", 0)],
        },
    ];
}
