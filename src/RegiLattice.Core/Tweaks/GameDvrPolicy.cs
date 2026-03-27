// GameDvrPolicy.cs — Game DVR, Game Bar, and game overlay enforcement
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR
// Slug: gamedvr
// Category: Game DVR Policy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GameDvrPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gamedvr-disable-all",
            Label = "Game DVR Policy: Disable Game DVR Recording",
            Category = "Game DVR Policy",
            Description =
                "Disables Windows Game DVR (Game Digital Video Recording) via Group Policy. "
                + "Game DVR continuously captures gameplay footage in the background, consuming CPU, GPU, RAM, and disk resources even when the user is not actively recording. "
                + "On non-gaming workstations this is pure overhead with no benefit. "
                + "Removing this policy re-enables Game DVR recording capability.",
            Tags = ["gamedvr", "recording", "game-bar", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowGameDVR", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowGameDVR")],
            DetectOps = [RegOp.CheckDword(Key, "AllowGameDVR", 0)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Disables Game DVR background recording; recovers continuous background resource overhead.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-game-bar",
            Label = "Game DVR Policy: Disable Xbox Game Bar",
            Category = "Game DVR Policy",
            Description =
                "Disables the Xbox Game Bar overlay via Group Policy, preventing it from launching via Win+G or game launch hooks. "
                + "Game Bar is a WinRT overlay that injects into render pipelines and can cause micro-stutters and frame drops in some titles. "
                + "On enterprise workstations its recording and social features are inappropriate and add unnecessary attack surface. "
                + "Removing this policy re-enables the Game Bar overlay.",
            Tags = ["gamedvr", "game-bar", "overlay", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGameBar", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGameBar")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGameBar", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes Win+G Game Bar; eliminates overlay injection that can cause frame pacing issues.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-background-recording",
            Label = "Game DVR Policy: Disable Background Video Recording",
            Category = "Game DVR Policy",
            Description =
                "Disables Game DVR's background clip recording feature that captures the last N minutes of gameplay continuously. "
                + "This feature allocates a rolling video buffer on disk and in RAM at all times, degrading performance of high-load applications. "
                + "Disabling it via policy prevents users or MSI installers from re-enabling it. "
                + "Removing this policy allows background recording to be re-enabled.",
            Tags = ["gamedvr", "background-recording", "background", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowBackgroundRecording", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowBackgroundRecording")],
            DetectOps = [RegOp.CheckDword(Key, "AllowBackgroundRecording", 0)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes rolling background clip buffer; reclaims GPU encoder cycles and disk I/O bandwidth.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-broadcast",
            Label = "Game DVR Policy: Disable Game Broadcasting",
            Category = "Game DVR Policy",
            Description =
                "Prohibits live broadcasting of gameplay via services such as Mixer (retired) or any future broadcasting back-end. "
                + "Broadcasting continually encodes and streams video, consuming significant bandwidth and GPU encoder capacity. "
                + "On enterprise networks, live broadcasting is a data exfiltration risk and a bandwidth hog. "
                + "Removing this policy re-enables broadcasting capability.",
            Tags = ["gamedvr", "broadcast", "streaming", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowBroadcast", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowBroadcast")],
            DetectOps = [RegOp.CheckDword(Key, "AllowBroadcast", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks live game broadcasting; removes bandwidth and GPU encoder overhead.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-game-mode",
            Label = "Game DVR Policy: Disable Automatic Game Mode",
            Category = "Game DVR Policy",
            Description =
                "Prevents Windows from automatically activating Game Mode when a game is detected. "
                + "Game Mode reprioritises threads and may cause stutter in other processes sharing the CPU, including audio and network services. "
                + "On workstations running mixed workloads, fixed CPU scheduling policy is more predictable than automatic game detection. "
                + "Removing this policy allows Windows to auto-enable Game Mode for detected games.",
            Tags = ["gamedvr", "game-mode", "scheduling", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowAutoGameMode", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAutoGameMode")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAutoGameMode", 0)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents auto Game Mode; avoids CPU scheduling disruption on mixed-workload machines.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-feedback-button",
            Label = "Game DVR Policy: Disable Game Bar Feedback Button",
            Category = "Game DVR Policy",
            Description =
                "Removes the feedback button from the Game Bar overlay that prompts users to submit feedback about gaming performance to Microsoft. "
                + "On managed systems, direct feedback telemetry paths should be controlled centrally rather than through individual user submissions. "
                + "This also removes one entry point for initiating corporate network connections from game sessions. "
                + "Removing this policy restores the feedback button in the Game Bar.",
            Tags = ["gamedvr", "feedback", "telemetry", "game-bar", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGameBarFeedbackButton", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGameBarFeedbackButton")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGameBarFeedbackButton", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Removes feedback button from Game Bar; blocks one Microsoft telemetry submission path.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-coach-tips",
            Label = "Game DVR Policy: Disable Game Bar Coach Tips",
            Category = "Game DVR Policy",
            Description =
                "Suppresses the 'coach' overlay tips that appear over games when Game Bar is active, prompting users to use recording and features. "
                + "Coach tips are intrusive and distract from productive workflows when applications are mistakenly classified as games. "
                + "Removing this policy restores the Game Bar coach tip overlays.",
            Tags = ["gamedvr", "coach", "tips", "ux", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGameBarCoach", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGameBarCoach")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGameBarCoach", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hides Game Bar coach tips; no more intrusive recording prompts over full-screen apps.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-game-overlay",
            Label = "Game DVR Policy: Disable Game Bar On-Screen Overlay",
            Category = "Game DVR Policy",
            Description =
                "Disables the Game Bar on-screen performance and stats overlay that can appear in full-screen applications. "
                + "The overlay periodically injects into the application's render surface and can introduce frame time spikes. "
                + "On non-gaming workstations the overlay provides no value and adds rendering overhead. "
                + "Removing this policy re-enables the Game Bar overlay capability.",
            Tags = ["gamedvr", "overlay", "rendering", "performance", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGameOverlay", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGameOverlay")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGameOverlay", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Removes GPU overlay rendering; eliminates potential frame-time spikes from overlay injection.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-handheld",
            Label = "Game DVR Policy: Disable Handheld Console DVR Support",
            Category = "Game DVR Policy",
            Description =
                "Disables Game DVR recording support for Windows handheld gaming devices via the AllowHandheld policy flag. "
                + "On desktop and laptop enterprise machines this capability is irrelevant and its associated services consume memory at startup. "
                + "Disabling via policy prevents peripheral detection code from loading the DVR subsystem on non-handheld hardware. "
                + "Removing this policy re-enables handheld DVR support.",
            Tags = ["gamedvr", "handheld", "gaming-device", "service", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowHandheld", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowHandheld")],
            DetectOps = [RegOp.CheckDword(Key, "AllowHandheld", 0)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks handheld DVR subsystem loading on desktop/laptop hardware; minor memory saving.",
        },
        new TweakDef
        {
            Id = "gamedvr-disable-social-features",
            Label = "Game DVR Policy: Disable Game Social Features",
            Category = "Game DVR Policy",
            Description =
                "Disables Xbox social features integration in the Game Bar that show friends, activity feeds, and achievements. "
                + "Social features require persistent background network connections to Xbox Live services, adding ongoing network traffic and CPU wake events. "
                + "On enterprise systems, Xbox Live social connectivity is inappropriate and a data governance concern. "
                + "Removing this policy re-enables Xbox social integration in the Game Bar.",
            Tags = ["gamedvr", "social", "xbox-live", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowSocialFeatures", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSocialFeatures")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSocialFeatures", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disconnects Xbox Live social in Game Bar; removes background network wake events.",
        },
    ];
}
