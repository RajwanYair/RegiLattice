// RegiLattice.Core — Tweaks/PrinterGpoPolicy.cs
// Sprint 308: Printer Group Policy tweaks (10 tweaks)
// Category: "Printer GPO Policy" | Slug: prtgpo
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrinterGpoPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prtgpo-disable-print-spooler-sharing",
            Label = "Disable Printer Spooler Network Sharing",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "The Windows print spooler provides print job management services and can expose a network interface for accepting remote print jobs. Disabling print spooler network sharing prevents the endpoint from acting as a print server accessible over the network. Exposed print spooler interfaces have been the source of critical vulnerabilities including PrintNightmare that allowed remote code execution. Limiting print spooler network exposure reduces the attack surface associated with vulnerabilities in spooler-accessible interfaces. Endpoints that do not function as print servers have no legitimate need to expose print spooler network interfaces. This setting is critical on servers and endpoints where print server functionality is not required.",
            Tags = ["printing", "spooler", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWebPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWebPrinting")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWebPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-disable-internet-printing",
            Label = "Disable Internet Printing Protocol (IPP)",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Internet Printing Protocol allows print jobs to be submitted to printers over HTTP and the internet through a web-based printing interface. Disabling Internet Printing prevents corporate endpoints from submitting or receiving print jobs through internet-facing printing services. Documents sent to internet printers are transmitted over HTTP and may be intercepted or stored on third-party services. Internet Printing Protocol serves as an attack vector for accessing print interfaces accessible from the internet or through browser exploitation. Enterprise printing should use approved secured print infrastructure with proper access controls and audit logging. Disabling IPP does not affect standard local and network printing functionality.",
            Tags = ["printing", "ipp", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHTTPPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHTTPPrinting")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHTTPPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-block-driver-install",
            Label = "Block Unapproved Printer Driver Installation",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Printer drivers run in kernel or highly privileged host processes and have historically been vectors for privilege escalation and malware distribution. Blocking unapproved printer driver installation prevents standard users from installing driver packages that have not been vetted by IT. Restricting driver installation to administrator-approved packages reduces exposure to malicious printer drivers distributed through malvertising or social engineering. PrintNightmare and related vulnerabilities were exploited in part through the printer driver installation subsystem allowing untrusted code to load. Enterprise-approved printer drivers should be deployed through managed software distribution with appropriate testing and validation. Driver installation restrictions support defense in depth by requiring administrative approval for each driver added to the device.",
            Tags = ["printing", "drivers", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictDriverInstallationToAdministrators", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictDriverInstallationToAdministrators")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictDriverInstallationToAdministrators", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-disable-pointed-print-warnings",
            Label = "Enforce Point and Print Security Warnings",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Point and Print allows Windows to automatically download printer drivers from print servers when connecting to network printers. Security warnings are displayed when downloading printer drivers from servers that are not on the approved list to inform users of installation risk. Disabling Point and Print security warnings removes the user protection prompt that alerts about third-party driver installation. Removing security prompts allows silent installation of potentially malicious printer drivers from untrusted print servers. The PrintNightmare vulnerability exploited default Point and Print configurations to allow unauthenticated remote code execution. Maintaining security warnings is essential to prevent silent driver installation from untrusted sources.",
            Tags = ["printing", "point-and-print", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoWarningNoElevationOnInstall", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoWarningNoElevationOnInstall")],
            DetectOps = [RegOp.CheckDword(Key, "NoWarningNoElevationOnInstall", 0)],
        },
        new TweakDef
        {
            Id = "prtgpo-disable-v3-driver-priority",
            Label = "Disable V3 Printer Driver Package-Aware Priority",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "V3 printer drivers run in the print spooler process itself whereas V4 drivers use an isolated architecture with reduced privilege. Disabling V3 driver priority preference ensures that the endpoint does not preferentially install lower-isolation V3 drivers over the more secure V4 architecture. V3 drivers running in-process with the spooler were the primary exploitation opportunity in PrintNightmare class vulnerabilities. Limiting V3 driver deployment in favor of the isolated V4 architecture reduces the blast radius of printer driver compromise. Modern printers increasingly support V4 drivers providing equivalent printing functionality with improved security isolation. Enforcing V4 driver preference is part of a defense-in-depth approach to printer subsystem hardening.",
            Tags = ["printing", "drivers", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PackagePointAndPrintOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PackagePointAndPrintOnly")],
            DetectOps = [RegOp.CheckDword(Key, "PackagePointAndPrintOnly", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-restrict-print-server-list",
            Label = "Restrict Point and Print to Approved Servers",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Point and Print can be configured to only allow automatic printer driver downloads from a list of enterprise-approved print servers. Restricting Point and Print to approved servers prevents driver installation from unapproved or internet-sourced print server endpoints. Attackers can create rogue print servers and cause endpoints to connect to them and install malicious drivers. An allowlist of trusted internal print servers ensures that driver installation only occurs from IT-managed infrastructure. Combined with driver signing requirements this control creates a complete validation chain for printer driver provenance. Enterprise print server lists should be managed through Group Policy and updated when print infrastructure changes.",
            Tags = ["printing", "point-and-print", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ServerList", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ServerList")],
            DetectOps = [RegOp.CheckDword(Key, "ServerList", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-disable-print-discovery",
            Label = "Disable Network Printer Discovery",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "Network printer discovery uses WSD and other protocols to automatically detect and display available printers on the local network. Disabling automatic network printer discovery prevents endpoints from finding and attempting to connect to arbitrary network-accessible printers. Automatic printer discovery can cause endpoints to connect to and install drivers from printers not approved for enterprise use. Malicious devices posing as printers can advertise themselves through discovery protocols and trigger driver installation on endpoints. Enterprise printers should be provisioned through Group Policy deployed printer connections rather than user-initiated discovery. Disabling discovery does not affect connectivity to printers already configured through managed Group Policy printer policies.",
            Tags = ["printing", "discovery", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWebPnpDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWebPnpDownload")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWebPnpDownload", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-disable-print-driver-updates",
            Label = "Disable Automatic Print Driver Updates via Windows Update",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "Windows Update can automatically download and install updated printer drivers when new versions become available for installed print devices. Disabling automatic print driver updates through Windows Update prevents uncontrolled updates to drivers running in the privileged spooler process. Enterprise driver management policies require testing and validation of printer drivers before deployment to production endpoints. Automatic updates can receive untested or incompatible drivers that break printing functionality or introduce new security exposure. Printer driver updates should be evaluated, approved, and deployed through enterprise software management tools on a controlled schedule. This setting redirects driver update control to IT rather than preventing security updates entirely.",
            Tags = ["printing", "updates", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWindowsUpdatePrinterDrivers", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsUpdatePrinterDrivers")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWindowsUpdatePrinterDrivers", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-disable-printer-extension",
            Label = "Disable Printer Extension Apps",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 4,
            Description =
                "Printer extension apps are UWP applications that can be installed alongside V4 printer drivers to provide enhanced printing UI and management features. Disabling printer extension apps prevents installation and execution of vendor-supplied applications that accompany printer drivers. Third-party printer extension applications can include telemetry, marketing functionality, and network communications not required for printing. Vendor applications that run with elevated context via printer driver installation represent additional attack surface beyond core print functionality. Enterprise endpoints benefit from restricting software installation to explicitly approved applications while still supporting printing. Core printing functionality is fully retained without printer extension applications as they only provide supplemental UI features.",
            Tags = ["printing", "extensions", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrinterExtensions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterExtensions")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrinterExtensions", 1)],
        },
        new TweakDef
        {
            Id = "prtgpo-disable-rpc-over-namedpipes",
            Label = "Disable Print Spooler RPC over Named Pipes",
            Category = "Printer GPO Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "The print spooler communicates with remote print servers using RPC over TCP and over named pipes as transport mechanisms. Disabling RPC over named pipes for the print spooler forces all remote spooler communication to use RPC over TCP. Named pipe-based spooler communication was exploited in several PrintNightmare variants to achieve remote code execution and privilege escalation. Restricting spooler communications to TCP-based RPC makes monitoring and firewall control of spooler communications more straightforward. Named pipe transport is more difficult to restrict and monitor compared to TCP port-based firewall rules. Forcing TCP transport enables precise firewall rules to prevent unauthorized access to the print spooler service.",
            Tags = ["printing", "rpc", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RpcOverNamedPipes", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "RpcOverNamedPipes")],
            DetectOps = [RegOp.CheckDword(Key, "RpcOverNamedPipes", 0)],
        },
    ];
}
