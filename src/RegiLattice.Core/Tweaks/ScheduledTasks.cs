namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from ScheduledTasks.cs ──
[TweakModule]
internal static class ScheduledTasks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "schtask-disable-disk-diagnostics",
            Label = "Disable Disk Diagnostic Scheduled Task",
            Category = "Maintenance 2",
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
            Category = "Maintenance 2",
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
            Category = "Maintenance 2",
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
            Id = "schtask-task-disable-diagtrack-service",
            Label = "Disable Connected User Experiences Service",
            Category = "Maintenance 2",
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
            Id = "schtask-task-disable-scheduled-diagnostics",
            Label = "Disable Scheduled Diagnostics Task",
            Category = "Maintenance 2",
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
            Id = "schtask-disable-defender-scheduled-scan",
            Label = "Disable Windows Defender Scheduled Full Scan (GPO)",
            Category = "Maintenance 2",
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
            Category = "Maintenance 2",
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
            Category = "Maintenance 2",
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
            Category = "Maintenance 2",
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
            Category = "Maintenance 2",
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
            Id = "schtask-disable-winsat-rating",
            Label = "Block Windows Experience Index (WinSAT) Task",
            Category = "Maintenance 2",
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
            Category = "Maintenance 2",
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
    ];
}
