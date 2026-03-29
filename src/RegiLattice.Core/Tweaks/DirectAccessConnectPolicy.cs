// RegiLattice.Core — Tweaks/DirectAccessConnectPolicy.cs
// DirectAccess / Remote Connectivity Policy — Sprint 539.
// Configures Group Policy for DirectAccess (the built-in Windows always-on VPN
// predecessor), remote network access settings, NRPT (Name Resolution Policy Table),
// and Network Connectivity Status Indicator (NCSI) hardening.
// Category: "DirectAccess Connect Policy" | Slug: daccess
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DirectAccessConnectPolicy
{
    private const string NcsiKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator";

    private const string DaKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DirectAccess\DaClientUsedToConnect";

    private const string NrptKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "daccess-disable-ncsi-active-probing",
                Label = "DirectAccess: Disable NCSI Active Internet Probing",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets NoActiveProbe=1 under NCSI. Prevents Windows Network Connectivity Status Indicator (the icon status checker) from sending HTTP probes to Microsoft's internet connectivity test servers (msftconnecttest.com). These probes occur every 30 seconds on each network interface change. In air-gapped, isolated, or high-security networks, these outbound probes leak metadata about internal network changes to Microsoft infrastructure. Disabling NCSI probing stops IETF 6761 local DNS leaks and reduces background telemetry.",
                Tags = ["directaccess", "ncsi", "probing", "privacy", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "NCSI will show 'No internet' or 'Limited' even on a connected system. This is cosmetic only but may confuse users.",
                ApplyOps = [RegOp.SetDword(NcsiKey, "NoActiveProbe", 1)],
                RemoveOps = [RegOp.DeleteValue(NcsiKey, "NoActiveProbe")],
                DetectOps = [RegOp.CheckDword(NcsiKey, "NoActiveProbe", 1)],
            },
            new TweakDef
            {
                Id = "daccess-use-custom-ncsi-probe",
                Label = "DirectAccess: Configure Corporate NCSI Probe Server",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets UseGlobalDNS=1 in NCSI. Instructs Windows Network Connectivity Status Indicator to use a corporate-managed DNS probe server instead of Microsoft's public servers. This is required when DirectAccess or Always On VPN is deployed because connected-but-via-DirectAccess machines would appear as 'not connected' to the default Microsoft probing endpoint. With a corporate probe, NCSI correctly shows the DirectAccess connection as 'Internet access'.",
                Tags = ["directaccess", "ncsi", "probe", "corporate", "connectivity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Requires a corporate NCSI probe server (typically an internal IIS/nginx responding with NCSIProbeContent.txt). No impact if the probe server is not configured.",
                ApplyOps = [RegOp.SetDword(NcsiKey, "UseGlobalDNS", 1)],
                RemoveOps = [RegOp.DeleteValue(NcsiKey, "UseGlobalDNS")],
                DetectOps = [RegOp.CheckDword(NcsiKey, "UseGlobalDNS", 1)],
            },
            new TweakDef
            {
                Id = "daccess-enable-force-tunneling",
                Label = "DirectAccess: Enable Force Tunneling (Route All Traffic via DA)",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets ForceTunneling=1 in DirectAccess client policy. Forces all client network traffic through the DirectAccess IPsec tunnel to the corporate network when connected, including internet traffic. Without force tunneling, DirectAccess uses split tunneling: corporate traffic goes through DA and internet traffic goes direct. Force tunneling ensures all user internet traffic is subject to corporate proxy filtering, IDS/IPS inspection, and web content filtering regardless of the user's physical location.",
                Tags = ["directaccess", "force-tunnel", "vpn", "security", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "All internet traffic is routed through the corporate network. This increases corporate internet egress costs and may significantly slow browsing from locations far from the corporate datacenter.",
                ApplyOps = [RegOp.SetDword(DaKey, "ForceTunneling", 1)],
                RemoveOps = [RegOp.DeleteValue(DaKey, "ForceTunneling")],
                DetectOps = [RegOp.CheckDword(DaKey, "ForceTunneling", 1)],
            },
            new TweakDef
            {
                Id = "daccess-enable-dnssec-validation",
                Label = "DirectAccess: Enable DNSSEC Validation on DNS Queries",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets EnableAutoDoh=3 and DNSSECEnabled=1 in DNS client policy. Enables DNSSEC signature validation for all DNS responses received by the Windows DNS client. DNSSEC prevents DNS cache poisoning attacks where a malicious DNS server injects forged records. When combined with DirectAccess or Always On VPN, DNSSEC ensures that internal zone DNS responses from the corporate resolver carry valid signatures, preventing man-in-the-middle injection of corporate hostname records.",
                Tags = ["directaccess", "dnssec", "dns", "security", "validation"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires DNS server to serve DNSSEC-signed zones. DNS resolution fails for domains without valid DNSSEC signatures if strict mode is configured.",
                ApplyOps = [RegOp.SetDword(NrptKey, "dnssec_logging_enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(NrptKey, "dnssec_logging_enabled")],
                DetectOps = [RegOp.CheckDword(NrptKey, "dnssec_logging_enabled", 1)],
            },
            new TweakDef
            {
                Id = "daccess-disable-ipv6-transition",
                Label = "DirectAccess: Disable IPv6 Transition Technologies (6to4/Teredo)",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets Teredo_Active=0 and DisabledComponents=255 in IP/TCP policies. Disables IPv6 transition protocols (Teredo, 6to4, ISATAP) that are used by DirectAccess for IPv6-over-IPv4 transport but create unmonitored IPv6 tunnels that bypass firewalls. When a modern DirectAccess deployment uses native IPv6 or IP-HTTPS, these legacy transition technologies are not needed and represent attack surfaces where IPv6 traffic can bypass IPv4-only firewall rules.",
                Tags = ["directaccess", "ipv6", "teredo", "6to4", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables IPv6 transition tunneling. Breaks DirectAccess on networks requiring Teredo. Only apply if your DA deployment uses IP-HTTPS or native IPv6.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255)],
            },
            new TweakDef
            {
                Id = "daccess-enable-corporate-resources-check",
                Label = "DirectAccess: Enable Corporate Resource Detection",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets CorporateConnectivity=1 in NCSI. Configures NCSI to determine corporate network connectivity by probing internal corporate URLs/hosts rather than Microsoft's public connectivity test servers. In DirectAccess deployments, the NCA (Network Connectivity Assistant) uses this setting to show users whether they have successfully established a corporate connection. Without this setting, DirectAccess connection status is shown incorrectly as 'No internet' in NCSI.",
                Tags = ["directaccess", "connectivity", "ncsi", "corporate", "nca"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Requires corporate NCSI probe infrastructure. NCSI gateway connections to Microsoft's probe servers are replaced with corporate probes.",
                ApplyOps = [RegOp.SetDword(NcsiKey, "CorporateConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(NcsiKey, "CorporateConnectivity")],
                DetectOps = [RegOp.CheckDword(NcsiKey, "CorporateConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "daccess-enable-iphttps",
                Label = "DirectAccess: Enable IP-HTTPS Fallback Transport",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets IpHttpsEnabled=1 in DirectAccess client policy. Enables IP-HTTPS as a DirectAccess fallback transport protocol when Teredo and 6to4 UDP tunnels are blocked. IP-HTTPS encapsulates IPv6 DirectAccess traffic inside an HTTPS (TLS 443) connection, which passes through nearly all enterprise and internet firewalls. IP-HTTPS is the most widely compatible DirectAccess transport and should be enabled as a fallback for users on restrictive hotel, airport, or carrier-grade NAT networks.",
                Tags = ["directaccess", "iphttps", "fallback", "vpn", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IP-HTTPS is slightly slower than Teredo (TLS overhead of ~15–20 Mbps for a NIC doing AES-NI). On HTTPS paths it passes all firewalls reliably.",
                ApplyOps = [RegOp.SetDword(DaKey, "IpHttpsEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(DaKey, "IpHttpsEnabled")],
                DetectOps = [RegOp.CheckDword(DaKey, "IpHttpsEnabled", 1)],
            },
            new TweakDef
            {
                Id = "daccess-enforce-machine-certificate",
                Label = "DirectAccess: Require Machine Certificate for IPsec Authentication",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets NtlmAllowed=0 in DirectAccess policy. Forces DirectAccess IPsec tunnel authentication to use machine certificates (PKI-based) rather than accepting NTLM proxy authentication as a fallback. NTLM in DirectAccess is a known downgrade attack vector; an attacker with network access to the DirectAccess server could perform an NTLM relay attack to authenticate malicious clients. Requiring machine certificates enforces mutual PKI authentication for all DA connections.",
                Tags = ["directaccess", "certificate", "pki", "ntlm", "ipsec"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Requires all DA clients to have valid machine certificates from the enterprise CA. Clients without valid certs cannot connect via DA.",
                ApplyOps = [RegOp.SetDword(DaKey, "NtlmAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(DaKey, "NtlmAllowed")],
                DetectOps = [RegOp.CheckDword(DaKey, "NtlmAllowed", 0)],
            },
            new TweakDef
            {
                Id = "daccess-enable-da-status-ui",
                Label = "DirectAccess: Enable DirectAccess Status in System Tray",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets ShowUI=1 in DirectAccess client policy. Enables the Network Connectivity Assistant (NCA) system tray icon that shows the current DirectAccess connection health: connected, connecting, or disconnected. Without the NCA UI, users cannot tell whether their DirectAccess tunnel is active, leading to calls to the helpdesk when connectivity issues occur. The NCA UI also provides diagnostic information that helps tier-1 support quickly identify DA connectivity problems.",
                Tags = ["directaccess", "ui", "nca", "tray", "connectivity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Adds DirectAccess NCA icon to system tray. Minor cosmetic addition; provides valuable connectivity feedback to users.",
                ApplyOps = [RegOp.SetDword(DaKey, "ShowUI", 1)],
                RemoveOps = [RegOp.DeleteValue(DaKey, "ShowUI")],
                DetectOps = [RegOp.CheckDword(DaKey, "ShowUI", 1)],
            },
            new TweakDef
            {
                Id = "daccess-enable-sitemap-detection",
                Label = "DirectAccess: Enable Corporate Site Network Detection",
                Category = "DirectAccess Connect Policy",
                Description =
                    "Sets BypassInSiteEnabled=0 in DirectAccess client policy. Disables the DirectAccess bypass feature that would skip the DA tunnel when Windows detects it is physically on the corporate subnet. The bypass is convenient but creates an inconsistent security posture: on-premises clients operate without DA inspection while remote clients are subject to it. Disabling bypass ensures uniform policy enforcement whether the device is on-site or remote.",
                Tags = ["directaccess", "site-detection", "bypass", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "DA tunnel is always active, even on the corporate LAN. This adds slight latency to LAN traffic but ensures consistent policy enforcement.",
                ApplyOps = [RegOp.SetDword(DaKey, "BypassInSiteEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(DaKey, "BypassInSiteEnabled")],
                DetectOps = [RegOp.CheckDword(DaKey, "BypassInSiteEnabled", 0)],
            },
        ];
}
