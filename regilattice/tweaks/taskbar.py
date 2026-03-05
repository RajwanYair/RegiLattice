"""Windows Taskbar tweaks -- alignment, icons, badges, button combining, and more.

Covers: taskbar alignment, icon size, search box, Task View, Widgets,
Chat, Copilot, button combining, badges, flashing, End Task, and recent searches.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# -- Shared key paths ---------------------------------------------------------

_ADV = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"
_SEARCH = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"
_WIDGETS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"


# -- 1. Align Taskbar Left (Win11) -------------------------------------------


def _apply_align_left(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: setting alignment to left")
    SESSION.backup([_ADV], "TaskbarAlignLeft")
    SESSION.set_dword(_ADV, "TaskbarAl", 0)
    SESSION.log("Taskbar: alignment set to left")


def _remove_align_left(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring alignment to center")
    SESSION.backup([_ADV], "TaskbarAlignLeft_Remove")
    SESSION.set_dword(_ADV, "TaskbarAl", 1)
    SESSION.log("Taskbar: alignment restored to center")


def _detect_align_left() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarAl") == 0


# -- 2. Small Taskbar Icons ---------------------------------------------------


def _apply_small_icons(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: enabling small icons")
    SESSION.backup([_ADV], "TaskbarSmallIcons")
    SESSION.set_dword(_ADV, "TaskbarSmallIcons", 1)
    SESSION.log("Taskbar: small icons enabled")


def _remove_small_icons(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring normal icon size")
    SESSION.backup([_ADV], "TaskbarSmallIcons_Remove")
    SESSION.set_dword(_ADV, "TaskbarSmallIcons", 0)
    SESSION.log("Taskbar: normal icon size restored")


def _detect_small_icons() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarSmallIcons") == 1


# -- 3. Hide Taskbar Search Box -----------------------------------------------


def _apply_hide_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: hiding search box")
    SESSION.backup([_SEARCH], "TaskbarHideSearch")
    SESSION.set_dword(_SEARCH, "SearchboxTaskbarMode", 0)
    SESSION.log("Taskbar: search box hidden")


def _remove_hide_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring search icon")
    SESSION.backup([_SEARCH], "TaskbarHideSearch_Remove")
    SESSION.set_dword(_SEARCH, "SearchboxTaskbarMode", 1)
    SESSION.log("Taskbar: search icon restored")


def _detect_hide_search() -> bool:
    return SESSION.read_dword(_SEARCH, "SearchboxTaskbarMode") == 0


# -- 4. Hide Task View Button -------------------------------------------------


def _apply_hide_task_view(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: hiding Task View button")
    SESSION.backup([_ADV], "TaskbarHideTaskView")
    SESSION.set_dword(_ADV, "ShowTaskViewButton", 0)
    SESSION.log("Taskbar: Task View button hidden")


def _remove_hide_task_view(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring Task View button")
    SESSION.backup([_ADV], "TaskbarHideTaskView_Remove")
    SESSION.set_dword(_ADV, "ShowTaskViewButton", 1)
    SESSION.log("Taskbar: Task View button restored")


def _detect_hide_task_view() -> bool:
    return SESSION.read_dword(_ADV, "ShowTaskViewButton") == 0


# -- 5. Hide Widgets (Policy) ------------------------------------------------

_WIDGETS_KEYS = [_ADV, _WIDGETS_POLICY]


def _apply_hide_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: hiding Widgets via policy")
    SESSION.backup(_WIDGETS_KEYS, "TaskbarHideWidgets")
    SESSION.set_dword(_WIDGETS_POLICY, "AllowNewsAndInterests", 0)
    SESSION.set_dword(_ADV, "TaskbarDa", 0)
    SESSION.log("Taskbar: Widgets hidden via policy")


def _remove_hide_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring Widgets")
    SESSION.backup(_WIDGETS_KEYS, "TaskbarHideWidgets_Remove")
    SESSION.delete_value(_WIDGETS_POLICY, "AllowNewsAndInterests")
    SESSION.set_dword(_ADV, "TaskbarDa", 1)
    SESSION.log("Taskbar: Widgets restored")


def _detect_hide_widgets() -> bool:
    policy = SESSION.read_dword(_WIDGETS_POLICY, "AllowNewsAndInterests")
    return policy == 0


# -- 6. Hide Chat / Teams Icon ------------------------------------------------


def _apply_hide_chat(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: hiding Chat icon")
    SESSION.backup([_ADV], "TaskbarHideChat")
    SESSION.set_dword(_ADV, "TaskbarMn", 0)
    SESSION.log("Taskbar: Chat icon hidden")


def _remove_hide_chat(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring Chat icon")
    SESSION.backup([_ADV], "TaskbarHideChat_Remove")
    SESSION.set_dword(_ADV, "TaskbarMn", 1)
    SESSION.log("Taskbar: Chat icon restored")


def _detect_hide_chat() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarMn") == 0


# -- 7. Hide Copilot Button ---------------------------------------------------


def _apply_hide_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: hiding Copilot button")
    SESSION.backup([_ADV], "TaskbarHideCopilot")
    SESSION.set_dword(_ADV, "ShowCopilotButton", 0)
    SESSION.log("Taskbar: Copilot button hidden")


def _remove_hide_copilot(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring Copilot button")
    SESSION.backup([_ADV], "TaskbarHideCopilot_Remove")
    SESSION.set_dword(_ADV, "ShowCopilotButton", 1)
    SESSION.log("Taskbar: Copilot button restored")


def _detect_hide_copilot() -> bool:
    return SESSION.read_dword(_ADV, "ShowCopilotButton") == 0


# -- 8. Never Combine Taskbar Buttons -----------------------------------------


def _apply_never_combine(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: setting buttons to never combine")
    SESSION.backup([_ADV], "TaskbarNeverCombine")
    SESSION.set_dword(_ADV, "TaskbarGlomLevel", 2)
    SESSION.log("Taskbar: buttons set to never combine")


def _remove_never_combine(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring button combining")
    SESSION.backup([_ADV], "TaskbarNeverCombine_Remove")
    SESSION.set_dword(_ADV, "TaskbarGlomLevel", 0)
    SESSION.log("Taskbar: button combining restored")


def _detect_never_combine() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarGlomLevel") == 2


# -- 9. Disable Notification Badges -------------------------------------------


def _apply_disable_badges(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disabling notification badges")
    SESSION.backup([_ADV], "TaskbarDisableBadges")
    SESSION.set_dword(_ADV, "TaskbarBadges", 0)
    SESSION.log("Taskbar: notification badges disabled")


def _remove_disable_badges(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring notification badges")
    SESSION.backup([_ADV], "TaskbarDisableBadges_Remove")
    SESSION.set_dword(_ADV, "TaskbarBadges", 1)
    SESSION.log("Taskbar: notification badges restored")


def _detect_disable_badges() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarBadges") == 0


# -- 10. Disable Taskbar Button Flashing --------------------------------------


def _apply_disable_flashing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disabling button flashing")
    SESSION.backup([_ADV], "TaskbarDisableFlashing")
    SESSION.set_dword(_ADV, "TaskbarFlashing", 0)
    SESSION.log("Taskbar: button flashing disabled")


def _remove_disable_flashing(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring button flashing")
    SESSION.backup([_ADV], "TaskbarDisableFlashing_Remove")
    SESSION.set_dword(_ADV, "TaskbarFlashing", 1)
    SESSION.log("Taskbar: button flashing restored")


def _detect_disable_flashing() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarFlashing") == 0


# -- 11. Enable End Task in Taskbar Right-Click --------------------------------


def _apply_end_task(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: enabling End Task context menu")
    SESSION.backup([_ADV], "TaskbarEndTask")
    SESSION.set_dword(_ADV, "TaskbarEndTask", 1)
    SESSION.log("Taskbar: End Task context menu enabled")


def _remove_end_task(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disabling End Task context menu")
    SESSION.backup([_ADV], "TaskbarEndTask_Remove")
    SESSION.set_dword(_ADV, "TaskbarEndTask", 0)
    SESSION.log("Taskbar: End Task context menu disabled")


def _detect_end_task() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarEndTask") == 1


# -- 12. Disable Recent Searches in Taskbar -----------------------------------


def _apply_disable_recent_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disabling recent searches")
    SESSION.backup([_SEARCH], "TaskbarDisableRecentSearch")
    SESSION.set_dword(_SEARCH, "SearchboxTaskbarRecentSearchesEnabled", 0)
    SESSION.log("Taskbar: recent searches disabled")


def _remove_disable_recent_search(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: restoring recent searches")
    SESSION.backup([_SEARCH], "TaskbarDisableRecentSearch_Remove")
    SESSION.set_dword(_SEARCH, "SearchboxTaskbarRecentSearchesEnabled", 1)
    SESSION.log("Taskbar: recent searches restored")


def _detect_disable_recent_search() -> bool:
    return SESSION.read_dword(_SEARCH, "SearchboxTaskbarRecentSearchesEnabled") == 0


# -- TWEAKS export ------------------------------------------------------------

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="taskbar-align-left",
        label="Align Taskbar Left (Win11)",
        category="Taskbar",
        apply_fn=_apply_align_left,
        remove_fn=_remove_align_left,
        detect_fn=_detect_align_left,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Sets Windows 11 taskbar alignment to left instead of center. "
            "Default: center. Recommended: left."
        ),
        tags=["taskbar", "alignment", "win11"],
    ),
    TweakDef(
        id="taskbar-small-icons",
        label="Use Small Taskbar Icons",
        category="Taskbar",
        apply_fn=_apply_small_icons,
        remove_fn=_remove_small_icons,
        detect_fn=_detect_small_icons,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Shrinks taskbar icons and reduces taskbar height (Win10). "
            "Default: large icons. Recommended: small icons."
        ),
        tags=["taskbar", "icons", "size"],
    ),
    TweakDef(
        id="taskbar-hide-search",
        label="Hide Taskbar Search Box",
        category="Taskbar",
        apply_fn=_apply_hide_search,
        remove_fn=_remove_hide_search,
        detect_fn=_detect_hide_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH],
        description=(
            "Hides the search box or icon from the taskbar. "
            "You can still search by pressing Win+S. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "search", "declutter"],
    ),
    TweakDef(
        id="taskbar-hide-task-view",
        label="Hide Task View Button",
        category="Taskbar",
        apply_fn=_apply_hide_task_view,
        remove_fn=_remove_hide_task_view,
        detect_fn=_detect_hide_task_view,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Hides the Task View button from the taskbar. "
            "You can still use Win+Tab for virtual desktops. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "task-view", "declutter"],
    ),
    TweakDef(
        id="taskbar-hide-widgets",
        label="Hide Widgets (Policy)",
        category="Taskbar",
        apply_fn=_apply_hide_widgets,
        remove_fn=_remove_hide_widgets,
        detect_fn=_detect_hide_widgets,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WIDGETS_POLICY, _ADV],
        description=(
            "Disables Widgets board and weather widget via HKLM policy. "
            "Frees resources used by the Edge WebView2 widget host. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "widgets", "policy", "performance"],
    ),
    TweakDef(
        id="taskbar-hide-chat",
        label="Hide Chat / Teams Icon",
        category="Taskbar",
        apply_fn=_apply_hide_chat,
        remove_fn=_remove_hide_chat,
        detect_fn=_detect_hide_chat,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Hides the Microsoft Teams Chat icon from the taskbar. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "chat", "teams", "declutter"],
    ),
    TweakDef(
        id="taskbar-hide-copilot",
        label="Hide Copilot Button",
        category="Taskbar",
        apply_fn=_apply_hide_copilot,
        remove_fn=_remove_hide_copilot,
        detect_fn=_detect_hide_copilot,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Hides the Copilot button from the taskbar. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "copilot", "ai", "declutter"],
    ),
    TweakDef(
        id="taskbar-never-combine",
        label="Never Combine Taskbar Buttons",
        category="Taskbar",
        apply_fn=_apply_never_combine,
        remove_fn=_remove_never_combine,
        detect_fn=_detect_never_combine,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Prevents taskbar from grouping windows of the same app. "
            "Each window gets its own button with a visible label. "
            "Default: always combine. Recommended: never combine."
        ),
        tags=["taskbar", "grouping", "buttons", "ux"],
    ),
    TweakDef(
        id="taskbar-disable-badges",
        label="Disable Notification Badges",
        category="Taskbar",
        apply_fn=_apply_disable_badges,
        remove_fn=_remove_disable_badges,
        detect_fn=_detect_disable_badges,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables unread message count badges on taskbar app icons. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "badges", "notifications", "declutter"],
    ),
    TweakDef(
        id="taskbar-disable-flashing",
        label="Disable Taskbar Button Flashing",
        category="Taskbar",
        apply_fn=_apply_disable_flashing,
        remove_fn=_remove_disable_flashing,
        detect_fn=_detect_disable_flashing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Stops taskbar buttons from flashing to get your attention. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "flashing", "focus", "ux"],
    ),
    TweakDef(
        id="taskbar-end-task",
        label="Enable End Task in Taskbar",
        category="Taskbar",
        apply_fn=_apply_end_task,
        remove_fn=_remove_end_task,
        detect_fn=_detect_end_task,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Adds an End Task option to the taskbar right-click menu "
            "for quickly killing unresponsive apps. "
            "Default: disabled. Recommended: enabled."
        ),
        tags=["taskbar", "end-task", "productivity"],
    ),
    TweakDef(
        id="taskbar-disable-recent-search",
        label="Disable Recent Searches in Taskbar",
        category="Taskbar",
        apply_fn=_apply_disable_recent_search,
        remove_fn=_remove_disable_recent_search,
        detect_fn=_detect_disable_recent_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH],
        description=(
            "Disables recent search suggestions shown in the taskbar search box. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "search", "privacy", "declutter"],
    ),
]


# -- Disable Notification Badge Overlay ----------------------------------------


def _apply_taskbar_disable_notification_badges(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disabling notification badge overlay")
    SESSION.backup([_ADV], "TaskbarNotifBadges")
    SESSION.set_dword(_ADV, "TaskbarBadges", 0)
    SESSION.log("Taskbar: notification badge overlay disabled")


def _remove_taskbar_disable_notification_badges(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "TaskbarNotifBadges_Remove")
    SESSION.set_dword(_ADV, "TaskbarBadges", 1)


def _detect_taskbar_disable_notification_badges() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarBadges") == 0


# -- Disable People Bar --------------------------------------------------------

_PEOPLE_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced\People"
)


def _apply_taskbar_disable_people(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disabling People bar")
    SESSION.backup([_PEOPLE_KEY], "PeopleBar")
    SESSION.set_dword(_PEOPLE_KEY, "PeopleBand", 0)
    SESSION.log("Taskbar: People bar disabled")


def _remove_taskbar_disable_people(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_PEOPLE_KEY], "PeopleBar_Remove")
    SESSION.set_dword(_PEOPLE_KEY, "PeopleBand", 1)


def _detect_taskbar_disable_people() -> bool:
    return SESSION.read_dword(_PEOPLE_KEY, "PeopleBand") == 0


TWEAKS += [
    TweakDef(
        id="taskbar-disable-notification-badges",
        label="Disable Notification Badge Overlay",
        category="Taskbar",
        apply_fn=_apply_taskbar_disable_notification_badges,
        remove_fn=_remove_taskbar_disable_notification_badges,
        detect_fn=_detect_taskbar_disable_notification_badges,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables unread notification badges on taskbar app icons. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["taskbar", "badges", "notifications"],
    ),
    TweakDef(
        id="taskbar-disable-people",
        label="Disable People Bar on Taskbar",
        category="Taskbar",
        apply_fn=_apply_taskbar_disable_people,
        remove_fn=_remove_taskbar_disable_people,
        detect_fn=_detect_taskbar_disable_people,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PEOPLE_KEY],
        description=(
            "Removes the People bar from the taskbar. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["taskbar", "people", "social", "declutter"],
    ),
]
