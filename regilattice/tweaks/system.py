"""System tweaks — Long Paths, Reserved Storage, Remote Assistance."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Long Paths ─────────────────────────────────────────────────────────────

_LONGPATH_KEY = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem"
_RESERVED_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\ReserveManager"
)
_REMOTE_KEY = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\Remote Assistance"
)


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


# ── Disable Reserved Storage ─────────────────────────────────────────────


def _apply_disable_reserved(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable reserved storage (frees ~7 GB)")
    SESSION.backup([_RESERVED_KEY], "ReservedStorage")
    SESSION.set_dword(_RESERVED_KEY, "ShippedWithReserves", 0)


def _remove_disable_reserved(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_RESERVED_KEY, "ShippedWithReserves", 1)


def _detect_disable_reserved() -> bool:
    return SESSION.read_dword(_RESERVED_KEY, "ShippedWithReserves") == 0


# ── Disable Remote Assistance ────────────────────────────────────────────


def _apply_disable_remote_assist(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("System: disable Remote Assistance")
    SESSION.backup([_REMOTE_KEY], "RemoteAssist")
    SESSION.set_dword(_REMOTE_KEY, "fAllowToGetHelp", 0)


def _remove_disable_remote_assist(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_REMOTE_KEY, "fAllowToGetHelp", 1)


def _detect_disable_remote_assist() -> bool:
    return SESSION.read_dword(_REMOTE_KEY, "fAllowToGetHelp") == 0


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
        tags=["system", "filesystem", "long-paths"],
    ),
    TweakDef(
        id="disable-reserved-storage",
        label="Disable Reserved Storage (~7 GB)",
        category="System",
        apply_fn=_apply_disable_reserved,
        remove_fn=_remove_disable_reserved,
        detect_fn=_detect_disable_reserved,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RESERVED_KEY],
        description="Disables Windows Reserved Storage which holds ~7 GB for updates.",
        tags=["system", "disk", "storage", "cleanup"],
    ),
    TweakDef(
        id="disable-remote-assistance",
        label="Disable Remote Assistance",
        category="System",
        apply_fn=_apply_disable_remote_assist,
        remove_fn=_remove_disable_remote_assist,
        detect_fn=_detect_disable_remote_assist,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_REMOTE_KEY],
        description="Disables Remote Assistance to reduce attack surface.",
        tags=["system", "security", "remote"],
    ),
]
