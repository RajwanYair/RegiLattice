// RegiLattice.Core — Tweaks/RemotePsJeaPolicy.cs
// Just Enough Administration (JEA) endpoint configuration, WinRM listener hardening,
// session timeout, and constrained endpoint controls — Sprint 451.
// Category: "Remote PS JEA Policy" | Slug: psjea
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RemotePsJeaPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "psjea-disable-winrm-service",
                Label = "Disable WinRM Service Auto-Start via Policy",
                Category = "Remote PS JEA Policy",
                Description =
                    "Disables the WinRM service from starting automatically via Group Policy, preventing incoming PowerShell remoting and WMI-over-WinRM connections unless explicitly activated.",
                Tags = ["winrm", "remoting", "jea", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Incoming WinRM connections blocked; PS remoting sessions cannot be established.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWinRM", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWinRM")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWinRM", 1)],
            },
            new TweakDef
            {
                Id = "psjea-block-unencrypted-winrm",
                Label = "Block Unencrypted WinRM Traffic",
                Category = "Remote PS JEA Policy",
                Description =
                    "Disallows unencrypted WinRM communication, requiring all WinRM traffic to use HTTPS or Kerberos/TLS encryption to protect credentials and data in transit.",
                Tags = ["winrm", "encryption", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Plain-text WinRM rejected; all remote PS connections must use HTTPS or Kerberos.",
                ApplyOps = [RegOp.SetDword(Key, "AllowUnencryptedTraffic", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUnencryptedTraffic")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUnencryptedTraffic", 0)],
            },
            new TweakDef
            {
                Id = "psjea-disable-basic-auth-server",
                Label = "Disable WinRM Basic Authentication (Server Side)",
                Category = "Remote PS JEA Policy",
                Description =
                    "Disables Basic authentication on the WinRM server side, preventing password transmission in clear text (Base64) over WinRM connections.",
                Tags = ["winrm", "basic-auth", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WinRM Basic auth disabled server-side; Kerberos/NTLM/Certificates required.",
                ApplyOps = [RegOp.SetDword(Key, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "psjea-disable-basic-auth-client",
                Label = "Disable WinRM Basic Authentication (Client Side)",
                Category = "Remote PS JEA Policy",
                Description =
                    "Disables Basic authentication for the WinRM client, preventing the client from offering or accepting Basic auth credentials when connecting to remote endpoints.",
                Tags = ["winrm", "basic-auth", "client", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WinRM client will not use Basic auth for connections.",
                ApplyOps = [RegOp.SetDword(Key2, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(Key2, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(Key2, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "psjea-disable-digest-auth",
                Label = "Disable WinRM Digest Authentication",
                Category = "Remote PS JEA Policy",
                Description =
                    "Disables the Digest authentication scheme on the WinRM client, preventing weak credential hashing schemes from being used in remote management connections.",
                Tags = ["winrm", "digest-auth", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Digest auth blocked; modern Kerberos or certificate auth required.",
                ApplyOps = [RegOp.SetDword(Key2, "AllowDigest", 0)],
                RemoveOps = [RegOp.DeleteValue(Key2, "AllowDigest")],
                DetectOps = [RegOp.CheckDword(Key2, "AllowDigest", 0)],
            },
            new TweakDef
            {
                Id = "psjea-require-kerberos",
                Label = "Require Kerberos for WinRM Authentication",
                Category = "Remote PS JEA Policy",
                Description =
                    "Configures the WinRM client to require Kerberos-based authentication for remote management connections, ensuring only domain-authenticated sessions are established.",
                Tags = ["winrm", "kerberos", "authentication", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Kerberos only for WinRM; workgroup machines cannot use PS remoting.",
                ApplyOps = [RegOp.SetDword(Key2, "AllowKerberos", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "AllowKerberos")],
                DetectOps = [RegOp.CheckDword(Key2, "AllowKerberos", 1)],
            },
            new TweakDef
            {
                Id = "psjea-set-idle-timeout",
                Label = "Set WinRM Session Idle Timeout to 900 Seconds",
                Category = "Remote PS JEA Policy",
                Description =
                    "Sets the WinRM service idle timeout to 900 seconds (15 minutes) to automatically terminate abandoned PowerShell remoting sessions, reducing attack window for session hijacking.",
                Tags = ["winrm", "timeout", "jea", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Idle remote sessions disconnected after 15 minutes; long-running automation scripts should keep alive.",
                ApplyOps = [RegOp.SetDword(Key, "IdleTimeoutms", 900000)],
                RemoveOps = [RegOp.DeleteValue(Key, "IdleTimeoutms")],
                DetectOps = [RegOp.CheckDword(Key, "IdleTimeoutms", 900000)],
            },
            new TweakDef
            {
                Id = "psjea-disable-runasinteractive",
                Label = "Disable RunAs Interactive in WinRM Sessions",
                Category = "Remote PS JEA Policy",
                Description =
                    "Prevents users on WinRM sessions from using RunAs to elevate to interactive logon tokens, limiting privilege escalation paths within remote management sessions.",
                Tags = ["winrm", "runas", "jea", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Interactive RunAs blocked in remote sessions; all JEA sessions operate under delegated role accounts.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRunAsInteractive", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRunAsInteractive")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRunAsInteractive", 1)],
            },
            new TweakDef
            {
                Id = "psjea-block-client-unencrypted",
                Label = "Block Unencrypted WinRM on Client Side",
                Category = "Remote PS JEA Policy",
                Description =
                    "Disallows the WinRM client from sending or accepting unencrypted messages, ensuring all outgoing remote management traffic is protected.",
                Tags = ["winrm", "encryption", "client", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "WinRM client refuses unencrypted connections; HTTPS listeners required.",
                ApplyOps = [RegOp.SetDword(Key2, "AllowUnencryptedTraffic", 0)],
                RemoveOps = [RegOp.DeleteValue(Key2, "AllowUnencryptedTraffic")],
                DetectOps = [RegOp.CheckDword(Key2, "AllowUnencryptedTraffic", 0)],
            },
            new TweakDef
            {
                Id = "psjea-disable-credssp",
                Label = "Disable CredSSP Authentication for WinRM",
                Category = "Remote PS JEA Policy",
                Description =
                    "Disables CredSSP (Credential Security Support Provider) in WinRM, preventing credential delegation attacks where full network credentials are passed through to remote hosts.",
                Tags = ["winrm", "credssp", "delegation", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "CredSSP blocked; no double-hop credential delegation via PS remoting. Use Kerberos constrained delegation instead.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCredSSP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCredSSP")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCredSSP", 1)],
            },
        ];
}
