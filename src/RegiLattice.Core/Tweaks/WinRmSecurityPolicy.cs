// RegiLattice.Core — Tweaks/WinRmSecurityPolicy.cs
// WinRM / WSMAN remote management endpoint security, authentication, and transport policy — Sprint 519.
// Category: "WinRM Security Policy" | Slug: winrmadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WinRmSecurityPolicy
{
    private const string SvcKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";
    private const string CliKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
    private const string WsmKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "winrmadv-disable-basic-auth-service",
            Label        = "Disable WinRM Basic Authentication on Service (Listener)",
            Category     = "WinRM Security Policy",
            Description  = "Prevents the WinRM service from accepting Basic authentication credentials, which transmit usernames and passwords in Base64 without encryption. Forces use of Kerberos or CredSSP authenticated sessions.",
            Tags         = ["winrm", "basic-auth", "authentication", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "WinRM Basic authentication disabled on service; Kerberos or CredSSP required for remote management.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "AllowBasic", 0)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "AllowBasic")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id           = "winrmadv-disable-basic-auth-client",
            Label        = "Disable WinRM Basic Authentication on Client",
            Category     = "WinRM Security Policy",
            Description  = "Prevents the WinRM client from sending Basic authentication credentials to remote hosts, ensuring WinRM connections from this machine always use authenticated and encrypted transport.",
            Tags         = ["winrm", "basic-auth", "client", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "WinRM client Basic authentication disabled; remote management connections require Kerberos/CredSSP.",
            ApplyOps     = [RegOp.SetDword(CliKey, "AllowBasic", 0)],
            RemoveOps    = [RegOp.DeleteValue(CliKey, "AllowBasic")],
            DetectOps    = [RegOp.CheckDword(CliKey, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id           = "winrmadv-require-https-transport",
            Label        = "Require HTTPS Transport for All WinRM Connections",
            Category     = "WinRM Security Policy",
            Description  = "Configures WinRM to only accept management sessions over HTTPS (port 5986), blocking unencrypted HTTP WinRM connections (port 5985) that transmit management traffic in plaintext.",
            Tags         = ["winrm", "https", "transport", "encryption", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 5,
            SafetyRating = 5,
            ImpactNote   = "WinRM HTTP (5985) blocked; only HTTPS (5986) remote management sessions accepted.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "AllowUnencrypted", 0)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "AllowUnencrypted")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "AllowUnencrypted", 0)],
        },
        new TweakDef
        {
            Id           = "winrmadv-disable-client-digest-auth",
            Label        = "Disable WinRM Client Digest Authentication",
            Category     = "WinRM Security Policy",
            Description  = "Prevents the WinRM client from using Digest authentication to remote hosts, as Digest sends password hashes that are susceptible to pass-the-hash attacks in non-Kerberos environments.",
            Tags         = ["winrm", "digest-auth", "client", "pass-the-hash", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "WinRM Digest authentication disabled on client; hash-based auth not sent to remote endpoints.",
            ApplyOps     = [RegOp.SetDword(CliKey, "AllowDigest", 0)],
            RemoveOps    = [RegOp.DeleteValue(CliKey, "AllowDigest")],
            DetectOps    = [RegOp.CheckDword(CliKey, "AllowDigest", 0)],
        },
        new TweakDef
        {
            Id           = "winrmadv-set-idle-timeout",
            Label        = "Set WinRM Session Idle Timeout to 7200 Seconds (2 Hours)",
            Category     = "WinRM Security Policy",
            Description  = "Sets the WinRM service idle session timeout to 7200 seconds, ensuring that management sessions that have been idle for more than 2 hours are automatically terminated, preventing stale privileged sessions.",
            Tags         = ["winrm", "session-timeout", "idle", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WinRM idle timeout set to 2 hours; stale privileged remote management sessions auto-terminated.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "IdleTimeoutms", 7200000)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "IdleTimeoutms")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "IdleTimeoutms", 7200000)],
        },
        new TweakDef
        {
            Id           = "winrmadv-set-max-connections",
            Label        = "Limit Maximum Concurrent WinRM Management Connections",
            Category     = "WinRM Security Policy",
            Description  = "Sets the maximum number of concurrent WinRM management sessions to 25, preventing resource exhaustion from session flooding attacks against the WinRM listener.",
            Tags         = ["winrm", "max-connections", "dos-prevention", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "WinRM concurrent session limit set to 25; session flooding against management endpoint prevented.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "MaxConnections", 25)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "MaxConnections")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "MaxConnections", 25)],
        },
        new TweakDef
        {
            Id           = "winrmadv-restrict-trusted-hosts-empty",
            Label        = "Clear WinRM Trusted Hosts (Require Kerberos Domain Auth)",
            Category     = "WinRM Security Policy",
            Description  = "Clears the WinRM TrustedHosts list, preventing workgroup/NTLM authentication to arbitrary hosts and requiring all WinRM connections to use Kerberos domain authentication.",
            Tags         = ["winrm", "trusted-hosts", "kerberos", "ntlm", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "TrustedHosts cleared; WinRM connections require Kerberos domain auth. Workgroup NTLM auth disabled.",
            ApplyOps     = [RegOp.SetString(CliKey, "TrustedHosts", "")],
            RemoveOps    = [RegOp.DeleteValue(CliKey, "TrustedHosts")],
            DetectOps    = [RegOp.CheckString(CliKey, "TrustedHosts", "")],
        },
        new TweakDef
        {
            Id           = "winrmadv-disable-credssp-service",
            Label        = "Disable CredSSP Authentication on WinRM Service",
            Category     = "WinRM Security Policy",
            Description  = "Prevents the WinRM service from accepting CredSSP (Credential Security Support Provider) authentication, which delegates full NTLM/Kerberos credentials to the remote host and is the credential delegation method most exploited in pass-the-credential attacks.",
            Tags         = ["winrm", "credssp", "credential-delegation", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "CredSSP authentication on WinRM service disabled; credential delegation to remote hosts blocked.",
            ApplyOps     = [RegOp.SetDword(SvcKey, "AllowCredSSP", 0)],
            RemoveOps    = [RegOp.DeleteValue(SvcKey, "AllowCredSSP")],
            DetectOps    = [RegOp.CheckDword(SvcKey, "AllowCredSSP", 0)],
        },
        new TweakDef
        {
            Id           = "winrmadv-disable-winrm-telemetry",
            Label        = "Disable WinRM / WSMAN Telemetry to Microsoft",
            Category     = "WinRM Security Policy",
            Description  = "Prevents WinRM / WSMAN from sending authentication event, session usage, and protocol negotiation telemetry to Microsoft.",
            Tags         = ["winrm", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "WinRM telemetry to Microsoft disabled; remote management session data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(WsmKey, "DisableTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(WsmKey, "DisableTelemetry")],
            DetectOps    = [RegOp.CheckDword(WsmKey, "DisableTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "winrmadv-log-authentication-failures",
            Label        = "Log WinRM Authentication Failure Events in Security Log",
            Category     = "WinRM Security Policy",
            Description  = "Enables Security event log entries for all failed WinRM authentication attempts, providing visibility into brute-force and credential stuffing attacks against the remote management endpoint.",
            Tags         = ["winrm", "auth-failure", "event-log", "security-audit", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "WinRM auth failure events logged in Security log; brute-force attempts visible for SIEM alerting.",
            ApplyOps     = [RegOp.SetDword(WsmKey, "LogAuthFailures", 1)],
            RemoveOps    = [RegOp.DeleteValue(WsmKey, "LogAuthFailures")],
            DetectOps    = [RegOp.CheckDword(WsmKey, "LogAuthFailures", 1)],
        },
    ];
}
