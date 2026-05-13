namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Sprint 649 — Reliability Monitor data collection and WER reporting policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting
///           HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Reliability
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting
/// Controls whether Reliability Monitor gathers crash data, uploads it,
/// and exposes it through the Windows Error Reporting UI.
/// </summary>
/// <summary>
/// Sprint 650 — DNS client security and multicast name resolution policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager\config
/// Controls LLMNR multicast, DNS-over-HTTPS enforcement, smart name resolution,
/// and related DNS/name resolution security policies.
/// </summary>
/// <summary>
/// Sprint 651 — Windows SmartScreen and application reputation enforcement (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System (EnableSmartScreen)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\WTDS\Components (Enhanced Phishing)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell (SmartScreen for scripts)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\Safer (Software Restriction)
/// Controls Windows SmartScreen filters, enhanced phishing protection, and
/// application reputation checks for downloaded files.
/// </summary>
[TweakModule]
internal static class PolicySmartScreenWin
{
    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string WtdsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WTDS\Components";
    private const string SaferKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers";
    private const string AppRepKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-capture",
            Label = "Enable Enhanced Phishing Protection — Capture Check",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CaptureThreatWindow=1 in WTDS Components policy. "
                + "Activates Enhanced Phishing Protection's threat capture mechanism, which screenshots and checks credential entry pages. "
                + "Detects credential harvesting phishing sites in real time, even when embedded in enterprise applications or documents.",
            Tags = ["smartscreen", "phishing", "wtds", "credential", "enhanced"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Captures phishing attempts at credential entry; slight performance overhead.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "CaptureThreatWindow", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "CaptureThreatWindow")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "CaptureThreatWindow", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-notify-malicious",
            Label = "Enhanced Phishing Protection — Notify on Malicious Site",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyMalicious=1 in WTDS Components policy. "
                + "Configures Enhanced Phishing Protection to display a warning notification when a user visits a detected phishing or malicious site. "
                + "Alerts users in real time rather than silently blocking traffic, allowing them to understand why access was interrupted.",
            Tags = ["smartscreen", "phishing", "notification", "wtds", "warning"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Shows phishing warnings in Windows; requires Microsoft Defender support.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyMalicious", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyMalicious")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyMalicious", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-notify-password-reuse",
            Label = "Enhanced Phishing Protection — Warn on Password Reuse",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyPasswordReuse=1 in WTDS Components policy. "
                + "Enables the Enhanced Phishing Protection warning that fires when a user types their Windows account password into a non-Windows credential form. "
                + "Detects password reuse attacks where users enter their corporate password on a personal or untrusted site.",
            Tags = ["smartscreen", "phishing", "password-reuse", "wtds", "credential"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Warns when Windows password is reused on other sites; zero performance impact.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyPasswordReuse", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyPasswordReuse")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyPasswordReuse", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-unsafe-app",
            Label = "Enhanced Phishing Protection — Warn on Unsafe App Password Entry",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyUnsafeApp=1 in WTDS Components policy. "
                + "Triggers Enhanced Phishing Protection warnings when the Windows account password is entered in an application flagged as potentially unsafe. "
                + "Extends phishing detection beyond browser sessions to desktop applications that prompt for credentials.",
            Tags = ["smartscreen", "phishing", "unsafe-app", "wtds", "desktop-app"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Monitors desktop app credential prompts for password misuse.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyUnsafeApp", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyUnsafeApp")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyUnsafeApp", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-safer-log-policy",
            Label = "Enable Software Restriction Policy Event Logging",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LogFileName=%WINDIR%\\system32\\spp.log in Software Restriction Policy code identifiers. "
                + "Enables SRP to write a detailed log of all application execution events with their restriction disposition to the specified log file. "
                + "Provides an audit trail for compliance and incident investigation.",
            Tags = ["srp", "software-restriction", "safer", "logging", "audit"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Logs all SRP execution decisions to a file for audit review.",
            ApplyOps = [RegOp.SetExpandString(SaferKey, "LogFileName", @"%WINDIR%\system32\spp.log")],
            RemoveOps = [RegOp.DeleteValue(SaferKey, "LogFileName")],
            DetectOps = [RegOp.CheckString(SaferKey, "LogFileName", @"%WINDIR%\system32\spp.log")],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-mrt-disable-auto-download",
            Label = "Disable Automatic MRT Download via Windows Update",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontOfferThroughWUAU=1 in MRT policy. "
                + "Prevents Windows Update from automatically downloading and running the Microsoft Malicious Software Removal Tool (MRT/MSRT). "
                + "In enterprise environments, MRT deployment should be managed through SCCM/Intune or WSUS rather than automatic Windows Update push, "
                + "to control scan timing and avoid unexpected CPU/disk load during business hours.",
            Tags = ["mrt", "msrt", "windows-update", "malware-removal", "enterprise"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents auto MRT push via WU; enterprise should deploy MRT through managed channels.",
            ApplyOps = [RegOp.SetDword(AppRepKey, "DontOfferThroughWUAU", 1)],
            RemoveOps = [RegOp.DeleteValue(AppRepKey, "DontOfferThroughWUAU")],
            DetectOps = [RegOp.CheckDword(AppRepKey, "DontOfferThroughWUAU", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-mrt-disable-infection-report",
            Label = "Disable MRT Infection Report Upload to Microsoft",
            Category = "Security — Firewall Misc",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontReportInfectionInformation=1 in MRT policy. "
                + "Prevents the Malicious Software Removal Tool from sending infection report telemetry to Microsoft after removing malware. "
                + "The infection report includes information about the malware found, the machine configuration, and the removal status. "
                + "In air-gapped or high-security environments, preventing this upload limits external data transmission.",
            Tags = ["mrt", "infection-report", "telemetry", "upload", "privacy", "air-gap"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops MRT infection report uploads; no impact on malware removal capability.",
            ApplyOps = [RegOp.SetDword(AppRepKey, "DontReportInfectionInformation", 1)],
            RemoveOps = [RegOp.DeleteValue(AppRepKey, "DontReportInfectionInformation")],
            DetectOps = [RegOp.CheckDword(AppRepKey, "DontReportInfectionInformation", 1)],
        },
    ];
}
