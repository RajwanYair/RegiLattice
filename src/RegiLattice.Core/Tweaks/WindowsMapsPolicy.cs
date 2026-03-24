// Connected Search & Maps Policy — Sprint 145
// Slug "wmaps" — Windows Maps auto-download + Connected Search privacy settings
// via Windows Search Group Policy (distinct from Cortana.cs values).
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search  (unused values)
// HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Windows Search
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class WindowsMapsPolicy
{
    private const string Maps = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Maps";

    private const string WinSearch = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

    private const string WinSearchCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Windows Search";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wmaps-disable-auto-download",
            Label = "Maps: Disable automatic map data download and update",
            Category = "Windows Maps Policy",
            Description =
                "Sets AutoDownloadAndUpdateMapData=0 in the Maps policy key. Prevents the Maps app "
                + "from automatically downloading and updating offline map data in the background.",
            Tags = ["maps", "download", "background", "data", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Maps, "AutoDownloadAndUpdateMapData", 0)],
            RemoveOps = [RegOp.DeleteValue(Maps, "AutoDownloadAndUpdateMapData")],
            DetectOps = [RegOp.CheckDword(Maps, "AutoDownloadAndUpdateMapData", 0)],
        },
        new TweakDef
        {
            Id = "wmaps-disable-untriggered-network",
            Label = "Maps: Disable untriggered network traffic from Maps settings page",
            Category = "Windows Maps Policy",
            Description =
                "Sets AllowUntriggeredNetworkTrafficOnSettingsPage=0. Prevents the Maps settings page "
                + "from making unsolicited network requests to check for map updates or new regions.",
            Tags = ["maps", "network", "traffic", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Maps, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
            RemoveOps = [RegOp.DeleteValue(Maps, "AllowUntriggeredNetworkTrafficOnSettingsPage")],
            DetectOps = [RegOp.CheckDword(Maps, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
        },
        new TweakDef
        {
            Id = "wmaps-no-connected-search-privacy",
            Label = "Search: Enforce maximum privacy for Connected Search",
            Category = "Windows Maps Policy",
            Description =
                "Sets ConnectedSearchPrivacy=3 (machine policy). Value 3 blocks web search from "
                + "the taskbar search box, enforcing the strictest privacy posture.",
            Tags = ["search", "connected-search", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearch, "ConnectedSearchPrivacy", 3)],
            RemoveOps = [RegOp.DeleteValue(WinSearch, "ConnectedSearchPrivacy")],
            DetectOps = [RegOp.CheckDword(WinSearch, "ConnectedSearchPrivacy", 3)],
        },
        new TweakDef
        {
            Id = "wmaps-enforce-safe-search-strict",
            Label = "Search: Enforce strict SafeSearch in Windows Search",
            Category = "Windows Maps Policy",
            Description =
                "Sets ConnectedSearchSafeSearch=2 (machine policy). Value 2 = Strict; filters adult "
                + "content from Windows Search results. Default: 1 (moderate).",
            Tags = ["search", "safe-search", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearch, "ConnectedSearchSafeSearch", 2)],
            RemoveOps = [RegOp.DeleteValue(WinSearch, "ConnectedSearchSafeSearch")],
            DetectOps = [RegOp.CheckDword(WinSearch, "ConnectedSearchSafeSearch", 2)],
        },
        new TweakDef
        {
            Id = "wmaps-disable-search-highlights",
            Label = "Search: Disable dynamic search highlights in the taskbar",
            Category = "Windows Maps Policy",
            Description =
                "Sets AllowSearchHighlights=0 (machine policy). Prevents Windows from displaying "
                + "rotating 'Search Highlights' icons and animations in the taskbar search box.",
            Tags = ["search", "search-highlights", "taskbar", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearch, "AllowSearchHighlights", 0)],
            RemoveOps = [RegOp.DeleteValue(WinSearch, "AllowSearchHighlights")],
            DetectOps = [RegOp.CheckDword(WinSearch, "AllowSearchHighlights", 0)],
        },
        new TweakDef
        {
            Id = "wmaps-disable-cortana-aad",
            Label = "Search: Disable Cortana for Azure AD accounts (machine policy)",
            Category = "Windows Maps Policy",
            Description =
                "Sets AllowCortanaInAAD=0 in Windows Search policy. Prevents Cortana from being "
                + "available for Azure Active Directory (Microsoft Entra ID) signed-in accounts.",
            Tags = ["search", "cortana", "aad", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearch, "AllowCortanaInAAD", 0)],
            RemoveOps = [RegOp.DeleteValue(WinSearch, "AllowCortanaInAAD")],
            DetectOps = [RegOp.CheckDword(WinSearch, "AllowCortanaInAAD", 0)],
        },
        new TweakDef
        {
            Id = "wmaps-user-no-connected-search-privacy",
            Label = "Search (user): Enforce maximum Connected Search privacy per user",
            Category = "Windows Maps Policy",
            Description =
                "Sets ConnectedSearchPrivacy=3 at HKCU user-policy scope. Enforces per-user strictest "
                + "Connected Search privacy for the current signed-in account.",
            Tags = ["search", "connected-search", "privacy", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearchCu, "ConnectedSearchPrivacy", 3)],
            RemoveOps = [RegOp.DeleteValue(WinSearchCu, "ConnectedSearchPrivacy")],
            DetectOps = [RegOp.CheckDword(WinSearchCu, "ConnectedSearchPrivacy", 3)],
        },
        new TweakDef
        {
            Id = "wmaps-user-enforce-safe-search",
            Label = "Search (user): Enforce strict SafeSearch per user",
            Category = "Windows Maps Policy",
            Description =
                "Sets ConnectedSearchSafeSearch=2 at HKCU user-policy scope. Enforces strict search "
                + "result filtering for the current user's Windows Search results.",
            Tags = ["search", "safe-search", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearchCu, "ConnectedSearchSafeSearch", 2)],
            RemoveOps = [RegOp.DeleteValue(WinSearchCu, "ConnectedSearchSafeSearch")],
            DetectOps = [RegOp.CheckDword(WinSearchCu, "ConnectedSearchSafeSearch", 2)],
        },
        new TweakDef
        {
            Id = "wmaps-user-disable-search-highlights",
            Label = "Search (user): Disable search highlights per user",
            Category = "Windows Maps Policy",
            Description =
                "Sets AllowSearchHighlights=0 at HKCU user-policy scope. Hides the rotating "
                + "Search Highlights animations in the taskbar for the current user.",
            Tags = ["search", "search-highlights", "taskbar", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearchCu, "AllowSearchHighlights", 0)],
            RemoveOps = [RegOp.DeleteValue(WinSearchCu, "AllowSearchHighlights")],
            DetectOps = [RegOp.CheckDword(WinSearchCu, "AllowSearchHighlights", 0)],
        },
        new TweakDef
        {
            Id = "wmaps-user-disable-cortana-aad",
            Label = "Search (user): Disable Cortana for AAD accounts per user",
            Category = "Windows Maps Policy",
            Description =
                "Sets AllowCortanaInAAD=0 at HKCU user-policy scope. Disables Cortana for Azure AD "
                + "accounts at the individual user level, complementing the machine-wide policy.",
            Tags = ["search", "cortana", "aad", "enterprise", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WinSearchCu, "AllowCortanaInAAD", 0)],
            RemoveOps = [RegOp.DeleteValue(WinSearchCu, "AllowCortanaInAAD")],
            DetectOps = [RegOp.CheckDword(WinSearchCu, "AllowCortanaInAAD", 0)],
        },
    ];
}
