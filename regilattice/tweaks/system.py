"""System tweaks — Long Paths, Power Plan, etc."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Long Paths ───────────────────────────────────────────────────────────────

_LONGPATH_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"


def apply_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-LongPaths")
    SESSION.backup([_LONGPATH_KEY], "LongPaths")
    SESSION.set_dword(_LONGPATH_KEY, "LongPathsEnabled", 1)
    SESSION.log("Completed Add-LongPaths")


def remove_long_paths(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-LongPaths")
    SESSION.backup([_LONGPATH_KEY], "LongPaths_Remove")
    SESSION.set_dword(_LONGPATH_KEY, "LongPathsEnabled", 0)
    SESSION.log("Completed Remove-LongPaths")


def detect_long_paths() -> bool:
    return SESSION.read_dword(_LONGPATH_KEY, "LongPathsEnabled") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="enable-long-paths",
        label="Enable Win32 Long Paths",
        category="System",
        apply_fn=apply_long_paths,
        remove_fn=remove_long_paths,
        detect_fn=detect_long_paths,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_LONGPATH_KEY],
        description="Allows Win32 applications to use paths longer than 260 chars.",
    ),
]
