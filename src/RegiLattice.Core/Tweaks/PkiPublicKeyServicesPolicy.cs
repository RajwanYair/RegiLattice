// RegiLattice.Core — Tweaks/PkiPublicKeyServicesPolicy.cs
// PKI Public Key Services Policy — Sprint 549.
// Configures Group Policy for Windows PKI services: certificate auto-enrollment,
// smart card certificate mapping, PKI credential provider visibility, chain
// validation hardening, and key archival settings.
// Category: "PKI Public Key Services Policy" | Slug: pki
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\Cryptography\AutoEnrollment

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PkiPublicKeyServicesPolicy
{
    private const string SmartCardKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SmartCardCredentialProvider";

    private const string AutoEnrollKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Cryptography\AutoEnrollment";

    private const string PkiKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PKI";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "pki-enable-certificate-auto-enrollment",
                Label = "PKI: Enable Certificate Auto-Enrollment from Enterprise CA",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets AEPolicy=7 in AutoEnrollment policy (value = AUTOENROLLMENT_ENABLED | UPDATE_PENDING | ENROLL_ON_BEHALF_OF). Enables automatic certificate enrollment from an enterprise CA via Active Directory Certificate Services. Workstations request and renew certificates without user interaction: machine authentication certificates, user signing certificates, and EFS keys are automatically provisioned to domain-joined machines according to certificate templates published in the AD CA. Essential for large-scale PKI deployments.",
                Tags = ["pki", "auto-enrollment", "certificate", "active-directory", "ca"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Auto-enrollment requires an enterprise CA with published certificate templates. Machines silently enroll for configured templates. No impact if no enterprise CA or templates are configured.",
                ApplyOps = [RegOp.SetDword(AutoEnrollKey, "AEPolicy", 7)],
                RemoveOps = [RegOp.DeleteValue(AutoEnrollKey, "AEPolicy")],
                DetectOps = [RegOp.CheckDword(AutoEnrollKey, "AEPolicy", 7)],
            },
            new TweakDef
            {
                Id = "pki-disable-smartcard-pin-recovery",
                Label = "PKI: Disable Smart Card PIN Recovery Mode",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets DisablePINRecovery=1 in SmartCardCredentialProvider policy. Prevents smart card PIN recovery mechanisms that allow an administrator or escrowed key to bypass Smart Card PIN verification. PIN recovery is a usability feature but it weakens the two-factor authentication model of smart cards: if the PIN can be recovered or bypassed administratively, the authentication factor is reduced from 'something you have + something you know' to effectively 'something you have + a password held by IT'. Disabling PIN recovery enforces the full 2FA model.",
                Tags = ["pki", "smart-card", "pin", "2fa", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Smart card PIN recovery is disabled. Forgotten PINs require re-issuance of the smart card. Ensure lifecycle processes (lost card, forgotten PIN) are documented for users.",
                ApplyOps = [RegOp.SetDword(SmartCardKey, "DisablePINRecovery", 1)],
                RemoveOps = [RegOp.DeleteValue(SmartCardKey, "DisablePINRecovery")],
                DetectOps = [RegOp.CheckDword(SmartCardKey, "DisablePINRecovery", 1)],
            },
            new TweakDef
            {
                Id = "pki-enable-reverse-subject-name",
                Label = "PKI: Enable Reversal of Encoded Subject Name in Certificate UI",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets ReverseSubject=1 in PKI policy. Changes the display order of certificate subject Distinguished Name components in the certificate viewer UI from ASN.1-encoded reverse order (dc=net, dc=contoso, cn=Users, cn=JaneExample) to the more intuitive forward-reading order (cn=JaneExample, cn=Users, dc=contoso, dc=net). This purely cosmetic change makes it easier for users and helpdesk staff to verify certificate identity fields without understanding ASN.1 DER encoding conventions.",
                Tags = ["pki", "certificate", "display", "subject-name", "ui"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Certificate subject names in UI dialogs display in human-readable order. No functional impact — purely cosmetic change in certificate display.",
                ApplyOps = [RegOp.SetDword(PkiKey, "ReverseSubject", 1)],
                RemoveOps = [RegOp.DeleteValue(PkiKey, "ReverseSubject")],
                DetectOps = [RegOp.CheckDword(PkiKey, "ReverseSubject", 1)],
            },
            new TweakDef
            {
                Id = "pki-force-logon-smartcard",
                Label = "PKI: Require Smart Card for Interactive Logon",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets ScForceOption=1 in SmartCardCredentialProvider policy. Requires that all interactive logon sessions use a smart card for authentication. When this setting is active, the username/password credential provider is hidden and only the smart card credential provider is visible at the logon screen and UAC prompts. This enforces hardware-backed two-factor authentication for all interactive access: physical smart card (something you have) + PIN (something you know). Cannot be bypassed by users.",
                Tags = ["pki", "smart-card", "logon", "2fa", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Smart card is required for all interactive logon. Password logon is hidden. ALL administrators must have a working smart card before enabling — lockout risk if smart card infrastructure fails.",
                ApplyOps = [RegOp.SetDword(SmartCardKey, "ScForceOption", 1)],
                RemoveOps = [RegOp.DeleteValue(SmartCardKey, "ScForceOption")],
                DetectOps = [RegOp.CheckDword(SmartCardKey, "ScForceOption", 1)],
            },
            new TweakDef
            {
                Id = "pki-enable-cert-prop-to-user-store",
                Label = "PKI: Enable Certificate Propagation from Smart Card to User Store",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets EnableCertPropagation=1 in SmartCardCredentialProvider policy. Activates the Windows Certificate Propagation Service which copies certificates from an inserted smart card into the user's personal certificate store (Cert:\\CurrentUser\\My). Applications that enumerate the user certificate store (email clients for S/MIME, VPN clients, code signing tools) can then find the smart card certificate without requiring explicit application-level smart card support. Certificates are removed from the store when the card is removed.",
                Tags = ["pki", "smart-card", "certificate", "propagation", "store"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Smart card certificates are copied to user store on card insertion. Required for many applications that read from the certificate store rather than directly querying the smart card.",
                ApplyOps = [RegOp.SetDword(SmartCardKey, "EnableCertPropagation", 1)],
                RemoveOps = [RegOp.DeleteValue(SmartCardKey, "EnableCertPropagation")],
                DetectOps = [RegOp.CheckDword(SmartCardKey, "EnableCertPropagation", 1)],
            },
            new TweakDef
            {
                Id = "pki-enable-root-cert-update",
                Label = "PKI: Allow Enterprise Trusted Root Certificate Updates via GP",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets EnablePKIUpdates=1 in PKI policy. Allows the Windows PKI infrastructure to process and install enterprise root and intermediate CA certificates that are distributed via the NTAuth certificate store in Active Directory and via Group Policy Objects. Required for domain-joined machines to automatically receive internally issued CA certificate updates. Without this, machines require manual certificate installations when the enterprise CA hierarchy changes (new intermediate, renewed root, distrusted CA).",
                Tags = ["pki", "certificate", "root-ca", "group-policy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enterprise CA certificates are automatically propagated from Active Directory. Required for enterprise certificate-based authentication (802.1x, SSTP VPN, IPsec).",
                ApplyOps = [RegOp.SetDword(PkiKey, "EnablePKIUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(PkiKey, "EnablePKIUpdates")],
                DetectOps = [RegOp.CheckDword(PkiKey, "EnablePKIUpdates", 1)],
            },
            new TweakDef
            {
                Id = "pki-enable-pin-change-on-logon",
                Label = "PKI: Enable Smart Card PIN Change Option at Logon",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets AllowPINChangeAtLogon=1 in SmartCardCredentialProvider. Presents the 'Change PIN' option in the Windows Security screen (Ctrl+Alt+Del) for smart card users, allowing them to update their smart card PIN through the Windows credential interface. Without this option, users must use vendor-specific middleware or management tools to change PINs. Providing PIN change through the familiar Windows interface reduces friction for PIN management, encouraging regular PIN rotation.",
                Tags = ["pki", "smart-card", "pin", "change", "usability"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enables PIN change option in Windows security screen. Purely usability improvement — no security impact. Requires compatible smart card middleware.",
                ApplyOps = [RegOp.SetDword(SmartCardKey, "AllowPINChangeAtLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(SmartCardKey, "AllowPINChangeAtLogon")],
                DetectOps = [RegOp.CheckDword(SmartCardKey, "AllowPINChangeAtLogon", 1)],
            },
            new TweakDef
            {
                Id = "pki-disable-smartcard-logon-no-dirsvc",
                Label = "PKI: Disable Smart Card Logon Without Active Directory Service",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets AllowSmartCardWithoutDirectoryService=0 in SmartCardCredentialProvider policy. Prevents smart card logon when Active Directory is not reachable. In hybrid or cached-credential scenarios, Windows can sometimes allow smart card logon using locally cached credentials even when the AD DC is unavailable. Disabling this prevents smart card authentication from falling back to cached credentials. Ensures all smart card authentications are validated against a live domain controller — preventing stale credential-based access.",
                Tags = ["pki", "smart-card", "active-directory", "logon", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Smart card logon fails when AD DC is unreachable. Off-network users (VPN users) must be able to reach a DC before authenticating. Do not enable for laptops that frequently work disconnected from the network.",
                ApplyOps =
                [
                    RegOp.SetDword(SmartCardKey, "AllowSmartCardWithoutDirectoryService", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(SmartCardKey, "AllowSmartCardWithoutDirectoryService"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(SmartCardKey, "AllowSmartCardWithoutDirectoryService", 0),
                ],
            },
            new TweakDef
            {
                Id = "pki-enable-cert-transparency-log",
                Label = "PKI: Enable Certificate Transparency Log Validation",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets EnableCTLog=1 in PKI policy. Enables validation against Certificate Transparency (RFC 9162) logs when verifying TLS server certificates. Certificate Transparency is a public audit mechanism: all publicly trusted CAs are required to submit issued certificates to public CT logs, allowing domain owners to detect mis-issued certificates within hours. When CT validation is enabled, Windows Schannel verifies that a TLS certificate has Signed Certificate Timestamp (SCT) extensions proving inclusion in a CT log.",
                Tags = ["pki", "certificate", "transparency", "ct-log", "auditing"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "TLS certificates without SCTs are rejected. Public CAs include SCTs since 2022. Internal CA certificates may lack SCTs — configure CT whitelist for internal domains.",
                ApplyOps = [RegOp.SetDword(PkiKey, "EnableCTLog", 1)],
                RemoveOps = [RegOp.DeleteValue(PkiKey, "EnableCTLog")],
                DetectOps = [RegOp.CheckDword(PkiKey, "EnableCTLog", 1)],
            },
            new TweakDef
            {
                Id = "pki-enable-eku-filtering",
                Label = "PKI: Enable Enhanced Key Usage Filtering in Certificate Validation",
                Category = "PKI Public Key Services Policy",
                Description =
                    "Sets EKUFiltering=1 in PKI policy. Enables strict Extended Key Usage (EKU) filtering during certificate path validation. The EKU extension in a certificate restricts the cryptographic operations for which the certificate is valid (e.g., serverAuthentication, clientAuthentication, codeSigning, emailProtection). Without EKU filtering, a client authentication certificate could theoretically be misused for server authentication or code signing. EKU filtering enforces the certificate's intended use constraints.",
                Tags = ["pki", "eku", "certificate", "key-usage", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Certificate EKU constraints are strictly enforced. Certificates issued without an EKU for their intended use are rejected. Audit certificate templates to ensure correct EKU assignments.",
                ApplyOps = [RegOp.SetDword(PkiKey, "EKUFiltering", 1)],
                RemoveOps = [RegOp.DeleteValue(PkiKey, "EKUFiltering")],
                DetectOps = [RegOp.CheckDword(PkiKey, "EKUFiltering", 1)],
            },
        ];
}
