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
            Id = "virt-set-hyperv-scheduler-classic",
            Label = "Set Hyper-V Classic Scheduler",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the hypervisor scheduler type to classic (non-root). Better CPU performance for VMs at the cost of security mitigations.",
            Tags = ["virtualization", "hyper-v", "scheduler", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "SchedulerType", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "SchedulerType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV", "SchedulerType", 1)],
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
            Id = "virt-disable-wdag-policy",
            Label = "Disable Windows Defender Application Guard via Policy",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Defender Application Guard (WDAG) via Group Policy. Frees VBS resources when WDAG is not needed.",
            Tags = ["virtualization", "wdag", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI", "AllowAppHVSI_ProviderSet", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI", "AllowAppHVSI_ProviderSet")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI", "AllowAppHVSI_ProviderSet", 0)],
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
            Id = "sandbox-disable-networking",
            Label = "Disable Windows Sandbox Networking",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "network", "isolation"],
            Description =
                "Disables network access inside Windows Sandbox via Group Policy. "
                + "Useful when analysing potentially malicious files and you don't want the sample to phone home or download payloads.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowNetworking", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowNetworking")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowNetworking", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-vgpu",
            Label = "Disable Windows Sandbox vGPU (Protect Host GPU Driver)",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "gpu", "vgpu", "isolation"],
            Description =
                "Disables GPU virtualization for Windows Sandbox. "
                + "Prevents sandbox workloads from accessing the host GPU driver attack surface. "
                + "Sandbox falls back to software rendering — slower but more isolated.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowVGPU", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowVGPU")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowVGPU", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-clipboard",
            Label = "Disable Windows Sandbox Clipboard Sharing",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "clipboard", "isolation", "data exfiltration"],
            Description =
                "Prevents clipboard data from being shared between the host and the sandbox. "
                + "Stops malicious code inside the sandbox from reading sensitive clipboard contents (passwords, tokens) from the host.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowClipboardRedirection", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowClipboardRedirection")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowClipboardRedirection", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-audio",
            Label = "Disable Windows Sandbox Audio Input",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "audio", "microphone", "isolation"],
            Description =
                "Disables microphone and audio input redirection into Windows Sandbox. "
                + "Prevents malware inside the sandbox from accessing the host's microphone to eavesdrop.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowAudioInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowAudioInput")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowAudioInput", 0)],
        },
        new TweakDef
        {
            Id = "sandbox-disable-video-input",
            Label = "Disable Windows Sandbox Camera/Video Input",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "camera", "video", "privacy", "isolation"],
            Description =
                "Disables camera and video input device redirection into Windows Sandbox. "
                + "Prevents code inside the sandbox from activating the host's camera.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowVideoInput", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowVideoInput")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowVideoInput", 0)],
        },
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
            Id = "sandbox-protect-client-folders",
            Label = "Restrict Windows Sandbox Mapped Folder Write Access",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "security", "mapped folders", "write protection", "isolation"],
            Description =
                "Enforces read-only mode for all host folders mapped into Windows Sandbox by policy. "
                + "Prevents executed code from writing back to host filesystem through shared directories.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowMappedFolders", 1), RegOp.SetDword(SbPol, "AllowWriteToMappedFolders", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowWriteToMappedFolders")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowWriteToMappedFolders", 0)],
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
            Id = "sandbox-disable-all-folder-mapping",
            Label = "Disable All Host Folder Mapping in Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["sandbox", "isolation", "folders", "security"],
            Description =
                "Completely blocks host folder sharing into Windows Sandbox via Group Policy. "
                + "Combined with the write-protect tweak this provides the strongest "
                + "isolation: no host files are accessible from within the sandbox at all.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowMappedFolders", 0)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowMappedFolders")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowMappedFolders", 0)],
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
            Id = "sandbox-enable-protected-client",
            Label = "Enable Protected Client Mode for Windows Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "protected client", "isolation", "security"],
            Description =
                "Runs the Sandbox's RDP client in Windows Protected Process Light (PPL) "
                + "mode. This blocks injection into the sandbox session host process "
                + "from even admin-level processes on the host.",
            ApplyOps = [RegOp.SetDword(SbPol, "AllowProtectedClient", 1)],
            RemoveOps = [RegOp.DeleteValue(SbPol, "AllowProtectedClient")],
            DetectOps = [RegOp.CheckDword(SbPol, "AllowProtectedClient", 1)],
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
            Id = "sandbox-disable-telemetry",
            Label = "Disable Telemetry and Diagnostics Inside Sandbox",
            Category = "Virtualization",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sandbox", "telemetry", "privacy", "diagnostics"],
            Description =
                "Turns off Windows telemetry and diagnostic data collection "
                + "within the sandbox environment. Useful for clean-slate testing "
                + "and prevents telemetry from leaking sandbox activity to Microsoft.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0)],
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
