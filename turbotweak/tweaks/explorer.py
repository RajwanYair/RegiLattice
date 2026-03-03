"""Explorer tweaks — Recent Folders in Quick Access."""

from __future__ import annotations

from typing import List

from turbotweak.registry import SESSION, assert_admin
from turbotweak.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_CLSID = "{22877a6d-37a1-461a-91b0-dbda5aaebc99}"
_KEYS = [
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{_CLSID}",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\CLSID\{_CLSID}\ShellFolder",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\Wow6432Node\CLSID\{_CLSID}",
    rf"HKEY_CURRENT_USER\SOFTWARE\Classes\Wow6432Node\CLSID\{_CLSID}\ShellFolder",
]


# ── Functions ────────────────────────────────────────────────────────────────


def apply_recent_places(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-RecentFolders")
    SESSION.backup(_KEYS, "RecentFolders")
    SESSION.set_string(_KEYS[0], None, "Recent Places")
    SESSION.set_dword(_KEYS[1], "Attributes", 0x30040000)
    SESSION.set_string(_KEYS[2], None, "Recent Places")
    SESSION.set_dword(_KEYS[3], "Attributes", 0x30040000)
    SESSION.log("Completed Add-RecentFolders")


def remove_recent_places(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-RecentFolders")
    parent_keys = [_KEYS[0], _KEYS[2]]
    SESSION.backup(parent_keys, "RecentFolders_Remove")
    for key in parent_keys:
        SESSION.delete_tree(key)
    SESSION.log("Completed Remove-RecentFolders")


def detect_recent_places() -> bool:
    return SESSION.key_exists(_KEYS[0])


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="recent-places",
        label="Recent Folders in Quick Access",
        category="Explorer",
        apply_fn=apply_recent_places,
        remove_fn=remove_recent_places,
        detect_fn=detect_recent_places,
        needs_admin=False,
        corp_safe=True,
        registry_keys=_KEYS,
    ),
]
