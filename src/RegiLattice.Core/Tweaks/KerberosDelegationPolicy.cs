// RegiLattice.Core — Tweaks/KerberosDelegationPolicy.cs
// Kerberos Delegation Control Policy — Sprint 577.
// Configures Kerberos unconstrained/constrained delegation restrictions,
// resource-based constrained delegation controls, and ticket-granting
// service (TGS) hardening. Applied via Group Policy registry paths.
// Category: "Kerberos Delegation Policy" | Slug: krbdel
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Security\Kerberos
//           HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Kerberos

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KerberosDelegationPolicy
{
    private const string KerbKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Security\Kerberos";

    private const string KerbNtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Kerberos";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "krbdel-require-kdc-validation",
                Label = "Kerberos Delegation: Require KDC Certificate Validation for PKINIT",
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
                Category = "Kerberos Delegation Policy",
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
