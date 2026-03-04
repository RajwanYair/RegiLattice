"""Communication & social apps registry tweaks.

Covers: Microsoft Teams, Zoom, Discord, Spotify, Slack.
"""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_RUN = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"
_TEAMS = r"HKEY_CURRENT_USER\Software\Microsoft\Teams"
_TEAMS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams"
_ZOOM = r"HKEY_CURRENT_USER\Software\Zoom\Zoom\General"
_ZOOM_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Zoom\Zoom\General"
_DISCORD = r"HKEY_CURRENT_USER\Software\Discord"
_SPOTIFY = r"HKEY_CURRENT_USER\Software\Spotify"
_SLACK = r"HKEY_CURRENT_USER\Software\Slack"


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


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-teams-autostart",
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
        id="disable-teams-gpu",
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
        id="disable-zoom-autoupdate",
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
        id="disable-discord-autostart",
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
        id="disable-discord-hwaccel",
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
        id="disable-spotify-autostart",
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
        id="disable-spotify-hwaccel",
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
]
