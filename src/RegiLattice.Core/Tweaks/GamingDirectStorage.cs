#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class GamingDirectStorage
{
    private const string DxStoreKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectX\DirectStorage";
    private const string FsKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem";
    private const string DiskKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\disk";
    private const string StorNvmeKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\stornvme\Parameters";
    private const string GamesTaskKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dxstore-enable-gpu-decompression",
            Label = "Enable DirectStorage GPU Decompression",
            Category = "Gaming",
            Description =
                "Routes asset decompression to the GPU rather than CPU, enabling faster game loading on DirectStorage-compatible DX12 titles.",
            Tags = ["directstorage", "gaming", "gpu", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Reduces game loading times by offloading decompression to the GPU on supported hardware.",
            ApplyOps = [RegOp.SetDword(DxStoreKey, "UseGPUDecompression", 1)],
            RemoveOps = [RegOp.DeleteValue(DxStoreKey, "UseGPUDecompression")],
            DetectOps = [RegOp.CheckDword(DxStoreKey, "UseGPUDecompression", 1)],
        },
        new TweakDef
        {
            Id = "dxstore-increase-staging-buffer",
            Label = "Increase DirectStorage Staging Buffer to 32 MB",
            Category = "Gaming",
            Description = "Sets the DirectStorage staging buffer to 32 MB for higher throughput when streaming large game assets from NVMe SSDs.",
            Tags = ["directstorage", "gaming", "nvme", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Increases asset streaming throughput at the cost of 32 MB additional RAM.",
            ApplyOps = [RegOp.SetDword(DxStoreKey, "StagingBufferSize", 32)],
            RemoveOps = [RegOp.DeleteValue(DxStoreKey, "StagingBufferSize")],
            DetectOps = [RegOp.CheckDword(DxStoreKey, "StagingBufferSize", 32)],
        },
        new TweakDef
        {
            Id = "dxstore-disable-hdd-compat-mode",
            Label = "Disable DirectStorage HDD Compatibility Mode",
            Category = "Gaming",
            Description =
                "Disables the HDD compatibility mode in DirectStorage, allowing the driver to use NVMe-optimised fast paths exclusive to PCIe SSDs.",
            Tags = ["directstorage", "gaming", "nvme"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Only beneficial on NVMe SSDs; do not apply on mechanical HDDs.",
            ApplyOps = [RegOp.SetDword(DxStoreKey, "DisableHddCompatibility", 1)],
            RemoveOps = [RegOp.DeleteValue(DxStoreKey, "DisableHddCompatibility")],
            DetectOps = [RegOp.CheckDword(DxStoreKey, "DisableHddCompatibility", 1)],
        },
        new TweakDef
        {
            Id = "dxstore-enable-bypass-io",
            Label = "Enable Windows Bypass IO for Game Storage",
            Category = "Gaming",
            Description =
                "Enables Windows Bypass IO (introduced in Windows 11 22H2), allowing game engines to read from NVMe SSDs with reduced OS kernel overhead.",
            Tags = ["directstorage", "gaming", "nvme", "io"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Reduces latency for NVMe-based game asset loading by bypassing OS caching layers.",
            ApplyOps = [RegOp.SetDword(FsKey, "EnableBypassIo", 1)],
            RemoveOps = [RegOp.DeleteValue(FsKey, "EnableBypassIo")],
            DetectOps = [RegOp.CheckDword(FsKey, "EnableBypassIo", 1)],
        },
        new TweakDef
        {
            Id = "dxstore-disk-timeout-extend",
            Label = "Extend Disk I/O Timeout for Game Loading",
            Category = "Gaming",
            Description =
                "Increases the disk I/O timeout from the default 60 s to 120 s to prevent false stalls during intensive game asset loading on busy storage.",
            Tags = ["gaming", "disk", "io", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents false disk-timeout errors during intensive game asset streaming.",
            ApplyOps = [RegOp.SetDword(DiskKey, "TimeOutValue", 120)],
            RemoveOps = [RegOp.SetDword(DiskKey, "TimeOutValue", 60)],
            DetectOps = [RegOp.CheckDword(DiskKey, "TimeOutValue", 120)],
        },
        new TweakDef
        {
            Id = "dxstore-nvme-disable-stats",
            Label = "Disable NVMe Driver Statistics Collection",
            Category = "Gaming",
            Description =
                "Disables the stornvme driver's per-I/O statistics collection, reducing overhead during high-frequency game asset reads on NVMe SSDs.",
            Tags = ["gaming", "nvme", "io", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Marginal latency improvement for high-IOPS gaming workloads on NVMe.",
            ApplyOps = [RegOp.SetDword(StorNvmeKey, "NVMeDisableStatsCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(StorNvmeKey, "NVMeDisableStatsCollection")],
            DetectOps = [RegOp.CheckDword(StorNvmeKey, "NVMeDisableStatsCollection", 1)],
        },
        new TweakDef
        {
            Id = "dxstore-games-clock-rate",
            Label = "Set Games MMCSS Clock Rate to 10 000",
            Category = "Gaming",
            Description = "Sets the multimedia task clock rate for the MMCSS Games class to 10 000, improving timer precision for game update loops.",
            Tags = ["gaming", "mmcss", "latency", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Improves game-loop timer accuracy without system-wide side effects.",
            ApplyOps = [RegOp.SetDword(GamesTaskKey, "Clock Rate", 10000)],
            RemoveOps = [RegOp.SetDword(GamesTaskKey, "Clock Rate", 10000)],
            DetectOps = [RegOp.CheckDword(GamesTaskKey, "Clock Rate", 10000)],
        },
        new TweakDef
        {
            Id = "dxstore-games-io-priority",
            Label = "Set Games MMCSS I/O Priority to Critical",
            Category = "Gaming",
            Description =
                "Raises the MMCSS Games task I/O priority to Critical, ensuring game threads receive storage bandwidth over background Windows processes.",
            Tags = ["gaming", "mmcss", "io", "priority"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prioritises game I/O bandwidth over background Windows tasks during gameplay.",
            ApplyOps = [RegOp.SetString(GamesTaskKey, "Io Priority", "Critical")],
            RemoveOps = [RegOp.SetString(GamesTaskKey, "Io Priority", "Normal")],
            DetectOps = [RegOp.CheckString(GamesTaskKey, "Io Priority", "Critical")],
        },
        new TweakDef
        {
            Id = "dxstore-disable-write-protect",
            Label = "Disable DirectStorage Write-through Protection Flag",
            Category = "Gaming",
            Description =
                "Clears the write-through protection flag in the DirectStorage driver, allowing the driver to use direct mapped I/O for streaming reads.",
            Tags = ["directstorage", "gaming", "io"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Allows DirectStorage to use lower-overhead I/O paths for game asset reads.",
            ApplyOps = [RegOp.SetDword(DxStoreKey, "DisableWriteProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(DxStoreKey, "DisableWriteProtection")],
            DetectOps = [RegOp.CheckDword(DxStoreKey, "DisableWriteProtection", 1)],
        },
        new TweakDef
        {
            Id = "dxstore-max-meta-commands",
            Label = "Increase DirectStorage Meta-Command Budget",
            Category = "Gaming",
            Description =
                "Increases the DirectStorage maximum meta-command count from default, allowing more GPU decompression operations to be queued simultaneously.",
            Tags = ["directstorage", "gaming", "gpu", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Allows more parallel GPU decompression jobs for games with large asset bundles.",
            ApplyOps = [RegOp.SetDword(DxStoreKey, "MaxMetaCommandCount", 16)],
            RemoveOps = [RegOp.DeleteValue(DxStoreKey, "MaxMetaCommandCount")],
            DetectOps = [RegOp.CheckDword(DxStoreKey, "MaxMetaCommandCount", 16)],
        },
    ];
}
