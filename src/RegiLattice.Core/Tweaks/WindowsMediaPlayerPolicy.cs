// WindowsMediaPlayerPolicy.cs — Windows Media Player enterprise policies
// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer
// Slug: wmplay
// Category: Windows Media Player Policy

namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class WindowsMediaPlayerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wmplay-disable-auto-codec-download",
            Label = "Windows Media Player: Disable Automatic Codec Download",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents Windows Media Player from automatically downloading codecs from the Internet when a media file requires one. "
                + "Automatic codec download can introduce unsigned or malicious codec software that runs in a privileged context. "
                + "On managed endpoints codecs should be deployed via the software management tool, not pulled from Internet sources at runtime. "
                + "Removing this policy re-enables automatic codec download when WMP encounters an unsupported format.",
            Tags = ["media-player", "codec", "download", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventCodecDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventCodecDownload")],
            DetectOps = [RegOp.CheckDword(Key, "PreventCodecDownload", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks runtime codec downloads; prevents unsigned codec execution from Internet sources.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-network-settings-change",
            Label = "Windows Media Player: Disable Network Settings Changes",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents WMP users from modifying network configuration in the Windows Media Player settings dialog. "
                + "Network settings in WMP include proxy configuration, streaming protocol selection, and bandwidth limits. "
                + "On managed endpoints these settings should be centrally controlled to ensure network traffic complies with organizational policies. "
                + "Removing this policy re-enables user ability to change WMP network settings.",
            Tags = ["media-player", "network", "settings", "lockdown", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetworkSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkSettings")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetworkSettings", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents WMP network config changes; keeps media streaming under organizational control.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-auto-update-check",
            Label = "Windows Media Player: Disable Automatic Update Checking",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents Windows Media Player from automatically checking for updates on the Internet. "
                + "Automatic update checks for WMP can generate unexpected outbound traffic to Microsoft update servers. "
                + "Updates should be managed through WSUS, SCCM, or Intune rather than individual application self-update mechanisms. "
                + "Removing this policy re-enables WMP's automatic update check on application launch.",
            Tags = ["media-player", "auto-update", "bandwidth", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventAutoUpdate")],
            DetectOps = [RegOp.CheckDword(Key, "PreventAutoUpdate", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables WMP self-update checks; consolidates media player updates through WSUS.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-internet-streaming",
            Label = "Windows Media Player: Disable Internet Media Streaming",
            Category = "Windows Media Player Policy",
            Description =
                "Restricts Windows Media Player from streaming media content from Internet URLs. "
                + "Allowing arbitrary Internet streaming can consume significant bandwidth and may result in access to unlicensed or inappropriate content. "
                + "On corporate networks, media streaming should be restricted to internal or approved sources only. "
                + "Removing this policy re-enables Internet-based media streaming in WMP.",
            Tags = ["media-player", "streaming", "internet", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventMediaSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventMediaSharing")],
            DetectOps = [RegOp.CheckDword(Key, "PreventMediaSharing", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents WMP Internet streaming; conserves bandwidth and blocks unapproved content.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-digital-rights-management",
            Label = "Windows Media Player: Disable DRM License Acquisition from Internet",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents Windows Media Player from automatically acquiring DRM (Digital Rights Management) licenses from the Internet. "
                + "Automatic DRM license acquisition initiates outbound connections to third-party license servers without explicit user consent. "
                + "On managed endpoints, DRM license acquisition should be user-confirmed or blocked entirely. "
                + "Removing this policy re-enables automatic DRM license acquisition when protected media files are opened.",
            Tags = ["media-player", "drm", "licensing", "internet", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventDRMacquisition", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventDRMacquisition")],
            DetectOps = [RegOp.CheckDword(Key, "PreventDRMacquisition", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks DRM auto-acquisition; prevents silent outbound connections to license servers.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-library-sharing",
            Label = "Windows Media Player: Disable Media Library Sharing",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents Windows Media Player from sharing its media library on the local network through the Windows Media Network Sharing service. "
                + "WMP library sharing exposes media file metadata and content over the network, which may include corporate training materials, meeting recordings, or other sensitive files. "
                + "Disabling library sharing reduces lateral movement attack surface from media sharing protocols. "
                + "Removing this policy allows WMP library sharing to be enabled by users.",
            Tags = ["media-player", "library", "sharing", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventLibrarySharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventLibrarySharing")],
            DetectOps = [RegOp.CheckDword(Key, "PreventLibrarySharing", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks WMP library sharing; reduces network attack surface from media sharing protocols.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-media-information-online",
            Label = "Windows Media Player: Disable Online Media Information Retrieval",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents WMP from connecting to the Internet to retrieve album artwork, track information, and music metadata. "
                + "Online metadata requests reveal what media files are being played to Microsoft or third-party data providers. "
                + "This is a privacy risk on endpoints where users play internal audio recordings or video files. "
                + "Removing this policy re-enables online media information lookup in WMP.",
            Tags = ["media-player", "metadata", "privacy", "internet", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMusicMetadata", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMusicMetadata")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMusicMetadata", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks online metadata lookup; prevents media file usage disclosure to third parties.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-usage-reporting",
            Label = "Windows Media Player: Disable Usage Reporting",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents Windows Media Player from sending usage reports and playback data to Microsoft. "
                + "WMP usage reporting transmits data about media formats played, codecs used, and playback errors. "
                + "On enterprise endpoints, this constitutes unnecessary telemetry that should be disabled in line with data minimization policies. "
                + "Removing this policy re-enables WMP usage reporting to Microsoft.",
            Tags = ["media-player", "telemetry", "usage-reporting", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventCDDVDMetadataRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventCDDVDMetadataRetrieval")],
            DetectOps = [RegOp.CheckDword(Key, "PreventCDDVDMetadataRetrieval", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables WMP telemetry and CD/DVD metadata requests; reduces media usage reporting.",
        },
        new TweakDef
        {
            Id = "wmplay-disable-remote-skin-download",
            Label = "Windows Media Player: Disable Remote Skin and Visualizer Download",
            Category = "Windows Media Player Policy",
            Description =
                "Prevents Windows Media Player from downloading skins, visualizations, and plug-in content from the Internet. "
                + "Remote skin and plug-in downloads represent an arbitrary code execution risk if the download source is compromised or spoofed. "
                + "On managed endpoints, WMP customization content should come only from the software management catalog. "
                + "Removing this policy re-enables remote skin and visualizer downloads from Microsoft.",
            Tags = ["media-player", "skin", "plugin", "download", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PreventRadioPresetsRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PreventRadioPresetsRetrieval")],
            DetectOps = [RegOp.CheckDword(Key, "PreventRadioPresetsRetrieval", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks WMP remote content downloads; prevents unofficial plugins and skins from executing.",
        },
        new TweakDef
        {
            Id = "wmplay-hide-privacy-tab",
            Label = "Windows Media Player: Hide Privacy Tab in Options",
            Category = "Windows Media Player Policy",
            Description =
                "Removes the Privacy tab from the Windows Media Player Options dialog. "
                + "The Privacy tab allows users to modify privacy settings including DRM, usage reporting, and online content lookups. "
                + "When privacy settings are centrally locked via GPO, hiding the tab prevents users from attempting to change managed settings. "
                + "Removing this policy restores the Privacy tab in WMP Options.",
            Tags = ["media-player", "privacy", "options-tab", "lockdown", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HidePrivacyTab", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HidePrivacyTab")],
            DetectOps = [RegOp.CheckDword(Key, "HidePrivacyTab", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides WMP Privacy tab; prevents users from modifying centrally managed privacy settings.",
        },
    ];
}
