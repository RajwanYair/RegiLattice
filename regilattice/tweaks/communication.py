"""Communication & social apps registry tweaks.

Covers: Microsoft Teams, Zoom, Discord, Spotify, Slack.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_RUN = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"
_TEAMS = r"HKEY_CURRENT_USER\Software\Microsoft\Teams"
_ZOOM = r"HKEY_CURRENT_USER\Software\Zoom\Zoom\General"
_ZOOM_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom\General"
_DISCORD = r"HKEY_CURRENT_USER\Software\Discord"
_SPOTIFY = r"HKEY_CURRENT_USER\Software\Spotify"
_TEAMS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams"
_SKYPE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SkypeForBusiness"


# ── Disable Teams Auto-Start ────────────────────────────────────────────────


def _apply_teams_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Teams: disable auto-start")
    SESSION.backup([_RUN, _TEAMS], "TeamsAutoStart")
    SESSION.delete_value(_RUN, "com.squirrel.Teams.Teams")
    SESSION.set_dword(_TEAMS, "DisableAutoStart", 1)


def _remove_teams_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TEAMS, "DisableAutoStart")


def _detect_teams_autostart() -> bool:
    return SESSION.read_dword(_TEAMS, "DisableAutoStart") == 1


# ── Disable Teams GPU Acceleration ──────────────────────────────────────────


def _apply_teams_gpu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Teams: disable GPU hardware acceleration")
    SESSION.backup([_TEAMS], "TeamsGPU")
    SESSION.set_dword(_TEAMS, "disableGpu", 1)


def _remove_teams_gpu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TEAMS, "disableGpu")


def _detect_teams_gpu() -> bool:
    return SESSION.read_dword(_TEAMS, "disableGpu") == 1


# ── Disable Zoom Auto-Update ────────────────────────────────────────────────


def _apply_zoom_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Zoom: disable auto-update")
    SESSION.backup([_ZOOM, _ZOOM_POLICY], "ZoomUpdate")
    SESSION.set_dword(_ZOOM, "EnableAutoUpdate", 0)
    SESSION.set_dword(_ZOOM_POLICY, "DisableAutoUpdate", 1)


def _remove_zoom_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_ZOOM, "EnableAutoUpdate", 1)
    SESSION.delete_value(_ZOOM_POLICY, "DisableAutoUpdate")


def _detect_zoom_update() -> bool:
    return SESSION.read_dword(_ZOOM, "EnableAutoUpdate") == 0


# ── Disable Discord Auto-Start ──────────────────────────────────────────────


def _apply_discord_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Discord: disable auto-start")
    SESSION.backup([_RUN], "DiscordAutoStart")
    SESSION.delete_value(_RUN, "Discord")
    SESSION.set_dword(_DISCORD, "DisableAutoStart", 1)


def _remove_discord_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DISCORD, "DisableAutoStart")


def _detect_discord_autostart() -> bool:
    return SESSION.read_dword(_DISCORD, "DisableAutoStart") == 1


# ── Disable Discord HW Acceleration ─────────────────────────────────────────


def _apply_discord_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Discord: disable hardware acceleration")
    SESSION.backup([_DISCORD], "DiscordHW")
    SESSION.set_dword(_DISCORD, "DANGEROUS_ENABLE_DEVTOOLS_ONLY_ENABLE_IF_YOU_KNOW_WHAT_YOURE_DOING", 0)
    SESSION.set_dword(_DISCORD, "disableHardwareAcceleration", 1)


def _remove_discord_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DISCORD, "disableHardwareAcceleration")


def _detect_discord_hwaccel() -> bool:
    return SESSION.read_dword(_DISCORD, "disableHardwareAcceleration") == 1


# ── Disable Spotify Auto-Start ──────────────────────────────────────────────


def _apply_spotify_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Spotify: disable auto-start")
    SESSION.backup([_RUN, _SPOTIFY], "SpotifyAutoStart")
    SESSION.delete_value(_RUN, "Spotify")
    SESSION.set_dword(_SPOTIFY, "DisableAutoStart", 1)


def _remove_spotify_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPOTIFY, "DisableAutoStart")


def _detect_spotify_autostart() -> bool:
    return SESSION.read_dword(_SPOTIFY, "DisableAutoStart") == 1


# ── Disable Spotify HW Acceleration ─────────────────────────────────────────


def _apply_spotify_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Spotify: disable hardware acceleration")
    SESSION.backup([_SPOTIFY], "SpotifyHW")
    SESSION.set_dword(_SPOTIFY, "ui.hardware_acceleration", 0)


def _remove_spotify_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPOTIFY, "ui.hardware_acceleration")


def _detect_spotify_hwaccel() -> bool:
    return SESSION.read_dword(_SPOTIFY, "ui.hardware_acceleration") == 0


# ── Disable Slack Auto-Start ─────────────────────────────────────────────────

_SLACK = r"HKEY_CURRENT_USER\Software\Slack"


def _apply_slack_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Slack: disable auto-start")
    SESSION.backup([_RUN], "SlackAutoStart")
    SESSION.delete_value(_RUN, "com.squirrel.slack.slack")


def _remove_slack_autostart(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    # Re-enable would need Slack's actual path; just remove our block
    SESSION.log("Slack: re-enable auto-start (manual Slack config may be needed)")


def _detect_slack_autostart() -> bool:
    return SESSION.read_string(_RUN, "com.squirrel.slack.slack") is None


# ── Disable Zoom Background Video ────────────────────────────────────────────


def _apply_zoom_no_video(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Zoom: disable auto-start video in meetings")
    SESSION.backup([_ZOOM], "ZoomVideo")
    SESSION.set_dword(_ZOOM, "NoVideo", 1)


def _remove_zoom_no_video(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ZOOM, "NoVideo")


def _detect_zoom_no_video() -> bool:
    return SESSION.read_dword(_ZOOM, "NoVideo") == 1


# ── Disable Teams Background Effects Telemetry ───────────────────────────────


def _apply_teams_telemetry(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Teams: disable telemetry & diagnostics")
    SESSION.backup([_TEAMS], "TeamsTelemetry")
    SESSION.set_dword(_TEAMS, "disableTelemetry", 1)


def _remove_teams_telemetry(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TEAMS, "disableTelemetry")


def _detect_teams_telemetry() -> bool:
    return SESSION.read_dword(_TEAMS, "disableTelemetry") == 1


# ── Disable Zoom Chat Notifications ──────────────────────────────────────────


def _apply_zoom_mute_chat(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Zoom: mute persistent chat notifications")
    SESSION.backup([_ZOOM], "ZoomChat")
    SESSION.set_dword(_ZOOM, "MuteIMNotification", 1)


def _remove_zoom_mute_chat(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ZOOM, "MuteIMNotification")


def _detect_zoom_mute_chat() -> bool:
    return SESSION.read_dword(_ZOOM, "MuteIMNotification") == 1


# ── Disable Slack HW Acceleration ───────────────────────────────────────────


def _apply_slack_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Slack: disable hardware acceleration")
    SESSION.backup([_SLACK], "SlackHW")
    SESSION.set_dword(_SLACK, "HardwareAccelerationEnabled", 0)


def _remove_slack_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SLACK, "HardwareAccelerationEnabled")


def _detect_slack_hwaccel() -> bool:
    return SESSION.read_dword(_SLACK, "HardwareAccelerationEnabled") == 0


# ── Disable Teams Auto-Start (Policy) ───────────────────────────────────────


def _apply_teams_autostart_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Teams: disable auto-start via machine policy")
    SESSION.backup([_TEAMS_POLICY], "TeamsAutoStartPolicy")
    SESSION.set_dword(_TEAMS_POLICY, "DisableAutoStart", 1)


def _remove_teams_autostart_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TEAMS_POLICY, "DisableAutoStart")


def _detect_teams_autostart_policy() -> bool:
    return SESSION.read_dword(_TEAMS_POLICY, "DisableAutoStart") == 1


# ── Disable Skype Telemetry ─────────────────────────────────────────────────


def _apply_skype_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Skype: disable telemetry and diagnostic data")
    SESSION.backup([_SKYPE_POLICY], "SkypeTelemetry")
    SESSION.set_dword(_SKYPE_POLICY, "DisableTelemetry", 1)


def _remove_skype_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SKYPE_POLICY, "DisableTelemetry")


def _detect_skype_telemetry() -> bool:
    return SESSION.read_dword(_SKYPE_POLICY, "DisableTelemetry") == 1


# ── Disable Typing Insights ──────────────────────────────────────────────────

_INPUT_SETTINGS = r"HKEY_CURRENT_USER\Software\Microsoft\input\Settings"


def _apply_disable_typing_insights(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Communication: disable typing insights/suggestions")
    SESSION.backup([_INPUT_SETTINGS], "TypingInsights")
    SESSION.set_dword(_INPUT_SETTINGS, "InsightsEnabled", 0)


def _remove_disable_typing_insights(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_INPUT_SETTINGS, "InsightsEnabled", 1)


def _detect_disable_typing_insights() -> bool:
    return SESSION.read_dword(_INPUT_SETTINGS, "InsightsEnabled") == 0


# ── Disable Online Speech Recognition ────────────────────────────────────────

_SPEECH_PRIVACY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore"
    r"\Settings\OnlineSpeechPrivacy"
)


def _apply_disable_online_speech(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Communication: disable online speech recognition")
    SESSION.backup([_SPEECH_PRIVACY], "OnlineSpeech")
    SESSION.set_dword(_SPEECH_PRIVACY, "HasAccepted", 0)


def _remove_disable_online_speech(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_SPEECH_PRIVACY, "HasAccepted", 1)


def _detect_disable_online_speech() -> bool:
    return SESSION.read_dword(_SPEECH_PRIVACY, "HasAccepted") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="comm-disable-teams-autostart-user",
        label="Disable Teams Auto-Start",
        category="Communication",
        apply_fn=_apply_teams_autostart,
        remove_fn=_remove_teams_autostart,
        detect_fn=_detect_teams_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN, _TEAMS],
        description="Prevents Microsoft Teams from starting automatically at login.",
        tags=["teams", "autostart", "startup"],
    ),
    TweakDef(
        id="comm-disable-teams-gpu",
        label="Disable Teams GPU Acceleration",
        category="Communication",
        apply_fn=_apply_teams_gpu,
        remove_fn=_remove_teams_gpu,
        detect_fn=_detect_teams_gpu,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TEAMS],
        description="Disables GPU hardware acceleration in Teams to reduce resource usage.",
        tags=["teams", "performance", "gpu"],
    ),
    TweakDef(
        id="comm-disable-zoom-autoupdate",
        label="Disable Zoom Auto-Update",
        category="Communication",
        apply_fn=_apply_zoom_update,
        remove_fn=_remove_zoom_update,
        detect_fn=_detect_zoom_update,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_ZOOM, _ZOOM_POLICY],
        description="Disables Zoom's automatic update mechanism.",
        tags=["zoom", "update"],
    ),
    TweakDef(
        id="comm-disable-discord-autostart",
        label="Disable Discord Auto-Start",
        category="Communication",
        apply_fn=_apply_discord_autostart,
        remove_fn=_remove_discord_autostart,
        detect_fn=_detect_discord_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN, _DISCORD],
        description="Prevents Discord from starting automatically at login.",
        tags=["discord", "autostart", "startup"],
    ),
    TweakDef(
        id="comm-disable-discord-hwaccel",
        label="Disable Discord HW Acceleration",
        category="Communication",
        apply_fn=_apply_discord_hwaccel,
        remove_fn=_remove_discord_hwaccel,
        detect_fn=_detect_discord_hwaccel,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_DISCORD],
        description="Disables hardware acceleration in Discord to reduce GPU usage.",
        tags=["discord", "performance", "gpu"],
    ),
    TweakDef(
        id="comm-disable-spotify-autostart",
        label="Disable Spotify Auto-Start",
        category="Communication",
        apply_fn=_apply_spotify_autostart,
        remove_fn=_remove_spotify_autostart,
        detect_fn=_detect_spotify_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN, _SPOTIFY],
        description="Prevents Spotify from starting automatically at login.",
        tags=["spotify", "autostart", "startup"],
    ),
    TweakDef(
        id="comm-disable-spotify-hwaccel",
        label="Disable Spotify HW Acceleration",
        category="Communication",
        apply_fn=_apply_spotify_hwaccel,
        remove_fn=_remove_spotify_hwaccel,
        detect_fn=_detect_spotify_hwaccel,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SPOTIFY],
        description="Disables hardware acceleration in Spotify.",
        tags=["spotify", "performance", "gpu"],
    ),
    TweakDef(
        id="comm-disable-slack-autostart",
        label="Disable Slack Auto-Start",
        category="Communication",
        apply_fn=_apply_slack_autostart,
        remove_fn=_remove_slack_autostart,
        detect_fn=_detect_slack_autostart,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_RUN],
        description="Prevents Slack from starting automatically at login.",
        tags=["slack", "autostart", "startup"],
    ),
    TweakDef(
        id="comm-disable-zoom-auto-video",
        label="Disable Zoom Auto-Start Video",
        category="Communication",
        apply_fn=_apply_zoom_no_video,
        remove_fn=_remove_zoom_no_video,
        detect_fn=_detect_zoom_no_video,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ZOOM],
        description="Prevents Zoom from automatically enabling video when joining meetings.",
        tags=["zoom", "video", "privacy"],
    ),
    TweakDef(
        id="comm-disable-teams-telemetry",
        label="Disable Teams Telemetry",
        category="Communication",
        apply_fn=_apply_teams_telemetry,
        remove_fn=_remove_teams_telemetry,
        detect_fn=_detect_teams_telemetry,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TEAMS],
        description="Disables Microsoft Teams telemetry and diagnostic data collection.",
        tags=["teams", "telemetry", "privacy"],
    ),
    TweakDef(
        id="comm-disable-zoom-chat-notify",
        label="Mute Zoom Chat Notifications",
        category="Communication",
        apply_fn=_apply_zoom_mute_chat,
        remove_fn=_remove_zoom_mute_chat,
        detect_fn=_detect_zoom_mute_chat,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ZOOM],
        description="Mutes persistent chat notifications in Zoom.",
        tags=["zoom", "chat", "notifications"],
    ),
    TweakDef(
        id="comm-disable-slack-hwaccel",
        label="Disable Slack HW Acceleration",
        category="Communication",
        apply_fn=_apply_slack_hwaccel,
        remove_fn=_remove_slack_hwaccel,
        detect_fn=_detect_slack_hwaccel,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SLACK],
        description="Disables hardware acceleration in Slack desktop client.",
        tags=["slack", "performance", "gpu"],
    ),
    TweakDef(
        id="comm-disable-teams-autostart",
        label="Disable Teams Auto-Start (Policy)",
        category="Communication",
        apply_fn=_apply_teams_autostart_policy,
        remove_fn=_remove_teams_autostart_policy,
        detect_fn=_detect_teams_autostart_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TEAMS_POLICY],
        description=(
            "Prevents Microsoft Teams from starting automatically at login. "
            "Reduces boot time and memory usage. Default: Auto-start. Recommended: Disabled."
        ),
        tags=["communication", "teams", "startup", "performance"],
    ),
    TweakDef(
        id="comm-disable-skype-telemetry",
        label="Disable Skype Telemetry",
        category="Communication",
        apply_fn=_apply_skype_telemetry,
        remove_fn=_remove_skype_telemetry,
        detect_fn=_detect_skype_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SKYPE_POLICY],
        description=("Disables Skype for Business telemetry and diagnostic data collection. Default: Enabled. Recommended: Disabled."),
        tags=["communication", "skype", "telemetry", "privacy"],
    ),
    TweakDef(
        id="comm-disable-typing-insights",
        label="Disable Typing Insights",
        category="Communication",
        apply_fn=_apply_disable_typing_insights,
        remove_fn=_remove_disable_typing_insights,
        detect_fn=_detect_disable_typing_insights,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_INPUT_SETTINGS],
        description=(
            "Disables typing insights and suggestions that analyze your typing patterns. Improves privacy. Default: Enabled. Recommended: Disabled."
        ),
        tags=["communication", "typing", "insights", "privacy"],
    ),
    TweakDef(
        id="comm-disable-online-speech",
        label="Disable Online Speech Recognition",
        category="Communication",
        apply_fn=_apply_disable_online_speech,
        remove_fn=_remove_disable_online_speech,
        detect_fn=_detect_disable_online_speech,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SPEECH_PRIVACY],
        description=(
            "Disables online speech recognition that sends voice data "
            "to Microsoft for processing. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["communication", "speech", "voice", "privacy"],
    ),
]


# ── Block Teams First Launch After Install ───────────────────────────────────

_TEAMS_OFFICE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Teams"
_SKYPE_TELEMETRY_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Skype\Telemetry"


def _apply_teams_first_launch_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Prevent Teams first launch after install")
    SESSION.backup([_TEAMS_OFFICE_POLICY], "TeamsFirstLaunch")
    SESSION.set_dword(_TEAMS_OFFICE_POLICY, "PreventFirstLaunchAfterInstall", 1)


def _remove_teams_first_launch_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TEAMS_OFFICE_POLICY, "PreventFirstLaunchAfterInstall")


def _detect_teams_first_launch_off() -> bool:
    return SESSION.read_dword(_TEAMS_OFFICE_POLICY, "PreventFirstLaunchAfterInstall") == 1


# ── Disable Skype Telemetry (User) ──────────────────────────────────────────


def _apply_skype_telemetry_off_cu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Skype telemetry (HKCU)")
    SESSION.backup([_SKYPE_TELEMETRY_CU], "SkypeTelemetryCU")
    SESSION.set_dword(_SKYPE_TELEMETRY_CU, "Enabled", 0)


def _remove_skype_telemetry_off_cu(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SKYPE_TELEMETRY_CU, "Enabled")


def _detect_skype_telemetry_off_cu() -> bool:
    return SESSION.read_dword(_SKYPE_TELEMETRY_CU, "Enabled") == 0


TWEAKS += [
    TweakDef(
        id="comm-prevent-teams-first-launch",
        label="Prevent Teams Auto-Start After Install",
        category="Communication",
        apply_fn=_apply_teams_first_launch_off,
        remove_fn=_remove_teams_first_launch_off,
        detect_fn=_detect_teams_first_launch_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_TEAMS_OFFICE_POLICY],
        description=(
            "Prevents Microsoft Teams from automatically launching after Office installation via policy. Default: Auto-launch. Recommended: Disabled."
        ),
        tags=["communication", "teams", "autostart", "office", "policy"],
    ),
    TweakDef(
        id="comm-disable-skype-telemetry-user",
        label="Disable Skype Telemetry (User)",
        category="Communication",
        apply_fn=_apply_skype_telemetry_off_cu,
        remove_fn=_remove_skype_telemetry_off_cu,
        detect_fn=_detect_skype_telemetry_off_cu,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SKYPE_TELEMETRY_CU],
        description=(
            "Disables Skype desktop telemetry data collection at the "
            "user level. Reduces background data transmission. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["communication", "skype", "telemetry", "privacy", "user"],
    ),
]


# ── Disable Teams Background Activity ────────────────────────────────────────


def _apply_teams_background_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Teams: disable background activity on login")
    SESSION.backup([_TEAMS], "TeamsBackgroundOff")
    SESSION.set_dword(_TEAMS, "OpenAsHidden", 0)


def _remove_teams_background_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TEAMS, "OpenAsHidden")


def _detect_teams_background_off() -> bool:
    return SESSION.read_dword(_TEAMS, "OpenAsHidden") == 0


# ── Disable Skype Feedback Survey ────────────────────────────────────────────

_SKYPE_FEEDBACK = r"HKEY_CURRENT_USER\Software\Microsoft\Skype\Feedback"


def _apply_skype_feedback_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Skype: disable feedback survey prompts")
    SESSION.backup([_SKYPE_FEEDBACK], "SkypeFeedbackOff")
    SESSION.set_dword(_SKYPE_FEEDBACK, "SurveyEnabled", 0)


def _remove_skype_feedback_off(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SKYPE_FEEDBACK, "SurveyEnabled")


def _detect_skype_feedback_off() -> bool:
    return SESSION.read_dword(_SKYPE_FEEDBACK, "SurveyEnabled") == 0


# ── Block Zoom Auto-Update (Policy) ─────────────────────────────────────────


def _apply_zoom_auto_update_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Zoom: block auto-update via machine policy")
    SESSION.backup([_ZOOM_POLICY], "ZoomAutoUpdateOff")
    SESSION.set_dword(_ZOOM_POLICY, "DisableAutoUpdate", 1)


def _remove_zoom_auto_update_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ZOOM_POLICY, "DisableAutoUpdate")


def _detect_zoom_auto_update_off() -> bool:
    return SESSION.read_dword(_ZOOM_POLICY, "DisableAutoUpdate") == 1


TWEAKS += [
    TweakDef(
        id="comm-disable-teams-background",
        label="Disable Teams Background Activity on Login",
        category="Communication",
        apply_fn=_apply_teams_background_off,
        remove_fn=_remove_teams_background_off,
        detect_fn=_detect_teams_background_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TEAMS],
        description=(
            "Prevents Microsoft Teams from running hidden in the background "
            "on login by setting OpenAsHidden to 0. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["communication", "teams", "background", "login", "performance"],
    ),
    TweakDef(
        id="comm-disable-skype-feedback",
        label="Disable Skype Feedback Survey Prompts",
        category="Communication",
        apply_fn=_apply_skype_feedback_off,
        remove_fn=_remove_skype_feedback_off,
        detect_fn=_detect_skype_feedback_off,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SKYPE_FEEDBACK],
        description=("Disables Skype feedback survey prompts and data collection at the user level. Default: Enabled. Recommended: Disabled."),
        tags=["communication", "skype", "feedback", "privacy", "survey"],
    ),
    TweakDef(
        id="comm-block-zoom-auto-update",
        label="Block Zoom Auto-Update (Policy)",
        category="Communication",
        apply_fn=_apply_zoom_auto_update_off,
        remove_fn=_remove_zoom_auto_update_off,
        detect_fn=_detect_zoom_auto_update_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_ZOOM_POLICY],
        description=(
            "Blocks Zoom desktop client from automatically updating "
            "via machine-level group policy. "
            "Default: Auto-update enabled. Recommended: Disabled for managed environments."
        ),
        tags=["communication", "zoom", "update", "policy", "managed"],
    ),
]
