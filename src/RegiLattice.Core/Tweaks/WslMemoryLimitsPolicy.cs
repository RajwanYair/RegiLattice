// RegiLattice.Core — Tweaks/WslMemoryLimitsPolicy.cs
// WSL 2 virtual machine memory allocation, swap, and CPU resource limit Group Policy controls (Sprint 609).
// Category: "WSL Memory Limits Policy" | Slug: wslmemlim
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\Lxss\Memory

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WslMemoryLimitsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Memory";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wslmemlim-set-max-wsl-vm-memory-4gb",
            Label = "WSL Memory: Cap WSL VM Maximum Memory Allocation to 4 GB",
            Category = "WSL Memory Limits Policy",
            Description = "Sets MaxVmMemoryMB=4096 in Lxss Memory policy. Limits the maximum amount of host RAM the WSL 2 Hyper-V virtual machine can allocate to 4 GB, preventing WSL workloads from consuming the majority of the host system's physical memory. " +
                "By default, WSL 2 can claim up to 50% of host RAM (up to 8 GB total). On workstations with 16–32 GB RAM, a developer building a large project in WSL (e.g., a Linux kernel build, a large Rust project, or a Docker image layer operation) can cause the Windows desktop to become unresponsive due to memory pressure. A 4 GB cap ensures the host OS retains sufficient memory for responsive interactive use.",
            Tags = ["wsl", "memory", "vm", "performance", "limits"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL VM limited to 4 GB RAM; prevents WSL workloads from starving the Windows desktop of memory.",
            ApplyOps = [RegOp.SetDword(Key, "MaxVmMemoryMB", 4096)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxVmMemoryMB")],
            DetectOps = [RegOp.CheckDword(Key, "MaxVmMemoryMB", 4096)],
        },
        new TweakDef
        {
            Id = "wslmemlim-set-max-wsl-cpu-cores-4",
            Label = "WSL Memory: Limit WSL VM to 4 Virtual CPU Cores",
            Category = "WSL Memory Limits Policy",
            Description = "Sets MaxVmProcessors=4 in Lxss Memory policy. Caps the number of virtual CPU cores visible to the WSL 2 VM to 4, limiting the maximum parallelism available to Linux workloads to prevent CPU starvation of host Windows processes. " +
                "WSL 2 inherits all host CPU cores by default. On a 12-core/24-thread development workstation, a Linux build job running make -j24 can saturate all CPU cores, causing Windows UI, background services, and other processes to become CPU-starved. Capping WSL at 4 cores ensures the OS retains burst capacity for interactive workloads, antivirus scans, and system services.",
            Tags = ["wsl", "cpu", "vm", "cores", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL VM limited to 4 vCPU; parallel Linux build jobs cannot saturate all host CPU cores.",
            ApplyOps = [RegOp.SetDword(Key, "MaxVmProcessors", 4)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxVmProcessors")],
            DetectOps = [RegOp.CheckDword(Key, "MaxVmProcessors", 4)],
        },
        new TweakDef
        {
            Id = "wslmemlim-set-swap-file-size-2gb",
            Label = "WSL Memory: Set WSL VM Swap File Size to 2 GB",
            Category = "WSL Memory Limits Policy",
            Description = "Sets VmSwapFileSizeMB=2048 in Lxss Memory policy. Sets the WSL 2 VM's swap file (used when VM RAM is exhausted) to a fixed 2 GB size, preventing the swap file from growing unboundedly on the Windows host disk. " +
                "The WSL 2 VM swap file is created dynamically on the Windows NTFS volume. Without a size cap, memory-intensive Linux workloads can cause the swap file to expand to match the available NTFS free space — effectively allowing WSL to consume the entire host disk as virtual memory. A 2 GB fixed swap cap provides a safety margin for legitimate memory overcommit while preventing disk exhaustion.",
            Tags = ["wsl", "swap", "memory", "disk-space", "limits"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL swap file fixed at 2 GB; prevents WSL virtual memory usage from consuming all available host disk space.",
            ApplyOps = [RegOp.SetDword(Key, "VmSwapFileSizeMB", 2048)],
            RemoveOps = [RegOp.DeleteValue(Key, "VmSwapFileSizeMB")],
            DetectOps = [RegOp.CheckDword(Key, "VmSwapFileSizeMB", 2048)],
        },
        new TweakDef
        {
            Id = "wslmemlim-enable-wsl-memory-reclaim-gradual",
            Label = "WSL Memory: Enable Gradual Memory Reclaim from VM to Host",
            Category = "WSL Memory Limits Policy",
            Description = "Sets EnableGradualMemoryReclaim=1 in Lxss Memory policy. Instructs the WSL 2 hypervisor to periodically reclaim unused virtual machine memory pages back to the Windows host pool using the gradual reclaim algorithm, rather than holding memory until the VM terminates. " +
                "Without memory reclaim enabled, WSL 2 acquires RAM as needed and holds it indefinitely, even after Linux processes that consumed the memory have exited. This means that after a large Linux build job completes, the RAM it consumed is not returned to the Windows pool, causing the host to appear low on memory. Gradual reclaim enables the WSL Hyper-V balloon driver to return idle pages to Windows.",
            Tags = ["wsl", "memory", "reclaim", "vm", "balloon"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "WSL VM memory automatically returned to host when Linux processes exit; prevents post-build RAM starvation.",
            ApplyOps = [RegOp.SetDword(Key, "EnableGradualMemoryReclaim", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableGradualMemoryReclaim")],
            DetectOps = [RegOp.CheckDword(Key, "EnableGradualMemoryReclaim", 1)],
        },
        new TweakDef
        {
            Id = "wslmemlim-disable-wsl-swap-file",
            Label = "WSL Memory: Disable WSL VM Swap File Creation",
            Category = "WSL Memory Limits Policy",
            Description = "Sets DisableVmSwapFile=1 in Lxss Memory policy. Prevents the WSL 2 virtual machine from creating a swap file on the Windows host disk. With no swap, the Linux VM operates in a pure in-memory mode and Linux processes that exhaust available VM memory will be OOM-killed rather than swap-paging. " +
                "The WSL swap file is a write-capable artifact on the Windows NTFS volume. Data written to Linux virtual memory (including sensitive in-memory data structures like encryption keys that are paged out under memory pressure) is persisted in a cleartext swap file on Windows disk. Disabling swap eliminates this data-at-rest exposure channel, at the cost of Linux OOM-kill risk for memory-intensive workloads.",
            Tags = ["wsl", "swap", "memory", "data-at-rest", "oom"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "No WSL swap file; sensitive paged-out VM memory not persisted to Windows disk; OOM-kill for memory-intensive Linux workloads.",
            ApplyOps = [RegOp.SetDword(Key, "DisableVmSwapFile", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVmSwapFile")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVmSwapFile", 1)],
        },
        new TweakDef
        {
            Id = "wslmemlim-enable-vm-page-reporting",
            Label = "WSL Memory: Enable VM Page Reporting for Efficient Host Memory Return",
            Category = "WSL Memory Limits Policy",
            Description = "Sets EnableVmPageReporting=1 in Lxss Memory policy. Enables the Hyper-V VM page reporting guest protocol within the WSL 2 VM, allowing the Linux guest to proactively report free memory pages to the host hypervisor for immediate host memory pool reuse. " +
                "Page reporting is a more aggressive memory return mechanism than the balloon driver; while balloon reclaim waits for host memory pressure, page reporting proactively marks guest-free pages as available to the host. This results in faster and more complete memory return after burst WSL workloads, improving host memory availability for concurrent Windows applications.",
            Tags = ["wsl", "memory", "page-reporting", "hyperv", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL VM proactively reports free pages to host; faster host memory recovery after WSL burst workloads.",
            ApplyOps = [RegOp.SetDword(Key, "EnableVmPageReporting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableVmPageReporting")],
            DetectOps = [RegOp.CheckDword(Key, "EnableVmPageReporting", 1)],
        },
        new TweakDef
        {
            Id = "wslmemlim-set-vm-memory-reclaim-idle-threshold-5min",
            Label = "WSL Memory: Set VM Memory Reclaim Idle Threshold to 5 Minutes",
            Category = "WSL Memory Limits Policy",
            Description = "Sets VmMemoryReclaimIdleThresholdMin=5 in Lxss Memory policy. Sets the idle timeout after which the WSL 2 Hyper-V host will begin reclaiming memory pages from an idle WSL VM to 5 minutes. " +
                "The default idle threshold before WSL memory reclaim begins is typically 10–15 minutes. In enterprise environments where developers run WSL sessions intermittently (editing code in Windows, compiling in WSL, back to editing), the 5-minute threshold ensures that memory allocated during compile jobs is returned to the host within 5 minutes of the WSL VM becoming idle, rather than holding the memory for the duration of the edit cycle.",
            Tags = ["wsl", "memory", "reclaim", "idle", "threshold"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL VM memory reclaimed after 5 min idle; faster host memory recovery for intermittent WSL usage patterns.",
            ApplyOps = [RegOp.SetDword(Key, "VmMemoryReclaimIdleThresholdMin", 5)],
            RemoveOps = [RegOp.DeleteValue(Key, "VmMemoryReclaimIdleThresholdMin")],
            DetectOps = [RegOp.CheckDword(Key, "VmMemoryReclaimIdleThresholdMin", 5)],
        },
        new TweakDef
        {
            Id = "wslmemlim-disable-kernel-samepage-merging",
            Label = "WSL Memory: Disable Kernel Same-Page Merging in WSL VM",
            Category = "WSL Memory Limits Policy",
            Description = "Sets DisableKernelSamepageMerging=1 in Lxss Memory policy. Disables the Linux kernel's KSM (Kernel Same-page Merging) memory deduplication feature in the WSL 2 VM, which periodically scans VM memory for identical pages and merges them into copy-on-write shared pages. " +
                "KSM is a known side-channel: the merge/de-merge timing of identical pages can be used to detect whether a particular secret value (e.g., a cryptographic key) exists in another process's memory. Research has demonstrated KSM-based cross-process memory probing exploits. Disabling KSM removes this timing side-channel within the WSL VM's memory subsystem.",
            Tags = ["wsl", "memory", "ksm", "side-channel", "crypto"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "KSM disabled in WSL VM; eliminates KSM timing side-channel; slight increase in Linux VM memory usage.",
            ApplyOps = [RegOp.SetDword(Key, "DisableKernelSamepageMerging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableKernelSamepageMerging")],
            DetectOps = [RegOp.CheckDword(Key, "DisableKernelSamepageMerging", 1)],
        },
        new TweakDef
        {
            Id = "wslmemlim-enable-memory-pressure-notifications",
            Label = "WSL Memory: Enable Host Memory Pressure Notifications to WSL VM",
            Category = "WSL Memory Limits Policy",
            Description = "Sets EnableMemoryPressureNotifications=1 in Lxss Memory policy. Enables the WSL 2 hypervisor to send memory pressure notifications into the Linux guest when the Windows host is experiencing memory pressure, allowing the Linux kernel to invoke its own memory pressure handlers (cgroup high/low events, transparent huge page compaction) proactively. " +
                "Without pressure notifications, the Linux VM has no visibility into host memory pressure and will continue normal memory allocation, worsening host memory pressure. With notifications enabled, the guest can perform early memory reclaim before the hypervisor is forced to balloon-reclaim pages, resulting in more cooperative memory sharing between the WSL VM and Windows host processes.",
            Tags = ["wsl", "memory", "pressure", "notification", "cgroup"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "WSL VM receives host memory pressure signals; Linux guest proactively reclaims memory, reducing host contention.",
            ApplyOps = [RegOp.SetDword(Key, "EnableMemoryPressureNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMemoryPressureNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMemoryPressureNotifications", 1)],
        },
        new TweakDef
        {
            Id = "wslmemlim-disable-large-page-allocation",
            Label = "WSL Memory: Disable Huge Page Allocation in WSL VM",
            Category = "WSL Memory Limits Policy",
            Description = "Sets DisableLargePageAllocation=1 in Lxss Memory policy. Disables transparent huge page (THP) allocation within the WSL 2 VM's Linux kernel, preventing the VM from allocating 2 MB memory pages instead of 4 KB pages. " +
                "Transparent huge pages improve Linux application throughput for memory-intensive workloads but make host memory reclaim significantly less efficient. A 2 MB THP page cannot be reclaimed until all 512 sub-pages are free simultaneously, causing huge pages to become 'locked' memory that resists balloon and page-report reclaim. Disabling THP makes WSL VM memory more granularly reclaimable, improving host memory return latency.",
            Tags = ["wsl", "memory", "huge-pages", "thp", "reclaim"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "THP disabled in WSL VM; slightly lower Linux throughput for memory-intensive workloads but significantly faster host memory reclaim.",
            ApplyOps = [RegOp.SetDword(Key, "DisableLargePageAllocation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLargePageAllocation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLargePageAllocation", 1)],
        },
    ];
}
