"""Visual Studio Code registry tweaks (policy-based)."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_VSCODE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"
_VSCODE_KEYS = [_VSCODE_POLICY]


# ── Disable VSCode Telemetry ────────────────────────────────────────────────


def apply_disable_vscode_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableVSCodeTelemetry")
    SESSION.backup(_VSCODE_KEYS, "VSCodeTelemetry")
    SESSION.set_string(_VSCODE_POLICY, "telemetry.telemetryLevel", "off")
    SESSION.set_dword(_VSCODE_POLICY, "telemetry.enableTelemetry", 0)
    SESSION.set_dword(_VSCODE_POLICY, "telemetry.enableCrashReporter", 0)
    SESSION.log("Completed Add-DisableVSCodeTelemetry")


def remove_disable_vscode_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableVSCodeTelemetry")
    SESSION.backup(_VSCODE_KEYS, "VSCodeTelemetry_Remove")
    SESSION.delete_value(_VSCODE_POLICY, "telemetry.telemetryLevel")
    SESSION.delete_value(_VSCODE_POLICY, "telemetry.enableTelemetry")
    SESSION.delete_value(_VSCODE_POLICY, "telemetry.enableCrashReporter")
    SESSION.log("Completed Remove-DisableVSCodeTelemetry")


def detect_disable_vscode_telemetry() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "telemetry.telemetryLevel") == "off"


# ── Disable VSCode Auto-Update ──────────────────────────────────────────────


def apply_disable_vscode_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableVSCodeUpdate")
    SESSION.backup(_VSCODE_KEYS, "VSCodeUpdate")
    SESSION.set_string(_VSCODE_POLICY, "update.mode", "none")
    SESSION.log("Completed Add-DisableVSCodeUpdate")


def remove_disable_vscode_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableVSCodeUpdate")
    SESSION.backup(_VSCODE_KEYS, "VSCodeUpdate_Remove")
    SESSION.delete_value(_VSCODE_POLICY, "update.mode")
    SESSION.log("Completed Remove-DisableVSCodeUpdate")


def detect_disable_vscode_update() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "update.mode") == "none"


# ── Disable VSCode Extensions Auto-Update ────────────────────────────────────


def apply_disable_vscode_ext_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableVSCodeExtUpdate")
    SESSION.backup(_VSCODE_KEYS, "VSCodeExtUpdate")
    SESSION.set_dword(_VSCODE_POLICY, "extensions.autoUpdate", 0)
    SESSION.set_dword(_VSCODE_POLICY, "extensions.autoCheckUpdates", 0)
    SESSION.log("Completed Add-DisableVSCodeExtUpdate")


def remove_disable_vscode_ext_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableVSCodeExtUpdate")
    SESSION.backup(_VSCODE_KEYS, "VSCodeExtUpdate_Remove")
    SESSION.delete_value(_VSCODE_POLICY, "extensions.autoUpdate")
    SESSION.delete_value(_VSCODE_POLICY, "extensions.autoCheckUpdates")
    SESSION.log("Completed Remove-DisableVSCodeExtUpdate")


def detect_disable_vscode_ext_update() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "extensions.autoUpdate") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-vscode-telemetry",
        label="Disable VS Code Telemetry",
        category="VS Code",
        apply_fn=apply_disable_vscode_telemetry,
        remove_fn=remove_disable_vscode_telemetry,
        detect_fn=detect_disable_vscode_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Disables VS Code telemetry, crash reporting, and usage "
            "data collection via machine-level policy."
        ),
    ),
    TweakDef(
        id="disable-vscode-update",
        label="Disable VS Code Auto-Update",
        category="VS Code",
        apply_fn=apply_disable_vscode_update,
        remove_fn=remove_disable_vscode_update,
        detect_fn=detect_disable_vscode_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_VSCODE_KEYS,
        description="Prevents VS Code from auto-updating to new versions.",
    ),
    TweakDef(
        id="disable-vscode-ext-update",
        label="Disable VS Code Extension Auto-Update",
        category="VS Code",
        apply_fn=apply_disable_vscode_ext_update,
        remove_fn=remove_disable_vscode_ext_update,
        detect_fn=detect_disable_vscode_ext_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_VSCODE_KEYS,
        description="Prevents VS Code extensions from auto-updating.",
    ),
]
