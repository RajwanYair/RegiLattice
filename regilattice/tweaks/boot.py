"""Boot tweaks — Verbose boot messages."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows" r"\CurrentVersion\Policies\System"
)


# ── Functions ────────────────────────────────────────────────────────────────


def apply_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-VerboseBoot")
    SESSION.backup([_KEY], "VerboseBoot")
    SESSION.set_dword(_KEY, "verbosestatus", 1)
    SESSION.log("Completed Add-VerboseBoot")


def remove_verbose_boot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-VerboseBoot")
    SESSION.backup([_KEY], "VerboseBoot_Remove")
    SESSION.delete_value(_KEY, "verbosestatus")
    SESSION.log("Completed Remove-VerboseBoot")


def detect_verbose_boot() -> bool:
    return SESSION.read_dword(_KEY, "verbosestatus") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="verbose-boot",
        label="Verbose Boot Messages",
        category="Boot",
        apply_fn=apply_verbose_boot,
        remove_fn=remove_verbose_boot,
        detect_fn=detect_verbose_boot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY],
    ),
]
