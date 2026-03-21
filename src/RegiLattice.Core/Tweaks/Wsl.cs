using System.IO;

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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableNestedVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "wsl-disable-interop",
            Label = "Disable WSL Windows Interop",
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Id = "wsl-enforce-v2-policy",
            Label = "Enforce WSL Version 2 as Default (Policy)",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces WSL version 2 as the default for all new distributions via machine-wide Group Policy. Default: not set. Recommended: version 2.",
            Tags = ["wsl", "version", "v2", "policy", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss", "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsl-limit-memory",
            Label = "Limit WSL Memory to 4 GB",
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
        // ── Command-based WSL tweaks ───────────────────────────────────────
        new TweakDef
        {
            Id = "wsl-enable-feature",
            Label = "Enable WSL Windows Feature (DISM)",
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Id = "wsl-dns-tunneling",
            Label = "Enable WSL DNS Tunneling",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables DNS tunneling in WSL2 so DNS requests are routed through the Windows host. Improves name resolution behind VPNs/proxies.",
            Tags = ["wsl", "dns", "tunneling", "networking", "vpn"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableDnsTunneling", 1)],
        },
        new TweakDef
        {
            Id = "wsl-enable-localhost-forward",
            Label = "Enable WSL Localhost Forwarding",
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
            Id = "wsl-memory-reclaim",
            Label = "Enable WSL Auto Memory Reclaim",
            Category = "WSL",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables automatic memory reclaim so WSL2 returns unused cached memory to Windows. Reduces host memory pressure. Win11 22H2+.",
            Tags = ["wsl", "memory", "reclaim", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss", "EnableAutoMemoryReclaim", 1)],
        },
        new TweakDef
        {
            Id = "wsl-mirrored-network",
            Label = "Enable WSL Mirrored Networking",
            Category = "WSL",
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
            Category = "WSL",
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
            Category = "WSL",
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
