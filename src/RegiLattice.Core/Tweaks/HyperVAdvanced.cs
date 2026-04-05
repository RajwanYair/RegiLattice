namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
