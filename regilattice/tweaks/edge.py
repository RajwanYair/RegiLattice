"""Microsoft Edge registry tweaks."""

from __future__ import annotations

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


# ── Disable Edge First Run Experience ──────────────────────────────────────


def apply_disable_edge_fre(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable first-run experience")
    SESSION.backup([_EDGE_POLICY], "EdgeFRE")
    SESSION.set_dword(_EDGE_POLICY, "HideFirstRunExperience", 1)
    SESSION.set_dword(_EDGE_POLICY, "NewTabPageHideDefaultTopSites", 1)


def remove_disable_edge_fre(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "HideFirstRunExperience")
    SESSION.delete_value(_EDGE_POLICY, "NewTabPageHideDefaultTopSites")


def detect_disable_edge_fre() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "HideFirstRunExperience") == 1


# ── Disable Edge Password Manager ─────────────────────────────────────────


def apply_disable_edge_passwords(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable built-in password manager")
    SESSION.backup([_EDGE_POLICY], "EdgePasswords")
    SESSION.set_dword(_EDGE_POLICY, "PasswordManagerEnabled", 0)


def remove_disable_edge_passwords(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "PasswordManagerEnabled")


def detect_disable_edge_passwords() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "PasswordManagerEnabled") == 0


# ── Disable Edge Sync ────────────────────────────────────────────────────────


def apply_disable_edge_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable sync")
    SESSION.backup([_EDGE_POLICY], "EdgeSync")
    SESSION.set_dword(_EDGE_POLICY, "SyncDisabled", 1)


def remove_disable_edge_sync(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "SyncDisabled")


def detect_disable_edge_sync() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "SyncDisabled") == 1


# ── Disable Edge Shopping Features ──────────────────────────────────────────


def apply_disable_edge_shopping(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable shopping features")
    SESSION.backup([_EDGE_POLICY], "EdgeShopping")
    SESSION.set_dword(_EDGE_POLICY, "EdgeShoppingAssistantEnabled", 0)


def remove_disable_edge_shopping(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "EdgeShoppingAssistantEnabled")


def detect_disable_edge_shopping() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "EdgeShoppingAssistantEnabled") == 0


# ── Disable Edge Sidebar (Discover) ─────────────────────────────────────────


def apply_disable_edge_sidebar_discover(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable sidebar (Discover)")
    SESSION.backup([_EDGE_POLICY], "EdgeSidebarDiscover")
    SESSION.set_dword(_EDGE_POLICY, "HubsSidebarEnabled", 0)


def remove_disable_edge_sidebar_discover(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "HubsSidebarEnabled")


def detect_disable_edge_sidebar_discover() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "HubsSidebarEnabled") == 0


# ── Disable Microsoft Rewards Prompts ───────────────────────────────────────


def apply_disable_edge_rewards(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable rewards prompts")
    SESSION.backup([_EDGE_POLICY], "EdgeRewards")
    SESSION.set_dword(_EDGE_POLICY, "ShowMicrosoftRewards", 0)


def remove_disable_edge_rewards(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "ShowMicrosoftRewards")


def detect_disable_edge_rewards() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "ShowMicrosoftRewards") == 0


# ── Block Edge Third-Party Cookies ──────────────────────────────────────────


def apply_block_edge_third_party_cookies(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: block third-party cookies")
    SESSION.backup([_EDGE_POLICY], "EdgeThirdPartyCookies")
    SESSION.set_dword(_EDGE_POLICY, "BlockThirdPartyCookies", 1)


def remove_block_edge_third_party_cookies(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "BlockThirdPartyCookies")


def detect_block_edge_third_party_cookies() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "BlockThirdPartyCookies") == 1


# ── Disable Edge Sidebar (Policy) ───────────────────────────────────────────


def _apply_edge_sidebar_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable sidebar via policy")
    SESSION.backup([_EDGE_POLICY], "EdgeSidebarPolicy")
    SESSION.set_dword(_EDGE_POLICY, "HubsSidebarEnabled", 0)


def _remove_edge_sidebar_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "HubsSidebarEnabled")


def _detect_edge_sidebar_policy() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "HubsSidebarEnabled") == 0


# ── Disable Edge Shopping Features (Policy) ────────────────────────────────


def _apply_edge_shopping_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable shopping features via policy")
    SESSION.backup([_EDGE_POLICY], "EdgeShoppingPolicy")
    SESSION.set_dword(_EDGE_POLICY, "EdgeShoppingAssistantEnabled", 0)


def _remove_edge_shopping_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "EdgeShoppingAssistantEnabled")


def _detect_edge_shopping_policy() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "EdgeShoppingAssistantEnabled") == 0


# ── Disable Edge Collections ────────────────────────────────────────────────


def _apply_edge_disable_collections(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable Collections feature")
    SESSION.backup([_EDGE_POLICY], "EdgeCollections")
    SESSION.set_dword(_EDGE_POLICY, "EdgeCollectionsEnabled", 0)


def _remove_edge_disable_collections(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "EdgeCollectionsEnabled")


def _detect_edge_disable_collections() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "EdgeCollectionsEnabled") == 0


