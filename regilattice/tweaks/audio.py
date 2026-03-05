"""Audio tweaks — System sounds, enhancements, spatial audio, notification sounds."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Registry key constants ───────────────────────────────────────────────────

_KEY_SOUND_SCHEME = r"HKEY_CURRENT_USER\AppEvents\Schemes"

_KEY_BOOT_ANIM = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Authentication\LogonUI\BootAnimation"
)

_KEY_MM_AUDIO = r"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"

_KEY_CUR_AUDIO = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio"
)

_KEY_NOTIF_SETTINGS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Notifications\Settings"
)

_KEY_LOW_BATTERY = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows"
    r"\CurrentVersion\Policies\System"
)

_KEY_SPEECH = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore"
    r"\Settings\OnlineSpeechPrivacy"
)

_KEY_SOUND_COMMS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"
)

_KEY_EXCLUSIVE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"
)

_KEY_DEFAULT_FMT = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"
)

_KEY_AUDIO_GLOBAL = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"
)

_KEY_AUDIO_TASK = (
    r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT"
    r"\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"
)


# ── Disable System Sounds ────────────────────────────────────────────────────


def _apply_disable_system_sounds(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: disable system sounds (set scheme to .None)")
    SESSION.backup([_KEY_SOUND_SCHEME], "SystemSounds")
    SESSION.set_string(_KEY_SOUND_SCHEME, "", ".None")


def _remove_disable_system_sounds(*, require_admin: bool = False) -> None:
    SESSION.set_string(_KEY_SOUND_SCHEME, "", ".Default")


def _detect_disable_system_sounds() -> bool:
    return SESSION.read_string(_KEY_SOUND_SCHEME, "") == ".None"


# ── Disable Startup Sound ────────────────────────────────────────────────────


def _apply_disable_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Audio: disable Windows startup sound")
    SESSION.backup([_KEY_BOOT_ANIM], "StartupSound")
    SESSION.set_dword(_KEY_BOOT_ANIM, "DisableStartupSound", 1)


def _remove_disable_startup_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_BOOT_ANIM, "DisableStartupSound")


def _detect_disable_startup_sound() -> bool:
    return SESSION.read_dword(_KEY_BOOT_ANIM, "DisableStartupSound") == 1


# ── Disable Communication Auto-Reduction ─────────────────────────────────────


def _apply_disable_ducking(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: disable communication auto-reduction (ducking)")
    SESSION.backup([_KEY_MM_AUDIO], "CommDucking")
    SESSION.set_dword(_KEY_MM_AUDIO, "UserDuckingPreference", 3)


def _remove_disable_ducking(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_MM_AUDIO, "UserDuckingPreference")


def _detect_disable_ducking() -> bool:
    return SESSION.read_dword(_KEY_MM_AUDIO, "UserDuckingPreference") == 3


# ── Disable Audio Enhancements ───────────────────────────────────────────────


def _apply_disable_enhancements(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: disable audio enhancements globally")
    SESSION.backup([_KEY_MM_AUDIO], "AudioEnhancements")
    SESSION.set_dword(_KEY_MM_AUDIO, "DisableAllEnhancements", 1)


def _remove_disable_enhancements(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_MM_AUDIO, "DisableAllEnhancements")


def _detect_disable_enhancements() -> bool:
    return SESSION.read_dword(_KEY_MM_AUDIO, "DisableAllEnhancements") == 1


# ── Disable Spatial Sound Auto ───────────────────────────────────────────────


def _apply_disable_spatial_audio(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: disable spatial audio auto-activation")
    SESSION.backup([_KEY_CUR_AUDIO], "SpatialAudio")
    SESSION.set_dword(_KEY_CUR_AUDIO, "EnableSpatialAudio", 0)


def _remove_disable_spatial_audio(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_CUR_AUDIO, "EnableSpatialAudio")


def _detect_disable_spatial_audio() -> bool:
    return SESSION.read_dword(_KEY_CUR_AUDIO, "EnableSpatialAudio") == 0


# ── Disable Notification Sounds ──────────────────────────────────────────────


def _apply_disable_notification_sounds(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: disable notification sounds globally")
    SESSION.backup([_KEY_NOTIF_SETTINGS], "NotifSounds")
    SESSION.set_dword(
        _KEY_NOTIF_SETTINGS,
        "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
        0,
    )


def _remove_disable_notification_sounds(*, require_admin: bool = False) -> None:
    SESSION.delete_value(
        _KEY_NOTIF_SETTINGS,
        "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
    )


def _detect_disable_notification_sounds() -> bool:
    return (
        SESSION.read_dword(
            _KEY_NOTIF_SETTINGS,
            "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
        )
        == 0
    )


# ── Set Default Audio Format to 24-bit ───────────────────────────────────────


def _apply_set_24bit(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: set default audio format to 24-bit quality")
    SESSION.backup([_KEY_DEFAULT_FMT], "DefaultAudioFormat")
    SESSION.set_dword(_KEY_DEFAULT_FMT, "DefaultFormat", 24)


def _remove_set_24bit(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_DEFAULT_FMT, "DefaultFormat")


def _detect_set_24bit() -> bool:
    return SESSION.read_dword(_KEY_DEFAULT_FMT, "DefaultFormat") == 24


# ── Disable Audio Exclusive Mode ─────────────────────────────────────────────


def _apply_disable_exclusive_mode(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: disable exclusive mode for audio devices")
    SESSION.backup([_KEY_EXCLUSIVE], "ExclusiveMode")
    SESSION.set_dword(_KEY_EXCLUSIVE, "ExclusiveMode", 0)


def _remove_disable_exclusive_mode(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_KEY_EXCLUSIVE, "ExclusiveMode")


def _detect_disable_exclusive_mode() -> bool:
    return SESSION.read_dword(_KEY_EXCLUSIVE, "ExclusiveMode") == 0


# ── Disable Low Battery Sound ────────────────────────────────────────────────


def _apply_disable_low_battery_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Audio: disable low-battery warning sound")
    SESSION.backup([_KEY_LOW_BATTERY], "LowBatterySound")
    SESSION.set_dword(_KEY_LOW_BATTERY, "DisableLowBatterySound", 1)


def _remove_disable_low_battery_sound(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_LOW_BATTERY, "DisableLowBatterySound")


def _detect_disable_low_battery_sound() -> bool:
    return SESSION.read_dword(_KEY_LOW_BATTERY, "DisableLowBatterySound") == 1


# ── Disable Online Speech Recognition ────────────────────────────────────────


def _apply_disable_speech_recognition(*, require_admin: bool = False) -> None:
    SESSION.log("Audio: disable online speech recognition auto-opt-in")
    SESSION.backup([_KEY_SPEECH], "SpeechRecognition")
    SESSION.set_dword(_KEY_SPEECH, "HasAccepted", 0)


def _remove_disable_speech_recognition(*, require_admin: bool = False) -> None:
    SESSION.set_dword(_KEY_SPEECH, "HasAccepted", 1)


def _detect_disable_speech_recognition() -> bool:
    return SESSION.read_dword(_KEY_SPEECH, "HasAccepted") == 0


# ── Disable Audio Enhancements Globally (HKLM) ──────────────────────────────


def _apply_disable_enhancements_global(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Audio: disable audio enhancements system-wide (HKLM)")
    SESSION.backup([_KEY_AUDIO_GLOBAL], "AudioEnhancementsGlobal")
    SESSION.set_dword(_KEY_AUDIO_GLOBAL, "DisableEnhancements", 1)


def _remove_disable_enhancements_global(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_KEY_AUDIO_GLOBAL, "DisableEnhancements")


def _detect_disable_enhancements_global() -> bool:
    return SESSION.read_dword(_KEY_AUDIO_GLOBAL, "DisableEnhancements") == 1


# ── Set Audio Exclusive Mode Priority ────────────────────────────────────────


def _apply_exclusive_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Audio: set audio thread scheduling to highest priority")
    SESSION.backup([_KEY_AUDIO_TASK], "AudioExclusivePriority")
    SESSION.set_dword(_KEY_AUDIO_TASK, "Priority", 6)
    SESSION.set_string(_KEY_AUDIO_TASK, "Scheduling Category", "High")


def _remove_exclusive_priority(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_KEY_AUDIO_TASK, "Priority", 2)
    SESSION.set_string(_KEY_AUDIO_TASK, "Scheduling Category", "Medium")


def _detect_exclusive_priority() -> bool:
    return SESSION.read_dword(_KEY_AUDIO_TASK, "Priority") == 6


# ── TWEAKS list ──────────────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="audio-disable-system-sounds",
        label="Disable System Sounds",
        category="Audio",
        apply_fn=_apply_disable_system_sounds,
        remove_fn=_remove_disable_system_sounds,
        detect_fn=_detect_disable_system_sounds,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_SOUND_SCHEME],
        description=(
            "Sets the Windows sound scheme to .None, silencing all "
            "system event sounds (alerts, notifications, asterisks, etc.)."
        ),
        tags=["audio", "sounds", "scheme", "silence"],
    ),
    TweakDef(
        id="audio-disable-startup-sound",
        label="Disable Startup Sound",
        category="Audio",
        apply_fn=_apply_disable_startup_sound,
        remove_fn=_remove_disable_startup_sound,
        detect_fn=_detect_disable_startup_sound,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_BOOT_ANIM],
        description=(
            "Disables the Windows boot/startup sound played when logging in."
        ),
        tags=["audio", "startup", "boot", "logon"],
    ),
    TweakDef(
        id="audio-disable-comm-ducking",
        label="Disable Communication Auto-Reduction",
        category="Audio",
        apply_fn=_apply_disable_ducking,
        remove_fn=_remove_disable_ducking,
        detect_fn=_detect_disable_ducking,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_MM_AUDIO],
        description=(
            "Prevents Windows from automatically reducing the volume of "
            "other sounds during voice/video calls (UserDuckingPreference=3)."
        ),
        tags=["audio", "ducking", "communication", "volume"],
    ),
    TweakDef(
        id="audio-disable-enhancements",
        label="Disable Audio Enhancements",
        category="Audio",
        apply_fn=_apply_disable_enhancements,
        remove_fn=_remove_disable_enhancements,
        detect_fn=_detect_disable_enhancements,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_MM_AUDIO],
        description=(
            "Disables all audio enhancements (equalizer, bass boost, "
            "virtual surround, loudness equalization)."
        ),
        tags=["audio", "enhancements", "equalizer", "processing"],
    ),
    TweakDef(
        id="audio-disable-spatial-audio",
        label="Disable Spatial Audio Auto-Activation",
        category="Audio",
        apply_fn=_apply_disable_spatial_audio,
        remove_fn=_remove_disable_spatial_audio,
        detect_fn=_detect_disable_spatial_audio,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_CUR_AUDIO],
        description=(
            "Disables automatic spatial audio (Windows Sonic / Dolby Atmos) "
            "activation by the OS."
        ),
        tags=["audio", "spatial", "sonic", "atmos"],
    ),
    TweakDef(
        id="audio-disable-notification-sounds",
        label="Disable Notification Sounds",
        category="Audio",
        apply_fn=_apply_disable_notification_sounds,
        remove_fn=_remove_disable_notification_sounds,
        detect_fn=_detect_disable_notification_sounds,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_NOTIF_SETTINGS],
        description=(
            "Silences all notification sounds while keeping toast "
            "notifications visible."
        ),
        tags=["audio", "notifications", "sounds", "toast"],
    ),
    TweakDef(
        id="audio-set-24bit-quality",
        label="Set Default Audio to 24-bit Quality",
        category="Audio",
        apply_fn=_apply_set_24bit,
        remove_fn=_remove_set_24bit,
        detect_fn=_detect_set_24bit,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_DEFAULT_FMT],
        description=(
            "Sets the default audio format to 24-bit quality for improved "
            "audio fidelity."
        ),
        tags=["audio", "quality", "24-bit", "format"],
    ),
    TweakDef(
        id="audio-disable-exclusive-mode",
        label="Disable Audio Exclusive Mode",
        category="Audio",
        apply_fn=_apply_disable_exclusive_mode,
        remove_fn=_remove_disable_exclusive_mode,
        detect_fn=_detect_disable_exclusive_mode,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_EXCLUSIVE],
        description=(
            "Prevents applications from taking exclusive control of audio "
            "devices, avoiding sound conflicts."
        ),
        tags=["audio", "exclusive", "sharing", "device"],
    ),
    TweakDef(
        id="audio-disable-low-battery-sound",
        label="Disable Low Battery Warning Sound",
        category="Audio",
        apply_fn=_apply_disable_low_battery_sound,
        remove_fn=_remove_disable_low_battery_sound,
        detect_fn=_detect_disable_low_battery_sound,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_LOW_BATTERY],
        description=(
            "Silences the audible alert played when the battery level "
            "drops below the warning threshold."
        ),
        tags=["audio", "battery", "warning", "power"],
    ),
    TweakDef(
        id="audio-disable-speech-recognition",
        label="Disable Online Speech Recognition",
        category="Audio",
        apply_fn=_apply_disable_speech_recognition,
        remove_fn=_remove_disable_speech_recognition,
        detect_fn=_detect_disable_speech_recognition,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_KEY_SPEECH],
        description=(
            "Opts out of online speech recognition, preventing voice data "
            "from being sent to Microsoft cloud services."
        ),
        tags=["audio", "speech", "recognition", "privacy"],
    ),
    TweakDef(
        id="audio-disable-enhancements-global",
        label="Disable Audio Enhancements Globally",
        category="Audio",
        apply_fn=_apply_disable_enhancements_global,
        remove_fn=_remove_disable_enhancements_global,
        detect_fn=_detect_disable_enhancements_global,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_AUDIO_GLOBAL],
        description=(
            "Disables all audio processing enhancements system-wide. "
            "Reduces audio latency and prevents driver conflicts. "
            "Default: Enabled. Recommended: Disabled for music production."
        ),
        tags=["audio", "enhancements", "latency", "performance"],
    ),
    TweakDef(
        id="audio-exclusive-priority",
        label="Set Audio Exclusive Mode Priority",
        category="Audio",
        apply_fn=_apply_exclusive_priority,
        remove_fn=_remove_exclusive_priority,
        detect_fn=_detect_exclusive_priority,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_KEY_AUDIO_TASK],
        description=(
            "Sets audio thread scheduling to highest priority. "
            "Reduces audio glitches during heavy CPU load. "
            "Default: Medium. Recommended: High for audio workstations."
        ),
        tags=["audio", "priority", "performance", "latency"],
    ),
]
