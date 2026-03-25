// RegiLattice.Core — Tweaks/PageFilePolicy.cs
// Sprint 289: Page File Policy tweaks (10 tweaks)
// Category: "Page File Policy" | Slug: pgfpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PageFile

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PageFilePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PageFile";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pgfpol-ensure-pagefile-enabled",
            Label = "Ensure Page File Is Enabled",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The Windows page file provides virtual memory overflow capacity by extending RAM with disk storage. Ensuring the page file is not forcibly disabled prevents out-of-memory conditions that can crash applications and the operating system. This policy verifies that DisablePageFile is set to zero, meaning the page file is permitted to exist. Systems processing large datasets, running virtual machines, or hosting multiple applications depend on adequate virtual memory. Removing the page file entirely can cause critical system failures on memory-constrained workloads. Maintaining this setting at its safe default ensures system stability across diverse workload profiles.",
            Tags = ["pagefile", "memory", "stability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePageFile", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePageFile")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePageFile", 0)],
        },
        new TweakDef
        {
            Id = "pgfpol-clear-pagefile-shutdown",
            Label = "Clear Page File at Shutdown",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "The page file can retain sensitive data written there by applications during a session, including encryption keys, passwords, and confidential documents. Clearing the page file at shutdown overwrites the page file contents with zeros, preventing data recovery from the swap space. This is a security hardening measure required by many compliance frameworks including NIST, CIS, and DoD STIGs. The clearing operation adds time to the shutdown sequence proportional to the page file size and storage speed. Systems with large page files on slow HDDs may experience noticeably longer shutdown times. On SSDs the performance impact is minimal, and the security benefit justifies the small delay in all regulated environments.",
            Tags = ["pagefile", "security", "shutdown", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ClearPageFileAtShutdown", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClearPageFileAtShutdown")],
            DetectOps = [RegOp.CheckDword(Key, "ClearPageFileAtShutdown", 1)],
        },
        new TweakDef
        {
            Id = "pgfpol-ensure-swapfile-active",
            Label = "Ensure Swap File Is Active",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The Windows swap file is a separate virtual memory file used for background application paging on modern Windows versions. Ensuring the swap file is not disabled preserves the operating system's ability to handle memory pressure through multiple paging mechanisms. This policy sets DisableSwapFile to zero, confirming the swap file remains active for background app memory management. Disabling the swap file on RAM-constrained systems can lead to application termination under memory pressure. The swap file works in conjunction with the page file to provide comprehensive virtual memory management. Maintaining this setting at its default ensures predictable memory behavior for suspended applications.",
            Tags = ["pagefile", "swapfile", "memory", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSwapFile", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSwapFile")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSwapFile", 0)],
        },
        new TweakDef
        {
            Id = "pgfpol-set-max-size-4096",
            Label = "Set Maximum Page File Size 4096 MB",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Setting a maximum page file size prevents the page file from growing unboundedly and consuming excessive disk space on system drives. A 4096 MB maximum represents a balanced limit suitable for most enterprise workstations with 16 GB or more of installed RAM. Unbounded page file growth can fill system drives and trigger low-disk-space conditions that destabilize the operating system. This setting must be calibrated against the actual workload requirements of the target machine class. Memory-intensive workloads such as database servers, virtualization hosts, and development environments may require higher limits. Administrators should monitor memory usage before applying this limit to production systems.",
            Tags = ["pagefile", "memory", "disk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxPageFileSize", 4096)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxPageFileSize")],
            DetectOps = [RegOp.CheckDword(Key, "MaxPageFileSize", 4096)],
        },
        new TweakDef
        {
            Id = "pgfpol-disable-peak-detection",
            Label = "Disable Automatic Peak Page File Detection",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows automatically adjusts the page file size based on observed peak memory usage patterns over time. This adaptive mechanism can cause the page file to fluctuate in size, leading to disk fragmentation on HDD systems. Disabling automatic peak detection freezes the page file at its configured size, providing predictable storage consumption. Administrators managing server environments with known memory requirements benefit from deterministic page file sizing. The adaptive mechanism is primarily beneficial on consumer devices with widely varying workloads. Enterprise systems with stable application loads gain more from a fixed, well-configured page file size than from dynamic adjustment.",
            Tags = ["pagefile", "memory", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutomaticPeakDetection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutomaticPeakDetection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutomaticPeakDetection", 1)],
        },
        new TweakDef
        {
            Id = "pgfpol-allow-system-managed",
            Label = "Allow System-Managed Page File",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The system-managed page file allows Windows to dynamically size and manage the page file based on available disk space and observed usage patterns. Allowing system management ensures the page file automatically adjusts to unusual workload spikes that exceed manually configured sizes. This policy sets DisableSystemManagedPageFile to zero, preserving the default system management behavior. Organizations relying on Windows to handle memory management automatically benefit from this setting on general-purpose workstations. Environments with strict resource governance may prefer manual page file sizing, but system management provides a reliable fallback. This setting is safe and recommended unless a specific maximum size policy is required.",
            Tags = ["pagefile", "memory", "system", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSystemManagedPageFile", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSystemManagedPageFile")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSystemManagedPageFile", 0)],
        },
        new TweakDef
        {
            Id = "pgfpol-allow-low-memory-detection",
            Label = "Allow Low Memory Detection",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Low memory detection triggers warnings and system responses when available physical and virtual memory falls below critical thresholds. Maintaining the low memory detection mechanism active at its default ensures the system can respond appropriately to memory pressure. This policy sets DisableLowMemoryDetection to zero, preserving protective system behavior. With detection enabled, the system can proactively terminate unresponsive processes and log diagnostic information before a complete memory exhaustion event. Disabling detection removes the safety net and can cause sudden application crashes or system hangs without warning. This setting should remain at the safe default on all production systems.",
            Tags = ["pagefile", "memory", "stability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLowMemoryDetection", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLowMemoryDetection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLowMemoryDetection", 0)],
        },
        new TweakDef
        {
            Id = "pgfpol-place-on-system-drive",
            Label = "Place Page File on System Drive",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Placing the page file on the system drive keeps virtual memory on the fastest and most reliable storage device in most configurations. The system drive typically hosts the operating system on an NVMe or SATA SSD with superior I/O characteristics. This policy ensures the page file is configured to use the system drive, providing consistent performance for virtual memory operations. Keeping the page file on the system drive also simplifies disk management by avoiding dependency on secondary drives. On multi-drive configurations, administrators may prefer secondary drives for page file placement to reduce I/O contention on the system drive. This setting is appropriate for workstations with a single high-performance storage device.",
            Tags = ["pagefile", "storage", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PageFileOnSystemDrive", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "PageFileOnSystemDrive")],
            DetectOps = [RegOp.CheckDword(Key, "PageFileOnSystemDrive", 1)],
        },
        new TweakDef
        {
            Id = "pgfpol-disable-telemetry",
            Label = "Disable Page File Telemetry",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Page file telemetry transmits metrics about virtual memory usage patterns, page fault rates, and page file sizing to Microsoft. This data provides Microsoft with insight into memory pressure scenarios across the Windows user base. Disabling page file telemetry prevents virtual memory utilization data from being transmitted to external services. Organizations with strict data residency requirements or network egress monitoring policies benefit from disabling this telemetry. The page file continues to function identically regardless of whether telemetry is enabled. Administrators can obtain equivalent memory usage insights through Windows Performance Monitor and ETW tracing.",
            Tags = ["pagefile", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePageFileTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePageFileTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePageFileTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "pgfpol-disable-memory-dump",
            Label = "Disable Memory Dump Creation",
            Category = "Page File Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Memory dumps capture the contents of RAM to disk following a system crash and are stored in the page file or dedicated dump files. These dump files can contain sensitive data including credentials, encryption keys, and application data present in memory at crash time. Disabling memory dump creation prevents sensitive memory contents from being written to disk where they could be extracted. Security-hardened environments and regulated industries often disable memory dumps as part of data-at-rest protection policies. The tradeoff is reduced diagnostic capability for analyzing crash root causes in post-incident investigations. Environments with stringent memory protection requirements should disable dumps and rely on live debugging or remote crash analysis tools.",
            Tags = ["pagefile", "memory-dump", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMemoryDump", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMemoryDump")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMemoryDump", 1)],
        },
    ];
}
