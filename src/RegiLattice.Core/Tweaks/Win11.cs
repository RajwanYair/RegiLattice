namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Win11
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "w11-disable-widgets",
            Label = "Disable Widgets (News & Interests)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Removes the Widgets button from the taskbar and disables the feed.",
            Tags = ["win11", "taskbar", "debloat"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-snap-assist",
            Label = "Disable Snap Assist & Flyout",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Snap Assist suggestion panel and Snap Bar when hovering maximize.",
            Tags = ["win11", "window-management", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
        },
        new TweakDef
        {
            Id = "w11-classic-context-menu",
            Label = "Classic Right-Click Context Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Restores the full Windows 10 right-click context menu, bypassing the truncated Windows 11 menu.",
            Tags = ["win11", "context-menu", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
        },
        new TweakDef
        {
            Id = "w11-disable-lockscreen-tips",
            Label = "Disable Lock Screen Tips & Spotlight",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Spotlight, lock screen tips, suggested apps, and silent app installs.",
            Tags = ["win11", "lockscreen", "spotlight", "debloat"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
        },
        new TweakDef
        {
            Id = "w11-disable-wu-autorestart",
            Label = "Disable Windows Update Auto-Restart",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows Update from auto-rebooting while a user is logged in. Sets updates to notify-before-download.",
            Tags = ["win11", "update", "reboot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
        },
        new TweakDef
        {
            Id = "w11-disable-bing-search",
            Label = "Disable Bing Search in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Start Menu from sending search queries to Bing.",
            Tags = ["win11", "search", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
        },
        new TweakDef
        {
            Id = "w11-disable-app-suggestions",
            Label = "Disable App Suggestions & Bloatware",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested apps in Start Menu and prevents OEM/pre-installed app promotions.",
            Tags = ["win11", "start-menu", "debloat"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
        },
        new TweakDef
        {
            Id = "w11-dark-mode",
            Label = "Enable System-Wide Dark Mode",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets both app and system theme to dark mode.",
            Tags = ["win11", "theme", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
        },
        new TweakDef
        {
            Id = "w11-disable-notifications",
            Label = "Disable Toast Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all toast/push notifications system-wide.",
            Tags = ["win11", "notifications", "focus"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications"],
        },
        new TweakDef
        {
            Id = "w11-disable-snap-flyout",
            Label = "Disable Snap Layout Flyout",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the snap layout flyout shown on maximize button hover.",
            Tags = ["win11", "snap", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
        },
        new TweakDef
        {
            Id = "w11-disable-taskbar-chat",
            Label = "Disable Taskbar Chat Icon",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Teams Chat icon from the Windows 11 taskbar.",
            Tags = ["win11", "taskbar", "teams"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
        },
        new TweakDef
        {
            Id = "w11-win11-disable-widgets",
            Label = "Disable Widgets",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows 11 Widgets board and Weather widget on the taskbar. Frees resources used by the Edge WebView2 widget host. Default: Enabled. Recommended: Disabled.",
            Tags = ["win11", "widgets", "performance", "taskbar"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"],
        },
        new TweakDef
        {
            Id = "w11-win11-disable-suggested-actions",
            Label = "Disable Suggested Actions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Suggested Actions popup when copying dates, phone numbers, etc. Removes the clipboard suggestion overlay. Default: Enabled. Recommended: Disabled.",
            Tags = ["win11", "suggested-actions", "clipboard", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "w11-restore-right-click",
            Label = "Restore Classic Right-Click Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Restores Windows 10 classic context menu on right-click. Default: Win11 modern menu. Recommended: Classic.",
            Tags = ["win11", "context-menu", "right-click", "classic"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-chat-icon",
            Label = "Disable Chat/Teams Taskbar Icon",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Chat/Teams icon from the Windows 11 taskbar. Default: Shown. Recommended: Hidden.",
            Tags = ["win11", "chat", "teams", "taskbar"],
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
            Id = "w11-end-task-context",
            Label = "Enable End Task in Taskbar Right-Click",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds an 'End Task' option to the taskbar right-click context menu for quickly killing unresponsive apps. Win11 23H2+. Default: disabled. Recommended: enabled.",
            Tags = ["win11", "taskbar", "end-task", "24h2"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            MinBuild = 22631,
        },
        new TweakDef
        {
            Id = "w11-start-more-pins",
            Label = "Start Menu: Show More Pins",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Changes the Start Menu layout to show more pinned apps and fewer recommendations. Default: balanced. Recommended: more pins.",
            Tags = ["win11", "start-menu", "pins", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
        },
        new TweakDef
        {
            Id = "w11-disable-recommendations",
            Label = "Disable Start Menu Recommendations",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 'Recommended' section in the Win11 Start Menu that shows recently opened files and suggested apps. Default: enabled. Recommended: disabled.",
            Tags = ["win11", "start-menu", "recommendations", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-cross-device",
            Label = "Disable Cross-Device Resume",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Cross-Device Resume (CDP) which syncs activities across devices linked to the same Microsoft account. Win11 24H2+. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["win11", "cross-device", "cdp", "privacy", "24h2"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            MinBuild = 26100,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-sync-notifications",
            Label = "Disable Sync Provider Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables OneDrive and other sync provider advertising notifications in File Explorer. Default: shown. Recommended: disabled.",
            Tags = ["win11", "sync", "notifications", "onedrive", "explorer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSyncProviderNotifications", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSyncProviderNotifications", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSyncProviderNotifications", 0)],
        },
        new TweakDef
        {
            Id = "w11-never-combine-taskbar",
            Label = "Never Combine Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets taskbar to never combine app buttons, showing each window individually with its label. Win11 23H2+. Default: always combine. Recommended: never combine.",
            Tags = ["win11", "taskbar", "combine", "buttons"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            MinBuild = 22631,
        },
        new TweakDef
        {
            Id = "w11-hide-settings-home",
            Label = "Hide Settings Home Page",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides the Windows Settings home page that shows recommendations and account promotions. Settings opens to System instead. Win11 24H2+. Default: shown. Recommended: hidden.",
            Tags = ["win11", "settings", "home", "24h2", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            MinBuild = 26100,
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "SettingsPageVisibility"),
            ],
        },
        new TweakDef
        {
            Id = "w11-taskbar-left",
            Label = "Align Taskbar to the Left",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Aligns the Windows 11 taskbar icons to the left instead of center. Default: center. Recommended: personal preference.",
            Tags = ["win11", "taskbar", "alignment", "left"],
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
            Id = "w11-disable-rounded-corners",
            Label = "Disable Rounded Window Corners",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows 11 rounded window corners by disabling DWM frame staging buffer. Gives a sharper, square-corner look. Default: enabled. Recommended: personal preference.",
            Tags = ["win11", "rounded-corners", "dwm", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "UseWindowFrameStagingBuffer", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "UseWindowFrameStagingBuffer"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "UseWindowFrameStagingBuffer", 0)],
        },
        new TweakDef
        {
            Id = "w11-classic-taskbar-context",
            Label = "Restore Classic Taskbar Context Menu",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restores the classic taskbar right-click context menu with full options like toolbars and Task Manager via Group Policy. Default: modern menu. Recommended: classic.",
            Tags = ["win11", "taskbar", "context-menu", "classic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicTaskbarContextMenu", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicTaskbarContextMenu"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicTaskbarContextMenu", 1)],
        },
        new TweakDef
        {
            Id = "w11-disable-suggested-actions-policy",
            Label = "Disable Suggested Actions (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows 11 Suggested Actions popup via Group Policy. Machine-wide enforcement for managed environments. Default: enabled. Recommended: disabled.",
            Tags = ["win11", "suggested-actions", "policy", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "w11-disable-start-account-notif",
            Label = "Disable Start Menu Account Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables account-related notifications in the Windows 11 Start menu (e.g. backup reminders, Microsoft 365 upsells). Default: Enabled. Recommended: Disabled.",
            Tags = ["win11", "start-menu", "account", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_AccountNotifications", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_AccountNotifications"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_AccountNotifications", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-dynamic-lighting",
            Label = "Disable Dynamic Lighting",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows 11 dynamic lighting (ambient RGB control). Prevents Windows from controlling peripheral RGB lighting. Default: Enabled. Recommended: Disabled if unused.",
            Tags = ["win11", "lighting", "rgb", "ambient", "peripherals"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Lighting"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Lighting", "AmbientLightingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Lighting", "AmbientLightingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Lighting", "AmbientLightingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-recent-start",
            Label = "Disable Recent Items in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables tracking and display of recently opened documents in the Start menu. Improves privacy. Default: Enabled. Recommended: Disabled.",
            Tags = ["win11", "start-menu", "recent", "privacy", "tracking"],
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
            Id = "w11-taskbar-never-combine",
            Label = "Never Combine Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets taskbar button combining to 'Never' (each window gets its own button). Default: combine when taskbar is full. Recommended: Never for power users.",
            Tags = ["win11", "taskbar", "combine", "ux", "windows"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
        },
        new TweakDef
        {
            Id = "w11-disable-search-highlights",
            Label = "Disable Search Box / Highlights on Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Search box/icon on the taskbar and disables 'Search highlights' (Bing-powered trending topics). Default: visible. Recommended: hidden for clean taskbar.",
            Tags = ["win11", "search", "taskbar", "bing", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-spotlight-tips",
            Label = "Disable Windows Spotlight Tips & Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables content delivery (Spotlight tips, app suggestions, lock-screen ads). Default: enabled. Recommended: disabled for privacy and clean experience.",
            Tags = ["win11", "spotlight", "ads", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContentEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContentEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContentEnabled", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-copilot-taskbar-btn",
            Label = "Disable Copilot Button on Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Copilot button from the Windows 11 taskbar via policy. Default: shown (Win11 23H2+). Recommended: disabled if not using Copilot.",
            Tags = ["win11", "copilot", "taskbar", "ai", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\WindowsCopilot"],
        },
        new TweakDef
        {
            Id = "w11-disable-recall-ai",
            Label = "Disable Windows Recall AI (Snapshots)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Recall (AI-powered screenshot indexing/analysis). Prevents continuous screen capture and local AI indexing. Default: may be enabled on Copilot+ PCs. Recommended: disabled for privacy.",
            Tags = ["win11", "recall", "ai", "privacy", "snapshot", "copilot-plus"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"],
        },
        new TweakDef
        {
            Id = "w11-show-this-pc-on-desktop",
            Label = "Show This PC Icon on Desktop",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the 'This PC' icon on the desktop (hidden by default in Windows 11). Default: hidden. Recommended: shown for quick storage access.",
            Tags = ["win11", "desktop", "this-pc", "icons", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel"],
        },
    ];
}
