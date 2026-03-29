// RegiLattice.Core — Tweaks/GameBarPolicy.cs
// Windows Game Bar, Game Mode, game captures, and Xbox overlay policy — Sprint 513.
// Category: "Game Bar Policy" | Slug: gamebar
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GameBarPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";
    private const string GbKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameBar";
    private const string GmKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameMode";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "gamebar-disable-gamedvr",
            Label        = "Disable Game DVR Background Recording",
            Category     = "Game Bar Policy",
            Description  = "Disables the Game DVR background recording feature that continuously records game footage to disk, freeing GPU encoder time and eliminating the performance overhead of continuous H.264/H.265 encoding in the background.",
            Tags         = ["gamedvr", "recording", "background", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Game DVR background recording disabled; GPU encoder freed, disk writes stopped, game perf overhead removed.",
            ApplyOps     = [RegOp.SetDword(Key, "AllowGameDVR", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AllowGameDVR")],
            DetectOps    = [RegOp.CheckDword(Key, "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id           = "gamebar-disable-gamebar-tips",
            Label        = "Disable Game Bar First-Run Tips and Overlay Prompts",
            Category     = "Game Bar Policy",
            Description  = "Prevents the Game Bar from displaying first-run tips and overlay prompt notifications in full-screen applications, eliminating interruptions during gaming or full-screen media playback.",
            Tags         = ["gamebar", "tips", "overlay", "notifications", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Game Bar tips and overlay prompts disabled; no interruptions during full-screen app or game sessions.",
            ApplyOps     = [RegOp.SetDword(GbKey, "DisableGameBarTips", 1)],
            RemoveOps    = [RegOp.DeleteValue(GbKey, "DisableGameBarTips")],
            DetectOps    = [RegOp.CheckDword(GbKey, "DisableGameBarTips", 1)],
        },
        new TweakDef
        {
            Id           = "gamebar-disable-gamemode",
            Label        = "Disable Windows Game Mode Globally",
            Category     = "Game Bar Policy",
            Description  = "Disables Windows Game Mode which dynamically adjusts CPU/GPU scheduling when a game is in focus. On systems running background services or VMs, Game Mode can disrupt non-game workloads; disabling it provides more predictable scheduling.",
            Tags         = ["gamemode", "scheduler", "cpu", "gpu", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Game Mode disabled; CPU/GPU scheduler not dynamically adjusted when game is focused. More predictable perf.",
            ApplyOps     = [RegOp.SetDword(GmKey, "AutoGameModeEnabled", 0)],
            RemoveOps    = [RegOp.DeleteValue(GmKey, "AutoGameModeEnabled")],
            DetectOps    = [RegOp.CheckDword(GmKey, "AutoGameModeEnabled", 0)],
        },
        new TweakDef
        {
            Id           = "gamebar-disable-xbox-network-check",
            Label        = "Disable Xbox Network Connectivity Check at Game Launch",
            Category     = "Game Bar Policy",
            Description  = "Prevents the Game Bar from performing Xbox Live / Microsoft network connectivity checks at game launch, eliminating network latency at game start and avoiding telemetry associated with Xbox network status probes.",
            Tags         = ["gamebar", "xbox", "network-check", "telemetry", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Xbox Live network check at game launch disabled; no network probe or telemetry on game start.",
            ApplyOps     = [RegOp.SetDword(GbKey, "DisableXboxNetworkCheck", 1)],
            RemoveOps    = [RegOp.DeleteValue(GbKey, "DisableXboxNetworkCheck")],
            DetectOps    = [RegOp.CheckDword(GbKey, "DisableXboxNetworkCheck", 1)],
        },
        new TweakDef
        {
            Id           = "gamebar-disable-broadcast-streaming",
            Label        = "Disable Game Bar Broadcast Streaming to Twitch/Mixer",
            Category     = "Game Bar Policy",
            Description  = "Prevents the Game Bar from offering game broadcast streaming functionality to third-party streaming services, disabling the broadcast API and ensuring game streams cannot be initiated without explicit user action outside the Game Bar.",
            Tags         = ["gamebar", "broadcast", "streaming", "twitch", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Game Bar broadcast streaming disabled; game sessions cannot be streamed via Game Bar broadcast UI.",
            ApplyOps     = [RegOp.SetDword(GbKey, "DisableBroadcasting", 1)],
            RemoveOps    = [RegOp.DeleteValue(GbKey, "DisableBroadcasting")],
            DetectOps    = [RegOp.CheckDword(GbKey, "DisableBroadcasting", 1)],
        },
        new TweakDef
        {
            Id           = "gamebar-disable-screenshot-shortcut",
            Label        = "Disable Game Bar Screenshot Keyboard Shortcut",
            Category     = "Game Bar Policy",
            Description  = "Disables the Win+Alt+PrtSc keyboard shortcut that captures game screenshots via Game Bar, preventing accidental screenshot capture and avoiding screenshots being stored in the GameCapture screenshots folder.",
            Tags         = ["gamebar", "screenshot", "keyboard-shortcut", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Game Bar screenshot hotkey disabled; Win+Alt+PrtSc no longer captures game screenshots.",
            ApplyOps     = [RegOp.SetDword(GbKey, "DisableScreenshotShortcut", 1)],
            RemoveOps    = [RegOp.DeleteValue(GbKey, "DisableScreenshotShortcut")],
            DetectOps    = [RegOp.CheckDword(GbKey, "DisableScreenshotShortcut", 1)],
        },
        new TweakDef
        {
            Id           = "gamebar-disable-gameclips-upload",
            Label        = "Disable Automatic Game Clip Upload to Xbox Cloud",
            Category     = "Game Bar Policy",
            Description  = "Prevents automatically uploading captured game clips and screenshots to Xbox cloud storage, ensuring game captures remain local and are not synchronized to Microsoft cloud accounts without explicit user action.",
            Tags         = ["gamebar", "clips", "cloud-upload", "xbox", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Game clip auto-upload to Xbox cloud disabled; captures stored locally only, not synced to Microsoft cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableCloudUpload", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableCloudUpload")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableCloudUpload", 1)],
        },
        new TweakDef
        {
            Id           = "gamebar-disable-gamebar-telemetry",
            Label        = "Disable Game Bar and GameDVR Telemetry to Microsoft",
            Category     = "Game Bar Policy",
            Description  = "Prevents Game Bar and GameDVR from sending gaming session duration, game title names, capture statistics, and hardware performance data to Microsoft.",
            Tags         = ["gamebar", "gamedvr", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Game Bar and GameDVR telemetry to Microsoft disabled; game session and capture data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableGameBarTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableGameBarTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableGameBarTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "gamebar-set-capture-folder-policy",
            Label        = "Set Game Capture Storage Folder via Policy",
            Category     = "Game Bar Policy",
            Description  = "Configures the Game Bar capture storage location to the local Videos\\GameCaptures path via policy, overriding per-user settings to ensure game recordings are stored to a known, auditable location and not redirected elsewhere.",
            Tags         = ["gamebar", "capture-folder", "storage", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Game capture folder fixed to Videos\\GameCaptures via policy; per-user folder change overridden.",
            ApplyOps     = [RegOp.SetString(Key, "CaptureFolder", @"%USERPROFILE%\Videos\GameCaptures")],
            RemoveOps    = [RegOp.DeleteValue(Key, "CaptureFolder")],
            DetectOps    = [RegOp.CheckString(Key, "CaptureFolder", @"%USERPROFILE%\Videos\GameCaptures")],
        },
        new TweakDef
        {
            Id           = "gamebar-log-capture-events",
            Label        = "Log Game Bar Capture Start/Stop Events",
            Category     = "Game Bar Policy",
            Description  = "Enables event log entries when Game Bar starts or stops a recording or screenshot capture session, providing visibility into screen capture activity for compliance auditing.",
            Tags         = ["gamebar", "event-log", "capture", "audit", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Game Bar capture start/stop events logged; recording sessions visible in System event log for auditing.",
            ApplyOps     = [RegOp.SetDword(Key, "LogCaptureEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LogCaptureEvents")],
            DetectOps    = [RegOp.CheckDword(Key, "LogCaptureEvents", 1)],
        },
    ];
}
