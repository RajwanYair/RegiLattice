namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Multimedia
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "media-disable-autoplay",
            Label = "Disable AutoPlay for All Drives",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables AutoPlay for all media and removable drives via NoDriveTypeAutoRun policy. Prevents automatic execution of media content. Default: Enabled. Recommended: Disabled.",
            Tags = ["multimedia", "autoplay", "security", "drives"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
        },
        new TweakDef
        {
            Id = "media-disable-autorun",
            Label = "Disable AutoRun for All Drives",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables AutoRun for all drive types, preventing automatic execution of autorun.inf files. Mitigates USB-based malware. Default: Enabled. Recommended: Disabled.",
            Tags = ["multimedia", "autorun", "security", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-media-sharing",
            Label = "Disable Media Streaming/Sharing Service",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Media Player Network Sharing Service (WMPNetworkSvc). Prevents DLNA media streaming to other devices. Default: Manual. Recommended: Disabled.",
            Tags = ["multimedia", "sharing", "dlna", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "media-disable-game-dvr",
            Label = "Disable Game DVR Captures",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Game DVR background recording and captures. Frees GPU encoder resources and reduces disk I/O. Default: Enabled. Recommended: Disabled.",
            Tags = ["multimedia", "game-dvr", "recording", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-game-bar-tips",
            Label = "Disable Game Bar Startup Tips",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Game Bar startup panel and tips overlay. Removes the popup that appears when launching games. Default: Enabled. Recommended: Disabled.",
            Tags = ["multimedia", "gamebar", "tips", "overlay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-screensaver",
            Label = "Disable Screen Saver",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows screen saver. Prevents screen saver from activating during idle periods. Default: Enabled. Recommended: Disabled on desktops.",
            Tags = ["multimedia", "screensaver", "display", "idle"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "ScreenSaveActive", "0")],
        },
        new TweakDef
        {
            Id = "media-set-wallpaper-quality",
            Label = "Set Wallpaper JPEG Quality to Maximum",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets desktop wallpaper JPEG import quality to 100 percent. Prevents Windows from recompressing wallpaper images. Default: ~85. Recommended: 100.",
            Tags = ["multimedia", "wallpaper", "quality", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
        },
        new TweakDef
        {
            Id = "media-disable-window-animations",
            Label = "Disable Window Minimize/Maximize Animations",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables minimize and maximize window animations. Makes window actions feel instant. Default: Enabled. Recommended: Disabled for responsiveness.",
            Tags = ["multimedia", "animations", "performance", "windows"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "MinAnimate", "0")],
        },
        new TweakDef
        {
            Id = "media-disable-startup-sound",
            Label = "Disable Windows Startup Sound",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows startup/logon sound. Silences the chime played during boot. Default: Enabled. Recommended: Disabled.",
            Tags = ["multimedia", "startup", "sound", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-autoplay-handlers",
            Label = "Disable AutoPlay Handlers (User)",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AutoPlay handlers at the user level via DisableAutoplay DWORD. Prevents automatic launch of programs when media is inserted. Default: enabled. Recommended: disabled for security.",
            Tags = ["multimedia", "autoplay", "handlers", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
        },
        new TweakDef
        {
            Id = "media-disable-media-streaming",
            Label = "Disable Windows Media Streaming (Policy)",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Media Player library sharing via policy. Prevents media streaming to other devices on the network. Default: allowed. Recommended: disabled for security.",
            Tags = ["multimedia", "streaming", "media", "sharing", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
        },
        new TweakDef
        {
            Id = "media-set-default-player-assoc",
            Label = "Set Default Media Player Associations",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Suppresses WMP first-run setup and player prompt via policy. Prevents Windows Media Player from claiming file associations. Default: enabled. Recommended: disabled.",
            Tags = ["multimedia", "player", "associations", "default", "wmp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "SetupFirstRun", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PlayerPrompt", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "SetupFirstRun"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PlayerPrompt"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "SetupFirstRun", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-wm-drm",
            Label = "Disable Windows Media DRM",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Windows Media DRM online license acquisition via policy. Prevents DRM phone-home for protected media content. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["multimedia", "drm", "wmdrm", "license", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-gamebar-policy",
            Label = "Disable Xbox Game Bar (Policy)",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Xbox Game Bar via Group Policy (AllowGameDVR=0). Prevents Win+G from opening and removes the overlay entirely. Default: Allowed. Recommended: Disabled for non-gaming workstations.",
            Tags = ["multimedia", "gamebar", "xbox", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "media-menu-show-instant",
            Label = "Instant Context Menu (0 ms Delay)",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets MenuShowDelay to 0, making context menus and pop-up menus appear instantly. Default: 400 ms. Recommended: 0 for power users.",
            Tags = ["multimedia", "menu", "delay", "performance", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "400"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0")],
        },
        new TweakDef
        {
            Id = "media-disable-logon-anim",
            Label = "Disable First-Logon Animation",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the animated user-tile / Getting Ready screen shown on first logon after updates. Speeds up login for domain and managed devices. Default: Enabled.",
            Tags = ["multimedia", "logon", "animation", "boot", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "EnableFirstLogonAnimation", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "EnableFirstLogonAnimation", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "EnableFirstLogonAnimation", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-cursor-blink",
            Label = "Disable Text Cursor Blinking",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables cursor blinking in text fields (CursorBlinkRate=-1). Reduces visual distraction for users who find blinking cursors disruptive. Default: 530 ms.",
            Tags = ["multimedia", "cursor", "blink", "accessibility", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "-1"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "530"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "-1")],
        },
        new TweakDef
        {
            Id = "media-reduce-tooltip-delay",
            Label = "Instant Tooltip Display (0 ms)",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets extended UI hover time to 0, making Explorer tooltips appear immediately. Default: system default. Recommended: 0 for fast typists.",
            Tags = ["multimedia", "tooltip", "delay", "explorer", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-auto-play",
            Label = "Disable AutoPlay for All Drives",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables AutoPlay for all removable and fixed drives. Prevents automatic execution of media. Default: enabled.",
            Tags = ["media", "autoplay", "security", "drives"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "media-set-wmf-no-telemetry",
            Label = "Disable Windows Media Foundation Telemetry",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables telemetry/DRM phone-home in Windows Media Foundation components. Default: enabled.",
            Tags = ["media", "wmf", "telemetry", "drm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-casting",
            Label = "Disable Media Casting to Device",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Cast to Device feature for media streaming. Default: enabled.",
            Tags = ["media", "cast", "device", "streaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver", "AutoEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver", "AutoEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver", "AutoEnabled", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-media-player-sharing",
            Label = "Disable Windows Media Player Network Sharing",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Media Player Network Sharing Service. Prevents DLNA media streaming. Default: enabled.",
            Tags = ["multimedia", "media-player", "sharing", "dlna"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "media-disable-disc-burning",
            Label = "Disable CD/DVD Burning in Explorer",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the built-in CD/DVD burning capability in Windows Explorer. Default: enabled.",
            Tags = ["multimedia", "disc", "burning", "cdrom"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoCDBurning", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoCDBurning")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoCDBurning", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-sound-scheme",
            Label = "Disable System Sound Scheme",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the Windows sound scheme to None, disabling all system sounds. Creates a silent desktop experience. Default: Windows Default.",
            Tags = ["multimedia", "sound", "scheme", "silent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\AppEvents\Schemes"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".Default")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
        },
        new TweakDef
        {
            Id = "media-disable-wmp-network-sharing",
            Label = "Disable WMP Network Sharing",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Media Player network sharing service. Prevents media library from being shared across the network. Default: enabled.",
            Tags = ["multimedia", "wmp", "sharing", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
        },
    ];
}
