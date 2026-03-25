// RegiLattice.Core — Tweaks/PrintSpoolAdvPolicy.cs
// Sprint 342: Print Spooler Advanced Policy tweaks (10 tweaks)
// Category: "Print Spool Advanced Policy" | Slug: prtspool
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintSpoolAdvPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "prtspool-disable-point-and-print-unrestricted",
            Label = "Disable Unrestricted Point and Print Driver Installation",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Point and Print allows non-administrative users to install printer drivers from print servers which was exploited in PrintNightmare (CVE-2021-34527) to achieve remote code execution. Disabling unrestricted Point and Print requires administrator approval for all printer driver installations preventing non-admin users from silently installing potentially malicious drivers. The PrintNightmare vulnerability affected all versions of Windows and had critical severity because print spool runs as SYSTEM allowing full system compromise through printer driver installation. Restricting Point and Print to specific approved print servers further limits the attack surface compared to completely unrestricted driver installation. Organizations should combine Point and Print restrictions with disabling the Print Spooler service on systems that do not need printing capability. Microsoft released multiple patches for PrintNightmare and organizations should ensure all patches are applied in addition to implementing this Group Policy restriction.",
            Tags = ["print-spool", "point-and-print", "printnightmare", "driver", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "Restricted", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "Restricted")],
            DetectOps = [RegOp.CheckDword(Key, "Restricted", 1)],
        },
        new TweakDef
        {
            Id = "prtspool-require-admin-for-driver-update",
            Label = "Require Administrator Approval for Printer Driver Updates",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Printer driver updates through Point and Print can silently replace existing drivers with malicious versions without user consent if administrator approval is not enforced. Requiring administrator approval for driver updates closes the update vector of PrintNightmare-style attacks where a driver update was used to inject malicious code. Automatic driver updates from print servers allow a compromised print server to push malicious driver updates to all clients that connect to it. Administrator approval workflows for driver updates ensure that only vetted and approved printer drivers are installed on systems. Organizations with centrally managed print infrastructure should use Windows Server Update Services or System Center to manage printer driver distribution rather than Point and Print. Monitoring for printer driver installation events (Event ID 316 in the System log) provides detection capability for unauthorized driver installation attempts.",
            Tags = ["print-spool", "driver-update", "point-and-print", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "UpdatePromptSettings", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "UpdatePromptSettings")],
            DetectOps = [RegOp.CheckDword(Key, "UpdatePromptSettings", 2)],
        },
        new TweakDef
        {
            Id = "prtspool-restrict-point-and-print-servers",
            Label = "Restrict Point and Print to Approved Print Server List",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Restricting Point and Print to a specific allowlist of approved print servers prevents users from installing printer drivers from arbitrary or attacker-controlled print servers. Print server allowlists are managed through the TrustedServers registry value and contain hostnames or IP addresses of organizational print servers. Unrestricted print server access allows an attacker to set up a rogue print server and convince users to connect to it which silently installs malicious drivers. Print server restrictions combined with driver installation approval requirements provide layered defense against print driver based attacks. Organizations should define all authorized print servers in the Group Policy allowlist and review it during server decommissioning to remove stale entries. Users attempting to connect to non-allowlisted print servers should receive an error message directing them to submit a request for an approved print server.",
            Tags = ["print-spool", "server-restriction", "point-and-print", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "TrustedServersEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "TrustedServersEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "TrustedServersEnabled", 1)],
        },
        new TweakDef
        {
            Id = "prtspool-disable-print-spooler-remote-rpc",
            Label = "Disable Remote Print Spooler RPC Connections",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "The Print Spooler service exposes Remote Procedure Call interfaces on the network that were used to exploit PrintNightmare and the Windows Print Spooler Remote Code Execution vulnerability series. Disabling remote print spooler RPC connections prevents network-based exploitation of print spooler vulnerabilities by blocking the RPC endpoint from remote access. Print spooler RPC is most dangerous on domain controllers where compromise of the print spooler runs as SYSTEM and can lead to full domain compromise. Organizations running Windows Server Core deployments where printing is not required should disable the Print Spooler service entirely along with its RPC exposure. Print workstations and print servers require the print spooler but should restrict it to local connections and authorized traffic only. The RegisterSpoolerRemoteRpcEndPoint value set to 2 disables remote RPC while allowing local printing to continue.",
            Tags = ["print-spool", "rpc", "remote-access", "printnightmare", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RegisterSpoolerRemoteRpcEndPoint", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "RegisterSpoolerRemoteRpcEndPoint")],
            DetectOps = [RegOp.CheckDword(Key, "RegisterSpoolerRemoteRpcEndPoint", 2)],
        },
        new TweakDef
        {
            Id = "prtspool-disable-web-printing-communication",
            Label = "Disable Windows Internet Printing Protocol Communication",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Internet Printing Protocol (IPP) over HTTP/HTTPS allows printing to printers and print servers over the internet which is rarely needed and creates unnecessary attack surface. Disabling web printing communication prevents users from connecting to internet-based print servers and removes HTTPS-based printer communication channels. Internet printing can be used by attackers to exfiltrate data by printing to an attacker-controlled IPP server over HTTPS bypassing traditional data loss prevention. Windows Internet Printing is implemented through the Internet Printing Client feature which can be removed through Programs and Features for systems that do not require it. Organizations should audit whether any users require internet printing capability and disable it for those who do not. IPP printing to internal corporate printers over secure networks may still use the IPP protocol through internal print servers without requiring the Internet Printing feature.",
            Tags = ["print-spool", "ipp", "internet-printing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWebPrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWebPrinting")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWebPrinting", 1)],
        },
        new TweakDef
        {
            Id = "prtspool-redirect-print-spool-directory",
            Label = "Restrict Print Spooler Directory to Non-System Drive",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "The print spooler temporary directory stores print jobs before they are sent to the printer and is a location that historically has been exploited for privilege escalation. Redirecting the print spooler directory to a non-system drive with restricted permissions reduces the risk of print-related privilege escalation through directory manipulation. Print spooler exploitation often involves writing malicious DLLs or executables to the spool directory which gets loaded by the SYSTEM-level spooler process. Configuring a dedicated print spool directory on a non-system partition with appropriate ACLs reduces the ability to inject code into the print spooler execution context. Organizations should set strict permissions on the print spooler directory allowing only the print spooler service account to write to it. Monitoring for unexpected file creation in the print spool directory provides detection capability for attempted print spooler attacks.",
            Tags = ["print-spool", "directory-security", "privilege-escalation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ForceGuestAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceGuestAccess")],
            DetectOps = [RegOp.CheckDword(Key, "ForceGuestAccess", 0)],
        },
        new TweakDef
        {
            Id = "prtspool-disable-print-spool-named-pipe",
            Label = "Disable Print Spooler Named Pipe Access for Non-Admins",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Print spooler named pipes provide a local IPC channel for print management that can be exploited by local attackers to relay credentials or exploit spooler vulnerabilities. Disabling print spooler named pipe access for non-administrators limits the attack surface available to standard users who want to exploit the print spooler. The spooler named pipe wssprint2 was used in some print spooler exploits to relay authentication from the SYSTEM-level spooler process to attacker-controlled services. Limiting named pipe access to the print spooler reduces attackers' ability to force-authenticate the SYSTEM account through the print spooler using credential relay techniques. Organizations that disable named pipe access should test printing functionality thoroughly as some applications use named pipes to communicate with the print spooler. Monitoring for non-standard named pipe access to the print spooler service helps detect exploitation attempts.",
            Tags = ["print-spool", "named-pipe", "credential-relay", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictDriverInstallation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictDriverInstallation")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictDriverInstallation", 1)],
        },
        new TweakDef
        {
            Id = "prtspool-enable-detailed-spool-audit-events",
            Label = "Enable Detailed Audit Events for Print Spooler Operations",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Detailed print spooler audit events capture driver installations, print job submission, and administrative changes to the print infrastructure for security monitoring. Enabling detailed spooler audit events provides forensic data for investigating potential PrintNightmare exploitation attempts and unauthorized driver installations. Print spooler attack events are recorded in the Microsoft-Windows-PrintService/Admin and Operational event logs which should be captured by SIEM. Event ID 808 in the PrintService/Admin log records when a printer driver was added which is a key indicator for print-based attacks. Regular review of print service audit data helps identify unauthorized print server connections and driver installation attempts that may indicate attack attempts. Organizations should configure event log forwarding specifically for print service logs on systems where printing is a potential attack vector.",
            Tags = ["print-spool", "audit", "monitoring", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableDetailedAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedAudit")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDetailedAudit", 1)],
        },
        new TweakDef
        {
            Id = "prtspool-disable-print-to-file",
            Label = "Disable Print to File Functionality for Standard Users",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Print to File functionality in the print spooler allows output to be redirected to a file rather than a printer which can be used as a data exfiltration method on systems with strict network controls. Disabling print-to-file for standard users reduces this exfiltration vector preventing users from printing sensitive documents to file shares or removable media. Print-to-file combined with DLP policies can help enforce data handling requirements for sensitive documents that must only be printed to controlled printers. Organizations in regulated industries should evaluate whether print-to-file aligns with their data control policies before making individual decisions. PDF printers and virtual print drivers provide similar functionality to print-to-file and should be reviewed as part of a comprehensive print control policy. Disabling print-to-file does not prevent printing to physical printers and has no impact on operational printing activities.",
            Tags = ["print-spool", "print-to-file", "data-exfiltration", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrintToFile", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToFile")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrintToFile", 1)],
        },
        new TweakDef
        {
            Id = "prtspool-enforce-print-driver-signing",
            Label = "Enforce Digital Signature Verification for Printer Drivers",
            Category = "Print Spool Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Printer driver signature enforcement ensures that only drivers signed by trusted certificate authorities can be installed by the print spooler service. Enforcing driver signature verification prevents installation of unsigned or improperly signed printer drivers that could contain rootkit-level malware. The PrintNightmare exploitation path relied on the ability to install arbitrary DLLs as printer drivers even when driver signing was supposed to be enforced. Strict driver signature requirements should require Extended Validation (EV) code signing certificates to minimize the risk of spoofed or stolen certificates being used to sign malicious drivers. Organizations should test driver signing enforcement with their existing printer fleet to ensure all deployed drivers have valid signatures before enforcement. Legacy printer drivers without valid signatures must be replaced with signed alternatives or the printers must be retired before enforcing strict driver signing.",
            Tags = ["print-spool", "driver-signing", "code-signing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "InheritedPolicies", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "InheritedPolicies")],
            DetectOps = [RegOp.CheckDword(Key, "InheritedPolicies", 1)],
        },
    ];
}
