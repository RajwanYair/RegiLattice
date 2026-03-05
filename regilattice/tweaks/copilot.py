"""Windows 11 Copilot tweaks — disable/enable Windows Copilot."""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_COPILOT_POLICY = (
    r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WindowsCopilot"
)
_COPILOT_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot"
_COPILOT_EXPLORER = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
)
_COPILOT_KEYS = [_COPILOT_POLICY, _COPILOT_LM, _COPILOT_EXPLORER]


# ── Disable Windows Copilot ─────────────────────────────────────────────────


def apply_disable_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableCopilot")
    SESSION.backup(_COPILOT_KEYS, "Copilot")
    SESSION.set_dword(_COPILOT_POLICY, "TurnOffWindowsCopilot", 1)
    SESSION.set_dword(_COPILOT_LM, "TurnOffWindowsCopilot", 1)
    SESSION.set_dword(_COPILOT_EXPLORER, "ShowCopilotButton", 0)
    SESSION.log("Completed Add-DisableCopilot")


def remove_disable_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableCopilot")
    SESSION.backup(_COPILOT_KEYS, "Copilot_Remove")
    SESSION.delete_value(_COPILOT_POLICY, "TurnOffWindowsCopilot")
    SESSION.delete_value(_COPILOT_LM, "TurnOffWindowsCopilot")
    SESSION.set_dword(_COPILOT_EXPLORER, "ShowCopilotButton", 1)
    SESSION.log("Completed Remove-DisableCopilot")


def detect_disable_copilot() -> bool:
    return SESSION.read_dword(_COPILOT_POLICY, "TurnOffWindowsCopilot") == 1


# ── Disable Recall (AI Snapshots) ───────────────────────────────────────────

_RECALL_KEY = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WindowsAI"
_RECALL_LM = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"
_RECALL_KEYS = [_RECALL_KEY, _RECALL_LM]


def apply_disable_recall(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableRecall")
    SESSION.backup(_RECALL_KEYS, "Recall")
    SESSION.set_dword(_RECALL_KEY, "DisableAIDataAnalysis", 1)
    SESSION.set_dword(_RECALL_LM, "DisableAIDataAnalysis", 1)
    SESSION.set_dword(_RECALL_KEY, "TurnOffSavingSnapshots", 1)
    SESSION.set_dword(_RECALL_LM, "TurnOffSavingSnapshots", 1)
    SESSION.log("Completed Add-DisableRecall")


def remove_disable_recall(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableRecall")
    SESSION.backup(_RECALL_KEYS, "Recall_Remove")
    SESSION.delete_value(_RECALL_KEY, "DisableAIDataAnalysis")
    SESSION.delete_value(_RECALL_LM, "DisableAIDataAnalysis")
    SESSION.delete_value(_RECALL_KEY, "TurnOffSavingSnapshots")
    SESSION.delete_value(_RECALL_LM, "TurnOffSavingSnapshots")
    SESSION.log("Completed Remove-DisableRecall")


def detect_disable_recall() -> bool:
    return SESSION.read_dword(_RECALL_KEY, "DisableAIDataAnalysis") == 1


# ── Disable AI Suggestions in Settings ───────────────────────────────────

_AI_SETTINGS_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)


def _apply_disable_ai_settings(*, require_admin: bool = False) -> None:
    SESSION.log("Copilot: disable AI tips and suggestions in Settings")
    SESSION.backup([_AI_SETTINGS_CU], "AISettings")
    SESSION.set_dword(_AI_SETTINGS_CU, "Start_IrisRecommendations", 0)


def _remove_disable_ai_settings(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_AI_SETTINGS_CU, "Start_IrisRecommendations")


def _detect_disable_ai_settings() -> bool:
    return SESSION.read_dword(_AI_SETTINGS_CU, "Start_IrisRecommendations") == 0


# ── Disable Copilot in Edge ──────────────────────────────────────────────

_EDGE_COPILOT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"


def _apply_disable_edge_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: disable Copilot sidebar in Edge")
    SESSION.backup([_EDGE_COPILOT], "EdgeCopilot")
    SESSION.set_dword(_EDGE_COPILOT, "HubsSidebarEnabled", 0)
    SESSION.set_dword(_EDGE_COPILOT, "CopilotCDPPageContext", 0)


def _remove_disable_edge_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_COPILOT, "HubsSidebarEnabled")
    SESSION.delete_value(_EDGE_COPILOT, "CopilotCDPPageContext")


