// RegiLattice.Core — Tweaks/HyperVAdvanced.cs
// Advanced Hyper-V configuration registry settings for Windows 10/11 Pro/Enterprise.
// Slug: "hyperv" — complements Virtualization.cs (virt-) without duplicating its IDs.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HyperVAdvanced
{
    private const string HvPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV";
    private const string HvWorker = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization";
    private const string HvSched = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV";
    private const string HvNet = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV";
    private const string HvWorkerCfg = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization\Worker";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "hyperv-enable-core-scheduler",
            Label = "Enable Hyper-V Core Scheduler Security Mode",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hyperv", "virtualization", "scheduler", "security", "spectre"],
            Description =
                "Sets the Hyper-V hypervisor scheduler to Core mode which schedules VM virtual processors "
                + "only on physical core pairs. Mitigates side-channel attacks (Spectre/MDS) between VMs without disabling HyperThreading.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "SchedulerType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "SchedulerType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "SchedulerType", 2)],
        },
        new TweakDef
        {
            Id = "hyperv-enable-slat-enforcement",
            Label = "Enforce Second Level Address Translation (SLAT) Requirement",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hyperv", "virtualization", "slat", "memory", "security"],
            Description =
                "Configures Hyper-V to require SLAT (Second Level Address Translation / EPT or NPT) hardware support. "
                + "SLAT prevents guest VMs from accessing physical memory frames outside their allocated range.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "RequireSLAT", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "RequireSLAT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "RequireSLAT", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-disable-time-sync",
            Label = "Disable Hyper-V Guest Time Synchronization",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hyperv", "virtualization", "time sync", "ntp", "guest"],
            Description =
                "Disables the Hyper-V Integration Service for time synchronization. "
                + "Useful for VMs that maintain their own NTP source and should not have the host override the guest clock.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Virtual Machine\Guest\Parameters", "SyncICSEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Virtual Machine\Guest\Parameters", "SyncICSEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Virtual Machine\Guest\Parameters", "SyncICSEnabled", 0)],
        },
        new TweakDef
        {
            Id = "hyperv-enable-vnuma",
            Label = "Enable vNUMA Topology Exposure to Hyper-V Guests",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hyperv", "virtualization", "numa", "performance", "memory"],
            Description =
                "Enables virtual NUMA topology exposure so guest VMs can see NUMA architecture. "
                + "Allows memory-sensitive workloads (databases, HPC) to optimize placement across NUMA nodes inside the guest.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization\Worker", "AllowNumaTopology", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization\Worker", "AllowNumaTopology"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization\Worker", "AllowNumaTopology", 1),
            ],
        },
        new TweakDef
        {
            Id = "hyperv-set-vm-queue-depth",
            Label = "Increase VMQ Depth for Hyper-V Virtual Switch",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hyperv", "virtualization", "network", "vmq", "performance"],
            Description =
                "Sets the Virtual Machine Queue (VMQ) hardware queue depth to 64 for the Hyper-V virtual switch. "
                + "Increases network throughput for VMs by using more NIC hardware receive queues, reducing CPU polling.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "MaxVMQueueDepth", 64)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "MaxVMQueueDepth")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "MaxVMQueueDepth", 64)],
        },
        new TweakDef
        {
            Id = "hyperv-enable-enhanced-session",
            Label = "Enable Hyper-V Enhanced Session Mode (RDP Protocol)",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hyperv", "virtualization", "enhanced session", "rdp", "usb redirect"],
            Description =
                "Enables Enhanced Session Mode in Hyper-V Manager which connects to VMs via RDP. "
                + "Provides clipboard, USB device, audio, and high-resolution display redirects — matching the VMware/VirtualBox experience.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fEnableVirtualizedSessionMode", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "EnableEnhancedSessionMode", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fEnableVirtualizedSessionMode"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "EnableEnhancedSessionMode"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "EnableEnhancedSessionMode", 1),
            ],
        },
        new TweakDef
        {
            Id = "hyperv-disable-live-migration",
            Label = "Disable Hyper-V Live Migration (Security Hardening)",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hyperv", "virtualization", "live migration", "security", "hardening"],
            Description =
                "Disables Live Migration capability on standalone Hyper-V hosts not part of a failover cluster. "
                + "Reduces attack surface by preventing unauthorized VM migration over the network.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Virtualization", "DisableLiveMigration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Virtualization", "DisableLiveMigration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Virtualization", "DisableLiveMigration", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-restrict-management-os-vhd",
            Label = "Restrict Hyper-V Management OS VHD Access",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["hyperv", "virtualization", "vhd", "storage", "security"],
            Description =
                "Restricts which users can attach and manage VHD/VHDX files in the Hyper-V management OS. "
                + "Prevents standard users from mounting arbitrary VHDs to extract their content.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "RestrictManagementOSVhd", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "RestrictManagementOSVhd")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "RestrictManagementOSVhd", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-enable-nested-virtualization",
            Label = "Enable Nested Virtualization Support Hint",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hyperv", "virtualization", "nested", "docker", "wsl2", "performance"],
            Description =
                "Configures the Hyper-V platform registry hint to advertise nested virtualization features. "
                + "Required for running Docker Server, Kubernetes, or WSL2 Hyper-V backend inside a Hyper-V guest VM.",
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization",
                    "AllowExposeNestedVirtualization",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization",
                    "AllowExposeNestedVirtualization"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization",
                    "AllowExposeNestedVirtualization",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "hyperv-set-worker-thread-count",
            Label = "Pin Hyper-V Worker Process Thread Pool Size",
            Category = "Hyper-V Advanced",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["hyperv", "virtualization", "performance", "threads", "cpu"],
            Description =
                "Sets the Hyper-V virtual machine worker process thread count to match logical CPU count (16). "
                + "Prevents under-provisioning on high core-count CPUs where the default 4-thread pool causes context-switch bottlenecks.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization\Worker", "MaxWorkerThreadCount", 16),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization\Worker", "MaxWorkerThreadCount"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization\Worker",
                    "MaxWorkerThreadCount",
                    16
                ),
            ],
        },
    ];
}
