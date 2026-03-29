// RegiLattice.Core — Tweaks/SmbServerHardeningPolicy.cs
// SMB server hardening, signing enforcement, v1 removal, compression disablement, and audit policy — Sprint 525.
// Category: "SMB Server Hardening Policy" | Slug: smbsvr
// Registry: HKLM\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmbServerHardeningPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters";
    private const string SrvKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";
    private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "smbsvr-require-server-signing",
            Label        = "Require SMB Server Packet Signing on All Connections",
            Category     = "SMB Server Hardening Policy",
            Description  = "Configures the SMB server to require packet signing on all incoming connections, preventing NTLM relay attacks and connection hijacking by clients that do not sign their SMB traffic.",
            Tags         = ["smb", "signing", "server", "ntlm-relay", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "SMB server signing required; unsigned connections rejected. NTLM relay and session hijacking mitigated.",
            ApplyOps     = [RegOp.SetDword(Key, "RequireSecuritySignature", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequireSecuritySignature")],
            DetectOps    = [RegOp.CheckDword(Key, "RequireSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id           = "smbsvr-require-client-signing",
            Label        = "Require SMB Client Packet Signing for All Outbound Connections",
            Category     = "SMB Server Hardening Policy",
            Description  = "Configures the SMB client (LanmanWorkstation) to require packet signing on all outbound SMB connections, ensuring this machine never sends unsigned SMB traffic to remote servers.",
            Tags         = ["smb", "signing", "client", "outbound", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "SMB client signing required for outbound connections; session tampering against network shares blocked.",
            ApplyOps     = [RegOp.SetDword(SrvKey, "RequireSecuritySignature", 1)],
            RemoveOps    = [RegOp.DeleteValue(SrvKey, "RequireSecuritySignature")],
            DetectOps    = [RegOp.CheckDword(SrvKey, "RequireSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id           = "smbsvr-disable-smb1-server",
            Label        = "Disable SMBv1 Protocol on Server (Remove EternalBlue Attack Surface)",
            Category     = "SMB Server Hardening Policy",
            Description  = "Completely disables the SMBv1 server protocol, removing the attack surface exploited by EternalBlue (MS17-010), WannaCry, and NotPetya. SMBv2 and SMBv3 are fully supported by all versions of Windows since Vista.",
            Tags         = ["smb", "smb1", "eternalblue", "wannacry", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "SMBv1 server disabled; EternalBlue/WannaCry/NotPetya attack surface eliminated.",
            ApplyOps     = [RegOp.SetDword(Key, "SMB1", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "SMB1")],
            DetectOps    = [RegOp.CheckDword(Key, "SMB1", 0)],
        },
        new TweakDef
        {
            Id           = "smbsvr-disable-smb-compression",
            Label        = "Disable SMBv3 Compression to Prevent SMBleed Attacks",
            Category     = "SMB Server Hardening Policy",
            Description  = "Disables SMB compression on the server, mitigating SMBleed (CVE-2020-1206) and similar compression-path vulnerabilities that can allow unauthenticated reading of uninitialized kernel memory through SMB3 compressed data.",
            Tags         = ["smb", "compression", "smbleed", "cve-2020-1206", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "SMBv3 compression disabled; SMBleed class vulnerabilities mitigated. Minor performance impact on compressed transfers.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableCompression", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableCompression")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableCompression", 1)],
        },
        new TweakDef
        {
            Id           = "smbsvr-enable-smb-encryption",
            Label        = "Enable SMBv3 Encryption for All Shares (Enforce in Transit)",
            Category     = "SMB Server Hardening Policy",
            Description  = "Enables SMBv3 end-to-end encryption for all SMB connections to this server, ensuring file transfer content is AES-encrypted in transit and cannot be captured in plaintext on the network.",
            Tags         = ["smb", "encryption", "aes", "in-transit", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "SMBv3 encryption enforced; file data is AES-encrypted in transit. Requires Windows 8/2012 or later clients.",
            ApplyOps     = [RegOp.SetDword(Key, "EncryptData", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "EncryptData")],
            DetectOps    = [RegOp.CheckDword(Key, "EncryptData", 1)],
        },
        new TweakDef
        {
            Id           = "smbsvr-disable-guest-fallback",
            Label        = "Disable SMB Guest Authentication Fallback",
            Category     = "SMB Server Hardening Policy",
            Description  = "Prevents the SMB client from automatically falling back to anonymous guest authentication when the provided credentials are rejected, stopping silent elevation-of-failure-to-anonymous-access on misconfigured shares.",
            Tags         = ["smb", "guest", "anonymous", "fallback", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "SMB guest auth fallback disabled; authentication failures are hard failures, not silent anonymous access.",
            ApplyOps     = [RegOp.SetDword(SrvKey, "EnableInsecureGuestLogons", 0)],
            RemoveOps    = [RegOp.DeleteValue(SrvKey, "EnableInsecureGuestLogons")],
            DetectOps    = [RegOp.CheckDword(SrvKey, "EnableInsecureGuestLogons", 0)],
        },
        new TweakDef
        {
            Id           = "smbsvr-set-max-connections-512",
            Label        = "Set SMB Server Maximum Concurrent Connections to 512",
            Category     = "SMB Server Hardening Policy",
            Description  = "Sets the SMB server maximum concurrent user sessions to 512, limiting resource exhaustion from session flooding attacks that open thousands of SMB connections without completing authentication.",
            Tags         = ["smb", "max-connections", "dos-prevention", "resource-limit", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "SMB concurrent sessions limited to 512; connection flooding resource exhaustion mitigated.",
            ApplyOps     = [RegOp.SetDword(Key, "MaxMpxCt", 512)],
            RemoveOps    = [RegOp.DeleteValue(Key, "MaxMpxCt")],
            DetectOps    = [RegOp.CheckDword(Key, "MaxMpxCt", 512)],
        },
        new TweakDef
        {
            Id           = "smbsvr-log-auth-failures",
            Label        = "Log SMB Authentication Failure Events in Security Log",
            Category     = "SMB Server Hardening Policy",
            Description  = "Enables Security event log audit entries for failed SMB authentication attempts, providing visibility into brute-force attacks and pass-the-hash attempts against network shares.",
            Tags         = ["smb", "auth-failure", "event-log", "audit", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "SMB auth failure events logged in Security log; brute-force and pass-the-hash attempts visible.",
            ApplyOps     = [RegOp.SetDword(PolKey, "LogAuthFailures", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "LogAuthFailures")],
            DetectOps    = [RegOp.CheckDword(PolKey, "LogAuthFailures", 1)],
        },
        new TweakDef
        {
            Id           = "smbsvr-disable-admin-shares",
            Label        = "Disable Automatic Hidden Administrative Shares (C$, D$, ADMIN$)",
            Category     = "SMB Server Hardening Policy",
            Description  = "Prevents the LanmanServer service from automatically creating hidden administrative shares (C$, D$, ADMIN$) at startup, reducing the attack surface for lateral movement via default administrative share enumeration.",
            Tags         = ["smb", "admin-shares", "lateral-movement", "enumeration", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Hidden admin shares (C$, ADMIN$) disabled; automatic lateral movement share targets removed.",
            ApplyOps     = [RegOp.SetDword(Key, "AutoShareServer", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "AutoShareServer")],
            DetectOps    = [RegOp.CheckDword(Key, "AutoShareServer", 0)],
        },
        new TweakDef
        {
            Id           = "smbsvr-disable-smb-telemetry",
            Label        = "Disable SMB Server Telemetry Reporting to Microsoft",
            Category     = "SMB Server Hardening Policy",
            Description  = "Prevents the SMB server from sending connection statistics, negotiated cipher suites, session rates, and protocol version telemetry to Microsoft.",
            Tags         = ["smb", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "SMB telemetry to Microsoft disabled; session rates and cipher negotiation data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(PolKey, "DisableSMBTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(PolKey, "DisableSMBTelemetry")],
            DetectOps    = [RegOp.CheckDword(PolKey, "DisableSMBTelemetry", 1)],
        },
    ];
}