def _detect_disable_edge_copilot() -> bool:
    return SESSION.read_dword(_EDGE_COPILOT, "HubsSidebarEnabled") == 0


# ── Disable Copilot Taskbar Button ──────────────────────────────────────────

_TB_KEY = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"


def _apply_disable_copilot_button(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: hide Copilot taskbar button")
    SESSION.backup([_TB_KEY], "CopilotButton")
    SESSION.set_dword(_TB_KEY, "ShowCopilotButton", 0)


def _remove_disable_copilot_button(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TB_KEY, "ShowCopilotButton", 1)


def _detect_disable_copilot_button() -> bool:
    return SESSION.read_dword(_TB_KEY, "ShowCopilotButton") == 0


# ── Disable Bing Chat in Search ─────────────────────────────────────────────

_SEARCH_KEY = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"


def _apply_disable_bing_chat(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: disable Bing Chat in Windows Search")
    SESSION.backup([_SEARCH_KEY], "BingChat")
    SESSION.set_dword(_SEARCH_KEY, "DisableSearchBoxSuggestions", 1)


def _remove_disable_bing_chat(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_KEY, "DisableSearchBoxSuggestions")


def _detect_disable_bing_chat() -> bool:
    return SESSION.read_dword(_SEARCH_KEY, "DisableSearchBoxSuggestions") == 1


# ── Disable Windows Recall (AI Screenshot Feature) ──────────────────────────


def _apply_disable_recall_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: disable Windows Recall AI data analysis via HKLM policy")
    SESSION.backup([_RECALL_LM], "RecallAIPolicy")
    SESSION.set_dword(_RECALL_LM, "DisableAIDataAnalysis", 1)


def _remove_disable_recall_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RECALL_LM, "DisableAIDataAnalysis")


def _detect_disable_recall_policy() -> bool:
    return SESSION.read_dword(_RECALL_LM, "DisableAIDataAnalysis") == 1


# ── Remove Copilot from Taskbar ─────────────────────────────────────────────


def _apply_disable_copilot_taskbar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: remove Copilot button from taskbar")
    SESSION.backup([_TB_KEY], "CopilotTaskbar")
    SESSION.set_dword(_TB_KEY, "ShowCopilotButton", 0)


def _remove_disable_copilot_taskbar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_TB_KEY, "ShowCopilotButton", 1)


def _detect_disable_copilot_taskbar() -> bool:
    return SESSION.read_dword(_TB_KEY, "ShowCopilotButton") == 0


# ── Disable AI-Powered Suggestions in Settings ──────────────────────────────


def _apply_disable_ai_start_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: disable AI-powered suggestions in Settings")
    SESSION.backup([_AI_SETTINGS_CU], "AIStartSuggestions")
    SESSION.set_dword(_AI_SETTINGS_CU, "Start_IrisRecommendations", 0)


def _remove_disable_ai_start_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_AI_SETTINGS_CU, "Start_IrisRecommendations", 1)


def _detect_disable_ai_start_suggestions() -> bool:
    return SESSION.read_dword(_AI_SETTINGS_CU, "Start_IrisRecommendations") == 0


# ── Disable Copilot in Edge Browser (CDP) ────────────────────────────────────


def _apply_disable_copilot_edge(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: disable Copilot page context in Edge")
    SESSION.backup([_EDGE_COPILOT], "CopilotEdge")
    SESSION.set_dword(_EDGE_COPILOT, "CopilotCDPPageContext", 0)


def _remove_disable_copilot_edge(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_COPILOT, "CopilotCDPPageContext")


def _detect_disable_copilot_edge() -> bool:
    return SESSION.read_dword(_EDGE_COPILOT, "CopilotCDPPageContext") == 0


# ── Disable AI-Enhanced Widgets Feed ────────────────────────────────────────

_DSH_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"


def _apply_disable_ai_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: disable AI-enhanced widgets feed")
    SESSION.backup([_DSH_KEY], "AIWidgets")
    SESSION.set_dword(_DSH_KEY, "AllowNewsAndInterests", 0)


def _remove_disable_ai_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_DSH_KEY, "AllowNewsAndInterests")


