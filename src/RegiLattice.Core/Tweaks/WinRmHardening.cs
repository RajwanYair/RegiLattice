// RegiLattice.Core — Tweaks/WinRmHardening.cs
// WinRM client and service authentication hardening policies (Sprint 136).
// Slug "winrm" — HKLM\SOFTWARE\Policies\Microsoft\Windows\WinRM paths.
// Complements RemoteDesktop.cs (RDP) with WS-Management protocol hardening.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WinRmHardening
{
    private const string Client = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
    private const string Service = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "winrm-client-no-basic",
            Label = "Disable WinRM Client Basic Authentication",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Prevents WinRM clients from using Basic authentication, which transmits "
                + "credentials in Base64-encoded plaintext over the network. AllowBasic=0.",
            Tags = ["winrm", "authentication", "basic auth", "security", "remote management"],
            RegistryKeys = [Client],
            ApplyOps = [RegOp.SetDword(Client, "AllowBasic", 0)],
            RemoveOps = [RegOp.DeleteValue(Client, "AllowBasic")],
            DetectOps = [RegOp.CheckDword(Client, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id = "winrm-client-no-unencrypted",
            Label = "Disable Unencrypted WinRM Client Traffic",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Forces WinRM client connections to always use encrypted channels. "
                + "Prevents plaintext remote management sessions over the network. "
                + "AllowUnencrypted=0.",
            Tags = ["winrm", "encryption", "plaintext", "security", "remote management"],
            RegistryKeys = [Client],
            ApplyOps = [RegOp.SetDword(Client, "AllowUnencrypted", 0)],
            RemoveOps = [RegOp.DeleteValue(Client, "AllowUnencrypted")],
            DetectOps = [RegOp.CheckDword(Client, "AllowUnencrypted", 0)],
        },
        new TweakDef
        {
            Id = "winrm-client-no-digest",
            Label = "Disable WinRM Client Digest Authentication",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Disables Digest authentication for WinRM clients. Digest is vulnerable "
                + "to offline dictionary and replay attacks. AllowDigest=0.",
            Tags = ["winrm", "digest", "authentication", "security"],
            RegistryKeys = [Client],
            ApplyOps = [RegOp.SetDword(Client, "AllowDigest", 0)],
            RemoveOps = [RegOp.DeleteValue(Client, "AllowDigest")],
            DetectOps = [RegOp.CheckDword(Client, "AllowDigest", 0)],
        },
        new TweakDef
        {
            Id = "winrm-client-no-negotiate",
            Label = "Disable WinRM Client NTLM (Negotiate) Authentication",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Prevents WinRM clients from using NTLM (Negotiate) authentication. "
                + "In Kerberos-capable environments, this forces Kerberos-only remote "
                + "management. AllowNegotiate=0. May break cross-forest management.",
            Tags = ["winrm", "ntlm", "negotiate", "kerberos", "security"],
            RegistryKeys = [Client],
            ApplyOps = [RegOp.SetDword(Client, "AllowNegotiate", 0)],
            RemoveOps = [RegOp.DeleteValue(Client, "AllowNegotiate")],
            DetectOps = [RegOp.CheckDword(Client, "AllowNegotiate", 0)],
        },
        new TweakDef
        {
            Id = "winrm-client-no-credssp",
            Label = "Disable WinRM Client CredSSP Authentication",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Disables CredSSP (Credential Security Support Provider) in WinRM clients. "
                + "CredSSP delegates user credentials to the remote host, enabling "
                + "credential theft if the remote host is compromised. AllowCredSSP=0.",
            Tags = ["winrm", "credssp", "credential delegation", "security"],
            RegistryKeys = [Client],
            ApplyOps = [RegOp.SetDword(Client, "AllowCredSSP", 0)],
            RemoveOps = [RegOp.DeleteValue(Client, "AllowCredSSP")],
            DetectOps = [RegOp.CheckDword(Client, "AllowCredSSP", 0)],
        },
        new TweakDef
        {
            Id = "winrm-service-no-basic",
            Label = "Disable WinRM Service Basic Authentication",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Prevents the WinRM service (listener) from accepting Basic authentication "
                + "requests from remote clients. AllowBasic=0 on the service side.",
            Tags = ["winrm", "service", "basic auth", "security", "listener"],
            RegistryKeys = [Service],
            ApplyOps = [RegOp.SetDword(Service, "AllowBasic", 0)],
            RemoveOps = [RegOp.DeleteValue(Service, "AllowBasic")],
            DetectOps = [RegOp.CheckDword(Service, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id = "winrm-service-no-unencrypted",
            Label = "Disable Unencrypted WinRM Service Traffic",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Forces the WinRM service listener to require encrypted sessions for "
                + "all incoming remote management connections. AllowUnencrypted=0.",
            Tags = ["winrm", "service", "encryption", "security"],
            RegistryKeys = [Service],
            ApplyOps = [RegOp.SetDword(Service, "AllowUnencrypted", 0)],
            RemoveOps = [RegOp.DeleteValue(Service, "AllowUnencrypted")],
            DetectOps = [RegOp.CheckDword(Service, "AllowUnencrypted", 0)],
        },
        new TweakDef
        {
            Id = "winrm-service-no-negotiate",
            Label = "Disable WinRM Service NTLM (Negotiate) Authentication",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Prevents the WinRM service from accepting NTLM-based authentication. "
                + "In a fully Kerberos domain, this enforces Kerberos-only remote access. "
                + "AllowNegotiate=0. May break workgroup/non-domain management.",
            Tags = ["winrm", "service", "ntlm", "kerberos", "security"],
            RegistryKeys = [Service],
            ApplyOps = [RegOp.SetDword(Service, "AllowNegotiate", 0)],
            RemoveOps = [RegOp.DeleteValue(Service, "AllowNegotiate")],
            DetectOps = [RegOp.CheckDword(Service, "AllowNegotiate", 0)],
        },
        new TweakDef
        {
            Id = "winrm-service-disable-runas",
            Label = "Disable WinRM Service RunAs Feature",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Disables the ability for WinRM service plug-ins to run as a different "
                + "user account via the RunAs feature. DisableRunAs=1.",
            Tags = ["winrm", "service", "runas", "privilege", "security"],
            RegistryKeys = [Service],
            ApplyOps = [RegOp.SetDword(Service, "DisableRunAs", 1)],
            RemoveOps = [RegOp.DeleteValue(Service, "DisableRunAs")],
            DetectOps = [RegOp.CheckDword(Service, "DisableRunAs", 1)],
        },
        new TweakDef
        {
            Id = "winrm-service-allow-kerberos",
            Label = "Explicitly Allow Kerberos for WinRM Service",
            Category = "WinRM Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Explicitly enables Kerberos authentication on the WinRM service. "
                + "Combine with disabling Basic/NTLM to enforce Kerberos-only remote "
                + "management. AllowKerberos=1.",
            Tags = ["winrm", "service", "kerberos", "authentication", "security"],
            RegistryKeys = [Service],
            ApplyOps = [RegOp.SetDword(Service, "AllowKerberos", 1)],
            RemoveOps = [RegOp.DeleteValue(Service, "AllowKerberos")],
            DetectOps = [RegOp.CheckDword(Service, "AllowKerberos", 1)],
        },
    ];
}
