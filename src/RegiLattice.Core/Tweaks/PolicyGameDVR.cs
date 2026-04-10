namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PolicyGameDVR
{
    private const string GameDvrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gdvr-disable-gamedvr",
            Label = "Disable Game DVR (Policy)",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowGameDVR=0 in GameDVR policy. Completely disables the Game DVR feature, "
                + "preventing background game recording, screenshots, and broadcasting. Frees GPU "
                + "and CPU resources that Game DVR reserves for video encoding.",
            Tags = ["game-dvr", "recording", "gpu", "performance", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Game DVR disabled; no background game recording or screenshots via Game Bar.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "AllowGameDVR", 0)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "AllowGameDVR")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "gdvr-disable-game-bar",
            Label = "Disable Game Bar (Policy)",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowGameBar=0 in GameDVR policy. Prevents the Xbox Game Bar overlay from "
                + "appearing when pressing Win+G. Eliminates the overlay's GPU compositor overhead "
                + "and prevents accidental activation during full-screen games.",
            Tags = ["game-bar", "overlay", "xbox", "performance", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Xbox Game Bar overlay disabled; Win+G does nothing.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "AllowGameBar", 0)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "AllowGameBar")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "AllowGameBar", 0)],
        },
        new TweakDef
        {
            Id = "gdvr-disable-broadcasting",
            Label = "Disable Game Broadcasting",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowBroadcasting=0 in GameDVR policy. Prevents live game streaming "
                + "(broadcasting) via the Xbox Game Bar. Reduces bandwidth consumption and prevents "
                + "unintentional screen sharing during gaming sessions.",
            Tags = ["broadcasting", "streaming", "game-dvr", "policy", "privacy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Live game broadcasting disabled; game recording may still work if DVR is on.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "AllowBroadcasting", 0)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "AllowBroadcasting")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "AllowBroadcasting", 0)],
        },
        new TweakDef
        {
            Id = "gdvr-disable-game-mode",
            Label = "Disable Game Mode (Policy)",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowAutoGameMode=0 in GameDVR policy. Disables automatic Game Mode, which "
                + "normally reallocates system resources to the foreground game. On workstations "
                + "running multiple critical background tasks, Game Mode can starve those tasks.",
            Tags = ["game-mode", "performance", "background-tasks", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Auto Game Mode off; background tasks get normal resource allocation.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "AllowAutoGameMode", 0)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "AllowAutoGameMode")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "AllowAutoGameMode", 0)],
        },
        new TweakDef
        {
            Id = "gdvr-max-recording-length",
            Label = "Limit Game Recording to 30 Minutes",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxRecordingLength=30 (minutes) in GameDVR policy. Caps the maximum length "
                + "of a single Game DVR recording session, preventing hours-long recordings from "
                + "filling the disk. Default is unlimited.",
            Tags = ["game-dvr", "recording", "length", "disk-space", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Recordings capped at 30 minutes; longer sessions need manual restart.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "MaxRecordingLength", 30)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "MaxRecordingLength")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "MaxRecordingLength", 30)],
        },
        new TweakDef
        {
            Id = "gdvr-disable-audio-recording",
            Label = "Disable Game Audio Recording",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowAudioRecording=0 in GameDVR policy. Disables microphone and system audio "
                + "capture during game recordings. Prevents accidental recording of voice chat, "
                + "private conversations, or background audio.",
            Tags = ["game-dvr", "audio", "microphone", "privacy", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Game recordings are silent (video only); no audio captured.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "AllowAudioRecording", 0)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "AllowAudioRecording")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "AllowAudioRecording", 0)],
        },
        new TweakDef
        {
            Id = "gdvr-disable-background-recording",
            Label = "Disable Background Game Recording",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowBackgroundRecording=0 in GameDVR policy. Prevents Game DVR from "
                + "continuously recording gameplay in the background (for 'record that' clips). "
                + "Eliminates the constant GPU encoding overhead that background recording imposes.",
            Tags = ["game-dvr", "background", "recording", "gpu", "performance", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "No background recording; 'Record that' instant replay unavailable.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "AllowBackgroundRecording", 0)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "AllowBackgroundRecording")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "AllowBackgroundRecording", 0)],
        },
        new TweakDef
        {
            Id = "gdvr-set-video-quality-standard",
            Label = "Set Game Recording Quality to Standard",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets VideoQuality=1 (Standard) in GameDVR policy. Forces game recordings to use "
                + "standard quality instead of high. Reduces file sizes and GPU encoding load. "
                + "Standard quality is sufficient for most game clip sharing.",
            Tags = ["game-dvr", "quality", "standard", "disk-space", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Game recordings use standard quality; smaller files, less GPU load.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "VideoQuality", 1)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "VideoQuality")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "VideoQuality", 1)],
        },
        new TweakDef
        {
            Id = "gdvr-set-fps-30",
            Label = "Limit Game Recording FPS to 30",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets VideoFPS=30 in GameDVR policy. Caps game recording frame rate to 30 FPS "
                + "instead of 60. Reduces GPU encoding overhead by 50% and cuts recorded file "
                + "sizes roughly in half. Adequate for most game clip sharing.",
            Tags = ["game-dvr", "fps", "30fps", "performance", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Game recordings capped at 30 FPS; smoother in-game performance.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "VideoFPS", 30)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "VideoFPS")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "VideoFPS", 30)],
        },
        new TweakDef
        {
            Id = "gdvr-disable-cursor-capture",
            Label = "Disable Cursor Capture in Recordings",
            Category = "Gaming — Game DVR Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CaptureCursor=0 in GameDVR policy. Excludes the mouse cursor from game "
                + "recordings. Produces cleaner gameplay videos without a visible cursor, "
                + "especially useful for gamepad-centric titles.",
            Tags = ["game-dvr", "cursor", "recording", "video", "policy"],
            RegistryKeys = [GameDvrKey],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Mouse cursor hidden in game recordings; purely cosmetic change.",
            ApplyOps = [RegOp.SetDword(GameDvrKey, "CaptureCursor", 0)],
            RemoveOps = [RegOp.DeleteValue(GameDvrKey, "CaptureCursor")],
            DetectOps = [RegOp.CheckDword(GameDvrKey, "CaptureCursor", 0)],
        },
    ];
}