def _detect_disable_ai_widgets() -> bool:
    return SESSION.read_dword(_DSH_KEY, "AllowNewsAndInterests") == 0


# ── Disable Recall Feature ──────────────────────────────────────────────────


def _apply_copilot_disable_recall(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: disable Windows Recall AI feature")
    SESSION.backup([_RECALL_LM], "CopilotDisableRecall")
    SESSION.set_dword(_RECALL_LM, "DisableAIDataAnalysis", 1)


def _remove_copilot_disable_recall(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_RECALL_LM, "DisableAIDataAnalysis")


def _detect_copilot_disable_recall() -> bool:
    return SESSION.read_dword(_RECALL_LM, "DisableAIDataAnalysis") == 1


# ── Disable Copilot Taskbar Button ─────────────────────────────────────────


def _apply_copilot_disable_taskbar_button(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Copilot: hide Copilot button from taskbar")
    SESSION.backup([_COPILOT_EXPLORER], "CopilotTaskbarButton")
    SESSION.set_dword(_COPILOT_EXPLORER, "ShowCopilotButton", 0)


def _remove_copilot_disable_taskbar_button(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.set_dword(_COPILOT_EXPLORER, "ShowCopilotButton", 1)


def _detect_copilot_disable_taskbar_button() -> bool:
    return SESSION.read_dword(_COPILOT_EXPLORER, "ShowCopilotButton") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="disable-copilot",
        label="Disable Windows Copilot",
        category="AI / Copilot",
        apply_fn=apply_disable_copilot,
        remove_fn=remove_disable_copilot,
        detect_fn=detect_disable_copilot,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_COPILOT_KEYS,
        description=(
            "Disables Windows Copilot via Group Policy and hides the "
            "taskbar button. Prevents AI-powered assistant from running."
        ),
        tags=["ai", "copilot", "privacy"],
    ),
    TweakDef(
        id="disable-recall",
        label="Disable Recall (AI Snapshots)",
        category="AI / Copilot",
        apply_fn=apply_disable_recall,
        remove_fn=remove_disable_recall,
        detect_fn=detect_disable_recall,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_RECALL_KEYS,
        description=(
            "Disables Windows Recall (AI-powered activity snapshots) which "
            "periodically screenshots your activity. Privacy-critical."
        ),
        tags=["ai", "recall", "privacy"],
    ),
    TweakDef(
        id="disable-ai-suggestions",
        label="Disable AI Tips in Settings/Start",
        category="AI / Copilot",
        apply_fn=_apply_disable_ai_settings,
        remove_fn=_remove_disable_ai_settings,
        detect_fn=_detect_disable_ai_settings,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_AI_SETTINGS_CU],
        description="Disables AI-powered suggestions and tips in the Start menu and Settings.",
        tags=["ai", "suggestions", "privacy"],
    ),
    TweakDef(
        id="disable-edge-copilot",
        label="Disable Copilot in Edge Browser",
        category="AI / Copilot",
        apply_fn=_apply_disable_edge_copilot,
        remove_fn=_remove_disable_edge_copilot,
        detect_fn=_detect_disable_edge_copilot,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_COPILOT],
        description="Disables the Copilot sidebar and page context sharing in Microsoft Edge.",
        tags=["ai", "copilot", "edge", "privacy"],
    ),
    TweakDef(
        id="disable-copilot-button",
        label="Hide Copilot Taskbar Button",
        category="AI / Copilot",
        apply_fn=_apply_disable_copilot_button,
        remove_fn=_remove_disable_copilot_button,
        detect_fn=_detect_disable_copilot_button,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TB_KEY],
        description="Hides the Copilot button from the Windows 11 taskbar.",
        tags=["ai", "copilot", "taskbar", "ux"],
    ),
    TweakDef(
        id="disable-bing-chat",
        label="Disable Bing Chat in Search",
        category="AI / Copilot",
        apply_fn=_apply_disable_bing_chat,
        remove_fn=_remove_disable_bing_chat,
        detect_fn=_detect_disable_bing_chat,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_KEY],
        description="Disables Bing Chat / AI suggestions in Windows Search.",
        tags=["ai", "bing", "search", "privacy"],
    ),
    TweakDef(
        id="disable-recall-policy",
        label="Disable Windows Recall (AI Screenshot Feature)",
        category="AI / Copilot",
        apply_fn=_apply_disable_recall_policy,
        remove_fn=_remove_disable_recall_policy,
        detect_fn=_detect_disable_recall_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RECALL_LM],
        description=(
            "Disables Windows Recall AI data analysis via machine-level "
            "policy. Prevents the AI screenshot feature from capturing activity."
        ),
        tags=["ai", "copilot", "recall", "privacy"],
    ),
    TweakDef(
        id="disable-copilot-taskbar",
        label="Remove Copilot from Taskbar",
        category="AI / Copilot",
        apply_fn=_apply_disable_copilot_taskbar,
        remove_fn=_remove_disable_copilot_taskbar,
        detect_fn=_detect_disable_copilot_taskbar,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_TB_KEY],
        description="Removes the Copilot button from the Windows taskbar.",
        tags=["ai", "copilot", "taskbar"],
    ),
    TweakDef(
        id="disable-ai-start-suggestions",
        label="Disable AI-Powered Suggestions in Settings",
        category="AI / Copilot",
        apply_fn=_apply_disable_ai_start_suggestions,
        remove_fn=_remove_disable_ai_start_suggestions,
        detect_fn=_detect_disable_ai_start_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_AI_SETTINGS_CU],
        description=(
            "Disables AI-powered Iris recommendations in the Start menu "
            "and Settings app."
        ),
        tags=["ai", "copilot", "suggestions", "start"],
    ),
    TweakDef(
        id="disable-copilot-edge",
        label="Disable Copilot in Edge Browser",
        category="AI / Copilot",
        apply_fn=_apply_disable_copilot_edge,
        remove_fn=_remove_disable_copilot_edge,
        detect_fn=_detect_disable_copilot_edge,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_COPILOT],
        description=(
            "Disables the Copilot CDP page context feature in Microsoft Edge."
        ),
        tags=["ai", "copilot", "edge"],
    ),
    TweakDef(
        id="disable-ai-widgets",
        label="Disable AI-Enhanced Widgets Feed",
        category="AI / Copilot",
        apply_fn=_apply_disable_ai_widgets,
        remove_fn=_remove_disable_ai_widgets,
        detect_fn=_detect_disable_ai_widgets,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_DSH_KEY],
        description=(
            "Disables the AI-enhanced News and Interests widgets feed "
            "via Dsh policy."
        ),
        tags=["ai", "copilot", "widgets"],
    ),
    TweakDef(
        id="copilot-disable-recall",
        label="Disable Recall Feature",
        category="AI / Copilot",
        apply_fn=_apply_copilot_disable_recall,
        remove_fn=_remove_copilot_disable_recall,
        detect_fn=_detect_copilot_disable_recall,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_RECALL_LM],
        description=(
            "Disables Windows Recall AI feature that takes periodic screenshots. "
            "Prevents privacy-invasive screen capture and analysis. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["copilot", "recall", "privacy", "ai"],
    ),
    TweakDef(
        id="copilot-disable-taskbar-button",
        label="Disable Copilot Taskbar Button",
        category="AI / Copilot",
        apply_fn=_apply_copilot_disable_taskbar_button,
        remove_fn=_remove_copilot_disable_taskbar_button,
        detect_fn=_detect_copilot_disable_taskbar_button,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_COPILOT_EXPLORER],
        description=(
            "Hides the Copilot button from the Windows taskbar. "
            "Reduces visual clutter. Default: Shown. Recommended: Hidden."
        ),
        tags=["copilot", "taskbar", "ux", "ai"],
    ),
]


