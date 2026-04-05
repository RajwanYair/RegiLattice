namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// RegiLattice.Core — Tweaks/PolicyWinRM.cs
// Windows Remote Management (WinRM) server and client security hardening Group Policy tweaks.
// Category: "Security"
// Sprint 670 (v6.11.0)

internal static class PolicyWinRM
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _WinRMServicePolicy.Data,
            .. _WinRMClientPolicy.Data,
        ];

    // ── Sprint 670a — WinRM Service (Server Side) Hardening ───────────────────
    private static class _WinRMServicePolicy
    {
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-winrm-disable-basic-auth-svc",
                    Label = "Disable Basic Authentication on WinRM Service",
                    Category = "Security",
                    Description =
                        "Disables Basic authentication on the WinRM server service. Basic auth sends credentials in base64 (effectively plaintext) — blocking it forces Negotiate/Kerberos/HTTPS-only auth. Default: Basic auth allowed. Recommended: disabled (CIS Benchmark L1).",
                    Tags = ["winrm", "remoting", "authentication", "security", "policy", "cis"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-digest-auth",
                    Label = "Disable Digest Authentication on WinRM Service",
                    Category = "Security",
                    Description =
                        "Disables Digest authentication on the WinRM server. Digest auth is weak against offline dictionary and pass-the-hash attacks. Disabling forces more secure protocols (Kerberos, HTTPS + certificate). Default: Digest allowed. Recommended: disabled.",
                    Tags = ["winrm", "digest", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowDigest", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowDigest")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowDigest", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-unencrypted-svc",
                    Label = "Disable Unencrypted WinRM Service Traffic",
                    Category = "Security",
                    Description =
                        "Forces the WinRM service to reject unencrypted remote connections. Without encryption, WinRM session data including command content and output is transmitted in plaintext on the network. Default: unencrypted traffic allowed. Recommended: disabled.",
                    Tags = ["winrm", "encryption", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowUnencryptedTraffic", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowUnencryptedTraffic")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowUnencryptedTraffic", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-auto-config",
                    Label = "Disable WinRM Service Auto-Configuration",
                    Category = "Security",
                    Description =
                        "Prevents the WinRM service from automatically configuring itself at startup. Auto-configuration can silently enable HTTP listeners and create firewall rules without explicit administrator action. Disabling requires explicit manual configuration. Default: auto-config allowed. Recommended: disabled.",
                    Tags = ["winrm", "autoconfig", "listener", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowAutoConfig", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowAutoConfig")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowAutoConfig", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-credssp-svc",
                    Label = "Disable CredSSP Authentication on WinRM Service",
                    Category = "Security",
                    Description =
                        "Disables Credential Security Support Provider (CredSSP) authentication on the WinRM service side. CredSSP delegates full user credentials to the remote server. A compromised remote server gains access to all credentials. Default: CredSSP allowed. Recommended: disabled unless double-hop Kerberos delegation is unavailable.",
                    Tags = ["winrm", "credssp", "delegation", "credentials", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(SvcKey, "DisableRunAs", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "DisableRunAs")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "DisableRunAs", 1)],
                },
            ];
    }

    // ── Sprint 670b — WinRM Client Hardening ──────────────────────────────────
    private static class _WinRMClientPolicy
    {
        private const string CliKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sec-winrm-disable-basic-auth-client",
                    Label = "Disable Basic Authentication on WinRM Client",
                    Category = "Security",
                    Description =
                        "Prevents the WinRM client from using Basic authentication to connect to remote servers. Basic auth passes credentials in base64; a network observer or man-in-the-middle can trivially decode them. Default: Basic auth allowed. Recommended: disabled (CIS Benchmark L1).",
                    Tags = ["winrm", "client", "basic-auth", "security", "policy", "cis"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CliKey, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(CliKey, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-unencrypted-client",
                    Label = "Disable Unencrypted WinRM Client Traffic",
                    Category = "Security",
                    Description =
                        "Prevents the WinRM client from sending unencrypted data to remote servers. Ensures all remote management sessions are encrypted (HTTPS or Kerberos-backed HTTP encryption). Default: unencrypted traffic permitted. Recommended: disabled.",
                    Tags = ["winrm", "client", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CliKey, "AllowUnencryptedTraffic", 0)],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "AllowUnencryptedTraffic")],
                    DetectOps = [RegOp.CheckDword(CliKey, "AllowUnencryptedTraffic", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-digest-client",
                    Label = "Disable Digest Authentication on WinRM Client",
                    Category = "Security",
                    Description =
                        "Prevents the WinRM client from using Digest authentication. Digest is vulnerable to offline password recovery attacks; disabling it forces the client to use Negotiate (Kerberos/NTLM) or certificate-based authentication. Default: Digest allowed. Recommended: disabled.",
                    Tags = ["winrm", "client", "digest", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ApplyOps = [RegOp.SetDword(CliKey, "AllowDigest", 0)],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "AllowDigest")],
                    DetectOps = [RegOp.CheckDword(CliKey, "AllowDigest", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-disable-credssp-client",
                    Label = "Disable CredSSP Authentication on WinRM Client",
                    Category = "Security",
                    Description =
                        "Prevents the WinRM client from using CredSSP authentication. CredSSP forwards the user's full credentials to the remote server, creating a risk that a compromised server can harvest credentials. Default: CredSSP allowed. Recommended: disabled unless explicitly required for double-hop scenarios.",
                    Tags = ["winrm", "client", "credssp", "credentials", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ApplyOps = [RegOp.SetDword(CliKey, "AllowCredSSP", 0)],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "AllowCredSSP")],
                    DetectOps = [RegOp.CheckDword(CliKey, "AllowCredSSP", 0)],
                },
                new TweakDef
                {
                    Id = "sec-winrm-empty-trusted-hosts",
                    Label = "Enforce Empty WinRM Trusted Hosts List",
                    Category = "Security",
                    Description =
                        "Sets WinRM Trusted Hosts to an empty string via Group Policy, preventing the client from connecting to any non-domain remote server without explicit per-connection authentication. An open Trusted Hosts list enables NTLM credential forwarding to non-Kerberos hosts. Default: allows user-defined list. Recommended: empty string.",
                    Tags = ["winrm", "client", "trusted-hosts", "kerberos", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ApplyOps = [RegOp.SetString(CliKey, "TrustedHosts", "")],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "TrustedHosts")],
                    DetectOps = [RegOp.CheckString(CliKey, "TrustedHosts", "")],
                },
            ];
    }
}
