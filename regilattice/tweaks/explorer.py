"""Windows Explorer tweaks — file display, navigation, and UX enhancements.

Covers: file extensions, hidden files, Quick Access, compact view,
folder thumbnails, gallery pane, and more.
"""

from __future__ import annotations

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


# ── Disable Breadcrumb Bar ────────────────────────────────────────────────────

_RIBBON_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Ribbon"
)


def _apply_disable_breadcrumbs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable breadcrumb bar (show full path)")
    SESSION.backup([_CABINETSTATE], "Breadcrumbs")
    SESSION.set_dword(_CABINETSTATE, "FullPath", 1)


def _remove_disable_breadcrumbs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CABINETSTATE], "Breadcrumbs_Remove")
    SESSION.delete_value(_CABINETSTATE, "FullPath")


def _detect_disable_breadcrumbs() -> bool:
    return SESSION.read_dword(_CABINETSTATE, "FullPath") == 1


# ── Disable Folder Merge Conflicts ──────────────────────────────────────────


def _apply_disable_merge_conflicts(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable folder merge conflict prompts")
    SESSION.backup([_ADV], "MergeConflicts")
    SESSION.set_dword(_ADV, "HideMergeConflicts", 1)


def _remove_disable_merge_conflicts(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "MergeConflicts_Remove")
    SESSION.set_dword(_ADV, "HideMergeConflicts", 0)


def _detect_disable_merge_conflicts() -> bool:
    return SESSION.read_dword(_ADV, "HideMergeConflicts") == 1


# ── Thumbnail Cache Size Increase ────────────────────────────────────────────────────────────

_THUMB_CACHE = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache"
)
_THUMB_QUALITY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"


def _apply_thumbnail_performance(*, require_admin: bool = False) -> None:
    """Optimize thumbnail display: disable cache cleanup + increase quality."""
    assert_admin(require_admin)
    SESSION.log("Explorer: optimize thumbnail caching and quality")
    SESSION.backup([_THUMB_QUALITY, _ADV], "ThumbnailPerf")
    # Disable thumbnail cache auto-cleanup via Disk Cleanup
    SESSION.set_dword(_ADV, "DisableThumbnailCache", 0)       # Keep thumbs.db
    SESSION.set_dword(_ADV, "DisableThumbsDBOnNetworkFolders", 1)  # No thumbs.db on network
    # Enable high-quality thumbnails
    SESSION.set_dword(_THUMB_QUALITY, "ThumbnailSize", 256)    # Larger thumbnails
    SESSION.set_dword(_THUMB_QUALITY, "ThumbnailQuality", 100)  # Max quality


def _remove_thumbnail_performance(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "DisableThumbnailCache")
    SESSION.set_dword(_ADV, "DisableThumbsDBOnNetworkFolders", 0)
    SESSION.delete_value(_THUMB_QUALITY, "ThumbnailSize")
    SESSION.delete_value(_THUMB_QUALITY, "ThumbnailQuality")


def _detect_thumbnail_performance() -> bool:
    return SESSION.read_dword(_THUMB_QUALITY, "ThumbnailSize") == 256


# ── Enable Explorer Status Bar ───────────────────────────────────────────────────────────────


def _apply_status_bar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: enable status bar at bottom")
    SESSION.backup([_ADV], "StatusBar")
    SESSION.set_dword(_ADV, "ShowStatusBar", 1)


def _remove_status_bar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ADV, "ShowStatusBar", 0)


def _detect_status_bar() -> bool:
    return SESSION.read_dword(_ADV, "ShowStatusBar") == 1


# ── Open PowerShell at Selected Folder ───────────────────────────────────────

_PS_FOLDER_KEY = r"HKEY_CLASSES_ROOT\Directory\shell\OpenPowerShellHere"
_PS_FOLDER_CMD = rf"{_PS_FOLDER_KEY}\command"

_PS_BG_KEY = r"HKEY_CLASSES_ROOT\Directory\Background\shell\OpenPowerShellHere"
_PS_BG_CMD = rf"{_PS_BG_KEY}\command"


