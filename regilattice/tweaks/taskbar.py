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
        id="tb-taskbar-align-left",
        label="Align Taskbar Left (Win11)",
        category="Taskbar",
        apply_fn=_apply_align_left,
        remove_fn=_remove_align_left,
        detect_fn=_detect_align_left,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Sets Windows 11 taskbar alignment to left instead of center. Default: center. Recommended: left."),
        tags=["taskbar", "alignment", "win11"],
    ),
    TweakDef(
        id="tb-taskbar-small-icons",
        label="Use Small Taskbar Icons",
        category="Taskbar",
        apply_fn=_apply_small_icons,
        remove_fn=_remove_small_icons,
        detect_fn=_detect_small_icons,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Shrinks taskbar icons and reduces taskbar height (Win10). Default: large icons. Recommended: small icons."),
        tags=["taskbar", "icons", "size"],
    ),
    TweakDef(
        id="tb-taskbar-hide-search",
        label="Hide Taskbar Search Box",
        category="Taskbar",
        apply_fn=_apply_hide_search,
        remove_fn=_remove_hide_search,
        detect_fn=_detect_hide_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH],
        description=(
            "Hides the search box or icon from the taskbar. You can still search by pressing Win+S. Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "search", "declutter"],
    ),
    TweakDef(
        id="tb-taskbar-hide-task-view",
        label="Hide Task View Button",
        category="Taskbar",
        apply_fn=_apply_hide_task_view,
        remove_fn=_remove_hide_task_view,
        detect_fn=_detect_hide_task_view,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Hides the Task View button from the taskbar. You can still use Win+Tab for virtual desktops. Default: enabled. Recommended: disabled."
        ),
        tags=["taskbar", "task-view", "declutter"],
    ),
    TweakDef(
        id="tb-taskbar-hide-widgets",
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
        id="tb-taskbar-hide-chat",
        label="Hide Chat / Teams Icon",
        category="Taskbar",
        apply_fn=_apply_hide_chat,
        remove_fn=_remove_hide_chat,
        detect_fn=_detect_hide_chat,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Hides the Microsoft Teams Chat icon from the taskbar. Default: enabled. Recommended: disabled."),
        tags=["taskbar", "chat", "teams", "declutter"],
    ),
    TweakDef(
        id="tb-taskbar-hide-copilot",
        label="Hide Copilot Button",
        category="Taskbar",
        apply_fn=_apply_hide_copilot,
        remove_fn=_remove_hide_copilot,
        detect_fn=_detect_hide_copilot,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Hides the Copilot button from the taskbar. Default: enabled. Recommended: disabled."),
        tags=["taskbar", "copilot", "ai", "declutter"],
    ),
    TweakDef(
        id="tb-taskbar-never-combine",
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
        id="tb-taskbar-disable-badges",
        label="Disable Notification Badges",
        category="Taskbar",
        apply_fn=_apply_disable_badges,
        remove_fn=_remove_disable_badges,
        detect_fn=_detect_disable_badges,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Disables unread message count badges on taskbar app icons. Default: enabled. Recommended: disabled."),
        tags=["taskbar", "badges", "notifications", "declutter"],
    ),
    TweakDef(
        id="tb-taskbar-disable-flashing",
        label="Disable Taskbar Button Flashing",
        category="Taskbar",
        apply_fn=_apply_disable_flashing,
        remove_fn=_remove_disable_flashing,
        detect_fn=_detect_disable_flashing,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Stops taskbar buttons from flashing to get your attention. Default: enabled. Recommended: disabled."),
        tags=["taskbar", "flashing", "focus", "ux"],
    ),
    TweakDef(
        id="tb-taskbar-end-task",
        label="Enable End Task in Taskbar",
        category="Taskbar",
        apply_fn=_apply_end_task,
        remove_fn=_remove_end_task,
        detect_fn=_detect_end_task,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Adds an End Task option to the taskbar right-click menu for quickly killing unresponsive apps. Default: disabled. Recommended: enabled."
        ),
        tags=["taskbar", "end-task", "productivity"],
    ),
    TweakDef(
        id="tb-taskbar-disable-recent-search",
        label="Disable Recent Searches in Taskbar",
        category="Taskbar",
        apply_fn=_apply_disable_recent_search,
        remove_fn=_remove_disable_recent_search,
        detect_fn=_detect_disable_recent_search,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH],
        description=("Disables recent search suggestions shown in the taskbar search box. Default: enabled. Recommended: disabled."),
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
        id="tb-taskbar-disable-notification-badges",
        label="Disable Notification Badge Overlay",
        category="Taskbar",
        apply_fn=_apply_taskbar_disable_notification_badges,
        remove_fn=_remove_taskbar_disable_notification_badges,
        detect_fn=_detect_taskbar_disable_notification_badges,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Disables unread notification badges on taskbar app icons. Default: Enabled. Recommended: Disabled."),
        tags=["taskbar", "badges", "notifications"],
    ),
    TweakDef(
        id="tb-taskbar-disable-people",
        label="Disable People Bar on Taskbar",
        category="Taskbar",
        apply_fn=_apply_taskbar_disable_people,
        remove_fn=_remove_taskbar_disable_people,
        detect_fn=_detect_taskbar_disable_people,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_PEOPLE_KEY],
        description=("Removes the People bar from the taskbar. Default: Enabled. Recommended: Disabled."),
        tags=["taskbar", "people", "social", "declutter"],
    ),
]


