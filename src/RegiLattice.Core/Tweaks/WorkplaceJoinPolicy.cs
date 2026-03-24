// RegiLattice.Core — Tweaks/WorkplaceJoinPolicy.cs
// Workplace Join and Azure AD device registration GPO controls — Sprint 198.
// Controls hybrid Azure AD join, workplace registration, and device compliance enforcement.
// Category: "Workplace Join Policy" | Slug: wpjoin
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WorkplaceJoinPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wpjoin-disable-auto",
                Label = "Disable Automatic Workplace Join",
                Category = "Workplace Join Policy",
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
                Id = "wpjoin-block-aad",
                Label = "Block Azure AD Workplace Join",
                Category = "Workplace Join Policy",
                Description =
                    "Prevents users from performing Azure AD Workplace Join on the device. Useful in air-gapped environments or where cloud synchronisation is not permitted. Default: 0. Recommended: 1 for offline/air-gapped networks.",
                Tags = ["workplace-join", "azure-ad", "block", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks AAD join; device cannot register cloud identity. May affect Intune enrolment.",
                ApplyOps = [RegOp.SetDword(Key, "BlockAADWorkplaceJoin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAADWorkplaceJoin")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAADWorkplaceJoin", 1)],
            },
            new TweakDef
            {
                Id = "wpjoin-require-tls",
                Label = "Require TLS for Workplace Join",
                Category = "Workplace Join Policy",
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
                Category = "Workplace Join Policy",
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
                Category = "Workplace Join Policy",
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
                Category = "Workplace Join Policy",
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
                Category = "Workplace Join Policy",
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
                Category = "Workplace Join Policy",
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
                Category = "Workplace Join Policy",
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
                Category = "Workplace Join Policy",
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
