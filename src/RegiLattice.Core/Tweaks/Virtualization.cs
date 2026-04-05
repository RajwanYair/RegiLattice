namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Virtualization
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "virt-disable-enhanced-session",
            Label = "Disable Hyper-V Enhanced Session Default",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Hyper-V Enhanced Session Mode by default. Useful if clipboard/file sharing between host and VM causes issues.",
            Tags = ["hyperv", "virtualization", "enhanced-session"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "AllowEnhancedSessionMode", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "AllowEnhancedSessionMode", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "AllowEnhancedSessionMode", 0),
            ],
        },
        new TweakDef
        {
            Id = "virt-disable-hvci",
            Label = "Disable HVCI (Memory Integrity)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Hypervisor-enforced Code Integrity (HVCI / Memory Integrity). Can improve gaming performance by 5-10% but reduces security.",
            Tags = ["hvci", "virtualization", "performance", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "Enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "Enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "virt-disable-autostop",
            Label = "Disable Hyper-V Auto Stop Action",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic VM stop action on host shutdown. VMs will be saved to disk instead of stopped. Default: Disabled. Recommended: Enabled for server VMs.",
            Tags = ["hyperv", "virtualization", "auto-stop"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "DisableAutomaticStopAction", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "DisableAutomaticStopAction")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "DisableAutomaticStopAction", 1)],
        },
        new TweakDef
        {
            Id = "virt-optimize-worker-priority",
            Label = "Optimize VM Worker Process Priority",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Hyper-V VM worker processes to high priority within the Windows scheduler. Reduces VM latency. Default: Normal priority. Recommended: High priority for dedicated VM hosts.",
            Tags = ["hyperv", "virtualization", "performance", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "HighVmWorkerProcessPriority", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "HighVmWorkerProcessPriority"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "HighVmWorkerProcessPriority", 1),
            ],
        },
        new TweakDef
        {
            Id = "virt-enable-nested-virt",
            Label = "Enable Nested Virtualization",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables Hyper-V nested virtualization. Allows running hypervisors inside Hyper-V VMs. Default: disabled. Recommended: enable for dev/test workloads.",
            Tags = ["hyperv", "virtualization", "nested", "development"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "NestedVirtualization", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "NestedVirtualization"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "NestedVirtualization", 1),
            ],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-autostart",
            Label = "Disable Hyper-V Autostart (vmms Manual)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Hyper-V Virtual Machine Management service to manual start. Reduces boot time and background resource usage when VMs are not in use. Default: Automatic (2). Recommended: Manual (3).",
            Tags = ["hyperv", "virtualization", "autostart", "service", "boot"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 3)],
        },
        new TweakDef
        {
            Id = "virt-vds-manual",
            Label = "Set Virtual Disk Service to Manual Start",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Virtual Disk Service (VDS) to manual start, reducing boot time overhead on systems not using disk management tools. Default: Automatic. Recommended: Manual on workstations.",
            Tags = ["virtualization", "vds", "service", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vds"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vds", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vds", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vds", "Start", 3)],
        },
        new TweakDef
        {
            Id = "virt-disable-rdv-policy",
            Label = "Disable Remote Desktop Virtualization Policy",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Remote Desktop Virtualization (RDV) policy. Prevents RD session host from using virtualization layer. Default: Enabled. Recommended: Disabled for non-RDS systems.",
            Tags = ["virtualization", "rdv", "rdp", "policy", "rds"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "DisableRemoteDesktopVirtualization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "DisableRemoteDesktopVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "DisableRemoteDesktopVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "virt-enable-nested-virtualization",
            Label = "Enable Nested Virtualization",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables nested virtualization for Hyper-V VMs. Allows running hypervisors inside VMs. Default: disabled.",
            Tags = ["virtualization", "nested", "hyper-v", "vms"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "EnableNestedVirtualization", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "EnableNestedVirtualization"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "EnableNestedVirtualization", 1),
            ],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-dynamic-memory",
            Label = "Disable Hyper-V Dynamic Memory Ballooning",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Hyper-V dynamic memory ballooning for the host. Ensures memory allocation remains static. Default: enabled.",
            Tags = ["virtualization", "hyper-v", "dynamic-memory", "ballooning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "DisableDynamicMemory", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "DisableDynamicMemory"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "DisableDynamicMemory", 1),
            ],
        },
        new TweakDef
        {
            Id = "virt-disable-containers-ext",
            Label = "Disable Windows Containers Extension",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Containers feature extension. Reduces attack surface if containers are not used. Default: varies.",
            Tags = ["virtualization", "containers", "windows", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Containers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Containers", "ContainersOff", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Containers", "ContainersOff")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Containers", "ContainersOff", 1)],
        },
        new TweakDef
        {
            Id = "virt-hypervisor-core-scheduler",
            Label = "Enable Hypervisor Core Scheduler",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the hypervisor core scheduler for improved VM performance and security isolation. Mitigates side-channel attacks. Default: root scheduler.",
            Tags = ["virtualization", "hypervisor", "scheduler", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "HvpScheduler", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "HvpScheduler")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "HvpScheduler", 2)],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "virt-disable-hyperv-time-sync",
            Label = "Disable Hyper-V Time Synchronization",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Hyper-V Integration Services time synchronization for guest VMs. Useful when guests use their own NTP configuration.",
            Tags = ["virtualization", "hyper-v", "time-sync", "guest"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Virtual Machine\Auto"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Virtual Machine\Auto", "DisableTimeSync", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Virtual Machine\Auto", "DisableTimeSync")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Virtual Machine\Auto", "DisableTimeSync", 1)],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-heartbeat",
            Label = "Disable Hyper-V Heartbeat Service",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Hyper-V Heartbeat integration service. Reduces overhead for VMs that do not need host health monitoring.",
            Tags = ["virtualization", "hyper-v", "heartbeat", "overhead"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicheartbeat"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicheartbeat", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicheartbeat", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicheartbeat", "Start", 4)],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-guest-shutdown",
            Label = "Disable Hyper-V Guest Shutdown Service",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Hyper-V Guest Shutdown integration service. Host will no longer be able to gracefully shut down guest VMs remotely.",
            Tags = ["virtualization", "hyper-v", "shutdown", "guest"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicshutdown"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicshutdown", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicshutdown", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicshutdown", "Start", 4)],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-kvp-exchange",
            Label = "Disable Hyper-V KVP Exchange Service",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Hyper-V Data Exchange (KVP) integration service. Prevents key-value pair metadata exchange between host and guest.",
            Tags = ["virtualization", "hyper-v", "kvp", "metadata"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmickvpexchange"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmickvpexchange", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmickvpexchange", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmickvpexchange", "Start", 4)],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-video-offload",
            Label = "Disable Hyper-V Video Remote FX",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Hyper-V Remote FX video rendering service. Reduces GPU resource consumption when RemoteFX is not used.",
            Tags = ["virtualization", "hyper-v", "remotefx", "gpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicrdv"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicrdv", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicrdv", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicrdv", "Start", 4)],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-vss-writer",
            Label = "Disable Hyper-V VSS Writer",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Hyper-V Volume Shadow Copy (VSS) integration service. Not needed if host-level backup of VMs is not required.",
            Tags = ["virtualization", "hyper-v", "vss", "backup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicvss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicvss", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicvss", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicvss", "Start", 4)],
        },
        new TweakDef
        {
            Id = "virt-set-vm-memory-weight-high",
            Label = "Set VM Memory Weight to High Priority",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Hyper-V memory weight configuration to high priority. VMs receive memory allocation preference over low-priority workloads.",
            Tags = ["virtualization", "hyper-v", "memory", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "MemoryWeight", 5000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "MemoryWeight")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "MemoryWeight", 5000)],
        },
        new TweakDef
        {
            Id = "virt-disable-hyperv-gcs",
            Label = "Disable Hyper-V Guest Clustering Service",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Hyper-V Guest Clustering integration service. Not needed for standalone VMs that are not part of a failover cluster.",
            Tags = ["virtualization", "hyper-v", "clustering", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicguestinterface"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicguestinterface", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicguestinterface", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmicguestinterface", "Start", 4)],
        },
    ];
}

// ── Merged from HyperVAdvanced.cs ──────────────────────────────────────────────────

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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
            Category = "Virtualization",
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
        new TweakDef
        {
            Id = "hyperv-no-auto-checkpoints",
            Label = "Disable Automatic VM Checkpoints",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableAutomaticCheckpoints = 1 via Hyper-V policy. Prevents Hyper-V from silently "
                + "creating a checkpoint before major operations (shutdown, upgrade). Auto-checkpoints accumulate "
                + "on the host disk and can fill volumes on busy dev machines. Default: automatic checkpoints enabled.",
            Tags = ["hyperv", "virtualization", "checkpoint", "disk", "policy"],
            RegistryKeys = [HvPol],
            ApplyOps = [RegOp.SetDword(HvPol, "DisableAutomaticCheckpoints", 1)],
            RemoveOps = [RegOp.DeleteValue(HvPol, "DisableAutomaticCheckpoints")],
            DetectOps = [RegOp.CheckDword(HvPol, "DisableAutomaticCheckpoints", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-require-net-creds",
            Label = "Require Credentials for VM Network Configuration",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RequireCredentialsForNetworkConfiguration = 1 via Hyper-V policy. Forces re-authentication "
                + "before modifying virtual switch bindings or VM network adapters, preventing privilege escalation "
                + "through unauthorized network reconfiguration.",
            Tags = ["hyperv", "virtualization", "network", "credentials", "security"],
            RegistryKeys = [HvPol],
            ApplyOps = [RegOp.SetDword(HvPol, "RequireCredentialsForNetworkConfiguration", 1)],
            RemoveOps = [RegOp.DeleteValue(HvPol, "RequireCredentialsForNetworkConfiguration")],
            DetectOps = [RegOp.CheckDword(HvPol, "RequireCredentialsForNetworkConfiguration", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-allow-sriov",
            Label = "Allow SR-IOV Virtual Functions for Hyper-V NICs",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets AllowSriovNetworking = 1 via Hyper-V policy. Permits SR-IOV (Single Root I/O Virtualisation) "
                + "virtual functions to be exposed to VMs, enabling near-native NIC performance by bypassing the virtual switch "
                + "for latency-sensitive VM workloads. Requires SR-IOV capable hardware. Default: not set.",
            Tags = ["hyperv", "virtualization", "sriov", "network", "performance"],
            RegistryKeys = [HvPol],
            ApplyOps = [RegOp.SetDword(HvPol, "AllowSriovNetworking", 1)],
            RemoveOps = [RegOp.DeleteValue(HvPol, "AllowSriovNetworking")],
            DetectOps = [RegOp.CheckDword(HvPol, "AllowSriovNetworking", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-vm-bw-management",
            Label = "Enable VM Network Bandwidth Management",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableVMNetworkBandwidthManagement = 1 via Hyper-V policy. Allows the Hyper-V virtual switch "
                + "to enforce per-VM minimum and maximum bandwidth limits (QoS). Prevents a single noisy VM from "
                + "saturating the physical NIC and degrading other VMs.",
            Tags = ["hyperv", "virtualization", "network", "bandwidth", "qos"],
            RegistryKeys = [HvPol],
            ApplyOps = [RegOp.SetDword(HvPol, "EnableVMNetworkBandwidthManagement", 1)],
            RemoveOps = [RegOp.DeleteValue(HvPol, "EnableVMNetworkBandwidthManagement")],
            DetectOps = [RegOp.CheckDword(HvPol, "EnableVMNetworkBandwidthManagement", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-no-vm-broadcast",
            Label = "Block VM-to-Host Broadcast Traffic",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableVMtoBroadcast = 1 via Hyper-V policy. Prevents VMs from flooding the management OS NIC "
                + "with broadcast/multicast frames, reducing noisy-neighbour impact on the host network stack and improving "
                + "VM-to-VM isolation.",
            Tags = ["hyperv", "virtualization", "network", "broadcast", "isolation"],
            RegistryKeys = [HvPol],
            ApplyOps = [RegOp.SetDword(HvPol, "DisableVMtoBroadcast", 1)],
            RemoveOps = [RegOp.DeleteValue(HvPol, "DisableVMtoBroadcast")],
            DetectOps = [RegOp.CheckDword(HvPol, "DisableVMtoBroadcast", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-max-vms-8",
            Label = "Cap Maximum Running VMs to 8",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets MaxVirtualMachines = 8 in the Hyper-V Virtualization key. Limits the number of concurrently "
                + "running virtual machines to 8, preventing runaway VM sprawl from exhausting host RAM and CPU on "
                + "workstation deployments. Remove to revert to no limit.",
            Tags = ["hyperv", "virtualization", "limit", "resource", "workstation"],
            RegistryKeys = [HvWorker],
            ApplyOps = [RegOp.SetDword(HvWorker, "MaxVirtualMachines", 8)],
            RemoveOps = [RegOp.DeleteValue(HvWorker, "MaxVirtualMachines")],
            DetectOps = [RegOp.CheckDword(HvWorker, "MaxVirtualMachines", 8)],
        },
        new TweakDef
        {
            Id = "hyperv-host-memory-reserve-512",
            Label = "Reserve 512 MB RAM for Hyper-V Host OS",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets HostMemoryReserve = 512 (MB) in the Hyper-V scheduler key. Guarantees that at least 512 MB "
                + "of physical RAM is always available to the management OS, preventing VM memory pressure from "
                + "starving the host and causing system instability.",
            Tags = ["hyperv", "virtualization", "memory", "host", "reserve"],
            RegistryKeys = [HvSched],
            ApplyOps = [RegOp.SetDword(HvSched, "HostMemoryReserve", 512)],
            RemoveOps = [RegOp.DeleteValue(HvSched, "HostMemoryReserve")],
            DetectOps = [RegOp.CheckDword(HvSched, "HostMemoryReserve", 512)],
        },
        new TweakDef
        {
            Id = "hyperv-no-default-switch",
            Label = "Disable Hyper-V Default Switch (NAT)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableDefaultSwitch = 1 via Hyper-V policy. Removes the 'Default Switch' NAT virtual switch "
                + "that Windows creates automatically. The Default Switch IP range (172.x) can conflict with VPN and "
                + "corporate network ranges. Disable when using custom external or internal switches only.",
            Tags = ["hyperv", "virtualization", "switch", "nat", "network", "vpn"],
            RegistryKeys = [HvPol],
            ApplyOps = [RegOp.SetDword(HvPol, "DisableDefaultSwitch", 1)],
            RemoveOps = [RegOp.DeleteValue(HvPol, "DisableDefaultSwitch")],
            DetectOps = [RegOp.CheckDword(HvPol, "DisableDefaultSwitch", 1)],
        },
        new TweakDef
        {
            Id = "hyperv-strict-network-isolation",
            Label = "Enable Strict VM Network Isolation",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableStrictIsolation = 1 via Hyper-V policy. Enables strict inter-VM network isolation mode "
                + "on the virtual switch, ensuring that VMs on the same host cannot communicate unless explicitly "
                + "connected to a shared virtual switch. Strengthens tenant separation on multi-VM hosts.",
            Tags = ["hyperv", "virtualization", "network", "isolation", "security"],
            RegistryKeys = [HvPol],
            ApplyOps = [RegOp.SetDword(HvPol, "EnableStrictIsolation", 1)],
            RemoveOps = [RegOp.DeleteValue(HvPol, "EnableStrictIsolation")],
            DetectOps = [RegOp.CheckDword(HvPol, "EnableStrictIsolation", 1)],
        },
    ];
}

// ── Merged from WindowsSandboxAdv.cs ──────────────────────────────────────────────────

internal static class WindowsSandboxAdv
{
    private const string SbPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox";
    private const string SbSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\cmstp";
    private const string ContainerPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Container";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sandbox-disable-printer-sharing",
            Label = "Disable Windows Sandbox Printer Redirection",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "printer", "isolation"],
            Description =
                "Prevents host printers from being visible inside Windows Sandbox. "
                + "Stops documents produced inside the sandbox from being printed on the host network.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowPrintAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowPrintAccess")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowPrintAccess", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-set-memory-limit",
            Label = "Set Windows Sandbox Memory Limit (4 GB Cap)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["sandbox", "performance", "memory", "resource limit"],
            Description =
                "Caps the maximum memory Windows Sandbox can consume at 4096 MB. "
                + "Prevents sandbox workloads from starving the host of RAM during analysis sessions.",
            ApplyOps = [RegOp.SetDword(SbPol, "MemoryInMB", 4096)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "MemoryInMB")],
            DetectOps = [RegOp.CheckDword(SbPol, "MemoryInMB", 4096)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-microphone",
            Label = "Disable Microphone Input in Windows Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "microphone", "audio", "isolation", "privacy"],
            Description =
                "Prevents sandboxed applications from accessing the host microphone. "
                + "Complements the existing audio-disable tweak with dedicated "
                + "mic-only policy for environments that allow speaker pass-through.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowMicrophoneInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowMicrophoneInput")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowMicrophoneInput", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-cap-cpu-count-2",
            Label = "Cap Windows Sandbox CPU Count to 2 Cores",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "cpu", "performance", "resource limit"],
            Description =
                "Limits Windows Sandbox to 2 logical processor cores via Group Policy. "
                + "Prevents sandbox workloads from monopolising the host CPU during "
                + "malware analysis or web isolation sessions.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowedCPUCount", 2)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowedCPUCount")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowedCPUCount", 2)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-windows-installer",
            Label = "Disable Windows Installer (MSI) Inside Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "msi", "installer", "security", "policy"],
            Description =
                "Prevents the Windows Installer service from running inside the sandbox. "
                + "Restricts sandboxed sessions to portable/xcopy deployments and "
                + "blocks installer-level exploit techniques.",
            ApplyOps = [RegOp.SetDword(ContainerPol, "DisableWindowsInstaller", 1)],
            RemoveOps = [RegOp.DeleteValue(ContainerPol, "DisableWindowsInstaller")],
            DetectOps = [RegOp.CheckDword(ContainerPol, "DisableWindowsInstaller", 1)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-cortana-search",
            Label = "Disable Cortana / Windows Search Inside Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "cortana", "search", "privacy"],
            Description =
                "Prevents Cortana and Windows Search from running inside the sandbox, "
                + "cutting network calls to Bing and reducing idle CPU usage in "
                + "sandboxed sessions.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowCortana", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowCortana")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowCortana", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-location-service",
            Label = "Disable Location Service Inside Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "location", "gps", "privacy"],
            Description =
                "Blocks location API calls from applications running inside "
                + "Windows Sandbox. Prevents location-aware malware from "
                + "detecting the sandbox environment via geolocation discrepancies.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowLocationService", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowLocationService")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowLocationService", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-set-idle-timeout-1h",
            Label = "Set Windows Sandbox Idle Timeout to 1 Hour",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "idle timeout", "session", "resource"],
            Description =
                "Automatically terminates an idle Windows Sandbox session after "
                + "3600 seconds (1 hour), reclaiming RAM and CPU resources. "
                + "Prevents forgotten sandbox sessions from running indefinitely.",
            ApplyOps = [RegOp.SetDword(SbPol, "IdleTimeoutInSeconds", 3600)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "IdleTimeoutInSeconds")],
            DetectOps = [RegOp.CheckDword(SbPol, "IdleTimeoutInSeconds", 3600)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-logon-screensaver",
            Label = "Disable Screensaver Inside Windows Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "screensaver", "display", "session"],
            Description =
                "Disables the screensaver within the Windows Sandbox session to "
                + "prevent the sandbox display from blanking during automated "
                + "analysis tasks or long-running test scripts.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowScreenSaver", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowScreenSaver")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowScreenSaver", 0)],
        },
    ];
}

