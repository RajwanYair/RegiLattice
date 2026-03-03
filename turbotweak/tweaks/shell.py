"""Shell tweaks — Take Ownership context menu."""

from __future__ import annotations

import subprocess
from typing import List

from turbotweak.registry import SESSION, assert_admin
from turbotweak.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_KEYS = [
    r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership\command",
    r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership\command",
    r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership",
    r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership\command",
]

_CMD_FILE = (
    'cmd.exe /k takeown /f "%1" && icacls "%1"' " /grant *S-1-3-4:F /t /c /l && pause"
)
_CMD_DIR = (
    'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1"'
    " /grant *S-1-3-4:F /t /c /q && pause"
)
_CMD_DRIVE = (
    'cmd.exe /k takeown /f "%1" /r /d y && icacls "%1"'
    " /grant *S-1-3-4:F /t /c && pause"
)


def _add_context_entry(base: str, command: str) -> None:
    _run = lambda a: subprocess.run(  # noqa: E731
        ["reg", *a], check=True, capture_output=True, text=True
    )
    _run(["add", base, "/f"])
    _run(["add", base, "/ve", "/d", "Take Ownership", "/f"])
    _run(["add", base, "/v", "NoWorkingDirectory", "/d", "", "/f"])
    _run(["add", base, "/v", "Extended", "/d", "", "/f"])
    _run(["add", f"{base}\\command", "/f"])
    _run(["add", f"{base}\\command", "/ve", "/d", command, "/f"])


# ── Functions ────────────────────────────────────────────────────────────────


def apply_take_ownership(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-TakeOwnership")
    SESSION.backup(_KEYS, "TakeOwnership")
    _add_context_entry(r"HKEY_CLASSES_ROOT\*\shell\TakeOwnership", _CMD_FILE)
    _add_context_entry(r"HKEY_CLASSES_ROOT\Directory\shell\TakeOwnership", _CMD_DIR)
    _add_context_entry(r"HKEY_CLASSES_ROOT\Drive\shell\TakeOwnership", _CMD_DRIVE)
    SESSION.log("Completed Add-TakeOwnership")


def remove_take_ownership(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-TakeOwnership")
    top_keys = [k for k in _KEYS if not k.endswith("\\command")]
    SESSION.backup(top_keys, "TakeOwnership_Remove")
    for key in top_keys:
        subprocess.run(
            ["reg", "delete", key, "/f"],
            check=False,
            capture_output=True,
        )
    SESSION.log("Completed Remove-TakeOwnership")


def detect_take_ownership() -> bool:
    return SESSION.key_exists(_KEYS[0])


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="take-ownership",
        label="Take Ownership Context Menu",
        category="Shell",
        apply_fn=apply_take_ownership,
        remove_fn=remove_take_ownership,
        detect_fn=detect_take_ownership,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_KEYS,
    ),
]
