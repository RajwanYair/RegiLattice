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
    ];
}
