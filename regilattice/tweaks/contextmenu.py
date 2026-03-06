"""Context Menu tweaks.

Covers Windows 11 context menu style, removing unwanted
shell extensions: Share, Cast to Device, Edit with Paint 3D,
Edit with Photos, Give Access To, Include in Library, and more.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_CTX_CLASSIC = (
    r"HKEY_CURRENT_USER\Software\Classes\CLSID"
    r"\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"
)
_SHARE_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Shell Extensions\Blocked"
)
# {E2BF9676-5F8F-435C-97EB-11607A5BEDF7} = Share
_SHARE_CLSID = "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}"

# Cast to Device = {7AD84985-87B4-4a16-BE58-8B72A5B390F7}
_CAST_CLSID = "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}"

# Give Access To = {F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}
_GIVE_ACCESS_CLSID = "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}"

# Include in Library = {3dad6c5d-2167-4cae-9914-f99e41c12cfa}
_INCLUDE_LIB_CLSID = "{3DAD6C5D-2167-4CAE-9914-F99E41C12CFA}"

# Edit with Paint 3D = {D2B7917A-1EAC-4872-B063-0E97D5A82E89}
_PAINT3D_KEY = r"HKEY_CLASSES_ROOT\SystemFileAssociations\.bmp\Shell\3D Edit"
_PAINT3D_JPG = r"HKEY_CLASSES_ROOT\SystemFileAssociations\.jpg\Shell\3D Edit"
_PAINT3D_PNG = r"HKEY_CLASSES_ROOT\SystemFileAssociations\.png\Shell\3D Edit"

# Edit with Photos
_PHOTOS_KEY = (
    r"HKEY_CLASSES_ROOT\AppX43ztkmn2e2q6vhzjqeps9v44v72r52m3"
    r"\Shell\ShellEdit"
)

# Troubleshoot Compatibility
_COMPAT = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Shell Extensions\Blocked"
)
_COMPAT_CLSID = "{1D27F844-3A1F-4410-85AC-14651078412D}"

# Restore Previous Versions
_PREV_VER_DIR = (
    r"HKEY_CLASSES_ROOT\AllFilesystemObjects\shellex"
    r"\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}"
)
_PREV_VER = (
    r"HKEY_CLASSES_ROOT\CLSID\{450D8FBA-AD25-11D0-98A8-0800361B1103}"
    r"\shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}"
)

# Pin to Quick Access / Pin to Start
_PIN_START = r"HKEY_CLASSES_ROOT\Folder\shellex\ContextMenuHandlers\PintoStartScreen"

# Open With
_OPEN_WITH = r"HKEY_CLASSES_ROOT\*\shellex\ContextMenuHandlers\OpenWith"


# ── Restore Classic Context Menu (Win11) ─────────────────────────────────────


def _apply_classic_ctx(*, require_admin: bool = False) -> None:
    SESSION.log("ContextMenu: restore Windows 10 classic context menu")
    SESSION.backup([_CTX_CLASSIC], "ClassicContext")
    # Creating key with empty default value triggers classic menu
    SESSION.set_string(_CTX_CLASSIC, "", "")


def _remove_classic_ctx(*, require_admin: bool = False) -> None:
    SESSION.delete_tree(_CTX_CLASSIC)


def _detect_classic_ctx() -> bool:
    return SESSION.key_exists(_CTX_CLASSIC)


# ── Remove "Share" Context Menu ──────────────────────────────────────────────


def _apply_remove_share(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Share' from context menu")
    SESSION.backup([_SHARE_KEY], "CtxShare")
    SESSION.set_string(_SHARE_KEY, _SHARE_CLSID, "")


def _remove_remove_share(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SHARE_KEY, _SHARE_CLSID)


def _detect_remove_share() -> bool:
    return SESSION.read_string(_SHARE_KEY, _SHARE_CLSID) is not None


# ── Remove "Cast to Device" Context Menu ─────────────────────────────────────


def _apply_remove_cast(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Cast to Device' from context menu")
    SESSION.backup([_SHARE_KEY], "CtxCast")
    SESSION.set_string(_SHARE_KEY, _CAST_CLSID, "")


def _remove_remove_cast(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SHARE_KEY, _CAST_CLSID)


def _detect_remove_cast() -> bool:
    return SESSION.read_string(_SHARE_KEY, _CAST_CLSID) is not None


# ── Remove "Give Access To" Context Menu ─────────────────────────────────────


def _apply_remove_give_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Give access to' from context menu")
    SESSION.backup([_SHARE_KEY], "CtxGiveAccess")
    SESSION.set_string(_SHARE_KEY, _GIVE_ACCESS_CLSID, "")


def _remove_remove_give_access(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SHARE_KEY, _GIVE_ACCESS_CLSID)


def _detect_remove_give_access() -> bool:
    return SESSION.read_string(_SHARE_KEY, _GIVE_ACCESS_CLSID) is not None


# ── Remove "Include in Library" Context Menu ─────────────────────────────────


def _apply_remove_include_lib(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Include in library' from context menu")
    SESSION.backup([_SHARE_KEY], "CtxIncludeLib")
    SESSION.set_string(_SHARE_KEY, _INCLUDE_LIB_CLSID, "")


def _remove_remove_include_lib(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SHARE_KEY, _INCLUDE_LIB_CLSID)


def _detect_remove_include_lib() -> bool:
    return SESSION.read_string(_SHARE_KEY, _INCLUDE_LIB_CLSID) is not None


# ── Remove "Edit with Paint 3D" Context Menu ────────────────────────────────


def _apply_remove_paint3d(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Edit with Paint 3D' from images")
    keys = [_PAINT3D_KEY, _PAINT3D_JPG, _PAINT3D_PNG]
    SESSION.backup(keys, "CtxPaint3D")
    for key in keys:
        SESSION.set_string(key, "ProgrammaticAccessOnly", "")


def _remove_remove_paint3d(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    for key in [_PAINT3D_KEY, _PAINT3D_JPG, _PAINT3D_PNG]:
        SESSION.delete_value(key, "ProgrammaticAccessOnly")


def _detect_remove_paint3d() -> bool:
    return SESSION.read_string(_PAINT3D_KEY, "ProgrammaticAccessOnly") is not None


# ── Remove "Edit with Photos" Context Menu ───────────────────────────────────


def _apply_remove_photos_edit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Edit with Photos' from context menu")
    SESSION.backup([_PHOTOS_KEY], "CtxPhotos")
    SESSION.set_string(_PHOTOS_KEY, "ProgrammaticAccessOnly", "")


def _remove_remove_photos_edit(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PHOTOS_KEY, "ProgrammaticAccessOnly")


def _detect_remove_photos_edit() -> bool:
    return SESSION.read_string(_PHOTOS_KEY, "ProgrammaticAccessOnly") is not None


# ── Remove "Troubleshoot Compatibility" Context Menu ─────────────────────────


def _apply_remove_troubleshoot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Troubleshoot compatibility' from context menu")
    SESSION.backup([_COMPAT], "CtxCompat")
    SESSION.set_string(_COMPAT, _COMPAT_CLSID, "")


def _remove_remove_troubleshoot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COMPAT, _COMPAT_CLSID)


def _detect_remove_troubleshoot() -> bool:
    return SESSION.read_string(_COMPAT, _COMPAT_CLSID) is not None


# ── Remove "Restore Previous Versions" Context Menu ──────────────────────────


def _apply_remove_prev_versions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Restore previous versions' from context menu")
    SESSION.backup([_PREV_VER_DIR, _PREV_VER], "CtxPrevVer")
    SESSION.delete_tree(_PREV_VER_DIR)
    SESSION.delete_tree(_PREV_VER)


def _remove_remove_prev_versions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    # Re-create the keys to restore the handler
    SESSION.set_string(_PREV_VER_DIR, "", "{596AB062-B4D2-4215-9F74-E9109B0A8153}")
    SESSION.set_string(_PREV_VER, "", "{596AB062-B4D2-4215-9F74-E9109B0A8153}")


def _detect_remove_prev_versions() -> bool:
    return not SESSION.key_exists(_PREV_VER_DIR)


# ── Remove "Pin to Start" from Folder Context Menu ──────────────────────────


def _apply_remove_pin_start(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("ContextMenu: remove 'Pin to Start' from folder context menu")
    SESSION.backup([_PIN_START], "CtxPinStart")
    SESSION.delete_tree(_PIN_START)


def _remove_remove_pin_start(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_PIN_START, "", "{470C0EBD-5D73-4d58-9CED-E91E22E23282}")


def _detect_remove_pin_start() -> bool:
    return not SESSION.key_exists(_PIN_START)


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="ctx-classic-context-menu",
        label="Restore Classic Context Menu (Win11)",
        category="Context Menu",
        apply_fn=_apply_classic_ctx,
        remove_fn=_remove_classic_ctx,
        detect_fn=_detect_classic_ctx,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CTX_CLASSIC],
        description=(
            "Restores the full Windows 10 right-click context menu "
            "in Windows 11. Removes the truncated 'Show more options' menu. "
            "Default: Win11 menu. Recommended: classic."
        ),
        tags=["context-menu", "win11", "classic", "right-click"],
    ),
    TweakDef(
        id="ctx-remove-share",
        label="Remove 'Share' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_share,
        remove_fn=_remove_remove_share,
        detect_fn=_detect_remove_share,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SHARE_KEY],
        description=("Blocks the Share shell extension from appearing in the context menu. Default: shown. Recommended: hidden."),
        tags=["context-menu", "share", "shell-extension", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-cast-to-device",
        label="Remove 'Cast to Device' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_cast,
        remove_fn=_remove_remove_cast,
        detect_fn=_detect_remove_cast,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SHARE_KEY],
        description=("Removes 'Cast to Device' (Play To) from the right-click menu. Default: shown. Recommended: hidden."),
        tags=["context-menu", "cast", "miracast", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-give-access",
        label="Remove 'Give Access To' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_give_access,
        remove_fn=_remove_remove_give_access,
        detect_fn=_detect_remove_give_access,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SHARE_KEY],
        description=("Removes 'Give access to' (Share with) from the context menu. Default: shown. Recommended: hidden."),
        tags=["context-menu", "give-access", "share", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-include-in-library",
        label="Remove 'Include in Library' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_include_lib,
        remove_fn=_remove_remove_include_lib,
        detect_fn=_detect_remove_include_lib,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SHARE_KEY],
        description=("Removes 'Include in library' from the folder context menu. Default: shown. Recommended: hidden."),
        tags=["context-menu", "library", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-paint3d-edit",
        label="Remove 'Edit with Paint 3D' from Images",
        category="Context Menu",
        apply_fn=_apply_remove_paint3d,
        remove_fn=_remove_remove_paint3d,
        detect_fn=_detect_remove_paint3d,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PAINT3D_KEY, _PAINT3D_JPG, _PAINT3D_PNG],
        description=(
            "Removes 'Edit with Paint 3D' from .bmp, .jpg, .png context menus. Uses ProgrammaticAccessOnly flag. Default: shown. Recommended: hidden."
        ),
        tags=["context-menu", "paint3d", "images", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-photos-edit",
        label="Remove 'Edit with Photos' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_photos_edit,
        remove_fn=_remove_remove_photos_edit,
        detect_fn=_detect_remove_photos_edit,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PHOTOS_KEY],
        description=("Removes the 'Edit' option added by the Photos app from image context menus. Default: shown. Recommended: hidden."),
        tags=["context-menu", "photos", "edit", "images", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-troubleshoot-compat",
        label="Remove 'Troubleshoot Compatibility'",
        category="Context Menu",
        apply_fn=_apply_remove_troubleshoot,
        remove_fn=_remove_remove_troubleshoot,
        detect_fn=_detect_remove_troubleshoot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_COMPAT],
        description=("Removes the 'Troubleshoot compatibility' option from executable context menus. Default: shown. Recommended: hidden."),
        tags=["context-menu", "compatibility", "troubleshoot", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-previous-versions",
        label="Remove 'Restore Previous Versions'",
        category="Context Menu",
        apply_fn=_apply_remove_prev_versions,
        remove_fn=_remove_remove_prev_versions,
        detect_fn=_detect_remove_prev_versions,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PREV_VER_DIR, _PREV_VER],
        description=("Removes 'Restore previous versions' from file/folder context menus. Default: shown. Recommended: hidden."),
        tags=["context-menu", "previous-versions", "shadow-copy", "cleanup"],
    ),
    TweakDef(
        id="ctx-remove-pin-to-start",
        label="Remove 'Pin to Start' from Folders",
        category="Context Menu",
        apply_fn=_apply_remove_pin_start,
        remove_fn=_remove_remove_pin_start,
        detect_fn=_detect_remove_pin_start,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PIN_START],
        description=("Removes 'Pin to Start' from the folder right-click menu. Default: shown. Recommended: hidden."),
        tags=["context-menu", "pin", "start", "folder", "cleanup"],
    ),
]


_TAKE_OWNERSHIP = r"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership"
_TAKE_OWNERSHIP_CMD = r"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership\command"


# -- Add Take Ownership to Context Menu -------------------------------------------


def _apply_take_ownership(*, require_admin: bool = True) -> None:
    SESSION.log("Context Menu: add Take Ownership entry")
    SESSION.backup([_TAKE_OWNERSHIP], "TakeOwnership")
    SESSION.set_string(_TAKE_OWNERSHIP, "", "Take Ownership")
    SESSION.set_string(_TAKE_OWNERSHIP, "HasLUAShield", "")
    SESSION.set_string(_TAKE_OWNERSHIP, "NoWorkingDirectory", "")
    SESSION.set_string(
        _TAKE_OWNERSHIP_CMD,
        "",
        'cmd.exe /c takeown /f "%1" && icacls "%1" /grant administrators:F',
    )


def _remove_take_ownership(*, require_admin: bool = True) -> None:
    SESSION.delete_tree(_TAKE_OWNERSHIP)


def _detect_take_ownership() -> bool:
    return SESSION.key_exists(_TAKE_OWNERSHIP)


# -- Remove Include in Library from Context Menu ----------------------------------


def _apply_remove_include_library(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Context Menu: remove Include in Library via shell extension block")
    SESSION.backup([_SHARE_KEY], "IncludeInLibrary")
    SESSION.set_string(_SHARE_KEY, _INCLUDE_LIB_CLSID, "")


def _remove_remove_include_library(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SHARE_KEY, _INCLUDE_LIB_CLSID)


def _detect_remove_include_library() -> bool:
    return SESSION.read_string(_SHARE_KEY, _INCLUDE_LIB_CLSID) is not None


TWEAKS += [
    TweakDef(
        id="ctx-add-take-ownership",
        label="Add 'Take Ownership' to Context Menu",
        category="Context Menu",
        apply_fn=_apply_take_ownership,
        remove_fn=_remove_take_ownership,
        detect_fn=_detect_take_ownership,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TAKE_OWNERSHIP, _TAKE_OWNERSHIP_CMD],
        description=(
            "Adds a 'Take Ownership' option to the right-click context menu. "
            "Runs takeown and icacls to grant full control. "
            "Default: not present. Recommended: add for power users."
        ),
        tags=["context-menu", "ownership", "takeown", "permissions"],
    ),
    TweakDef(
        id="ctx-remove-include-library",
        label="Remove 'Include in Library' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_include_library,
        remove_fn=_remove_remove_include_library,
        detect_fn=_detect_remove_include_library,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SHARE_KEY],
        description=(
            "Removes the 'Include in Library' option from the folder context menu "
            "by blocking its shell extension CLSID. "
            "Default: shown. Recommended: hidden if unused."
        ),
        tags=["context-menu", "library", "include", "cleanup"],
    ),
]


# ── Remove "Send to" from Context Menu ───────────────────────────────────────

_SEND_TO_CLSID = "{7BA4C740-9E81-11CF-99D3-00AA004AE837}"


def _apply_remove_send_to(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Context Menu: remove Send to via shell extension block")
    SESSION.backup([_SHARE_KEY], "RemoveSendTo")
    SESSION.set_string(_SHARE_KEY, _SEND_TO_CLSID, "")


def _remove_remove_send_to(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SHARE_KEY, _SEND_TO_CLSID)


def _detect_remove_send_to() -> bool:
    return SESSION.read_string(_SHARE_KEY, _SEND_TO_CLSID) is not None


# ── Remove "Print" from Context Menu ─────────────────────────────────────────

_PRINT_CLSID = "{09799AFB-AD67-11d1-ABCD-00C04FC30936}"


def _apply_remove_print(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Context Menu: remove Print from shell extension")
    SESSION.backup([_SHARE_KEY], "RemovePrint")
    SESSION.set_string(_SHARE_KEY, _PRINT_CLSID, "")


def _remove_remove_print(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SHARE_KEY, _PRINT_CLSID)


def _detect_remove_print() -> bool:
    return SESSION.read_string(_SHARE_KEY, _PRINT_CLSID) is not None


# ── Add "Open PowerShell Here" to Context Menu ───────────────────────────────

_PS_HERE = r"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere"
_PS_HERE_CMD = r"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere\command"


def _apply_powershell_here(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Context Menu: add Open PowerShell Here")
    SESSION.backup([_PS_HERE], "PowerShellHere")
    SESSION.set_string(_PS_HERE, "", "Open PowerShell Here")
    SESSION.set_string(_PS_HERE, "Icon", "powershell.exe")
    SESSION.set_string(
        _PS_HERE_CMD,
        "",
        "powershell.exe -NoExit -Command Set-Location -LiteralPath '%V'",
    )


def _remove_powershell_here(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_tree(_PS_HERE)


def _detect_powershell_here() -> bool:
    return SESSION.key_exists(_PS_HERE)


TWEAKS += [
    TweakDef(
        id="ctx-remove-send-to",
        label="Remove 'Send to' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_send_to,
        remove_fn=_remove_remove_send_to,
        detect_fn=_detect_remove_send_to,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SHARE_KEY],
        description=(
            "Removes the 'Send to' cascading menu from the right-click "
            "context menu by blocking its shell extension CLSID. "
            "Default: shown. Recommended: hidden if unused."
        ),
        tags=["context-menu", "send-to", "cleanup", "shell"],
    ),
    TweakDef(
        id="ctx-remove-print",
        label="Remove 'Print' from Context Menu",
        category="Context Menu",
        apply_fn=_apply_remove_print,
        remove_fn=_remove_remove_print,
        detect_fn=_detect_remove_print,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_SHARE_KEY],
        description=(
            "Removes the 'Print' option from the right-click context menu "
            "by blocking its shell extension CLSID. "
            "Default: shown. Recommended: hidden if no printer is used."
        ),
        tags=["context-menu", "print", "cleanup", "shell"],
    ),
    TweakDef(
        id="ctx-add-powershell-here",
        label="Add 'Open PowerShell Here' to Context Menu",
        category="Context Menu",
        apply_fn=_apply_powershell_here,
        remove_fn=_remove_powershell_here,
        detect_fn=_detect_powershell_here,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PS_HERE, _PS_HERE_CMD],
        description=(
            "Adds an 'Open PowerShell Here' entry to the directory background "
            "context menu for quick terminal access. "
            "Default: not present. Recommended: add for power users."
        ),
        tags=["context-menu", "powershell", "terminal", "productivity"],
    ),
]
