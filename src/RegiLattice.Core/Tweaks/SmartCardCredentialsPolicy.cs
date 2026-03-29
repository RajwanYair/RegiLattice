// RegiLattice.Core — Tweaks/SmartCardCredentialsPolicy.cs
// Smart Card Credential Provider and Logon Policy — Sprint 631.
// Category: "Smart Card Credentials Policy" | Slug: sccredpol
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\System (smart card system settings)

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmartCardCredentialsPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";
    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "sccredpol-allow-certificates-with-no-extended-key-usage",
            Label = "SC Credentials: Allow Smart Card Certificates Without Extended Key Usage for Logon",
            Category = "Smart Card Credentials Policy",
            Description = "Sets AllowCertificatesWithNoEKU=0 in Smart Card Credential Provider policy. Prevents smart card certificates without an Extended Key Usage (EKU) extension — or with an EKU that doesn't include Client Authentication (1.3.6.1.5.5.7.3.2) — from being used for Windows logon. " +
                "Smart card certificates without an EKU or with an all-inclusive EKU (Any Purpose) are certificates that were issued without specifying a legitimate use constraint. Such certificates are typically misconfigured CA root certificates or test certificates. If Windows allows logon with any certificate present on a smart card regardless of EKU, an attacker who compromises a user's smart card PIN and inserts a root CA certificate or code signing certificate into the card can attempt logon with the inappropriate certificate. Requiring Client Authentication EKU ensures only purpose-constrained logon certificates can authenticate to interactive sessions.",
            Tags = ["sccredpol", "smart-card", "eku", "certificate", "logon", "client-auth"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Smart card certificates must have Client Authentication EKU for interactive logon. Misconfigured test certs or CA-root certs cannot authenticate.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCertificatesWithNoEKU", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCertificatesWithNoEKU")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCertificatesWithNoEKU", 0)],
        },
        new TweakDef
        {
            Id = "sccredpol-enforce-certificate-time-validity",
            Label = "SC Credentials: Reject Expired Smart Card Certificates from Logon",
            Category = "Smart Card Credentials Policy",
            Description = "Sets EnforceCAExpiry=1 in Smart Card Credential Provider policy. Enforces certificate validity period checking — prevents Windows from accepting smart card certificates for logon that have expired or whose issuing CA certificate chain has expired. By default, Windows may allow logon with expired smart card certificates in some scenarios (offline cached logon) if the certificate was previously valid. " +
                "Expired certificates represent an operational risk in smart card deployments: when a user's smart card certificate expires but the card PIN remains valid, Windows may continue to accept the card for domain logon relying on cached credentials — even though the PKI infrastructure considers the certificate expired. An attacker who obtains an expired certificate and the corresponding private key (from a compromised card) can attempt offline certificate logon. EnforceCAExpiry=1 ensures the current certificate validity timestamp is always checked, preventing expired certificate acceptance even in cached credential scenarios.",
            Tags = ["sccredpol", "smart-card", "certificate-expiry", "ca-expiry", "validity", "pki"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Expired smart card certificates rejected. Users with expired certificates must renew before interactive logon works. Ensure certificate renewal reminders are in place.",
            ApplyOps = [RegOp.SetDword(Key, "EnforceCAExpiry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceCAExpiry")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceCAExpiry", 1)],
        },
        new TweakDef
        {
            Id = "sccredpol-filter-duplicate-certificates",
            Label = "SC Credentials: Filter Duplicate Smart Card Certificates Shown in Logon Picker",
            Category = "Smart Card Credentials Policy",
            Description = "Sets FilterDuplicateCerts=1 in Smart Card Credential Provider policy. When a smart card contains multiple certificates with the same Subject and public key (e.g., during certificate renewal where both old and new certificates co-exist on the card), this setting shows only the most recently issued certificate in the Windows logon certificate picker, preventing user confusion from duplicate entries. " +
                "During smart card certificate lifecycle management, cards frequently transition through a state where both the old (near-expired) and new (freshly issued) certificates are on the card simultaneously — to allow the renewal to proceed without requiring the user to surrender their card. The Windows logon certificate picker displays all certificates, presenting two identical-looking entries to the user. Users who select the expired certificate will experience logon failures. FilterDuplicateCerts reduces the duplicate entries to one (the most recent), eliminating this user experience issue.",
            Tags = ["sccredpol", "smart-card", "duplicate-certificate", "certificate-renewal", "logon-picker"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Duplicate smart card certificates filtered in logon picker. Only most recently issued certificate shown when multiple share the same subject.",
            ApplyOps = [RegOp.SetDword(Key, "FilterDuplicateCerts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "FilterDuplicateCerts")],
            DetectOps = [RegOp.CheckDword(Key, "FilterDuplicateCerts", 1)],
        },
        new TweakDef
        {
            Id = "sccredpol-force-read-all-certificates",
            Label = "SC Credentials: Force Reading All Certificates from Smart Card (Not Just Root/Signing)",
            Category = "Smart Card Credentials Policy",
            Description = "Sets ForceReadingAllCertificates=1 in Smart Card Credential Provider policy. Forces Windows to read all certificates stored on the smart card during authentication enumeration, rather than only examining the first matching certificate. Some cards store certificate-based logon credentials on non-default slots or with non-standard EKU ordering — without ForceReadingAllCertificates, Windows may skip valid authentication certificates. " +
                "Smart card credential providers have an optimisation that stops scanning the card after finding the first usable certificate. On cards with multiple valid Client Authentication certificates (multi-profile cards, cards issued by different CAs for different resource domains), the optimisation may select a certificate for a different trust domain, causing failed authentication. ForceReadingAllCertificates ensures the complete certificate set is enumerated and the credential provider selects the certificate with the best chain match for the current domain.",
            Tags = ["sccredpol", "smart-card", "certificate-enumeration", "multi-profile", "credential-provider"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "All smart card certificates read and enumerated. Slight performance increase per logon attempt; negligible on modern smart card readers.",
            ApplyOps = [RegOp.SetDword(Key, "ForceReadingAllCertificates", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceReadingAllCertificates")],
            DetectOps = [RegOp.CheckDword(Key, "ForceReadingAllCertificates", 1)],
        },
        new TweakDef
        {
            Id = "sccredpol-require-smart-card-for-logon",
            Label = "SC Credentials: Require Smart Card for Interactive Logon (Disable Password-Based Logon)",
            Category = "Smart Card Credentials Policy",
            Description = "Sets ScForceOption=1 in Windows System policy. Requires users to authenticate with a smart card for interactive (local and Remote Desktop) logon. Password-based interactive logon is disabled. This setting is the full enforcement of a smart card-mandatory authentication policy — ensuring that physical possession of the smart card is required for every interactive logon event, eliminating password-based bypass paths. " +
                "Password-based logon as a fallback for smart card environments creates a persistent weak authentication path: users who 'lose' their smart card can fall back to passwords, which are substantially easier to steal via phishing or shoulder surfing than compromising a physical authentication token plus PIN. In high-assurance environments (financial trading, government classified systems, nuclear facility IT, PCI DSS Level 1), all interactive logon must be protected by a physical authentication factor. ScForceOption=1 eliminates the password fallback and enforces the physical factor requirement absolutely.",
            Tags = ["sccredpol", "smart-card", "force-logon", "disable-password-logon", "mfa", "high-assurance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 2,
            ImpactNote = "BREAKING: Password interactive logon fully disabled. Smart card REQUIRED for all logon. Ensure all users have working smart cards and readers before deployment. Service accounts need smartcard exemption.",
            ApplyOps = [RegOp.SetDword(SysKey, "ScForceOption", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "ScForceOption")],
            DetectOps = [RegOp.CheckDword(SysKey, "ScForceOption", 1)],
        },
        new TweakDef
        {
            Id = "sccredpol-enable-smart-card-lock-on-removal",
            Label = "SC Credentials: Lock Workstation Automatically When Smart Card is Removed",
            Category = "Smart Card Credentials Policy",
            Description = "Sets SmartCardRemovalOption=1 in Windows System policy. Automatically locks the workstation when the user removes their smart card from the reader, replacing the 'no action' default. Ensures the workstation is immediately locked when the user physically departs (smart card is typically in their lanyard or pocket which they take with them). " +
                "Smart card removal detection is a behavioural lock triggered by physical possession of the authentication token. The security premise: a person who removes their smart card from the reader is physically leaving the workstation. Without removal lock, the authenticated session remains unlocked and accessible to anyone who approaches the workstation during the user's brief absence (printer, coffee, restroom). SmartCardRemovalOption=1 means the session locks within seconds of card removal — the physical authentication token acts as a proximity-based session lock device.",
            Tags = ["sccredpol", "smart-card", "removal-lock", "session-lock", "physical-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Workstation locks immediately on smart card removal. Users who briefly remove their card for any reason will need to re-insert and re-authenticate.",
            ApplyOps = [RegOp.SetDword(SysKey, "SmartCardRemovalOption", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "SmartCardRemovalOption")],
            DetectOps = [RegOp.CheckDword(SysKey, "SmartCardRemovalOption", 1)],
        },
        new TweakDef
        {
            Id = "sccredpol-disable-smart-card-credential-caching",
            Label = "SC Credentials: Disable Windows Cached Credentials for Smart Card Logons",
            Category = "Smart Card Credentials Policy",
            Description = "Sets DisableSmartCardLogonCheck=0 in Smart Card Credential Provider policy. Ensures Windows performs a full smart card authentication challenge on every logon attempt — disabling any cached credential shortcut paths that might allow logon without re-validating the current smart card state against the DC. " +
                "Cached credential logon for smart card authentication creates an inconsistency: the cached domain credential may be valid even after the smart card certificate has been revoked (e.g., following employee termination or card loss). If Windows allows cached credential logon for smart card sessions, a terminated employee's workstation retains the logon capability for up to the domain cache lifetime (default 10 cached logons). Ensuring full smart card validation on each logon forces certificate revocation to be effective immediately — revoked smart cards are rejected on first logon attempt after CRL update.",
            Tags = ["sccredpol", "smart-card", "credential-cache", "revocation", "crl", "terminated-employee"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Full smart card DC validation required. Offline logon (no DC reachable) requires network connectivity. Deploy alongside always-on VPN for remote workers.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSmartCardLogonCheck", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartCardLogonCheck")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSmartCardLogonCheck", 0)],
        },
        new TweakDef
        {
            Id = "sccredpol-enable-smart-card-puk-logging",
            Label = "SC Credentials: Enable Smart Card PUK/PIN Operation Logging",
            Category = "Smart Card Credentials Policy",
            Description = "Sets EnableSmartCardLogonLogging=1 in Smart Card Credential Provider policy. Enables logging of smart card PIN entry events, PUK (PIN Unblocking Key) operations, and certificate selection events to the Windows Application event log. PIN operation logging provides an audit trail of smart card authentication activity at the workstation — enabling detection of PIN brute-force attempts (excessive failed PIN entries), card blocking events (PUK operation triggered), and certificate selection anomalies. " +
                "Smart card PIN brute-force attacks are rate-limited by card hardware (typically 3-10 failed attempts before card lockout), but without logging, an attacker who attempts multiple combinations across the threshold boundary and reinserts the card leaves no system event trace. Smart card logging events can be collected by SIEM, enabling detection of cards that are being tested for PIN guessing (rapid sequence of failed PIN events at an unexpected workstation), identifying potentially compromised or stolen cards before the card locks.",
            Tags = ["sccredpol", "smart-card", "logging", "pin-brute-force", "puk", "audit"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Smart card PIN and PUK events logged to Application event log. SIEM collection of card-specific events enables PIN brute-force detection.",
            ApplyOps = [RegOp.SetDword(Key, "EnableSmartCardLogonLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSmartCardLogonLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSmartCardLogonLogging", 1)],
        },
        new TweakDef
        {
            Id = "sccredpol-restrict-to-root-trusted-certificates",
            Label = "SC Credentials: Restrict Smart Card Logon to Root-CA Trusted Certificates Only",
            Category = "Smart Card Credentials Policy",
            Description = "Sets RootCA=1 in Smart Card Credential Provider policy. Restricts smart card logon to only accept certificates that chain to a root CA in the machine's Trusted Root Certification Authorities store — preventing certificates issued by intermediate-only CAs or enterprise subordinate CAs whose root is not in the machine trust store from being used for logon. " +
                "In multi-forest or partner organisation environments, smart cards issued by external PKI hierarchies may be physically interoperable (same card form factor, compatible reader drivers) but should not grant logon access to the local domain unless their issuing CA root is explicitly trusted. Without RootCA=1, certificates from any technically valid PKI chain — including self-signed certificates added to a card by an attacker — could be used for logon. Restricting to root-CA-trusted certs ensures the local domain trust policy governs which PKI hierarchies are authorised for smart card authentication.",
            Tags = ["sccredpol", "smart-card", "root-ca", "trust", "pki", "cross-forest"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Smart card certs must chain to machine trust store root CA. Self-signed and untrusted-root certificates rejected for logon.",
            ApplyOps = [RegOp.SetDword(Key, "RootCA", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RootCA")],
            DetectOps = [RegOp.CheckDword(Key, "RootCA", 1)],
        },
        new TweakDef
        {
            Id = "sccredpol-enable-integrated-unblock",
            Label = "SC Credentials: Enable Integrated Smart Card Unblock Screen at Logon",
            Category = "Smart Card Credentials Policy",
            Description = "Sets EnableIntegratedUnblock=1 in Smart Card Credential Provider policy. Enables the Windows integrated smart card unblock screen — presented at the Ctrl+Alt+Del logon screen when a smart card's PIN is blocked (after exceeding the incorrect PIN attempt limit). The integrated unblock screen allows users to unblock their card at the logon screen using PUK without requiring a separate unblock tool or helpdesk intervention. " +
                "Without integrated unblock, a user whose card PIN is blocked must call the IT helpdesk, be issued a temporary PUK, and use a separate smart card management utility to unblock the card. This process typically takes 15–60 minutes depending on helpdesk availability. The integrated unblock screen presents the PUK entry interface directly at the Windows logon screen — the user provides their PUK and new PIN, the card is immediately unblocked, and logon proceeds. EnableIntegratedUnblock reduces helpdesk call volume for card lockouts by eliminating the manual unblock workflow.",
            Tags = ["sccredpol", "smart-card", "unblock", "puk", "helpdesk", "user-experience"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Smart card unblock screen shown at Windows logon when PIN is blocked. Users can self-service PUK entry. Reduces helpdesk calls for locked cards.",
            ApplyOps = [RegOp.SetDword(Key, "EnableIntegratedUnblock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegratedUnblock")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIntegratedUnblock", 1)],
        },
    ];
}
