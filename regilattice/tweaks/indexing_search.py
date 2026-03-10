"""Indexing & Search tweaks.

Covers Windows Search indexer, search suggestions, web results in Start,
indexer performance modes, and search highlight/interests.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_SEARCH = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"
_SEARCH_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"
_SEARCH_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"
_EXPLORER = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
_SEARCH_SETTINGS = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"
_GATHER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager"

# ── 1. Disable Windows Search indexer service ─────────────────────────────────


def _apply_disable_search_indexer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_SVC, "Start", 4)


def _remove_disable_search_indexer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_SVC, "Start", 2)


def _detect_disable_search_indexer() -> bool:
    return SESSION.read_dword(_SEARCH_SVC, "Start") == 4


# ── 2. Disable web search in Start menu ──────────────────────────────────────


def _apply_disable_web_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH, "DisableWebSearch", 1)


def _remove_disable_web_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "DisableWebSearch")


def _detect_disable_web_search() -> bool:
    return SESSION.read_dword(_SEARCH, "DisableWebSearch") == 1


# ── 3. Disable connected search (Bing in Start) ──────────────────────────────


def _apply_disable_connected_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH, "ConnectedSearchUseWeb", 0)


def _remove_disable_connected_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "ConnectedSearchUseWeb")


def _detect_disable_connected_search() -> bool:
    return SESSION.read_dword(_SEARCH, "ConnectedSearchUseWeb") == 0


# ── 4. Disable search highlights / interests ─────────────────────────────────


def _apply_disable_search_highlights(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_CU, "BingSearchEnabled", 0)


def _remove_disable_search_highlights(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_CU, "BingSearchEnabled", 1)


def _detect_disable_search_highlights() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "BingSearchEnabled") == 0


# ── 5. Hide search box / icon on taskbar ─────────────────────────────────────


def _apply_hide_search_box(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_CU, "SearchboxTaskbarMode", 0)


def _remove_hide_search_box(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_CU, "SearchboxTaskbarMode", 2)


def _detect_hide_search_box() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "SearchboxTaskbarMode") == 0


# ── 6. Show search icon only (no box) ────────────────────────────────────────


def _apply_search_icon_only(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_CU, "SearchboxTaskbarMode", 1)


def _remove_search_icon_only(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_CU, "SearchboxTaskbarMode", 2)


def _detect_search_icon_only() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "SearchboxTaskbarMode") == 1


# ── 7. Disable search indexer backoff ─────────────────────────────────────────


def _apply_disable_indexer_backoff(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH, "PreventIndexingLowDiskSpaceMB", 0)


def _remove_disable_indexer_backoff(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "PreventIndexingLowDiskSpaceMB")


def _detect_disable_indexer_backoff() -> bool:
    return SESSION.read_dword(_SEARCH, "PreventIndexingLowDiskSpaceMB") == 0


# ── 8. Disable search suggestions in Start ───────────────────────────────────


def _apply_disable_search_suggest(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled", 0)


def _remove_disable_search_suggest(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled", 1)


def _detect_disable_search_suggest() -> bool:
    return SESSION.read_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled") == 0


# ── 9. Disable cloud content in search ───────────────────────────────────────


def _apply_disable_cloud_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH, "AllowCloudSearch", 0)


def _remove_disable_cloud_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "AllowCloudSearch")


def _detect_disable_cloud_search() -> bool:
    return SESSION.read_dword(_SEARCH, "AllowCloudSearch") == 0


# ── 10. Enhanced indexing mode ────────────────────────────────────────────────


def _apply_enhanced_indexing(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SEARCH_SETTINGS, "IsAADCloudSearchEnabled", 0)
    SESSION.set_dword(_SEARCH_SETTINGS, "IsMSACloudSearchEnabled", 0)


def _remove_enhanced_indexing(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_SEARCH_SETTINGS, "IsAADCloudSearchEnabled")
    SESSION.delete_value(_SEARCH_SETTINGS, "IsMSACloudSearchEnabled")


def _detect_enhanced_indexing() -> bool:
    return SESSION.read_dword(_SEARCH_SETTINGS, "IsAADCloudSearchEnabled") == 0


# ── 11. Disable location for search ──────────────────────────────────────────


def _apply_disable_search_location(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH, "AllowSearchToUseLocation", 0)


def _remove_disable_search_location(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "AllowSearchToUseLocation")


def _detect_disable_search_location() -> bool:
    return SESSION.read_dword(_SEARCH, "AllowSearchToUseLocation") == 0


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="idx-disable-search-indexer",
        label="Disable Windows Search Indexer",
        category="Indexing & Search",
        apply_fn=_apply_disable_search_indexer,
        remove_fn=_remove_disable_search_indexer,
        detect_fn=_detect_disable_search_indexer,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH_SVC],
        description="Disable Windows Search indexer service entirely. Saves CPU/disk but disables fast search. Default: enabled.",
        tags=["indexer", "search", "service", "performance"],
    ),
    TweakDef(
        id="idx-disable-web-search",
        label="Disable Web Search in Start",
        category="Indexing & Search",
        apply_fn=_apply_disable_web_search,
        remove_fn=_remove_disable_web_search,
        detect_fn=_detect_disable_web_search,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description="Disable web search results in Start menu. Policy setting. Default: enabled. Recommended: disabled.",
        tags=["web", "search", "start", "bing"],
    ),
    TweakDef(
        id="idx-disable-connected-search",
        label="Disable Connected Search (Bing)",
        category="Indexing & Search",
        apply_fn=_apply_disable_connected_search,
        remove_fn=_remove_disable_connected_search,
        detect_fn=_detect_disable_connected_search,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description="Disable Bing online results in Windows Search. Default: enabled. Recommended: disabled for privacy.",
        tags=["bing", "connected", "online", "privacy"],
    ),
    TweakDef(
        id="idx-disable-search-highlights",
        label="Disable Search Highlights",
        category="Indexing & Search",
        apply_fn=_apply_disable_search_highlights,
        remove_fn=_remove_disable_search_highlights,
        detect_fn=_detect_disable_search_highlights,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description="Disable trending/interest highlights in search. Default: enabled. Recommended: disabled.",
        tags=["highlights", "interests", "trending", "bing"],
    ),
    TweakDef(
        id="idx-hide-search-box",
        label="Hide Search Box on Taskbar",
        category="Indexing & Search",
        apply_fn=_apply_hide_search_box,
        remove_fn=_remove_hide_search_box,
        detect_fn=_detect_hide_search_box,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description="Completely hide the search box/icon from taskbar. Default: full box shown.",
        tags=["search", "taskbar", "hide", "box"],
    ),
    TweakDef(
        id="idx-search-icon-only",
        label="Show Search Icon Only (No Box)",
        category="Indexing & Search",
        apply_fn=_apply_search_icon_only,
        remove_fn=_remove_search_icon_only,
        detect_fn=_detect_search_icon_only,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description="Show only a small search icon instead of the full box. Default: full box.",
        tags=["search", "icon", "taskbar", "compact"],
    ),
    TweakDef(
        id="idx-disable-indexer-backoff",
        label="Disable Indexer Low-Disk Backoff",
        category="Indexing & Search",
        apply_fn=_apply_disable_indexer_backoff,
        remove_fn=_remove_disable_indexer_backoff,
        detect_fn=_detect_disable_indexer_backoff,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description="Prevent search indexer from pausing when disk space is low. Default: backs off.",
        tags=["indexer", "disk", "space", "backoff"],
    ),
    TweakDef(
        id="idx-disable-search-suggestions",
        label="Disable Search Suggestions",
        category="Indexing & Search",
        apply_fn=_apply_disable_search_suggest,
        remove_fn=_remove_disable_search_suggest,
        detect_fn=_detect_disable_search_suggest,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description="Disable dynamic search suggestions. Default: enabled.",
        tags=["search", "suggestion", "dynamic"],
    ),
    TweakDef(
        id="idx-disable-cloud-search",
        label="Disable Cloud Content in Search",
        category="Indexing & Search",
        apply_fn=_apply_disable_cloud_search,
        remove_fn=_remove_disable_cloud_search,
        detect_fn=_detect_disable_cloud_search,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description="Disable cloud content (OneDrive, M365) from appearing in search. Default: enabled.",
        tags=["cloud", "search", "onedrive", "m365"],
    ),
    TweakDef(
        id="idx-disable-cloud-accounts",
        label="Disable Cloud Account Search",
        category="Indexing & Search",
        apply_fn=_apply_enhanced_indexing,
        remove_fn=_remove_enhanced_indexing,
        detect_fn=_detect_enhanced_indexing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description="Disable AAD/MSA cloud account search integration. Default: enabled.",
        tags=["cloud", "aad", "msa", "account"],
    ),
    TweakDef(
        id="idx-disable-search-location",
        label="Disable Location for Search",
        category="Indexing & Search",
        apply_fn=_apply_disable_search_location,
        remove_fn=_remove_disable_search_location,
        detect_fn=_detect_disable_search_location,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description="Prevent search from using device location. Default: enabled. Recommended: disabled for privacy.",
        tags=["location", "search", "privacy", "gps"],
    ),
]


# -- Disable Dynamic Search Box Content -------------------------------------------


def _apply_disable_dynamic_searchbox(*, require_admin: bool = True) -> None:
    SESSION.log("Indexing & Search: disable dynamic search box content")
    SESSION.backup([_SEARCH_SETTINGS], "DynamicSearchBox")
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled", 0)


def _remove_disable_dynamic_searchbox(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled")


def _detect_disable_dynamic_searchbox() -> bool:
    return SESSION.read_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled") == 0


# -- Disable Recent Search Suggestions --------------------------------------------


def _apply_disable_recent_search(*, require_admin: bool = True) -> None:
    SESSION.log("Indexing & Search: disable recent search suggestions")
    SESSION.backup([_SEARCH_SETTINGS], "RecentSearch")
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDeviceSearchHistoryEnabled", 0)


def _remove_disable_recent_search(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_SEARCH_SETTINGS, "IsDeviceSearchHistoryEnabled")


def _detect_disable_recent_search() -> bool:
    return SESSION.read_dword(_SEARCH_SETTINGS, "IsDeviceSearchHistoryEnabled") == 0


TWEAKS += [
    TweakDef(
        id="idx-disable-dynamic-searchbox",
        label="Disable Dynamic Search Box Content",
        category="Indexing & Search",
        apply_fn=_apply_disable_dynamic_searchbox,
        remove_fn=_remove_disable_dynamic_searchbox,
        detect_fn=_detect_disable_dynamic_searchbox,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description=(
            "Disables dynamic content in the search box (IsDynamicSearchBoxEnabled=0). "
            "Removes trending searches and images from the search experience. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["search", "dynamic", "searchbox", "privacy"],
    ),
    TweakDef(
        id="idx-disable-recent-search",
        label="Disable Recent Search Suggestions",
        category="Indexing & Search",
        apply_fn=_apply_disable_recent_search,
        remove_fn=_remove_disable_recent_search,
        detect_fn=_detect_disable_recent_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description=(
            "Disables recent search history suggestions in Windows Search. "
            "Prevents previously searched terms from appearing as suggestions. "
            "Default: enabled. Recommended: disabled for privacy."
        ),
        tags=["search", "recent", "history", "suggestions", "privacy"],
    ),
]


# -- Reduce Indexer I/O Footprint ---------------------------------------------


def _apply_reduce_indexer_io(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: raise gathering disk-space threshold")
    SESSION.backup([_GATHER], "ReduceIndexerIO")
    SESSION.set_dword(_GATHER, "DesiredRemainingDiskSpaceMB", 5000)


def _remove_reduce_indexer_io(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GATHER, "DesiredRemainingDiskSpaceMB")


def _detect_reduce_indexer_io() -> bool:
    val = SESSION.read_dword(_GATHER, "DesiredRemainingDiskSpaceMB")
    return val is not None and val >= 5000


# -- Disable Outlook Indexing -------------------------------------------------


def _apply_disable_outlook_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: disable Outlook data indexing")
    SESSION.backup([_SEARCH], "OutlookIndexing")
    SESSION.set_dword(_SEARCH, "PreventIndexingOutlook", 1)


def _remove_disable_outlook_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "PreventIndexingOutlook")


def _detect_disable_outlook_indexing() -> bool:
    return SESSION.read_dword(_SEARCH, "PreventIndexingOutlook") == 1


# -- Prevent Indexing on Battery Power ----------------------------------------


def _apply_prevent_indexing_battery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: prevent indexing when on battery")
    SESSION.backup([_SEARCH], "IndexOnBattery")
    SESSION.set_dword(_SEARCH, "PreventIndexOnBattery", 1)


def _remove_prevent_indexing_battery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "PreventIndexOnBattery")


def _detect_prevent_indexing_battery() -> bool:
    return SESSION.read_dword(_SEARCH, "PreventIndexOnBattery") == 1


TWEAKS += [
    TweakDef(
        id="idx-reduce-indexer-io",
        label="Reduce Indexer Disk I/O",
        category="Indexing & Search",
        apply_fn=_apply_reduce_indexer_io,
        remove_fn=_remove_reduce_indexer_io,
        detect_fn=_detect_reduce_indexer_io,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GATHER],
        description=(
            "Sets the Gathering Manager disk-space threshold to 5 GB, causing "
            "the indexer to back off earlier and reduce disk I/O pressure. "
            "Default: not set. Recommended: Apply on systems with slow disks."
        ),
        tags=["search", "indexer", "disk", "io", "performance"],
    ),
    TweakDef(
        id="idx-disable-outlook-indexing",
        label="Disable Outlook Indexing",
        category="Indexing & Search",
        apply_fn=_apply_disable_outlook_indexing,
        remove_fn=_remove_disable_outlook_indexing,
        detect_fn=_detect_disable_outlook_indexing,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description=(
            "Prevents Windows Search from indexing Outlook email data via "
            "policy. Reduces indexer CPU and disk usage on large mailboxes. "
            "Default: indexed. Recommended: Disabled if Outlook search unused."
        ),
        tags=["search", "outlook", "email", "indexing", "performance"],
    ),
    TweakDef(
        id="idx-prevent-indexing-battery",
        label="Prevent Indexing on Battery Power",
        category="Indexing & Search",
        apply_fn=_apply_prevent_indexing_battery,
        remove_fn=_remove_prevent_indexing_battery,
        detect_fn=_detect_prevent_indexing_battery,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description=(
            "Prevents the Windows Search indexer from running when on battery "
            "power. Significantly improves laptop battery life. "
            "Default: indexing continues. Recommended: Apply on laptops."
        ),
        tags=["search", "indexer", "battery", "power", "laptop"],
    ),
]


# ── Disable Bing Search in Start Menu ─────────────────────────────────────────


def _apply_disable_bing_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: disable Bing search in Start menu")
    SESSION.backup([_SEARCH_CU], "BingSearch")
    SESSION.set_dword(_SEARCH_CU, "BingSearchEnabled", 0)


def _remove_disable_bing_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "BingSearchEnabled")


def _detect_disable_bing_search() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "BingSearchEnabled") == 0


# ── Limit Indexer Worker Threads ──────────────────────────────────────────────


def _apply_limit_indexer_threads(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: limit indexer CPU threads to 1")
    SESSION.backup([_GATHER], "IndexerThreads")
    SESSION.set_dword(_GATHER, "GatheringMaxServerThreadCount", 1)


def _remove_limit_indexer_threads(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_GATHER, "GatheringMaxServerThreadCount")


def _detect_limit_indexer_threads() -> bool:
    val = SESSION.read_dword(_GATHER, "GatheringMaxServerThreadCount")
    return val is not None and val <= 1


# ── Disable Safe Search ───────────────────────────────────────────────────────


def _apply_disable_safe_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: disable SafeSearch filter")
    SESSION.backup([_SEARCH_CU], "SafeSearch")
    SESSION.set_dword(_SEARCH_CU, "SafeSearch", 0)


def _remove_disable_safe_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "SafeSearch")


def _detect_disable_safe_search() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "SafeSearch") == 0


# ── Disable Indexing of Network Locations ─────────────────────────────────────


def _apply_disable_network_index(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: disable indexing of network locations")
    SESSION.backup([_SEARCH], "NetworkIndex")
    SESSION.set_dword(_SEARCH, "PreventIndexingNetworkLocations", 1)


def _remove_disable_network_index(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "PreventIndexingNetworkLocations")


def _detect_disable_network_index() -> bool:
    return SESSION.read_dword(_SEARCH, "PreventIndexingNetworkLocations") == 1


# ── Disable Microsoft Account Cloud Search ────────────────────────────────────


def _apply_disable_msa_cloud_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Indexing & Search: disable MSA cloud search integration")
    SESSION.backup([_SEARCH_CU], "MSACloudSearch")
    SESSION.set_dword(_SEARCH_CU, "IsMSACloudSearchEnabled", 0)


def _remove_disable_msa_cloud_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "IsMSACloudSearchEnabled")


def _detect_disable_msa_cloud_search() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "IsMSACloudSearchEnabled") == 0


TWEAKS += [
    TweakDef(
        id="idx-disable-bing-search",
        label="Disable Bing Search in Start Menu",
        category="Indexing & Search",
        apply_fn=_apply_disable_bing_search,
        remove_fn=_remove_disable_bing_search,
        detect_fn=_detect_disable_bing_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description=(
            "Sets BingSearchEnabled=0 in user Search settings to prevent Bing web results "
            "from appearing in Start menu search. "
            "Default: enabled. Recommended: disabled for faster local search."
        ),
        tags=["search", "bing", "start", "web-results", "privacy"],
    ),
    TweakDef(
        id="idx-limit-indexer-threads",
        label="Limit Indexer CPU Threads",
        category="Indexing & Search",
        apply_fn=_apply_limit_indexer_threads,
        remove_fn=_remove_limit_indexer_threads,
        detect_fn=_detect_limit_indexer_threads,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_GATHER],
        description=(
            "Limits the Windows Search indexer to 1 worker thread via GatheringMaxServerThreadCount, "
            "reducing CPU load during burst indexing. "
            "Default: uncapped. Recommended: Apply on dual-core systems."
        ),
        tags=["search", "indexer", "cpu", "performance", "threads"],
    ),
    TweakDef(
        id="idx-disable-safe-search",
        label="Disable SafeSearch Filter",
        category="Indexing & Search",
        apply_fn=_apply_disable_safe_search,
        remove_fn=_remove_disable_safe_search,
        detect_fn=_detect_disable_safe_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description=(
            "Sets SafeSearch=0 in Windows Search settings, disabling the content filter "
            "that restricts explicit content in search results. "
            "Default: moderate (1). Recommended: off for unrestricted results."
        ),
        tags=["search", "safe-search", "filter", "content"],
    ),
    TweakDef(
        id="idx-disable-network-index",
        label="Disable Indexing of Network Locations",
        category="Indexing & Search",
        apply_fn=_apply_disable_network_index,
        remove_fn=_remove_disable_network_index,
        detect_fn=_detect_disable_network_index,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SEARCH],
        description=(
            "Prevents Windows Search from indexing mapped network drives and UNC paths via policy. "
            "Reduces indexer CPU and network load. "
            "Default: allowed. Recommended: Disabled on slow or corporate networks."
        ),
        tags=["search", "network", "indexer", "policy", "performance"],
    ),
    TweakDef(
        id="idx-disable-msa-cloud-search",
        label="Disable Microsoft Account Cloud Search",
        category="Indexing & Search",
        apply_fn=_apply_disable_msa_cloud_search,
        remove_fn=_remove_disable_msa_cloud_search,
        detect_fn=_detect_disable_msa_cloud_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description=(
            "Disables Microsoft Account cloud search integration in Windows Search. "
            "Prevents OneDrive and MSA content from appearing in local search results. "
            "Default: enabled. Recommended: disabled for privacy."
        ),
        tags=["search", "cloud", "msa", "microsoft-account", "privacy"],
    ),
]
