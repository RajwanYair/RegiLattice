namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ScheduledTasks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "schtask-task-disable-ceip",
            Label = "Disable Customer Experience Improvement Program",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows CEIP data collection task. Stops sending usage data to Microsoft. Default: enabled. Recommended: 0 (disabled).",
            Tags = ["tasks", "ceip", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-appcompat",
            Label = "Disable Application Compatibility Assistant",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Application Compatibility Engine, AIT agent, and Program Compatibility Assistant. Saves CPU on older PCs. Default: enabled. Recommended: disabled.",
            Tags = ["tasks", "appcompat", "performance", "pca"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-dmwappush",
            Label = "Disable WAP Push Service (dmwappushsvc)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the WAP Push Message Routing Service used by telemetry for device management messages. Default: 3 (manual). Recommended: 4 (disabled).",
            Tags = ["tasks", "wappush", "telemetry", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice", "Start", 4)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-maps-update",
            Label = "Disable Offline Maps Auto-Update",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic download and update of offline maps data. Saves bandwidth and storage. Default: enabled. Recommended: 0 (disabled).",
            Tags = ["tasks", "maps", "bandwidth", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AllowUntriggeredNetworkTrafficOnSettingsPage", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AllowUntriggeredNetworkTrafficOnSettingsPage"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData", 0)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostics Data Collection",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the disk diagnostic data collector scheduled task. Default: enabled. Recommended: 0 (disabled).",
            Tags = ["tasks", "disk", "diagnostics", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-compat-appraiser",
            Label = "Disable Compatibility Appraiser",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Compatibility Appraiser that collects program telemetry. Reduces CPU and disk usage. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "compatibility", "appraiser", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-ceip",
            Label = "Disable CEIP Data Collection (Policy)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Customer Experience Improvement Program data collection via policy. Stops CEIP telemetry scheduled tasks. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "ceip", "telemetry", "policy"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient",
            ],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-app-experience",
            Label = "Disable Application Experience (PCA)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Application Experience and Program Compatibility Assistant tasks. Reduces background CPU usage from compatibility checks. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "app-experience", "pca", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-disk-diag",
            Label = "Disable Disk Diagnostics Data Collector",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Disk Diagnostics data collector scheduled task. Stops disk telemetry reporting to Microsoft. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "disk", "diagnostics", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{29689E29-2CE9-4751-B4FC-8EFF5066E3FD}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-disable-mrt-update",
            Label = "Disable MRT Automatic Update via WU",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Malicious Software Removal Tool from being offered through Windows Update Automatic Updates. Default: Offered. Recommended: Blocked for controlled environments.",
            Tags = ["tasks", "mrt", "malware", "update", "wu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-smartscreen",
            Label = "Disable SmartScreen Background Updates",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows SmartScreen via policy, stopping background filter data updates. Reduces network calls for reputation checking. Default: Enabled. Recommended: Disabled.",
            Tags = ["tasks", "smartscreen", "filter", "update", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-office-telemetry-task",
            Label = "Disable Office Telemetry Scheduled Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the OfficeTelemetryAgentLogOn and OfficeTelemetryAgentFallBack tasks via registry. Default: enabled.",
            Tags = ["scheduled-tasks", "office", "telemetry", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\OSM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\OSM", "EnableLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\OSM", "EnableLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\OSM", "EnableLogging", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-customer-experience",
            Label = "Disable Customer Experience Improvement Program Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Customer Experience Improvement Program data collection. Default: enabled.",
            Tags = ["scheduled-tasks", "ceip", "telemetry", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-application-experience",
            Label = "Disable Application Experience Compatibility Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Application Compatibility Appraiser that collects app telemetry. Default: enabled.",
            Tags = ["scheduled-tasks", "compatibility", "appraiser", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostic Scheduled Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the DiskDiagnosticDataCollector that sends disk health data to Microsoft. Default: enabled.",
            Tags = ["scheduled-tasks", "disk", "diagnostics", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}",
                    "ScenarioExecutionEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDI\{C29A23D7-7A0F-4C75-8A44-60A5E8AB81A0}",
                    "ScenarioExecutionEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-disable-maps-update",
            Label = "Disable Offline Maps Update Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic downloading and updating of offline map data. Default: enabled.",
            Tags = ["scheduled-tasks", "maps", "offline", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps", "AutoDownloadAndUpdateMapData", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-ngen-log",
            Label = "Disable .NET Framework NGen Log Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the .NET Framework NGen log compilation task that precompiles .NET assemblies in the background. Saves CPU cycles. Default: enabled.",
            Tags = ["scheduled-tasks", "ngen", "dotnet", "performance"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\.NET Framework\.NET Framework NGEN v4.0.30319",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-disable-power-diagnostics",
            Label = "Disable Power Efficiency Diagnostics",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Power Efficiency Diagnostics task that analyses power consumption. Reduces background CPU and I/O. Default: enabled.",
            Tags = ["scheduled-tasks", "power", "diagnostics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics", "EnabledExecution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics", "EnabledExecution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\ScheduledDiagnostics", "EnabledExecution", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-speech-download",
            Label = "Disable Speech Model Download Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic downloading of speech model updates. Prevents background network and disk usage from speech data updates. Default: enabled.",
            Tags = ["scheduled-tasks", "speech", "download", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech", "AllowSpeechModelUpdate", 0)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-diagtrack-autologger",
            Label = "Disable DiagTrack AutoLogger Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DiagTrack AutoLogger ETW session that collects telemetry data in the background. Reduces CPU and I/O overhead. Default: enabled.",
            Tags = ["scheduled-tasks", "diagtrack", "autologger", "telemetry"],
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
            Id = "schtask-task-disable-diagtrack-service",
            Label = "Disable Connected User Experiences Service",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DiagTrack (Connected User Experiences and Telemetry) service. Stops telemetry data collection at the service level. Default: automatic.",
            Tags = ["scheduled-tasks", "diagtrack", "telemetry", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", 4)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-maintenance",
            Label = "Disable Automatic Maintenance",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic maintenance that runs defrag, updates, and security scans during idle time. Prevents surprise disk activity. Default: enabled.",
            Tags = ["scheduled-tasks", "maintenance", "automatic", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "MaintenanceDisabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-maintenance-wakeup",
            Label = "Disable Maintenance Wake Timer",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents automatic maintenance from waking the computer from sleep. Stops surprise middle-of-night wake events. Default: enabled.",
            Tags = ["scheduled-tasks", "maintenance", "wake", "sleep"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-scheduled-defrag",
            Label = "Disable Scheduled Disk Defrag",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the scheduled disk defragmentation task. Recommended for SSD systems where defrag is unnecessary and wears the drive. Default: enabled.",
            Tags = ["scheduled-tasks", "defrag", "disk", "ssd"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "Y")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Dfrg\BootOptimizeFunction", "Enable", "N")],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-scheduled-diagnostics",
            Label = "Disable Scheduled Diagnostics Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows scheduled diagnostics task that analyses system performance and reports issues. Reduces background activity. Default: enabled.",
            Tags = ["scheduled-tasks", "diagnostics", "performance", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "DisableQueryRemoteServer",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "DisableQueryRemoteServer"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy",
                    "DisableQueryRemoteServer",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "schtask-task-disable-wer",
            Label = "Disable Windows Error Reporting Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Error Reporting queue-processing task. Prevents crash data from being collected and sent to Microsoft. Default: enabled.",
            Tags = ["scheduled-tasks", "wer", "error-reporting", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
    ];
}
