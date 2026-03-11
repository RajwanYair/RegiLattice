"""Speech tweaks — Voice Access, Narrator, dictation, and online speech.

Covers: Voice access settings, narrator customisation, dictation,
online speech recognition, and speech service policies.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_SPEECH_USER = r"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"
_NARRATOR = r"HKEY_CURRENT_USER\Software\Microsoft\Narrator"
_NARRATOR_NO_ROAM = r"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"
_SPEECH_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Speech"
_INPUT_PERSON = r"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization"
_INPUT_TRAIN = r"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore"
_VOICE_ACCESS = r"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\VoiceAccess"
_DICTATION = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\ImpressionWhitelist"
_ACC_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Accessibility"
_NARRATOR_SVC = r"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NarSvc"


# ── Disable Online Speech Recognition ───────────────────────────────────────


def _apply_disable_online_speech(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: disable online speech recognition")
    SESSION.backup([_SPEECH_USER], "OnlineSpeech")
    SESSION.set_dword(_SPEECH_USER, "HasAccepted", 0)


def _remove_disable_online_speech(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_SPEECH_USER, "HasAccepted", 1)


def _detect_disable_online_speech() -> bool:
    return SESSION.read_dword(_SPEECH_USER, "HasAccepted") == 0


# ── Disable Online Speech via Policy ────────────────────────────────────────


def _apply_disable_speech_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Speech: disable online speech recognition via policy")
    SESSION.backup([_SPEECH_POLICY], "SpeechPolicy")
    SESSION.set_dword(_SPEECH_POLICY, "AllowSpeechModelUpdate", 0)


def _remove_disable_speech_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPEECH_POLICY, "AllowSpeechModelUpdate")


def _detect_disable_speech_policy() -> bool:
    return SESSION.read_dword(_SPEECH_POLICY, "AllowSpeechModelUpdate") == 0


# ── Mute Narrator Sounds ────────────────────────────────────────────────────


def _apply_mute_narrator(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: mute Narrator navigation sounds")
    SESSION.backup([_NARRATOR_NO_ROAM], "NarratorSounds")
    SESSION.set_dword(_NARRATOR_NO_ROAM, "PlayNavigationSounds", 0)


def _remove_mute_narrator(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NARRATOR_NO_ROAM, "PlayNavigationSounds", 1)


def _detect_mute_narrator() -> bool:
    return SESSION.read_dword(_NARRATOR_NO_ROAM, "PlayNavigationSounds") == 0


# ── Disable Typing Insights ─────────────────────────────────────────────────


def _apply_disable_typing_insights(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: disable input personalisation / typing insights")
    SESSION.backup([_INPUT_PERSON, _INPUT_TRAIN], "TypingInsights")
    SESSION.set_dword(_INPUT_PERSON, "RestrictImplicitInkCollection", 1)
    SESSION.set_dword(_INPUT_PERSON, "RestrictImplicitTextCollection", 1)
    SESSION.set_dword(_INPUT_TRAIN, "HarvestContacts", 0)


def _remove_disable_typing_insights(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_INPUT_PERSON, "RestrictImplicitInkCollection", 0)
    SESSION.set_dword(_INPUT_PERSON, "RestrictImplicitTextCollection", 0)
    SESSION.set_dword(_INPUT_TRAIN, "HarvestContacts", 1)


def _detect_disable_typing_insights() -> bool:
    return SESSION.read_dword(_INPUT_PERSON, "RestrictImplicitTextCollection") == 1


# ── Disable Voice Access ────────────────────────────────────────────────────


def _apply_disable_voice_access(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: disable Voice Access feature")
    SESSION.backup([_VOICE_ACCESS], "VoiceAccess")
    SESSION.set_dword(_VOICE_ACCESS, "VoiceAccessEnabled", 0)


def _remove_disable_voice_access(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_VOICE_ACCESS, "VoiceAccessEnabled", 1)


def _detect_disable_voice_access() -> bool:
    return SESSION.read_dword(_VOICE_ACCESS, "VoiceAccessEnabled") == 0


# ── Disable Voice Activation ────────────────────────────────────────────────


def _apply_disable_voice_activation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Speech: disable voice activation (Hey Cortana / wake words)")
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"
    SESSION.backup([_key], "VoiceActivation")
    SESSION.set_dword(_key, "LetAppsActivateWithVoice", 2)


def _remove_disable_voice_activation(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"
    SESSION.delete_value(_key, "LetAppsActivateWithVoice")


def _detect_disable_voice_activation() -> bool:
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"
    return SESSION.read_dword(_key, "LetAppsActivateWithVoice") == 2


# ── Disable Voice Activation Above Lock ─────────────────────────────────────


def _apply_disable_voice_above_lock(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Speech: disable voice activation above lock screen")
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"
    SESSION.backup([_key], "VoiceAboveLock")
    SESSION.set_dword(_key, "LetAppsActivateWithVoiceAboveLock", 2)


def _remove_disable_voice_above_lock(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"
    SESSION.delete_value(_key, "LetAppsActivateWithVoiceAboveLock")


def _detect_disable_voice_above_lock() -> bool:
    _key = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"
    return SESSION.read_dword(_key, "LetAppsActivateWithVoiceAboveLock") == 2


# ── Set Narrator Voice Speed ────────────────────────────────────────────────


def _apply_narrator_speed(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: set Narrator reading speed to fast")
    SESSION.backup([_NARRATOR_NO_ROAM], "NarratorSpeed")
    SESSION.set_dword(_NARRATOR_NO_ROAM, "SpeakingRate", 8)


def _remove_narrator_speed(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NARRATOR_NO_ROAM, "SpeakingRate", 5)


def _detect_narrator_speed() -> bool:
    return SESSION.read_dword(_NARRATOR_NO_ROAM, "SpeakingRate") == 8


# ── Disable Narrator Cursor ─────────────────────────────────────────────────


def _apply_disable_narrator_cursor(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: disable Narrator cursor indicator")
    SESSION.backup([_NARRATOR], "NarratorCursor")
    SESSION.set_dword(_NARRATOR, "ShowCursor", 0)


def _remove_disable_narrator_cursor(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NARRATOR, "ShowCursor", 1)


def _detect_disable_narrator_cursor() -> bool:
    return SESSION.read_dword(_NARRATOR, "ShowCursor") == 0


# ── Disable Narrator Service ────────────────────────────────────────────────


def _apply_disable_narrator_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Speech: set Narrator service to disabled")
    SESSION.backup([_NARRATOR_SVC], "NarratorSvc")
    SESSION.set_dword(_NARRATOR_SVC, "Start", 4)


def _remove_disable_narrator_svc(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_NARRATOR_SVC, "Start", 3)


def _detect_disable_narrator_svc() -> bool:
    return SESSION.read_dword(_NARRATOR_SVC, "Start") == 4


# ── Disable Narrator Keyboard Shortcut ──────────────────────────────────────


def _apply_disable_narrator_shortcut(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Speech: disable Narrator keyboard shortcut (Win+Ctrl+Enter)")
    SESSION.backup([_ACC_POLICY], "NarratorShortcut")
    SESSION.set_dword(_ACC_POLICY, "ForceDisableNarratorShortcutKeys", 1)


def _remove_disable_narrator_shortcut(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ACC_POLICY, "ForceDisableNarratorShortcutKeys")


def _detect_disable_narrator_shortcut() -> bool:
    return SESSION.read_dword(_ACC_POLICY, "ForceDisableNarratorShortcutKeys") == 1


# ── Disable Speech Model Updates ────────────────────────────────────────────


def _apply_disable_model_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Speech: disable automatic speech model updates")
    SESSION.backup([_SPEECH_POLICY], "SpeechModelUpdate")
    SESSION.set_dword(_SPEECH_POLICY, "AllowSpeechModelUpdate", 0)
    SESSION.set_dword(_SPEECH_POLICY, "AllowSpeechDataCollection", 0)


def _remove_disable_model_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPEECH_POLICY, "AllowSpeechModelUpdate")
    SESSION.delete_value(_SPEECH_POLICY, "AllowSpeechDataCollection")


def _detect_disable_model_update() -> bool:
    return SESSION.read_dword(_SPEECH_POLICY, "AllowSpeechDataCollection") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="speech-disable-online",
        label="Disable Online Speech Recognition",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_online_speech,
        remove_fn=_remove_disable_online_speech,
        detect_fn=_detect_disable_online_speech,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SPEECH_USER],
        description="Prevents speech data from being sent to Microsoft for processing. Forces on-device-only recognition. Recommended.",
        tags=["speech", "online", "privacy", "voice"],
    ),
    TweakDef(
        id="speech-disable-policy",
        label="Disable Speech Recognition (Policy)",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_speech_policy,
        remove_fn=_remove_disable_speech_policy,
        detect_fn=_detect_disable_speech_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SPEECH_POLICY],
        description="Blocks online speech model updates via Group Policy. Reduces background data usage.",
        tags=["speech", "policy", "privacy"],
    ),
    TweakDef(
        id="speech-mute-narrator",
        label="Mute Narrator Navigation Sounds",
        category="Voice Access & Speech",
        apply_fn=_apply_mute_narrator,
        remove_fn=_remove_mute_narrator,
        detect_fn=_detect_mute_narrator,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR_NO_ROAM],
        description="Silences Narrator navigation and notification sounds. Default: Enabled.",
        tags=["speech", "narrator", "sounds"],
    ),
    TweakDef(
        id="speech-disable-typing-insights",
        label="Disable Typing Insights & Ink Collection",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_typing_insights,
        remove_fn=_remove_disable_typing_insights,
        detect_fn=_detect_disable_typing_insights,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_INPUT_PERSON, _INPUT_TRAIN],
        description="Stops Windows from collecting typing patterns and ink data for personalisation. Recommended: Disabled for privacy.",
        tags=["speech", "typing", "ink", "privacy"],
    ),
    TweakDef(
        id="speech-disable-voice-access",
        label="Disable Voice Access",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_voice_access,
        remove_fn=_remove_disable_voice_access,
        detect_fn=_detect_disable_voice_access,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_VOICE_ACCESS],
        description="Disables Windows 11 Voice Access feature (hands-free PC control). Default: Disabled.",
        tags=["speech", "voice-access", "accessibility"],
    ),
    TweakDef(
        id="speech-disable-voice-activation",
        label="Disable Voice Activation (Wake Words)",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_voice_activation,
        remove_fn=_remove_disable_voice_activation,
        detect_fn=_detect_disable_voice_activation,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
        description="Prevents apps from listening for wake words (Hey Cortana, etc.). Recommended: Disabled for privacy.",
        tags=["speech", "voice", "activation", "cortana", "privacy"],
    ),
    TweakDef(
        id="speech-disable-voice-above-lock",
        label="Disable Voice Activation Above Lock",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_voice_above_lock,
        remove_fn=_remove_disable_voice_above_lock,
        detect_fn=_detect_disable_voice_above_lock,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
        description="Prevents voice activation when the screen is locked. Security measure against wake-word hijacking.",
        tags=["speech", "voice", "lock-screen", "security"],
    ),
    TweakDef(
        id="speech-narrator-fast-speed",
        label="Set Narrator Speed to Fast",
        category="Voice Access & Speech",
        apply_fn=_apply_narrator_speed,
        remove_fn=_remove_narrator_speed,
        detect_fn=_detect_narrator_speed,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR_NO_ROAM],
        description="Sets Narrator reading speed to fast (8/10) for power users who rely on screen readers.",
        tags=["speech", "narrator", "speed"],
    ),
    TweakDef(
        id="speech-disable-narrator-cursor",
        label="Disable Narrator Cursor Indicator",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_narrator_cursor,
        remove_fn=_remove_disable_narrator_cursor,
        detect_fn=_detect_disable_narrator_cursor,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR],
        description="Hides the blue Narrator cursor box that highlights the current element.",
        tags=["speech", "narrator", "cursor", "visual"],
    ),
    TweakDef(
        id="speech-disable-narrator-svc",
        label="Disable Narrator Service",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_narrator_svc,
        remove_fn=_remove_disable_narrator_svc,
        detect_fn=_detect_disable_narrator_svc,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_NARRATOR_SVC],
        description="Disables the Narrator background service. The service can be re-enabled manually if needed.",
        tags=["speech", "narrator", "service"],
    ),
    TweakDef(
        id="speech-disable-narrator-shortcut",
        label="Disable Narrator Keyboard Shortcut",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_narrator_shortcut,
        remove_fn=_remove_disable_narrator_shortcut,
        detect_fn=_detect_disable_narrator_shortcut,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ACC_POLICY],
        description="Disables Win+Ctrl+Enter shortcut that accidentally launches Narrator. Policy setting.",
        tags=["speech", "narrator", "shortcut", "keyboard"],
    ),
    TweakDef(
        id="speech-disable-model-update",
        label="Disable Speech Model & Data Collection",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_model_update,
        remove_fn=_remove_disable_model_update,
        detect_fn=_detect_disable_model_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SPEECH_POLICY],
        description="Stops automatic speech model downloads and voice data collection. Reduces bandwidth and improves privacy.",
        tags=["speech", "model", "telemetry", "privacy"],
    ),
]


# ── Disable Windows Dictation (Win+H) ────────────────────────────────────────

_DICTATION_KEY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\SpeechRecognition"


def _apply_disable_dictation_shortcut(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: disable Windows dictation shortcut (Win+H)")
    SESSION.backup([_DICTATION_KEY], "DictationShortcut")
    SESSION.set_dword(_DICTATION_KEY, "Value", 0)


def _remove_disable_dictation_shortcut(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_DICTATION_KEY, "Value", 1)


def _detect_disable_dictation_shortcut() -> bool:
    return SESSION.read_dword(_DICTATION_KEY, "Value") == 0


# ── Disable Narrator Hints & Coaching ────────────────────────────────────────


def _apply_disable_narrator_hints(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: disable Narrator hints and coaching messages")
    SESSION.backup([_NARRATOR_NO_ROAM], "NarratorHints")
    SESSION.set_dword(_NARRATOR_NO_ROAM, "SpeakWindowsHints", 0)
    SESSION.set_dword(_NARRATOR_NO_ROAM, "DetailedFeedback", 0)


def _remove_disable_narrator_hints(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NARRATOR_NO_ROAM, "SpeakWindowsHints", 1)
    SESSION.set_dword(_NARRATOR_NO_ROAM, "DetailedFeedback", 1)


def _detect_disable_narrator_hints() -> bool:
    return SESSION.read_dword(_NARRATOR_NO_ROAM, "SpeakWindowsHints") == 0


# ── Set Narrator Verbosity to Low ─────────────────────────────────────────────


def _apply_narrator_verbosity_low(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: set Narrator verbosity to low (essential info only)")
    SESSION.backup([_NARRATOR_NO_ROAM], "NarratorVerbosity")
    SESSION.set_dword(_NARRATOR_NO_ROAM, "VerbosityLevel", 1)  # 0=minimal, 1=some, 2=full


def _remove_narrator_verbosity_low(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_NARRATOR_NO_ROAM, "VerbosityLevel", 2)


def _detect_narrator_verbosity_low() -> bool:
    return SESSION.read_dword(_NARRATOR_NO_ROAM, "VerbosityLevel") == 1


# ── Disable Cortana Speech Permissions ───────────────────────────────────────

_CORTANA_SPEECH = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CPSS\Store\UserSpeechAuthorization"


def _apply_disable_cortana_speech(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: revoke Cortana / assistant speech permissions")
    SESSION.backup([_CORTANA_SPEECH], "CortanaSpeech")
    SESSION.set_dword(_CORTANA_SPEECH, "Value", 0)


def _remove_disable_cortana_speech(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_CORTANA_SPEECH, "Value", 1)


def _detect_disable_cortana_speech() -> bool:
    return SESSION.read_dword(_CORTANA_SPEECH, "Value") == 0


# ── Disable Automatic Language Detection ─────────────────────────────────────

_LANG_DETECT = r"HKEY_CURRENT_USER\Control Panel\International\User Profile"


def _apply_disable_lang_detect(*, require_admin: bool = False) -> None:
    SESSION.log("Speech: disable automatic input language detection")
    SESSION.backup([_LANG_DETECT], "LangDetect")
    SESSION.set_dword(_LANG_DETECT, "HttpAcceptLanguageOptOut", 1)


def _remove_disable_lang_detect(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_LANG_DETECT, "HttpAcceptLanguageOptOut", 0)


def _detect_disable_lang_detect() -> bool:
    return SESSION.read_dword(_LANG_DETECT, "HttpAcceptLanguageOptOut") == 1


TWEAKS += [
    TweakDef(
        id="speech-disable-dictation-shortcut",
        label="Disable Dictation Shortcut (Win+H)",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_dictation_shortcut,
        remove_fn=_remove_disable_dictation_shortcut,
        detect_fn=_detect_disable_dictation_shortcut,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DICTATION_KEY],
        description=(
            "Disables the Win+H keyboard shortcut that launches Windows speech dictation. "
            "Prevents accidental activation during gaming or video playback. "
            "Default: Enabled. Recommended: Disabled for non-dictation users."
        ),
        tags=["speech", "dictation", "shortcut", "win+h"],
    ),
    TweakDef(
        id="speech-disable-narrator-hints",
        label="Disable Narrator Hints & Coaching",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_narrator_hints,
        remove_fn=_remove_disable_narrator_hints,
        detect_fn=_detect_disable_narrator_hints,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR_NO_ROAM],
        description=(
            "Turns off Narrator's spoken hints about keyboard shortcuts and coaching messages. "
            "Reduces verbosity for experienced Narrator users. Default: Enabled."
        ),
        tags=["speech", "narrator", "hints", "coaching"],
    ),
    TweakDef(
        id="speech-narrator-verbosity-low",
        label="Set Narrator Verbosity to Low",
        category="Voice Access & Speech",
        apply_fn=_apply_narrator_verbosity_low,
        remove_fn=_remove_narrator_verbosity_low,
        detect_fn=_detect_narrator_verbosity_low,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_NARRATOR_NO_ROAM],
        description=(
            "Sets Narrator to only announce essential information (level 1 of 3). "
            "Reduces noise for power users of screen readers. Default: Level 2 (some)."
        ),
        tags=["speech", "narrator", "verbosity", "accessibility"],
    ),
    TweakDef(
        id="speech-disable-cortana-speech",
        label="Revoke Cortana Speech Permissions",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_cortana_speech,
        remove_fn=_remove_disable_cortana_speech,
        detect_fn=_detect_disable_cortana_speech,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CORTANA_SPEECH],
        description=(
            "Removes Cortana and assistant speech authorisation. "
            "Prevents Windows from using speech data for personalisation and assistant features. "
            "Recommended: Disabled for privacy."
        ),
        tags=["speech", "cortana", "assistant", "privacy"],
    ),
    TweakDef(
        id="speech-disable-lang-detect",
        label="Disable Automatic Language Detection",
        category="Voice Access & Speech",
        apply_fn=_apply_disable_lang_detect,
        remove_fn=_remove_disable_lang_detect,
        detect_fn=_detect_disable_lang_detect,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_LANG_DETECT],
        description=(
            "Opts out of automatic input language detection sent via HTTP Accept-Language headers. "
            "A minor privacy improvement for users with multiple input languages. Default: Opted in."
        ),
        tags=["speech", "language", "detection", "privacy"],
    ),
]
