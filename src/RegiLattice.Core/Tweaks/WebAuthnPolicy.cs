// RegiLattice.Core — Tweaks/WebAuthnPolicy.cs
// Sprint 280: WebAuthn FIDO2 Group Policy (10 tweaks)
// Category: "WebAuthn Policy" | Slug: wauthn
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WebAuthn

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WebAuthnPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WebAuthn";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wauthn-disable-touch-id-fallback",
            Label = "Disable WebAuthn Biometric Fallback to PIN",
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
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
            Category = "WebAuthn Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 4,
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
