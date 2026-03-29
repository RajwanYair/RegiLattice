// RegiLattice.Core — Tweaks/WindowsUpdateScanPolicy.cs
// WSUS server configuration, scan cadence, and update source routing (Sprint 601).
// Category: "WU Scan Policy" | Slug: wuscan
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate + \AU subkey
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsUpdateScanPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
    private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wuscan-enable-wsus-server-mode",
            Label = "WU Scan: Route Update Scanning Through WSUS Server",
            Category = "WU Scan Policy",
            Description = "Sets UseWUServer=1 in WU AU policy. Configures the Windows Update client to scan against the WSUS server configured in WUServer, rather than the public Windows Update service. " +
                "This is the primary switch that activates WSUS-based update management. Without this flag set to 1, WUServer and WUStatusServer URL values are present in the registry but ignored by the WU client, which continues to scan against Microsoft's cloud endpoint.",
            Tags = ["windows-update", "wsus", "server", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Activates WSUS-sourced scanning; all updates sourced from and approved via internal WSUS server.",
            ApplyOps = [RegOp.SetDword(AuKey, "UseWUServer", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "UseWUServer")],
            DetectOps = [RegOp.CheckDword(AuKey, "UseWUServer", 1)],
        },
        new TweakDef
        {
            Id = "wuscan-set-wsus-scan-frequency-22hours",
            Label = "WU Scan: Set WSUS Detection Frequency to 22 Hours",
            Category = "WU Scan Policy",
            Description = "Sets DetectionFrequency=22 and DetectionFrequencyEnabled=1 in WU AU policy. Configures the WU client to scan for updates every 22 hours instead of the default random interval (17-22 hours). " +
                "A fixed 22-hour interval ensures predictable scan timing for environments where WSUS server load must be managed. Scan frequency should be set to complement WSUS synchronisation schedule so clients scan after the server has synced from Microsoft.",
            Tags = ["windows-update", "wsus", "scan", "frequency", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "22-hour fixed scan interval; predictable WSUS load distribution vs. default random timing.",
            ApplyOps = [RegOp.SetDword(AuKey, "DetectionFrequency", 22), RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "DetectionFrequency"), RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled")],
            DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequency", 22)],
        },
        new TweakDef
        {
            Id = "wuscan-enable-automatic-update-download-and-schedule",
            Label = "WU Scan: Set Auto-Update Mode to Download and Schedule Install",
            Category = "WU Scan Policy",
            Description = "Sets AUOptions=4 in WU AU policy. Configures the auto-update behaviour to automatically download approved updates and schedule their installation for a configured maintenance window. " +
                "AUOptions values: 2=Notify only, 3=Auto download + notify for install, 4=Auto download + schedule install, 5=Allow local admin to configure. Value 4 is standard for enterprise WSUS where deployments are scheduled to minimize business disruption.",
            Tags = ["windows-update", "auto-update", "download", "schedule", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Auto-download with scheduled install; standard WSUS mode for planned maintenance window deployments.",
            ApplyOps = [RegOp.SetDword(AuKey, "AUOptions", 4)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "AUOptions")],
            DetectOps = [RegOp.CheckDword(AuKey, "AUOptions", 4)],
        },
        new TweakDef
        {
            Id = "wuscan-set-scheduled-install-day-0-every-day",
            Label = "WU Scan: Set Scheduled Install Day to Every Day",
            Category = "WU Scan Policy",
            Description = "Sets ScheduledInstallDay=0 in WU AU policy. Configures Windows Update to install scheduled updates every day (rather than a specific day of the week). " +
                "Day=0 means daily; Day=1-7 means a specific day (1=Sunday through 7=Saturday). Combined with ScheduledInstallTime, daily installation ensures patches are applied within 24 hours of their scheduled maintenance window rather than waiting up to a week.",
            Tags = ["windows-update", "schedule", "install", "daily", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Daily scheduled install cadence; updates applied within 24h of availability rather than weekly batch.",
            ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallDay", 0)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallDay")],
            DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallDay", 0)],
        },
        new TweakDef
        {
            Id = "wuscan-set-scheduled-install-time-2am",
            Label = "WU Scan: Set Scheduled Install Time to 2:00 AM",
            Category = "WU Scan Policy",
            Description = "Sets ScheduledInstallTime=2 in WU AU policy. Schedules automatic update installations to occur at 2:00 AM local time. " +
                "2 AM is the classic maintenance window: after business hours, before early-morning workers arrive, outside of backup windows (typically 1–2 AM), and during a period when most machines are idle but still powered on. " +
                "This time balances update deployment speed with business disruption minimisation.",
            Tags = ["windows-update", "schedule", "install", "maintenance-window", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "2 AM scheduled installs; classic after-hours maintenance window that avoids business hours disruption.",
            ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallTime", 2)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallTime")],
            DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallTime", 2)],
        },
        new TweakDef
        {
            Id = "wuscan-enable-intranet-update-service-stats",
            Label = "WU Scan: Enable Intranet Update Statistics Reporting",
            Category = "WU Scan Policy",
            Description = "Sets UseWUServer=1 and IntranetServerInternetOptions=3 in WU AU policy. Configures the WU client to send update scan statistics (detection results, download progress, installation outcomes) to the WSUS status server rather than Microsoft. " +
                "This populates the WSUS server's reporting database, enabling IT administrators to view an accurate picture of update compliance across the enterprise from the WSUS console.",
            Tags = ["windows-update", "wsus", "reporting", "statistics", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Routes update scan stats to WSUS; populates compliance reports in WSUS console.",
            ApplyOps = [RegOp.SetDword(AuKey, "IntranetServerInternetOptions", 3)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "IntranetServerInternetOptions")],
            DetectOps = [RegOp.CheckDword(AuKey, "IntranetServerInternetOptions", 3)],
        },
        new TweakDef
        {
            Id = "wuscan-enable-automatic-minor-update-install",
            Label = "WU Scan: Enable Automatic Installation of Minor Updates",
            Category = "WU Scan Policy",
            Description = "Sets AutoInstallMinorUpdates=1 in WU AU policy. Allows Windows Update to automatically install minor (maintenance release) updates without user notification or interaction. " +
                "Minor updates are typically service definition updates, component metadata refreshes, and low-risk patches that carry essentially no regression risk. Auto-installing these keeps the system at the latest minor version baseline without requiring a scheduled maintenance window for trivial updates.",
            Tags = ["windows-update", "minor-updates", "auto-install", "baseline", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Auto-installs minor updates silently; keeps system at full baseline without scheduled window for low-risk patches.",
            ApplyOps = [RegOp.SetDword(AuKey, "AutoInstallMinorUpdates", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "AutoInstallMinorUpdates")],
            DetectOps = [RegOp.CheckDword(AuKey, "AutoInstallMinorUpdates", 1)],
        },
        new TweakDef
        {
            Id = "wuscan-enable-allow-mu-service-alongside-wu",
            Label = "WU Scan: Scan Microsoft Update Service Alongside Windows Update",
            Category = "WU Scan Policy",
            Description = "Sets AllowMUUpdateService=1 in WU AU policy. Opts the machine into the Microsoft Update (MU) service in addition to the base Windows Update service. " +
                "Microsoft Update delivers updates for Office, Visual Studio, .NET, SQL Server, and other Microsoft products alongside OS updates. Without this setting, only Windows OS updates are delivered by WU, while Office and other products update through their own channels, which may not honour the configured maintenance window.",
            Tags = ["windows-update", "microsoft-update", "office", "products", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enrolls in Microsoft Update alongside WU; Office and other MS products update in the same maintenance window.",
            ApplyOps = [RegOp.SetDword(AuKey, "AllowMUUpdateService", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "AllowMUUpdateService")],
            DetectOps = [RegOp.CheckDword(AuKey, "AllowMUUpdateService", 1)],
        },
        new TweakDef
        {
            Id = "wuscan-set-reboot-launch-timeout-5min",
            Label = "WU Scan: Set Post-Install Reboot Launch Timeout to 5 Minutes",
            Category = "WU Scan Policy",
            Description = "Sets RebootLaunchTimeout=5 and RebootLaunchTimeoutEnabled=1 in WU policy. After updates are installed during a scheduled maintenance window and a restart is required, Windows waits this many minutes before initiating the restart automatically. " +
                "5 minutes gives any background processes time to complete gracefully while keeping the restart within the maintenance window. Without a timeout, the restart may be postponed indefinitely if a user was actively logged in during the overnight window.",
            Tags = ["windows-update", "restart", "timeout", "maintenance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "5-minute post-install restart timeout; keeps restart within maintenance window while allowing graceful process shutdown.",
            ApplyOps = [RegOp.SetDword(Key, "RebootLaunchTimeout", 5), RegOp.SetDword(Key, "RebootLaunchTimeoutEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RebootLaunchTimeout"), RegOp.DeleteValue(Key, "RebootLaunchTimeoutEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "RebootLaunchTimeoutEnabled", 1)],
        },
        new TweakDef
        {
            Id = "wuscan-set-reboot-warning-timeout-30min",
            Label = "WU Scan: Set Pre-Restart Warning Timeout to 30 Minutes",
            Category = "WU Scan Policy",
            Description = "Sets RebootWarningTimeout=30 and RebootWarningTimeoutEnabled=1 in WU policy. Configures Windows to display a countdown restart warning 30 minutes before the scheduled restart. " +
                "30 minutes provides a comfortable window for users to save work and close applications before the restart. This setting complements ScheduleRestartWarning (hours-in-advance general notice) — the 30-minute warning is the final specific countdown before imminent restart.",
            Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "30-minute final restart countdown; gives users time to save before scheduled maintenance restart.",
            ApplyOps = [RegOp.SetDword(Key, "RebootWarningTimeout", 30), RegOp.SetDword(Key, "RebootWarningTimeoutEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RebootWarningTimeout"), RegOp.DeleteValue(Key, "RebootWarningTimeoutEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "RebootWarningTimeoutEnabled", 1)],
        },
    ];
}
