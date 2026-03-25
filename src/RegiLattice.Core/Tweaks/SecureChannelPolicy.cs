// RegiLattice.Core — Tweaks/SecureChannelPolicy.cs
// Sprint 340: Secure Channel Policy tweaks (10 tweaks)
// Category: "Secure Channel Policy" | Slug: secchan
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetLogon

namespace RegiLattice.Core.Tweaks;
using RegiLattice.Core.Models;

internal static class SecureChannelPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetLogon";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "secchan-require-secure-channel-signing",
            Label = "Require Signing on Domain Secure Channel Communication",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 5,
            Description = "Domain secure channel signing ensures that NetLogon traffic between domain members and domain controllers is cryptographically signed to prevent replay and man-in-the-middle attacks. Requiring secure channel signing prevents an attacker from intercepting and replaying domain authentication traffic to authenticate as legitimate domain users. Unsigned secure channel traffic can be captured and manipulated by an attacker with network access to forge authentication responses from a domain controller. Secure channel signing is required in addition to sealing to provide both integrity and confidentiality protection for domain authentication. Organizations with older legacy clients that do not support secure channel signing must upgrade those systems before requiring signing across the domain. Microsoft requires secure channel signing in all hardened domain configurations and it is enforced in Windows Server 2022 default hardening baselines.",
            Tags = ["secure-channel", "signing", "netlogon", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireSignOrSeal", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSignOrSeal")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSignOrSeal", 1)],
        },
        new TweakDef
        {
            Id = "secchan-require-secure-channel-sealing",
            Label = "Require Encryption Sealing on Domain Secure Channel",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 5,
            Description = "Secure channel sealing encrypts all NetLogon traffic between domain members and domain controllers protecting authentication data from network eavesdropping. Requiring secure channel sealing prevents credential harvesting from network captures by encrypting all domain authentication communication. Secure channel sealing uses the session key negotiated during the NetLogon authentication to provide symmetric encryption of subsequent traffic. The combination of signing and sealing provides both integrity and confidentiality protection for domain authentication channels. Organizations should enable both RequireSignOrSeal and SealSecureChannel to enforce maximum security on domain communication. Secure channel sealing has negligible performance impact on modern systems and should be enabled universally across all domain-joined systems.",
            Tags = ["secure-channel", "sealing", "encryption", "netlogon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SealSecureChannel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SealSecureChannel")],
            DetectOps = [RegOp.CheckDword(Key, "SealSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "secchan-require-strong-session-key",
            Label = "Require Strong Session Keys for Domain Secure Channel",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description = "Strong secure channel session keys require the use of 128-bit encryption for NetLogon channel protection replacing older 40-bit and 56-bit keys that are trivially crackable. Requiring strong session keys ensures that encrypted secure channel traffic cannot be decrypted by an attacker even with captured traffic and offline attacks. Older Windows NT systems used 40-bit session keys that can be broken in minutes on modern hardware making strong key requirements essential for current environments. Strong session key requirements are enabled by default on Windows Vista and later but should be explicitly required through policy to prevent negotiation of weaker keys. Organizations must ensure all domain-joined systems running Windows Vista or later which support 128-bit keys before enabling this requirement. The transition from weak to strong session keys is part of the NetLogon hardening changes in the Zerologon vulnerability remediation.",
            Tags = ["secure-channel", "session-keys", "encryption", "netlogon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireStrongKey", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireStrongKey")],
            DetectOps = [RegOp.CheckDword(Key, "RequireStrongKey", 1)],
        },
        new TweakDef
        {
            Id = "secchan-enable-full-netlogon-audit",
            Label = "Enable Full Audit Logging for NetLogon Secure Channel Events",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description = "NetLogon audit logging captures secure channel establishment failures and anomalous authentication events for security monitoring and incident response. Enabling full NetLogon audit creates visibility into domain authentication attempts that may indicate credential attacks or infrastructure problems. NetLogon failures during secure channel establishment can indicate Zerologon-style attacks or misconfigured domain trust relationships. The NetLogon log is stored on domain controllers and should be forwarded to SIEM for centralized analysis and alerting. Domain controller NetLogon log data combined with Security event log data provides comprehensive coverage of authentication threats. NetLogon audit data is particularly valuable during incident response to trace lateral movement paths through domain authentication.",
            Tags = ["secure-channel", "audit", "netlogon", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "FullSecureChannelAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "FullSecureChannelAudit")],
            DetectOps = [RegOp.CheckDword(Key, "FullSecureChannelAudit", 1)],
        },
        new TweakDef
        {
            Id = "secchan-disable-vulnerable-netlogon",
            Label = "Block Vulnerable NetLogon Connections from Non-Compliant Devices",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 4,
            Description = "The Zerologon vulnerability (CVE-2020-1472) exploits a flaw in the NetLogon secure channel allowing unauthenticated clients to set machine account passwords and gain domain administrator access. Blocking vulnerable NetLogon connections enforces the second phase of CVE-2020-1472 remediation denying connections from clients that do not use secure RPC. Microsoft released this as a phased fix where the initial patch logged vulnerable connections and the second phase blocked them outright. All Windows clients released after 2016 use secure RPC for NetLogon by default making this restriction safe for modern environments. Non-Windows devices like Linux Samba domain members may require updates to support secure RPC NetLogon before enforcement mode can be enabled. Organizations should review the NetLogon event log for any vulnerable client connections before enabling enforcement mode.",
            Tags = ["secure-channel", "zerologon", "cve-2020-1472", "netlogon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "FullSecureChannelProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "FullSecureChannelProtection")],
            DetectOps = [RegOp.CheckDword(Key, "FullSecureChannelProtection", 1)],
        },
        new TweakDef
        {
            Id = "secchan-set-machine-account-password-age",
            Label = "Set Maximum Machine Account Password Age for Secure Channel",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description = "Machine account password age controls how frequently domain-joined computers automatically rotate their computer account passwords as part of secure channel maintenance. Setting maximum machine account password age ensures regular rotation of computer credentials that are used to establish the secure channel to domain controllers. Computer account passwords are automatically changed every 30 days by default in Active Directory but this setting can enforce a shorter maximum. Environments with compliance requirements for credential rotation should set machine account password age to align with their policy requirements. Machine accounts with very old passwords may indicate stale or abandoned systems that should be reviewed for decommissioning. Disabling machine account password changes creates long-lived credentials that are a security risk if the machine is compromised.",
            Tags = ["secure-channel", "machine-account", "password-rotation", "netlogon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaximumPasswordAge", 30)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaximumPasswordAge")],
            DetectOps = [RegOp.CheckDword(Key, "MaximumPasswordAge", 30)],
        },
        new TweakDef
        {
            Id = "secchan-disable-machine-account-password-changes",
            Label = "Prevent Disabling of Automatic Machine Account Password Changes",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description = "Automatic machine account password rotation maintains the security of domain trust relationships by regularly changing computer account credentials. Preventing administrators from disabling machine account password changes ensures that automated credential rotation is not bypassed which would create static long-term credentials. Some organizations disable machine account password changes for problematic legacy systems but this creates long-lived credentials that weaken domain security. Machine account password change failures are often the cause of systems falling off the domain which should be addressed by fixing root causes rather than disabling rotation. DisablePasswordChange set to 1 is a common misconfiguration that should be detected and remediated during security assessments. Domain controllers should always enforce machine account password rotation as part of a healthy Active Directory environment.",
            Tags = ["secure-channel", "machine-account", "password-changes", "netlogon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePasswordChange", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordChange")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePasswordChange", 0)],
        },
        new TweakDef
        {
            Id = "secchan-restrict-domain-controller-replication",
            Label = "Restrict Unauthorized Domain Controller Replication Requests",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 5, SafetyRating = 4,
            Description = "DC Sync attacks exploit domain replication rights to steal password hashes from Active Directory without requiring code execution on a domain controller. Restricting domain controller replication ensures that only authorized domain controller accounts have rights to request replication of sensitive AD information. DCSync attacks use DRSUAPI replication calls to extract KRBTGT and other sensitive account hashes enabling further credential attacks. Replication rights should be limited exclusively to accounts with the DS-Replication-Get-Changes and DS-Replication-Get-Changes-All permissions that are domain controllers and select administrative accounts. Monitoring for DS-Replication-Get-Changes events from non-domain-controller accounts is a high-fidelity detection indicator for DCSync attacks. Organizations should audit replication permissions quarterly and remove any accounts that do not require these rights.",
            Tags = ["secure-channel", "dcsync", "replication", "netlogon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictDCReplication", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictDCReplication")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictDCReplication", 1)],
        },
        new TweakDef
        {
            Id = "secchan-enforce-netlogon-service-hardening",
            Label = "Enforce NetLogon Service Security Hardening Settings",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description = "NetLogon service hardening enforces additional security constraints on the NetLogon service that handles domain authentication preventing exploitation of service vulnerabilities. Enabling NetLogon service hardening sets a higher security bar for all NetLogon operations reducing the attack surface available to vulnerabilities like Zerologon. NetLogon service hardening was introduced as part of the CVE-2020-1472 remediation and Microsoft recommends it for all domain-joined systems. Service hardening configuration ensures the NetLogon service uses maximally secure communication parameters for all domain authentication operations. Organizations should monitor for NetLogon service failures after enabling hardening as service misconfiguration may prevent authentication. Testing NetLogon hardening in a non-production environment is recommended before wide deployment in complex multi-domain environments.",
            Tags = ["secure-channel", "service-hardening", "netlogon", "zero-day", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ServiceHardeningEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ServiceHardeningEnabled")],
            DetectOps = [RegOp.CheckDword(Key, "ServiceHardeningEnabled", 1)],
        },
        new TweakDef
        {
            Id = "secchan-set-account-lockout-on-channel-failure",
            Label = "Enable Account Lockout after Secure Channel Authentication Failures",
            Category = "Secure Channel Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
            Description = "Account lockout on secure channel authentication failure provides protection against brute-force attacks that attempt to guess machine account passwords or negotiate vulnerable secure channel parameters. Enabling lockout after repeated secure channel failures prevents automated attacks from making unlimited authentication guesses against domain accounts. Secure channel authentication failure lockout thresholds should be set high enough to avoid locking out systems with legitimate authentication problems such as time synchronization issues. The lockout threshold for machine accounts should be higher than interactive user accounts since machines do not have users who can notice and unlock their accounts. Organizations should implement account lockout monitoring to identify systems triggering lockout as this indicates either misconfiguration or active attack attempts. Automatically unlocking accounts after a short observation period balances security protection with operational availability.",
            Tags = ["secure-channel", "account-lockout", "brute-force", "netlogon", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LockoutOnChannelFailure", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LockoutOnChannelFailure")],
            DetectOps = [RegOp.CheckDword(Key, "LockoutOnChannelFailure", 1)],
        },
    ];
}
