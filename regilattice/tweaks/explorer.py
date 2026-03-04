"""Windows Explorer tweaks — file display, navigation, and UX enhancements.

Covers: file extensions, hidden files, Quick Access, compact view,
folder thumbnails, gallery pane, and more.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Shared key paths ────────────────────────────────────────────────────────

_ADV = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
_EXPLORER = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"
_CABINETSTATE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState"
)
_SEARCH_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"
)
_GALLERY_KEY = (
    r"HKEY_CURRENT_USER\Software\Classes\CLSID"
    r"\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}"
)
_BAGS_KEY = (
    r"HKEY_CURRENT_USER\Software\Classes\Local Settings"
    r"\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell"
)
_CLSID_QA = "{22877a6d-37a1-461a-91b0-dbda5aaebc99}"
_QA_KEYS = [
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{_CLSID_QA}",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{_CLSID_QA}\ShellFolder",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\Wow6432Node\CLSID\{_CLSID_QA}",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\Wow6432Node\CLSID\{_CLSID_QA}\ShellFolder",
]


# ── Show File Extensions ────────────────────────────────────────────────────


def _apply_show_extensions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: show file extensions")
    SESSION.backup([_ADV], "ShowExtensions")
    SESSION.set_dword(_ADV, "HideFileExt", 0)


def _remove_show_extensions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "ShowExtensions_Remove")
    SESSION.set_dword(_ADV, "HideFileExt", 1)


def _detect_show_extensions() -> bool:
    return SESSION.read_dword(_ADV, "HideFileExt") == 0


# ── Show Hidden Files ───────────────────────────────────────────────────────


def _apply_show_hidden(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: show hidden files")
    SESSION.backup([_ADV], "ShowHidden")
    SESSION.set_dword(_ADV, "Hidden", 1)


def _remove_show_hidden(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "ShowHidden_Remove")
    SESSION.set_dword(_ADV, "Hidden", 2)


def _detect_show_hidden() -> bool:
    return SESSION.read_dword(_ADV, "Hidden") == 1


# ── Show Protected OS Files ─────────────────────────────────────────────────


def _apply_show_super_hidden(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: show super-hidden files")
    SESSION.backup([_ADV], "SuperHidden")
    SESSION.set_dword(_ADV, "ShowSuperHidden", 1)


def _remove_show_super_hidden(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "SuperHidden_Remove")
    SESSION.set_dword(_ADV, "ShowSuperHidden", 0)


def _detect_show_super_hidden() -> bool:
    return SESSION.read_dword(_ADV, "ShowSuperHidden") == 1


# ── Open Explorer to This PC ────────────────────────────────────────────────


def _apply_open_this_pc(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: open to This PC")
    SESSION.backup([_ADV], "OpenThisPC")
    SESSION.set_dword(_ADV, "LaunchTo", 1)  # 1=This PC, 2=Quick Access


def _remove_open_this_pc(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "OpenThisPC_Remove")
    SESSION.set_dword(_ADV, "LaunchTo", 2)


def _detect_open_this_pc() -> bool:
    return SESSION.read_dword(_ADV, "LaunchTo") == 1


# ── Disable Folder Thumbnails ───────────────────────────────────────────────


def _apply_disable_thumbnails(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable folder thumbnails")
    SESSION.backup([_ADV], "DisableThumbnails")
    SESSION.set_dword(_ADV, "IconsOnly", 1)


def _remove_disable_thumbnails(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "DisableThumbnails_Remove")
    SESSION.set_dword(_ADV, "IconsOnly", 0)


def _detect_disable_thumbnails() -> bool:
    return SESSION.read_dword(_ADV, "IconsOnly") == 1


# ── Show Full Path in Title Bar ──────────────────────────────────────────────


def _apply_full_path_title(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: full path in title bar")
    SESSION.backup([_CABINETSTATE], "FullPathTitle")
    SESSION.set_dword(_CABINETSTATE, "FullPath", 1)


def _remove_full_path_title(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CABINETSTATE], "FullPathTitle_Remove")
    SESSION.set_dword(_CABINETSTATE, "FullPath", 0)


def _detect_full_path_title() -> bool:
    return SESSION.read_dword(_CABINETSTATE, "FullPath") == 1


# ── Disable Recent Files in Quick Access ─────────────────────────────────────


def _apply_disable_recent_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable recent files in Quick Access")
    SESSION.backup([_EXPLORER], "DisableRecentFiles")
    SESSION.set_dword(_EXPLORER, "ShowRecent", 0)
    SESSION.set_dword(_EXPLORER, "ShowFrequent", 0)


def _remove_disable_recent_files(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_EXPLORER], "DisableRecentFiles_Remove")
    SESSION.set_dword(_EXPLORER, "ShowRecent", 1)
    SESSION.set_dword(_EXPLORER, "ShowFrequent", 1)


def _detect_disable_recent_files() -> bool:
    return SESSION.read_dword(_EXPLORER, "ShowRecent") == 0


# ── Recent Folders in Quick Access (original tweak) ──────────────────────────


def _apply_recent_places(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: add Recent Places to Quick Access")
    SESSION.backup(_QA_KEYS, "RecentFolders")
    SESSION.set_string(_QA_KEYS[0], None, "Recent Places")
    SESSION.set_dword(_QA_KEYS[1], "Attributes", 0x30040000)
    SESSION.set_string(_QA_KEYS[2], None, "Recent Places")
    SESSION.set_dword(_QA_KEYS[3], "Attributes", 0x30040000)


def _remove_recent_places(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_QA_KEYS[0], _QA_KEYS[2]], "RecentFolders_Remove")
    for key in [_QA_KEYS[0], _QA_KEYS[2]]:
        SESSION.delete_tree(key)


def _detect_recent_places() -> bool:
    return SESSION.key_exists(_QA_KEYS[0])


# ── Disable Search History ──────────────────────────────────────────────────


def _apply_disable_search_history(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable search history")
    SESSION.backup([_SEARCH_KEY], "SearchHistory")
    SESSION.set_dword(_SEARCH_KEY, "IsDeviceSearchHistoryEnabled", 0)


def _remove_disable_search_history(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SEARCH_KEY], "SearchHistory_Remove")
    SESSION.set_dword(_SEARCH_KEY, "IsDeviceSearchHistoryEnabled", 1)


def _detect_disable_search_history() -> bool:
    return SESSION.read_dword(_SEARCH_KEY, "IsDeviceSearchHistoryEnabled") == 0


# ── Disable Gallery in Navigation Pane (Win11 23H2+) ────────────────────────


def _apply_disable_gallery(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable Gallery in nav pane")
    SESSION.backup([_GALLERY_KEY], "Gallery")
    SESSION.set_dword(_GALLERY_KEY, "System.IsPinnedToNameSpaceTree", 0)


def _remove_disable_gallery(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_GALLERY_KEY], "Gallery_Remove")
    SESSION.delete_value(_GALLERY_KEY, "System.IsPinnedToNameSpaceTree")


def _detect_disable_gallery() -> bool:
    return SESSION.read_dword(_GALLERY_KEY, "System.IsPinnedToNameSpaceTree") == 0


# ── Compact View (reduce spacing) ───────────────────────────────────────────


def _apply_compact_view(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: compact view / reduced spacing")
    SESSION.backup([_ADV], "CompactView")
    SESSION.set_dword(_ADV, "UseCompactMode", 1)


def _remove_compact_view(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "CompactView_Remove")
    SESSION.set_dword(_ADV, "UseCompactMode", 0)


def _detect_compact_view() -> bool:
    return SESSION.read_dword(_ADV, "UseCompactMode") == 1


# ── Disable Automatic Folder Type Discovery ──────────────────────────────────


def _apply_disable_auto_folder_type(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable auto folder type detection")
    SESSION.backup([_BAGS_KEY], "AutoFolderType")
    SESSION.set_string(_BAGS_KEY, "FolderType", "NotSpecified")


def _remove_disable_auto_folder_type(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_BAGS_KEY], "AutoFolderType_Remove")
    SESSION.delete_value(_BAGS_KEY, "FolderType")


def _detect_disable_auto_folder_type() -> bool:
    return SESSION.read_string(_BAGS_KEY, "FolderType") == "NotSpecified"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="show-file-extensions",
        label="Show File Extensions",
        category="Explorer",
        apply_fn=_apply_show_extensions,
        remove_fn=_remove_show_extensions,
        detect_fn=_detect_show_extensions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Always shows file extensions (.txt, .exe, etc.) in Explorer.",
        tags=["explorer", "files", "security"],
    ),
    TweakDef(
        id="show-hidden-files",
        label="Show Hidden Files",
        category="Explorer",
        apply_fn=_apply_show_hidden,
        remove_fn=_remove_show_hidden,
        detect_fn=_detect_show_hidden,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Shows hidden files and folders in Explorer.",
        tags=["explorer", "files"],
    ),
    TweakDef(
        id="show-super-hidden",
        label="Show Protected OS Files",
        category="Explorer",
        apply_fn=_apply_show_super_hidden,
        remove_fn=_remove_show_super_hidden,
        detect_fn=_detect_show_super_hidden,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Shows protected operating system files (super hidden).",
        tags=["explorer", "files", "advanced"],
    ),
    TweakDef(
        id="open-this-pc",
        label="Open Explorer to This PC",
        category="Explorer",
        apply_fn=_apply_open_this_pc,
        remove_fn=_remove_open_this_pc,
        detect_fn=_detect_open_this_pc,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Opens File Explorer to 'This PC' instead of Quick Access.",
        tags=["explorer", "navigation"],
    ),
    TweakDef(
        id="disable-thumbnails",
        label="Disable Folder Thumbnails",
        category="Explorer",
        apply_fn=_apply_disable_thumbnails,
        remove_fn=_remove_disable_thumbnails,
        detect_fn=_detect_disable_thumbnails,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Shows icons instead of thumbnails for faster folder browsing.",
        tags=["explorer", "performance"],
    ),
    TweakDef(
        id="full-path-title",
        label="Full Path in Title Bar",
        category="Explorer",
        apply_fn=_apply_full_path_title,
        remove_fn=_remove_full_path_title,
        detect_fn=_detect_full_path_title,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CABINETSTATE],
        description="Shows the full folder path in the Explorer title bar.",
        tags=["explorer", "navigation"],
    ),
    TweakDef(
        id="disable-recent-files",
        label="Disable Recent Files in Quick Access",
        category="Explorer",
        apply_fn=_apply_disable_recent_files,
        remove_fn=_remove_disable_recent_files,
        detect_fn=_detect_disable_recent_files,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER],
        description="Disables recent and frequent files from appearing in Quick Access.",
        tags=["explorer", "privacy"],
    ),
    TweakDef(
        id="recent-places",
        label="Recent Folders in Quick Access",
        category="Explorer",
        apply_fn=_apply_recent_places,
        remove_fn=_remove_recent_places,
        detect_fn=_detect_recent_places,
        needs_admin=False,
        corp_safe=True,
        registry_keys=_QA_KEYS,
        description="Adds a 'Recent Places' virtual folder to Quick Access.",
        tags=["explorer", "navigation"],
    ),
    TweakDef(
        id="disable-search-history",
        label="Disable Search History",
        category="Explorer",
        apply_fn=_apply_disable_search_history,
        remove_fn=_remove_disable_search_history,
        detect_fn=_detect_disable_search_history,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_KEY],
        description="Prevents Windows from storing device search history.",
        tags=["explorer", "privacy"],
    ),
    TweakDef(
        id="disable-gallery",
        label="Disable Gallery in Nav Pane",
        category="Explorer",
        apply_fn=_apply_disable_gallery,
        remove_fn=_remove_disable_gallery,
        detect_fn=_detect_disable_gallery,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_GALLERY_KEY],
        description="Removes the Gallery entry from Explorer navigation pane (23H2+).",
        tags=["explorer", "win11"],
    ),
    TweakDef(
        id="compact-view",
        label="Enable Compact View",
        category="Explorer",
        apply_fn=_apply_compact_view,
        remove_fn=_remove_compact_view,
        detect_fn=_detect_compact_view,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Reduces item spacing in Explorer for a denser file list.",
        tags=["explorer", "win11"],
    ),
    TweakDef(
        id="disable-auto-folder-type",
        label="Disable Auto Folder Type Detection",
        category="Explorer",
        apply_fn=_apply_disable_auto_folder_type,
        remove_fn=_remove_disable_auto_folder_type,
        detect_fn=_detect_disable_auto_folder_type,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_BAGS_KEY],
        description=(
            "Prevents Explorer from auto-detecting folder content type "
            "(e.g. 'Pictures', 'Music') which causes slow loading."
        ),
        tags=["explorer", "performance"],
    ),
]
