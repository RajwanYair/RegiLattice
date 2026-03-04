"""Boot tweaks — Verbose boot, boot logo, timeout."""

from __future__ import annotations

import subprocess
from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows" r"\CurrentVersion\Policies\System"
)
_BCD = r"HKEY_LOCAL_MACHINE\BCD00000000\Objects"
_BOOTUX = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\BootControl"
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


# ── Disable Boot Logo ────────────────────────────────────────────────────────


def apply_disable_boot_logo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: disable boot splash logo via bcdedit")
    subprocess.run(
        ["bcdedit", "/set", "quietboot", "off"],
        check=False, capture_output=True, text=True,
    )
    SESSION.log("Completed disable-boot-logo")


def remove_disable_boot_logo(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["bcdedit", "/set", "quietboot", "on"],
        check=False, capture_output=True, text=True,
    )


def detect_disable_boot_logo() -> bool:
    try:
        r = subprocess.run(
            ["bcdedit", "/enum", "{current}"],
            capture_output=True, text=True, timeout=5,
        )
        return "quietboot" in r.stdout.lower() and "off" in r.stdout.lower()
    except Exception:
        return False


# ── Reduce Boot Timeout ──────────────────────────────────────────────────────


def apply_fast_boot_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Boot: set OS selection timeout to 3 seconds")
    subprocess.run(
        ["bcdedit", "/timeout", "3"],
        check=False, capture_output=True, text=True,
    )


def remove_fast_boot_timeout(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    subprocess.run(
        ["bcdedit", "/timeout", "30"],
        check=False, capture_output=True, text=True,
    )


def detect_fast_boot_timeout() -> bool:
    try:
        r = subprocess.run(
            ["bcdedit", "/enum", "{bootmgr}"],
            capture_output=True, text=True, timeout=5,
        )
        for line in r.stdout.splitlines():
            if "timeout" in line.lower():
                return "3" in line.split()[-1]
        return False
    except Exception:
        return False


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
        description="Shows detailed service and driver status messages during boot.",
        tags=["boot", "debug", "diagnostic"],
    ),
    TweakDef(
        id="disable-boot-logo",
        label="Disable Boot Splash Logo",
        category="Boot",
        apply_fn=apply_disable_boot_logo,
        remove_fn=remove_disable_boot_logo,
        detect_fn=detect_disable_boot_logo,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[],
        description="Disables the Windows boot splash animation for a text-mode boot.",
        tags=["boot", "performance", "splash"],
    ),
    TweakDef(
        id="fast-boot-timeout",
        label="Reduce Boot Menu Timeout (3s)",
        category="Boot",
        apply_fn=apply_fast_boot_timeout,
        remove_fn=remove_fast_boot_timeout,
        detect_fn=detect_fast_boot_timeout,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[],
        description="Reduces the OS selection boot menu timeout from 30s to 3s.",
        tags=["boot", "performance", "timeout"],
    ),
]
