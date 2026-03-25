// RegiLattice.Core — Tweaks/SuperFetchSysmainPolicy.cs
// Sprint 274: SuperFetch / SysMain Group Policy (10 tweaks)
// Category: "SuperFetch Policy" | Slug: sfetch
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SuperFetch

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SuperFetchSysmainPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SuperFetch";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sfetch-disable-superfetch",
            Label = "Disable SuperFetch (SysMain) Service",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets EnableSuperfetch=0 in the SuperFetch policy key. Disables the SysMain "
                + "service's predictive pre-loading of frequently used application binaries "
                + "into RAM ahead of launch. On systems with NVMe SSDs, SuperFetch provides "
                + "negligible benefit because cold-load times are already under 100 ms, "
                + "while the service continuously writes prefetch metadata to disk and "
                + "consumes a persistent memory allocation. "
                + "Default: 3 (all modes). Recommended: 0 on SSD systems.",
            Tags = ["superfetch", "sysmain", "performance", "ssd", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSuperfetch", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSuperfetch")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSuperfetch", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-prefetch",
            Label = "Disable Application Prefetch",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description =
                "Sets EnablePrefetcher=0 in the SuperFetch policy key. Stops Windows from "
                + "recording application launch traces in %SystemRoot%\\Prefetch and using "
                + "them to pre-load executable pages before user invocation. On SSDs the "
                + "prefetch metadata I/O adds unnecessary write amplification without "
                + "meaningfully reducing startup latency. "
                + "Default: 3. Recommended: 0 on NVMe/SSD systems.",
            Tags = ["prefetch", "performance", "ssd", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnablePrefetcher", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePrefetcher")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePrefetcher", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-readyboost",
            Label = "Disable ReadyBoost",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets EnableReadyboost=0 in the SuperFetch policy key. Prevents SysMain "
                + "from using removable flash storage as a ReadyBoost cache. ReadyBoost "
                + "was designed for systems with slow HDDs; on systems with SSDs it "
                + "provides no performance benefit and writes extensively to the flash "
                + "device, accelerating wear. "
                + "Default: not set (enabled by policy probe). Recommended: 0.",
            Tags = ["readyboost", "flash", "usb", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableReadyboost", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableReadyboost")],
            DetectOps = [RegOp.CheckDword(Key, "EnableReadyboost", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-readydrive",
            Label = "Disable ReadyDrive Hybrid HDD Cache",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets EnableReadydrive=0 in the SuperFetch policy key. Disables ReadyDrive, "
                + "the feature that uses the NAND cache on hybrid hard drives to speed up "
                + "hibernation resume and boot. On systems using pure SSDs there are no "
                + "hybrid drives and this policy has no effect, but disabling it prevents "
                + "the SysMain driver from scanning for hybrid device capabilities on each "
                + "boot. Default: not set. Recommended: 0.",
            Tags = ["readydrive", "hybrid", "hdd", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableReadydrive", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableReadydrive")],
            DetectOps = [RegOp.CheckDword(Key, "EnableReadydrive", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-boot-trace",
            Label = "Disable Boot Trace for Prefetch",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets EnableBootTrace=0 in the SuperFetch policy key. Stops SysMain from "
                + "collecting an I/O trace during the boot sequence to optimise disk access "
                + "order for subsequent boots. On SSDs random-access latency is sub-0.1 ms, "
                + "rendering access-order optimisation meaningless; the trace itself adds "
                + "kernel overhead during the boot sensitive period. "
                + "Default: 1. Recommended: 0 on SSD systems.",
            Tags = ["boot", "trace", "prefetch", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableBootTrace", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBootTrace")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBootTrace", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-app-launch-prefetch",
            Label = "Disable App-Launch Prefetch Optimisation",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 4,
            Description =
                "Sets SuperFetchMaxSecsBeforeSuspend=0 in the SuperFetch policy key. "
                + "Prevents SysMain from pre-fetching pages for applications that were "
                + "recently suspended and are about to be re-activated. On SSDs the "
                + "resume latency is already negligible, and this prefetch phase keeps "
                + "RAM pages warm that could otherwise be used by actively running code. "
                + "Default: 90 (seconds). Recommended: 0.",
            Tags = ["superfetch", "launch", "prefetch", "resume", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SuperFetchMaxSecsBeforeSuspend", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchMaxSecsBeforeSuspend")],
            DetectOps = [RegOp.CheckDword(Key, "SuperFetchMaxSecsBeforeSuspend", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-logon-prefetch",
            Label = "Disable Logon Prefetch Scenario",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets SuperFetchScenarioPolicyHibernate=0 in the SuperFetch policy key. "
                + "Disables the post-logon SysMain scenario that pre-loads anticipated "
                + "application pages immediately after the user desktop appears. On high-CPU "
                + "machines this scenario races with startup applications, creating "
                + "contention and increasing first-30-second CPU load. "
                + "Default: 1. Recommended: 0.",
            Tags = ["superfetch", "logon", "scenario", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SuperFetchScenarioPolicyHibernate", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchScenarioPolicyHibernate")],
            DetectOps = [RegOp.CheckDword(Key, "SuperFetchScenarioPolicyHibernate", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-memory-profiling",
            Label = "Disable SysMain Memory Profiling",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets SuperFetchMaxSampledPageAge=0 in the SuperFetch policy key. Prevents "
                + "SysMain from maintaining a per-page age histogram that tracks how recently "
                + "each physical memory page was accessed. This profiling data guides the "
                + "pre-loading algorithm but requires SysMain to walk page-frame number "
                + "tables on a recurring timer, adding kernel-mode overhead. "
                + "Default: 7 (days). Recommended: 0.",
            Tags = ["superfetch", "memory", "profiling", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SuperFetchMaxSampledPageAge", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchMaxSampledPageAge")],
            DetectOps = [RegOp.CheckDword(Key, "SuperFetchMaxSampledPageAge", 0)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-heap-prefetch",
            Label = "Disable Application Heap Prefetch",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets SuperFetchDisableHeapDetect=1 in the SuperFetch policy key. Stops "
                + "SysMain from recording heap-allocation patterns of active processes and "
                + "using them to pre-warm heap segments ahead of growth allocations. Heap "
                + "profiling involves introspecting target process address spaces via kernel "
                + "callbacks, adding scheduling jitter on highly multi-threaded workloads. "
                + "Default: 0. Recommended: 1.",
            Tags = ["superfetch", "heap", "prefetch", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SuperFetchDisableHeapDetect", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchDisableHeapDetect")],
            DetectOps = [RegOp.CheckDword(Key, "SuperFetchDisableHeapDetect", 1)],
        },
        new TweakDef
        {
            Id = "sfetch-disable-telemetry",
            Label = "Disable SuperFetch Telemetry",
            Category = "SuperFetch Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets SuperFetchDisableTelemetry=1 in the SuperFetch policy key. Prevents "
                + "SysMain from emitting ETW events and submitting memory-usage reports to "
                + "the Windows feedback infrastructure. These reports include page-fault "
                + "rates, working-set statistics, and popular application lists derived from "
                + "all user accounts on the machine, which can profile usage patterns. "
                + "Default: 0. Recommended: 1.",
            Tags = ["superfetch", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SuperFetchDisableTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchDisableTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "SuperFetchDisableTelemetry", 1)],
        },
    ];
}
