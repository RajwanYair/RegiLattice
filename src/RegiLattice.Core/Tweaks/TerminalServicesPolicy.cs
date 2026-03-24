// RegiLattice.Core — Tweaks/TerminalServicesPolicy.cs
// Remote Desktop Services (RDS / Terminal Services) Group Policy hardening settings.
// Slug: "tspol" — distinct from RemoteDesktop.cs (user-level RDP connection settings)
//                 and RdpClientPolicy.cs (RDP client-side group policies).
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TerminalServicesPolicy
{
    private const string TsPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tspol-require-nla",
            Label = "RDS: Require Network Level Authentication (NLA)",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "nla", "authentication", "security", "group policy", "terminal services"],
            Description =
                "Requires Network Level Authentication (NLA/CredSSP) before establishing an RDP session. "
                + "UserAuthentication = 1. Prevents brute-force attacks against the Windows login screen "
                + "by requiring valid credentials before the remote session is created. "
                + "Default: NLA not enforced. This is the single most impactful RDS security hardening step.",
            ApplyOps = [RegOp.SetDword(TsPol, "UserAuthentication", 1)],
            RemoveOps = [RegOp.SetDword(TsPol, "UserAuthentication", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "UserAuthentication", 1)],
        },
        new TweakDef
        {
            Id = "tspol-set-encryption-high",
            Label = "RDS: Enforce High (128-bit) Encryption Level",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "encryption", "security", "group policy", "terminal services"],
            Description =
                "Forces all RDP/RDS session traffic to use 128-bit RC4 encryption (High level). "
                + "MinEncryptionLevel = 3 (1=Low, 2=Client-Compatible, 3=High, 4=FIPS). "
                + "Prevents session eavesdropping on 40-bit or client-negotiated weaker ciphers. "
                + "Default: client-compatible (2). Recommended: High (3) or FIPS (4) for sensitive data.",
            ApplyOps = [RegOp.SetDword(TsPol, "MinEncryptionLevel", 3)],
            RemoveOps = [RegOp.SetDword(TsPol, "MinEncryptionLevel", 2)],
            DetectOps = [RegOp.CheckDword(TsPol, "MinEncryptionLevel", 3)],
        },
        new TweakDef
        {
            Id = "tspol-session-timeout-active",
            Label = "RDS: Set Active Session Timeout to 4 Hours",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "timeout", "session", "security", "group policy", "terminal services"],
            Description =
                "Limits active Remote Desktop sessions to a maximum of 4 hours (240 minutes). "
                + "MaxConnectionTime = 14400000 ms. "
                + "Prevents orphaned or hijacked sessions from remaining active indefinitely. "
                + "Default: no maximum connection time limit. Recommended for multi-user RDS servers.",
            ApplyOps = [RegOp.SetDword(TsPol, "MaxConnectionTime", 14400000)],
            RemoveOps = [RegOp.SetDword(TsPol, "MaxConnectionTime", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "MaxConnectionTime", 14400000)],
        },
        new TweakDef
        {
            Id = "tspol-session-timeout-idle",
            Label = "RDS: Disconnect Idle Sessions After 15 Minutes",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "idle", "timeout", "security", "group policy", "terminal services"],
            Description =
                "Disconnects Remote Desktop sessions that have been idle for 15 minutes. "
                + "MaxIdleTime = 900000 ms. "
                + "Frees server resources and closes unattended sessions that could be hijacked "
                + "from an unlocked workstation. Default: no idle timeout. CIS benchmark recommended: 15 min.",
            ApplyOps = [RegOp.SetDword(TsPol, "MaxIdleTime", 900000)],
            RemoveOps = [RegOp.SetDword(TsPol, "MaxIdleTime", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "MaxIdleTime", 900000)],
        },
        new TweakDef
        {
            Id = "tspol-session-timeout-disconnect",
            Label = "RDS: Terminate Disconnected Sessions After 1 Hour",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "reconnect", "timeout", "security", "group policy", "terminal services"],
            Description =
                "Ends disconnected RDS sessions after they have been in the disconnected state "
                + "for more than 1 hour. MaxDisconnectionTime = 3600000 ms. "
                + "Reclaims memory and CPU from abandoned sessions and limits re-attach window "
                + "for stolen session tokens. Default: disconnected sessions kept indefinitely.",
            ApplyOps = [RegOp.SetDword(TsPol, "MaxDisconnectionTime", 3600000)],
            RemoveOps = [RegOp.SetDword(TsPol, "MaxDisconnectionTime", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "MaxDisconnectionTime", 3600000)],
        },
        new TweakDef
        {
            Id = "tspol-disable-drive-redirection",
            Label = "RDS: Disable Client Drive Redirection",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "drive redirection", "data loss prevention", "security", "group policy", "terminal services"],
            Description =
                "Prevents RDP clients from mapping local drives into the remote session. "
                + "fDisableCdm = 1. Stops users from copying files between the local machine "
                + "and the RDS server via drive mapping. Key DLP control for preventing data exfiltration "
                + "from terminal servers through redirected drives. Default: drive redirection allowed.",
            ApplyOps = [RegOp.SetDword(TsPol, "fDisableCdm", 1)],
            RemoveOps = [RegOp.SetDword(TsPol, "fDisableCdm", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "fDisableCdm", 1)],
        },
        new TweakDef
        {
            Id = "tspol-disable-clipboard-redirection",
            Label = "RDS: Disable Client Clipboard Redirection",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "clipboard", "data loss prevention", "security", "group policy", "terminal services"],
            Description =
                "Blocks clipboard synchronisation between the RDP client and the remote session. "
                + "fDisableClip = 1. Prevents copy-paste of sensitive data from the RDS server "
                + "to the local machine and vice versa. Critical DLP control. "
                + "Default: clipboard redirection allowed. CIS recommends disabling on shared RDS servers.",
            ApplyOps = [RegOp.SetDword(TsPol, "fDisableClip", 1)],
            RemoveOps = [RegOp.SetDword(TsPol, "fDisableClip", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "tspol-disable-printer-redirection",
            Label = "RDS: Disable Client Printer Redirection",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "printing", "data loss prevention", "security", "group policy", "terminal services"],
            Description =
                "Prevents local printers from appearing inside Remote Desktop sessions. "
                + "fDisableCpm = 1. Stops users from printing sensitive server-side documents to "
                + "local or home printers via the RDP session. "
                + "Default: printer redirection enabled. Recommended for HIPAA/regulated environments.",
            ApplyOps = [RegOp.SetDword(TsPol, "fDisableCpm", 1)],
            RemoveOps = [RegOp.SetDword(TsPol, "fDisableCpm", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "fDisableCpm", 1)],
        },
        new TweakDef
        {
            Id = "tspol-single-session-per-user",
            Label = "RDS: Limit Users to a Single Remote Session",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "session limit", "licensing", "group policy", "terminal services"],
            Description =
                "Restricts each user account to a maximum of one concurrent Remote Desktop session. "
                + "fSingleSessionPerUser = 1. Prevents the same account from being used across "
                + "multiple simultaneous sessions, reducing session hijack risk and managing RDS CAL consumption. "
                + "Default: multiple concurrent sessions per user allowed.",
            ApplyOps = [RegOp.SetDword(TsPol, "fSingleSessionPerUser", 1)],
            RemoveOps = [RegOp.SetDword(TsPol, "fSingleSessionPerUser", 0)],
            DetectOps = [RegOp.CheckDword(TsPol, "fSingleSessionPerUser", 1)],
        },
        new TweakDef
        {
            Id = "tspol-enable-automatic-reconnect",
            Label = "RDS: Enable Session Reconnect on Network Drop",
            Category = "Terminal Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["rdp", "rds", "reconnect", "reliability", "network", "group policy", "terminal services"],
            Description =
                "Allows RDP clients to automatically reconnect to a disconnected session after "
                + "a transient network interruption. "
                + "fDisableAutoReconnect = 0. "
                + "Improves user experience on unstable network links (Wi-Fi, VPN) by resuming the "
                + "session without requiring a full new logon. "
                + "Default: auto-reconnect enabled. This tweak explicitly enforces that policy.",
            ApplyOps = [RegOp.SetDword(TsPol, "fDisableAutoReconnect", 0)],
            RemoveOps = [RegOp.DeleteValue(TsPol, "fDisableAutoReconnect")],
            DetectOps = [RegOp.CheckDword(TsPol, "fDisableAutoReconnect", 0)],
        },
    ];
}
