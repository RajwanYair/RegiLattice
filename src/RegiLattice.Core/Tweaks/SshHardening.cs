// RegiLattice.Core — Tweaks/SshHardening.cs
// SSH server (OpenSSH for Windows) hardening via sshd_config file modifications.
// Slug: "ssh" — applies only when sshd_config is present (IsApplicable guard).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;
using RegiLattice.Core.Services;

internal static class SshHardening
{
    private const string SshdConfig = @"C:\ProgramData\ssh\sshd_config";

    // Helper: apply a directive in sshd_config (add or replace).
    private static void SetSshdDirective(string directive, string value, bool dryRun)
    {
        if (dryRun)
            return;
        if (!System.IO.File.Exists(SshdConfig))
            return;
        string line = $"{directive} {value}";
        var lines = System.IO.File.ReadAllLines(SshdConfig);
        bool found = false;
        for (int i = 0; i < lines.Length; i++)
        {
            string trimmed = lines[i].TrimStart();
            if (
                trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                && (trimmed.Length == directive.Length || trimmed[directive.Length] == ' ')
            )
            {
                lines[i] = line;
                found = true;
                break;
            }
        }
        if (!found)
        {
            System.Array.Resize(ref lines, lines.Length + 1);
            lines[^1] = line;
        }
        System.IO.File.WriteAllLines(SshdConfig, lines);
    }

    // Helper: remove / comment out a directive.
    private static void RemoveSshdDirective(string directive, bool dryRun)
    {
        if (dryRun || !System.IO.File.Exists(SshdConfig))
            return;
        var lines = System.IO.File.ReadAllLines(SshdConfig);
        for (int i = 0; i < lines.Length; i++)
        {
            string trimmed = lines[i].TrimStart();
            if (
                trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                && (trimmed.Length == directive.Length || trimmed[directive.Length] == ' ')
            )
            {
                lines[i] = "#" + lines[i];
                break;
            }
        }
        System.IO.File.WriteAllLines(SshdConfig, lines);
    }

    // Helper: detect a directive is set to the expected value.
    private static bool DetectSshdDirective(string directive, string expectedValue)
    {
        if (!System.IO.File.Exists(SshdConfig))
            return false;
        foreach (string raw in System.IO.File.ReadAllLines(SshdConfig))
        {
            string trimmed = raw.TrimStart();
            if (trimmed.StartsWith("#", System.StringComparison.Ordinal))
                continue;
            if (
                trimmed.StartsWith(directive, System.StringComparison.OrdinalIgnoreCase)
                && trimmed.Length > directive.Length
                && trimmed[directive.Length] == ' '
            )
            {
                string actual = trimmed[(directive.Length + 1)..].Trim();
                return string.Equals(actual, expectedValue, System.StringComparison.OrdinalIgnoreCase);
            }
        }
        return false;
    }

