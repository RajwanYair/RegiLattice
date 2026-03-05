"""Telemetry Advanced tweaks.

Deep telemetry controls: Connected User Experience, DiagTrack,
app telemetry, handwriting reporting, advertising ID, CEIP,
feedback frequency, and diagnostic data settings.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_DATA_COLLECTION = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"
)
_DATA_COLLECTION_CU = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\DataCollection"
)
_DIAGTRACK_SVC = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack"
)
_APP_TELEMETRY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"
)
_HANDWRITING = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\TabletPC"
)
_HANDWRITING_CU = (
    r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows"
    r"\TabletPC"
)
_ADVERTISING_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\AdvertisingInfo"
)
_ADVERTISING_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\AdvertisingInfo"
)
_FEEDBACK = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"
)
_CEIP_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SQMClient\Windows"
)
_INVENTORY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"
)
_USAGE_STATS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)
_INPUT_TELEMETRY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC"
)
_DIAG_LOG = (
    r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"
    r"\WMI\Autologger\AutoLogger-Diagtrack-Listener"
)
_TYPE_INSIGHTS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\CPSS\Store\InkingAndTypingPersonalization"
)


# ── Set Telemetry to Security Only (0) ───────────────────────────────────────


def _apply_telemetry_security(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: set diagnostic data to security-only (Enterprise)")
    SESSION.backup([_DATA_COLLECTION, _DATA_COLLECTION_CU], "TelemetrySecurity")
    SESSION.set_dword(_DATA_COLLECTION, "AllowTelemetry", 0)
    SESSION.set_dword(_DATA_COLLECTION_CU, "AllowTelemetry", 0)


def _remove_telemetry_security(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DATA_COLLECTION, "AllowTelemetry", 3)
    SESSION.set_dword(_DATA_COLLECTION_CU, "AllowTelemetry", 3)


def _detect_telemetry_security() -> bool:
    return SESSION.read_dword(_DATA_COLLECTION, "AllowTelemetry") == 0


# ── Disable Diagnostic Data Opt-In Settings UI ──────────────────────────────


def _apply_disable_diag_optin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: block diagnostic data opt-in settings change")
    SESSION.backup([_DATA_COLLECTION], "DiagOptIn")
    SESSION.set_dword(_DATA_COLLECTION, "DisableDiagnosticDataViewer", 1)
    SESSION.set_dword(_DATA_COLLECTION, "DisableOneSettingsSyncDiag", 1)


def _remove_disable_diag_optin(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DATA_COLLECTION, "DisableDiagnosticDataViewer")
    SESSION.delete_value(_DATA_COLLECTION, "DisableOneSettingsSyncDiag")


def _detect_disable_diag_optin() -> bool:
    return SESSION.read_dword(_DATA_COLLECTION, "DisableDiagnosticDataViewer") == 1


# ── Disable App Telemetry (Steps Recorder / Inventory) ───────────────────────


def _apply_disable_app_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: disable app telemetry (Steps Recorder + Inventory)")
    SESSION.backup([_APP_TELEMETRY], "AppTelemetry")
    SESSION.set_dword(_APP_TELEMETRY, "DisableUAR", 1)  # Steps Recorder
    SESSION.set_dword(_APP_TELEMETRY, "DisableInventory", 1)


def _remove_disable_app_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_APP_TELEMETRY, "DisableUAR")
    SESSION.delete_value(_APP_TELEMETRY, "DisableInventory")


def _detect_disable_app_telemetry() -> bool:
    return SESSION.read_dword(_APP_TELEMETRY, "DisableInventory") == 1


# ── Disable Handwriting Data Reporting ───────────────────────────────────────


def _apply_disable_handwriting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: disable handwriting error reporting")
    SESSION.backup([_HANDWRITING], "Handwriting")
    SESSION.set_dword(_HANDWRITING, "PreventHandwritingDataSharing", 1)
    SESSION.set_dword(_HANDWRITING, "PreventHandwritingErrorReports", 1)


def _remove_disable_handwriting(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_HANDWRITING, "PreventHandwritingDataSharing")
    SESSION.delete_value(_HANDWRITING, "PreventHandwritingErrorReports")


def _detect_disable_handwriting() -> bool:
    return SESSION.read_dword(_HANDWRITING, "PreventHandwritingDataSharing") == 1


# ── Disable Advertising ID ──────────────────────────────────────────────────


def _apply_disable_adv_id(*, require_admin: bool = False) -> None:
    SESSION.log("Telemetry: disable advertising ID")
    SESSION.backup([_ADVERTISING_CU, _ADVERTISING_POLICY], "AdvID")
    SESSION.set_dword(_ADVERTISING_CU, "Enabled", 0)
    SESSION.set_dword(_ADVERTISING_POLICY, "DisabledByGroupPolicy", 1)


def _remove_disable_adv_id(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_ADVERTISING_CU, "Enabled", 1)
    SESSION.delete_value(_ADVERTISING_POLICY, "DisabledByGroupPolicy")


def _detect_disable_adv_id() -> bool:
    return SESSION.read_dword(_ADVERTISING_CU, "Enabled") == 0


# ── Disable Feedback Notifications ───────────────────────────────────────────


def _apply_disable_feedback(*, require_admin: bool = False) -> None:
    SESSION.log("Telemetry: disable feedback notifications (frequency=0)")
    SESSION.backup([_FEEDBACK], "Feedback")
    SESSION.set_dword(_FEEDBACK, "NumberOfSIUFInPeriod", 0)


def _remove_disable_feedback(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_FEEDBACK, "NumberOfSIUFInPeriod")


def _detect_disable_feedback() -> bool:
    return SESSION.read_dword(_FEEDBACK, "NumberOfSIUFInPeriod") == 0


# ── Disable Input Telemetry (Typing/Inking) ─────────────────────────────────


def _apply_disable_input_telemetry(*, require_admin: bool = False) -> None:
    SESSION.log("Telemetry: disable typing/inking data collection")
    SESSION.backup([_INPUT_TELEMETRY], "InputTelemetry")
    SESSION.set_dword(_INPUT_TELEMETRY, "Enabled", 0)


def _remove_disable_input_telemetry(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_INPUT_TELEMETRY, "Enabled", 1)


def _detect_disable_input_telemetry() -> bool:
    return SESSION.read_dword(_INPUT_TELEMETRY, "Enabled") == 0


# ── Disable Inking & Typing Personalization ──────────────────────────────────


def _apply_disable_type_insights(*, require_admin: bool = False) -> None:
    SESSION.log("Telemetry: disable inking and typing personalization")
    SESSION.backup([_TYPE_INSIGHTS], "TypeInsights")
    SESSION.set_dword(_TYPE_INSIGHTS, "Value", 0)


def _remove_disable_type_insights(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TYPE_INSIGHTS, "Value", 1)


def _detect_disable_type_insights() -> bool:
    return SESSION.read_dword(_TYPE_INSIGHTS, "Value") == 0


# ── Disable DiagTrack Autologger Boot Trace ──────────────────────────────────


def _apply_disable_diag_autologger(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: disable DiagTrack autologger ETW trace")
    SESSION.backup([_DIAG_LOG], "DiagAutoLogger")
    SESSION.set_dword(_DIAG_LOG, "Start", 0)


def _remove_disable_diag_autologger(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DIAG_LOG, "Start", 1)


def _detect_disable_diag_autologger() -> bool:
    return SESSION.read_dword(_DIAG_LOG, "Start") == 0


# ── Disable Tailored Experiences ─────────────────────────────────────────────

_TAILORED = (
    r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent"
)


def _apply_disable_tailored(*, require_admin: bool = False) -> None:
    SESSION.log("Telemetry: disable tailored experiences with diagnostic data")
    SESSION.backup([_TAILORED], "TailoredExperiences")
    SESSION.set_dword(_TAILORED, "DisableTailoredExperiencesWithDiagnosticData", 1)


def _remove_disable_tailored(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_TAILORED, "DisableTailoredExperiencesWithDiagnosticData")


def _detect_disable_tailored() -> bool:
    return SESSION.read_dword(_TAILORED, "DisableTailoredExperiencesWithDiagnosticData") == 1


# ── Disable Do-Not-Track Override ────────────────────────────────────────────


def _apply_disable_usage_stats(*, require_admin: bool = False) -> None:
    SESSION.log("Telemetry: disable Start menu app usage tracking")
    SESSION.backup([_USAGE_STATS], "UsageStats")
    SESSION.set_dword(_USAGE_STATS, "Start_TrackProgs", 0)


def _remove_disable_usage_stats(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_USAGE_STATS, "Start_TrackProgs", 1)


def _detect_disable_usage_stats() -> bool:
    return SESSION.read_dword(_USAGE_STATS, "Start_TrackProgs") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="telem-security-only",
        label="Set Telemetry to Security Only (0)",
        category="Telemetry Advanced",
        apply_fn=_apply_telemetry_security,
        remove_fn=_remove_telemetry_security,
        detect_fn=_detect_telemetry_security,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DATA_COLLECTION, _DATA_COLLECTION_CU],
        description=(
            "Sets AllowTelemetry=0 (Security level, Enterprise/Education only). "
            "On Home/Pro this sets Required level minimum. "
            "Default: 3 (Full). Recommended: 0."
        ),
        tags=["telemetry", "privacy", "security", "diagnostic"],
    ),
    TweakDef(
        id="telem-disable-diag-optin",
        label="Block Diagnostic Data Settings Changes",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_diag_optin,
        remove_fn=_remove_disable_diag_optin,
        detect_fn=_detect_disable_diag_optin,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DATA_COLLECTION],
        description=(
            "Disables the diagnostic data viewer and prevents users "
            "from changing opt-in level via Settings. "
            "Default: allowed. Recommended: 1 (blocked)."
        ),
        tags=["telemetry", "diagnostic", "settings", "policy"],
    ),
    TweakDef(
        id="telem-disable-app-telemetry",
        label="Disable App Telemetry (Steps Recorder + Inventory)",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_app_telemetry,
        remove_fn=_remove_disable_app_telemetry,
        detect_fn=_detect_disable_app_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_APP_TELEMETRY],
        description=(
            "Disables Steps Recorder (UAR) and Application Inventory "
            "collection. Reduces background telemetry. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["telemetry", "app", "steps-recorder", "inventory"],
    ),
    TweakDef(
        id="telem-disable-handwriting",
        label="Disable Handwriting Data Sharing",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_handwriting,
        remove_fn=_remove_disable_handwriting,
        detect_fn=_detect_disable_handwriting,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HANDWRITING],
        description=(
            "Prevents handwriting recognition data and error reports "
            "from being sent to Microsoft. "
            "Default: allowed. Recommended: 1 (blocked)."
        ),
        tags=["telemetry", "handwriting", "privacy", "tablet"],
    ),
    TweakDef(
        id="telem-disable-advertising-id",
        label="Disable Advertising ID",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_adv_id,
        remove_fn=_remove_disable_adv_id,
        detect_fn=_detect_disable_adv_id,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADVERTISING_CU, _ADVERTISING_POLICY],
        description=(
            "Disables the per-user advertising ID used for cross-app "
            "ad targeting. Sets both user and policy values. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["telemetry", "advertising", "privacy", "ads"],
    ),
    TweakDef(
        id="telem-disable-feedback",
        label="Disable Feedback Notifications",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_feedback,
        remove_fn=_remove_disable_feedback,
        detect_fn=_detect_disable_feedback,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FEEDBACK],
        description=(
            "Sets feedback frequency to 0 (never). Stops 'Rate Windows' "
            "and similar feedback prompts. "
            "Default: automatic. Recommended: 0 (never)."
        ),
        tags=["telemetry", "feedback", "notifications", "privacy"],
    ),
    TweakDef(
        id="telem-disable-input-telemetry",
        label="Disable Typing/Inking Telemetry",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_input_telemetry,
        remove_fn=_remove_disable_input_telemetry,
        detect_fn=_detect_disable_input_telemetry,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_INPUT_TELEMETRY],
        description=(
            "Disables collection of typing and inking data for "
            "improving language recognition. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["telemetry", "typing", "inking", "input", "privacy"],
    ),
    TweakDef(
        id="telem-disable-type-personalization",
        label="Disable Inking & Typing Personalization",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_type_insights,
        remove_fn=_remove_disable_type_insights,
        detect_fn=_detect_disable_type_insights,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TYPE_INSIGHTS],
        description=(
            "Disables inking and typing personalization that learns "
            "from your writing patterns. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["telemetry", "inking", "typing", "personalization"],
    ),
    TweakDef(
        id="telem-disable-diagtrack-autologger",
        label="Disable DiagTrack ETW Autologger",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_diag_autologger,
        remove_fn=_remove_disable_diag_autologger,
        detect_fn=_detect_disable_diag_autologger,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DIAG_LOG],
        description=(
            "Disables the DiagTrack ETW autologger that starts at boot. "
            "Stops kernel-level telemetry trace collection. "
            "Default: 1 (enabled). Recommended: 0 (disabled)."
        ),
        tags=["telemetry", "diagtrack", "etw", "boot", "trace"],
    ),
    TweakDef(
        id="telem-disable-tailored-experiences",
        label="Disable Tailored Experiences",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_tailored,
        remove_fn=_remove_disable_tailored,
        detect_fn=_detect_disable_tailored,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TAILORED],
        description=(
            "Prevents Windows from using diagnostic data to provide "
            "personalized tips, ads, and recommendations. "
            "Default: allowed. Recommended: 1 (disabled)."
        ),
        tags=["telemetry", "tailored", "suggestions", "privacy"],
    ),
    TweakDef(
        id="telem-disable-usage-tracking",
        label="Disable Start Menu Usage Tracking",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_usage_stats,
        remove_fn=_remove_disable_usage_stats,
        detect_fn=_detect_disable_usage_stats,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_USAGE_STATS],
        description=(
            "Disables app launch tracking used for Start menu 'Most Used' "
            "list and personalization. "
            "Default: 1 (track). Recommended: 0 (disabled)."
        ),
        tags=["telemetry", "start-menu", "usage", "tracking", "privacy"],
    ),
]


# -- 12. Disable Windows Error Reporting ─────────────────────────────────────

_TELEM_WER = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"


def _apply_disable_wer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_TELEM_WER], "DisableWER")
    SESSION.set_dword(_TELEM_WER, "Disabled", 1)


def _remove_disable_wer(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TELEM_WER, "Disabled", 0)


def _detect_disable_wer() -> bool:
    return SESSION.read_dword(_TELEM_WER, "Disabled") == 1


# -- 13. Disable Inventory Collector ─────────────────────────────────────────


def _apply_disable_inventory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_APP_TELEMETRY], "InventoryCollector")
    SESSION.set_dword(_APP_TELEMETRY, "DisableInventory", 1)


def _remove_disable_inventory(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_APP_TELEMETRY, "DisableInventory", 0)


def _detect_disable_inventory() -> bool:
    return SESSION.read_dword(_APP_TELEMETRY, "DisableInventory") == 1


TWEAKS += [
    TweakDef(
        id="telem-disable-win-error-reporting",
        label="Disable Windows Error Reporting",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_wer,
        remove_fn=_remove_disable_wer,
        detect_fn=_detect_disable_wer,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TELEM_WER],
        description=(
            "Disables Windows Error Reporting (WER). Prevents sending crash data to Microsoft. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["telemetry", "wer", "error-reporting", "crash", "privacy"],
    ),
    TweakDef(
        id="telem-disable-inventory-collector",
        label="Disable Inventory Collector",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_inventory,
        remove_fn=_remove_disable_inventory,
        detect_fn=_detect_disable_inventory,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_APP_TELEMETRY],
        description=(
            "Disables the Inventory Collector that sends application/driver data to Microsoft. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["telemetry", "inventory", "collector", "appcompat"],
    ),
]


# -- Disable Connected User Experiences -----------------------------------------

_CONNECTED_UX = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\CurrentVersion\PushNotifications"
)


def _apply_disable_connected_ux(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: disabling Connected User Experiences")
    SESSION.backup([_DATA_COLLECTION, _CONNECTED_UX], "ConnectedUX")
    SESSION.set_dword(_DATA_COLLECTION, "DisableEnterpriseAuthProxy", 1)
    SESSION.set_dword(_CONNECTED_UX, "NoCloudApplicationNotification", 1)
    SESSION.log("Telemetry: Connected User Experiences disabled")


def _remove_disable_connected_ux(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DATA_COLLECTION, "DisableEnterpriseAuthProxy")
    SESSION.delete_value(_CONNECTED_UX, "NoCloudApplicationNotification")


def _detect_disable_connected_ux() -> bool:
    return SESSION.read_dword(_DATA_COLLECTION, "DisableEnterpriseAuthProxy") == 1


# -- Set Telemetry Max Cache Size -----------------------------------------------

_DIAGTRACK_CONF = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows"
    r"\DataCollection"
)


def _apply_set_telemetry_max_size(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: setting max telemetry cache to 1 MB")
    SESSION.backup([_DIAGTRACK_CONF], "TelemMaxSize")
    SESSION.set_dword(_DIAGTRACK_CONF, "LimitDumpCollection", 1)
    SESSION.set_dword(_DIAGTRACK_CONF, "LimitDiagnosticLogCollection", 0)
    SESSION.log("Telemetry: max cache size set")


def _remove_set_telemetry_max_size(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DIAGTRACK_CONF, "LimitDumpCollection")
    SESSION.delete_value(_DIAGTRACK_CONF, "LimitDiagnosticLogCollection")


def _detect_set_telemetry_max_size() -> bool:
    return SESSION.read_dword(_DIAGTRACK_CONF, "LimitDumpCollection") == 1


# -- Disable Diagnostic Log Collection ------------------------------------------


def _apply_disable_diag_log(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Telemetry: disabling diagnostic log collection")
    SESSION.backup([_DIAGTRACK_CONF], "DiagLogCollection")
    SESSION.set_dword(_DIAGTRACK_CONF, "LimitDiagnosticLogCollection", 1)
    SESSION.log("Telemetry: diagnostic log collection disabled")


def _remove_disable_diag_log(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_DIAGTRACK_CONF, "LimitDiagnosticLogCollection", 0)


def _detect_disable_diag_log() -> bool:
    return SESSION.read_dword(_DIAGTRACK_CONF, "LimitDiagnosticLogCollection") == 1


TWEAKS += [
    TweakDef(
        id="telemetry-disable-connected-user",
        label="Disable Connected User Experiences",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_connected_ux,
        remove_fn=_remove_disable_connected_ux,
        detect_fn=_detect_disable_connected_ux,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DATA_COLLECTION, _CONNECTED_UX],
        description=(
            "Disables Connected User Experiences and Telemetry proxy. "
            "Prevents cloud-based notifications and data sync. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["telemetry", "connected-ux", "push", "privacy"],
    ),
    TweakDef(
        id="telemetry-set-max-size",
        label="Limit Telemetry Cache / Dump Collection",
        category="Telemetry Advanced",
        apply_fn=_apply_set_telemetry_max_size,
        remove_fn=_remove_set_telemetry_max_size,
        detect_fn=_detect_set_telemetry_max_size,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DIAGTRACK_CONF],
        description=(
            "Limits telemetry dump collection to reduce disk usage and data sent to Microsoft. "
            "Default: Unlimited. Recommended: Limited."
        ),
        tags=["telemetry", "cache", "dump", "size", "limit"],
    ),
    TweakDef(
        id="telemetry-disable-diagnostic-log",
        label="Disable Diagnostic Log Collection",
        category="Telemetry Advanced",
        apply_fn=_apply_disable_diag_log,
        remove_fn=_remove_disable_diag_log,
        detect_fn=_detect_disable_diag_log,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DIAGTRACK_CONF],
        description=(
            "Disables diagnostic log collection via LimitDiagnosticLogCollection policy. "
            "Reduces telemetry data stored locally. Default: Enabled. Recommended: Disabled."
        ),
        tags=["telemetry", "diagnostic", "log", "collection", "privacy"],
    ),
]
