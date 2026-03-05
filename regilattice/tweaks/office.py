"""Microsoft Office registry tweaks — supports Office 2010 through 365.

Iterates detected Office versions (14.0 = 2010, 15.0 = 2013, 16.0 = 2016/
2019/365) and applies tweaks to all present installations.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Version-agnostic helpers ─────────────────────────────────────────────────

# All supported Office version keys (oldest → newest)
_VERSIONS = ("14.0", "15.0", "16.0")
_OFFICE_CU = r"HKEY_CURRENT_USER\Software\Microsoft\Office"
_OFFICE_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office"
_OFFICE_COMMON = r"HKEY_CURRENT_USER\Software\Microsoft\Office\Common\ClientTelemetry"
_OFFICE_TELEM_DASH = r"HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\OSM"
_OFFICE_C2R = r"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration"


def _ver_key(ver: str, suffix: str) -> str:
    """Build HKCU Office version-specific key path."""
    return rf"{_OFFICE_CU}\{ver}\Common\{suffix}"


def _ver_policy(ver: str, suffix: str) -> str:
    """Build HKLM Office policy version-specific key path."""
    return rf"{_OFFICE_LM}\{ver}\Common\{suffix}"


def _detected_versions() -> list[str]:
    """Return Office versions actually present in the registry."""
    found = []
    for ver in _VERSIONS:
        key = rf"{_OFFICE_CU}\{ver}\Common"
        if SESSION.key_exists(key):
            found.append(ver)
    # Always include 16.0 for modern M365 which may not write Common early
    if "16.0" not in found:
        found.append("16.0")
    return found


# ── Disable Office Telemetry ────────────────────────────────────────────────


def _apply_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable telemetry (all versions)")
    # Version-common key
    SESSION.backup([_OFFICE_COMMON], "OfficeTelemetry")
    SESSION.set_dword(_OFFICE_COMMON, "DisableTelemetry", 1)
    # Per-version keys
    for ver in _detected_versions():
        feedback = _ver_key(ver, "Feedback")
        privacy = _ver_key(ver, "Privacy")
        policy = _ver_policy(ver, "ClientTelemetry")
        SESSION.backup([feedback, privacy, policy], f"OfficeTelemetry_{ver}")
        SESSION.set_dword(feedback, "Enabled", 0)
        SESSION.set_dword(privacy, "DisconnectedState", 2)
        SESSION.set_dword(privacy, "UserContentDisabled", 2)
        SESSION.set_dword(privacy, "DownloadContentDisabled", 2)
        SESSION.set_dword(privacy, "ControllerConnectedServicesEnabled", 2)
        SESSION.set_dword(policy, "DisableTelemetry", 1)
        SESSION.set_dword(policy, "SendTelemetry", 3)


def _remove_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: restore telemetry defaults (all versions)")
    SESSION.delete_value(_OFFICE_COMMON, "DisableTelemetry")
    for ver in _detected_versions():
        feedback = _ver_key(ver, "Feedback")
        privacy = _ver_key(ver, "Privacy")
        policy = _ver_policy(ver, "ClientTelemetry")
        SESSION.delete_value(feedback, "Enabled")
        for val in (
            "DisconnectedState",
            "UserContentDisabled",
            "DownloadContentDisabled",
            "ControllerConnectedServicesEnabled",
        ):
            SESSION.delete_value(privacy, val)
        SESSION.delete_value(policy, "DisableTelemetry")
        SESSION.delete_value(policy, "SendTelemetry")


def _detect_telemetry() -> bool:
    return SESSION.read_dword(_OFFICE_COMMON, "DisableTelemetry") == 1


# ── Disable Office Start Screen ─────────────────────────────────────────────


def _apply_start_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable start screen (all versions)")
    for ver in _detected_versions():
        general = _ver_key(ver, "General")
        SESSION.backup([general], f"OfficeStartScreen_{ver}")
        SESSION.set_dword(general, "DisableBootToOfficeStart", 1)
        SESSION.set_dword(general, "ShownFirstRunOptin", 1)


def _remove_start_screen(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        general = _ver_key(ver, "General")
        SESSION.delete_value(general, "DisableBootToOfficeStart")
        SESSION.delete_value(general, "ShownFirstRunOptin")


def _detect_start_screen() -> bool:
    return any(SESSION.read_dword(_ver_key(ver, "General"), "DisableBootToOfficeStart") == 1 for ver in _detected_versions())


# ── Disable Office Connected Experiences ─────────────────────────────────────


def _apply_connected(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable connected experiences (all versions)")
    for ver in _detected_versions():
        privacy = _ver_key(ver, "Privacy")
        SESSION.backup([privacy], f"OfficeConnected_{ver}")
        SESSION.set_dword(privacy, "ControllerConnectedServicesEnabled", 2)
        SESSION.set_dword(privacy, "DisconnectedState", 2)


def _remove_connected(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        privacy = _ver_key(ver, "Privacy")
        SESSION.delete_value(privacy, "ControllerConnectedServicesEnabled")
        SESSION.delete_value(privacy, "DisconnectedState")


def _detect_connected() -> bool:
    return any(SESSION.read_dword(_ver_key(ver, "Privacy"), "DisconnectedState") == 2 for ver in _detected_versions())


# ── Disable Office Hardware Acceleration ─────────────────────────────────────


def _apply_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable hardware acceleration (all versions)")
    for ver in _detected_versions():
        graphics = _ver_key(ver, "Graphics")
        SESSION.backup([graphics], f"OfficeHWAccel_{ver}")
        SESSION.set_dword(graphics, "DisableHardwareAcceleration", 1)


def _remove_hwaccel(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        graphics = _ver_key(ver, "Graphics")
        SESSION.delete_value(graphics, "DisableHardwareAcceleration")


def _detect_hwaccel() -> bool:
    return any(SESSION.read_dword(_ver_key(ver, "Graphics"), "DisableHardwareAcceleration") == 1 for ver in _detected_versions())


# ── Disable Office Macro Security (Trust VBA) ───────────────────────────────

_OFFICE_APPS = ("Word", "Excel", "PowerPoint", "Access", "Outlook", "Publisher")


def _apply_macro_trust(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: lower macro security to 'Enable all macros'")
    for ver in _detected_versions():
        for app in _OFFICE_APPS:
            sec = rf"{_OFFICE_CU}\{ver}\{app}\Security"
            SESSION.backup([sec], f"MacroSecurity_{ver}_{app}")
            SESSION.set_dword(sec, "VBAWarnings", 1)  # 1=Enable all
            SESSION.set_dword(sec, "AccessVBOM", 1)


def _remove_macro_trust(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        for app in _OFFICE_APPS:
            sec = rf"{_OFFICE_CU}\{ver}\{app}\Security"
            SESSION.delete_value(sec, "VBAWarnings")
            SESSION.delete_value(sec, "AccessVBOM")


def _detect_macro_trust() -> bool:
    for ver in _detected_versions():
        sec = rf"{_OFFICE_CU}\{ver}\Word\Security"
        if SESSION.read_dword(sec, "VBAWarnings") == 1:
            return True
    return False


# ── Disable Office AutoSave / Increase Recovery Interval ─────────────────────


def _apply_autosave(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: set AutoRecover interval to 2 min, enable save")
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            opts = rf"{_OFFICE_CU}\{ver}\{app}\Options"
            SESSION.backup([opts], f"AutoSave_{ver}_{app}")
            SESSION.set_dword(opts, "SaveInterval", 2)  # minutes


def _remove_autosave(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            opts = rf"{_OFFICE_CU}\{ver}\{app}\Options"
            SESSION.set_dword(opts, "SaveInterval", 10)  # default


def _detect_autosave() -> bool:
    for ver in _detected_versions():
        opts = rf"{_OFFICE_CU}\{ver}\Word\Options"
        if SESSION.read_dword(opts, "SaveInterval") == 2:
            return True
    return False


# ── Disable LinkedIn Integration ───────────────────────────────────────────


def _apply_disable_linkedin(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable LinkedIn integration")
    for ver in _detected_versions():
        linkedin = _ver_key(ver, "LinkedIn")
        SESSION.backup([linkedin], f"OfficeLinkedIn_{ver}")
        SESSION.set_dword(linkedin, "OfficeLinkedIn", 0)


def _remove_disable_linkedin(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        linkedin = _ver_key(ver, "LinkedIn")
        SESSION.delete_value(linkedin, "OfficeLinkedIn")


def _detect_disable_linkedin() -> bool:
    return any(SESSION.read_dword(_ver_key(ver, "LinkedIn"), "OfficeLinkedIn") == 0 for ver in _detected_versions())


# ── Disable Office Animations ──────────────────────────────────────────────


def _apply_disable_office_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable UI animations")
    for ver in _detected_versions():
        graphics = _ver_key(ver, "Graphics")
        SESSION.backup([graphics], f"OfficeAnimations_{ver}")
        SESSION.set_dword(graphics, "DisableAnimations", 1)


def _remove_disable_office_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        graphics = _ver_key(ver, "Graphics")
        SESSION.delete_value(graphics, "DisableAnimations")


def _detect_disable_office_animations() -> bool:
    return any(SESSION.read_dword(_ver_key(ver, "Graphics"), "DisableAnimations") == 1 for ver in _detected_versions())


# ── Disable Office Recent Documents ──────────────────────────────────────────


def _apply_disable_recent_docs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable recent documents list")
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            place = rf"{_OFFICE_CU}\{ver}\{app}\Place MRU"
            SESSION.backup([place], f"RecentDocs_{ver}_{app}")
            SESSION.set_dword(place, "ShowPlaceMRU", 0)
            SESSION.set_dword(place, "MaxDisplay", 0)


def _remove_disable_recent_docs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            place = rf"{_OFFICE_CU}\{ver}\{app}\Place MRU"
            SESSION.delete_value(place, "ShowPlaceMRU")
            SESSION.delete_value(place, "MaxDisplay")


def _detect_disable_recent_docs() -> bool:
    for ver in _detected_versions():
        place = rf"{_OFFICE_CU}\{ver}\Word\Place MRU"
        if SESSION.read_dword(place, "ShowPlaceMRU") == 0:
            return True
    return False


# ── Disable Office Cloud File Storage Prompt ──────────────────────────────────


def _apply_disable_cloud_save(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: default save to local (disable OneDrive/SharePoint prompt)")
    for ver in _detected_versions():
        general = _ver_key(ver, "General")
        SESSION.backup([general], f"CloudSave_{ver}")
        SESSION.set_dword(general, "PreferCloudSaveLocations", 0)
        SESSION.set_dword(general, "SkySaveHost", 0)


def _remove_disable_cloud_save(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        general = _ver_key(ver, "General")
        SESSION.delete_value(general, "PreferCloudSaveLocations")
        SESSION.delete_value(general, "SkySaveHost")


def _detect_disable_cloud_save() -> bool:
    return any(SESSION.read_dword(_ver_key(ver, "General"), "PreferCloudSaveLocations") == 0 for ver in _detected_versions())


# ── Disable Office Feedback Button ───────────────────────────────────────────


def _apply_disable_feedback_button(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable feedback / 'send a smile' button")
    for ver in _detected_versions():
        feedback = _ver_key(ver, "Feedback")
        SESSION.backup([feedback], f"FeedbackBtn_{ver}")
        SESSION.set_dword(feedback, "Enabled", 0)
        SESSION.set_dword(feedback, "IncludeEmail", 0)


def _remove_disable_feedback_button(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        feedback = _ver_key(ver, "Feedback")
        SESSION.delete_value(feedback, "Enabled")
        SESSION.delete_value(feedback, "IncludeEmail")


def _detect_disable_feedback_button() -> bool:
    return any(SESSION.read_dword(_ver_key(ver, "Feedback"), "Enabled") == 0 for ver in _detected_versions())


# ── Disable Office Auto-Updates ──────────────────────────────────────────────

_OFFICE_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0\common\officeupdate"


def _apply_disable_office_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable automatic updates")
    SESSION.backup([_OFFICE_UPDATE], "OfficeUpdate")
    SESSION.set_dword(_OFFICE_UPDATE, "EnableAutomaticUpdates", 0)
    SESSION.set_dword(_OFFICE_UPDATE, "HideEnableDisableUpdates", 1)


def _remove_disable_office_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OFFICE_UPDATE, "EnableAutomaticUpdates")
    SESSION.delete_value(_OFFICE_UPDATE, "HideEnableDisableUpdates")


def _detect_disable_office_updates() -> bool:
    return SESSION.read_dword(_OFFICE_UPDATE, "EnableAutomaticUpdates") == 0


# ── Relax Protected View ──────────────────────────────────────────────────────


def _apply_relax_protected_view(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: relax Protected View for internet/Outlook files")
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            sec = rf"{_OFFICE_CU}\{ver}\{app}\Security\ProtectedView"
            SESSION.backup([sec], f"ProtectedView_{ver}_{app}")
            SESSION.set_dword(sec, "DisableInternetFilesInPV", 1)
            SESSION.set_dword(sec, "DisableAttachementsInPV", 1)
            SESSION.set_dword(sec, "DisableUnsafeLocationsInPV", 1)


def _remove_relax_protected_view(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    for ver in _detected_versions():
        for app in ("Word", "Excel", "PowerPoint"):
            sec = rf"{_OFFICE_CU}\{ver}\{app}\Security\ProtectedView"
            SESSION.delete_value(sec, "DisableInternetFilesInPV")
            SESSION.delete_value(sec, "DisableAttachementsInPV")
            SESSION.delete_value(sec, "DisableUnsafeLocationsInPV")


def _detect_relax_protected_view() -> bool:
    for ver in _detected_versions():
        sec = rf"{_OFFICE_CU}\{ver}\Word\Security\ProtectedView"
        if SESSION.read_dword(sec, "DisableInternetFilesInPV") == 1:
            return True
    return False


# ── Disable Office Telemetry Dashboard ───────────────────────────────────────


def _apply_disable_telemetry_dash(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable telemetry dashboard agent (OSM)")
    SESSION.backup([_OFFICE_TELEM_DASH], "OfficeTelemetryDash")
    SESSION.set_dword(_OFFICE_TELEM_DASH, "Enablelogging", 0)
    SESSION.set_dword(_OFFICE_TELEM_DASH, "EnableUpload", 0)


def _remove_disable_telemetry_dash(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OFFICE_TELEM_DASH, "Enablelogging")
    SESSION.delete_value(_OFFICE_TELEM_DASH, "EnableUpload")


def _detect_disable_telemetry_dash() -> bool:
    return SESSION.read_dword(_OFFICE_TELEM_DASH, "Enablelogging") == 0


# ── Disable Office Background Updates (Click-to-Run) ────────────────────────


def _apply_disable_background_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Office: disable background Click-to-Run updates")
    SESSION.backup([_OFFICE_C2R], "OfficeC2RUpdates")
    SESSION.set_string(_OFFICE_C2R, "UpdatesEnabled", "False")


def _remove_disable_background_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.set_string(_OFFICE_C2R, "UpdatesEnabled", "True")


def _detect_disable_background_updates() -> bool:
    return SESSION.read_string(_OFFICE_C2R, "UpdatesEnabled") == "False"


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-office-telemetry",
        label="Disable Office Telemetry",
        category="Office",
        apply_fn=_apply_telemetry,
        remove_fn=_remove_telemetry,
        detect_fn=_detect_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_OFFICE_COMMON],
        description=(
            "Disables Microsoft Office telemetry, feedback, and connected "
            "services data collection across all installed versions "
            "(2010-365)."
        ),
        tags=["office", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-office-start-screen",
        label="Disable Office Start Screen",
        category="Office",
        apply_fn=_apply_start_screen,
        remove_fn=_remove_start_screen,
        detect_fn=_detect_start_screen,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Common\General"],
        description=(
            "Skips the Office Start screen and opens directly to a blank "
            "document (all versions)."
        ),
        tags=["office", "ux"],
    ),
    TweakDef(
        id="disable-office-connected",
        label="Disable Office Connected Experiences",
        category="Office",
        apply_fn=_apply_connected,
        remove_fn=_remove_connected,
        detect_fn=_detect_connected,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Common\Privacy"],
        description=(
            "Disables cloud-powered Office features (Designer, Editor, "
            "etc.) across all versions."
        ),
        tags=["office", "privacy", "cloud"],
    ),
    TweakDef(
        id="disable-office-hwaccel",
        label="Disable Office Hardware Acceleration",
        category="Office",
        apply_fn=_apply_hwaccel,
        remove_fn=_remove_hwaccel,
        detect_fn=_detect_hwaccel,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Common\Graphics"],
        description=(
            "Disables GPU hardware acceleration in Office apps to fix "
            "display glitches (all versions)."
        ),
        tags=["office", "performance", "gpu"],
    ),
    TweakDef(
        id="office-macro-trust",
        label="Trust VBA Macros (All Apps)",
        category="Office",
        apply_fn=_apply_macro_trust,
        remove_fn=_remove_macro_trust,
        detect_fn=_detect_macro_trust,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Word\Security"],
        description=(
            "Lowers macro security to 'Enable all' and trusts the VBA "
            "project object model (Word, Excel, PowerPoint, Access, "
            "Outlook, Publisher)."
        ),
        tags=["office", "macros", "security"],
    ),
    TweakDef(
        id="office-autosave-fast",
        label="Set Office AutoRecover to 2 min",
        category="Office",
        apply_fn=_apply_autosave,
        remove_fn=_remove_autosave,
        detect_fn=_detect_autosave,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Word\Options"],
        description=(
            "Sets the AutoRecover interval to 2 minutes for Word, Excel, "
            "and PowerPoint (all versions)."
        ),
        tags=["office", "autosave", "recovery"],
    ),
    TweakDef(
        id="disable-office-linkedin",
        label="Disable LinkedIn Integration",
        category="Office",
        apply_fn=_apply_disable_linkedin,
        remove_fn=_remove_disable_linkedin,
        detect_fn=_detect_disable_linkedin,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Common\LinkedIn"],
        description="Disables LinkedIn resume assistant and profile features in Office.",
        tags=["office", "linkedin", "privacy"],
    ),
    TweakDef(
        id="disable-office-animations",
        label="Disable Office UI Animations",
        category="Office",
        apply_fn=_apply_disable_office_animations,
        remove_fn=_remove_disable_office_animations,
        detect_fn=_detect_disable_office_animations,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Common\Graphics"],
        description="Disables transitions and animations in Office apps for snappier UI.",
        tags=["office", "performance", "animations"],
    ),
    TweakDef(
        id="disable-office-recent-docs",
        label="Disable Office Recent Documents",
        category="Office",
        apply_fn=_apply_disable_recent_docs,
        remove_fn=_remove_disable_recent_docs,
        detect_fn=_detect_disable_recent_docs,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Word\Place MRU"],
        description="Hides the recent documents list in Office apps for tidier startup.",
        tags=["office", "privacy", "recent"],
    ),
    TweakDef(
        id="disable-office-cloud-save",
        label="Default Save to Local (Not Cloud)",
        category="Office",
        apply_fn=_apply_disable_cloud_save,
        remove_fn=_remove_disable_cloud_save,
        detect_fn=_detect_disable_cloud_save,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Common\General"],
        description=(
            "Sets Office default save location to local disk instead "
            "of prompting for OneDrive/SharePoint."
        ),
        tags=["office", "save", "cloud", "onedrive"],
    ),
    TweakDef(
        id="disable-office-feedback",
        label="Disable Office Feedback Button",
        category="Office",
        apply_fn=_apply_disable_feedback_button,
        remove_fn=_remove_disable_feedback_button,
        detect_fn=_detect_disable_feedback_button,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Common\Feedback"],
        description="Removes the 'Send a Smile' / feedback button from the Office ribbon.",
        tags=["office", "feedback", "ux"],
    ),
    TweakDef(
        id="disable-office-updates",
        label="Disable Office Auto-Updates",
        category="Office",
        apply_fn=_apply_disable_office_updates,
        remove_fn=_remove_disable_office_updates,
        detect_fn=_detect_disable_office_updates,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_OFFICE_UPDATE],
        description="Disables automatic Office updates so you control when to update.",
        tags=["office", "update", "control"],
    ),
    TweakDef(
        id="relax-office-protected-view",
        label="Relax Office Protected View",
        category="Office",
        apply_fn=_apply_relax_protected_view,
        remove_fn=_remove_relax_protected_view,
        detect_fn=_detect_relax_protected_view,
        needs_admin=False,
        corp_safe=False,
        registry_keys=[rf"{_OFFICE_CU}\16.0\Word\Security\ProtectedView"],
        description=(
            "Disables Protected View for files from the internet and "
            "Outlook attachments. Speeds up opening but reduces security."
        ),
        tags=["office", "security", "protected-view"],
    ),
    TweakDef(
        id="office-disable-telemetry-dash",
        label="Disable Office Telemetry Dashboard",
        category="Office",
        apply_fn=_apply_disable_telemetry_dash,
        remove_fn=_remove_disable_telemetry_dash,
        detect_fn=_detect_disable_telemetry_dash,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_OFFICE_TELEM_DASH],
        description=(
            "Disables Office telemetry dashboard and diagnostic data collection. "
            "Reduces network traffic and CPU from Office telemetry agent. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["office", "telemetry", "privacy", "performance"],
    ),
    TweakDef(
        id="office-disable-background-updates",
        label="Disable Office Background Updates (C2R)",
        category="Office",
        apply_fn=_apply_disable_background_updates,
        remove_fn=_remove_disable_background_updates,
        detect_fn=_detect_disable_background_updates,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_OFFICE_C2R],
        description=(
            "Disables automatic background Office Click-to-Run updates. Updates must "
            "be applied manually through Office or WSUS. Default: Enabled. "
            "Recommended: Disabled for managed environments."
        ),
        tags=["office", "updates", "performance"],
    ),
]


# ── Disable Office Connected Experiences ─────────────────────────────────────

_OFFICE_PRIVACY_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Privacy"


def _apply_disable_connected_exp(*, require_admin: bool = False) -> None:
    SESSION.log("Office: disable optional connected experiences")
    SESSION.backup([_OFFICE_PRIVACY_CU], "OfficeConnectedExp")
    SESSION.set_dword(_OFFICE_PRIVACY_CU, "DisableOptionalConnectedExperiences", 1)


def _remove_disable_connected_exp(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_OFFICE_PRIVACY_CU, "DisableOptionalConnectedExperiences")


def _detect_disable_connected_exp() -> bool:
    return SESSION.read_dword(_OFFICE_PRIVACY_CU, "DisableOptionalConnectedExperiences") == 1


# ── Disable Office Feedback Prompts ──────────────────────────────────────────

_OFFICE_FEEDBACK_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Feedback"


def _apply_disable_feedback_prompts(*, require_admin: bool = False) -> None:
    SESSION.log("Office: disable feedback survey prompts")
    SESSION.backup([_OFFICE_FEEDBACK_CU], "OfficeFeedbackPrompts")
    SESSION.set_dword(_OFFICE_FEEDBACK_CU, "SurveyEnabled", 0)
    SESSION.set_dword(_OFFICE_FEEDBACK_CU, "Enabled", 0)


def _remove_disable_feedback_prompts(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_OFFICE_FEEDBACK_CU, "SurveyEnabled")
    SESSION.delete_value(_OFFICE_FEEDBACK_CU, "Enabled")


def _detect_disable_feedback_prompts() -> bool:
    return (
        SESSION.read_dword(_OFFICE_FEEDBACK_CU, "SurveyEnabled") == 0
        and SESSION.read_dword(_OFFICE_FEEDBACK_CU, "Enabled") == 0
    )


TWEAKS += [
    TweakDef(
        id="office-disable-connected-experiences",
        label="Disable Office Connected Experiences",
        category="Office",
        apply_fn=_apply_disable_connected_exp,
        remove_fn=_remove_disable_connected_exp,
        detect_fn=_detect_disable_connected_exp,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_OFFICE_PRIVACY_CU],
        description=(
            "Disables optional connected experiences in Office 365. "
            "Prevents cloud-powered features like LinkedIn integration and 3D maps. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["office", "connected", "privacy", "cloud"],
    ),
    TweakDef(
        id="office-disable-feedback",
        label="Disable Office Feedback Prompts",
        category="Office",
        apply_fn=_apply_disable_feedback_prompts,
        remove_fn=_remove_disable_feedback_prompts,
        detect_fn=_detect_disable_feedback_prompts,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_OFFICE_FEEDBACK_CU],
        description=(
            "Disables Office feedback survey prompts and the feedback button. "
            "Prevents interruptions during work. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["office", "feedback", "survey", "notifications"],
    ),
]