# ── Disable Edge Mini Menu ───────────────────────────────────────────────────


def _apply_edge_disable_mini_menu(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Edge: disable mini context menu on text selection")
    SESSION.backup([_EDGE_POLICY], "EdgeMiniMenu")
    SESSION.set_dword(_EDGE_POLICY, "MiniContextMenuEnabled", 0)


def _remove_edge_disable_mini_menu(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "MiniContextMenuEnabled")


def _detect_edge_disable_mini_menu() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "MiniContextMenuEnabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
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
    TweakDef(
        id="disable-edge-first-run",
        label="Disable Edge First-Run Experience",
        category="Edge",
        apply_fn=apply_disable_edge_fre,
        remove_fn=remove_disable_edge_fre,
        detect_fn=detect_disable_edge_fre,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description="Skips Edge first-run wizard and hides default top sites on new tab.",
        tags=["edge", "browser", "ux"],
    ),
    TweakDef(
        id="disable-edge-password-manager",
        label="Disable Edge Password Manager",
        category="Edge",
        apply_fn=apply_disable_edge_passwords,
        remove_fn=remove_disable_edge_passwords,
        detect_fn=detect_disable_edge_passwords,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description="Disables the Edge built-in password manager via policy.",
        tags=["edge", "browser", "password", "security"],
    ),
    TweakDef(
        id="edge-disable-sync",
        label="Disable Edge Sync",
        category="Edge",
        apply_fn=apply_disable_edge_sync,
        remove_fn=remove_disable_edge_sync,
        detect_fn=detect_disable_edge_sync,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description="Disables Edge profile sync via policy.",
        tags=["edge", "browser", "sync", "privacy"],
    ),
    TweakDef(
        id="edge-disable-sidebar",
        label="Disable Edge Sidebar",
        category="Edge",
        apply_fn=_apply_edge_sidebar_policy,
        remove_fn=_remove_edge_sidebar_policy,
        detect_fn=_detect_edge_sidebar_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the Edge sidebar (Bing Copilot, games, tools panel). "
            "Reduces memory usage and distractions. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["edge", "sidebar", "performance", "copilot"],
    ),
    TweakDef(
        id="edge-disable-shopping",
        label="Disable Edge Shopping Features",
        category="Edge",
        apply_fn=_apply_edge_shopping_policy,
        remove_fn=_remove_edge_shopping_policy,
        detect_fn=_detect_edge_shopping_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables Edge shopping assistant, price tracking, and coupons. "
            "Reduces CPU and network usage. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["edge", "shopping", "performance", "privacy"],
    ),
    TweakDef(
        id="edge-disable-rewards",
        label="Disable Microsoft Rewards Prompts",
        category="Edge",
        apply_fn=apply_disable_edge_rewards,
        remove_fn=remove_disable_edge_rewards,
        detect_fn=detect_disable_edge_rewards,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description="Disables Microsoft Rewards prompts in Edge via policy.",
        tags=["edge", "browser", "rewards"],
    ),
    TweakDef(
        id="edge-block-third-party-cookies",
        label="Block Edge Third-Party Cookies",
        category="Edge",
        apply_fn=apply_block_edge_third_party_cookies,
        remove_fn=remove_block_edge_third_party_cookies,
        detect_fn=detect_block_edge_third_party_cookies,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description="Blocks third-party cookies in Edge via policy.",
        tags=["edge", "browser", "cookies", "privacy"],
    ),
    TweakDef(
        id="edge-disable-collections",
        label="Disable Edge Collections",
        category="Edge",
        apply_fn=_apply_edge_disable_collections,
        remove_fn=_remove_edge_disable_collections,
        detect_fn=_detect_edge_disable_collections,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the Edge Collections feature used for organizing "
            "web content. Reduces UI clutter and memory usage. "
            "Default: Enabled. Recommended: Disabled if not used."
        ),
        tags=["edge", "collections", "ux", "performance"],
    ),
    TweakDef(
        id="edge-disable-mini-menu",
        label="Disable Edge Mini Context Menu",
        category="Edge",
        apply_fn=_apply_edge_disable_mini_menu,
        remove_fn=_remove_edge_disable_mini_menu,
        detect_fn=_detect_edge_disable_mini_menu,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the mini context menu that appears on text selection "
            "in Edge. Removes the floating toolbar with search/copy/etc. "
            "Default: Enabled. Recommended: Disabled for cleaner UX."
        ),
        tags=["edge", "mini-menu", "context-menu", "ux"],
    ),
]


# ── Disable Edge Sidebar Hub ─────────────────────────────────────────────────


def _apply_edge_sidebar_hub_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Edge sidebar panel")
    SESSION.backup([_EDGE_POLICY], "EdgeSidebarHub")
    SESSION.set_dword(_EDGE_POLICY, "HubsSidebarEnabled", 0)


