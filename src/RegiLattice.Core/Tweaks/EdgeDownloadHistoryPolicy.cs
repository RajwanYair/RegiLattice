namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class EdgeDownloadHistoryPolicy
{
    private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edgedl-restrict-dangerous-downloads",
            Label = "Edge Download & History Policy: Block Dangerous and Malicious Downloads",
            Category = "Edge Download & History Policy",
            Description =
                "Configures Microsoft Edge to block downloads that are flagged as dangerous or malicious by Microsoft Defender SmartScreen. Setting DownloadRestrictions to 3 instructs Edge to block all downloads that SmartScreen identifies as potentially dangerous, unrecognised, or hosting malware. This is the recommended CIS-aligned value for enterprise environments. Values: 0=no restriction, 1=block SmartScreen malware detections only, 2=block unrecognised downloads, 3=block all dangerous downloads (malware + unrecognised).",
            Tags = ["edge", "downloads", "smartscreen", "malware", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "DownloadRestrictions", 3)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "DownloadRestrictions")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "DownloadRestrictions", 3)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Dangerous and unrecognised downloads are blocked by SmartScreen; users cannot override the block in policy mode.",
        },
        new TweakDef
        {
            Id = "edgedl-prompt-download-location",
            Label = "Edge Download & History Policy: Always Prompt for Download Save Location",
            Category = "Edge Download & History Policy",
            Description =
                "Configures Microsoft Edge to always ask the user where to save a downloaded file instead of automatically placing files in the default downloads folder. Setting PromptForDownloadLocation to 1 ensures users review the destination before saving, which reduces accidental saves to unsanctioned locations (e.g., cloud-synced folders, shared drives, or removable media). In data-loss-prevention scenarios, prompting for location also gives the user a moment to consider whether downloading is appropriate.",
            Tags = ["edge", "downloads", "save location", "data loss prevention", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "PromptForDownloadLocation", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "PromptForDownloadLocation")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "PromptForDownloadLocation", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Save dialog opens on every download; users must explicitly choose the destination folder each time.",
        },
        new TweakDef
        {
            Id = "edgedl-force-bing-safe-search",
            Label = "Edge Download & History Policy: Force Bing SafeSearch",
            Category = "Edge Download & History Policy",
            Description =
                "Activates Bing SafeSearch filtering for all Bing searches made in Microsoft Edge, filtering out adult-content results at the search engine level. Setting ForceBingSafeSearch to 1 enables moderate SafeSearch. Popular settings: 0=off (default), 1=moderate filtering, 2=strict filtering. In corporate, educational, and public-access environments, enabling ForceBingSafeSearch at policy level prevents users from disabling SafeSearch via account settings and ensures consistent safe-content enforcement across the user base.",
            Tags = ["edge", "bing", "safe search", "content filtering", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "ForceBingSafeSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "ForceBingSafeSearch")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "ForceBingSafeSearch", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Bing searches filtered at moderate SafeSearch level; adult content results are removed from Bing results pages.",
        },
        new TweakDef
        {
            Id = "edgedl-delete-history-on-exit",
            Label = "Edge Download & History Policy: Delete Browsing History on Browser Exit",
            Category = "Edge Download & History Policy",
            Description =
                "Configures Microsoft Edge to automatically clear browsing history (visited URLs, page titles, and cached timestamps) each time the browser closes. Setting DeleteBrowsingHistoryOnExit to 1 ensures that no browsing record persists on the local machine between sessions. This reduces the residual-data exposure on shared or public-facing machines, makes it harder for physical-access attackers to reconstruct user activity, and complies with data-minimisation requirements in privacy-sensitive deployments.",
            Tags = ["edge", "browsing history", "privacy", "data minimisation", "shared device", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "DeleteBrowsingHistoryOnExit", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "DeleteBrowsingHistoryOnExit")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "DeleteBrowsingHistoryOnExit", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "All browsing history is wiped when Edge closes; users lose history for session-restore and tab-search.",
        },
        new TweakDef
        {
            Id = "edgedl-disable-media-router",
            Label = "Edge Download & History Policy: Disable Cast (Media Router) Feature",
            Category = "Edge Download & History Policy",
            Description =
                "Disables the Cast/Media Router infrastructure in Microsoft Edge that discovers nearby Chromecast and Miracast display devices and allows browser tabs or media to be cast to them wirelessly. Setting EnableMediaRouter to 0 removes the Cast button from the Edge toolbar and prevents network scanning for cast targets. Cast device discovery sends mDNS probe packets to the local network, creating unsolicited network traffic and potentially leaking device identity information to local network listeners.",
            Tags = ["edge", "cast", "media router", "chromecast", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "EnableMediaRouter", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "EnableMediaRouter")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "EnableMediaRouter", 0)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Cast button and media router removed from Edge; tab or video casting to display devices is unavailable.",
        },
        new TweakDef
        {
            Id = "edgedl-enable-auto-update",
            Label = "Edge Download & History Policy: Ensure Microsoft Edge Automatic Updates are Enabled",
            Category = "Edge Download & History Policy",
            Description =
                "Explicitly sets Microsoft Edge to automatically apply browser updates. Setting AutoUpdateEnabled to 1 ensures Edge is not permanently frozen at a specific version by earlier misconfiguration and that security patches are applied as they are released. Enterprises that use a slower update cadence may layer version-lag policies on top, but this baseline ensures the update mechanism itself is not entirely disabled, which would leave the browser permanently unpatched.",
            Tags = ["edge", "updates", "patch management", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "AutoUpdateEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "AutoUpdateEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "AutoUpdateEnabled", 1)],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Edge automatic updates are enabled; browser receives security patches from Microsoft Update on release.",
        },
        new TweakDef
        {
            Id = "edgedl-hide-external-protocol-checkbox",
            Label = "Edge Download & History Policy: Remove 'Always Open' Checkbox from External Protocol Dialogs",
            Category = "Edge Download & History Policy",
            Description =
                "Removes the 'Always open this type of link' checkbox from the confirmation dialog that appears when Edge is about to open an external protocol handler (e.g., mailto:, ms-teams:, itms:). Setting ExternalProtocolDialogShowAlwaysOpenCheckbox to 0 means users can only approve a single launch at a time and cannot create a permanent auto-open rule for a potentially malicious custom protocol. Each subsequent click of a custom protocol link will re-show the dialog, preventing drive-by permanent associations with attacker-controlled handlers.",
            Tags = ["edge", "external protocol", "custom protocol", "dialog", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "ExternalProtocolDialogShowAlwaysOpenCheckbox", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "ExternalProtocolDialogShowAlwaysOpenCheckbox")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "ExternalProtocolDialogShowAlwaysOpenCheckbox", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "External protocol dialogs show only a one-time prompt; no persistent 'always open' rule can be created by users.",
        },
        new TweakDef
        {
            Id = "edgedl-warn-before-exit",
            Label = "Edge Download & History Policy: Warn User Before Closing Edge with Multiple Tabs",
            Category = "Edge Download & History Policy",
            Description =
                "Enables a confirmation dialog when the user attempts to close Microsoft Edge with multiple tabs or windows open. Setting WarnBeforeExitingEdge to 1 displays a 'You are about to close N tabs' prompt before the browser exits. This prevents accidental closure of browser sessions during active downloads, form filling, or multi-step web application workflows. It also deters rage-quits of the browser during intensive research sessions where re-finding pages would be time-consuming.",
            Tags = ["edge", "exit warning", "tabs", "ux", "productivity", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "WarnBeforeExitingEdge", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "WarnBeforeExitingEdge")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "WarnBeforeExitingEdge", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "A confirmation dialog appears before closing Edge when multiple tabs are open; prevents accidental session loss.",
        },
        new TweakDef
        {
            Id = "edgedl-hide-office-shortcut-in-favorites",
            Label = "Edge Download & History Policy: Remove Microsoft Office Shortcut from Favorites Bar",
            Category = "Edge Download & History Policy",
            Description =
                "Removes the Microsoft Office shortcut button that Edge adds to the Favorites bar by default, which links to the Microsoft Office web portal. Setting ShowOfficeShortcutInFavoritesBar to 0 clears this commercial shortcut from the browser chrome. In enterprise environments where the Favourites bar is managed through policy, including Office portal shortcuts that users did not add themselves creates a cluttered bar that undermines IT-configured bookmarks and may imply Microsoft has elevated access to browser configuration.",
            Tags = ["edge", "office shortcut", "favorites bar", "ux", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "ShowOfficeShortcutInFavoritesBar", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "ShowOfficeShortcutInFavoritesBar")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "ShowOfficeShortcutInFavoritesBar", 0)],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Microsoft Office shortcut removed from Favorites bar; bar shows only IT-configured or user-added bookmarks.",
        },
        new TweakDef
        {
            Id = "edgedl-suppress-unsupported-os-warning",
            Label = "Edge Download & History Policy: Suppress Unsupported Operating System Warning",
            Category = "Edge Download & History Policy",
            Description =
                "Suppresses the banner that Microsoft Edge displays when it detects it is running on an operating system version that Microsoft has officially dropped from the support matrix. Setting SuppressUnsupportedOSWarning to 1 prevents this warning from appearing. This policy is primarily used in enterprise environments that have authorised extended-support or controlled-deployment Windows versions where the warning is known and managed. The underlying OS support status is unchanged; only the UI notice is hidden.",
            Tags = ["edge", "os support", "warning", "banner", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "SuppressUnsupportedOSWarning", 1)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "SuppressUnsupportedOSWarning")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "SuppressUnsupportedOSWarning", 1)],
            ImpactScore = 1,
            SafetyRating = 4,
            ImpactNote = "OS-unsupported banner is hidden; Edge continues to run on the OS but without updates if not supported.",
        },
    ];
}
