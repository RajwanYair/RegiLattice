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
            Id = "cortana-disable-cortana-completely",
            Label = "Disable Cortana Entirely",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Id = "cortana-disable-search-location",
            Label = "Disable Windows Search Location Access",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Id = "cortana-disable-device-search-history",
            Label = "Disable Device Search History",
            Category = "Windows 11",
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
            Id = "cortana-disable-bing-search-in-start",
            Label = "Disable Bing Search in Start Menu",
            Category = "Windows 11",
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
        new TweakDef
        {
            Id = "cortana-disable-cloud-content-search",
            Label = "Disable Cloud Content Search",
            Category = "Windows 11",
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
            Id = "cortana-disable-search-history-local",
            Label = "Disable Local Search History",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Id = "cortana-limit-search-indexer-throttle",
            Label = "Throttle Search Indexer CPU Usage",
            Category = "Windows 11",
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
            Id = "cortana-disable-dynamic-wsb-content",
            Label = "Disable Dynamic Content in Windows Search Bar",
            Category = "Windows 11",
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
            Id = "idx-disable-web-search",
            Label = "Disable Web Search in Start",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Id = "idx-disable-cloud-search",
            Label = "Disable Cloud Content in Search",
            Category = "Windows 11",
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
            Id = "idx-disable-dynamic-searchbox",
            Label = "Disable Dynamic Search Box Content",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Id = "idx-disable-outlook-indexing",
            Label = "Disable Outlook Indexing",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
        new TweakDef
        {
            Id = "idx-disable-search-indexing-backoff",
            Label = "Disable Search Indexing Backoff",
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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
            Category = "Windows 11",
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

// ── merged from MsStore.cs ────────────────────────────────────────
internal static class MsStore
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msstore-disable-store",
            Label = "Disable Microsoft Store",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Microsoft Store via group policy. Default: enabled. Recommended: disabled.",
            Tags = ["store", "microsoft", "policy", "bloat"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "RemoveWindowsStore", 1)],
        },
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
            Id = "msstore-disable-auto-update",
            Label = "Disable Store App Auto-Updates",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic updates of Store apps. Default: enabled. Recommended: disabled.",
            Tags = ["store", "updates", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
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
            Id = "msstore-disable-preinstalled",
            Label = "Disable Preinstalled Apps",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables preinstalled apps from being installed on new accounts. Default: enabled. Recommended: disabled.",
            Tags = ["store", "preinstalled", "bloat", "cleanup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
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
            Id = "msstore-disable-feedback",
            Label = "Disable Feedback Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables feedback notification prompts. Default: enabled. Recommended: disabled.",
            Tags = ["store", "feedback", "notifications", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
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
            Id = "msstore-disable-content-delivery",
            Label = "Disable Content Delivery",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables content delivery entirely. Default: enabled. Recommended: disabled.",
            Tags = ["store", "content", "delivery", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
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
            Id = "msstore-disable-consumer-experiences",
            Label = "Disable Windows Consumer Experiences (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows consumer experiences via Group Policy. Prevents bloatware, suggested apps, and consumer account content. Default: enabled. Recommended: disabled.",
            Tags = ["store", "consumer", "bloatware", "policy", "experiences"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableConsumerAccountStateContent", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableConsumerAccountStateContent"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableConsumerAccountStateContent", 1),
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
            Id = "msstore-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks third-party app suggestions from appearing in Windows via the CloudContent group policy key. Prevents promoted apps in Start, Settings, and lock screen. Default: allowed. Recommended: blocked.",
            Tags = ["store", "third-party", "suggestions", "cloud-content", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
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
            Id = "msstore-preinstalled-apps-off",
            Label = "Disable pre-installed app reinstallation",
            Category = "Windows 11",
            Tags = ["msstore", "preinstall", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "PreInstalledAppsEnabled", 0),
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
            Id = "msstore-consumer-features-off",
            Label = "Disable Windows consumer features (Store suggestions)",
            Category = "Windows 11",
            Tags = ["msstore", "consumer", "bloat", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsConsumerFeatures", 1),
            ],
        },
        new TweakDef
        {
            Id = "msstore-soft-landing-off",
            Label = "Disable Store soft-landing tips on first run",
            Category = "Windows 11",
            Tags = ["msstore", "onboarding", "tips"],
            NeedsAdmin = false,
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
            Id = "msstore-content-delivery-off",
            Label = "Disable content delivery manager entirely",
            Category = "Windows 11",
            Tags = ["msstore", "content", "delivery", "privacy", "bloat"],
            NeedsAdmin = false,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
        },
        new TweakDef
        {
            Id = "msstore-subscription-content-off",
            Label = "Disable Store subscription content highlights",
            Category = "Windows 11",
            Tags = ["msstore", "subscription", "privacy"],
            NeedsAdmin = false,
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
            Id = "msstore-store-autodownload-off",
            Label = "Disable Microsoft Store automatic app downloads via GPO",
            Category = "Windows 11",
            Tags = ["msstore", "autodownload", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
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
            Id = "msstore-disable-contentdelivery-allowed",
            Label = "Disable all Windows content delivery (master CDM switch)",
            Category = "Windows 11",
            Tags = ["msstore", "content-delivery", "cdm", "privacy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "ContentDeliveryAllowed", 0),
            ],
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
            Id = "snap-disable-snap-layouts",
            Label = "Disable Snap Layouts Flyout",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable the hover-over maximize button Snap Layouts flyout (Win11). Default: enabled.",
            Tags = ["snap", "layouts", "flyout", "maximize", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
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
            Id = "snap-disable-aero-shake",
            Label = "Disable Aero Shake",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable shaking a window title bar to minimise all others. Default: enabled.",
            Tags = ["aero", "shake", "minimize"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
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
            Id = "snap-disable-snap-suggestions",
            Label = "Disable Snap Window Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable AI/suggested windows when snapping. Default: enabled.",
            Tags = ["snap", "suggestion", "ai"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0),
            ],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0)],
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
            Id = "snap-disable-edge-swipe-nav",
            Label = "Disable Edge Swipe Navigation",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables edge swipe navigation gestures on touchscreens. Default: Enabled. Recommended: Disabled on desktops.",
            Tags = ["snap", "edge", "swipe", "gesture", "touch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "AllowEdgeSwipe", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-flyout",
            Label = "Disable Snap Fly-Out Overlay",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the snap bar fly-out overlay when dragging windows. Reduces visual clutter during window arrangement. Default: Enabled. Recommended: Disabled.",
            Tags = ["snap", "flyout", "overlay", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapBar", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-resize-snap",
            Label = "Disable Window Resize Snap Assist",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables joint resize when dragging the border between two snapped windows. Prevents accidental resizing of adjacent snapped windows. Default: Enabled. Recommended: Disabled.",
            Tags = ["snap", "resize", "joint", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "JointResize", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-fly-out",
            Label = "Disable Snap Layouts Fly-Out",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the snap layouts fly-out shown when hovering over maximize button. Default: enabled.",
            Tags = ["snap", "layouts", "fly-out", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssistFlyout", 0),
            ],
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
            Id = "snap-set-virtual-desktop-show-all-taskbar",
            Label = "Show All Virtual Desktop Windows in Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows windows from all virtual desktops in the taskbar, instead of only the current desktop. Default: current desktop only.",
            Tags = ["snap", "virtual-desktop", "taskbar", "windows"],
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
            Id = "snap-disable-edge-snap",
            Label = "Disable Edge Snapping",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables automatic window snapping when dragging to screen edges. Default: enabled.",
            Tags = ["snap", "edge", "window", "drag"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WindowArrangementActive", "0")],
        },
        new TweakDef
        {
            Id = "snap-disable-shake-minimize",
            Label = "Disable Aero Shake to Minimize",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables Aero Shake — shaking a window no longer minimizes all other windows. Default: enabled.",
            Tags = ["snap", "shake", "minimize", "aero"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "snap-disable-corner-snap",
            Label = "Disable Corner Snap",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables window snapping to corners (quarter-screen layout). Default: enabled.",
            Tags = ["snap", "corner", "quarter", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DITest", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-auto-snap-fill",
            Label = "Disable Snap Fill",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables automatic filling of available space when snapping a window. Default: enabled.",
            Tags = ["snap", "fill", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapFill", 0)],
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
        new TweakDef
        {
            Id = "snap-disable-task-view-button",
            Label = "Disable Task View Button",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Removes the Task View button from the taskbar. Task View is still accessible via Win+Tab. Default: shown.",
            Tags = ["taskview", "taskbar", "button", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-timeline",
            Label = "Disable Activity History / Timeline",
            Category = "Windows 11",
            NeedsAdmin = true,
            Description = "Disables Windows Timeline and activity history collection. Default: enabled.",
            Tags = ["timeline", "activity", "history", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "snap-disable-recent-apps-multitasking",
            Label = "Disable Recent Apps in Multitasking View",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Hides recent applications from the Alt+Tab and Task View multitasking interfaces. Default: shown.",
            Tags = ["snap", "recent", "alt-tab", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackDocs", 0)],
        },
        new TweakDef
        {
            Id = "snap-hide-edge-tabs-alt-tab",
            Label = "Hide Edge Tabs in Alt+Tab",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description =
                "Prevents Microsoft Edge browser tabs from appearing in the Alt+Tab window switcher. Shows only windows. Default: 5 recent tabs.",
            Tags = ["alt-tab", "edge", "tabs", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "MultiTaskingAltTabFilter", 3),
            ],
        },
        new TweakDef
        {
            Id = "snap-disable-snap-suggest",
            Label = "Disable Snap Layout Suggestions",
            Category = "Windows 11",
            NeedsAdmin = false,
            Description = "Disables the automatic suggestion overlay when snapping a window. Default: enabled.",
            Tags = ["snap", "suggest", "layout", "overlay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
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
            Id = "vd-disable-timeline",
            Label = "Disable Windows Timeline",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "timeline", "activity history", "privacy"],
            Description =
                "Disables the Windows Timeline feature in Task View that tracks your "
                + "recent activity and documents. Prevents activity history collection "
                + "and removes the timeline from Task View.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableActivityFeed", 0)],
        },
        new TweakDef
        {
            Id = "vd-disable-timeline-upload",
            Label = "Disable Timeline Activity Sync to Cloud",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["virtual desktops", "timeline", "cloud sync", "privacy", "microsoft account"],
            Description =
                "Prevents Timeline activity history from being uploaded to Microsoft cloud "
                + "servers (requires a Microsoft account). Activity remains local only even "
                + "if Timeline is still enabled.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0)],
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
            Id = "notif-disable-action-center",
            Label = "Disable Action Center",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows Action Center sidebar. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "action-center", "sidebar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer", "DisableNotificationCenter", 1)],
        },
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
            Id = "notif-disable-finish-setup",
            Label = "Disable Finish Setting Up Reminders",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the recurring 'finish setting up your device' reminders. Default: enabled. Recommended: disabled.",
            Tags = ["notifications", "setup", "reminders"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement",
                    "ScoobeSystemSettingEnabled",
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
            Id = "notif-disable-suggested-notifications",
            Label = "Disable Suggested Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows suggesting you finish setting up your device via notifications. Default: enabled.",
            Tags = ["notifications", "suggested", "setup", "tips"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "ScoobeSystemSettingEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UserProfileEngagement",
                    "ScoobeSystemSettingEnabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "notif-disable-suggested-actions",
            Label = "Disable Suggested Actions Notifications",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested actions that appear after copying phone numbers or dates. Default: enabled.",
            Tags = ["notifications", "suggested-actions", "clipboard", "popup"],
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
            Id = "notif-disable-banners",
            Label = "Disable Notification Banners",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables notification banner popups on the desktop. Notifications go silently to Action Center only. Default: banners shown.",
            Tags = ["notifications", "banners", "toast", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0)],
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
            Id = "notif-disable-low-disk-alert",
            Label = "Disable Low Disk Space Alert",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows popup warnings about low disk space on drives. Default: Enabled.",
            Tags = ["notifications", "disk", "low-disk", "alert"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLowDiskSpaceChecks", 1),
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
            Id = "notif-disable-balloon-tips",
            Label = "Disable System Tray Balloon Tips",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables legacy balloon tips from system tray icons. Default: enabled. Modern toast notifications are not affected.",
            Tags = ["notifications", "balloon", "tray", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableBalloonTips", 0)],
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
            Id = "notif-disable-clear-recent-on-exit",
            Label = "Clear Recent Items List on Exit",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Automatically clears Recent Documents (MRU) lists each time the user logs off. Prevents leaving access trail.",
            Tags = ["notifications", "recent-docs", "privacy", "mru"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "ClearRecentDocsOnExit", 1),
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
            Id = "widgets-news-disable-widgets-panel",
            Label = "Disable Widgets Panel (Win11)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows 11 Widgets panel (Weather/News/Sports). Removes the Widgets button from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "taskbar", "win11"],
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
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0)],
        },
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
            Id = "widgets-news-disable-feeds-taskbar",
            Label = "Disable News and Interests on Taskbar",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables news and interests feed on the Windows taskbar via policy. Removes the weather/news widget from the taskbar. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "feeds", "taskbar"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-widget-disable-news-feed",
            Label = "Disable News Feed Content",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the news feed content delivery in Widgets panel via policy. Stops news articles from loading in the background. Default: enabled. Recommended: disabled.",
            Tags = ["widgets", "news", "feed", "content"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
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
            Id = "widgets-widget-remove-weather-taskbar",
            Label = "Remove Weather from Taskbar",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the weather widget from the taskbar by setting view mode to hidden. Default: shown. Recommended: hidden.",
            Tags = ["widgets", "weather", "taskbar", "hide"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Feeds", "ShellFeedsTaskbarViewMode", 2)],
        },
        new TweakDef
        {
            Id = "widgets-disable-machine-feeds",
            Label = "Disable Windows Feeds via Machine Policy",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Feeds (News and Interests) via machine-level policy, applying to all users on the system. Default: Enabled. Recommended: Disabled.",
            Tags = ["widgets", "feeds", "news", "policy", "machine"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
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
            Id = "widgets-disable-widget-board",
            Label = "Disable Widgets Board Completely",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Fully disables the Windows 11 Widgets board via Group Policy. Default: enabled.",
            Tags = ["widgets", "board", "disable", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Dsh", "AllowNewsAndInterests", 0)],
        },
        new TweakDef
        {
            Id = "widgets-disable-open-on-hover",
            Label = "Disable Widgets Open on Hover",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Widgets panel from opening when hovering over the taskbar icon. Default: open on hover.",
            Tags = ["widgets", "hover", "taskbar", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 0)],
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
            Id = "widgets-disable-tips-tricks-suggestions",
            Label = "Disable Tips, Tricks, and Suggestions",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Tips, Tricks, and Suggestions notifications from Windows. Default: enabled.",
            Tags = ["widgets", "tips", "suggestions", "notifications"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
        },
        new TweakDef
        {
            Id = "widgets-disable-start-personalization",
            Label = "Disable Start Menu Personalization",
            Category = "Windows 11",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables personalized content recommendations in the Start menu. Removes suggested apps and content cards. Default: enabled.",
            Tags = ["widgets", "start", "personalization", "recommendations"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_IrisRecommendations", 0),
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
            Id = "widgets-disable-feeds-policy",
            Label = "Disable Windows Feeds / News Bar (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFeeds=0 in the Windows Feeds policy key. Disables the news and interests feed bar at the group-policy level, covering devices where HKCU-level tweaks are overridden.",
            Tags = ["widgets", "news", "feeds", "gpo", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds", "EnableFeeds", 0)],
        },
        new TweakDef
        {
            Id = "widgets-disable-cloud-optimized-content",
            Label = "Disable Cloud-Optimised Content in Widgets",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableCloudOptimizedContent=1 in CloudContent policy. Stops Windows from fetching and displaying cloud-optimised widget content including personalised tiles.",
            Tags = ["widgets", "cloud", "content", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableCloudOptimizedContent", 1)],
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
            Id = "widgets-disable-spotlight-action-center-gpo",
            Label = "Disable Windows Spotlight in Action Center (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableWindowsSpotlightOnActionCenter=1. Stops Spotlight suggestions from being injected into the Windows Action Center notification area.",
            Tags = ["widgets", "spotlight", "action-center", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnActionCenter", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnActionCenter"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableWindowsSpotlightOnActionCenter", 1),
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
            Id = "widgets-gpo-disable-third-party-spotlight",
            Label = "Disable Third-Party Suggestions in Spotlight (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableThirdPartySuggestions=1 in CloudContent policy. Prevents third-party apps and advertisers from appearing in Windows Spotlight lock-screen and Start suggestions.",
            Tags = ["widgets", "spotlight", "third-party", "ads", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableThirdPartySuggestions", 1)],
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
        new TweakDef
        {
            Id = "widgets-disable-soft-landing-tips",
            Label = "Disable Windows Soft-Landing Feature Tips (Policy)",
            Category = "Windows 11",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSoftLanding=1 in CloudContent policy. Prevents the initial \"tip overlay\" that appears over new features after Windows is installed or updated.",
            Tags = ["widgets", "tips", "soft-landing", "debloat", "gpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent", "DisableSoftLanding", 1)],
        },
    ];
}

// ── merged from PolicyUpdate.cs ──
// RegiLattice.Core — Tweaks/PolicyUpdate.cs
// Windows Update configuration, AU restart, update notifications, driver updates, scan frequency, and update pause policies
// Category: "Windows Update Policy"
// Consolidated from 9 modules.

internal static class PolicyUpdate
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CbsUpdatePolicy.Data,
            .. _UpdateAutoRestartPolicy.Data,
            .. _WindowsPauseUpdatesPolicy.Data,
            .. _WindowsUpdateAdvanced.Data,
            .. _WindowsUpdateDriverPolicy.Data,
            .. _WindowsUpdateNotificationPolicy.Data,
            .. _WindowsUpdatePolicy.Data,
            .. _WindowsUpdateScanPolicy.Data,
            .. _WindowsUpdateUsoPolicy.Data,
            // ── merged from: WindowsUpdate.cs ───────────────────────────────────────
            new TweakDef
            {
                Id = "wu-defer-quality-updates",
                Label = "Defer Quality Updates (30 days)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Defers quality (security/bug-fix) updates by 30 days.",
                Tags = ["update", "deferral"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 30),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1)],
            },
            new TweakDef
            {
                Id = "wu-defer-feature-updates",
                Label = "Defer Feature Updates (90 days)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Defers feature (major version) updates by 90 days.",
                Tags = ["update", "deferral"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1)],
            },
            new TweakDef
            {
                Id = "wu-no-auto-restart",
                Label = "Disable Forced Auto-Restart",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Prevents Windows from automatically restarting while a user is logged in after update installation.",
                Tags = ["update", "restart"],
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
                Id = "wu-update-notify-only",
                Label = "Notify-Only Updates (No Auto-Install)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                Description = "Sets Windows Update to notify before downloading, giving you full control over update timing.",
                Tags = ["update", "control"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 2),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate", 0),
                ],
                RemoveOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 3),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 2)],
            },
            new TweakDef
            {
                Id = "wu-set-active-hours-au",
                Label = "Set Active Hours (8 AM - 11 PM)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Sets Windows Update active hours to 8 AM - 11 PM to prevent restart during work.",
                Tags = ["update", "active-hours", "restart"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours", 1),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursStart", 8),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursEnd", 23),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursStart"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursEnd"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-msrt",
                Label = "Disable MSRT Delivery",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Prevents the Malicious Software Removal Tool from being offered via Windows Update.",
                Tags = ["update", "msrt", "security"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUAU", 1)],
            },
            new TweakDef
            {
                Id = "wu-target-release-version",
                Label = "Pin to Windows 11 24H2",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Pins the device to Windows 11 24H2 to prevent unwanted feature updates.",
                Tags = ["update", "feature", "pin", "24H2"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersion", 1),
                    RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersionInfo", "24H2"),
                    RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ProductVersion", "Windows 11"),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersion"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersionInfo"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ProductVersion"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "TargetReleaseVersion", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-do-upload",
                Label = "Disable Delivery Optimization Upload",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables Delivery Optimization peer-to-peer upload. Prevents your PC from serving update files to other PCs. Sets upload bandwidth to zero. Default: Unlimited. Recommended: Disabled.",
                Tags = ["update", "delivery-optimization", "bandwidth", "performance"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            },
            new TweakDef
            {
                Id = "wu-disable-driver-updates",
                Label = "Disable Driver Updates via Windows Update",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Excludes driver updates from Windows Update quality updates. Default: Included. Recommended: Excluded for driver stability.",
                Tags = ["update", "driver", "exclude", "stability"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
                ],
            },
            new TweakDef
            {
                Id = "wu-defer-quality-updates-14d",
                Label = "Defer Quality Updates by 14 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Defers quality/security updates by 14 days to allow time for issue reports. Default: 0. Recommended: 14 for stability.",
                Tags = ["update", "defer", "quality", "delay"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 30),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdates", 1)],
            },
            new TweakDef
            {
                Id = "wu-block-driver-search",
                Label = "Block Driver Search via Windows Update",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows from searching for driver updates through Windows Update. Different from WU driver exclusion policy. Default: enabled. Recommended: disabled for stability.",
                Tags = ["update", "driver", "search", "block"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 0)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 1)],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 0),
                ],
            },
            new TweakDef
            {
                Id = "wu-defer-feature-365d",
                Label = "Defer Feature Updates by 365 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Defers Windows feature updates by 365 days. Provides maximum time for stability reports before upgrading. Default: 0. Recommended: 365 for production stability.",
                Tags = ["update", "defer", "feature", "delay", "365"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdates", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-os-upgrade",
                Label = "Disable Windows OS Upgrade via Update",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from offering or installing OS version upgrades. Blocks W10 to W11 upgrades being pushed silently. Default: Enabled. Recommended: Disabled for production stability.",
                Tags = ["update", "upgrade", "os", "block"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-safeguard-hold",
                Label = "Disable Windows Update Safeguard Holds",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables Microsoft's safeguard holds that block updates on incompatible hardware. Use only if you understand the update risks for your system. Default: Enabled. Recommended: Enabled (disable only if blocked).",
                Tags = ["update", "safeguard", "hold", "compatibility"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-optional-updates",
                Label = "Disable Auto-Install of Optional Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from automatically installing optional/minor updates. Gives you manual control over optional update installations. Default: Enabled. Recommended: Disabled.",
                Tags = ["update", "optional", "minor", "auto-install"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates", 0),
                ],
            },
            new TweakDef
            {
                Id = "wu-set-active-hours-8-20",
                Label = "Set Windows Update Active Hours (8 AM – 8 PM)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Sets Windows Update active hours to 8 AM – 8 PM. No restart prompts during this window. Default: auto.",
                Tags = ["update", "active-hours", "restart", "schedule"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart", 8),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursEnd", 20),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursEnd"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart", 8)],
            },
            new TweakDef
            {
                Id = "wu-defer-quality-updates-7days",
                Label = "Defer Quality Updates by 7 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Defers quality (security/bug fix) updates by 7 days. Gives time for known issues to surface. Default: 0 days.",
                Tags = ["update", "defer", "quality", "days"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 7),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferQualityUpdatesPeriodInDays", 7),
                ],
            },
            new TweakDef
            {
                Id = "wu-defer-feature-updates-90days",
                Label = "Defer Feature Updates by 90 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Defers feature updates (major releases) by 90 days. Ensures stability before adopting new builds. Default: 0 days.",
                Tags = ["update", "defer", "feature", "days"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DeferFeatureUpdatesPeriodInDays", 90),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-seeker-updates",
                Label = "Disable Optional Update Seeker",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Prevents Windows from seeking optional quality updates. Only mandatory updates are installed. Default: seeks all.",
                Tags = ["update", "optional", "seeker", "quality"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-auto-restart",
                Label = "Disable Auto-Restart After Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows from automatically restarting after installing updates. User must manually initiate the restart. Default: auto-restart.",
                Tags = ["update", "restart", "automatic", "disable"],
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
                Id = "wu-disable-delivery-optimization",
                Label = "Disable Delivery Optimisation P2P",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables peer-to-peer delivery optimisation. Updates only download from Microsoft servers, not other PCs. Default: LAN + Internet.",
                Tags = ["update", "delivery-optimization", "p2p", "bandwidth"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization", "DODownloadMode", 0)],
            },
            new TweakDef
            {
                Id = "wu-disable-update-notifications",
                Label = "Disable Update Notifications",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Suppresses Windows Update restart notifications and nagging prompts. Updates still install but silently. Default: notifications shown.",
                Tags = ["update", "notifications", "nag", "quiet"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel", 2)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel", 2),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-update-orchestrator",
                Label = "Disable Update Orchestrator Service",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Disables the Update Orchestrator Service (UsoSvc). Prevents Windows from automatically checking for and installing updates. Default: automatic.",
                Tags = ["update", "orchestrator", "service", "disable"],
                SideEffects = "Windows will not automatically check for security updates.",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 4)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 2)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 4)],
            },
            new TweakDef
            {
                Id = "wu-disable-ux-access",
                Label = "Disable Windows Update UX Access",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Hides the Windows Update page in Settings. Prevents non-admin users from triggering manual update checks. Default: accessible.",
                Tags = ["update", "ux", "settings", "hide"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-wus-medic",
                Label = "Disable Windows Update Medic Service",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Disables the Windows Update Medic Service (WaaSMedicSvc) that repairs Windows Update components. Prevents forced re-enablement. Default: automatic.",
                Tags = ["update", "medic", "service", "disable"],
                SideEffects = "Windows Update cannot self-repair if components become corrupted.",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 4)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 3)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 4)],
            },
            new TweakDef
            {
                Id = "wu-exclude-drivers-quality",
                Label = "Exclude Drivers from Quality Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Excludes driver updates from quality update installations. Prevents Windows Update from overwriting manually installed drivers. Default: included.",
                Tags = ["update", "drivers", "exclude", "quality"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ExcludeWUDriversInQualityUpdate", 1),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-automatic-updates",
                Label = "Disable Automatic Update Downloads",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Prevents Windows Update from automatically downloading updates. Updates will still be detected but must be manually approved and installed. Default: auto-download enabled.",
                Tags = ["windows-update", "automatic", "download", "control"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "wu-set-schedule-day-saturday",
                Label = "Schedule Updates for Saturday Installation",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Configures Windows Update to install scheduled updates on Saturday at 3:00 AM, minimising disruption during working hours.",
                Tags = ["windows-update", "schedule", "maintenance", "saturday"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay", 7),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallTime", 3),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 4),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallTime"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay", 7)],
            },
            new TweakDef
            {
                Id = "wu-disable-store-app-auto-updates",
                Label = "Disable Microsoft Store App Auto-Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Prevents the Microsoft Store from automatically updating installed applications in the background. You retain control over when app updates are applied.",
                Tags = ["windows-update", "store", "apps", "auto-update"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsStore", "AutoDownload", 2)],
            },
            new TweakDef
            {
                Id = "wu-set-update-service-manual",
                Label = "Set Windows Update Service to Manual Start",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Changes the Windows Update service (wuauserv) to manual start so it only runs when you initiate a check, preventing background update scans from consuming resources.",
                Tags = ["windows-update", "service", "manual", "background"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 3)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 2)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 3)],
            },
            new TweakDef
            {
                Id = "wu-require-admin-for-updates",
                Label = "Require Admin Approval for Update Installation",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from installing updates without explicit administrator approval. Useful on shared systems to maintain control over when patches are applied.",
                Tags = ["windows-update", "admin", "approval", "control"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins", 0)],
            },
            new TweakDef
            {
                Id = "wu-disable-metered-update-download",
                Label = "Block Updates on Metered Connections",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from downloading updates when the network connection is marked as metered (mobile hotspot, limited data plans), saving mobile data costs.",
                Tags = ["windows-update", "metered", "mobile", "data", "network"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings", "DownloadMode", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings", "DownloadMode"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings",
                        "DownloadMode",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-reboot-required-notification",
                Label = "Disable Post-Update Reboot Notifications",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Suppresses the nagging 'Restart Required' toast notifications that appear after Windows Update installs patches. Reboots can still be performed manually.",
                Tags = ["windows-update", "reboot", "notification", "toast"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig", 1),
                ],
            },
            new TweakDef
            {
                Id = "wu-set-feature-update-channel-general",
                Label = "Set Windows Update Channel to General Availability",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Pins the Windows Update servicing channel to General Availability / Semi-Annual Channel, avoiding early feature updates that may be less stable.",
                Tags = ["windows-update", "channel", "feature", "stable", "semi-annual"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "BranchReadinessLevel", 16)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "BranchReadinessLevel")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "BranchReadinessLevel", 16)],
            },
            new TweakDef
            {
                Id = "wu-set-orchestrator-service-manual",
                Label = "Set Update Orchestrator Service to Manual",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Changes the Update Orchestrator Service (UsoSvc) to manual start, preventing it from waking the system for updates outside your active hours.",
                Tags = ["windows-update", "orchestrator", "schedule", "wake", "service"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 3)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 2)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 3)],
            },
            new TweakDef
            {
                Id = "wu-disable-third-party-preview",
                Label = "Disable Third-Party Windows Update Preview Consent",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Blocks the consent dialog that prompts users to participate in Windows Update previews from third-party software publishers.",
                Tags = ["windows-update", "preview", "third-party", "consent"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess", 0),
                ],
            },
        ];

    // ── CbsUpdatePolicy ──
    private static class _CbsUpdatePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CBS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cbsupd-enable-auto-repair",
                Label = "Enable Automatic Component-Based Servicing Repair",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Component-Based Servicing (CBS) is the Windows component store infrastructure that manages OS component installation, updates, and repairs through the DISM subsystem. Enabling automatic CBS repair ensures that corrupted or missing system components are automatically detected and repaired from the Windows component store without manual intervention. CBS corruption can prevent Windows Update from installing updates and security patches creating security vulnerabilities from missed patching cycles. Automatic repair through CBS uses the component manifest store to verify component integrity and restore damaged components to their correct state. Organizations should enable automatic CBS repair to ensure that system component corruption does not cause persistent patching failures or security gaps. CBS repair events are logged in the CBS.log file which should be reviewed during system health checks to identify recurring repair needs.",
                Tags = ["cbs", "component-repair", "system-integrity", "update", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutomaticRepair", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutomaticRepair")],
                DetectOps = [RegOp.CheckDword(Key, "AutomaticRepair", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enforce-component-hash-verification",
                Label = "Enforce Cryptographic Hash Verification for CBS Components",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS component hash verification validates the cryptographic hash of each system component against the component manifest preventing installation of tampered or corrupted components. Enforcing hash verification for CBS operations ensures that only genuine Microsoft-signed components are installed as part of servicing operations. Component hash bypass attacks attempt to install modified system files by manipulating the CBS manifest or hash database to accept attacker-controlled components. CBS hash verification provides a layer of protection against supply chain attacks that attempt to replace legitimate system files with backdoored versions. Organizations should ensure that CBS integrity checking is enabled and that the component store hash database has not been modified through monitoring. CBS hash verification failures generate events in the CBS.log that should be treated as high-severity alerts indicating potential system tampering.",
                Tags = ["cbs", "hash-verification", "integrity", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceHashVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceHashVerification")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceHashVerification", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-restrict-cbs-offline-servicing",
                Label = "Restrict CBS Offline Servicing to Authorized Administrators",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "CBS offline servicing allows modification of Windows component store contents from offline boot environments which is a powerful capability that can be used to bypass OS-level security controls. Restricting CBS offline servicing to authorized administrators prevents unauthorized use of offline tools to modify system components outside the normal OS boot environment. BitLocker full-disk encryption is the primary defense against offline servicing attacks as it prevents booting from external media to access the encrypted drive. Organizations running Secure Boot with TPM-based integrity measurement provide additional protection against offline servicing attacks by detecting changes to the boot environment. CBS offline servicing is a legitimate maintenance capability used for repair scenarios but should be restricted through physical security and encryption rather than software policy alone. Organizations should include CBS offline servicing in their threat model and ensure physical security controls prevent unauthorized access to servers.",
                Tags = ["cbs", "offline-servicing", "admin-restriction", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictOfflineServicing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictOfflineServicing")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictOfflineServicing", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-cbs-cleanup-scheduled",
                Label = "Enable Scheduled Cleanup of Superseded CBS Components",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Windows component store retains superseded component versions after updates to support rollback capability but accumulates significant disk space over time. Enabling scheduled CBS cleanup removes superseded components after a defined retention period freeing disk space while retaining recent versions for rollback. System drives running close to capacity due to component store accumulation can cause update failures when insufficient space exists for patch installation. The DISM cleanup task removes components that can no longer be uninstalled based on the uninstall window policy reducing disk usage by 10-20% on long-running systems. Organizations should balance component store cleanup with rollback requirements as aggressive cleanup prevents rolling back recent updates if problems are discovered. WSFC clusters during CBS cleanup and fail-safe mechanisms prevent critical system failures due to premature component removal.",
                Tags = ["cbs", "cleanup", "disk-space", "maintenance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ScheduledCleanup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScheduledCleanup")],
                DetectOps = [RegOp.CheckDword(Key, "ScheduledCleanup", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enforce-manifest-signing",
                Label = "Enforce Digital Signature on CBS Component Manifests",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS component manifests describe the contents and attributes of system components and are signed by Microsoft to ensure their integrity and prevent modification. Enforcing manifest signature verification ensures that modified manifests that attempt to introduce malicious components or disable security features are rejected. Component manifest signing is part of the Windows Trusted Installer infrastructure that protects system file integrity against unauthorized modification. Manifests that have been tampered with to override hash values or add unauthorized components will be rejected when signature enforcement is active. Manifest signature enforcement is a defense-in-depth measure complementing Windows Resource Protection (WRP) and other component store integrity mechanisms. Organizations should treat CBS manifest signature verification failures as critical security events indicating potential kernel-level or bootkit-level compromise.",
                Tags = ["cbs", "manifest-signing", "code-signing", "integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceManifestSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceManifestSigning")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceManifestSigning", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-cbs-verbose-logging",
                Label = "Enable Verbose CBS Logging for Update Failure Diagnostics",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "CBS verbose logging captures detailed information about servicing operations including component installations, updates, and failures in the CBS.log file for troubleshooting. Enabling verbose CBS logging provides the detailed diagnostic data needed to identify root causes of Windows Update failures that may indicate security patching gaps. CBS.log is typically several hundred megabytes to gigabytes in size with verbose logging and should be captured and analyzed as part of update compliance monitoring. Update failures identified through CBS verbose logging should be cross-referenced with security vulnerability databases to prioritize remediation of security-relevant failures. Organizations with update compliance requirements should monitor CBS logs for persistent failures that indicate systems are not receiving security patches. Verbose CBS logging helps distinguish between installation failures caused by disk space, compatibility, corruption, or other factors to inform targeted remediation.",
                Tags = ["cbs", "verbose-logging", "diagnostics", "update-compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "VerboseLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "VerboseLogging")],
                DetectOps = [RegOp.CheckDword(Key, "VerboseLogging", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-set-cbs-store-health-check-interval",
                Label = "Set Scheduled Interval for CBS Component Store Health Verification",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "CBS component store health checks verify the integrity of the Windows component store by comparing installed component hashes against the reference values in the component manifest. Setting regular health check intervals ensures that component store corruption is detected promptly before it leads to update failures or security vulnerabilities. Component store corruption can occur due to disk errors, unexpected shutdowns, or malware modification of system files. Regular health verification similar to running DISM /CheckHealth provides ongoing assurance that the system components match their expected values. Health check interval policies complement automatic repair by detecting corruption early before it causes operational problems. Organizations should define health check intervals based on their risk posture with more frequent checks for high-security systems and critical infrastructure.",
                Tags = ["cbs", "health-check", "component-integrity", "maintenance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HealthCheckInterval", 7)],
                RemoveOps = [RegOp.DeleteValue(Key, "HealthCheckInterval")],
                DetectOps = [RegOp.CheckDword(Key, "HealthCheckInterval", 7)],
            },
            new TweakDef
            {
                Id = "cbsupd-block-unsigned-packages",
                Label = "Block Installation of Unsigned or Untrusted CBS Packages",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS package signature verification ensures that only packages signed by trusted certificate authorities including Microsoft and hardware vendors can be installed through the CBS servicing infrastructure. Blocking unsigned CBS packages prevents installation of tampered or third-party packages that could introduce vulnerabilities or backdoors into the system component store. Unsigned packages submitted to CBS represent a significant threat vector for supply chain attacks where unauthorized components are installed as system components. CBS package signature enforcement should apply to both online and offline servicing operations to prevent bypass through offline tools. Organizations running Windows Server should audit the custom packages installed through CBS to identify any unsigned or questionable packages in the component store. CBS signature enforcement is complementary to Windows code signing policies and should be aligned with the organization's overall application trust model.",
                Tags = ["cbs", "unsigned-packages", "code-signing", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUnsignedPackages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnsignedPackages")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnsignedPackages", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-restrict-cbs-to-trusted-sources",
                Label = "Restrict CBS Package Sources to Microsoft Update and WSUS Only",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "CBS package source restriction limits where the CBS servicing infrastructure can obtain component packages to Microsoft Update or organizational WSUS servers. Restricting CBS to trusted sources prevents the use of arbitrary package sources that could deliver malicious components masked as system updates. Third-party package sources for CBS are rarely needed in enterprise environments where updates are managed through WSUS or Configuration Manager. Source restriction for CBS complements Windows Update source restrictions to create a consistent update trust chain from Microsoft to the endpoint. Organizations should configure both Windows Update and CBS source policies together to ensure coherent update supply chain protection. Audit CB package installation events to detect any packages sourced from unexpected origins that may indicate a source restriction bypass.",
                Tags = ["cbs", "trusted-sources", "update-chain", "wsus", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToTrustedSources", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToTrustedSources")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToTrustedSources", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-servicing-stack-updates-priority",
                Label = "Enable Priority Installation of Servicing Stack Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Servicing Stack Updates (SSUs) update the foundational CBS infrastructure itself and must be installed before cumulative updates that depend on the updated servicing stack. Enabling priority installation of SSUs ensures that the servicing stack is always current before applying other updates preventing installation failures from an outdated stack. Outdated servicing stacks are a common cause of Windows Update failure where cumulative updates cannot be installed because they require SSU capabilities not yet present. SSU prioritization is implemented in Windows 10 1903 and later through the Unified Update Platform that automatically handles SSU installation order. Organizations running older Windows versions should prioritize SSU installation in their WSUS or Configuration Manager patch deployment groups. Servicing stack currency is a prerequisite for comprehensive security patching and should be verified during update compliance audits.",
                Tags = ["cbs", "servicing-stack", "update-priority", "patching", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PrioritizeServicingStackUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PrioritizeServicingStackUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "PrioritizeServicingStackUpdates", 1)],
            },
        ];
    }

    // ── UpdateAutoRestartPolicy ──
    private static class _UpdateAutoRestartPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-deadline-7days",
                    Label = "WU Auto-Restart: Set Engaged Restart Deadline to 7 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets EngagedRestartDeadline=7 in WU policy. After a quality update is downloaded, Windows enters 'engaged restart' mode where users are repeatedly notified. "
                        + "This value sets the absolute deadline after which Windows will force a restart regardless of user activity. "
                        + "7 days is a balance that gives users a full work week to schedule the restart while ensuring machines don't stay un-patched indefinitely.",
                    Tags = ["windows-update", "restart", "deadline", "policy", "engaged"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Forces restart after 7 days; ensures machines are patched while giving users a workweek to choose their own restart time.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartDeadline", 7)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartDeadline")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartDeadline", 7)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-snooze-3days",
                    Label = "WU Auto-Restart: Set Engaged Restart Snooze Interval to 3 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets EngagedRestartSnoozeSchedule=3 in WU policy. Controls how frequently Windows re-displays the engaged restart notification after a user dismisses it. "
                        + "Value of 3 means the reminder returns every 3 days, ensuring users don't forget a pending restart while avoiding daily interruptions that lead to notification fatigue and dismissal without action.",
                    Tags = ["windows-update", "restart", "snooze", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "3-day snooze interval for restart reminders; balances user awareness with notification fatigue.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartSnoozeSchedule", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartSnoozeSchedule")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartSnoozeSchedule", 3)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-transition-2days",
                    Label = "WU Auto-Restart: Set Engaged Restart Transition Schedule to 2 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets EngagedRestartTransitionSchedule=2 in WU policy. Controls how many days after an update becomes ready-to-install that Windows transitions from passive notifications to the more prominent 'engaged restart' mode. "
                        + "Setting this to 2 days means the first two days show soft notifications, after which the full engaged restart UI (with deadline counter) takes over.",
                    Tags = ["windows-update", "restart", "transition", "policy", "engaged"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Transitions to engaged restart mode after 2 days; earlier transition increases restart compliance rate.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartTransitionSchedule", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartTransitionSchedule")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartTransitionSchedule", 2)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-quality-update-deadline-3days",
                    Label = "WU Auto-Restart: Set Quality Update Install Deadline to 3 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineForQualityUpdates=3 in WU policy. Establishes a hard deadline of 3 days from when a quality (security + non-security) update is offered before Windows must restart to install it. "
                        + "For security teams managing patch compliance under CIS or NIST 800-53 patch SLAs, a 3-day restart deadline for quality updates ensures critical CVE patches are active within the compliance window.",
                    Tags = ["windows-update", "deadline", "quality", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "3-day hard restart deadline for quality updates; supports NIST 800-53 and CIS patch compliance SLAs.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForQualityUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-feature-update-deadline-14days",
                    Label = "WU Auto-Restart: Set Feature Update Install Deadline to 14 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineForFeatureUpdates=14 in WU policy. Establishes a 14-day hard deadline from when a feature update is offered before Windows must restart to complete installation. "
                        + "Feature updates are far more disruptive than quality updates (longer restart time, possible app compatibility breaks), so a longer 14-day window gives users and IT departments time to validate and prepare.",
                    Tags = ["windows-update", "deadline", "feature", "upgrade", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "14-day deadline for feature updates; longer window accommodates compatibility validation before forced restart.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForFeatureUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-deadline-grace-period-2days",
                    Label = "WU Auto-Restart: Set Post-Deadline Grace Period to 2 Days",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineGracePeriod=2 in WU policy. After the restart deadline passes, this grace period gives users an additional 2 days before the machine will restart outside of active hours. "
                        + "The grace period prevents the deadline enforcement from causing a disruptive forced restart mid-workday as soon as the deadline hits. The machine will restart during the next scheduled non-active hours window within the grace period.",
                    Tags = ["windows-update", "deadline", "grace", "restart", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "2-day grace period post-deadline; restart deferred to next active-hours window reducing in-day disruption.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineGracePeriod", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineGracePeriod")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineGracePeriod", 2)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-disable-no-auto-reboot-after-deadline",
                    Label = "WU Auto-Restart: Allow Auto-Reboot After Deadline Expires",
                    Category = "Windows Update",
                    Description =
                        "Sets ConfigureDeadlineNoAutoReboot=0 in WU policy. Ensures that once the deadline and grace period pass, Windows WILL automatically restart to apply the update. "
                        + "Value=0 means no moratorium on auto-reboot after the deadline. This overrides any 'NoAutoRebootWithLoggedOnUsers' policy for machines that have exceeded their deadline, ensuring patching is never blocked indefinitely by a persistent logged-on session.",
                    Tags = ["windows-update", "restart", "deadline", "enforcement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Post-deadline auto-reboot enabled; overrides logged-on user protection once deadline expires for compliance.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineNoAutoReboot")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-restart-warning-4hours",
                    Label = "WU Auto-Restart: Set Pre-Restart Warning to 4 Hours",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduleRestartWarning=4 in WU policy. When Windows schedules an automatic restart, this setting controls how many hours in advance users receive a prominent restart warning notification. "
                        + "A 4-hour advance warning gives users time to save work, close applications, and plan the restart, significantly reducing data loss from unexpected restarts.",
                    Tags = ["windows-update", "restart", "warning", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "4-hour advance restart warning; gives users time to save work and plan restart timing.",
                    ApplyOps = [RegOp.SetDword(Key, "ScheduleRestartWarning", 4)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScheduleRestartWarning")],
                    DetectOps = [RegOp.CheckDword(Key, "ScheduleRestartWarning", 4)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-enable-auto-restart-required-notification",
                    Label = "WU Auto-Restart: Enable Mandatory Restart Required Notification",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartRequiredNotificationDismissal=1 in WU policy. Configures Windows to show a non-dismissable restart required notification when a patch deadline is imminent. "
                        + "Without this, users can indefinitely dismiss restart prompts. With value=1, close-to-deadline notifications must be acknowledged with a concrete restart time selection rather than a simple dismiss.",
                    Tags = ["windows-update", "restart", "notification", "mandatory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Non-dismissable restart notification near deadline; forces users to choose restart time, increasing compliance.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartRequiredNotificationDismissal")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-enable-auto-restart-notification-config",
                    Label = "WU Auto-Restart: Enable Automatic Restart Notification Banner",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartNotificationConfig=1 in WU policy. Enables the automatic restart notification configuration, which shows a system tray and action centre banner when a pending restart is required. "
                        + "Without this setting the notification may be suppressed in locked-down enterprise notification policies. Enabling it ensures users are always informed of pending update restarts even in notification-restricted environments.",
                    Tags = ["windows-update", "restart", "notification", "banner", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enables restart notification banner in action centre; ensures user visibility of pending restarts in locked environments.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartNotificationConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartNotificationConfig")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartNotificationConfig", 1)],
                },
            ];
    }

    // ── WindowsPauseUpdatesPolicy ──
    private static class _WindowsPauseUpdatesPolicy
    {
        private const string PauseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
        private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pauseupd-defer-feature-30days",
                Label = "Windows Update Pause: Defer Feature Updates 30 Days",
                Category = "Windows Update",
                Description =
                    "Defers Windows feature updates by 30 days beyond their general availability date. "
                    + "Deferral gives IT administrators time to test compatibility before feature updates reach production endpoints. "
                    + "30 days is the minimum recommended deferral for enterprise deployments and allows Microsoft to identify critical regressions first. "
                    + "Removing this policy re-enables immediate feature update availability.",
                Tags = ["windows-update", "defer", "feature-update", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "DeferFeatureUpdatesPeriodInDays", 30)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "DeferFeatureUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(PauseKey, "DeferFeatureUpdatesPeriodInDays", 30)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Delays feature updates by 30 days; reduces exposure to day-zero feature regressions.",
            },
            new TweakDef
            {
                Id = "pauseupd-defer-quality-7days",
                Label = "Windows Update Pause: Defer Quality Updates 7 Days",
                Category = "Windows Update",
                Description =
                    "Defers Windows quality (security patch) updates by 7 days, allowing time for emergency patch retraction. "
                    + "Quality updates occasionally introduce regressions; a 7-day deferral window reduces blast radius from faulty patches. "
                    + "7 days is short enough to maintain adequate security posture while providing a testing buffer. "
                    + "Removing this policy makes quality updates available immediately upon release.",
                Tags = ["windows-update", "defer", "quality-update", "patch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "DeferQualityUpdatesPeriodInDays", 7)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "DeferQualityUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(PauseKey, "DeferQualityUpdatesPeriodInDays", 7)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Delays security patches by 7 days; provides testing buffer without excessive security lag.",
            },
            new TweakDef
            {
                Id = "pauseupd-disable-auto-install-on-shutdown",
                Label = "Windows Update Pause: Disable Auto-Install Updates on Shutdown",
                Category = "Windows Update",
                Description =
                    "Prevents Windows Update from automatically installing updates when the user initiates a shutdown. "
                    + "Auto-install-on-shutdown can extend shutdown times and cause unexpected restarts, especially on laptops before meetings. "
                    + "Updates are controlled through scheduled windows instead, giving IT full control over the timing. "
                    + "Removing this policy re-enables automatic installation during shutdown sequences.",
                Tags = ["windows-update", "shutdown", "auto-install", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "NoAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "NoAutoUpdate")],
                DetectOps = [RegOp.CheckDword(AuKey, "NoAutoUpdate", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents updates installing on shutdown; avoids unexpected extended shutdown times.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-active-hours-start",
                Label = "Windows Update Pause: Set Active Hours Start (8 AM)",
                Category = "Windows Update",
                Description =
                    "Configures the Windows Update active hours start time to 8 AM, preventing reboots for updates during business hours. "
                    + "Active hours protect users from unexpected reboots during the configured working hours window. "
                    + "Setting an explicit start ensures policy is enforced rather than relying on user configuration. "
                    + "Removing this policy reverts to Windows default or user-configured active hours.",
                Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursStart", 8)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursStart")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursStart", 8)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks active hours start to 8 AM; prevents update reboots interrupting morning workflows.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-active-hours-end",
                Label = "Windows Update Pause: Set Active Hours End (6 PM)",
                Category = "Windows Update",
                Description =
                    "Configures the Windows Update active hours end time to 6 PM (18:00), ensuring reboots cannot occur during standard business hours. "
                    + "With start fixed at 8 AM and end at 6 PM, the full working day is protected from forced reboots. "
                    + "Updates can install after 6 PM via the scheduled maintenance window. "
                    + "Removing this policy reverts to Windows default or user-configured active hours end.",
                Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursEnd", 18)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursEnd")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursEnd", 18)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks active hours end to 6 PM; complete 8 AM–6 PM protection from forced reboots.",
            },
            new TweakDef
            {
                Id = "pauseupd-block-driver-updates",
                Label = "Windows Update Pause: Block Driver Updates via Windows Update",
                Category = "Windows Update",
                Description =
                    "Prevents Windows Update from automatically downloading and installing driver updates. "
                    + "Automatic driver updates can replace validated enterprise drivers with incompatible versions, causing hardware failures or BSODs. "
                    + "Driver management should be handled by IT through validated packages rather than Windows Update. "
                    + "Removing this policy re-enables automatic driver updates through Windows Update.",
                Tags = ["windows-update", "driver", "exclusion", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ExcludeWUDriversInQualityUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ExcludeWUDriversInQualityUpdate")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ExcludeWUDriversInQualityUpdate", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks automatic driver updates via WU; prevents validated drivers being silently replaced.",
            },
            new TweakDef
            {
                Id = "pauseupd-disable-upgrade-notifications",
                Label = "Windows Update Pause: Disable Upgrade Notification Toasts",
                Category = "Windows Update",
                Description =
                    "Suppresses the Windows Update toast notifications that prompt users to restart for pending updates. "
                    + "In a managed environment, restart timing is controlled by IT policy — user-visible prompts are redundant and disruptive. "
                    + "Suppressing notifications prevents users from inadvertently triggering reboots outside the maintenance window. "
                    + "Removing this policy re-enables Windows Update restart notification toasts.",
                Tags = ["windows-update", "notifications", "restart", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(AuKey, "SetDisableUXWUAccess", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides WU restart prompts from users; IT maintains full control of update timing.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-update-detection-frequency",
                Label = "Windows Update Pause: Set Update Detection Frequency (22 Hours)",
                Category = "Windows Update",
                Description =
                    "Sets the Windows Update service to check for updates every 22 hours instead of the default automatic random interval. "
                    + "A predictable 22-hour check interval prevents multiple machines on the same network from surging the update server simultaneously. "
                    + "Combined with an WSUS/SCCM deployment, this ensures consistent, manageable update bandwidth. "
                    + "Removing this policy reverts to Windows' random detection frequency.",
                Tags = ["windows-update", "detection", "frequency", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled")],
                DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequencyEnabled", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Predictable 22-hour WU check interval; prevents bandwidth surge on shared networks.",
            },
            new TweakDef
            {
                Id = "pauseupd-allow-mu-updates",
                Label = "Windows Update Pause: Allow Microsoft Update for Other Products",
                Category = "Windows Update",
                Description =
                    "Configures Windows Update to also deliver updates for other Microsoft products (Office, .NET, Visual C++) alongside OS patches. "
                    + "Receiving all Microsoft product updates through a single channel simplifies patch management and reduces the attack surface. "
                    + "This is equivalent to enabling 'Give me updates for other Microsoft products' in Windows Update settings. "
                    + "Removing this policy reverts to OS-only updates via Windows Update.",
                Tags = ["windows-update", "microsoft-update", "office", "patch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "AllowMUUpdateService", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "AllowMUUpdateService")],
                DetectOps = [RegOp.CheckDword(AuKey, "AllowMUUpdateService", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables Microsoft Update for all products; consolidates patching into a single channel.",
            },
            new TweakDef
            {
                Id = "pauseupd-enforce-restart-deadline",
                Label = "Windows Update Pause: Enforce 72-Hour Restart Deadline",
                Category = "Windows Update",
                Description =
                    "Sets a 72-hour mandatory restart deadline after Windows Update installs updates requiring a reboot. "
                    + "Without a deadline, users can indefinitely postpone required restarts, leaving the system vulnerable to active exploits. "
                    + "72 hours provides reasonable flexibility for users to save work while ensuring security patches are applied promptly. "
                    + "Removing this policy removes the forced restart deadline.",
                Tags = ["windows-update", "restart", "deadline", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "SetAutoRestartDeadline", 72)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "SetAutoRestartDeadline")],
                DetectOps = [RegOp.CheckDword(PauseKey, "SetAutoRestartDeadline", 72)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Forces restart within 72 hours of patch install; prevents indefinite deferral of security updates.",
            },
        ];
    }

    // ── WindowsUpdateAdvanced ──
    private static class _WindowsUpdateAdvanced
    {
        private const string WuPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        private const string WuAu = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        private const string DeliveryOpt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wuadv-exclude-driver-updates",
                Label = "Exclude Driver Updates from Windows Update",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["windows update", "drivers", "policy", "quality update"],
                Description =
                    "Prevents Windows Update from automatically installing driver updates "
                    + "alongside quality/security updates. ExcludeWUDriversInQualityUpdate=1. "
                    + "Useful when you manage drivers manually via Device Manager or vendor tools.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "ExcludeWUDriversInQualityUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "ExcludeWUDriversInQualityUpdate")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "ExcludeWUDriversInQualityUpdate", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-defer-feature-updates-30-days",
                Label = "Defer Feature (Major) Updates by 30 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "feature update", "deferral", "stability"],
                Description =
                    "Delays the installation of major Windows feature updates (annual releases) "
                    + "by 30 days. DeferFeatureUpdatesPeriodInDays=30. Gives time for early bugs "
                    + "in new Windows versions to be patched before your machine upgrades.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "DeferFeatureUpdates", 1), RegOp.SetDword(WuPolicy, "DeferFeatureUpdatesPeriodInDays", 30)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "DeferFeatureUpdates"), RegOp.DeleteValue(WuPolicy, "DeferFeatureUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "DeferFeatureUpdatesPeriodInDays", 30)],
            },
            new TweakDef
            {
                Id = "wuadv-defer-quality-updates-7-days",
                Label = "Defer Quality (Security) Updates by 7 Days",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["windows update", "quality update", "security", "deferral"],
                Description =
                    "Delays monthly quality (security) updates by 7 days. "
                    + "DeferQualityUpdatesPeriodInDays=7. Allows time for faulty patches to be "
                    + "identified and pulled before your machine installs them, "
                    + "while keeping you close to the security patch baseline.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "DeferQualityUpdates", 1), RegOp.SetDword(WuPolicy, "DeferQualityUpdatesPeriodInDays", 7)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "DeferQualityUpdates"), RegOp.DeleteValue(WuPolicy, "DeferQualityUpdatesPeriodInDays")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "DeferQualityUpdatesPeriodInDays", 7)],
            },
            new TweakDef
            {
                Id = "wuadv-block-update-settings-access",
                Label = "Block Standard Users from Accessing Windows Update Settings",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["windows update", "settings", "access control", "admin"],
                Description =
                    "Prevents non-administrator users from accessing Windows Update settings. "
                    + "SetDisableUXWUAccess=1. Standard users cannot scan for, pause, or configure "
                    + "updates. Only administrators can manage the update schedule.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "SetDisableUXWUAccess", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-disable-update-reboot-notification",
                Label = "Suppress Forced Reboot Notifications After Updates",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "reboot", "notification", "restart"],
                Description =
                    "Prevents Windows Update from showing aggressive restart countdown notifications "
                    + "after installing updates. SetAutoRestartNotificationConfig=1 (suppress) / "
                    + "NoAutoRebootWithLoggedOnUsers=1. Users restart at their own pace.",
                ApplyOps = [RegOp.SetDword(WuAu, "NoAutoRebootWithLoggedOnUsers", 1)],
                RemoveOps = [RegOp.DeleteValue(WuAu, "NoAutoRebootWithLoggedOnUsers")],
                DetectOps = [RegOp.CheckDword(WuAu, "NoAutoRebootWithLoggedOnUsers", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-disable-delivery-optimization",
                Label = "Disable Delivery Optimization (P2P Update Sharing)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "delivery optimization", "p2p", "bandwidth"],
                Description =
                    "Disables Windows Delivery Optimization — the P2P update sharing feature that "
                    + "uploads update packages to other devices on the LAN or internet. "
                    + "DODownloadMode=0 (disabled). Eliminates upload bandwidth usage and privacy "
                    + "concerns about sharing data with unknown peers.",
                ApplyOps = [RegOp.SetDword(DeliveryOpt, "DODownloadMode", 0)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOpt, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DeliveryOpt, "DODownloadMode", 0)],
            },
            new TweakDef
            {
                Id = "wuadv-lan-only-delivery-optimization",
                Label = "Restrict Delivery Optimization to LAN Only",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["windows update", "delivery optimization", "lan", "p2p", "bandwidth"],
                Description =
                    "Restricts Delivery Optimization to only share update data with devices on "
                    + "the local LAN — not with external internet peers. DODownloadMode=1 (LAN "
                    + "only). Allows faster local updates while preventing internet upload.",
                ApplyOps = [RegOp.SetDword(DeliveryOpt, "DODownloadMode", 1)],
                RemoveOps = [RegOp.DeleteValue(DeliveryOpt, "DODownloadMode")],
                DetectOps = [RegOp.CheckDword(DeliveryOpt, "DODownloadMode", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-require-update-signature",
                Label = "Require Code-Signed Updates from WSUS",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "wsus", "signing", "security"],
                Description =
                    "Requires that all updates from a WSUS server are signed by a trusted publisher "
                    + "in the local machine certificate store. UsePolicyBasedQosMarkings=1 is the "
                    + "underlying policy; AcceptTrustedPublisherCerts=1 enables the WSUS signing check.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "AcceptTrustedPublisherCerts")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-allow-mu-updates-with-wu",
                Label = "Enable Microsoft Update (Office + Products) via Windows Update",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "microsoft update", "office", "products"],
                Description =
                    "Enables Microsoft Update service via the Windows Update policy — allows Office, "
                    + "Visual Studio, and other Microsoft products to receive updates through "
                    + "Windows Update instead of requiring separate update channels. "
                    + "EnableFeaturedSoftware=1.",
                ApplyOps = [RegOp.SetDword(WuAu, "EnableFeaturedSoftware", 1)],
                RemoveOps = [RegOp.DeleteValue(WuAu, "EnableFeaturedSoftware")],
                DetectOps = [RegOp.CheckDword(WuAu, "EnableFeaturedSoftware", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-set-active-hours-start",
                Label = "Set Windows Update Active Hours (8am–8pm)",
                Category = "Windows Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "active hours", "restart", "schedule"],
                Description =
                    "Sets Windows Update active hours to 8am–8pm (hours 8–20). Windows will not "
                    + "automatically restart to apply updates during these hours. "
                    + "ActiveHoursStart=8, ActiveHoursEnd=20. Prevents disruptive mid-day reboots.",
                ApplyOps =
                [
                    RegOp.SetDword(WuPolicy, "SetActiveHours", 1),
                    RegOp.SetDword(WuPolicy, "ActiveHoursStart", 8),
                    RegOp.SetDword(WuPolicy, "ActiveHoursEnd", 20),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(WuPolicy, "SetActiveHours"),
                    RegOp.DeleteValue(WuPolicy, "ActiveHoursStart"),
                    RegOp.DeleteValue(WuPolicy, "ActiveHoursEnd"),
                ],
                DetectOps = [RegOp.CheckDword(WuPolicy, "ActiveHoursStart", 8), RegOp.CheckDword(WuPolicy, "ActiveHoursEnd", 20)],
            },
        ];
    }

    // ── WindowsUpdateDriverPolicy ──
    private static class _WindowsUpdateDriverPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";
        private const string SignKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Driver Signing";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wudrv-deny-unidentified-device-installation",
                    Label = "WU Driver: Block Installation of Unidentified Device Drivers",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyUnidentifiedDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from installing drivers for hardware devices that are not in the Windows Driver Store and do not have a matching entry in Windows Update. "
                        + "Unidentified devices are a common attack vector — malicious USB devices can present as unknown hardware that auto-installs a malicious driver. This policy requires all devices to have a recognized driver before they can function.",
                    Tags = ["driver", "device", "security", "usb", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks unidentified device driver installs; prevents USB hardware-based driver injection attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyUnidentifiedDeviceInstallation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyUnidentifiedDeviceInstallation")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyUnidentifiedDeviceInstallation", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-deny-removable-device-driver-install",
                    Label = "WU Driver: Block Automatic Driver Installation for Removable Devices",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyRemovableDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from automatically installing drivers for any removable device. "
                        + "Removable devices (USB storage, USB hubs, card readers, portable audio devices) are frequently connected in enterprise environments. Without this policy, each new removable device triggers an automatic driver installation from WU, bypassing IT-managed driver sets and potentially installing unsigned or vulnerable drivers.",
                    Tags = ["driver", "removable", "usb", "device", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks auto-install of removable device drivers via WU; requires IT-managed driver pre-staging for new devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyRemovableDeviceInstallation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyRemovableDeviceInstallation")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyRemovableDeviceInstallation", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enforce-driver-signing-block-unsigned",
                    Label = "WU Driver: Block Installation of Unsigned Device Drivers",
                    Category = "Windows Update",
                    Description =
                        "Sets BehaviorOnFailedVerify=2 in Driver Signing policy. Configures Windows to silently block the installation of any device driver that fails digital signature verification. "
                        + "Value 2 = Block (value 1 = Warn, value 0 = Ignore). Blocking unsigned drivers prevents rootkits and malicious kernel-mode code from loading under the guise of a hardware driver. This is a critical defence-in-depth control alongside Secure Boot and HVCI.",
                    Tags = ["driver", "signing", "security", "kernel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Silently blocks unsigned drivers; prevents rootkits and kernel-level malware from installing via driver packages.",
                    ApplyOps = [RegOp.SetDword(SignKey, "BehaviorOnFailedVerify", 2)],
                    RemoveOps = [RegOp.DeleteValue(SignKey, "BehaviorOnFailedVerify")],
                    DetectOps = [RegOp.CheckDword(SignKey, "BehaviorOnFailedVerify", 2)],
                },
                new TweakDef
                {
                    Id = "wudrv-prevent-device-class-installations",
                    Label = "WU Driver: Enable Device Class Installation Restriction Policy",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyDeviceClasses=1 in DeviceInstall\\Restrictions policy. Activates the device class restriction feature that, when combined with a list of blocked device class GUIDs, prevents installation of entire categories of devices. "
                        + "This policy enables the enforcement of device class blocklists (e.g., blocking all Bluetooth adapters, all wireless adapters, or all imaging devices) across the enterprise without per-device ID management.",
                    Tags = ["driver", "device-class", "restriction", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Activates device class restriction framework; prerequisite for GUID-based device category blocklists.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyDeviceClasses", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyDeviceClasses")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyDeviceClasses", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enable-device-id-restriction-policy",
                    Label = "WU Driver: Enable Device ID-Based Installation Restriction",
                    Category = "Windows Update",
                    Description =
                        "Sets DenyDeviceIDs=1 in DeviceInstall\\Restrictions policy. Activates the device ID restriction feature. When enabled, Windows checks all device hardware IDs against a configured deny list. "
                        + "Device ID restrictions are more granular than class restrictions and allow blocking specific problematic hardware models (e.g., a specific USB key brand with a known firmware vulnerability) while permitting similar hardware from other vendors.",
                    Tags = ["driver", "device-id", "restriction", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Activates device ID restriction; enables HWID-based device blocklists for targeted hardware exclusions.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyDeviceIDs", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyDeviceIDs")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyDeviceIDs", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-log-driver-install-restriction-events",
                    Label = "WU Driver: Enable Event Logging for Blocked Driver Installations",
                    Category = "Windows Update",
                    Description =
                        "Sets WritePolicy=1 in DeviceInstall\\Restrictions policy. Enables Windows to write an event log entry whenever a device installation is blocked by Device Installation Policy. "
                        + "Without this, blocked installations fail silently, making it impossible to audit what hardware was attempted and blocked. With logging enabled, security teams can monitor for repeated installation attempts which may indicate hardware-based persistence attempts.",
                    Tags = ["driver", "logging", "audit", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Logs blocked driver installations to event log; enables audit trail for hardware-based attack detection.",
                    ApplyOps = [RegOp.SetDword(Key, "WritePolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WritePolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "WritePolicy", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-disable-windows-error-reporting-driver",
                    Label = "WU Driver: Disable Driver Crash Data Upload to Microsoft",
                    Category = "Windows Update",
                    Description =
                        "Sets DisableDriverLookup=1 in DeviceInstall\\Restrictions policy. Prevents Windows from looking up driver information and uploading crash data to the Microsoft Windows Error Reporting service when a device driver causes an error. "
                        + "In regulated environments, data sovereignty requirements may prohibit telemetry of driver crash details (device type, hardware ID, crash context) from being transmitted to Microsoft's cloud infrastructure.",
                    Tags = ["driver", "telemetry", "privacy", "wer", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks driver crash data upload to Microsoft; supports data sovereignty requirements for regulated industries.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDriverLookup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverLookup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDriverLookup", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-prevent-non-admin-driver-install",
                    Label = "WU Driver: Restrict Driver Installation to Administrators Only",
                    Category = "Windows Update",
                    Description =
                        "Sets PreventInstallationOfDevicesNotDescribedByOtherPolicySettings=1 in DeviceInstall\\Restrictions policy. Sets a default-deny posture for device installation: only devices explicitly permitted by an allowlist policy are installed. All others are blocked. "
                        + "This inverts the default Windows behaviour (allow-by-default) into a deny-by-default stance that requires active IT involvement to introduce any new device type into the environment.",
                    Tags = ["driver", "device", "allowlist", "default-deny", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Default-deny for new device types; requires IT-managed allowlist for any new hardware class to function.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enable-device-metadata-retrieval-block",
                    Label = "WU Driver: Block Device Metadata Retrieval from Windows Update",
                    Category = "Windows Update",
                    Description =
                        "Sets PreventDeviceMetadataFromNetwork=1 in DeviceInstall policy. Prevents Windows from searching the Windows Update network service for device metadata (device icons, model pages, UWP companion apps). "
                        + "Device metadata retrieval can prompt automatic download of companion apps without explicit user action. In locked-down environments, all device metadata should be pre-staged via WSUS rather than retrieved on-demand from Microsoft servers.",
                    Tags = ["driver", "metadata", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks network-sourced device metadata; prevents unsolicited companion app downloads on device connection.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork",
                            1
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork",
                            1
                        ),
                    ],
                },
                new TweakDef
                {
                    Id = "wudrv-allow-admin-override-device-restriction",
                    Label = "WU Driver: Allow Administrators to Override Device Installation Policy",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowAdminInstall=1 in DeviceInstall\\Restrictions policy. When device installation restrictions are in effect (including deny-by-default), this allows users in the local Administrators group to install any device regardless of policy restrictions. "
                        + "This maintains an escape hatch for IT staff to provision new hardware on managed endpoints without requiring a Group Policy update cycle, while standard users remain restricted.",
                    Tags = ["driver", "admin", "override", "device", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Allows admins to bypass device installation restrictions; provides IT escape hatch without weakening user-level controls.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAdminInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAdminInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAdminInstall", 1)],
                },
            ];
    }

    // ── WindowsUpdateNotificationPolicy ──
    private static class _WindowsUpdateNotificationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wunotif-set-update-notification-level-standard",
                    Label = "WU Notification: Set Update Notification Level to Standard",
                    Category = "Windows Update",
                    Description =
                        "Sets UpdateNotificationLevel=1 in WU policy. Configures the Windows Update notification level presented to users. "
                        + "Level 1 = Standard Notifications (users see action centre notifications and system tray alerts for pending updates). Level 2 = Disable all restart notifications. "
                        + "Setting level 1 ensures users are informed without overly aggressive interruptions, and is the baseline for notification management before other more specific controls are applied.",
                    Tags = ["windows-update", "notification", "level", "action-centre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sets base notification level; ensures users are informed of pending updates without restart interruptions.",
                    ApplyOps = [RegOp.SetDword(Key, "UpdateNotificationLevel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UpdateNotificationLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "UpdateNotificationLevel", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-suppress-restart-notification-when-busy",
                    Label = "WU Notification: Suppress Auto-Restart Notifications During Active Use",
                    Category = "Windows Update",
                    Description =
                        "Sets SuppressRestartNotification=1 in WU policy. Instructs Windows to suppress automatic restart notifications while the user is actively using the computer (mouse/keyboard activity detected). "
                        + "This prevents the restart prompt from appearing mid-presentation or mid-call, reducing user frustration while still allowing notifications when the device is idle.",
                    Tags = ["windows-update", "notification", "restart", "suppress", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses restart notifications during device activity; notifications appear only when user is idle.",
                    ApplyOps = [RegOp.SetDword(Key, "SuppressRestartNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SuppressRestartNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "SuppressRestartNotification", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-update-availability-popup",
                    Label = "WU Notification: Disable Update Availability Pop-Up Toast",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartNotificationExclusion=1 in WU policy. Disables the 'restart to update' toast notification pop-up that appears in the bottom-right corner of the screen. "
                        + "In enterprise SCCM/Intune-managed environments, the deployment tool provides its own notification and deadline management. The built-in WU toast in these environments creates duplicate, confusing messages that contradict the managed deployment window.",
                    Tags = ["windows-update", "notification", "toast", "popup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses WU toast pop-ups; eliminates duplicate notifications in SCCM/Intune managed environments.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartNotificationExclusion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartNotificationExclusion")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartNotificationExclusion", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-suppress-update-reboot-during-fullscreen",
                    Label = "WU Notification: Block Update Restart During Full-Screen Applications",
                    Category = "Windows Update",
                    Description =
                        "Sets SetAutoRestartDeadline=1 in WU policy combined with full-screen detection. Prevents Windows from showing the restart notification or initiating an automatic restart while a full-screen application is active. "
                        + "This is critical for kiosk, digital signage, and presentation machines where a mid-presentation WU restart notification would disrupt a live business event or customer-facing display.",
                    Tags = ["windows-update", "notification", "fullscreen", "kiosk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses WU restarts during full-screen apps; prevents disruption of presentations and digital signage.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartDeadline", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartDeadline")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartDeadline", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-upgrade-feature-notifications",
                    Label = "WU Notification: Disable Feature Upgrade Recommendation Notifications",
                    Category = "Windows Update",
                    Description =
                        "Sets DisableWindowsUpdateUI=0 in WU policy combined with DisableWUfBSafeguards=0. Suppresses the persistent Windows 11/Windows 10 upgrade promotion banners and notifications that appear when a newer major version is available. "
                        + "In enterprise environments managed to a specific OS release, these upgrade solicitations confuse users and generate IT support calls from users requesting to upgrade outside the approved schedule.",
                    Tags = ["windows-update", "notification", "upgrade", "feature", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses OS version upgrade promotions; prevents users from self-initiating unapproved major upgrades.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWUfBSafeguards", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWUfBSafeguards")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWUfBSafeguards", 0)],
                },
                new TweakDef
                {
                    Id = "wunotif-set-reboot-warning-timeout-15min",
                    Label = "WU Notification: Set Reboot Warning Timeout to 15 Minutes",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduleImminentRestartWarning=15 in WU policy. Sets the duration of the imminent-restart countdown dialog to 15 minutes. "
                        + "When Windows determines a restart is imminent (e.g., deadline approaching), this countdown gives users exactly 15 minutes to save their work before the restart proceeds. This is shorter than the ScheduleRestartWarning (advance warning hours) and is the 'last chance' save reminder.",
                    Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "15-minute last-chance countdown before restart; reduces data loss from unwarned forced restarts.",
                    ApplyOps = [RegOp.SetDword(Key, "ScheduleImminentRestartWarning", 15)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScheduleImminentRestartWarning")],
                    DetectOps = [RegOp.CheckDword(Key, "ScheduleImminentRestartWarning", 15)],
                },
                new TweakDef
                {
                    Id = "wunotif-enable-windows-update-log-events",
                    Label = "WU Notification: Enable Verbose Windows Update Event Logging",
                    Category = "Windows Update",
                    Description =
                        "Sets EnableDetailedLogging=1 in WU policy. Enables detailed verbose logging of Windows Update events to the Windows Event Log under the WindowsUpdateClient/Operational channel. "
                        + "By default, Windows Update logs minimal information. Detailed logs capture download start/stop, error codes, and deployment decisions, enabling IT to troubleshoot why updates fail, succeed late, or trigger unexpected restarts on specific machines.",
                    Tags = ["windows-update", "logging", "audit", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables verbose WU logging to event log; critical for diagnosing update failures and compliance audit trails.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDetailedLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDetailedLogging", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-block-user-changing-update-settings",
                    Label = "WU Notification: Block Users from Modifying Update Settings",
                    Category = "Windows Update",
                    Description =
                        "Sets SetUpdateNotificationLevel=2 in WU policy. Removes the Windows Update section from the Windows Settings app for standard users, so they cannot view or modify the pending update state, notification preferences, or restart schedules. "
                        + "For high-security and kiosk deployments, the WU settings page should be invisible to users to prevent them from deferring updates or changing restart windows outside of IT-approved schedules.",
                    Tags = ["windows-update", "settings", "user", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hides WU settings from non-admin users; prevents unauthorised deferrals or notification preference changes.",
                    ApplyOps = [RegOp.SetDword(Key, "SetUpdateNotificationLevel", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetUpdateNotificationLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "SetUpdateNotificationLevel", 2)],
                },
                new TweakDef
                {
                    Id = "wunotif-enable-update-health-tools-reporting",
                    Label = "WU Notification: Enable Update Health Tools Status Reporting",
                    Category = "Windows Update",
                    Description =
                        "Sets EnableUpdateHealthTools=1 in WU policy. Activates the Update Compliance Health Tools which report patch status, restart compliance, and update health metrics to Azure Monitor, Microsoft Endpoint Manager, or custom OMS workspaces. "
                        + "Without health tools enabled, IT dashboards show no patch status for affected machines, making it impossible to identify non-compliant devices in the estate.",
                    Tags = ["windows-update", "health", "reporting", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables patch status reporting to endpoint management platforms; provides patch compliance visibility.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableUpdateHealthTools", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableUpdateHealthTools")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableUpdateHealthTools", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-outdated-browser-notifications",
                    Label = "WU Notification: Disable Outdated Browser/App Update Notifications from WU",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowNonMicrosoftSignedUpdate=0 in WU policy. Prevents Windows Update from delivering and notifying about updates from non-Microsoft third-party publishers via the Microsoft Update service. "
                        + "Third-party update notifications through Windows Update are not needed when dedicated application management tools (SCCM, Intune, Chocolatey) are already used for non-OS software, reducing noise and preventing IT-unmanaged software updates.",
                    Tags = ["windows-update", "notification", "third-party", "apps", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Blocks third-party software update notifications via WU; channel reserved for OS updates only in managed environments.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowNonMicrosoftSignedUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
                },
            ];
    }

    // ── WindowsUpdatePolicy ──
    private static class _WindowsUpdatePolicy
    {
        private const string WuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wupol-disable-wu-access",
                    Label = "Disable Direct Windows Update Access",
                    Category = "Windows Update",
                    Description = "Blocks direct access to Windows Update servers; devices must use an internal WSUS or managed update source.",
                    Tags = ["windows-update", "wsus", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Prevents unmanaged updates; requires a WSUS or Microsoft Endpoint Manager infrastructure to deliver updates.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DisableWindowsUpdateAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DisableWindowsUpdateAccess")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DisableWindowsUpdateAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-block-internet-wu-locations",
                    Label = "Block Direct Connection to Windows Update Internet Locations",
                    Category = "Windows Update",
                    Description =
                        "Forces all update traffic through an internal catalog; prevents the client from contacting Microsoft update servers directly.",
                    Tags = ["windows-update", "internet", "policy", "wsus"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Devices only receive updates through the configured internal source; requires WUServer to be set.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DoNotConnectToWindowsUpdateInternetLocations")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-exclude-driver-updates",
                    Label = "Exclude Hardware Drivers from Windows Update",
                    Category = "Windows Update",
                    Description = "Prevents Windows Update from automatically delivering hardware driver updates through quality update channels.",
                    Tags = ["windows-update", "drivers", "policy", "stability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Driver updates must be installed manually or via WSUS driver category; prevents unstable driver push.",
                    ApplyOps = [RegOp.SetDword(WuKey, "ExcludeWUDriversInQualityUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "ExcludeWUDriversInQualityUpdate")],
                    DetectOps = [RegOp.CheckDword(WuKey, "ExcludeWUDriversInQualityUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-disable-os-upgrade",
                    Label = "Disable OS Upgrade Offers via Windows Update",
                    Category = "Windows Update",
                    Description = "Prevents Windows Update from offering or installing major operating system version upgrades.",
                    Tags = ["windows-update", "upgrade", "feature-update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Stops feature upgrades (e.g., Windows 10 → 11); quality and security patches are unaffected.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DisableOSUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DisableOSUpgrade")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DisableOSUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-quality-updates",
                    Label = "Defer Quality Updates",
                    Category = "Windows Update",
                    Description = "Enables deferral of quality (non-security) updates, delaying their installation after Microsoft release.",
                    Tags = ["windows-update", "quality-update", "deferral", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Quality non-security updates are delayed; security patches are included in quality updates and also deferred.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferQualityUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferQualityUpdates")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferQualityUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-quality-updates-14d",
                    Label = "Set Quality Update Deferral to 14 Days",
                    Category = "Windows Update",
                    Description = "Defers quality updates by 14 days after Microsoft releases them, providing a burn-in window.",
                    Tags = ["windows-update", "quality-update", "deferral", "days", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "14-day window to observe crash reports on early adopters before applying to managed devices.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferQualityUpdatesPeriodInDays", 14)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferQualityUpdatesPeriodInDays")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferQualityUpdatesPeriodInDays", 14)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-feature-updates",
                    Label = "Defer Feature Updates",
                    Category = "Windows Update",
                    Description = "Enables deferral of Windows feature updates, preventing the installation of new OS versions immediately.",
                    Tags = ["windows-update", "feature-update", "deferral", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Feature updates are held back until the deferral period expires; quality updates can be deferred separately.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferFeatureUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferFeatureUpdates")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferFeatureUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-defer-feature-updates-180d",
                    Label = "Set Feature Update Deferral to 180 Days",
                    Category = "Windows Update",
                    Description = "Defers Windows feature updates by 180 days, keeping the device on the current version for 6 months.",
                    Tags = ["windows-update", "feature-update", "deferral", "days", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Six-month stability window before upgrading OS; balance between security and compatibility testing.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DeferFeatureUpdatesPeriodInDays", 180)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DeferFeatureUpdatesPeriodInDays")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DeferFeatureUpdatesPeriodInDays", 180)],
                },
                new TweakDef
                {
                    Id = "wupol-block-preview-builds",
                    Label = "Block Windows Insider / Preview Builds",
                    Category = "Windows Update",
                    Description = "Prevents users from opting in to Windows Insider or preview builds on managed devices.",
                    Tags = ["windows-update", "insider", "preview", "policy", "stability"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot enrol in Windows Insider program; production build only on this device.",
                    ApplyOps = [RegOp.SetDword(WuKey, "ManagePreviewBuilds", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "ManagePreviewBuilds")],
                    DetectOps = [RegOp.CheckDword(WuKey, "ManagePreviewBuilds", 1)],
                },
                new TweakDef
                {
                    Id = "wupol-set-semi-annual-channel",
                    Label = "Set Update Branch to Semi-Annual Channel",
                    Category = "Windows Update",
                    Description = "Configures Windows Update to use the Semi-Annual Channel for feature update readiness (General Availability).",
                    Tags = ["windows-update", "branch", "semi-annual", "channel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Value 16 = Semi-Annual Channel; device receives GA feature updates rather than Insider or preview rings.",
                    ApplyOps = [RegOp.SetDword(WuKey, "BranchReadinessLevel", 16)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "BranchReadinessLevel")],
                    DetectOps = [RegOp.CheckDword(WuKey, "BranchReadinessLevel", 16)],
                },
            ];
    }

    // ── WindowsUpdateScanPolicy ──
    private static class _WindowsUpdateScanPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
        private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuscan-enable-wsus-server-mode",
                    Label = "WU Scan: Route Update Scanning Through WSUS Server",
                    Category = "Windows Update",
                    Description =
                        "Sets UseWUServer=1 in WU AU policy. Configures the Windows Update client to scan against the WSUS server configured in WUServer, rather than the public Windows Update service. "
                        + "This is the primary switch that activates WSUS-based update management. Without this flag set to 1, WUServer and WUStatusServer URL values are present in the registry but ignored by the WU client, which continues to scan against Microsoft's cloud endpoint.",
                    Tags = ["windows-update", "wsus", "server", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Activates WSUS-sourced scanning; all updates sourced from and approved via internal WSUS server.",
                    ApplyOps = [RegOp.SetDword(AuKey, "UseWUServer", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "UseWUServer")],
                    DetectOps = [RegOp.CheckDword(AuKey, "UseWUServer", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-wsus-scan-frequency-22hours",
                    Label = "WU Scan: Set WSUS Detection Frequency to 22 Hours",
                    Category = "Windows Update",
                    Description =
                        "Sets DetectionFrequency=22 and DetectionFrequencyEnabled=1 in WU AU policy. Configures the WU client to scan for updates every 22 hours instead of the default random interval (17-22 hours). "
                        + "A fixed 22-hour interval ensures predictable scan timing for environments where WSUS server load must be managed. Scan frequency should be set to complement WSUS synchronisation schedule so clients scan after the server has synced from Microsoft.",
                    Tags = ["windows-update", "wsus", "scan", "frequency", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "22-hour fixed scan interval; predictable WSUS load distribution vs. default random timing.",
                    ApplyOps = [RegOp.SetDword(AuKey, "DetectionFrequency", 22), RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "DetectionFrequency"), RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled")],
                    DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequency", 22)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-automatic-update-download-and-schedule",
                    Label = "WU Scan: Set Auto-Update Mode to Download and Schedule Install",
                    Category = "Windows Update",
                    Description =
                        "Sets AUOptions=4 in WU AU policy. Configures the auto-update behaviour to automatically download approved updates and schedule their installation for a configured maintenance window. "
                        + "AUOptions values: 2=Notify only, 3=Auto download + notify for install, 4=Auto download + schedule install, 5=Allow local admin to configure. Value 4 is standard for enterprise WSUS where deployments are scheduled to minimize business disruption.",
                    Tags = ["windows-update", "auto-update", "download", "schedule", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Auto-download with scheduled install; standard WSUS mode for planned maintenance window deployments.",
                    ApplyOps = [RegOp.SetDword(AuKey, "AUOptions", 4)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "AUOptions")],
                    DetectOps = [RegOp.CheckDword(AuKey, "AUOptions", 4)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-scheduled-install-day-0-every-day",
                    Label = "WU Scan: Set Scheduled Install Day to Every Day",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduledInstallDay=0 in WU AU policy. Configures Windows Update to install scheduled updates every day (rather than a specific day of the week). "
                        + "Day=0 means daily; Day=1-7 means a specific day (1=Sunday through 7=Saturday). Combined with ScheduledInstallTime, daily installation ensures patches are applied within 24 hours of their scheduled maintenance window rather than waiting up to a week.",
                    Tags = ["windows-update", "schedule", "install", "daily", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Daily scheduled install cadence; updates applied within 24h of availability rather than weekly batch.",
                    ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallDay", 0)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallDay")],
                    DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallDay", 0)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-scheduled-install-time-2am",
                    Label = "WU Scan: Set Scheduled Install Time to 2:00 AM",
                    Category = "Windows Update",
                    Description =
                        "Sets ScheduledInstallTime=2 in WU AU policy. Schedules automatic update installations to occur at 2:00 AM local time. "
                        + "2 AM is the classic maintenance window: after business hours, before early-morning workers arrive, outside of backup windows (typically 1–2 AM), and during a period when most machines are idle but still powered on. "
                        + "This time balances update deployment speed with business disruption minimisation.",
                    Tags = ["windows-update", "schedule", "install", "maintenance-window", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "2 AM scheduled installs; classic after-hours maintenance window that avoids business hours disruption.",
                    ApplyOps = [RegOp.SetDword(AuKey, "ScheduledInstallTime", 2)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "ScheduledInstallTime")],
                    DetectOps = [RegOp.CheckDword(AuKey, "ScheduledInstallTime", 2)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-intranet-update-service-stats",
                    Label = "WU Scan: Enable Intranet Update Statistics Reporting",
                    Category = "Windows Update",
                    Description =
                        "Sets UseWUServer=1 and IntranetServerInternetOptions=3 in WU AU policy. Configures the WU client to send update scan statistics (detection results, download progress, installation outcomes) to the WSUS status server rather than Microsoft. "
                        + "This populates the WSUS server's reporting database, enabling IT administrators to view an accurate picture of update compliance across the enterprise from the WSUS console.",
                    Tags = ["windows-update", "wsus", "reporting", "statistics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Routes update scan stats to WSUS; populates compliance reports in WSUS console.",
                    ApplyOps = [RegOp.SetDword(AuKey, "IntranetServerInternetOptions", 3)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "IntranetServerInternetOptions")],
                    DetectOps = [RegOp.CheckDword(AuKey, "IntranetServerInternetOptions", 3)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-automatic-minor-update-install",
                    Label = "WU Scan: Enable Automatic Installation of Minor Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets AutoInstallMinorUpdates=1 in WU AU policy. Allows Windows Update to automatically install minor (maintenance release) updates without user notification or interaction. "
                        + "Minor updates are typically service definition updates, component metadata refreshes, and low-risk patches that carry essentially no regression risk. Auto-installing these keeps the system at the latest minor version baseline without requiring a scheduled maintenance window for trivial updates.",
                    Tags = ["windows-update", "minor-updates", "auto-install", "baseline", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto-installs minor updates silently; keeps system at full baseline without scheduled window for low-risk patches.",
                    ApplyOps = [RegOp.SetDword(AuKey, "AutoInstallMinorUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "AutoInstallMinorUpdates")],
                    DetectOps = [RegOp.CheckDword(AuKey, "AutoInstallMinorUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-allow-mu-service-alongside-wu",
                    Label = "WU Scan: Scan Microsoft Update Service Alongside Windows Update",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowMUUpdateService=1 in WU AU policy. Opts the machine into the Microsoft Update (MU) service in addition to the base Windows Update service. "
                        + "Microsoft Update delivers updates for Office, Visual Studio, .NET, SQL Server, and other Microsoft products alongside OS updates. Without this setting, only Windows OS updates are delivered by WU, while Office and other products update through their own channels, which may not honour the configured maintenance window.",
                    Tags = ["windows-update", "microsoft-update", "office", "products", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enrolls in Microsoft Update alongside WU; Office and other MS products update in the same maintenance window.",
                    ApplyOps = [RegOp.SetDword(AuKey, "AllowMUUpdateService", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "AllowMUUpdateService")],
                    DetectOps = [RegOp.CheckDword(AuKey, "AllowMUUpdateService", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-reboot-launch-timeout-5min",
                    Label = "WU Scan: Set Post-Install Reboot Launch Timeout to 5 Minutes",
                    Category = "Windows Update",
                    Description =
                        "Sets RebootLaunchTimeout=5 and RebootLaunchTimeoutEnabled=1 in WU policy. After updates are installed during a scheduled maintenance window and a restart is required, Windows waits this many minutes before initiating the restart automatically. "
                        + "5 minutes gives any background processes time to complete gracefully while keeping the restart within the maintenance window. Without a timeout, the restart may be postponed indefinitely if a user was actively logged in during the overnight window.",
                    Tags = ["windows-update", "restart", "timeout", "maintenance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "5-minute post-install restart timeout; keeps restart within maintenance window while allowing graceful process shutdown.",
                    ApplyOps = [RegOp.SetDword(Key, "RebootLaunchTimeout", 5), RegOp.SetDword(Key, "RebootLaunchTimeoutEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RebootLaunchTimeout"), RegOp.DeleteValue(Key, "RebootLaunchTimeoutEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RebootLaunchTimeoutEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-reboot-warning-timeout-30min",
                    Label = "WU Scan: Set Pre-Restart Warning Timeout to 30 Minutes",
                    Category = "Windows Update",
                    Description =
                        "Sets RebootWarningTimeout=30 and RebootWarningTimeoutEnabled=1 in WU policy. Configures Windows to display a countdown restart warning 30 minutes before the scheduled restart. "
                        + "30 minutes provides a comfortable window for users to save work and close applications before the restart. This setting complements ScheduleRestartWarning (hours-in-advance general notice) — the 30-minute warning is the final specific countdown before imminent restart.",
                    Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "30-minute final restart countdown; gives users time to save before scheduled maintenance restart.",
                    ApplyOps = [RegOp.SetDword(Key, "RebootWarningTimeout", 30), RegOp.SetDword(Key, "RebootWarningTimeoutEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RebootWarningTimeout"), RegOp.DeleteValue(Key, "RebootWarningTimeoutEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RebootWarningTimeoutEnabled", 1)],
                },
            ];
    }

    // ── WindowsUpdateUsoPolicy ──
    private static class _WindowsUpdateUsoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuuso-block-wu-downloads-metered-network",
                    Label = "WU USO: Block Windows Update Downloads on Metered Networks",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowAutoWindowsUpdateDownloadOverMeteredNetwork=0 in WU policy. Prevents Windows Update from automatically downloading update packages when the active network connection is marked as metered. "
                        + "On mobile devices and machines on cellular or satellite connections, unrestricted WU downloads can exhaust data allowances or incur substantial overage charges. This policy applies to both background and foreground download scenarios.",
                    Tags = ["windows-update", "metered", "network", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks WU auto-downloads on metered connections; prevents data-plan exhaustion on mobile/satellite links.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-temporary-enterprise-feature-drops",
                    Label = "WU USO: Block In-Period Temporary Enterprise Feature Drops",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowTemporaryEnterpriseFeatureControl=0 in WU policy. Disables the delivery of optional 'temporary enterprise feature' updates — incremental functionality enhancements that Microsoft ships between major version releases. "
                        + "These in-period feature drops are not security updates and can change application behaviour mid-support-lifecycle. Blocking them keeps the OS in a stable, enterprise-validated state between planned upgrade windows.",
                    Tags = ["windows-update", "features", "enterprise", "stability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks temporary enterprise feature drops; keeps OS behaviour predictable between scheduled upgrade events.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowTemporaryEnterpriseFeatureControl")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-prevent-user-pausing-updates",
                    Label = "WU USO: Prevent Users from Pausing Windows Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets SetDisablePauseUXAccess=1 in WU policy (AU subkey). Removes the 'Pause Updates' option from the Windows Update settings UI. "
                        + "Without this policy, standard users can pause updates for up to 5 weeks, leaving machines unpatched and out of compliance. This is a key control in corporate environments operating under patch management SLAs where user-initiated update deferrals are not permitted.",
                    Tags = ["windows-update", "pause", "user", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removes pause updates control from user UI; ensures patch compliance SLAs are not bypassed by users.",
                    ApplyOps = [RegOp.SetDword(Key, "SetDisablePauseUXAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetDisablePauseUXAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "SetDisablePauseUXAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-disable-dual-scan-on-wsus",
                    Label = "WU USO: Disable Dual-Scan When WSUS Is Configured",
                    Category = "Windows Update",
                    Description =
                        "Sets DisableDualScan=1 in WU policy. When a WSUS server (WUServer) is configured, Windows 10/11 will by default simultaneously scan both the WSUS server and the public Windows Update/Microsoft Update cloud. "
                        + "This 'dual scan' allows unapproved updates to arrive from the cloud even when WSUS approval workflows are in place. Disabling dual scan ensures all updates flow exclusively through WSUS, preserving IT update approval control.",
                    Tags = ["windows-update", "wsus", "dual-scan", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks cloud WU source when WSUS is configured; enforces WSUS approval pipeline with no cloud bypass.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDualScan", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDualScan")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDualScan", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-internet-wu-when-wsus-active",
                    Label = "WU USO: Block Internet Windows Update Access When WSUS Active",
                    Category = "Windows Update",
                    Description =
                        "Sets DoNotConnectToWindowsUpdateInternetLocations=1 in WU policy. When active, prevents the WU client from connecting to the public internet endpoints for update detection, metadata, or downloads. "
                        + "This is required in air-gapped or WSUS-only environments where all internet traffic is blocked by firewall policy. Without this setting, WU may attempt internet connections that trigger firewall alerts or fail silently and produce misleading update status.",
                    Tags = ["windows-update", "wsus", "internet", "air-gapped", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks all public WU internet connections; required for WSUS-only or air-gapped deployment scenarios.",
                    ApplyOps = [RegOp.SetDword(Key, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DoNotConnectToWindowsUpdateInternetLocations")],
                    DetectOps = [RegOp.CheckDword(Key, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-recommended-updates-auto-install",
                    Label = "WU USO: Block Automatic Installation of Recommended Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets IncludeRecommendedUpdates=0 in WU policy. Prevents Windows Update from automatically installing 'recommended' updates which include non-security improvements, application updates, and optional Windows features. "
                        + "In enterprise environments, recommended updates should be reviewed and approved through a patch management process rather than automatically deployed, as they can change application behaviour without a security justification.",
                    Tags = ["windows-update", "recommended", "auto-install", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks auto-install of recommended updates; only critical and security updates deploy automatically.",
                    ApplyOps = [RegOp.SetDword(Key, "IncludeRecommendedUpdates", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IncludeRecommendedUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "IncludeRecommendedUpdates", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-allow-only-trusted-publisher-certs",
                    Label = "WU USO: Accept Only Updates from Trusted Publisher Certificates",
                    Category = "Windows Update",
                    Description =
                        "Sets AcceptTrustedPublisherCerts=1 in WU policy. Configures the WU client to only accept and install updates that are signed by certificates in the machine's Trusted Publishers certificate store. "
                        + "This prevents installation of updates signed by untrusted authority chains, which is relevant in WSUS deployments where custom update packages may be published by third parties or internal teams.",
                    Tags = ["windows-update", "trusted-publisher", "certificate", "signing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only installs updates signed by trusted publisher certificates; guards against malicious WSUS packages.",
                    ApplyOps = [RegOp.SetDword(Key, "AcceptTrustedPublisherCerts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AcceptTrustedPublisherCerts")],
                    DetectOps = [RegOp.CheckDword(Key, "AcceptTrustedPublisherCerts", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-optional-content-updates",
                    Label = "WU USO: Block Optional Windows Content Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets AllowOptionalContent=0 in WU policy. Prevents Windows Update from offering and installing optional content packages — these include font packs, additional language components, accessibility features, and recreational apps. "
                        + "Optional content updates consume storage and bandwidth and are not security-relevant. Blocking them reduces WU noise and storage footprint on tightly managed enterprise machines.",
                    Tags = ["windows-update", "optional", "content", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks optional Windows content updates; reduces WU bandwidth and storage usage on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowOptionalContent", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowOptionalContent")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowOptionalContent", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-featured-software-via-wu",
                    Label = "WU USO: Block Automatic Installation of Featured Software",
                    Category = "Windows Update",
                    Description =
                        "Sets EnableFeaturedSoftware=0 in WU policy. Stops Windows Update from offering and automatically installing 'featured software' — typically free Microsoft utilities, game trials, and promotional apps. "
                        + "Without this setting, WU silently installs marketing-tied software packages that were never requested by the user or IT administrator, increasing the installed application footprint and creating an unexpected change management event.",
                    Tags = ["windows-update", "featured", "software", "bloat", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks OEM/Microsoft featured software installs via WU; prevents unsolicited app additions on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableFeaturedSoftware", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableFeaturedSoftware")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableFeaturedSoftware", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-policy-driven-other-update-source",
                    Label = "WU USO: Force Policy-Driven Update Source for Other Updates",
                    Category = "Windows Update",
                    Description =
                        "Sets SetPolicyDrivenUpdateSourceForOtherUpdates=1 in WU policy. Ensures that non-feature, non-quality updates (such as drivers from the 'Other' category in WU) are sourced exclusively through the configured policy-driven update source (WSUS/SCCM). "
                        + "Without this setting, updates in the 'Other' category may still be retrieved directly from Microsoft Update regardless of the WSUS or DeliveryOptimization configuration.",
                    Tags = ["windows-update", "wsus", "policy-driven", "other-updates", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Routes 'Other' category updates through policy-driven source; closes WSUS bypass for non-standard update types.",
                    ApplyOps = [RegOp.SetDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
                },
            ];
    }
}
