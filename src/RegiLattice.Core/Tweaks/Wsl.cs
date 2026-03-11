namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Wsl
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsl-nested-virt",
            Label = "WSL Nested Virtualisation",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables nested virtualisation for WSL 2 guests, allowing Docker Desktop, KVM, and other VM workloads inside WSL.",
            Tags = ["wsl", "virtualisation", "docker"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Virtualization"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "wsl-disable-interop",
            Label = "Disable WSL Windows Interop",
            Category = "WSL",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables WSL Windows interop (running Windows executables from WSL). Default: Enabled. Recommended: Disabled for isolation.",
            Tags = ["wsl", "interop", "windows", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "WslInterop", 0)],
        },
        new TweakDef
        {
            Id = "wsl-sparse-vhd",
            Label = "Enable WSL Sparse VHD",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables sparse VHD mode so WSL2 virtual disks automatically shrink when free space is released inside the distro. Win11 22H2+. Default: disabled. Recommended: enabled.",
            Tags = ["wsl", "sparse", "vhd", "disk", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableSparseVhd", 1)],
        },
        new TweakDef
        {
            Id = "wsl-firewall",
            Label = "Enable WSL Firewall Integration",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Windows Firewall integration for WSL2 network traffic. Allows corporate firewall rules to apply to WSL traffic. Win11 22H2+. Default: disabled. Recommended: enabled on managed networks.",
            Tags = ["wsl", "firewall", "security", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "wsl-disable-gui",
            Label = "Disable WSLg (GUI App Support)",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables WSLg (Windows Subsystem for Linux GUI) to reduce memory and GPU overhead when only CLI workloads are needed. Default: enabled. Recommended: disabled for CLI-only usage.",
            Tags = ["wsl", "gui", "wslg", "performance", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableWslGraphics", 0)],
        },
        new TweakDef
        {
            Id = "wsl-safe-mode",
            Label = "Enable WSL Safe Mode",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables WSL safe mode which bypasses custom /etc/wsl.conf settings for troubleshooting. Useful when a bad config prevents WSL from starting. Default: disabled. Recommended: disabled (enable only for debugging).",
            Tags = ["wsl", "safe-mode", "diagnostic", "troubleshooting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SafeMode", 1)],
        },
        new TweakDef
        {
            Id = "wsl-debug-console",
            Label = "Enable WSL Debug Console",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the WSL debug console for kernel log output and diagnostics. Default: disabled. Recommended: disabled (enable only for debugging).",
            Tags = ["wsl", "debug", "console", "kernel", "diagnostic"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DebugConsole", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enforce-v2-policy",
            Label = "Enforce WSL Version 2 as Default (Policy)",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enforces WSL version 2 as the default for all new distributions via machine-wide Group Policy. Default: not set. Recommended: version 2.",
            Tags = ["wsl", "version", "v2", "policy", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion", 2),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsl-limit-memory",
            Label = "Limit WSL Memory to 4 GB",
            Category = "WSL",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Limits the maximum memory allocated to WSL2 virtual machines to 4096 MB. Prevents WSL from consuming excessive host RAM. Default: 50%% of host RAM. Recommended: 4 GB.",
            Tags = ["wsl", "memory", "limit", "ram", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory", 4096),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "MaxVmMemory", 4096)],
        },
        new TweakDef
        {
            Id = "wsl-systemd-default",
            Label = "Enable Systemd as Default Init (Policy)",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables systemd as the default init system for WSL2 distributions via Group Policy. Required for services like snap and Docker. Default: disabled. Recommended: enabled.",
            Tags = ["wsl", "systemd", "init", "policy", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "SystemdEnabled", 1)],
        },
        new TweakDef
        {
            Id = "wsl-automount-metadata",
            Label = "Enable DrvFs Auto-Mount with Metadata",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DrvFs metadata on Windows drive mounts inside WSL, allowing proper Linux file permissions (chmod/chown) on /mnt/c etc. Default: disabled. Recommended: enabled for development.",
            Tags = ["wsl", "drvfs", "mount", "metadata", "permissions"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "DrvFsEnableMetadata", 1)],
        },
        new TweakDef
        {
            Id = "wsl-no-windows-path",
            Label = "Disable Windows PATH Append in WSL",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WSL from appending Windows system PATH directories to the Linux $PATH. Keeps the Linux environment clean and avoids Windows executable conflicts. Default: append enabled. Recommended: disabled for isolated dev environments.",
            Tags = ["wsl", "path", "interop", "isolation", "development"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "AppendNtPath", 0)],
        },
        new TweakDef
        {
            Id = "wsl-swap-size",
            Label = "Limit WSL2 Swap to 2 GB",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits the WSL2 virtual machine swap file to 2 GB to prevent excessive disk usage. Default: 25%% of host RAM. Recommended: 2 GB for most workloads.",
            Tags = ["wsl", "swap", "disk", "memory", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize", 2048),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "SwapSize", 2048)],
        },
        new TweakDef
        {
            Id = "wsl-gpu-compute",
            Label = "Enable GPU Compute Pass-Through (CUDA/DirectML)",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables GPU compute pass-through in WSL2 for CUDA, DirectML, and OpenCL workloads. Required for machine learning and GPU-accelerated applications inside WSL. Default: enabled on Win11. Recommended: enabled.",
            Tags = ["wsl", "gpu", "cuda", "directml", "compute", "ml"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows", "EnableGpuSupport", 1)],
        },
        new TweakDef
        {
            Id = "wsl-interop-off-policy",
            Label = "Disable WSL Windows Interop",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables WSL interoperability with Windows (launching Windows .exe from WSL). Reduces attack surface and eliminates Windows path leakage. Default: enabled. Recommended: disabled for isolated/security workloads.",
            Tags = ["wsl", "interop", "security", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableInterop", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-binfmt-misc",
            Label = "Disable WSL Binfmt Misc Registration",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables binfmt_misc registration in WSL2, preventing the kernel from automatically running Windows executables from Linux paths. Default: enabled. Recommended: disabled for pure-Linux dev environments.",
            Tags = ["wsl", "binfmt", "kernel", "interop", "isolation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableBinfmtMisc", 0)],
        },
        new TweakDef
        {
            Id = "wsl-limit-processors",
            Label = "Limit WSL2 VM to 4 Processors",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Caps the number of logical processors available to the WSL2 VM to 4. Prevents WSL from starving the host of CPU resources during builds. Default: all host processors. Recommended: 4 for background dev use.",
            Tags = ["wsl", "cpu", "performance", "vm", "resource"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount", 4),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "ProcessorCount", 4)],
        },
        new TweakDef
        {
            Id = "wsl-disable-crash-reporting",
            Label = "Disable WSL Crash Dump Creation",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Watson (crash reporting) from creating crash dumps for WSL processes. Frees disk space and avoids slow post-crash dump write. Default: enabled. Recommended: disabled on developer machines.",
            Tags = ["wsl", "crash", "dump", "telemetry", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DrWatson", "CreateCrashDump", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-telemetry",
            Label = "Disable WSL Telemetry",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables WSL subsystem telemetry data collection sent to Microsoft. Default: enabled. Recommended: disabled for privacy-focused environments.",
            Tags = ["wsl", "telemetry", "privacy", "microsoft"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableTelemetry", 0)],
        },
        new TweakDef
        {
            Id = "wsl-disable-windows-path-interop",
            Label = "Disable Windows PATH Interop in WSL",
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic memory reclaim in WSL2. Prevents WSL from releasing cached memory back to Windows. Default: enabled.",
            Tags = ["wsl", "memory", "reclaim", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 0)],
        },
    ];
}
