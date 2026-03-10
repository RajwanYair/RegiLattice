"""Widgets & News tweaks.

Covers Windows Widgets panel, News and Interests, tips/suggestions,
Windows Spotlight, Welcome Experience, and content delivery.
"""

from __future__ import annotations

from regilattice.registry import SESSION, assert_admin
from regilattice.tweaks import TweakDef

# ── Key paths ────────────────────────────────────────────────────────────────

_WIDGETS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"
_EXPLORER_ADV = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Explorer\Advanced"
)
_FEEDS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"
_FEEDS_CU = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\Feeds"
)
_CONTENT = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)
_CLOUD_CONTENT = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"
_CLOUD_CU = r"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CloudContent"
_SUGGESTIONS = (
    r"HKEY_CURRENT_USER\Software\Microsoft\Windows"
    r"\CurrentVersion\ContentDeliveryManager"
)
_SPOTLIGHT_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"
_SYSTEM_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


# ── Disable Widgets Panel (Win11) ────────────────────────────────────────────


def _apply_disable_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Widgets: disable Windows Widgets panel")
    SESSION.backup([_WIDGETS_POLICY, _EXPLORER_ADV], "Widgets")
    SESSION.set_dword(_WIDGETS_POLICY, "AllowNewsAndInterests", 0)
    SESSION.set_dword(_EXPLORER_ADV, "TaskbarDa", 0)


def _remove_disable_widgets(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WIDGETS_POLICY, "AllowNewsAndInterests")
    SESSION.set_dword(_EXPLORER_ADV, "TaskbarDa", 1)


def _detect_disable_widgets() -> bool:
    return SESSION.read_dword(_EXPLORER_ADV, "TaskbarDa") == 0


# ── Disable News and Interests (Win10) ───────────────────────────────────────


