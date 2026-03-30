// RegiLattice.Core — Tweaks/ActiveDirectoryServicesPolicy.cs
// Active Directory Domain Services Client Policy — Sprint 579.
// Configures AD DS client behaviors: netlogon signing, secure channel,
// machine account lockout, NTLM restriction, and domain controller
// locator hardening.
// Category: "Active Directory Services Policy" | Slug: addsvc
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System
//           HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ActiveDirectoryServicesPolicy
{
    private const string AdPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    private const string NetlogonKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "addsvc-require-strong-dc-channel",
                Label = "AD Services: Require Sign-and-Seal (Strong) Secure Channel to DC",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets RequireSignOrSeal=1 in Netlogon\\Parameters. Requires all Netlogon secure channel traffic between this workstation and domain controllers to be both signed and sealed (encrypted). The Netlogon secure channel carries authentication traffic, machine account password changes, and group policy downloads. If unsigned and unencrypted, the secure channel is susceptible to the Zerologon vulnerability (CVE-2020-1472) and earlier Netlogon protocol attacks that allow privilege escalation to Domain Admin. Requiring sign-and-seal ensures all Netlogon traffic is integrity-protected and encrypted.",
                Tags = ["netlogon", "secure-channel", "zerologon", "cve-2020-1472", "sign-seal"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Netlogon secure channel requires sign-and-seal. Mitigates Zerologon and Netlogon protocol downgrade attacks. No user-visible impact — all modern Windows DCs support sign-and-seal. May block authentication if older (pre-Windows 2000) DCs exist in the domain.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "RequireSignOrSeal", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "RequireSignOrSeal")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "RequireSignOrSeal", 1)],
            },
            new TweakDef
            {
                Id = "addsvc-enable-secure-channel-sealing",
                Label = "AD Services: Enable Netlogon Secure Channel Encryption (Seal)",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets SealSecureChannel=1 in Netlogon\\Parameters. Enables encryption (sealing) of the Netlogon secure channel in addition to signing. While RequireSignOrSeal=1 ensures integrity, this setting specifically ensures confidentiality — the channel content is encrypted and cannot be captured by network eavesdropping. Together, signing and sealing provide authenticated-and-encrypted communication between clients and domain controllers for all Netlogon protocol messages, including machine account password refresh operations.",
                Tags = ["netlogon", "seal", "encryption", "secure-channel", "confidentiality"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Netlogon channel encryption enabled. Works in conjunction with signing (RequireSignOrSeal). No visible impact to users or applications — encryption is handled transparently by the Netlogon service. Requires DCs to support secure channel encryption, which all Windows Server 2000+ DCs support.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "SealSecureChannel", 1)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "SealSecureChannel")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "SealSecureChannel", 1)],
            },
            new TweakDef
            {
                Id = "addsvc-set-machine-password-age-30days",
                Label = "AD Services: Set Machine Account Password Rotation Interval to 30 Days",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets MaximumPasswordAge=30 in Netlogon\\Parameters (units: days). Sets the maximum age of the machine account password to 30 days, after which Netlogon automatically requests a new password from the DC. Machine account passwords authenticate the workstation to the domain (used in Netlogon secure channel setup and Kerberos S4U2Proxy). An attacker who compromises a machine account password can perform Pass-the-Hash or Silver Ticket attacks using the machine account's Kerberos hash. Frequent rotation limits the attacker's window of opportunity. Default is 30 days; explicitly setting this prevents GPO drift to longer values.",
                Tags = ["machine-account", "password-rotation", "netlogon", "silver-ticket", "s4u"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Machine account password rotated every 30 days automatically by Netlogon. This is the default behaviour; explicitly setting it prevents accidental policy drift. No user-visible impact. Machine account password changes are transparent. Disable rotation only for specific reasons (domain join cloning scenarios) via DisablePasswordChange.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "MaximumPasswordAge", 30)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "MaximumPasswordAge")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "MaximumPasswordAge", 30)],
            },
            new TweakDef
            {
                Id = "addsvc-enable-machine-account-password-change",
                Label = "AD Services: Enable Automatic Machine Account Password Rotation",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets DisablePasswordChange=0 in Netlogon\\Parameters. Explicitly enables automatic machine account password changes (sets the DisablePasswordChange flag to 0 = do NOT disable). Automatic machine account password rotation is a security feature — some organisations disable it to prevent 'secure channel' credential staleness in certain edge cases (e.g., VDI golden image re-deployment). Disabling rotation means the machine's Active Directory password stays static indefinitely, making it a persistent credential that is more valuable to attackers and never expires. Explicitly enabling rotation is a defence-in-depth measure.",
                Tags = ["machine-account", "password-rotation", "disable-prevention", "netlogon"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "Machine account automatic password change enabled (DisablePasswordChange=0). This is the default Windows behaviour. If your environment uses VDI clones or domain-joined VM templates, ensure the VDI solution handles machine account conflicts via SysPrep or equivalent before enforcing.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "DisablePasswordChange", 0)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "DisablePasswordChange")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "DisablePasswordChange", 0)],
            },
            new TweakDef
            {
                Id = "addsvc-restrict-ntlm-outbound-to-trusted-servers",
                Label = "AD Services: Restrict Outbound NTLM to Domain-Trusted Servers Only",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets RestrictSendingNTLMTraffic=2 in the System policy (value 2 = Deny All, value 1 = Audit, value 0 = Allow). Restricts outbound NTLM authentication to only servers in the trusted exception list. NTLM credentials can be captured by rogue SMB or HTTP servers (e.g., via LLMNR/NBT-NS poisoning with Responder) — any outbound NTLM challenge-response that reaches an attacker's server provides an NTLM hash that can be relayed or cracked. Denying outbound NTLM to non-trusted-listed servers prevents credential leakage via NTLM to attacker-controlled resources. Start with value 1 (Audit) to identify NTLM usage before enforcing value 2.",
                Tags = ["ntlm", "outbound-restriction", "responder", "ntlm-relay", "credential-capture"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "Outbound NTLM is denied for all servers not in the NTLM exception list. This is high-impact — applications and services that use NTLM for server connections (file shares, proxy auth, legacy web apps) will fail unless added to the exception list. Deploy first with value 1 (Audit) and examine NTLM audit events (Event ID 4011/4013) to map usage before enforcing value 2.",
                ApplyOps = [RegOp.SetDword(AdPolicyKey, "RestrictSendingNTLMTraffic", 2)],
                RemoveOps = [RegOp.DeleteValue(AdPolicyKey, "RestrictSendingNTLMTraffic")],
                DetectOps = [RegOp.CheckDword(AdPolicyKey, "RestrictSendingNTLMTraffic", 2)],
            },
            new TweakDef
            {
                Id = "addsvc-enable-ntlm-audit-logging",
                Label = "AD Services: Enable NTLM Outbound Authentication Audit Logging",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets AuditReceivingNTLMTraffic=2 in the System policy (value 2 = Enable auditing for all NTLM authentication). Enables auditing of all outbound NTLM authentication requests from this client. Audited events appear in the Security event log (Event ID 8001/8002/8003) and include the destination server, the NTLM authentication type, and the caller process. This is essential for mapping NTLM usage before deploying NTLM restriction policies — it allows the security team to identify which applications, services, and users are using NTLM so equivalent Kerberos or modern authentication alternatives can be configured before NTLM is denied.",
                Tags = ["ntlm", "audit", "event-log", "siem", "authentication-map"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "All outbound NTLM authentications are logged to the Security event log. Generates events for every NTLM use — in Active Directory environments this may include Windows file share browsing, SMB connections, etc. Integrate with SIEM. Event volume may be high in environments with heavy NTLM usage.",
                ApplyOps = [RegOp.SetDword(AdPolicyKey, "AuditReceivingNTLMTraffic", 2)],
                RemoveOps = [RegOp.DeleteValue(AdPolicyKey, "AuditReceivingNTLMTraffic")],
                DetectOps = [RegOp.CheckDword(AdPolicyKey, "AuditReceivingNTLMTraffic", 2)],
            },
            new TweakDef
            {
                Id = "addsvc-require-ldap-server-integrity",
                Label = "AD Services: Require DC-Side LDAP Server Signing (Integrity Check)",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets LDAPServerIntegrity=2 in the Netlogon\\Parameters hive (value 2 = Require signing). Requires LDAP signing on LDAP connections from clients to domain controllers. This is the server-side complement to the LDAP client signing requirement (LDAPClientIntegrity). When both client and server require signing, LDAP relay attacks that attempt to intercept and modify LDAP traffic are blocked at both endpoints. Without the server-side requirement, an attacker could spoof a DC with an unsigned LDAP server even if client policy sends signed requests.",
                Tags = ["ldap", "server-signing", "ldap-relay", "netlogon", "integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote =
                    "DC-side LDAP signing required as well as client-side. All LDAP connections from this client to DCs must use signed sessions — both sides enforce it. LDAP clients that send unsigned LDAP requests will receive an LDAP_UNWILLING_TO_PERFORM error. Applies to all LDAP on port 389; LDAPS on 636 is unaffected (TLS provides equivalent protection).",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "LDAPServerIntegrity", 2)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "LDAPServerIntegrity")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "LDAPServerIntegrity", 2)],
            },
            new TweakDef
            {
                Id = "addsvc-set-dc-locator-force-rediscovery-600",
                Label = "AD Services: Set DC Locator Force Re-Discovery Period to 600 Seconds",
                Category = "Active Directory Services Policy",
                Description =
                    "Sets ForceRediscoveryInterval=600 in Netlogon\\Parameters (units: seconds). Sets the minimum interval between forced DC re-discovery events to 600 seconds (10 minutes). DC locator caches the preferred domain controller for each domain to avoid repeated DC lookup traffic. If the preferred DC becomes unavailable (patched, restarted, or taken down), the client should re-discover a DC within a reasonable time. Setting 600 seconds ensures that clients do not hold stale DC references for longer than 10 minutes when a DC failure event occurs, reducing authentication outage windows during DC failover events.",
                Tags = ["netlogon", "dc-locator", "failover", "rediscovery", "resilience"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "DC preference re-discovery forced every 600 seconds maximum. Clients will check for a better DC up to every 10 minutes. May generate additional Netlogon DC locator traffic in large environments with many workstations. Appropriate for standard enterprise environments with 2+ DCs per site.",
                ApplyOps = [RegOp.SetDword(NetlogonKey, "ForceRediscoveryInterval", 600)],
                RemoveOps = [RegOp.DeleteValue(NetlogonKey, "ForceRediscoveryInterval")],
                DetectOps = [RegOp.CheckDword(NetlogonKey, "ForceRediscoveryInterval", 600)],
            },
        ];
}
