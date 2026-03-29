// RegiLattice.Core — Tweaks/CredentialRoamingPolicy.cs
// Credential roaming, roaming profile sync, certificate roaming, and cross-device credential security — Sprint 530 (replacement).
// Category: "Credential Roaming Policy" | Slug: credroam
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CredentialRoamingPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon";
    private const string RoamKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Winlogon\RoamingProfile";
    private const string CertKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\RoamingProfile";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "credroam-disable-credential-roaming",
            Label        = "Disable User Credential Roaming Between Domain Computers",
            Category     = "Credential Roaming Policy",
            Description  = "Prevents user credentials (certificates, private keys, smart card PINs) from being copied to the user's roaming profile and thus synchronised to other domain computers, keeping credentials machine-local and reducing the credential surface exposed if a profile is compromised.",
            Tags         = ["credential-roaming", "certificates", "private-keys", "roaming-profile", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Credential roaming disabled; private keys and certificates stay machine-local, not synced via roaming profile.",
            ApplyOps     = [RegOp.SetDword(Key, "SyncForegroundPolicy", 0)],
            RemoveOps    = [RegOp.DeleteValue(Key, "SyncForegroundPolicy")],
            DetectOps    = [RegOp.CheckDword(Key, "SyncForegroundPolicy", 0)],
        },
        new TweakDef
        {
            Id           = "credroam-block-certificate-roaming",
            Label        = "Block Roaming of User Certificates via User Profile",
            Category     = "Credential Roaming Policy",
            Description  = "Specifically blocks the roaming of user certificates and key containers via the Windows credential roaming feature, preventing certificates imported on one machine from appearing on all machines on next logon.",
            Tags         = ["credential-roaming", "certificates", "key-container", "profile-sync", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Certificate roaming via user profile blocked; imported certs and keys remain on the issuing machine only.",
            ApplyOps     = [RegOp.SetDword(CertKey, "DisableCertificateRoaming", 1)],
            RemoveOps    = [RegOp.DeleteValue(CertKey, "DisableCertificateRoaming")],
            DetectOps    = [RegOp.CheckDword(CertKey, "DisableCertificateRoaming", 1)],
        },
        new TweakDef
        {
            Id           = "credroam-restrict-profile-sync-to-domain",
            Label        = "Restrict Roaming Profile Sync to Domain Networks Only",
            Category     = "Credential Roaming Policy",
            Description  = "Prevents roaming profile synchronisation from occurring over non-domain networks (public WiFi, VPN), ensuring credential and profile data is only synced when connected to the corporate domain network.",
            Tags         = ["roaming-profile", "domain-network", "profile-sync", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Roaming profile sync restricted to domain networks; profile data not synced over public networks or VPN.",
            ApplyOps     = [RegOp.SetDword(RoamKey, "SlowLinkTimeOut", 500)],
            RemoveOps    = [RegOp.DeleteValue(RoamKey, "SlowLinkTimeOut")],
            DetectOps    = [RegOp.CheckDword(RoamKey, "SlowLinkTimeOut", 500)],
        },
        new TweakDef
        {
            Id           = "credroam-delete-cached-roaming-profiles",
            Label        = "Delete Cached Copies of Roaming Profiles at Logoff",
            Category     = "Credential Roaming Policy",
            Description  = "Configures Windows to delete the locally cached copy of the roaming profile when the user logs off, ensuring credential data and profile contents are not left on shared or non-primary workstations after user sessions.",
            Tags         = ["roaming-profile", "cached-profile", "logoff", "data-cleanup", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Cached roaming profile deleted at logoff; credential data not retained on non-primary workstations.",
            ApplyOps     = [RegOp.SetDword(Key, "DeleteRoamingCache", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DeleteRoamingCache")],
            DetectOps    = [RegOp.CheckDword(Key, "DeleteRoamingCache", 1)],
        },
        new TweakDef
        {
            Id           = "credroam-disable-smart-card-pin-roaming",
            Label        = "Disable Smart Card PIN Roaming via Credential Roaming Service",
            Category     = "Credential Roaming Policy",
            Description  = "Prevents smart card PINs cached by the Windows Smart Card PIN cache from being synchronised between machines via the credential roaming service, keeping smart card PIN caches strictly machine-local.",
            Tags         = ["credroam", "smart-card", "pin-cache", "roaming", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Smart card PIN roaming disabled; cached PINs remain machine-local only.",
            ApplyOps     = [RegOp.SetDword(CertKey, "DisableSmartCardPINRoaming", 1)],
            RemoveOps    = [RegOp.DeleteValue(CertKey, "DisableSmartCardPINRoaming")],
            DetectOps    = [RegOp.CheckDword(CertKey, "DisableSmartCardPINRoaming", 1)],
        },
        new TweakDef
        {
            Id           = "credroam-require-admin-roaming-profile",
            Label        = "Block Administrator Accounts from Using Roaming Profiles",
            Category     = "Credential Roaming Policy",
            Description  = "Prevents administrator accounts from using roaming profiles, ensuring that elevated account credentials, SAM keys, and administrative certificates are never synchronised to roaming profile storage.",
            Tags         = ["credroam", "admin-account", "roaming-profile", "privilege", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Admin accounts blocked from roaming profiles; elevated credential data never synced via profile infrastructure.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableAdminRoamingProfile", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableAdminRoamingProfile")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableAdminRoamingProfile", 1)],
        },
        new TweakDef
        {
            Id           = "credroam-encrypt-roaming-profile-at-rest",
            Label        = "Encrypt Roaming Profile Server-Side Copy at Rest",
            Category     = "Credential Roaming Policy",
            Description  = "Requires the roaming profile share to encrypt profile data server-side before writing to the UNC profile path, ensuring that the server-side copy of the roaming profile is EFS-protected and not readable by share administrators.",
            Tags         = ["credroam", "efs", "encryption", "profile-server", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Roaming profile server-side copy EFS-encrypted; share admins cannot read profile credential data at rest.",
            ApplyOps     = [RegOp.SetDword(RoamKey, "EncryptProfileData", 1)],
            RemoveOps    = [RegOp.DeleteValue(RoamKey, "EncryptProfileData")],
            DetectOps    = [RegOp.CheckDword(RoamKey, "EncryptProfileData", 1)],
        },
        new TweakDef
        {
            Id           = "credroam-log-profile-sync-events",
            Label        = "Log Roaming Profile Synchronisation Events in System Log",
            Category     = "Credential Roaming Policy",
            Description  = "Enables System event log entries for all roaming profile synchronisation operations, including sync success, failure, conflict, and truncation events, providing audit visibility into profile and credential roaming activity.",
            Tags         = ["credroam", "event-log", "audit", "profile-sync", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Roaming profile sync events logged in System log; credential roaming activity visible for auditing.",
            ApplyOps     = [RegOp.SetDword(Key, "LogProfileSyncEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LogProfileSyncEvents")],
            DetectOps    = [RegOp.CheckDword(Key, "LogProfileSyncEvents", 1)],
        },
        new TweakDef
        {
            Id           = "credroam-block-plaintext-credential-cache",
            Label        = "Block Caching of Plaintext Credentials in Roaming Profile",
            Category     = "Credential Roaming Policy",
            Description  = "Prevents the credentials manager and credential providers from storing reversible (plaintext-equivalent) credential blobs in the user's roaming profile, ensuring only hashed or certificate-protected credentials are ever written to profile storage.",
            Tags         = ["credroam", "plaintext-credential", "credential-cache", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Plaintext credential caching in roaming profile blocked; only hashed/cert-protected credentials in profile.",
            ApplyOps     = [RegOp.SetDword(CertKey, "BlockPlaintextCredentialCache", 1)],
            RemoveOps    = [RegOp.DeleteValue(CertKey, "BlockPlaintextCredentialCache")],
            DetectOps    = [RegOp.CheckDword(CertKey, "BlockPlaintextCredentialCache", 1)],
        },
        new TweakDef
        {
            Id           = "credroam-disable-credential-roaming-telemetry",
            Label        = "Disable Credential Roaming Telemetry to Microsoft",
            Category     = "Credential Roaming Policy",
            Description  = "Prevents the Windows credential roaming service from sending certificate sync counts, roaming failures, and credential manager sync statistics to Microsoft.",
            Tags         = ["credroam", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 2,
            SafetyRating = 5,
            ImpactNote   = "Credential roaming telemetry to Microsoft disabled; cert sync stats and roaming activity not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(CertKey, "DisableCredentialRoamingTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(CertKey, "DisableCredentialRoamingTelemetry")],
            DetectOps    = [RegOp.CheckDword(CertKey, "DisableCredentialRoamingTelemetry", 1)],
        },
    ];
}
