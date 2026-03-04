"""Microsoft Edge registry tweaks."""

from __future__ import annotations

from typing import List

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Constants ────────────────────────────────────────────────────────────────

_EDGE_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge"
_EDGE_UPDATE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate"
_EDGE_KEYS = [_EDGE_POLICY, _EDGE_UPDATE]


# ── Disable Edge Startup Boost ──────────────────────────────────────────────


def apply_disable_edge_startup_boost(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableEdgeStartupBoost")
    SESSION.backup([_EDGE_POLICY], "EdgeStartupBoost")
    SESSION.set_dword(_EDGE_POLICY, "StartupBoostEnabled", 0)
    SESSION.log("Completed Add-DisableEdgeStartupBoost")


def remove_disable_edge_startup_boost(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableEdgeStartupBoost")
    SESSION.backup([_EDGE_POLICY], "EdgeStartupBoost_Remove")
    SESSION.delete_value(_EDGE_POLICY, "StartupBoostEnabled")
    SESSION.log("Completed Remove-DisableEdgeStartupBoost")


def detect_disable_edge_startup_boost() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "StartupBoostEnabled") == 0


# ── Disable Edge Sidebar / Discover ─────────────────────────────────────────


def apply_disable_edge_sidebar(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableEdgeSidebar")
    SESSION.backup([_EDGE_POLICY], "EdgeSidebar")
    SESSION.set_dword(_EDGE_POLICY, "HubsSidebarEnabled", 0)
    SESSION.set_dword(_EDGE_POLICY, "EdgeShoppingAssistantEnabled", 0)
    SESSION.set_dword(_EDGE_POLICY, "EdgeCollectionsEnabled", 0)
    SESSION.log("Completed Add-DisableEdgeSidebar")


def remove_disable_edge_sidebar(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableEdgeSidebar")
    SESSION.backup([_EDGE_POLICY], "EdgeSidebar_Remove")
    SESSION.delete_value(_EDGE_POLICY, "HubsSidebarEnabled")
    SESSION.delete_value(_EDGE_POLICY, "EdgeShoppingAssistantEnabled")
    SESSION.delete_value(_EDGE_POLICY, "EdgeCollectionsEnabled")
    SESSION.log("Completed Remove-DisableEdgeSidebar")


def detect_disable_edge_sidebar() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "HubsSidebarEnabled") == 0


# ── Disable Edge Telemetry ──────────────────────────────────────────────────


def apply_disable_edge_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableEdgeTelemetry")
    SESSION.backup(_EDGE_KEYS, "EdgeTelemetry")
    SESSION.set_dword(_EDGE_POLICY, "MetricsReportingEnabled", 0)
    SESSION.set_dword(_EDGE_POLICY, "SendSiteInfoToImproveServices", 0)
    SESSION.set_dword(_EDGE_POLICY, "PersonalizationReportingEnabled", 0)
    SESSION.set_dword(_EDGE_POLICY, "DiagnosticData", 0)
    SESSION.set_dword(_EDGE_POLICY, "EdgeFollowEnabled", 0)
    SESSION.set_dword(_EDGE_POLICY, "SpotlightExperiencesAndSuggestionsEnabled", 0)
    SESSION.set_dword(_EDGE_POLICY, "ShowRecommendationsEnabled", 0)
    SESSION.log("Completed Add-DisableEdgeTelemetry")


def remove_disable_edge_telemetry(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableEdgeTelemetry")
    SESSION.backup(_EDGE_KEYS, "EdgeTelemetry_Remove")
    for val in (
        "MetricsReportingEnabled",
        "SendSiteInfoToImproveServices",
        "PersonalizationReportingEnabled",
        "DiagnosticData",
        "EdgeFollowEnabled",
        "SpotlightExperiencesAndSuggestionsEnabled",
        "ShowRecommendationsEnabled",
    ):
        SESSION.delete_value(_EDGE_POLICY, val)
    SESSION.log("Completed Remove-DisableEdgeTelemetry")


def detect_disable_edge_telemetry() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "MetricsReportingEnabled") == 0


# ── Disable Edge Auto-Update ────────────────────────────────────────────────


def apply_disable_edge_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Add-DisableEdgeUpdate")
    SESSION.backup([_EDGE_UPDATE], "EdgeUpdate")
    # UpdateDefault: 0=disabled, 1=auto, 2=manual-only, 3=forced
    SESSION.set_dword(_EDGE_UPDATE, "UpdateDefault", 0)
    SESSION.log("Completed Add-DisableEdgeUpdate")


def remove_disable_edge_update(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Starting Remove-DisableEdgeUpdate")
    SESSION.backup([_EDGE_UPDATE], "EdgeUpdate_Remove")
    SESSION.delete_value(_EDGE_UPDATE, "UpdateDefault")
    SESSION.log("Completed Remove-DisableEdgeUpdate")


def detect_disable_edge_update() -> bool:
    return SESSION.read_dword(_EDGE_UPDATE, "UpdateDefault") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: List[TweakDef] = [
    TweakDef(
        id="disable-edge-startup-boost",
        label="Disable Edge Startup Boost",
        category="Edge",
        apply_fn=apply_disable_edge_startup_boost,
        remove_fn=remove_disable_edge_startup_boost,
        detect_fn=detect_disable_edge_startup_boost,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Prevents Edge from pre-launching at login, saving memory and "
            "CPU for users who don't use Edge as primary browser."
        ),
        tags=["edge", "browser", "startup"],
    ),
    TweakDef(
        id="disable-edge-sidebar",
        label="Disable Edge Sidebar & Shopping",
        category="Edge",
        apply_fn=apply_disable_edge_sidebar,
        remove_fn=remove_disable_edge_sidebar,
        detect_fn=detect_disable_edge_sidebar,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the Edge sidebar (Discover), shopping assistant, "
            "and collections panel for a cleaner browsing experience."
        ),
        tags=["edge", "browser", "sidebar"],
    ),
    TweakDef(
        id="disable-edge-telemetry",
        label="Disable Edge Telemetry",
        category="Edge",
        apply_fn=apply_disable_edge_telemetry,
        remove_fn=remove_disable_edge_telemetry,
        detect_fn=detect_disable_edge_telemetry,
        needs_admin=True,
        corp_safe=False,
        registry_keys=_EDGE_KEYS,
        description=(
            "Disables Edge metrics, diagnostics, personalisation reporting, "
            "follow, spotlight and recommendation features."
        ),
        tags=["edge", "browser", "telemetry", "privacy"],
    ),
    TweakDef(
        id="disable-edge-update",
        label="Disable Edge Auto-Update",
        category="Edge",
        apply_fn=apply_disable_edge_update,
        remove_fn=remove_disable_edge_update,
        detect_fn=detect_disable_edge_update,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_UPDATE],
        description=(
            "Prevents Edge from auto-updating. Useful for controlled "
            "environments or when pinning to a specific version."
        ),
        tags=["edge", "browser", "update"],
    ),
]
