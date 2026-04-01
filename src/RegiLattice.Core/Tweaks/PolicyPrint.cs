// RegiLattice.Core — Tweaks/PolicyPrint.cs
// Printer management, print spooler security, IPP, internet printing, fax, and print queue policies
// Category: "Print Policy"
// Consolidated from 18 modules.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyPrint
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _FaxServicePolicy.Data,
            .. _InternetPrintingPolicy.Data,
            .. _IppEverywherePolicy.Data,
            .. _IppProtocolPolicy.Data,
            .. _PrinterDirectoryServicesPolicy.Data,
            .. _PrinterDriverIsolationPolicy.Data,
            .. _PrinterGpoPolicy.Data,
            .. _PrinterRedirectionPolicy.Data,
            .. _PrintJobManagementPolicy.Data,
            .. _PrintManagementPolicy.Data,
            .. _PrintQueuePolicy.Data,
            .. _PrintSpoolAdvPolicy.Data,
            .. _PrintSpoolerAdvancedPolicy.Data,
            .. _PrintSpoolerPolicy.Data,
            .. _PrintSpoolerSecurity.Data,
            .. _PrintSpoolFinalPolicy.Data,
            .. _PrintTicketPolicy.Data,
            .. _ProtectedPrintModePolicy.Data,
        ];

    // ── FaxServicePolicy ──
    private static class _FaxServicePolicy
    {
        private const string FaxLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Fax";
        private const string FaxCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows NT\Fax";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "faxsvc-disable-fax",
                Label = "Disable Fax Service",
                Category = "Printing",
                Description =
                    "Sets Fax=1 in the machine Fax policy key under DisabledComponents. "
                    + "Configures Windows Group Policy to mark the Fax service component as disabled at the policy level. "
                    + "Prevents fax services from being used on machines where faxing functionality is not required. "
                    + "Default: absent (Fax service allowed). Recommended: 1 on machines with no fax hardware or requirements.",
                Tags = ["fax", "service", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Fax service restricted at the Group Policy level; fax send/receive operations are blocked.",
                ApplyOps = [RegOp.SetDword(FaxLm, "Fax", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "Fax")],
                DetectOps = [RegOp.CheckDword(FaxLm, "Fax", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-online-fax",
                Label = "Disable Online Fax Service",
                Category = "Printing",
                Description =
                    "Sets OnlineFax=1 in the machine Fax policy key. "
                    + "Prevents users from sending faxes via online fax providers or cloud-based fax services. "
                    + "Blocks the 'Connect to a fax modem' and online fax integration from the Windows fax tools UI. "
                    + "Default: absent (online fax allowed). Recommended: 1 to prevent unsanctioned cloud fax usage.",
                Tags = ["fax", "online", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Online/cloud fax providers blocked at the policy level; only local modem-based fax permitted.",
                ApplyOps = [RegOp.SetDword(FaxLm, "OnlineFax", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "OnlineFax")],
                DetectOps = [RegOp.CheckDword(FaxLm, "OnlineFax", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-cover-pages",
                Label = "Disable Fax Cover Pages",
                Category = "Printing",
                Description =
                    "Sets CoverPages=1 in the machine Fax policy key. "
                    + "Prevents users from attaching cover pages to faxes sent through the Windows fax tool. "
                    + "Useful in environments that use standardised fax headers from the PBX or fax server. "
                    + "Default: absent (cover pages allowed). Recommended: 1 when corporate headers are auto-applied by the fax server.",
                Tags = ["fax", "cover-page", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Fax cover page attachment disabled; outgoing faxes are sent without a Windows-generated cover page.",
                ApplyOps = [RegOp.SetDword(FaxLm, "CoverPages", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "CoverPages")],
                DetectOps = [RegOp.CheckDword(FaxLm, "CoverPages", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-personal-cover-pages",
                Label = "Disable Personal Fax Cover Pages",
                Category = "Printing",
                Description =
                    "Sets PersonalCoverPages=1 in the machine Fax policy key. "
                    + "Prevents users from creating or storing personal fax cover page templates ("
                    + ".cov files) on their profile, restricting cover page management to IT-distributed templates. "
                    + "Default: absent (personal covers allowed). Recommended: 1 to enforce corporate cover page standards.",
                Tags = ["fax", "cover-page", "personal", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Personal fax cover page creation and storage blocked; only shared network templates usable.",
                ApplyOps = [RegOp.SetDword(FaxLm, "PersonalCoverPages", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "PersonalCoverPages")],
                DetectOps = [RegOp.CheckDword(FaxLm, "PersonalCoverPages", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-recipients",
                Label = "Disable Fax Recipient Book",
                Category = "Printing",
                Description =
                    "Sets DisableRecipients=1 in the machine Fax policy key. "
                    + "Removes the 'Select Recipients' feature from the Windows Fax and Scan UI, "
                    + "preventing users from building a personal fax contacts book. "
                    + "Default: absent (recipient book enabled). Recommended: 1 when fax routing is managed centrally.",
                Tags = ["fax", "recipients", "contacts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Personal fax recipient book removed from Windows Fax and Scan UI.",
                ApplyOps = [RegOp.SetDword(FaxLm, "DisableRecipients", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "DisableRecipients")],
                DetectOps = [RegOp.CheckDword(FaxLm, "DisableRecipients", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-require-send-tapi",
                Label = "Restrict Fax to TAPI Lines Only",
                Category = "Printing",
                Description =
                    "Sets TapiOnly=1 in the machine Fax policy key. "
                    + "Forces the Windows Fax service to use only TAPI-registered lines for sending faxes, "
                    + "preventing direct modem or non-TAPI send paths. Ensures fax traffic flows through audited channels. "
                    + "Default: absent (all send paths allowed). Recommended: 1 to ensure audit trail compliance.",
                Tags = ["fax", "tapi", "modem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Fax transmission restricted to TAPI-registered phone lines; non-TAPI fax paths are blocked.",
                ApplyOps = [RegOp.SetDword(FaxLm, "TapiOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "TapiOnly")],
                DetectOps = [RegOp.CheckDword(FaxLm, "TapiOnly", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-inbound-routing",
                Label = "Disable Inbound Fax Routing",
                Category = "Printing",
                Description =
                    "Sets InboundRouting=1 in the machine Fax policy key. "
                    + "Prevents the Windows fax service from routing incoming faxes to user inboxes or email. "
                    + "Forces a passive receive mode where received faxes are not forwarded or archived automatically. "
                    + "Default: absent (inbound routing enabled). Recommended: 1 when the PBX or upstream fax server handles routing.",
                Tags = ["fax", "inbound", "routing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Inbound fax auto-routing to user mailboxes or folders is disabled.",
                ApplyOps = [RegOp.SetDword(FaxLm, "InboundRouting", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "InboundRouting")],
                DetectOps = [RegOp.CheckDword(FaxLm, "InboundRouting", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-archive",
                Label = "Disable Fax Archive",
                Category = "Printing",
                Description =
                    "Sets Archive=1 in the machine Fax policy key. "
                    + "Prevents the Windows fax service from automatically archiving copies of sent and received faxes. "
                    + "Useful when archiving is handled by a dedicated fax compliance server and client-side archive is redundant. "
                    + "Default: absent (archive enabled). Recommended: 1 when a server-side archive is the sole authoritative copy.",
                Tags = ["fax", "archive", "retention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Client-side fax archive disabled; faxes won't be stored locally in the Windows Fax and Scan archive.",
                ApplyOps = [RegOp.SetDword(FaxLm, "Archive", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "Archive")],
                DetectOps = [RegOp.CheckDword(FaxLm, "Archive", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-fax-user",
                Label = "Disable Fax for Current User",
                Category = "Printing",
                Description =
                    "Sets Fax=1 in the per-user Fax policy key. "
                    + "Applies the fax disable policy for the current user only, without requiring a machine-wide GPO. "
                    + "Useful in BYOD or per-user policy environments where fax usage is restricted to specific roles. "
                    + "Default: absent (fax allowed for user). Recommended: 1 for non-fax users on a shared machine.",
                Tags = ["fax", "user", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Fax service restricted for the current user profile only.",
                ApplyOps = [RegOp.SetDword(FaxCu, "Fax", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxCu, "Fax")],
                DetectOps = [RegOp.CheckDword(FaxCu, "Fax", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-new-account",
                Label = "Disable Fax New Account Creation",
                Category = "Printing",
                Description =
                    "Sets NewAccounts=1 in the machine Fax policy key. "
                    + "Prevents users from adding new fax accounts or configuring additional fax connections in Windows. "
                    + "Ensures fax account provisioning is controlled by IT and prevents shadow fax connections. "
                    + "Default: absent (new account creation allowed). Recommended: 1 to lock down fax account provisioning.",
                Tags = ["fax", "account", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Users cannot add new fax accounts; all fax connections must be IT-provisioned.",
                ApplyOps = [RegOp.SetDword(FaxLm, "NewAccounts", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "NewAccounts")],
                DetectOps = [RegOp.CheckDword(FaxLm, "NewAccounts", 1)],
            },
        ];

    }

    // ── InternetPrintingPolicy ──
    private static class _InternetPrintingPolicy
    {
        private const string Prnt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
        private const string PnP = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "inetprt-disable-web-printing",
                Label = "Disable Web Printing",
                Category = "Printing",
                Description = "Prevents users from printing to Internet printers over HTTP.",
                Tags = ["printing", "network", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "DisableWebPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "DisableWebPrinting")],
                DetectOps = [RegOp.CheckDword(Prnt, "DisableWebPrinting", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-disable-http-printing",
                Label = "Disable HTTP Printing",
                Category = "Printing",
                Description = "Disables use of HTTP for connecting to printers on intranet/internet print servers.",
                Tags = ["printing", "network", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "DisableHTTPPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "DisableHTTPPrinting")],
                DetectOps = [RegOp.CheckDword(Prnt, "DisableHTTPPrinting", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-block-spooler-rpc-endpoint",
                Label = "Block Spooler Remote RPC Endpoint Registration",
                Category = "Printing",
                Description = "Prevents the print spooler from registering with the remote RPC endpoint mapper, reducing remote attack surface.",
                Tags = ["printing", "security", "group-policy", "hardening", "rpc"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "RegisterSpoolerRemoteRPCEndPoint", 0)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "RegisterSpoolerRemoteRPCEndPoint")],
                DetectOps = [RegOp.CheckDword(Prnt, "RegisterSpoolerRemoteRPCEndPoint", 0)],
            },
            new TweakDef
            {
                Id = "inetprt-block-kernel-mode-drivers",
                Label = "Block Kernel-Mode Printer Drivers",
                Category = "Printing",
                Description = "Prevents installation of kernel-mode printer drivers, which can be exploited for privilege escalation.",
                Tags = ["printing", "security", "group-policy", "hardening", "drivers"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "KMPrintersAreBlocked", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "KMPrintersAreBlocked")],
                DetectOps = [RegOp.CheckDword(Prnt, "KMPrintersAreBlocked", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-package-point-and-print-only",
                Label = "Restrict Point and Print to Package-Aware Drivers Only",
                Category = "Printing",
                Description = "Requires Point and Print connections to use only package-aware (.inf-packaged) printer drivers.",
                Tags = ["printing", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PnP, "PackagePointAndPrintOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(PnP, "PackagePointAndPrintOnly")],
                DetectOps = [RegOp.CheckDword(PnP, "PackagePointAndPrintOnly", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-pnp-no-warning-on-install",
                Label = "Require Warning + Elevation for Point and Print Driver Install",
                Category = "Printing",
                Description =
                    "Ensures users are warned and elevation is required when installing Point and Print drivers, mitigating PrintNightmare-class attacks.",
                Tags = ["printing", "security", "group-policy", "hardening", "uac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PnP, "NoWarningNoElevationOnInstall", 0)],
                RemoveOps = [RegOp.DeleteValue(PnP, "NoWarningNoElevationOnInstall")],
                DetectOps = [RegOp.CheckDword(PnP, "NoWarningNoElevationOnInstall", 0)],
            },
            new TweakDef
            {
                Id = "inetprt-pnp-require-update-prompt",
                Label = "Require Elevation for Point and Print Driver Updates",
                Category = "Printing",
                Description = "Forces elevation prompt when connecting to a print server that requires a newer driver version.",
                Tags = ["printing", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PnP, "UpdatePromptSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(PnP, "UpdatePromptSettings")],
                DetectOps = [RegOp.CheckDword(PnP, "UpdatePromptSettings", 0)],
            },
            new TweakDef
            {
                Id = "inetprt-disable-print-driver-download",
                Label = "Disable Automatic Print Driver Download from Windows Update",
                Category = "Printing",
                Description = "Prevents Windows from automatically downloading printer drivers from Windows Update.",
                Tags = ["printing", "network", "group-policy", "update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "DisableWebPnPDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "DisableWebPnPDownload")],
                DetectOps = [RegOp.CheckDword(Prnt, "DisableWebPnPDownload", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-restrict-driver-install-to-admins",
                Label = "Restrict Printer Driver Installation to Administrators",
                Category = "Printing",
                Description =
                    "Allows only administrators to install printer drivers, preventing non-admins from installing potentially malicious drivers.",
                Tags = ["printing", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "RestrictDriverInstallationToAdministrators", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "RestrictDriverInstallationToAdministrators")],
                DetectOps = [RegOp.CheckDword(Prnt, "RestrictDriverInstallationToAdministrators", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-disable-v3-printer-driver",
                Label = "Disable v3 User-Mode Printer Drivers",
                Category = "Printing",
                Description = "Prevents the use of v3 (user-mode) printer drivers; only v4 (kernel-mode isolated) drivers are allowed.",
                Tags = ["printing", "security", "group-policy", "drivers"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "V3DriverPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "V3DriverPolicy")],
                DetectOps = [RegOp.CheckDword(Prnt, "V3DriverPolicy", 1)],
            },
        ];

    }

    // ── IppEverywherePolicy ──
    private static class _IppEverywherePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPPEverywhere";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ippevy-disable-ipp-everywhere",
                    Label = "Disable IPP Everywhere Driverless Printing",
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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

    // ── IppProtocolPolicy ──
    private static class _IppProtocolPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPP";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ipppol-disable-ipp-client",
                    Label = "Disable IPP Printing Client",
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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

    // ── PrinterDirectoryServicesPolicy ──
    private static class _PrinterDirectoryServicesPolicy
    {
        private const string DsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DS";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pdssp-disable-printer-publishing",
                    Label = "Disable Automatic Printer Publishing to AD",
                    Category = "Printing",
                    Description =
                        "Sets PublishPrinters=0 to prevent Windows from automatically publishing printers to Active Directory "
                        + "Directory Services when they are added to the system. Unpublished printers are not discoverable via "
                        + "AD queries, reducing AD pollution from transient or personal printers on endpoints.",
                    Tags = ["printing", "active-directory", "publishing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops auto-publishing printers to AD; manually published printers and shared printers unaffected.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PublishPrinters", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PublishPrinters")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PublishPrinters", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-disable-printer-pruning",
                    Label = "Disable Printer Object Pruning from AD",
                    Category = "Printing",
                    Description =
                        "Sets PruningRetries=0 to disable the printer pruning mechanism that removes stale printer objects "
                        + "from Active Directory when the print server is unreachable. Prevents pruning in environments where "
                        + "print servers go offline temporarily (maintenance, DR failover) but should remain resolvable via AD.",
                    Tags = ["printing", "active-directory", "pruning", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Disables AD pruning; stale printer objects persist in AD if print servers are decommissioned.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningRetries", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetries")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningRetries", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-set-pruning-interval",
                    Label = "Set Printer Pruning Check Interval",
                    Category = "Printing",
                    Description =
                        "Sets PruningInterval=480 to check every 8 hours (480 minutes) whether printer objects in Active Directory "
                        + "should be pruned. The default check interval is every 8 hours; a longer interval reduces AD queries "
                        + "from the Directory Service pruning subsystem on domain controllers.",
                    Tags = ["printing", "active-directory", "pruning", "interval", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Sets pruning poll interval to 480 min; reduces printer-related AD query load.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningInterval", 480)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningInterval")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningInterval", 480)],
                },
                new TweakDef
                {
                    Id = "pdssp-set-pruning-priority",
                    Label = "Set Printer Pruning Thread Priority",
                    Category = "Printing",
                    Description =
                        "Sets PruningPriority=0 to run the printer pruning thread at low priority. "
                        + "Reduces CPU contention from the background AD pruning process on heavily loaded print servers "
                        + "and domain controllers that share hardware resources.",
                    Tags = ["printing", "active-directory", "pruning", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Low-priority pruning thread; negligible performance impact on print servers.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningPriority", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningPriority")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningPriority", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-log-pruning-events",
                    Label = "Enable Printer Pruning Event Logging",
                    Category = "Printing",
                    Description =
                        "Sets PruningRetryLog=1 to record printer pruning retry and failure events to the Windows Application event log. "
                        + "Provides audit visibility into AD printer object lifecycle events for SIEM ingestion and printer infrastructure monitoring.",
                    Tags = ["printing", "active-directory", "pruning", "logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Logs pruning events to Application log; minor increase in event log volume.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningRetryLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetryLog")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningRetryLog", 1)],
                },
                new TweakDef
                {
                    Id = "pdssp-disable-non-published-printer-access",
                    Label = "Block Access to Non-Published AD Printers",
                    Category = "Printing",
                    Description =
                        "Sets NonPublishedPrinters=0 to prevent users from connecting to network printers that are not published "
                        + "in Active Directory. Ensures all printer installations go through the AD Directory Services vetting process "
                        + "and prevents rogue or personal printers from being added via direct UNC paths.",
                    Tags = ["printing", "active-directory", "access-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks non-AD-published printer connections; only AD-listed printers can be installed.",
                    ApplyOps = [RegOp.SetDword(DsKey, "NonPublishedPrinters", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "NonPublishedPrinters")],
                    DetectOps = [RegOp.CheckDword(DsKey, "NonPublishedPrinters", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-disable-ipp-web-printing",
                    Label = "Disable IPP Web Printing via AD",
                    Category = "Printing",
                    Description =
                        "Sets DisableWebPrinting=1 to prevent users from installing printers via Internet Printing Protocol (IPP) "
                        + "URLs discovered through Active Directory. Web-based printer installation bypasses network printer "
                        + "deployment controls and may allow connection to external or untrusted print services.",
                    Tags = ["printing", "ipp", "web-printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks IPP web printer discovery via AD; direct \\server\\printer UNC paths still work.",
                    ApplyOps = [RegOp.SetDword(DsKey, "DisableWebPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "DisableWebPrinting")],
                    DetectOps = [RegOp.CheckDword(DsKey, "DisableWebPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "pdssp-set-server-thread-count",
                    Label = "Limit Printer DS Server Thread Count",
                    Category = "Printing",
                    Description =
                        "Sets ServerThread=2 to limit the number of concurrent threads used by the spooler for Active Directory "
                        + "printer publishing operations. Reducing thread count lowers CPU usage on print servers with many shared "
                        + "printers during AD bulk-publish events after policy refresh.",
                    Tags = ["printing", "active-directory", "performance", "threads", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Limits AD spooler threads to 2; larger environments may need higher values for timely publishing.",
                    ApplyOps = [RegOp.SetDword(DsKey, "ServerThread", 2)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "ServerThread")],
                    DetectOps = [RegOp.CheckDword(DsKey, "ServerThread", 2)],
                },
                new TweakDef
                {
                    Id = "pdssp-enforce-pre-publish-printers",
                    Label = "Enforce Pre-Publication of Printers to AD",
                    Category = "Printing",
                    Description =
                        "Sets PrePublishPrinters=1 to require printers to be pre-published to Active Directory before they "
                        + "become available to clients. Pre-publishing ensures printer metadata is available for directory browsing "
                        + "before the first client connection attempt, reducing discovery latency for distributed print deployments.",
                    Tags = ["printing", "active-directory", "publishing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Requires pre-publishing; AD objects created before printers accept connections.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PrePublishPrinters", 1)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PrePublishPrinters")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PrePublishPrinters", 1)],
                },
                new TweakDef
                {
                    Id = "pdssp-set-max-pruning-retries",
                    Label = "Set Maximum Printer Pruning Retry Count",
                    Category = "Printing",
                    Description =
                        "Sets PruningRetries=2 to limit the number of times the pruning mechanism retries an unreachable "
                        + "print server before removing its printer AD objects. A lower retry count speeds up cleanup of "
                        + "permanently decommissioned print servers while reducing spurious pruning from temporary outages.",
                    Tags = ["printing", "active-directory", "pruning", "retry", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 4,
                    ImpactNote = "Limits pruning retries to 2; printers may get pruned sooner after a short server outage.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningRetries", 2)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetries")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningRetries", 2)],
                },
            ];

    }

    // ── PrinterDriverIsolationPolicy ──
    private static class _PrinterDriverIsolationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DriverIsolation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pdrv-enforce-driver-isolation",
                    Label = "Enforce Printer Driver Isolation (Separate Process)",
                    Category = "Printing",
                    Description =
                        "Forces printer drivers to run in isolated processes separate from the spooler service, preventing a buggy or malicious printer driver from crashing or compromising the spooler.",
                    Tags = ["printing", "driver-isolation", "spooler", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Printer drivers run isolated from spooler; spooler crash impact from bad driver reduced.",
                    ApplyOps = [RegOp.SetDword(Key, "IsolationMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IsolationMode")],
                    DetectOps = [RegOp.CheckDword(Key, "IsolationMode", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-block-unsigned-drivers",
                    Label = "Block Installation of Unsigned Printer Drivers",
                    Category = "Printing",
                    Description =
                        "Blocks the installation of printer drivers that do not have a valid WHQL or enterprise certificate signature, preventing malicious or vulnerable unsigned printer drivers from loading.",
                    Tags = ["printing", "unsigned-driver", "security", "whql", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Unsigned printer drivers rejected; only WHQL or enterprise-signed drivers install.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUnsignedPrinterDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUnsignedPrinterDrivers")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUnsignedPrinterDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-limit-driver-install-to-admin",
                    Label = "Restrict Printer Driver Installation to Administrators Only",
                    Category = "Printing",
                    Description =
                        "Requires administrator privileges to install any new printer driver, preventing standard users from adding potentially exploitable printer drivers via easy-to-add printer workflows.",
                    Tags = ["printing", "driver-install", "admin", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot install printer drivers; admin credentials required.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDriverInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDriverInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDriverInstall", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-disable-v3-kernel-drivers",
                    Label = "Disable Legacy V3 Kernel-Mode Printer Drivers",
                    Category = "Printing",
                    Description =
                        "Disables legacy V3 (kernel-mode) printer drivers, allowing only V4 user-mode drivers which run isolated from the kernel and reduce the risk of privilege escalation via printer drivers.",
                    Tags = ["printing", "v3-driver", "kernel-mode", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "V3 kernel-mode printer drivers blocked; some older printers may require V4 driver wrappers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableV3KernelModePrinterDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableV3KernelModePrinterDrivers")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableV3KernelModePrinterDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-block-network-driver-download",
                    Label = "Block Automatic Printer Driver Download from Network",
                    Category = "Printing",
                    Description =
                        "Blocks Windows from automatically downloading printer drivers from remote print servers or Windows Update when a new printer is detected, requiring manual driver installation.",
                    Tags = ["printing", "auto-driver-download", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto printer driver download blocked; must manually install drivers before adding network printers.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAutoPrinterDriverDownload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoPrinterDriverDownload")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAutoPrinterDriverDownload", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-enable-enhanced-point-and-print",
                    Label = "Enable Enhanced Point and Print Restriction",
                    Category = "Printing",
                    Description =
                        "Enables enhanced Point and Print restrictions requiring that drivers originate from an approved printer server list, preventing attackers from serving malicious drivers via rogue print servers.",
                    Tags = ["printing", "point-and-print", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Point and Print restricted to approved servers; rogue print server attacks blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "Restricted", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Restricted")],
                    DetectOps = [RegOp.CheckDword(Key, "Restricted", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-disable-driver-update-prompt",
                    Label = "Disable Automatic Printer Driver Update Prompts",
                    Category = "Printing",
                    Description =
                        "Suppresses automatic driver update prompts from existing printer drivers via Windows Update, preventing unexpected printer driver updates that could introduce vulnerabilities.",
                    Tags = ["printing", "driver-update", "windows-update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Auto driver update prompts suppressed; printer drivers only update via manual action.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDriverUpdatePrompt", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverUpdatePrompt")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDriverUpdatePrompt", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-block-driver-staging-from-drivers-folder",
                    Label = "Block Driver Installation from Drivers Folder Without Inbox",
                    Category = "Printing",
                    Description =
                        "Prevents printer drivers from being installed from the Windows Drivers directory without being in the inbox driver store, blocking attack paths that stage evil drivers into the Drivers folder.",
                    Tags = ["printing", "driver-staging", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only inbox-store printer drivers installable; staged evil-driver attack paths closed.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockStagedDriverInstalls", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockStagedDriverInstalls")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockStagedDriverInstalls", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-disable-printer-driver-dcom",
                    Label = "Disable DCOM Access for Printer Driver Processes",
                    Category = "Printing",
                    Description =
                        "Prevents printer driver host processes from making DCOM calls to other processes, reducing lateral movement risk if a printer driver process is compromised.",
                    Tags = ["printing", "dcom", "driver-isolation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DCOM disabled for printer driver host processes; compromised driver cannot do COM-based lateral movement.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDCOMInDriverHost", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDCOMInDriverHost")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDCOMInDriverHost", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-log-driver-install-events",
                    Label = "Enable Audit Logging for Printer Driver Installs",
                    Category = "Printing",
                    Description =
                        "Enables security audit events whenever a printer driver is installed, updated, or removed, providing a log trail for detecting unauthorized driver installation activity.",
                    Tags = ["printing", "audit-log", "driver-install", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Printer driver installation events logged in Security event log; unauthorised installs detectable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditDriverInstallEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditDriverInstallEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditDriverInstallEvents", 1)],
                },
            ];

    }

    // ── PrinterGpoPolicy ──
    private static class _PrinterGpoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prtgpo-disable-print-spooler-sharing",
                Label = "Disable Printer Spooler Network Sharing",
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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

    // ── PrinterRedirectionPolicy ──
    private static class _PrinterRedirectionPolicy
    {
        private const string TsKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtred-disable-client-printer-redirect",
                    Label = "Printer Redirection: Disable Client Printer Redirection in RDS Sessions",
                    Category = "Printing",
                    Description =
                        "Sets fDisableCam=1 in Terminal Services policy. Prevents client printers from being automatically mapped into Remote Desktop Services sessions. When client printer redirection is enabled, every printer installed on the client machine is mapped into the RDS session as a session-specific printer. In large VDI deployments this creates hundreds of ghost printer objects per session host, causing significant spooler memory consumption, slow logon (each session must enumerate and map client printers), and instability. For environments where users should only print to central print servers, disabling client printer redirection is the recommended configuration.",
                    Tags = ["rds", "printer-redirection", "vdi", "rdp", "logon-speed"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Client local printers are not mapped into RDS/VDI sessions. Users print to centrally managed network printers deployed via Group Policy. Users with home printers or local USB printers cannot print from remote sessions. Best used when central print server coverage is complete.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableCam", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableCam")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableCam", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-enable-easy-print",
                    Label = "Printer Redirection: Enable Remote Desktop Easy Print Driver",
                    Category = "Printing",
                    Description =
                        "Sets UseUniversalPrinter=1 in Terminal Services policy. Enables the Remote Desktop Easy Print driver as the primary driver for redirected client printers. When Easy Print is enabled, redirected client printers use a single universal print driver on the session host rather than requiring the client's specific printer driver to be installed on every session host server. This eliminates the printer driver management burden of server-side driver installation: a 200-server RDS farm no longer needs every printer driver for every model used by clients. The Easy Print driver communicates rendering instructions to the client, which uses its own installed driver.",
                    Tags = ["rds", "easy-print", "universal-driver", "printer-redirection", "vdi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Redirected client printers use the Easy Print universal driver. Printer-specific features (stapling, booklet mode, duplex) may be unavailable through Easy Print. Print rendering is sent to the client; print quality is consistent with local direct printing.",
                    ApplyOps = [RegOp.SetDword(TsKey, "UseUniversalPrinter", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "UseUniversalPrinter")],
                    DetectOps = [RegOp.CheckDword(TsKey, "UseUniversalPrinter", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-set-printer-redirection-timeout-60s",
                    Label = "Printer Redirection: Set Printer Redirection Timeout to 60 Seconds",
                    Category = "Printing",
                    Description =
                        "Sets PrinterRedirectionTimeout=60 in Terminal Services policy. Sets the maximum wait time during session logon for redirected printers to become available. When client printer redirection is enabled, the session host waits for the RDP printer redirection channel to report all client printers before proceeding with logon. On slow WAN connections, printer enumeration over RDP can take tens of seconds. If the session host waits indefinitely, logon appears to hang. Setting a 60-second timeout ensures logon proceeds even if some client printers fail to enumerate, preventing printer redirection from delaying session startup.",
                    Tags = ["rds", "printer-redirection", "timeout", "logon-speed", "rdp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Printer redirection enumeration is limited to 60 seconds. Printers that do not enumerate within 60 seconds are not available in the session. User logon proceeds after 60 seconds regardless of printer redirection status. Slow WAN clients may see fewer redirected printers.",
                    ApplyOps = [RegOp.SetDword(TsKey, "PrinterRedirectionTimeout", 60)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "PrinterRedirectionTimeout")],
                    DetectOps = [RegOp.CheckDword(TsKey, "PrinterRedirectionTimeout", 60)],
                },
                new TweakDef
                {
                    Id = "prtred-disable-xps-redirection",
                    Label = "Printer Redirection: Disable XPS Printer Redirection",
                    Category = "Printing",
                    Description =
                        "Sets DisableXpsRedirection=1 in Terminal Services policy. Prevents the Microsoft XPS Document Writer virtual printer from being redirected into user sessions. The XPS Document Writer is a file-generation virtual printer: when a user 'prints' to it, a .XPS file is created on the user's local machine. In RDS sessions, redirected XPS printing places XPS files on the user's local machine through the RDP file system redirection channel. This creates a data exfiltration path: users on session hosts with sensitive application data can 'print' documents as XPS files and take them home. Disabling XPS redirection closes this path.",
                    Tags = ["rds", "xps-printer", "data-exfiltration", "restriction", "virtual-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "XPS Document Writer is not available in RDS sessions. Users cannot create XPS files by printing from within RDS sessions. Physical and network printers are unaffected.",
                    ApplyOps = [RegOp.SetDword(TsKey, "DisableXpsRedirection", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "DisableXpsRedirection")],
                    DetectOps = [RegOp.CheckDword(TsKey, "DisableXpsRedirection", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-restrict-auto-printer-creation",
                    Label = "Printer Redirection: Restrict Automatic Session Printer Creation to Default Only",
                    Category = "Printing",
                    Description =
                        "Sets LoadDriversForDefaultPrinterOnly=1 in Terminal Services policy. Limits automatic printer creation in RDS sessions to the client's default printer only, rather than all client printers. Mapping every client printer into every session is the primary cause of session host spooler memory exhaustion in large VDI farms. A user with 5 printers on their client machine causes 5 session-specific printer entries on every session host they connect to. 'Default printer only' mode preserves the one-click printing experience for the user's preferred printer while eliminating the overhead of mapping every lesser-used printer.",
                    Tags = ["rds", "printer-auto-creation", "default-printer", "vdi", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only the client's default printer is mapped into RDS sessions. Secondary client printers are not available in sessions unless manually added by the user. Significant reduction in session host spooler memory usage in VDI deployments.",
                    ApplyOps = [RegOp.SetDword(TsKey, "LoadDriversForDefaultPrinterOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "LoadDriversForDefaultPrinterOnly")],
                    DetectOps = [RegOp.CheckDword(TsKey, "LoadDriversForDefaultPrinterOnly", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-disable-pdf-printer-redirect",
                    Label = "Printer Redirection: Disable PDF Printer Redirection in RDS",
                    Category = "Printing",
                    Description =
                        "Sets DisablePDFRedirection=1 in Terminal Services policy. Prevents the Microsoft Print to PDF virtual printer from being redirected into RDS sessions. Microsoft Print to PDF, like XPS, is a file-generation virtual printer that creates PDF files on the user's local machine via the RDP file system redirection channel. This is an equally effective data exfiltration path: users can take sensitive documents from session hosts as PDF files. Enterprise DRM-protected documents that cannot be copied via clipboard or USB may still be 'printed' to local PDF files through this channel.",
                    Tags = ["rds", "pdf-printer", "data-exfiltration", "dlp", "virtual-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Microsoft Print to PDF is not available in RDS sessions. Users cannot create PDF files by printing from within RDS sessions. Physical and network printers are unaffected. Users relying on session-based PDF generation need an alternative (server-side PDF converter).",
                    ApplyOps = [RegOp.SetDword(TsKey, "DisablePDFRedirection", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "DisablePDFRedirection")],
                    DetectOps = [RegOp.CheckDword(TsKey, "DisablePDFRedirection", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-enable-bidirectional-communication",
                    Label = "Printer Redirection: Enable Bidirectional Printer Communication",
                    Category = "Printing",
                    Description =
                        "Sets BidiComm=1 in Terminal Services policy. Enables bidirectional (bidi) printer communication for redirected printers in RDS sessions. Bidi communication allows the session to query the printer's current status — toner levels, paper jam conditions, available paper sizes, and duplexing capability — from within the session. Without bidi, users cannot see printer status from their RDS session and the print driver cannot adapt to the printer's available options. Bidi requires the Easy Print driver path and the client to support bidi reporting.",
                    Tags = ["rds", "bidi", "printer-status", "toner", "bidirectional"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Redirected printers report toner, paper, and status information to the RDS session. Users see accurate printer capability information in print dialogs. Requires Easy Print driver and a printer with bidi reporting capability.",
                    ApplyOps = [RegOp.SetDword(TsKey, "BidiComm", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "BidiComm")],
                    DetectOps = [RegOp.CheckDword(TsKey, "BidiComm", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-set-max-redirected-printers-5",
                    Label = "Printer Redirection: Limit Redirected Printers Per Session to 5",
                    Category = "Printing",
                    Description =
                        "Sets MaxRedirectedPrinters=5 in Terminal Services policy. Caps the maximum number of client printers that can be redirected into a single RDS session. Without this limit, a user with 20+ printers installed (e.g., a power user with many VPN-connected branch printers) will have all 20 mapped into every session — consuming substantial memory and logon time on the session host server. Limiting to 5 redirected printers covers virtually all legitimate printing needs while preventing excessive session host resource consumption from clients with large printer inventories.",
                    Tags = ["rds", "printer-limit", "session", "performance", "resource"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "At most 5 client printers are mapped per RDS session. Clients with more than 5 printers have their first 5 (by enumeration order) mapped. Users rarely need more than 5 printers in a session. Configure in conjunction with default-printer-only policy for maximum benefit.",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxRedirectedPrinters", 5)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxRedirectedPrinters")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxRedirectedPrinters", 5)],
                },
                new TweakDef
                {
                    Id = "prtred-use-compression-for-print-data",
                    Label = "Printer Redirection: Enable Compression for Redirected Print Data",
                    Category = "Printing",
                    Description =
                        "Sets CompressPrintData=1 in Terminal Services policy. Enables compression of print job data transmitted through the RDP printer redirection channel. Print job data (especially EMF) can be highly compressible — text-heavy documents may compress by 80%+. Without compression, printing large documents over WAN-connected RDS sessions consumes significant RDP session bandwidth. With compression enabled, the RDP virtual channel compresses the print data stream before transmission, reducing the bandwidth and time required to print large documents over slow connections.",
                    Tags = ["rds", "print-compression", "bandwidth", "wan", "rdp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Print data is compressed before transmission through the RDP channel. Significant bandwidth savings for text-heavy documents over WAN connections. Minor CPU overhead on the session host for compression. No user-visible impact.",
                    ApplyOps = [RegOp.SetDword(TsKey, "CompressPrintData", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "CompressPrintData")],
                    DetectOps = [RegOp.CheckDword(TsKey, "CompressPrintData", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-allow-only-easy-print-fallback",
                    Label = "Printer Redirection: Use Easy Print as Exclusive Fallback Driver",
                    Category = "Printing",
                    Description =
                        "Sets FallbackToEasyPrint=1 in Terminal Services policy. Configures RDS to use the Easy Print driver as the exclusive fallback when the client printer's specific driver is not installed on the session host. Without this setting, if the specific printer driver is absent, redirection may fail entirely or attempt to download the driver automatically. With FallbackToEasyPrint enabled, any printer whose driver is not on the server falls back to Easy Print — ensuring the printer is always usable even if not optimally configured. Eliminates 'Printer unavailable' errors from driver-absent conditions.",
                    Tags = ["rds", "easy-print", "fallback-driver", "printer-availability", "rdp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Printers without a matching server-side driver fall back to Easy Print. All client printers are available in sessions, potentially with reduced feature sets. Eliminates printer redirection failures due to missing drivers without requiring driver installation on session hosts.",
                    ApplyOps = [RegOp.SetDword(TsKey, "FallbackToEasyPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "FallbackToEasyPrint")],
                    DetectOps = [RegOp.CheckDword(TsKey, "FallbackToEasyPrint", 1)],
                },
            ];

    }

    // ── PrintJobManagementPolicy ──
    private static class _PrintJobManagementPolicy
    {
        private const string JobKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\JobManagement";

        private const string PrtKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtjob-purge-jobs-on-restart",
                    Label = "Print Job Management: Purge All Print Jobs on Spooler Restart",
                    Category = "Printing",
                    Description =
                        "Sets PurgeJobsOnRestart=1 in JobManagement policy. Clears all pending print jobs from all print queues when the Print Spooler service restarts. By default, the spooler preserves queued jobs across restarts, which can cause problems when a restarted spooler encounters corrupted spool files (EMF or RAW) from a failed previous session — leading to an infinite loop where the spooler starts, crashes processing a bad job, and restarts. Purging on restart ensures the spooler always starts with a clean queue. Lost jobs must be resubmitted by users.",
                    Tags = ["print-job", "spooler-restart", "queue-purge", "stability", "recovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Pending print jobs are lost when the spooler restarts (service restart, machine reboot). Users must resubmit their print jobs. Prevents spooler crash loops caused by corrupted spool files. Useful on print servers with history of spooler instability.",
                    ApplyOps = [RegOp.SetDword(JobKey, "PurgeJobsOnRestart", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "PurgeJobsOnRestart")],
                    DetectOps = [RegOp.CheckDword(JobKey, "PurgeJobsOnRestart", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-set-max-spool-file-size-1gb",
                    Label = "Print Job Management: Set Maximum Spool File Size to 1 GB",
                    Category = "Printing",
                    Description =
                        "Sets MaxSpoolFileSize=1073741824 in JobManagement policy (1 GB in bytes). Sets the maximum allowed size for individual print spool files. Without a spool file size limit, a single print job (e.g., a 10,000-page CAD print run or a large PDF) can generate a spool file that consumes all available disk space on the print server, starving all other users' jobs. 1 GB is sufficient for most large-format print jobs while protecting against runaway spool generation. Jobs exceeding the limit are rejected with a 'Spool file too large' error.",
                    Tags = ["print-job", "spool-file", "disk-space", "limit", "print-server"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Print jobs exceeding 1 GB spool file size are rejected. Very large print jobs (high-resolution CAD, book-length PDFs) may need to be split into smaller jobs. Protects print server disk from runaway spool consumption.",
                    ApplyOps = [RegOp.SetDword(JobKey, "MaxSpoolFileSize", 1073741824)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "MaxSpoolFileSize")],
                    DetectOps = [RegOp.CheckDword(JobKey, "MaxSpoolFileSize", 1073741824)],
                },
                new TweakDef
                {
                    Id = "prtjob-enable-separator-page",
                    Label = "Print Job Management: Enable Separator Page Between Print Jobs",
                    Category = "Printing",
                    Description =
                        "Sets UseSeparatorPage=1 in JobManagement policy. Enables job separator pages (banner pages) between print jobs. A separator page is a printed page inserted before each job containing: user name, date, time, and job ID. In shared printer environments, separator pages allow users to find their document among others' output in the printer tray output bin. Without separator pages, documents from multiple users in a busy shared printer pile together, causing users to accidentally take others' confidential documents — a physical information disclosure risk.",
                    Tags = ["print-job", "separator-page", "banner-page", "physical-security", "shared-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "A separator page is printed before each job. One additional page of toner consumed per job. Users can identify their documents in the output tray. Separator page format is configured per-printer (PCL, PostScript, Windows default).",
                    ApplyOps = [RegOp.SetDword(JobKey, "UseSeparatorPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "UseSeparatorPage")],
                    DetectOps = [RegOp.CheckDword(JobKey, "UseSeparatorPage", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-set-job-expiry-8hours",
                    Label = "Print Job Management: Expire Unprinted Jobs After 8 Hours",
                    Category = "Printing",
                    Description =
                        "Sets JobExpiryHours=8 in JobManagement policy. Automatically removes print jobs that have been queued but not processed (printed) within 8 hours. Jobs can accumulate in a queue when a printer is taken offline, goes into an error state, or is deliberately paused. Without expiry, a queue can accumulate hundreds of stale jobs — some of which may contain sensitive documents submitted by users who no longer need them. 8 hours aligns with a standard business day — a job submitted in the morning and not printed by end of day is auto-purged.",
                    Tags = ["print-job", "expiry", "queue-management", "security", "cleanup"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Print jobs not printed within 8 hours are automatically removed. Users must resubmit jobs if the printer was unavailable for more than 8 hours. Prevents accumulation of stale documents in print queues.",
                    ApplyOps = [RegOp.SetDword(JobKey, "JobExpiryHours", 8)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "JobExpiryHours")],
                    DetectOps = [RegOp.CheckDword(JobKey, "JobExpiryHours", 8)],
                },
                new TweakDef
                {
                    Id = "prtjob-disable-interactive-print-sharing",
                    Label = "Print Job Management: Disable Interactive Console Print Sharing",
                    Category = "Printing",
                    Description =
                        "Sets DisableInteractivePrinterSharing=1 in Printers policy. Prevents users from interactively sharing printers through the Windows Printer Properties dialog. Without this restriction, any local user can share their local printer to the network — creating unmanaged, unmonitored print shares that bypass central print server controls. Printer sharing should only be managed through Group Policy printer deployment or by administrators. Unmanaged printer shares can also have misconfigured permissions, allowing unauthenticated network print access.",
                    Tags = ["print-job", "printer-sharing", "management", "control", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "The 'Share this printer' checkbox in Printer Properties is disabled. Users cannot share their local printers to the network. Print sharing is only possible by administrators via Print Management or Group Policy.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableInteractivePrinterSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableInteractivePrinterSharing")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableInteractivePrinterSharing", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-enforce-default-queue-priority",
                    Label = "Print Job Management: Enforce Default Queue Priority Level (49)",
                    Category = "Printing",
                    Description =
                        "Sets DefaultPriority=49 in JobManagement policy. Sets the default print job priority to 49 (scale of 1-99, where 99 is highest). When a user does not specify a priority or when they have priority escalation rights, print jobs default to priority 49. This ensures administrators can designate executive or time-critical queues with priority 50+ that will always preempt standard user jobs. Without a defined default, systems may inherit OS defaults that vary between Windows versions, making priority management unpredictable.",
                    Tags = ["print-job", "priority", "queue-management", "fairness", "scheduling"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Standard user print jobs use priority 49. Priority 50-99 reserved for administrator-managed high-priority queues. No change in observed behaviour for most users — priority ordering is internal to the print queue scheduler.",
                    ApplyOps = [RegOp.SetDword(JobKey, "DefaultPriority", 49)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "DefaultPriority")],
                    DetectOps = [RegOp.CheckDword(JobKey, "DefaultPriority", 49)],
                },
                new TweakDef
                {
                    Id = "prtjob-set-spool-directory-to-secured",
                    Label = "Print Job Management: Set Secure Spool Directory ACL Enforcement",
                    Category = "Printing",
                    Description =
                        "Sets SecureSpoolDirectory=1 in JobManagement policy. Enables ACL enforcement on the print spool directory (%SystemRoot%\\System32\\spool\\PRINTERS). By default this directory has permissive ACLs that allow any authenticated user to read or delete spool files. Spool files contain the raw or EMF rendering of documents being printed — reading them is equivalent to reading the document. With SecureSpoolDirectory enabled, only the SYSTEM account and print administrators can read spool files. Standard users cannot access other users' spool files.",
                    Tags = ["print-job", "spool-directory", "acl", "file-security", "information-disclosure"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "The print spool directory is protected with restrictive ACLs. Standard users cannot read or delete other users' spool files. Third-party print monitoring software that reads spool files directly may require the SYSTEM or print administrator context.",
                    ApplyOps = [RegOp.SetDword(JobKey, "SecureSpoolDirectory", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "SecureSpoolDirectory")],
                    DetectOps = [RegOp.CheckDword(JobKey, "SecureSpoolDirectory", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-disable-printer-status-popup",
                    Label = "Print Job Management: Disable Print Status Notification Popups",
                    Category = "Printing",
                    Description =
                        "Sets DisablePrinterstatusNotifications=1 in JobManagement policy. Prevents the print status notification system tray balloon and popup messages from appearing when a print job completes successfully. In enterprise environments with high print volumes, completed print job notifications are a source of notification fatigue — users who print dozens of documents per day receive an equal number of transient notifications that they learn to dismiss immediately. Disabling successful-completion notifications reduces noise; error notifications (failure, out of paper) are separately configurable and should remain enabled.",
                    Tags = ["print-job", "notifications", "user-experience", "task-bar", "status"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Print job completion notifications are suppressed in the system tray. Users are not notified when their document successfully prints. Error notifications (print failure, printer offline) can be separately enabled. Reduces notification clutter in high-volume printing environments.",
                    ApplyOps = [RegOp.SetDword(JobKey, "DisablePrinterstatusNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "DisablePrinterstatusNotifications")],
                    DetectOps = [RegOp.CheckDword(JobKey, "DisablePrinterstatusNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-enable-emf-spool-format",
                    Label = "Print Job Management: Use Enhanced Metafile (EMF) Spooling Format",
                    Category = "Printing",
                    Description =
                        "Sets UseEMFSpool=1 in JobManagement policy. Configures the print spooler to spool print jobs in Enhanced Metafile (EMF) format rather than the RAW (device-ready) format. EMF spooling returns control to the application faster — the application finishes its print call as soon as the EMF commands are written to the spool file, rather than waiting for the full rasterisation to the printer's native format. The spooler then renders EMF to RAW in the background. Faster application hand-off is the primary benefit; the trade-off is that EMF rendering errors are deferred to the spooler.",
                    Tags = ["print-job", "emf", "spool-format", "performance", "application-responsiveness"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Print calls return to the application faster. Rendering errors may not surface until after the application has completed the print call. EMF spool files are larger than RAW until rendered; more temporary disk space is needed during active large print jobs.",
                    ApplyOps = [RegOp.SetDword(JobKey, "UseEMFSpool", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "UseEMFSpool")],
                    DetectOps = [RegOp.CheckDword(JobKey, "UseEMFSpool", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-block-untrusted-printer-fonts",
                    Label = "Print Job Management: Block Untrusted Fonts in Print Jobs",
                    Category = "Printing",
                    Description =
                        "Sets BlockUntrustedFonts=1 in JobManagement policy. Blocks loading of fonts from untrusted sources within print job processing. The Windows font parsing subsystem has historically been a high-value attack target — multiple CVEs involve malformed fonts causing kernel memory corruption during parsing. Print jobs submitted from remote clients can contain embedded fonts. By blocking fonts that are not installed in the trusted Windows font store, the attack surface for font-based exploitation via print jobs is reduced. Print jobs with embedded, untrusted fonts may render with fallback system fonts.",
                    Tags = ["print-job", "font", "untrusted-fonts", "kernel", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Print jobs containing fonts not in the system font store render with fallback fonts. Documents with custom branding fonts may look different when printed if those fonts are not installed on the print server. Impact visible on print servers processing documents with embedded non-standard fonts.",
                    ApplyOps = [RegOp.SetDword(JobKey, "BlockUntrustedFonts", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "BlockUntrustedFonts")],
                    DetectOps = [RegOp.CheckDword(JobKey, "BlockUntrustedFonts", 1)],
                },
            ];

    }

    // ── PrintManagementPolicy ──
    private static class _PrintManagementPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PrintManagement";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prtmgmt-disable-mmc",
                Label = "Disable Print Management MMC Console",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The Print Management MMC console provides a graphical interface for managing printers, print queues, and printer servers across an organization. Disabling the Print Management MMC console prevents users and non-print-admin accounts from accessing this powerful management interface. Centralized print management is handled by designated print server administrators rather than individual workstation users. Restricting access to the MMC prevents accidental or unauthorized printer configuration changes. Printer management tasks are delegated to specialized IT staff through proper RBAC mechanisms. Standard print functionality including printing documents and managing local print jobs remains fully accessible to users.",
                Tags = ["printing", "management", "mmc", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintManagementMmc", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintManagementMmc")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintManagementMmc", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-driver-autoinstall",
                Label = "Disable Printer Driver Auto-Install",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Printer driver auto-installation automatically downloads and installs printer drivers when a network printer is connected or added. Disabling auto-installation prevents arbitrary printer drivers from being installed from network sources without administrator approval. The Windows print driver ecosystem has historically been a significant source of privilege escalation vulnerabilities including PrintNightmare. Requiring administrator approval for all printer driver installations enforces a curated and tested driver baseline. Enterprise print environments deploy standardized, approved driver packages through SCCM or similar management platforms. Blocking automatic driver installation is a key mitigation for print spooler attack vectors on managed endpoints.",
                Tags = ["printing", "drivers", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrinterDriverAutoInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterDriverAutoInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrinterDriverAutoInstall", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-default-mgmt",
                Label = "Disable Default Printer Management",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows automatically manages the default printer by changing it to the most recently used printer when the dynamic default printer setting is active. Disabling default printer management through policy prevents Windows from automatically changing the configured default printer. Enterprise print environments rely on precise default printer assignments tied to physical location or department, which should not be dynamically changed. Automatic default printer changes cause user confusion and support calls when the wrong printer is selected for important documents. Lock-in of the default printer to a specific device is commonly required for compliance and operational workflow reasons. Disabling dynamic default management ensures the IT-configured default printer assignment remains stable.",
                Tags = ["printing", "default-printer", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDefaultPrinterManagement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDefaultPrinterManagement")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDefaultPrinterManagement", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-queue-sharing",
                Label = "Disable Print Queue Sharing",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Print queue sharing allows workstations to act as print servers by sharing locally connected printers to other network clients. Disabling queue sharing prevents workstations from hosting shared print queues, consolidating print spooler exposure to designated print servers. Workstation-based print sharing expands the print spooler attack surface to a larger number of targets on the network. Enterprise printing should be managed through dedicated print servers with hardened configurations and regular patching. Reducing the number of hosts running print server functionality limits the blast radius of print-related vulnerabilities. Centralized print servers with properly configured access controls provide superior audit and management capabilities compared to peer-to-peer sharing.",
                Tags = ["printing", "sharing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableQueueSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableQueueSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableQueueSharing", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-print-pdf-rdp",
                Label = "Disable Print to PDF from RDP",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Print to PDF from Remote Desktop sessions allows users to redirect print jobs from remote desktop sessions to local PDF files on the connecting client. Disabling this feature prevents document exfiltration through the Print to PDF mechanism in RDP sessions. Sensitive documents viewed in remote desktop sessions can be saved locally on unauthorized or unmanaged client devices via PDF redirection. Data governance policies require that documents accessed through remote desktop remain on the enterprise endpoint and are not redirected to potentially unmanaged clients. Print redirection in general represents a data movement risk in RDP scenarios handling confidential information. Organizations with strict data residency requirements should disable all print redirection in remote desktop sessions.",
                Tags = ["printing", "rdp", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintToPdfFromRdp", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToPdfFromRdp")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintToPdfFromRdp", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-telemetry",
                Label = "Disable Print Management Telemetry",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Print management telemetry reports usage statistics about printer configurations, print job characteristics, and driver versions to Microsoft. This data helps improve printer compatibility and print subsystem performance in future Windows releases. Disabling print management telemetry prevents printer configuration and usage data from being transmitted outside the enterprise. Print infrastructure details including printer models, drivers, and queue configurations represent sensitive IT asset information. Enterprise print environment information should not be disclosed through telemetry to external parties. Print functionality is completely unaffected by disabling this telemetry stream.",
                Tags = ["printing", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintManagementTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintManagementTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintManagementTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-discovery",
                Label = "Disable Printer Discovery",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Printer discovery uses WS-Discovery, mDNS, and network broadcast protocols to automatically find printers on the local network. Disabling printer discovery prevents workstations from automatically enumerating network printers and presenting them to users for installation. Enterprise printer deployments are managed through Group Policy printer deployment, not automatic discovery. Automatic printer discovery can expose users to rogue printers or allow installation of unapproved printer devices. Centrally managed printer policies ensure users only have access to approved printers with validated drivers. Disabling discovery does not remove previously installed network printers from the device.",
                Tags = ["printing", "discovery", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrinterDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrinterDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-xps-writer",
                Label = "Disable Microsoft XPS Document Writer",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The Microsoft XPS Document Writer is a virtual printer that saves print output as XPS format files on the local filesystem. Disabling the XPS Document Writer prevents users from saving print jobs as local XPS files, limiting an alternate document export path. XPS files created through the virtual printer can bypass DLP controls that monitor file creation through standard save dialogs. In environments where Microsoft Print to PDF is also disabled, disabling the XPS writer eliminates file-based print redirection. Enterprise document management workflows should use approved document export paths rather than virtual printer mechanisms. Disabling this virtual printer reduces the attack surface while preserving all physical printer functionality.",
                Tags = ["printing", "xps", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMicrosoftXpsDocumentWriter", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMicrosoftXpsDocumentWriter")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMicrosoftXpsDocumentWriter", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-internet-printing",
                Label = "Disable Internet Printing",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Internet Printing Protocol (IPP) allows clients to submit print jobs to remote printers over HTTP connections including those on the public internet. Disabling internet printing prevents the Windows print spooler from acting as an IPP client or accepting IPP-based print requests from the internet. Internet-accessible print services represent an attack vector used in PrintNightmare and related print spooler exploitation chains. Enterprise printers are managed on segmented internal networks and should not be accessible or reachable via internet-routable paths. Removing internet printing capability eliminates printer-based lateral movement vectors used in network attacks. All internal network printing through standard SMB and IPP on the corporate LAN remains unaffected.",
                Tags = ["printing", "internet", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInternetPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetPrinting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInternetPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-cloud-print-sharing",
                Label = "Disable Cloud Print Sharing",
                Category = "Printing",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Cloud print sharing allows enterprise printers to be shared through Microsoft's cloud printing infrastructure for access from mobile devices and remote workers. Disabling cloud print sharing prevents printers from being registered with cloud print services and accessed through cloud endpoints. Cloud-shared printers send print jobs through Microsoft's cloud infrastructure, which creates data residency and confidentiality concerns for sensitive documents. Organizations handling classified, regulated, or sensitive documents should ensure all print paths remain within the enterprise boundary. Cloud print sharing functionality is appropriate for consumer scenarios but requires careful data governance evaluation for enterprise use. Disabling cloud print sharing ensures all print data stays within the physical enterprise network infrastructure.",
                Tags = ["printing", "cloud", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudPrintSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPrintSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudPrintSharing", 1)],
            },
        ];

    }

    // ── PrintQueuePolicy ──
    private static class _PrintQueuePolicy
    {
        private const string PrtKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        private const string RpcKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\RPC";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtq-disable-spooler-on-non-print-servers",
                    Label = "Print Queue: Disable Print Spooler Service on Non-Print Servers",
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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

    // ── PrintSpoolAdvPolicy ──
    private static class _PrintSpoolAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prtspool-disable-point-and-print-unrestricted",
                Label = "Disable Unrestricted Point and Print Driver Installation",
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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

    // ── PrintSpoolerAdvancedPolicy ──
    private static class _PrintSpoolerAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id           = "spladv-disable-print-spooler",
                Label        = "Disable Print Spooler Service",
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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

    // ── PrintSpoolerPolicy ──
    private static class _PrintSpoolerPolicy
    {
        private const string SpoolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
        private const string PnPKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "prtspool-block-km-printer-drivers",
                Label = "Block Kernel-Mode Printer Drivers",
                Category = "Printing",
                Description = "Prevents kernel-mode printer drivers from loading in the Windows print spooler process (PrintNightmare mitigation).",
                Tags = ["print-spooler", "kernel-mode", "driver", "printnightmare", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Critical PrintNightmare fix; some legacy printer hardware may require KM drivers and stop working.",
                ApplyOps = [RegOp.SetDword(SpoolKey, "KMPrintersAreBlocked", 1)],
                RemoveOps = [RegOp.DeleteValue(SpoolKey, "KMPrintersAreBlocked")],
                DetectOps = [RegOp.CheckDword(SpoolKey, "KMPrintersAreBlocked", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-disable-remote-rpc",
                Label = "Disable Remote Print Spooler RPC Endpoint",
                Category = "Printing",
                Description = "Disables the remote RPC endpoint of the print spooler, preventing remote printer enumeration and exploitation.",
                Tags = ["print-spooler", "rpc", "remote", "printnightmare", "network", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Value 2 disables remote spooler; breaks network printer sharing from this machine but blocks remote exploitation.",
                ApplyOps = [RegOp.SetDword(SpoolKey, "RegisterSpoolerRemoteRpcEndPoint", 2)],
                RemoveOps = [RegOp.DeleteValue(SpoolKey, "RegisterSpoolerRemoteRpcEndPoint")],
                DetectOps = [RegOp.CheckDword(SpoolKey, "RegisterSpoolerRemoteRpcEndPoint", 2)],
            },
            new TweakDef
            {
                Id = "prtspool-require-signed-copy-files",
                Label = "Require Signed Copy Files for PnP Printers",
                Category = "Printing",
                Description = "Restricts printer driver copy-files during PnP association to only allow digitally signed drivers.",
                Tags = ["print-spooler", "copy-files", "pnp", "signed-driver", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Value 1 = allow copy-files only from signed drivers; blocks unsigned driver payloads used in PrintNightmare.",
                ApplyOps = [RegOp.SetDword(SpoolKey, "CopyFilesPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(SpoolKey, "CopyFilesPolicy")],
                DetectOps = [RegOp.CheckDword(SpoolKey, "CopyFilesPolicy", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-disable-web-pnp-download",
                Label = "Disable Printer Driver Download from Windows Update",
                Category = "Printing",
                Description = "Prevents Windows from automatically downloading printer drivers from Windows Update during PnP printer installation.",
                Tags = ["print-spooler", "windows-update", "pnp", "driver-download", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes an untrusted driver download path; IT must manually approve and supply printer drivers instead.",
                ApplyOps = [RegOp.SetDword(SpoolKey, "DisableWebPnPDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(SpoolKey, "DisableWebPnPDownload")],
                DetectOps = [RegOp.CheckDword(SpoolKey, "DisableWebPnPDownload", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-disable-http-printing",
                Label = "Disable Printing over HTTP",
                Category = "Printing",
                Description = "Blocks the Windows print spooler from connecting to or using printers over the HTTP/IPP protocol.",
                Tags = ["print-spooler", "http", "ipp", "network", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Eliminates unauthenticated HTTP printing surface; affects cloud print services that use HTTP transport.",
                ApplyOps = [RegOp.SetDword(SpoolKey, "DisableHTTPPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(SpoolKey, "DisableHTTPPrinting")],
                DetectOps = [RegOp.CheckDword(SpoolKey, "DisableHTTPPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-pnp-restrict-to-admins",
                Label = "Restrict Point and Print Driver Installation to Admins",
                Category = "Printing",
                Description = "Requires administrator privileges to install printer drivers via Point and Print, regardless of the print server.",
                Tags = ["print-spooler", "point-and-print", "admin", "driver", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Key PrintNightmare mitigation; non-admin users cannot install printer drivers via PnP to any server.",
                ApplyOps = [RegOp.SetDword(PnPKey, "Restricted", 1)],
                RemoveOps = [RegOp.DeleteValue(PnPKey, "Restricted")],
                DetectOps = [RegOp.CheckDword(PnPKey, "Restricted", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-pnp-no-trusted-servers",
                Label = "Disable Trusted Print Server Exemption for Point and Print",
                Category = "Printing",
                Description = "Removes the trusted print server list exemption, requiring admin-level approval for ALL Point and Print servers.",
                Tags = ["print-spooler", "point-and-print", "trusted-servers", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "No servers are automatically trusted for PnP driver download; all require explicit admin consent.",
                ApplyOps = [RegOp.SetDword(PnPKey, "TrustedServers", 0)],
                RemoveOps = [RegOp.DeleteValue(PnPKey, "TrustedServers")],
                DetectOps = [RegOp.CheckDword(PnPKey, "TrustedServers", 0)],
            },
            new TweakDef
            {
                Id = "prtspool-pnp-no-forest-trust",
                Label = "Disable Forest-Level Trust for Point and Print",
                Category = "Printing",
                Description = "Disables the implicit trust granted to print servers in the same Active Directory forest for Point and Print.",
                Tags = ["print-spooler", "point-and-print", "forest", "ad", "trust", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Forest-member print servers no longer bypass driver installation prompts; treats all servers equally.",
                ApplyOps = [RegOp.SetDword(PnPKey, "InForest", 0)],
                RemoveOps = [RegOp.DeleteValue(PnPKey, "InForest")],
                DetectOps = [RegOp.CheckDword(PnPKey, "InForest", 0)],
            },
            new TweakDef
            {
                Id = "prtspool-pnp-elevate-driver-update",
                Label = "Require Elevation When Updating Printer Drivers via PnP",
                Category = "Printing",
                Description = "Forces a UAC elevation prompt when an existing printer driver is updated via Point and Print.",
                Tags = ["print-spooler", "point-and-print", "uac", "elevation", "update", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Value 2 = always require elevation for driver updates; prevents silent malicious driver replacement.",
                ApplyOps = [RegOp.SetDword(PnPKey, "UpdatePromptSettings", 2)],
                RemoveOps = [RegOp.DeleteValue(PnPKey, "UpdatePromptSettings")],
                DetectOps = [RegOp.CheckDword(PnPKey, "UpdatePromptSettings", 2)],
            },
            new TweakDef
            {
                Id = "prtspool-pnp-elevate-driver-install",
                Label = "Require Elevation When Installing New Printer Drivers via PnP",
                Category = "Printing",
                Description = "Forces a UAC elevation prompt when a new printer driver is installed via Point and Print.",
                Tags = ["print-spooler", "point-and-print", "uac", "elevation", "install", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Value 2 = always require elevation for new driver installs; closes a common PrintNightmare attack vector.",
                ApplyOps = [RegOp.SetDword(PnPKey, "InstallDriverPromptSetting", 2)],
                RemoveOps = [RegOp.DeleteValue(PnPKey, "InstallDriverPromptSetting")],
                DetectOps = [RegOp.CheckDword(PnPKey, "InstallDriverPromptSetting", 2)],
            },
        ];

    }

    // ── PrintSpoolerSecurity ──
    private static class _PrintSpoolerSecurity
    {
        private const string Spooler = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler";

        private const string SpoolerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        private const string SpoolerPointAndPrint = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        private const string PrintNightmare = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Management";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "spool-disable-spooler-service",
                Label = "Disable Print Spooler Service (Non-Print Servers/Workstations)",
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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
                Category = "Printing",
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

    // ── PrintSpoolFinalPolicy ──
    private static class _PrintSpoolFinalPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Cleanup";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "splfinal-enable-print-spooler-cleanup-on-idle",
                Label = "Enable Automatic Print Spooler Cleanup When Print Queue Is Idle",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description = "Enabling automatic print spooler cleanup when the print queue is idle removes completed print jobs and temporary spool files from the spooler directory ensuring that document content is not retained in the spool longer than necessary for the print operation. Print spool files contain document images in EMF or RAW format that may include sensitive content and should be removed promptly after the print job completes to minimize exposure. Automatic cleanup on idle conditions ensures that print spool data is cleared during normal operational periods without requiring administrative intervention for routine spool maintenance. Spool file cleanup reduces the attack surface on print servers by minimizing the window during which attackers can access spool files to recover document content. Organizations should verify that spool cleanup policies are applied consistently on all print servers and workstations with local print queues. Spool cleanup events should be logged to provide evidence that print data was disposed of appropriately for compliance reporting purposes.",
                Tags = ["print-spooler", "cleanup", "spool-files", "data-retention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSpoolCleanupOnIdle", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolCleanupOnIdle")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSpoolCleanupOnIdle", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-enforce-immediate-spool-file-deletion",
                Label = "Enforce Immediate Deletion of Print Spool Files After Job Completion",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description = "Enforcing immediate deletion of print spool files upon job completion eliminates the retention window during which print spool data would otherwise be recoverable from the spool directory on print servers and workstations. Immediate spool deletion is a defense against forensic recovery of document content from print infrastructure that has been accessed by an attacker. Organizations handling sensitive information under regulatory requirements may need to implement immediate spool deletion to satisfy data minimization requirements for printed document data. Immediate deletion should be applied to all stages of the print spool including temporary intermediate files generated during EMF to device format conversion. The deletion operation should be verified to ensure files are actually removed rather than simply marked for deletion by the file system. Secure deletion using file overwrite operations rather than simple deletion should be considered for high-security environments where forensic recovery of spool data poses a significant risk.",
                Tags = ["print-spooler", "immediate-deletion", "spool-files", "secure-disposal", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceImmediateSpoolFileDeletion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceImmediateSpoolFileDeletion")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceImmediateSpoolFileDeletion", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-restrict-orphan-spool-file-retention",
                Label = "Restrict Retention of Orphaned Print Spool Files to Mandatory Cleanup Period",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description = "Orphaned print spool files resulting from failed or interrupted print jobs are retained in the spool directory indefinitely without automatic cleanup which creates unnecessary data accumulation and potential sensitive data exposure. Restricting orphaned spool file retention period to a maximum defined duration ensures that print data from failed jobs is automatically removed within a predictable timeframe. Long-term retention of orphaned spool files on print servers can accumulate large volumes of sensitive document data from all users who have sent print jobs to the server. Cleanup of orphaned spool files should be automated through the print spooler service rather than relying on manual administrator cleanup which may not occur regularly. The retention period for orphaned spool files should be set based on the sensitivity of the documents typically printed in the environment with shorter periods for environments processing sensitive regulated data. Cleanup operations for orphaned spool files should be logged to provide an audit trail of data disposal activities.",
                Tags = ["print-spooler", "orphaned-files", "cleanup", "spool-retention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "OrphanedSpoolFileRetentionHours", 24)],
                RemoveOps = [RegOp.DeleteValue(Key, "OrphanedSpoolFileRetentionHours")],
                DetectOps = [RegOp.CheckDword(Key, "OrphanedSpoolFileRetentionHours", 24)],
            },
            new TweakDef
            {
                Id = "splfinal-enable-secure-spool-file-overwrite",
                Label = "Enable Secure Multi-Pass Overwrite for Print Spool File Deletion",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description = "Enabling secure overwrite for spool files replaces the content of spool files with random data before deletion ensuring that the document data is irrecoverable from the storage media through standard data recovery utilities. Simple deletion of spool files marks the file system entry as free but does not overwrite the underlying disk sectors leaving document content recoverable until those sectors are reused by other files. Organizations that process classified or highly sensitive documents using print infrastructure should implement secure overwrite for spool files to satisfy media sanitization requirements. The performance impact of secure overwrite operations on print servers is generally low because spool files are relatively small but the impact should be tested before deployment in high-volume print environments. Secure overwrite should be applied to all temporary files generated during the print rendering process including intermediate format conversion files that may contain partial document images. Compliance documentation for sensitive data handling programs should reference secure spool file deletion as a control contributing to data disposal assurance.",
                Tags = ["print-spooler", "secure-overwrite", "data-sanitization", "spool-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSecureSpoolFileOverwrite", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSecureSpoolFileOverwrite")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSecureSpoolFileOverwrite", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-audit-spool-directory-access",
                Label = "Enable Audit Logging for Print Spool Directory File System Access Events",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description = "Enabling audit logging for print spool directory access events records all reads writes and deletions of files in the print spool directory providing visibility into unauthorized access to spool data by processes other than the print spooler service. Unauthorized access to the print spool directory by non-spooler processes may indicate malware attempting to read document content from spool files or an attacker harvesting document data. Access to spool directory files should be restricted to the Print Spooler service and local SYSTEM account with all other access attempts generating security audit events. Spool directory access audit events should be reviewed for access by unusual processes or user identities that do not have legitimate access needs. Security audit rules for the spool directory should be configured at the object access audit level to capture both successful and failed access attempts. Spool directory access audit data should be forwarded to SIEM for correlation with other endpoint security events to identify malicious access patterns.",
                Tags = ["print-spooler", "spool-directory", "audit-logging", "file-access", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSpoolDirectoryAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSpoolDirectoryAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSpoolDirectoryAccess", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-restrict-spool-directory-permissions",
                Label = "Restrict File System Permissions on Print Spool Directory to Minimum Required Access",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description = "Restricting file system permissions on the print spool directory ensures that only the Print Spooler service account and local administrators have access to spool files preventing unauthorized reading or modification of print job data. Default Windows configurations allow the Network Service account and some user accounts to read from the spool directory which is broader access than required for normal printing operations. Tightening spool directory ACLs to SYSTEM and Print Spooler service only requires careful testing to ensure that the print spooler functionality is not broken and that legitimate access patterns are maintained. The Windows default spool directory path is %SYSTEMROOT%\\System32\\spool\\PRINTERS which should have restrictive ACLs preventing standard user access. Spool directory permission changes should be performed with care and tested thoroughly before production deployment as misconfigured permissions can prevent printing from functioning. Periodic review of spool directory permissions should verify that ACLs have not been relaxed by software installation or administrative changes.",
                Tags = ["print-spooler", "directory-permissions", "acl", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSpoolDirectoryPermissions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSpoolDirectoryPermissions")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSpoolDirectoryPermissions", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-block-spool-file-access-by-network",
                Label = "Block Remote Network Access to Print Spooler Spool File Directory",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description = "Blocking remote network access to the print spool directory ensures that network shares and remote file access protocols cannot be used to read or enumerate print spool contents from remote systems without the authorization required for spooler management operations. The PrintNightmare vulnerability family demonstrated that access to the spool directory from remote network connections can be exploited for privilege escalation and remote code execution. Blocking network access to the spool directory at the file system level provides defense in depth complementing the print spooler service access controls. Network firewall rules should also block remote access to the print spooler service on port 445 from systems that are not authorized print clients or print administrators. The printer driver path within the spool directory is particularly sensitive as it can be used to load arbitrary DLLs if network access is permitted. Vulnerability assessments should specifically test for network access to the spool directory as part of print infrastructure security evaluations.",
                Tags = ["print-spooler", "network-access", "printnightmare", "remote-access", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNetworkSpoolFileAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNetworkSpoolFileAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNetworkSpoolFileAccess", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-enable-spool-service-hardening",
                Label = "Enable Additional Security Hardening for Print Spooler Service Operation",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
                Description = "Print spooler service hardening applies additional security restrictions to the spooler process including restricting which DLLs can be loaded controlling network communication capabilities and applying attack surface reduction rules specifically targeting the print spooler attack surface. The print spooler service has historically been a common target for privilege escalation exploit chains and running the spooler with hardened configuration significantly reduces the effectiveness of known exploit techniques. Spooler hardening includes disabling the ability for the Print Spooler to accept remote connections when the system is not intended to serve as a print server which eliminates the network attack surface. Applications on workstations that do not require serving print jobs to other computers should run the print spooler in local-only mode to prevent remote exploitation. Print server configurations that require the remote print spooler functionality should apply spooler hardening in ways that are compatible with the remote printing use case. Microsoft security updates for the print spooler should be applied promptly due to the elevated risk associated with known spooler vulnerabilities.",
                Tags = ["print-spooler", "service-hardening", "attack-surface", "exploit-mitigation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSpoolServiceHardening", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolServiceHardening")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSpoolServiceHardening", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-configure-spool-file-encryption",
                Label = "Configure Encryption for Print Spool Files on Disk at Rest",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
                Description = "Configuring encryption for print spool files on disk ensures that document content written to the spool directory during print operations is protected against unauthorized access by processes that can access the file system but are not authorized to access print data. Spool file encryption can be implemented through EFS Encrypting File System applied to the spool directory or through volume-level BitLocker encryption that covers the system drive where the spool directory resides. EFS applied specifically to the spool directory provides per-file encryption with the Print Spooler service as the authorized accessor while BitLocker provides volume-level protection relevant to physical media attacks. Organizations processing highly sensitive documents should evaluate spool file encryption as a control that complements access control restrictions on the spool directory. Encryption key management for spool file encryption should integrate with organizational key management practices to ensure keys are recoverable in the event of system failure. Performance testing should validate that spool file encryption does not introduce unacceptable latency in the print workflow for high-volume print environments.",
                Tags = ["print-spooler", "spool-encryption", "data-at-rest", "efs", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSpoolFileEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolFileEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSpoolFileEncryption", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-disable-persistently-cached-print-jobs",
                Label = "Disable Persistent Caching of Print Jobs in Print Spool for Offline Recovery",
                Category = "Printing",
                NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
                Description = "Disabling persistent caching of print jobs prevents the print spooler from retaining print job data across system restarts for the purpose of re-submitting jobs that were queued when a printer was offline. Persistent print job caching means that document content can remain in the spool for extended periods including across security-relevant system events such as user logoff or system hibernation. Users who submit print jobs intending them to be printed will have a poor experience if persistent caching is disabled when the target printer is unavailable but the security benefit justifies the workflow impact in high-security environments. Organizations with strict data handling requirements for sensitive document categories should disable persistent print job caching to ensure document data does not accumulate in the spool across operational sessions. Alternative print management approaches including print management software that provides controlled job resubmission with appropriate authentication can address legitimate offline printing requirements. User communication about the impact of disabling persistent print caching should be provided before the policy is deployed.",
                Tags = ["print-spooler", "persistent-cache", "data-minimization", "offline-printing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePersistentlyCachedPrintJobs", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePersistentlyCachedPrintJobs")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePersistentlyCachedPrintJobs", 1)],
            },
        ];

    }

    // ── PrintTicketPolicy ──
    private static class _PrintTicketPolicy
    {
        private const string TktKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PrintTicket";

        private const string PrtKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prttkt-enable-print-ticket-validation",
                    Label = "Print Ticket: Enable Print Ticket Schema Validation",
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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
                    Category = "Printing",
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

    // ── ProtectedPrintModePolicy ──
    private static class _ProtectedPrintModePolicy
    {
        private const string WppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ProtectedPrint";
        private const string WppDriverKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ProtectedPrint\DriverPolicy";
        private const string PrintSpoolerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\WPP";

        public static IReadOnlyList<TweakDef> Data =>
        [
            new TweakDef
            {
                Id = "wpp-enable-protected-print-mode",
                Label = "Enable Windows Protected Print Mode",
                Category = "Printing",
                Description = "Enables Windows Protected Print (WPP) mode, which restricts printing to only Windows-protected printer drivers that are signed and certified by Microsoft. Prevents malicious print drivers.",
                Tags = ["wpp", "printing", "protected-print", "driver-security", "windows-11"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Eliminates third-party unsigned print driver attack vectors; only Microsoft-supplied IPP-class drivers are permitted.",
                RegistryKeys = [WppKey],
                ApplyOps  = [RegOp.SetDword(WppKey, "EnableProtectedPrintMode", 1)],
                RemoveOps = [RegOp.DeleteValue(WppKey, "EnableProtectedPrintMode")],
                DetectOps = [RegOp.CheckDword(WppKey, "EnableProtectedPrintMode", 1)],
            },
            new TweakDef
            {
                Id = "wpp-block-legacy-print-drivers",
                Label = "Block Legacy (Non-WPP) Print Drivers",
                Category = "Printing",
                Description = "Prevents Windows from loading or using non-WPP print drivers. Only drivers explicitly certified under the Windows Protected Print certification program are permitted to run.",
                Tags = ["wpp", "printing", "driver-block", "legacy-driver", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "May prevent older printers without WPP-certified drivers from functioning. Verify printer compatibility before enabling in production.",
                RegistryKeys = [WppKey],
                ApplyOps  = [RegOp.SetDword(WppKey, "BlockLegacyPrintDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(WppKey, "BlockLegacyPrintDrivers")],
                DetectOps = [RegOp.CheckDword(WppKey, "BlockLegacyPrintDrivers", 1)],
            },
            new TweakDef
            {
                Id = "wpp-require-driver-signature",
                Label = "Require Driver Signature Verification for Print Drivers",
                Category = "Printing",
                Description = "Enforces cryptographic signature verification for all print drivers prior to loading. Drivers without a valid Microsoft-issued signature are rejected, even in a non-WPP environment.",
                Tags = ["wpp", "printing", "driver-signing", "code-integrity", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Prevents unsigned or self-signed malicious drivers from being loaded by the print spooler service.",
                RegistryKeys = [WppDriverKey],
                ApplyOps  = [RegOp.SetDword(WppDriverKey, "RequireSignedDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(WppDriverKey, "RequireSignedDrivers")],
                DetectOps = [RegOp.CheckDword(WppDriverKey, "RequireSignedDrivers", 1)],
            },
            new TweakDef
            {
                Id = "wpp-disable-driver-installation-from-user",
                Label = "Prevent Users from Installing Print Drivers",
                Category = "Printing",
                Description = "Restricts print driver installation to administrators only. Standard users cannot add printers with non-WPP drivers via the Windows print management UI or mapped drives.",
                Tags = ["wpp", "printing", "driver-install", "user-restriction", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "A common attack vector involves tricking users into connecting to rogue printers that install malicious drivers; this policy blocks that path.",
                RegistryKeys = [WppDriverKey],
                ApplyOps  = [RegOp.SetDword(WppDriverKey, "PreventUserDriverInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(WppDriverKey, "PreventUserDriverInstall")],
                DetectOps = [RegOp.CheckDword(WppDriverKey, "PreventUserDriverInstall", 1)],
            },
            new TweakDef
            {
                Id = "wpp-audit-driver-load-events",
                Label = "Audit Print Driver Load Events",
                Category = "Printing",
                Description = "Enables audit logging for all print driver load operations. Events include driver name, installer identity, and whether the load was permitted or denied by WPP policy.",
                Tags = ["wpp", "printing", "driver-audit", "event-log", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Creates a forensic trail of print driver activity, enabling detection of unexpected driver installations.",
                RegistryKeys = [WppKey],
                ApplyOps  = [RegOp.SetDword(WppKey, "AuditDriverLoadEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(WppKey, "AuditDriverLoadEvents")],
                DetectOps = [RegOp.CheckDword(WppKey, "AuditDriverLoadEvents", 1)],
            },
            new TweakDef
            {
                Id = "wpp-block-raw-printing",
                Label = "Block RAW Format Print Job Submission",
                Category = "Printing",
                Description = "Prevents applications from submitting RAW-format print jobs, which bypass the Windows print rendering pipeline and can embed arbitrary data. WPP requires rendering through the IPP stack.",
                Tags = ["wpp", "printing", "raw-print", "ipp", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "RAW print jobs can exfiltrate data to printers; IPP-rendered jobs pass through the OS pipeline which can be inspected by DLP tools.",
                RegistryKeys = [PrintSpoolerKey],
                ApplyOps  = [RegOp.SetDword(PrintSpoolerKey, "BlockRawPrintJobs", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "BlockRawPrintJobs")],
                DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "BlockRawPrintJobs", 1)],
            },
            new TweakDef
            {
                Id = "wpp-restrict-remote-print-driver-install",
                Label = "Block Remote Print Driver Installation via RPC",
                Category = "Printing",
                Description = "Prevents print drivers from being remotely installed via the Print Spooler RPC interface. Remote driver installation was exploited by PrintNightmare (CVE-2021-1675); WPP mode disables this endpoint.",
                Tags = ["wpp", "printing", "rpc", "print-spooler", "printnightmare", "cve"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Directly mitigates PrintNightmare-class RPC exploitation. Eliminates remote driver install surface from the print spooler.",
                RegistryKeys = [PrintSpoolerKey],
                ApplyOps  = [RegOp.SetDword(PrintSpoolerKey, "BlockRemoteDriverInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "BlockRemoteDriverInstall")],
                DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "BlockRemoteDriverInstall", 1)],
            },
            new TweakDef
            {
                Id = "wpp-require-ipp-protocol-only",
                Label = "Restrict Print Communication to IPP Protocol Only",
                Category = "Printing",
                Description = "Configures the Windows print stack to communicate with printers using Internet Printing Protocol (IPP) only, blocking legacy LPR and SMB-based print protocols that WPP does not support.",
                Tags = ["wpp", "printing", "ipp", "protocol", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Requires printers to support IPP; legacy network printers using LPR or SMB printing will not work. Test compatibility in a pilot group first.",
                RegistryKeys = [PrintSpoolerKey],
                ApplyOps  = [RegOp.SetDword(PrintSpoolerKey, "RestrictToIPPOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "RestrictToIPPOnly")],
                DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "RestrictToIPPOnly", 1)],
            },
            new TweakDef
            {
                Id = "wpp-disable-printer-redirection-rdp",
                Label = "Disable Client-Side Print Redirection in Remote Desktop",
                Category = "Printing",
                Description = "Prevents local printers from being redirected and made available in Remote Desktop sessions. Eliminates the risk of untrusted WPP-non-compliant client drivers being exposed to an RDS server.",
                Tags = ["wpp", "printing", "rdp", "print-redirection", "remote-desktop"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Users in RDP sessions cannot print to their local printers; they must use printers accessible from the server side.",
                RegistryKeys = [PrintSpoolerKey],
                ApplyOps  = [RegOp.SetDword(PrintSpoolerKey, "DisableRdpPrinterRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "DisableRdpPrinterRedirection")],
                DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "DisableRdpPrinterRedirection", 1)],
            },
            new TweakDef
            {
                Id = "wpp-enable-spooler-process-isolation",
                Label = "Enable Print Spooler Process Isolation",
                Category = "Printing",
                Description = "Configures the Windows Print Spooler to run third-party print processors and drivers in isolated job-scoped processes rather than within the main spooler process. Limits the blast radius of a compromised driver.",
                Tags = ["wpp", "printing", "process-isolation", "spooler", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 26100,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "A malicious or buggy print driver only affects its isolated process rather than the entire spooler, reducing privilege escalation risk.",
                RegistryKeys = [PrintSpoolerKey],
                ApplyOps  = [RegOp.SetDword(PrintSpoolerKey, "EnableSpoolerProcessIsolation", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "EnableSpoolerProcessIsolation")],
                DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "EnableSpoolerProcessIsolation", 1)],
            },
        ];

    }

}