# -- Disable Windows Recall (HKLM policy only) --------------------------------


def _apply_ai_disable_recall(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("AI: disabling Windows Recall via HKLM policy")
    SESSION.backup([_RECALL_LM], "AIDisableRecall")
    SESSION.set_dword(_RECALL_LM, "DisableAIDataAnalysis", 1)
    SESSION.log("AI: Windows Recall disabled")


def _remove_ai_disable_recall(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_RECALL_LM], "AIDisableRecall_Remove")
    SESSION.delete_value(_RECALL_LM, "DisableAIDataAnalysis")


def _detect_ai_disable_recall() -> bool:
    return SESSION.read_dword(_RECALL_LM, "DisableAIDataAnalysis") == 1


# -- Disable Copilot Keyboard Shortcut ----------------------------------------


def _apply_ai_disable_copilot_keyboard(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("AI: disabling Copilot keyboard shortcut")
    SESSION.backup([_COPILOT_EXPLORER], "AICopilotKeyboard")
    SESSION.set_dword(_COPILOT_EXPLORER, "ShowCopilotButton", 0)
    SESSION.log("AI: Copilot keyboard shortcut disabled")


def _remove_ai_disable_copilot_keyboard(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_COPILOT_EXPLORER], "AICopilotKeyboard_Remove")
    SESSION.set_dword(_COPILOT_EXPLORER, "ShowCopilotButton", 1)


