"""Microsoft Office registry tweaks."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_OFFICE_COMMON = r"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry"
_OFFICE_FEEDBACK = r"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Feedback"
_OFFICE_PRIVACY = r"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy"
_OFFICE_CONNECTED = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common" r"\General"
)
_OFFICE_START = r"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\General"
_OFFICE_TELEMETRY_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\ClientTelemetry"
)

_TELEMETRY_KEYS = [
    _OFFICE_COMMON,
    _OFFICE_FEEDBACK,
    _OFFICE_PRIVACY,
    _OFFICE_TELEMETRY_POLICY,
]
_UX_KEYS = [_OFFICE_START, _OFFICE_CONNECTED]


# ── Disable Office Telemetry ────────────────────────────────────────────────


def apply_disable_office_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableOfficeTelemetry")
    SESSION.backup(_TELEMETRY_KEYS, "OfficeTelemetry")
    # DisableTelemetry: 1 = disable upload of client telemetry data
    SESSION.set_dword(_OFFICE_COMMON, "DisableTelemetry", 1)
    SESSION.set_dword(_OFFICE_FEEDBACK, "Enabled", 0)
    SESSION.set_dword(_OFFICE_PRIVACY, "DisconnectedState", 2)
    SESSION.set_dword(_OFFICE_PRIVACY, "UserContentDisabled", 2)
    SESSION.set_dword(_OFFICE_PRIVACY, "DownloadContentDisabled", 2)
    SESSION.set_dword(_OFFICE_PRIVACY, "ControllerConnectedServicesEnabled", 2)
    SESSION.set_dword(_OFFICE_TELEMETRY_POLICY, "DisableTelemetry", 1)
    SESSION.set_dword(_OFFICE_TELEMETRY_POLICY, "SendTelemetry", 3)
    SESSION.log("Completed Add-DisableOfficeTelemetry")


def remove_disable_office_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableOfficeTelemetry")
    SESSION.backup(_TELEMETRY_KEYS, "OfficeTelemetry_Remove")
    SESSION.delete_value(_OFFICE_COMMON, "DisableTelemetry")
    SESSION.delete_value(_OFFICE_FEEDBACK, "Enabled")
    for val in (
        "DisconnectedState",
        "UserContentDisabled",
        "DownloadContentDisabled",
        "ControllerConnectedServicesEnabled",
    ):
        SESSION.delete_value(_OFFICE_PRIVACY, val)
    SESSION.delete_value(_OFFICE_TELEMETRY_POLICY, "DisableTelemetry")
    SESSION.delete_value(_OFFICE_TELEMETRY_POLICY, "SendTelemetry")
    SESSION.log("Completed Remove-DisableOfficeTelemetry")


def detect_disable_office_telemetry() -> bool:
    return SESSION.read_dword(_OFFICE_COMMON, "DisableTelemetry") == 1


# ── Disable Office Start Screen ─────────────────────────────────────────────


def apply_disable_office_start_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableOfficeStartScreen")
    SESSION.backup(_UX_KEYS, "OfficeStartScreen")
    SESSION.set_dword(_OFFICE_START, "DisableBootToOfficeStart", 1)
    SESSION.set_dword(_OFFICE_START, "ShownFirstRunOptin", 1)
    SESSION.log("Completed Add-DisableOfficeStartScreen")


def remove_disable_office_start_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableOfficeStartScreen")
    SESSION.backup(_UX_KEYS, "OfficeStartScreen_Remove")
    SESSION.delete_value(_OFFICE_START, "DisableBootToOfficeStart")
    SESSION.delete_value(_OFFICE_START, "ShownFirstRunOptin")
    SESSION.log("Completed Remove-DisableOfficeStartScreen")


def detect_disable_office_start_screen() -> bool:
    return SESSION.read_dword(_OFFICE_START, "DisableBootToOfficeStart") == 1


# ── Disable Office Connected Experiences ─────────────────────────────────────

_OFFICE_CONNECTED_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Privacy"
)


def apply_disable_office_connected(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableOfficeConnected")
    SESSION.backup([_OFFICE_CONNECTED_KEY], "OfficeConnected")
    SESSION.set_dword(_OFFICE_CONNECTED_KEY, "ControllerConnectedServicesEnabled", 2)
    SESSION.set_dword(_OFFICE_CONNECTED_KEY, "DisconnectedState", 2)
    SESSION.log("Completed Add-DisableOfficeConnected")


def remove_disable_office_connected(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableOfficeConnected")
    SESSION.backup([_OFFICE_CONNECTED_KEY], "OfficeConnected_Remove")
    SESSION.delete_value(_OFFICE_CONNECTED_KEY, "ControllerConnectedServicesEnabled")
    SESSION.delete_value(_OFFICE_CONNECTED_KEY, "DisconnectedState")
    SESSION.log("Completed Remove-DisableOfficeConnected")


def detect_disable_office_connected() -> bool:
    return SESSION.read_dword(_OFFICE_CONNECTED_KEY, "DisconnectedState") == 2


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-office-telemetry",
        label="Disable Office Telemetry",
        category="Office",
        apply_fn=apply_disable_office_telemetry,
        remove_fn=remove_disable_office_telemetry,
        detect_fn=detect_disable_office_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_TELEMETRY_KEYS,
        description=(
            "Disables Microsoft Office telemetry, feedback prompts, "
            "and connected services data collection."
        ),
    ),
    TweakDef(
        id="disable-office-start-screen",
        label="Disable Office Start Screen",
        category="Office",
        apply_fn=apply_disable_office_start_screen,
        remove_fn=remove_disable_office_start_screen,
        detect_fn=detect_disable_office_start_screen,
        needs_admin=False,
        corp_safe=True,
        registry_keys=_UX_KEYS,
        description=(
            "Skips the Office Start screen/splash and opens directly "
            "to a blank document."
        ),
    ),
    TweakDef(
        id="disable-office-connected",
        label="Disable Office Connected Experiences",
        category="Office",
        apply_fn=apply_disable_office_connected,
        remove_fn=remove_disable_office_connected,
        detect_fn=detect_disable_office_connected,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[_OFFICE_CONNECTED_KEY],
        description=(
            "Disables Office connected experiences (cloud-powered "
            "features like Designer, Editor suggestions, etc.)."
        ),
    ),
]
