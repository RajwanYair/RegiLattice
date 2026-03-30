// RegiLattice.Core — Tweaks/PrinterAdvanced.cs
// Advanced printer security and network printing policy controls.
// Slug: "prnta" — complements Printing.cs (printing-) without duplicating its IDs.
// Focus: WSD, IPP over HTTPS, encrypted printing, audit logging, and printer driver policy.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrinterAdvanced
{
    private const string WsdPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD";
    private const string PrintPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string SpoolerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Workflow";
    private const string IppPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string AuditPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prnta-disable-wsd-printer-discovery",
            Label = "Disable WSD (Web Services on Devices) Printer Discovery",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "network", "discovery", "security"],
            Description =
                "Disables the WSD (Web Services on Devices) port monitor, preventing Windows from auto-discovering "
                + "network printers via SOAP/WSD. Reduces broadcast network noise and eliminates a legacy protocol attack surface.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableWSDPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableWSDPrinting")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableWSDPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-require-https-ipp-printing",
            Label = "Require HTTPS for Internet Printing Protocol (IPP)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "ipp", "https", "security", "tls", "network"],
            Description =
                "Forces Windows to only accept IPP (Internet Printing Protocol) connections over HTTPS (port 443/631 TLS). "
                + "Prevents cleartext print job data from being transmitted over the network.",
            ApplyOps = [RegOp.SetDword(PrintPol, "ForceIPPSsl", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "ForceIPPSsl")],
            DetectOps = [RegOp.CheckDword(PrintPol, "ForceIPPSsl", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-print-workflow-service",
            Label = "Disable Print Workflow App Integration",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "workflow", "uwp", "policy", "attack surface"],
            Description =
                "Prevents third-party UWP applications from registering as print workflow apps. "
                + "Eliminates the risk of a malicious workflow app intercepting or modifying print jobs.",
            ApplyOps = [RegOp.SetDword(SpoolerPol, "DisablePrintWorkflow", 1)],
            RemoveOps = [RegOp.DeleteValue(SpoolerPol, "DisablePrintWorkflow")],
            DetectOps = [RegOp.CheckDword(SpoolerPol, "DisablePrintWorkflow", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-ports",
            Label = "Disable WSD Port Monitor Installation",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "port monitor", "security", "lateral movement"],
            Description =
                "Blocks installation of the WSD port monitor via Group Policy. "
                + "WSD ports have been used in lateral-movement scenarios; disabling the monitor prevents auto-creation.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD", "DisableWSDPortMonitor", 1)],
        },
        new TweakDef
        {
            Id = "prnta-enable-spooler-audit",
            Label = "Enable Print Spooler Service Audit Events",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "audit", "spooler", "logging", "security"],
            Description =
                "Enables detailed audit logging for the Print Spooler service in the Windows event log. "
                + "Required to detect PrintNightmare-style exploitation attempts and unauthorized driver installs.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler", "AuditSpoolerEvents", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-package-point-and-print",
            Label = "Disable Package Point-and-Print Non-Admin Install",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "security", "package point and print", "driver", "policy"],
            Description =
                "Restricts Package Point-and-Print to prevent non-admin users from installing packaged print drivers. "
                + "Closes a known elevation vector where a malicious print server could install arbitrary kernel drivers.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PackagePointAndPrint",
                    "PackagePointAndPrintOnly",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "prnta-restrict-printer-connection-unsigned",
            Label = "Disallow Connecting to Servers with Unsigned Printer Drivers",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "security", "unsigned", "driver", "policy"],
            Description =
                "Prevents Windows from connecting to a print server if the server's driver package is unsigned. "
                + "Ensures all automatically installed print drivers pass Windows Signature Enforcement (WHQL).",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisallowUserPrinterInstall", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-banner-page",
            Label = "Disable Printer Banner/Separator Page",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "paper", "waste", "eco", "banner page"],
            Description =
                "Removes the separator cover page that some print servers insert before each print job. "
                + "Eliminates wasted paper and toner on shared printers in small-office environments.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print", "DisableBannerPage", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-lpr-port",
            Label = "Disable LPR Port Monitor (Legacy Unix Printing)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "lpr", "legacy", "security", "port monitor"],
            Description =
                "Disables the legacy LPR/LPD (Line Printer Remote) port monitor via Group Policy. "
                + "LPR is an unencrypted 1980s printing protocol; disabling it eliminates a legacy attack surface.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers", "DisableLPRPort", 1)],
        },
        new TweakDef
        {
            Id = "prnta-require-https-spooler",
            Label = "Require HTTPS for Print Spooler Remote Connections",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "spooler", "https", "tls", "security"],
            Description =
                "Enforces TLS-encrypted HTTPS for all inbound remote print spooler "
                + "connections, blocking unencrypted (HTTP) print job submissions "
                + "across the network. Requires the spooler to present a valid certificate.",
            ApplyOps = [RegOp.SetDword(PrintPol, "EnableTLSForHTTPSPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "EnableTLSForHTTPSPrinting")],
            DetectOps = [RegOp.CheckDword(PrintPol, "EnableTLSForHTTPSPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-restrict-driver-install-admin",
            Label = "Restrict Print Driver Installation to Administrators",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "driver", "admin", "security", "privilege escalation"],
            Description =
                "Prevents standard users from installing print drivers. "
                + "Unvetted print drivers run in kernel space and are a documented "
                + "privilege-escalation vector (PrintNightmare/CVE-2021-34527 family).",
            ApplyOps = [RegOp.SetDword(PrintPol, "RestrictDriverInstallationToAdministrators", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "RestrictDriverInstallationToAdministrators")],
            DetectOps = [RegOp.CheckDword(PrintPol, "RestrictDriverInstallationToAdministrators", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-cloud-print",
            Label = "Disable Microsoft Cloud Print (Print to Cloud)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "cloud print", "microsoft", "privacy"],
            Description =
                "Blocks the Microsoft Cloud Print service (formerly Mopria) from "
                + "enumerating and uploading spool data to Microsoft cloud endpoints. "
                + "Keeps all print jobs local.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisablePrinterCloudPrint", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisablePrinterCloudPrint")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisablePrinterCloudPrint", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-multicast-discovery",
            Label = "Disable WSD Multicast Printer Discovery",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "discovery", "multicast", "network"],
            Description =
                "Prevents the Web Services on Devices (WSD) listener from responding "
                + "to multicast discovery probes on the local subnet. "
                + "Reduces unsolicited network chatter and removes WSD as an attack surface.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableDiscovery")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-win32-spool-com",
            Label = "Disable Windows 32-Bit Spooler COM Object",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "spooler", "com", "security", "legacy"],
            Description =
                "Disables the legacy 32-bit in-process COM spooler extensions. "
                + "These extensions can be abused to load arbitrary DLLs into the print spooler process "
                + "under SYSTEM context — a known persistence vector.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableWebPnpDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableWebPnpDownload")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableWebPnpDownload", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-rpc-over-http-spooler",
            Label = "Disable RPC-over-HTTP for Spooler (Restrict to Named Pipes)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "rpc", "http", "spooler", "network", "security"],
            Description =
                "Restricts inbound RPC connections to the print spooler to named "
                + "pipes only, blocking RPC-over-HTTP transport. Reduces remote "
                + "exploit surface for CVE-2021-1675 / PrintNightmare variants.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-internet-print-client",
            Label = "Disable Internet Printing Client",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "internet printing", "ipp", "feature", "security"],
            Description =
                "Disables the Windows Internet Printing Client component "
                + "(connects to HTTP/HTTPS printers by URL). Closes an infrequently "
                + "used remote printing feature that can be abused for SSRF and "
                + "credential-relay attacks.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableHTTPPrintingClient", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableHTTPPrintingClient")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableHTTPPrintingClient", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-wsd-printer-announce",
            Label = "Disable WSD Printer Announce (Host Advertising)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "wsd", "announcement", "network", "privacy"],
            Description =
                "Stops Windows from broadcasting WSD printer-announcement packets "
                + "on the network (Hello/Bye messages). Prevents other hosts on "
                + "the subnet from seeing shared printers.",
            ApplyOps = [RegOp.SetDword(WsdPol, "DisableAnnouncement", 1)],
            RemoveOps = [RegOp.DeleteValue(WsdPol, "DisableAnnouncement")],
            DetectOps = [RegOp.CheckDword(WsdPol, "DisableAnnouncement", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-driver-update-from-wu",
            Label = "Block Automatic Print Driver Updates via Windows Update",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["printing", "driver", "windows update", "wu", "policy"],
            Description =
                "Prevents Windows Update from automatically pushing print driver "
                + "updates. Automatic driver installs have been weaponised by "
                + "PrintNightmare-class vulnerabilities; use WSUS or manual approval instead.",
            ApplyOps = [RegOp.SetDword(PrintPol, "DisableAutoInstallDriverViaPnP", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableAutoInstallDriverViaPnP")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableAutoInstallDriverViaPnP", 1)],
        },
        new TweakDef
        {
            Id = "prnta-disable-inbound-print-spooler",
            Label = "Disable Inbound Remote Print Connections (Spooler Server)",
            Category = "Printing",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["printing", "spooler", "remote", "network", "security"],
            Description =
                "Blocks inbound remote connections to the Windows print spooler "
                + "service. Workstations should not accept remote print jobs; "
                + "this policy closes the highest-impact PrintNightmare attack path "
                + "without disabling the spooler entirely (local printing still works).",
            ApplyOps = [RegOp.SetDword(PrintPol, "NoAddPrinter", 0), RegOp.SetDword(PrintPol, "DisableSpoolerRemote", 1)],
            RemoveOps = [RegOp.DeleteValue(PrintPol, "DisableSpoolerRemote"), RegOp.DeleteValue(PrintPol, "NoAddPrinter")],
            DetectOps = [RegOp.CheckDword(PrintPol, "DisableSpoolerRemote", 1)],
        },
    ];
}
