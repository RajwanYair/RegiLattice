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
            Id = "virt-optimise-dynamic-memory",
            Label = "Optimise Dynamic Memory for VMs",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Tunes Windows memory manager for better VM density by disabling large system cache and second-level data cache.",
            Tags = ["hyperv", "virtualization", "memory", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SecondLevelDataCache", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "SecondLevelDataCache"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "LargeSystemCache", 0),
            ],
        },
        new TweakDef
        {
            Id = "virt-disable-vbs",
            Label = "Disable Virtualization Based Security (VBS)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables VBS which can reduce performance in games and creative apps. WARNING: Reduces security — only use on personal gaming/performance rigs.",
            Tags = ["vbs", "virtualization", "performance", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 0),
            ],
        },
        new TweakDef
        {
            Id = "virt-disable-credential-guard",
            Label = "Disable Credential Guard",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Credential Guard (VBS-backed LSASS protection). May be needed for compatibility with third-party VPN/auth tools.",
            Tags = ["virtualization", "security", "credential-guard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "LsaCfgFlags", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "EnableVirtualizationBasedSecurity", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "LsaCfgFlags", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "EnableVirtualizationBasedSecurity", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "LsaCfgFlags", 0)],
        },
        new TweakDef
        {
            Id = "virt-disable-sandbox",
            Label = "Disable Windows Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Sandbox feature via policy.",
            Tags = ["virtualization", "sandbox", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowSandbox", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowSandbox", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowSandbox", 0)],
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
            Id = "virt-disable-appguard",
            Label = "Disable Application Guard",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Microsoft Defender Application Guard (MDAG). Frees memory and CPU used by isolation containers for Edge browser. Default: Enabled. Recommended: Disabled if not needed.",
            Tags = ["virtualization", "appguard", "security", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI", "AllowAppHVSI_ProviderSet", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI", "AllowAppHVSI_ProviderSet")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI", "AllowAppHVSI_ProviderSet", 0)],
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
            Id = "virt-nested-virt-policy",
            Label = "Enable Nested Virtualization (Policy)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables nested virtualization via Hyper-V group policy. Allows hypervisors inside VMs at the policy level. Default: Not set. Recommended: Enabled for dev workloads.",
            Tags = ["hyperv", "virtualization", "nested", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV", "AllowNestedVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "virt-enable-enhanced-session",
            Label = "Enable Hyper-V Enhanced Session Mode",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables Hyper-V Enhanced Session Mode for RDP-like VM experience with clipboard, audio, and drive sharing. Default: Disabled. Recommended: Enabled.",
            Tags = ["hyperv", "virtualization", "enhanced-session", "rdp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "AllowEnhancedSessionMode", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "AllowEnhancedSessionMode", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization", "AllowEnhancedSessionMode", 1),
            ],
        },
        new TweakDef
        {
            Id = "virt-enable-vm-platform",
            Label = "Enable Virtual Machine Platform",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables the Virtual Machine Platform / Hypervisor Enforced Code Integrity. Required for WSL2 and Android subsystem. Default: Disabled. Recommended: Enabled if using VMs.",
            Tags = ["virtualization", "vm-platform", "wsl2", "hypervisor"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                    "Enabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                    "Enabled",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity",
                    "Enabled",
                    1
                ),
            ],
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
            Id = "virt-vmms-manual",
            Label = "Set Hyper-V Virtual Machine Management Service to Manual",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Hyper-V Virtual Machine Management Service (VMMS) to manual start. Frees resources if Hyper-V VMs are not regularly used. Default: Automatic. Recommended: Manual on non-VM hosts.",
            Tags = ["virtualization", "vmms", "hyperv", "service", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 3)],
        },
        new TweakDef
        {
            Id = "virt-enable-wsl2-default",
            Label = "Set WSL Default Version to 2",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default WSL version to 2 (full Linux kernel via Hyper-V). Default: 1.",
            Tags = ["virtualization", "wsl", "wsl2", "linux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "virt-disable-hyper-v-vmms",
            Label = "Disable Hyper-V Virtual Machine Management",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Hyper-V Virtual Machine Management service. Frees resources if not using Hyper-V VMs. Default: auto.",
            Tags = ["virtualization", "hyper-v", "vmms", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\vmms", "Start", 4)],
        },
        new TweakDef
        {
            Id = "virt-disable-sandbox-networking",
            Label = "Disable Windows Sandbox Networking",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables networking inside Windows Sandbox. Isolates sandbox from the network. Default: enabled.",
            Tags = ["virtualization", "sandbox", "networking", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox", "AllowNetworking", 0)],
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
        new TweakDef
        {
            Id = "virt-require-platform-security",
            Label = "Require Virtualization Platform Security",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Requires UEFI Secure Boot and TPM for Hyper-V platform security features. Enforces hardware-backed isolation. Default: not required.",
            Tags = ["virtualization", "platform", "security", "tpm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "RequirePlatformSecurityFeatures", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "RequirePlatformSecurityFeatures")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "RequirePlatformSecurityFeatures", 3),
            ],
        },
    ];
}
