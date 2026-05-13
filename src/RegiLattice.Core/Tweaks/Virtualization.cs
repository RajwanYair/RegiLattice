namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
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
