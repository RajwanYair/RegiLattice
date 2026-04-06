#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class GamingLatencyTuning
{
    private const string MmcssKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";
    private const string KernelKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel";
    private const string TimerKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Executive";
    private const string GfxKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler";
    private const string TcpKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";
    private const string PwrKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "glatency-enable-global-timer-resolution",
            Label = "Enable Global High-Resolution Timer Requests",
            Category = "Gaming",
            Description =
                "Enables GlobalTimerResolutionRequests in the kernel, allowing any process that requests a high-resolution timer to benefit globally, not just within its own process.",
            Tags = ["latency", "gaming", "timer", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Reduces system-wide scheduling jitter; may slightly increase power consumption.",
            ApplyOps = [RegOp.SetDword(KernelKey, "GlobalTimerResolutionRequests", 1)],
            RemoveOps = [RegOp.DeleteValue(KernelKey, "GlobalTimerResolutionRequests")],
            DetectOps = [RegOp.CheckDword(KernelKey, "GlobalTimerResolutionRequests", 1)],
        },
        new TweakDef
        {
            Id = "glatency-set-system-responsiveness",
            Label = "Set MMCSS System Responsiveness to 0% Background",
            Category = "Gaming",
            Description =
                "Sets MMCSS SystemResponsiveness to 0, dedicating all scheduler time to latency-sensitive threads (MMCSS) and leaving 0% guaranteed for background tasks during gaming.",
            Tags = ["latency", "gaming", "mmcss", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Maximises game thread priority at the expense of background task responsiveness.",
            ApplyOps = [RegOp.SetDword(MmcssKey, "SystemResponsiveness", 0)],
            RemoveOps = [RegOp.SetDword(MmcssKey, "SystemResponsiveness", 20)],
            DetectOps = [RegOp.CheckDword(MmcssKey, "SystemResponsiveness", 0)],
        },
        new TweakDef
        {
            Id = "glatency-set-gpu-dpc-priority",
            Label = "Set GPU Scheduler DPC Priority to Latency-Optimised",
            Category = "Gaming",
            Description =
                "Configures the GPU kernel scheduler to run DPCs at a higher priority, reducing the time between a frame being ready on the GPU and it being displayed.",
            Tags = ["latency", "gaming", "gpu", "dpc"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces GPU-to-display pipeline latency in latency-sensitive titles.",
            ApplyOps = [RegOp.SetDword(GfxKey, "GpuDpcPriority", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "GpuDpcPriority")],
            DetectOps = [RegOp.CheckDword(GfxKey, "GpuDpcPriority", 1)],
        },
        new TweakDef
        {
            Id = "glatency-disable-power-throttling",
            Label = "Disable Power Throttling for Game Processes",
            Category = "Gaming",
            Description =
                "Disables EcoQoS / Power Throttling globally so Windows does not reduce CPU clock speeds for processes the scheduler misidentifies as background workloads during gameplay.",
            Tags = ["latency", "gaming", "power", "cpu"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Prevents CPU throttling for game processes; increases power draw slightly.",
            ApplyOps = [RegOp.SetDword(PwrKey, "PowerThrottlingOff", 1)],
            RemoveOps = [RegOp.DeleteValue(PwrKey, "PowerThrottlingOff")],
            DetectOps = [RegOp.CheckDword(PwrKey, "PowerThrottlingOff", 1)],
        },
        new TweakDef
        {
            Id = "glatency-tcp-ack-frequency",
            Label = "Set TCP ACK Frequency for Low-Latency Gaming",
            Category = "Gaming",
            Description =
                "Sets TCP ACKFrequency to 1 for the local TCP stack, causing ACKs to be sent immediately rather than batched, reducing round-trip time for online game traffic.",
            Tags = ["latency", "gaming", "network", "tcp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces TCP round-trip latency for online games at the cost of marginally more ACK packets.",
            ApplyOps = [RegOp.SetDword(TcpKey, "TcpAckFrequency", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpKey, "TcpAckFrequency")],
            DetectOps = [RegOp.CheckDword(TcpKey, "TcpAckFrequency", 1)],
        },
        new TweakDef
        {
            Id = "glatency-disable-cpu-parking",
            Label = "Disable CPU Core Parking for Gaming",
            Category = "Gaming",
            Description =
                "Disables CPU core parking so all logical cores remain active at all times, eliminating delays caused by unparking a core when a game suddenly requires additional CPU threads.",
            Tags = ["latency", "gaming", "cpu", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Eliminates core-unparking delays; all CPU cores remain warm and ready during gameplay.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax",
                    100
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                    "ValueMax",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "glatency-disable-hyperthreading-adaptive",
            Label = "Disable Heterogeneous Scheduler SMT Utilisation Cap",
            Category = "Gaming",
            Description =
                "Removes the adaptive SMT cap Windows applies on hybrid CPUs (Intel 12th/13th/14th gen), allowing the game scheduler to use HT/SMT threads without efficiency-core interference.",
            Tags = ["latency", "gaming", "cpu", "scheduler"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Beneficial on Intel hybrid-core CPUs (Alder/Raptor Lake); may not help on homogeneous CPUs.",
            ApplyOps = [RegOp.SetDword(KernelKey, "HeteroCpuPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(KernelKey, "HeteroCpuPolicy")],
            DetectOps = [RegOp.CheckDword(KernelKey, "HeteroCpuPolicy", 0)],
        },
        new TweakDef
        {
            Id = "glatency-set-irq-affinity-hint",
            Label = "Set Interrupt Affinity Hint for Low-Latency Scheduling",
            Category = "Gaming",
            Description =
                "Sets the interrupt service routine affinity hint in the kernel, biasing device interrupts toward a logical core group not used by the game's main thread.",
            Tags = ["latency", "gaming", "interrupt", "cpu"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Reduces interrupt interference on game threads; advanced tuning, works best on CPUs with 8+ cores.",
            ApplyOps = [RegOp.SetDword(KernelKey, "SerializeImages", 0)],
            RemoveOps = [RegOp.DeleteValue(KernelKey, "SerializeImages")],
            DetectOps = [RegOp.CheckDword(KernelKey, "SerializeImages", 0)],
        },
        new TweakDef
        {
            Id = "glatency-gpu-preemption-compute",
            Label = "Set GPU Compute Preemption to Instruction Level",
            Category = "Gaming",
            Description =
                "Configures the WDDM GPU scheduler to preempt compute workloads at instruction granularity, reducing GPU preemption latency when switching between game and compute tasks.",
            Tags = ["latency", "gaming", "gpu", "compute"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces GPU context-switch latency for gaming workloads on systems also running compute tasks.",
            ApplyOps = [RegOp.SetDword(GfxKey, "ComputePreemption", 1)],
            RemoveOps = [RegOp.DeleteValue(GfxKey, "ComputePreemption")],
            DetectOps = [RegOp.CheckDword(GfxKey, "ComputePreemption", 1)],
        },
        new TweakDef
        {
            Id = "glatency-tcp-no-delay",
            Label = "Enable TCP No-Delay (Disable Nagle per Interface)",
            Category = "Gaming",
            Description =
                "Enables TCP_NODELAY globally for the Tcpip stack, flushing small game packets immediately instead of waiting for the Nagle algorithm to accumulate data.",
            Tags = ["latency", "gaming", "network", "tcp"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces in-game network latency by sending small packets immediately without buffering.",
            ApplyOps = [RegOp.SetDword(TcpKey, "TcpNoDelay", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpKey, "TcpNoDelay")],
            DetectOps = [RegOp.CheckDword(TcpKey, "TcpNoDelay", 1)],
        },
    ];
}
