// GamingAudioOpt.cs — gaming-specific audio latency and quality tweaks
// Category: Audio  |  IDs: gamaudio-*  |  10 tweaks

#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class GamingAudioOpt
{
    private const string MmcssKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games";
    private const string AudioKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio";
    private const string SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Audiosrv";
    private const string DevKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e96c-e325-11ce-bfc1-08002be10318}\0000";
    private const string SpatialKey =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\spatialAudio";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "gamaudio-mmcss-games-priority",
            Label = "Set MMCSS Games Task Priority to Highest",
            Category = "Audio",
            Description = "Raises the MMCSS scheduling priority for the Games task to ensure game threads receive maximum audio scheduler time.",
            Tags = ["audio", "gaming", "mmcss", "latency", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Reduces audio stutter in games by raising the MMCSS Games task priority.",
            ApplyOps = [RegOp.SetDword(MmcssKey, "Priority", 6)],
            RemoveOps = [RegOp.SetDword(MmcssKey, "Priority", 2)],
            DetectOps = [RegOp.CheckDword(MmcssKey, "Priority", 6)],
        },
        new TweakDef
        {
            Id = "gamaudio-mmcss-games-backoff",
            Label = "Set MMCSS Games Task Background Only = False",
            Category = "Audio",
            Description =
                "Disables the background-only flag for the MMCSS Games task so the scheduler applies game task priority to foreground audio threads.",
            Tags = ["audio", "gaming", "mmcss", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Allows MMCSS Games task priority to apply when the game is in the foreground.",
            ApplyOps = [RegOp.SetDword(MmcssKey, "Background Only", 0)],
            RemoveOps = [RegOp.DeleteValue(MmcssKey, "Background Only")],
            DetectOps = [RegOp.CheckDword(MmcssKey, "Background Only", 0)],
        },
        new TweakDef
        {
            Id = "gamaudio-mmcss-proaudio-priority",
            Label = "Set MMCSS Pro Audio Task Priority to 1",
            Category = "Audio",
            Description = "Sets the Pro Audio MMCSS task priority to 1 (highest), reducing audio buffer underruns during gaming.",
            Tags = ["audio", "gaming", "mmcss", "latency", "pro-audio"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Minimises buffer underruns and audio dropouts for low-latency gaming audio.",
            ApplyOps = [RegOp.SetDword(AudioKey, "Priority", 1)],
            RemoveOps = [RegOp.SetDword(AudioKey, "Priority", 2)],
            DetectOps = [RegOp.CheckDword(AudioKey, "Priority", 1)],
        },
        new TweakDef
        {
            Id = "gamaudio-mmcss-proaudio-sfio",
            Label = "Raise MMCSS Pro Audio SFIO Priority",
            Category = "Audio",
            Description =
                "Sets SFIO (Scheduled File I/O) priority for the Pro Audio MMCSS task to High so audio I/O is not delayed by disk activity.",
            Tags = ["audio", "gaming", "mmcss", "sfio", "latency"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents disk I/O from delaying audio I/O, reducing crackle under load.",
            ApplyOps = [RegOp.SetString(AudioKey, "SFIO Priority", "High")],
            RemoveOps = [RegOp.SetString(AudioKey, "SFIO Priority", "Normal")],
            DetectOps = [RegOp.CheckString(AudioKey, "SFIO Priority", "High")],
        },
        new TweakDef
        {
            Id = "gamaudio-audiosrv-start-demand",
            Label = "Set Audio Service Start to Auto (Delayed)",
            Category = "Audio",
            Description =
                "Ensures the Windows Audio service (Audiosrv) starts automatically with delayed start, preventing it from being stopped by power or game optimisers.",
            Tags = ["audio", "gaming", "service"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures audio service is always running; prevents silent audio failures mid-game.",
            ApplyOps = [RegOp.SetDword(SvcKey, "Start", 2)],
            RemoveOps = [RegOp.SetDword(SvcKey, "Start", 2)],
            DetectOps = [RegOp.CheckDword(SvcKey, "Start", 2)],
        },
        new TweakDef
        {
            Id = "gamaudio-disable-audio-enhancements",
            Label = "Disable Global Audio Enhancements",
            Category = "Audio",
            Description =
                "Disables Windows audio signal processing (APO enhancements) globally to reduce audio latency and eliminate DSP processing delay.",
            Tags = ["audio", "gaming", "enhancements", "latency"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Eliminates APO processing latency (5–20ms); removes equaliser / reverb effects.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "UserDuckingPreference", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "UserDuckingPreference")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "UserDuckingPreference", 3)],
        },
        new TweakDef
        {
            Id = "gamaudio-disable-ducking",
            Label = "Disable Communications Audio Ducking",
            Category = "Audio",
            Description =
                "Prevents Windows from lowering other audio streams when a communication app (Teams, Discord) is active, keeping game audio at full volume.",
            Tags = ["audio", "gaming", "ducking", "communications"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Stops game audio quieting when Discord or Teams is active.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\DefaultCommunicationDevice", "UserDuckingPreference", 3),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\DefaultCommunicationDevice", "UserDuckingPreference"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\DefaultCommunicationDevice", "UserDuckingPreference", 3),
            ],
        },
        new TweakDef
        {
            Id = "gamaudio-mmcss-games-sfio",
            Label = "Set MMCSS Games SFIO Priority to High",
            Category = "Audio",
            Description = "Raises SFIO priority for the MMCSS Games task, reducing audio I/O starvation during intensive game disk reads.",
            Tags = ["audio", "gaming", "mmcss", "sfio"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents audio I/O starvation during game asset streaming.",
            ApplyOps = [RegOp.SetString(MmcssKey, "SFIO Priority", "High")],
            RemoveOps = [RegOp.SetString(MmcssKey, "SFIO Priority", "Normal")],
            DetectOps = [RegOp.CheckString(MmcssKey, "SFIO Priority", "High")],
        },
        new TweakDef
        {
            Id = "gamaudio-mmcss-games-scheduling",
            Label = "Set MMCSS Games Scheduling Category to High",
            Category = "Audio",
            Description = "Sets the MMCSS scheduling category for Games to High, giving game audio threads more CPU time slices per quantum.",
            Tags = ["audio", "gaming", "mmcss", "scheduling"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Grants game audio threads more CPU quanta, reducing stutters under CPU load.",
            ApplyOps = [RegOp.SetString(MmcssKey, "Scheduling Category", "High")],
            RemoveOps = [RegOp.SetString(MmcssKey, "Scheduling Category", "Medium")],
            DetectOps = [RegOp.CheckString(MmcssKey, "Scheduling Category", "High")],
        },
        new TweakDef
        {
            Id = "gamaudio-mmcss-games-clock",
            Label = "Reduce MMCSS Games Clock Rate",
            Category = "Audio",
            Description = "Sets the MMCSS clock rate period for the Games task to 10 000 (1 ms) to minimise wake latency between audio callbacks.",
            Tags = ["audio", "gaming", "mmcss", "latency", "clock"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Reduces audio wakeup latency to ~1 ms; minimal effect on battery-powered devices.",
            ApplyOps = [RegOp.SetDword(MmcssKey, "Clock Rate", 10000)],
            RemoveOps = [RegOp.DeleteValue(MmcssKey, "Clock Rate")],
            DetectOps = [RegOp.CheckDword(MmcssKey, "Clock Rate", 10000)],
        },
    ];
}
