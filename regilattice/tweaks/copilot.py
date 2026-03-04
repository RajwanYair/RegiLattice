"""Windows 11 Copilot tweaks — disable/enable Windows Copilot."""

from __future__ import annotations

from typing import List

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


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
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
]
