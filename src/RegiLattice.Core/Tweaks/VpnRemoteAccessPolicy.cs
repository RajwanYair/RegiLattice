// RegiLattice.Core — Tweaks/VpnRemoteAccessPolicy.cs
// VPN and Remote Access Service (RRAS) security and connection policy — Sprint 636.
// Category: "VPN Remote Access Policy" | Slug: vpnras
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VpnRemoteAccessPolicy
{
    private const string RasKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess";
    private const string IkeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess\IKEv2";
    private const string ConnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteAccess\Config";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "vpnras-require-strong-encryption",
            Label        = "Require Strong Encryption for VPN Connections",
            Category     = "VPN Remote Access Policy",
            Description  = "Enforces maximum-strength encryption (MPPE 128-bit or AES-256) for all RRAS VPN connections. Rejects connections that negotiate weaker ciphers. Default: optional encryption.",
            Tags         = ["vpn", "encryption", "rras", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 4,
            ImpactNote   = "VPN connections must use strong encryption; clients with weak cipher support will fail to connect.",
            ApplyOps     = [RegOp.SetDword(RasKey, "RequireStrongEncryption", 1)],
            RemoveOps    = [RegOp.DeleteValue(RasKey, "RequireStrongEncryption")],
            DetectOps    = [RegOp.CheckDword(RasKey, "RequireStrongEncryption", 1)],
        },
        new TweakDef
        {
            Id           = "vpnras-disable-pptp-protocol",
            Label        = "Disable PPTP VPN Protocol",
            Category     = "VPN Remote Access Policy",
            Description  = "Disables the insecure PPTP (Point-to-Point Tunneling Protocol) for VPN connections. PPTP is considered cryptographically broken. Default: enabled.",
            Tags         = ["vpn", "pptp", "security", "deprecated", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 3,
            ImpactNote   = "PPTP connections blocked; legacy clients relying on PPTP must migrate to IKEv2/L2TP.",
            ApplyOps     = [RegOp.SetDword(RasKey, "DisablePPTP", 1)],
            RemoveOps    = [RegOp.DeleteValue(RasKey, "DisablePPTP")],
            DetectOps    = [RegOp.CheckDword(RasKey, "DisablePPTP", 1)],
        },
        new TweakDef
        {
            Id           = "vpnras-enable-ikev2-preferred",
            Label        = "Set IKEv2 as Preferred VPN Protocol",
            Category     = "VPN Remote Access Policy",
            Description  = "Configures RRAS to prefer IKEv2 (Internet Key Exchange v2) for VPN tunnel negotiation. IKEv2 supports MOBIKE for seamless roaming. Default: automatic protocol selection.",
            Tags         = ["vpn", "ikev2", "protocol", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 4,
            ImpactNote   = "IKEv2 preferred for new connections; fallback to L2TP/SSTP if IKEv2 unavailable.",
            ApplyOps     = [RegOp.SetDword(IkeKey, "PreferIKEv2", 1)],
            RemoveOps    = [RegOp.DeleteValue(IkeKey, "PreferIKEv2")],
            DetectOps    = [RegOp.CheckDword(IkeKey, "PreferIKEv2", 1)],
        },
        new TweakDef
        {
            Id           = "vpnras-set-idle-timeout",
            Label        = "Set VPN Idle Disconnect Timeout to 30 Minutes",
            Category     = "VPN Remote Access Policy",
            Description  = "Automatically disconnects inactive VPN sessions after 30 minutes of idle time. Frees up VPN server resources. Default: no idle timeout.",
            Tags         = ["vpn", "idle", "timeout", "rras", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Idle VPN sessions dropped after 30 minutes; users reconnect on next activity.",
            ApplyOps     = [RegOp.SetDword(ConnKey, "IdleDisconnectTimeout", 30)],
            RemoveOps    = [RegOp.DeleteValue(ConnKey, "IdleDisconnectTimeout")],
            DetectOps    = [RegOp.CheckDword(ConnKey, "IdleDisconnectTimeout", 30)],
        },
        new TweakDef
        {
            Id           = "vpnras-set-max-sessions",
            Label        = "Set Maximum Concurrent VPN Sessions to 100",
            Category     = "VPN Remote Access Policy",
            Description  = "Limits the maximum number of concurrent VPN connections to the RRAS server to 100. Prevents resource exhaustion from excessive connections. Default: unlimited.",
            Tags         = ["vpn", "sessions", "limit", "rras", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 3,
            ImpactNote   = "Connection limit prevents server overload; users beyond 100 are queued or rejected.",
            ApplyOps     = [RegOp.SetDword(ConnKey, "MaxSessions", 100)],
            RemoveOps    = [RegOp.DeleteValue(ConnKey, "MaxSessions")],
            DetectOps    = [RegOp.CheckDword(ConnKey, "MaxSessions", 100)],
        },
        new TweakDef
        {
            Id           = "vpnras-enable-connection-logging",
            Label        = "Enable VPN Connection Audit Logging",
            Category     = "VPN Remote Access Policy",
            Description  = "Enables audit logging for all VPN connection attempts (successful and failed). Logs are written to the Windows Security event log. Default: disabled.",
            Tags         = ["vpn", "logging", "audit", "security", "rras", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "All VPN connection events logged for audit; slight increase in event log volume.",
            ApplyOps     = [RegOp.SetDword(RasKey, "EnableConnectionLogging", 1)],
            RemoveOps    = [RegOp.DeleteValue(RasKey, "EnableConnectionLogging")],
            DetectOps    = [RegOp.CheckDword(RasKey, "EnableConnectionLogging", 1)],
        },
        new TweakDef
        {
            Id           = "vpnras-disable-split-tunneling",
            Label        = "Disable VPN Split Tunneling",
            Category     = "VPN Remote Access Policy",
            Description  = "Forces all client traffic through the VPN tunnel (full-tunnel mode). Prevents clients from accessing the internet directly while connected. Default: split tunneling allowed.",
            Tags         = ["vpn", "split-tunnel", "security", "rras", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 3,
            ImpactNote   = "All traffic routed through VPN; increases VPN server bandwidth but eliminates bypass risk.",
            ApplyOps     = [RegOp.SetDword(RasKey, "DisableSplitTunnel", 1)],
            RemoveOps    = [RegOp.DeleteValue(RasKey, "DisableSplitTunnel")],
            DetectOps    = [RegOp.CheckDword(RasKey, "DisableSplitTunnel", 1)],
        },
        new TweakDef
        {
            Id           = "vpnras-set-sa-lifetime",
            Label        = "Set IKEv2 SA Lifetime to 8 Hours",
            Category     = "VPN Remote Access Policy",
            Description  = "Sets the IKEv2 security association (SA) lifetime to 8 hours (480 minutes). After expiry, the tunnel renegotiates keys. Default: 8 hours (may vary).",
            Tags         = ["vpn", "ikev2", "sa-lifetime", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "VPN tunnel renegotiates keys every 8 hours; brief reconnection on rekey.",
            ApplyOps     = [RegOp.SetDword(IkeKey, "SALifeTimeMinutes", 480)],
            RemoveOps    = [RegOp.DeleteValue(IkeKey, "SALifeTimeMinutes")],
            DetectOps    = [RegOp.CheckDword(IkeKey, "SALifeTimeMinutes", 480)],
        },
        new TweakDef
        {
            Id           = "vpnras-enable-nap-enforcement",
            Label        = "Enable Network Access Protection for VPN",
            Category     = "VPN Remote Access Policy",
            Description  = "Enables NAP (Network Access Protection) health checks for VPN clients. Clients must meet health requirements (AV, firewall, updates) before being granted full access. Default: no NAP enforcement.",
            Tags         = ["vpn", "nap", "health-check", "security", "compliance", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 3,
            ImpactNote   = "VPN clients undergo health validation; non-compliant devices get restricted access.",
            ApplyOps     = [RegOp.SetDword(RasKey, "EnableNAP", 1)],
            RemoveOps    = [RegOp.DeleteValue(RasKey, "EnableNAP")],
            DetectOps    = [RegOp.CheckDword(RasKey, "EnableNAP", 1)],
        },
        new TweakDef
        {
            Id           = "vpnras-disable-saved-credentials",
            Label        = "Prevent Saving VPN Credentials",
            Category     = "VPN Remote Access Policy",
            Description  = "Prevents users from saving VPN connection credentials. Users must enter credentials each time they connect. Reduces credential theft risk. Default: saving allowed.",
            Tags         = ["vpn", "credentials", "security", "credential-theft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 4,
            ImpactNote   = "Users re-enter VPN credentials each session; reduces risk of stored credential theft.",
            ApplyOps     = [RegOp.SetDword(RasKey, "DisableSavedCredentials", 1)],
            RemoveOps    = [RegOp.DeleteValue(RasKey, "DisableSavedCredentials")],
            DetectOps    = [RegOp.CheckDword(RasKey, "DisableSavedCredentials", 1)],
        },
    ];
}