    private static bool SshdConfigExists() => System.IO.File.Exists(SshdConfig);

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ssh-max-auth-tries-3",
            Label = "Limit SSH Authentication Attempts to 3",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets MaxAuthTries 3 in sshd_config. Limits failed authentication attempts "
                + "per connection to 3 before disconnecting, reducing brute-force window. Default: 6.",
            Tags = ["ssh", "authentication", "brute-force", "security", "hardening"],
            ApplyAction = dry => SetSshdDirective("MaxAuthTries", "3", dry),
            RemoveAction = dry => RemoveSshdDirective("MaxAuthTries", dry),
            DetectAction = () => DetectSshdDirective("MaxAuthTries", "3"),
        },
        new TweakDef
        {
            Id = "ssh-login-grace-time-30",
            Label = "SSH Login Grace Time 30 Seconds",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets LoginGraceTime 30 in sshd_config. The server disconnects if a user "
                + "has not authenticated within 30 seconds, preventing half-open connection exhaustion attacks. Default: 120.",
            Tags = ["ssh", "timeout", "security", "hardening", "dos"],
            ApplyAction = dry => SetSshdDirective("LoginGraceTime", "30", dry),
            RemoveAction = dry => RemoveSshdDirective("LoginGraceTime", dry),
            DetectAction = () => DetectSshdDirective("LoginGraceTime", "30"),
        },
        new TweakDef
        {
            Id = "ssh-permit-empty-passwords-no",
            Label = "Disallow SSH Empty Password Logins",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets PermitEmptyPasswords no in sshd_config. Prevents accounts with blank passwords "
                + "from authenticating via SSH. Default: no (safe), but explicitly enforced here.",
            Tags = ["ssh", "password", "authentication", "security"],
            ApplyAction = dry => SetSshdDirective("PermitEmptyPasswords", "no", dry),
            RemoveAction = dry => RemoveSshdDirective("PermitEmptyPasswords", dry),
            DetectAction = () => DetectSshdDirective("PermitEmptyPasswords", "no"),
        },
        new TweakDef
        {
            Id = "ssh-disable-agent-forwarding",
            Label = "Disable SSH Agent Forwarding",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets AllowAgentForwarding no in sshd_config. Prevents forwarding of the SSH authentication "
                + "agent from remote hosts, which could allow lateral movement if a relay host is compromised. Default: yes.",
            Tags = ["ssh", "agent", "forwarding", "security", "lateral-movement"],
            ApplyAction = dry => SetSshdDirective("AllowAgentForwarding", "no", dry),
            RemoveAction = dry => RemoveSshdDirective("AllowAgentForwarding", dry),
            DetectAction = () => DetectSshdDirective("AllowAgentForwarding", "no"),
        },
        new TweakDef
        {
            Id = "ssh-disable-tcp-forwarding",
            Label = "Disable SSH TCP Forwarding",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets AllowTcpForwarding no in sshd_config. Prevents SSH tunnelling of TCP connections "
                + "through this host, blocking use of SSH as a proxy or pivot point. Default: yes.",
            Tags = ["ssh", "tcp", "tunnel", "forwarding", "security"],
            ApplyAction = dry => SetSshdDirective("AllowTcpForwarding", "no", dry),
            RemoveAction = dry => RemoveSshdDirective("AllowTcpForwarding", dry),
            DetectAction = () => DetectSshdDirective("AllowTcpForwarding", "no"),
        },
        new TweakDef
        {
            Id = "ssh-max-sessions-2",
            Label = "Limit SSH Concurrent Sessions to 2",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = false,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets MaxSessions 2 in sshd_config. Caps multiplexed sessions per connection to 2, "
                + "limiting resource exhaustion attacks. May need increasing for automation workflows. Default: 10.",
            Tags = ["ssh", "sessions", "dos", "security", "resource"],
            ApplyAction = dry => SetSshdDirective("MaxSessions", "2", dry),
            RemoveAction = dry => RemoveSshdDirective("MaxSessions", dry),
            DetectAction = () => DetectSshdDirective("MaxSessions", "2"),
        },
        new TweakDef
        {
            Id = "ssh-strict-modes",
            Label = "Enable SSH StrictModes",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets StrictModes yes in sshd_config. Forces SSH to check file and directory permissions "
                + "before accepting logins. Rejects login if home directory or authorized_keys are world-writable. Default: yes.",
            Tags = ["ssh", "permissions", "strictmodes", "security"],
            ApplyAction = dry => SetSshdDirective("StrictModes", "yes", dry),
            RemoveAction = dry => RemoveSshdDirective("StrictModes", dry),
            DetectAction = () => DetectSshdDirective("StrictModes", "yes"),
        },
        new TweakDef
        {
            Id = "ssh-disable-x11-forwarding",
            Label = "Disable SSH X11 Forwarding",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets X11Forwarding no in sshd_config. X11 forwarding is irrelevant on Windows "
                + "and expands the attack surface by creating X11 proxy connections. Default: no on Windows.",
            Tags = ["ssh", "x11", "forwarding", "security", "attack-surface"],
            ApplyAction = dry => SetSshdDirective("X11Forwarding", "no", dry),
            RemoveAction = dry => RemoveSshdDirective("X11Forwarding", dry),
            DetectAction = () => DetectSshdDirective("X11Forwarding", "no"),
        },
        new TweakDef
        {
            Id = "ssh-set-strong-ciphers",
            Label = "Restrict SSH to Strong Ciphers",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets Ciphers to AES-256 in CTR/GCM modes only in sshd_config. "
                + "Removes weak ciphers (3DES, RC4, AES-128-CBC) from the negotiation list. "
                + "Default: broad cipher list. Recommended: AES-256-GCM and AES-256-CTR only.",
            Tags = ["ssh", "cipher", "encryption", "security", "hardening"],
            ApplyAction = dry => SetSshdDirective("Ciphers", "aes256-gcm@openssh.com,aes256-ctr", dry),
            RemoveAction = dry => RemoveSshdDirective("Ciphers", dry),
            DetectAction = () => DetectSshdDirective("Ciphers", "aes256-gcm@openssh.com,aes256-ctr"),
        },
        new TweakDef
        {
            Id = "ssh-set-strong-macs",
            Label = "Restrict SSH to Strong MAC Algorithms",
            Category = "SSH Configuration",
            KindHint = TweakKind.FileConfig,
            NeedsAdmin = true,
            CorpSafe = true,
            IsApplicable = SshdConfigExists,
            ApplicabilityNote = "OpenSSH Server (sshd) is not installed.",
            Description =
                "Sets MACs to HMAC-SHA2-512 and HMAC-SHA2-256 in sshd_config. "
                + "Removes weak MACs (MD5, SHA1) from negotiation. "
                + "Default: broad MAC list including SHA1. Recommended: SHA-256/SHA-512 only.",
            Tags = ["ssh", "mac", "hmac", "encryption", "security"],
            ApplyAction = dry => SetSshdDirective("MACs", "hmac-sha2-512,hmac-sha2-256", dry),
            RemoveAction = dry => RemoveSshdDirective("MACs", dry),
            DetectAction = () => DetectSshdDirective("MACs", "hmac-sha2-512,hmac-sha2-256"),
        },
    ];
}