# -- Disable Taskbar Weather Widget (Feeds) ------------------------------------

_FEEDS_KEY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"


def _apply_tb_disable_weather(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disabling weather widget (News and Interests)")
    SESSION.backup([_FEEDS_KEY], "TBWeatherWidget")
    SESSION.set_dword(_FEEDS_KEY, "EnableFeeds", 0)
    SESSION.log("Taskbar: weather widget disabled")


def _remove_tb_disable_weather(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_FEEDS_KEY], "TBWeatherWidget_Remove")
    SESSION.delete_value(_FEEDS_KEY, "EnableFeeds")


def _detect_tb_disable_weather() -> bool:
    return SESSION.read_dword(_FEEDS_KEY, "EnableFeeds") == 0


# -- Disable Meet Now Icon ------------------------------------------------------

_MEET_NOW_KEY = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Policies\Explorer"
)


def _apply_tb_disable_meet_now(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: hiding Meet Now icon")
    SESSION.backup([_MEET_NOW_KEY], "TBMeetNow")
    SESSION.set_dword(_MEET_NOW_KEY, "HideSCAMeetNow", 1)
    SESSION.log("Taskbar: Meet Now icon hidden")


def _remove_tb_disable_meet_now(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_MEET_NOW_KEY], "TBMeetNow_Remove")
    SESSION.delete_value(_MEET_NOW_KEY, "HideSCAMeetNow")


def _detect_tb_disable_meet_now() -> bool:
    return SESSION.read_dword(_MEET_NOW_KEY, "HideSCAMeetNow") == 1


# -- Set Taskbar Button Grouping (Never Combine) --------------------------------


def _apply_tb_button_grouping(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: setting button grouping to never combine")
    SESSION.backup([_ADV], "TBButtonGrouping")
    SESSION.set_dword(_ADV, "TaskbarGlomLevel", 2)
    SESSION.log("Taskbar: button grouping set to never combine")


def _remove_tb_button_grouping(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_ADV], "TBButtonGrouping_Remove")
    SESSION.set_dword(_ADV, "TaskbarGlomLevel", 0)


def _detect_tb_button_grouping() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarGlomLevel") == 2