def _detect_ai_disable_copilot_keyboard() -> bool:
    return SESSION.read_dword(_COPILOT_EXPLORER, "ShowCopilotButton") == 0


TWEAKS += [
    TweakDef(
        id="ai-disable-recall",
        label="Disable Windows Recall (HKLM Policy)",
        category="AI / Copilot",
        apply_fn=_apply_ai_disable_recall,
        remove_fn=_remove_ai_disable_recall,
        detect_fn=_detect_ai_disable_recall,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_RECALL_LM],
        description=(
            "Disables Windows Recall AI data analysis via HKLM Group Policy. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["ai", "recall", "privacy", "policy"],
    ),
    TweakDef(
        id="ai-disable-copilot-keyboard",
        label="Disable Copilot Keyboard Shortcut",
        category="AI / Copilot",
        apply_fn=_apply_ai_disable_copilot_keyboard,
        remove_fn=_remove_ai_disable_copilot_keyboard,
        detect_fn=_detect_ai_disable_copilot_keyboard,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_COPILOT_EXPLORER],
        description=(
            "Hides the Copilot button and disables keyboard shortcut. "
            "Default: Shown. Recommended: Hidden."
        ),
        tags=["ai", "copilot", "keyboard", "shortcut"],
    ),
]


# ══ Windows 11 24H2+ Copilot Paths ══════════════════════════════════════

# In 24H2, Copilot moved from a sidebar to a system app with new registry
# paths.  These tweaks target the updated locations.

_COPILOT_24H2 = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"
_COPILOT_RUNTIME = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CopilotRuntime"
_BING_CHAT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge\BingChat"
_COPILOT_ELIGIBLE = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Copilot\BingChat"
)


# -- Disable Copilot Runtime (24H2) ────────────────────────────────────


