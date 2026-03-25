// RegiLattice.Core — Tweaks/WindowsPerformancePolicy.cs
// Sprint 356: Windows Performance Policy tweaks (10 tweaks)
// Category: "Windows Performance Policy" | Slug: wnperf
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Performance

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsPerformancePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Performance";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wnperf-restrict-background-activity",
            Label = "Restrict Background Application Activity Through Performance Policy",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting background application activity through performance policy limits the CPU and I/O resources that background and suspended applications can consume improving foreground application responsiveness. Background activity restrictions ensure that non-interactive applications do not consume system resources at the expense of user-facing processes. Enterprise workstations running data analytics batch jobs or synchronization tasks in the background benefit from policies that prioritize interactive work. Resource limitations on background activity also constrain the impact of malware that attempts to use background execution contexts for long-running operations like encryption or data exfiltration. Performance policy restrictions apply to applications running in the background execution manager context which covers many Windows Store and background service applications. Organizations should test background activity restrictions to verify that necessary background operations like antivirus scanning and backup software continue to function.",
            Tags = ["performance", "background-activity", "resource-management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictBackgroundActivity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictBackgroundActivity")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictBackgroundActivity", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-enable-cpu-priority-boost",
            Label = "Enable CPU Priority Boosting for Foreground Interactive Applications",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "CPU priority boosting for foreground applications gives interactive applications preferential CPU scheduling over background processes improving user experience for interactive computing tasks. Windows automatically boosts the scheduling priority of the foreground application to ensure responsive input handling but policy controls can extend this boost to all interactive applications. Priority boosting ensures that user-visible applications remain responsive even when background tasks are consuming significant CPU resources. Security tools like real-time antivirus engines use background priority to avoid impacting foreground performance and the foreground priority boost ensures that interactive work retains precedence. Organizations should verify that critical services that run in the background are not negatively impacted by foreground priority boosts reducing their ability to complete time-sensitive work. Performance monitoring should verify that the priority boost achieves the intended improvement in interactive responsiveness.",
            Tags = ["performance", "cpu-priority", "foreground-application", "scheduling", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableForegroundPriorityBoost", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableForegroundPriorityBoost")],
            DetectOps = [RegOp.CheckDword(Key, "EnableForegroundPriorityBoost", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-configure-memory-usage-policy",
            Label = "Configure System Memory Usage Policy for Balanced Performance",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Memory usage policy configuration controls how Windows manages physical memory allocation between application working sets page file usage and system cache to optimize performance for the primary system role. Server systems benefit from configuring memory policy to prioritize system cache and services while workstations benefit from policies that prioritize application working sets. Appropriate memory policy configuration reduces the frequency of hard page faults where data must be read from disk rather than satisfied from physical memory. Memory policy can be tuned to reduce paging activity on systems with adequate memory by configuring more aggressive working set retention. Organizations should evaluate the primary workload of each system type when setting memory policy to ensure the configuration matches the workload profile. Memory configuration changes should be validated through performance baseline comparison to verify improvement in the target metrics.",
            Tags = ["performance", "memory-management", "working-set", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMemoryUsagePolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMemoryUsagePolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMemoryUsagePolicy", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-disable-animated-windows-effects",
            Label = "Disable Animated Window Effects for Improved System Performance",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Animated window effects including window minimize maximize fade and slide animations consume GPU resources and cause perceived latency in window operations that can be eliminated through policy without user impact on most enterprise workstations. Disabling animations through performance policy provides consistent visual performance settings across all managed workstations. On systems with limited GPU resources or dedicated GPU resources needed for business applications animation effects compete for GPU time with productive work. Disabling window animations is particularly beneficial for virtual desktop infrastructure environments where GPU resources are shared across many VM sessions. Policy-based animation control ensures consistent application of performance settings without relying on individual users to configure visual effects manually. The performance impact of disabling animations varies by hardware but is most significant on integrated graphics systems where CPU and GPU share memory bandwidth.",
            Tags = ["performance", "animations", "visual-effects", "gpu", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAnimatedEffects", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAnimatedEffects")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAnimatedEffects", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-configure-disk-io-scheduling",
            Label = "Configure Disk I/O Scheduling for Application vs System Service Balance",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Disk I/O scheduling policy controls how Windows prioritizes disk I/O requests between application I/O background I/O and system service I/O ensuring that critical I/O operations are completed with appropriate latency. I/O scheduling configuration affects how quickly applications can write logs read data files and access databases compared to background operations like disk defragmentation and search indexing. Systems that run intensive background disk I/O operations benefit from I/O scheduling policy that prevents background I/O from saturating disk bandwidth needed by foreground applications. NVMe SSD systems have lower scheduling overhead than traditional spinning disks but can still benefit from I/O prioritization for mixed workloads with both interactive and batch I/O. Organizations should profile disk I/O patterns across their fleet to identify systems where I/O contention is causing application performance degradation that policy can mitigate. I/O scheduling policy is particularly relevant for database servers file servers and storage-intensive workloads.",
            Tags = ["performance", "disk-io", "scheduling", "storage", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableDiskIOScheduling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDiskIOScheduling")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDiskIOScheduling", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-restrict-startup-program-execution",
            Label = "Restrict Startup Program Execution to Approved Application List",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Startup program execution restrictions prevent unauthorized applications from adding themselves to Windows startup locations and executing automatically at user login increasing both security and performance. Malware and potentially unwanted applications frequently add themselves to startup locations to maintain persistence and execute at each user login. Restricting startup execution to an approved list prevents both unauthorized persistence establishment and the performance degradation from accumulating startup programs over time. Startup program restrictions work best when combined with Software Restriction Policies or AppLocker to prevent programs from loading regardless of how they are invoked. Organizations should define the startup program allowlist based on applications that have legitimate business requirements for startup execution and review and trim this list periodically. User-controlled startup programs should be completely disabled for standard users who should not have the ability to modify which programs run automatically.",
            Tags = ["performance", "startup-programs", "persistence-prevention", "allowlist", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictStartupPrograms", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictStartupPrograms")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictStartupPrograms", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-configure-network-throttling",
            Label = "Configure Network Bandwidth Throttling for Background Update Operations",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Network bandwidth throttling for background update operations limits the bandwidth consumed by Windows Update delivery optimization and other background network consumers preventing them from saturating network connections during productive hours. Without throttling Windows Update delivery optimization and peer-to-peer update distribution can consume significant network bandwidth that impacts interactive work and business applications. Policy-based throttling configurations can restrict background network usage to specific percentages of available bandwidth and can vary restrictions based on time of day to allow unrestricted updates during off-hours. Delivery optimization settings should be configured to prioritize enterprise update caching servers over internet downloads where available reducing upstream bandwidth consumption. Organizations with thin WAN links benefit most from network throttling policies that prevent update traffic from impacting business-critical applications on constrained bandwidth connections. Background network usage should be monitored to verify that throttling is correctly limiting consumption and that updates are still completing within required timeframes.",
            Tags = ["performance", "network-throttling", "background-updates", "bandwidth", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableNetworkThrottling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNetworkThrottling")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNetworkThrottling", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-enable-prefetch-optimization",
            Label = "Enable Prefetch Optimization for Frequently Used Application Launch",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Prefetch optimization allows Windows to pre-load frequently used application data and code from disk into memory before the application is launched improving perceived application startup performance. Prefetch monitoring tracks which application data is accessed during startup and creates prefetch files that inform future startup pre-loading operations. The prefetch system improves application launch time by ensuring that frequently used executable pages and data files are in memory before they are needed. Prefetch optimization is most effective on spinning disk systems where sequential pre-reading is significantly faster than random access. On SSD systems the performance benefit is smaller but prefetch still provides improvement for large applications with slow-loading modules. Organizations should ensure that prefetch is enabled on production workstations and that the prefetch data directory has adequate storage space for the prefetch files accumulated by the applications in use.",
            Tags = ["performance", "prefetch", "application-launch", "startup", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnablePrefetchOptimization", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePrefetchOptimization")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePrefetchOptimization", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-configure-power-performance-balance",
            Label = "Configure Power and Performance Balance Policy for Enterprise Workloads",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Power and performance balance policy controls the tradeoff between computing performance and energy consumption ensuring that enterprise workstations deliver appropriate performance for business workloads. High-performance power plans increase processor performance states and disable power saving features that can reduce responsiveness for interactive and compute-intensive applications. Balanced power plans provide a reasonable compromise for most enterprise workloads adjusting performance states dynamically based on workload demand. Performance-critical applications like database servers engineering applications and real-time processing systems benefit from high-performance power plan configurations. Organizations should match power plan configuration to the workload profile of each system type rather than applying a uniform policy across all systems. Power consumption monitoring can verify that the configured power plan achieves the intended energy consumption profile for server hosting environments.",
            Tags = ["performance", "power-plan", "energy", "workload-tuning", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ConfigurePowerPerformanceBalance", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ConfigurePowerPerformanceBalance")],
            DetectOps = [RegOp.CheckDword(Key, "ConfigurePowerPerformanceBalance", 1)],
        },
        new TweakDef
        {
            Id = "wnperf-enable-performance-audit-logging",
            Label = "Enable Performance Audit Logging for System Resource Utilization",
            Category = "Windows Performance Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Performance audit logging records system resource utilization events including CPU memory disk and network consumption providing data for capacity planning and performance anomaly detection. Performance data collected through audit logging enables historical analysis that identifies performance degradation trends before they become user-impacting problems. Security-relevant performance events such as sudden increases in CPU or memory consumption may indicate ongoing exploitation or malware execution. Performance audit data should be retained and analyzed alongside security event data to correlate performance anomalies with security incidents. Organizations should establish performance baselines for each system role to enable meaningful comparison of current performance against expected ranges. Performance audit logging data should be lightweight and targeted at high-value metrics that provide actionable insight without generating excessive log volume.",
            Tags = ["performance", "audit-logging", "resource-utilization", "baseline", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnablePerformanceAuditLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnablePerformanceAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnablePerformanceAuditLogging", 1)],
        },
    ];
}