def _apply_disable_news(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Widgets: disable News and Interests taskbar widget (Win10)")
    SESSION.backup([_FEEDS, _FEEDS_CU], "NewsInterests")
    SESSION.set_dword(_FEEDS, "EnableFeeds", 0)
    SESSION.set_dword(_FEEDS_CU, "ShellFeedsTaskbarViewMode", 2)  # 2 = hidden


def _remove_disable_news(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FEEDS, "EnableFeeds")
    SESSION.set_dword(_FEEDS_CU, "ShellFeedsTaskbarViewMode", 0)  # 0 = icon+text


def _detect_disable_news() -> bool:
    return SESSION.read_dword(_FEEDS, "EnableFeeds") == 0


# ── Disable Tips and Suggestions ─────────────────────────────────────────────


def _apply_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.log("Widgets: disable tips, tricks, and suggestions")
    SESSION.backup([_CONTENT], "TipsSuggestions")
    SESSION.set_dword(_CONTENT, "SubscribedContent-338389Enabled", 0)
    SESSION.set_dword(_CONTENT, "SoftLandingEnabled", 0)


def _remove_disable_tips(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT, "SubscribedContent-338389Enabled")
    SESSION.delete_value(_CONTENT, "SoftLandingEnabled")


def _detect_disable_tips() -> bool:
    return SESSION.read_dword(_CONTENT, "SubscribedContent-338389Enabled") == 0


# ── Disable Windows Spotlight ────────────────────────────────────────────────


def _apply_disable_spotlight(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Widgets: disable Windows Spotlight lock screen images")
    SESSION.backup([_SPOTLIGHT_POLICY, _CONTENT], "Spotlight")
    SESSION.set_dword(_SPOTLIGHT_POLICY, "DisableWindowsSpotlightFeatures", 1)
    SESSION.set_dword(_CONTENT, "RotatingLockScreenEnabled", 0)
    SESSION.set_dword(_CONTENT, "RotatingLockScreenOverlayEnabled", 0)


def _remove_disable_spotlight(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SPOTLIGHT_POLICY, "DisableWindowsSpotlightFeatures")
    SESSION.set_dword(_CONTENT, "RotatingLockScreenEnabled", 1)
    SESSION.delete_value(_CONTENT, "RotatingLockScreenOverlayEnabled")


def _detect_disable_spotlight() -> bool:
    return SESSION.read_dword(_SPOTLIGHT_POLICY, "DisableWindowsSpotlightFeatures") == 1


# ── Disable Welcome Experience ───────────────────────────────────────────────


def _apply_disable_welcome(*, require_admin: bool = False) -> None:
    SESSION.log("Widgets: disable Windows Welcome Experience after updates")
    SESSION.backup([_CONTENT], "WelcomeExperience")
    SESSION.set_dword(_CONTENT, "SubscribedContent-310093Enabled", 0)


def _remove_disable_welcome(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT, "SubscribedContent-310093Enabled")


def _detect_disable_welcome() -> bool:
    return SESSION.read_dword(_CONTENT, "SubscribedContent-310093Enabled") == 0


# ── Disable Get Even More Out of Windows ─────────────────────────────────────


def _apply_disable_get_more(*, require_admin: bool = False) -> None:
    SESSION.log("Widgets: disable 'Get Even More Out of Windows' suggestions")
    SESSION.backup([_CONTENT], "GetMore")
    SESSION.set_dword(_CONTENT, "SubscribedContent-338393Enabled", 0)
    SESSION.set_dword(_CONTENT, "SubscribedContent-353694Enabled", 0)
    SESSION.set_dword(_CONTENT, "SubscribedContent-353696Enabled", 0)


def _remove_disable_get_more(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT, "SubscribedContent-338393Enabled")
    SESSION.delete_value(_CONTENT, "SubscribedContent-353694Enabled")
    SESSION.delete_value(_CONTENT, "SubscribedContent-353696Enabled")


def _detect_disable_get_more() -> bool:
    return SESSION.read_dword(_CONTENT, "SubscribedContent-338393Enabled") == 0


# ── Disable App Suggestion in Start Menu ─────────────────────────────────────


def _apply_disable_start_suggestions(*, require_admin: bool = False) -> None:
    SESSION.log("Widgets: disable suggested apps in Start menu")
    SESSION.backup([_CONTENT], "StartSuggestions")
    SESSION.set_dword(_CONTENT, "SubscribedContent-338388Enabled", 0)
    SESSION.set_dword(_CONTENT, "SystemPaneSuggestionsEnabled", 0)


def _remove_disable_start_suggestions(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT, "SubscribedContent-338388Enabled")
    SESSION.set_dword(_CONTENT, "SystemPaneSuggestionsEnabled", 1)


def _detect_disable_start_suggestions() -> bool:
    return SESSION.read_dword(_CONTENT, "SystemPaneSuggestionsEnabled") == 0


# ── Disable Suggested Content in Settings ────────────────────────────────────


def _apply_disable_settings_suggestions(*, require_admin: bool = False) -> None:
    SESSION.log("Widgets: disable suggested content in Settings app")
    SESSION.backup([_CONTENT], "SettingsSuggestions")
    SESSION.set_dword(_CONTENT, "SubscribedContent-338393Enabled", 0)
    SESSION.set_dword(_CONTENT, "SubscribedContent-353698Enabled", 0)


def _remove_disable_settings_suggestions(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT, "SubscribedContent-338393Enabled")
    SESSION.delete_value(_CONTENT, "SubscribedContent-353698Enabled")


def _detect_disable_settings_suggestions() -> bool:
    return SESSION.read_dword(_CONTENT, "SubscribedContent-353698Enabled") == 0


# ── Disable Cloud Content (Policy) ──────────────────────────────────────────


def _apply_disable_cloud_content(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Widgets: disable all cloud content features via policy")
    SESSION.backup([_CLOUD_CONTENT], "CloudContent")
    SESSION.set_dword(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures", 1)
    SESSION.set_dword(_CLOUD_CONTENT, "DisableSoftLanding", 1)
    SESSION.set_dword(_CLOUD_CONTENT, "DisableCloudOptimizedContent", 1)


def _remove_disable_cloud_content(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures")
    SESSION.delete_value(_CLOUD_CONTENT, "DisableSoftLanding")
    SESSION.delete_value(_CLOUD_CONTENT, "DisableCloudOptimizedContent")


def _detect_disable_cloud_content() -> bool:
    return SESSION.read_dword(_CLOUD_CONTENT, "DisableWindowsConsumerFeatures") == 1


# ── Disable Finish Setup Reminder ────────────────────────────────────────────


def _apply_disable_finish_setup(*, require_admin: bool = False) -> None:
    SESSION.log("Widgets: disable 'Let's finish setting up your device' nag")
    SESSION.backup([_CONTENT], "FinishSetup")
    SESSION.set_dword(_CONTENT, "SubscribedContent-310093Enabled", 0)
    SESSION.set_dword(_CONTENT, "SubscribedContent-338389Enabled", 0)


def _remove_disable_finish_setup(*, require_admin: bool = False) -> None:
    SESSION.delete_value(_CONTENT, "SubscribedContent-310093Enabled")
    SESSION.delete_value(_CONTENT, "SubscribedContent-338389Enabled")


def _detect_disable_finish_setup() -> bool:
    return SESSION.read_dword(_CONTENT, "SubscribedContent-310093Enabled") == 0


# ── Plugin registration ─────────────────────────────────────────────────────

TWEAKS: list[TweakDef] = [
    TweakDef(
        id="widgets-news-disable-widgets-panel",
        label="Disable Widgets Panel (Win11)",
        category="Widgets & News",
        apply_fn=_apply_disable_widgets,
        remove_fn=_remove_disable_widgets,
        detect_fn=_detect_disable_widgets,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_WIDGETS_POLICY, _EXPLORER_ADV],
        description=(
            "Disables the Windows 11 Widgets panel (Weather/News/Sports). "
            "Removes the Widgets button from the taskbar. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "news", "taskbar", "win11"],
    ),
    TweakDef(
        id="widgets-news-disable-news-interests",
        label="Disable News and Interests (Win10)",
        category="Widgets & News",
        apply_fn=_apply_disable_news,
        remove_fn=_remove_disable_news,
        detect_fn=_detect_disable_news,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FEEDS, _FEEDS_CU],
        description=(
            "Disables the News and Interests taskbar widget in Win10. "
            "Removes the weather/news flyout from the taskbar. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "news", "interests", "taskbar", "win10"],
    ),
    TweakDef(
        id="widgets-news-disable-tips-suggestions",
        label="Disable Tips and Suggestions",
        category="Widgets & News",
        apply_fn=_apply_disable_tips,
        remove_fn=_remove_disable_tips,
        detect_fn=_detect_disable_tips,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT],
        description=("Disables Windows tips, tricks, and suggestions notifications. Default: enabled. Recommended: disabled."),
        tags=["widgets", "tips", "suggestions", "notifications"],
    ),
    TweakDef(
        id="widgets-news-disable-spotlight",
        label="Disable Windows Spotlight",
        category="Widgets & News",
        apply_fn=_apply_disable_spotlight,
        remove_fn=_remove_disable_spotlight,
        detect_fn=_detect_disable_spotlight,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_SPOTLIGHT_POLICY, _CONTENT],
        description=(
            "Disables Windows Spotlight (rotating Bing lock screen images). "
            "Also disables the fun facts/tips overlay on the lock screen. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "spotlight", "lockscreen", "bing"],
    ),
    TweakDef(
        id="widgets-news-disable-welcome-experience",
        label="Disable Welcome Experience",
        category="Widgets & News",
        apply_fn=_apply_disable_welcome,
        remove_fn=_remove_disable_welcome,
        detect_fn=_detect_disable_welcome,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT],
        description=(
            "Disables the Windows Welcome Experience page that opens after updates to show new features. Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "welcome", "update", "experience"],
    ),
    TweakDef(
        id="widgets-news-disable-get-more",
        label="Disable 'Get Even More Out of Windows'",
        category="Widgets & News",
        apply_fn=_apply_disable_get_more,
        remove_fn=_remove_disable_get_more,
        detect_fn=_detect_disable_get_more,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT],
        description=(
            "Disables the 'Get Even More Out of Windows' popup and "
            "similar Microsoft 365 / OneDrive nag prompts. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "suggestions", "nag", "onedrive", "m365"],
    ),
    TweakDef(
        id="widgets-news-disable-start-suggestions",
        label="Disable Suggested Apps in Start Menu",
        category="Widgets & News",
        apply_fn=_apply_disable_start_suggestions,
        remove_fn=_remove_disable_start_suggestions,
        detect_fn=_detect_disable_start_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT],
        description=(
            "Disables suggested (promoted) apps in the Start menu. "
            "Stops Microsoft Store app recommendations. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "start", "suggestions", "apps", "ads"],
    ),
    TweakDef(
        id="widgets-news-disable-settings-suggestions",
        label="Disable Suggested Content in Settings",
        category="Widgets & News",
        apply_fn=_apply_disable_settings_suggestions,
        remove_fn=_remove_disable_settings_suggestions,
        detect_fn=_detect_disable_settings_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT],
        description=("Disables suggested content and feature highlights in the Windows Settings app. Default: enabled. Recommended: disabled."),
        tags=["widgets", "settings", "suggestions", "content"],
    ),
    TweakDef(
        id="widgets-news-disable-cloud-consumer-features",
        label="Disable Cloud Consumer Features (Policy)",
        category="Widgets & News",
        apply_fn=_apply_disable_cloud_content,
        remove_fn=_remove_disable_cloud_content,
        detect_fn=_detect_disable_cloud_content,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_CLOUD_CONTENT],
        description=(
            "Disables all Windows consumer features via Group Policy: "
            "cloud-optimized content, soft landing, and consumer features. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "cloud", "consumer", "policy", "bloat"],
    ),
    TweakDef(
        id="widgets-news-disable-finish-setup",
        label="Disable 'Finish Setting Up' Reminder",
        category="Widgets & News",
        apply_fn=_apply_disable_finish_setup,
        remove_fn=_remove_disable_finish_setup,
        detect_fn=_detect_disable_finish_setup,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT],
        description=(
            "Disables the recurring 'Let's finish setting up your device' nag screen after updates. Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "finish-setup", "nag", "reminder"],
    ),
]


# -- Disable News and Interests on Taskbar ----------------------------------------


def _apply_disable_feeds_taskbar(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Widgets: disable news and interests on taskbar")
    SESSION.backup([_FEEDS], "FeedsTaskbar")
    SESSION.set_dword(_FEEDS, "EnableFeeds", 0)


def _remove_disable_feeds_taskbar(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FEEDS, "EnableFeeds")


def _detect_disable_feeds_taskbar() -> bool:
    return SESSION.read_dword(_FEEDS, "EnableFeeds") == 0


# -- Disable Subscribed Content 338388 in Start ----------------------------------


def _apply_disable_subscribed_338388(*, require_admin: bool = True) -> None:
    SESSION.log("Widgets: disable SubscribedContent-338388 in Start")
    SESSION.backup([_CONTENT], "SubscribedContent338388")
    SESSION.set_dword(_CONTENT, "SubscribedContent-338388Enabled", 0)


def _remove_disable_subscribed_338388(*, require_admin: bool = True) -> None:
    SESSION.delete_value(_CONTENT, "SubscribedContent-338388Enabled")


def _detect_disable_subscribed_338388() -> bool:
    return SESSION.read_dword(_CONTENT, "SubscribedContent-338388Enabled") == 0


TWEAKS += [
    TweakDef(
        id="widgets-news-disable-feeds-taskbar",
        label="Disable News and Interests on Taskbar",
        category="Widgets & News",
        apply_fn=_apply_disable_feeds_taskbar,
        remove_fn=_remove_disable_feeds_taskbar,
        detect_fn=_detect_disable_feeds_taskbar,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FEEDS],
        description=(
            "Disables news and interests feed on the Windows taskbar via policy. "
            "Removes the weather/news widget from the taskbar. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "news", "feeds", "taskbar"],
    ),
    TweakDef(
        id="widgets-news-disable-subscribed-content",
        label="Disable Subscribed Content in Start (338388)",
        category="Widgets & News",
        apply_fn=_apply_disable_subscribed_338388,
        remove_fn=_remove_disable_subscribed_338388,
        detect_fn=_detect_disable_subscribed_338388,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_CONTENT],
        description=(
            "Disables SubscribedContent-338388 in the Start menu. "
            "Removes suggested content and app recommendations from Start. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "start", "subscribed", "content", "suggestions"],
    ),
]


# -- Disable News Feed Content ------------------------------------------------


def _apply_disable_news_feed(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Widgets: disable news feed content delivery")
    SESSION.backup([_FEEDS], "NewsFeedContent")
    SESSION.set_dword(_FEEDS, "ContentEnabled", 0)


def _remove_disable_news_feed(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FEEDS, "ContentEnabled")


def _detect_disable_news_feed() -> bool:
    return SESSION.read_dword(_FEEDS, "ContentEnabled") == 0


# -- Disable Widget Background Updates ----------------------------------------


def _apply_disable_widget_bg_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.log("Widgets: disable background content updates")
    SESSION.backup([_WIDGETS_POLICY], "WidgetBgUpdates")
    SESSION.set_dword(_WIDGETS_POLICY, "AllowWidgetContentUpdates", 0)


def _remove_disable_widget_bg_updates(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WIDGETS_POLICY, "AllowWidgetContentUpdates")


def _detect_disable_widget_bg_updates() -> bool:
    return SESSION.read_dword(_WIDGETS_POLICY, "AllowWidgetContentUpdates") == 0


# -- Remove Weather from Taskbar ----------------------------------------------


def _apply_remove_weather_taskbar(*, require_admin: bool = True) -> None:
    SESSION.log("Widgets: hide weather from taskbar")
    SESSION.backup([_FEEDS_CU], "WeatherTaskbar")
    SESSION.set_dword(_FEEDS_CU, "ShellFeedsTaskbarViewMode", 2)


def _remove_remove_weather_taskbar(*, require_admin: bool = True) -> None:
    SESSION.set_dword(_FEEDS_CU, "ShellFeedsTaskbarViewMode", 0)


def _detect_remove_weather_taskbar() -> bool:
    return SESSION.read_dword(_FEEDS_CU, "ShellFeedsTaskbarViewMode") == 2


TWEAKS += [
    TweakDef(
        id="widgets-widget-disable-news-feed",
        label="Disable News Feed Content",
        category="Widgets & News",
        apply_fn=_apply_disable_news_feed,
        remove_fn=_remove_disable_news_feed,
        detect_fn=_detect_disable_news_feed,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_FEEDS],
        description=(
            "Disables the news feed content delivery in Widgets panel via policy. "
            "Stops news articles from loading in the background. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "news", "feed", "content"],
    ),
    TweakDef(
        id="widgets-widget-disable-bg-updates",
        label="Disable Widget Background Updates",
        category="Widgets & News",
        apply_fn=_apply_disable_widget_bg_updates,
        remove_fn=_remove_disable_widget_bg_updates,
        detect_fn=_detect_disable_widget_bg_updates,
        needs_admin=True,
        corp_safe=False,
        registry_keys=[_WIDGETS_POLICY],
        description=(
            "Prevents widgets from updating content in the background. "
            "Reduces network and CPU usage from the Widgets host process. "
            "Default: enabled. Recommended: disabled."
        ),
        tags=["widgets", "background", "updates", "performance"],
    ),
    TweakDef(
        id="widgets-widget-remove-weather-taskbar",
        label="Remove Weather from Taskbar",
        category="Widgets & News",
        apply_fn=_apply_remove_weather_taskbar,
        remove_fn=_remove_remove_weather_taskbar,
        detect_fn=_detect_remove_weather_taskbar,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_FEEDS_CU],
        description=("Hides the weather widget from the taskbar by setting view mode to hidden. Default: shown. Recommended: hidden."),
        tags=["widgets", "weather", "taskbar", "hide"],
    ),
]

# ── Extra widgets / news controls ─────────────────────────────────────────────

_FEEDS_MACHINE = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"
_TIPS_POLICY = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"
_WIN_TIPS = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"
_SEARCH_HIGHLIGHTS = r"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"
_START_FEEDS = r"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"


def _apply_disable_machine_feeds(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_FEEDS_MACHINE], "MachineFeeds")
    SESSION.set_dword(_FEEDS_MACHINE, "EnableFeeds", 0)


def _remove_disable_machine_feeds(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_FEEDS_MACHINE, "EnableFeeds")


def _detect_disable_machine_feeds() -> bool:
    return SESSION.read_dword(_FEEDS_MACHINE, "EnableFeeds") == 0


def _apply_disable_third_party_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_WIN_TIPS], "ThirdPartySuggestions")
    SESSION.set_dword(_WIN_TIPS, "ThirdPartySuggestionsEnabled", 0)


def _remove_disable_third_party_suggestions(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_WIN_TIPS, "ThirdPartySuggestionsEnabled")


def _detect_disable_third_party_suggestions() -> bool:
    return SESSION.read_dword(_WIN_TIPS, "ThirdPartySuggestionsEnabled") == 0


def _apply_disable_search_highlights(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.backup([_SEARCH_HIGHLIGHTS], "SearchHighlights")
    SESSION.set_dword(_SEARCH_HIGHLIGHTS, "IsDynamicSearchBoxEnabled", 0)


def _remove_disable_search_highlights(*, require_admin: bool = False) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_SEARCH_HIGHLIGHTS, "IsDynamicSearchBoxEnabled")


def _detect_disable_search_highlights() -> bool:
    return SESSION.read_dword(_SEARCH_HIGHLIGHTS, "IsDynamicSearchBoxEnabled") == 0


def _apply_disable_spotlight_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_TIPS_POLICY], "SpotlightSuggestions")
    SESSION.set_dword(_TIPS_POLICY, "DisableWindowsSpotlightFeatures", 1)