def _apply_disable_copilot_runtime(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("AI: disable Copilot Runtime (24H2+ system app)")
    SESSION.backup([_COPILOT_RUNTIME], "CopilotRuntime24H2")
    SESSION.set_dword(_COPILOT_RUNTIME, "AllowCopilotRuntime", 0)


def _remove_disable_copilot_runtime(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_RUNTIME, "AllowCopilotRuntime")


def _detect_disable_copilot_runtime() -> bool:
    return SESSION.read_dword(_COPILOT_RUNTIME, "AllowCopilotRuntime") == 0


# -- Disable Bing Chat in Edge (24H2) ──────────────────────────────────


def _apply_disable_bing_chat_edge(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("AI: disable Bing Chat / Copilot in Edge sidebar (24H2 path)")
    SESSION.backup([_BING_CHAT], "BingChatEdge24H2")
    SESSION.set_dword(_BING_CHAT, "IsUserEligible", 0)


def _remove_disable_bing_chat_edge(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_BING_CHAT, "IsUserEligible")


def _detect_disable_bing_chat_edge() -> bool:
    return SESSION.read_dword(_BING_CHAT, "IsUserEligible") == 0


# -- Block Copilot User Eligibility ────────────────────────────────────


def _apply_copilot_ineligible(*, require_admin: bool = False) -> None:
    SESSION.log("AI: mark user as ineligible for Copilot")
    SESSION.backup([_COPILOT_ELIGIBLE], "CopilotEligible")
    SESSION.set_dword(_COPILOT_ELIGIBLE, "IsUserEligible", 0)


def _remove_copilot_ineligible(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_COPILOT_ELIGIBLE, "IsUserEligible")


def _detect_copilot_ineligible() -> bool:
    return SESSION.read_dword(_COPILOT_ELIGIBLE, "IsUserEligible") == 0


# -- Disable Copilot in Edge via Policy ────────────────────────────────


def _apply_disable_copilot_edge_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("AI: disable Copilot in Edge via HubsSidebarEnabled policy")
    SESSION.backup([_COPILOT_24H2], "CopilotEdgePolicy")
    SESSION.set_dword(_COPILOT_24H2, "HubsSidebarEnabled", 0)


def _remove_disable_copilot_edge_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_COPILOT_24H2, "HubsSidebarEnabled")


def _detect_disable_copilot_edge_policy() -> bool:
    return SESSION.read_dword(_COPILOT_24H2, "HubsSidebarEnabled") == 0


TWEAKS += [
    TweakDef(
        id="ai-disable-copilot-runtime-24h2",
        label="Disable Copilot Runtime (24H2)",
        category="AI / Copilot",
        apply_fn=_apply_disable_copilot_runtime,
        remove_fn=_remove_disable_copilot_runtime,
        detect_fn=_detect_disable_copilot_runtime,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_COPILOT_RUNTIME],
        description=(
            "Disables the Copilot Runtime system app introduced in Windows 11 24H2. "
            "Uses the new AllowCopilotRuntime policy path. "
            "Default: enabled. Recommended: disabled for privacy."
        ),
        tags=["ai", "copilot", "24h2", "runtime", "policy"],
    ),
    TweakDef(
        id="ai-disable-bing-chat-edge-24h2",
        label="Disable Bing Chat in Edge (24H2)",
        category="AI / Copilot",
        apply_fn=_apply_disable_bing_chat_edge,
        remove_fn=_remove_disable_bing_chat_edge,
        detect_fn=_detect_disable_bing_chat_edge,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_BING_CHAT],
        description=(
            "Blocks Bing Chat / Copilot in Edge sidebar using the updated "
            "24H2 policy path (BingChat\\IsUserEligible). "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["ai", "copilot", "edge", "bing-chat", "24h2"],
    ),
    TweakDef(
        id="ai-copilot-ineligible",
        label="Block Copilot User Eligibility",
        category="AI / Copilot",
        apply_fn=_apply_copilot_ineligible,
        remove_fn=_remove_copilot_ineligible,
        detect_fn=_detect_copilot_ineligible,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_COPILOT_ELIGIBLE],
        description=(
            "Marks the current user as ineligible for Copilot via the Shell "
            "BingChat registry key. Prevents Copilot from activating. "
            "Default: eligible. Recommended: ineligible for privacy."
        ),
        tags=["ai", "copilot", "eligible", "user", "shell"],
    ),
    TweakDef(
        id="ai-disable-copilot-edge-sidebar",
        label="Disable Copilot Edge Sidebar (Policy)",
        category="AI / Copilot",
        apply_fn=_apply_disable_copilot_edge_policy,
        remove_fn=_remove_disable_copilot_edge_policy,
        detect_fn=_detect_disable_copilot_edge_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_COPILOT_24H2],
        description=(
            "Disables the Copilot/Bing sidebar in Microsoft Edge via the "
            "HubsSidebarEnabled policy. Default: enabled. Recommended: disabled."
        ),
        tags=["ai", "copilot", "edge", "sidebar", "policy"],
    ),
]
