"""Privacy tweaks — Telemetry and Cortana."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Telemetry ────────────────────────────────────────────────────────────────

_TELEMETRY_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft" r"\Windows\DataCollection"
)
_TELEMETRY_DATA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\DataCollection"
)
_TELEMETRY_KEYS = [_TELEMETRY_POLICY, _TELEMETRY_DATA]


def apply_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableTelemetry")
    SESSION.backup(_TELEMETRY_KEYS, "Telemetry")
    SESSION.set_dword(_TELEMETRY_POLICY, "AllowTelemetry", 0)
    SESSION.set_dword(_TELEMETRY_DATA, "AllowTelemetry", 0)
    SESSION.set_dword(_TELEMETRY_POLICY, "DoNotShowFeedbackNotifications", 1)
    SESSION.log("Completed Add-DisableTelemetry")


def remove_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableTelemetry")
    SESSION.backup(_TELEMETRY_KEYS, "Telemetry_Remove")
    SESSION.set_dword(_TELEMETRY_POLICY, "AllowTelemetry", 3)
    SESSION.set_dword(_TELEMETRY_DATA, "AllowTelemetry", 3)
    SESSION.delete_value(_TELEMETRY_POLICY, "DoNotShowFeedbackNotifications")
    SESSION.log("Completed Remove-DisableTelemetry")


def detect_disable_telemetry() -> bool:
    return SESSION.read_dword(_TELEMETRY_POLICY, "AllowTelemetry") == 0


# ── Cortana ──────────────────────────────────────────────────────────────────

_CORTANA_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft" r"\Windows\Windows Search"
)


def apply_disable_cortana(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableCortana")
    SESSION.backup([_CORTANA_KEY], "Cortana")
    SESSION.set_dword(_CORTANA_KEY, "AllowCortana", 0)
    SESSION.set_dword(_CORTANA_KEY, "AllowSearchToUseLocation", 0)
    SESSION.set_dword(_CORTANA_KEY, "DisableWebSearch", 1)
    SESSION.set_dword(_CORTANA_KEY, "ConnectedSearchUseWeb", 0)
    SESSION.log("Completed Add-DisableCortana")


def remove_disable_cortana(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableCortana")
    SESSION.backup([_CORTANA_KEY], "Cortana_Remove")
    SESSION.delete_value(_CORTANA_KEY, "AllowCortana")
    SESSION.delete_value(_CORTANA_KEY, "AllowSearchToUseLocation")
    SESSION.delete_value(_CORTANA_KEY, "DisableWebSearch")
    SESSION.delete_value(_CORTANA_KEY, "ConnectedSearchUseWeb")
    SESSION.log("Completed Remove-DisableCortana")


def detect_disable_cortana() -> bool:
    return SESSION.read_dword(_CORTANA_KEY, "AllowCortana") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-telemetry",
        label="Disable Telemetry",
        category="Privacy",
        apply_fn=apply_disable_telemetry,
        remove_fn=remove_disable_telemetry,
        detect_fn=detect_disable_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_TELEMETRY_KEYS,
    ),
    TweakDef(
        id="disable-cortana",
        label="Disable Cortana",
        category="Privacy",
        apply_fn=apply_disable_cortana,
        remove_fn=remove_disable_cortana,
        detect_fn=detect_disable_cortana,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CORTANA_KEY],
    ),
]
