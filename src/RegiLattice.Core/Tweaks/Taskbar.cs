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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons", 1)],
        },

        new TweakDef
        {
            Id = "tb-taskbar-hide-task-view",
            Label = "Hide Task View Button",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Hides the Task View button from the taskbar. You can still use Win+Tab for virtual desktops. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "task-view", "declutter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-hide-widgets",
            Label = "Hide Widgets (Policy)",
            Category = "Taskbar",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Widgets board and weather widget via HKLM policy. Frees resources used by the Edge WebView2 widget host. Default: enabled. Recommended: disabled.",
            Tags = ["taskbar", "widgets", "policy", "performance"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0)],
        },

        new TweakDef
        {
            Id = "tb-taskbar-never-combine",
            Label = "Never Combine Taskbar Buttons",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents taskbar from grouping windows of the same app. Each window gets its own button with a visible label. Default: always combine. Recommended: never combine.",
            Tags = ["taskbar", "grouping", "buttons", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 0)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarFlashing", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-end-task",
            Label = "Enable End Task in Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds an End Task option to the taskbar right-click menu for quickly killing unresponsive apps. Default: disabled. Recommended: enabled.",
            Tags = ["taskbar", "end-task", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarEndTask", 0)],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarRecentSearchesEnabled", 0),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarBadges", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideSCAMeetNow", 1)],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock", 1),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-jump-lists",
            Label = "Disable Taskbar Jump Lists",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables jump list and recent program tracking on the taskbar. Prevents Windows from storing recently used file history in taskbar. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["taskbar", "jump-list", "recent", "privacy", "history"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-recent-docs",
            Label = "Disable Recent Documents Tracking",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows from tracking recently opened documents for taskbar jump lists. Reduces filesystem activity and improves privacy. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["taskbar", "recent-docs", "privacy", "history", "tracking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },

        new TweakDef
        {
            Id = "tb-lock-taskbar",
            Label = "Lock Taskbar Position and Size",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Locks the taskbar to prevent accidental resizing or repositioning. Default: Unlocked. Recommended: Locked for stable work environments.",
            Tags = ["taskbar", "lock", "resize", "position", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove", 0)],
        },
        new TweakDef
        {
            Id = "tb-show-all-tray-icons",
            Label = "Always Show All System Tray Icons",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the auto-hide feature for system tray icons. All tray icons are always visible without clicking the expand arrow. Default: Auto-hide. Recommended: Show all for quick access.",
            Tags = ["taskbar", "tray", "icons", "notification-area", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableAutoTray")],
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

        new TweakDef
        {
            Id = "tb-set-button-grouping",
            Label = "Never Group Taskbar Buttons",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the taskbar from grouping similar windows together. Each window gets its own button. Default: always combine.",
            Tags = ["taskbar", "grouping", "buttons", "combine"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel", 2)],
        },

        new TweakDef
        {
            Id = "tb-show-full-path-title",
            Label = "Show Full Path in Explorer Title Bar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows the full folder path in Explorer window title bars, making it easier to identify windows. Default: folder name only.",
            Tags = ["taskbar", "explorer", "path", "title"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\CabinetState", "FullPath", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-cortana-taskbar",
            Label = "Disable Cortana in Taskbar",
            Category = "Taskbar",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Cortana from appearing in the taskbar via Group Policy. Default: enabled.",
            Tags = ["taskbar", "cortana", "disable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-taskbar-animations",
            Label = "Disable Taskbar Animations",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables taskbar button animations (slide, pulse, flash). Reduces visual distractions. Default: enabled.",
            Tags = ["taskbar", "animation", "disable", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
        },
        new TweakDef
        {
            Id = "tb-hide-ink-workspace-button",
            Label = "Hide Ink Workspace Taskbar Button",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Windows Ink Workspace button from the taskbar. Default: visible on pen-enabled devices.",
            Tags = ["taskbar", "ink", "workspace", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace",
                    "PenWorkspaceButtonDesiredVisibility",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "tb-disable-news-feed-taskbar",
            Label = "Disable News & Interests Feed",
            Category = "Taskbar",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the News & Interests feed widget via Group Policy. Removes weather/news from the taskbar. Default: enabled.",
            Tags = ["taskbar", "news", "interests", "feed", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "tb-taskbar-multi-display-show-all",
            Label = "Show Taskbar on All Displays",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the taskbar on all connected monitors in a multi-display setup. Default: primary only on Win11.",
            Tags = ["taskbar", "multi-display", "monitor", "show"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarEnabled", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-thumbnail-preview",
            Label = "Disable Taskbar Thumbnail Previews",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the thumbnail preview popup when hovering over taskbar buttons. Shows tooltip text instead. Default: enabled.",
            Tags = ["taskbar", "thumbnail", "preview", "hover"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 30000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 30000),
            ],
        },
        new TweakDef
        {
            Id = "tb-set-thumbnail-size",
            Label = "Increase Thumbnail Preview Size",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the size of taskbar thumbnail previews from 200 to 350 pixels wide. Default: 200.",
            Tags = ["taskbar", "thumbnail", "size", "preview"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "MaxThumbSizePx", 350)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "MaxThumbSizePx")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "MaxThumbSizePx", 350)],
        },

        new TweakDef
        {
            Id = "tb-hide-clock-from-taskbar",
            Label = "Hide Clock from Taskbar Notification Area",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets HideClock=1 in Explorer policies. Removes the clock and date display from the system tray area of the taskbar, reclaiming tray space.",
            Tags = ["taskbar", "clock", "tray", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideClock", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideClock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HideClock", 1)],
        },
        new TweakDef
        {
            Id = "tb-set-search-icon-only",
            Label = "Show Search as Icon Only (Not Full Box)",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SearchboxTaskbarMode=1 in the Search key. Shows a compact search icon on the taskbar instead of the expanded search box (mode 2) or nothing (mode 0), saving taskbar space.",
            Tags = ["taskbar", "search", "icon", "compact"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
        },
        new TweakDef
        {
            Id = "tb-enable-compact-mode",
            Label = "Enable Compact Taskbar Mode (Windows 11)",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets TaskbarDensity=0 in Explorer Advanced. Switches the Windows 11 taskbar to compact density mode with smaller button padding, reclaiming vertical screen space.",
            Tags = ["taskbar", "compact", "density", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDensity", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDensity", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDensity", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-overflow-menu",
            Label = "Disable Taskbar Icon Overflow Menu",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets EnableTaskbarOverflow=0 in Explorer Advanced. Removes the overflow chevron (\"^\") that appears when too many pinned icons exist, preventing hidden icons from piling up out of sight.",
            Tags = ["taskbar", "overflow", "icons", "tray"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskbarOverflow", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskbarOverflow", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskbarOverflow", 0),
            ],
        },
        new TweakDef
        {
            Id = "tb-hide-language-bar",
            Label = "Hide Language Bar from Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ShowStatus=3 in the CTF LangBar key. Hides the language/input method indicator from the system tray. Useful on single-language machines where the bar wastes tray space.",
            Tags = ["taskbar", "language", "bar", "tray"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar", "ShowStatus", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar", "ShowStatus", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\CTF\LangBar", "ShowStatus", 3)],
        },
        new TweakDef
        {
            Id = "tb-hide-recently-added-apps",
            Label = "Hide Recently Added Apps in Start Menu",
            Category = "Taskbar",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HideRecentlyAddedApps=1 in the Explorer policy. Removes the \"Recently added\" section from the top of the Start Menu's app list, reducing distraction after new software installs.",
            Tags = ["taskbar", "start-menu", "recently-added", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecentlyAddedApps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecentlyAddedApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecentlyAddedApps", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-most-used-apps",
            Label = "Remove Most Used Apps from Start Menu",
            Category = "Taskbar",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoStartMenuMFUprogramsList=1 in the Explorer policy. Hides the \"Most used\" / frequently-used apps section from the Start Menu, producing a cleaner app list.",
            Tags = ["taskbar", "start-menu", "most-used", "apps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoStartMenuMFUprogramsList", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoStartMenuMFUprogramsList")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoStartMenuMFUprogramsList", 1)],
        },
        new TweakDef
        {
            Id = "tb-disable-start-suggestions",
            Label = "Disable App Suggestions in Start Menu",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SystemPaneSuggestionsEnabled=0 in ContentDeliveryManager. Removes Microsoft-promoted app suggestions from appearing in the Start Menu's recommended section.",
            Tags = ["taskbar", "start-menu", "suggestions", "ads"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "tb-hide-show-desktop-button",
            Label = "Hide the Show Desktop Button (Bottom-Right Corner)",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets TaskbarSd=0 in Explorer Advanced. Removes the tiny \"Show Desktop\" peek button in the bottom-right corner of the taskbar, preventing accidental desktop exposure.",
            Tags = ["taskbar", "show-desktop", "button", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", 0)],
        },
        new TweakDef
        {
            Id = "tb-set-multimonitor-local-windows",
            Label = "Show Only Local Windows on Each Monitor's Taskbar",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MMTaskbarMode=2 in Explorer Advanced. When multi-monitor taskbars are enabled, each monitor's taskbar shows only the windows that belong to apps on that monitor.",
            Tags = ["taskbar", "multimonitor", "windows", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MMTaskbarMode", 2)],
        },
        new TweakDef
        {
            Id = "tb-disable-start-track-programs",
            Label = "Stop Tracking Launched Programs in Start Menu",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Start_TrackProgs=0 in Explorer Advanced. Prevents Windows from recording which programs are launched via Start Menu, disabling the Most Used and Recently Added tracking inputs.",
            Tags = ["taskbar", "start-menu", "tracking", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "tb-disable-pinning-to-taskbar",
            Label = "Disable Pinning Apps to Taskbar (Policy)",
            Category = "Taskbar",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets NoPinningToTaskbar=1 in Explorer policies. Prevents items from being pinned to the taskbar by the user or via setup wizards, locking the taskbar layout.",
            Tags = ["taskbar", "pinning", "lock", "policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoPinningToTaskbar", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoPinningToTaskbar")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoPinningToTaskbar", 1)],
        },
    ];
}
