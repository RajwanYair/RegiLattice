// RegiLattice.Core — Tweaks/RemoteNetworkAccessPolicy.cs
// Remote Network Access & RAS Policy — Sprint 541.
// Configures Group Policy for remote access infrastructure: RAS (Remote Access
// Service) server policies, dial-in permissions, VPN protocol settings, and
// the Windows RAS/VPN credential management security controls.
// Category: "Remote Network Access Policy" | Slug: rnas
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\...\RemoteAccess

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RemoteNetworkAccessPolicy
{
    private const string RasKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAccessProtection";

    private const string RemAccKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc";

    private const string RasMgrKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "rnas-disable-nap-client",
                Label = "Remote Access: Disable Network Access Protection (NAP) Client",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets EnableNap=0 in Network Access Protection policy. Disables the legacy Windows Network Access Protection (NAP) client which was deprecated in Windows Server 2012 R2 and removed from the enforcement role. NAP agents running on modern Windows clients generate event log warnings and consume background resources checking against a non-existent NAP infrastructure. On corporate networks without NAP servers, disabling the NAP client eliminates background health validation traffic and event log noise.",
                Tags = ["remote-access", "nap", "legacy", "network", "cleanup"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables legacy NAP client. No impact on modern network access enforcement (Intune Compliance, Azure AD Conditional Access, NPS). Only affects deprecated Windows Server 2008-era NAP infrastructure.",
                ApplyOps = [RegOp.SetDword(RasKey, "EnableNap", 0)],
                RemoveOps = [RegOp.DeleteValue(RasKey, "EnableNap")],
                DetectOps = [RegOp.CheckDword(RasKey, "EnableNap", 0)],
            },
            new TweakDef
            {
                Id = "rnas-restrict-rpc-remote-calls",
                Label = "Remote Access: Restrict Unauthenticated RPC Remote Calls",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets RestrictRemoteClients=1 in RPC policy. Restricts unauthenticated (anonymous) RPC remote procedure calls to the local machine. By default, Windows accepts anonymous RPC calls from any network client for certain services (print spooler, DCOM). This is the root cause exploit path for critical Windows vulnerabilities including PrintNightmare (CVE-2021-34527) and several DCOM escalation chains. Restricting anonymous RPC to local clients eliminates these remote code execution attack vectors.",
                Tags = ["remote-access", "rpc", "anonymous", "printspooler", "harden"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Blocks unauthenticated RPC from remote machines. May break legacy applications using anonymous DCOM or RPC on the local network.",
                ApplyOps = [RegOp.SetDword(RemAccKey, "RestrictRemoteClients", 1)],
                RemoveOps = [RegOp.DeleteValue(RemAccKey, "RestrictRemoteClients")],
                DetectOps = [RegOp.CheckDword(RemAccKey, "RestrictRemoteClients", 1)],
            },
            new TweakDef
            {
                Id = "rnas-enable-remote-access-audit",
                Label = "Remote Access: Enable Remote Access Service Audit Logging",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets EnableRemoteAccessAudit=1 in Remote Access policy. Enables comprehensive audit logging for Windows RAS/VPN connection events, including connection establishment, authentication success/failure, accounting start/stop, and session termination. These events are critical for SIEM correlation, compliance reporting (SOC 2, ISO 27001), and incident response timelines when investigating unauthorized remote access. Without this policy, VPN connection events may not appear in the Windows Security event log.",
                Tags = ["remote-access", "vpn", "audit", "logging", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Generates VPN connection/disconnection audit events in the Security log. Ensure log capacity and SIEM forwarding are configured to handle the additional log volume.",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "EnableRemoteAccessAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "EnableRemoteAccessAudit")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "EnableRemoteAccessAudit", 1)],
            },
            new TweakDef
            {
                Id = "rnas-disable-pap-auth",
                Label = "Remote Access: Disable PAP (Plaintext Password) Authentication",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets AllowPap=0 in Remote Access policy. Disables Password Authentication Protocol (PAP) for VPN authentication. PAP transmits usernames and passwords in plaintext in the CHAP exchange, making them trivially interceptable on any network where the traffic can be captured (including TCP/IP networks without encryption). All modern VPN implementations use MSCHAPv2 or certificate-based authentication; PAP support is a legacy compatibility option that should be removed from all VPN endpoints.",
                Tags = ["remote-access", "pap", "authentication", "plaintext", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Breaks VPN clients using PAP authentication. All modern VPN clients use MSCHAPv2 or EAP; PAP is only used by very old Cisco/legacy clients.",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "AllowPap", 0)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "AllowPap")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "AllowPap", 0)],
            },
            new TweakDef
            {
                Id = "rnas-set-idle-hwm-timeout",
                Label = "Remote Access: Set Remote Access Idle Timeout to 20 Minutes",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets IdleTimeoutMinutes=20 in Remote Access policy. Sets the VPN idle timeout: after 20 minutes of no user-initiated traffic through the VPN tunnel, the server terminates the connection. Idle VPN sessions hold server resources (IP allocations, NAT state, crypto session keys) indefinitely without this timeout. The 20-minute window accommodates brief work pauses while ensuring sessions from laptops left unattended in public locations are eventually cleaned up.",
                Tags = ["remote-access", "vpn", "idle-timeout", "resource", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "VPN sessions are terminated after 20 minutes of idle traffic. AOVPN clients reconnect automatically; manual VPN users must reconnect.",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "IdleTimeoutMinutes", 20)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "IdleTimeoutMinutes")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "IdleTimeoutMinutes", 20)],
            },
            new TweakDef
            {
                Id = "rnas-disable-legacy-protocols",
                Label = "Remote Access: Disable Legacy VPN Protocols (PPTP, L2TP without IPsec)",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets AllowPptp=0 in Remote Access policy. Disables PPTP (Point-to-Point Tunneling Protocol) VPN connections. PPTP uses RC4 encryption which is broken: known plaintext attacks can decrypt PPTP streams in real time given sufficient traffic. Microsoft released MS-CHAPv2 as PPTP's authentication but MS-CHAPv2 dictionary attacks complete in hours on modern hardware. PPTP provides no meaningful security. IKEv2 or SSL VPN should replace PPTP in all enterprise environments.",
                Tags = ["remote-access", "pptp", "legacy", "encryption", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Breaks PPTP VPN connections. Very old client operating systems (Windows XP, early Android) using PPTP will not connect. IKEv2/SSL clients are unaffected.",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "AllowPptp", 0)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "AllowPptp")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "AllowPptp", 0)],
            },
            new TweakDef
            {
                Id = "rnas-enable-ikev2-mobility",
                Label = "Remote Access: Enable IKEv2 Mobility (Network Roaming Support)",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets EnableIkev2Mobility=1 in Remote Access policy. Activates IKEv2 MOBIKE (RFC 4555) support for VPN sessions. MOBIKE allows an active IKEv2 VPN tunnel to survive a client IP address change (e.g., switching from Ethernet to Wi-Fi, or from office to home network) without tearing down and re-establishing the tunnel. With MOBIKE, users experience seamless network transitions with VPN reconnection times of under 1 second instead of the 5–15 seconds required for full IKEv2 re-establishment.",
                Tags = ["remote-access", "ikev2", "mobility", "mobike", "roaming"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables IKEv2 MOBIKE for seamless network transitions. Requires VPN server support for MOBIKE (Windows Server 2016+ RRAS does support it).",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "EnableIkev2Mobility", 1)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "EnableIkev2Mobility")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "EnableIkev2Mobility", 1)],
            },
            new TweakDef
            {
                Id = "rnas-disable-password-caching",
                Label = "Remote Access: Disable VPN Credential Caching",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets DisablePasswordCaching=1 in Remote Access policy. Prevents Windows from caching VPN usernames and passwords in the Windows Credential Manager. Cached VPN credentials are stored in the LSA credential store and can be extracted by credential dumping tools (Mimikatz, Windows Credential Editor) running as SYSTEM. An attacker who compromises a device should not be able to harvest VPN credentials for lateral movement. Disabling caching forces VPN re-authentication on each session.",
                Tags = ["remote-access", "credential-cache", "credential", "mimikatz", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Users must enter VPN credentials at each connection. Does not affect certificate-based or SSO (SAML/OIDC) VPN authentication where no password is stored.",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "DisablePasswordCaching", 1)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "DisablePasswordCaching")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "DisablePasswordCaching", 1)],
            },
            new TweakDef
            {
                Id = "rnas-enable-rras-accounting",
                Label = "Remote Access: Enable RRAS RADIUS Accounting for Sessions",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets EnableAccounting=1 in Remote Access policy. Enables RADIUS accounting messages (Start, Stop, Interim) from Windows RRAS to a configured RADIUS server. RADIUS accounting provides a complete audit trail of VPN session duration, bytes transferred, client IP address, and authentication method for each VPN connection. This data is required for network access compliance reporting, ISP billing reconciliation, and post-incident forensic analysis of data volume transferred over VPN.",
                Tags = ["remote-access", "radius", "accounting", "rras", "compliance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Requires a RADIUS server configured in Windows RRAS. If no RADIUS server is configured, this setting has no effect.",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "EnableAccounting", 1)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "EnableAccounting")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "EnableAccounting", 1)],
            },
            new TweakDef
            {
                Id = "rnas-limit-concurrent-connections",
                Label = "Remote Access: Limit Concurrent Remote Access Connections to 100",
                Category = "Remote Network Access Policy",
                Description =
                    "Sets MaxConcurrentConnections=100 in Remote Access policy. Sets a configured maximum for simultaneous VPN/remote access connections on the server. Without a limit, RRAS servers can be overwhelmed by connection floods (either from legitimate growth or from denial-of-service attacks). Setting 100 as the limit on a non-production or branch RRAS server prevents resource exhaustion. Adjust the value based on server capacity (a typical Windows Server 2022 VM supports 200–500 concurrent VPN sessions).",
                Tags = ["remote-access", "concurrent", "limit", "dos", "capacity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "VPN connections above the configured limit are rejected. Adjust value based on server CPU/RAM capacity and expected peak concurrent user count.",
                ApplyOps = [RegOp.SetDword(RasMgrKey, "MaxConcurrentConnections", 100)],
                RemoveOps = [RegOp.DeleteValue(RasMgrKey, "MaxConcurrentConnections")],
                DetectOps = [RegOp.CheckDword(RasMgrKey, "MaxConcurrentConnections", 100)],
            },
        ];
}
