"""Shell tweaks — Take Ownership context menu."""

from __future__ import annotations

import subprocess
from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

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


# ── Open Command Prompt Here (Context Menu) ───────────────────────────────

_CMD_HERE_KEY = r"HKEY_CLASSES_ROOT\Directory\Background\shell\cmd_here"
_CMD_HERE_CMD = rf"{_CMD_HERE_KEY}\command"
_CMD_KEYS = [_CMD_HERE_KEY, _CMD_HERE_CMD]


def _apply_cmd_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: add 'Open Command Prompt Here' context menu")
    SESSION.backup(_CMD_KEYS, "CmdHere")
    _run = lambda a: subprocess.run(
        ["reg", *a], check=True, capture_output=True, text=True
    )
    _run(["add", _CMD_HERE_KEY, "/ve", "/d", "Open Command Prompt Here", "/f"])
    _run(["add", _CMD_HERE_KEY, "/v", "Icon", "/d", "cmd.exe", "/f"])
    _run(["add", _CMD_HERE_CMD, "/ve", "/d", 'cmd.exe /k cd /d "%V"', "/f"])


def _remove_cmd_here(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["reg", "delete", _CMD_HERE_KEY, "/f"],
        check=False, capture_output=True,
    )


def _detect_cmd_here() -> bool:
    return SESSION.key_exists(_CMD_HERE_KEY)


# ── Add Hash File Context Menu ────────────────────────────────────────────

_HASH_KEY = r"HKEY_CLASSES_ROOT\*\shell\GetFileHash"
_HASH_CMD = rf"{_HASH_KEY}\command"
_HASH_KEYS = [_HASH_KEY, _HASH_CMD]


def _apply_hash_context(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Shell: add 'Get File Hash' context menu")
    SESSION.backup(_HASH_KEYS, "FileHash")
    _run = lambda a: subprocess.run(
        ["reg", *a], check=True, capture_output=True, text=True
    )
    _run(["add", _HASH_KEY, "/ve", "/d", "Get File Hash (SHA256)", "/f"])
    _run(["add", _HASH_KEY, "/v", "Icon", "/d", "powershell.exe", "/f"])
    ps_cmd = (
        'powershell.exe -NoProfile -Command "'
        "Get-FileHash '%1' -Algorithm SHA256 | "
        "Format-List; pause"
        '"'
    )
    _run(["add", _HASH_CMD, "/ve", "/d", ps_cmd, "/f"])


def _remove_hash_context(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["reg", "delete", _HASH_KEY, "/f"],
        check=False, capture_output=True,
    )


def _detect_hash_context() -> bool:
    return SESSION.key_exists(_HASH_KEY)


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
        description=(
            "Adds a 'Take Ownership' entry to the right-click context "
            "menu for files, folders, and drives."
        ),
        tags=["shell", "context-menu", "ownership"],
    ),
    TweakDef(
        id="open-cmd-here",
        label="'Open CMD Here' Context Menu",
        category="Shell",
        apply_fn=_apply_cmd_here,
        remove_fn=_remove_cmd_here,
        detect_fn=_detect_cmd_here,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_CMD_KEYS,
        description="Adds 'Open Command Prompt Here' to the folder background context menu.",
        tags=["shell", "context-menu", "cmd"],
    ),
    TweakDef(
        id="file-hash-context",
        label="'Get File Hash' Context Menu",
        category="Shell",
        apply_fn=_apply_hash_context,
        remove_fn=_remove_hash_context,
        detect_fn=_detect_hash_context,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_HASH_KEYS,
        description="Adds 'Get File Hash (SHA256)' to the right-click menu for any file.",
        tags=["shell", "context-menu", "hash", "security"],
    ),
]
