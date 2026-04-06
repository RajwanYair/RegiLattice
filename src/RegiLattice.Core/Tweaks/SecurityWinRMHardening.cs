namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityWinRMHardening
{
    private const string WinRMServiceKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

    private const string WinRMClientKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "winrmsec-block-basic-auth-service",
                Label = "Block WinRM Basic Authentication on Service",
                Category = "Security — WinRM",
                Description =
                    "Disables Basic (plaintext username/password) authentication on the WinRM service. "
                    + "Basic auth transmits credentials in base64 without encryption; disabling it forces stronger auth methods. "
                    + "Default: basic auth may be enabled. Recommended: disabled.",
                Tags = ["winrm", "basic-auth", "plaintext", "credentials", "remote-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMServiceKey, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(WinRMServiceKey, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(WinRMServiceKey, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-credssp-auth-service",
                Label = "Block CredSSP Authentication on WinRM Service",
                Category = "Security — WinRM",
                Description =
                    "Disables CredSSP (Credential Security Support Provider) authentication on the WinRM service. "
                    + "CredSSP sends full credentials to the remote host and is vulnerable to credential harvesting if that host is compromised. "
                    + "Default: may be enabled. Recommended: disabled.",
                Tags = ["winrm", "credssp", "credential-delegation", "remote-management", "pass-the-hash"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMServiceKey, "AllowCredSSP", 0)],
                RemoveOps = [RegOp.DeleteValue(WinRMServiceKey, "AllowCredSSP")],
                DetectOps = [RegOp.CheckDword(WinRMServiceKey, "AllowCredSSP", 0)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-service-runas",
                Label = "Block WinRM Service RunAs Credential Storage",
                Category = "Security — WinRM",
                Description =
                    "Prevents the WinRM service from storing credentials for RunAs operations. "
                    + "Eliminates stored credential material that could be extracted from the WinRM service credential store. "
                    + "Default: RunAs credential storage may be enabled. Recommended: disabled.",
                Tags = ["winrm", "runas", "stored-credentials", "credential-theft", "service"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMServiceKey, "DisableRunAs", 1)],
                RemoveOps = [RegOp.DeleteValue(WinRMServiceKey, "DisableRunAs")],
                DetectOps = [RegOp.CheckDword(WinRMServiceKey, "DisableRunAs", 1)],
            },
            new TweakDef
            {
                Id = "winrmsec-set-idle-timeout",
                Label = "Set Maximum WinRM Session Idle Timeout",
                Category = "Security — WinRM",
                Description =
                    "Limits the maximum time a WinRM session can remain idle before being terminated. "
                    + "1800000ms (30 minutes) ensures abandoned remote sessions are closed, reducing exposure from forgotten open connections. "
                    + "Default: no maximum timeout. Recommended: 1800000 (30 min).",
                Tags = ["winrm", "session-timeout", "idle-session", "session-management", "remote-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMServiceKey, "MaxIdleTimeoutMs", 1800000)],
                RemoveOps = [RegOp.DeleteValue(WinRMServiceKey, "MaxIdleTimeoutMs")],
                DetectOps = [RegOp.CheckDword(WinRMServiceKey, "MaxIdleTimeoutMs", 1800000)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-basic-auth-client",
                Label = "Block WinRM Basic Authentication on Client",
                Category = "Security — WinRM",
                Description =
                    "Disables Basic authentication on the WinRM client, preventing it from connecting to services using plaintext credentials. "
                    + "A compromised client cannot be tricked into sending credentials in cleartext to a rogue WinRM service. "
                    + "Default: basic auth may be available. Recommended: disabled.",
                Tags = ["winrm", "client", "basic-auth", "plaintext", "remote-management"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMClientKey, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(WinRMClientKey, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(WinRMClientKey, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-credssp-auth-client",
                Label = "Block CredSSP Authentication on WinRM Client",
                Category = "Security — WinRM",
                Description =
                    "Disables CredSSP authentication on the WinRM client. "
                    + "Prevents the client from sending full credentials to remote servers, mitigating credential harvesting if connected to a rogue WinRM endpoint. "
                    + "Default: CredSSP may be supported. Recommended: disabled.",
                Tags = ["winrm", "client", "credssp", "credential-delegation", "authentication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMClientKey, "AllowCredSSP", 0)],
                RemoveOps = [RegOp.DeleteValue(WinRMClientKey, "AllowCredSSP")],
                DetectOps = [RegOp.CheckDword(WinRMClientKey, "AllowCredSSP", 0)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-digest-auth-client",
                Label = "Block WinRM Digest Authentication on Client",
                Category = "Security — WinRM",
                Description =
                    "Disables Digest authentication on the WinRM client. Digest auth over HTTP sends an MD5 hash of credentials "
                    + "that is vulnerable to offline cracking. Disabling it forces Kerberos or NTLM. "
                    + "Default: Digest may be available. Recommended: disabled.",
                Tags = ["winrm", "client", "digest-auth", "md5", "credential-cracking"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMClientKey, "AllowDigest", 0)],
                RemoveOps = [RegOp.DeleteValue(WinRMClientKey, "AllowDigest")],
                DetectOps = [RegOp.CheckDword(WinRMClientKey, "AllowDigest", 0)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-kerberos-client",
                Label = "Require Mutual Authentication for WinRM Kerberos Sessions",
                Category = "Security — WinRM",
                Description =
                    "Requires Kerberos mutual authentication (server validates to client BEFORE credentials are sent). "
                    + "Prevents connecting to a rogue WinRM service that presents itself as a known server. "
                    + "AllowKerberos=1 enables Kerberos, but AllowUnencrypted=0 forces encryption. See winrmsec-block-unencrypted.",
                Tags = ["winrm", "client", "kerberos", "mutual-authentication", "rogue-server"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMClientKey, "AllowKerberos", 1)],
                RemoveOps = [RegOp.DeleteValue(WinRMClientKey, "AllowKerberos")],
                DetectOps = [RegOp.CheckDword(WinRMClientKey, "AllowKerberos", 1)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-unencrypted-service",
                Label = "Block Unencrypted WinRM Service Traffic",
                Category = "Security — WinRM",
                Description =
                    "Prevents the WinRM service from accepting or sending plaintext unencrypted transport messages. "
                    + "All WinRM communication must use encrypted transport (HTTPS or NTLM/Kerberos encrypted channels). "
                    + "Default: unencrypted may be allowed. Recommended: disabled.",
                Tags = ["winrm", "encryption", "plaintext", "transport-security", "tls"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMServiceKey, "AllowUnencrypted", 0)],
                RemoveOps = [RegOp.DeleteValue(WinRMServiceKey, "AllowUnencrypted")],
                DetectOps = [RegOp.CheckDword(WinRMServiceKey, "AllowUnencrypted", 0)],
            },
            new TweakDef
            {
                Id = "winrmsec-block-unencrypted-client",
                Label = "Block Unencrypted WinRM Client Traffic",
                Category = "Security — WinRM",
                Description =
                    "Prevents the WinRM client from sending plaintext unencrypted messages to WinRM services. "
                    + "Even with AllowBasic=0, if unencrypted transport is allowed the session can be intercepted. "
                    + "Default: unencrypted may be allowed. Recommended: disabled.",
                Tags = ["winrm", "client", "encryption", "plaintext", "transport-security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(WinRMClientKey, "AllowUnencrypted", 0)],
                RemoveOps = [RegOp.DeleteValue(WinRMClientKey, "AllowUnencrypted")],
                DetectOps = [RegOp.CheckDword(WinRMClientKey, "AllowUnencrypted", 0)],
            },
        ];
}
