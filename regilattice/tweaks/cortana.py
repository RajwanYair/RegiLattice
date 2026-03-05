"""Cortana and search-related registry tweaks.

Covers: Cortana, web search in Start, search highlights,
Bing suggestions, and search box visibility.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_SEARCH = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\Windows Search"
)
_SEARCH_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Search"
)
_CORTANA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\Windows Search"
)
_EXPLORER = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)
_WSEARCH_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"
_SEARCH_SETTINGS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\SearchSettings"
)


# ── Disable Cortana ──────────────────────────────────────────────────────────


def _apply_disable_cortana(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cortana: disable on lock screen")
    SESSION.backup([_CORTANA], "CortanaLock")
    SESSION.set_dword(_CORTANA, "AllowCortanaAboveLock", 0)


def _remove_disable_cortana(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CORTANA, "AllowCortanaAboveLock")


def _detect_disable_cortana() -> bool:
    return SESSION.read_dword(_CORTANA, "AllowCortanaAboveLock") == 0


# ── Disable Web Search in Start Menu ─────────────────────────────────────────


def _apply_disable_web_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable Bing web results in Start menu")
    SESSION.backup([_SEARCH_CU], "WebSearch")
    SESSION.set_dword(_SEARCH_CU, "BingSearchEnabled", 0)
    SESSION.set_dword(_SEARCH_CU, "CortanaConsent", 0)


def _remove_disable_web_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "BingSearchEnabled")
    SESSION.delete_value(_SEARCH_CU, "CortanaConsent")


def _detect_disable_web_search() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "BingSearchEnabled") == 0


# ── Disable Search Highlights ────────────────────────────────────────────────


def _apply_disable_search_highlights(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable search highlights (Bing curated content)")
    SESSION.backup([_SEARCH], "SearchHighlights")
    SESSION.set_dword(_SEARCH, "EnableDynamicContentInWSB", 0)


def _remove_disable_search_highlights(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "EnableDynamicContentInWSB")


def _detect_disable_search_highlights() -> bool:
    return SESSION.read_dword(_SEARCH, "EnableDynamicContentInWSB") == 0


# ── Hide Search Box from Taskbar ─────────────────────────────────────────────


def _apply_hide_search_box(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: hide search box on taskbar")
    SESSION.backup([_EXPLORER], "SearchBox")
    SESSION.set_dword(_EXPLORER, "SearchboxTaskbarMode", 0)  # 0=Hidden


def _remove_hide_search_box(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_EXPLORER, "SearchboxTaskbarMode", 1)  # 1=Icon


def _detect_hide_search_box() -> bool:
    return SESSION.read_dword(_EXPLORER, "SearchboxTaskbarMode") == 0


# ── Disable Cortana Completely ─────────────────────────────────────────────


def _apply_disable_cortana_completely(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Cortana: disable entirely via policy")
    SESSION.backup([_CORTANA], "CortanaDisable")
    SESSION.set_dword(_CORTANA, "AllowCortana", 0)


def _remove_disable_cortana_completely(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CORTANA, "AllowCortana")


def _detect_disable_cortana_completely() -> bool:
    return SESSION.read_dword(_CORTANA, "AllowCortana") == 0


# ── Disable Cloud Content Search ───────────────────────────────────────────


def _apply_disable_cloud_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable cloud content in search results")
    SESSION.backup([_SEARCH_CU], "CloudSearch")
    SESSION.set_dword(_SEARCH_CU, "AllowCloudSearch", 0)
    SESSION.set_dword(_SEARCH_CU, "AllowSearchToUseLocation", 0)


def _remove_disable_cloud_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "AllowCloudSearch")
    SESSION.delete_value(_SEARCH_CU, "AllowSearchToUseLocation")


def _detect_disable_cloud_search() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "AllowCloudSearch") == 0


# ── Disable Windows Search Indexing Service ─────────────────────────────────


def _apply_disable_search_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable Windows Search indexing service")
    SESSION.backup([_WSEARCH_SVC], "SearchIndexing")
    SESSION.set_dword(_WSEARCH_SVC, "Start", 4)


def _remove_disable_search_indexing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_WSEARCH_SVC, "Start", 2)


def _detect_disable_search_indexing() -> bool:
    return SESSION.read_dword(_WSEARCH_SVC, "Start") == 4


# ── Disable Search Highlights/Tips (User) ────────────────────────────────────


def _apply_disable_search_highlights_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable search highlights / tips")
    SESSION.backup([_SEARCH_SETTINGS], "SearchHighlightsTips")
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled", 0)


def _remove_disable_search_highlights_tips(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled", 1)


def _detect_disable_search_highlights_tips() -> bool:
    return SESSION.read_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled") == 0


# ── Disable Cloud Content in Windows Search ──────────────────────────────────


def _apply_disable_cloud_search_content(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable cloud content in Windows Search")
    SESSION.backup([_SEARCH_SETTINGS], "CloudSearchContent")
    SESSION.set_dword(_SEARCH_SETTINGS, "IsAADCloudSearchEnabled", 0)
    SESSION.set_dword(_SEARCH_SETTINGS, "IsMSACloudSearchEnabled", 0)


def _remove_disable_cloud_search_content(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_SETTINGS, "IsAADCloudSearchEnabled", 1)
    SESSION.set_dword(_SEARCH_SETTINGS, "IsMSACloudSearchEnabled", 1)


def _detect_disable_cloud_search_content() -> bool:
    return (
        SESSION.read_dword(_SEARCH_SETTINGS, "IsAADCloudSearchEnabled") == 0
        and SESSION.read_dword(_SEARCH_SETTINGS, "IsMSACloudSearchEnabled") == 0
    )


# ── Disable Enhanced Search (Find My Files) ──────────────────────────────────


def _apply_disable_find_my_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable enhanced search (Find My Files)")
    SESSION.backup([_SEARCH_SETTINGS], "FindMyFiles")
    SESSION.set_dword(_SEARCH_SETTINGS, "SearchMode", 0)


def _remove_disable_find_my_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_SETTINGS, "SearchMode", 1)


def _detect_disable_find_my_files() -> bool:
    return SESSION.read_dword(_SEARCH_SETTINGS, "SearchMode") == 0


# ── Disable Windows Search Location Access ───────────────────────────────────


def _apply_disable_search_location(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable location access for Windows Search")
    SESSION.backup([_SEARCH_CU], "SearchLocation")
    SESSION.set_dword(_SEARCH_CU, "AllowSearchToUseLocation", 0)


def _remove_disable_search_location(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_CU, "AllowSearchToUseLocation", 1)


def _detect_disable_search_location() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "AllowSearchToUseLocation") == 0


# ── Disable Search Highlights (Dynamic Box) ───────────────────────────────


def _apply_disable_search_highlights_box(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable dynamic search box highlights")
    SESSION.backup([_SEARCH_SETTINGS], "SearchHighlightsBox")
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled", 0)


def _remove_disable_search_highlights_box(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled", 1)


def _detect_disable_search_highlights_box() -> bool:
    return SESSION.read_dword(_SEARCH_SETTINGS, "IsDynamicSearchBoxEnabled") == 0


# ── Disable Cloud Search (Policy) ─────────────────────────────────────────


def _apply_disable_cloud_search_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Search: disable cloud search via policy")
    SESSION.backup([_SEARCH], "CloudSearchPolicy")
    SESSION.set_dword(_SEARCH, "AllowCloudSearch", 0)


def _remove_disable_cloud_search_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH, "AllowCloudSearch")


def _detect_disable_cloud_search_policy() -> bool:
    return SESSION.read_dword(_SEARCH, "AllowCloudSearch") == 0


# ── Disable Search Box Suggestions ───────────────────────────────────────────

_EXPLORER_POLICIES = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"


def _apply_disable_search_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cortana: disable web results in search box")
    SESSION.backup([_EXPLORER_POLICIES], "SearchBoxSuggestions")
    SESSION.set_dword(_EXPLORER_POLICIES, "DisableSearchBoxSuggestions", 1)


def _remove_disable_search_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXPLORER_POLICIES, "DisableSearchBoxSuggestions")


def _detect_disable_search_suggestions() -> bool:
    return SESSION.read_dword(_EXPLORER_POLICIES, "DisableSearchBoxSuggestions") == 1


# ── Disable Bing Search Results ──────────────────────────────────────────────


def _apply_disable_bing_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Cortana: disable Bing search results in Start menu")
    SESSION.backup([_SEARCH_CU], "BingSearch")
    SESSION.set_dword(_SEARCH_CU, "BingSearchEnabled", 0)


def _remove_disable_bing_search(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "BingSearchEnabled")


def _detect_disable_bing_search() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "BingSearchEnabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-cortana-lockscreen",
        label="Disable Cortana on Lock Screen",
        category="Cortana & Search",
        apply_fn=_apply_disable_cortana,
        remove_fn=_remove_disable_cortana,
        detect_fn=_detect_disable_cortana,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CORTANA],
        description="Disables Cortana voice assistant on the lock screen.",
        tags=["cortana", "privacy", "lockscreen"],
    ),
    TweakDef(
        id="disable-web-search",
        label="Disable Web Search in Start Menu",
        category="Cortana & Search",
        apply_fn=_apply_disable_web_search,
        remove_fn=_remove_disable_web_search,
        detect_fn=_detect_disable_web_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description=(
            "Disables Bing web results in Start menu search "
            "via CurrentUser registry keys."
        ),
        tags=["search", "privacy", "bing"],
    ),
    TweakDef(
        id="disable-search-highlights",
        label="Disable Search Highlights",
        category="Cortana & Search",
        apply_fn=_apply_disable_search_highlights,
        remove_fn=_remove_disable_search_highlights,
        detect_fn=_detect_disable_search_highlights,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SEARCH],
        description=(
            "Removes the Bing-curated 'Search Highlights' content "
            "from the Windows search box."
        ),
        tags=["search", "bing", "ux"],
    ),
    TweakDef(
        id="hide-search-box",
        label="Hide Taskbar Search Box",
        category="Cortana & Search",
        apply_fn=_apply_hide_search_box,
        remove_fn=_remove_hide_search_box,
        detect_fn=_detect_hide_search_box,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER],
        description="Hides the search box / icon from the taskbar.",
        tags=["search", "taskbar", "ux"],
    ),
    TweakDef(
        id="disable-cortana-completely",
        label="Disable Cortana Entirely",
        category="Cortana & Search",
        apply_fn=_apply_disable_cortana_completely,
        remove_fn=_remove_disable_cortana_completely,
        detect_fn=_detect_disable_cortana_completely,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CORTANA],
        description="Completely disables Cortana via Group Policy.",
        tags=["cortana", "privacy", "assistant"],
    ),
    TweakDef(
        id="disable-cloud-search",
        label="Disable Cloud Content Search",
        category="Cortana & Search",
        apply_fn=_apply_disable_cloud_search,
        remove_fn=_remove_disable_cloud_search,
        detect_fn=_detect_disable_cloud_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description="Disables cloud content and location-based results in Windows search.",
        tags=["search", "cloud", "privacy"],
    ),
    TweakDef(
        id="disable-search-indexing",
        label="Disable Windows Search Indexing Service",
        category="Cortana & Search",
        apply_fn=_apply_disable_search_indexing,
        remove_fn=_remove_disable_search_indexing,
        detect_fn=_detect_disable_search_indexing,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WSEARCH_SVC],
        description="Disables the Windows Search indexing service entirely.",
        tags=["search", "cortana", "indexing", "performance"],
    ),
    TweakDef(
        id="disable-search-highlights-dynamic",
        label="Disable Dynamic Search Highlights",
        category="Cortana & Search",
        apply_fn=_apply_disable_search_highlights_tips,
        remove_fn=_remove_disable_search_highlights_tips,
        detect_fn=_detect_disable_search_highlights_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description="Disables dynamic search highlights and tips in the search box.",
        tags=["search", "cortana", "highlights", "ux"],
    ),
    TweakDef(
        id="disable-cloud-search-aadmsa",
        label="Disable AAD/MSA Cloud Search",
        category="Cortana & Search",
        apply_fn=_apply_disable_cloud_search_content,
        remove_fn=_remove_disable_cloud_search_content,
        detect_fn=_detect_disable_cloud_search_content,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description=(
            "Disables AAD and MSA cloud search content "
            "in Windows Search results."
        ),
        tags=["search", "cortana", "cloud", "privacy"],
    ),
    TweakDef(
        id="disable-find-my-files",
        label="Disable Enhanced Search (Find My Files)",
        category="Cortana & Search",
        apply_fn=_apply_disable_find_my_files,
        remove_fn=_remove_disable_find_my_files,
        detect_fn=_detect_disable_find_my_files,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description="Sets search mode to classic, disabling enhanced Find My Files indexing.",
        tags=["search", "cortana", "indexing", "privacy"],
    ),
    TweakDef(
        id="disable-search-location",
        label="Disable Windows Search Location Access",
        category="Cortana & Search",
        apply_fn=_apply_disable_search_location,
        remove_fn=_remove_disable_search_location,
        detect_fn=_detect_disable_search_location,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description="Prevents Windows Search from using device location.",
        tags=["search", "cortana", "location", "privacy"],
    ),
    TweakDef(
        id="cortana-disable-search-highlights",
        label="Disable Search Highlights",
        category="Cortana & Search",
        apply_fn=_apply_disable_search_highlights_box,
        remove_fn=_remove_disable_search_highlights_box,
        detect_fn=_detect_disable_search_highlights_box,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_SETTINGS],
        description=(
            "Disables search highlights (trending searches, news) in the "
            "Windows Search box. Reduces distractions and network traffic. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["cortana", "search", "highlights", "performance"],
    ),
    TweakDef(
        id="cortana-disable-cloud-search",
        label="Disable Cloud Search",
        category="Cortana & Search",
        apply_fn=_apply_disable_cloud_search_policy,
        remove_fn=_remove_disable_cloud_search_policy,
        detect_fn=_detect_disable_cloud_search_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SEARCH],
        description=(
            "Disables cloud content in Windows Search results. Only shows "
            "local files and settings. Default: Enabled. "
            "Recommended: Disabled for privacy."
        ),
        tags=["cortana", "search", "cloud", "privacy"],
    ),
    TweakDef(
        id="cortana-disable-web-search",
        label="Disable Search Box Web Suggestions",
        category="Cortana & Search",
        apply_fn=_apply_disable_search_suggestions,
        remove_fn=_remove_disable_search_suggestions,
        detect_fn=_detect_disable_search_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_POLICIES],
        description=(
            "Disables web suggestions and results in the Windows Search "
            "box. Only shows local results. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["cortana", "search", "web", "suggestions", "privacy"],
    ),
    TweakDef(
        id="cortana-disable-bing-search",
        label="Disable Bing Search in Start Menu",
        category="Cortana & Search",
        apply_fn=_apply_disable_bing_search,
        remove_fn=_remove_disable_bing_search,
        detect_fn=_detect_disable_bing_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description=(
            "Disables Bing search results integration in the Start menu "
            "and taskbar search. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["cortana", "bing", "search", "start-menu", "privacy"],
    ),
]


# ── Disable Bing Integration in Start ────────────────────────────────────────


def _apply_bing_in_start_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Bing integration in Start menu")
    SESSION.backup([_SEARCH_CU], "BingInStart")
    SESSION.set_dword(_SEARCH_CU, "ConnectedSearchUseWeb", 0)
    SESSION.set_dword(_SEARCH_CU, "ConnectedSearchUseWebOverMeteredConnections", 0)


def _remove_bing_in_start_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "ConnectedSearchUseWeb")
    SESSION.delete_value(_SEARCH_CU, "ConnectedSearchUseWebOverMeteredConnections")


def _detect_bing_in_start_off() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "ConnectedSearchUseWeb") == 0


# ── Disable Search Box Suggestions ──────────────────────────────────────────


def _apply_searchbox_suggestions_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable search box suggestions")
    SESSION.backup([_SEARCH_CU], "SearchBoxSuggestionsOff")
    SESSION.set_dword(_SEARCH_CU, "SearchboxTaskbarMode", 0)
    SESSION.set_dword(_SEARCH_CU, "AllowSearchToUseLocation", 0)


def _remove_searchbox_suggestions_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_CU, "SearchboxTaskbarMode")
    SESSION.delete_value(_SEARCH_CU, "AllowSearchToUseLocation")


def _detect_searchbox_suggestions_off() -> bool:
    return SESSION.read_dword(_SEARCH_CU, "SearchboxTaskbarMode") == 0


TWEAKS += [
    TweakDef(
        id="cortana-block-bing-in-start",
        label="Disable Bing Integration in Start",
        category="Cortana & Search",
        apply_fn=_apply_bing_in_start_off,
        remove_fn=_remove_bing_in_start_off,
        detect_fn=_detect_bing_in_start_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description=(
            "Disables Bing web search integration in Start menu and "
            "connected search features including metered connections. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["cortana", "bing", "start", "connected-search", "privacy"],
    ),
    TweakDef(
        id="cortana-disable-searchbox-suggestions",
        label="Disable Search Box Suggestions",
        category="Cortana & Search",
        apply_fn=_apply_searchbox_suggestions_off,
        remove_fn=_remove_searchbox_suggestions_off,
        detect_fn=_detect_searchbox_suggestions_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_CU],
        description=(
            "Disables search box taskbar suggestions and location-based "
            "search results. Only shows local results. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["cortana", "search", "suggestions", "taskbar", "privacy"],
    ),
]
