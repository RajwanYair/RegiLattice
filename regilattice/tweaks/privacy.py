"""Privacy tweaks — Telemetry, Cortana, Activity History, Location, Advertising ID."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Telemetry ────────────────────────────────────────────────────────────────

_TELEMETRY_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"
)
_TELEMETRY_DATA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\DataCollection"
)
_TELEMETRY_KEYS = [_TELEMETRY_POLICY, _TELEMETRY_DATA]


def _apply_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable Windows telemetry")
    SESSION.backup(_TELEMETRY_KEYS, "Telemetry")
    SESSION.set_dword(_TELEMETRY_POLICY, "AllowTelemetry", 0)
    SESSION.set_dword(_TELEMETRY_DATA, "AllowTelemetry", 0)
    SESSION.set_dword(_TELEMETRY_POLICY, "DoNotShowFeedbackNotifications", 1)


def _remove_disable_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup(_TELEMETRY_KEYS, "Telemetry_Remove")
    SESSION.set_dword(_TELEMETRY_POLICY, "AllowTelemetry", 3)
    SESSION.set_dword(_TELEMETRY_DATA, "AllowTelemetry", 3)
    SESSION.delete_value(_TELEMETRY_POLICY, "DoNotShowFeedbackNotifications")


def _detect_disable_telemetry() -> bool:
    return SESSION.read_dword(_TELEMETRY_POLICY, "AllowTelemetry") == 0


# ── Cortana ──────────────────────────────────────────────────────────────────

_CORTANA_KEY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"
)


def _apply_disable_cortana(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable Cortana")
    SESSION.backup([_CORTANA_KEY], "Cortana")
    SESSION.set_dword(_CORTANA_KEY, "AllowCortana", 0)
    SESSION.set_dword(_CORTANA_KEY, "AllowSearchToUseLocation", 0)
    SESSION.set_dword(_CORTANA_KEY, "DisableWebSearch", 1)
    SESSION.set_dword(_CORTANA_KEY, "ConnectedSearchUseWeb", 0)


def _remove_disable_cortana(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_CORTANA_KEY], "Cortana_Remove")
    SESSION.delete_value(_CORTANA_KEY, "AllowCortana")
    SESSION.delete_value(_CORTANA_KEY, "AllowSearchToUseLocation")
    SESSION.delete_value(_CORTANA_KEY, "DisableWebSearch")
    SESSION.delete_value(_CORTANA_KEY, "ConnectedSearchUseWeb")


def _detect_disable_cortana() -> bool:
    return SESSION.read_dword(_CORTANA_KEY, "AllowCortana") == 0


# ── Disable Activity History ────────────────────────────────────────────────

_ACTIVITY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_disable_activity(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable Activity History / Timeline")
    SESSION.backup([_ACTIVITY], "ActivityHistory")
    SESSION.set_dword(_ACTIVITY, "EnableActivityFeed", 0)
    SESSION.set_dword(_ACTIVITY, "PublishUserActivities", 0)
    SESSION.set_dword(_ACTIVITY, "UploadUserActivities", 0)


def _remove_disable_activity(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ACTIVITY, "EnableActivityFeed")
    SESSION.delete_value(_ACTIVITY, "PublishUserActivities")
    SESSION.delete_value(_ACTIVITY, "UploadUserActivities")


def _detect_disable_activity() -> bool:
    return SESSION.read_dword(_ACTIVITY, "EnableActivityFeed") == 0


# ── Disable Location Tracking ───────────────────────────────────────────────

_LOCATION = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\CapabilityAccessManager\ConsentStore\location"
)
_LOCATION_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors"
)


def _apply_disable_location(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable location tracking")
    SESSION.backup([_LOCATION, _LOCATION_POLICY], "Location")
    SESSION.set_string(_LOCATION, "Value", "Deny")
    SESSION.set_dword(_LOCATION_POLICY, "DisableLocation", 1)


def _remove_disable_location(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_LOCATION, "Value", "Allow")
    SESSION.delete_value(_LOCATION_POLICY, "DisableLocation")


def _detect_disable_location() -> bool:
    return SESSION.read_string(_LOCATION, "Value") == "Deny"


# ── Disable Advertising ID ──────────────────────────────────────────────────

_ADID = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\AdvertisingInfo"
)
_ADID_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvertisingInfo"
)


def _apply_disable_adid(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable advertising ID")
    SESSION.backup([_ADID, _ADID_POLICY], "AdvertisingID")
    SESSION.set_dword(_ADID, "Enabled", 0)
    SESSION.set_dword(_ADID_POLICY, "DisabledByGroupPolicy", 1)


def _remove_disable_adid(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ADID, "Enabled", 1)
    SESSION.delete_value(_ADID_POLICY, "DisabledByGroupPolicy")


def _detect_disable_adid() -> bool:
    return SESSION.read_dword(_ADID, "Enabled") == 0


# ── Disable Camera access ───────────────────────────────────────────────────

_CAMERA = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam"
)


def _apply_disable_camera(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable camera access for apps")
    SESSION.backup([_CAMERA], "CameraAccess")
    SESSION.set_string(_CAMERA, "Value", "Deny")


def _remove_disable_camera(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_CAMERA, "Value", "Allow")


def _detect_disable_camera() -> bool:
    return SESSION.read_string(_CAMERA, "Value") == "Deny"


# ── Disable Microphone access ───────────────────────────────────────────────

_MIC = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone"
)


def _apply_disable_mic(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable microphone access for apps")
    SESSION.backup([_MIC], "MicAccess")
    SESSION.set_string(_MIC, "Value", "Deny")


def _remove_disable_mic(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_MIC, "Value", "Allow")


def _detect_disable_mic() -> bool:
    return SESSION.read_string(_MIC, "Value") == "Deny"


# ── Disable Diagnostic Data Viewer ───────────────────────────────────────────

_DIAG = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Diagnostics\DiagTrack"
)


def _apply_disable_diagtrack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable DiagTrack (Connected User Experiences)")
    SESSION.backup([_DIAG], "DiagTrack")
    SESSION.set_dword(_DIAG, "ShowedToastAtLevel", 1)


def _remove_disable_diagtrack(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DIAG, "ShowedToastAtLevel")


def _detect_disable_diagtrack() -> bool:
    return SESSION.read_dword(_DIAG, "ShowedToastAtLevel") == 1


# ── Disable Online Speech Recognition ────────────────────────────────────────

_SPEECH = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"
)


def _apply_disable_speech(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable online speech recognition")
    SESSION.backup([_SPEECH], "Speech")
    SESSION.set_dword(_SPEECH, "HasAccepted", 0)


def _remove_disable_speech(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SPEECH, "HasAccepted", 1)


def _detect_disable_speech() -> bool:
    return SESSION.read_dword(_SPEECH, "HasAccepted") == 0


# ── Disable Inking & Typing Personalization ──────────────────────────────────

_INK = (
    r"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization"
)
_INK_TRAINED = (
    r"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore"
)


def _apply_disable_inking(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable inking & typing personalization")
    SESSION.backup([_INK, _INK_TRAINED], "Inking")
    SESSION.set_dword(_INK, "RestrictImplicitInkCollection", 1)
    SESSION.set_dword(_INK, "RestrictImplicitTextCollection", 1)
    SESSION.set_dword(_INK_TRAINED, "HarvestContacts", 0)


def _remove_disable_inking(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_INK, "RestrictImplicitInkCollection", 0)
    SESSION.set_dword(_INK, "RestrictImplicitTextCollection", 0)
    SESSION.set_dword(_INK_TRAINED, "HarvestContacts", 1)


def _detect_disable_inking() -> bool:
    return SESSION.read_dword(_INK, "RestrictImplicitInkCollection") == 1


# ── Disable Clipboard History (Privacy) ──────────────────────────────────────

_CLIPBOARD_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_priv_clipboard_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable clipboard history")
    SESSION.backup([_CLIPBOARD_POLICY], "PrivClipboardHistory")
    SESSION.set_dword(_CLIPBOARD_POLICY, "AllowClipboardHistory", 0)


def _remove_priv_clipboard_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLIPBOARD_POLICY, "AllowClipboardHistory")


def _detect_priv_clipboard_history() -> bool:
    return SESSION.read_dword(_CLIPBOARD_POLICY, "AllowClipboardHistory") == 0


# ── Disable Online Speech Recognition (Privacy) ─────────────────────────────


def _apply_priv_online_speech(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable online speech recognition (privacy)")
    SESSION.backup([_SPEECH], "PrivOnlineSpeech")
    SESSION.set_dword(_SPEECH, "HasAccepted", 0)


def _remove_priv_online_speech(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SPEECH, "HasAccepted", 1)


def _detect_priv_online_speech() -> bool:
    return SESSION.read_dword(_SPEECH, "HasAccepted") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-telemetry",
        label="Disable Windows Telemetry",
        category="Privacy",
        apply_fn=_apply_disable_telemetry,
        remove_fn=_remove_disable_telemetry,
        detect_fn=_detect_disable_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_TELEMETRY_KEYS,
        description="Disables Windows telemetry and feedback notifications.",
        tags=["privacy", "telemetry", "microsoft"],
    ),
    TweakDef(
        id="disable-cortana",
        label="Disable Cortana",
        category="Privacy",
        apply_fn=_apply_disable_cortana,
        remove_fn=_remove_disable_cortana,
        detect_fn=_detect_disable_cortana,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_CORTANA_KEY],
        description="Disables Cortana and web search integration.",
        tags=["privacy", "cortana", "search"],
    ),
    TweakDef(
        id="disable-activity-history",
        label="Disable Activity History",
        category="Privacy",
        apply_fn=_apply_disable_activity,
        remove_fn=_remove_disable_activity,
        detect_fn=_detect_disable_activity,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ACTIVITY],
        description=(
            "Disables Windows Activity History (Timeline), preventing "
            "activity data collection and cloud sync."
        ),
        tags=["privacy", "activity", "timeline"],
    ),
    TweakDef(
        id="disable-location",
        label="Disable Location Tracking",
        category="Privacy",
        apply_fn=_apply_disable_location,
        remove_fn=_remove_disable_location,
        detect_fn=_detect_disable_location,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_LOCATION, _LOCATION_POLICY],
        description="Disables location tracking for all apps and Windows services.",
        tags=["privacy", "location", "tracking"],
    ),
    TweakDef(
        id="disable-advertising-id",
        label="Disable Advertising ID",
        category="Privacy",
        apply_fn=_apply_disable_adid,
        remove_fn=_remove_disable_adid,
        detect_fn=_detect_disable_adid,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ADID, _ADID_POLICY],
        description=(
            "Disables the Windows advertising ID used for cross-app "
            "ad targeting."
        ),
        tags=["privacy", "advertising", "tracking"],
    ),
    TweakDef(
        id="disable-camera-access",
        label="Deny Camera Access (Apps)",
        category="Privacy",
        apply_fn=_apply_disable_camera,
        remove_fn=_remove_disable_camera,
        detect_fn=_detect_disable_camera,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CAMERA],
        description="Denies camera access for UWP/Store apps by default.",
        tags=["privacy", "camera", "hardware"],
    ),
    TweakDef(
        id="disable-microphone-access",
        label="Deny Microphone Access (Apps)",
        category="Privacy",
        apply_fn=_apply_disable_mic,
        remove_fn=_remove_disable_mic,
        detect_fn=_detect_disable_mic,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_MIC],
        description="Denies microphone access for UWP/Store apps by default.",
        tags=["privacy", "microphone", "hardware"],
    ),
    TweakDef(
        id="disable-diagtrack",
        label="Disable DiagTrack (CEIP)",
        category="Privacy",
        apply_fn=_apply_disable_diagtrack,
        remove_fn=_remove_disable_diagtrack,
        detect_fn=_detect_disable_diagtrack,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_DIAG],
        description="Disables Connected User Experiences and Telemetry (DiagTrack).",
        tags=["privacy", "telemetry", "diagtrack"],
    ),
    TweakDef(
        id="disable-online-speech",
        label="Disable Online Speech Recognition",
        category="Privacy",
        apply_fn=_apply_disable_speech,
        remove_fn=_remove_disable_speech,
        detect_fn=_detect_disable_speech,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SPEECH],
        description="Stops sending voice data to Microsoft for online speech recognition.",
        tags=["privacy", "speech", "voice"],
    ),
    TweakDef(
        id="disable-inking-personalization",
        label="Disable Inking & Typing Personalization",
        category="Privacy",
        apply_fn=_apply_disable_inking,
        remove_fn=_remove_disable_inking,
        detect_fn=_detect_disable_inking,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_INK, _INK_TRAINED],
        description="Prevents Windows from collecting typing/inking data for personalization.",
        tags=["privacy", "inking", "typing"],
    ),
    TweakDef(
        id="privacy-disable-clipboard-history",
        label="Disable Clipboard History",
        category="Privacy",
        apply_fn=_apply_priv_clipboard_history,
        remove_fn=_remove_priv_clipboard_history,
        detect_fn=_detect_priv_clipboard_history,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLIPBOARD_POLICY],
        description=(
            "Disables Windows clipboard history (Win+V). Prevents sensitive copied data from being stored. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["privacy", "clipboard", "history"],
    ),
    TweakDef(
        id="privacy-disable-online-speech",
        label="Disable Online Speech Recognition",
        category="Privacy",
        apply_fn=_apply_priv_online_speech,
        remove_fn=_remove_priv_online_speech,
        detect_fn=_detect_priv_online_speech,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SPEECH],
        description=(
            "Disables online speech recognition which sends voice data to Microsoft servers. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["privacy", "speech", "recognition", "telemetry"],
    ),
]


# ── Disable Activity History (policy) ────────────────────────────────────────


def _apply_priv_activity_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable activity history via policy")
    SESSION.backup([_ACTIVITY], "PrivActivityHistory")
    SESSION.set_dword(_ACTIVITY, "EnableActivityFeed", 0)
    SESSION.set_dword(_ACTIVITY, "PublishUserActivities", 0)


def _remove_priv_activity_history(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ACTIVITY, "EnableActivityFeed")
    SESSION.delete_value(_ACTIVITY, "PublishUserActivities")


def _detect_priv_activity_history() -> bool:
    return (
        SESSION.read_dword(_ACTIVITY, "EnableActivityFeed") == 0
        and SESSION.read_dword(_ACTIVITY, "PublishUserActivities") == 0
    )


# ── Disable Advertising ID (user-level) ─────────────────────────────────────


def _apply_priv_adid_off(*, require_admin: bool = False) -> None:
    SESSION.log("Privacy: disable advertising ID")
    SESSION.backup([_ADID], "PrivAdID")
    SESSION.set_dword(_ADID, "Enabled", 0)


def _remove_priv_adid_off(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_ADID, "Enabled", 1)


def _detect_priv_adid_off() -> bool:
    return SESSION.read_dword(_ADID, "Enabled") == 0


TWEAKS += [
    TweakDef(
        id="privacy-disable-activity-history",
        label="Disable Activity History",
        category="Privacy",
        apply_fn=_apply_priv_activity_history,
        remove_fn=_remove_priv_activity_history,
        detect_fn=_detect_priv_activity_history,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ACTIVITY],
        description=(
            "Disables Windows activity history and timeline via Group Policy. "
            "Prevents activity feed and user activity publishing. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["privacy", "activity", "history", "timeline", "policy"],
    ),
    TweakDef(
        id="privacy-disable-advertising-id",
        label="Disable Advertising ID",
        category="Privacy",
        apply_fn=_apply_priv_adid_off,
        remove_fn=_remove_priv_adid_off,
        detect_fn=_detect_priv_adid_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADID],
        description=(
            "Disables the Windows advertising ID for the current user. "
            "Prevents apps from using the ID for cross-app ad targeting. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["privacy", "advertising", "id", "tracking"],
    ),
]


# ══ Additional Privacy Tweaks (Sophia Script / WinUtil) ═══════════════════

_TAILORED = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Privacy"
_CONTENT_DELIVERY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion"
    r"\ContentDeliveryManager"
)
_FEEDBACK = r"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"
_SETTINGS_PRIV = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SettingsSync"


# -- Disable Tailored Experiences ────────────────────────────────────────


def _apply_disable_tailored(*, require_admin: bool = False) -> None:
    SESSION.log("Privacy: disable tailored experiences based on diagnostic data")
    SESSION.backup([_TAILORED], "TailoredExperiences")
    SESSION.set_dword(_TAILORED, "TailoredExperiencesWithDiagnosticDataEnabled", 0)


def _remove_disable_tailored(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_TAILORED, "TailoredExperiencesWithDiagnosticDataEnabled", 1)


def _detect_disable_tailored() -> bool:
    return SESSION.read_dword(_TAILORED, "TailoredExperiencesWithDiagnosticDataEnabled") == 0


# -- Disable Windows Tips & Suggestions ──────────────────────────────────


def _apply_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.log("Privacy: disable Windows tips, tricks, and suggestions")
    SESSION.backup([_CONTENT_DELIVERY], "WindowsTips")
    SESSION.set_dword(_CONTENT_DELIVERY, "SoftLandingEnabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-338389Enabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-310093Enabled", 0)


def _remove_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONTENT_DELIVERY, "SoftLandingEnabled", 1)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-338389Enabled", 1)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-310093Enabled", 1)


def _detect_disable_tips() -> bool:
    return SESSION.read_dword(_CONTENT_DELIVERY, "SoftLandingEnabled") == 0


# -- Disable Feedback Prompts ────────────────────────────────────────────


def _apply_disable_feedback(*, require_admin: bool = False) -> None:
    SESSION.log("Privacy: disable feedback frequency prompts")
    SESSION.backup([_FEEDBACK], "FeedbackPrompts")
    SESSION.set_dword(_FEEDBACK, "NumberOfSIUFInPeriod", 0)


def _remove_disable_feedback(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_FEEDBACK, "NumberOfSIUFInPeriod")


def _detect_disable_feedback() -> bool:
    return SESSION.read_dword(_FEEDBACK, "NumberOfSIUFInPeriod") == 0


# -- Disable App Launch Tracking ─────────────────────────────────────────

_LAUNCH_TRACK = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"


def _apply_disable_app_launch_tracking(*, require_admin: bool = False) -> None:
    SESSION.log("Privacy: disable app launch tracking for Start menu")
    SESSION.backup([_LAUNCH_TRACK], "AppLaunchTracking")
    SESSION.set_dword(_LAUNCH_TRACK, "Start_TrackProgs", 0)


def _remove_disable_app_launch_tracking(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_LAUNCH_TRACK, "Start_TrackProgs", 1)


def _detect_disable_app_launch_tracking() -> bool:
    return SESSION.read_dword(_LAUNCH_TRACK, "Start_TrackProgs") == 0


# -- Disable Suggested Content in Settings ───────────────────────────────


def _apply_disable_settings_suggestions(*, require_admin: bool = False) -> None:
    SESSION.log("Privacy: disable suggested content in Settings app")
    SESSION.backup([_CONTENT_DELIVERY], "SettingsSuggestions")
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-338393Enabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-353694Enabled", 0)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-353696Enabled", 0)


def _remove_disable_settings_suggestions(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-338393Enabled", 1)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-353694Enabled", 1)
    SESSION.set_dword(_CONTENT_DELIVERY, "SubscribedContent-353696Enabled", 1)


def _detect_disable_settings_suggestions() -> bool:
    return SESSION.read_dword(_CONTENT_DELIVERY, "SubscribedContent-338393Enabled") == 0


TWEAKS += [
    TweakDef(
        id="privacy-disable-tailored-experiences",
        label="Disable Tailored Experiences",
        category="Privacy",
        apply_fn=_apply_disable_tailored,
        remove_fn=_remove_disable_tailored,
        detect_fn=_detect_disable_tailored,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TAILORED],
        description=(
            "Disables Microsoft tailored experiences that use diagnostic data "
            "to personalise ads, tips, and recommendations. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["privacy", "tailored", "diagnostic", "personalisation"],
    ),
    TweakDef(
        id="privacy-disable-windows-tips",
        label="Disable Windows Tips & Suggestions",
        category="Privacy",
        apply_fn=_apply_disable_tips,
        remove_fn=_remove_disable_tips,
        detect_fn=_detect_disable_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description=(
            "Disables Windows tips, tricks, and 'Get started' suggestions "
            "that appear in Start menu, lock screen and notifications. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["privacy", "tips", "suggestions", "content-delivery"],
    ),
    TweakDef(
        id="privacy-disable-feedback",
        label="Disable Feedback Prompts",
        category="Privacy",
        apply_fn=_apply_disable_feedback,
        remove_fn=_remove_disable_feedback,
        detect_fn=_detect_disable_feedback,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FEEDBACK],
        description=(
            "Disables 'How likely are you to recommend Windows?' feedback "
            "prompts from appearing periodically. "
            "Default: occasional. Recommended: disabled."
        ),
        tags=["privacy", "feedback", "prompts", "telemetry"],
    ),
    TweakDef(
        id="privacy-disable-app-launch-tracking",
        label="Disable App Launch Tracking",
        category="Privacy",
        apply_fn=_apply_disable_app_launch_tracking,
        remove_fn=_remove_disable_app_launch_tracking,
        detect_fn=_detect_disable_app_launch_tracking,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LAUNCH_TRACK],
        description=(
            "Prevents Windows from tracking which apps you launch to improve "
            "Start menu suggestions. Default: enabled. Recommended: disabled."
        ),
        tags=["privacy", "tracking", "start-menu", "launch"],
    ),
    TweakDef(
        id="privacy-disable-settings-suggestions",
        label="Disable Suggested Content in Settings",
        category="Privacy",
        apply_fn=_apply_disable_settings_suggestions,
        remove_fn=_remove_disable_settings_suggestions,
        detect_fn=_detect_disable_settings_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT_DELIVERY],
        description=(
            "Disables suggested content and recommendations in the Windows "
            "Settings app. Default: enabled. Recommended: disabled."
        ),
        tags=["privacy", "settings", "suggestions", "content-delivery"],
    ),
]


# ── Disable Handwriting Data Sharing ────────────────────────────────────────

_HANDWRITING = r"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization"
_HANDWRITING_POLICY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft"
    r"\Windows\HandwritingErrorReports"
)


def _apply_disable_handwriting_sharing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable handwriting data sharing")
    SESSION.backup([_HANDWRITING, _HANDWRITING_POLICY], "HandwritingSharing")
    SESSION.set_dword(_HANDWRITING, "RestrictImplicitInkCollection", 1)
    SESSION.set_dword(_HANDWRITING, "RestrictImplicitTextCollection", 1)
    SESSION.set_dword(_HANDWRITING_POLICY, "PreventHandwritingErrorReports", 1)


def _remove_disable_handwriting_sharing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_HANDWRITING, "RestrictImplicitInkCollection")
    SESSION.delete_value(_HANDWRITING, "RestrictImplicitTextCollection")
    SESSION.delete_value(_HANDWRITING_POLICY, "PreventHandwritingErrorReports")


def _detect_disable_handwriting_sharing() -> bool:
    return (
        SESSION.read_dword(_HANDWRITING, "RestrictImplicitInkCollection") == 1
        and SESSION.read_dword(_HANDWRITING, "RestrictImplicitTextCollection") == 1
        and SESSION.read_dword(_HANDWRITING_POLICY, "PreventHandwritingErrorReports") == 1
    )


# ── Disable App Launch Tracking (Policy) ───────────────────────────────────

_APP_COMPAT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppCompat"


def _apply_disable_launch_tracking_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable app launch tracking via policy (UAR)")
    SESSION.backup([_APP_COMPAT], "AppLaunchTrackingPolicy")
    SESSION.set_dword(_APP_COMPAT, "DisableUAR", 1)


def _remove_disable_launch_tracking_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_APP_COMPAT, "DisableUAR")


def _detect_disable_launch_tracking_policy() -> bool:
    return SESSION.read_dword(_APP_COMPAT, "DisableUAR") == 1


# ── Disable Cross-Device Experiences ────────────────────────────────────────

_CDP_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_disable_cross_device(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Privacy: disable cross-device experiences (CDP)")
    SESSION.backup([_CDP_POLICY], "CrossDeviceExperiences")
    SESSION.set_dword(_CDP_POLICY, "EnableCdp", 0)


def _remove_disable_cross_device(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CDP_POLICY, "EnableCdp")


def _detect_disable_cross_device() -> bool:
    return SESSION.read_dword(_CDP_POLICY, "EnableCdp") == 0


TWEAKS += [
    TweakDef(
        id="priv-disable-handwriting-sharing",
        label="Disable Handwriting Data Sharing",
        category="Privacy",
        apply_fn=_apply_disable_handwriting_sharing,
        remove_fn=_remove_disable_handwriting_sharing,
        detect_fn=_detect_disable_handwriting_sharing,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_HANDWRITING, _HANDWRITING_POLICY],
        description=(
            "Disables implicit ink/text collection and handwriting error reports. "
            "Prevents handwriting data from being shared with Microsoft. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["privacy", "handwriting", "ink", "data-sharing"],
    ),
    TweakDef(
        id="priv-disable-launch-tracking",
        label="Disable App Launch Tracking (Policy)",
        category="Privacy",
        apply_fn=_apply_disable_launch_tracking_policy,
        remove_fn=_remove_disable_launch_tracking_policy,
        detect_fn=_detect_disable_launch_tracking_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_APP_COMPAT],
        description=(
            "Disables User Activity Reporting (UAR) at the machine policy level. "
            "Prevents Windows from tracking application launches system-wide. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["privacy", "tracking", "launch", "uar", "policy"],
    ),
    TweakDef(
        id="priv-disable-cross-device",
        label="Disable Cross-Device Experiences",
        category="Privacy",
        apply_fn=_apply_disable_cross_device,
        remove_fn=_remove_disable_cross_device,
        detect_fn=_detect_disable_cross_device,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CDP_POLICY],
        description=(
            "Disables the Connected Devices Platform (CDP) which enables "
            "cross-device experiences like phone-to-PC linking and shared "
            "clipboard. Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["privacy", "cross-device", "cdp", "phone-link"],
    ),
]
