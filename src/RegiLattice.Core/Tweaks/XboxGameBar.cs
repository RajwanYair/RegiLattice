// RegiLattice.Core — Tweaks/XboxGameBar.cs
// Xbox Game Bar and GameInput fine-grained controls (Win10 1903+ / Win11).
// Uses slug "xbgb" — does NOT overlap with Gaming.cs (game-) or Debloat.cs (debloat-).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class XboxGameBar
{
    private const string GameBar = @"HKEY_CURRENT_USER\Software\Microsoft\GameBar";
    private const string GameCfg = @"HKEY_CURRENT_USER\System\GameConfigStore";
    private const string XNetProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Gaming\AllowGameDVR";
    private const string XSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services";
    private const string GamebarPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";
    private const string CapturesPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppX";
    private const string XboxGlobal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "xbgb-disable-captures-folder-indexing",
            Label = "Disable Game Bar Captures Folder Indexing",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "performance"],
            Description = "Prevents Windows Search from indexing the Game Bar captures folder, " + "reducing background I/O during gaming sessions.",
            ApplyOps = [RegOp.SetDword(GameBar, "GamebarCaptureFolderIndexingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "GamebarCaptureFolderIndexingEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "GamebarCaptureFolderIndexingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-controller-activation",
            Label = "Disable Game Bar Activation via Controller",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "controller"],
            Description =
                "Prevents pressing the Xbox button on an XInput controller from opening "
                + "the Game Bar overlay, avoiding accidental pop-ups mid-game.",
            ApplyOps = [RegOp.SetDword(GameBar, "ShowStartupPanel", 0), RegOp.SetDword(GameBar, "UseNexusForGameBarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "ShowStartupPanel"), RegOp.SetDword(GameBar, "UseNexusForGameBarEnabled", 1)],
            DetectOps = [RegOp.CheckDword(GameBar, "UseNexusForGameBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-capture-audio",
            Label = "Disable Game Bar Audio Capture",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "audio", "performance"],
            Description = "Disables audio capture in Game Bar recordings to reduce CPU and I/O " + "overhead when recording gameplay.",
            ApplyOps = [RegOp.SetDword(GameBar, "AudioCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "AudioCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "AudioCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-achievement-notifications",
            Label = "Disable Xbox Achievement Notifications",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "notifications"],
            Description = "Suppresses Xbox achievement toast notifications which can break " + "full-screen exclusive games and cause stutter.",
            ApplyOps = [RegOp.SetDword(GameBar, "NotificationsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "NotificationsEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "NotificationsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-game-bar-tips",
            Label = "Disable Game Bar First-Run Tips",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "notifications", "tips"],
            Description = "Hides the first-run tutorial tips pane inside the Game Bar overlay " + "to keep the overlay uncluttered.",
            ApplyOps = [RegOp.SetDword(GameBar, "GameBarTipsEnabled", 0), RegOp.SetDword(GameBar, "FirstTimeExperienceCompleted", 1)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "GameBarTipsEnabled"), RegOp.DeleteValue(GameBar, "FirstTimeExperienceCompleted")],
            DetectOps = [RegOp.CheckDword(GameBar, "GameBarTipsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-game-dvr-policy",
            Label = "Disable Game DVR via Group Policy",
            Category = "Xbox / Game Bar",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["gaming", "xbox", "game-dvr", "policy"],
            Description =
                "Applies the Group Policy that disables Game DVR recording at the "
                + "system level, complementing the per-user GameConfigStore setting.",
            ApplyOps = [RegOp.SetDword(GamebarPol, "AllowGameDVR", 0)],
            RemoveOps = [RegOp.DeleteValue(GamebarPol, "AllowGameDVR")],
            DetectOps = [RegOp.CheckDword(GamebarPol, "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-full-scene-optimizations",
            Label = "Disable Full-Scene Optimizations Globally",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "performance", "fps"],
            Description = "Sets the global Full-Scene Optimization flag off, which can improve " + "frame-time consistency in some DX11/DX12 titles.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameConfigStoreEnable", 1), RegOp.SetDword(GameCfg, "Win32GameDVR_FSEBehaviorMode", 2)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "Win32GameDVR_FSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(GameCfg, "Win32GameDVR_FSEBehaviorMode", 2)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-social-features",
            Label = "Disable Xbox Social Integration",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "privacy", "social"],
            Description = "Disables the Xbox social panel and friends list integration in the " + "Game Bar, removing online presence tracking.",
            ApplyOps = [RegOp.SetDword(GameBar, "SocialEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "SocialEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "SocialEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-premium-badges",
            Label = "Disable Game Bar Premium Feature Badges",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar"],
            Description = "Removes the premium/sponsor badge promotions displayed inside the " + "Game Bar overlay.",
            ApplyOps = [RegOp.SetDword(GameBar, "GameBarPremiumFeaturesEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "GameBarPremiumFeaturesEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "GameBarPremiumFeaturesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-enable-exclusive-fullscreen",
            Label = "Force Exclusive Fullscreen (Disable Optimised FSE Override)",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "performance", "fps", "fullscreen"],
            Description =
                "Restores true exclusive fullscreen for games that have been "
                + "quietly switched to FSO (Fullscreen Optimisation) mode by Windows, "
                + "which can lower latency in some GPU + monitor combinations.",
            ApplyOps = [RegOp.SetDword(GameCfg, "Win32GameDVR_EFSEBehavior", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "Win32GameDVR_EFSEBehavior")],
            DetectOps = [RegOp.CheckDword(GameCfg, "Win32GameDVR_EFSEBehavior", 0)],
        },
    ];
}
