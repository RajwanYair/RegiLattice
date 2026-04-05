namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MemoryOptimization
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "mem-disable-paging-executive",
            Label = "Keep Kernel in RAM (Disable Paging Executive)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the kernel and drivers from being paged to disk. Requires 8 GB+ RAM.",
            Tags = ["memory", "performance", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "DisablePagingExecutive",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-enable-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Optimises the file system cache for server-like workloads (large file operations, databases).",
            Tags = ["memory", "performance", "cache", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-iot-registry-quota",
            Label = "Increase Registry Size Limit",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the registry size limit to allow larger registry hives (useful for machines with many tweaks/software).",
            Tags = ["memory", "registry", "system"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "RegistrySizeLimit", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "RegistrySizeLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "RegistrySizeLimit", unchecked((int)0xFFFFFFFF))],
        },
        new TweakDef
        {
            Id = "mem-disable-memory-compression",
            Label = "Disable Memory Compression",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables Windows memory compression. Frees CPU cycles on systems with ample RAM (16 GB+).",
            Tags = ["memory", "performance", "cpu"],
            SideEffects = "May increase page file usage on low-RAM systems.",
            ApplyAction = _ => ShellRunner.RunPowerShell("Disable-MMAgent -MemoryCompression -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Enable-MMAgent -MemoryCompression -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MMAgent).MemoryCompression");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "mem-set-second-level-data-cache",
            Label = "Set L2 Cache Size Hint (1024 KB)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the SecondLevelDataCache hint to 1024 KB to help Windows optimize memory management for modern CPUs.",
            Tags = ["memory", "performance", "cpu", "cache"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SecondLevelDataCache",
                    1024
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SecondLevelDataCache"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SecondLevelDataCache",
                    1024
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-io-page-lock-limit",
            Label = "Set I/O Page Lock Limit (64 MB)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the I/O page lock limit used for disk transfers. Improves large file copy performance.",
            Tags = ["memory", "performance", "io", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit", 67108864),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "IoPageLockLimit"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "IoPageLockLimit",
                    67108864
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-disable-page-combining",
            Label = "Disable Memory Page Combining",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables memory page combining (deduplication). Reduces CPU overhead on systems with enough RAM.",
            Tags = ["memory", "performance", "cpu"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Disable-MMAgent -PageCombining -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Enable-MMAgent -PageCombining -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MMAgent).PageCombining");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "mem-set-nonpaged-pool-limit",
            Label = "Set Non-Paged Pool Limit (256 MB)",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the non-paged pool limit. Prevents drivers from exhausting non-paged memory.",
            Tags = ["memory", "stability", "kernel", "drivers"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "NonPagedPoolSize",
                    268435456
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "NonPagedPoolSize"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "NonPagedPoolSize",
                    268435456
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-disable-trim-on-memory-pressure",
            Label = "Disable Working Set Trim on Memory Pressure",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents aggressive working set trimming when memory pressure increases. Keeps apps responsive.",
            Tags = ["memory", "performance", "working-set"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LowMemoryThreshold", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LowMemoryThreshold"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LowMemoryThreshold", 0),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-system-pages",
            Label = "Set System PTE Pages to Maximum",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets SystemPages to 0 (auto-maximum), allowing Windows to use the maximum number of PTE pages for system resources.",
            Tags = ["memory", "performance", "kernel", "pte"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SystemPages", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SystemPages")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SystemPages", 0),
            ],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "mem-set-session-pool-size",
            Label = "Optimize Session Pool Size",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the session paged pool to auto-tune (0) for optimal allocation based on available RAM and workload.",
            Tags = ["memory", "pool", "session", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SessionPoolSize", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SessionPoolSize"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SessionPoolSize", 0),
            ],
        },
        new TweakDef
        {
            Id = "mem-conservative-swap",
            Label = "Conservative Swap File Usage",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces Windows to exhaust physical RAM before using the page file, reducing disk I/O on systems with ample RAM.",
            Tags = ["memory", "swap", "pagefile", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ConservativeSwapfileUsage",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ConservativeSwapfileUsage"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ConservativeSwapfileUsage",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-dirty-page-threshold",
            Label = "Set System Cache Dirty Page Threshold",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the number of dirty pages the system cache can accumulate before flushing to disk, reducing bursty I/O.",
            Tags = ["memory", "cache", "performance", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SystemCacheDirtyPageThreshold",
                    256
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SystemCacheDirtyPageThreshold"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SystemCacheDirtyPageThreshold",
                    256
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-heap-decommit",
            Label = "Optimize Heap Decommit Free Block Threshold",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the threshold for heap manager to decommit free blocks, returning memory to the OS faster.",
            Tags = ["memory", "heap", "performance", "decommit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold", 262144),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "HeapDeCommitFreeBlockThreshold", 262144),
            ],
        },
        new TweakDef
        {
            Id = "mem-enable-pae",
            Label = "Enable Physical Address Extension",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures Physical Address Extension (PAE) is enabled, allowing 32-bit Windows to use Data Execution Prevention and more RAM.",
            Tags = ["memory", "pae", "security", "hardware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalAddressExtension",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalAddressExtension"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalAddressExtension",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-paged-pool-quota",
            Label = "Disable Per-Process Paged Pool Quota",
            Category = "System",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables per-process paged pool quota enforcement, allowing processes to use more paged pool memory when available.",
            Tags = ["memory", "pool", "quota", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolQuota", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolQuota"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagedPoolQuota", 0),
            ],
        },
        // ── merged from: Gpu.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "gpu-disable-nvidia-telemetry",
            Label = "Disable NVIDIA Telemetry",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables NVIDIA telemetry and usage data collection. Only applies if NVIDIA drivers are installed. Default: Enabled (opt-in). Recommended: Disabled.",
            Tags = ["gpu", "nvidia", "privacy", "telemetry"],
            IsApplicable = HardwareInfo.HasNvidiaGpu,
            ApplicabilityNote = "Requires NVIDIA GPU",
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID44231", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID64640", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID66610", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID44231"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID64640"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\FTS", "EnableRID66610"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client", "OptInOrOutPreference", 0)],
        },
        new TweakDef
        {
            Id = "gpu-nvidia-tdr-delay",
            Label = "Increase NVIDIA TDR Delay (8s)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the GPU TDR (Timeout Detection and Recovery) delay to 8 seconds. Prevents driver resets during heavy GPU workloads. Removal deletes the value, restoring the Windows default (2s). Default: 2s. Recommended: 8s.",
            Tags = ["gpu", "nvidia", "stability", "tdr"],
            IsApplicable = HardwareInfo.HasNvidiaGpu,
            ApplicabilityNote = "Requires NVIDIA GPU",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDelay", 8)],
        },
        new TweakDef
        {
            Id = "gpu-disable-gpu-preemption",
            Label = "Disable GPU Preemption (Low Latency)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables GPU preemption (EnablePreemption=0) for lower render latency. May improve frame times in GPU-bound scenarios but can affect multi-tasking and system responsiveness. Default: Enabled. Removal deletes the value.",
            Tags = ["gpu", "latency", "gaming", "preemption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler", "EnablePreemption", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-dx12-ultimate",
            Label = "Force DirectX 12 Ultimate",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces DirectX 12 mode for all compatible applications. Enables advanced features like mesh shaders and raytracing. Default: Auto. Recommended: Enabled for DX12-capable GPUs.",
            Tags = ["gpu", "directx", "dx12", "performance", "raytracing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ForceD3D12", 1)],
        },
        new TweakDef
        {
            Id = "gpu-wddm-scheduler",
            Label = "Optimize WDDM GPU Scheduler",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Optimizes WDDM flip queue length to 2 frames for reduced input latency. Trades slight throughput for lower frame queue depth. Default: 3. Recommended: 2 for competitive gaming.",
            Tags = ["gpu", "wddm", "flip-queue", "latency", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength", 2),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwFlipQueueLength", 2)],
        },
        new TweakDef
        {
            Id = "gpu-max-prerendered-frames",
            Label = "Set Max Pre-Rendered Frames to 1",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the flip queue size to 1, limiting pre-rendered frames. Reduces input lag at the cost of slightly lower throughput. Default: 3 frames. Recommended: 1 for competitive gaming.",
            Tags = ["gpu", "pre-rendered", "frames", "input-lag", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "FlipQueueSize", 1)],
        },
        new TweakDef
        {
            Id = "gpu-enable-dx12-async",
            Label = "Enable DirectX 12 Async Compute",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables D3D12 asynchronous command buffer reuse for improved GPU throughput. Most beneficial in DirectX 12 games with async compute shaders. Default: Not set. Recommended: Enabled for gaming.",
            Tags = ["gpu", "directx12", "async", "compute", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE", 1)],
        },
        new TweakDef
        {
            Id = "gpu-disable-shader-cache",
            Label = "Disable DirectX Shader Disk Cache",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the DirectX on-disk shader cache. Reduces disk I/O; useful in scenarios where fresh shader compilation is preferred or disk space is constrained. Default: Enabled.",
            Tags = ["gpu", "shader", "cache", "disk", "directx"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX", "ShaderCachePath", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-high-performance-power",
            Label = "Force GPU High Performance Power Plan",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces the GPU to maximum performance power mode. Disables GPU power saving. Default: adaptive.",
            Tags = ["gpu", "power", "performance", "high-performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power", "DefaultD3ColdSupported", 0)],
        },
        new TweakDef
        {
            Id = "gpu-disable-igpu-powersave",
            Label = "Disable iGPU Power Saving",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Intel integrated GPU power-saving features. Forces maximum iGPU performance. Default: power saving enabled.",
            Tags = ["gpu", "igpu", "intel", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "PlatformSupportMiracast", 0)],
        },
        new TweakDef
        {
            Id = "gpu-force-software-cursor",
            Label = "Force Software Cursor",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces software cursor rendering instead of hardware cursor. May fix cursor corruption issues on some GPUs. Default: hardware cursor.",
            Tags = ["gpu", "cursor", "software", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "EnableSWCursor", 1)],
        },
        new TweakDef
        {
            Id = "gpu-preemption-disable",
            Label = "Disable GPU Compute Preemption",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables GPU compute preemption at the DWM level. May improve GPGPU compute performance but can cause display hangs. Default: enabled.",
            Tags = ["gpu", "compute", "preemption", "dwm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "CompositionPolicy", 0)],
        },
        new TweakDef
        {
            Id = "gpu-wddm3-miracast",
            Label = "Disable Miracast (WDDM)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Miracast wireless display support at the driver level. Frees GPU resources used for Miracast. Default: enabled.",
            Tags = ["gpu", "miracast", "wddm", "wireless-display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Connect", "AllowProjectionToPC", 0)],
        },
        new TweakDef
        {
            Id = "gpu-tasks-gpu-priority",
            Label = "Boost GPU Priority for Games Profile",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GpuPriority=8 in the Multimedia SystemProfile Tasks\\Games key. Requests highest GPU scheduling priority for game processes via MMCSS. Default: 1 or 2.",
            Tags = ["gpu", "priority", "mmcss", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GpuPriority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "gpu-hw-sched-policy",
            Label = "Set WDDM Scheduler Policy to Batch",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SchedulerPolicy=2 (batch mode) in GraphicsDrivers. Batches GPU work submissions to reduce context-switch overhead. Default: preemptive.",
            Tags = ["gpu", "wddm", "scheduler", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "SchedulerPolicy", 2)],
        },
        new TweakDef
        {
            Id = "gpu-threading-optimization",
            Label = "Enable GPU Driver Threading Optimization",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ThreadedOptimizationFlags=1 in GraphicsDrivers to enable driver threading optimizations. Allows the GPU driver to use multiple CPU threads for command processing. Default: off.",
            Tags = ["gpu", "threading", "driver", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "ThreadedOptimizationFlags", 1)],
        },
        new TweakDef
        {
            Id = "gpu-idle-power-engine-timeout",
            Label = "Set GPU Engine Timeout for Idle Power",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrEngineTimeout=13 seconds in GraphicsDrivers. Controls how long the GPU engine can be unresponsive before reset recovery. Default: 2 seconds.",
            Tags = ["gpu", "tdr", "timeout", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout", 13)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrEngineTimeout", 13)],
        },
        new TweakDef
        {
            Id = "gpu-hdr-auto-color",
            Label = "Enable DWM Auto Color Management",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AutoColorManagement=1 in DWM registry key to enable automatic HDR/color management for connected displays that support it. Default: 0.",
            Tags = ["gpu", "hdr", "color", "dwm"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AutoColorManagement", 1)],
        },
        new TweakDef
        {
            Id = "gpu-tdr-limit-extend",
            Label = "Extend GPU TDR Limit Count to 10",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitCount=10 in GraphicsDrivers. Allows up to 10 TDR recoveries in 60 seconds before crashing. Useful for overclocked or compute workloads. Default: 5.",
            Tags = ["gpu", "tdr", "stability", "overclock"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount", 10)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitCount", 10)],
        },
        new TweakDef
        {
            Id = "gpu-multi-adapter-alt",
            Label = "Set Multi-GPU Alternate Frame Rendering",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UseNoPlatformUpdateMode=1 in GraphicsDrivers to hint drivers to avoid platform-specific update mode that may conflict with multi-GPU setups. Default: 0.",
            Tags = ["gpu", "multi-gpu", "adapter", "rendering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "UseNoPlatformUpdateMode", 1)],
        },
        new TweakDef
        {
            Id = "gpu-opengl-flip-interval",
            Label = "Set DirectDraw Flip Interval to 0",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets FlipInterval=0 in DirectDraw settings to allow immediate buffer flips without vertical sync wait. Reduces display-pipeline latency for OpenGL/DDraw apps. Default: 1.",
            Tags = ["gpu", "opengl", "directdraw", "vsync"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\DirectDraw", "FlipInterval", 0)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-level-recover",
            Label = "Set GPU TDR Level to Recover (No Bugcheck)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLevel=3 so Windows recovers the GPU engine after a Timeout Detection & Recovery (TDR) event without triggering a bugcheck. Improves stability for overclocked or demanding GPU workloads. Default: 3 on most systems.",
            Tags = ["gpu", "tdr", "stability", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLevel", 3)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-debugging-off",
            Label = "Disable GPU TDR Crash Dump Generation",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables TDR debug crash dump generation by setting TdrDebugging=0. Prevents large crash dumps when the GPU recovers from a timeout, reducing disk I/O overhead during recovery. Default: 0.",
            Tags = ["gpu", "tdr", "dump", "debugging"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDebugging", 0)],
        },
        new TweakDef
        {
            Id = "gpu-enable-vrr-optimize",
            Label = "Enable Windows 11 VRR Optimisation",
            Category = "Display",
            NeedsAdmin = false,
            CorpSafe = true,
            MinBuild = 22000,
            Description =
                "Enables the Variable Refresh Rate (VRR) optimisation in DWM on Windows 11. Allows the desktop compositor to leverage VRR/FreeSync/G-Sync for smoother UI rendering. Default: off (requires supported display).",
            Tags = ["gpu", "vrr", "freesync", "gsync", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "VrrOptimizeEnable", 1)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-ddi-delay",
            Label = "Set GPU TDR DDI Delay to 0 (Immediate)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrDdiDelay=0 so the DDI watchdog immediately detects when a DDI call exceeds the allowed time. Allows faster GPU error detection with less latency on recovery. Default: not set (uses kernel default).",
            Tags = ["gpu", "tdr", "ddi", "watchdog"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrDdiDelay", 0)],
        },
        new TweakDef
        {
            Id = "gpu-enable-hw-flip-queue",
            Label = "Enable GPU Hardware Flip Queue",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 19041,
            Description =
                "Enables the DirectX Graphics Kernel hardware flip queue via DxgkrnlEnableHwFlipQueue=1. Moves present queue management to hardware, reducing CPU involvement and frame delivery latency. Default: system-managed.",
            Tags = ["gpu", "flip-queue", "latency", "dx", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "DxgkrnlEnableHwFlipQueue", 1)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-limit-value",
            Label = "Increase GPU TDR Limit Count (Stability)",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitValue=60 to allow up to 60 GPU timeouts within the TdrLimitTime window before triggering a full system bugcheck. More tolerant under heavy GPU load or overclocking. Default: 5.",
            Tags = ["gpu", "tdr", "limit", "stability", "overclocking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitValue", 60)],
        },
        new TweakDef
        {
            Id = "gpu-set-tdr-limit-time",
            Label = "Extend GPU TDR Limit Time Window",
            Category = "Display",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TdrLimitTime=60 to extend the TDR limit counting window to 60 seconds. Combined with a higher TdrLimitValue this prevents bugchecks on systems that have occasional GPU hangs under load. Default: 60 (may vary).",
            Tags = ["gpu", "tdr", "limit", "time", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime", 60)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "TdrLimitTime", 60)],
        },
    ];
}
