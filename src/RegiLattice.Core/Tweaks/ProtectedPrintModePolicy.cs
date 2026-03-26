// RegiLattice.Core — Tweaks/ProtectedPrintModePolicy.cs
// Windows Protected Print Mode (WPP) Group Policy controls — Sprint 369.
// Category: "Windows Protected Print Policy" | Slug: wpp
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\ProtectedPrint
// MinBuild: 26100 (Windows 11 24H2+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ProtectedPrintModePolicy
{
    private const string WppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ProtectedPrint";
    private const string WppDriverKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ProtectedPrint\DriverPolicy";
    private const string PrintSpoolerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\WPP";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wpp-enable-protected-print-mode",
            Label = "Enable Windows Protected Print Mode",
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
            Category = "Windows Protected Print Policy",
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