TWEAKS += [
    TweakDef(
        id="tb-disable-weather-widget",
        label="Disable Taskbar Weather Widget",
        category="Taskbar",
        apply_fn=_apply_tb_disable_weather,
        remove_fn=_remove_tb_disable_weather,
        detect_fn=_detect_tb_disable_weather,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FEEDS_KEY],
        description=("Disables the News and Interests weather widget on the taskbar via group policy. Default: Enabled. Recommended: Disabled."),
        tags=["taskbar", "weather", "widget", "feeds", "news"],
    ),
    TweakDef(
        id="tb-disable-meet-now",
        label="Disable Meet Now Icon",
        category="Taskbar",
        apply_fn=_apply_tb_disable_meet_now,
        remove_fn=_remove_tb_disable_meet_now,
        detect_fn=_detect_tb_disable_meet_now,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_MEET_NOW_KEY],
        description=("Hides the Meet Now (Skype) icon from the taskbar notification area. Default: Shown. Recommended: Hidden."),
        tags=["taskbar", "meet-now", "skype", "declutter"],
    ),
    TweakDef(
        id="tb-set-button-grouping",
        label="Set Taskbar Button Grouping (Never Combine)",
        category="Taskbar",
        apply_fn=_apply_tb_button_grouping,
        remove_fn=_remove_tb_button_grouping,
        detect_fn=_detect_tb_button_grouping,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Sets taskbar buttons to never combine, showing full labels for each window. Default: Always combine (0). Recommended: Never combine (2)."
        ),
        tags=["taskbar", "grouping", "combine", "buttons", "labels"],
    ),
]


# ══ Additional Taskbar Tweaks ══════════════════════════════════════════


