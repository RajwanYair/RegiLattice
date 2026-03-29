// RegiLattice.Core — Tweaks/IppProtocolPolicy.cs
// Internet Printing Protocol (IPP) policy controls for client and server endpoints — Sprint 474.
// Category: "IPP Protocol Policy" | Slug: ipppol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPP

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class IppProtocolPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPP";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ipppol-disable-ipp-client",
                Label = "Disable IPP Printing Client",
                Category = "IPP Protocol Policy",
                Description =
                    "Disables the Windows Internet Printing Protocol (IPP) client, preventing Windows from submitting print jobs to network printers using RFC 8011 IPP over TCP/631.",
                Tags = ["ipp", "printing", "network", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IPP client disabled; cannot print to RFC 8011 IPP-compliant network printers.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIPPClient", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPClient")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPPClient", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-enforce-ipp-tls",
                Label = "Enforce TLS for IPP Print Jobs (IPPS)",
                Category = "IPP Protocol Policy",
                Description =
                    "Forces all IPP print jobs to use IPPS (IPP over TLS, port 443/631), preventing print data from being sent in plaintext over the network where it could be intercepted.",
                Tags = ["ipp", "ipps", "tls", "printing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "IPP print traffic encrypted via TLS; plaintext IPP on port 631 no longer accepted by client.",
                ApplyOps = [RegOp.SetDword(Key, "RequireIPPSForPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireIPPSForPrinting")],
                DetectOps = [RegOp.CheckDword(Key, "RequireIPPSForPrinting", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-block-ipp-everywhere-auto-add",
                Label = "Block IPP Everywhere Auto-Add Network Printers",
                Category = "IPP Protocol Policy",
                Description =
                    "Prevents Windows from automatically adding IPP Everywhere printers discovered on the local network via mDNS/Bonjour, stopping printers from being silently added to the system when connecting to a network.",
                Tags = ["ipp", "ipp-everywhere", "auto-add", "mdns", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IPP Everywhere auto-discovery blocked; new printers must be added manually.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIPPEverywhereAutoAdd", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPEverywhereAutoAdd")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPPEverywhereAutoAdd", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-require-ipp-auth",
                Label = "Require Authentication for IPP Print Jobs",
                Category = "IPP Protocol Policy",
                Description =
                    "Forces authentication for all IPP print jobs submitted to network printers, preventing anonymous IPP printing that could allow unauthorised print access or queue inspection.",
                Tags = ["ipp", "authentication", "printing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IPP print auth required; anonymous print jobs to IPP printers rejected.",
                ApplyOps = [RegOp.SetDword(Key, "RequireIPPAuthentication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireIPPAuthentication")],
                DetectOps = [RegOp.CheckDword(Key, "RequireIPPAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-block-cross-domain-ipp",
                Label = "Block IPP Printing to Cross-Domain Servers",
                Category = "IPP Protocol Policy",
                Description =
                    "Restricts IPP printing to print servers within the same domain, preventing print data (which may contain sensitive content) from being submitted to external or untrusted IPP endpoints.",
                Tags = ["ipp", "domain", "printing", "data-loss-prevention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "IPP printing restricted to domain servers; print jobs cannot be sent to external IPP endpoints.",
                ApplyOps = [RegOp.SetDword(Key, "BlockCrossDomainIPP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCrossDomainIPP")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCrossDomainIPP", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-disable-ipp-printer-share",
                Label = "Disable IPP Printer Sharing Outbound from This Host",
                Category = "IPP Protocol Policy",
                Description =
                    "Disables this host from acting as an IPP print server, stopping Windows from advertising locally configured printers as IPP endpoints that other devices can connect to.",
                Tags = ["ipp", "printer-sharing", "printing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "This host stops advertising printers via IPP; remote IPP print jobs to this machine rejected.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIPPPrinterSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPPrinterSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPPPrinterSharing", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-limit-ipp-max-job-size",
                Label = "Limit Maximum IPP Print Job Size to 100 MB",
                Category = "IPP Protocol Policy",
                Description =
                    "Caps the maximum size of a single IPP print job at 100 MB, preventing denial-of-service attacks that attempt to exhaust disk space or spooler memory via unexpectedly large print jobs.",
                Tags = ["ipp", "print-job", "dos-protection", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "IPP print jobs capped at 100 MB; oversized jobs rejected by spooler.",
                ApplyOps = [RegOp.SetDword(Key, "MaxIPPJobSizeMB", 100)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxIPPJobSizeMB")],
                DetectOps = [RegOp.CheckDword(Key, "MaxIPPJobSizeMB", 100)],
            },
            new TweakDef
            {
                Id = "ipppol-disable-ipp-compressed-jobs",
                Label = "Disable IPP Compressed (GZIP) Job Data",
                Category = "IPP Protocol Policy",
                Description =
                    "Disables compression (gzip/deflate) for IPP print job data, mitigating compression-based timing and oracle attacks against the IPP stream while simplifying spooler job processing.",
                Tags = ["ipp", "compression", "printing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "IPP job compression disabled; print data transmitted uncompressed.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIPPJobCompression", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPJobCompression")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPPJobCompression", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-block-ipp-mdns-advertisement",
                Label = "Block IPP Printer mDNS/Bonjour Advertisement",
                Category = "IPP Protocol Policy",
                Description =
                    "Prevents this host from broadcasting locally-share printers via mDNS/Bonjour, hiding the presence of connected printers from device discovery on the local network.",
                Tags = ["ipp", "mdns", "bonjour", "printer-discovery", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "mDNS printer advertisements disabled; connected printers invisible to network discovery tools.",
                ApplyOps = [RegOp.SetDword(Key, "BlockIPPmDNSAdvertisement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockIPPmDNSAdvertisement")],
                DetectOps = [RegOp.CheckDword(Key, "BlockIPPmDNSAdvertisement", 1)],
            },
            new TweakDef
            {
                Id = "ipppol-enable-ipp-audit-log",
                Label = "Enable IPP Print Job Audit Logging",
                Category = "IPP Protocol Policy",
                Description =
                    "Enables event log entries for IPP print jobs (job start, completion, errors), providing traceability for print operations for security monitoring and compliance.",
                Tags = ["ipp", "audit-log", "printing", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "IPP job lifecycle events logged; print activity auditable via event viewer.",
                ApplyOps = [RegOp.SetDword(Key, "EnableIPPAuditLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIPPAuditLog")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIPPAuditLog", 1)],
            },
        ];
}