def _remove_disable_spotlight_suggestions(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_TIPS_POLICY, "DisableWindowsSpotlightFeatures")


def _detect_disable_spotlight_suggestions() -> bool:
    return SESSION.read_dword(_TIPS_POLICY, "DisableWindowsSpotlightFeatures") == 1


def _apply_disable_start_feeds_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.backup([_START_FEEDS], "StartFeedsPolicy")
    SESSION.set_dword(_START_FEEDS, "DisablePersonalization", 1)


def _remove_disable_start_feeds_policy(*, require_admin: bool = True) -> None:
    assert_admin(require_admin)
    SESSION.delete_value(_START_FEEDS, "DisablePersonalization")


def _detect_disable_start_feeds_policy() -> bool:
    return SESSION.read_dword(_START_FEEDS, "DisablePersonalization") == 1


TWEAKS += [
    TweakDef(
        id="widgets-disable-machine-feeds",
        label="Disable Windows Feeds via Machine Policy",
        category="Widgets & News",
        apply_fn=_apply_disable_machine_feeds,
        remove_fn=_remove_disable_machine_feeds,
        detect_fn=_detect_disable_machine_feeds,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_FEEDS_MACHINE],
        description=(
            "Disables Windows Feeds (News and Interests) via machine-level policy, "
            "applying to all users on the system. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["widgets", "feeds", "news", "policy", "machine"],
    ),
    TweakDef(
        id="widgets-disable-third-party-suggestions",
        label="Disable Third-Party App Suggestions",
        category="Widgets & News",
        apply_fn=_apply_disable_third_party_suggestions,
        remove_fn=_remove_disable_third_party_suggestions,
        detect_fn=_detect_disable_third_party_suggestions,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_WIN_TIPS],
        description=(
            "Disables Windows from showing suggestions for third-party apps in search and feeds. "
            "Reduces promotional content. Default: Enabled. Recommended: Disabled."
        ),
        tags=["widgets", "suggestions", "third-party", "ads", "privacy"],
    ),
    TweakDef(
        id="widgets-disable-search-highlights",
        label="Disable Dynamic Search Box Highlights",
        category="Widgets & News",
        apply_fn=_apply_disable_search_highlights,
        remove_fn=_remove_disable_search_highlights,
        detect_fn=_detect_disable_search_highlights,
        needs_admin=False,
        corp_safe=True,
        registry_keys=[_SEARCH_HIGHLIGHTS],
        description=(
            "Disables the dynamic search box highlights that show trending topics. "
            "Removes news and promotional content from the search interface. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["widgets", "search", "highlights", "news", "bing"],
    ),
    TweakDef(
        id="widgets-disable-spotlight-features",
        label="Disable Windows Spotlight Features (Policy)",
        category="Widgets & News",
        apply_fn=_apply_disable_spotlight_suggestions,
        remove_fn=_remove_disable_spotlight_suggestions,
        detect_fn=_detect_disable_spotlight_suggestions,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_TIPS_POLICY],
        description=(
            "Disables all Windows Spotlight features via policy, including lock screen, "
            "start menu, and Action Center Spotlight content. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["widgets", "spotlight", "lock-screen", "policy", "content"],
    ),
    TweakDef(
        id="widgets-disable-start-personalization",
        label="Disable Start Menu Personalization / Feeds",
        category="Widgets & News",
        apply_fn=_apply_disable_start_feeds_policy,
        remove_fn=_remove_disable_start_feeds_policy,
        detect_fn=_detect_disable_start_feeds_policy,
        needs_admin=True,
        corp_safe=True,
        registry_keys=[_START_FEEDS],
        description=(
            "Disables Start menu personalization that shows recommended apps and ads. "
            "Keeps the Start menu clean and static. "
            "Default: Enabled. Recommended: Disabled."
        ),
        tags=["widgets", "start", "personalization", "feeds", "privacy"],
    ),
]
