"""Visual Studio Code registry tweaks (policy-based)."""

from __future__ import annotations

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


# ── Disable VSCode A/B Experiments ───────────────────────────────────────────


def apply_disable_vscode_experiments(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable A/B experiment framework")
    SESSION.backup(_VSCODE_KEYS, "VSCodeExperiments")
    SESSION.set_dword(_VSCODE_POLICY, "experimentalFeatures.enabled", 0)
    SESSION.set_dword(_VSCODE_POLICY, "workbench.enableExperiments", 0)


def remove_disable_vscode_experiments(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "experimentalFeatures.enabled")
    SESSION.delete_value(_VSCODE_POLICY, "workbench.enableExperiments")


def detect_disable_vscode_experiments() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "workbench.enableExperiments") == 0


# ── Disable VSCode Settings Sync ─────────────────────────────────────────────


def apply_disable_vscode_settings_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable settings sync via policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeSettingsSync")
    SESSION.set_dword(_VSCODE_POLICY, "settingsSync.enabled", 0)


def remove_disable_vscode_settings_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "settingsSync.enabled")


def detect_disable_vscode_settings_sync() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "settingsSync.enabled") == 0


# ── Disable VS Code Startup Editor ──────────────────────────────────────────


def apply_disable_vscode_startup_editor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable startup welcome editor via policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeStartupEditor")
    SESSION.set_string(_VSCODE_POLICY, "workbench.startupEditor", "none")


def remove_disable_vscode_startup_editor(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "workbench.startupEditor")


def detect_disable_vscode_startup_editor() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "workbench.startupEditor") == "none"


# ── VS Code Disable Recommended Extensions ──────────────────────────────────


def apply_disable_vscode_recommendations(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable extension recommendations via policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeRecommendations")
    SESSION.set_dword(_VSCODE_POLICY, "extensions.showRecommendationsOnlyOnDemand", 1)
    SESSION.set_dword(_VSCODE_POLICY, "extensions.ignoreRecommendations", 1)


def remove_disable_vscode_recommendations(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "extensions.showRecommendationsOnlyOnDemand")
    SESSION.delete_value(_VSCODE_POLICY, "extensions.ignoreRecommendations")


def detect_disable_vscode_recommendations() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "extensions.ignoreRecommendations") == 1


# ── Disable VS Code Telemetry (Policy-Level) ────────────────────────────────


def apply_vscode_disable_telemetry_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable telemetry via TelemetryLevel policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeTelemetryPolicy")
    SESSION.set_string(_VSCODE_POLICY, "TelemetryLevel", "off")


def remove_vscode_disable_telemetry_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "TelemetryLevel")


def detect_vscode_disable_telemetry_policy() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "TelemetryLevel") == "off"


# ── Disable VS Code A/B Experiments (Policy-Level) ──────────────────────────


def apply_vscode_disable_experiments(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable A/B experiments via EnableExperiments policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeDisableExperiments")
    SESSION.set_dword(_VSCODE_POLICY, "EnableExperiments", 0)


def remove_vscode_disable_experiments(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "EnableExperiments")


def detect_vscode_disable_experiments() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "EnableExperiments") == 0


# ── Disable VS Code Update Notifications ─────────────────────────────────────


def apply_vscode_disable_update_notification(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable update notifications via UpdateMode policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeUpdateNotification")
    SESSION.set_string(_VSCODE_POLICY, "UpdateMode", "none")


def remove_vscode_disable_update_notification(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "UpdateMode")


def detect_vscode_disable_update_notification() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "UpdateMode") == "none"


# ── Disable VS Code Crash Reporter ───────────────────────────────────────────


def apply_vscode_disable_crash_reporter(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable crash reporter via CrashReporterEnabled policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeCrashReporter")
    SESSION.set_dword(_VSCODE_POLICY, "CrashReporterEnabled", 0)


def remove_vscode_disable_crash_reporter(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "CrashReporterEnabled")


def detect_vscode_disable_crash_reporter() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "CrashReporterEnabled") == 0


# ── Restrict VS Code Extension Gallery ───────────────────────────────────────


def apply_vscode_disable_extension_gallery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: restrict extension gallery via policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeExtGallery")
    SESSION.set_string(_VSCODE_POLICY, "ExtensionGalleryServiceUrl", "")


def remove_vscode_disable_extension_gallery(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "ExtensionGalleryServiceUrl")


def detect_vscode_disable_extension_gallery() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "ExtensionGalleryServiceUrl") == ""