def _remove_edge_sidebar_hub_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "HubsSidebarEnabled")


def _detect_edge_sidebar_hub_off() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "HubsSidebarEnabled") == 0


# ── Disable Edge First Run Experience ────────────────────────────────────────


def _apply_edge_first_run_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Edge first run experience")
    SESSION.backup([_EDGE_POLICY], "EdgeFirstRun")
    SESSION.set_dword(_EDGE_POLICY, "HideFirstRunExperience", 1)


def _remove_edge_first_run_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "HideFirstRunExperience")


def _detect_edge_first_run_off() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "HideFirstRunExperience") == 1


TWEAKS += [
    TweakDef(
        id="edge-disable-sidebar-hub",
        label="Disable Edge Sidebar Hub",
        category="Edge",
        apply_fn=_apply_edge_sidebar_hub_off,
        remove_fn=_remove_edge_sidebar_hub_off,
        detect_fn=_detect_edge_sidebar_hub_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the Edge sidebar (Hubs) panel via enterprise "
            "policy. Removes the sidebar icon and panel. "
            "Default: Enabled. Recommended: Disabled for cleaner UX."
        ),
        tags=["edge", "sidebar", "hubs", "ux", "policy"],
    ),
    TweakDef(
        id="edge-disable-first-run",
        label="Disable Edge First Run Experience",
        category="Edge",
        apply_fn=_apply_edge_first_run_off,
        remove_fn=_remove_edge_first_run_off,
        detect_fn=_detect_edge_first_run_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Hides the Edge first run experience and welcome page "
            "via enterprise policy. "
            "Default: Shown. Recommended: Hidden for managed deployments."
        ),
        tags=["edge", "first-run", "welcome", "policy", "ux"],
    ),
]


# ── Disable Edge Workspaces ──────────────────────────────────────────────────


def _apply_edge_workspaces_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Edge Workspaces feature")
    SESSION.backup([_EDGE_POLICY], "EdgeWorkspaces")
    SESSION.set_dword(_EDGE_POLICY, "EdgeWorkspacesEnabled", 0)


def _remove_edge_workspaces_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "EdgeWorkspacesEnabled")


def _detect_edge_workspaces_off() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "EdgeWorkspacesEnabled") == 0


# ── Disable Edge Drop ────────────────────────────────────────────────────────


def _apply_edge_drop_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Edge Drop file sharing feature")
    SESSION.backup([_EDGE_POLICY], "EdgeDrop")
    SESSION.set_dword(_EDGE_POLICY, "EdgeEDropEnabled", 0)


def _remove_edge_drop_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "EdgeEDropEnabled")


def _detect_edge_drop_off() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "EdgeEDropEnabled") == 0


# ── Disable Edge Discover Button ─────────────────────────────────────────────


def _apply_edge_discover_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Disable Edge Discover page context button")
    SESSION.backup([_EDGE_POLICY], "EdgeDiscover")
    SESSION.set_dword(_EDGE_POLICY, "DiscoverPageContextEnabled", 0)


def _remove_edge_discover_off(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_EDGE_POLICY, "DiscoverPageContextEnabled")


def _detect_edge_discover_off() -> bool:
    return SESSION.read_dword(_EDGE_POLICY, "DiscoverPageContextEnabled") == 0


TWEAKS += [
    TweakDef(
        id="edge-disable-workspaces",
        label="Disable Edge Workspaces",
        category="Edge",
        apply_fn=_apply_edge_workspaces_off,
        remove_fn=_remove_edge_workspaces_off,
        detect_fn=_detect_edge_workspaces_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the Edge Workspaces feature for shared browsing sessions "
            "via enterprise policy. Reduces background sync overhead. "
            "Default: Enabled. Recommended: Disabled if not used."
        ),
        tags=["edge", "workspaces", "collaboration", "policy", "performance"],
    ),
    TweakDef(
        id="edge-disable-drop",
        label="Disable Edge Drop",
        category="Edge",
        apply_fn=_apply_edge_drop_off,
        remove_fn=_remove_edge_drop_off,
        detect_fn=_detect_edge_drop_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the Edge Drop feature used for cross-device file sharing "
            "via enterprise policy. Reduces cloud sync and network usage. "
            "Default: Enabled. Recommended: Disabled for managed environments."
        ),
        tags=["edge", "drop", "file-sharing", "policy", "privacy"],
    ),
    TweakDef(
        id="edge-disable-discover",
        label="Disable Edge Discover Button",
        category="Edge",
        apply_fn=_apply_edge_discover_off,
        remove_fn=_remove_edge_discover_off,
        detect_fn=_detect_edge_discover_off,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_EDGE_POLICY],
        description=(
            "Disables the Edge Discover (compass) button and page context "
            "features via enterprise policy. Reduces Copilot integration. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["edge", "discover", "copilot", "policy", "privacy"],
    ),
]
