namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Win11
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "w11-win11-disable-suggested-actions",
            Label = "Disable Suggested Actions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Suggested Actions popup when copying dates, phone numbers, etc. Removes the clipboard suggestion overlay. Default: Enabled. Recommended: Disabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-rounded-corners",
            Label = "Disable Rounded Window Corners",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows 11 rounded window corners by disabling DWM frame staging buffer. Gives a sharper, square-corner look. Default: enabled. Recommended: personal preference.",
            Tags = ["win11", "rounded-corners", "dwm", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "UseWindowFrameStagingBuffer", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "UseWindowFrameStagingBuffer")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "UseWindowFrameStagingBuffer", 0)],
        },
        new TweakDef
        {
            Id = "w11-classic-taskbar-context",
            Label = "Restore Classic Taskbar Context Menu",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Restores the classic taskbar right-click context menu with full options like toolbars and Task Manager via Group Policy. Default: modern menu. Recommended: classic.",
            Tags = ["win11", "taskbar", "context-menu", "classic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicTaskbarContextMenu", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicTaskbarContextMenu")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicTaskbarContextMenu", 1)],
        },
        new TweakDef
        {
            Id = "w11-disable-start-account-notif",
            Label = "Disable Start Menu Account Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables account-related notifications in the Windows 11 Start menu (e.g. backup reminders, Microsoft 365 upsells). Default: Enabled. Recommended: Disabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_AccountNotifications", 0),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-dynamic-lighting",
            Label = "Disable Dynamic Lighting",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows 11 dynamic lighting (ambient RGB control). Prevents Windows from controlling peripheral RGB lighting. Default: Enabled. Recommended: Disabled if unused.",
            Tags = ["win11", "lighting", "rgb", "ambient", "peripherals"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Lighting"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Lighting", "AmbientLightingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Lighting", "AmbientLightingEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Lighting", "AmbientLightingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-spotlight-tips",
            Label = "Disable Windows Spotlight Tips & Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables content delivery (Spotlight tips, app suggestions, lock-screen ads). Default: enabled. Recommended: disabled for privacy and clean experience.",
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
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContentEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-restore-classic-context-menu",
            Label = "Restore Classic Context Menu (Win11)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Restores the classic full right-click context menu in Windows 11, removing the simplified modern menu. Default: modern.",
            Tags = ["win11", "context-menu", "classic", "right-click"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", "")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", ""),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-recommended-section",
            Label = "Disable Recommended Section in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Recommended section in the Windows 11 Start menu. Default: shown.",
            Tags = ["win11", "start-menu", "recommended", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecommendedSection", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecommendedSection")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "HideRecommendedSection", 1)],
        },
        new TweakDef
        {
            Id = "w11-enable-end-task-taskbar",
            Label = "Enable End Task in Taskbar Right-Click",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds an 'End Task' option to the taskbar right-click menu in Windows 11. Default: hidden.",
            Tags = ["win11", "taskbar", "end-task", "context-menu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings",
                    "TaskbarEndTask",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings",
                    "TaskbarEndTask",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings",
                    "TaskbarEndTask",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-snap-bar",
            Label = "Disable Snap Bar When Dragging Windows",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the snap layout bar that appears at the top of the screen when dragging a window. Default: enabled.",
            Tags = ["win11", "snap", "bar", "drag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-voice-access-autostart",
            Label = "Disable Voice Access Auto-Start",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Voice Access from starting automatically on sign-in. Default: off.",
            Tags = ["win11", "voice-access", "autostart", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Accessibility\VoiceAccess"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Accessibility\VoiceAccess",
                    "StartVoiceAccessOnLogon",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Accessibility\VoiceAccess",
                    "StartVoiceAccessOnLogon"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Accessibility\VoiceAccess",
                    "StartVoiceAccessOnLogon",
                    0
                ),
            ],
        },
        // ── Restored stubs with real registry operations ──────────────────

        new TweakDef
        {
            Id = "w11-classic-context-menu",
            Label = "Classic Context Menu (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Restores the classic full context menu via machine-level policy. Complements the per-user CLSID approach.",
            Tags = ["win11", "context-menu", "policy", "classic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicMenu", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicMenu")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "ForceClassicMenu", 1)],
        },
        new TweakDef
        {
            Id = "w11-disable-app-suggestions",
            Label = "Disable App Suggestions in Start",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from suggesting apps to install in the Start menu and Settings.",
            Tags = ["win11", "start-menu", "suggestions", "debloat"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-lockscreen-tips",
            Label = "Disable Lock Screen Tips & Ads",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from showing tips, tricks and ads on the lock screen.",
            Tags = ["win11", "lockscreen", "tips", "ads", "debloat"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338387Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338387Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "RotatingLockScreenOverlayEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-notifications",
            Label = "Disable All Toast Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all toast/banner notifications from apps and system.",
            Tags = ["win11", "notifications", "toast", "focus"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-snap-flyout",
            Label = "Disable Snap Layout Flyout",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the snap layout flyout that appears when hovering over the maximize button.",
            Tags = ["win11", "snap", "flyout", "maximize", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-taskbar-chat",
            Label = "Disable Taskbar Chat (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Teams Chat icon on the taskbar via Group Policy. Complements the per-user TaskbarMn approach.",
            Tags = ["win11", "taskbar", "chat", "teams", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Chat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Chat", "ChatIcon", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Chat", "ChatIcon")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Chat", "ChatIcon", 3)],
        },
        new TweakDef
        {
            Id = "w11-end-task-context",
            Label = "End Task in Taskbar Context Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22631,
            Description = "Adds an 'End Task' option to the taskbar right-click context menu for running apps. Requires 23H2+.",
            Tags = ["win11", "taskbar", "end-task", "developer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings",
                    "TaskbarEndTask",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings",
                    "TaskbarEndTask"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\TaskbarDeveloperSettings",
                    "TaskbarEndTask",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-hide-settings-home",
            Label = "Hide Settings Home Page",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            MinBuild = 22631,
            Description = "Hides the new 'Home' landing page in Settings, opening directly to System instead.",
            Tags = ["win11", "settings", "home", "debloat"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                    "SettingsPageVisibility",
                    "hide:home"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "SettingsPageVisibility"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                    "SettingsPageVisibility",
                    "hide:home"
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-restore-right-click",
            Label = "Restore Full Right-Click (User)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Restores the Windows 10 full context menu by registering a shell extension override for the current user.",
            Tags = ["win11", "context-menu", "right-click", "classic"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", "")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", ""),
            ],
        },
        new TweakDef
        {
            Id = "w11-show-this-pc-on-desktop",
            Label = "Show 'This PC' on Desktop",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows the 'This PC' icon on the desktop. Default: hidden in Windows 11.",
            Tags = ["win11", "desktop", "this-pc", "icon"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel",
                    "{20D04FE0-3AEA-1069-A2D8-08002B30309D}",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel",
                    "{20D04FE0-3AEA-1069-A2D8-08002B30309D}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel",
                    "{20D04FE0-3AEA-1069-A2D8-08002B30309D}",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-start-more-pins",
            Label = "Start Menu — More Pins Layout",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22621,
            Description = "Switches the Start menu to show more pinned apps and fewer recommendations.",
            Tags = ["win11", "start-menu", "pins", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_Layout", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_Layout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_Layout", 1)],
        },
        new TweakDef
        {
            Id = "w11-taskbar-never-combine",
            Label = "Never Combine Taskbar (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents taskbar button grouping via Group Policy. Applies to all users on the machine.",
            Tags = ["win11", "taskbar", "combine", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoTaskGrouping", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoTaskGrouping")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoTaskGrouping", 1)],
        },
        new TweakDef
        {
            Id = "w11-start-app-list-off",
            Label = "Hide All Apps List in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets Start_ShowClassicMode=0 to hide the full 'All apps' list shortcut from the Start menu, keeping only pinned items visible by default. Default: 0.",
            Tags = ["win11", "start-menu", "apps", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowClassicMode", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowClassicMode"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowClassicMode", 0),
            ],
        },
        new TweakDef
        {
            Id = "w11-settings-ads-off",
            Label = "Disable Ads in Settings App",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets SubscribedContent-338393Enabled=0 to suppress promotional and recommended content displayed in the Windows Settings app. Default: 1 (ads shown).",
            Tags = ["win11", "settings", "ads", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "w11-disable-notification-center",
            Label = "Disable Notification Center (Action Center)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Disables the Windows 11 Notification Center (Action Center) so no notification panel appears when clicking the system clock/tray. Reduces background notification processing. Default: enabled.",
            Tags = ["w11", "notifications", "action-center", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter", 1)],
        },
        new TweakDef
        {
            Id = "w11-disable-balloon-tips",
            Label = "Disable Taskbar Balloon Tips / Toast Hints",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables balloon/toast tip notifications shown by the taskbar (e.g., 'You have unused desktop icons'). Cleans up the notification area from informational nags. Default: enabled.",
            Tags = ["w11", "balloon", "tips", "taskbar", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-shutdown-tracking",
            Label = "Disable Shutdown Reason Tracker Dialog",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the 'Shutdown Event Tracker' dialog prompting for a reason when shutting down or restarting. Speeds up shutdown workflow. Default: optional on client Windows; mandatory on server OS.",
            Tags = ["w11", "shutdown", "dialog", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "ShutdownReasonUI", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "ShutdownReasonUI")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Reliability", "ShutdownReasonUI", 0)],
        },
        new TweakDef
        {
            Id = "w11-disable-startup-sound",
            Label = "Disable Windows Startup Sound",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows startup chime sound that plays during user login. Silences the audio on boot for quiet environments. Default: system startup sound plays on login.",
            Tags = ["w11", "startup", "sound", "audio"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableStartupSound", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableStartupSound")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableStartupSound", 1)],
        },
    ];
}

// === Merged from: Cortana.cs ===

internal static class Cortana
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cortana-disable-cortana-lockscreen",
            Label = "Disable Cortana on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Cortana voice assistant on the lock screen.",
            Tags = ["cortana", "privacy", "lockscreen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-web-search-start",
            Label = "Disable Web Search in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Bing web results in Start menu search via CurrentUser registry keys.",
            Tags = ["search", "privacy", "bing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "CortanaConsent", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "CortanaConsent"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-highlights-policy",
            Label = "Disable Search Highlights",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the Bing-curated 'Search Highlights' content from the Windows search box.",
            Tags = ["search", "bing", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-hide-search-box",
            Label = "Hide Taskbar Search Box",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the search box / icon from the taskbar.",
            Tags = ["search", "taskbar", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SearchboxTaskbarMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SearchboxTaskbarMode", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SearchboxTaskbarMode", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-search-aadmsa",
            Label = "Disable AAD/MSA Cloud Search",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AAD and MSA cloud search content in Windows Search results.",
            Tags = ["search", "cortana", "cloud", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-find-my-files",
            Label = "Disable Enhanced Search (Find My Files)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets search mode to classic, disabling enhanced Find My Files indexing.",
            Tags = ["search", "cortana", "indexing", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "SearchMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "SearchMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "SearchMode", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-metered",
            Label = "Disable Search Over Metered Connections",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows Search from using web search over metered or pay-per-use network connections. Default: Enabled. Recommended: Disabled to save bandwidth.",
            Tags = ["cortana", "search", "metered", "bandwidth", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "ConnectedSearchUseWebOverMeteredConnections",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "ConnectedSearchUseWebOverMeteredConnections"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                    "ConnectedSearchUseWebOverMeteredConnections",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-voice-activation",
            Label = "Disable Voice Activation",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables voice activation for Cortana and speech services. Prevents the microphone from listening for wake words. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "voice", "activation", "microphone", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences", "VoiceActivationOn", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences", "VoiceActivationOn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Preferences", "VoiceActivationOn", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-battery",
            Label = "Disable Search Indexing on Battery",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the search indexer from running on battery power. Saves battery on laptops. Default: continue on battery.",
            Tags = ["cortana", "search", "indexer", "battery", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-safe-search",
            Label = "Disable Safe Search",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Safe Search content filtering in Windows Search results. Default: moderate filtering.",
            Tags = ["cortana", "search", "safe-search", "filter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings", "SafeSearchMode", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings", "SafeSearchMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings", "SafeSearchMode", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-outlook",
            Label = "Disable Search Indexing of Outlook",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Search from indexing Microsoft Outlook data. Reduces CPU and disk usage. Default: indexed.",
            Tags = ["cortana", "search", "index", "outlook"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
        },
        new TweakDef
        {
            Id = "cortana-block-bing-in-start",
            Label = "Block Bing Results in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Bing web results from appearing in Windows Start menu search. Local-only search results. Default: Bing results shown.",
            Tags = ["cortana", "bing", "start", "search"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "cortana-block-web-results-policy",
            Label = "Block Web Search via Group Policy",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables web search in Windows Search via Group Policy. Strongest method to prevent web queries from desktop search. Default: allowed.",
            Tags = ["cortana", "web", "search", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "cortana-disable-search-in-store",
            Label = "Disable Search the Microsoft Store",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Search the Microsoft Store' link in Windows Search results. Removes promotional app suggestions.",
            Tags = ["cortana", "search", "store", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoUseStoreOpenWith", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoUseStoreOpenWith")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoUseStoreOpenWith", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-windows-copilot",
            Label = "Disable Windows Copilot in Search",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Copilot integration in Windows Search. Prevents AI-powered suggestions and Bing Chat results from appearing.",
            Tags = ["cortana", "copilot", "ai", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot", "TurnOffWindowsCopilot", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-transparency",
            Label = "Disable Search Box Transparency",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables transparency effects in the Windows Search flyout. Improves rendering performance on integrated GPUs.",
            Tags = ["cortana", "search", "transparency", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTransparencyEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTransparencyEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTransparencyEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-removable",
            Label = "Disable Search Indexing on Removable Drives",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Search from indexing removable drives (USB sticks, external HDDs). Reduces I/O on external media.",
            Tags = ["cortana", "indexing", "removable", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableRemovableDriveIndexing", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableRemovableDriveIndexing"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableRemovableDriveIndexing", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-encrypted",
            Label = "Disable Indexing of Encrypted Files",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Search from indexing encrypted (EFS) files. Reduces indexing overhead and potential data exposure in the search index.",
            Tags = ["cortana", "indexing", "encrypted", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowIndexingEncryptedStoresOrItems", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-preview-pane",
            Label = "Disable Search Preview Pane",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the preview pane in Windows Search that shows file contents and web results. Speeds up search UI rendering.",
            Tags = ["cortana", "search", "preview", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarPreview", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarPreview")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarPreview", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-can-enable",
            Label = "Prevent Cortana from Being Enabled (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CanCortanaBeEnabled=0 via Windows Search policy. Blocks the Cortana service from being enabled even by users with admin rights.",
            Tags = ["cortana", "search", "policy", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "CanCortanaBeEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "CanCortanaBeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "CanCortanaBeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-lock-screen-search",
            Label = "Disable Search Box on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets IsLockScreenSearchEnabled=0 in SearchSettings. Removes the search field from the lock screen, reducing the attack surface for unauthenticated searches.",
            Tags = ["cortana", "search", "lock-screen", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsLockScreenSearchEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsLockScreenSearchEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsLockScreenSearchEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-personalized-search",
            Label = "Disable Personalised Search Results",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets IsPersonalSearchEnabled=0 in SearchSettings. Turns off personalised ranking of search results based on past activity.",
            Tags = ["cortana", "search", "personalization", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsPersonalSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsPersonalSearchEnabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsPersonalSearchEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-copilot-in-search",
            Label = "Disable Copilot / AI Assistant in Search",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets IsAssistantEnabled=0 in SearchSettings. Removes the Bing Copilot AI answer panel from Windows Search results.",
            Tags = ["cortana", "search", "copilot", "ai", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAssistantEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAssistantEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAssistantEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-spelling-in-search",
            Label = "Disable Spelling Correction in Search",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets EnableSpellingCorrection=0 in SearchSettings. Stops Windows Search from auto-correcting query spelling, which would otherwise expand or alter the intended search.",
            Tags = ["cortana", "search", "spelling", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "EnableSpellingCorrection", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "EnableSpellingCorrection"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "EnableSpellingCorrection", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-online-tips-search",
            Label = "Disable Online Tips in Search Results",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets OnlineTipsEnabled=0 in SearchSettings. Prevents Windows Search from appending online troubleshooting tips to local search results.",
            Tags = ["cortana", "search", "tips", "online"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "OnlineTipsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "OnlineTipsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "OnlineTipsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-office-indexing",
            Label = "Disable Search Indexing of Microsoft Office Files (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets PreventIndexingMicrosoftOffice=1 in Windows Search policy. Stops the indexer from crawling Office documents, reducing background I/O on systems where Office file search is not needed.",
            Tags = ["cortana", "search", "indexer", "office", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingMicrosoftOffice", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingMicrosoftOffice"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingMicrosoftOffice", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-unc-indexing",
            Label = "Disable Search Indexing of UNC / Network Paths",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingUNCCrawledPaths=1 in Windows Search policy. Stops the indexer from traversing UNC shares, eliminating network bandwidth consumption from background crawl.",
            Tags = ["cortana", "search", "indexer", "unc", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingUNCCrawledPaths", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingUNCCrawledPaths"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingUNCCrawledPaths", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-public-folder-indexing",
            Label = "Disable Search Indexing of Public Folders",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets PreventIndexingPublicFolders=1 in Windows Search policy. Stops the indexer from crawling shared Public folders, reducing unnecessary read I/O.",
            Tags = ["cortana", "search", "indexer", "public-folders"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingPublicFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingPublicFolders")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingPublicFolders", 1),
            ],
        },
        new TweakDef
        {
            Id = "cortana-gpo-block-bing-answers",
            Label = "Disable Bing Answers in Windows Search (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableBingAnswers=0 in Windows Search policy. Prevents Bing from returning inline answer cards (weather, calculations, sports scores) in the local search panel.",
            Tags = ["cortana", "search", "bing", "answers", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableBingAnswers", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableBingAnswers")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableBingAnswers", 0)],
        },
    ];
}

// ── Merged from IndexingSearch.cs ──────────────────────────────────────────────────

internal static class IndexingSearch
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "idx-disable-search-indexer",
            Label = "Disable Windows Search Indexer",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable Windows Search indexer service entirely. Saves CPU/disk but disables fast search. Default: enabled.",
            Tags = ["indexer", "search", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
        },
        new TweakDef
        {
            Id = "idx-disable-indexer-backoff",
            Label = "Disable Indexer Low-Disk Backoff",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevent search indexer from pausing when disk space is low. Default: backs off.",
            Tags = ["indexer", "disk", "space", "backoff"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingLowDiskSpaceMB", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingLowDiskSpaceMB"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingLowDiskSpaceMB", 0),
            ],
        },
        new TweakDef
        {
            Id = "idx-disable-search-suggestions",
            Label = "Disable Search Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable dynamic search suggestions. Default: enabled.",
            Tags = ["search", "suggestion", "dynamic"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "idx-disable-search-location",
            Label = "Disable Location for Search",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevent search from using device location. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["location", "search", "privacy", "gps"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowSearchToUseLocation", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowSearchToUseLocation")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowSearchToUseLocation", 0)],
        },
        new TweakDef
        {
            Id = "idx-reduce-indexer-io",
            Label = "Reduce Indexer Disk I/O",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Gathering Manager disk-space threshold to 5 GB, causing the indexer to back off earlier and reduce disk I/O pressure. Default: not set. Recommended: Apply on systems with slow disks.",
            Tags = ["search", "indexer", "disk", "io", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager", "DesiredRemainingDiskSpaceMB", 5000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager", "DesiredRemainingDiskSpaceMB")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager", "DesiredRemainingDiskSpaceMB", 5000),
            ],
        },
        new TweakDef
        {
            Id = "idx-limit-indexer-threads",
            Label = "Limit Indexer CPU Threads",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Limits the Windows Search indexer to 1 worker thread via GatheringMaxServerThreadCount, reducing CPU load during burst indexing. Default: uncapped. Recommended: Apply on dual-core systems.",
            Tags = ["search", "indexer", "cpu", "performance", "threads"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager", "GatheringMaxServerThreadCount", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager", "GatheringMaxServerThreadCount"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search\Gathering Manager", "GatheringMaxServerThreadCount", 1),
            ],
        },
        new TweakDef
        {
            Id = "idx-disable-safe-search",
            Label = "Disable SafeSearch Filter",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SafeSearch=0 in Windows Search settings, disabling the content filter that restricts explicit content in search results. Default: moderate (1). Recommended: off for unrestricted results.",
            Tags = ["search", "safe-search", "filter", "content"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SafeSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SafeSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SafeSearch", 0)],
        },
        new TweakDef
        {
            Id = "idx-disable-network-index",
            Label = "Disable Indexing of Network Locations",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows Search from indexing mapped network drives and UNC paths via policy. Reduces indexer CPU and network load. Default: allowed. Recommended: Disabled on slow or corporate networks.",
            Tags = ["search", "network", "indexer", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingNetworkLocations", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingNetworkLocations"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingNetworkLocations", 1),
            ],
        },
        new TweakDef
        {
            Id = "idx-disable-msa-cloud-search",
            Label = "Disable Microsoft Account Cloud Search",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Microsoft Account cloud search integration in Windows Search. Prevents OneDrive and MSA content from appearing in local search results. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["search", "cloud", "msa", "microsoft-account", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "IsMSACloudSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "IsMSACloudSearchEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "IsMSACloudSearchEnabled", 0)],
        },
    ];
}

// ── merged from AppCompatibility.cs ────────────────────────────────────────
internal static class AppCompatibility
{
    private const string CuKey = @"HKEY_CURRENT_USER";
    private const string LmKey = @"HKEY_LOCAL_MACHINE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "compat-disable-compatibility-telemetry",
            Label = "Disable Application Compatibility Telemetry",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Application Experience telemetry component (CompatTelRunner.exe) that collects app usage data.",
            Tags = ["compatibility", "telemetry", "privacy", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "AITEnable", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-program-compatibility-assistant",
            Label = "Disable Program Compatibility Assistant",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables PCA which monitors programs and offers to apply compatibility fixes. Can slow down program launches.",
            Tags = ["compatibility", "performance", "pca"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePCA", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-steps-recorder",
            Label = "Disable Steps Recorder (PSR)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Problem Steps Recorder (PSR.exe) used for recording user actions for troubleshooting. Reduces background resource usage.",
            Tags = ["compatibility", "privacy", "recorder", "troubleshooting"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-inventory-collector",
            Label = "Disable Application Inventory Collector",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the inventory collector that scans installed applications and sends data to Microsoft for compatibility assessment.",
            Tags = ["compatibility", "telemetry", "privacy", "inventory"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableInventory", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-engine",
            Label = "Disable Compatibility Engine",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Completely disables the Application Compatibility Engine. Programs will run without any compatibility shims applied.",
            Tags = ["compatibility", "performance", "engine"],
            SideEffects = "Some older applications may not function correctly.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisableEngine", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-switchback",
            Label = "Disable SwitchBack Compatibility",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables SwitchBack compatibility, which reverts some system behaviour for older apps. Improves consistency on modern systems.",
            Tags = ["compatibility", "performance", "switchback"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "SbEnable", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "SbEnable")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "SbEnable", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-web-search-in-run",
            Label = "Disable Web Search in Run Dialog",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Run dialog from searching the web when a command is not found locally.",
            Tags = ["compatibility", "privacy", "search", "run"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRunasInstallPrompt", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRunasInstallPrompt")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRunasInstallPrompt", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-fault-tolerant-heap",
            Label = "Disable Fault Tolerant Heap",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Fault Tolerant Heap (FTH) which Windows enables for apps that crash frequently. FTH adds overhead.",
            Tags = ["compatibility", "performance", "heap", "memory"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\FTH"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\FTH", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\FTH", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-customer-experience",
            Label = "Disable Customer Experience Improvement Program",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows CEIP which collects hardware and software usage data for quality improvement.",
            Tags = ["compatibility", "telemetry", "privacy", "ceip"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows", "CEIPEnable", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\SQMClient\Windows", "CEIPEnable", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-smart-screen-apps",
            Label = "Disable SmartScreen for Downloaded Apps",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Disables SmartScreen checking for apps downloaded from the Internet. Speeds up app launches.",
            Tags = ["compatibility", "smartscreen", "security", "performance"],
            SideEffects = "Reduced protection against unrecognised executables.",
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer", "SmartScreenEnabled", "Off")],
            RemoveOps = [RegOp.SetString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer", "SmartScreenEnabled", "Warn")],
            DetectOps = [RegOp.CheckString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer", "SmartScreenEnabled", "Off")],
        },
        new TweakDef
        {
            Id = "compat-disable-app-launch-tracking",
            Label = "Disable App Launch Tracking",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables tracking of application launches used for Start menu personalisation and telemetry.",
            Tags = ["compatibility", "privacy", "tracking", "start-menu"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
            RemoveOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 1)],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-autoplay-devices",
            Label = "Disable AutoPlay for Non-Volume Devices",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AutoPlay for non-volume devices like cameras and phones. Prevents automatic import prompts.",
            Tags = ["compatibility", "security", "autoplay", "devices"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutoplayfornonVolume", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutoplayfornonVolume")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutoplayfornonVolume", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-maintenance-wakeup",
            Label = "Disable Automatic Maintenance Wake-Up",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows automatic maintenance from waking the computer from sleep.",
            Tags = ["compatibility", "performance", "maintenance", "sleep", "power"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance", "WakeUp", 0)],
        },
        new TweakDef
        {
            Id = "compat-force-classic-shutdown",
            Label = "Force Classic Shutdown Dialog",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces the classic shutdown dialog (Alt+F4 on desktop) instead of the modern one.",
            Tags = ["compatibility", "ui", "shutdown", "classic"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoWinKeys", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoWinKeys")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoWinKeys", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-background-apps",
            Label = "Disable Background Apps (Global)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents UWP/Store apps from running in the background. Reduces CPU and memory usage from idle apps.",
            Tags = ["compatibility", "performance", "background", "uwp", "memory"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 1)],
            RemoveOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 0)],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 1),
            ],
        },
        new TweakDef
        {
            Id = "compat-disable-tips-suggestions",
            Label = "Disable Tips and Suggestions Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and suggestion notifications that appear in the Action Center.",
            Tags = ["compatibility", "notifications", "tips", "suggestions", "ui"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SubscribedContent-338389Enabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "compat-disable-shim-database",
            Label = "Disable Application Shim Database (SDB)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the application compatibility shim database that applies runtime fixes for older software.",
            Tags = ["compatibility", "performance", "shim", "legacy"],
            SideEffects = "Some older applications may not function correctly without shims.",
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePropPage", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePropPage")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "DisablePropPage", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-wer-server-connection",
            Label = "Disable WER Server Connection",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Error Reporting from connecting to Microsoft servers to upload crash dumps and diagnostic data.",
            Tags = ["compatibility", "crash", "wer", "privacy", "telemetry"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting", "Disabled", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-compat-telemetry-runner",
            Label = "Disable CompatTelRunner Scheduled Tasks",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Microsoft Compatibility Appraiser and CompatTelRunner tasks that upload telemetry during idle time.",
            Tags = ["compatibility", "telemetry", "scheduled-task", "privacy"],
            ApplyAction = dryRun =>
            {
                if (!dryRun)
                {
                    ShellRunner.Run(
                        "schtasks",
                        ["/Change", "/TN", @"Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser", "/DISABLE"]
                    );
                    ShellRunner.Run("schtasks", ["/Change", "/TN", @"Microsoft\Windows\Application Experience\ProgramDataUpdater", "/DISABLE"]);
                }
            },
            RemoveAction = dryRun =>
            {
                if (!dryRun)
                {
                    ShellRunner.Run(
                        "schtasks",
                        ["/Change", "/TN", @"Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser", "/ENABLE"]
                    );
                    ShellRunner.Run("schtasks", ["/Change", "/TN", @"Microsoft\Windows\Application Experience\ProgramDataUpdater", "/ENABLE"]);
                }
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run(
                    "schtasks",
                    ["/Query", "/TN", @"Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser", "/FO", "LIST"]
                );
                return stdout.Contains("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "compat-disable-user-choice-protection",
            Label = "Disable User Choice Protection Driver (UCPD)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the User Choice Protection Driver that Microsoft installs to prevent changing default browser/app associations via registry.",
            Tags = ["compatibility", "defaults", "ucpd", "browser"],
            SideEffects = "Microsoft may re-enable this periodically via Windows Update.",
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\UCPD", "Start", 4)],
        },
        new TweakDef
        {
            Id = "compat-disable-vdm-allowed",
            Label = "Disable 16-bit DOS Application Support (VDM)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Virtual DOS Machine (NTVDM/VDM) that allows 16-bit legacy applications to run on 64-bit Windows.",
            Tags = ["compatibility", "legacy", "vdm", "dos", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "VDMDisallowed", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "VDMDisallowed")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\AppCompat", "VDMDisallowed", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-app-repkg-service",
            Label = "Disable App Repackaging Service",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Application Repackage Service used for automatic compatibility assessment, reducing background CPU usage.",
            Tags = ["compatibility", "performance", "background", "service"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppReadiness", "Start", 4)],
        },
        new TweakDef
        {
            Id = "compat-disable-install-service",
            Label = "Disable Application Identity Service",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Application Identity service (AppIDSvc) used by AppLocker for code integrity checks.",
            Tags = ["compatibility", "applocker", "service", "appid"],
            SideEffects = "AppLocker policies will not function if this service is disabled.",
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AppIDSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "compat-disable-just-in-time-debugging",
            Label = "Disable Just-In-Time (JIT) Debugger",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the JIT debugger entry so that application crashes don't prompt 'Would you like to debug?' dialog boxes.",
            Tags = ["compatibility", "debugging", "crash", "ux"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug"],
            ApplyOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Debugger")],
            RemoveOps =
            [
                RegOp.SetString($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Debugger", @"vsjitdebugger.exe -p %ld -e %ld"),
            ],
            DetectOps = [RegOp.CheckMissing($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug", "Debugger")],
        },
        new TweakDef
        {
            Id = "compat-enable-dep-always-on",
            Label = "Enable DEP Always-On (All Programs)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces Data Execution Prevention (DEP/NX) for all processes, not just Windows system components, increasing exploit mitigation.",
            Tags = ["compatibility", "dep", "security", "exploit", "hardening"],
            SideEffects = "Some very old or poorly-written applications may crash with DEP enabled.",
            ApplyAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.Run("bcdedit", ["/set", "nx", "AlwaysOn"]);
            },
            RemoveAction = dryRun =>
            {
                if (!dryRun)
                    ShellRunner.Run("bcdedit", ["/set", "nx", "OptIn"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit", ["/enum", "{current}"]);
                return stdout.Contains("nx                      AlwaysOn", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "compat-disable-error-reporting-ui",
            Label = "Disable Error Reporting UI Dialog",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the 'Report problem to Microsoft?' dialog box shown after application crashes.",
            Tags = ["compatibility", "wer", "crash", "ux", "privacy"],
            RegistryKeys = [$@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
            RemoveOps = [RegOp.DeleteValue($@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI")],
            DetectOps = [RegOp.CheckDword($@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "compat-disable-ie-compat-view",
            Label = "Disable IE Compatibility View List Updates",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic updates to the IE Compatibility View List from Microsoft, preventing background internet checks.",
            Tags = ["compatibility", "ie", "internet-explorer", "privacy", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation", "DisableIECompatView", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation", "DisableIECompatView")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Internet Explorer\BrowserEmulation", "DisableIECompatView", 1)],
        },
    ];
}

// ── merged from Debloat.cs ────────────────────────────────────────
internal static class Debloat
{
    private const string CuKey = @"HKEY_CURRENT_USER";
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string ContentDelivery = $@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";
    private const string Policies = $@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "debloat-remove-preinstalled-apps",
            Label = "Remove All Pre-installed Store Apps",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Removes common pre-installed Store apps (Clipchamp, News, Weather, Solitaire, etc.) for all users.",
            Tags = ["debloat", "apps", "store", "bloatware"],
            SideEffects = "Apps can be reinstalled from the Microsoft Store.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "$bloat = @('Clipchamp.Clipchamp','Microsoft.BingNews','Microsoft.BingWeather','Microsoft.GamingApp',"
                        + "'Microsoft.GetHelp','Microsoft.Getstarted','Microsoft.MicrosoftSolitaireCollection','Microsoft.People',"
                        + "'Microsoft.PowerAutomateDesktop','Microsoft.Todos','Microsoft.WindowsFeedbackHub','Microsoft.WindowsMaps',"
                        + "'Microsoft.ZuneMusic','Microsoft.ZuneVideo','MicrosoftTeams','Microsoft.MicrosoftOfficeHub',"
                        + "'Microsoft.549981C3F5F10','Microsoft.YourPhone','Microsoft.WindowsAlarms','Microsoft.WindowsSoundRecorder'); "
                        + "foreach ($app in $bloat) { Get-AppxPackage -AllUsers -Name $app -ErrorAction SilentlyContinue | Remove-AppxPackage -AllUsers -ErrorAction SilentlyContinue; "
                        + "Get-AppxProvisionedPackage -Online -ErrorAction SilentlyContinue | Where-Object DisplayName -eq $app | Remove-AppxProvisionedPackage -Online -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ => { }, // Cannot auto-reinstall; user must use Store
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-AppxPackage -AllUsers -Name 'Clipchamp.Clipchamp' -ErrorAction SilentlyContinue) -eq $null"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "debloat-disable-suggested-apps",
            Label = "Disable Suggested Apps in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from showing suggested (advertising) apps in the Start menu.",
            Tags = ["debloat", "advertising", "start-menu"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338388Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338389Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-314559Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338388Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338389Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-314559Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SystemPaneSuggestionsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "SoftLandingEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SystemPaneSuggestionsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-auto-app-install",
            Label = "Disable Automatic App Installation",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from silently installing promoted apps (Spotify, Disney+, etc.).",
            Tags = ["debloat", "apps", "auto-install", "advertising"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SilentInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "ContentDeliveryAllowed", 0),
                RegOp.SetDword(ContentDelivery, "OemPreInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "PreInstalledAppsEnabled", 0),
                RegOp.SetDword(ContentDelivery, "PreInstalledAppsEverEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SilentInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "ContentDeliveryAllowed"),
                RegOp.DeleteValue(ContentDelivery, "OemPreInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "PreInstalledAppsEnabled"),
                RegOp.DeleteValue(ContentDelivery, "PreInstalledAppsEverEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SilentInstalledAppsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-tips-and-tricks",
            Label = "Disable Tips, Tricks & Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips, tricks, and suggestions notifications.",
            Tags = ["debloat", "tips", "notifications", "advertising"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338393Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-353694Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-353696Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338393Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-353694Enabled"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-353696Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338393Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-lock-screen-ads",
            Label = "Disable Lock Screen Tips & Ads",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes fun facts and tips (ads) from the Windows lock screen.",
            Tags = ["debloat", "lock-screen", "advertising"],
            RegistryKeys = [ContentDelivery],
            ApplyOps =
            [
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338387Enabled", 0),
                RegOp.SetDword(ContentDelivery, "RotatingLockScreenEnabled", 0),
                RegOp.SetDword(ContentDelivery, "RotatingLockScreenOverlayEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-338387Enabled"),
                RegOp.DeleteValue(ContentDelivery, "RotatingLockScreenEnabled"),
                RegOp.DeleteValue(ContentDelivery, "RotatingLockScreenOverlayEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-338387Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-consumer-features",
            Label = "Disable Consumer Experience Features",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft consumer features that install suggested apps and games.",
            Tags = ["debloat", "consumer", "apps", "advertising"],
            RegistryKeys = [$@"{Policies}\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsConsumerFeatures", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableSoftLanding", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableCloudOptimizedContent", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsConsumerFeatures"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableSoftLanding"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableCloudOptimizedContent"),
            ],
            DetectOps = [RegOp.CheckDword($@"{Policies}\CloudContent", "DisableWindowsConsumerFeatures", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-settings-ads",
            Label = "Disable Ads in Settings App",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes promotional content and suggested actions from the Windows Settings app.",
            Tags = ["debloat", "settings", "advertising"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-338393Enabled", 0),
                RegOp.SetDword(ContentDelivery, "SubscribedContent-353698Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations"),
                RegOp.DeleteValue(ContentDelivery, "SubscribedContent-353698Enabled"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-xbox-game-bar",
            Label = "Disable Xbox Game Bar Overlay",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Xbox Game Bar overlay (Win+G). Reduces background resource usage.",
            Tags = ["debloat", "xbox", "gaming", "overlay"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\GameBar", $@"{CuKey}\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\GameBar", "AutoGameModeEnabled", 0),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0),
                RegOp.SetDword($@"{CuKey}\System\GameConfigStore", "GameDVR_Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\GameBar", "AutoGameModeEnabled"),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 1),
                RegOp.SetDword($@"{CuKey}\System\GameConfigStore", "GameDVR_Enabled", 1),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-remove-optional-features",
            Label = "Remove Optional Features (IE, Media Player, etc.)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Removes rarely-used optional features: Internet Explorer, Windows Media Player, Steps Recorder, and WordPad.",
            Tags = ["debloat", "features", "optional", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "$features = @('Browser.InternetExplorer~~~~0.0.11.0','Media.WindowsMediaPlayer~~~~0.0.12.0',"
                        + "'App.StepsRecorder~~~~0.0.1.0','Microsoft.Windows.WordPad~~~~0.0.1.0'); "
                        + "foreach ($f in $features) { "
                        + "Remove-WindowsCapability -Online -Name $f -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "$features = @('Browser.InternetExplorer~~~~0.0.11.0','Media.WindowsMediaPlayer~~~~0.0.12.0'); "
                        + "foreach ($f in $features) { "
                        + "Add-WindowsCapability -Online -Name $f -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-WindowsCapability -Online -Name 'Browser.InternetExplorer*' -ErrorAction SilentlyContinue).State -eq 'NotPresent'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "debloat-disable-windows-ink",
            Label = "Disable Windows Ink Workspace",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Ink Workspace. Frees taskbar space and background processes.",
            Tags = ["debloat", "ink", "workspace", "taskbar"],
            RegistryKeys = [$@"{Policies}\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword($@"{Policies}\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{Policies}\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword($@"{Policies}\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "debloat-unpin-all-start-tiles",
            Label = "Unpin All Start Menu Tiles (Win10)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Removes all pinned tiles from the Start menu for a clean layout (Windows 10).",
            Tags = ["debloat", "start-menu", "tiles", "clean"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "(New-Object -ComObject Shell.Application).Namespace('shell:::{4234d49b-0245-4df3-b780-3893943456e1}').Items() | "
                        + "ForEach-Object { $_.Verbs() | Where-Object Name -Match 'Un.*pin' | ForEach-Object { $_.DoIt() } }"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "debloat-disable-app-suggestions",
            Label = "Disable App Suggestions (Finish Setup)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the 'Let's finish setting up your device' nag screen and app suggestions on login.",
            Tags = ["debloat", "setup", "oobe", "nag"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "debloat-disable-cloud-content",
            Label = "Disable Cloud-Delivered Content",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Microsoft cloud content that powers Spotlight, suggested content, and feature recommendations.",
            Tags = ["debloat", "cloud", "content", "advertising"],
            RegistryKeys = [$@"{Policies}\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableThirdPartySuggestions", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsSpotlightFeatures", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsSpotlightWindowsWelcomeExperience", 1),
                RegOp.SetDword($@"{Policies}\CloudContent", "DisableWindowsSpotlightOnActionCenter", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableThirdPartySuggestions"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsSpotlightFeatures"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsSpotlightWindowsWelcomeExperience"),
                RegOp.DeleteValue($@"{Policies}\CloudContent", "DisableWindowsSpotlightOnActionCenter"),
            ],
            DetectOps = [RegOp.CheckDword($@"{Policies}\CloudContent", "DisableThirdPartySuggestions", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-start-web-search",
            Label = "Disable Web Search in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Start menu searches from querying Bing web results.",
            Tags = ["debloat", "search", "bing", "start-menu"],
            RegistryKeys = [$@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-tailored-experiences",
            Label = "Disable Tailored Experiences",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from using diagnostic data to show personalized ads and recommendations.",
            Tags = ["debloat", "advertising", "privacy", "tailored"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "debloat-disable-feedback-notifications",
            Label = "Disable Feedback Hub Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Feedback Hub from sending survey requests and notifications.",
            Tags = ["debloat", "feedback", "notifications", "nag"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Siuf\Rules"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\Siuf\Rules", "PeriodInNanoSeconds", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod"),
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Siuf\Rules", "PeriodInNanoSeconds"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-windows-hello-prompt",
            Label = "Disable Windows Hello Setup Prompt",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from nagging users to set up Windows Hello biometrics.",
            Tags = ["debloat", "hello", "biometrics", "nag", "setup"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\PassportForWork", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-people-bar",
            Label = "Disable People Bar on Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the People icon and contact integration from the taskbar.",
            Tags = ["debloat", "taskbar", "people", "contacts"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-getting-started-app",
            Label = "Disable Getting Started App",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the 'Get Started' tips app from launching after updates or on first login.",
            Tags = ["debloat", "tips", "oobe", "nag", "startup"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoStartPage", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoStartPage")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoStartPage", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-find-my-device",
            Label = "Disable Find My Device",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Find My Device feature which periodically tracks the device location.",
            Tags = ["debloat", "privacy", "tracking", "findmydevice"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\FindMyDevice"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceAutomaticReenablement", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceAutomaticReenablement")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\FindMyDevice", "FindMyDeviceAutomaticReenablement", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-inking-typing-personalization",
            Label = "Disable Inking & Typing Personalization",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from collecting inking and typing data for input personalization.",
            Tags = ["debloat", "privacy", "inking", "typing", "personalization"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\InputPersonalization"],
            ApplyOps =
            [
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                RegOp.SetDword($@"{CuKey}\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection"),
                RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection"),
            ],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-nearby-sharing",
            Label = "Disable Nearby Sharing",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Cross Device Experience (Nearby Sharing) used for Bluetooth phone-to-PC file transfer.",
            Tags = ["debloat", "nearby-sharing", "cross-device", "bluetooth", "privacy"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\CDP", "NearShareChannelUserAuthzPolicy", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-mixed-reality-portal",
            Label = "Disable Mixed Reality Portal Prompt",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Mixed Reality Portal from showing setup prompts on non-VR hardware.",
            Tags = ["debloat", "mixed-reality", "vr", "portal", "nag"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Holographic"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Holographic", "FirstRunSucceeded", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Holographic", "FirstRunSucceeded")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Holographic", "FirstRunSucceeded", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-steps-recorder",
            Label = "Disable Steps Recorder (PSR)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Problem Steps Recorder tool, which can capture screenshots of screen activity.",
            Tags = ["debloat", "privacy", "steps-recorder", "psr", "screen-capture"],
            RegistryKeys = [$@"{CuKey}\Software\Policies\Microsoft\Windows\AppCompat"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Policies\Microsoft\Windows\AppCompat", "DisableUAR")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Policies\Microsoft\Windows\AppCompat", "DisableUAR", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-error-reporting-ui",
            Label = "Suppress Error Reporting Pop-Up Dialog",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Windows Error Reporting pop-up dialog when an application crashes.",
            Tags = ["debloat", "error-reporting", "wer", "popup", "crash"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\Windows Error Reporting"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\Windows Error Reporting", "DontShowUI")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\Windows Error Reporting", "DontShowUI", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-wireless-display-projection",
            Label = "Disable Wireless Display Projection",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the ability for other devices to project content to this PC wirelessly via Miracast.",
            Tags = ["debloat", "wireless-display", "projection", "miracast", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\WirelessDisplay"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\WirelessDisplay", "AllowProjectionFromPC", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\WirelessDisplay", "AllowProjectionFromPC")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\WirelessDisplay", "AllowProjectionFromPC", 0)],
        },
        new TweakDef
        {
            Id = "debloat-disable-oobe-post-update",
            Label = "Disable Post-Update OOBE Privacy Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the privacy experience screen shown after major Windows updates via Group Policy.",
            Tags = ["debloat", "oobe", "update", "nag", "setup", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\OOBE"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\OOBE", "DisablePrivacyExperience", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\OOBE", "DisablePrivacyExperience")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\OOBE", "DisablePrivacyExperience", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-tablet-mode-auto-switch",
            Label = "Disable Auto Tablet Mode Switch",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from automatically switching to tablet mode when the keyboard is detached.",
            Tags = ["debloat", "tablet-mode", "touch", "sign-in", "usability"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode", 1)],
        },
        new TweakDef
        {
            Id = "debloat-disable-subscribed-spotlight-settings",
            Label = "Disable Spotlight Content in Settings App",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Spotlight-powered suggestion banners displayed inside the Settings application.",
            Tags = ["debloat", "spotlight", "settings", "advertising", "nag"],
            RegistryKeys = [ContentDelivery],
            ApplyOps = [RegOp.SetDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(ContentDelivery, "SubscribedContent-310093Enabled")],
            DetectOps = [RegOp.CheckDword(ContentDelivery, "SubscribedContent-310093Enabled", 0)],
        },
    ];
}


internal static class MsStore
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msstore-disable-auto-install",
            Label = "Disable Auto-Install of Suggested Apps",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic installation of suggested apps. Default: enabled. Recommended: disabled.",
            Tags = ["store", "auto-install", "suggestions", "bloat"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-tips",
            Label = "Disable Windows Tips About Store",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows tips and suggestions about the Store. Default: enabled. Recommended: disabled.",
            Tags = ["store", "tips", "suggestions", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "SoftLandingEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-consumer-features",
            Label = "Disable Consumer Features / App Suggestions",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows consumer features and app suggestions. Default: enabled. Recommended: disabled.",
            Tags = ["store", "consumer", "suggestions", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-app-suggestions-start",
            Label = "Disable App Suggestions in Start",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables app suggestions in the Start menu. Default: enabled. Recommended: disabled.",
            Tags = ["store", "start", "suggestions", "ux"],
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
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled"
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
            Id = "msstore-disable-push-install",
            Label = "Disable Remote Push-to-Install",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables remote push-to-install from Microsoft Store. Prevents apps from being silently installed via the web store. Default: enabled. Recommended: disabled.",
            Tags = ["store", "push", "install", "silent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-store-disable-auto-install-suggested",
            Label = "Disable Auto-Install of Suggested Apps",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables silent auto-install of suggested apps via ContentDeliveryManager. Prevents Microsoft from pushing unwanted app installations. Default: enabled. Recommended: disabled.",
            Tags = ["store", "auto-install", "suggested", "silent", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-store-disable-video-autoplay",
            Label = "Disable Store Video Autoplay",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables video autoplay in the Microsoft Store app. Default: enabled. Recommended: disabled.",
            Tags = ["store", "video", "autoplay", "media"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Store", "AutoPlayVideo", 0)],
        },
        new TweakDef
        {
            Id = "msstore-oem-apps-disable",
            Label = "Disable OEM Pre-Installed App Delivery",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents ContentDeliveryManager from installing OEM-bundled apps silently on new accounts or upgrades. Default: enabled. Recommended: disabled.",
            Tags = ["store", "oem", "preinstalled", "bloatware", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-feature-mgmt-disable",
            Label = "Disable Store Feature Management Experiments",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables ContentDeliveryManager feature management, preventing Microsoft from running A/B experiments that silently enable new Store and content features. Default: enabled. Recommended: disabled.",
            Tags = ["store", "feature-management", "experiments", "cdm", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "FeatureManagementEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "FeatureManagementEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-post-upgrade-apps",
            Label = "Disable Post-Upgrade App Restoration",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from reinstalling Store apps after a feature upgrade or clean install via ContentDeliveryManager. Default: enabled. Recommended: disabled.",
            Tags = ["store", "post-upgrade", "apps", "bloatware", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WindowsPostUpgradeEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WindowsPostUpgradeEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "WindowsPostUpgradeEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-start-tips",
            Label = "Disable Cortana/Bing Tips in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables SubscribedContent-280810 delivery which pushes Cortana and Bing tips into the Start menu. Default: enabled. Recommended: disabled.",
            Tags = ["store", "cortana", "bing", "tips", "start", "cdm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-280810Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-280810Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-280810Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-push-notifications",
            Label = "Disable Store Push Notifications",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables push notifications from the Microsoft Store. Default: enabled.",
            Tags = ["msstore", "notifications", "push", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisablePushNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisablePushNotifications")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisablePushNotifications", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-private-store-only",
            Label = "Restrict to Private Store Only",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts Microsoft Store to only show private store apps (enterprise). Default: all apps visible.",
            Tags = ["msstore", "private-store", "enterprise", "restrict"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePrivateStoreOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePrivateStoreOnly")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePrivateStoreOnly", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-silent-app-installs",
            Label = "Disable Silent App Installations",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Microsoft Store from silently installing suggested apps. Default: enabled.",
            Tags = ["msstore", "silent", "install", "bloatware"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-app-suggestions",
            Label = "Disable App Suggestions in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables app suggestions (ads) in the Start menu from the Microsoft Store. Default: enabled.",
            Tags = ["msstore", "suggestions", "start-menu", "ads"],
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
            Id = "msstore-store-disable-app-recommendations",
            Label = "Disable Store App Recommendations",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables app recommendation popups from the Microsoft Store. Prevents promotional content in the Store app. Default: enabled.",
            Tags = ["msstore", "recommendations", "apps", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-video-autoplay-off",
            Label = "Disable Store video autoplay",
            Category = "Windows 11",
            Tags = ["msstore", "video", "autoplay"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "VideoAutoplay", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "VideoAutoplay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "VideoAutoplay", 0)],
        },
        new TweakDef
        {
            Id = "msstore-oem-preinstall-off",
            Label = "Disable OEM-preinstalled app recommendations",
            Category = "Windows 11",
            Tags = ["msstore", "oem", "preinstall", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "OemPreInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-silent-installs-off",
            Label = "Disable silent app installations",
            Category = "Windows 11",
            Tags = ["msstore", "silent", "install", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SilentInstalledAppsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-require-purchase-auth",
            Label = "Require admin authorization for Microsoft Store purchases",
            Category = "Windows 11",
            Tags = ["msstore", "purchase", "authorization", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePurchaseAuthorization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePurchaseAuthorization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RequirePurchaseAuthorization", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-store-apps-policy",
            Label = "Disable Microsoft Store application access via GPO",
            Category = "Windows 11",
            Tags = ["msstore", "disable", "gpo", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisableStoreApps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisableStoreApps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "DisableStoreApps", 1)],
        },
        new TweakDef
        {
            Id = "msstore-disable-welcome-app",
            Label = "Disable Windows welcome experience / app suggestion notifications",
            Category = "Windows 11",
            Tags = ["msstore", "welcome", "notification", "cdm"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WelcomeAppEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WelcomeAppEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "WelcomeAppEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-subscribed-338380",
            Label = "Disable SubscribedContent-338380 (Start menu app suggestions)",
            Category = "Windows 11",
            Tags = ["msstore", "subscribed-content", "suggestions", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338380Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338380Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338380Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-subscribed-310091",
            Label = "Disable SubscribedContent-310091 (Windows welcome experience highlights)",
            Category = "Windows 11",
            Tags = ["msstore", "subscribed-content", "welcome", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310091Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310091Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310091Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-subscribed-314559",
            Label = "Disable SubscribedContent-314559 (social media / tips highlights)",
            Category = "Windows 11",
            Tags = ["msstore", "subscribed-content", "social", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-314559Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "msstore-disable-enterprise-cloud-store",
            Label = "Disable Windows Store for Business / Enterprise cloud integration",
            Category = "Windows 11",
            Tags = ["msstore", "enterprise", "business", "cloud", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "EnableWindowsStoreForBusiness", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "EnableWindowsStoreForBusiness")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "EnableWindowsStoreForBusiness", 0)],
        },
        new TweakDef
        {
            Id = "msstore-disable-adinfo",
            Label = "Disable Windows personalized advertising ID",
            Category = "Windows 11",
            Tags = ["msstore", "ads", "advertising", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0)],
        },
    ];
}

// ── merged from SnapMultitasking.cs ────────────────────────────────────────
internal static class SnapMultitasking
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "snap-disable-snap-assist",
            Label = "Disable Snap Assist",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable showing suggestions when snapping windows. Default: enabled. Recommended: personal preference.",
            Tags = ["snap", "assist", "window", "suggestion"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-groups",
            Label = "Disable Snap Groups in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable grouping Snap windows in Alt+Tab and taskbar. Default: enabled.",
            Tags = ["snap", "groups", "alt-tab", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableTaskGroups", 0)],
        },
        new TweakDef
        {
            Id = "snap-alttab-windows-only",
            Label = "Alt+Tab: Open Windows Only",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Show only open windows in Alt+Tab, not browser tabs. Default: includes Edge tabs.",
            Tags = ["alt-tab", "tabs", "edge", "windows"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MultitaskingView\AllUpView", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-vd-all-monitors",
            Label = "Show Desktops on All Monitors",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Show virtual desktop windows on all monitors in taskbar. Default: current monitor only.",
            Tags = ["virtual-desktop", "monitor", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VirtualDesktopTaskbarFilter", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-auto-arrange",
            Label = "Disable Auto-Arrange on Dock",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable automatic window rearrangement when docking/undocking. Default: enabled.",
            Tags = ["dock", "arrange", "resize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-fill",
            Label = "Disable Snap Fill Available Space",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable automatically filling available space when snapping a window. Default: enabled.",
            Tags = ["snap", "fill", "space", "resize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-dwm-anim-policy",
            Label = "Disable DWM Animations (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Machine-wide policy to disable Desktop Window Manager animations. Default: enabled.",
            Tags = ["dwm", "animation", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DWM", "DisallowAnimations", 1)],
        },
        new TweakDef
        {
            Id = "snap-disable-edge-snapping",
            Label = "Disable Window Edge Snap (Aero Snap)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Aero Snap (dragging windows to screen edges). Default: enabled.",
            Tags = ["snap", "aero", "edge", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
        },
        new TweakDef
        {
            Id = "snap-disable-alt-tab-edge-tabs",
            Label = "Disable Edge Tabs in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Microsoft Edge tabs from appearing in the Alt+Tab switcher. Shows only open windows. Default: recent 5 tabs.",
            Tags = ["snap", "alt-tab", "edge", "tabs"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-desktop-peek",
            Label = "Disable Desktop Peek",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the desktop peek feature when hovering over the Show Desktop button. Prevents accidental window hiding. Default: enabled.",
            Tags = ["snap", "desktop", "peek", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop", 1),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-vd-edge-swipe",
            Label = "Disable Virtual Desktop Edge Swipe",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the touchpad edge swipe gesture for switching virtual desktops. Prevents accidental desktop switches. Default: enabled.",
            Tags = ["snap", "virtual-desktop", "swipe", "gesture"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableEdgeSwipe", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableEdgeSwipe")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableEdgeSwipe", 0)],
        },
        new TweakDef
        {
            Id = "snap-vd-switch-anim-speed",
            Label = "Speed Up Virtual Desktop Switch Animation",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces the virtual desktop switch animation duration. Makes workspace switching feel more responsive. Default: standard speed.",
            Tags = ["snap", "virtual-desktop", "animation", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VDDesktopIconsEnabled", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VDDesktopIconsEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "VDDesktopIconsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-across-monitors",
            Label = "Disable Snap Across Monitors",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Prevents windows from snapping across monitor boundaries in multi-monitor setups. Default: enabled.",
            Tags = ["snap", "monitor", "multi-display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiMonSnap", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiMonSnap")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiMonSnap", 0)],
        },
    ];
}

// ── Merged from VirtualDesktops.cs ──────────────────────────────────────────────────

internal static class VirtualDesktops
{
    private const string VdKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VirtualDesktops";

    private const string TaskView = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string TaskViewPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

    private const string AltTabKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

    private const string DwmKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vd-show-all-windows-in-alt-tab",
            Label = "Show All VD Windows in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "task view", "windows"],
            Description =
                "Configures Alt+Tab to show windows from all virtual desktops (default "
                + "Win11 behavior). Value 1 = all desktops. Useful if a previous policy "
                + "restricted Alt+Tab to the current desktop only.",
            ApplyOps = [RegOp.SetDword(AltTabKey, "VirtualDesktopAltTabFilter", 1)],
            RemoveOps = [RegOp.DeleteValue(AltTabKey, "VirtualDesktopAltTabFilter")],
            DetectOps = [RegOp.CheckDword(AltTabKey, "VirtualDesktopAltTabFilter", 1)],
        },
        new TweakDef
        {
            Id = "vd-show-current-desktop-in-alt-tab",
            Label = "Show Only Current VD Windows in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "focus", "productivity"],
            Description =
                "Restricts Alt+Tab to only show windows from the current virtual desktop. "
                + "Value 2 = current desktop only. Reduces clutter when using many desktops.",
            ApplyOps = [RegOp.SetDword(AltTabKey, "VirtualDesktopAltTabFilter", 2)],
            RemoveOps = [RegOp.DeleteValue(AltTabKey, "VirtualDesktopAltTabFilter")],
            DetectOps = [RegOp.CheckDword(AltTabKey, "VirtualDesktopAltTabFilter", 2)],
        },
        new TweakDef
        {
            Id = "vd-show-all-taskbar-buttons",
            Label = "Show Taskbar Buttons from All Virtual Desktops",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "buttons", "all desktops"],
            Description =
                "Configures the taskbar to show app buttons from all virtual desktops "
                + "instead of only the current one. Value 1 = show all. Useful for quick "
                + "cross-desktop app switching without Task View.",
            ApplyOps = [RegOp.SetDword(TaskView, "VirtualDesktopTaskbarFilter", 1)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "VirtualDesktopTaskbarFilter")],
            DetectOps = [RegOp.CheckDword(TaskView, "VirtualDesktopTaskbarFilter", 1)],
        },
        new TweakDef
        {
            Id = "vd-show-current-taskbar-buttons",
            Label = "Show Only Current VD Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "current desktop", "focus"],
            Description =
                "Restricts the taskbar to only show buttons for apps open on the current "
                + "virtual desktop. Value 2 = current desktop only. Keeps the taskbar clean "
                + "when using many virtual desktops.",
            ApplyOps = [RegOp.SetDword(TaskView, "VirtualDesktopTaskbarFilter", 2)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "VirtualDesktopTaskbarFilter")],
            DetectOps = [RegOp.CheckDword(TaskView, "VirtualDesktopTaskbarFilter", 2)],
        },
        new TweakDef
        {
            Id = "vd-disable-task-view-button",
            Label = "Hide Task View Button from Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "task view", "taskbar", "button", "clean"],
            Description =
                "Hides the Task View (multi-desktop) button from the taskbar. "
                + "Virtual desktops remain functional via Win+Tab or Win+Ctrl+D. "
                + "Reduces taskbar clutter on single-user desktops.",
            ApplyOps = [RegOp.SetDword(TaskView, "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-task-view-system",
            Label = "Disable Task View Feature via Policy",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["virtual desktops", "task view", "policy", "disable"],
            Description =
                "Disables the Task View (Win+Tab) feature entirely via system policy. "
                + "Users cannot access virtual desktops, and the shortcut is disabled. "
                + "Intended for kiosk/corporate lockdown environments.",
            ApplyOps = [RegOp.SetDword(TaskViewPolicy, "DisableTaskView", 1)],
            RemoveOps = [RegOp.DeleteValue(TaskViewPolicy, "DisableTaskView")],
            DetectOps = [RegOp.CheckDword(TaskViewPolicy, "DisableTaskView", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-assist-flyout",
            Label = "Disable Snap Assist Desktop Flyout on Switch",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "flyout", "animation", "ux"],
            Description =
                "Disables the snap group flyout that appears when hovering over taskbar "
                + "buttons during virtual desktop operations. Reduces UI clutter for power "
                + "users who prefer compact taskbar behavior.",
            ApplyOps = [RegOp.SetDword(TaskView, "EnableSnapAssistFlyout", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "EnableSnapAssistFlyout", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "EnableSnapAssistFlyout", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-desktops-on-taskbar",
            Label = "Disable Virtual Desktop Previews on Taskbar Hover",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "preview", "hover"],
            Description =
                "Disables the virtual desktop thumbnail previews that appear when hovering "
                + "over the Task View button. Saves screen space and reduces compositor load "
                + "on hover-intensive workflows.",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskView", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "TaskView", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskView", 0)],
        },
        new TweakDef
        {
            Id = "vd-set-taskbar-multimonitor-all",
            Label = "Show All Desktop Windows on All Monitor Taskbars",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "windows"],
            Description =
                "Configures multi-monitor taskbars to show buttons for windows "
                + "from all virtual desktops (MMTaskbarMode=0). The default shows "
                + "only the current desktop on each monitor's taskbar.",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarMode", 0)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "MMTaskbarMode")],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarMode", 0)],
        },
        new TweakDef
        {
            Id = "vd-set-taskbar-multimonitor-local-only",
            Label = "Show Only Local Monitor Windows on Each Monitor Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "focus"],
            Description =
                "Configures multi-monitor taskbars to show only windows that are "
                + "open on that specific monitor (MMTaskbarMode=2). Reduces clutter "
                + "on multi-monitor setups.",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarMode", 2)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "MMTaskbarMode")],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarMode", 2)],
        },
        new TweakDef
        {
            Id = "vd-disable-aero-peek",
            Label = "Disable Aero Peek (Desktop Peek Overlay)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "aero", "peek", "dwm", "transparency"],
            Description =
                "Disables the Aero Peek feature that toggles window transparency to "
                + "show the desktop when hovering over the 'Show Desktop' corner button. "
                + "Removes the glass overlay effect (EnableAeroPeek=0).",
            ApplyOps = [RegOp.SetDword(DwmKey, "EnableAeroPeek", 0)],
            RemoveOps = [RegOp.SetDword(DwmKey, "EnableAeroPeek", 1)],
            DetectOps = [RegOp.CheckDword(DwmKey, "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-fill",
            Label = "Disable Snap Fill (Auto-Fill Adjacent Window After Snap)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "fill", "layout"],
            Description =
                "Prevents Windows from prompting you to fill the remaining screen area "
                + "after snapping a window. Only the snapped window moves; no assist popup "
                + "appears for the other half (SnapFill=0).",
            ApplyOps = [RegOp.SetDword(TaskView, "SnapFill", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "SnapFill", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "SnapFill", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-snap-revert",
            Label = "Disable Snap Revert (Don't Move Partner Window When Moving Dragged Window)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "snap", "revert", "layout"],
            Description =
                "When dragging a snapped window away from its position, disables the "
                + "automatic reverting of the paired window to its pre-snap size and position "
                + "(SnapRevert=0). Gives more predictable behavior.",
            ApplyOps = [RegOp.SetDword(TaskView, "SnapRevert", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "SnapRevert", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "SnapRevert", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-taskbar-grouping",
            Label = "Never Group Taskbar Buttons",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "grouping", "buttons"],
            Description =
                "Prevents the taskbar from grouping multiple windows of the same app "
                + "into a single button. Each open window gets its own dedicated button "
                + "regardless of how many are open (TaskbarGlomLevel=2).",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskbarGlomLevel", 2)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "TaskbarGlomLevel")],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskbarGlomLevel", 2)],
        },
        new TweakDef
        {
            Id = "vd-enable-background-per-desktop",
            Label = "Enable Unique Wallpaper Per Virtual Desktop",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "wallpaper", "background", "personalization"],
            Description =
                "Allows each virtual desktop to have its own wallpaper that changes "
                + "automatically when you switch desktops. Enables the per-desktop background "
                + "feature (BackgroundChangesOnDesktopSwitch=1).",
            ApplyOps = [RegOp.SetDword(VdKey, "BackgroundChangesOnDesktopSwitch", 1)],
            RemoveOps = [RegOp.SetDword(VdKey, "BackgroundChangesOnDesktopSwitch", 0)],
            DetectOps = [RegOp.CheckDword(VdKey, "BackgroundChangesOnDesktopSwitch", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-alt-tab-thumbnails",
            Label = "Delay Alt+Tab Thumbnail Preview (Effectively Disable Hover Thumbnails)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "alt-tab", "thumbnails", "hover", "performance"],
            Description =
                "Sets the taskbar thumbnail hover delay to 30 seconds, effectively "
                + "preventing thumbnail previews from appearing while still keeping the "
                + "feature technically enabled (ExtendedUIHoverTime=30000 ms).",
            ApplyOps = [RegOp.SetDword(TaskView, "ExtendedUIHoverTime", 30000)],
            RemoveOps = [RegOp.DeleteValue(TaskView, "ExtendedUIHoverTime")],
            DetectOps = [RegOp.CheckDword(TaskView, "ExtendedUIHoverTime", 30000)],
        },
        new TweakDef
        {
            Id = "vd-show-taskbar-on-secondary-monitors",
            Label = "Show Taskbar on All Monitors (Multi-Monitor)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "multimonitor", "secondary"],
            Description =
                "Enables the extended taskbar on secondary monitors so each display "
                + "shows its own taskbar. Useful when the multi-monitor taskbar was "
                + "previously disabled (MMTaskbarEnabled=1).",
            ApplyOps = [RegOp.SetDword(TaskView, "MMTaskbarEnabled", 1)],
            RemoveOps = [RegOp.SetDword(TaskView, "MMTaskbarEnabled", 0)],
            DetectOps = [RegOp.CheckDword(TaskView, "MMTaskbarEnabled", 1)],
        },
        new TweakDef
        {
            Id = "vd-disable-taskbar-end-task-button",
            Label = "Disable End Task Button on Taskbar (Windows 11 23H2+)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["virtual desktops", "taskbar", "end-task", "windows 11"],
            Description =
                "Removes the 'End Task' button that appears in the right-click context "
                + "menu for taskbar buttons in Windows 11 version 23H2 and later. "
                + "Prevents accidental process termination (TaskbarEndTask=0).",
            ApplyOps = [RegOp.SetDword(TaskView, "TaskbarEndTask", 0)],
            RemoveOps = [RegOp.SetDword(TaskView, "TaskbarEndTask", 1)],
            DetectOps = [RegOp.CheckDword(TaskView, "TaskbarEndTask", 0)],
        },
    ];
}

// ── merged from Notifications.cs ──
internal static class Notifications
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "notif-disable-toast",
            Label = "Disable Toast Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pop-up toast notifications from all applications. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "toast", "popup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications",
                    "NoToastApplicationNotification",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications",
                    "NoToastApplicationNotification"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications",
                    "NoToastApplicationNotification",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-lock-screen",
            Label = "Disable Lock Screen Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents notifications from appearing on the lock screen. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "lock-screen", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-sounds",
            Label = "Disable Notification Sounds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Silences all notification sounds system-wide. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "sounds", "audio"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-suggestions",
            Label = "Disable Windows Suggestions / Tips",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Windows from showing tips, tricks, and suggestion notifications. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "suggestions", "tips"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-welcome",
            Label = "Disable Windows Welcome Experience",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows welcome experience shown after updates. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "welcome", "updates"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-app-suggestions",
            Label = "Disable Suggested Apps in Start",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from suggesting apps in the Start menu. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "suggestions", "start-menu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-quiet-hours-auto",
            Label = "Auto-enable Quiet Hours",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables quiet hours (focus assist) to suppress all toast notifications. Default: disabled. Recommended: enabled.",
            Tags = ["notifications", "quiet-hours", "focus-assist"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-startup-app-notif",
            Label = "Disable Background App Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables 'apps are running in the background' system toast notifications. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "background", "startup"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.BackgroundAccess",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-quiet-hours",
            Label = "Disable Push Toast Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all push toast notifications globally. Default: Enabled. Recommended: Disabled for focus.",
            Tags = ["notifications", "toast", "push", "quiet-hours"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_TOASTS_ENABLED",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-display-time-3s",
            Label = "Set Notification Display Time to 3 Seconds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets notification display duration to 3 seconds instead of the default 5. Reduces visual distraction. Default: 5s. Recommended: 3s.",
            Tags = ["notifications", "display-time", "duration", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 5)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", 3)],
        },
        new TweakDef
        {
            Id = "notif-disable-security-center",
            Label = "Disable Windows Security Center Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Disables toast notifications from the Windows Security and Maintenance center. Reduces interruptions from security alerts. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "security", "maintenance", "center"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-autoconnect",
            Label = "Disable Auto Connect Network Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables notifications from the AutoConnect (hotspot) system toast. Reduces Wi-Fi connection prompt interruptions. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "network", "autoconnect", "wifi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.AutoConnect",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-account-notif",
            Label = "Disable Microsoft Account Connected Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables notification toasts from Microsoft account connected services. Stops account sync prompts and MSA-linked notifications. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "account", "microsoft", "msa"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
                    "Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\MicrosoftAccount.Notifications.Connected",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-lock-screen-toasts",
            Label = "Disable Notifications on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents app toast notifications from displaying on the lock screen. Protects notification content from shoulder-surfers. Default: Enabled. Recommended: Disabled.",
            Tags = ["notifications", "lock-screen", "privacy", "toast"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_TOASTS_ABOVE_LOCK",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-notification-sounds",
            Label = "Disable Notification Sounds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all Windows notification sounds. Toasts still appear silently. Default: enabled.",
            Tags = ["notifications", "sounds", "mute", "silent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-reduce-toast-duration",
            Label = "Reduce Toast Notification Duration",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the time toast notifications are displayed from 5 seconds to 3 seconds. Default: 5000ms.",
            Tags = ["notifications", "toast", "duration", "timeout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", "5")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility", "MessageDuration", "3")],
        },
        new TweakDef
        {
            Id = "notif-silence-global-sounds",
            Label = "Silence All Notification Sounds",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Silences all Windows notification sounds globally. Visual notifications still appear but without audio. Default: sounds enabled.",
            Tags = ["notifications", "sounds", "silence", "mute"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-defender-user-notif",
            Label = "Suppress Windows Defender Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Suppresses Windows Defender security notifications that appear during scans and threat detections.",
            Tags = ["notifications", "defender", "antivirus", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows Defender\UX Configuration", "Notification_Suppress", 1)],
        },
        new TweakDef
        {
            Id = "notif-disable-reboot-required",
            Label = "Disable Windows Update Reboot Required Notification",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows Update from nagging users to reboot when an update is pending installation.",
            Tags = ["notifications", "windows-update", "reboot", "nagging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationDisable", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationDisable"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationDisable", 1),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-smartscreen-user",
            Label = "Disable SmartScreen Evaluation Notifications (User)",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Disables per-user SmartScreen web content evaluation, suppressing SmartScreen block and warning notifications.",
            Tags = ["notifications", "smartscreen", "browser", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost", "EnableWebContentEvaluation", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost", "EnableWebContentEvaluation", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AppHost", "EnableWebContentEvaluation", 0)],
        },
        new TweakDef
        {
            Id = "notif-disable-taskbar-suggestions",
            Label = "Disable Taskbar & Start Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Microsoft-promoted content appearing in the taskbar system pane. Removes commercial suggestions from the system tray area.",
            Tags = ["notifications", "taskbar", "suggestions", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "SystemPaneSuggestionsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "SystemPaneSuggestionsEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "SystemPaneSuggestionsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-oem-preinstall-suggestions",
            Label = "Disable OEM Preinstalled App Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from suggesting OEM preinstalled applications and cloud app links via notification banners.",
            Tags = ["notifications", "oem", "preinstall", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "OemPreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "OemPreInstalledAppsEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDelivery", "OemPreInstalledAppsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-no-logged-users-reboot",
            Label = "Allow Reboot with Logged-On Users (Windows Update)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows Update from rebooting while a user is logged on. Eliminates surprise forced-reboot notifications.",
            Tags = ["notifications", "windows-update", "reboot", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "NoAutoRebootWithLoggedOnUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "NoAutoRebootWithLoggedOnUsers")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
        },
    ];
}

// === Merged from: Widgets.cs ===

internal static class Widgets
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "widgets-news-disable-news-interests",
            Label = "Disable News and Interests (Win10)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the News and Interests taskbar widget in Win10. Removes the weather/news flyout from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "interests", "taskbar", "win10"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-welcome-experience",
            Label = "Disable Welcome Experience",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows Welcome Experience page that opens after updates to show new features. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "welcome", "update", "experience"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-get-more",
            Label = "Disable 'Get Even More Out of Windows'",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the 'Get Even More Out of Windows' popup and similar Microsoft 365 / OneDrive nag prompts. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "suggestions", "nag", "onedrive", "m365"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353694Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353696Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353694Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353696Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-start-suggestions",
            Label = "Disable Suggested Apps in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables suggested (promoted) apps in the Start menu. Stops Microsoft Store app recommendations. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "start", "suggestions", "apps", "ads"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SystemPaneSuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338388Enabled"
                ),
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
            Id = "widgets-news-disable-settings-suggestions",
            Label = "Disable Suggested Content in Settings",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested content and feature highlights in the Windows Settings app. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "settings", "suggestions", "content"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353698Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338393Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353698Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-353698Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-finish-setup",
            Label = "Disable 'Finish Setting Up' Reminder",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the recurring 'Let's finish setting up your device' nag screen after updates. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "finish-setup", "nag", "reminder"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-310093Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows from showing suggestions for third-party apps in search and feeds. Reduces promotional content. Default: Enabled. Recommended: Disabled.",
            Tags = ["widgets", "suggestions", "third-party", "ads", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "ThirdPartySuggestionsEnabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "ThirdPartySuggestionsEnabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "ThirdPartySuggestionsEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight",
            Label = "Disable Windows Spotlight on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Spotlight images and tips on the lock screen. Default: enabled.",
            Tags = ["widgets", "spotlight", "lock-screen", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnLockScreen", 1),
            ],
        },
        new TweakDef
        {
            Id = "widgets-news-disable-subscribed-content",
            Label = "Disable Subscribed Content Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Microsoft subscribed content suggestions shown in Settings and lock screen. Removes premium service promotions. Default: enabled.",
            Tags = ["widgets", "subscribed", "content", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                    "SubscribedContent-338389Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-lock-app-notifications",
            Label = "Disable App Notifications on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableLockScreenAppNotifications=1 in CloudContent policy. Prevents app toast notifications from appearing on the lock screen.",
            Tags = ["widgets", "lock-screen", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableLockScreenAppNotifications", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableLockScreenAppNotifications"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableLockScreenAppNotifications", 1),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight-in-settings-gpo",
            Label = "Disable Windows Spotlight in Settings App (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightInSettings=1. Prevents the Settings app from showing Spotlight-sourced feature suggestions and background images.",
            Tags = ["widgets", "spotlight", "settings", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSettings", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSettings"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSettings", 1),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight-welcome-gpo",
            Label = "Disable Windows Spotlight Welcome Experience (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightWindowsWelcomeExperience=1. Disables the full-screen Spotlight highlight shown after major Windows updates.",
            Tags = ["widgets", "spotlight", "welcome", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableWindowsSpotlightWindowsWelcomeExperience",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableWindowsSpotlightWindowsWelcomeExperience"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableWindowsSpotlightWindowsWelcomeExperience",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-tailored-diag-experiences",
            Label = "Disable Tailored Experiences from Diagnostic Data (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableTailoredExperiencesWithDiagnosticData=1. Prevents Windows from using collected diagnostic data to show personalised tips, advertising, and feature recommendations.",
            Tags = ["widgets", "tailored", "diagnostic", "privacy", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent",
                    "DisableTailoredExperiencesWithDiagnosticData",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "widgets-disable-taskbar-meet-now",
            Label = "Hide Teams Meet Now Button from Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets TaskbarMn=0 in Explorer settings. Removes the Teams \"Meet Now\" button from the Windows 10/11 system tray area.",
            Tags = ["widgets", "taskbar", "teams", "meet-now", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "TaskbarMn", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "TaskbarMn")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "TaskbarMn", 0)],
        },
        new TweakDef
        {
            Id = "widgets-disable-spotlight-in-search-gpo",
            Label = "Disable Windows Spotlight in Search Interface (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightInSearch=1. Removes Spotlight-sourced background images and suggestions from the Windows Search home panel.",
            Tags = ["widgets", "spotlight", "search", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSearch", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSearch"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightInSearch", 1),
            ],
        },
    ];
}