def _apply_explorer_ps_here(*, require_admin: bool = True) -> None:
    import subprocess

    assert_admin(require_admin)
    SESSION.log("Explorer: add 'Open PowerShell Here' context menu")
    SESSION.backup([_PS_FOLDER_KEY, _PS_BG_KEY], "ExplorerPsHere")

    def _run(args: list[str]) -> None:
        subprocess.run(["reg", *args], check=True, capture_output=True, text=True)

    # Right-click on a folder
    _run(["add", _PS_FOLDER_KEY, "/ve", "/d", "Open PowerShell Here", "/f"])
    _run(["add", _PS_FOLDER_KEY, "/v", "Icon", "/d", "powershell.exe", "/f"])
    _run(["add", _PS_FOLDER_CMD, "/ve", "/d",
          'powershell.exe -NoExit -Command "Set-Location \'%V\'"', "/f"])
    # Right-click on folder background
    _run(["add", _PS_BG_KEY, "/ve", "/d", "Open PowerShell Here", "/f"])
    _run(["add", _PS_BG_KEY, "/v", "Icon", "/d", "powershell.exe", "/f"])
    _run(["add", _PS_BG_CMD, "/ve", "/d",
          'powershell.exe -NoExit -Command "Set-Location \'%V\'"', "/f"])


def _remove_explorer_ps_here(*, require_admin: bool = True) -> None:
    import subprocess

    assert_admin(require_admin)
    for key in (_PS_FOLDER_KEY, _PS_BG_KEY):
        subprocess.run(
            ["reg", "delete", key, "/f"], check=False, capture_output=True,
        )


def _detect_explorer_ps_here() -> bool:
    return SESSION.key_exists(_PS_FOLDER_KEY) and SESSION.key_exists(_PS_BG_KEY)


# ── Disable Recent Documents History ─────────────────────────────────────────

_EXPLORER_POLICIES = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Policies\Explorer"
)


def _apply_disable_recent_docs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable recent documents tracking")
    SESSION.backup([_EXPLORER_POLICIES], "RecentDocs")
    SESSION.set_dword(_EXPLORER_POLICIES, "NoRecentDocsHistory", 1)


def _remove_disable_recent_docs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXPLORER_POLICIES, "NoRecentDocsHistory")


def _detect_disable_recent_docs() -> bool:
    return SESSION.read_dword(_EXPLORER_POLICIES, "NoRecentDocsHistory") == 1


# ── Disable Thumbnail Cache ─────────────────────────────────────────────────


def _apply_disable_thumbnail_cache(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Explorer: disable thumbnail cache files")
    SESSION.backup([_ADV], "ThumbnailCache")
    SESSION.set_dword(_ADV, "DisableThumbnailCache", 1)


def _remove_disable_thumbnail_cache(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ADV, "DisableThumbnailCache", 0)


def _detect_disable_thumbnail_cache() -> bool:
    return SESSION.read_dword(_ADV, "DisableThumbnailCache") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
    TweakDef(
        id="disable-breadcrumbs",
        label="Disable Breadcrumb Bar",
        category="Explorer",
        apply_fn=_apply_disable_breadcrumbs,
        remove_fn=_remove_disable_breadcrumbs,
        detect_fn=_detect_disable_breadcrumbs,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CABINETSTATE],
        description="Shows the full path in Explorer address bar instead of breadcrumbs.",
        tags=["explorer", "navigation"],
    ),
    TweakDef(
        id="disable-merge-conflicts",
        label="Disable Folder Merge Conflicts",
        category="Explorer",
        apply_fn=_apply_disable_merge_conflicts,
        remove_fn=_remove_disable_merge_conflicts,
        detect_fn=_detect_disable_merge_conflicts,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description="Hides folder merge conflict prompts when copying/moving folders.",
        tags=["explorer", "ux"],
    ),
    TweakDef(
        id="thumbnail-performance",
        label="Optimize Thumbnail Caching & Quality",
        category="Explorer",
        apply_fn=_apply_thumbnail_performance,
        remove_fn=_remove_thumbnail_performance,
        detect_fn=_detect_thumbnail_performance,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_THUMB_QUALITY, _ADV],
        description=(
            "Optimizes Explorer thumbnail display: keeps thumbnail cache, "
            "increases size to 256px and quality to 100%, disables thumbs.db "
            "on network folders. Results in sharper, faster file previews. "
            "Default: 96px low quality. Recommended: 256px max quality."
        ),
        tags=["explorer", "thumbnails", "performance", "quality"],
    ),
    TweakDef(
        id="show-status-bar",
        label="Show Explorer Status Bar",
        category="Explorer",
        apply_fn=_apply_status_bar,
        remove_fn=_remove_status_bar,
        detect_fn=_detect_status_bar,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Enables the status bar at the bottom of Explorer windows showing "
            "selected item count, size, and free space. "
            "Default: Hidden. Recommended: Shown."
        ),
        tags=["explorer", "ux", "status-bar"],
    ),
    TweakDef(
        id="explorer-ps-here",
        label="'Open PowerShell Here' in Explorer",
        category="Explorer",
        apply_fn=_apply_explorer_ps_here,
        remove_fn=_remove_explorer_ps_here,
        detect_fn=_detect_explorer_ps_here,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PS_FOLDER_KEY, _PS_BG_KEY],
        description=(
            "Adds 'Open PowerShell Here' to the context menu when "
            "right-clicking a folder or the folder background in Explorer."
        ),
        tags=["explorer", "powershell", "context-menu", "terminal"],
    ),
    TweakDef(
        id="explorer-disable-recent-docs",
        label="Disable Recent Documents History",
        category="Explorer",
        apply_fn=_apply_disable_recent_docs,
        remove_fn=_remove_disable_recent_docs,
        detect_fn=_detect_disable_recent_docs,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER_POLICIES],
        description=(
            "Disables recent documents tracking in the Start menu and "
            "File Explorer. Improves privacy by not recording file access. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["explorer", "recent", "privacy", "history"],
    ),
    TweakDef(
        id="explorer-disable-thumbnail-cache",
        label="Disable Thumbnail Cache",
        category="Explorer",
        apply_fn=_apply_disable_thumbnail_cache,
        remove_fn=_remove_disable_thumbnail_cache,
        detect_fn=_detect_disable_thumbnail_cache,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables thumbnail cache (thumbs.db) creation in folders. "
            "Reduces disk writes and avoids locked files on network shares. "
            "Default: Enabled. Recommended: Disabled on SSDs/network drives."
        ),
        tags=["explorer", "thumbnails", "cache", "performance"],
    ),
]


