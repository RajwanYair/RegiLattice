namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class SystemTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sys-detailed-bsod",
            Label = "Enable Detailed Blue Screen Info",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Shows technical parameters on BSoD screens instead of just the QR code and sad-face. Useful for diagnosing crash causes. Default: disabled. Recommended: enabled.",
            Tags = ["system", "bsod", "crash", "diagnostic", "blue-screen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl", "DisplayParameters", 1)],
        },
        new TweakDef
        {
            Id = "sys-disable-wpbt",
            Label = "Disable WPBT (Vendor Bloatware Injection)",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Platform Binary Table which allows vendors to inject software via UEFI firmware (e.g., Lenovo, HP bloatware). Default: enabled. Recommended: disabled.",
            Tags = ["system", "wpbt", "uefi", "bloatware", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "DisableWpbtExecution", 1)],
        },
        new TweakDef
        {
            Id = "sys-enable-utc-hardware-clock",
            Label = "Set Hardware Clock to UTC",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Windows hardware clock (RTC) to UTC instead of local time. Fixes time drift in dual-boot with Linux. Default: local.",
            Tags = ["system", "clock", "utc", "dual-boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation", "RealTimeIsUniversal", 1)],
        },
        new TweakDef
        {
            Id = "sys-memory-limit-none",
            Label = "Remove System Memory Limit",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the PhysicalMemoryAllocationPolicy value from Memory Management to let Windows use all available RAM without a capped upper bound. Default: not set.",
            Tags = ["system", "memory", "ram", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalMemoryAllocationPolicy"
                ),
            ],
            // NOTE: No RemoveOps — this tweak deletes a cap value that was set by the user or
            // OEM. We cannot safely restore to an unknown prior value; re-enabling any limit
            // would require knowing what it was. Removal is intentionally one-directional.
            RemoveOps = [],
            DetectOps =
            [
                RegOp.CheckMissing(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "PhysicalMemoryAllocationPolicy"
                ),
            ],
        },
        new TweakDef
        {
            Id = "sys-io-priority-boost",
            Label = "Enable I/O Priority Boost for Foreground",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ITEPriority=3 in PriorityControl to give foreground processes an I/O priority boost (Normal+). Improves responsiveness under disk-heavy background workloads. Default: 3.",
            Tags = ["system", "io", "priority", "foreground"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "ITEPriority", 3)],
        },
        new TweakDef
        {
            Id = "sys-gdi-batch-limit",
            Label = "Set GDI Batch Limit to 256",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GDIBatchLimit=256 in the Session Manager key. Increases the GDI batch flush threshold, reducing context switches for apps that make many consecutive GDI calls. Default: 0 (unbatched).",
            Tags = ["system", "gdi", "graphics", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit", 256)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "GDIBatchLimit", 256)],
        },
        new TweakDef
        {
            Id = "sys-vm-write-watch-off",
            Label = "Disable VM Write Watch",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets WriteWatch=0 in Memory Management to disable write-watch tracking. Reduces kernel overhead when this feature is not needed by the workload. Default: 0.",
            Tags = ["system", "vm", "memory", "kernel"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "WriteWatch", 0)],
        },
        new TweakDef
        {
            Id = "sys-idle-task-priority",
            Label = "Set Idle Task CPU Priority to Low",
            Category = "System 2",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IdleTaskPriority=1 in PriorityControl to ensure idle maintenance tasks run at lowest possible CPU priority. Prevents background tasks from stealing CPU time. Default: 1.",
            Tags = ["system", "idle", "priority", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IdleTaskPriority", 1)],
        },
    ];
}
