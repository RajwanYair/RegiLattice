"""Mozilla Firefox registry tweaks (policy-based).

Firefox respects HKLM policies via its enterprise policy engine.
These correspond to ``policies.json`` entries but set via registry.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_FF_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"
_FF_KEYS = [_FF_POLICY]


# ── Disable Firefox Telemetry ───────────────────────────────────────────────


def apply_disable_ff_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableFirefoxTelemetry")
    SESSION.backup(_FF_KEYS, "FirefoxTelemetry")
    SESSION.set_dword(_FF_POLICY, "DisableTelemetry", 1)
    SESSION.set_dword(_FF_POLICY, "DisableFirefoxStudies", 1)
    SESSION.set_dword(_FF_POLICY, "DisableDefaultBrowserAgent", 1)
    SESSION.log("Completed Add-DisableFirefoxTelemetry")


def remove_disable_ff_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableFirefoxTelemetry")
    SESSION.backup(_FF_KEYS, "FirefoxTelemetry_Remove")
    SESSION.delete_value(_FF_POLICY, "DisableTelemetry")
    SESSION.delete_value(_FF_POLICY, "DisableFirefoxStudies")
    SESSION.delete_value(_FF_POLICY, "DisableDefaultBrowserAgent")
    SESSION.log("Completed Remove-DisableFirefoxTelemetry")


def detect_disable_ff_telemetry() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableTelemetry") == 1


# ── Disable Firefox Pocket ──────────────────────────────────────────────────


def apply_disable_ff_pocket(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableFirefoxPocket")
    SESSION.backup(_FF_KEYS, "FirefoxPocket")
    SESSION.set_dword(_FF_POLICY, "DisablePocket", 1)
    SESSION.log("Completed Add-DisableFirefoxPocket")


def remove_disable_ff_pocket(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableFirefoxPocket")
    SESSION.backup(_FF_KEYS, "FirefoxPocket_Remove")
    SESSION.delete_value(_FF_POLICY, "DisablePocket")
    SESSION.log("Completed Remove-DisableFirefoxPocket")


def detect_disable_ff_pocket() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisablePocket") == 1


# ── Disable Firefox Auto-Update ─────────────────────────────────────────────


def apply_disable_ff_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableFirefoxUpdate")
    SESSION.backup(_FF_KEYS, "FirefoxUpdate")
    SESSION.set_dword(_FF_POLICY, "DisableAppUpdate", 1)
    SESSION.log("Completed Add-DisableFirefoxUpdate")


def remove_disable_ff_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableFirefoxUpdate")
    SESSION.backup(_FF_KEYS, "FirefoxUpdate_Remove")
    SESSION.delete_value(_FF_POLICY, "DisableAppUpdate")
    SESSION.log("Completed Remove-DisableFirefoxUpdate")


def detect_disable_ff_update() -> bool:
    return SESSION.read_dword(_FF_POLICY, "DisableAppUpdate") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-firefox-telemetry",
        label="Disable Firefox Telemetry & Studies",
        category="Firefox",
        apply_fn=apply_disable_ff_telemetry,
        remove_fn=remove_disable_ff_telemetry,
        detect_fn=detect_disable_ff_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description=(
            "Disables Firefox telemetry, Shield studies, and the "
            "Default Browser Agent background task."
        ),
    ),
    TweakDef(
        id="disable-firefox-pocket",
        label="Disable Firefox Pocket",
        category="Firefox",
        apply_fn=apply_disable_ff_pocket,
        remove_fn=remove_disable_ff_pocket,
        detect_fn=detect_disable_ff_pocket,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_FF_KEYS,
        description="Disables the Pocket integration in Firefox new tab page.",
    ),
    TweakDef(
        id="disable-firefox-update",
        label="Disable Firefox Auto-Update",
        category="Firefox",
        apply_fn=apply_disable_ff_update,
        remove_fn=remove_disable_ff_update,
        detect_fn=detect_disable_ff_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_FF_KEYS,
        description="Prevents Firefox from auto-updating. Use in controlled environments.",
    ),
]
