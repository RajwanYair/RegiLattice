// RegiLattice.Core — Tweaks/WddmDriverPolicy.cs
// Windows Display Driver Model (WDDM) GPU scheduler, TDR, and driver store policy — Sprint 510.
// Category: "WDDM Driver Policy" | Slug: wddmpol
// Registry: HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WddmDriverPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";
    private const string ScKey  = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler";
    private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "wddmpol-enable-hwscheduling",
            Label        = "Enable Hardware-Accelerated GPU Scheduling (HAGS)",
            Category     = "WDDM Driver Policy",
            Description  = "Enables Hardware-Accelerated GPU Scheduling (HAGS/WDDM 2.7) which offloads GPU memory scheduling decisions from the CPU-based WDDM scheduler to the GPU firmware, reducing latency and CPU overhead for GPU-bound workloads.",
            Tags         = ["wddm", "gpu-scheduling", "hags", "performance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "GPU Hardware Accelerated Scheduling enabled; reduced CPU-GPU scheduling latency and CPU overhead.",
            ApplyOps     = [RegOp.SetDword(Key, "HwSchMode", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "HwSchMode")],
            DetectOps    = [RegOp.CheckDword(Key, "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id           = "wddmpol-set-tdr-delay-8s",
            Label        = "Set GPU Timeout Detection and Recovery Delay to 8 Seconds",
            Category     = "WDDM Driver Policy",
            Description  = "Sets the Timeout Detection and Recovery (TDR) delay to 8 seconds (from the default 2 seconds), preventing false positive GPU resets during long compute operations (ML inference, video encoding) that legitimately use the GPU for longer than 2 seconds.",
            Tags         = ["wddm", "tdr", "gpu-timeout", "compute", "stability", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "TDR delay set to 8s; prevents false GPU resets during long compute jobs. Delays detection of real hangs.",
            ApplyOps     = [RegOp.SetDword(Key, "TdrDelay", 8)],
            RemoveOps    = [RegOp.DeleteValue(Key, "TdrDelay")],
            DetectOps    = [RegOp.CheckDword(Key, "TdrDelay", 8)],
        },
        new TweakDef
        {
            Id           = "wddmpol-set-tdr-level-recover",
            Label        = "Set TDR Level to Recover GPU Without System Reboot",
            Category     = "WDDM Driver Policy",
            Description  = "Configures the TDR recovery level to attempt GPU reset and recovery without a full system reboot (TdrLevelRecover=3), allowing the display driver to be restarted and the session to continue after a GPU hang.",
            Tags         = ["wddm", "tdr", "recovery", "gpu-reset", "stability", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "TDR recovery without reboot enabled; GPU hang results in driver restart, not full system crash.",
            ApplyOps     = [RegOp.SetDword(Key, "TdrLevel", 3)],
            RemoveOps    = [RegOp.DeleteValue(Key, "TdrLevel")],
            DetectOps    = [RegOp.CheckDword(Key, "TdrLevel", 3)],
        },
        new TweakDef
        {
            Id           = "wddmpol-enable-gpu-preemption-dma",
            Label        = "Enable GPU DMA-Level Preemption for Responsiveness",
            Category     = "WDDM Driver Policy",
            Description  = "Sets the GPU preemption granularity to DMA buffer level, allowing the WDDM scheduler to preempt running GPU workloads at DMA packet boundaries for improved UI responsiveness during background GPU-intensive tasks.",
            Tags         = ["wddm", "preemption", "dma", "scheduler", "responsiveness", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "GPU preemption at DMA level enabled; UI stays responsive during background GPU workloads.",
            ApplyOps     = [RegOp.SetDword(ScKey, "EnablePreemption", 1)],
            RemoveOps    = [RegOp.DeleteValue(ScKey, "EnablePreemption")],
            DetectOps    = [RegOp.CheckDword(ScKey, "EnablePreemption", 1)],
        },
        new TweakDef
        {
            Id           = "wddmpol-block-display-driver-fallback",
            Label        = "Block Fallback to Microsoft Basic Display Adapter",
            Category     = "WDDM Driver Policy",
            Description  = "Prevents Windows from falling back to the Microsoft Basic Display Adapter (2048×1152 VESA-only) when the GPU driver crashes, maintaining the last known working display driver and attempting recovery instead.",
            Tags         = ["wddm", "basic-display", "driver-fallback", "recovery", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "VGA-mode fallback blocked; driver crash triggers recovery, not basic display. May yield blank screen.",
            ApplyOps     = [RegOp.SetDword(PolKey, "DisableBasicDisplayDriverFallback", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "DisableBasicDisplayDriverFallback")],
            DetectOps    = [RegOp.CheckDword(PolKey, "DisableBasicDisplayDriverFallback", 1)],
        },
        new TweakDef
        {
            Id           = "wddmpol-disable-dxgi-flip-discard",
            Label        = "Disable Presentation Model Flip-Discard Optimisation",
            Category     = "WDDM Driver Policy",
            Description  = "Disables the DXGI flip-discard presentation model that reuses swap chain surfaces, falling back to flip-sequential for maximum frame ordering correctness in trading and video production environments where tearing prevention is critical.",
            Tags         = ["wddm", "dxgi", "flip-discard", "presentation", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Flip-discard presentation disabled; flip-sequential used. Maximum frame ordering correctness at slight perf cost.",
            ApplyOps     = [RegOp.SetDword(PolKey, "DisableFlipDiscard", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "DisableFlipDiscard")],
            DetectOps    = [RegOp.CheckDword(PolKey, "DisableFlipDiscard", 1)],
        },
        new TweakDef
        {
            Id           = "wddmpol-log-tdr-events",
            Label        = "Log GPU TDR Recovery Events to System Event Log",
            Category     = "WDDM Driver Policy",
            Description  = "Enables System event log entries (EventID 4101, Display driver stopped responding and has recovered) for GPU TDR events, providing a history of GPU hangs and recovery cycles for diagnostics.",
            Tags         = ["wddm", "tdr", "event-log", "audit", "gpu-stability", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "GPU TDR events logged in System log; driver hang frequency visible for GPU stability diagnostics.",
            ApplyOps     = [RegOp.SetDword(Key, "TdrLogging", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "TdrLogging")],
            DetectOps    = [RegOp.CheckDword(Key, "TdrLogging", 1)],
        },
        new TweakDef
        {
            Id           = "wddmpol-disable-gpu-driver-telemetry",
            Label        = "Disable WDDM GPU Driver Telemetry to Microsoft",
            Category     = "WDDM Driver Policy",
            Description  = "Prevents the Windows Display Driver Model from sending GPU driver crash reports, TDR telemetry, and hardware capability telemetry to Microsoft, protecting GPU model/driver version information from cloud disclosure.",
            Tags         = ["wddm", "telemetry", "privacy", "gpu", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WDDM GPU telemetry to Microsoft disabled; GPU model, driver version, and TDR events not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(PolKey, "DisableGPUTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "DisableGPUTelemetry")],
            DetectOps    = [RegOp.CheckDword(PolKey, "DisableGPUTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "wddmpol-enable-virtual-display",
            Label        = "Enable Virtual Display Adapter for Headless Operation",
            Category     = "WDDM Driver Policy",
            Description  = "Enables the Windows virtual display adapter (IndirectDisplay) for headless server scenarios, providing a software display output that supports RDP and remote management tools without a physical GPU or monitor.",
            Tags         = ["wddm", "virtual-display", "headless", "rdp", "server", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Virtual display adapter enabled for headless RDP; servers without physical GPU get a software display.",
            ApplyOps     = [RegOp.SetDword(PolKey, "EnableVirtualDisplayAdapter", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "EnableVirtualDisplayAdapter")],
            DetectOps    = [RegOp.CheckDword(PolKey, "EnableVirtualDisplayAdapter", 1)],
        },
        new TweakDef
        {
            Id           = "wddmpol-set-gpu-priority-realtime",
            Label        = "Set GPU Work Item Priority to Normal for System Processes",
            Category     = "WDDM Driver Policy",
            Description  = "Configures the WDDM scheduler to run system and background GPU work items at Normal priority, preventing GPU starvation of foreground applications by long-running background ML or compute workloads.",
            Tags         = ["wddm", "gpu-priority", "scheduler", "background", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Background GPU work items set to Normal priority; foreground app rendering not starved by compute jobs.",
            ApplyOps     = [RegOp.SetDword(ScKey, "BackgroundGPUPriority", 1)],
            RemoveOps    = [RegOp.DeleteValue(ScKey, "BackgroundGPUPriority")],
            DetectOps    = [RegOp.CheckDword(ScKey, "BackgroundGPUPriority", 1)],
        },
    ];
}
