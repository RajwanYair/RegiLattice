"""Cortana and search-related registry tweaks.

Covers: Cortana, web search in Start, search highlights,
Bing suggestions, and search box visibility.
"""

from __future__ import annotations

from typing import List

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


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
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
]
