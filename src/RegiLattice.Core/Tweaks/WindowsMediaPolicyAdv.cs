// Windows Media Player Advanced Policy — Sprint 143
// Slug "wmply" — controls Windows Media Player network, metadata, and update behaviour
// via Group Policy registry settings distinct from the runtime tweaks in Multimedia.cs.
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer  (machine scope)
// HKEY_CURRENT_USER\Software\Policies\Microsoft\WindowsMediaPlayer   (user scope)
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class WindowsMediaPolicyAdv
{
    private const string WmpLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer";
    private const string WmpCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\WindowsMediaPlayer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wmply-no-screensaver",
            Label = "WMP: Disable screensaver activation during audio playback",
            Category = "Windows Media Player Policy",
            Description =
                "Sets AllowScreenSaver=0 in the Windows Media Player policy key. Prevents the "
                + "screensaver from activating while WMP is playing audio, even when the screen is idle.",
            Tags = ["media", "wmp", "screensaver", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpLm, "AllowScreenSaver", 0)],
            RemoveOps = [RegOp.DeleteValue(WmpLm, "AllowScreenSaver")],
            DetectOps = [RegOp.CheckDword(WmpLm, "AllowScreenSaver", 0)],
        },
        new TweakDef
        {
            Id = "wmply-no-cd-metadata",
            Label = "WMP: Prevent CD/DVD metadata retrieval from the internet",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventCDDVDMetadataRetrieval=1. Stops Windows Media Player from contacting "
                + "online databases to retrieve CD/DVD album art, track names, and other metadata.",
            Tags = ["media", "wmp", "metadata", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpLm, "PreventCDDVDMetadataRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventCDDVDMetadataRetrieval")],
            DetectOps = [RegOp.CheckDword(WmpLm, "PreventCDDVDMetadataRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "wmply-no-music-metadata",
            Label = "WMP: Prevent music file metadata retrieval from the internet",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventMusicFileMetadataRetrieval=1. Blocks WMP from downloading online "
                + "metadata for music files such as album art, artist info, and lyrics.",
            Tags = ["media", "wmp", "metadata", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpLm, "PreventMusicFileMetadataRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventMusicFileMetadataRetrieval")],
            DetectOps = [RegOp.CheckDword(WmpLm, "PreventMusicFileMetadataRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "wmply-no-radio-presets",
            Label = "WMP: Prevent internet radio preset retrieval",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventRadioPresetsRetrieval=1. Prevents Windows Media Player from downloading "
                + "internet radio station presets from online services.",
            Tags = ["media", "wmp", "radio", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpLm, "PreventRadioPresetsRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventRadioPresetsRetrieval")],
            DetectOps = [RegOp.CheckDword(WmpLm, "PreventRadioPresetsRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "wmply-no-auto-update",
            Label = "WMP: Disable automatic Windows Media Player updates",
            Category = "Windows Media Player Policy",
            Description =
                "Sets DisableAutoUpdate=1. Prevents Windows Media Player from automatically checking "
                + "for and downloading software updates in the background.",
            Tags = ["media", "wmp", "update", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpLm, "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpLm, "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(WmpLm, "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "wmply-no-codec-download",
            Label = "WMP: Prevent automatic codec download",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventCodecDownload=1. Stops Windows Media Player from automatically "
                + "downloading codecs needed to play unsupported media formats.",
            Tags = ["media", "wmp", "codec", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpLm, "PreventCodecDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventCodecDownload")],
            DetectOps = [RegOp.CheckDword(WmpLm, "PreventCodecDownload", 1)],
        },
        new TweakDef
        {
            Id = "wmply-no-network-protocol-download",
            Label = "WMP: Prevent automatic network protocol download",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventNetworkProtocolAutomaticDownload=1. Prevents Windows Media Player from "
                + "automatically downloading streaming network protocol components.",
            Tags = ["media", "wmp", "network", "protocol", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpLm, "PreventNetworkProtocolAutomaticDownload", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpLm, "PreventNetworkProtocolAutomaticDownload")],
            DetectOps = [RegOp.CheckDword(WmpLm, "PreventNetworkProtocolAutomaticDownload", 1)],
        },
        new TweakDef
        {
            Id = "wmply-user-no-cd-metadata",
            Label = "WMP (user): Prevent CD/DVD metadata retrieval per user",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventCDDVDMetadataRetrieval=1 at the per-user policy scope (HKCU). Enforces "
                + "no-internet-metadata policy for the current user regardless of machine policy.",
            Tags = ["media", "wmp", "metadata", "privacy", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpCu, "PreventCDDVDMetadataRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventCDDVDMetadataRetrieval")],
            DetectOps = [RegOp.CheckDword(WmpCu, "PreventCDDVDMetadataRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "wmply-user-no-music-metadata",
            Label = "WMP (user): Prevent music metadata retrieval per user",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventMusicFileMetadataRetrieval=1 at the per-user policy scope (HKCU). "
                + "Stops the current user's WMP session from downloading online music metadata.",
            Tags = ["media", "wmp", "metadata", "privacy", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpCu, "PreventMusicFileMetadataRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventMusicFileMetadataRetrieval")],
            DetectOps = [RegOp.CheckDword(WmpCu, "PreventMusicFileMetadataRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "wmply-user-no-radio-presets",
            Label = "WMP (user): Prevent internet radio presets per user",
            Category = "Windows Media Player Policy",
            Description =
                "Sets PreventRadioPresetsRetrieval=1 at the per-user policy scope (HKCU). Prevents "
                + "the current user's WMP from fetching online radio station preset lists.",
            Tags = ["media", "wmp", "radio", "privacy", "policy"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(WmpCu, "PreventRadioPresetsRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(WmpCu, "PreventRadioPresetsRetrieval")],
            DetectOps = [RegOp.CheckDword(WmpCu, "PreventRadioPresetsRetrieval", 1)],
        },
    ];
}
