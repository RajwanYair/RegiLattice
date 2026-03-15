namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Tweaks that disable or modify Windows scheduled tasks.
/// These use PowerShell Disable-ScheduledTask / Enable-ScheduledTask cmdlets.
/// </summary>
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
    ];
}