def _apply_tb_show_seconds_clock(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: show seconds in system clock")
    SESSION.backup([_ADV], "TBSecondsClock")
    SESSION.set_dword(_ADV, "ShowSecondsInSystemClock", 1)


def _remove_tb_show_seconds_clock(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "ShowSecondsInSystemClock")


def _detect_tb_show_seconds_clock() -> bool:
    return SESSION.read_dword(_ADV, "ShowSecondsInSystemClock") == 1


def _apply_tb_disable_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disable taskbar animations")
    SESSION.backup([_ADV], "TBAnimations")
    SESSION.set_dword(_ADV, "TaskbarAnimations", 0)


def _remove_tb_disable_animations(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "TaskbarAnimations")


def _detect_tb_disable_animations() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarAnimations") == 0


TWEAKS += [
    TweakDef(
        id="tb-show-seconds-clock",
        label="Show Seconds in System Clock",
        category="Taskbar",
        apply_fn=_apply_tb_show_seconds_clock,
        remove_fn=_remove_tb_show_seconds_clock,
        detect_fn=_detect_tb_show_seconds_clock,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Shows seconds in the taskbar system clock for precision timing. Default: Hidden. Recommended: Personal preference."),
        tags=["taskbar", "clock", "seconds", "time"],
    ),
    TweakDef(
        id="tb-disable-animations",
        label="Disable Taskbar Animations",
        category="Taskbar",
        apply_fn=_apply_tb_disable_animations,
        remove_fn=_remove_tb_disable_animations,
        detect_fn=_detect_tb_disable_animations,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=("Disables taskbar button animations for a snappier feel. Default: Enabled. Recommended: Disabled for performance."),
        tags=["taskbar", "animations", "performance", "ui"],
    ),
]


# ── Taskbar Jump Lists, Aero Peek & Tray Tweaks ───────────────────────────────


def _apply_tb_disable_jump_lists(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disable jump lists and recent program tracking")
    SESSION.backup([_ADV], "TBJumpLists")
    SESSION.set_dword(_ADV, "Start_TrackProgs", 0)


def _remove_tb_disable_jump_lists(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "Start_TrackProgs")


def _detect_tb_disable_jump_lists() -> bool:
    return SESSION.read_dword(_ADV, "Start_TrackProgs") == 0


def _apply_tb_disable_recent_docs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disable recent documents tracking in jump lists")
    SESSION.backup([_ADV], "TBRecentDocs")
    SESSION.set_dword(_ADV, "Start_TrackDocs", 0)


def _remove_tb_disable_recent_docs(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "Start_TrackDocs")


def _detect_tb_disable_recent_docs() -> bool:
    return SESSION.read_dword(_ADV, "Start_TrackDocs") == 0


def _apply_tb_disable_aero_peek(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: disable Aero Peek on taskbar hover")
    SESSION.backup([_ADV], "TBAeroPeek")
    SESSION.set_dword(_ADV, "EnableAeroPeek", 0)


def _remove_tb_disable_aero_peek(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "EnableAeroPeek")


def _detect_tb_disable_aero_peek() -> bool:
    return SESSION.read_dword(_ADV, "EnableAeroPeek") == 0


def _apply_tb_lock_taskbar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: lock taskbar from being moved or resized")
    SESSION.backup([_ADV], "TBLock")
    SESSION.set_dword(_ADV, "TaskbarSizeMove", 0)


def _remove_tb_lock_taskbar(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "TaskbarSizeMove")


def _detect_tb_lock_taskbar() -> bool:
    return SESSION.read_dword(_ADV, "TaskbarSizeMove") == 0


def _apply_tb_show_all_tray_icons(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.log("Taskbar: always show all system tray icons")
    SESSION.backup([_ADV], "TBAllTrayIcons")
    SESSION.set_dword(_ADV, "EnableAutoTray", 0)


def _remove_tb_show_all_tray_icons(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_ADV, "EnableAutoTray")


def _detect_tb_show_all_tray_icons() -> bool:
    return SESSION.read_dword(_ADV, "EnableAutoTray") == 0


TWEAKS += [
    TweakDef(
        id="tb-disable-jump-lists",
        label="Disable Taskbar Jump Lists",
        category="Taskbar",
        apply_fn=_apply_tb_disable_jump_lists,
        remove_fn=_remove_tb_disable_jump_lists,
        detect_fn=_detect_tb_disable_jump_lists,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables jump list and recent program tracking on the taskbar. "
            "Prevents Windows from storing recently used file history in taskbar. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["taskbar", "jump-list", "recent", "privacy", "history"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="tb-disable-recent-docs",
        label="Disable Recent Documents Tracking",
        category="Taskbar",
        apply_fn=_apply_tb_disable_recent_docs,
        remove_fn=_remove_tb_disable_recent_docs,
        detect_fn=_detect_tb_disable_recent_docs,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables Windows from tracking recently opened documents for taskbar jump lists. "
            "Reduces filesystem activity and improves privacy. "
            "Default: Enabled. Recommended: Disabled for privacy."
        ),
        tags=["taskbar", "recent-docs", "privacy", "history", "tracking"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="tb-disable-aero-peek",
        label="Disable Aero Peek Preview",
        category="Taskbar",
        apply_fn=_apply_tb_disable_aero_peek,
        remove_fn=_remove_tb_disable_aero_peek,
        detect_fn=_detect_tb_disable_aero_peek,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables the Aero Peek desktop preview when hovering over the Show Desktop button. "
            "Eliminates accidental desktop reveals. "
            "Default: Enabled. Recommended: Personal preference."
        ),
        tags=["taskbar", "aero-peek", "preview", "desktop", "ui"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="tb-lock-taskbar",
        label="Lock Taskbar Position and Size",
        category="Taskbar",
        apply_fn=_apply_tb_lock_taskbar,
        remove_fn=_remove_tb_lock_taskbar,
        detect_fn=_detect_tb_lock_taskbar,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Locks the taskbar to prevent accidental resizing or repositioning. Default: Unlocked. Recommended: Locked for stable work environments."
        ),
        tags=["taskbar", "lock", "resize", "position", "ui"],
        depends_on=[],
        side_effects="",
    ),
    TweakDef(
        id="tb-show-all-tray-icons",
        label="Always Show All System Tray Icons",
        category="Taskbar",
        apply_fn=_apply_tb_show_all_tray_icons,
        remove_fn=_remove_tb_show_all_tray_icons,
        detect_fn=_detect_tb_show_all_tray_icons,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_ADV],
        description=(
            "Disables the auto-hide feature for system tray icons. "
            "All tray icons are always visible without clicking the expand arrow. "
            "Default: Auto-hide. Recommended: Show all for quick access."
        ),
        tags=["taskbar", "tray", "icons", "notification-area", "ui"],
        depends_on=[],
        side_effects="",
    ),
]
