// RegiLattice.Core — Tweaks/IppEverywherePolicy.cs
// IPP Everywhere driverless printing standards compliance and policy controls — Sprint 476.
// Category: "IPP Everywhere Policy" | Slug: ippevy
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPPEverywhere

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class IppEverywherePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPPEverywhere";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ippevy-disable-ipp-everywhere",
                Label = "Disable IPP Everywhere Driverless Printing",
                Category = "IPP Everywhere Policy",
                Description =
                    "Disables the IPP Everywhere driverless printing framework, forcing Windows to rely on traditional printer drivers instead of the universal IPP print path used by modern printers.",
                Tags = ["ipp-everywhere", "driverless-printing", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "IPP Everywhere disabled; modern printers may require explicit driver install instead of auto-detection.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIPPEverywhere", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPEverywhere")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPPEverywhere", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-block-cloud-ipp-print",
                Label = "Block Cloud IPP Print (Universal Cloud Print Path)",
                Category = "IPP Everywhere Policy",
                Description =
                    "Blocks cloud-relayed IPP print paths that route print jobs through Microsoft cloud infrastructure, ensuring all print jobs are submitted directly to local network printers without cloud relay.",
                Tags = ["ipp-everywhere", "cloud-print", "printing", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Cloud-relayed IPP printing blocked; print jobs only submitted directly to LAN printers.",
                ApplyOps = [RegOp.SetDword(Key, "BlockCloudIPPPrint", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCloudIPPPrint")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCloudIPPPrint", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-require-pw-format",
                Label = "Require PWG Raster Format Validation for IPP Jobs",
                Category = "IPP Everywhere Policy",
                Description =
                    "Enforces format validation for PWG Raster print data submitted via IPP Everywhere, rejecting malformed print data that could trigger parsing vulnerabilities in printer firmware.",
                Tags = ["ipp-everywhere", "pwg-raster", "printing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "PWG Raster data validated before forwarding; malformed print payloads rejected at host.",
                ApplyOps = [RegOp.SetDword(Key, "RequirePWGRasterValidation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePWGRasterValidation")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePWGRasterValidation", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-block-apple-airprint",
                Label = "Block Apple AirPrint via IPP Everywhere on Windows",
                Category = "IPP Everywhere Policy",
                Description =
                    "Blocks the AirPrint protocol layer that allows Apple devices to print to Windows-shared printers using IPP Everywhere, preventing uncontrolled cross-platform printer sharing.",
                Tags = ["ipp-everywhere", "airprint", "apple", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "AirPrint to Windows-shared printers blocked; Apple devices cannot print to this host via AirPrint.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAirPrint", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAirPrint")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAirPrint", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-disable-ipp-infra-service",
                Label = "Disable IPP Infrastructure Service (Universal Print Relay)",
                Category = "IPP Everywhere Policy",
                Description =
                    "Disables the Windows IPP Infrastructure Background Service that routes IPP jobs to printers registered in Microsoft Universal Print, forcing direct queue usage.",
                Tags = ["ipp-everywhere", "universal-print", "microsoft", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IPP Infrastructure Service disabled; Universal Print routing service inactive.",
                ApplyOps = [RegOp.SetDword(Key, "DisableIPPInfraService", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPInfraService")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPPInfraService", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-block-mopria-print",
                Label = "Block Mopria Print Discovery and Submission",
                Category = "IPP Everywhere Policy",
                Description =
                    "Blocks the Mopria Alliance standard print path that allows Android and other devices to discover and submit print jobs to Windows-shared printers via Mopria-compliant IPP.",
                Tags = ["ipp-everywhere", "mopria", "android", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Mopria print blocked; Android devices cannot print to this host via Mopria IPP.",
                ApplyOps = [RegOp.SetDword(Key, "BlockMopriaPrint", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMopriaPrint")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMopriaPrint", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-require-tls-12-minimum",
                Label = "Require TLS 1.2 Minimum for IPP Everywhere HTTPS",
                Category = "IPP Everywhere Policy",
                Description =
                    "Enforces a minimum of TLS 1.2 for IPPS connections used in IPP Everywhere print paths, blocking print traffic over TLS 1.0 or 1.1 which are deprecated and cryptographically weak.",
                Tags = ["ipp-everywhere", "tls", "security", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IPP Everywhere IPPS upgraded to TLS 1.2 minimum; old TLS versions rejected.",
                ApplyOps = [RegOp.SetDword(Key, "MinimumTLSVersionForIPPS", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinimumTLSVersionForIPPS")],
                DetectOps = [RegOp.CheckDword(Key, "MinimumTLSVersionForIPPS", 2)],
            },
            new TweakDef
            {
                Id = "ippevy-disable-pdf-print-path",
                Label = "Disable IPP Everywhere PDF Print Format Path",
                Category = "IPP Everywhere Policy",
                Description =
                    "Disables the PDF-based print format path in IPP Everywhere, preventing Windows from generating PDF documents during the print process which avoids PDF parser vulnerabilities in printer firmware.",
                Tags = ["ipp-everywhere", "pdf", "print-format", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "PDF print path disabled in IPP Everywhere; pwg-raster or other formats used instead.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePDFPrintPath", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePDFPrintPath")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePDFPrintPath", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-log-ipp-everywhere-jobs",
                Label = "Enable Audit Logging for IPP Everywhere Print Jobs",
                Category = "IPP Everywhere Policy",
                Description =
                    "Enables event log entries for print jobs submitted via the IPP Everywhere path, providing a record of driverless print activity including job source IP and document metadata.",
                Tags = ["ipp-everywhere", "audit-log", "printing", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "IPP Everywhere print jobs logged; driverless print activity auditable in event viewer.",
                ApplyOps = [RegOp.SetDword(Key, "AuditIPPEverywhereJobs", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditIPPEverywhereJobs")],
                DetectOps = [RegOp.CheckDword(Key, "AuditIPPEverywhereJobs", 1)],
            },
            new TweakDef
            {
                Id = "ippevy-block-anonymous-ipp-print",
                Label = "Block Anonymous IPP Everywhere Print Submissions",
                Category = "IPP Everywhere Policy",
                Description =
                    "Blocks unauthenticated (anonymous) print job submissions via IPP Everywhere, requiring all IPP Everywhere clients to present credentials before print jobs are accepted.",
                Tags = ["ipp-everywhere", "authentication", "anonymous", "printing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Anonymous IPP Everywhere print blocked; authentication required for all driverless print submissions.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAnonymousIPPEverywherePrint", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAnonymousIPPEverywherePrint")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAnonymousIPPEverywherePrint", 1)],
            },
        ];
}