# ── Explorer Open to This PC ─────────────────────────────────────────────────


def _apply_launch_this_pc(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Set Explorer to open to This PC")
    SESSION.backup([_ADV], "LaunchToThisPC")
    SESSION.set_dword(_ADV, "LaunchTo", 1)


def _remove_launch_this_pc(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ADV, "LaunchTo", 2)


def _detect_launch_this_pc() -> bool:
    return SESSION.read_dword(_ADV, "LaunchTo") == 1


# ── Disable Quick Access Recent Files ────────────────────────────────────────


def _apply_disable_quick_access_recent(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Quick Access recent and frequent files")
    SESSION.backup([_EXPLORER], "QuickAccessRecent")
    SESSION.set_dword(_EXPLORER, "ShowRecent", 0)
    SESSION.set_dword(_EXPLORER, "ShowFrequent", 0)


def _remove_disable_quick_access_recent(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_EXPLORER, "ShowRecent", 1)
    SESSION.set_dword(_EXPLORER, "ShowFrequent", 1)


def _detect_disable_quick_access_recent() -> bool:
    v1 = SESSION.read_dword(_EXPLORER, "ShowRecent") == 0
    v2 = SESSION.read_dword(_EXPLORER, "ShowFrequent") == 0
    return v1 and v2


TWEAKS += [
    TweakDef(
        id="explorer-launch-to-this-pc",
        label="Open Explorer to This PC",
        category="Explorer",
        apply_fn=_apply_launch_this_pc,
        remove_fn=_remove_launch_this_pc,
        detect_fn=_detect_launch_this_pc,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Sets File Explorer to open to This PC instead of Quick "
            "Access or Home. Provides direct access to drives. "
            "Default: Quick Access. Recommended: This PC."
        ),
        tags=["explorer", "this-pc", "launch", "navigation"],
    ),
    TweakDef(
        id="explorer-disable-quick-access",
        label="Disable Quick Access Recent Files",
        category="Explorer",
        apply_fn=_apply_disable_quick_access_recent,
        remove_fn=_remove_disable_quick_access_recent,
        detect_fn=_detect_disable_quick_access_recent,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_EXPLORER],
        description=(
            "Disables Quick Access recent and frequent files display. "
            "Improves privacy and reduces Explorer clutter. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["explorer", "quick-access", "recent", "privacy"],
    ),
]
