#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class PrintSpoolerPolicy
{
    private const string SpoolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string PnPKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "prtspool-block-km-printer-drivers",
            Label = "Block Kernel-Mode Printer Drivers",
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
            Category = "Print Spooler Policy",
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
