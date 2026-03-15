namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Memory and virtual memory tweaks — page file, working set, large pages, and memory management.
/// </summary>
internal static class MemoryOptimization
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "mem-disable-paging-executive",
            Label = "Keep Kernel in RAM (Disable Paging Executive)",
            Category = "Memory",
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
            Category = "Memory",
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
            Id = "mem-clear-pagefile-on-shutdown",
            Label = "Clear Page File at Shutdown",
            Category = "Memory",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Zeros out the page file on shutdown. Prevents sensitive data from persisting in the page file.",
            Tags = ["memory", "security", "privacy"],
            SideEffects = "Shutdown takes slightly longer.",
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "ClearPageFileAtShutdown",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-iot-registry-quota",
            Label = "Increase Registry Size Limit",
            Category = "Memory",
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
            Id = "mem-optimize-svchosts",
            Label = "Increase Svchost Split Threshold",
            Category = "Memory",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the SvcHostSplitThresholdInKB to a high value so Windows groups services into fewer svchost processes. Saves RAM.",
            Tags = ["memory", "performance", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "SvcHostSplitThresholdInKB", 67108864)],
        },
        new TweakDef
        {
            Id = "mem-disable-memory-compression",
            Label = "Disable Memory Compression",
            Category = "Memory",
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
            Category = "Memory",
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
            Id = "mem-disable-prefetch-boost",
            Label = "Reduce Prefetch Memory Usage",
            Category = "Memory",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces Prefetch/Superfetch memory consumption by lowering EnablePrefetcher to boot-only mode.",
            Tags = ["memory", "performance", "prefetch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "mem-set-io-page-lock-limit",
            Label = "Set I/O Page Lock Limit (64 MB)",
            Category = "Memory",
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
            Category = "Memory",
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
            Category = "Memory",
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
            Category = "Memory",
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
            Category = "Memory",
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
        new TweakDef
        {
            Id = "mem-disable-superfetch-service",
            Label = "Set SysMain (Superfetch) Service to Manual",
            Category = "Memory",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the SysMain service (Superfetch) to manual start. Reduces memory and I/O overhead on SSD systems.",
            Tags = ["memory", "performance", "ssd", "service", "superfetch"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "mem-enable-large-pages",
            Label = "Enable Large Pages for Performance",
            Category = "Memory",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the system to prefer large memory pages (2 MB) which improves performance for memory-intensive applications.",
            Tags = ["memory", "performance", "large-pages", "database"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride",
                    3
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverrideMask",
                    3
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverrideMask"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "FeatureSettingsOverride",
                    3
                ),
            ],
        },
    ];
}
