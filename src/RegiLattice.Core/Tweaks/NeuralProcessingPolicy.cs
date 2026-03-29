// RegiLattice.Core — Tweaks/NeuralProcessingPolicy.cs
// Neural Processing Unit Policy — Sprint 554.
// Configures Group Policy for Windows NPU (Neural Processing Unit) governance:
// NPU workload scheduling, NPU telemetry, power management during AI tasks,
// and inter-application NPU resource sharing policies.
// Category: "Neural Processing Policy" | Slug: npu
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\AI\NPU

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NeuralProcessingPolicy
{
    private const string NpuKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AI\NPU";

    private const string AiHwKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AI\HardwareAcceleration";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "npu-enable-npu-scheduling",
                Label = "Neural Processing: Enable NPU Scheduling for AI Workloads",
                Category = "Neural Processing Policy",
                Description =
                    "Sets NPUSchedulingEnabled=1 in NPU policy. Activates the Windows NPU scheduler to manage AI inference workloads across the Neural Processing Unit present in Copilot+ PC devices (Qualcomm Hexagon, Intel NPU, AMD XDNA). NPU scheduling distributes inference tasks across NPU compute clusters, prioritises interactive AI workloads over background tasks, and prevents one application from monopolising the NPU. Without NPU scheduling, each application competes directly for NPU resources leading to latency spikes.",
                Tags = ["npu", "scheduling", "ai", "inference", "hardware"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "NPU scheduling is enabled. Requires a device with a Neural Processing Unit (Copilot+ PC). On devices without an NPU, this setting has no effect.",
                ApplyOps = [RegOp.SetDword(NpuKey, "NPUSchedulingEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "NPUSchedulingEnabled")],
                DetectOps = [RegOp.CheckDword(NpuKey, "NPUSchedulingEnabled", 1)],
            },
            new TweakDef
            {
                Id = "npu-disable-npu-telemetry",
                Label = "Neural Processing: Disable NPU Performance Telemetry Collection",
                Category = "Neural Processing Policy",
                Description =
                    "Sets NPUTelemetryEnabled=0 in NPU policy. Prevents the Windows AI platform from collecting and transmitting NPU performance metrics, utilisation statistics, and inference timing data to Microsoft. NPU telemetry helps Microsoft optimise NPU workload scheduling but includes data about which applications use the NPU, inference latency percentiles, and NPU idle/active time ratios. Disabling this telemetry complements general AI telemetry restrictions in privacy-focused managed environments.",
                Tags = ["npu", "telemetry", "privacy", "ai", "hardware"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU telemetry is disabled. No NPU usage data is sent to Microsoft. NPU features continue to function; optimisation of NPU scheduling by Microsoft may be slower.",
                ApplyOps = [RegOp.SetDword(NpuKey, "NPUTelemetryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "NPUTelemetryEnabled")],
                DetectOps = [RegOp.CheckDword(NpuKey, "NPUTelemetryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "npu-set-max-npu-utilisation",
                Label = "Neural Processing: Cap Maximum NPU Utilisation to 80%",
                Category = "Neural Processing Policy",
                Description =
                    "Sets MaxNPUUtilisation=80 in NPU policy. Caps the maximum NPU utilisation percentage used by AI inference workloads at 80%. Reserving 20% NPU headroom ensures the NPU can respond to new inference requests without queuing when the device is already running background AI tasks. An NPU saturated at 100% exhibits high inference latency for new requests. The 80% cap balances throughput (allowing substantial AI workloads) against interactive responsiveness.",
                Tags = ["npu", "utilisation", "performance", "ai", "throttle"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU utilisation is capped at 80%. Background AI workloads may take longer to complete. Interactive AI responses remain responsive due to reserved headroom.",
                ApplyOps = [RegOp.SetDword(NpuKey, "MaxNPUUtilisation", 80)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "MaxNPUUtilisation")],
                DetectOps = [RegOp.CheckDword(NpuKey, "MaxNPUUtilisation", 80)],
            },
            new TweakDef
            {
                Id = "npu-disable-npu-on-battery",
                Label = "Neural Processing: Disable NPU AI Workloads When on Battery",
                Category = "Neural Processing Policy",
                Description =
                    "Sets DisableNPUOnBattery=1 in NPU policy. Prevents AI inference tasks from being dispatched to the NPU when the device is operating on battery power without AC connection. The NPU, while more power-efficient than the CPU or GPU for AI tasks, still consumes meaningful battery power when running sustained inference workloads. Disabling NPU on battery extends battery life and reduces thermal output for portable productivity use. AI features fall back to CPU inference or are deferred.",
                Tags = ["npu", "battery", "power", "ai", "inference"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU is disabled on battery power. AI features use CPU inference on battery with higher latency but better battery life. NPU is re-enabled when AC power is connected.",
                ApplyOps = [RegOp.SetDword(NpuKey, "DisableNPUOnBattery", 1)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "DisableNPUOnBattery")],
                DetectOps = [RegOp.CheckDword(NpuKey, "DisableNPUOnBattery", 1)],
            },
            new TweakDef
            {
                Id = "npu-enable-hardware-acceleration-audit",
                Label = "Neural Processing: Enable AI Hardware Acceleration Audit Log",
                Category = "Neural Processing Policy",
                Description =
                    "Sets HardwareAccelerationAudit=1 in HardwareAcceleration policy. Writes an Event Log entry each time an application requests and is granted AI hardware acceleration (NPU or GPU) for inference. Audit events include the requesting application, the hardware accelerator assigned, the model type, and the timestamp. Useful for security monitoring (detecting unusual applications performing AI inference) and capacity planning (understanding which applications drive NPU/GPU demand).",
                Tags = ["npu", "gpu", "audit", "hardware-acceleration", "ai"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hardware acceleration requests are logged. Log volume depends on AI feature usage. Adds Event Log entries for each inference acceleration grant.",
                ApplyOps = [RegOp.SetDword(AiHwKey, "HardwareAccelerationAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(AiHwKey, "HardwareAccelerationAudit")],
                DetectOps = [RegOp.CheckDword(AiHwKey, "HardwareAccelerationAudit", 1)],
            },
            new TweakDef
            {
                Id = "npu-restrict-npu-to-system-apps",
                Label = "Neural Processing: Restrict NPU Access to System and Approved Apps",
                Category = "Neural Processing Policy",
                Description =
                    "Sets RestrictNPUToSystemApps=1 in NPU policy. Limits NPU access to Microsoft system components and applications explicitly approved for NPU use via enterprise policy. Third-party applications that request NPU inference acceleration are redirected to CPU inference. Prevents third-party applications from performing high-throughput AI processing that could be used for data exfiltration through covert AI channels, or for competitive intelligence gathering through on-device model execution.",
                Tags = ["npu", "access-control", "system-apps", "restriction", "ai"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Third-party apps cannot use NPU for AI inference. Only Microsoft-signed system components use NPU acceleration. Third-party apps fall back to CPU with degraded AI performance.",
                ApplyOps = [RegOp.SetDword(NpuKey, "RestrictNPUToSystemApps", 1)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "RestrictNPUToSystemApps")],
                DetectOps = [RegOp.CheckDword(NpuKey, "RestrictNPUToSystemApps", 1)],
            },
            new TweakDef
            {
                Id = "npu-enable-npu-power-saving-mode",
                Label = "Neural Processing: Enable NPU Power Efficiency Mode",
                Category = "Neural Processing Policy",
                Description =
                    "Sets NPUPowerSavingMode=1 in NPU policy. Activates the NPU's power-efficiency execution profile, which reduces hardware clock speeds and voltage when the inference workload can tolerate slightly higher latency. NPUs have distinct performance and efficiency operating points; the efficiency mode runs at a lower operating point that delivers acceptable inference latency for background tasks at 30–50% less power consumption. Effective for background tasks like continuous live caption, real-time translation, or ambient AI where sub-10ms latency is not critical.",
                Tags = ["npu", "power-saving", "efficiency", "ai", "battery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU operates in power-saving mode with reduced clock speed. Background inference latency increases by ~1.5–2×. Interactive AI responses (user-initiated) run in full performance mode.",
                ApplyOps = [RegOp.SetDword(NpuKey, "NPUPowerSavingMode", 1)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "NPUPowerSavingMode")],
                DetectOps = [RegOp.CheckDword(NpuKey, "NPUPowerSavingMode", 1)],
            },
            new TweakDef
            {
                Id = "npu-disable-npu-firmware-update",
                Label = "Neural Processing: Disable Automatic NPU Firmware Updates",
                Category = "Neural Processing Policy",
                Description =
                    "Sets DisableNPUFirmwareUpdate=1 in NPU policy. Prevents Windows from automatically downloading and installing NPU firmware/driver updates via Windows Update. In production enterprise environments, driver and firmware updates must go through IT change management processes before deployment: a firmware update that changes NPU instruction set compatibility or model inference accuracy could break AI-dependent workflows without warning. IT controls NPU firmware roll-out through WSUS, MECM, or Intune.",
                Tags = ["npu", "firmware", "update", "managed", "change-control"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU firmware updates do not install automatically. IT must push NPU driver/firmware updates through managed channels. Keeps AI inference behaviour predictable across device fleet.",
                ApplyOps = [RegOp.SetDword(NpuKey, "DisableNPUFirmwareUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "DisableNPUFirmwareUpdate")],
                DetectOps = [RegOp.CheckDword(NpuKey, "DisableNPUFirmwareUpdate", 1)],
            },
            new TweakDef
            {
                Id = "npu-set-npu-workload-priority-interactive",
                Label = "Neural Processing: Prioritise Interactive NPU Workloads",
                Category = "Neural Processing Policy",
                Description =
                    "Sets NPUInteractivePriority=2 in NPU policy (value = HIGH, enum: 0=LOW, 1=NORMAL, 2=HIGH). Elevates the scheduling priority of interactive AI inference requests  (those triggered by direct user action: pressing a button, speaking a command, requesting a summary) above background inference tasks. When the NPU is partially loaded with background tasks, an interactive request preempts the queue. This ensures AI-powered features in the user's active workflow respond within acceptable latency bounds even when background AI tasks are running.",
                Tags = ["npu", "priority", "interactive", "scheduling", "ai"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Interactive AI requests get HIGH scheduling priority on the NPU. Background tasks may be preempted momentarily. Net user experience improvement for interactive AI features.",
                ApplyOps = [RegOp.SetDword(NpuKey, "NPUInteractivePriority", 2)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "NPUInteractivePriority")],
                DetectOps = [RegOp.CheckDword(NpuKey, "NPUInteractivePriority", 2)],
            },
            new TweakDef
            {
                Id = "npu-enable-npu-diagnostics",
                Label = "Neural Processing: Enable NPU Diagnostics for AI Failure Analysis",
                Category = "Neural Processing Policy",
                Description =
                    "Sets NPUDiagnosticsEnabled=1 in NPU policy. Enables the Windows NPU diagnostics subsystem to record NPU fault events, inference exceptions, model loading failures, and memory dump events to a local diagnostics buffer. When an AI inference pipeline fails (model crash, memory access violation, hardware NPU error), the diagnostics buffer captures root cause context. Required by IT teams that need to diagnose intermittent AI-related crashes or unexpected AI output changes that originate in NPU hardware or driver faults.",
                Tags = ["npu", "diagnostics", "ai", "fault-analysis", "debugging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NPU diagnostics data is collected locally. No additional data is sent to Microsoft beyond standard telemetry settings. Diagnostic buffer size is small (<10 MB). Required for investigating NPU-related AI failures.",
                ApplyOps = [RegOp.SetDword(NpuKey, "NPUDiagnosticsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NpuKey, "NPUDiagnosticsEnabled")],
                DetectOps = [RegOp.CheckDword(NpuKey, "NPUDiagnosticsEnabled", 1)],
            },
        ];
}