# ── Restrict VS Code Workspace Trust ────────────────────────────────────────


def apply_vscode_restrict_workspace_trust(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable workspace trust prompts via policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeWorkspaceTrust")
    SESSION.set_dword(_VSCODE_POLICY, "security.workspace.trust.enabled", 0)


def remove_vscode_restrict_workspace_trust(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "security.workspace.trust.enabled")


def detect_vscode_restrict_workspace_trust() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "security.workspace.trust.enabled") == 0


# ── Disable VS Code GPU Acceleration ────────────────────────────────────────


def apply_vscode_disable_gpu(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VS Code: disable GPU/hardware acceleration via policy")
    SESSION.backup(_VSCODE_KEYS, "VSCodeGPU")
    SESSION.set_dword(_VSCODE_POLICY, "disable-hardware-acceleration", 1)


def remove_vscode_disable_gpu(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "disable-hardware-acceleration")


def detect_vscode_disable_gpu() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "disable-hardware-acceleration") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
        tags=["vscode", "developer", "telemetry", "privacy"],
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
        tags=["vscode", "developer", "update"],
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
        tags=["vscode", "developer", "extensions", "update"],
    ),
    TweakDef(
        id="disable-vscode-experiments",
        label="Disable VS Code A/B Experiments",
        category="VS Code",
        apply_fn=apply_disable_vscode_experiments,
        remove_fn=remove_disable_vscode_experiments,
        detect_fn=detect_disable_vscode_experiments,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description="Disables VS Code experiment framework and feature flighting.",
        tags=["vscode", "developer", "experiments"],
    ),
    TweakDef(
        id="disable-vscode-settings-sync",
        label="Disable VS Code Settings Sync",
        category="VS Code",
        apply_fn=apply_disable_vscode_settings_sync,
        remove_fn=remove_disable_vscode_settings_sync,
        detect_fn=detect_disable_vscode_settings_sync,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description="Disables VS Code settings sync via machine policy.",
        tags=["vscode", "developer", "sync"],
    ),
    TweakDef(
        id="disable-vscode-startup-editor",
        label="Disable VS Code Welcome Tab",
        category="VS Code",
        apply_fn=apply_disable_vscode_startup_editor,
        remove_fn=remove_disable_vscode_startup_editor,
        detect_fn=detect_disable_vscode_startup_editor,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description="Prevents VS Code from opening the Welcome tab on every launch.",
        tags=["vscode", "developer", "startup"],
    ),
    TweakDef(
        id="disable-vscode-recommendations",
        label="Disable VS Code Extension Recommendations",
        category="VS Code",
        apply_fn=apply_disable_vscode_recommendations,
        remove_fn=remove_disable_vscode_recommendations,
        detect_fn=detect_disable_vscode_recommendations,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description="Disables extension recommendation popups and banners in VS Code.",
        tags=["vscode", "developer", "recommendations"],
    ),
    TweakDef(
        id="vscode-disable-telemetry-policy",
        label="Disable VS Code Telemetry (Policy)",
        category="VS Code",
        apply_fn=apply_vscode_disable_telemetry_policy,
        remove_fn=remove_vscode_disable_telemetry_policy,
        detect_fn=detect_vscode_disable_telemetry_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Disables VS Code telemetry via the machine-level "
            "TelemetryLevel policy value."
        ),
        tags=["vscode", "telemetry", "privacy", "policy"],
    ),
    TweakDef(
        id="vscode-disable-experiments",
        label="Disable VS Code A/B Experiments",
        category="VS Code",
        apply_fn=apply_vscode_disable_experiments,
        remove_fn=remove_vscode_disable_experiments,
        detect_fn=detect_vscode_disable_experiments,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Disables VS Code A/B experiments via the machine-level "
            "EnableExperiments policy value."
        ),
        tags=["vscode", "experiments", "policy"],
    ),
    TweakDef(
        id="vscode-disable-update-notification",
        label="Disable VS Code Update Notifications",
        category="VS Code",
        apply_fn=apply_vscode_disable_update_notification,
        remove_fn=remove_vscode_disable_update_notification,
        detect_fn=detect_vscode_disable_update_notification,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Disables VS Code update notifications via the machine-level "
            "UpdateMode policy value."
        ),
        tags=["vscode", "update", "notifications", "policy"],
    ),
    TweakDef(
        id="vscode-disable-crash-reporter",
        label="Disable VS Code Crash Reporter",
        category="VS Code",
        apply_fn=apply_vscode_disable_crash_reporter,
        remove_fn=remove_vscode_disable_crash_reporter,
        detect_fn=detect_vscode_disable_crash_reporter,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Disables VS Code crash reporter via the machine-level "
            "CrashReporterEnabled policy value."
        ),
        tags=["vscode", "crash-reporter", "privacy", "policy"],
    ),
    TweakDef(
        id="vscode-disable-extension-gallery",
        label="Restrict VS Code Extension Gallery (Security)",
        category="VS Code",
        apply_fn=apply_vscode_disable_extension_gallery,
        remove_fn=remove_vscode_disable_extension_gallery,
        detect_fn=detect_vscode_disable_extension_gallery,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Blocks access to the VS Code extension marketplace by setting "
            "ExtensionGalleryServiceUrl to an empty string via policy."
        ),
        tags=["vscode", "extensions", "security", "policy"],
    ),
    TweakDef(
        id="vscode-restrict-workspace-trust",
        label="Restrict VS Code Workspace Trust",
        category="VS Code",
        apply_fn=apply_vscode_restrict_workspace_trust,
        remove_fn=remove_vscode_restrict_workspace_trust,
        detect_fn=detect_vscode_restrict_workspace_trust,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Disables VS Code Workspace Trust prompts via machine policy. "
            "All workspaces are treated as trusted. "
            "Default: Enabled. Recommended: Disabled for developer machines."
        ),
        tags=["vscode", "workspace-trust", "ux", "developer"],
    ),
    TweakDef(
        id="vscode-disable-gpu-acceleration",
        label="Disable VS Code GPU Acceleration",
        category="VS Code",
        apply_fn=apply_vscode_disable_gpu,
        remove_fn=remove_vscode_disable_gpu,
        detect_fn=detect_vscode_disable_gpu,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_VSCODE_KEYS,
        description=(
            "Disables GPU/hardware acceleration in VS Code via machine policy. "
            "Fixes rendering issues on certain graphics drivers. "
            "Default: Enabled. Recommended: Disabled if experiencing display glitches."
        ),
        tags=["vscode", "gpu", "performance", "rendering"],
    ),
]


