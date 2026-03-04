"""Adobe Reader / Acrobat registry tweaks.

Covers: auto-update, telemetry, protected mode, JavaScript.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

# Adobe Reader DC / Acrobat DC policy paths
_UPDATER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_UPDATER2 = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Adobe Acrobat\DC\FeatureLockDown"
_ARM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Adobe\Adobe ARM\1.0\ARM"
_JS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_WELCOME = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cWelcomeScreen"
_PROTECTED = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown"
_CLOUD = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Adobe\Acrobat Reader\DC\FeatureLockDown\cCloud"


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


# ── Enable Adobe Protected Mode (Sandbox) ─────────────────────────────────


def _apply_protected_mode(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: enable Protected Mode (sandbox)")
    SESSION.backup([_PROTECTED], "AdobeProtected")
    SESSION.set_dword(_PROTECTED, "bProtectedMode", 1)
    SESSION.set_dword(_PROTECTED, "iProtectedView", 2)  # 2 = All files


def _remove_protected_mode(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PROTECTED, "bProtectedMode")
    SESSION.delete_value(_PROTECTED, "iProtectedView")


def _detect_protected_mode() -> bool:
    return SESSION.read_dword(_PROTECTED, "bProtectedMode") == 1


# ── Disable Adobe Cloud Services ──────────────────────────────────────────


def _apply_disable_cloud(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Adobe: disable cloud storage integration")
    SESSION.backup([_CLOUD], "AdobeCloud")
    SESSION.set_dword(_CLOUD, "bDisableADCFileStore", 1)
    SESSION.set_dword(_CLOUD, "bUpdatesHidden", 1)


def _remove_disable_cloud(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD, "bDisableADCFileStore")
    SESSION.delete_value(_CLOUD, "bUpdatesHidden")


def _detect_disable_cloud() -> bool:
    return SESSION.read_dword(_CLOUD, "bDisableADCFileStore") == 1


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
    TweakDef(
        id="enable-adobe-protected-mode",
        label="Enable Adobe Protected Mode",
        category="Adobe",
        apply_fn=_apply_protected_mode,
        remove_fn=_remove_protected_mode,
        detect_fn=_detect_protected_mode,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_PROTECTED],
        description="Enables Protected Mode sandbox and Protected View for all PDF files.",
        tags=["adobe", "security", "sandbox"],
    ),
    TweakDef(
        id="disable-adobe-cloud",
        label="Disable Adobe Cloud Services",
        category="Adobe",
        apply_fn=_apply_disable_cloud,
        remove_fn=_remove_disable_cloud,
        detect_fn=_detect_disable_cloud,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLOUD],
        description="Disables Adobe Document Cloud file storage integration.",
        tags=["adobe", "cloud", "privacy"],
    ),
]
