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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "DisablePagingExecutive", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "ClearPageFileAtShutdown", 1)],
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
    ];
}
