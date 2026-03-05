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
