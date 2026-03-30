namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Multimedia
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [

        new TweakDef
        {
            Id = "media-disable-autorun",
            Label = "Disable AutoRun for All Drives",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables AutoRun for all drive types, preventing automatic execution of autorun.inf files. Mitigates USB-based malware. Default: Enabled. Recommended: Disabled.",
            Tags = ["multimedia", "autorun", "security", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-media-sharing",
            Label = "Disable Media Streaming/Sharing Service",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Media Player Network Sharing Service (WMPNetworkSvc). Prevents DLNA media streaming to other devices. Default: Manual. Recommended: Disabled.",
            Tags = ["multimedia", "sharing", "dlna", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "media-set-wallpaper-quality",
            Label = "Set Wallpaper JPEG Quality to Maximum",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets desktop wallpaper JPEG import quality to 100 percent. Prevents Windows from recompressing wallpaper images. Default: ~85. Recommended: 100.",
            Tags = ["multimedia", "wallpaper", "quality", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
        },


        new TweakDef
        {
            Id = "media-disable-media-streaming",
            Label = "Disable Windows Media Streaming (Policy)",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows Media Player library sharing via policy. Prevents media streaming to other devices on the network. Default: allowed. Recommended: disabled for security.",
            Tags = ["multimedia", "streaming", "media", "sharing", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
        },
        new TweakDef
        {
            Id = "media-set-default-player-assoc",
            Label = "Set Default Media Player Associations",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Suppresses WMP first-run setup and player prompt via policy. Prevents Windows Media Player from claiming file associations. Default: enabled. Recommended: disabled.",
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
            Description =
                "Disables Windows Media DRM online license acquisition via policy. Prevents DRM phone-home for protected media content. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["multimedia", "drm", "wmdrm", "license", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-gamebar-policy",
            Label = "Disable Xbox Game Bar (Policy)",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Xbox Game Bar via Group Policy (AllowGameDVR=0). Prevents Win+G from opening and removes the overlay entirely. Default: Allowed. Recommended: Disabled for non-gaming workstations.",
            Tags = ["multimedia", "gamebar", "xbox", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "media-menu-show-instant",
            Label = "Instant Context Menu (0 ms Delay)",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MenuShowDelay to 0, making context menus and pop-up menus appear instantly. Default: 400 ms. Recommended: 0 for power users.",
            Tags = ["multimedia", "menu", "delay", "performance", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "400")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MenuShowDelay", "0")],
        },
        new TweakDef
        {
            Id = "media-disable-logon-anim",
            Label = "Disable First-Logon Animation",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the animated user-tile / Getting Ready screen shown on first logon after updates. Speeds up login for domain and managed devices. Default: Enabled.",
            Tags = ["multimedia", "logon", "animation", "boot", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "EnableFirstLogonAnimation", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "EnableFirstLogonAnimation", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "EnableFirstLogonAnimation", 0),
            ],
        },
        new TweakDef
        {
            Id = "media-reduce-tooltip-delay",
            Label = "Instant Tooltip Display (0 ms)",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets extended UI hover time to 0, making Explorer tooltips appear immediately. Default: system default. Recommended: 0 for fast typists.",
            Tags = ["multimedia", "tooltip", "delay", "explorer", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 0),
            ],
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
            Description =
                "Sets the Windows sound scheme to None, disabling all system sounds. Creates a silent desktop experience. Default: Windows Default.",
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
            Description =
                "Disables Windows Media Player network sharing service. Prevents media library from being shared across the network. Default: enabled.",
            Tags = ["multimedia", "wmp", "sharing", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "media-disable-wmp-autoplay",
            Label = "Disable WMP AutoPlay",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Media Player from automatically playing media when inserted.",
            Tags = ["multimedia", "wmp", "autoplay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings", "AutoStart", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings", "AutoStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings", "AutoStart", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-wmp-codec-download",
            Label = "Disable WMP Automatic Codec Download",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Media Player from automatically downloading codecs from the internet.",
            Tags = ["multimedia", "wmp", "codecs", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-video-thumbnail-cache",
            Label = "Disable Video Thumbnail Cache",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops Windows from caching video thumbnail images in hidden system folders.",
            Tags = ["multimedia", "thumbnails", "privacy", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableThumbnailCache", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableThumbnailCache")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableThumbnailCache", 1)],
        },
        new TweakDef
        {
            Id = "media-set-system-responsiveness-media",
            Label = "Optimise System Responsiveness for Media",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MMCSS SystemResponsiveness to 0 so multimedia threads get maximum CPU time.",
            Tags = ["multimedia", "performance", "audio", "mmcss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    20
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "media-enable-hardware-video-decode",
            Label = "Enable Hardware-Accelerated Video Decode",
            Category = "Multimedia",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces Windows apps to use GPU hardware decoding for video playback to reduce CPU load.",
            Tags = ["multimedia", "gpu", "performance", "video"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "VideoQualityMode", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "VideoQualityMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "VideoQualityMode", 2)],
        },
        new TweakDef
        {
            Id = "media-set-pro-audio-latency",
            Label = "Set Pro Audio Scheduling Latency",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MMCSS Pro Audio task scheduling key for lower latency; optimises for audio production.",
            Tags = ["multimedia", "audio", "latency", "pro-audio", "mmcss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "media-disable-casting-extension",
            Label = "Disable Cast to Device Extension",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Cast to device' option from Explorer context menus.",
            Tags = ["multimedia", "casting", "context-menu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "media-disable-media-metadata-streaming",
            Label = "Disable Media Metadata Internet Lookup",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Media Player from downloading album art and track metadata from the internet.",
            Tags = ["multimedia", "privacy", "metadata", "wmp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventCDDVDMetadataRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventCDDVDMetadataRetrieval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventCDDVDMetadataRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-media-usage-reporting",
            Label = "Disable Media Usage Reporting",
            Category = "Multimedia",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables anonymous usage reporting sent from Windows Media Player to Microsoft.",
            Tags = ["multimedia", "privacy", "telemetry", "wmp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventMusicFileMetadataRetrieval", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventMusicFileMetadataRetrieval"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventMusicFileMetadataRetrieval", 1),
            ],
        },
    ];
}
