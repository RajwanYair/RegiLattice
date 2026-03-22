// RegiLattice.Core — Tweaks/PrintSpoolerSecurity.cs
// Print spooler security and hardening settings (Sprint 96).
// Slug "spool" — HKLM spooler hardening, CVE-2021-1675 (PrintNightmare) mitigations.
// Distinct from Printing.cs (general print settings) and PrinterAdvanced.cs (printer policies).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintSpoolerSecurity
{
    private const string Spooler = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler";

    private const string SpoolerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    private const string SpoolerPointAndPrint = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

    private const string PrintNightmare = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Management";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "spool-disable-spooler-service",
            Label = "Disable Print Spooler Service (Non-Print Servers/Workstations)",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["spooler", "print", "service", "disable", "security"],
            Description =
                "Disables the Print Spooler service (Start=4) on systems that never print. "
                + "Eliminates the entire PrintNightmare attack surface. "
                + "WARNING: all printing including PDF will stop working.",
            ApplyOps = [RegOp.SetDword(Spooler, "Start", 4)],
            RemoveOps = [RegOp.SetDword(Spooler, "Start", 2)],
            DetectOps = [RegOp.CheckDword(Spooler, "Start", 4)],
        },
        new TweakDef
        {
            Id = "spool-disable-spooler-remote-rpc",
            Label = "Disable Remote Print Spooler RPC (CVE-2021-1675 Mitigation)",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["spooler", "printnightmare", "rpc", "remote", "cve-2021-1675"],
            Description =
                "Disables remote access to the print spooler via RPC by setting "
                + "RegisterSpoolerRemoteRpcEndPoint=2. Mitigates PrintNightmare "
                + "(CVE-2021-1675 / CVE-2021-34527) without fully disabling printing. "
                + "Local print continues to work.",
            ApplyOps = [RegOp.SetDword(Spooler, "RegisterSpoolerRemoteRpcEndPoint", 2)],
            RemoveOps = [RegOp.DeleteValue(Spooler, "RegisterSpoolerRemoteRpcEndPoint")],
            DetectOps = [RegOp.CheckDword(Spooler, "RegisterSpoolerRemoteRpcEndPoint", 2)],
        },
        new TweakDef
        {
            Id = "spool-restrict-driver-install",
            Label = "Restrict Driver Installation via Point-and-Print",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["spooler", "point and print", "driver", "policy", "cve-2021-1675"],
            Description =
                "Requires admin elevation when installing printer drivers via Point and "
                + "Print (NoWarningNoElevationOnInstall=0). Prevents non-admin users from "
                + "silently installing potentially malicious printer drivers.",
            ApplyOps = [RegOp.SetDword(SpoolerPointAndPrint, "NoWarningNoElevationOnInstall", 0)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPointAndPrint, "NoWarningNoElevationOnInstall")],
            DetectOps = [RegOp.CheckDword(SpoolerPointAndPrint, "NoWarningNoElevationOnInstall", 0)],
        },
        new TweakDef
        {
            Id = "spool-restrict-update-without-elevation",
            Label = "Require Elevation to Update Printer Drivers",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["spooler", "driver update", "elevation", "security", "point and print"],
            Description =
                "Requires administrator elevation when updating an existing printer driver "
                + "via Point and Print. UpdatePromptSettings=0. Closes the second half "
                + "of the PrintNightmare driver update bypass.",
            ApplyOps = [RegOp.SetDword(SpoolerPointAndPrint, "UpdatePromptSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPointAndPrint, "UpdatePromptSettings")],
            DetectOps = [RegOp.CheckDword(SpoolerPointAndPrint, "UpdatePromptSettings", 0)],
        },
        new TweakDef
        {
            Id = "spool-restrict-point-and-print-servers",
            Label = "Restrict Point-and-Print to Approved Servers Only",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["spooler", "point and print", "servers", "restrict"],
            Description =
                "Enables server restriction so Point-and-Print driver installation is "
                + "only permitted from an administrator-approved list of print servers. "
                + "Restricted=1. Prevents driver downloads from arbitrary network shares.",
            ApplyOps = [RegOp.SetDword(SpoolerPointAndPrint, "Restricted", 1)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPointAndPrint, "Restricted")],
            DetectOps = [RegOp.CheckDword(SpoolerPointAndPrint, "Restricted", 1)],
        },
        new TweakDef
        {
            Id = "spool-disable-http-printing",
            Label = "Disable HTTP Printing (Internet Printing Protocol)",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["spooler", "http printing", "ipp", "disable"],
            Description =
                "Disables the Internet Printing Protocol (HTTP/IPP) client which allows "
                + "printing to URLs. DisableHTTPPrinting=1. Removes an infrequently "
                + "used network printing path that expands attack surface.",
            ApplyOps = [RegOp.SetDword(SpoolerPolicy, "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPolicy, "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(SpoolerPolicy, "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "spool-disable-web-based-printing",
            Label = "Disable Web-Based Printer Browsing",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["spooler", "web printing", "browser", "disable"],
            Description =
                "Disables the Web-based printing browser interface and printer discovery "
                + "via HTTP. DisableWebPnPDownload=1. Stops the spooler from downloading "
                + "printer drivers from websites.",
            ApplyOps = [RegOp.SetDword(SpoolerPolicy, "DisableWebPnPDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPolicy, "DisableWebPnPDownload")],
            DetectOps = [RegOp.CheckDword(SpoolerPolicy, "DisableWebPnPDownload", 1)],
        },
        new TweakDef
        {
            Id = "spool-disable-printer-driver-download",
            Label = "Disable Automatic Printer Driver Download from Windows Update",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["spooler", "windows update", "driver download", "security"],
            Description =
                "Prevents Windows from automatically downloading and installing printer "
                + "drivers from Windows Update when a new printer is detected. "
                + "ExcludeWUDriversInQualityUpdate=1. Ensures only manually approved drivers "
                + "are installed.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverInstall\Restrictions", "AllowUserDeviceClasses", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverInstall\Restrictions", "AllowUserDeviceClasses"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverInstall\Restrictions", "AllowUserDeviceClasses", 0),
            ],
        },
        new TweakDef
        {
            Id = "spool-disable-mxdw-pdf-writer",
            Label = "Disable Microsoft XPS Document Writer (MXDW) Printer",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["spooler", "xps", "mxdw", "printer driver", "cleanup"],
            Description =
                "Prevents the Microsoft XPS Document Writer virtual printer from being "
                + "added. The XPS format is largely superseded by PDF in Windows 10+. "
                + "Reduces the number of virtual printers and simplifies the print dialog.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\MXDW", "MXDWInstalled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\MXDW", "MXDWInstalled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\MXDW", "MXDWInstalled", 0)],
        },
        new TweakDef
        {
            Id = "spool-log-spooler-events",
            Label = "Enable Print Spooler Event Logging",
            Category = "Print Spooler Security",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["spooler", "event log", "audit", "logging"],
            Description =
                "Ensures the Print Spooler logs detailed events to the Windows Event Log. "
                + "EventLog=1. Enables forensic review of printer driver installations "
                + "and spooler anomalies for security incident response.",
            ApplyOps = [RegOp.SetDword(SpoolerPolicy, "EventLog", 1)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPolicy, "EventLog")],
            DetectOps = [RegOp.CheckDword(SpoolerPolicy, "EventLog", 1)],
        },
    ];
}
