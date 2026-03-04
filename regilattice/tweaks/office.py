"""Microsoft Office registry tweaks — supports Office 2010 through 365.

Iterates detected Office versions (14.0 = 2010, 15.0 = 2013, 16.0 = 2016/
2019/365) and applies tweaks to all present installations.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Version-agnostic helpers ─────────────────────────────────────────────────

# All supported Office version keys (oldest → newest)
_VERSIONS = ("14.0", "15.0", "16.0")
_OFFICE_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Office"
_OFFICE_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office"
_OFFICE_COMMON = r"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry"


def _ver_key(ver: str, suffix: str) -> str:
    """Build HKCU Office version-specific key path."""
    return rf"{_OFFICE_CU}\{ver}\Common\{suffix}"


def _ver_policy(ver: str, suffix: str) -> str:
    """Build HKLM Office policy version-specific key path."""
    return rf"{_OFFICE_LM}\{ver}\Common\{suffix}"


def _detected_versions() -> list[str]:
    """Return Office versions actually present in the registry."""
    found = []
    for ver in _VERSIONS:
        key = rf"{_OFFICE_CU}\{ver}\Common"
        if SESSION.key_exists(key):
            found.append(ver)
    # Always include 16.0 for modern M365 which may not write Common early
    if "16.0" not in found:
        found.append("16.0")
    return found


def _all_keys_for(suffix: str, *, policy: bool = False) -> list[str]:
    """Return key paths for all detected versions."""
    fn = _ver_policy if policy else _ver_key
    return [fn(v, suffix) for v in _detected_versions()]


# ── Disable Office Telemetry ────────────────────────────────────────────────


def _apply_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable telemetry (all versions)")
    # Version-common key
    SESSION.backup([_OFFICE_COMMON], "OfficeTelemetry")
    SESSION.set_dword(_OFFICE_COMMON, "DisableTelemetry", 1)
    # Per-version keys
    for ver in _detected_versions():
        feedback = _ver_key(ver, "Feedback")
        privacy = _ver_key(ver, "Privacy")
        policy = _ver_policy(ver, "ClientTelemetry")
        SESSION.backup([feedback, privacy, policy], f"OfficeTelemetry_{ver}")
        SESSION.set_dword(feedback, "Enabled", 0)
        SESSION.set_dword(privacy, "DisconnectedState", 2)
        SESSION.set_dword(privacy, "UserContentDisabled", 2)
        SESSION.set_dword(privacy, "DownloadContentDisabled", 2)
        SESSION.set_dword(privacy, "ControllerConnectedServicesEnabled", 2)
        SESSION.set_dword(policy, "DisableTelemetry", 1)
        SESSION.set_dword(policy, "SendTelemetry", 3)


def _remove_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: restore telemetry defaults (all versions)")
    SESSION.delete_value(_OFFICE_COMMON, "DisableTelemetry")
    for ver in _detected_versions():
        feedback = _ver_key(ver, "Feedback")
        privacy = _ver_key(ver, "Privacy")
        policy = _ver_policy(ver, "ClientTelemetry")
        SESSION.delete_value(feedback, "Enabled")
        for val in (
            "DisconnectedState",
            "UserContentDisabled",
            "DownloadContentDisabled",
            "ControllerConnectedServicesEnabled",
        ):
            SESSION.delete_value(privacy, val)
        SESSION.delete_value(policy, "DisableTelemetry")
        SESSION.delete_value(policy, "SendTelemetry")


def _detect_telemetry() -> bool:
    return SESSION.read_dword(_OFFICE_COMMON, "DisableTelemetry") == 1


# ── Disable Office Start Screen ─────────────────────────────────────────────


def _apply_start_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable start screen (all versions)")
    for ver in _detected_versions():
        general = _ver_key(ver, "General")
        SESSION.backup([general], f"OfficeStartScreen_{ver}")
        SESSION.set_dword(general, "DisableBootToOfficeStart", 1)
        SESSION.set_dword(general, "ShownFirstRunOptin", 1)


def _remove_start_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        general = _ver_key(ver, "General")
        SESSION.delete_value(general, "DisableBootToOfficeStart")
        SESSION.delete_value(general, "ShownFirstRunOptin")


def _detect_start_screen() -> bool:
    for ver in _detected_versions():
        if SESSION.read_dword(_ver_key(ver, "General"), "DisableBootToOfficeStart") == 1:
            return True
    return False


# ── Disable Office Connected Experiences ─────────────────────────────────────


def _apply_connected(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable connected experiences (all versions)")
    for ver in _detected_versions():
        privacy = _ver_key(ver, "Privacy")
        SESSION.backup([privacy], f"OfficeConnected_{ver}")
        SESSION.set_dword(privacy, "ControllerConnectedServicesEnabled", 2)
        SESSION.set_dword(privacy, "DisconnectedState", 2)


def _remove_connected(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        privacy = _ver_key(ver, "Privacy")
        SESSION.delete_value(privacy, "ControllerConnectedServicesEnabled")
        SESSION.delete_value(privacy, "DisconnectedState")


def _detect_connected() -> bool:
    for ver in _detected_versions():
        if SESSION.read_dword(_ver_key(ver, "Privacy"), "DisconnectedState") == 2:
            return True
    return False


# ── Disable Office Hardware Acceleration ─────────────────────────────────────


def _apply_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable hardware acceleration (all versions)")
    for ver in _detected_versions():
        graphics = _ver_key(ver, "Graphics")
        SESSION.backup([graphics], f"OfficeHWAccel_{ver}")
        SESSION.set_dword(graphics, "DisableHardwareAcceleration", 1)


def _remove_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        graphics = _ver_key(ver, "Graphics")
        SESSION.delete_value(graphics, "DisableHardwareAcceleration")


def _detect_hwaccel() -> bool:
    for ver in _detected_versions():
        if (
            SESSION.read_dword(
                _ver_key(ver, "Graphics"), "DisableHardwareAcceleration"
            )
            == 1
        ):
            return True
    return False


# ── Disable Office Macro Security (Trust VBA) ───────────────────────────────

_OFFICE_APPS = ("Word", "Excel", "PowerPoint", "Access", "Outlook", "Publisher")


def _apply_macro_trust(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: lower macro security to 'Enable all macros'")
    for ver in _detected_versions():
        for app in _OFFICE_APPS:
            sec = rf"{_OFFICE_CU}\{ver}\{app}\Security"
            SESSION.backup([sec], f"MacroSecurity_{ver}_{app}")
            SESSION.set_dword(sec, "VBAWarnings", 1)  # 1=Enable all
            SESSION.set_dword(sec, "AccessVBOM", 1)


def _remove_macro_trust(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        for app in _OFFICE_APPS:
            sec = rf"{_OFFICE_CU}\{ver}\{app}\Security"
            SESSION.delete_value(sec, "VBAWarnings")
            SESSION.delete_value(sec, "AccessVBOM")


def _detect_macro_trust() -> bool:
    for ver in _detected_versions():
        sec = rf"{_OFFICE_CU}\{ver}\Word\Security"
        if SESSION.read_dword(sec, "VBAWarnings") == 1:
            return True
    return False


# ── Disable Office AutoSave / Increase Recovery Interval ─────────────────────


def _apply_autosave(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: set AutoRecover interval to 2 min, enable save")
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            opts = rf"{_OFFICE_CU}\{ver}\{app}\Options"
            SESSION.backup([opts], f"AutoSave_{ver}_{app}")
            SESSION.set_dword(opts, "SaveInterval", 2)  # minutes


def _remove_autosave(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            opts = rf"{_OFFICE_CU}\{ver}\{app}\Options"
            SESSION.set_dword(opts, "SaveInterval", 10)  # default


def _detect_autosave() -> bool:
    for ver in _detected_versions():
        opts = rf"{_OFFICE_CU}\{ver}\Word\Options"
        if SESSION.read_dword(opts, "SaveInterval") == 2:
            return True
    return False


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-office-telemetry",
        label="Disable Office Telemetry",
        category="Office",
        apply_fn=_apply_telemetry,
        remove_fn=_remove_telemetry,
        detect_fn=_detect_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_OFFICE_COMMON],
        description=(
            "Disables Microsoft Office telemetry, feedback, and connected "
            "services data collection across all installed versions "
            "(2010–365)."
        ),
        tags=["office", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-office-start-screen",
        label="Disable Office Start Screen",
        category="Office",
        apply_fn=_apply_start_screen,
        remove_fn=_remove_start_screen,
        detect_fn=_detect_start_screen,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[],
        description=(
            "Skips the Office Start screen and opens directly to a blank "
            "document (all versions)."
        ),
        tags=["office", "ux"],
    ),
    TweakDef(
        id="disable-office-connected",
        label="Disable Office Connected Experiences",
        category="Office",
        apply_fn=_apply_connected,
        remove_fn=_remove_connected,
        detect_fn=_detect_connected,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[],
        description=(
            "Disables cloud-powered Office features (Designer, Editor, "
            "etc.) across all versions."
        ),
        tags=["office", "privacy", "cloud"],
    ),
    TweakDef(
        id="disable-office-hwaccel",
        label="Disable Office Hardware Acceleration",
        category="Office",
        apply_fn=_apply_hwaccel,
        remove_fn=_remove_hwaccel,
        detect_fn=_detect_hwaccel,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[],
        description=(
            "Disables GPU hardware acceleration in Office apps to fix "
            "display glitches (all versions)."
        ),
        tags=["office", "performance", "gpu"],
    ),
    TweakDef(
        id="office-macro-trust",
        label="Trust VBA Macros (All Apps)",
        category="Office",
        apply_fn=_apply_macro_trust,
        remove_fn=_remove_macro_trust,
        detect_fn=_detect_macro_trust,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[],
        description=(
            "Lowers macro security to 'Enable all' and trusts the VBA "
            "project object model (Word, Excel, PowerPoint, Access, "
            "Outlook, Publisher)."
        ),
        tags=["office", "macros", "security"],
    ),
    TweakDef(
        id="office-autosave-fast",
        label="Set Office AutoRecover to 2 min",
        category="Office",
        apply_fn=_apply_autosave,
        remove_fn=_remove_autosave,
        detect_fn=_detect_autosave,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[],
        description=(
            "Sets the AutoRecover interval to 2 minutes for Word, Excel, "
            "and PowerPoint (all versions)."
        ),
        tags=["office", "autosave", "recovery"],
    ),
]
