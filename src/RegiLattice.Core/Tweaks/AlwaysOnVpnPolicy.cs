// RegiLattice.Core — Tweaks/AlwaysOnVpnPolicy.cs
// Always On VPN Policy — Sprint 540.
// Configures Group Policy for Always On VPN (AOVPN) device tunnel and user tunnel
// settings. AOVPN is Microsoft's successor to DirectAccess providing IKEv2/SSL-based
// remote access without requiring IPv6. Focuses on tunnel persistence, authentication
// hardening, traffic filtering, and monitoring policies.
// Category: "Always On VPN Policy" | Slug: aovpn
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\...\VPN

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AlwaysOnVpnPolicy
{
    private const string VpnKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator";

    private const string AgentKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";

    private const string RasKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RasMan\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "aovpn-require-machine-cert-ikev2",
                Label = "Always On VPN: Require Machine Certificate for IKEv2 Auth",
                Category = "Always On VPN Policy",
                Description =
                    "Sets DisableAdvancedCredentialUI=1 in RasMan policy parameters. Disables username/password (MSCHAPv2) authentication fallback for IKEv2 VPN connections, requiring machine certificate authentication. MSCHAPv2 is vulnerable to offline brute-force attacks; certificate-based IKEv2 auth uses asymmetric cryptography that cannot be brute-forced. This policy is critical for AOVPN deployments where device tunnel must authenticate before user logon.",
                Tags = ["aovpn", "certificate", "ikev2", "authentication", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Requires valid machine certificate from enterprise CA on all AOVPN clients. Clients without certificates lose VPN connectivity.",
                ApplyOps = [RegOp.SetDword(RasKey, "DisableAdvancedCredentialUI", 1)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "DisableAdvancedCredentialUI")],
                DetectOps = [RegOp.CheckDword(RasKey, "DisableAdvancedCredentialUI", 1)],
            },
            new TweakDef
            {
                Id = "aovpn-enable-dns-registration",
                Label = "Always On VPN: Enable Dynamic DNS Registration via VPN",
                Category = "Always On VPN Policy",
                Description =
                    "Sets RegisterDnsARecords=1 in RasMan/Parameters. Enables dynamic DNS registration for the VPN adapter's IP address against the corporate DNS server when AOVPN connects. Without DNS registration, remote clients cannot be reached by hostname from the corporate network, breaking RDP-to-client, IT-admin remote management, SCCM/Intune management channels, and MDM policies that require network-initiated connections to the endpoint.",
                Tags = ["aovpn", "dns", "registration", "management", "connectivity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Creates DNS A records for VPN client IPs in corporate DNS zones. DNS records must have appropriate scavenging to prevent stale accumulation.",
                ApplyOps = [RegOp.SetDword(RasKey, "RegisterDnsARecords", 1)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "RegisterDnsARecords")],
                DetectOps = [RegOp.CheckDword(RasKey, "RegisterDnsARecords", 1)],
            },
            new TweakDef
            {
                Id = "aovpn-disable-vpn-reconnect-prompt",
                Label = "Always On VPN: Disable Reconnect UI Prompt After Disconnect",
                Category = "Always On VPN Policy",
                Description =
                    "Sets DisableReconnectToIncompatible=1. Suppresses the Windows dialog that prompts users to reconnect to their VPN after an unexpected disconnection. In AOVPN deployments, the VPN reconnects automatically without user interaction; the reconnect dialog is therefore unnecessary and confusing. Hiding it prevents users from attempting manual reconnects that could interfere with the AOVPN auto-reconnect logic.",
                Tags = ["aovpn", "reconnect", "ui", "ux", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "VPN reconnect dialog is suppressed. AOVPN auto-reconnects silently. Users should be informed that VPN connectivity is managed automatically.",
                ApplyOps = [RegOp.SetDword(RasKey, "DisableReconnectToIncompatible", 1)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "DisableReconnectToIncompatible")],
                DetectOps = [RegOp.CheckDword(RasKey, "DisableReconnectToIncompatible", 1)],
            },
            new TweakDef
            {
                Id = "aovpn-enable-bypass-for-local",
                Label = "Always On VPN: Enable Local Network Subnet Bypass",
                Category = "Always On VPN Policy",
                Description =
                    "Sets BypassForLocal=1 in VPN profile policy. Allows traffic to local network resources (LAN printers, local file shares, home NAS) to bypass the VPN tunnel and route directly over the local interface. Without local bypass, users connected via AOVPN in a full-tunnel configuration must route all local traffic through the VPN server, preventing access to home printers and causing unnecessarily slow local file transfers.",
                Tags = ["aovpn", "split-tunnel", "local-bypass", "lan", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Local subnet devices are accessible directly, bypassing VPN inspection. Corporate security policy may prohibit LAN bypass in sensitive environments.",
                ApplyOps = [RegOp.SetDword(RasKey, "BypassForLocal", 1)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "BypassForLocal")],
                DetectOps = [RegOp.CheckDword(RasKey, "BypassForLocal", 1)],
            },
            new TweakDef
            {
                Id = "aovpn-set-ikev2-max-retries",
                Label = "Always On VPN: Set IKEv2 Reconnect Max Retries to 3",
                Category = "Always On VPN Policy",
                Description =
                    "Sets MaxRetries=3 in RasMan. Limits IKEv2 tunnel re-establishment attempts to 3 on network interruptions before the AOVPN client gives up and waits for next trigger. Excessive retries during network instability cause IKEv2 SA flooding on the VPN gateway server and degrade performance for all concurrent VPN users. Three retries covers transient interruptions while preventing retry storm behavior.",
                Tags = ["aovpn", "ikev2", "retry", "reliability", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Limits IKEv2 retry attempts. On very unstable networks, AOVPN may appear disconnected longer between retries.",
                ApplyOps = [RegOp.SetDword(RasKey, "MaxRetries", 3)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "MaxRetries")],
                DetectOps = [RegOp.CheckDword(RasKey, "MaxRetries", 3)],
            },
            new TweakDef
            {
                Id = "aovpn-disable-rpc-over-http",
                Label = "Always On VPN: Disable RPC over HTTP for VPN Connections",
                Category = "Always On VPN Policy",
                Description =
                    "Sets RpcOverHttpEnabled=0 in Internet Settings. Disables Outlook's RPC over HTTP (ActiveSync/HTTPS proxy) for RPC calls when the AOVPN connection is active. RPC over HTTP creates a secondary HTTPS path for Exchange/Outlook traffic that bypasses the VPN tunnel's traffic inspection. When AOVPN is active, all corporate Outlook/Exchange traffic should route through the IPsec tunnel to the corporate Exchange server, not through a separate HTTPS path.",
                Tags = ["aovpn", "rpc", "outlook", "exchange", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "RPC-over-HTTP for Outlook Anywhere is blocked when VPN is active. Exchange-over-VPN via MAPI or EAS is the supported channel.",
                ApplyOps = [RegOp.SetDword(AgentKey, "RpcOverHttpEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(AgentKey, "RpcOverHttpEnabled")],
                DetectOps = [RegOp.CheckDword(AgentKey, "RpcOverHttpEnabled", 0)],
            },
            new TweakDef
            {
                Id = "aovpn-enable-filter-list-audit",
                Label = "Always On VPN: Enable VPN Traffic Filter Audit Logging",
                Category = "Always On VPN Policy",
                Description =
                    "Sets VpnFilterAudit=1 in RasMan. Enables Windows auditing for VPN traffic filter rule matches (AppId filters and destination IP/port filters) configured in the AOVPN profile. Filter audit events are written to the Windows Security event log (Event ID 5455). This provides visibility into which applications and traffic flows are matching or bypassing AOVPN traffic routing rules, supporting both security monitoring and VPN policy debugging.",
                Tags = ["aovpn", "audit", "filter", "logging", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Generates additional Windows Security event log entries per filter match. Event log capacity should be reviewed if filter policies are granular.",
                ApplyOps = [RegOp.SetDword(RasKey, "VpnFilterAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "VpnFilterAudit")],
                DetectOps = [RegOp.CheckDword(RasKey, "VpnFilterAudit", 1)],
            },
            new TweakDef
            {
                Id = "aovpn-disable-class-based-route",
                Label = "Always On VPN: Disable Class-Based Default Route via VPN",
                Category = "Always On VPN Policy",
                Description =
                    "Sets DisableClassBasedDefaultRoute=1 in RasMan. Prevents Windows from adding a class-based default IP route through the VPN adapter when AOVPN connects in split-tunnel mode. Class-based routes incorrectly override specific split-tunnel routes, causing internet traffic to unexpectedly route through the VPN (de facto full-tunneling despite split-tunnel configuration). Disabling class-based routes ensures only the explicitly defined AOVPN split-tunnel routes are used.",
                Tags = ["aovpn", "routing", "split-tunnel", "default-route", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Corrects routing behavior in split-tunnel AOVPN deployments. No impact on full-tunnel configurations.",
                ApplyOps = [RegOp.SetDword(RasKey, "DisableClassBasedDefaultRoute", 1)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "DisableClassBasedDefaultRoute")],
                DetectOps = [RegOp.CheckDword(RasKey, "DisableClassBasedDefaultRoute", 1)],
            },
            new TweakDef
            {
                Id = "aovpn-set-ike-sa-lifetime",
                Label = "Always On VPN: Set IKE SA Lifetime to 8 Hours",
                Category = "Always On VPN Policy",
                Description =
                    "Sets IkeProtocolStateTransitionTimeout=28800 (8 hours). Sets the IKE Security Association (SA) lifetime for AOVPN IKEv2 tunnels to 8 hours. After 8 hours, the SA requires cryptographic renewal (re-keying). Shorter SA lifetimes improve forward secrecy (compromising one session key doesn't expose 24 hours of traffic) while not rekeying so frequently that it disrupts session continuity. 8 hours matches a standard work day without mid-session rekeying.",
                Tags = ["aovpn", "ikev2", "sa-lifetime", "cryptography", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "IKE SA rekeys every 8 hours. The rekey occurs transparently without session interruption in well-configured AOVPN deployments.",
                ApplyOps = [RegOp.SetDword(RasKey, "IkeProtocolStateTransitionTimeout", 28800)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "IkeProtocolStateTransitionTimeout")],
                DetectOps = [RegOp.CheckDword(RasKey, "IkeProtocolStateTransitionTimeout", 28800)],
            },
            new TweakDef
            {
                Id = "aovpn-enable-lockdown",
                Label = "Always On VPN: Enable Lockdown Mode (Block Traffic When VPN Down)",
                Category = "Always On VPN Policy",
                Description =
                    "Sets VpnLockDown=1 in RasMan. Activates AOVPN Lockdown mode which uses the Windows Filtering Platform to block ALL network traffic except the VPN tunnel traffic when the AOVPN connection is disconnected or not yet established. In Lockdown mode, sensitive endpoint data cannot leak to the local network or internet during the window between network connection and VPN establishment. Essential for privileged endpoints handling classified or highly sensitive data.",
                Tags = ["aovpn", "lockdown", "traffic-block", "security", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "ALL network traffic is blocked if VPN is not connected. Pre-logon connections and emergency internet access are blocked. Use only on privileged endpoints where data sensitivity justifies the restriction.",
                ApplyOps = [RegOp.SetDword(RasKey, "VpnLockDown", 1)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "VpnLockDown")],
                DetectOps = [RegOp.CheckDword(RasKey, "VpnLockDown", 1)],
            },
        ];
}
