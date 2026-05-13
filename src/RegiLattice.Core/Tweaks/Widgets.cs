namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// === Merged from: Widgets.cs ===

[TweakModule]
internal static class Widgets
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "widgets-disable-third-party-suggestions",
            Label = "Disable Third-Party App Suggestions",
            Category = "Windows 11 3",
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
            Category = "Windows 11 3",
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
            Id = "widgets-disable-spotlight-in-settings-gpo",
            Label = "Disable Windows Spotlight in Settings App (Policy)",
            Category = "Windows 11 3",
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
            Id = "widgets-disable-taskbar-meet-now",
            Label = "Hide Teams Meet Now Button from Taskbar",
            Category = "Windows 11 3",
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
            Category = "Windows 11 3",
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
