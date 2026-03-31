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
            Id = "w11-disable-chat-icon",
            Label = "Disable Chat/Teams Taskbar Icon",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the Chat/Teams icon from the Windows 11 taskbar. Default: Shown. Recommended: Hidden.",
            Tags = ["win11", "chat", "teams", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 0)],
        },

        new TweakDef
        {
            Id = "w11-disable-cross-device",
            Label = "Disable Cross-Device Resume",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Cross-Device Resume (CDP) which syncs activities across devices linked to the same Microsoft account. Win11 24H2+. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["win11", "cross-device", "cdp", "privacy", "24h2"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            MinBuild = 26100,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableCdp", 0)],
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
            Id = "w11-disable-suggested-actions-policy",
            Label = "Disable Suggested Actions (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows 11 Suggested Actions popup via Group Policy. Machine-wide enforcement for managed environments. Default: enabled. Recommended: disabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SmartActionPlatform\SmartClipboard", "Disabled", 1),
            ],
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
            Id = "w11-disable-recent-start",
            Label = "Disable Recent Items in Start Menu",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables tracking and display of recently opened documents in the Start menu. Improves privacy. Default: Enabled. Recommended: Disabled.",
            Tags = ["win11", "start-menu", "recent", "privacy", "tracking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
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
            Id = "w11-disable-bing-search",
            Label = "Disable Bing in Start Menu Search",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents web/Bing results from appearing in the Start menu search box.",
            Tags = ["win11", "bing", "search", "start-menu", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
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
            Id = "w11-disable-wu-autorestart",
            Label = "Prevent Auto-Restart for Windows Update",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Windows from automatically rebooting when a user is signed in after installing updates.",
            Tags = ["win11", "windows-update", "restart", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
            ],
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
            Id = "w11-clock-seconds",
            Label = "Show Seconds in System Clock",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets ShowSecondsInSystemClock=1 to display seconds in the taskbar system clock. Useful for monitoring and timing tasks. Default: off.",
            Tags = ["win11", "clock", "taskbar", "seconds"],
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
            Id = "w11-lock-screen-disable",
            Label = "Disable Lock Screen via Policy",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets NoLockScreen=1 in the Personalization policy key to bypass the lock screen and go directly to the sign-in box. Default: 0 (lock screen shown).",
            Tags = ["win11", "lock-screen", "policy", "login"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", 1)],
        },


        new TweakDef
        {
            Id = "w11-taskbar-wiggle-off",
            Label = "Disable Taskbar Badge Wiggle Animation",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets TaskbarAcrylicOpacity=0 and disables the badge-count wiggle animation on pinned app buttons. Reduces visual noise in the taskbar. Default: enabled.",
            Tags = ["win11", "taskbar", "animation", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAnimations", 0)],
        },
        new TweakDef
        {
            Id = "w11-power-button-action",
            Label = "Set Power Menu Shutdown as Default Action",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Sets CsEnabled=0 in the power policy to change the default power button behavior. Helps ensure a clean shutdown on systems with connected standby. Default: depends on OEM.",
            Tags = ["win11", "power", "shutdown", "button"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "CsEnabled", 0)],
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
            Id = "w11-disable-explorer-ads",
            Label = "Disable File Explorer Sync Provider Ads",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables promotional notifications from sync providers (e.g., OneDrive) shown inside File Explorer. Removes subscription and cloud storage upsell banners. Default: enabled.",
            Tags = ["w11", "explorer", "ads", "onedrive", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSyncProviderNotifications", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSyncProviderNotifications", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "ShowSyncProviderNotifications",
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
            Id = "w11-disable-lock-screen-camera",
            Label = "Disable Camera Access on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the lock screen camera by setting NoLockScreenCamera=1 via Group Policy. Prevents the camera from being accessible while the device is locked. Default: camera accessible on lock screen.",
            Tags = ["w11", "lock-screen", "camera", "privacy", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreenCamera", 1)],
        },
        new TweakDef
        {
            Id = "w11-disable-lock-screen-app-notif",
            Label = "Disable App Notifications on Lock Screen",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables app notification previews shown on the lock screen (DisableLockScreenAppNotifications=1 via Group Policy). Prevents sensitive notification content from being visible while the device is locked. Default: notifications shown.",
            Tags = ["w11", "lock-screen", "notifications", "privacy", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "DisableLockScreenAppNotifications", 1)],
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
        new TweakDef
        {
            Id = "w11-set-wallpaper-quality-100",
            Label = "Disable Wallpaper JPEG Compression",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets JPEG import quality for desktop wallpapers to 100 (no compression). Prevents Windows from silently re-compressing high-quality wallpaper images when they are applied. Default: 85 (lossy compression).",
            Tags = ["w11", "wallpaper", "quality", "jpeg", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "cortana-disable-cortana-completely",
            Label = "Disable Cortana Entirely",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Completely disables Cortana via Group Policy.",
            Tags = ["cortana", "privacy", "assistant"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortanaAboveLock", 0)],
        },

        new TweakDef
        {
            Id = "cortana-disable-search-highlights-dynamic",
            Label = "Disable Dynamic Search Highlights",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables dynamic search highlights and tips in the search box.",
            Tags = ["search", "cortana", "highlights", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-search-aadmsa",
            Label = "Disable AAD/MSA Cloud Search",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "cortana-disable-search-location",
            Label = "Disable Windows Search Location Access",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Search from using device location.",
            Tags = ["search", "cortana", "location", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "AllowSearchToUseLocation", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-highlights",
            Label = "Disable Search Highlights",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables search highlights (trending searches, news) in the Windows Search box. Reduces distractions and network traffic. Default: Enabled. Recommended: Disabled.",
            Tags = ["cortana", "search", "highlights", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-search",
            Label = "Disable Cloud Search",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables cloud content in Windows Search results. Only shows local files and settings. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "cloud", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
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
            Id = "cortana-disable-web-search",
            Label = "Disable Search Box Web Suggestions",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables web suggestions and results in the Windows Search box. Only shows local results. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "web", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
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
            Id = "cortana-disable-bing-search",
            Label = "Disable Bing Search in Start Menu",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Bing search results integration in the Start menu and taskbar search. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "bing", "search", "start-menu", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-cloud-personalization",
            Label = "Disable Cloud Content Personalization",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables cloud-based content personalization in Windows Search via HKLM policy (AllowCloudSearch=0). Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "cloud", "personalization", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-metered",
            Label = "Disable Search Over Metered Connections",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "cortana-disable-device-search-history",
            Label = "Disable Device Search History",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables local device search history storage. Prevents Windows from saving search queries on the device. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["cortana", "search", "history", "device", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-battery",
            Label = "Disable Search Indexing on Battery",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "cortana-disable-bing-search-in-start",
            Label = "Disable Bing Search in Start Menu",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Bing web search results from appearing in Start menu searches. Only local results shown. Default: enabled.",
            Tags = ["cortana", "bing", "search", "start-menu"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableSearchBoxSuggestions", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-indexing-outlook",
            Label = "Disable Search Indexing of Outlook",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
        new TweakDef
        {
            Id = "cortana-disable-cloud-content-search",
            Label = "Disable Cloud Content Search",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables searching cloud content (OneDrive, Outlook, SharePoint) from Windows Search. Local files only. Default: cloud content included.",
            Tags = ["cortana", "cloud", "search", "content"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsAADCloudSearchEnabled", 0),
            ],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "cortana-disable-search-in-store",
            Label = "Disable Search the Microsoft Store",
            Category = "Cortana & Search",
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
            Id = "cortana-disable-search-history-local",
            Label = "Disable Local Search History",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables local search history in the Windows Search box. Prevents recent searches from appearing as suggestions.",
            Tags = ["cortana", "search", "history", "local"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "cortana-disable-cortana-consent",
            Label = "Disable Cortana Consent Required",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Cortana consent popup that appears on first use. Prevents Cortana from requesting permissions.",
            Tags = ["cortana", "consent", "popup", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "cortana-show-search-icon-only",
            Label = "Show Search Icon Only (Hide Search Box)",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows only the search icon on the taskbar instead of the full search box. Saves taskbar space while keeping search accessible.",
            Tags = ["cortana", "search", "icon", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 1)],
        },
        new TweakDef
        {
            Id = "cortana-disable-windows-copilot",
            Label = "Disable Windows Copilot in Search",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "cortana-limit-search-indexer-throttle",
            Label = "Throttle Search Indexer CPU Usage",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the Search Indexer to reduced performance mode. Caps CPU usage during indexing, trading speed for lower system impact.",
            Tags = ["cortana", "indexer", "throttle", "cpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
        },
        new TweakDef
        {
            Id = "cortana-disable-search-preview-pane",
            Label = "Disable Search Preview Pane",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "cortana-disable-dynamic-wsb-content",
            Label = "Disable Dynamic Content in Windows Search Bar",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableDynamicContentInWSB=0 via Windows Search policy. Prevents the search bar from displaying rotating news, trending searches, or other dynamic web content.",
            Tags = ["cortana", "search", "dynamic-content", "news", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "EnableDynamicContentInWSB", 0)],
        },
        new TweakDef
        {
            Id = "cortana-gpo-block-bing-answers",
            Label = "Disable Bing Answers in Windows Search (Policy)",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "idx-disable-web-search",
            Label = "Disable Web Search in Start",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable web search results in Start menu. Policy setting. Default: enabled. Recommended: disabled.",
            Tags = ["web", "search", "start", "bing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", 1)],
        },
        new TweakDef
        {
            Id = "idx-disable-connected-search",
            Label = "Disable Connected Search (Bing)",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable Bing online results in Windows Search. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["bing", "connected", "online", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
        },
        new TweakDef
        {
            Id = "idx-disable-search-highlights",
            Label = "Disable Search Highlights",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable trending/interest highlights in search. Default: enabled. Recommended: disabled.",
            Tags = ["highlights", "interests", "trending", "bing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "BingSearchEnabled", 0)],
        },


        new TweakDef
        {
            Id = "idx-disable-indexer-backoff",
            Label = "Disable Indexer Low-Disk Backoff",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Id = "idx-disable-cloud-search",
            Label = "Disable Cloud Content in Search",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable cloud content (OneDrive, M365) from appearing in search. Default: enabled.",
            Tags = ["cloud", "search", "onedrive", "m365"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCloudSearch", 0)],
        },
        new TweakDef
        {
            Id = "idx-disable-search-location",
            Label = "Disable Location for Search",
            Category = "Cortana & Search",
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
            Id = "idx-disable-dynamic-searchbox",
            Label = "Disable Dynamic Search Box Content",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables dynamic content in the search box (IsDynamicSearchBoxEnabled=0). Removes trending searches and images from the search experience. Default: enabled. Recommended: disabled.",
            Tags = ["search", "dynamic", "searchbox", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDynamicSearchBoxEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "idx-disable-recent-search",
            Label = "Disable Recent Search Suggestions",
            Category = "Cortana & Search",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables recent search history suggestions in Windows Search. Prevents previously searched terms from appearing as suggestions. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["search", "recent", "history", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SearchSettings", "IsDeviceSearchHistoryEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "idx-reduce-indexer-io",
            Label = "Reduce Indexer Disk I/O",
            Category = "Cortana & Search",
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
            Id = "idx-disable-outlook-indexing",
            Label = "Disable Outlook Indexing",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows Search from indexing Outlook email data via policy. Reduces indexer CPU and disk usage on large mailboxes. Default: indexed. Recommended: Disabled if Outlook search unused.",
            Tags = ["search", "outlook", "email", "indexing", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
        },
        new TweakDef
        {
            Id = "idx-prevent-indexing-battery",
            Label = "Prevent Indexing on Battery Power",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the Windows Search indexer from running when on battery power. Significantly improves laptop battery life. Default: indexing continues. Recommended: Apply on laptops.",
            Tags = ["search", "indexer", "battery", "power", "laptop"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
        },
        new TweakDef
        {
            Id = "idx-limit-indexer-threads",
            Label = "Limit Indexer CPU Threads",
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
            Category = "Cortana & Search",
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
        new TweakDef
        {
            Id = "idx-disable-search-indexing-backoff",
            Label = "Disable Search Indexing Backoff",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the search indexer from reducing indexing speed when the system is busy. Indexes faster at the cost of more CPU. Default: enabled.",
            Tags = ["search", "indexing", "backoff", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "SetupCompletedSuccessfully", 0)],
        },
        new TweakDef
        {
            Id = "idx-disable-cortana-in-search",
            Label = "Disable Cortana in Windows Search",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Cortana integration in Windows Search. No web suggestions or Bing queries. Default: enabled.",
            Tags = ["search", "cortana", "bing", "web"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "idx-limit-indexer-locations",
            Label = "Disable Indexing of Outlook Data",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Search from indexing Outlook data stores. Reduces indexer CPU and disk usage. Default: indexed.",
            Tags = ["search", "indexing", "outlook", "email"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexingOutlook", 1)],
        },
        new TweakDef
        {
            Id = "idx-disable-search-web-results",
            Label = "Disable Web Results in Search",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables web results in Windows Search. Only local files and apps appear. Default: enabled.",
            Tags = ["search", "web", "bing", "local-only"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "ConnectedSearchUseWeb", 0)],
        },
        new TweakDef
        {
            Id = "idx-disable-indexing-on-battery",
            Label = "Disable Indexing on Battery Power",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the search indexer from running on battery power. Saves battery life on laptops. Default: reduced indexing.",
            Tags = ["search", "indexing", "battery", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "PreventIndexOnBattery", 1)],
        },
        new TweakDef
        {
            Id = "idx-disable-cloud-accounts",
            Label = "Disable Cloud Account Search Indexing",
            Category = "Cortana & Search",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows Search from indexing cloud-based accounts (Microsoft, work/school). Limits search to local content only. Default: indexed.",
            Tags = ["indexing", "cloud", "accounts", "search"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search", "DisableWebSearch", 1)],
        },
    ];
}
