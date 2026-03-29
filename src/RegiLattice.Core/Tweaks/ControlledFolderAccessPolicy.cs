// RegiLattice.Core — Tweaks/ControlledFolderAccessPolicy.cs
// Controlled Folder Access (CFA) Policy — Sprint 533.
// Configures Windows Defender Exploit Guard Controlled Folder Access to protect
// designated folders from unauthorized modification by ransomware and other threats.
// Category: "Controlled Folder Access Policy" | Slug: cfa
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ControlledFolderAccessPolicy
{
    private const string CfaKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Controlled Folder Access";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cfa-enable-block-mode",
                Label = "Controlled Folder Access: Enable Block Mode",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets EnableControlledFolderAccess=1 (enabled, block mode). Activates Controlled Folder Access, which prevents untrusted applications from modifying files in protected folders such as Documents, Pictures, and Desktop. This is the most effective built-in ransomware protection available in Windows. Any untrusted process attempting to write to protected folders is blocked and an event is logged to the Security event channel.",
                Tags = ["cfa", "ransomware", "defender", "folder-protection", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "Strong ransomware protection. Common false positives: backup tools, photo editors, custom apps writing to Documents. Build an allow-list before deploying.",
                ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 1)],
            },
            new TweakDef
            {
                Id = "cfa-enable-audit-mode",
                Label = "Controlled Folder Access: Enable Audit Mode",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets EnableControlledFolderAccess=2 (audit mode). Logs all write attempts to protected folders without blocking them. Use audit mode to identify which applications need to be added to the CFA allow-list before enabling block mode. Events appear in Event Viewer under Microsoft-Windows-Windows Defender/Operational with Event ID 1124. Microsoft recommends a 2–4 week audit period before switching to block mode.",
                Tags = ["cfa", "audit", "defender", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Audit-only; use before enabling block mode to prevent application breakage.",
                ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 2)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 2)],
            },
            new TweakDef
            {
                Id = "cfa-protect-network-drives",
                Label = "Controlled Folder Access: Extend Protection to Network Shares",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets EnableNetworkProtection=1. Extends Controlled Folder Access to network drives and UNC paths mapped to the machine. Ransomware typically moves laterally to file servers shortly after executing on a workstation; protecting network-mapped drives prevents encrypted-file propagation to shared storage. Requires CFA to be in block or audit mode (EnableControlledFolderAccess ≠ 0) to take effect.",
                Tags = ["cfa", "network", "ransomware", "network-drive", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Network drive protection may cause latency for legitimate writes. Test with mapped backup and file server shares.",
                ApplyOps = [RegOp.SetDword(CfaKey, "EnableNetworkProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableNetworkProtection")],
                DetectOps = [RegOp.CheckDword(CfaKey, "EnableNetworkProtection", 1)],
            },
            new TweakDef
            {
                Id = "cfa-block-disk-modification",
                Label = "Controlled Folder Access: Block Unauthorized Disk Sector Modifications",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets EnableControlledFolderAccessForRawAccess=1. Prevents unauthorized applications from issuing raw disk write operations that bypass the filesystem layer. Some ransomware variants use direct disk sector writes (via CreateFile with physical drive paths) to overwrite the MBR or encrypt entire partition sectors without touching individual files. This policy blocks raw disk access from untrusted processes.",
                Tags = ["cfa", "mbr", "raw-disk", "ransomware", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks raw disk writes from untrusted processes. Disk imaging and partition tools must be allow-listed.",
                ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccessForRawAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccessForRawAccess")],
                DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccessForRawAccess", 1)],
            },
            new TweakDef
            {
                Id = "cfa-disable-notification",
                Label = "Controlled Folder Access: Suppress Block Notifications to Users",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets DisableNotifications=1 under CFA. Suppresses the toast notification that appears when CFA blocks an application. In corporate environments, end-user CFA block notifications can be confusing and generate spurious helpdesk tickets. Security events are always logged regardless of this setting. Suitable for managed environments where the SOC monitors event logs rather than relying on user reports.",
                Tags = ["cfa", "notification", "defender", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Silent blocks. Ensure event log monitoring is in place so blocked events are not missed.",
                ApplyOps = [RegOp.SetDword(CfaKey, "DisableNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "DisableNotifications")],
                DetectOps = [RegOp.CheckDword(CfaKey, "DisableNotifications", 1)],
            },
            new TweakDef
            {
                Id = "cfa-protect-temp-folder",
                Label = "Controlled Folder Access: Protect %TEMP% Folder",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets ProtectedFoldersTempDir=1. Adds the user-specific %TEMP% directory to the CFA-protected folder list. Dropper malware commonly writes stage-2 payloads to %TEMP% before executing them. Protecting %TEMP% prevents untrusted processes from writing new executable content to this frequently-targeted location. Legitimate installers that extract to %TEMP% must be allow-listed before enabling this.",
                Tags = ["cfa", "temp", "dropper", "defender", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 2,
                ImpactNote = "High false positives: many installers write and execute from %TEMP%. Allow-list all installers before deploying.",
                ApplyOps = [RegOp.SetDword(CfaKey, "ProtectedFoldersTempDir", 1)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "ProtectedFoldersTempDir")],
                DetectOps = [RegOp.CheckDword(CfaKey, "ProtectedFoldersTempDir", 1)],
            },
            new TweakDef
            {
                Id = "cfa-protect-browser-data",
                Label = "Controlled Folder Access: Protect Browser Profile Folders",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets ProtectBrowserFolders=1. Adds browser profile directories (Chrome/Edge/Firefox user data) to the CFA protected folder list. Browser profile data contains saved passwords, cookies, and session tokens — high-value targets for infostealer malware. Preventing unauthorized writes to browser profile folders blocks infostealers from exfiltrating credential data without affecting normal browser operation.",
                Tags = ["cfa", "browser", "infostealer", "cookies", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Protects browser credential stores. Backup tools that copy browser profiles may need allow-listing.",
                ApplyOps = [RegOp.SetDword(CfaKey, "ProtectBrowserFolders", 1)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "ProtectBrowserFolders")],
                DetectOps = [RegOp.CheckDword(CfaKey, "ProtectBrowserFolders", 1)],
            },
            new TweakDef
            {
                Id = "cfa-enable-block-mode-disk",
                Label = "Controlled Folder Access: Enable Block Mode Including Disk Sectors",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets EnableControlledFolderAccess=3 (block mode with disk sector protection). Combines standard CFA file-level protection with block mode for raw disk writes in a single policy value. Using value 3 is the strongest CFA configuration, protecting against both file-encrypting ransomware and MBR/boot-sector wipers in one policy. Equivalent to enabling EnableControlledFolderAccess=1 AND EnableControlledFolderAccessForRawAccess=1 separately.",
                Tags = ["cfa", "ransomware", "mbr", "wiper", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 2,
                ImpactNote = "Maximum protection but highest false positive risk. Only use after completing an audit period with value 2.",
                ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 3)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 3)],
            },
            new TweakDef
            {
                Id = "cfa-audit-mode-disk",
                Label = "Controlled Folder Access: Audit Mode Including Disk Sector Checks",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets EnableControlledFolderAccess=4 (audit mode with disk sector monitoring). Logs both file-level and raw disk sector write attempts to protected locations without blocking them. Use value 4 when planning to deploy value 3 (block + disk) to pre-identify which applications perform raw disk writes. Events are logged to the Windows Defender/Operational channel with Event IDs 1124 (allowed) and 1125 (would-be blocked).",
                Tags = ["cfa", "audit", "disk", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Audits disk sector writes in addition to file writes. No blocking; safe for production.",
                ApplyOps = [RegOp.SetDword(CfaKey, "EnableControlledFolderAccess", 4)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "EnableControlledFolderAccess")],
                DetectOps = [RegOp.CheckDword(CfaKey, "EnableControlledFolderAccess", 4)],
            },
            new TweakDef
            {
                Id = "cfa-enforce-allow-list-only",
                Label = "Controlled Folder Access: Enforce Allow-List Only Mode",
                Category = "Controlled Folder Access Policy",
                Description =
                    "Sets OnlyEnforceAllowedApplicationsList=1. When set, only explicitly allow-listed applications (configured via separate CFA Allowed Applications policy) may write to protected folders. Without this flag, CFA maintains an internal safe-apps list based on signing and reputation; with it, only the IT-defined allow-list is trusted. Provides maximum enterprise control at the cost of requiring a maintained allow-list.",
                Tags = ["cfa", "allow-list", "enterprise", "defender", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Requires a maintained application allow-list. All apps not on the list will be blocked, including signed apps.",
                ApplyOps = [RegOp.SetDword(CfaKey, "OnlyEnforceAllowedApplicationsList", 1)],
                RemoveOps = [RegOp.DeleteValue(CfaKey, "OnlyEnforceAllowedApplicationsList")],
                DetectOps = [RegOp.CheckDword(CfaKey, "OnlyEnforceAllowedApplicationsList", 1)],
            },
        ];
}
