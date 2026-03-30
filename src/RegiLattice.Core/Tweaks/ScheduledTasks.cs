namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ScheduledTasks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
        new TweakDef
        {
            Id = "schtask-disable-defender-scheduled-scan",
            Label = "Disable Windows Defender Scheduled Full Scan (GPO)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Defender automatic full-disk scheduled scan via Group Policy. Reduces background CPU/IO spikes from periodic full scans. Real-time protection remains active. Default: scheduled scan enabled.",
            Tags = ["scheduled-tasks", "defender", "scan", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableScheduledScanning", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableScheduledScanning")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Scan", "DisableScheduledScanning", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-mrt-run",
            Label = "Block Malicious Removal Tool Execution",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Microsoft Windows Malicious Software Removal Tool (MRT) from running via Group Policy (DontRunMRT=1). Stops the monthly MRT execution that scans for specific malware. Default: MRT runs monthly.",
            Tags = ["scheduled-tasks", "mrt", "malware", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontRunMRT", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontRunMRT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontRunMRT", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-ngen-32",
            Label = "Disable .NET NGEN Pre-JIT Service (32-bit)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the 32-bit CLR Optimization Service (clr_optimization_v4.0.30319_32) which background-compiles .NET assemblies at idle. Can cause CPU spikes during gaming or presentations. Default: automatic.",
            Tags = ["scheduled-tasks", "ngen", "dotnet", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_32", "Start", 4)],
        },
        new TweakDef
        {
            Id = "schtask-disable-ngen-64",
            Label = "Disable .NET NGEN Pre-JIT Service (64-bit)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the 64-bit CLR Optimization Service (clr_optimization_v4.0.30319_64) which idle-compiles native images for .NET assemblies. Prevents background CPU spikes from the JIT optimization jobs. Default: automatic.",
            Tags = ["scheduled-tasks", "ngen", "dotnet", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\clr_optimization_v4.0.30319_64", "Start", 4)],
        },
        new TweakDef
        {
            Id = "schtask-disable-app-usage-record",
            Label = "Disable Application Usage Recording (AppCompat)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Application Compatibility user action recorder (DisableUserActionRecord=1) that logs which apps are run and when. Reduces AppCompat background telemetry data collection. Default: enabled.",
            Tags = ["scheduled-tasks", "compat", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUserActionRecord", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUserActionRecord")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUserActionRecord", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-inventory",
            Label = "Disable Application Compatibility Inventory Collection",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the scheduled AppCompat inventory collection task that catalogues installed applications. Reduces background CPU and disk usage from inventory scans. Default: enabled.",
            Tags = ["scheduled-tasks", "compat", "inventory", "telemetry"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-winsat-rating",
            Label = "Block Windows Experience Index (WinSAT) Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks the Windows System Assessment Tool (WinSAT) from running by setting BlockWinSAT=1. WinSAT benchmarks hardware for the Windows Experience Index score; disabling eliminates periodic benchmark runs. Default: enabled.",
            Tags = ["scheduled-tasks", "winsat", "benchmark", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat", "BlockWinSAT", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat", "BlockWinSAT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winsat", "BlockWinSAT", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-autoplay",
            Label = "Disable AutoPlay for All Media and Devices (GPO)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables AutoPlay for all media types and devices via Group Policy. Prevents automatic execution of code when USB drives, CDs, or other removable media are inserted. Default: enabled per-device.",
            Tags = ["scheduled-tasks", "autoplay", "usb", "security", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay", "DisableAutoplay", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay", "DisableAutoplay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AutoPlay", "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "schtask-disable-fault-tolerant-heap",
            Label = "Disable Fault Tolerant Heap (FTH)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Fault Tolerant Heap (FTH) service which monitors crashing applications and silently patches their heap allocations to prevent re-crashes. Removes memory overhead and startup interference from FTH monitoring. Default: enabled.",
            Tags = ["scheduled-tasks", "fth", "heap", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "schtask-disable-spotlight-features",
            Label = "Disable All Windows Spotlight Features (GPO)",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables all Windows Spotlight features globally via Group Policy. Prevents Spotlight from running background tasks to fetch and rotate AI-curated images, tips, and ads. Default: enabled for eligible Windows editions.",
            Tags = ["scheduled-tasks", "spotlight", "ai", "gpo", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightFeatures", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightFeatures"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightFeatures", 1),
            ],
        },
    ];
}

// ── Merged from ScheduledTaskTweaks.cs ──────────────────────────────────────────────────

internal static class ScheduledTaskTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pst-disable-customer-experience",
            Label = "Disable Customer Experience Improvement Program Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables CEIP data collection scheduled tasks (Consolidator, UsbCeip, KernelCeipTask).",
            Tags = ["scheduledtask", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\KernelCeipTask')) { "
                        + "Disable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip',"
                        + "'\\Microsoft\\Windows\\Customer Experience Improvement Program\\KernelCeipTask')) { "
                        + "Enable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 2;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-app-telemetry",
            Label = "Disable Application Telemetry Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Application Experience and Compatibility Appraiser data collection tasks.",
            Tags = ["scheduledtask", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser',"
                        + "'\\Microsoft\\Windows\\Application Experience\\ProgramDataUpdater',"
                        + "'\\Microsoft\\Windows\\Application Experience\\StartupAppTask')) { "
                        + "Disable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($t in @('\\Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser',"
                        + "'\\Microsoft\\Windows\\Application Experience\\ProgramDataUpdater',"
                        + "'\\Microsoft\\Windows\\Application Experience\\StartupAppTask')) { "
                        + "Enable-ScheduledTask -TaskName $t -ErrorAction SilentlyContinue | Out-Null }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 2;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-windows-maps-update",
            Label = "Disable Windows Maps Update Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the offline maps background update scheduled task.",
            Tags = ["scheduledtask", "network", "disk"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsUpdateTask' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsToastTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsUpdateTask' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -TaskName 'MapsToastTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Maps\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-feedback-hub",
            Label = "Disable Feedback Hub Scheduled Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Feedback Hub and SIUF data collection tasks.",
            Tags = ["scheduledtask", "telemetry", "privacy", "feedback"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClient' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClientOnScenarioDownload' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClient' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -TaskName 'DmClientOnScenarioDownload' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Feedback\\Siuf\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostic Data Collection",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the disk diagnostic data collector scheduled task.",
            Tags = ["scheduledtask", "telemetry", "disk", "diagnostics"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\DiskDiagnostic\\' -TaskName 'Microsoft-Windows-DiskDiagnosticDataCollector' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\DiskDiagnostic\\' -TaskName 'Microsoft-Windows-DiskDiagnosticDataCollector' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\DiskDiagnostic\\' -TaskName 'Microsoft-Windows-DiskDiagnosticDataCollector' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-office-telemetry",
            Label = "Disable Office Telemetry Scheduled Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Microsoft Office telemetry agent and dashboard tasks.",
            Tags = ["scheduledtask", "telemetry", "office", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Office\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'Telemetry' } | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Office\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'Telemetry' } | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Office\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'Telemetry' -and $_.State -eq 'Disabled' }).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-speech-model-update",
            Label = "Disable Speech Model Auto-Update Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the speech model download and update scheduled task.",
            Tags = ["scheduledtask", "speech", "network", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Speech\\' -TaskName 'SpeechModelDownloadTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Speech\\' -TaskName 'SpeechModelDownloadTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Speech\\' -TaskName 'SpeechModelDownloadTask' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-device-census",
            Label = "Disable Device Census Telemetry Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Device Census hardware inventory collection task.",
            Tags = ["scheduledtask", "telemetry", "privacy", "hardware"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -TaskName 'Device' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -TaskName 'Device' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -TaskName 'Device' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-handwriting-data",
            Label = "Disable Handwriting Data Sharing Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables handwriting recognition data collection and sharing scheduled tasks.",
            Tags = ["scheduledtask", "privacy", "handwriting", "telemetry"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TabletPC\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TabletPC\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TabletPC\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-cloud-experience",
            Label = "Disable Cloud Experience Host Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Cloud Experience Host tasks that handle Windows setup, OOBE, and Microsoft account prompts.",
            Tags = ["scheduledtask", "privacy", "cloud", "microsoft-account"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\CloudExperienceHost\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\CloudExperienceHost\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\CloudExperienceHost\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-diagnostic-data-controller",
            Label = "Disable Diagnostic Data Controller Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Windows diagnostic data upload controller tasks.",
            Tags = ["scheduledtask", "telemetry", "diagnostics", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\FeatureConfig\\' -TaskName 'ReconcileFeatures' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\OneSettings\\' -TaskName 'RefreshCache' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\FeatureConfig\\' -TaskName 'ReconcileFeatures' -ErrorAction SilentlyContinue | Out-Null;"
                        + "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\OneSettings\\' -TaskName 'RefreshCache' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Flighting\\*' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-power-efficiency",
            Label = "Disable Power Efficiency Diagnostics Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the power efficiency diagnostic report task that runs periodically.",
            Tags = ["scheduledtask", "performance", "power", "diagnostics"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Power Efficiency Diagnostics\\' -TaskName 'AnalyzeSystem' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Power Efficiency Diagnostics\\' -TaskName 'AnalyzeSystem' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Power Efficiency Diagnostics\\' -TaskName 'AnalyzeSystem' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-idle-maintenance",
            Label = "Disable Idle Maintenance Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables automatic idle maintenance that runs during system idle periods.",
            Tags = ["scheduledtask", "performance", "maintenance", "idle"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TaskScheduler\\' -ErrorAction SilentlyContinue "
                        + "| Where-Object { $_.TaskName -match 'Idle|Maintenance' } "
                        + "| Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TaskScheduler\\' -ErrorAction SilentlyContinue "
                        + "| Where-Object { $_.TaskName -match 'Idle|Maintenance' } "
                        + "| Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\TaskScheduler\\' -ErrorAction SilentlyContinue "
                        + "| Where-Object { $_.TaskName -match 'Idle|Maintenance' -and $_.State -eq 'Disabled' }).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-defrag-scheduled",
            Label = "Disable Scheduled Defragmentation",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the scheduled disk defragmentation task. Recommended for SSD-only systems where defrag is unnecessary.",
            Tags = ["scheduledtask", "performance", "defrag", "ssd", "disk"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-location-notification",
            Label = "Disable Location Notification Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the location notification background task.",
            Tags = ["scheduledtask", "privacy", "location", "notifications"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Location\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Location\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Location\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-windows-error-reporting",
            Label = "Disable Windows Error Reporting Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables WER queue and reporting scheduled tasks.",
            Tags = ["scheduledtask", "privacy", "error-reporting", "wer"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-family-safety",
            Label = "Disable Family Safety Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables Microsoft Family Safety monitoring tasks.",
            Tags = ["scheduledtask", "privacy", "family-safety", "parental"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyMonitor*' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null;"
                        + "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyRefresh*' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyMonitor*' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null;"
                        + "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\FamilySafetyRefresh*' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Shell\\' -ErrorAction SilentlyContinue | Where-Object { $_.TaskName -match 'FamilySafety' -and $_.State -eq 'Disabled' }).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-autochk-rebooter",
            Label = "Disable AutoChk Reboot Notification Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Autochk proxy scheduled task that notifies about pending chkdsk operations.",
            Tags = ["scheduledtask", "performance", "chkdsk", "notifications"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Autochk\\' -TaskName 'Proxy' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Autochk\\' -TaskName 'Proxy' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Autochk\\' -TaskName 'Proxy' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-license-validation",
            Label = "Disable License Validation Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Software Protection Platform license validation and rearm tasks.",
            Tags = ["scheduledtask", "privacy", "licensing", "activation"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\SoftwareProtectionPlatform\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\SoftwareProtectionPlatform\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\SoftwareProtectionPlatform\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-net-framework-ngen",
            Label = "Disable .NET Framework NGEN Tasks",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables background .NET native image generation tasks that consume CPU during idle periods.",
            Tags = ["scheduledtask", "performance", "dotnet", "ngen", "cpu"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\.NET Framework\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\.NET Framework\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\.NET Framework\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-compat-appraiser",
            Label = "Disable Compatibility Appraiser Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Microsoft Compatibility Appraiser task that gathers telemetry for upgrade compatibility.",
            Tags = ["scheduledtask", "privacy", "telemetry", "compatibility", "upgrade"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'Microsoft Compatibility Appraiser' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'Microsoft Compatibility Appraiser' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'Microsoft Compatibility Appraiser' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-rac-task",
            Label = "Disable Reliability Monitor (RAC) Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Reliability Monitor data-collection task that reports system stability errors.",
            Tags = ["scheduledtask", "privacy", "telemetry", "reliability", "rac"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RAC\\' -TaskName 'RacTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RAC\\' -TaskName 'RacTask' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RAC\\' -TaskName 'RacTask' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-program-compat-updater",
            Label = "Disable Program Compatibility Data Updater",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Application Experience ProgramDataUpdater task that uploads compatibility telemetry.",
            Tags = ["scheduledtask", "privacy", "telemetry", "compatibility", "appcompat"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'ProgramDataUpdater' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'ProgramDataUpdater' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Application Experience\\' -TaskName 'ProgramDataUpdater' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-wer-queue-reporting",
            Label = "Disable WER Queue Error Reporting Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Windows Error Reporting QueueReporting scheduled task that submits crash reports.",
            Tags = ["scheduledtask", "privacy", "telemetry", "wer", "crash"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -TaskName 'QueueReporting' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -TaskName 'QueueReporting' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Error Reporting\\' -TaskName 'QueueReporting' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-device-info-collector",
            Label = "Disable Device Information Collection Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Device Information tasks that gather hardware inventory for Microsoft telemetry.",
            Tags = ["scheduledtask", "privacy", "telemetry", "hardware", "inventory"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -ErrorAction SilentlyContinue | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -ErrorAction SilentlyContinue | Enable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Device Information\\' -ErrorAction SilentlyContinue | Where-Object State -eq 'Disabled').Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count >= 1;
            },
        },
        new TweakDef
        {
            Id = "pst-disable-smart-screen-app-id",
            Label = "Disable SmartScreen App-ID Background Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the SmartScreen credential lookup background task under AppID.",
            Tags = ["scheduledtask", "smartscreen", "privacy", "security", "appid"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\AppID\\' -TaskName 'SmartScreenSpecific' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\AppID\\' -TaskName 'SmartScreenSpecific' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\AppID\\' -TaskName 'SmartScreenSpecific' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-mrt-telemetry",
            Label = "Disable MRT Telemetry Heartbeat Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the MRT_HB heartbeat task that sends usage telemetry for the Malicious Software Removal Tool.",
            Tags = ["scheduledtask", "mrt", "telemetry", "privacy", "malware"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RemovalTools\\' -TaskName 'MRT_HB' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RemovalTools\\' -TaskName 'MRT_HB' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\RemovalTools\\' -TaskName 'MRT_HB' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-defender-cache-maintenance",
            Label = "Disable Defender Cache Maintenance Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the Windows Defender background cache maintenance scheduled task to reduce idle CPU usage.",
            Tags = ["scheduledtask", "defender", "cache", "performance", "cpu"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Defender\\' -TaskName 'Windows Defender Cache Maintenance' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Defender\\' -TaskName 'Windows Defender Cache Maintenance' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Windows Defender\\' -TaskName 'Windows Defender Cache Maintenance' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "pst-disable-usbceip",
            Label = "Disable USB CEIP Telemetry Task",
            Category = "Scheduled Tasks",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ScheduledTask,
            Description = "Disables the USB Customer Experience Improvement Program background data collection task.",
            Tags = ["scheduledtask", "usb", "ceip", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -TaskName 'UsbCeip' -ErrorAction SilentlyContinue | Out-Null"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -TaskName 'UsbCeip' -ErrorAction SilentlyContinue | Out-Null"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Customer Experience Improvement Program\\' -TaskName 'UsbCeip' -ErrorAction SilentlyContinue).State -eq 'Disabled'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}
