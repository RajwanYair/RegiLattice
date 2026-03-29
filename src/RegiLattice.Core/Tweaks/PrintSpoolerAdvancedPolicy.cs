// RegiLattice.Core — Tweaks/PrintSpoolerAdvancedPolicy.cs
// Print Spooler service hardening, driver install restrictions, and spooler security policy — Sprint 472.
// Category: "Print Spooler Advanced Policy" | Slug: spladv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PrintSpoolerAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "spladv-disable-print-spooler",
            Label        = "Disable Print Spooler Service",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Disables the Windows Print Spooler service entirely, eliminating the PrintNightmare attack surface and all spooler-related privilege escalation vectors on systems that do not need to print.",
            Tags         = ["spooler", "printing", "security", "printnightmare", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 4,
            ImpactNote   = "Print Spooler fully disabled; printing and fax completely unavailable until re-enabled.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableSpooler", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableSpooler")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableSpooler", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-block-remote-printer-install",
            Label        = "Block Non-Admin Remote Printer Driver Installation",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Prevents non-administrator users from installing printer drivers remotely via the spooler, closing the PrintNightmare (CVE-2021-34527) driver-install privilege escalation path.",
            Tags         = ["spooler", "printing", "security", "printnightmare", "driver", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Non-admin remote driver install blocked; closes PrintNightmare attack path.",
            ApplyOps     = [RegOp.SetDword(Key, "RestrictDriverInstallationToAdministrators", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RestrictDriverInstallationToAdministrators")],
            DetectOps    = [RegOp.CheckDword(Key, "RestrictDriverInstallationToAdministrators", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-disable-mxdc-rendering",
            Label        = "Disable MXDC Package Rendering in Print Spooler",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Disables the Microsoft XPS Document Converter (MXDC) rendering path in the spooler, blocking an attack vector where malicious XPS documents exploit the spooler RPC interface.",
            Tags         = ["spooler", "xps", "mxdc", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "MXDC XPS rendering disabled in spooler; XPS print conversion requires an alternative method.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableMXDCRendering", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableMXDCRendering")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableMXDCRendering", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-disable-internet-printing-client",
            Label        = "Disable Internet Printing Client (IPP over HTTP)",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Disables the Internet Printing Protocol (IPP) client in Windows, preventing print jobs from being submitted to printers over HTTP/HTTPS and closing the associated network attack surface.",
            Tags         = ["spooler", "ipp", "internet-printing", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "IPP client disabled; cannot print to networked IPP printers over HTTP. Local USB printing unaffected.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableWebPrinting", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableWebPrinting")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableWebPrinting", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-disable-printer-browse-list",
            Label        = "Disable Printer Browse List on Domain",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Disables the automatic browse list that advertises available printers across a domain, reducing network discovery noise and preventing spooler-based reconnaissance.",
            Tags         = ["spooler", "printing", "browsing", "domain", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Printer browse list disabled; users must manually add printers by UNC path or IP.",
            ApplyOps     = [RegOp.SetDword(Key, "DisablePrinterBrowsing", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisablePrinterBrowsing")],
            DetectOps    = [RegOp.CheckDword(Key, "DisablePrinterBrowsing", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-block-print-to-xps",
            Label        = "Block Print to XPS Document Writer",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Blocks the Microsoft XPS Document Writer virtual printer, preventing users from saving print jobs to XPS format files and closing the XPS writer spooler attack surface.",
            Tags         = ["spooler", "xps", "virtual-printer", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "XPS Document Writer virtual printer disabled; cannot save print output as .xps files.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableXPSDocumentWriter", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableXPSDocumentWriter")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableXPSDocumentWriter", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-block-lpt-port-printing",
            Label        = "Block LPT Parallel Port Printer Access",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Blocks the spooler from accessing LPT (parallel port) printer connections, removing a legacy attack surface on systems that do not have or use parallel port printers.",
            Tags         = ["spooler", "lpt", "parallel-port", "legacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 1,
            SafetyRating = 5,
            ImpactNote   = "LPT parallel port printing blocked; legacy parallel-port printers not usable.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableLPTPortPrinting", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableLPTPortPrinting")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableLPTPortPrinting", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-disable-com-port-printing",
            Label        = "Block COM Serial Port Printer Access",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Blocks the spooler from accessing COM (serial port) printer connections, removing legacy serial printing capability that is not needed on modern systems.",
            Tags         = ["spooler", "com-port", "serial-port", "legacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 1,
            SafetyRating = 5,
            ImpactNote   = "COM serial port printing blocked; legacy serial-port printers not usable.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableCOMPortPrinting", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableCOMPortPrinting")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableCOMPortPrinting", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-block-outbound-spool-jobs",
            Label        = "Block Outbound Print Job Forwarding from This Machine",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Prevents this Windows machine from forwarding print jobs to remote printers via the spooler, an attack path used to steal NTLM credentials (printer capture attacks).",
            Tags         = ["spooler", "printing", "ntlm-capture", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Outbound spooler job forwarding blocked; remote printer capture attacks (e.g., RespNTLM) mitigated.",
            ApplyOps     = [RegOp.SetDword(Key, "NoRemoteSpooler", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "NoRemoteSpooler")],
            DetectOps    = [RegOp.CheckDword(Key, "NoRemoteSpooler", 1)],
        },
        new TweakDef
        {
            Id           = "spladv-disable-spooler-inbound-access",
            Label        = "Disable Inbound Print Spooler RPC Access",
            Category     = "Print Spooler Advanced Policy",
            Description  = "Disables the inbound RPC interface on the Print Spooler, preventing remote machines from submitting print jobs to this machine via the spooler, closing another PrintNightmare-family attack vector.",
            Tags         = ["spooler", "rpc", "security", "printnightmare", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "Inbound spooler RPC disabled; this machine cannot be used as a print server by remote clients.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableSpoolerInboundRPC", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableSpoolerInboundRPC")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableSpoolerInboundRPC", 1)],
        },
    ];
}
