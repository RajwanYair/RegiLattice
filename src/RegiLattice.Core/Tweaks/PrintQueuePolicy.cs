// RegiLattice.Core — Tweaks/PrintQueuePolicy.cs
// Print Queue Policy — Sprint 562.
// Configures Group Policy for Windows print queue management: spooler
// security, printer enumeration, RPC endpoint binding, and printer
// driver installation controls.
// Category: "Print Queue Policy" | Slug: prtq
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers
//           HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\RPC

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintQueuePolicy
{
    private const string PrtKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    private const string RpcKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\RPC";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "prtq-disable-spooler-on-non-print-servers",
                Label = "Print Queue: Disable Print Spooler Service on Non-Print Servers",
                Category = "Print Queue Policy",
                Description =
                    "Sets DisableSpooler=1 in Printers policy. Disables the Print Spooler service on machines that are not designated print servers. The Print Spooler service has been the subject of critical vulnerabilities including PrintNightmare (CVE-2021-34527) and SpoolFool (CVE-2022-22718). Every machine running the spooler is a potential target. Domain controllers, application servers, and most workstations do not need to act as print servers. Disabling the spooler on these machines eliminates the entire attack surface — the only cost is that users cannot share their local printers with other network users from that machine.",
                Tags = ["print-spooler", "printnightmare", "cve-2021-34527", "security", "attack-surface"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "The Print Spooler service is disabled. Users cannot print from this machine unless it connects to a remote print server via the Remote Procedure Call path. Local printer installation is blocked. Apply to servers only — do not apply on workstations if users print locally.",
                ApplyOps = [RegOp.SetDword(PrtKey, "DisableSpooler", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableSpooler")],
                DetectOps = [RegOp.CheckDword(PrtKey, "DisableSpooler", 1)],
            },
            new TweakDef
            {
                Id = "prtq-restrict-driver-installation-to-admins",
                Label = "Print Queue: Restrict Printer Driver Installation to Administrators",
                Category = "Print Queue Policy",
                Description =
                    "Sets RestrictDriverInstallationToAdministrators=1 in Printers policy. Prevents standard (non-administrator) users from installing printer drivers. The PrintNightmare vulnerability chain exploited the ability of standard users to install printer drivers via the Windows Point and Print mechanism — using driver installation as a code execution vector to escalate privileges to SYSTEM. Restricting driver installation to administrators ensures that only IT-approved, tested drivers are deployed and closes the user-mode attack path for printer driver exploitation.",
                Tags = ["printer-driver", "printnightmare", "privilege-escalation", "security", "point-and-print"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Only administrators can install printer drivers. Point and Print from a remote print server requires admin rights if the server does not have a matching driver locally. IT must pre-stage approved drivers or use enterprise driver deployment tools.",
                ApplyOps = [RegOp.SetDword(PrtKey, "RestrictDriverInstallationToAdministrators", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "RestrictDriverInstallationToAdministrators")],
                DetectOps = [RegOp.CheckDword(PrtKey, "RestrictDriverInstallationToAdministrators", 1)],
            },
            new TweakDef
            {
                Id = "prtq-require-rpc-authentication",
                Label = "Print Queue: Require RPC Authentication for Printer Client Connections",
                Category = "Print Queue Policy",
                Description =
                    "Sets RpcUseNamedPipeProtocol=1 in Printers/RPC policy. Requires authenticated named pipe (rather than anonymous TCP) for RPC connections to print servers. Unauthenticated or weakly-authenticated RPC endpoints allow attackers to send RPC calls to printers without valid credentials — exploitable by several PrintNightmare-era attack chains. By requiring authenticated named pipe transport, each print spooler RPC call is associated with a verified security principal, enabling access control and audit logging of all print server interactions.",
                Tags = ["print-rpc", "authentication", "named-pipe", "security", "printnightmare"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Print server RPC connections use authenticated named pipes only. Domain-joined clients connecting with valid Kerberos/NTLM credentials are unaffected. Non-domain clients or applications using anonymous RPC to print servers may fail to connect.",
                ApplyOps = [RegOp.SetDword(RpcKey, "RpcUseNamedPipeProtocol", 1)],
                RemoveOps = [RegOp.DeleteValue(RpcKey, "RpcUseNamedPipeProtocol")],
                DetectOps = [RegOp.CheckDword(RpcKey, "RpcUseNamedPipeProtocol", 1)],
            },
            new TweakDef
            {
                Id = "prtq-disable-internet-printing",
                Label = "Print Queue: Disable Internet Printing Protocol (IPP) Client",
                Category = "Print Queue Policy",
                Description =
                    "Sets DisableHTTPPrinting=1 in Printers policy. Disables the Internet Printing Protocol (IPP) client component that allows printing to HTTP/HTTPS-hosted print servers. IPP printing was designed for consumer environments and internet-hosted printers. In enterprise environments, all printing should go through internal print servers using SMB/named pipe transport with Kerberos authentication. IPP printing bypasses enterprise print audit controls and can send documents to external internet printers if a user knows the IPP URL. Disabling IPP ensures all print traffic is channelled through monitored, authenticated print servers.",
                Tags = ["ipp", "internet-printing", "http-printing", "security", "data-exfiltration"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "IPP (HTTP/HTTPS) printing is disabled. Users cannot add printers using http:// or https:// printer URLs. Direct TCP/IP printing to IP printers via the standard TCP/IP port monitor (LPR/RAW) is unaffected. AirPrint may be affected.",
                ApplyOps = [RegOp.SetDword(PrtKey, "DisableHTTPPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableHTTPPrinting")],
                DetectOps = [RegOp.CheckDword(PrtKey, "DisableHTTPPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtq-restrict-point-and-print",
                Label = "Print Queue: Restrict Point and Print to Approved Print Servers",
                Category = "Print Queue Policy",
                Description =
                    "Sets NoWarningNoElevationOnInstall=0 and UpdatePromptSettings=0 in Printers policy. Configures Point and Print policy to warn users and require elevated privileges for both driver installation and driver updates from non-approved print servers. NoWarningNoElevationOnInstall=0 ensures that attempts to install printer drivers from unapproved servers prompt for admin credentials. UpdatePromptSettings=0 ensures driver updates from arbitrary servers also require elevation. This is the core Point and Print hardening — Microsoft's own mitigations for CVE-2021-36958.",
                Tags = ["point-and-print", "driver-install", "cve-2021-36958", "elevation", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Point and Print driver installs and updates from servers not in the approved list require elevation. Users attempting to add printers from unlisted servers see an elevation prompt. Approved print server names must be listed in the TrustedServers policy value.",
                ApplyOps =
                [
                    RegOp.SetDword(PrtKey, "NoWarningNoElevationOnInstall", 0),
                    RegOp.SetDword(PrtKey, "UpdatePromptSettings", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(PrtKey, "NoWarningNoElevationOnInstall"),
                    RegOp.DeleteValue(PrtKey, "UpdatePromptSettings"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(PrtKey, "NoWarningNoElevationOnInstall", 0),
                    RegOp.CheckDword(PrtKey, "UpdatePromptSettings", 0),
                ],
            },
            new TweakDef
            {
                Id = "prtq-disable-web-based-printing",
                Label = "Print Queue: Disable Web-Based Printer Queue Management",
                Category = "Print Queue Policy",
                Description =
                    "Sets DisableWebBasedPrinting=1 in Printers policy. Disables the Internet Information Services (IIS)-based web print queue management interface that allows users to manage print jobs via a browser on port 80. The web-based print management component requires IIS and opens an additional HTTP listener. In enterprise environments, print queue management is performed by IT via the Print Management MMC snap-in. Exposing a web interface for print queue management on domain print servers creates an unnecessary attack surface on the internal network.",
                Tags = ["print", "web-printing", "iis", "attack-surface", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "The IIS-based web print queue manager is disabled. Users cannot manage print jobs via the http://servername/printers web portal. Standard client-side print job management via the taskbar or Print Management console is unaffected.",
                ApplyOps = [RegOp.SetDword(PrtKey, "DisableWebBasedPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableWebBasedPrinting")],
                DetectOps = [RegOp.CheckDword(PrtKey, "DisableWebBasedPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtq-enable-spooler-event-logging",
                Label = "Print Queue: Enable Print Spooler Event Logging",
                Category = "Print Queue Policy",
                Description =
                    "Sets EnableEventLogging=1 in Printers policy. Enables detailed event logging in the Microsoft-Windows-PrintService/Operational event channel. Print spooler events record: job submitted, job printed, job failed, driver installed, printer added, printer deleted. Without this logging, detecting abuse of the print spooler (lateral movement, privilege escalation attempts, sensitive document printing) is impossible. The operational log is disabled by default to reduce log volume — enabling it on high-value machines (DCs, app servers, HR workstations) provides a forensic trail.",
                Tags = ["print-spooler", "event-log", "audit", "monitoring", "forensics"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print spooler operational events are logged. Minor disk overhead for log writes. Events include job processing, driver activity, and printer configuration changes. Useful for DLP monitoring on machines with access to sensitive documents.",
                ApplyOps = [RegOp.SetDword(PrtKey, "EnableEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "EnableEventLogging")],
                DetectOps = [RegOp.CheckDword(PrtKey, "EnableEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "prtq-disable-auto-download-of-drivers",
                Label = "Print Queue: Disable Automatic Download of Printer Drivers from Windows Update",
                Category = "Print Queue Policy",
                Description =
                    "Sets DisableWindowsUpdateDriverSearching=1 in Printers policy. Prevents the Print Spooler from automatically downloading and installing printer drivers from Windows Update when a new printer is detected. Automatic driver downloads from Windows Update bypass the enterprise software approval process: the driver may not be tested in the organisation's environment, may contain outdated firmware, or might be a supply-chain compromised update. Enterprise environments should pre-stage approved drivers in driver stores and deploy them via Group Policy or Intune.",
                Tags = ["printer-driver", "windows-update", "auto-download", "approval", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Printer drivers are not auto-downloaded from Windows Update. When a new printer is detected without a local driver, users see 'Driver not found' rather than automatic download. IT must pre-stage or push approved drivers.",
                ApplyOps = [RegOp.SetDword(PrtKey, "DisableWindowsUpdateDriverSearching", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableWindowsUpdateDriverSearching")],
                DetectOps = [RegOp.CheckDword(PrtKey, "DisableWindowsUpdateDriverSearching", 1)],
            },
            new TweakDef
            {
                Id = "prtq-require-package-aware-drivers",
                Label = "Print Queue: Require Package-Aware Printer Driver Architecture",
                Category = "Print Queue Policy",
                Description =
                    "Sets PackagePointAndPrintOnly=1 in Printers policy. Requires that Point and Print operations only install printer drivers that are packaged as Windows printer driver packages (not legacy kernel-mode drivers). Package-aware drivers use a sandboxed installation process that does not require kernel-mode code execution during driver install. Legacy v3 kernel-mode printer drivers run in the same trust context as the spooler (SYSTEM) — which is why PrintNightmare's kernel driver DLL injection worked. Package-aware (v4) drivers run in a lower-privilege isolated host.",
                Tags = ["printer-driver", "v4-driver", "package-aware", "kernel-mode", "printnightmare"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Only v4 package-aware printer drivers are accepted via Point and Print. Legacy v3 kernel-mode printer drivers are rejected. Some older printers only have v3 drivers — they require alternative connection methods (Type 3 class driver, IPP, etc.).",
                ApplyOps = [RegOp.SetDword(PrtKey, "PackagePointAndPrintOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "PackagePointAndPrintOnly")],
                DetectOps = [RegOp.CheckDword(PrtKey, "PackagePointAndPrintOnly", 1)],
            },
            new TweakDef
            {
                Id = "prtq-enable-lpd-service-logging",
                Label = "Print Queue: Enable Line Printer Daemon Service Audit Logging",
                Category = "Print Queue Policy",
                Description =
                    "Sets EnableLpdLogging=1 in Printers policy. Enables audit logging for the Line Printer Daemon (LPD) service when it is installed. LPD is the Unix/Linux print protocol listener (TCP port 515) that allows Unix-style lpr/lpq clients to submit print jobs to Windows print servers. LPD lacks authentication and is disabled by default on Windows Server, but legacy environments that enable it for Unix/Linux compatibility should maintain an audit log of all LPD print submissions. The log provides the source IP, user name, and document name for every LPD print job.",
                Tags = ["lpd", "lpr", "print-audit", "unix-print", "logging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "LPD service print job events are logged if the LPD service is installed and running. No impact if LPD is not installed. LPD service is disabled by default.",
                ApplyOps = [RegOp.SetDword(PrtKey, "EnableLpdLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "EnableLpdLogging")],
                DetectOps = [RegOp.CheckDword(PrtKey, "EnableLpdLogging", 1)],
            },
        ];
}
