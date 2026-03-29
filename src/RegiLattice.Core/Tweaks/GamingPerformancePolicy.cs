// RegiLattice.Core — Tweaks/GamingPerformancePolicy.cs
// Game scheduler, CPU/GPU priority, and gaming performance Group Policy controls (Sprint 605).
// Category: "Gaming Performance Policy" | Slug: gamperf
// Key: HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class GamingPerformancePolicy
{
    private const string GamesKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games";
    private const string SysKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "gamperf-set-games-scheduling-gpu100",
            Label = "Gaming Perf: Set Game Scheduler GPU Priority to Maximum (100%)",
            Category = "Gaming Performance Policy",
            Description = "Sets GPU Priority=8 in Multimedia SystemProfile Games task. Configures the Windows Multimedia Class Scheduler Service (MMCSS) to assign the highest GPU execution priority to processes registered under the Games scheduling category. " +
                "MMCSS Games profile governs time-critical GPU resource allocation for games and real-time rendering applications. Setting GPU Priority=8 (the maximum value) ensures game rendering passes are given priority access to the GPU command queue over background tasks such as desktop composition, codec decode, or system monitoring overlays.",
            Tags = ["gaming", "gpu", "scheduler", "mmcss", "priority"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Max GPU scheduling priority for games; other GPU workloads de-prioritised while game is running.",
            ApplyOps = [RegOp.SetDword(GamesKey, "GPU Priority", 8)],
            RemoveOps = [RegOp.DeleteValue(GamesKey, "GPU Priority")],
            DetectOps = [RegOp.CheckDword(GamesKey, "GPU Priority", 8)],
        },
        new TweakDef
        {
            Id = "gamperf-set-games-scheduling-priority-high",
            Label = "Gaming Perf: Set Game Thread Priority to High",
            Category = "Gaming Performance Policy",
            Description = "Sets Priority=6 in Multimedia SystemProfile Games task. Sets the Windows MMCSS thread priority for game processes to 6, which maps to the 'High' thread scheduling priority. " +
                "MMCSS priority 6 (High) gives game threads elevated scheduling preference over normal threads (priority 2) and background threads (priority 1), without reaching real-time priority levels that could cause system instability. This ensures game simulation and rendering threads are not preempted by background telemetry and maintenance tasks during time-sensitive frame computation.",
            Tags = ["gaming", "cpu", "thread-priority", "mmcss", "scheduler"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Game threads at High priority; reduced preemption from background tasks during frame computation.",
            ApplyOps = [RegOp.SetDword(GamesKey, "Priority", 6)],
            RemoveOps = [RegOp.DeleteValue(GamesKey, "Priority")],
            DetectOps = [RegOp.CheckDword(GamesKey, "Priority", 6)],
        },
        new TweakDef
        {
            Id = "gamperf-set-mmcss-scheduling-category-high",
            Label = "Gaming Perf: Set MMCSS Games Scheduling Category to High",
            Category = "Gaming Performance Policy",
            Description = "Sets Scheduling Category=High in Multimedia SystemProfile Games task. Configures the MMCSS scheduling category for the Games profile, determining how aggressively the scheduler boosts game thread quantum lengths. " +
                "The High scheduling category allows game threads to receive longer CPU quantum slices per scheduling interval compared to Medium or Low, reducing the frequency of context switches during active rendering loops. Fewer context switches mean less OS scheduling overhead per frame and more deterministic frame timing.",
            Tags = ["gaming", "cpu", "quantum", "mmcss", "frame-time"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "High scheduling category for game threads; longer CPU quanta reduce context-switch overhead per frame.",
            ApplyOps = [RegOp.SetString(GamesKey, "Scheduling Category", "High")],
            RemoveOps = [RegOp.DeleteValue(GamesKey, "Scheduling Category")],
            DetectOps = [RegOp.CheckString(GamesKey, "Scheduling Category", "High")],
        },
        new TweakDef
        {
            Id = "gamperf-set-mmcss-sfio-priority-high",
            Label = "Gaming Perf: Set MMCSS SFIO Priority to High for Asset Streaming",
            Category = "Gaming Performance Policy",
            Description = "Sets SFIO Priority=High in Multimedia SystemProfile Games task. Sets the Scheduled File I/O (SFIO) priority for game processes within MMCSS to High. " +
                "Modern games heavily rely on background asset streaming — loading textures, audio, and map data from disk while the game is running. SFIO priority determines how quickly the OS services I/O requests from game processes relative to other I/O consumers. High SFIO priority ensures disk operations for game asset streaming are serviced before background indexer, antivirus scanner, or cloud sync I/O.",
            Tags = ["gaming", "io", "sfio", "asset-streaming", "disk"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "High SFIO priority for game I/O; asset streaming served ahead of background disk consumers.",
            ApplyOps = [RegOp.SetString(GamesKey, "SFIO Priority", "High")],
            RemoveOps = [RegOp.DeleteValue(GamesKey, "SFIO Priority")],
            DetectOps = [RegOp.CheckString(GamesKey, "SFIO Priority", "High")],
        },
        new TweakDef
        {
            Id = "gamperf-set-games-affinity-all-cores",
            Label = "Gaming Perf: Set MMCSS Games Affinity to All CPU Cores",
            Category = "Gaming Performance Policy",
            Description = "Sets Affinity=0 in Multimedia SystemProfile Games task. Sets the CPU affinity mask for the MMCSS Games scheduling category to 0, which instructs MMCSS to allow game threads to run on all available CPU cores rather than a constrained subset. " +
                "A value of 0 in the Affinity field means no affinity restriction — game threads can migrate to any core. This is optimal for modern games that use job-graph and worker-thread models to parallelise simulation, physics, and audio processing across all available cores. Pinning to a subset of cores (non-zero affinity) reduces parallelism and can cause load imbalance on multi-core CPUs.",
            Tags = ["gaming", "cpu", "affinity", "cores", "parallel"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Unrestricted CPU core affinity for game threads; game parallelism spans all CCD/cores.",
            ApplyOps = [RegOp.SetDword(GamesKey, "Affinity", 0)],
            RemoveOps = [RegOp.DeleteValue(GamesKey, "Affinity")],
            DetectOps = [RegOp.CheckDword(GamesKey, "Affinity", 0)],
        },
        new TweakDef
        {
            Id = "gamperf-disable-background-only-affinity",
            Label = "Gaming Perf: Disable Background-Only MMCSS Affinity Restriction",
            Category = "Gaming Performance Policy",
            Description = "Sets Background Only=False in Multimedia SystemProfile Games task. Clears the background-only restriction flag for the MMCSS Games profile, ensuring game processes are not treated as background-priority workloads by the scheduler. " +
                "When Background Only is True, MMCSS treats threads in that category as background work — deprioritising their scheduling and reducing their CPU time allocation when a foreground process is active. For games that must be in the foreground to run, setting this False is a no-op in practice, but explicitly clearing it in MMCSS configuration prevents any edge-case scenario where the game process is momentarily backgrounded (e.g., during Alt+Tab) from permanently degrading its scheduling.",
            Tags = ["gaming", "cpu", "background", "mmcss", "foreground"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Game threads not restricted to background-only affinity; normal scheduling during brief window focus changes.",
            ApplyOps = [RegOp.SetString(GamesKey, "Background Only", "False")],
            RemoveOps = [RegOp.DeleteValue(GamesKey, "Background Only")],
            DetectOps = [RegOp.CheckString(GamesKey, "Background Only", "False")],
        },
        new TweakDef
        {
            Id = "gamperf-set-system-responsiveness-20pct",
            Label = "Gaming Perf: Set SystemResponsiveness to Reserve 20% CPU for Background Tasks",
            Category = "Gaming Performance Policy",
            Description = "Sets SystemResponsiveness=20 in Multimedia SystemProfile (parent key). Controls what percentage of CPU time MMCSS reserves for background tasks when a high-priority multimedia or gaming application is running. " +
                "The default value is 20 (20% reserved for background). Setting this to 20 (or lower) maximises the CPU time available to the gaming/multimedia process. Values above 20 make the system more responsive to background tasks at the cost of game frame rates. Windows audio and game processes compete for the 80% non-reserved pool; a 20% reservation ensures the system remains stable (audio does not glitch) even under full game load.",
            Tags = ["gaming", "cpu", "mmcss", "responsiveness", "background"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "20% CPU reserved for background tasks; 80% available to games — matches Windows MMCSS default but made explicit.",
            ApplyOps = [RegOp.SetDword(SysKey, "SystemResponsiveness", 20)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "SystemResponsiveness")],
            DetectOps = [RegOp.CheckDword(SysKey, "SystemResponsiveness", 20)],
        },
        new TweakDef
        {
            Id = "gamperf-enable-network-throttling-index-bypass",
            Label = "Gaming Perf: Disable MMCSS Network Throttling for Low-Latency Gaming",
            Category = "Gaming Performance Policy",
            Description = "Sets NetworkThrottlingIndex=0xFFFFFFFF in Multimedia SystemProfile. Disables MMCSS network throttling for multimedia applications. " +
                "By default, MMCSS throttles network activity for multimedia processes to prevent network I/O from interrupting the CPU scheduler's time allocations for audio/video threads. While beneficial for video playback, this throttling adds latency to outbound network packets from game processes. Setting NetworkThrottlingIndex to 0xFFFFFFFF disables the throttle, allowing game networking threads to send packets at their full rate without artificial scheduling delays.",
            Tags = ["gaming", "network", "latency", "mmcss", "throttling"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Network throttling disabled for multimedia tasks; game packets sent without MMCSS-imposed delay. May affect audio in edge cases.",
            ApplyOps = [RegOp.SetDword(SysKey, "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.DeleteValue(SysKey, "NetworkThrottlingIndex")],
            DetectOps = [RegOp.CheckDword(SysKey, "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
        },
        new TweakDef
        {
            Id = "gamperf-set-games-clock-rate-10000hz",
            Label = "Gaming Perf: Set MMCSS Games Clock Rate to 10,000 Hz (0.1 ms Precision)",
            Category = "Gaming Performance Policy",
            Description = "Sets Clock Rate=10000 in Multimedia SystemProfile Games task. Sets the Windows multimedia timer resolution for game processes to 10,000 units (100 microseconds / 0.1 ms). " +
                "The Clock Rate value in MMCSS controls the minimum timer resolution requested by game processes. A higher clock rate (lower number of 100ns units) results in more frequent clock interrupts, enabling finer-grained sleep/wait precision for game loops. " +
                "10,000 (0.1ms) represents the finest practical clock granularity achievable on x86 hardware. This benefits games with precision sleep-based frame limiters and reduces the floor on observable frame timing jitter.",
            Tags = ["gaming", "timer", "clock", "mmcss", "precision"],
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "0.1ms timer clock for game tasks; tighter frame timing precision at cost of slightly higher interrupt rate.",
            ApplyOps = [RegOp.SetDword(GamesKey, "Clock Rate", 10000)],
            RemoveOps = [RegOp.DeleteValue(GamesKey, "Clock Rate")],
            DetectOps = [RegOp.CheckDword(GamesKey, "Clock Rate", 10000)],
        },
        new TweakDef
        {
            Id = "gamperf-enable-multimedia-gaming-class-scheduler",
            Label = "Gaming Perf: Enable Multimedia Class Scheduler for Gaming Tasks",
            Category = "Gaming Performance Policy",
            Description = "Sets Enabled=1 in Multimedia SystemProfile. Ensures the Windows Multimedia Class Scheduler Service (MMCSS) is active and applies scheduling boosts for registered audio, gaming, and multimedia tasks. " +
                "MMCSS is the system service that applies all Games and Audio scheduling profiles described in the SystemProfile Tasks registry. If MMCSS is disabled or its settings are cleared during OS hardening, the GPU priority, CPU thread priority, and timer clock rate settings have no effect. Explicitly enabling MMCSS as a policy ensures all the other gaming performance settings in this module are active.",
            Tags = ["gaming", "mmcss", "scheduler", "enable", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures MMCSS is active; prerequisite for all other MMCSS gaming performance settings to take effect.",
            ApplyOps = [RegOp.SetDword(SysKey, "Enabled", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "Enabled")],
            DetectOps = [RegOp.CheckDword(SysKey, "Enabled", 1)],
        },
    ];
}