# -- Disable VS Code Telemetry Reporting (HKCU) --------------------------------

_VSCODE_POLICY_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode"


def _apply_vscode_disable_telemetry_reporting(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("VSCode: disabling telemetry reporting (HKCU)")
    SESSION.backup([_VSCODE_POLICY_CU], "VSCodeTelemetryReporting")
    SESSION.set_dword(_VSCODE_POLICY_CU, "EnableTelemetry", 0)
    SESSION.log("VSCode: telemetry reporting disabled (HKCU)")


def _remove_vscode_disable_telemetry_reporting(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_VSCODE_POLICY_CU], "VSCodeTelemetryReporting_Remove")
    SESSION.delete_value(_VSCODE_POLICY_CU, "EnableTelemetry")


def _detect_vscode_disable_telemetry_reporting() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY_CU, "EnableTelemetry") == 0


# -- Disable VS Code Update Check (HKCU) --------------------------------------


def _apply_vscode_disable_update_check(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("VSCode: disabling update check (HKCU)")
    SESSION.backup([_VSCODE_POLICY_CU], "VSCodeUpdateCheck")
    SESSION.set_string(_VSCODE_POLICY_CU, "UpdateMode", "disabled")
    SESSION.log("VSCode: update check disabled (HKCU)")


def _remove_vscode_disable_update_check(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_VSCODE_POLICY_CU], "VSCodeUpdateCheck_Remove")
    SESSION.delete_value(_VSCODE_POLICY_CU, "UpdateMode")


def _detect_vscode_disable_update_check() -> bool:
    return SESSION.read_string(_VSCODE_POLICY_CU, "UpdateMode") == "disabled"


TWEAKS += [
    TweakDef(
        id="vscode-disable-telemetry-reporting",
        label="Disable VS Code Telemetry (User Policy)",
        category="VS Code",
        apply_fn=_apply_vscode_disable_telemetry_reporting,
        remove_fn=_remove_vscode_disable_telemetry_reporting,
        detect_fn=_detect_vscode_disable_telemetry_reporting,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VSCODE_POLICY_CU],
        description=(
            "Disables VS Code telemetry via user-level policy. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["vscode", "telemetry", "privacy", "user-policy"],
    ),
    TweakDef(
        id="vscode-disable-update-check",
        label="Disable VS Code Update Check (User Policy)",
        category="VS Code",
        apply_fn=_apply_vscode_disable_update_check,
        remove_fn=_remove_vscode_disable_update_check,
        detect_fn=_detect_vscode_disable_update_check,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VSCODE_POLICY_CU],
        description=(
            "Disables VS Code auto-update checking via user-level policy. "
            "Default: Enabled. Recommended: Disabled for stable environments."
        ),
        tags=["vscode", "update", "auto-update", "user-policy"],
    ),
]


