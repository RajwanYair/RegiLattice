namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TelemetryAdvanced
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "telem-disable-diag-optin",
            Label = "Block Diagnostic Data Settings Changes",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the diagnostic data viewer and prevents users from changing opt-in level via Settings. Default: allowed. Recommended: 1 (blocked).",
            Tags = ["telemetry", "diagnostic", "settings", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDiagnosticDataViewer", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsSyncDiag", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDiagnosticDataViewer"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsSyncDiag"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableDiagnosticDataViewer", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-app-telemetry",
            Label = "Disable App Telemetry (Steps Recorder + Inventory)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Steps Recorder (UAR) and Application Inventory collection. Reduces background telemetry. Default: enabled. Recommended: disabled.",
            Tags = ["telemetry", "app", "steps-recorder", "inventory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-handwriting",
            Label = "Disable Handwriting Data Sharing",
            Category = "Telemetry Advanced",
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
            Id = "telem-disable-feedback",
            Label = "Disable Feedback Notifications",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets feedback frequency to 0 (never). Stops 'Rate Windows' and similar feedback prompts. Default: automatic. Recommended: 0 (never).",
            Tags = ["telemetry", "feedback", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-input-telemetry",
            Label = "Disable Typing/Inking Telemetry",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables collection of typing and inking data for improving language recognition. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["telemetry", "typing", "inking", "input", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Telemetry Advanced",
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
            Id = "telem-disable-inventory-collector",
            Label = "Disable Inventory Collector",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Inventory Collector that sends application/driver data to Microsoft. Default: Enabled. Recommended: Disabled.",
            Tags = ["telemetry", "inventory", "collector", "appcompat"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-sqm-upload",
            Label = "Disable SQM Telemetry Upload",
            Category = "Telemetry Advanced",
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
            Id = "telem-disable-mrt-report",
            Label = "Disable MRT Infection Reporting",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Malicious Software Removal Tool from reporting infection information to Microsoft. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["telemetry", "mrt", "malware", "reporting", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-speech-model-update",
            Label = "Disable Speech Model Automatic Update",
            Category = "Telemetry Advanced",
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
            Category = "Telemetry Advanced",
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
            Id = "telem-disable-ncsi-probing",
            Label = "Disable NCSI Active Probe",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Network Connectivity Status Indicator active probe that contacts Microsoft servers to check internet connectivity on login. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["telemetry", "ncsi", "probe", "network", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-customer-experience",
            Label = "Disable Customer Experience Improvement Program",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables CEIP data collection. Hardware/software usage data won't be sent to Microsoft. Default: enabled.",
            Tags = ["telemetry", "ceip", "data-collection", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-handwriting-data",
            Label = "Disable Handwriting Data Sharing",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables sending handwriting error reports and data to Microsoft. Default: enabled.",
            Tags = ["telemetry", "handwriting", "data", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingDataSharing", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-kms-client-emulation",
            Label = "Disable KMS Client Online AVS Validation",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic KMS client activation validation pings. Reduces outbound telemetry traffic. Default: enabled.",
            Tags = ["telemetry", "kms", "activation", "validation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform",
                    "NoGenTicket",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform",
                    "NoGenTicket"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform",
                    "NoGenTicket",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-advertising-id",
            Label = "Disable Advertising ID (Policy)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the per-user advertising ID via Group Policy. Prevents personalised ad tracking across apps. Default: enabled.",
            Tags = ["telemetry", "advertising-id", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo", "DisabledByGroupPolicy", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-diagtrack-autologger",
            Label = "Disable DiagTrack AutoLogger",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DiagTrack/Autologger-Diagtrack-Listener ETW session. Prevents early-boot telemetry logging. Default: enabled.",
            Tags = ["telemetry", "diagtrack", "autologger", "etw"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener", "Start", 0),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-win-error-reporting",
            Label = "Disable Windows Error Reporting (Policy)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Error Reporting via Group Policy. Prevents crash dumps and error data from being sent to Microsoft. Default: enabled.",
            Tags = ["telemetry", "wer", "error-reporting", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "telem-security-only",
            Label = "Set Telemetry to Security Only",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Windows telemetry to Security only (0 = Enterprise only) via Group Policy. Minimum data collection level. Default: Full (3).",
            Tags = ["telemetry", "security", "minimal", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "telem-telemetry-set-max-size",
            Label = "Set Telemetry Cache Max Size to 0 MB",
            Category = "Telemetry Advanced",
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
            Id = "telem-disable-ceip",
            Label = "Disable Customer Experience Improvement Programme (SQMClient)",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CEIPEnable=0 in the SQMClient Windows key. Disables the CEIP data-collection component associated with program quality telemetry uploads.",
            Tags = ["telemetry", "ceip", "sqm", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-one-settings-download",
            Label = "Disable OneSettings Telemetry Configuration Downloads",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableOneSettingsDownloads=1 in DataCollection policy. Prevents Windows from downloading dynamic telemetry configuration updates (\"OneSettings\") from Microsoft servers.",
            Tags = ["telemetry", "one-settings", "download", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsDownloads", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsDownloads")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "DisableOneSettingsDownloads", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-limit-diagnostic-log",
            Label = "Limit Diagnostic Log Collection to Minimum",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LimitDiagnosticLogCollection=1. Restricts the additional diagnostic log bundles gathered by Windows Feedback Hub and Windows Error Reporting to the minimum required.",
            Tags = ["telemetry", "diagnostic", "logs", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "LimitDiagnosticLogCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "LimitDiagnosticLogCollection")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "LimitDiagnosticLogCollection", 1),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-cloud-analytics-enhanced",
            Label = "Restrict Enhanced Diagnostic Data to Windows Analytics",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LimitEnhancedDiagnosticDataWindowsAnalytics=1. Even when Enhanced telemetry is enabled, only the subset required by Windows Analytics for update-health monitoring is uploaded.",
            Tags = ["telemetry", "enhanced", "analytics", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "LimitEnhancedDiagnosticDataWindowsAnalytics",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "LimitEnhancedDiagnosticDataWindowsAnalytics"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                    "LimitEnhancedDiagnosticDataWindowsAnalytics",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "telem-disable-device-metadata-lookup",
            Label = "Disable Device Metadata Service URL Lookups",
            Category = "Telemetry Advanced",
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
            Id = "telem-disable-language-list-share",
            Label = "Disable Language List Sharing with Websites",
            Category = "Telemetry Advanced",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets HttpAcceptLanguageOptOut=1 in the International User Profile key. Sends a generic Accept-Language header to websites instead of the user's locale list, preventing language-based fingerprinting.",
            Tags = ["telemetry", "language", "privacy", "fingerprinting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\International\User Profile"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 1)],
        },
        new TweakDef
        {
            Id = "telem-disable-insider-builds",
            Label = "Disable Windows Insider Preview Build Access",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowBuildPreview=0 via PreviewBuilds policy. Prevents the device from enrolling in Windows Insider Program flights, eliminating associated feedback and diagnostic data collection.",
            Tags = ["telemetry", "insider", "preview", "builds"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds", "AllowBuildPreview", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds", "AllowBuildPreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PreviewBuilds", "AllowBuildPreview", 0)],
        },
        new TweakDef
        {
            Id = "telem-disable-inking-telemetry",
            Label = "Disable Inking and Typing Privacy Telemetry",
            Category = "Telemetry Advanced",
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
            Category = "Telemetry Advanced",
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
            Category = "Telemetry Advanced",
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
        new TweakDef
        {
            Id = "telem-disable-oobe-privacy-wizard",
            Label = "Disable OOBE Privacy Experience Wizard",
            Category = "Telemetry Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePrivacyExperience=1 in the OOBE key. Prevents Windows from launching the privacy-settings wizard that prompts users to send diagnostic data after installation or upgrade.",
            Tags = ["telemetry", "oobe", "setup", "privacy", "wizard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE", "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE", "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OOBE", "DisablePrivacyExperience", 1)],
        },
    ];
}
