// RegiLattice.Core — Tweaks/PrintTicketPolicy.cs
// Print Ticket Policy — Sprint 566.
// Configures Group Policy for Print Schema/Print Ticket validation,
// XPS rendering, printer capability discovery, and print schema
// namespace enforcement in enterprise print environments.
// Category: "Print Ticket Policy" | Slug: prttkt
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PrintTicket

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintTicketPolicy
{
    private const string TktKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PrintTicket";

    private const string PrtKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "prttkt-enable-print-ticket-validation",
                Label = "Print Ticket: Enable Print Ticket Schema Validation",
                Category = "Print Ticket Policy",
                Description =
                    "Sets ValidatePrintTickets=1 in PrintTicket policy. Enables XML schema validation of Print Tickets before they are processed by the print driver. A Print Ticket is an XML document that describes the desired print job settings (paper size, colour mode, duplex, media type). Malformed or crafted Print Tickets with invalid XML — including oversized attribute values or deeply nested structures — can trigger XML parser vulnerabilities in GDI/XPS rendering code. Enabling validation rejects malformed Print Tickets before they reach the vulnerable parsing code, reducing the attack surface for print-job-based exploits.",
                Tags = ["print-ticket", "xml", "validation", "security", "schema"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Print Tickets are schema-validated before processing. Malformed or non-compliant Print Tickets cause the job to fail with an error. Well-formed Print Tickets from standard Windows print dialogs and Microsoft applications are always valid.",
                ApplyOps = [RegOp.SetDword(TktKey, "ValidatePrintTickets", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "ValidatePrintTickets")],
                DetectOps = [RegOp.CheckDword(TktKey, "ValidatePrintTickets", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-disable-xps-rendering-sandbox-bypass",
                Label = "Print Ticket: Disable XPS Rendering Sandbox Bypass",
                Category = "Print Ticket Policy",
                Description =
                    "Sets DisableXpsRenderingBypass=1 in PrintTicket policy. Prevents applications from bypassing the XPS rendering pipeline sandbox. When a print job is sent as XPS data (from an application using the XPS Document Interface), the rendering is performed in a sandboxed low-privilege process. Some applications or malicious payloads can attempt to invoke a direct rendering path that bypasses the sandbox — processing XPS content with the full privilege of the calling process. Enabling this setting forces all XPS rendering through the sandboxed pipeline regardless of the caller's request.",
                Tags = ["print-ticket", "xps", "sandbox", "security", "rendering"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "XPS print jobs cannot bypass the rendering sandbox. All XPS content is processed in the isolated XPS rendering host. No user-visible impact for standard printing. Prevents privilege escalation via malicious XPS payloads in print jobs.",
                ApplyOps = [RegOp.SetDword(TktKey, "DisableXpsRenderingBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "DisableXpsRenderingBypass")],
                DetectOps = [RegOp.CheckDword(TktKey, "DisableXpsRenderingBypass", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-restrict-print-ticket-namespace",
                Label = "Print Ticket: Restrict Print Ticket XML Namespaces to Approved List",
                Category = "Print Ticket Policy",
                Description =
                    "Sets RestrictCustomNamespaces=1 in PrintTicket policy. Restricts Print Ticket XML namespaces to the standard Print Schema namespace plus explicitly approved vendor extensions. Print Tickets support vendor-defined custom XML namespaces for proprietary printer features. A maliciously crafted Print Ticket can include a large number of custom namespace declarations, causing the XML parser to resolve namespaces recursively (XML namespace expansion attack) or consume excessive memory. Restricting namespaces to known-good ones eliminates this attack vector.",
                Tags = ["print-ticket", "xml-namespace", "security", "print-schema", "restriction"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print Tickets with unapproved custom XML namespaces are rejected. Standard Print Schema namespaces (Microsoft) are always approved. Printers using proprietary extensions with unlisted namespaces may have those extensions ignored.",
                ApplyOps = [RegOp.SetDword(TktKey, "RestrictCustomNamespaces", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "RestrictCustomNamespaces")],
                DetectOps = [RegOp.CheckDword(TktKey, "RestrictCustomNamespaces", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-enable-wsd-printer-discovery-logging",
                Label = "Print Ticket: Enable WSD Printer Discovery Logging",
                Category = "Print Ticket Policy",
                Description =
                    "Sets WsdDiscoveryLogging=1 in PrintTicket policy. Enables logging of Web Services on Devices (WSD) printer discovery events. WSD is the network printer discovery protocol used by Windows to automatically find and install network printers. WSD discovery responses are XML documents parsed by the Windows printer subsystem. Logging WSD discovery events provides visibility into which printers the system detected, which printers were installed automatically, and whether any unexpected WSD responses were received — useful for detecting rogue printer injection attacks where an attacker's device responds to WSD probes with a malicious printer description.",
                Tags = ["wsd", "printer-discovery", "logging", "network", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WSD printer discovery events are logged. Provides audit trail of automatic printer installations. Useful for detecting rogue printer injection in environments with WSD-enabled networks. Minor event log volume in stable environments.",
                ApplyOps = [RegOp.SetDword(TktKey, "WsdDiscoveryLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "WsdDiscoveryLogging")],
                DetectOps = [RegOp.CheckDword(TktKey, "WsdDiscoveryLogging", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-disable-auto-wsd-install",
                Label = "Print Ticket: Disable Automatic WSD Printer Installation",
                Category = "Print Ticket Policy",
                Description =
                    "Sets DisableAutoWsdInstall=1 in Printers policy. Prevents Windows from automatically installing WSD-discovered printers without user confirmation or administrator intervention. WSD auto-install reads the printer's XML device description and installs a print driver automatically. An attacker on the local network can broadcast crafted WSD printer advertisements causing Windows to auto-install drivers from rogue printers — if the driver installation triggers a code execution vector (custom driver DLL), the auto-install path is exploitable without any user interaction. Disabling auto-install prevents unsolicited printer additions.",
                Tags = ["wsd", "auto-install", "printer", "security", "rogue-printer"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WSD printers on the network are not automatically installed. Users or administrators must manually add WSD printers via 'Add a printer'. Prevents rogue WSD printer injection. All new printer additions become explicit IT-authorised actions.",
                ApplyOps = [RegOp.SetDword(PrtKey, "DisableAutoWsdInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableAutoWsdInstall")],
                DetectOps = [RegOp.CheckDword(PrtKey, "DisableAutoWsdInstall", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-set-max-print-ticket-size-64kb",
                Label = "Print Ticket: Set Maximum Print Ticket XML Size to 64 KB",
                Category = "Print Ticket Policy",
                Description =
                    "Sets MaxPrintTicketSize=65536 in PrintTicket policy (bytes). Sets the maximum allowed size for a Print Ticket XML document to 64 KB. A legitimate Print Ticket for a printer with comprehensive feature support (media handling, finishing, stapling options, colour profiles) is typically 5-15 KB. There is no legitimate reason for a Print Ticket to be larger. Oversized Print Tickets that exceed the limit are rejected before being passed to the XML parser — preventing XML bomb attacks (exponential entity expansion) or other size-based parser exploits that would attempt to process megabytes of XML through a kernel component.",
                Tags = ["print-ticket", "xml-size", "dos", "security", "parser"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Print Tickets larger than 64 KB are rejected. All standard Windows print dialogs generate Print Tickets well under 64 KB. Only custom or malformed Print Tickets would exceed this size. No impact on normal printing.",
                ApplyOps = [RegOp.SetDword(TktKey, "MaxPrintTicketSize", 65536)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "MaxPrintTicketSize")],
                DetectOps = [RegOp.CheckDword(TktKey, "MaxPrintTicketSize", 65536)],
            },
            new TweakDef
            {
                Id = "prttkt-enable-capability-schema-enforcement",
                Label = "Print Ticket: Enforce PrintCapabilities Schema on Driver Provider",
                Category = "Print Ticket Policy",
                Description =
                    "Sets EnforceCapabilitySchema=1 in PrintTicket policy. Requires print drivers to provide a schema-conformant PrintCapabilities document when queried by the print subsystem. PrintCapabilities is the XML document that describes what a printer can do (available media types, print qualities, finishing options). Some legacy drivers return malformed or empty PrintCapabilities responses causing the Windows XPS/Print Schema layer to fall back to guessed defaults or crash. Enforcing schema compliance causes drivers returning invalid PrintCapabilities to produce a validation error rather than passing corrupt XML further into the stack.",
                Tags = ["print-ticket", "print-capabilities", "driver", "schema", "validation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Drivers providing non-schema-conformant PrintCapabilities generate an error. Some legacy printer drivers (pre-Vista v3 drivers) may fail this check. The printer appears in print dialogs with limited options rather than crashing. Only affects non-conformant legacy drivers.",
                ApplyOps = [RegOp.SetDword(TktKey, "EnforceCapabilitySchema", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "EnforceCapabilitySchema")],
                DetectOps = [RegOp.CheckDword(TktKey, "EnforceCapabilitySchema", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-disable-network-scan-to-print",
                Label = "Print Ticket: Disable Network Scan-to-Print Direct Integration",
                Category = "Print Ticket Policy",
                Description =
                    "Sets DisableScanToPrint=1 in PrintTicket policy. Disables the Windows Scan-to-Print direct integration feature that allows WSD-enabled multi-function printers to push scanned documents directly into the Windows print queue for automatic printing. Direct scan-to-print integration accepts document data from network devices without user-initiated authentication. An attacker with access to the local network who can simulate a WSD scanner can push arbitrary document data into the print pipeline by impersonating a scannner. Disabling this feature requires users to initiate scan operations from Windows Fax and Scan or third-party software.",
                Tags = ["scan-to-print", "wsd", "network-scanner", "security", "injection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Scan-to-Print direct integration is disabled. Scanned documents are not pushed directly to the printer from the scanner. Users must initiate scanning via Windows Fax and Scan or the scanner's software. Prevents unauthorized network device injection of print data.",
                ApplyOps = [RegOp.SetDword(TktKey, "DisableScanToPrint", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "DisableScanToPrint")],
                DetectOps = [RegOp.CheckDword(TktKey, "DisableScanToPrint", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-allow-only-v4-xps-print-path",
                Label = "Print Ticket: Allow Only V4 XPS Print Path for Network Printers",
                Category = "Print Ticket Policy",
                Description =
                    "Sets EnforceXpsPrintPath=1 in PrintTicket policy. Restricts network printer connections to the v4 XPS print path exclusively. The v4 XPS print path processes all print jobs through the GDI-to-XPS conversion path, running in an isolated XPS rendering host. The legacy v3 GDI direct print path processes documents in the context of the calling application or SYSTEM — a code execution vulnerability in the rendering path is much higher privilege. Enforcing the XPS path for network printers ensures malicious print data processed from network sources is contained in the lower-privilege XPS host.",
                Tags = ["print-ticket", "v4-driver", "xps-path", "security", "isolation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Network printers must use the v4 XPS print path. Printers with only v3 legacy drivers may fail to connect via network. Most printers released after Windows 8 have v4 driver packages. Legacy specialty printers (label, receipt, industrial) may only have v3 drivers.",
                ApplyOps = [RegOp.SetDword(TktKey, "EnforceXpsPrintPath", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "EnforceXpsPrintPath")],
                DetectOps = [RegOp.CheckDword(TktKey, "EnforceXpsPrintPath", 1)],
            },
            new TweakDef
            {
                Id = "prttkt-restrict-print-ticket-processing-to-users",
                Label = "Print Ticket: Restrict Print Ticket Processing to Authorised User Sessions",
                Category = "Print Ticket Policy",
                Description =
                    "Sets RestrictToUserSessions=1 in PrintTicket policy. Restricts Print Ticket processing to originate only from authenticated user sessions (interactive or service sessions with a valid user token). Print Tickets submitted without an associated user session token (e.g., from an anonymous service account or through a NULL session SMB path) are rejected. This prevents attackers from submitting print jobs anonymously that would be processed with SYSTEM-level privileges in the spooler. All legitimate print submissions in enterprise environments originate from authenticated user accounts.",
                Tags = ["print-ticket", "authentication", "session", "security", "anonymous"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Print Tickets without an authenticated user session token are rejected. Anonymous print submissions are blocked. All printing from authenticated users and services with valid user tokens is unaffected.",
                ApplyOps = [RegOp.SetDword(TktKey, "RestrictToUserSessions", 1)],
                RemoveOps = [RegOp.DeleteValue(TktKey, "RestrictToUserSessions")],
                DetectOps = [RegOp.CheckDword(TktKey, "RestrictToUserSessions", 1)],
            },
        ];
}
