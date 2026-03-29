// RegiLattice.Core — Tweaks/Wsl2AdvancedPolicy.cs
// WSL2 distro management, memory/CPU limits, feature gating, and kernel parameters policy — Sprint 518.
// Category: "WSL2 Advanced Policy" | Slug: wsl2adv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Lxss

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Wsl2AdvancedPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss";
    private const string FwKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Firewall";
    private const string NwKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss\Networking";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "wsl2adv-disable-wsl2-entirely",
            Label        = "Disable WSL2 (Windows Subsystem for Linux 2) Entirely",
            Category     = "WSL2 Advanced Policy",
            Description  = "Completely disables WSL2 via policy, preventing installation of Linux distributions and blocking any WSL2 virtual machine from starting. Applied on endpoints where Linux environments are not permitted.",
            Tags         = ["wsl2", "linux", "subsystem", "disable", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "WSL2 fully disabled; no Linux distro installation or execution possible on this endpoint.",
            ApplyOps     = [RegOp.SetDword(Key, "DefaultVersion", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DefaultVersion")],
            DetectOps    = [RegOp.CheckDword(Key, "DefaultVersion", 0)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-block-network-access",
            Label        = "Block WSL2 Instances from Making Network Connections",
            Category     = "WSL2 Advanced Policy",
            Description  = "Prevents WSL2 virtual machine network interfaces from making outbound connections, isolating Linux workloads from the network when running untrusted code inside WSL2 distros.",
            Tags         = ["wsl2", "network", "isolation", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "WSL2 network access blocked; Linux distros run air-gapped with no internet or LAN connectivity.",
            ApplyOps     = [RegOp.SetDword(NwKey, "BlockOutboundNetwork", 1)],
            RemoveOps    = [RegOp.DeleteValue(NwKey, "BlockOutboundNetwork")],
            DetectOps    = [RegOp.CheckDword(NwKey, "BlockOutboundNetwork", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-block-system-distro",
            Label        = "Block Installation of WSL2 System Distributions",
            Category     = "WSL2 Advanced Policy",
            Description  = "Prevents the installation of Microsoft-managed system utility distributions (Docker Desktop backend, cloud-agent distros) from being registered in WSL2, limiting distros to user-managed ones.",
            Tags         = ["wsl2", "system-distro", "docker", "cloud-agent", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "System WSL2 distros blocked; Docker Desktop WSL2 backend and cloud agent distros cannot register.",
            ApplyOps     = [RegOp.SetDword(Key, "BlockSystemDistros", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "BlockSystemDistros")],
            DetectOps    = [RegOp.CheckDword(Key, "BlockSystemDistros", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-disable-wsl2-localhost-relay",
            Label        = "Disable WSL2 Localhost Port Relay to Windows Host",
            Category     = "WSL2 Advanced Policy",
            Description  = "Prevents automatic port forwarding from WSL2 Linux network listeners to the Windows host network, stopping services running inside WSL2 from being reachable on Windows localhost without explicit configuration.",
            Tags         = ["wsl2", "localhost-relay", "port-forwarding", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WSL2 localhost port relay disabled; Linux services not automatically accessible on Windows localhost.",
            ApplyOps     = [RegOp.SetDword(NwKey, "DisableLocalhostRelay", 1)],
            RemoveOps    = [RegOp.DeleteValue(NwKey, "DisableLocalhostRelay")],
            DetectOps    = [RegOp.CheckDword(NwKey, "DisableLocalhostRelay", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-disable-wsl2-firewall-integration",
            Label        = "Disable WSL2 Windows Firewall Integration",
            Category     = "WSL2 Advanced Policy",
            Description  = "Prevents WSL2 from applying Windows Firewall rules to the virtual machine network adapter, ensuring WSL2 network traffic is not filtered or monitored by Windows Defender Firewall rules created for WSL2.",
            Tags         = ["wsl2", "firewall", "network", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WSL2 firewall integration disabled; Windows Firewall rules not applied to WSL2 virtual network.",
            ApplyOps     = [RegOp.SetDword(FwKey, "DisableFirewallIntegration", 1)],
            RemoveOps    = [RegOp.DeleteValue(FwKey, "DisableFirewallIntegration")],
            DetectOps    = [RegOp.CheckDword(FwKey, "DisableFirewallIntegration", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-disable-wsl2-dns-tunneling",
            Label        = "Disable WSL2 DNS Tunneling Mode",
            Category     = "WSL2 Advanced Policy",
            Description  = "Disables WSL2 DNS tunneling (introduced in Windows 11 22H2) which routes DNS queries from Linux through a Windows-side DNS proxy, reverting to the standard VM network DNS and preventing potential data disclosure via DNS-over-proxy.",
            Tags         = ["wsl2", "dns-tunneling", "dns", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WSL2 DNS tunneling disabled; Linux DNS queries use VM network directly, not Windows DNS proxy.",
            ApplyOps     = [RegOp.SetDword(NwKey, "DisableDNSTunneling", 1)],
            RemoveOps    = [RegOp.DeleteValue(NwKey, "DisableDNSTunneling")],
            DetectOps    = [RegOp.CheckDword(NwKey, "DisableDNSTunneling", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-block-gui-apps",
            Label        = "Block WSL2 GUI (WSLg) Application Display on Windows Desktop",
            Category     = "WSL2 Advanced Policy",
            Description  = "Prevents WSL2 Linux GUI applications from rendering their windows on the Windows host desktop via WSLg (Windows Subsystem for Linux GUI), limiting WSL2 usage to headless/terminal-only workloads.",
            Tags         = ["wsl2", "wslg", "gui-apps", "linux", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WSLg blocked; Linux GUI apps cannot render windows on Windows desktop. Terminal access only.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableWSLg", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableWSLg")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableWSLg", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-disable-wsl2-telemetry",
            Label        = "Disable WSL2 Telemetry Reporting to Microsoft",
            Category     = "WSL2 Advanced Policy",
            Description  = "Prevents WSL2 from sending distro usage, feature adoption, crash, and diagnostics telemetry to Microsoft.",
            Tags         = ["wsl2", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WSL2 telemetry to Microsoft disabled; distro usage and crash data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-log-distro-lifecycle",
            Label        = "Log WSL2 Distro Registration and Start Events",
            Category     = "WSL2 Advanced Policy",
            Description  = "Enables Windows event log entries when a WSL2 Linux distribution is registered, unregistered, started, or terminated, providing audit visibility into Linux environment usage on corporate endpoints.",
            Tags         = ["wsl2", "event-log", "audit", "distro", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WSL2 distro lifecycle events logged; Linux environment registration and use visible for auditing.",
            ApplyOps     = [RegOp.SetDword(Key, "LogDistroLifecycle", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LogDistroLifecycle")],
            DetectOps    = [RegOp.CheckDword(Key, "LogDistroLifecycle", 1)],
        },
        new TweakDef
        {
            Id           = "wsl2adv-require-admin-distro-install",
            Label        = "Require Administrator to Install WSL2 Distributions",
            Category     = "WSL2 Advanced Policy",
            Description  = "Prevents standard users from installing new Linux distributions into WSL2, ensuring distro installation is an administrative action subject to change management.",
            Tags         = ["wsl2", "distro-install", "admin", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WSL2 distro installation restricted to admins; standard users cannot add new Linux distributions.",
            ApplyOps     = [RegOp.SetDword(Key, "RequireAdminForDistroInstall", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequireAdminForDistroInstall")],
            DetectOps    = [RegOp.CheckDword(Key, "RequireAdminForDistroInstall", 1)],
        },
    ];
}
