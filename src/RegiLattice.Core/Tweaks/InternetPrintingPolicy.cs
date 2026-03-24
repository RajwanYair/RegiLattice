#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class InternetPrintingPolicy
{
    private const string Prnt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
    private const string PnP = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "inetprt-disable-web-printing",
            Label = "Disable Web Printing",
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
            Category = "Internet Printing Policy",
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
