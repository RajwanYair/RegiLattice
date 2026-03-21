// RegiLattice.Core — Tweaks/XboxGameBar.cs
// Xbox Game Bar and GameInput fine-grained controls (Win10 1903+ / Win11).
// Uses slug "xbgb" — does NOT overlap with Gaming.cs (game-) or Debloat.cs (debloat-).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class XboxGameBar
{
    private const string GameBar = @"HKEY_CURRENT_USER\Software\Microsoft\GameBar";
    private const string GameCfg = @"HKEY_CURRENT_USER\System\GameConfigStore";
    private const string AppCapturePol = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR";
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
        new TweakDef
        {
            Id = "xbgb-disable-app-capture",
            Label = "Disable Game DVR App Capture",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "privacy", "performance"],
            Description =
                "Disables the background application capture that the Game DVR driver uses "
                + "to record gameplay footage on demand. Reduces CPU/GPU overhead even when "
                + "Game Bar itself is disabled at the policy level.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "AppCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "AppCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "AppCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-cursor-in-capture",
            Label = "Exclude Cursor from Game Captures",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "cursor"],
            Description =
                "Prevents the mouse cursor from being included in Game DVR recordings " + "and screenshots, producing cleaner gameplay footage.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "CursorCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "CursorCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "CursorCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-mic-in-capture",
            Label = "Disable Microphone in Game DVR Captures",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "microphone", "audio", "privacy"],
            Description =
                "Mutes microphone input from Game DVR recordings so voice chat or " + "background noise is not embedded into saved clips by default.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "MicrophoneCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "MicrophoneCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "MicrophoneCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-audio-capture",
            Label = "Disable Audio Capture in Game DVR Recordings",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "audio"],
            Description =
                "Turns off background audio capture in Game DVR recordings, including "
                + "system audio. Useful when you record silently or handle audio in post.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "AudioCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "AudioCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "AudioCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-fse-hook",
            Label = "Disable Game DVR Fullscreen Exclusivity Hook",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "fullscreen", "fse", "performance"],
            Description =
                "Prevents Game DVR from hooking into the fullscreen exclusive "
                + "(FSE) path used by legacy DirectX games, eliminating a source "
                + "of frame-rate stutters on older titles.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameDVR_HonorUserFSEBehaviorMode", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "GameDVR_HonorUserFSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(GameCfg, "GameDVR_HonorUserFSEBehaviorMode", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-dxgi-fse-compat",
            Label = "Disable Game DVR DXGI Fullscreen Compatibility Mode",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "fullscreen", "dxgi", "performance"],
            Description =
                "Stops Game DVR from forcing DXGI-based games into its FSE "
                + "compatibility wrapper, which can cap frame rates and introduce "
                + "latency on DX11/DX12 titles.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameDVR_DXGIHonorFSEWindowsCompatible", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "GameDVR_DXGIHonorFSEWindowsCompatible")],
            DetectOps = [RegOp.CheckDword(GameCfg, "GameDVR_DXGIHonorFSEWindowsCompatible", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-startup-panel",
            Label = "Disable Game Bar Startup Tip Panel",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "startup"],
            Description =
                "Hides the introductory tip panel that appears the first time " + "Game Bar is opened after an OS update or new installation.",
            ApplyOps = [RegOp.SetDword(GameBar, "ShowStartupPanel", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "ShowStartupPanel")],
            DetectOps = [RegOp.CheckDword(GameBar, "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-nexus-bar",
            Label = "Disable Game Bar Nexus Pop-up Panel",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "nexus", "ui"],
            Description =
                "Disables the Nexus (circular game-bar widget) pop-up panel that "
                + "appears at the edge of the screen when a game is detected, "
                + "removing a persistent UI intrusion during gameplay.",
            ApplyOps = [RegOp.SetDword(GameBar, "UseNexusForGameBarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "UseNexusForGameBarEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "UseNexusForGameBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-enable-game-mode-all-games",
            Label = "Allow Game Mode for All Games (Enable AllowAutoGameMode)",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game mode", "performance"],
            Description =
                "Sets the Game Bar flag that allows Windows Game Mode to "
                + "auto-activate for any process that registers as a game, "
                + "giving it CPU/GPU scheduling priority.",
            ApplyOps = [RegOp.SetDword(GameBar, "AllowAutoGameMode", 1)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "AllowAutoGameMode")],
            DetectOps = [RegOp.CheckDword(GameBar, "AllowAutoGameMode", 1)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-efs-feature-hooks",
            Label = "Disable Game DVR Extended FSE Feature Flags",
            Category = "Xbox / Game Bar",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "fse", "fullscreen", "performance"],
            Description =
                "Zeroes the extended fullscreen-exclusive feature flags "
                + "(EFSEFeatureFlags) used by Game DVR hooks, preventing "
                + "unwanted interference with exclusive-fullscreen rendering paths.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameDVR_EFSEFeatureFlags", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "GameDVR_EFSEFeatureFlags")],
            DetectOps = [RegOp.CheckDword(GameCfg, "GameDVR_EFSEFeatureFlags", 0)],
        },
    ];
}
