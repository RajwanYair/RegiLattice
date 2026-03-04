"""Adobe Reader / Acrobat registry tweaks.

Covers: auto-update, telemetry, protected mode, JavaScript.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

# Adobe Reader DC / Acrobat DC (major version key)
_READER_CU = r"HKEY_CURRENT_USER\Software\Adobe\Acrobat Reader\DC\AVGeneral"
_ACROBAT_CU = r"HKEY_CURRENT_USER\Software\Adobe\Adobe Acrobat\DC\AVGeneral"
_UPDATER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_UPDATER2 = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Adobe Acrobat\DC\FeatureLockDown"
_ARM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe ARM\1.0\ARM"
_JS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_WELCOME = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen"


# ── Disable Adobe Auto-Update ───────────────────────────────────────────────


def _apply_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable auto-update (Reader + Acrobat)")
    SESSION.backup([_ARM, _UPDATER, _UPDATER2], "AdobeUpdate")
    SESSION.set_dword(_ARM, "iCheckReader", 0)
    SESSION.set_dword(_UPDATER, "bUpdater", 0)
    SESSION.set_dword(_UPDATER2, "bUpdater", 0)


def _remove_disable_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ARM, "iCheckReader", 3)
    SESSION.delete_value(_UPDATER, "bUpdater")
    SESSION.delete_value(_UPDATER2, "bUpdater")


def _detect_disable_update() -> bool:
    return SESSION.read_dword(_ARM, "iCheckReader") == 0


# ── Disable Adobe Telemetry ─────────────────────────────────────────────────


def _apply_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable usage data collection")
    SESSION.backup([_UPDATER], "AdobeTelemetry")
    SESSION.set_dword(_UPDATER, "bUsageMeasurement", 0)
    SESSION.set_dword(_UPDATER, "bAcroSuppressUpsell", 1)


def _remove_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_UPDATER, "bUsageMeasurement")
    SESSION.delete_value(_UPDATER, "bAcroSuppressUpsell")


def _detect_disable_telemetry() -> bool:
    return SESSION.read_dword(_UPDATER, "bUsageMeasurement") == 0


# ── Disable Adobe JavaScript ────────────────────────────────────────────────


def _apply_disable_js(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable JavaScript in PDFs")
    SESSION.backup([_JS], "AdobeJS")
    SESSION.set_dword(_JS, "bDisableJavaScript", 1)


def _remove_disable_js(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_JS, "bDisableJavaScript", 0)


def _detect_disable_js() -> bool:
    return SESSION.read_dword(_JS, "bDisableJavaScript") == 1


# ── Disable Adobe Welcome Screen ────────────────────────────────────────────


def _apply_disable_welcome(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable welcome screen")
    SESSION.backup([_WELCOME], "AdobeWelcome")
    SESSION.set_dword(_WELCOME, "bShowWelcomeScreen", 0)


def _remove_disable_welcome(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WELCOME, "bShowWelcomeScreen")


def _detect_disable_welcome() -> bool:
    return SESSION.read_dword(_WELCOME, "bShowWelcomeScreen") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-adobe-update",
        label="Disable Adobe Auto-Update",
        category="Adobe",
        apply_fn=_apply_disable_update,
        remove_fn=_remove_disable_update,
        detect_fn=_detect_disable_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ARM, _UPDATER, _UPDATER2],
        description="Disables automatic updates for Adobe Reader and Acrobat DC.",
        tags=["adobe", "update"],
    ),
    TweakDef(
        id="disable-adobe-telemetry",
        label="Disable Adobe Telemetry",
        category="Adobe",
        apply_fn=_apply_disable_telemetry,
        remove_fn=_remove_disable_telemetry,
        detect_fn=_detect_disable_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_UPDATER],
        description="Disables Adobe usage data collection and suppresses upsell prompts.",
        tags=["adobe", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-adobe-javascript",
        label="Disable Adobe PDF JavaScript",
        category="Adobe",
        apply_fn=_apply_disable_js,
        remove_fn=_remove_disable_js,
        detect_fn=_detect_disable_js,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_JS],
        description=(
            "Disables JavaScript execution in PDF documents — "
            "major security hardening."
        ),
        tags=["adobe", "security", "javascript"],
    ),
    TweakDef(
        id="disable-adobe-welcome",
        label="Disable Adobe Welcome Screen",
        category="Adobe",
        apply_fn=_apply_disable_welcome,
        remove_fn=_remove_disable_welcome,
        detect_fn=_detect_disable_welcome,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WELCOME],
        description="Suppresses the Adobe Reader welcome / start screen on launch.",
        tags=["adobe", "ux"],
    ),
]
