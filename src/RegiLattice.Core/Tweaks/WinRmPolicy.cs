// RegiLattice.Core — Tweaks/WinRmPolicy.cs
// Windows Remote Management (WinRM) GPO controls — Sprint 215.
// Hardens WinRM listener, authentication, and encryption settings.
// Category: "WinRM Policy" | Slug: winrmpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WinRmPolicy
{
    private const string Client =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
    private const string Service =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "winrmpol-disable-basic-auth-client",
                Label = "Disable Basic Authentication on WinRM Client",
                Category = "WinRM Policy",
                Description =
                    "Prevents the WinRM client from using HTTP Basic authentication, which transmits credentials in a reversibly encoded form. Forces use of Kerberos, NTLM, or certificate-based authentication instead. Default: Basic auth allowed. Recommended: 1 to prevent credential interception.",
                Tags = ["winrm", "remoting", "authentication", "basic-auth", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WinRM client refuses Basic authentication; credentials cannot be sent in cleartext over HTTP.",
                ApplyOps = [RegOp.SetDword(Client, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(Client, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "winrmpol-disable-basic-auth-service",
                Label = "Disable Basic Authentication on WinRM Service",
                Category = "WinRM Policy",
                Description =
                    "Prevents the WinRM service (listener) from accepting Basic authentication requests. Complementary to the client-side setting — both must be set to fully eliminate Basic auth from the WinRM channel. Default: Basic auth accepted by service. Recommended: 1.",
                Tags = ["winrm", "remoting", "authentication", "basic-auth", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WinRM service rejects Basic authentication; only Kerberos / NTLM / certificate connections succeed.",
                ApplyOps = [RegOp.SetDword(Service, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(Service, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(Service, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "winrmpol-require-encrypted-traffic-client",
                Label = "Require Encrypted Traffic on WinRM Client",
                Category = "WinRM Policy",
                Description =
                    "Forces the WinRM client to use encrypted transport (HTTPS or Kerberos message-level encryption) for all remote management sessions. Plaintext HTTP-based sessions are refused. Default: unencrypted HTTP sessions permitted. Recommended: 1 on all managed endpoints.",
                Tags = ["winrm", "remoting", "encryption", "https", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "WinRM client only uses encrypted channels; cleartext remote management sessions are blocked.",
                ApplyOps = [RegOp.SetDword(Client, "AllowUnencryptedTraffic", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowUnencryptedTraffic")],
                DetectOps = [RegOp.CheckDword(Client, "AllowUnencryptedTraffic", 0)],
            },
            new TweakDef
            {
                Id = "winrmpol-require-encrypted-traffic-service",
                Label = "Require Encrypted Traffic on WinRM Service",
                Category = "WinRM Policy",
                Description =
                    "Forces the WinRM listener to reject any inbound session that does not use transport-level or message-level encryption. Prevents man-in-the-middle interception of remote management traffic. Default: unencrypted connections accepted. Recommended: 1.",
                Tags = ["winrm", "remoting", "encryption", "https", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "WinRM service rejects non-encrypted inbound connections; remote PowerShell/WMI sessions must use HTTPS or Kerberos.",
                ApplyOps = [RegOp.SetDword(Service, "AllowUnencryptedTraffic", 0)],
                RemoveOps = [RegOp.DeleteValue(Service, "AllowUnencryptedTraffic")],
                DetectOps = [RegOp.CheckDword(Service, "AllowUnencryptedTraffic", 0)],
            },
            new TweakDef
            {
                Id = "winrmpol-disable-digest-auth-client",
                Label = "Disable Digest Authentication on WinRM Client",
                Category = "WinRM Policy",
                Description =
                    "Prevents the WinRM client from offering Digest authentication. Digest auth sends a hash of the credentials that can be cracked offline. Kerberos or certificate auth should be used instead. Default: Digest allowed on client. Recommended: 1.",
                Tags = ["winrm", "remoting", "digest-auth", "authentication", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WinRM client does not offer Digest auth; offline hash-cracking attacks against captured sessions are prevented.",
                ApplyOps = [RegOp.SetDword(Client, "AllowDigest", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowDigest")],
                DetectOps = [RegOp.CheckDword(Client, "AllowDigest", 0)],
            },
            new TweakDef
            {
                Id = "winrmpol-disable-credential-delegation",
                Label = "Disable Credential Delegation in WinRM",
                Category = "WinRM Policy",
                Description =
                    "Prevents WinRM sessions from delegating the user's credentials to the remote machine (CredSSP / AllowCredSSP=0). Credential delegation is the basis of pass-the-credentials attacks — the remote host receives usable Kerberos tickets. Disable unless explicitly required for multi-hop scenarios. Default: CredSSP delegation permitted. Recommended: 1.",
                Tags = ["winrm", "remoting", "credssp", "delegation", "credentials", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "WinRM CredSSP delegation is blocked; multi-hop credential forwarding attacks via remote management are prevented.",
                ApplyOps = [RegOp.SetDword(Client, "AllowCredSSP", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowCredSSP")],
                DetectOps = [RegOp.CheckDword(Client, "AllowCredSSP", 0)],
            },
            new TweakDef
            {
                Id = "winrmpol-disable-credssp-service",
                Label = "Disable CredSSP on WinRM Service",
                Category = "WinRM Policy",
                Description =
                    "Prevents the WinRM service from accepting CredSSP-authenticated inbound connections. Blocking CredSSP at the service prevents the remote endpoint from collecting forwarded credentials even if an attacker manipulates the client configuration. Default: service accepts CredSSP. Recommended: 1.",
                Tags = ["winrm", "remoting", "credssp", "service", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "WinRM service refuses CredSSP logons; remote credential harvesting via CredSSP is blocked.",
                ApplyOps = [RegOp.SetDword(Service, "AllowCredSSP", 0)],
                RemoveOps = [RegOp.DeleteValue(Service, "AllowCredSSP")],
                DetectOps = [RegOp.CheckDword(Service, "AllowCredSSP", 0)],
            },
            new TweakDef
            {
                Id = "winrmpol-restrict-trusted-hosts",
                Label = "Restrict WinRM Trusted Hosts to Empty List",
                Category = "WinRM Policy",
                Description =
                    "Sets the WinRM TrustedHosts list to empty, preventing the client from connecting to non-domain machines using NTLM. Trusted hosts bypass server certificate validation; an empty list forces certificate or Kerberos authentication. Default: TrustedHosts may be set by users. Recommended: 1 (empty list) in domain environments.",
                Tags = ["winrm", "remoting", "trusted-hosts", "ntlm", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "TrustedHosts is locked to empty; WinRM cannot connect to non-domain hosts via NTLM without explicit admin configuration.",
                ApplyOps = [RegOp.SetString(Client, "TrustedHosts", "")],
                RemoveOps = [RegOp.DeleteValue(Client, "TrustedHosts")],
                DetectOps = [RegOp.CheckString(Client, "TrustedHosts", "")],
            },
            new TweakDef
            {
                Id = "winrmpol-disable-winrm-service",
                Label = "Disable WinRM Service Autostart",
                Category = "WinRM Policy",
                Description =
                    "Prevents the Windows Remote Management service from starting automatically. On endpoints that do not require remote management (most workstations), disabling WinRM removes the remote PowerShell attack surface entirely. Default: WinRM may be enabled on domain machines via Group Policy. Recommended: 1 on non-managed or non-admin workstations.",
                Tags = ["winrm", "remoting", "service", "disabled", "attack-surface", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "WinRM service is disabled; remote PowerShell, remote WMI, and DSC push configurations do not work until re-enabled.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM",
                        "Start",
                        4
                    ),
                ],
                RemoveOps =
                [
                    RegOp.SetDword(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM",
                        "Start",
                        3
                    ),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM",
                        "Start",
                        4
                    ),
                ],
            },
            new TweakDef
            {
                Id = "winrmpol-enable-audit-logging",
                Label = "Enable WinRM Session Audit Logging",
                Category = "WinRM Policy",
                Description =
                    "Records successful and failed WinRM authentication attempts, session creation, and session teardown events in the Windows event log (Microsoft-Windows-WinRM/Operational). Provides forensic visibility into remote management activity. Default: WinRM operational log not always enabled. Recommended: 1 on all endpoints.",
                Tags = ["winrm", "remoting", "audit", "logging", "forensics", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "WinRM session events (connect, authenticate, disconnect) are written to the Operational event channel.",
                ApplyOps = [RegOp.SetDword(Service, "EnableVerboseLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Service, "EnableVerboseLogging")],
                DetectOps = [RegOp.CheckDword(Service, "EnableVerboseLogging", 1)],
            },
        ];
}