# -- Disable VS Code Telemetry via Registry (Machine Policy) --------------------


def _apply_vsc_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VSCode: disabling telemetry via machine registry policy")
    SESSION.backup([_VSCODE_POLICY], "VscDisableTelemetry")
    SESSION.set_dword(_VSCODE_POLICY, "EnableTelemetry", 0)
    SESSION.set_string(_VSCODE_POLICY, "telemetry.telemetryLevel", "off")
    SESSION.log("VSCode: telemetry disabled via machine policy")


def _remove_vsc_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "EnableTelemetry")
    SESSION.delete_value(_VSCODE_POLICY, "telemetry.telemetryLevel")


def _detect_vsc_disable_telemetry() -> bool:
    return SESSION.read_dword(_VSCODE_POLICY, "EnableTelemetry") == 0


# -- Disable VS Code Update Notifications (Machine Policy) ----------------------


def _apply_vsc_disable_update_notif(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VSCode: disabling update notifications via machine policy")
    SESSION.backup([_VSCODE_POLICY], "VscDisableUpdateNotif")
    SESSION.set_string(_VSCODE_POLICY, "update.mode", "none")
    SESSION.set_dword(_VSCODE_POLICY, "update.showReleaseNotes", 0)
    SESSION.log("VSCode: update notifications disabled")


def _remove_vsc_disable_update_notif(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "update.mode")
    SESSION.delete_value(_VSCODE_POLICY, "update.showReleaseNotes")


def _detect_vsc_disable_update_notif() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "update.mode") == "none"


# -- Set VS Code GPU Acceleration (Machine Policy) ------------------------------


def _apply_vsc_set_gpu_accel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("VSCode: enabling GPU acceleration via machine policy")
    SESSION.backup([_VSCODE_POLICY], "VscGPUAccel")
    SESSION.set_string(_VSCODE_POLICY, "gpu.acceleration", "on")
    SESSION.log("VSCode: GPU acceleration set to on")


def _remove_vsc_set_gpu_accel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_VSCODE_POLICY, "gpu.acceleration")


def _detect_vsc_set_gpu_accel() -> bool:
    return SESSION.read_string(_VSCODE_POLICY, "gpu.acceleration") == "on"


TWEAKS += [
    TweakDef(
        id="vsc-disable-telemetry",
        label="Disable VS Code Telemetry (Machine Policy)",
        category="VS Code",
        apply_fn=_apply_vsc_disable_telemetry,
        remove_fn=_remove_vsc_disable_telemetry,
        detect_fn=_detect_vsc_disable_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VSCODE_POLICY],
        description=(
            "Disables VS Code telemetry via HKLM machine-level policy. "
            "Applies to all users on the machine. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["vscode", "telemetry", "privacy", "machine-policy"],
    ),
    TweakDef(
        id="vsc-disable-update-notif",
        label="Disable VS Code Update Notifications (Machine Policy)",
        category="VS Code",
        apply_fn=_apply_vsc_disable_update_notif,
        remove_fn=_remove_vsc_disable_update_notif,
        detect_fn=_detect_vsc_disable_update_notif,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VSCODE_POLICY],
        description=(
            "Disables VS Code update notifications and release notes via machine policy. "
            "Default: Enabled. Recommended: Disabled for stable environments."
        ),
        tags=["vscode", "update", "notifications", "machine-policy"],
    ),
    TweakDef(
        id="vsc-set-gpu-accel",
        label="Set VS Code GPU Acceleration (Machine Policy)",
        category="VS Code",
        apply_fn=_apply_vsc_set_gpu_accel,
        remove_fn=_remove_vsc_set_gpu_accel,
        detect_fn=_detect_vsc_set_gpu_accel,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_VSCODE_POLICY],
        description=(
            "Enables GPU acceleration for VS Code via machine-level policy. "
            "Improves rendering performance. Default: Auto. Recommended: On."
        ),
        tags=["vscode", "gpu", "acceleration", "performance", "machine-policy"],
    ),
]
