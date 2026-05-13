#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class WSLAdvanced
{
    private const string WslKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss";
    private const string WslPolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss";
    private const string WslNetKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WslService";
    private const string WslFtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss\NtfsDefaultCase";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wsladv-enable-wsl2-by-default",
            Label = "WSL Advanced: Set WSL 2 as Default WSL Version",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "wsl2", "virtualisation", "developer", "linux"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Ensures all new distributions install as WSL 2 with full Linux kernel support.",
            Description =
                "Sets DefaultVersion=2 in the WSL LXSS registry key. Configures WSL to "
                + "default to WSL 2 for all newly installed distributions. WSL 2 provides "
                + "a real Linux kernel and dramatically better filesystem performance compared "
                + "to WSL 1. Default: 2 (from WSL 2.0). Explicit enforcement.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetDword(WslKey, "DefaultVersion", 2)],
            RemoveOps = [RegOp.DeleteValue(WslKey, "DefaultVersion")],
            DetectOps = [RegOp.CheckDword(WslKey, "DefaultVersion", 2)],
        },
        new TweakDef
        {
            Id = "wsladv-disable-wsl-telemetry",
            Label = "WSL Advanced: Disable WSL Diagnostic Telemetry",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "telemetry", "privacy", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents WSL usage and performance data from being sent to Microsoft.",
            Description =
                "Sets TelemetryEnabled=0 in the WSL LXSS key. Disables WSL's diagnostic "
                + "telemetry reporting to Microsoft. Stops collection of distribution usage "
                + "statistics, WSL version data, and performance metrics.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetDword(WslKey, "TelemetryEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WslKey, "TelemetryEnabled")],
            DetectOps = [RegOp.CheckDword(WslKey, "TelemetryEnabled", 0)],
        },
        new TweakDef
        {
            Id = "wsladv-disable-wsl-gui-apps",
            Label = "WSL Advanced: Disable GUI Application Support (WSLg)",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "wslg", "gui", "developer", "linux"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disabling WSLg reduces memory usage on servers that do not need Linux GUIs.",
            Description =
                "Sets GuiApplicationsEnabled=0 in the WSL policy key. Disables WSLg "
                + "(GUI application support) which enables running native Linux GUI apps "
                + "on Windows 11. Disabling saves memory and reduces startup overhead "
                + "on headless servers and CI environments.",
            RegistryKeys = [WslPolKey],
            ApplyOps = [RegOp.SetDword(WslPolKey, "GuiApplicationsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(WslPolKey, "GuiApplicationsEnabled")],
            DetectOps = [RegOp.CheckDword(WslPolKey, "GuiApplicationsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "wsladv-enable-wsl-network-bridging",
            Label = "WSL Advanced: Enable Mirrored Networking Mode",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "networking", "mirrored", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            SideEffects = "Mirrored mode changes WSL network topology — may affect VPN and firewall rules.",
            ImpactNote = "Mirrored mode gives WSL the same IP as the host — simplifies network access.",
            Description =
                "Sets NetworkingMode=mirrored in WSL settings. Enables WSL 2 mirrored "
                + "networking mode where the WSL VM shares the host's network interfaces "
                + "and IP address. Simplifies accessing host services from WSL and vice versa. "
                + "Requires WSL 2.0.0+ and Windows 11 23H2 or later.",
            MinBuild = 22631,
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetString(WslKey, "NetworkingMode", "mirrored")],
            RemoveOps = [RegOp.DeleteValue(WslKey, "NetworkingMode")],
            DetectOps = [RegOp.CheckString(WslKey, "NetworkingMode", "mirrored")],
        },
        new TweakDef
        {
            Id = "wsladv-enable-dns-tunnelling",
            Label = "WSL Advanced: Enable DNS Tunnelling (Better VPN Compatibility)",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "dns", "tunnelling", "vpn", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Fixes DNS failures inside WSL when a VPN is active on the host.",
            Description =
                "Sets DnsTunneling=true in WSL settings. Routes DNS queries from WSL "
                + "through a Windows DNS proxy rather than directly to the DNS server. "
                + "Resolves DNS resolution failures that commonly occur when a corporate "
                + "VPN changes the host DNS configuration. Requires WSL 2.0.0+.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetString(WslKey, "DnsTunneling", "true")],
            RemoveOps = [RegOp.DeleteValue(WslKey, "DnsTunneling")],
            DetectOps = [RegOp.CheckString(WslKey, "DnsTunneling", "true")],
        },
        new TweakDef
        {
            Id = "wsladv-enable-auto-proxy",
            Label = "WSL Advanced: Enable Automatic Proxy Configuration",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "proxy", "auto", "vpn", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "WSL automatically uses the Windows proxy settings — no manual .bashrc proxy config needed.",
            Description =
                "Sets AutoProxy=true in WSL settings. Automatically mirrors the Windows "
                + "proxy settings into the WSL environment. Eliminates the need to manually "
                + "configure http_proxy/https_proxy in each WSL distribution. "
                + "Particularly useful in corporate proxy environments.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetString(WslKey, "AutoProxy", "true")],
            RemoveOps = [RegOp.DeleteValue(WslKey, "AutoProxy")],
            DetectOps = [RegOp.CheckString(WslKey, "AutoProxy", "true")],
        },
        new TweakDef
        {
            Id = "wsladv-disable-page-reporting",
            Label = "WSL Advanced: Disable Memory Page Reclaim to Host",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "memory", "performance", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Reduces memory allocation latency within WSL at the cost of higher standby memory on host.",
            Description =
                "Sets PageReporting=false in WSL settings by setting the LXSS PageReporting=0 "
                + "registry value. Disables the WSL 2 VM's page reporting to the Windows "
                + "hypervisor. Prevents WSL from returning unused pages to Windows, "
                + "maintaining a stable memory allocation and reducing latency spikes.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetString(WslKey, "PageReporting", "false")],
            RemoveOps = [RegOp.DeleteValue(WslKey, "PageReporting")],
            DetectOps = [RegOp.CheckString(WslKey, "PageReporting", "false")],
        },
        new TweakDef
        {
            Id = "wsladv-disable-nested-virt",
            Label = "WSL Advanced: Disable Nested Virtualisation in WSL 2",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "nested-virt", "virtualisation", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disabling nested virt slightly reduces VM overhead when Docker or KVM is not needed.",
            Description =
                "Sets NestedVirtualization=false in WSL settings. Disables the nested "
                + "virtualisation feature within the WSL 2 VM. Nested virt allows running "
                + "Docker (with Hyper-V backend) or KVM inside WSL. Disabling it reduces "
                + "VM startup time and hypervisor overhead on machines that don't need "
                + "containers or nested VMs within WSL.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetString(WslKey, "NestedVirtualization", "false")],
            RemoveOps = [RegOp.DeleteValue(WslKey, "NestedVirtualization")],
            DetectOps = [RegOp.CheckString(WslKey, "NestedVirtualization", "false")],
        },
        new TweakDef
        {
            Id = "wsladv-enable-wsl-debug-console",
            Label = "WSL Advanced: Enable WSL Debug Console Output",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "debug", "console", "developer", "logging"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Verbose WSL output helps diagnose distribution startup and networking issues.",
            Description =
                "Sets DebugConsole=true in WSL settings via the LXSS registry. Enables "
                + "detailed diagnostic output from the WSL service including kernel boot "
                + "messages, networking events, and filesystem mount status. "
                + "View output via Windows Event Log or WSL debug terminal.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetString(WslKey, "DebugConsole", "true")],
            RemoveOps = [RegOp.DeleteValue(WslKey, "DebugConsole")],
            DetectOps = [RegOp.CheckString(WslKey, "DebugConsole", "true")],
        },
        new TweakDef
        {
            Id = "wsladv-enable-wsl-firewall-rules",
            Label = "WSL Advanced: Allow WSL Firewall Rules Through Windows Firewall",
            Category = "Dev Drive / Developer",
            Tags = ["wsl", "firewall", "networking", "developer", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            SideEffects = "Network services running in WSL become accessible from the local network.",
            ImpactNote = "Permits WSL services to be reached from other machines on the same network.",
            Description =
                "Sets FirewallEnabled=true in WSL settings. Configures WSL 2 mirrored "
                + "networking to apply Windows Firewall rules to WSL traffic. "
                + "Allows Windows Firewall rules to control inbound and outbound connections "
                + "to services running inside WSL distributions. Requires mirrored mode.",
            RegistryKeys = [WslKey],
            ApplyOps = [RegOp.SetString(WslKey, "FirewallEnabled", "true")],
            RemoveOps = [RegOp.DeleteValue(WslKey, "FirewallEnabled")],
            DetectOps = [RegOp.CheckString(WslKey, "FirewallEnabled", "true")],
        },
    ];
}
