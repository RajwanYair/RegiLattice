namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from SsdOptimization.cs ────────────────────────────────────────
internal static class SsdOptimization
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ssd-disable-last-access-timestamp",
            Label = "Disable Last Access Timestamp (NTFS)",
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Id = "ssd-enable-write-caching",
            Label = "Enable Write Caching on SSD",
            Category = "Storage",
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
            Category = "Storage",
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
            Category = "Storage",
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
            Id = "ssd-disable-boot-trace",
            Label = "Disable Boot Trace Logging",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables boot trace diagnostic logging that writes to disk during startup.",
            Tags = ["ssd", "performance", "boot", "trace", "io"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot", "Start", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot", "Start", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\WMI\Autologger\ReadyBoot", "Start", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-ntfs-compression",
            Label = "Disable NTFS Compression (System Drive)",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS compression on the system drive. Compression adds CPU overhead and provides minimal benefit on fast SSDs.",
            Tags = ["ssd", "performance", "ntfs", "compression", "cpu"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableCompression", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableCompression")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableCompression", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-ntfs-encryption",
            Label = "Disable NTFS Encryption Paging",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NTFS encrypted paging file. Reduces I/O overhead from encrypting page file writes.",
            Tags = ["ssd", "performance", "ntfs", "encryption", "paging"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsEncryptPagingFile", 0)],
        },
        new TweakDef
        {
            Id = "ssd-set-io-priority-normal",
            Label = "Set Default I/O Priority to Normal",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default I/O priority for all processes to Normal. Prevents I/O starvation from low-priority background tasks.",
            Tags = ["ssd", "performance", "io-priority", "scheduling"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl", "ConvertibleSlateMode", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl", "ConvertibleSlateMode")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\PriorityControl", "ConvertibleSlateMode", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-readyboost",
            Label = "Disable ReadyBoost Service",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the ReadyBoost service which uses USB flash drives as cache. Useless on systems with SSDs.",
            Tags = ["ssd", "performance", "readyboost", "service", "usb"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\rdyboost", "Start", 4)],
        },
        new TweakDef
        {
            Id = "ssd-disable-disk-perf-counters",
            Label = "Disable Disk Performance Counters",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables disk performance counters to reduce I/O overhead. Disable only if you don't monitor disk performance.",
            Tags = ["ssd", "performance", "counters", "io", "monitoring"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk", "Start", 2)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\PerfDisk", "Start", 4)],
        },
        // ── Sprint 20 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "ssd-disable-link-power-management",
            Label = "Disable AHCI Link Power Management",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AHCI Link Power Management (HIPM/DIPM) to prevent SSD latency spikes from power state transitions.",
            Tags = ["ssd", "performance", "power", "ahci"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableHIPM", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableHIPM")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableHIPM", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-dipm",
            Label = "Disable Device-Initiated Power Management",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DIPM (Device-Initiated Power Management) on SATA SSDs. Prevents the SSD from entering low-power states that add latency.",
            Tags = ["ssd", "performance", "power", "dipm"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableDIPM", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableDIPM")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device", "EnableDIPM", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-idle-power-timeout",
            Label = "Disable Disk Idle Power Timeout",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the disk idle timeout to 0, preventing SSDs from entering sleep mode. Eliminates wake-up latency spikes.",
            Tags = ["ssd", "performance", "power", "timeout"],
            RegistryKeys =
            [
                $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
                    "ValueMax",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
                    "ValueMax"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\0012ee47-9041-4b5d-9b77-535fba8b1442\6738e2c4-e8a5-4a42-b16a-e040e769756e",
                    "ValueMax",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "ssd-disable-log-file-flush",
            Label = "Reduce NTFS Log File Flush Frequency",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Reduces the frequency of NTFS journal log file flushes. Decreases write amplification on SSDs at a small risk of data on power loss.",
            Tags = ["ssd", "ntfs", "log", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLogfileFlush", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLogfileFlush")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLogfileFlush", 1)],
        },
        new TweakDef
        {
            Id = "ssd-enable-volatile-write-cache",
            Label = "Enable Volatile Write Cache on SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables volatile write caching on SSD controller. Improves sequential write performance but data may be lost on sudden power loss.",
            Tags = ["ssd", "write-cache", "performance", "risk"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Enum\SCSI"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCache", 1)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCache", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\disk", "EnableWriteCache", 1)],
        },
        new TweakDef
        {
            Id = "ssd-indexer-low-priority-io",
            Label = "Run Windows Search Indexer at Low I/O Priority",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LowPriorityIO=1 in the Windows Search gatherer parameters. The indexer uses background I/O priority, reducing I/O contention with interactive applications on SSDs.",
            Tags = ["ssd", "search", "indexer", "io-priority", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex", "LowPriorityIO", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex", "LowPriorityIO")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search\Gather\Windows\SystemIndex", "LowPriorityIO", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-boot-auto-layout",
            Label = "Disable Boot File Auto-Layout Optimisation",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableAutoLayout=0. On SSDs the auto-layout rearrangement of boot files provides no latency benefit because seek time is negligible. Eliminates the post-defrag layout phase.",
            Tags = ["ssd", "boot", "auto-layout", "defrag", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout", "EnableAutoLayout", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout", "EnableAutoLayout")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\OptimalLayout", "EnableAutoLayout", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-rac-sampling",
            Label = "Disable Reliability Activity Centre Sampling",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GeneralSamplingEnabled=0 in the RAC registry key. Stops the Reliability Analysis Component from periodically sampling system activity and writing entries to the RAC database on the SSD.",
            Tags = ["ssd", "reliability", "rac", "wear", "writes"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC", "GeneralSamplingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC", "GeneralSamplingEnabled")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Reliability Analysis\RAC", "GeneralSamplingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "ssd-ntfs-force-nonpaged-pool",
            Label = "Force NTFS Metadata into Non-Paged Pool",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NtfsForceNonPagedPoolAllocation=1. Keeps NTFS internal metadata structures in non-paged pool memory, eliminating paging I/O on the SSD for the file system's own working set.",
            Tags = ["ssd", "ntfs", "memory", "paged-pool", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsForceNonPagedPoolAllocation", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsForceNonPagedPoolAllocation")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsForceNonPagedPoolAllocation", 1)],
        },
        new TweakDef
        {
            Id = "ssd-disable-smb-read-ahead",
            Label = "Disable SMB Read-Ahead for SSD",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ReadAheadThreshold=0 in LanmanWorkstation parameters. Disables SMB client read-ahead prefetching. On SSDs random I/O is as fast as sequential, so pre-reading data wastes write bandwidth and cache without benefit.",
            Tags = ["ssd", "smb", "read-ahead", "network", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "ReadAheadThreshold", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "ReadAheadThreshold")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "ReadAheadThreshold", 0)],
        },
        new TweakDef
        {
            Id = "ssd-search-no-backoff-busy",
            Label = "Disable Windows Search I/O Backoff When Disk Busy",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BackOffIfDiskBusy=0 in Windows Search. By default the indexer backs off when disk utilisation is high. On SSDs concurrent I/O is fully supported; disabling backoff lets indexing keep pace.",
            Tags = ["ssd", "search", "indexer", "io", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows Search"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search", "BackOffIfDiskBusy", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows Search", "BackOffIfDiskBusy")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows Search", "BackOffIfDiskBusy", 0)],
        },
        new TweakDef
        {
            Id = "ssd-disable-auto-volume-mount",
            Label = "Disable Automatic New-Volume Mounting",
            Category = "Storage",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoAutoMount=1 in the MountMgr parameters. Prevents Windows from automatically assigning drive letters and mounting new storage volumes. Avoids unexpected I/O and AutoRun triggers when USB drives are inserted.",
            Tags = ["ssd", "mount", "volume", "autorun", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr", "NoAutoMount", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr", "NoAutoMount")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\MountMgr", "NoAutoMount", 1)],
        },
    ];
}
