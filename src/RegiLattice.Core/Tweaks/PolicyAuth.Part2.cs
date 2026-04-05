namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyAuth
{
    // ── KerberosDelegationPolicy ──
    private static class _KerberosDelegationPolicy
    {
        private const string KerbKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Security\Kerberos";

        private const string KerbNtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Kerberos";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "krbdel-require-kdc-validation",
                    Label = "Kerberos Delegation: Require KDC Certificate Validation for PKINIT",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets RequireKdcCertificate=1 in the Kerberos policy hive. Requires that the KDC (Key Distribution Center) presents a valid certificate during PKINIT (Public Key Initial Authentication) operations. Without KDC certificate validation, a rogue KDC on the network could successfully complete PKINIT authentication, allowing an attacker who performs an ARP or DNS spoofing attack to position a fake KDC and capture Kerberos TGT requests. This setting is particularly important in environments using smartcard or certificate-based authentication — certificate-based Kerberos without KDC validation is vulnerable to man-in-the-middle attacks.",
                    Tags = ["kerberos", "kdc", "pkinit", "certificate", "validation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "PKINIT operations require valid KDC certificate. Only applies in environments using smartcard or certificate-based Kerberos login. No impact on password-based Kerberos. PKINIT authentication will fail if the KDC certificate is expired, revoked, or untrusted — ensure domain controller certificates are maintained.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "RequireKdcCertificate", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "RequireKdcCertificate")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "RequireKdcCertificate", 1)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-claims-and-compound-auth",
                    Label = "Kerberos Delegation: Enable Kerberos Claims and Compound Authentication",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets EnableCbacAndArmor=3 in the Kerberos policy hive (always enable armoring and CBAC). Kerberos Armoring (FAST — Flexible Authentication Secure Tunneling) wraps Kerberos AS and TGS exchange messages in a protective tunnel, preventing offline cracking of AS-REQ pre-authentication data (a technique used by Kerberoasting attacks). Claims-Based Access Control (CBAC) augments Kerberos tickets with user and device claims for Dynamic Access Control. Setting value 3 (always armor — not just when supported) ensures the strongest protection for all authenticating clients.",
                    Tags = ["kerberos", "fast", "armoring", "claims", "cbac", "kerberoasting"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Kerberos FAST armoring and CBAC are always enabled. Kerberoasting offline cracking of AS-REQ data is prevented. Requires all authenticating DCs to support Kerberos armoring (Windows Server 2012+). Older DCs (2008 R2 or earlier) cannot participate. Test in a lab before deploying domain-wide.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "EnableCbacAndArmor", 3)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "EnableCbacAndArmor")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "EnableCbacAndArmor", 3)],
                },
                new TweakDef
                {
                    Id = "krbdel-set-ticket-lifetime-600",
                    Label = "Kerberos Delegation: Set TGT Maximum Lifetime to 600 Minutes",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets MaxTicketAge=600 in the Kerberos WinLogon hive. Caps the Kerberos Ticket-Granting Ticket (TGT) lifetime at 600 minutes (10 hours). A TGT is a long-lived credential that allows a user to obtain service tickets without re-authenticating to the KDC. If an attacker compromises a TGT (e.g., via Pass-the-Ticket or Golden Ticket attacks), the ticket is valid until its expiry. The default TGT lifetime is 10 hours; reducing it to 600 minutes aligns with a single work session and minimises the window in which a stolen TGT is valid. Combine with 10-minute service ticket lifetime for strongest protection.",
                    Tags = ["kerberos", "tgt", "ticket-lifetime", "pass-the-ticket", "golden-ticket"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "TGT lifetime is 600 minutes. Users with sessions longer than 10 hours may be prompted for re-authentication after the TGT expires (typically once their TGT is not automatically renewed). For most corporate environments this matches the working day. Kerberos ticket renewal is transparent to users in most cases.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxTicketAge", 600)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxTicketAge")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxTicketAge", 600)],
                },
                new TweakDef
                {
                    Id = "krbdel-set-service-ticket-lifetime-10",
                    Label = "Kerberos Delegation: Set TGS Service Ticket Lifetime to 10 Minutes",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets MaxServiceAge=10 in the Kerberos WinLogon hive. Sets the maximum lifetime for Kerberos service tickets (TGS tickets) to 10 minutes. Service tickets are short-lived credentials used for authenticating to a specific service (file share, SQL server, web application). If a service ticket is intercepted (e.g., via Kerberoasting — requesting a service ticket for an SPN and attempting offline cracking), a 10-minute lifetime means the cracked ticket is useful for only a very short window. Combined with strong service account passwords, this severely limits the utility of Kerberoasted tickets.",
                    Tags = ["kerberos", "service-ticket", "tgs", "kerberoasting", "lifetime"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Service ticket lifetime is 10 minutes. Clients silently request new service tickets as old ones expire; in most cases this is transparent. Some legacy applications with hard-coded Kerberos ticket handling may fail after 10 minutes. Test with critical line-of-business applications before deploying.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxServiceAge", 10)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxServiceAge")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxServiceAge", 10)],
                },
                new TweakDef
                {
                    Id = "krbdel-set-renewable-ticket-lifetime-7days",
                    Label = "Kerberos Delegation: Set Renewable Ticket Maximum Lifetime to 7 Days",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets MaxRenewAge=7 in the Kerberos WinLogon hive (units: days). Limits the window during which a Kerberos TGT can be renewed without re-authentication to 7 days. Kerberos TGTs can be marked as renewable: a client can present an expired TGT to the KDC and obtain a fresh one without providing a password, as long as the renewal request is within the MaxRenewAge window. If an attacker gets a copy of a TGT before it expires, they can potentially keep renewing it for up to the MaxRenewAge period. Setting 7 days limits the long-tail abuse window while supporting common Remote Desktop and service account usage patterns.",
                    Tags = ["kerberos", "renewable-ticket", "renewal-window", "tgt", "pass-the-ticket"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Renewable ticket window is 7 days. TGTs that have not been renewed within 7 days of original issuance require full re-authentication. Service accounts and interactive users with sessions spanning weekends are unaffected (7 days covers typical weekend coverage). No visible impact to most users.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxRenewAge", 7)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxRenewAge")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxRenewAge", 7)],
                },
                new TweakDef
                {
                    Id = "krbdel-enforce-strict-kdc-clock-skew-5min",
                    Label = "Kerberos Delegation: Enforce Strict 5-Minute Clock Skew Tolerance",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets MaxClockSkew=5 in the Kerberos WinLogon hive (units: minutes). Enforces a 5-minute maximum clock skew between client and KDC for Kerberos authentication. Kerberos relies on timestamps to prevent replay attacks — a Service ticket is only valid within a specific time window. If the clock skew limit is large, an attacker can capture a Kerberos service ticket and replay it successfully within the extended window. The default is 5 minutes (matching RFC 4120 recommendation). Explicitly setting it prevents GPO inheritance from accidentally relaxing this to a larger value.",
                    Tags = ["kerberos", "clock-skew", "replay-attack", "ntp", "timestamp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "5-minute clock skew strictly enforced. Systems with clocks drifted more than 5 minutes from the KDC will fail Kerberos authentication. Requires functioning NTP infrastructure. Virtual machines that resume from sleep may experience brief Kerberos failures until the clock synchronises with the domain controller.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "MaxClockSkew", 5)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "MaxClockSkew")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "MaxClockSkew", 5)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-pac-validation",
                    Label = "Kerberos Delegation: Enable PAC Request Validation on Kerberos Tickets",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets ValidateKdcPacSignature=1 in the Kerberos WinLogon hive. Enables validation of the Privilege Attribute Certificate (PAC) signature in Kerberos service tickets. The PAC contains group membership, logon hours, user rights, and other authorisation data. The MS14-068 vulnerability (a critical KDC privilege escalation) allowed an attacker to forge the PAC signature and elevate to Domain Admin. Enabling PAC signature validation ensures that all PACs in Kerberos tickets are cryptographically validated by the KDC before authorisation data is trusted. This closes a class of Kerberos PAC forgery attacks.",
                    Tags = ["kerberos", "pac", "ms14-068", "signature-validation", "privilege-escalation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "PAC cryptographic signature validated on every Kerberos service ticket. Mitigates MS14-068 and related PAC forgery attack classes. No visible impact to users — validation is performed transparently by the KDC. Requires all DCs to have the MS14-068 patch or be Windows Server 2012 R2+.",
                    ApplyOps = [RegOp.SetDword(KerbNtKey, "ValidateKdcPacSignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbNtKey, "ValidateKdcPacSignature")],
                    DetectOps = [RegOp.CheckDword(KerbNtKey, "ValidateKdcPacSignature", 1)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-aes256-kerberos-encryption",
                    Label = "Kerberos Delegation: Enforce AES-256 Kerberos Encryption, Disable RC4",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets SupportedEncryptionTypes=0x7FFFFFF8 in the Kerberos policy hive (enables AES256-HMAC-SHA1, AES128-HMAC-SHA1, DES-CBC-MD5 is excluded; RC4 HMAC is disabled). RC4-HMAC (also known as ARCFOUR-HMAC or Kerberos etype 17) is broken for Kerberos purposes — the NTLM hash of the user's password is directly usable as the Kerberos session key (enabling Pass-the-Hash attacks that bypass Kerberos entirely). Forcing AES-256 and AES-128 only means that stolen NTLM hashes cannot be used to forge Kerberos session keys, and Kerberoasted service ticket encryption must be broken as strong AES rather than weak RC4.",
                    Tags = ["kerberos", "aes256", "rc4", "encryption-type", "kerberoasting", "pass-the-hash"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "RC4-HMAC Kerberos encryption is disabled. AES-256 and AES-128 are enforced. Kerberoasting attacks must now defeat AES-256 encryption instead of RC4. Legacy applications and services that only support RC4 Kerberos encryption (typically very old Java, Oracle JDBC, or custom Kerberos implementations) will fail to authenticate. Test with all Kerberos-authenticating services.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "SupportedEncryptionTypes", 0x7FFFFFF8)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "SupportedEncryptionTypes")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "SupportedEncryptionTypes", 0x7FFFFFF8)],
                },
                new TweakDef
                {
                    Id = "krbdel-disable-des-kerberos-encryption",
                    Label = "Kerberos Delegation: Disable DES Kerberos Encryption Types",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets NtlmMinClientSec=0x20080000 in the Kerberos policy to explicitly exclude DES-based Kerberos encryption types (DES-CBC-CRC and DES-CBC-MD5). DES is a 56-bit block cipher that is comprehensively broken and should never be used in any security context. In Kerberos, DES encryption types (etypes 1 and 3) are retained for backwards compatibility with very old systems (pre-Windows 2000). An attacker who obtains a DES-encrypted Kerberos ticket can crack it in seconds to hours with commodity hardware. Windows Vista+ disabled DES by default; this policy ensures no Group Policy can accidentally re-enable it.",
                    Tags = ["kerberos", "des", "broken-crypto", "encryption-type", "etype"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "DES Kerberos encryption types are explicitly disabled. No modern Windows system requires DES Kerberos. If any legacy system (pre-Vista Windows or old UNIX Kerberos client) attempts DES-based Kerberos authentication, it will fail. Survey the environment before enforcing if there are unknown legacy systems.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "DESEncryptionDisabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "DESEncryptionDisabled")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "DESEncryptionDisabled", 1)],
                },
                new TweakDef
                {
                    Id = "krbdel-enable-kerberos-pre-auth-required",
                    Label = "Kerberos Delegation: Require Kerberos Pre-Authentication for All Accounts",
                    Category = "User Account — Credential Delegation",
                    Description =
                        "Sets DoNotRequirePreauth=0 in the Kerberos policy hive. Ensures that Kerberos pre-authentication is required for all accounts by policy. By default, any account in Active Directory with the 'Do not require Kerberos preauthentication' flag set (DONT_REQ_PREAUTH) will respond to an AS-REQ with an AS-REP without the client first proving knowledge of the password. This is the condition that enables AS-REP Roasting — an attacker can request an AS-REP for any account with this flag and attempt offline cracking of the encrypted portion. This policy setting prevents the environment from inadvertently introducing accounts with this flag via attribute editors.",
                    Tags = ["kerberos", "pre-auth", "as-rep-roasting", "asrep", "preauth"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "AS-REP Roasting is blocked: all accounts require Kerberos pre-auth by policy. Pre-existing accounts with DONT_REQ_PREAUTH set in AD must be audited and corrected separately (policy alone cannot override per-account AD attribute flags). Use PowerShell to audit: Get-ADUser -Filter {DoesNotRequirePreAuth -eq $true}.",
                    ApplyOps = [RegOp.SetDword(KerbKey, "DoNotRequirePreauth", 0)],
                    RemoveOps = [RegOp.DeleteValue(KerbKey, "DoNotRequirePreauth")],
                    DetectOps = [RegOp.CheckDword(KerbKey, "DoNotRequirePreauth", 0)],
                },
            ];
    }

    // ── KerberosEncryptionPolicy ──
    private static class _KerberosEncryptionPolicy
    {
        private const string KerbPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
        private const string KerbLsaKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "kerb-disable-des-encryption",
                Label = "Disable DES Encryption for Kerberos",
                Category = "User Account — Kerberos Encryption",
                Description = "Prevents Kerberos from using the broken DES (56-bit) encryption type for tickets.",
                Tags = ["kerberos", "des", "encryption", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "DES is trivially broken; removing it forces AES. Requires DC and clients on Server 2008+/Vista+.",
                ApplyOps = [RegOp.SetDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483640)],
                RemoveOps = [RegOp.DeleteValue(KerbPolicyKey, "SupportedEncryptionTypes")],
                DetectOps = [RegOp.CheckDword(KerbPolicyKey, "SupportedEncryptionTypes", 2147483640)],
            },
            new TweakDef
            {
                Id = "kerb-set-max-ticket-age-600",
                Label = "Set Kerberos Maximum Ticket Age to 600 Minutes",
                Category = "User Account — Kerberos Encryption",
                Description = "Limits Kerberos TGT lifetime to 10 hours (600 minutes) to reduce stolen-ticket window.",
                Tags = ["kerberos", "ticket-age", "tgt", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Shorter TGT lifetime reduces Pass-the-Ticket window. Default is 10h; this enforces policy alignment.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "MaxTicketAge", 600)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "MaxTicketAge")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "MaxTicketAge", 600)],
            },
            new TweakDef
            {
                Id = "kerb-set-max-renew-age-7days",
                Label = "Set Kerberos Maximum Ticket Renewal Age to 7 Days",
                Category = "User Account — Kerberos Encryption",
                Description = "Limits how long a Kerberos TGT can be renewed before requiring full re-authentication.",
                Tags = ["kerberos", "renewal", "tgt", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "7-day renewal window is the CIS benchmark default. Prevents stale tickets being used indefinitely.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "MaxRenewAge", 10080)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "MaxRenewAge")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "MaxRenewAge", 10080)],
            },
            new TweakDef
            {
                Id = "kerb-set-max-service-ticket-600",
                Label = "Set Kerberos Maximum Service Ticket Age to 600 Minutes",
                Category = "User Account — Kerberos Encryption",
                Description = "Limits service ticket lifetime to 600 minutes to reduce the stolen service ticket window.",
                Tags = ["kerberos", "service-ticket", "st", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Matches Microsoft security baseline. Transparent to users.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "MaxServiceAge", 600)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "MaxServiceAge")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "MaxServiceAge", 600)],
            },
            new TweakDef
            {
                Id = "kerb-set-clock-skew-5min",
                Label = "Set Kerberos Maximum Clock Skew to 5 Minutes",
                Category = "User Account — Kerberos Encryption",
                Description = "Enforces a 5-minute maximum clock skew between client and KDC to prevent replay attacks.",
                Tags = ["kerberos", "clock-skew", "replay", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Standard Kerberos replay protection. Ensure NTP is configured to avoid authentication failures.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "SkewTime", 5)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "SkewTime")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "SkewTime", 5)],
            },
            new TweakDef
            {
                Id = "kerb-enable-armoring",
                Label = "Enable Kerberos Armoring (FAST)",
                Category = "User Account — Kerberos Encryption",
                Description = "Enables Kerberos Flexible Authentication Secure Tunnelling (FAST/armoring) to protect AS-REQ exchanges.",
                Tags = ["kerberos", "armoring", "fast", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "FAST prevents AS-REP roasting and preauthentication attacks. Requires Windows 8+ clients and Server 2012+ DCs.",
                ApplyOps = [RegOp.SetDword(KerbPolicyKey, "cbindingPolicy", 2)],
                RemoveOps = [RegOp.DeleteValue(KerbPolicyKey, "cbindingPolicy")],
                DetectOps = [RegOp.CheckDword(KerbPolicyKey, "cbindingPolicy", 2)],
            },
            new TweakDef
            {
                Id = "kerb-disable-upn-hint",
                Label = "Disable Kerberos UPN Hint Leakage",
                Category = "User Account — Kerberos Encryption",
                Description = "Prevents Kerberos error responses from leaking UPN/username hints to unauthenticated requesters.",
                Tags = ["kerberos", "upn", "enumeration", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks username enumeration via Kerberos pre-auth errors. Transparent for legitimate clients.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "UseUpnForClientAuthEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "UseUpnForClientAuthEnabled")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "UseUpnForClientAuthEnabled", 0)],
            },
            new TweakDef
            {
                Id = "kerb-set-preauthentication-required",
                Label = "Require Kerberos Preauthentication",
                Category = "User Account — Kerberos Encryption",
                Description = "Enforces Kerberos preauthentication to prevent AS-REP roasting on accounts that have it disabled.",
                Tags = ["kerberos", "preauthentication", "as-rep-roasting", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "AS-REP roasting requires accounts with 'Do not require Kerberos preauth' set. This policy enforces it machine-wide.",
                ApplyOps = [RegOp.SetDword(KerbLsaKey, "PreAuthRequiredLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(KerbLsaKey, "PreAuthRequiredLevel")],
                DetectOps = [RegOp.CheckDword(KerbLsaKey, "PreAuthRequiredLevel", 1)],
            },
        ];
    }

    // ── KerberosSecurityPolicy ──
    private static class _KerberosSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System\Audit";
        private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Security\Kerberos";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "krbadv-require-fast-armoring",
                    Label = "Require Kerberos Armoring (FAST) for All Authentication",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Requires Kerberos Flexible Authentication Secure Tunneling (FAST/Kerberos Armoring) for all Kerberos exchanges, providing protection against offline pre-authentication blob cracking attacks (AS-REP roasting).",
                    Tags = ["kerberos", "fast", "armoring", "asrep-roasting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos FAST armoring required; AS-REP roasting attacks mitigated. Requires KDC support for FAST.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableKerberosArmoring", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableKerberosArmoring")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableKerberosArmoring", 2)],
                },
                new TweakDef
                {
                    Id = "krbadv-block-rc4-encryption",
                    Label = "Block RC4-HMAC Encryption for Kerberos Tickets",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Disables the RC4-HMAC cipher suite for Kerberos ticket encryption, forcing all tickets to use AES-128 or AES-256 encryption, which is significantly stronger than the legacy RC4 encryption still used by some service accounts.",
                    Tags = ["kerberos", "rc4", "aes", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos RC4-HMAC encryption disabled; only AES-128/AES-256 tickets accepted. Service accounts need AES keys.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRC4Encryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRC4Encryption")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRC4Encryption", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-enable-des-encryption-off",
                    Label = "Disable DES Cipher for Kerberos (Legacy Removal)",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Disables DES (Data Encryption Standard) cipher support in Kerberos, eliminating the use of the cryptographically broken DES algorithm that was still negotiated with very old service accounts in some mixed environments.",
                    Tags = ["kerberos", "des", "legacy-cipher", "encryption", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos DES encryption completely disabled; broken DES cipher no longer negotiated in any Kerberos exchange.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowDES", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowDES")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowDES", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-ticket-lifetime-8h",
                    Label = "Set Kerberos Ticket Maximum Lifetime to 8 Hours",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Configures the Kerberos TGT (Ticket Granting Ticket) maximum lifetime to 8 hours, ensuring tickets expire during a typical business day so stolen tickets cannot be replayed indefinitely.",
                    Tags = ["kerberos", "ticket-lifetime", "tgt", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos TGT lifetime set to 8 hours; stolen tickets expire within a business day.",
                    ApplyOps = [RegOp.SetDword(SecKey, "MaxTicketAge", 8)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "MaxTicketAge")],
                    DetectOps = [RegOp.CheckDword(SecKey, "MaxTicketAge", 8)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-service-ticket-lifetime-10m",
                    Label = "Set Kerberos Service Ticket Maximum Lifetime to 600 Minutes",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Sets the maximum service ticket (TGS) lifetime to 600 minutes (10 hours), which is long enough for a business day session while limiting the window during which a stolen service ticket could be replayed against a service.",
                    Tags = ["kerberos", "service-ticket", "tgs", "lifetime", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos service ticket lifetime limited to 10 hours; limits replay window for stolen service tickets.",
                    ApplyOps = [RegOp.SetDword(SecKey, "MaxServiceAge", 600)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "MaxServiceAge")],
                    DetectOps = [RegOp.CheckDword(SecKey, "MaxServiceAge", 600)],
                },
                new TweakDef
                {
                    Id = "krbadv-set-renew-lifetime-7d",
                    Label = "Set Kerberos Ticket Maximum Renewal Lifetime to 7 Days",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Sets the maximum TGT renewal lifetime to 7 days, after which the user must fully re-authenticate with their password or smart card rather than just renewing an existing ticket.",
                    Tags = ["kerberos", "renewal-lifetime", "tgt", "re-authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos TGT renewable lifetime set to 7 days; full re-auth required after 1 week.",
                    ApplyOps = [RegOp.SetDword(SecKey, "MaxRenewAge", 7)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "MaxRenewAge")],
                    DetectOps = [RegOp.CheckDword(SecKey, "MaxRenewAge", 7)],
                },
                new TweakDef
                {
                    Id = "krbadv-log-kerberos-failures",
                    Label = "Log Kerberos Pre-Authentication Failure Events",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Enables Security audit logging for Kerberos AS exchange pre-authentication failures (EventID 4771), providing visibility into password-spraying and Kerberoasting attempts against domain accounts.",
                    Tags = ["kerberos", "pre-auth-failure", "audit", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos pre-auth failures logged (EventID 4771); password spray and Kerberoasting attempts visible.",
                    ApplyOps = [RegOp.SetDword(SecKey, "AuditPreAuthFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "AuditPreAuthFailures")],
                    DetectOps = [RegOp.CheckDword(SecKey, "AuditPreAuthFailures", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-block-unconstrained-delegation",
                    Label = "Block Accounts from Using Unconstrained Kerberos Delegation",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Enables the 'Account is sensitive and cannot be delegated' flag enforcement at policy level, blocking non-protected accounts from being marked for unconstrained delegation which allows impersonation of any user who authenticates to the delegate.",
                    Tags = ["kerberos", "delegation", "unconstrained", "impersonation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Unconstrained Kerberos delegation blocked for new accounts; existing delegation settings unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUnconstrainedDelegation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUnconstrainedDelegation")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUnconstrainedDelegation", 1)],
                },
                new TweakDef
                {
                    Id = "krbadv-disable-kerberos-telemetry",
                    Label = "Disable Kerberos Authentication Telemetry to Microsoft",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Prevents the Windows Kerberos provider from sending cipher negotiation stats, authentication failure rates, and encryption algorithm telemetry to Microsoft.",
                    Tags = ["kerberos", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Kerberos telemetry to Microsoft disabled; cipher negotiation and failure rate data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableKerberosTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableKerberosTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableKerberosTelemetry", 1)],
                },
            ];
    }

    // ── LapsPolicy ──
    private static class _LapsPolicy
    {
        private const string LapsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\LAPS";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lapspol-enable-ad-backup",
                    Label = "Configure LAPS to Back Up Password to Active Directory",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Directs Windows LAPS to store the managed local administrator password in Active Directory DS.",
                    Tags = ["laps", "password", "active-directory", "backup", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Enables centralised credential management; requires AD DS and LAPS schema extension.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "BackupDirectory", 2)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "BackupDirectory")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "BackupDirectory", 2)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-password-age-30",
                    Label = "Set LAPS Maximum Password Age to 30 Days",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Configures the Windows LAPS managed account password to expire after a maximum of 30 days.",
                    Tags = ["laps", "password", "expiry", "rotation", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Password is automatically rotated every 30 days; no user action required.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordAgeDays", 30)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordAgeDays")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordAgeDays", 30)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-password-length-20",
                    Label = "Set LAPS Minimum Password Length to 20",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Forces the LAPS-managed local administrator password to be at least 20 characters long.",
                    Tags = ["laps", "password", "length", "complexity", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Longer passwords improve brute-force resistance; LAPS manages them automatically.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordLength", 20)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordLength")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordLength", 20)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-password-complexity-full",
                    Label = "Set LAPS Password Complexity to Full",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Requires the LAPS-generated password to include uppercase, lowercase, digits, and special characters.",
                    Tags = ["laps", "password", "complexity", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Value 4 = large letters + small letters + digits + special chars; maximum entropy.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordComplexity", 4)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordComplexity")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordComplexity", 4)],
                },
                new TweakDef
                {
                    Id = "lapspol-post-auth-reset-logoff",
                    Label = "Reset LAPS Password and Log Off After Admin Use",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Automatically resets the managed local admin password and logs off the session after it is used for authentication.",
                    Tags = ["laps", "password", "post-auth", "rotation", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Value 3 = reset password + terminate managed account logon sessions; prevents credential reuse.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PostAuthenticationActions", 3)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PostAuthenticationActions")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PostAuthenticationActions", 3)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-post-auth-delay-24h",
                    Label = "Set LAPS Post-Auth Reset Delay to 24 Hours",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Delays the post-authentication password reset for 24 hours to allow admin tasks to complete before rotation.",
                    Tags = ["laps", "password", "post-auth", "delay", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Gives admins 24 hours to finish tasks before the managed account password is rotated.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PostAuthenticationResetDelay", 24)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PostAuthenticationResetDelay")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PostAuthenticationResetDelay", 24)],
                },
                new TweakDef
                {
                    Id = "lapspol-enable-ad-encryption",
                    Label = "Encrypt LAPS Password in Active Directory",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Stores the LAPS-managed password in Active Directory using AES-256 encryption instead of plain text.",
                    Tags = ["laps", "password", "encryption", "active-directory", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Password is AES-256 encrypted at rest in AD; requires Windows Server 2016 DC or later.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "ADPasswordEncryptionEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "ADPasswordEncryptionEnabled")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "ADPasswordEncryptionEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "lapspol-enable-expiry-protection",
                    Label = "Enable LAPS Password Expiry Protection",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Prevents the LAPS password expiry date from being set into the future by unauthorised parties.",
                    Tags = ["laps", "password", "expiry", "protection", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks attackers from extending the LAPS password lifetime to avoid rotation.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "PasswordExpirationProtectionEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "PasswordExpirationProtectionEnabled")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "PasswordExpirationProtectionEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "lapspol-enable-audit-policy",
                    Label = "Enable LAPS Audit Policy",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Enables Windows LAPS audit logging to track password read and update events in the Security event log.",
                    Tags = ["laps", "audit", "logging", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Records LAPS credential access events; useful for detecting unauthorised admin account usage.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "AuditPolicyEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "AuditPolicyEnabled")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "AuditPolicyEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "lapspol-set-expiry-notify-7d",
                    Label = "Notify 7 Days Before LAPS Password Expiry",
                    Category = "User Account — Kerberos Encryption",
                    Description = "Sends a warning notification to administrators 7 days before the LAPS-managed password expires.",
                    Tags = ["laps", "password", "expiry", "notification", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Generates an event log warning 7 days before password rotation; purely informational.",
                    ApplyOps = [RegOp.SetDword(LapsKey, "NotifyPasswordExpiryDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(LapsKey, "NotifyPasswordExpiryDays")],
                    DetectOps = [RegOp.CheckDword(LapsKey, "NotifyPasswordExpiryDays", 7)],
                },
            ];
    }

    // ── LapsSecurity ──
    private static class _LapsSecurity
    {
        private const string LapsPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LAPS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "laps-backup-to-ad",
                Label = "LAPS: Back Up Local Admin Password to Active Directory",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["laps", "active-directory", "password-backup", "admin-password", "on-premises"],
                Description =
                    "Sets BackupDirectory=2 in the Windows LAPS policy. "
                    + "Configures Windows LAPS to back up the managed local administrator password to "
                    + "on-premises Active Directory. Values: 0=disabled, 1=Azure AD/Entra, 2=AD on-premises. "
                    + "Requires the AD schema to be extended for Windows LAPS (distinct from legacy Microsoft LAPS).",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "BackupDirectory", 2)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "BackupDirectory")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "BackupDirectory", 2)],
            },
            new TweakDef
            {
                Id = "laps-max-age-14-days",
                Label = "LAPS: Set Maximum Password Age to 14 Days",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "password-age", "rotation", "admin-password"],
                Description =
                    "Sets PasswordAgeDays=14 in the Windows LAPS policy. "
                    + "Requires the local administrator password to be rotated at least every 14 days. "
                    + "The default is 30 days. Shorter rotation limits the window of exposure if a cached/leaked "
                    + "password is used in a pass-the-hash or lateral movement attack.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordAgeDays", 14)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordAgeDays")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordAgeDays", 14)],
            },
            new TweakDef
            {
                Id = "laps-password-length-20",
                Label = "LAPS: Set Minimum Password Length to 20 Characters",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "password-length", "strength", "admin-password"],
                Description =
                    "Sets PasswordLength=20 in the Windows LAPS policy. "
                    + "Requires the generated local administrator password to be at least 20 characters long. "
                    + "The default is 14 characters. At 20 characters with mixed complexity, the password provides "
                    + "over 100 bits of entropy, making offline brute-force attacks computationally infeasible.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordLength", 20)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordLength")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordLength", 20)],
            },
            new TweakDef
            {
                Id = "laps-password-max-complexity",
                Label = "LAPS: Require Maximum Password Complexity (All Character Types)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "password-complexity", "strength", "admin-password"],
                Description =
                    "Sets PasswordComplexity=4 in the Windows LAPS policy. "
                    + "Requires the generated password to include large letters, small letters, numbers, and "
                    + "special characters. Values: 1=large only, 2=large+small, 3=large+small+numbers, "
                    + "4=large+small+numbers+specials (maximum complexity).",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PasswordComplexity", 4)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PasswordComplexity")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PasswordComplexity", 4)],
            },
            new TweakDef
            {
                Id = "laps-enable-password-encryption",
                Label = "LAPS: Enable Encrypted Password Storage in Active Directory",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["laps", "encryption", "active-directory", "password-storage"],
                Description =
                    "Sets ADPasswordEncryptionEnabled=1 in the Windows LAPS policy. "
                    + "Encrypts the LAPS password before it is stored in Active Directory, using the AD computer "
                    + "object's NTDS DPAPI master key. Only authorized AD principals (admins, the computer itself) "
                    + "can decrypt the password. Requires on-premises AD with the Windows LAPS schema extension.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "ADPasswordEncryptionEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "ADPasswordEncryptionEnabled")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "ADPasswordEncryptionEnabled", 1)],
            },
            new TweakDef
            {
                Id = "laps-post-auth-reset-and-logoff",
                Label = "LAPS: Reset Password and Log Off After Admin Authentication",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "post-auth", "password-reset", "logoff", "hygiene"],
                Description =
                    "Sets PostAuthenticationActions=3 in the Windows LAPS policy. "
                    + "After the local admin account is used to authenticate (e.g., for a break-glass login), "
                    + "LAPS automatically resets the password AND logs off active sessions. "
                    + "Values: 1=reset password only, 2=logoff+reset, 3=logoff+reset+terminate processes.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PostAuthenticationActions", 3)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PostAuthenticationActions")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PostAuthenticationActions", 3)],
            },
            new TweakDef
            {
                Id = "laps-post-auth-delay-24h",
                Label = "LAPS: Set 24-Hour Grace Period Before Post-Auth Password Reset",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["laps", "post-auth", "delay", "grace-period"],
                Description =
                    "Sets PostAuthenticationResetDelay=24 in the Windows LAPS policy. "
                    + "Specifies how many hours Windows waits after an authentication event before triggering "
                    + "the post-authentication password rotation action. "
                    + "A 24-hour delay gives administrators time to complete maintenance work before the "
                    + "account is reset and active sessions are terminated.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "PostAuthenticationResetDelay", 24)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "PostAuthenticationResetDelay")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "PostAuthenticationResetDelay", 24)],
            },
            new TweakDef
            {
                Id = "laps-encrypt-history-12",
                Label = "LAPS: Retain 12 Encrypted Previous Passwords in AD History",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["laps", "history", "encryption", "active-directory", "audit"],
                Description =
                    "Sets ADEncryptedPasswordHistorySize=12 in the Windows LAPS policy. "
                    + "Configures Windows LAPS to retain the last 12 encrypted historical passwords in "
                    + "Active Directory. Enables recovery of previously used passwords for forensic analysis "
                    + "or rolling back after an incident. Requires ADPasswordEncryptionEnabled=1.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "ADEncryptedPasswordHistorySize", 12)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "ADEncryptedPasswordHistorySize")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "ADEncryptedPasswordHistorySize", 12)],
            },
            new TweakDef
            {
                Id = "laps-disable-legacy-laps",
                Label = "LAPS: Disable Legacy Microsoft LAPS (Allow Only Windows LAPS)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 22621,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["laps", "legacy", "migration", "admin-password"],
                Description =
                    "Sets LegacyMicrosoftLAPSEnabled=0 in the Windows LAPS policy. "
                    + "Disables the legacy Microsoft LAPS CSE (Client-Side Extension) when the built-in Windows "
                    + "LAPS is configured. Prevents both legacy and new LAPS from running simultaneously, which "
                    + "could cause password conflicts. Required when migrating from legacy LAPS to Windows LAPS.",
                ApplyOps = [RegOp.SetDword(LapsPolicy, "LegacyMicrosoftLAPSEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(LapsPolicy, "LegacyMicrosoftLAPSEnabled")],
                DetectOps = [RegOp.CheckDword(LapsPolicy, "LegacyMicrosoftLAPSEnabled", 0)],
            },
        ];
    }

    // ── LegacyAuthPolicy ──
    private static class _LegacyAuthPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "legauth-disable-lm-response",
                Label = "Disable LAN Manager Hash Response (LM Authentication)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "LAN Manager authentication is a decades-old protocol that uses weak DES-encrypted password hashes that are trivially cracked with modern hardware. Disabling LM authentication responses prevents Windows from responding to LM authentication challenge requests from legacy systems. LM hashes can be cracked offline in minutes using dictionary attacks or rainbow tables making stored LM credentials immediately exploitable. Windows systems should be configured to use NTLMv2 or Kerberos instead of LM for all network authentication. LM authentication may be required for compatibility with very old systems like Windows 95/98 but these systems should not be present in modern enterprise networks. Disabling LM responses forces all authentication to use stronger NTLM or Kerberos protocols that provide superior security.",
                Tags = ["lm", "authentication", "legacy", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLmResponse", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLmResponse")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLmResponse", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-ntlm-v1",
                Label = "Disable NTLMv1 Authentication Protocol",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "NTLMv1 is an older version of the NTLM authentication protocol that lacks the security improvements added in NTLMv2. Disabling NTLMv1 forces upgrading to NTLMv2 which includes session nonces preventing credential replay attacks that work against NTLMv1. NTLMv1 is vulnerable to pass-the-hash attacks where captured NTLM hashes are replayed without knowing the actual password. Microsoft has recommended disabling NTLMv1 since Windows Vista and its continued use represents unnecessary authentication risk. NTLMv1 responses can be downgr- forced by attackers in man-in-the-middle positions to capture more easily cracked credential material. Enterprise environments should audit for remaining NTLMv1 usage and migrate legacy applications to Kerberos or NTLMv2 before disabling.",
                Tags = ["ntlm", "ntlmv1", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNtlmV1", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNtlmV1")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNtlmV1", 1)],
            },
            new TweakDef
            {
                Id = "legauth-require-ntlmv2",
                Label = "Require NTLMv2 Response Only for Network Authentication",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Requiring NTLMv2-only responses ensures that Windows systems only send NTLMv2 credentials rejecting all older LM and NTLMv1 authentication requests. NTLMv2 includes session security features like mutual authentication keys and per-session nonces that reduce credential theft and replay risks. Requiring NTLMv2 only is an effective defense against downgrade attacks that force less secure authentication protocols. Systems requiring NTLMv2-only should be tested for compatibility with servers and applications using older NTLM versions before policy enforcement. NTLMv2 requirement should be combined with session security settings to maximize the security improvement. Organizations should prefer Kerberos over NTLM where possible with NTLMv2 as the fallback for legacy compatibility scenarios.",
                Tags = ["ntlm", "ntlmv2", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireNtlmV2", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireNtlmV2")],
                DetectOps = [RegOp.CheckDword(Key, "RequireNtlmV2", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-weak-hash-storing",
                Label = "Prevent Storage of LAN Manager Hashes in SAM Database",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "The Security Accounts Manager database can store both NTLM and LAN Manager password hashes with LM hashes being significantly weaker. Preventing LM hash storage ensures that even if the SAM database is extracted attackers only obtain NTLM hashes rather than trivially crackable LM hashes. LM hashes split passwords into 7-character chunks that can be brute-forced independently reducing cracking complexity dramatically. Removing LM hash storage means that new password changes will not generate LM hashes but existing LM hashes persist until passwords change. A password change cycle should be initiated after enabling this policy to eliminate stored LM hashes from the SAM database. This policy is effective when combined with the NTLMv2-only response requirement and LM authentication disablement.",
                Tags = ["lm-hash", "sam", "password", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoLmHash", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoLmHash")],
                DetectOps = [RegOp.CheckDword(Key, "NoLmHash", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-ntlm-outbound",
                Label = "Restrict Outbound NTLM Authentication Requests",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Outbound NTLM authentication allows Windows systems to authenticate to remote servers using NTLM which can be exploited to capture NTLM credentials. Restricting outbound NTLM prevents Windows systems from sending NTLM responses to rogue servers set up by attackers for credential capture. NTLM credential capture attacks involve an attacker triggering authentication requests to a server they control and capturing the NTLM response for offline cracking. Restricting outbound NTLM to allowed server lists forces explicit whitelisting of servers that require NTLM authentication. Organizations should identify all servers requiring NTLM authentication before enabling this restriction to prevent service disruptions. The restriction should be set to audit mode first to identify NTLM usage before switching to denial mode.",
                Tags = ["ntlm", "outbound", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictOutboundNtlm", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictOutboundNtlm")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictOutboundNtlm", 2)],
            },
            new TweakDef
            {
                Id = "legauth-restrict-ntlm-inbound",
                Label = "Restrict Inbound NTLM Authentication to Domain Accounts",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Inbound NTLM authentication allows external systems to authenticate to this Windows server using NTLM which can be exploited in pass-the-hash attacks. Restricting inbound NTLM to domain accounts prevents local account NTLM authentication which is commonly exploited for lateral movement. Domain account NTLM authentication is subject to Kerberos validation in domain environments providing stronger authentication guarantees. Local account NTLM authentication bypasses domain controller validation making it useful for attackers with captured local credentials. Restricting inbound NTLM to domain accounts forces attackers to use Kerberos authentication which provides better monitoring and audit capabilities. Organizations should test inbound NTLM restrictions in audit mode before enforcement to identify local account dependencies.",
                Tags = ["ntlm", "inbound", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictInboundNtlm", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictInboundNtlm")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictInboundNtlm", 1)],
            },
            new TweakDef
            {
                Id = "legauth-enable-ntlm-audit",
                Label = "Enable NTLM Authentication Event Auditing",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NTLM authentication auditing generates Windows event log entries for all NTLM authentication attempts providing visibility into NTLM usage. Enabling NTLM auditing allows security teams to identify which applications and systems are still using NTLM authentication. NTLM audit logs reveal authentication patterns that indicate pass-the-hash attacks or unauthorized lateral movement using NTLM credentials. Audit data is necessary to identify NTLM dependencies before implementing NTLM restrictions that could disrupt production services. NTLM authentication events should be forwarded to SIEM for correlation with threat intelligence and anomaly detection. Regular review of NTLM audit data helps drive migration from NTLM to Kerberos over time as dependencies are identified and resolved.",
                Tags = ["ntlm", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditNtlm", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNtlm")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNtlm", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-basic-auth",
                Label = "Disable Basic HTTP Authentication for Network Providers",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Basic HTTP authentication transmits credentials in Base64 encoding which is trivially decoded providing plaintext username and password to network observers. Disabling basic authentication for network providers prevents credentials from being transmitted in a format that exposes them to network sniffing. Basic authentication is insecure even over HTTPS as logs and proxies may capture the authentication header containing credentials. Network providers including WebDAV implementations that support basic authentication should be updated to use Negotiate or modern token-based authentication. Disabling basic auth may break legacy applications and web services that use basic authentication but safer alternatives are available. Organizations must identify all basic authentication dependencies and migrate them before enforcing this restriction.",
                Tags = ["basic-auth", "authentication", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBasicAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBasicAuth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBasicAuth", 1)],
            },
            new TweakDef
            {
                Id = "legauth-disable-digest-auth",
                Label = "Disable Digest Authentication for Network Connections",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Digest authentication is a challenge-response authentication scheme that provides limited protection compared to modern authentication protocols. Disabling digest authentication prevents Windows network providers from using this older authentication mechanism for WebDAV and similar connections. Digest authentication stores passwords in a reversible format on servers requiring it making server compromise expose all user credentials. Modern web applications should use OAuth2, SAML, or Negotiate authentication rather than Basic or Digest schemes. Digest authentication is vulnerable to man-in-the-middle attacks where an attacker can downgrade or capture authentication sequences. Organizations using IIS or other web servers that rely on digest authentication should migrate to Windows Integrated Authentication or modern tokens.",
                Tags = ["digest-auth", "authentication", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDigestAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDigestAuth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDigestAuth", 1)],
            },
            new TweakDef
            {
                Id = "legauth-enable-extended-protection",
                Label = "Enable Extended Protection for Authentication (EPA)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Extended Protection for Authentication binds authentication tokens to the channel establishment preventing credential forwarding to unauthorized channels. Enabling EPA prevents NTLM credential relay attacks where an attacker intercepts and forwards authentication tokens to a different server. Channel binding tokens ensure that authentication credentials cannot be used against a server other than the one the client intended to authenticate to. EPA is particularly important for protecting against cross-protocol relay attacks such as NTLM relay to SMB or LDAP. Enabling EPA may impact older applications that do not support channel binding so compatibility testing is required. Microsoft recommends enabling EPA on all servers that support it as a defense-in-depth control against credential relay attacks.",
                Tags = ["epa", "authentication", "relay-protection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableExtendedProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableExtendedProtection")],
                DetectOps = [RegOp.CheckDword(Key, "EnableExtendedProtection", 1)],
            },
        ];
    }

    // ── LocalSecurityAuthorityPolicy ──
    private static class _LocalSecurityAuthorityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
        private const string SecKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string CfgKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\WDigest";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lsapol-disable-legacy-auth-packages",
                    Label = "Remove Legacy Security Support Provider Packages from LSA",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Removes legacy SSPI authentication packages (msapsspc, msnsspc) from the LSA Security Packages list, preventing these deprecated packages from being loaded as SSPI providers that could be backdoored or exploited.",
                    Tags = ["lsa", "sspi", "legacy-packages", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Legacy LSA SSPI packages removed; deprecated authentication DLLs not loaded in LSASS process.",
                    ApplyOps = [RegOp.SetDword(SecKey, "DisableLegacyLSAPackages", 1)],
                    RemoveOps = [RegOp.DeleteValue(SecKey, "DisableLegacyLSAPackages")],
                    DetectOps = [RegOp.CheckDword(SecKey, "DisableLegacyLSAPackages", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-deny-network-logon-local-accounts",
                    Label = "Deny Network Logon for Local Administrator Accounts",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Blocks local administrator accounts (SID S-1-5-113) from performing network logons (interactive pass-the-hash, NTLM relay), ensuring only domain accounts can authenticate over the network and local creds cannot be used for lateral movement.",
                    Tags = ["lsa", "network-logon", "local-admin", "lateral-movement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Network logon denied for local administrator accounts; local account pass-the-hash lateral movement blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyNetworkLogonForLocalAccounts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyNetworkLogonForLocalAccounts")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyNetworkLogonForLocalAccounts", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-enable-token-filter-policy",
                    Label = "Enable Local Account Token Filter Policy (Full Token on Network)",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Enables LocalAccountTokenFilterPolicy which allows local admin accounts that authenticate over the network to receive a full elevated token (rather than a filtered one), enabling legitimate remote administration without requiring domain accounts. Counterintuitively named, this is required for tools like PSExec to work over the network to local admin.",
                    Tags = ["lsa", "token-filter", "local-admin", "remote-admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "Local account token filter disabled; local admin gets full elevated token on network logon. Required for PSExec-style remote admin.",
                    ApplyOps = [RegOp.SetDword(Key, "LocalAccountTokenFilterPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LocalAccountTokenFilterPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "LocalAccountTokenFilterPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-lsa-telemetry",
                    Label = "Disable LSA / Authentication Provider Telemetry to Microsoft",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Prevents the LSA and Windows authentication providers from sending authentication event rates, credential provider selection, and logon failure telemetry to Microsoft.",
                    Tags = ["lsa", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LSA telemetry to Microsoft disabled; auth event data and failure rates not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLSATelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLSATelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLSATelemetry", 1)],
                },
            ];
    }

    // ── LogonCachePolicy ──
    private static class _LogonCachePolicy
    {
        private const string Winlogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
        private const string NetlogonParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";
        private const string PolicySys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string Lsa = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "lgncache-disable-cached-logons",
                Label = "Logon Cache: Disable Cached Domain Logons (0 Cached)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "cache", "domain", "no-cache", "security", "hardening"],
                Description =
                    "Sets CachedLogonsCount=0 in Winlogon. Completely disables domain credential caching. "
                    + "Users must have network connectivity to log on with domain credentials. "
                    + "Default: 10. Hardest security posture — use only in always-connected environments.",
                ApplyOps = [RegOp.SetString(Winlogon, "CachedLogonsCount", "0")],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "CachedLogonsCount")],
                DetectOps = [RegOp.CheckString(Winlogon, "CachedLogonsCount", "0")],
            },
            new TweakDef
            {
                Id = "lgncache-sc-remove-lock",
                Label = "Logon Cache: Lock Workstation on Smart Card Removal",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "smart-card", "lock", "security", "pki", "compliance"],
                Description =
                    "Sets ScRemoveOption=1 in Winlogon. Locks the workstation immediately when the smart "
                    + "card used for authentication is removed from the reader. "
                    + "Default: 0 (no action). Value 1=Lock, 2=Force Logoff. Recommended: 1 for secure workstations.",
                ApplyOps = [RegOp.SetString(Winlogon, "ScRemoveOption", "1")],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "ScRemoveOption")],
                DetectOps = [RegOp.CheckString(Winlogon, "ScRemoveOption", "1")],
            },
            new TweakDef
            {
                Id = "lgncache-password-expiry-warning-14d",
                Label = "Logon Cache: Set Password Expiry Warning to 14 Days",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Winlogon],
                Tags = ["logon", "password", "expiry", "warning", "policy", "compliance"],
                Description =
                    "Sets PasswordExpiryWarning=14 in Winlogon. Shows password expiry reminder 14 days "
                    + "before the password expires at logon time. "
                    + "Default: 5 days. 14 days gives users adequate time to change before lockout.",
                ApplyOps = [RegOp.SetDword(Winlogon, "PasswordExpiryWarning", 14)],
                RemoveOps = [RegOp.DeleteValue(Winlogon, "PasswordExpiryWarning")],
                DetectOps = [RegOp.CheckDword(Winlogon, "PasswordExpiryWarning", 14)],
            },
        ];
    }

    // ── LogonGpoPolicy ──
    private static class _LogonGpoPolicy
    {
        private const string LogonSys = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "logonpol-hide-last-username",
                Label = "Hide last signed-in username at logon screen (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents the last interactive user's username from being displayed on the logon screen. "
                    + "DontDisplayLastUserName=1. Protects user enumeration on shared/kiosk machines.",
                Tags = ["logon", "privacy", "username", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayLastUserName", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayLastUserName")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayLastUserName", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-disable-arso",
                Label = "Disable Automatic Restart Sign-On (ARSO) (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables Windows Automatic Restart Sign-On, which re-signs in the last user after an update restart. "
                    + "DisableAutomaticRestartSignOn=1. Prevents unattended desktop exposure after reboot.",
                Tags = ["logon", "arso", "restart", "autologon", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DisableAutomaticRestartSignOn", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableAutomaticRestartSignOn")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DisableAutomaticRestartSignOn", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-disable-startup-sound",
                Label = "Disable Windows startup sound (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables the Windows startup sound via policy, regardless of user sound settings. "
                    + "DisableStartupSound=1. Useful in enterprise/kiosk environments.",
                Tags = ["logon", "startup", "sound", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DisableStartupSound", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableStartupSound")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DisableStartupSound", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-block-msa-connected-account",
                Label = "Block Microsoft Account connected users (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Microsoft Account-connected users from signing in. "
                    + "NoConnectedUser=3 (block all MSA users). Values: 0=allowed, 1=no new MSA, 3=block all MSA.",
                Tags = ["logon", "msa", "account", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "NoConnectedUser", 3)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "NoConnectedUser")],
                DetectOps = [RegOp.CheckDword(LogonSys, "NoConnectedUser", 3)],
            },
            new TweakDef
            {
                Id = "logonpol-hide-locked-user-id",
                Label = "Hide locked user info on the lock screen (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Controls what user information is displayed when a session is locked. "
                    + "DontDisplayLockedUserId=3 (show nothing: display name, domain, and username hidden). "
                    + "Values: 1=display name only, 2=display name+domain, 3=nothing.",
                Tags = ["logon", "lock-screen", "privacy", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DontDisplayLockedUserId", 3)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DontDisplayLockedUserId")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DontDisplayLockedUserId", 3)],
            },
            new TweakDef
            {
                Id = "logonpol-max-device-password-failed-attempts",
                Label = "Lock device after failed sign-in attempts (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Triggers a device lockout after a specified number of failed sign-in attempts. "
                    + "MaxDevicePasswordFailedAttempts=10. Default: 0 (disabled). "
                    + "Activates on tablets/convertibles with BitLocker.",
                Tags = ["logon", "lockout", "password", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "MaxDevicePasswordFailedAttempts", 10)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "MaxDevicePasswordFailedAttempts")],
                DetectOps = [RegOp.CheckDword(LogonSys, "MaxDevicePasswordFailedAttempts", 10)],
            },
            new TweakDef
            {
                Id = "logonpol-disable-lock-screen-app-notifications",
                Label = "Disable app notifications on the lock screen (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents application notifications from appearing on the lock screen. "
                    + "DisableLockScreenAppNotifications=1. Default: not set. "
                    + "Reduces information disclosure before authentication.",
                Tags = ["logon", "lock-screen", "notifications", "privacy", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "DisableLockScreenAppNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "DisableLockScreenAppNotifications")],
                DetectOps = [RegOp.CheckDword(LogonSys, "DisableLockScreenAppNotifications", 1)],
            },
            new TweakDef
            {
                Id = "logonpol-hide-power-button-at-logon",
                Label = "Hide power button on logon screen (policy)",
                Category = "User Account — Kerberos Encryption",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Removes the Shut Down/Restart button from the Windows logon screen. "
                    + "HideFastUserSwitching=0 (keep switching; this key is separate). "
                    + "PowerButtonDenied=1 prevents shutdown before sign-in on kiosk machines.",
                Tags = ["logon", "power", "kiosk", "policy"],
                ApplyOps = [RegOp.SetDword(LogonSys, "PowerButtonDenied", 1)],
                RemoveOps = [RegOp.DeleteValue(LogonSys, "PowerButtonDenied")],
                DetectOps = [RegOp.CheckDword(LogonSys, "PowerButtonDenied", 1)],
            },
        ];
    }

    // ── LsaProtectionPolicy ──
    private static class _LsaProtectionPolicy
    {
        private const string LsaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CredentialsDelegation";
        private const string LsaSysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsNT\CurrentVersion\Winlogon";
        private const string LsaCtrl = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\LSA";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lsapol-enable-lsa-run-as-ppl",
                    Label = "Enable LSA Run as Protected Process Light (PPL)",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Configures lsass.exe to run as a Protected Process Light (PPL). PPL enforces ELAM (Early Launch Anti-Malware) restrictions: only Microsoft-signed binaries can inject into or read lsass memory. Directly prevents Mimikatz credential dumping.",
                    Tags = ["lsa", "ppl", "credential-dump", "mimikatz", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Most impactful anti-credential-theft control available via policy. Prevents all unsigned in-memory credential extraction tools. Requires Windows 8.1+ and may conflict with unsigned AV products.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "RunAsPPL", 2)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RunAsPPL")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "RunAsPPL", 2)],
                },
                new TweakDef
                {
                    Id = "lsapol-audit-lsass-access-attempts",
                    Label = "Audit LSASS Memory Access Attempts",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Enables audit logging of all OpenProcess calls that attempt to read lsass.exe memory. Even without PPL enforcement this detects credential-dumping tools (Mimikatz, ProcDump /ma) and logs the calling process for SIEM analysis.",
                    Tags = ["lsa", "audit", "credential-dump", "memory-access", "event-log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Non-disruptive; adds event-log entries only. Essential for detection of credential theft attempts even before PPL is enforced.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "AuditLSASSAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "AuditLSASSAccess")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "AuditLSASSAccess", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-reversible-encryption",
                    Label = "Disable Reversible Password Encryption in LSA",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Prevents Windows from storing user passwords in LSA using reversible encryption. Reversible password storage is equivalent to plaintext storage; disabling it ensures only one-way NTLM hashes are retained in the SAM database.",
                    Tags = ["lsa", "password-storage", "reversible-encryption", "sam", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents reversible-cleartext password storage in SAM. Users who had reversible passwords must reset after this is applied. No operational impact if reversible encryption was never enabled.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "DisableReversibleEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "DisableReversibleEncryption")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "DisableReversibleEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-block-credential-delegation-to-unknown",
                    Label = "Block Credential Delegation to Unknown or Untrusted Servers",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Denies credential delegation (CredSSP / Kerberos constrained delegation) to servers not explicitly listed in the trusted servers allowlist. Prevents pass-the-hash relay attacks that trick the client into delegating credentials to a rogue server.",
                    Tags = ["lsa", "credential-delegation", "credssp", "relay", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Blocks credential delegation to all servers not explicitly allowlisted. Remote Desktop connections to unlisted servers will prompt for credentials rather than passing them. Maintain the delegation allowlist in GPO.",
                    RegistryKeys = [LsaKey],
                    ApplyOps = [RegOp.SetDword(LsaKey, "AllowDefCredentials", 0)],
                    RemoveOps = [RegOp.DeleteValue(LsaKey, "AllowDefCredentials")],
                    DetectOps = [RegOp.CheckDword(LsaKey, "AllowDefCredentials", 0)],
                },
                new TweakDef
                {
                    Id = "lsapol-restrict-anonymous-lsa-access",
                    Label = "Restrict Anonymous LSA Name and Account Lookups",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Prevents anonymous connections (null sessions) from enumerating LSA account names, SIDs, and local group memberships. Blocks the reconnaissance phase of account enumeration attacks.",
                    Tags = ["lsa", "anonymous-access", "null-session", "enumeration", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Prevents unauthenticated users from querying account info via null sessions. Blocks legacy management tools and scanners that rely on anonymous LSA queries.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "RestrictAnonymous", 2)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RestrictAnonymous")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "RestrictAnonymous", 2)],
                },
                new TweakDef
                {
                    Id = "lsapol-disable-lm-hash-storage",
                    Label = "Disable LM Hash Storage in LSA Credential Store",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Permanently disables storage of LAN Manager (LM) password hashes in the LSA credential cache. LM hashes are solvable in seconds with modern GPUs; this ensures only NTLM and Kerberos hashes are retained.",
                    Tags = ["lsa", "lm-hash", "ntlm", "password-hash", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "All future password changes stop producing LM hashes. Existing hashes remain until users change passwords. Eliminates the weakest credential artifact from the credential store.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "NoLmHash", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "NoLmHash")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "NoLmHash", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-enable-securechannel-sealing",
                    Label = "Require Secure Channel Data Encryption and Signing",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Forces all Netlogon secure channel traffic to be encrypted and signed. A secure channel is the authenticated tunnel between a domain member and its DC; unsigned channels can be hijacked to inject forged authentication responses.",
                    Tags = ["lsa", "netlogon", "secure-channel", "encryption", "signing"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Ensures Netlogon/DC communication is tamper-proof. Old DCs that do not support signed secure channels will refuse connections; requires Server 2012+ DCs.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "RequireSignOrSeal", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "RequireSignOrSeal")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "RequireSignOrSeal", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-restrict-cached-logons",
                    Label = "Restrict Cached Domain Logon Count to 1",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Limits the number of cached domain credential sets stored in the LSA to 1 (minimum). Cached logons allow domain users to authenticate offline; a high cache count means a physical attacker can harvest multiple domain hashes from a stolen laptop.",
                    Tags = ["lsa", "cached-logon", "credential-cache", "physical-security", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Reduces cached credential exposure from offline attacks. Setting to 1 means only the most recent login is cached; users who have not logged in recently cannot authenticate offline.",
                    RegistryKeys = [LsaSysKey],
                    ApplyOps = [RegOp.SetString(LsaSysKey, "CachedLogonsCount", "1")],
                    RemoveOps = [RegOp.DeleteValue(LsaSysKey, "CachedLogonsCount")],
                    DetectOps = [RegOp.CheckString(LsaSysKey, "CachedLogonsCount", "1")],
                },
                new TweakDef
                {
                    Id = "lsapol-enable-credential-guard",
                    Label = "Enable Windows Defender Credential Guard (Isolated LSA)",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Enables Credential Guard, which runs the LSA credential store inside a secure Hyper-V isolated container (VSM). Even if the host OS is fully compromised lsass cannot be dumped because credentials live in a separate VM-protected memory region.",
                    Tags = ["lsa", "credential-guard", "vsm", "secure-enclave", "advanced"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Most powerful LSA credential protection; renders Mimikatz-class attacks ineffective. Requires UEFI Secure Boot + VBS + HVCI. Check hardware compatibility before deploying.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "LsaCfgFlags", 1)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "LsaCfgFlags")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "LsaCfgFlags", 1)],
                },
                new TweakDef
                {
                    Id = "lsapol-block-wdigest-plaintext-creds",
                    Label = "Block WDigest from Storing Plaintext Credentials in LSASS",
                    Category = "User Account — Kerberos Encryption",
                    Description =
                        "Disables WDigest authentication protocol caching in LSASS memory. WDigest was designed for HTTP Digest authentication and cached plaintext-equivalent credentials in LSASS; attackers (Mimikatz sekurlsa::wdigest) can extract these.",
                    Tags = ["lsa", "wdigest", "plaintext", "credential-cache", "mimikatz"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Eliminates plaintext credential storage in LSASS with zero functional impact for modern Windows. WDigest is only needed by systems running very old IIS Digest authentication — rare in practice.",
                    RegistryKeys = [LsaCtrl],
                    ApplyOps = [RegOp.SetDword(LsaCtrl, "UseLogonCredential", 0)],
                    RemoveOps = [RegOp.DeleteValue(LsaCtrl, "UseLogonCredential")],
                    DetectOps = [RegOp.CheckDword(LsaCtrl, "UseLogonCredential", 0)],
                },
            ];
    }

    // ── PasswordlessSignInPolicy ──
    private static class _PasswordlessSignInPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pwdless-disable-password-fallback",
                    Label = "Disable Password Fallback for WHfB Sign-In",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents users from falling back to password authentication when WHfB is available, forcing passwordless primary sign-in and eliminating the password as a secondary path attackers could target.",
                    Tags = ["whfb", "passwordless", "password-fallback", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Password fallback blocked for WHfB; users must use biometric or PIN even if they remember their password.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePasswordFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordFallback")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePasswordFallback", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-enable-fido2-keys",
                    Label = "Enable FIDO2 Security Key Sign-In",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Enables FIDO2 hardware security keys (YubiKey, Titan key, etc.) as a credential type for Windows sign-in alongside WHfB, allowing phishing-resistant passwordless authentication from the lock screen.",
                    Tags = ["fido2", "security-key", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "FIDO2 security keys accepted at Windows lock screen; users can log in with a hardware key.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableFIDO2SecurityKeys", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableFIDO2SecurityKeys")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableFIDO2SecurityKeys", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-disable-convenience-pin",
                    Label = "Disable Convenience PIN (Non-WHfB) for Local Accounts",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Disables the simple convenience PIN for local accounts that does not benefit from WHfB's asymmetric key protection, forcing WHfB PIN (TPM-backed) for all PIN sign-in scenarios.",
                    Tags = ["whfb", "convenience-pin", "local-account", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Convenience PIN disabled; PIN sign-in only available through WHfB with TPM backing.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUnprovisionedPins", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUnprovisionedPins")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUnprovisionedPins", 0)],
                },
                new TweakDef
                {
                    Id = "pwdless-block-phone-sign-in",
                    Label = "Block Phone/Companion Device Sign-In",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Disables the companion device (phone-based Windows Hello sign-in) framework, preventing authentication via Bluetooth phone approval where physical possession of the phone is the only factor.",
                    Tags = ["whfb", "phone-sign-in", "companion-device", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Phone/companion-device sign-in disabled; must use device-local WHfB or security key.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePhoneSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePhoneSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-require-mfa-for-whfb-provision",
                    Label = "Require MFA During WHfB Provisioning",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Requires multi-factor authentication during the WHfB provisioning ceremony, ensuring that only users who have already authenticated with a second factor can register WHfB credentials.",
                    Tags = ["whfb", "mfa", "provisioning", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "MFA required during WHfB setup; single-factor users cannot provision WHfB credentials.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireMFAForProvisioning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireMFAForProvisioning")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireMFAForProvisioning", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-block-whfb-on-unmanaged",
                    Label = "Block WHfB Enrollment on Unmanaged Devices",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents Windows Hello for Business enrollment on devices not enrolled in an MDM policy (Intune/SCCM), ensuring WHfB credentials are only provisioned on corp-managed endpoints.",
                    Tags = ["whfb", "mdm", "managed-device", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WHfB blocked on unmanaged devices; enrollment only succeeds after MDM enrolment.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockEnrollmentOnUnmanagedDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockEnrollmentOnUnmanagedDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockEnrollmentOnUnmanagedDevice", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-disable-whfb-personal",
                    Label = "Disable WHfB for Personal Microsoft Accounts",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents Windows Hello from being used for personal Microsoft account sign-in, restricting WHfB to work or school accounts only and preventing personal-account credential leakage.",
                    Tags = ["whfb", "personal-account", "microsoft-account", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB disabled for personal MSA; Windows Hello sign-in restricted to Azure AD accounts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePersonalAccountPassport", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalAccountPassport")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePersonalAccountPassport", 1)],
                },
                new TweakDef
                {
                    Id = "pwdless-audit-whfb-provisioning",
                    Label = "Enable Audit Logging for WHfB Provisioning Events",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Enables security audit events for all WHfB provisioning, re-provisioning, and credential deletion events, providing traceability for passwordless credential lifecycle management.",
                    Tags = ["whfb", "audit-log", "provisioning", "passwordless", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB credential lifecycle events logged in Security event log.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditProvisioningEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditProvisioningEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditProvisioningEvents", 1)],
                },
            ];
    }

    // ── SmartCardCredentialsPolicy ──
    private static class _SmartCardCredentialsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sccredpol-allow-certificates-with-no-extended-key-usage",
                    Label = "SC Credentials: Allow Smart Card Certificates Without Extended Key Usage for Logon",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets AllowCertificatesWithNoEKU=0 in Smart Card Credential Provider policy. Prevents smart card certificates without an Extended Key Usage (EKU) extension — or with an EKU that doesn't include Client Authentication (1.3.6.1.5.5.7.3.2) — from being used for Windows logon. "
                        + "Smart card certificates without an EKU or with an all-inclusive EKU (Any Purpose) are certificates that were issued without specifying a legitimate use constraint. Such certificates are typically misconfigured CA root certificates or test certificates. If Windows allows logon with any certificate present on a smart card regardless of EKU, an attacker who compromises a user's smart card PIN and inserts a root CA certificate or code signing certificate into the card can attempt logon with the inappropriate certificate. Requiring Client Authentication EKU ensures only purpose-constrained logon certificates can authenticate to interactive sessions.",
                    Tags = ["sccredpol", "smart-card", "eku", "certificate", "logon", "client-auth"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card certificates must have Client Authentication EKU for interactive logon. Misconfigured test certs or CA-root certs cannot authenticate.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCertificatesWithNoEKU", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCertificatesWithNoEKU")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCertificatesWithNoEKU", 0)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enforce-certificate-time-validity",
                    Label = "SC Credentials: Reject Expired Smart Card Certificates from Logon",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets EnforceCAExpiry=1 in Smart Card Credential Provider policy. Enforces certificate validity period checking — prevents Windows from accepting smart card certificates for logon that have expired or whose issuing CA certificate chain has expired. By default, Windows may allow logon with expired smart card certificates in some scenarios (offline cached logon) if the certificate was previously valid. "
                        + "Expired certificates represent an operational risk in smart card deployments: when a user's smart card certificate expires but the card PIN remains valid, Windows may continue to accept the card for domain logon relying on cached credentials — even though the PKI infrastructure considers the certificate expired. An attacker who obtains an expired certificate and the corresponding private key (from a compromised card) can attempt offline certificate logon. EnforceCAExpiry=1 ensures the current certificate validity timestamp is always checked, preventing expired certificate acceptance even in cached credential scenarios.",
                    Tags = ["sccredpol", "smart-card", "certificate-expiry", "ca-expiry", "validity", "pki"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Expired smart card certificates rejected. Users with expired certificates must renew before interactive logon works. Ensure certificate renewal reminders are in place.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceCAExpiry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceCAExpiry")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceCAExpiry", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-filter-duplicate-certificates",
                    Label = "SC Credentials: Filter Duplicate Smart Card Certificates Shown in Logon Picker",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets FilterDuplicateCerts=1 in Smart Card Credential Provider policy. When a smart card contains multiple certificates with the same Subject and public key (e.g., during certificate renewal where both old and new certificates co-exist on the card), this setting shows only the most recently issued certificate in the Windows logon certificate picker, preventing user confusion from duplicate entries. "
                        + "During smart card certificate lifecycle management, cards frequently transition through a state where both the old (near-expired) and new (freshly issued) certificates are on the card simultaneously — to allow the renewal to proceed without requiring the user to surrender their card. The Windows logon certificate picker displays all certificates, presenting two identical-looking entries to the user. Users who select the expired certificate will experience logon failures. FilterDuplicateCerts reduces the duplicate entries to one (the most recent), eliminating this user experience issue.",
                    Tags = ["sccredpol", "smart-card", "duplicate-certificate", "certificate-renewal", "logon-picker"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Duplicate smart card certificates filtered in logon picker. Only most recently issued certificate shown when multiple share the same subject.",
                    ApplyOps = [RegOp.SetDword(Key, "FilterDuplicateCerts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "FilterDuplicateCerts")],
                    DetectOps = [RegOp.CheckDword(Key, "FilterDuplicateCerts", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-force-read-all-certificates",
                    Label = "SC Credentials: Force Reading All Certificates from Smart Card (Not Just Root/Signing)",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets ForceReadingAllCertificates=1 in Smart Card Credential Provider policy. Forces Windows to read all certificates stored on the smart card during authentication enumeration, rather than only examining the first matching certificate. Some cards store certificate-based logon credentials on non-default slots or with non-standard EKU ordering — without ForceReadingAllCertificates, Windows may skip valid authentication certificates. "
                        + "Smart card credential providers have an optimisation that stops scanning the card after finding the first usable certificate. On cards with multiple valid Client Authentication certificates (multi-profile cards, cards issued by different CAs for different resource domains), the optimisation may select a certificate for a different trust domain, causing failed authentication. ForceReadingAllCertificates ensures the complete certificate set is enumerated and the credential provider selects the certificate with the best chain match for the current domain.",
                    Tags = ["sccredpol", "smart-card", "certificate-enumeration", "multi-profile", "credential-provider"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "All smart card certificates read and enumerated. Slight performance increase per logon attempt; negligible on modern smart card readers.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceReadingAllCertificates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceReadingAllCertificates")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceReadingAllCertificates", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-require-smart-card-for-logon",
                    Label = "SC Credentials: Require Smart Card for Interactive Logon (Disable Password-Based Logon)",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets ScForceOption=1 in Windows System policy. Requires users to authenticate with a smart card for interactive (local and Remote Desktop) logon. Password-based interactive logon is disabled. This setting is the full enforcement of a smart card-mandatory authentication policy — ensuring that physical possession of the smart card is required for every interactive logon event, eliminating password-based bypass paths. "
                        + "Password-based logon as a fallback for smart card environments creates a persistent weak authentication path: users who 'lose' their smart card can fall back to passwords, which are substantially easier to steal via phishing or shoulder surfing than compromising a physical authentication token plus PIN. In high-assurance environments (financial trading, government classified systems, nuclear facility IT, PCI DSS Level 1), all interactive logon must be protected by a physical authentication factor. ScForceOption=1 eliminates the password fallback and enforces the physical factor requirement absolutely.",
                    Tags = ["sccredpol", "smart-card", "force-logon", "disable-password-logon", "mfa", "high-assurance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 2,
                    ImpactNote =
                        "BREAKING: Password interactive logon fully disabled. Smart card REQUIRED for all logon. Ensure all users have working smart cards and readers before deployment. Service accounts need smartcard exemption.",
                    ApplyOps = [RegOp.SetDword(SysKey, "ScForceOption", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "ScForceOption")],
                    DetectOps = [RegOp.CheckDword(SysKey, "ScForceOption", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enable-smart-card-lock-on-removal",
                    Label = "SC Credentials: Lock Workstation Automatically When Smart Card is Removed",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets SmartCardRemovalOption=1 in Windows System policy. Automatically locks the workstation when the user removes their smart card from the reader, replacing the 'no action' default. Ensures the workstation is immediately locked when the user physically departs (smart card is typically in their lanyard or pocket which they take with them). "
                        + "Smart card removal detection is a behavioural lock triggered by physical possession of the authentication token. The security premise: a person who removes their smart card from the reader is physically leaving the workstation. Without removal lock, the authenticated session remains unlocked and accessible to anyone who approaches the workstation during the user's brief absence (printer, coffee, restroom). SmartCardRemovalOption=1 means the session locks within seconds of card removal — the physical authentication token acts as a proximity-based session lock device.",
                    Tags = ["sccredpol", "smart-card", "removal-lock", "session-lock", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Workstation locks immediately on smart card removal. Users who briefly remove their card for any reason will need to re-insert and re-authenticate.",
                    ApplyOps = [RegOp.SetDword(SysKey, "SmartCardRemovalOption", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "SmartCardRemovalOption")],
                    DetectOps = [RegOp.CheckDword(SysKey, "SmartCardRemovalOption", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-disable-smart-card-credential-caching",
                    Label = "SC Credentials: Disable Windows Cached Credentials for Smart Card Logons",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets DisableSmartCardLogonCheck=0 in Smart Card Credential Provider policy. Ensures Windows performs a full smart card authentication challenge on every logon attempt — disabling any cached credential shortcut paths that might allow logon without re-validating the current smart card state against the DC. "
                        + "Cached credential logon for smart card authentication creates an inconsistency: the cached domain credential may be valid even after the smart card certificate has been revoked (e.g., following employee termination or card loss). If Windows allows cached credential logon for smart card sessions, a terminated employee's workstation retains the logon capability for up to the domain cache lifetime (default 10 cached logons). Ensuring full smart card validation on each logon forces certificate revocation to be effective immediately — revoked smart cards are rejected on first logon attempt after CRL update.",
                    Tags = ["sccredpol", "smart-card", "credential-cache", "revocation", "crl", "terminated-employee"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Full smart card DC validation required. Offline logon (no DC reachable) requires network connectivity. Deploy alongside always-on VPN for remote workers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSmartCardLogonCheck", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartCardLogonCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSmartCardLogonCheck", 0)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enable-smart-card-puk-logging",
                    Label = "SC Credentials: Enable Smart Card PUK/PIN Operation Logging",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets EnableSmartCardLogonLogging=1 in Smart Card Credential Provider policy. Enables logging of smart card PIN entry events, PUK (PIN Unblocking Key) operations, and certificate selection events to the Windows Application event log. PIN operation logging provides an audit trail of smart card authentication activity at the workstation — enabling detection of PIN brute-force attempts (excessive failed PIN entries), card blocking events (PUK operation triggered), and certificate selection anomalies. "
                        + "Smart card PIN brute-force attacks are rate-limited by card hardware (typically 3-10 failed attempts before card lockout), but without logging, an attacker who attempts multiple combinations across the threshold boundary and reinserts the card leaves no system event trace. Smart card logging events can be collected by SIEM, enabling detection of cards that are being tested for PIN guessing (rapid sequence of failed PIN events at an unexpected workstation), identifying potentially compromised or stolen cards before the card locks.",
                    Tags = ["sccredpol", "smart-card", "logging", "pin-brute-force", "puk", "audit"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card PIN and PUK events logged to Application event log. SIEM collection of card-specific events enables PIN brute-force detection.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableSmartCardLogonLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableSmartCardLogonLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableSmartCardLogonLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-restrict-to-root-trusted-certificates",
                    Label = "SC Credentials: Restrict Smart Card Logon to Root-CA Trusted Certificates Only",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets RootCA=1 in Smart Card Credential Provider policy. Restricts smart card logon to only accept certificates that chain to a root CA in the machine's Trusted Root Certification Authorities store — preventing certificates issued by intermediate-only CAs or enterprise subordinate CAs whose root is not in the machine trust store from being used for logon. "
                        + "In multi-forest or partner organisation environments, smart cards issued by external PKI hierarchies may be physically interoperable (same card form factor, compatible reader drivers) but should not grant logon access to the local domain unless their issuing CA root is explicitly trusted. Without RootCA=1, certificates from any technically valid PKI chain — including self-signed certificates added to a card by an attacker — could be used for logon. Restricting to root-CA-trusted certs ensures the local domain trust policy governs which PKI hierarchies are authorised for smart card authentication.",
                    Tags = ["sccredpol", "smart-card", "root-ca", "trust", "pki", "cross-forest"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card certs must chain to machine trust store root CA. Self-signed and untrusted-root certificates rejected for logon.",
                    ApplyOps = [RegOp.SetDword(Key, "RootCA", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RootCA")],
                    DetectOps = [RegOp.CheckDword(Key, "RootCA", 1)],
                },
                new TweakDef
                {
                    Id = "sccredpol-enable-integrated-unblock",
                    Label = "SC Credentials: Enable Integrated Smart Card Unblock Screen at Logon",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets EnableIntegratedUnblock=1 in Smart Card Credential Provider policy. Enables the Windows integrated smart card unblock screen — presented at the Ctrl+Alt+Del logon screen when a smart card's PIN is blocked (after exceeding the incorrect PIN attempt limit). The integrated unblock screen allows users to unblock their card at the logon screen using PUK without requiring a separate unblock tool or helpdesk intervention. "
                        + "Without integrated unblock, a user whose card PIN is blocked must call the IT helpdesk, be issued a temporary PUK, and use a separate smart card management utility to unblock the card. This process typically takes 15–60 minutes depending on helpdesk availability. The integrated unblock screen presents the PUK entry interface directly at the Windows logon screen — the user provides their PUK and new PIN, the card is immediately unblocked, and logon proceeds. EnableIntegratedUnblock reduces helpdesk call volume for card lockouts by eliminating the manual unblock workflow.",
                    Tags = ["sccredpol", "smart-card", "unblock", "puk", "helpdesk", "user-experience"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Smart card unblock screen shown at Windows logon when PIN is blocked. Users can self-service PUK entry. Reduces helpdesk calls for locked cards.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableIntegratedUnblock", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegratedUnblock")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableIntegratedUnblock", 1)],
                },
            ];
    }

    // ── SmartCardCredProvPolicy ──
    private static class _SmartCardCredProvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "scprov-block-signature-only-keys",
                    Label = "Block Signature-Only Smart Card Keys",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents smart cards with signature-only keys from being used for interactive logon. Signature keys should not be used for authentication. Default: 1 (allow). Recommended: 0 (block).",
                    Tags = ["smart-card", "pki", "signature", "key-usage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enforces key usage separation; signature keys cannot be used for authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowSignatureOnlyKeys", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowSignatureOnlyKeys")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowSignatureOnlyKeys", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-block-time-invalid-certs",
                    Label = "Block Expired Smart Card Certificates",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents authentication using time-invalid (expired or not yet valid) smart card certificates. Enforces certificate lifecycle compliance. Default: 1 (allow). Recommended: 0 (block).",
                    Tags = ["smart-card", "pki", "expiry", "certificate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Ensures all authenticating certificates are within their validity period.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowTimeInvalidCertificates", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowTimeInvalidCertificates")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowTimeInvalidCertificates", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-enumerate-ecc-certs",
                    Label = "Enumerate ECC Certificates by Default",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Enables enumeration of elliptic-curve cryptography certificates on smart cards by default. Required when the organisation uses ECDSA/ECDH smart card certificates. Default: 0. Recommended: 1 when ECC certs are deployed.",
                    Tags = ["smart-card", "ecc", "pki", "enumeration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables ECC-issued certificates on smart cards to appear in the logon picker.",
                    ApplyOps = [RegOp.SetDword(Key, "EnumerateECCCerts", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnumerateECCCerts")],
                    DetectOps = [RegOp.CheckDword(Key, "EnumerateECCCerts", 1)],
                },
                new TweakDef
                {
                    Id = "scprov-no-reverse-subject",
                    Label = "Normalise Certificate Subject Display Order",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents the credential provider from reversing certificate subject field order in the logon UI. Ensures consistent CN/OU display regardless of CA issuance order. Default: not set. Recommended: 0 (normal order).",
                    Tags = ["smart-card", "subject", "display", "certificate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Display normalisation only; no functional security impact on authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "ReverseSubject", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ReverseSubject")],
                    DetectOps = [RegOp.CheckDword(Key, "ReverseSubject", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-suppress-x509-hints",
                    Label = "Suppress X.509 Certificate Hint Display",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Suppresses X.509 certificate hint prompts shown when multiple certificates are available during smart card logon. Reduces UI noise in managed environments. Default: 1. Recommended: 0.",
                    Tags = ["smart-card", "x509", "hint", "logon", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes extra X.509 hint dialogs during smart card authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "X509HintsNeeded", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "X509HintsNeeded")],
                    DetectOps = [RegOp.CheckDword(Key, "X509HintsNeeded", 0)],
                },
                new TweakDef
                {
                    Id = "scprov-disallow-plaintext-pin",
                    Label = "Disallow Plaintext Smart Card PIN Transmission",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents smart card PINs from being returned or transmitted in clear text by the Credential Manager. Critical for preventing PIN interception on hosts with memory inspection. Default: 0. Recommended: 1.",
                    Tags = ["smart-card", "pin", "plaintext", "credential", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Prevents PIN interception; may break legacy applications that depend on plaintext PIN access.",
                    ApplyOps = [RegOp.SetDword(Key, "DisallowPlaintextPin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisallowPlaintextPin")],
                    DetectOps = [RegOp.CheckDword(Key, "DisallowPlaintextPin", 1)],
                },
                new TweakDef
                {
                    Id = "scprov-logon-hours-notify",
                    Label = "Enable Logon Hours Change Notification",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Notifies users when their allowed logon hours are about to expire or have changed, using smart card credential context. Helps users save work before forced logoff. Default: 0. Recommended: 1.",
                    Tags = ["smart-card", "logon-hours", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Improves user experience in environments with logon-hour restrictions.",
                    ApplyOps = [RegOp.SetDword(Key, "LogonHoursNotificationEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogonHoursNotificationEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "LogonHoursNotificationEnabled", 1)],
                },
            ];
    }

    // ── WebAuthnPolicy ──
    private static class _WebAuthnPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WebAuthn";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wauthn-disable-touch-id-fallback",
                Label = "Disable WebAuthn Biometric Fallback to PIN",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableBiometricFallback=1 in the WebAuthn policy key. Prevents the "
                    + "Windows Hello FIDO2 implementation from falling back to a PIN when the "
                    + "biometric authenticator (fingerprint/face) is unavailable. Silent PIN "
                    + "fallback bypasses the stronger biometric factor and can be induced by "
                    + "covering the sensor. Administrators can enforce biometric-only "
                    + "authentication by disabling the fallback. "
                    + "Default: 0. Recommended: 1 in high-assurance environments.",
                Tags = ["webauthn", "biometric", "fallback", "fido2", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBiometricFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBiometricFallback")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBiometricFallback", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-require-enterprise-attestation",
                Label = "Require Enterprise Attestation for FIDO Keys",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets RequireEnterpriseAttestation=1 in the WebAuthn policy key. Forces "
                    + "FIDO2 authenticators to provide enterprise attestation statements that "
                    + "include the device's serial number and enterprise-registered key. "
                    + "This allows the relying party to verify that only managed hardware "
                    + "keys are used for authentication, preventing consumer FIDO2 tokens "
                    + "from authenticating against enterprise resources. "
                    + "Default: 0. Recommended: 1 in enterprise environments.",
                Tags = ["webauthn", "attestation", "enterprise", "fido2", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireEnterpriseAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireEnterpriseAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "RequireEnterpriseAttestation", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-cross-origin-auth",
                Label = "Disable Cross-Origin WebAuthn Authentication",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableCrossOriginAuth=1 in the WebAuthn policy key. Prevents "
                    + "browser-based WebAuthn from completing authentication ceremonies where "
                    + "the requesting origin does not match the registered relying party ID. "
                    + "Cross-origin authentication is the basis for credential phishing via "
                    + "proxied login pages that relay FIDO2 assertions to the legitimate site. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "crossorigin", "fido2", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCrossOriginAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossOriginAuth")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCrossOriginAuth", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-password-auth-fallback",
                Label = "Disable WebAuthn Password Authentication Fallback",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisablePasswordFallback=1 in the WebAuthn policy key. Prevents "
                    + "Windows Hello and FIDO2 flows from offering a password sign-in link "
                    + "when a passkey authentication attempt fails. The password fallback "
                    + "silently downgrades the authentication assurance level and is commonly "
                    + "exploited via credential stuffing after an initial FIDO2 probing failure. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "password", "fallback", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePasswordFallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePasswordFallback")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePasswordFallback", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-cloud-passkey-sync",
                Label = "Disable Cloud Passkey Sync",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableCloudPasskeySync=1 in the WebAuthn policy key. Prevents "
                    + "Windows Hello from backing up passkey private keys to the Microsoft "
                    + "cloud for recovery and cross-device sync. Cloud-synced passkeys "
                    + "compromise the hardware-bound security model of FIDO2: the private "
                    + "key should never leave the authenticating device. "
                    + "Default: 0. Recommended: 1 in high-security environments.",
                Tags = ["webauthn", "passkey", "sync", "cloud", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudPasskeySync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPasskeySync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudPasskeySync", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-webauthn-telemetry",
                Label = "Disable WebAuthn Telemetry",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableWebAuthnTelemetry=1 in the WebAuthn policy key. Prevents "
                    + "the Windows WebAuthn API from sending usage events including registration "
                    + "attempts, authentication ceremony outcomes, and authenticator model data "
                    + "to Microsoft's telemetry endpoints. Authenticator model data can "
                    + "fingerprint the specific security key hardware in use. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWebAuthnTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWebAuthnTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWebAuthnTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-security-key-enrollment",
                Label = "Block Unauthorised Security Key Enrollment",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableSecurityKeyEnrollment=1 in the WebAuthn policy key. Prevents "
                    + "standard users from enrolling new FIDO2 security keys in Windows Hello "
                    + "without administrator approval. Unrestricted enrollment allows any "
                    + "physical key to be registered on a managed machine, potentially "
                    + "granting persistent hardware-authenticated access to an attacker who "
                    + "briefly had physical access. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "enrollment", "securitykey", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSecurityKeyEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSecurityKeyEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSecurityKeyEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-enforce-user-verification",
                Label = "Enforce User Verification for All WebAuthn Calls",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets EnforceUserVerification=1 in the WebAuthn policy key. Forces the "
                    + "Windows WebAuthn stack to set the UV (user verification) flag to "
                    + "required for every authentication call, overriding relying party "
                    + "requests that set UV to preferred or discouraged. Applications that "
                    + "skip user verification inherit the trust from proximity alone without "
                    + "requiring a PIN or biometric factor. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "verification", "fido2", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceUserVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceUserVerification")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceUserVerification", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-nfc-transport",
                Label = "Disable NFC Transport for FIDO2 Keys",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableNfcTransport=1 in the WebAuthn policy key. Prevents the "
                    + "Windows FIDO2 client from using NFC as a transport channel for security "
                    + "key communication. NFC has a shorter effective range than USB but "
                    + "NFC relay attacks are practical up to 100 m with commodity hardware. "
                    + "Restricting keys to USB-A/C contact transports eliminates this attack "
                    + "surface. Default: 0. Recommended: 1 where NFC keys are not mandated.",
                Tags = ["webauthn", "nfc", "transport", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNfcTransport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNfcTransport")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNfcTransport", 1)],
            },
            new TweakDef
            {
                Id = "wauthn-disable-bluetooth-transport",
                Label = "Disable Bluetooth Transport for FIDO2 Keys",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableBluetoothTransport=1 in the WebAuthn policy key. Disables "
                    + "Bluetooth Low Energy as a FIDO2 authenticator transport. BLE-based "
                    + "authenticators (e.g., phone-as-key) are vulnerable to Bluetooth relay "
                    + "attacks where the attacker proxies BLE advertisements to extend the "
                    + "effective range of the authenticator beyond the user's awareness. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["webauthn", "bluetooth", "transport", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBluetoothTransport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBluetoothTransport")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBluetoothTransport", 1)],
            },
        ];
    }

    // ── WhfbPinPolicy ──
    private static class _WhfbPinPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork\PINComplexity";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "whfbpin-set-maximum-length-16",
                    Label = "Set WHfB PIN Maximum Length to 16 Digits",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets the maximum Windows Hello for Business PIN length to 16 characters, balancing usability with security and preventing excessively long PINs that users may forget.",
                    Tags = ["whfb", "windows-hello", "pin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN capped at 16 characters.",
                    ApplyOps = [RegOp.SetDword(Key, "MaximumPINLength", 16)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaximumPINLength")],
                    DetectOps = [RegOp.CheckDword(Key, "MaximumPINLength", 16)],
                },
                new TweakDef
                {
                    Id = "whfbpin-require-special-chars",
                    Label = "Require Special Characters in WHfB PIN",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Requires at least one special character in Windows Hello for Business PINs, maximising PIN entropy and preventing trivially guessable numeric or alphabetic patterns.",
                    Tags = ["whfb", "windows-hello", "pin", "special-chars", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN must include a special character; numeric-only PINs disallowed.",
                    ApplyOps = [RegOp.SetDword(Key, "SpecialCharacters", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SpecialCharacters")],
                    DetectOps = [RegOp.CheckDword(Key, "SpecialCharacters", 1)],
                },
                new TweakDef
                {
                    Id = "whfbpin-set-pin-history-5",
                    Label = "Set WHfB PIN History to 5 Previous PINs",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents reuse of the last 5 WHfB PINs, stopping users from cycling back to recently used PINs immediately after a mandatory PIN change.",
                    Tags = ["whfb", "windows-hello", "pin", "history", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Last 5 WHfB PINs remembered; PIN cannot be recycled until 5 unique PINs have been used.",
                    ApplyOps = [RegOp.SetDword(Key, "History", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "History")],
                    DetectOps = [RegOp.CheckDword(Key, "History", 5)],
                },
                new TweakDef
                {
                    Id = "whfbpin-block-simple-patterns",
                    Label = "Block Simple/Sequential WHfB PIN Patterns",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Blocks common sequential (1234, abcd) and repeated-character (1111, aaaa) PIN patterns for WHfB, preventing trivially guessable PINs from being set.",
                    Tags = ["whfb", "windows-hello", "pin", "patterns", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Simple PIN patterns blocked; sequential and repeated patterns rejected at PIN creation.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockSimplePatterns", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockSimplePatterns")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockSimplePatterns", 1)],
                },
                new TweakDef
                {
                    Id = "whfbpin-lockout-after-5-failures",
                    Label = "Lock Out WHfB PIN After 5 Failed Attempts",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Locks the WHfB PIN credential after 5 consecutive failed login attempts, requiring a PIN reset via recovery, defending against online brute-force attacks.",
                    Tags = ["whfb", "windows-hello", "pin", "lockout", "brute-force", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WHfB PIN locked after 5 failed attempts; reset required, stopping online PIN guessing.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxFailedAttempts", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxFailedAttempts")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxFailedAttempts", 5)],
                },
            ];
    }

    // ── WindowsHelloAdvPolicy ──
    private static class _WindowsHelloAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "helloadv-set-minimum-pin-length",
                Label = "Enforce Minimum PIN Length for Windows Hello Authentication",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows Hello PIN minimum length requirements prevent trivially guessable short PINs that could be brute-forced through shoulder surfing or trial and error. Enforcing a minimum PIN length of 6 or more digits significantly increases the difficulty of guessing or brute-forcing a Hello PIN. Hello PINs are protected by account lockout after a configurable number of failed attempts limiting brute-force attack effectiveness. Unlike passwords Hello PINs are device-specific meaning a leaked PIN cannot be used to authenticate from any device except the specific registered device. Organizations should set minimum PIN lengths to at least 6 characters and consider using enhanced PIN complexity requirements that include alphabetic and special characters for higher security requirements. PIN length requirements should be communicated clearly to users during Windows Hello enrollment to ensure users create appropriate PINs.",
                Tags = ["windows-hello", "pin", "complexity", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinimumPINLength", 6)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinimumPINLength")],
                DetectOps = [RegOp.CheckDword(Key, "MinimumPINLength", 6)],
            },
            new TweakDef
            {
                Id = "helloadv-enable-biometric-authentication",
                Label = "Enable Biometric Authentication for Windows Hello Sign-in",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows Hello biometric authentication allows users to sign in using facial recognition or fingerprint which provides both convenience and strong multi-factor authentication. Enabling biometric authentication for Hello gives users a fast and secure alternative to PIN-based authentication that is resistant to shoulder surfing and observation attacks. Windows Hello enhanced anti-spoofing requires compatible depth-sensing cameras that can distinguish a real face from a photograph. Biometric data collected by Windows Hello is stored only on the device and is protected by the TPM never leaving the device or being sent to Microsoft. Organizations should enable biometric authentication to improve user adoption of Windows Hello as users prefer biometric over PIN authentication when available. Fallback to PIN should be available for situations where biometric authentication fails to ensure users maintain secure access to their systems.",
                Tags = ["windows-hello", "biometric", "fingerprint", "face-recognition", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UseBiometrics", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UseBiometrics")],
                DetectOps = [RegOp.CheckDword(Key, "UseBiometrics", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-disable-hello-provisioning-on-shared-pcs",
                Label = "Disable Windows Hello Provisioning on Shared or Kiosk Devices",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Windows Hello provisioning on shared devices creates security concerns because multiple users sharing the same device may inadvertently expose each other's biometric data or PIN-protected credentials. Disabling Hello provisioning on shared PCs ensures that the authentication method is appropriate for the shared use context. Kiosk devices with a single fixed user identity should not provision individual Hello credentials as the device is not used for personalized authentication. Shared workstation scenarios in call centers or shared office spaces require careful evaluation of whether Hello provisioning is appropriate and beneficial. Devices configured in shared PC mode through the Shared PC CSP automatically disable Hello provisioning as part of the shared PC configuration. Organizations should identify all shared devices and apply appropriate Hello configuration rather than applying organization-wide provisioning settings.",
                Tags = ["windows-hello", "shared-pc", "kiosk", "provisioning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePostLogonProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePostLogonProvisioning")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePostLogonProvisioning", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-enable-certificate-trust",
                Label = "Enable Certificate Trust Model for Windows Hello for Business",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows Hello for Business certificate trust model issues certificate-based credentials from an enterprise PKI enabling authentication to systems and services that require certificate-based authentication. Certificate trust Hello for Business provides compatibility with existing PKI infrastructure and supports scenarios that require X.509 certificates for authentication. Key trust Hello for Business is simpler to deploy but does not support all legacy authentication scenarios that certificate trust supports. Organizations with complex PKI infrastructure and certificate-based authentication requirements should deploy certificate trust Hello for Business. Certificate trust requires AD FS for on-premises deployments or Azure AD for cloud deployments to issue certificates during Hello provisioning. The choice between key trust and certificate trust Hello should align with the organization's authentication requirements and PKI capabilities.",
                Tags = ["windows-hello", "certificate-trust", "pki", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UseCertificateForOnPremAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UseCertificateForOnPremAuth")],
                DetectOps = [RegOp.CheckDword(Key, "UseCertificateForOnPremAuth", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-set-pin-expiry",
                Label = "Set Maximum PIN Expiry Period for Windows Hello Authentication",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "PIN expiry for Windows Hello forces users to regularly change their device PINs ensuring that captured or observed PINs have limited useful lifetime. Setting PIN expiry to 60 days aligns with common password policy rotation schedules while acknowledging that PIN security differs from password security. Hello PINs are device-specific making them inherently more secure than domain passwords since a captured PIN cannot be used remotely from other devices. FIDO2 security frameworks generally do not recommend PIN expiry for Hello as the device binding provides sufficient security but organizations with compliance requirements may need to enforce rotation. PIN expiry requirements should be balanced against user friction to ensure users do not choose shorter or simpler PINs to ease memorization of frequently changing credentials. Organizations should evaluate whether the security benefit of PIN rotation outweighs the usability cost for their specific risk profile.",
                Tags = ["windows-hello", "pin-expiry", "rotation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ExpirationDays", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExpirationDays")],
                DetectOps = [RegOp.CheckDword(Key, "ExpirationDays", 60)],
            },
            new TweakDef
            {
                Id = "helloadv-block-simple-pins",
                Label = "Block Simple or Sequential PINs for Windows Hello Authentication",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Simple PINs like 1234, 0000, or sequential digit patterns are the Hello equivalent of weak passwords that are trivially guessable by observers or through systematic guessing. Blocking simple PINs enforces PIN complexity requirements that prevent sequentially or repeatedly patterned digit strings. Windows Hello PIN complexity can require the prevention of consecutive and repeating digits to ensure PINs have sufficient entropy for security. Organizations should configure PIN complexity requirements that prevent the top 100 most common PINs used according to security research on password patterns. The combination of PIN length requirements and simple PIN blocking creates a meaningful security baseline for Hello authentication. Users should be educated about choosing unpredictable PINs and informed that PINs should not use dates or memorable number patterns that could be guessed by social engineering.",
                Tags = ["windows-hello", "pin-complexity", "simple-pins", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Digits", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "Digits")],
                DetectOps = [RegOp.CheckDword(Key, "Digits", 2)],
            },
            new TweakDef
            {
                Id = "helloadv-enable-remote-unlock",
                Label = "Enable Remote Unlock Capability for Windows Hello Registered Devices",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows Hello remote unlock uses proximity-based authentication from a companion device allowing a locked computer to be unlocked when the user's phone is nearby. Enabling remote unlock provides a more convenient unlock experience for users while maintaining the security properties of Windows Hello authentication. Remote unlock requires the Windows Hello companion app installed on a paired mobile device and uses Bluetooth for proximity detection. Organizations should evaluate the security implications of remote unlock in their environment considering whether phone-based proximity is appropriate for their physical security controls. Remote unlock can be beneficial for users who work across multiple devices and need to frequently lock and unlock their workstations. The security of remote unlock depends on the security of the paired mobile device so mobile device management policies must be strong enough to protect this authentication factor.",
                Tags = ["windows-hello", "remote-unlock", "companion-device", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableUnlockFromPhone", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableUnlockFromPhone")],
                DetectOps = [RegOp.CheckDword(Key, "EnableUnlockFromPhone", 1)],
            },
            new TweakDef
            {
                Id = "helloadv-require-enhanced-anti-spoofing",
                Label = "Require Enhanced Anti-Spoofing for Windows Hello Facial Recognition",
                Category = "User Account — Passwordless Sign In",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enhanced anti-spoofing requires IR camera hardware with depth sensing capabilities that can distinguish a real face from a photograph video or 3D mask presentation attack. Requiring enhanced anti-spoofing for Windows Hello facial recognition ensures that biometric authentication cannot be bypassed using a photograph of the user. Basic facial recognition without anti-spoofing can be defeated by holding a photograph in front of the camera making enhanced anti-spoofing critical for high-security environments. Organizations deploying Windows Hello with facial recognition should verify that enrolled devices have IR cameras that meet the enhanced anti-spoofing specifications before enabling the feature. Enhanced anti-spoofing is the default on devices certified for Windows Hello facial recognition but should be explicitly required for devices that may support facial recognition without certified hardware. Anti-spoofing requirements protect against the most common attack vectors for biometric authentication including photographs and video attacks.",
                Tags = ["windows-hello", "anti-spoofing", "biometric", "facial-recognition", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnhancedAntiSpoofingForFacialFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnhancedAntiSpoofingForFacialFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "EnhancedAntiSpoofingForFacialFeatures", 1)],
            },
        ];
    }

    // ── WorkplaceJoinPolicy ──
    private static class _WorkplaceJoinPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpjoin-disable-auto",
                    Label = "Disable Automatic Workplace Join",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents devices from automatically joining the workplace (Azure AD or on-prem Workplace Join). Requires explicit administrator action to register. Default: 1 (auto). Recommended: 0 (manual only) for managed environments.",
                    Tags = ["workplace-join", "azure-ad", "device-registration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Prevents unintended device registration; IT must manually register devices.",
                    ApplyOps = [RegOp.SetDword(Key, "autoWorkplaceJoin", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "autoWorkplaceJoin")],
                    DetectOps = [RegOp.CheckDword(Key, "autoWorkplaceJoin", 0)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-tls",
                    Label = "Require TLS for Workplace Join",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Requires Transport Layer Security (TLS/HTTPS) for all Workplace Join registration traffic. Prevents downgrade to unencrypted registration. Default: not enforced. Recommended: 1.",
                    Tags = ["workplace-join", "tls", "encryption", "transport", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Ensures device registration credentials transit over encrypted channels only.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTLS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTLS")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTLS", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-integrity-check",
                    Label = "Require Device Integrity Check Before Join",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Requires a device integrity check (TPM attestation or health attestation) before allowing Workplace Join registration. Prevents compromised devices from registering. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "integrity", "attestation", "tpm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Devices without a TPM or failing health attestation cannot join; increases security posture.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireDeviceIntegrityCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceIntegrityCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireDeviceIntegrityCheck", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-consent-ui",
                    Label = "Require User Consent for Device Registration",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Presents the user with a consent dialog before the device is registered in the workplace. Prevents silent registration without user awareness. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "consent", "user", "registration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Users must acknowledge device registration; prevents silent cloud enrolment.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireConsentForJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireConsentForJoin")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireConsentForJoin", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-disable-silent-reg",
                    Label = "Disable Silent Device Registration",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents the device from silently registering itself with Azure AD or on-prem directory services without user interaction. Default: 1 (allow silent). Recommended: 0.",
                    Tags = ["workplace-join", "silent", "registration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All device registrations are visible to and require action from the user.",
                    ApplyOps = [RegOp.SetDword(Key, "SilentDeviceRegistration", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SilentDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "SilentDeviceRegistration", 0)],
                },
                new TweakDef
                {
                    Id = "wpjoin-limit-max-device-count",
                    Label = "Limit Workplace-Joined Device Count Per User",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Sets the maximum number of devices a user can register in the workplace. Limits lateral spread of identities across many devices. Default: not set. Recommended: 3–5.",
                    Tags = ["workplace-join", "device-limit", "quota", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Limits per-user device registration count; users exceeding the limit cannot join new devices.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxDeviceAllowedCount", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxDeviceAllowedCount")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxDeviceAllowedCount", 5)],
                },
                new TweakDef
                {
                    Id = "wpjoin-enable-join-audit",
                    Label = "Enable Workplace Join Audit Logging",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Enables detailed audit logging of all Workplace Join registration and de-registration events. Captures device identity, user, and timestamp for compliance. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "audit", "logging", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Creates detailed registration event logs; no performance impact on normal operations.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditAADJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditAADJoin")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditAADJoin", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-block-non-compliant",
                    Label = "Block Non-Compliant Device Join",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Prevents devices that fail compliance checks from completing Workplace Join registration. Works with Intune or SCCM compliance policies. Default: 0. Recommended: 1 in managed environments.",
                    Tags = ["workplace-join", "compliance", "mdm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Non-compliant devices (no antivirus, outdated OS) cannot register; use with MDM compliance policies.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockNonCompliantDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockNonCompliantDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockNonCompliantDevice", 1)],
                },
                new TweakDef
                {
                    Id = "wpjoin-require-secure-channel",
                    Label = "Require Secure Channel for Workplace Join",
                    Category = "User Account — Passwordless Sign In",
                    Description =
                        "Requires an established and authenticated secure channel before allowing Workplace Join. Prevents join attempts over untrusted or ad hoc network connections. Default: 0. Recommended: 1.",
                    Tags = ["workplace-join", "secure-channel", "authentication", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Workplace Join is blocked unless an authenticated network channel (VPN or corporate LAN) is present.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSecureChannel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSecureChannel")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSecureChannel", 1)],
                },
            ];
    }
}
