// RegiLattice.Core — Tweaks/MediaPlayerAdvPolicy.cs
// Windows Media Player Advanced Policy — Sprint 630.
// Category: "Media Player Adv Policy" | Slug: wmpol
// Key: HKLM\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MediaPlayerAdvPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wmpol-prevent-codec-download",
            Label = "WMP: Prevent Automatic Codec Download from the Internet",
            Category = "Media Player Adv Policy",
            Description = "Sets PreventCodecDownload=1 in Windows Media Player policy. Prevents Windows Media Player from automatically downloading codecs from the internet to play media files that use unknown or missing codecs. Automatic codec download has historically been a malware delivery vector: specially crafted media files embedded codec 'requirements' that redirected to malicious codec installer EXEs from attacker-controlled servers rather than legitimate codec repositories. " +
                "The drive-by codec attack vector was prevalent in the Windows XP/Vista era: opening a video file triggers WMP's codec detection, which displays a dialog offering to download from a URL embedded in the media file's codec detection field — which can point to any server. Modern enterprise security policies require that all software (including codecs) be installed through approved channels (SCCM, Intune). Blocking automatic codec download ensures users cannot inadvertently install unapproved software via a malicious media file.",
            Tags = ["wmpol", "windows-media-player", "codec", "download", "malware", "drive-by"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WMP cannot auto-download codecs. Users will see 'codec missing' error for unsupported formats. Enterprise codec deployments via SCCM/Intune unaffected.",
            ApplyOps = [RegOp.SetDword(Key, "PreventCodecDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventCodecDownload")],
            DetectOps = [RegOp.CheckDword(Key, "PreventCodecDownload", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-disable-auto-update",
            Label = "WMP: Disable Windows Media Player Automatic Online Update Check",
            Category = "Media Player Adv Policy",
            Description = "Sets DisableAutoUpdate=1 in Windows Media Player policy. Prevents Windows Media Player from automatically checking for updates and new functionality online. WMP update checks contact Microsoft servers on every WMP launch, contributing to outbound telemetry traffic and potentially introducing version changes to a controlled software baseline. " +
                "On enterprise-managed systems where application updates are managed by WSUS or SCCM, unsolicited update checks by individual applications create unpredictable patching timelines. WMP updates have in the past introduced new codec support, UI changes, and DRM policy updates that required revalidation by enterprise compatibility teams. Disabling auto-update ensures WMP version state is controlled exclusively by IT-managed patching processes.",
            Tags = ["wmpol", "windows-media-player", "auto-update", "baseline", "wsus"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WMP update checks disabled. Updates delivered via WSUS/Windows Update instead of direct WMP check. No functional impact on media playback.",
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-disable-network-settings",
            Label = "WMP: Prevent User Modification of Network Streaming Settings",
            Category = "Media Player Adv Policy",
            Description = "Sets DisableNetworkSettings=1 in Windows Media Player policy. Prevents users from modifying Windows Media Player network settings (streaming protocol selection, proxy configuration, bandwidth usage). On corporate networks, streaming settings should be configured by IT to ensure WMP uses approved proxy settings and consumption limits, preventing users from configuring direct internet streaming paths that bypass proxy inspection. " +
                "WMP network settings include the ability to configure RTSP and HTTP streaming protocol preferences and proxy exclusion lists. A user who configures WMP to bypass the corporate proxy for streaming sources creates an uninspected traffic path for internet-sourced audio/video streams. Corporate DLP and web filtering policies rely on all internet traffic flowing through the approved proxy for content inspection. Locking WMP network settings prevents direct-to-internet streaming paths.",
            Tags = ["wmpol", "windows-media-player", "network-settings", "proxy", "streaming", "dlp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WMP network settings page locked. Streaming uses system proxy settings configured by IT. Users cannot configure alternative streaming protocol paths.",
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkSettings")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkSettings", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-disable-privacy-tab",
            Label = "WMP: Hide Privacy Settings Tab to Lock in Policy-Configured Privacy Options",
            Category = "Media Player Adv Policy",
            Description = "Sets DisablePrivacyTab=1 in Windows Media Player policy. Hides the Privacy tab in Windows Media Player preferences, preventing users from changing privacy settings (DRM licence acquisition, licence backup, Windows Media metafile security, internet radio station access). Hiding the tab ensures IT-configured privacy settings remain in effect and cannot be reversed by end users. " +
                "The WMP Privacy tab controls whether WMP sends usage data to Microsoft (Enhanced Playback Experience / CEIP), whether it acquires media player licences automatically, and whether it shows WMP in the Media Guide. In corp environments where these settings are locked by policy, displaying the Privacy tab presents options the user cannot actually save — leading to confusion and support desk calls. Hiding the tab presents a cleaner, policy-consistent experience.",
            Tags = ["wmpol", "windows-media-player", "privacy-tab", "policy-lock", "drm"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "WMP Privacy settings tab hidden. Privacy policy settings enforced by Group Policy regardless of this setting.",
            ApplyOps = [RegOp.SetDword(Key, "DisablePrivacyTab", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacyTab")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrivacyTab", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-disable-media-sharing",
            Label = "WMP: Disable Windows Media Player Media Sharing Service",
            Category = "Media Player Adv Policy",
            Description = "Sets DisableMedia​Sharing=1 in Windows Media Player policy (RegSZ). Wait — this should be SetDword. Disabling media sharing prevents the Windows Media Sharing UPnP service from advertising the local media library to other devices on the network. WMP's media sharing exposes a UPnP media server that broadcasts the local music, video, and picture library to all devices on the same subnet. " +
                "UPnP-based media sharing is a network discovery and information disclosure risk: the WMP UPnP server exposes a list of all media files in the user's library to any device on the same network (including guest Wi-Fi segments if inter-VLAN routing allows). File names, album metadata, and media thumbnails may contain sensitive information or personal data. On corporate networks, the UPnP media broadcasting also generates multicast traffic that consumes bandwidth and may trigger IDS rules configured to alert on UPnP device announcements from endpoints.",
            Tags = ["wmpol", "windows-media-player", "media-sharing", "upnp", "network-discovery"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WMP media sharing (UPnP DLNA) disabled. Media library not advertised on network. Home users who stream to smart TVs will need to re-enable this setting.",
            ApplyOps = [RegOp.SetDword(Key, "DisableMediaSharingTab", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaSharingTab")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMediaSharingTab", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-prevent-drm-internet-access",
            Label = "WMP: Block DRM Internet Connections for Licence Acquisition",
            Category = "Media Player Adv Policy",
            Description = "Sets PolicyDontAllow=1 in Windows Media Player DRM policy (actually PreventDRMUpdate=1 in the main key). Prevents Windows Media Player from connecting to internet-hosted DRM (Digital Rights Management) licence servers to acquire, update, or backup media playback licences. DRM licence acquisition involves contacting Microsoft PlayReady servers and potentially third-party vendor licence servers based on the media file's licence URL embedded in the WRM header. " +
                "DRM internet connections are an outbound channel that operates based on media file content: a specially crafted WMA/WMV file with a malicious licence acquisition URL will cause WMP to reach out to an attacker-controlled server for licence validation — generating an outbound HTTP request to an external host triggered by opening a media file. This is a data exfiltration vector for leaking internal host information (IP address, Windows Media identifier, user details) to external servers via the licence request header.",
            Tags = ["wmpol", "windows-media-player", "drm", "licence-acquisition", "outbound-dns", "data-exfiltration"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WMP DRM licence internet acquisition blocked. DRM-protected media files that require new licence download will not play. Locally cached licences still usable.",
            ApplyOps = [RegOp.SetDword(Key, "PreventDRMUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventDRMUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "PreventDRMUpdate", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-prevent-media-information-retrieval",
            Label = "WMP: Prevent Automatic Online Media Information Retrieval",
            Category = "Media Player Adv Policy",
            Description = "Sets PreventMediaRetrieval=1 in Windows Media Player policy. Prevents Windows Media Player from automatically sending the names of media files being played to Microsoft's online content service to retrieve album art, track information, lyrics, and related metadata. This retrieval exposes media file names and playing history to Microsoft's servers. " +
                "Automatic media information retrieval sends the track title, artist name, and album to Microsoft's media content service (previously WindowsMedia.com, now Microsoft's CDN) for every media file opened in WMP. In healthcare or legal environments, media files may have confidential file names (patient ID numbers, case numbers, attorney names in video deposition recordings). Transmitting these file names to external servers violates data minimisation principles under GDPR and HIPAA. Disabling retrieval ensures locally held media metadata is not transmitted externally.",
            Tags = ["wmpol", "windows-media-player", "media-metadata", "privacy", "album-art", "gdpr"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WMP does not retrieve online metadata. Album art, track info, and lyrics not downloaded from Microsoft servers. Locally embedded metadata still displayed.",
            ApplyOps = [RegOp.SetDword(Key, "PreventMediaRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventMediaRetrieval")],
            DetectOps = [RegOp.CheckDword(Key, "PreventMediaRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-hide-music-library-tab",
            Label = "WMP: Hide Music Library Tab to Prevent Windows Media Player Library Exposure",
            Category = "Media Player Adv Policy",
            Description = "Sets DisableMusicLibraryTab=1 in Windows Media Player policy. Hides the Music library tab in the WMP Library view, preventing the Windows Media Player library from being browsed by other applications or users via the WMP COM API or shell integration. The WMP library database (containing all indexed media file paths) is accessible via COM to any application with the user's privilege level. " +
                "Windows Media Player maintains an indexed library database of all media files accessible on the system, stored in %LocalAppData%\\Microsoft\\Media Player\\. The library database contains full file paths, playback statistics, and metadata for all media files the user has played. Malware running under the user context can query the WMP COM interface to enumerate all media files, obtaining a list of all file paths in the user's media collection — a comprehensive directory traversal without requiring file system access. Hiding the Music Library tab also removes the WMP library sharing surface area.",
            Tags = ["wmpol", "windows-media-player", "library", "com-api", "privacy", "data-enumeration"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WMP Music Library tab hidden. Does not prevent WMP COM API access but removes the browsable surface. Full library protection requires disabling WMP entirely.",
            ApplyOps = [RegOp.SetDword(Key, "DisableMusicLibraryTab", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMusicLibraryTab")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMusicLibraryTab", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-prevent-desktop-shortcut",
            Label = "WMP: Suppress Windows Media Player Desktop Shortcut Creation",
            Category = "Media Player Adv Policy",
            Description = "Sets PreventDesktopShortcutCreation=1 in Windows Media Player policy. Prevents Windows Media Player from creating or re-creating a desktop shortcut after updates or new user profile setup. On managed enterprise desktops, the shortcut layout is controlled by IT policy and unexpected shortcuts (including WMP shortcuts re-created after each feature update) violate the managed desktop configuration. " +
                "Like the SkyDrive desktop shortcut policy, Windows Media Player has a history of re-creating its desktop shortcut after major Windows Updates, particularly after Media Pack installations in Windows N/KN editions where Media Player is added. Suppressing creation via policy ensures the shortcut stays absent without requiring GPO shortcut deletion scripts.",
            Tags = ["wmpol", "windows-media-player", "desktop-shortcut", "managed-desktop", "enterprise"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "WMP desktop shortcut creation suppressed. WMP still accessible via Start menu and as default media handler. No functional impact.",
            ApplyOps = [RegOp.SetDword(Key, "PreventDesktopShortcutCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventDesktopShortcutCreation")],
            DetectOps = [RegOp.CheckDword(Key, "PreventDesktopShortcutCreation", 1)],
        },
        new TweakDef
        {
            Id = "wmpol-prevent-radio-access",
            Label = "WMP: Disable Internet Radio Access in Windows Media Player",
            Category = "Media Player Adv Policy",
            Description = "Sets DisableRadioBar=1 in Windows Media Player policy. Disables the Windows Media Player internet radio feature and radio bar UI, preventing users from streaming internet radio stations through WMP. Internet radio streaming creates a persistent outbound streaming connection on a potentially high-bandwidth audio stream that bypasses content filtering proxies that only filter HTTP web traffic. " +
                "Internet radio streaming in WMP uses RTSP and HTTP streaming protocols directly to external radio station servers. These connections are not inspected by web content filtering proxies that focus on HTTP page content. A persistent audio stream connection to an external server also creates a long-lived outbound connection that some SIEM rules identify as potential C2 beacon traffic — generating false positive alerts that consume SOC analyst time. Disabling internet radio access eliminates this uninspected outbound streaming channel.",
            Tags = ["wmpol", "windows-media-player", "internet-radio", "streaming", "rtsp", "c2-detection"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WMP internet radio feature disabled. Users cannot stream internet radio via WMP. No impact on local media file playback.",
            ApplyOps = [RegOp.SetDword(Key, "DisableRadioBar", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRadioBar")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRadioBar", 1)],
        },
    ];
}
