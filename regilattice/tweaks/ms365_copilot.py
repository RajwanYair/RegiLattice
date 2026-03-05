"""Microsoft 365 Copilot tweaks — manage M365 Copilot features in Office apps.

Controls Microsoft 365 Copilot (the AI assistant embedded in Word, Excel,
PowerPoint, Outlook, Teams) via Group Policy / registry keys.  Separate from
the Windows Copilot tweaks in ``copilot.py``.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_OFFICE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\office\16.0"
_OFFICE_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\office\16.0"
_COPILOT_POLICY = rf"{_OFFICE_POLICY}\common\officecopilot"
_COPILOT_CU = rf"{_OFFICE_CU}\common\officecopilot"
_OUTLOOK_POLICY = rf"{_OFFICE_POLICY}\outlook\options\mail"
_TEAMS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Teams"
_TEAMS_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Teams"
_WORD_COPILOT = rf"{_OFFICE_POLICY}\word\options"
_EXCEL_COPILOT = rf"{_OFFICE_POLICY}\excel\options"
_PPT_COPILOT = rf"{_OFFICE_POLICY}\powerpoint\options"
_M365_COAUTH = rf"{_OFFICE_POLICY}\coauth"
_M365_CONNECTED = rf"{_OFFICE_POLICY}\common\privacy"
_M365_CONNECTED_CU = rf"{_OFFICE_CU}\common\privacy"
_LOOP_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Loop"
_COPILOT_WEB = rf"{_OFFICE_POLICY}\common\copilotwebsearch"
_COPILOT_PLUGINS = rf"{_OFFICE_POLICY}\common\copilotplugins"


# ── Disable M365 Copilot (master switch) ────────────────────────────────────

_DISABLE_KEYS = [_COPILOT_POLICY, _COPILOT_CU]


def _apply_disable_m365_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Office Copilot globally")
    SESSION.backup(_DISABLE_KEYS, "M365Copilot")
    SESSION.set_dword(_COPILOT_POLICY, "EnableCopilot", 0)
    SESSION.set_dword(_COPILOT_CU, "EnableCopilot", 0)


def _remove_disable_m365_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_POLICY, "EnableCopilot")
    SESSION.delete_value(_COPILOT_CU, "EnableCopilot")


def _detect_disable_m365_copilot() -> bool:
    val = SESSION.read_dword(_COPILOT_POLICY, "EnableCopilot")
    return val == 0


# ── Disable Copilot Web Search ──────────────────────────────────────────────

_WEB_KEYS = [_COPILOT_WEB]


def _apply_disable_copilot_web_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable web search from Copilot")
    SESSION.backup(_WEB_KEYS, "M365CopilotWeb")
    SESSION.set_dword(_COPILOT_WEB, "DisableCopilotWebSearch", 1)


def _remove_disable_copilot_web_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_WEB, "DisableCopilotWebSearch")


def _detect_disable_copilot_web_search() -> bool:
    return SESSION.read_dword(_COPILOT_WEB, "DisableCopilotWebSearch") == 1


# ── Disable Copilot Plugins / Extensions ────────────────────────────────────

_PLUGIN_KEYS = [_COPILOT_PLUGINS]


def _apply_disable_copilot_plugins(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable third-party Copilot plugins")
    SESSION.backup(_PLUGIN_KEYS, "M365CopilotPlugins")
    SESSION.set_dword(_COPILOT_PLUGINS, "DisableCopilotPlugins", 1)


def _remove_disable_copilot_plugins(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_PLUGINS, "DisableCopilotPlugins")


def _detect_disable_copilot_plugins() -> bool:
    return SESSION.read_dword(_COPILOT_PLUGINS, "DisableCopilotPlugins") == 1


# ── Disable Copilot in Outlook ──────────────────────────────────────────────

_OUTLOOK_KEYS = [_OUTLOOK_POLICY]


def _apply_disable_copilot_outlook(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Copilot in Outlook")
    SESSION.backup(_OUTLOOK_KEYS, "M365CopilotOutlook")
    SESSION.set_dword(_OUTLOOK_POLICY, "DisableCopilotOutlook", 1)


def _remove_disable_copilot_outlook(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OUTLOOK_POLICY, "DisableCopilotOutlook")


def _detect_disable_copilot_outlook() -> bool:
    return SESSION.read_dword(_OUTLOOK_POLICY, "DisableCopilotOutlook") == 1


# ── Disable Copilot in Teams ────────────────────────────────────────────────

_TEAMS_KEYS = [_TEAMS_POLICY, _TEAMS_CU]


def _apply_disable_copilot_teams(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Copilot in Teams")
    SESSION.backup(_TEAMS_KEYS, "M365CopilotTeams")
    SESSION.set_dword(_TEAMS_POLICY, "DisableCopilot", 1)
    SESSION.set_dword(_TEAMS_CU, "DisableCopilot", 1)


def _remove_disable_copilot_teams(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TEAMS_POLICY, "DisableCopilot")
    SESSION.delete_value(_TEAMS_CU, "DisableCopilot")


def _detect_disable_copilot_teams() -> bool:
    return SESSION.read_dword(_TEAMS_POLICY, "DisableCopilot") == 1


# ── Disable Copilot in Word ─────────────────────────────────────────────────

_WORD_KEYS = [_WORD_COPILOT]


def _apply_disable_copilot_word(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Copilot in Word")
    SESSION.backup(_WORD_KEYS, "M365CopilotWord")
    SESSION.set_dword(_WORD_COPILOT, "DisableCopilotWord", 1)


def _remove_disable_copilot_word(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WORD_COPILOT, "DisableCopilotWord")


def _detect_disable_copilot_word() -> bool:
    return SESSION.read_dword(_WORD_COPILOT, "DisableCopilotWord") == 1


# ── Disable Copilot in Excel ────────────────────────────────────────────────

_EXCEL_KEYS = [_EXCEL_COPILOT]


def _apply_disable_copilot_excel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Copilot in Excel")
    SESSION.backup(_EXCEL_KEYS, "M365CopilotExcel")
    SESSION.set_dword(_EXCEL_COPILOT, "DisableCopilotExcel", 1)


def _remove_disable_copilot_excel(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EXCEL_COPILOT, "DisableCopilotExcel")


def _detect_disable_copilot_excel() -> bool:
    return SESSION.read_dword(_EXCEL_COPILOT, "DisableCopilotExcel") == 1


# ── Disable Copilot in PowerPoint ───────────────────────────────────────────

_PPT_KEYS = [_PPT_COPILOT]


def _apply_disable_copilot_ppt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Copilot in PowerPoint")
    SESSION.backup(_PPT_KEYS, "M365CopilotPPT")
    SESSION.set_dword(_PPT_COPILOT, "DisableCopilotPowerPoint", 1)


def _remove_disable_copilot_ppt(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_PPT_COPILOT, "DisableCopilotPowerPoint")


def _detect_disable_copilot_ppt() -> bool:
    return SESSION.read_dword(_PPT_COPILOT, "DisableCopilotPowerPoint") == 1


# ── Disable Connected Experiences ───────────────────────────────────────────

_CONNECTED_KEYS = [_M365_CONNECTED, _M365_CONNECTED_CU]


def _apply_disable_connected_experiences(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable connected experiences")
    SESSION.backup(_CONNECTED_KEYS, "M365Connected")
    SESSION.set_dword(_M365_CONNECTED, "DisconnectedState", 2)
    SESSION.set_dword(_M365_CONNECTED, "ControllerConnectedServicesEnabled", 2)
    SESSION.set_dword(_M365_CONNECTED_CU, "DisconnectedState", 2)
    SESSION.set_dword(_M365_CONNECTED_CU, "ControllerConnectedServicesEnabled", 2)


def _remove_disable_connected_experiences(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    for key in (_M365_CONNECTED, _M365_CONNECTED_CU):
        SESSION.delete_value(key, "DisconnectedState")
        SESSION.delete_value(key, "ControllerConnectedServicesEnabled")


def _detect_disable_connected_experiences() -> bool:
    return SESSION.read_dword(_M365_CONNECTED, "DisconnectedState") == 2


# ── Disable Microsoft Loop ─────────────────────────────────────────────────

_LOOP_KEYS = [_LOOP_POLICY]


def _apply_disable_loop(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Microsoft Loop")
    SESSION.backup(_LOOP_KEYS, "M365Loop")
    SESSION.set_dword(_LOOP_POLICY, "DisableLoop", 1)


def _remove_disable_loop(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_LOOP_POLICY, "DisableLoop")


def _detect_disable_loop() -> bool:
    return SESSION.read_dword(_LOOP_POLICY, "DisableLoop") == 1


# ── Disable Real-Time Co-Authoring Telemetry ────────────────────────────────

_COAUTH_KEYS = [_M365_COAUTH]


def _apply_disable_coauth_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable co-authoring telemetry")
    SESSION.backup(_COAUTH_KEYS, "M365CoAuth")
    SESSION.set_dword(_M365_COAUTH, "DisableCoAuthTelemetry", 1)


def _remove_disable_coauth_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_M365_COAUTH, "DisableCoAuthTelemetry")


def _detect_disable_coauth_telemetry() -> bool:
    return SESSION.read_dword(_M365_COAUTH, "DisableCoAuthTelemetry") == 1


# ── Disable Copilot Data Collection ─────────────────────────────────────────

_DATA_KEYS = [_COPILOT_POLICY]


def _apply_disable_copilot_data(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Copilot data collection")
    SESSION.backup(_DATA_KEYS, "M365CopilotData")
    SESSION.set_dword(_COPILOT_POLICY, "DisableCopilotDataCollection", 1)


def _remove_disable_copilot_data(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_POLICY, "DisableCopilotDataCollection")


def _detect_disable_copilot_data() -> bool:
    return SESSION.read_dword(_COPILOT_POLICY, "DisableCopilotDataCollection") == 1


# ── Disable Copilot Auto-Suggestions ────────────────────────────────────────

_AUTOSUGGEST_KEYS = [_COPILOT_POLICY]


def _apply_disable_copilot_autosuggest(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable auto-suggestions")
    SESSION.backup(_AUTOSUGGEST_KEYS, "M365CopilotAutoSuggest")
    SESSION.set_dword(_COPILOT_POLICY, "DisableCopilotAutoSuggestions", 1)


def _remove_disable_copilot_autosuggest(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_POLICY, "DisableCopilotAutoSuggestions")


def _detect_disable_copilot_autosuggest() -> bool:
    return SESSION.read_dword(_COPILOT_POLICY, "DisableCopilotAutoSuggestions") == 1


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="m365-disable-copilot",
        label="Disable M365 Copilot (Master Switch)",
        category="M365 Copilot",
        apply_fn=_apply_disable_m365_copilot,
        remove_fn=_remove_disable_m365_copilot,
        detect_fn=_detect_disable_m365_copilot,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_DISABLE_KEYS,
        description=(
            "Disables Microsoft 365 Copilot globally via Office policy. "
            "Prevents the AI assistant from appearing in Word, Excel, "
            "PowerPoint, and Outlook. Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["m365", "copilot", "office", "ai", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-web-search",
        label="Disable M365 Copilot Web Search",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_web_search,
        remove_fn=_remove_disable_copilot_web_search,
        detect_fn=_detect_disable_copilot_web_search,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_WEB_KEYS,
        description=(
            "Prevents M365 Copilot from using Bing web search to enhance responses. "
            "Keeps Copilot responses limited to local/org data only. "
            "Default: Enabled. Recommended: Disabled for data privacy."
        ),
        tags=["m365", "copilot", "web", "search", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-plugins",
        label="Disable M365 Copilot Plugins",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_plugins,
        remove_fn=_remove_disable_copilot_plugins,
        detect_fn=_detect_disable_copilot_plugins,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_PLUGIN_KEYS,
        description=(
            "Blocks third-party Copilot plugins and extensions in M365 apps. "
            "Reduces attack surface and data exposure. "
            "Default: Enabled. Recommended: Disabled in enterprise."
        ),
        tags=["m365", "copilot", "plugins", "extensions", "security"],
    ),
    TweakDef(
        id="m365-disable-copilot-outlook",
        label="Disable M365 Copilot in Outlook",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_outlook,
        remove_fn=_remove_disable_copilot_outlook,
        detect_fn=_detect_disable_copilot_outlook,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_OUTLOOK_KEYS,
        description=(
            "Disables Copilot AI features in Microsoft Outlook including "
            "email summarization and draft suggestions. "
            "Default: Enabled. Recommended: Disabled for email privacy."
        ),
        tags=["m365", "copilot", "outlook", "email", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-teams",
        label="Disable M365 Copilot in Teams",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_teams,
        remove_fn=_remove_disable_copilot_teams,
        detect_fn=_detect_disable_copilot_teams,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_TEAMS_KEYS,
        description=(
            "Disables Copilot AI features in Microsoft Teams including "
            "meeting summaries and chat suggestions. "
            "Default: Enabled. Recommended: Disabled for meeting privacy."
        ),
        tags=["m365", "copilot", "teams", "meetings", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-word",
        label="Disable M365 Copilot in Word",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_word,
        remove_fn=_remove_disable_copilot_word,
        detect_fn=_detect_disable_copilot_word,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_WORD_KEYS,
        description=(
            "Disables Copilot AI features in Microsoft Word including "
            "text generation and rewriting. "
            "Default: Enabled. Recommended: Disabled for document privacy."
        ),
        tags=["m365", "copilot", "word", "documents", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-excel",
        label="Disable M365 Copilot in Excel",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_excel,
        remove_fn=_remove_disable_copilot_excel,
        detect_fn=_detect_disable_copilot_excel,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_EXCEL_KEYS,
        description=(
            "Disables Copilot AI features in Microsoft Excel including "
            "formula generation and data analysis. "
            "Default: Enabled. Recommended: Disabled for spreadsheet privacy."
        ),
        tags=["m365", "copilot", "excel", "data", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-ppt",
        label="Disable M365 Copilot in PowerPoint",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_ppt,
        remove_fn=_remove_disable_copilot_ppt,
        detect_fn=_detect_disable_copilot_ppt,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_PPT_KEYS,
        description=(
            "Disables Copilot AI features in PowerPoint including "
            "slide generation and presentation summaries. "
            "Default: Enabled. Recommended: Disabled for presentation privacy."
        ),
        tags=["m365", "copilot", "powerpoint", "presentations", "privacy"],
    ),
    TweakDef(
        id="m365-disable-connected-experiences",
        label="Disable M365 Connected Experiences",
        category="M365 Copilot",
        apply_fn=_apply_disable_connected_experiences,
        remove_fn=_remove_disable_connected_experiences,
        detect_fn=_detect_disable_connected_experiences,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_CONNECTED_KEYS,
        description=(
            "Disables Office connected experiences that send data to Microsoft "
            "cloud for analysis, including Copilot prerequisites. "
            "Default: Enabled. Recommended: Disabled for maximum privacy."
        ),
        tags=["m365", "copilot", "connected", "cloud", "privacy", "telemetry"],
    ),
    TweakDef(
        id="m365-disable-loop",
        label="Disable Microsoft Loop",
        category="M365 Copilot",
        apply_fn=_apply_disable_loop,
        remove_fn=_remove_disable_loop,
        detect_fn=_detect_disable_loop,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_LOOP_KEYS,
        description=(
            "Disables Microsoft Loop, the collaborative workspace that "
            "integrates with M365 Copilot for real-time AI-assisted editing. "
            "Default: Enabled. Recommended: Disabled if not needed."
        ),
        tags=["m365", "loop", "collaboration", "ai"],
    ),
    TweakDef(
        id="m365-disable-coauth-telemetry",
        label="Disable M365 Co-Authoring Telemetry",
        category="M365 Copilot",
        apply_fn=_apply_disable_coauth_telemetry,
        remove_fn=_remove_disable_coauth_telemetry,
        detect_fn=_detect_disable_coauth_telemetry,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_COAUTH_KEYS,
        description=(
            "Disables telemetry from real-time co-authoring sessions. "
            "Reduces data sent to Microsoft during collaborative editing. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["m365", "coauth", "telemetry", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-data-collection",
        label="Disable M365 Copilot Data Collection",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_data,
        remove_fn=_remove_disable_copilot_data,
        detect_fn=_detect_disable_copilot_data,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_DATA_KEYS,
        description=(
            "Prevents M365 Copilot from collecting interaction data for "
            "model training and improvement. "
            "Default: Enabled. Recommended: Disabled for data sovereignty."
        ),
        tags=["m365", "copilot", "data", "collection", "privacy"],
    ),
    TweakDef(
        id="m365-disable-copilot-autosuggest",
        label="Disable M365 Copilot Auto-Suggestions",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_autosuggest,
        remove_fn=_remove_disable_copilot_autosuggest,
        detect_fn=_detect_disable_copilot_autosuggest,
        needs_admin=True,
        corp_safe=True,
        registry_keys=_AUTOSUGGEST_KEYS,
        description=(
            "Disables automatic Copilot suggestions that appear while "
            "typing in Office apps. Reduces AI interruptions. "
            "Default: Enabled. Recommended: Disabled for focus."
        ),
        tags=["m365", "copilot", "autosuggest", "ux"],
    ),
]

# -- Key paths (HKCU-only M365 tweaks) ----------------------------------------

_OFFICEGRAPH_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\OfficeGraph"
_FLUID_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Office\16.0\Common\Fluid"


# -- Disable M365 Copilot Web Access (HKCU) -----------------------------------


def _apply_disable_copilot_web_cu(*, require_admin: bool = False) -> None:
    SESSION.log("M365 Copilot: disable Copilot web access (HKCU)")
    SESSION.backup([_OFFICEGRAPH_CU], "M365CopilotWebCU")
    SESSION.set_dword(_OFFICEGRAPH_CU, "DisableGraphOnline", 1)


def _remove_disable_copilot_web_cu(*, require_admin: bool = False) -> None:
    SESSION.backup([_OFFICEGRAPH_CU], "M365CopilotWebCU_Remove")
    SESSION.delete_value(_OFFICEGRAPH_CU, "DisableGraphOnline")


def _detect_disable_copilot_web_cu() -> bool:
    return SESSION.read_dword(_OFFICEGRAPH_CU, "DisableGraphOnline") == 1


# -- Disable Loop Components in M365 (HKCU) -----------------------------------


def _apply_disable_loop_components_cu(*, require_admin: bool = False) -> None:
    SESSION.log("M365 Copilot: disable Loop components (HKCU)")
    SESSION.backup([_FLUID_CU], "M365LoopComponentsCU")
    SESSION.set_dword(_FLUID_CU, "LoopEnabled", 0)


def _remove_disable_loop_components_cu(*, require_admin: bool = False) -> None:
    SESSION.backup([_FLUID_CU], "M365LoopComponentsCU_Remove")
    SESSION.delete_value(_FLUID_CU, "LoopEnabled")


def _detect_disable_loop_components_cu() -> bool:
    return SESSION.read_dword(_FLUID_CU, "LoopEnabled") == 0


TWEAKS += [
    TweakDef(
        id="m365-disable-copilot-web",
        label="Disable M365 Copilot Web Access (User)",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_web_cu,
        remove_fn=_remove_disable_copilot_web_cu,
        detect_fn=_detect_disable_copilot_web_cu,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_OFFICEGRAPH_CU],
        description=(
            "Disables M365 Copilot web/graph access at the user level via OfficeGraph policy. "
            "Prevents Copilot from reaching external data sources. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["m365", "copilot", "web", "privacy", "officegraph"],
    ),
    TweakDef(
        id="m365-disable-loop-components",
        label="Disable Loop Components in M365 (User)",
        category="M365 Copilot",
        apply_fn=_apply_disable_loop_components_cu,
        remove_fn=_remove_disable_loop_components_cu,
        detect_fn=_detect_disable_loop_components_cu,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FLUID_CU],
        description=(
            "Disables Microsoft Loop embedded components in Office apps at the user level. "
            "Prevents Fluid/Loop collaborative blocks from rendering. "
            "Default: Enabled. Recommended: Disabled if Loop is not needed."
        ),
        tags=["m365", "loop", "fluid", "components", "collaboration"],
    ),
]


# -- Disable Copilot Compose in Word ───────────────────────────────────────────

_WORD_COMPOSE = rf"{_OFFICE_POLICY}\word\options\vpref"


def _apply_disable_word_copilot_compose(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable Copilot compose in Word")
    SESSION.backup([_WORD_COMPOSE], "M365WordCompose")
    SESSION.set_dword(_WORD_COMPOSE, "EnableCopilotCompose", 0)


def _remove_disable_word_copilot_compose(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WORD_COMPOSE, "EnableCopilotCompose")


def _detect_disable_word_copilot_compose() -> bool:
    return SESSION.read_dword(_WORD_COMPOSE, "EnableCopilotCompose") == 0


# -- Disable Copilot Preview Features ─────────────────────────────────────────

_COPILOT_PREVIEW = rf"{_OFFICE_POLICY}\common\copilotpreview"


def _apply_disable_copilot_preview(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable preview features")
    SESSION.backup([_COPILOT_PREVIEW], "M365CopilotPreview")
    SESSION.set_dword(_COPILOT_PREVIEW, "EnablePreviewFeatures", 0)


def _remove_disable_copilot_preview(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_PREVIEW, "EnablePreviewFeatures")


def _detect_disable_copilot_preview() -> bool:
    return SESSION.read_dword(_COPILOT_PREVIEW, "EnablePreviewFeatures") == 0


# -- Disable AI-Powered Suggestions in Outlook ─────────────────────────────────

_OUTLOOK_AI = rf"{_OFFICE_POLICY}\outlook\options\mail\compose"


def _apply_disable_outlook_ai_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("M365 Copilot: disable AI-powered suggestions in Outlook")
    SESSION.backup([_OUTLOOK_AI], "M365OutlookAI")
    SESSION.set_dword(_OUTLOOK_AI, "EnableAISuggestions", 0)


def _remove_disable_outlook_ai_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_OUTLOOK_AI, "EnableAISuggestions")


def _detect_disable_outlook_ai_suggestions() -> bool:
    return SESSION.read_dword(_OUTLOOK_AI, "EnableAISuggestions") == 0


TWEAKS += [
    TweakDef(
        id="m365-disable-word-copilot-compose",
        label="Disable Copilot Compose in Word",
        category="M365 Copilot",
        apply_fn=_apply_disable_word_copilot_compose,
        remove_fn=_remove_disable_word_copilot_compose,
        detect_fn=_detect_disable_word_copilot_compose,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WORD_COMPOSE],
        description=(
            "Disables Copilot compose/draft features in Word via policy. "
            "Prevents AI-assisted text generation in documents. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["m365", "copilot", "word", "compose", "draft"],
    ),
    TweakDef(
        id="m365-disable-copilot-preview",
        label="Disable Copilot Preview Features",
        category="M365 Copilot",
        apply_fn=_apply_disable_copilot_preview,
        remove_fn=_remove_disable_copilot_preview,
        detect_fn=_detect_disable_copilot_preview,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_COPILOT_PREVIEW],
        description=(
            "Disables M365 Copilot preview/experimental features via policy. "
            "Prevents early-access AI features from appearing in Office apps. "
            "Default: Enabled. Recommended: Disabled for stability."
        ),
        tags=["m365", "copilot", "preview", "experimental", "features"],
    ),
    TweakDef(
        id="m365-disable-outlook-ai-suggestions",
        label="Disable AI-Powered Suggestions in Outlook",
        category="M365 Copilot",
        apply_fn=_apply_disable_outlook_ai_suggestions,
        remove_fn=_remove_disable_outlook_ai_suggestions,
        detect_fn=_detect_disable_outlook_ai_suggestions,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_OUTLOOK_AI],
        description=(
            "Disables AI-powered compose suggestions in Outlook via policy. "
            "Prevents AI text predictions and smart reply features. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["m365", "copilot", "outlook", "ai", "suggestions"],
    ),
]
