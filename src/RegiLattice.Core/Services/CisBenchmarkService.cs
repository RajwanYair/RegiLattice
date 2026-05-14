// RegiLattice.Core — Services/CisBenchmarkService.cs
// C.2 CIS Benchmark tagging — maps RegiLattice tweak IDs to CIS Windows 11 v3.0 control IDs.
// Reference: CIS Microsoft Windows 11 Enterprise Benchmark v3.0.0 (2024-01-31)

namespace RegiLattice.Core.Services;

using RegiLattice.Core.Models;

/// <summary>
/// Provides CIS Windows 11 Benchmark v3.0 control mappings for RegiLattice tweaks.
/// Each entry maps a <see cref="TweakDef"/> ID to one or more CIS control IDs in the
/// form <c>CIS:X.Y.Z</c>, and the tag <c>cis-l1</c> or <c>cis-l2</c> that applies.
/// </summary>
public static class CisBenchmarkService
{
    /// <summary>
    /// Static registry of tweak ID → CIS control mappings.
    /// Level 1 (L1): mandatory controls for corporate-managed workstations.
    /// Level 2 (L2): high-security environments (includes all L1 controls).
    /// </summary>
    private static readonly IReadOnlyDictionary<string, CisControlMapping> _mappings =
        new Dictionary<string, CisControlMapping>(StringComparer.OrdinalIgnoreCase)
        {
            // ── Account Policies ─────────────────────────────────────────────
            ["harden-no-blank-password-network"] = new("18.2.1", "L1", "Ensure 'Accounts: Limit local account use of blank passwords to console logon only' is set to 'Enabled'"),
            ["harden-no-default-admin-owner"] = new("2.3.1.3", "L1", "Ensure 'Accounts: Administrator account status' is set to 'Disabled'"),
            ["uac-always-on-secure-desktop"] = new("2.3.17.4", "L1", "Ensure 'User Account Control: Switch to the secure desktop when prompting for elevation' is set to 'Enabled'"),

            // ── Audit Policies ───────────────────────────────────────────────
            ["harden-enable-audit-logon-events"] = new("17.5.1", "L1", "Ensure 'Audit Logon' is set to 'Success and Failure'"),

            // ── Network / Protocol Hardening ─────────────────────────────────
            ["harden-disable-smb1"] = new("18.3.3", "L1", "Ensure 'Configure SMB v1 server' is set to 'Disabled'"),
            ["harden-enable-smb-encryption"] = new("18.3.5", "L2", "Ensure 'Enable SMB Encryption' is set to 'Enabled'"),
            ["harden-block-ntlm-outgoing-traffic"] = new("2.3.11.8", "L2", "Ensure 'Network security: Restrict NTLM: Outgoing NTLM traffic to remote servers' is set to 'Deny All'"),
            ["harden-restrict-anonymous-connections"] = new("2.3.11.1", "L1", "Ensure 'Network access: Allow anonymous SID/Name translation' is set to 'Disabled'"),
            ["harden-no-anonymous-in-everyone"] = new("2.3.11.2", "L1", "Ensure 'Network access: Do not allow anonymous enumeration of SAM accounts and shares' is set to 'Enabled'"),
            ["harden-netlogon-require-sign-seal"] = new("2.3.8.3", "L1", "Ensure 'Domain member: Digitally encrypt or sign secure channel data (always)' is set to 'Enabled'"),
            ["harden-netlogon-seal-secure-channel"] = new("2.3.8.2", "L1", "Ensure 'Domain member: Digitally encrypt secure channel data (when possible)' is set to 'Enabled'"),
            ["harden-netlogon-sign-secure-channel"] = new("2.3.8.1", "L1", "Ensure 'Domain member: Digitally sign secure channel data (when possible)' is set to 'Enabled'"),
            ["harden-netlogon-require-strong-key"] = new("2.3.8.4", "L1", "Ensure 'Domain member: Require strong (Windows 2000 or later) session key' is set to 'Enabled'"),

            // ── Exploit Protection / CFG ─────────────────────────────────────
            ["harden-enable-cfg"] = new("18.9.27.1", "L1", "Ensure 'Exploit Protection' settings are set to 'On by default'"),

            // ── Remote Access ────────────────────────────────────────────────
            ["harden-disable-remote-uac-filter"] = new("2.3.17.2", "L1", "Ensure 'User Account Control: Allow UIAccess applications to prompt for elevation without using the secure desktop' is set to 'Disabled'"),
            ["harden-rpc-restrict-remote-clients"] = new("18.9.101.2", "L1", "Ensure 'Enable RPC Endpoint Mapper Client Authentication' is set to 'Enabled'"),
            ["harden-machine-password-max-age"] = new("2.3.8.5", "L1", "Ensure 'Domain member: Maximum machine account password age' is set to '30 or fewer days, but not 0'"),

            // ── Firewall ─────────────────────────────────────────────────────
            ["harden-enable-firewall-all-profiles"] = new("9.1.1", "L1", "Ensure 'Windows Firewall: Domain: Firewall state' is set to 'On'"),

            // ── Screen Lock / Session ────────────────────────────────────────
            ["harden-force-logon-on-unlock"] = new("2.3.7.3", "L1", "Ensure 'Interactive logon: Do not require CTRL+ALT+DEL' is set to 'Disabled'"),

            // ── Encryption ───────────────────────────────────────────────────
            ["enc-disable-rc4"] = new("2.3.11.4", "L1", "Ensure 'Network security: Configure encryption types allowed for Kerberos' — disable RC4"),
        };

    /// <summary>All CIS control mappings, keyed by tweak ID.</summary>
    public static IReadOnlyDictionary<string, CisControlMapping> Mappings => _mappings;

    /// <summary>Returns the CIS control mapping for the given tweak ID, or <c>null</c> if not mapped.</summary>
    public static CisControlMapping? GetMapping(string tweakId) =>
        _mappings.TryGetValue(tweakId, out var m) ? m : null;

    /// <summary>Returns all tweak IDs tagged at CIS Level 1 or higher.</summary>
    public static IReadOnlyList<string> GetCisL1TweakIds() =>
        [.. _mappings.Where(kv => kv.Value.Level is "L1" or "L2").Select(kv => kv.Key)];

    /// <summary>Returns all tweak IDs tagged at CIS Level 2.</summary>
    public static IReadOnlyList<string> GetCisL2TweakIds() =>
        [.. _mappings.Where(kv => kv.Value.Level == "L2").Select(kv => kv.Key)];
}

/// <summary>A single CIS Benchmark control mapping entry.</summary>
/// <param name="ControlId">CIS control number (e.g. <c>18.3.3</c>).</param>
/// <param name="Level">Benchmark level: <c>L1</c> or <c>L2</c>.</param>
/// <param name="Title">Human-readable control title from the CIS Benchmark document.</param>
public sealed record CisControlMapping(string ControlId, string Level, string Title);
