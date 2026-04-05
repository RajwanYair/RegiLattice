namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// RegiLattice.Core — Tweaks/PolicyMediaPlayer.cs
// Windows Media Player privacy, network, codec, and DRM Group Policy tweaks.
// Category: "Multimedia"
// Sprint 672 (v6.11.0)

internal static class PolicyMediaPlayer
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _WmpPolicy.Data,
        ];

    // ── Sprint 672 — Windows Media Player Policy ──────────────────────────────
    private static class _WmpPolicy
    {
        private const string WmpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "media-policy-disable-first-run",
                    Label = "Disable Windows Media Player First-Run Setup Wizard",
                    Category = "Multimedia",
                    Description =
                        "Prevents Windows Media Player from showing the first-run setup wizard that asks users to configure privacy, codec download, and CodecLink options. The wizard can silently enable online data sharing. Default: first-run wizard shown on launch. Recommended: disabled.",
                    Tags = ["wmp", "media-player", "setup", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "GroupPrivacyAcceptance", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "GroupPrivacyAcceptance")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "GroupPrivacyAcceptance", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-codec-download",
                    Label = "Disable Automatic Codec Download in Media Player",
                    Category = "Multimedia",
                    Description =
                        "Prevents Windows Media Player from automatically downloading codecs from the internet when it encounters unrecognised media formats. Automatic codec downloads can install untrusted third-party binary components. Default: auto-download allowed. Recommended: disabled.",
                    Tags = ["wmp", "codec", "download", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "DisableAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "DisableAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "DisableAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-mms-protocol",
                    Label = "Disable MMS Streaming Protocol in Media Player",
                    Category = "Multimedia",
                    Description =
                        "Blocks Windows Media Player from using the legacy MMS (Microsoft Media Server) streaming protocol. MMS uses unauthenticated UDP/TCP streams and is deprecated; blocking it reduces the network attack surface. Default: MMS protocol allowed. Recommended: disabled.",
                    Tags = ["wmp", "mms", "streaming", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventMMSProtocol", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventMMSProtocol")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventMMSProtocol", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-library-sharing",
                    Label = "Disable Media Library Sharing (DLNA/UPnP) in Media Player",
                    Category = "Multimedia",
                    Description =
                        "Prevents Windows Media Player from sharing its media library over the local network via DLNA/UPnP. Library sharing can expose media file metadata and content to all devices on the network segment. Default: sharing allowed. Recommended: disabled for privacy.",
                    Tags = ["wmp", "dlna", "upnp", "sharing", "network", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventLibrarySharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventLibrarySharing")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventLibrarySharing", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-drm-online",
                    Label = "Disable Online DRM Licence Acquisition in Media Player",
                    Category = "Multimedia",
                    Description =
                        "Prevents Windows Media Player from contacting Microsoft's DRM licensing servers to acquire playback licences for protected content. Online licence acquisition sends media metadata and hardware fingerprint data to Microsoft. Default: online licence acquisition enabled. Recommended: disabled.",
                    Tags = ["wmp", "drm", "licence", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventDRMLicenseAcquisition", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventDRMLicenseAcquisition")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventDRMLicenseAcquisition", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-metatitle-retrieval",
                    Label = "Disable Online CD/DVD Metadata Title Retrieval",
                    Category = "Multimedia",
                    Description =
                        "Stops Windows Media Player from querying Microsoft's online metadata service to retrieve album art, track titles, and disc information when a CD or DVD is inserted. Queries send media identifiers and hardware IDs to Microsoft servers. Default: online retrieval enabled. Recommended: disabled.",
                    Tags = ["wmp", "metadata", "cddb", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventCDDVDMetadataRetrieval", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventCDDVDMetadataRetrieval")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventCDDVDMetadataRetrieval", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-music-retrieval",
                    Label = "Disable Online Music Metadata Retrieval in Media Player",
                    Category = "Multimedia",
                    Description =
                        "Prevents Windows Media Player from querying internet music databases for song metadata (artwork, lyrics, ratings). Disabling stops media identifiers from being sent to third-party metadata providers via the Media Player pipeline. Default: metadata retrieval enabled. Recommended: disabled.",
                    Tags = ["wmp", "metadata", "music", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventMusicFileMetadataRetrieval", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventMusicFileMetadataRetrieval")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventMusicFileMetadataRetrieval", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-radio-ui",
                    Label = "Hide Windows Media Player Radio UI",
                    Category = "Multimedia",
                    Description =
                        "Hides the Radio feature in Windows Media Player, which streams internet radio content via Microsoft's WindowsMedia.com service. The Radio UI includes usage tracking and content recommendations. Default: Radio UI visible. Recommended: hidden.",
                    Tags = ["wmp", "radio", "streaming", "privacy", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "PreventRadioPresence", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "PreventRadioPresence")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "PreventRadioPresence", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-disable-network-buffering",
                    Label = "Disable Predictive Network Buffering in Media Player",
                    Category = "Multimedia",
                    Description =
                        "Disables the predictive network buffering feature that pre-fetches additional stream data based on playback patterns. Pre-fetch behaviour creates passive network chatter that can be used for traffic fingerprinting of media consumption. Default: enabled. Recommended: disabled for privacy.",
                    Tags = ["wmp", "buffering", "network", "privacy", "streaming", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "DisableNetworkSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "DisableNetworkSettings")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "DisableNetworkSettings", 1)],
                },
                new TweakDef
                {
                    Id = "media-policy-hide-privacy-tab",
                    Label = "Lock Media Player Privacy Settings via Policy",
                    Category = "Multimedia",
                    Description =
                        "Hides the Privacy tab in Windows Media Player Options, preventing users from changing privacy settings (codec download, metadata retrieval, usage reporting). Used together with the other WMP policy tweaks to lock a hardened configuration in place. Default: Privacy tab accessible. Recommended: hidden after hardening.",
                    Tags = ["wmp", "privacy", "settings", "lockdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(WmpKey, "HidePrivacyTab", 1)],
                    RemoveOps = [RegOp.DeleteValue(WmpKey, "HidePrivacyTab")],
                    DetectOps = [RegOp.CheckDword(WmpKey, "HidePrivacyTab", 1)],
                },
            ];
    }
}
