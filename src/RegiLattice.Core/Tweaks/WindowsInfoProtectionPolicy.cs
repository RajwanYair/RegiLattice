namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsInfoProtectionPolicy
{
    private const string WipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataProtection";
    private const string EdpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseDataProtection";
    private const string NetIsoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkIsolation";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wippol-allow-user-decrypt",
            Label = "WIP: Allow User to Decrypt Protected Files",
            Category = "Windows Information Protection Policy",
            Description =
                "Allows the owner of a WIP-protected file to decrypt it. When disabled, only IT admins can remove protection. Enabled by default; disable for stricter DLP control.",
            Tags = ["wip", "edp", "dlp", "data-protection", "encryption", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Prevents users from self-decrypting WIP-protected files; admin-only removal.",
            RegistryKeys = [WipKey],
            ApplyOps = [RegOp.SetDword(WipKey, "AllowUserDecryption", 0)],
            RemoveOps = [RegOp.DeleteValue(WipKey, "AllowUserDecryption")],
            DetectOps = [RegOp.CheckDword(WipKey, "AllowUserDecryption", 0)],
        },
        new TweakDef
        {
            Id = "wippol-require-protection-under-lock",
            Label = "WIP: Require Protection While Under Lock",
            Category = "Windows Information Protection Policy",
            Description =
                "Requires that WIP-protected data remain encrypted even when the device is locked. Prevents data access from unauthorized physical access when the screen is locked.",
            Tags = ["wip", "edp", "lock-screen", "data-protection", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Keeps WIP data encrypted when device is locked.",
            RegistryKeys = [WipKey],
            ApplyOps = [RegOp.SetDword(WipKey, "RequireProtectionUnderLockConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(WipKey, "RequireProtectionUnderLockConfig")],
            DetectOps = [RegOp.CheckDword(WipKey, "RequireProtectionUnderLockConfig", 1)],
        },
        new TweakDef
        {
            Id = "wippol-enable-edp",
            Label = "WIP: Enable Enterprise Data Protection",
            Category = "Windows Information Protection Policy",
            Description =
                "Enables Windows Information Protection (formerly Enterprise Data Protection/EDP) on the device. Mode 3 = Block — prevents users from copying work data to personal apps.",
            Tags = ["wip", "edp", "dlp", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Enables WIP/EDP on the device — prerequisite for all WIP policy tweaks.",
            RegistryKeys = [EdpKey],
            ApplyOps = [RegOp.SetDword(EdpKey, "WIPEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdpKey, "WIPEnabled")],
            DetectOps = [RegOp.CheckDword(EdpKey, "WIPEnabled", 1)],
        },
        new TweakDef
        {
            Id = "wippol-silent-mode",
            Label = "WIP: Set Silent Enforcement (Audit Without Block)",
            Category = "Windows Information Protection Policy",
            Description =
                "Sets WIP/EDP to silent enforcement mode. Personal data leakage is logged to the event log but not blocked. Useful for piloting WIP before enforcing restrictions.",
            Tags = ["wip", "edp", "dlp", "audit", "silent", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Sets WIP to audit-only mode for piloting before full enforcement.",
            RegistryKeys = [EdpKey],
            ApplyOps = [RegOp.SetDword(EdpKey, "EnforcementMode", 1)],
            RemoveOps = [RegOp.DeleteValue(EdpKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(EdpKey, "EnforcementMode", 1)],
        },
        new TweakDef
        {
            Id = "wippol-block-copy-to-personal",
            Label = "WIP: Block Copying Work Data to Personal Apps",
            Category = "Windows Information Protection Policy",
            Description =
                "Enforces WIP to Block mode — users cannot copy, paste, or share protected work data to personal or unmanaged applications. The strictly enforced DLP level.",
            Tags = ["wip", "edp", "dlp", "block", "clipboard", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Block mode — prevents copying work data to personal apps.",
            RegistryKeys = [EdpKey],
            ApplyOps = [RegOp.SetDword(EdpKey, "EnforcementMode", 3)],
            RemoveOps = [RegOp.DeleteValue(EdpKey, "EnforcementMode")],
            DetectOps = [RegOp.CheckDword(EdpKey, "EnforcementMode", 3)],
        },
        new TweakDef
        {
            Id = "wippol-disable-bing-results-wip",
            Label = "WIP: Disable Bing Integration for Work Searches",
            Category = "Windows Information Protection Policy",
            Description =
                "Prevents Windows Search from sending work-context search queries to Bing when WIP is enabled. Keeps enterprise search results isolated from the internet.",
            Tags = ["wip", "edp", "bing", "search", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents work-context searches from going to Bing when WIP is active.",
            RegistryKeys = [EdpKey],
            ApplyOps = [RegOp.SetDword(EdpKey, "DisableWindowsBingSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(EdpKey, "DisableWindowsBingSearch")],
            DetectOps = [RegOp.CheckDword(EdpKey, "DisableWindowsBingSearch", 1)],
        },
        new TweakDef
        {
            Id = "wippol-revoke-on-unenroll",
            Label = "WIP: Revoke Access Keys on MDM Unenrollment",
            Category = "Windows Information Protection Policy",
            Description =
                "Automatically revokes WIP encryption keys when the device is unenrolled from MDM. Prevents access to protected work data after device management is removed.",
            Tags = ["wip", "edp", "mdm", "revoke", "unenroll", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Revokes WIP keys when device leaves MDM management.",
            RegistryKeys = [EdpKey],
            ApplyOps = [RegOp.SetDword(EdpKey, "RevokeOnMDMHandoff", 1)],
            RemoveOps = [RegOp.DeleteValue(EdpKey, "RevokeOnMDMHandoff")],
            DetectOps = [RegOp.CheckDword(EdpKey, "RevokeOnMDMHandoff", 1)],
        },
        new TweakDef
        {
            Id = "wippol-show-ede-icons",
            Label = "WIP: Show Enterprise Data Protection Icons on Protected Files",
            Category = "Windows Information Protection Policy",
            Description =
                "Displays a work briefcase icon overlay on WIP-protected files in Explorer and on the Start menu to visually distinguish protected corporate data from personal files.",
            Tags = ["wip", "edp", "icons", "explorer", "visibility", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Shows work briefcase icon overlay on WIP-protected files.",
            RegistryKeys = [EdpKey],
            ApplyOps = [RegOp.SetDword(EdpKey, "ShowIcons", 1)],
            RemoveOps = [RegOp.DeleteValue(EdpKey, "ShowIcons")],
            DetectOps = [RegOp.CheckDword(EdpKey, "ShowIcons", 1)],
        },
        new TweakDef
        {
            Id = "wippol-restrict-clipboard",
            Label = "WIP: Restrict Clipboard Sharing Between Work and Personal",
            Category = "Windows Information Protection Policy",
            Description =
                "Restricts clipboard operations to prevent copying WIP-protected (work) content into unmanaged (personal) applications. Prevents clipboard-based data exfiltration.",
            Tags = ["wip", "edp", "clipboard", "dlp", "restriction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Restricts clipboard to prevent work-to-personal data leakage.",
            RegistryKeys = [EdpKey],
            ApplyOps = [RegOp.SetDword(EdpKey, "ClipboardProtectionLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(EdpKey, "ClipboardProtectionLevel")],
            DetectOps = [RegOp.CheckDword(EdpKey, "ClipboardProtectionLevel", 2)],
        },
        new TweakDef
        {
            Id = "wippol-enterprise-ip-isolation",
            Label = "WIP: Enable Enterprise Network Isolation",
            Category = "Windows Information Protection Policy",
            Description =
                "Activates WIP network isolation policy — only IP ranges and domains defined in the enterprise network boundary list are treated as 'work' destinations.",
            Tags = ["wip", "edp", "network-isolation", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Activates WIP network isolation policy for enterprise boundaries.",
            RegistryKeys = [NetIsoKey],
            ApplyOps = [RegOp.SetDword(NetIsoKey, "EnterpriseCloudResources", 1)],
            RemoveOps = [RegOp.DeleteValue(NetIsoKey, "EnterpriseCloudResources")],
            DetectOps = [RegOp.CheckDword(NetIsoKey, "EnterpriseCloudResources", 1)],
        },
    ];
}
