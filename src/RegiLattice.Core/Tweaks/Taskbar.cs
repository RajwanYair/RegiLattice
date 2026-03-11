namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Taskbar
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tb-taskbar-align-left",
            Label = "Align Taskbar Left (Win11)",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Windows 11 taskbar alignment to left instead of center. Default: center. Recommended: left.",
            Tags = ["taskbar", "alignment", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-small-icons",
            Label = "Use Small Taskbar Icons",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shrinks taskbar icons and reduces taskbar height (Win10). Default: large icons. Recommended: small icons.",
            Tags = ["taskbar", "icons", "size"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-search",
            Label = "Hide Taskbar Search Box",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the search box or icon from the taskbar. You can still search by pressing Win+S. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "search", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-task-view",
            Label = "Hide Task View Button",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Task View button from the taskbar. You can still use Win+Tab for virtual desktops. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "task-view", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-widgets",
            Label = "Hide Widgets (Policy)",
            Category = "Taskbar",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Widgets board and weather widget via HKLM policy. Frees resources used by the Edge WebView2 widget host. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "widgets", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 1),
            ],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-chat",
            Label = "Hide Chat / Teams Icon",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Microsoft Teams Chat icon from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "chat", "teams", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-copilot",
            Label = "Hide Copilot Button",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Copilot button from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "copilot", "ai", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCopilotButton", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-never-combine",
            Label = "Never Combine Taskbar Buttons",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents taskbar from grouping windows of the same app. Each window gets its own button with a visible label. Default: always combine. Recommended: never combine.",
            Tags = ["taskbar", "grouping", "buttons", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-badges",
            Label = "Disable Notification Badges",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables unread message count badges on taskbar app icons. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "badges", "notifications", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-flashing",
            Label = "Disable Taskbar Button Flashing",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops taskbar buttons from flashing to get your attention. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "flashing", "focus", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-end-task",
            Label = "Enable End Task in Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds an End Task option to the taskbar right-click menu for quickly killing unresponsive apps. Default: disabled. Recommended: enabled.",
            Tags = ["taskbar", "end-task", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 1)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-recent-search",
            Label = "Disable Recent Searches in Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables recent search suggestions shown in the taskbar search box. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "search", "privacy", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarRecentSearchesEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarRecentSearchesEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarRecentSearchesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-notification-badges",
            Label = "Disable Notification Badge Overlay",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables unread notification badges on taskbar app icons. Default: Enabled. Recommended: Disabled.",
            Tags = ["taskbar", "badges", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-disable-people",
            Label = "Disable People Bar on Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the People bar from the taskbar. Default: Enabled. Recommended: Disabled.",
            Tags = ["taskbar", "people", "social", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-weather-widget",
            Label = "Disable Taskbar Weather Widget",
            Category = "Taskbar",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the News and Interests weather widget on the taskbar via group policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["taskbar", "weather", "widget", "feeds", "news"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
        },
        new TweakDef
        {
            Id = "tb-disable-meet-now",
            Label = "Disable Meet Now Icon",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Meet Now (Skype) icon from the taskbar notification area. Default: Shown. Recommended: Hidden.",
            Tags = ["taskbar", "meet-now", "skype", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow", 1)],
        },
        new TweakDef
        {
            Id = "tb-set-button-grouping",
            Label = "Set Taskbar Button Grouping (Never Combine)",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets taskbar buttons to never combine, showing full labels for each window. Default: Always combine (0). Recommended: Never combine (2).",
            Tags = ["taskbar", "grouping", "combine", "buttons", "labels"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
        },
        new TweakDef
        {
            Id = "tb-show-seconds-clock",
            Label = "Show Seconds in System Clock",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows seconds in the taskbar system clock for precision timing. Default: Hidden. Recommended: Personal preference.",
            Tags = ["taskbar", "clock", "seconds", "time"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-animations",
            Label = "Disable Taskbar Animations",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables taskbar button animations for a snappier feel. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["taskbar", "animations", "performance", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-jump-lists",
            Label = "Disable Taskbar Jump Lists",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables jump list and recent program tracking on the taskbar. Prevents Windows from storing recently used file history in taskbar. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["taskbar", "jump-list", "recent", "privacy", "history"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-recent-docs",
            Label = "Disable Recent Documents Tracking",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows from tracking recently opened documents for taskbar jump lists. Reduces filesystem activity and improves privacy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["taskbar", "recent-docs", "privacy", "history", "tracking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-aero-peek",
            Label = "Disable Aero Peek Preview",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Aero Peek desktop preview when hovering over the Show Desktop button. Eliminates accidental desktop reveals. Default: Enabled. Recommended: Personal preference.",
            Tags = ["taskbar", "aero-peek", "preview", "desktop", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "tb-lock-taskbar",
            Label = "Lock Taskbar Position and Size",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Locks the taskbar to prevent accidental resizing or repositioning. Default: Unlocked. Recommended: Locked for stable work environments.",
            Tags = ["taskbar", "lock", "resize", "position", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0)],
        },
        new TweakDef
        {
            Id = "tb-show-all-tray-icons",
            Label = "Always Show All System Tray Icons",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the auto-hide feature for system tray icons. All tray icons are always visible without clicking the expand arrow. Default: Auto-hide. Recommended: Show all for quick access.",
            Tags = ["taskbar", "tray", "icons", "notification-area", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray", 0)],
        },
        new TweakDef
        {
            Id = "tb-set-taskbar-small-icons",
            Label = "Use Small Taskbar Icons",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the taskbar to use small icons, increasing available space. Only works on Windows 10. Default: large.",
            Tags = ["taskbar", "small", "icons", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-news-and-interests",
            Label = "Disable News and Interests on Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the News and Interests (weather) widget on the taskbar. Windows 10 only. Default: shown.",
            Tags = ["taskbar", "news", "interests", "weather"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2)],
        },
        new TweakDef
        {
            Id = "tb-disable-taskbar-people",
            Label = "Disable People Button on Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the People button from the taskbar. Default: shown.",
            Tags = ["taskbar", "people", "contacts", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
        },
        new TweakDef
        {
            Id = "tb-move-taskbar-left",
            Label = "Align Taskbar Icons to Left (Windows 11)",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Aligns taskbar icons to the left side instead of centered. Windows 11 only. Default: center.",
            Tags = ["taskbar", "alignment", "left", "windows-11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
        },
    ];
}