// ── merged from PolicySubsystems.cs ──
// RegiLattice.Core — Tweaks/PolicySubsystems.cs
// WSL 2, Windows Subsystem for Android, Hyper-V containers, Windows Sandbox, and holographic device policies
// Category: "Windows Subsystems Policy"
// Consolidated from 16 modules.

internal static class PolicySubsystems
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _AndroidAppDebuggingPolicy.Data,
            .. _AndroidSensorAccessPolicy.Data,
            .. _HolographicDevicePolicy.Data,
            .. _HyperVContainerPolicy.Data,
            .. _WindowsContainerPolicy.Data,
            .. _WindowsSandboxPolicy.Data,
            .. _WsaAndroidPolicy.Data,
            .. _WsaNetworkIsolationPolicy.Data,
            .. _WsaStoragePolicy.Data,
            .. _Wsl2AdvancedPolicy.Data,
            .. _WslDistroManagementPolicy.Data,
            .. _WslFileSystemPolicy.Data,
            .. _WslKernelUpdatePolicy.Data,
            .. _WslMemoryLimitsPolicy.Data,
            .. _WslSecurityHardeningPolicy.Data,
            .. _WindowsSubsystemLinuxPolicy.Data,
        ];

    // ── AndroidAppDebuggingPolicy ──
    private static class _AndroidAppDebuggingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Debugging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsadbg-disable-adb-access",
                    Label = "Disable ADB Access to WSA Android Container",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables Android Debug Bridge (ADB) access to the WSA container, preventing developer debug connections, app sideloading via ADB, and adb shell command execution.",
                    Tags = ["wsa", "android", "adb", "debugging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ADB disabled for WSA; cannot sideload APKs or access Android shell via adb connect.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableADBAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableADBAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableADBAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-disable-android-developer-options",
                    Label = "Disable Android Developer Options in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables the Android Developer Options menu in WSA settings, blocking users from enabling USB debugging, mock location, or other developer settings within the Android container.",
                    Tags = ["wsa", "android", "developer-options", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Developer Options hidden in WSA; advanced Android debug settings unavailable.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidDeveloperOptions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidDeveloperOptions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidDeveloperOptions", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-block-sideloaded-apks",
                    Label = "Block Sideloaded APK Installation in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks the installation of APK files from outside the Amazon Appstore, preventing users from installing potentially malicious Android apps via direct APK transfer.",
                    Tags = ["wsa", "android", "sideloading", "apk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "APK sideloading blocked; only Amazon Appstore apps can be installed in WSA.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockSideloadedAPKInstallation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockSideloadedAPKInstallation")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockSideloadedAPKInstallation", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-disable-logcat-output",
                    Label = "Disable Android Logcat Access from Host OS",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents the Windows host OS processes from reading Android logcat output from the WSA container, reducing diagnostic data exposure from Android app crash logs.",
                    Tags = ["wsa", "android", "logcat", "debugging", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Android logcat access from host blocked; app crash data from WSA not accessible to host processes.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLogcatFromHost", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLogcatFromHost")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLogcatFromHost", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-disable-android-crash-reporting",
                    Label = "Disable Android App Crash Report Upload from WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables the automatic upload of Android application crash reports from within the WSA container to app developers or Amazon, preventing personal or usage data from reaching third parties.",
                    Tags = ["wsa", "android", "crash-report", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Android app crash reports not uploaded; developers and Amazon receive no crash telemetry.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidCrashReporting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidCrashReporting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidCrashReporting", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-block-android-root-detection-bypass",
                    Label = "Block Android Root-Detection Bypass Tools in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Magisk-style root detection bypass frameworks from being installed or operating within the WSA container, preventing banking and DRM apps from being tricked into running on a 'rooted' environment.",
                    Tags = ["wsa", "android", "root-detection", "magisk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Root bypass frameworks blocked; WSA reports its true environment status to apps.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockRootDetectionBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockRootDetectionBypass")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockRootDetectionBypass", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-require-apk-signature-verify",
                    Label = "Require APK Signature Verification Before Install",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Enforces APK v2/v3 signature verification for all Android packages installed in WSA, blocking install of APKs with tampered or missing signatures.",
                    Tags = ["wsa", "android", "apk-signature", "integrity", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "APK signature required for install; tampered or unsigned APKs rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAPKSignatureVerification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAPKSignatureVerification")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAPKSignatureVerification", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-block-android-profiling",
                    Label = "Block Android Performance Profiling from Host",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents host-side profiling tools (simpleperf, systrace, atrace) from attaching to Android processes in the WSA container, protecting Android app internals from host-side introspection.",
                    Tags = ["wsa", "android", "profiling", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Android performance profiling from host blocked; APK memory and execution cannot be profiled from Windows.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidProfilingFromHost", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidProfilingFromHost")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidProfilingFromHost", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-disable-android-mock-location",
                    Label = "Disable Mock Location in Android WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables the Android mock location provider in WSA, preventing apps and developers from injecting fake GPS coordinates to spoof location-based services.",
                    Tags = ["wsa", "android", "mock-location", "gps", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Mock location disabled in WSA; GPS spoofing via Android developer options not possible.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMockLocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMockLocation")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMockLocation", 1)],
                },
                new TweakDef
                {
                    Id = "wsadbg-block-wifi-password-sharing",
                    Label = "Block Android Wi-Fi Password Sharing from WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks the Android Wi-Fi password sharing feature in WSA that can export saved Wi-Fi credentials from the Android container as a QR code, preventing corporate Wi-Fi key leakage.",
                    Tags = ["wsa", "android", "wifi", "credential-sharing", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android Wi-Fi password QR sharing blocked; saved Wi-Fi credentials cannot be exported from WSA.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidWifiPasswordSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidWifiPasswordSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidWifiPasswordSharing", 1)],
                },
            ];
    }

    // ── AndroidSensorAccessPolicy ──
    private static class _AndroidSensorAccessPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Sensors";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsasnsr-block-accelerometer",
                    Label = "Block Accelerometer Access for Android Apps",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android applications in WSA from accessing the device accelerometer sensor, preventing motion-based fingerprinting and keystroke inference attacks via accelerometer data.",
                    Tags = ["wsa", "android", "accelerometer", "sensor", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Accelerometer blocked for Android apps; motion-based tracking and keystroke inference prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAccelerometer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAccelerometer")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAccelerometer", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-block-gyroscope",
                    Label = "Block Gyroscope Access for Android Apps",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android applications in WSA from accessing the gyroscope, preventing orientation and rotation tracking that can be used for covert activity inference.",
                    Tags = ["wsa", "android", "gyroscope", "sensor", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Gyroscope blocked for Android apps; rotation-based side-channel attacks prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockGyroscope", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockGyroscope")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockGyroscope", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-block-proximity-sensor",
                    Label = "Block Proximity Sensor Access for Android Apps",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android applications in WSA from reading the proximity sensor, preventing apps from detecting physical presence near the device for surveillance or power-state manipulation.",
                    Tags = ["wsa", "android", "proximity", "sensor", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Proximity sensor blocked for Android apps; presence detection by Android apps prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockProximitySensor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockProximitySensor")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockProximitySensor", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-block-light-sensor",
                    Label = "Block Ambient Light Sensor Access for Android Apps",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android applications from accessing the ambient light sensor in WSA, preventing light-level side-channel information from being used to infer room or user context.",
                    Tags = ["wsa", "android", "light-sensor", "sensor", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Ambient light sensor blocked for Android apps; auto-brightness in WSA apps disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockLightSensor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockLightSensor")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockLightSensor", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-block-barometer",
                    Label = "Block Barometer Sensor Access for Android Apps",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android applications in WSA from reading barometric pressure data, preventing apps from using pressure data for floor-level location inference.",
                    Tags = ["wsa", "android", "barometer", "sensor", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Barometer blocked for Android apps; floor-level location inference via pressure data prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockBarometerSensor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockBarometerSensor")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockBarometerSensor", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-block-fingerprint-sensor",
                    Label = "Block Fingerprint Sensor API for Android Apps in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents Android apps in WSA from accessing the Windows Hello fingerprint hardware via the Android fingerprint API, stopping Android banking apps from using the Windows biometric sensor incorrectly.",
                    Tags = ["wsa", "android", "fingerprint", "biometric", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Fingerprint API blocked for Android WSA apps; biometric auth in Android apps falls back to PIN.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidFingerprintSensor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidFingerprintSensor")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidFingerprintSensor", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-block-magnetometer",
                    Label = "Block Magnetometer/Compass Access for Android Apps",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android apps in WSA from accessing the magnetometer/digital compass sensor, preventing directional tracking and compass-based location correlation.",
                    Tags = ["wsa", "android", "magnetometer", "compass", "sensor", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Magnetometer blocked for Android apps; compass navigation and direction-based apps disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockMagnetometerSensor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockMagnetometerSensor")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockMagnetometerSensor", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-block-temperature-sensor",
                    Label = "Block Temperature Sensor Access for Android Apps",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android apps in WSA from reading CPU or board temperature sensors, preventing apps from using thermal data to infer workload patterns or detect sandboxed execution.",
                    Tags = ["wsa", "android", "temperature", "sensor", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Temperature sensor blocked; Android apps cannot infer CPU workload via thermal readings.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockTemperatureSensor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockTemperatureSensor")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockTemperatureSensor", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-disable-sensor-fusion",
                    Label = "Disable Android Sensor Fusion in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables the Android sensor fusion layer in WSA that aggregates multiple sensor streams into virtual sensors (rotation vector, gravity, linear acceleration), reducing composite tracking attack surface.",
                    Tags = ["wsa", "android", "sensor-fusion", "virtual-sensor", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sensor fusion disabled; Android virtual sensor APIs (rotation vector, gravity) return empty data.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSensorFusion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSensorFusion")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSensorFusion", 1)],
                },
                new TweakDef
                {
                    Id = "wsasnsr-disable-step-counter",
                    Label = "Disable Step Counter Sensor for Android Apps in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android fitness and health apps in WSA from accessing the step counter hardware sensor, preventing pedometer-based location tracking and activity inference.",
                    Tags = ["wsa", "android", "step-counter", "fitness", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Step counter blocked for Android apps; fitness tracking apps in WSA will not count steps.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStepCounterSensor", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStepCounterSensor")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStepCounterSensor", 1)],
                },
            ];
    }

    // ── HolographicDevicePolicy ──
    private static class _HolographicDevicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HolographicDevices";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "holodv-restrict-holographic-device-pairing",
                Label = "Restrict Holographic Device Pairing to Authorized Devices Only",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting holographic device pairing to authorized devices prevents unauthorized HoloLens and Mixed Reality headsets from connecting to enterprise networks and accessing corporate resources. Unauthorized holographic devices connecting to enterprise networks represent a difficult-to-monitor endpoint that may not have endpoint security software installed. HoloLens devices that pair to enterprise networks can potentially access shared resources and data visible to the user identity used for authentication. Device pairing restrictions should require that holographic devices be enrolled in the organization's mobile device management platform before being granted network access. MDM enrollment ensures that holographic devices have security baselines applied including PIN requirements remote wipe capability and application control. Organizations should evaluate the business justification for holographic device usage and implement device-level certificate authentication for authorized devices.",
                Tags = ["hololens", "holographic", "device-pairing", "mixed-reality", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDevicePairing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDevicePairing")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDevicePairing", 1)],
            },
            new TweakDef
            {
                Id = "holodv-disable-developer-mode-hololens",
                Label = "Disable Developer Mode on HoloLens Mixed Reality Devices",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Developer mode on HoloLens devices enables Device Portal access side-loading of unsigned applications and remote debugging capabilities that significantly increase attack surface. Disabling developer mode on enterprise HoloLens deployments ensures that devices can only run applications that have been deployed through the organization's approved application management channels. Developer mode allows remote access to the device file system and processes through Device Portal which is a significant security risk for devices that can record audio visual information and spatial mapping data. Side-loading capabilities in developer mode allow unapproved applications to be installed bypassing the organizational application control policies. Enterprise HoloLens deployments using Commercial Suite and MDM management should have developer mode disabled from the initial device configuration. Organizations should periodically verify that developer mode has not been re-enabled on managed devices through configuration compliance checks.",
                Tags = ["hololens", "developer-mode", "side-loading", "mixed-reality", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeveloperMode", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeveloperMode")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeveloperMode", 1)],
            },
            new TweakDef
            {
                Id = "holodv-require-pin-for-holographic-access",
                Label = "Require PIN or Iris Authentication for HoloLens Device Access",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Requiring PIN or biometric authentication for HoloLens device access prevents unauthorized use of a misplaced or stolen device that could otherwise be accessed without authentication. HoloLens devices store authentication credentials cached application state and potentially recorded spatial data that should be protected against unauthorized physical access. Authentication requirements for holographic devices should align with the organizational policy for mobile device access controls including minimum PIN complexity and biometric fallback policies. Iris recognition on HoloLens provides a convenient and relatively secure biometric authentication method that is appropriate for devices worn on the head. Organizations should configure automatic lock timeouts that require re-authentication after periods of inactivity reducing window of opportunity for unauthorized access to an unattended device. MDM policies that enforce authentication requirements on HoloLens should also configure remote wipe capabilities for devices reported as lost or stolen.",
                Tags = ["hololens", "pin", "authentication", "biometrics", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequirePinForAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePinForAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePinForAccess", 1)],
            },
            new TweakDef
            {
                Id = "holodv-restrict-holographic-telemetry",
                Label = "Restrict Holographic Device Telemetry and Diagnostic Data Transmission",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Holographic device telemetry includes spatial mapping data usage patterns sensor data and system diagnostic information that is transmitted to Microsoft cloud services. Restricting holographic telemetry prevents spatial mapping data which contains detailed three-dimensional representations of the environments where the device is used from leaving organizational control. Spatial mapping data from HoloLens devices used in sensitive manufacturing research or classified environments could reveal facility layouts and equipment configurations. Enterprise deployments using the Commercial Suite should configure telemetry restriction through MDM alongside other data governance policies. The minimum telemetry level configured for standard enterprise devices should also apply to holographic devices as they are subject to the same data governance requirements. Organizations should understand what specific data is transmitted at each telemetry level and select the minimum level that maintains device functionality.",
                Tags = ["hololens", "telemetry", "spatial-data", "data-governance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictHolographicTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictHolographicTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictHolographicTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "holodv-block-unknown-app-installation",
                Label = "Block Installation of Applications from Unknown Sources on HoloLens",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Blocking application installation from unknown sources on HoloLens prevents side-loading of unapproved applications that could introduce malicious code or data exfiltration capabilities. Unknown source applications bypass the organizational application vetting process and may have access to the HoloLens sensors including cameras microphones and spatial mapping capabilities. Applications from unknown sources can capture sensitive environmental data from wherever the device is used which is a significant privacy and security risk for enterprise deployments. Enterprise HoloLens deployments should only allow applications deployed through Microsoft Store for Business or through MDM application deployment to ensure all applications have been reviewed and approved. Application allowlisting for HoloLens should complement the block on unknown sources by defining which applications from approved sources are allowed on enterprise devices. Policy changes that allow unknown sources should be treated as high-risk changes requiring security review and approval.",
                Tags = ["hololens", "application-control", "side-loading", "allowlist", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUnknownAppInstallation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnknownAppInstallation")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnknownAppInstallation", 1)],
            },
            new TweakDef
            {
                Id = "holodv-restrict-camera-access-policy",
                Label = "Restrict HoloLens Camera Access to Authorized Applications",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "HoloLens camera systems including the visible light cameras depth sensors and front-facing cameras can capture video audio and spatial data from the user's environment making camera access control critical for sensitive deployments. Restricting camera access to authorized applications prevents unapproved applications from capturing visual recordings of secure facilities classified equipment or sensitive meetings. Camera access control policy should define the specific applications that are authorized to use each camera type separately since different applications may have different sensor requirements. Environments where HoloLens is used for industrial or research applications may have specific camera access requirements that differ from general enterprise usage policies. Organizations should implement technical controls that prevent camera access revocation from being overridden by applications at runtime. Audit logging for camera access by applications provides visibility into which applications are using camera capabilities and at what times.",
                Tags = ["hololens", "camera-access", "sensor-security", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictCameraAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCameraAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCameraAccess", 1)],
            },
            new TweakDef
            {
                Id = "holodv-enable-remote-management-policy",
                Label = "Enable Remote Management and MDM Enrollment for HoloLens Devices",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Remote management enrollment ensures that HoloLens devices are enrolled in the organizational Mobile Device Management platform enabling policy enforcement remote configuration and remote wipe capabilities. MDM enrollment is the foundation for enterprise HoloLens management providing the control plane for all other security policy enforcement on the device. Without MDM enrollment HoloLens devices operate without organizational policy oversight and cannot have security policies applied or verified remotely. Remote wipe capability through MDM is critical for HoloLens devices which are mobile and can be lost or stolen with organizational data and credentials present. MDM enrollment should be configured to auto-enroll devices upon first setup to ensure that all enterprise devices are enrolled before they are deployed to users. Organizations should define the MDM authority server URL and certificate configuration as part of the holographic device onboarding process.",
                Tags = ["hololens", "mdm", "remote-management", "enrollment", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRemoteManagement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteManagement")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRemoteManagement", 1)],
            },
            new TweakDef
            {
                Id = "holodv-configure-holographic-update-policy",
                Label = "Configure Update Deferral Policy for Holographic Device Operating System",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Update deferral policy for HoloLens devices controls when operating system updates are applied ensuring that updates are tested before deployment to production devices. Immediate uncontrolled update application to holographic devices used in production workflows can cause unexpected behavior changes that disrupt sensitive operations. A deferral period of 7 to 14 days allows organizations to verify that updates do not cause compatibility issues with enterprise applications before they are applied to production devices. Security update deferrals should be minimized as HoloLens security patches address vulnerabilities in a device with significant sensor capabilities. Organizations should maintain a test group of HoloLens devices that receive updates on the standard timeline to detect compatibility issues before the production fleet updates. Update deferral policy should balance security patch velocity against operational stability requirements.",
                Tags = ["hololens", "updates", "deferral", "patch-management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigureUpdateDeferral", 7)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigureUpdateDeferral")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigureUpdateDeferral", 7)],
            },
            new TweakDef
            {
                Id = "holodv-restrict-cross-device-experiences",
                Label = "Restrict Cross-Device Experience Sharing on Holographic Platforms",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Cross-device experience sharing on holographic platforms enables activity and clipboard sharing between HoloLens devices and paired Windows computers which can expose sensitive data if the paired device is not managed. Restricting cross-device experiences prevents holographic device activity data including application usage state and clipboard content from being shared with external devices. Activity sharing with unmanaged devices creates a data leakage path where content from managed HoloLens sessions can be transferred to personal devices outside organizational control. Cross-device sharing should only be permitted between managed organizational devices that have the same security policy applied. Organizations should define which cross-device experience features are permissible and configure policy to allow only those specific features while blocking others. Monitoring for cross-device sharing events can help detect attempts to circumvent data governance through holographic device cross-device features.",
                Tags = ["hololens", "cross-device", "data-sharing", "data-governance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictCrossDeviceExperiences", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictCrossDeviceExperiences")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictCrossDeviceExperiences", 1)],
            },
            new TweakDef
            {
                Id = "holodv-enable-holographic-audit-events",
                Label = "Enable Audit Event Logging for Holographic Device Operations",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Holographic device audit event logging captures device operations including authentication events application launches sensor access and policy changes providing visibility for security monitoring. Audit events from HoloLens devices should be forwarded to centralized log management for correlation with network and identity events. Sensor access audit events are particularly important for HoloLens devices as they reveal which applications are using camera microphone and spatial mapping capabilities at what times. Authentication audit events for holographic devices help detect unauthorized access attempts including brute force attacks on PIN codes. Organizations should configure the audit event forwarding to ensure that log data is not retained solely on the device where it might be lost if the device is damaged or reset. Holographic device audit data combined with network access logs provides comprehensive visibility into how and when enterprise holographic devices are used.",
                Tags = ["hololens", "audit", "event-logging", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableHolographicAuditEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableHolographicAuditEvents")],
                DetectOps = [RegOp.CheckDword(Key, "EnableHolographicAuditEvents", 1)],
            },
        ];
    }

    // ── HyperVContainerPolicy ──
    private static class _HyperVContainerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV";
        private const string VmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VirtualMachineMonitor";
        private const string CtrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Containers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "hvcon-require-admin-vm-creation",
                    Label = "Require Administrator to Create Hyper-V Virtual Machines",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Restricts virtual machine creation in Hyper-V to administrator accounts only, preventing standard users from provisioning new VMs that could be used to bypass security policy controls on the host system.",
                    Tags = ["hyper-v", "vm-creation", "admin", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Hyper-V VM creation restricted to admins; standard users cannot provision new virtual machines.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForVMCreation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForVMCreation")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForVMCreation", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-disable-vm-network-passthrough",
                    Label = "Disable Network Passthrough (SR-IOV) for Hyper-V VMs",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents Hyper-V virtual machines from using SR-IOV (Single Root I/O Virtualisation) network passthrough, ensuring all VM network traffic flows through the Hyper-V virtual switch for monitoring and filtering.",
                    Tags = ["hyper-v", "sriov", "network-passthrough", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "SR-IOV network passthrough disabled; VM traffic routed through vSwitch for visibility and filtering.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSRIOVPassthrough", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSRIOVPassthrough")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSRIOVPassthrough", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-block-live-migration-plain",
                    Label = "Block Unencrypted Hyper-V Live Migration",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents Hyper-V live migrations from using the unencrypted migration transport, requiring Kerberos authentication or SMB encryption for all live migration sessions to prevent VM memory interception during migration.",
                    Tags = ["hyper-v", "live-migration", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Plaintext live migration blocked; Kerberos/SMB encryption required for all VM live migration sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireEncryptedLiveMigration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireEncryptedLiveMigration")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireEncryptedLiveMigration", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-set-vm-memory-limit",
                    Label = "Set Maximum Hyper-V VM Memory to 64 GB Per VM",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Limits the maximum amount of RAM that any single Hyper-V virtual machine can be assigned to 65536 MB (64 GB), preventing individual VMs from monopolising all host memory and providing fair resource sharing.",
                    Tags = ["hyper-v", "memory-limit", "resources", "fairness", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Maximum RAM per Hyper-V VM capped at 64 GB; no single VM can consume all host memory.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxVMMemoryMB", 65536)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxVMMemoryMB")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxVMMemoryMB", 65536)],
                },
                new TweakDef
                {
                    Id = "hvcon-enable-secure-boot-for-vms",
                    Label = "Enforce Secure Boot Enabled for All New Hyper-V VMs",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Requires that all new Hyper-V Generation 2 virtual machines are created with Secure Boot enabled, preventing VMs from being provisioned with Secure Boot disabled and booting unsigned guest operating systems.",
                    Tags = ["hyper-v", "secure-boot", "generation-2", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Secure Boot enforced for all new Gen2 Hyper-V VMs; unsigned guest OS images cannot be booted.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceSecureBootForNewVMs", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureBootForNewVMs")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceSecureBootForNewVMs", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-disable-vm-clipboard",
                    Label = "Disable Clipboard Sharing Between Hyper-V VMs and Host",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents clipboard data from being passed between Hyper-V virtual machine sessions and the host desktop, eliminating the clipboard as a data exfiltration channel between VM and host environments.",
                    Tags = ["hyper-v", "clipboard", "isolation", "data-protection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard sharing between Hyper-V VMs and host disabled; data cannot be copy/pasted across VM boundary.",
                    ApplyOps = [RegOp.SetDword(VmKey, "DisableVMClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(VmKey, "DisableVMClipboard")],
                    DetectOps = [RegOp.CheckDword(VmKey, "DisableVMClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-disable-vm-drives-mount",
                    Label = "Disable VM Drive Mounting from Guest to Host",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents Hyper-V virtual machines from mounting host filesystem paths via VMBus drive sharing, ensuring guest VMs cannot read host files through the Hyper-V file share integration component.",
                    Tags = ["hyper-v", "drive-mounting", "integration", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VM drive mounting from guest to host disabled; Hyper-V file share integration component blocked.",
                    ApplyOps = [RegOp.SetDword(VmKey, "DisableVMDriveSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(VmKey, "DisableVMDriveSharing")],
                    DetectOps = [RegOp.CheckDword(VmKey, "DisableVMDriveSharing", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-block-container-user-creation",
                    Label = "Block Standard Users from Creating Windows Containers",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Restricts Windows Container (Hyper-V isolation / process isolation) session creation to administrator accounts, preventing standard users from running containerised workloads that could bypass host security controls.",
                    Tags = ["containers", "windows-container", "standard-user", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Windows Container creation restricted to admins; standard users cannot run Docker/containerd containers.",
                    ApplyOps = [RegOp.SetDword(CtrKey, "RequireAdminForContainerCreation", 1)],
                    RemoveOps = [RegOp.DeleteValue(CtrKey, "RequireAdminForContainerCreation")],
                    DetectOps = [RegOp.CheckDword(CtrKey, "RequireAdminForContainerCreation", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-disable-hyper-v-telemetry",
                    Label = "Disable Hyper-V and Container Management Telemetry to Microsoft",
                    Category = "Virtualization — Android App Debugging",
                    Description = "Prevents Hyper-V and Windows Containers from sending VM usage, crash, and performance telemetry to Microsoft.",
                    Tags = ["hyper-v", "containers", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hyper-V and container telemetry to Microsoft disabled; VM usage and performance data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableHyperVTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableHyperVTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableHyperVTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "hvcon-log-vm-lifecycle-events",
                    Label = "Log Hyper-V VM Lifecycle Events in System Event Log",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Enables System event log entries for Hyper-V virtual machine creation, deletion, start, stop, and live migration events, providing a complete audit trail of VM lifecycle changes for compliance.",
                    Tags = ["hyper-v", "event-log", "audit", "lifecycle", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Hyper-V VM lifecycle events logged; create/start/stop/migrate events visible in System log for auditing.",
                    ApplyOps = [RegOp.SetDword(Key, "LogVMLifecycleEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogVMLifecycleEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "LogVMLifecycleEvents", 1)],
                },
            ];
    }

    // ── WindowsContainerPolicy ──
    private static class _WindowsContainerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Containers";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wincnt-disable-container-network-access",
                Label = "Disable Container Unrestricted Network Access",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows containers can be configured with different network isolation levels ranging from full host network access to complete isolation. Disabling unrestricted container network access prevents containers from having the same network visibility as the host operating system. Containers with full host network access bypass container network isolation and can reach host-accessible services and network resources. Compromised containers with host network access can attack other hosts and services that would otherwise be outside container reach. Enterprise container policies should enforce network isolation appropriate to the sensitivity of the workload running in each container. Containers requiring specific network connectivity should use explicitly defined virtual networks rather than shared host networking.",
                Tags = ["containers", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictContainerNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictContainerNetworkAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictContainerNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-disable-host-device-access",
                Label = "Disable Container Host Device Access",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Containers may be configured to access physical devices attached to the host system including storage devices, USB controllers, and other hardware. Disabling host device access from containers prevents container workloads from directly accessing physical hardware connected to the host. Direct device access from containers can allow container workloads to access sensitive data on host storage devices beyond the container file system. Physical device access bypasses container isolation boundaries and provides a path for malicious containers to affect host hardware state. Enterprise container policies should restrict device access to only explicitly required and approved devices rather than allowing broad hardware access. Containers should access persistent data through volume mounts with appropriate permissions rather than direct device access.",
                Tags = ["containers", "devices", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeviceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeviceAccess", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-enforce-isolation-level",
                Label = "Enforce Minimum Container Isolation Level",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows containers support different isolation levels from process isolation sharing the host kernel to Hyper-V isolation using separate virtual machine kernels. Enforcing minimum isolation level requirements ensures containers cannot be started with weaker isolation than enterprise policy allows. Process-isolated containers share the Windows kernel with the host making kernel vulnerabilities directly exploitable from within containers. Hyper-V isolated containers use virtual machine boundaries providing much stronger protection against kernel-level exploits. Enterprise security policies should require Hyper-V isolation for containers handling sensitive workloads or serving external traffic. Enforcing minimum isolation through policy prevents developers from inadvertently deploying containers with insufficient isolation guarantees.",
                Tags = ["containers", "isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceIsolationLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceIsolationLevel")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceIsolationLevel", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-disable-container-image-pull-insecure",
                Label = "Disable Container Image Pull from Insecure Registries",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Container images can be pulled from registries using either HTTPS secured connections or insecure unencrypted HTTP connections. Disabling insecure container image pulls ensures that all image downloads use encrypted HTTPS connections to verify registry identity. HTTP-sourced container images are vulnerable to man-in-the-middle attacks that can inject malicious code or replace the legitimate image. Insecure registry connections cannot be authenticated making it impossible to verify that the image came from the intended source. Enterprise container policies should require all registry communication to use HTTPS with valid certificates from trusted authorities. Preventing insecure pulls is a fundamental container supply chain security requirement.",
                Tags = ["containers", "registry", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInsecureRegistryPull", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInsecureRegistryPull")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInsecureRegistryPull", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-require-image-signing",
                Label = "Require Signed Container Images",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Container image signing allows image publishers to cryptographically sign images ensuring integrity and provenance can be verified before deployment. Requiring signed container images prevents execution of images that have not been signed by a trusted key. Unsigned container images cannot be verified to originate from the expected publisher or remain unmodified since publication. Supply chain attacks on container registries can replace legitimate images with malicious versions that would not have valid signatures. Enterprise container policies should require that only images signed by approved keys can be deployed to production endpoints. Image signing requirements should be combined with a key management policy specifying which signing keys are trusted.",
                Tags = ["containers", "signing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireImageSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireImageSigning")],
                DetectOps = [RegOp.CheckDword(Key, "RequireImageSigning", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-disable-privileged-containers",
                Label = "Disable Privileged Container Execution",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Privileged containers run with elevated privileges that grant them access to host capabilities typically restricted from unprivileged containers. Disabling privileged container execution prevents containers from being launched with elevated host privileges that bypass isolation. Privileged containers can often escape container isolation and directly access host resources including file systems and network interfaces. Workloads running in privileged containers are treated essentially as trusted processes rather than isolated workloads. Security vulnerabilities in privileged container workloads have a much higher impact due to their elevated access to host systems. Most legitimate container workloads can be designed to operate without privileged access through appropriate capability management.",
                Tags = ["containers", "privileges", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrivilegedContainers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivilegedContainers")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrivilegedContainers", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-disable-container-mounts",
                Label = "Restrict Container Host Volume Mounts",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Container host volume mounts allow containers to access specific directories on the host file system through bind mounts. Restricting volume mounts prevents containers from accessing sensitive host directories that should remain isolated from container workloads. Overly permissive volume mounts can expose host system files, secrets, and certificates to container workloads. Compromised containers with sensitive host mounts can read configuration files, private keys, and security credentials. Container workloads should access only specifically scoped directories with minimal permissions rather than broad host file system access. Volume mount restrictions prevent accidental or malicious access to host system files from within container environments.",
                Tags = ["containers", "mounts", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictHostMounts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictHostMounts")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictHostMounts", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-enable-container-audit-logging",
                Label = "Enable Container Activity Audit Logging",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Container audit logging records lifecycle events including container start stop and significant operations performed by container workloads. Enabling container audit logging provides visibility into container operations for security monitoring and forensic investigation. Detection of malicious container activity requires that logs of container operations be available to security teams. Container audit events should be forwarded to central logging infrastructure alongside other Windows Event Log security events. Compliance frameworks including SOC 2 and PCI DSS require audit logging of infrastructure operations including containerized workloads. Audit logging overhead is minimal and provides significant security and compliance value.",
                Tags = ["containers", "audit", "logging", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableContainerAuditLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableContainerAuditLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableContainerAuditLogging", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-restrict-registry-access",
                Label = "Restrict Container Host Registry Access",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Container workloads may be configured to access host registry hives directly through registry mounts similar to volume mounts for file system. Restricting host registry access from containers prevents container workloads from reading or modifying host system configuration. Host registry hives contain sensitive configuration data including credentials, certificates, and security policy settings. Containers with host registry access can read security credentials and configuration data from a registry hive mounted as writable. Malicious container workloads modifying the host registry can persist configuration changes that survive container deletion. Containers should use their own isolated registry hives rather than accessing host registry data.",
                Tags = ["containers", "registry", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictRegistryAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictRegistryAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictRegistryAccess", 1)],
            },
            new TweakDef
            {
                Id = "wincnt-disable-container-telemetry",
                Label = "Disable Container Telemetry Collection",
                Category = "Virtualization — Android App Debugging",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "The Windows container runtime collects telemetry about container usage patterns including image usage, runtime performance, and configuration details. Disabling container telemetry prevents container deployment and usage data from being transmitted to Microsoft. Container usage patterns and image references represent sensitive information about enterprise application architecture. Software inventory derived from container image references can reveal internal application portfolio and development practices. Enterprise container operations monitoring should be performed through internal tools rather than external telemetry collection. Disabling telemetry does not affect container functionality including creation, execution, or management operations.",
                Tags = ["containers", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableContainerTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableContainerTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableContainerTelemetry", 1)],
            },
        ];
    }

    // ── WindowsSandboxPolicy ──
    private static class _WindowsSandboxPolicy
    {
        private const string SandboxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sbpol-disable-sandbox",
                Label = "Disable Windows Sandbox",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowSandbox=0 in the Sandbox policy key. "
                    + "Disables Windows Sandbox entirely via Group Policy. Windows Sandbox is a "
                    + "lightweight isolated virtual environment for running untrusted applications. "
                    + "While beneficial for security testing, it requires Hyper-V and consumes significant "
                    + "CPU/RAM. Organisations that do not need it can disable the feature via GPO rather "
                    + "than relying solely on the optional-features toggle. "
                    + "Default: absent (Sandbox allowed if feature is installed). "
                    + "Recommended: 0 on production servers and systems where Sandbox is not needed.",
                Tags = ["sandbox", "virtualization", "isolation", "policy", "hyperv"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Windows Sandbox blocked by policy; cannot be used even when the optional feature is installed.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowSandbox", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowSandbox")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowSandbox", 0)],
            },
            new TweakDef
            {
                Id = "sbpol-disable-clipboard",
                Label = "Disable Clipboard Sharing With Windows Sandbox",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowClipboardRedirection=0 in the Sandbox policy key. "
                    + "Blocks bi-directional clipboard sharing between the Windows Sandbox and the host OS. "
                    + "Without this restriction, content copied inside the sandbox (e.g., stolen credentials "
                    + "or extracted secrets harvested by a malicious app) can be trivially transferred to "
                    + "the host by pasting. Similarly, sensitive content from the host can be accidentally "
                    + "pasted into untrustworthy sandbox applications. "
                    + "Default: absent (clipboard shared). Recommended: 0 for true isolation.",
                Tags = ["sandbox", "clipboard", "data-exfiltration", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Copy/paste between Sandbox and host blocked in both directions.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowClipboardRedirection", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowClipboardRedirection")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowClipboardRedirection", 0)],
            },
            new TweakDef
            {
                Id = "sbpol-disable-printer-redirection",
                Label = "Disable Printer Redirection Into Windows Sandbox",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowPrinterRedirection=0 in the Sandbox policy key. "
                    + "Prevents printers that are in scope on the host from being redirected and made "
                    + "available inside the Windows Sandbox session. Printer redirection exposes the "
                    + "host's print subsystem to the sandbox, and a malicious application could use it "
                    + "to print documents, discover IP print server addresses, or trigger driver invocations "
                    + "against the host print spooler. Blocking redirected printing tightens isolation. "
                    + "Default: absent (printers accessible in Sandbox). Recommended: 0 for isolation.",
                Tags = ["sandbox", "printing", "isolation", "spooler", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "No host printers visible inside Windows Sandbox; print spooler not exposed.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowPrinterRedirection", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowPrinterRedirection")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowPrinterRedirection", 0)],
            },
            new TweakDef
            {
                Id = "sbpol-disable-audio-input",
                Label = "Disable Microphone (Audio Input) in Windows Sandbox",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowAudioInput=0 in the Sandbox policy key. "
                    + "Blocks the microphone and other audio capture devices from being accessible inside "
                    + "the Windows Sandbox session. An untrusted application running in Sandbox could use "
                    + "microphone access to capture ambient audio (conversation recording, credential "
                    + "dictation) and exfiltrate it if networking is also enabled. Disabling microphone "
                    + "inside Sandbox removes this passive surveillance capability. "
                    + "Default: absent (microphone accessible). Recommended: 0 for privacy.",
                Tags = ["sandbox", "microphone", "audio", "privacy", "surveillance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Microphone not accessible inside Windows Sandbox; audio capture by sandboxed apps prevented.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowAudioInput", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowAudioInput")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowAudioInput", 0)],
            },
            new TweakDef
            {
                Id = "sbpol-disable-video-input",
                Label = "Disable Camera (Video Input) in Windows Sandbox",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowVideoInput=0 in the Sandbox policy key. "
                    + "Blocks webcams and other video capture devices from being visible inside the "
                    + "Windows Sandbox session. A sandboxed malicious application with camera access "
                    + "could silently capture images or video of the user's environment and exfiltrate it. "
                    + "Disabling video input in Sandbox eliminates this covert visual surveillance path "
                    + "without affecting the host system's camera access outside Sandbox. "
                    + "Default: absent (camera accessible). Recommended: 0 for privacy.",
                Tags = ["sandbox", "camera", "video", "privacy", "surveillance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Camera not accessible inside Windows Sandbox; video capture by sandboxed apps prevented.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowVideoInput", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowVideoInput")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowVideoInput", 0)],
            },
            new TweakDef
            {
                Id = "sbpol-disable-mapped-folders",
                Label = "Disable Host Folder Mapping Into Windows Sandbox",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowMappedFolders=0 in the Sandbox policy key. "
                    + "Prevents host filesystem folders from being mapped and shared into the Windows Sandbox "
                    + "environment via the Sandbox configuration file (WSB). Without this restriction, a "
                    + "carefully crafted .wsb config file can mount sensitive host directories (Documents, "
                    + "Desktop, code repositories) as writable shares inside Sandbox, enabling a malicious "
                    + "payload to read or modify host files. Blocking mapped folders enforces full filesystem "
                    + "isolation. Default: absent (folder mapping allowed). Recommended: 0 for strict isolation.",
                Tags = ["sandbox", "filesystem", "folder-mapping", "isolation", "data-access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Host filesystem folders cannot be mapped into Sandbox; .wsb MappedFolders entries blocked.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowMappedFolders", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowMappedFolders")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowMappedFolders", 0)],
            },
            new TweakDef
            {
                Id = "sbpol-disable-mapped-folders-write",
                Label = "Restrict Mapped Sandbox Folders to Read-Only",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowWritableSharedFolders=0 in the Sandbox policy key. "
                    + "Even when folder mapping is allowed (AllowMappedFolders=1), this policy ensures "
                    + "that all mapped host folders are mounted as read-only inside the Sandbox. "
                    + "A sandboxed application can read shared files but cannot write back to the host "
                    + "filesystem. This provides a middle ground: files can be passed into Sandbox for "
                    + "analysis without allowing the Sandbox to modify or delete them. "
                    + "Default: absent (writable mapping permitted). Recommended: 0 if mapping is enabled.",
                Tags = ["sandbox", "filesystem", "read-only", "folder-mapping", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Mapped host folders accessible read-only inside Sandbox; sandbox cannot write to host filesystem.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowWritableSharedFolders", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowWritableSharedFolders")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowWritableSharedFolders", 0)],
            },
            new TweakDef
            {
                Id = "sbpol-restrict-logon-credentials",
                Label = "Block Windows Logon Credential Exposure in Windows Sandbox",
                Category = "Virtualization — Android App Debugging",
                Description =
                    "Sets AllowLogonCredentials=0 in the Sandbox policy key. "
                    + "Prevents Windows from passing or forwarding the user's login credentials (tokens, "
                    + "tickets, or cached NTLM hashes) into the Windows Sandbox environment. Without this "
                    + "setting, a sandboxed process may be able to leverage inherited authentication tokens "
                    + "to access domain resources or network shares as the current user. Blocking credential "
                    + "propagation ensures that a compromised Sandbox cannot pivot to domain resources. "
                    + "Default: absent. Recommended: 0 on domain-joined systems.",
                Tags = ["sandbox", "credentials", "ntlm", "token", "domain", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Login credentials not forwarded into Sandbox; sandboxed apps cannot use domain identity.",
                ApplyOps = [RegOp.SetDword(SandboxKey, "AllowLogonCredentials", 0)],
                RemoveOps = [RegOp.DeleteValue(SandboxKey, "AllowLogonCredentials")],
                DetectOps = [RegOp.CheckDword(SandboxKey, "AllowLogonCredentials", 0)],
            },
        ];
    }

    // ── WsaAndroidPolicy ──
    private static class _WsaAndroidPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsacore-disable-wsa",
                    Label = "Disable Windows Subsystem for Android",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables the Windows Subsystem for Android (WSA) entirely, preventing Android app installation and the associated Amazon Appstore service from running.",
                    Tags = ["wsa", "android", "subsystem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSA disabled; Android apps cannot be installed or launched.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWindowsSubsystemForAndroid", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsSubsystemForAndroid")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWindowsSubsystemForAndroid", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-block-amazon-appstore",
                    Label = "Block Amazon Appstore Integration with WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks the Amazon Appstore integration in WSA, preventing users from browsing, installing, or updating Android apps via the Amazon storefront.",
                    Tags = ["wsa", "amazon-appstore", "android", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Amazon Appstore blocked in WSA; Android app discovery and install blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAmazonAppstoreIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAmazonAppstoreIntegration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAmazonAppstoreIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-disable-android-diagnostics",
                    Label = "Disable WSA Diagnostic Data Upload",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables the upload of Android runtime diagnostic data (crash reports, performance telemetry) from WSA to Microsoft and Amazon servers.",
                    Tags = ["wsa", "android", "diagnostics", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSA diagnostic data not uploaded; Android app crashes not sent to telemetry endpoints.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidDiagnosticsUpload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidDiagnosticsUpload")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidDiagnosticsUpload", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-disable-wsa-autostart",
                    Label = "Disable WSA Auto-Start on Windows Startup",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents the WSA container virtual machine from starting automatically on Windows boot, reducing memory and CPU overhead on systems where Android apps are rarely used.",
                    Tags = ["wsa", "android", "autostart", "startup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSA VM not auto-started; first Android app launch takes slightly longer.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSAAutoStart", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSAAutoStart")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSAAutoStart", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-block-android-clipboard",
                    Label = "Block Android App Access to Windows Clipboard",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents Android applications running in WSA from reading or writing the Windows clipboard, isolating Android app clipboard access from sensitive Windows application data.",
                    Tags = ["wsa", "android", "clipboard", "isolation", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android apps cannot access Windows clipboard; paste between Windows and Android apps blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidClipboardAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidClipboardAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidClipboardAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-block-android-camera",
                    Label = "Block Android App Camera Access in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents Android applications in WSA from accessing the Windows system camera, blocking Android apps from using the webcam or integrated camera hardware.",
                    Tags = ["wsa", "android", "camera", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Camera access blocked for Android apps in WSA; video calls and photo apps in WSA cannot use the webcam.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidCameraAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidCameraAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidCameraAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-block-android-microphone",
                    Label = "Block Android App Microphone Access in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Prevents Android applications in WSA from accessing the system microphone, blocking audio recording by Android apps running in the Windows subsystem.",
                    Tags = ["wsa", "android", "microphone", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Microphone blocked for Android apps in WSA; voice recording Android apps will see no audio input.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidMicrophoneAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidMicrophoneAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidMicrophoneAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-disable-android-gpu",
                    Label = "Disable Android GPU Hardware Acceleration in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Disables hardware GPU acceleration for the WSA Android container, forcing software rendering, which reduces GPU load and prevents direct GPU driver access from Android apps.",
                    Tags = ["wsa", "android", "gpu", "acceleration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Android GPU acceleration disabled; apps use software rendering (slower but isolated from GPU driver).",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidGPUAcceleration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidGPUAcceleration")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidGPUAcceleration", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-restrict-android-location",
                    Label = "Restrict Android App Location Access in WSA",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Blocks Android applications in WSA from accessing the Windows location service, preventing Android apps from determining geolocation via GPS, Wi-Fi triangulation, or IP-based lookup.",
                    Tags = ["wsa", "android", "location", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Location blocked for Android apps in WSA; apps request location and receive denials.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidLocationAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidLocationAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidLocationAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsacore-limit-android-memory",
                    Label = "Limit WSA Container Memory to 4 GB",
                    Category = "Virtualization — Android App Debugging",
                    Description =
                        "Limits the maximum RAM allocation for the WSA Android container to 4 GB, preventing Android apps from consuming excessive system memory on devices with limited RAM.",
                    Tags = ["wsa", "android", "memory", "resource-limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSA container capped at 4 GB RAM; memory-intensive Android apps may be terminated by the Android OOM killer.",
                    ApplyOps = [RegOp.SetDword(Key, "AndroidContainerMaxMemoryMB", 4096)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AndroidContainerMaxMemoryMB")],
                    DetectOps = [RegOp.CheckDword(Key, "AndroidContainerMaxMemoryMB", 4096)],
                },
            ];
    }

    // ── WsaNetworkIsolationPolicy ──
    private static class _WsaNetworkIsolationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Network";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsanet-block-android-internet-access",
                    Label = "Block Android Container Internet Access",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Blocks all outbound internet access from the WSA Android container, allowing Android apps to run offline-only without connecting to internet services.",
                    Tags = ["wsa", "android", "network", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Android container offline; all WSA Android app network calls fail. Only local localhost communication permitted.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidInternetAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidInternetAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidInternetAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-block-android-local-network",
                    Label = "Block Android Container Local Network Access",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents the WSA Android container from accessing the local area network, stopping Android apps from scanning or communicating with LAN resources and IoT devices.",
                    Tags = ["wsa", "android", "lan", "network", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSA container LAN access blocked; Android apps cannot reach local network devices.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidLANAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidLANAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidLANAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-disable-android-vpn-client",
                    Label = "Disable Android VPN Client within WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Disables the Android VPN API within WSA, preventing Android VPN apps from creating VPN tunnels that could route all Windows traffic through an Android-configured tunnel.",
                    Tags = ["wsa", "android", "vpn", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Android VPN API disabled in WSA; Android VPN apps cannot create tunnels or intercept Windows traffic.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidVPNClient", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidVPNClient")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidVPNClient", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-block-android-peer-to-peer",
                    Label = "Block Android P2P Wi-Fi Direct in WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Blocks Android Wi-Fi Direct (P2P) in WSA, preventing Android apps from creating ad-hoc Wi-Fi connections to nearby devices that bypass enterprise network controls.",
                    Tags = ["wsa", "android", "wifi-direct", "p2p", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Wi-Fi Direct blocked in WSA; Android apps cannot create direct peer connections.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidWifiDirect", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidWifiDirect")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidWifiDirect", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-disable-android-hotspot",
                    Label = "Disable Android Mobile Hotspot via WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Disables the ability for Android apps in WSA to activate a Wi-Fi hotspot, preventing an Android app from sharing the Windows internet connection without authorisation.",
                    Tags = ["wsa", "android", "hotspot", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android hotspot creation blocked in WSA; Android tethering apps have no effect.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidHotspot", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidHotspot")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidHotspot", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-restrict-android-dns",
                    Label = "Restrict Android Container to Enterprise DNS",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Forces the WSA Android container to use the enterprise DNS servers configured for the Windows host, preventing Android apps from using public or hardcoded DNS resolvers (DNS-over-HTTPS bypass).",
                    Tags = ["wsa", "android", "dns", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android DNS forced to enterprise resolvers; hardcoded Android DNS-over-HTTPS bypassed.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceAndroidEnterpriseDNS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceAndroidEnterpriseDNS")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceAndroidEnterpriseDNS", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-block-android-nfc",
                    Label = "Block NFC Access for Android Apps in WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Blocks Android NFC APIs in WSA, preventing Android apps from accessing the Windows NFC stack if present, and stopping NFC-based contactless payment or data transfer by Android apps.",
                    Tags = ["wsa", "android", "nfc", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "NFC access blocked for Android apps in WSA; contactless payment apps and NFC reader apps have no hardware access.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidNFCAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidNFCAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidNFCAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-disable-android-bluetooth",
                    Label = "Disable Bluetooth Access for Android Apps in WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Disables Bluetooth access for Android applications in WSA, preventing Android apps from pairing with or communicating via Bluetooth peripherals using the Windows Bluetooth stack.",
                    Tags = ["wsa", "android", "bluetooth", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Bluetooth blocked for Android apps in WSA; BT audio/file-transfer Android apps have no hardware access.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidBluetooth", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidBluetooth")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidBluetooth", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-block-android-background-data",
                    Label = "Block Android Background Data Usage in WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Blocks Android apps in WSA from using network connectivity in the background (when the app screen is not visible), reducing data usage and preventing hidden data exfiltration.",
                    Tags = ["wsa", "android", "background-data", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Background Android networking blocked; apps only access network when their window is active.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidBackgroundData", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidBackgroundData")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidBackgroundData", 1)],
                },
                new TweakDef
                {
                    Id = "wsanet-log-android-network-activity",
                    Label = "Enable Audit Logging for Android Network Activity",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Enables event logging for all network connections initiated by Android applications in WSA, providing visibility into Android app network behaviour for security monitoring.",
                    Tags = ["wsa", "android", "network", "logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android app network connections logged; outbound connections from WSA visible in event log.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableAndroidNetworkAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableAndroidNetworkAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableAndroidNetworkAuditLog", 1)],
                },
            ];
    }

    // ── WsaStoragePolicy ──
    private static class _WsaStoragePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsSubsystemForAndroid\Storage";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsastor-block-android-host-file-access",
                    Label = "Block Android Container Access to Windows File System",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents Android applications in WSA from accessing Windows host file system paths (outside the dedicated Android container storage), isolating Android apps from Windows user documents and system files.",
                    Tags = ["wsa", "android", "storage", "file-system", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Android apps confined to container storage; Windows file system paths invisible to Android apps.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidHostFileAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidHostFileAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidHostFileAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-disable-android-sd-card",
                    Label = "Disable Android Virtual SD Card in WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Disables the virtual SD card / removable storage emulation in WSA, preventing Android apps from accessing or exfiltrating data via the Android external storage API.",
                    Tags = ["wsa", "android", "sd-card", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Virtual SD card removed from WSA; apps using external storage API will see no removable storage.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidSDCard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidSDCard")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidSDCard", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-restrict-android-download-folder",
                    Label = "Restrict Android Download Folder to Container Only",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Restricts the Android Downloads folder to within the WSA container, preventing downloaded files from automatically syncing to the Windows Downloads folder.",
                    Tags = ["wsa", "android", "download", "storage", "isolation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android downloads stay in container; no automatic sync to Windows Downloads folder.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictAndroidDownloadToContainer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictAndroidDownloadToContainer")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictAndroidDownloadToContainer", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-limit-container-storage",
                    Label = "Limit WSA Container Storage to 16 GB",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Caps the WSA Android container image size at 16 GB, preventing Android apps from consuming excessive disk space on devices with limited storage.",
                    Tags = ["wsa", "android", "storage", "quota", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSA container storage capped at 16 GB; app installs fail when limit is reached.",
                    ApplyOps = [RegOp.SetDword(Key, "ContainerMaxStorageGB", 16)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ContainerMaxStorageGB")],
                    DetectOps = [RegOp.CheckDword(Key, "ContainerMaxStorageGB", 16)],
                },
                new TweakDef
                {
                    Id = "wsastor-block-android-usb-transfer",
                    Label = "Block Android MTP/USB File Transfer from WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Blocks the Android Media Transfer Protocol (MTP) and USB file transfer APIs in WSA, preventing Android apps from transferring files to/from connected USB storage devices.",
                    Tags = ["wsa", "android", "mtp", "usb", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android MTP/USB transfer blocked; Android apps cannot access or write to USB drives.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidMTPTransfer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidMTPTransfer")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidMTPTransfer", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-disable-android-photos-sync",
                    Label = "Disable Android Photos Auto-Sync with Windows Photos",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Disables automatic synchronisation between the Android container photo gallery and the Windows Photos app, preventing Android photo data from being accessible to Windows apps without explicit sharing.",
                    Tags = ["wsa", "android", "photos", "sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Android photos not synced to Windows Photos; camera roll data stays in Android container.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidPhotosSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidPhotosSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidPhotosSync", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-block-android-contact-access",
                    Label = "Block Android App Access to Windows Contacts",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents Android applications in WSA from reading Windows People/Contacts data, isolating the Android contact database from the Windows contact store.",
                    Tags = ["wsa", "android", "contacts", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Android apps cannot read Windows contacts; contact harvesting by Android apps prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidContactAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidContactAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidContactAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-disable-android-calendar-sync",
                    Label = "Disable Android Calendar Sync with Windows Calendar",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Disables calendar event synchronisation between Android apps in WSA and the Windows Calendar app, preventing calendar data from crossing the Android/Windows boundary.",
                    Tags = ["wsa", "android", "calendar", "sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Android and Windows calendars not synced; schedule data stays in its respective system.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAndroidCalendarSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAndroidCalendarSync")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAndroidCalendarSync", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-enable-android-storage-audit",
                    Label = "Enable Audit Logging for Android Storage Operations",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Enables event log entries for significant Android storage operations (large reads/writes, external storage access) to provide visibility into Android app file system behaviour.",
                    Tags = ["wsa", "android", "storage", "audit", "logging", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Android storage events logged; bulk read/write operations by Android apps are auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableAndroidStorageAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableAndroidStorageAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableAndroidStorageAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "wsastor-block-android-screencapture",
                    Label = "Block Screenshot/Screen Recording by Android Apps in WSA",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents Android applications in WSA from capturing screenshots or recording the screen, ensuring Android apps cannot exfiltrate screen contents to their cloud services.",
                    Tags = ["wsa", "android", "screenshot", "screen-recording", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Android screenshot and screen recording APIs blocked in WSA; screen data cannot be saved or uploaded by Android apps.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAndroidScreenCapture", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAndroidScreenCapture")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAndroidScreenCapture", 1)],
                },
            ];
    }

    // ── Wsl2AdvancedPolicy ──
    private static class _Wsl2AdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss";
        private const string FwKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Firewall";
        private const string NwKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Networking";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsl2adv-disable-wsl2-entirely",
                    Label = "Disable WSL2 (Windows Subsystem for Linux 2) Entirely",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Completely disables WSL2 via policy, preventing installation of Linux distributions and blocking any WSL2 virtual machine from starting. Applied on endpoints where Linux environments are not permitted.",
                    Tags = ["wsl2", "linux", "subsystem", "disable", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 fully disabled; no Linux distro installation or execution possible on this endpoint.",
                    ApplyOps = [RegOp.SetDword(Key, "DefaultVersion", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DefaultVersion")],
                    DetectOps = [RegOp.CheckDword(Key, "DefaultVersion", 0)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-block-network-access",
                    Label = "Block WSL2 Instances from Making Network Connections",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents WSL2 virtual machine network interfaces from making outbound connections, isolating Linux workloads from the network when running untrusted code inside WSL2 distros.",
                    Tags = ["wsl2", "network", "isolation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 network access blocked; Linux distros run air-gapped with no internet or LAN connectivity.",
                    ApplyOps = [RegOp.SetDword(NwKey, "BlockOutboundNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(NwKey, "BlockOutboundNetwork")],
                    DetectOps = [RegOp.CheckDword(NwKey, "BlockOutboundNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-block-system-distro",
                    Label = "Block Installation of WSL2 System Distributions",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents the installation of Microsoft-managed system utility distributions (Docker Desktop backend, cloud-agent distros) from being registered in WSL2, limiting distros to user-managed ones.",
                    Tags = ["wsl2", "system-distro", "docker", "cloud-agent", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "System WSL2 distros blocked; Docker Desktop WSL2 backend and cloud agent distros cannot register.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockSystemDistros", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockSystemDistros")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockSystemDistros", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-disable-wsl2-localhost-relay",
                    Label = "Disable WSL2 Localhost Port Relay to Windows Host",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents automatic port forwarding from WSL2 Linux network listeners to the Windows host network, stopping services running inside WSL2 from being reachable on Windows localhost without explicit configuration.",
                    Tags = ["wsl2", "localhost-relay", "port-forwarding", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 localhost port relay disabled; Linux services not automatically accessible on Windows localhost.",
                    ApplyOps = [RegOp.SetDword(NwKey, "DisableLocalhostRelay", 1)],
                    RemoveOps = [RegOp.DeleteValue(NwKey, "DisableLocalhostRelay")],
                    DetectOps = [RegOp.CheckDword(NwKey, "DisableLocalhostRelay", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-disable-wsl2-firewall-integration",
                    Label = "Disable WSL2 Windows Firewall Integration",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents WSL2 from applying Windows Firewall rules to the virtual machine network adapter, ensuring WSL2 network traffic is not filtered or monitored by Windows Defender Firewall rules created for WSL2.",
                    Tags = ["wsl2", "firewall", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 firewall integration disabled; Windows Firewall rules not applied to WSL2 virtual network.",
                    ApplyOps = [RegOp.SetDword(FwKey, "DisableFirewallIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwKey, "DisableFirewallIntegration")],
                    DetectOps = [RegOp.CheckDword(FwKey, "DisableFirewallIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-disable-wsl2-dns-tunneling",
                    Label = "Disable WSL2 DNS Tunneling Mode",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Disables WSL2 DNS tunneling (introduced in Windows 11 22H2) which routes DNS queries from Linux through a Windows-side DNS proxy, reverting to the standard VM network DNS and preventing potential data disclosure via DNS-over-proxy.",
                    Tags = ["wsl2", "dns-tunneling", "dns", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 DNS tunneling disabled; Linux DNS queries use VM network directly, not Windows DNS proxy.",
                    ApplyOps = [RegOp.SetDword(NwKey, "DisableDNSTunneling", 1)],
                    RemoveOps = [RegOp.DeleteValue(NwKey, "DisableDNSTunneling")],
                    DetectOps = [RegOp.CheckDword(NwKey, "DisableDNSTunneling", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-block-gui-apps",
                    Label = "Block WSL2 GUI (WSLg) Application Display on Windows Desktop",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents WSL2 Linux GUI applications from rendering their windows on the Windows host desktop via WSLg (Windows Subsystem for Linux GUI), limiting WSL2 usage to headless/terminal-only workloads.",
                    Tags = ["wsl2", "wslg", "gui-apps", "linux", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSLg blocked; Linux GUI apps cannot render windows on Windows desktop. Terminal access only.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWSLg", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWSLg")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWSLg", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-disable-wsl2-telemetry",
                    Label = "Disable WSL2 Telemetry Reporting to Microsoft",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description = "Prevents WSL2 from sending distro usage, feature adoption, crash, and diagnostics telemetry to Microsoft.",
                    Tags = ["wsl2", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 telemetry to Microsoft disabled; distro usage and crash data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-log-distro-lifecycle",
                    Label = "Log WSL2 Distro Registration and Start Events",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Enables Windows event log entries when a WSL2 Linux distribution is registered, unregistered, started, or terminated, providing audit visibility into Linux environment usage on corporate endpoints.",
                    Tags = ["wsl2", "event-log", "audit", "distro", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 distro lifecycle events logged; Linux environment registration and use visible for auditing.",
                    ApplyOps = [RegOp.SetDword(Key, "LogDistroLifecycle", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogDistroLifecycle")],
                    DetectOps = [RegOp.CheckDword(Key, "LogDistroLifecycle", 1)],
                },
                new TweakDef
                {
                    Id = "wsl2adv-require-admin-distro-install",
                    Label = "Require Administrator to Install WSL2 Distributions",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Prevents standard users from installing new Linux distributions into WSL2, ensuring distro installation is an administrative action subject to change management.",
                    Tags = ["wsl2", "distro-install", "admin", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL2 distro installation restricted to admins; standard users cannot add new Linux distributions.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDistroInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDistroInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDistroInstall", 1)],
                },
            ];
    }

    // ── WslDistroManagementPolicy ──
    private static class _WslDistroManagementPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Distros";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsldist-block-external-distro-sources",
                    Label = "WSL Distro: Block Installation of Unverified External Distros",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets AllowOnlyApprovedDistributions=1 in Lxss Distros policy. Restricts WSL distro installation to the set of distributions approved in this enterprise policy, blocking users from installing unverified third-party Linux distributions. "
                        + "Third-party WSL distros installed from .tar.gz archives or custom OCI images bypass the Microsoft Store signing process, are not subject to Windows Defender malware scanning during import, and may include custom kernel modules or services that establish network connections to external command-and-control infrastructure. Restricting to approved distributions ensures all WSL environments meet the organisation's security baseline.",
                    Tags = ["wsl", "distro", "installation", "security", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only approved WSL distros installable; blocks arbitrary Linux environments from unverified sources.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowOnlyApprovedDistributions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowOnlyApprovedDistributions")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowOnlyApprovedDistributions", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-disable-distro-export",
                    Label = "WSL Distro: Disable Distro Export to External Archive",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableDistributionExport=1 in Lxss Distros policy. Prevents users from running 'wsl --export' to create .tar.gz archives of installed WSL distributions. "
                        + "Exporting a WSL distro creates a portable archive of the entire Linux file system — including any data, credentials, keys, or sensitive files stored within the Linux home directory. This archive can then be transferred to an unmanaged device. Blocking distro export prevents the Linux container's data from being extracted and exfiltrated outside the managed device.",
                    Tags = ["wsl", "distro", "export", "data-exfiltration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks WSL distro export; Linux container filesystem cannot be archived and transferred off-device.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDistributionExport", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionExport")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDistributionExport", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-disable-distro-import",
                    Label = "WSL Distro: Disable Distro Import from External Archive",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableDistributionImport=1 in Lxss Distros policy. Prevents users from running 'wsl --import' to install a custom Linux distribution from a .tar.gz or OCI container archive. "
                        + "Importing a custom WSL distribution bypasses all Microsoft Store distribution vetting. An attacker who has compromised a development machine can create a custom Linux distro archive with embedded persistence mechanisms, additional network listeners, or credential theft tooling, then import it on other machines using only standard user 'wsl' CLI commands. Disabling import forces all WSL distro installations through the Store pipeline.",
                    Tags = ["wsl", "distro", "import", "lateral-movement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks WSL custom distro import; all distributions must come from the Microsoft Store.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDistributionImport", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionImport")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDistributionImport", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-set-max-distros-allowed-2",
                    Label = "WSL Distro: Limit Maximum Installed Distros to 2",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets MaxDistributionsAllowed=2 in Lxss Distros policy. Caps the number of WSL distributions that a user can have installed simultaneously to 2. "
                        + "Each installed WSL distribution adds to the attack surface: an additional Linux kernel, an additional network-accessible file system, and an additional set of Linux packages that may have known CVEs. Limiting users to 2 simultaneous distros (e.g., one primary development environment and one for testing) reduces this footprint while still supporting legitimate multi-environment development workflows.",
                    Tags = ["wsl", "distro", "limit", "attack-surface", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Max 2 WSL distros; limits Linux environment proliferation on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxDistributionsAllowed", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxDistributionsAllowed")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxDistributionsAllowed", 2)],
                },
                new TweakDef
                {
                    Id = "wsldist-require-admin-for-distro-removal",
                    Label = "WSL Distro: Require Administrative Approval to Unregister Distros",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets RequireAdminForDistributionRemoval=1 in Lxss Distros policy. Requires administrator privileges to unregister or remove a WSL distribution via 'wsl --unregister'. "
                        + "If a WSL distro becomes compromised, malware running within the Linux environment may attempt to cover its tracks by unregistering the distro after data exfiltration, destroying forensic evidence. Requiring admin elevation to remove a distro ensures that Linux environment removal is a deliberate IT/admin action, not something malware inside the WSL container can trigger via WSL CLI commands.",
                    Tags = ["wsl", "distro", "removal", "forensics", "admin"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Admin required to unregister WSL distros; prevents malware from destroying Linux container forensic evidence.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDistributionRemoval", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDistributionRemoval")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDistributionRemoval", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-disable-distro-version-downgrade",
                    Label = "WSL Distro: Block Downgrading Distros to WSL 1 Mode",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableDistributionVersionDowngrade=1 in Lxss Distros policy. Prevents users from converting installed WSL 2 distributions back to WSL 1 mode via 'wsl --set-version'. "
                        + "WSL 1 uses a translation layer (instead of a real Linux kernel) that is significantly more permissive in its Windows-Linux boundary enforcement. WSL 2 uses a Hyper-V lightweight VM with stronger isolation. Downgrading to WSL 1 weakens the isolation model and re-enables file system access patterns that WSL 2's VM architecture blocks, potentially creating a security regression on machines where WSL 2 was specifically required for its isolation guarantees.",
                    Tags = ["wsl", "wsl1", "wsl2", "isolation", "downgrade"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prevents WSL 2→WSL 1 downgrade; preserves Hyper-V VM isolation for all distros.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDistributionVersionDowngrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionVersionDowngrade")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDistributionVersionDowngrade", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-disable-distro-backup-creation",
                    Label = "WSL Distro: Disable Automatic Distro Backup Creation",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableDistributionBackup=1 in Lxss Distros policy. Prevents automatic backup snapshots of WSL distribution state from being written to the Windows user profile directory. "
                        + "WSL distribution backups are compressed archives of the Linux VHD that can be several gigabytes in size. On managed devices with roaming profiles or OneDrive-synced user profiles, these large backup files are undesirably synchronised to cloud storage, consuming bandwidth and cloud quota. Additionally, backups may contain sensitive Linux-resident credentials.",
                    Tags = ["wsl", "backup", "vhd", "profile", "cloud-sync"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "No automatic WSL distro backups; prevents large Linux VHD archives from consuming cloud sync quota.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDistributionBackup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionBackup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDistributionBackup", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-enable-distro-audit-logging",
                    Label = "WSL Distro: Enable Audit Logging for Distro Install and Remove Events",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnableDistributionAuditLogging=1 in Lxss Distros policy. Enables Security Event Log entries when WSL distributions are registered (installed), unregistered (removed), or converted between WSL 1/2 modes. "
                        + "Without distro lifecycle logging, there is no Security event log record of when Linux environments were created or deleted on a machine. If an attacker installs a WSL distro for lateral movement and then removes it to destroy evidence, the only forensic trace would be file system artefacts. Event log entries for distro lifecycle operations enable detection rules in SIEM systems.",
                    Tags = ["wsl", "audit", "logging", "siem", "lifecycle"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Security log entries for WSL distro install/remove; enables SIEM detection of Linux environment manipulation.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDistributionAuditLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDistributionAuditLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDistributionAuditLogging", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-disable-distro-rename",
                    Label = "WSL Distro: Disable User Renaming of Installed Distros",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableDistributionRename=1 in Lxss Distros policy. Prevents users from renaming installed WSL distributions via 'wsl --rename' or through the Windows registry. "
                        + "Distribution names are used by monitoring tools, DLP agents, and endpoint security software to identify WSL environments and apply appropriate policies. If a user renames a restricted distribution (e.g., a distro named 'blocked-distro') to an unrestricted name, policy enforcement based on distribution identity may be bypassed. Locking distribution names preserves the integrity of name-based policy enforcement.",
                    Tags = ["wsl", "distro", "rename", "policy-bypass", "identity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Distro names locked; prevents renaming to bypass name-based policy enforcement.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDistributionRename", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDistributionRename")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDistributionRename", 1)],
                },
                new TweakDef
                {
                    Id = "wsldist-disable-distro-updates-without-approval",
                    Label = "WSL Distro: Require Admin Approval for Distro Auto-Updates",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets RequireAdminForDistributionUpdates=1 in Lxss Distros policy. Requires administrator approval before a WSL distribution is allowed to automatically update its base image to a newer version from the Microsoft Store. "
                        + "While updating a WSL base image is generally desirable for security patch coverage, uncontrolled automatic updates can change the Linux environment's toolchain version, breaking developer builds that depend on specific library or compiler versions. Requiring admin approval gates distribution updates through change management, ensuring updates are tested before deployment.",
                    Tags = ["wsl", "distro", "updates", "change-management", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WSL distro updates require admin approval; prevents uncontrolled auto-updates breaking environment baselines.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDistributionUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDistributionUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDistributionUpdates", 1)],
                },
            ];
    }

    // ── WslFileSystemPolicy ──
    private static class _WslFileSystemPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\FileSystem";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wslfs-disable-windows-drive-automount",
                    Label = "WSL Filesystem: Disable Auto-Mount of Windows Drives in WSL",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableWindowsDriveAutomount=1 in Lxss FileSystem policy. Prevents WSL from automatically mounting Windows drive letters (C:, D:, etc.) under /mnt/ when a terminal session starts. "
                        + "Auto-mounting of Windows drives gives every process within the WSL environment — including any Linux malware — unrestricted read/write access to the full Windows user profile, including OneDrive, Documents, and AppData. With auto-mount disabled, a compromised Linux process cannot traverse from /mnt/c to Windows system paths without an explicit user mount command.",
                    Tags = ["wsl", "filesystem", "automount", "drvfs", "isolation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Windows drives not auto-mounted in WSL; Linux processes cannot access Windows file system without explicit mount.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWindowsDriveAutomount", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsDriveAutomount")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWindowsDriveAutomount", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-disable-network-drive-mount",
                    Label = "WSL Filesystem: Disable Mounting of Network UNC Paths in WSL",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableNetworkDriveMount=1 in Lxss FileSystem policy. Prevents WSL from mounting UNC paths (\\\\server\\share) or mapped network drives in the WSL file system. "
                        + "Allowing Linux processes to mount network shares expands the blast radius of WSL-based compromise to network-attached storage. Ransomware running in WSL with network drive access can encrypt network share contents with Linux-native encryption tools (openssl, gpg) that may not be detected by Windows-based endpoint protection monitoring network path writes.",
                    Tags = ["wsl", "filesystem", "network-drive", "unc", "ransomware"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "UNC/network paths not mountable in WSL; Linux processes cannot reach network shares.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableNetworkDriveMount", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableNetworkDriveMount")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableNetworkDriveMount", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-disable-wsl-host-mount",
                    Label = "WSL Filesystem: Disable WSL Host Physical Disk Mount (wsl --mount)",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableHostDiskMount=1 in Lxss FileSystem policy. Blocks the 'wsl --mount' command that allows a user to attach a physical disk or disk image directly into the WSL 2 VM, bypassing Windows file system filters. "
                        + "The 'wsl --mount' feature was designed for accessing Linux-native file systems (ext4, btrfs) on physical disks. However, it also allows attaching NTFS volumes directly into the WSL VM's kernel, bypassing Windows NTFS ACLs and file system minifilter drivers (including DLP, AV, and EDR file access monitors). Blocking this command eliminates a Windows security control bypass vector.",
                    Tags = ["wsl", "filesystem", "disk-mount", "host-mount", "filter-bypass"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Physical disk mount into WSL blocked; prevents Windows file system filter driver bypass via wsl --mount.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableHostDiskMount", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableHostDiskMount")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableHostDiskMount", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-disable-bind-mount",
                    Label = "WSL Filesystem: Disable Linux Bind Mounts Across Distro Boundaries",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableBindMount=1 in Lxss FileSystem policy. Prevents the use of Linux bind mounts within WSL that would map one distro's file system paths into another distro's namespace. "
                        + "In environments where multiple WSL distros coexist, allowing bind mounts between distros removes the isolation boundary between them. A compromised distro could bind-mount another distro's home directory or secret store, reading credentials that belong to a separate Linux identity/environment. Disabling cross-distro bind mounts preserves per-distro filesystem isolation.",
                    Tags = ["wsl", "filesystem", "bind-mount", "distro-isolation", "multi-distro"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Cross-distro bind mounts blocked; each WSL distro's filesystem remains isolated from sibling distros.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBindMount", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBindMount")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBindMount", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-enforce-drvfs-read-only",
                    Label = "WSL Filesystem: Enforce DrvFs Windows Drive Mounts as Read-Only",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnforceDrvFsReadOnly=1 in Lxss FileSystem policy. Forces all DrvFs mounts of Windows drives (e.g., /mnt/c) to be mounted read-only, preventing Linux processes from writing to the Windows file system through the DrvFs mount point. "
                        + "This is the strongest DrvFs hardening mode — Linux tools can read Windows files (e.g., build input files) but cannot write to Windows folders. Write-only WSL access to Windows paths is the most common vector for WSL-based file destruction: ransomware in WSL can encrypt /mnt/c/Users/... using Linux commands that bypass Windows AV real-time protection.",
                    Tags = ["wsl", "drvfs", "read-only", "ransomware", "write-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "DrvFs mounts are read-only; Linux processes cannot write to Windows drives. Cross-environment write workflows will break.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceDrvFsReadOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceDrvFsReadOnly")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceDrvFsReadOnly", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-disable-drvfs-metadata-mode",
                    Label = "WSL Filesystem: Disable DrvFs Metadata Mode (Linux Permission Emulation)",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableDrvFsMetadata=1 in Lxss FileSystem policy. Disables the DrvFs metadata extension that stores Linux file permissions, ownership (UID/GID), and extended attributes in NTFS extended attributes on Windows files. "
                        + "DrvFs metadata mode allows Linux processes to mark Windows files as setuid-root or setgid, creating files in Windows directories that, if subsequently executed by a Windows process, might behave unexpectedly due to permission metadata misinterpretation. While Windows ignores setuid bits, disabling metadata prevents Linux permission artefacts from being embedded in Windows file system objects.",
                    Tags = ["wsl", "drvfs", "metadata", "permissions", "setuid"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "No Linux permission metadata stored on Windows NTFS files; DrvFs presents all files with default umask ownership.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDrvFsMetadata", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDrvFsMetadata")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDrvFsMetadata", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-disable-plan9-mount-server",
                    Label = "WSL Filesystem: Disable Plan 9 File System Mount Server",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisablePlan9MountServer=1 in Lxss FileSystem policy. Disables the 9P (Plan 9 File System Protocol) server running inside the WSL 2 VM that provides the Windows←→Linux file sharing capability over a virtual Hyper-V vsock connection. "
                        + "The 9P file server in the WSL VM is the component that handles all cross-OS file access. If a vulnerability exists in the 9P server implementation, it could be exploited by a compromised Windows process to escalate into the WSL VM, or by a compromised Linux process to reach the Windows namespace. Disabling 9P eliminates this boundary component entirely.",
                    Tags = ["wsl", "plan9", "9p", "vsock", "attack-surface"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "9P file server disabled; cross-OS file sharing via /mnt/ will stop working entirely.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePlan9MountServer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePlan9MountServer")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePlan9MountServer", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-set-vhd-disk-quota-20gb",
                    Label = "WSL Filesystem: Set VHD Disk Quota Maximum to 20 GB",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets VhdDiskQuotaGB=20 in Lxss FileSystem policy. Limits the maximum size that a WSL virtual hard disk (ext4.vhdx) can grow to 20 GB per distribution, preventing runaway Linux processes from filling the host disk. "
                        + "WSL 2 VHD files start small and dynamically expand on demand up to a default cap of 1 TB. Linux processes performing large operations (building Docker images, running large ML training jobs, downloading large datasets) can inadvertently — or intentionally — fill the host disk by consuming the VHD expansion headroom. A 20 GB cap ensures WSL disk usage remains bounded.",
                    Tags = ["wsl", "vhd", "disk-quota", "disk-space", "ext4"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL VHD limited to 20 GB; Linux environments filling the host disk mitigation.",
                    ApplyOps = [RegOp.SetDword(Key, "VhdDiskQuotaGB", 20)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VhdDiskQuotaGB")],
                    DetectOps = [RegOp.CheckDword(Key, "VhdDiskQuotaGB", 20)],
                },
                new TweakDef
                {
                    Id = "wslfs-enable-filesystem-access-audit",
                    Label = "WSL Filesystem: Enable Cross-OS Filesystem Access Audit Logging",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnableFileSystemAccessAudit=1 in Lxss FileSystem policy. Enables logging of all file access events that cross the Windows-Linux filesystem boundary via DrvFs, writing entries to the Security event log under the Windows Subsystem for Linux provider. "
                        + "Without DrvFs access auditing, there is no Windows Security event log record of which Linux processes accessed which Windows files through /mnt/. This makes it impossible to determine the scope of a WSL-based file access incident post-breach. Audit logging of DrvFs access enables forensic reconstruction and real-time SIEM alerting on unexpected Linux access to sensitive Windows paths.",
                    Tags = ["wsl", "filesystem", "audit", "siem", "drvfs"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "DrvFs cross-OS access events logged to Security event log; enables forensic reconstruction of Linux file access incidents.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableFileSystemAccessAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableFileSystemAccessAudit")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableFileSystemAccessAudit", 1)],
                },
                new TweakDef
                {
                    Id = "wslfs-disable-tmpfs-overflow-to-host",
                    Label = "WSL Filesystem: Disable tmpfs Overflow Spilling to Windows Host Disk",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableTmpfsHostOverflow=1 in Lxss FileSystem policy. Prevents the WSL VM's in-memory tmpfs (/tmp, /run) from spilling overflow pages onto the Windows host disk when the VM's allocated RAM is exhausted. "
                        + "When WSL processes fill /tmp with large temporary files and the VM's RAM is exhausted, the kernel may begin swapping tmpfs pages to a backing swap store. Allowing this swap store to be on the host Windows disk effectively extends the VM's writable footprint onto the Windows NTFS volume in a way that bypasses the explicit DrvFs mount controls, since swap activity occurs at a lower abstraction layer.",
                    Tags = ["wsl", "tmpfs", "swap", "host-disk", "overflow"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "tmpfs overflow to host disk blocked; WSL VM memory pressure will OOM-kill processes rather than spill to host.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTmpfsHostOverflow", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTmpfsHostOverflow")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTmpfsHostOverflow", 1)],
                },
            ];
    }

    // ── WslKernelUpdatePolicy ──
    private static class _WslKernelUpdatePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Updates";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wslkupd-pin-kernel-update-channel-stable",
                    Label = "WSL Kernel Update: Pin WSL Kernel Updates to Stable Channel",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets KernelUpdateChannel=0 in Lxss Updates policy. Locks the WSL kernel update distribution channel to the 'Stable' channel (0 = stable release), preventing the system from automatically switching to preview or developer channel kernel builds via Windows Update. "
                        + "Preview and developer channel WSL kernel builds may contain experimental features or recently introduced security changes that have not undergone the full Windows Update quality validation cycle. In enterprise environments, pinning to the stable channel ensures that WSL kernel updates receive the same update quality bar as production Windows kernel updates, reducing the risk of a kernel regression breaking production developer workflows.",
                    Tags = ["wsl", "kernel", "update-channel", "stable", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL kernel updates restricted to Stable channel; no preview kernel builds deployed via Windows Update.",
                    ApplyOps = [RegOp.SetDword(Key, "KernelUpdateChannel", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "KernelUpdateChannel")],
                    DetectOps = [RegOp.CheckDword(Key, "KernelUpdateChannel", 0)],
                },
                new TweakDef
                {
                    Id = "wslkupd-enforce-kernel-signature-verification",
                    Label = "WSL Kernel Update: Require Digital Signature Verification on Kernel Updates",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnforceKernelSignatureVerification=1 in Lxss Updates policy. Requires that every WSL kernel update package delivered via Microsoft Update is verified against Microsoft's Authenticode certificate chain before being applied, blocking tampered or unsigned kernel update packages. "
                        + "Microsoft Update delivery of WSL kernel updates uses HTTPS transport, but a compromised Windows Update cache, WSUS proxy, or a threat actor with WSUS-level man-in-the-proxy access could substitute a malicious kernel package. Requiring explicit Authenticode signature verification ensures that even a package delivered through a compromised update pipeline is rejected if it is not signed by Microsoft's production signing keys.",
                    Tags = ["wsl", "kernel", "signature", "update", "supply-chain"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "WSL kernel packages require valid Microsoft Authenticode signature; tampered packages rejected before kernel update.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceKernelSignatureVerification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceKernelSignatureVerification")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceKernelSignatureVerification", 1)],
                },
                new TweakDef
                {
                    Id = "wslkupd-block-manual-kernel-downgrade",
                    Label = "WSL Kernel Update: Block Manual WSL Kernel Version Downgrade",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableKernelVersionDowngrade=1 in Lxss Updates policy. Prevents users from manually reverting the WSL Linux kernel to an older version via 'wsl --update --rollback' or by directly replacing the kernel package files. "
                        + "A threat actor who knows of an unpatched kernel vulnerability in an older WSL kernel version may attempt to roll the kernel back to the vulnerable version after the enterprise has applied a security patch. Blocking downgrade ensures that once a security-relevant WSL kernel update has been applied, it cannot be reversed without administrative action, enforcing a one-way patch ratchet.",
                    Tags = ["wsl", "kernel", "downgrade", "patch", "rollback"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL kernel rollback via --rollback blocked; applied kernel security patches cannot be reversed by standard users.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableKernelVersionDowngrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableKernelVersionDowngrade")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableKernelVersionDowngrade", 1)],
                },
                new TweakDef
                {
                    Id = "wslkupd-enable-urgent-kernel-security-updates",
                    Label = "WSL Kernel Update: Enable Urgent Security Updates via Windows Update",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets AllowUrgentKernelSecurityUpdates=1 in Lxss Updates policy. Allows the Windows Update service to automatically apply WSL kernel updates that are classified as 'Critical' or 'Security Update' severity without waiting for the standard Patch Tuesday deployment cycle. "
                        + "WSL kernel security vulnerabilities that are being actively exploited in the wild may be patched with emergency out-of-band updates. Without this policy, enterprises using slow deployment rings (e.g., broad ring with 14–30 day deferral) may leave systems vulnerable for weeks after Microsoft releases an emergency patch. Enabling urgent update delivery ensures critical WSL kernel fixes bypass deployment ring deferrals.",
                    Tags = ["wsl", "kernel", "security-update", "critical", "patch"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Critical WSL kernel security updates applied immediately; bypasses deployment ring deferrals for emergency patches.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUrgentKernelSecurityUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUrgentKernelSecurityUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUrgentKernelSecurityUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "wslkupd-set-kernel-update-defer-days-0",
                    Label = "WSL Kernel Update: Disable WSL Kernel Update Deferral (Apply Immediately)",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets KernelUpdateDeferralDays=0 in Lxss Updates policy. Sets the WSL kernel update deferral period to zero days, ensuring that WSL kernel updates are applied as soon as they are delivered and approved in the Windows Update service, with no additional deferral delay. "
                        + "Unlike the Windows NT kernel which is updated on the monthly Patch Tuesday cadence, WSL kernel updates are typically small, targeted patches. Deferring them unnecessarily extends the window during which a known-patched WSL kernel vulnerability remains present. Setting deferral to zero ensures the enterprise's WSL kernel vulnerability exposure window matches the Windows Update delivery latency, not an additional IT-imposed delay.",
                    Tags = ["wsl", "kernel", "deferral", "patch", "timely"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WSL kernel update deferral removed; updates applied as soon as Windows Update delivers them.",
                    ApplyOps = [RegOp.SetDword(Key, "KernelUpdateDeferralDays", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "KernelUpdateDeferralDays")],
                    DetectOps = [RegOp.CheckDword(Key, "KernelUpdateDeferralDays", 0)],
                },
                new TweakDef
                {
                    Id = "wslkupd-enable-kernel-update-audit-log",
                    Label = "WSL Kernel Update: Enable Security Event Log Entry on Kernel Update or Rollback",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnableKernelUpdateAuditLog=1 in Lxss Updates policy. Causes a Security event log entry to be written whenever the WSL Linux kernel is updated to a new version or rolled back to a previous version, recording the previous and new kernel version strings. "
                        + "Without audit logging of WSL kernel version changes, there is no Security event record of when WSL kernel updates were applied (or not applied) on a device. This makes it impossible to determine whether a managed device was running a vulnerable WSL kernel during a specific incident timeframe. Kernel version change events enable compliance managers to demonstrate patch deployment timelines and detect unexplained rollbacks.",
                    Tags = ["wsl", "kernel", "audit", "logging", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Security log entry written on WSL kernel version change; kernel update history auditable in SIEM.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableKernelUpdateAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelUpdateAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableKernelUpdateAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "wslkupd-block-user-manual-kernel-update",
                    Label = "WSL Kernel Update: Block Manual 'wsl --update' by Standard Users",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableUserManualKernelUpdate=1 in Lxss Updates policy. Removes the ability of standard (non-administrator) users to manually trigger WSL kernel updates via the 'wsl --update' command, restricting kernel update initiation to Windows Update and IT administrator action only. "
                        + "While 'wsl --update' legitimately downloads the latest stable kernel, standard users initiating manual updates bypass the enterprise's staged Windows Update ring deployment schedule. If a specific kernel version is deferred in slow deployment rings due to a regression, users running 'wsl --update' would receive the update immediately, bypassing change management controls. Restricting update initiation to Windows Update enforces the enterprise deployment schedule.",
                    Tags = ["wsl", "kernel", "manual-update", "deployment-ring", "change-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "wsl --update blocked for standard users; WSL kernel updates controlled by Windows Update and admin action only.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUserManualKernelUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUserManualKernelUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUserManualKernelUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "wslkupd-enable-kernel-integrity-verification-on-boot",
                    Label = "WSL Kernel Update: Verify WSL Kernel Image Integrity at VM Start",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets VerifyKernelIntegrityOnBoot=1 in Lxss Updates policy. Enables hash-based integrity verification of the WSL Linux kernel image file (vmlinux) against its stored signature each time a WSL session starts a new Hyper-V VM instance. "
                        + "Without startup integrity verification, a threat actor with write access to the WSL kernel image path on the Windows file system can replace the kernel with a modified version that installs kernel-level hooks for all subsequent WSL sessions. Verifying the kernel image hash at each VM start ensures that any tampering with the stored kernel image is detected before the compromised kernel is loaded into the Hyper-V VM.",
                    Tags = ["wsl", "kernel", "integrity", "hash", "vm-start"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WSL kernel image hash verified at each VM start; tampered kernel image detected before being loaded.",
                    ApplyOps = [RegOp.SetDword(Key, "VerifyKernelIntegrityOnBoot", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VerifyKernelIntegrityOnBoot")],
                    DetectOps = [RegOp.CheckDword(Key, "VerifyKernelIntegrityOnBoot", 1)],
                },
                new TweakDef
                {
                    Id = "wslkupd-block-preview-kernel-builds",
                    Label = "WSL Kernel Update: Block Preview and Insider WSL Kernel Preview Builds",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets BlockPreviewKernelBuilds=1 in Lxss Updates policy. Prevents Windows Insider Preview and Windows Update Preview channels from delivering pre-release WSL Linux kernel builds to managed devices that are enrolled in preview rings for Windows OS preview builds. "
                        + "Managed enterprise devices may be enrolled in Windows Insider rings for OS preview testing. However, preview WSL kernel builds may have known vulnerabilities, experimental security mitigations disabled, and debugging interfaces enabled that are inappropriate for production use. Blocking preview kernel builds ensures enterprise devices only receive production-quality WSL kernels regardless of their Windows Insider ring membership.",
                    Tags = ["wsl", "kernel", "preview", "insider", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Preview WSL kernel builds blocked even for Insider-enrolled devices; production kernels only.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPreviewKernelBuilds", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPreviewKernelBuilds")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPreviewKernelBuilds", 1)],
                },
                new TweakDef
                {
                    Id = "wslkupd-require-restart-to-apply-kernel-update",
                    Label = "WSL Kernel Update: Require Full WSL Session Termination to Activate Kernel Updates",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets RequireSessionRestartForKernelUpdate=1 in Lxss Updates policy. Requires that all running WSL distro sessions are fully terminated (wsl --shutdown) and a new Hyper-V VM is started before a delivered WSL kernel update becomes active. Prevents in-place kernel hot-patching that may skip the integrity verification startup checks. "
                        + "Hot-patching-style kernel activation (patching a running VM's kernel in-memory) bypasses the startup integrity verification performed when a new VM is instantiated. By requiring a full session shutdown and VM restart after each kernel update, this policy ensures that the newly applied kernel image goes through the full chain-of-trust verification (signature check → AppArmor load → seccomp policy activation) before any distro sessions run on it.",
                    Tags = ["wsl", "kernel", "restart", "hot-patch", "chain-of-trust"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WSL kernel updates require session restart to activate; hot-patch bypasses chain-of-trust checks prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSessionRestartForKernelUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSessionRestartForKernelUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSessionRestartForKernelUpdate", 1)],
                },
            ];
    }

    // ── WslMemoryLimitsPolicy ──
    private static class _WslMemoryLimitsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Memory";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wslmemlim-set-max-wsl-vm-memory-4gb",
                    Label = "WSL Memory: Cap WSL VM Maximum Memory Allocation to 4 GB",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets MaxVmMemoryMB=4096 in Lxss Memory policy. Limits the maximum amount of host RAM the WSL 2 Hyper-V virtual machine can allocate to 4 GB, preventing WSL workloads from consuming the majority of the host system's physical memory. "
                        + "By default, WSL 2 can claim up to 50% of host RAM (up to 8 GB total). On workstations with 16–32 GB RAM, a developer building a large project in WSL (e.g., a Linux kernel build, a large Rust project, or a Docker image layer operation) can cause the Windows desktop to become unresponsive due to memory pressure. A 4 GB cap ensures the host OS retains sufficient memory for responsive interactive use.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets MaxVmProcessors=4 in Lxss Memory policy. Caps the number of virtual CPU cores visible to the WSL 2 VM to 4, limiting the maximum parallelism available to Linux workloads to prevent CPU starvation of host Windows processes. "
                        + "WSL 2 inherits all host CPU cores by default. On a 12-core/24-thread development workstation, a Linux build job running make -j24 can saturate all CPU cores, causing Windows UI, background services, and other processes to become CPU-starved. Capping WSL at 4 cores ensures the OS retains burst capacity for interactive workloads, antivirus scans, and system services.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets VmSwapFileSizeMB=2048 in Lxss Memory policy. Sets the WSL 2 VM's swap file (used when VM RAM is exhausted) to a fixed 2 GB size, preventing the swap file from growing unboundedly on the Windows host disk. "
                        + "The WSL 2 VM swap file is created dynamically on the Windows NTFS volume. Without a size cap, memory-intensive Linux workloads can cause the swap file to expand to match the available NTFS free space — effectively allowing WSL to consume the entire host disk as virtual memory. A 2 GB fixed swap cap provides a safety margin for legitimate memory overcommit while preventing disk exhaustion.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnableGradualMemoryReclaim=1 in Lxss Memory policy. Instructs the WSL 2 hypervisor to periodically reclaim unused virtual machine memory pages back to the Windows host pool using the gradual reclaim algorithm, rather than holding memory until the VM terminates. "
                        + "Without memory reclaim enabled, WSL 2 acquires RAM as needed and holds it indefinitely, even after Linux processes that consumed the memory have exited. This means that after a large Linux build job completes, the RAM it consumed is not returned to the Windows pool, causing the host to appear low on memory. Gradual reclaim enables the WSL Hyper-V balloon driver to return idle pages to Windows.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableVmSwapFile=1 in Lxss Memory policy. Prevents the WSL 2 virtual machine from creating a swap file on the Windows host disk. With no swap, the Linux VM operates in a pure in-memory mode and Linux processes that exhaust available VM memory will be OOM-killed rather than swap-paging. "
                        + "The WSL swap file is a write-capable artifact on the Windows NTFS volume. Data written to Linux virtual memory (including sensitive in-memory data structures like encryption keys that are paged out under memory pressure) is persisted in a cleartext swap file on Windows disk. Disabling swap eliminates this data-at-rest exposure channel, at the cost of Linux OOM-kill risk for memory-intensive workloads.",
                    Tags = ["wsl", "swap", "memory", "data-at-rest", "oom"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "No WSL swap file; sensitive paged-out VM memory not persisted to Windows disk; OOM-kill for memory-intensive Linux workloads.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableVmSwapFile", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableVmSwapFile")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableVmSwapFile", 1)],
                },
                new TweakDef
                {
                    Id = "wslmemlim-enable-vm-page-reporting",
                    Label = "WSL Memory: Enable VM Page Reporting for Efficient Host Memory Return",
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnableVmPageReporting=1 in Lxss Memory policy. Enables the Hyper-V VM page reporting guest protocol within the WSL 2 VM, allowing the Linux guest to proactively report free memory pages to the host hypervisor for immediate host memory pool reuse. "
                        + "Page reporting is a more aggressive memory return mechanism than the balloon driver; while balloon reclaim waits for host memory pressure, page reporting proactively marks guest-free pages as available to the host. This results in faster and more complete memory return after burst WSL workloads, improving host memory availability for concurrent Windows applications.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets VmMemoryReclaimIdleThresholdMin=5 in Lxss Memory policy. Sets the idle timeout after which the WSL 2 Hyper-V host will begin reclaiming memory pages from an idle WSL VM to 5 minutes. "
                        + "The default idle threshold before WSL memory reclaim begins is typically 10–15 minutes. In enterprise environments where developers run WSL sessions intermittently (editing code in Windows, compiling in WSL, back to editing), the 5-minute threshold ensures that memory allocated during compile jobs is returned to the host within 5 minutes of the WSL VM becoming idle, rather than holding the memory for the duration of the edit cycle.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableKernelSamepageMerging=1 in Lxss Memory policy. Disables the Linux kernel's KSM (Kernel Same-page Merging) memory deduplication feature in the WSL 2 VM, which periodically scans VM memory for identical pages and merges them into copy-on-write shared pages. "
                        + "KSM is a known side-channel: the merge/de-merge timing of identical pages can be used to detect whether a particular secret value (e.g., a cryptographic key) exists in another process's memory. Research has demonstrated KSM-based cross-process memory probing exploits. Disabling KSM removes this timing side-channel within the WSL VM's memory subsystem.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets EnableMemoryPressureNotifications=1 in Lxss Memory policy. Enables the WSL 2 hypervisor to send memory pressure notifications into the Linux guest when the Windows host is experiencing memory pressure, allowing the Linux kernel to invoke its own memory pressure handlers (cgroup high/low events, transparent huge page compaction) proactively. "
                        + "Without pressure notifications, the Linux VM has no visibility into host memory pressure and will continue normal memory allocation, worsening host memory pressure. With notifications enabled, the guest can perform early memory reclaim before the hypervisor is forced to balloon-reclaim pages, resulting in more cooperative memory sharing between the WSL VM and Windows host processes.",
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
                    Category = "Virtualization — Wsa Network Isolation",
                    Description =
                        "Sets DisableLargePageAllocation=1 in Lxss Memory policy. Disables transparent huge page (THP) allocation within the WSL 2 VM's Linux kernel, preventing the VM from allocating 2 MB memory pages instead of 4 KB pages. "
                        + "Transparent huge pages improve Linux application throughput for memory-intensive workloads but make host memory reclaim significantly less efficient. A 2 MB THP page cannot be reclaimed until all 512 sub-pages are free simultaneously, causing huge pages to become 'locked' memory that resists balloon and page-report reclaim. Disabling THP makes WSL VM memory more granularly reclaimable, improving host memory return latency.",
                    Tags = ["wsl", "memory", "huge-pages", "thp", "reclaim"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "THP disabled in WSL VM; slightly lower Linux throughput for memory-intensive workloads but significantly faster host memory reclaim.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLargePageAllocation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLargePageAllocation")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLargePageAllocation", 1)],
                },
            ];
    }

    // ── WslSecurityHardeningPolicy ──
    private static class _WslSecurityHardeningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Security";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wslsechrd-disable-linux-privileged-container",
                    Label = "WSL Security: Block Privileged Linux Containers (--privileged)",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets DisablePrivilegedContainerMode=1 in Lxss Security policy. Prevents users from launching Linux containers in privileged mode within WSL, which would grant the container full access to the WSL kernel's device tree and capabilities. "
                        + "Privileged Linux containers bypass cgroup and namespace isolation — a privileged container escape is effectively a WSL hypervisor boundary bypass. Container image registries routinely contain malicious images that exploit the privileged mode to escape to the host. Blocking privileged container mode ensures all Docker and podman containers within WSL remain namespace-isolated.",
                    Tags = ["wsl", "container", "privileged", "isolation", "escape"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Privileged Linux containers blocked in WSL; Docker --privileged flag will be denied.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePrivilegedContainerMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivilegedContainerMode")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePrivilegedContainerMode", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-require-secure-boot-for-wsl",
                    Label = "WSL Security: Require Secure Boot (UEFI) Validation Before WSL Launch",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets RequireSecureBootForWsl=1 in Lxss Security policy. Configures WSL to verify that the host system has Secure Boot enabled and that the WSL system distro image is digitally signed before allowing any WSL distribution to launch. "
                        + "A threat actor with physical access or bootkit privileges can replace the WSL system distro image (kernel/initramfs) with a malicious version that intercepts WSL sessions. Requiring Secure Boot validation means that only Microsoft-signed WSL kernel images will be accepted — any tampered or unsigned replacement will be rejected at launch, preventing persistent WSL-level rootkit installation.",
                    Tags = ["wsl", "secure-boot", "kernel", "integrity", "bootkit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WSL requires Secure Boot; unsigned or tampered WSL kernel images rejected at launch.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSecureBootForWsl", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureBootForWsl")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSecureBootForWsl", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-enable-apparmor-enforcement",
                    Label = "WSL Security: Enable AppArmor Mandatory Access Control Enforcement",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets RequireAppArmorEnforcement=1 in Lxss Security policy. Requires that AppArmor (the Linux Mandatory Access Control framework) is active and in enforcing mode within WSL distributions before those distros are permitted to run Linux processes. "
                        + "AppArmor-enforcing mode means that every Linux process is subject to a per-executable MAC policy that limits the files, capabilities, and network resources it can access. Without AppArmor enforcement, a compromised Linux process within WSL can access any file the Linux user has permission to reach (including all DrvFs-mounted Windows files). AppArmor confines individual processes to their expected access patterns.",
                    Tags = ["wsl", "apparmor", "mac", "mandatory-access-control", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "AppArmor MAC enforcement required; distros without AppArmor active will be blocked from launching processes.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAppArmorEnforcement", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAppArmorEnforcement")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAppArmorEnforcement", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-disable-wsl-sudo-escalation",
                    Label = "WSL Security: Block sudo Privilege Escalation in WSL Distributions",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets DisableSudoEscalation=1 in Lxss Security policy. Prevents the 'sudo' and 'su' commands from granting root privileges within WSL distributions launched in standard user sessions, enforcing that all WSL processes run under the Linux user identity only. "
                        + "sudo root access within WSL gives a Linux process full root capabilities within the WSL VM, including the ability to install kernel modules, bind to privileged ports, and reconfigure network namespaces. While the WSL VM boundary limits the blast radius, root access within WSL provides a much larger attack surface for container escape and VM privilege escalation research exploitation.",
                    Tags = ["wsl", "sudo", "root", "privilege-escalation", "least-privilege"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "sudo/su blocked in WSL; all Linux processes run as the mapped Windows user identity only.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSudoEscalation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSudoEscalation")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSudoEscalation", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-enable-seccomp-enforcement",
                    Label = "WSL Security: Enable Seccomp System Call Filtering Enforcement",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets RequireSeccompEnforcement=1 in Lxss Security policy. Requires the WSL Linux kernel to apply Seccomp-BPF (system call filtration) policies to all user-space processes, blocking access to dangerous system calls that are not needed for standard application workloads. "
                        + "Many Linux kernel privilege escalation vulnerabilities are triggered via obscure or rarely-used system calls (ptrace, perf_event_open, io_uring). Seccomp filtering blocks these system calls unless explicitly allowed by the process's policy, preventing exploitation of kernel vulnerabilities that require those call paths. This is particularly important because the WSL Linux kernel shares the root namespace with all distros.",
                    Tags = ["wsl", "seccomp", "syscall", "kernel", "exploit-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Seccomp syscall filtering enforced; dangerous kernel interfaces blocked from user-space, reducing kernel exploit surface.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSeccompEnforcement", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSeccompEnforcement")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSeccompEnforcement", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-disable-raw-socket-creation",
                    Label = "WSL Security: Block Raw Socket Creation by Non-Root Linux Processes",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets DisableRawSocketCreation=1 in Lxss Security policy. Prevents non-root Linux processes within WSL from creating AF_PACKET (raw) network sockets, which would allow them to perform low-level network packet capture and injection without Windows-side network monitoring visibility. "
                        + "Raw sockets enable Linux processes to capture all network traffic visible to the WSL VM's network namespace. In mirrored networking mode, this includes all traffic from the Windows host. A malicious tool running in WSL with raw socket access can perform ARP spoofing, DNS poisoning, and credential harvest from unencrypted protocol traffic, bypassing Windows network monitoring solutions.",
                    Tags = ["wsl", "raw-socket", "packet-capture", "network", "monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Raw sockets blocked for non-root Linux processes; WSL cannot be used for network packet capture without root.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRawSocketCreation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRawSocketCreation")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRawSocketCreation", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-disable-ptrace-between-distros",
                    Label = "WSL Security: Block ptrace Cross-Distro Process Attachment",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets DisableCrossDistrictPtrace=1 in Lxss Security policy. Prevents Linux processes in one WSL distribution from using ptrace() to attach to and debug processes running in another WSL distribution sharing the same Hyper-V VM. "
                        + "When multiple WSL distros run in the same Hyper-V partition (the typical configuration), they share a Linux kernel and a process namespace at the VM level. Without a ptrace policy, a process in distro A can attach to a process in distro B and read its memory, modify its execution, or extract credentials it holds. Restricting cross-distro ptrace enforces process isolation between co-located Linux environments.",
                    Tags = ["wsl", "ptrace", "debugging", "multi-distro", "isolation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ptrace blocked across WSL distros; one distro cannot debug or read memory of processes in another distro.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCrossDistrictPtrace", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossDistrictPtrace")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCrossDistrictPtrace", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-enable-user-namespace-restrictions",
                    Label = "WSL Security: Restrict Unprivileged Linux User Namespace Creation",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets RestrictUnprivilegedUserNamespaces=1 in Lxss Security policy. Limits the ability of unprivileged Linux user-space processes to create new user namespaces within the WSL VM, which are frequently exploited as container escape stepping stones. "
                        + "User namespaces are the Linux kernel primitive that enables unprivileged container tools (rootless Docker, rootless podman). However, user namespaces have also been the root cause or enabling boundary for a significant fraction of Linux kernel privilege escalation CVEs (CVE-2023-4911, CVE-2022-0847 'Dirty Pipe', CVE-2022-25375). Restricting their creation prevents their use as an escalation primitive while still allowing root-managed container workflows.",
                    Tags = ["wsl", "user-namespace", "container", "cve", "kernel-exploit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Unprivileged user namespace creation restricted; rootless container tools may not function; reduces kernel namespace CVE exposure.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictUnprivilegedUserNamespaces", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictUnprivilegedUserNamespaces")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictUnprivilegedUserNamespaces", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-enable-defender-scan-on-wsl-exec",
                    Label = "WSL Security: Enable Microsoft Defender Scanning on WSL Executable Launch",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets EnableDefenderScanOnWslExecution=1 in Lxss Security policy. Enables Microsoft Defender for Endpoint to scan Linux ELF executables and scripts when they are launched within the WSL environment, before process execution begins. "
                        + "By default, Defender's real-time file system protection monitors the Windows NTFS volume but may not scan Linux ELF binaries within the ext4 VHD. With WSL execution scanning enabled, Defender analyses Linux binaries using Linux threat intelligence signatures, detecting known Linux malware, coin miners, and reverse shells that reside within the WSL file system. This closes the gap between Windows AV coverage and Linux-resident threats.",
                    Tags = ["wsl", "defender", "malware", "scanning", "elf"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Defender scans Linux ELF binaries at WSL launch time; Linux malware in the WSL VHD detected before execution.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDefenderScanOnWslExecution", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDefenderScanOnWslExecution")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDefenderScanOnWslExecution", 1)],
                },
                new TweakDef
                {
                    Id = "wslsechrd-disable-wsl-kernel-module-load",
                    Label = "WSL Security: Block Loading of Unsigned Linux Kernel Modules in WSL",
                    Category = "Virtualization — Wsl Security Hardening",
                    Description =
                        "Sets DisableUnsignedKernelModuleLoad=1 in Lxss Security policy. Prevents the WSL Linux kernel from loading unsigned or third-party kernel modules (LKMs) that are not part of the Microsoft-signed WSL kernel image. "
                        + "Linux kernel modules run with ring-0 (kernel mode) privileges and have unrestricted access to all memory, devices, and kernel data structures within the VM. A malicious loadable kernel module loaded in WSL can install kernel-level hooks, intercept all WSL system calls, and exfiltrate data from other processes at the kernel level. Blocking unsigned module loading enforces that only the Microsoft-vetted WSL kernel components can extend the kernel's attack surface.",
                    Tags = ["wsl", "kernel-module", "lkm", "ring0", "unsigned"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Unsigned Linux kernel modules blocked in WSL; custom LKMs and third-party drivers cannot be loaded into the WSL kernel.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUnsignedKernelModuleLoad", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUnsignedKernelModuleLoad")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUnsignedKernelModuleLoad", 1)],
                },
            ];
    }

    // ── WindowsSubsystemLinuxPolicy ──
    private static class _WindowsSubsystemLinuxPolicy
    {
        private const string LxssKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wslpol-disable-wsl",
                    Label = "Disable Windows Subsystem for Linux",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets Enabled=0 in the Lxss policy key to disable WSL entirely. Prevents installation or launch of any Linux distributions. Use in high-security or compliance environments where Linux workloads on Windows are prohibited.",
                    Tags = ["wsl", "linux", "subsystem", "policy", "enterprise"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Blocks all WSL distributions from running; developers lose Linux workflow access.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-block-kernel-debugging",
                    Label = "Block WSL Kernel Debugging",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowKernelDebugging=0 in the WSL policy key. Prevents users from attaching kernel debuggers to the WSL2 virtual machine, reducing the attack surface from kernel exploits originating in Linux workloads.",
                    Tags = ["wsl", "kernel", "debug", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks kernel debugger attachment to WSL2 VM; no impact on normal WSL usage.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowKernelDebugging", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowKernelDebugging")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowKernelDebugging", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-block-dev-mode-install",
                    Label = "Block WSL Developer Mode Installs",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowDeveloperModeInstall=0 in the WSL policy key. Blocks installation of Linux distributions from developer mode sources outside the Microsoft Store, enforcing controlled distribution channels.",
                    Tags = ["wsl", "developer", "install", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks sideloaded WSL distributions; Store-sourced distros unaffected.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowDeveloperModeInstall", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowDeveloperModeInstall")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowDeveloperModeInstall", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-block-custom-kernel",
                    Label = "Block WSL Custom Kernel Configuration",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowCustomKernelConfiguration=0 in the WSL policy key. Prevents users from replacing the WSL2 kernel with custom builds via .wslconfig, ensuring only the Microsoft-signed kernel runs.",
                    Tags = ["wsl", "kernel", "custom", "policy", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks custom WSL kernel; standard WSL operations unaffected.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowCustomKernelConfiguration", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowCustomKernelConfiguration")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowCustomKernelConfiguration", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-disable-disk-mount",
                    Label = "Disable WSL Disk Image Mounting",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowMount=0 in the WSL policy key. Blocks mounting of physical disks or VHD image files inside WSL2, preventing lateral movement via raw disk access from within a Linux distribution.",
                    Tags = ["wsl", "mount", "disk", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents 'wsl --mount' command; limits developers who mount external drives in WSL.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowMount", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowMount")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowMount", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-block-wsl-networking",
                    Label = "Block WSL Outbound Networking",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowNetworking=0 in the WSL policy key. Disables network access for all WSL2 distributions, isolating Linux workloads from the corporate network and the internet.",
                    Tags = ["wsl", "network", "isolation", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Completely isolates WSL2 network stack; breaks package managers (apt, pip) inside Linux.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowNetworking", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowNetworking")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowNetworking", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-disable-systemd",
                    Label = "Disable systemd in WSL",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowSystemd=0 in the WSL policy key. Prevents WSL2 distributions from using systemd as PID 1, blocking system service management that could be exploited for persistence mechanisms.",
                    Tags = ["wsl", "systemd", "service", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Blocks systemd-managed services in WSL2; affects distros relying on systemd service units.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowSystemd", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowSystemd")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowSystemd", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-block-gpu-compute",
                    Label = "Block WSL GPU Compute Access",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowGPUCompute=0 in the WSL policy key. Disables DirectML and GPU acceleration inside WSL2, preventing Linux processes from accessing GPU compute resources via the host WDDM driver.",
                    Tags = ["wsl", "gpu", "compute", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks GPU/DirectML from WSL2; CUDA and ML workloads inside WSL affected.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowGPUCompute", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowGPUCompute")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowGPUCompute", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-disable-dns-tunneling",
                    Label = "Disable WSL DNS Tunneling",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowDNSTunneling=0 in the WSL policy key. Prevents WSL2 from using DNS-over-HTTPS tunneling mode, ensuring DNS queries from Linux distributions go through standard Windows resolver configuration.",
                    Tags = ["wsl", "dns", "tunnel", "policy", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables DoH DNS tunneling in WSL2; DNS resolution still works via standard Windows DNS.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowDNSTunneling", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowDNSTunneling")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowDNSTunneling", 0)],
                },
                new TweakDef
                {
                    Id = "wslpol-block-vtpm",
                    Label = "Block WSL Virtual TPM",
                    Category = "Virtualization — Windows Subsystem Linux",
                    Description =
                        "Sets AllowVTPM=0 in the WSL policy key. Blocks Linux distributions from accessing a virtual TPM device inside WSL2. Reduces risk of TPM-based key material in the Linux trust boundary.",
                    Tags = ["wsl", "tpm", "virtual", "policy", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks vTPM access from WSL2; affects WSL distributions requiring TPM attestation.",
                    ApplyOps = [RegOp.SetDword(LxssKey, "AllowVTPM", 0)],
                    RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowVTPM")],
                    DetectOps = [RegOp.CheckDword(LxssKey, "AllowVTPM", 0)],
                },
            ];
    }
}

internal static class VsCode
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vscode-disable-telemetry-reporting",
            Label = "Disable VS Code Telemetry (User Policy)",
            Category = "Developer — Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables VS Code telemetry via user-level policy. Default: Enabled. Recommended: Disabled.",
            Tags = ["vscode", "telemetry", "privacy", "user-policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "EnableTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-update-check",
            Label = "Disable VS Code Update Check (User Policy)",
            Category = "Developer — Virtualization",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables VS Code auto-update checking via user-level policy. Default: Enabled. Recommended: Disabled for stable environments.",
            Tags = ["vscode", "update", "auto-update", "user-policy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode", "disabled")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\VisualStudioCode", "UpdateMode", "disabled")],
        },
        new TweakDef
        {
            Id = "vscode-vsc-disable-update-notif",
            Label = "Disable VS Code Update Notifications (Machine Policy)",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables VS Code update notifications and release notes via machine policy. Default: Enabled. Recommended: Disabled for stable environments.",
            Tags = ["vscode", "update", "notifications", "machine-policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none"),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.showReleaseNotes", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.showReleaseNotes"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "update.mode", "none")],
        },
        new TweakDef
        {
            Id = "vscode-vsc-set-gpu-accel",
            Label = "Set VS Code GPU Acceleration (Machine Policy)",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables GPU acceleration for VS Code via machine-level policy. Improves rendering performance. Default: Auto. Recommended: On.",
            Tags = ["vscode", "gpu", "acceleration", "performance", "machine-policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration", "on")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "gpu.acceleration", "on")],
        },
        new TweakDef
        {
            Id = "vscode-disable-natural-language-search",
            Label = "Disable VS Code Natural Language Search",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the natural language search feature in VS Code settings (prevents Bing queries). Default: enabled.",
            Tags = ["vscode", "search", "bing", "natural-language"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.settings.enableNaturalLanguageSearch", 0),
            ],
        },
        new TweakDef
        {
            Id = "vscode-disable-extension-recommendations",
            Label = "Disable VS Code Extension Recommendations",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extension recommendations in VS Code via machine policy. Default: enabled.",
            Tags = ["vscode", "extensions", "recommendations", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.showRecommendationsOnlyOnDemand", 1)],
        },
        new TweakDef
        {
            Id = "vscode-disable-crash-reporter",
            Label = "Disable VS Code Crash Reporter",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code crash reporter via machine policy. Stops sending crash dumps to Microsoft. Default: enabled.",
            Tags = ["vscode", "crash", "reporter", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "enable-crash-reporter", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-experiments",
            Label = "Disable VS Code Experiments",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables A/B experiments in VS Code that can change features. Default: enabled.",
            Tags = ["vscode", "experiments", "ab-testing", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.enableExperiments", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-remote-telemetry",
            Label = "Disable VS Code Remote Extension Telemetry",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables telemetry for VS Code Remote extensions (SSH, WSL, Dev Containers). Default: enabled.",
            Tags = ["vscode", "remote", "telemetry", "ssh", "wsl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "remote.telemetryLevel", "off")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "remote.telemetryLevel")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "remote.telemetryLevel", "off")],
        },
        new TweakDef
        {
            Id = "vscode-disable-edit-sessions",
            Label = "Disable VS Code Edit Sessions Cloud",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code Edit Sessions that sync uncommitted changes to the cloud. Keeps changes local. Default: enabled.",
            Tags = ["vscode", "edit-sessions", "cloud", "sync", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.editSessions.autoResume", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.editSessions.autoResume")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.editSessions.autoResume", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-online-services",
            Label = "Disable VS Code Online Services",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables VS Code online service features including settings sync, marketplace, etc. Default: enabled.",
            Tags = ["vscode", "online", "services", "offline", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoCheckUpdates", 0)],
        },
        // ── Restored stubs with real registry operations ──────────────────

        new TweakDef
        {
            Id = "vscode-disable-extension-gallery",
            Label = "Disable Extension Marketplace",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the VS Code extension marketplace via Group Policy, preventing extension installs.",
            Tags = ["vscode", "extensions", "marketplace", "policy", "offline"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.serviceUrl", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.serviceUrl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.serviceUrl", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-gpu-acceleration",
            Label = "Disable VS Code GPU Acceleration",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables GPU/hardware acceleration in VS Code via policy. Useful for remote desktop or VM environments.",
            Tags = ["vscode", "gpu", "acceleration", "rdp", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "disable-hardware-acceleration", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "disable-hardware-acceleration")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "disable-hardware-acceleration", 1)],
        },
        new TweakDef
        {
            Id = "vscode-disable-telemetry-policy",
            Label = "Disable VS Code Telemetry (Machine)",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables all VS Code telemetry and diagnostics via machine-level Group Policy.",
            Tags = ["vscode", "telemetry", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableCrashReporter", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableTelemetry", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableCrashReporter"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.enableTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-ext-update",
            Label = "Disable VS Code Extension Auto-Update",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from automatically updating extensions via policy.",
            Tags = ["vscode", "extensions", "auto-update", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.autoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-settings-sync",
            Label = "Disable VS Code Settings Sync",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Settings Sync feature in VS Code via Group Policy.",
            Tags = ["vscode", "settings-sync", "cloud", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-startup-editor",
            Label = "Disable VS Code Startup Welcome Tab",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from showing the Welcome tab on startup via policy.",
            Tags = ["vscode", "startup", "welcome", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.startupEditor", "none")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.startupEditor")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "workbench.startupEditor", "none")],
        },
        new TweakDef
        {
            Id = "vscode-disable-vscode-telemetry",
            Label = "Disable VS Code Telemetry (All)",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets VS Code telemetry level to 'off' via machine policy, disabling all data collection.",
            Tags = ["vscode", "telemetry", "off", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel.value", "off")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel.value")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "telemetry.telemetryLevel.value", "off")],
        },
        new TweakDef
        {
            Id = "vscode-restrict-workspace-trust",
            Label = "Restrict VS Code Workspace Trust",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic workspace trust prompts and defaults to restricted mode via policy.",
            Tags = ["vscode", "workspace-trust", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "security.workspace.trust.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "security.workspace.trust.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "security.workspace.trust.enabled", 0)],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "vscode-policy-extension-gallery",
            Label = "Disable VS Code Extension Gallery",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables access to the VS Code public extension marketplace (useful in locked-down environments).",
            Tags = ["vscode", "extensions", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.gallery.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-policy-online-services",
            Label = "Disable VS Code Online Services",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents VS Code from making requests to online services (cloud settings, snippets).",
            Tags = ["vscode", "privacy", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.ignoreRecommendations", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.ignoreRecommendations")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.ignoreRecommendations", 1)],
        },
        new TweakDef
        {
            Id = "vscode-policy-nls-search",
            Label = "Disable VS Code Natural Language Search",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bing-powered natural language extension search feature in VS Code.",
            Tags = ["vscode", "privacy", "search"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.naturalLanguageSearch.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.naturalLanguageSearch.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "extensions.naturalLanguageSearch.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-account-sync",
            Label = "Disable VS Code Account & Settings Sync",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks VS Code from syncing settings, keybindings and extensions to a Microsoft account.",
            Tags = ["vscode", "privacy", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.keybindingsPerPlatform", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.keybindingsPerPlatform")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "settingsSync.keybindingsPerPlatform", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-github-copilot-chat",
            Label = "Disable VS Code GitHub Copilot Chat",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables GitHub Copilot Chat AI features in VS Code via policy registry key.",
            Tags = ["vscode", "copilot", "ai", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "chat.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "chat.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "chat.enabled", 0)],
        },
        new TweakDef
        {
            Id = "vscode-disable-output-link-detection",
            Label = "Disable VS Code Output Link Detection",
            Category = "Developer — Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops VS Code from scanning terminal/output panels for clickable links (reduces CPU on heavy output).",
            Tags = ["vscode", "performance", "terminal"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "output.linkify", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "output.linkify")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\VSCode", "output.linkify", 0)],
        },
        // ── merged from: Wsl.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "wsl-disable-interop",
            Label = "Disable WSL Windows Interop",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables WSL Windows interop (running Windows executables from WSL). Default: Enabled. Recommended: Disabled for isolation.",
            Tags = ["wsl", "interop", "windows", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 0)],
        },
        new TweakDef
        {
            Id = "wsl-sparse-vhd",
            Label = "Enable WSL Sparse VHD",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables sparse VHD mode so WSL2 virtual disks automatically shrink when free space is released inside the distro. Win11 22H2+. Default: disabled. Recommended: enabled.",
            Tags = ["wsl", "sparse", "vhd", "disk", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd", 1)],
        },
        new TweakDef
        {
            Id = "wsl-firewall",
            Label = "Enable WSL Firewall Integration",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Firewall integration for WSL2 network traffic. Allows corporate firewall rules to apply to WSL traffic. Win11 22H2+. Default: disabled. Recommended: enabled on managed networks.",
            Tags = ["wsl", "firewall", "security", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "wsl-disable-gui",
            Label = "Disable WSLg (GUI App Support)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables WSLg (Windows Subsystem for Linux GUI) to reduce memory and GPU overhead when only CLI workloads are needed. Default: enabled. Recommended: disabled for CLI-only usage.",
            Tags = ["wsl", "gui", "wslg", "performance", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics", 0)],
        },
        new TweakDef
        {
            Id = "wsl-safe-mode",
            Label = "Enable WSL Safe Mode",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables WSL safe mode which bypasses custom /etc/wsl.conf settings for troubleshooting. Useful when a bad config prevents WSL from starting. Default: disabled. Recommended: disabled (enable only for debugging).",
            Tags = ["wsl", "safe-mode", "diagnostic", "troubleshooting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode", 1)],
        },
        new TweakDef
        {
            Id = "wsl-debug-console",
            Label = "Enable WSL Debug Console",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the WSL debug console for kernel log output and diagnostics. Default: disabled. Recommended: disabled (enable only for debugging).",
            Tags = ["wsl", "debug", "console", "kernel", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole", 1)],
        },
        new TweakDef
        {
            Id = "wsl-limit-memory",
            Label = "Limit WSL Memory to 4 GB",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Limits the maximum memory allocated to WSL2 virtual machines to 4096 MB. Prevents WSL from consuming excessive host RAM. Default: 50%% of host RAM. Recommended: 4 GB.",
            Tags = ["wsl", "memory", "limit", "ram", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory", 4096)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory", 4096)],
        },
        new TweakDef
        {
            Id = "wsl-systemd-default",
            Label = "Enable Systemd as Default Init (Policy)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables systemd as the default init system for WSL2 distributions via Group Policy. Required for services like snap and Docker. Default: disabled. Recommended: enabled.",
            Tags = ["wsl", "systemd", "init", "policy", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled", 1)],
        },
        new TweakDef
        {
            Id = "wsl-automount-metadata",
            Label = "Enable DrvFs Auto-Mount with Metadata",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables DrvFs metadata on Windows drive mounts inside WSL, allowing proper Linux file permissions (chmod/chown) on /mnt/c etc. Default: disabled. Recommended: enabled for development.",
            Tags = ["wsl", "drvfs", "mount", "metadata", "permissions"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata", 1)],
        },
        new TweakDef
        {
            Id = "wsl-no-windows-path",
            Label = "Disable Windows PATH Append in WSL",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents WSL from appending Windows system PATH directories to the Linux $PATH. Keeps the Linux environment clean and avoids Windows executable conflicts. Default: append enabled. Recommended: disabled for isolated dev environments.",
            Tags = ["wsl", "path", "interop", "isolation", "development"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath", 0)],
        },
        new TweakDef
        {
            Id = "wsl-swap-size",
            Label = "Limit WSL2 Swap to 2 GB",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits the WSL2 virtual machine swap file to 2 GB to prevent excessive disk usage. Default: 25%% of host RAM. Recommended: 2 GB for most workloads.",
            Tags = ["wsl", "swap", "disk", "memory", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize", 2048)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize", 2048)],
        },
        new TweakDef
        {
            Id = "wsl-gpu-compute",
            Label = "Enable GPU Compute Pass-Through (CUDA/DirectML)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables GPU compute pass-through in WSL2 for CUDA, DirectML, and OpenCL workloads. Required for machine learning and GPU-accelerated applications inside WSL. Default: enabled on Win11. Recommended: enabled.",
            Tags = ["wsl", "gpu", "cuda", "directml", "compute", "ml"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport", 1)],
        },
        new TweakDef
        {
            Id = "wsl-interop-off-policy",
            Label = "Disable WSL Windows Interop",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables WSL interoperability with Windows (launching Windows .exe from WSL). Reduces attack surface and eliminates Windows path leakage. Default: enabled. Recommended: disabled for isolated/security workloads.",
            Tags = ["wsl", "interop", "security", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-binfmt-misc",
            Label = "Disable WSL Binfmt Misc Registration",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables binfmt_misc registration in WSL2, preventing the kernel from automatically running Windows executables from Linux paths. Default: enabled. Recommended: disabled for pure-Linux dev environments.",
            Tags = ["wsl", "binfmt", "kernel", "interop", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc", 0)],
        },
        new TweakDef
        {
            Id = "wsl-limit-processors",
            Label = "Limit WSL2 VM to 4 Processors",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Caps the number of logical processors available to the WSL2 VM to 4. Prevents WSL from starving the host of CPU resources during builds. Default: all host processors. Recommended: 4 for background dev use.",
            Tags = ["wsl", "cpu", "performance", "vm", "resource"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount", 4)],
        },
        new TweakDef
        {
            Id = "wsl-disable-crash-reporting",
            Label = "Disable WSL Crash Dump Creation",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Watson (crash reporting) from creating crash dumps for WSL processes. Frees disk space and avoids slow post-crash dump write. Default: enabled. Recommended: disabled on developer machines.",
            Tags = ["wsl", "crash", "dump", "telemetry", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-telemetry",
            Label = "Disable WSL Telemetry",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables WSL subsystem telemetry data collection sent to Microsoft. Default: enabled. Recommended: disabled for privacy-focused environments.",
            Tags = ["wsl", "telemetry", "privacy", "microsoft"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-windows-path-interop",
            Label = "Disable Windows PATH Interop in WSL",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows PATH from being appended to WSL $PATH. Avoids conflicts with Windows executables. Default: enabled.",
            Tags = ["wsl", "path", "interop", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "AppendNtPath", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "AppendNtPath", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "AppendNtPath", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-gui-support",
            Label = "Disable WSLg (GUI App Support)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables WSLg (Linux GUI app support via Wayland/X11). Reduces memory and resource usage. Default: enabled.",
            Tags = ["wsl", "wslg", "gui", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableGUIApplications", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableGUIApplications", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableGUIApplications", 0)],
        },
        new TweakDef
        {
            Id = "wsl-set-default-version-2",
            Label = "Set Default WSL Version to 2",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default WSL version to 2 for new distro installations. WSL2 uses a real Linux kernel. Default: 1.",
            Tags = ["wsl", "version", "wsl2", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsl-disable-dns-tunneling",
            Label = "Disable WSL DNS Tunneling",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables DNS tunneling in WSL2. Uses host DNS resolution instead. Default: enabled in newer builds.",
            Tags = ["wsl", "dns", "tunneling", "networking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-auto-memory-reclaim",
            Label = "Disable WSL Auto Memory Reclaim",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic memory reclaim in WSL2. Prevents WSL from releasing cached memory back to Windows. Default: enabled.",
            Tags = ["wsl", "memory", "reclaim", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 0)],
        },
        // ── Command-based WSL tweaks ───────────────────────────────────────
        new TweakDef
        {
            Id = "wsl-enable-feature",
            Label = "Enable WSL Windows Feature (DISM)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables the Microsoft-Windows-Subsystem-Linux optional feature via DISM. Requires reboot.",
            Tags = ["wsl", "feature", "dism", "install"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                var (code, _, stderr) = Elevation.RunElevated(
                    "dism",
                    ["/online", "/enable-feature", "/featurename:Microsoft-Windows-Subsystem-Linux", "/norestart"]
                );
                if (code != 0 && !stderr.Contains("already enabled", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"DISM enable WSL feature failed: {stderr}");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("dism", ["/online", "/disable-feature", "/featurename:Microsoft-Windows-Subsystem-Linux", "/norestart"]);
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = Elevation.RunElevated(
                    "dism",
                    ["/online", "/get-featureinfo", "/featurename:Microsoft-Windows-Subsystem-Linux"]
                );
                return code == 0 && stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-enable-vmplatform",
            Label = "Enable Virtual Machine Platform (DISM)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables the VirtualMachinePlatform feature required for WSL2. Requires reboot.",
            Tags = ["wsl", "vm", "platform", "dism", "install"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                var (code, _, stderr) = Elevation.RunElevated(
                    "dism",
                    ["/online", "/enable-feature", "/featurename:VirtualMachinePlatform", "/norestart"]
                );
                if (code != 0 && !stderr.Contains("already enabled", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"DISM enable VirtualMachinePlatform failed: {stderr}");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                Elevation.RunElevated("dism", ["/online", "/disable-feature", "/featurename:VirtualMachinePlatform", "/norestart"]);
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = Elevation.RunElevated("dism", ["/online", "/get-featureinfo", "/featurename:VirtualMachinePlatform"]);
                return code == 0 && stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-compact-vhd",
            Label = "Compact WSL2 VHD Disks",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Terminates running WSL instances and compacts all .vhdx virtual disk files to reclaim unused space. One-time action.",
            Tags = ["wsl", "vhd", "compact", "disk", "storage"],
            KindHint = TweakKind.PowerShell,
            SideEffects = "Shuts down all running WSL2 instances.",
            ApplyAction = (_) =>
            {
                // Shut down WSL
                ShellRunner.Run("wsl", ["--shutdown"]);

                // Find and compact all .vhdx files under the WSL Lxss directory
                string lxssPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages");
                if (!Directory.Exists(lxssPath))
                    return;

                foreach (var vhdx in Directory.EnumerateFiles(lxssPath, "ext4.vhdx", SearchOption.AllDirectories))
                {
                    ShellRunner.RunPowerShell($"Optimize-VHD -Path '{vhdx.Replace("'", "''")}' -Mode Full");
                }
            },
            DetectAction = () => false, // One-time action, always shows as "not applied"
        },
        new TweakDef
        {
            Id = "wsl-shutdown",
            Label = "Shutdown All WSL2 Instances",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Immediately terminates all running WSL2 distributions and the lightweight utility VM. Frees memory and CPU resources.",
            Tags = ["wsl", "shutdown", "memory", "resource"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "All running WSL sessions will be terminated.",
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--shutdown"]);
            },
            DetectAction = () => false,
        },
        // ── Restored tweaks ───────────────────────────────────────────────

        new TweakDef
        {
            Id = "wsl-autostart",
            Label = "Auto-Start WSL2 at Logon",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds a Run key to pre-boot the WSL2 lightweight VM at logon, eliminating cold-start latency for the first wsl.exe invocation.",
            Tags = ["wsl", "startup", "performance", "boot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "WSLBootstrap", @"wsl.exe --exec /bin/true"),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "WSLBootstrap")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "WSLBootstrap", @"wsl.exe --exec /bin/true"),
            ],
        },
        new TweakDef
        {
            Id = "wsl-compact-disk",
            Label = "Enable WSL Automatic Disk Compaction",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables automatic compaction of WSL2 virtual disks to reclaim unused space without manual intervention. Win11 22H2+.",
            Tags = ["wsl", "disk", "compact", "storage", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoCompact", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoCompact")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoCompact", 1)],
        },
        new TweakDef
        {
            Id = "wsl-default-v2",
            Label = "Set Default WSL Version to 2 (CLI)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the default WSL version to 2 via the wsl.exe CLI. WSL2 uses a full Linux kernel with better I/O and syscall compatibility.",
            Tags = ["wsl", "version", "wsl2", "default", "cli"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--set-default-version", "2"]);
            },
            DetectAction = () =>
            {
                var (code, stdout, _) = ShellRunner.Run("wsl", ["--status"]);
                return code == 0 && stdout.Contains("Default Version: 2", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-default-version-2",
            Label = "Set Default WSL Version to 2 (User Registry)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default WSL version to 2 via the user-level Lxss registry key. New distro installations will use WSL2.",
            Tags = ["wsl", "version", "wsl2", "default", "registry"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsl-disable-auto-update",
            Label = "Disable WSL Auto-Update",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WSL from automatically checking for and installing kernel/runtime updates. Useful for controlled environments.",
            Tags = ["wsl", "update", "auto-update", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoUpdate", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-nested-virt",
            Label = "Disable WSL Nested Virtualisation",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Explicitly disables nested virtualisation for WSL2 guests. Reduces attack surface when Docker/KVM inside WSL is not needed.",
            Tags = ["wsl", "virtualisation", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 0)],
        },
        new TweakDef
        {
            Id = "wsl-enable-localhost-forward",
            Label = "Enable WSL Localhost Forwarding",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables automatic forwarding of WSL2 ports to localhost on the Windows host, allowing access to WSL services via 127.0.0.1.",
            Tags = ["wsl", "localhost", "forwarding", "networking", "port"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "LocalhostForwarding", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "LocalhostForwarding")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "LocalhostForwarding", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enable-nested-virt-policy",
            Label = "Enable Nested Virtualisation (Hyper-V Policy)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables nested virtualisation via Hyper-V Group Policy. Required by some organisations before the per-VM Lxss setting takes effect.",
            Tags = ["wsl", "virtualisation", "policy", "hyperv", "nested"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enable-systemd",
            Label = "Enable Systemd (User Registry)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables systemd as the default init system for WSL2 via the user-level Lxss key. Services like snap, Docker, and journald require systemd.",
            Tags = ["wsl", "systemd", "init", "services", "user"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "EnableSystemd", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "EnableSystemd")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Lxss", "EnableSystemd", 1)],
        },
        new TweakDef
        {
            Id = "wsl-feature",
            Label = "Enable WSL Feature (PowerShell)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables the Microsoft-Windows-Subsystem-Linux optional feature via PowerShell cmdlet. Requires reboot. Alternative to DISM approach.",
            Tags = ["wsl", "feature", "install", "powershell"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux -NoRestart");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Disable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux -NoRestart");
            },
            DetectAction = () =>
            {
                var (_, stdout, _2) = ShellRunner.RunPowerShell(
                    "(Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux).State"
                );
                return stdout.Trim().Equals("Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "wsl-kernel-update",
            Label = "Update WSL Kernel to Latest",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Runs wsl --update to download and install the latest WSL kernel and runtime. One-time action.",
            Tags = ["wsl", "kernel", "update", "maintenance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--update"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "wsl-mirrored-network",
            Label = "Enable WSL Mirrored Networking",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Switches WSL2 to mirrored networking mode. WSL shares the host network stack for full LAN visibility and IPv6 support. Win11 23H2+.",
            Tags = ["wsl", "network", "mirrored", "networking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MirroredNetworkingMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MirroredNetworkingMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MirroredNetworkingMode", 1)],
        },
        new TweakDef
        {
            Id = "wsl-update-distro",
            Label = "Update WSL Distributions (Web Download)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Runs wsl --update --web-download to update WSL components directly from the web. One-time action.",
            Tags = ["wsl", "update", "distro", "maintenance"],
            KindHint = TweakKind.SystemCommand,
            ApplyAction = (_) =>
            {
                ShellRunner.Run("wsl", ["--update", "--web-download"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "wsl-vm-platform",
            Label = "Enable VM Platform (PowerShell)",
            Category = "Virtualization — Windows Subsystem Linux",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables the VirtualMachinePlatform optional feature via PowerShell cmdlet. Required for WSL2. Requires reboot. Alternative to DISM approach.",
            Tags = ["wsl", "vm", "platform", "install", "powershell"],
            KindHint = TweakKind.SystemCommand,
            SideEffects = "Requires reboot to take effect.",
            ApplyAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Enable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform -NoRestart");
            },
            RemoveAction = (admin) =>
            {
                Elevation.AssertAdmin(admin);
                ShellRunner.RunPowerShell("Disable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform -NoRestart");
            },
            DetectAction = () =>
            {
                var (_, stdout, _2) = ShellRunner.RunPowerShell("(Get-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform).State");
                return stdout.Trim().Equals("Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}
