namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class SecurityPrinterHardening
{
    private const string PnpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

    private const string PrintersKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

    private const string SpoolerKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "prtharden-block-pointandprint-unrestricted",
                Label = "Block Unrestricted Point-and-Print Driver Install",
                Category = "Security — Printer Hardening",
                Description =
                    "Restricts which print servers can install printer drivers without administrator approval. "
                    + "Mitigates PrintNightmare (CVE-2021-34527) by preventing arbitrary driver installation from untrusted servers. "
                    + "Default: unrestricted. Recommended: restricted.",
                Tags = ["printer", "printnightmare", "cve-2021-34527", "drivers", "spooler", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(PnpKey, "Restricted", 1)],
                RemoveOps = [RegOp.DeleteValue(PnpKey, "Restricted")],
                DetectOps = [RegOp.CheckDword(PnpKey, "Restricted", 1)],
            },
            new TweakDef
            {
                Id = "prtharden-block-pointandprint-warn",
                Label = "Require Warning When Installing Printer Drivers",
                Category = "Security — Printer Hardening",
                Description =
                    "Requires a security warning to be shown when Point-and-Print installs a new driver. "
                    + "Allows users to detect and reject unexpected printer driver installations. "
                    + "Default: no warning. Recommended: warn.",
                Tags = ["printer", "driver-install", "warning", "point-and-print", "uac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(PnpKey, "NoWarningNoElevationOnInstall", 0)],
                RemoveOps = [RegOp.DeleteValue(PnpKey, "NoWarningNoElevationOnInstall")],
                DetectOps = [RegOp.CheckDword(PnpKey, "NoWarningNoElevationOnInstall", 0)],
            },
            new TweakDef
            {
                Id = "prtharden-block-pointandprint-update-warn",
                Label = "Require Warning When Updating Printer Drivers",
                Category = "Security — Printer Hardening",
                Description =
                    "Requires a security warning when an existing Point-and-Print driver is updated. "
                    + "Prevents silent driver updates as part of PrintNightmare mitigations. "
                    + "Default: silent update. Recommended: warn.",
                Tags = ["printer", "driver-update", "warning", "point-and-print", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(PnpKey, "UpdatePromptSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(PnpKey, "UpdatePromptSettings")],
                DetectOps = [RegOp.CheckDword(PnpKey, "UpdatePromptSettings", 1)],
            },
            new TweakDef
            {
                Id = "prtharden-enable-rpc-privacy",
                Label = "Enable Print Spooler RPC Authentication Privacy",
                Category = "Security — Printer Hardening",
                Description =
                    "Requires packet-level privacy (encryption) on all RPC connections to the print spooler. "
                    + "Prevents interception of spooler communications and protects against MiTM on spooler RPC. "
                    + "Default: no encryption. Recommended: enabled.",
                Tags = ["printer", "rpc", "spooler", "encryption", "privacy", "mitm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(SpoolerKey, "RpcAuthnLevelPrivacyEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SpoolerKey, "RpcAuthnLevelPrivacyEnabled")],
                DetectOps = [RegOp.CheckDword(SpoolerKey, "RpcAuthnLevelPrivacyEnabled", 1)],
            },
            new TweakDef
            {
                Id = "prtharden-disable-remote-spooler",
                Label = "Disallow Remote Connections to Print Spooler",
                Category = "Security — Printer Hardening",
                Description =
                    "Disables network remote access to the Windows print spooler service. "
                    + "Key mitigation for PrintNightmare (CVE-2021-34527) on servers that must keep the spooler running locally. "
                    + "Default: remote connections allowed. Recommended: disabled.",
                Tags = ["printer", "spooler", "remote-access", "printnightmare", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(PrintersKey, "RegisterSpoolerRemoteRpcEndPoint", 2)],
                RemoveOps = [RegOp.DeleteValue(PrintersKey, "RegisterSpoolerRemoteRpcEndPoint")],
                DetectOps = [RegOp.CheckDword(PrintersKey, "RegisterSpoolerRemoteRpcEndPoint", 2)],
            },
            new TweakDef
            {
                Id = "prtharden-disable-web-printers",
                Label = "Disable Internet Printing via HTTP",
                Category = "Security — Printer Hardening",
                Description =
                    "Prevents the print spooler from accepting print jobs and managing printers over HTTP (Internet Printing Protocol). "
                    + "Reduces the HTTP-based attack surface of the spooler service. "
                    + "Default: HTTP printing enabled. Recommended: disabled.",
                Tags = ["printer", "http", "internet-printing", "spooler", "attack-surface"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(PrintersKey, "DisableHTTPPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintersKey, "DisableHTTPPrinting")],
                DetectOps = [RegOp.CheckDword(PrintersKey, "DisableHTTPPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtharden-disable-web-printer-download",
                Label = "Disable Printer Driver Download from Windows Update",
                Category = "Security — Printer Hardening",
                Description =
                    "Prevents Windows from automatically downloading printer drivers from Windows Update. "
                    + "Forces administrators to manually approve and deploy drivers, preventing potential supply-chain driver attacks. "
                    + "Default: auto-download enabled. Recommended: disabled.",
                Tags = ["printer", "driver-download", "windows-update", "supply-chain", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(PrintersKey, "DisableWebPnPDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintersKey, "DisableWebPnPDownload")],
                DetectOps = [RegOp.CheckDword(PrintersKey, "DisableWebPnPDownload", 1)],
            },
            new TweakDef
            {
                Id = "prtharden-block-v3-drivers",
                Label = "Restrict Type-3 Printer Drivers to System Accounts Only",
                Category = "Security — Printer Hardening",
                Description =
                    "Limits Type-3 (kernel-mode) printer driver installation and loading to SYSTEM and built-in accounts only. "
                    + "Prevents standard users and restricted admins from loading potentially vulnerable kernel drivers. "
                    + "Default: no restriction. Recommended: restricted.",
                Tags = ["printer", "kernel-driver", "v3-driver", "type3", "privilege-escalation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(PrintersKey, "TypeGPODriverInstallationPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(PrintersKey, "TypeGPODriverInstallationPolicy")],
                DetectOps = [RegOp.CheckDword(PrintersKey, "TypeGPODriverInstallationPolicy", 1)],
            },
            new TweakDef
            {
                Id = "prtharden-disable-printer-location-tracking",
                Label = "Disable Automatic Printer Location Tracking",
                Category = "Security — Printer Hardening",
                Description =
                    "Prevents Windows from tracking and publishing printer physical locations to Active Directory. "
                    + "Limits information disclosure about organisational network topology. "
                    + "Default: location tracking enabled when printer location data available. Recommended: disabled.",
                Tags = ["printer", "location", "active-directory", "information-disclosure", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(PrintersKey, "PhysicalLocation", 0)],
                RemoveOps = [RegOp.DeleteValue(PrintersKey, "PhysicalLocation")],
                DetectOps = [RegOp.CheckDword(PrintersKey, "PhysicalLocation", 0)],
            },
            new TweakDef
            {
                Id = "prtharden-disable-redirection-in-session",
                Label = "Block Client Printer Redirection in Remote Sessions",
                Category = "Security — Printer Hardening",
                Description =
                    "Prevents client printers from being redirected into Remote Desktop or RemoteApp sessions. "
                    + "Eliminates the printer-redirection vector used in some privilege escalation attacks. "
                    + "Default: redirection enabled. Recommended: disabled on servers.",
                Tags = ["printer", "rdp", "redirect", "remote-session", "privilege-escalation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
            },
        ];
}
