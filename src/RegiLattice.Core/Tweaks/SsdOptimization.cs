namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// SSD-specific optimizations — disables unnecessary disk operations that degrade SSD lifespan,
/// enables TRIM, tunes write caching, and reduces unnecessary I/O on solid-state drives.
/// </summary>
internal static class SsdOptimization
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ssd-disable-superfetch",
            Label = "Disable Superfetch / SysMain on SSD",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the SysMain (Superfetch) service which pre-loads apps into memory. Unnecessary on SSDs and reduces write wear.",
            Tags = ["ssd", "performance", "superfetch", "sysmain"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\SysMain", "Start", 4)],
        },
        new TweakDef
        {
            Id = "ssd-disable-prefetch",
            Label = "Disable Prefetch on SSD",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Prefetch feature. On SSDs, random read is fast enough that prefetching provides no benefit.",
            Tags = ["ssd", "performance", "prefetch"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    3
                ),
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnableSuperfetch",
                    3
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                    "EnablePrefetcher",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ssd-disable-last-access-timestamp",
            Label = "Disable Last Access Timestamp (NTFS)",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables NTFS last-access-time updates, reducing unnecessary SSD writes on every file read.",
            Tags = ["ssd", "performance", "ntfs", "filesystem"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disablelastaccess", "1"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disablelastaccess", "0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["behavior", "query", "disablelastaccess"]);
                return stdout.Contains("1", StringComparison.OrdinalIgnoreCase) || stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-enable-trim",
            Label = "Enable TRIM (Automatic Optimization)",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Ensures TRIM is enabled for SSD garbage collection. TRIM informs the SSD which blocks are no longer in use.",
            Tags = ["ssd", "performance", "trim", "defrag"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disabledeletenotify", "0"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["behavior", "set", "disabledeletenotify", "1"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["behavior", "query", "disabledeletenotify"]);
                return stdout.Contains("= 0", StringComparison.Ordinal);
            },
        },
        new TweakDef
        {
            Id = "ssd-disable-defrag-schedule",
            Label = "Disable Scheduled Disk Defragmentation",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables the scheduled defragmentation task. SSDs should use TRIM, not defragmentation.",
            Tags = ["ssd", "performance", "defrag", "scheduled-task"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Disable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Enable-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ScheduledTask -TaskPath '\\Microsoft\\Windows\\Defrag\\' -TaskName 'ScheduledDefrag' -ErrorAction SilentlyContinue).State"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-disable-windows-search-indexing",
            Label = "Disable Windows Search Indexing",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Search Indexer service. Reduces write amplification on SSDs. Search will still work but without instant results.",
            Tags = ["ssd", "performance", "indexing", "search"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WSearch", "Start", 4)],
        },
        new TweakDef
        {
            Id = "ssd-enable-write-caching",
            Label = "Enable Write Caching on SSD",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables disk write caching for improved SSD write performance. Data is cached in volatile memory before being written to disk.",
            Tags = ["ssd", "performance", "write-cache", "disk"],
            SideEffects = "Risk of data loss on sudden power failure without UPS.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-Disk | Where-Object { $_.BusType -ne 'USB' } | ForEach-Object { "
                        + "Set-StorageSetting -NewDiskPolicy OnlineAll -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ssd-disable-hibernation-ssd",
            Label = "Disable Hibernation (SSD Wear Reduction)",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables hibernation to avoid writing full RAM contents to SSD (hiberfil.sys). Reduces write wear and frees disk space equal to RAM size.",
            Tags = ["ssd", "performance", "hibernation", "disk-space"],
            SideEffects = "Hibernation and Fast Startup will be unavailable.",
            ApplyAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "off"]),
            RemoveAction = _ => ShellRunner.Run("powercfg.exe", ["-h", "on"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("Test-Path \"$env:SystemDrive\\hiberfil.sys\"");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-disable-8dot3-names",
            Label = "Disable 8.3 Short File Names (NTFS)",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables legacy 8.3 short filename generation in NTFS. Reduces overhead on every file creation.",
            Tags = ["ssd", "performance", "ntfs", "filesystem"],
            ApplyAction = _ => ShellRunner.Run("fsutil.exe", ["8dot3name", "set", "1"]),
            RemoveAction = _ => ShellRunner.Run("fsutil.exe", ["8dot3name", "set", "0"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("fsutil.exe", ["8dot3name", "query"]);
                return stdout.Contains("1", StringComparison.Ordinal) || stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ssd-increase-ntfs-memory-usage",
            Label = "Increase NTFS Memory Usage (Paged Pool)",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases NTFS paged pool memory usage for better file system performance. Trades RAM for reduced disk I/O.",
            Tags = ["ssd", "performance", "ntfs", "memory"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsMemoryUsage", 2)],
        },
        new TweakDef
        {
            Id = "ssd-large-system-cache",
            Label = "Enable Large System Cache",
            Category = "SSD Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures Windows to use a large system cache, improving file system performance at the cost of higher memory usage.",
            Tags = ["ssd", "performance", "cache", "memory"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 1)],
        },
    ];
}
